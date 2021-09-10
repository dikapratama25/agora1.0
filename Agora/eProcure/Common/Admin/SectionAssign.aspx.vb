Imports AgoraLegacy
Imports eProcure.Component


Public Class SectionAssign
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objAdm As New Admin
    Dim objGlobal As New AppGlobals
    'Dim chkAll As CheckBox

    Protected WithEvents cmd_search As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear1 As System.Web.UI.WebControls.Button
    Protected WithEvents dtgSection As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtUserId As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUserName As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddl_Select As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddl_Section As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Hide_Add2 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents chkUserAssigned As System.Web.UI.WebControls.CheckBox
    Dim ds As DataSet

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub


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
        SetGridProperty(dtgSection)
        If Not IsPostBack Then
            cmd_search.Enabled = False
            cmd_clear1.Enabled = False
            cmd_save.Enabled = False

            BindSection()
        End If

    End Sub

    Public Function BindSection()
        Dim cbolist As New ListItem
        Dim objAdmin As New Admin
        Dim dvCustom As DataView

        dvCustom = objAdmin.getUsrWithSection()
       
        ddl_Section.Items.Clear()
        If Not dvCustom Is Nothing Then
            Common.FillDdl(ddl_Section, "SECTION", "CS_SEC_INDEX", dvCustom)
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        ddl_Section.Items.Insert(0, cbolist)

    End Function

    Sub dtgSection_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgSection.PageIndexChanged
        ViewState("SortExpression") = "UM_USER_ID"
        dtgSection.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgSection.SortCommand

        Grid_SortCommand(sender, e)
        ViewState("SortExpression") = "UM_USER_ID"
        dtgSection.CurrentPageIndex = 0
        Bindgrid()

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objUsers As New Users
        Dim ds As DataSet
        Dim chkAssigned As String

        If chkUserAssigned.Checked = True Then
            chkAssigned = "Y"
        Else
            chkAssigned = "N"
        End If

        ds = objUsers.SearchAllUserSection(txtUserId.Text, txtUserName.Text, ViewState("SecCode"), chkAssigned)
        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            dtgSection.DataSource = dvViewSample
            dtgSection.DataBind()
            cmd_save.Enabled = True
        Else
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00006"), MsgBoxStyle.Information)
            dtgSection.DataBind()
            cmd_save.Enabled = False
        End If

    End Function

    Private Sub dtgSection_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgSection.ItemCreated

        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgSection, e)

        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            If chkUserAssigned.Checked = True Then
                chkAll.Checked = True
            End If
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If

    End Sub

    Private Sub dtgUser_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgSection.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            If dv("CSB_SECTION") = "Yes" Then
                chk.Checked = True
            Else
                chk.Checked = False
            End If

        End If
    End Sub

    Private Sub cmd_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_search.Click
        ViewState("SecCode") = ddl_Section.SelectedValue
        dtgSection.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub cmd_clear1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_clear1.Click
        chkUserAssigned.Checked = False
        txtUserId.Text = ""
        txtUserName.Text = ""
    End Sub

    Private Sub cmd_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_save.Click
        Dim intMsg As Integer
        Dim dsItem As New DataSet

        dsItem = bindItem()

        If dsItem.Tables(0).Rows.Count > 0 Or dsItem.Tables(1).Rows.Count > 0 Then
            Dim objUsers As New Users
            objUsers.RemoveSection(dsItem.Tables(1))
            intMsg = objUsers.insertSection(dsItem.Tables(0))
            Select Case intMsg
                Case WheelMsgNum.Save
                    Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
                    dtgSection.CurrentPageIndex = 0
                    Bindgrid()

                Case WheelMsgNum.NotSave
                    Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
            End Select
        End If
    End Sub

    Private Function bindItem() As DataSet
        Dim dtItem As New DataTable
        Dim dtDelete As New DataTable
        Dim ds As New DataSet

        dtItem.Columns.Add("UA_SEC_INDEX", Type.GetType("System.String"))
        dtItem.Columns.Add("UA_USER_ID", Type.GetType("System.String"))

        dtDelete.Columns.Add("UA_SEC_INDEX", Type.GetType("System.String"))
        dtDelete.Columns.Add("UA_USER_ID", Type.GetType("System.String"))

        Dim dtr As DataRow
        Dim dgItem As DataGridItem
        For Each dgItem In dtgSection.Items
            Dim chkSelection As CheckBox = dgItem.Cells(0).FindControl("chkSelection")
            If chkSelection.Checked Then
                dtr = dtItem.NewRow
                dtr("UA_SEC_INDEX") = ddl_Section.SelectedValue
                dtr("UA_USER_ID") = dgItem.Cells(1).Text
                dtItem.Rows.Add(dtr)

            Else    'remove section
                dtr = dtDelete.NewRow
                dtr("UA_SEC_INDEX") = ddl_Section.SelectedValue
                dtr("UA_USER_ID") = dgItem.Cells(1).Text
                dtDelete.Rows.Add(dtr)
            End If
        Next
        ds.Tables.Add(dtItem)
        ds.Tables.Add(dtDelete)
        bindItem = ds
    End Function

    Private Sub ddl_Select_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Select.SelectedIndexChanged

        If ddl_Select.SelectedItem.Value = "1" Then
            Me.Response.Redirect(dDispatcher.direct("Admin", "ExchangeRates.aspx", "side=BUYER" & "&pageid=" & strPageId))
        ElseIf ddl_Select.SelectedItem.Value = "1" Then
            Me.Response.Redirect(dDispatcher.direct("Admin", "DeliveryTerm.aspx", "side=BUYER" & "&pageid=" & strPageId))
        ElseIf ddl_Select.SelectedItem.Value = "2" Then
            Me.Response.Redirect(dDispatcher.direct("Admin", "PackingType.aspx", "side=BUYER" & "&pageid=" & strPageId))
        ElseIf ddl_Select.SelectedItem.Value = "3" Then
            Me.Response.Redirect(dDispatcher.direct("Admin", "Section.aspx", "side=BUYER" & "&pageid=" & strPageId))
        ElseIf ddl_Select.SelectedItem.Value = "4" Then
            Me.Response.Redirect(dDispatcher.direct("Admin", "SectionAssign.aspx", "side=BUYER" & "&pageid=" & strPageId))
        Else
            Me.Response.Redirect(dDispatcher.direct("Admin", "Others.aspx", "side=BUYER" & "&pageid=" & strPageId))
        End If
    End Sub

    Private Sub ddl_Section_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Section.SelectedIndexChanged

        If ddl_Section.SelectedIndex > 0 Then
            ViewState("SortAscending") = "yes"
            ViewState("SortExpression") = "UM_USER_ID"
            dtgSection.CurrentPageIndex = 0
            ViewState("SecCode") = ddl_Section.SelectedValue
            hidIndex.Value = ddl_Section.SelectedValue
            cmd_search.Enabled = True
            cmd_clear1.Enabled = True
            Bindgrid()
        Else
            ViewState("SecCode") = ""
            hidIndex.Value = ""
            cmd_search.Enabled = False
            cmd_clear1.Enabled = False
            cmd_save.Enabled = False
            dtgSection.DataSource = Nothing
            dtgSection.DataBind()
        End If

    End Sub
End Class
