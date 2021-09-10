Imports AgoraLegacy
Imports eProcure.Component


Public Class SoftwareApp
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objAdm As New Admin
    ' Dim strGrpName As String
    Protected WithEvents txtSWSearch As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmd_search As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear1 As System.Web.UI.WebControls.Button
    Protected WithEvents lbl_add_mod As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txt_add_mod As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmd_save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear As System.Web.UI.WebControls.Button
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    '   Protected WithEvents validate_grp_name As System.Web.UI.WebControls.RequiredFieldValidator
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
    'Protected WithEvents lblRed As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents rfvType As System.Web.UI.WebControls.RequiredFieldValidator
    '  Protected WithEvents trGrpType As System.Web.UI.HtmlControls.HtmlTableRow
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

    'Public Enum AWEnum
    '    AWGrpName = 1
    '    AWConsol = 2
    '    AWAAO = 3
    '    AWBuyer = 4
    '    AWIndex = 5
    '    AWUserID = 6
    '    AWType = 7
    'End Enum

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
        ''//additional checking
        'If hidMode.Value <> "m" Then
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
        'End If
        alButtonList.Clear()
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
            ViewState("Side") = Request.Params("side")
            GenerateTab()
            cmd_search.Enabled = True
            cmd_clear1.Enabled = True
            cmd_delete.Enabled = False
            cmdModify.Enabled = False
            cmd_Reset.Disabled = True
            Hide_Add2.Style("display") = "none"
            bindgridApp()

            '-----New code Added By Praveen for Cbotype on 07.08.2007
            'Call CheckBuyer()
            '-----End
        End If
        cmd_delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
    End Sub
    'Public Sub CheckBuyer()
    '    Dim tDS As DataSet
    '    Dim strsql As String
    '    Dim str As String
    '    Dim str1 As String
    '    strsql = "Select CM_COY_TYPE  from COMPANY_MSTR where CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"

    '    tDS = objDb.FillDs(strsql)
    '    If tDS.Tables(0).Rows.Count > 0 Then
    '        str = tDS.Tables(0).Rows(0).Item("CM_COY_TYPE")
    '    End If
    '    strsql = "Select CM_INV_APPR from COMPANY_MSTR where CM_COY_TYPE ='" & str & "' and CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
    '    tDS = objDb.FillDs(strsql)
    '    If tDS.Tables(0).Rows.Count > 0 Then
    '        str1 = tDS.Tables(0).Rows(0).Item("CM_INV_APPR")
    '    End If
    '    If str1 = "N" Then
    '        cboType.Items.Add("PO")
    '        cboType.Items.Add("PR")
    '    ElseIf str1 <> "N" Then
    '        cboType.Items.Add("PO")
    '        cboType.Items.Add("PR")
    '        cboType.Items.Add("INV")

    '    End If
    'End Sub
    'Public Function BindData()
    '    Dim dvcustom As DataView
    '    dvcustom = objApp.bindcboconsol()

    '    cboConsol.Items.Clear()
    '    If Not dvcustom Is Nothing Then
    '        Dim cbolist As New ListItem
    '        Common.FillDdl(cboConsol, "two", "UM_USER_ID", dvcustom)
    '        cbolist.Value = ""
    '        cbolist.Text = "---Select---"
    '        cboConsol.Items.Insert(0, cbolist)
    '    End If

    'End Function

    Function bindgridApp(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim ds As New DataSet

        ds = objAdm.getSoftware(Replace(Me.txtSWSearch.Text, "'", ""))

        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"


        If viewstate("action") = "del" Then
            If dtgApp.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgApp.PageSize = 0 Then
                dtgApp.CurrentPageIndex = dtgApp.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgApp, dvViewSample)
            cmd_delete.Enabled = True
            cmdModify.Enabled = True
            cmd_Reset.Disabled = False
            dtgApp.DataSource = dvViewSample
            dtgApp.DataBind()
            'lblRed.Visible = True
        Else
            cmd_delete.Enabled = False
            cmdModify.Enabled = False
            cmd_Reset.Disabled = True
            dtgApp.DataSource = dvViewSample
            dtgApp.DataBind()
            'lblRed.Visible = False

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
        txtSWSearch.Text = ""
    End Sub

    'Private Sub dtgApp_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgApp.ItemDataBound
    '    If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
    '        Dim chk As CheckBox
    '        Dim intTotalCell, intLoop As Integer
    '        chk = e.Item.FindControl("chkSelection")
    '        chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
    '        Dim intCnt As Integer
    '        Dim dsApp As New DataSet
    '        Dim dsAppBuyer As New DataView
    '        Dim lblBuyer As Label = e.Item.Cells(4).FindControl("lblBuyer")
    '        Dim lblApproval As Label = e.Item.Cells(3).FindControl("lblApproval")
    '        Dim val As String = e.Item.Cells(5).Text

    '        ' ai chu add on 05/10/2005
    '        ' deleted or inactive users are displayed in red colour
    '        Dim strColour1 As String = "<font color=Red>"
    '        Dim strColour2 As String = "</font>"

    '        '//THIS BIND DATAGRID with COMBINE 3 FIELD(Level, AO, AAO)

    '        If cboType.SelectedValue = "INV" Then
    '            dsApp = objApp.GetFinanceApprGroup(val, "FO")
    '        Else
    '            dsApp = objApp.GetGroup(val)
    '        End If

    '        If Not dsApp Is Nothing Then
    '            intCnt = dsApp.Tables(0).Rows.Count
    '            For intLoop = 0 To intCnt - 1
    '                If intLoop = 0 Then
    '                    lblApproval.Text = Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AGA_SEQ"), "-")
    '                    lblApproval.Text &= "."
    '                    If dsApp.Tables(0).Rows(intLoop)("AO_ACTIVE") = 1 Then
    '                        lblApproval.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AO_NAME"), "-")
    '                    Else
    '                        lblApproval.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AO_NAME"), "-") & strColour2
    '                    End If

    '                    lblApproval.Text &= " / "
    '                    If dsApp.Tables(0).Rows(intLoop)("AAO_ACTIVE") = 1 Then
    '                        lblApproval.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AAO_NAME"), "-")
    '                    Else
    '                        lblApproval.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AAO_NAME"), "-") & strColour2
    '                    End If

    '                Else
    '                    lblApproval.Text &= "<br>"
    '                    lblApproval.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AGA_SEQ"), "-")
    '                    lblApproval.Text &= "."
    '                    If dsApp.Tables(0).Rows(intLoop)("AO_ACTIVE") = 1 Then
    '                        lblApproval.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AO_NAME"), "-")
    '                    Else
    '                        lblApproval.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AO_NAME"), "-") & strColour2
    '                    End If

    '                    lblApproval.Text &= " / "
    '                    If dsApp.Tables(0).Rows(intLoop)("AAO_ACTIVE") = 1 Then
    '                        lblApproval.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AAO_NAME"), "-")
    '                    Else
    '                        lblApproval.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AAO_NAME"), "-") & strColour2
    '                    End If
    '                End If
    '            Next
    '        Else
    '            lblApproval.Text = "-"
    '        End If
    '        '//THIS BIND DATAGRID with COMBINE 1 FIELD(Buyer)
    '        If cboType.SelectedValue = "INV" Then
    '            dsAppBuyer = Nothing
    '            Dim ds As DataSet = objApp.GetFinanceApprGroup(val, "FM")

    '            If Not ds Is Nothing Then
    '                If ds.Tables.Count > 0 Then
    '                    dsAppBuyer = ds.Tables(0).DefaultView
    '                End If
    '            End If

    '            If Not dsAppBuyer Is Nothing Then
    '                intCnt = dsAppBuyer.Table.Rows.Count
    '                For intLoop = 0 To intCnt - 1
    '                    If intLoop = 0 Then
    '                        lblBuyer.Text = Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AGA_SEQ"), "-")
    '                        lblBuyer.Text &= "."
    '                        If dsAppBuyer.Table.Rows(intLoop)("AO_ACTIVE") = 1 Then
    '                            lblBuyer.Text &= Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AO_NAME"), "-")
    '                        Else
    '                            lblBuyer.Text &= strColour1 & Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AO_NAME"), "-") & strColour2
    '                        End If

    '                        lblBuyer.Text &= " / "
    '                        If dsAppBuyer.Table.Rows(intLoop)("AAO_ACTIVE") = 1 Then
    '                            lblBuyer.Text &= Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AAO_NAME"), "-")
    '                        Else
    '                            lblBuyer.Text &= strColour1 & Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AAO_NAME"), "-") & strColour2
    '                        End If

    '                    Else
    '                        lblBuyer.Text &= "<br>"
    '                        lblBuyer.Text &= Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AGA_SEQ"), "-")
    '                        lblBuyer.Text &= "."
    '                        If dsAppBuyer.Table.Rows(intLoop)("AO_ACTIVE") = 1 Then
    '                            lblBuyer.Text &= Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AO_NAME"), "-")
    '                        Else
    '                            lblBuyer.Text &= strColour1 & Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AO_NAME"), "-") & strColour2
    '                        End If

    '                        lblBuyer.Text &= " / "
    '                        If dsAppBuyer.Table.Rows(intLoop)("AAO_ACTIVE") = 1 Then
    '                            lblBuyer.Text &= Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AAO_NAME"), "-")
    '                        Else
    '                            lblBuyer.Text &= strColour1 & Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AAO_NAME"), "-") & strColour2
    '                        End If
    '                    End If
    '                Next
    '            Else
    '                lblBuyer.Text = ""
    '            End If


    '        Else
    '            dsAppBuyer = objApp.SelectedBuyerByAppList(val)

    '            If Not dsAppBuyer Is Nothing Then
    '                intCnt = dsAppBuyer.Count
    '                For intLoop = 0 To intCnt - 1
    '                    If intLoop = 0 Then
    '                        If dsAppBuyer(intLoop)("USER_STATUS") = 1 Then
    '                            lblBuyer.Text = Common.parseNull(dsAppBuyer(intLoop)("UM_USER_NAME"))
    '                        Else
    '                            lblBuyer.Text = strColour1 & Common.parseNull(dsAppBuyer(intLoop)("UM_USER_NAME")) & strColour2
    '                        End If
    '                    Else
    '                        lblBuyer.Text &= "<br>"
    '                        If dsAppBuyer(intLoop)("USER_STATUS") = 1 Then
    '                            lblBuyer.Text &= Common.parseNull(dsAppBuyer(intLoop)("UM_USER_NAME"))
    '                        Else
    '                            lblBuyer.Text &= strColour1 & Common.parseNull(dsAppBuyer(intLoop)("UM_USER_NAME")) & strColour2
    '                        End If
    '                    End If
    '                Next
    '            Else
    '                lblBuyer.Text = "-"
    '            End If
    '        End If
    '    End If
    'End Sub

    Private Sub cmd_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_clear.Click
        If hidMode.Value = "a" Then
            txt_add_mod.Text = ""
        Else
            txt_add_mod.Text = ViewState("oldvalue")
        End If

    End Sub

    Private Sub cmd_delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_delete.Click
        '//To prevent "No Record Found" msg
        txtSWSearch.Text = ""

        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim strIndex As String
        Dim intMsgNo As Integer

        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add("Index", Type.GetType("System.String"))

        For Each dgItem In dtgApp.Items
            chk = dgItem.FindControl("chkSelection")
            strIndex = dgItem.Cells.Item(2).Text
            If chk.Checked Then
                dr = dt.NewRow
                dr("Index") = strIndex
                dt.Rows.Add(dr)
            End If
        Next

        If objAdm.delSoftware(dt) Then
            Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        Else
            Common.NetMsgbox(Me, MsgRecordNotDelete, MsgBoxStyle.Information)
        End If

        ViewState("action") = "del"
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
        End If
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        '// to unset the value in the combo box

        Hide_Add2.Style("display") = "inline"
        txt_add_mod.Text = ""
        Me.lbl_add_mod.Text = "add"
        cmd_clear.Text = "Clear"
        hidMode.Value = "a"
        ''cboType.SelectedIndex = 0

        'If cboType.SelectedValue <> "INV" Then
        '    CheckParam()
        '    chkConsol.Style("display") = "inline"
        '    lblConsol.Style("display") = "inline"
        'End If

        'cboType.Enabled = False
        'If Session("Env") = "FTN" Then
        '    Me.dtgApp.Columns(2).Visible = False
        'End If

    End Sub

    Sub view(ByVal selected As String, ByVal strvalcboconsol As String)
        Dim objPersonal As New PersonalSetting
        Dim strmsg As String
        Dim intmsgno As Integer
        Dim strRedirect As String
        Dim strvalchk As String

        'If chkConsol.Checked = True Then
        '    strvalchk = "T"
        'Else
        '    strvalchk = "F"
        'End If

        'If cboType.SelectedValue = "PO" Then
        '    strvalcboconsol = ""
        '    chkConsol.Checked = False
        '    chkConsol.Style("display") = "none"
        '    lblConsol.Style("display") = "none"
        'ElseIf cboType.SelectedValue = "INV" Then
        '    strvalcboconsol = ""
        '    chkConsol.Checked = False
        'End If

        If selected = "add" Then
            Me.lbl_add_mod.Text = "add"
            '//// TO CATER DIFFERENT SITUATION during ADD
            'If chkConsol.Checked = True Then
            '    If strvalcboconsol = "" Then
            '        intmsgno = objApp.AddAppWorkFlow(txt_add_mod.Text, cboConsol.SelectedItem.Value, cboType.SelectedValue)
            '    Else
            '        intmsgno = objApp.AddAppWorkFlow(txt_add_mod.Text, strvalcboconsol, cboType.SelectedValue)
            '    End If
            'Else
            '    intmsgno = objApp.AddAppWorkFlow(txt_add_mod.Text, strvalcboconsol, cboType.SelectedValue)
            'End If
            '////
            intmsgno = objAdm.addSoftware(txt_add_mod.Text)
            'Select Case intmsgno
            '    Case WheelMsgNum.Save
            '        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
            '    Case WheelMsgNum.Duplicate
            '        strmsg = MsgRecordDuplicate
            '        If chkConsol.Checked = False Then
            '            val_consol.Enabled = False
            '        End If
            '        Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
            '    Case WheelMsgNum.NotSave
            '        strmsg = MsgRecordNotSave
            '        Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
            'End Select

        ElseIf selected = "mod" Then
            Me.lbl_add_mod.Text = "modify"
            '//// TO CATER DIFFERENT SITUATION during MODIFY
            'If viewstate("oldvalue2") = "&nbsp;" Or viewstate("oldvalue2") = "" Then
            '    intmsgno = objApp.modAppWorkFlow(hidIndex.Value, txt_add_mod.Text, strvalcboconsol, cboType.SelectedValue, viewstate("oldvalue"))
            'ElseIf strvalcboconsol <> "" Then
            '    intmsgno = objApp.modAppWorkFlow(hidIndex.Value, txt_add_mod.Text, strvalcboconsol, cboType.SelectedValue, viewstate("oldvalue"))
            'ElseIf strvalcboconsol = "" And viewstate("oldvalue2") <> "" Then
            '    If chkConsol.Checked = False Then
            '        intmsgno = objApp.modAppWorkFlow(hidIndex.Value, txt_add_mod.Text, strvalcboconsol, cboType.SelectedValue, viewstate("oldvalue"))
            '    Else
            '        intmsgno = objApp.modAppWorkFlow(hidIndex.Value, txt_add_mod.Text, viewstate("oldvalue2"), cboType.SelectedValue, viewstate("oldvalue"))
            '    End If
            'Else
            '    intmsgno = objApp.modAppWorkFlow(hidIndex.Value, txt_add_mod.Text, viewstate("oldvalue2"), cboType.SelectedValue, viewstate("oldvalue"))
            'End If
            '////
            intmsgno = objAdm.modSoftware(hidIndex.Value, txt_add_mod.Text)

        End If

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
            Case WheelMsgNum.NotSave
                strmsg = MsgRecordNotSave
                Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
        End Select
    End Sub

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click

        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim i As Integer
        i = 0

        Hide_Add2.Style("display") = "inline"
        cmd_clear.Text = "Reset"
        hidMode.Value = "m"
        For Each dgItem In dtgApp.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                hidIndex.Value = dgItem.Cells(2).Text
                txt_add_mod.Text = dgItem.Cells(1).Text
                ViewState("oldvalue") = dgItem.Cells(1).Text
                'viewstate("oldvalue2") = dgItem.Cells.Item(AWEnum.AWUserID).Text
                'viewstate("oldvalue3") = dgItem.Cells.Item(AWEnum.AWType).Text
                ''cboType.SelectedValue = dgItem.Cells.Item(AWEnum.AWType).Text
                ''//check for consolidator,modify by Moo
                ''&nbsp;"
                'If Not (dgItem.Cells(AWEnum.AWConsol).Text = "" Or dgItem.Cells(AWEnum.AWConsol).Text = "&nbsp;") Then
                '    chkConsol.Style("display") = "inline"
                '    lblConsol.Style("display") = "inline"
                '    chkConsol.Checked = True
                '    viewstate("consol") = True
                'Else
                '    chkConsol.Style("display") = "none"
                '    lblConsol.Style("display") = "none"
                '    chkConsol.Checked = False
                '    viewstate("consol") = False
                'End If
                'chkConsol_CheckedChanged(sender, e)
                'If Not (dgItem.Cells(AWEnum.AWConsol).Text = "" Or dgItem.Cells(AWEnum.AWConsol).Text = "&nbsp;") Then
                '    Common.SelDdl(dgItem.Cells(AWEnum.AWUserID).Text, cboConsol)
                'End If
                Exit For
            End If
            i = i + 1
        Next

        Me.lbl_add_mod.Text = "modify"
        cmdModify.Enabled = True
    End Sub

    Private Sub cmd_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_save.Click
        '//To prevent "No Record Found" msg
        txtSWSearch.Text = ""

        'If strvalcboconsol = "" Then
        If hidMode.Value = "a" Then
            Me.view("add", "")
        ElseIf hidMode.Value = "m" Then
            Me.view("mod", "")
        End If
        'Else
        'strvalcboconsol = cboConsol.SelectedItem.Value

        'If hidMode.Value = "a" Then
        '    Me.view("add", strvalcboconsol)
        'ElseIf hidMode.Value = "m" Then
        '    Me.view("mod", strvalcboconsol)
        'End If

        'End If

        'cboType.Enabled = True
        bindgridApp(0)
    End Sub

    'Private Sub cboConsol_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboConsol.SelectedIndexChanged
    '    If cboConsol.SelectedItem.Text <> "---Select---" Then
    '        strvalcboconsol = cboConsol.SelectedItem.Value
    '        'Else
    '        '    Common.NetMsgbox(Me, MsgNoRecord)
    '    End If
    'End Sub

    'Private Sub chkConsol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkConsol.CheckedChanged

    '    If chkConsol.Checked = True Then
    '        divConsolidator.Style("display") = "inline"
    '        lblSymbol.Visible = True
    '        BindData()
    '        val_consol.Enabled = True
    '    Else
    '        val_consol.Enabled = False
    '        divConsolidator.Style("display") = "none"
    '        lblSymbol.Visible = False
    '    End If

    'End Sub
    'Function CheckParam()
    '    Dim intchkmsg As Integer
    '    intchkmsg = objApp.checkconsolidatorParam()
    '    Select Case intchkmsg
    '        Case 0
    '            chkConsol.Checked = False
    '            divConsolidator.Style("display") = "none"
    '        Case 1
    '            chkConsol.Checked = True

    '            divConsolidator.Style("display") = "inline"
    '            lblSymbol.Visible = True
    '            BindData()
    '            val_consol.Enabled = True
    '    End Select
    'End Function


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
        If ViewState("Side") = "BUYER" Then
            '    Session("w_SoftWare_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                            "<li><a class=""t_entity_btn"" href=""../Companies/coCompanyDetail.aspx?mode=modify&pageid=" & strPageId & """><span>Company Details</span></a></li>" & _
            '                            "<li><div class=""space""></div></li>" & _
            '                            "<li><a class=""t_entity_btn"" href=""../Admin/BComParam.aspx?pageid=" & strPageId & """><span>Parameters</span></a></li>" & _
            '                            "<li><div class=""space""></div></li>" & _
            '                            "<li><a class=""t_entity_btn"" href=""../Admin/AddressMaster.aspx?type=D&mod=T&pageid=" & strPageId & """><span>Delivery Address</span></a></li>" & _
            '                            "<li><div class=""space""></div></li>" & _
            '                            "<li><a class=""t_entity_btn"" href=""../Admin/AddressMaster.aspx?type=B&mod=T&pageid=" & strPageId & """><span>Billing Address</span></a></li>" & _
            '                            "<li><div class=""space""></div></li>" & _
            '                            "<li><a class=""t_entity_btn"" href=""../Admin/DeptSetup.aspx?pageid=" & strPageId & """><span>Department</span></a></li>" & _
            '                            "<li><div class=""space""></div></li>" & _
            '                             "<li><a class=""t_entity_btn_selected"" href=""../Companies/SoftwareApp.aspx?side=BUYER&pageid=" & strPageId & """><span>Software</span></a></li>" & _
            '                             "<li><div class=""space""></div></li>" & _
            '                            "<li><a class=""t_entity_btn"" href=""../Companies/SalesInfo.aspx?side=BUYER&pageid=" & strPageId & """><span>Sales Info</span></a></li>" & _
            '                            "<li><div class=""space""></div></li>" & _
            '                             "<li><a class=""t_entity_btn"" href=""../Companies/QualityStd.aspx?side=BUYER&pageid=" & strPageId & """><span>Quality Standards</span></a></li>" & _
            '                             "<li><div class=""space""></div></li>" & _
            '                  "</ul><div></div></div>"
            'Else
            '    Session("w_SoftWare_tabs") = "<div class=""t_entity""><ul>" & _
            '                     "<li><a class=""t_entity_btn"" href=""../Companies/coCompanyDetail.aspx?side=VENDOR&mode=modify&pageid=" & strPageId & """><span>Company Details</span></a></li>" & _
            '                     "<li><div class=""space""></div></li>" & _
            '                     "<li><a class=""t_entity_btn"" href=""../Admin/BComVendor.aspx?side=VENDOR&pageid=" & strPageId & """><span>Parameters</span></a></li>" & _
            '                     "<li><div class=""space""></div></li>" & _
            '                        "<li><a class=""t_entity_btn_selected"" href=""../Companies/SoftwareApp.aspx?side=VENDOR&pageid=" & strPageId & """><span>Software</span></a></li>" & _
            '                        "<li><div class=""space""></div></li>" & _
            '                        "<li><a class=""t_entity_btn"" href=""../Companies/SalesInfo.aspx?side=VENDOR&pageid=" & strPageId & """><span>Sales Info</span></a></li>" & _
            '                        "<li><div class=""space""></div></li>" & _
            '                        "<li><a class=""t_entity_btn"" href=""../Companies/QualityStd.aspx?side=VENDOR&pageid=" & strPageId & """><span>Quality Standards</span></a></li>" & _
            '                        "<li><div class=""space""></div></li>" & _
            '                  "</ul><div></div></div>"
            Session("w_SoftWare_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "mode=modify" & "&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "type=D&mod=T" & "&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "type=B&mod=T" & "&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "pageid=" & strPageId) & """><span>Department</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SalesInfo.aspx", "side=BUYER" & "&pageid=" & strPageId) & """><span>Sales Info</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Companies", "SoftwareApp.aspx", "side=BUYER" & "&pageid=" & strPageId) & """><span>Software</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "QualityStd.aspx", "side=BUYER" & "&pageid=" & strPageId) & """><span>Quality Standards</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                          "</ul><div></div></div>"
        Else
            Session("w_SoftWare_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=VENDOR&mode=modify" & "&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComVendor.aspx", "side=VENDOR" & "&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SalesInfo.aspx", "side=VENDOR" & "&pageid=" & strPageId) & """><span>Sales Info</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Companies", "SoftwareApp.aspx", "side=VENDOR" & "&pageid=" & strPageId) & """><span>Software</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "QualityStd.aspx", "side=VENDOR" & "&pageid=" & strPageId) & """><span>Quality Standards</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                          "</ul><div></div></div>"





        End If
    End Sub

End Class
