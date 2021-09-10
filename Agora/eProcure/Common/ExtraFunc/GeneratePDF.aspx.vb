Imports System.IO.Directory
Imports System.IO
Imports System.Text
Imports eProcure.Component
Imports AgoraLegacy

Public Class GeneratePDF
    Inherits AgoraLegacy.AppBaseClass
    Dim blnFreeze As Boolean
    Dim strNewUrl As String
    Dim i As Integer
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Public Property NewUrl() As String
        Get
            NewUrl = strNewUrl
        End Get
        Set(ByVal Value As String)
            strNewUrl = Value
        End Set
    End Property

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
        MyBase.Page_Load(sender, e)
        'Response.Buffer = False
        ' Put user code to initialize the page here
        'Session("UserId") = "myongnc"
        'Session("CompanyId") = "demo"
        'Freeze("test.htm")
        'bindData()
        'Dim strToUrl As String
        'Dim strCont As String ' continue prev form
        'strToUrl = Request.QueryString("tourl")
        'strCont = Request.QueryString("cont")
        Dim strType As String
        'With Response
        '    .Write("<HTML>" & vbCrLf)
        '    .Write("<HEAD>" & vbCrLf)
        '    .Write("<TITLE>Invoice</TITLE>" & vbCrLf)
        '    .Write("<STYLE>" & vbCrLf)
        '    .Write("	P.break { page-break-after: always }" & vbCrLf)
        '    .Write("</STYLE>" & vbCrLf)
        '    .Write("</HEAD>" & vbCrLf)
        '    '.Write("<BODY BGCOLOR=""#ffffff"">" & vbCrLf)
        '    '.Write("<BODY BGCOLOR=""#ffffff"" onLoad=""top.frames[0].window.close();window.close();"">" & vbCrLf)
        '    '.Write("<BODY BGCOLOR=""#ffffff"" onLoad=""parent.close();"">" & vbCrLf)
        '    .Write("<BODY BGCOLOR=""#ffffff"" onLoad=""top.frames[0].location='MessagePDFComplete.htm';"">" & vbCrLf)
        'End With

        'If File.Exists(Server.MapPath("Invoice2.htm")) Then
        '    File.Delete(Server.MapPath("Invoice2.htm"))
        'End If

        Dim dtPrint As DataTable
        Dim drData As DataRow
        Dim intCnt As Integer
        If Session("dtprint") Is Nothing Then
            intCnt = -1
        Else
            dtPrint = CType(Session("dtprint"), DataTable)
            intCnt = dtPrint.Rows.Count - 1
        End If

        Dim iLoop As Integer
        Dim intLoop As Integer
        Dim strPDFFile As String

        Dim objFile As New FileManagement
        Dim strFilePath, strMovePath As String

        drData = dtPrint.Rows(0)
        strType = drData("type")
        Session("InvFileName") = Session("CompanyID") & "_" & strType & "_" & Format(Now, "ddMMyyyyHHmmss") & ".htm"
        objFile.getFilePath(EnumUploadFrom.FrontOff, EnumUploadType.PDFDownload, strFilePath, strMovePath, False, strType)

        For iLoop = 0 To intCnt
            drData = dtPrint.Rows(iLoop)
            strType = drData("type")
            Select Case strType.ToUpper
                Case "INV"
                    Server.Execute(dDispatcher.direct("Invoice", "PreviewInvoice.aspx", "pageid=" & strPageId & "&freeze=1&INVNO=" & drData("INVNO") & "&vcomid=" & drData("vcomid")))
                Case "PO"
                    Server.Execute(dDispatcher.direct("PO", "PreviewPO.aspx", "freeze=1&pageid=" & strPageId & "&PO_No=" & drData("po_no") & "&side=" & drData("side") & "&BCoyID=" & drData("bcoyid")))
                    'Case "DO"
                    '    Server.Execute("../PO/POReport.aspx?pageid=11&po_no=" & drData("inv_no") & "&side=b&BCoyID=" & drData("bcom"))
                Case "PR"
                    Server.Execute(dDispatcher.direct("PR", "PRReport.aspx", "pageid=" & strPageId & "&prno=" & drData("prno") & "&index=" & drData("index")))
                Case "GRN"
                    Server.Execute(dDispatcher.direct("GRN", "PreviewGRN.aspx", "pageid=" & strPageId & "&DOIndex=" & drData("doindex") & "&GRNNo1=" & drData("grnno1") & "&BCoyID1=" & drData("bcoyid1")))
                Case "QUO"
                    Server.Execute(dDispatcher.direct("RFQ", "QuoReport.aspx", "pageid=" & strPageId & "&RFQ_ID=" & drData("rfqid") & "&vcomid=" & drData("vcomid")))
                Case "RFQ"
                    Server.Execute(dDispatcher.direct("RFQ", "RFQReport.aspx", "pageid=" & strPageId & "&RFQ_ID=" & drData("rfqid") & "&vcom_id=" & drData("vcomid")))
            End Select

            'If iLoop <> intCnt Then
            '    Response.Write("<P Class=""break"">&nbsp;</P>")
            'End If
        Next

        'With Response
        '    .Write("</BODY>" & vbCrLf)
        '    .Write("</HTML>" & vbCrLf)
        'End With

        objFile = Nothing
        File.Copy(strFilePath & Session("InvFileName"), strFilePath & "src\" & Session("InvFileName"))
        File.Delete(strFilePath & Session("InvFileName"))
        strPDFFile = strFilePath & "dest\" & Replace(Session("InvFileName"), ".htm", ".pdf")
        '//Loop 30 second
        Dim dtLater, dtLastAccess As Date
        Dim blnSuccess As Boolean = False
        dtLater = DateAdd(DateInterval.Second, 60, Now)
        '//loop 60 seconds
        'Dim f As New FileInfo(strPDFFile)
        Do While Now < dtLater
            If File.Exists(strPDFFile) Then
                '    If f.Exists Then
                'file.GetAttributes(
                'If f.Length > 0 Then
                dtLastAccess = File.GetLastWriteTime(strPDFFile)
                'dtLastAccess = f.LastAccessTime
                If DateDiff(DateInterval.Second, dtLastAccess, Now) > 10 Then
                    blnSuccess = True
                    Exit Do
                End If
                'End If
            End If
        Loop

        ''//double check
        'dtNow = Now
        'dtLater = DateAdd(DateInterval.Second, 30, Now)
        'Do While Now < dtLater
        '    If File.Exists(strPDFFile) Then
        '        dtLastAccess = File.GetLastWriteTime(strPDFFile)
        '        If DateDiff(DateInterval.Second, dtLastAccess, Now) > 10 Then
        '            blnSuccess = True
        '            Exit Do
        '        End If
        '    End If
        'Loop


        'Session("dtprint") = Nothing

        If blnSuccess Then
            'Response.Clear()
            Response.ClearContent()
            Response.ClearHeaders()
            Response.ContentType = "application/octet-stream"
            Response.AppendHeader("Content-Disposition",
            "attachment; filename=""" & Replace(Session("InvFileName"), ".htm", ".pdf") & """")
            Response.Flush()
            Response.WriteFile(strPDFFile)
            'Session("InvFileName") = Nothing
            Dim vbs As String
            vbs = vbs & "<script language=""vbs"">"
            'vbs = vbs & "Call MsgBox(""" & msg & """, " & Style & ", """ & title & """)"
            vbs = vbs & vbLf & "parent.close();"""
            vbs = vbs & "</script>"
            Dim rndKey As New Random
            Response.Write(vbs)
            Response.End()
        Else
            'Common.NetMsgbox(Me, "Failed")
            With Response
                .Write("<HTML>" & vbCrLf)
                .Write("<HEAD>" & vbCrLf)
                .Write("<LINK Rel=""stylesheet"" HRef=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """>" & vbCrLf)
                .Write("</HEAD>" & vbCrLf)
                .Write("<BODY>" & vbCrLf)
                .Write("<H1>Download Not Successful</H1>" & vbCrLf)
                .Write("<FONT Face=""Verdana"" Size=""2"" Color=""red"">Unable to download the " _
                  & "<I>pdf</I> file from the server as it has exceeded the one-minute time limit. " _
                  & "Please try again.<BR><BR>" & vbCrLf)
                .Write("If the error persists, kindly inform the System Administrator and quote the " _
                  & "file name <B><I>" & Replace(Session("InvFileName"), ".htm", ".pdf") & "</I></B>.</FONT>" & vbCrLf)
                .Write("</BODY>" & vbCrLf)
                .Write("</HTML>" & vbCrLf)
            End With
        End If
    End Sub
End Class
