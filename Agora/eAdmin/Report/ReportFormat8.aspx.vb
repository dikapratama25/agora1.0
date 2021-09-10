Imports AppCommon
Imports eProcure.Component

Public Class ReportFormat8
    Inherits AppCommon.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents cboVendorCompy As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ValVendorCompy As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
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

        If Not IsPostBack Then
            viewstate("type") = Request.QueryString("type")
            BindData()

            Select Case UCase(viewstate("type"))
                Case "AUDIT"
                    lblHeader.Text = "Audit Trial Product Activity Log Report"
            End Select
        End If

        lnkBack.NavigateUrl = dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId)
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim crParam As New Hashtable

        crParam.Add("userIDParam", Session("UserID"))
        crParam.Add("companyIDParam", cboVendorCompy.SelectedValue)

        Select Case UCase(viewstate("type"))
            Case Else
                ReportViewer.popCrystalReport(Me, "AuditTrailProductActivityLog.rpt", crParam)
        End Select
    End Sub

    Public Function BindData()
        'Dim dvcustom As DataSet
        'Dim objreport As New Report

        'dvcustom = objreport.DisplayVendor(False)

        'If Not dvcustom Is Nothing Then
        '    Dim cbolist As New ListItem

        '    Common.FillDdl(cboVendorCompy, "CV_S_COY_ID", "CV_S_COY_ID", dvcustom)
        '    cbolist.Value = ""
        '    cbolist.Text = "---Select---"
        '    cboVendorCompy.Items.Insert(0, cbolist)
        'End If
        'objreport = Nothing
        Dim cbolist As New ListItem
        cbolist.Value = ""
        cbolist.Text = "---Select---"

        Common.FillDefault(cboVendorCompy, "COMPANY_MSTR", "CM_COY_NAME", "CM_COY_ID", , " CM_COY_TYPE IN ('VENDOR','BOTH') AND CM_DELETED<> 'Y'")
        cboVendorCompy.Items.Insert(0, cbolist)
    End Function
End Class
