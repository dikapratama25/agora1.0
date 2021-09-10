Imports AgoraLegacy
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class ReportFormatN6
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lnkBack.NavigateUrl = dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId)
    End Sub

    Private Sub ExportToExcel()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim dtFrom As Date
        Dim dtTo As Date
        Dim dtDate As Date
        Dim strBeginDate As String
        Dim strEndDate As String
        Dim strStart As String
        Dim strEnd As String = ""
        Dim strFileName As String = ""

        Try
            'If Me.cmbMonthFrom.SelectedIndex > 0 Then
            '    dtDate = New DateTime(Me.cmbYearFrom.SelectedValue, Me.cmbMonthFrom.SelectedValue, 1)
            '    dtFrom = dtDate

            '    If Me.cmbMonthTo.SelectedValue < 12 Then
            '        dtDate = New DateTime(Me.cmbYearTo.SelectedValue, Me.cmbMonthTo.SelectedValue + 1, 1)
            '        dtTo = DateAdd(DateInterval.Day, -1, dtDate)
            '        dtTo = New DateTime(Me.cmbYearTo.SelectedValue, Me.cmbMonthTo.SelectedValue, dtTo.Day)

            '    Else
            '        dtDate = New DateTime(Me.cmbYearFrom.SelectedValue, Me.cmbMonthFrom.SelectedValue, 1)
            '        dtFrom = dtDate
            '        dtTo = New DateTime(Me.cmbYearTo.SelectedValue, Me.cmbMonthTo.SelectedValue, 31)
            '    End If

            'Else
            '    dtDate = New DateTime(Me.cmbYearFrom.SelectedValue, 1, 1)
            '    dtFrom = dtDate
            '    dtTo = New DateTime(Me.cmbYearTo.SelectedValue, 12, 31)
            'End If

            'strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
            'strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
            'strStart = Format(dtFrom, "MMM yyyy")
            'strEnd = Format(dtTo, "MMM yyyy")

            myConnectionString = System.Configuration.ConfigurationSettings.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT POD_PO_NO AS 'PO Number',DATE_FORMAT(POM_PO_DATE,'%d/%m/%Y') AS 'PO Date', " _
                        & "IF(POD_ETD>0,DATE_FORMAT(DATE_ADD(POM_CREATED_DATE, INTERVAL POD_ETD DAY),'%d/%m/%Y'),'Ex-Stock') AS 'EDD'," _
                        & "POM_S_COY_NAME AS 'Vendor Name', IF(PO_DETAILS.POD_VENDOR_ITEM_CODE='&nbsp;','',PO_DETAILS.POD_VENDOR_ITEM_CODE) AS 'Item Code', PO_DETAILS.POD_PRODUCT_DESC AS 'Item Description'," _
                        & "POD_ORDERED_QTY AS 'PO Qty',SUM(DOD_DO_QTY) AS 'DO Qty',SUM(GD_RECEIVED_QTY) AS 'GRN Qty'," _
                        & "POD_REJECTED_QTY AS 'Rejected Qty'," _
                        & "(POD_ORDERED_QTY - POD_DELIVERED_QTY - POD_CANCELLED_QTY) AS 'Overdue Qty'," _
                        & "DATEDIFF(CURRENT_DATE,DATE_ADD(POM_CREATED_DATE, INTERVAL POD_ETD DAY)) AS 'Overdue Day(s)' " _
                        & "FROM PO_MSTR PM " _
                        & "INNER JOIN PO_DETAILS ON POD_COY_ID = POM_B_COY_ID AND POD_PO_NO = POM_PO_NO " _
                        & "LEFT JOIN do_mstr ON DOM_PO_INDEX=POM_PO_INDEX " _
                        & "LEFT JOIN do_details ON do_mstr.DOM_DO_NO=do_details.DOD_DO_NO AND DOD_PO_LINE=POD_PO_LINE " _
                        & "LEFT JOIN GRN_MSTR ON GRN_MSTR.GM_DO_Index = DO_Mstr.DOM_DO_Index AND GRN_MSTR.GM_PO_Index = PM.POM_PO_Index " _
                        & "LEFT JOIN GRN_Details ON GRN_Details.GD_GRN_NO = GRN_Mstr.GM_GRN_NO AND GRN_Mstr.GM_B_COY_ID = GRN_Details.GD_B_COY_ID AND GRN_Details.GD_PO_LINE = PO_Details.POD_Po_Line " _
                        & "WHERE POM_B_COY_ID= '" & Session("CompanyIdToken") & "' AND PM.POM_PO_STATUS IN (1,2,3)  " _
                        & "AND (POD_ORDERED_QTY - POD_DELIVERED_QTY - POD_CANCELLED_QTY)>0 " _
                        & "AND CURRENT_DATE>DATE_ADD(PM.POM_CREATED_DATE, INTERVAL POD_ETD DAY) " _
                        & "GROUP BY POM_PO_NO,POD_PO_LINE " _
                        & "ORDER BY DATEDIFF(CURRENT_DATE,DATE_ADD(POM_CREATED_DATE, INTERVAL POD_ETD DAY)) DESC,POD_PO_NO"
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
            strFileName = "OverduePOReport" & "(" & Format(Now, "ddMMMyyyy") & ").xls"
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
        Dim dtFrom As Date
        Dim dtTo As Date
        Dim dtDate As Date
        Dim strBeginDate As String
        Dim strEndDate As String
        Dim strTitle As String
        Dim strFileName As String = ""

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyIdToken"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationSettings.AppSettings.Get("eProcurePath"))
        'strImgSrc = objFile.getCoyLogo(EnumUploadFrom.BackOff, Session("CompanyIdToken"), System.Configuration.ConfigurationSettings.AppSettings.Get("eProcurePath"))
        'strImgSrc = objFile.getReportCoLogo(EnumUploadFrom.BackOff, Session("CompanyIdToken"), "", System.Configuration.ConfigurationSettings.AppSettings.Get("eProcurePath"))

        Try

            myConnectionString = System.Configuration.ConfigurationSettings.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT po_details.POD_PO_NO, PM.POM_CREATED_DATE, PM.POM_PO_DATE, po_details.POD_ETD, PM.POM_S_COY_NAME, " _
                            & "po_details.POD_VENDOR_ITEM_CODE, po_details.POD_PRODUCT_DESC, po_details.POD_ORDERED_QTY AS POQty, SUM(do_details.DOD_DO_QTY) " _
                            & "AS DOQty, SUM(grn_details.GD_RECEIVED_QTY) AS GRNQty, po_details.POD_REJECTED_QTY, po_details.POD_ORDERED_QTY, " _
                            & "po_details.POD_DELIVERED_QTY, po_details.POD_CANCELLED_QTY, company_mstr.CM_COY_NAME, PM.POM_BUYER_ID " _
                            & "FROM po_mstr PM INNER JOIN " _
                            & "company_mstr ON PM.POM_B_COY_ID = company_mstr.CM_COY_ID INNER JOIN " _
                            & "po_details ON po_details.POD_COY_ID = PM.POM_B_COY_ID AND po_details.POD_PO_NO = PM.POM_PO_NO LEFT OUTER JOIN " _
                            & "do_mstr ON do_mstr.DOM_PO_INDEX = PM.POM_PO_INDEX LEFT OUTER JOIN " _
                            & "do_details ON do_mstr.DOM_DO_NO = do_details.DOD_DO_NO AND do_details.DOD_PO_LINE = po_details.POD_PO_LINE LEFT OUTER JOIN " _
                            & "grn_mstr ON grn_mstr.GM_DO_INDEX = do_mstr.DOM_DO_INDEX AND grn_mstr.GM_PO_INDEX = PM.POM_PO_INDEX LEFT OUTER JOIN " _
                            & "grn_details ON grn_details.GD_GRN_NO = grn_mstr.GM_GRN_NO AND grn_mstr.GM_B_COY_ID = grn_details.GD_B_COY_ID AND  " _
                            & "grn_details.GD_PO_LINE = po_details.POD_PO_LINE " _
                            & "WHERE (PM.POM_B_COY_ID = @prmCoyID) AND (PM.POM_PO_STATUS IN (1, 2, 3)) AND  " _
                            & "(po_details.POD_ORDERED_QTY - po_details.POD_DELIVERED_QTY - po_details.POD_CANCELLED_QTY > 0) " _
                            & "AND CURRENT_DATE>DATE_ADD(PM.POM_CREATED_DATE, INTERVAL POD_ETD DAY) " _
                            & "GROUP BY PM.POM_PO_NO, po_details.POD_PO_LINE " _
                            & "ORDER BY DATEDIFF(CURRENT_DATE,DATE_ADD(POM_CREATED_DATE, INTERVAL POD_ETD DAY)) DESC,POD_PO_NO"
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
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyIdToken")))
            'da.SelectCommand.Parameters.Add(New MySqlParameter("@prmStartDate", strBeginDate))
            'da.SelectCommand.Parameters.Add(New MySqlParameter("@prmEndDate", strEndDate))
            strUserId = Session("UserName") 'Session("UserId")
            strCoyName = Session("CoyName")

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("OverduePO_DataSetOverduePO", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            'localreport.ReportPath = appPath & "Common\Report\POSummary_pdf.rdlc"
            localreport.ReportPath = dispatcher.direct("Report", "OverduePO_pdf.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationSettings.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            If TotalParameter > 0 Then
                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
                For I = 0 To localreport.GetParameters.Count - 1
                    GetParameter = localreport.GetParameters.Item(I).Name
                    Select Case LCase(GetParameter)
                        Case "prmrequestedby"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

                        Case "logo"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationSettings.AppSettings(strID) & strImgSrc)

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

            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            strFileName = "OverduePOReport.pdf"
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