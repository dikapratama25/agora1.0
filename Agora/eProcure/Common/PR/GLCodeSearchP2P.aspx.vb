Imports AgoraLegacy
Imports eProcure.Component

Public Class GLCodeSearchP2P
    Inherits AgoraLegacy.AppBaseClass
    Dim strGLCode As String = ""
    Dim strGLDesc As String = ""
    Dim strGLStatus As String = "A"

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
        SetGridProperty(dtgGLCode)

        If Not IsPostBack Then
            lblTitle.Text = "Select GL Code"
            hidID.Value = Request.QueryString("id")
            hidGLCode.Value = Request.QueryString("hidGLCode")
            hidMode.Value = Request.QueryString("mode")
            Bindgrid()
        End If
    End Sub

    Private Function Bindgrid() As String
        Dim objGLCode As New IPP

        Dim dt As DataTable
        Dim ds As New DataSet
        strGLCode = txtCode.Text
        strGLDesc = txtDesc.Text

        ds = objGLCode.GetGLCode(strGLCode, strGLDesc, strGLStatus, True)
        dt = ds.Tables(0)
        intPageRecordCnt = dt.Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        Dim dvGLCode As DataView
        dvGLCode = dt.DefaultView

        dvGLCode.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvGLCode.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgGLCode, dvGLCode)
            dtgGLCode.DataSource = dvGLCode
            dtgGLCode.DataBind()
            cmdSelect.Disabled = False
        Else
            dtgGLCode.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtgGLCode.PageCount

        objGLCode = Nothing
        ds = Nothing
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgGLCode.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub cmd_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_search.Click
        Bindgrid()
    End Sub

    Public Sub SortCommandValues_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgGLCode.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgGLCode_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGLCode.ItemCreated
        Grid_ItemCreated(dtgGLCode, e)
        intPageRecordCnt = ViewState("intPageRecordCnt")
    End Sub

    Private Sub dtgGLCode_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGLCode.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lblIndex As Label
            lblIndex = e.Item.FindControl("lblIndex")
            lblIndex.Text = dv("CBG_B_GL_CODE")

            Dim lbl As Label = e.Item.FindControl("lblSelection")
            lbl.Text = "<input type=radio name='Myid' value=""" & Common.parseNull(dv("CBG_B_GL_CODE")) & """ onclick='Chk(""" & Common.parseNull(dv("CBG_B_GL_CODE")) & """,""" & Common.parseNull(dv("CBG_B_GL_DESC")) & """)';>"
        End If
    End Sub

    Private Sub dtgGLCode_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgGLCode.PageIndexChanged
        dtgGLCode.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub
End Class
