Imports AgoraLegacy
Imports eProcure.Component
Public Class GRNACKDetail
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher = New dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dtgAckDtl As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
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
    Dim objGRN As New GRN
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        blnPaging = False
        blnSorting = False
        SetGridProperty(dtgAckDtl)
        MyBase.Page_Load(sender, e)
        If Not IsPostBack Then
            Dim GrnIdx, GrnPoLine As Integer
            GrnIdx = Me.Request.QueryString("GrnIdx")
            GrnPoLine = Me.Request.QueryString("GrnPoLine")
            BindGrid(GrnIdx, GrnPoLine)
        End If
        'lnkBack.NavigateUrl = "javascript:history.back();"
        'lnkBack.NavigateUrl = "GRNDetails.aspx?Frm=GRNACKDetail&GRNNo=" & Me.Request.QueryString("GRNNo") & "&BCoyID=" & Session("CompanyID") & "&pageid=" & strPageId
        lnkBack.NavigateUrl = dDispatcher.direct("GRN", "GRNDetails.aspx", "Frm=GRNACKDetail&GRNNo=" & Me.Request.QueryString("GRNNo") & "&BCoyID=" & Session("CompanyID") & "&pageid=" & strPageId)
        'lnkBack.NavigateUrl = "AddGRN.aspx?pageid=" & strPageId
    End Sub
    Private Sub dtgAckDtl_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAckDtl.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgAckDtl, e)

    End Sub

    Private Sub dtgAckDtl_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAckDtl.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dt As DataTable
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            e.Item.Cells(2).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("GL_ACTION_DT"))
        End If
    End Sub

    'Public Sub dtgDODtl_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgAckDtl.PageIndexChanged
    '    dtgAckDtl.CurrentPageIndex = e.NewPageIndex
    'End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgAckDtl.SortCommand
        Grid_SortCommand(sender, e)
        dtgAckDtl.CurrentPageIndex = 0
    End Sub
    Private Function BindGrid(ByVal GrnIdx As Integer, ByVal GrnPoLine As Integer)
        Dim dsAck As DataSet
        Dim intTotRecord As Integer

        dsAck = objGRN.GetAckDetails(GrnIdx, GrnPoLine)

        '//for sorting asc or desc
        Dim dvViewGrnAck As DataView
        dvViewGrnAck = dsAck.Tables(0).DefaultView

        'If pSorted Then
        dvViewGrnAck.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewGrnAck.Sort += " DESC"

        intTotRecord = dsAck.Tables(0).Rows.Count
        intPageRecordCnt = intTotRecord

        If intTotRecord > 0 Then
            'intTotPage = dtgDO.PageCount
            dtgAckDtl.DataSource = dvViewGrnAck
            dtgAckDtl.DataBind()
        Else
            Common.NetMsgbox(Me, MsgNoRecord)
        End If


    End Function
End Class
