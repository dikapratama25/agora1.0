Imports eProcure.Component
Imports AgoraLegacy


Public Class ContractCatalogueItem
    Inherits AgoraLegacy.AppBaseClass

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Public Enum EnumCat
        icCode = 0
        icDesc = 1
        icCurr = 2
        icPrice = 3
        icTax = 4
        icUOM = 5
        icRemark = 5
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dtgItem As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblContractRef As System.Web.UI.WebControls.Label
    Protected WithEvents lblDescription As System.Web.UI.WebControls.Label
    Protected WithEvents lblStartDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblEndDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblBuyerCompany As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink


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
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgItem)
        populatelbl()
        Bindgrid()
        lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "ContractCatalogueList.aspx", "type=A&pageid=" & strPageId)
    End Sub
    Sub populatelbl()
        Dim objconcat As New ContCat
        Dim ds As New DataSet
        ds = objconcat.getContractDetail(Request.QueryString("index"))
        lblContractRef.Text = ds.Tables(0).Rows(0).Item("cdm_group_code")
        lblDescription.Text = ds.Tables(0).Rows(0).Item("cdm_group_desc")
        lblStartDate.Text = ds.Tables(0).Rows(0).Item("cdm_start_date")
        lblEndDate.Text = ds.Tables(0).Rows(0).Item("cdm_end_date")
        lblBuyerCompany.Text = ds.Tables(0).Rows(0).Item("cdc_b_coy_id")
    End Sub
    Private Function Bindgrid() As String
        Dim objCat As New ContCat
        Dim ds As New DataSet

        ds = objCat.getContractItemList(Request.QueryString("index"))
        intPageRecordCnt = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If viewstate("action") = "del" Then
            If dtgItem.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgItem.PageSize = 0 Then
                dtgItem.CurrentPageIndex = dtgItem.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        If intPageRecordCnt > 0 Then            
            resetDatagridPageIndex(dtgItem, dvViewSample)
            dtgItem.DataSource = dvViewSample
            dtgItem.DataBind()
        Else
            Common.NetMsgbox(Me, MsgNoRecord)
            dtgItem.DataBind()
        End If
        ' add for above checking
        ViewState("PageCount") = dtgItem.PageCount
        objCat = Nothing
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgItem.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgCatalogue_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemCreated
        Grid_ItemCreated(dtgItem, e)
    End Sub

    Private Sub dtgCatalogue_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkCode As HyperLink
            lnkCode = e.Item.Cells(EnumCat.icCode).FindControl("lnkCode")
            lnkCode.NavigateUrl = "javascript:;"
            lnkCode.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Product", "ProductDetail.aspx", "ContCatType=Vendor&companytype=V&tax=" & dv("cdi_gst") & "&gstrate=" & Common.parseNull(dv("CDI_GST_RATE")) & "&price=" & dv("cdi_unit_cost") & "&pid=" & dv("cdi_product_code") & "&pageid=" & strPageId & "&index=" & dv("cdm_group_index") & "") & "')")
            lnkCode.Text = dv("cdi_vendor_item_code")            
        End If
    End Sub
    Private Sub dtgCatalogue_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgItem.PageIndexChanged
        dtgItem.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub
End Class