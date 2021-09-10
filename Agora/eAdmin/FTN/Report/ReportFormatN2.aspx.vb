Imports AgoraLegacy

Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Public Class ReportFormatN21
    Inherits AppBaseClass
    Dim dispatcher As New dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim ii_ddl, ii_ddl2, jj_ddl As Integer
        Dim lstItem As New ListItem

        If Not (Page.IsPostBack) Then
            lstItem.Value = ""
            lstItem.Text = "---Select---"
            cmbYearFrom.Items.Insert(0, lstItem)
            cmbYearTo.Items.Insert(0, lstItem)
            cmbMonthFrom.Items.Insert(0, lstItem)
            cmbMonthTo.Items.Insert(0, lstItem)

            ii_ddl2 = 1
            jj_ddl = Year(Date.Now)
            For ii_ddl = 2002 To jj_ddl
                cmbYearFrom.Items.Insert(ii_ddl2, New ListItem(ii_ddl))
                cmbYearTo.Items.Insert(ii_ddl2, New ListItem(ii_ddl))
                ii_ddl2 = ii_ddl2 + 1
            Next

            ii_ddl = 1
            jj_ddl = 12
            Dim strdate As String
            For ii_ddl = 1 To jj_ddl
                Dim lst As New ListItem
                strdate = "01/" & ii_ddl & "/2005"
                lst.Value = ii_ddl
                lst.Text = Format(CDate(strdate), "MMMM")
                cmbMonthFrom.Items.Insert(ii_ddl, lst)
                cmbMonthTo.Items.Insert(ii_ddl, lst)
            Next
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
        Dim strCoyName As String
        Dim dtFrom As Date
        Dim dtTo As Date
        Dim dtDate As Date
        Dim strBeginDate As String
        Dim strEndDate As String
        Dim strStart As String
        Dim strEnd As String
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
                .CommandText = "SELECT   PO_MSTR.POM_PO_INDEX, PO_MSTR.POM_PO_NO, PO_MSTR.POM_B_COY_ID, PO_MSTR.POM_BUYER_ID, " _
                        & "PO_MSTR.POM_BUYER_NAME,po_mstr.POM_DEPT_INDEX, a.CDM_DEPT_NAME, " _
                        & "PO_MSTR.POM_S_COY_ID, AM_DEPT_INDEX,b.CDM_DEPT_NAME AS Cost_Center,PO_MSTR.POM_PO_STATUS,STATUS_DESC, " _
                        & "PO_MSTR.POM_S_COY_NAME, CT_NAME,PO_MSTR.POM_S_ATTN, " _
                        & "PO_MSTR.POM_S_REMARK, PO_MSTR.POM_PO_DATE, PO_MSTR.POM_FREIGHT_TERMS, " _
                        & "PO_MSTR.POM_CURRENCY_CODE, PO_MSTR.POM_EXCHANGE_RATE, " _
                        & "PO_MSTR.POM_PAYMENT_TERM_CODE, PO_MSTR.POM_SHIP_VIA, " _
                        & "PO_MSTR.POM_STATUS_CHANGED_BY, PO_MSTR.POM_STATUS_CHANGED_ON, " _
                        & "PO_MSTR.POM_EXTERNAL_REMARK, PO_MSTR.POM_CREATED_BY, PO_MSTR.POM_PO_COST, " _
                        & "PO_MSTR.POM_BILLING_METHOD, PO_MSTR.POM_PO_PREFIX, PO_MSTR.POM_FULFILMENT, PO_MSTR.POM_DEPT_INDEX, " _
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
                        & "USER_MSTR.UM_INVOICE_MASS_APP, PO_MSTR.POM_SHIP_AMT " _
                        & "FROM PO_MSTR INNER JOIN " _
                        & "PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO AND " _
                        & "PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID " _
                        & "LEFT JOIN PRODUCT_MSTR ON POD_PRODUCT_CODE = PM_PRODUCT_CODE " _
                        & "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " _
                        & "INNER JOIN COMPANY_MSTR ON PO_MSTR.POM_B_COY_ID = COMPANY_MSTR.CM_COY_ID " _
                        & "INNER JOIN USER_MSTR ON PO_MSTR.POM_BUYER_ID = USER_MSTR.UM_USER_ID AND " _
                        & "PO_MSTR.POM_B_COY_ID = USER_MSTR.UM_COY_ID " _
                        & "INNER JOIN status_mstr ON status_mstr.STATUS_NO=PO_MSTR.POM_PO_STATUS " _
                        & "LEFT OUTER JOIN company_dept_mstr a ON a.CDM_DEPT_INDEX = po_mstr.POM_DEPT_INDEX " _
                        & "LEFT OUTER JOIN account_mstr ON account_mstr.AM_ACCT_INDEX=po_details.POD_ACCT_INDEX " _
                        & "LEFT OUTER JOIN company_dept_mstr b ON account_mstr.AM_DEPT_INDEX = b.CDM_DEPT_INDEX " _
                        & "WHERE (PO_MSTR.POM_B_COY_ID = @prmCoyID) " _
                        & "AND POM_PO_DATE >=@prmStartDate AND POM_PO_DATE <=@prmEndDate " _
                        & "AND STATUS_TYPE='PO' " _
                        & "ORDER BY POM_PO_DATE desc"
            End With

            da = New MySqlDataAdapter(cmd)

            If Me.cmbMonthFrom.SelectedIndex > 0 Then
                dtDate = New DateTime(Me.cmbYearFrom.SelectedValue, Me.cmbMonthFrom.SelectedValue, 1)
                dtFrom = dtDate

                If Me.cmbMonthTo.SelectedValue < 12 Then
                    dtDate = New DateTime(Me.cmbYearTo.SelectedValue, Me.cmbMonthTo.SelectedValue + 1, 1)
                    dtTo = DateAdd(DateInterval.Day, -1, dtDate)
                    dtTo = New DateTime(Me.cmbYearTo.SelectedValue, Me.cmbMonthTo.SelectedValue, dtTo.Day)

                Else
                    dtDate = New DateTime(Me.cmbYearFrom.SelectedValue, Me.cmbMonthFrom.SelectedValue, 1)
                    dtFrom = dtDate
                    dtTo = New DateTime(Me.cmbYearTo.SelectedValue, Me.cmbMonthTo.SelectedValue, 31)
                End If

            Else
                dtDate = New DateTime(Me.cmbYearFrom.SelectedValue, 1, 1)
                dtFrom = dtDate
                dtTo = New DateTime(Me.cmbYearTo.SelectedValue, 12, 31)
            End If

            strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
            strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
            strStart = Format(dtFrom, "MMM yyyy")
            strEnd = Format(dtTo, "MMM yyyy")
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyIdToken")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmStartDate", strBeginDate))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmEndDate", strEndDate))
            strUserId = Session("UserName") 'Session("UserId")
            strCoyName = Session("CoyName")

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PODetails_xls_DataSetPODetails", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = dispatcher.direct("Report", "PODetails_pdf_ftn.rdlc", "FTNReport") ' Server.MapPath("PODetails_pdf.rdlc")  'appPath & "Report\PODetails_pdf.rdlc"
            'localreport.ReportPath = appPath & "Report\PODetails_xls.rdlc"
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationSettings.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "pmrequestedby"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

                    Case "pmdatefrom"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strStart)

                    Case "pmdateto"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strEnd)

                    Case "logo"
                        'par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationSettings.AppSettings("ReportCoyLogoPath") & strImgSrc)
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationSettings.AppSettings(strID) & strImgSrc)

                    Case "prmbuyercoyname"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)

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
            strFileName = "PODetailsReport.pdf"
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
        Dim strStart As String
        Dim strEnd As String = ""
        Dim strFileName As String = ""

        Try
            If Me.cmbMonthFrom.SelectedIndex > 0 Then
                dtDate = New DateTime(Me.cmbYearFrom.SelectedValue, Me.cmbMonthFrom.SelectedValue, 1)
                dtFrom = dtDate

                If Me.cmbMonthTo.SelectedValue < 12 Then
                    dtDate = New DateTime(Me.cmbYearTo.SelectedValue, Me.cmbMonthTo.SelectedValue + 1, 1)
                    dtTo = DateAdd(DateInterval.Day, -1, dtDate)
                    dtTo = New DateTime(Me.cmbYearTo.SelectedValue, Me.cmbMonthTo.SelectedValue, dtTo.Day)

                Else
                    dtDate = New DateTime(Me.cmbYearFrom.SelectedValue, Me.cmbMonthFrom.SelectedValue, 1)
                    dtFrom = dtDate
                    dtTo = New DateTime(Me.cmbYearTo.SelectedValue, Me.cmbMonthTo.SelectedValue, 31)
                End If

            Else
                dtDate = New DateTime(Me.cmbYearFrom.SelectedValue, 1, 1)
                dtFrom = dtDate
                dtTo = New DateTime(Me.cmbYearTo.SelectedValue, 12, 31)
            End If

            strBeginDate = Format(dtFrom, "yyyy-MM-dd" & " " & "00:00:00")
            strEndDate = Format(dtTo, "yyyy-MM-dd" & " " & "23:59:59")
            strStart = Format(dtFrom, "MMM yyyy")
            strEnd = Format(dtTo, "MMM yyyy")

            myConnectionString = System.Configuration.ConfigurationSettings.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT PO_MSTR.POM_PO_NO AS 'PO Number', DATE_FORMAT(po_mstr.POM_PO_DATE,'%d/%m/%Y') AS 'PO Date',po_mstr.POM_S_COY_NAME AS 'Vendor Name'," _
                        & "a.CDM_DEPT_NAME AS 'Department',PO_MSTR.POM_BUYER_NAME AS 'Buyer Name'," _
                        & "CASE WHEN po_mstr.POM_PO_STATUS=0 OR POM_PO_STATUS=12 THEN 'Draft' " _
                        & "WHEN po_mstr.POM_PO_STATUS=1 OR po_mstr.POM_PO_STATUS=2 OR po_mstr.POM_PO_STATUS=9 THEN 'Approved' " _
                        & "WHEN po_mstr.POM_PO_STATUS=3 THEN 'Accepted' " _
                        & "WHEN po_mstr.POM_PO_STATUS=4 OR po_mstr.POM_PO_STATUS=10 THEN 'Rejected' " _
                        & "WHEN po_mstr.POM_PO_STATUS=5 OR po_mstr.POM_PO_STATUS=13 THEN 'Cancelled' " _
                        & "WHEN po_mstr.POM_PO_STATUS=6 THEN 'Closed' " _
                        & "WHEN po_mstr.POM_PO_STATUS=7 OR po_mstr.POM_PO_STATUS=8 OR po_mstr.POM_PO_STATUS=11 THEN 'Submitted' END AS 'PO Status', " _
                        & "CT_NAME AS 'Commodity Type', " _
                        & "IF(PO_DETAILS.POD_VENDOR_ITEM_CODE='&nbsp;','',PO_DETAILS.POD_VENDOR_ITEM_CODE) AS 'Item Code', " _
                        & "PO_DETAILS.POD_PRODUCT_DESC AS 'Item Desc',PO_DETAILS.POD_UOM AS 'UOM', PO_DETAILS.POD_ORDERED_QTY AS 'Qty',company_mstr.CM_CURRENCY_CODE AS 'Currency', " _
                        & "FORMAT(((PO_DETAILS.POD_ORDERED_QTY*PO_DETAILS.POD_UNIT_COST)+(((PO_DETAILS.POD_ORDERED_QTY*PO_DETAILS.POD_UNIT_COST)*IFNULL(PO_DETAILS.POD_GST,0))/100)),2) AS 'PO Value' " _
                        & "FROM PO_MSTR " _
                        & "INNER JOIN PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO AND PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID  " _
                        & "LEFT JOIN PRODUCT_MSTR ON POD_PRODUCT_CODE = PM_PRODUCT_CODE  " _
                        & "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID  " _
                        & "INNER JOIN COMPANY_MSTR ON PO_MSTR.POM_B_COY_ID = COMPANY_MSTR.CM_COY_ID  " _
                        & "INNER JOIN USER_MSTR ON PO_MSTR.POM_BUYER_ID = USER_MSTR.UM_USER_ID AND PO_MSTR.POM_B_COY_ID = USER_MSTR.UM_COY_ID  " _
                        & "INNER JOIN status_mstr ON status_mstr.STATUS_NO=PO_MSTR.POM_PO_STATUS AND STATUS_TYPE='PO'  " _
                        & "LEFT OUTER JOIN company_dept_mstr a ON a.CDM_DEPT_INDEX = po_mstr.POM_DEPT_INDEX  " _
                        & "LEFT OUTER JOIN account_mstr ON account_mstr.AM_ACCT_INDEX=po_details.POD_ACCT_INDEX  " _
                        & "LEFT OUTER JOIN company_dept_mstr b ON account_mstr.AM_DEPT_INDEX = b.CDM_DEPT_INDEX  " _
                        & "WHERE (PO_MSTR.POM_B_COY_ID =  '" & Session("CompanyIdToken") & "') AND POM_PO_DATE >='" & strBeginDate & "' " _
                        & "AND POM_PO_DATE <='" & strEndDate & "' " _
                        & "ORDER BY POM_PO_DATE desc"
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
            strFileName = "PODetailsReport" & "(" & Format(dtFrom, "MMMyyyy") & ").xls"
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