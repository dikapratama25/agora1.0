Imports AgoraLegacy
Imports eProcure.Component


Public Class IPPUserGroup
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objApp As New ApprWorkFlow
    Dim strGrpName As String

    Protected WithEvents cmd_search As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear1 As System.Web.UI.WebControls.Button
    Protected WithEvents lbl_add_mod As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txt_add_mod As System.Web.UI.WebControls.TextBox
  
    Protected WithEvents cmd_save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear As System.Web.UI.WebControls.Button
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label

    Protected WithEvents dtgApp As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_delete As System.Web.UI.WebControls.Button
    Protected WithEvents Hide_Add2 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Reset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lblSymbol As System.Web.UI.WebControls.Label


    Protected WithEvents Label2 As System.Web.UI.WebControls.Label


    Protected WithEvents trGrpType As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents trhid As System.Web.UI.HtmlControls.HtmlTableRow

    Protected WithEvents lbl1 As System.Web.UI.WebControls.Label




    Dim ds As DataSet

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Enum AWEnum
        AWGrpName = 1
        AWUserID = 2
        AWBRCode = 3
        AWBRDesc = 4
        AWCompID = 5
        AWCCCode = 6
        AWCCDesc = 7
        AWIndex = 8
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdAdd.Enabled = False
        cmd_delete.Enabled = False
        cmdModify.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdModify)
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmd_delete)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        '//additional checking
        If intPageRecordCnt > 0 Then
            cmd_delete.Enabled = blnCanDelete
            cmdModify.Enabled = blnCanUpdate
            '//mean Enable, can't use button.Enabled because this is a HTML button
            'cmdReset.Disabled = False
        Else
            cmd_delete.Enabled = False
            cmdModify.Enabled = False
            'cmdReset.Disabled = True
        End If
        alButtonList.Clear()

        'Zulham 08/05/2018 - PAMB
        If Session("CompanyID").ToString.ToUpper = "PAMB" Then
            lblAction.Text = "<b>=></b> Step 1: Create, delete or modify User Group.<br/>Step 2: Assign Cost Center to the User Group.<br>Step 3: Assign User to the User Group."
        End If

    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objUser As New Users
        '-----New code Added By Praveen for Cbotype on 02.08.2007.It Is repeating
        'when search button fired so it shipped to under the if Ispostback condition
        'Call CheckBuyer()
        '-----End
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgApp)
        If Not IsPostBack Then
            'bindDdl()
            GenerateTab()
            cmd_search.Enabled = True
            cmd_clear1.Enabled = True
            cmd_delete.Enabled = False
            cmdModify.Enabled = False
            cmd_Reset.Disabled = True
            Hide_Add2.Style("display") = "none"
            '-----New code Added By Praveen for Cbotype on 07.08.2007
            'Call CheckBuyer()
            dtgApp.CurrentPageIndex = 0
            ViewState("SortAscending") = "yes"
            ViewState("SortExpression") = "IUM_GRP_NAME"
            Call bindgridApp(0)
            '-----End

        End If

        'cboType.Attributes.Add("onchange", "DisplayConsolidator();")
        cmd_delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        

    End Sub


    Function bindgridApp(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim ds As New DataSet
        Dim strValGroup As String
        Dim objIPP As New IPP
        strValGroup = Me.txtUserGroup.Text

        ds = objIPP.GetIPPUserGroup(strValGroup)

        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = viewstate("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"

        'If ViewState("action") = "del" Then
        If dtgApp.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgApp.PageSize = 0 Then
            dtgApp.CurrentPageIndex = dtgApp.CurrentPageIndex - 1
            'ViewState("action") = ""
        End If
        'End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgApp, dvViewSample)
            cmd_delete.Enabled = True
            cmdModify.Enabled = True
            cmd_Reset.Disabled = False
            dtgApp.DataSource = dvViewSample
            dtgApp.DataBind()

        Else
            cmd_delete.Enabled = False
            cmdModify.Enabled = False
            cmd_Reset.Disabled = True
            dtgApp.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ' add for above checking
        ViewState("PageCount") = dtgApp.PageCount

    End Function

    Public Sub MyDataGrid_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgApp.CurrentPageIndex = e.NewPageIndex
        bindgridApp(0)
    End Sub
    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgApp.CurrentPageIndex = 0
        bindgridApp(0, True)
    End Sub

    Private Sub cmd_clear1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_clear1.Click
        txtUserGroup.Text = ""
    End Sub

    Private Sub dtgApp_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgApp.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim chk As CheckBox
            Dim intTotalCell, intLoop, intLoop2, intLoop3 As Integer
            chk = e.Item.FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            'Dim intLoop As Integer
            Dim ds, ds2, ds3 As New DataSet
            Dim objIPP As New IPP
            Dim lblUserName As Label = e.Item.Cells(2).FindControl("lblUserName")
            Dim lblBranchCode As Label = e.Item.Cells(3).FindControl("lblBranchCode")
            Dim lblCostCenter As Label = e.Item.Cells(6).FindControl("lblCostCenter")

            Dim lblBranchCodeDesc As Label = e.Item.Cells(4).FindControl("lblBranchCodeDesc")
            Dim lblBRCoyID As Label = e.Item.Cells(5).FindControl("lblBRCoyID")
            Dim lblCostCenterDesc As Label = e.Item.Cells(7).FindControl("lblCostCenterDesc")

            Dim val As String = e.Item.Cells(8).Text

            ds = objIPP.GetIPPUserGroup2(val)
            ds2 = objIPP.GetIPPUserGroup3(val)
            ds3 = objIPP.GetIPPUserGroup4(val)

            If Not ds Is Nothing Then
                For intLoop = 0 To ds.Tables(0).Rows.Count - 1
                    If intLoop = 0 Then
                        lblBranchCode.Text &= ds.Tables(0).Rows(intLoop).Item("IUB_BRANCH_CODE")
                        lblBranchCodeDesc.Text &= ds.Tables(0).Rows(intLoop).Item("cbm_branch_name")
                        lblBRCoyID.Text &= ds.Tables(0).Rows(intLoop).Item("IUB_BR_COY_ID")
                    Else
                        lblBranchCode.Text &= "<br>"
                        lblBranchCode.Text &= ds.Tables(0).Rows(intLoop).Item("IUB_BRANCH_CODE")
                        lblBranchCodeDesc.Text &= "<br>"
                        lblBranchCodeDesc.Text &= ds.Tables(0).Rows(intLoop).Item("cbm_branch_name")
                        lblBRCoyID.Text &= "<br>"
                        lblBRCoyID.Text &= ds.Tables(0).Rows(intLoop).Item("IUB_BR_COY_ID")
                    End If
                Next
            End If

            'Zulham 10072018 - PAMB
            If Session("CompanyID").ToString.ToUpper = "PAMB" Then lblBRCoyID.Text = Session("CompanyID").ToString.ToUpper

            If Not ds2 Is Nothing Then
                For intLoop2 = 0 To ds2.Tables(0).Rows.Count - 1
                    If intLoop2 = 0 Then
                        lblCostCenter.Text &= ds2.Tables(0).Rows(intLoop2).Item("IUC_CC_CODE")
                        lblCostCenterDesc.Text &= ds2.Tables(0).Rows(intLoop2).Item("cc_cc_desc")
                    Else
                        lblCostCenter.Text &= "<br>"
                        lblCostCenter.Text &= ds2.Tables(0).Rows(intLoop2).Item("IUC_CC_CODE")
                        lblCostCenterDesc.Text &= "<br>"
                        lblCostCenterDesc.Text &= ds2.Tables(0).Rows(intLoop2).Item("cc_cc_desc")
                    End If

                Next

            End If

            If Not ds3 Is Nothing Then
                For intLoop3 = 0 To ds3.Tables(0).Rows.Count - 1
                    If intLoop3 = 0 Then
                        lblUserName.Text &= ds3.Tables(0).Rows(intLoop3).Item("UM_USER_NAME")
                    Else
                        lblUserName.Text &= "<br>"
                        lblUserName.Text &= ds3.Tables(0).Rows(intLoop3).Item("UM_USER_NAME")
                    End If
                Next
            End If

        End If
    End Sub

    Private Sub cmd_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_clear.Click
        If hidMode.Value = "a" Then
            txt_add_mod.Text = ""

        Else
            txt_add_mod.Text = ViewState("oldvalue")
           
        End If
    End Sub

    Private Sub cmd_delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_delete.Click
        '//To prevent "No Record Found" msg
        txtUserGroup.Text = ""

        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim strgrpName, strconsol, strlev, strO, strAO, strBuyer, strIndex As String
        Dim i As Integer = 0
        Dim strFailDept As String
        Dim objIPP As New IPP

        For Each dgItem In dtgApp.Items

            chk = dgItem.FindControl("chkSelection")

            strIndex = dgItem.Cells.Item(AWEnum.AWIndex).Text

            If chk.Checked Then
                'If objApp.AttachedToDept(strIndex) Then
                '    strFailDept &= dgItem.Cells.Item(AWEnum.AWGrpName).Text & ","
                'Else
                'objApp.DelAppWorkFlow(strIndex)
                objIPP.DelIPPUserGroup(strIndex)
                'End If
            End If
            ' i = i + 1
        Next

        'If strFailDept <> "" Then
        '    strFailDept = strFailDept.Replace(",", vbCrLf)
        '    Common.NetMsgbox(Me, "Unable to delete the workflow group(s).", MsgBoxStyle.Exclamation)

        'Else
        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)


        'ViewState("action") = "del"
        bindgridApp(0)
    End Sub

    Private Sub cmd_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_search.Click
        dtgApp.CurrentPageIndex = 0
        Call bindgridApp(0)
    End Sub

    Private Sub dtgApp_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgApp.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgApp, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")

            'Zulham 08/05/2018 - PAMB
            If Session("CompanyID").ToString.ToUpper = "PAMB" Then
                e.Item.Cells(3).Visible = False
                e.Item.Cells(4).Visible = False
            End If

        End If

    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        '// to unset the value in the combo box

        Hide_Add2.Style("display") = "inline"
        'validate_grp_name.Enabled = True
        txt_add_mod.Text = ""
        Me.lbl_add_mod.Text = "add"
        cmd_clear.Text = "Clear"
        hidMode.Value = "a"

    End Sub

    Sub view(ByVal selected As String, ByVal strvalcboconsol As String)
        Dim objPersonal As New PersonalSetting
        Dim strmsg As String
        Dim intmsgno As Integer
        Dim strRedirect As String
        Dim strvalchk As String
        Dim objIPP As New IPP


        If selected = "add" Then
            Me.lbl_add_mod.Text = "add"
            '//// TO CATER DIFFERENT SITUATION during ADD
  
            'intmsgno = objApp.AddAppWorkFlow(txt_add_mod.Text, strvalcboconsol, cboType.SelectedValue, ddlDept.SelectedItem.Value)
            intmsgno = objIPP.AddIPPUserGroup(txt_add_mod.Text, lbl_add_mod.Text)
            '////

            Select Case intmsgno
                Case WheelMsgNum.Save
                   
                    Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
                Case WheelMsgNum.Duplicate
                    strmsg = MsgRecordDuplicate                   
                    Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
                Case WheelMsgNum.NotSave
                    strmsg = MsgRecordNotSave
                    Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
            End Select

        ElseIf selected = "mod" Then
            Me.lbl_add_mod.Text = "modify"
            '//// TO CATER DIFFERENT SITUATION during MODIFY
           
            'intmsgno = objApp.modAppWorkFlow(hidIndex.Value, txt_add_mod.Text, ViewState("oldvalue2"), cboType.SelectedValue, ViewState("oldvalue"))
            intmsgno = objIPP.UpdateIPPUserGroup(hidIndex.Value, txt_add_mod.Text, lbl_add_mod.Text, ViewState("oldvalue"))
            '////
            Select Case intmsgno
                Case WheelMsgNum.Save
                    'strRedirect = "AppGrpAsg.aspx?grp=" & Server.UrlEncode(txt_add_mod.Text) & "&chk=" & strvalchk & "&apprType=" & cboType.SelectedValue & "&frm=AW&pageid=" & strPageId
                    'strmsg = MsgRecordSave
                    'txt_add_mod.Text = ""
                    'ViewState("oldvalue") = ""
                    'viewstate("oldvalue2") = ""
                    'Common.SelDdl(viewstate("oldvalue2"), cboConsol, True, False)
                    'Hide_Add2.Style("display") = "none"
                    'Common.NetPrompt(Me, MsgRecordSave & """& vbCrLf & ""Proceed to Approval Group Assignment?", strRedirect)
                    Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
                Case WheelMsgNum.Duplicate
                    strmsg = MsgRecordDuplicate
                    txt_add_mod.Text = ViewState("oldvalue")                
                    Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
                Case WheelMsgNum.NotSave
                    strmsg = MsgRecordNotSave
                    Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
            End Select
        End If
    End Sub

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click

        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim strlistindex As String
        Dim i As Integer
        i = 0

        Hide_Add2.Style("display") = "inline"
        cmd_clear.Text = "Reset"
        hidMode.Value = "m"
        For Each dgItem In dtgApp.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                hidIndex.Value = dgItem.Cells(AWEnum.AWIndex).Text
                txt_add_mod.Text = dgItem.Cells(AWEnum.AWGrpName).Text

                viewstate("oldvalue") = dgItem.Cells(AWEnum.AWGrpName).Text
                viewstate("oldvalue2") = dgItem.Cells.Item(AWEnum.AWUserID).Text
                'ViewState("oldvalue3") = dgItem.Cells.Item(AWEnum.AWType).Text
                'ViewState("oldvalue4") = dgItem.Cells.Item(AWEnum.AWDeptcode).Text
                'cboType.SelectedValue = dgItem.Cells.Item(AWEnum.AWType).Text
                '//check for consolidator,modify by Moo
                '&nbsp;"
                            
                Exit For
            End If
            i = i + 1
        Next
        'If Session("Env") = "FTN" Then
        '    Me.dtgApp.Columns(2).Visible = False
        'End If

        Me.lbl_add_mod.Text = "modify"

        cmdModify.Enabled = True
    End Sub

    Private Sub cmd_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_save.Click
        '//To prevent "No Record Found" msg
        'ViewState("oldvalue") = txt_add_mod.Text
        txtUserGroup.Text = ""

        'If strvalcboconsol = "" Then
        If hidMode.Value = "a" Then
            Me.view("add", "")
        ElseIf hidMode.Value = "m" Then
            Me.view("mod", "")
        End If
        'Else

        '    If hidMode.Value = "a" Then
        '        Me.view("add", strvalcboconsol)
        '    ElseIf hidMode.Value = "m" Then
        '        Me.view("mod", strvalcboconsol)
        '    End If

        'End If

        bindgridApp(0)
    End Sub



    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Hide_Add2.Style("display") = "none"
        hidMode.Value = ""

    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        'Zulham 08/05/2018 - PAMB
        If Session("CompanyID").ToString.ToUpper = "PAMB" Then
            Session("w_ApprWF_tabs") = "<div class=""t_entity""><ul>" &
            "<li><div class=""space""></div></li>" &
             "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPPAdmin", "IPPUserGroup.aspx", "pageid=" & strPageId) & """><span>User Group</span></a></li>" &
             "<li><div class=""space""></div></li>" &
             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPPAdmin", "IPPUsrGrpCostCenterAsg.aspx", "pageid=" & strPageId) & """><span>Cost Center Assignment</span></a></li>" &
             "<li><div class=""space""></div></li>" &
             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPPAdmin", "IPPUsrGrpUserAsg.aspx", "pageid=" & strPageId) & """><span>User Assignment</span></a></li>" &
             "<li><div class=""space""></div></li>" &
             "</ul><div></div></div>"
        Else
            Session("w_ApprWF_tabs") = "<div class=""t_entity""><ul>" &
            "<li><div class=""space""></div></li>" &
             "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPPAdmin", "IPPUserGroup.aspx", "pageid=" & strPageId) & """><span>User Group</span></a></li>" &
             "<li><div class=""space""></div></li>" &
             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPPAdmin", "IPPUsrGrpBranchAsg.aspx", "pageid=" & strPageId) & """><span>Branch Assignment</span></a></li>" &
             "<li><div class=""space""></div></li>" &
             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPPAdmin", "IPPUsrGrpCostCenterAsg.aspx", "pageid=" & strPageId) & """><span>Cost Center Assignment</span></a></li>" &
             "<li><div class=""space""></div></li>" &
             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPPAdmin", "IPPUsrGrpUserAsg.aspx", "pageid=" & strPageId) & """><span>User Assignment</span></a></li>" &
             "<li><div class=""space""></div></li>" &
             "</ul><div></div></div>"
        End If

    End Sub
End Class
