Imports AgoraLegacy
Imports eProcure.Component


Public Class AppGrpAsgPur
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
    Protected WithEvents lbldeptname As System.Web.UI.WebControls.Label
    Protected WithEvents lbldept As System.Web.UI.WebControls.Label
    Protected WithEvents lblLeftPanel As System.Web.UI.WebControls.Label
    Protected WithEvents lblRightPanel As System.Web.UI.WebControls.Label
    Protected WithEvents lblBottomNote As System.Web.UI.WebControls.Label
    Protected WithEvents lblAction As System.Web.UI.WebControls.Label

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
            End If
        End If

        If ViewState("ApprType") <> cboType.SelectedValue Then

            ViewState("ApprType") = cboType.SelectedValue

            If ViewState("ApprType") = "INV" Then

                trAppAsign.Style.Item("display") = "none"

                Div_AA.Style("display") = "none"
            ElseIf ViewState("ApprType") = "E2P" Then
                Div_AA.Style("display") = "inline"
                'lblAction.Text = "Step 1: Create, delete or modify Approval Group.<br />Step 2: Assign Approving Officer to the Selected Approval Group.<br><b>=></b> Step 3: Assign IPP Tellers to the Selected Approval Group."
                lblLeftPanel.Text = "Available E2P Tellers"
                lblRightPanel.Text = "Selected E2P Tellers"
                GenerateTab()
                lblBottomNote.Text = "a) To assign E2P Tellers to the Approval Group, choose the name from 'Available E2P Tellers' and click Assign button.<br>b) To remove/unassign E2P Tellers from the Approval Group, choose the 'Selected E2P Tellers' and click Remove button."
                'Modified for IPP Stage 2A - CH
            ElseIf ViewState("ApprType") = "BIL" Then
                Div_AA.Style("display") = "inline"
                'lblAction.Text = "Step 1: Create, delete or modify Approval Group.<br />Step 2: Assign Approving Officer to the Selected Approval Group.<br><b>=></b> Step 3: Assign IPP Tellers to the Selected Approval Group."
                lblLeftPanel.Text = "Available Billing Tellers"
                lblRightPanel.Text = "Selected Billing Tellers"
                GenerateTab()
                lblBottomNote.Text = "a) To assign Billing Tellers to the Approval Group, choose the name from 'Available Biiling Tellers' and click Assign button.<br>b) To remove/unassign Billing Tellers from the Approval Group, choose the 'Selected Billing Tellers' and click Remove button."
                '-------------------------------
            Else
                Div_AA.Style("display") = "inline"
                'lblAction.Text = "Step 1: Create, delete or modify Approval Group.<br />Step 2: Assign Approving Officer to the Selected Approval Group.<br><b>=></b> Step 3: Assign Purchaser to the Selected Approval Group."
                lblLeftPanel.Text = "Available Purchasers"
                lblRightPanel.Text = "Selected Purchasers"
                GenerateTab()
                lblBottomNote.Text = "a) To assign Purchasers to the Approval Group, choose the name from 'Available Purchasers' and click Assign button.<br>b) To remove/unassign purchaser from the Approval Group, choose the 'Selected Purchasers' and click Remove button."
            End If
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

        End If
        '//MODITFY BY MOO
        If cboGroup.SelectedIndex <> 0 Then
            Div_AA.Style("display") = "inline"
        Else
            Div_AA.Style("display") = "none"
        End If
        lnkBack.Visible = False

    End Sub

    Public Function BindGroup()
        Dim cbolist As New ListItem
        Dim dvCustom As DataView

        dvCustom = objApp.getcboApp("AGM_TYPE = '" & ViewState("ApprType") & "'")
        cboGroup.Items.Clear()
        If Not dvCustom Is Nothing Then
            Common.FillDdl(cboGroup, "AGM_GRP_NAME", "AGM_GRP_INDEX", dvCustom)
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboGroup.Items.Insert(0, cbolist)
    End Function

    Public Sub MyData_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        Bindgrid(0)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(0)
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        valGroup = cboGroup.SelectedItem.Value
        Dim ds As New DataSet

        If ViewState("ApprType") = "INV" Then
            ds = objApp.BindInvAppGroupAsg(valGroup)
        Else
            ds = objApp.BindAppGroupAsg(valGroup)
        End If

    End Function

    Public Function AppOfficerData()
        valGroup = cboGroup.SelectedItem.Value
        Dim dvcustom As DataView

        dvcustom = objApp.bindlistbox_AppOfficerData(valGroup)

    End Function

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
        strvalofficerlist = cboGroup.SelectedItem.Value
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

        If ViewState("ApprType") = "E2P" Then
            'Zulham 05072018 - PAMB
            'Added argument for appType
            dvcustom = objApp.AvailTellerByAppList(strvalofficerlist, ViewState("ApprType"))
            'Modified for IPP Stage 2A - CH
        ElseIf ViewState("ApprType") = "BIL" Then
            dvcustom = objApp.AvailBillTellerByAppList(strvalofficerlist)
            '-------------------------------
        Else
            dvcustom = objApp.AvailBuyerByAppList(strvalofficerlist, grouptype)
        End If



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
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        lstBox2.Items.Clear()
        lstbox1.Items.Clear()
        selectedData()
        SearchData()
        AppOfficerData()
        cmdAdd1.Enabled = True
        cmdAdd2.Enabled = True
    End Sub

    Private Sub cboGroup_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboGroup.SelectedIndexChanged
        If cboGroup.SelectedItem.Text = "---Select---" Then
            lstbox1.Items.Clear()
            lstBox2.Items.Clear()
            lbldeptname.Text = ""
        Else
            If viewstate("ApprType") = "INV" Then
                lbldept.Visible = False
                lbldeptname.Visible = False

                Bindgrid(0)
            Else
                If ViewState("ApprType") = "E2P" Then
                    lbldept.Visible = False
                    lbldeptname.Visible = False
                    'Arif,05062013 - For eMRS
                ElseIf ViewState("ApprType") = "MRS" Or ViewState("ApprType") = "BIL" Then 'Modified for IPP Stage 2A - CH
                    lbldept.Visible = False
                    lbldeptname.Visible = False
                    'End-Arif,05062013
                Else
                    lbldept.Visible = True
                    lbldeptname.Visible = True
                    lbldeptname.Text = objApp.getdeptname(cboGroup.SelectedItem.Value)
                End If

                Dim intchkmsg As Integer
                intchkmsg = objApp.checkconsolidatorApp(cboGroup.SelectedItem.Value)
                lstbox1.Items.Clear()
                lstBox2.Items.Clear()
                selectedData()
                SearchData()
                AppOfficerData()

            End If
        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_ApprWFAsgPur_tabs") = "<div class=""t_entity""><ul>" &
                "<li><div class=""space""></div></li>" &
                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("ApprWorkFlow", "ApprovalWorkFlow.aspx", "pageid=" & strPageId) & """><span>Approval Group</span></a></li>" &
                "<li><div class=""space""></div></li>" &
                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("ApprWorkFlow", "AppGrpAsg.aspx", "pageid=" & strPageId) & """><span>Approval Officer Assignment</span></a></li>" &
                "<li><div class=""space""></div></li>" &
                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("ApprWorkFlow", "AppGrpAsgPur.aspx", "pageid=" & strPageId) & """><span>User Assignment</span></a></li>" &
                "<li><div class=""space""></div></li>" &
                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("ApprWorkFlow", "AppGrpAsgItem.aspx", "pageid=" & strPageId) & """><span>Item Assignment</span></a></li>" &
                "<li><div class=""space""></div></li>" &
                "</ul><div></div></div>"

    End Sub
    Private Sub cboType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboType.SelectedIndexChanged
        If cboType.SelectedItem.Text = "PO" Or cboType.SelectedItem.Text = "PR" Or cboType.SelectedItem.Text = "Staff Claim" Then
            lbldept.Visible = True
            lbldeptname.Visible = True
        Else
            lbldept.Visible = False
            lbldeptname.Visible = False
        End If
    End Sub
End Class
