Imports System.Web.Security

Partial Class TimeOut
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

    Dim ctx As HttpContext = HttpContext.Current

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'MyBase.Page_Load(sender, e)
        'Put user code to initialize the page here
        '//Put Redirect here
        Dim strURL As String
        'If Session("UserId") = String.Empty Then

        ClearUpSession()
        ClearUpCookies()
        FormsAuthentication.SignOut()
        NetMsgbox("Your session has expired, please login again.", "../../Common/Initial/Login.aspx", MsgBoxStyle.Information)
        'End If
        'strURL = "http://" & Request.ServerVariables("Server_Name") & _
        'Request.ServerVariables("URL") & "?" & Request.ServerVariables("Query_String")
        'Response.Write("<script>parent.location = 'Login.aspx';</script>")
        'Response.Redirect("http://localhost/SSO/Login.aspx?o=" & strURL)
    End Sub

    Public Sub NetMsgbox(ByVal msg As String, ByVal pRedirect As String, Optional ByVal style As MsgBoxStyle = MsgBoxStyle.Exclamation, Optional ByVal title As String = "eProcure")
        Dim vbs As String
        vbs = vbs & "<script language=""vbs"">"
        vbs = vbs & "Call MsgBox(""" & msg & """, " & style & ", """ & title & """)"
        vbs = vbs & vbLf & "parent.location=""" & pRedirect & """"
        vbs = vbs & "</script>"
        Dim rndKey As New Random
        Me.RegisterStartupScript(rndKey.Next.ToString, vbs)
    End Sub

    Private Sub ClearUpSession()
        ctx.Session.Abandon()
        ctx.Session.Clear()
        ctx.Session("UserId") = Nothing
        ctx.Session("UserName") = Nothing
        ctx.Session("CompanyId") = Nothing
        ctx.Session("CompanyName") = Nothing
        ctx.Session("Last_Login") = Nothing
    End Sub

    Private Sub ClearUpCookies()
        ctx.Response.Cookies.Clear()
        ctx.Request.Cookies.Clear()
        'ctx.Request.Cookies.Remove("aHideUserId")
        'ctx.Request.Cookies.Remove("aHideCompanyId")
        'ctx.Request.Cookies.Remove("aPackageName")
        'ctx.Request.Cookies.Remove("aTotalPackage")
        ctx.Response.Cookies("aHideUserId").Expires = DateTime.Now.AddDays(-1)
        ctx.Response.Cookies("aHideCompanyId").Expires = DateTime.Now.AddDays(-1)
        ctx.Response.Cookies("aPackageName").Expires = DateTime.Now.AddDays(-1)
        ctx.Response.Cookies("aTotalPackage").Expires = DateTime.Now.AddDays(-1)
    End Sub
End Class
