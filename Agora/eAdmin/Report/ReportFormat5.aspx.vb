Imports AppCommon
Imports eProcure.Component

Public Class ReportFormat5
    Inherits AppCommon.AppBaseClass

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
            Dim objGlobal As New AppGlobals
            objGlobal.FillCodeTable(Me.cboCurrency, CodeTable.Currency)

            Select Case UCase(viewstate("type"))
                Case "VALUES"
                    lblHeader.Text = "Top 50 SKU Values Summary Report"
                Case "VOLUME"
                    lblHeader.Text = "Top 50 SKU Volumn Summary Report"
            End Select
        End If
        lnkBack.NavigateUrl = dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId)
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim crParam As New Hashtable

        crParam.Add("userIDParam", Session("UserID"))
        crParam.Add("CurrencyCode", cboCurrency.SelectedValue)

        Select Case UCase(viewstate("type"))
            Case "VALUES"
                ReportViewer.popCrystalReport(Me, "topSkuValues.rpt", crParam)
            Case "VOLUME"
                ReportViewer.popCrystalReport(Me, "topSkuVolume.rpt", crParam)
        End Select

    End Sub
End Class
