Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Public Class PreviewDebitNote1
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher
    Dim objDb As New EAD.DBCom

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PreviewDebitNote()
    End Sub

    Private Sub PreviewDebitNote()
        Dim ds, ds1 As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim da1 As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strSQL As String = ""
        Dim a As Integer = 0
        Dim strNo As String = ""
        Dim strTaxInv As String = "N"

        'strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("vcomid"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")
        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request.QueryString("SCoyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand

            With cmd
                .Connection = conn
                .CommandText = CommandType.Text
                .CommandText = "SELECT im_gst_invoice, DNM_DN_NO FROM debit_note_mstr " &
                                "INNER JOIN invoice_mstr ON im_invoice_no = dnm_inv_no AND im_s_coy_Id = dnm_dn_s_coy_id AND im_b_coy_Id = dnm_dn_b_coy_id " &
                                "WHERE (dnm_dn_b_coy_id = '" & Request.QueryString("BCoyID") & "') AND (dnm_dn_s_coy_id = '" & Request.QueryString("SCoyID") & "') " &
                                "AND (dnm_dn_no = '" & Request.QueryString("DN_NO") & "')"
            End With

            da1 = New MySqlDataAdapter(cmd)
            'da1.SelectCommand.Parameters.Add(New MySqlParameter("@prmSCoyID", Request.QueryString("SCoyID")))
            'da1.SelectCommand.Parameters.Add(New MySqlParameter("@prmBCoyID", Request.QueryString("BCoyID")))
            'da1.SelectCommand.Parameters.Add(New MySqlParameter("@prmDNNo", Request.QueryString("DN_NO")))
            da1.Fill(ds1)

            If ds1.Tables(0).Rows.Count > 0 Then
                strNo = ds1.Tables(0).Rows(0)("DNM_DN_NO")
                strTaxInv = ds1.Tables(0).Rows(0)("im_gst_invoice")
            End If

            If strTaxInv = "Y" Then ''For Debit Note
                With cmd
                    .Connection = conn
                    .CommandType = CommandType.Text
                    '.CommandText = "SELECT debit_note_mstr.DNM_DN_INDEX, debit_note_mstr.DNM_DN_B_COY_ID, debit_note_mstr.DNM_DN_S_COY_ID, debit_note_mstr.DNM_DN_NO, " & _
                    '                "debit_note_mstr.DNM_DN_DATE, debit_note_mstr.DNM_DN_TYPE, debit_note_mstr.DNM_ADDR_LINE1, debit_note_mstr.DNM_ADDR_LINE2, " & _
                    '                "debit_note_mstr.DNM_ADDR_LINE3, debit_note_mstr.DNM_POSTCODE, debit_note_mstr.DNM_CITY,debit_note_mstr.DNM_STATE, " & _
                    '                "debit_note_mstr.DNM_COUNTRY, debit_note_mstr.DNM_INV_NO, debit_note_mstr.DNM_CURRENCY_CODE,  IF(debit_note_mstr.DNM_EXCHANGE_RATE IS NULL OR debit_note_mstr.DNM_EXCHANGE_RATE = 0,1,debit_note_mstr.DNM_EXCHANGE_RATE) AS DNM_EXCHANGE_RATE, " & _
                    '                "debit_note_mstr.DNM_REMARKS, debit_note_mstr.DNM_DN_STATUS, debit_note_mstr.DNM_STATUS_CHANGED_BY, debit_note_mstr.DNM_STATUS_CHANGED_ON, " & _
                    '                "debit_note_mstr.DNM_FM_APPROVED_DATE, debit_note_mstr.DNM_PAYMENT_TERM, debit_note_mstr.DNM_PAYMENT_DATE, " & _
                    '                "debit_note_mstr.DNM_DOWNLOADED_DATE,debit_note_mstr.DNM_CREATED_BY, debit_note_mstr.DNM_CREATED_DATE, debit_note_mstr.DNM_DN_TOTAL, " & _
                    '                "debit_note_details.DND_DN_S_COY_ID, debit_note_details.DND_DN_NO, debit_note_details.DND_DN_LINE, debit_note_details.DND_INV_LINE, " & _
                    '                "debit_note_details.DND_QTY, debit_note_details.DND_UNIT_COST,debit_note_details.DND_GST_RATE, debit_note_details.DND_GST_INPUT_TAX_CODE, " & _
                    '                "debit_note_details.DND_GST_OUTPUT_TAX_CODE, debit_note_details.DND_TICKETING_NO_INPUT, debit_note_details.DND_REMARKS, " & _
                    '                "INVOICE_MSTR.IM_INVOICE_INDEX, INVOICE_MSTR.IM_INVOICE_NO, " & _
                    '                "INVOICE_MSTR.IM_S_COY_ID, INVOICE_MSTR.IM_S_COY_NAME, INVOICE_MSTR.IM_PO_INDEX, INVOICE_MSTR.IM_B_COY_ID, INVOICE_MSTR.IM_PAYMENT_DATE, " & _
                    '                "INVOICE_MSTR.IM_REMARK, INVOICE_MSTR.IM_CREATED_BY, INVOICE_MSTR.IM_CREATED_ON, INVOICE_MSTR.IM_INVOICE_STATUS,INVOICE_MSTR.IM_PAYMENT_NO, " & _
                    '                "INVOICE_MSTR.IM_YOUR_REF, INVOICE_MSTR.IM_OUR_REF, INVOICE_MSTR.IM_INVOICE_PREFIX, INVOICE_MSTR.IM_SUBMITTEDBY_FO, " & _
                    '                "INVOICE_MSTR.IM_EXCHANGE_RATE, INVOICE_MSTR.IM_FINANCE_REMARKS, INVOICE_MSTR.IM_PRINTED, INVOICE_MSTR.IM_FOLDER, " & _
                    '                "INVOICE_MSTR.IM_FM_APPROVED_DATE, INVOICE_MSTR.IM_WITHHOLDING_TAX, INVOICE_MSTR.IM_DOWNLOADED_DATE, INVOICE_MSTR.IM_EXTERNAL_IND," & _
                    '                "INVOICE_MSTR.IM_REFERENCE_NO, INVOICE_MSTR.IM_INVOICE_TOTAL, INVOICE_MSTR.IM_GST_INVOICE,INVOICE_MSTR.IM_PAYMENT_TERM, " & _
                    '                "INVOICE_MSTR.IM_STATUS_CHANGED_BY, INVOICE_MSTR.IM_STATUS_CHANGED_ON, INVOICE_MSTR.IM_SHIP_AMT, " & _
                    '                "(SELECT   CODE_DESC " & _
                    '                "FROM      CODE_MSTR AS a " & _
                    '                "WHERE   (CODE_ABBR = cm.CM_STATE) AND (CODE_CATEGORY = 's') AND " & _
                    '                "(CODE_VALUE = cm.CM_COUNTRY)) AS SupplierAddrState, " & _
                    '                "(SELECT   CODE_DESC " & _
                    '                "FROM      CODE_MSTR AS a " & _
                    '                "WHERE   (CODE_ABBR =cm.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS SupplierAddrCtry, " & _
                    '                "(SELECT   CODE_DESC " & _
                    '                "FROM      CODE_MSTR AS a " & _
                    '                "WHERE   (CODE_ABBR =  debit_note_mstr.DNM_STATE) AND (CODE_CATEGORY = 's') " & _
                    '                "AND  (CODE_VALUE = debit_note_mstr.DNM_COUNTRY))  AS BillAddrState,  " & _
                    '                "(SELECT   CODE_DESC " & _
                    '                "FROM      CODE_MSTR AS a " & _
                    '                "WHERE   (CODE_ABBR = debit_note_mstr.DNM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS BillAddrCtry, " & _
                    '                "invoice_details.ID_PRODUCT_DESC,invoice_details.ID_UOM,(dnd_qty*dnd_unit_cost) AS Amount, " & _
                    '                "dnd_qty*dnd_unit_cost*tax_perc/100 AS GST, " & _
                    '                "(dnd_qty*dnd_unit_cost) + (dnd_qty*dnd_unit_cost*tax_perc/100) AS Total, " & _
                    '                "cm.cm_business_reg_no, cm.cm_phone, cm.cm_email, cm.cm_tax_reg_no, " & _
                    '                "cm.cm_coy_name AS BuyerCompanyName, cm.CM_ADDR_LINE1, cm.CM_ADDR_LINE2, cm.CM_ADDR_LINE3, cm.CM_POSTCODE, cm.CM_CITY, cm.CM_COUNTRY " & _
                    '                "FROM debit_note_mstr " & _
                    '                "INNER JOIN debit_note_details ON debit_note_mstr.dnm_dn_s_coy_id = debit_note_details.dnd_dn_s_coy_id AND debit_note_mstr.dnm_dn_no = debit_note_details.dnd_dn_no " & _
                    '                "INNER JOIN invoice_mstr ON debit_note_mstr.dnm_inv_no = invoice_mstr.im_invoice_no AND debit_note_mstr.dnm_dn_s_coy_id = invoice_mstr.im_s_coy_Id " & _
                    '                "AND debit_note_mstr.dnm_dn_b_coy_id = invoice_mstr.im_b_coy_Id " & _
                    '                "INNER JOIN invoice_details ON invoice_mstr.im_invoice_no = invoice_details.id_invoice_no AND invoice_mstr.im_s_coy_id = invoice_details.id_s_coy_id " & _
                    '                "AND debit_note_details.dnd_inv_line = invoice_details.id_invoice_line " & _
                    '                "INNER JOIN company_mstr cm ON invoice_mstr.im_s_coy_id = cm.cm_coy_id " & _
                    '                "LEFT JOIN tax ON tax.TAX_COUNTRY_CODE = cm.cm_country AND DND_GST_RATE =  TAX_CODE " & _
                    '                "WHERE (debit_note_mstr.dnm_dn_b_coy_id = @prmBCoyID) AND (debit_note_mstr.dnm_dn_s_coy_id = @prmSCoyID) " & _
                    '                "AND (debit_note_mstr.dnm_dn_no = @prmDNNo) "
                    .CommandText = "SELECT DNM_DN_INDEX, DNM_DN_B_COY_ID, DNM_DN_S_COY_ID, DNM_DN_NO, " &
                                    "DNM_DN_DATE, DNM_DN_TYPE, DNM_ADDR_LINE1, DNM_ADDR_LINE2, " &
                                    "DNM_ADDR_LINE3, DNM_POSTCODE, DNM_CITY,DNM_STATE, " &
                                    "DNM_COUNTRY, DNM_INV_NO, DNM_CURRENCY_CODE,  IF(DNM_EXCHANGE_RATE IS NULL OR DNM_EXCHANGE_RATE = 0,1,DNM_EXCHANGE_RATE) AS DNM_EXCHANGE_RATE, " &
                                    "DNM_REMARKS, DNM_DN_STATUS, DNM_STATUS_CHANGED_BY, DNM_STATUS_CHANGED_ON, " &
                                    "DNM_FM_APPROVED_DATE, DNM_PAYMENT_TERM, DNM_PAYMENT_DATE, " &
                                    "DNM_DOWNLOADED_DATE,DNM_CREATED_BY, DNM_CREATED_DATE, debit_note_mstr.DNM_DN_TOTAL, " &
                                    "DND_DN_S_COY_ID, DND_DN_NO, DND_DN_LINE, DND_INV_LINE, " &
                                    "DND_QTY, DND_UNIT_COST,DND_GST_RATE, DND_GST_INPUT_TAX_CODE, " &
                                    "DND_GST_OUTPUT_TAX_CODE, DND_TICKETING_NO_INPUT, DND_REMARKS, " &
                                    "IM_INVOICE_INDEX, IM_INVOICE_NO, " &
                                    "IM_S_COY_ID, IM_S_COY_NAME, IM_PO_INDEX, IM_B_COY_ID, IM_PAYMENT_DATE, " &
                                    "IM_REMARK, IM_CREATED_BY, IM_CREATED_ON, IM_INVOICE_STATUS,IM_PAYMENT_NO, " &
                                    "IM_YOUR_REF, IM_OUR_REF, IM_INVOICE_PREFIX, IM_SUBMITTEDBY_FO, " &
                                    "IM_EXCHANGE_RATE, IM_FINANCE_REMARKS, IM_PRINTED, IM_FOLDER, " &
                                    "IM_FM_APPROVED_DATE, IM_WITHHOLDING_TAX, IM_DOWNLOADED_DATE, IM_EXTERNAL_IND, " &
                                    "IM_REFERENCE_NO, IM_INVOICE_TOTAL, IM_GST_INVOICE,IM_PAYMENT_TERM, " &
                                    "IM_STATUS_CHANGED_BY, IM_STATUS_CHANGED_ON, IM_SHIP_AMT, " &
                                    "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = cm.CM_STATE) AND (CODE_CATEGORY = 's') " &
                                    "AND (CODE_VALUE = cm.CM_COUNTRY)) AS SupplierAddrState, " &
                                    "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR =cm.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS SupplierAddrCtry, " &
                                    "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR =  debit_note_mstr.DNM_STATE) AND (CODE_CATEGORY = 's') " &
                                    "AND (CODE_VALUE = debit_note_mstr.DNM_COUNTRY))  AS BillAddrState, " &
                                    "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = debit_note_mstr.DNM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS BillAddrCtry, " &
                                    "ID_PRODUCT_DESC, ID_UOM, (DND_QTY * DND_UNIT_COST) AS Amount, " &
                                    "(DND_QTY * DND_UNIT_COST) * IFNULL(ID_GST,0) /100 AS GST, " &
                                    "(DND_QTY * DND_UNIT_COST) + ((DND_QTY * DND_UNIT_COST) * IFNULL(ID_GST,0) /100) AS Total, " &
                                    "cm.cm_business_reg_no, cm.cm_phone, cm.cm_email, cm.cm_tax_reg_no, " &
                                    "cm.cm_coy_name, cm_b.cm_coy_name AS BuyerCompanyName, cm.CM_ADDR_LINE1, cm.CM_ADDR_LINE2, cm.CM_ADDR_LINE3, cm.CM_POSTCODE, cm.CM_CITY, cm.CM_COUNTRY " &
                                    "FROM debit_note_mstr " &
                                    "INNER JOIN debit_note_details ON debit_note_mstr.dnm_dn_s_coy_id = debit_note_details.dnd_dn_s_coy_id AND debit_note_mstr.dnm_dn_no = debit_note_details.dnd_dn_no " &
                                    "INNER JOIN invoice_mstr ON debit_note_mstr.dnm_inv_no = invoice_mstr.im_invoice_no AND debit_note_mstr.dnm_dn_s_coy_id = invoice_mstr.im_s_coy_Id " &
                                    "AND debit_note_mstr.dnm_dn_b_coy_id = invoice_mstr.im_b_coy_Id " &
                                    "INNER JOIN invoice_details ON invoice_mstr.im_invoice_no = invoice_details.id_invoice_no AND invoice_mstr.im_s_coy_id = invoice_details.id_s_coy_id " &
                                    "AND debit_note_details.dnd_inv_line = invoice_details.id_invoice_line " &
                                    "INNER JOIN company_mstr cm ON invoice_mstr.im_s_coy_id = cm.cm_coy_id " &
                                    "INNER JOIN company_mstr cm_b ON invoice_mstr.im_b_coy_id = cm_b.cm_coy_id " &
                                    "WHERE (debit_note_mstr.dnm_dn_b_coy_id = @prmBCoyID) AND (debit_note_mstr.dnm_dn_s_coy_id = @prmSCoyID) " &
                                    "AND (debit_note_mstr.dnm_dn_no = @prmDNNo)"
                End With

                da = New MySqlDataAdapter(cmd)
                da.SelectCommand.Parameters.Add(New MySqlParameter("@prmSCoyID", Request.QueryString("SCoyID")))
                da.SelectCommand.Parameters.Add(New MySqlParameter("@prmBCoyID", Request.QueryString("BCoyID")))
                da.SelectCommand.Parameters.Add(New MySqlParameter("@prmDNNo", Request.QueryString("DN_NO")))
                da.Fill(ds)

                Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewDebitNote_PreviewDebitNote", ds.Tables(0))
                Dim localreport As New LocalReport
                localreport.DataSources.Clear()
                localreport.DataSources.Add(rptDataSource)

                'Jules 2018.10.08 - SST
                'localreport.ReportPath = dispatcher.direct("Report", "PreviewDebitNote.rdlc", "Report") ' appPath & "Invoice\PreviewINVOICE-FTN.rdlc"
                Dim objSST As New GST
                Dim strDocType As String = "Service"
                Dim blnSST As Boolean = False

                blnSST = objSST.chkDocumentType(Request.QueryString("DN_NO"), "DN",, Request.QueryString("SCoyID"), Request.QueryString("BCoyID"),, strDocType)
                If blnSST Then
                    localreport.ReportPath = dispatcher.direct("Report", "PreviewDebitNote-" & strDocType & ".rdlc", "Report") ' appPath & "Invoice\PreviewINVOICE-FTN.rdlc"
                Else
                    localreport.ReportPath = dispatcher.direct("Report", "PreviewDebitNote.rdlc", "Report") ' appPath & "Invoice\PreviewINVOICE-FTN.rdlc"
                End If
                'End modification.

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

                        Case "prmno"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strNo)

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
                Dim deviceInfo As String =
                                "<DeviceInfo>" +
                                    "  <OutputFormat>EMF</OutputFormat>" +
                                    "</DeviceInfo>"
                Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
                Dim strFileName As String = "DN_" & strNo & "_" & Session("CompanyID") & "_" & Session("UserId") & ".pdf"
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

            Else ''For Debit Advice
                With cmd
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = "SELECT debit_note_mstr.DNM_DN_INDEX, debit_note_mstr.DNM_DN_B_COY_ID, debit_note_mstr.DNM_DN_S_COY_ID, debit_note_mstr.DNM_DN_NO, " &
                                    "debit_note_mstr.DNM_DN_DATE, debit_note_mstr.DNM_DN_TYPE, debit_note_mstr.DNM_ADDR_LINE1, debit_note_mstr.DNM_ADDR_LINE2, " &
                                    "debit_note_mstr.DNM_ADDR_LINE3, debit_note_mstr.DNM_POSTCODE, debit_note_mstr.DNM_CITY,debit_note_mstr.DNM_STATE, " &
                                    "debit_note_mstr.DNM_COUNTRY, debit_note_mstr.DNM_INV_NO, debit_note_mstr.DNM_CURRENCY_CODE, IF(debit_note_mstr.DNM_EXCHANGE_RATE IS NULL OR debit_note_mstr.DNM_EXCHANGE_RATE = 0,1,debit_note_mstr.DNM_EXCHANGE_RATE) AS DNM_EXCHANGE_RATE, " &
                                    "debit_note_mstr.DNM_REMARKS, debit_note_mstr.DNM_DN_STATUS, debit_note_mstr.DNM_STATUS_CHANGED_BY, debit_note_mstr.DNM_STATUS_CHANGED_ON, " &
                                    "debit_note_mstr.DNM_FM_APPROVED_DATE, debit_note_mstr.DNM_PAYMENT_TERM, debit_note_mstr.DNM_PAYMENT_DATE, " &
                                    "debit_note_mstr.DNM_DOWNLOADED_DATE,debit_note_mstr.DNM_CREATED_BY, debit_note_mstr.DNM_CREATED_DATE, debit_note_mstr.DNM_DN_TOTAL, " &
                                    "debit_note_details.DND_DN_S_COY_ID, debit_note_details.DND_DN_NO, debit_note_details.DND_DN_LINE, debit_note_details.DND_INV_LINE, " &
                                    "debit_note_details.DND_QTY, debit_note_details.DND_UNIT_COST,debit_note_details.DND_GST_RATE, debit_note_details.DND_GST_INPUT_TAX_CODE, " &
                                    "debit_note_details.DND_GST_OUTPUT_TAX_CODE, debit_note_details.DND_TICKETING_NO_INPUT, debit_note_details.DND_REMARKS, " &
                                    "INVOICE_MSTR.IM_INVOICE_INDEX, INVOICE_MSTR.IM_INVOICE_NO, " &
                                    "INVOICE_MSTR.IM_S_COY_ID, INVOICE_MSTR.IM_S_COY_NAME, INVOICE_MSTR.IM_PO_INDEX, INVOICE_MSTR.IM_B_COY_ID, INVOICE_MSTR.IM_PAYMENT_DATE, " &
                                    "INVOICE_MSTR.IM_REMARK, INVOICE_MSTR.IM_CREATED_BY, INVOICE_MSTR.IM_CREATED_ON, INVOICE_MSTR.IM_INVOICE_STATUS,INVOICE_MSTR.IM_PAYMENT_NO, " &
                                    "INVOICE_MSTR.IM_YOUR_REF, INVOICE_MSTR.IM_OUR_REF, INVOICE_MSTR.IM_INVOICE_PREFIX, INVOICE_MSTR.IM_SUBMITTEDBY_FO, " &
                                    "INVOICE_MSTR.IM_EXCHANGE_RATE, INVOICE_MSTR.IM_FINANCE_REMARKS, INVOICE_MSTR.IM_PRINTED, INVOICE_MSTR.IM_FOLDER, " &
                                    "INVOICE_MSTR.IM_FM_APPROVED_DATE, INVOICE_MSTR.IM_WITHHOLDING_TAX, INVOICE_MSTR.IM_DOWNLOADED_DATE, INVOICE_MSTR.IM_EXTERNAL_IND," &
                                    "INVOICE_MSTR.IM_REFERENCE_NO, INVOICE_MSTR.IM_INVOICE_TOTAL, INVOICE_MSTR.IM_GST_INVOICE,INVOICE_MSTR.IM_PAYMENT_TERM, " &
                                    "INVOICE_MSTR.IM_STATUS_CHANGED_BY, INVOICE_MSTR.IM_STATUS_CHANGED_ON, INVOICE_MSTR.IM_SHIP_AMT, " &
                                    "(SELECT   CODE_DESC " &
                                    "FROM      CODE_MSTR AS a " &
                                    "WHERE   (CODE_ABBR = cm.CM_STATE) AND (CODE_CATEGORY = 's') AND " &
                                    "(CODE_VALUE = cm.CM_COUNTRY)) AS SupplierAddrState, " &
                                    "(SELECT   CODE_DESC " &
                                    "FROM      CODE_MSTR AS a " &
                                    "WHERE   (CODE_ABBR =cm.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS SupplierAddrCtry, " &
                                    "(SELECT   CODE_DESC " &
                                    "FROM      CODE_MSTR AS a " &
                                    "WHERE   (CODE_ABBR =  debit_note_mstr.DNM_STATE) AND (CODE_CATEGORY = 's') " &
                                    "AND  (CODE_VALUE = debit_note_mstr.DNM_COUNTRY))  AS BillAddrState,  " &
                                    "(SELECT   CODE_DESC " &
                                    "FROM      CODE_MSTR AS a " &
                                    "WHERE   (CODE_ABBR = debit_note_mstr.DNM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS BillAddrCtry, " &
                                    "invoice_details.ID_PRODUCT_DESC,invoice_details.ID_UOM,(dnd_qty*dnd_unit_cost) AS Amount, " &
                                    "cm.cm_business_reg_no, cm.cm_phone, cm.cm_email, cm.cm_tax_reg_no, " &
                                    "cm.cm_coy_name, cm_b.cm_coy_name AS BuyerCompanyName, cm.CM_ADDR_LINE1, cm.CM_ADDR_LINE2, cm.CM_ADDR_LINE3, cm.CM_POSTCODE, cm.CM_CITY, cm.CM_COUNTRY, '' AS LineDesc " &
                                    "FROM debit_note_mstr " &
                                    "INNER JOIN debit_note_details ON debit_note_mstr.dnm_dn_s_coy_id = debit_note_details.dnd_dn_s_coy_id AND debit_note_mstr.dnm_dn_no = debit_note_details.dnd_dn_no " &
                                    "INNER JOIN invoice_mstr ON debit_note_mstr.dnm_inv_no = invoice_mstr.im_invoice_no AND debit_note_mstr.dnm_dn_s_coy_id = invoice_mstr.im_s_coy_Id " &
                                    "AND debit_note_mstr.dnm_dn_b_coy_id = invoice_mstr.im_b_coy_Id " &
                                    "INNER JOIN invoice_details ON invoice_mstr.im_invoice_no = invoice_details.id_invoice_no AND invoice_mstr.im_s_coy_id = invoice_details.id_s_coy_id " &
                                    "AND debit_note_details.dnd_inv_line = invoice_details.id_invoice_line " &
                                    "INNER JOIN company_mstr cm ON invoice_mstr.im_s_coy_id = cm.cm_coy_id " &
                                    "INNER JOIN company_mstr cm_b ON invoice_mstr.im_b_coy_id = cm_b.cm_coy_id " &
                                    "WHERE (debit_note_mstr.dnm_dn_b_coy_id = @prmBCoyID) AND (debit_note_mstr.dnm_dn_s_coy_id = @prmSCoyID) " &
                                    "AND (debit_note_mstr.dnm_dn_no = @prmDNNo) "

                End With

                da = New MySqlDataAdapter(cmd)
                da.SelectCommand.Parameters.Add(New MySqlParameter("@prmSCoyID", Request.QueryString("SCoyID")))
                da.SelectCommand.Parameters.Add(New MySqlParameter("@prmBCoyID", Request.QueryString("BCoyID")))
                da.SelectCommand.Parameters.Add(New MySqlParameter("@prmDNNo", Request.QueryString("DN_NO")))
                da.Fill(ds)

                Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewDebitNote_PreviewDebitAdvice", ds.Tables(0))
                Dim localreport As New LocalReport
                localreport.DataSources.Clear()
                localreport.DataSources.Add(rptDataSource)
                localreport.ReportPath = dispatcher.direct("Report", "PreviewDebitAdvice.rdlc", "Report")
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

                        Case "prmno"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strNo)

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
                Dim strFileName As String = "DA_" & strNo & "_" & Session("CompanyID") & "_" & Session("UserId") & ".pdf"
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
            End If

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