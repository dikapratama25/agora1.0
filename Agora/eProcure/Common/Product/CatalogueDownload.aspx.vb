Imports AgoraLegacy
Imports eProcure.Component
Public Class CatalogueDownload
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents Button2 As System.Web.UI.WebControls.Button
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
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

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Dim o1 As New WheelExcel()
        Dim strFile1, strFile2, ss As String
        'strFile1 = o1.WriteToExcel()
        'Wheel.Components.Common.NetMsgbox(Me, "Finish")
        strFile1 = "doc1.doc"
        Response.Redirect(dDispatcher.direct("Initial", "FileDownload.aspx", "file=" & strFile1))
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim ctx As Web.HttpContext = Web.HttpContext.Current
        Dim objExcel As New AppExcel
        Dim objDb As New EAD.DBCom
        objExcel.OpenConnToExcel(Server.MapPath(Request.ApplicationPath) & "\xml\moo.xls", True)

        Dim SqlAry(0) As String
        Dim lsSql As String
        Dim intLoop, intLoop1, intTotRow, intTotCol As Integer
        For intLoop = 0 To 10
            lsSql = "Insert into [Sheet1$](Moo,Moo1,Moo2,Moo3) Values(1,1,1,1)"
            Common.Insert2Ary(SqlAry, lsSql)
        Next
        objDb.BatchExecute(SqlAry)
        objDb = Nothing
    End Sub
End Class
