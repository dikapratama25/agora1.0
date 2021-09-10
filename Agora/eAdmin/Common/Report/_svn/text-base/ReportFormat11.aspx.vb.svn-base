Imports AppCommon
Imports eProcure.Component

Public Class ReportFormat11
    Inherits AppCommon.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents optPr_Po_No As System.Web.UI.WebControls.RadioButton
    Protected WithEvents optPr_status As System.Web.UI.WebControls.RadioButton
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents cboCompy As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ValCompy As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents txtSDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents ValSDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents txtEndDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents ValEDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
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
                Case "PR"
                    lblHeader.Text = "Purchase Requisition Detail Log"
            End Select
        End If
        lnkBack.NavigateUrl = dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId)
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim crParam As New Hashtable
        Dim startdate As String = Format(CDate(txtSDate.Text), "MM/dd/yyyy")
        Dim Enddate As String = Format(CDate(txtEndDate.Text), "MM/dd/yyyy")

        crParam.Add("userIDParam", Session("UserID"))
        crParam.Add("companyIDParam", cboCompy.SelectedValue)
        crParam.Add("dateFromParam", startdate)
        crParam.Add("dateToParam", Enddate)

        Select Case UCase(viewstate("type"))
            Case "PR"
                If optPr_Po_No.Checked = True Then
                    ReportViewer.popCrystalReport(Me, "PurchaseRequisitionDetailLogByPRPO.rpt", crParam)
                Else
                    ReportViewer.popCrystalReport(Me, "PurchaseRequisitionDetailLogByPRStatus.rpt", crParam)
                End If
        End Select
    End Sub

    Public Function BindData()
        Dim cbolist As New ListItem
        cbolist.Value = ""
        cbolist.Text = "---Select---"

        Common.FillDefault(cboCompy, "COMPANY_MSTR", "CM_COY_NAME", "CM_COY_ID", , " CM_COY_TYPE IN('BUYER','BOTH') AND CM_DELETED<> 'Y'")
        cboCompy.Items.Insert(0, cbolist)
    End Function
End Class
