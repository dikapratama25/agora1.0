Imports AgoraLegacy
Imports eProcure.Component
Imports MySql.Data.MySqlClient

Public Class ReportFormat2
    Inherits AgoraLegacy.AppBaseClass
    Public Const MsgInvalidDate As String = "Start Date Must Be Earlier Than Or Equal To End Date"

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents txtSDate As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtEndDate As System.Web.UI.WebControls.TextBox
    Dim strType As String
    'Protected WithEvents ValSDate As System.Web.UI.WebControls.RequiredFieldValidator
    'Protected WithEvents ValEDate As System.Web.UI.WebControls.RequiredFieldValidator
    'Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    'Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
    'Protected WithEvents Validationsummary1 As System.Web.UI.WebControls.ValidationSummary
    'Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    'Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    'Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    'Protected WithEvents label10 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    'Protected WithEvents lblDay As System.Web.UI.WebControls.Label
    'Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    'Protected WithEvents txtDay As System.Web.UI.WebControls.TextBox
    'Protected WithEvents ValDay As System.Web.UI.WebControls.RequiredFieldValidator
    'Protected WithEvents tr1 As System.Web.UI.HtmlControls.HtmlTableRow

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        strType = Me.Request.QueryString("type")

        ValDay.Enabled = False

        Select Case strType
            Case "OPO"
                lblHeader.Text = "Outstanding Purchase Order From Vendor"
            Case "OPR"
                lblHeader.Text = "Outstanding Purchase Requisition By Stock Item"
            Case "OGRN"
                lblHeader.Text = "Outstanding Goods Receipt Note From Vendors (Base On DO Date)"
            Case "DRP"
                lblHeader.Text = "Detailed Report On Purchase-To-Invoice Cycle"
            Case "OPOV"
                lblHeader.Text = "Outstanding Purchase Order By Vendor"
            Case "PME"
                lblHeader.Text = "Pme Expenses Report"
            Case "PMED"
                lblHeader.Text = "Pme Expenses Detail Report By Section - S.E.H"
            Case "CRFV"
                lblHeader.Text = "Consumption Report From Vendor"
            Case "InvSumm"
                lblHeader.Text = "Invoice Summary Report For Vendor (Transaction)"
            Case "OPOD"
                lblHeader.Text = "Outstanding Purchase Order By Department"
            Case "ODO"
                lblHeader.Text = "Outstanding Delivery Order By Department"
            Case "PO"
                lblHeader.Text = "Purchase Orders Raised Report"
            Case "ODOGRN"
                lblHeader.Text = "Delivery Orders Vs. GRN Listing By Department"
                lblDay.Text = "DO to GRN Days"
                ValDay.Enabled = True
                tr1.Style.Item("Display") = ""
            Case "CRBC"
                lblHeader.Text = "Consumption Report By Category"
            Case "PTIC"
                lblHeader.Text = "Detailed Report On Purchase-To-Invoice Cycle By Category (without RPO)"
                'Michelle (17/8/2007) - Add in HLB Reports
            Case "INV20TO100"
                lblHeader.Text = "Invoice Approval For Amount > RM 20,000 Up To RM 100,000"
            Case "INV100TO500"
                lblHeader.Text = "Invoice Approval For Amount > RM 100,000 Up To RM 500,000"
                'Michelle (22/8/2007) - Add back the missing link
            Case "POBYPSITE"
                lblHeader.Text = "Details Of PO By Purchase Site Report"
            Case "GRNBYPSITE"
                lblHeader.Text = "Outstanding Goods Receipt Note By Purchase Site Report"
            Case "OSINVBYDEPT"
                lblHeader.Text = "Outstanding Invoice By Department Report"
            Case "APPRINVS"
                lblHeader.Text = "Summary Approved Invoices Report Sorted By Vendor"
            Case "R5"
                lblHeader.Text = "R5 Summary Approved And Paid Invoices Report Sorted By Item For Branch"
            Case "R6"
                lblHeader.Text = "R6  Summary Approved And Paid Invoices Report Sorted By Item For Head Office"
            Case "PTIFM"
                lblHeader.Text = "Detailed Report On Purchase-To-Invoice Cycle By Category For HQ (without RPO)"
            Case "PTIBR"
                lblHeader.Text = "Detailed Report On Purchase-To-Invoice Cycle By Category For Branch (without RPO)"
            Case "PTGOEFF"
                lblHeader.Text = "Detailed Report On Purchase-To-Good Receipt Note Cycle for OE & FF (without RPO)"
            Case "R11"
                lblHeader.Text = "R11 Detail Outstanding Goods Receipt Note Report Sorted By Branch For Branch"
            Case "R12"
                lblHeader.Text = "R12 Detail Outstanding Goods Receipt Note Report Sorted By Branch For Head Office"
            Case "VRFR"
                lblHeader.Text = "Vendor Revenue Forecast Report"
                'Michelle (5/5/2009) - CR0051
            Case "DBN"
                lblHeader.Text = "Debit Note"

                'Jules 2016.01.28 - FITR enhancement
            Case "R6_2"
                lblHeader.Text = "R6_2 Summary Approved And Paid Invoices Report Sorted By Item For Branch and Head Office"

                'Jules 2016.03.01 - FITR enhancement 
            Case "R12_2"
                lblHeader.Text = "R12_2 Detail Outstanding Goods Receipt Note Report Sorted By Branch For Branch and Head Office"
        End Select

        'Chee hong (07/02/2014) - Included options for capex & opex
        If strType = "APPRINVS" Then
            tr2.Style("display") = ""
        Else
            tr2.Style("display") = "none"
        End If

        lnkBack.NavigateUrl = "ReportSelection.aspx?pageid=" & strPageId
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim strReportName As String = ""
        Dim reportURL As String
        'Dim crParam As New Hashtable
        Dim strParam As String = ""
        Dim startdate As String = Format(CDate(txtSDate.Text), "MM/dd/yyyy")
        Dim enddate As String = Format(CDate(txtEndDate.Text), "MM/dd/yyyy")

        'crParam.Add("userIDParam", Session("UserID"))
        'crParam.Add("dateFromParam", startdate)
        'crParam.Add("dateToParam", enddate)

        'Select Case strType
        '    Case "OPO"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "outstandPOFrSuppl2.rpt"
        '    Case "OPR"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "outstandPRByItem2.rpt"
        '    Case "OGRN"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        'Michelle (24/8/2007) - Upon Lee Ling's request email dated 24/8/2007
        '        Dim strMultiPO As String = ""
        '        Dim objDb As New EAD.DBCom
        '        Dim strSql As String = "SELECT CM_MULTI_PO FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Session("CompanyID") & "'"
        '        Dim tDS As DataSet = objDb.FillDs(strSql)

        '        If tDS.Tables(0).Rows.Count > 0 Then
        '            strMultiPO = Common.parseNull(tDS.Tables(0).Rows(0).Item("CM_MULTI_PO"))
        '        End If
        '        If strMultiPO = "Y" Then
        '            strReportName = "outstandGRNFrSupplHLB.rpt"
        '        Else
        '            strReportName = "outstandGRNFrSuppl2.rpt"
        '        End If
        '    Case "DRP"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "PurchaseToInvoiceCycleRpt2.rpt"
        '    Case "OPOV"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "outstandPOFrSupplSeh2.rpt"
        '    Case "PME"
        '        crParam.Add("companyIDParam", Session("CompanyID"))
        '        strReportName = "pmeExpenses2.rpt"
        '    Case "PMED"
        '        crParam.Add("companyIDParam", Session("CompanyID"))
        '        strReportName = "pmeExpensesDetail2.rpt"
        '    Case "CRFV"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "consumptionFromVendor.rpt"
        '    Case "InvSumm"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "InvoiceSummaryBySupplier.rpt"
        '    Case "OPOD"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "outstandingPoByDept.rpt"
        '    Case "ODO"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "outstandDOByDept.rpt"
        '    Case "PO"
        '        crParam.Add("BCOYID", Session("CompanyID"))
        '        strReportName = "PORaised.rpt"
        '    Case "ODOGRN"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        crParam.Add("doVsGrnDayParam", txtDay.Text)
        '        strReportName = "DOvsGRNByDept.rpt"
        '    Case "CRBC"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "consumptionByCategory.rpt"
        '    Case "PTIC"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "PurchaseToInvoiceCycleByCategory.rpt"
        '    Case "INV20TO100"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "invoiceApproval20KTo100K.rpt"
        '    Case "INV100TO500"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "invoiceApproval100KTo500K.rpt"
        '    Case "POBYPSITE"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "detailOfPOByPurchaseSite.rpt"
        '    Case "GRNBYPSITE"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "outstandGRNByPurchaseSite.rpt"
        '    Case "OSINVBYDEPT"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "outstandInvoiceByDept.rpt"
        '    Case "APPRINVS"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "SummaryApprovedInvoicesByVendor.rpt"
        '    Case "R5"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "R5SummaryApprovedPaidInvoicesSortedByItem-BR.rpt"
        '    Case "R6"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "R6SummaryApprovedPaidInvoicesSortedByItem-HQ.rpt"
        '    Case "PTIFM"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "PurchaseToInvoiceCyclePrintStatSecurity-HQ.rpt"
        '    Case "PTIBR"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "PurchaseToInvoiceCyclePrintStatSecurity-BR.rpt"
        '    Case "PTGOEFF"
        '        crParam.Add("BCOYID", Session("CompanyID"))
        '        strReportName = "PurchaseToGRNCycleOEFF.rpt"
        '    Case "R11"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "R11DetailOutstandGRNReprotSortedByBranch-BR.rpt"
        '    Case "R12"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "R12DetailOutstandGRNReprotSortedByBranch-HQ.rpt"
        '    Case "VRFR"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "vendorRevenueForecast.rpt"
        '    Case "DBN"
        '        crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        '        strReportName = "DebitNote.rpt"
        'End Select
        Select Case strType
            Case "OPO"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "outstandPOFrSuppl2.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "OPR"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "outstandPRByItem2.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "OGRN"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                'Michelle (24/8/2007) - Upon Lee Ling's request email dated 24/8/2007
                Dim strSql, strMultiPO

                Dim objDb As New EAD.DBCom
                strSql = "SELECT CM_MULTI_PO FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Session("CompanyID") & "'"
                Dim tDS As DataSet = objDb.FillDs(strSql)
                If tDS.Tables(0).Rows.Count > 0 Then
                    strMultiPO = Common.parseNull(tDS.Tables(0).Rows(0).Item("CM_MULTI_PO"))
                End If
                'If drPR.Read Then
                '    strMultiPO = Common.parseNull(drPR("CM_MULTI_PO"))
                'End If 
                If strMultiPO = "Y" Then
                    strReportName = "outstandGRNFrSupplHLB.rpt"
                Else
                    strReportName = "outstandGRNFrSuppl2.rpt"
                End If
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "DRP"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "PurchaseToInvoiceCycleRpt2.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&BuyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "OPOV"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "outstandPOFrSupplSeh2.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "PME"
                'crParam.Add("companyIDParam", Session("CompanyID"))
                strReportName = "pmeExpenses2.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&companyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "PMED"
                'crParam.Add("companyIDParam", Session("CompanyID"))
                strReportName = "pmeExpensesDetail2.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&companyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "CRFV"
                'Jules 2016.01.11 - EPROC FITR.
                Dim objDbcom As New EAD.DBCom
                Dim strTaxCodes As String = ""
                Dim decRecoverablePercentage As Decimal = 0
                'mimi - 17/01/11 - Fitr enhancement
                Dim strRecoverablePercentage As String
                Dim strIRRecoverablePercentage As String

                Dim strTaxSQL As String = "SELECT FTC_TAX_CODE FROM fitr_tax_code WHERE FTC_MODULE='eProcure' "
                Dim ds As DataSet = objDbcom.FillDs(strTaxSQL)
                If ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                        If strTaxCodes = "" Then
                            strTaxCodes = ds.Tables(0).Rows(i)("FTC_TAX_CODE")
                        Else
                            strTaxCodes &= "," & ds.Tables(0).Rows(i)("FTC_TAX_CODE")
                        End If
                    Next
                End If

                'mimi 09/01/2017 - fitr_mstr enhancement
                decRecoverablePercentage = objDbcom.GetVal("SELECT fm_fitr_recoverable FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                'mimi - 17/01/11 - Fitr enhancement
                strRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_recoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                strIRRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_irrecoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")

                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "consumptionFromVendor.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate & "&taxCodes=" & strTaxCodes & "&recPerc=" & decRecoverablePercentage & "&RecoverablePercentage=" & strRecoverablePercentage & "&IRRecoverablePercentage=" & strIRRecoverablePercentage
                'end - mimi
                'End. 
            Case "InvSumm"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "InvoiceSummaryBySupplier.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "OPOD"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "outstandingPoByDept.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "ODO"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "outstandDOByDept.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "PO"
                'crParam.Add("BCOYID", Session("CompanyID"))
                strReportName = "PORaised.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&BCOYID=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "ODOGRN"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                'crParam.Add("doVsGrnDayParam", txtDay.Text)
                strReportName = "DOvsGRNByDept.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate & "&doVsGrnDayParam=" & txtDay.Text

            Case "CRBC"
                'Jules 2016.01.11 - EPROC FITR.
                Dim objDbcom As New EAD.DBCom
                Dim strTaxCodes As String = ""
                Dim decRecoverablePercentage As Decimal = 0
                'mimi - 17/01/11 - Fitr enhancement
                Dim strRecoverablePercentage As String
                Dim strIRRecoverablePercentage As String

                Dim strTaxSQL As String = "SELECT FTC_TAX_CODE FROM fitr_tax_code WHERE FTC_MODULE='eProcure' "
                Dim ds As DataSet = objDbcom.FillDs(strTaxSQL)
                If ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                        If strTaxCodes = "" Then
                            strTaxCodes = ds.Tables(0).Rows(i)("FTC_TAX_CODE")
                        Else
                            strTaxCodes &= "," & ds.Tables(0).Rows(i)("FTC_TAX_CODE")
                        End If
                    Next
                End If

                'mimi 09/01/2017 - fitr_mstr enhancement
                decRecoverablePercentage = objDbcom.GetVal("SELECT fm_fitr_recoverable FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")

                'mimi - 17/01/11 - Fitr enhancement
                strRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_recoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                strIRRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_irrecoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")

                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "consumptionByCategory.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate & "&taxCodes=" & strTaxCodes & "&recPerc=" & decRecoverablePercentage & "&RecoverablePercentage=" & strRecoverablePercentage & "&IRRecoverablePercentage=" & strIRRecoverablePercentage
                'end - mimi
                'End. 
            Case "PTIC"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "PurchaseToInvoiceCycleByCategory.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "INV20TO100"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "invoiceApproval20KTo100K.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "INV100TO500"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "invoiceApproval100KTo500K.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "POBYPSITE"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "detailOfPOByPurchaseSite.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "GRNBYPSITE"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "outstandGRNByPurchaseSite.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "OSINVBYDEPT"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "outstandInvoiceByDept.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "APPRINVS"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                If Me.dtRadioBtn.SelectedValue = "opex" Then
                    strReportName = "SummaryApprovedInvoicesByVendor-Opex.rpt"
                ElseIf Me.dtRadioBtn.SelectedValue = "capex" Then
                    strReportName = "SummaryApprovedInvoicesByVendor-Capex.rpt"
                Else
                    strReportName = "SummaryApprovedInvoicesByVendor.rpt"
                End If
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "R5"
                'Jules 2016.01.11 - EPROC FITR.
                Dim objDbcom As New EAD.DBCom
                Dim strTaxCodes As String = ""
                Dim decRecoverablePercentage As Decimal = 0
                'mimi - 17/01/10 - Fitr enhancement
                Dim strRecoverablePercentage As String
                Dim strIRRecoverablePercentage As String
                'end-mimi
                Dim strTaxSQL As String = "SELECT FTC_TAX_CODE FROM fitr_tax_code WHERE FTC_MODULE='eProcure' "
                Dim ds As DataSet = objDbcom.FillDs(strTaxSQL)
                If ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                        If strTaxCodes = "" Then
                            strTaxCodes = ds.Tables(0).Rows(i)("FTC_TAX_CODE")
                        Else
                            strTaxCodes &= "," & ds.Tables(0).Rows(i)("FTC_TAX_CODE")
                        End If
                    Next
                End If

                'mimi 09/01/2017 - fitr_mstr enhancement
                decRecoverablePercentage = objDbcom.GetVal("SELECT fm_fitr_recoverable FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")

                'mimi - 17/01/10 - Fitr enhancement
                strRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_recoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                strIRRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_irrecoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")

                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "R5SummaryApprovedPaidInvoicesSortedByItem-BR.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate & "&taxCodes=" & strTaxCodes & "&recPerc=" & decRecoverablePercentage & "&RecoverablePercentage=" & strRecoverablePercentage & "&IRRecoverablePercentage=" & strIRRecoverablePercentage
                'end - mimi
                'End.
            Case "R6"
                'Jules 2016.01.11 - EPROC FITR.
                Dim objDbcom As New EAD.DBCom
                Dim strTaxCodes As String = ""
                Dim decRecoverablePercentage As Decimal = 0
                'mimi - 17/01/10 - Fitr enhancement
                Dim strRecoverablePercentage As String
                Dim strIRRecoverablePercentage As String
                Dim strTaxSQL As String = "SELECT FTC_TAX_CODE FROM fitr_tax_code WHERE FTC_MODULE='eProcure' "
                Dim ds As DataSet = objDbcom.FillDs(strTaxSQL)
                If ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                        If strTaxCodes = "" Then
                            strTaxCodes = ds.Tables(0).Rows(i)("FTC_TAX_CODE")
                        Else
                            strTaxCodes &= "," & ds.Tables(0).Rows(i)("FTC_TAX_CODE")
                        End If
                    Next
                End If

                'mimi 09/01/2017 - fitr_mstr enhancement
                decRecoverablePercentage = objDbcom.GetVal("SELECT fm_fitr_recoverable FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                'mimi - 17/01/10 - Fitr enhancement
                strRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_recoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                strIRRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_irrecoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")

                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "R6SummaryApprovedPaidInvoicesSortedByItem-HQ.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate & "&taxCodes=" & strTaxCodes & "&recPerc=" & decRecoverablePercentage & "&RecoverablePercentage=" & strRecoverablePercentage & "&IRRecoverablePercentage=" & strIRRecoverablePercentage
                'end - mimi
                'End.
            Case "PTIFM"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "PurchaseToInvoiceCyclePrintStatSecurity-HQ.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "PTIBR"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "PurchaseToInvoiceCyclePrintStatSecurity-BR.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "PTGOEFF"
                'crParam.Add("BCOYID", Session("CompanyID"))
                strReportName = "PurchaseToGRNCycleOEFF.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&BCOYID=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "R11"
                'Jules 2016.01.11 - EPROC FITR.
                Dim objDbcom As New EAD.DBCom
                Dim strTaxCodes As String = ""
                Dim decRecoverablePercentage As Decimal = 0
                Dim strTaxSQL As String = "SELECT FTC_TAX_CODE FROM fitr_tax_code WHERE FTC_MODULE='eProcure' "
                Dim ds As DataSet = objDbcom.FillDs(strTaxSQL)
                If ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                        If strTaxCodes = "" Then
                            strTaxCodes = ds.Tables(0).Rows(i)("FTC_TAX_CODE")
                        Else
                            strTaxCodes &= "," & ds.Tables(0).Rows(i)("FTC_TAX_CODE")
                        End If
                    Next
                End If

                decRecoverablePercentage = objDbcom.GetVal("SELECT fm_fitr_recoverable FROM fitr_mstr WHERE fm_deleted='N'")
                'End EPROC FITR.  
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "R11DetailOutstandGRNReprotSortedByBranch-BR.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

            Case "R12"
                'Jules 2016.01.11 - EPROC FITR.
                'Dim objDbcom As New EAD.DBCom
                'Dim strTaxCodes As String = ""
                'Dim decRecoverablePercentage As Decimal = 0
                'Dim strTaxSQL As String = "SELECT FTC_TAX_CODE FROM fitr_tax_code WHERE FTC_MODULE='eProcure' "
                'Dim ds As DataSet = objDbcom.FillDs(strTaxSQL)
                'If ds.Tables(0).Rows.Count > 0 Then
                '    For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                '        If strTaxCodes = "" Then
                '            strTaxCodes = ds.Tables(0).Rows(i)("FTC_TAX_CODE")
                '        Else
                '            strTaxCodes &= "," & ds.Tables(0).Rows(i)("FTC_TAX_CODE")
                '        End If
                '    Next
                'End If

                'decRecoverablePercentage = objDbcom.GetVal("SELECT fm_fitr_recoverable FROM fitr_mstr WHERE fm_coy_id='" & Session("CompanyID") & "' AND fm_deleted='N'")
                'End EPROC FITR.  

                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "R12DetailOutstandGRNReprotSortedByBranch-HQ.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

                'Case "VRFR"
                '    'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                '    strReportName = "vendorRevenueForecast.rpt"
            Case "R12_2"
                'Jules 2016.03.01 - EPROC FITR.               
                strReportName = "R12_2DetailOutstandGRNReprotSortedByBranch.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate
                'End EPROC FITR. 

            Case "DBN"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "DebitNote.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate

                'Jules 2016.01.28 - FITR enhancement
            Case "R6_2"
                Dim objDbcom As New EAD.DBCom
                Dim strTaxCodes As String = ""
                Dim decRecoverablePercentage As Decimal = 0
                'mimi - 17/01/10 - Fitr enhancement
                Dim strRecoverablePercentage As String
                Dim strIRRecoverablePercentage As String
                Dim strTaxSQL As String = "SELECT FTC_TAX_CODE FROM fitr_tax_code WHERE FTC_MODULE='eProcure' "
                Dim ds As DataSet = objDbcom.FillDs(strTaxSQL)
                If ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                        If strTaxCodes = "" Then
                            strTaxCodes = ds.Tables(0).Rows(i)("FTC_TAX_CODE")
                        Else
                            strTaxCodes &= "," & ds.Tables(0).Rows(i)("FTC_TAX_CODE")
                        End If
                    Next
                End If

                'mimi 09/01/2017 - fitr_mstr enhancement
                decRecoverablePercentage = objDbcom.GetVal("SELECT fm_fitr_recoverable FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                'mimi - 17/01/10 - Fitr enhancement
                strRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_recoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                strIRRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_irrecoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")

                strReportName = "R6_2SummaryApprovedPaidInvoicesSortedByItem.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & enddate & "&taxCodes=" & strTaxCodes & "&recPerc=" & decRecoverablePercentage & "&RecoverablePercentage=" & strRecoverablePercentage & "&IRRecoverablePercentage=" & strIRRecoverablePercentage
                'end - mimi
                'End.
        End Select
        'Session("Param") = crParam

        reportURL = "../Report/ReportViewer.aspx?rpt=" & strReportName & strParam & "&rptType=" & cboReportType.SelectedIndex

        Dim jscript As String = String.Empty
        jscript &= "<script language=""Javascript"">" & vbCrLf
        jscript &= "x = screen.width -200;" & vbCrLf
        jscript &= "y = screen.height - 200;" & vbCrLf
        jscript &= "var props = 'scrollBars=yes, resizable=yes, toolbar=no, menubar=no, location=no, directories=no, top=0, left=0, width=' + x + ', height=' + y ;" & vbCrLf
        jscript &= "window.location = """ & reportURL & """;" & vbCrLf
        jscript &= "//-->" & vbCrLf
        jscript &= "</script>" & vbCrLf
        Dim rndKey As New Random
        Me.ClientScript.RegisterStartupScript(GetType(ReportFormat2), rndKey.Next.ToString, jscript)
    End Sub

    'Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
    '    Dim crParam As New Hashtable
    '    crParam.Add("userIDParam", Session("UserID"))

    '    Dim startdate As String = Format(CDate(txtSDate.Text), "MM/dd/yyyy")
    '    Dim enddate As String = Format(CDate(txtEndDate.Text), "MM/dd/yyyy")
    '    crParam.Add("dateFromParam", startdate)
    '    crParam.Add("dateToParam", enddate)

    '    Select Case strType
    '        Case "OPO"
    '            crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "outstandPOFrSuppl2.rpt", crParam)
    '        Case "OPR"
    '            crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "outstandPRByItem2.rpt", crParam)
    '        Case "OGRN"
    '            crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '            'Michelle (24/8/2007) - Upon Lee Ling's request email dated 24/8/2007
    '            Dim strSql, strMultiPO
    '            Dim objDb As New EAD.DBCom
    '            strSql = "SELECT CM_MULTI_PO FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Session("CompanyID") & "'"
    '            Dim tDS As DataSet = objDb.FillDs(strSql)

    '            If tDS.Tables(0).Rows.Count > 0 Then
    '                strMultiPO = Common.parseNull(tDS.Tables(0).Rows(0).Item("CM_MULTI_PO"))
    '            End If
    '            If strMultiPO = "Y" Then
    '                eProcure.Component.ReportViewer.popCrystalReport(Me, "outstandGRNFrSupplHLB.rpt", crParam)
    '            Else
    '                eProcure.Component.ReportViewer.popCrystalReport(Me, "outstandGRNFrSuppl2.rpt", crParam)
    '            End If
    '        Case "DRP"
    '                crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '                ReportViewer.popCrystalReport(Me, "PurchaseToInvoiceCycleRpt2.rpt", crParam)
    '        Case "OPOV"
    '                crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '                ReportViewer.popCrystalReport(Me, "outstandPOFrSupplSeh2.rpt", crParam)
    '        Case "PME"
    '                crParam.Add("companyIDParam", Session("CompanyID"))
    '                ReportViewer.popCrystalReport(Me, "pmeExpenses2.rpt", crParam)
    '        Case "PMED"
    '                crParam.Add("companyIDParam", Session("CompanyID"))
    '                ReportViewer.popCrystalReport(Me, "pmeExpensesDetail2.rpt", crParam)
    '        Case "CRFV"
    '                crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '                ReportViewer.popCrystalReport(Me, "consumptionFromVendor.rpt", crParam)
    '        Case "InvSumm"
    '                crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '                ReportViewer.popCrystalReport(Me, "InvoiceSummaryBySupplier.rpt", crParam)
    '        Case "OPOD"
    '                crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '                ReportViewer.popCrystalReport(Me, "outstandingPoByDept.rpt", crParam)
    '        Case "ODO"
    '                crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '                ReportViewer.popCrystalReport(Me, "outstandDOByDept.rpt", crParam)
    '        Case "PO"
    '                crParam.Add("BCOYID", Session("CompanyID"))
    '                ReportViewer.popCrystalReport(Me, "PORaised.rpt", crParam)
    '        Case "ODOGRN"
    '                crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '                crParam.Add("doVsGrnDayParam", txtDay.Text)
    '                ReportViewer.popCrystalReport(Me, "DOvsGRNByDept.rpt", crParam)
    '        Case "CRBC"
    '                crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '                ReportViewer.popCrystalReport(Me, "consumptionByCategory.rpt", crParam)
    '        Case "PTIC"
    '                crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '                ReportViewer.popCrystalReport(Me, "PurchaseToInvoiceCycleByCategory.rpt", crParam)
    '        Case "INV20TO100"
    '                crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '                ReportViewer.popCrystalReport(Me, "invoiceApproval20KTo100K.rpt", crParam)
    '        Case "INV100TO500"
    '                crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '                ReportViewer.popCrystalReport(Me, "invoiceApproval100KTo500K.rpt", crParam)
    '        Case "POBYPSITE"
    '                crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '                ReportViewer.popCrystalReport(Me, "detailOfPOByPurchaseSite.rpt", crParam)
    '        Case "GRNBYPSITE"
    '                crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '                ReportViewer.popCrystalReport(Me, "outstandGRNByPurchaseSite.rpt", crParam)
    '        Case "OSINVBYDEPT"
    '                crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '                ReportViewer.popCrystalReport(Me, "outstandInvoiceByDept.rpt", crParam)
    '        Case "APPRINVS"
    '            crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "SummaryApprovedInvoicesByVendor.rpt", crParam)
    '        Case "R5"
    '            crParam.Add("buyercompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "R5SummaryApprovedPaidInvoicesSortedByItem-BR.rpt", crParam)
    '        Case "R6"
    '            crParam.Add("buyercompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "R6SummaryApprovedPaidInvoicesSortedByItem-HQ.rpt", crParam)
    '        Case "PTIFM"
    '            crParam.Add("buyercompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "PurchaseToInvoiceCyclePrintStatSecurity-HQ.rpt", crParam)
    '        Case "PTIBR"
    '            crParam.Add("buyercompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "PurchaseToInvoiceCyclePrintStatSecurity-BR.rpt", crParam)
    '        Case "PTGOEFF"
    '            crParam.Add("BCOYID", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "PurchaseToGRNCycleOEFF.rpt", crParam)
    '        Case "R11"
    '            crParam.Add("buyercompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "R11DetailOutstandGRNReprotSortedByBranch-BR.rpt", crParam)
    '        Case "R12"
    '            crParam.Add("buyercompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "R12DetailOutstandGRNReprotSortedByBranch-HQ.rpt", crParam)
    '        Case "VRFR"
    '            crParam.Add("buyercompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "vendorRevenueForecast.rpt", crParam)
    '        Case "DBN"
    '            crParam.Add("buyercompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "DebitNote.rpt", crParam)
    '    End Select
    'End Sub
End Class

