Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Public Class AuditTrail
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher
    Dim cbolist As New ListItem

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        If Not IsPostBack Then
            GenerateTab()

            Dim objCat As New ContCat
            Dim dsCat As New DataSet
            dsCat = objCat.getConRefNo()
            Common.FillDdl(ddlCode, "CDM_GROUP_CODE", "CDM_GROUP_INDEX", dsCat)
            cbolist.Value = ""
            cbolist.Text = "---Select---"
            ddlCode.Items.Insert(0, cbolist)
        End If

        ' cmdView.Attributes.Add("onclick", "return compareDates();")

    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_ConCat_tabs") = "<div class=""t_entity""><ul>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dispatcher.direct("Catalogue", "ContractCatalogue.aspx", "pageid=" & strPageId) & """><span>Contract Catalogue</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dispatcher.direct("Catalogue", "ContractItem.aspx", "pageid=" & strPageId) & """><span>Items Assignment</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dispatcher.direct("Catalogue", "ConCat_AssignUser.aspx", "pageid=" & strPageId) & """><span>Users Assignment</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dispatcher.direct("Catalogue", "ConCat_AssignMultiUser.aspx", "pageid=" & strPageId) & """><span>Multi Users Assignment</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dispatcher.direct("Catalogue", "ConCatBatchUploadDownload.aspx", "pageid=" & strPageId) & """><span>Batch Upload/Download</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn_selected"" href=""" & dispatcher.direct("Catalogue", "AuditTrail.aspx", "pageid=" & strPageId) & """><span>Audit</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "</ul><div></div></div>"

    End Sub

    Private Sub ddlCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCode.SelectedIndexChanged
        If ddlCode.SelectedIndex > 0 Then
            cmdView.Enabled = True
        Else
            cmdView.Enabled = False
        End If
    End Sub

    Private Sub ExportToPDF()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strUserId As String
        Dim strCoyName As String
        Dim strBeginDate As String
        Dim strEndDate As String
        Dim strStart As String
        Dim strEnd As String
        Dim strFileName As String = ""
        Dim s1 As String = ""
        Dim s2 As String = ""
        Dim strTitle As String = ""

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))
        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = " SELECT AU_LOG_INDEX,AU_GROUP_INDEX,au_date, AU_DESC," _
                        & "CASE AU_MODULE WHEN 'H' THEN 'Contract Catalogue' WHEN 'I' THEN 'Item Assignment' WHEN 'U' THEN 'Users Assignment' WHEN 'B' THEN 'Batch Upload/Download' END AS 'AU_MODULE'," _
                        & "CASE AU_ACTION WHEN 'A' THEN 'Add' WHEN 'E' THEN 'Modify' WHEN 'D' THEN 'Delete' WHEN 'W' THEN 'Download' END AS 'AU_ACTION'," _
                        & "AU_BEFORE,AU_AFTER ,user_mstr.UM_USER_NAME " _
                        & "FROM au_contract " _
                        & "INNER JOIN USER_MSTR ON AU_USER = USER_MSTR.UM_USER_ID AND USER_MSTR.UM_COY_ID = AU_COY " _
                        & "WHERE AU_GROUP_INDEX= @prmGID AND AU_COY= @prmCoyID "
                If txtDateFr.Text <> "" Then
                    strBeginDate = Format(CDate(Me.txtDateFr.Text), "yyyy-MM-dd" & " " & "00:00:00")
                    strStart = Format(CDate(Me.txtDateFr.Text), "ddMMMyyyy")
                    s1 = Format(CDate(Me.txtDateFr.Text), "dd MMM yyyy")
                    .CommandText &= "AND au_date>='" & strBeginDate & "' "
                End If

                If txtDateTo.Text <> "" Then
                    strEndDate = Format(CDate(Me.txtDateTo.Text), "yyyy-MM-dd" & " " & "23:59:59")
                    strEnd = Format(CDate(Me.txtDateTo.Text), "ddMMMyyyy")
                    s2 = Format(CDate(Me.txtDateTo.Text), "dd MMM yyyy")
                    .CommandText &= "AND au_date <='" & strEndDate & "' "
                End If

                .CommandText &= "ORDER BY AU_LOG_INDEX"
            End With

            da = New MySqlDataAdapter(cmd)

            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmGID", ddlCode.SelectedValue))
            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewAuditTrail_DataSetAuditTrail", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            'localreport.ReportPath = appPath & "Common\Report\POSummary_pdf.rdlc"         
            localreport.ReportPath = dispatcher.direct("Report", "PreviewAuditTrail.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")
            localreport.EnableExternalImages = True

            If strStart = "" Then
                If strEnd = "" Then

                Else
                    strTitle = "TILL " & s2
                End If
            Else
                If strEnd = "" Then
                    strTitle = "FROM " & s1
                Else
                    strTitle = "FROM " & s1 & " TO " & s2
                End If
            End If

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "prmdate"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strTitle)

                    Case "prmgroupcode"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, ddlCode.SelectedItem.Text)

                    Case "prmlogo"
                        'par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            Dim deviceInfo As String =
                "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds

            strFileName = "ContractCatalogueAuditTrail.pdf"
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub

    Private Sub ExportToExcel()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim strBeginDate As String
        Dim strEndDate As String
        Dim strStart As String
        Dim strEnd As String = ""
        Dim strFileName As String = ""
        Dim strDate As String = ""

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = " SELECT DATE_FORMAT(au_date,'%d/%m/%Y %T') AS 'Action Date'," _
                        & "CASE AU_MODULE WHEN 'H' THEN 'Contract Catalogue' WHEN 'I' THEN 'Item Assignment' WHEN 'U' THEN 'Users Assignment' WHEN 'B' THEN 'Batch Upload/Download' END AS 'Module'," _
                        & "CASE AU_ACTION WHEN 'A' THEN 'Add' WHEN 'E' THEN 'Modify' WHEN 'D' THEN 'Delete' WHEN 'W' THEN 'Download' END AS 'Action Type'," _
                        & "AU_DESC AS 'Action Desc',AU_BEFORE AS 'Pre-Action',AU_AFTER AS 'Post-Action', user_mstr.UM_USER_NAME AS 'Action By' " _
                        & "FROM au_contract " _
                        & "INNER JOIN USER_MSTR ON AU_USER = USER_MSTR.UM_USER_ID " _
                        & "AND USER_MSTR.UM_COY_ID = AU_COY " _
                        & "WHERE AU_GROUP_INDEX=" & ddlCode.SelectedValue & " " _
                        & "AND AU_COY= '" & Session("CompanyID") & "' "
                If txtDateFr.Text <> "" Then
                    strBeginDate = Format(CDate(Me.txtDateFr.Text), "yyyy-MM-dd" & " " & "00:00:00")
                    strStart = Format(CDate(Me.txtDateFr.Text), "ddMMMyyyy")
                    .CommandText &= "AND au_date>='" & strBeginDate & "' "
                End If

                If txtDateTo.Text <> "" Then
                    strEndDate = Format(CDate(Me.txtDateTo.Text), "yyyy-MM-dd" & " " & "23:59:59")
                    strEnd = Format(CDate(Me.txtDateTo.Text), "ddMMMyyyy")
                    .CommandText &= "AND au_date <='" & strEndDate & "' "
                End If

                .CommandText &= "ORDER BY AU_LOG_INDEX"
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
        
            If strStart = "" Then
                If strEnd = "" Then

                Else
                    strDate = "(Till" & strEnd & ")"
                End If
            Else
                If strEnd = "" Then
                    strDate = "(From" & strStart & ")"
                Else
                    strDate = "(" & strStart & " - " & strEnd & ")"
                End If
            End If

            strFileName = "ContractCatalogueAuditTrail" & strDate & ".xls"
            Dim attachment As String = "attachment;filename=" & strFileName
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"

            Dim dc As DataColumn
            Dim i As Integer

            i = 0
            For Each dc In ds.Tables(0).Columns
                If i > 0 Then
                    Response.Write(vbTab + dc.ColumnName)
                Else
                    Response.Write(dc.ColumnName)
                End If
                i += 1

            Next
            Response.Write(vbCrLf)

            Dim dr As DataRow
            For Each dr In ds.Tables(0).Rows
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    If i > 0 Then
                        Response.Write(vbTab + dr.Item(i).ToString)
                    Else
                        Response.Write(dr.Item(i).ToString)
                    End If
                Next
                Response.Write(vbCrLf)
            Next
            Response.End()

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try

    End Sub

    Private Sub cmdView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdView.Click
        If cboReportType.SelectedValue = "Excel" Then
            ExportToExcel()

        ElseIf cboReportType.SelectedValue = "PDF" Then
            ExportToPDF()
        End If
    End Sub
End Class