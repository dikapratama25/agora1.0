Imports AgoraLegacy
Imports eProcure.Component

Public Class BuyerCatalogue

    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim cbolist As New ListItem

    Protected WithEvents cboCatalogueBuyer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmd_search As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear1 As System.Web.UI.WebControls.Button
    Protected WithEvents lbl_add_mod As System.Web.UI.WebControls.Label
    Protected WithEvents txt_add_mod As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_add_mod2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmd_save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_cancel As System.Web.UI.WebControls.Button
    Protected WithEvents MyDataGrid As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmd_delete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents hide As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    'Protected WithEvents rfv_code As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_cat_name As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    ' Protected WithEvents txtsearch2 As System.Web.UI.WebControls.TextBox
    ' Protected WithEvents txt_add_mod2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Div1 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents cmdBuyerAsg As System.Web.UI.WebControls.Button
    Protected WithEvents cmdItemAsg As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Dim objBuyerCat As New BuyerCat
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
    Public Enum BEnum
        'Michelle (22/10/2010)
        'EBCM_Code = 1
        'EBCM_Desc = 2
        'EBCM_Buyer = 3
        'EBCM_Index = 4
        EBCM_Desc = 1
        EBCM_Buyer = 2
        EBCM_Index = 3
    End Enum

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
            'cmdAdd.Enabled = blnCanAdd
            cmd_delete.Enabled = blnCanDelete
            cmdModify.Enabled = blnCanUpdate
            '//mean Enable, can't use button.Enabled because this is a HTML button
            cmdReset.Disabled = False
        Else
            'cmdAdd.Enabled = False
            cmd_delete.Enabled = False
            cmdModify.Enabled = False
            cmdReset.Disabled = True
        End If
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        'Session("CompanyId") = "demo"
        'Session("UserId") = "moo"
        SetGridProperty(MyDataGrid)

        If Not Page.IsPostBack Then
            viewstate("from") = Request.UrlReferrer
            'cmdAdd.Enabled = False
            GenerateTab()
            cmdBuyerAsg.Enabled = False
            cmdItemAsg.Enabled = False
            cmdModify.Enabled = False
            cmd_delete.Enabled = False
            cmdReset.Disabled = True
            BindBuyerCat()
            MyDataGrid.CurrentPageIndex = 0
            Bindgrid(0)

        End If
        cmd_delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        'cmdBuyerAsg.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        'cmdItemAsg.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        '  Common.SelDdl(ViewState("valId"), cboCatalogueBuyer)
        '  cboCatalogueBuyer.SelectedIndex = 0
    End Sub

    Public Sub MyDataGrid_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        MyDataGrid.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0, True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(MyDataGrid.CurrentPageIndex, True)
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim ds As DataSet = New DataSet
        Dim record As String
        '       Dim bcmgroupcode As String
        Dim bcmgroupdesc As String

        '       bcmgroupcode = Me.txtsearch.Text
        'bcmgroupdesc = Me.txtsearch2.Text
        bcmgroupdesc = cboCatalogueBuyer.SelectedItem.Text
        If cboCatalogueBuyer.SelectedItem.Value = "" Then bcmgroupdesc = ""

        'Michelle (22/10/2010) - Search by company id & bcmgroupdesc
        ' ds = objBuyerCat.getbuyercatmain(bcmgroupcode, bcmgroupdesc)
        ds = objBuyerCat.getbuyercatmain("", bcmgroupdesc)

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView

        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        If ViewState("action") = "del" Then
            If MyDataGrid.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod MyDataGrid.PageSize = 0 Then
                MyDataGrid.CurrentPageIndex = MyDataGrid.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If
        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt
        If intPageRecordCnt > 0 Then

            resetDatagridPageIndex(MyDataGrid, dvViewSample)
            'cmdAdd.Enabled = True
            cmdBuyerAsg.Enabled = True
            cmdItemAsg.Enabled = True
            MyDataGrid.DataSource = dvViewSample
            MyDataGrid.DataBind()
        Else
            'cmdAdd.Enabled = False
            cmdBuyerAsg.Enabled = False
            cmdItemAsg.Enabled = False
            cmd_delete.Enabled = False
            cmdModify.Enabled = False
            cmdReset.Disabled = True
            MyDataGrid.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ' add for above checking
        ViewState("PageCount") = MyDataGrid.PageCount
    End Function
    Private Function BindBuyerCat()
        Dim dv As DataView
        dv = objBuyerCat.getBuyerCat
        cboCatalogueBuyer.Items.Clear()

        If Not dv Is Nothing Then
            cboCatalogueBuyer.Enabled = True
            Common.FillDdl(cboCatalogueBuyer, "name", "BCM_CAT_INDEX", dv)
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboCatalogueBuyer.Items.Insert(0, cbolist)
    End Function
    Sub view(ByVal selected As String)
        Dim objPersonal As New BuyerCat
        Dim strRedirect As String
        Dim strmsg As String
        Dim intmsgno As Integer
        Dim getindex As String
        Dim strvalchk As String
        If selected = "add" Then
            Me.lbl_add_mod.Text = "add"

            'Michelle (22/10/2010) 
            'intmsgno = objPersonal.addbuyercatmain(txt_add_mod.Text, txt_add_mod2.Text)
            intmsgno = objPersonal.addbuyercatmain("", txt_add_mod2.Text)
            getindex = objPersonal.getindex()
            Select Case intmsgno
                Case WheelMsgNum.Save
                    'Michelle (22/10/2010) 
                    'strRedirect = "BuyerCat_AsignBuyer.aspx?code=" & txt_add_mod2.Text & "&name=" & txt_add_mod2.Text & "&index=" & getindex & "&pageid=" & strPageId
                    '&chk=" & strvalchk
                    strmsg = MsgRecordSave
                    'txt_add_mod.Text = ""
                    txt_add_mod2.Text = ""
                    'Common.NetPrompt(Me, MsgRecordSave, MsgBoxStyle.Information)  & """& vbCrLf & ""Proceed to Buyer Assignment?", strRedirect)
                    Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
                Case WheelMsgNum.Duplicate
                    strmsg = MsgRecordDuplicate
                    Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
                Case WheelMsgNum.NotSave
                    strmsg = MsgRecordNotSave
                    Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
            End Select
            '
        ElseIf selected = "mod" Then
            Me.lbl_add_mod.Text = "modify"
            'intmsgno = objPersonal.modbuyercatmain(hidIndex.Value, txt_add_mod.Text, txt_add_mod2.Text, ViewState("oldvalue"), ViewState("oldvalue2"))
            intmsgno = objPersonal.modbuyercatmain(hidIndex.Value, "", txt_add_mod2.Text, ViewState("oldvalue"), ViewState("oldvalue2"))

        Select Case intmsgno
            Case WheelMsgNum.Save
                    'strRedirect = "BuyerCat_AsignBuyer.aspx?code=" & txt_add_mod.Text & "&name=" & txt_add_mod2.Text & "&index=" & hidIndex.Value & "&pageid=" & strPageId
                    'strRedirect = "BuyerCat_AsignBuyer.aspx?name=" & txt_add_mod2.Text & "&index=" & hidIndex.Value & "&pageid=" & strPageId
                    strmsg = MsgRecordSave
                    'txt_add_mod.Text = ""
                    ViewState("oldvalue") = ""
                    ViewState("oldvalue2") = ""
                    hide.Style("display") = "none"
                    Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
                    '     Common.NetPrompt(Me, MsgRecordSave & """& vbCrLf & ""Proceed to Buyer Assignment?", strRedirect)
            Case WheelMsgNum.Duplicate
                    '   txt_add_mod.Text = viewstate("oldvalue")
                    txt_add_mod2.Text = ViewState("oldvalue2")
                    strmsg = MsgRecordDuplicate
                    Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
                'Hide_Add2.Style("display") = ""
            Case WheelMsgNum.NotSave
                    strmsg = MsgRecordNotSave
                    Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
                'Hide_Add2.Style("display") = ""
        End Select
            End If
    End Sub

    Private Sub cmd_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_search.Click
        MyDataGrid.CurrentPageIndex = 0
        Bindgrid(0)
    End Sub

    Private Sub cmd_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_save.Click
        '//To prevent "No Record Found" msg

        If hidMode.Value = "a" Then
            Me.view("add")
        ElseIf hidMode.Value = "m" Then
            Me.view("mod")
        End If
        Bindgrid(0)
        BindBuyerCat()
    End Sub

    Private Sub cmd_delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_delete.Click
        Dim objCo As New BuyerCat
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim strbcmindex As String
        Dim i As Integer = 0

        '//To prevent "No Record Found" msg

        For Each dgItem In MyDataGrid.Items

            chk = dgItem.FindControl("chkSelection")
            strbcmindex = MyDataGrid.DataKeys.Item(i)
            If chk.Checked Then
                'Don't allow user to delete the Default Purchaser Catalogue
                If dgItem.Cells(BEnum.EBCM_Desc).Text = "Default Purchaser Catalogue" Then
                    Common.NetMsgbox(Me, "Default Purchaser Catalogue cannot be deleted.", MsgBoxStyle.Information)
                    Exit For
                Else
                    objCo.delbuyercatmain(strbcmindex)
                End If
            End If
            i = i + 1
        Next

        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        viewstate("action") = "del"
        Bindgrid(0)
        BindBuyerCat()
    End Sub

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim strbcmindex As String
        Dim i As Integer
        i = 0

        hide.Style("display") = ""
        hidMode.Value = "m"
        Me.cmd_delete.Style("display") = ""
        cmd_clear.Text = "Reset"
        For Each dgItem In MyDataGrid.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                If dgItem.Cells(BEnum.EBCM_Desc).Text = "Default Purchaser Catalogue" Then
                    Common.NetMsgbox(Me, "Default Purchaser Catalogue cannot be modified.", MsgBoxStyle.Information)
                    Exit For
                End If

                hidIndex.Value = dgItem.Cells(BEnum.EBCM_Index).Text
                '  txt_add_mod.Text = dgItem.Cells(BEnum.EBCM_Code).Text
                txt_add_mod2.Text = dgItem.Cells(BEnum.EBCM_Desc).Text
                '  viewstate("oldvalue") = dgItem.Cells(BEnum.EBCM_Code).Text
                ViewState("oldvalue2") = dgItem.Cells(BEnum.EBCM_Desc).Text
                Exit For
            End If
            i = i + 1
        Next
        Me.lbl_add_mod.Text = "modify"
        cmdModify.Enabled = True

    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        hide.Style("display") = ""
        hidMode.Value = "a"
        '  txt_add_mod.Text = ""
        txt_add_mod2.Text = ""
        Me.lbl_add_mod.Text = "add"
        cmd_clear.Text = "Clear"
        ' Me.rfv_code.Enabled = True
        Me.rfv_cat_name.Enabled = True
    End Sub

    Private Sub cmd_clear1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_clear1.Click
        cboCatalogueBuyer.SelectedIndex = 0
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
        Me.hide.Style("Display") = "none"
    End Sub
    Private Sub cmd_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_clear.Click
        If hidMode.Value = "a" Then
            '  txt_add_mod.Text = ""
            txt_add_mod2.Text = ""
        Else
            ' txt_add_mod.Text = viewstate("oldvalue")
            txt_add_mod2.Text = viewstate("oldvalue2")
        End If
    End Sub

    Private Sub MyDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles MyDataGrid.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim chk As CheckBox
            chk = e.Item.FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            '//to get buyer
            Dim dvBuyerCat As DataView
            Dim lblBuyer As Label = e.Item.Cells(BEnum.EBCM_Buyer).FindControl("lblbuyer")
            dvBuyerCat = objBuyerCat.bindlistbox_BuyerCatSelectedData(e.Item.Cells(BEnum.EBCM_Index).Text)
            If Not dvBuyerCat Is Nothing Then
                Dim intLoop, intCnt As Integer
                intCnt = dvBuyerCat.Count
                For intLoop = 0 To intCnt - 1
                    If intLoop = 0 Then
                        lblBuyer.Text = Common.parseNull(dvBuyerCat(intLoop)("UM_USER_NAME"))
                    Else
                        lblBuyer.Text = lblBuyer.Text & "<br>" & Common.parseNull(dvBuyerCat(intLoop)("UM_USER_NAME"))
                    End If
                Next
            Else
                lblBuyer.Text = "-"
            End If
            'ElseIf e.Item.ItemType = ListItemType.Header Then
            '    e.Item.Cells(3).Text = "Buyer"
        End If
    End Sub

    'Michelle (22/10/2010) - Remove the button
    'Private Sub cmdBuyerAsg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBuyerAsg.Click

    '    Dim dgItem As DataGridItem
    '    Dim strIndex, strcode, strname As String
    '    Dim chk As CheckBox
    '    Dim i As Integer
    '    For Each dgItem In MyDataGrid.Items

    '        chk = dgItem.FindControl("chkSelection")
    '        strIndex = dgItem.Cells.Item(BEnum.EBCM_Index).Text
    '        '   strcode = dgItem.Cells.Item(BEnum.EBCM_Code).Text
    '        strname = dgItem.Cells.Item(BEnum.EBCM_Desc).Text
    '        If chk.Checked Then
    '            'Response.Redirect("BuyerCat_AsignBuyer.aspx?index=" & strIndex & "&code=" & Server.UrlEncode(strcode) & "&name=" & Server.UrlEncode(strname) & "&pageid=" & strPageId)
    '            Response.Redirect("BuyerCat_AsignBuyer.aspx?index=" & strIndex & "&name=" & Server.UrlEncode(strname) & "&pageid=" & strPageId)
    '        End If
    '    Next
    'End Sub

    'Michelle (22/10/2010 - Remove the button
    'Private Sub cmdItemAsg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdItemAsg.Click
    '    Dim dgItem As DataGridItem
    '    Dim strIndex, strcode, strname As String
    '    Dim chk As CheckBox
    '    Dim i As Integer
    '    For Each dgItem In MyDataGrid.Items
    '        chk = dgItem.FindControl("chkSelection")
    '        strIndex = dgItem.Cells.Item(BEnum.EBCM_Index).Text
    '        'strcode = dgItem.Cells.Item(BEnum.EBCM_Code).Text
    '        strname = dgItem.Cells.Item(BEnum.EBCM_Desc).Text
    '        If chk.Checked Then
    '            'Response.Redirect("ItemCatalogue.aspx?index=" & strIndex & "&code=" & Server.UrlEncode(strcode) & "&name=" & Server.UrlEncode(strname) & "&pageid=" & strPageId)
    '            Response.Redirect("ItemCatalogue.aspx?index=" & strIndex & "&name=" & Server.UrlEncode(strname) & "&pageid=" & strPageId)
    '        End If
    '    Next
    'End Sub

    Private Sub cboCatalogueBuyer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCatalogueBuyer.SelectedIndexChanged
        Bindgrid(0)
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_BuyerCatalogue_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("BuyerCat", "BuyerCatalogue.aspx", "pageid=" & strPageId) & """><span>Purchaser Catalogue</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("BuyerCat", "ItemCatalogue.aspx", "pageid=" & strPageId) & """><span>Items Assignment</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("BuyerCat", "BuyerCat_AsignBuyer.aspx", "pageid=" & strPageId) & """><span>Purchaser Assignment</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
        'Session("w_BuyerCatalogue_tabs") = "<div class=""t_entity"">" & _
        '     "<a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("BuyerCat", "BuyerCatalogue.aspx", "pageid=" & strPageId) & """><span>Purchaser Catalogue</span></a>" & _
        '     "<a class=""t_entity_btn"" href=""" & dDispatcher.direct("BuyerCat", "ItemCatalogue.aspx", "pageid=" & strPageId) & """><span>Items Assignment</span></a>" & _
        '     "<a class=""t_entity_btn"" href=""" & dDispatcher.direct("BuyerCat", "BuyerCat_AsignBuyer.aspx", "pageid=" & strPageId) & """><span>Purchaser Assignment</span></a>" & _
        '     "</div>"

    End Sub

End Class


