Imports System.Drawing
Imports AgoraLegacy
Partial Public Class DashboardSearch
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetGridProperty(dtgDashboardList)
        GenerateTab()
        If Not Page.IsPostBack Then
            ViewState("SortAscending") = "yes"
            ViewState("SortExpression") = "DM_FIXED_ROLE_ID"
            Populate()
            Bindgrid()

        End If
    End Sub
    Sub Populate()
        Dim objGlobal As New AppGlobals
        objGlobal.FillFixedRole(ddlUserRole, ConfigurationSettings.AppSettings("eProcurePath"))
    End Sub
    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objDash As New Dashboard
        Dim dsDashboard As DataSet = New DataSet
        dsDashboard = objDash.GetDashboardMatrix("list", ddlUserRole.SelectedItem.Value, "", txtDashboardName.Text)
        Dim dvDashboard As New DataView
        dvDashboard = dsDashboard.Tables(0).DefaultView
        dvDashboard.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvDashboard.Sort += " DESC"
        ViewState("intPageRecordCnt") = dsDashboard.Tables(0).Rows.Count

        If ViewState("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtgDashboardList, dvDashboard)
            dtgDashboardList.DataSource = dvDashboard
            dtgDashboardList.DataBind()
        Else
            dtgDashboardList.DataBind()
        End If
        ViewState("PageCount") = dtgDashboardList.PageCount
    End Function
    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgDashboardList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub
    Sub dtgDashboardList_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgDashboardList.PageIndexChanged
        dtgDashboardList.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub
    Private Sub dtgDashboardList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDashboardList.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgDashboardList, e)
    End Sub
    Private Sub dtgDashboardList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDashboardList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkUserRole As HyperLink
            lnkUserRole = e.Item.FindControl("lnkUserRole")
            lnkUserRole.Text = dv("DM_FIXED_ROLE_ID")
            lnkUserRole.NavigateUrl = dDispatcher.direct("Global", "DashboardMatrix.aspx", "role='" & lnkUserRole.Text & "'")
        End If
    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Bindgrid()
    End Sub
    Sub GenerateTab()
        Session("Dashboard_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Global", "DashboardMaster.aspx") & """><span>Dashboard Master</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Global", "DashboardMatrix.aspx") & """><span>Dashboard Matrix</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                                 "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Global", "DashboardSearch.aspx") & """><span>Dashboard Listing</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                               "</ul><div></div></div>"
    End Sub
End Class