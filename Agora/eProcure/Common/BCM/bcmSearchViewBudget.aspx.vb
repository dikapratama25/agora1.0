Imports AgoraLegacy
Imports eProcure.Component
Public Class bcmSearchViewBudget
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtDeptName As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents lbl_result As System.Web.UI.WebControls.Label
    Protected WithEvents dgDept As System.Web.UI.WebControls.DataGrid

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
        MyBase.Page_Load(sender, e)
        SetGridProperty(dgDept)
    End Sub

    Sub dtgDept_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dgDept.PageIndexChanged
        dgDept.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dgDept.SortCommand
        Grid_SortCommand(sender, e)
        dgDept.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim strDeptCode As String
        Dim strDeptName As String

        'strDeptCode = Me.txtDeptCode.Text
        strDeptName = Me.txtDeptName.Text

        Dim objAdmin As New Admin

        '//Retrieve Data from Database
        Dim ds As DataSet = New DataSet
        ds = objAdmin.searchdept("", strDeptName, True)

        '//for sorting asc or desc
        Dim dvViewDept As DataView
        dvViewDept = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)
        'If pSorted Then
        dvViewDept.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewDept.Sort += " DESC"
        'End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If dgDept.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dgDept.PageSize = 0 Then
                dgDept.CurrentPageIndex = dgDept.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        If intPageRecordCnt > 0 Then
            'intTotPage = dtgDept.PageCount
            '//mean Enable, can't use button.Enabled because this is a HTML button
            dgDept.DataSource = dvViewDept
            dgDept.DataBind()
        Else
            'dtgDept.DataSource = ""
            dgDept.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            'intTotPage = 0
        End If
        'ShowStats()
    End Function

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dgDept.CurrentPageIndex = 0
        Bindgrid()

    End Sub

    Private Sub dgDept_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgDept.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            ''//to add JavaScript to Check Box
            'Dim chk As CheckBox
            'chk = e.Item.Cells(0).FindControl("chkSelection")
            'chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            '//to dynamic build hyoerlink
            Dim lnkDeptName As HyperLink
            lnkDeptName = e.Item.FindControl("lnkDeptName")
            lnkDeptName.NavigateUrl = dDispatcher.direct("BCM", "bcmViewBudget.aspx", "pageid=" & strPageId & "&deptindex=" & dv("CDM_DEPT_INDEX") & "&deptname=" & dv("CDM_DEPT_NAME"))
            lnkDeptName.Text = dv("CDM_DEPT_NAME")

            'e.Item.Cells(1).Te = "<a href='addDept.aspx'>" & e.Item.Cells(1).Text & "</a>"
        End If
    End Sub

    Private Sub dtgDept_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgDept.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dgDept, e)
        '//to add a JavaScript to CheckAll button
        'If e.Item.ItemType = ListItemType.Header Then
        '    Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
        '    chkAll.Attributes.Add("onclick", "selectAll();")
        'End If
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

End Class
