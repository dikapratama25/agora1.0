Imports eProcure.Component
Public Class ReportFormat4
    Inherits AppCommon.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents optAppVenList As System.Web.UI.WebControls.RadioButton
    Protected WithEvents optNAppVenList As System.Web.UI.WebControls.RadioButton
    Protected WithEvents oplList As System.Web.UI.WebControls.RadioButtonList
    Dim strType As String
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents label2 As System.Web.UI.WebControls.Label


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
        'strType = Me.Request.QueryString("type")


        'Select Case strType

        '    Case "VenList"
        '        lblHeader.Text = "VENDOR LISTING"

        'End Select
        lnkBack.NavigateUrl = dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId)

    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        'Me.Response.Redirect("ReportSelection.aspx?pageid=" & strPageId & "&list=" & oplList.SelectedValue)

        Dim crParam As New Hashtable
        'crParam.Add("companyIDParam", Session("CompanyID"))
        crParam.Add("BCOYID", Session("CompanyIdToken"))
        crParam.Add("userIDParam", Session("UserID"))

        'crParam.Add("BCOYID", "MBB")
        'crParam.Add("userIDParam", "MOO")
        'crParam.Add("monthParam", cboMonth.SelectedItem.Value)
        'crParam.Add("yearParam", cboYear.SelectedItem.Value)

        '********************************************
        If oplList.SelectedValue = "A" Then
            eProcure.Component.ReportViewer.popCrystalReport(Me, "SupplierInApprovedList2.rpt", crParam)
        Else
            eProcure.Component.ReportViewer.popCrystalReport(Me, "SupplierNotInApprovedList2.rpt", crParam)
        End If
        '********************************************


        'Select Case strType
        '    Case "VenList"
        '        Wheel.Components.ReportViewer.popCrystalReport(Me, "SupplierNotInApprovedList.rpt", crParam)
        'End Select

        
    End Sub
End Class
