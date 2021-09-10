Imports System.IO
Imports System.Text

Public Class FileDownload1
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Dim strFilePath As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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

        Dim strActualFile As String
        strFilePath = Session("FilePath")
        'strFilePath = CType(context.Items("FilePath"), String)
        strActualFile = Path.GetFileName(strFilePath)
        Response.Clear()
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition", _
          "attachment; filename=""" & strActualFile & """")
        Response.Flush()
        Response.WriteFile(strFilePath)
        Response.End()

    End Sub

End Class
