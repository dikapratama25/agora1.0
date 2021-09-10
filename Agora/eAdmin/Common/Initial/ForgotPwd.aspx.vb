Imports AgoraLegacy
Imports SSO.Component

Public Class forgotPwd
    Inherits AgoraLegacy.AppBaseClass
    Dim objDb As New  EAD.DBCom
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents txtQuestion As System.Web.UI.WebControls.TextBox


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents txtUserId As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblUserId As System.Web.UI.WebControls.Label
    Protected WithEvents txtCompID As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblCompanyId As System.Web.UI.WebControls.Label
    Protected WithEvents txtEmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblEmail As System.Web.UI.WebControls.Label
    Protected WithEvents cboQuestion As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblQuestion As System.Web.UI.WebControls.Label
    Protected WithEvents txtAns As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblAnswer As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents lblMsg3 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdBack As System.Web.UI.WebControls.Button
    Protected WithEvents lblMsg2 As System.Web.UI.WebControls.Label
    Protected WithEvents trSave As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents trRemark As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trBack As System.Web.UI.HtmlControls.HtmlTableRow


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

        'lblMsg2.Text = ""
        lblMsg3.Text = ""
        viewstate("Modes") = Request.Params("modes")
        If viewstate("Modes") = "F" Then
            'New pwd expired
            'lblMsg.Text = "Please Re-Generate Your Password."
            lblInfo.Text = "Your password have to re-generate due to previous new temporary password not activate yet after expired."
        ElseIf viewstate("Modes") = "" Then
            'Click on forgot pwd
            lblInfo.Text = "If you've forgotten the password to your account, " & _
                           " please confirm your identity below. We will" & _
                           " send a new temporary password to you. "
        End If

        If Not Page.IsPostBack Then
            'trBack.Visible = False
            trRemark.Visible = False
            trSave.Visible = True
            cboQuestion.Items.Add("Select An Question.")
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
        End If

    End Sub

   


    'Name       : GetPwdExpireDt
    'Author     : kk
    'Descption  : 
    'Remark     : 
    'ReturnValue: Get next expire date for password 
    'LastUpadte : 22 Nox 2004
    'Version    : 1.00
    Private Function GetPwdExpireDt() As String
        Dim iNewPwdSpan As Integer
        Dim objLPolicy As New LoginPolicy

        iNewPwdSpan = objLPolicy.getParamValue("NEW_PASSWORD_EXPIRING")
        If iNewPwdSpan = Nothing Then
            iNewPwdSpan = 7
        End If

        Return DateAdd(DateInterval.Day, iNewPwdSpan, Today)

    End Function


    Public Function ValidateFields(ByRef pErrMsg As String) As Boolean
        Dim objLPolicy As New LoginPolicy
        Dim objUserDetails As New User
        Dim objUser As New Users
        Dim bTrue As Boolean = True

        'Try
        ValidateFields = True
        If txtUserId.Text = "" Then
            pErrMsg = pErrMsg & "<LI type=square>User ID is required."
            bTrue = False
        End If

        If txtCompID.Text = "" Then
            pErrMsg = pErrMsg & "<LI type=square>Company ID is required."
            bTrue = False
        End If

        If txtEmail.Text = "" Then
            pErrMsg = pErrMsg & "<LI type=square>Email is required."
            bTrue = False
        End If

        'If cboQuestion.SelectedIndex = 0 Then
        '    pErrMsg = pErrMsg & "<LI type=square>Challege phrase required."
        '    bTrue = False
        'End If

        objUserDetails = objUser.GetUserDetails(txtUserId.Text, txtCompID.Text)
        If objUserDetails IsNot Nothing Then
            cboQuestion.SelectedIndex = IIf(objUserDetails.Question = -1, 0, objUserDetails.Question)
            Dim tem As String
            tem = cboQuestion.SelectedItem.Text
            txtQuestion.Text = CStr(tem)
        Else
            txtQuestion.Text = ""
        End If

        If txtAns.Text = "" Then
            pErrMsg = pErrMsg & "<LI type=square>Answer is required."
            bTrue = False
        End If

        If bTrue Then 'only check Database if all required field is filled in.
            objUserDetails = objUser.GetUserDetails(txtUserId.Text, txtCompID.Text)
            If objUserDetails Is Nothing And txtUserId.Text <> "" Then
                pErrMsg = pErrMsg & "<LI type=square>User/Company ID not found."
                bTrue = False
            Else
                If objUserDetails.Email = "" Then
                    pErrMsg = pErrMsg & "<LI type=square>Email address not found."
                    bTrue = False
                ElseIf objUserDetails.Email <> txtEmail.Text Then
                    pErrMsg = pErrMsg & "<LI type=square>Email address not match."
                    bTrue = False
                End If

                'If cboQuestion.SelectedIndex <> objUserDetails.Question Or UCase(txtAns.Text) <> UCase(objUserDetails.Answer) Then
                '    pErrMsg = pErrMsg & "<LI type=square>Incorrect answer for challege phase."
                '    bTrue = False
                'End If
            End If
        End If

        If bTrue Then
            Return True
        Else
            Return False
        End If


        'Catch exp As Exception
        '    Return False
        'End Try
    End Function

    Private Sub displaySuccess()
        Dim strMsg As String
        lblUserId.Text = txtUserId.Text
        lblCompanyId.Text = txtCompID.Text
        lblEmail.Text = txtEmail.Text
        lblAnswer.Text = txtAns.Text
        lblQuestion.Text = Me.cboQuestion.SelectedItem.Text
        txtUserId.Visible = False
        txtCompID.Visible = False
        txtEmail.Visible = False
        txtAns.Visible = False
        cboQuestion.Visible = False
        txtQuestion.Visible = False
        lblMsg2.Visible = False
        strMsg = writeSpace(1) & "<U>Notice</U>: "
        strMsg &= "<BR>" & writeSpace(2) & "1. Check your email to retrieve the new temporary password. "
        strMsg &= "<BR>" & writeSpace(2) & "2. Use the temporary password to login. "
        strMsg &= "<BR>" & writeSpace(2) & "3. Change the temporary password to your preferred password. "
        strMsg &= "<BR>" & writeSpace(2) & "4. If your account has been locked earlier, please contact your Administrator " & writeSpace(2)
        strMsg &= "<BR>" & writeSpace(6) & "to unlock your account before using the temporary password. "
        lblMsg3.Text = strMsg

    End Sub

    Private Function writeSpace(ByVal intCnt As Integer) As String
        Dim str As String = ""
        Dim i As Integer
        For i = 0 To intCnt - 1
            str &= "&nbsp;"
        Next
        writeSpace = str
    End Function

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Response.Redirect(dDispatcher.direct("Initial", "login.aspx", ""))
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim strErrMsg As String

        If ValidateFields(strErrMsg) Then
            Dim strAutoPwd As String
            Dim objLPolicy As New LoginPolicy
            Dim objMail As New Email
            Dim objUsers As New Users
            Dim objPwd As New RandomPassword
            strAutoPwd = objPwd.GenerateAutoPwd()

            'User temporary password from Login policy
            If objUsers.UpdatePassword(txtUserId.Text, strAutoPwd, txtCompID.Text, True) Then
                objMail.sendNotification(EmailType.ChangePwd, txtUserId.Text, txtCompID.Text, "", "", "", strAutoPwd)
                'SendMail(txtEmail.Text, GetMailFormat(txtUserId.Text, strAutoPwd, GetPwdExpireDt()), "New Password")
                'txtUserId.Enabled = False
                'txtEmail.Enabled = False
                'cboQuestion.Enabled = False
                'txtAns.Enabled = False
                'txtCompID.Enabled = False
                'cmdSubmit.Visible = False
                Common.NetMsgbox(Me, "Password has been auto generated and sent to your email account.", MsgBoxStyle.Information)
                'trBack.Visible = True
                trSave.Visible = False
                trRemark.Visible = True
                displaySuccess()
            Else
                Common.NetMsgbox(Me, "Application Error. Please Contact Administrator.", MsgBoxStyle.Information)
            End If
        Else
            lblMsg2.Text = strErrMsg
        End If
    End Sub

    Public Sub txtCompID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCompID.TextChanged
        Dim objUserDetails As New User
        Dim objUser As New Users
        objUserDetails = objUser.GetUserDetails(txtUserId.Text, txtCompID.Text)
        If objUserDetails IsNot Nothing Then
            cboQuestion.SelectedIndex = IIf(objUserDetails.Question = -1, 0, objUserDetails.Question)
            Dim tem As String
            tem = cboQuestion.SelectedItem.Text
            txtQuestion.Text = CStr(tem)
        Else
            txtQuestion.Text = ""
        End If

        'txtCompID.Attributes.Add("onblur", "document.getElementById('" & txtEmail.ClientID & "').focus(); ")
        ClientScript.RegisterStartupScript(txtCompID.GetType(), "onblur", "<script>document.getElementById('" & txtEmail.ClientID & "').focus ();</script>")

    End Sub

    Public Sub txtUserId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUserId.TextChanged
        Dim objUserDetails As New User
        Dim objUser As New Users
        objUserDetails = objUser.GetUserDetails(txtUserId.Text, txtCompID.Text)
        If objUserDetails IsNot Nothing Then
            cboQuestion.SelectedIndex = IIf(objUserDetails.Question = -1, 0, objUserDetails.Question)
            Dim tem As String
            tem = cboQuestion.SelectedItem.Text
            txtQuestion.Text = CStr(tem)
        Else
            txtQuestion.Text = ""
        End If

        'txtCompID.Attributes.Add("onblur", "document.getElementById('" & txtEmail.ClientID & "').focus(); ")
        ClientScript.RegisterStartupScript(txtUserId.GetType(), "onblur", "<script>document.getElementById('" & txtCompID.ClientID & "').focus ();</script>")

    End Sub
End Class
