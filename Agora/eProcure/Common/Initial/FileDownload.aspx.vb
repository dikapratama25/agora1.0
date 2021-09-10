Imports System.IO
Imports System.Text
Imports AgoraLegacy
Imports eProcure.Component
Public Class FileDownload
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label

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

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)

        'Dim strActualFile As String = "ProductList.xls"
        'Response.ContentType = "application/octet-stream"
        'Response.AddHeader("Content-Disposition", _
        '  "attachment; filename=""" & strActualFile & """")
        'Response.Flush()
        'Response.WriteFile(Server.MapPath("ProductList.xls"))

        'Request.Params("file") - filename without path
        'add param - DocType,uploadType
        'upload from - hardcode to 'FrontOff'
        Dim intPostBack As Integer = Request.Params("pb")
        Dim filepath As String = Request.Params("file")
        Dim strActualFile As String = Request.Params("actualfile") 'FileName only (not fullpath)
        Dim strDownloadType As String = Request.Params("type")
        Dim strdocType As String = Request.Params("doctype")
        ' ai chu modified on 05/10/2005
        ' when buyer wants to download attachment for item from vendor company, need to pass in vendor company id
        Dim strCoyId As String = Request.Params("coyId")
        'Dim strdocNo As String = Request.Params("docno")
        Dim strFileName, strURL As String
        If Not filepath Is Nothing Then
            Dim objFile As New FileManagement
            '//To get fullpath file name
            If strCoyId <> "" Then
                strFileName = objFile.FileDownload(filepath, strDownloadType, strdocType, EnumUploadFrom.FrontOff, strCoyId)
            Else
                strFileName = objFile.FileDownload(filepath, strDownloadType, strdocType, EnumUploadFrom.FrontOff)
            End If

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
    Sub Test()
        Dim iStream As System.IO.Stream

        ' Buffer to read 10K bytes in chunk:
        Dim buffer(10000) As Byte
        ' Length of the file:
        Dim length As Integer
        ' Total bytes to read:
        Dim dataToRead As Long

        ' Identify the file to download including its path.
        Dim filepath As String = "DownloadFileName"

        ' Identify the file name.
        Dim filename As String = System.IO.Path.GetFileName(filepath)

        Try
            ' Open the file.
            iStream = New System.IO.FileStream(filepath, System.IO.FileMode.Open, _
                                                   IO.FileAccess.Read, IO.FileShare.Read)

            ' Total bytes to read:
            dataToRead = iStream.Length

            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & filename)

            ' Read the bytes.
            While dataToRead > 0
                ' Verify that the client is connected.
                If Response.IsClientConnected Then
                    ' Read the data in buffer
                    length = iStream.Read(buffer, 0, 10000)

                    ' Write the data to the current output stream.
                    Response.OutputStream.Write(buffer, 0, length)

                    ' Flush the data to the HTML output.
                    Response.Flush()

                    ReDim buffer(10000) ' Clear the buffer
                    dataToRead = dataToRead - length
                Else
                    'prevent infinite loop if user disconnects
                    dataToRead = -1
                End If
            End While

        Catch ex As Exception
            ' Trap the error, if any.
            Response.Write("Error : " & ex.Message)
        Finally
            If IsNothing(iStream) = False Then
                ' Close the file.
                iStream.Close()
            End If
        End Try

    End Sub
End Class
