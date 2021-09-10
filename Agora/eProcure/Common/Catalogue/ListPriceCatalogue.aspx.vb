Imports AgoraLegacy
Imports eProcure.Component

Public Class ListPriceCatalogue
    Inherits AgoraLegacy.AppBaseClass
    Dim objCat As New ContCat
    Dim objGlobal As New AppGlobals
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Public Enum EnumCat
        icChk = 0
        icItemIndex = 1
        icItemCode = 2
        icItemDesc = 3
        icVendorItemCode = 4
        icCurrency = 5
        icPrice = 6
        icUOM = 7
        icStatus = 8
        icStatusNo = 9
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkHubPending As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkReject As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkHubApprove As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents dtgCatalogue As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents divApprove2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdSelectAll As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents trDiscount As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtItemId As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVendorItemCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents hidCommodity As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtCommodity As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cboCommodityType As System.Web.UI.WebControls.DropDownList
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
        alButtonList.Add(cmdDelete)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        cmdAdd.Enabled = blnCanAdd And viewstate("blnCmdAdd")
        cmdModify.Enabled = blnCanUpdate And viewstate("blnCmdModify")
        cmdDelete.Enabled = blnCanDelete And viewstate("blnCmdDelete")
        alButtonList.Clear()


    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        'Put user code to initialize the page here
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCatalogue)

        If Not IsPostBack Then
            'objGlobal.FillCommodityType(Me.cboCommodityType)

        End If

        If Not IsPostBack Then
            GenerateTab()
            ViewState("blnCmdAdd") = True
            ViewState("blnCmdModify") = True
            ViewState("blnCmdDelete") = True
            Session("ImageIndex") = ""
            cmdModify.Enabled = False
            ViewState("blnCmdModify") = False
            cmdDelete.Enabled = False
            ViewState("blnCmdDelete") = False
            lblTitle.Text = "Item Listing"
        End If
        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        'cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
    End Sub

    Private Function getStatusList() As String
        Dim strStatus As String = ""
        If chkHubPending.Checked Then
            strStatus = IIf(strStatus = "", ListPriceStatus.PendingApproval, strStatus & "," & ListPriceStatus.PendingApproval)
        End If

        If chkReject.Checked Then
            strStatus = IIf(strStatus = "", ListPriceStatus.Rejected, strStatus & "," & ListPriceStatus.Rejected)
        End If

        If chkHubApprove.Checked Then
            strStatus = IIf(strStatus = "", ListPriceStatus.Approved, strStatus & "," & ListPriceStatus.Approved)
        End If

        getStatusList = strStatus
    End Function

    Private Function Bindgrid() As String
        Dim ds As New DataSet
        Dim strCommodity As String = ""

        If Me.txtCommodity.Text <> "" Then
            strCommodity = Me.hidCommodity.Value
        End If
        ds = objCat.getListPrice(Session("CompanyId"), getStatusList, txtVendorItemCode.Text, txtDesc.Text, strCommodity)
        intPageRecordCnt = ds.Tables(0).Rows.Count
        viewstate("intPageRecordCnt") = intPageRecordCnt

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        ''//these only needed if you can select a grid item and click delete button
        ''//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        ''//then user delete one record. //total record = 20 (2 pages), 
        ''//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If dtgCatalogue.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgCatalogue.PageSize = 0 Then
                dtgCatalogue.CurrentPageIndex = dtgCatalogue.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgCatalogue, dvViewSample)
            dtgCatalogue.DataSource = dvViewSample
            dtgCatalogue.DataBind()
            cmdModify.Enabled = True
            cmdDelete.Enabled = True
            viewstate("blnCmdModify") = True
            viewstate("blnCmdDelete") = True
        Else
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            viewstate("blnCmdModify") = False
            viewstate("blnCmdDelete") = False
            Common.NetMsgbox(Me, MsgNoRecord)
            dtgCatalogue.DataBind()
        End If

        ' add for above checking
        viewstate("PageCount") = dtgCatalogue.PageCount
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_SortCommand(sender, e)
        dtgCatalogue.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgCatalogue_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
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

            Dim lnkCode As HyperLink
            lnkCode = e.Item.FindControl("lnkCode")
            lnkCode.Text = Common.parseNull(dv("PM_PRODUCT_CODE"))
            'lnkCode.Attributes.Add("onclick", "window.open('../Product/ProductDetail.aspx?companyType=V&pid=" & lnkCode.Text & "&index=&draft=0',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');"))
            lnkCode.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=V&pid=" & lnkCode.Text & "&index=&draft=0',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');"))
            lnkCode.NavigateUrl = "#"

            'Michelle (8/11/2010) - To add in the hyperlink in Vendor Item Code
            Dim lnkItemCode As HyperLink
            lnkItemCode = e.Item.FindControl("lnkItemCode")
            lnkItemCode.Text = dv("PM_VENDOR_ITEM_CODE")

            Dim lblCode As Label
            lblCode = e.Item.FindControl("lblCode")
            lblCode.Text = Common.parseNull(dv("PM_PRODUCT_CODE"))

            ' vendor can only modify item that been approved by Admin
            If e.Item.Cells(EnumCat.icStatusNo).Text = "3" Then ' approved by vendor
                chk.Visible = True
                lnkCode.Visible = True
                lblCode.Visible = False
                'lnkItemCode.NavigateUrl = "CatalogueNewProduct.aspx?itemindex=" & dv("PM_PRODUCT_INDEX") & "&itemid=" & dv("PM_PRODUCT_CODE") & "&mode=mod&pageid=" & strPageId
                lnkItemCode.NavigateUrl = dDispatcher.direct("Catalogue", "CatalogueNewProduct.aspx", "itemindex=" & dv("PM_PRODUCT_INDEX") & "&itemid=" & dv("PM_PRODUCT_CODE") & "&mode=mod&pageid=" & strPageId)
            Else
                chk.Visible = False
                lnkCode.Visible = False
                lblCode.Visible = True
            End If

            If IsDBNull(dv("PM_UNIT_COST")) Then
                e.Item.Cells(EnumCat.icPrice).Text = "N/A"
            Else
                e.Item.Cells(EnumCat.icPrice).Text = Format(CDbl(dv("PM_UNIT_COST")), "###,###,##0.0000")
            End If
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgCatalogue.CurrentPageIndex = 0
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
        Dim strItemIndex As String

        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")
            strItemID = CType(dgItem.FindControl("lblCode"), Label).Text
            strItemIndex = dgItem.Cells(EnumCat.icItemIndex).Text
            If chkItem.Checked Then
                'Session("ImageIndex") = objCat.copyAttachFile(strItemID)
                'Response.Redirect("CatalogueNewProduct.aspx?itemindex=" & strItemIndex & "&itemid=" & strItemID & "&mode=mod&pageid=" & strPageId)
                Response.Redirect(dDispatcher.direct("Catalogue", "CatalogueNewProduct.aspx", "itemindex=" & strItemIndex & "&itemid=" & strItemID & "&mode=mod&pageid=" & strPageId))
            End If
        Next
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox
        Dim objCat As New ContCat
        Dim dtItem As New DataTable
        Dim strDeleted, strNotDeleted As String
        Dim dtr As DataRow
        dtItem.Columns.Add("ProductCode", Type.GetType("System.String"))
        dtItem.Columns.Add("CoyId", Type.GetType("System.String"))
        dtItem.Columns.Add("VIC", Type.GetType("System.String"))

        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                dtr = dtItem.NewRow()
                dtr("ProductCode") = CType(dgItem.FindControl("lblCode"), Label).Text
                dtr("CoyId") = Session("CompanyId")
                dtr("VIC") = dgItem.Cells(EnumCat.icVendorItemCode).Text
                dtItem.Rows.Add(dtr)
            End If
        Next

        strDeleted = ""
        strNotDeleted = ""
        objCat.deteleProductMstr(dtItem, Session("CompanyId"), strDeleted, strNotDeleted)
        Me.Session.Add("ItemDeleted", strDeleted)
        Me.Session.Add("ItemNotDeleted", strNotDeleted)
        'Response.Redirect("CatalogueConfirm.aspx?role=L&act=1&pageid=" & strPageId)
        Response.Redirect(dDispatcher.direct("Catalogue", "CatalogueConfirm.aspx", "role=L&act=1&pageid=" & strPageId))
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Session("ImageIndex") = ""
        objCat.deleteAttachment(0, True, Session.SessionID)
        'Response.Redirect("CatalogueNewProduct.aspx?mode=add&pageid=" & strPageId)
        Response.Redirect(dDispatcher.direct("Catalogue", "CatalogueNewProduct.aspx", "mode=add&pageid=" & strPageId))
    End Sub

    Protected Sub cmdSelectAll_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelectAll.ServerClick

    End Sub

    'Private Sub cboCommodityType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCommodityType.SelectedIndexChanged
    '    ViewState("Commodity") = cboCommodityType.SelectedItem.Text
    'End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_VIM_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                          "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Catalogue", "ListPriceCatalogue.aspx", "pageid=" & strPageId) & """><span>Item Listing</span></a></li>" & _
                          "<li><div class=""space""></div></li>" & _
                          "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "CatalogueBatchUpload.aspx", "pageid=" & strPageId) & """><span>Item Batch Upload/Download</span></a></li>" & _
                          "<li><div class=""space""></div></li>" & _
                          "</ul><div></div></div>"
    End Sub

End Class
