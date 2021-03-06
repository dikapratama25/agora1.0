Imports System.IO
Imports System.Text
Imports AgoraLegacy

Public Class FileDownload
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        'Dim strActualFile As String = "453.doc"
        'Response.Clear()
        'Response.ContentType = "application/octet-stream"
        'Response.AddHeader("Content-Disposition", _
        '  "attachment; filename=""" & strActualFile & """")
        'Response.Flush()
        'Response.WriteFile("C:\WheelFileMgnt\ProductDoc\demo\453.doc")


        'Request.Params("file") - filename without path
        'add(param - DocType, uploadType)
        'upload from - hardcode to 'FrontOff'
        Dim intPostBack As Integer = Request.Params("pb")
        Dim filepath As String = Request.Params("file")
        Dim strActualFile As String = Request.Params("actualfile") 'FileName only (not fullpath)
        Dim strDownloadType As String = Request.Params("type")
        Dim strdocType As String = Request.Params("doctype")
        Dim strAppPackage As String = Request.Params("apppackage")
        'Dim strdocNo As String = Request.Params("docno")
        Dim strFileName, strURL As String
        'Dim objFile As New Wheel.Components.FileManagement
        'strFileName = objFile.FileDownload(filepath, strDownloadType, strdocType, Components.EnumUploadFrom.FrontOff, Session("CompanyIdToken"))
        'Response.Clear()
        'Response.ContentType = "application/octet-stream"
        'Response.AddHeader("Content-Disposition", _
        '  "attachment; filename=""" & strActualFile & """")
        'Response.Flush()
        'Response.WriteFile(strFileName)
        'Dim adoApp As Object
        'adoApp = Server.CreateObject("aspSmartUpload.SmartUpload")
        'adoApp = Nothing
        If Not filepath Is Nothing Then
            Dim objFile As New FileManagement
            '//To get fullpath file name

            If strAppPackage = "" Then

                Dim objCompany As New AgoraLegacy.Companies
                Dim dsAppPackage As DataSet = objCompany.getAppPackage(Web.HttpContext.Current.Session("CompanyIdToken"))

                If dsAppPackage.Tables(0).Rows.Count > 0 Then
                    Dim i As Integer

                    For i = 0 To dsAppPackage.Tables(0).Rows.Count - 1
                        If dsAppPackage.Tables(0).Rows(i).Item("chk") = "Y" Then
                            strFileName = objFile.FileDownload(filepath, strDownloadType, strdocType, EnumUploadFrom.FrontOff, Session("CompanyIdToken"), ConfigurationSettings.AppSettings(dsAppPackage.Tables(0).Rows(i).Item("ap_app_id") & "Path"))

                            If strFileName <> "" Then Exit For
                        End If
                    Next
                End If
            Else
                strFileName = objFile.FileDownload(filepath, strDownloadType, strdocType, EnumUploadFrom.FrontOff, Session("CompanyIdToken"), ConfigurationSettings.AppSettings(strAppPackage & "Path"))
            End If

            'strActualFile = "453.doc"
            'strFileName = "C:\WheelFileMgnt\ProductDoc\demo\453.doc"
            If File.Exists(strFileName) Then 'And filepath.StartsWith(root) Then
                'Dim filename As String = Path.GetFileName(strFileName)
                '//Add by Moo, to check file type
                Dim strFileType As String
                Dim ContentTypeS As String
                Dim strCallMethod As String
                strFileType = strActualFile.Substring(strActualFile.IndexOf(".") + 1)
                Select Case strFileType
                    Case "asf"
                        ContentTypeS = "video/x-ms-asf"
                        strCallMethod = "1"
                    Case "avi"
                        ContentTypeS = "video/avi"
                        strCallMethod = "1"
                    Case "doc"
                        ContentTypeS = "application/msword"
                        strCallMethod = "1"
                    Case "zip"
                        ContentTypeS = "application/zip"
                        strCallMethod = "1"
                    Case "xls"
                        ContentTypeS = "application/vnd.ms-excel"
                        strCallMethod = "1"
                    Case "gif"
                        ContentTypeS = "image/gif"
                        strCallMethod = "1"
                    Case "jpg", "jpeg"
                        ContentTypeS = "image/jpeg"
                        strCallMethod = "1"
                    Case "wav"
                        ContentTypeS = "audio/wav"
                        strCallMethod = "1"
                    Case "mp3"
                        ContentTypeS = "audio/mpeg3"
                        strCallMethod = "1"
                    Case "mpg", "mpeg"
                        strCallMethod = "1"
                        ContentTypeS = "video/mpeg"
                    Case "rtf"
                        ContentTypeS = "application/rtf"
                        strCallMethod = "1"
                    Case "htm", "html"
                        ContentTypeS = "text/html"
                        strCallMethod = "1"
                    Case "asp"
                        ContentTypeS = "text/asp"
                        strCallMethod = "1"
                    Case "pdf"
                        ContentTypeS = "application/pdf"
                        strCallMethod = "1"
                    Case "xml"
                        ContentTypeS = "application/xml"
                        strCallMethod = "1"
                    Case "tif"
                        ContentTypeS = "image/tiff"
                        strCallMethod = "1"
                    Case Else
                        ContentTypeS = "application/octet-stream"
                        strCallMethod = "1"
                End Select
                '//Add by Moo, to check file type
                Select Case strCallMethod
                    Case "1"
                        Response.Clear()
                        Response.ContentType = ContentTypeS
                        Response.AddHeader("Content-Disposition", _
                          "attachment; filename=""" & strActualFile & """")
                        Response.Flush()
                        Response.WriteFile(strFileName)
                        Response.End()
                        'Dim adoApp As Object
                        'adoApp = Server.CreateObject("aspSmartUpload.SmartUpload")
                        'adoApp = Nothing
                    Case "2"
                        Dim vbs As String
                        vbs = vbs & "<script language=""vbs"">"
                        vbs = vbs & "Call Test1(" & intPostBack & ",""" & strFileName & """)"
                        vbs = vbs & "</script>"
                        Dim rndKey As New Random
                        Me.RegisterStartupScript(rndKey.Next.ToString, vbs)
                End Select
            Else
                'Response.Write("File Doesn't Exists!")
                lblMsg.Text = "File Doesn't Exists!"
                'Response.Write(Request.ServerVariables("HTTP_REFERER"))
                'strURL = "http://" & Request.ServerVariables("Server_Name") & _
                'Request.ServerVariables("URL") & "?" & Request.ServerVariables("Query_String")
                strURL = Request.ServerVariables("HTTP_REFERER")
                'lnkBack.NavigateUrl = strURL
                lnkBack.NavigateUrl = "javascript:history.back();"
                'Response.Write("<BR><BR><A Href=" & strURL & "> <STRONG>&lt; Back</STRONG> </A>")
            End If
        Else
                'Response.Write("File Doesn't Exists!")
                lblMsg.Text = "File Doesn't Exists!"
                'Response.Write(Request.ServerVariables("HTTP_REFERER"))
                'strURL = "http://" & Request.ServerVariables("Server_Name") & _
                '    Request.ServerVariables("URL") & "?" & Request.ServerVariables("Query_String")
                strURL = Request.ServerVariables("HTTP_REFERER")
                'lnkBack.NavigateUrl = strURL
                lnkBack.NavigateUrl = "javascript:history.back();"
                'Response.Write("<BR><BR><A Href=" & strURL & "> <STRONG>&lt; Back</STRONG> </A>")
            End If
    End Sub
End Class
