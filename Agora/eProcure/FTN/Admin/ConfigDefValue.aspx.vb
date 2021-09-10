Imports AgoraLegacy
Imports eProcure.Component


Public Class ConfigDefValueFTN
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim intRecord As Integer
    Dim strType As String
    Dim strDefaultAdd As String
    Dim strDefaultLoc As String
    Dim strCustomAdd As String
    Dim objUserRole As New UserRoles
    Dim objGlobal As New AppGlobals
    Dim strFixRole As String
    Dim intLocIndicator As Integer
    Dim strLoc As String
    Dim strSLoc As String
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmd_save.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmd_save)
        htPageAccess.Add("add", alButtonList)
        htPageAccess.Add("update", alButtonList)
        CheckButtonAccess()
        'cmd_Reset.Enabled = blnCanAdd Or blnCanUpdate Or blnCanDelete
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnPaging = False
        SetGridProperty(dtg_Address)
        SetGridProperty(dtg_Custom)

        If Not Page.IsPostBack Then
            ddl_Select.Items.Clear()
            strFixRole = objUserRole.get_UserFixedRole()
            If strFixRole.IndexOf("Store Keeper") <> -1 Then    'Store Keeper
                ddl_Select.Items.Add("---Select---")
                ddl_Select.Items(0).Value = "---Select---"
                ddl_Select.Items.Add("Location")
                ddl_Select.Items(1).Value = "L"
            Else
                If strFixRole.IndexOf("Buyer") <> -1 Or strFixRole.IndexOf("Purchasing Manager") <> -1 Or strFixRole.IndexOf("Purchasing Officer") <> -1 Then
                    ddl_Select.Items.Add("---Select---")
                    ddl_Select.Items(0).Value = "---Select---"
                    ddl_Select.Items.Add("Billing Address")
                    ddl_Select.Items(1).Value = "B"
                    ddl_Select.Items.Add("Delivery Address")
                    ddl_Select.Items(2).Value = "D"
                End If
                '//FTN doesnt have Custom Field
                'ddl_Select.Items.Add("Custom Field")
                'ddl_Select.Items(3).Value = "C"
            End If
            'If strFixRole.Contains(",") = True Then 'More than one fixed role
            '    ddl_Select.Items.Add("---Select---")
            '    ddl_Select.Items(0).Value = "---Select---"
            '    ddl_Select.Items.Add("Billing Address")
            '    ddl_Select.Items(1).Value = "B"
            '    ddl_Select.Items.Add("Delivery Address")
            '    ddl_Select.Items(2).Value = "D"
            '    If strFixRole.IndexOf("Store Keeper") <> -1 Then    'Store Keeper
            '        ddl_Select.Items.Add("Location")
            '        ddl_Select.Items(3).Value = "L"
            '    End If
            'Else
            '    If strFixRole.IndexOf("Store Keeper") <> -1 Then    'Store Keeper
            '        ddl_Select.Items.Add("---Select---")
            '        ddl_Select.Items(0).Value = "---Select---"
            '        ddl_Select.Items.Add("Location")
            '        ddl_Select.Items(1).Value = "L"
            '    Else
            '        ddl_Select.Items.Add("---Select---")
            '        ddl_Select.Items(0).Value = "---Select---"
            '        ddl_Select.Items.Add("Billing Address")
            '        ddl_Select.Items(1).Value = "B"
            '        ddl_Select.Items.Add("Delivery Address")
            '        ddl_Select.Items(2).Value = "D"
            '    End If
            'End If

            '//because No DataGrid display when page first loaded
            'cmdDelete.Enabled = False
            'cmdModify.Enabled = False

            cmd_Save.Visible = False
            cmd_Reset2.Visible = False
            'cmd_Save.Enabled = False
            'cmd_Reset2.Disabled = True
            GenerateTab()
            ChangeHeaderText()
        End If

        intPageRecordCnt = ViewState("intPageRecordCnt")

        'Dim objGlobal As New appGlobals()
        'objGlobal.FillCodeTable(cbo_State, CodeTable.State)

        'Dim ds As New DataSet()
        'Dim dr As SqlDataReader()
        'Dim objAdmin As New Admin()

        'strType = "D"

        'Dim cal As String = objAdmin.user_Default_Add("B")
        'If (Me.ddl_Select.SelectedItem.Value = "b") Then
        '    Bindgrid()
        'End If


    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objAdmin As New Admin
        Dim ds As New DataSet
        Dim strSelect As String
        strSelect = ddl_Select.SelectedItem.Value

        'ds = objAdmin.PopulateAddress(strType, txt_Code.Text, txt_City.Text, cbo_State.SelectedItem.Value)
        'ds = objAdmin.PopulateAddress(strType, txt_Code.Text, txt_City.Text, "")
        ds = objAdmin.PopulateAddr(strSelect, "", "", "", True)
        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Dim dvViewSample As DataView

        'ds.Tables(0).Rows(1)(2)
        dvViewSample = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)
        If pSorted And ViewState("SortExpression") <> "" Then
            dvViewSample.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        End If


        strDefaultAdd = objAdmin.user_Default_Add(strSelect)

        If intPageRecordCnt > 0 Then
            dtg_Address.DataSource = dvViewSample
            dtg_Address.DataBind()
            cmd_Save.Enabled = True
            cmd_Reset2.Disabled = False
        Else
            dtg_Address.DataBind()
            cmd_Save.Enabled = False
            cmd_Reset2.Disabled = True
            Common.NetMsgbox(Me, MsgNoRecord)
            'intTotPage = 0
        End If
        objAdmin = Nothing

        'ShowStats()'ShowStats()

    End Function

    Private Function BindgridLocation(Optional ByVal pSorted As Boolean = False) As String
        Dim objInv As New Inventory
        Dim ds As New DataSet
        Dim strSelect As String
        Dim strTemp As String
        strSelect = ddl_Select.SelectedItem.Value
        'ds = objAdmin.PopulateAddr(viewState("Type"), txt_Code.Text, txt_City.Text, cbo_State.SelectedItem.Value, True)
        ds = objInv.PopulateLocation
        'ds = objAdmin.PopulateAddr(viewState("Type"), txt_Code.Text, txt_City.Text, "")

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count
        intPageRecordCnt = ViewState("intPageRecordCnt")

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If ViewState("action") = "del" Then
            If dtgLocation.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgLocation.PageSize = 0 Then
                dtgLocation.CurrentPageIndex = dtgLocation.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        strDefaultLoc = objInv.GetDafaultLocation("", "")
        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgLocation, dvViewSample)
            dtgLocation.DataSource = dvViewSample
            dtgLocation.DataBind()
            cmd_Save.Enabled = True
            cmd_Reset2.Disabled = False
        Else
            dtgLocation.ShowHeader = True
            dtgLocation.DataBind()
            cmd_Save.Enabled = False
            cmd_Reset2.Disabled = True
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00006"))
        End If
        objInv = Nothing
    End Function

    Private Function Bindgrid_Custom(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objAdmin As New Admin
        Dim ds As New DataSet
        Dim strSelect As String
        strSelect = ddl_Custom.SelectedItem.Value

        'meilai 20041229
        ' If ddl_Custom.lstItem.Value = "" Then
        'close

        'ds = objAdmin.PopulateAddress(strType, txt_Code.Text, txt_City.Text, cbo_State.SelectedItem.Value)
        'ds = objAdmin.PopulateAddress(strType, txt_Code.Text, txt_City.Text, "")
        ds = objAdmin.Populate_customField(strSelect, "")
        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count
        Dim dvViewSample As DataView

        'ds.Tables(0).Rows(1)(2)
        dvViewSample = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)
        If pSorted Then
            dvViewSample.Sort = viewstate("SortExpression")
            If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        End If

        'objAdmin.user_Default_Add(strSelect)
        strCustomAdd = objAdmin.CustomAddr(strSelect, ddl_Module.SelectedItem.Value)


        If viewstate("intPageRecordCnt") > 0 Then
            'meilai
            cmd_Save.Visible = True
            cmd_Reset2.Visible = True
            'close
            cmd_Save.Enabled = True
            cmd_Reset2.Disabled = False
            dtg_Custom.DataSource = dvViewSample
            dtg_Custom.DataBind()
            dtg_Custom.Visible = True
        Else
            cmd_Save.Enabled = False
            cmd_Reset2.Disabled = True
            dtg_Custom.DataBind()
        End If

        objAdmin = Nothing


    End Function

    Private Sub ddl_Module_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Module.SelectedIndexChanged
        BindCustom(ddl_Module.SelectedItem.Text)
    End Sub
    Public Function BindModule()
        ddl_Module.Items.Clear()
        ddl_Module.Items.Add("---Select---")
        ddl_Module.Items(0).Value = ""
        ddl_Module.Items.Add("PO")
        ddl_Module.Items(1).Value = "PO"
        ddl_Module.Items.Add("PR")
        ddl_Module.Items(2).Value = "PR"
    End Function

    Public Function BindCustom(ByVal CFmodule As String)
        Dim objAdmin As New Admin
        Dim dvCustom As DataView
        dvCustom = objAdmin.getCustomFieldName(CFmodule)
        ddl_Custom.Items.Clear()
        Dim lstItem As New ListItem
        If Not dvCustom Is Nothing Then
            Common.FillDdl(ddl_Custom, "CF_FIELD_NAME", "CF_FIELD_NO", dvCustom)
            '    'meilai
            'Else
            '    cmd_Save.Visible = False
            '    cmd_Reset2.Visible = False
            '    'close
        End If
        ' Add ---Select---
        lstItem.Value = ""
        lstItem.Text = "---Select---"
        ddl_Custom.Items.Insert(0, lstItem)
        objAdmin = Nothing
    End Function

    'Private Sub dtg_Address_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
    '    intPageRecordCnt = viewstate("intPageRecordCnt")
    '    Grid_ItemCreated(dtg_Address, e)
    'End Sub

    'Private Sub dtg_Address_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
    '    dtg_Address.CurrentPageIndex = e.NewPageIndex
    '    Bindgrid(0)
    'End Sub

    'Private Sub dtg_custom_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
    '    ' Grid_ItemCreated(dtg_Custom, e)
    'End Sub


    'Public Sub dtg_custom_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
    '    'dtg_Custom.CurrentPageIndex = e.NewPageIndex
    '    ' BindCustom(0)
    'End Sub
    'Sub ShowStats()
    '    'lblCurrentIndex.Text = intRecord & " Record(s) Found"
    'End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtg_Address.CurrentPageIndex = 0
        Bindgrid(0, True)
        'Grid_SortCommand(sender, e)
        'Bindgrid(dtg_Address.CurrentPageIndex, True)
    End Sub

    Public Sub SortCommandCustom_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtg_Custom.CurrentPageIndex = 0
        Bindgrid_Custom(0, True)
    End Sub

    Public Sub SortCommandLocation_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgLocation.CurrentPageIndex = 0
        BindgridLocation(True)
        'Grid_SortCommand(sender, e)
        'Bindgrid(dtg_Address.CurrentPageIndex, True)
    End Sub

    Private Sub ddl_Select_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Select.SelectedIndexChanged
        If ddl_Select.SelectedItem.Value = "B" Or ddl_Select.SelectedItem.Value = "D" Then  'Billing/Delivery Address
            ddl_Custom.Items.Clear()
            ddl_Module.Items.Clear()
            customfield.Style("display") = "none"
            loc.Style("display") = "none"
            divCustom.Style("display") = "none"
            addr.Style("display") = "inline"
            SetGridProperty(dtg_Address)
            'dtg_Address.AllowPaging = False
            'cmd_Save.Visible = True
            'cmd_Reset2.Visible = True
            'cmd_Save.Enabled = True
            'cmd_Reset2.Disabled = False
            ViewState("SortExpression") = ""
            Bindgrid()
            If dtg_Address.Items.Count > 0 Then
                cmd_Save.Visible = True
                cmd_Reset2.Visible = True
                cmd_Save.Enabled = True
                cmd_Reset2.Disabled = False
            Else
                cmd_Save.Visible = False
                cmd_Reset2.Visible = False
                cmd_Save.Enabled = False
                cmd_Reset2.Disabled = True
            End If

        ElseIf ddl_Select.SelectedItem.Value = "L" Then 'Location
            ddl_Custom.Items.Clear()
            ddl_Module.Items.Clear()
            customfield.Style("display") = "none"
            divCustom.Style("display") = "none"
            loc.Style("display") = "inline"
            addr.Style("display") = "none"
            SetGridProperty(dtgLocation)
            ViewState("SortExpression") = ""
            BindgridLocation()
            ChangeHeaderText()
            If dtgLocation.Items.Count > 0 Then
                cmd_Save.Visible = True
                cmd_Reset2.Visible = True
                cmd_Save.Enabled = True
                cmd_Reset2.Disabled = False
            Else
                cmd_Save.Visible = False
                cmd_Reset2.Visible = False
                cmd_Save.Enabled = False
                cmd_Reset2.Disabled = True
            End If

        ElseIf ddl_Select.SelectedItem.Value = "C" Then
            ddl_Custom.Items.Clear()
            ddl_Module.Items.Clear()
            divCustom.Style("display") = ""
            BindModule()
            dtg_Custom.Visible = False
            customfield.Style("display") = "inline"
            addr.Style("display") = "none"
            loc.Style("display") = "none"
            'meilai
            cmd_Save.Visible = False
            cmd_Reset2.Visible = False
            'cmd_Save.Visible = True
            'cmd_Reset2.Visible = True
            'close
            cmd_Save.Enabled = False
            cmd_Reset2.Disabled = True
        Else
            ddl_Custom.Items.Clear()
            ddl_Module.Items.Clear()
            customfield.Style("display") = "none"
            addr.Style("display") = "none"
            loc.Style("display") = "none"
            cmd_Save.Visible = False
            cmd_Reset2.Visible = False

        End If
        'cmd_Reset2.Disabled = True

    End Sub

    'Private Sub ddl_custom_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    'If ddl_custom.SelectedItem.Value = "test" Or ddl_custom.SelectedItem.Value = "ee" Then
    '    '    'customfield.Style("display") = "none"
    '    '    'addr.Style("display") = "inline"
    '    '    SetGridProperty(dtg_Custom)
    '    '    Bindgrid_Custom(0, True)
    '    'Else
    '    '    customfield.Style("display") = "inline"
    '    '    addr.Style("display") = "none"
    '    'End If

    '    SetGridProperty(dtg_Custom)
    '    Bindgrid_Custom()

    '    'cmd_Reset2.Disabled = True

    'End Sub

    Private Sub dtg_Address_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_Address.ItemDataBound
        Dim objAdmin As New Admin
        'objAdmin.user_Default_Add(ddl_Select.SelectedItem.Value)

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lbl As Label = e.Item.FindControl("lblAddr")

            If e.Item.Cells(1).Text = strDefaultAdd Then
                lbl.Text = "<input type=radio name='optAddr' value='" & e.Item.ItemIndex & "' checked=checked>"
            Else
                lbl.Text = "<input type=radio name='optAddr' value='" & e.Item.ItemIndex & "'>"
            End If

            'rsc.SelectedIndexes
            'rsc.se
            'ElseIf e.Item.ItemType = ListItemType.Header Then
            '    If ddl_Select.SelectedItem.Value = "B" Then
            '        e.Item.Cells(1).Text = "Billing Address"
            '    ElseIf ddl_Select.SelectedItem.Value = "D" Then
            '        e.Item.Cells(1).Text = "Delivery Address"
            '    End If

        End If
    End Sub

    Private Sub dtg_Custom_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_Custom.ItemDataBound

        Dim objAdmin As New Admin
        'objAdmin.user_Default_Add(ddl_Select.SelectedItem.Value)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            'Dim radCustom As RadioButton
            'radCustom = e.Item.Cells(0).FindControl("opt_SelectionCustom")

            'If e.Item.Cells(1).Text = strCustomAdd Then
            '    radCustom.Checked = True
            'End If

            Dim lbl As Label = e.Item.FindControl("lblCustom")

            If e.Item.Cells(1).Text = strCustomAdd Then
                lbl.Text = "<input type=radio name='optCustom' value='" & e.Item.ItemIndex & "' checked=checked>"
            Else
                lbl.Text = "<input type=radio name='optCustom' value='" & e.Item.ItemIndex & "'>"
            End If

        End If
    End Sub

    Private Sub cmd_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Save.Click
        Dim objAdmin As New Admin
        'Dim dgItem As DataGridItem
        Dim intSelectedRow As Integer

        If ddl_Select.SelectedItem.Value = "B" Or ddl_Select.SelectedItem.Value = "D" Then  'Billing/Delivery Address
            intSelectedRow = Request.Form("optAddr")
            objAdmin.updateUserDefault(ddl_Select.SelectedItem.Value, dtg_Address.Items(intSelectedRow).Cells(1).Text, "", "")
            Bindgrid(0, True)

            'For Each dgItem In dtg_Address.Items
            '    rdb = dgItem.Cells(0).FindControl("optSelection")
            '    If rdb.Checked Then
            '        objAdmin.updateUserDefault(ddl_Select.SelectedItem.Value, dgItem.Cells(1).Text, "")
            '        Exit For
            '    End If
            'Next

        ElseIf ddl_Select.SelectedItem.Value = "L" Then  'Location
            intSelectedRow = Request.Form("optLoc")
            objAdmin.updateUserDefault(ddl_Select.SelectedItem.Value, dtgLocation.Items(intSelectedRow).Cells(1).Text, "WH", "")
            BindgridLocation(True)

        Else
            intSelectedRow = Request.Form("optCustom")
            objAdmin.updateUserDefault(ddl_Select.SelectedItem.Value, dtg_Custom.Items(intSelectedRow).Cells(1).Text, ddl_Custom.SelectedItem.Value, ddl_Custom.SelectedItem.Text)
            Bindgrid_Custom(0, True)
            'For Each dgItem In dtg_Custom.Items
            '    rdb = dgItem.Cells(0).FindControl("opt_SelectionCustom")
            '    If rdb.Checked Then
            '        objAdmin.updateUserDefault(ddl_Select.SelectedItem.Value, dgItem.Cells(1).Text, ddl_custom.SelectedItem.Value)
            '        Exit For
            '    End If
            'Next
        End If

        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        objAdmin = Nothing
    End Sub


    Private Sub ddl_Custom_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Custom.SelectedIndexChanged
        If ddl_Custom.SelectedValue = "" Then
            cmd_Save.Visible = False
            cmd_Reset2.Visible = False
        Else
            cmd_Save.Visible = True
            cmd_Reset2.Visible = True
            'SetGridProperty(dtg_Custom)
        End If
        Bindgrid_Custom()
    End Sub



    'Sub dtgCustomField_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgCustomField.PageIndexChanged
    '    dtgCustomField.CurrentPageIndex = e.NewPageIndex
    '    Bindgrid(0, True)
    'End Sub

    Public Sub dtg_Address_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtg_Address.PageIndexChanged
        ' dtg_Address.CurrentPageIndex = e.NewPageIndex
        'Bindgrid(0, True)

    End Sub

    Public Sub dtg_Custom_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtg_Custom.PageIndexChanged
        ' dtg_Custom.CurrentPageIndex = e.NewPageIndex
        'Bindgrid_Custom(0, True)

    End Sub

    Private Sub dtg_Address_ItemCreated1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_Address.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtg_Address, e)

    End Sub

    Private Sub dtg_Custom_ItemCreated1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_Custom.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtg_Custom, e)
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_ConfigDef_tabs") = "<div class=""t_entity""><ul>" & _
                                    "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PersonalSetting", "PersonalDetails.aspx", "mode=modify&pageid=" & strPageId) & """><span>Personal Details</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Admin", "ConfigDefValue.aspx", "pageid=" & strPageId) & """><span>Configure Default Settings</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                   "</ul><div></div></div>"


        '        Session("w_ConfigDef_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '                             "<li><a class=""t_entity_btn"" href=""../PersonalSetting/PersonalDetails.aspx?mode=modify&pageid=" & strPageId & """><span>Personal Details</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                             "<li><a class=""t_entity_btn_selected"" href=""../Admin/ConfigDefValue.aspx?pageid=" & strPageId & """><span>Configure Default Values</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                           "</ul><div></div></div>"
    End Sub

    Private Sub dtgLocation_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgLocation.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtg_Address, e)
        ChangeHeaderText()
    End Sub

    Private Sub dtgLocation_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgLocation.ItemDataBound

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lbl As Label = e.Item.FindControl("lblLoc")

            If e.Item.Cells(1).Text = strDefaultLoc Then
                lbl.Text = "<input type=radio name='optLoc' value='" & e.Item.ItemIndex & "' checked=checked>"
            Else
                lbl.Text = "<input type=radio name='optLoc' value='" & e.Item.ItemIndex & "'>"
            End If

        End If
    End Sub

    Private Sub ChangeHeaderText()
        Dim objInv As New Inventory

        objInv.GetLocationInfo(strLoc, strSLoc, intLocIndicator)

        dtgLocation.Columns(2).HeaderText = strLoc
        dtgLocation.Columns(3).HeaderText = strSLoc

        If intLocIndicator > 1 Then 'Sub Location is defined
            Me.dtgLocation.Columns(3).Visible = True

        Else
            Me.dtgLocation.Columns(3).Visible = False

        End If
        objInv = Nothing

    End Sub
End Class
