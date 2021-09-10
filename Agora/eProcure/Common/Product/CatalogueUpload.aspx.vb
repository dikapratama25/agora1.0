Imports AgoraLegacy
Imports eProcure.Component
Public Class CatalogueUpload
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents uploadedFile As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Button2 As System.Web.UI.WebControls.Button
    Protected WithEvents File1 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents File2 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents Button3 As System.Web.UI.WebControls.Button
    Protected WithEvents Button4 As System.Web.UI.WebControls.Button
    Protected WithEvents TextBox2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button

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
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim o As New Products()
        Dim o1 As New AppExcel
        Dim strFile1, strFile2, ss As String
        strFile1 = o1.WriteToExcel()
        Common.NetMsgbox(Me, "Finish")
        'Response.Redirect("FileDownload.aspx?file=" & strFile1)
        strFile1 = Server.MapPath(Request.ApplicationPath) & "\xml\" & TextBox1.Text
        strFile2 = Server.MapPath(Request.ApplicationPath) & "\xml\ListPrice.xml"
        o.ValidateProductBatchUpload(strFile2, strFile1, ss)
        Response.Write(ss)
        ss = ""
        strFile1 = Server.MapPath(Request.ApplicationPath) & "\xml\demo.xls"
        strFile2 = Server.MapPath(Request.ApplicationPath) & "\xml\ListPrice.xml"
        o.ValidateProductBatchUpload(strFile2, strFile1, ss)
        Response.Write(ss)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim objFile As New FileManagement
        Dim strary(0) As String
        objFile.FileUpload(uploadedFile, EnumUploadType.DocAttachment, "PR", EnumUploadFrom.FrontOff, "1")
        'objFile.copyTermCondToPO("1")
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim objFile As New FileManagement
        Dim ary(2) As HttpPostedFile
        Dim strAry(0) As String
        ary(0) = uploadedFile.PostedFile
        ary(1) = File1.PostedFile
        ary(2) = File2.PostedFile
        'objFile.uploadProductAttach(ary, EnumUploadFrom.BackOff, "TX123", strAry)
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim objMail As New AppMail
        objMail.MailTo = TextBox2.Text
        objMail.Subject = "ABC"
        objMail.Body = "AAA"
        objMail.SendMail()
    End Sub
End Class
