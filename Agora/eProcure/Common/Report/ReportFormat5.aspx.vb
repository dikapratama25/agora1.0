Imports AgoraLegacy
Imports eProcure.Component

Public Class ReportFormat5
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents cboCurrency As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ValCur As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents cboReportType As System.Web.UI.WebControls.DropDownList
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
            viewstate("type") = Request.QueryString("type")
            Dim objGlobal As New AppGlobals
            objGlobal.FillCodeTable(Me.cboCurrency, CodeTable.Currency)

            Select Case UCase(viewstate("type"))
                Case "VALUES"
                    lblHeader.Text = "Top 50 SKU Values Summary Report"
                Case "VOLUME"
                    lblHeader.Text = "Top 50 SKU Volumn Summary Report"
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
        'crParam.Add("CurrencyCode", cboCurrency.SelectedValue)

        Select Case UCase(ViewState("type"))
            Case "VALUES"
                strReportName = "topSkuValues.rpt"
            Case "VOLUME"
                strReportName = "topSkuVolume.rpt"
        End Select

        'Session("Param") = crParam
        strParam = "&userIDParam=" & Session("UserID") & "&CurrencyCode=" & cboCurrency.SelectedValue
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
        Me.ClientScript.RegisterStartupScript(GetType(ReportFormat5), rndKey.Next.ToString, jscript)
    End Sub
    'Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
    '    Dim crParam As New Hashtable

    '    crParam.Add("userIDParam", Session("UserID"))
    '    crParam.Add("CurrencyCode", cboCurrency.SelectedValue)

    '    Select Case UCase(viewstate("type"))
    '        Case "VALUES"
    '            ReportViewer.popCrystalReport(Me, "topSkuValues.rpt", crParam)
    '        Case "VOLUME"
    '            ReportViewer.popCrystalReport(Me, "topSkuVolumn.rpt", crParam)
    '    End Select

    'End Sub
End Class
