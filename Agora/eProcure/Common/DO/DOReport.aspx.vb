Imports AgoraLegacy
Imports eProcure.Component
Public Class DDOReport
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblSuppComp As System.Web.UI.WebControls.Label
    Protected WithEvents lblSuppAdd1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblGstRegNo As System.Web.UI.WebControls.Label
    Protected WithEvents dtgDODtl As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents lblDONo As System.Web.UI.WebControls.Label
    Protected WithEvents lblDevlDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblPayTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblPayMthd As System.Web.UI.WebControls.Label
    Protected WithEvents lblPONo As System.Web.UI.WebControls.Label
    Protected WithEvents lblBuyerCompName As System.Web.UI.WebControls.Label
    Protected WithEvents lblAttnTo As System.Web.UI.WebControls.Label
    Protected WithEvents lblRequestor As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipAdd1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblBillAdd1 As System.Web.UI.WebControls.Label
    Dim i As Integer
    Protected WithEvents lblBuyerCompName2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipMthd As System.Web.UI.WebControls.Label
    Protected WithEvents lblOurRefNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblRefDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblAirWayBillNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblFreightCarrier As System.Web.UI.WebControls.Label
    Protected WithEvents lblFrightAmt As System.Web.UI.WebControls.Label
    Protected WithEvents lblTel As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmail As System.Web.UI.WebControls.Label
    Protected WithEvents lblRemark As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim strDONo, strSCoyID As String
    Dim intOrder, intShipped As Long
    Public Enum EnumDODtl
        DLine
        ICode
        P_Desc
        P_UOM
        P_ETD
        P_Waran
        P_Qty
        Order_Qty
        Ordered_Qty
        Ship_QTY
        Remark
    End Enum
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        'Session("CompanyID") = "PPSB"
        strDONo = Me.Request.QueryString("DONo")
        strSCoyID = Me.Request.QueryString("SCoyID")
        'strDONo = "DO04-001"
        'SetGridProperty(dtgDODtl)
        Bindgrid()
    End Sub
    Sub dtgDODtl_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgDODtl.PageIndexChanged
        dtgDODtl.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgDODtl.SortCommand
        Grid_SortCommand(sender, e)
        dtgDODtl.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objDO As New DeliveryOrder
        Dim strBillAdd, strShipAdd, strSuppAdd As String
        '//Retrieve Data from Database
        Dim dsDO As DataSet = New DataSet
        Dim dt, dt1 As DataTable

        dsDO = objDO.DOReport(strDONo, strSCoyID)
        dt = dsDO.Tables(0)
        ' SELECT Distinct DO_Mstr.*, DO_Details.DOD_DO_Line,DO_Details.DOD_Remarks,DO_Details.DOD_DO_Qty,DO_Details.DOD_SHIPPED_QTY, " & _
        '" PO_Mstr.POM_S_Coy_ID,PO_Mstr.POM_PO_Index,PO_Mstr.POM_Payment_TERM,PO_Mstr.POM_PAYMENT_METHOD,PO_Mstr.POM_Shipment_Term,PO_Mstr.POM_Shipment_Mode," & _
        '      " PO_Mstr.POM_PO_No,PO_Details.POD_D_Addr_Line1,PO_Details.POD_D_Addr_Line2,PO_Details.POD_D_Addr_Line3, " & _
        '      " PO_Details.POD_D_State,PO_Details.POD_D_Country,PO_Details.POD_D_PostCode,PO_Details.POD_D_City," & _
        '      " PO_Mstr.POM_B_Coy_ID, PO_Mstr.POM_PO_No, MC.CM_Coy_Logo,MC.CM_GST_Reg_No," & _
        '     " PO_Mstr.POM_B_Addr_Line1,PO_Mstr.POM_B_Addr_Line2,PO_Mstr.POM_B_Addr_Line3, " & _
        '" PO_Details.POD_Ordered_Qty,PO_Details.POD_B_ITEM_CODE,PO_Details.POD_Vendor_Item_Code,PO_Details.POD_Product_Desc,PO_Details.POD_UOM,PO_Details.POD_Po_Line, " & _
        '" PO_Details.POD_ETD,PO_Details.POD_Min_Pack_Qty,PO_Details.POD_Min_Order_Qty,PO_Details.POD_Warranty_Terms" & _

        'DOM_DO_INDEX(, DOM_DO_NO, DOM_S_COY_ID, DOM_DO_DATE, DOM_S_REF_NO, DOM_S_REF_DATE, DOM_PO_INDEX, DOM_WAYBILL_NO, DOM_FREIGHT_CARRIER, DOM_FREIGHT_AMT, DOM_DO_REMARKS, DOM_DO_STATUS, DOM_CREATED_DATE, DOM_CREATED_BY, DOM_NOOFCOPY_PRINTED, DOM_GRN_INDEX, DOM_DO_PREFIX, DOM_D_ADDR_CODE, DOM_D_ADDR_LINE1, DOM_D_ADDR_LINE2, DOM_D_ADDR_LINE3, DOM_D_POSTCODE, DOM_D_CITY, DOM_D_STATE, DOM_D_COUNTRY, DOM_EXTERNAL_IND, DOM_REFERENCE_NO)

        lblSuppComp.Text = dt.Rows(0)("POM_S_Coy_Name")
        strSuppAdd = Common.parseNull(dt.Rows(0)("POM_S_ADDR_LINE1"))
        If Not IsDBNull(dt.Rows(0)("POM_S_ADDR_LINE2")) AndAlso dt.Rows(0)("POM_S_ADDR_LINE2") <> "" Then
            strSuppAdd = strSuppAdd & "<BR>" & dt.Rows(0)("POM_S_ADDR_LINE2")
        End If

        If Not IsDBNull(dt.Rows(0)("POM_S_ADDR_LINE3")) AndAlso dt.Rows(0)("POM_S_ADDR_LINE3") <> "" Then
            strSuppAdd = strSuppAdd & "<BR>" & dt.Rows(0)("POM_S_ADDR_LINE3")
        End If

        If Not IsDBNull(dt.Rows(0)("POM_S_POSTCODE")) Then
            strSuppAdd = strSuppAdd & "<BR>" & dt.Rows(0)("POM_S_POSTCODE")
        End If

        If Not IsDBNull(dt.Rows(0)("POM_S_CITY")) Then
            strSuppAdd = strSuppAdd & " " & dt.Rows(0)("POM_S_CITY")
        End If

        If Not IsDBNull(dt.Rows(0)("POM_S_STATE")) Then
            strSuppAdd = strSuppAdd & "<BR>" & dt.Rows(0)("POM_S_STATE")
        End If

        If Not IsDBNull(dt.Rows(0)("POM_S_COUNTRY")) Then
            strSuppAdd = strSuppAdd & " " & dt.Rows(0)("POM_S_COUNTRY")
        End If

        lblSuppAdd1.Text = strSuppAdd
        lblGstRegNo.Text = Common.parseNull(dt.Rows(0)("CM_BUSINESS_REG_NO"))
        lblTel.Text = Common.parseNull(dt.Rows(0)("CM_PHONE"))
        lblEmail.Text = Common.parseNull(dt.Rows(0)("CM_EMAIL"))
        lblDONo.Text = strDONo
        lblPONo.Text = dt.Rows(0)("POM_PO_No")
        lblDevlDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dt.Rows(0)("DOM_DO_DATE"))
        lblOurRefNo.Text = Common.parseNull(dt.Rows(0)("DOM_S_REF_NO"))
        lblFreightCarrier.Text = Common.parseNull(dt.Rows(0)("DOM_FREIGHT_CARRIER"))
        lblRefDate.Text = Common.parseNull(dt.Rows(0)("DOM_S_REF_DATE"))
        lblFrightAmt.Text = Common.parseNull(dt.Rows(0)("DOM_FREIGHT_AMT"))
        lblAirWayBillNo.Text = Common.parseNull(dt.Rows(0)("DOM_Waybill_No"))
        'DOM_Waybill_No
        lblPayTerm.Text = Common.parseNull(dt.Rows(0)("POM_Payment_TERM"))
        lblShipTerm.Text = Common.parseNull(dt.Rows(0)("POM_Shipment_Term"))
        lblPayMthd.Text = Common.parseNull(dt.Rows(0)("POM_PAYMENT_METHOD"))
        lblShipMthd.Text = Common.parseNull(dt.Rows(0)("POM_Shipment_Mode"))
        lblBuyerCompName.Text = Common.parseNull(dt.Rows(0)("CM_Coy_Name"))
        lblBuyerCompName2.Text = Common.parseNull(dt.Rows(0)("CM_Coy_Name"))
        lblRequestor.Text = Common.parseNull(dt.Rows(0)("POM_BUYER_NAME"))

        ' ai chu add on 13/10/2005 - requested by user1
        lblRemark.Text = Common.parseNull(dt.Rows(0)("DOM_DO_Remarks"))

        '//Change by Moo, get from PR
        'PRM_S_ATTN,PRM_REQ_NAME
        dt1 = dsDO.Tables(1)
        'lblAttnTo.Text = Common.parseNull(dt.Rows(0)("POM_BUYER_NAME"))

        'Michelle 25/7/2007 - To cater for multiple POs
        '  lblAttnTo.Text = Common.parseNull(dt1.Rows(0).Item("PRM_S_ATTN"))

        Dim strsql_att As String
        Dim dt2 As New DataSet
        Dim objDb As New EAD.DBCom

        strsql_att = "SELECT PRM_S_ATTN,PRM_REQ_NAME FROM PR_MSTR WHERE PRM_PO_INDEX=" & dt.Rows(0)("POM_PO_Index")
        dt2 = objDb.FillDs(strsql_att)
        If dt2.Tables(0).Rows.Count > 0 Then
            lblAttnTo.Text = dt2.Tables(0).Rows(0)("PRM_S_ATTN")
        Else
            lblAttnTo.Text = " "
        End If

        strShipAdd = Common.parseNull(dt.Rows(0)("POD_D_Addr_Line1"))
        If Not IsDBNull(dt.Rows(0)("POD_D_Addr_Line2")) AndAlso dt.Rows(0)("POD_D_Addr_Line2") <> "" Then
            strShipAdd = strShipAdd & "<BR>" & dt.Rows(0)("POD_D_Addr_Line2")
        End If

        If Not IsDBNull(dt.Rows(0)("POD_D_Addr_Line3")) AndAlso dt.Rows(0)("POD_D_Addr_Line3") <> "" Then
            strShipAdd = strShipAdd & "<BR>" & dt.Rows(0)("POD_D_Addr_Line3")
        End If

        If Not IsDBNull(dt.Rows(0)("POD_D_POSTCODE")) Then
            strShipAdd = strShipAdd & "<BR>" & dt.Rows(0)("POD_D_POSTCODE")
        End If

        If Not IsDBNull(dt.Rows(0)("POD_D_CITY")) AndAlso dt.Rows(0)("POD_D_CITY") <> "" Then
            strShipAdd = strShipAdd & " " & dt.Rows(0)("POD_D_CITY")
        End If

        If Not IsDBNull(dt.Rows(0)("POD_D_STATE")) Then
            strShipAdd = strShipAdd & "<BR>" & dt.Rows(0)("POD_D_STATE")
        End If

        If Not IsDBNull(dt.Rows(0)("POD_D_COUNTRY")) Then
            strShipAdd = strShipAdd & " " & dt.Rows(0)("POD_D_COUNTRY")
        End If
        lblShipAdd1.Text = strShipAdd

        strBillAdd = Common.parseNull(dt.Rows(0)("POM_B_Addr_Line1"))

        If Not IsDBNull(dt.Rows(0)("POM_B_Addr_Line2")) AndAlso dt.Rows(0)("POM_B_Addr_Line2") <> "" Then
            strBillAdd = strBillAdd & "<BR>" & dt.Rows(0)("POM_B_Addr_Line2")
        End If

        If Not IsDBNull(dt.Rows(0)("POM_B_Addr_Line3")) AndAlso dt.Rows(0)("POM_B_Addr_Line3") <> "" Then
            strBillAdd = strBillAdd & "<BR>" & dt.Rows(0)("POM_B_Addr_Line3")
        End If

        If Not IsDBNull(dt.Rows(0)("POM_B_PostCode")) Then
            strBillAdd = strBillAdd & "<BR>" & dt.Rows(0)("POM_B_PostCode")
        End If

        If Not IsDBNull(dt.Rows(0)("POM_B_City")) AndAlso dt.Rows(0)("POM_B_City") <> "" Then
            strBillAdd = strBillAdd & " " & dt.Rows(0)("POM_B_City")
        End If

        If Not IsDBNull(dt.Rows(0)("POM_B_State")) Then
            strBillAdd = strBillAdd & "<BR>" & dt.Rows(0)("POM_B_State")
        End If

        If Not IsDBNull(dt.Rows(0)("POM_B_Country")) Then
            strBillAdd = strBillAdd & " " & dt.Rows(0)("POM_B_Country")
        End If

        lblBillAdd1.Text = strBillAdd
        '//add By Moo, for Logo
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        strImgSrc = objFile.getCoyLogo(EnumUploadFrom.FrontOff, strSCoyID)
        If strImgSrc <> "" Then
            Image1.Visible = True

            Dim bitmapImg As System.Drawing.Image = System.Drawing.Image.FromFile(Server.MapPath(strImgSrc))
            If bitmapImg.Width < Image1.Width.Value Then
                Image1.Width = System.Web.UI.WebControls.Unit.Pixel(bitmapImg.Width)
            End If

            Image1.ImageUrl = strImgSrc
        Else
            Image1.Visible = False
        End If
        objFile = Nothing

        '//for sorting asc or desc
        Dim dvViewDO As DataView
        dvViewDO = dsDO.Tables(0).DefaultView
        dvViewDO.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewDO.Sort += " DESC"

        'lblDONo.Text = Common.parseNull(dtMstr.Rows(0)("DO_Mstr. DOM_DO_Remarks"))

        dtgDODtl.DataSource = dvViewDO
        dtgDODtl.DataBind()
        'AddRow()
    End Function
    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub
    Sub AddRemark(ByVal remark As String)
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String

        For intL = 0 To dtgDODtl.Columns.Count - 1
            addCell(row2)
        Next

        For intL = 2 To dtgDODtl.Columns.Count - 1
            row2.Cells.RemoveAt(1)
        Next
        ' row.Cells(0).ColumnSpan = intCell - 1

        Dim lbl_bremark As New Label
        lbl_bremark.ID = "test2"
        lbl_bremark.Text = "<b>Remarks </b>:" & remark

        lbl_bremark.CssClass = "txtbox"
        'lbl_test2.Width = System.Web.UI.WebControls.Unit.Pixel(500)
        'lbl_bremark.Font.Bold = True
        row2.Cells(1).ColumnSpan = dtgDODtl.Columns.Count - 2
        row2.Cells(1).Controls.Add(lbl_bremark)
        row2.Cells(1).HorizontalAlign = HorizontalAlign.Left
        Me.dtgDODtl.Controls(0).Controls.Add(row2)

    End Sub


    Sub AddRow()
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer


        intTotalCol = dtgDODtl.Columns.Count - 1
        For intL = 0 To intTotalCol
            addCell(row)
        Next
        '//13+3-5=11
        '//16-11 = 5

        For intL = 0 To intTotalCol - 4
            row.Cells.RemoveAt(1)
        Next

        row.Cells(0).ColumnSpan = 5
        'Dim dg As DataGridItem
        'total = 0
        'For Each dg In dtgPRList.Items
        '    total += Decimal.Parse(dg.Cells(intCell).Text)
        'Next
        row.Cells(1).HorizontalAlign = HorizontalAlign.Right
        row.Cells(1).Text = "Total"
        row.Cells(1).Font.Bold = True
        row.Cells(2).HorizontalAlign = HorizontalAlign.Right
        row.Cells(2).Text = Format(intOrder)
        row.Cells(2).Font.Bold = True
        row.Cells(3).HorizontalAlign = HorizontalAlign.Right
        row.Cells(3).Text = Format(intShipped)
        row.Cells(3).Font.Bold = True

        'row.CssClass = "BODY"
        'row.BackColor = Color.FromName("#f4f4f4")
        dtgDODtl.Controls(0).Controls.Add(row)
    End Sub
    Private Sub dtgDODtl_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDODtl.ItemCreated
        '//this line must be included
        'Grid_ItemCreated(dtgDODtl, e)
    End Sub
    Sub DrawLine()
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String

        For intL = 0 To dtgDODtl.Columns.Count - 1
            addCell(row2)
        Next

        For intL = 1 To dtgDODtl.Columns.Count - 1
            row2.Cells.RemoveAt(1)
        Next
        ' row.Cells(0).ColumnSpan = intCell - 1

        Dim lbl_test2 As New Label
        lbl_test2.ID = "lblHeaderLine"
        lbl_test2.Text = "<HR>"

        'lbl_test2.CssClass = "txtbox"
        'lbl_test2.Width = System.Web.UI.WebControls.Unit.Pixel(500)
        lbl_test2.Font.Bold = True
        row2.Cells(0).ColumnSpan = 11
        row2.Cells(0).Controls.Add(lbl_test2)
        row2.Cells(0).HorizontalAlign = HorizontalAlign.Left
        Me.dtgDODtl.Controls(0).Controls.Add(row2)

    End Sub

    Private Sub dtgDODtl_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDODtl.ItemDataBound
        If i = 0 Then
            DrawLine()
        End If
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            intOrder = intOrder + dv("POD_Ordered_Qty")
            intShipped = intShipped + dv("DOD_SHIPPED_QTY")
            '//Original One
            'If IsDBNull(e.Item.Cells(4)) Or e.Item.Cells(4).Text = "0" Then
            '    e.Item.Cells(4).Text = "Ex-Stock"
            'Else
            '    e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, DateAdd("D", dv("POD_ETD"), dv("POM_PO_DATE")))
            'End If
            '//
            '***************meilai 4/5/2005************************
            AddRemark(Common.parseNull(dv("DOD_REMARKS")))
            '******************************************************

            If IsDBNull(e.Item.Cells(EnumDODtl.P_ETD)) Or e.Item.Cells(EnumDODtl.P_ETD).Text = "0" Then
                e.Item.Cells(EnumDODtl.P_ETD).Text = "Ex-Stock"
            Else
                e.Item.Cells(EnumDODtl.P_ETD).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, DateAdd("D", dv("POD_ETD"), dv("POM_PO_DATE")))
            End If

        End If
        i = i + 1
    End Sub

    Private Sub dtgDODtl_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtgDODtl.SelectedIndexChanged

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess()
    End Sub
End Class
