'Added/Modified for IPP GST Stage 2A - CH (30/1/2015)
Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Collections
Imports AgoraLegacy
Imports SSO.Component
Imports System.IO
Imports System.Web.UI.WebControls
Imports System.Diagnostics
Imports CryptoClass
Imports System.Security.Cryptography

Namespace AgoraLegacy

    Public Class Billing
        Dim objDb As New EAD.DBCom

        'Chee Hong - 23/12/2014 - 
        Public Function PopulateBillEnqList(ByVal docno As String, ByVal docstatus As String, ByVal doctype As String, ByVal docsdt As String, ByVal docedt As String, ByVal strVen As String, ByVal strVenAddr As String, Optional ByVal strDept As String = "", Optional ByVal strVendorIndex As String = "") As DataSet
            Dim strSql As String
            Dim strSql2 As String = ""
            Dim dsGroup As DataSet
            'Modified for IPP GST Stage 2A - CH - 13 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If
            '------------------------------------------------

            'strSql = "SELECT * FROM ("

            'Search document No.
            If strDept = "" Then
                strSql = "SELECT BM_INVOICE_INDEX, Billing_mstr.BM_INVOICE_NO, bm_dn_no, BM_CREATED_BY, BM_BILL_DATE, BM_S_COY_NAME, BM_BANK_CODE, BM_BANK_ACCT, BM_CURRENCY_CODE, " & _
                        "BM_INVOICE_TOTAL, BM_RECEIPT_DATE, BM_RECEIPT_NO, BM_INVOICE_STATUS, bm_remarks1 " & _
                        "FROM BILLING_MSTR " & _
                        "INNER JOIN IPP_COMPANY ON BM_S_COY_ID = IC_INDEX AND IC_COY_ID = '" & Common.Parse(strDefIPPCompID) & "' " & _
                        "INNER JOIN STATUS_MSTR ON STATUS_TYPE = 'BIL' AND STATUS_NO = BM_INVOICE_STATUS AND STATUS_DELETED = 'N' " & _
                        "INNER JOIN Billing_Details ON Billing_Details.bm_invoice_no = Billing_Mstr.bm_invoice_no " & _
                        "WHERE BM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                        "AND NOT BM_INVOICE_TYPE IS NULL"

                'strSql &= "SELECT IM_INVOICE_INDEX, IM_INVOICE_NO, ISD_DOC_NO, IM_CREATED_BY, IM_DOC_DATE,IM_S_COY_NAME, IM_BANK_CODE, IM_BANK_ACCT, " & _
                '        "IM_CURRENCY_CODE, ISD_DOC_AMT, IM_INVOICE_TOTAL, IM_PRCS_SENT, IM_PAYMENT_TERM, IM_PAYMENT_DATE, IM_PAYMENT_NO, " & _
                '        "IM_INVOICE_STATUS, ic2.IC_BANK_CODE, ic2.IC_BANK_ACCT, STATUS_DESC, ic2.IC_ADDR_LINE1, " & _
                '        "IM_INVOICE_TYPE, IM_PRCS_RECV " & _
                '        "FROM INVOICE_MSTR " & _
                '        "INNER JOIN IPP_COMPANY ON IM_S_COY_ID = IC_INDEX AND IC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                '        "INNER JOIN STATUS_MSTR ON STATUS_NO = IM_INVOICE_STATUS AND STATUS_TYPE = 'INV' AND STATUS_DELETED = 'N' " & _
                '        "INNER JOIN IPP_COMPANY ic2 ON IM_S_COY_ID = ic2.IC_INDEX AND ic2.IC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                '        "LEFT JOIN IPP_SUB_DOC ON IM_INVOICE_INDEX = ISD_MSTR_DOC_INDEX " & _
                '        "WHERE IM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                '        "AND NOT IM_INVOICE_TYPE IS NULL"
            Else
                strSql = "SELECT BM_INVOICE_INDEX, Billing_mstr.BM_INVOICE_NO, bm_dn_no, BM_CREATED_BY, BM_BILL_DATE, BM_S_COY_NAME, BM_BANK_CODE, BM_BANK_ACCT, BM_CURRENCY_CODE, " & _
                       "BM_INVOICE_TOTAL, BM_RECEIPT_DATE, BM_RECEIPT_NO, BM_INVOICE_STATUS, bm_remarks1 " & _
                       "FROM BILLING_MSTR " & _
                       "INNER JOIN IPP_COMPANY ON BM_S_COY_ID = IC_INDEX AND IC_COY_ID = '" & Common.Parse(strDefIPPCompID) & "' " & _
                       "INNER JOIN STATUS_MSTR ON STATUS_TYPE = 'BIL' AND STATUS_NO = BM_INVOICE_STATUS AND STATUS_DELETED = 'N' " & _
                       "INNER JOIN Billing_Details ON Billing_Details.bm_invoice_no = Billing_Mstr.bm_invoice_no " & _
                       "INNER JOIN USER_MSTR ON BM_CREATED_BY = UM_USER_ID " & _
                       "WHERE BM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                       "AND NOT BM_INVOICE_TYPE IS NULL " & _
                       "AND UM_DEPT_ID = '" & strDept & "'"

                'strSql &= "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO, ISD_DOC_NO, IM_CREATED_BY, IM_DOC_DATE, IM_S_COY_NAME, IM_BANK_CODE, IM_BANK_ACCT, " & _
                '        "IM_CURRENCY_CODE, ISD_DOC_AMT, IM_INVOICE_TOTAL, IM_PRCS_SENT, IM_PAYMENT_TERM, IM_PAYMENT_DATE, IM_PAYMENT_NO, " & _
                '        "IM_INVOICE_STATUS, ic2.IC_BANK_CODE, ic2.IC_BANK_ACCT, STATUS_DESC, ic2.IC_ADDR_LINE1, " & _
                '        "IM_INVOICE_TYPE, IM_PRCS_RECV " & _
                '        "FROM INVOICE_MSTR " & _
                '        "INNER JOIN IPP_COMPANY ON IM_S_COY_ID = IC_INDEX AND IC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                '        "INNER JOIN STATUS_MSTR ON STATUS_NO = IM_INVOICE_STATUS AND STATUS_TYPE = 'INV' AND STATUS_DELETED = 'N' " & _
                '        "INNER JOIN IPP_COMPANY ic2 ON IM_S_COY_ID = ic2.IC_INDEX AND ic2.IC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                '        "INNER JOIN USER_MSTR um ON IM_CREATED_BY = um.UM_USER_ID " & _
                '        "LEFT JOIN IPP_SUB_DOC ON IM_INVOICE_INDEX = ISD_MSTR_DOC_INDEX " & _
                '        "WHERE IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND NOT IM_INVOICE_TYPE IS NULL " & _
                '        "AND um.UM_DEPT_ID = '" & Dept & "'"
            End If

            If docno <> "" Then
                strSql &= " AND billing_mstr.BM_INVOICE_NO LIKE '%" & docno & "%'"
            End If
            If docsdt <> "" Then
                strSql &= " AND BM_CREATED_ON >= " & Common.ConvertDate(docsdt) & ""
            End If
            If docedt <> "" Then
                strSql &= " AND BM_CREATED_ON <= " & "'" & Format(CDate(docedt), "yyyy-MM-dd 23:59:00") & "'" & ""
            End If
            If doctype <> "" Then
                strSql &= " AND BM_INVOICE_TYPE ='" & doctype & "'"
            End If
            If docstatus <> "" Then
                strSql &= " AND BM_INVOICE_STATUS IN (" & docstatus & ")"
            End If
            If strVendorIndex <> "" Then
                strSql &= " AND billing_mstr.BM_S_COY_ID = '" & strVendorIndex & "'"
            End If
            If strVenAddr <> "" Then
                strSql &= " AND IC_ADDR_LINE1 LIKE '%" & strVenAddr & "%'"
            End If

            'strSql &= " UNION "

            ''Search Sub-document No.
            'If Dept = "" Then
            '    strSql &= "SELECT IM_INVOICE_INDEX, IM_INVOICE_NO, ISD_DOC_NO, IM_CREATED_BY, IM_DOC_DATE,IM_S_COY_NAME, IM_BANK_CODE, IM_BANK_ACCT, " & _
            '            "IM_CURRENCY_CODE, ISD_DOC_AMT, IM_INVOICE_TOTAL, IM_PRCS_SENT, IM_PAYMENT_TERM, IM_PAYMENT_DATE, IM_PAYMENT_NO, " & _
            '            "IM_INVOICE_STATUS, ic2.IC_BANK_CODE, ic2.IC_BANK_ACCT, STATUS_DESC, ic2.IC_ADDR_LINE1, " & _
            '            "IM_INVOICE_TYPE, IM_PRCS_RECV " & _
            '            "FROM INVOICE_MSTR " & _
            '            "INNER JOIN IPP_COMPANY ON IM_S_COY_ID = IC_INDEX AND IC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
            '            "INNER JOIN STATUS_MSTR ON STATUS_NO = IM_INVOICE_STATUS AND STATUS_TYPE = 'INV' AND STATUS_DELETED = 'N' " & _
            '            "INNER JOIN IPP_COMPANY ic2 ON IM_S_COY_ID = ic2.IC_INDEX AND ic2.IC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
            '            "LEFT JOIN IPP_SUB_DOC ON IM_INVOICE_INDEX = ISD_MSTR_DOC_INDEX " & _
            '            "WHERE IM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
            '            "AND NOT IM_INVOICE_TYPE IS NULL"
            'Else
            '    strSql &= "SELECT IM_INVOICE_INDEX,IM_INVOICE_NO, ISD_DOC_NO, IM_CREATED_BY, IM_DOC_DATE, IM_S_COY_NAME, IM_BANK_CODE, IM_BANK_ACCT, " & _
            '            "IM_CURRENCY_CODE, ISD_DOC_AMT, IM_INVOICE_TOTAL, IM_PRCS_SENT, IM_PAYMENT_TERM, IM_PAYMENT_DATE, IM_PAYMENT_NO, " & _
            '            "IM_INVOICE_STATUS, ic2.IC_BANK_CODE, ic2.IC_BANK_ACCT, STATUS_DESC, ic2.IC_ADDR_LINE1, " & _
            '            "IM_INVOICE_TYPE, IM_PRCS_RECV " & _
            '            "FROM INVOICE_MSTR " & _
            '            "INNER JOIN IPP_COMPANY ON IM_S_COY_ID = IC_INDEX AND IC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
            '            "INNER JOIN STATUS_MSTR ON STATUS_NO = IM_INVOICE_STATUS AND STATUS_TYPE = 'INV' AND STATUS_DELETED = 'N' " & _
            '            "INNER JOIN IPP_COMPANY ic2 ON IM_S_COY_ID = ic2.IC_INDEX AND ic2.IC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
            '            "INNER JOIN USER_MSTR um ON IM_CREATED_BY = um.UM_USER_ID " & _
            '            "LEFT JOIN IPP_SUB_DOC ON IM_INVOICE_INDEX = ISD_MSTR_DOC_INDEX " & _
            '            "WHERE IM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND NOT IM_INVOICE_TYPE IS NULL " & _
            '            "AND um.um_dept_id = '" & Dept & "'"
            'End If

            'If docno <> "" Then
            '    strSql &= " AND ISD_DOC_NO LIKE '%" & docno & "%'"
            'End If
            'If docsdt <> "" Then
            '    strSql &= " AND ISD_DOC_DATE >= " & Common.ConvertDate(docsdt) & ""
            'End If
            'If docedt <> "" Then
            '    strSql &= " AND ISD_DOC_DATE <= " & "'" & Format(CDate(docedt), "yyyy-MM-dd 23:59:00") & "'" & ""
            'End If
            'If payadv <> "" Then
            '    strSql &= " AND IM_PAYMENT_NO LIKE '%" & payadv & "%'"
            'End If
            'If docstatus <> "" Then
            '    strSql &= " AND IM_INVOICE_STATUS IN (" & docstatus & ")"
            'End If
            'If paysdt <> "" Then
            '    strSql &= " AND IM_PAYMENT_DATE >= " & Common.ConvertDate(paysdt) & ""
            'End If
            'If payedt <> "" Then
            '    strSql &= " AND IM_PAYMENT_DATE <= " & "'" & Format(CDate(payedt), "yyyy-MM-dd 23:59:00") & "'" & ""
            'End If
            'If psdsentsdt <> "" Then
            '    strSql &= " AND IM_PRCS_SENT >= " & Common.ConvertDate(psdsentsdt) & ""
            'End If
            'If psdsentedt <> "" Then
            '    strSql &= " AND IM_PRCS_SENT <= " & "'" & Format(CDate(psdsentedt), "yyyy-MM-dd 23:59:00") & "'" & ""
            'End If
            'If strVendorIndex <> "" Then
            '    strSql &= " AND IM_S_COY_ID = '" & strVendorIndex & "'"
            'End If
            'If strVenAddr <> "" Then
            '    strSql &= " AND ic2.IC_ADDR_LINE1 like '%" & strVenAddr & "%'"
            'End If

            strSql &= " GROUP BY bm_invoice_index ORDER BY BM_INVOICE_INDEX "

            dsGroup = objDb.FillDs(strSql)
            PopulateBillEnqList = dsGroup
        End Function

        'Zulham 30-01-2015 IPP-GST Stage 2A
        Public Function SaveBillingDocHeader(ByVal dsDoc As DataSet, Optional ByVal blnSave As Boolean = False, Optional ByVal strOldDoc As String = "", Optional ByVal blnUpdate As Boolean = False, Optional ByRef strBillingNo As String = "0")
            Dim strSQL As String
            Dim strAryQuery(0) As String
            Dim strUserDept As String
            Dim strIPPOF As String
            Dim blnIPPOF As Boolean
            Dim objUsers As New Users
            Dim strUserID As String
            'Zulham 25062015 - variable for billingInvoiceNo
            Dim strInvNo As String = ""

            strUserID = Common.parseNull(HttpContext.Current.Session("UserID"))

            strUserDept = objDb.GetVal("SELECT CDM_DEPT_INDEX FROM COMPANY_DEPT_MSTR WHERE CDM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND CDM_DELETED = 'N' AND CDM_DEPT_CODE = (SELECT UM_DEPT_ID FROM USER_MSTR WHERE UM_USER_ID = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND UM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "')")

            If blnSave Then
                'For update
                Dim invidx As String
                If invidx = "" Then
                    invidx = objDb.GetVal("SELECT BM_INVOICE_INDEX FROM BILLING_MSTR WHERE BM_INVOICE_NO='" & strOldDoc & "' AND BM_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND BM_S_COY_ID = '" & dsDoc.Tables(0).Rows(0)("OldVenCompIDX") & "'")

                End If

                If invidx = "" Then
                    invidx = objDb.GetVal("SELECT BM_INVOICE_INDEX FROM BILLING_MSTR WHERE BM_INVOICE_NO='" & strOldDoc & "' AND BM_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND BM_S_COY_ID = '" & dsDoc.Tables(0).Rows(0)("VenCompIDX") & "'")
                End If

                If blnIPPOF = True Then
                    'CH - 8 Apr 2015 - Why need Update/Insert into INVOICE_MSTR? Need futher investigation.
                    'strSQL = "UPDATE INVOICE_MSTR SET IM_INVOICE_NO='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocNo")) & "'," & _
                    '                "IM_S_COY_ID='" & dsDoc.Tables(0).Rows(0)("VenCompIDX") & "'," & _
                    '                "IM_S_COY_NAME='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VendorName")) & "'," & _
                    '                "IM_REMARK='" & Common.Parse(dsDoc.Tables(0).Rows(0)("InternalRemark")) & "'," & _
                    '                "IM_INVOICE_TOTAL='" & dsDoc.Tables(0).Rows(0)("PaymentAmt") & "'," & _
                    '                "IM_INVOICE_TYPE='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) & "'," & _
                    '                "IM_DOC_DATE=NOW()," & _
                    '                "IM_IPP_PO='" & Common.Parse(dsDoc.Tables(0).Rows(0)("ManualPONo")) & "'," & _
                    '                "IM_ADDR_LINE1='" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine1"))) & "'," & _
                    '                "IM_ADDR_LINE2='" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine2"))) & "'," & _
                    '                "IM_ADDR_LINE3='" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine3"))) & "'," & _
                    '                "IM_POSTCODE='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrPostCode")) & "'," & _
                    '                "IM_CITY='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrCity")) & "'," & _
                    '                "IM_STATE='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrState")) & "'," & _
                    '                "IM_COUNTRY='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrCountry")) & "'," & _
                    '                "IM_LATE_REASON='" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("LateSubmit"))) & "'," & _
                    '                "IM_CURRENCY_CODE='" & dsDoc.Tables(0).Rows(0)("CurrencyCode") & "'," & _
                    '                "IM_BANK_CODE = '" & dsDoc.Tables(0).Rows(0)("BankCode") & "'," & _
                    '                "IM_DUE_DATE=" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("DocDueDate")) & "," & _
                    '                "IM_BANK_ACCT = '" & dsDoc.Tables(0).Rows(0)("BankAccount") & "'," & _
                    '                "IM_REMARKS2 = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("BeneficiaryDetails")) & "', " & _
                    '                "im_ind1 = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("MasterDocument")) & "', " & _
                    '                "im_company_category = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("CompanyCategory")) & "', " & _
                    '                "im_resident_type = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("ResidentType")) & "', " & _
                    '                "im_additional_1 = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("EmpId")) & "' " & _
                    '                "WHERE IM_INVOICE_INDEX='" & invidx & "'"
                Else
                    strSQL = "UPDATE BILLING_MSTR SET BM_INVOICE_NO='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocNo")) & "'," & _
                    "BM_S_COY_ID='" & dsDoc.Tables(0).Rows(0)("VenCompIDX") & "'," & _
                    "BM_S_COY_NAME='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VendorName")) & "'," & _
                    "BM_REMARK='" & Common.Parse(dsDoc.Tables(0).Rows(0)("InternalRemark")) & "'," & _
                    "BM_INVOICE_TOTAL='" & dsDoc.Tables(0).Rows(0)("PaymentAmt") & "'," & _
                    "BM_INVOICE_TYPE='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) & "'," & _
                    "BM_DOC_DATE=NOW()," & _
                    "BM_ADDR_LINE1='" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine1"))) & "'," & _
                    "BM_ADDR_LINE2='" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine2"))) & "'," & _
                    "BM_ADDR_LINE3='" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine3"))) & "'," & _
                    "BM_POSTCODE='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrPostCode")) & "'," & _
                    "BM_CITY='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrCity")) & "'," & _
                    "BM_STATE='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrState")) & "'," & _
                    "BM_COUNTRY='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrCountry")) & "'," & _
                    "BM_CURRENCY_CODE='" & dsDoc.Tables(0).Rows(0)("CurrencyCode") & "'," & _
                    "BM_BANK_CODE = '" & dsDoc.Tables(0).Rows(0)("BankCode") & "'," & _
                    "BM_BANK_ACCT = '" & dsDoc.Tables(0).Rows(0)("BankAccount") & "'," & _
                    "BM_COMPANY_CATEGORY = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("CompanyCategory")) & "', " & _
                    "BM_RESIDENT_TYPE = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("ResidentType")) & "', " & _
                    "BM_INVOICE_WTH_TOTAL = '" & Common.Parse(dsDoc.Tables(0).Rows(0)("AmtWithoutGST")) & "' " & _
                    "WHERE BM_INVOICE_INDEX='" & invidx & "'"
                End If

                Common.Insert2Ary(strAryQuery, strSQL)

                'change vendor index in invoice detail
                strSQL = "UPDATE BILLING_DETAILS SET BM_S_COY_ID='" & dsDoc.Tables(0).Rows(0)("VenCompIDX") & "'," & _
                "BM_INVOICE_NO='" & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocNo")) & "' " & _
                "WHERE BM_INVOICE_NO='" & strOldDoc & "' AND BM_S_COY_ID = '" & dsDoc.Tables(0).Rows(0)("OldVenCompIDX") & "'"
                Common.Insert2Ary(strAryQuery, strSQL)

                ''Zulham 21/06/2017
                ''IPP Stage 3
                'If Trim(Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType"))) = "INV" _
                'Or Trim(Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType"))) = "NON" Then
                '    strSQL = "UPDATE BILLING_DETAILS SET BM_ref_no='' " & _
                '    "WHERE BM_INVOICE_NO='" & strOldDoc & "' AND BM_S_COY_ID = '" & dsDoc.Tables(0).Rows(0)("OldVenCompIDX") & "'"
                '    Common.Insert2Ary(strAryQuery, strSQL)
                'End If

                Dim strStatus As String
                strStatus = getBillingStatus(invidx)

                If strStatus = "4" And blnUpdate = False Then
                    strSQL = " UPDATE BILLING_MSTR SET  BM_INVOICE_STATUS = 1, " & _
                             "BM_STATUS_CHANGED_BY ='" & HttpContext.Current.Session("UserId") & "'," & _
                             "BM_STATUS_CHANGED_ON = NOW()," & _
                             "BM_BANK_CODE = '" & dsDoc.Tables(0).Rows(0)("BankCode") & "'," & _
                             "BM_BANK_ACCT = '" & dsDoc.Tables(0).Rows(0)("BankAccount") & "'" & _
                             "WHERE BM_INVOICE_INDEX = " & invidx & "  AND BM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"

                    Common.Insert2Ary(strAryQuery, strSQL)
                End If

            Else
                If blnIPPOF = True Then
                    'CH - 8 Apr 2015 - Why need Update/Insert into INVOICE_MSTR? Need futher investigation.
                    'strSQL = " INSERT INTO INVOICE_MSTR(" & _
                    '                        "IM_INVOICE_NO,IM_S_COY_ID,IM_S_COY_NAME,IM_B_COY_ID,IM_REMARK, " & _
                    '                        "IM_CREATED_BY,IM_CREATED_ON,IM_INVOICE_STATUS,IM_INVOICE_TOTAL,IM_PAYMENT_TERM, " & _
                    '                        "IM_INVOICE_TYPE,IM_DOC_DATE,IM_IPP_PO,IM_ADDR_LINE1, " & _
                    '                        "IM_ADDR_LINE2,IM_ADDR_LINE3,IM_POSTCODE,IM_CITY,IM_STATE,IM_COUNTRY,IM_LATE_REASON, " & _
                    '                        "IM_CURRENCY_CODE, IM_WITHHOLDING_TAX, IM_WITHHOLDING_OPT, IM_WITHHOLDING_REMARKS, IM_EXCHG_RATE,IM_BANK_CODE,IM_BANK_ACCT,IM_DUE_DATE,IM_PRCS_SENT,IM_PRCS_SENT_ID,IM_PRCS_RECV,IM_PRCS_RECV_ID,IM_REMARKS2,im_ind1,im_company_category,im_resident_type,im_additional_1,IM_DEPT_INDEX) " & _
                    '                        "VALUES('" & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocNo")) & "'," & _
                    '                        "'" & dsDoc.Tables(0).Rows(0)("VenCompIDX") & "'," & _
                    '                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VendorName")) & "'," & _
                    '                        "'" & Common.parseNull(HttpContext.Current.Session("CompanyID")) & "'," & _
                    '                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("InternalRemark")) & "'," & _
                    '                        "'" & HttpContext.Current.Session("UserID") & "'," & _
                    '                        "NOW(),1," & _
                    '                        "'" & dsDoc.Tables(0).Rows(0)("PaymentAmt") & "'," & _
                    '                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("PaymentMethod")) & "'," & _
                    '                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) & "'," & _
                    '                        "" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("DocDate") & " 00:00:00") & "," & _
                    '                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("ManualPONo")) & "'," & _
                    '                        "'" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine1"))) & "'," & _
                    '                        "'" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine2"))) & "'," & _
                    '                        "'" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine3"))) & "'," & _
                    '                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrPostCode")) & "'," & _
                    '                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrCity")) & "'," & _
                    '                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrState")) & "'," & _
                    '                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrCountry")) & "'," & _
                    '                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("LateSubmit")) & "'," & _
                    '                        "'" & dsDoc.Tables(0).Rows(0)("CurrencyCode") & "'," & _
                    '                        "" & dsDoc.Tables(0).Rows(0)("WHT") & "," & _
                    '                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("WHTOpt")) & "'," & _
                    '                        "'" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("NoWHTReason"))) & "'," & _
                    '                        "" & dsDoc.Tables(0).Rows(0)("ExchangeRate") & "," & _
                    '                        "'" & dsDoc.Tables(0).Rows(0)("BankCode") & "'," & _
                    '                        "'" & dsDoc.Tables(0).Rows(0)("BankAccount") & "'," & _
                    '                        "" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("DocDueDate") & " 00:00:00") & "," & _
                    '                        "" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("PRCSSentDate") & " 00:00:00") & "," & _
                    '                        "'" & strUserID & "'," & _
                    '                        "" & Common.ConvertDate(dsDoc.Tables(0).Rows(0)("PRCSReceivedDate") & " 00:00:00") & "," & _
                    '                        "'" & strUserID & "'," & _
                    '                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("BeneficiaryDetails")) & "'," & _
                    '                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("MasterDocument")) & "', " & _
                    '                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("CompanyCategory")) & "', " & _
                    '                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("ResidentType")) & "', " & _
                    '                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("EmpId")) & "' "
                Else

                    'Zulham 25062015 - Declare a variable to hold the billing invoice no
                    strSQL = " SET @T_NO = '';"
                    Common.Insert2Ary(strAryQuery, strSQL)

                    'strInvNo = "(select count(*) from billing_mstr)"
                    strInvNo = "(SELECT CONCAT((SELECT DATE_FORMAT(NOW(),'%y')),LPAD((SELECT COUNT(*) FROM billing_mstr),5,'0')) 'billNo')"
                    Dim strsql2 = "select bm_invoice_no from billing_mstr where bm_invoice_no = " & strInvNo
                    If objDb.Exist(strsql2) > 0 Then
                        SaveBillingDocHeader = WheelMsgNum.Duplicate
                        Exit Function
                    End If

                    'Zulham 25062015 - Changed the docDate to use Now()
                    strSQL = " INSERT INTO BILLING_MSTR(" & _
                        "BM_INVOICE_NO,BM_S_COY_ID,BM_S_COY_NAME,BM_B_COY_ID,BM_REMARK, " & _
                        "BM_CREATED_BY,BM_CREATED_ON,BM_INVOICE_STATUS,BM_INVOICE_TOTAL, " & _
                        "BM_INVOICE_TYPE,BM_DOC_DATE,BM_ADDR_LINE1, " & _
                        "BM_ADDR_LINE2,BM_ADDR_LINE3,BM_POSTCODE,BM_CITY,BM_STATE,BM_COUNTRY, " & _
                        "BM_CURRENCY_CODE,BM_EXCHG_RATE,BM_BANK_CODE,BM_BANK_ACCT,BM_REMARKS2,BM_IND1,BM_COMPANY_CATEGORY,BM_RESIDENT_TYPE, BM_INVOICE_WTH_TOTAL,BM_DEPT_INDEX) " & _
                        "select " & strInvNo & "," & _
                        "'" & dsDoc.Tables(0).Rows(0)("VenCompIDX") & "'," & _
                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VendorName")) & "'," & _
                        "'" & Common.parseNull(HttpContext.Current.Session("CompanyID")) & "'," & _
                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("InternalRemark")) & "'," & _
                        "'" & HttpContext.Current.Session("UserID") & "'," & _
                        "NOW(),1," & _
                        "'" & dsDoc.Tables(0).Rows(0)("PaymentAmt") & "'," & _
                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("DocType")) & "'," & _
                        " NOW()," & _
                        "'" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine1"))) & "'," & _
                        "'" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine2"))) & "'," & _
                        "'" & Common.Parse(Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrLine3"))) & "'," & _
                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrPostCode")) & "'," & _
                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrCity")) & "'," & _
                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrState")) & "'," & _
                        "'" & Common.parseNull(dsDoc.Tables(0).Rows(0)("VenAddrCountry")) & "'," & _
                        "'" & dsDoc.Tables(0).Rows(0)("CurrencyCode") & "'," & _
                        "" & dsDoc.Tables(0).Rows(0)("ExchangeRate") & "," & _
                        "'" & dsDoc.Tables(0).Rows(0)("BankCode") & "'," & _
                        "'" & dsDoc.Tables(0).Rows(0)("BankAccount") & "'," & _
                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("BeneficiaryDetails")) & "'," & _
                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("MasterDocument")) & "', " & _
                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("CompanyCategory")) & "', " & _
                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("ResidentType")) & "', " & _
                        "'" & Common.Parse(dsDoc.Tables(0).Rows(0)("AmtWithoutGST")) & "', " & _
                        "'0' "
                End If


                'If strUserDept = "" Then

                '    strSQL = strSQL & " , NULL)"
                'Else

                '    strSQL = strSQL & " ," & Common.parseNull(strUserDept) & ")"
                'End If

                Common.Insert2Ary(strAryQuery, strSQL)
                strSQL = "SET @abc = LAST_INSERT_ID();"
                Common.Insert2Ary(strAryQuery, strSQL)

                'Zulham 25062015 - Set T_NO to the latest billing invoice no
                strSQL = " SET @T_NO = " & strInvNo & "; "
                Common.Insert2Ary(strAryQuery, strSQL)

            End If

            'Zulham 25062015 - Use BatchExecuteNew for new Billing record
            Dim strTPONo As String = ""
            If Not blnSave Then
                If objDb.BatchExecuteNew(strAryQuery, , strTPONo, "T_NO") Then
                    strBillingNo = strTPONo - 1
                End If
            Else
                objDb.BatchExecute(strAryQuery)
            End If

        End Function
        'Zulham 30-01-2015 IPP-GST Stage 2A
        Public Function SaveBillingDocDetail(ByVal aryDoc As ArrayList, ByVal Docno As String, ByVal venidx As String, Optional ByVal MasterDoc As String = "N")
            Dim i, j, intCostAllocIndex As Integer
            Dim strPayFor, strSQL, strAryQuery(0), strReimb As String
            Dim InvLineAmt, Amount, ttlAmount As Double
            Dim GST As New GST
            Dim dsDocLine As DataSet
            Dim strInvNo As String
            strInvNo = objDb.GetVal("SELECT BM_INVOICE_INDEX FROM BILLING_MSTR WHERE BM_INVOICE_NO = '" & Common.Parse(Docno) & "' AND BM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND BM_S_COY_ID = '" & venidx & "'")
            For i = 0 To aryDoc.Count - 1
                If aryDoc.Item(i)(3) <> "" Then
                    If aryDoc.Item(i)(1) = "Own Co." Then
                        strPayFor = Common.Parse(HttpContext.Current.Session("CompanyID"))
                    Else
                        strPayFor = Common.Parse(aryDoc.Item(i)(1))
                    End If

                    aryDoc(i)(14) = objDb.GetVal("SELECT CC_CC_DESC FROM COST_CENTRE WHERE CC_CC_CODE = '" & Common.Parse(aryDoc.Item(i)(13)) & "' AND CC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
                    aryDoc(i)(15) = objDb.GetVal("SELECT CBM_BRANCH_NAME FROM COMPANY_BRANCH_MSTR WHERE CBM_BRANCH_CODE = '" & Common.Parse(aryDoc.Item(i)(12)) & "' and CBM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")

                    If InStr(aryDoc.Item(i)(8), ":") Then
                        aryDoc.Item(i)(8) = aryDoc.Item(i)(8).Substring(0, InStr(aryDoc.Item(i)(8), ":") - 1)
                    End If

                    'Get the Tax Rate and the Percentage
                    Dim gstTaxRate As String = ""
                    Dim gstPercentage As String
                    GST.getGSTInfobyRate_ForIPP(Common.Parse(aryDoc.Item(i)(22)), gstPercentage, gstTaxRate)
                    'End
                    If gstPercentage = "" Then
                        gstPercentage = "0.00"
                    End If

                    'Jules 2018.04.26 - PAMB Scrum 2 - Added Category and Analysis Codes.
                    'Zulham 21/06/2017
                    'IPP Stage 3
                    strSQL = " INSERT INTO BILLING_DETAILS(" &
                       "BM_INVOICE_NO, BM_S_COY_ID, BM_INVOICE_LINE, BM_PRODUCT_DESC, BM_UOM, BM_RECEIVED_QTY, " &
                       "BM_UNIT_COST, BM_B_GL_CODE, BM_COST_CENTER, BM_COST_CENTER_DESC, " &
                       "BM_BRANCH_CODE, BM_BRANCH_CODE_NAME, " &
                       "BM_GLRULE_CATEGORY, BM_GLRULE_CATEGORY_INDEX,BM_GST_VALUE,BM_GST_INPUT_TAX_CODE,BM_GST_OUTPUT_TAX_CODE,BM_GST_RATE, BM_GST, BM_REF_NO, " &
                       "BM_CATEGORY, BM_ANALYSIS_CODE1, BM_ANALYSIS_CODE2, BM_ANALYSIS_CODE3, BM_ANALYSIS_CODE4, BM_ANALYSIS_CODE5, BM_ANALYSIS_CODE8, BM_ANALYSIS_CODE9) " &
                       "VALUES('" & Common.Parse(Docno) & "'," &
                       "'" & Common.Parse(venidx) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(0)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(3)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(4)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(5)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(6)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(8)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(13)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(14)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(12)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(15)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(18)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(19)) & "'," &
                       "'" & IIf(Common.Parse(aryDoc.Item(i)(21)) = "", 0, aryDoc.Item(i)(21)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(22)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(23)) & "'," &
                       "'" & Common.Parse(gstTaxRate) & "'," &
                       "'" & CDec(gstPercentage) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(2)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(24)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(25)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(26)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(27)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(28)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(29)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(32)) & "'," &
                       "'" & Common.Parse(aryDoc.Item(i)(33)) & "')"

                    Common.Insert2Ary(strAryQuery, strSQL)
                    'CH - 8 Apr 2015 - Why need Update/Insert into INVOICE_DETAILS_ALLOC? Need futher investigation.
                    'If aryDoc.Item(i)(11) <> "" Then
                    '    intCostAllocIndex = CInt(objDb.GetVal("SELECT cam_index FROM cost_alloc_mstr WHERE cam_ca_code = '" & aryDoc.Item(i)(11) & "' AND cam_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"))

                    '    For j = 0 To dsDocLine.Tables(0).Rows.Count - 1

                    '        InvLineAmt = CDbl(Common.Parse(aryDoc.Item(i)(7)))
                    '        Amount = Format(Common.Parse(aryDoc.Item(i)(7)) * (dsDocLine.Tables(0).Rows(j)("CAD_PERCENT") / 100), "#,##0.00")
                    '        ttlAmount = ttlAmount + Amount

                    '        If j = dsDocLine.Tables(0).Rows.Count - 1 And ttlAmount <> InvLineAmt Then
                    '            Amount = Format(Common.Parse(aryDoc.Item(i)(7)) * (dsDocLine.Tables(0).Rows(j)("CAD_PERCENT") / 100), "#,##0.00") _
                    '            + (InvLineAmt - ttlAmount)
                    '        End If

                    '        strSQL = " INSERT INTO invoice_details_alloc(IDA_INVOICE_INDEX, IDA_INVOICE_LINE, IDA_COST_CENTER, IDA_COST_CENTER_DESC, IDA_BRANCH_CODE, IDA_BRANCH_NAME, IDA_PERCENT, IDA_AMOUNT)" & _
                    '                 "VALUES('" & strInvNo & "'," & _
                    '                 "" & aryDoc.Item(i)(0) & "," & _
                    '                 "'" & Common.Parse(dsDocLine.Tables(0).Rows(j)("CAD_CC_CODE")) & "'," & _
                    '                 "'" & Common.Parse(dsDocLine.Tables(0).Rows(j)("CC_CC_DESC")) & "'," & _
                    '                 "'" & Common.Parse(dsDocLine.Tables(0).Rows(j)("CAD_BRANCH_CODE")) & "'," & _
                    '                 "'" & Common.Parse(dsDocLine.Tables(0).Rows(j)("CBM_BRANCH_NAME")) & "'," & _
                    '                 "" & dsDocLine.Tables(0).Rows(j)("CAD_PERCENT") & "," & _
                    '                 "" & Amount & ")"
                    '        Common.Insert2Ary(strAryQuery, strSQL)
                    '    Next
                    'End If
                    'ttlAmount = 0
                End If

            Next

            'CH - 8 Apr 2015 - Why need Update/Insert into INVOICE_MSTR? Need futher investigation.
            'strSQL = "update invoice_mstr set im_ind1 = '" & MasterDoc & "' where im_invoice_index = " & strInvNo
            'Common.Insert2Ary(strAryQuery, strSQL)

            objDb.BatchExecute(strAryQuery)
        End Function
        'Zulham 30-01-2015 IPP-GST Stage 2A
        Public Function GetBillingDoc(ByVal index As Integer) As DataSet

            Dim strSql As String
            Dim dsDoc As New DataSet
            'Modified for IPP GST Stage 2A - CH - 13 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If
            '------------------------------------------------

            strSql = "SELECT bm.*, CM.CODE_DESC AS IM_STATE2,CM2.CODE_DESC AS IM_COUNTRY2 ,ic_bank_code,ic_bank_acct,ic_addr_line1,ic_addr_line2,ic_addr_line3,ic_postcode,ic_city,CM3.CODE_DESC AS IC_STATE,CM4.CODE_DESC AS IC_COUNTRY,IC_PAYMENT_METHOD,IC_COY_NAME,ic_credit_terms FROM billing_mstr bm " & _
            "INNER JOIN CODE_MSTR CM ON CM.CODE_ABBR = bm.bM_STATE AND CM.CODE_CATEGORY = 'S' " & _
            "INNER JOIN CODE_MSTR CM2 ON CM2.CODE_ABBR = bm.bM_COUNTRY AND CM2.CODE_CATEGORY = 'CT' " & _
            "INNER JOIN ipp_company ON bm_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "' " & _
                      "INNER JOIN CODE_MSTR CM3 ON CM3.CODE_ABBR = IC_STATE AND CM3.CODE_CATEGORY = 'S' " & _
            "INNER JOIN CODE_MSTR CM4 ON CM4.CODE_ABBR = IC_COUNTRY AND CM4.CODE_CATEGORY = 'CT' " & _
                "WHERE bm_invoice_index  =  '" & index & "' and bm_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"


            dsDoc = objDb.FillDs(strSql)
            GetBillingDoc = dsDoc

        End Function
        'Zulham 30-01-2015 IPP-GST Stage 2A
        Public Function GetBillingDocDetails(ByVal strDocNo As String, ByVal VenIdx As Integer, Optional ByVal lineno As String = "", Optional ByVal oldstrDocNo As String = "") As DataSet

            Dim strCoyId As String
            'Zulham 04/03/2015 IPP-GST Stage 2A
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strCoyId = HttpContext.Current.Session("CompanyID")
            Else
                strCoyId = strDefIPPCompID
            End If

            Dim strSql As String
            Dim dsDoc As New DataSet
            If lineno <> "" Then
                'Jules 2018.05.02 - PAMB Scrum 2 - Added Analysis Code and Description.
                strSql = "SELECT billing_details.*,(bm_RECEIVED_QTY * BM_UNIT_COST) AS BM_Amount,cbg_b_gl_desc,IF(BILLING_details.BM_BRANCH_CODE = '' AND BILLING_details.BM_BRANCH_CODE_NAME = '','', CONCAT(BILLING_details.BM_BRANCH_CODE,':',BILLING_details.BM_BRANCH_CODE_NAME)) AS BM_BRANCH_CODE_2, IF(BILLING_details.BM_COST_CENTER = '' AND BILLING_details.BM_COST_CENTER_DESC = '','',CONCAT(BILLING_details.BM_COST_CENTER,':',BILLING_details.BM_COST_CENTER_DESC)) AS BM_COST_CENTER_2 " &
                    ",ac1.AC_ANALYSIS_CODE_DESC 'AC_ANALYSIS_CODE1',ac2.AC_ANALYSIS_CODE_DESC 'AC_ANALYSIS_CODE2',ac3.AC_ANALYSIS_CODE_DESC 'AC_ANALYSIS_CODE3',ac4.AC_ANALYSIS_CODE_DESC 'AC_ANALYSIS_CODE4',ac5.AC_ANALYSIS_CODE_DESC 'AC_ANALYSIS_CODE5',ac8.AC_ANALYSIS_CODE_DESC 'AC_ANALYSIS_CODE8',ac9.AC_ANALYSIS_CODE_DESC 'AC_ANALYSIS_CODE9'" &
                    "FROM BILLING_details " &
                    "INNER JOIN company_b_gl_code ON cbg_b_gl_code = BM_b_gl_code And cbg_b_coy_id = '" & Common.Parse(strCoyId) & "' " &
                    "INNER JOIN analysis_code ac1 ON ac1.ac_b_coy_id = cbg_b_coy_id AND ac1.ac_analysis_code = BILLING_details.BM_ANALYSIS_CODE1 " &
                    "INNER JOIN analysis_code ac2 ON ac2.ac_b_coy_id = cbg_b_coy_id AND ac2.ac_analysis_code = BILLING_details.BM_ANALYSIS_CODE2 " &
                    "INNER JOIN analysis_code ac3 ON ac3.ac_b_coy_id = cbg_b_coy_id AND ac3.ac_analysis_code = BILLING_details.BM_ANALYSIS_CODE3 " &
                    "INNER JOIN analysis_code ac4 ON ac4.ac_b_coy_id = cbg_b_coy_id AND ac4.ac_analysis_code = BILLING_details.BM_ANALYSIS_CODE4 " &
                    "INNER JOIN analysis_code ac5 ON ac5.ac_b_coy_id = cbg_b_coy_id AND ac5.ac_analysis_code = BILLING_details.BM_ANALYSIS_CODE5 " &
                    "INNER JOIN analysis_code ac8 ON ac8.ac_b_coy_id = cbg_b_coy_id AND ac8.ac_analysis_code = BILLING_details.BM_ANALYSIS_CODE8 " &
                    "INNER JOIN analysis_code ac9 ON ac9.ac_b_coy_id = cbg_b_coy_id AND ac9.ac_analysis_code = BILLING_details.BM_ANALYSIS_CODE9 " &
                    "WHERE BM_invoice_no  =  '" & oldstrDocNo & "' and BM_s_coy_id = " & VenIdx & " and BM_invoice_line = '" & lineno & "'"

            Else
                'Jules 2018.05.02 - PAMB Scrum 2 - Added Analysis Code and Description.
                strSql = "SELECT BILLING_details.*,(BM_RECEIVED_QTY * BM_UNIT_COST) AS BM_Amount,cbg_b_gl_desc,IF(BILLING_details.BM_BRANCH_CODE = '' AND BILLING_details.BM_BRANCH_CODE_NAME = '','', CONCAT(BILLING_details.BM_BRANCH_CODE,':',BILLING_details.BM_BRANCH_CODE_NAME)) AS BM_BRANCH_CODE_2, IF(BILLING_details.BM_COST_CENTER = '' AND BILLING_details.BM_COST_CENTER_DESC = '','',CONCAT(BILLING_details.BM_COST_CENTER,':',BILLING_details.BM_COST_CENTER_DESC)) AS BM_COST_CENTER_2 " &
                    ",ac1.AC_ANALYSIS_CODE_DESC 'AC_ANALYSIS_CODE1',ac2.AC_ANALYSIS_CODE_DESC 'AC_ANALYSIS_CODE2',ac3.AC_ANALYSIS_CODE_DESC 'AC_ANALYSIS_CODE3',ac4.AC_ANALYSIS_CODE_DESC 'AC_ANALYSIS_CODE4',ac5.AC_ANALYSIS_CODE_DESC 'AC_ANALYSIS_CODE5',ac8.AC_ANALYSIS_CODE_DESC 'AC_ANALYSIS_CODE8',ac9.AC_ANALYSIS_CODE_DESC 'AC_ANALYSIS_CODE9'" &
                    "FROM BILLING_details " &
                    "INNER JOIN company_b_gl_code ON cbg_b_gl_code = BM_b_gl_code And cbg_b_coy_id = '" & Common.Parse(strCoyId) & "' " &
                    "INNER JOIN analysis_code ac1 ON ac1.ac_b_coy_id = cbg_b_coy_id AND ac1.ac_analysis_code = BILLING_details.BM_ANALYSIS_CODE1 " &
                    "INNER JOIN analysis_code ac2 ON ac2.ac_b_coy_id = cbg_b_coy_id AND ac2.ac_analysis_code = BILLING_details.BM_ANALYSIS_CODE2 " &
                    "INNER JOIN analysis_code ac3 ON ac3.ac_b_coy_id = cbg_b_coy_id AND ac3.ac_analysis_code = BILLING_details.BM_ANALYSIS_CODE3 " &
                    "INNER JOIN analysis_code ac4 ON ac4.ac_b_coy_id = cbg_b_coy_id AND ac4.ac_analysis_code = BILLING_details.BM_ANALYSIS_CODE4 " &
                    "INNER JOIN analysis_code ac5 ON ac5.ac_b_coy_id = cbg_b_coy_id AND ac5.ac_analysis_code = BILLING_details.BM_ANALYSIS_CODE5 " &
                    "INNER JOIN analysis_code ac8 ON ac8.ac_b_coy_id = cbg_b_coy_id AND ac8.ac_analysis_code = BILLING_details.BM_ANALYSIS_CODE8 " &
                    "INNER JOIN analysis_code ac9 ON ac9.ac_b_coy_id = cbg_b_coy_id AND ac9.ac_analysis_code = BILLING_details.BM_ANALYSIS_CODE9 " &
                    "WHERE BM_invoice_no  =  '" & oldstrDocNo & "' and BM_s_coy_id = " & VenIdx & ""

            End If

            dsDoc = objDb.FillDs(strSql)
            GetBillingDocDetails = dsDoc

        End Function
        'Zulham 30-01-2015 IPP-GST Stage 2A
        Public Function GetBillingDetailTotalAmt(ByVal strDocNo As String, ByVal VenIdx As Integer) As String
            Return objDb.Get1ColumnCheckNull("billing_details", "SUM(bm_RECEIVED_QTY * bm_UNIT_COST) AS bm_Amount", " WHERE bm_invoice_no  =  '" & strDocNo & "' and bm_s_coy_id = " & VenIdx & "")
        End Function
        'Zulham 30-01-2015 IPP-GST Stage 2A
        Public Function getBillingStatus(ByRef strDocNo As String) As Integer
            Dim strsql As String
            Dim ds As New DataSet
            Dim intStatus As Integer

            strsql = "SELECT BM_INVOICE_STATUS FROM BILLING_MSTR WHERE BM_INVOICE_INDEX = '" & strDocNo & "' AND BM_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'"
            intStatus = CInt(objDb.GetVal(strsql))

            Return intStatus
        End Function
        'Zulham 30-01-2015 IPP-GST Stage 2A
        Public Function getBillingApprovalWorkflowList() As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            strsql = "SELECT DISTINCT AGB_GRP_INDEX ,AGM_GRP_NAME "
            strsql &= "FROM APPROVAL_GRP_BUYER "
            strsql &= "INNER JOIN APPROVAL_GRP_MSTR ON  AGB_GRP_INDEX = AGM_GRP_INDEX AND AGM_TYPE='BIL' "
            strsql &= "INNER JOIN approval_grp_ao ON aga_grp_index = AGB_GRP_INDEX "
            strsql &= "WHERE agb_buyer = '" & HttpContext.Current.Session("UserId") & "' "
            strsql &= "AND AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'" 'IPP Gst Stage 2A - CH - 9 Mar 2015

            ds = objDb.FillDs(strsql)
            getBillingApprovalWorkflowList = ds
        End Function
        'Zulham 30-01-2015 IPP-GST Stage 2A
        Public Function SaveBillingDoc(ByRef dsDoc As DataSet, ByRef strfrm As String, ByRef strAction As String, ByRef intInvIdx As Integer, ByRef strOldDocNo As String, Optional ByVal intApprGrpIdx As String = "") As Boolean
            Dim i As Integer
            Dim strSQL As String
            Dim strPayFor As String
            Dim dsDocLine As New DataSet
            Dim strInvNo As String
            Dim Amount, ttlAmount, InvLineAmt As Decimal
            Dim j As Integer
            Dim intCostAllocIndex As Integer
            Dim strAryQuery(0) As String, boolAryQuery(0) As String
            Dim strUserDept As String
            Dim strStatus As String
            Dim strIPPO, strIPPOF As String
            Dim blnIPPO, blnIPPOF As Boolean
            Dim objUsers As New Users

            strIPPO = System.Enum.GetName(GetType(FixedRole), FixedRole.IPP_Officer)
            strIPPOF = System.Enum.GetName(GetType(FixedRole), FixedRole.IPP_OfficerF)
            strIPPO = "'" & Replace(strIPPO, "_", " ") & "'"
            strIPPOF = "'" & Replace(strIPPOF, strIPPOF, "IPP Officer(F)") & "'"

            blnIPPO = objUsers.checkUserFixedRole(strIPPO)
            blnIPPOF = objUsers.checkUserFixedRole(strIPPOF)
            Dim dsApprGrp As New DataSet
            Dim intApprType As Integer

            If intInvIdx = 0 Then
                intInvIdx = objDb.GetVal("SELECT bm_invoice_index FROM billing_mstr WHERE bm_invoice_no = '" & strOldDocNo & "' and bm_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND bM_S_COY_ID = '" & dsDoc.Tables(0).Rows(0)("VenCompIDX") & "'")
            End If
            SubmitBillingDoc(Common.parseNull(intInvIdx), , strAryQuery, blnIPPO, blnIPPOF)
            SaveBILLINGApproval(Common.parseNull(intInvIdx), dsDoc, strAryQuery, intApprGrpIdx)

            'Dim IppMain As New IPPMain
            'IppMain.AddAuditTrailRecordInsert2Ary(Common.parseNull(intInvIdx), "Billing Teller", strAryQuery, "Submit Document")
            'IPP Gst Stage 2A - CH - 13 Feb 2015
            AddBillAuditTrailRecordInsert2Ary(Common.parseNull(intInvIdx), "Billing Teller", strAryQuery, "Submit Document")
            'End If

            If objDb.BatchExecute(strAryQuery) Then
                Return True
            Else
                Return False
            End If

        End Function
        'Zulham 30-01-2015 IPP-GST Stage 2A
        Public Function SubmitBillingDoc(ByVal strIPPDocNo As String, Optional ByVal dsDoc As DataSet = Nothing, Optional ByRef pQuery() As String = Nothing, Optional ByVal blnIPPO As Boolean = False, Optional ByVal blnIPPOF As Boolean = False) As Boolean

            Dim strSQL As String
            Dim strInvNo As String

            If dsDoc Is Nothing Then
                strSQL = "UPDATE BILLING_MSTR SET BM_INVOICE_STATUS = 2,BM_SUBMIT_DATE=NOW(), " &
                        "BM_STATUS_CHANGED_BY ='" & HttpContext.Current.Session("UserId") & "'," &
                        "BM_STATUS_CHANGED_ON = NOW()" &
                        "WHERE BM_INVOICE_INDEX = '" & strIPPDocNo & "' AND BM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
            End If
            Common.Insert2Ary(pQuery, strSQL)
            Return True

        End Function
        'Zulham 30-01-2015 IPP-GST Stage 2A
        Public Function SaveBILLINGApproval(ByVal intInvIdx As String, ByVal dsDoc As DataSet, ByRef pQuery() As String, Optional ByVal intApprGrpIdx As String = "")
            Dim i As Integer
            Dim strSQL As String
            Dim dsApprGrp As New DataSet
            Dim intApprType As Integer
            strSQL = "SELECT * FROM approval_grp_ao " &
                     "WHERE aga_grp_index  =  '" & intApprGrpIdx & "'"
            '"UNION ALL " & _
            '"SELECT * FROM approval_grp_ao " & _
            '"WHERE aga_grp_index  =  '" & intApprGrpIdx & "' AND AGA_TYPE = 'FO' " & _
            '"UNION ALL " & _
            '"SELECT * FROM approval_grp_ao " & _
            '"WHERE aga_grp_index  =  '" & intApprGrpIdx & "' AND AGA_TYPE = 'FM' "

            dsApprGrp = objDb.FillDs(strSQL)

            Dim blnChk As Boolean = False

            If objDb.Exist("SELECT '*' FROM Billing_APPROVAL WHERE bA_bill_INDEX = '" & intInvIdx & "'") > 0 Then
                strSQL = "DELETE FROM Billing_APPROVAL WHERE bA_bill_INDEX = '" & intInvIdx & "'"
                Common.Insert2Ary(pQuery, strSQL)

            End If

            For i = 0 To dsApprGrp.Tables(0).Rows.Count - 1
                strSQL = " INSERT INTO billing_approval (BA_BILL_INDEX,bA_AO,BA_A_AO,BA_SEQ,BA_AO_ACTION,BA_APPROVAL_TYPE,BA_APPROVAL_GRP_INDEX,BA_RELIEF_IND)" &
                        "VALUES('" & intInvIdx & "'," &
                         "'" & dsApprGrp.Tables(0).Rows(i)("AGA_AO") & "'," &
                         "'" & dsApprGrp.Tables(0).Rows(i)("AGA_A_AO") & "'," &
                         "" & i + 1 & "," &
                         "0 ," & intApprType & "," &
                         "'" & dsApprGrp.Tables(0).Rows(i)("AGA_GRP_INDEX") & "'," &
                         "'" & dsApprGrp.Tables(0).Rows(i)("AGA_RELIEF_IND") & "')"

                Common.Insert2Ary(pQuery, strSQL)
            Next
        End Function
        'Zulham 30-01-2015 IPP-GST Stage 2A
        Public Function getBillingApprovalList(ByVal docno As String, ByVal doctype As String, ByVal startdt As String, ByVal enddt As String, ByVal vendor As String) As DataSet
            Dim sql As String
            Dim ds As New DataSet
            Dim deptCode = objDb.GetVal("select um_dept_id from user_mstr where um_user_id='" & HttpContext.Current.Session("UserID") & "' and um_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")

            sql = "SELECT * FROM billing_mstr LEFT JOIN billing_approval ON bm_invoice_index = ba_bill_index " &
            "LEFT JOIN user_mstr ON um_user_id = bm_created_by AND um_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' " &
            "WHERE bm_b_coy_id='" & HttpContext.Current.Session("CompanyId") & "' AND bm_invoice_status IN ('2') AND " &
            "(ba_ao = '" & HttpContext.Current.Session("UserId") & "' or ba_a_ao = '" & HttpContext.Current.Session("UserId") & "' ) AND ba_ao_action = (ba_seq - 1) " '& _
            '"AND um_dept_id = ( SELECT um_dept_id FROM user_mstr WHERE um_user_id = '" & HttpContext.Current.Session("UserId") & "' and um_coy_id = '" & HttpContext.Current.Session("CompanyId") & "')"

            If docno <> "" Then
                sql &= " AND BM_INVOICE_NO LIKE '%" & docno & "%'"
            End If
            If doctype <> "" Then
                sql &= " AND BM_INVOICE_TYPE = '" & doctype & "'"
            End If
            If startdt <> "" And enddt <> "" Then
                sql &= " AND BM_SUBMIT_DATE >= " & Common.ConvertDate(startdt) & " AND BM_SUBMIT_DATE <= " & Common.ConvertDate(enddt)
            End If
            If vendor <> "" Then
                sql &= " AND BM_S_COY_NAME LIKE '%" & vendor & "%'"
            End If
            ds = objDb.FillDs(sql)
            Return ds
        End Function
        'Zulham 30-01-2015 IPP-GST Stage 2A
        Public Function getApprBillingDetail(ByVal docno As String, ByVal index As String, ByVal compid As String) As DataSet
            Dim sql As String
            Dim ds As New DataSet
            'Modified for IPP GST Stage 2A - CH - 13 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If
            '------------------------------------------------

            sql = "SELECT BM.*, BD.*, CBGL.CBG_B_GL_DESC ,ipp_company.ic_bank_code, ipp_company.ic_bank_acct, " &
            "IF(bd.bm_BRANCH_CODE='' AND bd.bm_BRANCH_CODE_NAME = '','',CONCAT(bd.bm_BRANCH_CODE,':',bD.bm_BRANCH_CODE_NAME)) AS bm_BRANCH_CODE_2,  " &
            "IF(bd.bm_COST_CENTER='' AND bd.bm_COST_CENTER_DESC = '','',CONCAT(bD.bm_COST_CENTER,':',bD.bm_COST_CENTER_DESC)) AS bm_COST_CENTER_2  " &
            "FROM BILLING_MSTR BM, " &
            "ipp_company,BILLING_DETAILS BD LEFT JOIN company_b_gl_code CBGL ON CBGL.CBG_B_GL_CODE = bd.BM_B_GL_CODE WHERE bd.BM_INVOICE_NO= " &
            "BM.BM_INVOICE_NO " &
            "AND BM.BM_S_COY_ID=BD.BM_S_COY_ID AND cbg_b_coy_id ='" & strDefIPPCompID & "'AND ic_index = bm.bm_s_coy_id AND ic_coy_id = '" & strDefIPPCompID & "' " &
            "AND bm.bM_INVOICE_NO='" & docno & "' AND bd.bm_INVOICE_NO='" & docno & "' AND bm.bM_B_COY_ID='" & compid & "' AND bm.bM_INVOICE_INDEX = '" & index & "'"

            ds = objDb.FillDs(sql)
            Return ds
        End Function
        'Zulham 30-01-2015 IPP-GST Stage 2A
        Function getBillingApprFlow(ByVal strBillIndex As String, ByVal strCompanyId As String) As DataSet
            Dim strSql As String
            Dim intIndex As String
            Dim ds As DataSet
            'Dim strCoyId As String = HttpContext.Current.Session("CompanyId")
            'Dim strDeptIdx As String
            'Dim strInvStatus As String
            'Dim currentfaaction As Integer
            'Dim currentseq As String
            'Dim role As String
            'Dim blnAO As Boolean
            'Dim strAOAction As String

            'IPP Gst Stage 2A - CH - 13 Feb 2015
            strSql = "SELECT BM_INDEX FROM BILLING_TRANS_LOG " &
                    "WHERE LEFT(BM_REMARKS, 6) = 'Submit' AND BM_INVOICE_INDEX = '" & strBillIndex & "' " &
                    "ORDER BY BM_INDEX DESC LIMIT 1"
            If objDb.Exist(strSql) > 0 Then
                intIndex = objDb.GetVal(strSql)
            Else
                intIndex = 0
            End If

            strSql = "SELECT BM_PERFORMED_BY AS ACTION, BM_USER_ID AS FA_ACTIVE_AO, BM_TRANS_DATE AS FA_ACTION_DATE, BM_REMARKS AS FA_AO_REMARK " &
                    "FROM BILLING_TRANS_LOG " &
                    "WHERE BM_INVOICE_INDEX = '" & strBillIndex & "' AND BM_INDEX >= " & intIndex
            ds = objDb.FillDs(strSql)
            Return ds
            'Dim strIPPOF As String
            'Dim blnIPPOF As Boolean
            'Dim objUsers As New Users
            'Dim strUserID As String

            'strIPPOF = System.Enum.GetName(GetType(FixedRole), FixedRole.IPP_Officer)
            'strIPPOF = "'" & Replace(strIPPOF, strIPPOF, "IPP Officer") & "'"
            'blnIPPOF = objUsers.checkUserFixedRole(strIPPOF)

            'currentfaaction = objDb.GetVal("SELECT DISTINCT ba_ao_action FROM billing_approval WHERE ba_bill_index = '" & intInvoiceNo & "' ")
            'currentseq = objDb.Get1Column("billing_APPROVAL", "BA_SEQ", " WHERE BA_BILL_INDEX='" & intInvoiceNo & "' and Ba_ao = '" & HttpContext.Current.Session("UserId") & "' and ba_seq = '" & currentfaaction + 1 & "'")
            'role = "FO" 'objDb.Get1Column("billing_APPROVAL", "FA_AGA_TYPE", " WHERE BA_BILL_INDEX='" & intInvoiceNo & "' and ba_seq = '" & currentfaaction + 1 & "'")
            'blnAO = objDb.Exist("SELECT * FROM billing_APPROVAL WHERE ba_bill_index = '" & intInvoiceNo & "'") ' AND ba_aga_type = 'AO'")
            'strAOAction = objDb.Get1Column("billing_APPROVAL", "bA_AO_ACTION", " WHERE bA_bill_INDEX='" & intInvoiceNo & "'") ' AND ba_aga_type = 'AO'")
            'If currentseq = "" Then
            '    currentseq = objDb.Get1Column("billing_APPROVAL", "bA_SEQ", " WHERE bA_bill_INDEX='" & intInvoiceNo & "' and bA_A_AO = '" & HttpContext.Current.Session("UserId") & "' and ba_seq = '" & currentfaaction + 1 & "'")
            'End If

            'strsql3 = "SELECT bm_invoice_status FROM billing_mstr " & _
            '      "WHERE bm_invoice_index = '" & intInvoiceNo & "'"

            'strInvStatus = objDb.GetVal(strsql3)

            'If blnAO = False Then 'for finance user approval workflow panel
            '    strSql = "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " & _
            '            "'Finance Verifier' AS 'Action', " & _
            '            "FA.FA_SEQ + 1 AS FA_SEQ2 " & _
            '            "FROM FINANCE_APPROVAL FA " & _
            '            "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "', INVOICE_MSTR " & _
            '            "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'  AND fa_action_date IS NOT NULL " & _
            '            "AND fa_aga_type = 'FO' " & _
            '                "UNION ALL " & _
            '            "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " & _
            '            "'Finance Approver' AS 'Action', " & _
            '            "FA.FA_SEQ + 1 AS FA_SEQ2 " & _
            '            "FROM FINANCE_APPROVAL FA " & _
            '            "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "', INVOICE_MSTR " & _
            '            "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'  AND fa_action_date IS NOT NULL " & _
            '            "AND fa_aga_type = 'FM' " & _
            '                "UNION ALL " & _
            '            "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 0 AS FA_SEQ, " & _
            '            "im_created_by AS FA_ACTIVE_AO, im_created_on AS FA_ACTION_DATE, " & _
            '            "(SELECT DISTINCT fa_ao_action FROM finance_approval " & _
            '            "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Submit Document' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " & _
            '            "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'Finance Teller' AS 'Action',1 AS FA_SEQ2 " & _
            '            "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_created_by=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
            '            "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_created_on IS NOT NULL  ORDER BY FA_SEQ2 "

            'Else 'for source dept approval workflow panel
            '    If strInvStatus = "17" Then
            '        strSql = "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " & _
            '                 "'IPP Verifier' AS 'Action', " & _
            '                 "FA.FA_SEQ + 1 AS FA_SEQ2 " & _
            '                 "FROM FINANCE_APPROVAL FA " & _
            '                 "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "', INVOICE_MSTR " & _
            '                 "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'  AND fa_action_date IS NOT NULL " & _
            '                 "AND fa_aga_type = 'AO' " & _
            '                     "UNION ALL " & _
            '                 "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " & _
            '                 "'Finance Verifier' AS 'Action', " & _
            '                 "FA.FA_SEQ + 2 AS FA_SEQ2 " & _
            '                 "FROM FINANCE_APPROVAL FA " & _
            '                 "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "', INVOICE_MSTR " & _
            '                 "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'  AND fa_action_date IS NOT NULL " & _
            '                 "AND fa_aga_type = 'FO' " & _
            '                      "UNION ALL " & _
            '                 "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " & _
            '                 "'Finance Approver' AS 'Action', " & _
            '                 "FA.FA_SEQ + 2 AS FA_SEQ2 " & _
            '                 "FROM FINANCE_APPROVAL FA " & _
            '                 "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "', INVOICE_MSTR " & _
            '                 "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'  AND fa_action_date IS NOT NULL " & _
            '                 "AND fa_aga_type = 'FM' " & _
            '                      "UNION ALL " & _
            '                 "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 0 AS FA_SEQ, " & _
            '                 "im_created_by AS FA_ACTIVE_AO, im_created_on AS FA_ACTION_DATE, " & _
            '                 "(SELECT DISTINCT fa_ao_action FROM finance_approval " & _
            '                 "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Submit Document' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " & _
            '                 "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'IPP Teller' AS 'Action',1 AS FA_SEQ2 " & _
            '                 "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_created_by=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
            '                 "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_created_on IS NOT NULL  " & _
            '                        "UNION ALL " & _
            '                 "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 3 AS FA_SEQ, " & _
            '                 "im_prcs_sent_id AS FA_ACTIVE_AO, im_prcs_sent_upd_date AS FA_ACTION_DATE, " & _
            '                 "(SELECT DISTINCT fa_ao_action FROM finance_approval " & _
            '                 "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Update PSD Sent Date.' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " & _
            '                 "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,IF(uU_usrgrp_id = 'IPP Approving Officer','IPP Verifier','IPP Teller') AS 'Action',3 AS FA_SEQ2 " & _
            '                 "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_prcs_sent_id=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
            '                 "JOIN USERS_USRGRP ON uu_user_id = UMA.UM_USER_ID AND uu_coy_id = UM_COY_ID AND uU_usrgrp_id LIKE '%officer%' " & _
            '                 "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_prcs_sent_upd_date IS NOT NULL " & _
            '                 "ORDER BY FA_SEQ2 "

            '    ElseIf strInvStatus = "11" Or strInvStatus = "12" Or strInvStatus = "13" Or strInvStatus = "4" Then
            '        strSql = "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " & _
            '                 "'IPP Verifier' AS 'Action', " & _
            '                 "FA.FA_SEQ + 1 AS FA_SEQ2 " & _
            '                 "FROM FINANCE_APPROVAL FA " & _
            '                 "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "', INVOICE_MSTR " & _
            '                 "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'  AND fa_action_date IS NOT NULL " & _
            '                 "AND fa_aga_type = 'AO' " & _
            '                     "UNION ALL " & _
            '                 "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " & _
            '                 "'Finance Verifier' AS 'Action', " & _
            '                 "FA.FA_SEQ + 3 AS FA_SEQ2 " & _
            '                 "FROM FINANCE_APPROVAL FA " & _
            '                 "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "', INVOICE_MSTR " & _
            '                 "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'  AND fa_action_date IS NOT NULL " & _
            '                 "AND fa_aga_type = 'FO' " & _
            '                      "UNION ALL " & _
            '                 "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " & _
            '                 "'Finance Approver' AS 'Action', " & _
            '                 "FA.FA_SEQ + 3 AS FA_SEQ2 " & _
            '                 "FROM FINANCE_APPROVAL FA " & _
            '                 "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "', INVOICE_MSTR " & _
            '                 "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'  AND fa_action_date IS NOT NULL " & _
            '                 "AND fa_aga_type = 'FM' " & _
            '                      "UNION ALL " & _
            '                 "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 0 AS FA_SEQ, " & _
            '                 "im_created_by AS FA_ACTIVE_AO, im_created_on AS FA_ACTION_DATE, " & _
            '                 "(SELECT DISTINCT fa_ao_action FROM finance_approval " & _
            '                 "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Submit Document' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " & _
            '                 "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'IPP Teller' AS 'Action',1 AS FA_SEQ2 " & _
            '                 "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_created_by=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
            '                 "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_created_on IS NOT NULL  " & _
            '                        "UNION ALL " & _
            '                 "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 3 AS FA_SEQ, " & _
            '                 "im_prcs_sent_id AS FA_ACTIVE_AO, im_prcs_sent_upd_date AS FA_ACTION_DATE, " & _
            '                 "(SELECT DISTINCT fa_ao_action FROM finance_approval " & _
            '                 "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Update PSD Sent Date.' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " & _
            '                 "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,IF(uU_usrgrp_id = 'IPP Approving Officer','IPP Verifier','IPP Teller') AS 'Action',3 AS FA_SEQ2 " & _
            '                 "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_prcs_sent_id=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
            '                 "JOIN USERS_USRGRP ON uu_user_id = UMA.UM_USER_ID AND uu_coy_id = UM_COY_ID AND uU_usrgrp_id LIKE '%officer%' " & _
            '                 "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_prcs_sent_upd_date IS NOT NULL " & _
            '                    "UNION ALL " & _
            '                 "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 4 AS FA_SEQ, " & _
            '                 "im_prcs_recv_id AS FA_ACTIVE_AO, im_prcs_recv_upd_date AS FA_ACTION_DATE, " & _
            '                 "(SELECT DISTINCT fa_ao_action FROM finance_approval " & _
            '                 "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Update PSD Received Date.' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " & _
            '                 "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'Finance Teller' AS 'Action',4 AS FA_SEQ2 " & _
            '                 "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_prcs_recv_id=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
            '                 "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_prcs_recv_upd_date IS NOT NULL " & _
            '                 "ORDER BY FA_SEQ2 "


            '    ElseIf strInvStatus = "2" And (role = "FO" Or role = "FM") Then
            '        strSql = "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 0 AS FA_SEQ, " & _
            '                 "im_created_by AS FA_ACTIVE_AO, im_created_on AS FA_ACTION_DATE, " & _
            '                 "(SELECT DISTINCT fa_ao_action FROM finance_approval " & _
            '                 "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Submit Document' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " & _
            '                 "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'IPP Teller' AS 'Action',1 AS FA_SEQ2 " & _
            '                 "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_created_by=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
            '                 "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_created_on IS NOT NULL  " & _
            '                 "ORDER BY FA_SEQ2 "

            '        'AND im_prcs_recv_upd_date IS NOT NULL 
            '    ElseIf strInvStatus = "14" And role = "AO" And strAOAction <> "0" Then
            '        strSql = "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " & _
            '                 "'IPP Verifier' AS 'Action', " & _
            '                 "FA.FA_SEQ + 1 AS FA_SEQ2 " & _
            '                 "FROM FINANCE_APPROVAL FA " & _
            '                 "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "', INVOICE_MSTR " & _
            '                 "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'  AND fa_action_date IS NOT NULL " & _
            '                 "AND fa_aga_type = 'AO' " & _
            '                        "UNION ALL " & _
            '                 "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " & _
            '                 "'Finance Verifier' AS 'Action', " & _
            '                 "FA.FA_SEQ + 2 AS FA_SEQ2 " & _
            '                 "FROM FINANCE_APPROVAL FA " & _
            '                 "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "', INVOICE_MSTR " & _
            '                 "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'  AND fa_action_date IS NOT NULL " & _
            '                 "AND fa_aga_type = 'FO' " & _
            '                        "UNION ALL " & _
            '                 "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " & _
            '                 "'Finance Approver' AS 'Action', " & _
            '                 "FA.FA_SEQ + 2 AS FA_SEQ2 " & _
            '                 "FROM FINANCE_APPROVAL FA " & _
            '                 "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "', INVOICE_MSTR " & _
            '                 "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'  AND fa_action_date IS NOT NULL " & _
            '                 "AND fa_aga_type = 'FM' " & _
            '                        "UNION ALL " & _
            '                 "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 0 AS FA_SEQ, " & _
            '                 "im_created_by AS FA_ACTIVE_AO, im_created_on AS FA_ACTION_DATE, " & _
            '                 "(SELECT DISTINCT fa_ao_action FROM finance_approval " & _
            '                 "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Submit Document' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " & _
            '                 "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'IPP Teller' AS 'Action',1 AS FA_SEQ2 " & _
            '                 "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_created_by=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
            '                 "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_created_on IS NOT NULL  " & _
            '                        "UNION ALL " & _
            '                 "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 3 AS FA_SEQ,  " & _
            '                 "itl_user_id AS FA_ACTIVE_AO, itl_trans_date AS FA_ACTION_DATE,  " & _
            '                 "(SELECT DISTINCT fa_ao_action FROM finance_approval  " & _
            '                 "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,itl_remarks AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index,  " & _
            '                 "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'Finance Teller' AS 'Action',3 AS FA_SEQ2  " & _
            '                 "FROM(invoice_mstr) " & _
            '                "LEFT OUTER JOIN ipp_trans_log ON itl_invoice_index = im_invoice_index AND itl_index = (SELECT MAX(itl_index) FROM ipp_trans_log WHERE itl_invoice_index = '" & intInvoiceNo & "' AND itl_performed_by = 'Finance Teller') " & _
            '                "LEFT OUTER JOIN USER_MSTR UMA ON itl_user_id=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
            '                "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' " & _
            '                 "ORDER BY FA_SEQ2 "
            '    ElseIf strInvStatus = "14" And role = "AO" And strAOAction = "0" Then
            '        Dim chkLastActionUser As String

            '        chkLastActionUser = objDb.GetVal("SELECT ITL_PERFORMED_BY FROM ipp_trans_log WHERE itl_invoice_index = '" & intInvoiceNo & "' AND " & _
            '        "itl_index = (SELECT MAX(itl_index) FROM ipp_trans_log WHERE itl_invoice_index = '" & intInvoiceNo & "')")

            '        If chkLastActionUser = "Finance Teller" Then
            '            strSql = "SELECT itl_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4,  " & _
            '                    "FA_SEQ,itl_user_id  AS FA_ACTIVE_AO,itl_trans_date AS FA_ACTION_DATE, " & _
            '                    "(SELECT DISTINCT fa_ao_action FROM finance_approval WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION, itl_remarks AS FA_AO_REMARK, " & _
            '                                            "'' AS fa_approval_type, '' AS fa_approval_grp_index, '' AS fa_on_behalfof,'' AS fa_relief_ind, " & _
            '                    "UM_USER_NAME AS AO_NAME, 'IPP Verifier' AS 'Action',FINANCE_APPROVAL.FA_SEQ + 1 AS FA_SEQ2   " & _
            '                                            "FROM(ipp_trans_log) " & _
            '                    "LEFT OUTER JOIN USER_MSTR ON itl_user_id=UM_USER_ID AND UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
            '                    "LEFT JOIN FINANCE_APPROVAL ON itl_invoice_index = fa_invoice_index AND fa_aga_type = 'AO' " & _
            '                    "WHERE itl_index = (SELECT MAX(itl_index) FROM ipp_trans_log WHERE itl_invoice_index = '" & intInvoiceNo & "' AND itl_performed_by = 'IPP Verifier') " & _
            '                                            "UNION ALL  " & _
            '                    "SELECT itl_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, " & _
            '                    "0 AS FA_SEQ,itl_user_id  AS FA_ACTIVE_AO,itl_trans_date AS FA_ACTION_DATE, " & _
            '                    "(SELECT DISTINCT fa_ao_action FROM finance_approval WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION, itl_remarks AS FA_AO_REMARK, " & _
            '                                            "'' AS fa_approval_type, '' AS fa_approval_grp_index, '' AS fa_on_behalfof,'' AS fa_relief_ind, " & _
            '                    "UM_USER_NAME AS AO_NAME, 'IPP Teller' AS 'Action',3 AS FA_SEQ2   " & _
            '                                            "FROM(ipp_trans_log) " & _
            '                    "LEFT OUTER JOIN USER_MSTR ON itl_user_id=UM_USER_ID AND UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
            '                    "WHERE itl_index = (SELECT MAX(itl_index) FROM ipp_trans_log WHERE itl_invoice_index = '" & intInvoiceNo & "' AND itl_performed_by = 'IPP Teller') " & _
            '                     "UNION ALL " & _
            '                    "SELECT itl_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, " & _
            '                    "0 AS FA_SEQ,itl_user_id  AS FA_ACTIVE_AO,itl_trans_date AS FA_ACTION_DATE, " & _
            '                    "(SELECT DISTINCT fa_ao_action FROM finance_approval WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION, itl_remarks AS FA_AO_REMARK, " & _
            '                                            "'' AS fa_approval_type, '' AS fa_approval_grp_index, '' AS fa_on_behalfof,'' AS fa_relief_ind, " & _
            '                    "UM_USER_NAME AS AO_NAME, 'Finance Teller' AS 'Action',4 AS FA_SEQ2  " & _
            '                                            "FROM(ipp_trans_log) " & _
            '                    "LEFT OUTER JOIN USER_MSTR ON itl_user_id=UM_USER_ID AND UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
            '                    "WHERE itl_index = (SELECT MAX(itl_index) FROM ipp_trans_log WHERE itl_invoice_index = '" & intInvoiceNo & "' AND itl_performed_by = 'Finance Teller') " & _
            '                        "UNION ALL " & _
            '                     "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 0 AS FA_SEQ, " & _
            '                     "im_created_by AS FA_ACTIVE_AO, im_created_on AS FA_ACTION_DATE, " & _
            '                     "(SELECT DISTINCT fa_ao_action FROM finance_approval " & _
            '                     "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Submit Document' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " & _
            '                     "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'IPP Teller' AS 'Action',1 AS FA_SEQ2 " & _
            '                     "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_created_by=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
            '                     "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_created_on IS NOT NULL  " & _
            '                     "ORDER BY FA_SEQ2 "
            '        Else
            '            strSql = "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " & _
            '                                         "'IPP Verifier' AS 'Action', " & _
            '                                         "FA.FA_SEQ + 1 AS FA_SEQ2 " & _
            '                                         "FROM FINANCE_APPROVAL FA " & _
            '                                         "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "', INVOICE_MSTR " & _
            '                                         "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'  AND fa_action_date IS NOT NULL " & _
            '                                         "AND fa_aga_type = 'AO' " & _
            '                                                "UNION ALL " & _
            '                                         "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " & _
            '                                         "'Finance Verifier' AS 'Action', " & _
            '                                         "FA.FA_SEQ + 2 AS FA_SEQ2 " & _
            '                                         "FROM FINANCE_APPROVAL FA " & _
            '                                         "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "', INVOICE_MSTR " & _
            '                                         "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'  AND fa_action_date IS NOT NULL " & _
            '                                         "AND fa_aga_type = 'FO' " & _
            '                                                "UNION ALL " & _
            '                                         "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " & _
            '                                         "'Finance Approver' AS 'Action', " & _
            '                                         "FA.FA_SEQ + 2 AS FA_SEQ2 " & _
            '                                         "FROM FINANCE_APPROVAL FA " & _
            '                                         "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "', INVOICE_MSTR " & _
            '                                         "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'  AND fa_action_date IS NOT NULL " & _
            '                                         "AND fa_aga_type = 'FM' " & _
            '                                                "UNION ALL " & _
            '                                         "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 0 AS FA_SEQ, " & _
            '                                         "im_created_by AS FA_ACTIVE_AO, im_created_on AS FA_ACTION_DATE, " & _
            '                                         "(SELECT DISTINCT fa_ao_action FROM finance_approval " & _
            '                                         "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Submit Document' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " & _
            '                                         "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'IPP Teller' AS 'Action',1 AS FA_SEQ2 " & _
            '                                         "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_created_by=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
            '                                         "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_created_on IS NOT NULL  " & _
            '                                         "ORDER BY FA_SEQ2 "
            '        End If

            '    Else
            '        strSql = "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " & _
            '                 "'IPP Verifier' AS 'Action', " & _
            '                 "FA.FA_SEQ + 1 AS FA_SEQ2 " & _
            '                 "FROM FINANCE_APPROVAL FA " & _
            '                 "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "', INVOICE_MSTR " & _
            '                 "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'  AND fa_action_date IS NOT NULL " & _
            '                 "AND fa_aga_type = 'AO' " & _
            '                        "UNION ALL " & _
            '                 "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " & _
            '                 "'Finance Verifier' AS 'Action', " & _
            '                 "FA.FA_SEQ + 2 AS FA_SEQ2 " & _
            '                 "FROM FINANCE_APPROVAL FA " & _
            '                 "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "', INVOICE_MSTR " & _
            '                 "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'  AND fa_action_date IS NOT NULL " & _
            '                 "AND fa_aga_type = 'FO' " & _
            '                        "UNION ALL " & _
            '                 "SELECT FA.*,UMA.UM_USER_NAME AS AO_NAME, " & _
            '                 "'Finance Approver' AS 'Action', " & _
            '                 "FA.FA_SEQ + 2 AS FA_SEQ2 " & _
            '                 "FROM FINANCE_APPROVAL FA " & _
            '                 "LEFT OUTER JOIN USER_MSTR UMA ON FA.FA_ACTIVE_AO=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "', INVOICE_MSTR " & _
            '                 "WHERE FA_INVOICE_INDEX = IM_INVOICE_INDEX AND FA_INVOICE_INDEX = '" & intInvoiceNo & "' AND IM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'  AND fa_action_date IS NOT NULL " & _
            '                 "AND fa_aga_type = 'FM' " & _
            '                        "UNION ALL " & _
            '                 "SELECT im_invoice_index,'' AS FA_AGA_TYPE,'' AS FA_AO,'' AS FA_A_AO, '' AS FA_A_AO_2, '' AS FA_A_AO_3, '' AS FA_A_AO_4, 0 AS FA_SEQ, " & _
            '                 "im_created_by AS FA_ACTIVE_AO, im_created_on AS FA_ACTION_DATE, " & _
            '                 "(SELECT DISTINCT fa_ao_action FROM finance_approval " & _
            '                 "WHERE fa_invoice_index = '" & intInvoiceNo & "') AS FA_AO_ACTION,'Submit Document' AS FA_AO_REMARK,'' AS fa_approval_type, '' AS fa_approval_grp_index, " & _
            '                 "'' AS fa_on_behalfof,'' AS fa_relief_ind,UMA.UM_USER_NAME AS AO_NAME,'IPP Teller' AS 'Action',1 AS FA_SEQ2 " & _
            '                 "FROM invoice_mstr LEFT OUTER JOIN USER_MSTR UMA ON im_created_by=UMA.UM_USER_ID AND UMA.UM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " & _
            '                 "WHERE im_INVOICE_INDEX = '" & intInvoiceNo & "' AND im_created_on IS NOT NULL  " & _
            '                 "ORDER BY FA_SEQ2 "

            '    End If
            'End If
            'ds = objDb.FillDs(strSql)
            'Return ds
        End Function
        'Zulham 30-01-2015 IPP-GST Stage 2A
        Public Function ApproveBillingDoc(ByVal strIPPDocIdx As String, ByVal strRemark As String, ByRef blnRelief As Boolean, ByVal role As String, Optional ByVal paymentmethod As String = "", Optional ByVal exchangeRate As String = "", Optional ByVal paymenttype As String = "", Optional ByVal strTime As String = "", Optional ByVal intApprGrpIndex As String = "", Optional ByVal strBillInvApprBy As String = "", Optional ByVal aryDocDetails As ArrayList = Nothing, Optional ByVal chkConvertToRentas As Boolean = False) As Boolean
            Dim TimeNow As String = DateTime.Now.ToString("HH:mm")
            Dim strSQL, strSqlAry(0) As String
            Dim strDateTime As String
            Dim i As Integer
            Dim strDocNo, strVenIdx As String

            strSQL = " SET @DUPLICATE_CHK =''; update billing_mstr set bm_invoice_status = bm_invoice_status wHERE bM_INVOICE_INDEX = " & strIPPDocIdx
            Common.Insert2Ary(strSqlAry, strSQL)

            Dim status = ""

            strSQL = " SELECT CAST(@DUPLICATE_CHK := IF(@DUPLICATE_CHK='', IF(BM_INVOICE_STATUS = '" & status & "' ,'ACK', @DUPLICATE_CHK), @DUPLICATE_CHK) AS CHAR(1000)) AS Outs FROM Billing_MSTR WHERE bM_INVOICE_INDEX = " & strIPPDocIdx
            Common.Insert2Ary(strSqlAry, strSQL)

            'If role = "3" Then ' FM
            If exchangeRate.Trim.Length > 0 Then
                strSQL = " UPDATE BILLING_MSTR SET  bM_INVOICE_STATUS = 3, bm_exchange_rate = '" & exchangeRate & "', " & _
                "BM_STATUS_CHANGED_BY ='" & HttpContext.Current.Session("UserId") & "'," & _
                "BM_STATUS_CHANGED_ON = NOW(),bm_approved_date = NOW() " & _
                "WHERE BM_INVOICE_INDEX = " & strIPPDocIdx & " and bm_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'"
            Else
                strSQL = " UPDATE BILLING_MSTR SET  bM_INVOICE_STATUS = 3, " & _
                "BM_STATUS_CHANGED_BY ='" & HttpContext.Current.Session("UserId") & "'," & _
                "BM_STATUS_CHANGED_ON = NOW(),bm_approved_date = NOW() " & _
                "WHERE BM_INVOICE_INDEX = " & strIPPDocIdx & " and bm_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'"
            End If
            'End If

            Common.Insert2Ary(strSqlAry, strSQL)


            'for update line item's exchange rate & currency for related company only
            'If (role = "2" Or role = "3") And aryDocDetails IsNot Nothing Then
            strDocNo = objDb.GetVal("select bm_invoice_no from billing_mstr where bm_invoice_index = " & strIPPDocIdx & "")
            strVenIdx = objDb.GetVal("select bm_s_coy_id from billing_mstr where bm_invoice_index = " & strIPPDocIdx & "")

            If Not aryDocDetails Is Nothing Then
                For i = 0 To aryDocDetails.Count - 1
                    If (aryDocDetails(i)(2) = "" Or aryDocDetails(i)(2) = "MYR") And aryDocDetails(i)(3) = "" Then
                        strSQL = "UPDATE billing_DETAILS SET bm_DR_EXCHANGE_RATE = NULL , bm_DR_CURRENCY = NULL, bm_gsT_value = '" & aryDocDetails(i)(4) & "', bm_gsT_input_tax_code = '" & aryDocDetails(i)(5) & "', bm_gsT_output_tax_code = '" & aryDocDetails(i)(6) & "'" & _
                              "WHERE bm_INVOICE_NO = '" & strDocNo & "' AND bm_S_COY_ID = '" & strVenIdx & "' AND bm_INVOICE_LINE = '" & aryDocDetails(i)(0) & "'"

                    Else
                        strSQL = "UPDATE billing_DETAILS SET bm_DR_EXCHANGE_RATE = '" & Common.parseNull(aryDocDetails(i)(3)) & "' , bm_DR_CURRENCY = '" & Common.parseNull(aryDocDetails(i)(2)) & "', bm_gsT_value = '" & aryDocDetails(i)(4) & "', bm_gsT_input_tax_code = '" & aryDocDetails(i)(5) & "', bm_gsT_output_tax_code = '" & aryDocDetails(i)(6) & "'  " & _
                        "WHERE bm_INVOICE_NO = '" & strDocNo & "' AND bm_S_COY_ID = '" & strVenIdx & "' AND bm_INVOICE_LINE = '" & aryDocDetails(i)(0) & "'"
                    End If

                    Common.Insert2Ary(strSqlAry, strSQL)
                Next
            End If
            'End If

            If role = "2" Or role = "3" Or role = "5" Then

                Dim currentfaaction As Integer
                Dim currentseq As String
                currentfaaction = objDb.GetVal("SELECT DISTINCT ba_ao_action FROM billing_approval WHERE ba_bill_index = '" & strIPPDocIdx & "' ")
                currentseq = objDb.Get1Column("billing_APPROVAL", "bA_SEQ", " WHERE bA_bill_INDEX='" & strIPPDocIdx & "' and ba_ao = '" & HttpContext.Current.Session("UserId") & "' and ba_seq = '" & currentfaaction + 1 & "'")
                If currentseq = "" Then
                    currentseq = objDb.Get1Column("billing_APPROVAL", "bA_SEQ", " WHERE bA_bill_INDEX='" & strIPPDocIdx & "' and bA_A_AO = '" & HttpContext.Current.Session("UserId") & "' and ba_seq = '" & currentfaaction + 1 & "'")
                End If
                '-----
                If currentseq = "" Then
                    HttpContext.Current.Session("Verified") = "1"
                    Return False
                End If
                '-----
                If blnRelief = True Then
                    strSQL = " UPDATE billing_APPROVAL SET bA_AO_REMARK = '" & Common.Parse(strRemark) & "'," & _
                             "bA_AO ='" & HttpContext.Current.Session("UserId") & "'," & _
                             "bA_ACTION_DATE = NOW()," & _
                             "bA_ON_BEHALFOF ='" & HttpContext.Current.Session("UserId") & "'," & _
                             "bA_ACTIVE_AO ='" & HttpContext.Current.Session("UserId") & "'," & _
                             "bA_ACTION_DATE = NOW()" & _
                             "WHERE bA_INVOICE_INDEX = " & strIPPDocIdx & " and ba_seq = '" & currentseq & "' and (ba_ao = '" & HttpContext.Current.Session("UserId") & "' OR ba_a_ao = '" & HttpContext.Current.Session("UserId") & "')"
                Else

                    strSQL = " UPDATE billing_APPROVAL SET bA_AO_REMARK = '" & Common.Parse(strRemark) & "'," & _
                             "ba_ACTION_DATE = NOW()," & _
                             "bA_ACTIVE_AO ='" & HttpContext.Current.Session("UserId") & "'," & _
                             "bA_ACTION_DATE = NOW()" & _
                             " WHERE bA_bill_INDEX = " & strIPPDocIdx & " and ba_seq = '" & currentseq & "' and (ba_ao = '" & HttpContext.Current.Session("UserId") & "' OR ba_a_ao = '" & HttpContext.Current.Session("UserId") & "')"
                End If
                Common.Insert2Ary(strSqlAry, strSQL)

                strSQL = "UPDATE billing_APPROVAL SET bA_AO_ACTION='" & currentseq & "' WHERE bA_bill_INDEX='" & strIPPDocIdx & "'"
                Common.Insert2Ary(strSqlAry, strSQL)
                If role = "2" Then
                    strSQL = "UPDATE billing_APPROVAL SET " & _
                    "bA_ACTIVE_AO = NULL,bA_ACTION_DATE = NULL, " & _
                    "bA_AO_REMARK=NULL " & _
                    "WHERE bA_bill_INDEX='" & strIPPDocIdx & "' "
                    Common.Insert2Ary(strSqlAry, strSQL)
                End If

            End If

            'Dim IPPMAIN As New IPPMain
            'IPPMAIN.AddAuditTrailRecordInsert2Ary(Common.parseNull(strIPPDocIdx), "Billing Verifier", strSqlAry, "" & Common.Parse(strRemark) & "")
            'IPP Gst Stage 2A - CH - 13 Feb 2015
            AddBillAuditTrailRecordInsert2Ary(Common.parseNull(strIPPDocIdx), "Billing Verifier", strSqlAry, Common.Parse(strRemark))
            Dim pTranNo As String = ""
            If objDb.BatchExecuteForIPP(strSqlAry, True, pTranNo, role) Then 'Dec 5, 2013
                Return True
            Else
                Return False
            End If


        End Function
        'Zulham 30-01-2015 IPP-GST Stage 2A
        Public Function PopulateBillingListNew(ByVal docno As String, ByVal doctype As String, ByVal docstatus As String, ByVal docsdt As String, ByVal docedt As String, ByVal strVen As String, Optional ByVal role As String = "", Optional ByVal strVendorIndex As String = "") As DataSet
            Dim strSql As String
            Dim dsGroup As DataSet
            'Modified for IPP GST Stage 2A - CH - 12 Feb 2015
            strSql = "SELECT BM_INVOICE_INDEX,bm_INVOICE_NO,bm_INVOICE_TYPE,bm_DOC_DATE,bm_S_COY_NAME,bm_CURRENCY_CODE,bm_INVOICE_TOTAL, BM_CREATED_ON, " & _
                     "BM_INVOICE_STATUS, BM_BANK_CODE, BM_BANK_ACCT, STATUS_DESC " & _
                     "FROM billing_mstr  " & _
                     "INNER JOIN status_mstr ON status_no = bm_invoice_status AND status_type = 'BIL' AND status_deleted = 'N'  " & _
                     "WHERE BM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " & _
                     "AND BM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "

            If docno <> "" Then
                strSql &= " AND BM_INVOICE_NO LIKE '%" & docno & "%'"
            End If
            If doctype <> "" Then
                strSql &= " AND BM_INVOICE_TYPE ='" & doctype & "'"
            End If
            If docstatus <> "" Then
                strSql &= " AND BM_INVOICE_STATUS IN (" & docstatus & ")"
            End If
            If docsdt <> "" Then
                strSql &= " AND BM_CREATED_ON >= " & Common.ConvertDate(docsdt) & ""
            End If
            If docedt <> "" Then
                strSql &= " AND BM_CREATED_ON <= " & Common.ConvertDate(docedt & " 23:59:59") & ""
            End If
            If strVendorIndex <> "" Then
                strSql &= " AND BM_S_COY_ID = '" & strVendorIndex & "'"
            End If

            'strSql &= " UNION " & _
            '         "SELECT bm_INVOICE_INDEX,bm_INVOICE_NO,bm_INVOICE_TYPE,bm_DOC_DATE,bm_S_COY_NAME,bm_CURRENCY_CODE,bm_INVOICE_TOTAL,DATE_FORMAT(bm_created_on,'%d-%m-%Y') 'bm_created_on', " & _
            '         "bm_INVOICE_STATUS,bm_bank_code,bm_bank_acct,ic_bank_code,ic_bank_acct, " & _
            '         "STATUS_DESC " & _
            '         "FROM billing_mstr  " & _
            '        "INNER JOIN status_mstr ON status_no = bm_invoice_status AND status_type = 'BIL' and status_deleted = 'N' " & _
            '        "INNER JOIN ipp_company ON bm_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "' " & _
            '        "WHERE BM_CREATED_BY = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " & _
            '        "AND bM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' "

            'If docno <> "" Then
            '    strSql &= " AND BM_INVOICE_NO LIKE '%" & docno & "%'"
            'End If
            'If doctype <> "" Then
            '    strSql &= " AND bm_INVOICE_TYPE ='" & doctype & "'"
            'End If
            'If docstatus <> "" Then
            '    strSql &= " AND bm_INVOICE_STATUS IN (" & docstatus & ")"
            'End If
            'If docsdt <> "" Then
            '    strSql &= " AND bM_DOC_DATE >= " & Common.ConvertDate(docsdt) & ""
            'End If
            'If docedt <> "" Then
            '    strSql &= " AND BM_DOC_DATE <= " & Common.ConvertDate(docedt) & ""
            'End If
            'If strVendorIndex <> "" Then
            '    strSql &= " AND bM_S_COY_ID = '" & strVendorIndex & "'"
            'End If

            strSql &= " ORDER BY bM_INVOICE_INDEX "

            dsGroup = objDb.FillDs(strSql)
            PopulateBillingListNew = dsGroup
        End Function
        'Zulham 30-01-2015 IPP-GST Stage 2A
        Public Function GetApprBillingDetails(ByVal index As String, ByVal strIPPDocNo As String, ByVal strCoyId As String) As BillingDetails
            Dim dtIPP As New DataTable
            Dim objBillingDetails As New BillingDetails
            Dim strTempAddr As String
            Dim strsql As String
            Dim dtDiff As Long
            Dim objGlobal As New AppGlobals
            'Zulham 04/03/2015 IPP-GST Stage 2A
            Dim CoyId As String

            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                CoyId = HttpContext.Current.Session("CompanyID")
            Else
                CoyId = strDefIPPCompID
            End If

            strsql = "SELECT * ,ipp_company.ic_bank_code, ipp_company.ic_bank_acct,ipp_company.ic_credit_terms FROM Billing_MSTR, " & _
            "billing_DETAILS, ipp_company, STATUS_MSTR " & _
            "WHERE " & _
            "billing_DETAILS.bm_INVOICE_NO=billing_mstr.bM_INVOICE_NO AND " & _
            "billing_mstr.BM_S_COY_ID=billing_details.bm_S_COY_ID AND " & _
            "ic_index = billing_mstr.bm_s_coy_id AND " & _
            "ic_coy_id = '" & CoyId & "'  AND status_no = bm_invoice_status AND status_type = 'BIL' " & _
            "AND status_deleted = 'N' " & _
            "AND billing_DETAILS.bM_INVOICE_NO='" & strIPPDocNo & "' AND billing_DETAILS.bm_INVOICE_NO='" & strIPPDocNo & "' AND bM_B_COY_ID='" & strCoyId & "' " & _
            "AND bM_INVOICE_INDEX = '" & index & "' "

            dtIPP = objDb.FillDt(strsql)

            If Not dtIPP Is Nothing Then
                If Not dtIPP.Rows.Count = 0 Then
                    With dtIPP.Rows(0)
                        objBillingDetails.DocType = IIf(IsDBNull(.Item("BM_INVOICE_TYPE")), "", .Item("BM_INVOICE_TYPE"))
                        objBillingDetails.DocNo = IIf(IsDBNull(.Item("BM_INVOICE_NO")), "", .Item("BM_INVOICE_NO"))
                        objBillingDetails.DocDate = IIf(IsDBNull(.Item("bM_DOC_DATE")), "", .Item("bM_DOC_DATE"))
                        objBillingDetails.Vendor = IIf(IsDBNull(.Item("BM_S_COY_NAME")), "", .Item("BM_S_COY_NAME"))
                        strTempAddr = .Item("bM_ADDR_LINE1").ToString.Trim

                        If .Item("BM_ADDR_LINE2").ToString.Trim <> "" Then
                            strTempAddr = strTempAddr & "<BR>" & .Item("bM_ADDR_LINE2").ToString.Trim
                        End If

                        If .Item("bM_ADDR_LINE3").ToString.Trim <> "" Then
                            strTempAddr = strTempAddr & "<BR>" & .Item("bM_ADDR_LINE3").ToString.Trim
                        End If

                        If .Item("bM_POSTCODE").ToString.Trim <> "" Then
                            strTempAddr = strTempAddr & "<BR>" & .Item("bM_POSTCODE").ToString.Trim
                        End If
                        If .Item("bM_CITY").ToString.Trim <> "" Then
                            strTempAddr = strTempAddr & " " & .Item("bM_CITY").ToString.Trim
                        End If

                        If .Item("bM_STATE").ToString.Trim <> "" Then
                            strTempAddr = strTempAddr & "<BR>" & objGlobal.getCodeDesc(CodeTable.State, .Item("bM_STATE").ToString.Trim)
                        End If

                        If .Item("bM_COUNTRY").ToString.Trim <> "" Then
                            strTempAddr = strTempAddr & " " & objGlobal.getCodeDesc(CodeTable.Country, .Item("bM_COUNTRY").ToString.Trim)
                        End If

                        objBillingDetails.VAddr = strTempAddr
                        objBillingDetails.Status = IIf(IsDBNull(.Item("bM_INVOICE_STATUS")), "", .Item("bM_INVOICE_STATUS"))
                        objBillingDetails.Currency = IIf(IsDBNull(.Item("bM_CURRENCY_CODE")), "", .Item("bM_CURRENCY_CODE"))
                        objBillingDetails.PaymentAmt = Format(IIf(IsDBNull(.Item("bM_INVOICE_TOTAL")), "", .Item("bM_INVOICE_TOTAL")), "#,##0.00")
                        objBillingDetails.PaymentAmtWthGST = Format(IIf(IsDBNull(.Item("bM_INVOICE_wth_TOTAL")), "", .Item("bM_INVOICE_wth_TOTAL")), "#,##0.00")
                        objBillingDetails.Remarks = IIf(IsDBNull(.Item("bM_REMARK")), "", .Item("bM_REMARK"))
                        objBillingDetails.ExchangeRate = IIf(IsDBNull(.Item("bM_EXCHANGE_RATE")), "", .Item("bM_EXCHANGE_RATE"))
                        objBillingDetails.Remark = IIf(IsDBNull(.Item("bM_REMARK")), "", .Item("bM_REMARK"))
                        objBillingDetails.ExchangeRate = IIf(IsDBNull(.Item("BM_EXCHANGE_RATE")), "", .Item("bM_EXCHANGE_RATE"))
                        objBillingDetails.BeneficiaryDetails = IIf(IsDBNull(.Item("bM_REMARKS2")), "", .Item("bM_REMARKS2"))
                        objBillingDetails.BillInvApprBy = IIf(IsDBNull(.Item("bM_REMARKS1")), "", .Item("bM_REMARKS1"))
                        objBillingDetails.StatusDescription = IIf(IsDBNull(.Item("STATUS_DESC")), "", .Item("STATUS_DESC"))
                        objBillingDetails.GSTRegNo = IIf(IsDBNull(.Item("IC_TAX_REG_NO")), "", .Item("IC_TAX_REG_NO"))
                    End With
                    GetApprBillingDetails = objBillingDetails
                Else
                    GetApprBillingDetails = Nothing
                End If
            Else
                GetApprBillingDetails = Nothing
            End If
        End Function
        'Zulham 30-01-2015 IPP-GST Stage 2A
        Public Function PopulateBillingApprRejList(ByVal docno As String, ByVal doctype As String, ByVal docstatus As String, ByVal docsdt As String, ByVal docedt As String, ByVal vendor As String, ByVal verifiedsdt As String, ByVal verifiededt As String) As DataSet
            Dim strSql As String
            Dim dsGroup As DataSet
            'Modified for IPP GST Stage 2A - CH - 13 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If
            '------------------------------------------------

            'strSql = "SELECT distinct BM_INVOICE_INDEX,BM_INVOICE_NO,BM_INVOICE_TYPE,BM_DOC_DATE,BM_S_COY_NAME,BM_CURRENCY_CODE,bM_INVOICE_TOTAL,bM_INVOICE_STATUS, " _
            '        & "ic_bank_code, ic_bank_acct,STATUS_DESC, bM_created_on " _
            '        & "FROM BILLING_MSTR " _
            '        & "INNER JOIN status_mstr ON status_no = bm_invoice_status AND status_type = 'BIL' and status_deleted = 'N' " _
            '        & "INNER JOIN ipp_company ON bm_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "' " _
            '        & "INNER JOIN ipp_trans_log ON itl_invoice_index = bm_invoice_index AND itl_user_id = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " _
            '        & "LEFT JOIN billing_approval ON bM_INVOICE_INDEX = ba_bill_index WHERE " _
            '        & " bM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'"
            strSql = "SELECT DISTINCT BILLING_MSTR.BM_INVOICE_INDEX,BILLING_MSTR.BM_INVOICE_NO,BM_INVOICE_TYPE,BM_DOC_DATE,BM_S_COY_NAME,BM_CURRENCY_CODE," & _
                    "bM_INVOICE_TOTAL, bM_INVOICE_STATUS, ic_bank_code, ic_bank_acct, STATUS_DESC, bM_created_on " & _
                    "FROM BILLING_MSTR " & _
                    "INNER JOIN status_mstr ON status_no = bm_invoice_status AND status_type = 'BIL' AND status_deleted = 'N' " & _
                    "INNER JOIN ipp_company ON bm_s_coy_id = ic_index AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "' " & _
                    "INNER JOIN billing_trans_log BTL ON BTL.bm_invoice_index = BILLING_MSTR.bm_invoice_index AND BTL.BM_user_id = '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " & _
                    "LEFT JOIN billing_approval ON BILLING_MSTR.bM_INVOICE_INDEX = ba_bill_index WHERE  bM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and billing_mstr.bm_invoice_status not in ('1','2') "
            If docno <> "" Then
                strSql &= " AND BILLING_MSTR.bM_INVOICE_NO LIKE '%" & docno & "%'"
            End If
            If doctype <> "" Then
                strSql &= " AND bM_INVOICE_TYPE ='" & doctype & "'"
            End If
            If docstatus <> "" Then
                strSql &= " AND bM_INVOICE_STATUS IN (" & docstatus & ")"
            End If
            If docsdt <> "" Then
                strSql &= " AND bM_created_on >= " & Common.ConvertDate(docsdt) & ""
            End If
            If docedt <> "" Then
                strSql &= " AND bM_created_on <= '" & Format(CDate(docedt), "yyyy-MM-dd 23:59:59") & "'"
            End If
            If verifiedsdt <> "" Then
                strSql &= " AND CAST(bm_trans_date AS DATE) >= " & Common.ConvertDate(verifiedsdt) & ""
            End If
            If verifiededt <> "" Then
                strSql &= " AND CAST(BM_trans_date AS DATE)  <= '" & Format(CDate(verifiededt), "yyyy-MM-dd 23:59:59") & "'"
            End If
            If vendor <> "" Then
                strSql &= " AND bM_S_COY_NAME LIKE '%" & vendor & "%'"
            End If
            dsGroup = objDb.FillDs(strSql)
            PopulateBillingApprRejList = dsGroup
        End Function
        'Zulham 30-01-2015 IPP-GST Stage 2A
        Public Function PopulateBillingEnqList(ByVal docno As String, ByVal payadv As String, ByVal docstatus As String, ByVal docsdt As String, ByVal docedt As String, ByVal paysdt As String, ByVal payedt As String, ByVal strVen As String, ByVal strVenAddr As String, Optional ByVal Dept As String = "", Optional ByVal psdsentsdt As String = "", Optional ByVal psdsentedt As String = "", Optional ByVal strVendorIndex As String = "") As DataSet
            Dim strSql As String
            Dim strSql2 As String = ""
            Dim dsGroup As DataSet
            'Modified for IPP GST Stage 2A - CH - 13 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If
            '------------------------------------------------

            strSql = "SELECT * FROM ("

            'Search document No.
            If Dept = "" Then
                strSql &= "SELECT bM_INVOICE_INDEX, bm_INVOICE_NO, ISD_DOC_NO, bM_CREATED_BY, bM_DOC_DATE,bM_S_COY_NAME, bM_BANK_CODE, bM_BANK_ACCT, " & _
                        "bM_CURRENCY_CODE, ISD_DOC_AMT, bM_INVOICE_TOTAL " & _
                        "bM_INVOICE_STATUS, ic2.IC_BANK_CODE, ic2.IC_BANK_ACCT, STATUS_DESC, ic2.IC_ADDR_LINE1, " & _
                        "bM_INVOICE_TYPE " & _
                        "FROM billing_MSTR " & _
                        "INNER JOIN IPP_COMPANY ON bM_S_COY_ID = IC_INDEX AND IC_COY_ID = '" & Common.Parse(strDefIPPCompID) & "' " & _
                        "INNER JOIN STATUS_MSTR ON STATUS_NO = bM_INVOICE_STATUS AND STATUS_TYPE = 'BIL' AND STATUS_DELETED = 'N' " & _
                        "INNER JOIN IPP_COMPANY ic2 ON bM_S_COY_ID = ic2.IC_INDEX AND ic2.IC_COY_ID = '" & Common.Parse(strDefIPPCompID) & "' " & _
                        "LEFT JOIN IPP_SUB_DOC ON bM_INVOICE_INDEX = ISD_MSTR_DOC_INDEX " & _
                        "WHERE bM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " & _
                        "AND NOT bM_INVOICE_TYPE IS NULL"
            Else
                strSql &= "SELECT bM_INVOICE_INDEX,bM_INVOICE_NO, ISD_DOC_NO, bM_CREATED_BY, bM_DOC_DATE, bM_S_COY_NAME, bM_BANK_CODE, bM_BANK_ACCT, " & _
                        "bM_CURRENCY_CODE, ISD_DOC_AMT, bM_INVOICE_TOTAL " & _
                        "bM_INVOICE_STATUS, ic2.IC_BANK_CODE, ic2.IC_BANK_ACCT, STATUS_DESC, ic2.IC_ADDR_LINE1, " & _
                        "bM_INVOICE_TYPE " & _
                        "FROM billing_MSTR " & _
                        "INNER JOIN IPP_COMPANY ON bM_S_COY_ID = IC_INDEX AND IC_COY_ID = '" & Common.Parse(strDefIPPCompID) & "' " & _
                        "INNER JOIN STATUS_MSTR ON STATUS_NO = bM_INVOICE_STATUS AND STATUS_TYPE = 'BIL' AND STATUS_DELETED = 'N' " & _
                        "INNER JOIN IPP_COMPANY ic2 ON bM_S_COY_ID = ic2.IC_INDEX AND ic2.IC_COY_ID = '" & Common.Parse(strDefIPPCompID) & "' " & _
                        "INNER JOIN USER_MSTR um ON bM_CREATED_BY = um.UM_USER_ID " & _
                        "LEFT JOIN IPP_SUB_DOC ON bM_INVOICE_INDEX = ISD_MSTR_DOC_INDEX " & _
                        "WHERE bM_B_COY_ID ='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND NOT bm_INVOICE_TYPE IS NULL " & _
                        "AND um.UM_DEPT_ID = '" & Dept & "'"
            End If

            If docno <> "" Then
                strSql &= " AND bM_INVOICE_NO LIKE '%" & docno & "%'"
            End If
            If docsdt <> "" Then
                strSql &= " AND bM_DOC_DATE >= " & Common.ConvertDate(docsdt) & ""
            End If
            If docedt <> "" Then
                strSql &= " AND bM_DOC_DATE <= " & "'" & Format(CDate(docedt), "yyyy-MM-dd 23:59:00") & "'" & ""
            End If
            If docstatus <> "" Then
                strSql &= " AND bM_INVOICE_STATUS IN (" & docstatus & ")"
            End If
            If strVendorIndex <> "" Then
                strSql &= " AND bM_S_COY_ID = '" & strVendorIndex & "'"
            End If
            If strVenAddr <> "" Then
                strSql &= " AND ic2.IC_ADDR_LINE1 like '%" & strVenAddr & "%'"
            End If

            strSql &= ") tb ORDER BY bM_INVOICE_INDEX "

            dsGroup = objDb.FillDs(strSql)
            PopulateBillingEnqList = dsGroup
        End Function
        'Zulham 06/02/2015 Case 8317
        Public Function UpdateBillingDocDetail(ByVal aryDoc As ArrayList, ByVal Docno As String, ByVal venidx As String, Optional ByVal oldDocno As String = "", Optional ByVal MasterDoc As String = "N", Optional ByVal strIsGST As String = "")
            Dim i, j, intCostAllocIndex As Integer
            Dim strPayFor, strSQL, strAryQuery(0), strInvNo As String
            Dim InvLineAmt, Amount, ttlAmount As Double
            Dim dsDocLine As DataSet
            Dim GST As New GST

            strInvNo = objDb.GetVal("SELECT bm_invoice_index FROM billing_mstr WHERE bm_invoice_no = '" & Common.Parse(oldDocno) & "' and bm_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and bm_s_coy_id = '" & venidx & "'")

            If InStr(aryDoc.Item(i)(8), ":") Then
                aryDoc.Item(i)(8) = aryDoc.Item(i)(8).Substring(0, InStr(aryDoc.Item(i)(8), ":") - 1)
            End If

            If aryDoc.Item(i)(3) <> "" Then
                'If aryDoc.Item(i)(1) = "Own Co." Then
                '    strPayFor = 
                'Else
                '    strPayFor = Common.Parse(aryDoc.Item(i)(1))
                'End If

                'Get the Tax Rate and the Percentage
                Dim gstTaxRate As String = ""
                Dim gstPercentage As String = ""
                GST.getGSTInfobyRate_ForIPP(Common.Parse(aryDoc.Item(i)(22)), gstPercentage, gstTaxRate)
                If gstPercentage = "" Then
                    gstPercentage = "0.00"
                End If
                'End

                aryDoc(i)(14) = objDb.GetVal("SELECT cc_cc_desc FROM cost_centre WHERE cc_cc_code = '" & Common.Parse(aryDoc.Item(i)(13)) & "' and cc_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
                aryDoc(i)(15) = objDb.GetVal("SELECT cbm_branch_name FROM company_branch_mstr WHERE cbm_branch_code = '" & Common.Parse(aryDoc.Item(i)(12)) & "' and cbm_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
                'Zulham 21/06/2017
                'IPP Stage 3
                If strIsGST = "" Then
                    strSQL = "UPDATE Billing_DETAILS SET BM_PRODUCT_DESC='" & Common.Parse(aryDoc.Item(i)(3)) & "', " &
                                "bm_UOM='" & Common.Parse(aryDoc.Item(i)(4)) & "', " &
                                "bm_RECEIVED_QTY='" & Common.Parse(aryDoc.Item(i)(5)) & "', " &
                                "bm_UNIT_COST='" & Common.Parse(aryDoc.Item(i)(6)) & "', " &
                                "bm_B_GL_CODE='" & Common.Parse(aryDoc.Item(i)(8)) & "', " &
                                "bm_REF_NO='" & Common.Parse(aryDoc.Item(i)(2)) & "', " &
                                "bm_COST_CENTER='" & Common.Parse(aryDoc.Item(i)(13)) & "', " &
                                "bm_COST_CENTER_DESC='" & Common.Parse(aryDoc.Item(i)(14)) & "', " &
                                "bm_BRANCH_CODE='" & Common.Parse(aryDoc.Item(i)(12)) & "', " &
                                "bm_BRANCH_CODE_NAME='" & Common.Parse(aryDoc.Item(i)(15)) & "', " &
                                "bm_glrule_category='" & Common.Parse(aryDoc.Item(i)(18)) & "', " &
                                "bm_glrule_category_index='" & Common.Parse(aryDoc.Item(i)(19)) & "', " &
                                "bm_ref_no='" & Common.Parse(aryDoc.Item(i)(2)) & "', " &
                                "bm_category ='" & Common.Parse(aryDoc.Item(i)(24)) & "', " &
                                "bm_analysis_code1 ='" & Common.Parse(aryDoc.Item(i)(25)) & "', " &
                                "bm_analysis_code2 ='" & Common.Parse(aryDoc.Item(i)(26)) & "', " &
                                "bm_analysis_code3 ='" & Common.Parse(aryDoc.Item(i)(27)) & "', " &
                                "bm_analysis_code4 ='" & Common.Parse(aryDoc.Item(i)(28)) & "', " &
                                "bm_analysis_code5 ='" & Common.Parse(aryDoc.Item(i)(29)) & "', " &
                                "bm_analysis_code8 ='" & Common.Parse(aryDoc.Item(i)(32)) & "', " &
                                "bm_analysis_code9 ='" & Common.Parse(aryDoc.Item(i)(33)) & "' " &
                                "WHERE bm_INVOICE_LINE='" & Common.Parse(aryDoc.Item(i)(0)) & "' AND bm_S_COY_ID='" & venidx & "' AND bm_INVOICE_NO='" & oldDocno & "'"
                Else
                    'Jules 2018.04.26 - PAMB Scrum 2 - Added Category and Analysis Codes.
                    strSQL = "UPDATE billing_DETAILS SET bm_PRODUCT_DESC='" & Common.Parse(aryDoc.Item(i)(3)) & "', " &
                                "bm_UOM='" & Common.Parse(aryDoc.Item(i)(4)) & "', " &
                                "bm_RECEIVED_QTY='" & Common.Parse(aryDoc.Item(i)(5)) & "', " &
                                "bm_UNIT_COST='" & Common.Parse(aryDoc.Item(i)(6)) & "', " &
                                "bm_B_GL_CODE='" & Common.Parse(aryDoc.Item(i)(8)) & "', " &
                                "bm_COST_CENTER='" & Common.Parse(aryDoc.Item(i)(13)) & "', " &
                                "bm_COST_CENTER_DESC='" & Common.Parse(aryDoc.Item(i)(14)) & "', " &
                                "bm_BRANCH_CODE='" & Common.Parse(aryDoc.Item(i)(12)) & "', " &
                                "bm_BRANCH_CODE_NAME='" & Common.Parse(aryDoc.Item(i)(15)) & "', " &
                                "bm_glrule_category='" & Common.Parse(aryDoc.Item(i)(18)) & "', " &
                                "bm_glrule_category_index='" & Common.Parse(aryDoc.Item(i)(19)) & "', " &
                                "bm_gst_value ='" & IIf(Common.Parse(aryDoc.Item(i)(21)) = "", 0, aryDoc.Item(i)(21)) & "', " &
                                "bm_gst_input_tax_code ='" & Common.Parse(aryDoc.Item(i)(22)) & "', " &
                                "bm_gst_output_tax_code ='" & Common.Parse(aryDoc.Item(i)(23)) & "', " &
                                "bm_gst_rate ='" & Common.Parse(gstTaxRate) & "', " &
                                "bm_gst ='" & Common.Parse(gstPercentage) & "', " &
                                "bm_ref_no ='" & Common.Parse(aryDoc.Item(i)(2)) & "', " &
                                "bm_category ='" & Common.Parse(aryDoc.Item(i)(24)) & "', " &
                                "bm_analysis_code1 ='" & Common.Parse(aryDoc.Item(i)(25)) & "', " &
                                "bm_analysis_code2 ='" & Common.Parse(aryDoc.Item(i)(26)) & "', " &
                                "bm_analysis_code3 ='" & Common.Parse(aryDoc.Item(i)(27)) & "', " &
                                "bm_analysis_code4 ='" & Common.Parse(aryDoc.Item(i)(28)) & "', " &
                                "bm_analysis_code5 ='" & Common.Parse(aryDoc.Item(i)(29)) & "', " &
                                "bm_analysis_code8 ='" & Common.Parse(aryDoc.Item(i)(32)) & "', " &
                                "bm_analysis_code9 ='" & Common.Parse(aryDoc.Item(i)(33)) & "' " &
                                "WHERE bm_INVOICE_LINE='" & Common.Parse(aryDoc.Item(i)(0)) & "' AND bm_S_COY_ID='" & venidx & "' AND bm_INVOICE_NO='" & oldDocno & "'"
                End If
            End If
            'objDb.Execute(strSQL)

            Common.Insert2Ary(strAryQuery, strSQL)
            objDb.BatchExecute(strAryQuery)
        End Function
        'Zulham 08/02/2015 Case 8317
        Public Function VoidBillingDoc(ByVal strBillingDocNo As String) As Boolean

            Dim strSQL As String
            Dim strInvNo As String

            strInvNo = strBillingDocNo

            strSQL = "UPDATE Billing_MSTR SET " & _
                  "bM_INVOICE_STATUS = 5," & _
                  "bM_STATUS_CHANGED_BY ='" & HttpContext.Current.Session("UserId") & "'," & _
                  "bM_STATUS_CHANGED_ON = NOW()" & _
                  " WHERE bM_INVOICE_INDEX = '" & strInvNo & "' and bm_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'"


            objDb.Execute(strSQL)

            Return True

        End Function
        'Zulham 08/02/2015 Case 8317
        Public Function billing_detail(ByVal billlingDocNo As String, ByVal vCoyId As String, ByVal VenIdx As Integer, Optional ByVal frm As String = "", Optional ByVal userid As String = "") As DataSet
            Dim InvIdx As String
            Dim strsql As String

            If userid = "" Then
                userid = Common.Parse(HttpContext.Current.Session("UserID"))
            End If

            InvIdx = objDb.GetVal("SELECT bm_invoice_index FROM billing_mstr WHERE bm_invoice_no = '" & billlingDocNo & "' AND bm_s_coy_id = '" & VenIdx & "'")

            If frm = "PSDAcceptRejList" Then
                strsql = "SELECT IM.*, ID.*, CBGL.CBG_B_GL_DESC,IF(ID.ID_BRANCH_CODE='' AND ID.ID_BRANCH_CODE_NAME='','',CONCAT(ID.ID_BRANCH_CODE,':',ID.ID_BRANCH_CODE_NAME)) AS ID_BRANCH_CODE_2, IF(ID.ID_COST_CENTER='' AND ID.ID_COST_CENTER_DESC='','',CONCAT(ID.ID_COST_CENTER,':',ID.ID_COST_CENTER_DESC)) AS ID_COST_CENTER_2 FROM INVOICE_MSTR IM,INVOICE_DETAILS ID " _
                              & "LEFT JOIN company_b_gl_code CBGL ON CBGL.CBG_B_GL_CODE = ID_B_GL_CODE AND cbg_b_coy_id ='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                              & "WHERE ID_INVOICE_NO=IM_INVOICE_NO AND IM_S_COY_ID=ID_S_COY_ID " _
                               & "AND IM_INVOICE_NO='" & billlingDocNo & "' AND ID_INVOICE_NO='" & billlingDocNo & "' AND IM_B_COY_ID='" & vCoyId & "' and id_s_coy_id = '" & VenIdx & "' " _
                               & "AND im_invoice_index = '" & InvIdx & "'"
            ElseIf frm = "PSDAcceptanceDetails" Or frm = "PSDAcceptRejList,dashboard" Then
                strsql = "SELECT IM.*, ID.*, CBGL.CBG_B_GL_DESC,IF(ID.ID_BRANCH_CODE='' AND ID.ID_BRANCH_CODE_NAME='','',CONCAT(ID.ID_BRANCH_CODE,':',ID.ID_BRANCH_CODE_NAME)) AS ID_BRANCH_CODE_2, IF(ID.ID_COST_CENTER='' AND ID.ID_COST_CENTER_DESC='','',CONCAT(ID.ID_COST_CENTER,':',ID.ID_COST_CENTER_DESC)) AS ID_COST_CENTER_2 FROM INVOICE_MSTR IM,INVOICE_DETAILS ID " _
                              & "LEFT JOIN company_b_gl_code CBGL ON CBGL.CBG_B_GL_CODE = ID_B_GL_CODE AND cbg_b_coy_id ='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                              & "WHERE ID_INVOICE_NO=IM_INVOICE_NO AND IM_S_COY_ID=ID_S_COY_ID " _
                               & "AND IM_INVOICE_NO='" & billlingDocNo & "' AND ID_INVOICE_NO='" & billlingDocNo & "' AND IM_B_COY_ID='" & vCoyId & "' and id_s_coy_id = '" & VenIdx & "' " _
                               & "AND im_invoice_index = '" & InvIdx & "'"
            ElseIf frm = "EnterBC" Then
                strsql = "SELECT IM.*, ID.*, CBGL.CBG_B_GL_DESC,IF(ID.ID_BRANCH_CODE='' AND ID.ID_BRANCH_CODE_NAME='','',CONCAT(ID.ID_BRANCH_CODE,':',ID.ID_BRANCH_CODE_NAME)) AS ID_BRANCH_CODE_2, IF(ID.ID_COST_CENTER='' AND ID.ID_COST_CENTER_DESC='','',CONCAT(ID.ID_COST_CENTER,':',ID.ID_COST_CENTER_DESC)) AS ID_COST_CENTER_2 FROM INVOICE_MSTR IM,INVOICE_DETAILS ID " _
                                               & "LEFT JOIN company_b_gl_code CBGL ON CBGL.CBG_B_GL_CODE = ID_B_GL_CODE AND cbg_b_coy_id ='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                                               & "WHERE ID_INVOICE_NO=IM_INVOICE_NO AND IM_S_COY_ID=ID_S_COY_ID " _
                                                & "AND IM_INVOICE_NO='" & billlingDocNo & "' AND ID_INVOICE_NO='" & billlingDocNo & "' AND IM_B_COY_ID='" & vCoyId & "' AND  id_s_coy_id = '" & VenIdx & "' " _
                                                & "AND im_invoice_index = '" & InvIdx & "'"


            Else
                'strsql = "SELECT BM.*, bD.*, CBGL.CBG_B_GL_DESC,IF(bd.bm_BRANCH_CODE='' AND bd.bm_BRANCH_CODE_NAME='','',CONCAT(bd.bm_BRANCH_CODE,':',bd.bm_BRANCH_CODE_NAME)) AS bm_BRANCH_CODE_2, IF(bd.bm_COST_CENTER='' AND bd.bm_COST_CENTER_DESC='','',CONCAT(bd.bm_COST_CENTER,':',bd.bm_COST_CENTER_DESC)) AS bm_COST_CENTER_2 FROM billing_MSTR BM,billing_DETAILS BD " _
                '                & "LEFT JOIN company_b_gl_code CBGL ON CBGL.CBG_B_GL_CODE = bd.bm_B_GL_CODE AND cbg_b_coy_id ='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                '                & "WHERE bd.bm_INVOICE_NO=bm.bM_INVOICE_NO AND bm.bM_S_COY_ID=bd.bm_S_COY_ID " _
                '                 & "AND bm.bM_INVOICE_NO='" & billlingDocNo & "' AND bd.bm_INVOICE_NO='" & billlingDocNo & "' AND bm.bM_B_COY_ID='" & vCoyId & "' AND bm.bM_created_by = '" & userid & "' and bd.bm_s_coy_id = '" & VenIdx & "' " _
                '                 & "AND bm.bM_invoice_index = '" & InvIdx & "'"
                'IPP Gst Stage 2A - CH - 13 Feb 2015
                strsql = "SELECT BM.*, bD.*, CBGL.CBG_B_GL_DESC,IF(bd.bm_BRANCH_CODE='' AND bd.bm_BRANCH_CODE_NAME='','',CONCAT(bd.bm_BRANCH_CODE,':',bd.bm_BRANCH_CODE_NAME)) AS bm_BRANCH_CODE_2, IF(bd.bm_COST_CENTER='' AND bd.bm_COST_CENTER_DESC='','',CONCAT(bd.bm_COST_CENTER,':',bd.bm_COST_CENTER_DESC)) AS bm_COST_CENTER_2 FROM billing_MSTR BM,billing_DETAILS BD " _
                                & "LEFT JOIN company_b_gl_code CBGL ON CBGL.CBG_B_GL_CODE = bd.bm_B_GL_CODE AND cbg_b_coy_id ='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
                                & "WHERE bd.bm_INVOICE_NO=bm.bM_INVOICE_NO AND bm.bM_S_COY_ID=bd.bm_S_COY_ID " _
                                 & "AND bm.bM_INVOICE_NO='" & billlingDocNo & "' AND bd.bm_INVOICE_NO='" & billlingDocNo & "' AND bm.bM_B_COY_ID='" & vCoyId & "' and bd.bm_s_coy_id = '" & VenIdx & "' " _
                                 & "AND bm.bM_invoice_index = '" & InvIdx & "'"
            End If


            billing_detail = objDb.FillDs(strsql)
        End Function
        'Zulham 10/02/2015 Case 8317
        Public Function RejectBillingDoc(ByVal strIPPDocIdx As String, ByVal strRemark As String, ByRef blnRelief As Boolean, ByVal role As String, Optional ByVal strRouteTo As String = "") As Boolean

            Dim strSQL, strSqlAry(0) As String
            Dim currentfaaction As Integer
            Dim currentseq As String
            Dim strDocOwner As String
            Dim strFinanceVerifier As String
            Dim strFinanceTeller As String
            Dim blnAO As Boolean

            currentfaaction = objDb.GetVal("SELECT DISTINCT ba_ao_action FROM billing_approval WHERE ba_bill_index = '" & strIPPDocIdx & "' ")
            currentseq = objDb.Get1Column("billing_APPROVAL", "bA_SEQ", " WHERE bA_bIll_INDEX='" & strIPPDocIdx & "' and ba_ao = '" & HttpContext.Current.Session("UserId") & "' and ba_seq = '" & currentfaaction + 1 & "'")
            blnAO = objDb.Exist("SELECT * FROM billing_approval WHERE ba_bill_index = '" & strIPPDocIdx & "'")

            If currentseq = "" Then
                currentseq = objDb.Get1Column("billing_APPROVAL", "bA_SEQ", " WHERE bA_bill_INDEX='" & strIPPDocIdx & "' and bA_A_AO = '" & HttpContext.Current.Session("UserId") & "' and ba_seq = '" & currentfaaction + 1 & "'")
            End If

            strDocOwner = objDb.GetVal("SELECT im_created_by FROM invoice_mstr WHERE im_invoice_index = '" & strIPPDocIdx & "' ")

            'If role = "5" Then 'AO
            strSQL = " UPDATE billing_MSTR SET  bM_INVOICE_STATUS = 4," & _
                    "bM_STATUS_CHANGED_BY ='" & HttpContext.Current.Session("UserId") & "'," & _
                    "bM_STATUS_CHANGED_ON = NOW() " & _
                    "WHERE bM_INVOICE_INDEX = '" & strIPPDocIdx & "'"
            ', IM_ROUTE_TO = '" & strDocOwner & "'
            Common.Insert2Ary(strSqlAry, strSQL)
            'ElseIf role = "3" Then 'FM
            'strFinanceVerifier = objDb.GetVal("SELECT fa_active_ao FROM finance_approval WHERE fa_aga_type = 'FO' AND fa_invoice_index = '" & strIPPDocIdx & "' ")

            'strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 14," & _
            '        "IM_STATUS_CHANGED_BY ='" & HttpContext.Current.Session("UserId") & "'," & _
            '        "IM_STATUS_CHANGED_ON = NOW(), IM_ROUTE_TO = '" & strFinanceVerifier & "' " & _
            '        "WHERE IM_INVOICE_INDEX = '" & strIPPDocIdx & "'"

            'Common.Insert2Ary(strSqlAry, strSQL)
            'ElseIf role = "2" Then 'FO
            'If strRouteTo = "O" Then
            '    strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 14," & _
            '             "IM_STATUS_CHANGED_BY ='" & HttpContext.Current.Session("UserId") & "'," & _
            '             "IM_STATUS_CHANGED_ON = NOW(),im_prcs_sent =NULL, " & _
            '             "im_prcs_recv =NULL, IM_ROUTE_TO = NULL " & _
            '             "WHERE IM_INVOICE_INDEX = '" & strIPPDocIdx & "'"
            '    'im_prcs_sent_id = NULL,im_prcs_sent_upd_date = NULL
            '    ',im_prcs_recv_id = NULL,im_prcs_recv_upd_date = NULL
            '    ', IM_ROUTE_TO = '" & strDocOwner & "' 
            'Else
            '    strFinanceTeller = objDb.GetVal("SELECT im_prcs_recv_id FROM invoice_mstr WHERE im_invoice_index = '" & strIPPDocIdx & "' ")

            '    strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 14," & _
            '        "IM_STATUS_CHANGED_BY ='" & HttpContext.Current.Session("UserId") & "'," & _
            '        "IM_STATUS_CHANGED_ON = NOW(), IM_ROUTE_TO = '" & strFinanceTeller & "', " & _
            '         "im_prcs_recv =NULL " & _
            '        "WHERE IM_INVOICE_INDEX = '" & strIPPDocIdx & "'"
            'End If

            ',im_prcs_recv_id = NULL,im_prcs_recv_upd_date = NULL
            'Common.Insert2Ary(strSqlAry, strSQL)

            'ElseIf role = "FinanceTeller" Then
            'strSQL = " UPDATE INVOICE_MSTR SET  IM_INVOICE_STATUS = 14," & _
            '        "IM_STATUS_CHANGED_BY ='" & HttpContext.Current.Session("UserId") & "'," & _
            '        "IM_STATUS_CHANGED_ON = NOW(),im_prcs_sent =NULL,im_prcs_recv =NULL, IM_ROUTE_TO = NULL " & _
            '        "WHERE IM_INVOICE_INDEX = '" & strIPPDocIdx & "'"

            'Common.Insert2Ary(strSqlAry, strSQL)

            'End If

            'If role = "FinanceTeller" Then
            '    strSQL = " UPDATE FINANCE_APPROVAL SET FA_AO_REMARK = Null," & _
            '    "FA_ACTION_DATE = Null," & _
            '    "FA_ACTIVE_AO = Null," & _
            '    "FA_AO_ACTION = 0," & _
            '    "FA_ACTION_DATE = Null" & _
            '    " WHERE FA_INVOICE_INDEX = " & strIPPDocIdx & ""
            'End If

            'If role = "5" Then
            strSQL = "UPDATE BILLING_APPROVAL SET BA_AO_REMARK = '" & Common.Parse(strRemark) & "'," & _
                    "BA_ACTION_DATE = NOW()," & _
                    "BA_ACTIVE_AO = '" & HttpContext.Current.Session("UserId") & "' " & _
                    "WHERE BA_BILL_INDEX = " & strIPPDocIdx & " AND BA_SEQ = '" & currentseq & "' " & _
                    "AND (BA_AO = '" & HttpContext.Current.Session("UserId") & "' OR BA_A_AO = '" & HttpContext.Current.Session("UserId") & "')"
            'End If
            'If role = "2" Then
            '    strSQL = " UPDATE FINANCE_APPROVAL SET FA_AO_REMARK = '" & Common.Parse(strRemark) & "'," & _
            '               "FA_ACTION_DATE = NOW()," & _
            '               "FA_ACTIVE_AO ='" & HttpContext.Current.Session("UserId") & "' " & _
            '                 " WHERE FA_INVOICE_INDEX = " & strIPPDocIdx & " and fa_seq = '" & currentseq & "' and (fa_ao = '" & HttpContext.Current.Session("UserId") & "' or fa_a_ao = '" & HttpContext.Current.Session("UserId") & "' " & _
            '               "OR fa_a_ao_2 = '" & HttpContext.Current.Session("UserId") & "' OR fa_a_ao_3 = '" & HttpContext.Current.Session("UserId") & "' OR fa_a_ao_4 = '" & HttpContext.Current.Session("UserId") & "')"
            'End If
            'If role = "3" Then
            '    strSQL = " UPDATE FINANCE_APPROVAL SET FA_AO_REMARK = '" & Common.Parse(strRemark) & "'," & _
            '               "FA_ACTION_DATE = NOW()," & _
            '               "FA_ACTIVE_AO ='" & HttpContext.Current.Session("UserId") & "'," & _
            '               "FA_AO_ACTION = '" & currentseq & "' " & _
            '                 " WHERE FA_INVOICE_INDEX = " & strIPPDocIdx & " and fa_seq = '" & currentseq & "' and (fa_ao = '" & HttpContext.Current.Session("UserId") & "' or fa_a_ao = '" & HttpContext.Current.Session("UserId") & "' " & _
            '               "OR fa_a_ao_2 = '" & HttpContext.Current.Session("UserId") & "' OR fa_a_ao_3 = '" & HttpContext.Current.Session("UserId") & "' OR fa_a_ao_4 = '" & HttpContext.Current.Session("UserId") & "')"
            'End If

            'IPP Gst Stage 2A - CH - 13 Feb 2015
            AddBillAuditTrailRecordInsert2Ary(Common.parseNull(strIPPDocIdx), "Billing Verifier", strSqlAry, Common.Parse(strRemark))

            Common.Insert2Ary(strSqlAry, strSQL)
            Return objDb.BatchExecute(strSqlAry)

            'clear rejected info 
            'If role = "2" Then
            '    strSQL = " UPDATE FINANCE_APPROVAL SET FA_AO_REMARK = NULL," & _
            '               "FA_ACTION_DATE = NULL," & _
            '               "FA_ACTIVE_AO = NULL " & _
            '               "WHERE FA_INVOICE_INDEX = " & strIPPDocIdx & " and fa_aga_type = 'FM'"
            'End If

            'Common.Insert2Ary(strSqlAry, strSQL)

            'If role = "3" Then
            '    If blnAO = True Then 'set the fa_ao_action = 0 for Finance Approver reject if the document is from source dept
            '        strSQL = " UPDATE FINANCE_APPROVAL SET " & _
            '                                            "FA_AO_ACTION = 1" & _
            '                                              " WHERE FA_INVOICE_INDEX = " & strIPPDocIdx & " "
            '    Else 'set the fa_ao_action = 0 for Finance Approver reject if the document is from Finance
            '        strSQL = " UPDATE FINANCE_APPROVAL SET " & _
            '                        "FA_AO_ACTION = 0" & _
            '                          " WHERE FA_INVOICE_INDEX = " & strIPPDocIdx & " "
            '    End If


            '    Common.Insert2Ary(strSqlAry, strSQL)
            'End If

            'If role = "2" Then
            '    AddAuditTrailRecordInsert2Ary(Common.parseNull(strIPPDocIdx), "Finance Verifier", strSqlAry, "" & Common.Parse(strRemark) & "")
            'ElseIf role = "3" Then
            '    AddAuditTrailRecordInsert2Ary(Common.parseNull(strIPPDocIdx), "Finance Approver", strSqlAry, "" & Common.Parse(strRemark) & "")
            'ElseIf role = "5" Then
            '    AddAuditTrailRecordInsert2Ary(Common.parseNull(strIPPDocIdx), "IPP Verifier", strSqlAry, "" & Common.Parse(strRemark) & "")
            'ElseIf role = "FinanceTeller" Then
            '    AddAuditTrailRecordInsert2Ary(Common.parseNull(strIPPDocIdx), "Finance Teller", strSqlAry, "" & Common.Parse(strRemark) & "")
            'End If
            'If role = "FinanceTeller" Then
            '    If objDb.BatchExecute(strSqlAry) Then
            '        'get the file's owner id using docIndexNum
            '        Dim initiator = objDb.GetVal("Select im_created_by from invoice_mstr where im_invoice_index = " & strIPPDocIdx)

            '        'strSQL = "update invoice_mstr set im_route_to = '" & initiator & "', im_remarks3 = '" & strRemark & "' where IM_INVOICE_INDEX = '" & strIPPDocIdx & "'"
            '        strSQL = "update invoice_mstr set  im_remarks3 = '" & Common.Parse(strRemark) & "' where IM_INVOICE_INDEX = '" & strIPPDocIdx & "'"
            '        objDb.Execute(strSQL)
            '        Dim i As New IPP
            '        i.sendMailToIPPTeller(HttpContext.Current.Session("PSDIPPDoc"), strIPPDocIdx)
            '        Return True
            '    End If

            'ElseIf objDb.BatchExecute(strSqlAry) Then
            '    sendMailToIPPOfficer(strIPPDocIdx, role, "reject")
            '    Return True
            'Else
            '    Return False
            'End If

        End Function

        'IPP Gst Stage 2A - CH - 13 Feb 2015 (Insert trans into billing log)
        Public Function AddBillAuditTrailRecordInsert2Ary(ByVal intInvIndex As Integer, ByVal strRole As String, ByRef pQuery() As String, Optional ByVal strStateDesc As String = "") As Boolean

            Dim strSQL As String
            strSQL = "INSERT INTO BILLING_TRANS_LOG (BM_INVOICE_INDEX, BM_PERFORMED_BY, BM_USER_ID, BM_TRANS_DATE, BM_REMARKS) " & _
                     "VALUES(" & intInvIndex & ",'" & strRole & "','" & Common.Parse(HttpContext.Current.Session("UserId")) & "',Now()" & _
                     ",'" & strStateDesc & "')"

            Common.Insert2Ary(pQuery, strSQL)
        End Function

        'IPP Gst Stage 2A - CH - 13 Feb 2015 (Retreive trans from billing log)
        Public Function GetBillAudit(ByVal strDocIndex As String) As DataSet
            Dim strSQL As String
            Dim ds As DataSet

            strSQL = "SELECT BM_PERFORMED_BY AS ITL_PERFORMED_BY, BM_USER_ID AS ITL_USER_ID, " & _
                    "BM_TRANS_DATE AS ITL_TRANS_DATE, BM_REMARKS AS ITL_REMARKS " & _
                    "FROM BILLING_TRANS_LOG WHERE BM_INVOICE_INDEX = '" & strDocIndex & "'"
            ds = objDb.FillDs(strSQL)

            Return ds
        End Function
        'Zulham 03/03/2015 IPP GST Stage 2A (Case 8317)
        Public Function DelBillingDocDetail(ByVal slineno As String, ByVal invno As String, ByVal venidx As String)
            Dim strsql, strInvIndex As String

            strInvIndex = objDb.GetVal("SELECT BM_INVOICE_INDEX FROM BILLING_MSTR WHERE BM_INVOICE_NO = '" & invno & "' and BM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")

            strsql = "DELETE FROM BILLING_DETAILS WHERE BM_INVOICE_NO='" & invno & "' " & _
            "AND BM_S_COY_ID='" & venidx & "' AND BM_INVOICE_LINE='" & slineno & "'"
            objDb.Execute(strsql)

        End Function

        Public Function getIPPCompIndex(ByRef strVenName As String, ByVal strType As String, Optional ByVal strVenStatus As String = "A") As Integer
            Dim strSql, strCoyId As String
            Dim intCompId As Integer

            'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
            Dim strDefIPPCompID As String = ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strCoyId = HttpContext.Current.Session("CompanyID")
            Else
                strCoyId = strDefIPPCompID
            End If

            'Modified for IPP GST Stage 2A - CH - 13 Feb 2015
            strSql = "SELECT IC_INDEX FROM IPP_COMPANY WHERE IC_COY_NAME = '" & Common.Parse(strVenName) & "' " & _
                    "AND IC_COY_TYPE = '" & strType & "' AND IC_COY_ID = '" & Common.Parse(strCoyId) & "' "
            If strVenStatus = "A" Then
                strSql &= "AND IC_STATUS = 'A'"
            End If

            intCompId = CInt(objDb.GetVal(strSql))

            Return intCompId
        End Function

        Public Function exchangeRate(ByVal index As String) As Boolean
            Dim sql As String
            sql = "SELECT bm_exchange_rate FROM billing_mstr "
            If objDb.Get1ColumnCheckNull("billing_mstr", "bm_exchange_rate", " WHERE bm_invoice_index = " & index & "") = "" Then
                Return True
            Else
                Return False
            End If

        End Function
    End Class

End Namespace
