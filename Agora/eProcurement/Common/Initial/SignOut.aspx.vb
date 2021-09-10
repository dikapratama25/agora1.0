Imports SSO.Component
Imports System.Web.Security
Imports AgoraLegacy

Public Class SignOut
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim strArray(0) As String
        Dim objUsers As New Users
        Dim objDB As New EAD.DBCom
        objUsers.Log_UserActivity(strArray, WheelModule.SecurityControl, WheelUserActivity.Logout)
        objDB.BatchExecute(strArray)

        signOut()
        ''If (Not IsPostBack And (IsDBNull(Request.Cookies("__LOGINCOOKIE__")) Or Request.Cookies("__LOGINCOOKIE__") Is Nothing)) Then
        ''    'At this point, we do not know if the session ID that we have is a new
        ''    'session ID or if the session ID was passed by the client.
        ''    'Update the session ID.
        ''    signOut()
        ''End If

        ''Try
        ''    Dim ticket As FormsAuthenticationTicket
        ''    ticket = FormsAuthentication.Decrypt(Request.Cookies("__LOGINCOOKIE__").Value)

        ''    If IsDBNull(ticket) Then
        ''        RemoveRedirCookie()
        ''    Else
        ''        If ticket.Expired Then
        ''            RemoveRedirCookie()
        ''        End If
        ''    End If
        ''Catch ex As Exception
        ''    AddRedirCookie()
        ''    Response.Redirect(Request.Path)
        ''End Try
    End Sub

    Public Sub signOut()
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
        ''Response.Cookies.Add(New HttpCookie("ASP.NET_SessionId", ""))

        '''To make sure that the client clears the session ID cookie, respond to the client to tell
        '''it that we have responded. To do this, set another cookie.
        ''AddRedirCookie()
        ''Response.Redirect(Request.Path)
    End Sub

    Private Sub AddRedirCookie()
        Dim ticket As FormsAuthenticationTicket
        ticket = New FormsAuthenticationTicket(1, "Test", DateTime.Now, DateTime.Now.AddSeconds(5), False, "")

        Dim encryptedText As String = FormsAuthentication.Encrypt(ticket)
        Response.Cookies.Add(New HttpCookie("__LOGINCOOKIE__", encryptedText))
    End Sub

    Private Sub RemoveRedirCookie()
        Response.Cookies.Add(New HttpCookie("__LOGINCOOKIE__", ""))
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
