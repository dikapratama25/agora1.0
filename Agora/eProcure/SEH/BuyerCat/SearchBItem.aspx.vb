Imports AgoraLegacy
Imports eProcure.Component



Public Class SearchBItemSEH
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objBuyCat As New BuyerCat
    Dim objCat As New ContCat
    Dim objGlobal As New AppGlobals
    Dim strBCIdx, strFrm As String

    Public Enum EnumCat
        icChk = 0
        icItemIndex = 1
        icItemCode = 2
        icItemDesc = 3
        icVendorItemCode = 4
        icUOM = 5
        icProdCode = 6
        icStatus = 7
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkActive As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkInActive As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkSpot As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkStock As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkMRO As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents dtgCatalogue As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents btnHidden As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdActivate As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDeActivate As System.Web.UI.WebControls.Button
    Protected WithEvents divApprove2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdSelectAll As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents trDiscount As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtItemId As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVendorItemCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents cboCommodityType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents hidAction As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidCommodity As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtCommodity As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVenCode As System.Web.UI.WebControls.TextBox

    'Protected WithEvents cmdClose As System.Web.UI.WebControls.Button
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
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdModify)
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdActivate)
        alButtonList.Add(cmdDeActivate)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        cmdAdd.Enabled = blnCanAdd And ViewState("blnCmdAdd")
        cmdModify.Enabled = blnCanUpdate And ViewState("blnCmdModify")
        cmdDeActivate.Enabled = blnCanDelete And ViewState("blnCmdDelete")
        cmdActivate.Enabled = blnCanDelete And ViewState("blnCmdDelete")
        '    cmdSave.Enabled = False
        '    ViewState("blnCmdModify") = False
        'End If
        'cmdSave.Enabled = blnCanUpdate And ViewState("blnCmdModify")
        'cmdDelete.Enabled = blnCanDelete And ViewState("blnCmdDelete")
        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        'Put user code to initialize the page here
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCatalogue)

        If Not IsPostBack Then
            GenerateTab()
            'objGlobal.FillCommodityType(Me.cboCommodityType)
            ViewState("blnCmdAdd") = True
            ViewState("blnCmdModify") = False
            ViewState("blnCmdDelete") = False
            'cmdActivate.Visible = False
            'cmdDeActivate.Visible = False
            cmdSave.Enabled = False
            Session("ImageIndex") = ""
            cmdModify.Enabled = False
            ViewState("blnCmdModify") = False
            cmdActivate.Enabled = False
            cmdDeActivate.Enabled = False
            If strFrm = "ItemCat" Then
                cmdAdd.Visible = False
                cmdModify.Visible = False
                cmdActivate.Visible = False
                cmdDeActivate.Visible = False
                chkActive.Visible = False
                chkInActive.Visible = False
                ViewState("blnCmdAdd") = False
                lblTitle.Text = "Assign Item"
                cmdSave.Visible = True
                Me.hidAction.Style("display") = "none"
            Else
                cmdAdd.Visible = True
                'cmdClose.Visible = False
                'cmdModify.Visible = True
                'cmdActivate.Visible = True
                'cmdDeActivate.Visible = True
                chkActive.Visible = True
                chkInActive.Visible = True
                'ViewState("blnCmdAdd") = True
                'ViewState("blnCmdModify") = True
                'ViewState("blnCmdDelete") = True
                cmdSave.Visible = False

                DisplayUserCheckBtn()
            End If
        End If
        strBCIdx = Me.Request.QueryString("BCIdx")
        strFrm = Me.Request.QueryString("Frm")
        ChkCloseBtn()

        cmdSave.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        cmdActivate.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','activate');")
        cmdDeActivate.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','deactive');")
    End Sub
    Private Function Bindgrid() As String
        Dim ds As New DataSet
        Dim strCommodity As String = ""
        Dim strDel As String = ""
        Dim aryItemType As New ArrayList
        Dim objBuyCat_Ext As New BuyerCat_Ext
        Dim strSpt, strStk, strMro As String

        If Me.txtCommodity.Text <> "" Then
            strCommodity = Me.hidCommodity.Value
        End If

        If chkActive.Checked = True Then strDel = "N"
        If chkInActive.Checked = True Then strDel = "Y"
        If chkActive.Checked And chkInActive.Checked Then strDel = "A"
        If Not chkActive.Checked And Not chkInActive.Checked Then strDel = "" 'will not return any row if didn't check on the checkbox

        If Me.chkSpot.Checked = True Then
            aryItemType.Add("SP")
        End If

        If Me.chkStock.Checked = True Then
            aryItemType.Add("ST")
        End If

        If Me.chkMRO.Checked = True Then
            aryItemType.Add("MI")
        End If

        If Me.chkSpot.Enabled = False Then
            strSpt = "N"
        Else
            strSpt = "Y"
        End If

        If Me.chkStock.Enabled = False Then
            strStk = "N"
        Else
            strStk = "Y"
        End If

        If Me.chkMRO.Enabled = False Then
            strMro = "N"
        Else
            strMro = "Y"
        End If

        ds = objBuyCat_Ext.getItems(Session("CompanyId"), "B", strBCIdx, txtVendorItemCode.Text, txtDesc.Text, strCommodity, strDel, strSpt, strStk, strMro, aryItemType, txtVenCode.text)

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If ViewState("action") = "del" Then
            If dtgCatalogue.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgCatalogue.PageSize = 0 Then
                dtgCatalogue.CurrentPageIndex = dtgCatalogue.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgCatalogue, dvViewSample)
            dtgCatalogue.DataSource = dvViewSample
            dtgCatalogue.DataBind()
            cmdSave.Enabled = True
            If strFrm <> "ItemCat" Then
                cmdModify.Enabled = True
                cmdActivate.Enabled = True
                cmdDeActivate.Enabled = True
                ViewState("blnCmdModify") = True
                ViewState("blnCmdDelete") = True
            End If
        Else
            cmdSave.Enabled = False
            cmdActivate.Enabled = False
            cmdDeActivate.Enabled = False
            cmdModify.Enabled = False
            ViewState("blnCmdModify") = False
            ViewState("blnCmdDelete") = False
            'If strBCIdx = "" And strCommodity <> "" Then Common.NetMsgbox(Me, MsgNoRecord)
            If Session("Search") = "Yes" Or strFrm <> "ItemCat" Then Common.NetMsgbox(Me, MsgNoRecord)
            dtgCatalogue.DataBind()
            Session("Search") = "No"
        End If


        ' add for above checking
        ViewState("PageCount") = dtgCatalogue.PageCount
    End Function

    Sub DisplayUserCheckBtn()
        Dim objUser As New Users_Ext
        Dim dsStk As New DataSet

        dsStk = objUser.GetUserStockType(Session("UserId"), Session("CompanyId"))

        If dsStk.Tables(0).Rows(0)("UM_STK_TYPE_SPOT") = "Y" Then
            chkSpot.Enabled = True
        Else
            chkSpot.Enabled = False
        End If

        If dsStk.Tables(0).Rows(0)("UM_STK_TYPE_STOCK") = "Y" Then
            chkStock.Enabled = True
        Else
            chkStock.Enabled = False
        End If

        If dsStk.Tables(0).Rows(0)("UM_STK_TYPE_MRO") = "Y" Then
            chkMRO.Enabled = True
        Else
            chkMRO.Enabled = False
        End If

    End Sub

    'Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
    '    intPageRecordCnt = ViewState("intPageRecordCnt")
    '    Grid_SortCommand(sender, e)
    '    dtgCatalogue.CurrentPageIndex = 0
    '    Bindgrid()
    'End Sub
    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgCatalogue.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgCatalogue_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgCatalogue, e)

        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dtgCatalogue_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim chk As CheckBox
            chk = e.Item.Cells(EnumCat.icChk).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim lnkItem As HyperLink
            lnkItem = e.Item.FindControl("lnkItem")
            If strFrm <> "ItemCat" Then
                lnkItem.NavigateUrl = dDispatcher.direct("BuyerCat", "BIM.aspx", "itemid=" & dv("PM_PRODUCT_CODE") & "&mode=" & "mod" & "&pageid=" & strPageId)
            End If
            lnkItem.Text = dv("PM_VENDOR_ITEM_CODE")

            If dv("PM_DELETED") = "N" Then
                e.Item.Cells(EnumCat.icStatus).Text = "Active"
            Else
                e.Item.Cells(EnumCat.icStatus).Text = "Inactive"
            End If
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgCatalogue.CurrentPageIndex = 0
        Session("Search") = "Yes"
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "PM_VENDOR_ITEM_CODE"
        Bindgrid()
    End Sub

    Private Sub dtgCatalogue_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCatalogue.PageIndexChanged
        dtgCatalogue.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox
        Dim strItemID As String

        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")
            'strItemID = dgItem.FindControl("PM_PRODUCT_CODE")
            strItemID = dgItem.Cells(EnumCat.icProdCode).Text
            If chkItem.Checked Then
                Session("ImageIndex") = objCat.copyAttachFile(strItemID)
                Response.Redirect(dDispatcher.direct("BuyerCat", "BIM.aspx", "itemid=" & strItemID & "&mode=" & "mod" & "&pageid=" & strPageId))
            End If
        Next
    End Sub

    Private Sub cmdActivate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdActivate.Click
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox
        Dim objCat As New ContCat
        Dim dtItem As New DataTable
        Dim strDeleted, strNotDeleted As String
        Dim dtr As DataRow
        dtItem.Columns.Add("ProductCode", Type.GetType("System.String"))
        dtItem.Columns.Add("CoyId", Type.GetType("System.String"))

        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                dtr = dtItem.NewRow()
                dtr("ProductCode") = dgItem.Cells(6).Text
                dtr("CoyId") = Session("CompanyId")
                dtItem.Rows.Add(dtr)
            End If
        Next

        If objCat.UpdBuyerProductMstr(dtItem, Session("CompanyId"), "N") = WheelMsgNum.Delete Then
            Common.NetMsgbox(Me, "Item(s) activated", MsgBoxStyle.Information)
        Else
            Common.NetMsgbox(Me, "Item(s) not activated", MsgBoxStyle.Information)
        End If
        Bindgrid()
    End Sub
    Private Sub cmdDeActivate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDeActivate.Click
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox
        Dim objCat As New ContCat
        Dim dtItem As New DataTable
        Dim strDeleted, strNotDeleted As String
        Dim dtr As DataRow
        dtItem.Columns.Add("ProductCode", Type.GetType("System.String"))
        dtItem.Columns.Add("CoyId", Type.GetType("System.String"))

        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                dtr = dtItem.NewRow()
                dtr("ProductCode") = dgItem.Cells(6).Text
                dtr("CoyId") = Session("CompanyId")
                dtItem.Rows.Add(dtr)
            End If
        Next

        If objCat.UpdBuyerProductMstr(dtItem, Session("CompanyId"), "Y") = WheelMsgNum.Delete Then
            Common.NetMsgbox(Me, "Item(s) deactivated", MsgBoxStyle.Information)
        ElseIf objCat.UpdBuyerProductMstr(dtItem, Session("CompanyId"), "Y") = WheelMsgNum.NotDelete Then
            Common.NetMsgbox(Me, "Item(s) not deactivated. It has pending PR(s) or PO(s).", MsgBoxStyle.Information)
        Else
            Common.NetMsgbox(Me, "Item(s) not deactivated.", MsgBoxStyle.Information)
        End If
        Bindgrid()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim dgItem As DataGridItem
        Dim dt As New DataTable
        Dim dr As DataRow

        dt.Columns.Add("index", Type.GetType("System.String")) '//BCI_ITEM_INDEX
        dt.Columns.Add("productcode", Type.GetType("System.String")) '//product code
        dt.Columns.Add("bcoyid", Type.GetType("System.String")) '//buyer coy id

        Dim chk As CheckBox
        For Each dgItem In dtgCatalogue.Items
            chk = dgItem.FindControl("chkSelection")

            If chk.Checked Then
                dr = dt.NewRow
                dr("index") = strBCIdx
                dr("productcode") = dgItem.Cells.Item(EnumCat.icProdCode).Text
                dr("bcoyid") = Session("CompanyId")
                dt.Rows.Add(dr)
            End If
        Next

        If objBuyCat.addBuyerCatItem(dt) = WheelMsgNum.Save Then
            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
            Session("Save") = "AfterSave"
        Else
            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
        End If

        ViewState("action") = "del"
        Session("Search") = "No"
        Bindgrid()
    End Sub

    'Private Sub cboCommodityType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCommodityType.SelectedIndexChanged
    '    ViewState("Commodity") = cboCommodityType.SelectedItem.Text
    'End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Session("ImageIndex") = ""
        objCat.deleteAttachment(0, True, Session.SessionID)
        Response.Redirect(dDispatcher.direct("BuyerCat", "BIM.aspx", "mode=" & "add" & "&pageid=" & strPageId))
    End Sub

    Private Sub ChkCloseBtn()
        Dim strscript As New System.Text.StringBuilder

        strscript.Append("<script language=""javascript"">")
        ' strscript.Append("Alert(""df"");")
        If strFrm <> "ItemCat" Then
            strscript.Append("document.getElementById(""cmdClose"").style.display = 'none';")
        Else
            strscript.Append("document.getElementById(""cmdClose"").style.display = '';")
        End If
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())

    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        If strFrm <> "ItemCat" Then
            Session("w_BIM_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                                      "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("BuyerCat", "SearchBItem.aspx", "pageid=" & strPageId) & """><span>Item Listing</span></a></li>" & _
                                      "<li><div class=""space""></div></li>" & _
                                      "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("BuyerCat", "BIMBatchUpload.aspx", "pageid=" & strPageId) & """><span>Item Batch Upload/Download</span></a></li>" & _
                                      "<li><div class=""space""></div></li>" & _
                                      "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("BuyerCat", "BIMAuditTrail.aspx", "pageid=" & strPageId) & """><span>Audit</span></a></li>" & _
                                      "<li><div class=""space""></div></li>" & _
                                      "</ul><div></div></div>"
        Else
            Session("w_BIM_tabs") = Nothing
        End If

    End Sub
End Class
