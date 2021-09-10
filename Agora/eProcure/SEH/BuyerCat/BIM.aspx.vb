Imports AgoraLegacy
Imports eProcure.Component



Public Class BIMSEH
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objCat As New ContCat
    Dim objCat_Ext As New ContCat_Ext
    Dim arySetVendor As New ArrayList()
    Dim aryTemp_Vendor As New ArrayList()
    Dim intNumeric As Boolean = True
    Dim intCheck As Boolean = True
    Dim objGlobal As New AppGlobals
    Dim objGlobal_Ext As New AppGlobals_Ext
    Dim objDb As New EAD.DBCom
    Dim objFile As New FileManagement
    Dim blnDiscount As Boolean
    Dim cbolist As New ListItem

    'Protected WithEvents txtCategory As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cboPrefer As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents txtPreferVendor As System.Web.UI.WebControls.TextBox
    'Protected WithEvents hidPreferVendor As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtPreferVendor1st As System.Web.UI.WebControls.TextBox
    'Protected WithEvents hidPreferVendor1st As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtPreferVendor2nd As System.Web.UI.WebControls.TextBox
    'Protected WithEvents hidPreferVendor2nd As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtPreferVendor3rd As System.Web.UI.WebControls.TextBox
    'Protected WithEvents hidPreferVendor3rd As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cbo1st As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents cbo2nd As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents cbo3rd As System.Web.UI.WebControls.DropDownList
    Protected WithEvents chkStatus As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkPartialDel As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtVendorItemCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtItemName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRefNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCommodityType As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEOQ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRatio As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRQL As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBudgetPrice As System.Web.UI.WebControls.TextBox
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
    Protected WithEvents txtSpecification1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSpecification2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSpecification3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPackQty As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtManu As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtManu2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtManu3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNewItemCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLocCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSecCode As System.Web.UI.WebControls.TextBox

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
    Protected WithEvents Label19 As System.Web.UI.WebControls.Label
    Protected WithEvents Label20 As System.Web.UI.WebControls.Label

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

    Protected WithEvents ddlIQCType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboUOM As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlAccCode As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlCatCode As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlPackType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents vldVItemCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldItemName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldCommodityType As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldUOM As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldCurrencyCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    'Protected WithEvents vldDesc As System.Web.UI.WebControls.RequiredFieldValidator

    Protected WithEvents cmdVendor As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSelect As System.Web.UI.WebControls.Button
    Protected WithEvents cmdUploadImage As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button

    Protected WithEvents hidBtn As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidBtn2 As System.Web.UI.HtmlControls.HtmlInputButton
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
    Protected WithEvents txtMaxInvQty As System.Web.UI.WebControls.TextBox
    Protected WithEvents rd1 As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rd2 As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rd3 As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents dr2 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents trVendor As System.Web.UI.HtmlControls.HtmlTableRow

    'Protected WithEvents revPrice As System.Web.UI.WebControls.RegularExpressionValidator
    Dim blnList As Boolean
    Dim strItemId As String
    Dim strscript As New System.Text.StringBuilder
    Dim strFileName As String
    Dim strName As String

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

            If rd1.Items.FindByValue("ST").Selected = True Or rd1.Items.FindByValue("MI").Selected = True Then
                rd2.Enabled = True
                Label19.Visible = True
            Else
                Label19.Visible = False
                ddlIQCType.SelectedIndex = 0
            End If

            If rd2.Items.FindByValue("Y").Selected = True Then
                ddlIQCType.Enabled = True
                Label20.Visible = True
            Else
                ddlIQCType.Enabled = False
                Label20.Visible = False
            End If
        Else
            If rd1.SelectedItem.Value <> "SP" Then
                rd2.Enabled = True
                Label19.Visible = True
            Else
                Label19.Visible = False
            End If

            If rd2.SelectedItem.Value = "Y" Then
                ddlIQCType.Enabled = True
                Label20.Visible = True
            Else
                ddlIQCType.Enabled = False
                Label20.Visible = False
            End If
        End If
        displayImageFile(ViewState("ImageIndex"))
        displayAttachFile()
        lnkBack.NavigateUrl = dDispatcher.direct("BuyerCat", "SearchBItem.aspx", "pageid=" & strPageId)
        txtVendorItemCode.Attributes.Add("onblur", "checkDup();")
        cmdUploadImage.Attributes.Add("onclick", "return checkDocFile('img','" & FileProImage.ClientID & "');")
        cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & FileDoc.ClientID & "');")
        Image2.ImageUrl = dDispatcher.direct("Plugins/images", "collapse_up.gif")
        Image1.ImageUrl = dDispatcher.direct("Plugins/images", "collapse_up.gif")

        'cmdSave.Attributes.Add("onClick", "return resetSummary(0,1);")

        'rd1.Attributes.Add("onClick", "show();")

    End Sub

    Private Sub isNotPostBack()
        vldSumm.ShowSummary = False
        hidVendorItemCode.Value = ""
        hidItemName.Value = ""
        lblMsg.Text = ""
        ViewState("blnCmdSubmit") = True
        ViewState("blnCmdUploadDoc") = True
        ViewState("blnCmdUploadImg") = True
        Session("arySetVendor") = Nothing
        Session("arySetPrice") = Nothing
        Session("aryPrice") = Nothing
        Session("arySetPrice1") = Nothing
        Session("arySetPrice2") = Nothing
        ViewState("mode") = Request.QueryString("mode")
        BindIQCTestType()
        objCat_Ext.FillGLCode(ddlAccCode)
        objCat.FillCatCode(ddlCatCode)
        objGlobal.FillCodeTable(cboUOM, CodeTable.Uom)
        objGlobal_Ext.FillPackType(ddlPackType)

        'Me.btnSetPrice3.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("BuyerCat", "UnitPriceMaster.aspx"))

        BuildRow()
        ConstructTable()
        PopulateTypeAhead()
        DisplayUserCheckBtn()

        If rd1.Items.FindByValue("ST").Selected = True Then
            trVendor.Style("display") = ""
            txtCommodityType.Text = "SEH-STOCK"
            hidCommodityType.Value = objDb.GetVal("SELECT CT_ID FROM COMMODITY_TYPE WHERE CT_NAME = 'SEH-STOCK'")
        Else
            trVendor.Style("display") = "none"
        End If

        'If rd1.SelectedItem.Value = "ST" Then
        '    trVendor.Style("display") = ""
        '    txtCommodityType.Text = "SEH-STOCK"
        '    hidCommodityType.Value = "52987"
        'Else
        '    trVendor.Style("display") = "none"
        'End If


        If ViewState("mode") = "mod" Then
            If ViewState("itemid") Is Nothing Then ViewState("itemid") = Request.QueryString("itemid")
            displayData()
            cmdReset.Value = "Reset"
            If rd1.SelectedValue = "ST" Then
                'Me.txtRQL.ReadOnly = True
                'Me.txtMinInv.ReadOnly = True
                'Me.txtMaxInvQty.ReadOnly = True

                Me.txtRQL.Enabled = False
                Me.txtMinInv.Enabled = False
                Me.txtMaxInvQty.Enabled = False
            End If
            cmdadd.Visible = False
        Else
            ViewState("itemid") = ""
            cmdReset.Value = "Clear"
            clearTextBox()
            cmdReset.Visible = False
        End If
        objCat.deleteTempAttachment(ViewState("itemid"), Session.SessionID)
    End Sub

    Private Sub clearTextBox()
        Me.txtVendorItemCode.Text = ""
        Me.txtItemName.Text = ""
        Me.txtRefNo.Text = ""
        Me.txtDesc.Text = ""
        If rd1.Items.FindByValue("ST").Selected = False Then
            Me.txtCommodityType.Text = ""
        End If

        'Me.txtAccCode.Text = ""
        Me.txtMinInv.Text = ""
        Me.txtMin.Text = ""
        Me.txtMax.Text = ""
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
        Me.txtSecCode.Text = ""
        Me.txtLocCode.Text = ""
        Me.txtNewItemCode.Text = ""
        Me.txtRemarks.Text = ""

        Me.txtManu.Text = ""
        Me.txtManu2.Text = ""
        Me.txtManu3.Text = ""
        Me.txtSpecification1.Text = ""
        Me.txtSpecification2.Text = ""
        Me.txtSpecification3.Text = ""
        Me.txtPackQty.Text = ""
        Me.txtMaxInvQty.Text = ""

        'Me.rd1.SelectedValue = "SP"

        Me.rd2.SelectedValue = "N"
        Me.rd3.SelectedValue = "N"
        If rd1.Items.FindByValue("SP").Selected = False Then
            Me.rd2.Enabled = True
        Else
            Me.rd2.Enabled = False
        End If

        If rd2.Items.FindByValue("N").Selected = True Then
            ddlIQCType.Enabled = False
            ddlIQCType.SelectedIndex = 0
        Else
            ddlIQCType.Enabled = True
        End If


    End Sub

    Private Function displayData()
        Dim ds As New DataSet
        Dim objAdmin As New Admin

        ds = objCat_Ext.getSingleProduct(ViewState("itemid"), False)
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
            txtEOQ.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_EOQ"))
            txtRatio.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_RATIO"))

            If Common.parseNull(ds.Tables(0).Rows(0)("PM_PARTIAL_CD")) = "Y" Then
                chkPartialDel.Checked = True
            Else
                chkPartialDel.Checked = False
            End If

            Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("PM_UOM")), cboUOM, False, True)
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
            txtSpecification1.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_SPEC1"))
            txtSpecification2.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_SPEC2"))
            txtSpecification3.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_SPEC3"))
            txtManu.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_MANUFACTURER"))
            txtManu2.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_MANUFACTURER2"))
            txtManu3.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_MANUFACTURER3"))
            txtSecCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_SECTION"))
            txtLocCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_LOCATION"))
            txtNewItemCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_NEW_ITEM_CODE"))
            If Common.parseNull(ds.Tables(0).Rows(0)("PM_DELETED")) = "N" Then
                chkStatus.Checked = True
            Else
                chkStatus.Checked = False
            End If

            'txtAccCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_ACCT_CODE"))
            Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("PM_IQC_TYPE")), ddlIQCType, False, True)
            Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("PM_PACKING_TYPE")), ddlPackType, False, True)
            Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("PM_ACCT_CODE")), ddlAccCode, False, True)
            Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("PM_CAT_CODE")), ddlCatCode, False, True)

            txtMaxInvQty.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_MAX_INV_QTY"))
            txtMinInv.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_SAFE_QTY"))
            txtMin.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_ORD_MIN_QTY"))
            txtMax.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_ORD_MAX_QTY"))
            txtRQL.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_REORDER_QTY"))
            txtBudgetPrice.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_BUDGET_PRICE"))
            ddlPackType.SelectedValue = Common.parseNull(ds.Tables(0).Rows(0)("PM_PACKING_TYPE"))
            txtPackQty.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_PACKING_QTY"))

            ' display attached files
            ViewState("ImageIndex") = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_IMAGE"))
            ViewState("blnTemp") = False
            displayAttachFile()
            displayImageFile(ViewState("ImageIndex"))

            rd1.SelectedValue = Common.parseNull(ds.Tables(0).Rows(0)("PM_ITEM_TYPE"))
            rd2.SelectedValue = Common.parseNull(ds.Tables(0).Rows(0)("PM_IQC_IND"))
            rd3.SelectedValue = Common.parseNull(ds.Tables(0).Rows(0)("PM_OVERSEA"))

            If rd1.SelectedValue <> "ST" Then
                rd2.Enabled = False
                trVendor.Style("display") = "none"
            Else
                rd2.Enabled = True
                trVendor.Style("display") = ""
            End If
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

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        'If txtPreferVendor.Text = "" Then
        '    hidPreferVendor.Text = ""
        'End If
        'If txtPreferVendor1st.Text = "" Then
        '    hidPreferVendor1st.Text = ""
        'End If
        'If txtPreferVendor2nd.Text = "" Then
        '    hidPreferVendor2nd.Text = ""
        'End If
        'If txtPreferVendor3rd.Text = "" Then
        '    hidPreferVendor3rd.Text = ""
        'End If
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

        Rebuild()
        If Page.IsValid And validateData(strMsg) Then
            lblMsg.Text = ""
            dsProduct = bindProduct()

            If intCheck = False Then
                Common.NetMsgbox(Me, "More than 4 vendors entered for spot/mro item is not allowed.", MsgBoxStyle.Information)
                Exit Sub
            End If

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


                        If objCat_Ext.BIM(dsProduct, ViewState("mode"), ViewState("ImageIndex"), ViewState("strItemId"), hidVendorItemCode.Value, Session("arySaveVendor"), hidVendorItemCode.Value, hidItemName.Value, , Session("aryPrice")) Then
                            'If Not Session("arySetPrice") Is Nothing Then
                            '    objCat_Ext.InsertUnitPrice(ViewState("mode"), txtVendorItemCode.Text, hidPreferVendor.Text, "1", Session("arySetPrice1"))
                            '    objCat_Ext.InsertUnitPrice(ViewState("mode"), txtVendorItemCode.Text, hidPreferVendor1st.Text, "2", Session("arySetPrice2"))
                            '    objCat_Ext.InsertUnitPrice(ViewState("mode"), txtVendorItemCode.Text, hidPreferVendor2nd.Text, "3", Session("arySetPrice3"))
                            '    objCat_Ext.InsertUnitPrice(ViewState("mode"), txtVendorItemCode.Text, hidPreferVendor3rd.Text, "4", Session("arySetPrice4"))
                            'End If
                            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00003"), MsgBoxStyle.Information)
                            Me.cmdSave.Enabled = False
                            If rd1.SelectedValue = "ST" Then
                                Me.txtRQL.Enabled = False
                                Me.txtMinInv.Enabled = False
                                Me.txtMaxInvQty.Enabled = False
                            End If
                            'ViewState("ImageIndex") = ""
                            'ViewState("ImageIndex") = objDb.Get1Column("PRODUCT_ATTACHMENT", "PA_ATTACH_INDEX ", " WHERE PA_PRODUCT_CODE ='" & dsProduct.Tables(0).Rows(0)("ProductCode") & "' AND PA_TYPE = 'I' ")
                            ViewState("ImageIndex") = objDb.Get1Column("PRODUCT_ATTACHMENT", "PA_ATTACH_INDEX ", " WHERE PA_PRODUCT_CODE ='" & ViewState("strItemId") & "' AND PA_TYPE = 'I' ")

                            displayImageFile(ViewState("ImageIndex"))

                            'dsNewImage = objDb.FillDs("SELECT * FROM PRODUCT_MSTR WHERE PM_VENDOR_ITEM_CODE = '" & dsProduct.Tables(0).Rows(0)("VendorItemCode") & "' AND PM_S_COY_ID = '" & Session("CompanyId") & "'")
                            'strNewImage = dsNewImage.Tables(0).Rows(0)("PM_PRODUCT_IMAGE")
                            'If strNewImage <> "" Then ViewState("ImageIndex") = strNewImage
                            hidVendorItemCode.Value = dsProduct.Tables(0).Rows(0)("VendorItemCode")
                            hidItemName.Value = dsProduct.Tables(0).Rows(0)("ItemName")

                            If Me.txtBudgetPrice.Text = "" Then
                                Me.txtBudgetPrice.Text = Format(0, "###0.00")
                            Else
                                Me.txtBudgetPrice.Text = Format(CDbl(txtBudgetPrice.Text), "###0.00")
                            End If

                            If Me.txtRQL.Text = "" Then
                                Me.txtRQL.Text = Format(0, "###0.00")
                            Else
                                Me.txtRQL.Text = Format(CDbl(txtRQL.Text), "###0.00")
                            End If

                            If Me.txtMinInv.Text = "" Then
                                Me.txtMinInv.Text = Format(0, "###0.00")
                            Else
                                Me.txtMinInv.Text = Format(CDbl(txtMinInv.Text), "###0.00")
                            End If

                            If Me.txtMin.Text = "" Then
                                Me.txtMin.Text = Format(0, "###0.00")
                            Else
                                Me.txtMin.Text = Format(CDbl(txtMin.Text), "###0.00")
                            End If

                            If Me.txtMax.Text = "" Then
                                Me.txtMax.Text = Format(0, "###0.00")
                            Else
                                Me.txtMax.Text = Format(CDbl(txtMax.Text), "###0.00")
                            End If

                            If Me.txtMaxInvQty.Text = "" Then
                                Me.txtMaxInvQty.Text = Format(0, "###0.00")
                            Else
                                Me.txtMaxInvQty.Text = Format(CDbl(txtMaxInvQty.Text), "###0.00")
                            End If

                            If Me.txtPackQty.Text = "" Then
                                Me.txtPackQty.Text = Format(0, "###0.00")
                            Else
                                Me.txtPackQty.Text = Format(CDbl(txtPackQty.Text), "###0.00")
                            End If

                            ViewState("mode") = "mod"
                            ViewState("itemid") = ViewState("strItemId")
                            txtVendorItemCode.Enabled = False
                            displayAttachFile()
                            ConstructTable("Save", Session("arySetVendor"))
                        Else
                            ConstructTable("", Session("aryPrevSetVendor"))
                            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00007"), MsgBoxStyle.Information)
                        End If
                    Else
                        ConstructTable("", Session("aryPrevSetVendor"))
                        Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00363"), MsgBoxStyle.Information)
                    End If
                Else
                    ConstructTable("", Session("aryPrevSetVendor"))
                    Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00018"), MsgBoxStyle.Information)
                End If
            Else
                ConstructTable("", Session("aryPrevSetVendor"))
                Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00002"), MsgBoxStyle.Information)
            End If

        Else
            If strMsg <> "" Then
                lblMsg.Text = strMsg
                ConstructTable("", Session("aryPrevSetVendor"))
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
        dtProduct.Columns.Add("MinInv", Type.GetType("System.Decimal")) 'PM_SAFE_QTY
        dtProduct.Columns.Add("Min", Type.GetType("System.Decimal")) 'PM_ORD_MIN_QTY
        dtProduct.Columns.Add("Max", Type.GetType("System.Decimal")) 'PM_ORD_MAX_QTY
        dtProduct.Columns.Add("RQL", Type.GetType("System.String"))
        dtProduct.Columns.Add("BudgetPrice", Type.GetType("System.String"))
        dtProduct.Columns.Add("EOQ", Type.GetType("System.String"))
        dtProduct.Columns.Add("Ratio", Type.GetType("System.String"))

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
        dtProduct.Columns.Add("PartialDelivery", Type.GetType("System.String"))
        dtProduct.Columns.Add("IQCType", Type.GetType("System.String"))
        'dtProduct.Columns.Add("Prefer", Type.GetType("System.String"))
        'dtProduct.Columns.Add("1st", Type.GetType("System.String"))
        'dtProduct.Columns.Add("2nd", Type.GetType("System.String"))
        'dtProduct.Columns.Add("3rd", Type.GetType("System.String"))
        dtProduct.Columns.Add("MaxInvQty", Type.GetType("System.Decimal"))
        dtProduct.Columns.Add("Manu", Type.GetType("System.String"))
        dtProduct.Columns.Add("Manu2", Type.GetType("System.String"))
        dtProduct.Columns.Add("Manu3", Type.GetType("System.String"))
        dtProduct.Columns.Add("Spec", Type.GetType("System.String"))
        dtProduct.Columns.Add("Spec2", Type.GetType("System.String"))
        dtProduct.Columns.Add("Spec3", Type.GetType("System.String"))
        dtProduct.Columns.Add("PackType", Type.GetType("System.String"))
        dtProduct.Columns.Add("PackQty", Type.GetType("System.String"))
        dtProduct.Columns.Add("SectionCode", Type.GetType("System.String"))
        dtProduct.Columns.Add("LocationCode", Type.GetType("System.String"))
        dtProduct.Columns.Add("NewItemCode", Type.GetType("System.String"))
        dtProduct.Columns.Add("rd1", Type.GetType("System.String"))
        dtProduct.Columns.Add("rd2", Type.GetType("System.String"))
        dtProduct.Columns.Add("rd3", Type.GetType("System.String"))

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
        dtr("IQCType") = ddlIQCType.SelectedItem.Value
        dtr("EOQ") = txtEOQ.Text
        dtr("Ratio") = txtRatio.Text

        'dtr("Prefer") = IIf(hidPreferVendor.Text = "", "", hidPreferVendor.Text)
        'dtr("1st") = IIf(hidPreferVendor1st.Text = "", "", hidPreferVendor1st.Text)
        'dtr("2nd") = IIf(hidPreferVendor2nd.Text = "", "", hidPreferVendor2nd.Text)
        'dtr("3rd") = IIf(hidPreferVendor3rd.Text = "", "", hidPreferVendor3rd.Text)

        dtr("PackType") = ddlPackType.SelectedItem.Value
        dtr("SectionCode") = txtSecCode.Text
        dtr("LocationCode") = txtLocCode.Text
        dtr("NewItemCode") = txtNewItemCode.Text

        dtr("Manu") = txtManu.Text
        dtr("Manu2") = txtManu2.Text
        dtr("Manu3") = txtManu3.Text
        dtr("Spec") = txtSpecification1.Text
        dtr("Spec2") = txtSpecification2.Text
        dtr("Spec3") = txtSpecification3.Text
        dtr("rd1") = rd1.SelectedItem.Value
        dtr("rd2") = rd2.SelectedItem.Value
        dtr("rd3") = rd3.SelectedItem.Value

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

        If txtRQL.Text = "" Then
            dtr("RQL") = 0
        Else
            dtr("RQL") = txtRQL.Text
        End If

        If txtBudgetPrice.Text = "" Then
            dtr("BudgetPrice") = 0
        Else
            dtr("BudgetPrice") = txtBudgetPrice.Text
        End If

        If txtPackQty.Text = "" Then
            dtr("PackQty") = 0
        Else
            dtr("PackQty") = txtPackQty.Text
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

        If chkPartialDel.Checked Then
            dtr("PartialDelivery") = "Y"
        Else
            dtr("PartialDelivery") = "N"
        End If

        dtProduct.Rows.Add(dtr)
        ds.Tables.Add(dtProduct)

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
        If rd1.SelectedItem.Value = "ST" Then
            Label19.Visible = True
            rd2.Enabled = True
            trVendor.Style("display") = ""
            txtCommodityType.Text = "SEH-STOCK"
            hidCommodityType.Value = objDb.GetVal("SELECT CT_ID FROM COMMODITY_TYPE WHERE CT_NAME = 'SEH-STOCK'")
        ElseIf rd1.SelectedItem.Value = "MI" Then
            Label19.Visible = True
            rd2.Enabled = True
            trVendor.Style("display") = "none"
            txtCommodityType.Text = ""
            hidCommodityType.Value = ""
        Else
            Label19.Visible = False
            Label20.Visible = False
            ddlIQCType.Enabled = False
            rd2.SelectedValue = "N"
            rd2.Enabled = False
            trVendor.Style("display") = "none"
            txtCommodityType.Text = ""
            hidCommodityType.Value = ""
        End If

        ddlIQCType.SelectedIndex = 0
    End Sub

    Private Sub rd2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rd2.SelectedIndexChanged
        If rd2.SelectedItem.Value = "Y" Then
            Label20.Visible = True
            ddlIQCType.Enabled = True
        Else
            Label20.Visible = False
            ddlIQCType.Enabled = False
            ddlIQCType.SelectedIndex = 0
        End If
    End Sub

    Private Function BuildRow()
        Dim i, j As Integer
        Dim found As Boolean
        Dim strsql As String
        found = False
        Dim dsCount As New DataSet
        Dim objDb As New EAD.DBCom

        If Request.QueryString("mode") = "mod" Then
            dsCount = objDb.FillDs("SELECT '*' FROM PIM_VENDOR WHERE PV_PRODUCT_INDEX = (SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & Request.QueryString("itemid") & "') ")
            ViewState("Row") = dsCount.Tables(0).Rows.Count

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
                arySetVendor.Add(New String() {"Preferred Vendor", "P", "", "", "", "", "", "", "", "", "", "", "", "Add"})
            ElseIf i = 1 Then
                arySetVendor.Add(New String() {"1st Alternative Vendor", "1", "", "", "", "", "", "", "", "", "", "", "", "Add"})
            ElseIf i = 2 Then
                arySetVendor.Add(New String() {"2nd Alternative Vendor", "2", "", "", "", "", "", "", "", "", "", "", "", "Add"})
            ElseIf i = 3 Then
                arySetVendor.Add(New String() {"3rd Alternative Vendor", "3", "", "", "", "", "", "", "", "", "", "", "", "Add"})
            Else
                arySetVendor.Add(New String() {i & "th Alternative Vendor", i, "", "", "", "", "", "", "", "", "", "", "", "Add"})
            End If
        Next

        Session("arySetVendor") = arySetVendor

    End Function

    Private Function ConstructTable(Optional ByVal strSave As String = "", Optional ByVal aryVolume As ArrayList = Nothing)
        Dim strrow As String = ""
        Dim i, j, count As Integer
        Dim c As Integer
        Dim table As String
        Dim item2nd As Boolean = False
        Dim found As Boolean = False
        Dim objAdmin_ext As New Admin_Ext
        Dim arySetVendor_temp As New ArrayList()
        'Dim ds1 As New DataSet
        Dim dsPayTerm As New DataSet
        'Dim ds3 As New DataSet
        Dim dsTax As New DataSet
        Dim dsVC As New DataSet
        Dim dsInfo As New DataSet
        'Dim strDefPayTerm As String

        'arySetPrice = Session("arySetPrice")
        If Not aryVolume Is Nothing Then
            arySetVendor = aryVolume
        Else
            If Request.QueryString("mode") = "mod" And strSave = "" Then
                DisplayVendorData()

            End If
        End If


        'ds1 = objGlobal_Ext.FillDelTermForVen()
        dsPayTerm = objGlobal_Ext.FillPayTermForVen()
        'ds3 = objGlobal_Ext.FillCurrencyForVen()
        dsTax = objGlobal_Ext.FillTaxForVen()

        count = arySetVendor.Count

        For i = 0 To count - 1
            dsVC = objGlobal_Ext.FillSuppCodeForVen(arySetVendor(i)(3))
            dsInfo = objGlobal_Ext.FillSuppCodeForVen(arySetVendor(i)(3), arySetVendor(i)(4))


            'Continue For
            strrow &= "<tr>"
            strrow &= "<td class=""tablecol""><strong>&nbsp;" & arySetVendor(i)(0) & " :</strong></td>"
            strrow &= "<td>"
            strrow &= "<input style=""width:100%; margin-right:0px; "" class=""txtbox"" type=""text"" onchange =""onClick()"" id=""txtVendor" & i & """ name=""txtVendor" & i & """ value=""" & arySetVendor(i)(2) & """>"
            strrow &= "<input type=""hidden"" id=""hidVendor" & i & """ name=""hidVendor" & i & """ value=""" & arySetVendor(i)(3) & """ runat=""server"">"
            strrow &= "</td>"
            strrow &= "<td>"
            'strrow &= "<input style=""width:100%; margin-right:0px; "" class=""txtbox"" type=""text"" id=""txtSuppCode" & i & """ name=""txtSuppCode" & i & """ value=""" & arySetVendor(i)(4) & """>"
            strrow &= "<select class=""ddl"" style=""width:100%; margin-right:0px;"" onchange =""onClick()"" id=""cboSuppCode" & i & """ name=""cboSuppCode" & i & """>"
            strrow &= "<option title=""---Select---"" value="""" selected=""selected"">" & "---Select---" & "</option>"

            For c = 0 To dsVC.Tables(0).Rows.Count - 1
                If arySetVendor(i)(4) = dsVC.Tables(0).Rows(c).Item(0).ToString Then
                    strrow &= "<option title=""" & dsVC.Tables(0).Rows(c).Item("CVS_SUPP_CODE").ToString & """ value=""" & dsVC.Tables(0).Rows(c).Item("CVS_SUPP_CODE").ToString & """ selected=""selected"">" & dsVC.Tables(0).Rows(c).Item("CVS_SUPP_CODE").ToString & "</option>"
                Else
                    strrow &= "<option title=""" & dsVC.Tables(0).Rows(c).Item("CVS_SUPP_CODE").ToString & """ value=""" & dsVC.Tables(0).Rows(c).Item("CVS_SUPP_CODE").ToString & """>" & dsVC.Tables(0).Rows(c).Item("CVS_SUPP_CODE").ToString & "</option>"
                End If
            Next

            strrow &= "</td>"
            strrow &= "<td>"

            If arySetVendor(i)(4) <> "" Then
                If dsInfo.Tables(0).Rows.Count > 0 Then
                    strrow &= "<input type=""hidden"" id=""hidDelTerm" & i & """ name=""hidDelTerm" & i & """ value=""" & dsInfo.Tables(0).Rows(0).Item("CDT_DEL_CODE").ToString & """>"
                    strrow &= "<p title=""" & dsInfo.Tables(0).Rows(0).Item("CDT_DEL_CODE").ToString & " (" & dsInfo.Tables(0).Rows(0).Item("CDT_DEL_NAME").ToString & ")"">" & dsInfo.Tables(0).Rows(0).Item("CDT_DEL_CODE").ToString & "</p>"
                Else
                    strrow &= "<input type=""hidden"" id=""hidDelTerm" & i & """ name=""hidDelTerm" & i & """ value="""">"
                    'strrow &= "<label id=""lblDelTerm" & i & """ style=""width:100%;margin-right:0px; "" class=""txtbox""></label>"
                    'strrow &= "<label id=""lblDelTerm" & i & """ style=""width:100%;margin-right:0px; "" class=""txtbox""></label>"
                    strrow &= "<p title=""""></p>"
                End If
            Else
                strrow &= "<input type=""hidden"" id=""hidDelTerm" & i & """ name=""hidDelTerm" & i & """ value="""">"
                strrow &= "<p title=""""></p>"
            End If

            strrow &= "</td>"
            strrow &= "<td>"

            If ViewState("mode") = "mod" Then
                strFileName = dDispatcher.direct("BuyerCat", "UnitPriceMaster.aspx", "vid=" & arySetVendor(i)(3) & "&vline=" & arySetVendor(i)(1) & "&mode=mod&itemid=" & Request.QueryString("itemid") & "")
            Else
                strFileName = dDispatcher.direct("BuyerCat", "UnitPriceMaster.aspx", "vid=" & arySetVendor(i)(3) & "&vline=" & arySetVendor(i)(1) & "")
            End If
            strFileName = Server.UrlEncode(strFileName)

            strrow &= "<input style=""width:100%;"" class=""button"" type=""button""  id=""btnPrice" & i & """ name=""btnPrice" & i & """ onclick=""ShowDialog('" & dDispatcher.direct("BuyerCat", "Dialog.aspx", "page=" & strFileName) & "','250px');"" value=""Set"">"
            strrow &= "<input type=""hidden"" id=""hidVen" & i & """ name=""hidVen" & i & """ value=""ShowDialog('" & dDispatcher.direct("BuyerCat", "Dialog.aspx", "page=" & strFileName) & "','250px');"" />"
            strrow &= "</td>"
            strrow &= "<td>"
            strrow &= "<select class=""ddl"" style=""width:100%; margin-right:0px;"" id=""cboPayTerm" & i & """ name=""cboPayTerm" & i & """>"
            strrow &= "<option value="""" selected=""selected"">" & "---Select---" & "</option>"

            For c = 0 To dsPayTerm.Tables(0).Rows.Count - 1
                If arySetVendor(i)(6) = dsPayTerm.Tables(0).Rows(c).Item(0).ToString Then
                    strrow &= "<option value=""" & dsPayTerm.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & dsPayTerm.Tables(0).Rows(c).Item(1).ToString & "</option>"
                Else
                    strrow &= "<option value=""" & dsPayTerm.Tables(0).Rows(c).Item(0).ToString & """>" & dsPayTerm.Tables(0).Rows(c).Item(1).ToString & "</option>"
                End If
            Next

            strrow &= "</select>"
            strrow &= "</td>"
            strrow &= "<td>"

            If arySetVendor(i)(4) <> "" Then
                If dsInfo.Tables(0).Rows.Count > 0 Then
                    strrow &= "<input type=""hidden"" id=""hidCurrency" & i & """ name=""hidCurrency" & i & """ value=""" & dsInfo.Tables(0).Rows(0).Item("CVS_CURR").ToString & """> "
                    strrow &= "<label id=""lblCurrency" & i & """ style=""width:100%;margin-right:0px; "" class=""txtbox"">" & dsInfo.Tables(0).Rows(0).Item("CVS_CURR_DESC").ToString & "</label>"
                Else
                    strrow &= "<input type=""hidden"" id=""hidCurrency" & i & """ name=""hidCurrency" & i & """ value="""">"
                    strrow &= "<label id=""lblCurrency" & i & """ style=""width:100%;margin-right:0px; "" class=""txtbox""></label>"
                End If
            Else
                strrow &= "<input type=""hidden"" id=""hidCurrency" & i & """ name=""hidCurrency" & i & """ value="""">"
                strrow &= "<label id=""lblCurrency" & i & """ style=""width:100%;margin-right:0px; "" class=""txtbox""></label>"
            End If

            strrow &= "</td>"
            strrow &= "<td>"
            strrow &= "<input style=""width:100%; margin-right:0px; "" class=""txtbox"" type=""text"" id=""txtPurSpecNo" & i & """ name=""txtPurSpecNo" & i & """ value=""" & arySetVendor(i)(8) & """>"
            strrow &= "</td>"
            strrow &= "<td>"
            strrow &= "<input style=""width:100%; margin-right:0px; "" class=""txtbox"" type=""text"" id=""txtRevision" & i & """ name=""txtRevision" & i & """ value=""" & arySetVendor(i)(9) & """>"
            strrow &= "</td>"

            '2014/10/16 - Chee Hong - Hide Tax column after GST Cut Off Date
            If ViewState("GSTCOD") = False Then
                strrow &= "<td>"
                strrow &= "<select class=""ddl"" style=""width:100%; margin-right:0px;"" id=""cboTax" & i & """ name=""cboTax" & i & """>"
                strrow &= "<option value=""1"" selected=""selected"">" & "---Select---" & "</option>"

                For c = 0 To dsTax.Tables(0).Rows.Count - 1
                    If arySetVendor(i)(10) = dsTax.Tables(0).Rows(c).Item(0).ToString Then
                        strrow &= "<option value=""" & dsTax.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & dsTax.Tables(0).Rows(c).Item(2).ToString & "</option>"
                    Else
                        strrow &= "<option value=""" & dsTax.Tables(0).Rows(c).Item(0).ToString & """>" & dsTax.Tables(0).Rows(c).Item(2).ToString & "</option>"
                    End If
                Next

                strrow &= "</select>"
                strrow &= "</td>"
            End If
            '---------------------------------------------------------------

            strrow &= "<td>"
            strrow &= "<input style=""width:100%; margin-right:0px; "" class=""numerictxtbox"" type=""text"" onkeypress=""return isNumberKey(event)"" id=""txtLead" & i & """ name=""txtLead" & i & """ value=""" & arySetVendor(i)(11) & """>"
            strrow &= "</td>"
            strrow &= "<td>"
            strrow &= "<input style=""width:100%; margin-right:0px; "" class=""txtbox"" type=""text"" id=""txtVenCode" & i & """ name=""txtVenCode" & i & """ value=""" & arySetVendor(i)(12) & """>"
            strrow &= "</td>"
        Next

        table = "<table class=""alltable"" id=""Table10"" width=""130%""cellspacing=""0"" cellpadding=""0"" border=""0"">" &
                "<tr>" &
                "<td class=""tablecol"" style=""width: 13%""></td><td class=""tablecol"" style=""width: 18%""><strong>&nbsp;Use type ahead feature to select vendor</strong></td>" &
                "<td class=""tablecol"" style=""width: 6%""><strong>&nbsp;Vendor &nbsp;Code</strong></td><td class=""tablecol"" style=""width: 7%""><strong>&nbsp;Delivery &nbsp;Term</strong></td>" &
                "<td class=""tablecol"" style=""width: 5%""><strong>&nbsp;Unit Price</strong></td><td class=""tablecol"" style=""width: 8%""><strong>&nbsp;Payment &nbsp;Term</strong></td>" &
                "<td class=""tablecol"" style=""width: 10%""><strong>&nbsp;Currency</strong></td><td class=""tablecol"" style=""width: 6%""><strong>&nbsp;Purchaser &nbsp;Spec No.</strong></td>" &
                "<td class=""tablecol"" style=""width: 5%""><strong>&nbsp;Revision</strong></td>"

        '2014/10/16 - Chee Hong - Hide Tax column after GST Cut Off Date
        If ViewState("GSTCOD") = False Then
            table &= "<td class=""tablecol"" style=""width: 7%""><strong>&nbsp;GST/VAT</strong></td>"
        End If
        '---------------------------------------------------------------

        table &= "<td class=""tablecol"" style=""width: 6%""><strong>&nbsp;Order Lead &nbsp;Time(Days)</strong></td><td class=""tablecol"" style=""width: 20%""><strong>&nbsp;Vendor Item Code</strong></td>" &
                "</tr>" &
                strrow &
                "</table>"

        Session("ConstructTableBIM") = table

    End Function

    Sub PopulateTypeAhead()

        'Session("vendortypeahead") = Nothing
        Dim typeahead As String
        Dim i As Integer
        Dim content As String = ""
        Dim nametypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=BIM")

        For i = 0 To arySetVendor.Count - 1
            content &= "$(""#txtVendor" & i & """).autocomplete(""" & nametypeahead & """, {" & vbCrLf &
            "width: 342," & vbCrLf &
            "scroll: true," & vbCrLf &
            "selectFirst: false" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtVendor" & i & """).result(function(event, data, formatted) {" & vbCrLf &
            "if (data)" & vbCrLf &
            "$(""#hidVendor" & i & """).val(data[1]);" & vbCrLf &
            "var clickevent = $(""#hidVen" & i & """).val();" & vbCrLf &
            "var changeclick = updateparam(clickevent,""vid="",""vid="" + encodeURIComponent($(""#hidVendor" & i & """).val() + """"));" & vbCrLf &
            "var newclick = Function(changeclick);" & vbCrLf &
            "document.getElementById(""btnPrice" & i & """).onclick = newclick;" & vbCrLf &
            "var bt = document.getElementById(""hidBtn2"");" & vbCrLf &
            "bt.click();" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtVendor" & i & """).keyup(function() {" & vbCrLf &
            "if ($(""#txtVendor" & i & """).val() == """") {" & vbCrLf &
            "var bt = document.getElementById(""hidBtn2"");" & vbCrLf &
            "bt.click(); }" & vbCrLf &
            "});" & vbCrLf

        Next
        '"var changeclick = updateparam(clickevent,encodeURIComponent(""UnitPriceMaster.aspx?vid=""),""UnitPriceMaster.aspx?vid="" + encodeURIComponent($(""#hidVendor" & i & """).val() + ""&vline=" & arySetVendor(i)(1) & "&""));" & vbCrLf & _

        typeahead = "<script language=""javascript"">" & vbCrLf &
          "<!--" & vbCrLf &
            "$(document).ready(function(){" & vbCrLf &
            content & vbCrLf &
            "});" & vbCrLf &
            "-->" & vbCrLf &
            "</script>"

        Session("vendortypeahead") = typeahead

    End Sub

    Private Function Rebuild()
        Dim i, j As Integer
        Dim c As Integer = 0
        Dim vl As String
        Dim found As Integer = 0
        Dim vfound As Boolean = True
        Dim aryTemp As New ArrayList()
        Dim aryTemp_Price As New ArrayList()
        Dim aryPrice As New ArrayList()
        Dim strType As String
        Dim objDb As New EAD.DBCom

        arySetVendor = Session("arySetVendor")

        'Store all info into array for before save
        For i = 0 To arySetVendor.Count - 1
            arySetVendor(i)(2) = Request.Form("txtVendor" & i)
            arySetVendor(i)(3) = Request.Form("hidVendor" & i)
            arySetVendor(i)(4) = Request.Form("cboSuppCode" & i)
            arySetVendor(i)(5) = Request.Form("hidDelTerm" & i)
            arySetVendor(i)(6) = Request.Form("cboPayTerm" & i)
            arySetVendor(i)(7) = Request.Form("hidCurrency" & i)
            arySetVendor(i)(8) = Request.Form("txtPurSpecNo" & i)
            arySetVendor(i)(9) = Request.Form("txtRevision" & i)
            If ViewState("GSTCOD") = True Then
                arySetVendor(i)(10) = ""
            Else
                arySetVendor(i)(10) = Request.Form("cboTax" & i)
            End If
            arySetVendor(i)(11) = Request.Form("txtLead" & i)
            arySetVendor(i)(12) = Request.Form("txtVenCode" & i)

            If Request.Form("txtVendor" & i) <> "" Then
                c = c + 1
            End If
        Next

        Session("aryPrevSetVendor") = arySetVendor

        'Store all info into array for after save
        If c > 4 Then
            ViewState("Row") = c
        Else
            ViewState("Row") = 4
        End If

        c = 0

        For i = 0 To ViewState("Row") - 1
            If i = 0 Then
                aryTemp.Add(New String() {"Preferred Vendor", "P", "", "", "", "", "", "", "", "", "", "", "", ""})
            ElseIf i = 1 Then
                aryTemp.Add(New String() {"1st Alternative Vendor", "1", "", "", "", "", "", "", "", "", "", "", "", ""})
            ElseIf i = 2 Then
                aryTemp.Add(New String() {"2nd Alternative Vendor", "2", "", "", "", "", "", "", "", "", "", "", "", ""})
            ElseIf i = 3 Then
                aryTemp.Add(New String() {"3rd Alternative Vendor", "3", "", "", "", "", "", "", "", "", "", "", "", ""})
            Else
                aryTemp.Add(New String() {i & "th Alternative Vendor", i, "", "", "", "", "", "", "", "", "", "", "", ""})
            End If
        Next

        For j = 0 To arySetVendor.Count - 1
            If Request.Form("txtVendor" & j) <> "" Then

                aryTemp(c)(2) = Request.Form("txtVendor" & j)
                aryTemp(c)(3) = Request.Form("hidVendor" & j)
                aryTemp(c)(4) = Request.Form("cboSuppCode" & j)
                aryTemp(c)(5) = Request.Form("hidDelTerm" & j)
                aryTemp(c)(6) = Request.Form("cboPayTerm" & j)
                aryTemp(c)(7) = Request.Form("hidCurrency" & j)
                aryTemp(c)(8) = Request.Form("txtPurSpecNo" & j)
                aryTemp(c)(9) = Request.Form("txtRevision" & j)
                If ViewState("GSTCOD") = True Then
                    aryTemp(c)(10) = ""
                Else
                    aryTemp(c)(10) = Request.Form("cboTax" & j)
                End If
                aryTemp(c)(11) = Request.Form("txtLead" & j)
                aryTemp(c)(12) = Request.Form("txtVenCode" & j)

                If j = 0 Then
                    aryTemp(c)(13) = "P"
                Else
                    aryTemp(c)(13) = j
                End If

                c = c + 1
            End If
        Next

        c = 0

        'Store all info into array for save into db
        For j = 0 To arySetVendor.Count - 1
            If j = 0 Then
                aryTemp_Vendor.Add(New String() {"Preferred Vendor", "P", "", "", "", "", "", "", "", "", "", "", "", ""})
            ElseIf j = 1 Then
                aryTemp_Vendor.Add(New String() {"1st Alternative Vendor", "1", "", "", "", "", "", "", "", "", "", "", "", ""})
            ElseIf j = 2 Then
                aryTemp_Vendor.Add(New String() {"2nd Alternative Vendor", "2", "", "", "", "", "", "", "", "", "", "", "", ""})
            ElseIf j = 3 Then
                aryTemp_Vendor.Add(New String() {"3rd Alternative Vendor", "3", "", "", "", "", "", "", "", "", "", "", "", ""})
            Else
                aryTemp_Vendor.Add(New String() {j & "th Alternative Vendor", j, "", "", "", "", "", "", "", "", "", "", "", ""})
            End If
        Next

        For j = 0 To aryTemp.Count - 1
            If aryTemp(j)(2) <> "" Then
                aryTemp_Vendor(j)(2) = aryTemp(j)(2)
                aryTemp_Vendor(j)(3) = aryTemp(j)(3)
                aryTemp_Vendor(j)(4) = aryTemp(j)(4)
                aryTemp_Vendor(j)(5) = aryTemp(j)(5)
                aryTemp_Vendor(j)(6) = aryTemp(j)(6)
                aryTemp_Vendor(j)(7) = aryTemp(j)(7)
                aryTemp_Vendor(j)(8) = aryTemp(j)(8)
                aryTemp_Vendor(j)(9) = aryTemp(j)(9)
                aryTemp_Vendor(j)(10) = aryTemp(j)(10)
                aryTemp_Vendor(j)(11) = aryTemp(j)(11)
                aryTemp_Vendor(j)(12) = aryTemp(j)(12)
                aryTemp_Vendor(j)(13) = aryTemp(j)(13)
            End If

        Next

        Session("arySaveVendor") = aryTemp_Vendor

        'Check array setprice
        aryPrice = Session("aryPrice")


        If Not aryPrice Is Nothing Then
            'Remove from aryPrice if vendor waas blank or empty
            For i = 0 To aryTemp.Count - 1
                If arySetVendor(i)(2) = "" Then
                    Do While j < aryPrice.Count
                        If aryPrice(j)(3) = arySetVendor(i)(1) Then
                            aryPrice.RemoveAt(j)
                            j = 0
                        Else
                            j = j + 1
                        End If
                    Loop
                End If

            Next

            'update vline from arytemp to aryprice
            For i = 0 To aryTemp.Count - 1
                For j = 0 To aryPrice.Count - 1
                    If aryTemp(i)(3) <> "" Then
                        If aryPrice(j)(3) = aryTemp(i)(13) And aryPrice(j)(2) = aryTemp(i)(3) Then
                            aryPrice(j)(3) = aryTemp(i)(1)
                        End If
                    End If

                Next
            Next

            Session("aryPrice") = aryPrice
        End If

        If Request.QueryString("mode") = "mod" Then
            strType = objDb.GetVal("SELECT PM_ITEM_TYPE FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & Request.QueryString("itemid") & "'")

            If strType <> "SP" And rd1.SelectedItem.Value = "SP" Then
                For j = 0 To aryTemp.Count - 1
                    If aryTemp(j)(2) <> "" Then
                        found = found + 1
                    End If
                Next

                If found > 4 Then
                    intCheck = False
                End If
            End If
        End If

        Session("arySetVendor") = aryTemp

    End Function

    Sub DisplayVendorData()
        Dim strItemType As String
        Dim objDB As New EAD.DBCom
        Dim ds5 As New DataSet
        Dim ds6 As New DataSet
        Dim d As Integer

        For d = 0 To arySetVendor.Count - 1
            ds6 = objCat_Ext.GetVendorInfo2(Request.QueryString("itemid"), arySetVendor(d)(1))

            If ds6.Tables(0).Rows.Count > 0 Then
                arySetVendor(d)(2) = Common.parseNull(ds6.Tables(0).Rows(0).Item("PV_S_COY_NAME"))       'Company Name
                arySetVendor(d)(3) = Common.parseNull(ds6.Tables(0).Rows(0).Item("PV_S_COY_ID"))         'Company ID
                If ViewState("GSTCOD") = True Then
                    arySetVendor(d)(10) = ""   'Tax
                Else
                    arySetVendor(d)(10) = Common.parseNull(ds6.Tables(0).Rows(0).Item("PV_S_COY_ID_TAX_ID"))   'Tax
                End If
                arySetVendor(d)(4) = Common.parseNull(ds6.Tables(0).Rows(0).Item("PV_SUPP_CODE"))     'Supplier Code
                arySetVendor(d)(5) = Common.parseNull(ds6.Tables(0).Rows(0).Item("PV_DELIVERY_TERM")) 'Delivery Term
                arySetVendor(d)(6) = Common.parseNull(ds6.Tables(0).Rows(0).Item("PV_PAYMENT_CODE"))  'Payment Term 
                arySetVendor(d)(7) = Common.parseNull(ds6.Tables(0).Rows(0).Item("PV_CURR"))    'Currency
                arySetVendor(d)(8) = Common.parseNull(ds6.Tables(0).Rows(0).Item("PV_PUR_SPEC_NO"))   'Purchase Spec No
                arySetVendor(d)(9) = Common.parseNull(ds6.Tables(0).Rows(0).Item("PV_REVISION"))  'Revision
                arySetVendor(d)(11) = Common.parseNull(ds6.Tables(0).Rows(0).Item("PV_LEAD_TIME"))   'Lead Time
                arySetVendor(d)(12) = Common.parseNull(ds6.Tables(0).Rows(0).Item("PV_VENDOR_CODE")) 'Vendor Item Code
            End If
        Next

    End Sub

    Private Sub cmdVendor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdVendor.Click
        'Rebuild()
        BuildLine()
        ConstructTable()
        PopulateTypeAhead()
    End Sub

    Private Function BuildLine()
        Dim i As Integer
        Dim found As Integer
        found = 0

        arySetVendor = Session("arySetVendor")
        For i = 0 To arySetVendor.Count - 1
            arySetVendor(i)(2) = Request.Form("txtVendor" & i)
            arySetVendor(i)(3) = Request.Form("hidVendor" & i)
            arySetVendor(i)(4) = Request.Form("cboSuppCode" & i)
            arySetVendor(i)(5) = Request.Form("hidDelTerm" & i)
            arySetVendor(i)(6) = Request.Form("cboPayTerm" & i)
            arySetVendor(i)(7) = Request.Form("hidCurrency" & i)
            arySetVendor(i)(8) = Request.Form("txtPurSpecNo" & i)
            arySetVendor(i)(9) = Request.Form("txtRevision" & i)
            If ViewState("GSTCOD") = True Then
                arySetVendor(i)(10) = ""
            Else
                arySetVendor(i)(10) = Request.Form("cboTax" & i)
            End If
            arySetVendor(i)(11) = Request.Form("txtLead" & i)
            arySetVendor(i)(12) = Request.Form("txtVenCode" & i)

            If arySetVendor(i)(1) <> "" Then
                found = found + 1
            End If
        Next

        If ViewState("GSTCOD") = True Then
            arySetVendor.Add(New String() {found & "th Alternative Vendor", found, Request.Form("txtVendor" & found), Request.Form("hidVendor" & found), Request.Form("cboSuppCode" & found), Request.Form("hidDelTerm" & found), Request.Form("cboPayTerm" & found), Request.Form("hidCurrency" & found), Request.Form("txtPurSpecNo" & found), Request.Form("txtRevision" & found), "", Request.Form("txtLead" & found + i), Request.Form("txtVenCode" & found + i), "Add"})
        Else
            arySetVendor.Add(New String() {found & "th Alternative Vendor", found, Request.Form("txtVendor" & found), Request.Form("hidVendor" & found), Request.Form("cboSuppCode" & found), Request.Form("hidDelTerm" & found), Request.Form("cboPayTerm" & found), Request.Form("hidCurrency" & found), Request.Form("txtPurSpecNo" & found), Request.Form("txtRevision" & found), Request.Form("cboTax" & found + i), Request.Form("txtLead" & found + i), Request.Form("txtVenCode" & found + i), "Add"})
        End If
        Session("arySetVendor") = Nothing
        Session("arySetVendor") = arySetVendor

    End Function

    Private Function validateData(ByRef strMsg As String) As Boolean
        Dim i, j As Integer
        Dim temp_VenCode, temp_Curr, strTemp As String
        Dim aryTempPrice As New ArrayList()
        Dim blnFound As Boolean
        Dim objDb As New EAD.DBCom

        validateData = True

        strMsg = "<ul type='disc'>"
        For i = 0 To arySetVendor.Count - 1
            blnFound = False

            If Request.Form("txtVendor" & i) <> "" Then
                If Session("aryPrice") Is Nothing Then
                    'strMsg &= "<li>" & arySetVendor(i)(0) & " must have at least 1 record of unit price<ul type='disc'></ul></li>"
                    'validateData = False
                    'blnFound = False
                Else
                    aryTempPrice = Session("aryPrice")
                    For j = 0 To aryTempPrice.Count - 1
                        If (aryTempPrice(j)(0) <> "" And aryTempPrice(j)(1) <> "") And arySetVendor(i)(1) = aryTempPrice(j)(3) And arySetVendor(i)(3) = aryTempPrice(j)(2) Then
                            blnFound = True
                            Exit For
                        End If
                    Next
                End If

                If blnFound = False Then
                    If objDb.FillDs("SELECT '*' FROM PRODUCT_VOLUME_PRICE " & _
                                    "INNER JOIN PRODUCT_MSTR ON PVP_PRODUCT_CODE = PM_PRODUCT_CODE " & _
                                    "INNER JOIN PIM_VENDOR ON PV_PRODUCT_INDEX = PM_PRODUCT_INDEX AND PV_VENDOR_TYPE = PVP_VENDOR_TYPE " & _
                                    "WHERE PM_S_COY_ID = '" & Session("CompanyId") & "' AND PV_S_COY_ID = '" & arySetVendor(i)(3) & "' AND PM_PRODUCT_CODE = '" & ViewState("strItemId") & "' AND PV_VENDOR_TYPE = '" & arySetVendor(i)(1) & "'").Tables(0).Rows.Count > 0 Then
                    Else
                        strMsg &= "<li>" & arySetVendor(i)(0) & " must have at least 1 record of unit price<ul type='disc'></ul></li>"
                        validateData = False
                    End If
                End If

                'Check for lead time whether it is numeric number
                If Request.Form("txtLead" & i) <> "" Then
                    If IsNumeric(Request.Form("txtLead" & i)) = False Then
                        intNumeric = False
                        strMsg &= "<li>" & arySetVendor(i)(0) & "Order Lead Time " & objGlobal.GetErrorMessage("00013") & "<ul type='disc'></ul></li>"
                        validateData = False
                    End If
                End If

                temp_VenCode = Request.Form("cboSuppCode" & i)
                temp_Curr = Request.Form("hidCurrency" & i)

                If temp_VenCode <> "" Then
                    If temp_Curr = "" Then
                        intNumeric = False
                        strMsg &= "<li>" & arySetVendor(i)(0) & "'s Currency " & objGlobal.GetErrorMessage("00001") & "<ul type='disc'></ul></li>"
                        validateData = False
                    End If
                End If

                If temp_Curr <> "" Then
                    If temp_VenCode = "" Then
                        intNumeric = False
                        strMsg &= "<li>" & arySetVendor(i)(0) & "'s Vendor Code " & objGlobal.GetErrorMessage("00001") & "<ul type='disc'></ul></li>"
                        validateData = False
                    End If
                End If

            End If
        Next

        'Check from table whether vendor code / currency is duplicated or not
        If validateData = True Then
            For i = 0 To arySetVendor.Count - 1
                If Request.Form("txtVendor" & i) <> "" And Request.Form("cboSuppCode" & i) <> "" And Request.Form("hidCurrency" & i) <> "" Then

                    Dim dsTemp As New DataSet
                    Dim dsTemp2 As New DataSet

                    strTemp = "SELECT PV_SUPP_CODE, PV_CURR FROM PIM_VENDOR PV LEFT JOIN PRODUCT_MSTR PM ON PV.PV_PRODUCT_INDEX = PM.PM_PRODUCT_INDEX " & _
                            "WHERE PM.PM_S_COY_ID = '" & Session("CompanyId") & "' AND PV_SUPP_CODE = '" & Common.Parse(Request.Form("cboSuppCode" & i)) & "' AND PV_S_COY_ID = '" & Request.Form("hidVendor" & i) & "' "
                    dsTemp = objDb.FillDs(strTemp)

                    If dsTemp.Tables(0).Rows.Count > 0 Then
                        If Request.Form("hidCurrency" & i) <> dsTemp.Tables(0).Rows(0)("PV_CURR") Then
                            strMsg &= "<li>" & arySetVendor(i)(0) & "'s Vendor Code " & objGlobal.GetErrorMessage("00283") & "<ul type='disc'></ul></li>"
                            validateData = False
                        End If
                    Else
                        strTemp = "SELECT PV_SUPP_CODE, PV_CURR FROM PIM_VENDOR PV LEFT JOIN PRODUCT_MSTR PM ON PV.PV_PRODUCT_INDEX = PM.PM_PRODUCT_INDEX " & _
                                "WHERE PM.PM_S_COY_ID = '" & Session("CompanyId") & "' AND PV_CURR = '" & Request.Form("hidCurrency" & i) & "' AND PV_S_COY_ID = '" & Request.Form("hidVendor" & i) & "' "
                        dsTemp2 = objDb.FillDs(strTemp)

                        If dsTemp2.Tables(0).Rows.Count > 0 Then
                            'strMsg &= "<li>" & arySetVendor(i)(0) & "'s Currency Code " & objGlobal.GetErrorMessage("00283") & "<ul type='disc'></ul></li>"
                            strMsg &= "<li>" & arySetVendor(i)(0) & "'s Vendor Code is not match to Vendor Code for the same Currency existed in system.<ul type='disc'></ul></li>"
                            validateData = False
                        End If
                    End If
                End If
            Next
        End If

        'Compare vendor code / currency with other rows
        If validateData = True Then
            For i = 0 To arySetVendor.Count - 1
                If Request.Form("txtVendor" & i) <> "" Then
                    For j = 0 To arySetVendor.Count - 1
                        If i <> j And Request.Form("txtVendor" & i) = Request.Form("txtVendor" & j) And Request.Form("cboSuppCode" & i) = Request.Form("cboSuppCode" & j) Then
                            If Request.Form("hidCurrency" & i) <> Request.Form("hidCurrency" & j) Then
                                strMsg &= "<li>" & objGlobal.GetErrorMessage("00345") & "<ul type='disc'></ul></li>"
                                validateData = False
                                Exit For
                            End If
                        ElseIf i <> j And Request.Form("txtVendor" & i) = Request.Form("txtVendor" & j) And Request.Form("cboSuppCode" & i) <> Request.Form("cboSuppCode" & j) Then
                            If Request.Form("hidCurrency" & i) = Request.Form("hidCurrency" & j) Then
                                strMsg &= "<li>" & objGlobal.GetErrorMessage("00345") & "<ul type='disc'></ul></li>"
                                validateData = False
                                Exit For
                            End If
                        End If
                    Next
                End If
            Next
        End If

        'Check For IQC Test Type
        If rd2.SelectedItem.Value = "Y" And ddlIQCType.SelectedItem.Value = "" Then
            strMsg &= "<li>" & objGlobal.GetErrorMessage("00343") & "<ul type='disc'></ul></li>"
            validateData = False
        End If

        'Check for Account Code
        If ddlAccCode.SelectedValue = "" And rd1.SelectedValue <> "SP" Then
            strMsg &= "<li>" & objGlobal.GetErrorMessage("00344") & "<ul type='disc'></ul></li>"
            validateData = False
        End If

        strMsg &= "</ul>"
    End Function

    Sub DisplayUserCheckBtn()
        Dim objUser As New Users_Ext
        Dim dsStk As New DataSet

        dsStk = objUser.GetUserStockType(Session("UserId"), Session("CompanyId"))

        If dsStk.Tables(0).Rows(0)("UM_STK_TYPE_SPOT") = "N" Then
            rd1.Items.FindByValue("SP").Enabled = False
        Else
            rd1.Items.FindByValue("SP").Selected = True
        End If

        If dsStk.Tables(0).Rows(0)("UM_STK_TYPE_STOCK") = "N" Then
            rd1.Items.FindByValue("ST").Enabled = False
        Else
            If rd1.Items.FindByValue("SP").Enabled = False Then
                rd1.Items.FindByValue("ST").Selected = True
            End If
        End If

        If dsStk.Tables(0).Rows(0)("UM_STK_TYPE_MRO") = "N" Then
            rd1.Items.FindByValue("MI").Enabled = False
        Else
            If rd1.Items.FindByValue("SP").Enabled = False And rd1.Items.FindByValue("ST").Enabled = False Then
                rd1.Items.FindByValue("MI").Selected = True
            End If
        End If

    End Sub

    Public Function BindIQCTestType()
        Dim cbolist As New ListItem
        Dim objAdmin As New Admin
        Dim dvCustom As DataView

        dvCustom = objAdmin.getIQCTestType()

        ddlIQCType.Items.Clear()
        If Not dvCustom Is Nothing Then
            Common.FillDdl(ddlIQCType, "CPA_PARAM_LABEL", "CPA_PARAM_LABEL", dvCustom)
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        ddlIQCType.Items.Insert(0, cbolist)

    End Function

    Private Sub hidBtn_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidBtn.ServerClick
        Dim POFound, PRFound, RFQFound As Boolean

        If ViewState("mode") = "mod" Then
            If txtVendorItemCode.Text <> hidVendorItemCode.Value Then
                If txtVendorItemCode.Text <> "" Then
                    If txtVendorItemCode.Text <> hidVendorItemCode.Value Then
                        If objCat_Ext.checkItemCodeDup(txtVendorItemCode.Text) = True Then
                            Common.NetMsgbox(Me, "Duplicate item code found.", dDispatcher.direct("BuyerCat", "SearchBItem.aspx", "pageid=" & strPageId))
                        End If
                    End If
                End If
            End If
        Else
            If txtVendorItemCode.Text <> "" Then
                If objCat_Ext.checkItemCodeDup(txtVendorItemCode.Text) = True Then
                    Common.NetMsgbox(Me, "Duplicate item code found.", dDispatcher.direct("BuyerCat", "SearchBItem.aspx", "pageid=" & strPageId))
                End If
            End If
        End If

    End Sub

    Private Sub hidBtn2_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidBtn2.ServerClick
        Dim i As Integer
        Dim strDefPayTerm As String
        Dim objAdmin_ext As New Admin_Ext
        arySetVendor = Session("arySetVendor")
        For i = 0 To arySetVendor.Count - 1
            arySetVendor(i)(2) = Request.Form("txtVendor" & i)
            arySetVendor(i)(3) = IIf(Request.Form("txtVendor" & i) = "", "", Request.Form("hidVendor" & i))
            arySetVendor(i)(4) = Request.Form("cboSuppCode" & i)
            arySetVendor(i)(5) = Request.Form("hidDelTerm" & i)
            strDefPayTerm = objAdmin_ext.getVenDefaultPayTerm(Request.QueryString("itemid"), i, arySetVendor(i)(3))
            If strDefPayTerm <> "" Then
                arySetVendor(i)(6) = strDefPayTerm
            Else
                arySetVendor(i)(6) = Request.Form("cboPayTerm" & i)
            End If
            arySetVendor(i)(7) = Request.Form("hidCurrency" & i)
            arySetVendor(i)(8) = Request.Form("txtPurSpecNo" & i)
            arySetVendor(i)(9) = Request.Form("txtRevision" & i)
            If ViewState("GSTCOD") = True Then
                arySetVendor(i)(10) = ""
            Else
                arySetVendor(i)(10) = Request.Form("cboTax" & i)
            End If
            arySetVendor(i)(11) = Request.Form("txtLead" & i)
            arySetVendor(i)(12) = Request.Form("txtVenCode" & i)
        Next

        'Session("arySetVendor") = Nothing
        Session("arySetVendor") = arySetVendor

        ConstructTable(, arySetVendor)
        PopulateTypeAhead()
    End Sub
End Class
