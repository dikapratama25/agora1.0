Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions

Imports Access

Public Class PersonalDetails
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    'Dim userEmailOri As String
    Dim blnEmailChange As Boolean = False

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtNewPW As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtConfNPW As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblUserID As System.Web.UI.WebControls.Label
    Protected WithEvents lblDeptName As System.Web.UI.WebControls.Label
    Protected WithEvents txtEmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPhone As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFax As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDesination As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblUserRole As System.Web.UI.WebControls.Label
    Protected WithEvents lblUserGrp As System.Web.UI.WebControls.Label
    Protected WithEvents txtUserName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblExpPW As System.Web.UI.WebControls.Label
    Protected WithEvents hidDeptID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidStatus As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidDelInd As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidAppLimit As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidPOAppLimit As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents vldEmail As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldDesignation As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents lblMsg2 As System.Web.UI.WebControls.Label
    Protected WithEvents hidNewPW As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidConPW As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents revEmail As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCCList As System.Web.UI.WebControls.TextBox
    Protected WithEvents trCCList As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trUserRole As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDept As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cvEmail As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents cboPageCnt As System.Web.UI.WebControls.DropDownList
    Protected WithEvents hidInvAppLimit As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents rdOn As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rdOff As System.Web.UI.WebControls.RadioButton

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
        cmdSave.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdSave)
        htPageAccess.Add("update", alButtonList)
        CheckButtonAccess()
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        lblMsg2.Text = ""

        If Not IsPostBack Then
            Dim objComs As New Companies
            'If Session("Env") = "FTN" Then
            '    Me.lblUserGrp.Text = "User Role"
            'Else
            '    Me.lblUserGrp.Text = "User Group"
            'End If
            Me.lblUserGrp.Text = "User Group"
            ViewState("CompanyType") = UCase(objComs.GetCompanyType(Session("CompanyId")))
            cboPageCnt.Items.Add(10)
            cboPageCnt.Items.Add(20)
            cboPageCnt.Items.Add(30)
            cboPageCnt.Items.Add(40)
            cboPageCnt.Items.Add(50)

            displayPersonalDetail()
            Dim lngMaxLength As Long = Common.GetPasswordMaxLength(New EAD.DBCom)
            txtNewPW.MaxLength = lngMaxLength
            txtConfNPW.MaxLength = lngMaxLength

            If ViewState("CompanyType") = "BOTH" Or ViewState("CompanyType") = "VENDOR" Then
                trCCList.Style("display") = "inline"
            Else
                trCCList.Style("display") = "none"
            End If

            If ViewState("CompanyType") = "VENDOR" Then
                'trUserRole.Style("display") = "none"
                trDept.Style("display") = "none"
            End If
            ' trUserRole.Style("display") = "none"
            GenerateTab()
        End If
    End Sub

    Private Sub displayPersonalDetail()
        Dim objUsers As New Users
        Dim objUser As New User
        Dim strAryUser() As String
        Dim strUser As String = ""
        Dim i As Integer
        Dim dv As New DataView
        objUser = objUsers.GetUserDetails(Session("UserId"), Session("CompanyId"))
        lblUserID.Text = objUser.UserID
        hidDeptID.Value = objUser.DeptID
        txtUserName.Text = objUser.Name
        ViewState("usergroup") = objUser.UserGroupName

        'If Session("Env") <> "FTN" Then
        '    strAryUser = objUser.UserGroupName.Split(",")

        '    For i = 0 To strAryUser.Length - 1
        '        strUser &= strAryUser(i) & "<br>"
        '    Next
        'Else
        '    dv = objUsers.GetUserGroup(Session("UserId"), ViewState("CompanyType"), False)
        '    If Not dv Is Nothing AndAlso dv.Count() > 0 Then
        '        For i = 0 To dv.Count() - 1
        '            If dv.Item(i).Item(1) = "FTN (PO)" Then
        '                strUser = "Purchasing Officer"
        '                Exit For
        '            ElseIf dv.Item(i).Item(1) = "FTN (PM)" Then
        '                strUser = "Purchasing Manager"
        '                Exit For
        '            ElseIf dv.Item(i).Item(1) = "FTN (AO)" Then
        '                strUser = "Approval Officer"
        '                Exit For
        '            ElseIf dv.Item(i).Item(1) = "FTN (SK)" Then
        '                strUser = "Storekeeper"
        '                Exit For
        '            End If
        '        Next
        '    Else
        '        strUser = FixedRole.Super_Admin.ToString.Replace("_", " ")
        '    End If
        'End If

        strAryUser = objUser.UserGroupName.Split(",")

        For i = 0 To strAryUser.Length - 1
            strUser &= strAryUser(i) & "<br>"
        Next

        lblUserRole.Text = strUser
        lblDeptName.Text = objUser.DeptName
        txtEmail.Text = objUser.Email
        Session("userEmailOri") = objUser.Email
        txtPhone.Text = objUser.PhoneNo
        txtFax.Text = objUser.FaxNo
        txtDesination.Text = objUser.Designation
        Common.SelDdl(objUser.PageCount, cboPageCnt, True)

        If ViewState("CompanyType") = "BOTH" Or ViewState("CompanyType") = "VENDOR" Then
            txtCCList.Text = objUser.CCList
        End If

        'kk.remark.28122004.Using several input box to keep several values bcos 
        'the update function is shared. if pass nothing may cos info lost
        hidAppLimit.Value = objUser.ApproveLimit
        hidPOAppLimit.Value = objUser.POApproveLimit
        hidInvAppLimit.Value = objUser.InvoiceApproveLimit
        hidStatus.Value = objUser.Status
        hidDelInd.Value = objUser.DeleteInd
        txtNewPW.Text = objUser.NewPwdInd

        '2016/03/31 - Staff Claim Email Notification On / Off
        If objUser.SCEmailOnOff = "Y" Then
            rdOn.Checked = True
        ElseIf objUser.SCEmailOnOff = "N" Then
            rdOff.Checked = True
        End If

        Dim objComp As New Company
        Dim objComps As New Companies
        objComp = objComps.GetCompanyDetails(Session("CompanyId"))
        lblExpPW.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, objUser.NextExpiredDt)
        txtConfNPW.Text = ""
        objUsers = Nothing
        objUser = Nothing
        objComp = Nothing
        objComps = Nothing
    End Sub

    Private Sub updatePersonalDetail()
        Dim objUser As New User
        Dim objUsers As New Users
        Dim objLP As New LoginPolicy
        Dim intMsgNo, iPwdGeneration As Integer
        Dim strMsg As String

        objUser.UserID = lblUserID.Text
        objUser.Name = txtUserName.Text
        objUser.DeptID = hidDeptID.Value
        objUser.UserGroupName = ViewState("usergroup")
        objUser.Email = txtEmail.Text
        objUser.PhoneNo = txtPhone.Text
        objUser.FaxNo = txtFax.Text
        objUser.Designation = txtDesination.Text
        objUser.ApproveLimit = hidAppLimit.Value
        objUser.POApproveLimit = hidPOAppLimit.Value
        objUser.InvoiceApproveLimit = hidInvAppLimit.Value
        objUser.Status = hidStatus.Value
        objUser.DeleteInd = hidDelInd.Value
        objUser.NextExpiredDt = lblExpPW.Text
        objUser.PageCount = cboPageCnt.SelectedValue
        '2016/03/31 - Staff Claim Email Notification On / Off
        If rdOn.Checked = True Then
            objUser.SCEmailOnOff = "Y"
        Else
            objUser.SCEmailOnOff = "N"
        End If

        If ViewState("CompanyType") = "BOTH" Or ViewState("CompanyType") = "VENDOR" Then
            objUser.CCList = txtCCList.Text
        Else
            objUser.CCList = ""
        End If

        If txtNewPW.Text <> "" Then
            If objLP.getParamInd("PASSWORD_HISTORY") = "YES" Then
                iPwdGeneration = objLP.getParamValue("PASSWORD_HISTORY")
                If Not IsGeneration(Trim(txtNewPW.Text), iPwdGeneration) Then
                    lblMsg2.Text = "<ul type='disc'><li>Your password has been used previously. Your new password cannot be the same as any of your " & iPwdGeneration & " prior passwords.<ul type='disc'></ul></li></ul>"
                    Exit Sub
                End If
            End If
        End If

        Dim role As New ArrayList
        role = Session("MixUserRole")
        If role.IndexOf("Super Admin") <> -1 Then
            Dim objsync As New CheckAccess
            Dim strxml As String
            strxml = "<?xml version='1.0'?><ToProcurement><email>" & txtEmail.Text & "</email><Fax>" & txtFax.Text & "</Fax><Phone>" & txtPhone.Text & "</Phone><CompanyID>" & Session("CompanyId") & "</CompanyID></ToProcurement>"
            objsync.syncDetail(strxml)
        End If

        If txtEmail.Text <> Session("userEmailOri") Then
            blnEmailChange = True
        End If

        If objUsers.UpdateUser(objUser, False, EnumAppPackage.eProcure, False, blnEmailChange) Then
            Session("PageCount") = objUser.PageCount
            If txtNewPW.Text <> "" Then

                If role.IndexOf("Super Admin") <> -1 Then
                    Dim objsync As New CheckAccess
                    objsync.syncAccount(Session("CompanyId"), Trim(txtNewPW.Text))
                End If
                If objUsers.UpdatePassword(objUser.UserID, Trim(txtNewPW.Text), Session("CompanyId"), False) Then
                    Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
                Else
                    Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                End If
            Else
                Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
            End If
            Session("userEmailOri") = txtEmail.Text
        Else
            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
        End If
        objUsers = Nothing
        objUser = Nothing
    End Sub

    Private Function IsGeneration(ByVal pNewPwd As String, ByVal pGeneration As Integer) As Boolean
        Dim strSQL, strHistoryPwd As String
        Dim i As Integer
        Dim objUser As New Users
        Dim objDb As New EAD.DBCom

        strSQL = "SELECT *, CAST(UP_PASSWORD AS VARCHAR(100)) AS PWD FROM USER_PASSWORD_HISTORY " & _
                 " WHERE UP_COY_ID='" & Session("CompanyId") & "' AND UP_USER_ID='" & Session("UserId") & "'" & _
                 " ORDER BY UP_LASTCHANGED_DATE DESC"
        Dim tDS As DataSet = objDb.FillDs(strSQL)
        For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
            If i < pGeneration Then
                'strHistoryPwd = objUser.EncryptString(drdReader.Item("UP_PASSWORD"), "[+-*/]")
                strHistoryPwd = tDS.Tables(0).Rows(j).Item("PWD")
                If Common.EncryptString(pNewPwd) = strHistoryPwd Then
                    Return False
                End If
                i += 1
            Else
                Exit For
            End If
        Next
        Return True

    End Function

    Public Function ValidateFields(ByRef pErrMsg As String) As Boolean
        Dim objpo As New PurchaseOrder
        Dim objval As New POValue
        Dim objLPolicy As New LoginPolicy
        Dim bTrue As Boolean = True
        Dim iPwdMin, iPwdMax, iPwdGeneration As Integer

        iPwdMin = objLPolicy.getParamValue("PASSWORD_LENGTH_MIN")
        iPwdMax = objLPolicy.getParamValue("PASSWORD_LENGTH_MAX")
        iPwdGeneration = objLPolicy.getParamValue("PASSWORD_HISTORY")

        Try
            pErrMsg = ""
            ValidateFields = True

            If txtNewPW.Text <> "" Then
                If Not IsAlphaNum(Trim(txtNewPW.Text)) Then
                    pErrMsg &= "<li>Password must be alpha-numeric."
                    bTrue = False
                ElseIf Not Regex.IsMatch(Trim(txtNewPW.Text), "(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6,15})$") Then
                    pErrMsg &= "<li>Password must be alpha-numeric."
                    bTrue = False
                End If
                If txtConfNPW.Text = "" Then
                    pErrMsg = pErrMsg & "<li>Confirm New Password is required."
                    Return False
                End If
            Else
                If txtConfNPW.Text <> "" Then
                    pErrMsg = pErrMsg & "<li>New Password is required."
                    Return False
                End If
            End If

            If txtConfNPW.Text <> "" And txtNewPW.Text <> "" And txtConfNPW.Text <> txtNewPW.Text Then
                pErrMsg = pErrMsg & "<li>New Password and Confirm New Password must be identical."
                Return False
            End If

            If (Len(txtNewPW.Text) < iPwdMin And txtNewPW.Text <> "") Or (Len(txtNewPW.Text) > iPwdMax And txtNewPW.Text <> "") Then
                pErrMsg &= "<li>Password length must be " & iPwdMin & " - " & iPwdMax & " alpha-numeric."
                bTrue = False
            End If

            Return bTrue
        Catch exp As Exception
            Return False
        End Try
    End Function

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        'Dim objUser As New Users
        Dim strErrMsg As String
        'Dim NewPW, ConfNPW As String
        If Page.IsValid Then
            If ValidateFields(strErrMsg) Then
                'NewPW = objUser.EncryptString(Common.Parse(txtNewPW.Text), "[+-*/]")
                'ConfNPW = objUser.EncryptString(Common.Parse(txtConfNPW.Text), "[+-*/]")
                updatePersonalDetail()
            Else
                lblMsg2.Text = "<ul type='disc'>" & strErrMsg & "<ul type='disc'></ul></li></ul>"
            End If
        End If
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        'If ViewState("CompanyType") = "VENDOR" Then
        '    Session("w_PerDetail_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '                         "<li><a class=""t_entity_btn_selected"" href=""../PersonalSetting/PersonalDetails.aspx?mode=modify&pageid=" & strPageId & """><span>Personal Details</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                        "</ul><div></div></div>"
        'Else
        '    Dim objUserRole As New UserRoles
        '    Dim strFixRole As String

        '    strFixRole = objUserRole.get_UserFixedRole()
        '    If strFixRole.IndexOf("Purchasing Manager") <> -1 Or strFixRole.IndexOf("Purchasing Officer") <> -1 Then
        '        Session("w_PerDetail_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '                             "<li><a class=""t_entity_btn_selected"" href=""../PersonalSetting/PersonalDetails.aspx?mode=modify&pageid=" & strPageId & """><span>Personal Details</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                             "<li><a class=""t_entity_btn"" href=""../Admin/ConfigDefValue.aspx?pageid=" & strPageId & """><span>Configure Default Values</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                           "</ul><div></div></div>"
        '    Else
        '        Session("w_PerDetail_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '                             "<li><a class=""t_entity_btn_selected"" href=""../PersonalSetting/PersonalDetails.aspx?mode=modify&pageid=" & strPageId & """><span>Personal Details</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                            "</ul><div></div></div>"
        '    End If
        'End If
        If ViewState("CompanyType") = "VENDOR" Then
            Session("w_PerDetail_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                                 "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PersonalSetting", "PersonalDetails.aspx", "mode=modify&pageid=" & strPageId) & """><span>Personal Details</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                                "</ul><div></div></div>"
        Else
            Dim objUserRole As New UserRoles
            Dim strFixRole As String

            strFixRole = objUserRole.get_UserFixedRole()
            If strFixRole.IndexOf("Purchasing Manager") <> -1 Or strFixRole.IndexOf("Purchasing Officer") <> -1 Or (strFixRole.IndexOf("Buyer") <> -1 And strFixRole.IndexOf("Buyer Administrator") <> 0) Then
                Session("w_PerDetail_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PersonalSetting", "PersonalDetails.aspx", "mode=modify&pageid=" & strPageId) & """><span>Personal Details</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "ConfigDefValue.aspx", "pageid=" & strPageId) & """><span>Configure Default Settings</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                   "</ul><div></div></div>"

            ElseIf strFixRole.IndexOf("Store Keeper") <> -1 Then
                Session("w_PerDetail_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PersonalSetting", "PersonalDetails.aspx", "mode=modify&pageid=" & strPageId) & """><span>Personal Details</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "ConfigDefValue.aspx", "pageid=" & strPageId) & """><span>Configure Default Settings</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                   "</ul><div></div></div>"

            Else
                Session("w_PerDetail_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PersonalSetting", "PersonalDetails.aspx", "mode=modify&pageid=" & strPageId) & """><span>Personal Details</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                    "</ul><div></div></div>"
            End If
        End If
    End Sub
End Class
