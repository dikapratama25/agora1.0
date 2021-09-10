Imports AgoraLegacy
Imports eProcure.Component
Public Class ReportSelectionT
    Inherits AgoraLegacy.AppBaseClass

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
        Bindgrid(0)
    End Sub
    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objReport As New Report
        Dim ds As DataSet = New DataSet
        Dim dvViewSample As DataView
        Dim i As Integer

        ds = objReport.getReportType(Session("CompanyID"))
        'Yap Comment ds = objReport.getReportType(Session("CompanyID"), "T")
        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count


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
            lnkReportName.NavigateUrl = dv("RM_REPORT_URL")
            lnkReportName.Text = dv("rm_report_name")

        End If
    End Sub
End Class
