'Imports Wheel.Components
Imports AgoraLegacy
Imports eProcure.Component
Public Class ReportFormat4
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    'Protected WithEvents oplList As System.Web.UI.WebControls.RadioButtonList
    'Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    'Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    'Protected WithEvents label2 As System.Web.UI.WebControls.Label
    Protected WithEvents optAppVenList As System.Web.UI.WebControls.RadioButton
    Protected WithEvents optNAppVenList As System.Web.UI.WebControls.RadioButton
    Dim strType As String

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
        'strType = Me.Request.QueryString("type")


        'Select Case strType

        '    Case "VenList"
        '        lblHeader.Text = "VENDOR LISTING"

        'End Select
        lnkBack.NavigateUrl = "ReportSelection.aspx?pageid=" & strPageId

    End Sub
    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim strReportName As String = ""
        Dim reportURL As String
        Dim strParam As String = ""
        'Dim crParam As New Hashtable

        'crParam.Add("BCOYID", Session("CompanyID"))
        'crParam.Add("userIDParam", Session("UserID"))

        If oplList.SelectedValue = "A" Then
            strReportName = "SupplierInApprovedList2.rpt"
        Else
            strReportName = "SupplierNotInApprovedList2.rpt"
        End If

        'Session("Param") = crParam
        strParam = "&userIDParam=" & Session("UserID") & "&BCOYID=" & Session("CompanyID")
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
        Me.ClientScript.RegisterStartupScript(GetType(ReportFormat4), rndKey.Next.ToString, jscript)
    End Sub
    'Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
    '    'Me.Response.Redirect("ReportSelection.aspx?pageid=" & strPageId & "&list=" & oplList.SelectedValue)

    '    Dim crParam As New Hashtable
    '    'crParam.Add("companyIDParam", Session("CompanyID"))
    '    crParam.Add("BCOYID", Session("CompanyID"))
    '    crParam.Add("userIDParam", Session("UserID"))

    '    'crParam.Add("BCOYID", "MBB")
    '    'crParam.Add("userIDParam", "MOO")
    '    'crParam.Add("monthParam", cboMonth.SelectedItem.Value)
    '    'crParam.Add("yearParam", cboYear.SelectedItem.Value)

    '    '********************************************
    '    If oplList.SelectedValue = "A" Then
    '        ReportViewer.popCrystalReport(Me, "SupplierInApprovedList2.rpt", crParam)
    '    Else
    '        ReportViewer.popCrystalReport(Me, "SupplierNotInApprovedList2.rpt", crParam)
    '    End If
    '    '********************************************


    '    'Select Case strType
    '    '    Case "VenList"
    '    '        Wheel.Components.ReportViewer.popCrystalReport(Me, "SupplierNotInApprovedList.rpt", crParam)
    '    'End Select


    'End Sub
End Class
