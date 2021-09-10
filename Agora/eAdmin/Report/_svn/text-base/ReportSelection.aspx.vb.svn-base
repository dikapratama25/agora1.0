Imports eProcure.Component
Imports AppCommon

Public Class ReportSelection
    Inherits AppCommon.AppBaseClass

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
    Dim dDispatcher As New Dispatcher.dispatcher
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Session("CompanyId") = "demo"
        MyBase.Page_Load(sender, e)
        blnPaging = False
        SetGridProperty(dtgReport)
        'Put user code to initialize the page here
        'Session("CompanyId") = "demo"
        'Session("UserId") = "moofh"

        dtgReport.CurrentPageIndex = 0
        Bindgrid(0)


        'Dim index As Integer = CInt(dtgReport.DataKeyField)
        'lnkReportName.NavigateUrl = objReport.getReportUrl(index)


        ' displayReportType()
        'lnkPO.NavigateUrl = "ReportFormat1.aspx?type=PO&pageid=" & strPageId
        'lnkPR.NavigateUrl = "ReportFormat1.aspx?type=PR&pageid=" & strPageId
        'lnkRFQ.NavigateUrl = "ReportFormat1.aspx?type=RFQ&pageid=" & strPageId
        'lnkOPO.NavigateUrl = "ReportFormat2.aspx?type=OPO&pageid=" & strPageId
        'lnkOPR.NavigateUrl = "ReportFormat2.aspx?type=OPR&pageid=" & strPageId
        'lnkOGRN.NavigateUrl = "ReportFormat2.aspx?type=OGRN&pageid=" & strPageId
        'lnkMthConsumption.NavigateUrl = "ReportFormat3.aspx?type=MthCon&pageid=" & strPageId
        'lnkInvCycle.NavigateUrl = "ReportFormat2.aspx?type=DRP&pageid=" & strPageId
        'lnkVenList.NavigateUrl = "ReportFormat4.aspx?type=VenList&pageid=" & strPageId
        'lnkOPOSEH.NavigateUrl = "ReportFormat2.aspx?type=OPOV&pageid=" & strPageId
        'lnkPME.NavigateUrl = "ReportFormat2.aspx?type=PME&pageid=" & strPageId
        'lnkPMED.NavigateUrl = "ReportFormat2.aspx?type=PMED&pageid=" & strPageId
        'lnkDirectCharge.NavigateUrl = "ReportFormat3.aspx?type=DirectChar&pageid=" & strPageId
        'lnkMthInv.NavigateUrl = "ReportFormat3.aspx?type=MthInvCum&pageid=" & strPageId
        'lnkMthPO.NavigateUrl = "ReportFormat3.aspx?type=MthPOCum&pageid=" & strPageId
        'lnkPO.NavigateUrl = objReport.getReportUrl(1) & "?type=PO&pageid=" & strPageId


        'lnkPO.NavigateUrl = objReport.getReportUrl(1)
        'lnkPR.NavigateUrl = objReport.getReportUrl(2)
        'lnkRFQ.NavigateUrl = objReport.getReportUrl(3)
        'lnkOPO.NavigateUrl = objReport.getReportUrl(4)
        'lnkOPR.NavigateUrl = objReport.getReportUrl(5)
        'lnkOGRN.NavigateUrl = objReport.getReportUrl(6)
        'lnkMthConsumption.NavigateUrl = objReport.getReportUrl(7)
        'lnkInvCycle.NavigateUrl = objReport.getReportUrl(8)
        'lnkVenList.NavigateUrl = objReport.getReportUrl(9)
        'lnkOPOSEH.NavigateUrl = objReport.getReportUrl(10)
        'lnkPME.NavigateUrl = objReport.getReportUrl(11)
        'lnkPMED.NavigateUrl = objReport.getReportUrl(12)
        'lnkDirectCharge.NavigateUrl = objReport.getReportUrl(13)
        'lnkMthInv.NavigateUrl = objReport.getReportUrl(14)
        'lnkMthPO.NavigateUrl = objReport.getReportUrl(15)




        'Response.Write(Request.QueryString("mth"))
        'Response.Write(Request.QueryString("year"))
        'Response.Write(Request.QueryString("list"))
        'Response.Write(Request.QueryString("sd"))
        'Response.Write(Request.QueryString("ed"))
        'Response.Write(Request.QueryString("ven"))


    End Sub
    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objReport As New Report
        Dim ds As DataSet = New DataSet
        Dim dvViewSample As DataView
        'Dim strReport() As String
        'Dim strAllReport As String = ""
        'Dim record As String
        Dim i As Integer

        'strReport = objReport.getReportType.Split(",")
        'For i = 0 To strReport.Length - 1
        '    strAllReport &= strReport(i) & "<br>"
        'Next

        ds = objReport.getReportType(Session("CompanyIDToken"))
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
    'Private Sub displayReportType()

    '    Dim objReport As New Report
    '    Dim strAryReport() As String
    '    Dim strUser As String = ""
    '    Dim i As Integer

    '    strAryReport = objReport.getReportType.Split(",")
    '    For i = 0 To strAryReport.Length - 1
    '        strUser &= strAryReport(i) & "<br>"
    '    Next
    '    lnkdisplay.Text = strUser
    'End Sub




    Private Sub dtgReport_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgReport.ItemDataBound
        Dim sPara As New ArrayList
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            ' Dim i As Integer

            Dim lnkReportName As HyperLink
            lnkReportName = e.Item.FindControl("lnkReportName")
            'lnkReportName.NavigateUrl = dv("RM_REPORT_URL") 'objReport.getReportUrl(dv("rm_report_index"))


            sPara = dDispatcher.splitter(dv("RM_REPORT_URL"))
            lnkReportName.NavigateUrl = dDispatcher.direct("Report", sPara(1), sPara(2))
            lnkReportName.Text = dv("rm_report_name")


            '   lnkReportName.Attributes.Add("onclick", "return PopWindow('Favs_ItemList.aspx')")


        End If
    End Sub
End Class
