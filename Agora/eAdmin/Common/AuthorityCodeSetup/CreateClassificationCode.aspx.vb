'*************************************************************************************
'Created By:  Louise
'Date:  13/05/2005
'Screen:  Create Classification Code
'Purpose:  To create the classification code - Hub Admin 

'**************************************************************************************

Imports ERFP.Components
Imports AgoraLegacy

Public Class ClassificationSetup
    Inherits AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtClassiID As System.Web.UI.WebControls.TextBox
    Protected WithEvents cboRegAutho As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtClassiDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdCancel As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.WebControls.Button
    Protected WithEvents cmdUpdate As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents dtgClassification As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button

    Protected WithEvents lblClassification As System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents rfv_ClassiID As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents rfv_ClassiDesc As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldsumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents rfv_cboRegAutho As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents SS As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents UU As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents txtSRegAutho As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClearSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents BAdd As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents CIH As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents ASave As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents BSave As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents smr As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents CL As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents dtg As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents AAdd As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents cmdCancelSave As System.Web.UI.WebControls.Button
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label

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
        SetGridProperty(dtgClassification)

        If Not Page.IsPostBack Then
            UU.Visible = False
            SS.Visible = True

            Common.FillDefault(Me.cboRegAutho, "code_mstr", "code_desc", "code_abbr", "", "code_category ='RA'")

            Dim lstItem As New ListItem
            lstItem.Value = ""
            lstItem.Text = "---- Select ----"
            cboRegAutho.Items.Insert(0, lstItem)

            cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
            cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")

            CIH.Visible = False
            Table2.Visible = False
            ASave.Visible = False
            SS.Visible = False
            UU.Visible = False
            BSave.Visible = False
            smr.Visible = False
            CL.Visible = False
            dtg.Visible = False
            BAdd.Visible = False
            AAdd.Visible = False
            cmdSave.Visible = False
            cmdClear.Visible = False
            'Bindgrid()
            txtClassiID.Enabled = True
        End If
    End Sub

    Public Sub OnPageIndexChanged_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objinv As New invClass
        Dim ds As DataSet = New DataSet
        Dim record As Integer

        ds = objinv.getClassificationSetup(txtSRegAutho.Text)
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
            If dtgClassification.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgClassification.PageSize = 0 Then
                dtgClassification.CurrentPageIndex = dtgClassification.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        If viewstate("intPageRecordCnt") > 0 Then

            ' check when user re-enter search criteria and click on other page without click search button
            If dtgClassification.CurrentPageIndex > (dvViewSample.Count \ dtgClassification.PageSize) Then
                dtgClassification.CurrentPageIndex = IIf((dvViewSample.Count \ dtgClassification.PageSize) = 1, 0, (dvViewSample.Count \ dtgClassification.PageSize))
            ElseIf dtgClassification.CurrentPageIndex = (dvViewSample.Count \ dtgClassification.PageSize) Then
                If viewstate("PageCount") = (dvViewSample.Count \ dtgClassification.PageSize) Then
                    'user does not re-enter search criteria 
                    dtgClassification.CurrentPageIndex = IIf((dvViewSample.Count \ dtgClassification.PageSize) = 0, 0, (dvViewSample.Count \ dtgClassification.PageSize) - 1)
                Else
                    If (dvViewSample.Count Mod dtgClassification.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dtgClassification.CurrentPageIndex = IIf((dvViewSample.Count \ dtgClassification.PageSize) = 1, 0, (dvViewSample.Count \ dtgClassification.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        dtgClassification.CurrentPageIndex = (dvViewSample.Count \ dtgClassification.PageSize)
                    End If
                End If
            End If
            '--------------------------------

            dtgClassification.DataSource = dvViewSample
            dtgClassification.DataBind()

            CL.Visible = True
            lblClassification.Visible = True
            dtg.Visible = True
            dtgClassification.Visible = True
            AAdd.Visible = True

            cmdModify.Enabled = True
            cmdDelete.Enabled = True
        Else
            dtgClassification.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            CL.Visible = False
            lblClassification.Visible = False
            dtg.Visible = False
            dtgClassification.Visible = False
            AAdd.Visible = False
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
        End If

        ' add for above checking
        viewstate("PageCount") = dtgClassification.PageCount
        objinv = Nothing

    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(True)
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objDBRFP As New EAD.DBCom(ConfigurationSettings.AppSettings("eRFPPath"))

        Dim saveRegAutho As String
        Dim saveClassiID As String
        Dim saveClassiDesc As String
        Dim strtemp As String
        Dim endsql As Boolean
        Dim objinv As New invClass

        saveRegAutho = cboRegAutho.SelectedItem.Value
        saveClassiID = txtClassiID.Text
        saveClassiDesc = txtClassiDesc.Text

        strtemp = objinv.saveClassificationSetup(saveRegAutho, saveClassiID, saveClassiDesc)
        If strtemp = "1" Then
            Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Information)
            endsql = True
        Else
            objDBRFP.Execute(strtemp)
            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
            cboRegAutho.ClearSelection()
            txtClassiID.Text = ""
            txtClassiDesc.Text = ""
        End If

        Bindgrid()
        objinv = Nothing
        objDBRFP = Nothing


    End Sub

    Private Sub dtgClassification_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgClassification.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgClassification, e)
    End Sub

    Private Sub dtgClassification_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgClassification.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            e.Item.Cells(1).Text = Common.parseNull(dv("RC_REG_ID")) & " - " & Common.parseNull(dv("CODE_DESC"))
        End If
    End Sub

    Private Sub cboRegAutho_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRegAutho.SelectedIndexChanged
        txtClassiID.Text = ""
        txtClassiDesc.Text = ""
    End Sub

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim objDb As New EAD.DBCom
        Dim strtemp As String
        Dim chk As CheckBox
        Dim dtgitem As DataGridItem
        Dim strAryQuery(0) As String
        Dim i As Integer
        Dim m_rA As String
        Dim rA As String
        Dim m_classID As String
        Dim m_classDesc As String
        Dim dt As DataTable
        Dim strRA(2) As String

        For Each dtgitem In dtgClassification.Items
            chk = dtgitem.FindControl("chkSelection")

            If chk.Checked Then
                m_rA = dtgitem.Cells(1).Text
                strRA = Split(m_rA, "-")
                rA = Trim(strRA(0))

                m_classID = dtgitem.Cells(2).Text
                m_classDesc = dtgitem.Cells(3).Text
            End If
        Next

        Common.FillDefault(cboRegAutho, "code_mstr", "code_desc", "code_abbr", "", "code_category ='RA'")

        Dim lstItem As New ListItem
        lstItem.Value = ""
        lstItem.Text = "---- Select ----"
        cboRegAutho.Items.Insert(0, lstItem)

        Common.SelDdl(rA, cboRegAutho)
        txtClassiID.Text = m_classID
        txtClassiDesc.Text = m_classDesc
        txtClassiID.Enabled = False

        savetodt()
        cboRegAutho.Enabled = False
        cmdSave.Visible = False
        cmdClear.Visible = False
        cmdModify.Enabled = True
        cmdDelete.Enabled = True
        CIH.Visible = True
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

    Function savetodt()
        ViewState("dt") = getDefaultValue()
    End Function

    Function getDefaultValue() As DataTable
        Dim test As String
        Dim dbt As New DataTable
        Dim dtr As DataRow
        Dim i As Integer

        '  Dim ds As New DataSet
        dbt.Columns.Add("REG_ABBR")
        dbt.Columns.Add("CLASSI_ID")
        dbt.Columns.Add("CLASSI_DESC")

        dtr = dbt.NewRow()
        dtr("REG_ABBR") = cboRegAutho.SelectedItem.Value
        dtr("CLASSI_ID") = txtClassiID.Text
        dtr("CLASSI_DESC") = txtClassiDesc.Text

        dbt.Rows.Add(dtr)

        Return dbt
    End Function

    Function get_DataFromDt() As DataTable
        Return CType(ViewState("dt"), DataTable)
    End Function

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim objDBRFP As New EAD.DBCom(ConfigurationSettings.AppSettings("eRFPPath"))
        Dim strtemp As String
        Dim chk As CheckBox
        Dim dtgitem As DataGridItem
        Dim strAryQuery(0) As String
        Dim i As Integer
        Dim d_rA As String
        Dim rA As String
        Dim d_classiID As String
        Dim d_classiDesc As String
        Dim objinv As New invClass
        Dim j As Integer = 0
        Dim strtemp2 As String
        Dim strsplit(2) As String

        For Each dtgitem In dtgClassification.Items
            chk = dtgitem.FindControl("chkSelection")

            d_rA = dtgitem.Cells(1).Text
            strsplit = Split(d_rA, "-")
            rA = Trim(strsplit(0))

            d_classiID = dtgitem.Cells(2).Text
            d_classiDesc = dtgitem.Cells(3).Text

            If chk.Checked Then
                strtemp2 = objinv.deleteClassificationSetup(rA, d_classiID, d_classiDesc)
                If strtemp2 = "1" Then
                    Common.NetMsgbox(Me, "Deletion is not allowed.", MsgBoxStyle.Information)
                    j = 1
                    Exit Sub
                End If
            End If
        Next

        If j <> 1 Then
            For Each dtgitem In dtgClassification.Items
                chk = dtgitem.FindControl("chkSelection")

                d_rA = dtgitem.Cells(1).Text
                strsplit = Split(d_rA, "-")
                rA = Trim(strsplit(0))

                d_classiID = dtgitem.Cells(2).Text
                d_classiDesc = dtgitem.Cells(3).Text

                If chk.Checked Then
                    strtemp2 = objinv.deleteClassificationSetup(rA, d_classiID, d_classiDesc)
                    If strtemp2 <> "1" Then
                        objDBRFP.Execute(strtemp2)
                        j = 2
                    End If
                End If
            Next
        End If

        If j = 2 Then
            'visiblefalse()
            Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        End If

        If dtgClassification.Items.Count = 0 Then
            lblClassification.Visible = False
            cmdDelete.Enabled = False
            cmdModify.Enabled = False
        End If

        Bindgrid()
        CIH.Visible = False
        Table2.Visible = False
        ASave.Visible = False
        SS.Visible = False
        UU.Visible = False
        BSave.Visible = False
        objinv = Nothing
        objDBRFP = Nothing


    End Sub

    Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        cboRegAutho.ClearSelection()
        txtClassiID.Text = ""
        txtClassiDesc.Text = ""
    End Sub

    Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click

        Dim objDBRFP As New EAD.DBCom(ConfigurationSettings.AppSettings("eRFPPath"))

        Dim u_RA As String
        Dim u_ClassiID As String
        Dim u_ClassiDesc As String
        Dim strtemp As String
        Dim endsql As Boolean
        Dim dt As DataTable
        Dim objinv As New invClass

        u_RA = cboRegAutho.SelectedItem.Value
        u_ClassiID = txtClassiID.Text()
        u_ClassiDesc = txtClassiDesc.Text

        dt = get_DataFromDt()

        strtemp = objinv.updateClassificationSetup(dt, u_RA, u_ClassiID, u_ClassiDesc)
        If strtemp = "1" Then
            Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Information)
            endsql = True
        Else
            objDBRFP.Execute(strtemp)
            Common.NetMsgbox(Me, "Record Updated.", MsgBoxStyle.Information)
            'visiblefalse()
            BAdd.Visible = False
            CIH.Visible = False
            Table2.Visible = False
            ASave.Visible = False
            SS.Visible = False
            UU.Visible = False
        End If

        Bindgrid()
        'UU.Visible = True
        objinv = Nothing
        objDBRFP = Nothing

    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Dim dt As DataTable

        dt = Me.get_DataFromDt()
        txtClassiID.Text = dt.Rows(0)(1)
        txtClassiID.Enabled = False
        txtClassiDesc.Text = dt.Rows(0)(2)
        UU.Visible = True

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        'visiblefalse()
        BAdd.Visible = False
        CIH.Visible = False
        Table2.Visible = False
        ASave.Visible = False
        SS.Visible = False
        UU.Visible = False
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Bindgrid()
        'CIH.Visible = False
        'Table2.Visible = False
        If cmdSave.Visible = False And cmdUpdate.Visible = False Then
            ASave.Visible = False
        Else
            ASave.Visible = True
        End If
        'SS.Visible = False
        'UU.Visible = False
        'BSave.Visible = False
        smr.Visible = False
        cmdAdd.Visible = True
    End Sub

    Private Sub cmdClearSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClearSearch.Click
        txtSRegAutho.Text = ""
    End Sub

    Function visiblefalse()
        CIH.Visible = False
        Table2.Visible = False
        ASave.Visible = False
        SS.Visible = False
        UU.Visible = False
        BSave.Visible = False
        smr.Visible = False
        CL.Visible = False
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

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        BAdd.Visible = True
        CIH.Visible = True
        Table2.Visible = True
        ASave.Visible = True
        SS.Visible = True
        BSave.Visible = True
        cmdSave.Visible = True
        smr.Visible = True
        cmdClear.Visible = True
        cmdCancelSave.Visible = True
        UU.Visible = False
        txtClassiID.Text = ""
        txtClassiDesc.Text = ""
        cboRegAutho.ClearSelection()
        cboRegAutho.Enabled = True
        txtClassiID.Enabled = True
    End Sub

    Private Sub cmdCancelSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancelSave.Click
        BAdd.Visible = False
        CIH.Visible = False
        Table2.Visible = False
        ASave.Visible = False
        SS.Visible = False
        UU.Visible = False
    End Sub
End Class
