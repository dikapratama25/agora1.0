Imports System.IO
Imports AgoraLegacy
Imports eProcure.Component

Public Class POReport
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblPayTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblPayMethod As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipTo As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipVia As System.Web.UI.WebControls.Label
    Protected WithEvents lblBuyer As System.Web.UI.WebControls.Label
    Protected WithEvents lblTel As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipMode As System.Web.UI.WebControls.Label
    Protected WithEvents lblFreighTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblRemark As System.Web.UI.WebControls.Label
    Protected WithEvents lblPONo As System.Web.UI.WebControls.Label
    Protected WithEvents lblBillTo As System.Web.UI.WebControls.Label
    Protected WithEvents dtgPOReport As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblComName As System.Web.UI.WebControls.Label
    Protected WithEvents lblAddr As System.Web.UI.WebControls.Label
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
    Protected WithEvents lblTaxNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendorName2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblBusiness As System.Web.UI.WebControls.Label
    Protected WithEvents lblBuyerEmail As System.Web.UI.WebControls.Label
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
    Dim blnDrawLine As Boolean = False

    Dim blnFreeze As Boolean
    Dim strNewUrl As String

    Public Property NewUrl() As String
        Get
            NewUrl = strNewUrl
        End Get
        Set(ByVal Value As String)
            strNewUrl = Value
        End Set
    End Property

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If (blnFreeze) Then
            Dim htmlFile As New AppHtml
            ' let Asp.net render the output, catch it in the file creator
            MyBase.Render(htmlFile.RenderHere)
            htmlFile.WriteHTMLFile(Server.MapPath(NewUrl))

            Dim objFile As New FileManagement
            Dim strFilePath, strMovePath As String
            objFile.getFilePath(EnumUploadFrom.FrontOff, EnumUploadType.PDFDownload, strFilePath, strMovePath, False, "PO")
            objFile = Nothing
            Dim strFileName As String
            strFileName = strFilePath & Session("InvFileName")
            If Not File.Exists(strFileName) Then
                File.Copy(Server.MapPath(NewUrl), strFileName)
            Else
                htmlFile.WriteHTMLFile(Server.MapPath(NewUrl), strFileName)
            End If
            ' ai chu add on 21/09/2005
            ' to remove generated file in ExtraFunc folder or respective module folder
            File.Delete(Server.MapPath(NewUrl))
        Else
            MyBase.Render(writer)
        End If
    End Sub

    Protected Sub Freeze(ByVal strtoUrl As String)
        blnFreeze = True
        NewUrl = strtoUrl
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)

        'If Not IsPostBack Then
        If Request(Trim("po_from")) = "admin" Then
            VIEWSTATE("po_from") = Request(Trim("po_from"))
            displayPOReport2()
            Bindgrid()
        Else
            VIEWSTATE("po_from") = ""
            If Request.QueryString("freeze") = "1" Then
                blnFreeze = True
                Freeze(Session.SessionID & "_PO.htm")
            Else
                blnFreeze = False
            End If
            
            displayPOReport()
            Bindgrid()
        End If
        'End If
        'Image1.ImageUrl = dDispatcher.direct("Plugins/images", "logo_tx123_2.jpg")
    End Sub

    Private Function getBusinessRegistrationNo(ByVal strBCoyId As String, ByVal strVendorId As String)
        Dim objComp As New Company
        Dim objComps As New Companies
        objComp = objComps.GetCompanyDetails(strBCoyId)
        ' get buyer company registeration number
        lblBusiness.Text = objComp.BusinessRegNo

        ' get vendor company registeration number
        objComp = objComps.GetCompanyDetails(strVendorId)
        lblTaxNo.Text = objComp.BusinessRegNo

        objComp = Nothing
        objComps = Nothing
    End Function

    Private Function displayPOReport()
        Dim objpo As New PurchaseOrder
        Dim objval As New POValue
        objval.PO_Number = Request(Trim("po_no"))
        objval.buyer_coy = Request(Trim("BCoyID"))

        '//add by Moo, to cater for PR consolidation
        If Request.QueryString("prev") <> "" Then
            blnPreviewPO = Request.QueryString("prev")
            'objval.PO_Number = objpo.get_ponum(Request(Trim("po_no"))) 'po_no = po_index
        Else
            blnPreviewPO = False
        End If
        ' Request(Trim("side")) not used already
        objpo.get_PODetail(objval, Request(Trim("side")), blnPreviewPO)
        lblComName.Text = objval.BCoyName
        ' Request(Trim("side")) not used already
        lblAddr.Text = objpo.get_comadd(objval, Request(Trim("side")), blnPreviewPO)
        'lblPONo.Text = "<B>" & objval.PO_Number & "</B>"
        lblPONo.Text = objval.PO_Number
        lblDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, objval.PO_Date)
        lblPayTerm.Text = objval.pay_term
        lblPayMethod.Text = objval.pay_meth
        lblShipTerm.Text = objval.ship_term
        lblShipMode.Text = objval.ship_meth
        'lblFreighTerm.Text = objval.f
        lblShipVia.Text = objval.ship_via
        lblBuyer.Text = objval.buyer_contact
        lblBuyerEmail.Text = objval.buyer_email
        lblTel.Text = objval.buyer_tel
        'Me.lbl_fax.Text = objval.buyer_fax
        getBusinessRegistrationNo(objval.buyer_coy, objval.vendor_CoyId)
        lblVendorName.Text = objval.vendor_Coy
        lblVendorName2.Text = objval.BCoyName
        lblVAddr.Text = objval.vendor_adds
        lblVendorTel.Text = objval.vendor_tel
        'lblVendorFax.Text = objval.vendor_fax
        lblVendorEmail.Text = objval.vendor_email
        'lblCountry.Text=objval.
        lblShipTo.Text = "See Below" 'objval.buyer_adds
        'lblTaxNo.Text = objval.tax
        lblBillTo.Text = objval.bill_adds
        'lblAttn.Text = objval.vendor_contact
        lblRemark.Text = objval.remarks
        lblFreighTerm.Text = objval.FreightTerm

        '//add By Moo, for Logo
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        strImgSrc = objFile.getCoyLogo(EnumUploadFrom.FrontOff, Request(Trim("BCoyID")))
        If strImgSrc <> "" Then
            Image1.Visible = True

            Dim bitmapImg As System.Drawing.Image = System.Drawing.Image.FromFile(Server.MapPath(strImgSrc))
            If bitmapImg.Width < Image1.Width.Value Then
                Image1.Width = System.Web.UI.WebControls.Unit.Pixel(bitmapImg.Width)
            End If

            If blnFreeze Then
                Image1.ImageUrl = "file:///" & Server.MapPath(strImgSrc)
            Else
                Image1.ImageUrl = strImgSrc
            End If
        Else
            Image1.Visible = False
        End If

        objFile = Nothing
    End Function

    Private Function displayPOReport2()

        Dim objrfq As New RFQ
        lblComName.Text = Request(Trim("com_id")) 'objrfq.get_comname(Session("CompanyID"))
        lblAddr.Text = "XXXXX"
        'lblPONo.Text = "<B>" & objval.PO_Number & "</B>"
        lblPONo.Text = "PO/XXXXX"
        lblDate.Text = "DD/MM/YYYY"
        lblPayTerm.Text = "XX Days"
        lblPayMethod.Text = "XXXXX"
        lblShipTerm.Text = "XXXXX"
        lblShipMode.Text = "XXXXX"
        'lblFreighTerm.Text = objval.f
        lblShipVia.Text = "XXXXX"
        lblBusiness.Text = "XXXXX"
        lblBuyer.Text = "XXXXX"
        lblBuyerEmail.Text = "XXXXX"
        lblTel.Text = "XXXXX"
        lblVendorName.Text = "XXXXX"
        lblVendorName2.Text = "XXXXX"
        lblVAddr.Text = "XXXXX"
        lblVendorTel.Text = "XXXXX"
        'lblVendorFax.Text = "XXXXX"
        lblVendorEmail.Text = "XXXXX"
        'lblCountry.Text=objval.
        lblShipTo.Text = "XXXXX"
        lblTaxNo.Text = "XXXXX"
        lblBillTo.Text = "XXXXX"
        'lblAttn.Text = "XXXXX"
        lblRemark.Text = "XXXXX"
        lblFreighTerm.Text = "XXXXX"

        '//add By Moo, for Logo
        'Dim objFile As New FileManagement
        If Request.QueryString("img") <> "" Then
            Image1.ImageUrl = Request.QueryString("img")
        Else
            Dim objFile As New FileManagement
            Dim strImgSrc As String
            'strImgSrc = objFile.getCoyLogo(EnumUploadFrom.FrontOff, Request(Trim("BCoyID")))
            strImgSrc = objFile.getCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyId"))

            If strImgSrc <> "" Then
                Image1.Visible = True
                Image1.ImageUrl = strImgSrc
            Else
                Image1.Visible = False
            End If
            objFile = Nothing
        End If
    End Function

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgPOReport.SortCommand
        'Grid_SortCommand(sender, e)
        'dtgCustomField.CurrentPageIndex = 0
        ' Bindgrid(0, True)
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String

        'Dim objval As New POValue
        Dim ds As New DataSet
        Dim strPOCode As String
        strPOCode = Me.lblPONo.Text
        Dim dtr As DataRow
        Dim crdetail As New DataTable
        If VIEWSTATE("po_from") = "admin" Then

            crdetail.Columns.Add("POD_PO_LINE", Type.GetType("System.String"))
            crdetail.Columns.Add("POD_Product_Desc", Type.GetType("System.String"))
            crdetail.Columns.Add("POD_ORDERED_QTY", Type.GetType("System.String"))
            crdetail.Columns.Add("POD_UOM", Type.GetType("System.String"))
            crdetail.Columns.Add("POD_Min_Pack_Qty", Type.GetType("System.String"))
            crdetail.Columns.Add("POD_Min_Order_Qty", Type.GetType("System.String"))
            crdetail.Columns.Add("POD_UNIT_COST", Type.GetType("System.String"))
            crdetail.Columns.Add("amt", Type.GetType("System.String"))
            crdetail.Columns.Add("POD_GST", Type.GetType("System.String"))
            crdetail.Columns.Add("POD_ETD", Type.GetType("System.String"))
            crdetail.Columns.Add("POD_WARRANTY_TERMS", Type.GetType("System.String"))

            dtr = crdetail.NewRow()
            dtr("POD_PO_LINE") = "XXXXX"
            dtr("POD_Product_Desc") = "XXXXX"
            dtr("POD_ORDERED_QTY") = "XXXXX"
            dtr("POD_UOM") = "XXXXX"
            dtr("POD_Min_Pack_Qty") = "XXXXX"
            dtr("POD_Min_Order_Qty") = "XXXXX"
            dtr("POD_UNIT_COST") = "XXXXX"
            dtr("amt") = "XXXXX"
            dtr("POD_GST") = "XXXXX"
            dtr("POD_ETD") = "XXXXX"
            dtr("POD_WARRANTY_TERMS") = "XXXXX"
            crdetail.Rows.Add(dtr)

            ds.Tables.Add(crdetail)
        Else
            Dim objpo As New PurchaseOrder
            ds = objpo.getlineitem(strPOCode, Session("side"), blnPreviewPO, Request(Trim("BCoyID")))
            If ds.Tables(0).Rows.Count > 0 Then
                strCurrCode = ds.Tables(0).Rows(0)("POM_CURRENCY_CODE")
                Dim objPR As New PurchaseReq2
                If objPR.Need2PrintCField(ds.Tables(0).Rows(0)("POD_PR_INDEX")) Then
                    blnPrintCustField = True
                Else
                    blnPrintCustField = False
                End If
                If objPR.Need2PrintRemark(ds.Tables(0).Rows(0)("POD_PR_INDEX")) Then
                    blnPrintRemark = True
                Else
                    blnPrintRemark = False
                End If
                dtAddr = ds.Tables(0)
            End If
        End If

        Dim dvViewSample As DataView

        'ds.Tables(0).Rows(1)(2)
        dvViewSample = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)
        If pSorted Then
            dvViewSample.Sort = viewstate("SortExpression")
            If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        End If

        dtgPOReport.DataSource = dvViewSample
        dtgPOReport.DataBind()
        If viewstate("po_from") = "admin" Then
            viewstate("total") = 0
            viewstate("gst") = 0
        End If
        AddRowtotal(6)
        objGlobal = Nothing
    End Function

    Private Sub dtgPOReport_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPOReport.ItemDataBound
        If i = 0 Then
            'Dim ds As DataSet
            'Dim strPOCode As String
            'Dim money As String
            If viewstate("po_from") = "admin" Then
                strCurrCode = "XXX"
                'Else
                '    strPOCode = Me.lblPONo.Text
                '    Dim objpo As New PurchaseOrder
                '    'ds = objpo.getlineitem(strPOCode, Session("side"), blnPreviewPO, Request(Trim("BCoyID")))
                '    'money = ds.Tables(0).Rows(0)("POM_CURRENCY_CODE")
            End If

            AddRowmoney(6, strCurrCode)
            DrawLine()
            '//add address just before first line item
            '//need to put here because must add after AddRowmoney(6, strCurrCode)
            If viewstate("po_from") <> "admin" Then
                AddAddressRow(0)
                strDelCode = dtAddr.Rows(0)("POD_D_ADDR_CODE")
            End If
        End If

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objpo As New PurchaseOrder
            Dim objval As New POValue
            Dim po_line As String
            Dim po_index As String
            Dim j As Integer


            e.Item.Cells(3).Text = ""
            If viewstate("po_from") = "admin" Then
                '//to be called from "Preview PO Template" at company detail screen
                '//Please dont put any code with DATABASEFIELD outside this condition
                e.Item.Cells(7).Text = "XXXXX"
                e.Item.Cells(8).Text = "XXXXX"
                e.Item.Cells(9).Text = "XXXXX"
                AddRow2(1, 1, "XXXXX")
            Else
                'POD_VENDOR_ITEM_CODE
                ' ai chu add on 11/10/2005
                ' SRU30035 - include vendor item code under description column
                e.Item.Cells(1).Text = Common.parseNull(dv("POD_VENDOR_ITEM_CODE")) & " " & Common.parseNull(dv("POD_PRODUCT_DESC"))
                e.Item.Cells(7).Text = Format(dv("POD_UNIT_COST"), "#,##0.0000")
                e.Item.Cells(8).Text = Format(dv("POD_ORDERED_QTY") * dv("POD_UNIT_COST"), "#,##0.0000")
                e.Item.Cells(9).Text = Format((dv("POD_GST") / 100) * dv("POD_ORDERED_QTY") * dv("POD_UNIT_COST"), "#,##0.0000")

                viewstate("gst") = viewstate("gst") + (dv("POD_GST") / 100) * dv("POD_ORDERED_QTY") * dv("POD_UNIT_COST")
                viewstate("total") = viewstate("total") + (dv("POD_ORDERED_QTY") * dv("POD_UNIT_COST"))
                '//for PR consolidation, 1 PO -> many PR
                '//assume PR with different setting can't be consolidated
                If blnPrintRemark Or blnPrintCustField Then
                    If blnPrintCustField Then

                        '---New Code Added To Get The POline By Praveen on 16/07/2007    
                        e.Item.Cells(0).Text = e.Item.DataSetIndex + 1
                        'End by Praveen
                        objpo.get_customfield(value1, value2, dv("POD_PR_LINE"), dv("POD_PR_INDEX"), j)
                    End If
                    AddRow(1, value1, value2, j, dv("POD_REMARK"))
                End If
                If IsDBNull(dv("POD_ETD")) OrElse CStr(dv("POD_ETD")) = "0" Then
                    e.Item.Cells(10).Text = "Ex-Stock"
                Else
                    e.Item.Cells(10).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, DateAdd("D", dv("POD_ETD"), dv("POM_PO_DATE")))
                End If
            End If
            'jLoop = jLoop + 1
            'AddRemark(1, dv("POD_REMARK"))
        End If
        i = i + 1
    End Sub

    Sub AddRow2(ByVal intCell As Integer, ByVal j As Integer, ByVal remark As String)
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String

        For count = 0 To j - 1
            strtext = strtext & "<B>XXXXX</B> : XXXXX "
            strtext = strtext & "<br>"
        Next
        For intL = 0 To Me.dtgPOReport.Columns.Count - 1
            addCell(row)
        Next

        For intL = 0 To dtgPOReport.Columns.Count - 3
            row.Cells.RemoveAt(1)
        Next

        'row.Cells(0).ColumnSpan = intCell - 1
        Dim lbl_test As New Label
        lbl_test.ID = "test"
        lbl_test.Text = strtext
        lbl_test.CssClass = "txtbox"
        lbl_test.Width = System.Web.UI.WebControls.Unit.Pixel(500)
        lbl_test.Font.Bold = False
        row.Cells(1).Controls.Add(lbl_test)
        row.Cells(1).HorizontalAlign = HorizontalAlign.Left
        row.Cells(1).ColumnSpan = 10

        Me.dtgPOReport.Controls(0).Controls.Add(row)

        For intL = 0 To dtgPOReport.Columns.Count - 1
            addCell(row2)
        Next

        For intL = 0 To dtgPOReport.Columns.Count - 3
            row2.Cells.RemoveAt(1)
        Next

        ' row.Cells(0).ColumnSpan = intCell - 1
        Dim lbl_test2 As New Label
        lbl_test2.ID = "test2"
        lbl_test2.Text = "<B>Remarks</B> : " & remark & ""
        lbl_test2.CssClass = "txtbox"
        lbl_test2.Width = System.Web.UI.WebControls.Unit.Pixel(500)
        lbl_test2.Font.Bold = False
        lbl_test2.Font.Bold = False

        row2.Cells(1).Controls.Add(lbl_test2)
        row2.Cells(1).ColumnSpan = 10
        row2.Cells(1).HorizontalAlign = HorizontalAlign.Left
        Me.dtgPOReport.Controls(0).Controls.Add(row2)
    End Sub
    Sub AddRow(ByVal intCell As Integer, ByVal value1() As String, ByVal value2() As String, ByVal j As Integer, ByVal remark As String)
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String

        '//TO PRINT CUSTOM FIELD
        '//ONLY SHOW CUSTOM FIELD IF IT IS CHECKED IN PR
        If blnPrintCustField Then
            For count = 0 To j - 1
                strtext = strtext & "<B>" & value1(count) & "</B> : " & value2(count) & ""
                strtext = strtext & "<br>"
            Next
            For intL = 0 To Me.dtgPOReport.Columns.Count - 1
                addCell(row)
            Next

            For intL = 1 To dtgPOReport.Columns.Count - 2
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

            Me.dtgPOReport.Controls(0).Controls.Add(row)
        End If

        If blnPrintRemark Then
            For intL = 0 To dtgPOReport.Columns.Count - 1
                addCell(row2)
            Next

            For intL = 1 To dtgPOReport.Columns.Count - 2
                row2.Cells.RemoveAt(1)
            Next

            ' row.Cells(0).ColumnSpan = intCell - 1
            Dim lbl_test2 As New Label
            lbl_test2.ID = "test2"
            lbl_test2.Text = "<B>Remarks</B> : " & remark & ""
            ' lbl_test2.Text = "Remarks : "
            lbl_test2.CssClass = "txtbox"
            lbl_test2.Width = System.Web.UI.WebControls.Unit.Pixel(500)
            lbl_test2.Font.Bold = False

            row2.Cells(1).ColumnSpan = 10
            row2.Cells(1).Controls.Add(lbl_test2)
            row2.Cells(1).HorizontalAlign = HorizontalAlign.Left

            'Dim lbl_test3 As New Label
            'lbl_test3.ID = "test3"
            'lbl_test3.Text = remark
            'lbl_test3.CssClass = "txtbox"
            'lbl_test3.Width = System.Web.UI.WebControls.Unit.Pixel(500)
            'lbl_test3.Font.Bold = False
            'row2.Cells(2).Controls.Add(lbl_test3)
            'row2.Cells(2).HorizontalAlign = HorizontalAlign.Left

            Me.dtgPOReport.Controls(0).Controls.Add(row2)


        End If
    End Sub

    Sub AddAddressRow(ByVal intRow As Integer)
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String

        For intL = 0 To dtgPOReport.Columns.Count - 1
            addCell(row2)
        Next

        For intL = 1 To dtgPOReport.Columns.Count - 1
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
        row2.Cells(0).ColumnSpan = 11
        row2.Cells(0).Controls.Add(lbl_test2)
        row2.Cells(0).HorizontalAlign = HorizontalAlign.Left
        Me.dtgPOReport.Controls(0).Controls.Add(row2)

    End Sub

    Sub AddShipmentAddressRow(ByVal intRow As Integer)
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String

        For intL = 0 To dtgPOReport.Columns.Count - 1
            addCell(row2)
        Next

        For intL = 1 To dtgPOReport.Columns.Count - 1
            row2.Cells.RemoveAt(1)
        Next
        ' row.Cells(0).ColumnSpan = intCell - 1

        Dim strTempAddr As String
        strTempAddr = "XXXXX" 'Common.parseNull(dtAddr.Rows(intRow)("POD_D_ADDR_LINE1"))

        strTempAddr = strTempAddr & " " & "XXXXX" 'dtAddr.Rows(intRow)("POD_D_ADDR_LINE2")
        strTempAddr = strTempAddr & " " & "XXXXX" 'dtAddr.Rows(intRow)("POD_D_ADDR_LINE3")
        strTempAddr = strTempAddr & " " & "XXXXX" 'dtAddr.Rows(intRow)("POD_D_POSTCODE")
        strTempAddr = strTempAddr & " " & "XXXXX" 'dtAddr.Rows(intRow)("POD_D_CITY")
        strTempAddr = strTempAddr & " " & "XXXXX" 'objGlobal.getCodeDesc(CodeTable.State, dtAddr.Rows(intRow)("POD_D_STATE"))
        strTempAddr = strTempAddr & " " & "XXXXX" 'objGlobal.getCodeDesc(CodeTable.Country, dtAddr.Rows(intRow)("POD_D_COUNTRY"))

        Dim lbl_test2 As New Label
        lbl_test2.ID = "test2"
        lbl_test2.Text = "Ship To : " & "XXXXX" & " - " & strTempAddr 'Common.parseNull(dtAddr.Rows(intRow)("POD_D_ADDR_CODE")) & " - " & strTempAddr

        lbl_test2.CssClass = "txtbox"
        'lbl_test2.Width = System.Web.UI.WebControls.Unit.Pixel(500)
        lbl_test2.Font.Bold = True
        row2.Cells(0).ColumnSpan = 11
        row2.Cells(0).Controls.Add(lbl_test2)
        row2.Cells(0).HorizontalAlign = HorizontalAlign.Left
        Me.dtgPOReport.Controls(0).Controls.Add(row2)

    End Sub

    Sub DrawLine()
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        Dim intTotalCol As Integer
        Dim count As Integer
        Dim strtext As String

        For intL = 0 To dtgPOReport.Columns.Count - 1
            addCell(row2)
        Next

        For intL = 1 To dtgPOReport.Columns.Count - 1
            row2.Cells.RemoveAt(1)
        Next
        ' row.Cells(0).ColumnSpan = intCell - 1

        Dim lbl_test2 As New Label
        lbl_test2.ID = "lblHeaderLine"
        lbl_test2.Text = "<HR>"

        'lbl_test2.CssClass = "txtbox"
        'lbl_test2.Width = System.Web.UI.WebControls.Unit.Pixel(500)
        lbl_test2.Font.Bold = True
        row2.Cells(0).ColumnSpan = 12
        row2.Cells(0).Controls.Add(lbl_test2)
        row2.Cells(0).HorizontalAlign = HorizontalAlign.Left
        Me.dtgPOReport.Controls(0).Controls.Add(row2)

    End Sub
    Sub AddRowtotal(ByVal intCell As Integer)
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row3 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row4 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        For intL = 0 To Me.dtgPOReport.Columns.Count - 1
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
        row.Cells(7).Text = "Sub Total:"
        row.Cells(7).HorizontalAlign = HorizontalAlign.Right
        row.Cells(7).Font.Bold = True
        row.Cells(7).Wrap = False

        row.Cells(8).Text = Format(viewstate("total"), "#,##0.00")
        row.Cells(8).HorizontalAlign = HorizontalAlign.Right
        row.Cells(8).Wrap = False
        ' row.Cells(7).Font.Bold = True

        'row.Cells(9).Text = Format(viewstate("gst"), "#,##0.00")
        'row.Cells(9).HorizontalAlign = HorizontalAlign.Right
        ''row.Cells(8).ColumnSpan = 10

        Me.dtgPOReport.Controls(0).Controls.Add(row)

        For intL = 0 To Me.dtgPOReport.Columns.Count - 1
            addCell(row2)
        Next
        row2.Cells(7).Text = "Tax:"
        row2.Cells(7).HorizontalAlign = HorizontalAlign.Right
        row2.Cells(7).Font.Bold = True
        row2.Cells(7).Wrap = False
        'row2.Cells(7).Text = Format(viewstate("total") + viewstate("gst"), "#,##0.00")        
        row2.Cells(8).Text = Format(CDbl(Format(viewstate("total"), "#0.00")) + CDbl(Format(viewstate("gst"), "#0.00")), "#,##0.00")
        row2.Cells(8).HorizontalAlign = HorizontalAlign.Right
        row2.Cells(8).Wrap = False
        Me.dtgPOReport.Controls(0).Controls.Add(row2)

        For intL = 0 To Me.dtgPOReport.Columns.Count - 1
            addCell(row3)
        Next
        row3.Cells(7).Text = "Shipping & Handling:"
        row3.Cells(7).HorizontalAlign = HorizontalAlign.Right
        row3.Cells(7).Font.Bold = True
        row3.Cells(7).Wrap = False
        'row2.Cells(7).Text = Format(viewstate("total") + viewstate("gst"), "#,##0.00")        
        row3.Cells(8).Text = Format(CDbl(Format(ViewState("total"), "#0.00")) + CDbl(Format(ViewState("gst"), "#0.00")), "#,##0.00")
        row3.Cells(8).HorizontalAlign = HorizontalAlign.Right
        row3.Cells(8).Wrap = False
        Me.dtgPOReport.Controls(0).Controls.Add(row3)

        For intL = 0 To Me.dtgPOReport.Columns.Count - 1
            addCell(row4)
        Next
        row4.Cells(7).Text = "Grand Total:"
        row4.Cells(7).HorizontalAlign = HorizontalAlign.Right
        row4.Cells(7).Font.Bold = True
        row4.Cells(7).Wrap = False
        'row2.Cells(7).Text = Format(viewstate("total") + viewstate("gst"), "#,##0.00")        
        row4.Cells(8).Text = Format(CDbl(Format(ViewState("total"), "#0.00")) + CDbl(Format(ViewState("gst"), "#0.00")), "#,##0.00")
        row4.Cells(8).HorizontalAlign = HorizontalAlign.Right
        row4.Cells(8).Wrap = False
        Me.dtgPOReport.Controls(0).Controls.Add(row4)
    End Sub
    Sub AddRowmoney(ByVal intCell As Integer, ByVal money As String)
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        For intL = 0 To Me.dtgPOReport.Columns.Count - 1
            addCell(row)
        Next

        'For intL = 0 To 1
        '    row.Cells.RemoveAt(1)
        'Next
        'row.Cells(0).ColumnSpan = intCell - 1
        row.Cells(7).Text = "(" & money & ")"
        row.Cells(7).HorizontalAlign = HorizontalAlign.Right

        'row.Cells(6).ColumnSpan = 10

        row.Cells(8).Text = "(" & money & ")"
        row.Cells(8).HorizontalAlign = HorizontalAlign.Right
        ' row.Cells(7).ColumnSpan = 10

        row.Cells(9).Text = "(" & money & ")"
        row.Cells(9).HorizontalAlign = HorizontalAlign.Right
        'row.Cells(8).ColumnSpan = 10

        Me.dtgPOReport.Controls(0).Controls.Add(row)
    End Sub



    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)

    End Sub

    Private Sub dtgPOReport_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPOReport.ItemCreated
        '//iLoop - Looping for all Items(Header,item,footer)
        '//jLoop - Looping for Items
        If viewstate("po_from") <> "admin" Then
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
                'If Not blnDrawLine Then
                '    DrawLine()
                '    blnDrawLine = True
                'End If
            ElseIf e.Item.ItemType = ListItemType.Header Then

                'Dim i As Integer
                'For i = 0 To dtgPOReport.Columns.Count - 1
                '    e.Item.Cells(i).Text = "<U>" & e.Item.Cells(i).Text & "</U>"
                'Next
            End If
            iLoop += 1
        Else
            'AddShipmentAddressRow(0)
        End If
    End Sub

   
End Class
