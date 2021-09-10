Imports eProcure.Component
Public Class ReportFormat3
    Inherits AppCommon.AppBaseClass
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cboMonth As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboYear As System.Web.UI.WebControls.DropDownList
    Dim strType As String
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents ValMth As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents ValYr As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents label2 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Dim dispatcher As New Dispatcher.dispatcher

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


        Select Case strType

            Case "MthCon"
                lblHeader.Text = "Monthly Consumption Report"
            Case "DirectChar"
                lblHeader.Text = "Direct Charge Item"

            Case "MthInvCum"
                'lblHeader.Text = "Monthly Invoice Cumulative (MIP) - S.E.H"
                lblHeader.Text = "Monthly Invoice Cumulative (MIP)"

            Case "MthPOCum"
                lblHeader.Text = "Monthly Purchase Order Cumulative Special Project"
            Case "MthConCC"
                lblHeader.Text = "Monthly Summary Consumption By Cost Center"
                'Michelle (17/8/2007) - Add in HLB Reports
            Case "MTHEXPACTBUDG"
                lblHeader.Text = "Monthly Opex Expenditure Report (Actual VS Budget) By Department"
            Case "MTHEXCPEXP"
                lblHeader.Text = "Monthly Exception Expenditure By Cost Center"
            Case "MTHEXP"
                lblHeader.Text = "Monthly Expenditure Report (Compared With Previous Year) By Cost Center"
            Case "MTHOPEX"
                lblHeader.Text = "Monthly Opex Department Summary Expenditure Report"
            Case "R1DTINV"
                lblHeader.Text = "R1 Detail Paid Invoices Report Sort By GL Code For Branch"
            Case "R2DTINV"
                lblHeader.Text = "R2 Detail Paid Invoices Report Sort By GL Code For Head Office"
            Case "R3"
                lblHeader.Text = "R3 Detail Committed Orders Report Sort By GL Code For Branch"
            Case "R4"
                lblHeader.Text = "R4  Detail Committed Orders Report Sort By GL Code For Head Office"
                'Michelle (5/5/2009) - CR0051
            Case "EXC"
                lblHeader.Text = "Monthly Expense Charge Out To Cost Centre"
            Case "ESE"
                lblHeader.Text = "Monthly Expense For eSystems"
        End Select

        If Not (Page.IsPostBack) Then

            Dim ii_ddl, ii_ddl2, jj_ddl As Integer
            ii_ddl2 = 0
            'jj_ddl = 2020
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
        lnkBack.NavigateUrl = Dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId)

    End Sub

  
    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim crParam As New Hashtable
        'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        crParam.Add("userIDParam", Session("UserID"))


        'crParam.Add("BCOYID", "MBB")
        'crParam.Add("userIDParam", "MOO")
        'Dim test As String = cboMonth.SelectedValue
        'Dim test1 As String = cboMonth.SelectedItem.Value
        'Dim month1 As Integer = cboMonth.SelectedValue
        'Dim month As Integer = CInt(cboMonth.SelectedItem.Value)
        'Dim month As Integer = cboMonth.SelectedIndex
        Dim month As Integer = cboMonth.SelectedItem.Value
        'Dim month As String = "january"
        Dim year As Integer = cboYear.SelectedItem.Value

        crParam.Add("monthParam", month)
        crParam.Add("yearParam", year)


        Select Case strType
            Case "MthCon"               
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                eProcure.Component.ReportViewer.popCrystalReport(Me, "monthlyConsumptionRpt2.rpt", crParam) 'check this the date
            Case "DirectChar"
                crParam.Add("companyIDParam", Session("CompanyIdToken"))
                eProcure.Component.ReportViewer.popCrystalReport(Me, "directChargeItem2.rpt", crParam)
            Case "MthInvCum"
                crParam.Add("companyIDParam", Session("CompanyIdToken"))
                eProcure.Component.ReportViewer.popCrystalReport(Me, "monthlyInvoiceMIP2.rpt", crParam)
            Case "MthPOCum"
                crParam.Add("companyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "monthlyPOCummSpecProject2.rpt", crParam)
            Case "MthConCC"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "SummCostCenterByDept.rpt", crParam)
            Case "MTHEXPACTBUDG"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "monthlyNonCapexExpenditureActualVsBudget.rpt", crParam)
            Case "MTHEXCPEXP"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "monthlyExceptionExpendByCostCenter.rpt", crParam)
            Case "MTHEXP"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "monthlyExpendComparePrevYrByCostCenter.rpt", crParam)
            Case "MTHOPEX"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "monthlyNonCapexDepartmentalSummaryExpenditure.rpt", crParam)
            Case "R1DTINV"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "R1detailPaidInvoiceByGLCode-BR.rpt", crParam)
            Case "R2DTINV"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "R2detailPaidInvoiceByGLCode-HO.rpt", crParam)
            Case "R3"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "R3detailCommitedOrdersByGLCode-BR.rpt", crParam)
            Case "R4"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "R4detailCommitedOrdersByGLCode-HQ.rpt", crParam)
            Case "EXC"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "ExpenseChargeOutToCostCentre.rpt", crParam)
            Case "ESE"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "eSystemMonthlyExpense.rpt", crParam)
        End Select

    End Sub
End Class
