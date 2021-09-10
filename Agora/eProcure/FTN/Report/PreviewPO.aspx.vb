Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class PreviewPO1FTN
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PreviewPO()
    End Sub

    Private Sub PreviewPO()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim objGST As New GST
        Dim blnGST As Boolean = False

        ' strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString(Trim("BCoyID")), System.AppDomain.CurrentDomain.BaseDirectory & "images\")
        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("BCoyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                '.CommandText = "SELECT   PO_MSTR.POM_PO_INDEX, PO_MSTR.POM_PO_NO, PO_MSTR.POM_B_COY_ID, PO_MSTR.POM_BUYER_ID, " _
                '            & "PO_MSTR.POM_BUYER_NAME, PO_MSTR.POM_BUYER_PHONE, PO_MSTR.POM_BUYER_FAX, " _
                '            & "PO_MSTR.POM_S_COY_ID, PO_MSTR.POM_S_COY_NAME, PO_MSTR.POM_S_ATTN, " _
                '            & "PO_MSTR.POM_S_REMARK, PO_MSTR.POM_S_ADDR_LINE1, PO_MSTR.POM_S_ADDR_LINE2, " _
                '            & "PO_MSTR.POM_S_ADDR_LINE3, PO_MSTR.POM_S_POSTCODE, PO_MSTR.POM_S_CITY, " _
                '            & "PO_MSTR.POM_S_STATE, PO_MSTR.POM_S_COUNTRY, PO_MSTR.POM_S_PHONE, PO_MSTR.POM_S_FAX, " _
                '            & "PO_MSTR.POM_S_EMAIL, PO_MSTR.POM_PO_DATE, PO_MSTR.POM_FREIGHT_TERMS, " _
                '            & "PO_MSTR.POM_PAYMENT_TERM, PO_MSTR.POM_PAYMENT_METHOD, PO_MSTR.POM_SHIPMENT_MODE, " _
                '            & "PO_MSTR.POM_SHIPMENT_TERM, PO_MSTR.POM_CURRENCY_CODE, PO_MSTR.POM_EXCHANGE_RATE, " _
                '            & "PO_MSTR.POM_PAYMENT_TERM_CODE, PO_MSTR.POM_SHIP_VIA, PO_MSTR.POM_PO_STATUS, " _
                '            & "PO_MSTR.POM_STATUS_CHANGED_BY, PO_MSTR.POM_STATUS_CHANGED_ON, " _
                '            & "PO_MSTR.POM_EXTERNAL_REMARK, PO_MSTR.POM_CREATED_BY, PO_MSTR.POM_PO_COST, " _
                '            & "PO_MSTR.POM_BILLING_METHOD, PO_MSTR.POM_PO_PREFIX, PO_MSTR.POM_B_ADDR_CODE, " _
                '            & "PO_MSTR.POM_B_ADDR_LINE1, PO_MSTR.POM_B_ADDR_LINE2, PO_MSTR.POM_B_ADDR_LINE3,  " _
                '            & "PO_MSTR.POM_B_POSTCODE, PO_MSTR.POM_B_CITY, PO_MSTR.POM_B_STATE, " _
                '            & "PO_MSTR.POM_B_COUNTRY, PO_MSTR.POM_FULFILMENT, PO_MSTR.POM_DEPT_INDEX, " _
                '            & "PO_MSTR.POM_ACCEPTED_DATE, PO_MSTR.POM_DOWNLOADED_DATE, PO_MSTR.POM_ARCHIVE_IND, " _
                '            & "PO_MSTR.POM_TERMANDCOND, PO_MSTR.POM_REFERENCE_NO, PO_MSTR.POM_EXTERNAL_IND, " _
                '            & "PO_MSTR.POM_PRINT_REMARK, PO_MSTR.POM_PRINT_CUSTOM_FIELDS, PO_DETAILS.POD_COY_ID, " _
                '            & "PO_DETAILS.POD_PO_NO, PO_DETAILS.POD_PO_LINE, PO_DETAILS.POD_PRODUCT_CODE, " _
                '            & "PO_DETAILS.POD_VENDOR_ITEM_CODE, PO_DETAILS.POD_PRODUCT_DESC, PO_DETAILS.POD_UOM, " _
                '            & "PO_DETAILS.POD_ORDERED_QTY, PO_DETAILS.POD_RECEIVED_QTY, PO_DETAILS.POD_REJECTED_QTY, " _
                '            & "PO_DETAILS.POD_DELIVERED_QTY, PO_DETAILS.POD_CANCELLED_QTY, PO_DETAILS.POD_MIN_PACK_QTY, " _
                '            & "PO_DETAILS.POD_MIN_ORDER_QTY, PO_DETAILS.POD_ETD, PO_DETAILS.POD_WARRANTY_TERMS, " _
                '            & "PO_DETAILS.POD_UNIT_COST, PO_DETAILS.POD_REMARK, PO_DETAILS.POD_GST, " _
                '            & "PO_DETAILS.POD_PR_INDEX, PO_DETAILS.POD_PR_LINE, PO_DETAILS.POD_ACCT_INDEX, " _
                '            & "PO_DETAILS.POD_PRODUCT_TYPE, PO_DETAILS.POD_B_ITEM_CODE, PO_DETAILS.POD_SOURCE, " _
                '            & "PO_DETAILS.POD_D_ADDR_CODE, PO_DETAILS.POD_D_ADDR_LINE1, PO_DETAILS.POD_D_ADDR_LINE2, " _
                '            & "PO_DETAILS.POD_D_ADDR_LINE3, PO_DETAILS.POD_D_POSTCODE, PO_DETAILS.POD_D_CITY, " _
                '            & "PO_DETAILS.POD_D_STATE, PO_DETAILS.POD_D_COUNTRY, PO_DETAILS.POD_B_CATEGORY_CODE, " _
                '            & "PO_DETAILS.POD_B_GL_CODE, COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, " _
                '            & "COMPANY_MSTR.CM_COY_TYPE, COMPANY_MSTR.CM_PARENT_COY_ID, COMPANY_MSTR.CM_ACCT_NO, " _
                '            & "COMPANY_MSTR.CM_BANK, COMPANY_MSTR.CM_BRANCH, COMPANY_MSTR.CM_ADDR_LINE1, " _
                '            & "COMPANY_MSTR.CM_ADDR_LINE2, COMPANY_MSTR.CM_ADDR_LINE3, COMPANY_MSTR.CM_POSTCODE, " _
                '            & "COMPANY_MSTR.CM_CITY, COMPANY_MSTR.CM_STATE, COMPANY_MSTR.CM_COUNTRY, " _
                '            & "COMPANY_MSTR.CM_PHONE, COMPANY_MSTR.CM_FAX, COMPANY_MSTR.CM_EMAIL, " _
                '            & "COMPANY_MSTR.CM_COY_LOGO, COMPANY_MSTR.CM_BUSINESS_REG_NO, " _
                '            & "COMPANY_MSTR.CM_TAX_REG_NO, COMPANY_MSTR.CM_PAYMENT_TERM, " _
                '            & "COMPANY_MSTR.CM_PAYMENT_METHOD, COMPANY_MSTR.CM_ACTUAL_TERMSANDCONDFILE, " _
                '            & "COMPANY_MSTR.CM_HUB_TERMSANDCONDFILE, COMPANY_MSTR.CM_PWD_DURATION, " _
                '            & "COMPANY_MSTR.CM_TAX_CALC_BY, COMPANY_MSTR.CM_CURRENCY_CODE, " _
                '            & "COMPANY_MSTR.CM_BCM_SET, COMPANY_MSTR.CM_BUDGET_FROM_DATE, " _
                '            & "COMPANY_MSTR.CM_BUDGET_TO_DATE, COMPANY_MSTR.CM_RFQ_OPTION, " _
                '            & "COMPANY_MSTR.CM_LICENCE_PACKAGE, COMPANY_MSTR.CM_LICENSE_USERS, " _
                '            & "COMPANY_MSTR.CM_SUB_START_DT, COMPANY_MSTR.CM_SUB_END_DT, " _
                '            & "COMPANY_MSTR.CM_LICENSE_PRODUCTS, COMPANY_MSTR.CM_FINDEPT_MODE, " _
                '            & "COMPANY_MSTR.CM_PRIV_LABELING, COMPANY_MSTR.CM_SKINS_ID, COMPANY_MSTR.CM_TRAINING, " _
                '            & "COMPANY_MSTR.CM_STATUS, COMPANY_MSTR.CM_DELETED, COMPANY_MSTR.CM_MOD_BY, " _
                '            & "COMPANY_MSTR.CM_MOD_DT, COMPANY_MSTR.CM_ENT_BY, COMPANY_MSTR.CM_ENT_DT, " _
                '            & "COMPANY_MSTR.CM_SKU, COMPANY_MSTR.CM_TRANS_NO, COMPANY_MSTR.CM_CONTACT, " _
                '            & "COMPANY_MSTR.CM_REPORT_USERS, COMPANY_MSTR.CM_INV_APPR, COMPANY_MSTR.CM_MULTI_PO, " _
                '            & "COMPANY_MSTR.CM_BA_CANCEL, USER_MSTR.UM_AUTO_NO, USER_MSTR.UM_USER_ID, " _
                '            & "USER_MSTR.UM_DELETED, USER_MSTR.UM_PASSWORD, USER_MSTR.UM_USER_NAME, " _
                '            & "USER_MSTR.UM_COY_ID, USER_MSTR.UM_DEPT_ID, USER_MSTR.UM_EMAIL, USER_MSTR.UM_APP_LIMIT, " _
                '            & "USER_MSTR.UM_DESIGNATION, USER_MSTR.UM_TEL_NO, USER_MSTR.UM_FAX_NO, " _
                '            & "USER_MSTR.UM_USER_SUSPEND_IND, USER_MSTR.UM_PASSWORD_LAST_CHANGED, " _
                '            & "USER_MSTR.UM_NEW_PASSWORD_IND, USER_MSTR.UM_NEXT_EXPIRE_DT, " _
                '            & "USER_MSTR.UM_LAST_LOGIN_DT, USER_MSTR.UM_QUESTION, USER_MSTR.UM_ANSWER, " _
                '            & "USER_MSTR.UM_MASS_APP, USER_MSTR.UM_STATUS, USER_MSTR.UM_MOD_BY, " _
                '            & "USER_MSTR.UM_MOD_DT, USER_MSTR.UM_ENT_BY, USER_MSTR.UM_ENT_DATE, " _
                '            & "USER_MSTR.UM_RECORD_COUNT, USER_MSTR.UM_EMAIL_CC, USER_MSTR.UM_INVOICE_APP_LIMIT, " _
                '            & "USER_MSTR.UM_INVOICE_MASS_APP, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
                '            & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS CMState, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = PO_MSTR.POM_S_STATE) AND (CODE_CATEGORY = 's') AND " _
                '            & "(CODE_VALUE = PO_MSTR.POM_S_COUNTRY)) AS SupplierAddrState, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = PO_MSTR.POM_S_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS SupplierAddrCtry, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = PO_MSTR.POM_B_STATE) AND (CODE_CATEGORY = 's') AND " _
                '            & "(CODE_VALUE = PO_MSTR.POM_B_COUNTRY)) AS BillAddrState, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = PO_MSTR.POM_B_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS BillAddrCtry, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = PO_DETAILS.POD_D_STATE) AND (CODE_CATEGORY = 's') AND " _
                '            & "(CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS DelvAddrState, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = PO_DETAILS.POD_D_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS DelvAddrCtry, " _
                '            & "(SELECT   CM_BUSINESS_REG_NO " _
                '            & "FROM      COMPANY_MSTR AS B " _
                '            & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS sUPPBUSSREGNO, " _
                '            & "(SELECT   CM_EMAIL " _
                '            & "FROM      COMPANY_MSTR AS B " _
                '            & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS SUPPEMAIL, " _
                '            & "(SELECT   CM_PHONE " _
                '            & "FROM      COMPANY_MSTR AS B " _
                '            & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS SUPPPHONE, PO_MSTR.POM_SHIP_AMT " _
                '            & "FROM      PO_MSTR INNER JOIN " _
                '            & "PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO AND  " _
                '            & "PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID INNER JOIN " _
                '            & "COMPANY_MSTR ON PO_MSTR.POM_B_COY_ID = COMPANY_MSTR.CM_COY_ID INNER JOIN " _
                '            & "USER_MSTR ON PO_MSTR.POM_BUYER_ID = USER_MSTR.UM_USER_ID AND " _
                '            & "PO_MSTR.POM_B_COY_ID = USER_MSTR.UM_COY_ID " _
                '            & "WHERE   (PO_MSTR.POM_B_COY_ID = @prmCoyID) AND (PO_MSTR.POM_PO_NO = @prmPONo)"
                .CommandText = "SELECT a.CODE_DESC AS CMState,b.CODE_DESC AS CMCtry,c.CODE_DESC AS SupplierAddrState, " _
                            & "d.CODE_DESC AS SupplierAddrCtry,e.CODE_DESC AS BillAddrState,f.CODE_DESC AS BillAddrCtry, " _
                            & "g.CODE_DESC AS DelvAddrState,h.CODE_DESC AS DelvAddrCtry, " _
                            & "COMPANY_MSTR_1.CM_BUSINESS_REG_NO AS sUPPBUSSREGNO,COMPANY_MSTR_1.CM_EMAIL AS SUPPEMAIL, " _
                            & "COMPANY_MSTR_1.CM_PHONE AS SUPPPHONE, " _
                            & "PO_MSTR.POM_PO_INDEX, PO_MSTR.POM_PO_NO, PO_MSTR.POM_B_COY_ID, PO_MSTR.POM_BUYER_ID,  " _
                            & "PO_MSTR.POM_BUYER_NAME, PO_MSTR.POM_BUYER_PHONE, PO_MSTR.POM_BUYER_FAX,  " _
                            & "PO_MSTR.POM_S_COY_ID, PO_MSTR.POM_S_COY_NAME, PO_MSTR.POM_S_ATTN,  " _
                            & "PO_MSTR.POM_S_REMARK, PO_MSTR.POM_S_ADDR_LINE1, PO_MSTR.POM_S_ADDR_LINE2,  " _
                            & "PO_MSTR.POM_S_ADDR_LINE3, PO_MSTR.POM_S_POSTCODE, PO_MSTR.POM_S_CITY,  " _
                            & "PO_MSTR.POM_S_STATE, PO_MSTR.POM_S_COUNTRY, PO_MSTR.POM_S_PHONE, PO_MSTR.POM_S_FAX, " _
                            & "PO_MSTR.POM_S_EMAIL, PO_MSTR.POM_PO_DATE, PO_MSTR.POM_FREIGHT_TERMS, " _
                            & "PO_MSTR.POM_PAYMENT_TERM, PO_MSTR.POM_PAYMENT_METHOD, PO_MSTR.POM_SHIPMENT_MODE, " _
                            & "PO_MSTR.POM_SHIPMENT_TERM, PO_MSTR.POM_CURRENCY_CODE, PO_MSTR.POM_EXCHANGE_RATE, " _
                            & "PO_MSTR.POM_PAYMENT_TERM_CODE, PO_MSTR.POM_SHIP_VIA, PO_MSTR.POM_PO_STATUS, " _
                            & "PO_MSTR.POM_STATUS_CHANGED_BY, PO_MSTR.POM_STATUS_CHANGED_ON, " _
                            & "PO_MSTR.POM_EXTERNAL_REMARK, PO_MSTR.POM_CREATED_BY, POM_CREATED_DATE,PO_MSTR.POM_PO_COST, " _
                            & "PO_MSTR.POM_BILLING_METHOD, PO_MSTR.POM_PO_PREFIX, PO_MSTR.POM_B_ADDR_CODE, " _
                            & "PO_MSTR.POM_B_ADDR_LINE1, PO_MSTR.POM_B_ADDR_LINE2, PO_MSTR.POM_B_ADDR_LINE3, " _
                            & "PO_MSTR.POM_B_POSTCODE, PO_MSTR.POM_B_CITY, PO_MSTR.POM_B_STATE, " _
                            & "PO_MSTR.POM_B_COUNTRY, PO_MSTR.POM_FULFILMENT, PO_MSTR.POM_DEPT_INDEX, " _
                            & "PO_MSTR.POM_ACCEPTED_DATE, PO_MSTR.POM_DOWNLOADED_DATE, PO_MSTR.POM_ARCHIVE_IND, " _
                            & "PO_MSTR.POM_TERMANDCOND, PO_MSTR.POM_REFERENCE_NO, PO_MSTR.POM_EXTERNAL_IND, " _
                            & "PO_MSTR.POM_PRINT_REMARK, PO_MSTR.POM_PRINT_CUSTOM_FIELDS, PO_DETAILS.POD_COY_ID, " _
                            & "PO_DETAILS.POD_PO_NO, PO_DETAILS.POD_PO_LINE, PO_DETAILS.POD_PRODUCT_CODE, " _
                            & "PO_DETAILS.POD_VENDOR_ITEM_CODE, PO_DETAILS.POD_PRODUCT_DESC, PO_DETAILS.POD_UOM, " _
                            & "PO_DETAILS.POD_ORDERED_QTY, PO_DETAILS.POD_RECEIVED_QTY, PO_DETAILS.POD_REJECTED_QTY, " _
                            & "PO_DETAILS.POD_DELIVERED_QTY, PO_DETAILS.POD_CANCELLED_QTY, PO_DETAILS.POD_MIN_PACK_QTY, " _
                            & "PO_DETAILS.POD_MIN_ORDER_QTY, PO_DETAILS.POD_ETD, PO_DETAILS.POD_WARRANTY_TERMS, " _
                            & "PO_DETAILS.POD_UNIT_COST, PO_DETAILS.POD_REMARK, PO_DETAILS.POD_GST, " _
                            & "PO_DETAILS.POD_PR_INDEX, PO_DETAILS.POD_PR_LINE, PO_DETAILS.POD_ACCT_INDEX, " _
                            & "PO_DETAILS.POD_PRODUCT_TYPE, PO_DETAILS.POD_B_ITEM_CODE, PO_DETAILS.POD_SOURCE, " _
                            & "PO_DETAILS.POD_D_ADDR_CODE, PO_DETAILS.POD_D_ADDR_LINE1, PO_DETAILS.POD_D_ADDR_LINE2, " _
                            & "PO_DETAILS.POD_D_ADDR_LINE3, PO_DETAILS.POD_D_POSTCODE, PO_DETAILS.POD_D_CITY, " _
                            & "PO_DETAILS.POD_D_STATE, PO_DETAILS.POD_D_COUNTRY, PO_DETAILS.POD_B_CATEGORY_CODE, " _
                            & "PO_DETAILS.POD_B_GL_CODE, COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, " _
                            & "COMPANY_MSTR.CM_COY_TYPE, COMPANY_MSTR.CM_PARENT_COY_ID, COMPANY_MSTR.CM_ACCT_NO, " _
                            & "COMPANY_MSTR.CM_BANK, COMPANY_MSTR.CM_BRANCH, COMPANY_MSTR.CM_ADDR_LINE1, " _
                            & "COMPANY_MSTR.CM_ADDR_LINE2, COMPANY_MSTR.CM_ADDR_LINE3, COMPANY_MSTR.CM_POSTCODE, " _
                            & "COMPANY_MSTR.CM_CITY, COMPANY_MSTR.CM_STATE, COMPANY_MSTR.CM_COUNTRY, " _
                            & "COMPANY_MSTR.CM_PHONE, COMPANY_MSTR.CM_FAX, COMPANY_MSTR.CM_EMAIL, " _
                            & "COMPANY_MSTR.CM_COY_LOGO, COMPANY_MSTR.CM_BUSINESS_REG_NO, " _
                            & "COMPANY_MSTR.CM_TAX_REG_NO, COMPANY_MSTR.CM_PAYMENT_TERM, " _
                            & "COMPANY_MSTR.CM_PAYMENT_METHOD, COMPANY_MSTR.CM_ACTUAL_TERMSANDCONDFILE, " _
                            & "COMPANY_MSTR.CM_HUB_TERMSANDCONDFILE, COMPANY_MSTR.CM_PWD_DURATION, " _
                            & "COMPANY_MSTR.CM_TAX_CALC_BY, COMPANY_MSTR.CM_CURRENCY_CODE, " _
                            & "COMPANY_MSTR.CM_BCM_SET, COMPANY_MSTR.CM_BUDGET_FROM_DATE, " _
                            & "COMPANY_MSTR.CM_BUDGET_TO_DATE, COMPANY_MSTR.CM_RFQ_OPTION, " _
                            & "COMPANY_MSTR.CM_LICENCE_PACKAGE, COMPANY_MSTR.CM_LICENSE_USERS, " _
                            & "COMPANY_MSTR.CM_SUB_START_DT, COMPANY_MSTR.CM_SUB_END_DT, " _
                            & "COMPANY_MSTR.CM_LICENSE_PRODUCTS, COMPANY_MSTR.CM_FINDEPT_MODE, " _
                            & "COMPANY_MSTR.CM_PRIV_LABELING, COMPANY_MSTR.CM_SKINS_ID, COMPANY_MSTR.CM_TRAINING, " _
                            & "COMPANY_MSTR.CM_STATUS, COMPANY_MSTR.CM_DELETED, COMPANY_MSTR.CM_MOD_BY, " _
                            & "COMPANY_MSTR.CM_MOD_DT, COMPANY_MSTR.CM_ENT_BY, COMPANY_MSTR.CM_ENT_DT, " _
                            & "COMPANY_MSTR.CM_SKU, COMPANY_MSTR.CM_TRANS_NO, COMPANY_MSTR.CM_CONTACT, " _
                            & "COMPANY_MSTR.CM_REPORT_USERS, COMPANY_MSTR.CM_INV_APPR, COMPANY_MSTR.CM_MULTI_PO, " _
                            & "COMPANY_MSTR.CM_BA_CANCEL, USER_MSTR.UM_AUTO_NO, USER_MSTR.UM_USER_ID, " _
                            & "USER_MSTR.UM_DELETED, USER_MSTR.UM_PASSWORD, USER_MSTR.UM_USER_NAME, " _
                            & "USER_MSTR.UM_COY_ID, USER_MSTR.UM_DEPT_ID, USER_MSTR.UM_EMAIL, USER_MSTR.UM_APP_LIMIT, " _
                            & "USER_MSTR.UM_DESIGNATION, USER_MSTR.UM_TEL_NO, USER_MSTR.UM_FAX_NO, " _
                            & "USER_MSTR.UM_USER_SUSPEND_IND, USER_MSTR.UM_PASSWORD_LAST_CHANGED, " _
                            & "USER_MSTR.UM_NEW_PASSWORD_IND, USER_MSTR.UM_NEXT_EXPIRE_DT, " _
                            & "USER_MSTR.UM_LAST_LOGIN_DT, USER_MSTR.UM_QUESTION, USER_MSTR.UM_ANSWER, " _
                            & "USER_MSTR.UM_MASS_APP, USER_MSTR.UM_STATUS, USER_MSTR.UM_MOD_BY, " _
                            & "USER_MSTR.UM_MOD_DT, USER_MSTR.UM_ENT_BY, USER_MSTR.UM_ENT_DATE, " _
                            & "USER_MSTR.UM_RECORD_COUNT, USER_MSTR.UM_EMAIL_CC, USER_MSTR.UM_INVOICE_APP_LIMIT, " _
                            & "USER_MSTR.UM_INVOICE_MASS_APP, PO_MSTR.POM_SHIP_AMT,i.CODE_DESC AS GSTRate, t.TAX_PERC " _
                            & "FROM PO_MSTR " _
                            & "INNER JOIN PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO AND PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID  " _
                            & "INNER JOIN COMPANY_MSTR ON PO_MSTR.POM_B_COY_ID = COMPANY_MSTR.CM_COY_ID  " _
                            & "INNER JOIN COMPANY_MSTR AS COMPANY_MSTR_1 ON PO_MSTR.POM_S_COY_ID = COMPANY_MSTR_1.CM_COY_ID  " _
                            & "INNER JOIN USER_MSTR ON PO_MSTR.POM_BUYER_ID = USER_MSTR.UM_USER_ID AND PO_MSTR.POM_B_COY_ID = USER_MSTR.UM_COY_ID " _
                            & "LEFT JOIN CODE_MSTR a ON (a.CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (a.CODE_CATEGORY = 's') AND (a.CODE_VALUE = COMPANY_MSTR.CM_COUNTRY) " _
                            & "LEFT JOIN CODE_MSTR b ON (b.CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (b.CODE_CATEGORY = 'ct') " _
                            & "LEFT JOIN CODE_MSTR c ON (c.CODE_ABBR = PO_MSTR.POM_S_STATE) AND (c.CODE_CATEGORY = 's') AND (c.CODE_VALUE = PO_MSTR.POM_S_COUNTRY) " _
                            & "LEFT JOIN CODE_MSTR d ON (d.CODE_ABBR = PO_MSTR.POM_S_COUNTRY) AND (d.CODE_CATEGORY = 'ct') " _
                            & "LEFT JOIN CODE_MSTR e ON (e.CODE_ABBR = PO_MSTR.POM_B_STATE) AND (e.CODE_CATEGORY = 's') AND (e.CODE_VALUE = PO_MSTR.POM_B_COUNTRY) " _
                            & "LEFT JOIN CODE_MSTR f ON (f.CODE_ABBR = PO_MSTR.POM_B_COUNTRY) AND (f.CODE_CATEGORY = 'ct') " _
                            & "LEFT JOIN CODE_MSTR g ON (g.CODE_ABBR = PO_DETAILS.POD_D_STATE) AND (g.CODE_CATEGORY = 's') AND (g.CODE_VALUE = PO_DETAILS.POD_D_COUNTRY) " _
                            & "LEFT JOIN CODE_MSTR h ON (h.CODE_ABBR = PO_DETAILS.POD_D_COUNTRY) AND (h.CODE_CATEGORY = 'ct') " _
                            & "LEFT JOIN TAX t ON (t.TAX_CODE = PO_DETAILS.POD_GST_RATE) AND (t.TAX_COUNTRY_CODE = COMPANY_MSTR.CM_COUNTRY) " _
                            & "LEFT JOIN CODE_MSTR i ON   (i.CODE_ABBR = t.TAX_CODE) AND (i.CODE_CATEGORY = 'GST') AND (i.CODE_DELETED = 'N') " _
                            & "WHERE (PO_MSTR.POM_B_COY_ID = @prmCoyID) AND (PO_MSTR.POM_PO_NO = @prmPONo)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Request.QueryString(Trim("BCoyID"))))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmPONo", Request.QueryString(Trim("PO_No"))))

            da.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                'If objGST.chkGSTCOD() And objGST.chkGST(ds.Tables(0).Rows(0)("POM_S_COY_ID")) <> "" Then
                Dim strGSTCOD As String
                strGSTCOD = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
                If objGST.chkGSTCOD() And ds.Tables(0).Rows(0)("POM_CREATED_DATE") >= CDate(strGSTCOD) Then
                    blnGST = True
                Else
                    blnGST = False
                End If
            End If

            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewPO_DataSetPreviewPO", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            'localreport.ReportPath = dispatcher.direct("Report", "PreviewPO-FTN2.rdlc", "FTNReport") 'appPath & "PO\PreviewPO-FTN.rdlc"
            If blnGST Then
                localreport.ReportPath = appPath & "FTN\Report\PreviewPO-FTN2-GST.rdlc"
            Else
                localreport.ReportPath = appPath & "FTN\Report\PreviewPO-FTN2.rdlc"
            End If

            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "par1"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            'Dim deviceInfo As String = _
            '     "<DeviceInfo>" + _
            '         "  <OutputFormat>EMF</OutputFormat>" + _
            '         "  <PageWidth>8.27in</PageWidth>" + _
            '         "  <PageHeight>11in</PageHeight>" + _
            '         "  <MarginTop>0.25in</MarginTop>" + _
            '         "  <MarginLeft>0.25in</MarginLeft>" + _
            '         "  <MarginRight>0.25in</MarginRight>" + _
            '         "  <MarginBottom>0.25in</MarginBottom>" + _
            '         "</DeviceInfo>"
            Dim deviceInfo As String = _
                "<DeviceInfo>" + _
                    "  <OutputFormat>EMF</OutputFormat>" + _
                    "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            Dim strFileName As String = "PO_" & Request.QueryString(Trim("PO_No")) & "_" & Session("CompanyID") & "_" & Session("UserId") & ".pdf"
            strFileName = Replace(strFileName, "/", "^")
            'Dim strTemp As String = dispatcher.direct("Report", "Temp", "Report")
            Dim strTemp As String = appPath & "FTN\Report\Temp\"
            If Dir(strTemp, FileAttribute.Directory) = "" Then
                MkDir(strTemp)
            End If
            Dim fs As New FileStream(Server.MapPath("Temp/" & strFileName), FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('Temp/" & strFileName & "',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
            strJScript += "</script>"
            Response.Write(strJScript)

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