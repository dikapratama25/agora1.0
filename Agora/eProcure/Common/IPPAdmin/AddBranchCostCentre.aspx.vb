Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions
Imports System.Drawing

Public Class AddBranchCostCentre
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objGlobal As New AppGlobals
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblAction As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents dtgCC As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtBranchCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCostCentre As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Public Enum EnumCC
        icChk
        icCostCentre
        icDesc
        icIndex
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCC)

        If Not Page.IsPostBack Then
            ViewState("coyid") = Request.QueryString("coyid")
            ViewState("mode") = Request.QueryString("mode")
            hidMode.Value = ViewState("mode")

            cmdSearch.Enabled = True
            cmdSave.Enabled = False
            cmdDelete.Enabled = False

            If ViewState("mode") = "add" Then
                lblTitle.Text = "Assign Cost Centre"
                lblAction.Text = "Enter a valid branch code and click on the Search button to list down the cost centres that are available for tagging."
            Else
                lblTitle.Text = "Modify Cost Centre"
                lblAction.Text = "Select the Cost Centres and click on Remove button to remove it from the Branch Code."
            End If

            If ViewState("mode") = "edit" Then
                txtBranchCode.Text = Trim(Request.QueryString("branch"))
                txtBranchCode.Enabled = False
                cmdSave.Style("display") = "none"
                cmdDelete.Style("display") = ""
                Bindgrid()
            Else
                cmdSave.Style("display") = ""
                cmdDelete.Style("display") = "none"
            End If

        End If

        cmdSave.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")

    End Sub

    Sub dtgCC_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgCC.PageIndexChanged
        dtgCC.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim ds As DataSet
        Dim objIPPMain As New IPPMain

        If ViewState("mode") = "add" Then
            ds = objIPPMain.GetBranchCostCentre("A", ViewState("coyid"), txtBranchCode.Text, txtCostCentre.Text, txtDesc.Text)
        Else
            ds = objIPPMain.GetBranchCostCentre("M", ViewState("coyid"), txtBranchCode.Text, txtCostCentre.Text, txtDesc.Text)
        End If

        '//for sorting asc or desc
        Dim dvViewCC As DataView
        dvViewCC = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewCC.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewCC.Sort += " DESC"
        End If


        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt
        '//bind datagrid
        If intPageRecordCnt > 0 Then
            txtBranchCode.Enabled = False
            cmdSave.Enabled = True
            cmdDelete.Enabled = True
            resetDatagridPageIndex(dtgCC, dvViewCC)
            dtgCC.DataSource = dvViewCC
            dtgCC.DataBind()
            dtgCC.Visible = True
        Else
            cmdSave.Enabled = False
            cmdDelete.Enabled = False
            dtgCC.Visible = False
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00006"))
        End If

        ViewState("PageCount") = dtgCC.PageCount
    End Function

    Private Sub dtgCC_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCC.ItemCreated

        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgCC, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgCC.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgCC_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCC.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(EnumCC.icChk).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            If ViewState("mode") = "add" Then
                e.Item.Cells(EnumCC.icIndex).Text = ""
            Else
                e.Item.Cells(EnumCC.icIndex).Text = dv("BCC_BR_CC_INDEX")
            End If

        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click

        If ViewState("mode") = "add" Then
            If txtBranchCode.Text = "" Then
                Common.NetMsgbox(Me, "Branch Code " & objGlobal.GetErrorMessage("00001"), MsgBoxStyle.Information)
            Else
                If objDb.Exist("SELECT '*' FROM COMPANY_BRANCH_MSTR WHERE CBM_BRANCH_CODE = '" & Common.Parse(txtBranchCode.Text) & "' AND CBM_STATUS = 'A' and CBM_COY_ID = '" & ViewState("coyid") & "'") = 0 Then
                    Common.NetMsgbox(Me, "Invalid Branch Code.", MsgBoxStyle.Information)
                Else
                    dtgCC.CurrentPageIndex = 0
                    Bindgrid()
                End If
            End If
        Else
            dtgCC.CurrentPageIndex = 0
            Bindgrid()
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim dgItem As DataGridItem
        Dim strMsg As String
        Dim dtNewCC As New DataTable
        Dim drCC As DataRow
        Dim dsCC As New DataSet
        dtNewCC.Columns.Add("BCC_COY_ID", Type.GetType("System.String")) '//product code
        dtNewCC.Columns.Add("BCC_BRANCH_CODE", Type.GetType("System.String")) '//product code
        dtNewCC.Columns.Add("BCC_CC_CODE", Type.GetType("System.String")) '//product code

        For Each dgItem In dtgCC.Items
            Dim chkSelection As CheckBox = dgItem.Cells(EnumCC.icChk).FindControl("chkSelection")

            If chkSelection.Checked Then
                drCC = dtNewCC.NewRow
                drCC("BCC_COY_ID") = ViewState("coyid")
                drCC("BCC_BRANCH_CODE") = txtBranchCode.Text
                drCC("BCC_CC_CODE") = dgItem.Cells(EnumCC.icCostCentre).Text
                dtNewCC.Rows.Add(drCC)
            End If
        Next

        dsCC.Tables.Add(dtNewCC)

        Dim objIPPMain As New IPPMain
        strMsg = objIPPMain.InsertBranchCostCentre(dsCC)
        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        dtgCC.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim strMsg As String
        Dim dtCC As New DataTable
        Dim drCC As DataRow
        Dim dsCC As New DataSet
        dtCC.Columns.Add("BCC_BR_CC_INDEX", Type.GetType("System.String")) '//product code

        For Each dgItem In dtgCC.Items
            Dim chkSelection As CheckBox = dgItem.Cells(EnumCC.icChk).FindControl("chkSelection")

            If chkSelection.Checked Then
                drCC = dtCC.NewRow
                drCC("BCC_BR_CC_INDEX") = dgItem.Cells(EnumCC.icIndex).Text
                dtCC.Rows.Add(drCC)
            End If
        Next

        dsCC.Tables.Add(dtCC)

        Dim objIPPMain As New IPPMain
        strMsg = objIPPMain.DeleteBranchCostCentre(dsCC)
        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        dtgCC.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        If ViewState("mode") = "add" Then
            txtBranchCode.Text = ""
            txtBranchCode.Enabled = True
            cmdSave.Enabled = False
            dtgCC.DataSource = Nothing
            dtgCC.DataBind()
            dtgCC.Visible = False
        End If

        txtCostCentre.Text = ""
        txtDesc.Text = ""
    End Sub
End Class
