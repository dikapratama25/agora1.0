Imports AgoraLegacy
Imports eProcure.Component
Public Class bcmSearchBuyerAss
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents btnColEdit As New System.Web.UI.WebControls.ButtonColumn
    Protected WithEvents btnColDelete As New System.Web.UI.WebControls.ButtonColumn
    Protected WithEvents btnColEdit2 As New System.Web.UI.WebControls.ButtonColumn
    Protected WithEvents btnColDelete2 As New System.Web.UI.WebControls.ButtonColumn


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtDeptName As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents lbl_result As System.Web.UI.WebControls.Label
    Protected WithEvents dgDept As System.Web.UI.WebControls.DataGrid
    Protected WithEvents rdUser As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rdAccountCode As System.Web.UI.WebControls.RadioButton
    Protected WithEvents dgAccCode As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Textbox3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUserName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUserID As System.Web.UI.WebControls.TextBox
    Protected WithEvents tbUser As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents trUser As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents txtAccCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents tbAccCode As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents cmdSearchUser As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSearchAccCode As System.Web.UI.WebControls.Button
    Protected WithEvents Acc As System.Web.UI.HtmlControls.HtmlTableRow

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
        blnSorting = False
        SetGridProperty(dgDept)
        SetGridProperty(dgAccCode)
        IsAuthentication()

        viewstate("Mode") = Request.Params("mode")

        If Not Page.IsPostBack Then
            If viewstate("Mode") = "User" Then
                rdUser.Checked = True
            ElseIf viewstate("Mode") = "Acc" Then
                rdAccountCode.Checked = True
                tbAccCode.Style.Item("Display") = ""
                tbUser.Style.Item("Display") = "none"
            End If
        End If

        'btnColDelete.Text = "&lt;IMG src=""dDispatcher.direct(""Plugins/images"", ""i_delete2.gif"")""width =17px height =17px border=0 alt='Remove'&gt;"
        'btnColEdit.Text = "&lt;IMG src=""dDispatcher.direct(""Plugins/images"", ""i_edit.gif"")""width =17px height =17px border=0 alt='modify'&gt;"
        'btnColDelete2.Text = "&lt;IMG src=""dDispatcher.direct(""Plugins/images"", ""i_delete2.gif"")""width =17px height =17px border=0 alt='Remove'&gt;"
        'btnColEdit2.Text = "&lt;IMG src=""dDispatcher.direct(""Plugins/images"", ""i_edit.gif"")""width =17px height =17px border=0 alt='modify'&gt;"
    End Sub

    Private Sub Search(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearchUser.Click, cmdSearchAccCode.click
        dgDept.CurrentPageIndex = 0
        dgAccCode.CurrentPageIndex = 0
        viewstate("duplic") = ""
        If rdUser.Checked = True Then
            Bindgrid(dgDept)
            dgAccCode.DataBind()
            Acc.Visible = False
        Else
            Bindgrid(dgAccCode)
            Acc.Visible = True
            dgDept.DataBind()
        End If
    End Sub

    Private Function Filter(ByRef pDt As DataTable, ByVal pKey As String) As DataTable
        Dim dt As DataTable
        Dim sSelect As String = "ACCT_LIST" & Common.ParseSQL(pKey)
        dt = pDt.Clone
        For Each row As DataRow In pDt.Select(sSelect, "", DataViewRowState.CurrentRows)
            dt.ImportRow(row)
        Next
        Return dt
    End Function

    Private Function Bindgrid(ByVal source As DataGrid, Optional ByVal pSorted As Boolean = False) As String
        Dim strDeptName As String
        Dim strUserName As String
        Dim strUserID As String
        Dim strAccCode As String

        strDeptName = txtDeptName.Text
        strUserName = txtUserName.Text
        strUserID = txtUserID.Text
        strAccCode = txtAccCode.Text()

        Dim objBCM As New BudgetControl

        '//Retrieve Data from Database
        Dim ds As DataSet = New DataSet
        Dim dvViewDept As DataView

        If rdUser.Checked Then
            ds = objBCM.SearchAccUserByUser(strDeptName, strUserName, strUserID)
            dvViewDept = ds.Tables(0).DefaultView
            intPageRecordCnt = ds.Tables(0).Rows.Count
        Else
            'Dim objBCM As New BudgetControl
            Dim dt As New DataTable
            dt = objBCM.getBCMListByCompanyNew()
            If strAccCode <> "" Then
                dt = Filter(dt, strAccCode)
            End If
            dvViewDept = dt.DefaultView
            intPageRecordCnt = dt.Rows.Count
        End If

        ''//for sorting asc or desc

        ''dvViewSample(0)(0)
        ''If pSorted Then
        'dvViewDept.Sort = viewstate("SortExpression")
        'If viewstate("SortAscending") = "no" Then dvViewDept.Sort += " DESC"
        ''End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        'If viewstate("action") = "del" Then
        '    If source.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod source.PageSize = 0 Then
        '        source.CurrentPageIndex = source.CurrentPageIndex - 1
        '        viewstate("action") = ""
        '    End If
        'End If

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(source, dvViewDept)
            '//mean Enable, can't use button.Enabled because this is a HTML button
            source.DataSource = dvViewDept
            source.DataBind()
        Else
            'dtgDept.DataSource = ""
            source.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            'intTotPage = 0
        End If
        ' add for above checking
        viewstate("PageCount") = source.PageCount
        'ShowStats()
    End Function



    Private Sub dgAccCode_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgAccCode.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(6).ColumnSpan = 2
            e.Item.Cells(7).Visible = False
            e.Item.Cells(6).Text = "Function"
        ElseIf e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            CheckButtonRight(e)
            Dim rptDept As Repeater
            Dim rptDeptIndex As Repeater
            Dim rptUserName As Repeater
            Dim rptUserID As Repeater
            Dim objBCM As New BudgetControl

            rptDept = e.Item.FindControl("rptDept")
            rptDeptIndex = e.Item.FindControl("rptDeptIndex")
            rptUserName = e.Item.FindControl("rptUserName")
            rptUserID = e.Item.FindControl("rptUserID")

            rptDept.DataSource = objBCM.GetAccDept(e.Item.Cells(1).Text)
            rptDept.DataBind()
            rptDeptIndex.DataSource = objBCM.GetAccDept(e.Item.Cells(1).Text)
            rptDeptIndex.DataBind()
            rptUserName.DataSource = objBCM.GetAccBuyer(e.Item.Cells(1).Text)
            rptUserName.DataBind()
            rptUserID.DataSource = objBCM.GetAccBuyer(e.Item.Cells(1).Text)
            rptUserID.DataBind()
            objBCM = Nothing
            'Dim bcDelete As LinkButton
            'bcDelete = CType(e.Item.Cells(7).Controls(0), LinkButton)
            'bcDelete.Attributes.Add("Onclick", "return CustomMsg('deleteUA');")

            If CType(e.Item.Cells(4).Controls(1), Repeater).Items.Count = 0 Then
                e.Item.Cells(4).Text = "None"
                e.Item.Cells(5).Text = "None"
            End If

        End If
    End Sub
    Private Sub dgDept_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgDept.ItemDataBound

        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(5).ColumnSpan = 2
            e.Item.Cells(6).Visible = False
            e.Item.Cells(5).Text = "Function"
        ElseIf e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            CheckButtonRight(e)
            Dim rpt As Repeater
            rpt = e.Item.FindControl("sub")
            rpt.DataSource = GetAccListByBuyer(e.Item.Cells(3).Text)
            rpt.DataBind()

            'Dim bcDelete As LinkButton
            'bcDelete = CType(e.Item.Cells(6).Controls(0), LinkButton)
            'bcDelete.Attributes.Add("Onclick", "return CustomMsg('deleteUA');")

            If e.Item.Cells(0).Text = viewstate("duplic") Then
                e.Item.Cells(0).Text = ""
            Else
                viewstate("duplic") = e.Item.Cells(0).Text
            End If

            If CType(e.Item.Cells(4).Controls(1), Repeater).Items.Count = 0 Then
                e.Item.Cells(4).Text = "None"
            End If
        End If
    End Sub

    Private Sub CheckButtonRight(ByVal e)
        Dim bcUpdate As LinkButton
        Dim bcDelete As LinkButton

        If rdUser.Checked Then
            bcUpdate = CType(e.Item.Cells(5).Controls(0), LinkButton)
            bcDelete = CType(e.Item.Cells(6).Controls(0), LinkButton)
        Else
            bcUpdate = CType(e.Item.Cells(6).Controls(0), LinkButton)
            bcDelete = CType(e.Item.Cells(7).Controls(0), LinkButton)
        End If


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

    Private Sub dgDept_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgDept.ItemCommand
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim strDept As String = e.Item.Cells(1).Text
            Dim strDeptName As String = e.Item.Cells(0).Text
            Dim strUserId As String = e.Item.Cells(3).Text
            Dim strName As String = e.Item.Cells(2).Text

            If e.CommandName = "Modify" Then
                Response.Redirect(dDispatcher.direct("BCM", "bcmBuyerAssByUser.aspx", "pageid=" & strPageId & "&DeptName=" & Server.UrlEncode(strDeptName) & "&Deptid=" & Server.UrlEncode(strDept) & "&userid=" & strUserId & "&username=" & Server.UrlEncode(strName)))
            ElseIf e.CommandName = "Delete" Then
                DeleteAccUserByUser(strUserId)
            End If
        End If
    End Sub

    Private Sub dgAccCode_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgAccCode.ItemCommand
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim strAccIndex As String = e.Item.Cells(1).Text
            Dim strAccList As String = e.Item.Cells(0).Text
            'Dim strDept As String = ctype(CType(e.Item.FindControl("rptDept"), Repeater).ItemTemplate,i

            If e.CommandName = "Modify" Then
                Response.Redirect(dDispatcher.direct("BCM", "bcmBuyerAssByAccCode.aspx", "pageid=" & strPageId & "&accindex=" & strAccIndex & "&acclist=" & Server.UrlEncode(strAccList)))
            ElseIf e.CommandName = "Delete" Then
                DeleteAccUserByAcc(strAccIndex)
            End If
        End If
    End Sub

    Private Sub DeleteAccUserByUser(ByVal pUserId As String)
        Dim objBCM As New BudgetControl
        If objBCM.DelAccUserByUser(pUserId) Then
            viewstate("duplic") = ""
            'Common.NetMsgbox(Me, objBCM.Message, MsgBoxStyle.Information, "Wheel")
            Common.NetMsgbox(Me, "Record removed.", MsgBoxStyle.Information)
            Bindgrid(dgDept)
        Else
            'Common.NetMsgbox(Me, objBCM.Message, MsgBoxStyle.Information, "Wheel")
            Common.NetMsgbox(Me, "No record removed.", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub DeleteAccUserByAcc(ByVal pAccCode As String)
        Dim objBCM As New BudgetControl
        If objBCM.DelAccUserByAcc(pAccCode) Then
            viewstate("duplic") = ""
            'Common.NetMsgbox(Me, objBCM.Message, MsgBoxStyle.Information, "Wheel")
            Common.NetMsgbox(Me, "Record removed.", MsgBoxStyle.Information)
            Bindgrid(dgAccCode)
        Else
            'Common.NetMsgbox(Me, objBCM.Message, MsgBoxStyle.Information, "Wheel")
            Common.NetMsgbox(Me, "No record removed.", MsgBoxStyle.Information)
        End If
    End Sub

    Public Sub PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        source.CurrentPageIndex = e.NewPageIndex
        viewstate("duplic") = ""
        Bindgrid(source)

    End Sub

    Public Sub itemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        Grid_ItemCreated(sender, e)
    End Sub

    Private Sub rdUser_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdUser.CheckedChanged
        If rdUser.Checked Then
            tbUser.Style.Item("Display") = ""
            tbAccCode.Style.Item("Display") = "none"
            dgAccCode.DataBind()
        End If
    End Sub

    Private Sub rdAccountCode_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdAccountCode.CheckedChanged
        If rdAccountCode.Checked Then
            tbAccCode.Style.Item("Display") = ""
            tbUser.Style.Item("Display") = "none"
            dgDept.DataBind()
        End If
    End Sub


    'Name       : GetAccListByBuyer
    'Author     : kk
    'Descption  : Get BCM/ Acc List by a buyer
    'LastUpadte : 19 Dec 2004
    Private Function GetAccListByBuyer(ByVal pName As String) As DataSet
        Dim objBCM As New BudgetControl
        Dim ds As DataSet
        ds = objBCM.getBCMListByUserNew(pName)
        Return ds
    End Function


End Class
