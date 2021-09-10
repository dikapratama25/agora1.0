Imports AgoraLegacy
Imports eProcure.Component
Public Class Favs_ListMain
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents txtsearch As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmd_search As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear1 As System.Web.UI.WebControls.Button
    Protected WithEvents lbl_add_mod As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txt_add_mod As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmd_save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_cancel As System.Web.UI.WebControls.Button
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents vldFavList As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents MyDataGrid As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_delete As System.Web.UI.WebControls.Button
    Protected WithEvents hide As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents Div1 As System.Web.UI.HtmlControls.HtmlGenericControl

    Dim dDispatcher As New AgoraLegacy.dispatcher
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
        cmd_delete.Enabled = False
        cmdModify.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdModify)
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmd_delete)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        '//additional checking
        If intPageRecordCnt > 0 Then
            cmd_delete.Enabled = blnCanDelete
            cmdModify.Enabled = blnCanUpdate
            '//mean Enable, can't use button.Enabled because this is a HTML button
            cmdReset.Disabled = False
        Else
            cmd_delete.Enabled = False
            cmdModify.Enabled = False
            cmdReset.Disabled = True
        End If
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Session("CompanyId") = "demo"
        'Session("UserId") = "moofh"
        MyBase.Page_Load(sender, e)
        SetGridProperty(MyDataGrid)

        If Not Page.IsPostBack Then
            cmd_delete.Enabled = False
            cmdModify.Enabled = False
            cmdReset.Disabled = True
        End If
        cmd_delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        'Put user code to initialize the page here

    End Sub

    Public Sub MyDataGrid_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        MyDataGrid.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0, True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(MyDataGrid.CurrentPageIndex = 0, True)
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objRFQ As New PersonalSetting
        '   Dim objRFQ As String
        Dim ds As DataSet = New DataSet
        Dim record As String
        Dim strlistname As String

        strlistname = Me.txtsearch.Text

        ds = objRFQ.getfavlist(strlistname)

        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)
        'If pSorted Then
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        'End If

        ' MyDataGrid.DataSource = dvViewSample

        If viewstate("action") = "del" Then
            If MyDataGrid.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod MyDataGrid.PageSize = 0 Then
                MyDataGrid.CurrentPageIndex = MyDataGrid.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        'If MyDataGrid.CurrentPageIndex > 0 And (dvViewSample.Count \ MyDataGrid.PageSize) - 1 <= MyDataGrid.CurrentPageIndex Then
        '    MyDataGrid.CurrentPageIndex = 0
        'End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        If intPageRecordCnt > 0 Then

            ' check when user re-enter search criteria and click on other page without click search button
            If MyDataGrid.CurrentPageIndex > (dvViewSample.Count \ MyDataGrid.PageSize) Then
                MyDataGrid.CurrentPageIndex = IIf((dvViewSample.Count \ MyDataGrid.PageSize) = 1, 0, (dvViewSample.Count \ MyDataGrid.PageSize))
            ElseIf MyDataGrid.CurrentPageIndex = (dvViewSample.Count \ MyDataGrid.PageSize) Then
                If viewstate("PageCount") = (dvViewSample.Count \ MyDataGrid.PageSize) Then
                    'user does not re-enter search criteria 
                    MyDataGrid.CurrentPageIndex = IIf((dvViewSample.Count \ MyDataGrid.PageSize) = 0, 0, (dvViewSample.Count \ MyDataGrid.PageSize) - 1)
                Else
                    If (dvViewSample.Count Mod MyDataGrid.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        MyDataGrid.CurrentPageIndex = IIf((dvViewSample.Count \ MyDataGrid.PageSize) = 1, 0, (dvViewSample.Count \ MyDataGrid.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        MyDataGrid.CurrentPageIndex = (dvViewSample.Count \ MyDataGrid.PageSize)
                    End If
                End If
            End If
            '--------------------------------


            MyDataGrid.DataSource = dvViewSample
            MyDataGrid.DataBind()
        Else
            MyDataGrid.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ' add for above checking
        viewstate("PageCount") = MyDataGrid.PageCount
        objRFQ = Nothing
    End Function

    'Sub ShowStats()
    '    lblCurrentIndex.Text = record & " Record(s) Found"
    '    ' lblPageCount.Text = "Total Page is " & MyDataGrid.PageCount
    'End Sub

    Sub view(ByVal selected As String)
        Dim objPersonal As New PersonalSetting
        Dim strmsg As String
        Dim intmsgno As Integer
        'Dim strsqladdfavlist As String

        If selected = "add" Then
            ' Me.lbl_add_mod.Text = "Add"
            intmsgno = objPersonal.addfavlist(txt_add_mod.Text, "Y")

            Select Case intmsgno
                Case WheelMsgNum.Save
                    strmsg = MsgRecordSave
                    txt_add_mod.Text = ""
                    'hide.Style("display") = "none"


                Case WheelMsgNum.Duplicate
                    strmsg = MsgRecordDuplicate
                Case WheelMsgNum.NotSave
                    strmsg = MsgRecordNotSave
            End Select
            Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
            'If selected = "Add" Then
            '    Common.NetMsgbox(Me, "Record Saved!", MsgBoxStyle.OKOnly, "Wheel")
            'Else : Common.NetMsgbox(Me, "Record Not Saved!", MsgBoxStyle.OKCancel, "Wheel")
            'End If


        ElseIf selected = "mod" Then
            ' Me.lbl_add_mod.Text = "Modify"
            intmsgno = objPersonal.modfavlist(hidIndex.Value, txt_add_mod.Text, "Y", viewstate("oldvalue"))
            txt_add_mod.Text = viewstate("oldvalue")
            'Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information, "Wheel")

            'non-display the modify listing header 
            hide.Style("display") = "none"

            Select Case intmsgno
                Case WheelMsgNum.Save
                    strmsg = MsgRecordSave
                    txt_add_mod.Text = ""
                    hide.Style("display") = "none"
                    ViewState("oldvalue") = ""


                Case WheelMsgNum.Duplicate
                    strmsg = MsgRecordDuplicate
                    hide.Style("display") = ""
                Case WheelMsgNum.NotSave
                    strmsg = MsgRecordNotSave
                    hide.Style("display") = ""
            End Select
        End If
        Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
        Bindgrid()
        objPersonal = Nothing

    End Sub

    Private Sub cmd_add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        view("add")
    End Sub

    'Private Sub cmd_modify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    view("mod")
    'End Sub

    Private Sub cmd_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_search.Click
        MyDataGrid.CurrentPageIndex = 0
        Bindgrid(0)
    End Sub

    Private Sub cmd_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_save.Click
        '//To prevent "No Record Found" msg
        txtsearch.Text = ""

        If hidMode.Value = "a" Then
            Me.view("add")
        ElseIf hidMode.Value = "m" Then
            Me.view("mod")
        End If
        Bindgrid(0)
    End Sub

    Private Sub cmd_delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_delete.Click
        Dim objCo As New PersonalSetting
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim strlistindex As String
        Dim i As Integer = 0

        '//To prevent "No Record Found" msg
        txtsearch.Text = ""
        For Each dgItem In MyDataGrid.Items

            chk = dgItem.FindControl("chkSelection")
            strlistindex = MyDataGrid.DataKeys.Item(i)
            If chk.Checked Then
                objCo.delfavlist(strlistindex)
            End If
            i = i + 1
        Next

        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        viewstate("action") = "del"
        Bindgrid(0)
        objCo = Nothing

    End Sub

    'Private Sub cmdModify_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim dgItem As DataGridItem
    '    Dim chk As CheckBox
    '    Dim strlistindex As String

    '    For Each dgItem In MyDataGrid.Items
    '        chk = dgItem.FindControl("chkSelection")
    '        If chk.Checked Then
    '            strlistindex = MyDataGrid.DataKeys.Item(0)
    '        End If
    '    Next

    '    cmdModify.Attributes.Add("onclick", "Display(1);")
    'End Sub

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim strlistindex As String
        Dim i As Integer
        i = 0

        hide.Style("display") = ""
        cmd_clear.Text = "Reset"
        hidMode.Value = "m"
        For Each dgItem In MyDataGrid.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                hidIndex.Value = MyDataGrid.DataKeys.Item(i)
                txt_add_mod.Text = CType(dgItem.FindControl("lnkListName"), HyperLink).Text
                'viewstate("oldvalue") = dgItem.Cells(1).Text
                viewstate("oldvalue") = CType(dgItem.FindControl("lnkListName"), HyperLink).Text
                Exit For
            End If
            i = i + 1
        Next
        lbl_add_mod.Text = "Please modify the following value"
    End Sub

    Private Sub cmd_clear1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_clear1.Click
        Me.txtsearch.Text = ""
    End Sub

    Private Sub MyDataGrid_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles MyDataGrid.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(MyDataGrid, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub



    Private Sub cmd_cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_cancel.Click
        Me.hide.Style("display") = "none"
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click

        hide.Style("display") = ""
        cmd_clear.Text = "Clear"
        hidMode.Value = "a"
        txt_add_mod.Text = ""
        lbl_add_mod.Text = "Please add the following value"
        vldFavList.Enabled = True


    End Sub

    Private Sub cmd_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_clear.Click
        If hidMode.Value = "a" Then
            txt_add_mod.Text = ""
        Else
            txt_add_mod.Text = viewstate("oldvalue")

        End If

    End Sub

    Private Sub MyDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles MyDataGrid.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim chk As CheckBox
            chk = e.Item.FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            'If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            'to add JavaScript to Check Box

            'to dynamic build hyoerlink
            'Dim lnkListName As HyperLink
            'lnkListName = e.Item.FindControl("lnkListName")
            'lnkListName.NavigateUrl = "Favs_ItemList.aspx?listindex=" & dv("FLM_LIST_INDEX")

            'lnkListName.Text = dv("FLM_LIST_NAME")

            Dim lnkListName As HyperLink
            lnkListName = e.Item.FindControl("lnkListName")
            lnkListName.NavigateUrl = "javascript:;"
            lnkListName.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("PersonalSetting", "Favs_ItemList.aspx", "pageid=" & strPageId & "&listindex=" & dv("FLM_LIST_INDEX") & "')"))
            lnkListName.Text = dv("FLM_LIST_NAME")

            'Dim lnkProductCode As HyperLink
            'lnkProductCode = e.Item.Cells(EnumProduct.icProdCode).FindControl("lnkProductCode")
            'lnkProductCode.NavigateUrl = "javascript:;"
            'lnkProductCode.Attributes.Add("onclick", "return PopWindow('ProductDetail.aspx?pageid=" & strPageId & "&pid=" & _
            'dv("PM_PRODUCT_CODE") & "')")
            'lnkProductCode.Text = dv("PM_PRODUCT_CODE")
        End If
        '  End If

        ''If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
        'Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
        ''to add JavaScript to Check Box

        ''to dynamic build hyoerlink
        'Dim lnkListName As HyperLink
        'lnkListName = e.Item.FindControl("lnkListName")
        'lnkListName.NavigateUrl = "Favs_ItemList.aspx?userid=" & dv("UM_USER_ID")
        'lnkListName.Text = dv("FLM_LIST_NAME")
        'End If



    End Sub


End Class


