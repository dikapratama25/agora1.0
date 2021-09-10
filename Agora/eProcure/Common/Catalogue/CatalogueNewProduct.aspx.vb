Imports AgoraLegacy
Imports eProcure.Component


Public Class CatalogueNewProduct
    Inherits AgoraLegacy.AppBaseClass
    Dim objCat As New ContCat
  
    Dim objGlobal As New AppGlobals
    Dim objFile As New FileManagement
    Dim blnDiscount As Boolean
    Dim strMsg As String
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image

    'Protected WithEvents txtCategory As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVendorItemCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtItemName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRefNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCommodityType As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPrice As System.Web.UI.WebControls.TextBox
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
    Protected WithEvents hidresult As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents lblAttach As System.Web.UI.WebControls.Label
    Protected WithEvents lblAttach2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblTax As System.Web.UI.WebControls.Label

    Protected WithEvents hidProductCode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidVendorItemCode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidCurrentCommodityType As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidItemName As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidRefNo As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidItemDesc As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidCommodityType As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidTax As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidUOM As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidPrice As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidCurrencyCode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidImageAttached As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidBrand As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidModel As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidDrawingNumber As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidVersionNo As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidGrossWeight As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidNetWeight As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidLength As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidWidth As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidHeight As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidVolume As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidColorInfo As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidHSCode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidPacking As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidImageFile As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidMgmtCode As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidMgmtText As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidPostBack As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidRemarks As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents cboGSTRate As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents cboTax As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboUOM As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboCurrencyCode As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents cboMgmt As System.Web.UI.WebControls.DropDownList

    Protected WithEvents vldVItemCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldItemName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldCommodityType As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldTax As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldUOM As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldCurrencyCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    'Protected WithEvents vldDesc As System.Web.UI.WebControls.RequiredFieldValidator

    Protected WithEvents cmdSelect As System.Web.UI.WebControls.Button
    Protected WithEvents cmdUploadImage As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents cmdUpload As System.Web.UI.WebControls.Button
    Protected WithEvents cmdPreview As System.Web.UI.WebControls.Button
    Protected WithEvents btnHidden As System.Web.UI.WebControls.Button

    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents FileProImage As System.Web.UI.HtmlControls.HtmlInputFile

    Protected WithEvents pnlAttach As System.Web.UI.WebControls.Panel
    Protected WithEvents File1 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents pnlImage As System.Web.UI.WebControls.Panel
    '    Protected WithEvents txtCatPrice As System.Web.UI.WebControls.TextBox
    Protected WithEvents revPrice As System.Web.UI.WebControls.RegularExpressionValidator
    Dim blnList As Boolean
    Dim strItemId As String
    Dim strMode As String
    Dim dDispatcher = New dispatcher

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
        If ViewState("mode") = "add" Then
            alButtonList = New ArrayList
            alButtonList.Add(cmdSave)
            alButtonList.Add(cmdUpload)
            alButtonList.Add(cmdUploadImage)
            htPageAccess.Add("add", alButtonList)

        ElseIf ViewState("mode") = "mod" Then
            alButtonList = New ArrayList
            alButtonList.Add(cmdSave)
            alButtonList.Add(cmdUpload)
            alButtonList.Add(cmdUploadImage)
            htPageAccess.Add("update", alButtonList)
        End If
        CheckButtonAccess()
        cmdSave.Enabled = (blnCanAdd Or blnCanUpdate) And ViewState("blnCmdSubmit")
        cmdUpload.Enabled = (blnCanAdd Or blnCanUpdate) And ViewState("blnCmdUploadDoc")
        cmdUploadImage.Enabled = (blnCanAdd Or blnCanUpdate) And ViewState("blnCmdUploadImg")
        cmdReset.Disabled = Not (blnCanAdd Or blnCanUpdate)
        alButtonList.Clear()
        CheckPending()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnPaging = False
        blnSorting = False
        'SetGridProperty(dtgCatalogue)

        If Not IsPostBack Then
            Dim objGst As New GST
            ViewState("GSTCOD") = objGst.chkGSTCOD()
            strMode = Me.Request.QueryString("mode")
            ViewState("mode") = strMode
            isNotPostBack()
            Image1.ImageUrl = dDispatcher.direct("Plugins/Images", "collapse_up.gif")
        End If
        displayImageFile(Session("ImageIndex"))
        displayAttachFile()
        'lnkBack.NavigateUrl = "ListPriceCatalogue.aspx?pageid=" & strPageId
        lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "ListPriceCatalogue.aspx", "pageid=" & strPageId)
        cmdUploadImage.Attributes.Add("onclick", "return checkDocFile('img','" & FileProImage.ClientID & "');")
        cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & File1.ClientID & "');")

        txtRemarks.Attributes.Add("onKeyDown", "limitText (this, 3000);")

        'cmdSave.Attributes.Add("onClick", "return resetSummary(0,1);")
    End Sub

    Private Sub isNotPostBack()
        Dim objGst As New GST
        Dim lstItem As New ListItem
        Dim objDb As New EAD.DBCom
        ViewState("GST") = objDb.GetVal("SELECT IFNULL(CM_TAX_REG_NO,'') FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Session("CompanyId") & "'")
        objDb = Nothing

        vldSumm.ShowSummary = False
        lblMsg.Text = ""
        ViewState("blnCmdSubmit") = True
        ViewState("blnCmdUploadDoc") = True
        ViewState("blnCmdUploadImg") = True
        'Session.Add("ImageIndex", "")
        'ViewState("mode") = Request.QueryString("mode")
        'objGlobal.FillCodeTable(cboTax, CodeTable.Gst)
        If ViewState("GSTCOD") = True Then
            If ViewState("GST") <> "" Then
                objGlobal.FillGST(cboGSTRate, False)
                cboGSTRate.Enabled = True
            Else
                lstItem.Value = "N/A"
                lstItem.Text = "N/A"
                cboGSTRate.Items.Insert(0, lstItem)
                cboGSTRate.Enabled = False
            End If
            lblTax.Text = "SST Rate"
            vldTax.ErrorMessage = "SST Rate is required."
        Else
            objGlobal.FillTax(cboGSTRate)
            lblTax.Text = "Tax"
            vldTax.ErrorMessage = "Tax is required."
        End If

        objGlobal.FillCodeTable(cboUOM, CodeTable.Uom)
        objGlobal.FillCodeTable(cboCurrencyCode, CodeTable.Currency)

        ' objGlobal.FillCodeTable(cboMgmt, CodeTable.MgmtCode)
        If ViewState("mode") = "mod" Then
            'lblTitle.Text = "Modify Item"
            ViewState("itemid") = Request.QueryString("itemid")
            ViewState("itemindex") = Request.QueryString("itemindex")
            Session("ImageIndex") = objCat.copyAttachFile(ViewState("itemid"))
            'lblDiscGrp.Text = "To remove the assigned group, unselect the Discount Group Code."
            displayData()
            'lblHeader.Text = "Modify the required fields and Click Submit"
            cmdReset.Value = "Reset"
        Else
            'lblTitle.Text = "Add New Item"
            'lblItemId.Text = "To Be Allocated By System"
            'txtCategory.Text = "80801001"
            ViewState("itemid") = ""
            'trDiscGrp.Visible = False
            'lblHeader.Text = "Fill in the required fields and Click Submit"
            cmdReset.Value = "Clear"
            clearTextBox()
        End If
        'Bindgrid()
        hidPostBack.Value = "0"
        objCat.deleteTempAttachment(ViewState("itemid"), Session.SessionID)
    End Sub
    Private Sub CheckPending()
        If ViewState("mode") = "mod" And objCat.WithPendingRec(ViewState("itemid")) Then
            cmdSave.Enabled = False
        End If
    End Sub
    Private Sub clearTextBox()
        Me.txtVendorItemCode.Text = ""
        Me.txtItemName.Text = ""
        Me.txtRefNo.Text = ""
        Me.txtDesc.Text = ""
        Me.txtCommodityType.Text = ""
        Me.txtPrice.Text = ""
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

    End Sub

    Private Function displayData()
        Dim ds As New DataSet
        ds = objCat.getSingleProduct(ViewState("itemid"), False)
        If ds.Tables(0).Rows.Count > 0 Then
            'lblItemId.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_CODE"))
            strItemId = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_CODE"))
            hidProductCode.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_CODE"))
            txtVendorItemCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_VENDOR_ITEM_CODE"))
            hidVendorItemCode.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_VENDOR_ITEM_CODE"))
            txtItemName.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_DESC"))
            hidItemName.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_DESC"))
            txtRefNo.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_REF_NO"))
            hidRefNo.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_REF_NO"))
            txtDesc.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_LONG_DESC"))
            hidItemDesc.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_LONG_DESC"))
            txtCommodityType.Text = Common.parseNull(ds.Tables(0).Rows(0)("CT_NAME"))
            hidCommodityType.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_CATEGORY_NAME"))
            hidCurrentCommodityType.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_CATEGORY_NAME"))

            'GST Enhancement
            If ViewState("GSTCOD") = True Then
                Common.SelDdl(Common.parseNull(CStr(ds.Tables(0).Rows(0)("PM_GST_CODE"))), cboGSTRate, True, True)
                hidTax.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_GST_CODE"))
            Else
                Common.SelDdl(Common.parseNull(CStr(ds.Tables(0).Rows(0)("PM_TAX_ID"))), cboGSTRate, True, True)
                hidTax.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_TAX_ID"))
            End If

            Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("PM_UOM")), cboUOM, False, True)
            hidUOM.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_UOM"))

            If Not IsDBNull(ds.Tables(0).Rows(0)("PM_UNIT_COST")) Then
                txtPrice.Text = Format(CDbl(Common.parseNull(ds.Tables(0).Rows(0)("PM_UNIT_COST"))), "#0.0000")
            Else
                txtPrice.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_UNIT_COST"))
            End If
            hidPrice.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_UNIT_COST"))

            Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("PM_CURRENCY_CODE")), cboCurrencyCode, True, True)
            hidCurrencyCode.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_CURRENCY_CODE"))

            txtBrand.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_BRAND"))
            hidBrand.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_BRAND"))
            txtModel.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_MODEL"))
            hidModel.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_MODEL"))

            txtDrawingNumber.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_DRAW_NO"))
            hidDrawingNumber.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_DRAW_NO"))

            txtVersionNo.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_VERS_NO"))
            hidVersionNo.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_VERS_NO"))

            txtGrossWeight.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_GROSS_WEIGHT"))
            hidGrossWeight.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_GROSS_WEIGHT"))

            txtNetWeight.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_NET_WEIGHT"))
            hidNetWeight.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_NET_WEIGHT"))

            txtLength.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_LENGHT"))
            hidLength.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_LENGHT"))

            txtWidth.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_WIDTH"))
            hidWidth.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_WIDTH"))

            txtHeight.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_HEIGHT"))
            hidHeight.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_HEIGHT"))

            txtVolume.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_VOLUME"))
            hidVolume.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_VOLUME"))

            txtColorInfo.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_COLOR_INFO"))
            hidColorInfo.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_COLOR_INFO"))

            txtHSCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_HSC_CODE"))
            hidHSCode.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_HSC_CODE"))

            txtPacking.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_PACKING_REQ"))
            hidPacking.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_PACKING_REQ"))

            txtRemarks.Text = Common.parseNull(ds.Tables(0).Rows(0)("PM_REMARKS"))
            hidRemarks.Value = Common.parseNull(ds.Tables(0).Rows(0)("PM_REMARKS"))

            ' display attached files
            Session("ImageIndex") = Common.parseNull(ds.Tables(0).Rows(0)("PM_PRODUCT_IMAGE"))
            ViewState("blnTemp") = False
            displayAttachFile()
            displayImageFile(Session("ImageIndex"))
        End If
    End Function

    'Private Function Bindgrid() As String
    '    Dim ds As New DataSet
    '    ds = objCat.getDiscountGroupList(viewstate("itemid"), "D", Session("CompanyId"))
    '    intPageRecordCnt = ds.Tables(0).Rows.Count
    '    viewstate("intPageRecordCnt") = intPageRecordCnt

    '    Dim dvViewSample As DataView
    '    dvViewSample = ds.Tables(0).DefaultView
    '    dvViewSample.Sort = viewstate("SortExpression")
    '    If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

    '    If intPageRecordCnt > 0 Then
    '        dtgCatalogue.DataSource = dvViewSample
    '        dtgCatalogue.DataBind()
    '    Else
    '        dtgCatalogue.DataBind()
    '    End If
    'End Function

    Private Sub displayImageFile(ByVal strIndex As String)
        Dim dsImage As New DataSet
        Dim drvImage As DataView
        Dim blnTemp As Boolean
        Dim i As Integer
        Dim strDocNo As String
        Dim strFile, strFile1, strURL, strTemp, strCoyId, strBasePath, strDownloadPath, strFilePath, strDisplayPath As String

        If ViewState("mode") = "mod" Then
            blnTemp = False
            strDocNo = ViewState("itemid")
        Else
            strDocNo = Session.SessionID
            blnTemp = True
        End If
        'dsImage = objCat.getTempImageAttach(strIndex, viewstate("blnTemp"))  ' True)
        dsImage = objCat.getTempImageAttach(strIndex, blnTemp, strDocNo)

        drvImage = dsImage.Tables(0).DefaultView

        'Michelle (8/5/2011) 
        'If drvImage.Count = 0 Then
        '    dsImage = objCat.getTempImageAttach(strIndex, True)
        '    drvImage = dsImage.Tables(0).DefaultView
        'End If

        pnlImage.Controls.Clear()
        If drvImage.Count > 0 Then
            For i = 0 To drvImage.Count - 1
                strFile = drvImage(i)("PA_ATTACH_FILENAME")
                strFile1 = drvImage(i)("PA_HUB_FILENAME")

                'strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.ProductImage & ">" & strFile & "</A>"
                'Dim objFile As New Wheel.Components.FileManagement
                '//To get fullpath file name
                hidImageFile.Value = objFile.FileDownload(Server.UrlEncode(strFile1), EnumDownLoadType.ProductImage, "", EnumUploadFrom.FrontOff)
                strURL = "<A HREF='#' onclick='PreviewImage(1);'>" & strFile & "</A>"
                Dim lblBr As New Label
                Dim lblFile As New Label
                Dim lnk As New ImageButton
                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL '& " (" & drvImage(i)("PA_FILESIZE") & "KB) "
                'lblBr.Text = "<BR>"
                'lnk.ImageUrl = "../images/i_delete2.gif"
                lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete2.gif")

                lnk.ID = drvImage(i)("PA_ATTACH_INDEX")
                lnk.CausesValidation = False
                AddHandler lnk.Click, AddressOf deleteImageAttach

                pnlImage.Controls.Add(lblFile)
                pnlImage.Controls.Add(lnk)
                pnlImage.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlImage.Controls.Add(lblFile)
        End If
    End Sub

    Private Sub displayAttachFile()
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String
        If ViewState("mode") = "add" Then
            dsAttach = objCat.getTempAttach(Session.SessionID, True)
        Else
            dsAttach = objCat.getTempAttach(ViewState("itemid"), True)
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
                'lnk.ImageUrl = "../images/i_delete2.gif"
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
        'objCat.deleteAttachment(CType(sender, ImageButton).ID, True, , "V")
        Dim strDocNo As String
        If ViewState("mode") = "mod" Then
            strDocNo = ViewState("itemid")
        Else
            strDocNo = Session.SessionID
        End If
        objCat.deleteAttachment(CType(sender, ImageButton).ID, True, , "V", "I", strDocNo)
        displayImageFile(Session("ImageIndex"))
    End Sub

    Private Sub deleteAttach(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'objCat.deleteAttachment(CType(sender, ImageButton).ID, True, , "V")
        Dim strDocNo As String
        If ViewState("mode") = "mod" Then
            strDocNo = ViewState("itemid")
        Else
            strDocNo = Session.SessionID
        End If

        objCat.deleteAttachment(CType(sender, ImageButton).ID, True, , "V", "D", strDocNo)
        displayAttachFile()
    End Sub

    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        If File1.Value <> "" Then
            Dim objFile As New FileManagement
            Dim strDocNo As String
            If ViewState("mode") = "mod" Then
                strDocNo = ViewState("itemid")
            Else
                strDocNo = Session.SessionID
            End If

            Dim objDB As New EAD.DBCom
            Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(File1.PostedFile.FileName)

            If Len(sFileName) > 50 Then
                Common.NetMsgbox(Me, "File name exceeds 46 character")
            ElseIf File1.PostedFile.ContentLength > 0 And File1.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                objFile.FileUpload(File1, EnumUploadType.ProductAttachment, "", EnumUploadFrom.FrontOff, strDocNo, True)
                'Select Case viewstate("mode")
                '    Case "add"
                '        objFile.FileUpload(File1, EnumUploadType.ProductAttachment, "", EnumUploadFrom.FrontOff, strDocNo, True)
                '    Case "mod"
                '        objFile.FileUpload(File1, EnumUploadType.ProductAttachment, "", EnumUploadFrom.FrontOff, viewstate("itemid"), True)
                '        'objFile.FileUpload(File1, EnumUploadType.ProductAttachment, "", EnumUploadFrom.BackOff, viewstate("itemid"), True)
                'End Select
            ElseIf File1.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            displayAttachFile()
            displayImageFile(Session("ImageIndex"))
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
            Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'ImageAttachSize'"))

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(FileProImage.PostedFile.FileName)

            If Len(sFileName) > 50 Then
                Common.NetMsgbox(Me, "File name exceeds 46 character")
            ElseIf FileProImage.PostedFile.ContentLength > 0 And FileProImage.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                Session("ImageIndex") = objFile.FileUpload(FileProImage, EnumUploadType.ProductImage, "", EnumUploadFrom.FrontOff, strDocNo, True, Session("ImageIndex"))
            ElseIf FileProImage.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            ViewState("blnTemp") = True
            'Session("ImageIndex") = objFile.FileUpload(FileProImage, EnumUploadType.ProductImage, "", EnumUploadFrom.FrontOff, strDocNo, blnTemp, Session("ImageIndex"))
            displayImageFile(Session("ImageIndex"))
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
        dtr("ProductCode") = strItemId 'lblItemId.Text 'PM_PRODUCT_CODE
        dtr("FieldName") = strField
        dtr("Value") = strValue
        dtProduct.Rows.Add(dtr)
    End Function

    Private Function getChangesUpdate(ByRef bln As Boolean) As DataTable
        bln = False
        Dim dtProduct As New DataTable
        dtProduct.Columns.Add("ProductIndex", Type.GetType("System.String"))
        dtProduct.Columns.Add("ProductCode", Type.GetType("System.String"))
        dtProduct.Columns.Add("FieldName", Type.GetType("System.String"))
        dtProduct.Columns.Add("Value", Type.GetType("System.String"))

        If UCase(hidCommodityType.Value) <> UCase(hidCurrentCommodityType.Value) Then
            insertRow(dtProduct, "PM_CATEGORY_NAME", hidCommodityType.Value)
            bln = True
        End If
        If UCase(txtItemName.Text) <> UCase(hidItemName.Value) Then
            insertRow(dtProduct, "PM_PRODUCT_DESC", txtItemName.Text)
            bln = True
        End If
        If UCase(txtRefNo.Text) <> UCase(hidRefNo.Value) Then
            insertRow(dtProduct, "PM_REF_NO", txtRefNo.Text)
            bln = True
        End If
        If UCase(txtVendorItemCode.Text) <> UCase(hidVendorItemCode.Value) Then
            insertRow(dtProduct, "PM_VENDOR_ITEM_CODE", txtVendorItemCode.Text)
            bln = True
        End If
        If UCase(txtDesc.Text) <> UCase(hidItemDesc.Value) Then
            insertRow(dtProduct, "PM_LONG_DESC", txtDesc.Text)
            bln = True
        End If
        If UCase(txtBrand.Text) <> UCase(hidBrand.Value) Then
            insertRow(dtProduct, "PM_PRODUCT_BRAND", txtBrand.Text)
            bln = True
        End If
        If UCase(txtModel.Text) <> UCase(hidModel.Value) Then
            insertRow(dtProduct, "PM_PRODUCT_MODEL", txtModel.Text)
            bln = True
        End If
        If UCase(cboUOM.SelectedItem.Text) <> UCase(hidUOM.Value) Then
            insertRow(dtProduct, "PM_UOM", cboUOM.SelectedItem.Text)
            bln = True
        End If
        If UCase(cboCurrencyCode.SelectedItem.Text) <> UCase(hidCurrencyCode.Value) Then
            insertRow(dtProduct, "PM_CURRENCY_CODE", cboCurrencyCode.SelectedItem.Text)
            bln = True
        End If
        If ViewState("GSTCOD") = True Then
            If UCase(cboGSTRate.SelectedItem.Value) <> UCase(hidTax.Value) Then
                insertRow(dtProduct, "PM_GST_CODE", cboGSTRate.SelectedItem.Value)
                bln = True
            End If
        Else
            If UCase(cboGSTRate.SelectedItem.Text) <> UCase(hidTax.Value) Then
                insertRow(dtProduct, "PM_TAX_ID", cboGSTRate.SelectedItem.Text)
                bln = True
            End If
        End If
        If UCase(txtPrice.Text) <> UCase(hidPrice.Value) Then
            insertRow(dtProduct, "PM_UNIT_COST", txtPrice.Text)
            bln = True
        End If
        If UCase(txtDrawingNumber.Text) <> UCase(hidDrawingNumber.Value) Then
            insertRow(dtProduct, "PM_DRAW_NO", txtDrawingNumber.Text)
            bln = True
        End If
        If UCase(txtVersionNo.Text) <> UCase(hidVersionNo.Value) Then
            insertRow(dtProduct, "PM_VERS_NO", txtVersionNo.Text)
            bln = True
        End If
        If UCase(txtGrossWeight.Text) <> UCase(hidGrossWeight.Value) Then
            insertRow(dtProduct, "PM_GROSS_WEIGHT", txtGrossWeight.Text)
            bln = True
        End If
        If UCase(txtNetWeight.Text) <> UCase(hidNetWeight.Value) Then
            insertRow(dtProduct, "PM_NET_WEIGHT", txtNetWeight.Text)
            bln = True
        End If

        If UCase(txtLength.Text) <> UCase(hidLength.Value) Then
            insertRow(dtProduct, "PM_LENGHT", txtLength.Text)
            bln = True
        End If

        If UCase(txtWidth.Text) <> UCase(hidWidth.Value) Then
            insertRow(dtProduct, "PM_WIDTH", txtWidth.Text)
            bln = True
        End If

        If UCase(txtHeight.Text) <> UCase(hidHeight.Value) Then
            insertRow(dtProduct, "PM_HEIGHT", txtHeight.Text)
            bln = True
        End If

        If UCase(txtVolume.Text) <> UCase(hidVolume.Value) Then
            insertRow(dtProduct, "PM_VOLUME", txtVolume.Text)
            bln = True
        End If
        If UCase(txtColorInfo.Text) <> UCase(hidColorInfo.Value) Then
            insertRow(dtProduct, "PM_COLOR_INFO", txtColorInfo.Text)
            bln = True
        End If
        If UCase(txtHSCode.Text) <> UCase(hidHSCode.Value) Then
            insertRow(dtProduct, "PM_HSC_CODE", txtHSCode.Text)
            bln = True
        End If

        If UCase(txtPacking.Text) <> UCase(hidPacking.Value) Then
            insertRow(dtProduct, "PM_PACKING_REQ", txtPacking.Text)
            bln = True
        End If

        If UCase(txtRemarks.Text) <> UCase(hidRemarks.Value) Then
            insertRow(dtProduct, "PM_REMARKS", txtRemarks.Text)
            bln = True
        End If

        getChangesUpdate = dtProduct
    End Function

    Private Function validateInputs() As Boolean

        If Me.txtRemarks.Text.Length > Me.txtRemarks.MaxLength Then
            strMsg &= "<ul type='disc'><li>" & "Item remark should be less than " & Me.txtRemarks.MaxLength & " characters." & "</li></ul>"
            Return False
        End If

        Return True
    End Function

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        vldSumm.ShowSummary = True
        'Dim strMsg As String
        If Page.IsValid And validateInputs() Then
            If ViewState("mode") = "mod" Then
                'Dim strMsg As String
                Dim strscript As New System.Text.StringBuilder
                strMsg = "This item will be submitted to Hub Admin for Approval.\n\n"
                strMsg &= "Important Note: You are not allowed to make changes or modification until Hub Admin approval is completed.\n\n"
                strMsg &= "Do you want to proceed?"
                strscript.Append("<script language=""javascript"">")
                strscript.Append("PromptMsg('" & strMsg & "');")
                strscript.Append("document.getElementById('btnHidden').click();")
                strscript.Append("</script>")
                RegisterStartupScript("script1", strscript.ToString())
            Else
                Submit()
            End If
        Else
            If strMsg <> "" Then
                lblMsg.Text = strMsg
            Else
                lblMsg.Text = ""
            End If
        End If
    End Sub
    Public Sub btnHidden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHidden.Click
        If hidresult.Value = "1" Then Submit()
    End Sub

    Private Sub Submit()
        vldSumm.ShowSummary = True
        Dim strMsg As String
        Dim objFile As New FileManagement
        Dim blnSend As Boolean
        Dim strNewCode As String
        Dim dsProduct As New DataSet
        Dim dtProduct As New DataTable
        Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
        blnList = False
        blnDiscount = False

        ' If Page.IsValid And validateDatagrid(strMsg) Then
        ''If Page.IsValid Then
        lblMsg.Text = ""
        dsProduct = bindProduct()
        If ViewState("mode") = "add" Then

            If objDb.Get1Column("product_mstr_temp", "PM_PRODUCT_INDEX", " WHERE PM_VENDOR_ITEM_CODE = '" & Replace(dsProduct.Tables(0).Rows(0)("VendorItemCode"), "'", "''") & "' AND PM_S_COY_ID = '" & dsProduct.Tables(0).Rows(0)("CoyId") & "'") = "" And objDb.Get1Column("product_mstr", "PM_PRODUCT_INDEX", " WHERE PM_VENDOR_ITEM_CODE = '" & Replace(dsProduct.Tables(0).Rows(0)("VendorItemCode"), "'", "''") & "' AND PM_S_COY_ID = '" & dsProduct.Tables(0).Rows(0)("CoyId") & "'") = "" Then
                strNewCode = objCat.insertProductTemp(dsProduct, Session("ImageIndex"))
                'Michelle (25/10/2010) - Change the error message
                'Common.NetMsgbox(Me, "The List Price Item Code :  " & Server.UrlEncode(strNewCode) & " has been submitted for Approval From Hub Admin.")
                'Common.NetMsgbox(Me, "This item has been submitted to Hub Admin for approval.", "ListPriceCatalogue.aspx?pageid=" & strPageId)
                Common.NetMsgbox(Me, "This item has been submitted to Hub Admin for approval.", dDispatcher.direct("Catalogue", "ListPriceCatalogue.aspx", "pageid=" & strPageId))

                strMode = "mod"
                ViewState("mode") = strMode
                'Me.cmdSave.Enabled = False
                'Me.cmdReset.Disabled = True
                'Response.Redirect("ListPriceCatalogue.aspx?pageid=" & strPageId)
            Else
                Common.NetMsgbox(Me, "Duplicate Vendor Item Code.", MsgBoxStyle.Critical)
            End If
        Else
            dtProduct = getChangesUpdate(blnSend)
            dsProduct.Tables.Add(dtProduct)


            If UCase(txtVendorItemCode.Text) <> UCase(hidVendorItemCode.Value) Then
                If objDb.Get1Column("product_mstr_temp", "PM_PRODUCT_INDEX", " WHERE PM_VENDOR_ITEM_CODE = '" & dsProduct.Tables(0).Rows(0)("VendorItemCode") & "' AND PM_S_COY_ID = '" & dsProduct.Tables(0).Rows(0)("CoyId") & "'") = "" And objDb.Get1Column("product_mstr", "PM_PRODUCT_INDEX", " WHERE PM_VENDOR_ITEM_CODE = '" & dsProduct.Tables(0).Rows(0)("VendorItemCode") & "' AND PM_S_COY_ID = '" & dsProduct.Tables(0).Rows(0)("CoyId") & "'") = "" Then
                    If Session("ImageIndex") = "0" Then Session("ImageIndex") = ""
                    If txtPrice.Text <> hidPrice.Value Then
                        objCat.updateListPrice(Session("CompanyId"), dsProduct, blnSend, txtPrice.Text, Session("ImageIndex"), blnList, False)
                    Else
                        'objCat.updateListPrice(Session("CompanyId"), dsProduct, blnSend, "", Session("ImageIndex"), blnList, False)
                        objCat.updateListPrice(Session("CompanyId"), dsProduct, blnSend, txtPrice.Text, Session("ImageIndex"), blnList, False)
                    End If
                    Session("ImageIndex") = ""
                    If blnSend Then
                        ''Response.Redirect("CatalogueConfirm.aspx?pageid=" & strPageId & "&role=L&act=0&code=" & Server.UrlEncode(lblItemId.Text))
                        'Response.Redirect("CatalogueConfirm.aspx?pageid=" & strPageId & "&role=L&act=0&code=" & Server.UrlEncode(strItemId))
                        'Common.NetMsgbox(Me, "The Item has been submitted for Approval From Hub Admin.", "ListPriceCatalogue.aspx?pageid=" & strPageId)
                        Common.NetMsgbox(Me, "The Item has been submitted for Approval From Hub Admin.", dDispatcher.direct("Catalogue", "ListPriceCatalogue.aspx", "pageid=" & strPageId))
                        'Response.Redirect("ListPriceCatalogue.aspx?pageid=" & strPageId)
                        'Me.cmdSave.Enabled = False
                        'Me.cmdReset.Enabled = False
                    Else
                        'Common.NetMsgbox(Me, MsgRecordSave, "ListPriceCatalogue.aspx?pageid=" & strPageId, MsgBoxStyle.Information)
                        Common.NetMsgbox(Me, MsgRecordSave, dDispatcher.direct("Catalogue", "ListPriceCatalogue.aspx", "pageid=" & strPageId, MsgBoxStyle.Information))
                    End If
                Else
                    Common.NetMsgbox(Me, "Duplicate Vendor Item Code.", MsgBoxStyle.Critical)
                End If
            Else
                If Session("ImageIndex") = "0" Then Session("ImageIndex") = ""
                If txtPrice.Text <> hidPrice.Value Then
                    objCat.updateListPrice(Session("CompanyId"), dsProduct, blnSend, txtPrice.Text, Session("ImageIndex"), blnList, False)
                Else
                    'objCat.updateListPrice(Session("CompanyId"), dsProduct, blnSend, "", Session("ImageIndex"), blnList, False)
                    objCat.updateListPriceAttachment(Session("CompanyId"), dsProduct, blnSend, txtPrice.Text, Session("ImageIndex"), blnList, False)
                End If
                Session("ImageIndex") = ""
                If blnSend Then
                    ''Response.Redirect("CatalogueConfirm.aspx?pageid=" & strPageId & "&role=L&act=0&code=" & Server.UrlEncode(lblItemId.Text))
                    'Response.Redirect("CatalogueConfirm.aspx?pageid=" & strPageId & "&role=L&act=0&code=" & Server.UrlEncode(strItemId))
                    'Common.NetMsgbox(Me, "The Item has been submitted for Approval From Hub Admin.", "ListPriceCatalogue.aspx?pageid=" & strPageId)
                    Common.NetMsgbox(Me, "The Item has been submitted for Approval From Hub Admin.", dDispatcher.direct("Catalogue", "ListPriceCatalogue.aspx?pageid=" & strPageId))
                    'Response.Redirect("ListPriceCatalogue.aspx?pageid=" & strPageId)
                    'Me.cmdSave.Enabled = False
                    'Me.cmdReset.Enabled = False
                Else
                    'Common.NetMsgbox(Me, MsgRecordSave, "ListPriceCatalogue.aspx?pageid=" & strPageId, MsgBoxStyle.Information)
                    Common.NetMsgbox(Me, MsgRecordSave, dDispatcher.direct("Catalogue", "ListPriceCatalogue.aspx", "pageid=" & strPageId, MsgBoxStyle.Information))
                End If
            End If





        End If
        ''Else
        ''If strMsg <> "" Then
        ''    lblMsg.Text = strMsg
        ''Else
        ''    lblMsg.Text = ""
        ''End If
        ''End If
        objFile = Nothing
    End Sub

    Private Function bindProduct() As DataSet
        Dim ds As New DataSet
        Dim dtProduct As New DataTable
        Dim strPerc, strTaxId As String
        Dim objGst As New GST

        dtProduct.Columns.Add("CoyId", Type.GetType("System.String"))   'PM_S_COY_ID
        dtProduct.Columns.Add("ProductCode", Type.GetType("System.String")) 'PM_PRODUCT_CODE
        dtProduct.Columns.Add("VendorItemCode", Type.GetType("System.String"))  'PM_VENDOR_ITEM_CODE
        dtProduct.Columns.Add("ItemName", Type.GetType("System.String"))    'PM_PRODUCT_DESC ' dtProduct.Columns.Add("ProductDesc", Type.GetType("System.String"))
        dtProduct.Columns.Add("ReferenceNo", Type.GetType("System.String")) 'PM_REF_NO
        dtProduct.Columns.Add("Description", Type.GetType("System.String")) 'PM_LONG_DESC
        dtProduct.Columns.Add("CommodityType", Type.GetType("System.String")) 'PM_CATEGORY_NAME 'dtProduct.Columns.Add("CategoryName", Type.GetType("System.String"))
        dtProduct.Columns.Add("Tax", Type.GetType("System.String")) 'PM_GST_CODE 'dtProduct.Columns.Add("gstcode", Type.GetType("System.String"))
        dtProduct.Columns.Add("Perc", Type.GetType("System.String")) 'PM_TAX_PERC
        dtProduct.Columns.Add("TaxId", Type.GetType("System.String")) 'PM_TAX_ID
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
        dtProduct.Columns.Add("Remark", Type.GetType("System.String")) 'PM_REMARKS
        dtProduct.Columns.Add("PM_PRODUCT_FOR", Type.GetType("System.String")) 'PM_PRODUCT_FOR

        dtProduct.Columns.Add("status", Type.GetType("System.Int32"))
        dtProduct.Columns.Add("action", Type.GetType("System.String"))

        Dim dtr As DataRow
        dtr = dtProduct.NewRow()
        dtr("CoyId") = HttpContext.Current.Session("CompanyId")
        'dtr("ProductCode") = strItemId ' lblItemId.Text
        dtr("ProductCode") = Me.hidProductCode.Value ' lblItemId.Text

        dtr("VendorItemCode") = txtVendorItemCode.Text
        dtr("ItemName") = txtItemName.Text ' dtProduct.Columns.Add("ProductDesc", Type.GetType("System.String"))
        dtr("ReferenceNo") = txtRefNo.Text
        dtr("Description") = txtDesc.Text
        dtr("CommodityType") = Me.hidCommodityType.Value 'txtCommodityType.Text 'dtr("CategoryName", Type.GetType("System.String"))
        'dtr("Tax") = cboTax.SelectedItem.Text
        'dtr("Perc") = cboTax.SelectedItem.Text
        If ViewState("GSTCOD") = True Then
            dtr("Tax") = cboGSTRate.SelectedItem.Value
            If cboGSTRate.SelectedItem.Value = "N/A" Then
                dtr("Perc") = ""
                dtr("TaxId") = ""
            Else
                objGst.getGSTInfobyRate(cboGSTRate.SelectedItem.Value, strPerc, strTaxId)
                dtr("Perc") = strPerc
                dtr("TaxId") = strTaxId
            End If
        Else
            dtr("Tax") = cboGSTRate.SelectedItem.Text
            dtr("Perc") = cboGSTRate.SelectedItem.Text
            dtr("TaxId") = cboGSTRate.SelectedItem.Value
        End If
        
        
        'If Mid(cboTax.SelectedItem.Text, InStr(cboTax.SelectedItem.Text, " ") + 1) = "" Then
        '    dtr("Perc") = "0"
        'Else
        '    dtr("Perc") = Replace(Mid(cboTax.SelectedItem.Text, InStr(cboTax.SelectedItem.Text, " ") + 1), "%", "")
        'End If
        'dtr("TaxId") = cboTax.SelectedItem.Value
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
        dtr("Remark") = txtRemarks.Text
        dtr("PM_PRODUCT_FOR") = "V"

        '//Modify by Moo on 05/09/2005
        '//cboMgmt may not have data ,call "cboMgmt.SelectedItem.Value" on a dropdownlist that _
        '//do not have data causing "Object reference not set to an instance of an object" error
        'If cboMgmt.SelectedIndex <> -1 Then
        '    dtr("mgmtcode") = cboMgmt.SelectedItem.Value
        'Else
        '    dtr("mgmtcode") = ""
        'End If
        'dtr("mgmttext") = txtMgmtCode.Text


        dtr("status") = ListPriceStatus.PendingApproval
        If ViewState("mode") = "add" Then
            dtr("action") = "N" ' new
        Else
            dtr("action") = "M" ' modify
        End If
        dtProduct.Rows.Add(dtr)
        ds.Tables.Add(dtProduct)

        If IsNumeric(txtPrice.Text) And IsNumeric(hidPrice.Value) Then
            If CDbl(hidPrice.Value) <> CDbl(txtPrice.Text) Then
                blnList = True
            End If
        ElseIf hidPrice.Value = "" And txtPrice.Text <> "" Then
            blnList = True
        End If

        '' Discount Group
        'Dim dtDisc As New DataTable
        'dtDisc.Columns.Add("DiscIndex", Type.GetType("System.String"))
        'dtDisc.Columns.Add("DiscCode", Type.GetType("System.String"))
        'dtDisc.Columns.Add("DiscPrice", Type.GetType("System.String"))
        'dtDisc.Columns.Add("DiscRemark", Type.GetType("System.String"))
        'dtDisc.Columns.Add("Type", Type.GetType("System.String"))
        'dtDisc.Columns.Add("chk", Type.GetType("System.String"))
        'dtDisc.Columns.Add("Change", Type.GetType("System.Int32"))

        'Dim dgItem As DataGridItem
        'Dim chk As CheckBox
        'Dim hidChk As HtmlInputHidden
        'Dim hidPrice2 As HtmlInputHidden
        'For Each dgItem In dtgCatalogue.Items
        '    chk = dgItem.FindControl("chkSelection")
        '    hidChk = dgItem.FindControl("hidCheck")
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
        '        'dtr("DiscPrice") = CType(dgItem.FindControl("txtPrice"), TextBox).Text
        '        If CType(dgItem.FindControl("txtPrice"), TextBox).Text <> CType(dgItem.FindControl("hidPrice2"), HtmlInputHidden).Value Then
        '            blnDiscount = True
        '            dtr("Change") = 1
        '        End If
        '        dtr("DiscRemark") = CType(dgItem.FindControl("txtRemark"), TextBox).Text
        '        dtr("Type") = "D"
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
        'ds.Tables.Add(dtDisc)

        bindProduct = ds
    End Function

    'Private Sub dtgCatalogue_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemCreated
    '    intPageRecordCnt = viewstate("intPageRecordCnt")
    '    Grid_ItemCreated(dtgCatalogue, e)
    'End Sub

    'Private Sub dtgCatalogue_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemDataBound
    '    If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
    '        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
    '        Dim chk As CheckBox
    '        chk = e.Item.Cells(EnumCat.icChk).FindControl("chkSelection")

    '        Dim hidCheck As HtmlInputHidden
    '        hidCheck = e.Item.FindControl("hidCheck")

    '        Dim strChk As String
    '        strChk = Common.parseNull(dv("CHK"))
    '        If strChk = "1" Then
    '            chk.Checked = True
    '            hidCheck.Value = 1
    '        Else
    '            chk.Checked = False
    '            hidCheck.Value = 0
    '        End If

    '        Dim txtPrice As TextBox
    '        txtPrice = e.Item.FindControl("txtPrice")
    '        Dim hidPrice2 As HtmlInputHidden
    '        hidPrice2 = e.Item.FindControl("hidPrice2")

    '        If Not IsDBNull(dv("PDT_DISC_PRICE")) Then
    '            txtPrice.Text = Format(CDbl(dv("PDT_DISC_PRICE")), "#0.0000")
    '            hidPrice2.Value = Format(CDbl(dv("PDT_DISC_PRICE")), "#0.0000")
    '        Else
    '            txtPrice.Text = ""
    '            hidPrice2.Value = ""
    '        End If

    '        Dim txtRemark As TextBox
    '        txtRemark = e.Item.FindControl("txtRemark")
    '        txtRemark.Text = Common.parseNull(dv("PDT_DISC_REMARK"))
    '        txtRemark.Attributes.Add("onKeyDown", "limitText (this, 400);")

    '        Dim hidCode As HtmlInputHidden
    '        hidCode = e.Item.FindControl("hidCode")
    '        hidCode.Value = dv("PDT_DISC_CODE")

    '        ' for '?' purpose
    '        Dim txtQ As TextBox
    '        txtQ = e.Item.FindControl("txtQ")

    '        If hidSummary.Value = "" Then
    '            hidSummary.Value = "Discount Remarks-" & txtRemark.ClientID
    '        Else
    '            hidSummary.Value &= ",Discount Remarks-" & txtRemark.ClientID
    '        End If

    '        If hidControl.Value = "" Then
    '            hidControl.Value = hidCode.ClientID & "-" & txtQ.ClientID
    '        Else
    '            hidControl.Value &= "," & hidCode.ClientID & "-" & txtQ.ClientID
    '        End If
    '        ' ai chu add end
    '    End If
    'End Sub

    Private Sub cmdReset_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.ServerClick
        isNotPostBack()
        objCat.deleteTempAttachment(ViewState("itemid"), Session.SessionID)
        displayImageFile(Session("ImageIndex"))
        displayAttachFile()
    End Sub
End Class
