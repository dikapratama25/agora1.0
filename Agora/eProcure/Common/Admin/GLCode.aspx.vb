
Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data

Public Class GLCode
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals


#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents cmd_search As System.Web.UI.WebControls.Button
	Protected WithEvents cmd_clear As System.Web.UI.WebControls.Button
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAction As System.Web.UI.WebControls.Label
	Protected WithEvents hide As System.Web.UI.HtmlControls.HtmlGenericControl
	Protected WithEvents txtAddGLCode As System.Web.UI.WebControls.TextBox
	Protected WithEvents txtGLCode As System.Web.UI.WebControls.TextBox
	Protected WithEvents saveGL As System.Web.UI.WebControls.Button
	Protected WithEvents claerGL As System.Web.UI.WebControls.Button
	Protected WithEvents Label3 As System.Web.UI.WebControls.Label
	Protected WithEvents rfv_code As System.Web.UI.WebControls.RequiredFieldValidator
	Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
	Protected WithEvents MyDataGrid As System.Web.UI.WebControls.DataGrid
	Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
	Protected WithEvents cmd_delete As System.Web.UI.WebControls.Button

	Protected ds As DataTable = New DataTable
	Protected WithEvents glRadioBtn As System.Web.UI.WebControls.RadioButton
	Protected WithEvents catCodeRadioBtn As System.Web.UI.WebControls.RadioButton
	Protected WithEvents headerlbl As System.Web.UI.WebControls.Label
	Protected WithEvents srchCriLbl As System.Web.UI.WebControls.Label
	Protected WithEvents addLbl As System.Web.UI.WebControls.Label
    Protected WithEvents glDesclbl As System.Web.UI.WebControls.Label
    Protected WithEvents txtGLCodeDescription As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents cmd_modify As System.Web.UI.WebControls.Button
    Protected WithEvents btnClrGL As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents btncancel As System.Web.UI.HtmlControls.HtmlInputButton
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

	Private Enum SearchCriteria
		GLCode	      'used as default search
        CategoryCode
        GLCodeCategoryCode ''Jules 2013.12.09
	End Enum

	Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Put user code to initialize the page here
		MyBase.Page_Load(sender, e)

		If Not Page.IsPostBack Then
            MyBase.SetGridProperty(Me.MyDataGrid)
            MyBase.SetGridProperty(Me.dgGLCategory)
            blnPaging = False
            MyBase.SetGridProperty(Me.dgCategory)
			Me.cmd_delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
            Me.saveGL.Attributes.Add("onclick", "return ValidateGLCode();")
            Me.saveGLCategory.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','save');")
			Me.cmd_modify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
			Me.glRadioBtn.Checked = True
			viewstate.Item("SearchMode") = SearchCriteria.GLCode		     'default search mode
            Me.cmd_delete.Style("display") = "none" ''Jules 2014.03.31
            Me.cmd_modify.Style("display") = "none" ''Jules 2014.03.31
		End If

	End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        If Not CStr(ViewState.Item("SearchMode")).Equals(SearchCriteria.GLCodeCategoryCode.ToString()) Then
            If pPage = -1 Then
                Me.MyDataGrid.CurrentPageIndex = 0
            Else
                Me.MyDataGrid.CurrentPageIndex = pPage
            End If
            Page.DataBind()
            'If Session("Env") = "FTN" Then
            '    Me.MyDataGrid.Columns(1).Visible = False
            'Else
            '    Me.MyDataGrid.Columns(1).Visible = True
            'End If
            Me.MyDataGrid.Columns(1).Visible = True
        Else
            Page.DataBind()
        End If
    End Function

    Private Sub MyDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles MyDataGrid.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "');")
        End If
    End Sub

	Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles MyDataGrid.SortCommand
		Grid_SortCommand(sender, e)
		Bindgrid(MyDataGrid.CurrentPageIndex, True)
	End Sub

	Public Sub MyDataGrid_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles MyDataGrid.PageIndexChanged
		MyDataGrid.CurrentPageIndex = e.NewPageIndex
		Bindgrid(e.NewPageIndex, True)
	End Sub

	'Provide data source for datagrid
	Public Function LoadDataSource() As DataView
        Dim objDb As New EAD.DBCom
        Dim companyID As String = Session("CompanyId")
        Dim strSC As String = CStr(ViewState.Item("SearchMode"))
        Dim dvViewSample As DataView

        Dim sc As SearchCriteria = SearchCriteria.GLCode
        If strSC.Equals(SearchCriteria.CategoryCode.ToString()) Then
            sc = SearchCriteria.CategoryCode
        ElseIf strSC.Equals(SearchCriteria.GLCodeCategoryCode.ToString()) Then
            sc = SearchCriteria.GLCodeCategoryCode
            Return Nothing
        End If

        Dim sql As String = ""

        'IF search criterial is category code
        '	remove the GL description column
        'ELSE
        '	query the GL description column
        '	display the GL description column
        If sc = SearchCriteria.CategoryCode Then
            sql = "SELECT CBC_B_CATEGORY_CODE FROM COMPANY_B_CATEGORY_CODE WHERE " _
                & "CBC_B_COY_ID ='" & companyID & "'"
            Me.MyDataGrid.Columns.RemoveAt(2)                'remove GL description column

        ElseIf sc = SearchCriteria.GLCode Then
            '1 Oct 2009
            'Added CBG_B_GL_DESC column into query when searching GLCode
            sql = "SELECT CBG_B_GL_CODE , CBG_B_GL_DESC FROM COMPANY_B_GL_CODE WHERE " _
                & "CBG_B_COY_ID = '" & companyID & "'"
            'show description column
            Me.MyDataGrid.Columns(2).Visible = True
        End If

        If Me.txtGLCode.Text <> "" Then
            If sc = SearchCriteria.GLCode Then
                sql = sql & " AND CBG_B_GL_CODE LIKE '%" & Me.txtGLCode.Text & "%'"
            ElseIf sc = SearchCriteria.CategoryCode Then
                sql = sql & " AND CBC_B_CATEGORY_CODE LIKE '%" & Me.txtGLCode.Text & "%'"
            End If
        End If


        'Dim dvViewSample As DataView
        Try
            Dim dt As DataTable = New DataTable
            dt = objDb.FillDt(sql)

            'Both search criteria( By GL Code/By category code) 's column
            ' is named as [DATA_COL], is make it easy for setting the 
            'sort expression for this column
            dt.Columns(0).ColumnName = "DATA_COL"                'column that display GL Code/Category code

            If sc = SearchCriteria.CategoryCode Then
                'dt.Columns("DATA_COL").Caption = "Category Code"
                Me.MyDataGrid.Columns(1).HeaderText = "Category Code"
                ViewState("SortExpression") = "DATA_COL"
            Else
                'dt.Columns("DATA_COL").Caption = "GL Code"
                Me.MyDataGrid.Columns(1).HeaderText = "GL Code"
            End If
            dvViewSample = dt.DefaultView
            dvViewSample.Sort = ViewState("SortExpression")
            intPageRecordCnt = dt.Rows.Count
            ViewState("intPageRecordCnt") = intPageRecordCnt

            Dim st As String = dvViewSample.Table.Columns("DATA_COL").Caption
            'IF the number of record is more then 0,
            '	enable the [modify] button
            'ELSE
            '	disable the [modify] button( no needed by user )
            If intPageRecordCnt > 0 Then
                Me.cmd_modify.Enabled = True
            Else
                Me.cmd_modify.Enabled = True
            End If

            If ViewState("SortAscending") = "no" Then
                dvViewSample.Sort += " DESC"
            End If
            ViewState("PageCount") = MyDataGrid.PageCount
            If intPageRecordCnt > 0 Then
                resetDatagridPageIndex(MyDataGrid, dvViewSample)
            Else
                resetDatagridPageIndex(MyDataGrid, dvViewSample)
                AgoraLegacy.Common.NetMsgbox(Me, MsgNoRecord)
            End If
            'If Session("Env") = "FTN" Then
            '    Me.MyDataGrid.Columns(1).Visible = False
            'Else
            '    Me.MyDataGrid.Columns(1).Visible = True
            'End If
            Me.MyDataGrid.Columns(1).Visible = True

            ''Jules 2014.03.18
            If sc <> SearchCriteria.GLCodeCategoryCode Then
                Me.MyDataGrid.Style("display") = ""
                Me.divGLCategory.Style("display") = "none"
                Me.divCategory.Style("display") = "none"
                If intPageRecordCnt > 0 Then
                    Me.cmd_delete.Style("display") = ""
                    Me.cmd_modify.Style("display") = ""
                Else
                    Me.cmd_delete.Style("display") = "none"
                    Me.cmd_modify.Style("display") = "none"
                End If
            Else
                Me.MyDataGrid.Style("display") = "none"
                Me.divGLCategory.Style("display") = ""
                Me.divCategory.Style("display") = "none"
            End If
        Catch ex As Exception
            'process error here
            AgoraLegacy.Common.NetMsgbox(Me, "Error occur during retrieve data, please try again later")
        End Try
        Return dvViewSample

    End Function

    Private Sub MyDataGrid_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles MyDataGrid.ItemCreated
        Grid_ItemCreated(MyDataGrid, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub cmd_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_search.Click
        Me.divAddGLCategory.Style("display") = "none"
        Me.divCategory.Style("display") = "none"
        ViewState("EditMode") = Nothing
        ViewState("SortAscending") = "yes"
        Page.DataBind()
    End Sub

    'method to update GL Description
    Private Sub updateGL()
        Dim objDb As New EAD.DBCom
        Dim tablename As String = "COMPANY_B_GL_CODE"

        Dim company As String = CStr(Session("CompanyId"))
        Dim glDesc As String = AgoraLegacy.Common.Parse(Me.txtGLCodeDescription.Text)

        Dim sql As String
        sql = "UPDATE " & tablename &
            " SET CBG_B_GL_DESC = '" & Me.txtGLCodeDescription.Text.Replace("'", "\'") & "' " &
            " WHERE CBG_B_COY_ID = '" & company &
            "' AND CBG_B_GL_CODE = '" & Me.txtAddGLCode.Text.Replace("'", "\'") & "'"

        Dim sucess As Boolean = objDb.Execute(sql)

        If sucess Then
            AgoraLegacy.Common.NetMsgbox(Me, MsgRecordSave)
            hide.Style("display") = "none"
        Else
            AgoraLegacy.Common.NetMsgbox(Me, MsgRecordNotSave)
        End If

        Me.btnClrGL.Value = "Clear"
        Me.txtAddGLCode.Enabled = True

        txtAddGLCode.Text = ""
        Me.txtGLCodeDescription.Text = ""
        ViewState("EditMode") = Nothing

    End Sub

    Private Sub saveGL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveGL.Click

        Dim editing As Boolean = False
        If Not ViewState("EditMode") Is Nothing Then
            editing = DirectCast(ViewState("EditMode"), Boolean)
        End If

        If editing Then
            updateGL()
        Else
            'insert the new GL code
            Dim objDb As New EAD.DBCom

            'Step 1: Get current company's ID
            Dim company As String = CStr(Session("CompanyId"))

            'Step 2: Get the GL code enter by user and prevent malicious input(sql injection)
            Dim glCode As String = AgoraLegacy.Common.Parse(Me.txtAddGLCode.Text)

            '1 Oct 2009
            'GL Code cannot have "(" and ")"
            If glCode Is Nothing Then
                AgoraLegacy.Common.NetMsgbox(Me, "GL Code and Description required")
                Return
            Else
                If glCode.IndexOf("(") > -1 Or glCode.IndexOf(")") > -1 Then
                    AgoraLegacy.Common.NetMsgbox(Me, MsgInvalidValue)
                    Return
                End If
            End If

            txtAddGLCode.Text = ""
            txtGLCode.Text = ""

            'Step 3: Check the GL code/Category Code exist in DB or not

            'determine search criteria
            Dim strSC As String = CStr(ViewState.Item("SearchMode"))
            Dim sc As SearchCriteria = SearchCriteria.GLCode             'default mode
            If strSC.Equals(SearchCriteria.CategoryCode.ToString()) Then
                sc = SearchCriteria.CategoryCode
            End If

            'determine primary keys and table name
            '====================================================
            'pk : the GL Code/Category Code column
            'For GL Code, 
            '	pk = CBG_B_GL_CODE
            'For Category Code
            '	pk = CBC_B_CATEGORY_CODE
            Dim pk As String = "CBG_B_GL_CODE "
            If sc = SearchCriteria.CategoryCode Then
                pk = "CBC_B_CATEGORY_CODE"
            End If

            'pk2 : the Company ID column
            'For GL Code, 
            '	pk = CBG_B_COY_ID
            'For Category Code
            '	pk = CBC_B_COY_ID
            Dim pk2 As String = "CBG_B_COY_ID"
            If sc = SearchCriteria.CategoryCode Then
                pk2 = "CBC_B_COY_ID"
            End If

            'tablename : the target table name used to store new record
            'For GL Code, 
            '	table name = COMPANY_B_GL_CODE
            'For Category Code
            '	table name = COMPANY_B_CATEGORY_CODE
            Dim tablename As String = "COMPANY_B_GL_CODE"
            If sc = SearchCriteria.CategoryCode Then
                tablename = "COMPANY_B_CATEGORY_CODE"
            End If

            'end determine primary keys and table name
            '====================================================

            'construct sql statement
            Dim verifySQL As String = "SELECT " & pk &
            " FROM " & tablename &
            " WHERE " & pk & " = '" & glCode & "' " &
            " AND " & pk2 & " = '" & company & "'"

            'If sc = SearchCriteria.GLCode Then
            '	verifySQL = verifySQL & " AND CBG_B_COY_ID = '" & company & "'"
            'ElseIf sc = SearchCriteria.CategoryCode Then
            '	verifySQL = verifySQL & " AND CBC_B_COY_ID = '" & company & "'"
            'End If

            'If the pending added record exist in database
            Dim tDS As DataSet = objDb.FillDs(verifySQL)
            If tDS.Tables(0).Rows.Count > 0 Then
                AgoraLegacy.Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Information)
                Page.DataBind()
                Exit Sub
            End If

            'If not exist, then add the GL code
            'Step 4: Construct SQL string
            Dim sql As String
            If tablename = "COMPANY_B_GL_CODE" Then
                sql = "INSERT INTO " & tablename & "(cbg_b_coy_id,cbg_b_gl_code,cbg_b_gl_desc) VALUES( '" &
                                    company & "' , '" & glCode & "'"
            Else
                sql = "INSERT INTO " & tablename & " VALUES( '" &
                                    company & "' , '" & glCode & "'"
            End If


            '1 Oct 2009
            'if category is GL code, add the gl description
            If tablename.Equals("COMPANY_B_GL_CODE") Then
                Dim glDesc As String = AgoraLegacy.Common.Parse(Me.txtGLCodeDescription.Text)
                sql = sql & " , '" & glDesc & "' "
                Me.txtGLCodeDescription.Text = ""
            End If
            sql = sql & ")"

            'Step 5: Exceute the query
            Dim sucess As Boolean = objDb.Execute(sql)

            'Step 6: Tell user about the result
            If Not sucess Then
                'Let the user know if there is any error during adding new GL code/Category Code
                AgoraLegacy.Common.NetMsgbox(Me, MsgRecordNotSave)
            Else
                AgoraLegacy.Common.NetMsgbox(Me, MsgRecordSave)
            End If

        End If

        'Step 7: Remove the previous temporary GL Code/Description from viewstate
        ViewState.Remove("oldGL")             'not needed after saved
        ViewState.Remove("oldDesc")
        ViewState("Current_Val") = ""
        Page.DataBind()

    End Sub

    Private Sub cmd_delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_delete.Click

        If Not CStr(ViewState.Item("SearchMode")).Equals(SearchCriteria.GLCodeCategoryCode.ToString()) Then
            Dim strAryQuery(0) As String
            Dim dgItem As DataGridItem
            txtGLCode.Text = ""
            Dim chk As CheckBox
            Dim glCode As String
            Dim i As Integer = 0         'used to iterate every rows in the table
            Dim row As Integer = 0
            Dim objDb As New EAD.DBCom
            Dim companyID As String = CStr(Session("CompanyId"))

            Dim col As String = "POD_B_GL_CODE" '"prd_b_gl_code"
            Dim tablename As String = "COMPANY_B_GL_CODE"
            Dim deleteCol As String = "CBG_B_GL_CODE"
            Dim companyIDCol As String = "CBG_B_COY_ID"

            Dim strSC As String = CStr(ViewState.Item("SearchMode"))
            Dim sc As SearchCriteria = SearchCriteria.GLCode          'default mode

            If strSC.Equals(SearchCriteria.CategoryCode.ToString()) Then
                sc = SearchCriteria.CategoryCode
            End If

            If sc = SearchCriteria.CategoryCode Then
                col = "POD_B_CATEGORY_CODE" '"PRD_B_CATEGORY_CODE"
                tablename = "COMPANY_B_CATEGORY_CODE"
                deleteCol = "CBC_B_CATEGORY_CODE"
                companyIDCol = "CBC_B_COY_ID"
            End If

            'used to store GL Code that is already used by
            'Buyer GL Code 
            Dim glTable As DataTable

            'Before delete a GL Code, check whether it is already used by 
            'Buyer GL Code 
            Dim checkSQL As String
            If sc = SearchCriteria.GLCode Then
                checkSQL = "Select CBC_B_GL_CODE " &
                "FROM COMPANY_B_ITEM_CODE " &
                "INNER JOIN PRODUCT_MSTR " &
                "ON CBC_PRODUCT_CODE = PM_PRODUCT_CODE " &
                "AND CBC_B_COY_ID = '" & companyID & "' " &
                "WHERE ( LENGTH(CBC_B_GL_CODE) > 0 )  " &
                "AND PM_DELETED <> 'Y'  "
                glTable = New DataTable
                glTable = objDb.FillDt(checkSQL)
            End If

            Dim errorMsg As String
            Dim blnFound As Boolean = False
            Dim deletedRows As Integer = 0        'store number of rows was removed
            For Each dgItem In MyDataGrid.Items

                chk = dgItem.FindControl("chkSelection")
                glCode = MyDataGrid.DataKeys.Item(i)

                If chk.Checked Then
                    If Not ContainValue(glTable, glCode) Then
                        glCode = AgoraLegacy.Common.Parse(glCode)

                        If sc = SearchCriteria.GLCode Then
                            blnFound = (objDb.Get1Column("PRODUCT_MSTR", "PM_ACCT_CODE", " WHERE PM_S_COY_ID = '" & companyID & "' AND PM_ACCT_CODE = '" & glCode & "' ") = "")

                            If blnFound = False Then
                                errorMsg = "Cannot delete record (" & glCode & ") as it is tie to product item."
                                AgoraLegacy.Common.NetMsgbox(Me, errorMsg, MsgBoxStyle.Information)
                                Exit Sub
                            End If
                        ElseIf sc = SearchCriteria.CategoryCode Then
                            blnFound = (objDb.Get1Column("PRODUCT_MSTR", "PM_CAT_CODE", " WHERE PM_S_COY_ID = '" & companyID & "' AND PM_CAT_CODE = '" & glCode & "' ") = "")

                            If blnFound = False Then
                                errorMsg = "Cannot delete record (" & glCode & ") as it is tie to product item."
                                AgoraLegacy.Common.NetMsgbox(Me, errorMsg, MsgBoxStyle.Information)
                                Exit Sub
                            End If
                        End If

                        'Dim sql As String = "UPDATE  pr_details SET " & col & " = '' " & _
                        '"WHERE prd_coy_id = '" & companyID & "' AND " & col & " = '" & glCode & "' AND " & _
                        '"((prd_pr_no + prd_coy_id + CONVERT(char,prd_pr_line)) IN (select (prd_pr_no + prd_coy_id + convert(char,prd_pr_line))" & _
                        '"FROM pr_mstr, pr_details WHERE prd_coy_id = '" & companyID & "' and " & col & " = '" & glCode & "' AND prd_pr_no = prm_pr_no AND " & _
                        '"prd_coy_id = prm_coy_id AND prm_pr_status = '1'))"

                        'sql = sql & " ; DELETE FROM " & tablename & " WHERE " & _
                        'companyIDCol & " = '" & companyID & "' AND " & deleteCol & " ='" & glCode & "'"

                        'objDb.Execute(sql)
                        Dim sql As String = "select CONCAT(pod_po_no , pod_coy_id , CONVERT(pod_po_line,CHAR)) " &
                                           "FROM po_mstr, po_details WHERE pod_coy_id = '" & companyID & "' and " & col & " = '" & glCode & "' AND pod_po_no = pom_po_no AND " &
                                           "pod_coy_id = pom_b_coy_id AND pom_po_status = '1'"
                        Dim tDS As DataSet = objDb.FillDs(sql)
                        If tDS.Tables(0).Rows.Count > 0 Then
                            For row = 0 To tDS.Tables(0).Rows.Count - 1
                                sql = "UPDATE po_details SET " & col & " = '' " &
                                        "WHERE pod_coy_id = '" & companyID & "' AND " & col & " = '" & glCode & "' AND " &
                                        "(CONCAT(pod_po_no , pod_coy_id , CONVERT(pod_po_line,CHAR)) = '" & tDS.Tables(0).Rows(row).Item(0) & "')"
                                Common.Insert2Ary(strAryQuery, sql)
                            Next
                        End If
                        sql = "DELETE FROM " & tablename & " WHERE " &
                                companyIDCol & " = '" & companyID & "' AND " & deleteCol & " ='" & glCode & "'"
                        Common.Insert2Ary(strAryQuery, sql)
                        objDb.BatchExecute(strAryQuery)
                        deletedRows = deletedRows + 1
                    End If
                End If

                i = i + 1
            Next

            If deletedRows > 0 Then
                AgoraLegacy.Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
            Else
                errorMsg = "Cannot delete record as it is found in Buyer Item Code Assignment"
                AgoraLegacy.Common.NetMsgbox(Me, errorMsg, MsgBoxStyle.Information)
            End If
        Else
            DeleteGLCodeCategoryCode()
        End If
        Page.DataBind()

    End Sub

    Private Function ContainValue(ByVal table As DataTable, ByVal value As Object) As Boolean
        If Not table Is Nothing Then
            For Each row As DataRow In table.Rows
                If Not IsDBNull(row.Item(0)) Then
                    If CStr(row.Item(0)).Equals(value) Then
                        Return True
                    End If
                End If
            Next
        End If
        Return False
    End Function

    Private Sub glRadioBtn_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles glRadioBtn.CheckedChanged
        SetSearchCriteria(SearchCriteria.GLCode)
        Me.txtAddGLCode.Text = ViewState("Current_Val")

        ''Jules 2014.03.18
        Me.MyDataGrid.Style("display") = "none"
        Me.divGLCategory.Style("display") = "none"
        Me.divCategory.Style("display") = "none"
        Me.divAddGLCategory.Style("display") = "none"
        Me.cmd_modify.Enabled = False
        Me.cmd_delete.Style("display") = "none"
        Me.cmd_modify.Style("display") = "none"
    End Sub

    Private Sub SetLabelsText(ByVal text As String)
        Me.headerlbl.Text = text
        Me.srchCriLbl.Text = text
        Me.addLbl.Text = text
        Me.MyDataGrid.Columns(1).HeaderText = text
    End Sub

    Private Sub SetSearchCriteria(ByVal sc As SearchCriteria)
        txtAddGLCode.Text = ""
        Me.cmd_modify.Enabled = True
        Select Case sc
            Case SearchCriteria.GLCode
                SetLabelsText("GL Code")
                Me.cmd_modify.Visible = True
                'Me.cmd_modify.Style("display") = ""
                glDesclbl.Visible = True
                txtGLCodeDescription.Visible = True
                lblAction.Text = "Fill in the search criteria and click Search button to list the relevant GL Code(s). Click Add button to add new GL Code. Select GL Code and click Modify button to modify."

                ''Jules #2657 2013.12.09
                Me.srchCriLb2.Visible = False
                Me.txtCategoryCode.Visible = False
                ''#2657 end.

            Case SearchCriteria.CategoryCode
                SetLabelsText("Category Code")
                Me.cmd_modify.Visible = False
                'Me.cmd_modify.Style("display") = "none"
                glDesclbl.Visible = False
                txtGLCodeDescription.Visible = False
                lblAction.Text = "Fill in the search criteria and click Search button to list the relevant Category Code(s). Click Add button to add new Category Code. Select Category Code and click Modify button to modify."

                ''Jules #2657 2013.12.09
                Me.srchCriLb2.Visible = False
                Me.txtCategoryCode.Visible = False
            Case SearchCriteria.GLCodeCategoryCode
                Me.headerlbl.Text = "GL Code - Category Code"
                Me.addLbl.Text = "GL Code"
                Me.srchCriLb2.Visible = True
                Me.txtCategoryCode.Visible = True
                Me.cmd_modify.Visible = True
                Me.cmd_modify.Enabled = False
                glDesclbl.Visible = False
                lblAction.Text = "Fill in the search criteria and click Search button to list the relevant GL Code - Category Code(s). Click Add button to add new GL Code - Category Code. Select GL Code and click Modify button to modify."
                Me.divCategory.Style("display") = "none"
                ''#2657 end.

        End Select
        ViewState.Add("SearchMode", sc.ToString())
        txtGLCode.Text = ""
        'Page.DataBind()
    End Sub

    Private Sub catCodeRadioBtn_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles catCodeRadioBtn.CheckedChanged
        ViewState("Current_Val") = Me.txtAddGLCode.Text
        SetSearchCriteria(SearchCriteria.CategoryCode)

        ''Jules 2014.03.18
        Me.MyDataGrid.Style("display") = "none"
        Me.divGLCategory.Style("display") = "none"
        Me.divCategory.Style("display") = "none"
        Me.divAddGLCategory.Style("display") = "none"
        Me.cmd_modify.Enabled = False
        Me.cmd_delete.Style("display") = "none"
        Me.cmd_modify.Style("display") = "none"
    End Sub

    Private Sub cmd_modify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_modify.Click

        If Not Me.optGLCodeCategoryCode.Checked Then ''Jules 2014.03.19
            For Each row As DataGridItem In Me.MyDataGrid.Items
                Dim chk As CheckBox = row.FindControl("chkSelection")
                If chk.Checked Then
                    Me.txtAddGLCode.Text = row.Cells(1).Text
                    Me.txtGLCodeDescription.Text = row.Cells(2).Text

                    hide.Style("display") = ""
                    'Me.cmd_delete.Style("display") = ""
                    Me.txtAddGLCode.Enabled = False
                    ViewState("EditMode") = True
                    Exit For
                End If
            Next
        Else
            ''Jules 2014.03.19
            For Each row As DataGridItem In Me.dgGLCategory.Items
                Dim chk As CheckBox = row.FindControl("chkSelection")
                Dim strSql As String
                Dim i, j As Integer
                Dim objDb As New EAD.DBCom
                Dim company As String = CStr(Session("CompanyId"))
                If chk.Checked Then
                    Me.divGLCategory.Style("display") = "none"
                    Me.divAddGLCategory.Style("display") = ""
                    Me.divCategory.Style("display") = ""
                    Me.lblGLCategoryHeader.Text = "Modify GL Code - Category Code"
                    ViewState("EditMode") = True
                    Me.cmd_modify.Enabled = False
                    Me.cmd_delete.Enabled = False

                    ''Check if the Category Code is tied to the GL Code.
                    strSql = "SELECT * " &
                               " FROM COMPANY_B_GL_CODE_CATEGORY_CODE" &
                               " WHERE CBGC_B_GL_CODE = '" & row.Cells(1).Text & "' " &
                               " AND CBGC_B_COY_ID = '" & company & "' "

                    Dim ds As DataSet = objDb.FillDs(strSql)
                    If ds.Tables(0).Rows.Count > 0 Then
                        Me.txtAddGLCode2.Text = row.Cells(1).Text
                        Me.txtAddGLCode2.Enabled = False
                        Me.clearGLCategory.Disabled = True
                        For i = 0 To ds.Tables(0).Rows.Count - 1
                            For Each categoryRow As DataGridItem In Me.dgCategory.Items
                                Dim chk2 As CheckBox = categoryRow.FindControl("chkSelection")
                                If ds.Tables(0).Rows(i).Item("CBGC_B_CATEGORY_CODE") = categoryRow.Cells(1).Text Then
                                    chk2.Checked = True
                                    Exit For
                                End If
                            Next
                        Next
                    End If

                    Exit For
                End If
            Next
        End If

    End Sub

    Private Sub btncancel_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncancel.ServerClick
        Me.txtAddGLCode.Enabled = True
        Me.txtAddGLCode.Text = ""
        Me.txtGLCodeDescription.Text = ""
        Me.cmd_delete.Enabled = True
    End Sub

#Region "Enhancement 2657"
    Private Sub optGLCodeCategoryCode_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optGLCodeCategoryCode.CheckedChanged
        ViewState("Current_Val") = Me.txtAddGLCode.Text
        SetSearchCriteria(SearchCriteria.GLCodeCategoryCode)
        hide.Style("display") = "none"

        ''Jules 2014.03.18
        Me.MyDataGrid.Style("display") = "none"
        Me.divGLCategory.Style("display") = "none"
        Me.divCategory.Style("display") = "none"
        Me.divAddGLCategory.Style("display") = "none"
        Me.cmd_modify.Enabled = False
        Me.cmd_delete.Style("display") = "none"
        Me.cmd_modify.Style("display") = "none"
    End Sub

    Public Function LoadGLCategoryDataSource() As DataView
        'If ViewState("EditMode") Is Nothing Then
        Dim objDb As New EAD.DBCom
        Dim companyID As String = Session("CompanyId")
        Dim strSC As String = CStr(ViewState.Item("SearchMode"))
        Dim sc As SearchCriteria
        If strSC.Equals(SearchCriteria.GLCodeCategoryCode.ToString()) Then
            sc = SearchCriteria.GLCodeCategoryCode
        Else
            Return Nothing
        End If
        Dim sql As String = ""
        sql = "SELECT CBGC_B_GL_CODE , GROUP_CONCAT(CBGC_B_CATEGORY_CODE SEPARATOR '^') AS CBGC_B_CATEGORY_CODE " &
                    "FROM COMPANY_B_GL_CODE_CATEGORY_CODE WHERE " &
                    "CBGC_B_COY_ID='" & companyID & "' "

        If Me.txtGLCode.Text <> "" Then
            sql = sql & " AND CBGC_B_GL_CODE LIKE '%" & Me.txtGLCode.Text & "%' "
        End If

        If Me.txtCategoryCode.Text <> "" Then
            sql = "SELECT CBGC_B_GL_CODE, GROUP_CONCAT(CBGC_B_CATEGORY_CODE SEPARATOR '^') AS CBGC_B_CATEGORY_CODE " &
                    "FROM COMPANY_B_GL_CODE_CATEGORY_CODE " &
                    "WHERE CBGC_B_COY_ID='" & companyID & "'  AND CBGC_B_GL_CODE IN " &
                    "(SELECT CBGC_B_GL_CODE FROM COMPANY_B_GL_CODE_CATEGORY_CODE WHERE CBGC_B_CATEGORY_CODE LIKE '%" & Me.txtCategoryCode.Text & "%' )"
        End If

        sql = sql & "GROUP BY CBGC_B_GL_CODE"

        Dim dvViewSample As DataView
        Try
            Dim dt As DataTable = New DataTable
            dt = objDb.FillDt(sql)

            'Both search criteria( By GL Code/By category code) 's column
            ' is named as [DATA_COL], is make it easy for setting the 
            'sort expression for this column
            dt.Columns(0).ColumnName = "DATA_COL"                'column that display GL Code
            Me.dgGLCategory.Columns(1).HeaderText = "GL Code"
            dvViewSample = dt.DefaultView
            dvViewSample.Sort = "DATA_COL"
            intPageRecordCnt = dt.Rows.Count
            ViewState("intPageRecordCnt") = intPageRecordCnt

            Dim st As String = dvViewSample.Table.Columns("DATA_COL").Caption
            'IF the number of record is more then 0,
            '	enable the [modify] button
            'ELSE
            '	disable the [modify] button( no needed by user )
            If intPageRecordCnt > 0 Then
                Me.cmd_modify.Enabled = True
            Else
                Me.cmd_modify.Enabled = False
            End If

            If ViewState("SortAscending") = "no" Then
                dvViewSample.Sort += " DESC"
            End If
            If intPageRecordCnt > 0 Then
                resetDatagridPageIndex(dgGLCategory, dvViewSample)
            Else
                resetDatagridPageIndex(dgGLCategory, dvViewSample)
                If ViewState("EditMode") Is Nothing Then
                    AgoraLegacy.Common.NetMsgbox(Me, MsgNoRecord)
                End If
            End If

            'blnPaging = False
            Me.dgGLCategory.Columns(1).Visible = True

            ''Jules 2014.03.18     
            If sc = SearchCriteria.GLCodeCategoryCode Then
                Me.MyDataGrid.Style("display") = "none"

                'If intPageRecordCnt > 0 And (ViewState("EditMode") <> False Or ViewState("EditMode") Is Nothing) Then
                If intPageRecordCnt > 0 And ViewState("EditMode") Is Nothing Then
                    Me.divGLCategory.Style("display") = ""
                    Me.cmd_delete.Enabled = True
                    Me.cmd_delete.Style("display") = ""
                    Me.cmd_modify.Style("display") = ""
                ElseIf intPageRecordCnt > 0 And ViewState("EditMode") = False Then
                    Me.cmd_modify.Enabled = False
                    Me.divGLCategory.Style("display") = "none"
                Else
                    Me.divGLCategory.Style("display") = "none"
                    Me.cmd_delete.Enabled = True
                    Me.cmd_delete.Style("display") = "none"
                    Me.cmd_modify.Style("display") = "none"
                End If
            Else
                Me.MyDataGrid.Style("display") = ""
                Me.divGLCategory.Style("display") = "none"
                Me.divCategory.Style("display") = "none"
            End If
            Me.txtGLCode.Text = ""
            Me.txtCategoryCode.Text = ""
            'ViewState("EditMode") = Nothing
        Catch ex As Exception
            'process error here
            AgoraLegacy.Common.NetMsgbox(Me, "Error occur during retrieve data, please try again later")
        End Try
        Return dvViewSample
        'End If
    End Function

    Public Function LoadCategoryDataSource() As DataView
        'If ViewState("EditMode") Is Nothing Then
        Dim objDb As New EAD.DBCom
        Dim companyID As String = Session("CompanyId")
        Dim strSC As String = CStr(ViewState.Item("SearchMode"))
        Dim sc As SearchCriteria
        If strSC.Equals(SearchCriteria.GLCodeCategoryCode.ToString()) Then
            sc = SearchCriteria.GLCodeCategoryCode
        Else
            Return Nothing
        End If
        Dim sql As String = ""
        sql = "SELECT CBC_B_CATEGORY_CODE FROM COMPANY_B_CATEGORY_CODE WHERE " _
               & "CBC_B_COY_ID ='" & companyID & "'"

        Dim dvViewSample As DataView
        Try
            Dim dt As DataTable = New DataTable
            dt = objDb.FillDt(sql)

            'Both search criteria( By GL Code/By category code) 's column
            ' is named as [DATA_COL], is make it easy for setting the 
            'sort expression for this column
            dt.Columns(0).ColumnName = "DATA_COL"                'column that display GL Code/Category code            
            Me.dgCategory.Columns(1).HeaderText = "Category Code"
            dvViewSample = dt.DefaultView
            dvViewSample.Sort = "DATA_COL"
            intPageRecordCnt = dt.Rows.Count
            ViewState("intPageRecordCnt") = intPageRecordCnt

            Dim st As String = dvViewSample.Table.Columns("DATA_COL").Caption

            If ViewState("SortAscending") = "no" Then
                dvViewSample.Sort += " DESC"
            End If
            If intPageRecordCnt > 0 Then
                resetDatagridPageIndex(dgCategory, dvViewSample)
            Else
                resetDatagridPageIndex(dgCategory, dvViewSample)
                AgoraLegacy.Common.NetMsgbox(Me, MsgNoRecord)
            End If

            'blnPaging = False
            Me.dgCategory.Columns(1).Visible = True
        Catch ex As Exception
            'process error here
            AgoraLegacy.Common.NetMsgbox(Me, "Error occur during retrieve data, please try again later")
        End Try
        Return dvViewSample
        'End If
    End Function

    Public Sub dgGLCategory_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgGLCategory.PageIndexChanged
        dgGLCategory.CurrentPageIndex = e.NewPageIndex
        Bindgrid(e.NewPageIndex, True)
    End Sub

    Public Sub SortCommand2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgGLCategory.SortCommand
        Grid_SortCommand(sender, e)
        Bindgrid(dgGLCategory.CurrentPageIndex, True)
    End Sub


    Private Sub dgGLCategory_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgGLCategory.ItemCreated
        'Grid_ItemCreated_WORecCnt(dgGLCategory, e)
        Grid_ItemCreated(dgGLCategory, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAllGLCategory();")
        End If
    End Sub

    Private Sub dgGLCategory_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgGLCategory.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChildGLCategory('" & chk.ClientID & "');")
            e.Item.Cells(2).Text = Replace(e.Item.Cells(2).Text, "^", "<br>")
        End If
    End Sub

    Private Sub btnhidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhidden.Click
        Dim objDb As New EAD.DBCom
        Dim companyID As String = Session("CompanyId")
        Dim sql, strMsg As String
        Dim ds As DataSet
        sql = "SELECT * FROM COMPANY_B_GL_CODE_CATEGORY_CODE WHERE " &
              "CBGC_B_COY_ID='" & companyID & "' " &
              " AND CBGC_B_GL_CODE = '" & Me.txtAddGLCode2.Text & "' "

        ds = objDb.FillDs(sql)
        If ds.Tables(0).Rows.Count > 0 Then
            strMsg = objGlobal.GetErrorMessage("00002")
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            divGLCategory.Style("display") = "none"
            divCategory.Style("display") = ""
            divAddGLCategory.Style("display") = ""
            txtAddGLCode2.Text = ""
        End If
    End Sub

    Private Sub saveGLCategory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles saveGLCategory.Click
        'insert the new GL code-Category Code
        Dim objDb As New EAD.DBCom
        Dim objAdmin As New Admin
        Dim dgItem As DataGridItem
        Dim ds As DataSet
        Dim chk As CheckBox
        Dim i As Integer = 0         'used to iterate every rows in the table
        Dim row As Integer = 0
        Dim verifySQL As String = "", categoryCode As String = "", strSql As String = ""
        Dim strAryQuery(0) As String

        'Step 1: Get current company's ID
        Dim company As String = CStr(Session("CompanyId"))

        'determine primary keys and table name
        '====================================================

        Dim pk As String = "CBGC_B_GL_CODE"
        Dim pk2 As String = "CBGC_B_CATEGORY_CODE"
        Dim pk3 As String = "CBGC_B_COY_ID"
        Dim tablename As String = "COMPANY_B_GL_CODE_CATEGORY_CODE"
        'end determine primary keys and table name
        '====================================================

        'construct sql statement

        'Step 2: Get the GL code enter by user and prevent malicious input(sql injection)
        Dim glCode As String = AgoraLegacy.Common.Parse(Me.txtAddGLCode2.Text)

        '1 Oct 2009
        'GL Code cannot have "(" and ")"
        If glCode Is Nothing Then
            AgoraLegacy.Common.NetMsgbox(Me, "GL Code is required")
            Exit Sub
        Else
            If glCode.IndexOf("(") > -1 Or glCode.IndexOf(")") > -1 Then
                AgoraLegacy.Common.NetMsgbox(Me, MsgInvalidValue)
                Exit Sub
            Else
                verifySQL = "SELECT CBG_B_GL_CODE , CBG_B_GL_DESC FROM COMPANY_B_GL_CODE WHERE " _
                & "CBG_B_COY_ID = '" & company & "' AND CBG_B_GL_CODE = '" & Me.txtAddGLCode2.Text & "'"

                ds = objDb.FillDs(verifySQL)
                If ds.Tables(0).Rows.Count = 0 Then
                    Dim strMsg As String
                    strMsg = objGlobal.GetErrorMessage("00283")
                    strMsg = "GL Code" & strMsg
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Exit Sub
                End If
            End If
        End If

        If ViewState("EditMode") = True Then
            For Each dgItem In dgCategory.Items
                chk = dgItem.FindControl("chkSelection")
                categoryCode = dgItem.Cells(1).Text

                'Check the GL code-Category Code exist in DB or not
                verifySQL = "SELECT " & pk &
                            " FROM " & tablename &
                            " WHERE " & pk & " = '" & glCode & "' " &
                            " AND " & pk2 & " = '" & categoryCode & "'" &
                            " AND " & pk3 & " = '" & company & "'"

                Dim tDS As DataSet = objDb.FillDs(verifySQL)
                If tDS.Tables(0).Rows.Count > 0 Then
                    strSql = "DELETE FROM COMPANY_B_GL_CODE_CATEGORY_CODE " &
                               "WHERE " & pk3 & " = '" & company & "' AND " & pk & " ='" & glCode & "' " &
                               "AND " & pk2 & " ='" & categoryCode & "'"
                    If objDb.Execute(strSql) Then
                        objAdmin.InsertAuditGLCodeCategoryCode(glCode, categoryCode, "D")
                    End If
                End If

                If chk.Checked Then
                    strSql = "INSERT INTO " & tablename & "(" & pk3 & "," & pk & "," & pk2 & ")" & " VALUES('" &
                               company & "', '" & glCode & "', '" & categoryCode & "')"
                    If objDb.Execute(strSql) Then
                        objAdmin.InsertAuditGLCodeCategoryCode(glCode, categoryCode, "N")
                        i = i + 1
                    End If
                End If

            Next
            Me.btnClrGL.Value = "Clear"
            Me.txtAddGLCode.Enabled = True
            txtAddGLCode.Text = ""
            ViewState("EditMode") = Nothing

        Else
            'Check the GL code-Category Code exist in DB or not
            verifySQL = "SELECT " & pk &
                        " FROM " & tablename &
                        " WHERE " & pk & " = '" & glCode & "' " &
                        " AND " & pk2 & " = '" & categoryCode & "'" &
                        " AND " & pk3 & " = '" & company & "'"

            ds = objDb.FillDs(verifySQL)
            If ds.Tables(0).Rows.Count > 0 Then
                Dim strMsg As String
                strMsg = objGlobal.GetErrorMessage("00002")
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                Exit Sub
            End If

            For Each dgItem In dgCategory.Items

                chk = dgItem.FindControl("chkSelection")
                categoryCode = dgItem.Cells(1).Text

                If chk.Checked Then
                    'Step 3: Check the GL code-Category Code exist in DB or not
                    verifySQL = "SELECT " & pk &
                                " FROM " & tablename &
                                " WHERE " & pk & " = '" & glCode & "' " &
                                " AND " & pk2 & " = '" & categoryCode & "'" &
                                " AND " & pk3 & " = '" & company & "'"

                    Dim tDS As DataSet = objDb.FillDs(verifySQL)
                    If tDS.Tables(0).Rows.Count > 0 Then
                        'ignore if exists
                    Else
                        'add to database
                        strSql = "INSERT INTO " & tablename & "(" & pk3 & "," & pk & "," & pk2 & ")" & " VALUES('" &
                            company & "', '" & glCode & "', '" & categoryCode & "')"
                        If objDb.Execute(strSql) Then
                            objAdmin.InsertAuditGLCodeCategoryCode(glCode, categoryCode, "N")
                            i = i + 1
                        End If
                    End If
                End If

            Next
        End If

        If i > 0 Then
            hide.Style("display") = ""
            AgoraLegacy.Common.NetMsgbox(Me, MsgRecordSave)
        Else
            AgoraLegacy.Common.NetMsgbox(Me, MsgRecordNotSave)
        End If
        ViewState("Current_Val") = ""
        ViewState("EditMode") = Nothing
        Page.DataBind()
        hide.Style("display") = "none"
        Me.divGLCategory.Style("display") = ""
        Me.divAddGLCategory.Style("display") = "none"
        Me.divCategory.Style("display") = "none"
    End Sub

    Private Sub btnhidden2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhidden2.Click
        lblGLCategoryHeader.Text = "Add GL Code - Category Code"
        txtAddGLCode2.Enabled = True
        Me.divAddGLCategory.Style("display") = ""
        Me.divCategory.Style("display") = ""
        Me.divGLCategory.Style("display") = "none"
        Me.hide.Style("display") = "none"
        Me.cmd_modify.Enabled = False
        Me.cmd_delete.Enabled = False
        Me.clearGLCategory.Disabled = False
        Me.txtAddGLCode2.Text = ""
        ViewState("EditMode") = False
        Page.DataBind()
    End Sub



    Public Function DeleteGLCodeCategoryCode() As Boolean
        Dim objAdmin As New Admin
        Dim strAryQuery(0) As String
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim glCode As String
        Dim i As Integer = 0         'used to iterate every rows in the table
        Dim row As Integer = 0
        Dim objDb As New EAD.DBCom
        Dim companyID As String = CStr(Session("CompanyId"))
        Dim tablename As String = "COMPANY_B_GL_CODE_CATEGORY_CODE"
        Dim deleteCol As String = "CBGC_B_GL_CODE"
        Dim deleteCol2 As String = "CBGC_B_CATEGORY_CODE"        
        Dim companyIDCol As String = "CBGC_B_COY_ID"
        Dim strCategoryCode As String = ""
        Dim errorMsg As String
        Dim blnFound As Boolean = False
        Dim deletedRows As Integer = 0       'store number of rows was removed

        For Each dgItem In dgGLCategory.Items

            chk = dgItem.FindControl("chkSelection")
            glCode = dgGLCategory.DataKeys.Item(i)

            If chk.Checked Then
                glCode = AgoraLegacy.Common.Parse(glCode)

                Dim strSql As String = "SELECT CBGC_B_GL_CODE , CBGC_B_CATEGORY_CODE, CBGC_B_COY_ID " & _
                "FROM COMPANY_B_GL_CODE_CATEGORY_CODE WHERE " & _
                companyIDCol & " = '" & companyID & "' AND " & deleteCol & " = '" & glCode & "'"

                Dim tDS As DataSet = objDb.FillDs(strSql)
                If tDS.Tables(0).Rows.Count > 0 Then
                    For row = 0 To tDS.Tables(0).Rows.Count - 1
                        strSql = "DELETE FROM " & tablename & _
                                " WHERE " & companyIDCol & " = '" & companyID & "' AND " & deleteCol & " ='" & glCode & "' " & _
                                "AND " & deleteCol2 & " ='" & tDS.Tables(0).Rows(row).Item(1) & "'"
                        If objDb.Execute(strSql) Then
                            objAdmin.InsertAuditGLCodeCategoryCode(glCode, tDS.Tables(0).Rows(row).Item(1), "D")
                            deletedRows = deletedRows + 1
                        End If
                    Next
                End If

            End If

            i = i + 1
        Next

        If deletedRows > 0 Then
            AgoraLegacy.Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        Else
            errorMsg = "Unable to delete"
            AgoraLegacy.Common.NetMsgbox(Me, errorMsg, MsgBoxStyle.Information)
        End If

    End Function

    Private Sub dgCategory_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgCategory.ItemCreated
        'Grid_ItemCreated_WORecCnt(dgCategory, e)
        Grid_ItemCreated(dgCategory, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAllCategory();")
        End If
    End Sub

    Private Sub dgCategory_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgCategory.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChildCategory('" & chk.ClientID & "');")
        End If
    End Sub

    Private Sub btnhidden3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhidden3.Click
        Me.cmd_delete.Enabled = True
        Me.divAddGLCategory.Style("display") = "none"
        Me.divCategory.Style("display") = "none"
    End Sub

    Public Sub dgCategory_SortCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgCategory.SortCommand
        Grid_SortCommand(sender, e)
        Bindgrid(dgCategory.CurrentPageIndex, True)
    End Sub
#End Region
End Class
