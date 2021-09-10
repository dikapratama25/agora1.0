
Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data

Public Class GLCodeFTN
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

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
        GLCode        'used as default search
        CategoryCode
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)

        If Not Page.IsPostBack Then
            MyBase.SetGridProperty(Me.MyDataGrid)
            Me.cmd_delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
            Me.saveGL.Attributes.Add("onclick", "return ValidateGLCode();")
            Me.cmd_modify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
            Me.glRadioBtn.Checked = True
            ViewState.Item("SearchMode") = SearchCriteria.GLCode             'default search mode

        End If

    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
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
        Me.MyDataGrid.Columns(1).Visible = False
    End Function

    Private Sub MyDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles MyDataGrid.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
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

        Dim sc As SearchCriteria = SearchCriteria.GLCode
        If strSC.Equals(SearchCriteria.CategoryCode.ToString()) Then
            sc = SearchCriteria.CategoryCode
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


        Dim dvViewSample As DataView
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
            Me.MyDataGrid.Columns(1).Visible = False
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
        ViewState("EditMode") = False

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
            checkSQL = "Select CBC_B_GL_CODE " & _
            "FROM COMPANY_B_ITEM_CODE " & _
            "INNER JOIN PRODUCT_MSTR " & _
            "ON CBC_PRODUCT_CODE = PM_PRODUCT_CODE " & _
            "AND CBC_B_COY_ID = '" & companyID & "' " & _
            "WHERE ( LENGTH(CBC_B_GL_CODE) > 0 )  " & _
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
                    Dim sql As String = "select CONCAT(pod_po_no , pod_coy_id , CONVERT(pod_po_line,CHAR)) " & _
                                       "FROM po_mstr, po_details WHERE pod_coy_id = '" & companyID & "' and " & col & " = '" & glCode & "' AND pod_po_no = pom_po_no AND " & _
                                       "pod_coy_id = pom_b_coy_id AND pom_po_status = '1'"
                    Dim tDS As DataSet = objDb.FillDs(sql)
                    If tDS.Tables(0).Rows.Count > 0 Then
                        For row = 0 To tDS.Tables(0).Rows.Count - 1
                            sql = "UPDATE po_details SET " & col & " = '' " & _
                                    "WHERE pod_coy_id = '" & companyID & "' AND " & col & " = '" & glCode & "' AND " & _
                                    "(CONCAT(pod_po_no , pod_coy_id , CONVERT(pod_po_line,CHAR)) = '" & tDS.Tables(0).Rows(row).Item(0) & "')"
                            Common.Insert2Ary(strAryQuery, sql)
                        Next
                    End If
                    sql = "DELETE FROM " & tablename & " WHERE " & _
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
                glDesclbl.Visible = True
                txtGLCodeDescription.Visible = True
                lblAction.Text = "Fill in the search criteria and click Search button to list the relevant GL Code(s). Click Add button to add new GL Code. Select GL Code and click Modify button to modify."

            Case SearchCriteria.CategoryCode
                SetLabelsText("Category Code")
                Me.cmd_modify.Visible = False
                glDesclbl.Visible = False
                txtGLCodeDescription.Visible = False
                lblAction.Text = "Fill in the search criteria and click Search button to list the relevant Category Code(s). Click Add button to add new Category Code. Select Category Code and click Modify button to modify."

        End Select
        ViewState.Add("SearchMode", sc.ToString())
        txtGLCode.Text = ""
        Page.DataBind()
    End Sub

    Private Sub catCodeRadioBtn_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles catCodeRadioBtn.CheckedChanged
        ViewState("Current_Val") = Me.txtAddGLCode.Text
        SetSearchCriteria(SearchCriteria.CategoryCode)
    End Sub

    Private Sub cmd_modify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_modify.Click

        For Each row As DataGridItem In Me.MyDataGrid.Items
            Dim chk As CheckBox = row.FindControl("chkSelection")
            If chk.Checked Then
                Me.txtAddGLCode.Text = row.Cells(1).Text
                Me.txtGLCodeDescription.Text = row.Cells(2).Text

                hide.Style("display") = ""
                Me.cmd_delete.Style("display") = ""
                Me.txtAddGLCode.Enabled = False
                ViewState("EditMode") = True
                Exit For
            End If
        Next

    End Sub

    Private Sub btncancel_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncancel.ServerClick
        Me.txtAddGLCode.Enabled = True
        Me.txtAddGLCode.Text = ""
        Me.txtGLCodeDescription.Text = ""
    End Sub
End Class
