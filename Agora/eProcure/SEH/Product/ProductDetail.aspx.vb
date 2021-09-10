Imports AgoraLegacy
Imports eProcure.Component

Public Class ProductDetailSEH
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents lbl_unspsc As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_prodesc As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_desc As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_uom As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld1txt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld2txt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld3txt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld4txt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld5txt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld6txt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld2 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld3 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld4 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld5 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_fld6 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_prefer As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_prefertax As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Prefer_SuppCode As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Prefer_DelTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Prefer_UnitPrice As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Prefer_PayTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Prefer_Currency As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Prefer_PurSpecNo As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Prefer_Revision As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_1st As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_1sttax As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_1st_SuppCode As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_1st_DelTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_1st_UnitPrice As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_1st_PayTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_1st_Currency As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_1st_PurSpecNo As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_1st_Revision As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_2nd As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_2ndtax As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_2nd_SuppCode As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_2nd_DelTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_2nd_UnitPrice As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_2nd_PayTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_2nd_Currency As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_2nd_PurSpecNo As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_2nd_Revision As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_3rd As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_3rdtax As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_3rd_SuppCode As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_3rd_DelTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_3rd_UnitPrice As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_3rd_PayTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_3rd_Currency As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_3rd_PurSpecNo As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_3rd_Revision As System.Web.UI.WebControls.Label
    Protected WithEvents lblCatCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblAccCode As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_BudgetPrice_d As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_RQL_d As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_IQCType_d As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_EOQ_d As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Ratio_d As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Oversea_d As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_pardelcd As System.Web.UI.WebControls.Label

    Protected WithEvents lbl_leadP As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_vendorP As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_lead1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_vendor1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_lead2 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_vendor2 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_lead3 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_vendor3 As System.Web.UI.WebControls.Label
    Protected WithEvents lblManufacturerName As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Spec2 As System.Web.UI.WebControls.Label
    Protected WithEvents txtSpec2 As System.Web.UI.WebControls.Label
    Protected WithEvents txtSpec3 As System.Web.UI.WebControls.Label
    Protected WithEvents txtPackType As System.Web.UI.WebControls.Label
    Protected WithEvents txtPackQty As System.Web.UI.WebControls.Label
    Protected WithEvents txtManufacturerName2 As System.Web.UI.WebControls.Label
    Protected WithEvents txtManufacturerName3 As System.Web.UI.WebControls.Label
    Protected WithEvents txtSectionCode As System.Web.UI.WebControls.Label
    Protected WithEvents txtLocationCode As System.Web.UI.WebControls.Label
    Protected WithEvents txtNewItemCode As System.Web.UI.WebControls.Label

    Protected WithEvents lbl_venitemcode As System.Web.UI.WebControls.Label
    'Protected WithEvents lbl_ven As System.Web.UI.WebControls.Label
    'Protected WithEvents lbl_brand As System.Web.UI.WebControls.Label
    'Protected WithEvents lbl_model As System.Web.UI.WebControls.Label
    Protected WithEvents MyDataGrid As System.Web.UI.WebControls.DataGrid
    Protected WithEvents divItemInfo2 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents dtgTrx As System.Web.UI.WebControls.DataGrid
    Protected WithEvents divHist As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents divVendor As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents trQC As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trItemType As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trfld4 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trfld3 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_ItemSpec1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_ItemSpec2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_ItemSpec3 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_ItemSpec4 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_ItemSpec5 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_IQC As System.Web.UI.HtmlControls.HtmlTableRow

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
    Protected WithEvents txtVolume As System.Web.UI.WebControls.Label
    Protected WithEvents txtColorInfo As System.Web.UI.WebControls.Label
    Protected WithEvents txtHSCode As System.Web.UI.WebControls.Label
    Protected WithEvents txtRemarks As System.Web.UI.WebControls.Label
    'Protected WithEvents cmd_back As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents cmd_back As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents dtgContractInfo As System.Web.UI.WebControls.DataGrid
    Protected WithEvents divContractInfo As System.Web.UI.HtmlControls.HtmlGenericControl

    Protected WithEvents lbl_itemtype As System.Web.UI.WebControls.Label
    Protected WithEvents lblQC As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_needqc As System.Web.UI.WebControls.Label

    Dim clsproduct As New Products
    Dim objCont_Ext As New ContCat_Ext
    Protected WithEvents tdAttach As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents tdImage As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents lblProdImage As System.Web.UI.WebControls.Label
    Dim strType As String
    Dim objFile As New FileManagement
    Dim aryDisVendor As New ArrayList
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
            Dim objDb As New EAD.DBCom
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
                If Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PARTIAL_CD")) = "Y" Then
                    lbl_pardelcd.Text = "Yes"
                Else
                    lbl_pardelcd.Text = "No"
                End If

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
                lbl_fld5txt.Text = "Height (meter) :"
                lbl_fld6txt.Text = "Specification 1 :"
                lbl_fld1.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_ORD_MIN_QTY"))
                lbl_fld2.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_ORD_MAX_QTY"))
                lbl_fld3.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_SAFE_QTY"))
                lbl_fld4.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_MAX_INV_QTY"))
                lbl_RQL_d.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_REORDER_QTY"))
                lbl_BudgetPrice_d.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_BUDGET_PRICE"))
                lbl_EOQ_d.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_EOQ"))
                lbl_Ratio_d.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_RATIO"))
                If Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_OVERSEA")) = "N" Then
                    lbl_Oversea_d.Text = "No"
                Else
                    lbl_Oversea_d.Text = "Yes"
                End If

                If Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_ITEM_TYPE")) = "SP" Then
                    Me.trQC.Style("display") = "none"
                    Me.tr_IQC.Style("display") = "none"
                    'Me.trItemType.Style("display") = "none"
                Else
                    Me.tr_IQC.Style("display") = ""
                    lbl_IQCType_d.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_IQC_TYPE"))
                End If

                'lbl_prefer.Text = Common.parseNull(dsproduct.Tables(3).Rows(0)("PREFER_VENDOR"))
                'lbl_prefertax.Text = Common.parseNull(dsproduct.Tables(3).Rows(0)("PREFER_VENDOR_TAX"))
                'lbl_Prefer_SuppCode.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PREFER_S_COY_ID_SUPP_CODE"))
                '' lbl_Prefer_DelTerm.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)(""))
                'lbl_Prefer_UnitPrice.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PREFER_S_COY_ID_UNIT_PRICE"))
                '' lbl_Prefer_PayTerm.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)(""))
                'lbl_Prefer_Currency.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PREFER_S_COY_ID_CURR"))
                'lbl_Prefer_PurSpecNo.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PREFER_S_COY_ID_PUR_SPEC_CODE"))
                'lbl_Prefer_Revision.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PREFER_S_COY_ID_REVISION"))
                'lbl_1st.Text = Common.parseNull(dsproduct.Tables(3).Rows(0)("1st_VENDOR"))
                'lbl_1sttax.Text = Common.parseNull(dsproduct.Tables(3).Rows(0)("1st_VENDOR_TAX"))
                'lbl_1st_SuppCode.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_1ST_S_COY_ID_SUPP_CODE"))
                '' lbl_1st_DelTerm.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)(""))
                'lbl_1st_UnitPrice.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_1ST_S_COY_ID_UNIT_PRICE"))
                '' lbl_1st_PayTerm.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)(""))
                'lbl_1st_Currency.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_1ST_S_COY_ID_CURR"))
                'lbl_1st_PurSpecNo.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_1ST_S_COY_ID_PUR_SPEC_CODE"))
                'lbl_1st_Revision.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_1ST_S_COY_ID_REVISION"))
                'lbl_2nd.Text = Common.parseNull(dsproduct.Tables(3).Rows(0)("2nd_VENDOR"))
                'lbl_2ndtax.Text = Common.parseNull(dsproduct.Tables(3).Rows(0)("2nd_VENDOR_TAX"))
                'lbl_2nd_SuppCode.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_2ND_S_COY_ID_SUPP_CODE"))
                '' lbl_2nd_DelTerm.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)(""))
                'lbl_2nd_UnitPrice.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_2ND_S_COY_ID_UNIT_PRICE"))
                '' lbl_2nd_PayTerm.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)(""))
                'lbl_2nd_Currency.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_2ND_S_COY_ID_CURR"))
                'lbl_2nd_PurSpecNo.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_2ND_S_COY_ID_PUR_SPEC_CODE"))
                'lbl_2nd_Revision.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_2ND_S_COY_ID_REVISION"))
                'lbl_3rd.Text = Common.parseNull(dsproduct.Tables(3).Rows(0)("3rd_VENDOR"))
                'lbl_3rdtax.Text = Common.parseNull(dsproduct.Tables(3).Rows(0)("3rd_VENDOR_TAX"))
                'lbl_3rd_SuppCode.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_3RD_S_COY_ID_SUPP_CODE"))
                ''lbl_3rd_DelTerm.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)(""))
                'lbl_3rd_UnitPrice.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_3RD_S_COY_ID_UNIT_PRICE"))
                '' lbl_3rd_PayTerm.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)(""))
                'lbl_3rd_Currency.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_3RD_S_COY_ID_CURR"))
                'lbl_3rd_PurSpecNo.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_3RD_S_COY_ID_PUR_SPEC_CODE"))
                'lbl_3rd_Revision.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_3RD_S_COY_ID_REVISION"))

                'If dsproduct.Tables(4).Rows.Count > 0 Then
                '    Dim count As Integer
                '    For count = 0 To dsproduct.Tables(4).Rows.Count - 1
                '        Dim vType As String
                '        vType = Common.parseNull(dsproduct.Tables(4).Rows(count).Item("PV_VENDOR_TYPE"))

                '        If vType = "P" Then
                '            lbl_leadP.Text = Common.parseNull(dsproduct.Tables(4).Rows(count).Item("PV_LEAD_TIME"))
                '            lbl_vendorP.Text = Common.parseNull(dsproduct.Tables(4).Rows(count).Item("PV_VENDOR_CODE"))
                '            'lbl_leadP.Text = IIf(lbl_leadP.Text = "", "N/A", lbl_leadP.Text)
                '            'lbl_vendorP.Text = IIf(lbl_vendorP.Text = "", "N/A", lbl_vendorP.Text)
                '        ElseIf vType = "1" Then
                '            lbl_lead1.Text = Common.parseNull(dsproduct.Tables(4).Rows(count).Item("PV_LEAD_TIME"))
                '            lbl_vendor1.Text = Common.parseNull(dsproduct.Tables(4).Rows(count).Item("PV_VENDOR_CODE"))
                '        ElseIf vType = "2" Then
                '            lbl_lead2.Text = Common.parseNull(dsproduct.Tables(4).Rows(count).Item("PV_LEAD_TIME"))
                '            lbl_vendor2.Text = Common.parseNull(dsproduct.Tables(4).Rows(count).Item("PV_VENDOR_CODE"))
                '        ElseIf vType = "3" Then
                '            lbl_lead3.Text = Common.parseNull(dsproduct.Tables(4).Rows(count).Item("PV_LEAD_TIME"))
                '            lbl_vendor3.Text = Common.parseNull(dsproduct.Tables(4).Rows(count).Item("PV_VENDOR_CODE"))
                '        End If
                '    Next
                'End If

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

                txtBrand.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PRODUCT_BRAND"))
                txtModel.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PRODUCT_MODEL"))
                txtDrawingNumber.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_DRAW_NO"))
                txtVersionNo.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_VERS_NO"))
                txtGrossWeight.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_GROSS_WEIGHT"))
                txtNetWeight.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_NET_WEIGHT"))
                txtLength.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_LENGHT"))
                txtWidth.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_WIDTH"))
                lbl_fld5.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_HEIGHT"))
                txtVolume.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_VOLUME"))
                txtColorInfo.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_COLOR_INFO"))
                txtHSCode.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_HSC_CODE"))
                lbl_fld6.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_SPEC1"))
                If Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PACKING_TYPE")) = "" Then
                    txtPackType.Text = ""
                Else
                    txtPackType.Text = objDb.GetVal("SELECT CONCAT(CPT_PACK_CODE, ' (', CPT_PACK_NAME, ')') FROM COMPANY_PACKING_TYPE WHERE CPT_PACK_CODE = '" & Common.Parse(Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PACKING_TYPE"))) & "' AND  CPT_COY_ID = '" & Session("CompanyId") & "'")
                End If

                txtSpec2.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_SPEC2"))
                txtSpec3.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_SPEC3"))
                txtPackQty.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PACKING_QTY"))
                txtManufacturerName.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_MANUFACTURER"))
                txtSectionCode.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_SECTION"))
                txtManufacturerName2.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_MANUFACTURER2"))
                txtLocationCode.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_LOCATION"))
                txtManufacturerName3.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_MANUFACTURER3"))
                txtNewItemCode.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_NEW_ITEM_CODE"))
                txtRemarks.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_REMARKS"))

                Me.divItemSpec.Style("display") = ""
                Me.divItemInfo2.Style("display") = ""
                Me.tr_ItemSpec1.Style("display") = ""
                Me.tr_ItemSpec2.Style("display") = ""
                Me.tr_ItemSpec3.Style("display") = ""
                Me.tr_ItemSpec4.Style("display") = ""
                Me.tr_ItemSpec5.Style("display") = ""
                Me.lbl_Spec2.Visible = True
                Me.txtSpec2.Visible = True
                Me.divHist.Style("display") = ""
                Me.divVendor.Style("display") = ""

                'If Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_ITEM_TYPE")) <> "SP" Then
                '    Dim dsCount As New DataSet
                '    Dim objDb As New EAD.DBCom
                '    Dim strProCode As String = objDb.GetVal("SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & Request.QueryString("pid") & "'")

                '    dsCount = objDb.FillDs("SELECT '*' FROM PIM_VENDOR WHERE PV_PRODUCT_INDEX = '" & strProCode & "' ")

                '    If dsCount.Tables(0).Rows.Count > 4 Then
                '        ViewState("Row") = dsCount.Tables(0).Rows.Count
                '    Else
                '        ViewState("Row") = 4
                '    End If

                'Else
                '    ViewState("Row") = 4
                'End If

                'BuildRow()
                'ConstructTable()
                DisplayVendorInfo()
                DisplayUnitPricePopup()
            Else
                Me.trQC.Style("display") = "none"
                Me.tr_IQC.Style("display") = "none"
                Me.trItemType.Style("display") = "none"
                Me.trfld4.Style("display") = "none"
                lbl_fld1txt.Text = "Tax "
                lbl_fld2txt.Text = "Price "
                lbl_fld3txt.Text = "Reference No. "
                lbl_fld5txt.Text = "Packing Specification :"
                lbl_fld6txt.Text = "Height (meter) :"
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
                lbl_fld6.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_HEIGHT"))
                txtVolume.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_VOLUME"))
                txtColorInfo.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_COLOR_INFO"))
                txtHSCode.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_HSC_CODE"))
                lbl_fld5.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PACKING_REQ"))
                txtRemarks.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_REMARKS"))
                Me.divItemSpec.Style("display") = ""

            End If
        End If
        If Session("UrlLocation") = "BuyerCatalogueSearch" Or Session("UrlLocation") = "BuyerCatSearch" Or Session("UrlLocation") = "ContractItem" Or Session("UrlLocation") = "ConCatSearch" Or Session("UrlLocation") = "AppGrpAsgItem" Then
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
            e.Item.Cells(3).Text = Format(CDbl(e.Item.Cells(3).Text), "##0.00")
        End If
    End Sub

    Private Sub dtgContractInfo_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgContractInfo.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            e.Item.Cells(2).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, e.Item.Cells(2).Text)
            e.Item.Cells(3).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, e.Item.Cells(3).Text)
            e.Item.Cells(6).Text = Format(CDbl(e.Item.Cells(6).Text), "##0")
        End If
    End Sub

    Sub DisplayVendorInfo()
        BuildRow()
        ConstructTable()
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
            lbl_fld1txt.Text = "GST Rate "
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
        lbl_fld5txt.Text = "Packing Specification :"
        lbl_fld6txt.Text = "Height (meter) :"
        txtManufacturerName.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_MANUFACTURER"))
        txtBrand.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PRODUCT_BRAND"))
        txtModel.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PRODUCT_MODEL"))
        txtDrawingNumber.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_DRAW_NO"))
        txtVersionNo.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_VERS_NO"))
        txtGrossWeight.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_GROSS_WEIGHT"))
        txtNetWeight.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_NET_WEIGHT"))
        txtLength.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_LENGHT"))
        txtWidth.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_WIDTH"))
        lbl_fld6.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_HEIGHT"))
        txtVolume.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_VOLUME"))
        txtColorInfo.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_COLOR_INFO"))
        txtHSCode.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_HSC_CODE"))
        lbl_fld5.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_PACKING_REQ"))
        txtRemarks.Text = Common.parseNull(dsproduct.Tables(0).Rows(0)("PM_REMARKS"))
        Me.divItemSpec.Style("display") = ""
        Me.cmd_back.Attributes.Add("onclick", "window.close();")
    End Sub

    Sub DisplayUnitPricePopup()
        Dim ds As New DataSet
        Dim c, i As Integer
        Dim aryTemp As New ArrayList()
        Dim strVol, strline, strPrice As String
        Session("jqPopup") = ""



        For c = 0 To aryDisVendor.Count - 1
            ds = objCont_Ext.GetUnitPrice2(Request.QueryString("pid"), aryDisVendor(c)(1))

            aryTemp.Clear()

            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1

                    strVol = Common.parseNull(ds.Tables(0).Rows(i).Item("PVP_VOLUME"))
                    strPrice = Common.parseNull(ds.Tables(0).Rows(i).Item("PVP_VOLUME_PRICE"))
                    strline = "UnitPrice" & Common.parseNull(ds.Tables(0).Rows(i).Item("PVP_VENDOR_TYPE"))

                    aryTemp.Add(New String() {strVol, strPrice, strline})
                Next

                ContructRow(strline, aryTemp)

            End If

            'If (aryDisVendor(i)(2) <> "") And (tempComp = vendorP) Then
            '    aryTemp.Add(New String() {tempVolFrom, tempVolTo, tempVolPrice, "UnitPrice1"})
            'ElseIf (aryDisVendor(i)(2) <> "") And (tempComp = vendor1) Then
            '    aryTemp.Add(New String() {tempVolFrom, tempVolTo, tempVolPrice, "UnitPrice2"})
            'ElseIf (aryDisVendor(i)(2) <> "") And (tempComp = vendor2) Then
            '    aryTemp.Add(New String() {tempVolFrom, tempVolTo, tempVolPrice, "UnitPrice3"})
            'ElseIf (aryDisVendor(i)(2) <> "") And (tempComp = vendor3) Then
            '    aryTemp.Add(New String() {tempVolFrom, tempVolTo, tempVolPrice, "UnitPrice4"})
            'End If


        Next

        'aryTemp = aryTemp
        'If (vendorP <> "") Then
        '    ContructRow("UnitPrice1", aryTemp)
        'End If
        'If (vendor1 <> "") Then
        '    ContructRow("UnitPrice2", aryTemp)
        'End If
        'If (vendor2 <> "") Then
        '    ContructRow("UnitPrice3", aryTemp)
        'End If
        'If (vendor3 <> "") Then
        '    ContructRow("UnitPrice4", aryTemp)
        'End If

    End Sub

    Private Function ContructRow(ByVal strVCompLine As String, ByVal aryVolume As ArrayList) As String
        Dim strrow, strtable As String
        Dim i As Integer


        For i = 0 To aryVolume.Count - 1
            'If aryVolume(i)(3) = strVCompLine Then
            strrow &= "Volume " & aryVolume(i)(0) & " : " & aryVolume(i)(1) & "<BR>"
            'End If
        Next

        strtable = strrow

        Session("jqPopup") = Session("jqPopup") & "$('.jq" & strVCompLine & "').CreateBubblePopup({innerHtml: '" & strtable & "',position:'left', align: 'middle', innerHtmlStyle: { 'text-align':'left' },themeName:'all-black',themePath:'../../Common/Plugins/images/jquerybubblepopup-theme'});"

    End Function

    Private Function BuildRow()
        Dim i As Integer
        Dim dsCount As New DataSet
        Dim objDb As New EAD.DBCom
        Dim dsVen As New DataSet
        Dim dsVen2 As New DataSet
        Dim strItemType, strDelTerm As String


        strItemType = objDb.GetVal("SELECT PM_ITEM_TYPE FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & Request.QueryString("pid") & "'")

        If Common.parseNull(strItemType) <> "SP" Then
            Dim strProCode As String = objDb.GetVal("SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & Request.QueryString("pid") & "'")

            dsCount = objDb.FillDs("SELECT '*' FROM PIM_VENDOR WHERE PV_PRODUCT_INDEX = '" & strProCode & "' ")

            If dsCount.Tables(0).Rows.Count > 4 Then
                ViewState("Row") = dsCount.Tables(0).Rows.Count
            Else
                ViewState("Row") = 4
            End If

        Else
            ViewState("Row") = 4
        End If

       

        For i = 0 To ViewState("Row") - 1
            If i = 0 Then
                aryDisVendor.Add(New String() {"Preferred Vendor", "P", "", "", "", "", "", "", "", "", "", ""})
            ElseIf i = 1 Then
                aryDisVendor.Add(New String() {"1st Alternative Vendor", "1", "", "", "", "", "", "", "", "", "", ""})
            ElseIf i = 2 Then
                aryDisVendor.Add(New String() {"2nd Alternative Vendor", "2", "", "", "", "", "", "", "", "", "", ""})
            ElseIf i = 3 Then
                aryDisVendor.Add(New String() {"3rd Alternative Vendor", "3", "", "", "", "", "", "", "", "", "", ""})
            Else
                aryDisVendor.Add(New String() {i & "th Alternative Vendor", i, "", "", "", "", "", "", "", "", "", ""})
            End If
        Next

        For i = 0 To aryDisVendor.Count - 1
            dsVen2 = objCont_Ext.GetVendorInfo2(Request.QueryString("pid"), aryDisVendor(i)(1))
            strDelTerm = ""
            If dsVen2.Tables(0).Rows.Count > 0 Then
                aryDisVendor(i)(2) = Common.parseNull(dsVen2.Tables(0).Rows(0).Item("PV_S_COY_NAME")) 'Company Name
                'aryDisVendor(i)(10) = Common.parseNull(dsVen2.Tables(0).Rows(0).Item("PV_S_COY_ID_TAX")) 'Tax
                aryDisVendor(i)(4) = Common.parseNull(dsVen2.Tables(0).Rows(0).Item("PV_SUPP_CODE"))     'Supplier Code

                If Common.parseNull(dsVen2.Tables(0).Rows(0).Item("PV_DELIVERY_TERM")) = "" Then
                    strDelTerm = ""
                Else
                    strDelTerm = objDb.GetVal("SELECT CONCAT(CDT_DEL_CODE, ' (', CDT_DEL_NAME, ')') FROM COMPANY_DELIVERY_TERM WHERE CDT_COY_ID = '" & Session("CompanyId") & "' AND CDT_DEL_CODE = '" & Common.Parse(dsVen2.Tables(0).Rows(0).Item("PV_DELIVERY_TERM")) & "'")
                End If
                aryDisVendor(i)(5) = strDelTerm 'Delivery Term
                aryDisVendor(i)(6) = Common.parseNull(dsVen2.Tables(0).Rows(0).Item("PV_PAYMENT"))  'Payment Term 
                aryDisVendor(i)(7) = Common.parseNull(dsVen2.Tables(0).Rows(0).Item("PV_CURR_NAME"))          'Currency
                aryDisVendor(i)(8) = Common.parseNull(dsVen2.Tables(0).Rows(0).Item("PV_PUR_SPEC_NO"))   'Purchase Spec No
                aryDisVendor(i)(9) = Common.parseNull(dsVen2.Tables(0).Rows(0).Item("PV_REVISION"))      'Revision
                aryDisVendor(i)(10) = Common.parseNull(dsVen2.Tables(0).Rows(0).Item("PV_LEAD_TIME"))    'Lead Time
                aryDisVendor(i)(11) = Common.parseNull(dsVen2.Tables(0).Rows(0).Item("PV_VENDOR_CODE"))  'Vendor Item Code

            End If
        Next

    End Function

    Private Function ConstructTable()
        Dim strrow As String = ""
        Dim i, j As Integer
        Dim table As String

        For i = 0 To aryDisVendor.Count - 1
            strrow &= "<tr>"
            strrow &= "<td class=""tablecol""  width=""170px"" style=""height: 19px""><strong>&nbsp;" & aryDisVendor(i)(0) & " :</strong></td>"
            strrow &= "<td class=""TableInput"" style=""height: 19px; width: 230px;"" >&nbsp;" & aryDisVendor(i)(2) & "</td>" 'Vendor Company Name
            strrow &= "<td class=""TableInput"" style=""height: 19px;"">&nbsp;" & aryDisVendor(i)(4) & "</td>" 'Supplier Code
            strrow &= "<td class=""TableInput"" style=""height: 19px;"">&nbsp;" & aryDisVendor(i)(5) & "</td>" 'Delivery Term
            strrow &= "<td class=""TableInput"" align=""right"" style=""height: 19px;""><span style=""cursor:default;"" class=""jqUnitPrice" & aryDisVendor(i)(1) & """><IMG src=""" & dDispatcher.direct("Plugins/images", "v_icon.gif") & """></span></td>" 'Unit Price
            strrow &= "<td class=""TableInput"" style=""height: 19px;"">&nbsp;" & aryDisVendor(i)(6) & "</td>" 'Payment Term
            strrow &= "<td class=""TableInput"" style=""height: 19px;"">" & aryDisVendor(i)(7) & "</td>" 'Currency
            strrow &= "<td class=""TableInput"" style=""height: 19px;"">&nbsp;" & aryDisVendor(i)(8) & "</td>" 'Purchase Spec No
            strrow &= "<td class=""TableInput"" style=""height: 19px;"">&nbsp;" & aryDisVendor(i)(9) & "</td>" 'Revision
            strrow &= "<td class=""TableInput"" align=""center"" style=""height: 19px;"">&nbsp;" & aryDisVendor(i)(10) & "</td>" 'Lead Time
            strrow &= "<td class=""TableInput"">&nbsp;" & aryDisVendor(i)(11) & "</td>" 'Vendor Item Code
            'strrow &= "<td class=""TableInput"" align=""right"" style=""height: 19px;"">&nbsp;" & aryDisVendor(i)(10) & "</td>" 'Tax
            'strrow &= "<td class=""TableInput"" align=""center"" style=""height: 19px;"">&nbsp;" & aryDisVendor(i)(11) & "</td>" 'Lead Time
            'strrow &= "<td class=""TableInput"">&nbsp;" & aryDisVendor(i)(12) & "</td>" 'Vendor Item Code
        Next

        table = "<table id=""Table5"" cellspacing=""0"" cellpadding=""0"" width=""1200px"" border=""0"">" & _
                "<tr>" & _
                "<td class=""tableheader"" style=""width: 350px"" colspan=""2"">Vendor</td>" & _
                "<td class=""tableheader"" style=""width: 100px"">Vendor Code</td>" & _
                "<td class=""tableheader"" style=""width: 150px"">Delivery Term</td>" & _
                "<td class=""tableheader"" style=""width: 50px"">Unit Price</td>" & _
                "<td class=""tableheader"" style=""width: 100px"">Payment Term</td>" & _
                "<td class=""tableheader"" style=""width: 90px"">Currency</td>" & _
                "<td class=""tableheader"" style=""width: 60px"">Purchaser Spec No.</td>" & _
                "<td class=""tableheader"" style=""width: 100px"">Revision</td>" & _
                "<td class=""tableheader"" style=""width: 50px"">Order Lead Time (Days)</td>" & _
                "<td class=""tableheader"" style=""width: 100px"">Vendor Item code</td>" & _
                "</tr>" & _
                strrow & _
                "</table>"
        'table = "<table id=""Table5"" cellspacing=""0"" cellpadding=""0"" width=""1200px"" border=""0"">" & _
        '       "<tr>" & _
        '       "<td class=""tableheader"" style=""width: 350px"" colspan=""2"">Vendor</td>" & _
        '       "<td class=""tableheader"" style=""width: 100px"">Vendor Code</td>" & _
        '       "<td class=""tableheader"" style=""width: 150px"">Delivery Term</td>" & _
        '       "<td class=""tableheader"" style=""width: 50px"">Unit Price</td>" & _
        '       "<td class=""tableheader"" style=""width: 100px"">Payment Term</td>" & _
        '       "<td class=""tableheader"" style=""width: 90px"">Currency</td>" & _
        '       "<td class=""tableheader"" style=""width: 60px"">Purchaser Spec No.</td>" & _
        '       "<td class=""tableheader"" style=""width: 100px"">Revision</td>" & _
        '       "<td class=""tableheader"" style=""width: 50px"">GST/VAT</td>" & _
        '       "<td class=""tableheader"" style=""width: 50px"">Order Lead Time (Days)</td>" & _
        '       "<td class=""tableheader"" style=""width: 100px"">Vendor Item code</td>" & _
        '       "</tr>" & _
        '       strrow & _
        '       "</table>"

        Session("ConstructTable") = table

    End Function
End Class
