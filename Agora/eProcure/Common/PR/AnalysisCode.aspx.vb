Imports AgoraLegacy
Imports eProcure.Component

Public Class AnalysisCode
    Inherits AgoraLegacy.AppBaseClass
    Dim strAnalysisCode As String = ""
    Dim strAnalysisDesc As String = ""
    Dim strAnalysisStatus As String = "O"
    Dim strDeptCode As String = ""

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
        SetGridProperty(dtgAnalysisCode)

        strDeptCode = Request.QueryString("dept")
        If Not IsPostBack Then


            Select Case strDeptCode
                Case "L1"
                    lblTitle.Text = "Select Fund Type"
                    lblCode.Text = "Fund Type"
                    lblDesc.Text = "Fund Type"
                Case "L8"
                    lblTitle.Text = "Select Project Code"
                    lblCode.Text = "Project Code"
                    lblDesc.Text = "Project Code"
                Case "L9"
                    lblTitle.Text = "Select Person Code"
                    lblCode.Text = "Person Code"
                    lblDesc.Text = "Person Code"
            End Select
            hidID.Value = Request.QueryString("id")

            hidAnalysisCode.Value = Request.QueryString("hidAnalysisCode")
            hidMode.Value = Request.QueryString("mode")
            Bindgrid()
        End If
    End Sub

    Private Function Bindgrid() As String
        Dim objAnalysisCode As New IPP

        Dim dt As DataTable
        Dim ds As New DataSet

        strAnalysisCode = txtCode.Text
        strAnalysisDesc = txtDesc.Text

        ds = objAnalysisCode.GetAnalysisCode(strAnalysisCode, strAnalysisDesc, strDeptCode, strAnalysisStatus, "eProcure")
        dt = ds.Tables(0)
        intPageRecordCnt = dt.Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        Dim dvAnalysisCode As DataView
        dvAnalysisCode = dt.DefaultView

        dvAnalysisCode.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvAnalysisCode.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgAnalysisCode, dvAnalysisCode)
            dtgAnalysisCode.DataSource = dvAnalysisCode
            dtgAnalysisCode.DataBind()
            cmdSelect.Disabled = False
        Else
            dtgAnalysisCode.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtgAnalysisCode.PageCount

        objAnalysisCode = Nothing
        ds = Nothing
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgAnalysisCode.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub cmd_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_search.Click
        Bindgrid()
    End Sub

    Public Sub SortCommandValues_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgAnalysisCode.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgAnalysisCode_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAnalysisCode.ItemCreated
        Grid_ItemCreated(dtgAnalysisCode, e)
        intPageRecordCnt = ViewState("intPageRecordCnt")
    End Sub

    Private Sub dtgAnalysisCode_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAnalysisCode.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lblIndex As Label
            lblIndex = e.Item.FindControl("lblIndex")
            lblIndex.Text = dv("AC_ANALYSIS_CODE")

            Dim lbl As Label = e.Item.FindControl("lblSelection")
            lbl.Text = "<input type=radio name='Myid' value=""" & Common.parseNull(dv("AC_ANALYSIS_CODE")) & """ onclick='Chk(""" & Common.parseNull(dv("AC_ANALYSIS_CODE")) & """,""" & Common.parseNull(dv("AC_ANALYSIS_CODE_DESC")) & """,""" & Common.parseNull(dv("AC_ANALYSIS_CODE")) & """)';>"
        End If
    End Sub

    Private Sub dtgAnalysisCode_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgAnalysisCode.PageIndexChanged
        dtgAnalysisCode.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub
End Class
