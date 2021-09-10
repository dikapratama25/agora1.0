Imports AgoraLegacy
Imports eProcure.Component

Public Class CustomFieldValue
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Protected WithEvents lblName As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents dtgCustomField As System.Web.UI.WebControls.DataGrid
    Protected WithEvents custom As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents txtsearch As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmd_search As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear1 As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Add As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Modify As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Delete As System.Web.UI.WebControls.Button
    Protected WithEvents trT As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trP As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdSelect As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidItem As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lbl_add_mod As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCancel As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents hidValue As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents vldValue As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents TxtValue As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblFieldName As System.Web.UI.WebControls.Label

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
        '//only add security if come from menu
        '//no security for pop-up window
        If viewState("Mod") <> "P" Then
            cmd_Add.Enabled = False
            cmd_Delete.Enabled = False
            cmd_Modify.Enabled = False
            Dim alButtonList As ArrayList
            alButtonList = New ArrayList
            alButtonList.Add(cmd_Add)
            'alButtonList.Add(cmdValues)
            htPageAccess.Add("add", alButtonList)
            alButtonList = New ArrayList
            alButtonList.Add(cmd_Modify)
            'alButtonList.Add(cmdValues)
            htPageAccess.Add("update", alButtonList)
            alButtonList = New ArrayList
            alButtonList.Add(cmd_Delete)
            htPageAccess.Add("delete", alButtonList)
            CheckButtonAccess()
            '//additional checking
            If viewstate("intPageRecordCnt") Then
                cmd_Delete.Enabled = blnCanDelete
                cmd_Modify.Enabled = blnCanUpdate
                '//mean Enable, can't use button.Enabled because this is a HTML button
                cmdReset.Disabled = False
            Else
                cmd_Delete.Enabled = False
                cmd_Modify.Enabled = False
                cmdReset.Disabled = True
            End If
        End If
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCustomField)
        Dim objGlobal As New AppGlobals
        If Not IsPostBack Then
            viewState("Value") = Me.Request.QueryString("Value")
            viewState("Name") = Me.Request.QueryString("name")
            ViewState("Mod") = Me.Request.QueryString("mod") ' P - PR; C - custom
            ViewState("module") = Me.Request.QueryString("module")
            lblFieldName.Text = viewState("Name")

            If viewState("Mod") = "P" Then
                lblTitle.Text = "Select Custom Fields"
                hidID.Value = Request.QueryString("id")
                trP.Visible = True
                trT.Visible = False
                lnkBack.Visible = False
            Else
                lblTitle.Text = "Custom Field Value"
                trP.Visible = False
                trT.Visible = True
                cmd_Modify.Enabled = True
                cmd_Delete.Enabled = True
                lnkBack.NavigateUrl = "" & dDispatcher.direct("Admin", "CustomFields.aspx", "pageid=" & strPageId)
            End If
            Bindgrid()
        End If

        If viewState("Mod") = "C" Then
            cmd_Delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
            cmd_Modify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        End If
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String

        Dim objAdmin As New Admin
        Dim ds As New DataSet
        Dim strValue As String

        ds = objAdmin.Populate_customField(ViewState("Value"), txtsearch.Text, ViewState("module"))

        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count
        Dim dvViewDept As DataView
        dvViewDept = ds.Tables(0).DefaultView

        'If pSorted Then
        dvViewDept.Sort = viewstate("SortExpression")
        If pSorted Then
            dvViewDept.Sort = viewstate("SortExpression")
            If viewstate("SortAscending") = "no" Then dvViewDept.Sort += " DESC"
        End If

        If viewstate("action") = "del" Then
            If dtgCustomField.CurrentPageIndex > 0 And dvViewDept.Count Mod dtgCustomField.PageSize = 0 Then
                dtgCustomField.CurrentPageIndex = dtgCustomField.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        If viewstate("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtgCustomField, dvViewDept)

            dtgCustomField.DataSource = dvViewDept
            dtgCustomField.DataBind()
            If viewState("Mod") = "P" Then
                cmdSelect.Disabled = False
            Else
                cmd_Modify.Enabled = True
                cmd_Delete.Enabled = True
                cmdReset.Disabled = False
            End If
        Else
            'cmd_Add.Enabled = False
            cmd_Delete.Enabled = False
            cmd_Modify.Enabled = False
            cmdReset.Disabled = True
            dtgCustomField.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ' add for above checking
        viewstate("PageCount") = dtgCustomField.PageCount
    End Function

    Private Sub dtgCustomField_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCustomField.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgCustomField, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
            If viewState("Mod") = "P" Then
                chkAll.Visible = False
            End If
        End If
    End Sub

    'Private Sub dtgCustomField_SortCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgCustomField.SortCommand
    '    Grid_SortCommand(sender, e)
    '    Bindgrid(dtgCustomField.CurrentPageIndex, True)
    'End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgCustomField.CurrentPageIndex = 0
        Bindgrid(0, True)
    End Sub

    Public Sub dtgCustomField_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgCustomField.PageIndexChanged
        dtgCustomField.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub cmd_Add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Add.Click
        custom.Style("display") = "inline"
        TxtValue.Text = ""
        cmdClear.Text = "Clear"
        hidMode.Value = "a"
        lbl_add_mod.Text = "Please add the following value"

        'vldValues.Enabled = True
    End Sub

    Private Sub cmd_Delete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Delete.Click
        '//To prevent "No Record Found" msg
        txtsearch.Text = ""

        Dim objPersonal As New PersonalSetting
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim intIndex As String
        Dim i As Integer = 0
        For Each dgItem In dtgCustomField.Items

            chk = dgItem.FindControl("chkSelection")
            intIndex = dtgCustomField.DataKeys.Item(i)
            If chk.Checked Then
                objPersonal.delFieldValue(intIndex)
            End If
            i = i + 1
        Next
        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        viewstate("action") = "del"
        Bindgrid()
    End Sub

    Private Sub cmd_Modify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Modify.Click
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        'Dim strlistindex As String
        Dim i As Integer
        i = 0
        lbl_add_mod.Text = " Please modify the following value"
        ''---
        'custom.Style("display") = "inline"
        'value.Style("display") = "none"
        'vldValue.Visible = False
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
        '---
        custom.Style("display") = "inline"
        cmdClear.Text = "Reset"
        hidMode.Value = "m"
        For Each dgItem In dtgCustomField.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                hidIndex.Value = dtgCustomField.DataKeys.Item(i)
                TxtValue.Text = dgItem.Cells(1).Text
                'hidValue.Value = dgItem.Cells(1).Text
                ViewState("oldvalue") = dgItem.Cells(1).Text
                Exit For
            End If
            i = i + 1

        Next

    End Sub

    Private Sub TxtName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'vldValues.Enabled = True
    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        'vldValues.Enabled = True
        '//add by Moo,to prevent "no record found" message prompt after clicking save button
        txtsearch.Text = ""
        Dim objPersonal As New PersonalSetting
        Dim intMsgNo As Integer
        Dim strMsg As String

        If hidMode.Value = "a" Then
            intMsgNo = objPersonal.addValue(viewState("Value"), lblFieldName.Text.Trim, TxtValue.Text)
            Select Case intMsgNo
                Case WheelMsgNum.Save
                    strMsg = MsgRecordSave
                    TxtValue.Text = ""
                    'txtsearch.Text = ""
                Case WheelMsgNum.Duplicate
                    strMsg = MsgRecordDuplicate
                Case WheelMsgNum.NotSave
                    strMsg = MsgRecordNotSave
            End Select
            Bindgrid()
            'objAdmin = Nothing

        ElseIf hidMode.Value = "m" Then
            ' If hidValue.Value <> TxtName.Text Then
            intMsgNo = objPersonal.modCustomlist(viewState("Value"), lblFieldName.Text.Trim, hidIndex.Value, TxtValue.Text, ViewState("oldvalue"))
            TxtValue.Text = ViewState("oldvalue")

            Select Case intMsgNo
                Case WheelMsgNum.Save
                    strMsg = MsgRecordSave
                    TxtValue.Text = ""
                    custom.Style("display") = "none"
                    ViewState("oldvalue") = ""

                Case WheelMsgNum.Duplicate
                    strMsg = MsgRecordDuplicate
                    custom.Style("display") = ""
                Case WheelMsgNum.NotSave
                    strMsg = MsgRecordNotSave
                    custom.Style("display") = ""

            End Select

            'Else
            'Common.NetMsgbox(Me, "Not Change", MsgBoxStyle.Information, "Wheel")
            'End If
        End If
        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)

        Bindgrid()
    End Sub

    Private Sub cmd_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_search.Click
        Bindgrid(0)
    End Sub

    Private Sub cmd_clear1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_clear1.Click
        txtsearch.Text = ""
    End Sub

    Private Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        If hidMode.Value = "a" Then  ' add mode
            '  txtName.Text = ""
            TxtValue.Text = ""
        Else ' modify mode
            TxtValue.Text = ViewState("oldvalue")
        End If
    End Sub

    Private Sub dtgCustomField_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCustomField.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lbl As Label = e.Item.FindControl("lblSelection")
            Dim chk As CheckBox = e.Item.FindControl("chkSelection")

            If viewState("Mod") = "C" Then
                '//to add JavaScript to Check Box
                chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            ElseIf viewstate("Mod") = "P" Then
                'lbl.Text = "<input type=radio name='Myid' value='" & e.Item.Cells(1).Text & "' onclick=Chk('" & e.Item.Cells(1).Text & "')>"
                'lbl.Text = "<input type=radio name='Myid' value=""" & Server.UrlEncode(Common.parseNull(dv("FIELDVALUE"))) & """ onclick=Chk(""" & Server.UrlEncode(Common.parseNull(dv("FIELDVALUE"))) & """)>"
                lbl.Text = "<input type=radio name='Myid' value=""" & Server.UrlEncode(Common.parseNull(dv("CF_FIELD_VALUE"))) & """ onclick=Chk(""" & Server.UrlEncode(Common.parseNull(dv("CF_FIELD_VALUE"))) & """)>"
                chk.Visible = False
            End If
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        custom.Style("display") = "none"
        hidMode.Value = ""
    End Sub

    Public Sub SortCommandValues_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgCustomField.CurrentPageIndex = 0
        Bindgrid(0, True)
    End Sub
End Class
