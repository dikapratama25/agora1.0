
Imports AgoraLegacy
Imports eProcure.Component
Public Class usFinVwDept
    Inherits AgoraLegacy.AppBaseClass
    Dim objDb As New EAD.DBCom
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Dim chkAll As CheckBox
    Dim dDispatcher As New AgoraLegacy.dispatcher
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents txtUserID As System.Web.UI.WebControls.Label
    'Protected WithEvents txtUserName As System.Web.UI.WebControls.Label
    'Protected WithEvents lbl_result As System.Web.UI.WebControls.Label
    Protected WithEvents dgDept As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdBack As System.Web.UI.WebControls.Button
    'Protected WithEvents rdAllDept As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents cboUser As System.Web.UI.WebControls.DropDownList
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Protected WithEvents lblAction As System.Web.UI.WebControls.Label

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    'Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
    '    'cmdSave.Enabled = False
    '    Dim alButtonList As ArrayList
    '    alButtonList = New ArrayList
    '    alButtonList.Add(cmdSave)
    '    htPageAccess.Add("update", alButtonList)
    '    alButtonList.Add(cmdSave)
    '    htPageAccess.Add("add", alButtonList)
    '    CheckButtonAccess()
    '    cmdSave.Enabled = viewstate("blnCmdSave") And (blnCanAdd Or blnCanUpdate)
    '    'cmdClear.Enabled = blnCanAdd Or blnCanUpdate Or blnCanDelete
    'End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)

        SetGridProperty(dgDept)
        If Not Page.IsPostBack Then
            cmdSave.Enabled = False
            GenerateTab()
            ViewState("blnCmdSave") = True
            BindUser()
            Me.lblAction.Text = "Step 1: Create, modify or delete User Account.<br />Step 2: Assign Delivery Address to selected User Account.<br />Step 3: Assign Billing Address to selected User Account.<br /><b>=></b>Step 4: Assign Finance Viewing Department to selected User Account.<br />Step 5: Assign Commodity Type to selected User Account."

        End If

    End Sub

    Public Function BindUser()
        Dim cbolist As New ListItem
        Dim objUsr As New Users
        Dim dvCustom As DataView

        dvCustom = objUsr.getUsr(True)
        cboUser.Items.Clear()
        If Not dvCustom Is Nothing Then
            Common.FillDdl(cboUser, "USER", "UM_USER_ID", dvCustom)
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboUser.Items.Insert(0, cbolist)

    End Function

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim objUser As New Users

        '//Retrieve Data from Database
        Dim ds As DataSet = New DataSet 

        Dim VOpt As String = objDb.GetVal("SELECT FUD_VIEWOPTION FROM FINANCE_USER_DEPARTMENT WHERE FUD_USER_ID='" & ViewState("UserId") & "' LIMIT 1")

        If VOpt = "" Then
            VOpt = "0"
        End If

        If VOpt = 0 Then
            ds = objUser.SearchUserDept(ViewState("UserId"), 0)
        Else
            ds = objUser.SearchUserDept(ViewState("UserId"), 1)
        End If


        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt
        '//for sorting asc or desc
        Dim dvViewAddr As DataView
        dvViewAddr = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)
        'If pSorted Then
        dvViewAddr.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewAddr.Sort += " DESC"
        'End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If ViewState("action") = "del" Then
            If dgDept.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dgDept.PageSize = 0 Then
                dgDept.CurrentPageIndex = dgDept.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        If intPageRecordCnt > 0 Then
            'intTotPage = dtgDept.PageCount
            cmdSave.Enabled = True
            ViewState("blnCmdSave") = True
            'cmdReset.Disabled = True
            '//mean Enable, can't use button.Enabled because this is a HTML button
            'cmdReset.Disabled = False
            dgDept.DataSource = dvViewAddr
            dgDept.DataBind()
        Else
            'dtgDept.DataSource = ""
            cmdSave.Enabled = False
            ViewState("blnCmdSave") = False
            'cmdReset.Disabled = True
            dgDept.DataBind()
            'Common.NetMsgbox(Me, MsgNoRecord)
            'intTotPage = 0
        End If
        'ShowStats()
    End Function

    Private Sub dgAddr_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgDept.ItemCreated
        '//this line must be included
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dgDept, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            chkAll = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
            'ElseIf e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            '    Dim chk As CheckBox
            '    chk = e.Item.Cells(0).FindControl("chkSelection")
            '    chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim dgItem As DataGridItem
        Dim strSQL, strDeptCode As String
        Dim chkItem As CheckBox
        Dim objComp As New Companies
        Dim Query(0) As String
        Dim bEmpty As Boolean = True

        Try
            Dim strDept As String = Dg2String(dgDept, 2)
            objComp.DelFinDeptViewing(ViewState("UserId"), strDept)
            objComp.DelFinDeptViewing(ViewState("UserId"), "'0'", True)

            'If rdAllDept.SelectedValue = 1 Then

            '    strSQL = "INSERT INTO FINANCE_USER_DEPARTMENT(FUD_COY_ID,FUD_USER_ID,FUD_DEPT_CODE,FUD_VIEWOPTION) " & _
            '               "VALUES('" & Session("CompanyId") & "','" & viewstate("UserId") & "','0','1') "
            '    Common.Insert2Ary(Query, strSQL)
            '    bEmpty = False
            'Else

            For Each dgItem In dgDept.Items
                chkItem = dgItem.FindControl("chkSelection")
                If chkItem.Checked Then
                    strDeptCode = dgItem.Cells(2).Text
                    strSQL = "INSERT INTO FINANCE_USER_DEPARTMENT(FUD_COY_ID,FUD_USER_ID,FUD_DEPT_CODE,FUD_VIEWOPTION) " & _
                        "VALUES('" & Session("CompanyId") & "','" & ViewState("UserId") & "','" & Common.Parse(strDeptCode) & "','0') "
                    Common.Insert2Ary(Query, strSQL)
                    bEmpty = False
                End If
            Next
            'End If

            If bEmpty = False Then
                If objDb.BatchExecute(Query) Then
                    Common.NetMsgbox(Me, "Record Saved.", MsgBoxStyle.Information)
                Else
                    Common.NetMsgbox(Me, "Record not saved.", MsgBoxStyle.Information)
                End If
            Else
                Common.NetMsgbox(Me, "Record Saved.", MsgBoxStyle.Information)
            End If
            Bindgrid()
        Catch errExp As CustomException
            Common.TrwExp(errExp)
        Catch errExp1 As Exception
            Common.TrwExp(errExp1)
        End Try
    End Sub

    Private Sub dgAddr_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgDept.ItemDataBound
        'If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
        '    If rdAllDept.SelectedValue = 1 Then
        '        e.Item.Cells(0).Visible = False
        '    Else
        '        Dim chk As CheckBox = e.Item.FindControl("chkSelection")
        '        If e.Item.Cells(1).Text = "Y" Then
        '            chk.Checked = True
        '        Else
        '            chkAll.Checked = False
        '        End If
        '        e.Item.Cells(0).Visible = True
        '    End If
        'ElseIf e.Item.ItemType = ListItemType.Header Then
        '    If rdAllDept.SelectedValue = 1 Then
        '        e.Item.Cells(0).Visible = False
        '    Else
        '        e.Item.Cells(0).Visible = True
        '    End If
        'End If
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            If e.Item.Cells(1).Text = "Y" Then
                chk.Checked = True
            Else
                chkAll.Checked = False
            End If

        End If
    End Sub

    Sub dtgDept_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dgDept.PageIndexChanged
        dgDept.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    'Name       : Dg2String
    'Author     : KK
    'Descption  : Convert dg to String   
    'Remark     :
    'ReturnValue: String "vv,zz,xx,yy"
    'LastUpadte : 7 Dec 2004
    'Version    : 1.00
    Public Function Dg2String(ByRef pDg As DataGrid, ByVal pCol As Integer) As String
        Dim strReturn As String
        Dim varItem As DataGridItem
        For Each varItem In pDg.Items

            strReturn &= "'" & Common.Parse(varItem.Cells(pCol).Text) & "',"

        Next
        If strReturn <> "" Then
            Return Mid(strReturn, 1, Len(strReturn) - 1)
        Else
            Return Nothing
        End If
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgDept.SortCommand
        Grid_SortCommand(sender, e)
        dgDept.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    'Private Sub rdAllDept_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdAllDept.SelectedIndexChanged
    '    If rdAllDept.SelectedValue = 1 Then
    '        'dgDept.Columns(0).Visible = False
    '        'dgDept.Attributes.Add("class", "TableCol")
    '        '*******************meilai 11/1/2005 when select all department, invisible reset button*********
    '        cmdReset.Visible = False
    '        '*******************meilai close*************************************************************
    '    Else
    '        'dgDept.Columns(0).Visible = True
    '        'dgDept.Attributes.Add("class", "TableCol")
    '        '*******************meilai 11/1/2005 when select all department, invisible reset button*********
    '        cmdReset.Visible = True
    '        '*******************meilai close*************************************************************

    '    End If
    '    'SetGridProperty(dgDept)
    '    Bindgrid(True)
    'End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_UserAddr_tabs") = "<div class=""t_entity""><ul>" & _
                            "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usSearchUser.aspx", "pageid=" & strPageId) & """><span>User Account</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=D&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=B&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Users", "usFinVwDept.aspx", "pageid=" & strPageId) & """><span>Finance Viewing Department</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usCommodity.aspx", "pageid=" & strPageId) & """><span>Commodity Assignment</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "</ul><div></div></div>"
    End Sub

    Private Sub cboUser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboUser.SelectedIndexChanged
        If cboUser.SelectedIndex > 0 Then
            dgDept.CurrentPageIndex = 0
            ViewState("UserId") = cboUser.SelectedValue
            Bindgrid()
        Else
            ViewState("UserId") = ""
            'Bindgrid()
            cmdSave.Enabled = False
            dgDept.DataSource = Nothing
            dgDept.DataBind()
        End If
    End Sub
End Class
