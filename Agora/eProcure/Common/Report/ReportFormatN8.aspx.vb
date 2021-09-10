Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class ReportFormatN8
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lnkBack.NavigateUrl = dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId & "&type=" & Request.QueryString("type"))

        Dim strCoyType As String
        lblHeader.Text = Request.QueryString("rptname")
        strCoyType = lblHeader.Text.Substring(1, 1)
        If strCoyType = "V" Then
            ViewState("ReportType") = "Vendor"
        Else
            ViewState("ReportType") = "Buyer"
        End If
    End Sub

    Private Sub ExportToExcel()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        'Dim dtFrom As Date
        'Dim dtTo As Date
        'Dim dtDate As Date
        'Dim strBeginDate As String
        'Dim strEndDate As String
        'Dim strStart As String
        Dim strEnd As String = ""
        Dim strFileName As String = ""

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                If ViewState("ReportType") = "Buyer" Then
                    .CommandText = "SELECT INVOICE_DETAILS.ID_INVOICE_NO AS 'Invoice No', INVOICE_MSTR.IM_CREATED_ON AS 'Invoice Date', INVOICE_MSTR.IM_PAYMENT_DATE AS 'Due Date', " _
                            & "PO_MSTR.POM_PO_NO AS 'PO Number', PO_MSTR.POM_PO_DATE AS 'PO Date', PO_MSTR.POM_PAYMENT_METHOD AS 'Payment Method', STATUS_MSTR.STATUS_DESC AS 'Invoice Status', " _
                            & "PO_MSTR.POM_CURRENCY_CODE AS 'Currency', FORMAT(SUM(ROUND(ID_RECEIVED_QTY*ID_UNIT_COST,2)),2) AS 'Amount', INVOICE_MSTR.IM_INVOICE_TOTAL AS 'GST Amount', DATEDIFF(CURRENT_DATE, IM_PAYMENT_DATE) AS 'Day(s) to Due Date' " _
                            & "FROM INVOICE_DETAILS INNER JOIN INVOICE_MSTR ON INVOICE_DETAILS.ID_INVOICE_NO = INVOICE_MSTR.IM_INVOICE_NO " _
                            & "And INVOICE_DETAILS.ID_S_COY_ID = INVOICE_MSTR.IM_S_COY_ID INNER JOIN PO_DETAILS ON INVOICE_DETAILS.ID_PO_LINE = PO_DETAILS.POD_PO_LINE " _
                            & "INNER JOIN PO_MSTR ON PO_DETAILS.POD_PO_NO = PO_MSTR.POM_PO_NO AND PO_DETAILS.POD_COY_ID = PO_MSTR.POM_B_COY_ID " _
                            & "And INVOICE_MSTR.IM_PO_INDEX = PO_MSTR.POM_PO_INDEX And INVOICE_MSTR.IM_S_COY_ID = PO_MSTR.POM_S_COY_ID " _
                            & "INNER JOIN status_mstr ON status_mstr.STATUS_NO=INVOICE_MSTR.IM_INVOICE_STATUS AND STATUS_MSTR.STATUS_TYPE='INV' " _
                            & "INNER JOIN COMPANY_MSTR ON IM_B_COY_ID = COMPANY_MSTR.CM_COY_ID " _
                            & "WHERE (INVOICE_MSTR.IM_B_COY_ID = '" & Session("CompanyID") & "') " _
                            & "AND IM_INVOICE_STATUS IN (1,2,3) " _
                            & "And CURRENT_DATE > INVOICE_MSTR.IM_CREATED_ON And CURRENT_DATE > IM_PAYMENT_DATE " _
                            & "GROUP BY IM_INVOICE_INDEX " _
                            & "ORDER BY IM_S_COY_ID, DATEDIFF(CURRENT_DATE,IM_PAYMENT_DATE)"
                Else
                    .CommandText = "SELECT INVOICE_DETAILS.ID_INVOICE_NO AS 'Invoice No', INVOICE_MSTR.IM_CREATED_ON AS 'Invoice Date', INVOICE_MSTR.IM_PAYMENT_DATE AS 'Due Date', " _
                            & "PO_MSTR.POM_PO_NO AS 'PO Number', PO_MSTR.POM_PO_DATE AS 'PO Date', PO_MSTR.POM_PAYMENT_METHOD AS 'Payment Method', STATUS_MSTR.STATUS_DESC AS 'Invoice Status', " _
                            & "PO_MSTR.POM_CURRENCY_CODE AS 'Currency', FORMAT(SUM(ROUND(ID_RECEIVED_QTY*ID_UNIT_COST,2)),2) AS 'Amount', INVOICE_MSTR.IM_INVOICE_TOTAL AS 'GST Amount', DATEDIFF(CURRENT_DATE, IM_PAYMENT_DATE) AS 'Day(s) to Due Date' " _
                            & "FROM INVOICE_DETAILS INNER JOIN INVOICE_MSTR ON INVOICE_DETAILS.ID_INVOICE_NO = INVOICE_MSTR.IM_INVOICE_NO " _
                            & "And INVOICE_DETAILS.ID_S_COY_ID = INVOICE_MSTR.IM_S_COY_ID INNER JOIN PO_DETAILS ON INVOICE_DETAILS.ID_PO_LINE = PO_DETAILS.POD_PO_LINE " _
                            & "INNER JOIN PO_MSTR ON PO_DETAILS.POD_PO_NO = PO_MSTR.POM_PO_NO AND PO_DETAILS.POD_COY_ID = PO_MSTR.POM_B_COY_ID " _
                            & "And INVOICE_MSTR.IM_PO_INDEX = PO_MSTR.POM_PO_INDEX And INVOICE_MSTR.IM_S_COY_ID = PO_MSTR.POM_S_COY_ID " _
                            & "INNER JOIN STATUS_MSTR ON STATUS_MSTR.STATUS_NO=INVOICE_MSTR.IM_INVOICE_STATUS AND STATUS_MSTR.STATUS_TYPE='INV' " _
                            & "INNER JOIN COMPANY_MSTR ON IM_B_COY_ID = COMPANY_MSTR.CM_COY_ID " _
                            & "WHERE (INVOICE_MSTR.IM_S_COY_ID = '" & Session("CompanyID") & "') " _
                            & "AND IM_INVOICE_STATUS IN (1,2,3) " _
                            & "And CURRENT_DATE > INVOICE_MSTR.IM_CREATED_ON And CURRENT_DATE > IM_PAYMENT_DATE " _
                            & "GROUP BY IM_INVOICE_INDEX " _
                            & "ORDER BY IM_B_COY_ID, DATEDIFF(CURRENT_DATE,IM_PAYMENT_DATE)"
                End If
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
            strFileName = "OverdueInvoiceReport" & "(" & Format(Now, "ddMMMyyyy") & ").xls"
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
        'Dim dtFrom As Date
        'Dim dtTo As Date
        'Dim dtDate As Date
        'Dim strBeginDate As String
        'Dim strEndDate As String
        'Dim strTitle As String
        Dim strFileName As String = ""

        'strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyIdToken"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))
        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))
        'strImgSrc = objFile.getCoyLogo(EnumUploadFrom.BackOff, Session("CompanyIdToken"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))
        'strImgSrc = objFile.getReportCoLogo(EnumUploadFrom.BackOff, Session("CompanyIdToken"), "", System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                If ViewState("ReportType") = "Buyer" Then
                    .CommandText = "SELECT DISTINCT INVOICE_DETAILS.ID_INVOICE_NO, INVOICE_DETAILS.ID_S_COY_ID, " _
                            & "INVOICE_MSTR.IM_INVOICE_INDEX, INVOICE_MSTR.IM_INVOICE_NO, INVOICE_MSTR.IM_S_COY_ID, INVOICE_MSTR.IM_S_COY_NAME, " _
                            & "INVOICE_MSTR.IM_PO_INDEX, INVOICE_MSTR.IM_B_COY_ID, INVOICE_MSTR.IM_PAYMENT_DATE, INVOICE_MSTR.IM_CREATED_ON, " _
                            & "INVOICE_MSTR.IM_INVOICE_STATUS, INVOICE_MSTR.IM_INVOICE_TOTAL, INVOICE_MSTR.IM_PAYMENT_TERM, INVOICE_MSTR.IM_STATUS_CHANGED_BY, " _
                            & "INVOICE_MSTR.IM_STATUS_CHANGED_ON, PO_MSTR.POM_PO_INDEX, PO_MSTR.POM_PO_NO, PO_MSTR.POM_B_COY_ID, " _
                            & "PO_MSTR.POM_BUYER_ID, PO_MSTR.POM_BUYER_NAME, PO_MSTR.POM_S_COY_ID, PO_MSTR.POM_S_COY_NAME, PO_MSTR.POM_PO_DATE, " _
                            & "PO_MSTR.POM_PAYMENT_METHOD, PO_MSTR.POM_CURRENCY_CODE, COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, COMPANY_MSTR.CM_CURRENCY_CODE, STATUS_MSTR.STATUS_DESC " _
                            & "FROM INVOICE_DETAILS INNER JOIN INVOICE_MSTR ON INVOICE_DETAILS.ID_INVOICE_NO = INVOICE_MSTR.IM_INVOICE_NO " _
                            & "And INVOICE_DETAILS.ID_S_COY_ID = INVOICE_MSTR.IM_S_COY_ID INNER JOIN PO_DETAILS ON INVOICE_DETAILS.ID_PO_LINE = PO_DETAILS.POD_PO_LINE " _
                            & "INNER JOIN PO_MSTR ON PO_DETAILS.POD_PO_NO = PO_MSTR.POM_PO_NO AND PO_DETAILS.POD_COY_ID = PO_MSTR.POM_B_COY_ID " _
                            & "And INVOICE_MSTR.IM_PO_INDEX = PO_MSTR.POM_PO_INDEX And INVOICE_MSTR.IM_S_COY_ID = PO_MSTR.POM_S_COY_ID " _
                            & "INNER JOIN STATUS_MSTR ON STATUS_MSTR.STATUS_NO=INVOICE_MSTR.IM_INVOICE_STATUS AND STATUS_MSTR.STATUS_TYPE='INV' " _
                            & "INNER JOIN COMPANY_MSTR ON IM_B_COY_ID = COMPANY_MSTR.CM_COY_ID " _
                            & "WHERE (INVOICE_MSTR.IM_B_COY_ID = '" & Session("CompanyID") & "') " _
                            & "AND IM_INVOICE_STATUS IN (1,2,3) " _
                            & "And CURRENT_DATE > INVOICE_MSTR.IM_CREATED_ON And CURRENT_DATE > IM_PAYMENT_DATE " _
                            & "ORDER BY IM_S_COY_ID, DATEDIFF(CURRENT_DATE,IM_PAYMENT_DATE)"
                Else
                    .CommandText = "SELECT DISTINCT INVOICE_DETAILS.ID_INVOICE_NO, INVOICE_DETAILS.ID_S_COY_ID, " _
                            & "INVOICE_MSTR.IM_INVOICE_INDEX, INVOICE_MSTR.IM_INVOICE_NO, INVOICE_MSTR.IM_S_COY_ID, INVOICE_MSTR.IM_S_COY_NAME, " _
                            & "INVOICE_MSTR.IM_PO_INDEX, INVOICE_MSTR.IM_B_COY_ID, INVOICE_MSTR.IM_PAYMENT_DATE, INVOICE_MSTR.IM_CREATED_ON, " _
                            & "INVOICE_MSTR.IM_INVOICE_STATUS, INVOICE_MSTR.IM_INVOICE_TOTAL, INVOICE_MSTR.IM_PAYMENT_TERM, INVOICE_MSTR.IM_STATUS_CHANGED_BY, " _
                            & "INVOICE_MSTR.IM_STATUS_CHANGED_ON, PO_MSTR.POM_PO_INDEX, PO_MSTR.POM_PO_NO, PO_MSTR.POM_B_COY_ID, " _
                            & "PO_MSTR.POM_BUYER_ID, PO_MSTR.POM_BUYER_NAME, PO_MSTR.POM_S_COY_ID, PO_MSTR.POM_S_COY_NAME, PO_MSTR.POM_PO_DATE, " _
                            & "PO_MSTR.POM_PAYMENT_METHOD, PO_MSTR.POM_CURRENCY_CODE, COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, COMPANY_MSTR.CM_CURRENCY_CODE, STATUS_MSTR.STATUS_DESC " _
                            & "FROM INVOICE_DETAILS INNER JOIN INVOICE_MSTR ON INVOICE_DETAILS.ID_INVOICE_NO = INVOICE_MSTR.IM_INVOICE_NO " _
                            & "And INVOICE_DETAILS.ID_S_COY_ID = INVOICE_MSTR.IM_S_COY_ID INNER JOIN PO_DETAILS ON INVOICE_DETAILS.ID_PO_LINE = PO_DETAILS.POD_PO_LINE " _
                            & "INNER JOIN PO_MSTR ON PO_DETAILS.POD_PO_NO = PO_MSTR.POM_PO_NO AND PO_DETAILS.POD_COY_ID = PO_MSTR.POM_B_COY_ID " _
                            & "And INVOICE_MSTR.IM_PO_INDEX = PO_MSTR.POM_PO_INDEX And INVOICE_MSTR.IM_S_COY_ID = PO_MSTR.POM_S_COY_ID " _
                            & "INNER JOIN STATUS_MSTR ON STATUS_MSTR.STATUS_NO=INVOICE_MSTR.IM_INVOICE_STATUS AND STATUS_MSTR.STATUS_TYPE='INV' " _
                            & "INNER JOIN COMPANY_MSTR ON IM_B_COY_ID = COMPANY_MSTR.CM_COY_ID " _
                            & "WHERE (INVOICE_MSTR.IM_S_COY_ID = '" & Session("CompanyID") & "') " _
                            & "AND IM_INVOICE_STATUS IN (1,2,3) " _
                            & "And CURRENT_DATE > INVOICE_MSTR.IM_CREATED_ON And CURRENT_DATE > IM_PAYMENT_DATE " _
                            & "ORDER BY IM_B_COY_ID, DATEDIFF(CURRENT_DATE,IM_PAYMENT_DATE)"
                End If
            End With

            da = New MySqlDataAdapter(cmd)

            'If Me.cboMonth.SelectedIndex > 0 Then
            '    dtDate = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue, 1)
            '    dtFrom = dtDate

            '    If Me.cboMonth.SelectedValue < 12 Then
            '        dtDate = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue + 1, 1)
            '        dtTo = DateAdd(DateInterval.Day, -1, dtDate)
            '        dtTo = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue, dtTo.Day)

            '    Else    'December
            '        dtTo = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue, 31)
            '    End If
            'Else
            'End If

            'strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
            'strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
            'strTitle = Format(dtFrom, "MMM yyyy")
            'da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyIdToken")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyID")))
            'da.SelectCommand.Parameters.Add(New MySqlParameter("@prmStartDate", strBeginDate))
            'da.SelectCommand.Parameters.Add(New MySqlParameter("@prmEndDate", strEndDate))
            strUserId = Session("UserName") 'Session("UserId")
            strCoyName = Session("CompanyName")

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("OverdueInv_DataSetOverdueInv", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            'localreport.ReportPath = appPath & "Common\Report\POSummary_pdf.rdlc"
            If ViewState("ReportType") = "Buyer" Then
                localreport.ReportPath = dispatcher.direct("Report", "OverdueInv_pdf.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")
            Else
                localreport.ReportPath = dispatcher.direct("Report", "OverdueInvVendor_pdf.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")
            End If
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            If TotalParameter > 0 Then
                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
                For I = 0 To localreport.GetParameters.Count - 1
                    GetParameter = localreport.GetParameters.Item(I).Name
                    Select Case LCase(GetParameter)
                        Case "prmrequestedby"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

                        Case "logo"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                        Case "prmdate"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, Format(Now, "dd MMM yyyy"))

                        Case "prmbuyercoyname"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)

                        Case Else
                    End Select
                Next
                localreport.SetParameters(par)
            End If

            localreport.Refresh()

            Dim deviceInfo As String = _
                "<DeviceInfo>" + _
                    "  <OutputFormat>EMF</OutputFormat>" + _
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            strFileName = "OverdueInvoiceReport.pdf"
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()

            'Dim fs As New FileStream(appPath & "Report\" & strFileName, FileMode.Create)
            '
            ''Dim fs As New FileStream(Server.MapPath(strFileName), FileMode.Create)
            'Dim fs As New FileStream(dispatcher.direct("Report", strFileName), FileMode.Create)
            'fs.Write(PDF, 0, PDF.Length)
            'fs.Close()

            'Response.ContentType = "application/x-download"
            'Response.AddHeader("Content-Disposition", "attachment;filename=" & strFileName)
            'Response.WriteFile(Server.MapPath(strFileName))
            'Response.End()

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

    Protected Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        If cboReportType.SelectedValue = "Excel" Then
            ExportToExcel()

        ElseIf cboReportType.SelectedValue = "PDF" Then
            ExportToPDF()
        End If
    End Sub

End Class