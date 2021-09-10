Imports eProcure.Component
Imports AppCommon

Public Class ReportFormat13
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
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents optCom As System.Web.UI.WebControls.RadioButton
    Protected WithEvents optSelectOne As System.Web.UI.WebControls.RadioButton
    Protected WithEvents cboCompany As System.Web.UI.WebControls.DropDownList

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
        If Not IsPostBack Then
            BindData()
            optCom.Checked = True
            cboCompany.Enabled = False
        End If

        strType = Me.Request.QueryString("type")


        Select Case strType

            Case "MTHEXPACTBUDG"
                lblHeader.Text = "Monthly Opex Expenditure Report (Actual VS Budget) By Department"
            Case "MTHOPEX"
                lblHeader.Text = "Monthly Opex Department Summary Expenditure Report"
        End Select

        If Not (Page.IsPostBack) Then

            Dim ii_ddl, ii_ddl2, jj_ddl As Integer
            ii_ddl2 = 0
            jj_ddl = Year(Date.Now)
            For ii_ddl = 2002 To jj_ddl

                cboYear.Items.Insert(ii_ddl2, New ListItem(ii_ddl))
                ii_ddl2 = ii_ddl2 + 1
            Next
            Dim lstItem As New ListItem
            lstItem.Value = ""
            lstItem.Text = "---Select---"
            cboYear.Items.Insert(0, lstItem)

            MonthName(1, True)
            Dim i As Integer

            For i = 1 To 12
                Dim lstItemMonth As New ListItem
                lstItemMonth.Value = i
                cboMonth.Items.Add(New ListItem(MonthName(i, False), i))

            Next

            lstItem.Value = ""
            lstItem.Text = "---Select---"
            cboMonth.Items.Insert(0, lstItem)

        End If
        lnkBack.NavigateUrl = "ReportSelection.aspx?pageid=" & strPageId
    End Sub


    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim crParam As New Hashtable
        crParam.Add("userIDParam", Session("UserID"))

        Dim month As Integer = cboMonth.SelectedItem.Value
        Dim year As Integer = cboYear.SelectedItem.Value

        crParam.Add("monthParam", month)
        crParam.Add("yearParam", year)


        If optCom.Checked Then
            crParam.Add("VendorCompanyIDParam", "All")
        Else
            crParam.Add("VendorCompanyIDParam", cboCompany.SelectedValue)
        End If

        Select Case strType
            Case "MTHEXPACTBUDG"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "monthlyNonCapexExpenditureActualVsBudget.rpt", crParam)
            Case "MTHOPEX"
                crParam.Add("buyercompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "monthlyNonCapexDepartmentalSummaryExpenditure.rpt", crParam)
        End Select

    End Sub
    Public Function BindData()
        Dim dvcustom As DataSet
        Dim objreport As New Report


        dvcustom = objreport.DisplayVendor(True)
        If Not dvcustom Is Nothing Then
            Dim cbolist As New ListItem
            Common.FillDdl(cboCompany, "CV_S_COY_ID", "CV_S_COY_ID", dvcustom)
            cbolist.Value = ""
            cbolist.Text = "---Select---"
            cboCompany.Items.Insert(0, cbolist)
        End If
        objreport = Nothing

    End Function

    Private Sub optCom_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optCom.CheckedChanged
        cboCompany.Enabled = False
        cboCompany.SelectedIndex = -1
    End Sub


    Private Sub optSelectOne_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optSelectOne.CheckedChanged
        cboCompany.Enabled = True
    End Sub
End Class

