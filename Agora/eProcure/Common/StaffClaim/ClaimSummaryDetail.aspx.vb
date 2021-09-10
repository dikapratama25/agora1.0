Imports AgoraLegacy
Imports eProcure.Component


Public Class ClaimSummaryDetail
    Inherits AgoraLegacy.AppBaseClass
    Dim objSC As New eProcStaffClaim
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents hidresult As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblSCNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents lblUserName As System.Web.UI.WebControls.Label
    Protected WithEvents lblCompName As System.Web.UI.WebControls.Label
    Protected WithEvents lblDept As System.Web.UI.WebControls.Label
    Protected WithEvents lblDocDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblCurr As System.Web.UI.WebControls.Label
    Protected WithEvents ddlSelect As System.Web.UI.WebControls.DropDownList
    'Allowance WebControls
    Protected WithEvents lbl_N_Standby As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_Shift As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Y_Standby As System.Web.UI.WebControls.Label
    'Entertain WebControls
    Protected WithEvents lbl_N_Ent As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Y_Ent As System.Web.UI.WebControls.Label
    'Hardship WebControls
    Protected WithEvents lbl_N_HS As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Y_HS As System.Web.UI.WebControls.Label
    'Transportation WebControls
    Protected WithEvents lbl_N_Local_MC As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_Oversea_MC As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_Local_PK As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_Oversea_PK As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_Local_PT As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_Oversea_PT As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_Local_AF As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_Oversea_AF As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_Local_TL As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_Oversea_TL As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Y_Local_MC As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Y_Local_PK As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Y_Local_TL As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Y_Local_PT As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Y_Local_AF As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Y_Oversea_Amt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_SP As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_SP_REF As System.Web.UI.WebControls.Label 'mimi : 20/03/2017 - enhancement smart pay ref.
    'Outstation WebControls
    Protected WithEvents lbl_N_Local_SA As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_Oversea_SA As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_Local_AC As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_Oversea_AC As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_Local_AA As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_Oversea_AA As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Y_SA As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Y_AC As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Y_Local_AA As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Y_Oversea_AA As System.Web.UI.WebControls.Label
    'Other WebControls
    Protected WithEvents lbl_Misc_TC As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Misc_SA As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Misc_MC As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Misc_Dental As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Misc_Laundry As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_CHP As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_PHP As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_Stationery As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_HPS As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_Postage As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_DP As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Y_HP As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Y_Stationery As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Y_Others As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_Others As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_Gifts As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_N_APR As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_TAT As System.Web.UI.WebControls.Label
    'Overtime WebControls
    Protected WithEvents lbl_MA As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_OT_A As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_OT_B As System.Web.UI.WebControls.Label

    Protected WithEvents lbl_Total_Amt As System.Web.UI.WebControls.Label
    Protected WithEvents dtgAppFlow As System.Web.UI.WebControls.DataGrid
    Protected WithEvents tbApp As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents trInfo As System.Web.UI.HtmlControls.HtmlTableRow

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Dim clsAdmin As New Admin
    Dim decTotalAmt As Decimal = 0

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdSubmit.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdSubmit)
        htPageAccess.Add("add", alButtonList)
        htPageAccess.Add("update", alButtonList)
        CheckButtonAccess()
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnPaging = False
        blnSorting = False
        SetGridProperty(dtgAppFlow)

        If Not IsPostBack Then
            ViewState("index") = Request.QueryString("index")
            DisplayHeaders()
            'displayParam()
            GenerateTab()
            GenerateFormLink()

            If Session("urlreferer") = "SCTrankingList_All" Then
                lnkBack.NavigateUrl = dDispatcher.direct("StaffClaim", "StaffClaimTracking_All.aspx", "pageId=" & strPageId)
                lnkBack.Visible = True
            Else
                lnkBack.Visible = False
            End If
        End If
       
        'Dim strMsg As String = "Changing document prefixes may affect Buyer Company interface files.  Please click Cancel to abort Or click OK to proceed."
        'Me.cmd_save.Attributes.Add("onClick", "return confirmChanged('" & strMsg & "');")

    End Sub

    Private Sub BindDdl()

        ddlSelect.Items.Clear()
        ddlSelect.Items.Add(New ListItem("Transportation Claim Form", "Transportation"))
        ddlSelect.Items.Add(New ListItem("Standby / Shift Allowance Claim Form", "Allowance"))
        ddlSelect.Items.Add(New ListItem("Entertainment Claim Form", "Entertain"))
        ddlSelect.Items.Add(New ListItem("Hardship Claim Form", "Hardship"))
        ddlSelect.Items.Add(New ListItem("Overtime Claim Form", "Overtime"))
        ddlSelect.Items.Add(New ListItem("Outstation Claim Form", "Outstation"))
        ddlSelect.Items.Add(New ListItem("Other Claim Form", "Other"))
        ddlSelect.Items.Add(New ListItem("Claim Summary Sheet", "Summary"))
        ddlSelect.SelectedValue = "Summary"
    End Sub

    Private Sub DisplayHeaders()
        Dim ds As DataSet
        Dim objUser As New Users
        Dim objUserDetails As New User
        Dim objUserDtl As New User

        BindDdl()
        ds = objSC.GetSummaryDetails(ViewState("index"))
        ViewState("SCmode") = "e"

        If ds.Tables("Mstr").Rows.Count > 0 Then
            ViewState("SCNo") = ds.Tables("Mstr").Rows(0)("SCM_CLAIM_DOC_NO")
            lblSCNo.Text = ds.Tables("Mstr").Rows(0)("SCM_CLAIM_DOC_NO") 'SC No
            lblStatus.Text = ds.Tables("Mstr").Rows(0)("STATUS_DESC") 'Status
            objUserDetails = objUser.GetUserDetails(ds.Tables("Mstr").Rows(0)("SCM_STAFF_ID"), Session("CompanyId"))
            lblUserName.Text = objUserDetails.Name & " (" & objUserDetails.UserID & ")" 'User Name
            lblCompName.Text = ds.Tables("Mstr").Rows(0)("CM_COY_NAME") 'Company Name
            lblDept.Text = Common.parseNull(ds.Tables("Mstr").Rows(0)("CDM_DEPT_NAME")) 'Department Name
            lblDocDate.Text = Format(ds.Tables("Mstr").Rows(0)("SCM_CREATED_DATE"), "dd/MM/yyyy")
            If Session("urlreferer") = "SCTrankingList_All" Then
                cmdSubmit.Visible = False
                ViewState("SCmode") = "v"
                trInfo.Style("display") = "none"
            Else
                If ds.Tables("Mstr").Rows(0)("SCM_STATUS") = SCStatus.DraftSC Or ds.Tables("Mstr").Rows(0)("SCM_STATUS") = SCStatus.Rejected Then
                    cmdSubmit.Visible = True
                    ViewState("SCmode") = "e"
                Else
                    cmdSubmit.Visible = False
                    ViewState("SCmode") = "v"
                    trInfo.Style("display") = "none"
                End If
            End If
            objUserDtl = objUser.GetUserDetails(Session("UserId"), Session("CompanyId"))
            If Session("urlreferer") = "SCTrankingList_All" And objUser.IsRoleSelected(objUserDtl.UserGroup, FixedRole.Approving_Officer) Then
                DisplaySCApprFlow()
            Else
                If ds.Tables("Mstr").Rows(0)("SCM_STATUS") = SCStatus.Approved Or ds.Tables("Mstr").Rows(0)("SCM_STATUS") = SCStatus.PendingAppr Or ds.Tables("Mstr").Rows(0)("SCM_STATUS") = SCStatus.Rejected Then
                    DisplaySCApprFlow()
                Else
                    tbApp.Style("display") = "none"
                End If
            End If
            DisplayDetails()
        End If

    End Sub

    Private Sub DisplaySCApprFlow()
        Dim ds As New DataSet

        ds = objSC.getApprFlow(ViewState("index"))

        If ds.Tables(0).Rows.Count > 0 Then
            dtgAppFlow.DataSource = ds.Tables(0)
            dtgAppFlow.DataBind()
        End If

    End Sub

    Private Sub dtgAppFlow_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAppFlow.ItemCreated
        Grid_ItemCreated(sender, e)
    End Sub

    Private Sub dtgAppFlow_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAppFlow.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim intTotalCell, intLoop As Integer
            If dv("SCA_Seq") - 1 = dv("SCA_AO_Action") Then
                intTotalCell = e.Item.Cells.Count - 1
                For intLoop = 0 To intTotalCell
                    e.Item.Cells(intLoop).Font.Bold = True
                Next
            End If

            If dv("SCA_APPROVAL_TYPE") = 1 Then
                e.Item.Cells(3).Text = "Approval"
            Else
                e.Item.Cells(3).Text = "Endorsement"
            End If

            If IsDBNull(dv("AAO_NAME")) Then
                e.Item.Cells(2).Text = "-"
            End If

            If UCase(Common.parseNull(dv("SCA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("SCA_AO"))) Then
                e.Item.Cells(1).Font.Bold = True
            ElseIf UCase(Common.parseNull(dv("SCA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("SCA_A_AO"))) Then
                e.Item.Cells(2).Font.Bold = True
            End If

            If Not IsDBNull(dv("SCA_ACTION_DATE")) Then
                e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("SCA_ACTION_DATE"))
            End If
        End If
    End Sub

    Private Sub DisplayDetails()
        Dim dsAllw, dsEnt, dsHS, dsTrans, dsOut, dsOT As DataSet
        dsAllw = objSC.getClaimSummaryDt(ViewState("SCNo"), "Al")
        dsEnt = objSC.getClaimSummaryDt(ViewState("SCNo"), "Ent")
        dsHS = objSC.getClaimSummaryDt(ViewState("SCNo"), "Hs")
        dsTrans = objSC.getClaimSummaryDt(ViewState("SCNo"), "Trans")
        dsOut = objSC.getClaimSummaryDt(ViewState("SCNo"), "Out")
        dsOT = objSC.getClaimSummaryDt(ViewState("SCNo"), "Ot")

        If dsAllw.Tables(0).Rows.Count > 0 Then 'Display Allowance Info
            lbl_N_Standby.Text = TextFormat(dsAllw.Tables(0).Rows(0)("N_STANDBY"))
            lbl_N_Shift.Text = TextFormat(dsAllw.Tables(0).Rows(0)("N_SHIFT"))
            lbl_Y_Standby.Text = TextFormat(dsAllw.Tables(0).Rows(0)("Y_STANDBY") + dsAllw.Tables(0).Rows(0)("Y_SHIFT"))
        End If
        If dsEnt.Tables(0).Rows.Count > 0 Then 'Display Entertain Info
            lbl_N_Ent.Text = TextFormat(dsEnt.Tables(0).Rows(0)("N_ENT"))
            lbl_Y_Ent.Text = TextFormat(dsEnt.Tables(0).Rows(0)("Y_ENT"))
        End If
        If dsHS.Tables(0).Rows.Count > 0 Then 'Display Hardship Info
            lbl_N_HS.Text = TextFormat(dsHS.Tables(0).Rows(0)("N_HDSHIP"))
            lbl_Y_HS.Text = TextFormat(dsHS.Tables(0).Rows(0)("Y_HDSHIP"))
        End If
        If dsTrans.Tables(0).Rows.Count > 0 Then 'Display Transportation Info
            lbl_N_Local_MC.Text = TextFormat(dsTrans.Tables(0).Rows(0)("CAR_AMT") + dsTrans.Tables(0).Rows(0)("BK_AMT"))
            lbl_N_Local_PK.Text = TextFormat(dsTrans.Tables(0).Rows(0)("PK_AMT"))
            lbl_N_Local_PT.Text = TextFormat(dsTrans.Tables(0).Rows(0)("PT_AMT"))
            lbl_N_Local_AF.Text = TextFormat(dsTrans.Tables(0).Rows(0)("AF_AMT"))
            lbl_N_Local_TL.Text = TextFormat(dsTrans.Tables(0).Rows(0)("TL_AMT"))
            lbl_N_Oversea_MC.Text = TextFormat(dsTrans.Tables(1).Rows(0)("CAR_AMT") + dsTrans.Tables(1).Rows(0)("BK_AMT"))
            lbl_N_Oversea_PK.Text = TextFormat(dsTrans.Tables(1).Rows(0)("PK_AMT"))
            lbl_N_Oversea_PT.Text = TextFormat(dsTrans.Tables(1).Rows(0)("PT_AMT"))
            lbl_N_Oversea_AF.Text = TextFormat(dsTrans.Tables(1).Rows(0)("AF_AMT"))
            lbl_N_Oversea_TL.Text = TextFormat(dsTrans.Tables(1).Rows(0)("TL_AMT"))
            lbl_Y_Local_MC.Text = TextFormat(dsTrans.Tables(2).Rows(0)("CAR_AMT") + dsTrans.Tables(2).Rows(0)("BK_AMT"))
            lbl_Y_Local_PK.Text = TextFormat(dsTrans.Tables(2).Rows(0)("PK_AMT"))
            lbl_Y_Local_TL.Text = TextFormat(dsTrans.Tables(2).Rows(0)("TL_AMT"))
            lbl_Y_Local_PT.Text = TextFormat(dsTrans.Tables(2).Rows(0)("PT_AMT"))
            lbl_Y_Local_AF.Text = TextFormat(dsTrans.Tables(2).Rows(0)("AF_AMT"))
            lbl_Y_Oversea_Amt.Text = TextFormat(dsTrans.Tables(3).Rows(0)("TOTAL_AMT"))
            'mimi : 27/03/2017 - enhancement smart pay ref.
            If dsTrans.Tables(6).Rows(0)("SCM_SP").ToString <> "" _
                And dsTrans.Tables(6).Rows(0)("SCM_STATUS").ToString <> SCStatus.Rejected Then 'strSmartPay not empty mean stamp/save the document before, not draft.
                lbl_SP.Text += TextFormat(CDec(dsTrans.Tables(6).Rows(0)("SCM_SP").ToString))
            Else
                If dsTrans.Tables(6).Rows(0)("SCM_STATUS").ToString = SCStatus.DraftSC _
                    Or dsTrans.Tables(6).Rows(0)("SCM_STATUS").ToString = SCStatus.Rejected Then
                    If dsTrans.Tables(4).Rows(0)("SP") >= dsTrans.Tables(5).Rows(0)("SP_CL") Then
                        lbl_SP.Text = TextFormat(dsTrans.Tables(5).Rows(0)("SP_CL"))
                    Else
                        lbl_SP.Text = TextFormat(dsTrans.Tables(4).Rows(0)("SP"))
                    End If
                Else
                    lbl_SP.Text = TextFormat(dsTrans.Tables(4).Rows(0)("SP"))
                End If
            End If
            lbl_SP_REF.Text = dsTrans.Tables(4).Rows(0)("SP") 'mimi - 27/03/2017
            'end
        End If
        If dsOut.Tables(0).Rows.Count > 0 Then 'Display Outstation Info
            lbl_N_Local_SA.Text = TextFormat(dsOut.Tables(0).Rows(0)("N_LOCAL_SUB_ALLW"))
            lbl_N_Oversea_SA.Text = TextFormat(dsOut.Tables(0).Rows(0)("N_OVER_SUB_ALLW"))
            lbl_N_Local_AC.Text = TextFormat(dsOut.Tables(0).Rows(0)("N_LOCAL_ACC"))
            lbl_N_Oversea_AC.Text = TextFormat(dsOut.Tables(0).Rows(0)("N_OVER_ACC"))
            lbl_N_Local_AA.Text = TextFormat(dsOut.Tables(0).Rows(0)("N_LOCAL_ACC_ALLW"))
            lbl_N_Oversea_AA.Text = TextFormat(dsOut.Tables(0).Rows(0)("N_OVER_ACC_ALLW"))
            lbl_Y_SA.Text = TextFormat(dsOut.Tables(0).Rows(0)("Y_SUB_ALLW"))
            lbl_Y_AC.Text = TextFormat(dsOut.Tables(0).Rows(0)("Y_ACC"))
            lbl_Y_Local_AA.Text = TextFormat(dsOut.Tables(0).Rows(0)("Y_LOCAL_ACC_ALLW"))
            lbl_Y_Oversea_AA.Text = TextFormat(dsOut.Tables(0).Rows(0)("Y_OVER_ACC_ALLW"))
        End If
        If dsOT.Tables(0).Rows.Count > 0 Then 'Display Overtime Info
            lbl_MA.Text = TextFormat(dsOT.Tables(0).Rows(0)("MEAL_ALLOWANCE"))
            If Common.parseNull(dsOT.Tables(0).Rows(0)("TOTAL_HOUR_MIN_A")) <> "" Then
                lbl_OT_A.Text = dsOT.Tables(0).Rows(0)("TOTAL_HOUR_MIN_A")
            End If
            If Common.parseNull(dsOT.Tables(0).Rows(0)("TOTAL_HOUR_MIN_B")) <> "" Then
                lbl_OT_B.Text = dsOT.Tables(0).Rows(0)("TOTAL_HOUR_MIN_B")
            End If
        End If
        'Display Other Info
        lbl_Misc_TC.Text = TextFormat(objSC.getMiscClaimSummaryDt(ViewState("SCNo"), "Transportation Claim (ONG)"))
        lbl_Misc_SA.Text = TextFormat(objSC.getMiscClaimSummaryDt(ViewState("SCNo"), "Storage Allowance"))
        lbl_Misc_MC.Text = TextFormat(objSC.getMiscClaimSummaryDt(ViewState("SCNo"), "Medical Claims (MYR40)"))
        lbl_Misc_Dental.Text = TextFormat(objSC.getMiscClaimSummaryDt(ViewState("SCNo"), "Dental Claims (MYR50)"))
        lbl_Misc_Laundry.Text = TextFormat(objSC.getMiscClaimSummaryDt(ViewState("SCNo"), "Laundry"))
        lbl_N_CHP.Text = TextFormat(objSC.getMiscClaimSummaryDt(ViewState("SCNo"), "Company H/Phone Off. Calls", "N"))
        lbl_N_PHP.Text = TextFormat(objSC.getMiscClaimSummaryDt(ViewState("SCNo"), "Personal H/Phone Off. Calls", "N"))
        lbl_N_Stationery.Text = TextFormat(objSC.getMiscClaimSummaryDt(ViewState("SCNo"), "Stationery", "N"))
        lbl_N_HPS.Text = TextFormat(objSC.getMiscClaimSummaryDt(ViewState("SCNo"), "Handphone Subsidy", "N"))
        lbl_N_Postage.Text = TextFormat(objSC.getMiscClaimSummaryDt(ViewState("SCNo"), "Postage", "N"))
        lbl_N_DP.Text = TextFormat(objSC.getMiscClaimSummaryDt(ViewState("SCNo"), "Data Plan", "N"))
        lbl_Y_HP.Text = TextFormat(objSC.getMiscClaimSummaryDt(ViewState("SCNo"), "HP", "Y"))
        lbl_Y_Stationery.Text = TextFormat(objSC.getMiscClaimSummaryDt(ViewState("SCNo"), "Stationery", "Y"))
        lbl_Y_Others.Text = TextFormat(objSC.getMiscClaimSummaryDt(ViewState("SCNo"), "Others", "Y"))
        lbl_N_Others.Text = TextFormat(objSC.getMiscClaimSummaryDt(ViewState("SCNo"), "Others", "N"))
        lbl_N_Gifts.Text = TextFormat(objSC.getMiscClaimSummaryDt(ViewState("SCNo"), "Gifts", "N"))
        lbl_N_APR.Text = TextFormat(objSC.getMiscClaimSummaryDt(ViewState("SCNo"), "Attendence Performance Reward", "N"))
        lbl_TAT.Text = TextFormat(objSC.getMiscClaimSummaryDt(ViewState("SCNo"), "Travelling Advance Taken", "N"), True)
        'Total Amount
        lbl_Total_Amt.Text = Format(decTotalAmt, "##,###,##0.00")
    End Sub

    Private Function TextFormat(ByVal decField As Decimal, Optional ByVal blnMinus As Boolean = False) As String
        If decField = 0 Then
            TextFormat = ""
        Else
            If blnMinus = True Then
                decTotalAmt -= decField
            Else
                decTotalAmt += decField
            End If
            TextFormat = Format(decField, "##,###,##0.00")
        End If
    End Function

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim ds As New DataSet
        Dim intMsg As Integer
        Dim strMsg As String

        'mimi : 29/03/2017
        If lbl_SP.Text = "" Then
            lbl_SP.Text = 0.0
        End If

        If ChkPolicyDate(strMsg) = True Then
            ds = objSC.getSCApprFlow()
            If ds.Tables(0).Rows.Count = 0 Then
                Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                Exit Sub
            ElseIf ds.Tables(0).Rows.Count > 1 Then
                Response.Redirect(dDispatcher.direct("StaffClaim", "SCApprovalSetup.aspx", "pageid=" & strPageId & "&index=" & ViewState("index") & "&scno=" & lblSCNo.Text & "&sm=" & lbl_SP.Text))
                Exit Sub
            Else
                intMsg = objSC.submitSC(ViewState("index"), ds.Tables(0).Rows(0)("AGM_GRP_INDEX"), lbl_SP.Text)

                Select Case intMsg
                    Case WheelMsgNum.Save
                        cmdSubmit.Visible = False
                        Common.NetMsgbox(Me, "Staff Claim Number " & lblSCNo.Text & " " & objGlobal.GetErrorMessage("00024"), dDispatcher.direct("StaffClaim", "TransportationClaim.aspx", "pageid=" & strPageId))

                    Case WheelMsgNum.NotSave
                        Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00007"), MsgBoxStyle.Information)
                End Select
            End If
        Else
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
        End If

    End Sub

    Private Function ChkPolicyDate(ByRef strMsg As String) As Boolean
        ChkPolicyDate = True

        If objSC.chkPolicyDt(ViewState("SCNo"), "Al") = False Then
            strMsg = "[Standby / Shift Allowance Claim Form] Date has exceeded 110-days policy."
            ChkPolicyDate = False
        ElseIf objSC.chkPolicyDt(ViewState("SCNo"), "Ent") = False Then
            strMsg = "[Entertainment Claim Form] Date has exceeded 110-days policy."
            ChkPolicyDate = False
        ElseIf objSC.chkPolicyDt(ViewState("SCNo"), "Hs") = False Then
            strMsg = "[Hardship Claim Form] Date has exceeded 110-days policy."
            ChkPolicyDate = False
        ElseIf objSC.chkPolicyDt(ViewState("SCNo"), "Misc") = False Then
            strMsg = "[Other Claim Form] Date has exceeded 110-days policy."
            ChkPolicyDate = False
        ElseIf objSC.chkPolicyDt(ViewState("SCNo"), "Out") = False Then
            strMsg = "[Outstation Claim Form] Date has exceeded 110-days policy."
            ChkPolicyDate = False
        ElseIf objSC.chkPolicyDt(ViewState("SCNo"), "Ot") = False Then
            strMsg = "[Overtime Claim Form] Date has exceeded 110-days policy."
            ChkPolicyDate = False
        ElseIf objSC.chkPolicyDt(ViewState("SCNo"), "Tp") = False Then
            strMsg = "[Transportation Claim Form] Date has exceeded 110-days policy."
            ChkPolicyDate = False
        End If
    End Function

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        If Session("urlreferer") = "SCTrankingList_All" Then
            Session("w_Staff_Claim_tabs") = Nothing
        ElseIf ViewState("SCmode") = "v" Then
            Session("w_Staff_Claim_tabs") = "<div class=""t_entity""><ul>" & _
                                       "<li><div class=""space""></div></li>" & _
                                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("StaffClaim", "TransportationClaim.aspx", "pageid=" & strPageId) & """><span>Staff Claim Documents</span></a></li>" & _
                                       "<li><div class=""space""></div></li>" & _
                                       "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("StaffClaim", "StaffClaimTrackingList.aspx", "pageid=" & strPageId) & """><span>Staff Claim Listing</span></a></li>" & _
                                       "<li><div class=""space""></div></li>" & _
                                       "</ul><div></div></div>"
        Else
            Session("w_Staff_Claim_tabs") = "<div class=""t_entity""><ul>" & _
                                       "<li><div class=""space""></div></li>" & _
                                       "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("StaffClaim", "TransportationClaim.aspx", "pageid=" & strPageId) & """><span>Staff Claim Documents</span></a></li>" & _
                                       "<li><div class=""space""></div></li>" & _
                                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("StaffClaim", "StaffClaimTrackingList.aspx", "pageid=" & strPageId) & """><span>Staff Claim Listing</span></a></li>" & _
                                       "<li><div class=""space""></div></li>" & _
                                       "</ul><div></div></div>"
        End If
        

    End Sub

    Private Sub ddlSelect_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSelect.SelectedIndexChanged

        If ddlSelect.SelectedItem.Value = "Transportation" Then
            Me.Response.Redirect(dDispatcher.direct("StaffClaim", "TransportationClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
        ElseIf ddlSelect.SelectedItem.Value = "Allowance" Then
            Me.Response.Redirect(dDispatcher.direct("StaffClaim", "AllowanceClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
        ElseIf ddlSelect.SelectedItem.Value = "Entertain" Then
            Me.Response.Redirect(dDispatcher.direct("StaffClaim", "EntertainmentClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
        ElseIf ddlSelect.SelectedItem.Value = "Hardship" Then
            Me.Response.Redirect(dDispatcher.direct("StaffClaim", "HardShipClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
        ElseIf ddlSelect.SelectedItem.Value = "Overtime" Then
            Me.Response.Redirect(dDispatcher.direct("StaffClaim", "OverTimeClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
        ElseIf ddlSelect.SelectedItem.Value = "Summary" Then
            Me.Response.Redirect(dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
        ElseIf ddlSelect.SelectedItem.Value = "Other" Then
            Me.Response.Redirect(dDispatcher.direct("StaffClaim", "OtherClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
        ElseIf ddlSelect.SelectedItem.Value = "Outstation" Then
            Me.Response.Redirect(dDispatcher.direct("StaffClaim", "OutstationClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
        End If
    End Sub

    Private Sub GenerateFormLink()
        Dim strTableLink As String
        Dim strIcon As String = "<IMG style=""height:19px; vertical-align:middle; position: relative;"" src=" & dDispatcher.direct("Plugins/Images", "StaffClaimDoc.gif") & ">"

        strTableLink = "<tr><td colspan=""5"">" & _
        "<table cellspacing=""0"" cellpadding=""0"" class=""alltable"">" & _
        "<tr>"

        strTableLink &= "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><font color=""green""><strong>Claim Summary Sheet</strong></font></a></span></td>"

        strTableLink &= "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "TransportationClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Transportation Claim Form</strong></a></span></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "AllowanceClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Standby/Shift Allw. Claim Form</strong></a></span></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "EntertainmentClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Entertainment Claim Form</strong></a></span></td>" & _
        "</tr>" & _
        "<tr>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "HardShipClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Hardship Claim Form</strong></a></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "OverTimeClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Overtime Claim Form</strong></a></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "OutstationClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Outstation Claim Form</strong></a></span></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "OtherClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Other Claim Form</strong></a></span></td>" & _
        "</tr>" & _
        "</table>" & _
        "</td></tr>"
        Session("w_SC_Links") = strTableLink

    End Sub
End Class
