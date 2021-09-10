Imports eProcure.Component
Imports AgoraLegacy

Public Class ContractItemList
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher    
    Public Enum EnumCat
        icItemCode = 0
        icItemName = 1
        icCurrency = 2
        icPrice = 3
        icGSTRate = 4
        icTax = 5
        icUOM = 6
        icRemark = 7
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dtgCatalogue As System.Web.UI.WebControls.DataGrid

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
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCatalogue)

        If Not IsPostBack Then
            Bindgrid()
        End If

    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objCat As New ContCat
        Dim ds As New DataSet
        ds = objCat.getContractItemList(Request.QueryString("index"))
        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            dtgCatalogue.DataSource = dvViewSample
            dtgCatalogue.DataBind()
        Else
            Common.NetMsgbox(Me, MsgNoRecord)
            dtgCatalogue.DataBind()
        End If
        ViewState("PageCount") = dtgCatalogue.PageCount
    End Function
    Private Sub dtgCatalogue_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgCatalogue, e)
    End Sub

    Private Sub dtgCatalogue_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            e.Item.Cells(EnumCat.icPrice).Text = Format(CDbl(e.Item.Cells(EnumCat.icPrice).Text), "###,###,##0.0000")
            'e.Item.Cells(EnumCat.icTax).Text = iif(e.Item.Cells(EnumCat.icTax).Text isnot nothing, Format(CDbl(e.Item.Cells(EnumCat.icTax).Text), "##0"),0)
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
End Class


