Imports AgoraLegacy

Public Class ExecutePrinting
    Inherits System.Web.UI.Page

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
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim strType As String
        'strType = Request.QueryString("type")
        Dim strPageId As String
        strPageId = Request.QueryString("pageid")
        With Response
            .Write("<HTML>" & vbCrLf)
            .Write("<HEAD>" & vbCrLf)
            .Write("<TITLE>Invoice</TITLE>" & vbCrLf)
            .Write("<STYLE>" & vbCrLf)
            .Write("	P.break { page-break-after: always }" & vbCrLf)
            .Write("</STYLE>" & vbCrLf)
            .Write("</HEAD>" & vbCrLf)
            .Write("<BODY BGCOLOR=""#ffffff"" onLoad=""window.focus();window.print();top.frames[0].location='" & dDispatcher.direct("ExtraFunc", "MessageComplete.htm") & "';"">" & vbCrLf)
        End With

        Dim dtPrint As DataTable
        Dim drData As DataRow
        dtPrint = CType(Session("dtprint"), DataTable)
        Dim intCnt As Integer = dtPrint.Rows.Count - 1
        Dim iLoop As Integer

        For iLoop = 0 To intCnt
            drData = dtPrint.Rows(iLoop)
            strType = drData("type")
            Select Case strType.ToUpper
                Case "INV"
                    Server.Execute(dDispatcher.direct("Invoice", "PreviewInvoice.aspx", "pageid=" & strPageId & "&INVNO=" & drData("INVNO") & "&vcomid=" & drData("vcomid")))
                Case "PO"
                    Server.Execute(dDispatcher.direct("PO", "PreviewPO.aspx", "pageid=11&po_no=" & drData("inv_no") & "&side=b&BCoyID=" & drData("bcom")))
                Case "DO"
                    Server.Execute(dDispatcher.direct("PO", "PreviewPO.aspx", "pageid=11&po_no=" & drData("inv_no") & "&side=b&BCoyID=" & drData("bcom")))
            End Select

            If iLoop <> intCnt Then
                Response.Write("<P Class=""break"">&nbsp;</P>")
            End If
        Next

        With Response
            .Write("</BODY>" & vbCrLf)
            .Write("</HTML>" & vbCrLf)
        End With
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
End Class
