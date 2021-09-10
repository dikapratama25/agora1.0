'*************************************************************************************
'Created By:  Louise
'Date:  13/05/2005
'Screen:  Create Registration Authority Code
'Purpose:  To create the registration authority code - Hub Admin 

'**************************************************************************************

Imports ERFP.Components
Imports AgoraLegacy

Public Class RegAuthoritySetup
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtRegAbbr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRegDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents dtgRegAutho As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdCancel As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.WebControls.Button
    Protected WithEvents cmdUpdate As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Dim objinv As New invClass

    Protected WithEvents lblRegAutho As System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents rfv_txtRegAbbr As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_txtRegDesc As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldsumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents SS As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents UU As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents txtSRegAuthoAbbr As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClearSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents BAdd As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents RAI As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents ASave As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents BSave As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents smr As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents tt As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents dtg As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents AAdd As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents cmdCancelSave As System.Web.UI.WebControls.Button
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label

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
        Dim d_set As New DataSet

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgRegAutho)

        If Not Page.IsPostBack Then
            UU.Visible = False
            SS.Visible = True

            cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
            cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
            RAI.Visible = False
            Table2.Visible = False
            ASave.Visible = False
            SS.Visible = False
            UU.Visible = False
            BAdd.Visible = False
            AAdd.Visible = False
            BSave.Visible = False
            dtg.Visible = False
            smr.Visible = False
            tt.Visible = False
            cmdSave.Visible = False
            cmdClear.Visible = False
            txtRegAbbr.Enabled = True
            'Bindgrid()
        End If

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim ds As DataSet = New DataSet
        Dim record As Integer

        ds = objinv.getRegAuthority(txtSRegAuthoAbbr.Text)
        record = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)
        'If pSorted Then
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        'End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If dtgRegAutho.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgRegAutho.PageSize = 0 Then
                dtgRegAutho.CurrentPageIndex = dtgRegAutho.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        If viewstate("intPageRecordCnt") > 0 Then

            ' check when user re-enter search criteria and click on other page without click search button
            If dtgRegAutho.CurrentPageIndex > (dvViewSample.Count \ dtgRegAutho.PageSize) Then
                dtgRegAutho.CurrentPageIndex = IIf((dvViewSample.Count \ dtgRegAutho.PageSize) = 1, 0, (dvViewSample.Count \ dtgRegAutho.PageSize))
            ElseIf dtgRegAutho.CurrentPageIndex = (dvViewSample.Count \ dtgRegAutho.PageSize) Then
                If viewstate("PageCount") = (dvViewSample.Count \ dtgRegAutho.PageSize) Then
                    'user does not re-enter search criteria 
                    dtgRegAutho.CurrentPageIndex = IIf((dvViewSample.Count \ dtgRegAutho.PageSize) = 0, 0, (dvViewSample.Count \ dtgRegAutho.PageSize) - 1)
                Else
                    If (dvViewSample.Count Mod dtgRegAutho.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dtgRegAutho.CurrentPageIndex = IIf((dvViewSample.Count \ dtgRegAutho.PageSize) = 1, 0, (dvViewSample.Count \ dtgRegAutho.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        dtgRegAutho.CurrentPageIndex = (dvViewSample.Count \ dtgRegAutho.PageSize)
                    End If
                End If
            End If
            '--------------------------------

            dtgRegAutho.DataSource = dvViewSample
            dtgRegAutho.DataBind()

            dtgRegAutho.Visible = True
            dtg.Visible = True
            tt.Visible = True
            AAdd.Visible = True
            BAdd.Visible = True
            lblRegAutho.Visible = True
            cmdModify.Enabled = True
            cmdDelete.Enabled = True
        Else
            dtgRegAutho.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            dtgRegAutho.Visible = False
            dtg.Visible = False
            tt.Visible = False
            AAdd.Visible = False
            BAdd.Visible = False
            lblRegAutho.Visible = False
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
        End If

        ' add for above checking
        viewstate("PageCount") = dtgRegAutho.PageCount
        objinv = Nothing

    End Function

    Public Sub OnPageIndexChanged_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(True)
    End Sub

    Private Sub dtgRegAutho_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgRegAutho.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
        End If
    End Sub

    Private Sub dtgRegAutho_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgRegAutho.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgRegAutho, e)
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim objDBRFP As New  EAD.DBCom(ConfigurationSettings.AppSettings("eRFPPath"))
        Dim saveRAAbbr As String
        Dim saveRADesc As String
        Dim strtemp As String
        Dim endsql As Boolean

        saveRAAbbr = txtRegAbbr.Text
        saveRADesc = txtRegDesc.Text

        strtemp = objinv.saveRegAuthoritySetup(saveRAAbbr, saveRADesc)
        If strtemp = "1" Then
            Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Information)
            endsql = True
        Else
            objDBRFP.Execute(strtemp)
            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
            txtRegAbbr.Text = ""
            txtRegDesc.Text = ""
            'visiblefalse()
            'cmdAdd.Visible = True
        End If

        Bindgrid()
        objinv = Nothing
        objDBRFP = Nothing

    End Sub

    Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        txtRegAbbr.Text = ""
        txtRegDesc.Text = ""
    End Sub

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click

        Dim strtemp As String
        Dim chk As CheckBox
        Dim dtgitem As DataGridItem
        Dim strAryQuery(0) As String
        Dim i As Integer
        Dim m_rAAbbr As String
        Dim m_rADesc As String
        Dim dt As DataTable
        Dim endsql As Boolean

        For Each dtgitem In Me.dtgRegAutho.Items
            chk = dtgitem.FindControl("chkSelection")

            If chk.Checked Then
                m_rAAbbr = dtgitem.Cells(1).Text
                m_rADesc = dtgitem.Cells(2).Text
                
                txtRegAbbr.Enabled = False
                txtRegAbbr.Text = m_rAAbbr
                txtRegDesc.Text = m_rADesc
                savetodt()
                cmdSave.Visible = False
                cmdClear.Visible = False
                cmdModify.Enabled = True
                cmdDelete.Enabled = True
                BAdd.Visible = True
                RAI.Visible = True
                Table2.Visible = True
                ASave.Visible = True
                BSave.Visible = True
                cmdUpdate.Visible = True
                smr.Visible = True
                cmdReset.Visible = True
                cmdCancel.Visible = True
            End If
        Next
        SS.Visible = False
        UU.Visible = True
        objinv = Nothing
    End Sub

    Function savetodt()
        viewstate("dt") = getDefaultValue()
    End Function

    Function get_DataFromDt() As DataTable
        Return CType(viewstate("dt"), DataTable)
    End Function

    Function getDefaultValue() As DataTable
        Dim test As String
        Dim dbt As New DataTable
        Dim dtr As DataRow
        Dim i As Integer

        '  Dim ds As New DataSet
        dbt.Columns.Add("REG_ABBR")
        dbt.Columns.Add("REG_DESC")

        dtr = dbt.NewRow()
        dtr("REG_ABBR") = txtRegAbbr.Text
        dtr("REG_DESC") = txtRegDesc.Text

        dbt.Rows.Add(dtr)

        Return dbt
    End Function

    Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
        Dim objDBRFP As New  EAD.DBCom(ConfigurationSettings.AppSettings("eRFPPath"))

        Dim u_RAAbbr As String
        Dim u_RADesc As String
        Dim strtemp As String
        Dim endsql As Boolean
        Dim dt As DataTable

        u_RAAbbr = txtRegAbbr.Text
        u_RADesc = txtRegDesc.Text

        dt = get_DataFromDt()

        strtemp = objinv.UpdateRegAuthoritySetup(dt, u_RAAbbr, u_RADesc)
        If strtemp = "1" Then
            Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Information)
            endsql = True
        Else
            objDBRFP.Execute(strtemp)
            Common.NetMsgbox(Me, "Record Updated.", MsgBoxStyle.Information)
            BAdd.Visible = False
            RAI.Visible = False
            Table2.Visible = False
            BSave.Visible = False
            ASave.Visible = False
            SS.Visible = False
            UU.Visible = False
            cmdModify.Enabled = True
            cmdDelete.Enabled = True
            'visiblefalse()
            'savetodt()
        End If

        Bindgrid()
        objinv = Nothing
        objDBRFP = Nothing

    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Dim dt As DataTable

        dt = Me.get_DataFromDt()
        txtRegAbbr.Text = dt.Rows(0)(0)
        txtRegAbbr.Enabled = False
        txtRegDesc.Text = dt.Rows(0)(1)
        UU.Visible = True
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        'visiblefalse()
        BAdd.Visible = False
        RAI.Visible = False
        Table2.Visible = False
        ASave.Visible = False
        SS.Visible = False
        UU.Visible = False
        cmdModify.Enabled = True
        cmdDelete.Enabled = True
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

        Dim strtemp As String
        Dim chk As CheckBox
        Dim dtgitem As DataGridItem
        Dim strAryQuery(0) As String
        Dim i As Integer
        Dim d_rAAbbr As String
        Dim d_rADesc As String
        Dim endsql As Boolean
        Dim j As Integer = 0

        For Each dtgitem In dtgRegAutho.Items
            chk = dtgitem.FindControl("chkSelection")
            d_rAAbbr = dtgitem.Cells(1).Text
            d_rADesc = dtgitem.Cells(2).Text
            If chk.Checked Then
                strtemp = objinv.getRegAuthoSetupDependency(d_rAAbbr)
                If strtemp = "1" Then
                    Common.NetMsgbox(Me, "Deletion is not allowed.")
                    endsql = True
                    j = 1
                    Exit Sub
                End If
            End If
        Next

        If j <> 1 Then
            For Each dtgitem In dtgRegAutho.Items
                chk = dtgitem.FindControl("chkSelection")
                d_rAAbbr = dtgitem.Cells(1).Text
                d_rADesc = dtgitem.Cells(2).Text
                If chk.Checked Then
                    objinv.deleteRegAuthoritySetup(d_rAAbbr, d_rADesc)
                    j = 2
                End If
            Next
        End If

        'objinv.deleteRegAuthoritySetup(d_rAAbbr, d_rADesc)
        If j = 2 Then
            'visiblefalse()
            Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        End If

        If dtgRegAutho.Items.Count = 0 Then
            lblRegAutho.Visible = False
            cmdDelete.Enabled = False
            cmdModify.Enabled = False
        End If

        Bindgrid()
        RAI.Visible = False
        Table2.Visible = False
        ASave.Visible = False
        SS.Visible = False
        UU.Visible = False
        BSave.Visible = False
        objinv = Nothing
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Bindgrid()
        'RAI.Visible = False
        'Table2.Visible = False

        If cmdSave.Visible = False And cmdUpdate.Visible = False Then
            ASave.Visible = False
            BSave.Visible = False
        Else
            ASave.Visible = True
            BSave.Visible = True
        End If
        'SS.Visible = False
        'UU.Visible = False
        'cmdSave.Visible = False
        'cmdClear.Visible = False
        'cmdUpdate.Visible = False
        'cmdReset.Visible = False
        'cmdCancel.Visible = False
        cmdAdd.Visible = True
    End Sub

    Private Sub cmdClearSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClearSearch.Click
        txtSRegAuthoAbbr.Text = ""
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        BAdd.Visible = True
        RAI.Visible = True
        Table2.Visible = True
        ASave.Visible = True
        SS.Visible = True
        BSave.Visible = True
        cmdSave.Visible = True
        smr.Visible = True
        cmdClear.Visible = True
        cmdCancelSave.Visible = True
        UU.Visible = False
        txtRegAbbr.Text = ""
        txtRegDesc.Text = ""
        txtRegAbbr.Enabled = True
    End Sub

    Function visiblefalse()
        RAI.Visible = False
        Table2.Visible = False
        ASave.Visible = False
        SS.Visible = False
        UU.Visible = False
        BAdd.Visible = False
        AAdd.Visible = False
        BSave.Visible = False
        dtg.Visible = False
        smr.Visible = False
        tt.Visible = False
        cmdUpdate.Visible = False
        cmdReset.Visible = False
        cmdCancel.Visible = False
        cmdSave.Visible = False
        cmdClear.Visible = False
        cmdModify.Enabled = False
        cmdDelete.Enabled = False
    End Function

    Private Sub cmdCancelSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancelSave.Click
        BAdd.Visible = False
        RAI.Visible = False
        Table2.Visible = False
        ASave.Visible = False
        SS.Visible = False
        UU.Visible = False
        cmdModify.Enabled = True
        cmdDelete.Enabled = True
    End Sub
End Class
