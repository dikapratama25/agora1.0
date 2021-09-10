Imports AppCommon
Imports eProcure.Component
Public Class ReportFormat2
    Inherits AppCommon.AppBaseClass
    Public Const MsgInvalidDate As String = "Start Date Must Be Earlier Than Or Equal To End Date"
    Dim dispatcher As New Dispatcher.dispatcher
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtSDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEndDate As System.Web.UI.WebControls.TextBox
    Dim strType As String
    Protected WithEvents ValSDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValEDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents Validationsummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents label10 As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblDay As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents txtDay As System.Web.UI.WebControls.TextBox
    Protected WithEvents ValDay As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents tr1 As System.Web.UI.HtmlControls.HtmlTableRow

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
        'Put user code to initialize the page here
        strType = Me.Request.QueryString("type")
        ValDay.Enabled = False

        Select Case strType
            Case "OPO"
                'lblHeader.Text = "Outstanding Purchase Order"
                lblHeader.Text = "Outstanding Purchase Order From Vendor"

            Case "OPR"
                lblHeader.Text = "Outstanding Purchase Requisition By Stock Item"

            Case "OGRN"
                'lblHeader.Text = "Outstanding Good Receipt Note From Vendors"
                lblHeader.Text = "Outstanding Good Receipt Note From Vendors (Base On DO Date)"

            Case "DRP"
                lblHeader.Text = "Detailed Report On Purchase-To-Invoice Cycle"

            Case "OPOV"
                'lblHeader.Text = "Outstanding Purchase Order By Vendor - S.E.H"
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
        End Select
        lnkBack.NavigateUrl = Dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId)

    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim crParam As New Hashtable
        crParam.Add("userIDParam", Session("UserID"))

        Dim startdate As String = Format(CDate(txtSDate.Text), "MM/dd/yyyy")
        Dim enddate As String = Format(CDate(txtEndDate.Text), "MM/dd/yyyy")
        crParam.Add("dateFromParam", startdate)
        crParam.Add("dateToParam", enddate)

        Select Case strType
            Case "OPO"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                eProcure.Component.ReportViewer.popCrystalReport(Me, "outstandPOFrSuppl2.rpt", crParam)
            Case "OPR"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                eProcure.Component.ReportViewer.popCrystalReport(Me, "outstandPRByItem2.rpt", crParam)
            Case "OGRN"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                'Michelle (24/8/2007) - Upon Lee Ling's request email dated 24/8/2007
                Dim strSql, strMultiPO

                Dim objDb As New DBAccess.EAD.DBCom
                strSql = "SELECT CM_MULTI_PO FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Session("CompanyIdToken") & "'"
                Dim tDS As DataSet = objDb.FillDs(strSql)
                If tDS.Tables(0).Rows.Count > 0 Then
                    strMultiPO = Common.parseNull(tDS.Tables(0).Rows(0).Item("CM_MULTI_PO"))
                End If
                'If drPR.Read Then
                '    strMultiPO = Common.parseNull(drPR("CM_MULTI_PO"))
                'End If 
                If strMultiPO = "Y" Then
                    eProcure.Component.ReportViewer.popCrystalReport(Me, "outstandGRNFrSupplHLB.rpt", crParam)
                Else
                    eProcure.Component.ReportViewer.popCrystalReport(Me, "outstandGRNFrSuppl2.rpt", crParam)
                End If
            Case "DRP"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                eProcure.Component.ReportViewer.popCrystalReport(Me, "PurchaseToInvoiceCycleRpt2.rpt", crParam)
            Case "OPOV"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                eProcure.Component.ReportViewer.popCrystalReport(Me, "outstandPOFrSupplSeh2.rpt", crParam)
            Case "PME"
                crParam.Add("companyIDParam", Session("CompanyIdToken"))
                eProcure.Component.ReportViewer.popCrystalReport(Me, "pmeExpenses2.rpt", crParam)
            Case "PMED"
                crParam.Add("companyIDParam", Session("CompanyIdToken"))
                eProcure.Component.ReportViewer.popCrystalReport(Me, "pmeExpensesDetail2.rpt", crParam)
            Case "CRFV"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "consumptionFromVendor.rpt", crParam)
            Case "InvSumm"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "InvoiceSummaryBySupplier.rpt", crParam)
            Case "OPOD"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "outstandingPoByDept.rpt", crParam)
            Case "ODO"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "outstandDOByDept.rpt", crParam)
            Case "PO"
                crParam.Add("BCOYID", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "PORaised.rpt", crParam)
            Case "ODOGRN"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                crParam.Add("doVsGrnDayParam", txtDay.Text)
                ReportViewer.popCrystalReport(Me, "DOvsGRNByDept.rpt", crParam)
            Case "CRBC"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "consumptionByCategory.rpt", crParam)
            Case "PTIC"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "PurchaseToInvoiceCycleByCategory.rpt", crParam)
            Case "INV20TO100"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "invoiceApproval20KTo100K.rpt", crParam)
            Case "INV100TO500"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "invoiceApproval100KTo500K.rpt", crParam)
            Case "POBYPSITE"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "detailOfPOByPurchaseSite.rpt", crParam)
            Case "GRNBYPSITE"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "outstandGRNByPurchaseSite.rpt", crParam)
            Case "OSINVBYDEPT"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "outstandInvoiceByDept.rpt", crParam)
            Case "APPRINVS"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "SummaryApprovedInvoicesByVendor.rpt", crParam)
            Case "R5"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "R5SummaryApprovedPaidInvoicesSortedByItem-BR.rpt", crParam)
            Case "R6"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "R6SummaryApprovedPaidInvoicesSortedByItem-HQ.rpt", crParam)
            Case "PTIFM"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "PurchaseToInvoiceCyclePrintStatSecurity-HQ.rpt", crParam)
            Case "PTIBR"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "PurchaseToInvoiceCyclePrintStatSecurity-BR.rpt", crParam)
            Case "PTGOEFF"
                crParam.Add("BCOYID", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "PurchaseToGRNCycleOEFF.rpt", crParam)
            Case "R11"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "R11DetailOutstandGRNReprotSortedByBranch-BR.rpt", crParam)
            Case "R12"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "R12DetailOutstandGRNReprotSortedByBranch-HQ.rpt", crParam)
            Case "VRFR"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "vendorRevenueForecast.rpt", crParam)
            Case "DBN"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "DebitNote.rpt", crParam)
        End Select
    End Sub


End Class
