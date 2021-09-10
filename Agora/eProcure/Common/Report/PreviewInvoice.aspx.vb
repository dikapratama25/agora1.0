Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Public Class PreviewInvoice1
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher
    Dim objDb As New EAD.DBCom

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PreviewInvoice()
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
        Dim strSQL As String = ""
        Dim dsDO As New DataSet
        Dim dsGRN As New DataSet
        Dim strDO As String = ""
        Dim strGRN As String = ""
        Dim a As Integer = 0
        Dim strNo As String = ""
        Dim strDelCode As String = ""
        Dim strDelTerm As String = ""
        Dim strTaxInv As String = "N"

        'strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("vcomid"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")
        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("vcomid"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT   INVOICE_DETAILS.ID_INVOICE_NO, INVOICE_DETAILS.ID_S_COY_ID, INVOICE_DETAILS.ID_INVOICE_LINE, " _
                            & "INVOICE_DETAILS.ID_PO_LINE, INVOICE_DETAILS.ID_B_ITEM_CODE, INVOICE_DETAILS.ID_PRODUCT_DESC, " _
                            & "INVOICE_DETAILS.ID_UOM, INVOICE_DETAILS.ID_GST, INVOICE_DETAILS.ID_RECEIVED_QTY, " _
                            & "INVOICE_DETAILS.ID_UNIT_COST, INVOICE_DETAILS.ID_WARRANTY_TERMS, " _
                            & "INVOICE_DETAILS.ID_ACCT_INDEX, INVOICE_DETAILS.ID_B_CATEGORY_CODE, INVOICE_DETAILS.ID_GST_RATE, " _
                            & "INVOICE_DETAILS.ID_GST_INPUT_TAX_CODE, INVOICE_DETAILS.ID_GST_OUTPUT_TAX_CODE, INVOICE_DETAILS.ID_GST_VALUE, " _
                            & "INVOICE_DETAILS.ID_B_GL_CODE, INVOICE_MSTR.IM_INVOICE_INDEX, INVOICE_MSTR.IM_INVOICE_NO, " _
                            & "INVOICE_MSTR.IM_S_COY_ID, INVOICE_MSTR.IM_S_COY_NAME, INVOICE_MSTR.IM_PO_INDEX, " _
                            & "INVOICE_MSTR.IM_B_COY_ID, INVOICE_MSTR.IM_PAYMENT_DATE, INVOICE_MSTR.IM_REMARK, " _
                            & "INVOICE_MSTR.IM_CREATED_BY, INVOICE_MSTR.IM_CREATED_ON, INVOICE_MSTR.IM_INVOICE_STATUS, " _
                            & "INVOICE_MSTR.IM_PAYMENT_NO, INVOICE_MSTR.IM_YOUR_REF, INVOICE_MSTR.IM_OUR_REF, " _
                            & "INVOICE_MSTR.IM_INVOICE_PREFIX, INVOICE_MSTR.IM_SUBMITTEDBY_FO, " _
                            & "INVOICE_MSTR.IM_EXCHANGE_RATE, INVOICE_MSTR.IM_FINANCE_REMARKS, INVOICE_MSTR.IM_PRINTED, " _
                            & "INVOICE_MSTR.IM_FOLDER, INVOICE_MSTR.IM_FM_APPROVED_DATE, IFNULL(INVOICE_MSTR.IM_WITHHOLDING_TAX,0) AS IM_WITHHOLDING_TAX, " _
                            & "INVOICE_MSTR.IM_DOWNLOADED_DATE, INVOICE_MSTR.IM_EXTERNAL_IND, " _
                            & "INVOICE_MSTR.IM_REFERENCE_NO, INVOICE_MSTR.IM_INVOICE_TOTAL, INVOICE_MSTR.IM_GST_INVOICE, " _
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
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmSCoyID", Request.QueryString("vcomid")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmBCoyID", Request.QueryString("BCoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmInvNo", Request.QueryString("INVNO")))

            da.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then

                strSQL = "SELECT CDM_DO_NO, CDM_GRN_NO FROM COMPANY_DOC_MATCH WHERE CDM_INVOICE_NO = '" & Request.QueryString("INVNO") & "' AND CDM_S_COY_ID='" & Request.QueryString("vcomid") & "' AND CDM_B_COY_ID='" & Request.QueryString("BCoyID") & "' "

                dsDO = objDb.FillDs(strSQL)
                If dsDO.Tables(0).Rows.Count > 0 Then
                    For a = 0 To dsDO.Tables(0).Rows.Count - 1
                        If a = 0 Then
                            strDO = dsDO.Tables(0).Rows(a)(0)
                        Else
                            strDO = strDO & vbCrLf & dsDO.Tables(0).Rows(a)(0)
                        End If
                    Next

                    For a = 0 To dsDO.Tables(0).Rows.Count - 1
                        If a = 0 Then
                            strGRN = Common.parseNull(dsDO.Tables(0).Rows(a)(1))
                        Else
                            strGRN = strGRN & vbCrLf & dsDO.Tables(0).Rows(a)(1)
                        End If
                    Next

                End If

                'strSQL = "SELECT dom_do_no FROM do_mstr WHERE dom_po_index=" & ds.Tables(0).Rows(0)("POM_PO_INDEX") & " and dom_s_coy_id='" & Request.QueryString("vcomid") & "'"
                'dsDO = objDb.FillDs(strSQL)
                'If dsDO.Tables(0).Rows.Count > 0 Then
                '    For a = 0 To dsDO.Tables(0).Rows.Count - 1
                '        If a = 0 Then
                '            strDO = dsDO.Tables(0).Rows(a)(0)
                '        Else
                '            strDO = strDO & vbCrLf & dsDO.Tables(0).Rows(a)(0)
                '        End If
                '    Next
                'End If

                'strSQL = "SELECT gm_grn_no FROM grn_mstr WHERE gm_po_index=" & ds.Tables(0).Rows(0)("POM_PO_INDEX") & " and gm_b_coy_id='" & Request.QueryString("BCoyID") & "'"
                'dsGRN = objDb.FillDs(strSQL)
                'If dsGRN.Tables(0).Rows.Count > 0 Then
                '    For a = 0 To dsGRN.Tables(0).Rows.Count - 1
                '        If a = 0 Then
                '            strGRN = dsGRN.Tables(0).Rows(a)(0)
                '        Else
                '            strGRN = strGRN & vbCrLf & dsGRN.Tables(0).Rows(a)(0)
                '        End If
                '    Next
                'End If
                strNo = ds.Tables(0).Rows(0)("POM_PO_NO") & vbCrLf & strDO & vbCrLf & strGRN

                'Check Invoice whether it is Tax Invoice or Normal Invoice
                strSQL = "SELECT IM_GST_INVOICE FROM INVOICE_MSTR WHERE IM_INVOICE_NO = '" & Common.Parse(Request.QueryString("INVNO")) & "' " &
                        "AND IM_S_COY_ID = '" & Request.QueryString("vcomid") & "' AND IM_B_COY_ID = '" & Request.QueryString("BCoyID") & "'"
                strTaxInv = objDb.GetVal(strSQL)

                'Delivery Term
                strSQL = "SELECT IFNULL(POM_DEL_CODE,'') FROM PO_MSTR WHERE POM_B_COY_ID ='" & Request.QueryString("BCoyID") & "' AND POM_PO_NO = '" & ds.Tables(0).Rows(0)("POM_PO_NO") & "'"
                strDelCode = objDb.GetVal(strSQL)

                If strDelCode <> "" Then
                    strSQL = "SELECT IFNULL(CONCAT(CDT_DEL_CODE, ' (', CDT_DEL_NAME, ')'),'') FROM COMPANY_DELIVERY_TERM WHERE CDT_DEL_CODE = '" & Common.Parse(strDelCode) & "' AND CDT_COY_ID = '" & Request.QueryString("BCoyID") & "'"
                    strDelTerm = objDb.GetVal(strSQL)
                End If
            End If

            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("InvoiceDocument_FTN_DataSetPreviewInvoice", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            If strTaxInv = "Y" Then

                'Jules 2018.10.08 - SST
                'localreport.ReportPath = dispatcher.direct("Report", "PreviewTaxINVOICE-FTN.rdlc", "Report") ' appPath & "Invoice\PreviewINVOICE-FTN.rdlc"
                Dim objSST As New GST
                Dim strDocType As String = "Service"
                Dim blnSST As Boolean
                blnSST = objSST.chkDocumentType(Request.QueryString("INVNO"), "INV",, Request.QueryString("vcomid"), Request.QueryString("BCoyID"),, strDocType)
                If blnSST Then
                    localreport.ReportPath = dispatcher.direct("Report", "PreviewTaxINVOICE-" & strDocType & ".rdlc", "Report")
                Else 'original code
                    If strDelTerm = "" Then
                        localreport.ReportPath = dispatcher.direct("Report", "PreviewINVOICE-FTN.rdlc", "Report") ' appPath & "Invoice\PreviewINVOICE-FTN.rdlc"
                    Else
                        localreport.ReportPath = dispatcher.direct("Report", "PreviewINVOICE-FTN-SEH.rdlc", "Report")
                    End If
                End If
                'End modification.
            Else
                If strDelTerm = "" Then
                    localreport.ReportPath = dispatcher.direct("Report", "PreviewINVOICE-FTN.rdlc", "Report") ' appPath & "Invoice\PreviewINVOICE-FTN.rdlc"
                Else
                    localreport.ReportPath = dispatcher.direct("Report", "PreviewINVOICE-FTN-SEH.rdlc", "Report")
                End If
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

                        'Case "prmdono"
                        '    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strDO)

                        'Case "prmgrnno"
                        '    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strGRN)

                    Case "prmno"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strNo)

                    Case "dt"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strDelTerm)

                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            'Dim deviceInfo As String = _
            '    "<DeviceInfo>" + _
            '        "  <OutputFormat>EMF</OutputFormat>" + _
            '        "  <PageWidth>8.27in</PageWidth>" + _
            '        "  <PageHeight>11in</PageHeight>" + _
            '        "  <MarginTop>0.25in</MarginTop>" + _
            '        "  <MarginLeft>0.25in</MarginLeft>" + _
            '        "  <MarginRight>0.25in</MarginRight>" + _
            '        "  <MarginBottom>0.25in</MarginBottom>" + _
            '        "</DeviceInfo>"
            Dim deviceInfo As String = _
                            "<DeviceInfo>" + _
                                "  <OutputFormat>EMF</OutputFormat>" + _
                                "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            Dim strFileName As String = "INV_" & Request.QueryString(Trim("INVNO")) & "_" & Session("CompanyID") & "_" & Session("UserId") & ".pdf"
            strFileName = Replace(strFileName, "/", "^")
            Dim strTemp As String = dispatcher.direct("Report", "Temp", "Report")
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