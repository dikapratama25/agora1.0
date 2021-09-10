Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class ReportPaymenttoNonResident
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher
    Dim objDb As New EAD.DBCom

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strRptType As String
        Dim ii_ddl, ii_ddl2, jj_ddl As Integer
        Dim cbolist As New ListItem

        ViewState("type") = Request.QueryString("type")
        strRptType = ViewState("type")

        If Not (Page.IsPostBack) Then
            lblHeader.Text = "IPPA0019 - Payment to Non-Resident Report"

            cbolist.Value = ""
            cbolist.Text = "---Select---"
            cmbMonth.Items.Insert(0, cbolist)
            cmbYear.Items.Insert(0, cbolist)

            'Year
            ii_ddl2 = 1
            jj_ddl = Year(Date.Now)
            For ii_ddl = 2002 To jj_ddl
                cmbYear.Items.Insert(ii_ddl2, New ListItem(ii_ddl))
                ii_ddl2 = ii_ddl2 + 1
            Next

            'Month
            ii_ddl = 1
            jj_ddl = 12
            Dim strdate As String
            For ii_ddl = 1 To jj_ddl
                Dim lst As New ListItem
                strdate = "01/" & ii_ddl & "/2005"
                lst.Value = ii_ddl
                lst.Text = Format(CDate(strdate), "MMMM")
                cmbMonth.Items.Insert(ii_ddl, lst)
            Next
        End If

        lnkBack.NavigateUrl = dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId & "&type=" & Request.QueryString("type"))

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
        Dim strSection As String
        Dim strBeginDate As String
        Dim strEndDate As String
        Dim strFileName As String = ""

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT IM_S_COY_NAME, CODE_DESC AS IC_COUNTRY, IM_INVOICE_NO, (IM_INVOICE_TOTAL * IFNULL(IM_EXCHANGE_RATE, 1)) AS INVOICE_TOTAL, " & _
                            "IM_PAYMENT_DATE, IM_CURRENCY_CODE, IM_INVOICE_TOTAL, GROUP_CONCAT(DISTINCT ID_B_GL_CODE ORDER BY ID_B_GL_CODE SEPARATOR ', ') AS ID_B_GL_CODE, " & _
                            "CASE WHEN IM_WITHHOLDING_OPT = 1 THEN 'Yes, payable by company' WHEN IM_WITHHOLDING_OPT = 2 THEN 'Yes, payable by vendor' ELSE 'No' END AS IM_WITHHOLDING_OPT, " & _
                            "IM_WITHHOLDING_REMARKS, IM_WITHHOLDING_TAX, " & _
                            "CASE WHEN (IM_WITHHOLDING_OPT = 3 OR IM_WITHHOLDING_OPT = '') THEN NULL ELSE " & _
                            "((IM_INVOICE_TOTAL * IFNULL(IM_WITHHOLDING_TAX,0)) / 100) * IFNULL(IM_EXCHANGE_RATE, 1) END AS WHT_AMT, " & _
                            "IM_RECEIPT_NO, IM_RECEIPT_DATE, " & _
                            "CASE WHEN (IM_WITHHOLDING_OPT = 3 OR IM_WITHHOLDING_OPT = '') THEN NULL ELSE " & _
                            "(IM_INVOICE_TOTAL - ((IM_INVOICE_TOTAL * IFNULL(IM_WITHHOLDING_TAX,0)) / 100)) * IFNULL(IM_EXCHANGE_RATE, 1) END AS AMT_PAID, " & _
                            "CASE WHEN IM_WITHHOLDING_OPT = 1 THEN " & _
                            "((IM_INVOICE_TOTAL * IFNULL(IM_WITHHOLDING_TAX, 0)) / 100) * IFNULL(IM_EXCHANGE_RATE, 1) " & _
                            "ELSE NULL END AS WHT_BANK " & _
                            "FROM INVOICE_MSTR " & _
                            "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " & _
                            "INNER JOIN IPP_COMPANY ON IM_S_COY_ID = IC_INDEX " & _
                            "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'CT' AND CODE_ABBR = IC_COUNTRY " & _
                            "WHERE IM_B_COY_ID = @prmCoyID AND IM_PAYMENT_DATE IS NOT NULL " & _
                            "AND IM_INVOICE_STATUS = '4' AND IM_PO_INDEX IS NULL " & _
                            "AND IM_RESIDENT_TYPE = 'N' " & _
                            "AND IM_PAYMENT_DATE >= @prmStartDate AND IM_PAYMENT_DATE <= @prmEndDate "

                If cmbSection.SelectedValue <> "" Then
                    .CommandText &= "AND IM_SECTION = '" & cmbSection.SelectedValue & "' "
                End If

                .CommandText &= "GROUP BY IM_INVOICE_INDEX " & _
                            "ORDER BY IM_PAYMENT_DATE, IM_INVOICE_INDEX"
            End With

            da = New MySqlDataAdapter(cmd)

            If Me.cmbMonth.SelectedIndex > 0 Then
                dtDate = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 1)
                dtFrom = dtDate

                If Me.cmbMonth.SelectedValue < 12 Then
                    dtDate = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue + 1, 1)
                    dtTo = DateAdd(DateInterval.Day, -1, dtDate)
                    dtTo = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, dtTo.Day)

                Else    'December
                    dtTo = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 31)
                End If

            Else
            End If
            strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
            strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmStartDate", strBeginDate))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmEndDate", strEndDate))
            strUserId = Session("UserName") 'Session("UserId")
            strCoyName = Session("CompanyName")

            da.Fill(ds)

            If cmbSection.SelectedValue = "S107A" Then
                strSection = "Section 107A"
            ElseIf cmbSection.SelectedValue = "S109" Then
                strSection = "Section 109"
            ElseIf cmbSection.SelectedValue = "S109B" Then
                strSection = "Section 109B"
            ElseIf cmbSection.SelectedValue = "S109F" Then
                strSection = "Section 109F"
            Else
                strSection = "ALL"
            End If

            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PaymentToNonResident_DataSetPaymentToNonResident", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = dispatcher.direct("Report", "PaymentToNonResident.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "pmrequestedby"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

                    Case "logo"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                    Case "prmbuyercoyname"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)

                    Case "dt"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strBeginDate)

                    Case "section"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strSection)

                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            Dim deviceInfo As String = _
                "<DeviceInfo>" + _
                    "  <OutputFormat>EMF</OutputFormat>" + _
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds

            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            strFileName = "IPPA0019 - Payment To Non-Resident Report (" & strSection & ").pdf"

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
    Protected Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        If cboReportType.SelectedValue = "Excel" Then
            ExportToExcel()

        ElseIf cboReportType.SelectedValue = "PDF" Then
            ExportToPDF()
        End If

    End Sub

    Private Sub ExportToExcel()
        Dim ds As New DataSet
        Dim dsSql As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim dtFrom As Date
        Dim dtTo As Date
        Dim dtDate As Date
        Dim i As Integer
        Dim strBeginDate As String
        Dim strEndDate As String
        Dim strTitle As String
        Dim strFileName As String = ""

        Try
            If Me.cmbMonth.SelectedIndex > 0 Then
                dtDate = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 1)
                dtFrom = dtDate

                If Me.cmbMonth.SelectedValue < 12 Then
                    dtDate = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue + 1, 1)
                    dtTo = DateAdd(DateInterval.Day, -1, dtDate)
                    dtTo = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, dtTo.Day)

                Else    'December
                    dtTo = New DateTime(Me.cmbYear.SelectedValue, Me.cmbMonth.SelectedValue, 31)
                End If

            Else
            End If
            strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
            strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
            strTitle = Format(dtFrom, "MMM yyyy")

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT IM_S_COY_NAME AS 'Recipient''s Name', CODE_DESC AS 'Country of Residence', IM_INVOICE_NO AS 'Invoice No', " & _
                            "DATE_FORMAT(IM_PAYMENT_DATE,'%d/%m/%Y') AS 'Date of Payment To Recipient', FORMAT((IM_INVOICE_TOTAL * IFNULL(IM_EXCHANGE_RATE, 1)),2) AS 'Invoice Amount (RM)', " & _
                            "IM_CURRENCY_CODE AS 'Currency', FORMAT(IM_INVOICE_TOTAL,2) AS 'Invoice Amount (FCY)', GROUP_CONCAT(DISTINCT ID_B_GL_CODE ORDER BY ID_B_GL_CODE SEPARATOR ', ') AS 'Charged to which GL Account', " & _
                            "CASE WHEN IM_WITHHOLDING_OPT = 1 THEN 'Yes, payable by company' WHEN IM_WITHHOLDING_OPT = 2 THEN 'Yes, payable by vendor' ELSE 'No' END AS 'Withholding Tax (*WHT*) applicable? (Y/N)', " & _
                            "IM_WITHHOLDING_REMARKS AS 'Reason if WHT is not applicable', IM_WITHHOLDING_TAX AS 'WHT%', " & _
                            "CASE WHEN (IM_WITHHOLDING_OPT = 3 OR IM_WITHHOLDING_OPT = '') THEN NULL ELSE " & _
                            "FORMAT(((IM_INVOICE_TOTAL * IFNULL(IM_WITHHOLDING_TAX,0)) / 100) * IFNULL(IM_EXCHANGE_RATE, 1),2) END AS 'WHT Amount remitted to IRB', " & _
                            "IM_RECEIPT_NO AS 'Receipt No', DATE_FORMAT(IM_RECEIPT_DATE,'%d/%m/%Y') AS 'Receipt Date', " & _
                            "CASE WHEN (IM_WITHHOLDING_OPT = 3 OR IM_WITHHOLDING_OPT = '') THEN NULL ELSE " & _
                            "FORMAT((IM_INVOICE_TOTAL - ((IM_INVOICE_TOTAL * IFNULL(IM_WITHHOLDING_TAX,0)) / 100)) * IFNULL(IM_EXCHANGE_RATE, 1),2) END AS 'Amount paid/credited to recipient', " & _
                            "CASE WHEN IM_WITHHOLDING_OPT = 1 THEN " & _
                            "FORMAT(((IM_INVOICE_TOTAL * IFNULL(IM_WITHHOLDING_TAX, 0)) / 100) * IFNULL(IM_EXCHANGE_RATE, 1),2) " & _
                            "ELSE NULL END AS 'WHT borned by bank' " & _
                            "FROM INVOICE_MSTR " & _
                            "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " & _
                            "INNER JOIN IPP_COMPANY ON IM_S_COY_ID = IC_INDEX " & _
                            "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'CT' AND CODE_ABBR = IC_COUNTRY " & _
                            "WHERE IM_B_COY_ID = '" & Session("CompanyId") & "' AND IM_PAYMENT_DATE IS NOT NULL " & _
                            "AND IM_INVOICE_STATUS = '4' AND IM_PO_INDEX IS NULL " & _
                            "AND IM_RESIDENT_TYPE = 'N' " & _
                            "AND IM_PAYMENT_DATE >= '" & strBeginDate & "' AND IM_PAYMENT_DATE <= '" & strEndDate & "' "

                If cmbSection.SelectedValue <> "" Then
                    .CommandText &= "AND IM_SECTION = '" & cmbSection.SelectedValue & "' "
                End If

                .CommandText &= "GROUP BY IM_INVOICE_INDEX " & _
                            "ORDER BY IM_PAYMENT_DATE, IM_INVOICE_INDEX "
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)

            If cmbSection.SelectedValue = "S107A" Then
                strFileName = "IPPA0019 - Payment To Non-Resident Report (Section 107A)"
            ElseIf cmbSection.SelectedValue = "S109" Then
                strFileName = "IPPA0019 - Payment To Non-Resident Report (Section 109)"
            ElseIf cmbSection.SelectedValue = "S109B" Then
                strFileName = "IPPA0019 - Payment To Non-Resident Report (Section 109B)"
            ElseIf cmbSection.SelectedValue = "S109F" Then
                strFileName = "IPPA0019 - Payment To Non-Resident Report (Section 109F)"
            Else
                strFileName = "IPPA0019 - Payment To Non-Resident Report (ALL)"
            End If

            strFileName = strFileName & "(" & Format(dtFrom, "MMMyyyy") & ").xls"
            Dim attachment As String = "attachment;filename=" & strFileName
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"

            Dim dc As DataColumn
            'Dim sb As New StringBuilder()
            'Dim brstyle As String = "<style>br { mso-data-placement:same-cell; }</style>"
            i = 0

            For Each dc In ds.Tables(0).Columns
                If i > 0 Then
                    Response.Write(vbTab + dc.ColumnName)
                Else
                    Response.Write(dc.ColumnName)
                    'Response.Write(dc.ColumnName + brstyle + "test")
                    'Response.Write("<tr><td>111</td></tr>")
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

End Class