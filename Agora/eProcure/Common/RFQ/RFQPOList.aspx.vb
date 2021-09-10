Imports AgoraLegacy
Imports eProcure.Component
Public Class RFQPOList
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dtg_polist As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lbl_rfq As System.Web.UI.WebControls.Label
    Protected WithEvents cmdClose As System.Web.UI.WebControls.Button
    'Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    'Protected WithEvents lblTitle As System.Web.UI.WebControls.Label

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
        CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(Me.dtg_polist)

        Bindgrid()
    End Sub

    Sub dtg_polist_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtg_polist.PageIndexChanged
        dtg_polist.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtg_polist.SortCommand
        Grid_SortCommand(sender, e)
        Bindgrid(dtg_polist.CurrentPageIndex, True)
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objrfq As New RFQ
        Dim RFQ_No As String = Me.Request(Trim("RFQ_Num"))
        Dim RFQ_ID As String = Me.Request(Trim("RFQ_ID"))
        Me.lbl_rfq.Text = "RFQ No : " & Me.Request(Trim("RFQ_Num"))
        Dim ds As DataSet

        ds = objrfq.rfq_polist(RFQ_No, RFQ_ID)
        intPageRecordCnt = ds.Tables(0).Rows.Count
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        If pSorted Then
            dvViewSample.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        End If

        dtg_polist.DataSource = dvViewSample
        dtg_polist.DataBind()
    End Function

    Private Sub dtg_polist_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_polist.ItemCreated
        Grid_ItemCreated(dtg_polist, e)
    End Sub

    Private Sub dtg_vendor_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_polist.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Common.parseNull(dv("POM_PO_DATE")))
            If Common.parseNull(dv("POM_PO_STATUS")) = "Rejected By" Or Common.parseNull(dv("POM_PO_STATUS")) = "Rejected" Then
                e.Item.Cells(4).Text = "Rejected by management / vendor"
            ElseIf Common.parseNull(dv("POM_PO_STATUS")) = "Cancelled By" Or Common.parseNull(dv("POM_PO_STATUS")) = "Cancelled" Then
                e.Item.Cells(4).Text = "Cancelled by management / vendor"
            End If

            Dim i, COUNT As Integer
            Dim objrfq As New RFQ
            Dim ARRAY(100) As String

            objrfq.GetPR(Me.Request(Trim("RFQ_Num")), ARRAY, COUNT)   'Get PR Number
            If ARRAY(0) <> "" Then
                For i = 0 To COUNT - 1
                    'lnkPONo.Text = ARRAY(i)
                    'lnkPONo.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "side=b&caller=PRAPPALL&status=" & dv("POM_PO_STATUS") & "&BCoyID=" & Session("CompanyID") & "&pageid=" & strPageId & "&index=" & dv("POM_PO_Index") & "&PO_No=" & ARRAY(i))
                    'STR = STR & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("PO", "PODetail.aspx", "side=b&caller=PRAPPALL&status=" & dv("POM_PO_STATUS") & "&BCoyID=" & Session("CompanyID") & "&pageid=" & strPageId & "&index=" & dv("POM_PO_Index") & "&PO_No=" & ARRAY(i)) & "')"" ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
                    e.Item.Cells(5).Text = ARRAY(i)

                Next
            End If

        End If

    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language='javascript'> { window.close();}</script>")
    End Sub
End Class
