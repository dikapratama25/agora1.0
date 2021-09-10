Imports AgoraLegacy
Imports eProcure.Component
Public Class PProductDetail
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents lbl_unspsc As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_prodesc As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_desc As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_uom As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld1txt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld2txt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld3txt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld4txt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld2 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld3 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld4 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_prefer As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_prefertax As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_1st As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_1sttax As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_2nd As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_2ndtax As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_3rd As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_3rdtax As System.Web.UI.WebControls.Label
    Protected WithEvents lblCatCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblAccCode As System.Web.UI.WebControls.Label

    Protected WithEvents lbl_leadP As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_vendorP As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_lead1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_vendor1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_lead2 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_vendor2 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_lead3 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_vendor3 As System.Web.UI.WebControls.Label
    Protected WithEvents lblManufacturerName As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_venitemcode As System.Web.UI.WebControls.Label
    'Protected WithEvents lbl_ven As System.Web.UI.WebControls.Label
    'Protected WithEvents lbl_brand As System.Web.UI.WebControls.Label
    'Protected WithEvents lbl_model As System.Web.UI.WebControls.Label
    Protected WithEvents MyDataGrid As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtgTrx As System.Web.UI.WebControls.DataGrid
    Protected WithEvents divHist As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents divVendor As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents trQC As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trItemType As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trfld4 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trfld3 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tdTax As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents tdPreferTax As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents td1stTax As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents td2ndTax As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents td3rdTax As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents divItemSpec As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents txtManufacturerName As System.Web.UI.WebControls.Label
    Protected WithEvents txtBrand As System.Web.UI.WebControls.Label
    Protected WithEvents txtModel As System.Web.UI.WebControls.Label
    Protected WithEvents txtDrawingNumber As System.Web.UI.WebControls.Label
    Protected WithEvents txtVersionNo As System.Web.UI.WebControls.Label
    Protected WithEvents txtGrossWeight As System.Web.UI.WebControls.Label
    Protected WithEvents txtNetWeight As System.Web.UI.WebControls.Label
    Protected WithEvents txtLength As System.Web.UI.WebControls.Label
    Protected WithEvents txtWidth As System.Web.UI.WebControls.Label
    Protected WithEvents txtHeight As System.Web.UI.WebControls.Label
    Protected WithEvents txtVolume As System.Web.UI.WebControls.Label
    Protected WithEvents txtColorInfo As System.Web.UI.WebControls.Label
    Protected WithEvents txtHSCode As System.Web.UI.WebControls.Label
    Protected WithEvents txtPacking As System.Web.UI.WebControls.Label
    Protected WithEvents txtRemarks As System.Web.UI.WebControls.Label
    'Protected WithEvents cmd_back As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents cmd_back As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents dtgContractInfo As System.Web.UI.WebControls.DataGrid
    Protected WithEvents divContractInfo As System.Web.UI.HtmlControls.HtmlGenericControl

    Protected WithEvents lbl_itemtype As System.Web.UI.WebControls.Label
    Protected WithEvents lblQC As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_needqc As System.Web.UI.WebControls.Label

    Dim clsproduct As New Products
    Protected WithEvents tdAttach As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents tdImage As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents lblProdImage As System.Web.UI.WebControls.Label
    Dim strType As String
    Dim objFile As New FileManagement
    Dim dDispatcher As New AgoraLegacy.dispatcher

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

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)

        If Not IsPostBack Then
            Dim objGlobal As New AppGlobals
            Dim dsproduct As DataSet
            Dim objcall As New Products
            Dim objGst As New GST
            Dim strImgSrc As String
            Dim strCompanyType As String
            Dim strtxproid As String
            Dim strCoyId As String
            Dim strGrpIndex As String
            Dim strDraft As String
            Dim strRef As String
            Dim dsConInfo As DataSet
            blnSorting = False
            blnPaging = False
            blnCheckBox = False
            SetGridProperty(MyDataGrid)
            ViewState("GSTCOD") = objGst.chkGSTCOD()
            If Request.QueryString("ContCatType") = "Vendor" Then
                tdImage.RowSpan = 11
                VendorProd()
                Exit Sub
            End If
            strtxproid = Request.QueryString("pid")
            strCompanyType = Request.QueryString("companyType")
            strGrpIndex = Request.QueryString("index")
            strDraft = Request.QueryString("draft")
            strRef = Request.QueryString("Ref")
            dsproduct = objcall.getProDetails(strtxproid, Session("CompanyId"), IIf(strCompanyType = "B", True, False), strGrpIndex, IIf(strDraft = "1", True, False))
            dsConInfo = objcall.GetContractInfo(strtxproid, strRef)

            If dsproduct.Tables(0).Rows.Count > 0 Then
                strCoyId = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_S_COY_ID"))
                lbl_unspsc.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("CT_NAME"))
                lbl_prodesc.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PRODUCT_DESC"))
                lbl_venitemcode.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_VENDOR_ITEM_CODE"))
                lbl_desc.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_LONG_DESC"))
                lbl_uom.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_UOM"))
                lblCatCode.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_CAT_CODE"))
                lblAccCode.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_ACCT_CODE"))

                If Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PRODUCT_IMAGE")) <> "" Then
                    lblProdImage.Visible = True

                    Dim objFile As New FileManagement
                    strImgSrc = objFile.FileDownload(dsproduct.Tables(0).Rows(0)("PM_PRODUCT_IMAGE"), EnumDownLoadType.ProductImage, "", EnumUploadFrom.FrontOff, strCoyId)
                    objFile = Nothing

                Else
                    lblProdImage.Visible = False
                End If

                If Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_ITEM_TYPE")) = "SP" Then
                    lbl_itemtype.Text = "Spot (Non-Inventoried item)"
                    'Page.Form.FindControl("displayqc").Visible = False
                    'Me.trQC.Style("display") = ""

                ElseIf Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_ITEM_TYPE")) = "ST" Then
                    lbl_itemtype.Text = "Stock (Direct material - Inventoried item)"
                    If Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_IQC_IND")) = "Y" Then
                        lbl_needqc.Text = "Yes"
                    Else
                        lbl_needqc.Text = "No"
                    End If
                Else
                    lbl_itemtype.Text = "MRO, M&E and IT (Inventoried item)"
                    If Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_IQC_IND")) = "Y" Then
                        lbl_needqc.Text = "Yes"
                    Else
                        lbl_needqc.Text = "No"
                    End If
                End If
            End If

            MyDataGrid.DataSource = dsproduct.Tables(1).DefaultView
            MyDataGrid.DataBind()
            strType = ""


            If dsproduct.Tables(2).Rows.Count > 0 Then
                Dim dtAttach As DataTable
                Dim intCnt, intLoop, i As Integer
                Dim strTemp As String = ""
                Dim strFile, strFile1, strURL As String
                dtAttach = dsproduct.Tables(2)
                intCnt = dtAttach.Rows.Count - 1
                i = 0
                For intLoop = 0 To intCnt
                    strFile = dtAttach.Rows(intLoop)("PA_ATTACH_FILENAME")
                    strFile1 = dtAttach.Rows(intLoop)("PA_HUB_FILENAME")
                    strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.ProductAttach, "", EnumUploadFrom.FrontOff, , strCoyId)
                    If CStr(dtAttach.Rows(intLoop)("PA_ATTACH_INDEX")) = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PRODUCT_IMAGE")) Then
                        strImgSrc = objFile.FileDownload(Server.UrlEncode(strFile1), EnumDownLoadType.ProductImage, "", EnumUploadFrom.FrontOff, dsproduct.Tables(0).Rows(0)("PM_S_COY_ID"))
                        lblProdImage.Text = "<a href='javascript:PreviewImage(""" &
                                            Replace(strImgSrc, "\", "\\") & """);'>" _
                                            & "<img border='0' src='" &
                                            strImgSrc & "' height='150' width='150'></a>"
                        'strImgSrc & "' height='200' width='200'></a>"
                        lblProdImage.ToolTip = "Click here to view actual image"
                    End If

                    '*************************meilai************************************

                    If dtAttach.Rows(intLoop)("PA_TYPE") = "D" Then
                        i = i + 1
                        If strTemp = "" Then
                            strTemp = "&nbsp;" & i & ") " & strURL
                        Else
                            strTemp = strTemp & "<BR>&nbsp;" & i & ") " & strURL
                        End If
                    End If
                Next
                tdAttach.InnerHtml = strTemp
            End If
            If lblProdImage.Text = "" Then
                lblProdImage.Visible = False
            End If

            'Check whether it is VIM or PIM
            If Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PRODUCT_FOR")) = "B" Then
                Dim ds As New DataSet
                Dim dvViewSample As DataView

                lbl_fld1txt.Text = "Order Quantity (Min) "
                lbl_fld2txt.Text = "Order Quantity (Max) "
                lbl_fld3txt.Text = "Safety Level (Min Inventory) "
                lbl_fld4txt.Text = "Max Inventory Quantity"
                lbl_fld1.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_ORD_MIN_QTY"))
                lbl_fld2.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_ORD_MAX_QTY"))
                lbl_fld3.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_SAFE_QTY"))
                lbl_fld4.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_MAX_INV_QTY"))
                If Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_ITEM_TYPE")) = "SP" Then
                    Me.trQC.Style("display") = "none"
                    'Me.trItemType.Style("display") = "none"
                End If

                lbl_prefer.Text = Common.parseNull(dsproduct.Tables(3).Rows(0)("PREFER_VENDOR"))
                lbl_prefertax.Text = Common.parseNull(dsproduct.Tables(3).Rows(0)("PREFER_VENDOR_TAX"))
                lbl_1st.Text = Common.parseNull(dsproduct.Tables(3).Rows(0)("1st_VENDOR"))
                lbl_1sttax.Text = Common.parseNull(dsproduct.Tables(3).Rows(0)("1st_VENDOR_TAX"))
                lbl_2nd.Text = Common.parseNull(dsproduct.Tables(3).Rows(0)("2nd_VENDOR"))
                lbl_2ndtax.Text = Common.parseNull(dsproduct.Tables(3).Rows(0)("2nd_VENDOR_TAX"))
                lbl_3rd.Text = Common.parseNull(dsproduct.Tables(3).Rows(0)("3rd_VENDOR"))
                lbl_3rdtax.Text = Common.parseNull(dsproduct.Tables(3).Rows(0)("3rd_VENDOR_TAX"))

                If ViewState("GSTCOD") = True Then
                    tdTax.Style("display") = "none"
                    tdPreferTax.Style("display") = "none"
                    td1stTax.Style("display") = "none"
                    td2ndTax.Style("display") = "none"
                    td3rdTax.Style("display") = "none"
                Else
                    tdTax.Style("display") = ""
                    tdPreferTax.Style("display") = ""
                    td1stTax.Style("display") = ""
                    td2ndTax.Style("display") = ""
                    td3rdTax.Style("display") = ""
                End If

                If dsproduct.Tables(4).Rows.Count > 0 Then
                    Dim count As Integer
                    For count = 0 To dsproduct.Tables(4).Rows.Count - 1
                        Dim vType As String
                        vType = Common.parseNull(dsproduct.Tables(4).Rows(count).Item("PV_VENDOR_TYPE"))

                        If vType = "P" Then
                            lbl_leadP.Text = Common.parseNull(dsproduct.Tables(4).Rows(count).Item("PV_LEAD_TIME"))
                            lbl_vendorP.Text = Common.parseNull(dsproduct.Tables(4).Rows(count).Item("PV_VENDOR_CODE"))
                            'lbl_leadP.Text = IIf(lbl_leadP.Text = "", "N/A", lbl_leadP.Text)
                            'lbl_vendorP.Text = IIf(lbl_vendorP.Text = "", "N/A", lbl_vendorP.Text)
                        ElseIf vType = "1" Then
                            lbl_lead1.Text = Common.parseNull(dsproduct.Tables(4).Rows(count).Item("PV_LEAD_TIME"))
                            lbl_vendor1.Text = Common.parseNull(dsproduct.Tables(4).Rows(count).Item("PV_VENDOR_CODE"))
                        ElseIf vType = "2" Then
                            lbl_lead2.Text = Common.parseNull(dsproduct.Tables(4).Rows(count).Item("PV_LEAD_TIME"))
                            lbl_vendor2.Text = Common.parseNull(dsproduct.Tables(4).Rows(count).Item("PV_VENDOR_CODE"))
                        ElseIf vType = "3" Then
                            lbl_lead3.Text = Common.parseNull(dsproduct.Tables(4).Rows(count).Item("PV_LEAD_TIME"))
                            lbl_vendor3.Text = Common.parseNull(dsproduct.Tables(4).Rows(count).Item("PV_VENDOR_CODE"))
                        End If
                    Next
                End If

                ds = objcall.getTxnPrice(strtxproid)
                'dvViewSample = ds.Tables(0).DefaultView
                dtgTrx.DataSource = ds.Tables(0).DefaultView
                dtgTrx.DataBind()

                dtgContractInfo.DataSource = dsConInfo.Tables(0).DefaultView
                dtgContractInfo.DataBind()
                If ViewState("GSTCOD") = True Then
                    dtgContractInfo.Columns(6).Visible = False
                    dtgContractInfo.Columns(7).Visible = True
                Else
                    dtgContractInfo.Columns(6).Visible = True
                    dtgContractInfo.Columns(7).Visible = False
                End If
                Me.divContractInfo.Style("display") = ""

                'lbl_fld1.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("TAX_PERC"))
                'lbl_fld2.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_CURRENCY_CODE")) & " " & Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_UNIT_COST"))
                'lbl_fld3.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_REF_NO"))
                txtManufacturerName.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_MANUFACTURER"))
                txtBrand.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PRODUCT_BRAND"))
                txtModel.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PRODUCT_MODEL"))
                txtDrawingNumber.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_DRAW_NO"))
                txtVersionNo.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_VERS_NO"))
                txtGrossWeight.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_GROSS_WEIGHT"))
                txtNetWeight.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_NET_WEIGHT"))
                txtLength.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_LENGHT"))
                txtWidth.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_WIDTH"))
                txtHeight.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_HEIGHT"))
                txtVolume.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_VOLUME"))
                txtColorInfo.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_COLOR_INFO"))
                txtHSCode.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_HSC_CODE"))
                txtPacking.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PACKING_REQ"))
                txtRemarks.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_REMARKS"))
                Me.divItemSpec.Style("display") = ""


                Me.divHist.Style("display") = ""
                Me.divVendor.Style("display") = ""

            Else
                Me.trQC.Style("display") = "none"
                Me.trItemType.Style("display") = "none"
                Me.trfld4.Style("display") = "none"
                lbl_fld1txt.Text = "Tax "
                lbl_fld2txt.Text = "Price "
                lbl_fld3txt.Text = "Reference No. "
                lbl_fld1.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("TAX_PERC"))
                lbl_fld2.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_CURRENCY_CODE")) & " " & Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_UNIT_COST"))
                lbl_fld3.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_REF_NO"))
                txtBrand.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PRODUCT_BRAND"))
                txtModel.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PRODUCT_MODEL"))
                txtDrawingNumber.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_DRAW_NO"))
                txtVersionNo.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_VERS_NO"))
                txtGrossWeight.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_GROSS_WEIGHT"))
                txtNetWeight.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_NET_WEIGHT"))
                txtLength.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_LENGHT"))
                txtWidth.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_WIDTH"))
                txtHeight.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_HEIGHT"))
                txtVolume.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_VOLUME"))
                txtColorInfo.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_COLOR_INFO"))
                txtHSCode.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_HSC_CODE"))
                txtPacking.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PACKING_REQ"))
                txtRemarks.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_REMARKS"))
                txtManufacturerName.Visible = False
                lblManufacturerName.Visible = False
                Me.divItemSpec.Style("display") = ""

            End If
        End If
        If Session("UrlLocation") = "BuyerCatalogueSearch" Or Session("UrlLocation") = "BuyerCatSearch" Or Session("UrlLocation") = "ContractItem" Or Session("UrlLocation") = "ConCatSearch" Then
            Me.cmd_back.Attributes.Add("onclick", "window.close();")
        End If
        If Session("UrlLocation") = "BuyerCatalogueSearchPopup" Then

            'Me.cmd_back.Attributes.Add("href", dDispatcher.direct("Search", "BuyerCatalogueSearchPopup.aspx", "selVendor=" & Session("selVendor") & "&selSingleVendor=" & Session("selSingleVendor") & ""))
            Me.cmd_back.Attributes.Add("onclick", "location.href='" & dDispatcher.direct("Search", "BuyerCatalogueSearchPopup.aspx") & "'")

            If Session("POAddItem") = "CloseDirect" Then
                Me.cmd_back.Attributes.Add("onclick", "window.close();")
            End If
        End If
        If Session("UrlLocation") = "VendorSearch" Then
            Me.cmd_back.Attributes.Add("onclick", "window.close();")
        End If
    End Sub

    Private Sub MyDataGrid_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        Grid_ItemCreated(MyDataGrid, e)
    End Sub

    Private Sub MyDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles MyDataGrid.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            If e.Item.Cells(6).Text = "-" Then
                e.Item.Cells(1).Text = "NA"
                e.Item.Cells(2).Text = "-"
                e.Item.Cells(3).Text = "-"
                e.Item.Cells(4).Text = "-"
            Else
                If IsNumeric(e.Item.Cells(1).Text) Then
                    e.Item.Cells(1).Text = e.Item.Cells(6).Text & " " & Format(dv("PRICE"), "###,##0.0000")
                Else
                    e.Item.Cells(1).Text = "NA"
                End If
                If Not IsDBNull(dv("TRANSDATE")) Then
                    e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("TRANSDATE"))
                Else
                    e.Item.Cells(4).Text = "-"
                End If

            End If

            If strType = e.Item.Cells(0).Text Then
                strType = e.Item.Cells(0).Text
                e.Item.Cells(0).Text = ""
            Else
                strType = e.Item.Cells(0).Text
            End If

        End If
    End Sub

    Private Sub dtgTrx_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgTrx.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            e.Item.Cells(2).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, e.Item.Cells(2).Text)
            'e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("TRANSDATE"))
        End If
    End Sub

    Private Sub dtgContractInfo_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgContractInfo.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            e.Item.Cells(2).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, e.Item.Cells(2).Text)
            e.Item.Cells(3).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, e.Item.Cells(3).Text)
            'e.Item.Cells(6).Text = Format(CDbl(e.Item.Cells(6).Text), "##0")
        End If
    End Sub
    Sub VendorProd()
        Dim objGlobal As New AppGlobals
        Dim dsproduct As DataSet
        Dim objcall As New Products
        Dim strCompanyType As String
        Dim strtxproid As String
        Dim strCoyId As String
        Dim strGrpIndex As String
        Dim strDraft As String

        strtxproid = Request.QueryString("pid")
        strCompanyType = Request.QueryString("companyType")
        strGrpIndex = ""
        strDraft = "0"
        dsproduct = objcall.getProDetails(strtxproid, Session("CompanyId"), IIf(strCompanyType = "B", True, False), strGrpIndex, IIf(strDraft = "1", True, False))
        'attachment
        Dim strImgSrc As String
        Dim dtAttach As DataTable
        Dim intCnt, intLoop, i As Integer
        Dim strTemp As String = ""
        Dim strFile, strFile1, strURL As String

        strCoyId = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_S_COY_ID"))
        lbl_unspsc.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("CT_NAME"))
        lbl_prodesc.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PRODUCT_DESC"))
        lbl_venitemcode.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_VENDOR_ITEM_CODE"))
        lbl_desc.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_LONG_DESC"))
        lbl_uom.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_UOM"))
        lblCatCode.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_CAT_CODE"))
        lblAccCode.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_ACCT_CODE"))
        tdImage.Attributes.Add("WIDTH", "10px")
        dtAttach = dsproduct.Tables(2)
        intCnt = dtAttach.Rows.Count - 1
        i = 0
        For intLoop = 0 To intCnt
            strFile = dtAttach.Rows(intLoop)("PA_ATTACH_FILENAME")
            strFile1 = dtAttach.Rows(intLoop)("PA_HUB_FILENAME")
            strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.ProductAttach, "", EnumUploadFrom.FrontOff, , strCoyId)
            If CStr(dtAttach.Rows(intLoop)("PA_ATTACH_INDEX")) = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PRODUCT_IMAGE")) Then
                strImgSrc = objFile.FileDownload(Server.UrlEncode(strFile1), EnumDownLoadType.ProductImage, "", EnumUploadFrom.FrontOff, dsproduct.Tables(0).Rows(0)("PM_S_COY_ID"))
                lblProdImage.Text = "<a href='javascript:PreviewImage(""" &
                                    Replace(strImgSrc, "\", "\\") & """);'>" _
                                    & "<img border='0' src='" &
                                    strImgSrc & "' height='150' width='150'></a>"
                'strImgSrc & "' height='200' width='200'></a>"
                lblProdImage.ToolTip = "Click here to view actual image"
            End If

            '*************************meilai************************************

            If dtAttach.Rows(intLoop)("PA_TYPE") = "D" Then
                i = i + 1
                If strTemp = "" Then
                    strTemp = "&nbsp;" & i & ") " & strURL
                Else
                    strTemp = strTemp & "<BR>&nbsp;" & i & ") " & strURL
                End If
            End If
        Next
        tdAttach.InnerHtml = strTemp
        If lblProdImage.Text = "" Then
            lblProdImage.Visible = False
        End If
        If Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PRODUCT_IMAGE")) <> "" Then
            lblProdImage.Visible = True

            Dim objFile As New FileManagement
            strImgSrc = objFile.FileDownload(dsproduct.Tables(0).Rows(0)("PM_PRODUCT_IMAGE"), EnumDownLoadType.ProductImage, "", EnumUploadFrom.FrontOff, strCoyId)
            objFile = Nothing

        Else
            lblProdImage.Visible = False
        End If

        Me.trQC.Style("display") = "none"
        Me.trItemType.Style("display") = "none"
        Me.trfld4.Style("display") = "none"
        Me.trfld3.Style("display") = "none"
        'Chee Hong - Agora GST Enhancement - 10/9/2014
        'lbl_fld1txt.Text = "Tax "
        If ViewState("GSTCOD") = True Then
            lbl_fld1txt.Text = "SST Rate "
        Else
            lbl_fld1txt.Text = "Tax "
        End If

        '---------------------------------------------
        lbl_fld2txt.Text = "Price "

        If Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_ITEM_TYPE")) = "SP" Then
            lbl_itemtype.Text = "Spot (Non-Inventoried item)"

        ElseIf Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_ITEM_TYPE")) = "ST" Then
            lbl_itemtype.Text = "Stock (Direct material - Inventoried item)"
            If Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_IQC_IND")) = "Y" Then
                lbl_needqc.Text = "Yes"
            Else
                lbl_needqc.Text = "No"
            End If
        Else
            lbl_itemtype.Text = "MRO, M&E and IT (Inventoried item)"
            If Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_IQC_IND")) = "Y" Then
                lbl_needqc.Text = "Yes"
            Else
                lbl_needqc.Text = "No"
            End If
        End If

        'Chee Hong - Agora GST Enhancement - 10/9/2014
        If ViewState("GSTCOD") = True Then
            lbl_fld1.Text = Request.QueryString("gstrate")
        Else
            lbl_fld1.Text = Request.QueryString("tax")
        End If

        '---------------------------------------------
        lbl_fld2.Text = Request.QueryString("price")
        txtManufacturerName.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_MANUFACTURER"))
        txtBrand.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PRODUCT_BRAND"))
        txtModel.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PRODUCT_MODEL"))
        txtDrawingNumber.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_DRAW_NO"))
        txtVersionNo.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_VERS_NO"))
        txtGrossWeight.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_GROSS_WEIGHT"))
        txtNetWeight.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_NET_WEIGHT"))
        txtLength.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_LENGHT"))
        txtWidth.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_WIDTH"))
        txtHeight.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_HEIGHT"))
        txtVolume.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_VOLUME"))
        txtColorInfo.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_COLOR_INFO"))
        txtHSCode.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_HSC_CODE"))
        txtPacking.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PACKING_REQ"))
        txtRemarks.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_REMARKS"))
        Me.divItemSpec.Style("display") = ""
        Me.cmd_back.Attributes.Add("onclick", "window.close();")
    End Sub
End Class
