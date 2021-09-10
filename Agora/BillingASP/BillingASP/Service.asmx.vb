Imports System.IO
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Xml
Imports System
Imports SSO.Component
Imports AgoraLegacy
Imports eProcure.Component

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class Service
    Inherits System.Web.Services.WebService
    Dim objDB As New EAD.DBCom

    <WebMethod()> _
    Public Function getTransactionCount(ByVal xmlstr As String) As String
        Dim doc As New Xml.XmlDocument
        doc.LoadXml(xmlstr)
        Dim Companycount As Integer = CInt(doc.GetElementsByTagName("CompanyCount").Item(0).InnerText)
        Dim i, CompanySum As Integer
        Dim result, strsql As String
        Dim fDate, tDate, accID As String
        Dim ds As DataSet
        Dim dr As DataRow
        CompanySum = 0
        result = "<?xml version='1.0'?><ToBilling><PO>"
        '######### Start PO ###########
        For i = 0 To Companycount - 1
            fDate = doc.GetElementsByTagName("Fromdate" & i & "").Item(0).InnerText
            tDate = doc.GetElementsByTagName("Todate" & i & "").Item(0).InnerText
            accID = doc.GetElementsByTagName("Company" & i & "").Item(0).InnerText
            strsql = "SELECT pom_b_coy_id, COUNT(pom_po_date) AS total_po " & _
                    "FROM po_mstr WHERE pom_po_date BETWEEN '" & fDate & "' AND '" & tDate & "' " & _
                    "AND NOT pom_po_date IS NULL AND pom_b_coy_id='" & accID & "' GROUP BY pom_b_coy_id"
            ds = objDB.FillDsT(strsql)
            If ds.Tables(0).Rows.Count > 0 Then
                CompanySum = CompanySum + 1
                For Each dr In ds.Tables(0).Rows
                    result &= "<CompanyID" & CompanySum - 1 & ">" & dr("pom_b_coy_id") & "</CompanyID" & CompanySum - 1 & ">"
                    result &= "<UsageCount" & CompanySum - 1 & ">" & dr("total_po") & "</UsageCount" & CompanySum - 1 & ">"
                Next
            Else
                CompanySum = CompanySum + 1
                result &= "<CompanyID" & CompanySum - 1 & ">" & accID & "</CompanyID" & CompanySum - 1 & ">"
                result &= "<UsageCount" & CompanySum - 1 & ">0</UsageCount" & CompanySum - 1 & ">"
            End If
        Next
        result &= "<CompanyCount>" & CompanySum & "</CompanyCount>"
        result &= "</PO>"


        '########## Start RFQ #########
        result &= "<RFQ>"
        ds = New DataSet
        CompanySum = 0
        For i = 0 To Companycount - 1
            fDate = doc.GetElementsByTagName("Fromdate" & i & "").Item(0).InnerText
            tDate = doc.GetElementsByTagName("Todate" & i & "").Item(0).InnerText
            accID = doc.GetElementsByTagName("Company" & i & "").Item(0).InnerText
            strsql = "SELECT rm_coy_id, COUNT(rm_submission_date) AS total_rfq FROM rfq_mstr WHERE " & _
            "rm_submission_date BETWEEN '" & fDate & "' AND '" & tDate & "' " & _
            "AND NOT rm_submission_date IS NULL AND rm_coy_id='" & accID & "' " & _
            "GROUP BY rm_coy_id"
            ds = objDB.FillDsT(strsql)
            If ds.Tables(0).Rows.Count > 0 Then
                CompanySum = CompanySum + 1
                For Each dr In ds.Tables(0).Rows
                    result &= "<CompanyID" & CompanySum - 1 & ">" & dr("rm_coy_id") & "</CompanyID" & CompanySum - 1 & ">"
                    result &= "<UsageCount" & CompanySum - 1 & ">" & dr("total_rfq") & "</UsageCount" & CompanySum - 1 & ">"
                Next
            Else
                CompanySum = CompanySum + 1
                result &= "<CompanyID" & CompanySum - 1 & ">" & accID & "</CompanyID" & CompanySum - 1 & ">"
                result &= "<UsageCount" & CompanySum - 1 & ">0</UsageCount" & CompanySum - 1 & ">"
            End If
        Next
        result &= "<CompanyCount>" & CompanySum & "</CompanyCount>"
        result &= "</RFQ>"


        '########## Start QTN #########
        result &= "<QTN>"
        ds = New DataSet
        CompanySum = 0
        For i = 0 To Companycount - 1
            fDate = doc.GetElementsByTagName("Fromdate" & i & "").Item(0).InnerText
            tDate = doc.GetElementsByTagName("Todate" & i & "").Item(0).InnerText
            accID = doc.GetElementsByTagName("Company" & i & "").Item(0).InnerText
            strsql = "SELECT rrm_v_company_id, COUNT(rrm_created_on) AS total_qtn FROM rfq_replies_mstr WHERE " & _
            "rrm_created_on BETWEEN '" & fDate & "' AND '" & tDate & "' " & _
            "AND NOT rrm_created_on IS NULL AND rrm_v_company_id='" & accID & "' " & _
            "GROUP BY rrm_v_company_id"
            ds = objDB.FillDsT(strsql)
            If ds.Tables(0).Rows.Count > 0 Then
                CompanySum = CompanySum + 1
                For Each dr In ds.Tables(0).Rows
                    result &= "<CompanyID" & CompanySum - 1 & ">" & dr("rrm_v_company_id") & "</CompanyID" & CompanySum - 1 & ">"
                    result &= "<UsageCount" & CompanySum - 1 & ">" & dr("total_qtn") & "</UsageCount" & CompanySum - 1 & ">"
                Next
            Else
                CompanySum = CompanySum + 1
                result &= "<CompanyID" & CompanySum - 1 & ">" & accID & "</CompanyID" & CompanySum - 1 & ">"
                result &= "<UsageCount" & CompanySum - 1 & ">0</UsageCount" & CompanySum - 1 & ">"
            End If
        Next
        result &= "<CompanyCount>" & CompanySum & "</CompanyCount>"
        result &= "</QTN>"

        '########## Start Invoice #####
        result &= "<Invoice>"
        ds = New DataSet
        CompanySum = 0
        For i = 0 To Companycount - 1
            Try
                fDate = doc.GetElementsByTagName("Fromdate" & i & "").Item(0).InnerText
                tDate = doc.GetElementsByTagName("Todate" & i & "").Item(0).InnerText
                accID = doc.GetElementsByTagName("Company" & i & "").Item(0).InnerText
                'strsql = "SELECT im_s_coy_id, SUM(im_invoice_total) AS sum_invoice, pom_currency_code FROM invoice_mstr " & _
                '"INNER JOIN po_mstr ON pom_po_index = im_po_index" & _
                '" WHERE " & _
                '"im_created_on BETWEEN '" & fDate & "' AND '" & tDate & "' " & _
                '"AND im_s_coy_id='" & accID & "' " & _
                '"GROUP BY im_s_coy_id, pom_currency_code"
                '2015-10-07 CH: Calculate invoice amount exclude GST
                'strsql = "SELECT im_s_coy_id, SUM(id_received_qty * id_unit_cost) AS sum_invoice, pom_currency_code " & _
                '"FROM invoice_mstr " & _
                '"INNER JOIN po_mstr ON pom_po_index = im_po_index " & _
                '"INNER JOIN invoice_details ON id_invoice_no = im_invoice_no AND id_s_coy_id = im_s_coy_id " & _
                '"WHERE im_created_on BETWEEN '" & fDate & "' AND '" & tDate & "' " & _
                '"AND im_s_coy_id = '" & accID & "' " & _
                '"GROUP BY im_s_coy_id, pom_currency_code "
                strsql = "SELECT im_s_coy_id, SUM(invoice_amt + ship_amt) AS sum_invoice, pom_currency_code FROM " & _
                "(SELECT im_s_coy_id, im_invoice_no, SUM(ROUND(id_received_qty * id_unit_cost,2)) AS invoice_amt, " & _
                "IFNULL(im_ship_amt,0) AS ship_amt, im_invoice_total, pom_currency_code " & _
                "FROM invoice_mstr " & _
                "INNER JOIN po_mstr ON pom_po_index = im_po_index " & _
                "INNER JOIN invoice_details ON id_invoice_no = im_invoice_no AND id_s_coy_id = im_s_coy_id " & _
                "WHERE im_created_on BETWEEN '" & fDate & "' AND '" & tDate & "' " & _
                "AND im_s_coy_id = '" & accID & "' " & _
                "GROUP BY im_invoice_no, im_s_coy_id) tb " & _
                "GROUP BY im_s_coy_id, pom_currency_code "
                ds = objDB.FillDsT(strsql)
                If ds.Tables(0).Rows.Count > 0 Then
                    CompanySum = CompanySum + 1
                    For Each dr In ds.Tables(0).Rows
                        result &= "<CompanyID" & CompanySum - 1 & ">" & dr("im_s_coy_id") & "</CompanyID" & CompanySum - 1 & ">"
                        result &= "<UsageCount" & CompanySum - 1 & ">" & dr("sum_invoice") & "</UsageCount" & CompanySum - 1 & ">"
                        result &= "<Currency" & CompanySum - 1 & ">" & dr("pom_currency_code") & "</Currency" & CompanySum - 1 & ">"
                    Next
                Else
                    CompanySum = CompanySum + 1
                    result &= "<CompanyID" & CompanySum - 1 & ">" & accID & "</CompanyID" & CompanySum - 1 & ">"
                    result &= "<UsageCount" & CompanySum - 1 & ">0</UsageCount" & CompanySum - 1 & ">"
                    result &= "<Currency" & CompanySum - 1 & ">MYR</Currency" & CompanySum - 1 & ">"
                End If
            Catch ex As Exception
                WriteLog("(from BillingASP)Catch Exception: " & Now.Date.ToString & "-" & Now.TimeOfDay.ToString & vbCrLf & ex.Message)
                WriteLog("(from BillingASP)Stack Trace: " & Now.Date.ToString & "-" & Now.TimeOfDay.ToString & vbCrLf & ex.StackTrace)
            End Try
        Next
        result &= "<CompanyCount>" & CompanySum & "</CompanyCount>"
        result &= "</Invoice>"

        '########## Start Credit Note #####
        result &= "<Creditnote>"
        ds = New DataSet
        CompanySum = 0
        For i = 0 To Companycount - 1
            Try
                fDate = doc.GetElementsByTagName("Fromdate" & i & "").Item(0).InnerText
                tDate = doc.GetElementsByTagName("Todate" & i & "").Item(0).InnerText
                accID = doc.GetElementsByTagName("Company" & i & "").Item(0).InnerText
                'strsql = "SELECT cnm_cn_s_coy_id, SUM(cnm_cn_total) AS sum_cn, cnm_currency_code " & _
                '"FROM invoice_mstr " & _
                '"INNER JOIN credit_note_mstr ON cnm_cn_s_coy_id = im_s_coy_id AND cnm_inv_no = im_invoice_no " & _
                '"WHERE im_created_on BETWEEN '" & fDate & "' AND '" & tDate & "' " & _
                '"AND cnm_created_date BETWEEN '" & fDate & "' AND '" & tDate & "'" & _
                '"AND im_s_coy_id = '" & accID & "' " & _
                '"GROUP BY cnm_cn_s_coy_id, cnm_currency_code"
                strsql = "SELECT cnm_cn_s_coy_id, SUM(cnd_qty * cnd_unit_cost) AS sum_cn, cnm_currency_code " & _
                "FROM invoice_mstr " & _
                "INNER JOIN credit_note_mstr ON cnm_cn_s_coy_id = im_s_coy_id AND cnm_inv_no = im_invoice_no " & _
                "INNER JOIN credit_note_details ON cnd_cn_s_coy_id = cnm_cn_s_coy_id AND cnd_cn_no = cnm_cn_no " & _
                "WHERE im_created_on BETWEEN '" & fDate & "' AND '" & tDate & "' " & _
                "AND cnm_created_date BETWEEN '" & fDate & "' AND '" & tDate & "' " & _
                "AND im_s_coy_id = '" & accID & "' " & _
                "GROUP BY cnm_cn_s_coy_id, cnm_currency_code"
                ds = objDB.FillDsT(strsql)
                If ds.Tables(0).Rows.Count > 0 Then
                    CompanySum = CompanySum + 1
                    For Each dr In ds.Tables(0).Rows
                        result &= "<CompanyID" & CompanySum - 1 & ">" & dr("cnm_cn_s_coy_id") & "</CompanyID" & CompanySum - 1 & ">"
                        result &= "<UsageCount" & CompanySum - 1 & ">" & dr("sum_cn") & "</UsageCount" & CompanySum - 1 & ">"
                        result &= "<Currency" & CompanySum - 1 & ">" & dr("cnm_currency_code") & "</Currency" & CompanySum - 1 & ">"
                    Next
                Else
                    CompanySum = CompanySum + 1
                    result &= "<CompanyID" & CompanySum - 1 & ">" & accID & "</CompanyID" & CompanySum - 1 & ">"
                    result &= "<UsageCount" & CompanySum - 1 & ">0</UsageCount" & CompanySum - 1 & ">"
                    result &= "<Currency" & CompanySum - 1 & ">MYR</Currency" & CompanySum - 1 & ">"
                End If
            Catch ex As Exception
                WriteLog("(from BillingASP)Catch Exception: " & Now.Date.ToString & "-" & Now.TimeOfDay.ToString & vbCrLf & ex.Message)
                WriteLog("(from BillingASP)Stack Trace: " & Now.Date.ToString & "-" & Now.TimeOfDay.ToString & vbCrLf & ex.StackTrace)
            End Try
        Next
        result &= "<CompanyCount>" & CompanySum & "</CompanyCount>"
        result &= "</Creditnote>"

        '########## Start Debit Note #####
        result &= "<Debitnote>"
        ds = New DataSet
        CompanySum = 0
        For i = 0 To Companycount - 1
            Try
                fDate = doc.GetElementsByTagName("Fromdate" & i & "").Item(0).InnerText
                tDate = doc.GetElementsByTagName("Todate" & i & "").Item(0).InnerText
                accID = doc.GetElementsByTagName("Company" & i & "").Item(0).InnerText
                'strsql = "SELECT dnm_dn_s_coy_id, SUM(dnm_dn_total) AS sum_dn, dnm_currency_code " & _
                '"FROM debit_note_mstr " & _
                '"WHERE dnm_created_date BETWEEN '" & fDate & "' AND '" & tDate & "' " & _
                '"AND dnm_dn_s_coy_id = '" & accID & "' " & _
                '"GROUP BY dnm_dn_s_coy_id, dnm_currency_code"
                strsql = "SELECT dnm_dn_s_coy_id, SUM(dnd_qty * dnd_unit_cost) AS sum_dn, dnm_currency_code " & _
                "FROM debit_note_mstr " & _
                "INNER JOIN debit_note_details ON dnd_dn_s_coy_id = dnm_dn_s_coy_id AND dnd_dn_no = dnm_dn_no " & _
                "WHERE dnm_created_date BETWEEN '" & fDate & "' AND '" & tDate & "' " & _
                "AND dnm_dn_s_coy_id = '" & accID & "' " & _
                "GROUP BY dnm_dn_s_coy_id, dnm_currency_code"
                ds = objDB.FillDsT(strsql)
                If ds.Tables(0).Rows.Count > 0 Then
                    CompanySum = CompanySum + 1
                    For Each dr In ds.Tables(0).Rows
                        result &= "<CompanyID" & CompanySum - 1 & ">" & dr("dnm_dn_s_coy_id") & "</CompanyID" & CompanySum - 1 & ">"
                        result &= "<UsageCount" & CompanySum - 1 & ">" & dr("sum_dn") & "</UsageCount" & CompanySum - 1 & ">"
                        result &= "<Currency" & CompanySum - 1 & ">" & dr("dnm_currency_code") & "</Currency" & CompanySum - 1 & ">"
                    Next
                Else
                    CompanySum = CompanySum + 1
                    result &= "<CompanyID" & CompanySum - 1 & ">" & accID & "</CompanyID" & CompanySum - 1 & ">"
                    result &= "<UsageCount" & CompanySum - 1 & ">0</UsageCount" & CompanySum - 1 & ">"
                    result &= "<Currency" & CompanySum - 1 & ">MYR</Currency" & CompanySum - 1 & ">"
                End If
            Catch ex As Exception
                WriteLog("(from BillingASP)Catch Exception: " & Now.Date.ToString & "-" & Now.TimeOfDay.ToString & vbCrLf & ex.Message)
                WriteLog("(from BillingASP)Stack Trace: " & Now.Date.ToString & "-" & Now.TimeOfDay.ToString & vbCrLf & ex.StackTrace)
            End Try
        Next
        result &= "<CompanyCount>" & CompanySum & "</CompanyCount>"
        result &= "</Debitnote>"

        ''######### Start PO ###########
        'result = "<PO>"
        'strsql = "SELECT pom_b_coy_id, COUNT(pom_po_date) AS total_po " & _
        '"FROM po_mstr WHERE pom_po_date BETWEEN '" & fDate & "' AND '" & tDate & "' " & _
        '"AND NOT pom_po_date IS NULL AND pom_b_coy_id IN (" & accID & ") GROUP BY pom_b_coy_id"
        'ds = objDB.FillDs(strsql)
        'result &= "<CompanyCount>" & ds.Tables(0).Rows.Count & "</CompanyCount>"
        'For Each dr In ds.Tables(0).Rows
        '    result &= "<CompanyID" & count & ">" & dr("pom_b_coy_id") & "</CompanyID" & count & ">"
        '    result &= "<UsageCount" & count & ">" & dr("total_po") & "</UsageCount" & count & ">"
        '    count = count + 1
        'Next
        'result &= "</PO>"

        ''##########  End PO ###########
        ''########## Start RFQ #########
        'strsql = "SELECT rm_coy_id, COUNT(rm_submission_date) AS total_rfq FROM rfq_mstr WHERE " & _
        '"rm_submission_date BETWEEN '" & fDate & "' AND '" & tDate & "' " & _
        '"AND NOT rm_submission_date IS NULL AND rm_coy_id IN (" & accID & ")" & _
        '"GROUP BY rm_coy_id"
        'ds = New DataSet
        'ds = objDB.FillDs(strsql)
        'count = 0
        'result &= "<RFQ><CompanyCount>" & ds.Tables(0).Rows.Count & "</CompanyCount>"
        'For Each dr In ds.Tables(0).Rows
        '    result &= "<CompanyID" & count & ">" & dr("rm_coy_id") & "</CompanyID" & count & ">"
        '    result &= "<UsageCount" & count & ">" & dr("total_rfq") & "</UsageCount" & count & ">"
        '    count = count + 1
        'Next
        'result &= "</RFQ>"
        ''##########  End RFQ ##########
        ''########## Start QTN #########
        'strsql = "SELECT rrm_v_company_id, COUNT(rrm_created_on) AS total_qtn FROM rfq_replies_mstr WHERE " & _
        '"rrm_created_on BETWEEN '" & fDate & "' AND '" & tDate & "' " & _
        '"AND NOT rrm_created_on IS NULL AND rrm_v_company_id IN (" & accID & ")" & _
        '"GROUP BY rrm_v_company_id"
        'ds = New DataSet
        'ds = objDB.FillDs(strsql)
        'count = 0
        'result &= "<QTN><CompanyCount>" & ds.Tables(0).Rows.Count & "</CompanyCount>"
        'For Each dr In ds.Tables(0).Rows
        '    result &= "<CompanyID" & count & ">" & dr("rrm_v_company_id") & "</CompanyID" & count & ">"
        '    result &= "<UsageCount" & count & ">" & dr("total_qtn") & "</UsageCount" & count & ">"
        '    count = count + 1
        'Next
        'result &= "</QTN>"
        ''##########  End QTN ##########
        ''########## Start Invoice #####
        'strsql = "SELECT im_s_coy_id, SUM(im_invoice_total) AS sum_invoice, pom_currency_code FROM invoice_mstr " & _
        '"INNER JOIN po_mstr ON pom_po_index = im_po_index" & _
        '" WHERE " & _
        '"im_created_on BETWEEN '" & fDate & "' AND '" & tDate & "' " & _
        '"AND im_s_coy_id IN (" & accID & ") " & _
        '"GROUP BY im_s_coy_id, pom_currency_code"
        'strsql = "SELECT im_s_coy_id, SUM(im_invoice_total) AS sum_invoice FROM invoice_mstr WHERE " & _
        '"im_created_on BETWEEN '" & fDate & "' AND '" & tDate & "' " & _
        '"AND im_s_coy_id IN (" & accID & ")" & _
        '"GROUP BY im_s_coy_id"
        'ds = New DataSet
        'ds = objDB.FillDs(strsql)
        'count = 0
        'result &= "<Invoice><CompanyCount>" & ds.Tables(0).Rows.Count & "</CompanyCount>"
        'For Each dr In ds.Tables(0).Rows
        '    result &= "<CompanyID" & count & ">" & dr("im_s_coy_id") & "</CompanyID" & count & ">"
        '    result &= "<UsageCount" & count & ">" & dr("sum_invoice") & "</UsageCount" & count & ">"
        '    result &= "<Currency" & count & ">" & dr("pom_currency_code") & "</Currency" & count & ">"
        '    count = count + 1
        'Next
        'result &= "</Invoice>"
        ''##########  End Invoice ######
        result &= "</ToBilling>"
        Return result
    End Function

    <WebMethod()> _
    Public Function SyncAccount(ByVal strxml As String) As String
        Dim doc As New Xml.XmlDocument
        'to read the & symbol        
        'strxml = "<?xml version='1.0'?><ToProcurement><CompanyID>TestBuyer2a1a</CompanyID><CompanyName>Test Company</CompanyName><CompanyType>BUYER</CompanyType><UserLicense>5</UserLicense><Addr1>Test address 1</Addr1><Addr2>Test address 2</Addr2><Addr3>Test address 3</Addr3><City>Kuala Lumpur</City><State>06</State><Postcode>31350</Postcode><Country>MY</Country><BusinessRegNo>Bizreg123</BusinessRegNo><Fax>0349849844</Fax><Currency>MYR</Currency><Contact>Craven</Contact><Phone>03151564694</Phone><Email>testmail@hotmail.com</Email><JoinDate>2012-05-16</JoinDate><UserName>admin</UserName><Password>qkgHVf</Password><UserID>hubadmin</UserID><AddBatch>Y</AddBatch><ProcureType>1</ProcureType><BuyerID>ba</BuyerID><PurchasingOfficerID>po</PurchasingOfficerID><StoreKeeperID>sk</StoreKeeperID><PurchasingManagerID>pm</PurchasingManagerID></ToProcurement>"
        strxml = strxml.Replace("&", "&#38;")
        doc.LoadXml(strxml)
        'Dim strsql As String
        Dim UserID, coyid, coyname, entdt, coytype, licenseuser, addr1, addr2, addr3, postcode, city As String
        Dim state, ProcureType, country, phone, fax, email, businessregno, currency, contact, username, password, AddBatch As String
        Dim BuyerID, POfficerID, SKeeperID, PManagerID, ProcureURL As String
        Dim strApp(0) As String
        coyid = doc.GetElementsByTagName("CompanyID").Item(0).InnerText
        coyname = doc.GetElementsByTagName("CompanyName").Item(0).InnerText
        entdt = doc.GetElementsByTagName("JoinDate").Item(0).InnerText
        coytype = UCase(doc.GetElementsByTagName("CompanyType").Item(0).InnerText)
        licenseuser = doc.GetElementsByTagName("UserLicense").Item(0).InnerText
        addr1 = doc.GetElementsByTagName("Addr1").Item(0).InnerText
        addr2 = doc.GetElementsByTagName("Addr2").Item(0).InnerText
        addr3 = doc.GetElementsByTagName("Addr3").Item(0).InnerText
        postcode = doc.GetElementsByTagName("Postcode").Item(0).InnerText
        city = doc.GetElementsByTagName("City").Item(0).InnerText
        state = doc.GetElementsByTagName("State").Item(0).InnerText
        country = doc.GetElementsByTagName("Country").Item(0).InnerText
        phone = doc.GetElementsByTagName("Phone").Item(0).InnerText
        fax = doc.GetElementsByTagName("Fax").Item(0).InnerText
        email = doc.GetElementsByTagName("Email").Item(0).InnerText
        businessregno = doc.GetElementsByTagName("BusinessRegNo").Item(0).InnerText
        currency = doc.GetElementsByTagName("Currency").Item(0).InnerText
        contact = doc.GetElementsByTagName("Contact").Item(0).InnerText
        username = doc.GetElementsByTagName("UserName").Item(0).InnerText
        password = doc.GetElementsByTagName("Password").Item(0).InnerText
        UserID = doc.GetElementsByTagName("UserID").Item(0).InnerText
        AddBatch = doc.GetElementsByTagName("AddBatch").Item(0).InnerText
        ProcureType = doc.GetElementsByTagName("ProcureType").Item(0).InnerText
        ProcureURL = doc.GetElementsByTagName("ProcureURL").Item(0).InnerText
        Dim objComp As New Companies
        Dim objCompDetails As New Company
        objCompDetails.CoyId = coyid
        objCompDetails.CoyName = coyname
        objCompDetails.BankName = ""
        objCompDetails.AccountNo = ""
        objCompDetails.BankCode = ""
        objCompDetails.BranchCode = ""
        objCompDetails.Address1 = addr1
        objCompDetails.Address2 = addr2
        objCompDetails.Address3 = addr3
        objCompDetails.City = city
        objCompDetails.PostCode = postcode
        objCompDetails.Phone = phone
        objCompDetails.Fax = fax
        objCompDetails.Email = email
        objCompDetails.BusinessRegNo = businessregno
        objCompDetails.TaxRegNo = ""
        objCompDetails.PwdDuration = "60"
        objCompDetails.SubStart = entdt
        objCompDetails.SubEnd = ""
        objCompDetails.TaxRegNo = ""
        objCompDetails.Status = "A"
        objCompDetails.TaxCalBy = ""
        objCompDetails.BCMSetting = "0"
        objCompDetails.FinDeptMode = ""
        objCompDetails.InvAppr = ""
        objCompDetails.PrivLabeling = ""
        objCompDetails.Skins = "1"
        objCompDetails.TrainDemo = ""
        objCompDetails.SKU = ""
        objCompDetails.TransNo = "999999999"
        objCompDetails.ContactPerson = contact
        objCompDetails.LicenseUsers = licenseuser
        objCompDetails.CoyType = coytype
        If objCompDetails.CoyType = "VENDOR" Then
            objCompDetails.ReportUsers = "2"
        Else
            objCompDetails.ReportUsers = "3"
        End If
        objCompDetails.Country = country
        objCompDetails.State = state
        objCompDetails.Currency = currency
        objCompDetails.PaymentTerm = "1"
        objCompDetails.PaymentMethod = "Cheque"
        objCompDetails.ParentCoy = ""
        objCompDetails.Package = ""
        objCompDetails.MultiInvAppr = ""
        objCompDetails.BACanPO = ""
        objCompDetails.CoyLogo = ""
        objCompDetails.Actual_TC = ""
        objCompDetails.TC = ""
        strApp(0) = "eProcure"
        Dim result As Boolean
        'WriteLog("Before call add ")
        result = objComp.AddCompanyByBilling(objCompDetails, UserID, strApp, ProcureType)
        'WriteLog("After call add ")
        'Create User
        Dim objUser As New Users
        Dim objUserDetail As New User
        Dim objPwd As New RandomPassword
        Dim objMail As New Email
        Dim strAppPackageId, strUserGroupId As String
        strAppPackageId = "eProcure"
        objUserDetail.UserID = username
        objUserDetail.Name = contact
        objUserDetail.Email = email
        objUserDetail.Status = "A"
        objUserDetail.DeleteInd = "N"
        objUserDetail.NewPwdInd = "N"
        objUserDetail.CompanyID = coyid
        ' check whether is from batch registration or single registration
        If AddBatch = "Y" Then
            'for vendor
            If coytype = "BUYER" Then
                objUserDetail.UserGroup = "FTN (AO),Admin,FTN (IO),FTN (Buyer Admin),FTN (FM),FTN (RA)"

            ElseIf coytype = "VENDOR" Then
                objUserDetail.UserGroup = "AdminVendor,vendor,venadmin"
            Else
                objUserDetail.UserGroup = "AdminBoth"
            End If

        Else
            ' normal insert
            If coytype = "BUYER" Then
                If ProcureType = "S" Then
                    strUserGroupId = "FTN (AO),Admin,FTN (IO),FTN (Buyer Admin),FTN (FM),FTN (RA)"
                Else
                    strUserGroupId = "Admin"
                End If
            ElseIf coytype = "VENDOR" Then
                strUserGroupId = "AdminVendor"
            Else
                strUserGroupId = "AdminBoth"
            End If
            objUserDetail.UserGroup = strUserGroupId
        End If

        objUserDetail.Password = objPwd.GenerateAutoPwd()
        objUserDetail.NextExpiredDt = objUser.GetPwdExpireDt(True)

        If objUser.AddUserByBilling(objUserDetail, True, strAppPackageId, UserID) Then
            'email already send by billing
            'objMail.sendNotification(EmailType.NewUserAccount, objUserDetail.UserDetail.UserID, objUserDetail.CompanyId, objUserDetail.CompanyId, "", "", objUserDetail.UserDetail.Password, "")
            result = True
        Else
            result = False
        End If
        'batch registration for buyer need create 4 more user
        If AddBatch = "Y" Then
            If coytype = "BUYER" And ProcureType = "S" Then
                'Buyer ID
                BuyerID = doc.GetElementsByTagName("BuyerID").Item(0).InnerText
                objUserDetail = New User
                objUserDetail.UserID = BuyerID
                objUserDetail.Name = contact
                objUserDetail.Email = email
                objUserDetail.Status = "A"
                objUserDetail.DeleteInd = "N"
                objUserDetail.NewPwdInd = "Y"
                objUserDetail.CompanyID = coyid
                objUserDetail.CompanyID = coyid
                objUserDetail.Password = objPwd.GenerateAutoPwd()
                objUserDetail.NextExpiredDt = objUser.GetPwdExpireDt(True)
                objUserDetail.UserGroup = "FTN (Buyer)"
                objUser.AddUserByBilling(objUserDetail, True, strAppPackageId, UserID)
                objMail.sendNewUserAccBilling(EmailType.NewUserAccount, objUserDetail.UserID, objUserDetail.CompanyID, objUserDetail.CompanyID, "", "", objUserDetail.Password, "", ProcureURL)

                'Purchasing Officer ID
                POfficerID = doc.GetElementsByTagName("PurchasingOfficerID").Item(0).InnerText
                objUserDetail = New User
                objUserDetail.UserID = POfficerID
                objUserDetail.Name = contact
                objUserDetail.Email = email
                objUserDetail.Status = "A"
                objUserDetail.DeleteInd = "N"
                objUserDetail.NewPwdInd = "Y"
                objUserDetail.CompanyID = coyid
                objUserDetail.Password = objPwd.GenerateAutoPwd()
                objUserDetail.NextExpiredDt = objUser.GetPwdExpireDt(True)
                objUserDetail.UserGroup = "FTN (FO),FTN (PO)"
                objUserDetail.strFixedRole = "Purchasing Officer"
                objUser.AddUserByBilling(objUserDetail, True, strAppPackageId, UserID)
                objMail.sendNewUserAccBilling(EmailType.NewUserAccount, objUserDetail.UserID, objUserDetail.CompanyID, objUserDetail.CompanyID, "", "", objUserDetail.Password, "", ProcureURL)

                'Store Keeper ID
                SKeeperID = doc.GetElementsByTagName("StoreKeeperID").Item(0).InnerText
                objUserDetail = New User
                objUserDetail.UserID = SKeeperID
                objUserDetail.Name = contact
                objUserDetail.Email = email
                objUserDetail.Status = "A"
                objUserDetail.DeleteInd = "N"
                objUserDetail.NewPwdInd = "Y"
                objUserDetail.CompanyID = coyid
                objUserDetail.Password = objPwd.GenerateAutoPwd()
                objUserDetail.NextExpiredDt = objUser.GetPwdExpireDt(True)
                objUserDetail.UserGroup = "FTN (SK),FTN (QC)"
                objUser.AddUserByBilling(objUserDetail, True, strAppPackageId, UserID)
                objMail.sendNewUserAccBilling(EmailType.NewUserAccount, objUserDetail.UserID, objUserDetail.CompanyID, objUserDetail.CompanyID, "", "", objUserDetail.Password, "", ProcureURL)

                'Purchasing Manager ID
                PManagerID = doc.GetElementsByTagName("PurchasingManagerID").Item(0).InnerText
                objUserDetail = New User
                objUserDetail.UserID = PManagerID
                objUserDetail.Name = contact
                objUserDetail.Email = email
                objUserDetail.Status = "A"
                objUserDetail.DeleteInd = "N"
                objUserDetail.NewPwdInd = "Y"
                objUserDetail.CompanyID = coyid
                objUserDetail.Password = objPwd.GenerateAutoPwd()
                objUserDetail.NextExpiredDt = objUser.GetPwdExpireDt(True)
                objUserDetail.UserGroup = "FTN (RA),FTN (PM),FTN (AO)"
                objUser.AddUserByBilling(objUserDetail, True, strAppPackageId, UserID)
                objMail.sendNewUserAccBilling(EmailType.NewUserAccount, objUserDetail.UserID, objUserDetail.CompanyID, objUserDetail.CompanyID, "", "", objUserDetail.Password, "", ProcureURL)

            End If
        End If

        If result Then
            Return "true"
        Else
            Return "false"
        End If

    End Function

    Private filePath As String
    Private fileStream As FileStream
    Private streamWriter As StreamWriter

    Public Sub OpenFile()
        Dim strPath As String
        strPath = "C:\logbillingasp\Error.log"
        If System.IO.File.Exists(strPath) Then
            fileStream = New FileStream(strPath, FileMode.Append, FileAccess.Write)
        Else
            fileStream = New FileStream(strPath, FileMode.Create, FileAccess.Write)
        End If
        StreamWriter = New StreamWriter(fileStream)
    End Sub

    Public Sub WriteLog(ByVal strComments As String)
        OpenFile()
        StreamWriter.WriteLine(strComments)
        CloseFile()
    End Sub

    Public Sub CloseFile()
        StreamWriter.Close()
        fileStream.Close()
    End Sub

    <WebMethod()> _
    Public Function SuspendAccount(ByVal xmlstr As String) As String
        Dim doc As New Xml.XmlDocument
        doc.LoadXml(xmlstr)
        Dim companyid, sqlupdate As String
        companyid = doc.GetElementsByTagName("CompanyID").Item(0).InnerText
        sqlupdate = "UPDATE company_mstr SET CM_DELETED='Y', CM_STATUS='I' WHERE CM_COY_ID='" & companyid & "'"
        If objDB.Execute(sqlupdate) Then
            Return True
        Else
            Return False
        End If

    End Function
    <WebMethod()> _
    Public Function SyncUserLicense(ByVal xmlstr As String) As String
        Dim doc As New Xml.XmlDocument
        doc.LoadXml(xmlstr)
        Dim compid, userlicense, sqlupdate As String
        compid = doc.GetElementsByTagName("CompanyID").Item(0).InnerText
        userlicense = doc.GetElementsByTagName("UserLicense").Item(0).InnerText
        sqlupdate = "UPDATE company_mstr SET CM_LICENSE_USERS='" & userlicense & "' WHERE CM_COY_ID='" & compid & "'"
        If objDB.Execute(sqlupdate) Then
            Return True
        Else
            Return False
        End If
        Return True
    End Function
    <WebMethod()> _
    Public Function SyncPass(ByVal xmlstr As String) As String
        Dim doc As New Xml.XmlDocument
        doc.LoadXml(xmlstr)        
        Dim compid, passw, UserID, sqlpwexpire, sqlpw, expire, passduration As String
        compid = doc.GetElementsByTagName("CompanyID").Item(0).InnerText
        passw = doc.GetElementsByTagName("Password").Item(0).InnerText
        UserID = doc.GetElementsByTagName("UserID").Item(0).InnerText
        passw = Common.EncryptString(passw)
        sqlpw = "UPDATE user_mstr SET UM_PASSWORD= CAST('" & passw & "' AS BINARY) WHERE UM_COY_ID='" & compid & "' AND UM_USER_ID='" & UserID & "'"
        objDB.Execute(sqlpw)
        'For update Password Expire Date        
        passduration = objDB.Get1Column("company_mstr", "CM_PWD_DURATION", " WHERE CM_COY_ID='" & compid & "'")
        expire = DateAdd(DateInterval.Day, passduration - 1, Today)
        sqlpwexpire = "UPDATE USER_MSTR SET UM_NEXT_EXPIRE_DT = " & Common.ConvertDate(expire) & " WHERE UM_COY_ID='" & compid & "' AND UM_USER_ID='" & UserID & "'"
        If objDB.Execute(sqlpwexpire) Then
            Return True
        Else
            Return False
        End If
        Return True
    End Function
    <WebMethod()> _
    Public Function SyncAccountDetail(ByVal xmlstr As String) As String
        Dim doc As New Xml.XmlDocument
        doc.LoadXml(xmlstr)
        Dim compid, ContactPerson, sqlupdate, sqlupdateusermstr, Fax, Email, Phone, UserID, existingEmail As String
        compid = doc.GetElementsByTagName("CompanyID").Item(0).InnerText
        ContactPerson = doc.GetElementsByTagName("ContactPerson").Item(0).InnerText
        Phone = doc.GetElementsByTagName("Phone").Item(0).InnerText
        Fax = doc.GetElementsByTagName("Fax").Item(0).InnerText
        Email = doc.GetElementsByTagName("Email").Item(0).InnerText
        UserID = doc.GetElementsByTagName("UserID").Item(0).InnerText

        existingEmail = objDB.Get1Column("user_mstr", "UM_EMAIL", " WHERE UM_COY_ID='" & compid & "' AND UM_USER_ID='" & UserID & "'")
        If existingEmail = Email Then
            sqlupdateusermstr = "UPDATE user_mstr SET UM_EMAIL='" & Email & "' WHERE UM_COY_ID='" & compid & "' AND UM_USER_ID='" & UserID & "'"
            objDB.Execute(sqlupdateusermstr)
        Else
            sqlupdateusermstr = "UPDATE user_mstr SET UM_EMAIL='" & Email & "', UM_POLICY_AGREE_DATE = NULL WHERE UM_COY_ID='" & compid & "' AND UM_USER_ID='" & UserID & "'"
            objDB.Execute(sqlupdateusermstr)
        End If

        sqlupdate = "UPDATE COMPANY_MSTR SET CM_FAX='" & Fax & "', CM_EMAIL='" & Email & "', CM_PHONE='" & Phone & "', CM_CONTACT='" & ContactPerson & "' WHERE CM_COY_ID='" & compid & "'"
        If objDB.Execute(sqlupdate) Then
            Return True
        Else
            Return False
        End If
        Return True
    End Function
    <WebMethod()> _
    Public Function UpdateExpiredDate(ByVal xmlstr As String) As String
        Dim doc As New Xml.XmlDocument
        doc.LoadXml(xmlstr)
        Dim compid, UserID, passduration, expire, sqlpwexpire As String
        compid = doc.GetElementsByTagName("CompanyID").Item(0).InnerText        
        UserID = doc.GetElementsByTagName("UserID").Item(0).InnerText

        'For update Password Expire Date        
        passduration = objDB.Get1Column("login_policy", "lp_value", " WHERE lp_param='NEW_PASSWORD_DURATION'")
        expire = DateAdd(DateInterval.Day, passduration - 1, Today)
        sqlpwexpire = "UPDATE USER_MSTR SET UM_NEXT_EXPIRE_DT = " & Common.ConvertDate(expire) & " WHERE UM_COY_ID='" & compid & "' AND UM_USER_ID='" & UserID & "'"

        If objDB.Execute(sqlpwexpire) Then
            Return True
        Else
            Return False
        End If
        Return True
    End Function
    <WebMethod()> _
    Public Function GetExpiredDate(ByVal xmlstr As String) As String
        Dim doc As New Xml.XmlDocument
        doc.LoadXml(xmlstr)
        Dim compid, UserID, expiredate, resultxml As String
        compid = doc.GetElementsByTagName("CompanyID").Item(0).InnerText
        UserID = doc.GetElementsByTagName("UserID").Item(0).InnerText

        'For update Password Expire Date        
        expiredate = objDB.Get1Column("user_mstr", "UM_NEXT_EXPIRE_DT", " WHERE um_coy_id='" & compid & "' AND um_user_id='" & UserID & "'")
        resultxml = "<?xml version='1.0'?><ToBilling><Date>"
        resultxml &= expiredate
        resultxml &= "</Date></ToBilling>"
        Return resultxml
    End Function
    'webservice method to send min & max password char to billing
    <WebMethod()> _
    Public Function LoginPolicy() As String
        Dim lpparammin, lpparammax, resultxml As String
        Dim maxchar, minchar As Integer
        lpparammin = "PASSWORD_LENGTH_MIN"
        lpparammax = "PASSWORD_LENGTH_MAX"

        minchar = objDB.Get1Column(" login_policy ", " LP_VALUE ", " WHERE LP_PARAM='" & lpparammin & "'")
        maxchar = objDB.Get1Column(" login_policy ", " LP_VALUE ", " WHERE LP_PARAM='" & lpparammax & "'")
        resultxml = "<?xml version='1.0'?><ToBilling><MinChar>"
        resultxml &= minchar
        resultxml &= "</MinChar><MaxChar>"
        resultxml &= maxchar
        resultxml &= "</MaxChar></ToBilling>"
        Return resultxml
    End Function

    'webservice method to check value UM_POLICY_AGREE_DATE in user_mstrs and send to billing
    <WebMethod()> _
    Public Function PolicyAgree(ByVal xmlstr As String) As String
        Dim doc As New Xml.XmlDocument
        doc.LoadXml(xmlstr)
        Dim compid, UserID, agreedate, resultxml As String
        compid = doc.GetElementsByTagName("CompanyID").Item(0).InnerText
        UserID = doc.GetElementsByTagName("UserID").Item(0).InnerText

        'agreedate = Common.parseNull(objDB.Get1Column(" user_mstr ", " UM_POLICY_AGREE_DATE ", " WHERE UM_COY_ID='" & compid & "' AND UM_USER_ID='" & UserID & "'"))
        agreedate = Common.parseNull(objDB.Get1ColumnCheckNull(" user_mstr ", " UM_POLICY_AGREE_DATE ", " WHERE UM_COY_ID='" & compid & "' AND UM_USER_ID='" & UserID & "'"))
        resultxml = "<?xml version='1.0'?><ToBilling><AgreePolicyDate>"
        resultxml &= agreedate
        resultxml &= "</AgreePolicyDate></ToBilling>"
        Return resultxml
    End Function

    'webservice method to update value UM_POLICY_AGREE_DATE in user_mstrs table(user click agree for first time in billing system)
    <WebMethod()> _
    Public Function PolicyAgreeUpdate(ByVal xmlstr As String) As String
        Dim doc As New Xml.XmlDocument
        doc.LoadXml(xmlstr)
        Dim compid, UserID, sqlupdate, DateTime As String
        compid = doc.GetElementsByTagName("CompanyID").Item(0).InnerText
        UserID = doc.GetElementsByTagName("UserID").Item(0).InnerText
        DateTime = doc.GetElementsByTagName("DateTime").Item(0).InnerText

        sqlupdate = "UPDATE USER_MSTR SET UM_POLICY_AGREE_DATE='" & DateTime & "' WHERE UM_COY_ID='" & compid & "' AND UM_USER_ID='" & UserID & "'"
        If objDB.Execute(sqlupdate) Then
            Return True
        Else
            Return False
        End If
        Return True
    End Function
End Class
