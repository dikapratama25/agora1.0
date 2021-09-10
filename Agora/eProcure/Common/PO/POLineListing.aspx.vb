Imports AgoraLegacy
Imports eProcure.Component

Public Class POLineListing
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblPONo As System.Web.UI.WebControls.Label
    Protected WithEvents lblOrderDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblLineNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblVItemCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblDesc As System.Web.UI.WebControls.Label
    Protected WithEvents lblRemarks As System.Web.UI.WebControls.Label
    Protected WithEvents lblQty As System.Web.UI.WebControls.Label
    Protected WithEvents lblReceived As System.Web.UI.WebControls.Label
    Protected WithEvents lblUnitCost As System.Web.UI.WebControls.Label
    Protected WithEvents lblRejected As System.Web.UI.WebControls.Label
    Protected WithEvents lblTotRecCost As System.Web.UI.WebControls.Label
    Protected WithEvents lblGST As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_GST As System.Web.UI.WebControls.Label
    Protected WithEvents back As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents lblTotCostGST As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_TotCostGST As System.Web.UI.WebControls.Label
    Protected WithEvents lblCurrency As System.Web.UI.WebControls.Label
    Protected WithEvents lblTotCost As System.Web.UI.WebControls.Label
    Protected WithEvents Lblline As System.Web.UI.WebControls.Label
    Protected WithEvents Lno As System.Web.UI.HtmlControls.HtmlTableRow

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        MyBase.Page_Load(sender, e)
        displayPOLineList()
        '---New Code Added  for Line No by Praveen on 17/07/2007
        Lno.Visible = False
        '-----End the code
    End Sub

    Private Function displayPOLineList()

        Dim objPO As New PurchaseOrder
        Dim objval As New POValue
        Dim objGst As New GST
        Dim val1 As String
        objval.PO_Number = Request(Trim("PO_NO"))
        objval.PO_Line = Request(Trim("po_line"))
        'val1 = Request("po_line")
        objval.buyer_coy = Request(Trim("BCoyID"))
        objPO.get_POLineList(objval)
        '-----New Code Added  for Line No by Praveen on 17/07/2007
        objval.lineval = Request(Trim("lineval"))
        '----------End  The code -----------------

        lblPONo.Text = objval.PO_Number
        'lblOrderDate.Text = objval.PO_Date
        lblOrderDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, objval.PO_Date)

        lblLineNo.Text = objval.PO_Line
        '-----New Code Added  for Line No by Praveen on 17/07/2007
        Lblline.Text = objval.lineval
        '----------End  The code -----------------

        lblVItemCode.Text = objval.vendor_Item_Code
        lblDesc.Text = objval.Product_Desc
        lblCurrency.Text = objval.cur
        lblQty.Text = objval.Order_Qty
        lblReceived.Text = objval.Rec_Qty
        lblRejected.Text = objval.Rej_Qty
        lblUnitCost.Text = Format(CDbl(objval.Unit_Cost), "###,###,##0.00")
        lblRemarks.Text = objval.remarks

        lblGST.Text = Format(CDbl(objval.Order_Qty * objval.Unit_Cost) * (objval.tax / 100), "###,###,##0.00")
        'lblTotCost.Text = lblQty.Text * lblUnitCost.Text
        lblTotCost.Text = Format(CDbl(objval.Order_Qty * objval.Unit_Cost), "###,###,##0.00")
        lblTotCostGST.Text = Format(CDbl((objval.Order_Qty * objval.Unit_Cost) + lblGST.Text), "###,###,##0.00")
        lblTotRecCost.Text = Format(CDbl(objval.Rec_Qty - objval.Rej_Qty) * (objval.Unit_Cost), "###,###,##0.00")

        If objGst.chkGSTCOD(Format(CDate(objval.POM_CREATED_DATE), "dd/MM/yyyy")) = True Then
            lbl_GST.Text = "SST Rate"
            lblGST.Text = Common.parseNull(objval.gst_code)
            lbl_TotCostGST.Text = "Total Cost w/SST"
        Else
            lbl_GST.Text = "Tax"
            lbl_TotCostGST.Text = "Total Cost w/Tax"
        End If
    End Function
End Class
