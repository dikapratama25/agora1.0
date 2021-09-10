Imports AgoraLegacy
Imports eProcure.Component

Public Class CustomFields
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Protected WithEvents delete As System.Web.UI.WebControls.Button
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents dtgCustomField As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lbl_add_mod As System.Web.UI.WebControls.Label
    Protected WithEvents lblName As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents txtValue As System.Web.UI.WebControls.TextBox
    Protected WithEvents vldValue As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents value As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents cmd_clear1 As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_search As System.Web.UI.WebControls.Button    
    Protected WithEvents Button8 As System.Web.UI.WebControls.Button
    Protected WithEvents ddlModule As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlSearch As System.Web.UI.WebControls.DropDownList
    Protected WithEvents btnHidden As System.Web.UI.WebControls.Button


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
        cmdAdd.Enabled = False
        cmdDelete.Enabled = False
        cmdModify.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        'alButtonList.Add(cmdValues)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdModify)
        'alButtonList.Add(cmdValues)
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdDelete)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        '//additional checking
        If viewstate("intPageRecordCnt") > 0 Then
            cmdDelete.Enabled = blnCanDelete
            cmdModify.Enabled = blnCanUpdate
            '//mean Enable, can't use button.Enabled because this is a HTML button
        Else
            cmdDelete.Enabled = False
            cmdModify.Enabled = False
        End If
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCustomField)

        If Not Page.IsPostBack Then
            bindDDL()
            cmdDelete.Enabled = False
            cmdModify.Enabled = False
            'Bindgrid(0, True)
            'Else
            '    If viewstate("SortExpression") = "" Then
            '        Bindgrid(0)
            '    Else
            '        Bindgrid(0, True)
            '    End If
        End If
        intPageRecordCnt = viewstate("intPageRecordCnt")
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")

        'Put user code to initialize the page here

    End Sub
    Sub bindDDL()
        Dim dv As DataView
        Dim objadmin As New Admin
        Dim cbolist As New ListItem
        dv = objadmin.getCustomFieldName(ddlModule.Text)
        ddlSearch.Items.Clear()
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        ddlSearch.Items.Insert(0, cbolist)
        ddlSearch.SelectedIndex = 0
        Common.FillDdl(ddlsearch, "CF_FIELD_NAME", "CF_FIELD_NAME", dv)
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String

        Dim objAdmin As New Admin

        '   Dim objRFQ As String
        Dim dvw As New DataView

        dvw = objAdmin.getCustomFieldListing(ddlSearch.SelectedItem.Text, ddlModule.SelectedItem.Text)

        If dvw Is Nothing Then
            viewstate("intPageRecordCnt") = 0
            ButtonProp(False)
            Common.NetMsgbox(Me, MsgNoRecord)
            dtgCustomField.DataBind()
            Exit Function
        End If
        viewstate("intPageRecordCnt") = dvw.Count ' ds.Tables(0).Rows.Count

        ' If pSorted Then
        'If viewstate("SortExpression") Is Nothing Then
        '    viewstate("SortExpression") = "CF_Field_Name"
        '    viewstate("SortAscending") = "yes"
        'End If
        dvw.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvw.Sort += " DESC"

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If dtgCustomField.CurrentPageIndex > 0 And dvw.Count Mod dtgCustomField.PageSize = 0 Then
                dtgCustomField.CurrentPageIndex = dtgCustomField.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
            'Else
            '    If dtgCustomField.CurrentPageIndex > 0 And dvw.Count \ dtgCustomField.PageSize <= dtgCustomField.CurrentPageIndex Then
            '        dtgCustomField.CurrentPageIndex = dvw.Count \ dtgCustomField.PageSize
            '        ' b4 change=> If dtgCustomField.CurrentPageIndex > 0 And dvw.Count \ dtgCustomField.PageSize < dtgCustomField.CurrentPageIndex Then"
            '        ' after changes =>If dtgCustomField.CurrentPageIndex > 0 And dvw.Count \ dtgCustomField.PageSize <= dtgCustomField.CurrentPageIndex Then
            '    End If
        End If

        If viewstate("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtgCustomField, dvw)
            dtgCustomField.DataSource = dvw
            dtgCustomField.DataBind()
            ButtonProp(True)
        Else
            Common.NetMsgbox(Me, MsgNoRecord)
            'Common.NetMsgbox(Me, wheelms)
            dtgCustomField.DataBind()
            ButtonProp(False)
        End If

        ' add for above checking
        viewstate("PageCount") = dtgCustomField.PageCount
        objAdmin = Nothing


    End Function

    Sub dtgCustomField_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgCustomField.PageIndexChanged
        dtgCustomField.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0, True)
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgCustomField.SortCommand
        Grid_SortCommand(sender, e)
        dtgCustomField.CurrentPageIndex = 0
        Bindgrid(0, True)
    End Sub

    'Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click

    '    '//add by Moo,to prevent "no record found" message prompt after clicking save button
    '    txtSearch.Text = ""
    '    'Dim intnum As Integer
    '    Dim objPersonal As New PersonalSetting
    '    Dim intMsgNo As Integer
    '    Dim strMsg As String

    '    If hidMode.Value = "a" Then
    '        intMsgNo = objPersonal.addCustomList(txtName.Text, txtValue.Text)

    '        Select Case intMsgNo
    '            Case WheelMsgNum.Save
    '                strMsg = MsgRecordSave
    '                txtName.Text = ""
    '                txtValue.Text = ""
    '            Case WheelMsgNum.Duplicate
    '                strMsg = MsgRecordDuplicate
    '            Case WheelMsgNum.NotSave
    '                strMsg = MsgRecordNotSave
    '        End Select
    '    ElseIf hidMode.Value = "m" Then
    '        intMsgNo = objPersonal.modCustomlist(hidIndex.Value, txtName.Text, 0, "", ViewState("oldvalue"))
    '        Select Case intMsgNo
    '            Case WheelMsgNum.Save
    '                strMsg = MsgRecordSave
    '                txtName.Text = ""
    '                txtValue.Text = ""
    '                custom.Style("display") = "none"
    '                ViewState("oldvalue") = ""
    '            Case WheelMsgNum.Duplicate
    '                txtName.Text = ViewState("oldvalue")
    '                strMsg = MsgRecordDuplicate
    '                'custom.Style("display") = ""
    '            Case WheelMsgNum.NotSave
    '                strMsg = MsgRecordNotSave
    '                'custom.Style("display") = ""
    '        End Select
    '        'custom.Style("display") = "none"
    '    End If

    '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
    '    Bindgrid()

    'End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        Session("userAction") = "Add"

        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("Admin", "AddCustomFields.aspx", "pageid=" & strPageId)
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog('" & dDispatcher.direct("Admin", "Dialog.aspx", "page=" & strFileName) & "','250px');")
        strscript.Append("document.getElementById('btnHidden').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())
        ''Dim objPersonal As New Wheel.Components.PersonalSetting()

        'custom.Style("display") = "inline"
        'value.Style("display") = "inline"
        'vldValue.Visible = True
        'txtName.Text = ""
        'txtValue.Text = ""
        'cmdClear.Text = "Clear"
        'hidMode.Value = "a"
        'lbl_add_mod.Text = "Please add the following value"


        ''vldName2.Enabled = True
        ''objPersonal.addCustomList(TxtName.Text)

    End Sub

    Private Sub cmdModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim name, value, CFmodule, index As String
        For Each dgItem In dtgCustomField.Items

            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                CFmodule = dgItem.Cells(1).Text                
                index = dgItem.Cells(4).Text
            End If
        Next
        Session("userAction") = "Modify"

        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("Admin", "AddCustomFields.aspx", "pageid=" & strPageId & "&module=" & CFmodule & "&index=" & index)
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog('" & dDispatcher.direct("Admin", "Dialog.aspx", "page=" & strFileName) & "','250px');")
        strscript.Append("document.getElementById('btnHidden').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())
        'Dim dgItem As DataGridItem
        'Dim chk As CheckBox
        ''Dim strlistindex As String

        'custom.Style("display") = "inline"
        'value.Style("display") = "none"
        'vldValue.Visible = False
        'cmdClear.Text = "Reset"
        'hidMode.Value = "m"
        'For Each dgItem In dtgCustomField.Items
        '    chk = dgItem.FindControl("chkSelection")
        '    If chk.Checked Then
        '        hidIndex.Value = dtgCustomField.DataKeys.Item(dgItem.ItemIndex)
        '        txtName.Text = dgItem.Cells(1).Text
        '        ViewState("oldvalue") = dgItem.Cells(1).Text
        '        Exit For
        '    End If
        'Next
        'lbl_add_mod.Text = "Please modify the following value"

    End Sub

    Private Sub dtgCustomField_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCustomField.ItemCreated
        '//this line must be included
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgCustomField, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        '//To prevent "No Record Found" msg

        Dim objPersonal As New PersonalSetting
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim strlistindex As String
        For Each dgItem In dtgCustomField.Items

            chk = dgItem.FindControl("chkSelection")
            strlistindex = dtgCustomField.DataKeys.Item(dgItem.ItemIndex)
            If chk.Checked Then
                objPersonal.delCustomField(dgItem.Cells(1).Text, dgItem.Cells(2).Text, dgItem.Cells(3).Text)
            End If
        Next

        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        viewstate("action") = "del"
        Bindgrid()
    End Sub
    'Private Sub cmdValues_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdValues.Click

    '    Dim dgItem As DataGridItem
    '    Dim chk As CheckBox
    '    'Dim strlistindex As String
    '    Dim i As Integer
    '    Dim strName As String

    '    For Each dgItem In dtgCustomField.Items
    '        chk = dgItem.FindControl("chkSelection")
    '        If chk.Checked Then
    '            hidIndex.Value = dtgCustomField.DataKeys.Item(dgItem.ItemIndex)
    '            strName = dgItem.Cells(1).Text
    '            Me.Response.Redirect(dDispatcher.direct("Admin", "CustomFieldValue.aspx", "mod=C&value=" & Server.UrlEncode(hidIndex.Value) & "&name" & Server.UrlEncode(strName) & "&pageid=" & strPageId))
    '            Exit For
    '        End If
    '    Next
    '    'Dim strredirect As String
    '    'strredirect = "CustomFieldValue.aspx?pageid=" & strPageId

    'End Sub

    'Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
    '    custom.Style("display") = "none"
    '    hidMode.Value = ""
    '    'lbl_add_mod.Text = "Add"
    '    'vldName2.Enabled = True
    'End Sub

    Private Sub dtgCustomField_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCustomField.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim chk As CheckBox
            chk = e.Item.FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
        End If
    End Sub

    'Private Sub cmdClear_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
    '    If hidMode.Value = "a" Then  ' add mode
    '        txtName.Text = ""
    '        txtValue.Text = ""
    '    Else ' modify mode
    '        txtName.Text = ViewState("oldvalue")
    '    End If

    'End Sub

    Private Sub ButtonProp(ByVal blnEnable As Boolean)
        '//Add button should be always enable
        'cmdAdd.Enabled = blnEnable
        cmdDelete.Enabled = blnEnable
        cmdModify.Enabled = blnEnable
    End Sub

    Private Sub cmd_search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_search.Click
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "CF_FIELD_NAME"
        dtgCustomField.CurrentPageIndex = 0
        Bindgrid(0)

    End Sub
    Public Sub btnHidden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHidden.Click
        Bindgrid(True)
    End Sub
    Private Sub cmd_clear1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_clear1.Click
        ddlSearch.SelectedIndex = 0
        ddlModule.SelectedIndex = 0
    End Sub

    Private Sub ddlModule_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlModule.SelectedIndexChanged
        bindDDL()
    End Sub
End Class

