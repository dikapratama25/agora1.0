Imports AgoraLegacy
Imports eProcure.Component

Public Class ReportFormat9
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents cboCompanyName As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ValCompanyName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents cboUserName As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ValUserName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents txtSDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents ValSDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents txtEndDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents ValEDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
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

        If Not IsPostBack Then
            viewstate("type") = Request.QueryString("type")
            BindData()

            Select Case UCase(viewstate("type"))
                Case "DETAIL"
                    lblHeader.Text = "User Detail Modification Audit Trail For User"
                Case "INFO"
                    lblHeader.Text = "User Information Modification Log"
            End Select
        End If
        lnkBack.NavigateUrl = "ReportSelection.aspx?pageid=" & strPageId
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim strReportName As String = ""
        Dim reportURL As String
        'Dim crParam As New Hashtable
        Dim startdate As String = Format(CDate(txtSDate.Text), "MM/dd/yyyy")
        Dim Enddate As String = Format(CDate(txtEndDate.Text), "MM/dd/yyyy")
        Dim strParam As String = ""
        'crParam.Add("userIDParam", Session("UserID"))
        'crParam.Add("companyIDParam", cboCompanyName.SelectedValue)
        'crParam.Add("dateFromParam", startdate)
        'crParam.Add("dateToParam", Enddate)

        Select Case UCase(ViewState("type"))
            Case "DETAIL"
                'crParam.Add("companyUserParam", cboUserName.SelectedValue)
                strReportName = "userDetailModificationAudit.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&companyIDParam=" & cboCompanyName.SelectedValue & "&dateFromParam=" & startdate & "&dateToParam=" & Enddate & "&companyUserParam=" & cboUserName.SelectedValue

            Case "INFO"
                strReportName = "UserInforModificationLog.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&companyIDParam=" & cboCompanyName.SelectedValue & "&dateFromParam=" & startdate & "&dateToParam=" & Enddate
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
        Me.ClientScript.RegisterStartupScript(GetType(ReportFormat9), rndKey.Next.ToString, jscript)
    End Sub

    'Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
    '    Dim crParam As New Hashtable
    '    Dim startdate As String = Format(CDate(txtSDate.Text), "MM/dd/yyyy")
    '    Dim Enddate As String = Format(CDate(txtEndDate.Text), "MM/dd/yyyy")

    '    crParam.Add("userIDParam", Session("UserID"))
    '    crParam.Add("companyIDParam", cboCompanyName.SelectedValue)
    '    crParam.Add("dateFromParam", startdate)
    '    crParam.Add("dateToParam", Enddate)

    '    Select Case UCase(viewstate("type"))
    '        Case "DETAIL"
    '            crParam.Add("companyUserParam", cboUserName.SelectedValue)
    '            ReportViewer.popCrystalReport(Me, "userDetailModificationAudit.rpt", crParam)
    '        Case "INFO"
    '            ReportViewer.popCrystalReport(Me, "UserInforModificationLog.rpt", crParam)
    '    End Select
    'End Sub

    Public Function BindData()
        Dim cbolist As New ListItem
        cbolist.Value = ""
        cbolist.Text = "---Select---"

        Common.FillDefault(cboCompanyName, "COMPANY_MSTR", "CM_COY_NAME", "CM_COY_ID", , "CM_DELETED<>'Y'")
        cboCompanyName.Items.Insert(0, cbolist)

        cboUserName.Items.Clear()
        cboUserName.Items.Insert(0, cbolist)
    End Function

    Private Sub cboCompanyName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCompanyName.SelectedIndexChanged
        Dim cbolist As New ListItem
        cbolist.Value = ""
        cbolist.Text = "---Select---"

        cboUserName.Items.Clear()

        If cboCompanyName.SelectedIndex > 0 Then
            Common.FillDefault(cboUserName, "USER_MSTR", "UM_USER_NAME", "UM_USER_ID", , "UM_DELETED<>'Y' AND UM_COY_ID='" & cboCompanyName.SelectedValue & "'")
        End If

        cboUserName.Items.Insert(0, cbolist)
    End Sub
End Class