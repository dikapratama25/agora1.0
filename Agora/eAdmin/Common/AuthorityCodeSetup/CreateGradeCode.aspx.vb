'*************************************************************************************
'Created By:  Louise
'Date:  16/05/2005
'Screen:  Create Grade Code
'Purpose:  To create the grade code - Hub Admin 

'**************************************************************************************

Imports ERFP.Components
Imports AgoraLegacy

Public Class GradeSetup
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cboRegAutho As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtGradeID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCapacity As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents dtgGrade As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdCancel As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.WebControls.Button
    Protected WithEvents cmdUpdate As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button

    Protected WithEvents lblGrade As System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents rfv_cboRegAutho As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_GradeID As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_Capacity As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfv_Desc As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldsumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents SS As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents UU As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents txtSRegAutho As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClearSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents BAdd As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents GI As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents ASave As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents BSave As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents smr As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents GL As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents dtg As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents AAdd As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents cmdCancelSave As System.Web.UI.WebControls.Button
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
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
        SetGridProperty(dtgGrade)

        If Not Page.IsPostBack Then
            UU.Visible = False
            SS.Visible = True

            Common.FillDefault(Me.cboRegAutho, "code_mstr", "code_desc", "code_abbr", "", "code_category ='RA'")
            cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
            cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")

            Dim lstItem As New ListItem
            lstItem.Value = ""
            lstItem.Text = "---- Select ----"
            cboRegAutho.Items.Insert(0, lstItem)

            GI.Visible = False
            Table2.Visible = False
            ASave.Visible = False
            SS.Visible = False
            UU.Visible = False
            BSave.Visible = False
            smr.Visible = False
            GL.Visible = False
            dtg.Visible = False
            BAdd.Visible = False
            AAdd.Visible = False
            cmdSave.Visible = False
            cmdClear.Visible = False
            txtGradeID.Enabled = True
            txtCapacity.Enabled = True
            'Bindgrid()
        End If
    End Sub

    Public Sub OnPageIndexChanged_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim ds As DataSet = New DataSet
        Dim record As Integer
        Dim objinv As New invClass

        ds = objinv.getGradeSetup(txtSRegAutho.Text)
        record = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = viewstate("SortExpression")

        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If dtgGrade.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgGrade.PageSize = 0 Then
                dtgGrade.CurrentPageIndex = dtgGrade.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        If viewstate("intPageRecordCnt") > 0 Then

            ' check when user re-enter search criteria and click on other page without click search button
            If dtgGrade.CurrentPageIndex > (dvViewSample.Count \ dtgGrade.PageSize) Then
                dtgGrade.CurrentPageIndex = IIf((dvViewSample.Count \ dtgGrade.PageSize) = 1, 0, (dvViewSample.Count \ dtgGrade.PageSize))
            ElseIf dtgGrade.CurrentPageIndex = (dvViewSample.Count \ dtgGrade.PageSize) Then
                If viewstate("PageCount") = (dvViewSample.Count \ dtgGrade.PageSize) Then
                    'user does not re-enter search criteria 
                    dtgGrade.CurrentPageIndex = IIf((dvViewSample.Count \ dtgGrade.PageSize) = 0, 0, (dvViewSample.Count \ dtgGrade.PageSize) - 1)
                Else
                    If (dvViewSample.Count Mod dtgGrade.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dtgGrade.CurrentPageIndex = IIf((dvViewSample.Count \ dtgGrade.PageSize) = 1, 0, (dvViewSample.Count \ dtgGrade.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        dtgGrade.CurrentPageIndex = (dvViewSample.Count \ dtgGrade.PageSize)
                    End If
                End If
            End If
            '--------------------------------

            dtgGrade.DataSource = dvViewSample
            dtgGrade.DataBind()

            GL.Visible = True
            lblGrade.Visible = True
            dtg.Visible = True
            dtgGrade.Visible = True
            AAdd.Visible = True
            cmdModify.Enabled = True
            cmdDelete.Enabled = True
        Else
            dtgGrade.DataBind()

            GL.Visible = False
            lblGrade.Visible = False
            dtg.Visible = False
            dtgGrade.Visible = False
            AAdd.Visible = False
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
        End If

        ' add for above checking
        viewstate("PageCount") = dtgGrade.PageCount
        objinv = Nothing

    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(True)
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objDB As New  EAD.DBCom(ConfigurationSettings.AppSettings("eRFPPath"))
        Dim saveRegAutho As String
        Dim saveGradeID As String
        Dim saveGradeCapacity As String
        Dim saveGradeDesc As String
        Dim strtemp As String
        Dim endsql As Boolean
        Dim objinv As New invClass

        saveRegAutho = cboRegAutho.SelectedItem.Value
        saveGradeID = txtGradeID.Text
        saveGradeCapacity = txtCapacity.Text
        saveGradeDesc = txtDesc.Text

        strtemp = objinv.saveGradeSetup(saveRegAutho, saveGradeID, saveGradeCapacity, saveGradeDesc)
        If strtemp = "1" Then
            Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Information)
            endsql = True
        Else
            objDB.Execute(strtemp)
            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
            cboRegAutho.ClearSelection()
            txtGradeID.Text = ""
            txtCapacity.Text = ""
            txtDesc.Text = ""
        End If

        Bindgrid()
        objinv = Nothing
        objDB = Nothing


    End Sub

    Private Sub dtgGrade_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGrade.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgGrade, e)
    End Sub

    Private Sub dtgGrade_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGrade.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            e.Item.Cells(1).Text = Common.parseNull(dv("RG_REG_ID")) & " - " & Common.parseNull(dv("CODE_DESC"))
        End If
    End Sub

    Function savetodt()
        viewstate("dt") = getDefaultValue()
    End Function

    Function getDefaultValue() As DataTable
        Dim test As String
        Dim dbt As New DataTable
        Dim dtr As DataRow
        Dim i As Integer

        dbt.Columns.Add("REG_ABBR")
        dbt.Columns.Add("GRADE_ID")
        dbt.Columns.Add("GRADE_CAPACITY")
        dbt.Columns.Add("GRADE_DESC")

        dtr = dbt.NewRow()
        dtr("REG_ABBR") = cboRegAutho.SelectedItem.Value
        dtr("GRADE_ID") = txtGradeID.Text
        dtr("GRADE_CAPACITY") = txtCapacity.Text
        dtr("GRADE_DESC") = txtDesc.Text

        dbt.Rows.Add(dtr)

        Return dbt
    End Function

    Function get_DataFromDt() As DataTable
        Return CType(viewstate("dt"), DataTable)
    End Function

    Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        cboRegAutho.ClearSelection()
        txtGradeID.Text = ""
        txtCapacity.Text = ""
        txtDesc.Text = ""
    End Sub

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click

        Dim strtemp As String
        Dim chk As CheckBox
        Dim dtgitem As DataGridItem
        Dim strAryQuery(0) As String
        Dim i As Integer
        Dim m_rA As String
        Dim rA As String
        Dim m_gradeID As String
        Dim m_capacity As String
        Dim m_gradeDesc As String
        Dim dt As DataTable
        Dim strRA(2) As String

        For Each dtgitem In dtgGrade.Items
            chk = dtgitem.FindControl("chkSelection")

            If chk.Checked Then
                m_rA = dtgitem.Cells(1).Text
                strRA = Split(m_rA, "-")
                rA = Trim(strRA(0))

                m_gradeID = dtgitem.Cells(2).Text
                m_capacity = dtgitem.Cells(3).Text
                m_gradeDesc = dtgitem.Cells(4).Text
            End If
        Next

        Common.FillDefault(cboRegAutho, "code_mstr", "code_desc", "code_abbr", "", "code_category ='RA'")

        Dim lstItem As New ListItem
        lstItem.Value = ""
        lstItem.Text = "---- Select ----"
        cboRegAutho.Items.Insert(0, lstItem)

        Common.SelDdl(rA, cboRegAutho)
        txtGradeID.Text = m_gradeID
        txtCapacity.Text = m_capacity
        txtDesc.Text = m_gradeDesc
        txtGradeID.Enabled = False
        txtCapacity.Enabled = False

        savetodt()
        cboRegAutho.Enabled = False
        cmdSave.Visible = False
        cmdClear.Visible = False
        GI.Visible = True
        Table2.Visible = True
        ASave.Visible = True
        BSave.Visible = True
        smr.Visible = True
        cmdUpdate.Visible = True
        cmdReset.Visible = True
        cmdCancel.Visible = True
        SS.Visible = False
        UU.Visible = True
        BAdd.Visible = True
    End Sub

    Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
        Dim objDB As New  EAD.DBCom(ConfigurationSettings.AppSettings("eRFPPath"))
        Dim u_RA As String
        Dim u_gradeID As String
        Dim u_capacity As String
        Dim u_gradeDesc As String
        Dim strtemp As String
        Dim endsql As Boolean
        Dim dt As DataTable
        Dim objinv As New invClass

        u_RA = cboRegAutho.SelectedItem.Value
        u_gradeID = txtGradeID.Text
        u_capacity = txtCapacity.Text
        u_gradeDesc = txtDesc.Text

        If u_RA = "" Or u_gradeID = "" Or u_capacity = "" Or u_gradeDesc = "" Then
            Common.NetMsgbox(Me, "Please enter a value for each field.")
        Else
            dt = get_DataFromDt()

            strtemp = objinv.updateGradeSetup(dt, u_RA, u_gradeID, u_capacity, u_gradeDesc)
            If strtemp = "1" Then
                Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Information)
                endsql = True
            Else
                objDB.Execute(strtemp)
                Common.NetMsgbox(Me, "Record Updated.", MsgBoxStyle.Information)
                'visiblefalse()
                BAdd.Visible = False
                GI.Visible = False
                Table2.Visible = False
                ASave.Visible = False
                SS.Visible = False
                UU.Visible = False
            End If
        End If

        Bindgrid()
        'UU.Visible = True
        objinv = Nothing
        objDB = Nothing


    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim objDB As New  EAD.DBCom(ConfigurationSettings.AppSettings("eRFPPath"))
        Dim strtemp As String
        Dim chk As CheckBox
        Dim dtgitem As DataGridItem
        Dim strAryQuery(0) As String
        Dim i As Integer
        Dim d_rA As String
        Dim rA As String
        Dim d_gradeID As String
        Dim d_capacity As String
        Dim d_gradeDesc As String
        Dim objinv As New invClass
        Dim j As Integer
        Dim strtemp2 As String
        Dim strRA(2) As String

        For Each dtgitem In dtgGrade.Items
            chk = dtgitem.FindControl("chkSelection")

            d_rA = dtgitem.Cells(1).Text
            strRA = Split(d_rA, "-")
            rA = Trim(strRA(0))

            d_gradeID = dtgitem.Cells(2).Text
            d_capacity = dtgitem.Cells(3).Text
            d_gradeDesc = dtgitem.Cells(4).Text
            If chk.Checked Then
                strtemp2 = objinv.deleteGradeSetup(rA, d_gradeID, d_capacity, d_gradeDesc)
                If strtemp2 = "1" Then
                    Common.NetMsgbox(Me, "Deletion is not allowed.", MsgBoxStyle.Information)
                    j = 1
                    Exit Sub
                End If
            End If
        Next

        If j <> 1 Then
            For Each dtgitem In dtgGrade.Items
                chk = dtgitem.FindControl("chkSelection")

                d_rA = dtgitem.Cells(1).Text
                strRA = Split(d_rA, "-")
                rA = Trim(strRA(0))

                d_gradeID = dtgitem.Cells(2).Text
                d_capacity = dtgitem.Cells(3).Text
                d_gradeDesc = dtgitem.Cells(4).Text
                If chk.Checked Then
                    strtemp2 = objinv.deleteGradeSetup(rA, d_gradeID, d_capacity, d_gradeDesc)
                    If strtemp2 <> "1" Then
                        objDb.Execute(strtemp2)
                        j = 2
                    End If
                End If
            Next
        End If

        If j = 2 Then
            'visiblefalse()
            Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        End If

        If dtgGrade.Items.Count = 0 Then
            lblGrade.Visible = False
            cmdDelete.Enabled = False
            cmdModify.Enabled = False
        End If

        Bindgrid()
        GI.Visible = False
        Table2.Visible = False
        ASave.Visible = False
        SS.Visible = False
        UU.Visible = False
        BSave.Visible = False
        objinv = Nothing
        objDB = Nothing


    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Dim dt As DataTable

        dt = Me.get_DataFromDt()
        txtGradeID.Text = dt.Rows(0)(1)
        txtCapacity.Text = dt.Rows(0)(2)
        txtDesc.Text = dt.Rows(0)(3)
        UU.Visible = True

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        'visiblefalse()
        BAdd.Visible = False
        GI.Visible = False
        Table2.Visible = False
        ASave.Visible = False
        SS.Visible = False
        UU.Visible = False
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Bindgrid()

        If cmdSave.Visible = False And cmdUpdate.Visible = False Then
            ASave.Visible = False
            BSave.Visible = False
        Else
            ASave.Visible = True
            BSave.Visible = True
        End If

        smr.Visible = False
        cmdAdd.Visible = True
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        BAdd.Visible = True
        GI.Visible = True
        Table2.Visible = True
        ASave.Visible = True
        SS.Visible = True
        BSave.Visible = True
        cmdSave.Visible = True
        smr.Visible = True
        cmdClear.Visible = True
        cmdCancelSave.Visible = True
        txtGradeID.Text = ""
        txtCapacity.Text = ""
        txtDesc.Text = ""
        cboRegAutho.ClearSelection()
        cboRegAutho.Enabled = True
        txtGradeID.Enabled = True
        txtCapacity.Enabled = True
    End Sub

    Function visiblefalse()
        GI.Visible = False
        Table2.Visible = False
        ASave.Visible = False
        SS.Visible = False
        UU.Visible = False
        BSave.Visible = False
        smr.Visible = False
        GL.Visible = False
        dtg.Visible = False
        BAdd.Visible = False
        AAdd.Visible = False
        cmdUpdate.Visible = False
        cmdReset.Visible = False
        cmdCancel.Visible = False
        cmdSave.Visible = False
        cmdClear.Visible = False
        cmdModify.Enabled = False
        cmdDelete.Enabled = False
    End Function

    Private Sub cmdClearSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClearSearch.Click
        txtSRegAutho.Text = ""
    End Sub

    Private Sub cmdCancelSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancelSave.Click
        BAdd.Visible = False
        GI.Visible = False
        Table2.Visible = False
        ASave.Visible = False
        SS.Visible = False
        UU.Visible = False
    End Sub
End Class
