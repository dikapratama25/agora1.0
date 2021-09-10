Imports eProcure.Component
Imports AgoraLegacy

Public Class ContractItem
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Public Enum EnumCat
        icChk = 0
        icItemCode = 1
        icItemDesc = 2
        icCurrency = 3
        icPrice = 4
        iGST = 5
        iGSTRate = 6
        iGSTTaxCode = 7
        icUOM = 8
        icRemark = 9
        icProdCode = 10
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
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents lblBuyer As System.Web.UI.WebControls.Label
    'Protected WithEvents trBuyer As System.Web.UI.HtmlControls.HtmlTableRow
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
    'Protected WithEvents trCode As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents trDate As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents trCodeRead As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents trDateRead As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents revBuyer As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
    'Protected WithEvents trBuyerRead As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents hidDelete As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidComCnt As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidCatType As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lblCodeLabel As System.Web.UI.WebControls.Label
    Protected WithEvents hidCnt As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents btnHidden As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Dim cbolist As New ListItem

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

        'cmdDelete.Enabled = False
        'cmdModify.Enabled = False
        If Not IsPostBack Then
            cmdAdd.Enabled = False
            ViewState("index") = ""
            Dim objGst As New GST
            ViewState("GSTCOD") = objGst.chkGSTCOD()
            objGst = Nothing
            GenerateTab()
            ViewState("from") = Request.UrlReferrer
            Dim objCat As New ContCat
            Dim dsCat As New DataSet
            dsCat = objCat.getConRefNo()
            Common.FillDdl(ddlCode, "CDM_GROUP_CODE", "CDM_GROUP_INDEX", dsCat)
            cbolist.Value = ""
            cbolist.Text = "---Select---"
            ddlCode.Items.Insert(0, cbolist)
            'trBuyer.Visible = False
            'trCode.Visible = False
            'trDate.Visible = False            
        End If
        hidComCnt.Value = 0 'ViewState("companycnt")
        hidCatType.Value = "C" 'ViewState("cattype")
        'lnkBack.NavigateUrl = "HubCatalogueList.aspx?cattype=" & viewstate("cattype") & "&pageid=" & strPageId
        'lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "HubCatalogueList.aspx", "cattype=" & ViewState("cattype") & "&pageid=" & strPageId)
        cmdDelete.Attributes.Add("onclick", "return CheckDeleteMaster('chkSelection');")
        cmdModify.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objCat As New ContCat
        Dim ds As New DataSet
        ds = objCat.getCatalogueItemList(ddlCode.SelectedValue)
        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt
        hidCnt.Value = intPageRecordCnt

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        ''//these only needed if you can select a grid item and click delete button
        ''//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        ''//then user delete one record. //total record = 20 (2 pages), 
        ''//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If ViewState("action") = "del" Then
            If dtgCatalogue.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgCatalogue.PageSize = 0 Then
                dtgCatalogue.CurrentPageIndex = dtgCatalogue.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        If intPageRecordCnt > 0 Then
            dtgCatalogue.DataSource = dvViewSample
            dtgCatalogue.DataBind()
            cmdDelete.Enabled = blnCanDelete
            cmdModify.Enabled = blnCanUpdate
        Else
            'Common.NetMsgbox(Me, MsgNoRecord)
            dtgCatalogue.DataBind()
            cmdDelete.Enabled = False
            cmdModify.Enabled = False
        End If

        If ViewState("GSTCOD") = True Then
            dtgCatalogue.Columns(EnumCat.iGST).Visible = False
            dtgCatalogue.Columns(EnumCat.iGSTRate).Visible = True
            dtgCatalogue.Columns(EnumCat.iGSTTaxCode).Visible = True
        Else
            dtgCatalogue.Columns(EnumCat.iGST).Visible = True
            dtgCatalogue.Columns(EnumCat.iGSTRate).Visible = False
            dtgCatalogue.Columns(EnumCat.iGSTTaxCode).Visible = False
        End If
    End Function

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
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim lnkCode As HyperLink
            lnkCode = e.Item.FindControl("lnkCode")
            lnkCode.Text = dv("CDI_VENDOR_ITEM_CODE")
            ' ai chu modified on 13/10/2005
            ' item retrieved from CONTRACT_DIST_ITEMS
            'lnkCode.Attributes.Add("onclick", "window.open('ProductDetail.aspx?companyType=V&pid=" & lnkCode.Text & "&index=" & viewstate("index") & "&draft=0',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');")
            'lnkCode.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=B&pageid=" & strPageId & "&pid=" & dv("CDI_PRODUCT_CODE") & "&index=&draft=0',  '', 'resizable=no,scrollbars=yes,width=750,height=600,status=no,menubar=no');"))
            lnkCode.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=B&pageid=" & strPageId & "&pid=" & dv("CDI_PRODUCT_CODE") & "&index=&draft=0&ref=" & ddlCode.SelectedItem.Text & "',  '', 'resizable=no,scrollbars=yes,width=750,height=600,status=no,menubar=no');"))

            lnkCode.NavigateUrl = "#"

            e.Item.Cells(EnumCat.icPrice).Text = Format(CDbl(e.Item.Cells(EnumCat.icPrice).Text), "###,###,##0.0000")
            If Not IsDBNull(dv("CDI_GST")) Then
                e.Item.Cells(EnumCat.iGST).Text = Format(CDbl(dv("CDI_GST")), "##0")
            End If
            Session("UrlLocation") = "ContractItem"
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

    'Private Function saveCatalogueMaster() As Integer
    '    Dim dtMaster As New DataTable
    '    dtMaster.Columns.Add("index", Type.GetType("System.Int32"))
    '    dtMaster.Columns.Add("GroupCode", Type.GetType("System.String"))
    '    dtMaster.Columns.Add("GroupDesc", Type.GetType("System.String"))
    '    dtMaster.Columns.Add("VendorID", Type.GetType("System.String"))
    '    dtMaster.Columns.Add("CatType", Type.GetType("System.String"))
    '    dtMaster.Columns.Add("StartDate", Type.GetType("System.String"))
    '    dtMaster.Columns.Add("EndDate", Type.GetType("System.String"))
    '    dtMaster.Columns.Add("BuyerID", Type.GetType("System.String"))

    '    Dim dtr As DataRow
    '    dtr = dtMaster.NewRow()
    '    If Not ViewState("index") Is Nothing Then
    '        dtr("index") = ViewState("index")
    '    Else
    '        dtr("index") = 0
    '    End If
    '    dtr("GroupCode") = txtCode.Text
    '    dtr("GroupDesc") = txtDesc.Text
    '    dtr("VendorID") = HttpContext.Current.Session("CompanyIdToken")
    '    dtr("CatType") = ViewState("cattype")
    '    dtr("StartDate") = txtStartDate.Text
    '    dtr("EndDate") = txtEndDate.Text
    '    dtr("BuyerID") = ddlCode.SelectedItem.Value
    '    dtMaster.Rows.Add(dtr)

    '    Dim objCat As New ContCat
    '    Dim intMsg As Integer
    '    Select Case ViewState("mode")
    '        Case "add"
    '            intMsg = objCat.insertCatalogue(dtMaster)
    '            ViewState("index") = objCat.getLatestIndex("DC")
    '        Case "mod"
    '            intMsg = objCat.updateCatalogue(dtMaster, ViewState("oldCode"))
    '    End Select
    '    saveCatalogueMaster = intMsg
    'End Function

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

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim objCat As New ContCat
        Dim chkItem As CheckBox
        Dim dtItem As New DataTable

        dtItem.Columns.Add("index", Type.GetType("System.Int32"))
        'dtItem.Columns.Add("ContractCode", Type.GetType("System.String"))
        dtItem.Columns.Add("ProductCode", Type.GetType("System.String"))
        dtItem.Columns.Add("CoyId", Type.GetType("System.String"))

        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                Dim dtr As DataRow
                dtr = dtItem.NewRow()
                dtr("index") = ViewState("index")
                'dtr("ContractCode") = ViewState("code")
                dtr("ProductCode") = dgItem.Cells(EnumCat.icProdCode).Text
                dtr("CoyId") = Session("CompanyId")
                dtItem.Rows.Add(dtr)
            End If
        Next

        If hidDelete.Value = "0" Then
            'objCat.deteleConCatalogueItem(False, dtItem)
            'Me.Session.Add("ItemDeleted", strDeleted)
            'Me.Session.Add("ItemNotDeleted", strNotDeleted)
            'Response.Redirect("HubCatalogueConfirm.aspx?type=" & viewstate("cattype") & "&act=6&code=" & viewstate("code") & "&index=" & viewstate("index") & "&MasterDelete=0&pageid=" & strPageId)
            'Response.Redirect(dDispatcher.direct("Catalogue", "HubCatalogueConfirm.aspx", "type=" & ViewState("cattype") & "&act=6&code=" & ViewState("code") & "&index=" & ViewState("index") & "&MasterDelete=0&pageid=" & strPageId))
            'Response.Redirect("CatalogueConfirm.aspx?code=" & viewstate("code") & "&role=D&act=1&pageid=" & strPageId)
            If objCat.deteleConCatalogueItem(False, dtItem) = WheelMsgNum.Delete Then
                Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information, "Wheel")
                ViewState("action") = "del"
                Bindgrid()
                'Dim dsCat As New DataSet
                'dsCat = objCat.getConRefNo()
                'Common.FillDdl(ddlCode, "CDM_GROUP_CODE", "CDM_GROUP_INDEX", dsCat)
                'cbolist.Value = ""
                'cbolist.Text = "---Select---"
                'ddlCode.Items.Insert(0, cbolist)
            Else
                Common.NetMsgbox(Me, RecordDeleteNotAllowed, MsgBoxStyle.Information, "Wheel")
            End If
            'Bindgrid()
        Else ' delete master record
            'objCat.deteleConCatalogueItem(True, dtItem)
            'Me.Session.Add("ItemDeleted", strDeleted)
            'Me.Session.Add("ItemNotDeleted", strNotDeleted)
            'Response.Redirect("HubCatalogueConfirm.aspx?type=" & viewstate("cattype") & "&act=6&code=" & viewstate("code") & "&index=" & viewstate("index") & "&MasterDelete=1&pageid=" & strPageId)
            'Response.Redirect(dDispatcher.direct("Catalogue", "HubCatalogueConfirm.aspx", "type=" & ViewState("cattype") & "&act=6&code=" & ViewState("code") & "&index=" & ViewState("index") & "&MasterDelete=1&pageid=" & strPageId))
            'Response.Redirect("CatalogueConfirm.aspx?code=" & viewstate("code") & "&role=D&act=1&pageid=" & strPageId)
            If objCat.deteleConCatalogueItem(True, dtItem) = WheelMsgNum.Delete Then
                Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information, "Wheel")
                ViewState("action") = "del"
                Bindgrid()
                Dim dsCat As New DataSet
                dsCat = objCat.getConRefNo()
                Common.FillDdl(ddlCode, "CDM_GROUP_CODE", "CDM_GROUP_INDEX", dsCat)
                cbolist.Value = ""
                cbolist.Text = "---Select---"
                ddlCode.Items.Insert(0, cbolist)
            Else
                Common.NetMsgbox(Me, RecordDeleteNotAllowed, MsgBoxStyle.Information, "Wheel")
            End If
        End If

    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("Catalogue", "AddContractItem.aspx", "mode=add" & "&index=" & ViewState("index"))
        strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
        strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','630px');")
        strscript.Append("document.getElementById('btnHidden').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script13", strscript.ToString())

    End Sub

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
                strItemList &= dgItem.Cells(EnumCat.icProdCode).Text & ","
            End If
        Next
        Me.Session.Add("ContractItemCodeList", strItemList)
        'Response.Redirect("HubContractPrice.aspx?mode=mod&cattype=" & viewstate("cattype") & "&index=" & viewstate("index") & "&pageid=" & strPageId)
        'Response.Redirect(dDispatcher.direct("Catalogue", "HubContractPrice.aspx", "mode=mod&cattype=" & ViewState("cattype") & "&index=" & ViewState("index") & "&pageid=" & strPageId))
        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("Catalogue", "AddContractItem.aspx", "mode=mod" & "&index=" & ViewState("index") & "&ItemList=" & strItemList)
        strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
        strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','630px');")
        strscript.Append("document.getElementById('btnHidden').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script13", strscript.ToString())
    End Sub

    Private Sub ddlCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCode.SelectedIndexChanged
        If ddlCode.SelectedIndex > 0 Then
            cmdAdd.Enabled = True
            ViewState("index") = ddlCode.SelectedValue
            ViewState("SortAscending") = "yes"
            ViewState("SortExpression") = "CDI_VENDOR_ITEM_CODE"
            dtgCatalogue.CurrentPageIndex = 0
            Bindgrid()

        Else
            intPageRecordCnt = 0
            dtgCatalogue.DataBind()
            cmdAdd.Enabled = False
            cmdDelete.Enabled = False
            cmdModify.Enabled = False
        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_ConCat_tabs") = "<div class=""t_entity""><ul>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "pageid=" & strPageId) & """><span>Contract Catalogue</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Catalogue", "ContractItem.aspx", "pageid=" & strPageId) & """><span>Items Assignment</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "ConCat_AssignUser.aspx", "pageid=" & strPageId) & """><span>Users Assignment</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "ConCat_AssignMultiUser.aspx", "pageid=" & strPageId) & """><span>Multi Users Assignment</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "ConCatBatchUploadDownload.aspx", "pageid=" & strPageId) & """><span>Batch Upload/Download</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "AuditTrail.aspx", "pageid=" & strPageId) & """><span>Audit</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "</ul><div></div></div>"

    End Sub

    Private Sub ContractItem_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        cmdDelete.Enabled = False
        cmdModify.Enabled = False
        Dim alButtonList As ArrayList
        'alButtonList = New ArrayList
        'alButtonList.Add(cmdAdd)
        'htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdModify)
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdDelete)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        '//additional checking
        If intPageRecordCnt > 0 Then
            cmdDelete.Enabled = blnCanDelete
            cmdModify.Enabled = blnCanUpdate
            '//mean Enable, can't use button.Enabled because this is a HTML button
        Else
            cmdDelete.Enabled = False
            cmdModify.Enabled = False
        End If
    End Sub

    Public Sub btnHidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden.Click
        dtgCatalogue.CurrentPageIndex = 0
        Bindgrid()

    End Sub
End Class


