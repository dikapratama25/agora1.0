Imports System.Data.SqlClient
Imports AgoraLegacy

Public Class ApprovalRules
    Inherits AgoraLegacy.AppBaseClass


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmd_Save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Reset2 As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents opl_RulesOp As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents chkInvAppRule As System.Web.UI.WebControls.CheckBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim objHubAdmin As New HubAdmin
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)

        If Not IsPostBack Then
            Dim dsApprRules As New DataSet
            dsApprRules = objHubAdmin.getApprRules("eProcure")

            If dsApprRules.Tables(0).Rows.Count > 0 Then
                opl_RulesOp.SelectedValue = dsApprRules.Tables(0).Rows(0)(0)
            End If

            Dim dsInvApprRules As New DataSet
            dsInvApprRules = objHubAdmin.getInvApprRules("eProcure")

            If dsInvApprRules.Tables(0).Rows.Count > 0 Then
                chkInvAppRule.Checked = dsInvApprRules.Tables(0).Rows(0)(0)
            End If
        End If
        'Put user code to initialize the page here
    End Sub

    Private Sub cmd_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Save.Click

        Dim objHubAdmin As New HubAdmin
        Dim strRules As String

        objHubAdmin.updateApprRules(opl_RulesOp.SelectedValue, "eProcure")
        objHubAdmin.updateInvApprRules(chkInvAppRule.Checked, "eProcure")

        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
    End Sub
End Class
