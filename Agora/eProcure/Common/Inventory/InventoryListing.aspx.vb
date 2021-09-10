Imports AgoraLegacy
Imports eProcure.Component
Public Class InventoryListing
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim intLocIndicator As Integer
    Dim strLoc As String
    Dim strSLoc As String
    Protected WithEvents optSort As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents optSort1 As System.Web.UI.WebControls.RadioButton
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

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objInv As New Inventory

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgInv)
        If Not Page.IsPostBack Then
            LoadLoc()
            Me.ddl_SubLoc.Items.Insert(0, "---Select---")
            ChangeHeaderText()
        End If

    End Sub

    Private Sub LoadLoc()
        Dim objInv As New Inventory
        Dim ds As New DataSet

        ds = objInv.PopLocation

        Me.ddl_Loc.Items.Clear()
        Me.ddl_Loc.DataSource = ds.Tables(0)
        Me.ddl_Loc.DataTextField = "LM_LOCATION"
        Me.ddl_Loc.DataBind()
        Me.ddl_Loc.Items.Insert(0, "---Select---")

        objInv = Nothing

    End Sub

    Private Sub ChangeHeaderText()
        Dim objInv As New Inventory

        objInv.GetLocationInfo(strLoc, strSLoc, intLocIndicator)

        dtgInv.Columns(2).HeaderText = strLoc
        dtgInv.Columns(3).HeaderText = strSLoc
        Me.lblLoc.Text = strLoc
        Me.lblSubLoc.Text = strSLoc

        If intLocIndicator > 1 Then 'Sub Location is defined
            Me.dtgInv.Columns(3).Visible = True
            lblSubLoc.Visible = True
            Me.ddl_SubLoc.Visible = True
        Else
            Me.dtgInv.Columns(3).Visible = False
            lblSubLoc.Visible = False
            Me.ddl_SubLoc.Visible = False
        End If
        objInv = Nothing

    End Sub

    Private Sub ddl_Loc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Loc.SelectedIndexChanged
        Dim objInv As New Inventory
        Dim ds As New DataSet

        ds = objInv.PopLocation(Me.ddl_Loc.SelectedItem.Text)

        Me.ddl_SubLoc.Items.Clear()
        Me.ddl_SubLoc.DataSource = ds.Tables(0)
        Me.ddl_SubLoc.DataTextField = "LM_SUB_LOCATION"
        Me.ddl_SubLoc.DataValueField = "LM_LOCATION_INDEX"
        Me.ddl_SubLoc.DataBind()
        Me.ddl_SubLoc.Items.Insert(0, "---Select---")

        objInv = Nothing
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = Request("optSort") '"IM_ITEM_CODE"

        dtgInv.CurrentPageIndex = 0
        BindInv(True)
        ChangeHeaderText()
        Me.optSort.Items.FindByValue(ViewState("SortExpression")).Selected = True
        Me.cmdPrint.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewStockCount.aspx", "ItemCode=" & Me.txtItemCode.Text & "&ItemName=" & Me.txtItemName.Text & "&Loc=" & Me.ddl_Loc.SelectedItem.Text & "&SubLoc=" & Me.ddl_SubLoc.SelectedItem.Text & "&SortedBy=" & Request("optSort") & "&CoyID=" & Session("CompanyId")) & "&QC=" & Request("rd2") & "')")
        'Me.cmdPrint.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewITSlip.aspx", "ITNo=IT000006") & "')")

    End Sub

    Private Sub BindInv(Optional ByVal pSorted As Boolean = False)
        Dim objInv As New Inventory
        Dim ds As DataSet
        Dim dvViewInv As DataView

        ds = objInv.GetInvDetails(Me.txtItemCode.Text, Me.txtItemName.Text, Me.ddl_Loc.SelectedItem.Text, Me.ddl_SubLoc.SelectedItem.Text, Request("optSort"), Request("rd2"))
        dvViewInv = ds.Tables(0).DefaultView
        dvViewInv.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewInv.Sort += " DESC"

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgInv, dvViewInv)
            dtgInv.DataSource = dvViewInv
            dtgInv.DataBind()
        Else
            dtgInv.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ViewState("PageCount") = dtgInv.PageCount
        objInv = Nothing

    End Sub

    Private Sub dtgInv_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInv.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgInv, e)
        ChangeHeaderText()
    End Sub

    Private Sub dtgInv_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInv.ItemDataBound
        Dim objInv As New Inventory

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkItemCode As LinkButton

            lnkItemCode = e.Item.Cells(0).FindControl("lnkItemCode")
            lnkItemCode.Text = dv("IM_ITEM_CODE")
            lnkItemCode.CommandArgument = dDispatcher.direct("Inventory", "InvTransHistory.aspx", "ItemCode=" & Server.UrlEncode(dv("IM_ITEM_CODE")) & "&ItemName=" & Server.UrlEncode(dv("IM_INVENTORY_NAME")) & "&Loc=" & Server.UrlEncode(dv("LM_LOCATION")) & "&SubLoc=" & Server.UrlEncode(Common.parseNull(dv("LM_SUB_LOCATION"))) & "&Qty=" & Server.UrlEncode(dv("ID_INVENTORY_QTY")) & "&InvIndex=" & Server.UrlEncode(dv("ID_INVENTORY_INDEX")) & "&LocIndex=" & Server.UrlEncode(dv("ID_LOCATION_INDEX")) & "&NeedQC=" & dv("IM_IQC_IND") & "&pageid=" & strPageId)
        End If

        objInv = Nothing
    End Sub

    Sub LinkButton_Click(ByVal sender As Object, ByVal e As CommandEventArgs)
        'Dim lnkItemCode As LinkButton
        'lnkItemCode = Me.dtgInv.SelectedItem.Cells(Items.cel.FindControl("lnkItemCode")

        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        strFileName = e.CommandArgument 'dDispatcher.direct("Inventory", "InvTransHistory.aspx", "ItemCode=" & Server.UrlEncode(dtgInv.SelectedItem.Cells("IM_ITEM_CODE").Text) & "&ItemName=" & Server.UrlEncode(dtgInv.SelectedItem.Cells("IM_INVENTORY_NAME").Text) & "&Loc=" & Server.UrlEncode(dtgInv.SelectedItem.Cells("LM_LOCATION").Text) & "&SubLoc=" & Server.UrlEncode(Common.parseNull(dtgInv.SelectedItem.Cells("LM_SUB_LOCATION").Text)) & "&Qty=" & Server.UrlEncode(dtgInv.SelectedItem.Cells("ID_INVENTORY_QTY").Text) & "&InvIndex=" & Server.UrlEncode(dtgInv.SelectedItem.Cells("ID_INVENTORY_INDEX").Text) & "&LocIndex=" & Server.UrlEncode(dtgInv.SelectedItem.Cells("ID_LOCATION_INDEX").Text) & "&pageid=" & strPageId)
        strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
        strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','400px');")
        'strscript.Append("document.getElementById('lnkItemCode').onclick();")
        strscript.Append("</script>")
        RegisterStartupScript("script13", strscript.ToString())
    End Sub

    Private Sub dtgInv_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgInv.PageIndexChanged
        dtgInv.CurrentPageIndex = e.NewPageIndex
        BindInv(True)
        ChangeHeaderText()
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgInv.SortCommand
        Grid_SortCommand(sender, e)
        dtgInv.CurrentPageIndex = 0
        BindInv(True)
        ChangeHeaderText()
    End Sub

    Private Sub InventoryListing_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.cmdPrint.Disabled = True
        If ViewState("intPageRecordCnt") > 0 Then
            Me.cmdPrint.Disabled = False
        End If
    End Sub

    Private Sub clearTextBox()
        Me.rd2.SelectedValue = "N"
        Me.optSort.SelectedValue = "IM_ITEM_CODE"
    End Sub

    Private Sub cmdClear_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.ServerClick
        clearTextBox()
    End Sub

    Private Sub rd2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rd2.SelectedIndexChanged
        Me.cmdPrint.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewStockCount.aspx", "ItemCode=" & Me.txtItemCode.Text & "&ItemName=" & Me.txtItemName.Text & "&Loc=" & Me.ddl_Loc.SelectedItem.Text & "&SubLoc=" & Me.ddl_SubLoc.SelectedItem.Text & "&SortedBy=" & Request("optSort") & "&CoyID=" & Session("CompanyId")) & "&QC=" & Request("rd2") & "')")

    End Sub
End Class