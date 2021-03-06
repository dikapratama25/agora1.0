Imports eProcure.Component
Imports AppCommon
Imports SSO.Component

Public Class coCompanyDetail1
    Inherits AppCommon.AppBaseClass
    Dim dDispatcher As New Dispatcher.dispatcher
    Protected WithEvents txt_attach As System.Web.UI.WebControls.TextBox
    Protected WithEvents cbostate As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cbocountry As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Textbox10 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAddress As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAddress2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAddress3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCity As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPostCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPhone As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFax As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCompanyLogo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBusinessRegNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtGSTRegNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents rfv_address As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_city As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_postcode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_country As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_phone As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_fax As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_state As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_BRegNo As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents rev_postcode As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents rev_email As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents rfv_email As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents btnBrowse1 As System.Web.UI.WebControls.Button
    Protected WithEvents btnUpload1 As System.Web.UI.WebControls.Button
    Protected WithEvents btnPrevPO1 As System.Web.UI.WebControls.Button
    Protected WithEvents cboCompType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents cboParentCoy As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtBankName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAccountNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBankCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBranchCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents cbocurrency As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rfv_currency As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents btnUpload As System.Web.UI.WebControls.Button
    Protected WithEvents btnPrevPO As System.Web.UI.WebControls.Button
    Protected WithEvents btnBrowse As System.Web.UI.WebControls.Button
    Protected WithEvents rdGSTCalcBy As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents txtTC As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_txtFax As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents Regularexpressionvalidator1 As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents cv_cboLicensePackage As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents cv_cboCompType As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents rfc_txtUserLicense As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents lblLogo As System.Web.UI.WebControls.Label
    Protected WithEvents lnkTC As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdCompLogo As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents cmdTCBrow As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents rdStatus As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rfv_txtCoyName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtCoyName As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents rev_txtCoyID As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents rfv_txtCoyID As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtCoyID As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents Hubadmin1 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents cboLicensePackage As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtUserLicense As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUserLicenseHide As System.Web.UI.HtmlControls.HtmlInputText
    Protected WithEvents rfv_txtUserLicense As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_txtUserLicense2 As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents Rangevalidator1 As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents rfc_txtUserLicense3 As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents txtSubStart As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_txtSubStart As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents txtSubEnd As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_txtSubEnd As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents cvf_txtDate As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents lblComLogo As System.Web.UI.WebControls.Label
    Protected WithEvents rdFinMode As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rdBCM As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Hubadmin2 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents revPwdDur As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents PwdDur As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txt_PwdDur As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents PayMethod As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cbo_PayMeth As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents PayTerm As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cbo_PayTerm As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents trPayTerm As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trPayMethod As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents chktraining As System.Web.UI.WebControls.CheckBox
    Protected WithEvents rdSkin As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rdPrivateLbl As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Hubadmin3 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents cmdsave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents vldsumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents tdVendor As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents Label19 As System.Web.UI.WebControls.Label
    Protected WithEvents Label21 As System.Web.UI.WebControls.Label
    Protected WithEvents txtLst As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfvLst As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents chkLstOri As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents chkLst As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents txtCompTypeOri As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdReject As System.Web.UI.WebControls.Button
    Protected WithEvents cmdApprove As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents trBack As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblAttach As System.Web.UI.WebControls.Label
    Protected WithEvents trSKU As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trTrans As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtSKU As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTrans As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfvSKU As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents revSKU As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents rfvTrans As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents revTrans As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents txtContact As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents Label20 As System.Web.UI.WebControls.Label
    Protected WithEvents cvSKU As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents cvTrans As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents txtReportUser As System.Web.UI.WebControls.TextBox
    Protected WithEvents cvReportUser As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents txtBCMSetting As System.Web.UI.WebControls.TextBox
    Protected WithEvents rdInvPayAppr As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rdInvMultiAppr As System.Web.UI.WebControls.RadioButtonList
    Dim objFile As New FileManagement
    Protected WithEvents INV As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Multi As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents BA As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents rdBA As System.Web.UI.WebControls.RadioButtonList
    Dim objDb As New DBAccess.EAD.DBCom


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
    'Private Property AppAdded() As Boolean
    '    Get
    '        If ViewState("AppAdded") Is Nothing Then
    '            Return False
    '        Else
    '            Return True
    '        End If
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        ViewState("AppAdded") = Value
    '    End Set
    'End Property

    'Protected Overrides Sub LoadViewState(ByVal savedState As Object)
    '    MyBase.LoadViewState(savedState)
    '    If Me.AppAdded() Then
    '        Me.bindApp()
    '    End If
    'End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        ViewState("Mode") = Request.Params("mode")
        If ViewState("Mode") = "modify" Then
            If Session("CompanyIdToken") = "" Then
                Session("CompanyIdToken") = Session("CompanyIDBkup")
            End If
        End If
        If Session("CompanyIdToken") = "" Then
            'ViewState("Mode") = Request.Params("mode")
            If ViewState("Mode") = "modify" Then
                ViewState("Mode") = "add"
            End If
        Else
            ViewState("Mode") = "modify"
        End If
        ViewState("ComID") = Request.QueryString("ComID")
        ViewState("Status") = Request.QueryString("status")
        Call Visble()
        If Not IsPostBack Then
            bindApp()
            isNotPostBack()
        End If
        cmdReset.CausesValidation = False
        cmdsave.Attributes.Add("onclick", "return checkFileType('img','" & cmdCompLogo.ClientID & "', 'doc','" & cmdTCBrow.ClientID & "');")
        cboCompType.Attributes.Add("onchange", "return companyTypeChange('" & ViewState("Mode") & "');")
        chkLst.Attributes.Add("onclick", "checkPackageChange();")
    End Sub
    Function Visble()
        Dim str As String
        Dim sql As String

        'CM_COY_NAME='" & txtCoyName.Text & "'
        sql = "Select CM_COY_TYPE from COMPANY_MSTR where CM_COY_ID='" & Session("CompanyIdToken") & "'"
        Dim tDS As DataSet = objDb.FillDs(sql)
        'If dr1.Read Then
        '    str = dr1("CM_COY_TYPE")
        'End If
        If tDS.Tables(0).Rows.Count > 0 Then
            str = tDS.Tables(0).Rows(0).Item("CM_COY_TYPE")
        End If
        'If str = "VENDOR" Or str = "BOTH" Then

        If str = "VENDOR" Or Session("Env") = "FTN" Or cboCompType.SelectedValue = "VENDOR" Then
            INV.Visible = False
            Multi.Visible = False
            BA.Visible = False
            Hubadmin2.Style("display") = "none"
        Else
            INV.Visible = True
            Multi.Visible = True
            BA.Visible = True
            Hubadmin2.Style("display") = ""
        End If
    End Function
    Private Sub isNotPostBack()
        If viewstate("Mode") = "add" Then
            lblTitle.Text = "Company Details"
            lblHeader.Text = "Add company"

            clearTextBox()
            PopulateCodeTable()
            GetDefaultPwdDuration()

            lnkTC.Visible = False
            Image1.Visible = False
            cmdReset.Value = "Clear"
            cmdApprove.Visible = False
            cmdReject.Visible = False
            trBack.Visible = False

        ElseIf viewstate("Mode") = "regappr" Then
            lblTitle.Text = "Public Vendor Registration Approval"
            lblHeader.Text = "Company Details"

            PopulateCodeTable()
            Populate()

            ' Hub Admin can change company id before approve
            'txtCoyID.Enabled = True

            cmdReset.Value = "Reset"
            cmdApprove.Visible = True
            cmdReject.Visible = True

            trBack.Visible = True
            lnkBack.NavigateUrl = dDispatcher.direct("PublicVendorReg", "ViewPublicVendorRegApprHubadmin.aspx")

        ElseIf viewstate("Status") = "Approved" Or viewstate("Status") = "Rejected" Then
            lblTitle.Text = "Public Vendor Registration Approval"
            lblHeader.Text = "Company Details"

            PopulateCodeTable()
            Populate()
            cmdsave.Visible = False
            cmdApprove.Visible = False
            cmdReject.Visible = False
            cmdReset.Visible = False
            trBack.Visible = True
            lnkBack.NavigateUrl = dDispatcher.direct("PublicVendorReg", "ViewPublicVendorRegApprHubadmin.aspx")


        Else
            lblTitle.Text = "Company Details"
            lblHeader.Text = "Modify company"

            PopulateCodeTable()
            Populate()

            txtCoyID.Enabled = False
            cmdReset.Value = "Reset"
            cmdApprove.Visible = False
            cmdReject.Visible = False
            trBack.Visible = False
        End If
    End Sub

    Private Sub clearTextBox()
        Me.txt_PwdDur.Text = ""
        Me.txtBankName.Text = ""
        Me.txtAccountNo.Text = ""
        Me.txtAddress.Text = ""
        Me.txtAddress2.Text = ""
        Me.txtAddress3.Text = ""
        Me.txtBankCode.Text = ""
        Me.txtBranchCode.Text = ""
        Me.txtBusinessRegNo.Text = ""
        Me.txtCity.Text = ""
        'Me.txtCompanyLogo.Text = ""
        Me.txtCoyID.Text = ""
        Me.txtCoyName.Text = ""
        Me.txtEmail.Text = ""
        Me.txtFax.Text = ""
        Me.txtGSTRegNo.Text = ""
        Me.txtPhone.Text = ""
        Me.txtPostCode.Text = ""
        Me.txtSubEnd.Text = ""
        Me.txtSubStart.Text = ""
        Me.txtSubStart.Text = ""
        'Me.txtTC.Text = ""
        Me.txtUserLicense.Text = ""
        Me.txtUserLicenseHide.Value = ""
        Me.txtReportUser.Text = ""

        Me.txtSKU.Text = "1"
        Me.txtTrans.Text = ""
        Me.txtContact.Text = ""

        Me.rdStatus.SelectedValue = "A"
        Me.cboCompType.SelectedIndex = 0
        Me.rdPrivateLbl.SelectedValue = "N"

        Me.rdSkin.SelectedValue = "1"

        Me.cboLicensePackage.Items.Clear()
    End Sub

    Private Sub PopulateCodeTable()
        Dim objGlobal As New AppGlobals

        objGlobal.FillCompany(cboParentCoy, "HubAdmin")
        objGlobal.FillCodeTable(cbocurrency, CodeTable.Currency)
        objGlobal.FillCodeTable(cbocountry, CodeTable.Country)
        objGlobal.FillCodeTable(cbostate, CodeTable.State)
        objGlobal.FillCodeTable(cbo_PayTerm, CodeTable.PaymentTerm)
        objGlobal.FillCodeTable(cbo_PayMeth, CodeTable.PaymentMethod)
        Common.FillDefault(cboLicensePackage, "PACKAGE_MSTR", "PM_PACKAGE_NAME", "PM_PACKAGE_NAME", "---Select---", "PM_TYPE ='" & cboCompType.SelectedValue & "'")

    End Sub

    Private Sub GetDefaultPwdDuration()
        Dim objLP As New LoginPolicy
        txt_PwdDur.Text = objLP.getParamValue("PASSWORD_DURATION")
        objLP = Nothing
    End Sub

    'Private Sub ShowHubInfo(ByVal pShow As Boolean)
    '    If pShow Then
    '        Hubadmin1.Style.Item("Display") = ""
    '        Hubadmin2.Style.Item("Display") = ""
    '        Hubadmin3.Style.Item("Display") = ""
    '        cboLicensePackage.Enabled = True
    '        cboLicenseUser.Enabled = True
    '        txtSubStart.Enabled = True
    '        txtSubEnd.Enabled = True
    '        txtCoyID.Enabled = True
    '    End If
    'End Sub
    Private Sub Populate()
        Dim objComp As New Companies
        Dim objUser As New Users
        Dim objCompDetails As New Company

        If viewstate("Mode") = "regappr" Then
            objCompDetails = objComp.GetDetails(viewstate("ComID"))
        Else
            objCompDetails = objComp.GetCompanyDetails(Session("CompanyIdToken"))
        End If

        If Not objCompDetails Is Nothing Then
            txtCoyID.Text = objCompDetails.CoyId
            txtCoyName.Text = objCompDetails.CoyName
            txtBankName.Text = objCompDetails.BankName
            txtAccountNo.Text = objCompDetails.AccountNo
            txtBankCode.Text = objCompDetails.BankCode
            txtBranchCode.Text = objCompDetails.BranchCode
            txtAddress.Text = objCompDetails.Address1
            txtAddress2.Text = objCompDetails.Address2
            txtAddress3.Text = objCompDetails.Address3
            txtCity.Text = objCompDetails.City
            txtPostCode.Text = objCompDetails.PostCode
            txtPhone.Text = objCompDetails.Phone
            txtFax.Text = objCompDetails.Fax
            txtEmail.Text = objCompDetails.Email
            'txtCompanyLogo.Text = objCompDetails.CoyLogo
            txt_PwdDur.Text = objCompDetails.PwdDuration
            txtSubStart.Text = Left(objCompDetails.SubStart, 10)
            txtSubEnd.Text = Left(objCompDetails.SubEnd, 10)
            txtBusinessRegNo.Text = objCompDetails.BusinessRegNo
            txtGSTRegNo.Text = objCompDetails.TaxRegNo
            'txtTC.Text = objCompDetails.TC
            txtUserLicense.Text = objCompDetails.LicenseUsers
            txtReportUser.Text = objCompDetails.ReportUsers

            txtSKU.Text = objCompDetails.SKU
            If txtSKU.Text = "" And objCompDetails.CoyType = "VENDOR" Then txtSKU.Text = "1"
            txtTrans.Text = objCompDetails.TransNo
            txtContact.Text = objCompDetails.ContactPerson

            rdStatus.SelectedValue = objCompDetails.Status
            ViewState("coStatus") = objCompDetails.Status
            rdGSTCalcBy.SelectedValue = IIf(objCompDetails.TaxCalBy = "", 0, objCompDetails.TaxCalBy)

            '----Testing By Praveen on 13.08.2007
            txtBCMSetting.Text = IIf(objCompDetails.BCMSetting = "0", "3", objCompDetails.BCMSetting)
            '----End The Testing By Praveen 

            rdBCM.SelectedValue = IIf(objCompDetails.BCMSetting = "0", 0, 1)
            rdFinMode.SelectedValue = objCompDetails.FinDeptMode
            rdInvPayAppr.SelectedValue = objCompDetails.InvAppr
            rdPrivateLbl.SelectedValue = objCompDetails.PrivLabeling
            rdSkin.SelectedValue = IIf(Asc(objCompDetails.Skins) = 0, 1, objCompDetails.Skins)
            chktraining.Checked = IIf(objCompDetails.TrainDemo = "Y", True, False)
            'New Code Adding for 1 PR to Multiple POs
            rdInvMultiAppr.SelectedValue = objCompDetails.MultiInvAppr
            'Michelle (5/2/2010) - BA to Cancel PO
            rdBA.SelectedValue = objCompDetails.BACanPO
            'End 

            'Common.SelDdl(Common.parseNull(objCompDetails.LicenseUsers), cboLicenseUser, True, True)

            Common.FillDefault(cboLicensePackage, "PACKAGE_MSTR", "PM_PACKAGE_NAME", "PM_PACKAGE_NAME", "---Select---", "PM_TYPE ='" & objCompDetails.CoyType & "'")
            Common.SelDdl(Common.parseNull(objCompDetails.Package), cboLicensePackage, True, True)
            Common.SelDdl(Common.parseNull(objCompDetails.Country), cbocountry, True, True)
            If objCompDetails.Country <> "MY" Then
                Dim objGlobal As New AppGlobals
                objGlobal.FillState(cbostate, cbocountry.SelectedItem.Value)
                objGlobal = Nothing
            End If
            Common.SelDdl(Common.parseNull(objCompDetails.State), cbostate, True, True)
            Common.SelDdl(Common.parseNull(objCompDetails.Currency), cbocurrency, True, True)
            Common.SelDdl(Common.parseNull(objCompDetails.PaymentTerm), cbo_PayTerm, True, True)
            Common.SelDdl(Common.parseNull(objCompDetails.PaymentMethod), cbo_PayMeth, True, True)
            Common.SelDdl(Common.parseNull(objCompDetails.ParentCoy), cboParentCoy, True, True)

            If ViewState("Mode") = "regappr" Then
                cboCompType.Enabled = False
                Common.SelDdl("VENDOR", cboCompType, True, True)
                txtCompTypeOri.Text = cboCompType.SelectedIndex
            Else
                Common.SelDdl(Common.parseNull(objCompDetails.CoyType), cboCompType, True, True)
                txtCompTypeOri.Text = cboCompType.SelectedIndex
            End If

            txtUserLicenseHide.Value = objUser.GetUserCount(objCompDetails.CoyId, False, True)
            ShowHide()

            DislayLogo(objCompDetails.CoyId)
            ViewState("CoyLogo") = objCompDetails.CoyLogo
            ViewState("Actual_TC") = objCompDetails.Actual_TC
            ViewState("TC") = objCompDetails.TC

            ' strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & "&doctype=PR>" & strFile & "</A>"
            If objCompDetails.Actual_TC <> "" Then
                lnkTC.Visible = True

                If ViewState("Mode") = "regappr" Then
                    Dim lstItem As ListItem

                    For Each lstItem In chkLst.Items
                        If lstItem.Selected = True Then
                            lnkTC.Text = objFile.getAttachPath(Server.UrlEncode(objCompDetails.Actual_TC), objCompDetails.Actual_TC, Server.UrlEncode(objCompDetails.TC), EnumDownLoadType.TermAndCond, "", EnumUploadFrom.BackOff, ConfigurationSettings.AppSettings(lstItem.Value & "Path"))
                            If lnkTC.Text <> "" Then
                                'lnkTC.Text = lnkTC.Text.Concat("&apppackage=" & lstItem.Value)
                                lnkTC.Text = lnkTC.Text.Substring(0, lnkTC.Text.IndexOf("?") + 1) & "apppackage=" & lstItem.Value & "&" & lnkTC.Text.Substring(lnkTC.Text.IndexOf("?") + 1)
                            End If
                            Exit For
                        End If
                    Next
                Else
                    Dim objCompany As New Companies
                    Dim dsAppPackage As DataSet = objCompany.getAppPackage(Web.HttpContext.Current.Session("CompanyIdToken"))

                    If dsAppPackage.Tables(0).Rows.Count > 0 Then
                        Dim i As Integer

                        For i = 0 To dsAppPackage.Tables(0).Rows.Count - 1
                            If dsAppPackage.Tables(0).Rows(i).Item("chk") = "Y" Then
                                'lnkTC.Text = objCompDetails.Actual_TC
                                '*************************meilai 25/2/05****************************
                                'lnkTC.NavigateUrl = "../FileDownLoad.aspx?file=" & Server.UrlEncode(objCompDetails.TC) & "&actualfile=" & Server.UrlEncode(objCompDetails.Actual_TC) & "&type=" & EnumDownLoadType.TermAndCond
                                'strImgSrc = objFile.getCoyLogo(EnumUploadFrom.BackOff, strCoyID, ConfigurationSettings.AppSettings("eProcurePath"))
                                lnkTC.Text = objFile.getAttachPath(Server.UrlEncode(objCompDetails.Actual_TC), objCompDetails.Actual_TC, Server.UrlEncode(objCompDetails.TC), EnumDownLoadType.TermAndCond, "", EnumUploadFrom.BackOff, ConfigurationSettings.AppSettings(dsAppPackage.Tables(0).Rows(i).Item("ap_app_id") & "Path"))
                                '*************************meilai************************************
                                If lnkTC.Text <> "" Then Exit For
                            End If
                        Next
                    End If
                End If
            Else
                lnkTC.Visible = False
            End If
        End If
    End Sub

    Sub DislayLogo(ByVal strCoyID As String)
        'Dim objFile As New FileManagement
        Dim strImgSrc As String

        If viewstate("Mode") = "regappr" Then
            Dim objFileMgmt As New ERFP.Components.RFPFileManageClass
            Dim lstItem As ListItem

            For Each lstItem In chkLst.Items
                If lstItem.Selected = True Then
                    strImgSrc = objFileMgmt.getCoyLogo(EnumUploadFrom.BackOff, strCoyID, ConfigurationSettings.AppSettings(lstItem.Value & "Path"))
                    If strImgSrc <> "" Then Exit For
                End If
            Next
        Else
            Dim objCompany As New Companies
            Dim dsAppPackage As DataSet = objCompany.getAppPackage(Web.HttpContext.Current.Session("CompanyIdToken"))

            If dsAppPackage.Tables(0).Rows.Count > 0 Then
                Dim i As Integer

                For i = 0 To dsAppPackage.Tables(0).Rows.Count - 1
                    If dsAppPackage.Tables(0).Rows(i).Item("chk") = "Y" Then
                        'strImgSrc = objFile.getCoyLogo(EnumUploadFrom.BackOff, strCoyID, ConfigurationSettings.AppSettings("eProcurePath"))
                        'strImgSrc = objFile.getCoyLogo(EnumUploadFrom.BackOff, strCoyID, ConfigurationSettings.AppSettings(dsAppPackage.Tables(0).Rows(i).Item("ap_app_id") & "Path"))

                        If dsAppPackage.Tables(0).Rows(i).Item("ap_app_id") = EnumAppPackage.eRFP.ToString Then
                            Dim objFileMgmt As New ERFP.Components.RFPFileManageClass
                            strImgSrc = objFileMgmt.getCoyLogo(EnumUploadFrom.BackOff, strCoyID, ConfigurationSettings.AppSettings(dsAppPackage.Tables(0).Rows(i).Item("ap_app_id") & "Path"))
                        Else
                            Dim objFileMgmt As New FileManagement
                            strImgSrc = objFileMgmt.getCoyLogo(EnumUploadFrom.BackOff, strCoyID, ConfigurationSettings.AppSettings(dsAppPackage.Tables(0).Rows(i).Item("ap_app_id") & "Path"))
                        End If
                        If strImgSrc <> "" Then Exit For
                    End If
                Next
            End If
        End If

        If strImgSrc <> "" Then
            Image1.Visible = True
            Image1.ImageUrl = strImgSrc
            lblLogo.Visible = False
        Else
            Image1.Visible = False
            lblLogo.Visible = True
        End If
    End Sub

    Private Function GetInput(ByVal pAppPkg() As String) As Company
        Dim objCompDetails As New Company
        objCompDetails.CoyId = txtCoyID.Text
        objCompDetails.CoyName = txtCoyName.Text
        objCompDetails.BankName = txtBankName.Text
        objCompDetails.AccountNo = txtAccountNo.Text
        objCompDetails.BankCode = txtBankCode.Text
        objCompDetails.BranchCode = txtBranchCode.Text
        objCompDetails.Address1 = txtAddress.Text
        objCompDetails.Address2 = txtAddress2.Text
        objCompDetails.Address3 = txtAddress3.Text
        objCompDetails.City = txtCity.Text
        objCompDetails.PostCode = txtPostCode.Text
        objCompDetails.Phone = txtPhone.Text
        objCompDetails.Fax = txtFax.Text
        objCompDetails.Email = txtEmail.Text
        'objCompDetails.CoyLogo = txtCompanyLogo.Text
        objCompDetails.BusinessRegNo = txtBusinessRegNo.Text
        objCompDetails.TaxRegNo = txtGSTRegNo.Text
        objCompDetails.PwdDuration = txt_PwdDur.Text
        objCompDetails.SubStart = txtSubStart.Text
        objCompDetails.SubEnd = txtSubEnd.Text
        objCompDetails.BusinessRegNo = txtBusinessRegNo.Text
        objCompDetails.TaxRegNo = txtGSTRegNo.Text
        'objCompDetails.TC = txtTC.Text

        objCompDetails.Status = rdStatus.SelectedValue
        objCompDetails.TaxCalBy = rdGSTCalcBy.SelectedValue
        '-----------NEW Code by praveen on 13.08.2007(getting error string " missing in the SqlQuery
        'If rdBCM.SelectedValue = "0" Then
        '    objCompDetails.BCMSetting = rdBCM.SelectedValue
        'Else
        '    '  objCompDetails.BCMSetting = rdBCM.SelectedValue Michelle (4/10/2007) - To resolve the problem where system everytime default the BCMsetting to '1'
        '    objCompDetails.BCMSetting = txtBCMSetting.Text
        'End If
        '=-----End the Testing 

        objCompDetails.BCMSetting = rdBCM.SelectedValue 'Michelle (3/8/2010) - To resolve the problem on sqlQuery error

        objCompDetails.FinDeptMode = rdFinMode.SelectedValue
        objCompDetails.InvAppr = rdInvPayAppr.SelectedValue
        objCompDetails.PrivLabeling = rdPrivateLbl.SelectedValue
        objCompDetails.Skins = rdSkin.SelectedValue
        objCompDetails.TrainDemo = IIf(chktraining.Checked, "Y", "N")
        objCompDetails.SKU = txtSKU.Text
        objCompDetails.TransNo = txtTrans.Text
        objCompDetails.ContactPerson = txtContact.Text

        objCompDetails.LicenseUsers = txtUserLicense.Text
        objCompDetails.ReportUsers = txtReportUser.Text
        objCompDetails.Country = cbocountry.SelectedValue
        objCompDetails.State = cbostate.SelectedValue
        objCompDetails.Currency = cbocurrency.SelectedValue
        objCompDetails.PaymentTerm = cbo_PayTerm.SelectedValue
        objCompDetails.PaymentMethod = cbo_PayMeth.SelectedValue
        objCompDetails.ParentCoy = cboParentCoy.SelectedValue
        objCompDetails.CoyType = cboCompType.SelectedValue
        objCompDetails.Package = cboLicensePackage.SelectedValue
        'New code for Adding 1PR To Multiple POs by Praveen on 29/06/2007
        objCompDetails.MultiInvAppr = rdInvMultiAppr.SelectedValue
        objCompDetails.BACanPO = rdBA.SelectedValue
        'end 

        '//Added By Moo, for company Logo
        'ViewState("CoyLogo") = objCompDetails.CoyLogo
        'ViewState("Actual_TC") = objCompDetails.Actual_TC
        'ViewState("TC") = objCompDetails.TC
        Dim objFile As New FileManagement
        Dim strFile As String
        Dim objCompany As New Companies
        'Dim dsAppPackage As DataSet = objCompany.getAppPackage(Web.HttpContext.Current.Session("CompanyIdToken"))

        If cmdCompLogo.PostedFile.FileName <> "" Then

            If pAppPkg.Length > 0 Then
                Dim i As Integer

                For i = 0 To pAppPkg.Length - 1
                    If pAppPkg(i) <> "" Then

                        Dim objDB As New DBAccess.EAD.DBCom(ConfigurationSettings.AppSettings(pAppPkg(i) & "Path"))
                        Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'ImageAttachSize'"))

                        Dim sFileName As String
                        sFileName = System.IO.Path.GetFileName(cmdCompLogo.PostedFile.FileName)

                        If Len(sFileName) > 50 Then
                            Throw New Exception("File name exceeds 46 character")
                        ElseIf cmdCompLogo.PostedFile.ContentLength > 0 And cmdCompLogo.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                            strFile = objFile.uploadCoyLogo(cmdCompLogo, EnumUploadFrom.BackOff, txtCoyID.Text, ConfigurationSettings.AppSettings(pAppPkg(i) & "Path"))
                        ElseIf cmdCompLogo.PostedFile.ContentLength = 0 Then
                            Throw New Exception("0 byte document or file not found")
                        Else
                            Throw New Exception("File exceeds maximum file size")
                        End If

                        If strFile <> "" Then Exit For
                    End If
                Next
            End If

            'If dsAppPackage.Tables(0).Rows.Count > 0 Then
            '    Dim i As Integer

            '    For i = 0 To dsAppPackage.Tables(0).Rows.Count - 1
            '        If dsAppPackage.Tables(0).Rows(i).Item("chk") = "Y" Then
            '            strFile = objFile.uploadCoyLogo(cmdCompLogo, EnumUploadFrom.BackOff, txtCoyID.Text, ConfigurationSettings.AppSettings(dsAppPackage.Tables(0).Rows(i).Item("ap_app_id") & "Path"))
            '            If strFile <> "" Then Exit For
            '        End If
            '    Next
            'End If

            objCompDetails.CoyLogo = strFile
            ViewState("CoyLogo") = objCompDetails.CoyLogo
        Else
            objCompDetails.CoyLogo = ViewState("CoyLogo")
        End If

        Dim strActualTC, strHubTC As String
        'strFile = objFile.FileUpload(cmdTCBrow, EnumUploadType.TermAndCond, "", EnumUploadFrom.FrontOff, "")
        If cmdTCBrow.PostedFile.FileName <> "" Then

            If pAppPkg.Length > 0 Then
                Dim i As Integer

                For i = 0 To pAppPkg.Length - 1
                    If pAppPkg(i) <> "" Then

                        Dim objDB As New DBAccess.EAD.DBCom(ConfigurationSettings.AppSettings(pAppPkg(i) & "Path"))
                        Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))

                        Dim sFileName As String
                        sFileName = System.IO.Path.GetFileName(cmdTCBrow.PostedFile.FileName)

                        If Len(sFileName) > 50 Then
                            Throw New Exception("File name exceeds 46 character")
                        ElseIf cmdTCBrow.PostedFile.ContentLength > 0 And cmdTCBrow.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                            objFile.uploadCoyTC(cmdTCBrow, EnumUploadFrom.BackOff, txtCoyID.Text, strActualTC, strHubTC, ConfigurationSettings.AppSettings(pAppPkg(i) & "Path"))
                        ElseIf cmdTCBrow.PostedFile.ContentLength = 0 Then
                            Throw New Exception("0 byte document or file not found")
                        Else
                            Throw New Exception("File exceeds maximum file size")
                        End If

                        If strHubTC <> "" Then Exit For
                    End If
                Next
            End If

            'If dsAppPackage.Tables(0).Rows.Count > 0 Then
            '    Dim i As Integer

            '    For i = 0 To dsAppPackage.Tables(0).Rows.Count - 1
            '        If dsAppPackage.Tables(0).Rows(i).Item("chk") = "Y" Then
            '            objFile.uploadCoyTC(cmdTCBrow, EnumUploadFrom.BackOff, txtCoyID.Text, strActualTC, strHubTC, ConfigurationSettings.AppSettings(dsAppPackage.Tables(0).Rows(i).Item("ap_app_id") & "Path"))
            '            If strHubTC <> "" Then Exit For
            '        End If
            '    Next
            'End If

            'lnkTC.Text = strActualTC
            objCompDetails.Actual_TC = strActualTC
            objCompDetails.TC = strHubTC
            ViewState("Actual_TC") = objCompDetails.Actual_TC
            ViewState("TC") = objCompDetails.TC
        Else
            objCompDetails.Actual_TC = ViewState("Actual_TC")
            objCompDetails.TC = ViewState("TC")
        End If

        If objCompDetails.Actual_TC <> "" Then
            lnkTC.Visible = True
            'lnkTC.Text = objCompDetails.Actual_TC
            '*************************meilai 25/2/05****************************
            'lnkTC.NavigateUrl = "../FileDownLoad.aspx?file=" & Server.UrlEncode(objCompDetails.TC) & "&actualfile=" & Server.UrlEncode(objCompDetails.Actual_TC) & "&type=" & EnumDownLoadType.TermAndCond
            lnkTC.Text = objFile.getAttachPath(Server.UrlEncode(objCompDetails.Actual_TC), objCompDetails.Actual_TC, Server.UrlEncode(objCompDetails.TC), EnumDownLoadType.TermAndCond, "", EnumUploadFrom.BackOff, ConfigurationSettings.AppSettings("eRFPPath"))
            '*************************meilai************************************
        End If

        objFile = Nothing

        Return objCompDetails
    End Function


    Private Sub cmdsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdsave.Click
        Call SaveRecord()
    End Sub

    Private Sub cbocountry_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbocountry.SelectedIndexChanged
        Dim objGlobal As New AppGlobals
        'If cbocountry.SelectedIndex <> -1 Then
        objGlobal.FillState(cbostate, cbocountry.SelectedItem.Value)
        'End If
    End Sub

    Private Sub cboCompType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCompType.SelectedIndexChanged
        cboLicensePackage.Items.Clear()
        Common.FillDefault(cboLicensePackage, "PACKAGE_MSTR", "PM_PACKAGE_NAME", "PM_PACKAGE_NAME", "---Select---", "PM_TYPE ='" & cboCompType.SelectedValue & "'")
        ShowHide()
        '---------------New Code Added to Make the following are visible="false" By Praveen on 03.08.2007
        If cboCompType.SelectedValue = "VENDOR" Or Session("Env") = "FTN" Then
            INV.Visible = False
            Multi.Visible = False
            BA.Visible = False
            txtSKU.Text = "1"
            Hubadmin2.Style("display") = "none"
            'rdInvPayAppr.Visible = False
            'rdInvMultiAppr.Visible = False
        Else
            Hubadmin2.Style("display") = ""
            INV.Visible = True
            Multi.Visible = True
            BA.Visible = True
        End If
        '---------------End
    End Sub

    Private Sub ShowHide()
        If cboCompType.SelectedValue = "VENDOR" Then
            'tdVendor.Style.Item("Display") = ""
            trPayMethod.Style.Item("Display") = "none"
            trPayTerm.Style.Item("Display") = "none"
            'trSKU.Style("display") = "inline"
            trTrans.Style("display") = ""
            enableValidator(True)
            PayMethod.Enabled = False
            PayTerm.Enabled = False
        ElseIf cboCompType.SelectedValue = "BUYER" Then
            PayMethod.Enabled = True
            PayTerm.Enabled = True
            tdVendor.Style.Item("Display") = "none"
            trPayMethod.Style.Item("Display") = ""
            trPayTerm.Style.Item("Display") = ""
            trSKU.Style("display") = "none"
            trTrans.Style("display") = "none"
            enableValidator(False)
        ElseIf cboCompType.SelectedValue = "BOTH" Then
            PayMethod.Enabled = True
            PayTerm.Enabled = True
            'tdVendor.Style.Item("Display") = ""
            trPayMethod.Style.Item("Display") = ""
            trPayTerm.Style.Item("Display") = ""
            'trSKU.Style("display") = "inline"
            trTrans.Style("display") = ""
            enableValidator(True)
        End If
    End Sub

    Private Sub enableValidator(ByVal bln As Boolean)
        'Me.rfvSKU.Enabled = bln
        'Me.rfvTrans.Enabled = bln
        'Me.revSKU.Enabled = bln
        'Me.revTrans.Enabled = bln
        Me.cvSKU.Enabled = bln
        Me.cvTrans.Enabled = bln
    End Sub

    Private Sub cmdReset_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.ServerClick
        bindApp()
        isNotPostBack()
    End Sub

    Private Sub bindApp()
        Dim i As Integer
        Dim ds As New DataSet
        Dim objComp As New Companies
        Dim objCompDetails As New Company

        'Dim strCompany As String = objCompDetails.CoyId
        If viewstate("Mode") = "add" Or viewstate("Mode") = "regappr" Then
            ds = objComp.getAppPackage("")
        Else
            ds = objComp.getAppPackage(Session("CompanyIdToken"))
        End If

        chkLst.DataSource = ds.Tables(0).DefaultView
        chkLst.DataBind()
        chkLstOri.DataSource = ds.Tables(0).DefaultView
        chkLstOri.DataBind()

        If viewstate("Mode") = "regappr" Then
            chkLst.Enabled = False

            For i = 0 To ds.Tables(0).Rows.Count - 1
                If ds.Tables(0).Rows(i)("ap_app_id") = EnumAppPackage.eRFP.ToString Then
                    chkLst.Items(i).Selected = True
                    chkLstOri.Items(i).Selected = True
                Else
                    chkLst.Items(i).Selected = False
                    chkLstOri.Items(i).Selected = False
                End If
            Next
        Else
            For i = 0 To ds.Tables(0).Rows.Count - 1
                If ds.Tables(0).Rows(i)("chk") = "Y" Then
                    chkLst.Items(i).Selected = True
                    chkLstOri.Items(i).Selected = True
                Else
                    chkLst.Items(i).Selected = False
                    chkLstOri.Items(i).Selected = False
                End If
            Next
        End If

        ''For i = 0 To ds.Tables(0).Rows.Count - 1
        ''    'Dim chkLst As New CheckBox
        ''    'For Each chk In pnlApp.Controls
        ''    'If chk.Checked Then
        ''    chkLst.ID = ds.Tables(0).Rows(i)("AP_APP_ID")
        ''    chkLst.Text = ds.Tables(0).Rows(i)("AP_APP_NAME")
        ''    If ds.Tables(0).Rows(i)("chk") = "Y" Then
        ''        chkLst.Checked = True
        ''    Else
        ''        chkLst.Checked = False
        ''    End If
        ''    ' pnlApp.Controls.Add(chkLst)
        ''    'End If

        ''Next
        'AppAdded = True
    End Sub

    'Private Function bindSelectedApp(ByRef strApp() As String) As Boolean
    '    Dim chk As CheckBox
    '    'Dim strApp(4) As String
    '    Dim i As Integer = 0
    '    Dim cnt As Integer = pnlApp.Controls.Count
    '    If cnt > 0 Then
    '        Dim strApp2(cnt) As String
    '        For Each chk In pnlApp.Controls
    '            If CType(chk, CheckBox).Checked Then
    '                strApp2(i) = chk.ID
    '                i += 1
    '            End If
    '        Next
    '        strApp = strApp2
    '    End If
    'End Function

    Private Function bindSelectedApp(ByRef strApp() As String) As Boolean

        'Dim strApp(4) As String
        Dim i As Integer = 0
        Dim cnt As Integer = chkLst.Items.Count
        If cnt > 0 Then
            Dim strAppPkg(cnt) As String
            For i = 0 To cnt - 1

                If viewstate("Mode") = "regappr" Then
                    If chkLst.Items(i).Text = EnumAppPackage.eRFP.ToString Then
                        chkLst.Items(i).Selected = True
                        chkLstOri.Items(i).Selected = True
                    End If
                End If

                If chkLst.Items(i).Selected Then
                    strAppPkg(i) = chkLst.Items(i).Value
                End If
            Next
            strApp = strAppPkg
        End If
    End Function

    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    Dim strApp() As String
    '    bindSelectedApp(strApp)
    'End Sub

    Private Sub SaveRecord()
        Dim strErrMsg As String
        Dim objComp As New Companies
        Dim objCompDetails As New Company

        Dim strApp(4) As String
        bindSelectedApp(strApp)

        Try
            objCompDetails = GetInput(strApp)
            If ViewState("Mode") = "add" Then
                If objComp.AddCompany(objCompDetails, strApp) Then
                    'If objComp.AddCompany(objCompDetails) Then
                    txtCoyID.Enabled = False
                    'Response.Write("<script>window.parent.frames['menu'].location='../menu.aspx?mode=show';</script>")
                    'Session("CompanyIdToken") = objComp.GetCompanyID(txtCoyName.Text)
                    ViewState("Mode") = "modify"
                End If
                Common.NetMsgbox(Me, objComp.Message, MsgBoxStyle.Information)
                Session("CompanyIdToken") = objComp.GetCompanyID(txtCoyName.Text)
                DislayLogo(txtCoyID.Text)
            ElseIf ViewState("Mode") = "modify" Then
                If objComp.UpdateCompany(objCompDetails, strApp, False) Then
                    If ViewState("coStatus") <> objCompDetails.Status And objCompDetails.CoyType <> "BUYER" Then ' company's status changed
                        Dim objEmail As New Email
                        If objCompDetails.Status = "A" Then
                            objEmail.sendNotification(EmailType.CompanyActivated, "", "", objCompDetails.CoyId, "", "", "V")
                            objEmail.sendNotification(EmailType.CompanyActivated, "", "", objCompDetails.CoyId, "", "", "B")
                        Else
                            objEmail.sendNotification(EmailType.CompanyDeactivated, "", "", objCompDetails.CoyId, "", "", "V")
                            objEmail.sendNotification(EmailType.CompanyDeactivated, "", "", objCompDetails.CoyId, "", "", "B")
                        End If
                        objEmail = Nothing
                    End If
                    Common.NetMsgbox(Me, objComp.Message, Request.RawUrl, MsgBoxStyle.Information)
                    ViewState("Mode") = "modify"
                Else
                    Common.NetMsgbox(Me, objComp.Message, MsgBoxStyle.Information)
                End If

            ElseIf ViewState("Mode") = "regappr" Then

                If objComp.UpdateComDetails(ViewState("ComID"), objCompDetails) Then
                    Common.NetMsgbox(Me, objComp.Message, Request.RawUrl.Replace("ComID=" & ViewState("ComID"), "ComID=" & txtCoyID.Text), MsgBoxStyle.Information)
                Else
                    Common.NetMsgbox(Me, objComp.Message, MsgBoxStyle.Information)
                End If
            End If
        Catch ex As Exception
            Common.NetMsgbox(Me, ex.Message)
        End Try
    End Sub

    Private Sub cmdReject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReject.Click
        Dim objComp As New Companies
        Dim objCompDetails As New Company

        Dim strApp(4) As String
        bindSelectedApp(strApp)

        Try
            objCompDetails = GetInput(strApp)

            If objComp.UpdateComDetails(ViewState("ComID"), objCompDetails) Then
                If objComp.updateStatusHubReject(txtCoyID.Text, VendorRegApprStatus.Reject) Then

                    Dim objMail As New Email
                    Dim reject As String = VendorRegApprStatus.Reject

                    objMail.sendNotification(EmailType.ApprovedVCoyReg, "", "Hub", txtCoyID.Text, "", "", reject)

                    Common.NetMsgbox(Me, "Registration is Rejected.", "../PublicVendorReg/ViewPublicVendorRegApprHubadmin.aspx", MsgBoxStyle.Information)
                Else
                    Common.NetMsgbox(Me, objComp.Message, MsgBoxStyle.Information)
                End If
            Else
                Common.NetMsgbox(Me, objComp.Message, MsgBoxStyle.Information)
            End If
        Catch ex As Exception
            Common.NetMsgbox(Me, ex.Message)
        End Try
    End Sub

    Private Sub cmdApprove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApprove.Click
        Dim objComp As New Companies
        Dim objCompDetails As New Company

        Dim strApp(4) As String
        bindSelectedApp(strApp)

        Try
            objCompDetails = GetInput(strApp)

            If objComp.UpdateComDetails(ViewState("ComID"), objCompDetails) Then
                If objComp.updateStatusHubApproval(txtCoyID.Text) Then

                    Dim objMail As New Email
                    Dim approved As String = VendorRegApprStatus.Approved

                    objMail.sendNotification(EmailType.ApprovedVCoyReg, "", "Hub", txtCoyID.Text, "", "", approved)

                    Response.Write("<script>window.parent.frames['menu'].location='../menu.aspx';</script>")
                    Common.NetMsgbox(Me, "Approved Successfully.", "../PublicVendorReg/ViewPublicVendorRegApprHubadmin.aspx", MsgBoxStyle.Information)

                Else
                    Common.NetMsgbox(Me, objComp.Message, MsgBoxStyle.Information)
                End If
            Else
                Common.NetMsgbox(Me, objComp.Message, MsgBoxStyle.Information)
            End If
        Catch ex As Exception
            Common.NetMsgbox(Me, ex.Message)
        End Try
    End Sub


End Class
