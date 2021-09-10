Imports AgoraLegacy
Imports eProcure.Component

Public Class BIM
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objCat As New ContCat

    Dim objGlobal As New AppGlobals
    Dim objFile As New FileManagement
    Dim blnDiscount As Boolean
    Dim cbolist As New ListItem

    'Protected WithEvents txtCategory As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cboPrefer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtPreferVendor As System.Web.UI.WebControls.TextBox
    Protected WithEvents hidPreferVendor As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPreferVendor1st As System.Web.UI.WebControls.TextBox
    Protected WithEvents hidPreferVendor1st As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPreferVendor2nd As System.Web.UI.WebControls.TextBox
    Protected WithEvents hidPreferVendor2nd As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPreferVendor3rd As System.Web.UI.WebControls.TextBox
    Protected WithEvents hidPreferVendor3rd As System.Web.UI.WebControls.TextBox

    'Protected WithEvents cbo1st As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents cbo2nd As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents cbo3rd As System.Web.UI.WebControls.DropDownList
    Protected WithEvents chkStatus As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtVendorItemCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtItemName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRefNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCommodityType As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtAccCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMinInv As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMin As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMax As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBrand As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtModel As System.Web.UI.WebControls.TextBox
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
    Protected WithEvents txtRemarks As System.Web.UI.WebControls.TextBox

    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents lblAttach As System.Web.UI.WebControls.Label
    Protected WithEvents lblAttach2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblPrefer As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1st As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2nd As System.Web.UI.WebControls.Label
    Protected WithEvents lbl3rd As System.Web.UI.WebControls.Label
    Protected WithEvents lblNeedQ As System.Web.UI.WebControls.Label
    Protected WithEvents lblGST As System.Web.UI.WebControls.Label

    'Protected WithEvents hidVendorItemCode As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidItemName As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidItemDesc As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidCommodityType As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidVendorItemCode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidItemName As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidImageAttached As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidBrand As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidModel As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidDrawingNumber As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidVersionNo As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidGrossWeight As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidNetWeight As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidLength As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidWidth As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidHeight As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidVolume As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidColorInfo As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidHSCode As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidPacking As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidImageFile As System.Web.UI.HtmlControls.HtmlInputHidden
    ''Protected WithEvents hidMgmtCode As System.Web.UI.HtmlControls.HtmlInputHidden
    ''Protected WithEvents hidMgmtText As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidPostBack As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidRemarks As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents cboUOM As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboPreferTax As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cbo1stTax As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cbo2ndTax As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cbo3rdTax As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlAccCode As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlCatCode As System.Web.UI.WebControls.DropDownList

    Protected WithEvents vldVItemCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldItemName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldCommodityType As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldUOM As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldCurrencyCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    'Protected WithEvents vldDesc As System.Web.UI.WebControls.RequiredFieldValidator

    Protected WithEvents cmdSelect As System.Web.UI.WebControls.Button
    Protected WithEvents cmdUploadImage As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents cmdUpload As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdPreview As System.Web.UI.WebControls.Button

    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents FileProImage As System.Web.UI.HtmlControls.HtmlInputFile

    Protected WithEvents pnlAttach As System.Web.UI.WebControls.Panel
    Protected WithEvents FileDoc As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents pnlImage As System.Web.UI.WebControls.Panel
    Protected WithEvents cmdadd As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents Image2 As System.Web.UI.WebControls.Image
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image

    Protected WithEvents txtManu As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMaxInvQty As System.Web.UI.WebControls.TextBox

    Protected WithEvents txtVenCodeP As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVenCode1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVenCode2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVenCode3 As System.Web.UI.WebControls.TextBox

    Protected WithEvents txtLeadP As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLead1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLead2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLead3 As System.Web.UI.WebControls.TextBox

    Protected WithEvents rd1 As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rd2 As System.Web.UI.WebControls.RadioButtonList

    Protected WithEvents dr2 As System.Web.UI.HtmlControls.HtmlGenericControl

    'Protected WithEvents revPrice As System.Web.UI.WebControls.RegularExpressionValidator
    Dim blnList As Boolean
    Dim strItemId As String

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

    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    'Protected WithEvents lblItemId As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label


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
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdSave)
        alButtonList.Add(cmdUpload)
        alButtonList.Add(cmdUploadImage)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdSave)
        alButtonList.Add(cmdUpload)
        alButtonList.Add(cmdUploadImage)
        htPageAccess.Add("update", alButtonList)
        CheckButtonAccess()
        cmdSave.Enabled = (blnCanAdd Or blnCanUpdate) And ViewState("blnCmdSubmit")
        cmdUpload.Enabled = (blnCanAdd Or blnCanUpdate) And ViewState("blnCmdUploadDoc")
        cmdUploadImage.Enabled = (blnCanAdd Or blnCanUpdate) And ViewState("blnCmdUploadImg")
        cmdReset.Disabled = Not (blnCanAdd Or blnCanUpdate)
        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnPaging = False
        blnSorting = False

        If Not IsPostBack Then
            Dim objGst As New GST
            ViewState("GSTCOD") = objGst.chkGSTCOD()
            objGst = Nothing
            isNotPostBack()
        End If
        displayImageFile(ViewState("ImageIndex"))
        displayAttachFile()
        lnkBack.NavigateUrl = dDispatcher.direct("BuyerCat", "SearchBItem.aspx", "pageid=" & strPageId)
        cmdUploadImage.Attributes.Add("onclick", "return checkDocFile('img','" & FileProImage.ClientID & "');")
        cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & FileDoc.ClientID & "');")
        Image2.ImageUrl = dDispatcher.direct("Plugins/images", "collapse_up.gif")
        Image1.ImageUrl = dDispatcher.direct("Plugins/images", "collapse_up.gif")

        'cmdSave.Attributes.Add("onClick", "return resetSummary(0,1);")

        'rd1.Attributes.Add("onClick", "show();")

        If rd1.SelectedItem.Value <> "SP" Then
            rd2.Enabled = True
            'dr2.Style("display") = "none"
        End If

    End Sub
    'Private Sub PopulateVendor()
    '    Dim ds As DataSet
    '    Dim objAdmin As New Admin

    '    ds = objAdmin.searchapprvendorforBIM()
    '    cboPrefer.Items.Clear()
    '    cbo1st.Items.Clear()
    '    cbo2nd.Items.Clear()
    '    cbo3rd.Items.Clear()

    '    If ds.Tables(0).Rows.Count > 0 Then
    '        Common.FillDdl(cboPrefer, "CM_COY_NAME", "CM_COY_ID", ds)
    '        Common.FillDdl(cbo1st, "CM_COY_NAME", "CM_COY_ID", ds)
    '        Common.FillDdl(cbo2nd, "CM_COY_NAME", "CM_COY_ID", ds)
    '        Common.FillDdl(cbo3rd, "CM_COY_NAME", "CM_COY_ID", ds)
    '    End If
    '    'cbolist.Value = ""
    '    'cbolist.Text = "---Select---"
    '    'cboPrefer.Items.Insert(0, cbolist)
    '    'cbo1st.Items.Insert(0, cbolist)
    '    'cbo2nd.Items.Insert(0, cbolist)
    '    'cbo3rd.Items.Insert(0, cbolist)

    'End Sub

    Private Sub isNotPostBack()
        vldSumm.ShowSummary = False
        hidVendorItemCode.Value = ""
        hidItemName.Value = ""
        lblMsg.Text = ""
        ViewState("blnCmdSubmit") = True
        ViewState("blnCmdUploadDoc") = True
        ViewState("blnCmdUploadImg") = True
        'Session.Add("ImageIndex", "")
        ViewState("mode") = Request.QueryString("mode")
        objCat.FillGLCode(ddlAccCode)
        objCat.FillCatCode(ddlCatCode)
        objGlobal.FillCodeTable(cboUOM, CodeTable.Uom)
        objGlobal.FillTax(cboPreferTax)
        objGlobal.FillTax(cbo1stTax)
        objGlobal.FillTax(cbo2ndTax)
        objGlobal.FillTax(cbo3rdTax)

        If ViewState("GSTCOD") = True Then
            lblGST.Visible = False
            cboPreferTax.Visible = False
            cbo1stTax.Visible = False
            cbo2ndTax.Visible = False
            cbo3rdTax.Visible = False
        Else
            lblGST.Visible = True
            cboPreferTax.Visible = True
            cbo1stTax.Visible = True
            cbo2ndTax.Visible = True
            cbo3rdTax.Visible = True
        End If

        'PopulateVendor()
        If ViewState("mode") = "mod" Then
            If ViewState("itemid") Is Nothing Then ViewState("itemid") = Request.QueryString("itemid")
            displayData()
            cmdReset.Value = "Reset"
            cmdadd.Visible = False
        Else
            ViewState("itemid") = ""
            cmdReset.Value = "Clear"
            clearTextBox()
            cmdReset.Visible = False
        End If

        'Jules 2018.08.07
        If txtCommodityType.Text = "" AndAlso HttpContext.Current.Session("CompanyId").ToString.ToLower = "pamb" Then
            Dim objDB As New EAD.DBCom
            txtCommodityType.Text = objDB.GetVal("SELECT CT_NAME FROM COMMODITY_TYPE WHERE CT_ID = '52969'")
        End If
        'End modification.

        objCat.deleteTempAttachment(ViewState("itemid"), Session.SessionID)
    End Sub

    Private Sub clearTextBox()
        Me.txtVendorItemCode.Text = ""
        Me.txtItemName.Text = ""
        Me.txtRefNo.Text = ""
        Me.txtDesc.Text = ""
        Me.txtCommodityType.Text = ""
        'Me.txtAccCode.Text = ""
        Me.txtMinInv.Text = "0.00"
        Me.txtMin.Text = "0.00"
        Me.txtMax.Text = "0.00"
        Me.txtBrand.Text = ""
        Me.txtModel.Text = ""
        Me.txtDrawingNumber.Text = ""
        Me.txtVersionNo.Text = ""
        Me.txtGrossWeight.Text = ""
        Me.txtNetWeight.Text = ""
        Me.txtLength.Text = ""
        Me.txtWidth.Text = ""
        Me.txtHeight.Text = ""
        Me.txtVolume.Text = ""
        Me.txtColorInfo.Text = ""
        Me.txtHSCode.Text = ""
        Me.txtPacking.Text = ""
        Me.txtRemarks.Text = ""
        Me.txtPreferVendor.Text = ""
        Me.txtPreferVendor1st.Text = ""
        Me.txtPreferVendor2nd.Text = ""
        Me.txtPreferVendor3rd.Text = ""
        Me.hidPreferVendor.Text = ""
        Me.hidPreferVendor1st.Text = ""
        Me.hidPreferVendor2nd.Text = ""
        Me.hidPreferVendor3rd.Text = ""

        Me.txtManu.Text = ""
        Me.txtMaxInvQty.Text = "0.00"

        Me.txtLeadP.Text = ""
        Me.txtLead1.Text = ""
        Me.txtLead2.Text = ""
        Me.txtLead3.Text = ""

        Me.txtVenCodeP.Text = ""
        Me.txtVenCode1.Text = ""
        Me.txtVenCode2.Text = ""
        Me.txtVenCode3.Text = ""

        Me.rd1.SelectedValue = "SP"
        Me.rd2.SelectedValue = "N"

        Me.rd2.Enabled = False


    End Sub

    Private Function displayData()
        Dim ds As New DataSet
        Dim objAdmin As New Admin

        ds = objCat.getSingleProduct(ViewState("itemid"), False)
        If ds.Tables(0).Rows.Count > 0 Then
            ViewState("strItemId") = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_CODE"))
            txtVendorItemCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_VENDOR_ITEM_CODE"))
            hidVendorItemCode.Value = txtVendorItemCode.Text
            txtItemName.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_DESC"))
            hidItemName.Value = txtItemName.Text
            txtRefNo.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_REF_NO"))
            txtDesc.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_LONG_DESC"))
            txtCommodityType.Text = Common.parseNull(ds.Tables(0).Rows(0)("CT_NAME"))
            hidCommodityType.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_CATEGORY_NAME"))

            Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("PM_UOM")), cboUOM, False, True)
            If Not IsDBNull(ds.Tables(0).Rows(0)("PM_PREFER_S_COY_ID_TAX_ID")) AndAlso (CStr(Common.parseNull(ds.Tables(0).Rows(0)("PM_PREFER_S_COY_ID_TAX_ID"))) <> "") Then Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("PM_PREFER_S_COY_ID_TAX_ID")), cboPreferTax, True, True)
            If Not IsDBNull(ds.Tables(0).Rows(0)("PM_1ST_S_COY_ID_TAX_ID")) AndAlso (CStr(Common.parseNull(ds.Tables(0).Rows(0)("PM_1ST_S_COY_ID_TAX_ID"))) <> "") Then Common.SelDdl(Common.parseNull(CStr(ds.Tables(0).Rows(0)("PM_1ST_S_COY_ID_TAX_ID"))), cbo1stTax, True, True)
            If Not IsDBNull(ds.Tables(0).Rows(0)("PM_2ND_S_COY_ID_TAX_ID")) AndAlso (CStr(Common.parseNull(ds.Tables(0).Rows(0)("PM_2ND_S_COY_ID_TAX_ID"))) <> "") Then Common.SelDdl(Common.parseNull(CStr(ds.Tables(0).Rows(0)("PM_2ND_S_COY_ID_TAX_ID"))), cbo2ndTax, True, True)
            If Not IsDBNull(ds.Tables(0).Rows(0)("PM_3RD_S_COY_ID_TAX_ID")) AndAlso (CStr(Common.parseNull(ds.Tables(0).Rows(0)("PM_3RD_S_COY_ID_TAX_ID"))) <> "") Then Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("PM_3RD_S_COY_ID_TAX_ID")), cbo3rdTax, True, True)
            'If Common.parseNull(ds.Tables(0).Rows(0)("PM_PREFER_S_COY_ID")) <> "" Then Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("PM_PREFER_S_COY_ID")), cboPrefer, True, False)
            hidPreferVendor.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_PREFER_S_COY_ID"))
            txtPreferVendor.Text = Common.parseNull(ds.Tables(0).Rows(0)("S_COY_NAME_P"))
            ' If Common.parseNull(ds.Tables(0).Rows(0)("PM_1ST_S_COY_ID")) <> "" Then Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("PM_1ST_S_COY_ID")), cbo1st, True, False)
            hidPreferVendor1st.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_1ST_S_COY_ID"))
            txtPreferVendor1st.Text = Common.parseNull(ds.Tables(0).Rows(0)("S_COY_NAME_1"))
            'If Common.parseNull(ds.Tables(0).Rows(0)("PM_2ND_S_COY_ID")) <> "" Then Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("PM_2ND_S_COY_ID")), cbo2nd, True, False)
            hidPreferVendor2nd.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_2ND_S_COY_ID"))
            txtPreferVendor2nd.Text = Common.parseNull(ds.Tables(0).Rows(0)("S_COY_NAME_2"))
            ' If Common.parseNull(ds.Tables(0).Rows(0)("PM_3RD_S_COY_ID")) <> "" Then Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("PM_3RD_S_COY_ID")), cbo3rd, True, False)
            hidPreferVendor3rd.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_3RD_S_COY_ID"))
            txtPreferVendor3rd.Text = Common.parseNull(ds.Tables(0).Rows(0)("S_COY_NAME_3"))

            txtBrand.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_BRAND"))
            txtModel.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_MODEL"))
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
            If Common.parseNull(ds.Tables(0).Rows(0)("PM_DELETED")) = "N" Then
                chkStatus.Checked = True
            Else
                chkStatus.Checked = False
            End If

            'txtAccCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_ACCT_CODE"))
            Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("PM_ACCT_CODE")), ddlAccCode, False, True)
            Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("PM_CAT_CODE")), ddlCatCode, False, True)
            txtMinInv.Text = Format(CDbl(ds.Tables(0).Rows(0)("PM_SAFE_QTY")), "#####0.00")
            txtMin.Text = Format(CDbl(ds.Tables(0).Rows(0)("PM_ORD_MIN_QTY")), "#####0.00")
            txtMax.Text = Format(CDbl(ds.Tables(0).Rows(0)("PM_ORD_MAX_QTY")), "#####0.00")
            'If Common.parseNull(ds.Tables(0).Rows(0)("PM_SAFE_QTY")) = "0" Then
            '    txtMinInv.Text = ""
            'Else
            '    txtMinInv.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_SAFE_QTY"))
            'End If
            'If Common.parseNull(ds.Tables(0).Rows(0)("PM_ORD_MIN_QTY")) = "0" Then
            '    txtMin.Text = ""
            'Else
            '    txtMin.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_ORD_MIN_QTY"))
            'End If
            'If Common.parseNull(ds.Tables(0).Rows(0)("PM_ORD_MAX_QTY")) = "0" Then
            '    txtMax.Text = ""
            'Else
            '    txtMax.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_ORD_MAX_QTY"))
            'End If


            ' display attached files
            ViewState("ImageIndex") = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_IMAGE"))
            ViewState("blnTemp") = False
            displayAttachFile()
            displayImageFile(ViewState("ImageIndex"))

            txtManu.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_MANUFACTURER"))

            txtMaxInvQty.Text = Format(CDbl(ds.Tables(0).Rows(0)("PM_MAX_INV_QTY")), "#####0.00")
            'If Common.parseNull(ds.Tables(0).Rows(0)("PM_MAX_INV_QTY")) = "0" Then
            '    txtMaxInvQty.Text = ""
            'Else
            '    txtMaxInvQty.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_MAX_INV_QTY"))
            'End If

            rd1.SelectedValue = Common.parseNull(ds.Tables(0).Rows(0)("PM_ITEM_TYPE"))
            rd2.SelectedValue = Common.parseNull(ds.Tables(0).Rows(0)("PM_IQC_IND"))

            If rd1.SelectedValue = "SP" Then
                rd2.Enabled = False
            Else
                rd2.Enabled = True
            End If

            'PopulateVendor()
        End If

        ds = objCat.getSingleProductVenLead(ViewState("itemid"), False)
        If ds.Tables(0).Rows.Count > 0 Then
            Dim count As Integer
            For count = 0 To ds.Tables(0).Rows.Count - 1
                Dim vType As String
                vType = Common.parseNull(ds.Tables(0).Rows(count).Item("PV_VENDOR_TYPE"))

                If vType = "P" Then
                    txtLeadP.Text = Common.parseNull(ds.Tables(0).Rows(count).Item("PV_LEAD_TIME"))
                    txtVenCodeP.Text = Common.parseNull(ds.Tables(0).Rows(count).Item("PV_VENDOR_CODE"))
                ElseIf vType = "1" Then
                    txtLead1.Text = Common.parseNull(ds.Tables(0).Rows(count).Item("PV_LEAD_TIME"))
                    txtVenCode1.Text = Common.parseNull(ds.Tables(0).Rows(count).Item("PV_VENDOR_CODE"))
                ElseIf vType = "2" Then
                    txtLead2.Text = Common.parseNull(ds.Tables(0).Rows(count).Item("PV_LEAD_TIME"))
                    txtVenCode2.Text = Common.parseNull(ds.Tables(0).Rows(count).Item("PV_VENDOR_CODE"))
                ElseIf vType = "3" Then
                    txtLead3.Text = Common.parseNull(ds.Tables(0).Rows(count).Item("PV_LEAD_TIME"))
                    txtVenCode3.Text = Common.parseNull(ds.Tables(0).Rows(count).Item("PV_VENDOR_CODE"))
                End If
            Next
        End If
    End Function

    Private Sub displayImageFile(ByVal strIndex As String)
        Dim dsImage As New DataSet
        Dim drvImage As New DataView
        Dim blnTemp As Boolean, sImgFullPath As String
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp, strCoyId, strBasePath, strDownloadPath, strFilePath, strDisplayPath As String
        Dim objFile As New FileManagement

        'If viewstate("mode") = "mod" Then
        '    blnTemp = False
        'Else
        '    blnTemp = True
        'End If
        'dsImage = objCat.getTempImageAttach(strIndex, viewstate("blnTemp"))  ' True)
        dsImage = objCat.getTempImageAttachBuyer(strIndex)

        pnlImage.Controls.Clear()
        drvImage = dsImage.Tables(0).DefaultView
        If drvImage.Count > 0 Then
            For i = 0 To drvImage.Count - 1
                strFile = drvImage(i)("PA_ATTACH_FILENAME")
                strFile1 = drvImage(i)("PA_HUB_FILENAME")

                'strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.ProductImage & ">" & strFile & "</A>"
                'Dim objFile As New Wheel.Components.FileManagement
                '//To get fullpath file name
                sImgFullPath = objFile.FileDownload(Server.UrlEncode(strFile1), EnumDownLoadType.ProductImage, "", EnumUploadFrom.FrontOff)

                If objFile.FileExist(strFile1) Then
                    hidImageFile.Value = sImgFullPath
                    strURL = "<A HREF='#' onclick='PreviewImage(1);'>" & strFile & "</A>"
                    Dim lblBr As New Label
                    Dim lblFile As New Label
                    Dim lnk As New ImageButton
                    lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL '& " (" & drvImage(i)("PA_FILESIZE") & "KB) "
                    'lblBr.Text = "<BR>"
                    lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete2.gif")

                    lnk.ID = drvImage(i)("PA_ATTACH_INDEX")
                    lnk.CausesValidation = False
                    AddHandler lnk.Click, AddressOf deleteImageAttach

                    pnlImage.Controls.Add(lblFile)
                    pnlImage.Controls.Add(lnk)
                    pnlImage.Controls.Add(lblBr)
                End If


            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlImage.Controls.Add(lblFile)
        End If
    End Sub

    Private Sub displayAttachFile(Optional ByVal isbtnadd As Boolean = False)
        If isbtnadd = True Then
            pnlAttach.Controls.Clear()
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFile)
            Exit Sub
        End If
        Dim dsAttach As New DataSet
        Dim drvAttach As New DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String
        If ViewState("mode") = "add" Then
            If ViewState("itemid") = "" Then
                dsAttach = objCat.getTempAttachBuyer(Session.SessionID)
            Else
                dsAttach = objCat.getTempAttachBuyer(ViewState("itemid"))
            End If
        Else
            '    'If Not IsPostBack Then
            '    '    dsAttach = objCat.getTempAttach(ViewState("itemid"), False)
            '    'Else
            '    '    dsAttach = objCat.getTempAttach(ViewState("itemid"), True)
            '    'End If
            dsAttach = objCat.getTempAttachBuyer(ViewState("itemid"))
        End If

        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                Dim strFilePath As String

                strFile = drvAttach(i)("PA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("PA_HUB_FILENAME")

                'If Common.isImageFile(strFile) Then
                '    strFilePath = objFile.FileDownload(Server.UrlEncode(strFile1), EnumDownLoadType.ProductAttach, "", Components.EnumUploadFrom.FrontOff)
                '    strFilePath = strFilePath.Replace("\", "\\")
                '    strURL = "<A HREF='#' onclick='PreviewAttach(""" & Server.HtmlEncode(strFilePath) & """);'>" & strFile & "</A>"
                'Else
                '    strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.ProductAttach & ">" & strFile & "</A>"
                'End If
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.ProductAttach, "", EnumUploadFrom.FrontOff)

                Dim lblBr As New Label
                Dim lblFile As New Label
                Dim lnk As New ImageButton
                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("PA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"
                lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete2.gif")
                lnk.ID = drvAttach(i)("PA_ATTACH_INDEX")
                lnk.CausesValidation = False
                AddHandler lnk.Click, AddressOf deleteAttach

                pnlAttach.Controls.Add(lblFile)
                pnlAttach.Controls.Add(lnk)
                pnlAttach.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFile)
        End If
    End Sub

    Private Sub deleteImageAttach(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strDocNo As String
        If ViewState("mode") = "mod" Then
            strDocNo = ViewState("itemid")
        Else
            strDocNo = Session.SessionID
        End If
        'objCat.deleteAttachment(CType(sender, ImageButton).ID, False)
        'objCat.deleteAttachment(CType(sender, ImageButton).ID, False)
        objCat.deleteAttachmentBuyer(CType(sender, ImageButton).ID, strDocNo, "I")
        displayImageFile(ViewState("ImageIndex"))
    End Sub

    Private Sub deleteAttach(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strDocNo As String
        If ViewState("mode") = "mod" Then
            strDocNo = ViewState("itemid")
        Else
            strDocNo = Session.SessionID
        End If

        objCat.deleteAttachmentBuyer(CType(sender, ImageButton).ID, strDocNo, "D")
        displayAttachFile()
    End Sub

    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        If FileDoc.Value <> "" Then
            Dim objFile As New FileManagement
            Dim strDocNo As String
            If ViewState("mode") = "mod" Then
                strDocNo = ViewState("itemid")
            Else
                strDocNo = Session.SessionID
            End If

            Dim objDB As New EAD.DBCom
            'Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))
            Dim dblMaxFileSize As Double = Session("FileSize")

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(FileProImage.PostedFile.FileName)

            If Len(sFileName) > 50 Then
                Common.NetMsgbox(Me, "File name exceeds 46 character")
            ElseIf FileDoc.PostedFile.ContentLength > 0 And FileDoc.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                'objFile.FileUpload(File1, EnumUploadType.ProductAttachment, "", EnumUploadFrom.FrontOff, strDocNo, True)
                objFile.FileUpload(FileDoc, EnumUploadType.ProductAttachment, "", EnumUploadFrom.FrontOff, strDocNo, True, , , , "B")
            ElseIf FileDoc.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            displayAttachFile()
            displayImageFile(ViewState("ImageIndex"))
            objFile = Nothing
            objDB = Nothing
        End If
    End Sub

    Private Sub cmdUploadImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUploadImage.Click
        If FileProImage.Value <> "" Then
            Dim blnTemp As Boolean
            Dim objFile As New FileManagement
            Dim strDocNo As String
            If ViewState("mode") = "mod" Then
                strDocNo = ViewState("itemid")
                ' ai chu add on 19/09/2005
                ' for item to be modified, when user upload new product image, 
                ' image file is uploaded into PRODUCT_ATTACHMENT table, 
                ' not PRODUCT_ATTACHMENT_TEMP
                blnTemp = False
            Else
                strDocNo = Session.SessionID
                blnTemp = True
            End If

            Dim objDB As New EAD.DBCom
            'Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'ImageAttachSize'"))
            Dim dblMaxFileSize As Double = Session("FileSize")

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(FileProImage.PostedFile.FileName)

            If Len(sFileName) > 50 Then
                Common.NetMsgbox(Me, "File name exceeds 46 character")
            ElseIf FileProImage.PostedFile.ContentLength > 0 And FileProImage.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                'ViewState("ImageIndex") = objFile.FileUpload(FileProImage, EnumUploadType.ProductImage, "", EnumUploadFrom.FrontOff, strDocNo, True, ViewState("ImageIndex"))
                ViewState("ImageIndex") = objFile.FileUpload(FileProImage, EnumUploadType.ProductImage, "", EnumUploadFrom.FrontOff, strDocNo, True, ViewState("ImageIndex"), , , "B")
            ElseIf FileProImage.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            ViewState("blnTemp") = True
            'ViewState("ImageIndex") = objFile.FileUpload(FileProImage, EnumUploadType.ProductImage, "", EnumUploadFrom.FrontOff, strDocNo, blnTemp, ViewState("ImageIndex"))
            displayImageFile(ViewState("ImageIndex"))
            displayAttachFile()
            objFile = Nothing
            objDB = Nothing
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

    'Private Function validateDatagrid(ByRef strMsg As String) As Boolean
    '    validateDatagrid = True
    '    Dim dgItem As DataGridItem
    '    Dim chk As CheckBox
    '    Dim txtRemark As TextBox
    '    Dim txtQ As TextBox
    '    Dim txtQ2 As TextBox
    '    strMsg = "<ul type='disc'>"
    '    For Each dgItem In dtgCatalogue.Items
    '        chk = dgItem.FindControl("chkSelection")
    '        txtRemark = dgItem.FindControl("txtRemark")
    '        txtQ = dgItem.FindControl("txtQ")
    '        txtQ2 = dgItem.FindControl("txtQ2")
    '        If chk.Checked Then
    '            If Not isValidPrice(CType(dgItem.FindControl("txtPrice"), TextBox).Text) Then
    '                'If CDbl(CType(dgItem.FindControl("txtPrice"), TextBox).Text) <= 0 Then
    '                strMsg &= "<li>" & dgItem.Cells(EnumCat.icCode).Text & " - Discount Price is over limit/expecting numeric value.<ul type='disc'></ul></li>"
    '                txtQ2.Text = "?"
    '                validateDatagrid = False
    '                'End If
    '                'Else
    '                '   strMsg = "<ul type='disc'><li>" & dgItem.Cells(EnumCat.icCode).Text & " is over limit/expecting numeric value.<ul type='disc'></ul></li>"
    '                '  validateDatagrid = False
    '            Else
    '                txtQ2.Text = ""
    '            End If

    '            If Not Common.checkMaxLength(txtRemark.Text, 400) Then
    '                strMsg &= "<li>" & dgItem.Cells(EnumCat.icCode).Text & " - Discount Remarks is over limit.<ul type='disc'></ul></li>"
    '                txtQ.Text = "?"
    '                validateDatagrid = False
    '            Else
    '                txtQ.Text = ""
    '            End If
    '        End If
    '    Next
    '    strMsg &= "</ul>"
    'End Function

    Private Function insertRow(ByRef dtProduct As DataTable, ByVal strField As String, ByVal strValue As String)
        Dim dtr As DataRow
        dtr = dtProduct.NewRow()
        dtr("ProductIndex") = ViewState("itemindex")
        dtr("ProductCode") = ViewState("strItemId") 'lblItemId.Text 'PM_PRODUCT_CODE
        dtr("FieldName") = strField
        dtr("Value") = strValue
        dtProduct.Rows.Add(dtr)
    End Function

    'Private Function getChangesUpdate(ByRef bln As Boolean) As DataTable
    '    bln = False
    '    Dim dtProduct As New DataTable
    '    dtProduct.Columns.Add("ProductIndex", Type.GetType("System.String"))
    '    dtProduct.Columns.Add("ProductCode", Type.GetType("System.String"))
    '    dtProduct.Columns.Add("FieldName", Type.GetType("System.String"))
    '    dtProduct.Columns.Add("Value", Type.GetType("System.String"))

    '    insertRow(dtProduct, "PM_VENDOR_ITEM_CODE", txtVendorItemCode.Text)
    '    insertRow(dtProduct, "PM_PRODUCT_DESC", txtItemName.Text)
    '    insertRow(dtProduct, "PM_REF_NO", txtRefNo.Text)
    '    insertRow(dtProduct, "PM_LONG_DESC", txtDesc.Text)
    '    insertRow(dtProduct, "PM_CATEGORY_NAME", txtCommodityType.Text)
    '    insertRow(dtProduct, "PM_ACCT_CODE", txtAccCode.Text)
    '    insertRow(dtProduct, "PM_SAFE_QTY", txtMinInv.Text)
    '    insertRow(dtProduct, "PM_ORD_MIN_QTY", txtMin.Text)
    '    insertRow(dtProduct, "PM_ORD_MAX_QTY", txtMax.Text)
    '    insertRow(dtProduct, "PM_UOM", cboUOM.SelectedItem.Text)
    '    insertRow(dtProduct, "PM_PRODUCT_BRAND", txtBrand.Text)
    '    insertRow(dtProduct, "PM_PRODUCT_MODEL", txtModel.Text)
    '    insertRow(dtProduct, "PM_DRAW_NO", txtDrawingNumber.Text)
    '    insertRow(dtProduct, "PM_VERS_NO", txtVersionNo.Text)
    '    insertRow(dtProduct, "PM_GROSS_WEIGHT", txtGrossWeight.Text)
    '    insertRow(dtProduct, "PM_NET_WEIGHT", txtNetWeight.Text)
    '    insertRow(dtProduct, "PM_LENGHT", txtLength.Text)
    '    insertRow(dtProduct, "PM_WIDTH", txtWidth.Text)
    '    insertRow(dtProduct, "PM_HEIGHT", txtHeight.Text)
    '    insertRow(dtProduct, "PM_VOLUME", txtVolume.Text)
    '    insertRow(dtProduct, "PM_COLOR_INFO", txtColorInfo.Text)
    '    insertRow(dtProduct, "PM_HSC_CODE", txtHSCode.Text)
    '    insertRow(dtProduct, "PM_PACKING_REQ", txtPacking.Text)
    '    insertRow(dtProduct, "PM_REMARKS", txtRemarks.Text)
    '    If cboPrefer.SelectedIndex > 0 Then
    '        insertRow(dtProduct, "PREFER", cboPrefer.SelectedValue)
    '    Else
    '        insertRow(dtProduct, "PREFER", "")
    '    End If
    '    insertRow(dtProduct, "PREFER", cboPrefer.SelectedValue)
    '    insertRow(dtProduct, "PM_REMARKS", txtRemarks.Text)
    '    insertRow(dtProduct, "PM_REMARKS", txtRemarks.Text)
    '    insertRow(dtProduct, "PM_REMARKS", txtRemarks.Text)

    '    getChangesUpdate = dtProduct
    'End Function
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If txtPreferVendor.Text = "" Then
            hidPreferVendor.Text = ""
        End If
        If txtPreferVendor1st.Text = "" Then
            hidPreferVendor1st.Text = ""
        End If
        If txtPreferVendor2nd.Text = "" Then
            hidPreferVendor2nd.Text = ""
        End If
        If txtPreferVendor3rd.Text = "" Then
            hidPreferVendor3rd.Text = ""
        End If
        vldSumm.ShowSummary = True
        Dim strMsg, strItemIndex As String
        Dim objFile As New FileManagement
        Dim blnSend As Boolean
        Dim strNewCode, strNewImage As String
        Dim dsProduct As New DataSet
        Dim dtProduct As New DataTable
        Dim dsNewImage As New DataSet
        Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))

        blnList = False
        blnDiscount = False

        ' If Page.IsValid And validateDatagrid(strMsg) Then
        If Page.IsValid Then
            lblMsg.Text = ""
            dsProduct = bindProduct()
            'If ViewState("mode") = "add" Then
            Dim POFound, PRFound, RFQFound, INVENTORYFound As Boolean
            Dim getINVType, getINVQC As String

            getINVType = objDb.Get1Column("PRODUCT_MSTR", "PM_ITEM_TYPE", " WHERE PM_VENDOR_ITEM_CODE = '" & Replace(dsProduct.Tables(0).Rows(0)("VendorItemCode"), "'", "''") & "' AND PM_S_COY_ID = '" & Session("CompanyId") & "'")
            'Michelle (14/5/2011) - Add in the checking of Item nName
            'If txtVendorItemCode.Text <> hidVendorItemCode.Value Then
            If txtVendorItemCode.Text <> hidVendorItemCode.Value Then ' Or txtItemName.Text <> hidItemName.Value Then   'Modified by Joon on 31st May 2011
                If hidVendorItemCode.Value <> "" Then
                    POFound = (objDb.Get1Column("PO_DETAILS", "POD_PRODUCT_CODE", " WHERE (POD_VENDOR_ITEM_CODE = '" & Replace(hidVendorItemCode.Value, "'", "''") & "' AND POD_PRODUCT_CODE <> '&nbsp;')" & " AND POD_COY_ID = '" & Session("CompanyId") & "'") <> "")
                    PRFound = (objDb.Get1Column("PR_DETAILS", "PRD_PRODUCT_CODE", " WHERE (PRD_VENDOR_ITEM_CODE = '" & Replace(hidVendorItemCode.Value, "'", "''") & "' AND PRD_PRODUCT_CODE <> '&nbsp;')" & " AND PRD_COY_ID = '" & Session("CompanyId") & "'") <> "")
                    RFQFound = (objDb.Get1Column("RFQ_DETAIL", "RD_PRODUCT_CODE", " WHERE (RD_VENDOR_ITEM_CODE = '" & Replace(hidVendorItemCode.Value, "'", "''") & "' AND RD_PRODUCT_CODE <> '&nbsp;')" & " AND RD_COY_ID = '" & Session("CompanyId") & "'") <> "")
                Else
                    POFound = False
                    PRFound = False
                    RFQFound = False
                End If
                'POFound = (objDb.Get1Column("PO_DETAILS", "POD_PRODUCT_CODE", " WHERE (POD_VENDOR_ITEM_CODE = '" & Replace(hidVendorItemCode.Value, "'", "''") & "' AND POD_PRODUCT_CODE <> '&nbsp;')" & " AND POD_COY_ID = '" & Session("CompanyId") & "'") <> "")
                'PRFound = (objDb.Get1Column("PR_DETAILS", "PRD_PRODUCT_CODE", " WHERE (PRD_VENDOR_ITEM_CODE = '" & Replace(hidVendorItemCode.Value, "'", "''") & "' AND PRD_PRODUCT_CODE <> '&nbsp;')" & " AND PRD_COY_ID = '" & Session("CompanyId") & "'") <> "")
                'RFQFound = (objDb.Get1Column("RFQ_DETAIL", "RD_PRODUCT_CODE", " WHERE (RD_VENDOR_ITEM_CODE = '" & Replace(hidVendorItemCode.Value, "'", "''") & "' AND RD_PRODUCT_CODE <> '&nbsp;')" & " AND RD_COY_ID = '" & Session("CompanyId") & "'") <> "")
            ElseIf getINVType <> rd1.SelectedValue Then
                POFound = (objDb.Get1Column("PO_DETAILS", "POD_PRODUCT_CODE", " WHERE (POD_VENDOR_ITEM_CODE = '" & Replace(dsProduct.Tables(0).Rows(0)("VendorItemCode"), "'", "''") & "' AND POD_PRODUCT_CODE <> '&nbsp;')" & " AND POD_COY_ID = '" & Session("CompanyId") & "'") <> "")
                PRFound = (objDb.Get1Column("PR_DETAILS", "PRD_PRODUCT_CODE", " WHERE (PRD_VENDOR_ITEM_CODE = '" & Replace(dsProduct.Tables(0).Rows(0)("VendorItemCode"), "'", "''") & "' AND PRD_PRODUCT_CODE <> '&nbsp;')" & " AND PRD_COY_ID = '" & Session("CompanyId") & "'") <> "")
                RFQFound = (objDb.Get1Column("RFQ_DETAIL", "RD_PRODUCT_CODE", " WHERE (RD_VENDOR_ITEM_CODE = '" & Replace(dsProduct.Tables(0).Rows(0)("VendorItemCode"), "'", "''") & "' AND RD_PRODUCT_CODE <> '&nbsp;')" & " AND RD_COY_ID = '" & Session("CompanyId") & "'") <> "")
            Else
                getINVQC = objDb.Get1Column("PRODUCT_MSTR", "PM_IQC_IND", " WHERE PM_VENDOR_ITEM_CODE = '" & Replace(dsProduct.Tables(0).Rows(0)("VendorItemCode"), "'", "''") & "' AND PM_S_COY_ID = '" & Session("CompanyId") & "'")
                If getINVQC <> rd2.SelectedValue Then
                    POFound = (objDb.Get1Column("PO_DETAILS", "POD_PRODUCT_CODE", " WHERE (POD_VENDOR_ITEM_CODE = '" & Replace(dsProduct.Tables(0).Rows(0)("VendorItemCode"), "'", "''") & "' AND POD_PRODUCT_CODE <> '&nbsp;')" & " AND POD_COY_ID = '" & Session("CompanyId") & "'") <> "")
                    PRFound = (objDb.Get1Column("PR_DETAILS", "PRD_PRODUCT_CODE", " WHERE (PRD_VENDOR_ITEM_CODE = '" & Replace(dsProduct.Tables(0).Rows(0)("VendorItemCode"), "'", "''") & "' AND PRD_PRODUCT_CODE <> '&nbsp;')" & " AND PRD_COY_ID = '" & Session("CompanyId") & "'") <> "")
                    RFQFound = (objDb.Get1Column("RFQ_DETAIL", "RD_PRODUCT_CODE", " WHERE (RD_VENDOR_ITEM_CODE = '" & Replace(dsProduct.Tables(0).Rows(0)("VendorItemCode"), "'", "''") & "' AND RD_PRODUCT_CODE <> '&nbsp;')" & " AND RD_COY_ID = '" & Session("CompanyId") & "'") <> "")
                Else
                    POFound = False
                    PRFound = False
                    RFQFound = False
                End If
            End If

            INVENTORYFound = False
            If rd1.SelectedValue = "SP" Then
                strItemIndex = objDb.GetVal("SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR WHERE IM_ITEM_CODE = '" & Common.Parse(hidVendorItemCode.Value) & "' AND IM_COY_ID = '" & Session("CompanyId") & "'")

                If strItemIndex <> "" Then
                    If objDb.Exist("SELECT '*' FROM INVENTORY_DETAIL WHERE ID_INVENTORY_INDEX = '" & strItemIndex & "'") > 0 Then
                        INVENTORYFound = True
                    End If
                End If
            End If

            If (objDb.Get1Column("product_mstr", "PM_PRODUCT_INDEX", " WHERE PM_VENDOR_ITEM_CODE = '" & Replace(dsProduct.Tables(0).Rows(0)("VendorItemCode"), "'", "''") & "' AND PM_S_COY_ID = '" & Session("CompanyId") & "'") = "") Or hidVendorItemCode.Value = dsProduct.Tables(0).Rows(0)("VendorItemCode") Then
                If POFound <> True And PRFound <> True And RFQFound <> True Then
                    If INVENTORYFound = False Then
                        '(objDb.Get1Column("PO_DETAILS", "POD_PRODUCT_CODE", " WHERE POD_VENDOR_ITEM_CODE = '" & Replace(dsProduct.Tables(0).Rows(0)("VendorItemCode"), "'", "''") & "' AND POD_COY_ID = '" & Session("CompanyId") & "'") = "") Then
                        'Or (objDb.Get1Column("PR_DETAILS", "PRD_PRODUCT_CODE", " WHERE PRD_VENDOR_ITEM_CODE = '" & Replace(dsProduct.Tables(0).Rows(0)("VendorItemCode"), "'", "''") & "' AND PRD_COY_ID = '" & Session("CompanyId") & "'") = "") 
                        'Or (objDb.Get1Column("RFQ_DETAIL", "RD_PRODUCT_CODE", " WHERE RD_VENDOR_ITEM_CODE = '" & Replace(dsProduct.Tables(0).Rows(0)("VendorItemCode"), "'", "''") & "' AND RD_COY_ID = '" & Session("CompanyId") & "'") <> "") Then


                        If objCat.BIM(dsProduct, ViewState("mode"), ViewState("ImageIndex"), ViewState("strItemId"), hidVendorItemCode.Value, hidVendorItemCode.Value, hidItemName.Value) Then
                            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
                            Me.cmdSave.Enabled = False
                            'ViewState("ImageIndex") = ""
                            'ViewState("ImageIndex") = objDb.Get1Column("PRODUCT_ATTACHMENT", "PA_ATTACH_INDEX ", " WHERE PA_PRODUCT_CODE ='" & dsProduct.Tables(0).Rows(0)("ProductCode") & "' AND PA_TYPE = 'I' ")
                            ViewState("ImageIndex") = objDb.Get1Column("PRODUCT_ATTACHMENT", "PA_ATTACH_INDEX ", " WHERE PA_PRODUCT_CODE ='" & ViewState("strItemId") & "' AND PA_TYPE = 'I' ")

                            displayImageFile(ViewState("ImageIndex"))

                            'dsNewImage = objDb.FillDs("SELECT * FROM PRODUCT_MSTR WHERE PM_VENDOR_ITEM_CODE = '" & dsProduct.Tables(0).Rows(0)("VendorItemCode") & "' AND PM_S_COY_ID = '" & Session("CompanyId") & "'")
                            'strNewImage = dsNewImage.Tables(0).Rows(0)("PM_PRODUCT_IMAGE")
                            'If strNewImage <> "" Then ViewState("ImageIndex") = strNewImage
                            hidVendorItemCode.Value = dsProduct.Tables(0).Rows(0)("VendorItemCode")
                            hidItemName.Value = dsProduct.Tables(0).Rows(0)("ItemName")

                            ViewState("mode") = "mod"
                            ViewState("itemid") = ViewState("strItemId")
                            txtVendorItemCode.Enabled = False
                            displayAttachFile()

                            If txtMinInv.Text = "" Then
                                txtMinInv.Text = Format(0, "#####0.00")
                            Else
                                txtMinInv.Text = Format(CDbl(txtMinInv.Text), "#####0.00")
                            End If

                            If txtMin.Text = "" Then
                                txtMin.Text = Format(0, "#####0.00")
                            Else
                                txtMin.Text = Format(CDbl(txtMin.Text), "#####0.00")
                            End If

                            If txtMax.Text = "" Then
                                txtMax.Text = Format(0, "#####0.00")
                            Else
                                txtMax.Text = Format(CDbl(txtMax.Text), "#####0.00")
                            End If

                            If Me.txtMaxInvQty.Text = "" Then
                                Me.txtMaxInvQty.Text = Format(0, "#####0.00")
                            Else
                                Me.txtMaxInvQty.Text = Format(CDbl(txtMaxInvQty.Text), "#####0.00")
                            End If

                        Else
                            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00007"), MsgBoxStyle.Information)
                        End If
                    Else
                        Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00363"), MsgBoxStyle.Information)
                    End If
                    
                Else
                    Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00018"), MsgBoxStyle.Information)
                End If
            Else
                Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00002"), MsgBoxStyle.Information)
            End If

            'objCat.updateListPrice(Session("CompanyId"), dsProduct, blnSend, "", ViewState("ImageIndex"), blnList, False)
            'If blnSend Then
            '    'Response.Redirect("CatalogueConfirm.aspx?pageid=" & strPageId & "&role=L&act=0&code=" & Server.UrlEncode(lblItemId.Text))
            '    Response.Redirect("CatalogueConfirm.aspx?pageid=" & strPageId & "&role=L&act=0&code=" & Server.UrlEncode(strItemId))
            'Else
            '    Common.NetMsgbox(Me, MsgRecordSave, "ListPriceCatalogue.aspx?pageid=" & strPageId, MsgBoxStyle.Information)
            'End If
            'End If
        Else
            If strMsg <> "" Then
                lblMsg.Text = strMsg
            Else
                lblMsg.Text = ""
            End If
        End If
        objFile = Nothing
    End Sub

    Private Function bindProduct() As DataSet
        Dim ds As New DataSet
        Dim dtProduct As New DataTable
        dtProduct.Columns.Add("CoyId", Type.GetType("System.String"))   'PM_S_COY_ID
        dtProduct.Columns.Add("ProductCode", Type.GetType("System.String")) 'PM_PRODUCT_CODE
        dtProduct.Columns.Add("VendorItemCode", Type.GetType("System.String"))  'PM_VENDOR_ITEM_CODE
        dtProduct.Columns.Add("ItemName", Type.GetType("System.String"))    'PM_PRODUCT_DESC ' dtProduct.Columns.Add("ProductDesc", Type.GetType("System.String"))
        dtProduct.Columns.Add("ReferenceNo", Type.GetType("System.String")) 'PM_REF_NO
        dtProduct.Columns.Add("Description", Type.GetType("System.String")) 'PM_LONG_DESC
        dtProduct.Columns.Add("CommodityType", Type.GetType("System.String")) 'PM_CATEGORY_NAME 'dtProduct.Columns.Add("CategoryName", Type.GetType("System.String"))
        dtProduct.Columns.Add("AccCode", Type.GetType("System.String")) 'PM_ACCT_CODE
        dtProduct.Columns.Add("UOM", Type.GetType("System.String")) 'PM_UOM
        dtProduct.Columns.Add("CatCode", Type.GetType("System.String"))
        dtProduct.Columns.Add("MinInv", Type.GetType("System.String")) 'PM_SAFE_QTY
        dtProduct.Columns.Add("Min", Type.GetType("System.String")) 'PM_ORD_MIN_QTY
        dtProduct.Columns.Add("Max", Type.GetType("System.String")) 'PM_ORD_MAX_QTY
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
        dtProduct.Columns.Add("Remark", Type.GetType("System.String")) 'PM_REMARKS
        dtProduct.Columns.Add("Deleted", Type.GetType("System.String")) 'PM_DELETED
        dtProduct.Columns.Add("Prefer", Type.GetType("System.String"))
        dtProduct.Columns.Add("1st", Type.GetType("System.String"))
        dtProduct.Columns.Add("2nd", Type.GetType("System.String"))
        dtProduct.Columns.Add("3rd", Type.GetType("System.String"))
        dtProduct.Columns.Add("PreferTax", Type.GetType("System.String"))
        dtProduct.Columns.Add("1stTax", Type.GetType("System.String"))
        dtProduct.Columns.Add("2ndTax", Type.GetType("System.String"))
        dtProduct.Columns.Add("3rdTax", Type.GetType("System.String"))

        dtProduct.Columns.Add("MaxInvQty", Type.GetType("System.String"))
        dtProduct.Columns.Add("Manu", Type.GetType("System.String"))

        dtProduct.Columns.Add("rd1", Type.GetType("System.String"))
        dtProduct.Columns.Add("rd2", Type.GetType("System.String"))

        dtProduct.Columns.Add("LeadP", Type.GetType("System.String"))
        dtProduct.Columns.Add("Lead1", Type.GetType("System.String"))
        dtProduct.Columns.Add("Lead2", Type.GetType("System.String"))
        dtProduct.Columns.Add("Lead3", Type.GetType("System.String"))

        dtProduct.Columns.Add("VenCodeP", Type.GetType("System.String"))
        dtProduct.Columns.Add("VenCode1", Type.GetType("System.String"))
        dtProduct.Columns.Add("VenCode2", Type.GetType("System.String"))
        dtProduct.Columns.Add("VenCode3", Type.GetType("System.String"))

        Dim dtr As DataRow
        dtr = dtProduct.NewRow()
        dtr("CoyId") = HttpContext.Current.Session("CompanyId")
        dtr("ProductCode") = ViewState("strItemId") ' lblItemId.Text


        dtr("VendorItemCode") = txtVendorItemCode.Text
        dtr("ItemName") = txtItemName.Text ' dtProduct.Columns.Add("ProductDesc", Type.GetType("System.String"))
        dtr("ReferenceNo") = txtRefNo.Text
        dtr("Description") = txtDesc.Text
        dtr("CommodityType") = Me.hidCommodityType.Value 'txtCommodityType.Text 'dtr("CategoryName", Type.GetType("System.String"))
        'dtr("AccCode") = txtAccCode.Text
        dtr("AccCode") = ddlAccCode.SelectedItem.Value
        dtr("UOM") = cboUOM.SelectedItem.Text
        dtr("CatCode") = ddlCatCode.SelectedItem.Value
        dtr("Prefer") = IIf(hidPreferVendor.Text = "", "", hidPreferVendor.Text)
        dtr("1st") = IIf(hidPreferVendor1st.Text = "", "", hidPreferVendor1st.Text)
        dtr("2nd") = IIf(hidPreferVendor2nd.Text = "", "", hidPreferVendor2nd.Text)
        dtr("3rd") = IIf(hidPreferVendor3rd.Text = "", "", hidPreferVendor3rd.Text)

        dtr("PreferTax") = cboPreferTax.SelectedItem.Value
        dtr("1stTax") = cbo1stTax.SelectedItem.Value
        dtr("2ndTax") = cbo2ndTax.SelectedItem.Value
        dtr("3rdTax") = cbo3rdTax.SelectedItem.Value

        dtr("Manu") = txtManu.Text
        dtr("rd1") = rd1.SelectedItem.Value
        dtr("rd2") = rd2.SelectedItem.Value

        dtr("LeadP") = txtLeadP.Text
        dtr("Lead1") = txtLead1.Text
        dtr("Lead2") = txtLead2.Text
        dtr("Lead3") = txtLead3.Text

        dtr("VenCodeP") = txtVenCodeP.Text
        dtr("VenCode1") = txtVenCode1.Text
        dtr("VenCode2") = txtVenCode2.Text
        dtr("VenCode3") = txtVenCode3.Text

        If rd1.SelectedItem.Value = "SP" Then
            dtr("rd2") = "N"
        End If

        If txtMinInv.Text = "" Then
            dtr("MinInv") = 0
        Else
            dtr("MinInv") = txtMinInv.Text
        End If
        If txtMin.Text = "" Then
            dtr("Min") = 0
        Else
            dtr("Min") = txtMin.Text
        End If
        If txtMax.Text = "" Then
            dtr("Max") = 0
        Else
            dtr("Max") = txtMax.Text
        End If

        If txtMaxInvQty.Text = "" Then
            dtr("MaxInvQty") = 0
        Else
            dtr("MaxInvQty") = txtMaxInvQty.Text
        End If

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
        dtr("Remark") = txtRemarks.Text
        If chkStatus.Checked Then
            dtr("Deleted") = "N"
        Else
            dtr("Deleted") = "Y"
        End If

        'If ViewState("mode") = "add" Then hidVendorItemCode.Value = txtVendorItemCode.Text

        dtProduct.Rows.Add(dtr)
        ds.Tables.Add(dtProduct)

        'If IsNumeric(txtPrice.Text) And IsNumeric(hidPrice.Value) Then
        '    If CDbl(hidPrice.Value) <> CDbl(txtPrice.Text) Then
        '        blnList = True
        '    End If
        'ElseIf hidPrice.Value = "" And txtPrice.Text <> "" Then
        '    blnList = True
        'End If
        bindProduct = ds
    End Function

    Private Sub cmdReset_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.ServerClick
        objCat.deleteTempAttachment(ViewState("itemid"), Session.SessionID)
        isNotPostBack()
    End Sub

    Private Sub cmdadd_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdadd.ServerClick
        ViewState("mode") = "add"
        ViewState("ImageIndex") = ""
        displayImageFile(ViewState("ImageIndex"))
        txtVendorItemCode.Enabled = True
        displayAttachFile(True)
        isNotPostBack()
    End Sub

    Private Sub rd1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rd1.SelectedIndexChanged
        If rd1.SelectedItem.Value <> "SP" Then
            rd2.Enabled = True
        Else
            rd2.Enabled = False
        End If
    End Sub
End Class
