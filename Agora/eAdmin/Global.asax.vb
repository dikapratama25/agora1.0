Imports System.Web
Imports System.Web.SessionState


Public Class [Global]
    Inherits System.Web.HttpApplication

#Region " Component Designer Generated Code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
    End Sub

#End Region

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
        Session.Timeout = 30

    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
        Dim ex As Exception = Server.GetLastError
        '//Only check for unhandle exception
        '//Other Exception is checked on individual function
        If ex.GetType() Is GetType(HttpUnhandledException) Then
            If Not ex Is Nothing Then
                '//Because this is a UnhandledException
                '//the Orginal Exception is wrapped
                If Not ex.GetBaseException Is Nothing Then
                    AgoraLegacy.Common.TrwExp(ex.GetBaseException)
                End If
            End If
        End If
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        '' Fires when the session ends
        ''Dim o As New Wheel.Components.Users()
        ''o.Logout()
        ''o = Nothing
        ''Session.Abandon()
        ''Dim objDb1 As New  EAD.DBCom()
        'Dim ctx As HttpContext = HttpContext.Current
        'Dim lsSql As String
        'lsSql = "UPDATE USERS_MSTR SET UM_LOGOUT='Y' WHERE UM_USER_ID='moofh'"
        ''objDb1.Execute(lsSql)
        'Context.Response.Redirect(Context.Request.ApplicationPath & "/WebForm1.aspx")

    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

End Class
