Imports AgoraLegacy
Imports eProcure.Component

Public Class Budget
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub

    Protected WithEvents dtgCustomField As System.Web.UI.WebControls.DataGrid
    Protected WithEvents custom As System.Web.UI.HtmlControls.HtmlGenericControl
    
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
        SetGridProperty(dtgBudget)

        If Not IsPostBack Then
            lblTitle.Text = "Select Default Budget Account"
            hidID.Value = Request.QueryString("id")
            hidBudget.Value = Request.QueryString("hidBudgetCode")
            hidMode.Value = Request.QueryString("mode")
            Bindgrid()
        End If
    End Sub

    Private Function Bindgrid() As String
        Dim objBUdget As New BudgetControl
        Dim strValue As String

        Dim dt As DataTable
        Dim ds As New DataSet
		'ds = objBUdget.getBCMListByUser(Session("UserId"), txtCode.Text)

        ds = objBUdget.getBCMListByUserNew(Session("UserId"), txtCode.Text, txtDesc.Text)
        dt = ds.Tables(0)
        viewstate("intPageRecordCnt") = dt.Rows.Count
        Dim dvBudget As DataView
		dvBudget = dt.DefaultView

        dvBudget.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvBudget.Sort += " DESC"

        If viewstate("intPageRecordCnt") > 0 Then
            dtgBudget.DataSource = dvBudget
            dtgBudget.DataBind()
            cmdSelect.Disabled = False
        Else
            dtgBudget.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgBudget.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub cmd_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_search.Click
        Bindgrid()
    End Sub

    Public Sub SortCommandValues_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgBudget.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgBudget_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgBudget.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgBudget, e)
    End Sub

    Private Sub dtgBudget_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgBudget.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lblIndex As Label
            lblIndex = e.Item.FindControl("lblIndex")
            lblIndex.Text = dv("Acct_Index")

            Dim lbl As Label = e.Item.FindControl("lblSelection")
            lbl.Text = "<input type=radio name='Myid' value=""" & Common.parseNull(dv("Acct_Index")) & """ onclick='Chk(""" & Common.parseNull(dv("Acct_List")) & """,""" & Common.parseNull(dv("Acct_Index")) & """)';>"
        End If
    End Sub

    Private Sub dtgBudget_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgBudget.PageIndexChanged
        dtgBudget.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub
End Class
