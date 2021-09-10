Imports AgoraLegacy
Imports eProcure.Component


Public Class Section
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objAdm As New Admin
    Dim objGlobal As New AppGlobals

    Protected WithEvents cmd_search As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_cancel As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear1 As System.Web.UI.WebControls.Button
    Protected WithEvents dtgSec As System.Web.UI.WebControls.DataGrid
    Protected WithEvents textSecCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents textSecName As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddl_Select As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txt_add_SecCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_add_SecName As System.Web.UI.WebControls.TextBox
    Protected WithEvents Hide_Add2 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lbl_add_mod As System.Web.UI.WebControls.Label
    Protected WithEvents validate_Sec_code As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents validate_Sec_name As System.Web.UI.WebControls.RequiredFieldValidator
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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdAdd.Enabled = False
        cmdModify.Enabled = False
        cmdDelete.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdModify)
        htPageAccess.Add("update", alButtonList)
        CheckButtonAccess()
        '//additional checking
        If intPageRecordCnt > 0 Then
            cmdModify.Enabled = blnCanUpdate
            cmdDelete.Enabled = blnCanUpdate
            '//mean Enable, can't use button.Enabled because this is a HTML button
            'cmdReset.Disabled = False
        Else
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            'cmdReset.Disabled = True
        End If
        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgSec)
        If Not IsPostBack Then
            cmd_search.Enabled = True
            cmd_clear1.Enabled = True
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            Hide_Add2.Style("display") = "none"
        End If

        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")

    End Sub

    Sub dtgSec_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgSec.PageIndexChanged
        dtgSec.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgSec.SortCommand
        Grid_SortCommand(sender, e)
        dtgSec.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim ds As DataSet

        ds = objAdm.getSection(textSecCode.Text, textSecName.Text)

        '//for sorting asc or desc
        Dim dvViewSec As DataView
        dvViewSec = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewSec.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSec.Sort += " DESC"
        End If


        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            cmdModify.Enabled = True
            cmdDelete.Enabled = True
            resetDatagridPageIndex(dtgSec, dvViewSec)
            dtgSec.DataSource = dvViewSec
            dtgSec.DataBind()
            dtgSec.Visible = True
        Else
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            dtgSec.Visible = False
            'resetDatagridPageIndex(dtgSec, dvViewSec)
            'dtgSec.DataSource = dvViewSec
            'dtgSec.DataBind()
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00006"))
        End If

        ViewState("PageCount") = dtgSec.PageCount
    End Function

    Private Sub dtgSec_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgSec.ItemCreated

        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgSec, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If

    End Sub

    Private Sub dtgSec_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgSec.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
        End If
    End Sub

    Private Sub cmd_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_search.Click
        dtgSec.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim dgItem As DataGridItem
        Dim chk As CheckBox

        Hide_Add2.Style("display") = "inline"
        validate_Sec_code.Enabled = True
        validate_Sec_name.Enabled = True
        txt_add_SecCode.Enabled = True
        txt_add_SecCode.Text = ""
        txt_add_SecName.Text = ""
        Me.lbl_add_mod.Text = "add"
        cmd_clear.Text = "Clear"
        hidMode.Value = "a"

        For Each dgItem In dtgSec.Items
            chk = dgItem.FindControl("chkSelection")
            chk.Checked = False
        Next

    End Sub

    Private Sub cmd_clear1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_clear1.Click

        textSecCode.Text = ""
        textSecName.Text = ""
    End Sub

    Private Sub cmd_clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_clear.Click
        If hidMode.Value = "a" Then
            txt_add_SecCode.Text = ""
            txt_add_SecName.Text = ""
        Else
            txt_add_SecCode.Text = ViewState("oldSecCode")
            txt_add_SecName.Text = ViewState("oldSecName")
        End If

    End Sub

    Private Sub cmd_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_save.Click

        If hidMode.Value = "a" Then
            Me.view("add")
        ElseIf hidMode.Value = "m" Then
            Me.view("mod")
        End If

    End Sub

    Sub view(ByVal selected As String)
        Dim intmsgno As Integer

        If selected = "add" Then
            Me.lbl_add_mod.Text = "add"
            intmsgno = objAdm.AddSecCode(txt_add_SecCode.Text, txt_add_SecName.Text, "add", hidIndex.Value)
        ElseIf selected = "mod" Then
            Me.lbl_add_mod.Text = "modify"
            intmsgno = objAdm.AddSecCode(txt_add_SecCode.Text, txt_add_SecName.Text, "mod", hidIndex.Value)
        End If

        Select Case intmsgno
            Case WheelMsgNum.Save
                Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00003"), MsgBoxStyle.Information)
                textSecCode.Text = ""
                textSecName.Text = ""
                Bindgrid(0)
                Hide_Add2.Style("display") = "none"
                hidMode.Value = ""
            Case WheelMsgNum.NotSave
                Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00007"), MsgBoxStyle.Information)
                'Bindgrid(0)
                Hide_Add2.Style("display") = "none"
                hidMode.Value = ""
            Case -99
                Common.NetMsgbox(Me, "Section Code duplicated.", MsgBoxStyle.Information)
        End Select
    End Sub

    Private Sub cmdModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim i As Integer
        i = 0

        Hide_Add2.Style("display") = "inline"
        validate_Sec_code.Enabled = True
        validate_Sec_name.Enabled = True
        txt_add_SecCode.Enabled = False
        Me.lbl_add_mod.Text = "modify"
        cmd_clear.Text = "Reset"
        hidMode.Value = "m"
        For Each dgItem In dtgSec.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                txt_add_SecCode.Text = dgItem.Cells(1).Text
                txt_add_SecName.Text = dgItem.Cells(4).Text
                hidIndex.Value = dgItem.Cells(2).Text
                ViewState("oldSecCode") = dgItem.Cells(1).Text
                ViewState("oldSecName") = dgItem.Cells(4).Text
                Exit For
            End If
            i = i + 1
        Next

    End Sub

    Private Sub cmd_cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_cancel.Click
        Dim dgItem As DataGridItem
        Dim chk As CheckBox

        Hide_Add2.Style("display") = "none"
        hidMode.Value = ""

        For Each dgItem In dtgSec.Items
            chk = dgItem.FindControl("chkSelection")
            chk.Checked = False
        Next
    End Sub

    Private Sub ddl_Select_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Select.SelectedIndexChanged

        If ddl_Select.SelectedItem.Value = "1" Then
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

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim intMsg As Integer
        Dim dtNewSec As New DataTable
        Dim drSec As DataRow
        dtNewSec.Columns.Add("Sec_Code", Type.GetType("System.String")) '//product code
        dtNewSec.Columns.Add("Sec_Index", Type.GetType("System.Int32")) '//supplier id

        For Each dgItem In dtgSec.Items
            Dim chkSelection As CheckBox = dgItem.Cells(0).FindControl("chkSelection")

            If chkSelection.Checked Then
                Dim deptcode As String = dgItem.Cells(1).Text
                drSec = dtNewSec.NewRow
                drSec("Sec_Code") = dgItem.Cells(1).Text
                drSec("Sec_Index") = dgItem.Cells(2).Text
                dtNewSec.Rows.Add(drSec)
            End If
        Next

        Dim objAdmin As New Admin
        intMsg = objAdmin.delSecCode(dtNewSec)
        If intMsg = -99 Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00009") & " " & objGlobal.GetErrorMessage("00018"), MsgBoxStyle.Information)
        ElseIf intMsg = WheelMsgNum.Delete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00004"), MsgBoxStyle.Information)
            textSecCode.Text = ""
            textSecName.Text = ""
            Hide_Add2.Style("display") = "none"
            hidMode.Value = ""
            Bindgrid()
        ElseIf intMsg = WheelMsgNum.NotDelete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00008"), MsgBoxStyle.Information)
        End If

        
    End Sub
End Class
