
Imports AgoraLegacy
Imports eProcure.Component
Public Class usMassApp
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents chkMassApp As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lnkBack As System.Web.UI.WebControls.Button
    Protected WithEvents txtUserID As System.Web.UI.WebControls.Label
    Protected WithEvents txtUserName As System.Web.UI.WebControls.Label
    Protected WithEvents chkInvMassApp As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblDescription As System.Web.UI.WebControls.Label

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
        alButtonList.Add(cmdSave)
        htPageAccess.Add("add", alButtonList)
        CheckButtonAccess()
        'cmdClear.Enabled = blnCanAdd Or blnCanUpdate Or blnCanDelete
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        If Request.QueryString("pageid") <> "" Then
            strPageId = Request.QueryString("pageid")
        Else
            strPageId = "44"
        End If
        ViewState("UserId") = Request.Params("userid")
        viewstate("UserGroup") = Request.QueryString("usergroup")

        If Not Page.IsPostBack Then
            Populate()

            chkMassApp.Visible = False
            chkInvMassApp.Visible = False

            Dim strUserGroupId() As String = CType(ViewState("UserGroup"), String).Split(",")

            Dim objUserGroup As New UserGroups
            Dim strFixedRole As String = ""
            For Each strUserGroup As String In strUserGroupId
                Dim usrgrp As UserGroup = objUserGroup.GetUserGroupDetails(strUserGroup, EnumAppPackage.eProcure.ToString)
                strFixedRole &= usrgrp.FixedRole & ","
            Next

            viewstate("FixedRole") = strFixedRole

            If viewstate("FixedRole").tostring.IndexOf("Finance Officer") >= 0 _
                And viewstate("FixedRole").tostring.IndexOf("Approving Officer") >= 0 Then

                lblDescription.Text = "Mass Approval authorise approving officer to approve more than one PO or Invoice at  a time."

                chkMassApp.Visible = True
                chkInvMassApp.Visible = True
            Else
                If viewstate("FixedRole").ToString.IndexOf("Finance Officer") >= 0 Then
                    lblDescription.Text = "Mass Approval authorise approving officer to approve more than one Invoice at a   time."
                    chkInvMassApp.Visible = True

                ElseIf viewstate("FixedRole").ToString.IndexOf("Approving Officer") >= 0 Then
                    lblDescription.Text = "Mass Approval authorise approving officer to approve more than one PO at a time."
                    chkMassApp.Visible = True
                End If
            End If
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objUser As New Users

        If viewstate("FixedRole").ToString.IndexOf("Finance Officer") >= 0 Then
            objUser.UpdateInvoiceMassApproval(viewstate("UserId"), chkInvMassApp.Checked)
        End If

        If viewstate("FixedRole").ToString.IndexOf("Approving Officer") >= 0 Then
            objUser.UpdateMassApproval(viewstate("UserId"), chkMassApp.Checked)
        End If

        Common.NetMsgbox(Me, objUser.Message, MsgBoxStyle.Information)
    End Sub

    Private Sub Populate()
        Dim objUser As New Users
        Dim objUserDetails As New User
        Dim ctx As Web.HttpContext = Web.HttpContext.Current
        objUserDetails = objUser.GetUserDetails(viewstate("UserId"), ctx.Session("CompanyId"))
        If Not objUserDetails Is Nothing Then
            txtUserID.Text = objUserDetails.UserID
            txtUserName.Text = objUserDetails.Name
            If objUserDetails.MassApp = "Y" Then
                chkMassApp.Checked = True
            End If
            If objUserDetails.InvoiceMassApp = "Y" Then
                chkInvMassApp.Checked = True
            End If
        Else
            txtUserID.Text = "None"
            txtUserName.Text = "None"
            cmdSave.Enabled = False
            cmdReset.Disabled = True
            chkMassApp.Enabled = False
            chkInvMassApp.Enabled = False
            Common.NetMsgbox(Me, "Invalid User. Please save user before assign mass approval.", MsgBoxStyle.Information)
        End If
        objUserDetails = Nothing
        objUser = Nothing
    End Sub


End Class
