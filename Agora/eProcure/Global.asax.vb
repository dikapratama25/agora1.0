Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
        Dim ex As Exception = Server.GetLastError()
        '//Only check for unhandle exception
        '//Other Exception is checked on individual function
        If ex.GetType() Is GetType(HttpUnhandledException) Then
            If Not ex Is Nothing Then
                '//Because this is a UnhandledException
                '//the Orginal Exception is wrapped
                If Not ex.GetBaseException Is Nothing Then
                    Session("innerExceptionMsg") = ex.InnerException.Message
                    AgoraLegacy.Common.TrwExp(ex.GetBaseException, ex.InnerException.Message)
                End If
            End If
        End If
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

End Class