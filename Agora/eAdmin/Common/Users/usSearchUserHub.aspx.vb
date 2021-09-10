Imports AgoraLegacy
Imports SSO.Component

Public Class usSearchUserHub
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSearch As System.Web.UI.WebControls.TextBox
    Protected WithEvents imbSearch As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmbGroup As System.Web.UI.WebControls.DropDownList
    Protected WithEvents dgUser As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid2 As System.Web.UI.WebControls.DataGrid
    Private ds As DataSet
    Protected WithEvents totrows As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents txtUserID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUserName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_result As System.Web.UI.WebControls.Label
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents cmdUnlock As System.Web.UI.WebControls.Button
    Protected WithEvents cmdActivate As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDeactive As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblUL As System.Web.UI.WebControls.Label
    Protected WithEvents lblAU As System.Web.UI.WebControls.Label
    Protected WithEvents lblUserLicense As System.Web.UI.WebControls.Label
    Protected WithEvents lblActiveUser As System.Web.UI.WebControls.Label

    Private lngRecNO As Long
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
        'SearchID.Value = ""
        'SearchKey.Value = ""
    End Sub

#End Region



    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        If Session("CompanyIdToken") = "" Then
            Session("CompanyIdToken") = Session("CompanyIDBkup")
        End If
        SetGridProperty(dgUser)

      
        cmdActivate.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','activate');")
        cmdDeactive.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','deactive');")
        cmdUnlock.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','unlock');")
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        '*****************meilai 12/1/2005***************************
        lblAU.Visible = False
        lblActiveUser.Visible = False
        lblUL.Visible = False
        lblUserLicense.Visible = False

        '************************************************************
    End Sub



    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim strUserID As String
        Dim strUserName As String

        strUserID = Me.txtUserID.Text
        strUserName = Me.txtUserName.Text

        Dim objUsers As New Users

        '//Retrieve Data from Database
        Dim ds As DataSet = New DataSet
        ds = objUsers.SearchUser(strUserID, strUserName, True)

        '//for sorting asc or desc
        Dim dvViewDept As DataView
        dvViewDept = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)
        'If pSorted Then
        dvViewDept.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewDept.Sort += " DESC"
        'End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If dgUser.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dgUser.PageSize = 0 Then
                dgUser.CurrentPageIndex = dgUser.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        If intPageRecordCnt > 0 Then
            ' check when user re-enter search criteria and click on other page without click search button
            If dgUser.CurrentPageIndex > (dvViewDept.Count \ dgUser.PageSize) Then
                dgUser.CurrentPageIndex = IIf((dvViewDept.Count \ dgUser.PageSize) = 1, 0, (dvViewDept.Count \ dgUser.PageSize))
            ElseIf dgUser.CurrentPageIndex = (dvViewDept.Count \ dgUser.PageSize) Then
                If viewstate("PageCount") = (dvViewDept.Count \ dgUser.PageSize) Then
                    'user does not re-enter search criteria 
                    dgUser.CurrentPageIndex = IIf((dvViewDept.Count \ dgUser.PageSize) = 0, 0, (dvViewDept.Count \ dgUser.PageSize) - 1)
                Else
                    If (dvViewDept.Count Mod dgUser.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dgUser.CurrentPageIndex = IIf((dvViewDept.Count \ dgUser.PageSize) = 1, 0, (dvViewDept.Count \ dgUser.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        dgUser.CurrentPageIndex = (dvViewDept.Count \ dgUser.PageSize)
                    End If
                End If
            End If
            '--------------------------------
            'intTotPage = dtgDept.PageCount
            cmdDelete.Enabled = True
            cmdModify.Enabled = True
            cmdUnlock.Enabled = True
            cmdActivate.Enabled = True
            cmdDeactive.Enabled = True
            cmdUnlock.Enabled = True
            '//mean Enable, can't use button.Enabled because this is a HTML button
            cmdReset.Disabled = False
            '*****************meilai 12/1/2005 display user license and active user*********************
            lblAU.Visible = True
            lblActiveUser.Visible = True
            lblUL.Visible = True
            lblUserLicense.Visible = True
            lblUserLicense.Text = objUsers.getUserLicense(True)
            lblActiveUser.Text = objUsers.getActiveUser(True)
            '************************************************************************
            dgUser.DataSource = dvViewDept
            dgUser.DataBind()
        Else
            'dtgDept.DataSource = ""
            '****************meilai 12/1/2005******************
            lblAU.Visible = False
            lblActiveUser.Visible = False
            lblUL.Visible = False
            lblUserLicense.Visible = False
            '**************************************************
            cmdDelete.Enabled = False
            cmdModify.Enabled = False
            cmdUnlock.Enabled = False
            cmdActivate.Enabled = False
            cmdDeactive.Enabled = False
            cmdReset.Disabled = True
            dgUser.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            'intTotPage = 0
        End If
        ' add for above checking
        viewstate("PageCount") = dgUser.PageCount
        'ShowStats()
    End Function

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dgUser.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgDept_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgUser.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dgUser, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")

            e.Item.Cells(3).Text = "User Group"
            e.Item.Cells(3).ForeColor = Color.DarkSlateBlue
        End If
    End Sub

    Private Sub dgJob_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgUser.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            'to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            'to dynamic build hyoerlink
            Dim lnkUserID As HyperLink
            lnkUserID = e.Item.FindControl("lnkUserID")
            'lnkUserID.NavigateUrl = "UsUserHub2.aspx?mode=modify&userid=" & dv("UM_USER_ID")
            lnkUserID.NavigateUrl = dDispatcher.direct("Users", "UsUserHub2.aspx", "mode=modify&userid=" & dv("UM_USER_ID"))

            lnkUserID.Text = dv("UM_USER_ID")

            'to add a repeater to display user group
            Dim rpt As Repeater
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                rpt = e.Item.FindControl("sub")
                'AddHandler a.ItemDataBound, AddressOf Test
                rpt.DataSource = GetChildDatasource(CType(e.Item.Cells(1).Controls(1), HyperLink).Text)
                rpt.DataBind()

            End If

            If e.Item.Cells(6).Text = "S" Then
                e.Item.Cells(6).Text = "Y"
                e.Item.Cells(6).ForeColor = Color.Red
            End If
        End If

    End Sub

    Private Function GetChildDatasource(ByVal pName As String) As DataView
        Dim strSQl As String
        Dim objDb As New  EAD.DBCom

        'strSQl = "SELECT IsNULL(UGM_USRGRP_NAME,'HubAdmin') AS UGM_USRGRP_NAME  from USERS_USRGRP LEFT JOIN USER_GROUP_MSTR ON UU_USRGRP_ID=UGM_USRGRP_ID WHERE UU_USER_ID='" & Common.Parse(pName) & "' AND UU_COY_ID='" & Session("CompanyIdToken") & "'"
        strSQl = "SELECT IsNULL(UGM_USRGRP_NAME,'HubAdmin') AS UGM_USRGRP_NAME  from USERS_USRGRP LEFT JOIN USER_GROUP_MSTR ON UU_USRGRP_ID=UGM_USRGRP_ID AND UGM_APP_PKG=UU_APP_PKG WHERE UU_USER_ID='" & Common.Parse(pName) & "' AND UU_COY_ID='" & Common.Parse(Session("CompanyIdToken")) & "'"
        Return objDb.GetView(strSQl)
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgUser.SortCommand
        Grid_SortCommand(sender, e)
        dgUser.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub


    Sub dtgDept_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dgUser.PageIndexChanged
        dgUser.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub New()

    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim objUser As New Users
        'If objUser.IsExceedUserLimit(Session("CompanyIdToken")) = False Then
        If objUser.IsExceedAdminLimit(Session("CompanyIdToken")) = False Then
            'Me.Response.Redirect("UsUserHub2.aspx?pageid=" & ViewState("pageid") & "&mode=add")
            Me.Response.Redirect(dDispatcher.direct("Users", "UsUserHub2.aspx", "pageid=" & ViewState("pageid") & "&mode=add"))
        Else
            Common.NetMsgbox(Me, objUser.Message, MsgBoxStyle.Information)
        End If
        '*************meilai 14/1/2005******************************
        Bindgrid()
        '***********************************************************
    End Sub

    Private Sub cmdModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim strUserid As String
        Dim chkItem As CheckBox
        For Each dgItem In dgUser.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                strUserid = CType(dgItem.FindControl("lnkUserID"), HyperLink).Text
                'Me.Response.Redirect("UsUserHub2.aspx?mode=modify&userid=" & strUserid)
                Me.Response.Redirect(dDispatcher.direct("Users", "UsUserHub2.aspx", "mode=modify&userid=" & strUserid))
            End If
        Next
    End Sub

    'Name       : FillCheckBoxGrid
    'Author     : Kaithim
    'Descption  : fill the datagrid check box with a string 
    'Remark     :
    'ReturnValue: 
    'LastUpadte : 19 Sep 2002
    'Version    : 1.00
    Public Function FillCheckBoxGrid(ByVal pInString As String, _
                                     ByRef pDataGrid As DataGrid) As Boolean

        Dim lngLoop As Long
        Dim ary() As String = Split(pInString, ",")
        Dim varItem As DataGridItem
        'For lngLoop = 0 To UBound(ary)
        '    For Each varItem In pDataGrid.Items
        '        If pDataGrid.DataKeys(varItem.ItemIndex).ToString = ary(lngLoop) Then
        '            Dim chk As CheckBox = varItem.Cells(0).Controls(0)
        '            chk.Checked = True
        '        End If
        '    Next
        'Next
        For lngLoop = 0 To UBound(ary)
            For Each varItem In pDataGrid.Items
                If pDataGrid.DataKeys(varItem.ItemIndex).ToString = ary(lngLoop) Then
                    Dim chk As CheckBox = varItem.Cells(0).FindControl("select")
                    chk.Checked = True
                End If
            Next
        Next
    End Function


    Private Sub cmdUnlock_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUnlock.Click
        Dim dgItem As DataGridItem
        Dim strUserid As String
        Dim chkItem As CheckBox
        Dim objUser As New Users

        Try
            For Each dgItem In dgUser.Items
                chkItem = dgItem.FindControl("chkSelection")
                If chkItem.Checked Then
                    strUserid = CType(dgItem.FindControl("lnkUserID"), HyperLink).Text
                    objUser.SuspendAc(strUserid, Session("companyIdToken"), 0)
                End If
            Next
            Common.NetMsgbox(Me, "User Account Unlocked.", MsgBoxStyle.Information)
            Bindgrid()
        Catch errExp As CustomException
            Common.TrwExp(errExp)
        Catch errExp1 As Exception
            Common.TrwExp(errExp1)
        End Try
    End Sub

    Private Sub cmdActivate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdActivate.Click
        Dim dgItem As DataGridItem
        Dim strUserid As String
        Dim chkItem As CheckBox
        Dim objUser As New Users

        Try
            For Each dgItem In dgUser.Items
                chkItem = dgItem.FindControl("chkSelection")
                If chkItem.Checked Then
                    strUserid = CType(dgItem.FindControl("lnkUserID"), HyperLink).Text
                    If objUser.ActivateUserAccount(strUserid, True, True) = False Then
                        Exit For
                    End If
                End If
            Next
            Common.NetMsgbox(Me, objUser.Message, MsgBoxStyle.Information)
            Bindgrid()
        Catch errExp As CustomException
            Common.TrwExp(errExp)
        Catch errExp1 As Exception
            Common.TrwExp(errExp1)
        End Try
    End Sub

    Private Sub cmdDeactive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDeactive.Click
        Dim dgItem As DataGridItem
        Dim strUserid As String
        Dim chkItem As CheckBox
        Dim objUser As New Users

        Try
            For Each dgItem In dgUser.Items
                chkItem = dgItem.FindControl("chkSelection")
                If chkItem.Checked Then
                    strUserid = CType(dgItem.FindControl("lnkUserID"), HyperLink).Text
                    objUser.ActivateUserAccount(strUserid, False, True)
                End If
            Next
            Common.NetMsgbox(Me, "User Account Deactivated.", MsgBoxStyle.Information)
            Bindgrid()
        Catch errExp As CustomException
            Common.TrwExp(errExp)
        Catch errExp1 As Exception
            Common.TrwExp(errExp1)
        End Try
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim strUserId As String
        Dim chkItem As CheckBox
        Dim objUser As New Users

        For Each dgItem In dgUser.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                strUserId = CType(dgItem.FindControl("lnkUserID"), HyperLink).Text
                If objUser.DelUser(strUserId, True) = False Then
                    Common.NetMsgbox(Me, objUser.Message, MsgBoxStyle.Information)
                    Exit For
                End If
            End If
        Next
        viewstate("action") = "del"
        Bindgrid()
        objUser = Nothing
    End Sub




End Class


