
Imports AgoraLegacy
Imports eAdmin
Imports eProcure.Component
Imports eProcurement
Imports System.Drawing

Public Class usSearchUserFTN
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSearch As System.Web.UI.WebControls.TextBox
    Protected WithEvents imbSearch As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmbGroup As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents dgUser As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents totrows As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    'Protected WithEvents txtUserID As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtUserName As System.Web.UI.WebControls.TextBox
    'Protected WithEvents lbl_result As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    'Protected WithEvents cmdUnlock As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdActivate As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdDeactive As System.Web.UI.WebControls.Button
    'Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    'Protected WithEvents lblUL As System.Web.UI.WebControls.Label
    'Protected WithEvents lblUserLicense As System.Web.UI.WebControls.Label
    'Protected WithEvents lblAU As System.Web.UI.WebControls.Label
    'Protected WithEvents lblActiveUser As System.Web.UI.WebControls.Label
    'Protected WithEvents lblLegend As System.Web.UI.WebControls.Label
    'Protected WithEvents lblAction As System.Web.UI.WebControls.Label
    Private ds As DataSet
    Dim dvwPackage As DataView
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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdAdd.Enabled = False
        cmdUnlock.Enabled = False
        cmdActivate.Enabled = False
        cmdDeactive.Enabled = False
        cmdDelete.Enabled = False
        cmdModify.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdModify)
        alButtonList.Add(cmdUnlock)
        alButtonList.Add(cmdActivate)
        alButtonList.Add(cmdDeactive)
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdDelete)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        '//additional checking
        If intPageRecordCnt > 0 Then
            cmdDelete.Enabled = blnCanDelete
            cmdModify.Enabled = blnCanUpdate
            cmdUnlock.Enabled = blnCanUpdate
            cmdActivate.Enabled = blnCanUpdate
            cmdDeactive.Enabled = blnCanUpdate
            '//mean Enable, can't use button.Enabled because this is a HTML button
            cmdReset.Disabled = False
        Else
            cmdUnlock.Enabled = False
            cmdActivate.Enabled = False
            cmdDeactive.Enabled = False
            cmdDelete.Enabled = False
            cmdModify.Enabled = False
            'cmdReset.Disabled = True
        End If
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(dgUser)

        If Not Page.IsPostBack Then
            Dim objUser As New Users
            If objUser.getCompanyType() = "VENDOR" Then
                Me.dgUser.Columns(3).Visible = False
                Me.dgUser.Columns(4).Visible = False
                ViewState("Side") = "VENDOR"
            Else
                ViewState("Side") = "BUYER"
                Dim intDept As Integer
                Dim objDBAccess As New EAD.DBCom
                intDept = objDBAccess.GetCount("company_dept_mstr", " where cdm_coy_id = '" & Session("CompanyID") & "' and cdm_deleted = 'N'")
                If intDept = 0 Then Me.dgUser.Columns(4).Visible = False
                'If Session("Env") <> "FTN" Then 'Non FTN
                '    Me.lblAction.Text = "<b>=></b> Step 1: Create, modify or delete User Account.<br />Step 2: Assign Delivery Address to selected User Account.<br>Step 3: Assign Billing Address to selected User Account.<br>Step 4: Assign Finance Viewing Department to selected User Account.<br>Step 5: Assign Commodity Type to selected User Account."

                'Else
                '    Me.lblAction.Text = "<b>=></b> Step 1: Create, modify or delete User Account.<br />Step 2: Assign Delivery Address to selected User Account.<br>Step 3: Assign Billing Address to selected User Account.<br>Step 4: Assign Commodity Type to selected User Account."

                'End If
                Me.lblAction.Text = "<b>=></b> Step 1: Create, modify or delete User Account.<br />Step 2: Assign Delivery Address to selected User Account.<br>Step 3: Assign Billing Address to selected User Account.<br>Step 4: Assign Commodity Type to selected User Account."
            End If
            GenerateTab()
            dgUser.CurrentPageIndex = 0
            Bindgrid()
        End If

        ViewState("pageid") = Request.Params("pageid")

        cmdActivate.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','activate');")
        cmdDeactive.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','deactive');")
        cmdUnlock.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','unlock');")
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")

        lblAU.Visible = False
        lblActiveUser.Visible = False
        lblUL.Visible = False
        lblUserLicense.Visible = False

        'Bindgrid()
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim strUserID As String
        Dim strUserName As String

        strUserID = Me.txtUserID.Text
        strUserName = Me.txtUserName.Text

        Dim objUsers As New Users
        Dim objUserRoles As New UserRoles
        Dim ds As DataSet = New DataSet
        Dim strUserRoles As String

        'Michelle (16/5/2012) - Issue 1614,1615
        'ds = objUsers.SearchUser(strUserID, strUserName, False)
        'If ViewState("Side") = "VENDOR" Then
        '    ds = objUsers.SearchUser(strUserID, strUserName, False, True)
        'ElseIf Session("Env") = "FTN" And ViewState("Side") = "BUYER" And objUserRoles.get_UserRole().IndexOf(",") <> -1 Then             'Check whether the super admin is tied to other groups
        '    ds = objUsers.SearchUser(strUserID, strUserName, False, True)
        'Else
        '    ds = objUsers.SearchUser(strUserID, strUserName, False)
        'End If

        If ViewState("Side") = "VENDOR" Then
            ds = objUsers.SearchUser(strUserID, strUserName, False, True)
        ElseIf ViewState("Side") = "BUYER" And objUserRoles.get_UserRole().IndexOf(",") <> -1 Then             'Check whether the super admin is tied to other groups
            ds = objUsers.SearchUser(strUserID, strUserName, False, True)
        Else
            ds = objUsers.SearchUser(strUserID, strUserName, False)
        End If

        Dim dvViewDept As DataView
        dvViewDept = ds.Tables(0).DefaultView
        dvViewDept.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewDept.Sort += " DESC"

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If ViewState("action") = "del" Then
            If dgUser.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dgUser.PageSize = 0 Then
                dgUser.CurrentPageIndex = dgUser.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("SSO"))
        Dim objPack As New tx123_COMP.package
        ds = objPack.getAllPackageDetails(Session("CompanyId"))
        lblLegend.Text = ""
        Dim i As Integer
        If Not ds Is Nothing Then
            dvwPackage = ds.Tables(0).DefaultView
            ViewState("AppCount") = dvwPackage.Count
            If ViewState("AppCount") = 1 Then
                lblLegend.Text = ""
            Else
                For i = 0 To dvwPackage.Count - 1
                    lblLegend.Text &= "<BR><font class=label><font style='BACKGROUND-COLOR: " & dvwPackage.Item(i)("AP_COLOR") & "' color=" & dvwPackage.Item(i)("AP_COLOR") & ">12345</font> " & ds.Tables(0).Rows(i)("AP_APP_NAME") & " User Group</font>"
                Next
            End If
        End If

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dgUser, dvViewDept)
            cmdDelete.Enabled = True
            cmdModify.Enabled = True
            cmdUnlock.Enabled = True
            cmdActivate.Enabled = True
            cmdDeactive.Enabled = True
            cmdUnlock.Enabled = True
            '//mean Enable, can't use button.Enabled because this is a HTML button
            cmdReset.Disabled = False
            dgUser.DataSource = dvViewDept
            '*****************meilai 12/1/2005 display user license and active user*********************
            lblAU.Visible = True
            lblActiveUser.Visible = True
            lblUL.Visible = True
            lblUserLicense.Visible = True
            lblUserLicense.Text = objUsers.getUserLicense(False)
            lblActiveUser.Text = objUsers.getActiveUser(False)
            '************************************************************************
            dgUser.DataBind()
        Else
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
            'cmdReset.Disabled = True
            Common.NetMsgbox(Me, MsgNoRecord)
            dgUser.DataBind()
        End If

        ViewState("PageCount") = dgUser.PageCount
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

    Private Sub dgUser_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgUser.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            'to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim lnkUserID As HyperLink
            lnkUserID = e.Item.FindControl("lnkUserID")
            lnkUserID.NavigateUrl = dDispatcher.direct("Users", "usUser.aspx", "pageid=" & ViewState("pageid") & "&mode=modify&userid=" & dv("UM_USER_ID"))
            lnkUserID.Text = dv("UM_USER_ID")

            'to add a repeater to display user group
            Dim rpt As Repeater
            rpt = e.Item.FindControl("sub")
            Dim objUser As New Users
            rpt.DataSource = objUser.getUserGrpByEmployee(CType(e.Item.Cells(1).Controls(1), HyperLink).Text, dvwPackage, False)
            'AddHandler rpt.ItemDataBound, AddressOf rpt_ItemDataBound
            rpt.DataBind()


            'If dv("App") = "1" Then
            '    rpt = e.Item.FindControl("sub")
            '    rpt.DataSource = GetChildDatasource(CType(e.Item.Cells(1).Controls(1), HyperLink).Text)
            '    rpt.DataBind()

            'Else ' usergroup for other package no need to be displayed here
            '    e.Item.Cells(3).Text = ""
            'End If

            e.Item.Cells(3).VerticalAlign = VerticalAlign.Middle
            If e.Item.Cells(6).Text = "S" Then
                e.Item.Cells(6).Text = "Y"
                e.Item.Cells(6).ForeColor = Color.Red
            End If
        End If
    End Sub

    'Private Sub rpt_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
    '    If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
    '        If LCase(e.Item.DataItem("UU_APP_PKG")) = "eprocure" Then
    '            e.Item.DataItem("color1") = " 1 "
    '            e.Item.DataItem("color2") = " 2 "
    '        End If
    '    End If
    'End Sub

    'Private Function GetChildDatasource(ByVal pName As String) As DataView
    '    Dim strSQl As String
    '    Dim i As Integer
    '    Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("SSO"))

    '    If viewstate("AppCount") = 1 Then
    '        For i = 0 To dvwPackage.Count - 1
    '            If i = dvwPackage.Count - 1 Then
    '                strSQl &= reBuildChildDataSource(pName, dvwPackage.Item(i)("AP_COLOR"), dvwPackage.Item(i)("AP_APP_ID"))
    '            Else
    '                strSQl &= reBuildChildDataSource(pName, dvwPackage.Item(i)("AP_COLOR"), dvwPackage.Item(i)("AP_APP_ID")) & " UNION "
    '            End If
    '        Next
    '    End If

    '    Return objDb.GetView(strSQl)
    '    objDb = Nothing
    'End Function

    'Private Function reBuildChildDataSource(ByVal pName As String, ByVal strColor As String, ByVal strApp As String) As String
    '    Dim strsql As String
    '    strsql = "SELECT *, '<font color=" & strColor & ">' as color1, '</font>' as color2 from USERS_USRGRP LEFT JOIN USER_GROUP_MSTR ON UU_USRGRP_ID=UGM_USRGRP_ID "
    '    strsql &= "AND UGM_APP_PKG = UU_APP_PKG "
    '    strsql &= "WHERE UU_USER_ID='" & Common.Parse(pName) & "' "
    '    strsql &= "AND UU_COY_ID='" & Session("CompanyId") & "' "
    '    strsql &= "AND UU_APP_PKG = '" & strApp & "' "
    '    Return strsql
    'End Function

    Sub dtgDept_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dgUser.PageIndexChanged
        dgUser.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim objUser As New Users
        If objUser.IsExceedUserLimit(Session("CompanyId")) = False Then
            Me.Response.Redirect(dDispatcher.direct("Users", "usUser.aspx", "pageid=" & ViewState("pageid") & "&mode=add"))
        Else
            Common.NetMsgbox(Me, objUser.Message, MsgBoxStyle.Information)
            Bindgrid()
        End If
        objUser = Nothing
    End Sub

    Private Sub cmdModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim strUserid As String
        Dim chkItem As CheckBox
        For Each dgItem In dgUser.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                strUserid = CType(dgItem.FindControl("lnkUserID"), HyperLink).Text
                Me.Response.Redirect(dDispatcher.direct("Users", "usUser.aspx", "pageid=" & ViewState("pageid") & "&mode=modify&userid=" & strUserid))
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
                    objUser.SuspendAc(strUserid, Session("companyId"), 0)
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
                    If objUser.ActivateUserAccount(strUserid, True, False) = False Then
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
                    objUser.ActivateUserAccount(strUserid, False, False)
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
                If objUser.DelUser(LCase(strUserId), False) = False Then
                    Exit For
                End If
            End If
        Next
        Common.NetMsgbox(Me, objUser.Message, MsgBoxStyle.Information)
        ViewState("action") = "del"
        Bindgrid()
        objUser = Nothing
    End Sub

    Private Sub dgUser_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgUser.SortCommand
        Grid_SortCommand(source, e)
        dgUser.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        'If ViewState("Side") = "BUYER" Then
        '    Session("w_SearchUser_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn_selected"" href=""usSearchUser.aspx?pageid=" & strPageId & """><span>User Account</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""usDeliveryAddr.aspx?addrtype=D&pageid=" & strPageId & """><span>Delivery Address</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""usDeliveryAddr.aspx?addrtype=B&pageid=" & strPageId & """><span>Billing Address</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                  "</ul><div></div></div>"
        'Else
        '    Session("w_SearchUser_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn_selected"" href=""usSearchUser.aspx?pageid=" & strPageId & """><span>User Account</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        ' "</ul><div></div></div>"

        'End If
        If ViewState("Side") = "BUYER" Then
            'If Session("Env") <> "FTN" Then 'Non FTN
            '    Session("w_SearchUser_tabs") = "<div class=""t_entity""><ul>" & _
            '               "<li><div class=""space""></div></li>" & _
            '                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Users", "usSearchUser.aspx", "pageid=" & strPageId) & """><span>User Account</span></a></li>" & _
            '                "<li><div class=""space""></div></li>" & _
            '                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "addrtype=D&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
            '                "<li><div class=""space""></div></li>" & _
            '                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "addrtype=B&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
            '                "<li><div class=""space""></div></li>" & _
            '                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usFinVwDept.aspx", "pageid=" & strPageId) & """><span>Finance Viewing Department</span></a></li>" & _
            '                "<li><div class=""space""></div></li>" & _
            '                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usCommodity.aspx", "pageid=" & strPageId) & """><span>Commodity Assignment</span></a></li>" & _
            '                "<li><div class=""space""></div></li>" & _
            '                "</ul><div></div></div>"
            'Else    'FTN
            '    Session("w_SearchUser_tabs") = "<div class=""t_entity""><ul>" & _
            '               "<li><div class=""space""></div></li>" & _
            '                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Users", "usSearchUser.aspx", "pageid=" & strPageId) & """><span>User Account</span></a></li>" & _
            '                "<li><div class=""space""></div></li>" & _
            '                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "addrtype=D&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
            '                "<li><div class=""space""></div></li>" & _
            '                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "addrtype=B&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
            '                "<li><div class=""space""></div></li>" & _
            '                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usCommodity.aspx", "pageid=" & strPageId) & """><span>Commodity Assignment</span></a></li>" & _
            '                "<li><div class=""space""></div></li>" & _
            '                "</ul><div></div></div>"
            'End If
            Session("w_SearchUser_tabs") = "<div class=""t_entity""><ul>" & _
                   "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Users", "usSearchUser.aspx", "pageid=" & strPageId) & """><span>User Account</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "addrtype=D&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "addrtype=B&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usCommodity.aspx", "pageid=" & strPageId) & """><span>Commodity Assignment</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
        Else
            Session("w_SearchUser_tabs") = "<div class=""t_entity""><ul>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Users", "usSearchUser.aspx", "pageid=" & strPageId) & """><span>User Account</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
         "</ul><div></div></div>"

        End If
    End Sub

End Class


