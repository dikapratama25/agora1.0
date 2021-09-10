Imports AppCommon
Imports eProcure.Component

Public Class ReportFormat6
    Inherits AppCommon.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents cboMonthFrom As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents cboYearFrom As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rfvMonthFrom As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfvYearFrom As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents cboMonthTo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rfvMonthTo As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents cboYearTo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rfvYearTo As System.Web.UI.WebControls.RequiredFieldValidator

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
        If Not IsPostBack Then
            viewstate("type") = Request.QueryString("type")

            Select Case viewstate("type")
                Case "InvSumm"
                    lblHeader.Text = "Invoice Chargeable Summary Report For Vendor"
            End Select

            bindDDL()
        End If
        lnkBack.NavigateUrl = Dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId)
    End Sub

    Private Sub bindDDL()
        Dim ii_ddl, ii_ddl2, jj_ddl As Integer
        Dim lstItem As New ListItem

        lstItem.Value = ""
        lstItem.Text = "---Select---"
        cboYearFrom.Items.Insert(0, lstItem)
        cboYearTo.Items.Insert(0, lstItem)
        cboMonthFrom.Items.Insert(0, lstItem)
        cboMonthTo.Items.Insert(0, lstItem)

        ii_ddl2 = 1
        jj_ddl = 2020
        For ii_ddl = 1990 To jj_ddl
            cboYearFrom.Items.Insert(ii_ddl2, New ListItem(ii_ddl))
            cboYearTo.Items.Insert(ii_ddl2, New ListItem(ii_ddl))
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
            cboMonthFrom.Items.Insert(ii_ddl, lst)
            cboMonthTo.Items.Insert(ii_ddl, lst)
        Next
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim crParam As New Hashtable

        crParam.Add("userIDParam", Session("UserID"))
        crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
        crParam.Add("MonthFromParam", cboMonthFrom.SelectedValue)
        crParam.Add("MonthToParam", cboMonthTo.SelectedValue)
        crParam.Add("YearFromParam", cboYearFrom.SelectedValue)
        crParam.Add("YearToParam", cboYearTo.SelectedValue)

        Select Case UCase(viewstate("type"))
            Case "INVSUMM"
                ReportViewer.popCrystalReport(Me, "allcompInvoiceSummaryReport2.rpt", crParam)
        End Select

    End Sub

End Class
