Imports AgoraLegacy
Imports eProcure.Component

Imports System.Drawing

Public Class Add_Buyer_Catalogue_ItemSEH
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents txt_desc As System.Web.UI.WebControls.TextBox
    Protected WithEvents dtg_Cat As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtg_freeform As System.Web.UI.WebControls.DataGrid
    Protected WithEvents onchange As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents cmd_search As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_back As System.Web.UI.WebControls.Button
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lbl_check As System.Web.UI.WebControls.Label
    Protected WithEvents txt_item_desc As System.Web.UI.WebControls.TextBox
    Protected WithEvents cboCommodityType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents dt_rfq As System.Web.UI.WebControls.Table
    Protected WithEvents cmd_freeformClear As System.Web.UI.WebControls.Button

    Protected WithEvents cboCatalogueBuyer As Global.System.Web.UI.WebControls.DropDownList
    Protected WithEvents rd1 As Global.System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents hiddtg_cat As Global.System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents hiddtg_freeform As Global.System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents chkSpot As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkStock As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkMRO As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtItemCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents hidCommodity As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtCommodity As System.Web.UI.WebControls.TextBox

    Dim dDispatcher As New AgoraLegacy.dispatcher

    Dim appGlo As New AppGlobals
    Dim total As Integer
    Dim check_qty As Integer
    Dim objGlobal As New AppGlobals
    Dim dta As Table
    Dim strMsg As String = ""
    Dim rfq_name As String
    Dim back As String

    Dim strOversea, strItemType, strVendor, strMode As String

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
        cmd_Save.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmd_Save)
        htPageAccess.Add("add", alButtonList)

        If ViewState("intPageRecordCnt") > 0 Then
            cmd_Save.Enabled = blnCanAdd
            cmd_Save.Enabled = blnCanUpdate
        Else
            cmd_Save.Enabled = False
        End If

        CheckButtonAccess()
        alButtonList.Clear()

    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strNewCSS = "both"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtg_Cat)
        SetGridProperty(dtg_freeform)

        If Not Page.IsPostBack Then

            ' objGlobal.FillCommodityType(Me.cboCommodityType)
            Me.cmd_back.Attributes.Add("onclick", "window.close();")

            Dim objpro As New Products
            Dim objrfq As New RFQ
            Dim ary(100) As String

            BindBuyerCat()
        End If

        If rd1.SelectedValue = "CAT" Then
            cmd_Save.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','Save');")
        ElseIf rd1.SelectedValue = "FREE" Then
            cmd_Save.Attributes.Remove("onclick;")
        End If

        lbl_check.Text = ""
        Session("CurrentScreen") = "AddItem"

        strOversea = Request.QueryString("Oversea")
        strItemType = Request.QueryString("ItemType")
        strVendor = Request.QueryString("Vendor")
        strMode = Request.QueryString("mode")

        If strMode <> "bc" Then
            If strItemType = "SP" Then
                'chkSpot.Enabled = True
                chkStock.Enabled = False
                chkMRO.Enabled = False
            ElseIf strItemType = "ST" Then
                chkSpot.Enabled = False
                'chkStock.Enabled = True
                chkMRO.Enabled = False
            ElseIf strItemType = "MI" Then
                chkSpot.Enabled = False
                chkStock.Enabled = False
                'chkMRO.Enabled = True
            End If
        End If
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objpro As New Products
        Dim ds As New DataSet
        Dim ary(0) As String
        Dim objrfq As New RFQ
        Dim strCommodity As String = ""
        Dim strCatalogueBuyer As String = ""
        Dim aryProdCode As New ArrayList, i As Integer
        Dim strProdList As String = ""
        Dim aryItemType As New ArrayList

        If Me.txtCommodity.Text <> "" Then
            strCommodity = Me.hidCommodity.Value
        End If

        If Me.cboCatalogueBuyer.SelectedItem.Text <> "---Select---" Then
            strCatalogueBuyer = Me.cboCatalogueBuyer.SelectedValue
        End If

        If Me.chkSpot.Checked = True Then
            aryItemType.Add("SP")
        End If

        If Me.chkStock.Checked = True Then
            aryItemType.Add("ST")
        End If

        If Me.chkMRO.Checked = True Then
            aryItemType.Add("MI")
        End If

        If strMode <> "bc" Then
            If strItemType = "SP" Then
                aryItemType.Add("SP")
            End If

            If strItemType = "ST" Then
                aryItemType.Add("ST")
            End If

            If strItemType = "MI" Then
                aryItemType.Add("MI")
            End If
        End If


        Dim objBUYERCAT As New BuyerCat
        Dim objBUYERCAT_Ext As New BuyerCat_Ext
        Dim strItem As String = ""
        If cboCatalogueBuyer.SelectedItem.Text <> "---Select---" Then
            ds = objBUYERCAT_Ext.getBuyerCatItems1(cboCatalogueBuyer.SelectedItem.Value, strCommodity, txt_item_desc.Text, True, txtItemCode.Text, aryItemType, "", "")

        Else
            If cboCatalogueBuyer.Items.Count > 1 Then
                For i = 1 To cboCatalogueBuyer.Items.Count - 1
                    strItem = strItem & cboCatalogueBuyer.Items(i).Value & ","
                Next
                strItem = Mid(strItem, 1, Len(strItem) - 1)
            Else
                strItem = "0"
            End If
            ds = objBUYERCAT_Ext.getBuyerCatItemsByCombo1(strItem, strCommodity, txt_item_desc.Text, True, txtItemCode.Text, aryItemType, "", "")

        End If

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        intPageRecordCnt = dvViewSample.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtg_Cat, dvViewSample)
            dtg_Cat.DataSource = dvViewSample
            dtg_Cat.DataBind()
        Else
            dtg_Cat.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtg_Cat.PageCount
    End Function

    Public Sub dtg_cat_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        Bindgrid()

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        sender.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtg_Cat_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_Cat.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim lnkCode As HyperLink
            lnkCode = e.Item.Cells(1).FindControl("lnkCode")
            lnkCode.NavigateUrl = "javascript:;"
            Session("UrlLocation") = "BuyerCatSearch"
            lnkCode.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=B&pageid=" & strPageId & "&pid=" & dv("PM_PRODUCT_CODE") & "&index=&draft=0") & "')")
            lnkCode.Text = dv("PM_VENDOR_ITEM_CODE")

            Dim lblItemType As Label
            lblItemType = e.Item.Cells(2).FindControl("lblItemType")

            If dv("PM_ITEM_TYPE") = "SP" Then
                lblItemType.Text = "Spot"
            ElseIf dv("PM_ITEM_TYPE") = "ST" Then
                lblItemType.Text = "Stock"
            Else
                lblItemType.Text = "MRO"
            End If
        End If
    End Sub

    Private Sub dtg_Cat_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_Cat.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtg_Cat, e)

        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If

    End Sub

    Private Sub BindBuyerCat()
        Dim dv As DataView
        Dim cbolist As New ListItem
        Dim objcat As New BuyerCat

        dv = objcat.getBuyerCatByUser()
        cboCatalogueBuyer.Items.Clear()

        If Not dv Is Nothing Then
            cboCatalogueBuyer.Enabled = True
            Common.FillDdl(cboCatalogueBuyer, "BCM_GRP_DESC", "BCM_CAT_INDEX", dv)
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboCatalogueBuyer.Items.Insert(0, cbolist)
    End Sub

    Private Sub cmd_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_search.Click
        dtg_Cat.CurrentPageIndex = 0
        Bindgrid(0)
    End Sub

    Private Sub clear_All()
        txtItemCode.Text = ""
        txt_item_desc.Text = ""
        cboCatalogueBuyer.SelectedIndex = 0
        txtCommodity.Text = ""
        hidCommodity.Value = ""
        chkSpot.Checked = False
        chkStock.Checked = False
        chkMRO.Checked = False
    End Sub

    Private Sub cmd_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_clear.Click
        Call clear_All()
    End Sub

    Private Sub cmd_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Save.Click
        Dim chkItem As CheckBox
        Dim aryProdCode As New ArrayList
        Dim i, j As Integer
        Dim chkItemType As String = ""

        aryProdCode = Session("ProdList")
        If aryProdCode Is Nothing Then
            aryProdCode = New ArrayList
        End If
        Dim dgItemFF As DataGridItem
        If rd1.SelectedValue = "FREE" Then
            For Each dgItemFF In dtg_freeform.Items
                Dim txt_desc As TextBox
                Dim ddl_uom As New DropDownList
                'Dim ddl_comm As New DropDownList
                Dim txtCommodityFree As New TextBox
                txt_desc = dgItemFF.FindControl("txt_desc")
                ddl_uom = dgItemFF.FindControl("ddl_uom")
                'ddl_comm = dgItemFF.FindControl("ddl_comm")
                txtCommodityFree = dgItemFF.FindControl("txtCommodityFree")

                Dim ddl_stock As New DropDownList
                ddl_stock = dgItemFF.FindControl("ddl_stock")

                If chkItemType = "" Then
                    chkItemType = ddl_stock.SelectedIndex
                End If

                If txt_desc.Text <> "" Then
                    If strMode <> "bc" Then
                        If chkItemType <> ddl_stock.SelectedIndex Then
                            Common.NetMsgbox(Me, "Mixture of item type is not allowed.", MsgBoxStyle.Information)
                            Exit Sub
                        End If
                    End If
                    'aryProdCode.Add(New String() {"", txt_desc.Text, ddl_uom.SelectedItem.Text, ddl_comm.SelectedItem.Text, ""})
                    aryProdCode.Add(New String() {"", txt_desc.Text, ddl_uom.SelectedItem.Text, txtCommodityFree.Text, "", ddl_stock.SelectedItem.Value})
                End If

                
            Next
        Else
            cmd_Save.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','Save');")
            For i = 0 To dtg_Cat.Items.Count() - 1
                chkItem = dtg_Cat.Items(i).Cells(0).FindControl("chkSelection")
                Dim lblItemType As New Label
                lblItemType = dtg_Cat.Items(i).Cells(0).FindControl("lblItemType")

                If chkItemType = "" Then
                    chkItemType = lblItemType.Text
                End If

                If chkItem.Checked Then
                    If strMode <> "bc" Then
                        If chkItemType <> lblItemType.Text Then
                            Common.NetMsgbox(Me, "Mixture of item type is not allowed.", MsgBoxStyle.Information)
                            Exit Sub
                        End If
                    End If

                    If i = 0 Then 'keep track of the 1st row that is checked
                        aryProdCode.Add(New String() {dtg_Cat.Items(i).Cells(2).Text, "", "", "", "", lblItemType.Text})

                    Else
                        aryProdCode.Add(New String() {dtg_Cat.Items(i).Cells(2).Text, "", "", "", "", lblItemType.Text})
                    End If
                End If
            Next
        End If

        Session("ProdList") = aryProdCode
        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        If rd1.SelectedValue = "FREE" Then
            rd1_SelectedIndexChanged(rd1, New System.EventArgs())
        Else
            cmd_search_Click(cmd_search, New System.EventArgs())
        End If

    End Sub

    Private Sub rd1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rd1.SelectedIndexChanged
        If rd1.SelectedValue = "CAT" Then
            cmd_Save.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','Save');")
            hiddtg_freeform.Style.Item("display") = "none"
            hiddtg_cat.Style.Item("display") = ""
            dtg_Cat.DataSource = Nothing
            dtg_Cat.DataBind()
            dtg_freeform.DataSource = Nothing
            dtg_freeform.DataBind()
            cmd_search.Visible = True
            cmd_clear.Visible = True
            Call clear_All()
        ElseIf rd1.SelectedValue = "FREE" Then
            cmd_Save.Attributes.Remove("onclick")
            hiddtg_freeform.Style.Item("display") = ""
            hiddtg_cat.Style.Item("display") = "none"
            cmd_search.Visible = False
            cmd_clear.Visible = False

            'Dim dgItemFF As DataGridItem
            'For Each dgItemFF In dtg_freeform.Items
            '    Dim ddl_stock As New DropDownList
            '    ddl_stock = dgItemFF.FindControl("ddl_stock")
            '    ddl_stock.Enabled = False
            'Next

            Call clear_All()
            AddRow(10)
        End If
    End Sub

    Sub AddRow(ByVal intRow As Integer)
        Dim dt As New DataTable
        Dim row As DataRow
        Dim i, y As Integer
        dt.Columns.Add("txt_desc", GetType(String))
        For i = 1 To intRow
            row = dt.NewRow()
            dt.Rows.Add(row)
            Populate(i)
            Dim txtTotal As New TextBox
        Next
        dtg_freeform.DataSource = dt
        dtg_freeform.DataBind()
        buildjava()
    End Sub

    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub

    Private Sub dtg_freeform_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_freeform.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim txt_qty As TextBox
            Dim txt_desc As TextBox
            txt_desc = e.Item.FindControl("txt_desc")

            txt_desc.Attributes.Add("onKeyDown", "limitText (this, 500);")

            Dim ddl_uom As New DropDownList
            Dim dv_uom As DataView
            ddl_uom = e.Item.FindControl("ddl_uom")
            dv_uom = appGlo.GetCodeTableView(CodeTable.Uom)
            Common.FillDdl(ddl_uom, "CODE_DESC", "CODE_ABBR", dv_uom)

            'Dim ddl_comm As New DropDownList
            'ddl_comm = e.Item.FindControl("ddl_comm")
            'objGlobal.FillCommodityType(ddl_comm)

            Dim ddl_stock As New DropDownList
            ddl_stock = e.Item.FindControl("ddl_stock")

            If strMode <> "bc" Then
                'cbolist.Value = ""
                If strItemType = "SP" Then
                    ddl_stock.SelectedValue = "SP"
                End If

                If strItemType = "ST" Then
                    ddl_stock.SelectedValue = "ST"
                End If

                If strItemType = "MI" Then
                    ddl_stock.SelectedValue = "MI"
                End If


                If strItemType = "" Then
                    ddl_stock.Enabled = True
                Else
                    ddl_stock.Enabled = False
                End If
            End If
        End If
    End Sub

    Sub Populate(ByVal i As Integer)
        Dim content As String = ""
        Dim nametypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=Commodity")
        Dim j As String

        If Len(CStr((i + 2))) = 1 Then
            j = "0" & CStr((i + 2))
        Else
            j = (i + 2)
        End If
        content &= "$(""#dtg_freeform_ctl" & j & "_txtCommodityFree"").autocomplete(""" & nametypeahead & """, {" & vbCrLf & _
        "width: 170," & vbCrLf & _
        "scroll: true," & vbCrLf & _
        "selectFirst: false" & vbCrLf & _
        "});" & vbCrLf & _
        "$(""#dtg_freeform_ctl" & j & "_txtCommodityFree"").result(function(event, data, formatted) {" & vbCrLf & _
        "if(data)" & vbCrLf & _
        "$(""#dtg_freeform_ctl" & j & "_hidCommodityFree"").val(data[1]);" & vbCrLf & _
        "}); " & vbCrLf

        Session("content") &= content
    End Sub

    Sub buildjava()
        Dim typeahead As String
        typeahead = "<script language=""javascript"">" & vbCrLf & _
          "<!--" & vbCrLf & _
            "$(document).ready(function(){" & vbCrLf & _
            Session("content") & vbCrLf & _
            "});" & vbCrLf & _
            "-->" & vbCrLf & _
            "</script>"
        Session("typeahead") = typeahead
    End Sub

End Class
