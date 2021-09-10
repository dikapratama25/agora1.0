'Imports Wheel.Components
Imports AgoraLegacy
Imports eProcure.Component
Imports MySql.Data.MySqlClient
Public Class ReportFormat3
    Inherits AgoraLegacy.AppBaseClass
    Dim objDb As New EAD.DBCom("server=10.228.235.57;UID=root;pwd=p@ssw0rd;database=eProcure")
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
    Protected WithEvents cboReportType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents tr1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents dtRadioBtn As System.Web.UI.WebControls.RadioButtonList
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


        Select Case strType

            Case "MthCon"
                lblHeader.Text = "Monthly Consumption Report"

                ' Case "MthInv"
                '    lblHeader.Text = "MONTHLY INVOICE CUMULATIVE (MIP)"

                'Case "MthPO"
                '   lblHeader.Text = "MONTHLY PURCHASE ORDER CUMULATIVE SPECIAL PROJECT"

            Case "DirectChar"
                lblHeader.Text = "Direct Charge Item"

            Case "MthInvCum"
                'lblHeader.Text = "Monthly Invoice Cumulative (MIP) - S.E.H"
                lblHeader.Text = "Monthly Invoice Cumulative (MIP)"

            Case "MthPOCum"
                lblHeader.Text = "Monthly Purchase Order Cumulative Special Project"
            Case "MthConCC"
                lblHeader.Text = "Monthly Summary Consumption By Cost Center"
            Case "MTHEXCPEXP"
                lblHeader.Text = "Monthly Exception Expenditure By Cost Center"
            Case "MTHEXP"
                lblHeader.Text = "Monthly Expenditure Report (Compared With Previous Year) By Cost Center"
            Case "MTHOPEX"
                lblHeader.Text = "Monthly Opex Department Summary Expenditure Report"
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

                'Jules 2016.01.27 - FITR enhancement.
            Case "R1_1DTINV"
                lblHeader.Text = "R1_1 Detail Paid Invoices Report Sort By GL Code and Year For Branch"
            Case "R2_1DTINV"
                lblHeader.Text = "R2_1 Detail Paid Invoices Report Sort By GL Code and Year For Head Office"
            Case "R2_2DTINV"
                lblHeader.Text = "R2_2 Detail Paid Invoices Report Sort By GL Code For Branch and Head Office"
                'End.
        End Select

        'Chee hong (07/02/2014) - Included options for capex & opex

        'Jules 2016.01.27 - FITR enhancement.
        If strType = "R1DTINV" Or strType = "R2DTINV" Or strType = "R3" Or strType = "R4" Or strType = "R2_2DTINV" Then
            'If strType = "R1DTINV" Or strType = "R2DTINV" Or strType = "R3" Or strType = "R4" Then
            tr1.Style("display") = ""

        ElseIf strType = "R1_1DTINV" Or strType = "R2_1DTINV" Then
            tr1.Style("display") = ""
            trMth.Style("display") = "none"
            ValMth.Enabled = False
            'End.

        Else
            tr1.Style("display") = "none"
        End If

        If Not (Page.IsPostBack) Then

            Dim ii_ddl, ii_ddl2, jj_ddl As Integer
            ii_ddl2 = 0
            'jj_ddl = 2021
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
        lnkBack.NavigateUrl = "ReportSelection.aspx?pageid=" & strPageId
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim strReportName As String = ""
        Dim reportURL As String
        'Dim crParam As New Hashtable
        Dim strDateFrom As String = ""
        Dim strDateTo As String = ""
        Dim strParam As String = ""

        'Jules 2016.01.27 - FITR enhancement.
        'Dim month As Integer = cboMonth.SelectedItem.Value
        Dim month As Integer
        If strType <> "R1_1DTINV" And strType <> "R2_1DTINV" Then
            month = cboMonth.SelectedItem.Value
        End If
        'End.

        Dim year As Integer = cboYear.SelectedItem.Value

        'crParam.Add("userIDParam", Session("UserID"))
        'crParam.Add("monthParam", month)
        'crParam.Add("yearParam", year)

        Select Case strType
            Case "MthCon"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "monthlyConsumptionRpt2.rpt" 'check this the date
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&monthParam=" & month & "&yearParam=" & year

            Case "DirectChar"
                'crParam.Add("companyIDParam", Session("CompanyID"))
                strReportName = "directChargeItem2.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&companyIDParam=" & Session("CompanyID") & "&monthParam=" & month & "&yearParam=" & year

            Case "MthInvCum"
                'crParam.Add("companyIDParam", Session("CompanyID"))
                strReportName = "monthlyInvoiceMIP2.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&companyIDParam=" & Session("CompanyID") & "&monthParam=" & month & "&yearParam=" & year

            Case "MthPOCum"
                'crParam.Add("companyIDParam", Session("CompanyID"))
                strReportName = "monthlyPOCummSpecProject2.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&companyIDParam=" & Session("CompanyID") & "&monthParam=" & month & "&yearParam=" & year

            Case "MthConCC"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "SummCostCenterByDept.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&monthParam=" & month & "&yearParam=" & year

            Case "MTHEXPACTBUDG"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "monthlyNonCapexExpenditureActualVsBudget.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&monthParam=" & month & "&yearParam=" & year

            Case "MTHEXCPEXP"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "monthlyExceptionExpendByCostCenter.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&monthParam=" & month & "&yearParam=" & year

            Case "MTHEXP"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "monthlyExpendComparePrevYrByCostCenter.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&monthParam=" & month & "&yearParam=" & year

            Case "MTHOPEX"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "monthlyNonCapexDepartmentalSummaryExpenditure.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&monthParam=" & month & "&yearParam=" & year


            Case "R1DTINV"
                'mimi - 2017/01/10 - Fitr enhancement
                Dim objDbcom As New EAD.DBCom
                Dim strRecoverablePercentage As String
                Dim strIRRecoverablePercentage As String

                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strDateFrom = year & "-" & Format(month - 1, "00") & "-" & "22" & "T00:00:00"
                strDateTo = year & "-" & Format(month, "00") & "-" & "21" & "T23:59:59"
                'crParam.Add("InvDtFrom", strDateFrom)
                'crParam.Add("InvDtTo", strDateTo)

                'mimi - 16/01/2017 - Fitr Enhancement
                strRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_recoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                strIRRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_irrecoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")

                If Me.dtRadioBtn.SelectedValue = "opex" Then
                    strReportName = "R1detailPaidInvoiceByGLCode-BR-Opex.rpt"
                ElseIf Me.dtRadioBtn.SelectedValue = "capex" Then
                    strReportName = "R1detailPaidInvoiceByGLCode-BR-Capex.rpt"
                Else
                    strReportName = "R1detailPaidInvoiceByGLCode-BR.rpt"
                End If
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&monthParam=" & month & "&yearParam=" & year & "&InvDtFrom=" & strDateFrom & "&InvDtTo=" & strDateTo & "&RecoverablePercentage=" & strRecoverablePercentage & "&IRRecoverablePercentage=" & strIRRecoverablePercentage
                'end - mimi

            Case "R2DTINV"
                'mimi 2017/01/10 - Fitr enhancement
                Dim objDbcom As New EAD.DBCom
                Dim strRecoverablePercentage As String
                Dim strIRRecoverablePercentage As String
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strDateFrom = year & "-" & Format(month - 1, "00") & "-" & "22" & "T00:00:00"
                strDateTo = year & "-" & Format(month, "00") & "-" & "21" & "T23:59:59"
                'crParam.Add("InvDtFrom", strDateFrom)
                'crParam.Add("InvDtTo", strDateTo)

                'mimi 2017/01/16 - Fitr enhancement
                strRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_recoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                strIRRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_irrecoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")

                If Me.dtRadioBtn.SelectedValue = "opex" Then
                    strReportName = "R2detailPaidInvoiceByGLCode-HO-Opex.rpt"
                ElseIf Me.dtRadioBtn.SelectedValue = "capex" Then
                    strReportName = "R2detailPaidInvoiceByGLCode-HO-Capex.rpt"
                Else
                    strReportName = "R2detailPaidInvoiceByGLCode-HO.rpt"
                End If
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&monthParam=" & month & "&yearParam=" & year & "&InvDtFrom=" & strDateFrom & "&InvDtTo=" & strDateTo & "&RecoverablePercentage=" & strRecoverablePercentage & "&IRRecoverablePercentage=" & strIRRecoverablePercentage
                'mimi-end
            Case "R3"
                'mimi - 17/01/10 - Fitr enhancement
                Dim objDbcom As New EAD.DBCom
                Dim strRecoverablePercentage As String
                Dim strIRRecoverablePercentage As String

                'mimi 2017/01/16 - Fitr enhancement
                strRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_recoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                strIRRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_irrecoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                'end

                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                If Me.dtRadioBtn.SelectedValue = "opex" Then
                    strReportName = "R3detailCommitedOrdersByGLCode-BR-Opex.rpt"
                ElseIf Me.dtRadioBtn.SelectedValue = "capex" Then
                    strReportName = "R3detailCommitedOrdersByGLCode-BR-Capex.rpt"
                Else
                    strReportName = "R3detailCommitedOrdersByGLCode-BR.rpt"
                End If
                'strReportName = "R3detailCommitedOrdersByGLCode-BR.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&monthParam=" & month & "&yearParam=" & year & "&RecoverablePercentage=" & strRecoverablePercentage & "&IRRecoverablePercentage=" & strIRRecoverablePercentage
                'end - mimi
                'strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&monthParam=" & month & "&yearParam=" & year

            Case "R4"
                'mimi - 17/01/10 - Fitr enhancement
                Dim objDbcom As New EAD.DBCom
                Dim strRecoverablePercentage As String
                Dim strIRRecoverablePercentage As String

                'mimi 2017/01/16 - Fitr enhancement
                strRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_recoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                strIRRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_irrecoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                'end

                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                If Me.dtRadioBtn.SelectedValue = "opex" Then
                    'strReportName = "R4detailCommitedOrdersByGLCode-HQ-Opex.rpt" 'Jules commented 2016.01.27 - SVN version.                    
                    strReportName = "R4detailCommitedOrdersByGLCode-HO-Opex.rpt" 'Jules 2016.01.27 - FITR based on latest copy from CK.
                ElseIf Me.dtRadioBtn.SelectedValue = "capex" Then
                    'strReportName = "R4detailCommitedOrdersByGLCode-HQ-Capex.rpt" 'Jules commented 2016.01.27 - SVN version.
                    strReportName = "R4detailCommitedOrdersByGLCode-HO-Capex.rpt" 'Jules 2016.01.27 - FITR based on latest copy from CK.
                Else
                    strReportName = "R4detailCommitedOrdersByGLCode-HQ.rpt"
                End If
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&monthParam=" & month & "&yearParam=" & year & "&RecoverablePercentage=" & strRecoverablePercentage & "&IRRecoverablePercentage=" & strIRRecoverablePercentage
                'end - mimi

            Case "EXC"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "ExpenseChargeOutToCostCentre.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&monthParam=" & month & "&yearParam=" & year

            Case "ESE"
                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "eSystemMonthlyExpense.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&monthParam=" & month & "&yearParam=" & year

                'Jules 2016.01.27 - FITR enhancement.
            Case "R1_1DTINV"
                'mimi 2017/01/10 - Fitr enhancement
                Dim objDbcom As New EAD.DBCom
                Dim strRecoverablePercentage As String
                Dim strIRRecoverablePercentage As String

                'mimi 2017/01/16 - Fitr enhancement
                strRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_recoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                strIRRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_irrecoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                'end

                If Me.dtRadioBtn.SelectedValue = "opex" Then
                    strReportName = "R1_1detailPaidInvoiceByGLCode-BR-Opex.rpt"
                ElseIf Me.dtRadioBtn.SelectedValue = "capex" Then
                    strReportName = "R1_1detailPaidInvoiceByGLCode-BR-Capex.rpt"
                    'Else
                    '    strReportName = "R1detailPaidInvoiceByGLCode-BR.rpt"
                End If
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&yearParam=" & year & "&InvDtFrom=" & strDateFrom & "&InvDtTo=" & strDateTo & "&RecoverablePercentage=" & strRecoverablePercentage & "&IRRecoverablePercentage=" & strIRRecoverablePercentage
                'end-mimi
            Case "R2_1DTINV"
                'mimi 2017/01/10 - Fitr enhancement
                Dim objDbcom As New EAD.DBCom
                Dim strRecoverablePercentage As String
                Dim strIRRecoverablePercentage As String

                'mimi 2017/01/16 - Fitr enhancement
                strRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_recoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                strIRRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_irrecoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                'end

                If Me.dtRadioBtn.SelectedValue = "opex" Then
                    strReportName = "R2_1detailPaidInvoiceByGLCode-HO-Opex.rpt"
                ElseIf Me.dtRadioBtn.SelectedValue = "capex" Then
                    strReportName = "R2_1detailPaidInvoiceByGLCode-HO-Capex.rpt"
                    'Else
                    '    strReportName = "R2detailPaidInvoiceByGLCode-HO.rpt"
                End If
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&yearParam=" & year & "&InvDtFrom=" & strDateFrom & "&InvDtTo=" & strDateTo & "&RecoverablePercentage=" & strRecoverablePercentage & "&IRRecoverablePercentage=" & strIRRecoverablePercentage
                'end-mimi
            Case "R2_2DTINV"
                'mimi 2017/01/10 - Fitr enhancement
                Dim objDbcom As New EAD.DBCom
                Dim strRecoverablePercentage As String
                Dim strIRRecoverablePercentage As String

                'mimi 2017/01/16 - Fitr enhancement
                strRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_recoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                strIRRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_irrecoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                'end

                If Me.dtRadioBtn.SelectedValue = "opex" Then
                    strReportName = "R2_2detailPaidInvoiceByGLCode-Opex.rpt"
                ElseIf Me.dtRadioBtn.SelectedValue = "capex" Then
                    
                    strReportName = "R2_2detailPaidInvoiceByGLCode-Capex.rpt"
                    'Else
                    '    strReportName = "R2detailPaidInvoiceByGLCode-HO.rpt"
                End If
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&monthParam=" & month & "&yearParam=" & year & "&InvDtFrom=" & strDateFrom & "&InvDtTo=" & strDateTo & "&RecoverablePercentage=" & strRecoverablePercentage & "&IRRecoverablePercentage=" & strIRRecoverablePercentage
                'end-mimi
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
        Me.ClientScript.RegisterStartupScript(GetType(ReportFormat3), rndKey.Next.ToString, jscript)
    End Sub

    'Private Sub ExportToExcel()
    '    Dim ds As New DataSet
    '    Dim conn As MySqlConnection = Nothing
    '    Dim cmd As MySqlCommand = Nothing
    '    Dim da As MySqlDataAdapter = Nothing
    '    Dim rdr As MySqlDataReader = Nothing
    '    Dim myConnectionString As String
    '    Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
    '    Dim dtDate As Date
    '    Dim strBeginDate As String
    '    Dim strEndDate As String
    '    Dim strTitle As String
    '    Dim strFileName As String = ""

    '    Try
    '        Dim intMonth As Integer = cboMonth.SelectedItem.Value
    '        Dim intYear As Integer = cboYear.SelectedItem.Value

    '        strBeginDate = intYear & "-" & Format(intMonth - 1, "00") & "-" & "22" & " 00:00:00" '"2011-11-01 00:00:00" 
    '        strEndDate = intYear & "-" & Format(intMonth, "00") & "-" & "21" & " 23:59:59" '"2011-11-01 23:59:59" 
    '        strTitle = MonthName(intMonth, True) & intYear

    '        myConnectionString = "server=10.228.235.57;UID=root;pwd=p@ssw0rd;database=eProcure" 'System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" 'System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
    '        conn = New MySqlConnection(myConnectionString)
    '        conn.Open()

    '        cmd = New MySqlCommand
    '        With cmd
    '            .Connection = conn
    '            .CommandTimeout = 10000
    '            .CommandType = CommandType.Text
    '            Session("CompanyID") = "hlb"
    '            '.CommandText = "SELECT IM_B_COY_ID, POM_B_COY_ID, POM_PO_STATUS, POM_FULFILMENT, " _
    '            '        & "CM_COY_ID, CM_COY_NAME, IM_INVOICE_STATUS,  " _
    '            '        & "IM_FM_APPROVED_DATE, ID_B_GL_CODE, CDM_DEPT_CODE, " _
    '            '        & "IM_INVOICE_NO, ID_B_CATEGORY_CODE, CDM_DEPT_NAME,  " _
    '            '        & "ID_B_ITEM_CODE, ID_PRODUCT_DESC, IM_CREATED_ON,  " _
    '            '        & "ID_GST, ID_UNIT_COST,ID_RECEIVED_QTY,  " _
    '            '        & "ID_UOM, IM_S_COY_NAME, POM_PO_NO, POM_PO_DATE,  " _
    '            '        & "IM_DOWNLOADED_DATE, POM_BUYER_NAME, AM_ACCT_CODE,  " _
    '            '        & "AM_ACCT_DESC " _
    '            '        & "FROM   ((((PO_MSTR  " _
    '            '        & "INNER JOIN INVOICE_MSTR ON POM_PO_INDEX=IM_PO_INDEX AND IM_B_COY_ID=POM_B_COY_ID)  " _
    '            '        & "INNER JOIN COMPANY_MSTR ON POM_B_COY_ID=CM_COY_ID)  " _
    '            '        & "INNER JOIN ACCOUNT_MSTR ON POM_B_COY_ID=AM_COY_ID)  " _
    '            '        & "INNER JOIN COMPANY_DEPT_MSTR ON (AM_COY_ID=CDM_COY_ID)  " _
    '            '        & "AND (AM_DEPT_INDEX=CDM_DEPT_INDEX))  " _
    '            '        & "INNER JOIN INVOICE_DETAILS  " _
    '            '        & "ON (IM_INVOICE_NO=ID_INVOICE_NO)  " _
    '            '        & "AND (IM_S_COY_ID=ID_S_COY_ID) " _
    '            '        & "WHERE(IM_INVOICE_STATUS = 4) " _
    '            '        & "AND  NOT (POM_PO_STATUS=0 OR POM_PO_STATUS=1 OR POM_PO_STATUS=2  " _
    '            '        & "OR POM_PO_STATUS=4 OR POM_PO_STATUS=5 OR POM_PO_STATUS=7  " _
    '            '        & "OR POM_PO_STATUS=8 OR POM_PO_STATUS=10 OR POM_PO_STATUS=11  " _
    '            '        & "OR POM_PO_STATUS=12 OR POM_PO_STATUS=13)  " _
    '            '        & "AND  NOT (POM_FULFILMENT=1 OR POM_FULFILMENT=5)  " _
    '            '        & "AND (ID_B_GL_CODE='20709000000' OR ID_B_GL_CODE='70004090000'  " _
    '            '        & "OR ID_B_GL_CODE='70007041000' OR ID_B_GL_CODE='70007043000'  " _
    '            '        & "OR ID_B_GL_CODE='70007044000' OR ID_B_GL_CODE='70007046000'  " _
    '            '        & "OR ID_B_GL_CODE='70007047000') " _
    '            '        & "AND POM_B_COY_ID = '" & Session("CompanyID") & "' " _
    '            '        & "AND IM_B_COY_ID = '" & Session("CompanyID") & "' " _
    '            '        & "AND (INVOICE_MSTR.IM_FM_APPROVED_DATE >='" & strBeginDate & "'  " _
    '            '        & "AND INVOICE_MSTR.IM_FM_APPROVED_DATE <='" & strEndDate & "') " _
    '            '        & "AND RIGHT(CDM_DEPT_CODE,3) BETWEEN '001' AND '499' " _
    '            '        & "ORDER BY ID_B_GL_CODE"
    '            .CommandText = "SELECT IM_B_COY_ID, POM_B_COY_ID, POM_PO_STATUS, POM_FULFILMENT, " _
    '                                    & "CM_COY_ID, CM_COY_NAME, IM_INVOICE_STATUS,  " _
    '                                    & "IM_FM_APPROVED_DATE, ID_B_GL_CODE, CDM_DEPT_CODE, " _
    '                                    & "IM_INVOICE_NO, ID_B_CATEGORY_CODE, CDM_DEPT_NAME,  " _
    '                                    & "ID_B_ITEM_CODE, ID_PRODUCT_DESC, IM_CREATED_ON,  " _
    '                                    & "ID_GST, ID_UNIT_COST,ID_RECEIVED_QTY,  " _
    '                                    & "ID_UOM, IM_S_COY_NAME, POM_PO_NO, POM_PO_DATE,  " _
    '                                    & "IM_DOWNLOADED_DATE, POM_BUYER_NAME, AM_ACCT_CODE,  " _
    '                                    & "AM_ACCT_DESC " _
    '                                    & "FROM   ((((PO_MSTR  " _
    '                                    & "INNER JOIN INVOICE_MSTR ON POM_PO_INDEX=IM_PO_INDEX AND IM_B_COY_ID=POM_B_COY_ID)  " _
    '                                    & "INNER JOIN COMPANY_MSTR ON POM_B_COY_ID=CM_COY_ID)  " _
    '                                    & "INNER JOIN ACCOUNT_MSTR ON POM_B_COY_ID=AM_COY_ID)  " _
    '                                    & "INNER JOIN COMPANY_DEPT_MSTR ON (AM_COY_ID=CDM_COY_ID)  " _
    '                                    & "AND (AM_DEPT_INDEX=CDM_DEPT_INDEX))  " _
    '                                    & "INNER JOIN INVOICE_DETAILS  " _
    '                                    & "ON (IM_INVOICE_NO=ID_INVOICE_NO)  " _
    '                                    & "AND (IM_S_COY_ID=ID_S_COY_ID) " _
    '                                    & "WHERE(IM_INVOICE_STATUS = 4) " _
    '                                    & "AND (POM_PO_STATUS=3 OR POM_PO_STATUS=6 OR POM_PO_STATUS=9)  " _
    '                                    & "AND (POM_FULFILMENT=2 OR POM_FULFILMENT=3 OR POM_FULFILMENT=4)  " _
    '                                    & "AND (ID_B_GL_CODE='20709000000' OR ID_B_GL_CODE='70004090000'  " _
    '                                    & "OR ID_B_GL_CODE='70007041000' OR ID_B_GL_CODE='70007043000'  " _
    '                                    & "OR ID_B_GL_CODE='70007044000' OR ID_B_GL_CODE='70007046000'  " _
    '                                    & "OR ID_B_GL_CODE='70007047000') " _
    '                                    & "AND POM_B_COY_ID = '" & Session("CompanyID") & "' " _
    '                                    & "AND IM_B_COY_ID = '" & Session("CompanyID") & "' " _
    '                                    & "AND (INVOICE_MSTR.IM_FM_APPROVED_DATE >='" & strBeginDate & "'  " _
    '                                    & "AND INVOICE_MSTR.IM_FM_APPROVED_DATE <='" & strEndDate & "') " _
    '                                    & "AND RIGHT(CDM_DEPT_CODE,3) BETWEEN '001' AND '499' " _
    '                                    & "ORDER BY ID_B_GL_CODE"
    '        End With

    '        da = New MySqlDataAdapter(cmd)
    '        da.Fill(ds)
    '        strFileName = "InvDetailsReport" & "(" & strTitle & ").xls"
    '        Dim attachment As String = "attachment;filename=" & strFileName
    '        Response.ClearContent()
    '        Response.AddHeader("Content-Disposition", attachment)
    '        Response.ContentType = "application/vnd.ms-excel"

    '        Dim dc As DataColumn
    '        Dim i As Integer

    '        i = 0
    '        For Each dc In ds.Tables(0).Columns
    '            If i > 0 Then
    '                Response.Write(vbTab + dc.ColumnName)
    '            Else
    '                Response.Write(dc.ColumnName)
    '            End If
    '            i += 1

    '        Next
    '        Response.Write(vbCrLf)

    '        Dim dr As DataRow
    '        For Each dr In ds.Tables(0).Rows
    '            For i = 0 To ds.Tables(0).Columns.Count - 1
    '                If i > 0 Then
    '                    Response.Write(vbTab + dr.Item(i).ToString)
    '                Else
    '                    Response.Write(dr.Item(i).ToString)
    '                End If
    '            Next
    '            Response.Write(vbCrLf)
    '        Next
    '        Response.End()

    '    Catch ex As Exception
    '    Finally
    '        cmd = Nothing
    '        If Not IsNothing(rdr) Then
    '            rdr.Close()
    '        End If
    '        If Not IsNothing(conn) Then
    '            If conn.State = ConnectionState.Open Then
    '                conn.Close()
    '            End If
    '        End If
    '        conn = Nothing
    '    End Try

    'End Sub
    'Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
    '    Dim crParam As New Hashtable
    '    'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '    crParam.Add("userIDParam", Session("UserID"))


    '    'crParam.Add("BCOYID", "MBB")
    '    'crParam.Add("userIDParam", "MOO")
    '    'Dim test As String = cboMonth.SelectedValue
    '    'Dim test1 As String = cboMonth.SelectedItem.Value
    '    'Dim month1 As Integer = cboMonth.SelectedValue
    '    'Dim month As Integer = CInt(cboMonth.SelectedItem.Value)
    '    'Dim month As Integer = cboMonth.SelectedIndex
    '    Dim month As Integer = cboMonth.SelectedItem.Value
    '    'Dim month As String = "january"
    '    Dim year As Integer = cboYear.SelectedItem.Value

    '    crParam.Add("monthParam", month)
    '    crParam.Add("yearParam", year)


    '    Select Case strType
    '        Case "MthCon"
    '            'crParam.Add("monthParam", month)
    '            'crParam.Add("yearParam", year)
    '            crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "monthlyConsumptionRpt2.rpt", crParam) 'check this the date
    '        Case "DirectChar"
    '            crParam.Add("companyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "directChargeItem2.rpt", crParam)
    '        Case "MthInvCum"
    '            crParam.Add("companyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "monthlyInvoiceMIP2.rpt", crParam)
    '        Case "MthPOCum"
    '            crParam.Add("companyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "monthlyPOCummSpecProject2.rpt", crParam)
    '        Case "MthConCC"
    '            crParam.Add("buyercompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "SummCostCenterByDept.rpt", crParam)
    '        Case "MTHEXPACTBUDG"
    '            crParam.Add("buyercompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "monthlyNonCapexExpenditureActualVsBudget.rpt", crParam)
    '        Case "MTHEXCPEXP"
    '            crParam.Add("buyercompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "monthlyExceptionExpendByCostCenter.rpt", crParam)
    '        Case "MTHEXP"
    '            crParam.Add("buyercompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "monthlyExpendComparePrevYrByCostCenter.rpt", crParam)
    '        Case "MTHOPEX"
    '            crParam.Add("buyercompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "monthlyNonCapexDepartmentalSummaryExpenditure.rpt", crParam)
    '        Case "R1DTINV"
    '            crParam.Add("buyercompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "R1detailPaidInvoiceByGLCode-BR.rpt", crParam)
    '        Case "R2DTINV"
    '            crParam.Add("buyercompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "R2detailPaidInvoiceByGLCode-HO.rpt", crParam)
    '        Case "R3"
    '            crParam.Add("buyercompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "R3detailCommitedOrdersByGLCode-BR.rpt", crParam)
    '        Case "R4"
    '            crParam.Add("buyercompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "R4detailCommitedOrdersByGLCode-HQ.rpt", crParam)
    '        Case "EXC"
    '            crParam.Add("buyercompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "ExpenseChargeOutToCostCentre.rpt", crParam)
    '        Case "ESE"
    '            crParam.Add("buyercompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "eSystemMonthlyExpense.rpt", crParam)
    '    End Select

    'End Sub

    Private Sub ExportToExcel()
        Dim ds As DataSet
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim dtDate As Date
        Dim strBeginDate As String
        Dim strEndDate As String
        Dim strTitle As String
        Dim strFileName As String = ""
        Dim strsql As String = ""

        Try
            Dim intMonth As Integer = cboMonth.SelectedItem.Value
            Dim intYear As Integer = cboYear.SelectedItem.Value

            strBeginDate = intYear & "-" & Format(intMonth - 1, "00") & "-" & "22" & " 00:00:00" '"2011-11-01 00:00:00" 
            strEndDate = intYear & "-" & Format(intMonth, "00") & "-" & "21" & " 23:59:59" '"2011-11-01 23:59:59" 
            strTitle = MonthName(intMonth, True) & intYear

            'mimi 2018-04-24 : remove hardcode HLB to PAMB
            'Session("CompanyID") = "hlb"
            Session("CompanyID") = "pamb"
            strsql = "SELECT IM_B_COY_ID, POM_B_COY_ID, POM_PO_STATUS, POM_FULFILMENT, " _
                    & "CM_COY_ID, CM_COY_NAME, IM_INVOICE_STATUS,  " _
                    & "IM_FM_APPROVED_DATE, ID_B_GL_CODE, CDM_DEPT_CODE, " _
                    & "IM_INVOICE_NO, ID_B_CATEGORY_CODE, CDM_DEPT_NAME,  " _
                    & "ID_B_ITEM_CODE, ID_PRODUCT_DESC, IM_CREATED_ON,  " _
                    & "ID_GST, ID_UNIT_COST,ID_RECEIVED_QTY,  " _
                    & "ID_UOM, IM_S_COY_NAME, POM_PO_NO, POM_PO_DATE,  " _
                    & "IM_DOWNLOADED_DATE, POM_BUYER_NAME, AM_ACCT_CODE,  " _
                    & "AM_ACCT_DESC " _
                    & "FROM   ((((PO_MSTR  " _
                    & "INNER JOIN INVOICE_MSTR ON POM_PO_INDEX=IM_PO_INDEX AND IM_B_COY_ID=POM_B_COY_ID)  " _
                    & "INNER JOIN COMPANY_MSTR ON POM_B_COY_ID=CM_COY_ID)  " _
                    & "INNER JOIN ACCOUNT_MSTR ON POM_B_COY_ID=AM_COY_ID)  " _
                    & "INNER JOIN COMPANY_DEPT_MSTR ON (AM_COY_ID=CDM_COY_ID)  " _
                    & "AND (AM_DEPT_INDEX=CDM_DEPT_INDEX))  " _
                    & "INNER JOIN INVOICE_DETAILS  " _
                    & "ON (IM_INVOICE_NO=ID_INVOICE_NO)  " _
                    & "AND (IM_S_COY_ID=ID_S_COY_ID) " _
                    & "WHERE(IM_INVOICE_STATUS = 4) " _
                    & "AND  NOT (POM_PO_STATUS=0 OR POM_PO_STATUS=1 OR POM_PO_STATUS=2  " _
                    & "OR POM_PO_STATUS=4 OR POM_PO_STATUS=5 OR POM_PO_STATUS=7  " _
                    & "OR POM_PO_STATUS=8 OR POM_PO_STATUS=10 OR POM_PO_STATUS=11  " _
                    & "OR POM_PO_STATUS=12 OR POM_PO_STATUS=13)  " _
                    & "AND  NOT (POM_FULFILMENT=1 OR POM_FULFILMENT=5)  " _
                    & "AND (ID_B_GL_CODE='20709000000' OR ID_B_GL_CODE='70004090000'  " _
                    & "OR ID_B_GL_CODE='70007041000' OR ID_B_GL_CODE='70007043000'  " _
                    & "OR ID_B_GL_CODE='70007044000' OR ID_B_GL_CODE='70007046000'  " _
                    & "OR ID_B_GL_CODE='70007047000') " _
                    & "AND POM_B_COY_ID = '" & Session("CompanyID") & "' " _
                    & "AND IM_B_COY_ID = '" & Session("CompanyID") & "' " _
                    & "AND (INVOICE_MSTR.IM_FM_APPROVED_DATE >='" & strBeginDate & "'  " _
                    & "AND INVOICE_MSTR.IM_FM_APPROVED_DATE <='" & strEndDate & "') " _
                    & "AND RIGHT(CDM_DEPT_CODE,3) BETWEEN '001' AND '499' " _
                    & "ORDER BY ID_B_GL_CODE"
            '.CommandText = "SELECT IM_B_COY_ID, POM_B_COY_ID, POM_PO_STATUS, POM_FULFILMENT, " _
            '                        & "CM_COY_ID, CM_COY_NAME, IM_INVOICE_STATUS,  " _
            '                        & "IM_FM_APPROVED_DATE, ID_B_GL_CODE, CDM_DEPT_CODE, " _
            '                        & "IM_INVOICE_NO, ID_B_CATEGORY_CODE, CDM_DEPT_NAME,  " _
            '                        & "ID_B_ITEM_CODE, ID_PRODUCT_DESC, IM_CREATED_ON,  " _
            '                        & "ID_GST, ID_UNIT_COST,ID_RECEIVED_QTY,  " _
            '                        & "ID_UOM, IM_S_COY_NAME, POM_PO_NO, POM_PO_DATE,  " _
            '                        & "IM_DOWNLOADED_DATE, POM_BUYER_NAME, AM_ACCT_CODE,  " _
            '                        & "AM_ACCT_DESC " _
            '                        & "FROM   ((((PO_MSTR  " _
            '                        & "INNER JOIN INVOICE_MSTR ON POM_PO_INDEX=IM_PO_INDEX AND IM_B_COY_ID=POM_B_COY_ID)  " _
            '                        & "INNER JOIN COMPANY_MSTR ON POM_B_COY_ID=CM_COY_ID)  " _
            '                        & "INNER JOIN ACCOUNT_MSTR ON POM_B_COY_ID=AM_COY_ID)  " _
            '                        & "INNER JOIN COMPANY_DEPT_MSTR ON (AM_COY_ID=CDM_COY_ID)  " _
            '                        & "AND (AM_DEPT_INDEX=CDM_DEPT_INDEX))  " _
            '                        & "INNER JOIN INVOICE_DETAILS  " _
            '                        & "ON (IM_INVOICE_NO=ID_INVOICE_NO)  " _
            '                        & "AND (IM_S_COY_ID=ID_S_COY_ID) " _
            '                        & "WHERE(IM_INVOICE_STATUS = 4) " _
            '                        & "AND (POM_PO_STATUS=3 OR POM_PO_STATUS=6 OR POM_PO_STATUS=9)  " _
            '                        & "AND (POM_FULFILMENT=2 OR POM_FULFILMENT=3 OR POM_FULFILMENT=4)  " _
            '                        & "AND (ID_B_GL_CODE='20709000000' OR ID_B_GL_CODE='70004090000'  " _
            '                        & "OR ID_B_GL_CODE='70007041000' OR ID_B_GL_CODE='70007043000'  " _
            '                        & "OR ID_B_GL_CODE='70007044000' OR ID_B_GL_CODE='70007046000'  " _
            '                        & "OR ID_B_GL_CODE='70007047000') " _
            '                        & "AND POM_B_COY_ID = '" & Session("CompanyID") & "' " _
            '                        & "AND IM_B_COY_ID = '" & Session("CompanyID") & "' " _
            '                        & "AND (INVOICE_MSTR.IM_FM_APPROVED_DATE >='" & strBeginDate & "'  " _
            '                        & "AND INVOICE_MSTR.IM_FM_APPROVED_DATE <='" & strEndDate & "') " _
            '                        & "AND RIGHT(CDM_DEPT_CODE,3) BETWEEN '001' AND '499' " _
            '                        & "ORDER BY ID_B_GL_CODE"


            ds = objDb.FillDs(strsql)
            strFileName = "InvDetailsReport" & "(" & strTitle & ").xls"
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
           
        End Try

    End Sub
End Class
