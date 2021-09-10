Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions
Imports Microsoft.Web.UI.WebControls
Public Class bcmAccSetup
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents eccEdit As New System.Web.UI.WebControls.EditCommandColumn
    Protected WithEvents btnColDelete As New System.Web.UI.WebControls.ButtonColumn
    Protected WithEvents btnColDelete2 As New System.Web.UI.WebControls.ButtonColumn
    Protected WithEvents btnColDelete3 As New System.Web.UI.WebControls.ButtonColumn
    Protected WithEvents eccModify As New System.Web.UI.WebControls.EditCommandColumn
    Protected WithEvents eccModify2 As New System.Web.UI.WebControls.EditCommandColumn


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents lbl_result As System.Web.UI.WebControls.Label
    Protected WithEvents lblDeptName As System.Web.UI.WebControls.Label
    Protected WithEvents dgDept As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dgAccCode As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dgSubAccCode As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdProjCode As System.Web.UI.WebControls.Button
    Protected WithEvents cmdsearchSub As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSearchAccName As System.Web.UI.WebControls.Button
    Protected WithEvents dgProjCode As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Parent0 As System.Web.UI.WebControls.Label
    Protected WithEvents Parent1 As System.Web.UI.WebControls.Label
    Protected WithEvents Parent2 As System.Web.UI.WebControls.Label
    Protected WithEvents tsHoriz As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents mpHoriz As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents txtAccCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSubAccCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtProjCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents lblAccCode As System.Web.UI.WebControls.Label
    Protected WithEvents txtRowCnt0 As System.Web.UI.WebControls.Label
    Protected WithEvents txtRowCnt1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtRowCnt2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblBookmarkLabel As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblInit As System.Web.UI.WebControls.Label
    Protected WithEvents lblSubAcc As System.Web.UI.WebControls.Label
    Protected WithEvents lblIBAcc As System.Web.UI.WebControls.Label
    Protected WithEvents lblIBProj As System.Web.UI.WebControls.Label
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object


    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim ds As New DataSet



    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        blnFooter = False
        SetGridProperty(dgAccCode)
        SetGridProperty(dgSubAccCode)
        SetGridProperty(dgProjCode)

        viewstate("PageId") = Request.Params("pageid")
        viewstate("DeptId") = Request.Params("deptindex")
        viewstate("DeptName") = Request.Params("deptname")
        IsAuthentication()


        If Not Page.IsPostBack Then
            Populate()
            Bindgrid(dgAccCode, 1)
        End If

        lnkBack.NavigateUrl = dDispatcher.direct("BCM", "bcmSearchAccSetup.aspx", "pageid=" & strPageId)
        'btnColDelete.Text = "<IMG src=""dDispatcher.direct(""Plugins/images"", ""i_delete2.gif"")""width =17px height =17px border=0 alt='Delete'>"
        'btnColDelete2.Text = "&lt;IMG src=""dDispatcher.direct(""Plugins/images"", ""i_delete2.gif"")""width =17px height =17px border=0 alt='Delete'&gt;"
        'btnColDelete3.Text = "&lt;IMG src=""dDispatcher.direct(""Plugins/images"", ""i_delete2.gif"")""width =17px height =17px border=0 alt='Delete'&gt;"
        'eccEdit.EditText = "&lt;IMG src=""dDispatcher.direct(""Plugins/images"", ""i_edit.gif"")""width =17px height =17px border=0 alt='Modify'&gt;"
        'eccEdit.UpdateText = "&lt;IMG src=""dDispatcher.direct(""Plugins/images"", ""i_save.gif"")""width =17px height =17px border=0 alt='Save'&gt;"
        'eccModify.EditText = "&lt;IMG src=""dDispatcher.direct(""Plugins/images"", ""i_edit.gif"")""width =17px height =17px border=0 alt='Modify'&gt;"
        'eccModify.UpdateText = "&lt;IMG src=""dDispatcher.direct(""Plugins/images"", ""i_save.gif"")""width =17px height =17px border=0 alt='Save'&gt;"
        'eccModify2.EditText = "&lt;IMG src=""dDispatcher.direct(""Plugins/images"", ""i_edit.gif"")""width =17px height =17px border=0 alt='Modify'&gt;"
        'eccModify2.UpdateText = "&lt;IMG src=""dDispatcher.direct(""Plugins/images"", ""i_save.gif"")""width =17px height =17px border=0 alt='Save'&gt;"


    End Sub

    Private Sub Populate()
        Dim strSQL As String
        Dim objBCM As New BudgetControl
        lblDeptName.Text = ViewState("DeptName")
        'Zulham 20022019
        Dim objDB As New EAD.DBCom
        lblInit.Text = objBCM.GetDeptTotalInitBudget(ViewState("DeptId"))
        If lblInit.Text = "0.00" Then
            Dim idx = objDB.GetVal("SELECT c1.cdm_dept_index 
                                    FROM company_dept_mstr c1, company_dept_mstr c2 
                                    WHERE c1.cdm_dept_name = '" & lblDeptName.Text & "' 
                                    AND c1.cdm_dept_name = c2.cdm_dept_name
                                    AND c1.cdm_dept_code = c2.cdm_dept_code
                                    AND c1.cdm_approval_grp_index IS NULL
                                    AND c2.cdm_dept_index = '" & ViewState("DeptId") & "' 
                                    UNION ALL 
                                    SELECT c1.cdm_dept_index 
                                    FROM company_dept_mstr c1, company_dept_mstr c2 
                                    WHERE c1.cdm_dept_name = '" & lblDeptName.Text & "' 
                                    AND c1.cdm_dept_name = c2.cdm_dept_name
                                    AND c1.cdm_dept_code = c2.cdm_dept_code
                                    AND c2.cdm_dept_index = '" & ViewState("DeptId") & "'")
            ViewState("idx") = idx
            lblInit.Text = objBCM.GetDeptTotalInitBudget(idx)
        End If
        objBCM = Nothing
    End Sub


    Private Function Bindgrid(ByVal pDg As DataGrid, ByVal pLevel As Integer, Optional ByVal pSorted As Boolean = False) As String
        Dim strAccCode As String
        Dim strSubAccCode As String
        Dim strProjCode As String

        strAccCode = txtAccCode.Text
        strSubAccCode = txtSubAccCode.Text
        strProjCode = txtProjCode.Text

        Dim objBCM As New BudgetControl

        '//Retrieve Data from Database
        'Dim ds As DataSet = New DataSet
        If pDg.ID = "dgAccCode" Then
            ds = objBCM.SearcAccMstr(ViewState("DeptId"), pLevel, strAccCode)
        ElseIf pDg.ID = "dgSubAccCode" Then
            ds = objBCM.SearcAccMstr(viewstate("DeptId"), pLevel, strSubAccCode, Parent1.Text)
        Else
            ds = objBCM.SearcAccMstr(viewstate("DeptId"), pLevel, strProjCode, Parent2.Text)
        End If

        'Zulham 19072018 - PAMB
        If ds.Tables(0).Rows.Count = 0 Then
            If Not ViewState("idx") Is Nothing Then
                If pDg.ID = "dgAccCode" Then
                    ds = objBCM.SearcAccMstr(ViewState("idx"), pLevel, strAccCode)
                ElseIf pDg.ID = "dgSubAccCode" Then
                    ds = objBCM.SearcAccMstr(ViewState("idx"), pLevel, strSubAccCode, Parent1.Text)
                Else
                    ds = objBCM.SearcAccMstr(ViewState("idx"), pLevel, strProjCode, Parent2.Text)
                End If
            End If
        End If
        'End

        'Dim dr As DataRow
        'dr = ds.Tables(0).NewRow
        'dr(0) = "NEW"
        'dr(1) = ""
        'dr(2) = "0"
        'ds.Tables(0).Rows.Add(dr)

        '//for sorting asc or desc
        Dim dvViewDept As DataView
        dvViewDept = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)
        If pSorted Then
            dvViewDept.Sort = viewstate("SortExpression")
            If viewstate("SortAscending") = "yes" Then dvViewDept.Sort += " DESC"
        End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If pDg.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod pDg.PageSize = 0 Then
                pDg.CurrentPageIndex = pDg.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        'If intPageRecordCnt > 0 Then

        If viewstate("UpdateCommand") <> True Then resetDatagridPageIndex(pDg, dvViewDept)

        '//datagrid.pageCount only got value after databind

        'If intPageRecordCnt > 0 Then
        'intTotPage = dtgDept.PageCount
        '//mean Enable, can't use button.Enabled because this is a HTML button
        pDg.DataSource = dvViewDept
        pDg.DataBind()

        'Else
        'dtgDept.DataSource = ""
        'pDg.DataBind()
        'Common.NetMsgbox(Me, MsgNoRecord)
        'intTotPage = 0
        'End If
        ' add for above checking
        viewstate("PageCount") = pDg.PageCount
    End Function



    Public Sub ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        Dim dg As DataGrid = CType(source, DataGrid)
        If e.CommandName = "Go" Then
            Select Case dg.ID
                Case "dgAccCode"
                    Parent1.Text = CType(e.Item.Cells(0).Controls(0), LinkButton).Text
                    lblAccCode.Text = CType(e.Item.Cells(0).Controls(0), LinkButton).Text
                    lblIBAcc.Text = e.Item.Cells(2).Text
                    lblSubAcc.Text = "-"
                    lblIBProj.Text = "-"
                    ClearEditCommand()
                    tsHoriz.Items(2).Enabled = True
                    tsHoriz.Items(4).Enabled = False
                    tsHoriz.SelectedIndex = 1
                    GridRefresh()

                Case "dgSubAccCode"
                    Parent2.Text = CType(e.Item.Cells(0).Controls(0), LinkButton).Text
                    lblSubAcc.Text = CType(e.Item.Cells(0).Controls(0), LinkButton).Text
                    lblIBProj.Text = e.Item.Cells(2).Text
                    ClearEditCommand()
                    tsHoriz.Items(4).Enabled = True
                    tsHoriz.SelectedIndex = 2
                    GridRefresh()

                Case "dgProjCode"
                    ClearEditCommand()
                    GridRefresh()

            End Select
            'lblBookmarkLabel.Visible = True
        ElseIf e.CommandName = "AddNew" Then
            AddNew(source, e)
        End If
    End Sub

    Private Sub GridRefresh()
        Bindgrid(dgProjCode, 3)
        Bindgrid(dgSubAccCode, 2)
        Bindgrid(dgAccCode, 1)
    End Sub

    Private Sub AddNew(ByVal source As DataGrid, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        Dim strCode, strDesc, strBudget As String
        Dim iLevel As Integer = GetLevel(source)
        Dim strParent As String
        If iLevel = 1 Then
            strParent = ""
        ElseIf iLevel = 2 Then
            strParent = Parent1.Text
        ElseIf iLevel = 3 Then
            strParent = Parent2.Text
        End If
        strCode = CType(e.Item.Cells(0).Controls(0), TextBox).Text
        strDesc = CType(e.Item.Cells(1).Controls(0), TextBox).Text
        strBudget = CType(e.Item.Cells(2).Controls(0), TextBox).Text

        source.EditItemIndex = -1
        If ValidateFields(True, strCode, strBudget, iLevel, strParent) Then
            Dim objBCM As New BudgetControl
            If objBCM.AddAccount(strCode, strDesc, strBudget, viewstate("DeptId"), iLevel, strParent) Then
                Common.NetMsgbox(Me, objBCM.Message, MsgBoxStyle.Information)
            End If
        End If
        GridRefresh()
        Populate()

    End Sub

    Private Function GetLevel(ByVal source As DataGrid) As Integer
        Select Case CType(source, DataGrid).ID
            Case "dgAccCode"
                Return 1
            Case "dgSubAccCode"
                Return 2
            Case "dgProjCode"
                Return 3
        End Select
    End Function

    Private Function ValidateFields(ByVal pAddAction As Boolean, ByVal pCode As String, ByVal pBudget As String, ByVal pLevel As Integer, ByVal pParent As String) As Boolean
        Dim objBCM As New BudgetControl
        Dim bTrue As Boolean = True

        If pCode = "" Then
            Common.NetMsgbox(Me, "Code is required.", MsgBoxStyle.Information)
            Return False
        End If

        If pAddAction Then
            If objBCM.IsExist(pCode, viewstate("DeptId"), pLevel, pParent) Then
                Common.NetMsgbox(Me, "Duplicate record found.", MsgBoxStyle.Information)
                Return False
            End If
        End If

        If pBudget = "" Then
            Common.NetMsgbox(Me, "Initial Budget is required.", MsgBoxStyle.Information, "Wheel")
            Return False
        End If

        If pBudget <> "" And Not IsNumeric(pBudget) Then
            Common.NetMsgbox(Me, "Initial Budget is expecting numeric value.", MsgBoxStyle.Information)
            Return False
        End If

        If pBudget <> "" And pBudget < 0 Then
            Common.NetMsgbox(Me, "Initial Budget is expecting non negatif value.", MsgBoxStyle.Information)
            Return False
        End If

        If pBudget <> "" And IsCost(pBudget) = False Then
            Common.NetMsgbox(Me, "Invalid initial budget.", MsgBoxStyle.Information)
            Return False

        End If


        If pLevel > 1 Then
            If pBudget <> "" And Not objBCM.IsWithinBudget(pAddAction, pCode, pParent, pBudget, viewstate("DeptId"), pLevel) Then
                Common.NetMsgbox(Me, "Invalid initial budget. ""&vbCrLf&""Initial budget is over limit.", MsgBoxStyle.Information)
                Return False
            End If
        End If

        If pLevel < 3 Then
            If pBudget <> "" And Not objBCM.IsBudgetGreater(pCode, pBudget, viewstate("DeptId"), pLevel) Then
                Common.NetMsgbox(Me, "Invalid initial budget.""&vbCrLf&""Initial budget is lower limit.", MsgBoxStyle.Information)
                Return False
            End If
        End If

        Return True
    End Function
    Sub AddRow(ByVal pSource As DataGrid)
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer

        intTotalCol = 4

        For intL = 0 To intTotalCol
            addCell(row)
        Next

        Dim txtAccCode As New TextBox

        With txtAccCode
            .CssClass = "txtbox"
            .ID = "txtCode"
            .Width = Unit.Pixel(140)
            .MaxLength = 30
            '.Text = Trim("Enter for new record")
            'CellCode.Controls.Add(txtAccCode)
            row.Cells(0).Controls.Add(txtAccCode)
        End With

        Dim txtDesc As New TextBox

        With txtDesc
            .CssClass = "txtbox"
            .ID = "txtDesc"
            .Width = Unit.Pixel(300)
            .MaxLength = 100
            row.Cells(1).Controls.Add(txtDesc)
            'CellBud.Controls.Add(txtDesc)
        End With

        Dim txtBud As New TextBox
        With txtBud
            .CssClass = "txtbox"
            .ID = "txtBud"
            .Width = Unit.Pixel(120)
            row.Cells(2).Controls.Add(txtBud)
            'CellBud.Controls.Add(txtDesc)
        End With

        Dim bcAddNew As New ImageButton
        With bcAddNew
            .ImageUrl = dDispatcher.direct("Plugins/images", "i_save.gif")
            .ToolTip = "Add New"
            .Width = Unit.Pixel(17)
            .Height = Unit.Pixel(17)
            .CommandName = "AddNew"
            row.Cells(3).Style.Item("text-Align") = "Center"
            row.Cells(3).Controls.Add(bcAddNew)
        End With

        row.CssClass = "TableCol"
        row.Visible = blnCanAdd
        pSource.Controls(0).Controls.Add(row)
    End Sub


    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub

    Public Sub ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        Grid_ItemCreated(sender, e)
        If e.Item.ItemType = ListItemType.Footer Then
            AddRow(sender)
        End If
    End Sub


    Private Sub ClearEditCommand()
        dgAccCode.EditItemIndex = -1
        dgSubAccCode.EditItemIndex = -1
        dgProjCode.EditItemIndex = -1
    End Sub

    Private Sub CheckButtonRight(ByVal e)
        Dim bcUpdate As LinkButton
        Dim bcDelete As LinkButton
        bcUpdate = CType(e.Item.Cells(3).Controls(0), LinkButton)
        bcDelete = CType(e.Item.Cells(4).Controls(0), LinkButton)

        If blnCanUpdate = False Then
            bcUpdate.Enabled = False
            'Dim img As New ImageButton
            'img.ImageUrl = "../images/i_editdisable.gif"
            'img.Width = Unit.Pixel(17)
            'img.Height = Unit.Pixel(17)
            'img.Enabled = False
            'img.ToolTip = "Insufficient right"
            'e.Item.Cells(3).Style.Item("text-Align") = "Center"
            'e.Item.Cells(3).Controls.Add(img)
        End If

        If blnCanDelete = False Then
            bcDelete.Enabled = False

            'Dim img As New ImageButton
            'img.ImageUrl = "../images/i_editdisable.gif"
            'img.Width = Unit.Pixel(17)
            'img.Height = Unit.Pixel(17)
            'img.Enabled = False
            'img.ToolTip = "Insufficient right"
            'e.Item.Cells(4).Style.Item("text-Align") = "Center"
            'e.Item.Cells(4).Controls.Add(img)
        Else
            bcDelete.Attributes.Add("Onclick", "return CustomMsg('deleteUA');")
        End If

    End Sub
    Public Sub EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        Dim dg As DataGrid = CType(source, DataGrid)
        Dim iLevel As Integer = GetLevel(source)
        ClearEditCommand()
        dg.EditItemIndex = CInt(e.Item.ItemIndex)
        viewstate("UpdateCommand") = True
        GridRefresh()
        viewstate("UpdateCommand") = Nothing
        CheckButtonRight(e)

        Dim txtDesc As TextBox
        Dim txtBudget As TextBox
        txtDesc = CType(dg.Items(dg.EditItemIndex).Cells(1).Controls(0), TextBox)
        txtDesc.CssClass = "txtbox"
        txtDesc.Width = Unit.Pixel(300)
        txtDesc.MaxLength = 100
        txtBudget = CType(dg.Items(dg.EditItemIndex).Cells(2).Controls(0), TextBox)
        txtBudget.Text = Replace(txtBudget.Text, ",", "")
        txtBudget.CssClass = "txtbox"
        txtBudget.Width = Unit.Pixel(120)
    End Sub

    Public Sub UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        Dim dg As DataGrid = CType(source, DataGrid)
        Dim iLevel As Integer = GetLevel(source)
        Dim strParent As String
        If iLevel = 1 Then
            strParent = ""
        ElseIf iLevel = 2 Then
            strParent = Parent1.Text
        ElseIf iLevel = 3 Then
            strParent = Parent2.Text
        End If

        Dim strCode, strDesc, strBudget As String
        If iLevel = 3 Then
            strCode = CType(e.Item.Cells(0).Text, String)
        Else
            strCode = CType(e.Item.Cells(0).Controls(0), LinkButton).Text
        End If

        strDesc = CType(e.Item.Cells(1).Controls(0), TextBox).Text
        strBudget = CType(e.Item.Cells(2).Controls(0), TextBox).Text

        If ValidateFields(False, strCode, strBudget, iLevel, strParent) Then
            Dim objBCM As New BudgetControl
            If objBCM.SaveAccount(strCode, strDesc, strBudget, iLevel, viewstate("DeptId")) Then
                CType(source, DataGrid).EditItemIndex = -1
            End If
            viewstate("UpdateCommand") = True
            GridRefresh()
            viewstate("UpdateCommand") = Nothing
            Common.NetMsgbox(Me, objBCM.Message, MsgBoxStyle.Information)
        End If
        Populate()
    End Sub


    Public Sub DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        Dim dg As DataGrid = CType(source, DataGrid)
        Dim iLevel As Integer = GetLevel(source)

        Dim strCode, strDesc, strBudget As String
        If iLevel = 3 Then
            strCode = CType(e.Item.Cells(0).Text, String)
        Else
            strCode = CType(e.Item.Cells(0).Controls(0), LinkButton).Text
        End If


        CType(source, DataGrid).EditItemIndex = -1
        Dim objBCM As New BudgetControl
        If objBCM.DeleteBCMAccount(strCode, ViewState("DeptId")) Then
            ViewState("action") = "del"
            Bindgrid(dg, iLevel)
        End If
        Populate()
        GridRefresh()
        Common.NetMsgbox(Me, objBCM.Message, MsgBoxStyle.Information)

    End Sub

    Public Sub SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Dim iLevel As Integer = GetLevel(source)
        Grid_SortCommand(source, e)
        source.CurrentPageIndex = 0
        GridRefresh()
        Bindgrid(source, iLevel, True)

    End Sub

    Public Sub PageIndex(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        sender.EditItemIndex = -1
        Dim iLevel As Integer = GetLevel(sender)
        Bindgrid(sender, iLevel)
    End Sub


    Public Sub ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(3).ColumnSpan = 2
            e.Item.Cells(4).Visible = False
            e.Item.Cells(3).Text = "Function"
            e.Item.Cells(3).HorizontalAlign = HorizontalAlign.Left
        ElseIf e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            CheckButtonRight(e)
        End If

    End Sub




    Private Sub cmdSearchAccName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearchAccName.Click
        Bindgrid(dgAccCode, 1)
    End Sub

    Private Sub cmdsearchSub_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdsearchSub.Click
        Bindgrid(dgSubAccCode, 2)
    End Sub

    Private Sub cmdProjCode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdProjCode.Click
        Bindgrid(dgProjCode, 3)
    End Sub


    Private Sub tsHoriz_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsHoriz.SelectedIndexChange

    End Sub

    Private Sub mpHoriz_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles mpHoriz.SelectedIndexChange

    End Sub
End Class
