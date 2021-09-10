Imports AgoraLegacy
Imports SSO.Component

Public Class HubListPriceApproval
    Inherits AgoraLegacy.AppBaseClass
    Dim objCat As New ContCat
    Protected WithEvents lblVendorName As System.Web.UI.WebControls.Label
    Protected WithEvents lblDiscGrp As System.Web.UI.WebControls.Label
    Protected WithEvents trDiscGrp As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblContGrp As System.Web.UI.WebControls.Label
    Protected WithEvents dtgCat As System.Web.UI.WebControls.DataGrid
    Protected WithEvents trCat1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trCat2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trCat3 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trCat4 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDis1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDis2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDis4 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDis3 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdApprove As System.Web.UI.WebControls.Button
    Protected WithEvents trSave As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdReject As System.Web.UI.WebControls.Button
    Protected WithEvents trApprove As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents txtRemarks As System.Web.UI.WebControls.TextBox

    Protected WithEvents trRemark As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents trRemark2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents hidImageAttached As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents pnlImage As System.Web.UI.WebControls.Panel
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents vldCommodityType As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents hidCommodityType As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtCommodityType As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRefNo As System.Web.UI.WebControls.TextBox

    Protected WithEvents cmdSelect As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidPostBack As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidImageFile As System.Web.UI.HtmlControls.HtmlInputHidden

    Dim objGlobal As New AppGlobals
    Dim objFile As New FileManagement
    Dim blnList As Boolean
    Protected WithEvents hidCataloguePrice As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblAttach As System.Web.UI.WebControls.Label
    Dim blnDiscount As Boolean
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Public Enum EnumCat
        icChk = 0
        icIndex = 1
        icCode = 2
        icPrice = 3
        icRemark = 4
    End Enum
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblItemId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtCategory As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblvldDesc As System.Web.UI.WebControls.Label
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtItemName As System.Web.UI.WebControls.TextBox
    Protected WithEvents vldDesc As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents cboUOM As System.Web.UI.WebControls.DropDownList
    Protected WithEvents vldUOM As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents cboCurrencyCode As System.Web.UI.WebControls.DropDownList
    Protected WithEvents vldCurrencyCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents cboTax As System.Web.UI.WebControls.DropDownList
    Protected WithEvents vldTax As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cmdUploadImage As System.Web.UI.WebControls.Button
    Protected WithEvents cmdUpload As System.Web.UI.WebControls.Button
    Protected WithEvents pnlAttach As System.Web.UI.WebControls.Panel
    Protected WithEvents cboMgmt As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtMgmtCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCatPrice As System.Web.UI.WebControls.TextBox
    Protected WithEvents revPrice As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents lblVItemCode As System.Web.UI.WebControls.Label
    Protected WithEvents txtVendorItemCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents vldVItemCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtBrand As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtModel As System.Web.UI.WebControls.TextBox
    Protected WithEvents dtgCatalogue As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents FileProImage As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents File1 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents trHidUploadImage As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trHidUploadAttach As System.Web.UI.HtmlControls.HtmlTableRow


    Protected WithEvents txtPrice As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDrawingNumber As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVersionNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtGrossWeight As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNetWeight As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLength As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWidth As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtHeight As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVolume As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtColorInfo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtHSCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPacking As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRemark As System.Web.UI.WebControls.TextBox
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnSorting = False
        blnPaging = False
        'SetGridProperty(dtgCatalogue)
        'SetGridProperty(dtgCat)

        If Not IsPostBack Then
            isNotPostBack()
        End If
        'cmdSave.Attributes.Add("onclick", "return clearMsg();")
        displayAttachFile()
        displayImageFile(Session("ImageIndex"))
        'lnkBack.NavigateUrl = "HubListPriceCatalogue.aspx?type=" & viewstate("type") & "&pageid=" & strPageId
        lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "HubListPriceCatalogue.aspx", "type=" & ViewState("type") & "&pageid=" & strPageId)
        cmdUploadImage.Attributes.Add("onclick", "return checkDocFile('img','" & FileProImage.ClientID & "');")
        cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & File1.ClientID & "');")

        cmdReject.Attributes.Add("onClick", "return confirmReject();")

        'cmdReject.Attributes.Add("onClick", "return resetSummary(0,1);")
        'cmdSave.Attributes.Add("onClick", "return resetSummary(0,1);")
        'cmdApprove.Attributes.Add("onClick", "return resetSummary(0,1);")
    End Sub

    Private Sub isNotPostBack()
        lblMsg.Text = ""
        ViewState("type") = Request.QueryString("type")
        objGlobal.FillCodeTable(cboUOM, CodeTable.Uom)
        objGlobal.FillCodeTable(cboCurrencyCode, CodeTable.Currency)
        objGlobal.FillTax(cboTax)
        'objGlobal.FillCodeTable(cboMgmt, CodeTable.MgmtCode)
        Me.trHidUploadImage.Style("display") = "none"
        Me.trHidUploadAttach.Style("display") = "none"
        Dim objCat As New ContCat


        Select Case ViewState("type")
            Case "A" ' approval
                trSave.Visible = False
                ViewState("action") = Request.QueryString("action")
                ViewState("status") = Request.QueryString("status")
                ViewState("itemid") = Request.QueryString("itemid")
                ViewState("itemindex") = Request.QueryString("itemindex")
                'objCat.resetDeletedAttachment(viewstate("itemid"))
                'trCat1.Visible = False
                'trCat2.Visible = False
                'trCat3.Visible = False
                'trCat4.Visible = False
                lblTitle.Text = "Vendor Item Approval Detail"
                'lblDiscGrp.Text = "To remove the assigned group, unselect the Discount Group Code."
                lblHeader.Text = "List Price Catalogue Approval Details"

                Select Case ViewState("action")
                    Case "N"
                        displayData("A")
                    Case "M"
                        displayData("O")
                        Session("ImageIndex") = objCat.copyAttachFile(ViewState("itemid"), "Hub")
                        'trDis1.Visible = False
                        'trDis2.Visible = False
                        'trDis3.Visible = False
                        'trDis4.Visible = False
                End Select

                If ViewState("status") = "2" Then ' rejected by hub admin
                    trApprove.Visible = False
                    enableControl(False)
                End If

            Case "O" ' other
                ViewState("mode") = Request.QueryString("mode")
                trApprove.Visible = False
                trRemark.Visible = False
                trRemark2.Visible = False
                If ViewState("mode") = "mod" Then
                    lblTitle.Text = "Modify Item"
                    ViewState("itemid") = Request.QueryString("itemid")
                    ViewState("itemindex") = Request.QueryString("itemindex")
                    objCat.resetDeletedAttachment(ViewState("itemid"))
                    lblDiscGrp.Text = "To remove the assigned group, unselect the Discount Group Code."
                    lblContGrp.Text = "To remove the assigned group, unselect the Contract Ref. No."
                    displayData("P")
                    cmdReset.Value = "Reset"
                    lblHeader.Text = "Modify the required fields"
                Else
                    lblTitle.Text = "Add New Item"
                    'lblItemId.Value = "To Be Allocated By System"
                    Dim objComp As New Company
                    Dim objComps As New Companies
                    objComp = objComps.GetCompanyDetails(Session("CompanyIdToken")) ' vendor
                    lblVendorName.Text = objComp.CoyName
                    trCat3.Visible = False
                    trDis3.Visible = False
                    'txtCategory.Text = "80801001"
                    ViewState("itemid") = ""
                    cmdReset.Value = "Clear"
                    lblHeader.Text = "Fill in the required fields"
                    clearTextBox()
                End If
                BindCat()
        End Select
        Bindgrid()
        'displayAttachFile()
    End Sub

    Private Sub clearTextBox()
        Me.txtBrand.Text = ""
        'Me.txtCategory.Text = ""
        'Me.txtCatPrice.Text = ""
        Me.txtDesc.Text = ""
        'Me.txtMgmtCode.Text = ""
        Me.txtModel.Text = ""
        Me.txtVendorItemCode.Text = ""
        Me.txtRemark.Text = ""
    End Sub

    Private Function enableControl(ByVal bln As Boolean)
        'txtCategory.Enabled = bln
        txtDesc.Enabled = bln
        'txtMgmtCode.Enabled = bln
        'txtCatPrice.Enabled = bln
        txtVendorItemCode.Enabled = bln
        txtBrand.Enabled = bln
        txtModel.Enabled = bln
        cboUOM.Enabled = bln
        cboCurrencyCode.Enabled = bln
        cboTax.Enabled = bln
        'cboMgmt.Enabled = bln
        txtRemark.Enabled = bln
    End Function

    ' type = O and mode = mod; type = A and action = N
    Private Function displayData(ByVal strType As String)
        Dim ds As New DataSet
        Select Case strType
            Case "A" ' type = A and action = N
                ds = objCat.getSingleProduct(viewstate("itemid"), True)
            Case "O" ' type = A and action = M
                ds = objCat.getSingleModifiedProduct(viewstate("itemid"), viewstate("itemindex"))
            Case "P" ' type = O and mode = mod
                ds = objCat.getSingleProduct(viewstate("itemid"), False)
        End Select

        If ds.Tables(0).Rows.Count > 0 Then
            lblVendorName.Text = Common.parseNull(ds.Tables(0).Rows(0)("CM_COY_NAME"))
            lblItemId.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_CODE"))
            txtCommodityType.Text = Common.parseNull(ds.Tables(0).Rows(0)("CT_NAME"))
            hidCommodityType.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_CATEGORY_NAME"))
            txtItemName.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_DESC"))
            txtRefNo.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_REF_NO"))
            txtVendorItemCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_VENDOR_ITEM_CODE"))
            txtDesc.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_LONG_DESC"))
            'txtMgmtCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_MGMT_TEXT"))
            'If Not IsDBNull(ds.Tables(0).Rows(0)("PM_UNIT_COST")) Then
            '    txtCatPrice.Text = Format(CDbl(Common.parseNull(ds.Tables(0).Rows(0)("PM_UNIT_COST"))), "#0.0000")
            'Else
            '    txtCatPrice.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_UNIT_COST"))
            'End If
            'hidCataloguePrice.Value = txtCatPrice.Text


            viewstate("oldVIC") = Common.parseNull(ds.Tables(0).Rows(0)("PM_VENDOR_ITEM_CODE"))
            txtBrand.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_BRAND"))
            txtModel.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_MODEL"))
            Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("PM_UOM")), cboUOM, False, True)
            Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("PM_CURRENCY_CODE")), cboCurrencyCode, True, True)
            Common.SelDdl(Common.parseNull(CStr(ds.Tables(0).Rows(0)("PM_TAX_ID"))), cboTax, True, True)
            'Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("PM_MGMT_CODE")), cboMgmt, True, True)
            If Not IsDBNull(ds.Tables(0).Rows(0)("PM_UNIT_COST")) Then
                txtPrice.Text = Format(CDbl(Common.parseNull(ds.Tables(0).Rows(0)("PM_UNIT_COST"))), "#0.0000")
            Else
                txtPrice.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_UNIT_COST"))
            End If
            txtDrawingNumber.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_DRAW_NO"))
            txtVersionNo.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_VERS_NO"))
            txtGrossWeight.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_GROSS_WEIGHT"))
            txtNetWeight.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_NET_WEIGHT"))
            txtLength.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_LENGHT"))
            txtWidth.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_WIDTH"))
            txtHeight.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_HEIGHT"))
            txtVolume.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_VOLUME"))
            txtColorInfo.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_COLOR_INFO"))
            txtHSCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_HSC_CODE"))
            txtPacking.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_PACKING_REQ"))
            txtRemarks.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_REMARKS"))

            txtRemark.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_REMARK"))


            'Session("ImageIndex") = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_IMAGE"))
            Session("ImageIndex") = objCat.getItemTempImage(Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_CODE")))
        End If

        ' display attached files
        displayAttachFile()
        displayImageFile(Session("ImageIndex"))

        If viewstate("status") = "1" And strType <> "A" Then
            If ds.Tables(1).Rows.Count > 0 Then
                'txtCategory.Enabled = False
                txtCommodityType.Enabled = False 'CT_NAME
                txtItemName.Enabled = False 'PM_PRODUCT_DESC
                txtRefNo.Enabled = False 'PM_REF_NO
                txtVendorItemCode.Enabled = False 'PM_VENDOR_ITEM_CODE
                txtDesc.Enabled = False 'PM_LONG_DESC
                txtBrand.Enabled = False 'PM_PRODUCT_BRAND
                txtModel.Enabled = False 'PM_PRODUCT_MODEL
                cboUOM.Enabled = False 'PM_UOM
                cboCurrencyCode.Enabled = False 'PM_CURRENCY_CODE
                'txtCatPrice.Enabled = False
                cboTax.Enabled = False 'PM_TAX_ID
                'cboMgmt.Enabled = False
                txtPrice.Enabled = False 'PM_UNIT_COST
                txtDrawingNumber.Enabled = False 'PM_DRAW_NO
                txtVersionNo.Enabled = False 'PM_VERS_NO
                txtGrossWeight.Enabled = False 'PM_GROSS_WEIGHT
                txtNetWeight.Enabled = False 'PM_NET_WEIGHT
                txtLength.Enabled = False 'PM_LENGHT
                txtWidth.Enabled = False 'PM_WIDTH
                txtHeight.Enabled = False 'PM_HEIGHT
                txtVolume.Enabled = False 'PM_VOLUME
                txtColorInfo.Enabled = False 'PM_COLOR_INFO
                txtHSCode.Enabled = False 'PM_HSC_CODE
                txtPacking.Enabled = False 'PM_PACKING_REQ
                txtRemarks.Enabled = False 'PM_REMARKS
                'txtMgmtCode.Enabled = False

                Dim i As Integer
                For i = 0 To ds.Tables(1).Rows.Count - 1
                    'If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_CATEGORY_NAME" Then
                    '    txtCategory.Enabled = True
                    '    txtCategory.CssClass = "modtxtbox"
                    '    txtCategory.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    'End If
                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "CT_NAME" Then
                        txtCommodityType.Enabled = True
                        txtCommodityType.CssClass = "modtxtbox"
                        txtCommodityType.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If

                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_PRODUCT_DESC" Then
                        txtItemName.Enabled = True
                        txtItemName.CssClass = "modtxtbox"
                        txtItemName.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If
                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_REF_NO" Then
                        txtRefNo.Enabled = True
                        txtRefNo.CssClass = "modtxtbox"
                        txtRefNo.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If
                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_VENDOR_ITEM_CODE" Then
                        txtVendorItemCode.Enabled = True
                        txtVendorItemCode.CssClass = "modtxtbox"
                        txtVendorItemCode.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If
                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_LONG_DESC" Then
                        txtDesc.Enabled = True
                        txtDesc.CssClass = "modtxtbox"
                        txtDesc.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If

                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_PRODUCT_BRAND" Then
                        txtBrand.Enabled = True
                        txtBrand.CssClass = "modtxtbox"
                        txtBrand.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If
                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_PRODUCT_MODEL" Then
                        txtModel.Enabled = True
                        txtModel.CssClass = "modtxtbox"
                        txtModel.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If
                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_UOM" Then
                        cboUOM.Enabled = True
                        cboUOM.CssClass = "modtxtbox"
                        Common.SelDdl(Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE")), cboUOM, False, True)
                    End If
                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_CURRENCY_CODE" Then
                        cboCurrencyCode.Enabled = True
                        cboCurrencyCode.CssClass = "modtxtbox"
                        Common.SelDdl(Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE")), cboCurrencyCode, False, True)
                    End If
                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_TAX_ID" Then
                        cboTax.Enabled = True
                        cboTax.CssClass = "modtxtbox"
                        Common.SelDdl(Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE")), cboTax, False, True)
                    End If

                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_UNIT_COST" Then
                        txtPrice.Enabled = True
                        txtPrice.CssClass = "modtxtbox"
                        txtPrice.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If

                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_DRAW_NO" Then
                        txtDrawingNumber.Enabled = True
                        txtDrawingNumber.CssClass = "modtxtbox"
                        txtDrawingNumber.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If

                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_VERS_NO" Then
                        txtVersionNo.Enabled = True
                        txtVersionNo.CssClass = "modtxtbox"
                        txtVersionNo.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If


                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_GROSS_WEIGHT" Then
                        txtGrossWeight.Enabled = True
                        txtGrossWeight.CssClass = "modtxtbox"
                        txtGrossWeight.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If
                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_NET_WEIGHT" Then
                        txtNetWeight.Enabled = True
                        txtNetWeight.CssClass = "modtxtbox"
                        txtNetWeight.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If
                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_LENGHT" Then
                        txtLength.Enabled = True
                        txtLength.CssClass = "modtxtbox"
                        txtLength.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If
                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_WIDTH" Then
                        txtWidth.Enabled = True
                        txtWidth.CssClass = "modtxtbox"
                        txtWidth.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If
                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_HEIGHT" Then
                        txtHeight.Enabled = True
                        txtHeight.CssClass = "modtxtbox"
                        txtHeight.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If
                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_VOLUME" Then
                        txtVolume.Enabled = True
                        txtVolume.CssClass = "modtxtbox"
                        txtVolume.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If
                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_COLOR_INFO" Then
                        txtColorInfo.Enabled = True
                        txtColorInfo.CssClass = "modtxtbox"
                        txtColorInfo.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If
                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_HSC_CODE" Then
                        txtHSCode.Enabled = True
                        txtHSCode.CssClass = "modtxtbox"
                        txtHSCode.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If
                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_PACKING_REQ" Then
                        txtPacking.Enabled = True
                        txtPacking.CssClass = "modtxtbox"
                        txtPacking.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If
                    If ds.Tables(1).Rows(i)("PMT_FIELD_NAME") = "PM_REMARKS" Then
                        txtRemarks.Enabled = True
                        txtRemarks.CssClass = "modtxtbox"
                        txtRemarks.Text = Common.parseNull(ds.Tables(1).Rows(i)("PMT_VALUE"))
                    End If


                Next
            End If
        End If
    End Function

    Private Function Bindgrid() As String
        'Dim ds As New DataSet
        'ds = objCat.getDiscountGroupList(viewstate("itemid"), "D", Session("CompanyIdToken"))
        'intPageRecordCnt = ds.Tables(0).Rows.Count

        'Dim dvViewSample As DataView
        'dvViewSample = ds.Tables(0).DefaultView

        'If intPageRecordCnt > 0 Then
        '    dtgCatalogue.DataSource = dvViewSample
        '    dtgCatalogue.DataBind()
        'Else
        '    dtgCatalogue.DataBind()
        'End If
    End Function

    Private Function BindCat() As String
        'Dim ds As New DataSet
        'ds = objCat.getDiscountGroupList(viewstate("itemid"), "C", Session("CompanyIdToken"))
        'intPageRecordCnt = ds.Tables(0).Rows.Count

        'Dim dvViewSample As DataView
        'dvViewSample = ds.Tables(0).DefaultView

        'If intPageRecordCnt > 0 Then
        '    dtgCat.DataSource = dvViewSample
        '    dtgCat.DataBind()
        'Else
        '    dtgCat.DataBind()
        'End If
    End Function

    Private Sub displayAttachFile()
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String
        'If viewstate("type") = "A" Then
        '    dsAttach = objCat.getTempAttach(viewstate("itemid"), True)
        'Else
        'If ViewState("mode") = "mod" Then
        dsAttach = objCat.getTempAttach(ViewState("itemid"), True) ' False)
        'Else
        'dsAttach = objCat.getTempAttach(Session.SessionID, True) ' False)
        'End If
        'End If

        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("PA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("PA_HUB_FILENAME")
                'strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.ProductAttach & ">" & strFile & "</A>"

                Dim objCompany As New Companies
                Dim dsAppPackage As DataSet = objCompany.getAppPackage(Web.HttpContext.Current.Session("CompanyIdToken"))

                If dsAppPackage.Tables(0).Rows.Count > 0 Then
                    Dim j As Integer

                    For j = 0 To dsAppPackage.Tables(0).Rows.Count - 1
                        If dsAppPackage.Tables(0).Rows(j).Item("chk") = "Y" Then
                            strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.ProductAttach, "", EnumUploadFrom.BackOff, ConfigurationSettings.AppSettings(dsAppPackage.Tables(0).Rows(j).Item("ap_app_id") & "Path"))
                            If strURL <> "" Then Exit For
                        End If
                    Next
                End If

                Dim lblBr As New Label
                Dim lblFile As New Label
                Dim lnk As New ImageButton
                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("PA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"
                'lnk.ImageUrl = "../images/i_delete2.gif"
                'lnk.ImageUrl = dDispatcher.direct("Plugins/Images", "i_delete2.gif")
                'lnk.ID = drvAttach(i)("PA_ATTACH_INDEX")
                'lnk.CausesValidation = False
                'AddHandler lnk.Click, AddressOf deleteAttach

                pnlAttach.Controls.Add(lblFile)
                'pnlAttach.Controls.Add(lnk)
                pnlAttach.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFile)
        End If
    End Sub

    'Private Sub deleteAttach(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    If viewstate("type") = "A" Then
    '        objCat.deleteAttachment(CType(sender, ImageButton).ID, True, , "V", "D")
    '    Else
    '        objCat.deleteAttachment(CType(sender, ImageButton).ID, False, , "V", "D")
    '    End If

    '    displayAttachFile()
    'End Sub

    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        If File1.Value <> "" Then
            Dim objFile As New FileManagement
            Dim strDocNo As String

            Dim objCompany As New Companies
            'Dim dsAppPackage As DataSet = objCompany.getAppPackage(Web.HttpContext.Current.Session("CompanyIdToken"))

            Dim objDB As New  EAD.DBCom(ConfigurationSettings.AppSettings("eProcurePath"))
            Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))

            If viewstate("type") = "A" Then
                strDocNo = viewstate("itemid")

                'If dsAppPackage.Tables(0).Rows.Count > 0 Then
                '    Dim i As Integer

                '    For i = 0 To dsAppPackage.Tables(0).Rows.Count - 1
                '        If dsAppPackage.Tables(0).Rows(i).Item("chk") = "Y" Then
                '            objFile.FileUpload(File1, EnumUploadType.ProductAttachment, "", EnumUploadFrom.BackOff, strDocNo, True, , ConfigurationSettings.AppSettings(dsAppPackage.Tables(0).Rows(i).Item("ap_app_id") & "Path"))
                '            Exit For
                '        End If
                '    Next
                'End If

                Dim sFileName As String
                sFileName = System.IO.Path.GetFileName(File1.PostedFile.FileName)

                If Len(sFileName) > 50 Then
                    Common.NetMsgbox(Me, "File name exceeds 46 character")
                ElseIf File1.PostedFile.ContentLength > 0 And File1.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                    objFile.FileUpload(File1, EnumUploadType.ProductAttachment, "", EnumUploadFrom.BackOff, strDocNo, True, , ConfigurationSettings.AppSettings("eProcurePath"))
                ElseIf File1.PostedFile.ContentLength = 0 Then
                    Common.NetMsgbox(Me, "0 byte document or file not found")
                Else
                    Common.NetMsgbox(Me, "File exceeds maximum file size")
                End If
            Else
                If viewstate("mode") = "mod" Then
                    strDocNo = viewstate("itemid")
                Else
                    strDocNo = Session.SessionID
                End If

                'If dsAppPackage.Tables(0).Rows.Count > 0 Then
                '    Dim i As Integer

                '    For i = 0 To dsAppPackage.Tables(0).Rows.Count - 1
                '        If dsAppPackage.Tables(0).Rows(i).Item("chk") = "Y" Then
                '            objFile.FileUpload(File1, EnumUploadType.ProductAttachment, "", EnumUploadFrom.BackOff, strDocNo, True, , ConfigurationSettings.AppSettings(dsAppPackage.Tables(0).Rows(i).Item("ap_app_id") & "Path"))
                '            Exit For
                '        End If
                '    Next
                'End If

                Dim sFileName As String
                sFileName = System.IO.Path.GetFileName(File1.PostedFile.FileName)

                If Len(sFileName) > 50 Then
                    Common.NetMsgbox(Me, "File name exceeds 46 character")
                ElseIf File1.PostedFile.ContentLength > 0 And File1.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                    objFile.FileUpload(File1, EnumUploadType.ProductAttachment, "", EnumUploadFrom.BackOff, strDocNo, True, , ConfigurationSettings.AppSettings("eProcurePath"))
                ElseIf File1.PostedFile.ContentLength = 0 Then
                    Common.NetMsgbox(Me, "0 byte document or file not found")
                Else
                    Common.NetMsgbox(Me, "File exceeds maximum file size")
                End If
            End If

            displayAttachFile()
            displayImageFile(Session("ImageIndex"))
            objFile = Nothing
        End If
    End Sub

    Private Function isValidPrice(ByVal strVal As String) As Boolean
        If IsNumeric(strVal) Then
            If CDbl(strVal) > 0 And CDbl(strVal) < 10000000000 Then
                isValidPrice = True
            Else
                isValidPrice = False
            End If
        Else
            isValidPrice = False
        End If
    End Function

    Private Function validateDatagrid(ByRef strMsg As String) As Boolean
        validateDatagrid = True
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim txtQ As TextBox
        Dim txtQ2 As TextBox
        Dim txtP2 As TextBox
        Dim txtP As TextBox
        'Dim txtRemark As TextBox
        'Dim strErr As String
        strMsg = "<ul type='disc'>"
        ' Select Case viewstate("type")
        'Case "A" ' approval - remark only
        'If Not Common.checkMaxLength(txtRemarks.Text, 1000) Then
        '    strMsg &= "<li>Remarks is over limit.<ul type='disc'></ul></li>"
        '    validateDatagrid = False
        'End If
        'Case "O" ' other - for datagrid's remark
        'For Each dgItem In dtgCatalogue.Items
        '    txtRemark = dgItem.FindControl("txtRemark")
        '    txtQ = dgItem.FindControl("txtQ")
        '    'txtQ2 = dgItem.FindControl("txtQ2")
        '    'txtP2 = dgItem.FindControl("txtP2")
        '    txtP = dgItem.FindControl("txtP")
        '    chk = dgItem.FindControl("chkSelection")
        '    If chk.Checked Then
        '        If Not isValidPrice(CType(dgItem.FindControl("txtPrice"), TextBox).Text) Then
        '            strMsg &= "<li>" & dgItem.Cells(EnumCat.icCode).Text & "- Discount Price is over limit/expecting numeric value.<ul type='disc'></ul></li>"
        '            txtP.Text = "?"
        '            validateDatagrid = False
        '            'strMsg = "<ul type='disc'><li>" & dgItem.Cells(EnumCat.icCode).Text & " - Invalid Discount Price.<ul type='disc'></ul></li>"
        '        Else
        '            txtP.Text = ""
        '        End If
        '        If Not Common.checkMaxLength(txtRemark.Text, 400) Then
        '            strMsg &= "<li>" & dgItem.Cells(EnumCat.icCode).Text & " - Discount Remarks is over limit.<ul type='disc'></ul></li>"
        '            txtQ.Text = "?"
        '            validateDatagrid = False
        '        Else
        '            txtQ.Text = ""
        '        End If
        '    Else
        '        txtP.Text = ""
        '        txtQ.Text = ""
        '    End If
        'Next

        'For Each dgItem In dtgCat.Items
        '    txtRemark = dgItem.FindControl("txtContRemark")
        '    'txtQ = dgItem.FindControl("txtQ")
        '    txtQ2 = dgItem.FindControl("txtQ2")
        '    txtP2 = dgItem.FindControl("txtP2")
        '    'txtP = dgItem.FindControl("txtP")
        '    chk = dgItem.FindControl("chkSelection2")
        '    If chk.Checked Then
        '        If Not isValidPrice(CType(dgItem.FindControl("txtContPrice"), TextBox).Text) Then
        '            strMsg &= "<li>" & dgItem.Cells(EnumCat.icCode).Text & " - Contract Price is over limit/expecting numeric value.<ul type='disc'></ul></li>"
        '            txtP2.Text = "?"
        '            validateDatagrid = False
        '        Else
        '            txtP2.Text = ""
        '        End If
        '        If Not Common.checkMaxLength(txtRemark.Text, 400) Then
        '            strMsg &= "<li>" & dgItem.Cells(EnumCat.icCode).Text & " - Contract Remarks is over limit.<ul type='disc'></ul></li>"
        '            txtQ2.Text = "?"
        '            validateDatagrid = False
        '        Else
        '            txtQ2.Text = ""
        '        End If
        '    Else
        '        txtP2.Text = ""
        '        txtQ2.Text = ""
        '    End If
        'Next

        If Not Common.checkMaxLength(txtRemark.Text, 400) Then
            strMsg &= "<li>Remarks is over limit.<ul type='disc'></ul></li>"
            validateDatagrid = False
        End If

        'End Select

        'If blnRemark Then
        '    If txtRemarks.Text = "" Then
        '        strMsg &= "<li>Remarks is required.<ul type='disc'></ul></li>"
        '        validateDatagrid = False
        '    End If
        'End If
        strMsg &= "</ul>"
    End Function

    Private Function insertRow(ByRef dtProduct As DataTable, ByVal strField As String, ByVal strValue As String)
        Dim dtr As DataRow
        dtr = dtProduct.NewRow()
        dtr("ProductIndex") = viewstate("itemindex")
        dtr("ProductCode") = lblItemId.Value
        dtr("FieldName") = strField
        dtr("Value") = strValue
        dtProduct.Rows.Add(dtr)
    End Function

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        vldSumm.ShowSummary = True
        Dim strMsg As String
        Dim blnSend As Boolean
        Dim strNewCode As String
        Dim dsProduct As New DataSet
        Dim dtProduct As New DataTable
        blnList = False
        blnDiscount = False

        If Page.IsValid And validateDatagrid(strMsg) Then ' make sure discount price is entered correctly
            'If validateDatagrid(strMsg) Then
            lblMsg.Text = ""
            dsProduct = bindProduct()
            strNewCode = objCat.insertProductMstr(dsProduct, viewstate("mode"), Session("ImageIndex"), viewstate("oldVIC"), blnList, blnDiscount)
            If strNewCode <> "" Then
                'Response.Redirect("HubCatalogueConfirm.aspx?pageid=" & strPageId & "&act=5&code=" & strNewCode)
                'Response.Redirect(dDispatcher.direct("Catalogue", "HubCatalogueConfirm.aspx", "pageid=" & strPageId & "&act=5&code=" & strNewCode))
                Common.NetMsgbox(Me, "Item has been approved.", dDispatcher.direct("Catalogue", "HubListPriceCatalogue.aspx", "type=" & ViewState("type") & "&pageid=" & strPageId))
                Session("ImageIndex") = ""
            Else
                Common.NetMsgbox(Me, "Duplicate Vendor Item Code found.", dDispatcher.direct("Catalogue", "HubListPriceCatalogue.aspx", "type=" & ViewState("type") & "&pageid=" & strPageId))
            End If
            'Else
            '    lblMsg.Text = strMsg
            'End If
        Else
            If strMsg <> "" Then
                lblMsg.Text = strMsg
            Else
                lblMsg.Text = ""
            End If
        End If
    End Sub

    Private Function bindProduct() As DataSet
        Dim ds As New DataSet
        Dim dtProduct As New DataTable
        'dtProduct.Columns.Add("CoyId", Type.GetType("System.String"))
        'dtProduct.Columns.Add("ProductIndex", Type.GetType("System.String"))
        'dtProduct.Columns.Add("ProductCode", Type.GetType("System.String"))
        'dtProduct.Columns.Add("CategoryName", Type.GetType("System.String"))
        'dtProduct.Columns.Add("VendorItemCode", Type.GetType("System.String"))
        'dtProduct.Columns.Add("ProductDesc", Type.GetType("System.String"))
        'dtProduct.Columns.Add("UOM", Type.GetType("System.String"))
        'dtProduct.Columns.Add("UnitCost", Type.GetType("System.String"))
        'dtProduct.Columns.Add("gstcode", Type.GetType("System.String"))
        ''dtProduct.Columns.Add("mgmtcode", Type.GetType("System.String"))
        ''dtProduct.Columns.Add("mgmttext", Type.GetType("System.String"))
        'dtProduct.Columns.Add("currencycode", Type.GetType("System.String"))
        'dtProduct.Columns.Add("productbrand", Type.GetType("System.String"))
        'dtProduct.Columns.Add("productmodel", Type.GetType("System.String"))
        'dtProduct.Columns.Add("status", Type.GetType("System.Int32"))
        'dtProduct.Columns.Add("action", Type.GetType("System.String"))

        dtProduct.Columns.Add("CoyId", Type.GetType("System.String"))   'PM_S_COY_ID
        dtProduct.Columns.Add("ProductIndex", Type.GetType("System.String"))
        dtProduct.Columns.Add("ProductCode", Type.GetType("System.String")) 'PM_PRODUCT_CODE
        dtProduct.Columns.Add("VendorItemCode", Type.GetType("System.String"))  'PM_VENDOR_ITEM_CODE
        dtProduct.Columns.Add("ItemName", Type.GetType("System.String"))    'PM_PRODUCT_DESC ' dtProduct.Columns.Add("ProductDesc", Type.GetType("System.String"))
        dtProduct.Columns.Add("ReferenceNo", Type.GetType("System.String")) 'PM_REF_NO
        dtProduct.Columns.Add("Description", Type.GetType("System.String")) 'PM_LONG_DESC
        dtProduct.Columns.Add("CommodityType", Type.GetType("System.String")) 'PM_CATEGORY_NAME 'dtProduct.Columns.Add("CategoryName", Type.GetType("System.String"))
        dtProduct.Columns.Add("Tax", Type.GetType("System.String")) 'PM_GST_CODE 'dtProduct.Columns.Add("gstcode", Type.GetType("System.String"))
        dtProduct.Columns.Add("Perc", Type.GetType("System.String"))
        dtProduct.Columns.Add("TaxId", Type.GetType("System.String"))
        dtProduct.Columns.Add("UOM", Type.GetType("System.String")) 'PM_UOM
        dtProduct.Columns.Add("Price", Type.GetType("System.String")) 'PM_UNIT_COST        dtProduct.Columns.Add("UnitCost", Type.GetType("System.String"))
        dtProduct.Columns.Add("CurrencyCode", Type.GetType("System.String")) 'PM_CURRENCY_CODE
        dtProduct.Columns.Add("Brand", Type.GetType("System.String")) 'PM_PRODUCT_BRAND
        dtProduct.Columns.Add("Model", Type.GetType("System.String")) 'PM_PRODUCT_MODEL

        dtProduct.Columns.Add("DrawingNo", Type.GetType("System.String")) 'PM_DRAW_NO
        dtProduct.Columns.Add("VersionNo", Type.GetType("System.String")) 'PM_VERS_NO
        dtProduct.Columns.Add("GrossWeight", Type.GetType("System.String")) 'PM_GROSS_WEIGHT
        dtProduct.Columns.Add("NetWeight", Type.GetType("System.String")) 'PM_NET_WEIGHT
        dtProduct.Columns.Add("Length", Type.GetType("System.String")) 'PM_LENGHT
        dtProduct.Columns.Add("Width", Type.GetType("System.String")) 'PM_WIDTH
        dtProduct.Columns.Add("Height", Type.GetType("System.String")) 'PM_HEIGHT
        dtProduct.Columns.Add("Volume", Type.GetType("System.String")) 'PM_VOLUME
        dtProduct.Columns.Add("ColorInfo", Type.GetType("System.String")) 'PM_COLOR_INFO
        dtProduct.Columns.Add("HSCode", Type.GetType("System.String")) 'PM_HSC_CODE
        dtProduct.Columns.Add("Packing", Type.GetType("System.String")) 'PM_PACKING_REQ
        dtProduct.Columns.Add("Remark", Type.GetType("System.String")) 'PM_REMARK
        dtProduct.Columns.Add("Remarks", Type.GetType("System.String")) 'PM_REMARKS
        dtProduct.Columns.Add("PM_PRODUCT_FOR", Type.GetType("System.String")) 'PM_PRODUCT_FOR

        dtProduct.Columns.Add("status", Type.GetType("System.Int32"))
        dtProduct.Columns.Add("action", Type.GetType("System.String"))


        'Dim dtr As DataRow
        'dtr = dtProduct.NewRow()
        'dtr("CoyId") = HttpContext.Current.Session("CompanyIdToken")
        'dtr("ProductCode") = viewstate("itemid")
        'dtr("ProductIndex") = viewstate("itemindex")
        ''dtr("CategoryName") = hidCategoryId.Value ' txtCategory.Text
        'dtr("VendorItemCode") = txtVendorItemCode.Text
        'dtr("ProductDesc") = txtDesc.Text
        'dtr("UOM") = cboUOM.SelectedItem.Text
        'dtr("UnitCost") = txtCatPrice.Text
        'dtr("gstcode") = cboTax.SelectedItem.Value

        'If cboMgmt.SelectedIndex <> -1 Then
        '    dtr("mgmtcode") = cboMgmt.SelectedItem.Value
        'Else
        '    dtr("mgmtcode") = ""
        'End If

        'dtr("mgmttext") = txtMgmtCode.Text
        'dtr("currencycode") = cboCurrencyCode.SelectedItem.Value
        'dtr("productbrand") = txtBrand.Text
        'dtr("productmodel") = txtModel.Text
        Dim dtr As DataRow
        dtr = dtProduct.NewRow()
        dtr("CoyId") = HttpContext.Current.Session("CompanyIdToken")
        'dtr("ProductCode") = strItemId ' lblItemId.Text
        dtr("ProductCode") = lblItemId.Value  ' lblItemId.Text

        dtr("VendorItemCode") = txtVendorItemCode.Text
        dtr("ItemName") = txtItemName.Text ' dtProduct.Columns.Add("ProductDesc", Type.GetType("System.String"))
        dtr("ReferenceNo") = txtRefNo.Text
        dtr("Description") = txtDesc.Text
        dtr("CommodityType") = Me.hidCommodityType.Value 'txtCommodityType.Text 'dtr("CategoryName", Type.GetType("System.String"))
        dtr("Tax") = cboTax.SelectedItem.Text 'dtr("gstcode", Type.GetType("System.String"))
        dtr("Perc") = cboTax.SelectedItem.Text
        dtr("TaxId") = cboTax.SelectedItem.Value
        dtr("UOM") = cboUOM.SelectedItem.Text
        dtr("Price") = txtPrice.Text '        dtr("UnitCost", Type.GetType("System.String"))
        dtr("CurrencyCode") = cboCurrencyCode.SelectedItem.Value
        dtr("Brand") = txtBrand.Text
        dtr("Model") = txtModel.Text

        dtr("DrawingNo") = txtDrawingNumber.Text
        dtr("VersionNo") = txtVersionNo.Text
        dtr("GrossWeight") = txtGrossWeight.Text
        dtr("NetWeight") = txtNetWeight.Text
        dtr("Length") = txtLength.Text
        dtr("Width") = txtWidth.Text
        dtr("Height") = txtHeight.Text
        dtr("Volume") = txtVolume.Text
        dtr("ColorInfo") = txtColorInfo.Text
        dtr("HSCode") = txtHSCode.Text
        dtr("Packing") = txtPacking.Text
        dtr("Remarks") = txtRemarks.Text
        dtr("Remark") = txtRemark.Text
        dtr("PM_PRODUCT_FOR") = "V"

        dtr("status") = ListPriceStatus.PendingApproval
        If viewstate("mode") = "add" Then
            dtr("action") = "N" ' new
        Else
            dtr("action") = "M" ' modify
        End If
        dtProduct.Rows.Add(dtr)
        ds.Tables.Add(dtProduct)

        'If IsNumeric(txtCatPrice.Text) And IsNumeric(hidCataloguePrice.Value) Then
        '    If CDbl(hidCataloguePrice.Value) <> CDbl(txtCatPrice.Text) Then
        '        blnList = True
        '    End If
        'ElseIf hidCataloguePrice.Value = "" And txtCatPrice.Text <> "" Then
        '    blnList = True
        'End If

        '' Discount Group
        'Dim dtDisc As New DataTable
        'dtDisc.Columns.Add("DiscIndex", Type.GetType("System.String"))
        'dtDisc.Columns.Add("DiscCode", Type.GetType("System.String"))
        'dtDisc.Columns.Add("DiscPrice", Type.GetType("System.String"))
        'dtDisc.Columns.Add("DiscRemark", Type.GetType("System.String"))
        'dtDisc.Columns.Add("chk", Type.GetType("System.String"))
        'dtDisc.Columns.Add("Change", Type.GetType("System.Int32"))

        'Dim dgItem As DataGridItem
        'Dim chk As CheckBox
        'Dim hidChk As HtmlInputHidden
        'Dim hidPrice As HtmlInputHidden
        ' discount catalogue
        'For Each dgItem In dtgCatalogue.Items
        '    chk = dgItem.FindControl("chkSelection")
        '    hidChk = dgItem.FindControl("hidCheck")
        '    hidPrice = dgItem.FindControl("hidPrice")
        '    dtr = dtDisc.NewRow()
        '    dtr("DiscIndex") = dgItem.Cells(EnumCat.icIndex).Text
        '    dtr("DiscCode") = dgItem.Cells(EnumCat.icCode).Text
        '    If chk.Checked Then
        '        If hidChk.Value = 0 Then
        '            blnDiscount = True
        '            dtr("Change") = 1
        '        Else
        '            dtr("Change") = 0
        '        End If
        '        dtr("DiscPrice") = CType(dgItem.FindControl("txtPrice"), TextBox).Text
        '        If CType(dgItem.FindControl("txtPrice"), TextBox).Text <> hidPrice.Value Then
        '            blnDiscount = True
        '            dtr("Change") = 1
        '        End If
        '        dtr("DiscRemark") = CType(dgItem.FindControl("txtRemark"), TextBox).Text
        '        dtr("chk") = "1"
        '    Else
        '        If hidChk.Value = 1 Then
        '            blnDiscount = True
        '            dtr("Change") = 1
        '        Else
        '            dtr("Change") = 0
        '        End If
        '        dtr("chk") = "0"
        '    End If
        '    dtDisc.Rows.Add(dtr)
        'Next

        'If viewstate("type") <> "O" Then
        '    ds.Tables.Add(dtDisc)
        'Else
        '    ' contract catalogue
        '    For Each dgItem In dtgCat.Items
        '        chk = dgItem.FindControl("chkSelection2")
        '        dtr = dtDisc.NewRow()
        '        dtr("DiscIndex") = dgItem.Cells(EnumCat.icIndex).Text
        '        dtr("DiscCode") = dgItem.Cells(EnumCat.icCode).Text
        '        If chk.Checked Then
        '            dtr("DiscPrice") = CType(dgItem.FindControl("txtContPrice"), TextBox).Text
        '            dtr("DiscRemark") = CType(dgItem.FindControl("txtContRemark"), TextBox).Text
        '            dtr("chk") = "1"
        '        Else
        '            dtr("chk") = "0"
        '        End If
        '        dtr("Change") = 0
        '        dtDisc.Rows.Add(dtr)
        '    Next
        '    ds.Tables.Add(dtDisc)
        'End If

        bindProduct = ds
    End Function

    Private Sub dtgCatalogue_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemCreated
        Grid_ItemCreated(dtgCatalogue, e)
    End Sub

    Private Sub dtgCatalogue_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim chk As CheckBox
            chk = e.Item.Cells(EnumCat.icChk).FindControl("chkSelection")

            Dim hidCode As HtmlInputHidden
            hidCode = e.Item.FindControl("hidCode")
            hidCode.Value = dv("PDT_DISC_CODE")

            Dim strChk As String
            strChk = Common.parseNull(dv("CHK"))
            Dim hidCheck As HtmlInputHidden
            hidCheck = e.Item.FindControl("hidCheck")
            If strChk = "1" Then
                chk.Checked = True
                hidCheck.Value = 1
            Else
                chk.Checked = False
                hidCheck.Value = 0
            End If

            Dim txtPrice As TextBox
            txtPrice = e.Item.FindControl("txtPrice")
            Dim hidPrice As HtmlInputHidden
            hidPrice = e.Item.FindControl("hidPrice")
            If Not IsDBNull(dv("PDT_DISC_PRICE")) Then
                txtPrice.Text = Format(CDbl(dv("PDT_DISC_PRICE")), "#0.0000")
                hidPrice.Value = Format(CDbl(dv("PDT_DISC_PRICE")), "#0.0000")
            Else
                txtPrice.Text = ""
                hidPrice.Value = ""
            End If

            Dim txtRemark As TextBox
            txtRemark = e.Item.FindControl("txtRemark")
            txtRemark.Attributes.Add("onKeyDown", "limitText (this, 400);")
            txtRemark.Text = Common.parseNull(dv("PDT_DISC_REMARK"))

            If viewstate("status") = "2" Then
                txtPrice.Enabled = False
                txtRemark.Enabled = False
                chk.Enabled = False
            End If

            ' for '?' purpose
            Dim txtQ As TextBox
            txtQ = e.Item.FindControl("txtQ")

            If hidSummary.Value = "" Then
                hidSummary.Value = "Discount Remarks-" & txtRemark.ClientID
            Else
                hidSummary.Value &= ",Discount Remarks-" & txtRemark.ClientID
            End If

            If hidControl.Value = "" Then
                hidControl.Value = hidCode.ClientID & "-" & txtQ.ClientID
            Else
                hidControl.Value &= "," & hidCode.ClientID & "-" & txtQ.ClientID
            End If
        End If
    End Sub

    Private Sub dtgCat_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCat.ItemCreated
        Grid_ItemCreated(dtgCat, e)
    End Sub

    Private Sub dtgCat_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCat.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim chk As CheckBox
            chk = e.Item.Cells(EnumCat.icChk).FindControl("chkSelection2")

            Dim hidCode As HtmlInputHidden
            hidCode = e.Item.FindControl("hidCode2")
            hidCode.Value = dv("PDT_DISC_CODE")

            Dim strChk As String
            strChk = Common.parseNull(dv("CHK"))
            If strChk = "1" Then
                chk.Checked = True
            Else
                chk.Checked = False
            End If

            Dim txtContPrice As TextBox
            txtContPrice = e.Item.FindControl("txtContPrice")
            If Not IsDBNull(dv("PDT_DISC_PRICE")) Then
                txtContPrice.Text = Format(CDbl(dv("PDT_DISC_PRICE")), "#0.0000")
            End If

            Dim txtContRemark As TextBox
            txtContRemark = e.Item.FindControl("txtContRemark")
            txtContRemark.Attributes.Add("onKeyDown", "limitText (this, 400);")
            txtContRemark.Text = Common.parseNull(dv("PDT_DISC_REMARK"))

            Dim txtQ As TextBox
            txtQ = e.Item.FindControl("txtQ2")

            If hidSummary.Value = "" Then
                hidSummary.Value = "Contract Remarks-" & txtContRemark.ClientID
            Else
                hidSummary.Value &= ",Contract Remarks-" & txtContRemark.ClientID
            End If

            If hidControl.Value = "" Then
                hidControl.Value = hidCode.ClientID & "-" & txtQ.ClientID
            Else
                hidControl.Value &= "," & hidCode.ClientID & "-" & txtQ.ClientID
            End If
        End If
    End Sub

    Private Sub cmdApprove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApprove.Click
        vldSumm.ShowSummary = True
        Dim dsItem As New DataSet
        Dim strMsg As String
        Dim blnUpdate As Boolean
        Dim strUpdate(25) As String
        strUpdate(0) = viewstate("itemindex")
        strUpdate(11) = viewstate("itemid")
        If Page.IsValid And validateDatagrid(strMsg) Then
            dsItem = bindProduct()
            getChangesUpdate(strUpdate)
            blnUpdate = objCat.updateProductMstr(True, viewstate("action"), Session("CompanyIdToken"), dsItem, strUpdate, True, Session("ImageIndex"), viewstate("oldVIC"))
            If blnUpdate Then
                cmdApprove.Visible = False
                cmdReject.Visible = False
                'Response.Redirect("HubCatalogueConfirm.aspx?act=3&code=" & viewstate("itemid") & "&pageid=" & strPageId)
                'Response.Redirect(dDispatcher.direct("Catalogue", "HubCatalogueConfirm.aspx", "act=3&code=" & ViewState("itemid") & "&pageid=" & strPageId))
                Common.NetMsgbox(Me, "Item has been approved.", dDispatcher.direct("Catalogue", "HubListPriceCatalogue.aspx", "type=" & ViewState("type") & "&pageid=" & strPageId))
            Else
                Common.NetMsgbox(Me, "Duplicate Vendor Item Code found.", dDispatcher.direct("Catalogue", "HubListPriceCatalogue.aspx", "type=" & ViewState("type") & "&pageid=" & strPageId))
            End If
        Else
            If strMsg <> "" Then
                lblMsg.Text = strMsg
            Else
                lblMsg.Text = ""
            End If
        End If

    End Sub

    Private Sub cmdReject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReject.Click
        vldSumm.ShowSummary = True
        lblMsg.Text = ""
        Dim strUpdate(25) As String
        Dim strMsg As String
        Dim blnUpdate As Boolean
        Dim ds As DataSet

        resetQ()
        If txtRemark.Text <> "" Then
            If Not Common.checkMaxLength(txtRemark.Text, 1000) Then
                lblMsg.Text = "<ul type='disc'><li>Remarks is over limit.<ul type='disc'></ul></li></ul>"
                Exit Sub
            End If
            strUpdate(0) = ViewState("itemindex")
            strUpdate(25) = txtRemark.Text
            strUpdate(11) = ViewState("itemid")
            blnUpdate = objCat.updateProductMstr(False, ViewState("action"), Session("CompanyIdToken"), ds, strUpdate, True, Session("ImageIndex"), ViewState("oldVIC"))
            If blnUpdate Then
                cmdReject.Visible = False
                cmdApprove.Visible = False
                'Response.Redirect("HubCatalogueConfirm.aspx?act=4&code=" & ViewState("itemid") & "&pageid=" & strPageId)
                'Response.Redirect(dDispatcher.direct("Catalogue", "HubCatalogueConfirm.aspx", "act=4&code=" & ViewState("itemid") & "&pageid=" & strPageId))
                Common.NetMsgbox(Me, "Item has been rejected.", dDispatcher.direct("Catalogue", "HubListPriceCatalogue.aspx", "type=" & ViewState("type") & "&pageid=" & strPageId))
            End If
        Else
            lblMsg.Text = "<ul type='disc'><li>Remarks is required.<ul type='disc'></ul></li></ul>"
        End If



    End Sub

    Private Function resetQ()
        'carol(to clear the ? mark)
        Dim dgItem As DataGridItem
        Dim txtQ As TextBox
        Dim txtQ2 As TextBox
        Dim txtP2 As TextBox
        Dim txtP As TextBox
        'For Each dgItem In dtgCatalogue.Items
        '    txtQ = dgItem.FindControl("txtQ")
        '    txtP = dgItem.FindControl("txtP")

        '    txtP.Text = ""
        '    txtQ.Text = ""
        'Next

        'For Each dgItem In dtgCat.Items
        '    txtQ = dgItem.FindControl("txtQ")
        '    txtP = dgItem.FindControl("txtP")

        '    txtP2.Text = ""
        '    txtQ2.Text = ""
        'Next
    End Function

    Private Function getChangesUpdate(ByRef strUpdate() As String)
        'Reserved Value
        'strUpdate(0) = ViewState("itemindex")
        'strUpdate(11) = ViewState("itemid")

        'If txtCommodityType.Enabled Then
        strUpdate(1) = hidCommodityType.Value
        'End If
        If txtItemName.Enabled Then
            strUpdate(2) = txtItemName.Text
        End If
        If txtRefNo.Enabled Then
            strUpdate(3) = txtRefNo.Text
        End If
        If txtVendorItemCode.Enabled Then
            strUpdate(4) = txtVendorItemCode.Text
        End If
        If txtDesc.Enabled Then
            strUpdate(5) = txtDesc.Text
        End If
        If txtBrand.Enabled Then
            strUpdate(6) = txtBrand.Text
        End If
        If txtModel.Enabled Then
            strUpdate(7) = txtModel.Text
        End If
        If cboUOM.Enabled Then
            strUpdate(8) = cboUOM.SelectedItem.Text
        End If
        If cboCurrencyCode.Enabled Then
            strUpdate(9) = cboCurrencyCode.SelectedItem.Value
        End If
        If cboTax.Enabled Then
            strUpdate(10) = cboTax.SelectedItem.Value
        End If
        If txtPrice.Enabled Then
            strUpdate(12) = txtPrice.Text
        End If
        If txtDrawingNumber.Enabled Then
            strUpdate(13) = txtDrawingNumber.Text
        End If
        If txtVersionNo.Enabled Then
            strUpdate(14) = txtVersionNo.Text
        End If
        If txtGrossWeight.Enabled Then
            strUpdate(15) = txtGrossWeight.Text
        End If
        If txtNetWeight.Enabled Then
            strUpdate(16) = txtNetWeight.Text
        End If
        If txtLength.Enabled Then
            strUpdate(17) = txtLength.Text
        End If
        If txtWidth.Enabled Then
            strUpdate(18) = txtWidth.Text
        End If
        If txtHeight.Enabled Then
            strUpdate(19) = txtHeight.Text
        End If
        If txtVolume.Enabled Then
            strUpdate(20) = txtVolume.Text
        End If
        If txtColorInfo.Enabled Then
            strUpdate(21) = txtColorInfo.Text
        End If
        If txtHSCode.Enabled Then
            strUpdate(22) = txtHSCode.Text
        End If
        If txtPacking.Enabled Then
            strUpdate(23) = txtPacking.Text
        End If
        If txtRemarks.Enabled Then
            strUpdate(24) = txtRemarks.Text
        End If
        If txtRemark.Enabled Then
            strUpdate(25) = txtRemark.Text
        End If
    End Function

    Private Sub cmdUploadImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUploadImage.Click
        If FileProImage.Value <> "" Then
            Dim objFile As New FileManagement
            Dim strDocNo As String

            Dim objCompany As New Companies
            'Dim dsAppPackage As DataSet = objCompany.getAppPackage(Web.HttpContext.Current.Session("CompanyIdToken"))

            Dim objDB As New  EAD.DBCom(ConfigurationSettings.AppSettings("eProcurePath"))
            Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'ImageAttachSize'"))

            If viewstate("type") = "A" Then
                strDocNo = viewstate("itemid")

                'If dsAppPackage.Tables(0).Rows.Count > 0 Then
                '    Dim i As Integer

                '    For i = 0 To dsAppPackage.Tables(0).Rows.Count - 1
                '        If dsAppPackage.Tables(0).Rows(i).Item("chk") = "Y" Then
                '            Session("ImageIndex") = objFile.FileUpload(FileProImage, EnumUploadType.ProductImage, "", EnumUploadFrom.BackOff, strDocNo, True, Session("ImageIndex"), ConfigurationSettings.AppSettings(dsAppPackage.Tables(0).Rows(i).Item("ap_app_id") & "Path"))
                '            If Session("ImageIndex") <> "" Then Exit For
                '        End If
                '    Next
                'End If

                Dim sFileName As String
                sFileName = System.IO.Path.GetFileName(FileProImage.PostedFile.FileName)

                If Len(sFileName) > 50 Then
                    Common.NetMsgbox(Me, "File name exceeds 46 character")
                ElseIf FileProImage.PostedFile.ContentLength > 0 And FileProImage.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                    Session("ImageIndex") = objFile.FileUpload(FileProImage, EnumUploadType.ProductImage, "", EnumUploadFrom.BackOff, strDocNo, True, Session("ImageIndex"), ConfigurationSettings.AppSettings("eProcurePath"))
                ElseIf FileProImage.PostedFile.ContentLength = 0 Then
                    Common.NetMsgbox(Me, "0 byte document or file not found")
                Else
                    Common.NetMsgbox(Me, "File exceeds maximum file size")
                End If
            Else
                If viewstate("mode") = "mod" Then
                    strDocNo = viewstate("itemid")
                Else
                    strDocNo = Session.SessionID
                End If

                'If dsAppPackage.Tables(0).Rows.Count > 0 Then
                '    Dim i As Integer

                '    For i = 0 To dsAppPackage.Tables(0).Rows.Count - 1
                '        If dsAppPackage.Tables(0).Rows(i).Item("chk") = "Y" Then
                '            Session("ImageIndex") = objFile.FileUpload(FileProImage, EnumUploadType.ProductImage, "", EnumUploadFrom.BackOff, strDocNo, True, Session("ImageIndex"), ConfigurationSettings.AppSettings(dsAppPackage.Tables(0).Rows(i).Item("ap_app_id") & "Path"))
                '            If Session("ImageIndex") <> "" Then Exit For
                '        End If
                '    Next
                'End If

                Dim sFileName As String
                sFileName = System.IO.Path.GetFileName(FileProImage.PostedFile.FileName)

                If Len(sFileName) > 50 Then
                    Common.NetMsgbox(Me, "File name exceeds 46 character")
                ElseIf FileProImage.PostedFile.ContentLength > 0 And FileProImage.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                    Session("ImageIndex") = objFile.FileUpload(FileProImage, EnumUploadType.ProductImage, "", EnumUploadFrom.BackOff, strDocNo, True, Session("ImageIndex"), ConfigurationSettings.AppSettings("eProcurePath"))
                ElseIf FileProImage.PostedFile.ContentLength = 0 Then
                    Common.NetMsgbox(Me, "0 byte document or file not found")
                Else
                    Common.NetMsgbox(Me, "File exceeds maximum file size")
                End If
            End If

            displayImageFile(Session("ImageIndex"))
            displayAttachFile()
            objFile = Nothing
        End If
    End Sub

    Private Sub displayImageFile(ByVal strIndex As String)
        Dim dsImage As New DataSet
        Dim drvImage As DataView
        Dim objFile As New FileManagement
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp, strCoyId, strBasePath, strDownloadPath, strFilePath As String
        'If viewstate("type") = "A" Then
        '    dsImage = objCat.getTempImageAttach(strIndex, True)
        'Else
        '    dsImage = objCat.getTempImageAttach(strIndex, False)
        'End If
        dsImage = objCat.getTempImageAttach(strIndex, True)

        pnlImage.Controls.Clear()
        drvImage = dsImage.Tables(0).DefaultView
        If drvImage.Count > 0 Then
            For i = 0 To drvImage.Count - 1
                strFile = drvImage(i)("PA_ATTACH_FILENAME")
                strFile1 = drvImage(i)("PA_HUB_FILENAME")


                Dim objCompany As New Companies
                Dim dsAppPackage As DataSet = objCompany.getAppPackage(Web.HttpContext.Current.Session("CompanyIdToken"))

                If dsAppPackage.Tables(0).Rows.Count > 0 Then
                    Dim j As Integer

                    For j = 0 To dsAppPackage.Tables(0).Rows.Count - 1
                        If dsAppPackage.Tables(0).Rows(j).Item("chk") = "Y" Then
                            hidImageFile.Value = objFile.FileDownload(Server.UrlEncode(strFile1), EnumDownLoadType.ProductImage, "", EnumUploadFrom.BackOff, , ConfigurationSettings.AppSettings(dsAppPackage.Tables(0).Rows(j).Item("ap_app_id") & "Path"))
                            If hidImageFile.Value <> "" Then Exit For
                        End If
                    Next
                End If

                strURL = "<A HREF='#' onclick='PreviewImage(1);'>" & strFile & "</A>"
                'strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.ProductImage & ">" & strFile & "</A>"
                Dim lblBr As New Label
                Dim lblFile As New Label
                Dim lnk As New ImageButton
                'lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL '& " (" & drvImage(i)("PA_FILESIZE") & "KB) "
                lblFile.Text = "&nbsp;" & strURL '& " (" & drvImage(i)("PA_FILESIZE") & "KB) "
                'lblBr.Text = "<BR>"
                'lnk.ImageUrl = "../images/i_delete2.gif"
                'lnk.ImageUrl = dDispatcher.direct("Plugins/Images", "i_delete2.gif")
                'strCoyId = HttpContext.Current.Session("CompanyId")
                'strBasePath = objFile.getBasePath(Components.EnumUploadFrom.BackOff)
                'strDownloadPath = objFile.getSystemParam("ProductImagePath", "")
                'strFilePath = strBasePath & strDownloadPath & strCoyId & "\" & strFile1
                'hidImageAttached.Value = strFilePath
                ''strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & ">" & strFile & "</A>"
                'strURL = "<A HREF=""#"" onclick=""PreviewImage('1')"">" & strFile & "</A>"
                'Dim lblBr As New Label
                'Dim lblFile As New Label
                'Dim lnk As New ImageButton
                'lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvImage(i)("PA_FILESIZE") & "KB) "
                'lblBr.Text = "<BR>"
                'lnk.ImageUrl = "../images/i_delete2.gif"
                'lnk.ID = drvImage(i)("PA_ATTACH_INDEX")
                'lnk.CausesValidation = False
                'AddHandler lnk.Click, AddressOf deleteImageAttach

                pnlImage.Controls.Add(lblFile)
                'pnlImage.Controls.Add(lnk)
                pnlImage.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Picture Attached"
            pnlImage.Controls.Add(lblFile)
        End If
    End Sub

    'Private Sub deleteImageAttach(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    If viewstate("type") = "A" Then
    '        objCat.deleteAttachment(CType(sender, ImageButton).ID, True)
    '    Else
    '        objCat.deleteAttachment(CType(sender, ImageButton).ID, False)
    '    End If
    '    displayImageFile(Session("ImageIndex"))
    'End Sub

    Private Sub cmdReset_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReset.ServerClick
        isNotPostBack()
    End Sub
End Class
