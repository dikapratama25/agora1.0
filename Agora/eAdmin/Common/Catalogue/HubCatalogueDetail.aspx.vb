Imports AgoraLegacy

Public Class HubCatalogueDetail
    Inherits AgoraLegacy.AppBaseClass

    Public Enum EnumCat
        icChk = 0
        icItemCode = 1
        icVendorItemCode = 2
        icItemDesc = 3
        icCurrency = 4
        icPrice = 5
        icUOM = 6
        icRemark = 7
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblDesc As System.Web.UI.WebControls.Label
    Protected WithEvents lblStartDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblEndDate As System.Web.UI.WebControls.Label
    Protected WithEvents dtgCatalogue As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents lblBuyer As System.Web.UI.WebControls.Label
    Protected WithEvents trBuyer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents txtCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents revCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents revDesc As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtStartDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents revStartDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents txtEndDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents revEndDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents trCode As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDate As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trCodeRead As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDateRead As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cboBuyer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents revBuyer As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents trBuyerRead As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents hidDelete As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidComCnt As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidCatType As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents cmdCompany As System.Web.UI.WebControls.Button
    Protected WithEvents lblCodeLabel As System.Web.UI.WebControls.Label
    Protected WithEvents hidCnt As System.Web.UI.HtmlControls.HtmlInputHidden
    Dim dDispatcher = New dispatcher
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCatalogue)

        If Not IsPostBack Then
            viewstate("from") = Request.UrlReferrer
            Dim objCat As New ContCat
            viewstate("index") = Request.QueryString("index")
            viewstate("cattype") = Request.QueryString("cattype")
            viewstate("mode") = Request.QueryString("mode")
            cmdModify.Visible = False
            cmdDelete.Visible = False
            cmdSave.Visible = False

            trBuyer.Visible = False
            trCode.Visible = False
            trDate.Visible = False

            If viewstate("cattype") = "C" Then
                lblCodeLabel.Text = "Contract Ref. No."
                lblTitle.Text = "Contract Group Items"
                lblHeader.Text = "Contract Group Header"
                'Dim dsCat As New DataSet
                'dsCat = objCat.getBuyerCompany(Session("CompanyIdToken"))
                'Common.FillDdl(cboBuyer, "CM_COY_NAME", "CM_COY_ID", dsCat)

                'Dim lstItem As New ListItem
                'lstItem.Value = ""
                'lstItem.Text = "---Select---"
                'cboBuyer.Items.Insert(0, lstItem)
                cmdCompany.Visible = False

                'trCodeRead.Visible = False
                'trDateRead.Visible = False
                'trBuyerRead.Visible = False
                displayMaster()
                Bindgrid()
                
            Else
                lblCodeLabel.Text = "Discount Group Code"
                lblHeader.Text = "Discount Group Header"
                'cmdSave.Visible = False
                viewstate("companycnt") = objCat.getCompanyCount(viewstate("index"))
                lblTitle.Text = "Discount Group Items"
                'trBuyer.Visible = False
                trBuyerRead.Visible = False
                'trCode.Visible = False
                'trDate.Visible = False
                displayMaster()
                Bindgrid()
            End If
        End If
        hidComCnt.Value = viewstate("companycnt")
        hidCatType.Value = viewstate("cattype")
        'lnkBack.NavigateUrl = "HubCatalogueList.aspx?cattype=" & viewstate("cattype") & "&pageid=" & strPageId
        lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "HubCatalogueList.aspx", "cattype=" & ViewState("cattype") & "&pageid=" & strPageId)
        cmdDelete.Attributes.Add("onclick", "return CheckDeleteMaster('chkSelection');")
        cmdModify.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
    End Sub

    Private Function displayMaster()
        Dim objCat As New ContCat
        Dim ds As New DataSet
        ds = objCat.getSingleCatalogue(viewstate("index"), viewstate("cattype"))

        If ds.Tables(0).Rows.Count > 0 Then
            viewstate("oldCode") = Common.parseNull(ds.Tables(0).Rows(0)("CDM_GROUP_CODE"))
            lblCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("CDM_GROUP_CODE"))
            lblDesc.Text = Common.parseNull(ds.Tables(0).Rows(0)("CDM_GROUP_DESC"))
            lblStartDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds.Tables(0).Rows(0)("CDM_START_DATE"))
            lblEndDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds.Tables(0).Rows(0)("CDM_END_DATE"))

            If viewstate("cattype") = "C" Then
                lblBuyer.Text = Common.parseNull(ds.Tables(0).Rows(0)("CM_COY_NAME"))
            End If
            'Select Case viewstate("cattype")
            '    Case "C"
            '        'txtCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("CDM_GROUP_CODE"))
            '        'txtDesc.Text = Common.parseNull(ds.Tables(0).Rows(0)("CDM_GROUP_DESC"))
            '        'txtStartDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds.Tables(0).Rows(0)("CDM_START_DATE"))
            '        'txtEndDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds.Tables(0).Rows(0)("CDM_END_DATE"))
            '        'Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("CDC_B_COY_ID")), cboBuyer, True, True)

            '    Case "D"
            '        'lblCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("CDM_GROUP_CODE"))
            '        'lblDesc.Text = Common.parseNull(ds.Tables(0).Rows(0)("CDM_GROUP_DESC"))
            '        'lblStartDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds.Tables(0).Rows(0)("CDM_START_DATE"))
            '        'lblEndDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds.Tables(0).Rows(0)("CDM_END_DATE"))
            'End Select
        End If
    End Function

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objCat As New ContCat
        Dim ds As New DataSet
        ds = objCat.getCatalogueItemList(viewstate("index"))
        intPageRecordCnt = ds.Tables(0).Rows.Count
        viewstate("intPageRecordCnt") = intPageRecordCnt
        hidCnt.Value = intPageRecordCnt

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
            dtgCatalogue.DataSource = dvViewSample
            dtgCatalogue.DataBind()
            cmdDelete.Visible = True
            cmdModify.Visible = True
        Else
            'Common.NetMsgbox(Me, MsgNoRecord)
            dtgCatalogue.DataBind()
            cmdDelete.Enabled = False
        End If
    End Function

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
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim lnkCode As HyperLink
            lnkCode = e.Item.FindControl("lnkCode")
            lnkCode.Text = dv("CDI_PRODUCT_CODE")
            ' ai chu modified on 13/10/2005
            ' item retrieved from CONTRACT_DIST_ITEMS
            'lnkCode.Attributes.Add("onclick", "window.open('ProductDetail.aspx?companyType=V&pid=" & lnkCode.Text & "&index=" & viewstate("index") & "&draft=0',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');")
            lnkCode.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("Catalogue", "ProductDetail.aspx", "companyType=V&pid=" & lnkCode.Text & "&index=" & ViewState("index") & "&draft=0',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');"))
            lnkCode.NavigateUrl = "#"

            e.Item.Cells(EnumCat.icPrice).Text = Format(CDbl(e.Item.Cells(EnumCat.icPrice).Text), "###,###,##0.0000")
        End If
    End Sub

    Private Sub dtgCatalogue_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCatalogue.PageIndexChanged
        dtgCatalogue.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgCatalogue.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Function saveCatalogueMaster() As Integer
        Dim dtMaster As New DataTable
        dtMaster.Columns.Add("index", Type.GetType("System.Int32"))
        dtMaster.Columns.Add("GroupCode", Type.GetType("System.String"))
        dtMaster.Columns.Add("GroupDesc", Type.GetType("System.String"))
        dtMaster.Columns.Add("VendorID", Type.GetType("System.String"))
        dtMaster.Columns.Add("CatType", Type.GetType("System.String"))
        dtMaster.Columns.Add("StartDate", Type.GetType("System.String"))
        dtMaster.Columns.Add("EndDate", Type.GetType("System.String"))
        dtMaster.Columns.Add("BuyerID", Type.GetType("System.String"))

        Dim dtr As DataRow
        dtr = dtMaster.NewRow()
        If Not viewstate("index") Is Nothing Then
            dtr("index") = viewstate("index")
        Else
            dtr("index") = 0
        End If
        dtr("GroupCode") = txtCode.Text
        dtr("GroupDesc") = txtDesc.Text
        dtr("VendorID") = HttpContext.Current.Session("CompanyIdToken")
        dtr("CatType") = viewstate("cattype")
        dtr("StartDate") = txtStartDate.Text
        dtr("EndDate") = txtEndDate.Text
        dtr("BuyerID") = cboBuyer.SelectedItem.Value
        dtMaster.Rows.Add(dtr)

        Dim objCat As New ContCat
        Dim intMsg As Integer
        Select Case viewstate("mode")
            Case "add"
                intMsg = objCat.insertCatalogue(dtMaster)
                viewstate("index") = objCat.getLatestIndex("DC")
            Case "mod"
                intMsg = objCat.updateCatalogue(dtMaster, viewstate("oldCode"))
        End Select
        saveCatalogueMaster = intMsg
    End Function

    'Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
    '    Dim intMsg As Integer
    '    Dim strRedirect As String = "HubAddDiscountItem.aspx?index=" & viewstate("index") & "&type=" & viewstate("type") & "&pageid=" & strPageId
    '    intMsg = saveCatalogueMaster()
    '    Select Case intMsg
    '        Case WheelMsgNum.Save
    '            Common.NetPrompt(Me, MsgRecordSave & """& vbCrLf & ""Do you want to add item ?", strRedirect, "Wheel")
    '        Case WheelMsgNum.NotSave
    '            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information, "Wheel")
    '        Case WheelMsgNum.Duplicate
    '            Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Information, "Wheel")
    '    End Select
    'End Sub

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim strItemList As String
        Dim objCat As New ContCat
        Dim strIndex As String
        Dim strCurrency As String = ""
        Dim chkItem As CheckBox
        Dim txtPrice As TextBox
        Dim dblUnitPrice As Double

        strItemList = ""
        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                strItemList &= CType(dgItem.FindControl("lnkCode"), HyperLink).Text & ","
            End If
        Next
        Me.Session.Add("ContractItemCodeList", strItemList)
        'Response.Redirect("HubContractPrice.aspx?mode=mod&cattype=" & viewstate("cattype") & "&index=" & viewstate("index") & "&pageid=" & strPageId)
        Response.Redirect(dDispatcher.direct("Catalogue", "HubContractPrice.aspx", "mode=mod&cattype=" & ViewState("cattype") & "&index=" & ViewState("index") & "&pageid=" & strPageId))
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim objCat As New ContCat
        Dim chkItem As CheckBox
        Dim dtItem As New DataTable
        dtItem.Columns.Add("index", Type.GetType("System.Int32"))
        dtItem.Columns.Add("ContractCode", Type.GetType("System.String"))
        dtItem.Columns.Add("ProductCode", Type.GetType("System.String"))
        dtItem.Columns.Add("CoyId", Type.GetType("System.String"))

        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                Dim dtr As DataRow
                dtr = dtItem.NewRow()
                dtr("index") = viewstate("index")
                dtr("ContractCode") = viewstate("code")
                dtr("ProductCode") = CType(dgItem.FindControl("lnkCode"), HyperLink).Text
                dtr("CoyId") = Session("CompanyIdToken")
                dtItem.Rows.Add(dtr)
            End If
        Next

        Dim strDeleted, strNotDeleted As String
        If hidDelete.Value = "0" Then
            objCat.deteleCatalogueItem(False, dtItem, strDeleted, strNotDeleted)
            Me.Session.Add("ItemDeleted", strDeleted)
            Me.Session.Add("ItemNotDeleted", strNotDeleted)
            'Response.Redirect("HubCatalogueConfirm.aspx?type=" & viewstate("cattype") & "&act=6&code=" & viewstate("code") & "&index=" & viewstate("index") & "&MasterDelete=0&pageid=" & strPageId)
            Response.Redirect(dDispatcher.direct("Catalogue", "HubCatalogueConfirm.aspx", "type=" & ViewState("cattype") & "&act=6&code=" & ViewState("code") & "&index=" & ViewState("index") & "&MasterDelete=0&pageid=" & strPageId))
            'Response.Redirect("CatalogueConfirm.aspx?code=" & viewstate("code") & "&role=D&act=1&pageid=" & strPageId)
            'If objCat.deteleCatalogueItem(False, dtItem) = WheelMsgNum.Delete Then
            '    Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information, "Wheel")
            '    viewstate("action") = "del"
            '    Bindgrid()
            'Else
            '    Common.NetMsgbox(Me, MsgRecordNotDelete, MsgBoxStyle.Information, "Wheel")
            'End If
            'Bindgrid()
        Else ' delete master record
            objCat.deteleCatalogueItem(True, dtItem, strDeleted, strNotDeleted)
            Me.Session.Add("ItemDeleted", strDeleted)
            Me.Session.Add("ItemNotDeleted", strNotDeleted)
            'Response.Redirect("HubCatalogueConfirm.aspx?type=" & viewstate("cattype") & "&act=6&code=" & viewstate("code") & "&index=" & viewstate("index") & "&MasterDelete=1&pageid=" & strPageId)
            Response.Redirect(dDispatcher.direct("Catalogue", "HubCatalogueConfirm.aspx", "type=" & ViewState("cattype") & "&act=6&code=" & ViewState("code") & "&index=" & ViewState("index") & "&MasterDelete=1&pageid=" & strPageId))
            'Response.Redirect("CatalogueConfirm.aspx?code=" & viewstate("code") & "&role=D&act=1&pageid=" & strPageId)
            'If objCat.deteleCatalogueItem(True, dtItem) = WheelMsgNum.Delete Then
            '    Common.NetMsgbox(Me, MsgRecordDelete, "HubCatalogueList.aspx?cattype=" & viewstate("cattype") & "&pageid=" & strPageId, MsgBoxStyle.Information, "Wheel")
            'Else
            '    Common.NetMsgbox(Me, MsgRecordNotDelete, MsgBoxStyle.Information, "Wheel")
            'End If
        End If
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        'Dim intMsg As Integer
        'If viewstate("cattype") = "C" Then
        '    intMsg = saveCatalogueMaster()
        '    Select Case intMsg
        '        Case WheelMsgNum.Save
        '            Dim strRedirect As String = "HubAddDiscountItem.aspx?index=" & viewstate("index") & "&cattype=" & viewstate("cattype") & "&pageid=" & strPageId
        '            Common.NetMsgbox(Me, MsgRecordSave, strRedirect, MsgBoxStyle.Information, "Wheel")
        '        Case WheelMsgNum.NotSave
        '            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information, "Wheel")
        '        Case WheelMsgNum.Duplicate
        '            Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Information, "Wheel")
        '    End Select
        'Else
        '    Response.Redirect("HubAddDiscountItem.aspx?index=" & viewstate("index") & "&cattype=" & viewstate("cattype") & "&pageid=" & strPageId)
        'End If
        'Response.Redirect("HubAddDiscountItem.aspx?code=" & viewstate("code") & "&index=" & viewstate("index") & "&cattype=" & viewstate("cattype") & "&pageid=" & strPageId)
        Response.Redirect(dDispatcher.direct("Catalogue", "HubAddDiscountItem.aspx", "code=" & ViewState("code") & "&index=" & ViewState("index") & "&cattype=" & ViewState("cattype") & "&pageid=" & strPageId))
    End Sub

    Private Sub cmdCompany_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCompany.Click
        'Response.Redirect("HubCompanyAssign.aspx?caller=item&index=" & viewstate("index") & "&pageid=" & strPageId)
        Response.Redirect(dDispatcher.direct("Catalogue", "HubCompanyAssign.aspx", "caller=item&index=" & ViewState("index") & "&pageid=" & strPageId))
    End Sub
End Class
