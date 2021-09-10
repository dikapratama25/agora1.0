Imports AgoraLegacy
Imports eProcure.Component

Public Class ReportFormat8
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    'Protected WithEvents label2 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    'Protected WithEvents cboVendorCompy As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents ValVendorCompy As System.Web.UI.WebControls.RequiredFieldValidator
    'Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    'Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    'Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    'Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton

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

        If Not IsPostBack Then
            viewstate("type") = Request.QueryString("type")
            BindData()

            Select Case UCase(viewstate("type"))
                Case "AUDIT"
                    lblHeader.Text = "Audit Trial Product Activity Log Report"
            End Select
        End If

        lnkBack.NavigateUrl = "ReportSelection.aspx?pageid=" & strPageId
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim strReportName As String = ""
        Dim reportURL As String
        Dim strParam As String = ""
        'Dim crParam As New Hashtable

        'crParam.Add("userIDParam", Session("UserID"))
        'crParam.Add("companyIDParam", cboVendorCompy.SelectedValue)

        'Select Case UCase(viewstate("type"))
        '    Case Else
        strReportName = "AuditTrailProductActivityLog.rpt"
        'End Select

        'Session("Param") = crParam
        strParam = "&userIDParam=" & Session("UserID") & "&companyIDParam=" & cboVendorCompy.SelectedValue
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
        Me.ClientScript.RegisterStartupScript(GetType(ReportFormat8), rndKey.Next.ToString, jscript)
    End Sub

    'Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
    '    Dim crParam As New Hashtable

    '    crParam.Add("userIDParam", Session("UserID"))
    '    crParam.Add("companyIDParam", cboVendorCompy.SelectedValue)

    '    Select Case UCase(viewstate("type"))
    '        Case Else
    '            ReportViewer.popCrystalReport(Me, "AuditTrailProductActivityLog.rpt", crParam)
    '    End Select
    'End Sub

    Public Function BindData()
        Dim dvcustom As DataSet
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
