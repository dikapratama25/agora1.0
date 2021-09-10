Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class ReportFormatN3
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strCoyType As String
        lblHeader.Text = Request.QueryString("rptname")
        strCoyType = lblHeader.Text.Substring(1, 1)
        If strCoyType = "V" Then
            ViewState("ReportType") = "Vendor"
        Else
            ViewState("ReportType") = "Buyer"
        End If
		
        'Modified for Agora GST Stage 2 - CH - 3/3/2015
        ViewState("name") = Request.QueryString("name")
		'--------------------------------------------

        If Not (Page.IsPostBack) Then

            Dim ii_ddl, ii_ddl2, jj_ddl As Integer
            ii_ddl2 = 0
            'jj_ddl = 2021
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
        'Dim objFile As New FileManagement
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
        Dim objFile As New FileManagement

        'strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyIdToken"), System.AppDomain.CurrentDomain.BaseDirectory & "Common\Plugins\images\", System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))
        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                'Modified for Agora GST Stage 2 - CH - 2/2/2015
                If ViewState("name") = "DNSumm" Then
                    'Issue 7480 - CH - 25 Mar 2015 (No.65): Remove Exchange Rate
                    .CommandText = "SELECT DNM_DN_NO, DNM_CREATED_DATE, " &
                                "CMA.CM_COY_NAME AS COY_S_NAME, CMB.CM_COY_NAME AS COY_B_NAME, STATUS_DESC, DNM_INV_NO, DNM_CURRENCY_CODE, " &
                                "SUM((DND_UNIT_COST * DND_QTY) + " &
                                "((DND_UNIT_COST * DND_QTY) * IF(IFNULL(TAX_PERC, '') = 'N/A' OR IFNULL(TAX_PERC, '') = '', 0, TAX_PERC) / 100)) AS AMT " &
                                "FROM DEBIT_NOTE_DETAILS " &
                                "INNER JOIN DEBIT_NOTE_MSTR ON DND_DN_S_COY_ID = DNM_DN_S_COY_ID AND DND_DN_NO = DNM_DN_NO " &
                                "INNER JOIN INVOICE_MSTR ON DNM_DN_S_COY_ID = IM_S_COY_ID AND DNM_INV_NO = IM_INVOICE_NO " &
                                "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID AND DND_INV_LINE = ID_INVOICE_LINE " &
                                "INNER JOIN STATUS_MSTR ON STATUS_TYPE = 'DN' AND DNM_DN_STATUS = STATUS_NO " &
                                "INNER JOIN COMPANY_MSTR CMA ON DNM_DN_S_COY_ID = CMA.CM_COY_ID " &
                                "INNER JOIN COMPANY_MSTR CMB ON DNM_DN_B_COY_ID = CMB.CM_COY_ID " &
                                "LEFT JOIN TAX ON TAX_COUNTRY_CODE = CMA.CM_COUNTRY AND TAX_CODE = DND_GST_RATE " &
                                "WHERE DNM_DN_B_COY_ID = @prmCoyID " &
                                "AND DNM_CREATED_DATE >= @prmStartDate AND DNM_CREATED_DATE <=@prmEndDate " &
                                "GROUP BY DNM_DN_NO, DNM_DN_S_COY_ID " &
                                "ORDER BY DNM_CREATED_DATE DESC "

                ElseIf ViewState("name") = "DNSummV" Then
                    'Issue 7480 - CH - 25 Mar 2015 (No.65): Remove Exchange Rate
                    .CommandText = "SELECT DNM_DN_NO, DNM_CREATED_DATE, " &
                                "CMA.CM_COY_NAME AS COY_S_NAME, CMB.CM_COY_NAME AS COY_B_NAME, STATUS_DESC, DNM_INV_NO, DNM_CURRENCY_CODE, " &
                                "SUM((DND_UNIT_COST * DND_QTY) + " &
                                "((DND_UNIT_COST * DND_QTY) * IF(IFNULL(TAX_PERC, '') = 'N/A' OR IFNULL(TAX_PERC, '') = '', 0, TAX_PERC) / 100)) AS AMT " &
                                "FROM DEBIT_NOTE_DETAILS " &
                                "INNER JOIN DEBIT_NOTE_MSTR ON DND_DN_S_COY_ID = DNM_DN_S_COY_ID AND DND_DN_NO = DNM_DN_NO " &
                                "INNER JOIN INVOICE_MSTR ON DNM_DN_S_COY_ID = IM_S_COY_ID AND DNM_INV_NO = IM_INVOICE_NO " &
                                "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID AND DND_INV_LINE = ID_INVOICE_LINE " &
                                "INNER JOIN STATUS_MSTR ON STATUS_TYPE = 'DN' AND DNM_DN_STATUS = STATUS_NO " &
                                "INNER JOIN COMPANY_MSTR CMA ON DNM_DN_S_COY_ID = CMA.CM_COY_ID " &
                                "INNER JOIN COMPANY_MSTR CMB ON DNM_DN_B_COY_ID = CMB.CM_COY_ID " &
                                "LEFT JOIN TAX ON TAX_COUNTRY_CODE = CMA.CM_COUNTRY AND TAX_CODE = DND_GST_RATE " &
                                "WHERE DNM_DN_S_COY_ID = @prmCoyID " &
                                "AND DNM_CREATED_DATE >= @prmStartDate AND DNM_CREATED_DATE <=@prmEndDate " &
                                "GROUP BY DNM_DN_NO, DNM_DN_S_COY_ID " &
                                "ORDER BY DNM_CREATED_DATE DESC "

                ElseIf ViewState("name") = "CNSumm" Then
                    'Issue 7480 - CH - 25 Mar 2015 (No.65): Remove Exchange Rate
                    .CommandText = "SELECT CNM_CN_NO, CNM_CREATED_DATE, " &
                                "CMA.CM_COY_NAME AS COY_S_NAME, CMB.CM_COY_NAME AS COY_B_NAME, STATUS_DESC, CNM_INV_NO, CNM_CURRENCY_CODE, " &
                                "SUM((CND_UNIT_COST * CND_QTY) + " &
                                "((CND_UNIT_COST * CND_QTY) * IF(IFNULL(TAX_PERC, '') = 'N/A' OR IFNULL(TAX_PERC, '') = '', 0, TAX_PERC) / 100)) AS AMT " &
                                "FROM CREDIT_NOTE_DETAILS " &
                                "INNER JOIN CREDIT_NOTE_MSTR ON CND_CN_S_COY_ID = CNM_CN_S_COY_ID AND CND_CN_NO = CNM_CN_NO " &
                                "INNER JOIN INVOICE_MSTR ON CNM_CN_S_COY_ID = IM_S_COY_ID AND CNM_INV_NO = IM_INVOICE_NO " &
                                "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID AND CND_INV_LINE = ID_INVOICE_LINE " &
                                "INNER JOIN STATUS_MSTR ON STATUS_TYPE = 'CN' AND CNM_CN_STATUS = STATUS_NO " &
                                "INNER JOIN COMPANY_MSTR CMA ON CNM_CN_S_COY_ID = CMA.CM_COY_ID " &
                                "INNER JOIN COMPANY_MSTR CMB ON CNM_CN_B_COY_ID = CMB.CM_COY_ID " &
                                "LEFT JOIN TAX ON TAX_COUNTRY_CODE = CMA.CM_COUNTRY AND TAX_CODE = CND_GST_RATE " &
                                "WHERE CNM_CN_B_COY_ID = @prmCoyID " &
                                "AND CNM_CREATED_DATE >= @prmStartDate AND CNM_CREATED_DATE <=@prmEndDate " &
                                "GROUP BY CNM_CN_NO, CNM_CN_S_COY_ID " &
                                "ORDER BY CNM_CREATED_DATE DESC "

                ElseIf ViewState("name") = "CNSummV" Then
                    'Issue 7480 - CH - 25 Mar 2015 (No.65): Remove Exchange Rate
                    .CommandText = "SELECT CNM_CN_NO, CNM_CREATED_DATE, " &
                                "CMA.CM_COY_NAME AS COY_S_NAME, CMB.CM_COY_NAME AS COY_B_NAME, STATUS_DESC, CNM_INV_NO, CNM_CURRENCY_CODE, " &
                                "SUM((CND_UNIT_COST * CND_QTY) + " &
                                "((CND_UNIT_COST * CND_QTY) * IF(IFNULL(TAX_PERC, '') = 'N/A' OR IFNULL(TAX_PERC, '') = '', 0, TAX_PERC) / 100)) AS AMT " &
                                "FROM CREDIT_NOTE_DETAILS " &
                                "INNER JOIN CREDIT_NOTE_MSTR ON CND_CN_S_COY_ID = CNM_CN_S_COY_ID AND CND_CN_NO = CNM_CN_NO " &
                                "INNER JOIN INVOICE_MSTR ON CNM_CN_S_COY_ID = IM_S_COY_ID AND CNM_INV_NO = IM_INVOICE_NO " &
                                "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID AND CND_INV_LINE = ID_INVOICE_LINE " &
                                "INNER JOIN STATUS_MSTR ON STATUS_TYPE = 'CN' AND CNM_CN_STATUS = STATUS_NO " &
                                "INNER JOIN COMPANY_MSTR CMA ON CNM_CN_S_COY_ID = CMA.CM_COY_ID " &
                                "INNER JOIN COMPANY_MSTR CMB ON CNM_CN_B_COY_ID = CMB.CM_COY_ID " &
                                "LEFT JOIN TAX ON TAX_COUNTRY_CODE = CMA.CM_COUNTRY AND TAX_CODE = CND_GST_RATE " &
                                "WHERE CNM_CN_S_COY_ID = @prmCoyID " &
                                "AND CNM_CREATED_DATE >= @prmStartDate AND CNM_CREATED_DATE <=@prmEndDate " &
                                "GROUP BY CNM_CN_NO, CNM_CN_S_COY_ID " &
                                "ORDER BY CNM_CREATED_DATE DESC "
                    '-------------------------------------------------
                ElseIf ViewState("name") = "BillSummV" Then
                    .CommandText = "SELECT * FROM " &
                                "(SELECT IM_INVOICE_INDEX, IM_INVOICE_NO, IM_S_COY_ID, IM_S_COY_NAME, " &
                                "IM_CREATED_ON, CM_COY_NAME AS BUYER_COY_NAME, POM_PO_NO, " &
                                "POM_CURRENCY_CODE, POM_CREATED_DATE, SUM(ID_UNIT_COST * ID_RECEIVED_QTY) AS AMT, SUM(ID_GST_VALUE) AS GST, " &
                                "IM_SHIP_AMT, IM_INVOICE_TOTAL, STATUS_DESC, IM_PAYMENT_DATE " &
                                "FROM INVOICE_MSTR " &
                                "INNER JOIN INVOICE_DETAILS ON ID_INVOICE_NO = IM_INVOICE_NO AND ID_S_COY_ID = IM_S_COY_ID " &
                                "INNER JOIN PO_MSTR ON POM_PO_INDEX = IM_PO_INDEX " &
                                "INNER JOIN COMPANY_MSTR ON CM_COY_ID = IM_B_COY_ID " &
                                "INNER JOIN STATUS_MSTR ON STATUS_TYPE = 'INV' AND IM_INVOICE_STATUS = STATUS_NO " &
                                "WHERE IM_S_COY_ID = @prmCoyID " &
                                "AND IM_CREATED_ON >= @prmStartDate AND IM_CREATED_ON <=@prmEndDate " &
                                "GROUP BY IM_S_COY_ID, IM_INVOICE_NO) tb_inv " &
                                "LEFT JOIN " &
                                "(SELECT 'CNDN' AS TB, DNM_INV_NO AS INV_NO, DNM_DN_S_COY_ID AS S_COY_ID, " &
                                "DNM_DN_NO AS DN_CN_NO, DNM_CREATED_DATE AS DN_CN_CREATED_DATE, " &
                                "SUM(DND_UNIT_COST * DND_QTY) AS DN_CN_AMT, " &
                                "(DNM_DN_TOTAL - SUM(DND_UNIT_COST * DND_QTY)) AS DN_CN_GST, DNM_DN_TOTAL AS DN_CN_TOTAL_AMT " &
                                "FROM INVOICE_MSTR " &
                                "INNER JOIN DEBIT_NOTE_MSTR ON DNM_DN_S_COY_ID = IM_S_COY_ID AND DNM_INV_NO = IM_INVOICE_NO " &
                                "INNER JOIN DEBIT_NOTE_DETAILS ON DND_DN_S_COY_ID = DNM_DN_S_COY_ID AND DND_DN_NO = DNM_DN_NO " &
                                "WHERE IM_S_COY_ID = @prmCoyID " &
                                "AND IM_CREATED_ON >= @prmStartDate AND IM_CREATED_ON <=@prmEndDate " &
                                "GROUP BY DNM_DN_NO " &
                                "UNION ALL " &
                                "SELECT 'CNDN' AS TB, CNM_INV_NO AS INV_NO, CNM_CN_S_COY_ID AS S_COY_ID, " &
                                "CNM_CN_NO AS DN_CN_NO, CNM_CREATED_DATE AS CREATED_DATE, " &
                                "SUM(CND_UNIT_COST * CND_QTY) AS DN_CN_AMT, " &
                                "(CNM_CN_TOTAL - SUM(CND_UNIT_COST * CND_QTY)) AS DN_CN_GST, CNM_CN_TOTAL AS DN_CN_TOTAL_AMT " &
                                "FROM INVOICE_MSTR " &
                                "INNER JOIN CREDIT_NOTE_MSTR ON CNM_CN_S_COY_ID = IM_S_COY_ID AND CNM_INV_NO = IM_INVOICE_NO " &
                                "INNER JOIN CREDIT_NOTE_DETAILS ON CND_CN_S_COY_ID = CNM_CN_S_COY_ID AND CND_CN_NO = CNM_CN_NO " &
                                "WHERE IM_S_COY_ID = @prmCoyID " &
                                "AND IM_CREATED_ON >= @prmStartDate AND IM_CREATED_ON <=@prmEndDate " &
                                "GROUP BY CNM_CN_NO) tb_dn_cn " &
                                "ON tb_inv.IM_S_COY_ID = tb_dn_cn.S_COY_ID AND tb_inv.IM_INVOICE_NO = tb_dn_cn.INV_NO "
                Else
                    If ViewState("ReportType") = "Buyer" Then
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
                            & "FORMAT(SUM(ROUND(ID_RECEIVED_QTY*ID_UNIT_COST,2)),2) AS INVOICE_AMT, " _
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
                            & "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " _
                            & "INNER JOIN PO_MSTR ON INVOICE_MSTR.IM_PO_INDEX = PO_MSTR.POM_PO_INDEX AND INVOICE_MSTR.IM_S_COY_ID = PO_MSTR.POM_S_COY_ID " _
                            & "INNER JOIN COMPANY_MSTR ON IM_B_COY_ID = COMPANY_MSTR.CM_COY_ID " _
                            & "LEFT OUTER JOIN company_dept_mstr ON company_dept_mstr.CDM_DEPT_INDEX = po_mstr.POM_DEPT_INDEX " _
                            & "INNER JOIN status_mstr ON status_mstr.STATUS_NO=INVOICE_MSTR.IM_INVOICE_STATUS AND STATUS_TYPE='INV' " _
                            & "WHERE (INVOICE_MSTR.IM_B_COY_ID = @prmCoyID) " _
                            & "AND IM_CREATED_ON>= @prmStartDate AND IM_CREATED_ON<=@prmEndDate " _
                            & "GROUP BY IM_INVOICE_INDEX " _
                            & "ORDER BY IM_CREATED_ON DESC"
                    Else
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
                            & "FORMAT(SUM(ROUND(ID_RECEIVED_QTY*ID_UNIT_COST,2)),2) AS INVOICE_AMT, " _
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
                            & "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " _
                            & "INNER JOIN PO_MSTR ON INVOICE_MSTR.IM_PO_INDEX = PO_MSTR.POM_PO_INDEX AND INVOICE_MSTR.IM_S_COY_ID = PO_MSTR.POM_S_COY_ID " _
                            & "INNER JOIN COMPANY_MSTR ON IM_B_COY_ID = COMPANY_MSTR.CM_COY_ID " _
                            & "LEFT OUTER JOIN company_dept_mstr ON company_dept_mstr.CDM_DEPT_INDEX = po_mstr.POM_DEPT_INDEX " _
                            & "INNER JOIN status_mstr ON status_mstr.STATUS_NO=INVOICE_MSTR.IM_INVOICE_STATUS AND STATUS_TYPE='INV' AND STATUS_NO IN (1,2,3,4) " _
                            & "WHERE (INVOICE_MSTR.IM_S_COY_ID = @prmCoyID) " _
                            & "AND IM_CREATED_ON>= @prmStartDate AND IM_CREATED_ON<=@prmEndDate " _
                            & "GROUP BY IM_INVOICE_INDEX " _
                            & "ORDER BY IM_CREATED_ON DESC"
                    End If
                End If
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
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmStartDate", strBeginDate))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmEndDate", strEndDate))
            strUserId = Session("UserName") ' Session("UserId")
            strCoyName = Session("CompanyName")

            da.Fill(ds)
            'Modified for Agora GST Stage 2 - CH - 2/2/2015
            Dim strDataSet As String = ""
            If ViewState("name") = "DNSumm" Or ViewState("name") = "DNSummV" Then
                strDataSet = "DNSummary_DataTable1"
            ElseIf ViewState("name") = "CNSumm" Or ViewState("name") = "CNSummV" Then
                strDataSet = "CNSummary_DataTable1"
            ElseIf ViewState("name") = "BillSumm" Or ViewState("name") = "BillSummV" Then
                strDataSet = "BillingSummary_DatasetBillingSummary"
            Else
                strDataSet = "InvSummary_xls_DataSetInvSummary"
            End If

            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource(strDataSet, ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            If ViewState("name") = "DNSumm" Then
                localreport.ReportPath = dispatcher.direct("Report", "DNSummary_pdf.rdlc", "Report")
            ElseIf ViewState("name") = "DNSummV" Then
                localreport.ReportPath = dispatcher.direct("Report", "DNSummaryVendor_pdf.rdlc", "Report")
            ElseIf ViewState("name") = "CNSumm" Then
                localreport.ReportPath = dispatcher.direct("Report", "CNSummary_pdf.rdlc", "Report")
            ElseIf ViewState("name") = "CNSummV" Then
                localreport.ReportPath = dispatcher.direct("Report", "CNSummaryVendor_pdf.rdlc", "Report")
            ElseIf ViewState("name") = "BillSummV" Then
                localreport.ReportPath = dispatcher.direct("Report", "BillingSummaryVendor_pdf.rdlc", "Report")
            Else
                If ViewState("ReportType") = "Buyer" Then
                    localreport.ReportPath = dispatcher.direct("Report", "InvSummary_pdf.rdlc", "Report") 'Server.MapPath("InvSummary_pdf.rdlc")  ''appPath & "Report\InvSummary_pdf.rdlc"
                Else
                    localreport.ReportPath = dispatcher.direct("Report", "InvSummaryVendor_pdf.rdlc", "Report") 'Server.MapPath("InvSummary_pdf.rdlc")  ''appPath & "Report\InvSummary_pdf.rdlc"
                End If
            End If
            '-------------------------------------------
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
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
                        'par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                    Case "prmbuyercoyname"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)

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
            'Modified for Agora GST Stage 2 - CH - 2/2/2015
            If ViewState("name") = "DNSumm" Or ViewState("name") = "DNSummV" Then
                strFileName = "DN_DA_SummaryReport.pdf"
            ElseIf ViewState("name") = "CNSumm" Or ViewState("name") = "CNSummV" Then
                strFileName = "CN_CA_SummaryReport.pdf"
            ElseIf ViewState("name") = "BillSumm" Or ViewState("name") = "BillSummV" Then
                strFileName = "BillingSummaryReport.pdf"
            Else
                strFileName = "InvSummaryReport.pdf"
            End If
            '--------------------------------------------

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

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
				'Modified for Agora GST Stage 2 - CH - 2/2/2015
                If ViewState("name") = "DNSumm" Then
                    'Issue 7480 - CH - 25 Mar 2015 (No.65): Remove Exchange Rate
                    .CommandText = "SELECT DNM_DN_NO AS 'Debit Note/Debit Advice Number', DATE_FORMAT(DNM_CREATED_DATE,'%d/%m/%Y') AS 'Debit Note/Debit Advice Date', " & _
                                "CMA.CM_COY_NAME AS 'Vendor Name', STATUS_DESC AS 'Debit Note/Debit Advice Status', DNM_INV_NO AS 'Invoice Number', " & _
                                "DNM_CURRENCY_CODE AS 'Currency', " & _
                                "SUM((DND_UNIT_COST * DND_QTY) + " & _
                                "((DND_UNIT_COST * DND_QTY) * IF(IFNULL(TAX_PERC, '') = 'N/A' OR IFNULL(TAX_PERC, '') = '', 0, TAX_PERC) / 100)) AS 'Amount' " & _
                                "FROM DEBIT_NOTE_DETAILS " & _
                                "INNER JOIN DEBIT_NOTE_MSTR ON DND_DN_S_COY_ID = DNM_DN_S_COY_ID AND DND_DN_NO = DNM_DN_NO " & _
                                "INNER JOIN INVOICE_MSTR ON DNM_DN_S_COY_ID = IM_S_COY_ID AND DNM_INV_NO = IM_INVOICE_NO " & _
                                "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID AND DND_INV_LINE = ID_INVOICE_LINE " & _
                                "INNER JOIN STATUS_MSTR ON STATUS_TYPE = 'DN' AND DNM_DN_STATUS = STATUS_NO " & _
                                "INNER JOIN COMPANY_MSTR CMA ON DNM_DN_S_COY_ID = CMA.CM_COY_ID " & _
                                "INNER JOIN COMPANY_MSTR CMB ON DNM_DN_B_COY_ID = CMB.CM_COY_ID " & _
                                "LEFT JOIN TAX ON TAX_COUNTRY_CODE = CMA.CM_COUNTRY AND TAX_CODE = DND_GST_RATE " & _
                                "WHERE DNM_DN_B_COY_ID = '" & Session("CompanyID") & "' " & _
                                "AND DNM_CREATED_DATE >= '" & strBeginDate & "' AND DNM_CREATED_DATE <='" & strEndDate & "' " & _
                                "GROUP BY DNM_DN_NO, DNM_DN_S_COY_ID " & _
                                "ORDER BY DNM_CREATED_DATE DESC "

                ElseIf ViewState("name") = "DNSummV" Then
                    'Issue 7480 - CH - 25 Mar 2015 (No.65): Remove Exchange Rate
                    .CommandText = "SELECT DNM_DN_NO AS 'Debit Note/Debit Advice Number', DATE_FORMAT(DNM_CREATED_DATE,'%d/%m/%Y') AS 'Debit Note/Debit Advice Date', " & _
                                "CMB.CM_COY_NAME AS 'Buyer Name', STATUS_DESC AS 'Debit Note/Debit Advice Status', DNM_INV_NO AS 'Invoice Number', " & _
                                "DNM_CURRENCY_CODE AS 'Currency', " & _
                                "SUM((DND_UNIT_COST * DND_QTY) + " & _
                                "((DND_UNIT_COST * DND_QTY) * IF(IFNULL(TAX_PERC, '') = 'N/A' OR IFNULL(TAX_PERC, '') = '', 0, TAX_PERC) / 100)) AS 'Amount' " & _
                                "FROM DEBIT_NOTE_DETAILS " & _
                                "INNER JOIN DEBIT_NOTE_MSTR ON DND_DN_S_COY_ID = DNM_DN_S_COY_ID AND DND_DN_NO = DNM_DN_NO " & _
                                "INNER JOIN INVOICE_MSTR ON DNM_DN_S_COY_ID = IM_S_COY_ID AND DNM_INV_NO = IM_INVOICE_NO " & _
                                "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID AND DND_INV_LINE = ID_INVOICE_LINE " & _
                                "INNER JOIN STATUS_MSTR ON STATUS_TYPE = 'DN' AND DNM_DN_STATUS = STATUS_NO " & _
                                "INNER JOIN COMPANY_MSTR CMA ON DNM_DN_S_COY_ID = CMA.CM_COY_ID " & _
                                "INNER JOIN COMPANY_MSTR CMB ON DNM_DN_B_COY_ID = CMB.CM_COY_ID " & _
                                "LEFT JOIN TAX ON TAX_COUNTRY_CODE = CMA.CM_COUNTRY AND TAX_CODE = DND_GST_RATE " & _
                                "WHERE DNM_DN_S_COY_ID = '" & Session("CompanyID") & "' " & _
                                "AND DNM_CREATED_DATE >= '" & strBeginDate & "' AND DNM_CREATED_DATE <='" & strEndDate & "' " & _
                                "GROUP BY DNM_DN_NO, DNM_DN_S_COY_ID " & _
                                "ORDER BY DNM_CREATED_DATE DESC "

                ElseIf ViewState("name") = "CNSumm" Then
                    'Issue 7480 - CH - 25 Mar 2015 (No.65): Remove Exchange Rate
                    .CommandText = "SELECT CNM_CN_NO AS 'Credit Note/Credit Advice Number', DATE_FORMAT(CNM_CREATED_DATE,'%d/%m/%Y') AS 'Credit Note/Credit Advice Date', " & _
                                "CMA.CM_COY_NAME AS 'Vendor Name', STATUS_DESC AS 'Credit Note/Credit Advice Status', CNM_INV_NO AS 'Invoice Number', " & _
                                "CNM_CURRENCY_CODE AS 'Currency', " & _
                                "SUM((CND_UNIT_COST * CND_QTY) + " & _
                                "((CND_UNIT_COST * CND_QTY) * IF(IFNULL(TAX_PERC, '') = 'N/A' OR IFNULL(TAX_PERC, '') = '', 0, TAX_PERC) / 100)) AS 'Amount' " & _
                                "FROM CREDIT_NOTE_DETAILS " & _
                                "INNER JOIN CREDIT_NOTE_MSTR ON CND_CN_S_COY_ID = CNM_CN_S_COY_ID AND CND_CN_NO = CNM_CN_NO " & _
                                "INNER JOIN INVOICE_MSTR ON CNM_CN_S_COY_ID = IM_S_COY_ID AND CNM_INV_NO = IM_INVOICE_NO " & _
                                "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID AND CND_INV_LINE = ID_INVOICE_LINE " & _
                                "INNER JOIN STATUS_MSTR ON STATUS_TYPE = 'CN' AND CNM_CN_STATUS = STATUS_NO " & _
                                "INNER JOIN COMPANY_MSTR CMA ON CNM_CN_S_COY_ID = CMA.CM_COY_ID " & _
                                "INNER JOIN COMPANY_MSTR CMB ON CNM_CN_B_COY_ID = CMB.CM_COY_ID " & _
                                "LEFT JOIN TAX ON TAX_COUNTRY_CODE = CMA.CM_COUNTRY AND TAX_CODE = CND_GST_RATE " & _
                                "WHERE CNM_CN_B_COY_ID = '" & Session("CompanyID") & "' " & _
                                "AND CNM_CREATED_DATE >= '" & strBeginDate & "' AND CNM_CREATED_DATE <='" & strEndDate & "' " & _
                                "GROUP BY CNM_CN_NO, CNM_CN_S_COY_ID " & _
                                "ORDER BY CNM_CREATED_DATE DESC "

                ElseIf ViewState("name") = "CNSummV" Then
                    'Issue 7480 - CH - 25 Mar 2015 (No.65): Remove Exchange Rate
                    .CommandText = "SELECT CNM_CN_NO AS 'Credit Note/Credit Advice Number', DATE_FORMAT(CNM_CREATED_DATE,'%d/%m/%Y') AS 'Credit Note/Credit Advice Date', " & _
                               "CMB.CM_COY_NAME AS 'Buyer Name', STATUS_DESC AS 'Credit Note/Credit Advice Status', CNM_INV_NO AS 'Invoice Number', " & _
                               "CNM_CURRENCY_CODE AS 'Currency', " & _
                               "SUM((CND_UNIT_COST * CND_QTY) + " & _
                               "((CND_UNIT_COST * CND_QTY) * IF(IFNULL(TAX_PERC, '') = 'N/A' OR IFNULL(TAX_PERC, '') = '', 0, TAX_PERC) / 100)) AS 'Amount' " & _
                               "FROM CREDIT_NOTE_DETAILS " & _
                               "INNER JOIN CREDIT_NOTE_MSTR ON CND_CN_S_COY_ID = CNM_CN_S_COY_ID AND CND_CN_NO = CNM_CN_NO " & _
                               "INNER JOIN INVOICE_MSTR ON CNM_CN_S_COY_ID = IM_S_COY_ID AND CNM_INV_NO = IM_INVOICE_NO " & _
                               "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID AND CND_INV_LINE = ID_INVOICE_LINE " & _
                               "INNER JOIN STATUS_MSTR ON STATUS_TYPE = 'CN' AND CNM_CN_STATUS = STATUS_NO " & _
                               "INNER JOIN COMPANY_MSTR CMA ON CNM_CN_S_COY_ID = CMA.CM_COY_ID " & _
                               "INNER JOIN COMPANY_MSTR CMB ON CNM_CN_B_COY_ID = CMB.CM_COY_ID " & _
                               "LEFT JOIN TAX ON TAX_COUNTRY_CODE = CMA.CM_COUNTRY AND TAX_CODE = CND_GST_RATE " & _
                               "WHERE CNM_CN_S_COY_ID = '" & Session("CompanyID") & "' " & _
                               "AND CNM_CREATED_DATE >= '" & strBeginDate & "' AND CNM_CREATED_DATE <='" & strEndDate & "' " & _
                               "GROUP BY CNM_CN_NO, CNM_CN_S_COY_ID " & _
                               "ORDER BY CNM_CREATED_DATE DESC "
                    '------------------------------------------------
                ElseIf ViewState("name") = "BillSummV" Then
                    .CommandText = "SELECT DOC_TYPE AS 'Document Type', DOC_NO AS 'Document No', DOC_DATE AS 'Document Date', " & _
                                "BUYER_COY_NAME AS 'Buyer Company', ROUND(AMT,2) AS 'Amount', ROUND(GST_AMT,2) AS 'GST Amount', " & _
                                "ROUND(TOTAL_AMT,2) AS 'Total Amount', ROUND(SHIP_AMT,2) AS 'Shipping Amount', STATUS_DESC AS 'Status' FROM " & _
                                "(SELECT 'INV' AS DOC_TYPE, IM_INVOICE_NO AS DOC_NO, IM_CREATED_ON AS DOC_DATE, " & _
                                "IM_INVOICE_NO AS INV_NO, IM_CREATED_ON AS INV_DATE, CM_COY_NAME AS BUYER_COY_NAME, " & _
                                "SUM(ROUND(ID_UNIT_COST * ID_RECEIVED_QTY,2)) AS AMT, " & _
                                "SUM(ROUND(ID_GST_VALUE,2)) AS GST_AMT, IM_INVOICE_TOTAL AS TOTAL_AMT, IM_SHIP_AMT AS SHIP_AMT, " & _
                                "STATUS_DESC " & _
                                "FROM INVOICE_MSTR " & _
                                "INNER JOIN INVOICE_DETAILS ON ID_INVOICE_NO = IM_INVOICE_NO AND ID_S_COY_ID = IM_S_COY_ID " & _
                                "INNER JOIN PO_MSTR ON POM_PO_INDEX = IM_PO_INDEX " & _
                                "INNER JOIN COMPANY_MSTR ON CM_COY_ID = IM_B_COY_ID " & _
                                "INNER JOIN STATUS_MSTR ON STATUS_TYPE = 'INV' AND IM_INVOICE_STATUS = STATUS_NO " & _
                                "WHERE IM_S_COY_ID = '" & Session("CompanyID") & "' " & _
                                "AND IM_CREATED_ON >= '" & strBeginDate & "' AND IM_CREATED_ON <='" & strEndDate & "' " & _
                                "GROUP BY IM_INVOICE_NO " & _
                                "UNION ALL " & _
                                "SELECT CNM_CN_TYPE AS DOC_TYPE, CNM_CN_NO AS DOC_NO, CNM_CREATED_DATE AS DOC_DATE, " & _
                                "CNM_INV_NO AS INV_NO, IM_CREATED_ON AS INV_DATE, CM_COY_NAME AS BUYER_COY_NAME, " & _
                                "SUM(ROUND(CND_UNIT_COST * CND_QTY,2)) AS AMT, " & _
                                "(CNM_CN_TOTAL - SUM(ROUND(CND_UNIT_COST * CND_QTY,2))) AS GST_AMT, " & _
                                "CNM_CN_TOTAL AS TOTAL_AMT, '' AS SHIP_AMT, STATUS_DESC " & _
                                "FROM INVOICE_MSTR " & _
                                "INNER JOIN CREDIT_NOTE_MSTR ON CNM_CN_S_COY_ID = IM_S_COY_ID AND CNM_INV_NO = IM_INVOICE_NO " & _
                                "INNER JOIN CREDIT_NOTE_DETAILS ON CND_CN_S_COY_ID = CNM_CN_S_COY_ID AND CND_CN_NO = CNM_CN_NO " & _
                                "INNER JOIN COMPANY_MSTR ON CM_COY_ID = IM_B_COY_ID " & _
                                "INNER JOIN STATUS_MSTR ON STATUS_TYPE = 'CN' AND CNM_CN_STATUS = STATUS_NO " & _
                                "WHERE IM_S_COY_ID = '" & Session("CompanyID") & "' " & _
                                "AND IM_CREATED_ON >= '" & strBeginDate & "' AND IM_CREATED_ON <='" & strEndDate & "' " & _
                                "GROUP BY CNM_CN_NO " & _
                                "UNION ALL " & _
                                "SELECT DNM_DN_TYPE AS DOC_TYPE, DNM_DN_NO AS DOC_NO, DNM_CREATED_DATE AS DOC_DATE, " & _
                                "DNM_INV_NO AS INV_NO, IM_CREATED_ON AS INV_DATE, CM_COY_NAME AS BUYER_COY_NAME, " & _
                                "SUM(ROUND(DND_UNIT_COST * DND_QTY,2)) AS AMT, " & _
                                "(DNM_DN_TOTAL - SUM(ROUND(DND_UNIT_COST * DND_QTY,2))) AS GST_AMT, " & _
                                "DNM_DN_TOTAL AS TOTAL_AMT, '' AS SHIP_AMT, STATUS_DESC " & _
                                "FROM INVOICE_MSTR " & _
                                "INNER JOIN DEBIT_NOTE_MSTR ON DNM_DN_S_COY_ID = IM_S_COY_ID AND DNM_INV_NO = IM_INVOICE_NO " & _
                                "INNER JOIN DEBIT_NOTE_DETAILS ON DND_DN_S_COY_ID = DNM_DN_S_COY_ID AND DND_DN_NO = DNM_DN_NO " & _
                                "INNER JOIN COMPANY_MSTR ON CM_COY_ID = IM_B_COY_ID " & _
                                "INNER JOIN STATUS_MSTR ON STATUS_TYPE = 'DN' AND DNM_DN_STATUS = STATUS_NO " & _
                                "WHERE IM_S_COY_ID = '" & Session("CompanyID") & "' " & _
                                "AND IM_CREATED_ON >= '" & strBeginDate & "' AND IM_CREATED_ON <='" & strEndDate & "' " & _
                                "GROUP BY DNM_DN_NO) tb " & _
                                "ORDER BY BUYER_COY_NAME, INV_DATE, DOC_DATE"
                Else
                    If ViewState("ReportType") = "Buyer" Then
                        'Jules 2018.09.05 - Added GROUP BY IM_INVOICE_INDEX.
                        .CommandText = "SELECT INVOICE_MSTR.IM_INVOICE_NO AS 'Invoice No.',DATE_FORMAT(INVOICE_MSTR.IM_CREATED_ON,'%d/%m/%Y') AS 'Invoice Date', " _
                                & "DATE_FORMAT(INVOICE_MSTR.IM_PAYMENT_DATE,'%d/%m/%Y') AS 'Due Date',INVOICE_MSTR.IM_S_COY_NAME AS 'Vendor Name', " _
                                & "company_dept_mstr.CDM_DEPT_NAME AS 'Department',PO_MSTR.POM_BUYER_NAME AS 'Buyer Name'," _
                                & "STATUS_DESC AS 'Invoice Status',PO_MSTR.POM_PO_NO AS 'PO Number',DATE_FORMAT(PO_MSTR.POM_PO_DATE,'%d/%m/%Y') AS 'PO Date', " _
                                & "COMPANY_MSTR.CM_CURRENCY_CODE AS 'Currency', FORMAT(SUM(ROUND(ID_RECEIVED_QTY*ID_UNIT_COST,2)),2) AS 'Amount', FORMAT(INVOICE_MSTR.IM_INVOICE_TOTAL,2) AS 'GST Amount' " _
                                & "FROM INVOICE_MSTR " _
                                & "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " _
                                & "INNER JOIN PO_MSTR ON INVOICE_MSTR.IM_PO_INDEX = PO_MSTR.POM_PO_INDEX  " _
                                & "AND INVOICE_MSTR.IM_S_COY_ID = PO_MSTR.POM_S_COY_ID  " _
                                & "INNER JOIN COMPANY_MSTR ON IM_B_COY_ID = COMPANY_MSTR.CM_COY_ID  " _
                                & "LEFT OUTER JOIN company_dept_mstr ON company_dept_mstr.CDM_DEPT_INDEX = po_mstr.POM_DEPT_INDEX  " _
                                & "INNER JOIN status_mstr ON status_mstr.STATUS_NO=INVOICE_MSTR.IM_INVOICE_STATUS AND STATUS_TYPE='INV'  " _
                                & "WHERE (INVOICE_MSTR.IM_B_COY_ID ='" & Session("CompanyID") & "') " _
                                & "AND IM_CREATED_ON>= '" & strBeginDate & "' AND IM_CREATED_ON<='" & strEndDate & "' " _
                                & "GROUP BY IM_INVOICE_INDEX " _
                                & "ORDER BY IM_CREATED_ON DESC"
                    Else
                        'modify by mimi 07/02/2017
                        .CommandText = "SELECT INVOICE_MSTR.IM_INVOICE_NO AS 'Invoice No.',DATE_FORMAT(INVOICE_MSTR.IM_CREATED_ON,'%d/%m/%Y') AS 'Invoice Date', " _
                            & "DATE_FORMAT(INVOICE_MSTR.IM_PAYMENT_DATE,'%d/%m/%Y') AS 'Due Date',INVOICE_MSTR.IM_S_COY_NAME AS 'Vendor Name', " _
                            & "company_dept_mstr.CDM_DEPT_NAME AS 'Department',PO_MSTR.POM_BUYER_NAME AS 'Buyer Name'," _
                            & "STATUS_DESC AS 'Invoice Status',PO_MSTR.POM_PO_NO AS 'PO Number',DATE_FORMAT(PO_MSTR.POM_PO_DATE,'%d/%m/%Y') AS 'PO Date', " _
                            & "COMPANY_MSTR.CM_CURRENCY_CODE AS 'Currency', FORMAT(SUM(ROUND(ID_RECEIVED_QTY*ID_UNIT_COST,2)),2) AS 'Amount', FORMAT(INVOICE_MSTR.IM_INVOICE_TOTAL,2) AS 'GST Amount' " _
                            & "FROM INVOICE_MSTR " _
                            & "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " _
                            & "INNER JOIN PO_MSTR ON INVOICE_MSTR.IM_PO_INDEX = PO_MSTR.POM_PO_INDEX  " _
                            & "AND INVOICE_MSTR.IM_S_COY_ID = PO_MSTR.POM_S_COY_ID  " _
                            & "INNER JOIN COMPANY_MSTR ON IM_B_COY_ID = COMPANY_MSTR.CM_COY_ID  " _
                            & "LEFT OUTER JOIN company_dept_mstr ON company_dept_mstr.CDM_DEPT_INDEX = po_mstr.POM_DEPT_INDEX  " _
                            & "INNER JOIN status_mstr ON status_mstr.STATUS_NO=INVOICE_MSTR.IM_INVOICE_STATUS AND STATUS_TYPE='INV' AND STATUS_NO IN (1,2,3,4) " _
                            & "WHERE (INVOICE_MSTR.IM_S_COY_ID ='" & Session("CompanyID") & "') " _
                            & "AND IM_CREATED_ON>= '" & strBeginDate & "' AND IM_CREATED_ON<='" & strEndDate & "' " _
                            & "GROUP BY IM_INVOICE_INDEX " _
                            & "ORDER BY IM_CREATED_ON DESC"
                    End If
                End If
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
			'Modified for Agora GST Stage 2 - CH - 2/2/2015
            If ViewState("name") = "DNSumm" Or ViewState("name") = "DNSummV" Then
                strFileName = "DN_DA_SummaryReport" & "(" & Format(dtFrom, "MMMyyyy") & ").xls"
            ElseIf ViewState("name") = "CNSumm" Or ViewState("name") = "CNSummV" Then
                strFileName = "CN_CA_SummaryReport" & "(" & Format(dtFrom, "MMMyyyy") & ").xls"
            ElseIf ViewState("name") = "BillSumm" Or ViewState("name") = "BillSummV" Then
                strFileName = "BillingSummaryReport" & "(" & Format(dtFrom, "MMMyyyy") & ").xls"
            Else
                strFileName = "InvSummaryReport" & "(" & Format(dtFrom, "MMMyyyy") & ").xls"
            End If
			'-----------------------------------------------
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