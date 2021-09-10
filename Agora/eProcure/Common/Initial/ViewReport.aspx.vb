Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Partial Public Class ViewReport
    Inherits AgoraLegacy.AppBaseClass

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strFrm As String = Request.QueryString("ReportForm")

        Select Case strFrm
            Case "RFQ"
                PreviewVendorRFQ()

            Case "BuyerRFQ"
                PreviewBuyerRFQ()

            Case "QUO"
                PreviewQuotation()

            Case "PO"
                PreviewPO()

            Case "CR"
                PreviewCR()

            Case "DO"
                PreviewDO()

            Case "GRN"
                PreviewGRN()

            Case "INV"
                PreviewInvoice()
        End Select
    End Sub

    Private Sub PreviewDO()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strFreightAmount As String

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("SCoyID"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT *, (SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_S_STATE) AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS SupplierAddrState," _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_S_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS SupplierAddrCtry, " _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_B_STATE) AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS BillAddrState, " _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_B_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS BillAddrCtry," _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_DETAILS.POD_D_STATE) AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS DelvAddrState, " _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_DETAILS.POD_D_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS DelvAddrCtry," _
                        & "(SELECT CM_COY_NAME FROM COMPANY_MSTR AS b WHERE (CM_COY_ID = PO_MSTR.POM_B_COY_ID)) AS BuyerCompanyName " _
                        & "FROM PO_MSTR INNER JOIN PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO AND PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID INNER JOIN DO_MSTR ON PO_MSTR.POM_PO_INDEX = DO_MSTR.DOM_PO_INDEX INNER JOIN DO_DETAILS ON DO_MSTR.DOM_DO_NO = DO_DETAILS.DOD_DO_NO AND DO_MSTR.DOM_S_COY_ID = DO_DETAILS.DOD_S_COY_ID AND PO_DETAILS.POD_PO_LINE = DO_DETAILS.DOD_PO_LINE INNER JOIN COMPANY_MSTR ON PO_MSTR.POM_S_COY_ID = COMPANY_MSTR.CM_COY_ID " _
                        & "WHERE (PO_MSTR.POM_S_COY_ID = @prmCoyID) AND (DO_MSTR.DOM_DO_NO = @prmDONo)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Request.QueryString("SCoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmDONo", Request.QueryString("DONo")))

            da.Fill(ds)
            If IsDBNull(ds.Tables(0).Rows(0).Item("DOM_FREIGHT_AMT")) Then
                strFreightAmount = ""
            Else
                strFreightAmount = ds.Tables(0).Rows(0).Item("DOM_FREIGHT_AMT")
            End If


            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewDO_DataSetPreviewDO", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = appPath & "DO\PreveiwDO-FTN.rdlc"
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "par1"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                    Case "freightamt"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strFreightAmount)

                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            Dim deviceInfo As String =
                            "<DeviceInfo>" +
                                "  <OutputFormat>EMF</OutputFormat>" +
                                "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

            Dim fs As New FileStream(appPath & "DO\DOReport.PDF", FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('../DO/DOReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
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

    Private Sub PreviewGRN()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("BCoyID"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT   PO_MSTR.POM_PO_INDEX, PO_MSTR.POM_PO_NO, PO_MSTR.POM_B_COY_ID, PO_MSTR.POM_BUYER_ID, " _
                             & "PO_MSTR.POM_BUYER_NAME, PO_MSTR.POM_BUYER_PHONE, PO_MSTR.POM_BUYER_FAX, " _
                             & "PO_MSTR.POM_S_COY_ID, PO_MSTR.POM_S_COY_NAME, PO_MSTR.POM_S_ATTN, " _
                             & "PO_MSTR.POM_S_REMARK, PO_MSTR.POM_S_ADDR_LINE1, PO_MSTR.POM_S_ADDR_LINE2, " _
                             & "PO_MSTR.POM_S_ADDR_LINE3, PO_MSTR.POM_S_POSTCODE, PO_MSTR.POM_S_CITY, " _
                             & "PO_MSTR.POM_S_STATE, PO_MSTR.POM_S_COUNTRY, PO_MSTR.POM_S_PHONE, PO_MSTR.POM_S_FAX, " _
                             & "PO_MSTR.POM_S_EMAIL, PO_MSTR.POM_PO_DATE, PO_MSTR.POM_FREIGHT_TERMS, " _
                             & "PO_MSTR.POM_PAYMENT_TERM, PO_MSTR.POM_PAYMENT_METHOD, PO_MSTR.POM_SHIPMENT_MODE, " _
                             & "PO_MSTR.POM_SHIPMENT_TERM, PO_MSTR.POM_CURRENCY_CODE, PO_MSTR.POM_EXCHANGE_RATE, " _
                             & "PO_MSTR.POM_PAYMENT_TERM_CODE, PO_MSTR.POM_SHIP_VIA, PO_MSTR.POM_PO_STATUS, " _
                             & "PO_MSTR.POM_STATUS_CHANGED_BY, PO_MSTR.POM_STATUS_CHANGED_ON, " _
                             & "PO_MSTR.POM_EXTERNAL_REMARK, PO_MSTR.POM_CREATED_BY, PO_MSTR.POM_PO_COST, " _
                             & "PO_MSTR.POM_BILLING_METHOD, PO_MSTR.POM_PO_PREFIX, PO_MSTR.POM_B_ADDR_CODE, " _
                             & "PO_MSTR.POM_B_ADDR_LINE1, PO_MSTR.POM_B_ADDR_LINE2, PO_MSTR.POM_B_ADDR_LINE3, " _
                             & "PO_MSTR.POM_B_POSTCODE, PO_MSTR.POM_B_CITY, PO_MSTR.POM_B_STATE, " _
                             & "PO_MSTR.POM_B_COUNTRY, PO_MSTR.POM_FULFILMENT, PO_MSTR.POM_DEPT_INDEX, " _
                             & "PO_MSTR.POM_ACCEPTED_DATE, PO_MSTR.POM_DOWNLOADED_DATE, PO_MSTR.POM_ARCHIVE_IND, " _
                             & "PO_MSTR.POM_TERMANDCOND, PO_MSTR.POM_REFERENCE_NO, PO_MSTR.POM_EXTERNAL_IND, " _
                             & "PO_DETAILS.POD_COY_ID, PO_DETAILS.POD_PO_NO, PO_DETAILS.POD_PO_LINE, " _
                             & "PO_DETAILS.POD_PRODUCT_CODE, PO_DETAILS.POD_VENDOR_ITEM_CODE, " _
                             & "PO_DETAILS.POD_PRODUCT_DESC, PO_DETAILS.POD_UOM, PO_DETAILS.POD_ORDERED_QTY, " _
                             & "PO_DETAILS.POD_RECEIVED_QTY, PO_DETAILS.POD_REJECTED_QTY, PO_DETAILS.POD_DELIVERED_QTY, " _
                             & "PO_DETAILS.POD_CANCELLED_QTY, PO_DETAILS.POD_MIN_PACK_QTY, PO_DETAILS.POD_MIN_ORDER_QTY, " _
                             & "PO_DETAILS.POD_ETD, PO_DETAILS.POD_WARRANTY_TERMS, PO_DETAILS.POD_UNIT_COST, " _
                             & "PO_DETAILS.POD_REMARK, PO_DETAILS.POD_GST, PO_DETAILS.POD_PR_INDEX, " _
                             & "PO_DETAILS.POD_PR_LINE, PO_DETAILS.POD_ACCT_INDEX, PO_DETAILS.POD_PRODUCT_TYPE, " _
                             & "PO_DETAILS.POD_B_ITEM_CODE, PO_DETAILS.POD_SOURCE, PO_DETAILS.POD_D_ADDR_CODE, " _
                             & "PO_DETAILS.POD_D_ADDR_LINE1, PO_DETAILS.POD_D_ADDR_LINE2, PO_DETAILS.POD_D_ADDR_LINE3, " _
                             & "PO_DETAILS.POD_D_POSTCODE, PO_DETAILS.POD_D_CITY, PO_DETAILS.POD_D_STATE, " _
                             & "PO_DETAILS.POD_D_COUNTRY, PO_DETAILS.POD_B_CATEGORY_CODE, PO_DETAILS.POD_B_GL_CODE, " _
                             & "DO_MSTR.DOM_DO_INDEX, DO_MSTR.DOM_DO_NO, DO_MSTR.DOM_S_COY_ID, DO_MSTR.DOM_DO_DATE, " _
                             & "DO_MSTR.DOM_S_REF_NO, DO_MSTR.DOM_S_REF_DATE, DO_MSTR.DOM_PO_INDEX, " _
                             & "DO_MSTR.DOM_WAYBILL_NO, DO_MSTR.DOM_FREIGHT_CARRIER, DO_MSTR.DOM_FREIGHT_AMT, " _
                             & "DO_MSTR.DOM_DO_REMARKS, DO_MSTR.DOM_DO_STATUS, DO_MSTR.DOM_CREATED_DATE, " _
                             & "DO_MSTR.DOM_CREATED_BY, DO_MSTR.DOM_NOOFCOPY_PRINTED, DO_MSTR.DOM_DO_PREFIX, " _
                             & "DO_MSTR.DOM_D_ADDR_CODE, DO_MSTR.DOM_D_ADDR_LINE1, DO_MSTR.DOM_D_ADDR_LINE2, " _
                             & "DO_MSTR.DOM_D_ADDR_LINE3, DO_MSTR.DOM_D_POSTCODE, DO_MSTR.DOM_D_CITY, " _
                             & "DO_MSTR.DOM_D_STATE, DO_MSTR.DOM_D_COUNTRY, DO_MSTR.DOM_EXTERNAL_IND, " _
                             & "DO_MSTR.DOM_REFERENCE_NO, DO_DETAILS.DOD_S_COY_ID, DO_DETAILS.DOD_DO_NO, " _
                             & "DO_DETAILS.DOD_DO_LINE, DO_DETAILS.DOD_PO_LINE, DO_DETAILS.DOD_DO_QTY, " _
                             & "DO_DETAILS.DOD_SHIPPED_QTY, DO_DETAILS.DOD_REMARKS, GRN_MSTR.GM_GRN_INDEX, " _
                             & "GRN_MSTR.GM_GRN_NO, GRN_MSTR.GM_B_COY_ID, GRN_MSTR.GM_PO_INDEX, " _
                             & "GRN_MSTR.GM_DATE_RECEIVED, GRN_MSTR.GM_NOOFCOPY_PRINTED, GRN_MSTR.GM_DO_INDEX, " _
                             & "GRN_MSTR.GM_INVOICE_NO, GRN_MSTR.GM_GRN_PREFIX, GRN_MSTR.GM_S_COY_ID,  " _
                             & "GRN_MSTR.GM_GRN_STATUS, GRN_MSTR.GM_DOWNLOADED_DATE, GRN_MSTR.GM_GRN_LEVEL, " _
                             & "GRN_MSTR.GM_LEVEL2_USER, GRN_MSTR.GM_CREATED_BY, GRN_MSTR.GM_CREATED_DATE,  " _
                             & "GRN_DETAILS.GD_B_COY_ID, GRN_DETAILS.GD_GRN_NO, GRN_DETAILS.GD_PO_LINE,  " _
                             & "GRN_DETAILS.GD_RECEIVED_QTY, GRN_DETAILS.GD_REJECTED_QTY, GRN_DETAILS.GD_REMARKS,  " _
                             & "USER_MSTR.UM_AUTO_NO, USER_MSTR.UM_USER_ID, USER_MSTR.UM_DELETED,  " _
                             & "USER_MSTR.UM_PASSWORD, USER_MSTR.UM_USER_NAME, USER_MSTR.UM_COY_ID,  " _
                             & "USER_MSTR.UM_DEPT_ID, USER_MSTR.UM_EMAIL, USER_MSTR.UM_APP_LIMIT,  " _
                             & "USER_MSTR.UM_DESIGNATION, USER_MSTR.UM_TEL_NO, USER_MSTR.UM_FAX_NO,  " _
                             & "USER_MSTR.UM_USER_SUSPEND_IND, USER_MSTR.UM_PASSWORD_LAST_CHANGED,  " _
                             & "USER_MSTR.UM_NEW_PASSWORD_IND, USER_MSTR.UM_NEXT_EXPIRE_DT,  " _
                             & "USER_MSTR.UM_LAST_LOGIN_DT, USER_MSTR.UM_QUESTION, USER_MSTR.UM_ANSWER,  " _
                             & "USER_MSTR.UM_MASS_APP, USER_MSTR.UM_STATUS, USER_MSTR.UM_MOD_BY,  " _
                             & "USER_MSTR.UM_MOD_DT, USER_MSTR.UM_ENT_BY, USER_MSTR.UM_ENT_DATE, " _
                             & "USER_MSTR.UM_RECORD_COUNT, USER_MSTR.UM_EMAIL_CC, USER_MSTR.UM_INVOICE_APP_LIMIT, " _
                             & "USER_MSTR.UM_INVOICE_MASS_APP, COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, " _
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
                             & "COMPANY_MSTR.CM_REPORT_USERS, COMPANY_MSTR.CM_INV_APPR, COMPANY_MSTR.CM_MULTI_PO,  " _
                             & "COMPANY_MSTR.CM_BA_CANCEL, " _
                             & "(SELECT CODE_DESC " _
                             & "FROM CODE_MSTR AS a " _
                             & "WHERE (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
                             & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS CMState, " _
                             & "(SELECT CODE_DESC " _
                             & "FROM CODE_MSTR AS a " _
                             & "WHERE (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
                             & "(SELECT CODE_DESC " _
                             & "FROM CODE_MSTR AS a " _
                             & "WHERE (CODE_ABBR = DO_MSTR.DOM_D_STATE) AND (CODE_CATEGORY = 's') AND " _
                             & "(CODE_VALUE = DO_MSTR.DOM_D_COUNTRY)) AS DelvAddrState, " _
                             & "(SELECT CODE_DESC " _
                             & "FROM  CODE_MSTR AS a " _
                             & "WHERE (CODE_ABBR = DO_MSTR.DOM_D_COUNTRY) AND (CODE_CATEGORY = 'ct')) " _
                             & "AS DelvAddrCtry " _
                             & "FROM PO_MSTR INNER JOIN " _
                             & "PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO AND " _
                             & "PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID INNER JOIN " _
                             & "DO_MSTR ON PO_MSTR.POM_PO_INDEX = DO_MSTR.DOM_PO_INDEX INNER JOIN " _
                             & "DO_DETAILS ON DO_MSTR.DOM_DO_NO = DO_DETAILS.DOD_DO_NO AND  " _
                             & "DO_MSTR.DOM_S_COY_ID = DO_DETAILS.DOD_S_COY_ID AND  " _
                             & "PO_DETAILS.POD_PO_LINE = DO_DETAILS.DOD_PO_LINE INNER JOIN " _
                             & "GRN_MSTR ON PO_MSTR.POM_PO_INDEX = GRN_MSTR.GM_PO_INDEX AND  " _
                             & "DO_MSTR.DOM_DO_INDEX = GRN_MSTR.GM_DO_INDEX INNER JOIN " _
                             & "GRN_DETAILS ON GRN_MSTR.GM_GRN_NO = GRN_DETAILS.GD_GRN_NO AND  " _
                             & "GRN_MSTR.GM_B_COY_ID = GRN_DETAILS.GD_B_COY_ID AND  " _
                             & "DO_DETAILS.DOD_PO_LINE = GRN_DETAILS.GD_PO_LINE INNER JOIN " _
                             & "USER_MSTR ON GRN_MSTR.GM_CREATED_BY = USER_MSTR.UM_USER_ID " _
                             & " AND GRN_MSTR.GM_B_COY_ID = user_mstr.UM_COY_ID INNER JOIN " _
                             & "COMPANY_MSTR ON PO_MSTR.POM_B_COY_ID = COMPANY_MSTR.CM_COY_ID " _
                             & "WHERE (PO_MSTR.POM_PO_NO =@prmPONo) AND (GRN_MSTR.GM_GRN_NO = @prmGRN) AND " _
                             & "(DO_MSTR.DOM_DO_NO = @prmDONo)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmPONo", Request.QueryString("PONo")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmGRN", Request.QueryString("GRNNo")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmDONo", Request.QueryString("DONo")))

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewGRN_DataSetPreviewGRN", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = appPath & "GRN\PreviewGRN-FTN.rdlc"
            localreport.EnableExternalImages = True

            ' If strImgSrc <> "" Then
            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "grn_logo"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                    Case Else
                End Select
            Next
            localreport.SetParameters(par)

            localreport.Refresh()

            Dim deviceInfo As String =
                            "<DeviceInfo>" +
                                "  <OutputFormat>EMF</OutputFormat>" +
                                "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

            Dim fs As New FileStream(appPath & "GRN\GRNReport.PDF", FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('../GRN/GRNReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
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

    Private Sub PreviewInvoice()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("SCoyID"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT   INVOICE_DETAILS.ID_INVOICE_NO, INVOICE_DETAILS.ID_S_COY_ID, INVOICE_DETAILS.ID_INVOICE_LINE, " _
                            & "INVOICE_DETAILS.ID_PO_LINE, INVOICE_DETAILS.ID_PRODUCT_DESC, INVOICE_DETAILS.ID_B_ITEM_CODE, " _
                            & "INVOICE_DETAILS.ID_UOM, INVOICE_DETAILS.ID_GST, INVOICE_DETAILS.ID_RECEIVED_QTY, " _
                            & "INVOICE_DETAILS.ID_UNIT_COST, INVOICE_DETAILS.ID_WARRANTY_TERMS, " _
                            & "INVOICE_DETAILS.ID_ACCT_INDEX, INVOICE_DETAILS.ID_B_CATEGORY_CODE, " _
                            & "INVOICE_DETAILS.ID_B_GL_CODE, INVOICE_MSTR.IM_INVOICE_INDEX, INVOICE_MSTR.IM_INVOICE_NO, " _
                            & "INVOICE_MSTR.IM_S_COY_ID, INVOICE_MSTR.IM_S_COY_NAME, INVOICE_MSTR.IM_PO_INDEX, " _
                            & "INVOICE_MSTR.IM_B_COY_ID, INVOICE_MSTR.IM_PAYMENT_DATE, INVOICE_MSTR.IM_REMARK, " _
                            & "INVOICE_MSTR.IM_CREATED_BY, INVOICE_MSTR.IM_CREATED_ON, INVOICE_MSTR.IM_INVOICE_STATUS, " _
                            & "INVOICE_MSTR.IM_PAYMENT_NO, INVOICE_MSTR.IM_YOUR_REF, INVOICE_MSTR.IM_OUR_REF, " _
                            & "INVOICE_MSTR.IM_INVOICE_PREFIX, INVOICE_MSTR.IM_SUBMITTEDBY_FO, " _
                            & "INVOICE_MSTR.IM_EXCHANGE_RATE, INVOICE_MSTR.IM_FINANCE_REMARKS, INVOICE_MSTR.IM_PRINTED, " _
                            & "INVOICE_MSTR.IM_FOLDER, INVOICE_MSTR.IM_FM_APPROVED_DATE, " _
                            & "INVOICE_MSTR.IM_DOWNLOADED_DATE, INVOICE_MSTR.IM_EXTERNAL_IND, " _
                            & "INVOICE_MSTR.IM_REFERENCE_NO, INVOICE_MSTR.IM_INVOICE_TOTAL, " _
                            & "INVOICE_MSTR.IM_PAYMENT_TERM, INVOICE_MSTR.IM_STATUS_CHANGED_BY, " _
                            & "INVOICE_MSTR.IM_STATUS_CHANGED_ON, PO_DETAILS.POD_COY_ID, PO_DETAILS.POD_PO_NO, " _
                            & "PO_DETAILS.POD_PO_LINE, PO_DETAILS.POD_PRODUCT_CODE, PO_DETAILS.POD_VENDOR_ITEM_CODE, " _
                            & "PO_DETAILS.POD_PRODUCT_DESC, PO_DETAILS.POD_UOM, PO_DETAILS.POD_ORDERED_QTY, " _
                            & "PO_DETAILS.POD_RECEIVED_QTY, PO_DETAILS.POD_REJECTED_QTY, PO_DETAILS.POD_DELIVERED_QTY, " _
                            & "PO_DETAILS.POD_CANCELLED_QTY, PO_DETAILS.POD_MIN_PACK_QTY, PO_DETAILS.POD_MIN_ORDER_QTY, " _
                            & "PO_DETAILS.POD_ETD, PO_DETAILS.POD_WARRANTY_TERMS, PO_DETAILS.POD_UNIT_COST, " _
                            & "PO_DETAILS.POD_REMARK, PO_DETAILS.POD_GST, PO_DETAILS.POD_PR_INDEX, " _
                            & "PO_DETAILS.POD_PR_LINE, PO_DETAILS.POD_ACCT_INDEX, PO_DETAILS.POD_PRODUCT_TYPE, " _
                            & "PO_DETAILS.POD_B_ITEM_CODE, PO_DETAILS.POD_SOURCE, PO_DETAILS.POD_D_ADDR_CODE, " _
                            & "PO_DETAILS.POD_D_ADDR_LINE1, PO_DETAILS.POD_D_ADDR_LINE2, PO_DETAILS.POD_D_ADDR_LINE3, " _
                            & "PO_DETAILS.POD_D_POSTCODE, PO_DETAILS.POD_D_CITY, PO_DETAILS.POD_D_STATE, " _
                            & "PO_DETAILS.POD_D_COUNTRY, PO_DETAILS.POD_B_CATEGORY_CODE, PO_DETAILS.POD_B_GL_CODE, " _
                            & "PO_MSTR.POM_PO_INDEX, PO_MSTR.POM_PO_NO, PO_MSTR.POM_B_COY_ID, PO_MSTR.POM_BUYER_ID, " _
                            & "PO_MSTR.POM_BUYER_NAME, PO_MSTR.POM_BUYER_PHONE, PO_MSTR.POM_BUYER_FAX, " _
                            & "PO_MSTR.POM_S_COY_ID, PO_MSTR.POM_S_COY_NAME, PO_MSTR.POM_S_ATTN, " _
                            & "PO_MSTR.POM_S_REMARK, PO_MSTR.POM_S_ADDR_LINE1, PO_MSTR.POM_S_ADDR_LINE2, " _
                            & "PO_MSTR.POM_S_ADDR_LINE3, PO_MSTR.POM_S_POSTCODE, PO_MSTR.POM_S_CITY, " _
                            & "PO_MSTR.POM_S_STATE, PO_MSTR.POM_S_COUNTRY, PO_MSTR.POM_S_PHONE, PO_MSTR.POM_S_FAX, " _
                            & "PO_MSTR.POM_S_EMAIL, PO_MSTR.POM_PO_DATE, PO_MSTR.POM_FREIGHT_TERMS, " _
                            & "PO_MSTR.POM_PAYMENT_TERM, PO_MSTR.POM_PAYMENT_METHOD, PO_MSTR.POM_SHIPMENT_MODE, " _
                            & "PO_MSTR.POM_SHIPMENT_TERM, PO_MSTR.POM_CURRENCY_CODE, PO_MSTR.POM_EXCHANGE_RATE, " _
                            & "PO_MSTR.POM_PAYMENT_TERM_CODE, PO_MSTR.POM_SHIP_VIA, PO_MSTR.POM_PO_STATUS, " _
                            & "PO_MSTR.POM_STATUS_CHANGED_BY, PO_MSTR.POM_STATUS_CHANGED_ON, " _
                            & "PO_MSTR.POM_EXTERNAL_REMARK, PO_MSTR.POM_CREATED_BY, PO_MSTR.POM_PO_COST, " _
                            & "PO_MSTR.POM_BILLING_METHOD, PO_MSTR.POM_PO_PREFIX, PO_MSTR.POM_B_ADDR_CODE, " _
                            & "PO_MSTR.POM_B_ADDR_LINE1, PO_MSTR.POM_B_ADDR_LINE2, PO_MSTR.POM_B_ADDR_LINE3, " _
                            & "PO_MSTR.POM_B_POSTCODE, PO_MSTR.POM_B_CITY, PO_MSTR.POM_B_STATE, " _
                            & "PO_MSTR.POM_B_COUNTRY, PO_MSTR.POM_FULFILMENT, PO_MSTR.POM_DEPT_INDEX, " _
                            & "PO_MSTR.POM_ACCEPTED_DATE, PO_MSTR.POM_DOWNLOADED_DATE, PO_MSTR.POM_ARCHIVE_IND, " _
                            & "PO_MSTR.POM_TERMANDCOND, PO_MSTR.POM_REFERENCE_NO, PO_MSTR.POM_EXTERNAL_IND, " _
                            & "COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, COMPANY_MSTR.CM_COY_TYPE, " _
                            & "COMPANY_MSTR.CM_PARENT_COY_ID, COMPANY_MSTR.CM_ACCT_NO, COMPANY_MSTR.CM_BANK, " _
                            & "COMPANY_MSTR.CM_BRANCH, COMPANY_MSTR.CM_ADDR_LINE1, COMPANY_MSTR.CM_ADDR_LINE2, " _
                            & "COMPANY_MSTR.CM_ADDR_LINE3, COMPANY_MSTR.CM_POSTCODE, COMPANY_MSTR.CM_CITY, " _
                            & "COMPANY_MSTR.CM_STATE, COMPANY_MSTR.CM_COUNTRY, COMPANY_MSTR.CM_PHONE, " _
                            & "COMPANY_MSTR.CM_FAX, COMPANY_MSTR.CM_EMAIL, COMPANY_MSTR.CM_COY_LOGO, " _
                            & "COMPANY_MSTR.CM_BUSINESS_REG_NO, COMPANY_MSTR.CM_TAX_REG_NO, " _
                            & "COMPANY_MSTR.CM_PAYMENT_TERM, COMPANY_MSTR.CM_PAYMENT_METHOD, " _
                            & "COMPANY_MSTR.CM_ACTUAL_TERMSANDCONDFILE, COMPANY_MSTR.CM_HUB_TERMSANDCONDFILE, " _
                            & "COMPANY_MSTR.CM_PWD_DURATION, COMPANY_MSTR.CM_TAX_CALC_BY, " _
                            & "COMPANY_MSTR.CM_CURRENCY_CODE, COMPANY_MSTR.CM_BCM_SET, " _
                            & "COMPANY_MSTR.CM_BUDGET_FROM_DATE, COMPANY_MSTR.CM_BUDGET_TO_DATE, " _
                            & "COMPANY_MSTR.CM_RFQ_OPTION, COMPANY_MSTR.CM_LICENCE_PACKAGE, " _
                            & "COMPANY_MSTR.CM_LICENSE_USERS, COMPANY_MSTR.CM_SUB_START_DT, " _
                            & "COMPANY_MSTR.CM_SUB_END_DT, COMPANY_MSTR.CM_LICENSE_PRODUCTS, " _
                            & "COMPANY_MSTR.CM_FINDEPT_MODE, COMPANY_MSTR.CM_PRIV_LABELING, " _
                            & "COMPANY_MSTR.CM_SKINS_ID, COMPANY_MSTR.CM_TRAINING, COMPANY_MSTR.CM_STATUS, " _
                            & "COMPANY_MSTR.CM_DELETED, COMPANY_MSTR.CM_MOD_BY, COMPANY_MSTR.CM_MOD_DT, " _
                            & "COMPANY_MSTR.CM_ENT_BY, COMPANY_MSTR.CM_ENT_DT, COMPANY_MSTR.CM_SKU, " _
                            & "COMPANY_MSTR.CM_TRANS_NO, COMPANY_MSTR.CM_CONTACT, COMPANY_MSTR.CM_REPORT_USERS, " _
                            & "COMPANY_MSTR.CM_INV_APPR, COMPANY_MSTR.CM_MULTI_PO, COMPANY_MSTR.CM_BA_CANCEL, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_B_STATE) AND (CODE_CATEGORY = 's') AND " _
                            & "(CODE_VALUE = PO_MSTR.POM_B_COUNTRY)) AS BillAddrState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_B_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS BillAddrCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_S_STATE) AND (CODE_CATEGORY = 's') AND " _
                            & "(CODE_VALUE = PO_MSTR.POM_S_COUNTRY)) AS SupplierAddrState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_S_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS SupplierAddrCtry, " _
                            & "(SELECT   CM_COY_NAME " _
                            & "FROM      COMPANY_MSTR AS b " _
                            & "WHERE   (CM_COY_ID = PO_MSTR.POM_B_COY_ID)) AS BuyerCompanyName, " _
                            & "PO_MSTR.POM_PRINT_REMARK, PO_MSTR.POM_PRINT_CUSTOM_FIELDS, PO_MSTR.POM_SHIP_AMT, " _
                            & "INVOICE_MSTR.IM_SHIP_AMT " _
                            & "FROM      INVOICE_DETAILS INNER JOIN " _
                            & "INVOICE_MSTR ON INVOICE_DETAILS.ID_INVOICE_NO = INVOICE_MSTR.IM_INVOICE_NO AND " _
                            & "INVOICE_DETAILS.ID_S_COY_ID = INVOICE_MSTR.IM_S_COY_ID " _
                            & "INNER JOIN PO_DETAILS ON INVOICE_DETAILS.ID_PO_LINE = PO_DETAILS.POD_PO_LINE " _
                            & "INNER JOIN PO_MSTR ON PO_DETAILS.POD_PO_NO = PO_MSTR.POM_PO_NO AND " _
                            & "PO_DETAILS.POD_COY_ID = PO_MSTR.POM_B_COY_ID AND " _
                            & "INVOICE_MSTR.IM_PO_INDEX = PO_MSTR.POM_PO_INDEX AND " _
                            & "INVOICE_MSTR.IM_S_COY_ID = PO_MSTR.POM_S_COY_ID " _
                            & "INNER JOIN COMPANY_MSTR ON PO_MSTR.POM_S_COY_ID = COMPANY_MSTR.CM_COY_ID " _
                            & "WHERE   (INVOICE_MSTR.IM_S_COY_ID = @prmSCoyID) AND (INVOICE_MSTR.IM_B_COY_ID = @prmBCoyID) AND " _
                            & "(INVOICE_MSTR.IM_INVOICE_NO = @prmInvNo)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmSCoyID", Request.QueryString("SCoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmBCoyID", Request.QueryString("BCoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmInvNo", Request.QueryString("INVNO")))

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("InvoiceDocument_FTN_DataSetPreviewInvoice", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = appPath & "Invoice\PreviewINVOICE-FTN.rdlc"
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "par1"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            Dim deviceInfo As String =
                            "<DeviceInfo>" +
                                "  <OutputFormat>EMF</OutputFormat>" +
                                "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

            Dim fs As New FileStream(appPath & "Invoice\InvReport.PDF", FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('../Invoice/InvReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
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

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("BCoyID"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT   PO_MSTR.POM_PO_INDEX, PO_MSTR.POM_PO_NO, PO_MSTR.POM_B_COY_ID, PO_MSTR.POM_BUYER_ID, " _
                            & "PO_MSTR.POM_BUYER_NAME, PO_MSTR.POM_BUYER_PHONE, PO_MSTR.POM_BUYER_FAX, " _
                            & "PO_MSTR.POM_S_COY_ID, PO_MSTR.POM_S_COY_NAME, PO_MSTR.POM_S_ATTN, " _
                            & "PO_MSTR.POM_S_REMARK, PO_MSTR.POM_S_ADDR_LINE1, PO_MSTR.POM_S_ADDR_LINE2, " _
                            & "PO_MSTR.POM_S_ADDR_LINE3, PO_MSTR.POM_S_POSTCODE, PO_MSTR.POM_S_CITY, " _
                            & "PO_MSTR.POM_S_STATE, PO_MSTR.POM_S_COUNTRY, PO_MSTR.POM_S_PHONE, PO_MSTR.POM_S_FAX, " _
                            & "PO_MSTR.POM_S_EMAIL, PO_MSTR.POM_PO_DATE, PO_MSTR.POM_FREIGHT_TERMS, " _
                            & "PO_MSTR.POM_PAYMENT_TERM, PO_MSTR.POM_PAYMENT_METHOD, PO_MSTR.POM_SHIPMENT_MODE, " _
                            & "PO_MSTR.POM_SHIPMENT_TERM, PO_MSTR.POM_CURRENCY_CODE, PO_MSTR.POM_EXCHANGE_RATE, " _
                            & "PO_MSTR.POM_PAYMENT_TERM_CODE, PO_MSTR.POM_SHIP_VIA, PO_MSTR.POM_PO_STATUS, " _
                            & "PO_MSTR.POM_STATUS_CHANGED_BY, PO_MSTR.POM_STATUS_CHANGED_ON, " _
                            & "PO_MSTR.POM_EXTERNAL_REMARK, PO_MSTR.POM_CREATED_BY, PO_MSTR.POM_PO_COST, " _
                            & "PO_MSTR.POM_BILLING_METHOD, PO_MSTR.POM_PO_PREFIX, PO_MSTR.POM_B_ADDR_CODE, " _
                            & "PO_MSTR.POM_B_ADDR_LINE1, PO_MSTR.POM_B_ADDR_LINE2, PO_MSTR.POM_B_ADDR_LINE3,  " _
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
                            & "USER_MSTR.UM_INVOICE_MASS_APP, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
                            & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS CMState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_S_STATE) AND (CODE_CATEGORY = 's') AND " _
                            & "(CODE_VALUE = PO_MSTR.POM_S_COUNTRY)) AS SupplierAddrState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_S_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS SupplierAddrCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_B_STATE) AND (CODE_CATEGORY = 's') AND " _
                            & "(CODE_VALUE = PO_MSTR.POM_B_COUNTRY)) AS BillAddrState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_B_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS BillAddrCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_DETAILS.POD_D_STATE) AND (CODE_CATEGORY = 's') AND " _
                            & "(CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS DelvAddrState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_DETAILS.POD_D_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS DelvAddrCtry, " _
                            & "(SELECT   CM_BUSINESS_REG_NO " _
                            & "FROM      COMPANY_MSTR AS B " _
                            & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS sUPPBUSSREGNO, " _
                            & "(SELECT   CM_EMAIL " _
                            & "FROM      COMPANY_MSTR AS B " _
                            & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS SUPPEMAIL, " _
                            & "(SELECT   CM_PHONE " _
                            & "FROM      COMPANY_MSTR AS B " _
                            & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS SUPPPHONE, PO_MSTR.POM_SHIP_AMT " _
                            & "FROM      PO_MSTR INNER JOIN " _
                            & "PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO AND  " _
                            & "PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID INNER JOIN " _
                            & "COMPANY_MSTR ON PO_MSTR.POM_B_COY_ID = COMPANY_MSTR.CM_COY_ID INNER JOIN " _
                            & "USER_MSTR ON PO_MSTR.POM_BUYER_ID = USER_MSTR.UM_USER_ID AND " _
                            & "PO_MSTR.POM_B_COY_ID = USER_MSTR.UM_COY_ID " _
                            & "WHERE   (PO_MSTR.POM_B_COY_ID = @prmCoyID) AND (PO_MSTR.POM_PO_NO = @prmPONo)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Request.QueryString("BCoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmPONo", Request.QueryString(Trim("PO_No"))))

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewPO_DataSetPreviewPO", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = appPath & "PO\PreviewPO-FTN.rdlc"
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "par1"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            Dim deviceInfo As String =
                "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

            Dim fs As New FileStream(appPath & "PO\POReport.PDF", FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('../PO/POReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
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

    Private Sub PreviewCR()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("BCoyID"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT   PO_MSTR.POM_PO_INDEX, PO_MSTR.POM_PO_NO, PO_MSTR.POM_B_COY_ID, PO_MSTR.POM_BUYER_ID, " _
                            & "PO_MSTR.POM_BUYER_NAME, PO_MSTR.POM_BUYER_PHONE, PO_MSTR.POM_BUYER_FAX,  " _
                            & "PO_MSTR.POM_S_COY_ID, PO_MSTR.POM_S_COY_NAME, PO_MSTR.POM_S_ATTN,  " _
                            & "PO_MSTR.POM_S_REMARK, PO_MSTR.POM_S_ADDR_LINE1, PO_MSTR.POM_S_ADDR_LINE2,  " _
                            & "PO_MSTR.POM_S_ADDR_LINE3, PO_MSTR.POM_S_POSTCODE, PO_MSTR.POM_S_CITY, " _
                            & "PO_MSTR.POM_S_STATE, PO_MSTR.POM_S_COUNTRY, PO_MSTR.POM_S_PHONE, PO_MSTR.POM_S_FAX, " _
                            & "PO_MSTR.POM_S_EMAIL, PO_MSTR.POM_PO_DATE, PO_MSTR.POM_FREIGHT_TERMS, " _
                            & "PO_MSTR.POM_PAYMENT_TERM, PO_MSTR.POM_PAYMENT_METHOD, PO_MSTR.POM_SHIPMENT_MODE, " _
                            & "PO_MSTR.POM_SHIPMENT_TERM, PO_MSTR.POM_CURRENCY_CODE, PO_MSTR.POM_EXCHANGE_RATE, " _
                            & "PO_MSTR.POM_PAYMENT_TERM_CODE, PO_MSTR.POM_SHIP_VIA, PO_MSTR.POM_PO_STATUS,  " _
                            & "PO_MSTR.POM_STATUS_CHANGED_BY, PO_MSTR.POM_STATUS_CHANGED_ON,  " _
                            & "PO_MSTR.POM_EXTERNAL_REMARK, PO_MSTR.POM_CREATED_BY, PO_MSTR.POM_PO_COST, " _
                            & "PO_MSTR.POM_BILLING_METHOD, PO_MSTR.POM_PO_PREFIX, PO_MSTR.POM_B_ADDR_CODE, " _
                            & "PO_MSTR.POM_B_ADDR_LINE1, PO_MSTR.POM_B_ADDR_LINE2, PO_MSTR.POM_B_ADDR_LINE3, " _
                            & "PO_MSTR.POM_B_POSTCODE, PO_MSTR.POM_B_CITY, PO_MSTR.POM_B_STATE,  " _
                            & "PO_MSTR.POM_B_COUNTRY, PO_MSTR.POM_FULFILMENT, PO_MSTR.POM_DEPT_INDEX,  " _
                            & "PO_MSTR.POM_ACCEPTED_DATE, PO_MSTR.POM_DOWNLOADED_DATE, PO_MSTR.POM_ARCHIVE_IND, " _
                            & "PO_MSTR.POM_TERMANDCOND, PO_MSTR.POM_REFERENCE_NO, PO_MSTR.POM_EXTERNAL_IND,  " _
                            & "PO_MSTR.POM_PRINT_CUSTOM_FIELDS, PO_MSTR.POM_PRINT_REMARK, PO_DETAILS.POD_COY_ID, " _
                            & "PO_DETAILS.POD_PO_NO, PO_DETAILS.POD_PO_LINE, PO_DETAILS.POD_PRODUCT_CODE,  " _
                            & "PO_DETAILS.POD_VENDOR_ITEM_CODE, PO_DETAILS.POD_PRODUCT_DESC, PO_DETAILS.POD_UOM, " _
                            & "PO_DETAILS.POD_ORDERED_QTY, PO_DETAILS.POD_RECEIVED_QTY, PO_DETAILS.POD_REJECTED_QTY,  " _
                            & "PO_DETAILS.POD_DELIVERED_QTY, PO_DETAILS.POD_CANCELLED_QTY, PO_DETAILS.POD_MIN_PACK_QTY, " _
                            & "PO_DETAILS.POD_MIN_ORDER_QTY, PO_DETAILS.POD_ETD, PO_DETAILS.POD_WARRANTY_TERMS,  " _
                            & "PO_DETAILS.POD_UNIT_COST, PO_DETAILS.POD_REMARK, PO_DETAILS.POD_GST,  " _
                            & "PO_DETAILS.POD_PR_INDEX, PO_DETAILS.POD_PR_LINE, PO_DETAILS.POD_ACCT_INDEX,  " _
                            & "PO_DETAILS.POD_PRODUCT_TYPE, PO_DETAILS.POD_B_ITEM_CODE, PO_DETAILS.POD_SOURCE, " _
                            & "PO_DETAILS.POD_D_ADDR_CODE, PO_DETAILS.POD_D_ADDR_LINE1, PO_DETAILS.POD_D_ADDR_LINE2,  " _
                            & "PO_DETAILS.POD_D_ADDR_LINE3, PO_DETAILS.POD_D_POSTCODE, PO_DETAILS.POD_D_CITY, " _
                            & "PO_DETAILS.POD_D_STATE, PO_DETAILS.POD_D_COUNTRY, PO_DETAILS.POD_B_CATEGORY_CODE,  " _
                            & "PO_DETAILS.POD_B_GL_CODE, COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, " _
                            & "COMPANY_MSTR.CM_COY_TYPE, COMPANY_MSTR.CM_PARENT_COY_ID, COMPANY_MSTR.CM_ACCT_NO,  " _
                            & "COMPANY_MSTR.CM_BANK, COMPANY_MSTR.CM_BRANCH, COMPANY_MSTR.CM_ADDR_LINE1, " _
                            & "COMPANY_MSTR.CM_ADDR_LINE2, COMPANY_MSTR.CM_ADDR_LINE3, COMPANY_MSTR.CM_POSTCODE,  " _
                            & "COMPANY_MSTR.CM_CITY, COMPANY_MSTR.CM_STATE, COMPANY_MSTR.CM_COUNTRY, " _
                            & "COMPANY_MSTR.CM_PHONE, COMPANY_MSTR.CM_FAX, COMPANY_MSTR.CM_EMAIL, " _
                            & "COMPANY_MSTR.CM_COY_LOGO, COMPANY_MSTR.CM_BUSINESS_REG_NO, " _
                            & "COMPANY_MSTR.CM_TAX_REG_NO, COMPANY_MSTR.CM_PAYMENT_TERM,  " _
                            & "COMPANY_MSTR.CM_PAYMENT_METHOD, COMPANY_MSTR.CM_ACTUAL_TERMSANDCONDFILE, " _
                            & "COMPANY_MSTR.CM_HUB_TERMSANDCONDFILE, COMPANY_MSTR.CM_PWD_DURATION,  " _
                            & "COMPANY_MSTR.CM_TAX_CALC_BY, COMPANY_MSTR.CM_CURRENCY_CODE, " _
                            & "COMPANY_MSTR.CM_BCM_SET, COMPANY_MSTR.CM_BUDGET_FROM_DATE, " _
                            & "COMPANY_MSTR.CM_BUDGET_TO_DATE, COMPANY_MSTR.CM_RFQ_OPTION, " _
                            & "COMPANY_MSTR.CM_LICENCE_PACKAGE, COMPANY_MSTR.CM_LICENSE_USERS,  " _
                            & "COMPANY_MSTR.CM_SUB_START_DT, COMPANY_MSTR.CM_SUB_END_DT,  " _
                            & "COMPANY_MSTR.CM_LICENSE_PRODUCTS, COMPANY_MSTR.CM_FINDEPT_MODE,  " _
                            & "COMPANY_MSTR.CM_PRIV_LABELING, COMPANY_MSTR.CM_SKINS_ID, COMPANY_MSTR.CM_TRAINING,  " _
                            & "COMPANY_MSTR.CM_STATUS, COMPANY_MSTR.CM_DELETED, COMPANY_MSTR.CM_MOD_BY,  " _
                            & "COMPANY_MSTR.CM_MOD_DT, COMPANY_MSTR.CM_ENT_BY, COMPANY_MSTR.CM_ENT_DT, " _
                            & "COMPANY_MSTR.CM_SKU, COMPANY_MSTR.CM_TRANS_NO, COMPANY_MSTR.CM_CONTACT,  " _
                            & "COMPANY_MSTR.CM_REPORT_USERS, COMPANY_MSTR.CM_INV_APPR, COMPANY_MSTR.CM_MULTI_PO,  " _
                            & "COMPANY_MSTR.CM_BA_CANCEL, PO_CR_MSTR.PCM_CR_NO, PO_CR_MSTR.PCM_B_COY_ID,  " _
                            & "PO_CR_MSTR.PCM_S_COY_ID, PO_CR_MSTR.PCM_PO_INDEX, PO_CR_MSTR.PCM_CR_STATUS,  " _
                            & "PO_CR_MSTR.PCM_REQ_BY, PO_CR_MSTR.PCM_REQ_DATE, PO_CR_MSTR.PCM_CR_REMARKS,  " _
                            & "PO_CR_DETAILS.PCD_CR_NO, PO_CR_DETAILS.PCD_COY_ID, PO_CR_DETAILS.PCD_PO_LINE,  " _
                            & "PO_CR_DETAILS.PCD_CANCELLED_QTY, PO_CR_DETAILS.PCD_REMARKS, USER_MSTR.UM_AUTO_NO,  " _
                            & "USER_MSTR.UM_USER_ID, USER_MSTR.UM_DELETED, USER_MSTR.UM_PASSWORD,  " _
                            & "USER_MSTR.UM_USER_NAME, USER_MSTR.UM_COY_ID, USER_MSTR.UM_DEPT_ID,  " _
                            & "USER_MSTR.UM_EMAIL, USER_MSTR.UM_APP_LIMIT, USER_MSTR.UM_DESIGNATION,  " _
                            & "USER_MSTR.UM_TEL_NO, USER_MSTR.UM_FAX_NO, USER_MSTR.UM_USER_SUSPEND_IND,  " _
                            & "USER_MSTR.UM_PASSWORD_LAST_CHANGED, USER_MSTR.UM_NEW_PASSWORD_IND,  " _
                            & "USER_MSTR.UM_NEXT_EXPIRE_DT, USER_MSTR.UM_LAST_LOGIN_DT, USER_MSTR.UM_QUESTION,  " _
                            & "USER_MSTR.UM_ANSWER, USER_MSTR.UM_MASS_APP, USER_MSTR.UM_STATUS,  " _
                            & "USER_MSTR.UM_MOD_BY, USER_MSTR.UM_MOD_DT, USER_MSTR.UM_ENT_BY,  " _
                            & "USER_MSTR.UM_ENT_DATE, USER_MSTR.UM_RECORD_COUNT, USER_MSTR.UM_EMAIL_CC,  " _
                            & "USER_MSTR.UM_INVOICE_APP_LIMIT, USER_MSTR.UM_INVOICE_MASS_APP, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND  " _
                            & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS CMState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_S_STATE) AND (CODE_CATEGORY = 's') AND  " _
                            & "(CODE_VALUE = PO_MSTR.POM_S_COUNTRY)) AS SupplierAddrState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_S_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS SupplierAddrCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_B_STATE) AND (CODE_CATEGORY = 's') AND  " _
                            & "(CODE_VALUE = PO_MSTR.POM_B_COUNTRY)) AS BillAddrState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_B_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS BillAddrCtry, " _
                            & "(SELECT   CM_BUSINESS_REG_NO " _
                            & "FROM      COMPANY_MSTR AS B " _
                            & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS sUPPBUSSREGNO, " _
                            & "(SELECT   CM_EMAIL " _
                            & "FROM      COMPANY_MSTR AS B " _
                            & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS SUPPEMAIL, " _
                            & "(SELECT   CM_PHONE " _
                            & "FROM      COMPANY_MSTR AS B " _
                            & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS SUPPPHONE, " _
                            & "(SELECT   um_User_name FROM User_MSTR AS B WHERE   (PCM_REQ_By = UM_User_ID AND PCM_B_COY_ID = UM_coy_ID)) AS PCMCRBUYERNAME " _
                            & "FROM      PO_MSTR INNER JOIN " _
                            & "PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO AND  " _
                            & "PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID INNER JOIN " _
                            & "COMPANY_MSTR ON PO_MSTR.POM_B_COY_ID = COMPANY_MSTR.CM_COY_ID INNER JOIN " _
                            & "PO_CR_MSTR ON PO_MSTR.POM_PO_INDEX = PO_CR_MSTR.PCM_PO_INDEX AND  " _
                            & "PO_MSTR.POM_B_COY_ID = PO_CR_MSTR.PCM_B_COY_ID INNER JOIN " _
                            & "PO_CR_DETAILS ON PO_CR_MSTR.PCM_CR_NO = PO_CR_DETAILS.PCD_CR_NO AND  " _
                            & "PO_CR_MSTR.PCM_B_COY_ID = PO_CR_DETAILS.PCD_COY_ID AND  " _
                            & "PO_DETAILS.POD_PO_LINE = PO_CR_DETAILS.PCD_PO_LINE INNER JOIN " _
                            & "USER_MSTR ON PO_MSTR.POM_BUYER_ID = USER_MSTR.UM_USER_ID AND  " _
                            & "PO_MSTR.POM_B_COY_ID = USER_MSTR.UM_COY_ID " _
                            & "WHERE   (PO_MSTR.POM_B_COY_ID = @prmCoyID) AND (PO_MSTR.POM_PO_NO = @prmPONo) AND  " _
                            & "(PO_CR_MSTR.PCM_CR_NO = @prmCRNo)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Request.QueryString("BCoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmPONo", Request(Trim("PO_No"))))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCRNo", Request(Trim("cr_no"))))

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewCR_FTN_DataTable1", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = appPath & "PO\PreviewCR-FTN.rdlc"
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "par1"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            Dim deviceInfo As String =
                "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

            Dim fs As New FileStream(appPath & "PO\CRReport.PDF", FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('../PO/CRReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
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

    Private Sub PreviewBuyerRFQ()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("BCoyID"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT RFQ_MSTR.RM_RFQ_ID, RFQ_MSTR.RM_Coy_ID, RFQ_MSTR.RM_RFQ_No, RFQ_MSTR.RM_RFQ_Name, " _
                            & "RFQ_MSTR.RM_Expiry_Date, RFQ_MSTR.RM_Status, RFQ_MSTR.RM_Remark, RFQ_MSTR.RM_Created_By, " _
                            & "RFQ_MSTR.RM_Created_On, RFQ_MSTR.RM_Currency_Code, RFQ_MSTR.RM_Payment_Term, " _
                            & "RFQ_MSTR.RM_Payment_Type, RFQ_MSTR.RM_Shipment_Term, RFQ_MSTR.RM_Shipment_Mode, " _
                            & "RFQ_MSTR.RM_Prefix, RFQ_MSTR.RM_B_Display_Status, RFQ_MSTR.RM_Reqd_Quote_Validity, " _
                            & "RFQ_MSTR.RM_Contact_Person, RFQ_MSTR.RM_Contact_Number, RFQ_MSTR.RM_Email, " _
                            & "RFQ_MSTR.RM_RFQ_OPTION, RFQ_MSTR.RM_VEN_DISTR_LIST_INDEX, RFQ_DETAIL.RD_RFQ_ID, " _
                            & "RFQ_DETAIL.RD_Coy_ID, RFQ_DETAIL.RD_RFQ_Line, RFQ_DETAIL.RD_Product_Code, " _
                            & "RFQ_DETAIL.RD_Vendor_Item_Code, RFQ_DETAIL.RD_Quantity, RFQ_DETAIL.RD_Product_Desc, " _
                            & "RFQ_DETAIL.RD_UOM, RFQ_DETAIL.RD_Delivery_Lead_Time, RFQ_DETAIL.RD_Warranty_Terms, " _
                            & "RFQ_DETAIL.RD_Product_Name, RFQ_INVITED_VENLIST.RIV_RFQ_ID, RFQ_INVITED_VENLIST.RIV_S_Coy_ID, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_Coy_Name, RFQ_INVITED_VENLIST.RIV_S_Addr_Line1, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_Addr_Line2, RFQ_INVITED_VENLIST.RIV_S_Addr_Line3, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_PostCode, RFQ_INVITED_VENLIST.RIV_S_City, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_State, RFQ_INVITED_VENLIST.RIV_S_Country, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_Phone, RFQ_INVITED_VENLIST.RIV_S_Fax, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_Email, COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, " _
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
                            & "COMPANY_MSTR.CM_BA_CANCEL, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
                            & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS CMState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_INVITED_VENLIST.RIV_S_State) AND (CODE_CATEGORY = 's') AND " _
                            & "(CODE_VALUE = RFQ_INVITED_VENLIST.RIV_S_Country)) AS SupplierAddrState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_INVITED_VENLIST.RIV_S_Country) AND (CODE_CATEGORY = 'ct')) " _
                            & "AS SupplierAddrCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Payment_Term) AND (CODE_CATEGORY = 'pt')) " _
                            & "AS RFQ_PaymentTerm, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Payment_Type) AND (CODE_CATEGORY = 'pm')) " _
                            & "AS RFQ_PaymentMethod, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Shipment_Term) AND (CODE_CATEGORY = 'St')) " _
                            & "AS RFQ_ShipmentTerm, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Shipment_Mode) AND (CODE_CATEGORY = 'sm')) " _
                            & "AS RFQ_ShipmentMode, " _
                            & "(SELECT   CM_BUSINESS_REG_NO " _
                            & "FROM      COMPANY_MSTR AS B " _
                            & "WHERE   (RFQ_INVITED_VENLIST.RIV_S_Coy_ID = CM_COY_ID)) AS sUPPBUSSREGNO " _
                            & "FROM      RFQ_MSTR INNER JOIN " _
                            & "RFQ_DETAIL ON RFQ_MSTR.RM_RFQ_ID = RFQ_DETAIL.RD_RFQ_ID INNER JOIN " _
                            & "RFQ_INVITED_VENLIST ON RFQ_MSTR.RM_RFQ_ID = RFQ_INVITED_VENLIST.RIV_RFQ_ID INNER JOIN " _
                            & "COMPANY_MSTR ON RFQ_MSTR.RM_Coy_ID = COMPANY_MSTR.CM_COY_ID " _
                            & "WHERE   (RFQ_MSTR.RM_RFQ_No = @prmRFQNum) AND (RFQ_MSTR.RM_Coy_ID =@prmBCoyID)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmBCoyID", Request.QueryString("BCoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmRFQNum", Session("rfq_num")))

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewRFQ_FTN_DataTablePreviewRFQ", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = appPath & "RFQ\PreviewRFQ-FTN2.rdlc"
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "par1"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            Dim deviceInfo As String =
               "<DeviceInfo>" +
                   "  <OutputFormat>EMF</OutputFormat>" +
                   "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

            Dim fs As New FileStream(appPath & "RFQ\BuyerRFQReport.PDF", FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('../RFQ/BuyerRFQReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
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

    Private Sub PreviewVendorRFQ()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("BCoyID"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT   RFQ_MSTR.RM_RFQ_ID, RFQ_MSTR.RM_Coy_ID, RFQ_MSTR.RM_RFQ_No, RFQ_MSTR.RM_RFQ_Name, " _
                            & "RFQ_MSTR.RM_Expiry_Date, RFQ_MSTR.RM_Status, RFQ_MSTR.RM_Remark, RFQ_MSTR.RM_Created_By, " _
                            & "RFQ_MSTR.RM_Created_On, RFQ_MSTR.RM_Currency_Code, RFQ_MSTR.RM_Payment_Term, " _
                            & "RFQ_MSTR.RM_Payment_Type, RFQ_MSTR.RM_Shipment_Term, RFQ_MSTR.RM_Shipment_Mode, " _
                            & "RFQ_MSTR.RM_Prefix, RFQ_MSTR.RM_B_Display_Status, RFQ_MSTR.RM_Reqd_Quote_Validity, " _
                            & "RFQ_MSTR.RM_Contact_Person, RFQ_MSTR.RM_Contact_Number, RFQ_MSTR.RM_Email, " _
                            & "RFQ_MSTR.RM_RFQ_OPTION, RFQ_MSTR.RM_VEN_DISTR_LIST_INDEX, RFQ_DETAIL.RD_RFQ_ID, " _
                            & "RFQ_DETAIL.RD_Coy_ID, RFQ_DETAIL.RD_RFQ_Line, RFQ_DETAIL.RD_Product_Code, " _
                            & "RFQ_DETAIL.RD_Vendor_Item_Code, RFQ_DETAIL.RD_Quantity, RFQ_DETAIL.RD_Product_Desc, " _
                            & "RFQ_DETAIL.RD_UOM, RFQ_DETAIL.RD_Delivery_Lead_Time, RFQ_DETAIL.RD_Warranty_Terms, " _
                            & "RFQ_DETAIL.RD_Product_Name, RFQ_INVITED_VENLIST.RIV_RFQ_ID, RFQ_INVITED_VENLIST.RIV_S_Coy_ID, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_Coy_Name, RFQ_INVITED_VENLIST.RIV_S_Addr_Line1, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_Addr_Line2, RFQ_INVITED_VENLIST.RIV_S_Addr_Line3, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_PostCode, RFQ_INVITED_VENLIST.RIV_S_City, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_State, RFQ_INVITED_VENLIST.RIV_S_Country, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_Phone, RFQ_INVITED_VENLIST.RIV_S_Fax, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_Email, COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, " _
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
                            & "COMPANY_MSTR.CM_BA_CANCEL, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
                            & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS CMState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_INVITED_VENLIST.RIV_S_State) AND (CODE_CATEGORY = 's') AND " _
                            & "(CODE_VALUE = RFQ_INVITED_VENLIST.RIV_S_Country)) AS SupplierAddrState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_INVITED_VENLIST.RIV_S_Country) AND (CODE_CATEGORY = 'ct')) " _
                            & "AS SupplierAddrCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Payment_Term) AND (CODE_CATEGORY = 'pt')) " _
                            & "AS RFQ_PaymentTerm, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Payment_Type) AND (CODE_CATEGORY = 'pm')) " _
                            & "AS RFQ_PaymentMethod, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Shipment_Term) AND (CODE_CATEGORY = 'St')) " _
                            & "AS RFQ_ShipmentTerm, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Shipment_Mode) AND (CODE_CATEGORY = 'sm')) " _
                            & "AS RFQ_ShipmentMode, " _
                            & "(SELECT   CM_BUSINESS_REG_NO " _
                            & "FROM      COMPANY_MSTR AS B " _
                            & "WHERE   (RFQ_INVITED_VENLIST.RIV_S_Coy_ID = CM_COY_ID)) AS sUPPBUSSREGNO " _
                            & "FROM      RFQ_MSTR INNER JOIN " _
                            & "RFQ_DETAIL ON RFQ_MSTR.RM_RFQ_ID = RFQ_DETAIL.RD_RFQ_ID INNER JOIN " _
                            & "RFQ_INVITED_VENLIST ON RFQ_MSTR.RM_RFQ_ID = RFQ_INVITED_VENLIST.RIV_RFQ_ID INNER JOIN " _
                            & "COMPANY_MSTR ON RFQ_MSTR.RM_Coy_ID = COMPANY_MSTR.CM_COY_ID " _
                            & "WHERE (RFQ_MSTR.RM_RFQ_No = @prmRFQNum) AND (RFQ_MSTR.RM_Coy_ID = @prmBCoyID) AND " _
                            & "(RFQ_INVITED_VENLIST.RIV_S_Coy_ID = @prmVCoyID)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmVCoyID", Request.QueryString("SCoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmBCoyID", Request.QueryString("BCoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmRFQNum", Request.QueryString("RFQ_Num")))

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewRFQ_FTN_DataTablePreviewRFQ", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = appPath & "RFQ\PreviewRFQ-FTN2.rdlc"
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "par1"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            Dim deviceInfo As String =
               "<DeviceInfo>" +
                   "  <OutputFormat>EMF</OutputFormat>" +
                   "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

            Dim fs As New FileStream(appPath & "RFQ\RFQReport.PDF", FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('../RFQ/RFQReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
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

    Private Sub PreviewQuotation()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("SCoyID"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                '.CommandText = "SELECT (SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = COMPANY_MSTR_1.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
                '            & "(CODE_VALUE = COMPANY_MSTR_1.CM_COUNTRY)) AS CMState, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = COMPANY_MSTR_1.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
                '            & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS SupplierAddrState, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) " _
                '            & "AS SupplierAddrCtry, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Pay_Term_Code) AND (CODE_CATEGORY = 'pt')) " _
                '            & "AS PaymentTerm, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Payment_Type) AND (CODE_CATEGORY = 'pm')) " _
                '            & "AS PaymentMethod, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Term) AND (CODE_CATEGORY = 'St')) " _
                '            & "AS Ship_Term, " _
                '            & "(SELECT   CODE_DESC " _
                '            & "FROM      CODE_MSTR AS a " _
                '            & "WHERE   (CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Mode) AND (CODE_CATEGORY = 'sm')) " _
                '            & "AS Ship_Mode, COMPANY_MSTR_1.CM_COY_ID AS Buyer_Coy_ID, " _
                '            & "COMPANY_MSTR_1.CM_COY_NAME AS Buyer_Coy_Name, " _
                '            & "COMPANY_MSTR_1.CM_COY_TYPE AS Buyer_Coy_Type, " _
                '            & "COMPANY_MSTR_1.CM_ADDR_LINE1 AS Buyer_Addr_Line1, " _
                '            & "COMPANY_MSTR_1.CM_ADDR_LINE2 AS Buyer_Addr_Line2, " _
                '            & "COMPANY_MSTR_1.CM_ADDR_LINE3 AS Buyer_Addr_Line3, " _
                '            & "COMPANY_MSTR_1.CM_POSTCODE AS Buyer_Postcode, COMPANY_MSTR_1.CM_CITY AS Buyer_City, " _
                '            & "COMPANY_MSTR_1.CM_STATE AS Buyer_State, COMPANY_MSTR_1.CM_COUNTRY AS Buyer_Country, " _
                '            & "COMPANY_MSTR_1.CM_PHONE AS Buyer_Phone, COMPANY_MSTR_1.CM_FAX AS Buyer_Fax, " _
                '            & "COMPANY_MSTR_1.CM_EMAIL AS Buyer_Email, " _
                '            & "COMPANY_MSTR_1.CM_BUSINESS_REG_NO AS Buyer_Business_Reg_No, " _
                '            & "COMPANY_MSTR_1.CM_STATUS AS Buyer_Coy_Status, " _
                '            & "COMPANY_MSTR_1.CM_DELETED AS Buyer_Coy_Deleted, RFQ_MSTR.RM_RFQ_ID, RFQ_MSTR.RM_Coy_ID, " _
                '            & "RFQ_MSTR.RM_RFQ_No, RFQ_MSTR.RM_RFQ_Name, RFQ_MSTR.RM_Expiry_Date, RFQ_MSTR.RM_Status, " _
                '            & "RFQ_MSTR.RM_Remark, RFQ_MSTR.RM_Created_By, RFQ_MSTR.RM_Created_On, " _
                '            & "RFQ_MSTR.RM_Currency_Code, RFQ_MSTR.RM_Payment_Term, RFQ_MSTR.RM_Payment_Type, " _
                '            & "RFQ_MSTR.RM_Shipment_Term, RFQ_MSTR.RM_Shipment_Mode, RFQ_MSTR.RM_Prefix, " _
                '            & "RFQ_MSTR.RM_B_Display_Status, RFQ_MSTR.RM_Reqd_Quote_Validity, RFQ_MSTR.RM_Contact_Person, " _
                '            & "RFQ_MSTR.RM_Contact_Number, RFQ_MSTR.RM_Email, RFQ_MSTR.RM_RFQ_OPTION, " _
                '            & "RFQ_MSTR.RM_VEN_DISTR_LIST_INDEX, RFQ_REPLIES_MSTR.RRM_RFQ_ID, " _
                '            & "RFQ_REPLIES_MSTR.RRM_V_Company_ID, RFQ_REPLIES_MSTR.RRM_Currency_Code, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Offer_Till, RFQ_REPLIES_MSTR.RRM_ETD, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Remarks, RFQ_REPLIES_MSTR.RRM_Pay_Term_Code, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Payment_Type, RFQ_REPLIES_MSTR.RRM_Ship_Mode, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Ship_Term, RFQ_REPLIES_MSTR.RRM_Created_On, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Created_By, RFQ_REPLIES_MSTR.RRM_GST, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Quot_Seq_No, RFQ_REPLIES_MSTR.RRM_Quot_Prefix, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num, RFQ_REPLIES_MSTR.RRM_Contact_Person, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Contact_Number, RFQ_REPLIES_MSTR.RRM_Email, " _
                '            & "RFQ_REPLIES_MSTR.RRM_Status, RFQ_REPLIES_MSTR.RRM_B_Display_Status, " _
                '            & "RFQ_REPLIES_MSTR.RRM_V_Display_Status, RFQ_REPLIES_MSTR.RRM_Indicator, " _
                '            & "RFQ_REPLIES_MSTR.RRM_TotalValue, RFQ_REPLIES_DETAIL.RRD_RFQ_ID, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_V_Coy_Id, RFQ_REPLIES_DETAIL.RRD_Line_No, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Product_Code, RFQ_REPLIES_DETAIL.RRD_Vendor_Item_Code, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Quantity, RFQ_REPLIES_DETAIL.RRD_Unit_Price, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_GST_Code, RFQ_REPLIES_DETAIL.RRD_GST, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_GST_Desc, RFQ_REPLIES_DETAIL.RRD_Product_Desc, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_UOM, RFQ_REPLIES_DETAIL.RRD_Delivery_Lead_Time, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Warranty_Terms, RFQ_REPLIES_DETAIL.RRD_Min_Pack_Qty, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Min_Order_Qty, RFQ_REPLIES_DETAIL.RRD_Remarks, " _
                '            & "RFQ_REPLIES_DETAIL.RRD_Tolerance, COMPANY_MSTR.CM_COY_ID AS Supp_Coy_ID, " _
                '            & "COMPANY_MSTR.CM_COY_NAME AS Supp_Coy_Name, " _
                '            & "COMPANY_MSTR.CM_ADDR_LINE1 AS Supp_Addr_Line1, " _
                '            & "COMPANY_MSTR.CM_ADDR_LINE2 AS Supp_Addr_Line2, " _
                '            & "COMPANY_MSTR.CM_ADDR_LINE3 AS Supp_Addr_Line3, " _
                '            & "COMPANY_MSTR.CM_POSTCODE AS Supp_Coy_Postcode, COMPANY_MSTR.CM_CITY AS Supp_Coy_City, " _
                '            & "COMPANY_MSTR.CM_STATE AS Supp_Coy_State, COMPANY_MSTR.CM_COUNTRY AS Supp_Coy_Country, " _
                '            & "COMPANY_MSTR.CM_PHONE AS Supp_Coy_Phone, COMPANY_MSTR.CM_FAX AS Supp_Coy_Fax, " _
                '            & "COMPANY_MSTR.CM_EMAIL AS Supp_Coy_Email, " _
                '            & "COMPANY_MSTR.CM_BUSINESS_REG_NO AS Supp_Coy_BusinessRegNo " _
                '            & "FROM      RFQ_MSTR INNER JOIN " _
                '            & "RFQ_REPLIES_MSTR ON RFQ_MSTR.RM_RFQ_ID = RFQ_REPLIES_MSTR.RRM_RFQ_ID INNER JOIN " _
                '            & "RFQ_REPLIES_DETAIL ON RFQ_REPLIES_MSTR.RRM_RFQ_ID = RFQ_REPLIES_DETAIL.RRD_RFQ_ID AND " _
                '            & "RFQ_REPLIES_MSTR.RRM_V_Company_ID = RFQ_REPLIES_DETAIL.RRD_V_Coy_Id INNER JOIN " _
                '            & "COMPANY_MSTR ON RFQ_REPLIES_MSTR.RRM_V_Company_ID = COMPANY_MSTR.CM_COY_ID INNER JOIN " _
                '            & "COMPANY_MSTR AS COMPANY_MSTR_1 ON " _
                '            & "RFQ_MSTR.RM_Coy_ID = COMPANY_MSTR_1.CM_COY_ID " _
                '            & "WHERE   (RFQ_REPLIES_MSTR.RRM_V_Company_ID = @prmVCoyID) AND (RFQ_MSTR.RM_Coy_ID = @prmBCoyID) AND " _
                '            & "(RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num = @prmQuoNum) AND " _
                '            & "(RFQ_MSTR.RM_RFQ_No = @prmRFQNum)"
                .CommandText = "SELECT a.CODE_DESC AS CMState,b.CODE_DESC AS CMCtry,c.CODE_DESC AS SupplierAddrState,d.CODE_DESC AS SupplierAddrCtry," _
                       & "e.CODE_DESC AS PaymentTerm,f.CODE_DESC AS PaymentMethod,g.CODE_DESC AS Ship_Term,h.CODE_DESC AS Ship_Mode," _
                       & "COMPANY_MSTR_1.CM_COY_ID AS Buyer_Coy_ID, COMPANY_MSTR_1.CM_COY_NAME AS Buyer_Coy_Name, " _
                       & "COMPANY_MSTR_1.CM_COY_TYPE AS Buyer_Coy_Type, COMPANY_MSTR_1.CM_ADDR_LINE1 AS Buyer_Addr_Line1, " _
                       & "COMPANY_MSTR_1.CM_ADDR_LINE2 AS Buyer_Addr_Line2, COMPANY_MSTR_1.CM_ADDR_LINE3 AS Buyer_Addr_Line3, " _
                       & "COMPANY_MSTR_1.CM_POSTCODE AS Buyer_Postcode, COMPANY_MSTR_1.CM_CITY AS Buyer_City, " _
                       & "COMPANY_MSTR_1.CM_STATE AS Buyer_State, COMPANY_MSTR_1.CM_COUNTRY AS Buyer_Country, " _
                       & "COMPANY_MSTR_1.CM_PHONE AS Buyer_Phone, COMPANY_MSTR_1.CM_FAX AS Buyer_Fax, " _
                       & "COMPANY_MSTR_1.CM_EMAIL AS Buyer_Email, COMPANY_MSTR_1.CM_BUSINESS_REG_NO AS Buyer_Business_Reg_No, " _
                       & "COMPANY_MSTR_1.CM_STATUS AS Buyer_Coy_Status, COMPANY_MSTR_1.CM_DELETED AS Buyer_Coy_Deleted, " _
                       & "RFQ_MSTR.RM_RFQ_ID, RFQ_MSTR.RM_Coy_ID, RFQ_MSTR.RM_RFQ_No, RFQ_MSTR.RM_RFQ_Name, RFQ_MSTR.RM_Expiry_Date, " _
                       & "RFQ_MSTR.RM_Status, RFQ_MSTR.RM_Remark, RFQ_MSTR.RM_Created_By, RFQ_MSTR.RM_Created_On, " _
                       & "RFQ_MSTR.RM_Currency_Code, RFQ_MSTR.RM_Payment_Term, RFQ_MSTR.RM_Payment_Type, RFQ_MSTR.RM_Shipment_Term, " _
                       & "RFQ_MSTR.RM_Shipment_Mode, RFQ_MSTR.RM_Prefix, RFQ_MSTR.RM_B_Display_Status, " _
                       & "RFQ_MSTR.RM_Reqd_Quote_Validity, RFQ_MSTR.RM_Contact_Person, RFQ_MSTR.RM_Contact_Number, RFQ_MSTR.RM_Email," _
                       & "RFQ_MSTR.RM_RFQ_OPTION, RFQ_MSTR.RM_VEN_DISTR_LIST_INDEX, RFQ_REPLIES_MSTR.RRM_RFQ_ID, " _
                       & "RFQ_REPLIES_MSTR.RRM_V_Company_ID, RFQ_REPLIES_MSTR.RRM_Currency_Code, RFQ_REPLIES_MSTR.RRM_Offer_Till, " _
                       & "RFQ_REPLIES_MSTR.RRM_ETD, RFQ_REPLIES_MSTR.RRM_Remarks, RFQ_REPLIES_MSTR.RRM_Pay_Term_Code, " _
                       & "RFQ_REPLIES_MSTR.RRM_Payment_Type, RFQ_REPLIES_MSTR.RRM_Ship_Mode, RFQ_REPLIES_MSTR.RRM_Ship_Term, " _
                       & "RFQ_REPLIES_MSTR.RRM_Created_On, RFQ_REPLIES_MSTR.RRM_Created_By, RFQ_REPLIES_MSTR.RRM_GST, " _
                       & "RFQ_REPLIES_MSTR.RRM_Quot_Seq_No, RFQ_REPLIES_MSTR.RRM_Quot_Prefix, " _
                       & "RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num, RFQ_REPLIES_MSTR.RRM_Contact_Person, " _
                       & "RFQ_REPLIES_MSTR.RRM_Contact_Number, RFQ_REPLIES_MSTR.RRM_Email, RFQ_REPLIES_MSTR.RRM_Status, " _
                       & "RFQ_REPLIES_MSTR.RRM_B_Display_Status, RFQ_REPLIES_MSTR.RRM_V_Display_Status, " _
                       & "RFQ_REPLIES_MSTR.RRM_Indicator, RFQ_REPLIES_MSTR.RRM_TotalValue, RFQ_REPLIES_DETAIL.RRD_RFQ_ID, " _
                       & "RFQ_REPLIES_DETAIL.RRD_V_Coy_Id, RFQ_REPLIES_DETAIL.RRD_Line_No, RFQ_REPLIES_DETAIL.RRD_Product_Code, " _
                       & "RFQ_REPLIES_DETAIL.RRD_Vendor_Item_Code, RFQ_REPLIES_DETAIL.RRD_Quantity, " _
                       & "RFQ_REPLIES_DETAIL.RRD_Unit_Price, IFNULL(RFQ_REPLIES_DETAIL.RRD_Unit_Price,0) AS UnitPrice," _
                       & "RFQ_REPLIES_DETAIL.RRD_GST_Code, RFQ_REPLIES_DETAIL.RRD_GST, RFQ_REPLIES_DETAIL.RRD_GST_Desc, " _
                       & "RFQ_REPLIES_DETAIL.RRD_Product_Desc, RFQ_REPLIES_DETAIL.RRD_UOM, " _
                       & "RFQ_REPLIES_DETAIL.RRD_Delivery_Lead_Time, RFQ_REPLIES_DETAIL.RRD_Warranty_Terms, " _
                       & "RFQ_REPLIES_DETAIL.RRD_Min_Pack_Qty, RFQ_REPLIES_DETAIL.RRD_Min_Order_Qty, " _
                       & "RFQ_REPLIES_DETAIL.RRD_Remarks, RFQ_REPLIES_DETAIL.RRD_Tolerance, COMPANY_MSTR.CM_COY_ID AS Supp_Coy_ID," _
                       & "COMPANY_MSTR.CM_COY_NAME AS Supp_Coy_Name, COMPANY_MSTR.CM_ADDR_LINE1 AS Supp_Addr_Line1, " _
                       & "COMPANY_MSTR.CM_ADDR_LINE2 AS Supp_Addr_Line2, COMPANY_MSTR.CM_ADDR_LINE3 AS Supp_Addr_Line3, " _
                       & "COMPANY_MSTR.CM_POSTCODE AS Supp_Coy_Postcode, COMPANY_MSTR.CM_CITY AS Supp_Coy_City, " _
                       & "COMPANY_MSTR.CM_STATE AS Supp_Coy_State, COMPANY_MSTR.CM_COUNTRY AS Supp_Coy_Country, " _
                       & "COMPANY_MSTR.CM_PHONE AS Supp_Coy_Phone, COMPANY_MSTR.CM_FAX AS Supp_Coy_Fax, " _
                       & "COMPANY_MSTR.CM_EMAIL AS Supp_Coy_Email, COMPANY_MSTR.CM_BUSINESS_REG_NO AS Supp_Coy_BusinessRegNo " _
                       & "FROM RFQ_MSTR " _
                       & "INNER JOIN RFQ_REPLIES_MSTR ON RFQ_MSTR.RM_RFQ_ID = RFQ_REPLIES_MSTR.RRM_RFQ_ID " _
                       & "INNER JOIN RFQ_REPLIES_DETAIL ON RFQ_REPLIES_MSTR.RRM_RFQ_ID = RFQ_REPLIES_DETAIL.RRD_RFQ_ID " _
                       & "AND RFQ_REPLIES_MSTR.RRM_V_Company_ID = RFQ_REPLIES_DETAIL.RRD_V_Coy_Id " _
                       & "INNER JOIN COMPANY_MSTR ON RFQ_REPLIES_MSTR.RRM_V_Company_ID = COMPANY_MSTR.CM_COY_ID " _
                       & "INNER JOIN COMPANY_MSTR AS COMPANY_MSTR_1 ON RFQ_MSTR.RM_Coy_ID = COMPANY_MSTR_1.CM_COY_ID " _
                       & "INNER JOIN CODE_MSTR AS a ON   (a.CODE_ABBR = COMPANY_MSTR_1.CM_STATE) " _
                       & "AND (a.CODE_CATEGORY = 's') AND (a.CODE_VALUE = COMPANY_MSTR_1.CM_COUNTRY)" _
                       & "INNER JOIN CODE_MSTR b ON   (b.CODE_ABBR = COMPANY_MSTR_1.CM_COUNTRY) " _
                       & "AND (b.CODE_CATEGORY = 'ct') " _
                       & "INNER JOIN CODE_MSTR c ON   (c.CODE_ABBR = COMPANY_MSTR.CM_STATE) " _
                       & "AND (c.CODE_CATEGORY = 's') AND (c.CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)" _
                       & "INNER JOIN CODE_MSTR d ON   (d.CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) " _
                       & "AND (d.CODE_CATEGORY = 'ct') " _
                       & "INNER JOIN CODE_MSTR e ON   (e.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Pay_Term_Code) " _
                       & "AND (e.CODE_CATEGORY = 'pt') " _
                       & "INNER JOIN CODE_MSTR f ON   (f.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Payment_Type) " _
                       & "AND (f.CODE_CATEGORY = 'pm') " _
                       & "INNER JOIN CODE_MSTR g ON   (g.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Term) " _
                       & "AND (g.CODE_CATEGORY = 'St') " _
                       & "INNER JOIN CODE_MSTR h ON   (h.CODE_ABBR = RFQ_REPLIES_MSTR.RRM_Ship_Mode) " _
                       & "AND (h.CODE_CATEGORY = 'sm') " _
                       & "WHERE   (RFQ_REPLIES_MSTR.RRM_V_Company_ID = @prmVCoyID) AND (RFQ_MSTR.RM_Coy_ID = @prmBCoyID) " _
                       & "AND (RFQ_REPLIES_MSTR.RRM_Actual_Quot_Num = @prmQuoNum) AND (RFQ_MSTR.RM_RFQ_No = @prmRFQNum)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmVCoyID", Request.QueryString("SCoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmBCoyID", Request.QueryString("BCoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmQuoNum", Request.QueryString("QuoNo")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmRFQNum", Request.QueryString("RFQNo")))

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewQuotation_FTN_DataTablePreviewQuotation", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = appPath & "RFQ\PreviewQuotation-FTN.rdlc"
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "par1"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
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

            Dim fs As New FileStream(appPath & "RFQ\Quotation.PDF", FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('../RFQ/Quotation.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
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