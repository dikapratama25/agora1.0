Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class ReportIPPCoAudit
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher
    Dim objDb As New EAD.DBCom

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strRptType As String
        Dim cbolist As New ListItem

        ViewState("type") = Request.QueryString("type")
        strRptType = ViewState("type")

        If Not (Page.IsPostBack) Then
            lblHeader.Text = "Company Audit Trail Report"
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
        Dim strBeginDate As String
        Dim strEndDate As String
        Dim strAuditOn As String
        Dim strFileName As String = ""

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            'Zulham Case 8317 13022015
            Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                If cboAudit.SelectedValue = "CC" Then
                    '.CommandText = "SELECT IC_COY_NAME, IFNULL(CODE_DESC,'') AS IC_COMPANY_CATEGORY, NULL AS IC_RESIDENT_TYPE, UM_USER_NAME AS IC_MOD_BY, IC_MOD_DATETIME AS AU_DATE FROM " & _
                    '            "(SELECT IC_INDEX, IC_COY_NAME, AU_ACTION, IC_COMPANY_CATEGORY, " & _
                    '            "IC_ENT_BY AS IC_MOD_BY, IC_ENT_DATETIME AS IC_MOD_DATETIME " & _
                    '            "FROM AU_IPP_COMPANY " & _
                    '            "WHERE AU_LOG_INDEX IN " & _
                    '            "(SELECT (SELECT AU_B.AU_LOG_INDEX FROM AU_IPP_COMPANY AU_B WHERE AU_B.IC_COY_NAME = AU.IC_COY_NAME " & _
                    '            "AND AU_B.AU_ACTION = 'I' AND AU_B.IC_COY_TYPE = 'V' " & _
                    '            "ORDER BY AU_B.IC_INDEX DESC LIMIT 1) AS LOGINDEX " & _
                    '            "FROM AU_IPP_COMPANY AU WHERE AU.AU_ACTION = 'I' AND AU.IC_COY_TYPE = 'V' " & _
                    '            "AND AU.IC_COY_ID = @prmCoyID " & _
                    '            "AND AU.AU_DATE >=@prmStartDate AND AU.AU_DATE <=@prmEndDate " & _
                    '            "AND IC_COMPANY_CATEGORY IS NOT NULL " & _
                    '            "GROUP BY AU.IC_COY_NAME) " & _
                    '            "UNION ALL " & _
                    '            "SELECT IC_INDEX, IC_COY_NAME, AU_ACTION, IC_COMPANY_CATEGORY, IC_MOD_BY, IC_MOD_DATETIME " & _
                    '            "FROM AU_IPP_COMPANY " & _
                    '            "WHERE AU_LOG_INDEX IN " & _
                    '            "(SELECT NEXT_INDEX FROM " & _
                    '            "(SELECT AU.AU_LOG_INDEX, AU.IC_COMPANY_CATEGORY, " & _
                    '            "IFNULL((SELECT AU_B.IC_COMPANY_CATEGORY FROM AU_IPP_COMPANY AU_B WHERE AU_B.AU_LOG_INDEX > AU.AU_LOG_INDEX " & _
                    '            "AND AU_B.IC_INDEX = AU.IC_INDEX LIMIT 1), AU.IC_COMPANY_CATEGORY) AS NEXT_CAT, " & _
                    '            "(SELECT AU_C.AU_LOG_INDEX FROM AU_IPP_COMPANY AU_C WHERE AU_C.AU_LOG_INDEX > AU.AU_LOG_INDEX " & _
                    '            "AND AU_C.IC_INDEX = AU.IC_INDEX LIMIT 1) AS NEXT_INDEX " & _
                    '            "FROM AU_IPP_COMPANY AU " & _
                    '            "WHERE AU.AU_ACTION = 'U' AND AU.IC_COY_TYPE = 'V' " & _
                    '            "AND AU.IC_COY_ID = @prmCoyID " & _
                    '            "AND AU.AU_DATE >=@prmStartDate AND AU.AU_DATE <=@prmEndDate " & _
                    '            ") tb " & _
                    '            "WHERE IC_COMPANY_CATEGORY <> NEXT_CAT)) tb " & _
                    '            "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'IPPCC' AND CODE_ABBR = IC_COMPANY_CATEGORY " & _
                    '            "LEFT JOIN USER_MSTR ON UM_USER_ID = IC_MOD_BY AND UM_COY_ID = @prmCoyID " & _
                    '            "ORDER BY IC_MOD_DATETIME DESC,  IC_COY_NAME "

                    .CommandText = "SELECT IC_COY_NAME, IFNULL(CODE_DESC,'') AS IC_COMPANY_CATEGORY, NULL AS IC_RESIDENT_TYPE, UM_USER_NAME AS IC_MOD_BY, IC_MOD_DATETIME AS AU_DATE FROM " & _
                               "(SELECT IC_INDEX, IC_COY_NAME, AU_ACTION, IC_COMPANY_CATEGORY, IC_ENT_BY AS IC_MOD_BY, " & _
                               "IC_ENT_DATETIME AS IC_MOD_DATETIME " & _
                               "FROM AU_IPP_COMPANY " & _
                               "INNER JOIN (SELECT (SELECT AU_B.AU_LOG_INDEX FROM AU_IPP_COMPANY AU_B WHERE AU_B.IC_COY_NAME = AU.IC_COY_NAME AND AU_B.AU_ACTION = 'I' AND AU_B.IC_COY_TYPE = 'V' ORDER BY AU_B.IC_INDEX DESC LIMIT 1) AS LOGINDEX " & _
                               "FROM AU_IPP_COMPANY AU WHERE AU.AU_ACTION = 'I' AND AU.IC_COY_TYPE = 'V' AND AU.IC_COY_ID = @prmCoyID " & _
                               "AND AU.AU_DATE >=@prmStartDate AND AU.AU_DATE <=@prmEndDate AND IC_COMPANY_CATEGORY " & _
                               "IS NOT NULL GROUP BY AU.IC_COY_NAME) tb ON tb.LOGINDEX = AU_LOG_INDEX " & _
                               "UNION ALL " & _
                               "SELECT IC_INDEX, IC_COY_NAME, AU_ACTION, IC_COMPANY_CATEGORY, IC_MOD_BY, IC_MOD_DATETIME " & _
                               "FROM AU_IPP_COMPANY " & _
                               "INNER JOIN (SELECT NEXT_INDEX FROM (SELECT AU.AU_LOG_INDEX, AU.IC_COMPANY_CATEGORY, " & _
                               "IFNULL((SELECT AU_B.IC_COMPANY_CATEGORY FROM AU_IPP_COMPANY AU_B WHERE AU_B.AU_LOG_INDEX > AU.AU_LOG_INDEX AND AU_B.IC_INDEX = AU.IC_INDEX LIMIT 1), AU.IC_COMPANY_CATEGORY) AS NEXT_CAT, " & _
                               "(SELECT AU_C.AU_LOG_INDEX FROM AU_IPP_COMPANY AU_C WHERE AU_C.AU_LOG_INDEX > AU.AU_LOG_INDEX AND AU_C.IC_INDEX = AU.IC_INDEX LIMIT 1) AS NEXT_INDEX " & _
                               "FROM AU_IPP_COMPANY AU WHERE AU.AU_ACTION = 'U' AND AU.IC_COY_TYPE = 'V' AND AU.IC_COY_ID = @prmCoyID " & _
                               "AND AU.AU_DATE >=@prmStartDate AND AU.AU_DATE <=@prmEndDate) tb " & _
                               "WHERE IC_COMPANY_CATEGORY <> NEXT_CAT) tb ON tb.NEXT_INDEX = AU_LOG_INDEX) tb " & _
                               "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'IPPCC' AND CODE_ABBR = IC_COMPANY_CATEGORY " & _
                               "LEFT JOIN USER_MSTR ON UM_USER_ID = IC_MOD_BY AND UM_COY_ID = @prmCoyID " & _
                               "ORDER BY IC_MOD_DATETIME DESC,  IC_COY_NAME "
                Else
                    .CommandText = "SELECT IC_COY_NAME, CASE WHEN IC_RESIDENT_TYPE = 'Y' THEN 'Resident' ELSE 'Non Resident' END AS IC_RESIDENT_TYPE, NULL AS IC_COMPANY_CATEGORY, " & _
                                "UM_USER_NAME AS IC_MOD_BY, IC_MOD_DATETIME AS AU_DATE FROM " & _
                                "(SELECT IC_INDEX, IC_COY_NAME, AU_ACTION, IC_RESIDENT_TYPE, " & _
                                "IC_ENT_BY AS IC_MOD_BY, IC_ENT_DATETIME AS IC_MOD_DATETIME " & _
                                "FROM AU_IPP_COMPANY " & _
                                "INNER JOIN " & _
                                "(SELECT (SELECT AU_B.AU_LOG_INDEX FROM AU_IPP_COMPANY AU_B WHERE AU_B.IC_COY_NAME = AU.IC_COY_NAME " & _
                                "AND AU_B.AU_ACTION = 'I' AND AU_B.IC_COY_TYPE = 'V' " & _
                                "ORDER BY AU_B.IC_INDEX DESC LIMIT 1) AS LOGINDEX " & _
                                "FROM AU_IPP_COMPANY AU WHERE AU.AU_ACTION = 'I' AND AU.IC_COY_TYPE = 'V' " & _
                                "AND AU.IC_COY_ID = @prmCoyID " & _
                                "AND AU.AU_DATE >=@prmStartDate AND AU.AU_DATE <=@prmEndDate " & _
                                "GROUP BY AU.IC_COY_NAME) tb ON tb.LOGINDEX = AU_LOG_INDEX " & _
                                "UNION ALL " & _
                                "SELECT IC_INDEX, IC_COY_NAME, AU_ACTION, IC_RESIDENT_TYPE, IC_MOD_BY, IC_MOD_DATETIME " & _
                                "FROM AU_IPP_COMPANY " & _
                                "INNER JOIN " & _
                                "(SELECT NEXT_INDEX FROM " & _
                                "(SELECT AU.AU_LOG_INDEX, AU.IC_RESIDENT_TYPE, " & _
                                "IFNULL((SELECT AU_B.IC_RESIDENT_TYPE FROM AU_IPP_COMPANY AU_B WHERE AU_B.AU_LOG_INDEX > AU.AU_LOG_INDEX " & _
                                "AND AU_B.IC_INDEX = AU.IC_INDEX LIMIT 1), AU.IC_RESIDENT_TYPE) AS NEXT_TYPE, " & _
                                "(SELECT AU_C.AU_LOG_INDEX FROM AU_IPP_COMPANY AU_C WHERE AU_C.AU_LOG_INDEX > AU.AU_LOG_INDEX " & _
                                "AND AU_C.IC_INDEX = AU.IC_INDEX LIMIT 1) AS NEXT_INDEX " & _
                                "FROM AU_IPP_COMPANY AU " & _
                                "WHERE AU.AU_ACTION = 'U' AND AU.IC_COY_TYPE = 'V' " & _
                                "AND AU.IC_COY_ID = @prmCoyID " & _
                                "AND AU.AU_DATE >=@prmStartDate AND AU.AU_DATE <=@prmEndDate " & _
                                ") tb " & _
                                "WHERE IC_RESIDENT_TYPE <> NEXT_TYPE) tb ON tb.NEXT_INDEX = AU_LOG_INDEX " & _
                                ") tb " & _
                                "LEFT JOIN USER_MSTR ON UM_USER_ID = IC_MOD_BY AND UM_COY_ID = @prmCoyID " & _
                                "ORDER BY IC_MOD_DATETIME DESC,  IC_COY_NAME "
                End If
            End With

            da = New MySqlDataAdapter(cmd)

            dtFrom = Me.txtSDate.Text
            dtTo = Me.txtEndDate.Text
            strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
            strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
            'Zulham Case 8317 13022015
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", strDefIPPCompID))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmStartDate", strBeginDate))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmEndDate", strEndDate))
            strUserId = Session("UserName") 'Session("UserId")
            strCoyName = Session("CompanyName")
            strAuditOn = cboAudit.SelectedValue

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("IPPCompanyAuditTrail_DataSetIPPCompanyAuditTrail", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = dispatcher.direct("Report", "IPPCompanyAuditTrail.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")
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

                    Case "dtfrom"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strBeginDate)

                    Case "dtto"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strEndDate)

                    Case "auditon"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strAuditOn)
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

            strFileName = "IPPCompanyAuditTrailReport.pdf"

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
        Dim i As Integer
        Dim strBeginDate As String
        Dim strEndDate As String
        Dim strTitle As String
        Dim strFileName As String = ""

        Try
            dtFrom = Me.txtSDate.Text
            dtTo = Me.txtEndDate.Text
            strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
            strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
            strTitle = Format(dtFrom, "MMM yyyy")

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            'Zulham Case 8317 13022015
            Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                If cboAudit.SelectedValue = "CC" Then
                    '.CommandText = "SELECT IC_COY_NAME AS 'Company Name', IFNULL(CODE_DESC,'') AS 'Company Category', UM_USER_NAME AS 'Modified By', DATE_FORMAT(IC_MOD_DATETIME, '%d/%m/%Y') AS 'Modified Date' FROM " & _
                    '            "(SELECT IC_INDEX, IC_COY_NAME, AU_ACTION, IC_COMPANY_CATEGORY, " & _
                    '            "IC_ENT_BY AS IC_MOD_BY, IC_ENT_DATETIME AS IC_MOD_DATETIME " & _
                    '            "FROM AU_IPP_COMPANY " & _
                    '            "WHERE AU_LOG_INDEX IN " & _
                    '            "(SELECT (SELECT AU_B.AU_LOG_INDEX FROM AU_IPP_COMPANY AU_B WHERE AU_B.IC_COY_NAME = AU.IC_COY_NAME " & _
                    '            "AND AU_B.AU_ACTION = 'I' AND AU_B.IC_COY_TYPE = 'V' " & _
                    '            "ORDER BY AU_B.IC_INDEX DESC LIMIT 1) AS LOGINDEX " & _
                    '            "FROM AU_IPP_COMPANY AU WHERE AU.AU_ACTION = 'I' AND AU.IC_COY_TYPE = 'V' " & _
                    '            "AND AU.IC_COY_ID = '" & Session("CompanyId") & "' " & _
                    '            "AND AU.AU_DATE >='" & strBeginDate & "' AND AU.AU_DATE <='" & strEndDate & "' " & _
                    '            "AND IC_COMPANY_CATEGORY IS NOT NULL " & _
                    '            "GROUP BY AU.IC_COY_NAME) " & _
                    '            "UNION ALL " & _
                    '            "SELECT IC_INDEX, IC_COY_NAME, AU_ACTION, IC_COMPANY_CATEGORY, IC_MOD_BY, IC_MOD_DATETIME " & _
                    '            "FROM AU_IPP_COMPANY " & _
                    '            "WHERE AU_LOG_INDEX IN " & _
                    '            "(SELECT NEXT_INDEX FROM " & _
                    '            "(SELECT AU.AU_LOG_INDEX, AU.IC_COMPANY_CATEGORY, " & _
                    '            "IFNULL((SELECT AU_B.IC_COMPANY_CATEGORY FROM AU_IPP_COMPANY AU_B WHERE AU_B.AU_LOG_INDEX > AU.AU_LOG_INDEX " & _
                    '            "AND AU_B.IC_INDEX = AU.IC_INDEX LIMIT 1), AU.IC_COMPANY_CATEGORY) AS NEXT_CAT, " & _
                    '            "(SELECT AU_C.AU_LOG_INDEX FROM AU_IPP_COMPANY AU_C WHERE AU_C.AU_LOG_INDEX > AU.AU_LOG_INDEX " & _
                    '            "AND AU_C.IC_INDEX = AU.IC_INDEX LIMIT 1) AS NEXT_INDEX " & _
                    '            "FROM AU_IPP_COMPANY AU " & _
                    '            "WHERE AU.AU_ACTION = 'U' AND AU.IC_COY_TYPE = 'V' " & _
                    '            "AND AU.IC_COY_ID = '" & Session("CompanyId") & "' " & _
                    '            "AND AU.AU_DATE >='" & strBeginDate & "' AND AU.AU_DATE <='" & strEndDate & "' " & _
                    '            ") tb " & _
                    '            "WHERE IC_COMPANY_CATEGORY <> NEXT_CAT)) tb " & _
                    '            "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'IPPCC' AND CODE_ABBR = IC_COMPANY_CATEGORY " & _
                    '            "LEFT JOIN USER_MSTR ON UM_USER_ID = IC_MOD_BY AND UM_COY_ID = '" & Session("CompanyId") & "' " & _
                    '            "ORDER BY IC_MOD_DATETIME DESC,  IC_COY_NAME "
                    'Zulham Case 8317 13022015
                    .CommandText = "SELECT IC_COY_NAME AS 'Company Name', IFNULL(CODE_DESC,'') AS 'Company Category', " & _
                                "UM_USER_NAME AS 'Modified By', DATE_FORMAT(IC_MOD_DATETIME, '%d/%m/%Y') AS 'Modified Date' " & _
                                "FROM " & _
                                "(SELECT IC_INDEX, IC_COY_NAME, AU_ACTION, IC_COMPANY_CATEGORY, IC_ENT_BY AS IC_MOD_BY, " & _
                                "IC_ENT_DATETIME AS IC_MOD_DATETIME " & _
                                "FROM AU_IPP_COMPANY " & _
                                "INNER JOIN (SELECT (SELECT AU_B.AU_LOG_INDEX FROM AU_IPP_COMPANY AU_B WHERE AU_B.IC_COY_NAME = AU.IC_COY_NAME AND AU_B.AU_ACTION = 'I' AND AU_B.IC_COY_TYPE = 'V' ORDER BY AU_B.IC_INDEX DESC LIMIT 1) AS LOGINDEX " & _
                                "FROM AU_IPP_COMPANY AU WHERE AU.AU_ACTION = 'I' AND AU.IC_COY_TYPE = 'V' AND AU.IC_COY_ID = '" & strDefIPPCompID & "' " & _
                                "AND AU.AU_DATE >='" & strBeginDate & "' AND AU.AU_DATE <='" & strEndDate & "' AND IC_COMPANY_CATEGORY " & _
                                "IS NOT NULL GROUP BY AU.IC_COY_NAME) tb ON tb.LOGINDEX = AU_LOG_INDEX " & _
                                "UNION ALL " & _
                                "SELECT IC_INDEX, IC_COY_NAME, AU_ACTION, IC_COMPANY_CATEGORY, IC_MOD_BY, IC_MOD_DATETIME " & _
                                "FROM AU_IPP_COMPANY " & _
                                "INNER JOIN (SELECT NEXT_INDEX FROM (SELECT AU.AU_LOG_INDEX, AU.IC_COMPANY_CATEGORY, " & _
                                "IFNULL((SELECT AU_B.IC_COMPANY_CATEGORY FROM AU_IPP_COMPANY AU_B WHERE AU_B.AU_LOG_INDEX > AU.AU_LOG_INDEX AND AU_B.IC_INDEX = AU.IC_INDEX LIMIT 1), AU.IC_COMPANY_CATEGORY) AS NEXT_CAT, " & _
                                "(SELECT AU_C.AU_LOG_INDEX FROM AU_IPP_COMPANY AU_C WHERE AU_C.AU_LOG_INDEX > AU.AU_LOG_INDEX AND AU_C.IC_INDEX = AU.IC_INDEX LIMIT 1) AS NEXT_INDEX " & _
                                "FROM AU_IPP_COMPANY AU WHERE AU.AU_ACTION = 'U' AND AU.IC_COY_TYPE = 'V' AND AU.IC_COY_ID = '" & strDefIPPCompID & "' " & _
                                "AND AU.AU_DATE >='" & strBeginDate & "' AND AU.AU_DATE <='" & strEndDate & "' ) tb " & _
                                "WHERE IC_COMPANY_CATEGORY <> NEXT_CAT) tb ON tb.NEXT_INDEX = AU_LOG_INDEX) tb " & _
                                "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'IPPCC' AND CODE_ABBR = IC_COMPANY_CATEGORY " & _
                                "LEFT JOIN USER_MSTR ON UM_USER_ID = IC_MOD_BY AND UM_COY_ID = '" & Session("CompanyId") & "' " & _
                                "ORDER BY IC_MOD_DATETIME DESC,  IC_COY_NAME "
                Else
                    'Zulham Case 8317 13022015
                    .CommandText = "SELECT IC_COY_NAME AS 'Company Name', CASE WHEN IC_RESIDENT_TYPE = 'Y' THEN 'Resident' ELSE 'Non Resident' END AS 'Resident Type', " & _
                                "UM_USER_NAME AS 'Modified By', DATE_FORMAT(IC_MOD_DATETIME, '%d/%m/%Y') AS 'Modified Date' FROM " & _
                                "(SELECT IC_INDEX, IC_COY_NAME, AU_ACTION, IC_RESIDENT_TYPE, " & _
                                "IC_ENT_BY AS IC_MOD_BY, IC_ENT_DATETIME AS IC_MOD_DATETIME " & _
                                "FROM AU_IPP_COMPANY " & _
                                "INNER JOIN " & _
                                "(SELECT (SELECT AU_B.AU_LOG_INDEX FROM AU_IPP_COMPANY AU_B WHERE AU_B.IC_COY_NAME = AU.IC_COY_NAME " & _
                                "AND AU_B.AU_ACTION = 'I' AND AU_B.IC_COY_TYPE = 'V' " & _
                                "ORDER BY AU_B.IC_INDEX DESC LIMIT 1) AS LOGINDEX " & _
                                "FROM AU_IPP_COMPANY AU WHERE AU.AU_ACTION = 'I' AND AU.IC_COY_TYPE = 'V' " & _
                                "AND AU.IC_COY_ID = '" & strDefIPPCompID & "' " & _
                                "AND AU.AU_DATE >='" & strBeginDate & "' AND AU.AU_DATE <='" & strEndDate & "' " & _
                                "GROUP BY AU.IC_COY_NAME) tb ON tb.LOGINDEX = AU_LOG_INDEX " & _
                                "UNION ALL " & _
                                "SELECT IC_INDEX, IC_COY_NAME, AU_ACTION, IC_RESIDENT_TYPE, IC_MOD_BY, IC_MOD_DATETIME " & _
                                "FROM AU_IPP_COMPANY " & _
                                "INNER JOIN " & _
                                "(SELECT NEXT_INDEX FROM " & _
                                "(SELECT AU.AU_LOG_INDEX, AU.IC_RESIDENT_TYPE, " & _
                                "IFNULL((SELECT AU_B.IC_RESIDENT_TYPE FROM AU_IPP_COMPANY AU_B WHERE AU_B.AU_LOG_INDEX > AU.AU_LOG_INDEX " & _
                                "AND AU_B.IC_INDEX = AU.IC_INDEX LIMIT 1), AU.IC_RESIDENT_TYPE) AS NEXT_TYPE, " & _
                                "(SELECT AU_C.AU_LOG_INDEX FROM AU_IPP_COMPANY AU_C WHERE AU_C.AU_LOG_INDEX > AU.AU_LOG_INDEX " & _
                                "AND AU_C.IC_INDEX = AU.IC_INDEX LIMIT 1) AS NEXT_INDEX " & _
                                "FROM AU_IPP_COMPANY AU " & _
                                "WHERE AU.AU_ACTION = 'U' AND AU.IC_COY_TYPE = 'V' " & _
                                "AND AU.IC_COY_ID = '" & strDefIPPCompID & "' " & _
                                "AND AU.AU_DATE >='" & strBeginDate & "' AND AU.AU_DATE <='" & strEndDate & "' " & _
                                ") tb " & _
                                "WHERE IC_RESIDENT_TYPE <> NEXT_TYPE) tb ON tb.NEXT_INDEX = AU_LOG_INDEX " & _
                                ") tb " & _
                                "LEFT JOIN USER_MSTR ON UM_USER_ID = IC_MOD_BY AND UM_COY_ID = '" & Session("CompanyId") & "' " & _
                                "ORDER BY IC_MOD_DATETIME DESC,  IC_COY_NAME "
                End If
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
            strFileName = "IPPCompanyAuditTrailReport" & "(" & Format(dtFrom, "MMMyyyy") & ").xls"
            Dim attachment As String = "attachment;filename=" & strFileName
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"

            Dim dc As DataColumn

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

End Class