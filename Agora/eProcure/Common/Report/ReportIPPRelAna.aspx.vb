Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class ReportIPPRelAna
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher
    Dim objDb As New EAD.DBCom

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strRptType As String
        Dim cbolist As New ListItem

        ViewState("type") = Request.QueryString("type")
        strRptType = ViewState("type")

        'Modified for IPP GST Stage 2A - YAP - 13 Feb 2015
        Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
        If strDefIPPCompID = "" Then
            strDefIPPCompID = HttpContext.Current.Session("CompanyID")
        End If

        If Not (Page.IsPostBack) Then
            Select Case strRptType
                Case "RELATEDPARTIES"
                    lblHeader.Text = "IPPA0020 - Related Parties Transaction (RPT) Report"
                    tr1.Style("display") = ""
                    cbolist.Value = ""
                    cbolist.Text = "ALL"
                    Common.FillDefault(cboEntity, "CODE_MSTR", "CODE_DESC", "CODE_ABBR", , " CODE_CATEGORY = 'IPPCC' AND CODE_DELETED <> 'Y'")
                    cboEntity.Items.Insert(0, cbolist)

                Case "GLANALYSIS"
                    lblHeader.Text = "IPPA0018 - GL Analysis Report"
                    tr2.Style("display") = ""
                    ValGLCode.Enabled = True
                    cbolist.Value = ""
                    cbolist.Text = "---Select---"
                    Common.FillDefault(cboGLCode, "COMPANY_B_GL_CODE", "CBG_B_GL_CODE", "CBG_B_GL_CODE", , " CBG_STATUS = 'A' AND CBG_B_COY_ID = '" & strDefIPPCompID & "' AND CBG_B_GL_CODE IN (SELECT DISTINCT IGG_GL_CODE FROM IPP_GLRULE_GL INNER JOIN IPP_GLRULE ON IGG_GLRULE_INDEX WHERE IG_COY_ID = '" & strDefIPPCompID & "')")
                    cboGLCode.Items.Insert(0, cbolist)
            End Select

        End If

        lnkBack.NavigateUrl = dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId & "&type=" & Request.QueryString("type"))

    End Sub

    'Private Sub PreviewPOSummary()
    '    Dim ds As New DataSet
    '    Dim conn As MySqlConnection = Nothing
    '    Dim cmd As MySqlCommand = Nothing
    '    Dim da As MySqlDataAdapter = Nothing
    '    Dim rdr As MySqlDataReader = Nothing
    '    Dim myConnectionString As String
    '    Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
    '    'Dim objFile As New FileManagement
    '    Dim strImgSrc As String
    '    Dim strUserId As String
    '    Dim dtFrom As Date
    '    Dim dtTo As Date
    '    Dim dtDate As Date
    '    Dim strBeginDate As String
    '    Dim strEndDate As String
    '    Dim strTitle As String
    '    Dim strFileName As String = ""

    '    'strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request(Trim("BCoyID")), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

    '    Try

    '        myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

    '        conn = New MySqlConnection(myConnectionString)
    '        conn.Open()

    '        cmd = New MySqlCommand
    '        With cmd
    '            .Connection = conn
    '            .CommandType = CommandType.Text
    '            .CommandText = "SELECT po_mstr.POM_PO_INDEX, po_mstr.POM_PO_NO, po_mstr.POM_B_COY_ID, po_mstr.POM_BUYER_ID, po_mstr.POM_BUYER_NAME, " _
    '                    & "po_mstr.POM_DEPT_INDEX, company_dept_mstr.CDM_DEPT_NAME, po_mstr.POM_S_COY_ID, po_mstr.POM_S_COY_NAME, po_mstr.POM_S_ATTN,  " _
    '                    & "po_mstr.POM_S_REMARK, po_mstr.POM_PO_DATE, po_mstr.POM_FREIGHT_TERMS, po_mstr.POM_PAYMENT_TERM, " _
    '                    & "po_mstr.POM_PAYMENT_METHOD, po_mstr.POM_SHIPMENT_MODE, po_mstr.POM_SHIPMENT_TERM, po_mstr.POM_CURRENCY_CODE,  " _
    '                    & "po_mstr.POM_EXCHANGE_RATE, po_mstr.POM_PAYMENT_TERM_CODE, po_mstr.POM_SHIP_VIA, po_mstr.POM_PO_STATUS,STATUS_DESC,  " _
    '                    & "po_mstr.POM_STATUS_CHANGED_BY, po_mstr.POM_STATUS_CHANGED_ON, po_mstr.POM_EXTERNAL_REMARK, po_mstr.POM_CREATED_BY,  " _
    '                    & "po_mstr.POM_PO_COST, po_mstr.POM_BILLING_METHOD, po_mstr.POM_PO_PREFIX, po_mstr.POM_DEPT_INDEX AS Expr1,  " _
    '                    & "po_mstr.POM_ACCEPTED_DATE, po_mstr.POM_DOWNLOADED_DATE, po_mstr.POM_ARCHIVE_IND, po_mstr.POM_TERMANDCOND,  " _
    '                    & "po_mstr.POM_REFERENCE_NO, po_mstr.POM_EXTERNAL_IND, po_mstr.POM_PRINT_REMARK, po_mstr.POM_PRINT_CUSTOM_FIELDS,  " _
    '                    & "company_mstr.CM_COY_ID, company_mstr.CM_COY_NAME,  " _
    '                    & "company_mstr.CM_COY_TYPE, company_mstr.CM_PARENT_COY_ID, company_mstr.CM_ACCT_NO, company_mstr.CM_BANK,  " _
    '                    & "company_mstr.CM_BRANCH, company_mstr.CM_ADDR_LINE1, company_mstr.CM_ADDR_LINE2, company_mstr.CM_ADDR_LINE3,  " _
    '                    & "company_mstr.CM_POSTCODE, company_mstr.CM_CITY, company_mstr.CM_STATE, company_mstr.CM_COUNTRY, company_mstr.CM_PHONE,  " _
    '                    & "company_mstr.CM_FAX, company_mstr.CM_EMAIL, company_mstr.CM_COY_LOGO, company_mstr.CM_BUSINESS_REG_NO,  " _
    '                    & "company_mstr.CM_TAX_REG_NO, company_mstr.CM_PAYMENT_TERM, company_mstr.CM_PAYMENT_METHOD,  " _
    '                    & "company_mstr.CM_ACTUAL_TERMSANDCONDFILE, company_mstr.CM_HUB_TERMSANDCONDFILE, company_mstr.CM_PWD_DURATION,  " _
    '                    & "company_mstr.CM_TAX_CALC_BY, company_mstr.CM_CURRENCY_CODE, company_mstr.CM_BCM_SET, company_mstr.CM_BUDGET_FROM_DATE,  " _
    '                    & "company_mstr.CM_BUDGET_TO_DATE, company_mstr.CM_RFQ_OPTION, company_mstr.CM_LICENCE_PACKAGE,  " _
    '                    & "company_mstr.CM_LICENSE_USERS, company_mstr.CM_SUB_START_DT, company_mstr.CM_SUB_END_DT,  " _
    '                    & "company_mstr.CM_LICENSE_PRODUCTS, company_mstr.CM_FINDEPT_MODE, company_mstr.CM_PRIV_LABELING, company_mstr.CM_SKINS_ID,  " _
    '                    & "company_mstr.CM_TRAINING, company_mstr.CM_STATUS, company_mstr.CM_DELETED, company_mstr.CM_MOD_BY, company_mstr.CM_MOD_DT,  " _
    '                    & "company_mstr.CM_ENT_BY, company_mstr.CM_ENT_DT, company_mstr.CM_SKU, company_mstr.CM_TRANS_NO, company_mstr.CM_CONTACT,  " _
    '                    & "company_mstr.CM_REPORT_USERS, company_mstr.CM_INV_APPR, company_mstr.CM_MULTI_PO, company_mstr.CM_BA_CANCEL,  " _
    '                    & "user_mstr.UM_AUTO_NO, user_mstr.UM_USER_ID, user_mstr.UM_DELETED, user_mstr.UM_PASSWORD, user_mstr.UM_USER_NAME,  " _
    '                    & "user_mstr.UM_COY_ID, user_mstr.UM_DEPT_ID, user_mstr.UM_EMAIL, user_mstr.UM_APP_LIMIT, user_mstr.UM_DESIGNATION,  " _
    '                    & "user_mstr.UM_TEL_NO, user_mstr.UM_FAX_NO, user_mstr.UM_USER_SUSPEND_IND, user_mstr.UM_PASSWORD_LAST_CHANGED,  " _
    '                    & "user_mstr.UM_NEW_PASSWORD_IND, user_mstr.UM_NEXT_EXPIRE_DT, user_mstr.UM_LAST_LOGIN_DT, user_mstr.UM_QUESTION,  " _
    '                    & "user_mstr.UM_ANSWER, user_mstr.UM_MASS_APP, user_mstr.UM_STATUS, user_mstr.UM_MOD_BY, user_mstr.UM_MOD_DT,  " _
    '                    & "user_mstr.UM_ENT_BY, user_mstr.UM_ENT_DATE, user_mstr.UM_RECORD_COUNT, user_mstr.UM_EMAIL_CC, user_mstr.UM_INVOICE_APP_LIMIT,  " _
    '                    & "user_mstr.UM_INVOICE_MASS_APP, po_mstr.POM_SHIP_AMT " _
    '                    & "FROM po_mstr INNER JOIN " _
    '                    & "company_mstr ON po_mstr.POM_B_COY_ID = company_mstr.CM_COY_ID INNER JOIN " _
    '                    & "user_mstr ON po_mstr.POM_BUYER_ID = user_mstr.UM_USER_ID AND po_mstr.POM_B_COY_ID = user_mstr.UM_COY_ID " _
    '                    & "LEFT OUTER JOIN company_dept_mstr ON company_dept_mstr.CDM_DEPT_INDEX = po_mstr.POM_DEPT_INDEX " _
    '                    & "INNER JOIN status_mstr ON status_mstr.STATUS_NO=PO_MSTR.POM_PO_STATUS AND STATUS_TYPE='PO' " _
    '                    & "WHERE (po_mstr.POM_B_COY_ID = @prmCoyID) " _
    '                    & "AND POM_PO_DATE >=@prmStartDate AND POM_PO_DATE <=@prmEndDate ORDER BY POM_PO_DATE desc"
    '        End With

    '        da = New MySqlDataAdapter(cmd)

    '        If Me.cboMonth.SelectedIndex > 0 Then
    '            dtDate = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue, 1)
    '            dtFrom = dtDate

    '            If Me.cboMonth.SelectedValue < 12 Then
    '                dtDate = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue + 1, 1)
    '                dtTo = DateAdd(DateInterval.Day, -1, dtDate)
    '                dtTo = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue, dtTo.Day)

    '            Else    'December
    '                dtTo = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue, 31)
    '            End If
    '        Else
    '        End If

    '        strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
    '        strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
    '        strTitle = Format(dtFrom, "MMM yyyy")
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyIdToken")))
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmStartDate", strBeginDate))
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmEndDate", strEndDate))
    '        strUserId = Session("UserId")

    '        da.Fill(ds)
    '        Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("POSummary_DataSetPOSummary", ds.Tables(0))
    '        Dim localreport As New LocalReport
    '        'localreport.DataSources.Clear()
    '        'localreport.DataSources.Add(rptDataSource)
    '        'localreport.ReportPath = appPath & "Report\POSummary_pdf.rdlc" '"Report\POSummary.rdlc"
    '        'localreport.EnableExternalImages = True

    '        'Dim I As Byte
    '        'Dim GetParameter As String = ""
    '        'Dim TotalParameter As Byte
    '        'TotalParameter = localreport.GetParameters.Count
    '        'Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
    '        ''Dim paramlist As New Generic.List(Of ReportParameter)
    '        'For I = 0 To localreport.GetParameters.Count - 1
    '        '    GetParameter = localreport.GetParameters.Item(I).Name
    '        '    Select Case LCase(GetParameter)
    '        '        Case "prmrequestedby"
    '        '            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

    '        '        Case "prmtitle"
    '        '            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strTitle)

    '        '        Case Else
    '        '    End Select
    '        'Next
    '        'localreport.SetParameters(par)
    '        'localreport.Refresh()

    '        'Dim deviceInfo As String = _
    '        '    "<DeviceInfo>" + _
    '        '        "  <OutputFormat>EMF</OutputFormat>" + _
    '        '        "</DeviceInfo>"

    '        Select Case cboReportType.SelectedValue
    '            Case "Excel"
    '                localreport.DataSources.Clear()
    '                localreport.DataSources.Add(rptDataSource)
    '                localreport.ReportPath = appPath & "Report\POSummary.rdlc"
    '                localreport.EnableExternalImages = True

    '                Dim I As Byte
    '                Dim GetParameter As String = ""
    '                Dim TotalParameter As Byte
    '                TotalParameter = localreport.GetParameters.Count
    '                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
    '                'Dim paramlist As New Generic.List(Of ReportParameter)
    '                For I = 0 To localreport.GetParameters.Count - 1
    '                    GetParameter = localreport.GetParameters.Item(I).Name
    '                    Select Case LCase(GetParameter)
    '                        Case "prmrequestedby"
    '                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

    '                        Case "prmtitle"
    '                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strTitle)

    '                        Case Else
    '                    End Select
    '                Next
    '                localreport.SetParameters(par)
    '                localreport.Refresh()

    '                Dim deviceInfo As String = _
    '                    "<DeviceInfo>" + _
    '                        "  <OutputFormat>EMF</OutputFormat>" + _
    '                        "</DeviceInfo>"

    '                Dim PDF As Byte() = localreport.Render("Excel", deviceInfo, Nothing, Nothing, "xls", Nothing, Nothing)
    '                strFileName = "POSummaryReport.xls"
    '                Dim fs As New FileStream(appPath & "Report\" & strFileName, FileMode.Create)
    '                fs.Write(PDF, 0, PDF.Length)
    '                fs.Close()

    '                Response.ContentType = "application/x-download"
    '                Response.AddHeader("Content-Disposition", "attachment;filename=" & strFileName)
    '                Response.WriteFile(Server.MapPath(strFileName))
    '                Response.End()

    '            Case "PDF"
    '                localreport.DataSources.Clear()
    '                localreport.DataSources.Add(rptDataSource)
    '                localreport.ReportPath = appPath & "Report\POSummary_pdf.rdlc" '"Report\POSummary.rdlc"
    '                localreport.EnableExternalImages = True

    '                Dim I As Byte
    '                Dim GetParameter As String = ""
    '                Dim TotalParameter As Byte
    '                TotalParameter = localreport.GetParameters.Count
    '                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
    '                'Dim paramlist As New Generic.List(Of ReportParameter)
    '                For I = 0 To localreport.GetParameters.Count - 1
    '                    GetParameter = localreport.GetParameters.Item(I).Name
    '                    Select Case LCase(GetParameter)
    '                        Case "prmrequestedby"
    '                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

    '                        Case "prmtitle"
    '                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strTitle)

    '                        Case Else
    '                    End Select
    '                Next
    '                localreport.SetParameters(par)
    '                localreport.Refresh()

    '                Dim deviceInfo As String = _
    '                    "<DeviceInfo>" + _
    '                        "  <OutputFormat>EMF</OutputFormat>" + _
    '                        "</DeviceInfo>"

    '                Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
    '                strFileName = "POSummaryReport.pdf"
    '                Dim fs As New FileStream(appPath & "Report\" & strFileName, FileMode.Create)
    '                fs.Write(PDF, 0, PDF.Length)
    '                fs.Close()

    '                Response.ContentType = "application/x-download"
    '                Response.AddHeader("Content-Disposition", "attachment;filename=" & strFileName)
    '                Response.WriteFile(Server.MapPath(strFileName))
    '                Response.End()
    '        End Select


    '    Catch ex As Exception
    '    Finally
    '        cmd = Nothing
    '        If Not IsNothing(rdr) Then
    '            rdr.Close()
    '        End If
    '        If Not IsNothing(conn) Then
    '            If conn.State = ConnectionState.Open Then
    '                conn.Close()
    '            End If
    '        End If
    '        conn = Nothing
    '    End Try
    'End Sub

    Private Sub ExportToPDF()
        'Dim ds As New DataSet
        'Dim conn As MySqlConnection = Nothing
        'Dim cmd As MySqlCommand = Nothing
        'Dim da As MySqlDataAdapter = Nothing
        'Dim rdr As MySqlDataReader = Nothing
        'Dim myConnectionString As String
        'Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        'Dim objFile As New FileManagement
        'Dim strImgSrc As String
        'Dim strUserId As String
        'Dim strCoyName As String
        'Dim dtFrom As Date
        'Dim dtTo As Date
        'Dim dtDate As Date
        'Dim strBeginDate As String
        'Dim strEndDate As String
        'Dim strTitle As String
        'Dim strFileName As String = ""

        'strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))
        ''strImgSrc = objFile.getCoyLogo(EnumUploadFrom.BackOff, Session("CompanyIdToken"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))
        ''strImgSrc = objFile.getReportCoLogo(EnumUploadFrom.BackOff, Session("CompanyIdToken"), "", System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        'Try

        '    myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

        '    conn = New MySqlConnection(myConnectionString)
        '    conn.Open()

        '    cmd = New MySqlCommand
        '    With cmd
        '        .Connection = conn
        '        .CommandType = CommandType.Text
        '        If ViewState("ReportType") = "Buyer" Then
        '            .CommandText = "SELECT po_mstr.POM_PO_INDEX, po_mstr.POM_PO_NO, po_mstr.POM_B_COY_ID, po_mstr.POM_BUYER_ID, po_mstr.POM_BUYER_NAME, " _
        '                    & "po_mstr.POM_DEPT_INDEX, company_dept_mstr.CDM_DEPT_NAME, po_mstr.POM_S_COY_ID, po_mstr.POM_S_COY_NAME, po_mstr.POM_S_ATTN,  " _
        '                    & "po_mstr.POM_S_REMARK, po_mstr.POM_PO_DATE, po_mstr.POM_FREIGHT_TERMS, po_mstr.POM_PAYMENT_TERM, " _
        '                    & "po_mstr.POM_PAYMENT_METHOD, po_mstr.POM_SHIPMENT_MODE, po_mstr.POM_SHIPMENT_TERM, po_mstr.POM_CURRENCY_CODE,  " _
        '                    & "po_mstr.POM_EXCHANGE_RATE, po_mstr.POM_PAYMENT_TERM_CODE, po_mstr.POM_SHIP_VIA, po_mstr.POM_PO_STATUS,STATUS_DESC,  " _
        '                    & "po_mstr.POM_STATUS_CHANGED_BY, po_mstr.POM_STATUS_CHANGED_ON, po_mstr.POM_EXTERNAL_REMARK, po_mstr.POM_CREATED_BY,  " _
        '                    & "po_mstr.POM_PO_COST, po_mstr.POM_BILLING_METHOD, po_mstr.POM_PO_PREFIX, po_mstr.POM_DEPT_INDEX AS Expr1,  " _
        '                    & "po_mstr.POM_ACCEPTED_DATE, po_mstr.POM_DOWNLOADED_DATE, po_mstr.POM_ARCHIVE_IND, po_mstr.POM_TERMANDCOND,  " _
        '                    & "po_mstr.POM_REFERENCE_NO, po_mstr.POM_EXTERNAL_IND, po_mstr.POM_PRINT_REMARK, po_mstr.POM_PRINT_CUSTOM_FIELDS,  " _
        '                    & "company_mstr.CM_COY_ID, company_mstr.CM_COY_NAME,  " _
        '                    & "company_mstr.CM_COY_TYPE, company_mstr.CM_PARENT_COY_ID, company_mstr.CM_ACCT_NO, company_mstr.CM_BANK,  " _
        '                    & "company_mstr.CM_BRANCH, company_mstr.CM_ADDR_LINE1, company_mstr.CM_ADDR_LINE2, company_mstr.CM_ADDR_LINE3,  " _
        '                    & "company_mstr.CM_POSTCODE, company_mstr.CM_CITY, company_mstr.CM_STATE, company_mstr.CM_COUNTRY, company_mstr.CM_PHONE,  " _
        '                    & "company_mstr.CM_FAX, company_mstr.CM_EMAIL, company_mstr.CM_COY_LOGO, company_mstr.CM_BUSINESS_REG_NO,  " _
        '                    & "company_mstr.CM_TAX_REG_NO, company_mstr.CM_PAYMENT_TERM, company_mstr.CM_PAYMENT_METHOD,  " _
        '                    & "company_mstr.CM_ACTUAL_TERMSANDCONDFILE, company_mstr.CM_HUB_TERMSANDCONDFILE, company_mstr.CM_PWD_DURATION,  " _
        '                    & "company_mstr.CM_TAX_CALC_BY, company_mstr.CM_CURRENCY_CODE, company_mstr.CM_BCM_SET, company_mstr.CM_BUDGET_FROM_DATE,  " _
        '                    & "company_mstr.CM_BUDGET_TO_DATE, company_mstr.CM_RFQ_OPTION, company_mstr.CM_LICENCE_PACKAGE,  " _
        '                    & "company_mstr.CM_LICENSE_USERS, company_mstr.CM_SUB_START_DT, company_mstr.CM_SUB_END_DT,  " _
        '                    & "company_mstr.CM_LICENSE_PRODUCTS, company_mstr.CM_FINDEPT_MODE, company_mstr.CM_PRIV_LABELING, company_mstr.CM_SKINS_ID,  " _
        '                    & "company_mstr.CM_TRAINING, company_mstr.CM_STATUS, company_mstr.CM_DELETED, company_mstr.CM_MOD_BY, company_mstr.CM_MOD_DT,  " _
        '                    & "company_mstr.CM_ENT_BY, company_mstr.CM_ENT_DT, company_mstr.CM_SKU, company_mstr.CM_TRANS_NO, company_mstr.CM_CONTACT,  " _
        '                    & "company_mstr.CM_REPORT_USERS, company_mstr.CM_INV_APPR, company_mstr.CM_MULTI_PO, company_mstr.CM_BA_CANCEL,  " _
        '                    & "user_mstr.UM_AUTO_NO, user_mstr.UM_USER_ID, user_mstr.UM_DELETED, user_mstr.UM_PASSWORD, user_mstr.UM_USER_NAME,  " _
        '                    & "user_mstr.UM_COY_ID, user_mstr.UM_DEPT_ID, user_mstr.UM_EMAIL, user_mstr.UM_APP_LIMIT, user_mstr.UM_DESIGNATION,  " _
        '                    & "user_mstr.UM_TEL_NO, user_mstr.UM_FAX_NO, user_mstr.UM_USER_SUSPEND_IND, user_mstr.UM_PASSWORD_LAST_CHANGED,  " _
        '                    & "user_mstr.UM_NEW_PASSWORD_IND, user_mstr.UM_NEXT_EXPIRE_DT, user_mstr.UM_LAST_LOGIN_DT, user_mstr.UM_QUESTION,  " _
        '                    & "user_mstr.UM_ANSWER, user_mstr.UM_MASS_APP, user_mstr.UM_STATUS, user_mstr.UM_MOD_BY, user_mstr.UM_MOD_DT,  " _
        '                    & "user_mstr.UM_ENT_BY, user_mstr.UM_ENT_DATE, user_mstr.UM_RECORD_COUNT, user_mstr.UM_EMAIL_CC, user_mstr.UM_INVOICE_APP_LIMIT,  " _
        '                    & "user_mstr.UM_INVOICE_MASS_APP, po_mstr.POM_SHIP_AMT " _
        '                    & "FROM po_mstr INNER JOIN " _
        '                    & "company_mstr ON po_mstr.POM_B_COY_ID = company_mstr.CM_COY_ID INNER JOIN " _
        '                    & "user_mstr ON po_mstr.POM_BUYER_ID = user_mstr.UM_USER_ID AND po_mstr.POM_B_COY_ID = user_mstr.UM_COY_ID " _
        '                    & "LEFT OUTER JOIN company_dept_mstr ON company_dept_mstr.CDM_DEPT_INDEX = po_mstr.POM_DEPT_INDEX " _
        '                    & "INNER JOIN status_mstr ON status_mstr.STATUS_NO=PO_MSTR.POM_PO_STATUS AND STATUS_TYPE='PO' " _
        '                    & "WHERE (po_mstr.POM_B_COY_ID = @prmCoyID) " _
        '                    & "AND POM_PO_DATE >=@prmStartDate AND POM_PO_DATE <=@prmEndDate ORDER BY POM_PO_DATE desc"
        '        Else
        '            .CommandText = "SELECT po_mstr.POM_PO_INDEX, po_mstr.POM_PO_NO, po_mstr.POM_B_COY_ID, po_mstr.POM_BUYER_ID, po_mstr.POM_BUYER_NAME, " _
        '                & "po_mstr.POM_DEPT_INDEX, company_dept_mstr.CDM_DEPT_NAME, po_mstr.POM_S_COY_ID, po_mstr.POM_S_COY_NAME, po_mstr.POM_S_ATTN,  " _
        '                & "po_mstr.POM_S_REMARK, po_mstr.POM_PO_DATE, po_mstr.POM_FREIGHT_TERMS, po_mstr.POM_PAYMENT_TERM, " _
        '                & "po_mstr.POM_PAYMENT_METHOD, po_mstr.POM_SHIPMENT_MODE, po_mstr.POM_SHIPMENT_TERM, po_mstr.POM_CURRENCY_CODE,  " _
        '                & "po_mstr.POM_EXCHANGE_RATE, po_mstr.POM_PAYMENT_TERM_CODE, po_mstr.POM_SHIP_VIA, po_mstr.POM_PO_STATUS,STATUS_DESC,  " _
        '                & "po_mstr.POM_STATUS_CHANGED_BY, po_mstr.POM_STATUS_CHANGED_ON, po_mstr.POM_EXTERNAL_REMARK, po_mstr.POM_CREATED_BY,  " _
        '                & "po_mstr.POM_PO_COST, po_mstr.POM_BILLING_METHOD, po_mstr.POM_PO_PREFIX, po_mstr.POM_DEPT_INDEX AS Expr1,  " _
        '                & "po_mstr.POM_ACCEPTED_DATE, po_mstr.POM_DOWNLOADED_DATE, po_mstr.POM_ARCHIVE_IND, po_mstr.POM_TERMANDCOND,  " _
        '                & "po_mstr.POM_REFERENCE_NO, po_mstr.POM_EXTERNAL_IND, po_mstr.POM_PRINT_REMARK, po_mstr.POM_PRINT_CUSTOM_FIELDS,  " _
        '                & "company_mstr.CM_COY_ID, company_mstr.CM_COY_NAME,  " _
        '                & "company_mstr.CM_COY_TYPE, company_mstr.CM_PARENT_COY_ID, company_mstr.CM_ACCT_NO, company_mstr.CM_BANK,  " _
        '                & "company_mstr.CM_BRANCH, company_mstr.CM_ADDR_LINE1, company_mstr.CM_ADDR_LINE2, company_mstr.CM_ADDR_LINE3,  " _
        '                & "company_mstr.CM_POSTCODE, company_mstr.CM_CITY, company_mstr.CM_STATE, company_mstr.CM_COUNTRY, company_mstr.CM_PHONE,  " _
        '                & "company_mstr.CM_FAX, company_mstr.CM_EMAIL, company_mstr.CM_COY_LOGO, company_mstr.CM_BUSINESS_REG_NO,  " _
        '                & "company_mstr.CM_TAX_REG_NO, company_mstr.CM_PAYMENT_TERM, company_mstr.CM_PAYMENT_METHOD,  " _
        '                & "company_mstr.CM_ACTUAL_TERMSANDCONDFILE, company_mstr.CM_HUB_TERMSANDCONDFILE, company_mstr.CM_PWD_DURATION,  " _
        '                & "company_mstr.CM_TAX_CALC_BY, company_mstr.CM_CURRENCY_CODE, company_mstr.CM_BCM_SET, company_mstr.CM_BUDGET_FROM_DATE,  " _
        '                & "company_mstr.CM_BUDGET_TO_DATE, company_mstr.CM_RFQ_OPTION, company_mstr.CM_LICENCE_PACKAGE,  " _
        '                & "company_mstr.CM_LICENSE_USERS, company_mstr.CM_SUB_START_DT, company_mstr.CM_SUB_END_DT,  " _
        '                & "company_mstr.CM_LICENSE_PRODUCTS, company_mstr.CM_FINDEPT_MODE, company_mstr.CM_PRIV_LABELING, company_mstr.CM_SKINS_ID,  " _
        '                & "company_mstr.CM_TRAINING, company_mstr.CM_STATUS, company_mstr.CM_DELETED, company_mstr.CM_MOD_BY, company_mstr.CM_MOD_DT,  " _
        '                & "company_mstr.CM_ENT_BY, company_mstr.CM_ENT_DT, company_mstr.CM_SKU, company_mstr.CM_TRANS_NO, company_mstr.CM_CONTACT,  " _
        '                & "company_mstr.CM_REPORT_USERS, company_mstr.CM_INV_APPR, company_mstr.CM_MULTI_PO, company_mstr.CM_BA_CANCEL,  " _
        '                & "user_mstr.UM_AUTO_NO, user_mstr.UM_USER_ID, user_mstr.UM_DELETED, user_mstr.UM_PASSWORD, user_mstr.UM_USER_NAME,  " _
        '                & "user_mstr.UM_COY_ID, user_mstr.UM_DEPT_ID, user_mstr.UM_EMAIL, user_mstr.UM_APP_LIMIT, user_mstr.UM_DESIGNATION,  " _
        '                & "user_mstr.UM_TEL_NO, user_mstr.UM_FAX_NO, user_mstr.UM_USER_SUSPEND_IND, user_mstr.UM_PASSWORD_LAST_CHANGED,  " _
        '                & "user_mstr.UM_NEW_PASSWORD_IND, user_mstr.UM_NEXT_EXPIRE_DT, user_mstr.UM_LAST_LOGIN_DT, user_mstr.UM_QUESTION,  " _
        '                & "user_mstr.UM_ANSWER, user_mstr.UM_MASS_APP, user_mstr.UM_STATUS, user_mstr.UM_MOD_BY, user_mstr.UM_MOD_DT,  " _
        '                & "user_mstr.UM_ENT_BY, user_mstr.UM_ENT_DATE, user_mstr.UM_RECORD_COUNT, user_mstr.UM_EMAIL_CC, user_mstr.UM_INVOICE_APP_LIMIT,  " _
        '                & "user_mstr.UM_INVOICE_MASS_APP, po_mstr.POM_SHIP_AMT " _
        '                & "FROM po_mstr INNER JOIN " _
        '                & "company_mstr ON po_mstr.POM_B_COY_ID = company_mstr.CM_COY_ID INNER JOIN " _
        '                & "user_mstr ON po_mstr.POM_BUYER_ID = user_mstr.UM_USER_ID AND po_mstr.POM_B_COY_ID = user_mstr.UM_COY_ID " _
        '                & "LEFT OUTER JOIN company_dept_mstr ON company_dept_mstr.CDM_DEPT_INDEX = po_mstr.POM_DEPT_INDEX " _
        '                & "INNER JOIN status_mstr ON status_mstr.STATUS_NO=PO_MSTR.POM_PO_STATUS AND STATUS_TYPE='PO' AND STATUS_NO IN (1,2,3,6,9) " _
        '                & "WHERE (po_mstr.POM_S_COY_ID = @prmCoyID) " _
        '                & "AND POM_PO_DATE >=@prmStartDate AND POM_PO_DATE <=@prmEndDate ORDER BY POM_PO_DATE desc"
        '        End If
        '    End With

        '    da = New MySqlDataAdapter(cmd)

        '    If Me.cboMonth.SelectedIndex > 0 Then
        '        dtDate = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue, 1)
        '        dtFrom = dtDate

        '        If Me.cboMonth.SelectedValue < 12 Then
        '            dtDate = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue + 1, 1)
        '            dtTo = DateAdd(DateInterval.Day, -1, dtDate)
        '            dtTo = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue, dtTo.Day)

        '        Else    'December
        '            dtTo = New DateTime(Me.cboYear.SelectedValue, Me.cboMonth.SelectedValue, 31)
        '        End If
        '    Else
        '    End If

        '    strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
        '    strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
        '    strTitle = Format(dtFrom, "MMM yyyy")
        '    da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyID")))
        '    da.SelectCommand.Parameters.Add(New MySqlParameter("@prmStartDate", strBeginDate))
        '    da.SelectCommand.Parameters.Add(New MySqlParameter("@prmEndDate", strEndDate))
        '    strUserId = Session("UserName") 'Session("UserId")
        '    strCoyName = Session("CompanyName")

        '    da.Fill(ds)
        '    Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("POSummary_DataSetPOSummary", ds.Tables(0))
        '    Dim localreport As New LocalReport
        '    localreport.DataSources.Clear()
        '    localreport.DataSources.Add(rptDataSource)
        '    'localreport.ReportPath = appPath & "Common\Report\POSummary_pdf.rdlc"
        '    If ViewState("ReportType") = "Buyer" Then
        '        localreport.ReportPath = dispatcher.direct("Report", "POSummary_pdf.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")
        '    Else
        '        localreport.ReportPath = dispatcher.direct("Report", "POSummaryVendor_pdf.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")
        '    End If
        '    localreport.EnableExternalImages = True

        '    Dim I As Byte
        '    Dim GetParameter As String = ""
        '    Dim TotalParameter As Byte
        '    Dim strID As String = ""

        '    strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
        '    TotalParameter = localreport.GetParameters.Count
        '    Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
        '    For I = 0 To localreport.GetParameters.Count - 1
        '        GetParameter = localreport.GetParameters.Item(I).Name
        '        Select Case LCase(GetParameter)
        '            Case "prmrequestedby"
        '                par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

        '            Case "prmtitle"
        '                par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strTitle)

        '            Case "logo"
        '                par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

        '            Case "prmbuyercoyname"
        '                par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)

        '            Case Else
        '        End Select
        '    Next
        '    localreport.SetParameters(par)
        '    localreport.Refresh()

        '    Dim deviceInfo As String = _
        '        "<DeviceInfo>" + _
        '            "  <OutputFormat>EMF</OutputFormat>" + _
        '            "</DeviceInfo>"
        '    System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds

        '    Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
        '    System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds

        '    strFileName = "POSummaryReport.pdf"

        '    'Return PDF
        '    Me.Response.Clear()
        '    Me.Response.ContentType = "application/pdf"
        '    Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
        '    Me.Response.BinaryWrite(PDF)
        '    Me.Response.End()

        '    'Dim fs As New FileStream(appPath & "Report\" & strFileName, FileMode.Create)
        '    '
        '    ''Dim fs As New FileStream(Server.MapPath(strFileName), FileMode.Create)
        '    'Dim fs As New FileStream(dispatcher.direct("Report", strFileName), FileMode.Create)
        '    'fs.Write(PDF, 0, PDF.Length)
        '    'fs.Close()

        '    'Response.ContentType = "application/x-download"
        '    'Response.AddHeader("Content-Disposition", "attachment;filename=" & strFileName)
        '    'Response.WriteFile(Server.MapPath(strFileName))
        '    'Response.End()

        'Catch ex As Exception
        'Finally
        '    cmd = Nothing
        '    If Not IsNothing(rdr) Then
        '        rdr.Close()
        '    End If
        '    If Not IsNothing(conn) Then
        '        If conn.State = ConnectionState.Open Then
        '            conn.Close()
        '        End If
        '    End If
        '    conn = Nothing
        'End Try
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
        Dim i, iRow As Integer
        Dim strBeginDate As String
        Dim strEndDate As String
        Dim strTitle As String
        Dim strSql, strSql2, strSql3 As String
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

            'Modified for IPP GST Stage 2A - YAP - 13 Feb 2015
            Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                If ViewState("type") = "RELATEDPARTIES" Then
                    'Modified for IPP GST Stage 2A - YAP - 13 Feb 2015
                    .CommandText = "SELECT 'RPT' AS 'Related Parties', CODE_DESC AS 'Holding/Subsidiaries/Related Companies/Non-Related Companies', " & _
                                "IM_S_COY_NAME AS 'Vendor', ID_B_GL_CODE AS 'GL Code', CBG_B_GL_DESC AS 'GL Name', " & _
                                "(ID_RECEIVED_QTY * ID_UNIT_COST) AS 'Payment Amount', DATE_FORMAT(IM_PAYMENT_DATE, '%d-%b-%Y') AS 'Payment Date', " & _
                                "ID_PRODUCT_DESC AS 'Transaction Description', ID_PAY_FOR AS 'Entity', ID_BRANCH_CODE AS 'HD/BR', ID_COST_CENTER AS 'Cost Center', " & _
                                "IM_CURRENCY_CODE AS 'Currency', 'Invoice' AS 'Document Type', IM_INVOICE_NO AS 'Document Number', " & _
                                "DATE_FORMAT(IM_DOC_DATE, '%d-%b-%Y') AS 'Document Date' " & _
                                "FROM INVOICE_MSTR " & _
                                "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'IPPCC' AND CODE_ABBR = IM_COMPANY_CATEGORY " & _
                                "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " & _
                                "INNER JOIN COMPANY_B_GL_CODE ON CBG_B_COY_ID = '" & strDefIPPCompID & "' AND CBG_B_GL_CODE = ID_B_GL_CODE " & _
                                "WHERE IM_B_COY_ID = '" & Session("CompanyID") & "' " & _
                                "AND IM_PO_INDEX IS NULL AND IM_INVOICE_STATUS = '4' " & _
                                "AND IM_PAYMENT_DATE >='" & strBeginDate & "' AND IM_PAYMENT_DATE <='" & strEndDate & "' "

                    If cboEntity.SelectedItem.Value <> "" Then
                        .CommandText &= "AND IM_COMPANY_CATEGORY = '" & cboEntity.SelectedItem.Value & "' "
                    End If

                    .CommandText &= "ORDER BY IM_S_COY_NAME, ID_B_GL_CODE"
                    '.CommandText &= "ORDER BY IM_PAYMENT_DATE, IM_INVOICE_NO"

                ElseIf ViewState("type") = "GLANALYSIS" Then
                    'Modified for IPP GST Stage 2A - YAP - 13 Feb 2015
                    strSql = "SELECT IG_GLRULE_CODE, IGC_GLRULE_CATEGORY_INDEX, CAST(CONCAT('CAT_', IGC_GLRULE_CATEGORY_INDEX) AS CHAR(50)) AS CAT, IGC_GLRULE_CATEGORY " & _
                            "FROM IPP_GLRULE_CATEGORY " & _
                            "INNER JOIN IPP_GLRULE ON IG_GLRULE_INDEX = IGC_GLRULE_INDEX " & _
                            "WHERE IGC_GLRULE_INDEX = (SELECT IG_GLRULE_INDEX FROM IPP_GLRULE_GL " & _
                            "INNER JOIN IPP_GLRULE ON IG_GLRULE_INDEX = IGG_GLRULE_INDEX " & _
                            "WHERE IG_COY_ID = '" & strDefIPPCompID & "' AND IGG_GL_CODE = '" & cboGLCode.SelectedItem.Value & "')"

                    dsSql = objDb.FillDs(strSql)
                    iRow = dsSql.Tables(0).Rows.Count

                    If dsSql.Tables(0).Rows.Count > 0 Then
                        strSql = ""
                        strSql2 = ""
                        strSql3 = ""

                        For i = 0 To dsSql.Tables(0).Rows.Count - 1
                            strSql &= ", CASE WHEN ID_GLRULE_CATEGORY_INDEX = " & dsSql.Tables(0).Rows(i)("IGC_GLRULE_CATEGORY_INDEX") & " " & _
                                    "THEN (ID_RECEIVED_QTY * ID_UNIT_COST) ELSE NULL END AS '" & dsSql.Tables(0).Rows(i)("CAT") & "'"

                            strSql2 &= ", SUM(" & dsSql.Tables(0).Rows(i)("CAT") & ") AS '" & dsSql.Tables(0).Rows(i)("IGC_GLRULE_CATEGORY").ToString.Replace("'", "''").Replace(vbCrLf, "") & "'"

                            If strSql3 = "" Then
                                strSql3 &= "IFNULL(" & dsSql.Tables(0).Rows(i)("CAT") & ",0)"
                            Else
                                strSql3 &= " + IFNULL(" & dsSql.Tables(0).Rows(i)("CAT") & ",0)"
                            End If
                        Next

                        strSql3 = ", SUM((" & strSql3 & ")) AS 'Total Amount (RM)' "

                        .CommandText = "SELECT DATE_FORMAT(IM_PAYMENT_DATE, '%d-%b-%Y') AS 'Date Of Payment', '" & cboGLCode.SelectedItem.Value & "' AS 'GL Code', CBG_B_GL_DESC AS 'Description', " & _
                                "ID_BRANCH_CODE_NAME AS 'Transaction Branch', SUM(AMT) AS 'Amount (RM)' "

                        If strSql2 <> "" Then
                            .CommandText &= strSql2
                        End If

                        If strSql3 <> "" Then
                            .CommandText &= strSql3
                        End If

                        .CommandText &= "FROM " & _
                                    "(SELECT IM_PAYMENT_DATE,  CBG_B_GL_DESC, ID_A.ID_BRANCH_CODE_NAME, " & _
                                    "(ID_RECEIVED_QTY * ID_UNIT_COST) AS AMT "

                        If strSql <> "" Then
                            .CommandText &= strSql
                        End If

                        'Modified for IPP GST Stage 2A - YAP - 13 Feb 2015
                        .CommandText &= "FROM INVOICE_MSTR IM_A " & _
                                    "INNER JOIN INVOICE_DETAILS ID_A ON IM_A.IM_INVOICE_NO = ID_A.ID_INVOICE_NO AND IM_A.IM_S_COY_ID = ID_A.ID_S_COY_ID " & _
                                    "INNER JOIN COMPANY_B_GL_CODE ON CBG_B_COY_ID = '" & strDefIPPCompID & "' AND CBG_B_GL_CODE = ID_A.ID_B_GL_CODE " & _
                                    "WHERE IM_B_COY_ID = '" & Session("CompanyID") & "' AND IM_PAYMENT_DATE IS NOT NULL AND ID_B_GL_CODE = '" & cboGLCode.SelectedItem.Value & "' " & _
                                    "AND IM_INVOICE_STATUS = '4' AND IM_PO_INDEX IS NULL " & _
                                    "AND IM_PAYMENT_DATE >='" & strBeginDate & "' AND IM_PAYMENT_DATE <='" & strEndDate & "') tb " & _
                                    "GROUP BY DATE_FORMAT(IM_PAYMENT_DATE, '%d-%m-%Y'), CBG_B_GL_DESC, ID_BRANCH_CODE_NAME " & _
                                    "ORDER BY IM_PAYMENT_DATE, CBG_B_GL_DESC, ID_BRANCH_CODE_NAME "

                    Else
                        .CommandText = "SELECT '' AS 'Date Of Payment', '' AS 'GL Code', '' AS 'Description', '' AS 'Transaction Branch', '' AS 'Amount (RM)', '' AS 'Total Amount (RM)'"
                    End If
                End If
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
            If ViewState("type") = "RELATEDPARTIES" Then
                strFileName = "IPPA0020 - Related Parties Transaction Report"
            ElseIf ViewState("type") = "GLANALYSIS" Then
                strFileName = "IPPA0018 - GL Analysis Report"
            End If
            strFileName = strFileName & "(" & Format(dtFrom, "MMMyyyy") & ").xls"
            Dim attachment As String = "attachment;filename=" & strFileName
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"

            Dim dc As DataColumn

            ''Zulham 28/03/2016
            ''Commented
            If ViewState("type") = "GLANALYSIS" And iRow > 0 Then
                If (iRow Mod 2) = 0 Then
                    iRow = iRow / 2
                Else
                    If iRow > 1 Then
                        iRow = (iRow + 1) / 2
                    Else
                        iRow = 1
                    End If
                End If

                iRow = iRow + 4
                For i = 0 To iRow - 1
                    Response.Write(vbTab)
                Next
                Response.Write(dsSql.Tables(0).Rows(0)("IG_GLRULE_CODE"))
                Response.Write(vbCrLf)
            End If

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