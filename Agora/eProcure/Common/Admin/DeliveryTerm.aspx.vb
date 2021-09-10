Imports AgoraLegacy
Imports eProcure.Component


Public Class DeliveryTerm
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
    Protected WithEvents rbOversea As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents dtgDel As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ddl_Select As System.Web.UI.WebControls.DropDownList
    Protected WithEvents textDelCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents textDelName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_add_DelCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_add_DelName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_add_Factor As System.Web.UI.WebControls.TextBox
    Protected WithEvents Hide_Add2 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lbl_add_mod As System.Web.UI.WebControls.Label
    Protected WithEvents validate_del_code As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents validate_del_name As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents validate_grn_factor As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rev_grn_factor As System.Web.UI.WebControls.RegularExpressionValidator
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
            cmdDelete.Enabled = False
            cmdModify.Enabled = False
            'cmdReset.Disabled = True
        End If
        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgDel)
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

    Sub dtgDel_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgDel.PageIndexChanged
        dtgDel.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgDel.SortCommand
        Grid_SortCommand(sender, e)
        dtgDel.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim objSI As New Admin
        Dim ds As DataSet
        Dim objAdmin As New Admin

        ds = objAdmin.getDelTerm(textDelCode.Text, textDelName.Text)

        '//for sorting asc or desc
        Dim dvViewDel As DataView
        dvViewDel = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewDel.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewDel.Sort += " DESC"
        End If


        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            cmdModify.Enabled = True
            cmdDelete.Enabled = True
            resetDatagridPageIndex(dtgDel, dvViewDel)
            dtgDel.DataSource = dvViewDel
            dtgDel.DataBind()
            dtgDel.Visible = True
        Else
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            dtgDel.Visible = False
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00006"))
        End If

        ViewState("PageCount") = dtgDel.PageCount
    End Function

    Private Sub dtgDel_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDel.ItemCreated

        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgDel, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If

    End Sub

    Private Sub dtgDel_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDel.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            If IsDBNull(dv("CDT_DEL_GRNFACTOR")) Then
                e.Item.Cells(5).Text = "0"
            Else
                e.Item.Cells(5).Text = Format(dv("CDT_DEL_GRNFACTOR"), "###0.0000")
            End If

            If dv("CDT_DEL_OVERSEA") = "N" Then
                e.Item.Cells(6).Text = "No"
            Else
                e.Item.Cells(6).Text = "Yes"
            End If

        End If
    End Sub

    Private Sub cmd_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_search.Click
        dtgDel.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim dgItem As DataGridItem
        Dim chk As CheckBox

        Hide_Add2.Style("display") = "inline"
        validate_del_code.Enabled = True
        validate_del_name.Enabled = True
        validate_grn_factor.Enabled = True
        rev_grn_factor.Enabled = True
        txt_add_DelCode.Enabled = True
        txt_add_DelCode.Text = ""
        txt_add_DelName.Text = ""
        txt_add_Factor.Text = ""
        rbOversea.SelectedValue = "N"
        Me.lbl_add_mod.Text = "add"
        cmd_clear.Text = "Clear"
        hidMode.Value = "a"

        For Each dgItem In dtgDel.Items
            chk = dgItem.FindControl("chkSelection")
            chk.Checked = False
        Next

    End Sub

    Private Sub cmd_clear1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_clear1.Click

        textDelCode.Text = ""
        textDelName.Text = ""
    End Sub

    Private Sub cmd_clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_clear.Click
        If hidMode.Value = "a" Then
            txt_add_DelCode.Text = ""
            txt_add_DelName.Text = ""
            txt_add_Factor.Text = ""
            rbOversea.SelectedValue = "N"
        Else
            txt_add_DelCode.Text = ViewState("oldDelCode")
            txt_add_DelName.Text = ViewState("oldDelName")
            txt_add_Factor.Text = ViewState("oldDelFactor")
            rbOversea.SelectedValue = ViewState("oldDelOversea")
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
            intmsgno = objAdm.addDelTerm(txt_add_DelCode.Text, txt_add_DelName.Text, txt_add_Factor.Text, rbOversea.SelectedValue)
        ElseIf selected = "mod" Then
            Me.lbl_add_mod.Text = "modify"
            intmsgno = objAdm.modDelTerm(hidIndex.Value, txt_add_DelCode.Text, txt_add_DelName.Text, txt_add_Factor.Text, rbOversea.SelectedValue)
        End If

        Select Case intmsgno
            Case WheelMsgNum.Save
                Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
                Hide_Add2.Style("display") = "none"
                hidMode.Value = ""
                textDelCode.Text = ""
                textDelName.Text = ""
                Bindgrid(0)
            Case WheelMsgNum.NotSave
                Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                'Hide_Add2.Style("display") = "none"
                'hidMode.Value = ""
                'Bindgrid(0)
            Case WheelMsgNum.Duplicate
                Common.NetMsgbox(Me, "Delivery Code duplicated.", MsgBoxStyle.Information)
        End Select


    End Sub

    Private Sub cmdModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim i As Integer
        i = 0

        Hide_Add2.Style("display") = "inline"
        validate_del_code.Enabled = True
        validate_del_name.Enabled = True
        txt_add_DelCode.Enabled = False
        Me.lbl_add_mod.Text = "Modify"
        cmd_clear.Text = "Reset"
        hidMode.Value = "m"
        For Each dgItem In dtgDel.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                txt_add_DelCode.Text = dgItem.Cells(1).Text
                txt_add_DelName.Text = dgItem.Cells(4).Text
                txt_add_Factor.Text = dgItem.Cells(5).Text
                rbOversea.SelectedValue = IIf(dgItem.Cells(6).Text = "Yes", "Y", "N")
                hidIndex.Value = dgItem.Cells(2).Text
                ViewState("oldDelCode") = dgItem.Cells(1).Text
                ViewState("oldDelName") = dgItem.Cells(4).Text
                ViewState("oldDelFactor") = dgItem.Cells(5).Text
                ViewState("oldDelOversea") = IIf(dgItem.Cells(6).Text = "Yes", "Y", "N")
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
        For Each dgItem In dtgDel.Items
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
        Dim dtNewDel As New DataTable
        Dim drDel As DataRow
        dtNewDel.Columns.Add("Del_Code", Type.GetType("System.String")) '//delivery code
        dtNewDel.Columns.Add("Del_Index", Type.GetType("System.Int32")) '//delivery index

        For Each dgItem In dtgDel.Items
            Dim chkSelection As CheckBox = dgItem.Cells(0).FindControl("chkSelection")

            If chkSelection.Checked Then
                Dim deptcode As String = dgItem.Cells(1).Text
                drDel = dtNewDel.NewRow
                drDel("Del_Code") = dgItem.Cells(1).Text
                drDel("Del_Index") = dgItem.Cells(2).Text
                dtNewDel.Rows.Add(drDel)
            End If
        Next

        Dim objAdmin As New Admin
        intMsg = objAdmin.delDelTerm(dtNewDel)
        If intMsg = -99 Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00009") & " " & objGlobal.GetErrorMessage("00018"), MsgBoxStyle.Information)
        ElseIf intMsg = WheelMsgNum.Delete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00004"), MsgBoxStyle.Information)
            textDelCode.Text = ""
            textDelName.Text = ""
            Hide_Add2.Style("display") = "none"
            hidMode.Value = ""
            Bindgrid()
        ElseIf intMsg = WheelMsgNum.NotDelete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00008"), MsgBoxStyle.Information)
        End If
    End Sub
End Class
