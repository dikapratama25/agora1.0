Imports AgoraLegacy
Imports eProcure.Component


Public Class ApprovalWorkFlow
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objApp As New ApprWorkFlow
    Dim strGrpName As String
    Dim strvalcboconsol As String
    Protected WithEvents txtGrpSearch As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmd_search As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear1 As System.Web.UI.WebControls.Button
    Protected WithEvents lbl_add_mod As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txt_add_mod As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkConsol As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblconsolidator As System.Web.UI.WebControls.Label
    Protected WithEvents cboConsol As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboDpt As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboIQCType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboIQCType2 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmd_save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear As System.Web.UI.WebControls.Button
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents validate_grp_name As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents dtgApp As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_delete As System.Web.UI.WebControls.Button
    Protected WithEvents Hide_Add2 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents divConsolidator As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Reset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lblSymbol As System.Web.UI.WebControls.Label
    Protected WithEvents val_consol As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents lblRed As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblmsg As System.Web.UI.WebControls.Label
    Protected WithEvents cboType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblConsol As System.Web.UI.WebControls.Label
    Protected WithEvents rfvType As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cvConsol As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents trGrpType As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trhid As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trIQCType As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ddlDept As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lbl1 As System.Web.UI.WebControls.Label
    Protected WithEvents trUrgentIR As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtUrgentIR As System.Web.UI.WebControls.TextBox
    Protected WithEvents trRejectMRS As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtRejectMRS As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmd_save1 As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear2 As System.Web.UI.WebControls.Button
    Protected WithEvents button3 As System.Web.UI.WebControls.Button
    Protected WithEvents trConsol As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents chkResident As System.Web.UI.WebControls.CheckBox 'Zulham 26062018 - PAMB
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
        AWConsol = 2
        AWAAO = 3
        AWBuyer = 4
        AWIndex = 5
        AWUserID = 6
        AWFO = 7
        AWFM = 8
        AWIQCType = 9
        AWDept = 10
        AWDeptcode = 11
        AWEmail1 = 12
        AWEmail2 = 13
        AWType = 14
        AWResidence = 15 'Zulham 09072018 - PAMB
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
            bindDdl()
            BindIQCTestType(cboIQCType)
            BindIQCTestType(cboIQCType2, False)
            GenerateTab()
            cmd_search.Enabled = True
            cmd_clear1.Enabled = True
            cmd_delete.Enabled = False
            cmdModify.Enabled = False
            cmd_Reset.Disabled = True
            Hide_Add2.Style("display") = "none"
            '-----New code Added By Praveen for Cbotype on 07.08.2007
            Call CheckBuyer()
            dtgApp.CurrentPageIndex = 0
            ViewState("SortAscending") = "yes"
            ViewState("SortExpression") = "AGM_GRP_NAME"
            Call bindgridApp(0)
            '-----End
            Checktype()
        End If
        'Joon (16 Sept 2011) - Issue 847 - hide the cosolidator required check box for PR
        If cboType.SelectedValue = "INV" Or cboType.SelectedValue = "PR" Or cboType.SelectedValue = "Staff Claim" Or cboType.SelectedValue = "PO" Or cboType.SelectedValue = "E2P" Or cboType.SelectedValue = "IQC" Or cboType.SelectedValue = "eMRS" Or cboType.SelectedValue = "Billing" Then 'Modified for IPP Stage 2A - CH
            'cboType.SelectedValue = "INV"
            'trGrpType.Style.Item("display") = "none"

            chkConsol.Checked = False
            chkConsol.Style("display") = "none"
            lblConsol.Style("display") = "none"
            divConsolidator.Style("display") = "none"

            If cboType.SelectedValue = "IQC" Then
                trIQCType.Style("display") = "inline"
            Else
                trIQCType.Style("display") = "none"
            End If
            'Arif,04062013
            'For eMRS
            If cboType.SelectedValue = "eMRS" Then
                trUrgentIR.Style("display") = "inline"
                trRejectMRS.Style("display") = "inline"
                trConsol.Style("display") = "none"
            Else
                trUrgentIR.Style("display") = "none"
                trRejectMRS.Style("display") = "none"
            End If
            'End-Arif,04062013
            val_consol.Enabled = False
            val_consol.EnableClientScript = False
            cvConsol.Enabled = False
            cvConsol.EnableClientScript = False

            'Zulham 26062018 - PAMB
            If cboType.SelectedValue = "E2P" Or cboType.SelectedValue = "INV" Then
                chkResident.Visible = True
            Else
                chkResident.visible = False
            End If
            'End

        Else
            'cboType.SelectedValue = "PR"
            'trGrpType.Style.Item("display") = "none"

            'If Session("Env") = "FTN" Then
            '    chkConsol.Visible = False
            '    lblConsol.Visible = False
            '    divConsolidator.Style("display") = "none"
            'Else
            '    chkConsol.Visible = True
            '    lblConsol.Visible = True
            '    divConsolidator.Style("display") = "inline"
            'End If
            chkConsol.Visible = True
            lblConsol.Visible = True
            divConsolidator.Style("display") = "inline"

            'chkConsol.Style("display") = "inline"
            'lblConsol.Style("display") = "inline"
            'divConsolidator.Style("display") = "inline"
            '-------Val_consol made as Disable becoz on save Button Click Error Msg
            ' is appearing.Chaged by Praveen on 02.08.2007
            'val_consol.Enabled = True
            val_consol.Enabled = False
            '----End
            val_consol.EnableClientScript = True
            cvConsol.Enabled = True
            cvConsol.EnableClientScript = True
        End If
        'cboType.Attributes.Add("onchange", "DisplayConsolidator();")
        cmd_delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        'Zulham 10072018 - PAMB
        'cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")


    End Sub
    Public Sub CheckBuyer()
        Dim tDS As DataSet
        Dim strsql As String
        Dim str As String
        Dim str1 As String
        strsql = "Select CM_COY_TYPE  from COMPANY_MSTR where CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"

        tDS = objDb.FillDs(strsql)
        If tDS.Tables(0).Rows.Count > 0 Then
            str = tDS.Tables(0).Rows(0).Item("CM_COY_TYPE")
        End If
        strsql = "Select CM_INV_APPR from COMPANY_MSTR where CM_COY_TYPE ='" & str & "' and CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
        tDS = objDb.FillDs(strsql)
        If tDS.Tables(0).Rows.Count > 0 Then
            str1 = tDS.Tables(0).Rows(0).Item("CM_INV_APPR")
        End If
        'If str1 = "N" Or Session("Env") = "FTN" Then
        '    cboType.Items.Add("PO")
        '    cboType.Items.Add("PR")
        'ElseIf str1 <> "N" Then
        '    cboType.Items.Add("INV")
        '    cboType.Items.Add("PO")
        '    cboType.Items.Add("PR")
        '    cboType.Items.Add("IPP")
        '    cboType.SelectedValue = "PR"
        'End If

        If str1 = "N" Then
            cboType.Items.Add("PO")
            cboType.Items.Add("PR")
            cboType.Items.Add("IQC")
            cboType.Items.Add("eMRS")
            'Chee Hong - 04/12/2014 - IPP Stage 2A Enhancement
            cboType.Items.Add("Billing")
            '-------- End ----------
            cboType.Items.Add("Staff Claim")
        ElseIf str1 <> "N" Then
            cboType.Items.Add("INV")
            cboType.Items.Add("PO")
            cboType.Items.Add("PR")
            cboType.Items.Add("E2P")
            cboType.Items.Add("IQC")
            'Arif,04062013
            'For eMRS
            cboType.Items.Add("eMRS")
            'End-Arif,04062013

            'Chee Hong - 04/12/2014 - IPP Stage 2A Enhancement
            cboType.Items.Add("Billing")
            '-------- End ----------
            cboType.Items.Add("Staff Claim")
            cboType.SelectedValue = "PR"
        End If

    End Sub
    Sub bindDdl()
        Dim ds As DataSet
        Dim objadmin As New Admin        
        ds = objadmin.getdept()
        ddlDept.Items.Clear()
        cboDpt.Items.Clear()
        If Not ds Is Nothing Then
            Dim cbolist As New ListItem
            Common.FillDdl(ddlDept, "CDM_DEPT_NAME", "CDM_DEPT_CODE", ds)
            Common.FillDdl(cboDpt, "CDM_DEPT_NAME", "CDM_DEPT_CODE", ds)
            cbolist.Value = ""
            cbolist.Text = "---Select---"
            ddlDept.Items.Insert(0, cbolist)
            cboDpt.Items.Insert(0, cbolist)
        End If

    End Sub
    Public Function BindData()
        Dim dvcustom As DataView
        dvcustom = objApp.bindcboconsol()

        cboConsol.Items.Clear()
        If Not dvcustom Is Nothing Then
            Dim cbolist As New ListItem
            Common.FillDdl(cboConsol, "two", "UM_USER_ID", dvcustom)
            cbolist.Value = ""
            cbolist.Text = "---Select---"
            cboConsol.Items.Insert(0, cbolist)
        End If

    End Function

    Function bindgridApp(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim ds As New DataSet
        Dim strValGroup, strQuery As String
        strValGroup = Me.txtGrpSearch.Text

        If cboType.SelectedValue = "IQC" Then
            strQuery = "AGM_TYPE = '" & cboType.SelectedValue & "' "
            If cboIQCType.SelectedValue <> "" Then
                strQuery &= "AND AGM_IQC_TYPE = '" & cboIQCType.SelectedValue & "' "
            End If
            If cboDpt.SelectedValue <> "" Then
                strQuery &= "AND AGM_DEPT_CODE = '" & cboDpt.SelectedValue & "' "
            End If
            'Arif,04062013
            'For eMRS
        ElseIf cboType.SelectedValue = "eMRS" Then
            strQuery = "AGM_TYPE = 'MRS' "
            'End-Arif,04062013
		'Modified for IPP Stage 2A - CH
        ElseIf cboType.SelectedValue = "Billing" Then
            strQuery = "AGM_TYPE = 'BIL' "
            '------------------------------
        ElseIf cboType.SelectedValue = "Staff Claim" Then
            strQuery = "AGM_TYPE = 'SC' "
        Else
            strQuery = "AGM_TYPE = '" & cboType.SelectedValue & "'"
        End If

        ds = objApp.getAppWorkFlow(strValGroup, strQuery)
  
        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"

        If ViewState("action") = "del" Then
            If dtgApp.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgApp.PageSize = 0 Then
                dtgApp.CurrentPageIndex = dtgApp.CurrentPageIndex - 1
                ViewState("action") = ""
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
            lblRed.Visible = True
        Else
            cmd_delete.Enabled = False
            cmdModify.Enabled = False
            cmd_Reset.Disabled = True
            dtgApp.DataBind()
            lblRed.Visible = False
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
        txtGrpSearch.Text = ""
        cboDpt.SelectedValue = ""
        cboIQCType.SelectedValue = ""
    End Sub

    Private Sub dtgApp_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgApp.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim chk As CheckBox
            Dim intTotalCell, intLoop As Integer
            chk = e.Item.FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            Dim intCnt As Integer
            Dim dsApp As New DataSet
            Dim dsAppBuyer As New DataView
            Dim lblBuyer As Label = e.Item.Cells(AWEnum.AWBuyer).FindControl("lblBuyer")
            Dim lblApproval As Label = e.Item.Cells(AWEnum.AWAAO).FindControl("lblApproval")
            Dim val As String = e.Item.Cells(AWEnum.AWIndex).Text


            'Zulham 09072018 - PAMB
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Select Case Common.parseNull(dv("AGM_RESIDENT"))
                Case "Y"
                    e.Item.Cells(AWEnum.AWResidence).Text = "Yes"
                Case "N"
                    e.Item.Cells(AWEnum.AWResidence).Text = "No"
                Case Else
                    e.Item.Cells(AWEnum.AWResidence).Text = ""
            End Select

            'End

            ' ai chu add on 05/10/2005
            ' deleted or inactive users are displayed in red colour
            Dim strColour1 As String = "<font color=Red>"
            Dim strColour2 As String = "</font>"

            '//THIS BIND DATAGRID with COMBINE 3 FIELD(Level, AO, AAO)

            If cboType.SelectedValue = "INV" Then
                dsApp = objApp.GetFinanceApprGroup(val, "FO")
            ElseIf cboType.SelectedValue = "IQC" Then
                dsApp = objApp.GetIQCGroup(val)
            ElseIf cboType.SelectedValue = "eMRS" Then
                dsApp = objApp.GetMRSGroup(val)
			'Modified for IPP Stage 2A - CH
            ElseIf cboType.SelectedValue = "Billing" Then
                dsApp = objApp.GetBilGroup(val)
			'-------------------------------
            Else
				'Zulham 03082018 - PAMB
                dsApp = objApp.GetGroup(val, cboType.SelectedValue)
            End If

            If Not dsApp Is Nothing Then
                intCnt = dsApp.Tables(0).Rows.Count
                For intLoop = 0 To intCnt - 1
                    If intLoop = 0 Then
                        lblApproval.Text = Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AGA_SEQ"), "-")
                        lblApproval.Text &= "."
                        If dsApp.Tables(0).Rows(intLoop)("AO_ACTIVE") = 1 Then
                            lblApproval.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AO_NAME"), "-")
                        Else
                            lblApproval.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AO_NAME"), "-") & strColour2
                        End If

                        lblApproval.Text &= " / "
                        If dsApp.Tables(0).Rows(intLoop)("AAO_ACTIVE") = 1 Then
                            lblApproval.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AAO_NAME"), "-")
                        Else
                            lblApproval.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AAO_NAME"), "-") & strColour2
                        End If

                    Else
                        lblApproval.Text &= "<br>"
                        lblApproval.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AGA_SEQ"), "-")
                        lblApproval.Text &= "."
                        If dsApp.Tables(0).Rows(intLoop)("AO_ACTIVE") = 1 Then
                            lblApproval.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AO_NAME"), "-")
                        Else
                            lblApproval.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AO_NAME"), "-") & strColour2
                        End If

                        lblApproval.Text &= " / "
                        If dsApp.Tables(0).Rows(intLoop)("AAO_ACTIVE") = 1 Then
                            lblApproval.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AAO_NAME"), "-")
                        Else
                            lblApproval.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AAO_NAME"), "-") & strColour2
                        End If

                    End If
                Next
            Else
                lblApproval.Text = "-"
            End If

            If ViewState("type") = "IPP" Then
                'For FM
                Dim lblfm As Label = e.Item.Cells(AWEnum.AWFM).FindControl("lblfm")
                dsApp = New DataSet
                dsApp = objApp.GetIPPFMGroup(val)
                If Not dsApp Is Nothing Then
                    intCnt = dsApp.Tables(0).Rows.Count
                    For intLoop = 0 To intCnt - 1
                        If intLoop = 0 Then
                            lblfm.Text = Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AGFM_SEQ"), "-")
                            lblfm.Text &= "."
                            If dsApp.Tables(0).Rows(intLoop)("FM_ACTIVE") = 1 Then
                                lblfm.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("FM_NAME"), "-")
                            Else
                                lblfm.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("FM_NAME"), "-") & strColour2
                            End If

                            lblfm.Text &= " / "
                            If dsApp.Tables(0).Rows(intLoop)("AFM_ACTIVE") = 1 Then
                                lblfm.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFM_NAME"), "-")
                            Else
                                lblfm.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFM_NAME"), "-") & strColour2
                            End If

                            lblfm.Text &= " / "
                            If dsApp.Tables(0).Rows(intLoop)("AFM2_ACTIVE") = 1 Then
                                lblfm.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFM_NAME2"), "-")
                            Else
                                lblfm.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFM_NAME2"), "-") & strColour2
                            End If

                            lblfm.Text &= " / "
                            If dsApp.Tables(0).Rows(intLoop)("AFM3_ACTIVE") = 1 Then
                                lblfm.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFM_NAME3"), "-")
                            Else
                                lblfm.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFM_NAME3"), "-") & strColour2
                            End If

                            lblfm.Text &= " / "
                            If dsApp.Tables(0).Rows(intLoop)("AFM4_ACTIVE") = 1 Then
                                lblfm.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFM_NAME4"), "-")
                            Else
                                lblfm.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFM_NAME4"), "-") & strColour2
                            End If

                        Else
                            lblfm.Text &= "<br>"
                            lblfm.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AGFM_SEQ"), "-")
                            lblfm.Text &= "."
                            If dsApp.Tables(0).Rows(intLoop)("FM_ACTIVE") = 1 Then
                                lblfm.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("FM_NAME"), "-")
                            Else
                                lblfm.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("FM_NAME"), "-") & strColour2
                            End If

                            lblfm.Text &= " / "
                            If dsApp.Tables(0).Rows(intLoop)("AFM_ACTIVE") = 1 Then
                                lblfm.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFM_NAME"), "-")
                            Else
                                lblfm.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFM_NAME"), "-") & strColour2
                            End If

                            lblfm.Text &= " / "
                            If dsApp.Tables(0).Rows(intLoop)("AFM2_ACTIVE") = 1 Then
                                lblfm.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFM_NAME2"), "-")
                            Else
                                lblfm.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFM_NAME2"), "-") & strColour2
                            End If

                            lblfm.Text &= " / "
                            If dsApp.Tables(0).Rows(intLoop)("AFM3_ACTIVE") = 1 Then
                                lblfm.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFM_NAME3"), "-")
                            Else
                                lblfm.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFM_NAME3"), "-") & strColour2
                            End If

                            lblfm.Text &= " / "
                            If dsApp.Tables(0).Rows(intLoop)("AFM4_ACTIVE") = 1 Then
                                lblfm.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFM_NAME4"), "-")
                            Else
                                lblfm.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFM_NAME4"), "-") & strColour2
                            End If
                        End If
                    Next
                Else
                    lblfm.Text = "-"
                End If
                Dim lblfo As Label = e.Item.Cells(AWEnum.AWFM).FindControl("lblfo")
                dsApp = New DataSet
                dsApp = objApp.GetIPPFOGroup(val)
                'For FO
                If Not dsApp Is Nothing Then
                    intCnt = dsApp.Tables(0).Rows.Count
                    For intLoop = 0 To intCnt - 1
                        If intLoop = 0 Then
                            lblfo.Text = Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AGFO_SEQ"), "-")
                            lblfo.Text &= "."
                            If dsApp.Tables(0).Rows(intLoop)("FO_ACTIVE") = 1 Then
                                lblfo.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("FO_NAME"), "-")
                            Else
                                lblfo.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("FO_NAME"), "-") & strColour2
                            End If

                            lblfo.Text &= " / "
                            If dsApp.Tables(0).Rows(intLoop)("AFO_ACTIVE") = 1 Then
                                lblfo.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFO_NAME"), "-")
                            Else
                                lblfo.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFO_NAME"), "-") & strColour2
                            End If

                            lblfo.Text &= " / "
                            If dsApp.Tables(0).Rows(intLoop)("AFO2_ACTIVE") = 1 Then
                                lblfo.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFO_NAME2"), "-")
                            Else
                                lblfo.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFO_NAME3"), "-") & strColour2
                            End If

                            lblfo.Text &= " / "
                            If dsApp.Tables(0).Rows(intLoop)("AFO3_ACTIVE") = 1 Then
                                lblfo.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFO_NAME3"), "-")
                            Else
                                lblfo.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFO_NAME3"), "-") & strColour2
                            End If

                            lblfo.Text &= " / "
                            If dsApp.Tables(0).Rows(intLoop)("AFO4_ACTIVE") = 1 Then
                                lblfo.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFO_NAME4"), "-")
                            Else
                                lblfo.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFO_NAME4"), "-") & strColour2
                            End If

                        Else
                            lblfo.Text &= "<br>"
                            lblfo.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AGFO_SEQ"), "-")
                            lblfo.Text &= "."
                            If dsApp.Tables(0).Rows(intLoop)("FO_ACTIVE") = 1 Then
                                lblfo.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("FO_NAME"), "-")
                            Else
                                lblfo.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("FO_NAME"), "-") & strColour2
                            End If

                            lblfo.Text &= " / "
                            If dsApp.Tables(0).Rows(intLoop)("AFO_ACTIVE") = 1 Then
                                lblfo.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFO_NAME"), "-")
                            Else
                                lblfo.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFO_NAME"), "-") & strColour2
                            End If

                            lblfo.Text &= " / "
                            If dsApp.Tables(0).Rows(intLoop)("AFO2_ACTIVE") = 1 Then
                                lblfo.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFO_NAME2"), "-")
                            Else
                                lblfo.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFO_NAME2"), "-") & strColour2
                            End If

                            lblfo.Text &= " / "
                            If dsApp.Tables(0).Rows(intLoop)("AFO3_ACTIVE") = 1 Then
                                lblfo.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFO_NAME3"), "-")
                            Else
                                lblfo.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFO_NAME3"), "-") & strColour2
                            End If

                            lblfo.Text &= " / "
                            If dsApp.Tables(0).Rows(intLoop)("AFO4_ACTIVE") = 1 Then
                                lblfo.Text &= Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFO_NAME4"), "-")
                            Else
                                lblfo.Text &= strColour1 & Common.parseNull(dsApp.Tables(0).Rows(intLoop)("AFO_NAME4"), "-") & strColour2
                            End If
                        End If
                    Next
                Else
                    lblfo.Text = "-"
                End If
            End If
            '//THIS BIND DATAGRID with COMBINE 1 FIELD(Buyer)
            If cboType.SelectedValue = "INV" Then
                dsAppBuyer = Nothing
                Dim ds As DataSet = objApp.GetFinanceApprGroup(val, "FM")

                If Not ds Is Nothing Then
                    If ds.Tables.Count > 0 Then
                        dsAppBuyer = ds.Tables(0).DefaultView
                    End If
                End If

                If Not dsAppBuyer Is Nothing Then
                    intCnt = dsAppBuyer.Table.Rows.Count
                    For intLoop = 0 To intCnt - 1
                        If intLoop = 0 Then
                            lblBuyer.Text = Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AGA_SEQ"), "-")
                            lblBuyer.Text &= "."
                            If dsAppBuyer.Table.Rows(intLoop)("AO_ACTIVE") = 1 Then
                                lblBuyer.Text &= Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AO_NAME"), "-")
                            Else
                                lblBuyer.Text &= strColour1 & Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AO_NAME"), "-") & strColour2
                            End If

                            lblBuyer.Text &= " / "
                            If dsAppBuyer.Table.Rows(intLoop)("AAO_ACTIVE") = 1 Then
                                lblBuyer.Text &= Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AAO_NAME"), "-")
                            Else
                                lblBuyer.Text &= strColour1 & Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AAO_NAME"), "-") & strColour2
                            End If

                        Else
                            lblBuyer.Text &= "<br>"
                            lblBuyer.Text &= Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AGA_SEQ"), "-")
                            lblBuyer.Text &= "."
                            If dsAppBuyer.Table.Rows(intLoop)("AO_ACTIVE") = 1 Then
                                lblBuyer.Text &= Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AO_NAME"), "-")
                            Else
                                lblBuyer.Text &= strColour1 & Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AO_NAME"), "-") & strColour2
                            End If

                            lblBuyer.Text &= " / "
                            If dsAppBuyer.Table.Rows(intLoop)("AAO_ACTIVE") = 1 Then
                                lblBuyer.Text &= Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AAO_NAME"), "-")
                            Else
                                lblBuyer.Text &= strColour1 & Common.parseNull(dsAppBuyer.Table.Rows(intLoop)("AAO_NAME"), "-") & strColour2
                            End If
                        End If
                    Next
                Else
                    lblBuyer.Text = ""
                End If


            Else
                dsAppBuyer = objApp.SelectedBuyerByAppList(val)

                If Not dsAppBuyer Is Nothing Then
                    intCnt = dsAppBuyer.Count
                    For intLoop = 0 To intCnt - 1
                        If intLoop = 0 Then
                            If dsAppBuyer(intLoop)("USER_STATUS") = 1 Then
                                lblBuyer.Text = Common.parseNull(dsAppBuyer(intLoop)("UM_USER_NAME"))
                            Else
                                lblBuyer.Text = strColour1 & Common.parseNull(dsAppBuyer(intLoop)("UM_USER_NAME")) & strColour2
                            End If
                        Else
                            lblBuyer.Text &= "<br>"
                            If dsAppBuyer(intLoop)("USER_STATUS") = 1 Then
                                lblBuyer.Text &= Common.parseNull(dsAppBuyer(intLoop)("UM_USER_NAME"))
                            Else
                                lblBuyer.Text &= strColour1 & Common.parseNull(dsAppBuyer(intLoop)("UM_USER_NAME")) & strColour2
                            End If
                        End If
                    Next
                Else
                    lblBuyer.Text = "-"
                End If
            End If

			'Modified for IPP Stage 2A - CH
            If cboType.SelectedValue = "Billing" Then
                e.Item.Cells(AWEnum.AWType).Text = cboType.SelectedValue
            End If
            '-----
        End If
    End Sub

    Private Sub cmd_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_clear.Click
        If hidMode.Value = "a" Then
            txt_add_mod.Text = ""
            'cboType.SelectedIndex = 2
            ddlDept.SelectedIndex = 0
            cboIQCType2.SelectedIndex = 0
            'If Session("Env") <> "FTN" Then
            '    If cboConsol.SelectedIndex <> -1 Then
            '        cboConsol.SelectedIndex = 0
            '        chkConsol.Checked = False
            '    End If
            'End If
            If cboConsol.SelectedIndex <> -1 Then
                cboConsol.SelectedIndex = 0
                chkConsol.Checked = False
            End If
        Else
            txt_add_mod.Text = ViewState("oldvalue")
            Dim valuserid As String = ViewState("oldvalue2")
            Common.SelDdl(valuserid, cboConsol, True, False)
            cboType.SelectedValue = ViewState("oldvalue3")
            If ViewState("oldvalue4") = "&nbsp;" Then
                ddlDept.SelectedValue = ""
            Else
                ddlDept.SelectedValue = ViewState("oldvalue4")
            End If
            If ViewState("oldvalue5") = "&nbsp;" Then
                cboIQCType2.SelectedIndex = 0
            Else
                cboIQCType2.SelectedValue = ViewState("oldvalue5")
            End If

            'If Session("Env") <> "FTN" Then
            '    chkConsol.Checked = ViewState("consol")
            '    chkConsol_CheckedChanged(sender, e)
            'End If
            chkConsol.Checked = ViewState("consol")
            chkConsol_CheckedChanged(sender, e)
        End If

        'If Session("Env") = "FTN" Then
        '    Me.dtgApp.Columns(2).Visible = False
        'End If

    End Sub

    Private Sub cmd_delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_delete.Click
        '//To prevent "No Record Found" msg
        txtGrpSearch.Text = ""

        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim strgrpName, strconsol, strlev, strO, strAO, strBuyer, strIndex As String
        Dim i As Integer = 0
        Dim strFailDept As String

        For Each dgItem In dtgApp.Items

            chk = dgItem.FindControl("chkSelection")

            strIndex = dgItem.Cells.Item(AWEnum.AWIndex).Text

            If chk.Checked Then
                If objApp.AttachedToDept(strIndex) Then
                    strFailDept &= dgItem.Cells.Item(AWEnum.AWGrpName).Text & ","
                Else
                    objApp.DelAppWorkFlow(strIndex)
                End If
            End If
            i = i + 1
        Next

        If strFailDept <> "" Then
            strFailDept = strFailDept.Replace(",", vbCrLf)
            Common.NetMsgbox(Me, "Unable to delete the workflow group(s).", MsgBoxStyle.Exclamation)

        Else
            Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        End If

        viewstate("action") = "del"
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

        If cboType.SelectedValue = "INV" Then
            dtgApp.Columns(AWEnum.AWAAO).HeaderStyle.Width = 250
            dtgApp.Columns(AWEnum.AWConsol).Visible = False
            dtgApp.Columns(AWEnum.AWBuyer).Visible = True
            dtgApp.Columns(AWEnum.AWBuyer).HeaderText = "Approving Finance Manager / Alternative Approving Finance Manager"
            dtgApp.Columns(AWEnum.AWFO).Visible = False
            dtgApp.Columns(AWEnum.AWFM).Visible = False
            dtgApp.Columns(AWEnum.AWIQCType).Visible = False
            dtgApp.Columns(AWEnum.AWDept).Visible = False
            dtgApp.Columns(AWEnum.AWEmail1).Visible = False
            dtgApp.Columns(AWEnum.AWEmail2).Visible = False
        ElseIf cboType.SelectedValue = "E2P" Then
            dtgApp.Columns(AWEnum.AWAAO).HeaderStyle.Width = 250
            dtgApp.Columns(AWEnum.AWConsol).Visible = False
            dtgApp.Columns(AWEnum.AWBuyer).Visible = False
            dtgApp.Columns(AWEnum.AWFO).Visible = True
            dtgApp.Columns(AWEnum.AWFM).Visible = True
            dtgApp.Columns(AWEnum.AWIQCType).Visible = False
            dtgApp.Columns(AWEnum.AWDept).Visible = False
            dtgApp.Columns(AWEnum.AWEmail1).Visible = False
            dtgApp.Columns(AWEnum.AWEmail2).Visible = False
        ElseIf cboType.SelectedValue = "PR" Then
            dtgApp.Columns(AWEnum.AWConsol).Visible = False
            dtgApp.Columns(AWEnum.AWBuyer).Visible = True
            dtgApp.Columns(AWEnum.AWAAO).HeaderStyle.Width = 300
            dtgApp.Columns(AWEnum.AWFO).Visible = False
            dtgApp.Columns(AWEnum.AWFM).Visible = False
            dtgApp.Columns(AWEnum.AWIQCType).Visible = False
            dtgApp.Columns(AWEnum.AWDept).Visible = True
            dtgApp.Columns(AWEnum.AWEmail1).Visible = False
            dtgApp.Columns(AWEnum.AWEmail2).Visible = False
        ElseIf cboType.SelectedValue = "IQC" Then
            dtgApp.Columns(AWEnum.AWConsol).Visible = False
            dtgApp.Columns(AWEnum.AWBuyer).Visible = False
            dtgApp.Columns(AWEnum.AWAAO).HeaderStyle.Width = 300
            dtgApp.Columns(AWEnum.AWFO).Visible = False
            dtgApp.Columns(AWEnum.AWFM).Visible = False
            dtgApp.Columns(AWEnum.AWIQCType).Visible = True
            dtgApp.Columns(AWEnum.AWDept).Visible = True
            dtgApp.Columns(AWEnum.AWEmail1).Visible = False
            dtgApp.Columns(AWEnum.AWEmail2).Visible = False
            'Arif,04062013
            'For eMRS
        ElseIf cboType.SelectedValue = "eMRS" Then
            dtgApp.Columns(AWEnum.AWConsol).Visible = False
            dtgApp.Columns(AWEnum.AWBuyer).Visible = True
            dtgApp.Columns(AWEnum.AWBuyer).HeaderText = "Requester"
            dtgApp.Columns(AWEnum.AWAAO).HeaderStyle.Width = 300
            dtgApp.Columns(AWEnum.AWFO).Visible = False
            dtgApp.Columns(AWEnum.AWFM).Visible = False
            dtgApp.Columns(AWEnum.AWIQCType).Visible = False
            dtgApp.Columns(AWEnum.AWDept).Visible = False
            dtgApp.Columns(AWEnum.AWEmail1).Visible = True
            dtgApp.Columns(AWEnum.AWEmail2).Visible = True
            'End-Arif,04062013
		'Modified for IPP Stage 2A - CH
        ElseIf cboType.SelectedValue = "Billing" Then
            dtgApp.Columns(AWEnum.AWAAO).HeaderStyle.Width = 300
            dtgApp.Columns(AWEnum.AWAAO).HeaderText = "Billing Approver / Alternative Billing Approver"
            dtgApp.Columns(AWEnum.AWConsol).Visible = False
            dtgApp.Columns(AWEnum.AWBuyer).Visible = True
            dtgApp.Columns(AWEnum.AWBuyer).HeaderText = "Billing Teller"
            dtgApp.Columns(AWEnum.AWFO).Visible = False
            dtgApp.Columns(AWEnum.AWFM).Visible = False
            dtgApp.Columns(AWEnum.AWIQCType).Visible = False
            dtgApp.Columns(AWEnum.AWDept).Visible = False
            dtgApp.Columns(AWEnum.AWEmail1).Visible = False
            dtgApp.Columns(AWEnum.AWEmail2).Visible = False
            '-------------------------------
        ElseIf cboType.SelectedValue = "Staff Claim" Then
            dtgApp.Columns(AWEnum.AWConsol).Visible = False
            dtgApp.Columns(AWEnum.AWBuyer).Visible = True
            dtgApp.Columns(AWEnum.AWAAO).HeaderStyle.Width = 300
            dtgApp.Columns(AWEnum.AWFO).Visible = False
            dtgApp.Columns(AWEnum.AWFM).Visible = False
            dtgApp.Columns(AWEnum.AWIQCType).Visible = False
            dtgApp.Columns(AWEnum.AWDept).Visible = True
            dtgApp.Columns(AWEnum.AWEmail1).Visible = False
            dtgApp.Columns(AWEnum.AWEmail2).Visible = False
        Else
            dtgApp.Columns(AWEnum.AWAAO).HeaderStyle.Width = 300
            dtgApp.Columns(AWEnum.AWConsol).Visible = False
            dtgApp.Columns(AWEnum.AWBuyer).Visible = True
            dtgApp.Columns(AWEnum.AWBuyer).HeaderText = "Buyer"
            dtgApp.Columns(AWEnum.AWFO).Visible = False
            dtgApp.Columns(AWEnum.AWFM).Visible = False
            dtgApp.Columns(AWEnum.AWIQCType).Visible = False
            dtgApp.Columns(AWEnum.AWDept).Visible = True
            dtgApp.Columns(AWEnum.AWEmail1).Visible = False
            dtgApp.Columns(AWEnum.AWEmail2).Visible = False
        End If

        'If Session("Env") = "FTN" Then
        '    dtgApp.Columns(3).HeaderText = "Approving Officer"
        '    dtgApp.Columns(4).HeaderText = "Purchaser"
        'Else
        '    If cboType.SelectedValue <> "INV" Then
        '        dtgApp.Columns(3).HeaderText = "Approving Officer / Alternative Approving Officer"
        '        dtgApp.Columns(4).HeaderText = "Buyer"
        '    End If
        'End If

        If cboType.SelectedValue <> "INV" And cboType.SelectedValue <> "eMRS" And cboType.SelectedValue <> "Billing" Then 'Modified for IPP Stage 2A - CH
            dtgApp.Columns(AWEnum.AWAAO).HeaderText = "Approving Officer / Alternative Approving Officer"
            dtgApp.Columns(AWEnum.AWBuyer).HeaderText = "Buyer"
        End If
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        '// to unset the value in the combo box

        Hide_Add2.Style("display") = "inline"
        validate_grp_name.Enabled = True
        txt_add_mod.Text = ""
        txtUrgentIR.Text = ""
        txtRejectMRS.Text = ""
        cboIQCType2.SelectedIndex = 0
        ddlDept.SelectedIndex = 0
        Me.cboIQCType2.Enabled = True
        Me.lbl_add_mod.Text = "add"
        cmd_clear.Text = "Clear"
        cmd_clear2.Text = "Clear"
        hidMode.Value = "a"
        'cboType.SelectedIndex = 0

        If cboType.SelectedValue <> "INV" And cboType.SelectedValue <> "IQC" And cboType.SelectedValue <> "PR" And cboType.SelectedValue <> "Staff Claim" And cboType.SelectedValue <> "PO" And cboType.SelectedValue <> "E2P" And cboType.SelectedValue <> "eMRS" And cboType.SelectedValue <> "Billing" Then 'Modified for IPP Stage 2A - CH
            CheckParam()
            chkConsol.Style("display") = "inline"
            lblConsol.Style("display") = "inline"
        End If

        cboType.Enabled = False
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
        Dim strResident As String

        If chkConsol.Checked = True Then
            strvalchk = "T"
        Else
            strvalchk = "F"
        End If

        'Zulham 26062018 - PAMB
        If chkResident.Visible Then
            If chkResident.Checked Then
                strResident = "Y"
            ElseIf Not chkResident.Checked Then
                strResident = "N"
            End If
        ElseIf Not chkResident.Visible Then
            strResident = ""
        End If
        'End

        'Arif,04062013
        'Add value eMRS
        If cboType.SelectedValue = "PO" Or cboType.SelectedValue = "IQC" Or cboType.SelectedValue = "eMRS" Or cboType.SelectedValue = "Billing" Then 'Modified for IPP Stage 2A - CH
            strvalcboconsol = ""
            chkConsol.Checked = False
            chkConsol.Style("display") = "none"
            lblConsol.Style("display") = "none"
        ElseIf cboType.SelectedValue = "INV" Then
            strvalcboconsol = ""
            chkConsol.Checked = False
        End If

        ''Arif,04062013
        ''For eMRS
        'If selected = "add" And cboType.SelectedItem.Value = "eMRS" Then
        '    Me.lbl_add_mod.Text = "add"
        '    'intmsgno = objApp.AddAppWorkFlow2(txt_add_mod.Text, strvalcboconsol, "ooo", txtUrgentIR.Text, txtRejectMRS.Text)
        'End If
        ''End-Arif,04062013

        If selected = "add" Then
            Me.lbl_add_mod.Text = "add"
            '//// TO CATER DIFFERENT SITUATION during ADD
            If chkConsol.Checked = True Then
                If strvalcboconsol = "" Then
                    intmsgno = objApp.AddAppWorkFlow(txt_add_mod.Text, cboConsol.SelectedItem.Value, cboType.SelectedValue)
                Else
                    intmsgno = objApp.AddAppWorkFlow(txt_add_mod.Text, strvalcboconsol, cboType.SelectedValue)
                End If
                'Arif, 04062013
                'For eMRS
            ElseIf cboType.SelectedValue = "eMRS" Then
                intmsgno = objApp.AddAppWorkFlow(txt_add_mod.Text, strvalcboconsol, "MRS", ddlDept.SelectedItem.Value, cboIQCType2.SelectedItem.Value, txtUrgentIR.Text, txtRejectMRS.Text)
                'End-Arif,04062013
            ElseIf cboType.SelectedValue = "IQC" Then
                intmsgno = objApp.AddAppWorkFlow(txt_add_mod.Text, strvalcboconsol, cboType.SelectedValue, ddlDept.SelectedItem.Value, cboIQCType2.SelectedItem.Value)
                'Modified for IPP Stage 2A - CH
            ElseIf cboType.SelectedValue = "Billing" Then
                intmsgno = objApp.AddAppWorkFlow(txt_add_mod.Text, strvalcboconsol, "BIL", ddlDept.SelectedItem.Value)
                '------------------------------
            ElseIf cboType.SelectedValue = "Staff Claim" Then
                intmsgno = objApp.AddAppWorkFlow(txt_add_mod.Text, strvalcboconsol, "SC", ddlDept.SelectedItem.Value)
            Else
                'Zulham 26062018 - PAMB
                'Added strResident
                intmsgno = objApp.AddAppWorkFlow(txt_add_mod.Text, strvalcboconsol, cboType.SelectedValue, ddlDept.SelectedItem.Value, isResident:=strResident)
            End If
            '////

            Select Case intmsgno
                Case WheelMsgNum.Save
                    'strRedirect = "AppGrpAsg.aspx?grp=" & Server.UrlEncode(txt_add_mod.Text) & "&chk=" & strvalchk & "&apprType=" & cboType.SelectedValue & "&frm=AW&pageid=" & strPageId
                    'strmsg = MsgRecordSave
                    'txt_add_mod.Text = ""
                    'If chkConsol.Checked = True Then
                    '    cboConsol.SelectedIndex = 0
                    'ElseIf chkConsol.Checked = False Then
                    '    val_consol.Enabled = False
                    'End If
                    'Common.NetPrompt(Me, MsgRecordSave & """& vbCrLf & ""Proceed to Approval Group Assignment?", strRedirect)
                    Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
                Case WheelMsgNum.Duplicate
                    strmsg = MsgRecordDuplicate
                    If chkConsol.Checked = False Then
                        val_consol.Enabled = False
                    End If
                    Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
                Case WheelMsgNum.NotSave
                    strmsg = MsgRecordNotSave
                    Common.NetMsgbox(Me, strmsg, MsgBoxStyle.Information)
            End Select

        ElseIf selected = "Mod" Then
            Me.lbl_add_mod.Text = "modify"
            '//// TO CATER DIFFERENT SITUATION during MODIFY
            If (ViewState("oldvalue2") = "&nbsp;" Or ViewState("oldvalue2") = "") And cboType.SelectedValue = "eMRS" Then
                intmsgno = objApp.modAppWorkFlow(hidIndex.Value, txt_add_mod.Text, strvalcboconsol, "MRS", ViewState("oldvalue"), ddlDept.SelectedItem.Value, "", txtUrgentIR.Text, txtRejectMRS.Text)
                'Modified for IPP Stage 2A - CH
            ElseIf (ViewState("oldvalue2") = "&nbsp;" Or ViewState("oldvalue2") = "") And cboType.SelectedValue = "Billing" Then
                intmsgno = objApp.modAppWorkFlow(hidIndex.Value, txt_add_mod.Text, strvalcboconsol, "BIL", ViewState("oldvalue"), ddlDept.SelectedItem.Value, cboIQCType2.SelectedItem.Value)
                '------------------------------
            ElseIf (ViewState("oldvalue2") = "&nbsp;" Or ViewState("oldvalue2") = "") And cboType.SelectedValue = "Staff Claim" Then
                intmsgno = objApp.modAppWorkFlow(hidIndex.Value, txt_add_mod.Text, strvalcboconsol, "SC", ViewState("oldvalue"), ddlDept.SelectedItem.Value, cboIQCType2.SelectedItem.Value)
            ElseIf ViewState("oldvalue2") = "&nbsp;" Or ViewState("oldvalue2") = "" Then
                'Zulham 26062018 - PAMB
                'Added isResident
                intmsgno = objApp.modAppWorkFlow(hidIndex.Value, txt_add_mod.Text, strvalcboconsol, cboType.SelectedValue, ViewState("oldvalue"), ddlDept.SelectedItem.Value, cboIQCType2.SelectedItem.Value, isResident:=strResident)
            ElseIf strvalcboconsol <> "" Then
                intmsgno = objApp.modAppWorkFlow(hidIndex.Value, txt_add_mod.Text, strvalcboconsol, cboType.SelectedValue, ViewState("oldvalue"))
            ElseIf strvalcboconsol = "" And ViewState("oldvalue2") <> "" Then
                If chkConsol.Checked = False Then
                    intmsgno = objApp.modAppWorkFlow(hidIndex.Value, txt_add_mod.Text, strvalcboconsol, cboType.SelectedValue, ViewState("oldvalue"))
                Else
                    intmsgno = objApp.modAppWorkFlow(hidIndex.Value, txt_add_mod.Text, ViewState("oldvalue2"), cboType.SelectedValue, ViewState("oldvalue"))
                End If
            Else
                intmsgno = objApp.modAppWorkFlow(hidIndex.Value, txt_add_mod.Text, ViewState("oldvalue2"), cboType.SelectedValue, ViewState("oldvalue"))
            End If
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
                    Common.SelDdl(ViewState("oldvalue2"), cboConsol, True, False)
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
        Dim counter = 0  'Zulham 10072018 - PAMB
        Hide_Add2.Style("display") = "inline"
        cmd_clear.Text = "Reset"
        cmd_clear2.Text = "Reset"
        hidMode.Value = "m"
        For Each dgItem In dtgApp.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then

                'Zulham 10072018 - PAMB
                'Use this checker for multiple selection instead of javascript
                counter += 1
                If counter > 1 Then
                    Hide_Add2.Style("display") = "none" 'Zulham 11072018 - PAMB
                    Common.NetMsgbox(Me, "Please choose only one selection.", MsgBoxStyle.Information)
                    Exit For
                End If
                'End

                hidIndex.Value = dgItem.Cells(AWEnum.AWIndex).Text
                txt_add_mod.Text = dgItem.Cells(AWEnum.AWGrpName).Text
                If dgItem.Cells.Item(AWEnum.AWEmail1).Text = "&nbsp;" Then
                    txtUrgentIR.Text = ""
                Else
                    txtUrgentIR.Text = dgItem.Cells.Item(AWEnum.AWEmail1).Text
                End If
                If dgItem.Cells.Item(AWEnum.AWEmail2).Text = "&nbsp;" Then
                    txtRejectMRS.Text = ""
                Else
                    txtRejectMRS.Text = dgItem.Cells.Item(AWEnum.AWEmail2).Text
                End If
                If dgItem.Cells.Item(AWEnum.AWDeptcode).Text = "&nbsp;" Then
                    ddlDept.SelectedValue = ""
                Else
                    ddlDept.SelectedValue = dgItem.Cells.Item(AWEnum.AWDeptcode).Text
                End If
                If dgItem.Cells.Item(AWEnum.AWIQCType).Text = "&nbsp;" Then
                    cboIQCType2.SelectedIndex = 0
                Else
                    cboIQCType2.SelectedValue = dgItem.Cells.Item(AWEnum.AWIQCType).Text
                End If

                ViewState("oldvalue") = dgItem.Cells(AWEnum.AWGrpName).Text
                ViewState("oldvalue2") = dgItem.Cells.Item(AWEnum.AWUserID).Text
                ViewState("oldvalue3") = dgItem.Cells.Item(AWEnum.AWType).Text
                ViewState("oldvalue4") = dgItem.Cells.Item(AWEnum.AWDeptcode).Text
                ViewState("oldvalue5") = dgItem.Cells.Item(AWEnum.AWIQCType).Text
                ViewState("oldvalue6") = txtUrgentIR.Text
                ViewState("oldvalue7") = txtRejectMRS.Text
                'cboType.SelectedValue = dgItem.Cells.Item(AWEnum.AWType).Text
                '//check for consolidator,modify by Moo
                '&nbsp;"
                If Not (dgItem.Cells(AWEnum.AWConsol).Text = "" Or dgItem.Cells(AWEnum.AWConsol).Text = "&nbsp;") Then
                    chkConsol.Style("display") = "inline"
                    lblConsol.Style("display") = "inline"
                    chkConsol.Checked = True
                    ViewState("consol") = True
                Else
                    chkConsol.Style("display") = "none"
                    lblConsol.Style("display") = "none"
                    chkConsol.Checked = False
                    ViewState("consol") = False
                End If
                chkConsol_CheckedChanged(sender, e)
                If Not (dgItem.Cells(AWEnum.AWConsol).Text = "" Or dgItem.Cells(AWEnum.AWConsol).Text = "&nbsp;") Then
                    Common.SelDdl(dgItem.Cells(AWEnum.AWUserID).Text, cboConsol)
                End If

                'Zulham 26062018 - PAMB
                'Get agm_resident value
                Dim strResident = objDb.GetVal("SELECT IFNULL(agm_resident,"""") FROM approval_grp_mstr WHERE agm_grp_index = " & hidIndex.Value)
                If strResident = "Y" Then
                    chkResident.Checked = True
                Else
                    chkResident.Checked = False
                End If
                'End
                'Exit For 'Zulham 10072018 - PAMB 'Commented
            End If
            i = i + 1
        Next
        'If Session("Env") = "FTN" Then
        '    Me.dtgApp.Columns(2).Visible = False
        'End If

        Me.cboIQCType2.Enabled = False
        Me.lbl_add_mod.Text = "modify"
        cboType.Enabled = False
        cmdModify.Enabled = True
    End Sub

    Private Sub cmd_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_save.Click
        '//To prevent "No Record Found" msg
        'ViewState("oldvalue") = txt_add_mod.Text
        txtGrpSearch.Text = ""
        lblmsg.Text = ""
        If strvalcboconsol = "" Then
            If hidMode.Value = "a" Then
                Me.view("add", "")
            ElseIf hidMode.Value = "m" Then
                Me.view("Mod", "")
            End If
        Else
            strvalcboconsol = cboConsol.SelectedItem.Value

            If hidMode.Value = "a" Then
                Me.view("add", strvalcboconsol)
            ElseIf hidMode.Value = "m" Then
                Me.view("Mod", strvalcboconsol)
            End If

        End If

        cboType.Enabled = True
        bindgridApp(0)
    End Sub

    Private Sub cboConsol_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboConsol.SelectedIndexChanged
        If cboConsol.SelectedItem.Text <> "---Select---" Then
            strvalcboconsol = cboConsol.SelectedItem.Value
            'Else
            '    Common.NetMsgbox(Me, MsgNoRecord)
        End If
    End Sub

    Private Sub chkConsol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkConsol.CheckedChanged

        If chkConsol.Checked = True Then
            divConsolidator.Style("display") = "inline"
            lblSymbol.Visible = True
            BindData()
            val_consol.Enabled = True
        Else
            val_consol.Enabled = False
            divConsolidator.Style("display") = "none"
            lblSymbol.Visible = False
        End If

    End Sub
    Function CheckParam()
        Dim intchkmsg As Integer
        intchkmsg = objApp.checkconsolidatorParam()
        Select Case intchkmsg
            Case 0
                chkConsol.Checked = False
                divConsolidator.Style("display") = "none"
            Case 1
                chkConsol.Checked = True

                divConsolidator.Style("display") = "inline"
                lblSymbol.Visible = True
                BindData()
                val_consol.Enabled = True
        End Select
    End Function
    Sub Checktype()
        If cboType.SelectedItem.Text = "INV" Then
            Label5.Visible = False
            Label6.Visible = False
            cboIQCType.Visible = False
            cboDpt.Visible = False
            trhid.Visible = False
            lbl1.Visible = False
            ddlDept.Style.Item("display") = "none"
            cmd_save.Visible = True
            cmd_clear.Visible = True
            Button1.Visible = True
        ElseIf cboType.SelectedItem.Text = "E2P" Then
            ViewState("type") = "IPP"
            Label5.Visible = False
            Label6.Visible = False
            cboIQCType.Visible = False
            cboDpt.Visible = False
            trhid.Visible = False
            lbl1.Visible = False
            ddlDept.Style.Item("display") = "none"
            cmd_save.Visible = True
            cmd_clear.Visible = True
            Button1.Visible = True
        ElseIf cboType.SelectedItem.Text = "IQC" Then
            Label5.Visible = True
            Label6.Visible = True
            cboIQCType.Visible = True
            cboDpt.Visible = True
            trhid.Visible = True
            lbl1.Visible = True
            ddlDept.Style.Item("display") = ""
            cmd_save.Visible = True
            cmd_clear.Visible = True
            Button1.Visible = True
            'Arif,04062013
            'For eMRS
        ElseIf cboType.SelectedItem.Text = "eMRS" Then
            Label5.Visible = False
            Label6.Visible = False
            cboIQCType.Visible = False
            cboDpt.Visible = False
            trhid.Visible = False
            lbl1.Visible = False
            ddlDept.Style.Item("display") = "none"
            cmd_save.Visible = False
            cmd_clear.Visible = False
            Button1.Visible = False
            'End-Arif,04062013
		'Modified for IPP Stage 2A - CH
        ElseIf cboType.SelectedItem.Text = "Billing" Then
            Label5.Visible = False
            Label6.Visible = False
            cboIQCType.Visible = False
            cboDpt.Visible = False
            trhid.Visible = True
            lbl1.Visible = False
            ddlDept.Style.Item("display") = "none"
            cmd_save.Visible = True
            cmd_clear.Visible = True
            Button1.Visible = True
		'-------------------------------
        Else
            Label5.Visible = False
            Label6.Visible = False
            cboIQCType.Visible = False
            cboDpt.Visible = False
            trhid.Visible = True
            lbl1.Visible = True
            ddlDept.Style.Item("display") = ""
            cmd_save.Visible = True
            cmd_clear.Visible = True
            Button1.Visible = True
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Hide_Add2.Style("display") = "none"
        hidMode.Value = ""
        lblmsg.Text = ""
        cboType.Enabled = True
    End Sub

    Private Sub cboType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboType.SelectedIndexChanged
        Hide_Add2.Style.Item("Display") = "none"
        'If dtgApp.Items.Count > 0 Then
        dtgApp.CurrentPageIndex = 0
        Checktype()
        Call bindgridApp(0)
        'Else
        '    Checktype()
        'End If
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Session("w_ApprWF_tabs") = "<div Class=""t_entity""><ul>" & _
        '"<li><div Class=""space""></div></li>" & _
        '             "<li><a Class=""t_entity_btn_selected"" href=""ApprovalWorkFlow.aspx?pageid=" & strPageId & """><span>Approval Group</span></a></li>" & _
        '                         "<li><div Class=""space""></div></li>" & _
        '             "<li><a Class=""t_entity_btn"" href=""AppGrpAsg.aspx?pageid=" & strPageId & """><span>Approval Officer Assignment</span></a></li>" & _
        '                         "<li><div Class=""space""></div></li>" & _
        '             "<li><a Class=""t_entity_btn"" href=""AppGrpAsgPur.aspx?pageid=" & strPageId & """><span>Purchaser Assignment</span></a></li>" & _
        '                         "<li><div Class=""space""></div></li>" & _
        '              "</ul><div></div></div>"
        Session("w_ApprWF_tabs") = "<div Class=""t_entity""><ul>" & _
        "<li><div Class=""space""></div></li>" & _
                     "<li><a Class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("ApprWorkFlow", "ApprovalWorkFlow.aspx", "pageid=" & strPageId) & """><span>Approval Group</span></a></li>" & _
                     "<li><div Class=""space""></div></li>" & _
                     "<li><a Class=""t_entity_btn"" href=""" & dDispatcher.direct("ApprWorkFlow", "AppGrpAsg.aspx", "pageid=" & strPageId) & """><span>Approval Officer Assignment</span></a></li>" & _
                     "<li><div Class=""space""></div></li>" & _
                     "<li><a Class=""t_entity_btn"" href=""" & dDispatcher.direct("ApprWorkFlow", "AppGrpAsgPur.aspx", "pageid=" & strPageId) & """><span>User Assignment</span></a></li>" & _
                     "<li><div Class=""space""></div></li>" & _
                     "<li><a Class=""t_entity_btn"" href=""" & dDispatcher.direct("ApprWorkFlow", "AppGrpAsgItem.aspx", "pageid=" & strPageId) & """><span>Item Assignment</span></a></li>" & _
                     "<li><div Class=""space""></div></li>" & _
                     "</ul><div></div></div>"
    End Sub

    Sub BindIQCTestType(ByVal dropdownlist As DropDownList, Optional ByVal blnSelect As Boolean = True)
        Dim cbolist As New ListItem
        Dim objAdmin As New Admin
        Dim dvCustom As DataView

        dvCustom = objAdmin.getIQCTestType()

        dropdownlist.Items.Clear()
        If Not dvCustom Is Nothing Then
            Common.FillDdl(dropdownlist, "CPA_PARAM_LABEL", "CPA_PARAM_LABEL", dvCustom)
        End If

        If blnSelect Then
            cbolist.Value = ""
            cbolist.Text = "---Select---"
            dropdownlist.Items.Insert(0, cbolist)
        Else
            If dvCustom Is Nothing Then
                cbolist.Value = ""
                cbolist.Text = "---Select---"
                dropdownlist.Items.Insert(0, cbolist)
            End If
        End If

    End Sub

    Private Sub cmd_save1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_save1.Click
        '//To prevent "No Record Found" msg
        'ViewState("oldvalue") = txt_add_mod.Text
        txtGrpSearch.Text = ""
        lblmsg.Text = ""

        If validate() Then
            If strvalcboconsol = "" Then
                If hidMode.Value = "a" Then
                    Me.view("add", "")
                ElseIf hidMode.Value = "m" Then
                    Me.view("Mod", "")
                End If
            Else
                strvalcboconsol = cboConsol.SelectedItem.Value

                If hidMode.Value = "a" Then
                    Me.view("add", strvalcboconsol)
                ElseIf hidMode.Value = "m" Then
                    Me.view("Mod", strvalcboconsol)
                End If

            End If

            cboType.Enabled = True
            bindgridApp(0)
        End If
    End Sub

    'Private Sub cmd_clear2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_clear2.Click
    '    If hidMode.Value = "a" Then
    '        txt_add_mod.Text = ""
    '        'cboType.SelectedIndex = 2
    '        ddlDept.SelectedIndex = 0
    '        cboIQCType2.SelectedIndex = 0
    '        'If Session("Env") <> "FTN" Then
    '        '    If cboConsol.SelectedIndex <> -1 Then
    '        '        cboConsol.SelectedIndex = 0
    '        '        chkConsol.Checked = False
    '        '    End If
    '        'End If
    '        If cboConsol.SelectedIndex <> -1 Then
    '            cboConsol.SelectedIndex = 0
    '            chkConsol.Checked = False
    '        End If
    '    Else
    '        txt_add_mod.Text = ViewState("oldvalue")
    '        Dim valuserid As String = ViewState("oldvalue2")
    '        Common.SelDdl(valuserid, cboConsol, True, False)
    '        cboType.SelectedValue = ViewState("oldvalue3")
    '        If ViewState("oldvalue4") = "&nbsp;" Then
    '            ddlDept.SelectedValue = ""
    '        Else
    '            ddlDept.SelectedValue = ViewState("oldvalue4")
    '        End If
    '        If ViewState("oldvalue5") = "&nbsp;" Then
    '            cboIQCType2.SelectedIndex = 0
    '        Else
    '            cboIQCType2.SelectedValue = ViewState("oldvalue5")
    '        End If

    '        'Arif,04062013
    '        'For eMRS
    '        If ViewState("oldvalue6") = "&nbsp;" Then
    '            txtUrgentIR.Text = "" And txtRejectMRS.Text = ""
    '        Else
    '            txtUrgentIR.Text = ViewState("oldvalue6") Or txtRejectMRS.Text = ViewState("oldvalue6")
    '        End If
    '        'End-Arif,04062013

    '        'If Session("Env") <> "FTN" Then
    '        '    chkConsol.Checked = ViewState("consol")
    '        '    chkConsol_CheckedChanged(sender, e)
    '        'End If
    '        chkConsol.Checked = ViewState("consol")
    '        chkConsol_CheckedChanged(sender, e)
    '    End If

    'End Sub

    Private Sub button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles button3.Click
        Hide_Add2.Style("display") = "none"
        hidMode.Value = ""
        lblmsg.Text = ""
        cboType.Enabled = True
    End Sub

    Private Sub cmd_clear2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_clear2.Click
        If hidMode.Value = "a" Then
            txtUrgentIR.Text = ""
            txtRejectMRS.Text = ""
            lblmsg.Text = ""
            txt_add_mod.Text = ""

        ElseIf hidMode.Value = "m" Then
            txt_add_mod.Text = ViewState("oldvalue")
            txtUrgentIR.Text = ViewState("oldvalue6")
            txtRejectMRS.Text = ViewState("oldvalue7")
            'Dim valuserid As String = ViewState("oldvalue2")
            'Common.SelDdl(valuserid, cboConsol, True, False)
            'cboType.SelectedValue = ViewState("oldvalue3")
            'If ViewState("oldvalue4") = "&nbsp;" Then
            '    ddlDept.SelectedValue = ""
            'Else
            '    ddlDept.SelectedValue = ViewState("oldvalue4")
            'End If
            'If ViewState("oldvalue5") = "&nbsp;" Then
            '    cboIQCType2.SelectedIndex = 0
            'Else
            '    cboIQCType2.SelectedValue = ViewState("oldvalue5")
            'End If

            ''If Session("Env") <> "FTN" Then
            ''    chkConsol.Checked = ViewState("consol")
            ''    chkConsol_CheckedChanged(sender, e)
            ''End If
            'chkConsol.Checked = ViewState("consol")
            'chkConsol_CheckedChanged(sender, e)
        End If
       
    End Sub

    Function validate() As Boolean
        Dim strEmail(), strEmail2() As String
        Dim i As Integer

        strEmail = Split(txtUrgentIR.Text, ";")
        For i = 0 To strEmail.Length - 1
            If strEmail(i) <> "" Then
                If Not System.Text.RegularExpressions.Regex.IsMatch(strEmail(i), "^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z]) *@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$") Then
                    lblmsg.Text = "<ul type='disc'><li>Invalid Email recipient (Urgent IR).<ul type='disc'></ul></li></ul>"
                    Return False
                End If
            End If
        Next

        strEmail2 = Split(txtRejectMRS.Text, ";")
        For i = 0 To strEmail2.Length - 1
            If strEmail(i) <> "" Then
                If Not System.Text.RegularExpressions.Regex.IsMatch(strEmail2(i), "^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$") Then
                    lblmsg.Text = "<ul type='disc'><li>Invalid Email recipient (Reject MRS).<ul type='disc'></ul></li></ul>"
                    Return False
                End If
            End If
        Next

        Return True
    End Function
End Class
