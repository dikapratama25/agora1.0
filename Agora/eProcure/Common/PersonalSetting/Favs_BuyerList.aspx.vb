Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing

Public Class Favs_BuyerList
    Inherits AgoraLegacy.AppBaseClass
    Dim ds As DataSet
    Dim valTsource As String
    Dim objDb As New EAD.DBCom
    Dim objCo As New BuyerCat
    Dim value1, value2 As String
    'Dim intTotPage As Integer
    'Dim intTotRecord As Int32 = 0
    Dim cbolist As New ListItem
    Dim cbolistfav As New ListItem
    Dim blnSetExpired_B As Boolean
    Dim blnSetExpired_F As Boolean
    Dim strvalF, strvalB As String
    Dim objProduct As New Products
    Dim intstate As String
    Dim valvendor As String
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents cboCatalogueBuyer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboCatalogueFav As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents dtgBuyer As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtgFavs As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmd_Add As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Remove As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_RemoveAll As System.Web.UI.WebControls.Button
    Protected WithEvents Div_Buyer As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents rdSearch As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidDelete As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Clear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Div_Fav As System.Web.UI.HtmlControls.HtmlGenericControl
	Protected WithEvents txtVendorItem As System.Web.UI.WebControls.TextBox
	Protected WithEvents txtBuyerItem As System.Web.UI.WebControls.TextBox
	Protected WithEvents txtVendorName As System.Web.UI.WebControls.TextBox

    Dim blnUnavailable As Boolean ' true if unavailable items exist
  
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
    Public Enum FEnum
        chkbox = 0
        Qty = 1
        BSource = 2
        PItemCode = 3
        CBCItemCode = 4
        CoyName = 5
        ProductDesc = 6
        UOM = 7
        Currency = 8
        Cost = 9
        ProCode = 10
        EndDate = 11
        CoyID = 12
        CDGrpIndex = 13
        Expired = 14
    End Enum
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
       

        SetGridProperty(dtgBuyer)
        SetGridProperty(dtgFavs)

        If Not IsPostBack Then

            '//THIS FIRED WHEN GOT QUERY STRING
            viewstate("valType") = Me.Request.QueryString("type")
            'viewstate("valType") = "F"
            viewstate("valId") = Me.Request.QueryString("id")

           
            cmd_Remove.Enabled = False
            cmd_RemoveAll.Enabled = False
            cmd_Add.Enabled = False
            BindBuyerCat()
            BindFav()

            '//add by Moo
            If viewstate("valType") = "" Then
                ' rdSearch.SelectedValue = "F" - Michelle (CR0011)
                rdSearch.SelectedValue = "B"
            ElseIf viewstate("valType") = "F" Then
                rdSearch.SelectedValue = "F"
                Common.SelDdl(viewstate("valId"), cboCatalogueFav)
            ElseIf viewstate("valType") = "B" Then
                rdSearch.SelectedValue = "B"
                Common.SelDdl(viewstate("valId"), cboCatalogueBuyer)
            End If
        End If

        '//THIS FIRED WHEN USER INTERACTION
        If rdSearch.SelectedValue = "F" Then
            strvalF = rdSearch.SelectedValue
            cboCatalogueBuyer.Enabled = False
            cboCatalogueFav.Enabled = True
            cboCatalogueBuyer.SelectedIndex = 0
            cmd_Remove.Enabled = False
            cmd_RemoveAll.Enabled = False
            cmd_Add.Enabled = False
            dtgBuyer.DataBind()
        End If

        If rdSearch.SelectedValue = "B" Then
            strvalB = rdSearch.SelectedValue
            cboCatalogueFav.Enabled = False
            cboCatalogueBuyer.Enabled = True
            cboCatalogueFav.SelectedIndex = 0
            cmd_Remove.Enabled = False
            cmd_RemoveAll.Enabled = False
            cmd_Add.Enabled = False
            dtgFavs.DataBind()
        End If

        '//add by Moo
        If Not IsPostBack Then
            If rdSearch.SelectedValue = "F" Then
                '//to cater redirection from catalogue search
                favselected()
            ElseIf rdSearch.SelectedValue = "B" Then
                '//to cater redirection from catalogue search
                buyerselected()
            End If
        End If

        '//THIS IS DELETION FOR EXP PRODUCT ONE BY ONE(LINKBUTTON)
        If hidIndex.Value <> "" Then
            linkDel_FAV(hidIndex.Value)
            hidIndex.Value = ""
        End If
        cmd_RemoveAll.Attributes.Add("onclick", "return confirm('" & MsgForDeleteButton & "');")
    End Sub
    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        value1 = cboCatalogueBuyer.SelectedItem.Value
        Dim ds As DataSet = New DataSet
        Dim objBUYERCAT As New BuyerCat
        Dim record As String
        Dim strget As String
        AssignDesc()
        ds = objBUYERCAT.getBuyerCat_CDP(value1, "", viewstate("valdesc"))
        Dim dvViewItem As DataView
        dvViewItem = ds.Tables(0).DefaultView
		dvViewItem.Sort = viewstate("SortExpression")


		Dim filter As String = ""
		If Me.txtVendorItem.Text <> "" Then
			filter = filter & "PM_VENDOR_ITEM_CODE LIKE '%" & Me.txtVendorItem.Text & "%'"
		End If

		If Me.txtBuyerItem.Text <> "" Then
			If Not filter.Equals("") Then
				filter = filter & " AND "
			End If
			filter = filter & "CBC_B_ITEM_CODE LIKE '%" & Me.txtBuyerItem.Text & "%'"
		End If

		If Me.txtVendorName.Text <> "" Then
			If Not filter.Equals("") Then
				filter = filter & " AND "
			End If
			filter = filter & "CM_COY_NAME LIKE '%" & Me.txtVendorName.Text & "%'"
		End If

		dvViewItem.RowFilter = filter

		If viewstate("SortAscending") = "no" Then dvViewItem.Sort += " DESC"

		If viewstate("action") = "del" Then
			If dtgBuyer.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgBuyer.PageSize = 0 Then
				dtgBuyer.CurrentPageIndex = dtgBuyer.CurrentPageIndex - 1
				viewstate("action") = ""
			End If
		End If
		intPageRecordCnt = dvViewItem.Count
		viewstate("intPageRecordCnt") = intPageRecordCnt

		If intPageRecordCnt > 0 Then
			resetDatagridPageIndex(dtgBuyer, dvViewItem)
			'cmd_Remove.Enabled = True
			cmd_Add.Enabled = True
			cmd_RemoveAll.Enabled = False
			dtgBuyer.DataSource = dvViewItem
			dtgBuyer.DataBind()
		Else
			cmd_Remove.Enabled = False
			cmd_Add.Enabled = False
			cmd_RemoveAll.Enabled = False
			dtgBuyer.DataBind()
			Common.NetMsgbox(Me, MsgNoRecord)
		End If
		' add for above checking
		viewstate("PageCount") = dtgBuyer.PageCount
    End Function

    Private Function BindgridFav(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        value2 = cboCatalogueFav.SelectedItem.Value
        Dim ds As DataSet = New DataSet
        Dim objBUYERCAT As New BuyerCat
        Dim record As String
        Dim strget As String
        AssignDesc()
        ds = objBUYERCAT.getFavCat_CDP(value2, viewstate("valdesc"))
        Dim dvVItem As DataView
		dvVItem = ds.Tables(0).DefaultView


		Dim filter As String = ""
		If Me.txtVendorItem.Text <> "" Then
			filter = filter & "PM_VENDOR_ITEM_CODE LIKE '%" & Me.txtVendorItem.Text & "%'"
		End If

		If Me.txtBuyerItem.Text <> "" Then
			If Not filter.Equals("") Then
				filter = filter & " AND "
			End If
			filter = filter & "CBC_B_ITEM_CODE LIKE '%" & Me.txtBuyerItem.Text & "%'"
		End If

		If Me.txtVendorName.Text <> "" Then
			If Not filter.Equals("") Then
				filter = filter & " AND "
			End If
			filter = filter & "CM_COY_NAME LIKE '%" & Me.txtVendorName.Text & "%'"
		End If

		dvVItem.RowFilter = filter

		dvVItem.Sort = viewstate("SortExpression")
		If viewstate("SortAscending") = "no" Then dvVItem.Sort += " DESC"

		If viewstate("action") = "del" Then
			If dtgFavs.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgFavs.PageSize = 0 Then
				dtgFavs.CurrentPageIndex = dtgFavs.CurrentPageIndex - 1
				viewstate("action") = ""
			End If
		End If
		intPageRecordCnt = dvVItem.Count
		viewstate("intPageRecordCnt") = intPageRecordCnt
		If intPageRecordCnt > 0 Then
			resetDatagridPageIndex(dtgFavs, dvVItem)
			cmd_Remove.Enabled = True
			cmd_Add.Enabled = True
			'//set to false before bind grid
			'//if any invalid item fould, set to true
			cmd_RemoveAll.Enabled = False
			dtgFavs.DataSource = dvVItem
			blnUnavailable = False
			dtgFavs.DataBind()
			If blnUnavailable Then
				cmd_RemoveAll.Enabled = True
			End If
		Else
			cmd_Remove.Enabled = False
			cmd_Add.Enabled = False
			cmd_RemoveAll.Enabled = False
			'cmd_Reset.Disabled = True
			dtgFavs.DataBind()
			Common.NetMsgbox(Me, MsgNoRecord)
		End If
		' add for above checking
		viewstate("PageCount") = dtgFavs.PageCount
    End Function
    Private Function BindBuyerCat()
        Dim dv As DataView
        dv = objCo.getBuyerCatByUser
        cboCatalogueBuyer.Items.Clear()
       
        If Not dv Is Nothing Then
            cboCatalogueBuyer.Enabled = True
            Common.FillDdl(cboCatalogueBuyer, "name", "BCM_CAT_INDEX", dv)
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboCatalogueBuyer.Items.Insert(0, cbolist)
    End Function
    Private Function BindFav()
        Dim dv As DataView
        dv = objCo.Favlist_bindFav()
        cboCatalogueFav.Items.Clear()
       
        If Not dv Is Nothing Then
            Common.FillDdl(cboCatalogueFav, "name", "FLM_LIST_INDEX", dv)
        End If
        cbolistfav.Value = ""
        cbolistfav.Text = "---Select---"
        cboCatalogueFav.Items.Insert(0, cbolistfav)
    End Function

    Private Sub cboCatalogueFav_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCatalogueFav.SelectedIndexChanged
        If cboCatalogueFav.SelectedItem.Text <> "---Select---" Then
            Div_Buyer.Style("display") = "none"
            Div_Fav.Style("display") = "inline"
            '//there are datagrid which bind to different field in this page
            '//these 2 datagrid share the same viewstate
            viewstate("SortExpression") = ""
            viewstate("SortAscending") = "yes"
            dtgFavs.CurrentPageIndex = 0
            BindgridFav(0)
            cmd_Add.Attributes.Add("onclick", "return reCheckAtLeastOne('chkS');")
            cmd_Remove.Attributes.Add("onclick", "return CheckAtLeastOne('chkS','delete');")
            cmdSearch.Enabled = True
            cmd_Clear.Disabled = False
        Else
            cmdSearch.Enabled = False
            cmd_Clear.Disabled = True
            cmd_Remove.Enabled = False
            cmd_RemoveAll.Enabled = False
            cmd_Add.Enabled = False
            dtgFavs.DataBind()
        End If
    End Sub
    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgBuyer.CurrentPageIndex = 0
        Bindgrid(0, True)
    End Sub
    Public Sub SortCommandFav_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgFavs.CurrentPageIndex = 0
        BindgridFav(0, True)
    End Sub

    Private Sub dtgBuyer_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgBuyer.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgBuyer, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dtgFavs_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgFavs.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgFavs, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll2 As CheckBox = e.Item.FindControl("chkA")
            chkAll2.Attributes.Add("onclick", "selectAllF();")
        End If
    End Sub

    Public Sub MyDataGrid_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgBuyer.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0, True)
    End Sub
    Public Sub MyData_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgFavs.CurrentPageIndex = e.NewPageIndex
        BindgridFav(0, True)
    End Sub

    Private Sub cboCatalogueBuyer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCatalogueBuyer.SelectedIndexChanged

        If cboCatalogueBuyer.SelectedItem.Text <> "---Select---" Then
            Div_Buyer.Style("display") = "inline"
            Div_Fav.Style("display") = "none"
            dtgBuyer.CurrentPageIndex = 0
            viewstate("SortExpression") = ""
            viewstate("SortAscending") = "yes"
            Bindgrid(0)
            cmd_Add.Attributes.Add("onclick", "return reCheckAtLeastOne('chkSelection');")
            'cmd_Remove.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
            '//remark by Moo, unnecessary checking
            cmdSearch.Enabled = True
            cmd_Clear.Disabled = False
        Else
            cmdSearch.Enabled = False
            cmd_Clear.Disabled = True
            cmd_Remove.Enabled = False
            cmd_RemoveAll.Enabled = False
            cmd_Add.Enabled = False
            dtgBuyer.DataBind()
        End If
    End Sub
    Function buyerselected()
        If cboCatalogueBuyer.SelectedItem.Text <> "---Select---" Then
            Div_Buyer.Style("display") = "inline"
            Div_Fav.Style("display") = "none"
            dtgBuyer.CurrentPageIndex = 0
            viewstate("SortExpression") = ""
            viewstate("SortAscending") = "yes"
            Bindgrid(0)
            cmd_Add.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
            'cmd_Remove.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
            '//remark by Moo, unnecessary checking
        Else

            cmd_Remove.Enabled = False
            cmd_RemoveAll.Enabled = False
            cmd_Add.Enabled = False
            dtgBuyer.DataBind()
        End If
    End Function
    Function favselected()
        If cboCatalogueFav.SelectedItem.Text <> "---Select---" Then
            Div_Buyer.Style("display") = "none"
            Div_Fav.Style("display") = "inline"
            '//there are datagrid which bind to different field in this page
            '//these 2 datagrid share the same viewstate
            viewstate("SortExpression") = ""
            viewstate("SortAscending") = "yes"
            dtgFavs.CurrentPageIndex = 0
            BindgridFav(0)
            cmd_Add.Attributes.Add("onclick", "return CheckAtLeastOne('chkS');")
            cmd_Remove.Attributes.Add("onclick", "return CheckAtLeastOne('chkS','delete');")
        Else

            cmd_Remove.Enabled = False
            cmd_RemoveAll.Enabled = False
            cmd_Add.Enabled = False
            dtgFavs.DataBind()
        End If
    End Function
    Private Sub cmd_Remove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Remove.Click
        value1 = cboCatalogueBuyer.SelectedItem.Value
        value2 = cboCatalogueFav.SelectedItem.Value
        Dim obj As New BuyerCat

        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim procode, soucode As String
        Dim i As Integer = 0

        For Each dgItem In dtgFavs.Items

            chk = dgItem.FindControl("chkS")
            ' THIS TO GET MORE THAN ONE KEY VALUE FROM DATAGRID 
            soucode = dgItem.Cells.Item(FEnum.BSource).Text
            procode = dgItem.Cells.Item(FEnum.ProCode).Text

            If chk.Checked Then
                obj.delFav1(value2, procode, soucode)
            End If
            i = i + 1
        Next
        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        viewstate("action") = "del"
        BindgridFav(0)

    End Sub

    Private Sub dtgBuyer_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgBuyer.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim chk As CheckBox
            Dim intTotalCell, intLoop As Integer
            Dim bln As Boolean
            chk = e.Item.Cells(FEnum.chkbox).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dtCurrDate As Date = Now.Today

            If IsDBNull(dv("PM_UNIT_COST")) Or Trim(e.Item.Cells.Item(FEnum.Cost).Text) = "0.0000" Then
                e.Item.Cells(FEnum.Cost).Text = "N/A"
            End If

            If IsDBNull(dv("CDM_END_DATE")) And dv("BCU_SOURCE") = "CP" Then
                bln = True
                chk.Visible = False
                e.Item.Cells(FEnum.chkbox).Text = "Del"
                e.Item.BackColor = Color.FromName("#ff99cc")
            End If

            Dim arrVal As String = ""
            arrVal = objDb.ReturnArrayValue(dv("CDM_END_DATE"), UBound(dv("CDM_END_DATE")))

            If Not IsDBNull(arrVal) Then
                If CDate(arrVal) < dtCurrDate Then
                    bln = True
                    e.Item.BackColor = Color.FromName("#ff99cc")
                    e.Item.Cells(FEnum.Expired).Text = "EXP"
                    chk.Visible = False
                    e.Item.Cells(FEnum.chkbox).Text = "Del"
                End If
            End If

            Dim valvendor As String
            valvendor = e.Item.Cells(FEnum.CoyID).Text
            intstate = objCo.chkVendorState(valvendor)
            If intstate = "0" Then
                If Not bln Then
                    e.Item.BackColor = Color.PaleTurquoise
                    e.Item.Cells(FEnum.chkbox).Text = "Del"
                    chk.Visible = False
                    bln = True
                End If
            End If

            ' ai chu add on 09/12/2005
            ' deactived company's item will be displayed in 'LightSteelBlue' color
            If Common.parseNull(dv("CM_STATUS")) <> "A" Then
                e.Item.BackColor = Color.FromName("LightSteelBlue")
                e.Item.Cells(FEnum.Expired).Text = "EXP"
                chk.Visible = False
                e.Item.Cells(FEnum.chkbox).Text = "Del"
                bln = True
            End If

            Dim txtQty As TextBox
            txtQty = e.Item.FindControl("txtQty")
            txtQty.Text = ""
            If bln = True Then
                txtQty.Enabled = False
            End If

            Dim revQty As RegularExpressionValidator
            revQty = e.Item.FindControl("revQty")
            revQty.ValidationExpression = "(?!^0*$)^\d{1,5}?$" '"\d{1,5}" '"^\d+$" ' "^\d+\.+\d+$|^\d+$"
            revQty.ControlToValidate = "txtQty"
            revQty.ErrorMessage = "Invalid quantity"
            revQty.Display = ValidatorDisplay.Dynamic
        End If
    End Sub

    Private Sub dtgFavs_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgFavs.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim chk2 As CheckBox
            Dim delProcode, delsource As String
            Dim intTotalCell, intLoop As Integer
            Dim bln As Boolean = False

            chk2 = e.Item.Cells(FEnum.chkbox).FindControl("chkS")
            chk2.Attributes.Add("onclick", "checkC('" & chk2.ClientID & "')")
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dtCurrDate As String = Now.Today

            value2 = cboCatalogueFav.SelectedItem.Value
            ' ai chu remarked on 15/12/2005
            ' if exists unavailable item then cmd_RemoveAll.enabled = true
            'Dim buttonContorl As String = objCo.unavailable_buttoncontrol(value2)

            'If buttonContorl = "1" Then
            '    cmd_RemoveAll.Enabled = True
            'Else
            '    cmd_RemoveAll.Enabled = False
            'End If

            If IsDBNull(dv("PM_UNIT_COST")) Or Trim(e.Item.Cells.Item(FEnum.Cost).Text) = "0.0000" Then
                e.Item.Cells(FEnum.Cost).Text = "N/A"
            End If

            If (dv("FLI_CD_GROUP_INDEX") = "-1") Or (IsDBNull(dv("CDM_END_DATE")) And dv("FLI_SOURCE") = "CP") Then
                bln = True
                e.Item.BackColor = Color.FromName("#ff99cc")
                blnSetExpired_F = True
                chk2.Visible = False
                Dim clink As New LinkButton
                clink.ID = e.Item.Cells.Item(FEnum.BSource).Text & "," & e.Item.Cells.Item(FEnum.ProCode).Text
                clink.Attributes.Add("onclick", "return DelExpItem('" & clink.ID & "');")
                clink.Text = "Del"
                blnUnavailable = True
                'AddHandler alink.Click, AddressOf linkDel_FAV
                e.Item.Cells(FEnum.chkbox).Controls.Add(clink)
            End If

            Dim arrVal As String = ""
            arrVal = objDb.ReturnArrayValue(dv("CDM_END_DATE"), UBound(dv("CDM_END_DATE")))
            If Not IsDBNull(arrVal) Then
                If CDate(arrVal) < dtCurrDate Then
                    bln = True
                    e.Item.BackColor = Color.FromName("#ff99cc")
                    e.Item.Cells(FEnum.Expired).Text = "EXP"
                    blnSetExpired_F = True
                    chk2.Visible = False
                    Dim alink As New LinkButton
                    alink.ID = e.Item.Cells.Item(FEnum.BSource).Text & "," & e.Item.Cells.Item(FEnum.ProCode).Text
                    alink.Attributes.Add("onclick", "return DelExpItem('" & alink.ID & "');")
                    alink.Text = "Del"
                    blnUnavailable = True
                    e.Item.Cells(FEnum.chkbox).Controls.Add(alink)
                End If
            End If

            valvendor = e.Item.Cells(FEnum.CoyID).Text
            intstate = objCo.chkVendorState(valvendor)
            If intstate = "0" Then
                If Not bln Then
                    e.Item.BackColor = Color.PaleTurquoise
                    chk2.Visible = False
                    Dim blink As New LinkButton
                    blink.ID = e.Item.Cells.Item(FEnum.BSource).Text & "," & e.Item.Cells.Item(FEnum.ProCode).Text
                    blink.Attributes.Add("onclick", "return DelExpItem('" & blink.ID & "');")
                    blink.Text = "Del"
                    blnUnavailable = True
                    e.Item.Cells(FEnum.chkbox).Controls.Add(blink)
                    bln = True
                End If
            End If

            ' ai chu add on 09/12/2005
            ' deactived company's item will be displayed in 'LightSteelBlue' color
            If Common.parseNull(dv("CM_STATUS")) <> "A" Then
                e.Item.BackColor = Color.FromName("LightSteelBlue")
                'e.Item.Cells(FEnum.Expired).Text = "EXP"
                'chk2.Visible = False
                'e.Item.Cells(FEnum.chkbox).Text = "Del"
                'bln = True
                chk2.Visible = False
                Dim blink As New LinkButton
                blink.ID = e.Item.Cells.Item(FEnum.BSource).Text & "," & e.Item.Cells.Item(FEnum.ProCode).Text
                blink.Attributes.Add("onclick", "return DelExpItem('" & blink.ID & "');")
                blink.Text = "Del"
                blnUnavailable = True
                e.Item.Cells(FEnum.chkbox).Controls.Add(blink)
                bln = True
            End If

            Dim txtQty As TextBox
            txtQty = e.Item.FindControl("txtQty2")
            txtQty.Text = ""

            If bln = True Then
                txtQty.Enabled = False
            End If

            Dim revQty As RegularExpressionValidator
            revQty = e.Item.FindControl("revQty2")
            revQty.ValidationExpression = "(?!^0*$)^\d{1,5}?$" '"\d{1,5}" '"^\d+$" ' "^\d+\.+\d+$|^\d+$"
            revQty.ControlToValidate = "txtQty2"
            revQty.ErrorMessage = "Invalid quantity"
            revQty.Display = ValidatorDisplay.Dynamic
        End If
    End Sub

    Function linkDel_FAV(ByVal strID As String)
        value1 = cboCatalogueBuyer.SelectedItem.Value
        value2 = cboCatalogueFav.SelectedItem.Value
        Dim obj As New BuyerCat
        Dim strAry() As String
        Dim procode, soucode As String

        strAry = strID.Split(",")
        soucode = strAry(0)
        procode = strAry(1)
        obj.delFav1(value2, procode, soucode)
        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        viewstate("action") = "del"
        BindgridFav(0)
    End Function

    Private Sub cmd_Add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Add.Click
        
        Dim dtProduct As DataTable
        dtProduct = objProduct.getSCSchema()
        dtProduct.Columns.Add("GRP_INDEX", Type.GetType("System.Int32")) '//group index
        dtProduct.Columns.Add("CAT_TYPE", Type.GetType("System.String"))  '//LP,CP,DP
        dtProduct.Columns.Add("msg", Type.GetType("System.String"))  '//error msg
        Dim dr As DataRow
        Dim grdItem As DataGridItem
        If rdSearch.SelectedValue = "B" Then
            For Each grdItem In dtgBuyer.Items
                Dim chkSel As CheckBox = grdItem.Cells(FEnum.chkbox).FindControl("chkSelection")
                If chkSel.Checked Then
                    dr = dtProduct.NewRow
                    dr("S_COY_ID") = grdItem.Cells.Item(FEnum.CoyID).Text
                    dr("PRODUCT_CODE") = grdItem.Cells.Item(FEnum.ProCode).Text
                    dr("VENDOR_ITEM_CODE") = grdItem.Cells.Item(FEnum.PItemCode).Text
                    dr("SC_PRODUCT_DESC") = grdItem.Cells.Item(FEnum.ProductDesc).Text
                    dr("SC_UNIT_COST") = grdItem.Cells.Item(FEnum.Cost).Text
                    dr("SC_QUANTITY") = IIf(CType(grdItem.Cells(FEnum.Qty).FindControl("txtQty"), TextBox).Text = "", -1, CType(grdItem.Cells(FEnum.Qty).FindControl("txtQty"), TextBox).Text)
                    dr("SC_CURRENCY_CODE") = grdItem.Cells.Item(FEnum.Currency).Text
                    dr("SC_UOM") = grdItem.Cells.Item(FEnum.UOM).Text
                    '//modify By Moo
                    dr("CAT_TYPE") = grdItem.Cells.Item(FEnum.BSource).Text
                    dr("GRP_INDEX") = grdItem.Cells.Item(FEnum.CDGrpIndex).Text
                    dtProduct.Rows.Add(dr)
                End If
            Next
            objProduct.addToShopCart(0, dtProduct)
            Session("dt") = dtProduct
            Response.Redirect(dDispatcher.direct("Product", "DisplayAddedItem.aspx", "type=SC&type1=BF&pageid=" & strPageId))
        ElseIf rdSearch.SelectedValue = "F" Then
            For Each grdItem In dtgFavs.Items
                Dim chkSel As CheckBox = grdItem.Cells(FEnum.chkbox).FindControl("chkS")
                If chkSel.Checked Then
                    dr = dtProduct.NewRow
                    dr("S_COY_ID") = grdItem.Cells.Item(FEnum.CoyID).Text
                    dr("PRODUCT_CODE") = grdItem.Cells.Item(FEnum.ProCode).Text
                    dr("VENDOR_ITEM_CODE") = grdItem.Cells.Item(FEnum.PItemCode).Text
                    dr("SC_PRODUCT_DESC") = grdItem.Cells.Item(FEnum.ProductDesc).Text
                    dr("SC_UNIT_COST") = grdItem.Cells.Item(FEnum.Cost).Text
                    dr("SC_QUANTITY") = IIf(CType(grdItem.Cells(FEnum.Qty).FindControl("txtQty2"), TextBox).Text = "", -1, CType(grdItem.Cells(FEnum.Qty).FindControl("txtQty2"), TextBox).Text)
                    dr("SC_CURRENCY_CODE") = grdItem.Cells.Item(FEnum.Currency).Text
                    dr("SC_UOM") = grdItem.Cells.Item(FEnum.UOM).Text
                    '//add by Moo
                    dr("CAT_TYPE") = grdItem.Cells.Item(FEnum.BSource).Text
                    dr("GRP_INDEX") = grdItem.Cells.Item(FEnum.CDGrpIndex).Text
                    dtProduct.Rows.Add(dr)
                End If
            Next
            objProduct.addToShopCart(0, dtProduct)
            Session("dt") = dtProduct
            Response.Redirect(dDispatcher.direct("Product", "DisplayAddedItem.aspx", "type=SC&type1=BF&pageid=" & strPageId))
        End If
    End Sub


    Private Sub cmd_RemoveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_RemoveAll.Click

        value2 = cboCatalogueFav.SelectedItem.Value

        objCo.del_All_UnavailableFav(value2)
        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        viewstate("action") = "del"
        BindgridFav(0)

    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Dim str As String
       
        If rdSearch.SelectedValue = "F" Then
            favselected()
        ElseIf rdSearch.SelectedValue = "B" Then
            buyerselected()
        End If
        viewstate("valdesc") = ""
    End Sub
    Private Function AssignDesc()
        If txtDesc.Text <> "" Then
            If InStr(txtDesc.Text, "*") = 0 And InStr(txtDesc.Text, "?") = 0 Then
                viewstate("valdesc") = "*" & txtDesc.Text & "*"
            Else
                viewstate("valdesc") = txtDesc.Text
            End If
        Else
            viewstate("valdesc") = ""
        End If
    End Function

End Class
