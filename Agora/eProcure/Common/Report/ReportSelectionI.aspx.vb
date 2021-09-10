'Imports Wheel.Components

Imports AgoraLegacy
Imports eProcure.Component
Public Class ReportSelectionI
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim blnFM, blnFO, blnIPPAdmin, blnIPPO, blnIPPOF, blnBillingAO As Boolean 'Mod by Jules 2015-Feb-03 for IPP GST Stage 2A
    'Zulham 07/08/2017 - IPP Stage 3
    Dim blnBillingFT, blnBillingST, blnBillingSO As Boolean

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lnkOPR As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPR As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPO As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkRFQ As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkOPO As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkOGRN As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkMthConsumption As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkInvCycle As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkVenList As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkOPOSEH As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkDirectCharge As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkMthInv As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkMthPO As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LNKMthCon As System.Web.UI.WebControls.HyperLink
    Protected WithEvents back As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lnkPMED As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkPME As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkdisplay As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents dtgReport As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lnkReportName As System.Web.UI.WebControls.HyperLink
    Dim objReport As New Report
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
        blnPaging = False
        SetGridProperty(dtgReport)

        dtgReport.CurrentPageIndex = 0
        If Not Page.IsPostBack Then
            Dim objUserRole As New UserRoles
            'strFixRole = checkUserFixedRole(objUserRole.get_UserFixedRole()
            Dim strFM, strFO As String
            Dim objUsers As New Users
            strFM = System.Enum.GetName(GetType(FixedRole), FixedRole.Finance_Manager)
            strFM = "'" & Replace(strFM, "_", " ") & "'"
            'Michelle (5/6/2012) - Issue 1640)
            strFO = System.Enum.GetName(GetType(FixedRole), FixedRole.Finance_Officer)
            strFO = "'" & Replace(strFO, "_", " ") & "'"
            'Zulham Apr 22, 2013
            Dim Roles As New ArrayList : Roles = Session("MixUserRole")
            For Each str As String In Roles
                If str = "IPP Administrator" Then
                    blnIPPAdmin = objUsers.checkUserFixedRole("'" & str & "'")
                ElseIf str = "IPP Officer" Then
                    blnIPPO = objUsers.checkUserFixedRole("'" & str & "'")
                ElseIf str = "IPP Officer(F)" Then
                    blnIPPOF = objUsers.checkUserFixedRole("'" & str & "'")
                    'Zulham 07/08/2017 - IPP Stage 3
                ElseIf str = "Billing Officer" Then
                    blnBillingST = objUsers.checkUserFixedRole("'" & str & "'")
                ElseIf str = "Billing Approving Officer" Then
                    blnBillingSO = objUsers.checkUserFixedRole("'" & str & "'")
                ElseIf str = "Billing Officer(F)" Then
                    blnBillingFT = objUsers.checkUserFixedRole("'" & str & "'")
                    '''
                    'Jules 2015-Feb-03 for IPP GST Stage 2A
                ElseIf str.Contains("Billing Approving Officer") Then
                    blnBillingAO = objUsers.checkUserFixedRole("'" & str & "'")
                End If
            Next
            'End

            blnFM = objUsers.checkUserFixedRole(strFM)
            blnFO = objUsers.checkUserFixedRole(strFO)

        End If
        Bindgrid(0)
    End Sub
    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objReport As New Report
        Dim ds As DataSet = New DataSet
        Dim dvViewSample As DataView
        Dim strReport As String

        If blnFM = True Or blnFO = True Then
            strReport = "('IPPD0002 - Daily Online GL Entries Listing'," & _
                        "'IPPD0004 - Summary Invoices Pending Approval Report'," & _
                        "'IPPD0005 - Summary Invoices Released Report'," & _
                        "'IPPA0014 - Staff Claim By GL Account Code'," & _
                        "'IPPA0016 - Payment To Top 100 Vendor'," & _
                        "'IPPS0017 - Paid Staff Claim Report'," & _
                        "'Payment Advice','Debit Note','Debit Advice', 'Tax Invoice')"
        ElseIf blnIPPAdmin = True Then
            strReport = "('IPPA0012 - List Of Active Vendors Report'," & _
                        "'IPPA0013 - List Of Inactive Vendors Report')"

        ElseIf blnIPPOF = True And blnIPPAdmin = False Then
            strReport = "('IPPA0014 - Staff Claim By GL Account Code','Payment Advice'," & _
                        "'IPPS0017 - Paid Staff Claim Report')"


        ElseIf blnIPPO = True And blnIPPAdmin = False Then
            strReport = "('IPPA0015 - Daily Outstanding Invoice Ageing Report By Source Department')"
            'Zulham 07/08/2017 - IPP Stage 3
            'ElseIf (blnBillingFT Or blnBillingST Or blnBillingSO) And blnIPPAdmin = False Then
            'strReport = "('Billing Debit Note','Billing Credit Note','Billing Debit Advice','Billing Credit Advice')"
        End If

        If strReport = "" Then
            ds = objReport.getReportType(Session("CompanyID"), "", strReport)
        Else
            ds = objReport.getReportType(Session("CompanyID"), "I", strReport)
        End If

            ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

            dvViewSample = ds.Tables(0).DefaultView

            intPageRecordCnt = ds.Tables(0).Rows.Count

            If intPageRecordCnt > 0 Then
                dtgReport.DataSource = dvViewSample
                dtgReport.DataBind()
            Else
                dtgReport.DataBind()
                Common.NetMsgbox(Me, MsgNoRecord)
            End If
            objReport = Nothing

    End Function
    Public Sub dtgReport_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgReport.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0, True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(dtgReport.CurrentPageIndex = 0, True)
    End Sub

    Private Sub dtgReport_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgReport.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkReportName As HyperLink
            lnkReportName = e.Item.FindControl("lnkReportName")
            lnkReportName.Text = dv("rm_report_name")
            'Michelle (5/6/2012) - Issue 1640
            'If blnFM = True Then
            If (blnFM = True And (lnkReportName.Text = "IPPD0002 - Daily Online GL Entries Listing" Or lnkReportName.Text = "IPPD0004 - Summary Invoices Pending Approval Report" Or lnkReportName.Text = "IPPD0005 - Summary Invoices Released Report" Or lnkReportName.Text = "IPPA0014 - Staff Claim By GL Account Code" Or lnkReportName.Text = "IPPA0016 - Payment To Top 100 Vendor" Or lnkReportName.Text = "Payment Advice" Or lnkReportName.Text = "Debit Note" Or lnkReportName.Text = "Debit Advice" Or lnkReportName.Text = "Tax Invoice" Or lnkReportName.Text = "IPPS0017 - Paid Staff Claim Report")) Or _
            (blnFO = True And (lnkReportName.Text = "IPPD0002 - Daily Online GL Entries Listing" Or lnkReportName.Text = "IPPD0004 - Summary Invoices Pending Approval Report" Or lnkReportName.Text = "IPPD0005 - Summary Invoices Released Report" Or lnkReportName.Text = "IPPA0014 - Staff Claim By GL Account Code" Or lnkReportName.Text = "IPPA0016 - Payment To Top 100 Vendor" Or lnkReportName.Text = "Payment Advice" Or lnkReportName.Text = "Debit Note" Or lnkReportName.Text = "Debit Advice" Or lnkReportName.Text = "Tax Invoice" Or lnkReportName.Text = "IPPS0017 - Paid Staff Claim Report")) Then
                lnkReportName.NavigateUrl = dDispatcher.direct("Report", Mid(dv("RM_REPORT_URL"), 1, InStr(dv("RM_REPORT_URL"), "?") - 1), Mid(dv("RM_REPORT_URL"), InStr(dv("RM_REPORT_URL"), "?") + 1) & "&rptname=" & dv("rm_report_name")) 'objReport.getReportUrl(dv("rm_report_index"))
            ElseIf blnIPPAdmin = True And (lnkReportName.Text = "IPPA0012 - List Of Active Vendors Report" Or lnkReportName.Text = "IPPA0013 - List Of Inactive Vendors Report") Then
                lnkReportName.NavigateUrl = dDispatcher.direct("Report", Mid(dv("RM_REPORT_URL"), 1, InStr(dv("RM_REPORT_URL"), "?") - 1), Mid(dv("RM_REPORT_URL"), InStr(dv("RM_REPORT_URL"), "?") + 1) & "&rptname=" & dv("rm_report_name")) 'objReport.getReportUrl(dv("rm_report_index"))
            ElseIf blnIPPOF = True And blnIPPAdmin = False And (lnkReportName.Text = "IPPA0014 - Staff Claim By GL Account Code" Or lnkReportName.Text = "Payment Advice" Or lnkReportName.Text = "IPPS0017 - Paid Staff Claim Report") Then
                lnkReportName.NavigateUrl = dDispatcher.direct("Report", Mid(dv("RM_REPORT_URL"), 1, InStr(dv("RM_REPORT_URL"), "?") - 1), Mid(dv("RM_REPORT_URL"), InStr(dv("RM_REPORT_URL"), "?") + 1) & "&rptname=" & dv("rm_report_name")) 'objReport.getReportUrl(dv("rm_report_index"))
            ElseIf blnIPPO = True And blnIPPAdmin = False And (lnkReportName.Text = "IPPA0015 - Daily Outstanding Invoice Ageing Report By Source Department") Then
                lnkReportName.NavigateUrl = dDispatcher.direct("Report", Mid(dv("RM_REPORT_URL"), 1, InStr(dv("RM_REPORT_URL"), "?") - 1), Mid(dv("RM_REPORT_URL"), InStr(dv("RM_REPORT_URL"), "?") + 1) & "&rptname=" & dv("rm_report_name")) 'objReport.getReportUrl(dv("rm_report_index"))
                'Jules 2015-Feb-03 for IPP GST Stage 2A
            ElseIf blnBillingAO = True And blnIPPAdmin = False And (lnkReportName.Text = "Daily Billing Summary Report" Or lnkReportName.Text = "Billing Pending Approval Report" Or lnkReportName.Text = "Tax Invoice") Then
                lnkReportName.NavigateUrl = dDispatcher.direct("Report", Mid(dv("RM_REPORT_URL"), 1, InStr(dv("RM_REPORT_URL"), "?") - 1), Mid(dv("RM_REPORT_URL"), InStr(dv("RM_REPORT_URL"), "?") + 1) & "&rptname=" & dv("rm_report_name")) 'objReport.getReportUrl(dv("rm_report_index"))
                'Zulham 08/08/2017 - IPP Stage 3
            ElseIf (blnBillingAO = True Or blnBillingFT Or blnBillingST Or blnBillingSO) And blnIPPAdmin = False And (lnkReportName.Text = "Billing Credit Note" Or lnkReportName.Text = "Billing Debit Note" Or lnkReportName.Text = "Billing Credit Advice" Or lnkReportName.Text = "Billing Debit Advice" Or lnkReportName.Text = "Billing Tax Invoice") Then
                lnkReportName.NavigateUrl = dDispatcher.direct("Report", Mid(dv("RM_REPORT_URL"), 1, InStr(dv("RM_REPORT_URL"), "?") - 1), Mid(dv("RM_REPORT_URL"), InStr(dv("RM_REPORT_URL"), "?") + 1) & "&rptname=" & dv("rm_report_name")) 'objReport.getReportUrl(dv("rm_report_index"))
                '''
            Else
                lnkReportName.NavigateUrl = "#"
            End If

            'lnkReportName.Text = dv("rm_report_name")
            'Mod by Jules 2015-Feb-03 for IPP GST Stage 2A
            'Zulham c08/08/2017 - IPP Stage 3
            'added 3 more parameters for blnBillingFT, blnBillingST & blnBillingSO
            lnkReportName.Attributes.Add("onclick", "chkRole('" & blnFM & "','" & blnFO & "','" & blnIPPAdmin & "','" & blnIPPO & "','" & blnIPPOF & "','" & blnBillingAO & "','" & lnkReportName.Text & "','" & blnBillingFT & "','" & blnBillingST & "','" & blnBillingSO & "');")
            'cmdDelete.Attributes.Add("onclick", "return confirm('" & MsgForDeleteButton & "');")

        End If
    End Sub

    'Private Sub lnkReportName_Load(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.HyperLink) Handles lnkReportName.Load
    '    Dim strscript As New System.Text.StringBuilder
    '    strscript.Append("<script language=""javascript"">")
    '    strscript.Append("chkRole(strFixRole);")
    '    strscript.Append("</script>")
    '    RegisterStartupScript("script3", strscript.ToString())

    'End Sub

End Class
