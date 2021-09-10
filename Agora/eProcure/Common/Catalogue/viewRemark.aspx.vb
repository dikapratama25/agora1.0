Imports AgoraLegacy
Imports eProcure.Component

Public Class viewRemark
    Inherits AgoraLegacy.AppBaseClass
    Dim strIndexList As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblCode As System.Web.UI.WebControls.Label
    Protected WithEvents dtgCatalogue As System.Web.UI.WebControls.DataGrid
    Protected WithEvents trCodeRead As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblItem As System.Web.UI.WebControls.Label

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
            lblTitle.Text = "View Contract Group Item Remarks"
            viewstate("index") = Request.QueryString("index")
            viewstate("code") = Request.QueryString("code")
            viewstate("pid") = Request.QueryString("pid")
            lblItem.Text = viewstate("pid")
            lblCode.Text = viewstate("code")
            Bindgrid()
        End If
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objCat As New ContCat
        Dim ds As New DataSet
        ds = objCat.getContractItemRemark(viewstate("index"), viewstate("pid"))
        intPageRecordCnt = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        ''//datagrid.pageCount only got value after databind
        If intPageRecordCnt > 0 Then
            dtgCatalogue.DataSource = dvViewSample
            dtgCatalogue.DataBind()
        End If
        objCat = Nothing
    End Function

    Private Sub dtgCatalogue_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemCreated
        Grid_ItemCreated(dtgCatalogue, e)
    End Sub

    Private Sub dtgCatalogue_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            e.Item.Cells(0).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, e.Item.Cells(0).Text)

            Select Case e.Item.Cells(2).Text.Trim
                Case "B"
                    e.Item.Cells(2).Text = "BUYER"
                Case "V"
                    e.Item.Cells(2).Text = "VENDOR"
                Case "H"
                    e.Item.Cells(2).Text = "HUB ADMIN"
            End Select
        End If
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgCatalogue.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgCatalogue_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCatalogue.PageIndexChanged
        dtgCatalogue.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub
End Class
