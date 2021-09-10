Imports AgoraLegacy
Imports System.Text
Public Class ErrorPage
    Inherits System.Web.UI.Page
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSend As System.Web.UI.WebControls.Button

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
        lblMessage.Text = Session("ErrMsg")
      
    End Sub

    Private Sub cmdSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSend.Click
        Dim m As New appMail
        m.MailTo = ConfigurationSettings.AppSettings("EmailToHubAdmin")
        m.Subject = "Error found in eAdmin"
        m.Body = Session("ErrMsg")
        m.SendMail()
    End Sub
End Class
