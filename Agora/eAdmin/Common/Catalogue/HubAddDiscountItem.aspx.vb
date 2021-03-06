Imports AgoraLegacy

Public Class HubAddDiscountItem
    Inherits AgoraLegacy.AppBaseClass

    Public Enum EnumCat
        icChk = 0
        icItemCode = 1
        icVendorItemCode = 2
        icItemDesc = 3
        icCurrency = 4
        icPrice = 5
        icUOM = 6
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
    Protected WithEvents lblBuyer As System.Web.UI.WebControls.Label
    Protected WithEvents txtVendorItemCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtItemCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtItemDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents dtgCatalogue As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdContract As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblCodeLabel As System.Web.UI.WebControls.Label
    Protected WithEvents trBuyer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
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
            viewstate("index") = Request.QueryString("index")
            viewstate("cattype") = Request.QueryString("cattype")

            Select Case viewstate("cattype")
                Case "C"
                    lblHeader.Text = "Contract Group Header"
                    lblTitle.Text = "Add Contract Item"
                    lblCodeLabel.Text = "Contract Ref. No."
                    cmdContract.Text = "Contract Price"
                Case "D"
                    lblHeader.Text = "Discount Group Header"
                    lblTitle.Text = "Add Discount Item"
                    lblCodeLabel.Text = "Discount Group Code"
                    cmdContract.Text = "Discount Price"
                    trBuyer.Visible = False
            End Select
            displayMaster()
            cmdContract.Enabled = False
        End If
        cmdContract.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
        'lnkBack.NavigateUrl = "HubCatalogueDetail.aspx?cattype=" & viewstate("cattype") & "&mode=mod&index=" & viewstate("index") & "&pageid=" & strPageId
        lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "HubCatalogueDetail.aspx", "cattype=" & ViewState("cattype") & "&mode=mod&index=" & ViewState("index") & "&pageid=" & strPageId)
    End Sub

    Private Sub displayMaster()
        Dim objCat As New ContCat
        Dim dsCat As New DataSet
        dsCat = objCat.getSingleCatalogue(viewstate("index"), viewstate("cattype"))

        If dsCat.Tables(0).Rows.Count > 0 Then
            lblCode.Text = Common.parseNull(dsCat.Tables(0).Rows(0)("CDM_GROUP_CODE"))
            lblDesc.Text = Common.parseNull(dsCat.Tables(0).Rows(0)("CDM_GROUP_DESC"))
            lblStartDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dsCat.Tables(0).Rows(0)("CDM_START_DATE"))
            lblEndDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dsCat.Tables(0).Rows(0)("CDM_END_DATE"))
            If viewstate("cattype") = "C" Then
                lblBuyer.Text = Common.parseNull(dsCat.Tables(0).Rows(0)("CM_COY_NAME"))
                viewstate("BuyerId") = Common.parseNull(dsCat.Tables(0).Rows(0)("CDC_B_COY_ID"))
            End If
        End If
    End Sub

    Private Function Bindgrid() As String
        Dim objCat As New ContCat
        Dim ds As New DataSet
        Dim strSearchVal(4) As String

        strSearchVal(0) = txtItemCode.Text
        strSearchVal(1) = txtItemDesc.Text
        strSearchVal(2) = HttpContext.Current.Session("CompanyIdToken")
        strSearchVal(3) = txtVendorItemCode.Text

        ds = objCat.getProductList(strSearchVal, viewstate("index"), viewstate("type"), "DC", viewstate("BuyerId"), Session("CompanyIdToken"))

        intPageRecordCnt = ds.Tables(0).Rows.Count

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
            cmdContract.Enabled = True
        Else
            Common.NetMsgbox(Me, MsgNoRecord)
            dtgCatalogue.DataBind()
            cmdContract.Enabled = False
        End If
        ' add for above checking
        viewstate("PageCount") = dtgCatalogue.PageCount
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgCatalogue.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub cmdContract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdContract.Click
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
        'Response.Redirect("HubContractPrice.aspx?index=" & viewstate("index") & "&mode=add&cattype=" & viewstate("cattype") & "&pageid=" & strPageId)
        Response.Redirect(dDispatcher.direct("Catalogue", "HubContractPrice.aspx", "index=" & ViewState("index") & "&mode=add&cattype=" & ViewState("cattype") & "&pageid=" & strPageId))
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgCatalogue.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgCatalogue_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemCreated
        Grid_ItemCreated(dtgCatalogue, e)

        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dtgCatalogue_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(EnumCat.icChk).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim lnkCode As HyperLink
            lnkCode = e.Item.FindControl("lnkCode")
            lnkCode.Text = dv("PM_Product_Code")
            ' ai chu modified on 13/10/2005
            ' item retrieved from Product_Mstr
            'lnkCode.Attributes.Add("onclick", "window.open('ProductDetail.aspx?companyType=V&pid=" & lnkCode.Text & "&index=&draft=0',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');")
            lnkCode.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("Catalogue", "ProductDetail.aspx", "companyType=V&pid=" & lnkCode.Text & "&index=&draft=0',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');"))
            lnkCode.NavigateUrl = "#"

            If Not IsDBNull(dv("PM_Unit_Cost")) Then
                e.Item.Cells(EnumCat.icPrice).Text = Format(CDbl(e.Item.Cells(EnumCat.icPrice).Text), "###,###,##0.0000")
            Else
                e.Item.Cells(EnumCat.icPrice).Text = "N/A"
            End If
        End If
    End Sub

    Private Sub dtgCatalogue_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCatalogue.PageIndexChanged
        dtgCatalogue.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub
End Class
