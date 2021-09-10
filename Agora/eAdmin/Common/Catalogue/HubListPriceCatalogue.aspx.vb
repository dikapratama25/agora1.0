
Imports AgoraLegacy
Imports SSO.Component



Public Class HubListPriceCatalogue
    Inherits AgoraLegacy.AppBaseClass
    Dim objCat As New ContCat

    Public Enum EnumCat
        icChk = 0
        icItemCode = 1
        icItemIndex = 2
        icItemDesc = 3
        icVendorItemCode = 4
        icCurrency = 5
        icPrice = 6
        icUOM = 7
        icRemark = 8
        icSubmissionDate = 9
        icAction = 10
        icStatus = 11
        icStatusNo = 12
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtItemId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVendorItemCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkHubPending As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkReject As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkHubApprove As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents dtgCatalogue As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSelectAll As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents trAdd As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents trStatus As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents hidType As System.Web.UI.HtmlControls.HtmlInputHidden
    Dim dDispatcher As New AgoraLegacy.dispatcher

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
        If Session("CompanyIdToken") = "" Then
            Session("CompanyIdToken") = Session("CompanyIDBkup")
        End If
        Dim objComs As New Companies
        Dim strCompanyType As String
        strCompanyType = objComs.GetCompanyType(Session("CompanyIdToken"))
        If strCompanyType.ToUpper = "VENDOR" Or strCompanyType.ToUpper = "BOTH" Then
            SetGridProperty(dtgCatalogue)
            If Not IsPostBack Then
                viewstate("type") = Request.QueryString("type")
                hidType.Value = viewstate("type")
                Select Case viewstate("type")
                    Case "A" ' for approval
                        lblTitle.Text = "Vendor Item Master Approval"
                        trAdd.Visible = False
                        chkHubPending.Checked = True
                        cmdSearch_Click(sender, e)

                    Case "O" ' others - add/modify/delete
                        lblTitle.Text = "List Price Catalogue"
                        cmdSelectAll.Visible = False
                        trStatus.Visible = False
                        cmdModify.Enabled = False
                        cmdDelete.Enabled = False
                        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
                        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
                End Select
            End If
        Else
            viewstate("type") = Request.QueryString("type")
            Dim strMsg As String
            strMsg = objCat.BuyerDisallowedMsg("L", IIf(viewstate("type") = "A", True, False))
            'Common.NetMsgbox(Me, strMsg, "../Homepage.aspx", MsgBoxStyle.Exclamation)
            Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Initial", "Homepage.aspx"), MsgBoxStyle.Exclamation)
        End If
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
        If viewstate("type") = "A" Then
            ds = objCat.getListPriceApproval(Session("CompanyIdToken"), getStatusList, txtItemId.Value, txtDesc.Text, txtVendorItemCode.Text)
        Else
            ds = objCat.getListPrice(Session("CompanyIdToken"), "3", txtItemId.Value, txtDesc.Text, txtVendorItemCode.Text)
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        viewstate("intPageRecordCnt") = intPageRecordCnt

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            ' check when user re-enter search criteria and click on other page without click search button
            If dtgCatalogue.CurrentPageIndex > (dvViewSample.Count \ dtgCatalogue.PageSize) Then
                dtgCatalogue.CurrentPageIndex = IIf((dvViewSample.Count \ dtgCatalogue.PageSize) = 1, 0, (dvViewSample.Count \ dtgCatalogue.PageSize))
            ElseIf dtgCatalogue.CurrentPageIndex = (dvViewSample.Count \ dtgCatalogue.PageSize) Then
                If viewstate("PageCount") = (dvViewSample.Count \ dtgCatalogue.PageSize) Then
                    'user does not re-enter search criteria 
                    dtgCatalogue.CurrentPageIndex = IIf((dvViewSample.Count \ dtgCatalogue.PageSize) = 0, 0, (dvViewSample.Count \ dtgCatalogue.PageSize) - 1)
                Else
                    If (dvViewSample.Count Mod dtgCatalogue.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dtgCatalogue.CurrentPageIndex = IIf((dvViewSample.Count \ dtgCatalogue.PageSize) = 1, 0, (dvViewSample.Count \ dtgCatalogue.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        dtgCatalogue.CurrentPageIndex = (dvViewSample.Count \ dtgCatalogue.PageSize)
                    End If
                End If
            End If
            '--------------------------------
            dtgCatalogue.DataSource = dvViewSample
            dtgCatalogue.DataBind()
            cmdModify.Enabled = True
            cmdDelete.Enabled = True
        Else
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
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

        'Select Case viewstate("type")
        '    Case "A"
        '        e.Item.Cells(EnumCat.icChk).Visible = False
        '    Case "O"
        '        e.Item.Cells(EnumCat.icRemark).Visible = False
        '        e.Item.Cells(EnumCat.icAction).Visible = False
        'End Select

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

            Dim strItemIndex As String
            strItemIndex = e.Item.Cells(EnumCat.icItemIndex).Text

            Dim lnkCode As HyperLink
            lnkCode = e.Item.FindControl("lnkCode")
            lnkCode.Text = Common.parseNull(dv("PM_VENDOR_ITEM_CODE"))

            'Dim lblCode As Label
            'lblCode = e.Item.FindControl("lblCode")
            'lblCode.Text = Common.parseNull(dv("PM_PRODUCT_CODE"))

            Select Case viewstate("type")
                Case "A"
                    Dim strAction As String
                    Dim strStatus As String
                    strAction = e.Item.Cells(EnumCat.icAction).Text
                    strStatus = e.Item.Cells(EnumCat.icStatusNo).Text
                    e.Item.Cells(EnumCat.icSubmissionDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, e.Item.Cells(EnumCat.icSubmissionDate).Text)
                    Session("ImageIndex") = objCat.getItemTempImage(Common.parseNull(dv("PM_PRODUCT_CODE")))
                    'lnkCode.NavigateUrl = "HubListPriceApproval.aspx?pageid=" & strPageId & "&action=" & strAction & "&type=A&itemid=" & Common.parseNull(dv("PM_PRODUCT_CODE")) & "&itemindex=" & strItemIndex & "&status=" & strStatus
                    If strStatus = "3" Then
                        lnkCode.NavigateUrl = "javascript:;"
                        lnkCode.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Catalogue", "ProductDetail.aspx", "companyType=B&pageid=" & strPageId & "&pid=" & dv("PM_PRODUCT_CODE") & "&index=&draft=0") & "')")
                    Else
                        lnkCode.NavigateUrl = dDispatcher.direct("Catalogue", "HubListPriceApproval.aspx", "pageid=" & strPageId & "&action=" & strAction & "&type=A&itemid=" & Common.parseNull(dv("PM_PRODUCT_CODE")) & "&itemindex=" & strItemIndex & "&status=" & strStatus)
                    End If


                    'lblCode.Visible = False
                    e.Item.Cells(EnumCat.icChk).Visible = False
                    If e.Item.Cells(EnumCat.icAction).Text = "N" Then
                        e.Item.Cells(EnumCat.icAction).Text = "New"
                    Else
                        e.Item.Cells(EnumCat.icAction).Text = "Modified"
                    End If
                Case "O"
                    ' ai chu modified on 13/10/2005
                    ' item retrieved from Product_mstr
                    'lnkCode.Attributes.Add("onclick", "window.open('ProductDetail.aspx?companyType=V&pid=" & lnkCode.Text & "&index=&draft=0',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');")
                    lnkCode.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("Catalogue", "ProductDetail.aspx") & "companyType=V&pid=" & lnkCode.Text & "&index=&draft=0',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');")
                    lnkCode.NavigateUrl = "#"
                    e.Item.Cells(EnumCat.icRemark).Visible = False
                    e.Item.Cells(EnumCat.icAction).Visible = False
                    e.Item.Cells(EnumCat.icStatus).Visible = False
                    e.Item.Cells(EnumCat.icSubmissionDate).Visible = False
            End Select

            If IsDBNull(dv("PM_UNIT_COST")) Then
                e.Item.Cells(EnumCat.icPrice).Text = "N/A"
            Else
                e.Item.Cells(EnumCat.icPrice).Text = Format(CDbl(dv("PM_UNIT_COST")), "###,###,##0.0000")
            End If
        ElseIf e.Item.ItemType = ListItemType.Header Then
            Select Case viewstate("type")
                Case "A"
                    e.Item.Cells(EnumCat.icChk).Visible = False
                Case "O"
                    e.Item.Cells(EnumCat.icRemark).Visible = False
                    e.Item.Cells(EnumCat.icAction).Visible = False
                    e.Item.Cells(EnumCat.icStatus).Visible = False
                    e.Item.Cells(EnumCat.icSubmissionDate).Visible = False
            End Select
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
            strItemID = CType(dgItem.FindControl("lnkCode"), HyperLink).Text
            strItemIndex = dgItem.Cells(EnumCat.icItemIndex).Text
            If chkItem.Checked Then
                ' ai chu add on 20/09/2005
                ' when hub admin want to modify list price item, 
                ' copy 1 set of attachment from PRODUCT_ATTACHMENT TO PRODUCT_ATTACHMENT_TEMP
                Dim objCat As New ContCat
                Session("ImageIndex") = objCat.copyAttachFile(strItemID)
                'Response.Redirect("HubListPriceApproval.aspx?type=O&mode=mod&pageid=" & strPageId & "&itemid=" & strItemID & "&itemindex=" & strItemIndex)
                Response.Redirect(dDispatcher.direct("Catalogue", "HubListPriceApproval.aspx", "type=O&mode=mod&pageid=" & strPageId & "&itemid=" & strItemID & "&itemindex=" & strItemIndex))
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

        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                dtr = dtItem.NewRow()
                dtr("ProductCode") = CType(dgItem.FindControl("lnkCode"), HyperLink).Text
                dtr("CoyId") = Session("CompanyIdToken")
                dtItem.Rows.Add(dtr)
            End If
        Next

        strDeleted = ""
        strNotDeleted = ""
        objCat.deteleProductMstr(dtItem, Session("CompanyIdToken"), strDeleted, strNotDeleted)
        Me.Session.Add("ItemDeleted", strDeleted)
        Me.Session.Add("ItemNotDeleted", strNotDeleted)
        'Response.Redirect("HubCatalogueConfirm.aspx?type=L&act=6&pageid=" & strPageId)
        Response.Redirect(dDispatcher.direct("Catalogue", "HubCatalogueConfirm.aspx", "type=L&act=6&pageid=" & strPageId))
        'If strNotDeleted <> "" Then ' Items not deleted
        '    Me.Session.Add("ItemNotDeleted", strNotDeleted)
        '    Response.Redirect("HubCatalogueConfirm.aspx?act=6&pageid=" & strPageId)
        'Else
        '    Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information, "Wheel")
        '    Bindgrid()
        'End If
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Session("ImageIndex") = ""
        objCat.deleteAttachment(0, False, Session.SessionID)
        'Response.Redirect("HubListPriceApproval.aspx?type=O&mode=add&pageid=" & strPageId)
        Response.Redirect(dDispatcher.direct("Catalogue", "HubListPriceApproval.aspx", "type=O&mode=add&pageid=" & strPageId))
    End Sub

End Class
