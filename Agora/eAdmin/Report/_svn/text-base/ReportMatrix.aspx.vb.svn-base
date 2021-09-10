Imports AppCommon
Imports eProcure.Component
Public Class ReportMatrix
    Inherits AppCommon.AppBaseClass
    Dim blnAll As Boolean

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents dtgReport As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button

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
        blnPaging = False
        SetGridProperty(dtgReport)

        If Not IsPostBack Then
            lblHeader.Text = "Report Matrix"
            bindgrid()
        End If
    End Sub

    Private Sub bindgrid()
        Dim objReport As New Report
        Dim ds As New DataSet
        ds = objReport.getReportMatrix(Session("CompanyIdToken"))

        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count
        intPageRecordCnt = viewstate("intPageRecordCnt")

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        ''//datagrid.pageCount only got value after databind
        If intPageRecordCnt > 0 Then
            If dvViewSample.Item(0)("CNT") = intPageRecordCnt Then
                blnAll = True
            Else
                blnAll = False
            End If
            dtgReport.DataSource = dvViewSample
            dtgReport.DataBind()
        End If
    End Sub

    Private Sub dtgReport_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgReport.ItemCreated
        '//this line must be included
        Grid_ItemCreated(sender, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
            chkAll.Checked = blnAll
        End If
    End Sub

    Private Sub dtgReport_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgReport.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkAddrCode As HyperLink
            Dim lbl As Label = e.Item.FindControl("lblSelection")
            Dim chk As CheckBox = e.Item.FindControl("chkSelection")

            '//to add JavaScript to Check Box
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            If dv("CHK") = "1" Then
                chk.Checked = True
            Else
                chk.Checked = False
            End If
        End If
    End Sub

    Private Function bindReport() As DataTable
        Dim dtItem As New DataTable
        Dim chk As CheckBox

        dtItem.Columns.Add("Index", Type.GetType("System.Int32"))
        
        Dim dtr As DataRow
        Dim dgItem As DataGridItem
        For Each dgItem In dtgReport.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                dtr = dtItem.NewRow
                dtr("Index") = dgItem.Cells(2).Text
                dtItem.Rows.Add(dtr)
            End If
        Next

        bindReport = dtItem
    End Function

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objReport As New Report
        Dim intMsg As Integer
        Dim dt As New DataTable
        dt = bindReport()
        intMsg = objReport.updateReportMatrix(Session("CompanyIdToken"), dt)
        Select Case intMsg
            Case WheelMsgNum.Save
                Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
            Case WheelMsgNum.NotSave
                Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
        End Select
        bindgrid()
    End Sub
End Class
