Imports AgoraLegacy
Imports eProcure.Component


Public Class AppGrpAsgPurFTN
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objApp As New ApprWorkFlow
    Dim strvalofficerlist As String
    Dim valGroup, valconsol, valgrpname As String
    Dim strQueryselect As String
    'Protected WithEvents lstAppOfficer As System.Web.UI.WebControls.ListBox
    Dim ds1 As DataSet
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblRed As System.Web.UI.WebControls.Label
    Protected WithEvents trAppAsign As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents btn_Add2 As System.Web.UI.WebControls.Button
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents cboType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents trGrpType As System.Web.UI.HtmlControls.HtmlTableRow
    Dim bln As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cboGroup As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents chkconsol As System.Web.UI.WebControls.CheckBox
    'Protected WithEvents MyDataGrid As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lstbox1 As System.Web.UI.WebControls.ListBox
    Protected WithEvents cmdAdd1 As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd2 As System.Web.UI.WebControls.Button
    Protected WithEvents lstBox2 As System.Web.UI.WebControls.ListBox
    Protected WithEvents cmdsave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.WebControls.Button
    Protected WithEvents Div_AppGrp As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents Table3 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Div_AA As System.Web.UI.HtmlControls.HtmlGenericControl

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Enum AsgEnum
        ASeq = 1
        AID = 2
        AName = 3
        AMass = 4
        AAID = 5
        AAName = 6
        ARelief = 7
        AIndex = 8
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdsave.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdsave)
        htPageAccess.Add("update", alButtonList)
        CheckButtonAccess()
        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objUser As New Users

        MyBase.Page_Load(sender, e)

        If Not Page.IsPostBack Then
            GenerateTab()
            If Request.QueryString("apprType") <> Nothing Then
                Common.SelDdl(Request.QueryString("apprType"), cboType, True, False)
            Else
                cboType.SelectedIndex = 0
                'If Session("Env") = "FTN" Then
                '    cboType.Items.Remove(cboType.Items.FindByValue("INV"))
                'End If
            End If
        End If

        'SetGridProperty(MyDataGrid)

        If ViewState("ApprType") <> cboType.SelectedValue Then

            ViewState("ApprType") = cboType.SelectedValue

            If ViewState("ApprType") = "INV" Then
                'btn_Add2.Visible = True

                trAppAsign.Style.Item("display") = "none"
                'chkconsol.Style.Item("display") = "none"
                'chkconsol.Checked = False

                Div_AA.Style("display") = "none"
            Else
                Div_AA.Style("display") = "inline"
            End If

            'If Not Page.IsPostBack Then
            ViewState("strtxt") = Me.Request.QueryString("grp")
            ViewState("strchk") = Me.Request.QueryString("chk")
            ViewState("strlink") = Me.Request.QueryString("frm")
            If ViewState("strtxt") <> "" Then
                BindGroup()

                Common.SelDdl(ViewState("strtxt"), cboGroup, False, True)
                Call Bindgrid()

            Else
                BindGroup()
            End If
            'viewstate("strchk") = Me.Request.QueryString("chk")
            'cmd_Delete.Enabled = False
            'cmdModify.Enabled = False

        End If
        '//MODITFY BY MOO
        If cboGroup.SelectedIndex <> 0 Then
            'rdSearch.Enabled = True

            'If viewstate("ApprType") = "INV" Then
            'trAppAsign.Style.Item("display") = "none"
            'chkconsol.Style.Item("display") = "none"
            'chkconsol.Checked = False

            'Div_AA.Style("display") = "inline"
            'Div_AA.Style("display") = "none"
            'cmd_Add.Enabled = True
            'ElseIf rdSearch.SelectedValue = "A" Then
            '    Div_AppGrp.Style("display") = "inline"
            '    Div_AA.Style("display") = "none"
            '    'If cboGroup.SelectedItem.Text = "---Select---" Then
            '    'cmd_Add.Enabled = False
            '    'MyDataGrid.DataBind()
            '    'Else
            '    cmd_Add.Enabled = True
            '    'End If
            'ElseIf rdSearch.SelectedValue = "B" Then
            '    Div_AppGrp.Style("display") = "none"
            Div_AA.Style("display") = "inline"

            'If cboGroup.SelectedItem.Text = "---Select---" Then
            '    Common.NetMsgbox(Me, MsgNoRecord)
            'End If
            'lstAppOfficer.Items.Clear()
            'lstbox1.Items.Clear()
            'lstBox2.Items.Clear()
            'selectedData()
            'SearchData()
            'AppOfficerData()
            'End If
        Else
            'rdSearch.Enabled = False
            'Div_AppGrp.Style("display") = "none"
            Div_AA.Style("display") = "none"
            'cmd_Add.Enabled = False
        End If

        'cmd_Delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        'cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")

        'If ViewState("strlink") = "AW" Then
        '    lnkBack.NavigateUrl = "ApprovalWorkFlow.aspx?&pageid=" & strPageId
        '    lnkBack.Visible = True
        'ElseIf Div_AA.Style("display") = "inline" Then
        '    lnkBack.NavigateUrl = "AppGrpAsg.aspx?&pageid=" & strPageId
        '    lnkBack.Visible = True
        'Else
        lnkBack.Visible = False
        'End If

    End Sub

    Public Function BindGroup()
        Dim cbolist As New ListItem
        'Dim objApproval As New AppWorkFlow
        Dim dvCustom As DataView
        'If chkconsol.Checked = True Then
        'dvCustom = objApp.getcboAppconsol
        'Else
        'If viewstate("ApprType") = "INV" Then
        '    dvCustom = objApp.getcboApp("AGM_TYPE = 'INV'")
        'Else
        '    dvCustom = objApp.getcboApp("AGM_TYPE <> 'INV'")
        'End If
        dvCustom = objApp.getcboApp("AGM_TYPE = '" & ViewState("ApprType") & "'")

        'End If
        cboGroup.Items.Clear()
        If Not dvCustom Is Nothing Then
            Common.FillDdl(cboGroup, "AGM_GRP_NAME", "AGM_GRP_INDEX", dvCustom)
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboGroup.Items.Insert(0, cbolist)
    End Function

    Public Sub MyData_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        'MyDataGrid.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        'MyDataGrid.CurrentPageIndex = 0
        Bindgrid(0)
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        valGroup = cboGroup.SelectedItem.Value
        Dim ds As New DataSet

        If ViewState("ApprType") = "INV" Then
            ds = objApp.BindFinanceAppGroupAsg(valGroup)
        Else
            ds = objApp.BindAppGroupAsg(valGroup)
        End If

        'viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count
        'Dim dvViewSample As DataView
        'dvViewSample = ds.Tables(0).DefaultView
        'dvViewSample.Sort = viewstate("SortExpression")
        'If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        'If viewstate("action") = "del" Then
        '    If MyDataGrid.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod MyDataGrid.PageSize = 0 Then
        '        MyDataGrid.CurrentPageIndex = MyDataGrid.CurrentPageIndex - 1
        '        viewstate("action") = ""
        '    End If
        'End If
        'intPageRecordCnt = ds.Tables(0).Rows.Count
        'If intPageRecordCnt > 0 Then
        '    'cmd_Delete.Enabled = True
        '    'cmdModify.Enabled = True
        '    MyDataGrid.DataSource = dvViewSample
        '    MyDataGrid.DataBind()
        '    lblRed.Visible = True
        'Else
        '    'cmd_Delete.Enabled = False
        '    'cmdModify.Enabled = False
        '    MyDataGrid.DataBind()
        '    Common.NetMsgbox(Me, MsgNoRecord)
        '    lblRed.Visible = False
        'End If
    End Function

    'Private Sub cmd_delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Delete.Click
    '    valGroup = cboGroup.SelectedItem.Value
    '    Dim dgItem As DataGridItem
    '    Dim chk As CheckBox
    '    Dim strIndex, strLevel As String
    '    Dim i As Integer = 0
    '    For Each dgItem In MyDataGrid.Items

    '        chk = dgItem.FindControl("chkSelection")

    '        strLevel = dgItem.Cells(AsgEnum.ASeq).Text

    '        If chk.Checked Then

    '            If viewstate("ApprType") = "INV" Then
    '                If strLevel.StartsWith("FO") Then
    '                    objApp.DelFOAppGrpAsg(valGroup, strLevel.Substring(5))
    '                Else
    '                    objApp.DelFMAppGrpAsg(valGroup, strLevel.Substring(5))
    '                End If
    '            Else
    '                objApp.DelAppGrpAsg(valGroup, strLevel)
    '            End If

    '        End If
    '        i = i + 1
    '    Next
    '    Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
    '    viewstate("action") = "del"
    '    Bindgrid(0)
    'End Sub

    Public Function AppOfficerData()
        valGroup = cboGroup.SelectedItem.Value
        Dim dvcustom As DataView

        'If lstAppOfficer.SelectedIndex = -1 Then
        'strQueryselect = "SELECT  A.AGA_GRP_INDEX, A.AGA_SEQ, A.AGA_AO, A.AGA_A_AO,M.AGM_COY_ID, " & _
        '                 "CAST(A.AGA_SEQ AS VARCHAR(4)) + ' : ' + A.AGA_AO + ' : ' + A.AGA_A_AO as three " & _
        '                 "FROM APPROVAL_GRP_AO A INNER JOIN " & _
        '                 "APPROVAL_GRP_MSTR M ON A.AGA_GRP_INDEX = M.AGM_GRP_INDEX " & _
        '                 "WHERE M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' " & _
        '                 "AND A.AGA_GRP_INDEX ='" & valGroup & "'"
        dvcustom = objApp.bindlistbox_AppOfficerData(valGroup)
        'Else
        'strQueryselect = "SELECT  A.AGA_GRP_INDEX, A.AGA_SEQ, A.AGA_AO, A.AGA_A_AO,M.AGM_COY_ID, " & _
        '                "CAST(A.AGA_SEQ AS VARCHAR(4)) + ' : ' + A.AGA_AO + ' : ' + A.AGA_A_AO as three " & _
        '                "FROM APPROVAL_GRP_AO A INNER JOIN " & _
        '                "APPROVAL_GRP_MSTR M ON A.AGA_GRP_INDEX = M.AGM_GRP_INDEX " & _
        '                "WHERE M.AGM_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' " & _
        '                "AND A.AGA_GRP_INDEX ='" & valGroup & "'"
        ' dvcustom = objApp.bindlistbox_AppOfficerData(valGroup)
        'End If

        'lstAppOfficer.DataSource = dvcustom
        'lstAppOfficer.DataTextField = "three"
        'lstAppOfficer.DataValueField = "AGA_GRP_INDEX"
        'lstAppOfficer.DataBind()
    End Function

    'Private Sub MyDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles MyDataGrid.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim chk As CheckBox
    '        chk = e.Item.FindControl("chkSelection")
    '        chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
    '        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
    '        e.Item.Cells(AsgEnum.ARelief).Text = IIf(e.Item.Cells(AsgEnum.ARelief).Text = "C", "Controlled", "Open")

    '        ' ai chu add on 05/10/2005
    '        ' deleted or inactive users are displayed in red colour
    '        Dim strColour1 As String = "<font color=Red>"
    '        Dim strColour2 As String = "</font>"
    '        If dv("AO_ACTIVE") = 1 Then
    '            e.Item.Cells(AsgEnum.AName).Text = Common.parseNull(dv("AO_NAME"))
    '        Else
    '            e.Item.Cells(AsgEnum.AName).Text = strColour1 & Common.parseNull(dv("AO_NAME")) & strColour2
    '        End If

    '        If dv("AAO_ACTIVE") = 1 Then
    '            e.Item.Cells(AsgEnum.AAName).Text = Common.parseNull(dv("AAO_NAME"))
    '        Else
    '            e.Item.Cells(AsgEnum.AAName).Text = strColour1 & Common.parseNull(dv("AAO_NAME")) & strColour2
    '        End If

    '        If IsDBNull(dv("UM_MASS_APP")) Then
    '            e.Item.Cells(AsgEnum.AMass).Text = "N"
    '        End If
    '    End If
    'End Sub

    'Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
    '    If cboGroup.SelectedItem.Text = "---Select---" Then
    '        Common.NetMsgbox(Me, MsgNoRecord)
    '    Else
    '        hidMode.Value = "m"
    '        Dim dgItem As DataGridItem
    '        Dim chk As CheckBox
    '        Dim i As Integer
    '        Dim strLevel, str_AO, str_AAO, strRelief As String
    '        For Each dgItem In MyDataGrid.Items
    '            chk = dgItem.FindControl("chkSelection")
    '            strLevel = dgItem.Cells(AsgEnum.ASeq).Text
    '            str_AO = dgItem.Cells(AsgEnum.AID).Text
    '            str_AAO = dgItem.Cells(AsgEnum.AAID).Text
    '            strRelief = dgItem.Cells(AsgEnum.ARelief).Text
    '            valGroup = cboGroup.SelectedItem.Value
    '            valgrpname = cboGroup.SelectedItem.Text
    '            Dim strModeM As String = hidMode.Value

    '            If chk.Checked Then
    '                If viewstate("ApprType") = "INV" Then
    '                    If strLevel.StartsWith("FO") Then
    '                        Me.Response.Redirect("AsgAppOfficer.aspx?Mode=M&type=FO&grpName=" & Server.UrlEncode(valgrpname) & "&Index=" & valGroup & "&Level=" & strLevel & "&AO=" & str_AO & "&AAO=" & str_AAO & "&Relief=" & strRelief & "&pageid=" & strPageId & "&chk=" & viewstate("strchk") & "&frm=" & viewstate("strlink") & "&apprType=" & viewstate("ApprType"))
    '                    Else
    '                        Me.Response.Redirect("AsgAppOfficer.aspx?Mode=M&type=FM&grpName=" & Server.UrlEncode(valgrpname) & "&Index=" & valGroup & "&Level=" & strLevel & "&AO=" & str_AO & "&AAO=" & str_AAO & "&Relief=" & strRelief & "&pageid=" & strPageId & "&chk=" & viewstate("strchk") & "&frm=" & viewstate("strlink") & "&apprType=" & viewstate("ApprType"))
    '                    End If
    '                Else
    '                    Me.Response.Redirect("AsgAppOfficer.aspx?Mode=M&type=AO&grpName=" & Server.UrlEncode(valgrpname) & "&Index=" & valGroup & "&Level=" & strLevel & "&AO=" & str_AO & "&AAO=" & str_AAO & "&Relief=" & strRelief & "&pageid=" & strPageId & "&chk=" & viewstate("strchk") & "&frm=" & viewstate("strlink") & "&apprType=" & viewstate("ApprType"))
    '                End If

    '                'Response.Redirect("AsgAppOfficer.aspx?AppGrpIndex=" & strcboval & "&AppGrp=" & strGrpName & "&Mode=" & strMode & "")
    '                Exit For
    '            End If
    '        Next
    '    End If
    'End Sub

    'Private Sub MyDataGrid_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles MyDataGrid.ItemCreated
    '    intPageRecordCnt = viewstate("intPageRecordCnt")
    '    Grid_ItemCreated(MyDataGrid, e)
    '    If e.Item.ItemType = ListItemType.Header Then

    '        Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
    '        chkAll.Attributes.Add("onclick", "selectAll();")
    '    End If
    'End Sub

    Private Sub cmdAdd1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd1.Click
        Dim li As ListItem
        Dim objdb As New EAD.DBCom
        Dim strsql As String
        Dim ds As New DataSet
        For Each li In lstbox1.Items
            If li.Selected = True Then
                Dim lstItem As New ListItem
                lstItem.Text = li.Text
                lstItem.Value = li.Value
                'For FTN, check that 1 purchaser can only have 1 workflow
                'If Session("Env") = "FTN" Then
                '    strsql = "Select * from APPROVAL_GRP_BUYER, APPROVAL_GRP_MSTR where AGB_GRP_INDEX <> '" & cboGroup.SelectedValue & "' and AGM_GRP_INDEX = AGB_GRP_INDEX AND AGM_TYPE = 'PO' AND AGB_BUYER = '" & lstItem.Value & "' AND AGM_COY_ID = '" & Session("CompanyID") & "' "
                '    ds = objdb.FillDs(strsql)
                '    If ds.Tables(0).Rows.Count > 0 Then
                '        Common.NetMsgbox(Me, "This purchaser has already assigned with workflow.", MsgBoxStyle.Exclamation)
                '        Exit Sub
                '    End If
                'End If
                lstBox2.Items.Add(lstItem)
            End If
        Next
        Dim counter As Integer
        For counter = (lstbox1.Items.Count - 1) To 0 Step -1
            If lstbox1.Items(counter).Selected = True Then
                lstbox1.Items.RemoveAt(counter)
            End If
        Next
        cmdAdd1.Enabled = True
        cmdAdd2.Enabled = True
    End Sub

    Private Sub cmdAdd2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd2.Click
        Dim li As ListItem
        For Each li In lstBox2.Items
            If li.Selected = True Then
                Dim lstItem As New ListItem
                lstItem.Text = li.Text
                lstItem.Value = li.Value
                lstbox1.Items.Add(lstItem)
                'lstbox1.Items.Add(li.Text)
            End If
        Next
        Dim counter As Integer
        For counter = (lstBox2.Items.Count - 1) To 0 Step -1
            If lstBox2.Items(counter).Selected = True Then
                lstBox2.Items.RemoveAt(counter)
            End If
        Next
        cmdAdd1.Enabled = True
        cmdAdd2.Enabled = True
    End Sub

    Public Function selectedData()

        Dim strQueryselect As String
        Dim dvcustom As DataView
        'strvalofficerlist = lstAppOfficer.SelectedItem.Value
        strvalofficerlist = cboGroup.SelectedItem.Value
        'strQueryselect = "SELECT U.UM_DEPT_ID,U.UM_USER_ID, U.UM_USER_NAME, " & _
        '         "(U.UM_DEPT_ID + ' : ' + U.UM_USER_ID + ' : ' + U.UM_USER_NAME) as three " & _
        '         "FROM APPROVAL_GRP_BUYER K, USER_MSTR U " & _
        '         "where K.AGB_GRP_INDEX = '" & strvalofficerlist & "' AND K.AGB_BUYER = U.UM_USER_ID AND U.UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "'"

        dvcustom = objApp.SelectedBuyerByAppList(strvalofficerlist)

        lstBox2.DataSource = dvcustom
        lstBox2.DataTextField = "three"
        lstBox2.DataValueField = "UM_USER_ID"
        lstBox2.DataBind()
        cmdAdd2.Enabled = True
        cmdAdd2.Enabled = True
    End Function

    Public Function SearchData()
        Dim dvcustom As DataView
        Dim strvalofficerlist, grouptype As String
        'strvalofficerlist = lstAppOfficer.SelectedItem.Value
        grouptype = cboType.SelectedItem.Value
        strvalofficerlist = cboGroup.SelectedItem.Value
        Dim strQuery, strQueryselect As String

        'strQuery = "SELECT DISTINCT U.UM_DEPT_ID,U.UM_USER_ID, U.UM_USER_NAME, " & _
        '    "(U.UM_DEPT_ID + ' : ' + U.UM_USER_ID + ' : ' + U.UM_USER_NAME) as three " & _
        '    "FROM USER_MSTR U,USERS_USRGRP G " & _
        '    "where U.UM_USER_ID NOT IN (SELECT K.AGB_BUYER from APPROVAL_GRP_MSTR M ,APPROVAL_GRP_BUYER K where M.AGM_GRP_INDEX=k.AGB_GRP_INDEX AND M.AGM_GRP_INDEX = '" & strvalofficerlist & "') " & _
        '    "AND G.UU_USRGRP_ID='AppOfficer' AND UM_COY_ID='" & HttpContext.Current.Session("CompanyID") & "'"        
        dvcustom = objApp.AvailBuyerByAppList(strvalofficerlist, grouptype)

        lstbox1.DataSource = dvcustom
        lstbox1.DataTextField = "three"
        lstbox1.DataValueField = "UM_USER_ID"
        lstbox1.DataBind()
        cmdAdd1.Enabled = True
        cmdAdd2.Enabled = True
    End Function

    Private Sub cmdsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdsave.Click
        Dim li As ListItem

        Dim strvalofficerlist, strMsg, strTemp, strMsg1 As String
        strvalofficerlist = cboGroup.SelectedItem.Value
        Dim strSelectedBuyer(0), strSelectedBuyer1(0), strAry() As String
        Dim strBuyerid As String

        For Each li In lstBox2.Items
            Common.Insert2Ary(strSelectedBuyer, li.Value)
            strTemp = Split(li.Text, ":")(1)
            Common.Insert2Ary(strSelectedBuyer1, Trim(strTemp))
        Next

        strTemp = ""
        strMsg = objApp.AddAppBuyer(strvalofficerlist, strSelectedBuyer, strSelectedBuyer1, strTemp)

        If strTemp <> "" Then
            strAry = Split(strTemp, "$")
            strMsg1 = "The following buyer(s) cannot be assigned because they are already assigned as Approving Officer in the same list."
            For Each strBuyerid In strAry
                If strBuyerid.Trim <> "" Then
                    '"""& vbCrLf & ""-- " & strError4
                    strMsg1 = strMsg1 & """& vbCrLf & ""-- " & strBuyerid.Trim
                End If
            Next
            Common.NetMsgbox(Me, strMsg1, MsgBoxStyle.Information)
        End If

        If strMsg = WheelMsgNum.Save Then
            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        Else
            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
        End If
        'rdSearch_SelectedIndexChanged(sender, e)
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        lstBox2.Items.Clear()
        lstbox1.Items.Clear()
        'lstAppOfficer.Items.Clear()
        'cboGroup.SelectedIndex = 0
        selectedData()
        SearchData()
        AppOfficerData()

        'chkconsol.Checked = False
        cmdAdd1.Enabled = True
        cmdAdd2.Enabled = True
    End Sub

    Private Sub cboGroup_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboGroup.SelectedIndexChanged
        If cboGroup.SelectedItem.Text = "---Select---" Then
            'lstAppOfficer.Items.Clear()
            lstbox1.Items.Clear()
            lstBox2.Items.Clear()

            'If viewstate("ApprType") = "INV" Then
            '    trAppAsign.Style.Item("display") = "inline"
            '    chkconsol.Style.Item("display") = "inline"
            '    chkconsol.Checked = False
            'End If
        Else
            If viewstate("ApprType") = "INV" Then

                'End If

                'If objApp.checkapprovaltype(cboGroup.SelectedItem.Value) = ApprovalType.Invoice Then
                'trAppAsign.Style.Item("display") = "none"
                'chkconsol.Style.Item("display") = "none"
                'chkconsol.Checked = False

                'rdSearch.Enabled = False
                'Div_AppGrp.Style("display") = "inline"
                'Div_AA.Style("display") = "none"

                Bindgrid(0)
            Else
                'trAppAsign.Style.Item("display") = "inline"
                'chkconsol.Style.Item("display") = "inline"

                'If Session("Env") <> "FTN" Then
                '    Dim intchkmsg As Integer
                '    intchkmsg = objApp.checkconsolidatorApp(cboGroup.SelectedItem.Value)
                '    'Select Case intchkmsg
                '    '    Case 0
                '    '        chkconsol.Checked = False
                '    '    Case 1
                '    '        chkconsol.Checked = True
                '    'End Select
                'End If
                'lstAppOfficer.Items.Clear()
                lstbox1.Items.Clear()
                lstBox2.Items.Clear()
                selectedData()
                SearchData()
                AppOfficerData()
            End If
        End If
    End Sub

    'Private Sub cmd_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Add.Click
    '    If cboGroup.SelectedItem.Text = "---Select---" Then
    '        Common.NetMsgbox(Me, MsgNoRecord)
    '    Else
    '        hidMode.Value = "a"
    '        Dim strGrpName As String = cboGroup.SelectedItem.Text
    '        Dim strcboval As String = cboGroup.SelectedItem.Value
    '        Dim strMode As String = hidMode.Value

    '        If viewstate("ApprType") = "INV" Then
    '            Me.Response.Redirect("AsgAppOfficer.aspx?Mode=A&type=FO&AppGrpIndex=" & strcboval & "&AppGrp=" & Server.UrlEncode(strGrpName) & "&pageid=" & strPageId & "&chk=" & viewstate("strchk") & "&frm=" & viewstate("strlink") & "&apprType=" & viewstate("ApprType"))
    '        Else
    '            Me.Response.Redirect("AsgAppOfficer.aspx?Mode=A&type=AO&AppGrpIndex=" & strcboval & "&AppGrp=" & Server.UrlEncode(strGrpName) & "&pageid=" & strPageId & "&chk=" & viewstate("strchk") & "&frm=" & viewstate("strlink") & "&apprType=" & viewstate("ApprType"))
    '        End If
    '    End If
    'End Sub

    'Private Sub lstAppOfficer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstAppOfficer.SelectedIndexChanged
    'If lstAppOfficer.SelectedItem.Text = "" Then
    ' Common.NetMsgbox(Me, MsgNoRecord)
    ' Else
    ' selectedData()
    ' SearchData()
    'End If
    'End Sub

    'Private Sub chkconsol_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkconsol.CheckedChanged
    '    If chkconsol.Checked = True Then
    '        BindGroup()
    '    Else
    '        BindGroup()
    '    End If
    'End Sub

    'Private Sub rdSearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdSearch.SelectedIndexChanged
    '    'cboGroup.SelectedIndex = 0
    '    If rdSearch.SelectedValue = "B" Then
    '        If cboGroup.SelectedItem.Text <> "---Select---" Then
    '            lstbox1.Items.Clear()
    '            lstBox2.Items.Clear()
    '            lstAppOfficer.Items.Clear()
    '            selectedData()
    '            SearchData()
    '            AppOfficerData()
    '        Else
    '            lstbox1.Items.Clear()
    '            lstBox2.Items.Clear()
    '            lstAppOfficer.Items.Clear()
    '        End If
    '    Else
    '        MyDataGrid.DataBind()
    '        Bindgrid(0)
    '    End If
    'End Sub

    'Private Sub btn_Add2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Add2.Click
    '    If cboGroup.SelectedItem.Text = "---Select---" Then
    '        Common.NetMsgbox(Me, MsgNoRecord)
    '    Else
    '        hidMode.Value = "a"
    '        Dim strGrpName As String = cboGroup.SelectedItem.Text
    '        Dim strcboval As String = cboGroup.SelectedItem.Value
    '        Dim strMode As String = hidMode.Value

    '        If viewstate("ApprType") = "INV" Then
    '            Me.Response.Redirect("AsgAppOfficer.aspx?Mode=A&type=FM&AppGrpIndex=" & strcboval & "&AppGrp=" & Server.UrlEncode(strGrpName) & "&pageid=" & strPageId & "&chk=" & viewstate("strchk") & "&frm=" & viewstate("strlink") & "&apprType=" & viewstate("ApprType"))
    '        End If
    '    End If
    'End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        '    Session("w_ApprWFAsgPur_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                 "<li><a class=""t_entity_btn"" href=""ApprovalWorkFlow.aspx?pageid=" & strPageId & """><span>Approval Group</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                 "<li><a class=""t_entity_btn"" href=""AppGrpAsg.aspx?pageid=" & strPageId & """><span>Approval Officer Assignment</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                 "<li><a class=""t_entity_btn_selected"" href=""AppGrpAsgPur.aspx?pageid=" & strPageId & """><span>Purchaser Assignment</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                 "</ul><div></div></div>"

        Session("w_ApprWFAsgPur_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("ApprWorkFlow", "ApprovalWorkFlow.aspx", "pageid=" & strPageId) & """><span>Approval Group</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("ApprWorkFlow", "AppGrpAsg.aspx", "pageid=" & strPageId) & """><span>Approval Officer Assignment</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("ApprWorkFlow", "AppGrpAsgPur.aspx", "pageid=" & strPageId) & """><span>Purchaser Assignment</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
    End Sub
End Class
