Imports AgoraLegacy
Imports eProcure.Component


Public Class InvTransHistorySEH
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim intLocIndicator As Integer
    Dim strLoc As String
    Dim strSLoc As String
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        MyBase.Page_Load(sender, e)
        blnPaging = False
        SetGridProperty(dtgInv)

        If Not Page.IsPostBack Then
            'objInv.GetLocationInfo(strLoc, strSLoc, intLocIndicator)
            ViewState("SortAscending") = "no"
            ViewState("SortExpression") = "IT_TRANS_DATE"
            'dtgInv.CurrentPageIndex = 0
            BindInv(True)

        End If

        Me.lblItemCode.Text = Request.QueryString("ItemCode")
        Me.lblItemName.Text = Request.QueryString("ItemName")
        Me.lblLoc1.Text = Request.QueryString("Loc")
        Me.lblLotNo.Text = Request.QueryString("Lot")
        Me.lblSubLoc1.Text = Request.QueryString("SubLoc")
        Me.lblQty.Text = Request.QueryString("Qty")
        If Request.QueryString("NeedQC") = "N" Then
            lbl_needqc.Text = "No"
        Else
            lbl_needqc.Text = "Yes"
        End If
        ChangeHeaderText()

    End Sub

    Private Sub ChangeHeaderText()
        Dim objInv As New Inventory

        objInv.GetLocationInfo(strLoc, strSLoc, intLocIndicator)

        Me.lblLoc.Text = strLoc
        Me.lblSubLoc.Text = strSLoc

        If intLocIndicator > 1 Then 'Sub Location is defined
            lblSubLoc.Visible = True
            lblSubLoc1.Visible = True
        Else
            lblSubLoc.Visible = False
            lblSubLoc1.Visible = False
        End If
        objInv = Nothing

    End Sub

    Private Sub BindInv(Optional ByVal pSorted As Boolean = False)
        Dim objInv_Ext As New Inventory_Ext
        Dim ds As DataSet
        Dim dvViewInv As DataView

        ds = objInv_Ext.GetInvTransHistory(Request.QueryString("InvIndex"), Request.QueryString("LocIndex"), Request.QueryString("Lot"))
        dvViewInv = ds.Tables(0).DefaultView
        dvViewInv.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewInv.Sort += " DESC"

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        If intPageRecordCnt > 0 Then
            'resetDatagridPageIndex(dtgInv, dvViewInv)
            dtgInv.DataSource = dvViewInv
            dtgInv.DataBind()
            'Else
            '    dtgInv.DataBind()
            '    Common.NetMsgbox(Me, MsgNoRecord)
        End If
        'ViewState("PageCount") = dtgInv.PageCount
        objInv_Ext = Nothing

    End Sub

    Private Sub dtgInv_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInv.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgInv, e)
        ChangeHeaderText()
    End Sub

    Private Sub dtgInv_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInv.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            e.Item.Cells(0).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("IT_TRANS_DATE"))

            If dv("IT_TRANS_TYPE") = "TR" Then
                If dv("TRANS_QTY") > 0 Then
                    e.Item.Cells(1).Text = dv("CODE_DESC") & " - In"
                Else
                    e.Item.Cells(1).Text = dv("CODE_DESC") & " - Out"
                End If

            End If
        End If
    End Sub

    Private Sub dtgInv_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgInv.PageIndexChanged
        'dtgInv.CurrentPageIndex = e.NewPageIndex
        BindInv(True)
        ChangeHeaderText()
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgInv.SortCommand
        Grid_SortCommand(sender, e)
        'dtgInv.CurrentPageIndex = 0
        BindInv(True)
        ChangeHeaderText()
    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language='javascript'> { window.close();}</script>")

    End Sub

End Class