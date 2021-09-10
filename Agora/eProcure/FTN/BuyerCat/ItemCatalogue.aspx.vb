Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing
Public Class ItemCatalogue1
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Dim i As Integer = 0
    Dim objcat As New BuyerCat
    Dim value1 As String
    Dim strcode, strdesc As String
    Dim procode, soucode As String
    Dim intstate As String
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents lblcode As System.Web.UI.WebControls.Label
    Protected WithEvents lblname As System.Web.UI.WebControls.Label
    Protected WithEvents txtDesp As System.Web.UI.WebControls.TextBox
   
    Dim cbolist As New ListItem
    Dim objDb As New EAD.DBCom

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents dtgShopping As System.Web.UI.WebControls.DataGrid

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Enum IEnum
        'chkbox = 0
        'BSource = 1
        'PItemCode = 2
        'CBCItemCode = 3
        'CCoyName = 4
        'PProductDesc = 5
        'PUOM = 6
        'PCurrency = 7
        'PCost = 8
        'BProCode = 9
        'CEndDate = 10
        'CoyID = 11
        'CDGrpIndex = 12
        'Expired = 13
        chkbox = 0
        PItemCode = 1
        PProductDesc = 2
        CCommodityType = 3
        PUOM = 4
        BCItemIndex = 5

    End Enum
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        'cmd_Delete.Enabled = False
        'cmdDeleteContract.Enabled = False
        ''cmd_Add.Enabled = False
        'cmdAddContract.Enabled = False
        Dim alButtonList As ArrayList
        'alButtonList = New ArrayList
        'alButtonList.Add(cmd_Delete)
        'alButtonList.Add(cmdDeleteContract)
        'htPageAccess.Add("delete", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmd_Add)
        alButtonList.Add(cmdAddContract)
        htPageAccess.Add("add", alButtonList)
        'alButtonList = New ArrayList
        'alButtonList.Add(cmd_Add)
        'alButtonList.Add(cmdAddContract)
        'htPageAccess.Add("update", alButtonList)
        CheckButtonAccess()
        '//additional checking
        'cmd_Delete.Enabled = blnCanDelete And ViewState("blnCanDelete")
        If IsPostBack Then cmd_Add.Enabled = blnCanAdd
        'cmdDeleteContract.Enabled = blnCanDelete And ViewState("blnCanDeleteContract")

    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgItem)
        If Not IsPostBack Then
            'Michelle (25/10/2010)
            'ViewState("blnCanDelete") = True
            'ViewState("blnCanDeleteContract") = True
            'ViewState("valIndex") = Request.QueryString("index")
            cmd_Delete.Visible = False
            cmd_Add.Visible = False
            Me.trhid.Style("display") = "none"
            GenerateTab()
            BindBuyerCat()
            'If Session("Env") = "FTN" Then
            '    cmdAddContract.Visible = False
            '    cmdDeleteContract.Visible = False
            '    legend.Style("display") = "none"
            'End If
            cmdAddContract.Visible = False
            cmdDeleteContract.Visible = False
            legend.Style("display") = "none"

            'If ViewState("valIndex") <> "" Then
            '    ViewState("valCode") = Request.QueryString("code")
            '    ViewState("valName") = Request.QueryString("name")
            '    lblcode.Text = ViewState("valCode")
            '    lblname.Text = ViewState("valName")

            'Bindgrid(0, True)
            'End If

        End If
        cmd_Delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")

        lnkBack.NavigateUrl = dDispatcher.direct("BuyerCat", "BuyerCatalogue.aspx", "pageid=" & strPageId)
    End Sub
    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgItem.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub
    Public Sub MyDataGrid_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgItem.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub
    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String

        Dim ds As DataSet = New DataSet
        Dim objBUYERCAT As New BuyerCat

        If Me.cboCatalogueBuyer.SelectedItem.Text <> "---Select---" Then
            ds = objBUYERCAT.getBuyerCatItems(cboCatalogueBuyer.SelectedItem.Value)
        Else
            ds = objBUYERCAT.getBuyerCatItemsByCombo("0")
        End If

        Dim dvViewItem As DataView
        dvViewItem = ds.Tables(0).DefaultView

        dvViewItem.Sort = viewstate("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewItem.Sort += " DESC"

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        If ViewState("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtgItem, dvViewItem)

            If Me.cboCatalogueBuyer.SelectedItem.Text = "Default Purchaser Catalogue" Then
                cmd_Delete.Visible = False
                cmd_Add.Visible = False
                Me.trhid.Style("display") = "none"
            Else
                Me.trhid.Style("display") = ""
                cmd_Delete.Enabled = True
                cmd_Delete.Visible = True
                cmd_Add.Enabled = True
            End If
            ViewState("blnCanDelete") = True
            cmd_Reset.Disabled = False
            Label4.Visible = True
            dtgItem.DataSource = dvViewItem
            dtgItem.DataBind()
            'Michelle (25/10/2010) - To remove the 'No record' message
        Else
            If Me.cboCatalogueBuyer.SelectedItem.Text = "Default Purchaser Catalogue" Then
                cmd_Delete.Visible = False
                cmd_Add.Visible = False
                Me.trhid.Style("display") = "none"
            End If
            cmd_Delete.Enabled = False
            Label4.Visible = False
            dtgItem.DataBind()
        End If

        ' add for above checking
        ViewState("PageCount") = dtgItem.PageCount
    End Function
    Private Function BindBuyerCat()
        Dim dv As DataView
        dv = objcat.getBuyerCat
        cboCatalogueBuyer.Items.Clear()

        If Not dv Is Nothing Then
            cboCatalogueBuyer.Enabled = True
            Common.FillDdl(cboCatalogueBuyer, "name", "BCM_CAT_INDEX", dv)
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboCatalogueBuyer.Items.Insert(0, cbolist)
    End Function
    Private Sub cmd_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Delete.Click
        Dim dgItem As DataGridItem
        Dim dt As New DataTable
        Dim dr As DataRow

        dt.Columns.Add("index", Type.GetType("System.String")) '//BCI_ITEM_INDEX
        'dt.Columns.Add("index", Type.GetType("System.String")) '//product code
        'dt.Columns.Add("code", Type.GetType("System.String")) '//supplier id
        'dt.Columns.Add("source", Type.GetType("System.String")) '//Vendor ITem Code

        Dim chk As CheckBox
        value1 = viewstate("valIndex")
        For Each dgItem In dtgItem.Items
            chk = dgItem.FindControl("chkSelection")

            If chk.Checked Then
                dr = dt.NewRow
                'dr("index") = ViewState("valIndex")
                dr("index") = dgItem.Cells.Item(IEnum.BCItemIndex).Text
                'dr("code") = dgItem.Cells.Item(IEnum.BProCode).Text
                dt.Rows.Add(dr)
                'objCo.delBuyerItem(value1, procode, soucode)
            End If
        Next

        'If objCo.delBuyerItem(dt) = WheelMsgNum.Delete Then
        If objcat.delBuyerItem(dt) = WheelMsgNum.Delete Then
            Common.NetMsgbox(Me, MsgRecordRemoved, MsgBoxStyle.Information)
        Else
            Common.NetMsgbox(Me, MsgRecordNotRemoved, MsgBoxStyle.Information)
        End If

        ViewState("action") = "del"
        Bindgrid(0)
    End Sub
    'Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
    '    dtgItem.CurrentPageIndex = 0
    '    Bindgrid(0)
    'End Sub
    'Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
    '    Me.txtCode.Text = ""
    '    Me.txtDesp.Text = ""
    'End Sub

    Private Sub dtgItem_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgItem, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub
    Private Sub dtgItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim valdate As Date = Now.Today
            Dim bln As Boolean
        End If
    End Sub
    'Private Sub lnkMapItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkMapItem.Click
    '    Dim strFileName As String

    '    Dim strscript As New System.Text.StringBuilder
    '    strscript.Append("<script language=""javascript"">")
    '    strFileName = "../Catalogue/ListPriceCatalogue.aspx?Frm=ItemMapping"
    '    strFileName = Server.UrlEncode(strFileName)
    '    strscript.Append("ShowDialog('Dialog.aspx?page=" & strFileName & "','530px');")
    '    strscript.Append("</script>")
    '    RegisterStartupScript("script3", strscript.ToString())
    'End Sub

    Private Sub cmd_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Add.Click
        ' Response.Redirect("../Product/SearchCatalogue.aspx?type=B&id=" & cboCatalogueItem.SelectedItem.Value)
        Dim strFileName As String

        Session("strPageId") = strPageId
        'strFileName = Server.UrlEncode(strFileName)
        'strscript.Append("ShowDialog('Dialog.aspx?page=" & strFileName & "','530px');")

        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        'strscript.Append("ShowDialog('Dialog.aspx?page=" & Server.UrlEncode("SearchBItem.aspx?Frm=ItemCat&BCIdx=" + cboCatalogueBuyer.SelectedItem.Value) & "','530px');")
        strFileName = dDispatcher.direct("BuyerCat", "SearchBItem.aspx", "Frm=" & "ItemCat" & "&BCIdx=" & cboCatalogueBuyer.SelectedItem.Value & "&pageid=" & strPageId)
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','530px');")
        strscript.Append("document.getElementById('btnHidden').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())

        'Response.Redirect("SearchBItem.aspx?BCIdx=" & cboCatalogueBuyer.SelectedItem.Value & "&Frm=ItemCat")
    End Sub

    'Private Sub cmdBuyerAsg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBuyerAsg.Click
    '    Response.Redirect("BuyerCat_AsignBuyer.aspx?index=" & viewstate("valIndex") & "&code=" & Server.UrlEncode(viewstate("valCode")) & "&name=" & Server.UrlEncode(viewstate("valName")) & "&pageid=" & strPageId)
    'End Sub

    'Private Sub cmdAddContract_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddContract.Click
    '    Response.Redirect("AddBuyerContractItem.aspx?pageid=" & strPageId & "&mode=A&Index=" & viewstate("valIndex") & "&code=" & Server.UrlEncode(viewstate("valCode")) & "&name=" & Server.UrlEncode(viewstate("valName")))
    'End Sub

    'Private Sub cmdDeleteContract_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDeleteContract.Click
    '    Response.Redirect("AddBuyerContractItem.aspx?pageid=" & strPageId & "&mode=D&Index=" & viewstate("valIndex") & "&code=" & Server.UrlEncode(viewstate("valCode")) & "&name=" & Server.UrlEncode(viewstate("valName")))
    'End Sub
    Private Sub cboCatalogueBuyer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCatalogueBuyer.SelectedIndexChanged
        If cboCatalogueBuyer.SelectedItem.Value <> "" Then
            cmd_Add.Visible = True
            cmd_Add.Enabled = True
            cmd_Delete.Visible = True
            cmd_Delete.Enabled = True
            Me.trhid.Style("display") = ""
            ViewState("SortAscending") = "yes"
            ViewState("SortExpression") = "PM_VENDOR_ITEM_CODE"
            Bindgrid(0)
        Else
            cmd_Add.Visible = False
            cmd_Delete.Visible = False
            Label4.Visible = False
            Bindgrid()
        End If

    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_ItemCatalogue_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("BuyerCat", "BuyerCatalogue.aspx", "pageid=" & strPageId) & """><span>Purchaser Catalogue</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("BuyerCat", "ItemCatalogue.aspx", "pageid=" & strPageId) & """><span>Items Assignment</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("BuyerCat", "BuyerCat_AsignBuyer.aspx", "pageid=" & strPageId) & """><span>Purchaser Assignment</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                      "</ul><div></div></div>"

        'Session("w_ItemCatalogue_tabs") = "<div class=""t_entity"">" & _
        '     "<a class=""t_entity_btn"" href=""" & dDispatcher.direct("BuyerCat", "BuyerCatalogue.aspx", "pageid=" & strPageId) & """><span>Purchaser Catalogue</span></a>" & _
        '     "<a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("BuyerCat", "ItemCatalogue.aspx", "pageid=" & strPageId) & """><span>Items Assignment</span></a>" & _
        '     "<a class=""t_entity_btn"" href=""" & dDispatcher.direct("BuyerCat", "BuyerCat_AsignBuyer.aspx", "pageid=" & strPageId) & """><span>Purchaser Assignment</span></a>" & _
        '      "</div>"


    End Sub

    Public Sub btnHidden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHidden.Click
        Bindgrid(0)
    End Sub
End Class
