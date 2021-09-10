Public Class TimeOut
    Inherits System.Web.UI.Page
    Dim dDispatcher As New AgoraLegacy.dispatcher

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
        '//Put Redirect here
        Dim strURL As String

        'If Session("UserId") = String.Empty Then
        'NetMsgbox("You session has expired,please login again.", "Login.aspx", MsgBoxStyle.Information)
        NetMsgbox("You session has expired,please login again.", dDispatcher.direct("Initial", "Login.aspx"), MsgBoxStyle.Information)
        'End If
        'strURL = "http://" & Request.ServerVariables("Server_Name") & _
        'Request.ServerVariables("URL") & "?" & Request.ServerVariables("Query_String")
        'Response.Write("<script>parent.location = 'Login.aspx';</script>")
        'Response.Redirect("http://localhost/SSO/Login.aspx?o=" & strURL)
    End Sub

    Public Sub NetMsgbox(ByVal msg As String, ByVal pRedirect As String, Optional ByVal style As MsgBoxStyle = MsgBoxStyle.Exclamation, Optional ByVal title As String = "eAdmin")
        Dim vbs As String
        vbs = vbs & "<script language=""vbs"">"
        vbs = vbs & "Call MsgBox(""" & msg & """, " & style & ", """ & title & """)"
        vbs = vbs & vbLf & "parent.location=""" & pRedirect & """"
        vbs = vbs & "</script>"
        Dim rndKey As New Random
        Me.RegisterStartupScript(rndKey.Next.ToString, vbs)
    End Sub
End Class
