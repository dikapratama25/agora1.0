Imports System.Web.Security

Public Class Unauthorized
    Inherits System.Web.UI.Page

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
        'Put user code to initialize the page here
        ClearUpSession()
        ClearUpCookies()
        FormsAuthentication.SignOut()
    End Sub

    Private Sub ClearUpSession()
        Session.Abandon()
        Session.Clear()
        Session("UserId") = Nothing
        Session("UserName") = Nothing
        Session("CompanyId") = Nothing
        Session("CompanyName") = Nothing
        Session("Last_Login") = Nothing
        Session("Env") = Nothing
    End Sub

    Private Sub ClearUpCookies()
        Response.Cookies.Clear()
        Request.Cookies.Clear()
        Response.Cookies("aHideUserId").Expires = DateTime.Now.AddDays(-1)
        Response.Cookies("aHideCompanyId").Expires = DateTime.Now.AddDays(-1)
        Response.Cookies("aPackageName").Expires = DateTime.Now.AddDays(-1)
        Response.Cookies("aPackageID").Expires = DateTime.Now.AddDays(-1)
        Response.Cookies("aTotalPackage").Expires = DateTime.Now.AddDays(-1)
    End Sub
End Class
