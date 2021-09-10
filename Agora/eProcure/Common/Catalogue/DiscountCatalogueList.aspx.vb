Imports AgoraLegacy
Imports eProcure.Component

Public Class DiscountCatalogueList
    Inherits AgoraLegacy.AppBaseClass

    Public Enum EnumCat
        icChk = 0
        icCode = 1
        icDesc = 2
        icStartDate = 3
        icEndDate = 4
    End Enum
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBuyer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateFr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents dtgCatalogue As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents trDiscount As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdItem As System.Web.UI.WebControls.Button
    Protected WithEvents cmdCompany As System.Web.UI.WebControls.Button
    Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary

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
        'alButtonList.Add(cmdItem)
        'alButtonList.Add(cmdCompany)
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdDelete)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        cmdAdd.Enabled = blnCanAdd And viewstate("blnCmdAdd")
        cmdModify.Enabled = blnCanUpdate And viewstate("blnCmdModify")
        'cmdItem.Enabled = blnCanAdd And blnCanUpdate And blnCmdItem
        'cmdCompany.Enabled = blnCanAdd And blnCanUpdate And blnCmdCompany
        cmdDelete.Enabled = blnCanDelete And viewstate("blnCmdDelete")
        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCatalogue)

        If Not IsPostBack Then
            viewstate("blnCmdAdd") = True
            viewstate("blnCmdModify") = True
            viewstate("blnCmdCompany") = True
            viewstate("blnCmdItem") = True
            viewstate("blnCmdDelete") = True
            viewstate("cattype") = Request.QueryString("cattype")
            lblTitle.Text = "Discount Catalogue"
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            cmdCompany.Enabled = False
            cmdItem.Enabled = False
            viewstate("blnCmdModify") = False
            viewstate("blnCmdDelete") = False
            viewstate("blnCmdCompany") = False
            viewstate("blnCmdItem") = False
            cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
            cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
            cmdCompany.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
            cmdItem.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        End If
    End Sub

    Private Function Bindgrid() As String
        Dim objCat As New ContCat
        Dim ds As New DataSet
        ds = objCat.getCatalogueList(Session("CompanyId"), "C", "D", txtCode.Text, txtDesc.Text, txtBuyer.Text, txtDateFr.Text, txtDateTo.Text)
        intPageRecordCnt = ds.Tables(0).Rows.Count

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
            cmdModify.Enabled = True
            cmdDelete.Enabled = True
            cmdCompany.Enabled = True
            cmdItem.Enabled = True
            viewstate("blnCmdModify") = True
            viewstate("blnCmdDelete") = True
            viewstate("blnCmdCompany") = True
            viewstate("blnCmdItem") = True
            dtgCatalogue.DataSource = dvViewSample
            dtgCatalogue.DataBind()
        Else
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            cmdCompany.Enabled = False
            cmdItem.Enabled = False
            viewstate("blnCmdModify") = False
            viewstate("blnCmdDelete") = False
            viewstate("blnCmdCompany") = False
            viewstate("blnCmdItem") = False
            Common.NetMsgbox(Me, MsgNoRecord)
            dtgCatalogue.DataBind()
        End If
        ' add for above checking
        viewstate("PageCount") = dtgCatalogue.PageCount
        objCat = Nothing
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgCatalogue.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgCatalogue_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemCreated
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
            lnkCode.Text = dv("CDM_GROUP_CODE")

            Dim lblIndex As Label
            lblIndex = e.Item.FindControl("lblIndex")
            lblIndex.Text = dv("CDM_GROUP_INDEX")

            e.Item.Cells(EnumCat.icStartDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, e.Item.Cells(EnumCat.icStartDate).Text)
            e.Item.Cells(EnumCat.icEndDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, e.Item.Cells(EnumCat.icEndDate).Text)

            'lnkCode.NavigateUrl = "ContractCatalogue.aspx?mode=mod&index=" & lblIndex.Text & "&type=DC"
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

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        'Response.Redirect("AddCatalogue.aspx?type=DC&mode=add&pageid=" & strPageId)
        Response.Redirect(dDispatcher.direct("Catalogue", "AddCatalogue.aspx", "type=DC&mode=add&pageid=" & strPageId))
    End Sub

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox

        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                'Response.Redirect("AddCatalogue.aspx?type=DC&mode=mod&index=" & CType(dgItem.FindControl("lblIndex"), Label).Text & "&pageid=" & strPageId)
                Response.Redirect(dDispatcher.direct("Catalogue", "AddCatalogue.aspx", "type=DC&mode=mod&index=" & CType(dgItem.FindControl("lblIndex"), Label).Text & "&pageid=" & strPageId))
            End If
        Next
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim objCat As New ContCat
        Dim chkItem As CheckBox
        'Dim strMsg As String
        Dim dtMaster As New DataTable
        dtMaster.Columns.Add("index", Type.GetType("System.Int32"))
        dtMaster.Columns.Add("CoyId", Type.GetType("System.String"))

        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                Dim dtr As DataRow
                dtr = dtMaster.NewRow()
                dtr("index") = CType(dgItem.FindControl("lblIndex"), Label).Text
                dtr("CoyId") = Session("CompanyId")
                dtMaster.Rows.Add(dtr)
            End If
        Next
        If objCat.deleteContract(dtMaster, Session("CompanyId")) = WheelMsgNum.Delete Then
            Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        Else
            'strMsg = "Deletion is not allowed.""&vbCrLf&""It has outstanding PR(s)."
            'Common.NetMsgbox(Me, MsgRecordNotDelete, MsgBoxStyle.Information, "Wheel")
            Common.NetMsgbox(Me, RecordDeleteNotAllowed, MsgBoxStyle.Information)
        End If
        viewstate("action") = "del"
        Bindgrid()
        objCat = Nothing
    End Sub

    Private Sub cmdCompany_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCompany.Click
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox

        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                'Response.Redirect("CompanyAssign.aspx?type=DC&caller=list&index=" & CType(dgItem.FindControl("lblIndex"), Label).Text & "&pageid=" & strPageId)
                Response.Redirect(dDispatcher.direct("Catalogue", "CompanyAssign.aspx", "type=DC&caller=list&index=" & CType(dgItem.FindControl("lblIndex"), Label).Text & "&pageid=" & strPageId))
            End If
        Next
    End Sub

    Private Sub cmdItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdItem.Click
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox

        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                'Response.Redirect("ContractCatalogue.aspx?pageid=" & strPageId & "&mode=mod&type=DC&index=" & CType(dgItem.FindControl("lblIndex"), Label).Text)
                Response.Redirect(dDispatcher.direct("Catalogue", "ContractCatalogue.aspx?pageid=" & strPageId & "&mode=mod&type=DC&index=" & CType(dgItem.FindControl("lblIndex"), Label).Text))
            End If
        Next
    End Sub
End Class
