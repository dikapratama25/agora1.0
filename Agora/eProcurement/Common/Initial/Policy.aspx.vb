Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions
Imports SSO.Component
Imports System.IO

Public Class Policy
    Inherits AgoraLegacy.AppBaseClass
    Dim UserID, CompID As String

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.cmdDecline.Attributes.Add("onclick", "window.close(); ")

        UserID = Me.Request.QueryString("userid")
        CompID = Me.Request.QueryString("cid")

        Dim PolicyPath As String = ConfigurationSettings.AppSettings("Policy")

        frame1.Attributes.Add("src", PolicyPath)
    End Sub

    Private Sub cmdDecline_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDecline.Click
        If Session("Decline") = "Agree" Then
        Else
            Session("Decline") = "Decline"
        End If
    End Sub

    Private Sub cmdAgree_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAgree.Click
        Dim strscript As New System.Text.StringBuilder
        Dim objUser As New Users

        Session("Decline") = "Agree"

        objUser.UpdatePolicyAgreeDt(UserID, CompID) 'Update login time

        strscript.Append("<script language=""javascript"">")
        strscript.Append("document.getElementById('cmdDecline').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script4", strscript.ToString())
    End Sub
End Class