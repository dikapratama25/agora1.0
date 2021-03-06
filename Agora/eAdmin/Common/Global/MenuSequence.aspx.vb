Imports System.Drawing
Imports AgoraLegacy
Partial Public Class MenuSequence
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("PageCount") = 20
        blnPaging = False
        blnSorting = False
        SetGridProperty(dtgMenuSeq)
        GenerateTab()
        If Not Page.IsPostBack Then
            Populate()
            Bindgrid()
        End If

    End Sub
    Sub Populate()
        Dim objMS As New MenuSeq
        objMS.FillModule(ddlModule, ConfigurationSettings.AppSettings("eProcurePath"))
        objMS.FillSeqNo(ddlSeqNo, ConfigurationSettings.AppSettings("eProcurePath"))
    End Sub
    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objMS As New MenuSeq
        Dim dsMenu As DataSet = New DataSet
        dsMenu = objMS.GetMenu()
        Dim dvMenu As New DataView
        dvMenu = dsMenu.Tables(0).DefaultView
        If dsMenu.Tables(0).Rows.Count > 0 Then
            cmdSave.Visible = True
            dtgMenuSeq.DataSource = dvMenu
            dtgMenuSeq.DataBind()
        Else
            cmdSave.Visible = False
            dtgMenuSeq.DataBind()
        End If


    End Function
    Private Sub dtgMenuSeq_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgMenuSeq.ItemCreated
        Grid_ItemCreated(dtgMenuSeq, e)
    End Sub
    Private Sub dtgMenuSeq_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgMenuSeq.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim txtPanelName As TextBox
            txtPanelName = e.Item.FindControl("txtPanelName")
            txtPanelName.Text = dv("MM_MENU_NAME")
        End If
    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objM As New MenuSeq
        Dim SelectedModValue As Integer
        Dim NewSeqNo As Integer
        Dim ParentValue As Integer
        Dim MaxVal As Integer


        SelectedModValue = ddlModule.SelectedValue
        NewSeqNo = ddlSeqNo.Text
        ParentValue = objM.GetModuleParent(SelectedModValue)
        MaxVal = objM.GetMaxModule

        objM.UpdateModuleSeqNo(NewSeqNo, ParentValue, SelectedModValue, MaxVal)

        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        Populate()
        Bindgrid()
    End Sub


    Sub GenerateTab()
        Session("Menu_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                                         "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Global", "MenuMtn.aspx") & """><span>Menu Maintenance</span></a></li>" & _
                                         "<li><div class=""space""></div></li>" & _
                                         "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Global", "MenuSequence.aspx") & """><span>Menu Sequence</span></a></li>" & _
                                         "<li><div class=""space""></div></li>" & _
                                       "</ul><div></div></div>"



    End Sub

End Class