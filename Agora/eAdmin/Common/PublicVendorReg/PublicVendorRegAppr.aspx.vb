''*************************************************************************************
'Created By:Ya Li
'Date:      25/05/2005
'Screen:    Public Vendor Registration Approval
'Purpose:   Allows Hub Admin user to view the details of the Public Vendor Company 
'           Registration that is submitted for approval.Also can modify the details 
'           and then choose whether approve or reject the regsitration.

'**************************************************************************************
'Imports ERFP.Components
Imports AgoraLegacy
Imports SSO.Component

Public Class PublicVendorRegAppr
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents vldsumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdApprove As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReject As System.Web.UI.WebControls.Button
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents txtComID As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_txtComID As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rev_txtComID As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents txtComName As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_txtComName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents txtAddress As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_address As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtAddress2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAddress3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents txtCity As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_city As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents cboState As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rfv_state As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents txtPostCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents rev_postcode As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents rfv_postcode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents cboCountry As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rfv_country As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents txtPhone As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_phone As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents txtFax As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_fax As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents txtEmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents rev_email As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents rfv_email As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents txtBusinessRegNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents rfv_BRegNo As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtAccountNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBankCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBranchCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents cboCurrency As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rfv_currency As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents lblLogo As System.Web.UI.WebControls.Label
    Protected WithEvents lblComLogo As System.Web.UI.WebControls.Label
    Protected WithEvents txtGSTRegNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents optGSTCalcBy As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents optBCM As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents optFinMode As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents txtPwdDur As System.Web.UI.WebControls.TextBox
    Protected WithEvents PwdDur As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents revPwdDur As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents optPrivateLbl As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents optSkin As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents chktraining As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cmdCompLogo As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents cmdTCBrow As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents tdVendor As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents optStatus As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents lnkTC As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents trBack As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblAttach As System.Web.UI.WebControls.Label
    Protected WithEvents txtBCMSetting As System.Web.UI.WebControls.TextBox
    Protected WithEvents optInvAppr As System.Web.UI.WebControls.RadioButtonList

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim objComp As New Companies
    Dim objCompDetails As New Company
    Dim intMsg As Integer
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim coyid As String
        Dim userid As String
        Dim companyID As String
        Dim strStatus As Integer

        'Dim cm_hub_appr_status As Integer

        coyid = HttpContext.Current.Session("CompanyId")
        userid = HttpContext.Current.Session("UserId")

        MyBase.Page_Load(sender, e)

        If Not IsPostBack Then
            viewstate("ComID") = Request.QueryString("ComID")
            companyID = Request.QueryString("ComID")

            txtComID.Enabled = False

            Common.FillDefault(cboState, "CODE_MSTR", "CODE_DESC", "CODE_ABBR", "--- Select ---", "CODE_CATEGORY ='S'")
            Common.FillDefault(cboCountry, "CODE_MSTR", "CODE_DESC", "CODE_ABBR", "--- Select ---", "CODE_CATEGORY ='CT'")
            Common.FillDefault(cboCurrency, "CODE_MSTR", "CODE_DESC", "CODE_ABBR", "--- Select ---", "CODE_CATEGORY ='CU'")

            Populate()

            strStatus = objComp.getHubApproveStatus(companyID)
            If UCase(strStatus) = 1 Then
                cmdSave.Enabled = False
                cmdApprove.Enabled = False
                cmdReject.Enabled = False
            ElseIf UCase(strStatus) = 2 Then
                cmdSave.Enabled = False
                cmdApprove.Enabled = False
                cmdReject.Enabled = False
            End If
            'lnkBack.NavigateUrl = "ViewPublicVendorRegApprHubadmin.aspx?pageid=" & strPageId
            lnkBack.NavigateUrl = dDispatcher.direct("PublicVendorReg", "ViewPublicVendorRegApprHubadmin.aspx", "pageid=" & strPageId)
        End If


    End Sub

    Private Sub Populate()
        Dim objComp As New Companies
        Dim objUser As New Users
        Dim objCompDetails As New Company
        Dim objfile As New FileManagement

        objCompDetails = objComp.GetDetails(viewstate("ComID"))
        If Not objCompDetails Is Nothing Then
            txtComID.Text = objCompDetails.CoyId
            txtComName.Text = objCompDetails.CoyName
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
            txtBusinessRegNo.Text = objCompDetails.BusinessRegNo
            txtGSTRegNo.Text = objCompDetails.TaxRegNo
            txtPwdDur.Text = objCompDetails.PwdDuration
            txtBusinessRegNo.Text = objCompDetails.BusinessRegNo
            txtGSTRegNo.Text = objCompDetails.TaxRegNo

            optStatus.SelectedValue = objCompDetails.Status
            optGSTCalcBy.SelectedValue = objCompDetails.TaxCalBy
            txtBCMSetting.Text = IIf(objCompDetails.BCMSetting = "0", "3", objCompDetails.BCMSetting)
            optBCM.SelectedValue = IIf(objCompDetails.BCMSetting = "0", 0, 1)
            optFinMode.SelectedValue = objCompDetails.FinDeptMode
            optInvAppr.SelectedValue = objCompDetails.InvAppr
            optPrivateLbl.SelectedValue = objCompDetails.PrivLabeling
            optSkin.SelectedValue = objCompDetails.Skins
            chktraining.Checked = IIf(objCompDetails.TrainDemo = "Y", True, False)

            Common.SelDdl(Common.parseNull(objCompDetails.Country), cboCountry, True, True)
            If objCompDetails.Country <> "MY" Then
                Dim objGlobal As New AppGlobals
                objGlobal.FillState(cboState, cboCountry.SelectedItem.Value)
                objGlobal = Nothing
            End If
            Common.SelDdl(Common.parseNull(objCompDetails.State), cboState, True, True)
            Common.SelDdl(Common.parseNull(objCompDetails.Currency), cboCurrency, True, True)

            'ViewState("CoyLogo") = objCompDetails.CoyLogo
            'ViewState("Actual_TC") = objCompDetails.Actual_TC
            'ViewState("TC") = objCompDetails.TC

            DislayLogo(objCompDetails.CoyId)
            ViewState("CoyLogo") = objCompDetails.CoyLogo
            ViewState("Actual_TC") = objCompDetails.Actual_TC
            ViewState("TC") = objCompDetails.TC

            ' strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & "&doctype=PR>" & strFile & "</A>"
            If objCompDetails.Actual_TC <> "" Then
                lnkTC.Visible = True
                'lnkTC.Text = objCompDetails.Actual_TC
                '*************************meilai 25/2/05****************************
                'lnkTC.NavigateUrl = "../FileDownLoad.aspx?file=" & Server.UrlEncode(objCompDetails.TC) & "&actualfile=" & Server.UrlEncode(objCompDetails.Actual_TC) & "&type=" & EnumDownLoadType.TermAndCond
                lnkTC.Text = objfile.getAttachPath(Server.UrlEncode(objCompDetails.Actual_TC), objCompDetails.Actual_TC, Server.UrlEncode(objCompDetails.TC), EnumDownLoadType.TermAndCond, "", EnumUploadFrom.FrontOff, ConfigurationSettings.AppSettings("eRFPPath"))
                '*************************meilai************************************
            Else
                lnkTC.Visible = False
            End If

        End If
    End Sub

    Private Function GetInput() As Company
        Dim objCompDetails As New Company
        objCompDetails.CoyId = txtComID.Text
        objCompDetails.CoyName = txtComName.Text
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
        objCompDetails.BusinessRegNo = txtBusinessRegNo.Text
        objCompDetails.TaxRegNo = txtGSTRegNo.Text
        objCompDetails.PwdDuration = txtPwdDur.Text
        objCompDetails.BusinessRegNo = txtBusinessRegNo.Text
        objCompDetails.TaxRegNo = txtGSTRegNo.Text

        objCompDetails.Status = optStatus.SelectedValue
        objCompDetails.TaxCalBy = optGSTCalcBy.SelectedValue
        If optBCM.SelectedValue = "0" Then
            objCompDetails.BCMSetting = optBCM.SelectedValue
        Else
            objCompDetails.BCMSetting = txtBCMSetting.Text
        End If
        objCompDetails.FinDeptMode = optFinMode.SelectedValue
        objCompDetails.InvAppr = optInvAppr.SelectedValue
        objCompDetails.PrivLabeling = optPrivateLbl.SelectedValue
        objCompDetails.Skins = optSkin.SelectedValue
        objCompDetails.TrainDemo = IIf(chktraining.Checked, "Y", "N")

        objCompDetails.Country = cboCountry.SelectedValue
        objCompDetails.State = cboState.SelectedValue
        objCompDetails.Currency = cboCurrency.SelectedValue

        Dim objfile As New FileManagement
        Dim strFile As String
        If cmdCompLogo.PostedFile.FileName <> "" Then

            Dim objDB As New  EAD.DBCom(ConfigurationSettings.AppSettings("eRFPPath"))
            Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'ImageAttachSize'"))

            If cmdCompLogo.PostedFile.ContentLength > 0 And cmdCompLogo.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                strFile = objfile.uploadCoyLogo(cmdCompLogo, EnumUploadFrom.FrontOff, Session("CompanyId"), ConfigurationSettings.AppSettings("eRFPPath"))
                objCompDetails.CoyLogo = strFile
                ViewState("CoyLogo") = objCompDetails.CoyLogo
            ElseIf cmdCompLogo.PostedFile.ContentLength = 0 Then
                Throw New Exception("0 byte document or file not found")
            Else
                Throw New Exception("File exceeds maximum file size")
            End If
        Else
            objCompDetails.CoyLogo = ViewState("CoyLogo")
        End If

        Dim strActualTC, strHubTC As String
        'strFile = objFile.FileUpload(cmdTCBrow, EnumUploadType.TermAndCond, "", EnumUploadFrom.FrontOff, "")
        If cmdTCBrow.PostedFile.FileName <> "" Then

            Dim objDB As New  EAD.DBCom(ConfigurationSettings.AppSettings("eRFPPath"))
            Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))

            If cmdTCBrow.PostedFile.ContentLength > 0 And cmdTCBrow.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                objfile.uploadCoyTC(cmdTCBrow, EnumUploadFrom.FrontOff, Session("CompanyId"), strActualTC, strHubTC, ConfigurationSettings.AppSettings("eRFPPath"))
                'lnkTC.Text = strActualTC
                objCompDetails.Actual_TC = strActualTC
                objCompDetails.TC = strHubTC
                ViewState("Actual_TC") = objCompDetails.Actual_TC
                ViewState("TC") = objCompDetails.TC
            ElseIf cmdTCBrow.PostedFile.ContentLength = 0 Then
                Throw New Exception("0 byte document or file not found")
            Else
                Throw New Exception("File exceeds maximum file size")
            End If
        Else
            objCompDetails.Actual_TC = ViewState("Actual_TC")
            objCompDetails.TC = ViewState("TC")
        End If

        If objCompDetails.Actual_TC <> "" Then

            '**************meilai 25/2/05******************************
            'lnkTC.NavigateUrl = "../FileDownLoad.aspx?file=" & Server.UrlEncode(objCompDetails.TC) & "&actualfile=" & Server.UrlEncode(objCompDetails.Actual_TC) & "&type=" & EnumDownLoadType.TermAndCond
            lnkTC.Text = objfile.getAttachPath(Server.UrlEncode(objCompDetails.Actual_TC), objCompDetails.Actual_TC, Server.UrlEncode(objCompDetails.TC), EnumDownLoadType.TermAndCond, "", EnumUploadFrom.FrontOff, ConfigurationSettings.AppSettings("eRFPPath"))
            '**************meilai**************************************
        End If

        objfile = Nothing

        Return objCompDetails
    End Function

    Private Sub cmdsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Try
            objCompDetails = GetInput()

            intMsg = objComp.UpdateComDetails(viewstate("ComID"), objCompDetails)
            DislayLogo(objCompDetails.CoyId)

            Select Case intMsg
                Case WheelMsgNum.Save
                    Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information, "eRFP")
                Case WheelMsgNum.NotSave
                    Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information, "eRFP")
                Case WheelMsgNum.Duplicate
                    Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Information, "eRFP")
                Case 0
                    Common.NetMsgbox(Me, "You Are Not Allow To Modify This Page", MsgBoxStyle.Information, "eRFP")
            End Select
        Catch ex As Exception
            Common.NetMsgbox(Me, ex.Message, MsgBoxStyle.Exclamation, "eRFP")
        End Try
    End Sub

    Private Sub cmdApprove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApprove.Click

        Dim Str As String
        Dim Status As Integer
        Dim coyID As String
        Dim HubAdmin As String
        Dim Buyer As String
        Dim sCompType As String = objComp.GetCompanyType
        'Dim strRedirectURL As String = "ViewPublicVendorRegApprHubadmin.aspx"
        Dim strRedirectURL As String = dDispatcher.direct("PublicVendorReg", "ViewPublicVendorRegApprHubadmin.aspx", "")
        'If sCompType = "BUYER" Then

        'End If

        Try
            objCompDetails = GetInput()

            intMsg = objComp.AddInCompMSTR(viewstate("ComID"))
            'If sCompType = "BUYER" Then

            HubAdmin = objComp.UpdatePublicRegHub(objCompDetails, viewstate("ComID"))
            'ElseIf coyID Is "BUYER" Then
            '    Buyer = objComp.UpdatePublicRegBuyer(objCompDetails, viewstate("ComID"))
            'End If
            Status = VendorRegApprStatus.Approved
            'Str = objComp.updateStatusHubApproval(viewstate("ComID"))

            'Select Case intMsg
            '    Case WheelMsgNum.Save
            '        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information, "eRFP")
            '    Case WheelMsgNum.NotSave
            '        Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information, "eRFP")
            '    Case WheelMsgNum.Duplicate
            '        Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Information, "eRFP")
            '    Case 0
            '        Common.NetMsgbox(Me, "You Are Not Allow To Modify This Page", MsgBoxStyle.Information, "eRFP")
            'End Select

            Common.NetMsgbox(Me, "Approved Successfully.", strRedirectURL)
        Catch ex As Exception
            Common.NetMsgbox(Me, ex.Message, MsgBoxStyle.Exclamation, "eRFP")
        End Try
    End Sub

    Private Sub cmdReject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReject.Click

        Dim Status As String
        Dim Str As String
        'Dim strRedirectURL2 As String = "ViewPublicVendorRegApprHubadmin.aspx"
        Dim strRedirectURL2 As String = dDispatcher.direct("PublicVendorReg", "ViewPublicVendorRegApprHubadmin.aspx", "")
        Status = VendorRegApprStatus.Reject
        Str = objComp.updateStatusHubReject(viewstate("ComID"), Status)
        Common.NetMsgbox(Me, "Registration is Rejected.", strRedirectURL2)
    End Sub

    Sub DislayLogo(ByVal strCoyID As String)

        'Dim objFile As New RFPFileManageClass
        'Dim objfile As New FileManagement
        Dim objFile As New ERFP.Components.RFPFileManageClass
        Dim strImgSrc As String
        'strImgSrc = objfile.getCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyId"))
        strImgSrc = objFile.getTempCoyLogo(EnumUploadFrom.FrontOff, strCoyID, ConfigurationSettings.AppSettings("eRFPPath"))
        If strImgSrc <> "" Then
            Image1.Visible = True
            Image1.ImageUrl = strImgSrc
            lblLogo.Visible = False
        Else
            Image1.Visible = False
            lblLogo.Visible = True
        End If
        'objFile = Nothing
    End Sub

    Private Sub cbocountry_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCountry.SelectedIndexChanged
        Dim objGlobal As New AppGlobals
        'If cbocountry.SelectedIndex <> -1 Then
        objGlobal.FillState(cboState, cboCountry.SelectedItem.Value)
        'End If
    End Sub
End Class

