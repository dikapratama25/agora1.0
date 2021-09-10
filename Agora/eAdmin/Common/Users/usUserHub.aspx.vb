#Region " Remark (Please Remark For Major/Flow Changed) "
'******************************************************************************************
'Name   :KK
'Date   :13/09/2004 
'Remark :Changed user mstr to database driven, not longer use join USER_MSTR with JOBTYPE
'        Job title has been changed in Add, delete and update 
'******************************************************************************************
#End Region
Imports System
Imports System.Data.OleDb
Imports AgoraLegacy
Imports SSO.Component



Public Class usUserHub
    Inherits  AgoraLegacy.AppBaseClass
    Dim strMode As String
    Dim objDb As New  EAD.DBCom
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Object Declaration "
    Protected WithEvents txtName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUser As System.Web.UI.WebControls.TextBox
    Protected WithEvents vName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vUser As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vPassword As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cklChemist As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents cklCategory As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents txtStatus As System.Web.UI.WebControls.TextBox
    Protected WithEvents lnkUnlock As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    Protected WithEvents hplAdd As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LnkResetPwd As System.Web.UI.WebControls.LinkButton
    Protected WithEvents divtbl As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents cboCust As System.Web.UI.WebControls.DropDownList
    Protected WithEvents imgClientInfo As System.Web.UI.WebControls.Image
    Protected WithEvents lblClientID As System.Web.UI.WebControls.Label
    Protected WithEvents lblClientName As System.Web.UI.WebControls.Label
    Protected WithEvents lblContractPpl As System.Web.UI.WebControls.Label
    Protected WithEvents lblTel As System.Web.UI.WebControls.Label
    Protected WithEvents lblFaxNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmail As System.Web.UI.WebControls.Label
    Protected WithEvents lblAddress As System.Web.UI.WebControls.Label
    Protected WithEvents lblCustName As System.Web.UI.WebControls.Label
    Protected WithEvents txtEmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCompany As System.Web.UI.WebControls.TextBox
    Protected WithEvents rdAct As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rdDeAct As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkAccLock As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents Button2 As System.Web.UI.WebControls.Button
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdGeneratePwd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents txtPhone As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFax As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblPwdExp As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents rfv_txtUser As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_txtName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_txtEmail As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rev_email As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents vldsumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents Reset1 As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents cmdReset As System.Web.UI.WebControls.Button
    Protected WithEvents cboUserGrp As System.Web.UI.WebControls.DropDownList
    Protected WithEvents revUserId As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents rfv_cboUserGrp As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents txtHP As System.Web.UI.WebControls.TextBox


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
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then
            MyBase.Page_Load(sender, e)
            viewstate("Mode") = Me.Request.QueryString("mode")
            viewstate("UserId") = Me.Request.QueryString("userid")


            Dim objUser As New Users
            Dim objUserDetails As New User

            If viewstate("Mode") = "add" Then
                lblHeader.Text = "Add User Account"
                txtUser.Enabled = True
                cmdReset.Text = "Clear"
                cmdDelete.Visible = False
                cmdGeneratePwd.Visible = False
                chkAccLock.Enabled = False
            ElseIf viewstate("Mode") = "modify" Then
                lblHeader.Text = "Modify User Account"
                cmdReset.Text = "Reset"
                txtUser.Text = viewstate("UserId")
                txtUser.Enabled = False
                objUserDetails = objUser.GetUserDetails(viewstate("UserId"), Session("CompanyIdToken"))

            End If
            Populate(objUserDetails)
        End If
        cmdDelete.Attributes.Add("onclick", "return confirm('" & MsgForDeleteButton & "');")
        cmdReset.Attributes.Add("onclick", "Reset();")

        lnkBack.NavigateUrl = dDispatcher.direct("Users", "UsSearchUserHub.aspx")


    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objUser As New Users
        Dim objUserDetail As New User
        Dim objPwd As New RandomPassword
        Dim objMail As New Email


        objUserDetail = GetInput()
        objUserDetail.Password = objPwd.GenerateAutoPwd()
        objUserDetail.NextExpiredDt = objUser.GetPwdExpireDt(True)

        If viewstate("Mode") = "add" Then
            If objUser.AddUser(objUserDetail, True, EnumAppPackage.All) Then
                objMail.sendNotification(EmailType.NewUserAccount, objUserDetail.UserID, objUserDetail.CompanyID, objUserDetail.CompanyID, "", "", objUserDetail.Password, "")
                ''objMail.sendNotification(EmailType.NewUserAccount, objUserDetail.UserID, objUserDetail.CompanyID, "", "", "", objUserDetail.Password)
                'SendMail(objUserDetail.Email, GetMailFormat(objUserDetail.UserID, objUserDetail.Password, objUserDetail.NextExpiredDt), "New Account Password")
            End If
            Common.NetMsgbox(Me, objUser.Message, MsgBoxStyle.Information)
        ElseIf viewstate("Mode") = "modify" Then
            If objUser.UpdateUser(objUserDetail, True, False) Then

            End If
            Common.NetMsgbox(Me, objUser.Message, dDispatcher.direct("Users", "UsSearchUserHub.aspx", ""), MsgBoxStyle.Information)

        End If



    End Sub

    Private Sub cmdGeneratePwd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGeneratePwd.Click
        Dim objPwd As New RandomPassword
        Dim objMail As New Email
        Dim objUser As New Users
        Dim objUserDetail As New User
        Dim strNewPwd As String

        strNewPwd = objPwd.GenerateAutoPwd()
        objUserDetail = objUser.GetUserDetails(viewstate("UserId"), Session("CompanyIdToken"))

        'User temporary password from Login policy
        If objUser.UpdatePassword(viewstate("UserId"), strNewPwd, Session("CompanyIdToken"), True) Then
            objMail.sendNotification(EmailType.ChangePwd, viewstate("UserId"), Session("CompanyIdToken"), "", "", "", strNewPwd)
            'objMail.sendNotification(EmailType.ChangePwd, viewstate("UserId"), Session("CompanyIdToken"), Session("CompanyIdToken"), "", "", strNewPwd, "")
            'SendMail(objUserDetail.Email, GetMailFormat(objUserDetail.UserID, strNewPwd, objUser.GetPwdExpireDt(True)), "New Generated Password")
            'Common.NetMsgbox(Me, objUser.Message, MsgBoxStyle.Information, "Wheel")
            Common.NetMsgbox(Me, "Password generated.", MsgBoxStyle.Information)
        End If
    End Sub

    ''Name       : GetPwdExpireDt
    ''Author     : kk
    ''Descption  : 
    ''Remark     : 
    ''ReturnValue: Get next expire date for password 
    ''LastUpadte : 22 Nox 2004
    ''Version    : 1.00
    'Private Function GetPwdExpireDt() As String
    '    Dim iNewPwdSpan As Integer
    '    Dim objLPolicy As New LoginPolicy

    '    iNewPwdSpan = objLPolicy.getParamValue("NEW_PASSWORD_DURATION")
    '    If iNewPwdSpan = Nothing Then
    '        iNewPwdSpan = 7
    '    End If

    '    Return DateAdd(DateInterval.Day, iNewPwdSpan - 1, Today)

    'End Function

    Private Function GetInput() As User
        Dim objUserDetail As New User
        objUserDetail.UserID = txtUser.Text
        objUserDetail.Name = txtName.Text
        objUserDetail.UserGroup = cboUserGrp.SelectedValue
        objUserDetail.Email = txtEmail.Text
        objUserDetail.CompanyID = Session("CompanyIdToken")
        If rdAct.Checked = True Then
            objUserDetail.Status = "A"
        Else
            objUserDetail.Status = "I"
        End If
        If chkAccLock.Checked = True Then
            objUserDetail.DeleteInd = "S"
        Else
            objUserDetail.DeleteInd = "N"
        End If
        GetInput = objUserDetail

    End Function


    Public Sub Populate(ByVal pUser As User)
        Dim objCompDetails As New Company
        Dim objComp As New Companies
        Dim objUserGroup As New UserGroup
        Dim objDbcom As New  EAD.DBCom

        txtUser.Text = pUser.UserID
        txtName.Text = pUser.Name
        txtEmail.Text = pUser.Email

        'Common.FillDefault(cboUserGrp, "USER_GROUP_MSTR", "UGM_USRGRP_NAME", "UGM_USRGRP_ID", "---Select---", "UGM_FIXED_ROLE IN ('" & FixedRole.Buyer_Administrator & "','" & FixedRole.Vendor_Administrator & "') ")
        objCompDetails = objComp.GetCompanyDetails(Session("CompanyIdToken"))

        'If objCompDetails.CoyType = "VENDOR" Then
        '    Dim strSQL As String = "SELECT * FROM USER_GROUP_MSTR WHERE UGM_DELETED='N' AND UGM_FIXED_ROLE='" & Replace(System.Enum.GetName(GetType(FixedRole), FixedRole.Vendor_Administrator), "_", " ") & "'"
        '    Dim dv As DataView = objDbcom.GetView(strSQL)
        '    Common.FillDdl(cboUserGrp, "UGM_USRGRP_NAME", "UGM_USRGRP_ID", dv, "---Select---")
        'ElseIf objCompDetails.CoyType = "BUYER" Then
        '    Dim strSQL As String = "SELECT * FROM USER_GROUP_MSTR WHERE UGM_DELETED='N' AND UGM_FIXED_ROLE='" & Replace(System.Enum.GetName(GetType(FixedRole), FixedRole.Buyer_Administrator), "_", " ") & "'"
        '    Dim dv As DataView = objDbcom.GetView(strSQL)
        '    Common.FillDdl(cboUserGrp, "UGM_USRGRP_NAME", "UGM_USRGRP_ID", dv, "---Select---")
        'ElseIf objCompDetails.CoyType = "BOTH" Then
        '    Dim strSQL As String = "SELECT * FROM USER_GROUP_MSTR WHERE UGM_DELETED='N' AND UGM_FIXED_ROLE IN ('" & Replace(System.Enum.GetName(GetType(FixedRole), FixedRole.Buyer_Administrator), "_", " ") & "','" & Replace(System.Enum.GetName(GetType(FixedRole), FixedRole.Vendor_Administrator), "_", " ") & "')"
        '    Dim dv As DataView = objDbcom.GetView(strSQL)
        '    Common.FillDdl(cboUserGrp, "UGM_USRGRP_NAME", "UGM_USRGRP_ID", dv, "---Select---")
        'ElseIf objCompDetails.CoyType = "HUB" Then

        'End If

        If objCompDetails.CoyType = "HUB" Then
            ' Not applicable yet
        Else
            Dim strSQL As String = "SELECT DISTINCT UGM_USRGRP_NAME, UGM_USRGRP_ID FROM USER_GROUP_MSTR WHERE UGM_DELETED = 'N' AND UGM_FIXED_ROLE = '" & FixedRole.Super_Admin.ToString.Replace("_", " ") & "' AND UGM_TYPE = 'All'"
            Dim dv As DataView = objDbcom.GetView(strSQL)
            Common.FillDdl(cboUserGrp, "UGM_USRGRP_NAME", "UGM_USRGRP_ID", dv, "---Select---")
        End If


        'Dim lst As New ListItem

        'cboUserGrp.Items.Clear()

        'cboUserGrp.Items.Insert(0, "---Select---")
        'lst.Text = FixedRole.Admin.ToString.Replace("_", " ")
        'lst.Value = FixedRole.Admin.ToString
        'cboUserGrp.Items.Insert(1, lst)

        Common.SelDdl(pUser.UserGroup, cboUserGrp)

        If pUser.Status = "A" Then
            rdAct.Checked = True
        ElseIf pUser.Status = "I" Then  'Delete_Ind, Check for company active status
            rdDeAct.Checked = True
        End If

        If pUser.DeleteInd = "S" Then
            chkAccLock.Checked = True
        Else
            chkAccLock.Checked = False
        End If
    End Sub


    Private Sub SetDDL(ByRef pCombo As DropDownList, ByVal pText As String)
        Dim item As ListItem
        For Each item In pCombo.Items
            If item.Value = pText Then
                item.Selected = True
                Exit For
            End If
        Next
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim strMsg As String
        Dim objUser As New Users
        If objUser.DelUser(viewstate("UserId"), True) Then
            EnableInput(False)
        End If
        Common.NetMsgbox(Me, objUser.Message, MsgBoxStyle.Information)
    End Sub

    Private Sub EnableInput(ByVal pBoo As Boolean)
        txtUser.Enabled = pBoo
        txtName.Enabled = pBoo
        cboUserGrp.Enabled = pBoo
        rdAct.Enabled = pBoo
        rdDeAct.Enabled = pBoo
        txtEmail.Enabled = pBoo
        chkAccLock.Enabled = pBoo
        cmdSave.Enabled = pBoo
        cmdGeneratePwd.Enabled = pBoo
        cmdDelete.Enabled = pBoo
        cmdReset.Enabled = pBoo
    End Sub

   


    Private Sub cmdGeneratePwd_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles cmdGeneratePwd.Command

    End Sub
End Class
