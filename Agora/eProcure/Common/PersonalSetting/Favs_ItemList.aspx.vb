Imports AgoraLegacy
Imports eProcure.Component
Public Class Favs_ItemList
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents MyDataGrid As System.Web.UI.WebControls.DataGrid
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden

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
        SetGridProperty(MyDataGrid)

        'MyDataGrid.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Public Sub MyDataGrid_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        MyDataGrid.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0, True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(MyDataGrid.CurrentPageIndex = 0, True)
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objRFQ As New BuyerCat
        '   Dim objRFQ As String
        Dim ds As DataSet = New DataSet
        Dim record As String
        'Dim strlistname As String
        Dim strlistindex As String
        Dim dgItem As DataGridItem
        ' Dim i As Integer = 0

        'For Each dgItem In MyDataGrid.Items
        'hidIndex.Value = MyDataGrid.DataKeys.Item(dgItem.ItemIndex)
        hidIndex.Value = Request.QueryString("listindex")
        strlistindex = hidIndex.Value

        ds = objRFQ.getFavCat_CDP(strlistindex)

        ' Next

        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        ''dvViewSample(0)(0)
        ''If pSorted Then
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        intPageRecordCnt = ds.Tables(0).Rows.Count

        If intPageRecordCnt > 0 Then

            MyDataGrid.DataSource = dvViewSample
            MyDataGrid.DataBind()
        Else

            MyDataGrid.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)

        End If
        objRFQ = Nothing

    End Function

    Private Sub MyDataGrid_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles MyDataGrid.ItemCreated
        'intPageRecordCnt = viewstate("intPageRecordCnt")
    End Sub

    Private Sub MyDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles MyDataGrid.ItemDataBound

    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Response.Redirect("Favs_ListMain.aspx")
        ' Response.Close()




    End Sub
End Class
