'*************************************************************************************
'Created By:  Louise
'Date:  13/05/2005
'Screen:  Create Registration Authority Code
'Purpose:  To create the registration authority code - Hub Admin 

'**************************************************************************************
Imports AgoraLegacy

Public Class LocMapping
    Inherits AgoraLegacy.AppBaseClass
    Dim LocIndex As Integer


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtRegAbbr As System.Web.UI.WebControls.TextBox
    Protected WithEvents dtgLocMapping As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdCancel As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.WebControls.Button
    Protected WithEvents cmdUpdate As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button

    Protected WithEvents lblRegAutho As System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents rfv_txtAddrCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_txtAcctCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_txtLocCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldsumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents SS As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents UU As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClearSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents RAI As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents smr As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents tt As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents dtg As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents cmdCancelSave As System.Web.UI.WebControls.Button
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents txtAcctCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents txtSAddrCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAddrCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLocCode As System.Web.UI.WebControls.TextBox

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
        SetGridProperty(dtgLocMapping)

        If Not Page.IsPostBack Then
            UU.Visible = False
            SS.Visible = True

            cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
            cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
            RAI.Visible = False
            Table2.Visible = False
            SS.Visible = False
            UU.Visible = False
            dtg.Visible = False
            smr.Visible = False
            tt.Visible = False
            cmdSave.Visible = False
            cmdClear.Visible = False
            txtAddrCode.Enabled = True
            txtAcctCode.Enabled = True
            txtLocCode.Enabled = True
        End If

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        ' Dim ds As DataSet = New DataSet
        Dim record As Integer

        Dim objHubAdmin As New  HubAdmin
        Dim ds As DataSet = New DataSet
        ds = objHubAdmin.getLocMapping(txtSAddrCode.Text)

        ' ds = objinv.getRegAuthority(txtSAddrCode.Text)
        record = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If viewstate("action") = "del" Then
            If dtgLocMapping.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgLocMapping.PageSize = 0 Then
                dtgLocMapping.CurrentPageIndex = dtgLocMapping.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count

        If viewstate("intPageRecordCnt") > 0 Then

            ' check when user re-enter search criteria and click on other page without click search button
            If dtgLocMapping.CurrentPageIndex > (dvViewSample.Count \ dtgLocMapping.PageSize) Then
                dtgLocMapping.CurrentPageIndex = IIf((dvViewSample.Count \ dtgLocMapping.PageSize) = 1, 0, (dvViewSample.Count \ dtgLocMapping.PageSize))
            ElseIf dtgLocMapping.CurrentPageIndex = (dvViewSample.Count \ dtgLocMapping.PageSize) Then
                If viewstate("PageCount") = (dvViewSample.Count \ dtgLocMapping.PageSize) Then
                    'user does not re-enter search criteria 
                    dtgLocMapping.CurrentPageIndex = IIf((dvViewSample.Count \ dtgLocMapping.PageSize) = 0, 0, (dvViewSample.Count \ dtgLocMapping.PageSize) - 1)
                Else
                    If (dvViewSample.Count Mod dtgLocMapping.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dtgLocMapping.CurrentPageIndex = IIf((dvViewSample.Count \ dtgLocMapping.PageSize) = 1, 0, (dvViewSample.Count \ dtgLocMapping.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        dtgLocMapping.CurrentPageIndex = (dvViewSample.Count \ dtgLocMapping.PageSize)
                    End If
                End If
            End If
            '--------------------------------

            dtgLocMapping.DataSource = dvViewSample
            dtgLocMapping.DataBind()

            dtgLocMapping.Visible = True
            dtg.Visible = True
            tt.Visible = True
            lblRegAutho.Visible = True
            cmdAdd.Visible = True
            cmdModify.Visible = True
            cmdDelete.Visible = True
            cmdModify.Enabled = True
            cmdDelete.Enabled = True
        Else
            dtgLocMapping.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            dtgLocMapping.Visible = False
            dtg.Visible = False
            tt.Visible = False
            lblRegAutho.Visible = False
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
        End If

        ' add for above checking
        viewstate("PageCount") = dtgLocMapping.PageCount
        objHubAdmin = Nothing
    End Function

    Public Sub OnPageIndexChanged_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(True)
    End Sub

    Private Sub dtgLocMapping_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgLocMapping.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
        End If
    End Sub

    Private Sub dtgLocMapping_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgLocMapping.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgLocMapping, e)
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim objHubAdmin As New  HubAdmin
        If objHubAdmin.chkLocDuplicate(txtAddrCode.Text, txtAcctCode.Text, txtLocCode.Text) = 1 Then
            Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Information)
        Else
            objHubAdmin.saveLocMapping("A", txtAddrCode.Text, txtAcctCode.Text, txtLocCode.Text)
            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)

            txtAddrCode.Text = ""
            txtAcctCode.Text = ""
            txtLocCode.Text = ""
            objHubAdmin = Nothing
            Bindgrid()
        End If
    End Sub

    Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        txtAddrCode.Text = ""
        txtAcctCode.Text = ""
        txtLocCode.Text = ""
    End Sub

    Private Sub cmdModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdModify.Click

        Dim chk As CheckBox
        Dim dtgitem As DataGridItem

        For Each dtgitem In Me.dtgLocMapping.Items
            chk = dtgitem.FindControl("chkSelection")

            If chk.Checked Then
                txtAddrCode.Text = dtgitem.Cells(1).Text
                txtAcctCode.Text = dtgitem.Cells(2).Text
                txtLocCode.Text = dtgitem.Cells(3).Text
                LocIndex = dtgitem.Cells(4).Text
                Session("LocIndex") = LocIndex
                Table2.Visible = True
                cmdSave.Visible = False
                cmdClear.Visible = False
                cmdModify.Enabled = True
                cmdDelete.Enabled = True
                cmdUpdate.Visible = True
                cmdReset.Visible = True
                cmdCancel.Visible = True
            End If
        Next
        SS.Visible = False
        UU.Visible = True
    End Sub

    Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

        Dim dt As DataTable
        Dim objHubAdmin As New  HubAdmin
        If objHubAdmin.chkLocDuplicate(txtAddrCode.Text, txtAcctCode.Text, txtLocCode.Text) = 1 Then
            Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Information)
        Else
            objHubAdmin.saveLocMapping("M", txtAddrCode.Text, txtAcctCode.Text, txtLocCode.Text, Session("LocIndex"))
            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)

            RAI.Visible = False
            Table2.Visible = False
            SS.Visible = False
            UU.Visible = False
            cmdModify.Enabled = True
            cmdDelete.Enabled = True

            Bindgrid()
            objHubAdmin = Nothing
        End If
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Dim dt As DataTable

        txtAddrCode.Text = dt.Rows(0)(1)
        txtAcctCode.Text = dt.Rows(0)(2)
        txtLocCode.Text = dt.Rows(0)(3)
        UU.Visible = True
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        RAI.Visible = False
        Table2.Visible = False
        SS.Visible = False
        UU.Visible = False
        cmdModify.Enabled = True
        cmdDelete.Enabled = True
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

        Dim objHubAdmin As New  HubAdmin
        Dim DeleteOK As Boolean = False
        Dim chk As CheckBox
        Dim dtgitem As DataGridItem

        For Each dtgitem In dtgLocMapping.Items
            chk = dtgitem.FindControl("chkSelection")
            LocIndex = dtgitem.Cells(4).Text
            Session("LocIndex") = LocIndex
            If chk.Checked Then
                objHubAdmin.saveLocMapping("D", " ", " ", " ", Session("LocIndex"))
                DeleteOK = True
            End If
        Next

        If DeleteOK Then
            Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        End If

        If dtgLocMapping.Items.Count = 0 Then
            lblRegAutho.Visible = False
            cmdDelete.Enabled = False
            cmdModify.Enabled = False
        End If

        Bindgrid()
        RAI.Visible = False
        Table2.Visible = False
        SS.Visible = False
        UU.Visible = False
        objHubAdmin = Nothing
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Bindgrid()
        cmdAdd.Visible = True
    End Sub

    Private Sub cmdClearSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClearSearch.Click
        txtSAddrCode.Text = ""
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        RAI.Visible = True
        Table2.Visible = True
        SS.Visible = True
        cmdSave.Visible = True
        smr.Visible = True
        cmdClear.Visible = True
        cmdCancelSave.Visible = True
        UU.Visible = False
        txtAddrCode.Text = ""
        txtAcctCode.Text = ""
        txtAddrCode.Enabled = True
        txtLocCode.Text = ""
        txtLocCode.Enabled = True
        cmdAdd.Visible = False
        cmdModify.Visible = False
        cmdDelete.Visible = False
    End Sub

    Private Sub cmdCancelSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancelSave.Click
        RAI.Visible = False
        Table2.Visible = False
        SS.Visible = False
        UU.Visible = False
        cmdModify.Enabled = True
        cmdDelete.Enabled = True
    End Sub

End Class
