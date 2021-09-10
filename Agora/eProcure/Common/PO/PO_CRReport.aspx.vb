Imports AgoraLegacy
Imports eProcure.Component

Public Class PO_CRReport
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblVendorName As System.Web.UI.WebControls.Label
    Protected WithEvents lblVAddr As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendorTel As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendorEmail As System.Web.UI.WebControls.Label
    Dim i, jLoop, iLoop As Integer
    Dim objGlobal As New AppGlobals
    Dim value1(100) As String
    Dim value2(100) As String
    Protected WithEvents lblCountry As System.Web.UI.WebControls.Label
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents lblShipVia As System.Web.UI.WebControls.Label
    Protected WithEvents lblFreighTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipMode As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblPayMethod As System.Web.UI.WebControls.Label
    Protected WithEvents lblPayTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblReqDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblCRNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblPONo As System.Web.UI.WebControls.Label
    Protected WithEvents lblAddr As System.Web.UI.WebControls.Label
    Protected WithEvents lblComName As System.Web.UI.WebControls.Label
    Protected WithEvents lblRemark As System.Web.UI.WebControls.Label
    Protected WithEvents lblBillTo As System.Web.UI.WebControls.Label
    Protected WithEvents dtg_POList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblBuyerEmail As System.Web.UI.WebControls.Label
    Protected WithEvents lblTel As System.Web.UI.WebControls.Label
    Protected WithEvents lblReqBuyer As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipTo As System.Web.UI.WebControls.Label
    Protected WithEvents lblPODate As System.Web.UI.WebControls.Label
    Protected WithEvents lblBuyerComp As System.Web.UI.WebControls.Label
    Protected WithEvents lblBregNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblVregNo As System.Web.UI.WebControls.Label
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim blnPreviewPO As Boolean
    Dim strCurrCode As String
    Dim blnPrintRemark As Boolean
    Dim blnPrintCustField As Boolean
    Dim strDelCode As String = ""
    Dim dtAddr As DataTable

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'MyBase.Page_Load(sender, e
        'If Not IsPostBack Then
        MyBase.Page_Load(sender, e)
        displayPOReport()
        Bindgrid()
        'End If
    End Sub


    Private Function displayPOReport()

        Dim objpo As New PurchaseOrder
        Dim objval As New POValue
        Dim strsql As String

        objval.PO_Number = Request(Trim("po_no")).ToString.Trim
        objval.buyer_coy = Request(Trim("BCoyID"))
        objval.CR_NO = Request(Trim("cr_no"))
        objval.linevalue(0) = Request(Trim("linevalue(0)"))

        lblCRNo.Text = objval.CR_NO ' Michelle (21/12/2007) - To prevent system re-get the CR no.

        'Michelle (24/12/2007) - To get the correct CR remarks
        strsql = "Select PCM_CR_REMARKS from PO_CR_MSTR where PCM_CR_NO = '" & lblCRNo.Text & "' AND PCM_B_COY_ID = '" & objval.buyer_coy & "'"

        Dim objDb As New EAD.DBCom
        Dim tDS As DataSet = objDb.FillDs(strsql)
        If tDS.Tables(0).Rows.Count > 0 Then
            lblRemark.Text = tDS.Tables(0).Rows(0).Item("PCM_CR_REMARKS")
        End If


        Dim objComp As New Companies
        Dim objCompDetails As New Company
        objCompDetails = objComp.GetCompanyDetails(objval.buyer_coy) '// BUYER 
        lblBregNo.Text = objCompDetails.BusinessRegNo

        '//add by Moo, to cater for PR consolidation
        If Request.QueryString("prev") <> "" Then
            blnPreviewPO = Request.QueryString("prev")
        Else
            blnPreviewPO = False
        End If
        objpo.get_PODetail(objval, Request(Trim("side")), blnPreviewPO)
        lblComName.Text = objval.BCoyName 'objval.buyer_contact
        lblAddr.Text = objpo.get_comadd(objval, Request(Trim("side")), blnPreviewPO)
        'lblPONo.Text = "<B>" & objval.PO_Number & "</B>"
        lblPONo.Text = objval.PO_Number
        lblPODate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, objval.PO_Date)
        lblReqDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, objval.CR_ReqDate)
        lblPayTerm.Text = objval.pay_term
        lblPayMethod.Text = objval.pay_meth
        lblShipTerm.Text = objval.ship_term
        lblShipMode.Text = objval.ship_meth
        'lblFreighTerm.Text = objval.f
        lblShipVia.Text = objval.ship_via
        lblReqBuyer.Text = objval.CR_ReqBy
        'lblBuyer.Text = objval.buyer_contact
        lblTel.Text = objval.buyer_tel
        'lblBusiness.Text =
        lblBuyerEmail.Text = objval.buyer_email
        lblVendorName.Text = objval.vendor_Coy
        lblVAddr.Text = objval.vendor_adds
        lblVendorTel.Text = objval.vendor_tel
        'lblVendorFax.Text = objval.vendor_fax
        lblVendorEmail.Text = objval.vendor_email
        'lblCountry.Text=objval.
        lblShipTo.Text = "See Below" 'objval.buyer_adds
        'lblTaxNo.Text = objval.tax
        lblBillTo.Text = objval.bill_adds
        lblBuyerComp.Text = objval.BCoyName
        'lblAttn.Text = objval.vendor_contact
        lblFreighTerm.Text = objval.FreightTerm

        Dim objVenComp As New Companies
        Dim objVenCompDetails As New Company
        'Michelle (20/7/2007) - To provent error when vendor view the CR
        ' If objval.vendor_CoyId = "" Then
        'objval.vendor_CoyId = 
        '  End If
        objVenCompDetails = objComp.GetCompanyDetails(objval.vendor_CoyId) '// vendor
        lblVregNo.Text = objVenCompDetails.BusinessRegNo


        'add By Moo, for Logo
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        strImgSrc = objFile.getCoyLogo(EnumUploadFrom.FrontOff, Request(Trim("BCoyID")))
        If strImgSrc <> "" Then
            Image1.ImageUrl = strImgSrc
        Else
            Image1.Visible = False
        End If
    End Function
    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String


        Dim objPO As New PurchaseOrder

        '//Retrieve Data from Database
        Dim ds As DataSet
        'Dim objdb As New EAD.DBCom()
        'Dim strPOCode As String
        'strPOCode = Me.lblPONo.Text
        Dim cr_no As String = Request(Trim("cr_no"))
        Dim b_com_id As String = Request(Trim("BCoyID"))
        'ds = objPO.getlineitem(strPOCode)
        ds = objPO.get_cancelLineitem(Request(Trim("po_no")), b_com_id, Request(Trim("side")), cr_no, True)


        Dim PO_No As String = ""

        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewPR.Sort = viewstate("SortExpression")
            If viewstate("SortAscending") = "no" Then dvViewPR.Sort += " DESC"
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            'intTotPage = dtgDept.PageCount
            dtAddr = ds.Tables(0)
            dtg_POList.DataSource = dvViewPR
            dtg_POList.DataBind()
        Else
            'dtgDept.DataSource = ""
            dtg_POList.DataBind()
            ' Common.NetMsgbox(Me, "No record found.")
            'intTotPage = 0
        End If

    End Function
    
    Sub AddRow(ByVal remark As String)
        'adding totals row
        Dim intL As Integer

        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim strtext As String

        For intL = 0 To dtg_POList.Columns.Count - 1
            addCell(row2)
        Next

        For intL = 1 To dtg_POList.Columns.Count - 2
            row2.Cells.RemoveAt(1)
        Next

        ' row.Cells(0).ColumnSpan = intCell - 1
        Dim lbl_test2 As New Label
        lbl_test2.ID = "test2"
        lbl_test2.Text = "<B>Remarks</B> : " & remark & ""
        lbl_test2.CssClass = "txtbox"
        lbl_test2.Width = System.Web.UI.WebControls.Unit.Pixel(500)
        'lbl_test2.Font.Bold = True
        row2.Cells(1).ColumnSpan = 10
        row2.Cells(1).Controls.Add(lbl_test2)
        row2.Cells(1).HorizontalAlign = HorizontalAlign.Left
        Me.dtg_POList.Controls(0).Controls.Add(row2)
    End Sub
    Sub AddAddressRow(ByVal intRow As Integer)
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String

        For intL = 0 To dtg_POList.Columns.Count - 1
            addCell(row2)
        Next

        For intL = 1 To dtg_POList.Columns.Count - 1
            row2.Cells.RemoveAt(1)
        Next
        ' row.Cells(0).ColumnSpan = intCell - 1

        Dim strTempAddr As String
        strTempAddr = Common.parseNull(dtAddr.Rows(intRow)("POD_D_ADDR_LINE1"))

        If Not IsDBNull(dtAddr.Rows(intRow)("POD_D_ADDR_LINE2")) AndAlso dtAddr.Rows(intRow)("POD_D_ADDR_LINE2") <> "" Then
            strTempAddr = strTempAddr & " " & dtAddr.Rows(intRow)("POD_D_ADDR_LINE2")
        End If

        If Not IsDBNull(dtAddr.Rows(intRow)("POD_D_ADDR_LINE3")) AndAlso dtAddr.Rows(intRow)("POD_D_ADDR_LINE3") <> "" Then
            strTempAddr = strTempAddr & " " & dtAddr.Rows(intRow)("POD_D_ADDR_LINE3")
        End If

        If Not IsDBNull(dtAddr.Rows(intRow)("POD_D_POSTCODE")) AndAlso dtAddr.Rows(intRow)("POD_D_POSTCODE") <> "" Then
            strTempAddr = strTempAddr & " " & dtAddr.Rows(intRow)("POD_D_POSTCODE")
        End If

        If Not IsDBNull(dtAddr.Rows(intRow)("POD_D_CITY")) AndAlso dtAddr.Rows(intRow)("POD_D_CITY") <> "" Then
            strTempAddr = strTempAddr & " " & dtAddr.Rows(intRow)("POD_D_CITY")
        End If

        If Not IsDBNull(dtAddr.Rows(intRow)("POD_D_STATE")) AndAlso dtAddr.Rows(intRow)("POD_D_STATE") <> "" Then
            strTempAddr = strTempAddr & " " & objGlobal.getCodeDesc(CodeTable.State, dtAddr.Rows(intRow)("POD_D_STATE"))
        End If

        If Not IsDBNull(dtAddr.Rows(intRow)("POD_D_COUNTRY")) AndAlso dtAddr.Rows(intRow)("POD_D_COUNTRY") <> "" Then
            strTempAddr = strTempAddr & " " & objGlobal.getCodeDesc(CodeTable.Country, dtAddr.Rows(intRow)("POD_D_COUNTRY"))
        End If

        Dim lbl_test2 As New Label
        lbl_test2.ID = "test2"
        lbl_test2.Text = "Ship To : " & Common.parseNull(dtAddr.Rows(intRow)("POD_D_ADDR_CODE")) & " - " & strTempAddr

        lbl_test2.CssClass = "txtbox"
        'lbl_test2.Width = System.Web.UI.WebControls.Unit.Pixel(500)
        lbl_test2.Font.Bold = True
        row2.Cells(0).ColumnSpan = dtg_POList.Columns.Count
        row2.Cells(0).Controls.Add(lbl_test2)
        row2.Cells(0).HorizontalAlign = HorizontalAlign.Left
        Me.dtg_POList.Controls(0).Controls.Add(row2)

    End Sub
    Sub AddRowtotal(ByVal intCell As Integer)
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        For intL = 0 To Me.dtg_POList.Columns.Count - 1
            addCell(row)
        Next

        'For intL = 0 To 1
        '    row.Cells.RemoveAt(1)
        'Next
        'row.Cells(0).ColumnSpan = intCell - 1
        Dim lbl_test As New Label
        lbl_test.ID = "test"
        lbl_test.Text = "TEST "
        lbl_test.CssClass = "lblnumerictxtbox"
        lbl_test.Width = System.Web.UI.WebControls.Unit.Pixel(80)
        lbl_test.Font.Bold = True
        row.Cells(6).Text = "Sub Total"
        row.Cells(6).HorizontalAlign = HorizontalAlign.Right
        row.Cells(6).Font.Bold = True

        row.Cells(7).Text = Format(viewstate("total"), "#,##0.00")
        row.Cells(7).HorizontalAlign = HorizontalAlign.Right
        ' row.Cells(7).Font.Bold = True

        row.Cells(8).Text = Format(viewstate("gst"), "#,##0.00")
        row.Cells(8).HorizontalAlign = HorizontalAlign.Right
        'row.Cells(8).ColumnSpan = 10

        Me.dtg_POList.Controls(0).Controls.Add(row)


        For intL = 0 To Me.dtg_POList.Columns.Count - 1
            addCell(row2)
        Next

        row2.Cells(6).Text = "Total(w/Tax)"
        row2.Cells(6).HorizontalAlign = HorizontalAlign.Right
        row2.Cells(6).Font.Bold = True

        row2.Cells(7).Text = Format(viewstate("total") + viewstate("gst"), "#,##0.00")
        row2.Cells(7).HorizontalAlign = HorizontalAlign.Right

        Me.dtg_POList.Controls(0).Controls.Add(row2)

    End Sub
    Sub AddRowmoney(ByVal intCell As Integer, ByVal money As String)
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        For intL = 0 To Me.dtg_POList.Columns.Count - 1
            addCell(row)
        Next

        'For intL = 0 To 1
        '    row.Cells.RemoveAt(1)
        'Next
        'row.Cells(0).ColumnSpan = intCell - 1
        row.Cells(6).Text = "( " & money & " )"
        row.Cells(6).HorizontalAlign = HorizontalAlign.Left
        'row.Cells(6).ColumnSpan = 10

        row.Cells(7).Text = "( " & money & " )"
        row.Cells(7).HorizontalAlign = HorizontalAlign.Left
        ' row.Cells(7).ColumnSpan = 10

        row.Cells(8).Text = "( " & money & " )"
        row.Cells(8).HorizontalAlign = HorizontalAlign.Left
        'row.Cells(8).ColumnSpan = 10

        Me.dtg_POList.Controls(0).Controls.Add(row)
    End Sub

    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)

    End Sub

    Private Sub dtg_POList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_POList.ItemCreated
        '//iLoop - Looping for all Items(Header,item,footer)
        '//jLoop - Looping for Items
        If iLoop = 0 Then
            'AddAddressRow(0)
            'strDelCode = dtAddr.Rows(0)("POD_D_ADDR_CODE")
        Else
            '//display only address code if different from prev one
            If jLoop <> 0 And jLoop <= dtAddr.Rows.Count - 1 Then
                If strDelCode <> dtAddr.Rows(jLoop)("POD_D_ADDR_CODE") Then
                    AddAddressRow(jLoop)
                    strDelCode = dtAddr.Rows(jLoop)("POD_D_ADDR_CODE")
                End If
            End If
        End If

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            jLoop += 1
        End If
        iLoop += 1

    End Sub
    Sub DrawLine()
        Dim intL, intColToRemain As Integer
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String

        For intL = 0 To dtg_POList.Columns.Count - 1
            addCell(row2)
        Next

        For intL = 1 To dtg_POList.Columns.Count - 1
            row2.Cells.RemoveAt(1)
        Next

        Dim lbl_test2 As New Label
        lbl_test2.ID = "lblHeaderLine"
        lbl_test2.Text = "<HR>"
        lbl_test2.Font.Bold = True
        row2.Cells(0).ColumnSpan = dtg_POList.Columns.Count
        row2.Cells(0).Controls.Add(lbl_test2)
        row2.Cells(0).HorizontalAlign = HorizontalAlign.Left
        Me.dtg_POList.Controls(0).Controls.Add(row2)

    End Sub
    Private Sub dtg_POList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_POList.ItemDataBound
        Dim objpr As New PurchaseReq2
 
        If i = 0 Then
            DrawLine()
            AddAddressRow(0)
            strDelCode = dtAddr.Rows(0)("POD_D_ADDR_CODE")
        End If
         If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim j As Integer
            Dim objpo As New PurchaseOrder
            Dim cancel_item As Integer
            If IsDBNull(dv("POD_CANCELLED_QTY")) Then
                cancel_item = 0
            Else
                cancel_item = CInt(dv("POD_CANCELLED_QTY"))

            End If

            objpo.get_customfield(value1, value2, dv("POD_PR_LINE"), dv("POD_PR_INDEX"), j)

            AddRow1(1, value1, value2, j)

            'Michelle (3/7/2010) - To place the Outstanding value into the correct possition
            'e.Item.Cells(8).Text = CInt(dv("POD_ORDERED_QTY")) - cancel_item - CInt(dv("POD_DELIVERED_QTY"))
            e.Item.Cells(9).Text = CInt(dv("POD_ORDERED_QTY")) - cancel_item - CInt(dv("POD_DELIVERED_QTY"))
            AddRow(Common.parseNull(dv("PCD_REMARKS")))
            'Michelle (20/7/2007) - To get the re-sequenced PO Line no.
            e.Item.Cells(1).Text = objpr.getPO_CR_LineNo(Request(Trim("BCoyID")), lblPONo.Text, dv("POD_PO_LINE"))

        End If
        i = i + 1

    End Sub

    Sub AddRow1(ByVal intCell As Integer, ByVal value1() As String, ByVal value2() As String, ByVal j As Integer)
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String

        '//TO PRINT CUSTOM FIELD
        '//ONLY SHOW CUSTOM FIELD IF IT IS CHECKED IN PR

        For count = 0 To j - 1
            strtext = strtext & "<B>" & value1(count) & "</B> : " & value2(count) & ""
            strtext = strtext & "<br>"
        Next
        For intL = 0 To Me.dtg_POList.Columns.Count - 1
            addCell(row)
        Next

        For intL = 1 To dtg_POList.Columns.Count - 2
            row.Cells.RemoveAt(1)
        Next

        'row.Cells(0).ColumnSpan = intCell - 1
        Dim lbl_test As New Label
        lbl_test.ID = "test"
        lbl_test.Text = strtext
        lbl_test.CssClass = "txtbox"
        lbl_test.Width = System.Web.UI.WebControls.Unit.Pixel(500)
        'lbl_test.Font.Bold = True
        lbl_test.Font.Bold = False

        row.Cells(1).ColumnSpan = 10
        row.Cells(1).Controls.Add(lbl_test)
        row.Cells(1).HorizontalAlign = HorizontalAlign.Left

        Me.dtg_POList.Controls(0).Controls.Add(row)

    End Sub



    Private Sub dtg_POList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtg_POList.SelectedIndexChanged

    End Sub
End Class
