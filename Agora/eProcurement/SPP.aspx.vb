Public Partial Class SPP
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Redirect("Common/Initial/Login.aspx")
    End Sub

End Class