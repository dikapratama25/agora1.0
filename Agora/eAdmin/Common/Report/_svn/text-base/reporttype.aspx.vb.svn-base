Imports System.IO
'Imports CrystalDecisions
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
'Imports CrystalDecisions.Web


Partial Class Report_reporttype
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    ' Protected WithEvents CrystalReportViewer1 As CrystalDecisions.Web.CrystalReportViewer
    '  Protected WithEvents CrystalReportViewer2 As CrystalDecisions.Web.CrystalReportViewer

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim RptPath As String
        '   Dim aa As Report_reporttype = New Report_reporttype()
        Dim objDcom As New DBAccess.EAD.DBCom
        Dim rptDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim oStream As New MemoryStream

        Dim strType As String = Request.QueryString("Type")
        Dim strStartDate As String = Request.QueryString("StartDate")
        Dim strTRID As String = Request.QueryString("TR")
        ' RptPath = Server.MapPath("rpttr.rpt")
        RptPath = Server.MapPath("viewtr.rpt")
        rptDoc.Load(RptPath)
        Dim myConnectionInfo As CrystalDecisions.Shared.ConnectionInfo = New CrystalDecisions.Shared.ConnectionInfo

        myConnectionInfo.ServerName = "ssb"

        myConnectionInfo.DatabaseName = ""

        myConnectionInfo.UserID = "tmsmgr"

        myConnectionInfo.Password = "tmsmgr"

        SetDBLogonForReport(myConnectionInfo, rptDoc)

        '===
        Dim tableonloginfo As New TableLogOnInfo()

        Dim tableonloginfos As New TableLogOnInfos()

        Dim connonloginfo As New ConnectionInfo()

        Try


            Dim paramField As New ParameterField
            Dim paramFields As New ParameterFields

            Dim paramValue As New ParameterDiscreteValue

            paramField.ParameterFieldName = "p_TRID"
            paramValue.Value = strTRID
            paramField.CurrentValues.Add(paramValue)
            paramFields.Add(paramField)

            '   rptDoc.ParameterFields

            CrystalReportViewer2.ParameterFieldInfo = paramFields
            CrystalReportViewer2.DisplayToolbar = True
            CrystalReportViewer2.Enabled = True
            Dim currentvalues As ParameterValues
            currentvalues = rptDoc.DataDefinition.ParameterFields.Item("p_TRID").CurrentValues
            currentvalues.Add(paramValue)

            rptDoc.DataDefinition.ParameterFields.Item("p_TRID").ApplyCurrentValues(currentvalues)
            CrystalReportViewer2.ReportSource = rptDoc

            Dim ExportPath As String
            ExportPath = ConfigurationSettings.AppSettings("email_attachement_folder")
            Dim exportOpt As ExportOptions = rptDoc.ExportOptions
            Dim DiskFileDestOpt As DiskFileDestinationOptions = New DiskFileDestinationOptions()
            DiskFileDestOpt.DiskFileName = ExportPath + Session.SessionID + "viewtr.pdf"
            exportOpt.DestinationOptions = DiskFileDestOpt
            exportOpt.ExportDestinationType = ExportDestinationType.DiskFile
            exportOpt.ExportFormatType = ExportFormatType.PortableDocFormat
            '       rptdoc.
            '   CrystalReportViewer2.ReportSource = rptDoc

            rptDoc.Export()

            '   CrystalReportViewer2.ReportSource = rptDoc

            Response.Buffer = False
            Response.ClearContent()
            Response.ClearHeaders()
            Response.ContentType = "application/pdf"
            Response.WriteFile(ExportPath + Session.SessionID + "viewtr.pdf")
            Response.Flush()
            Response.Close()
            rptDoc.Close()

            System.IO.File.Delete(ExportPath + Session.SessionID + "viewtr.pdf")

        Catch e1 As Exception

            Dim st As String = e1.Message

        End Try




    End Sub
    Private Sub SetDBLogonForReport(ByVal myConnectionInfo As CrystalDecisions.Shared.ConnectionInfo, ByVal myReportDocument As CrystalDecisions.CrystalReports.Engine.ReportDocument)

        Dim myTables As Tables = myReportDocument.Database.Tables
        For Each myTable As CrystalDecisions.CrystalReports.Engine.Table In myTables

            Dim myTableLogonInfo As TableLogOnInfo = myTable.LogOnInfo

            myTableLogonInfo.ConnectionInfo = myConnectionInfo

            myTable.ApplyLogOnInfo(myTableLogonInfo)

        Next

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim RptPath As String
        '   Dim aa As Report_reporttype = New Report_reporttype()
        Dim objDcom As New DBAccess.EAD.DBCom
        Dim rptDoc As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim oStream As New MemoryStream

        Dim strType As String = Request.QueryString("Type")
        Dim strStartDate As String = Request.QueryString("StartDate")
        Dim strTRID As String = Request.QueryString("TR")
        ' RptPath = Server.MapPath("rpttr.rpt")
        RptPath = Server.MapPath("viewtr.rpt")
        rptDoc.Load(RptPath)
        Dim myConnectionInfo As CrystalDecisions.Shared.ConnectionInfo = New CrystalDecisions.Shared.ConnectionInfo

        myConnectionInfo.ServerName = "ssb"

        myConnectionInfo.DatabaseName = ""

        myConnectionInfo.UserID = "tmsmgr"

        myConnectionInfo.Password = "tmsmgr"

        SetDBLogonForReport(myConnectionInfo, rptDoc)

        '===
        Dim tableonloginfo As New TableLogOnInfo()

        Dim tableonloginfos As New TableLogOnInfos()

        Dim connonloginfo As New ConnectionInfo()

        Try


            Dim paramField As New ParameterField
            Dim paramFields As New ParameterFields

            Dim paramValue As New ParameterDiscreteValue

            paramField.ParameterFieldName = "p_TRID"
            paramValue.Value = strTRID
            paramField.CurrentValues.Add(paramValue)
            paramFields.Add(paramField)

            '   rptDoc.ParameterFields

            CrystalReportViewer2.ParameterFieldInfo = paramFields

            CrystalReportViewer2.ReportSource = rptDoc
        Catch e1 As Exception

            Dim st As String = e1.Message

        End Try
    End Sub
End Class

