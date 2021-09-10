Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Public Class ReportFormatN4
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
		'--------------------------------------

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
        Dim strCT As String

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
                'Modified for Agora GST Stage 2 - CH - 3/3/2015
                If ViewState("name") = "DNDetails" Then
                    'Issue 7480 - CH - 25 Mar 2015 (No.65): Remove Exchange Rate
                    .CommandText = "SELECT DNM_DN_NO, DNM_CREATED_DATE, " &
                                "CMA.CM_COY_NAME AS COY_S_NAME, CMB.CM_COY_NAME AS COY_B_NAME, STATUS_DESC, DNM_INV_NO, " &
                                "ID_B_ITEM_CODE, ID_PRODUCT_DESC, ID_UOM, DND_QTY, DNM_CURRENCY_CODE, " &
                                "IFNULL(DNM_EXCHANGE_RATE, 1) AS DNM_EXCHANGE_RATE, DND_UNIT_COST, IFNULL(TAX_PERC, 0) AS ID_GST " &
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
                                "ORDER BY DNM_CREATED_DATE DESC "

                ElseIf ViewState("name") = "DNDetailsV" Then
                    'Issue 7480 - CH - 25 Mar 2015 (No.65): Remove Exchange Rate
                    .CommandText = "SELECT DNM_DN_NO, DNM_CREATED_DATE, " &
                                "CMA.CM_COY_NAME AS COY_S_NAME, CMB.CM_COY_NAME AS COY_B_NAME, STATUS_DESC, DNM_INV_NO, " &
                                "ID_B_ITEM_CODE, ID_PRODUCT_DESC, ID_UOM, DND_QTY, DNM_CURRENCY_CODE, " &
                                "IFNULL(DNM_EXCHANGE_RATE, 1) AS DNM_EXCHANGE_RATE, DND_UNIT_COST, IFNULL(TAX_PERC, 0) AS ID_GST " &
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
                                "ORDER BY DNM_CREATED_DATE DESC "

                ElseIf ViewState("name") = "CNDetails" Then
                    'Issue 7480 - CH - 25 Mar 2015 (No.65): Remove Exchange Rate
                    .CommandText = "SELECT CNM_CN_NO, CNM_CREATED_DATE, " &
                                "CMA.CM_COY_NAME AS COY_S_NAME, CMB.CM_COY_NAME AS COY_B_NAME, STATUS_DESC, CNM_INV_NO, " &
                                "ID_B_ITEM_CODE, ID_PRODUCT_DESC, ID_UOM, CND_QTY, CNM_CURRENCY_CODE, " &
                                "IFNULL(CNM_EXCHANGE_RATE, 1) AS CNM_EXCHANGE_RATE, CND_UNIT_COST, IFNULL(TAX_PERC, 0) AS ID_GST " &
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
                                "ORDER BY CNM_CREATED_DATE DESC "

                ElseIf ViewState("name") = "CNDetailsV" Then
                    'Issue 7480 - CH - 25 Mar 2015 (No.65): Remove Exchange Rate
                    .CommandText = "SELECT CNM_CN_NO, CNM_CREATED_DATE, " &
                                "CMA.CM_COY_NAME AS COY_S_NAME, CMB.CM_COY_NAME AS COY_B_NAME, STATUS_DESC, CNM_INV_NO, " &
                                "ID_B_ITEM_CODE, ID_PRODUCT_DESC, ID_UOM, CND_QTY, CNM_CURRENCY_CODE, " &
                                "IFNULL(CNM_EXCHANGE_RATE, 1) AS CNM_EXCHANGE_RATE, CND_UNIT_COST, IFNULL(TAX_PERC, 0) AS ID_GST " &
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
                                "ORDER BY CNM_CREATED_DATE DESC "

                    '-----------------------------------------------
                Else
                    If ViewState("ReportType") = "Buyer" Then
                        .CommandText = "SELECT   INVOICE_DETAILS.ID_INVOICE_NO, INVOICE_DETAILS.ID_S_COY_ID, INVOICE_DETAILS.ID_INVOICE_LINE, " _
                                & "INVOICE_DETAILS.ID_PO_LINE, INVOICE_DETAILS.ID_PRODUCT_DESC, INVOICE_DETAILS.ID_B_ITEM_CODE, a.CDM_DEPT_NAME, AM_DEPT_INDEX, " _
                                & "b.CDM_DEPT_NAME AS Cost_Center, CT_NAME,  " _
                                & "INVOICE_DETAILS.ID_UOM, INVOICE_DETAILS.ID_GST, INVOICE_DETAILS.ID_RECEIVED_QTY, " _
                                & "INVOICE_DETAILS.ID_UNIT_COST, INVOICE_DETAILS.ID_WARRANTY_TERMS, INVOICE_DETAILS.ID_ACCT_INDEX, " _
                                & "INVOICE_DETAILS.ID_B_CATEGORY_CODE, INVOICE_DETAILS.ID_B_GL_CODE, INVOICE_MSTR.IM_INVOICE_INDEX, " _
                                & "INVOICE_MSTR.IM_INVOICE_NO, INVOICE_MSTR.IM_S_COY_ID, INVOICE_MSTR.IM_S_COY_NAME, INVOICE_MSTR.IM_PO_INDEX, " _
                                & "INVOICE_MSTR.IM_B_COY_ID, INVOICE_MSTR.IM_PAYMENT_DATE, INVOICE_MSTR.IM_REMARK, " _
                                & "INVOICE_MSTR.IM_CREATED_BY, INVOICE_MSTR.IM_CREATED_ON, INVOICE_MSTR.IM_INVOICE_STATUS,STATUS_DESC, " _
                                & "INVOICE_MSTR.IM_PAYMENT_NO, INVOICE_MSTR.IM_YOUR_REF, INVOICE_MSTR.IM_OUR_REF, " _
                                & "INVOICE_MSTR.IM_INVOICE_PREFIX, INVOICE_MSTR.IM_SUBMITTEDBY_FO, INVOICE_MSTR.IM_EXCHANGE_RATE, " _
                                & "INVOICE_MSTR.IM_FINANCE_REMARKS, INVOICE_MSTR.IM_PRINTED, INVOICE_MSTR.IM_FOLDER, " _
                                & "INVOICE_MSTR.IM_FM_APPROVED_DATE, INVOICE_MSTR.IM_DOWNLOADED_DATE, INVOICE_MSTR.IM_EXTERNAL_IND, " _
                                & "INVOICE_MSTR.IM_REFERENCE_NO, INVOICE_MSTR.IM_INVOICE_TOTAL, INVOICE_MSTR.IM_PAYMENT_TERM, " _
                                & "INVOICE_MSTR.IM_STATUS_CHANGED_BY, INVOICE_MSTR.IM_STATUS_CHANGED_ON, PO_DETAILS.POD_COY_ID, " _
                                & "PO_DETAILS.POD_PO_NO, PO_DETAILS.POD_PO_LINE, PO_DETAILS.POD_PRODUCT_CODE, " _
                                & "PO_DETAILS.POD_VENDOR_ITEM_CODE, PO_DETAILS.POD_PRODUCT_DESC, PO_DETAILS.POD_UOM, " _
                                & "PO_DETAILS.POD_ORDERED_QTY, PO_DETAILS.POD_RECEIVED_QTY, PO_DETAILS.POD_REJECTED_QTY, " _
                                & "PO_DETAILS.POD_DELIVERED_QTY, PO_DETAILS.POD_CANCELLED_QTY, PO_DETAILS.POD_MIN_PACK_QTY, " _
                                & "PO_DETAILS.POD_MIN_ORDER_QTY, PO_DETAILS.POD_ETD, PO_DETAILS.POD_WARRANTY_TERMS, " _
                                & "PO_DETAILS.POD_UNIT_COST, PO_DETAILS.POD_REMARK, PO_DETAILS.POD_GST, PO_DETAILS.POD_PR_INDEX, " _
                                & "PO_DETAILS.POD_PR_LINE, PO_DETAILS.POD_ACCT_INDEX, PO_DETAILS.POD_PRODUCT_TYPE, " _
                                & "PO_DETAILS.POD_B_ITEM_CODE, PO_DETAILS.POD_SOURCE, " _
                                & "PO_DETAILS.POD_B_CATEGORY_CODE, PO_DETAILS.POD_B_GL_CODE, PO_MSTR.POM_PO_INDEX, PO_MSTR.POM_PO_NO, " _
                                & "PO_MSTR.POM_B_COY_ID, PO_MSTR.POM_BUYER_ID, PO_MSTR.POM_BUYER_NAME, PO_MSTR.POM_S_COY_ID, PO_MSTR.POM_S_COY_NAME, PO_MSTR.POM_S_ATTN, " _
                                & "PO_MSTR.POM_S_REMARK, PO_MSTR.POM_PO_DATE,  " _
                                & "PO_MSTR.POM_FREIGHT_TERMS, PO_MSTR.POM_PAYMENT_TERM, PO_MSTR.POM_PAYMENT_METHOD,  " _
                                & "PO_MSTR.POM_SHIPMENT_MODE, PO_MSTR.POM_SHIPMENT_TERM, PO_MSTR.POM_CURRENCY_CODE, " _
                                & "PO_MSTR.POM_EXCHANGE_RATE, PO_MSTR.POM_PAYMENT_TERM_CODE, PO_MSTR.POM_SHIP_VIA,  " _
                                & "PO_MSTR.POM_PO_STATUS, PO_MSTR.POM_STATUS_CHANGED_BY, PO_MSTR.POM_STATUS_CHANGED_ON,  " _
                                & "PO_MSTR.POM_EXTERNAL_REMARK, PO_MSTR.POM_CREATED_BY, PO_MSTR.POM_PO_COST, PO_MSTR.POM_BILLING_METHOD, " _
                                & "PO_MSTR.POM_PO_PREFIX, PO_MSTR.POM_FULFILMENT, PO_MSTR.POM_DEPT_INDEX, PO_MSTR.POM_ACCEPTED_DATE, " _
                                & "PO_MSTR.POM_DOWNLOADED_DATE, PO_MSTR.POM_ARCHIVE_IND, PO_MSTR.POM_TERMANDCOND, " _
                                & "PO_MSTR.POM_REFERENCE_NO, PO_MSTR.POM_EXTERNAL_IND, COMPANY_MSTR.CM_COY_ID, " _
                                & "COMPANY_MSTR.CM_COY_NAME, COMPANY_MSTR.CM_COY_TYPE, COMPANY_MSTR.CM_PARENT_COY_ID, " _
                                & "COMPANY_MSTR.CM_ACCT_NO, COMPANY_MSTR.CM_BANK, COMPANY_MSTR.CM_BRANCH, COMPANY_MSTR.CM_ADDR_LINE1, " _
                                & "COMPANY_MSTR.CM_ADDR_LINE2, COMPANY_MSTR.CM_ADDR_LINE3, COMPANY_MSTR.CM_POSTCODE, " _
                                & "COMPANY_MSTR.CM_CITY, COMPANY_MSTR.CM_STATE, COMPANY_MSTR.CM_COUNTRY, COMPANY_MSTR.CM_PHONE, " _
                                & "COMPANY_MSTR.CM_FAX, COMPANY_MSTR.CM_EMAIL, COMPANY_MSTR.CM_COY_LOGO, " _
                                & "COMPANY_MSTR.CM_BUSINESS_REG_NO, COMPANY_MSTR.CM_TAX_REG_NO, COMPANY_MSTR.CM_PAYMENT_TERM, " _
                                & "COMPANY_MSTR.CM_PAYMENT_METHOD, COMPANY_MSTR.CM_ACTUAL_TERMSANDCONDFILE, " _
                                & "COMPANY_MSTR.CM_HUB_TERMSANDCONDFILE, COMPANY_MSTR.CM_PWD_DURATION, COMPANY_MSTR.CM_TAX_CALC_BY, " _
                                & "COMPANY_MSTR.CM_CURRENCY_CODE, COMPANY_MSTR.CM_BCM_SET, COMPANY_MSTR.CM_BUDGET_FROM_DATE, " _
                                & "COMPANY_MSTR.CM_BUDGET_TO_DATE, COMPANY_MSTR.CM_RFQ_OPTION, COMPANY_MSTR.CM_LICENCE_PACKAGE, " _
                                & "COMPANY_MSTR.CM_LICENSE_USERS, COMPANY_MSTR.CM_SUB_START_DT, COMPANY_MSTR.CM_SUB_END_DT, " _
                                & "COMPANY_MSTR.CM_LICENSE_PRODUCTS, COMPANY_MSTR.CM_FINDEPT_MODE, COMPANY_MSTR.CM_PRIV_LABELING, " _
                                & "COMPANY_MSTR.CM_SKINS_ID, COMPANY_MSTR.CM_TRAINING, COMPANY_MSTR.CM_STATUS, COMPANY_MSTR.CM_DELETED, " _
                                & "COMPANY_MSTR.CM_MOD_BY, COMPANY_MSTR.CM_MOD_DT, COMPANY_MSTR.CM_ENT_BY, COMPANY_MSTR.CM_ENT_DT, " _
                                & "COMPANY_MSTR.CM_SKU, COMPANY_MSTR.CM_TRANS_NO, COMPANY_MSTR.CM_CONTACT, " _
                                & "COMPANY_MSTR.CM_REPORT_USERS, COMPANY_MSTR.CM_INV_APPR, COMPANY_MSTR.CM_MULTI_PO, " _
                                & "COMPANY_MSTR.CM_BA_CANCEL,  " _
                                & "PO_MSTR.POM_PRINT_REMARK, PO_MSTR.POM_PRINT_CUSTOM_FIELDS, PO_MSTR.POM_SHIP_AMT, " _
                                & "INVOICE_MSTR.IM_SHIP_AMT FROM      INVOICE_DETAILS  " _
                                & "INNER JOIN INVOICE_MSTR ON INVOICE_DETAILS.ID_INVOICE_NO = INVOICE_MSTR.IM_INVOICE_NO " _
                                & "AND INVOICE_DETAILS.ID_S_COY_ID = INVOICE_MSTR.IM_S_COY_ID  " _
                                & "INNER JOIN PO_DETAILS ON INVOICE_DETAILS.ID_PO_LINE = PO_DETAILS.POD_PO_LINE " _
                                & "INNER JOIN PO_MSTR ON PO_DETAILS.POD_PO_NO = PO_MSTR.POM_PO_NO " _
                                & "AND PO_DETAILS.POD_COY_ID = PO_MSTR.POM_B_COY_ID  " _
                                & "AND INVOICE_MSTR.IM_PO_INDEX = PO_MSTR.POM_PO_INDEX  " _
                                & "AND INVOICE_MSTR.IM_S_COY_ID = PO_MSTR.POM_S_COY_ID  " _
                                & "INNER JOIN COMPANY_MSTR ON IM_B_COY_ID = COMPANY_MSTR.CM_COY_ID  " _
                                & "INNER JOIN status_mstr ON status_mstr.STATUS_NO=INVOICE_MSTR.IM_INVOICE_STATUS AND STATUS_TYPE='INV'  " _
                                & "LEFT OUTER JOIN company_dept_mstr a ON a.CDM_DEPT_INDEX = po_mstr.POM_DEPT_INDEX  " _
                                & "LEFT OUTER JOIN account_mstr ON account_mstr.AM_ACCT_INDEX=INVOICE_DETAILS.ID_ACCT_INDEX  " _
                                & "LEFT OUTER JOIN company_dept_mstr b ON account_mstr.AM_DEPT_INDEX = b.CDM_DEPT_INDEX  " _
                                & "LEFT JOIN PRODUCT_MSTR ON POD_PRODUCT_CODE = PM_PRODUCT_CODE  " _
                                & "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID  " _
                                & "WHERE (INVOICE_MSTR.IM_B_COY_ID = @prmCoyID)  " _
                                & "AND IM_CREATED_ON>= @prmStartDate AND IM_CREATED_ON<=@prmEndDate  " _
                                & "ORDER BY IM_CREATED_ON DESC"
                    Else
                        .CommandText = "SELECT   INVOICE_DETAILS.ID_INVOICE_NO, INVOICE_DETAILS.ID_S_COY_ID, INVOICE_DETAILS.ID_INVOICE_LINE, " _
                                & "INVOICE_DETAILS.ID_PO_LINE, INVOICE_DETAILS.ID_PRODUCT_DESC, INVOICE_DETAILS.ID_B_ITEM_CODE, a.CDM_DEPT_NAME, AM_DEPT_INDEX, " _
                                & "b.CDM_DEPT_NAME AS Cost_Center, CT_NAME,  " _
                                & "INVOICE_DETAILS.ID_UOM, INVOICE_DETAILS.ID_GST, INVOICE_DETAILS.ID_RECEIVED_QTY, " _
                                & "INVOICE_DETAILS.ID_UNIT_COST, INVOICE_DETAILS.ID_WARRANTY_TERMS, INVOICE_DETAILS.ID_ACCT_INDEX, " _
                                & "INVOICE_DETAILS.ID_B_CATEGORY_CODE, INVOICE_DETAILS.ID_B_GL_CODE, INVOICE_MSTR.IM_INVOICE_INDEX, " _
                                & "INVOICE_MSTR.IM_INVOICE_NO, INVOICE_MSTR.IM_S_COY_ID, INVOICE_MSTR.IM_S_COY_NAME, INVOICE_MSTR.IM_PO_INDEX, " _
                                & "INVOICE_MSTR.IM_B_COY_ID, INVOICE_MSTR.IM_PAYMENT_DATE, INVOICE_MSTR.IM_REMARK, " _
                                & "INVOICE_MSTR.IM_CREATED_BY, INVOICE_MSTR.IM_CREATED_ON, INVOICE_MSTR.IM_INVOICE_STATUS,STATUS_DESC, " _
                                & "INVOICE_MSTR.IM_PAYMENT_NO, INVOICE_MSTR.IM_YOUR_REF, INVOICE_MSTR.IM_OUR_REF, " _
                                & "INVOICE_MSTR.IM_INVOICE_PREFIX, INVOICE_MSTR.IM_SUBMITTEDBY_FO, INVOICE_MSTR.IM_EXCHANGE_RATE, " _
                                & "INVOICE_MSTR.IM_FINANCE_REMARKS, INVOICE_MSTR.IM_PRINTED, INVOICE_MSTR.IM_FOLDER, " _
                                & "INVOICE_MSTR.IM_FM_APPROVED_DATE, INVOICE_MSTR.IM_DOWNLOADED_DATE, INVOICE_MSTR.IM_EXTERNAL_IND, " _
                                & "INVOICE_MSTR.IM_REFERENCE_NO, INVOICE_MSTR.IM_INVOICE_TOTAL, INVOICE_MSTR.IM_PAYMENT_TERM, " _
                                & "INVOICE_MSTR.IM_STATUS_CHANGED_BY, INVOICE_MSTR.IM_STATUS_CHANGED_ON, PO_DETAILS.POD_COY_ID, " _
                                & "PO_DETAILS.POD_PO_NO, PO_DETAILS.POD_PO_LINE, PO_DETAILS.POD_PRODUCT_CODE, " _
                                & "PO_DETAILS.POD_VENDOR_ITEM_CODE, PO_DETAILS.POD_PRODUCT_DESC, PO_DETAILS.POD_UOM, " _
                                & "PO_DETAILS.POD_ORDERED_QTY, PO_DETAILS.POD_RECEIVED_QTY, PO_DETAILS.POD_REJECTED_QTY, " _
                                & "PO_DETAILS.POD_DELIVERED_QTY, PO_DETAILS.POD_CANCELLED_QTY, PO_DETAILS.POD_MIN_PACK_QTY, " _
                                & "PO_DETAILS.POD_MIN_ORDER_QTY, PO_DETAILS.POD_ETD, PO_DETAILS.POD_WARRANTY_TERMS, " _
                                & "PO_DETAILS.POD_UNIT_COST, PO_DETAILS.POD_REMARK, PO_DETAILS.POD_GST, PO_DETAILS.POD_PR_INDEX, " _
                                & "PO_DETAILS.POD_PR_LINE, PO_DETAILS.POD_ACCT_INDEX, PO_DETAILS.POD_PRODUCT_TYPE, " _
                                & "PO_DETAILS.POD_B_ITEM_CODE, PO_DETAILS.POD_SOURCE, " _
                                & "PO_DETAILS.POD_B_CATEGORY_CODE, PO_DETAILS.POD_B_GL_CODE, PO_MSTR.POM_PO_INDEX, PO_MSTR.POM_PO_NO, " _
                                & "PO_MSTR.POM_B_COY_ID, PO_MSTR.POM_BUYER_ID, PO_MSTR.POM_BUYER_NAME, PO_MSTR.POM_S_COY_ID, PO_MSTR.POM_S_COY_NAME, PO_MSTR.POM_S_ATTN, " _
                                & "PO_MSTR.POM_S_REMARK, PO_MSTR.POM_PO_DATE,  " _
                                & "PO_MSTR.POM_FREIGHT_TERMS, PO_MSTR.POM_PAYMENT_TERM, PO_MSTR.POM_PAYMENT_METHOD,  " _
                                & "PO_MSTR.POM_SHIPMENT_MODE, PO_MSTR.POM_SHIPMENT_TERM, PO_MSTR.POM_CURRENCY_CODE, " _
                                & "PO_MSTR.POM_EXCHANGE_RATE, PO_MSTR.POM_PAYMENT_TERM_CODE, PO_MSTR.POM_SHIP_VIA,  " _
                                & "PO_MSTR.POM_PO_STATUS, PO_MSTR.POM_STATUS_CHANGED_BY, PO_MSTR.POM_STATUS_CHANGED_ON,  " _
                                & "PO_MSTR.POM_EXTERNAL_REMARK, PO_MSTR.POM_CREATED_BY, PO_MSTR.POM_PO_COST, PO_MSTR.POM_BILLING_METHOD, " _
                                & "PO_MSTR.POM_PO_PREFIX, PO_MSTR.POM_FULFILMENT, PO_MSTR.POM_DEPT_INDEX, PO_MSTR.POM_ACCEPTED_DATE, " _
                                & "PO_MSTR.POM_DOWNLOADED_DATE, PO_MSTR.POM_ARCHIVE_IND, PO_MSTR.POM_TERMANDCOND, " _
                                & "PO_MSTR.POM_REFERENCE_NO, PO_MSTR.POM_EXTERNAL_IND, COMPANY_MSTR.CM_COY_ID, " _
                                & "COMPANY_MSTR.CM_COY_NAME, COMPANY_MSTR.CM_COY_TYPE, COMPANY_MSTR.CM_PARENT_COY_ID, " _
                                & "COMPANY_MSTR.CM_ACCT_NO, COMPANY_MSTR.CM_BANK, COMPANY_MSTR.CM_BRANCH, COMPANY_MSTR.CM_ADDR_LINE1, " _
                                & "COMPANY_MSTR.CM_ADDR_LINE2, COMPANY_MSTR.CM_ADDR_LINE3, COMPANY_MSTR.CM_POSTCODE, " _
                                & "COMPANY_MSTR.CM_CITY, COMPANY_MSTR.CM_STATE, COMPANY_MSTR.CM_COUNTRY, COMPANY_MSTR.CM_PHONE, " _
                                & "COMPANY_MSTR.CM_FAX, COMPANY_MSTR.CM_EMAIL, COMPANY_MSTR.CM_COY_LOGO, " _
                                & "COMPANY_MSTR.CM_BUSINESS_REG_NO, COMPANY_MSTR.CM_TAX_REG_NO, COMPANY_MSTR.CM_PAYMENT_TERM, " _
                                & "COMPANY_MSTR.CM_PAYMENT_METHOD, COMPANY_MSTR.CM_ACTUAL_TERMSANDCONDFILE, " _
                                & "COMPANY_MSTR.CM_HUB_TERMSANDCONDFILE, COMPANY_MSTR.CM_PWD_DURATION, COMPANY_MSTR.CM_TAX_CALC_BY, " _
                                & "COMPANY_MSTR.CM_CURRENCY_CODE, COMPANY_MSTR.CM_BCM_SET, COMPANY_MSTR.CM_BUDGET_FROM_DATE, " _
                                & "COMPANY_MSTR.CM_BUDGET_TO_DATE, COMPANY_MSTR.CM_RFQ_OPTION, COMPANY_MSTR.CM_LICENCE_PACKAGE, " _
                                & "COMPANY_MSTR.CM_LICENSE_USERS, COMPANY_MSTR.CM_SUB_START_DT, COMPANY_MSTR.CM_SUB_END_DT, " _
                                & "COMPANY_MSTR.CM_LICENSE_PRODUCTS, COMPANY_MSTR.CM_FINDEPT_MODE, COMPANY_MSTR.CM_PRIV_LABELING, " _
                                & "COMPANY_MSTR.CM_SKINS_ID, COMPANY_MSTR.CM_TRAINING, COMPANY_MSTR.CM_STATUS, COMPANY_MSTR.CM_DELETED, " _
                                & "COMPANY_MSTR.CM_MOD_BY, COMPANY_MSTR.CM_MOD_DT, COMPANY_MSTR.CM_ENT_BY, COMPANY_MSTR.CM_ENT_DT, " _
                                & "COMPANY_MSTR.CM_SKU, COMPANY_MSTR.CM_TRANS_NO, COMPANY_MSTR.CM_CONTACT, " _
                                & "COMPANY_MSTR.CM_REPORT_USERS, COMPANY_MSTR.CM_INV_APPR, COMPANY_MSTR.CM_MULTI_PO, " _
                                & "COMPANY_MSTR.CM_BA_CANCEL,  " _
                                & "PO_MSTR.POM_PRINT_REMARK, PO_MSTR.POM_PRINT_CUSTOM_FIELDS, PO_MSTR.POM_SHIP_AMT, " _
                                & "INVOICE_MSTR.IM_SHIP_AMT FROM      INVOICE_DETAILS  " _
                                & "INNER JOIN INVOICE_MSTR ON INVOICE_DETAILS.ID_INVOICE_NO = INVOICE_MSTR.IM_INVOICE_NO " _
                                & "AND INVOICE_DETAILS.ID_S_COY_ID = INVOICE_MSTR.IM_S_COY_ID  " _
                                & "INNER JOIN PO_DETAILS ON INVOICE_DETAILS.ID_PO_LINE = PO_DETAILS.POD_PO_LINE " _
                                & "INNER JOIN PO_MSTR ON PO_DETAILS.POD_PO_NO = PO_MSTR.POM_PO_NO " _
                                & "AND PO_DETAILS.POD_COY_ID = PO_MSTR.POM_B_COY_ID  " _
                                & "AND INVOICE_MSTR.IM_PO_INDEX = PO_MSTR.POM_PO_INDEX  " _
                                & "AND INVOICE_MSTR.IM_S_COY_ID = PO_MSTR.POM_S_COY_ID  " _
                                & "INNER JOIN COMPANY_MSTR ON IM_B_COY_ID = COMPANY_MSTR.CM_COY_ID  " _
                                & "INNER JOIN status_mstr ON status_mstr.STATUS_NO=INVOICE_MSTR.IM_INVOICE_STATUS AND STATUS_TYPE='INV' AND STATUS_NO IN (1,2,3,4) " _
                                & "LEFT OUTER JOIN company_dept_mstr a ON a.CDM_DEPT_INDEX = po_mstr.POM_DEPT_INDEX  " _
                                & "LEFT OUTER JOIN account_mstr ON account_mstr.AM_ACCT_INDEX=INVOICE_DETAILS.ID_ACCT_INDEX  " _
                                & "LEFT OUTER JOIN company_dept_mstr b ON account_mstr.AM_DEPT_INDEX = b.CDM_DEPT_INDEX  " _
                                & "LEFT JOIN PRODUCT_MSTR ON POD_PRODUCT_CODE = PM_PRODUCT_CODE  " _
                                & "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID  " _
                                & "WHERE (INVOICE_MSTR.IM_S_COY_ID = @prmCoyID)  " _
                                & "AND IM_CREATED_ON>= @prmStartDate AND IM_CREATED_ON<=@prmEndDate  " _
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
            strUserId = Session("UserName") 'Session("UserId")
            strCoyName = Session("CompanyName")

            da.Fill(ds)
            'Modified for Agora GST Stage 2 - CH - 2/2/2015
            Dim strDataSet As String = ""
            If ViewState("name") = "DNDetails" Or ViewState("name") = "DNDetailsV" Then
                strDataSet = "DnDetails_DataTable1"
            ElseIf ViewState("name") = "CNDetails" Or ViewState("name") = "CNDetailsV" Then
                strDataSet = "CnDetails_DataTable1"
            Else
                strDataSet = "InvDetails_xls_DataSetInvDetails"
            End If

            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource(strDataSet, ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)

            If ViewState("name") = "DNDetails" Then
                localreport.ReportPath = dispatcher.direct("Report", "DNDetails_pdf.rdlc", "Report")
            ElseIf ViewState("name") = "DNDetailsV" Then
                localreport.ReportPath = dispatcher.direct("Report", "DNDetailsVendor_pdf.rdlc", "Report")
            ElseIf ViewState("name") = "CNDetails" Then
                localreport.ReportPath = dispatcher.direct("Report", "CNDetails_pdf.rdlc", "Report")
            ElseIf ViewState("name") = "CNDetailsV" Then
                localreport.ReportPath = dispatcher.direct("Report", "CNDetailsVendor_pdf.rdlc", "Report")
            Else
                If ViewState("ReportType") = "Buyer" Then
                    localreport.ReportPath = dispatcher.direct("Report", "InvDetails_pdf.rdlc", "Report") 'Server.MapPath("InvDetails_pdf.rdlc")  'appPath & "Report\InvDetails_pdf.rdlc"
                Else
                    localreport.ReportPath = dispatcher.direct("Report", "InvDetailsVendor_pdf.rdlc", "Report") 'Server.MapPath("InvDetails_pdf.rdlc")  'appPath & "Report\InvDetails_pdf.rdlc"
                End If
            End If
            '-----------------------------------------
            localreport.EnableExternalImages = True

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
            If ViewState("name") = "DNDetails" Or ViewState("name") = "DNDetailsV" Then
                strFileName = "DN_DA_DetailsReport.pdf"
            ElseIf ViewState("name") = "CNDetails" Or ViewState("name") = "CNDetailsV" Then
                strFileName = "CN_CA_DetailsReport.pdf"
            Else
                strFileName = "InvDetailsReport.pdf"
            End If
            '-------------------------------------------------

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
                If ViewState("name") = "DNDetails" Then
                    'Issue 7480 - CH - 25 Mar 2015 (No.65): Remove Exchange Rate
                    .CommandText = "SELECT DNM_DN_NO AS 'Debit Note/Debit Advice Number', DATE_FORMAT(DNM_CREATED_DATE,'%d/%m/%Y') AS 'Debit Note/Debit Advice Date', " & _
                                "CMA.CM_COY_NAME AS 'Vendor Name', STATUS_DESC AS 'DN/DA Status', DNM_INV_NO AS 'Invoice Number', " & _
                                "ID_B_ITEM_CODE AS 'Item Code', ID_PRODUCT_DESC AS 'Item Description', ID_UOM AS 'UOM', DND_QTY AS 'Qty', DNM_CURRENCY_CODE AS 'Currency', " & _
                                "(DND_UNIT_COST * DND_QTY) + " & _
                                "((DND_UNIT_COST * DND_QTY) * IF(IFNULL(TAX_PERC, '') = 'N/A' OR IFNULL(TAX_PERC, '') = '', 0, TAX_PERC) / 100) AS 'Amount' " & _
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
                                "ORDER BY DNM_CREATED_DATE DESC "

                ElseIf ViewState("name") = "DNDetailsV" Then
                    'Issue 7480 - CH - 25 Mar 2015 (No.65): Remove Exchange Rate
                    .CommandText = "SELECT DNM_DN_NO AS 'Debit Note/Debit Advice Number', DATE_FORMAT(DNM_CREATED_DATE,'%d/%m/%Y') AS 'Debit Note/Debit Advice Date', " & _
                                "CMB.CM_COY_NAME AS 'Buyer Name', STATUS_DESC AS 'DN/DA Status', DNM_INV_NO AS 'Invoice Number', " & _
                                "ID_B_ITEM_CODE AS 'Item Code', ID_PRODUCT_DESC AS 'Item Description', ID_UOM AS 'UOM', DND_QTY AS 'Qty', DNM_CURRENCY_CODE AS 'Currency', " & _
                                "(DND_UNIT_COST * DND_QTY) + " & _
                                "((DND_UNIT_COST * DND_QTY) * IF(IFNULL(TAX_PERC, '') = 'N/A' OR IFNULL(TAX_PERC, '') = '', 0, TAX_PERC) / 100) AS 'Amount' " & _
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
                                "ORDER BY DNM_CREATED_DATE DESC "

                ElseIf ViewState("name") = "CNDetails" Then
                    'Issue 7480 - CH - 25 Mar 2015 (No.65): Remove Exchange Rate
                    .CommandText = "SELECT CNM_CN_NO AS 'Credit Note/Credit Advice Number', DATE_FORMAT(CNM_CREATED_DATE,'%d/%m/%Y') AS 'Credit Note/Credit Advice Date', " & _
                                "CMA.CM_COY_NAME AS 'Vendor Name', STATUS_DESC AS 'CN/CA Status', CNM_INV_NO AS 'Invoice Number', " & _
                                "ID_B_ITEM_CODE AS 'Item Code', ID_PRODUCT_DESC AS 'Item Description', ID_UOM AS 'UOM', CND_QTY AS 'Qty', CNM_CURRENCY_CODE AS 'Currency', " & _
                                "(CND_UNIT_COST * CND_QTY) + " & _
                                "((CND_UNIT_COST * CND_QTY) * IF(IFNULL(TAX_PERC, '') = 'N/A' OR IFNULL(TAX_PERC, '') = '', 0, TAX_PERC) / 100) AS 'Amount' " & _
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
                                "ORDER BY CNM_CREATED_DATE DESC "

                ElseIf ViewState("name") = "CNDetailsV" Then
                    'Issue 7480 - CH - 25 Mar 2015 (No.65): Remove Exchange Rate
                    .CommandText = "SELECT CNM_CN_NO AS 'Credit Note/Credit Advice Number', DATE_FORMAT(CNM_CREATED_DATE,'%d/%m/%Y') AS 'Credit Note/Credit Advice Date', " & _
                                "CMB.CM_COY_NAME AS 'Buyer Name', STATUS_DESC AS 'CN/CA Status', CNM_INV_NO AS 'Invoice Number', " & _
                                "ID_B_ITEM_CODE AS 'Item Code', ID_PRODUCT_DESC AS 'Item Description', ID_UOM AS 'UOM', CND_QTY AS 'Qty', CNM_CURRENCY_CODE AS 'Currency', " & _
                                "(CND_UNIT_COST * CND_QTY) + " & _
                                "((CND_UNIT_COST * CND_QTY) * IF(IFNULL(TAX_PERC, '') = 'N/A' OR IFNULL(TAX_PERC, '') = '', 0, TAX_PERC) / 100) AS 'Amount' " & _
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
                                "ORDER BY CNM_CREATED_DATE DESC "
                    '-------------------------------------------------
                Else
                    If ViewState("ReportType") = "Buyer" Then
                        .CommandText = "SELECT INVOICE_DETAILS.ID_INVOICE_NO AS 'Invoice Number'," _
                                    & "DATE_FORMAT(INVOICE_MSTR.IM_CREATED_ON,'%d/%m/%Y') AS 'Invoice Date', " _
                                    & "DATE_FORMAT(INVOICE_MSTR.IM_PAYMENT_DATE,'%d/%m/%Y') AS 'Due Date', " _
                                    & "INVOICE_MSTR.IM_S_COY_NAME AS 'Vendor Name', a.CDM_DEPT_NAME AS 'Department', " _
                                    & "PO_MSTR.POM_BUYER_NAME AS 'Buyer Name',  " _
                                    & "STATUS_DESC AS 'Invoice Status',PO_MSTR.POM_PO_NO AS 'PO Number',  " _
                                    & "DATE_FORMAT(PO_MSTR.POM_PO_DATE,'%d/%m/%Y') AS 'PO Date', " _
                                    & "CT_NAME AS 'Commodity Type',b.CDM_DEPT_NAME AS 'Cost Center', " _
                                    & "INVOICE_DETAILS.ID_B_CATEGORY_CODE AS 'Category Name',  " _
                                    & "INVOICE_DETAILS.ID_B_GL_CODE AS 'GL Code'," _
                                    & "IF(PO_DETAILS.POD_VENDOR_ITEM_CODE='&nbsp;','',PO_DETAILS.POD_VENDOR_ITEM_CODE) AS 'Item Code'," _
                                    & "INVOICE_DETAILS.ID_PRODUCT_DESC AS 'Item Description', " _
                                    & "INVOICE_DETAILS.ID_UOM AS 'UOM', INVOICE_DETAILS.ID_RECEIVED_QTY AS 'Qty',  " _
                                    & "COMPANY_MSTR.CM_CURRENCY_CODE AS 'Currency', " _
                                    & "FORMAT((invoice_details.ID_RECEIVED_QTY*INVOICE_DETAILS.ID_UNIT_COST),2) AS 'Amount', " _
                                    & "FORMAT(((invoice_details.ID_RECEIVED_QTY*INVOICE_DETAILS.ID_UNIT_COST)+(((invoice_details.ID_RECEIVED_QTY*INVOICE_DETAILS.ID_UNIT_COST)*IFNULL(INVOICE_DETAILS.ID_GST,0))/100)),2) AS 'GST Amount' " _
                                    & "FROM INVOICE_DETAILS " _
                                    & "INNER JOIN INVOICE_MSTR ON INVOICE_DETAILS.ID_INVOICE_NO = INVOICE_MSTR.IM_INVOICE_NO  " _
                                    & "AND INVOICE_DETAILS.ID_S_COY_ID = INVOICE_MSTR.IM_S_COY_ID   " _
                                    & "INNER JOIN PO_DETAILS ON INVOICE_DETAILS.ID_PO_LINE = PO_DETAILS.POD_PO_LINE  " _
                                    & "INNER JOIN PO_MSTR ON PO_DETAILS.POD_PO_NO = PO_MSTR.POM_PO_NO  " _
                                    & "AND PO_DETAILS.POD_COY_ID = PO_MSTR.POM_B_COY_ID  AND INVOICE_MSTR.IM_PO_INDEX = PO_MSTR.POM_PO_INDEX   " _
                                    & "AND INVOICE_MSTR.IM_S_COY_ID = PO_MSTR.POM_S_COY_ID   " _
                                    & "INNER JOIN COMPANY_MSTR ON IM_B_COY_ID = COMPANY_MSTR.CM_COY_ID   " _
                                    & "INNER JOIN status_mstr ON status_mstr.STATUS_NO=INVOICE_MSTR.IM_INVOICE_STATUS AND STATUS_TYPE='INV'   " _
                                    & "LEFT OUTER JOIN company_dept_mstr a ON a.CDM_DEPT_INDEX = po_mstr.POM_DEPT_INDEX   " _
                                    & "LEFT OUTER JOIN account_mstr ON account_mstr.AM_ACCT_INDEX=INVOICE_DETAILS.ID_ACCT_INDEX   " _
                                    & "LEFT OUTER JOIN company_dept_mstr b ON account_mstr.AM_DEPT_INDEX = b.CDM_DEPT_INDEX   " _
                                    & "LEFT JOIN PRODUCT_MSTR ON POD_PRODUCT_CODE = PM_PRODUCT_CODE   " _
                                    & "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID   " _
                                    & "WHERE (INVOICE_MSTR.IM_B_COY_ID = '" & Session("CompanyID") & "')   " _
                                    & "AND IM_CREATED_ON>= '" & strBeginDate & "' AND IM_CREATED_ON<='" & strEndDate & "' " _
                                    & "ORDER BY IM_CREATED_ON DESC"
                    Else
                        .CommandText = "SELECT INVOICE_DETAILS.ID_INVOICE_NO AS 'Invoice Number'," _
                                   & "DATE_FORMAT(INVOICE_MSTR.IM_CREATED_ON,'%d/%m/%Y') AS 'Invoice Date', " _
                                   & "DATE_FORMAT(INVOICE_MSTR.IM_PAYMENT_DATE,'%d/%m/%Y') AS 'Due Date', " _
                                   & "INVOICE_MSTR.IM_S_COY_NAME AS 'Vendor Name', a.CDM_DEPT_NAME AS 'Department', " _
                                   & "PO_MSTR.POM_BUYER_NAME AS 'Buyer Name',  " _
                                   & "STATUS_DESC AS 'Invoice Status',PO_MSTR.POM_PO_NO AS 'PO Number',  " _
                                   & "DATE_FORMAT(PO_MSTR.POM_PO_DATE,'%d/%m/%Y') AS 'PO Date', " _
                                   & "CT_NAME AS 'Commodity Type',b.CDM_DEPT_NAME AS 'Cost Center', " _
                                   & "INVOICE_DETAILS.ID_B_CATEGORY_CODE AS 'Category Name',  " _
                                   & "INVOICE_DETAILS.ID_B_GL_CODE AS 'GL Code'," _
                                   & "IF(PO_DETAILS.POD_VENDOR_ITEM_CODE='&nbsp;','',PO_DETAILS.POD_VENDOR_ITEM_CODE) AS 'Item Code'," _
                                   & "INVOICE_DETAILS.ID_PRODUCT_DESC AS 'Item Description', " _
                                   & "INVOICE_DETAILS.ID_UOM AS 'UOM', INVOICE_DETAILS.ID_RECEIVED_QTY AS 'Qty',  " _
                                   & "COMPANY_MSTR.CM_CURRENCY_CODE AS 'Currency', " _
                                   & "FORMAT((invoice_details.ID_RECEIVED_QTY*INVOICE_DETAILS.ID_UNIT_COST),2) AS 'Amount', " _
                                   & "FORMAT(((invoice_details.ID_RECEIVED_QTY*INVOICE_DETAILS.ID_UNIT_COST)+(((invoice_details.ID_RECEIVED_QTY*INVOICE_DETAILS.ID_UNIT_COST)*IFNULL(INVOICE_DETAILS.ID_GST,0))/100)),2) AS 'GST Amount' " _
                                   & "FROM INVOICE_DETAILS " _
                                   & "INNER JOIN INVOICE_MSTR ON INVOICE_DETAILS.ID_INVOICE_NO = INVOICE_MSTR.IM_INVOICE_NO  " _
                                   & "AND INVOICE_DETAILS.ID_S_COY_ID = INVOICE_MSTR.IM_S_COY_ID   " _
                                   & "INNER JOIN PO_DETAILS ON INVOICE_DETAILS.ID_PO_LINE = PO_DETAILS.POD_PO_LINE  " _
                                   & "INNER JOIN PO_MSTR ON PO_DETAILS.POD_PO_NO = PO_MSTR.POM_PO_NO  " _
                                   & "AND PO_DETAILS.POD_COY_ID = PO_MSTR.POM_B_COY_ID  AND INVOICE_MSTR.IM_PO_INDEX = PO_MSTR.POM_PO_INDEX   " _
                                   & "AND INVOICE_MSTR.IM_S_COY_ID = PO_MSTR.POM_S_COY_ID   " _
                                   & "INNER JOIN COMPANY_MSTR ON IM_B_COY_ID = COMPANY_MSTR.CM_COY_ID   " _
                                   & "INNER JOIN status_mstr ON status_mstr.STATUS_NO=INVOICE_MSTR.IM_INVOICE_STATUS AND STATUS_TYPE='INV' AND STATUS_NO IN (1,2,3,4)  " _
                                   & "LEFT OUTER JOIN company_dept_mstr a ON a.CDM_DEPT_INDEX = po_mstr.POM_DEPT_INDEX   " _
                                   & "LEFT OUTER JOIN account_mstr ON account_mstr.AM_ACCT_INDEX=INVOICE_DETAILS.ID_ACCT_INDEX   " _
                                   & "LEFT OUTER JOIN company_dept_mstr b ON account_mstr.AM_DEPT_INDEX = b.CDM_DEPT_INDEX   " _
                                   & "LEFT JOIN PRODUCT_MSTR ON POD_PRODUCT_CODE = PM_PRODUCT_CODE   " _
                                   & "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID   " _
                                   & "WHERE (INVOICE_MSTR.IM_S_COY_ID = '" & Session("CompanyID") & "')   " _
                                   & "AND IM_CREATED_ON>= '" & strBeginDate & "' AND IM_CREATED_ON<='" & strEndDate & "' " _
                                   & "ORDER BY IM_CREATED_ON DESC"
                    End If
                End If
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
			'Modified for Agora GST Stage 2 - CH - 2/2/2015
            If ViewState("name") = "DNDetails" Or ViewState("name") = "DNDetailsV" Then
                strFileName = "DN_DA_DetailsReport" & "(" & Format(dtFrom, "MMMyyyy") & ").xls"
            ElseIf ViewState("name") = "CNDetails" Or ViewState("name") = "CNDetailsV" Then
                strFileName = "CN_CA_DetailsReport" & "(" & Format(dtFrom, "MMMyyyy") & ").xls"
            Else
                strFileName = "InvDetailsReport" & "(" & Format(dtFrom, "MMMyyyy") & ").xls"
            End If
			'----------------------------------------------
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