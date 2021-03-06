Imports System.Drawing
Imports AgoraLegacy
Partial Public Class DashboardMaint
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        blnPaging = False
        SetGridProperty(dtgDashboardMaint)
        lblHeader.Text = "Dashboard Maintenance"
        ddlUserRole.Attributes.Add("onchange", "btnhidden.click();")
        GenerateTab()

        If Not Page.IsPostBack Then
            Populate()
            ViewState("SortAscending") = "yes"
            ViewState("SortExpression") = "DM_PANEL_NAME"
            Dim selecIndex As Integer
            selecIndex = ddlUserRole.Items.IndexOf(ddlUserRole.Items.FindByText(Replace(Request.QueryString("role"), "'", "")))
            If selecIndex > 0 Then
                ddlUserRole.SelectedIndex = selecIndex
                Bindgrid()
            End If
        End If


    End Sub
    Sub Populate()
        Dim objGlobal As New AppGlobals
        objGlobal.FillFixedRole(ddlUserRole, ConfigurationSettings.AppSettings("eProcurePath"))
    End Sub
    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objDash As New Dashboard
        Dim dsDashboard As DataSet = New DataSet
        dsDashboard = objDash.GetDashboardMatrix("all")
        Dim dvDashboard As New DataView
        dvDashboard = dsDashboard.Tables(0).DefaultView
        dvDashboard.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvDashboard.Sort += " DESC"


        If dsDashboard.Tables(0).Rows.Count > 0 Then
            cmdSave.Visible = True
            dtgDashboardMaint.DataSource = dvDashboard
            dtgDashboardMaint.DataBind()
        Else
            cmdSave.Visible = False
            dtgDashboardMaint.DataBind()
        End If




    End Function
    Private Sub dtgDashboardMaint_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDashboardMaint.ItemCreated
        Grid_ItemCreated(dtgDashboardMaint, e)
    End Sub
    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgDashboardMaint.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub
    Private Sub dtgDashboardMaint_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDashboardMaint.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim objDash As New Dashboard
            Dim ds As New DataSet
            Dim chkbox As CheckBox
            chkbox = e.Item.Cells(2).FindControl("ChkView")
            ds = objDash.GetDashboardMatrix("checkbox", ddlUserRole.SelectedItem.Text, e.Item.Cells(0).Text)
            If ds.Tables(0).Rows.Count > 0 Then
                chkbox.Checked = True
            Else
                chkbox.Checked = False
            End If

        End If
    End Sub

    Protected Sub btnhidden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnhidden.Click
        Bindgrid()
        If ddlUserRole.SelectedIndex = 0 Then
            cmdSave.Visible = False
        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objDash As New Dashboard
        Dim dtgItem As DataGridItem
        Dim chkitem As CheckBox
        objDash.ClearDashboardMatrix(ddlUserRole.SelectedItem.Text)
        For Each dtgItem In dtgDashboardMaint.Items
            chkitem = dtgItem.FindControl("ChkView")
            If chkitem.Checked Then
                objDash.AddDashboardMatrix(ddlUserRole.SelectedItem.Text, dtgItem.Cells.Item(0).Text)
            End If
        Next
        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)

    End Sub
    Sub GenerateTab()
        Session("Dashboard_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                                         "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Global", "DashboardMaster.aspx") & """><span>Dashboard Master</span></a></li>" & _
                                         "<li><div class=""space""></div></li>" & _
                                         "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Global", "DashboardMatrix.aspx") & """><span>Dashboard Matrix</span></a></li>" & _
                                         "<li><div class=""space""></div></li>" & _
                                         "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Global", "DashboardSearch.aspx") & """><span>Dashboard Listing</span></a></li>" & _
                                         "<li><div class=""space""></div></li>" & _
                                       "</ul><div></div></div>"
    End Sub
End Class