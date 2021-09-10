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



Public Class usUserHub2
    Inherits AgoraLegacy.AppBaseClass
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
    Protected WithEvents dgUserGroup As System.Web.UI.WebControls.DataGrid
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

        MyBase.Page_Load(sender, e)
        MyBase.blnPaging = False
        SetGridProperty(dgUserGroup)

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

    Private Sub Bindgrid(ByVal pUserId As String, ByVal pCompanyId As String, ByVal pCoyType As String)
        Dim strSQL As String
        Dim ds As New DataSet

        If pCoyType <> "BOTH" Then
            strSQL = "SELECT USERS_USRGRP.UU_USRGRP_ID, APPLICATION_MSTR.AP_APP_NAME, APPLICATION_MSTR.AP_APP_ID," & _
                " '" & pCoyType & "' AS USER_TYPE" & _
                " FROM USERS_USRGRP INNER JOIN" & _
                " USER_GROUP_MSTR ON USERS_USRGRP.UU_USRGRP_ID = USER_GROUP_MSTR.UGM_USRGRP_ID AND" & _
                " USERS_USRGRP.UU_APP_PKG = USER_GROUP_MSTR.UGM_APP_PKG AND" & _
                " USER_GROUP_MSTR.UGM_DELETED = 'N' AND" & _
                " USER_GROUP_MSTR.UGM_FIXED_ROLE = '" & FixedRole.Super_Admin.ToString.Replace("_", " ") & "' AND" & _
                " USER_GROUP_MSTR.UGM_TYPE = '" & pCoyType & "' RIGHT OUTER JOIN" & _
                " COMPANY_APPLICATION INNER JOIN" & _
                " APPLICATION_MSTR ON COMPANY_APPLICATION.CA_APP_ID = APPLICATION_MSTR.AP_APP_ID ON" & _
                " USERS_USRGRP.UU_APP_PKG = COMPANY_APPLICATION.CA_APP_ID AND USERS_USRGRP.UU_COY_ID = COMPANY_APPLICATION.CA_COY_ID AND" & _
                " USERS_USRGRP.UU_USER_ID = '" & pUserId & "'" & _
                " WHERE (COMPANY_APPLICATION.CA_COY_ID = '" & pCompanyId & "')" & _
                " ORDER BY AP_APP_NAME, USER_TYPE"
        Else
            strSQL = "SELECT USERS_USRGRP.UU_USRGRP_ID, APPLICATION_MSTR.AP_APP_NAME, APPLICATION_MSTR.AP_APP_ID," & _
                " 'VENDOR' AS USER_TYPE" & _
                " FROM USERS_USRGRP INNER JOIN" & _
                " USER_GROUP_MSTR ON USERS_USRGRP.UU_USRGRP_ID = USER_GROUP_MSTR.UGM_USRGRP_ID AND" & _
                " USERS_USRGRP.UU_APP_PKG = USER_GROUP_MSTR.UGM_APP_PKG AND" & _
                " USER_GROUP_MSTR.UGM_DELETED = 'N' AND" & _
                " USER_GROUP_MSTR.UGM_FIXED_ROLE = '" & FixedRole.Super_Admin.ToString.Replace("_", " ") & "' AND" & _
                " USER_GROUP_MSTR.UGM_TYPE = 'VENDOR' RIGHT OUTER JOIN" & _
                " COMPANY_APPLICATION INNER JOIN" & _
                " APPLICATION_MSTR ON COMPANY_APPLICATION.CA_APP_ID = APPLICATION_MSTR.AP_APP_ID ON" & _
                " USERS_USRGRP.UU_APP_PKG = COMPANY_APPLICATION.CA_APP_ID AND USERS_USRGRP.UU_COY_ID = COMPANY_APPLICATION.CA_COY_ID AND" & _
                " USERS_USRGRP.UU_USER_ID = '" & pUserId & "'" & _
                " WHERE (COMPANY_APPLICATION.CA_COY_ID = '" & pCompanyId & "')"

            strSQL &= " UNION "

            strSQL &= "SELECT USERS_USRGRP.UU_USRGRP_ID, APPLICATION_MSTR.AP_APP_NAME, APPLICATION_MSTR.AP_APP_ID," & _
                " 'BUYER' AS USER_TYPE" & _
                " FROM USERS_USRGRP INNER JOIN" & _
                " USER_GROUP_MSTR ON USERS_USRGRP.UU_USRGRP_ID = USER_GROUP_MSTR.UGM_USRGRP_ID AND" & _
                " USERS_USRGRP.UU_APP_PKG = USER_GROUP_MSTR.UGM_APP_PKG AND" & _
                " USER_GROUP_MSTR.UGM_DELETED = 'N' AND" & _
                " USER_GROUP_MSTR.UGM_FIXED_ROLE = '" & FixedRole.Super_Admin.ToString.Replace("_", " ") & "' AND" & _
                " USER_GROUP_MSTR.UGM_TYPE = 'BUYER' RIGHT OUTER JOIN" & _
                " COMPANY_APPLICATION INNER JOIN" & _
                " APPLICATION_MSTR ON COMPANY_APPLICATION.CA_APP_ID = APPLICATION_MSTR.AP_APP_ID ON" & _
                " USERS_USRGRP.UU_APP_PKG = COMPANY_APPLICATION.CA_APP_ID AND USERS_USRGRP.UU_COY_ID = COMPANY_APPLICATION.CA_COY_ID AND" & _
                " USERS_USRGRP.UU_USER_ID = '" & pUserId & "'" & _
                " WHERE (COMPANY_APPLICATION.CA_COY_ID = '" & pCompanyId & "')" & _
                " ORDER BY AP_APP_NAME, USER_TYPE"
        End If

        ds = objDb.FillDs(strSQL)

        intPageRecordCnt = ds.Tables(0).Rows.Count

        If intPageRecordCnt > 0 Then
            dgUserGroup.DataSource = ds
            dgUserGroup.DataBind()
        Else
            dgUserGroup.DataBind()
        End If
    End Sub

    Protected Function IsConfigured(ByVal obj As Object)
        If obj Is DBNull.Value Then
            Return False
        ElseIf obj.ToString() <> "" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objUser As New Users
        Dim objUserDetail As New UserUsrGrp
        Dim objPwd As New RandomPassword
        Dim objMail As New Email

        objUserDetail = GetInput()
        objUserDetail.UserDetail.Password = objPwd.GenerateAutoPwd()
        objUserDetail.UserDetail.NextExpiredDt = objUser.GetPwdExpireDt(True)

        If viewstate("Mode") = "add" Then
            If objUser.AddUser(objUserDetail, True) Then
                objMail.sendNotification(EmailType.NewUserAccount, objUserDetail.UserDetail.UserID, objUserDetail.CompanyId, objUserDetail.CompanyId, "", "", objUserDetail.UserDetail.Password, "")
                Common.NetMsgbox(Me, objUser.Message, Request.Path & "?mode=modify&userid=" & Server.UrlEncode(objUserDetail.UserDetail.UserID), MsgBoxStyle.Information)
            Else
                Common.NetMsgbox(Me, objUser.Message, MsgBoxStyle.Information)
            End If
        ElseIf viewstate("Mode") = "modify" Then
            objUser.UpdateUser(objUserDetail, True, True)
            'Common.NetMsgbox(Me, objUser.Message, "UsSearchUserHub.aspx", MsgBoxStyle.Information)
            Common.NetMsgbox(Me, objUser.Message, dDispatcher.direct("Users", "UsSearchUserHub.aspx", ""), MsgBoxStyle.Information)
        End If

        'If viewstate("Mode") = "add" Then
        '    If objUser.AddUser(objUserDetail.UserDetail, True, EnumAppPackage.All) Then
        '        objMail.sendNotification(EmailType.NewUserAccount, objUserDetail.UserDetail.UserID, objUserDetail.CompanyId, objUserDetail.CompanyId, "", "", objUserDetail.UserDetail.Password, "")
        '        ''objMail.sendNotification(EmailType.NewUserAccount, objUserDetail.UserID, objUserDetail.CompanyID, "", "", "", objUserDetail.Password)
        '        'SendMail(objUserDetail.Email, GetMailFormat(objUserDetail.UserID, objUserDetail.Password, objUserDetail.NextExpiredDt), "New Account Password")
        '    End If
        '    Common.NetMsgbox(Me, objUser.Message, MsgBoxStyle.Information)
        'ElseIf viewstate("Mode") = "modify" Then
        '    If objUser.UpdateUser(objUserDetail, True, False) Then

        '    End If
        '    Common.NetMsgbox(Me, objUser.Message, "UsSearchUserHub.aspx", MsgBoxStyle.Information)
        'End If
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

    Private Function GetInput() As UserUsrGrp
        Dim objUserDetail As New UserUsrGrp

        objUserDetail.UserDetail.UserID = txtUser.Text
        objUserDetail.UserDetail.Name = txtName.Text
        'objUserDetail.UserGroup = cboUserGrp.SelectedValue
        objUserDetail.UserDetail.Email = txtEmail.Text
        objUserDetail.CompanyId = Session("CompanyIdToken")

        If rdAct.Checked = True Then
            objUserDetail.UserDetail.Status = "A"
        Else
            objUserDetail.UserDetail.Status = "I"
        End If
        If chkAccLock.Checked = True Then
            objUserDetail.UserDetail.DeleteInd = "S"
        Else
            objUserDetail.UserDetail.DeleteInd = "N"
        End If

        Dim dgItem As DataGridItem
        Dim strAppPackageId As String
        Dim strUserGroupId As String

        For Each dgItem In dgUserGroup.Items
            Dim cboUserGroup As DropDownList = dgItem.FindControl("cboUserGroup")
            strAppPackageId = dgItem.Cells(1).Text
            strUserGroupId = cboUserGroup.SelectedValue

            objUserDetail.AddPackageUserGroup(strAppPackageId, strUserGroupId)
        Next

        Return objUserDetail
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
            Call Bindgrid(txtUser.Text, Session("CompanyIdToken"), objCompDetails.CoyType)
        End If

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
            'EnableInput(False)
            Common.NetMsgbox(Me, objUser.Message, dDispatcher.direct("Users", "UsSearchUserHub.aspx", ""), MsgBoxStyle.Information)
        Else
            Common.NetMsgbox(Me, objUser.Message, MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub EnableInput(ByVal pBoo As Boolean)
        txtUser.Enabled = pBoo
        txtName.Enabled = pBoo
        'cboUserGrp.Enabled = pBoo
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

    Private Sub dgUserGroup_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgUserGroup.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim strAppPackageId As String = e.Item.Cells(1).Text
            Dim strUserType As String = e.Item.Cells(2).Text

            Dim cboUserGroup As DropDownList = e.Item.FindControl("cboUserGroup")
            cboUserGroup.DataSource = GetUserGroupDataView(strAppPackageId, strUserType)
            cboUserGroup.DataTextField = "UGM_USRGRP_NAME"
            cboUserGroup.DataValueField = "UGM_USRGRP_ID"

            cboUserGroup.DataBind()

            Dim lst As New ListItem("---Select---", "")
            cboUserGroup.Items.Insert(0, lst)

            ' Purpose for following commented code is to change the required field validator error message
            'Dim rfvUserGroup As RequiredFieldValidator = e.Item.FindControl("rfv_UserGroup")
            'rfvUserGroup.ErrorMessage = StrConv(strUserType, VbStrConv.ProperCase) & " User Group for " & strAppPackageId & " is required."

            If IsDBNull(CType(e.Item.DataItem, DataRowView)("UU_USRGRP_ID")) Then
                cboUserGroup.SelectedIndex = 0
            Else
                Common.SelDdl(CType(e.Item.DataItem, DataRowView)("UU_USRGRP_ID"), cboUserGroup)
            End If
        End If
    End Sub

    Private Sub dgUserGroup_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgUserGroup.ItemCreated
        Grid_ItemCreated(dgUserGroup, e)
    End Sub

    Private Function GetUserGroupDataView(ByVal pAppPackageId As String, ByVal pUserType As String) As DataView
        Dim strSQl As String

        strSQl = "SELECT UGM_USRGRP_ID, UGM_USRGRP_NAME FROM USER_GROUP_MSTR WHERE " & _
            " UGM_FIXED_ROLE = '" & FixedRole.Super_Admin.ToString.Replace("_", " ") & "' AND " & _
            " UGM_APP_PKG = '" & pAppPackageId & "' AND UGM_TYPE = '" & pUserType & "' AND" & _
            " UGM_DELETED = 'N'"

        Return objDb.GetView(strSQl)
    End Function
End Class
