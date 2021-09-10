Imports System.Text
Imports eProcure.Component
Imports AgoraLegacy
Imports System.Text.RegularExpressions
Imports SSO.Component
Imports System.Web.Security

Public Class ChgPassword
    Inherits System.Web.UI.Page
    Dim objDb As New EAD.DBCom
    Dim dDispatcher As New AgoraLegacy.dispatcher
#Region " Object Declaration "
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents txtUserID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOldPW As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNewPW As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtConfNPW As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdClear As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdLogin As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cboQuestion As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtAns As System.Web.UI.WebControls.TextBox
    Protected WithEvents tblQA As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblMsg2 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lbl As System.Web.UI.WebControls.Label
    Protected WithEvents lnkLogin As System.Web.UI.WebControls.HyperLink
    Protected WithEvents rfv_cboQuestion As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cvf_cboQuestion As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents lblLogin As System.Web.UI.WebControls.Label
    Protected WithEvents lblPasswordType As System.Web.UI.WebControls.Label
    Protected WithEvents imgClear As System.Web.UI.WebControls.Image
#End Region

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        lblMsg2.Text = vbNullString
        viewstate("Modes") = Request.Params("modes")
        viewstate("Status") = Request.Params("status")
        viewState("UserId") = Request.Params("userid")
        viewState("CompanyId") = Request.Params("coyid")

        If Not Page.IsPostBack Then
            BindQuestion()
            If viewstate("Modes") = "F" Then
                If viewstate("Status") = "N" Then
                    lblPasswordType.Text = "Temporary"
                    lblMsg.Text = ": Please change the temporary password to your preferred password."
                ElseIf viewstate("Status") = "E" Then
                    lblPasswordType.Text = "Old"
                    lblMsg.Text = ": Please renew password due to password expired."
                End If
                tblQA.Style.Item("display") = ""
            Else
                Dim objUser As New Users
                Dim objUserDetails As New User

                objUserDetails = objUser.GetUserDetails(viewState("UserId"), viewState("CompanyId"))
                cboQuestion.SelectedIndex = objUserDetails.Question
                txtAns.Text = objUserDetails.Answer

                objUser = Nothing
                objUserDetails = Nothing

            End If
            txtUserID.Text = viewState("UserId")

            Dim lngMaxLength As Long = Common.GetPasswordMaxLength(New EAD.DBCom)
            txtOldPW.MaxLength = lngMaxLength
            txtNewPW.MaxLength = lngMaxLength
            txtConfNPW.MaxLength = lngMaxLength
        End If
    End Sub

    Private Sub BindQuestion()
        cboQuestion.Items.Add("---Select---")
        cboQuestion.Items.Add("What is your mother's maiden name?")
        cboQuestion.Items.Add("What is your cat's name?")
        cboQuestion.Items.Add("What is your spouse's name?")
        cboQuestion.Items.Add("What is the color of your car?")
        cboQuestion.Items.Add("What is your favorite football team?")
        cboQuestion.Items.Add("Where did you meet your spouse?")
        cboQuestion.Items.Add("Where did you spend your honeymoon?")
        cboQuestion.Items.Add("What is the first name of your favorite uncle?")
        cboQuestion.Items.Add("What is your oldest cousin's name?")
        cboQuestion.Items.Add("What is your youngest child's nickname?")
        cboQuestion.Items.Add("What is your oldest child's nickname?")
        cboQuestion.Items.Add("What is the first name of your oldest niece?")
        cboQuestion.Items.Add("What is the first name of your oldest nephew?")
        cboQuestion.Items.Add("Who is your all-time favorite movie character?")
        cboQuestion.Items.Add("Who is your favorite author?")
        cboQuestion.Items.Add("What is the name of your favorite book?")
        cboQuestion.Items.Add("What is the last name of your favorite musician?")
        cboQuestion.Items.Add("What is the name of the street on which you grew up?")
        cboQuestion.Items.Add("What is the name of your favorite sports team?")
        cboQuestion.Items.Add("What was your first pet's name?")
        cboQuestion.Items.Add("What was the last name of your favorite teacher?")
    End Sub

    Public Function ValidateFields(ByRef pErrMsg As String) As Boolean
        Dim objLPolicy As New LoginPolicy
        Dim objUser As New Users
        Dim bTrue As Boolean = True
        Dim strSQL, UserPW, OldPW As String
        Dim iPwdMin, iPwdMax, iPwdGeneration As Integer

        iPwdMin = objLPolicy.getParamValue("PASSWORD_LENGTH_MIN")
        iPwdMax = objLPolicy.getParamValue("PASSWORD_LENGTH_MAX")
        iPwdGeneration = objLPolicy.getParamValue("PASSWORD_HISTORY")

        Try
            pErrMsg = "<BR>"
            ValidateFields = True


            strSQL = "SELECT CAST(UM_PASSWORD AS VARCHAR(100)) AS PWD FROM USER_MSTR WHERE UM_USER_ID ='" & viewState("UserId") & "' AND UM_COY_ID='" & viewState("CompanyId") & "' AND UM_DELETED<>'Y'"
            Dim ds As DataSet = objDb.FillDs(strSQL)
            If Not ds Is Nothing Then
                'UserPW = ds.Tables(0).Rows(0).Item("UM_PASSWORD")
                UserPW = ds.Tables(0).Rows(0).Item("PWD")
                ds.Clear()
            End If

            If txtOldPW.Text = "" Then
                pErrMsg &= "<LI type=square>Old Password is required."
                bTrue = False
            Else
                OldPW = Common.EncryptString(Common.Parse(txtOldPW.Text))
                If UserPW <> OldPW Then
                    pErrMsg &= "<LI type=square>Invalid old password"
                    bTrue = False
                End If
            End If

            If txtNewPW.Text = "" Then
                pErrMsg &= "<LI type=square>New Password is required."
                bTrue = False
            Else

            End If

            If txtConfNPW.Text = "" Then
                pErrMsg &= "<LI type=square>Confirm New Password is required."
                bTrue = False
            End If


            If cboQuestion.SelectedIndex = 0 Then
                pErrMsg &= "<LI type=square>Challege phrase is required."
                bTrue = False
            End If

            If txtAns.Text = "" Then
                pErrMsg &= "<LI type=square>Answer is required."
                bTrue = False
            End If

            If txtNewPW.Text <> txtConfNPW.Text Then
                pErrMsg &= "<LI type=square>New password and Confirm new password not identical."
                Return False
            End If

            If (Len(txtNewPW.Text) < iPwdMin And txtNewPW.Text <> "") Or (Len(txtNewPW.Text) > iPwdMax And txtNewPW.Text <> "") Then
                pErrMsg &= "<LI type=square>Password must be " & iPwdMin & " - " & iPwdMax & " alphanumeric."
                bTrue = False
            End If

            If Not IsAlphaNum(Trim(txtNewPW.Text)) Then
                pErrMsg &= "<LI type=square>Password must be alpha-numeric."
                bTrue = False
            ElseIf Not Regex.IsMatch(Trim(txtNewPW.Text), "(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6,15})$") Then
                pErrMsg &= "<LI type=square>Password must be alpha-numeric."
                bTrue = False
            End If

            If objLPolicy.getParamInd("PASSWORD_HISTORY") = "YES" Then
                If Not IsGeneration(Trim(txtNewPW.Text), iPwdGeneration) Then
                    pErrMsg &= "<LI type=square>Your password has been used previously. Your new password cannot be the same as any of your " & iPwdGeneration & " prior passwords."
                    bTrue = False
                End If
            End If
            Return bTrue
        Catch exp As Exception
            Return False
        End Try
    End Function

    Private Function IsGeneration(ByVal pNewPwd As String, ByVal pGeneration As Integer) As Boolean
        Dim strSQL, strHistoryPwd As String
        Dim i As Integer
        Dim objUser As New Users


        strSQL = "SELECT *, CAST(UP_PASSWORD AS VARCHAR(100)) AS PWD FROM USER_PASSWORD_HISTORY " & _
                 " WHERE UP_COY_ID='" & viewState("CompanyId") & "' AND UP_USER_ID='" & viewState("UserId") & "'" & _
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

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim strErrMsg As String
        Dim Query(0) As String

        Dim objUser As New Users
        'Dim OldPW, NewPW, ConfNPW, strSQL, UserPW, DefaultPW, UcaseNewPW As String
        Dim ConfNPW, strSQL As String

        If ValidateFields(strErrMsg) Then
            'NewPW = objUser.EncryptString(Common.Parse(txtNewPW.Text), "[+-*/]")
            'ConfNPW = objUser.EncryptString(Common.Parse(txtConfNPW.Text), "[+-*/]")
            'UcaseNewPW = objUser.EncryptString(Common.Parse(UCase(txtNewPW.Text)), "[+-*/]")
            ConfNPW = txtConfNPW.Text


            'Sync to billing if is super admin
            Dim objrole As New UserRoles
            Dim role As String
            role = objrole.get_UserFixedRoleByParameter(ViewState("CompanyId"), ViewState("UserId"))
            If role.IndexOf("Super Admin") <> -1 Then
                Dim objsync As New CheckAccess
                objsync.syncAccount(ViewState("CompanyId"), ConfNPW)
            End If
            
            objUser.UpdatePwd(viewState("UserId"), viewstate("CompanyId"), ConfNPW, cboQuestion.SelectedIndex, txtAns.Text)
            FormsAuthentication.SignOut()
            Common.NetMsgbox(Me, objUser.Message, dDispatcher.direct("Initial", "Login.aspx"), MsgBoxStyle.Information)
            'cboQuestion.Enabled = False
            'txtAns.Enabled = False
            'txtOldPW.Enabled = False
            'txtNewPW.Enabled = False
            'txtConfNPW.Enabled = False
            'lblLogin.Text = "Please <a href=login.aspx>Login</a> again with the new password."

            ''***** System Audit *****************
            'Dim strChangePwd As String
            'strChangePwd = objSysParam.GetParamInd("AUDIT FOR CHANGE PASSWORD") 'added by kelly on 06/08/2003
            'If strChangePwd = "ON" Then 'if 'ON' then the activity will be recorded down
            '    If objSysParam.CheckUser(Session("user_id")) = False Then   ' never record when user is 'demo'
            '        objLog.InsertLog(LabVis.SystemAudit.AUModules.icCPPassword, _
            '        LabVis.SystemAudit.AUActions.icSubmit, "--", Session("User_ID"), _
            '        "UM_USER_ID", Session("User_ID"), "UM_USER_ID", Session("User_ID"))
            '    End If
            'End If
            '***** System Audit *****************

        Else
            lblMsg2.Text = strErrMsg
        End If
    End Sub

    '''tnc: get from eProcure.UI.vb
    'Name       : IsAlphaNum
    'Author     : Kaithim
    'Descption  : Validation check for AphlaNumeric
    'Remark     :
    'ReturnValue: True for alphanumeric
    'LastUpadte : 19 Sep 2002
    'Version    : 1.00
    Public Function IsAlphaNum(ByVal pCheck As String) As Boolean
        IsAlphaNum = True
        Dim i As Long, j As Long
        If Len(pCheck) <> 0 Then

            Dim strCheck As String = "0123456789"
            strCheck = strCheck & " " & "abcdefghijklmnopqrstuvwxyz"
            ' ai chu modify
            'strCheck = strCheck & "ABCEDFGHIJKLMNOPQRSTUVWXYZ" + "&_.'()"
            strCheck = strCheck & "ABCEDFGHIJKLMNOPQRSTUVWXYZ"

            Dim ch As Char
            For i = 1 To Len(pCheck)
                ch = Mid(pCheck, i, 1)
                For j = 1 To Len(strCheck)
                    If ch = Mid(strCheck, j, 1) Then Exit For
                Next
                If j = (Len(strCheck)) + 1 Then
                    IsAlphaNum = False
                    Exit Function
                End If
            Next
        End If
    End Function
End Class
