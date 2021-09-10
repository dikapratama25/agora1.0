
Imports AgoraLegacy
Imports eProcure.Component
Imports System.text
Public Class usDeliveryAddr
    Inherits AgoraLegacy.AppBaseClass
    Dim objDb As New EAD.DBCom
    Dim objUsers As New Users
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    'Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    'Protected WithEvents rdAdd As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents hidAll As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txt_Code As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_City As System.Web.UI.WebControls.TextBox
    Protected WithEvents cbo_State As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cbo_Country As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmd_Search As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Clear As System.Web.UI.WebControls.Button
    Protected WithEvents trSearch As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblAction As System.Web.UI.WebControls.Label
    Protected WithEvents rbFixedRole As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rbtnType As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents lblAddFor As System.Web.UI.WebControls.Label
    Protected WithEvents lblAddFor1 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdRemoveAll As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.WebControls.Button



    'Protected WithEvents btnRemoveSelected As System.Web.UI.WebControls.Button
    Dim chkAll As CheckBox
    Dim dDispatcher As New AgoraLegacy.dispatcher
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents txtUserID As System.Web.UI.WebControls.Label
    'Protected WithEvents lbl_result As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    'Protected WithEvents txtUserName As System.Web.UI.WebControls.Label
    Protected WithEvents dgAddr As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cboUser As System.Web.UI.WebControls.DropDownList
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        'cmdSave.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdSave)
        htPageAccess.Add("update", alButtonList)
        alButtonList.Add(cmdSave)
        htPageAccess.Add("add", alButtonList)
        CheckButtonAccess()
        cmdSave.Enabled = viewstate("blnCmdSave") And (blnCanAdd Or blnCanUpdate)
        'cmdClear.Enabled = blnCanAdd Or blnCanUpdate Or blnCanDelete
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        If Request.QueryString("pageid") <> "" Then
            strPageId = Request.QueryString("pageid")
        Else
            strPageId = "44"
        End If
        'ViewState("UserId") = Request.Params("userid")
        'ViewState("Role") = Request.Params("role")
       
        SetGridProperty(dgAddr)
        If Not Page.IsPostBack Then
            lblAddFor.Visible = False
            lblAddFor1.Visible = False
            rbFixedRole.Visible = False
            ViewState("AddrType") = Request.Params("AddrType")
            GenerateTab()
            If ViewState("AddrType") = "D" Then
                Me.lblAction.Text = "Step 1: Create, modify or delete User Account.<br /><b>=></b> Step 2: Assign Delivery Address to selected User Account.<br>Step 3: Assign Billing Address to selected User Account."
                'If Session("Env") <> "FTN" Then 'Non FTN
                '    Me.rbFixedRole.SelectedValue = "Buyer"
                '    ViewState("Role") = "B"
                '    Me.lblAction.Text = Me.lblAction.Text & "<br>Step 4: Assign Finance Viewing Department to selected User Account.<br>Step 5: Assign Commodity Type to selected User Account."
                'Else    'FTN
                '    Me.lblAction.Text = Me.lblAction.Text & "<br>Step 4: Assign Commodity Type to selected User Account."
                'End If
                Me.rbFixedRole.SelectedValue = "Buyer"
                ViewState("Role") = "B"
                Me.lblAction.Text = Me.lblAction.Text & "<br>Step 4: Assign Finance Viewing Department to selected User Account.<br>Step 5: Assign Commodity Type to selected User Account."

                cmdRemoveAll.Text = "Remove All Delivery Address"
                cmdRemoveAll.Attributes.Add("onclick", "return confirm('Warning! This will remove ALL selected addresses and NO Delivery Address will be assigned to this Buyer. Are you Sure?');")
                rbtnType.Items(0).Text = "All Delivery Address"
                rbtnType.Items(1).Text = "Selected Delivery Address"
            Else
                Me.lblAction.Text = "Step 1: Create, modify or delete User Account.<br />Step 2: Assign Delivery Address to selected User Account.<br><b>=></b> Step 3: Assign Billing Address to selected User Account."
                'If Session("Env") <> "FTN" Then 'Non FTN
                '    Me.lblAction.Text = Me.lblAction.Text & "<br>Step 4: Assign Finance Viewing Department to selected User Account.<br>Step 5: Assign Commodity Type to selected User Account."
                'Else    'FTN
                '    Me.lblAction.Text = Me.lblAction.Text & "<br>Step 4: Assign Commodity Type to selected User Account."
                'End If
                Me.lblAction.Text = Me.lblAction.Text & "<br>Step 4: Assign Finance Viewing Department to selected User Account.<br>Step 5: Assign Commodity Type to selected User Account."

                cmdRemoveAll.Text = "Remove All Billing Address"
                cmdRemoveAll.Attributes.Add("onclick", "return confirm('Warning! This will remove ALL selected addresses and NO Billing Address will be assigned to this Buyer. Are you Sure?');")
                rbtnType.Items(0).Text = "All Billing Address"
                rbtnType.Items(1).Text = "Selected Billing Address"
            End If
            BindUser()
            'ViewState("blnCmdSave") = True
            'ViewState("resetRadio") = True
            'If ViewState("Role") Is Nothing Then
            '    'lblTitle.Text = "Delivery Address"
            'Else
            '    ' for set Billing/Delivery Address for Buyer
            '    Select Case ViewState("Role")
            '        Case "SK"
            '            'lblTitle.Text = "Delivery Address"
            '        Case "B"
            '            ViewState("AddrType") = Request.Params("addrtype")
            '            If ViewState("AddrType") = "D" Then
            '                'lblTitle.Text = "Delivery Address"
            '                rdAdd.Items(0).Text = "All Delivery Address"
            '                rdAdd.Items(1).Text = "Selected Delivery Address"
            '            Else
            '                'lblTitle.Text = "Billing Address"
            '                rdAdd.Items(0).Text = "All Billing Address"
            '                rdAdd.Items(1).Text = "Selected Billing Address"
            '            End If
            '    End Select
            'End If

            'Dim objUserDetails As New User
            'Dim objUser As New Users
            'objUserDetails = objUser.GetUserDetails(ViewState("UserId"), Session("CompanyId"))
            'If Not objUserDetails Is Nothing Then
            '    'txtUserID.Text = objUserDetails.UserID
            '    'txtUserName.Text = objUserDetails.Name
            '    'objUser = Nothing
            '    'objUserDetails = Nothing
            '    Bindgrid()
            'Else
            '    'txtUserID.Text = "None"
            '    'txtUserName.Text = "None"
            '    cmdSave.Enabled = False
            '    ViewState("blnCmdSave") = False
            '    cmdReset.Disabled = True
            '    Common.NetMsgbox(Me, "Invalid user. Please save user before assign delivery address.", MsgBoxStyle.Information)
            'End If
            Dim objGlobal As New AppGlobals
            objGlobal.FillCodeTable(cbo_Country, CodeTable.Country)
            objGlobal.FillState(cbo_State, cbo_Country.SelectedItem.Value)

            'objGlobal.FillCodeTable(cbo_State, CodeTable.State)
            'Common.SelDdl("", cbo_State, True, True)
            objGlobal = Nothing
            'lnkBack.NavigateUrl = "#"
            'lnkBack.Attributes.Add("onClick", "window.close();")
            'cmdSave.Attributes.Add("onclick", "CheckAtLeastOneNotAll('chkSelection');")
        End If
    End Sub
    Public Function BindUser()
        Dim cbolist As New ListItem
        Dim objUsr As New Users
        Dim dvCustom As DataView

        If ViewState("AddrType") = "D" Then
            'If Session("Env") <> "FTN" Then 'Non FTN
            '    dvCustom = objUsr.getUsrWithAddr("D", False)
            'Else
            '    dvCustom = objUsr.getUsrWithAddr("D", True)
            'End If
            dvCustom = objUsr.getUsrWithAddr("D", False)
        Else
            'If Session("Env") <> "FTN" Then 'Non FTN
            '    dvCustom = objUsr.getUsrWithAddr("B", False)
            'Else
            '    dvCustom = objUsr.getUsrWithAddr("B", True)
            'End If
            dvCustom = objUsr.getUsrWithAddr("B", False)
        End If
        cboUser.Items.Clear()
        If Not dvCustom Is Nothing Then
            Common.FillDdl(cboUser, "USER", "UM_USER_ID", dvCustom)
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboUser.Items.Insert(0, cbolist)

    End Function
    'Private Function GetUserLocLevel() As Integer
    '    Dim objUser As New Users
    '    Dim objUserDetails As New User
    '    Dim iLevel As String

    '    objUserDetails = objUser.GetUserDetails(viewstate("UserId"), Session("CompanyId"))
    '    If objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Store_Keeper) And objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Second_Level_Receiver) Then
    '        iLevel = 0
    '    ElseIf objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Store_Keeper) And Not objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Second_Level_Receiver) Then
    '        iLevel = 1
    '    ElseIf Not objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Store_Keeper) And objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Second_Level_Receiver) Then
    '        iLevel = 2
    '    End If

    '    Return iLevel
    'End Function
    Sub checkaddress(ByVal user As String)
        Dim objUsers As New Users
        Dim objUserDetails As New User
        Dim ds As New DataSet
        Dim ds2 As New DataSet
        ds = objUsers.getAllAddr(ViewState("AddrType"), ViewState("Role"))
        ds2 = objUsers.getSelectedAddr(ViewState("AddrType"), user, ViewState("Role"))
        If ds.Tables(0).Rows.Count = ds2.Tables(0).Rows.Count Then
            rbtnType.SelectedIndex = 0
        Else
            rbtnType.SelectedIndex = 1
        End If

    End Sub
    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        'check user address        
        Dim objUsers As New Users
        Dim objUserDetails As New User
        '//Retrieve Data from Database
        Dim ds As DataSet = New DataSet

        objUserDetails = objUsers.GetUserDetails(ViewState("UserId"), Session("CompanyId"))
        'If ViewState("Role") = "SKB" Then
        '    If Request("rbFixedRole") = "Buyer" Then
        '        ViewState("Role") = "B"
        '    Else
        '        ViewState("Role") = "SK"
        '    End If
        'End If
        If cboUser.SelectedIndex <> 0 Then
            Select Case ViewState("Role")
                Case "SK"
                    If objUsers.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Store_Keeper) And objUsers.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Second_Level_Receiver) Then
                        ds = objUsers.SearchUserAddress(ViewState("UserId"), txt_Code.Text, txt_City.Text, cbo_State.SelectedValue, , , , cbo_Country.SelectedValue)
                    ElseIf objUsers.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Store_Keeper) And Not objUsers.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Second_Level_Receiver) Then
                        ds = objUsers.SearchUserAddress(ViewState("UserId"), txt_Code.Text, txt_City.Text, cbo_State.SelectedValue, 1, , , cbo_Country.SelectedValue)
                    ElseIf Not objUsers.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Store_Keeper) And objUsers.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Second_Level_Receiver) Then
                        ds = objUsers.SearchUserAddress(ViewState("UserId"), txt_Code.Text, txt_City.Text, cbo_State.SelectedValue, 2, , , cbo_Country.SelectedValue)
                    Else
                        ds = objUsers.SearchUserAddress(ViewState("UserId"), txt_Code.Text, txt_City.Text, cbo_State.SelectedValue, , , , cbo_Country.SelectedValue)
                    End If

                Case "B"
                    ds = objUsers.SearchUserAddress(ViewState("UserId"), txt_Code.Text, txt_City.Text, cbo_State.SelectedValue, IIf(ViewState("AddrType") = "D", 0, 1), True, FixedRole.Buyer, cbo_Country.SelectedValue)
            End Select
        End If
        '//for sorting asc or desc
        Dim dvViewAddr As DataView
        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                dvViewAddr = ds.Tables(0).DefaultView

                'If viewstate("resetRadio") = True Then
                '    If ds.Tables(1).Rows.Count > 0 Then
                '        'If CInt(ds.Tables(1).Rows(0)("Addr1")) = CInt(ds.Tables(1).Rows(0)("Addr2")) Then
                '        If CInt(ds.Tables(1).Rows(0)(0)) = 0 Then
                '            rdAdd.SelectedValue = 1
                '            viewstate("OrigAll") = 1 'true
                '            trSearch.Style("display") = "none"
                '            'dgAddr.Columns(0).Visible = False
                '            btnRemoveSelected.Visible = False
                '        Else
                '            rdAdd.SelectedValue = 0
                '            viewstate("OrigAll") = 0 'false
                '            trSearch.Style("display") = "inline"
                '            'dgAddr.Columns(0).Visible = True
                '            btnRemoveSelected.Visible = True
                '        End If
                '    End If
                'End If
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
                    If dgAddr.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dgAddr.PageSize = 0 Then
                        dgAddr.CurrentPageIndex = dgAddr.CurrentPageIndex - 1
                        ViewState("action") = ""
                    End If
                End If

                intPageRecordCnt = ds.Tables(0).Rows.Count
                ViewState("intPageRecordCnt") = intPageRecordCnt
                '//bind datagrid

                '//datagrid.pageCount only got value after databind

                If intPageRecordCnt > 0 Then
                    'intTotPage = dtgDept.PageCount
                    cmdSave.Enabled = True
                    ViewState("blnCmdSave") = True
                    'cmdReset.Disabled = True
                    '//mean Enable, can't use button.Enabled because this is a HTML button
                    'cmdReset.Disabled = False
                    dgAddr.DataSource = dvViewAddr
                    dgAddr.DataBind()
                Else
                    'dtgDept.DataSource = ""
                    cmdSave.Enabled = False
                    ViewState("blnCmdSave") = False
                    'cmdReset.Disabled = True
                    dgAddr.DataBind()
                    'Common.NetMsgbox(Me, MsgNoRecord)
                    'intTotPage = 0
                End If
                'ShowStats()
            Else
                dgAddr.DataBind()
                Common.NetMsgbox(Me, MsgNoRecord)
                cmdSave.Enabled = False
                ViewState("blnCmdSave") = False
            End If
        Else
            dgAddr.DataBind()
            cmdSave.Enabled = False
            ViewState("blnCmdSave") = False
        End If

    End Function

    Private Sub dgAddr_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgAddr.ItemCreated
        '//this line must be included
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dgAddr, e)

        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            chkAll = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        ElseIf e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            'mod by SAM, have to remove from ItemCreated to ItemDataBound in order to get unique ClientID
            'Dim chk As CheckBox
            'chk = e.Item.Cells(0).FindControl("chkSelection")
            'chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
        End If
    End Sub

    Sub dtgDept_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dgAddr.PageIndexChanged
        dgAddr.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub dgAddr_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgAddr.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            'If rdAdd.SelectedValue = 0 Then
            'Dim chk As CheckBox = e.Item.FindControl("chkSelection")

            'mod by SAM, have to remove from ItemCreated to ItemDataBound in order to get unique ClientID
            'START
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            'END

            '//When user change from 'All' to 'Selected' need to uncheck all checkbox
            '//need special handling for SK because we store all Address into table so e.Item.Cells(1).Text always equal Y
            '//For buyer, only an indicator stored, so e.Item.Cells(1).Text <> "Y"
            Select Case ViewState("Role")
                Case "SK"
                    'If viewstate("OrigAll") <> "1" Then
                    If e.Item.Cells(7).Text = "Y" Then
                        chk.Checked = True
                    Else
                        chkAll.Checked = False
                    End If
                    'Else
                    'chkAll.Checked = False
                    'End If

                Case "B"
                    If e.Item.Cells(7).Text = "Y" Then
                        chk.Checked = True
                    Else
                        chkAll.Checked = False
                    End If
            End Select
            If rbtnType.SelectedItem.Value = "allAddr" Then
                e.Item.Cells(0).Visible = False
                e.Item.Cells(7).Text = "Y"
            Else
                e.Item.Cells(0).Visible = True
            End If

            e.Item.Cells(7).Text = IIf(e.Item.Cells(7).Text = "Y", "Yes", "No")
        ElseIf e.Item.ItemType = ListItemType.Header Then
            If rbtnType.SelectedItem.Value = "allAddr" Then
                e.Item.Cells(0).Visible = False
            Else
                e.Item.Cells(0).Visible = True
            End If
        End If
    End Sub

    Private Function checkAtLeastOneItemSelected() As Boolean
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox
        Dim i As Integer = 0
        For Each dgItem In dgAddr.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                i = i + 1
            End If
        Next
        If i = 0 Then
            checkAtLeastOneItemSelected = False
        Else
            checkAtLeastOneItemSelected = True
        End If
    End Function

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim dgItem As DataGridItem
        Dim strSQL, strCode As String
        Dim chkItem As CheckBox
        Dim objComp As New Companies
        Dim Query(0) As String
        Dim bEmpty As Boolean = True
        Dim objUserDetails As New User
        Dim objUser As New Users
        Dim strLevel As Integer

        Try
            'If ViewState("Role") = "SKB" Then
            '    If Request("rbFixedRole") = "Buyer" Then
            '        ViewState("Role") = "B"
            '    Else
            '        ViewState("Role") = "SK"
            '    End If
            'End If
            Select Case ViewState("Role")
                Case "SK"
                    objUserDetails = objUser.GetUserDetails(ViewState("UserId"), Session("CompanyId"))
                    If objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Store_Keeper) Then
                        strLevel = 1
                    ElseIf objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Second_Level_Receiver) Then
                        strLevel = 2
                    End If

                    If rbtnType.SelectedItem.Value = "allAddr" Then ' select all
                        bEmpty = False
                        strSQL = "DELETE FROM USERS_LOCATION "
                        strSQL &= "WHERE UL_COY_ID = '" & Session("CompanyId") & "' "
                        strSQL &= "AND UL_USER_ID = '" & ViewState("UserId") & "' "
                        strSQL &= "AND UL_LEVEL = " & strLevel
                        Common.Insert2Ary(Query, strSQL)
                        strSQL = "INSERT INTO USERS_LOCATION(UL_COY_ID, UL_ADDR_CODE, UL_USER_ID, UL_LEVEL) "
                        strSQL &= "SELECT '" & Session("CompanyId") & "', "
                        strSQL &= "AM_ADDR_CODE, '" & ViewState("UserId") & "', " & strLevel & " "
                        strSQL &= "FROM ADDRESS_MSTR WHERE AM_COY_ID = '" & Session("CompanyId") & "' "
                        strSQL &= "AND AM_ADDR_TYPE = 'D' "
                        Common.Insert2Ary(Query, strSQL)
                    Else
                        Dim strAddr As String = Dg2String(dgAddr, 1)

                        ' ai chu remark on 09/11/2005 start
                        ' no need to select at least one before saving
                        'If Not checkAtLeastOneItemSelected() Then
                        '    If Not objUser.checkAddrSelected(viewstate("UserId"), "SK", straddr, strLevel) Then
                        '        Common.NetMsgbox(Me, "Please make at least one selection!", MsgBoxStyle.Information)
                        '        GoTo exit_function
                        '    End If
                        'End If
                        ' remark end

                        'objuser.checkAddrSelected(viewstate("UserId"), "SK",straddr,strlevel)
                        bEmpty = True
                        objComp.DelUsrLocation(ViewState("UserId"), strLevel, strAddr)
                        For Each dgItem In dgAddr.Items
                            chkItem = dgItem.FindControl("chkSelection")
                            If chkItem.Checked Then
                                strCode = dgItem.Cells(1).Text
                                strSQL = "INSERT INTO USERS_LOCATION(UL_COY_ID,UL_ADDR_CODE,UL_USER_ID,UL_LEVEL) " & _
                                       "VALUES('" & Session("CompanyId") & "','" & Common.Parse(strCode) & "','" & ViewState("UserId") & "'," & strLevel & ") "
                                Common.Insert2Ary(Query, strSQL)
                                bEmpty = False
                            End If
                        Next
                    End If

                Case "B"
                    If rbtnType.SelectedItem.Value = "allAddr" Then ' select all
                        strSQL = "DELETE FROM USERS_ADDR "
                        strSQL &= "WHERE UA_COY_ID = '" & Session("CompanyId") & "' "
                        strSQL &= "AND UA_USER_ID = '" & ViewState("UserId") & "' "
                        strSQL &= "AND UA_ADDR_TYPE = '" & ViewState("AddrType") & "' "
                        Common.Insert2Ary(Query, strSQL)
                        strSQL = "INSERT INTO USERS_ADDR (UA_COY_ID, UA_USER_ID, UA_ADDR_TYPE, "
                        strSQL &= "UA_ADDR_CODE, UA_ROLE, UA_VIEW_OPTION) "
                        strSQL &= "SELECT am_coy_id AS UA_COY_ID, '" & ViewState("UserId") & "' AS UA_USER_ID, "
                        strSQL &= "am_addr_type AS UA_ADDR_TYPE, am_addr_code AS UA_ADDR_CODE, "
                        strSQL &= "'Buyer' AS UA_ROLE, '1' AS UA_VIEW_OPTION FROM address_mstr a WHERE am_addr_type='" & ViewState("AddrType") & "'  "
                        strSQL &= "AND am_coy_id='" & Session("CompanyId") & "'"
                        Common.Insert2Ary(Query, strSQL)
                        bEmpty = False
                    Else
                        Dim strAddr As String = Dg2String(dgAddr, 1)

                        ' ai chu remark on 09/11/2005 start
                        ' no need to select at least one before saving
                        'If Not checkAtLeastOneItemSelected() Then
                        '    If Not objUser.checkAddrSelected(viewstate("UserId"), "B", straddr, viewstate("AddrType")) Then
                        '        Common.NetMsgbox(Me, "Please make at least one selection!", MsgBoxStyle.Information)
                        '        GoTo exit_function
                        '    End If
                        'End If
                        ' remark end

                        bEmpty = False
                        strSQL = "DELETE FROM USERS_ADDR "
                        strSQL &= "WHERE UA_COY_ID = '" & Session("CompanyId") & "' "
                        strSQL &= "AND UA_USER_ID = '" & ViewState("UserId") & "' "
                        strSQL &= "AND UA_ADDR_TYPE = '" & ViewState("AddrType") & "' "
                        strSQL &= "AND (UA_ADDR_CODE IN (" & strAddr & ") "
                        strSQL &= "OR UA_VIEW_OPTION = 1)"
                        Common.Insert2Ary(Query, strSQL)

                        For Each dgItem In dgAddr.Items
                            chkItem = dgItem.FindControl("chkSelection")
                            'sam: in VS2005, unable to get control = checked
                            If chkItem.Checked Then
                                strCode = dgItem.Cells(1).Text
                                strSQL = "INSERT INTO USERS_ADDR (UA_COY_ID, UA_USER_ID, UA_ADDR_TYPE, "
                                strSQL &= "UA_ADDR_CODE, UA_ROLE, UA_VIEW_OPTION) VALUES ("
                                strSQL &= "'" & Session("CompanyId") & "', "
                                strSQL &= "'" & ViewState("UserId") & "', "
                                strSQL &= "'" & ViewState("AddrType") & "', "
                                strSQL &= "'" & Common.Parse(strCode) & "', "
                                strSQL &= "'Buyer', 0)"
                                Common.Insert2Ary(Query, strSQL)
                            End If
                        Next

                    End If
            End Select

            If bEmpty = False Then
                If Query(0) <> String.Empty Then
                    If objDb.BatchExecute(Query) Then
                        Common.NetMsgbox(Me, "Record Saved.", MsgBoxStyle.Information)
                    Else
                        Common.NetMsgbox(Me, "Record not saved.", MsgBoxStyle.Information)
                    End If
                End If
            Else
                Common.NetMsgbox(Me, "Record Saved.", MsgBoxStyle.Information)
            End If


            'If viewstate("OrigAll") = 1 And rdAdd.SelectedValue = 0 Then
            '    '//First Load=ALL, change to 'Selected' and click Save Button
            '    viewstate("OrigAll") = 0
            'ElseIf viewstate("OrigAll") = 0 And rdAdd.SelectedValue = 1 Then
            '    '//First Load=Selected, change to 'All' and click Save Button
            '    viewstate("OrigAll") = 1
            'End If

            Bindgrid()
exit_function:
            objUserDetails = Nothing
            objUser = Nothing
            'Catch errExp As CustomException
            '    Common.TrwExp(errExp)
        Catch errExp1 As Exception
            Common.TrwExp(errExp1)
        End Try
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
        If strReturn.ToString <> "" Then
            Return Mid(strReturn, 1, Len(strReturn) - 1)
        Else
            Return Nothing
        End If
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgAddr.SortCommand
        Grid_SortCommand(sender, e)
        dgAddr.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    'Private Sub rdAdd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdAdd.SelectedIndexChanged
    '    If rdAdd.SelectedValue = 1 Then
    '        'dgAddr.Columns(0).Visible = False
    '        cmdReset.Visible = False
    '        txt_Code.Text = ""
    '        txt_City.Text = ""
    '        cbo_State.SelectedValue = ""
    '        trSearch.Style("display") = "none"
    '        btnRemoveSelected.Visible = False
    '    Else
    '        'dgAddr.Columns(0).Visible = True
    '        cmdReset.Visible = True
    '        trSearch.Style("display") = "inline"
    '        btnRemoveSelected.Visible = True
    '    End If
    '    hidAll.Value = rdAdd.SelectedValue
    '    viewstate("resetRadio") = False
    '    Bindgrid(True)
    'End Sub

    Private Sub cmd_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Search.Click
        If ViewState("UserId") Is Nothing Or ViewState("UserId") = "" Then
            Common.NetMsgbox(Me, MsgNoRecord)
            Exit Sub
        End If
        dgAddr.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    '    Private Sub btnRemoveSelected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveSelected.Click
    '        Dim dgItem As DataGridItem
    '        Dim strSQL, strCode As String
    '        Dim chkItem As CheckBox
    '        Dim objComp As New Companies
    '        Dim Query(0) As String
    '        Dim bEmpty As Boolean = True
    '        Dim objUserDetails As New User
    '        Dim objUser As New Users
    '        Dim strLevel As Integer

    '        Try
    '            Select Case viewstate("Role")
    '                Case "SK"
    '                    objUserDetails = objUser.GetUserDetails(viewstate("UserId"), Session("CompanyId"))
    '                    If objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Store_Keeper) Then
    '                        strLevel = 1
    '                    ElseIf objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Second_Level_Receiver) Then
    '                        strLevel = 2
    '                    End If

    '                    'If rdAdd.SelectedValue = 1 Then ' select all
    '                    'bEmpty = False
    '                    'strSQL = "DELETE FROM USERS_LOCATION "
    '                    'strSQL &= "WHERE UL_COY_ID = '" & Session("CompanyId") & "' "
    '                    'strSQL &= "AND UL_USER_ID = '" & viewstate("UserId") & "' "
    '                    'strSQL &= "AND UL_LEVEL = " & strLevel
    '                    'Common.Insert2Ary(Query, strSQL)
    '                    'strSQL = "INSERT INTO USERS_LOCATION(UL_COY_ID, UL_ADDR_CODE, UL_USER_ID, UL_LEVEL) "
    '                    'strSQL &= "SELECT '" & Session("CompanyId") & "', "
    '                    'strSQL &= "AM_ADDR_CODE, '" & viewstate("UserId") & "', " & strLevel & " "
    '                    'strSQL &= "FROM ADDRESS_MSTR WHERE AM_COY_ID = '" & Session("CompanyId") & "' "
    '                    'strSQL &= "AND AM_ADDR_TYPE = 'D' "
    '                    'Common.Insert2Ary(Query, strSQL)
    '                    Else
    '                    'Dim strAddr As String = Dg2String(dgAddr, 1)

    '                    ' ai chu remark on 09/11/2005 start
    '                    ' no need to select at least one before saving
    '                    'If Not checkAtLeastOneItemSelected() Then
    '                    '    If Not objUser.checkAddrSelected(viewstate("UserId"), "SK", straddr, strLevel) Then
    '                    '        Common.NetMsgbox(Me, "Please make at least one selection!", MsgBoxStyle.Information)
    '                    '        GoTo exit_function
    '                    '    End If
    '                    'End If
    '                    ' remark end

    '                    'objuser.checkAddrSelected(viewstate("UserId"), "SK",straddr,strlevel)
    '                    bEmpty = True
    '                    objComp.DelUsrLocation(ViewState("UserId"), strLevel, "")
    '                    'For Each dgItem In dgAddr.Items
    '                    '    chkItem = dgItem.FindControl("chkSelection")
    '                    '    If chkItem.Checked Then
    '                    '        strCode = dgItem.Cells(1).Text
    '                    '        strSQL = "INSERT INTO USERS_LOCATION(UL_COY_ID,UL_ADDR_CODE,UL_USER_ID,UL_LEVEL) " & _
    '                    '               "VALUES('" & Session("CompanyId") & "','" & Common.Parse(strCode) & "','" & viewstate("UserId") & "'," & strLevel & ") "
    '                    '        Common.Insert2Ary(Query, strSQL)
    '                    '        bEmpty = False
    '                    '    End If
    '                    'Next
    '                    End If

    '                Case "B"
    '                    If rdAdd.SelectedValue = 1 Then ' select all
    '                        'strSQL = "DELETE FROM USERS_ADDR "
    '                        'strSQL &= "WHERE UA_COY_ID = '" & Session("CompanyId") & "' "
    '                        'strSQL &= "AND UA_USER_ID = '" & viewstate("UserId") & "' "
    '                        'strSQL &= "AND UA_ADDR_TYPE = '" & viewstate("AddrType") & "' "
    '                        'Common.Insert2Ary(Query, strSQL)
    '                        'strSQL = "INSERT INTO USERS_ADDR (UA_COY_ID, UA_USER_ID, UA_ADDR_TYPE, "
    '                        'strSQL &= "UA_ADDR_CODE, UA_ROLE, UA_VIEW_OPTION) VALUES ("
    '                        'strSQL &= "'" & Session("CompanyId") & "', "
    '                        'strSQL &= "'" & viewstate("UserId") & "', "
    '                        'strSQL &= "'" & viewstate("AddrType") & "', "
    '                        'strSQL &= "'0', 'Buyer', 1)"
    '                        'Common.Insert2Ary(Query, strSQL)
    '                        'bEmpty = False
    '                    Else
    '                        'Dim strAddr As String = Dg2String(dgAddr, 1)

    '                        ' ai chu remark on 09/11/2005 start
    '                        ' no need to select at least one before saving
    '                        'If Not checkAtLeastOneItemSelected() Then
    '                        '    If Not objUser.checkAddrSelected(viewstate("UserId"), "B", straddr, viewstate("AddrType")) Then
    '                        '        Common.NetMsgbox(Me, "Please make at least one selection!", MsgBoxStyle.Information)
    '                        '        GoTo exit_function
    '                        '    End If
    '                        'End If
    '                        ' remark end

    '                        bEmpty = False
    '                        strSQL = "DELETE FROM USERS_ADDR "
    '                        strSQL &= "WHERE UA_COY_ID = '" & Session("CompanyId") & "' "
    '                        strSQL &= "AND UA_USER_ID = '" & ViewState("UserId") & "' "
    '                        strSQL &= "AND UA_ADDR_TYPE = '" & ViewState("AddrType") & "' "
    '                        'strSQL &= "AND (UA_ADDR_CODE IN (" & strAddr & ") "
    '                        'strSQL &= "OR UA_VIEW_OPTION = 1)"
    '                        Common.Insert2Ary(Query, strSQL)

    '                        'For Each dgItem In dgAddr.Items
    '                        '    chkItem = dgItem.FindControl("chkSelection")
    '                        '    If chkItem.Checked Then
    '                        '        strCode = dgItem.Cells(1).Text
    '                        '        strSQL = "INSERT INTO USERS_ADDR (UA_COY_ID, UA_USER_ID, UA_ADDR_TYPE, "
    '                        '        strSQL &= "UA_ADDR_CODE, UA_ROLE, UA_VIEW_OPTION) VALUES ("
    '                        '        strSQL &= "'" & Session("CompanyId") & "', "
    '                        '        strSQL &= "'" & viewstate("UserId") & "', "
    '                        '        strSQL &= "'" & viewstate("AddrType") & "', "
    '                        '        strSQL &= "'" & Common.Parse(strCode) & "', "
    '                        '        strSQL &= "'Buyer', 0)"
    '                        '        Common.Insert2Ary(Query, strSQL)
    '                        '    End If
    '                        'Next

    '                    End If
    '            End Select

    '            If bEmpty = False Then
    '                If Query(0) <> String.Empty Then
    '                    If objDb.BatchExecute(Query) Then
    '                        Common.NetMsgbox(Me, "All address have been removed.", MsgBoxStyle.Information)
    '                    Else
    '                        Common.NetMsgbox(Me, "All address have been removed.", MsgBoxStyle.Information)
    '                    End If
    '                End If
    '            Else
    '                Common.NetMsgbox(Me, "All address have been removed.", MsgBoxStyle.Information)
    '            End If


    '            'If viewstate("OrigAll") = 1 And rdAdd.SelectedValue = 0 Then
    '            '    '//First Load=ALL, change to 'Selected' and click Save Button
    '            '    viewstate("OrigAll") = 0
    '            'ElseIf viewstate("OrigAll") = 0 And rdAdd.SelectedValue = 1 Then
    '            '    '//First Load=Selected, change to 'All' and click Save Button
    '            '    viewstate("OrigAll") = 1
    '            'End If

    '            Bindgrid()
    'exit_function:
    '            objUserDetails = Nothing
    '            objUser = Nothing
    '            'Catch errExp As CustomException
    '            '    Common.TrwExp(errExp)
    '        Catch errExp1 As Exception
    '            Common.TrwExp(errExp1)
    '        End Try
    '    End Sub
    Private Sub cboUser_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboUser.SelectedIndexChanged
        If cboUser.SelectedItem.Text = "---Select---" Then
            ViewState("UserId") = ""
            ViewState("Role") = ""
            Bindgrid(0)
        Else
            ViewState("UserId") = cboUser.SelectedValue
            'If objUsers.IsPurchasing(ViewState("UserId")) Or objUsers.IsBuyer(ViewState("UserId")) Then
            '    ViewState("Role") = "B"
            '    lblAddFor.Visible = False
            '    rbFixedRole.Visible = False
            'Else
            '    ViewState("Role") = "SK"
            '    lblAddFor.Visible = True
            '    rbFixedRole.Visible = True
            'End If
            If objUsers.IsPurchasing(ViewState("UserId")) Or objUsers.IsBuyer(ViewState("UserId")) Then
                If objUsers.IsStoreKeeper(ViewState("UserId")) Then 'Fixed Role= (PO/PM/Buyer) and storekeeper
                    'ViewState("Role") = "SKB"
                    ViewState("Role") = "B"
                    If ViewState("AddrType") = "D" Then
                        Me.rbFixedRole.SelectedValue = "Buyer"
                        lblAddFor.Visible = True
                        lblAddFor1.Visible = True
                        rbFixedRole.Visible = True
                    End If

                Else
                    ViewState("Role") = "B"
                    lblAddFor.Visible = False
                    lblAddFor1.Visible = False
                    rbFixedRole.Visible = False
                End If
             
            Else    'Fixed Role= storekeeper only
                ViewState("Role") = "SK"
                lblAddFor.Visible = False
                lblAddFor1.Visible = False
                rbFixedRole.Visible = False
            End If
            checkaddress(ViewState("UserId"))
            Bindgrid(0)
        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        'If ViewState("AddrType") = "D" Then
        '    Session("w_UserAddr_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '                         "<li><a class=""t_entity_btn"" href=""usSearchUser.aspx?pageid=" & strPageId & """><span>User Account</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                         "<li><a class=""t_entity_btn_selected"" href=""usDeliveryAddr.aspx?addrtype=D&pageid=" & strPageId & """><span>Delivery Address</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                         "<li><a class=""t_entity_btn"" href=""usDeliveryAddr.aspx?addrtype=B&pageid=" & strPageId & """><span>Billing Address</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                       "</ul><div></div></div>"
        'Else
        '    Session("w_UserAddr_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '                        "<li><a class=""t_entity_btn"" href=""usSearchUser.aspx?pageid=" & strPageId & """><span>User Account</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                        "<li><a class=""t_entity_btn"" href=""usDeliveryAddr.aspx?addrtype=D&pageid=" & strPageId & """><span>Delivery Address</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                        "<li><a class=""t_entity_btn_selected"" href=""usDeliveryAddr.aspx?addrtype=B&pageid=" & strPageId & """><span>Billing Address</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                      "</ul><div></div></div>"
        'End If
        If ViewState("AddrType") = "D" Then
            'If Session("Env") <> "FTN" Then 'Non FTN
            '    Session("w_UserAddr_tabs") = "<div class=""t_entity""><ul>" & _
            '                                    "<li><div class=""space""></div></li>" & _
            '                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usSearchUser.aspx", "pageid=" & strPageId) & """><span>User Account</span></a></li>" & _
            '                                    "<li><div class=""space""></div></li>" & _
            '                                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=D&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
            '                                    "<li><div class=""space""></div></li>" & _
            '                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=B&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
            '                                    "<li><div class=""space""></div></li>" & _
            '                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usFinVwDept.aspx", "pageid=" & strPageId) & """><span>Finance Viewing Department</span></a></li>" & _
            '                                    "<li><div class=""space""></div></li>" & _
            '                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usCommodity.aspx", "pageid=" & strPageId) & """><span>Commodity Assignment</span></a></li>" & _
            '                                    "<li><div class=""space""></div></li>" & _
            '                                    "</ul><div></div></div>"
            'Else
            '    Session("w_UserAddr_tabs") = "<div class=""t_entity""><ul>" & _
            '                                    "<li><div class=""space""></div></li>" & _
            '                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usSearchUser.aspx", "pageid=" & strPageId) & """><span>User Account</span></a></li>" & _
            '                                    "<li><div class=""space""></div></li>" & _
            '                                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=D&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
            '                                    "<li><div class=""space""></div></li>" & _
            '                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=B&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
            '                                    "<li><div class=""space""></div></li>" & _
            '                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usCommodity.aspx", "pageid=" & strPageId) & """><span>Commodity Assignment</span></a></li>" & _
            '                                    "<li><div class=""space""></div></li>" & _
            '                                    "</ul><div></div></div>"
            'End If
            Session("w_UserAddr_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usSearchUser.aspx", "pageid=" & strPageId) & """><span>User Account</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=D&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=B&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usFinVwDept.aspx", "pageid=" & strPageId) & """><span>Finance Viewing Department</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usCommodity.aspx", "pageid=" & strPageId) & """><span>Commodity Assignment</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
        Else
            'If Session("Env") <> "FTN" Then 'Non FTN
            '    Session("w_UserAddr_tabs") = "<div class=""t_entity""><ul>" & _
            '                                     "<li><div class=""space""></div></li>" & _
            '                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usSearchUser.aspx", "pageid=" & strPageId) & """><span>User Account</span></a></li>" & _
            '                                     "<li><div class=""space""></div></li>" & _
            '                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=D&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
            '                                     "<li><div class=""space""></div></li>" & _
            '                                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=B&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
            '                                     "<li><div class=""space""></div></li>" & _
            '                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usFinVwDept.aspx", "pageid=" & strPageId) & """><span>Finance Viewing Department</span></a></li>" & _
            '                                     "<li><div class=""space""></div></li>" & _
            '                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usCommodity.aspx", "pageid=" & strPageId) & """><span>Commodity Assignment</span></a></li>" & _
            '                                     "<li><div class=""space""></div></li>" & _
            '                                     "</ul><div></div></div>"
            'Else
            '    Session("w_UserAddr_tabs") = "<div class=""t_entity""><ul>" & _
            '                                     "<li><div class=""space""></div></li>" & _
            '                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usSearchUser.aspx", "pageid=" & strPageId) & """><span>User Account</span></a></li>" & _
            '                                     "<li><div class=""space""></div></li>" & _
            '                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=D&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
            '                                     "<li><div class=""space""></div></li>" & _
            '                                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=B&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
            '                                     "<li><div class=""space""></div></li>" & _
            '                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usCommodity.aspx", "pageid=" & strPageId) & """><span>Commodity Assignment</span></a></li>" & _
            '                                     "<li><div class=""space""></div></li>" & _
            '                                     "</ul><div></div></div>"
            'End If
            Session("w_UserAddr_tabs") = "<div class=""t_entity""><ul>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usSearchUser.aspx", "pageid=" & strPageId) & """><span>User Account</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=D&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=B&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usFinVwDept.aspx", "pageid=" & strPageId) & """><span>Finance Viewing Department</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usCommodity.aspx", "pageid=" & strPageId) & """><span>Commodity Assignment</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
        End If
    End Sub

    Private Sub cmd_Clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Clear.Click
        Dim objGlobal As New AppGlobals
        txt_Code.Text = ""
        txt_City.Text = ""
        'cbo_State.SelectedIndex = 0
        'cbo_State.SelectedItem.Text = "---Select---"
        'cbo_State.SelectedItem.Value = ""
        cbo_Country.SelectedIndex = 0
        objGlobal.FillState(cbo_State, cbo_Country.SelectedItem.Value)
        cbo_State.SelectedItem.Text = "---Select---"
        cbo_State.SelectedItem.Value = ""
        cboUser.SelectedIndex = 0
        Me.cboUser_SelectedIndexChanged(sender, e)

    End Sub

    Private Sub rbFixedRole_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbFixedRole.SelectedIndexChanged
        If Request("rbFixedRole") = "Buyer" Then
            ViewState("Role") = "B"
        Else
            ViewState("Role") = "SK"
        End If
        If cboUser.SelectedItem.Text <> "---Select---" Then
            ViewState("UserId") = cboUser.SelectedValue
            checkaddress(ViewState("UserId"))
            Bindgrid(0)
        End If
    End Sub

    Private Sub cmdRemoveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRemoveAll.Click
        Dim strSQL As String
        Dim chkItem As CheckBox
        Dim Query(0) As String               
        Dim objUserDetails As New User
        Dim objUser As New Users
        Dim strLevel As Integer
        Try
            If ViewState("Role") = "B" Then
                strSQL = "DELETE FROM USERS_ADDR "
                strSQL &= "WHERE UA_COY_ID = '" & Session("CompanyId") & "' "
                strSQL &= "AND UA_USER_ID = '" & ViewState("UserId") & "' "
                strSQL &= "AND UA_ADDR_TYPE = '" & ViewState("AddrType") & "' "
                Common.Insert2Ary(Query, strSQL)
            Else
                objUserDetails = objUser.GetUserDetails(ViewState("UserId"), Session("CompanyId"))
                If objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Store_Keeper) Then
                    strLevel = 1
                ElseIf objUser.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Second_Level_Receiver) Then
                    strLevel = 2
                End If

                strSQL = "DELETE FROM USERS_LOCATION "
                strSQL &= "WHERE UL_COY_ID = '" & Session("CompanyId") & "' "
                strSQL &= "AND UL_USER_ID = '" & ViewState("UserId") & "' "
                strSQL &= "AND UL_LEVEL = " & strLevel
                Common.Insert2Ary(Query, strSQL)
            End If

            If Query(0) <> String.Empty Then
                If objDb.BatchExecute(Query) Then
                    Common.NetMsgbox(Me, "Record Saved.", MsgBoxStyle.Information)
                Else
                    Common.NetMsgbox(Me, "Record not saved.", MsgBoxStyle.Information)
                End If
            End If

            Bindgrid()
exit_function:
        Catch errExp1 As Exception
            Common.TrwExp(errExp1)
        End Try
    End Sub

    Private Sub rbtnType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnType.SelectedIndexChanged
        Bindgrid()
    End Sub

    Private Sub cmdReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Bindgrid()
    End Sub

    Private Sub cbo_Country_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Country.SelectedIndexChanged
        Dim objGlobal As New AppGlobals
        objGlobal.FillState(cbo_State, cbo_Country.SelectedItem.Value)
    End Sub
End Class
