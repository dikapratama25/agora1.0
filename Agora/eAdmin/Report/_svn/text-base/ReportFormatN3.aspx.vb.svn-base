Imports AppCommon
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class ReportFormatN3
    Inherits AppCommon.AppBaseClass
    Dim dispatcher As New Dispatcher.dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not (Page.IsPostBack) Then

            Dim ii_ddl, ii_ddl2, jj_ddl As Integer
            ii_ddl2 = 0
            'jj_ddl = 2020
            ' jj_ddl = CDate.Year
            'For ii_ddl = 1990 To jj_ddl
            'jj_ddl = Format(Date.Now, "yyyy")
            jj_ddl = Year(Date.Now)
            For ii_ddl = 2002 To jj_ddl

                cboYear.Items.Insert(ii_ddl2, New ListItem(ii_ddl))
                'cboYear.Items.Add(New ListItem(ii_ddl), ii_ddl2)

                ii_ddl2 = ii_ddl2 + 1
            Next
            Dim lstItem As New ListItem
            lstItem.Value = ""
            lstItem.Text = "---Select---"
            cboYear.Items.Insert(0, lstItem)

            MonthName(1, True)
            Dim i As Integer

            'ii_ddl = 1
            'jj_ddl = 12
            'For ii_ddl = 1 To jj_ddl
            For i = 1 To 12
                Dim lstItemMonth As New ListItem
                lstItemMonth.Value = i

                'cboMonth.Items.Insert(i - 1, New ListItem(i))
                '   cboMonth.Items.Insert(i - 1, New ListItem(MonthName(i, False)))
                cboMonth.Items.Add(New ListItem(MonthName(i, False), i))

            Next

            lstItem.Value = ""
            lstItem.Text = "---Select---"
            cboMonth.Items.Insert(0, lstItem)
            'Wheel.Components.Common.SelDdl(Now.Month, cboMonth)
            ' Wheel.Components.Common.SelDdl(Now.Year, cboYear)

        End If
        lnkBack.NavigateUrl = dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId)

    End Sub

    Private Sub ExportToPDF()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        'Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strUserId As String
        Dim dtFrom As Date
        Dim dtTo As Date
        Dim dtDate As Date
        Dim strBeginDate As String
        Dim strEndDate As String
        Dim strTitle As String
        Dim strFileName As String = ""
        Dim objFile As New FileManagement

        'strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyIdToken"), System.AppDomain.CurrentDomain.BaseDirectory & "Common\Plugins\images\", System.Configuration.ConfigurationSettings.AppSettings.Get("eProcurePath"))
        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyIdToken"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationSettings.AppSettings.Get("eProcurePath"))

        Try

            myConnectionString = System.Configuration.ConfigurationSettings.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT INVOICE_MSTR.IM_INVOICE_INDEX, " _
                    & "INVOICE_MSTR.IM_INVOICE_NO, INVOICE_MSTR.IM_S_COY_ID, INVOICE_MSTR.IM_S_COY_NAME, " _
                    & "INVOICE_MSTR.IM_PO_INDEX, INVOICE_MSTR.IM_B_COY_ID, INVOICE_MSTR.IM_PAYMENT_DATE, " _
                    & "INVOICE_MSTR.IM_REMARK, INVOICE_MSTR.IM_CREATED_BY, INVOICE_MSTR.IM_CREATED_ON, " _
                    & "INVOICE_MSTR.IM_INVOICE_STATUS,STATUS_DESC, INVOICE_MSTR.IM_PAYMENT_NO, INVOICE_MSTR.IM_YOUR_REF, " _
                    & "INVOICE_MSTR.IM_OUR_REF, INVOICE_MSTR.IM_INVOICE_PREFIX, INVOICE_MSTR.IM_SUBMITTEDBY_FO, " _
                    & "INVOICE_MSTR.IM_EXCHANGE_RATE, INVOICE_MSTR.IM_FINANCE_REMARKS, INVOICE_MSTR.IM_PRINTED, " _
                    & "INVOICE_MSTR.IM_FOLDER, INVOICE_MSTR.IM_FM_APPROVED_DATE, INVOICE_MSTR.IM_DOWNLOADED_DATE, " _
                    & "INVOICE_MSTR.IM_EXTERNAL_IND, INVOICE_MSTR.IM_REFERENCE_NO, INVOICE_MSTR.IM_INVOICE_TOTAL, " _
                    & "INVOICE_MSTR.IM_PAYMENT_TERM, INVOICE_MSTR.IM_STATUS_CHANGED_BY, INVOICE_MSTR.IM_STATUS_CHANGED_ON, " _
                    & "PO_MSTR.POM_PO_INDEX, PO_MSTR.POM_PO_NO, " _
                    & "PO_MSTR.POM_B_COY_ID, PO_MSTR.POM_BUYER_ID, PO_MSTR.POM_BUYER_NAME, PO_MSTR.POM_BUYER_PHONE, " _
                    & "PO_MSTR.POM_BUYER_FAX, PO_MSTR.POM_S_COY_ID, PO_MSTR.POM_S_COY_NAME, PO_MSTR.POM_S_ATTN, " _
                    & "PO_MSTR.POM_S_REMARK, PO_MSTR.POM_PO_DATE, " _
                    & "PO_MSTR.POM_FREIGHT_TERMS, PO_MSTR.POM_PAYMENT_TERM, PO_MSTR.POM_PAYMENT_METHOD, " _
                    & "PO_MSTR.POM_SHIPMENT_MODE, PO_MSTR.POM_SHIPMENT_TERM, PO_MSTR.POM_CURRENCY_CODE, " _
                    & "PO_MSTR.POM_EXCHANGE_RATE, PO_MSTR.POM_PAYMENT_TERM_CODE, PO_MSTR.POM_SHIP_VIA, " _
                    & "PO_MSTR.POM_PO_STATUS, PO_MSTR.POM_STATUS_CHANGED_BY, PO_MSTR.POM_STATUS_CHANGED_ON, " _
                    & "PO_MSTR.POM_EXTERNAL_REMARK, PO_MSTR.POM_CREATED_BY, PO_MSTR.POM_PO_COST, PO_MSTR.POM_BILLING_METHOD, " _
                    & "PO_MSTR.POM_PO_PREFIX, PO_MSTR.POM_FULFILMENT, PO_MSTR.POM_DEPT_INDEX, company_dept_mstr.CDM_DEPT_NAME, PO_MSTR.POM_ACCEPTED_DATE, " _
                    & "PO_MSTR.POM_DOWNLOADED_DATE, PO_MSTR.POM_ARCHIVE_IND, PO_MSTR.POM_TERMANDCOND, " _
                    & "PO_MSTR.POM_REFERENCE_NO, PO_MSTR.POM_EXTERNAL_IND, COMPANY_MSTR.CM_COY_ID, " _
                    & "COMPANY_MSTR.CM_COY_NAME, COMPANY_MSTR.CM_COY_TYPE, COMPANY_MSTR.CM_PARENT_COY_ID, " _
                    & "COMPANY_MSTR.CM_ACCT_NO, COMPANY_MSTR.CM_BANK, COMPANY_MSTR.CM_BRANCH,COMPANY_MSTR.CM_COY_LOGO, " _
                    & "COMPANY_MSTR.CM_BUSINESS_REG_NO, COMPANY_MSTR.CM_TAX_REG_NO, COMPANY_MSTR.CM_PAYMENT_TERM, " _
                    & "COMPANY_MSTR.CM_PAYMENT_METHOD, COMPANY_MSTR.CM_ACTUAL_TERMSANDCONDFILE, " _
                    & "COMPANY_MSTR.CM_HUB_TERMSANDCONDFILE, COMPANY_MSTR.CM_PWD_DURATION, COMPANY_MSTR.CM_TAX_CALC_BY, " _
                    & "COMPANY_MSTR.CM_CURRENCY_CODE, COMPANY_MSTR.CM_BCM_SET, COMPANY_MSTR.CM_BUDGET_FROM_DATE, " _
                    & "COMPANY_MSTR.CM_BUDGET_TO_DATE, COMPANY_MSTR.CM_RFQ_OPTION, COMPANY_MSTR.CM_LICENCE_PACKAGE, " _
                    & "COMPANY_MSTR.CM_LICENSE_USERS, COMPANY_MSTR.CM_SUB_START_DT, COMPANY_MSTR.CM_SUB_END_DT, " _
                    & "COMPANY_MSTR.CM_LICENSE_PRODUCTS, COMPANY_MSTR.CM_FINDEPT_MODE, COMPANY_MSTR.CM_PRIV_LABELING, " _
                    & "COMPANY_MSTR.CM_SKINS_ID, COMPANY_MSTR.CM_TRAINING, COMPANY_MSTR.CM_STATUS, " _
                    & "COMPANY_MSTR.CM_DELETED, COMPANY_MSTR.CM_MOD_BY, COMPANY_MSTR.CM_MOD_DT, COMPANY_MSTR.CM_ENT_BY, " _
                    & "COMPANY_MSTR.CM_ENT_DT, COMPANY_MSTR.CM_SKU, COMPANY_MSTR.CM_TRANS_NO, COMPANY_MSTR.CM_CONTACT, " _
                    & "COMPANY_MSTR.CM_REPORT_USERS, COMPANY_MSTR.CM_INV_APPR, COMPANY_MSTR.CM_MULTI_PO, " _
                    & "COMPANY_MSTR.CM_BA_CANCEL, " _
                    & "PO_MSTR.POM_PRINT_REMARK, PO_MSTR.POM_PRINT_CUSTOM_FIELDS, PO_MSTR.POM_SHIP_AMT, " _
                    & "INVOICE_MSTR.IM_SHIP_AMT " _
                    & "FROM INVOICE_MSTR " _
                    & "INNER JOIN PO_MSTR ON INVOICE_MSTR.IM_PO_INDEX = PO_MSTR.POM_PO_INDEX AND INVOICE_MSTR.IM_S_COY_ID = PO_MSTR.POM_S_COY_ID " _
                    & "INNER JOIN COMPANY_MSTR ON IM_B_COY_ID = COMPANY_MSTR.CM_COY_ID " _
                    & "LEFT OUTER JOIN company_dept_mstr ON company_dept_mstr.CDM_DEPT_INDEX = po_mstr.POM_DEPT_INDEX " _
                    & "INNER JOIN status_mstr ON status_mstr.STATUS_NO=INVOICE_MSTR.IM_INVOICE_STATUS AND STATUS_TYPE='INV' " _
                    & "WHERE (INVOICE_MSTR.IM_B_COY_ID = @prmCoyID) " _
                    & "AND IM_CREATED_ON>= @prmStartDate AND IM_CREATED_ON<=@prmEndDate " _
                    & "ORDER BY IM_CREATED_ON DESC"
            End With

            da = New MySqlDataAdapter(cmd)

            If Me.cboMonth.SelectedIndex > 0 Then
                dtDate = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue, 1)
                dtFrom = dtDate

                If Me.cboMonth.SelectedValue < 12 Then
                    dtDate = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue + 1, 1)
                    dtTo = DateAdd(DateInterval.Day, -1, dtDate)
                    dtTo = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue, dtTo.Day)

                Else    'December
                    dtTo = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue, 31)
                End If
            Else
            End If

            strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
            strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
            strTitle = Format(dtFrom, "MMM yyyy")
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyIdToken")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmStartDate", strBeginDate))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmEndDate", strEndDate))
            strUserId = Session("UserId")

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("InvSummary_xls_DataSetInvSummary", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = dispatcher.direct("Report", "InvSummary_pdf.rdlc", "Report") 'Server.MapPath("InvSummary_pdf.rdlc")  ''appPath & "Report\InvSummary_pdf.rdlc"
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationSettings.AppSettings.Get("Env") & "ReportCoyLogoPath"
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "prmrequestedby"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

                    Case "prmtitle"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strTitle)

                    Case "logo"
                        'par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationSettings.AppSettings("ReportCoyLogoPath") & strImgSrc)
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationSettings.AppSettings(strID) & strImgSrc)

                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            Dim deviceInfo As String = _
                "<DeviceInfo>" + _
                    "  <OutputFormat>EMF</OutputFormat>" + _
                    "</DeviceInfo>"

            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            strFileName = "InvSummaryReport.pdf"
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()
            'Dim fs As New FileStream(appPath & "Report\" & strFileName, FileMode.Create)
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
        Dim strTitle As String
        Dim strFileName As String = ""

        Try
            If Me.cboMonth.SelectedIndex > 0 Then
                dtDate = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue, 1)
                dtFrom = dtDate

                If Me.cboMonth.SelectedValue < 12 Then
                    dtDate = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue + 1, 1)
                    dtTo = DateAdd(DateInterval.Day, -1, dtDate)
                    dtTo = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue, dtTo.Day)

                Else    'December
                    dtTo = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue, 31)
                End If
            Else
            End If

            strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
            strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
            strTitle = Format(dtFrom, "MMM yyyy")

            myConnectionString = System.Configuration.ConfigurationSettings.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT INVOICE_MSTR.IM_INVOICE_NO AS 'Invoice No.',DATE_FORMAT(INVOICE_MSTR.IM_CREATED_ON,'%d/%m/%Y') AS 'Invoice Date', " _
                        & "DATE_FORMAT(INVOICE_MSTR.IM_PAYMENT_DATE,'%d/%m/%Y') AS 'Due Date',INVOICE_MSTR.IM_S_COY_NAME AS 'Vendor Name', " _
                        & "company_dept_mstr.CDM_DEPT_NAME AS 'Department',PO_MSTR.POM_BUYER_NAME AS 'Buyer Name'," _
                        & "STATUS_DESC AS 'Invoice Status',PO_MSTR.POM_PO_NO AS 'PO Number',DATE_FORMAT(PO_MSTR.POM_PO_DATE,'%d/%m/%Y') AS 'PO Date', " _
                        & "COMPANY_MSTR.CM_CURRENCY_CODE AS 'Currency', FORMAT(INVOICE_MSTR.IM_INVOICE_TOTAL,2) AS 'Amount' " _
                        & "FROM INVOICE_MSTR " _
                        & "INNER JOIN PO_MSTR ON INVOICE_MSTR.IM_PO_INDEX = PO_MSTR.POM_PO_INDEX  " _
                        & "AND INVOICE_MSTR.IM_S_COY_ID = PO_MSTR.POM_S_COY_ID  " _
                        & "INNER JOIN COMPANY_MSTR ON IM_B_COY_ID = COMPANY_MSTR.CM_COY_ID  " _
                        & "LEFT OUTER JOIN company_dept_mstr ON company_dept_mstr.CDM_DEPT_INDEX = po_mstr.POM_DEPT_INDEX  " _
                        & "INNER JOIN status_mstr ON status_mstr.STATUS_NO=INVOICE_MSTR.IM_INVOICE_STATUS AND STATUS_TYPE='INV'  " _
                        & "WHERE (INVOICE_MSTR.IM_B_COY_ID ='" & Session("CompanyIdToken") & "') " _
                        & "AND IM_CREATED_ON>= '" & strBeginDate & "' AND IM_CREATED_ON<='" & strEndDate & "' " _
                        & "ORDER BY IM_CREATED_ON DESC"
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
            strFileName = "InvSummaryReport" & "(" & Format(dtFrom, "MMMyyyy") & ").xls"
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

    Protected Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        If cboReportType.SelectedValue = "Excel" Then
            ExportToExcel()

        ElseIf cboReportType.SelectedValue = "PDF" Then
            ExportToPDF()
        End If
    End Sub
End Class