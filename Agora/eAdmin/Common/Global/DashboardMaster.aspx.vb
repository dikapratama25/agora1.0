Imports System.Drawing
Imports AgoraLegacy
Partial Public Class DashboardMaster
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("PageCount") = 20
        blnPaging = False
        blnSorting = False
        SetGridProperty(dtgDashboard)
        GenerateTab()
        If Not Page.IsPostBack Then
            Bindgrid()
        End If

    End Sub
    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objDO As New Dashboard
        Dim dsDashboard As DataSet = New DataSet
        dsDashboard = objDO.GetDashboardPanel()
        Dim dvDashboard As New DataView
        dvDashboard = dsDashboard.Tables(0).DefaultView
        If dsDashboard.Tables(0).Rows.Count > 0 Then
            cmdSave.Visible = True
            dtgDashboard.DataSource = dvDashboard
            dtgDashboard.DataBind()
        Else
            cmdSave.Visible = False
            dtgDashboard.DataBind()
        End If


    End Function
    Private Sub dtgDashboard_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDashboard.ItemCreated
        Grid_ItemCreated(dtgDashboard, e)
    End Sub
    Private Sub dtgDashboard_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDashboard.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim txtPanelName As TextBox
            txtPanelName = e.Item.FindControl("txtPanelName")
            txtPanelName.Text = dv("DM_PANEL_NAME")
        End If
    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objDash As New Dashboard
        Dim dtgItem As DataGridItem
        Dim txtPanelName As TextBox
        For Each dtgItem In dtgDashboard.Items
            txtPanelName = dtgItem.FindControl("txtPanelName")
            objDash.UpdateDashboardPanel(dtgItem.Cells.Item(0).Text, txtPanelName.Text)
        Next
        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
    End Sub

    Sub GenerateTab()
        Session("Dashboard_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                                         "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Global", "DashboardMaster.aspx") & """><span>Dashboard Master</span></a></li>" & _
                                         "<li><div class=""space""></div></li>" & _
                                         "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Global", "DashboardMatrix.aspx") & """><span>Dashboard Matrix</span></a></li>" & _
                                         "<li><div class=""space""></div></li>" & _
                                         "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Global", "DashboardSearch.aspx") & """><span>Dashboard Listing</span></a></li>" & _
                                         "<li><div class=""space""></div></li>" & _
                                       "</ul><div></div></div>"
    End Sub
End Class