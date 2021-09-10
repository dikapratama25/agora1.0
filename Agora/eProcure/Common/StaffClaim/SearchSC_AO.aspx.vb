'//Outstanding
'//RFQ Ind
'//Attachment Ind

Imports AgoraLegacy
Imports eProcure.Component
Public Class SearchSC_AO
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Protected WithEvents txtSCNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStaffID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStaffName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateFr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents vldDateFtDateTo As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents vldDateFr As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents vldDateTo As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents dtgSCList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdMassApp As System.Web.UI.WebControls.Button
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents cboAO As System.Web.UI.WebControls.DropDownList
    Protected WithEvents tdAO As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblVendor As System.Web.UI.WebControls.Label
    Protected WithEvents cboDeptName As System.Web.UI.WebControls.DropDownList

    Dim strCaller As String
    Dim strAO As String
    Dim blnRelief As Boolean
    Dim objSC As New eProcStaffClaim

    Public Enum EnumSC
        icCheckBox
        icStaffName
        icDeptName
        icSCNoLink
        icCreatedDate
        icSubmitDate
        icEnt
        icTrans
        icTotal
        icStatus
        icSCNo
        icStaffId
        icIndex
    End Enum
    
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        strCaller = UCase(Request.QueryString("caller"))
        SetGridProperty(dtgSCList)
        MyBase.Page_Load(sender, e)

        '//because No DataGrid display when page first loaded
        cmdReset.Disabled = True
        cmdMassApp.Enabled = True


        If cboAO.SelectedIndex <= 0 Then
            strAO = Session("UserID")
            blnRelief = False
        Else
            strAO = cboAO.SelectedValue
            blnRelief = True
        End If

        If Not Page.IsPostBack Then
            GenerateTab()
            Session("strurl") = Me.strCallFrom ' (gary add) url back from quote compare
            getReliefList()
            Common.FillDefault(cboDeptName, "COMPANY_DEPT_MSTR", "CDM_DEPT_NAME", "CDM_DEPT_CODE", "---Select---", " CDM_DELETED<>'Y' AND CDM_COY_ID='" & Session("CompanyId") & "'")
            If cboAO.SelectedIndex < 0 Then
                cmdSearch_Click(sender, e)
            End If
            cmdSearch_Click(sender, e)
        End If

        Session("urlreferer") = "SearchSCAO"

        'Check Access Status
        If Session("AccessStatus") = "Limited" Then
            cmdMassApp.Enabled = False
        End If

    End Sub

    Private Sub getReliefList()
        Dim dv As DataView
        Dim objPR As New PurchaseReq2
        dv = objPR.getReliefList("SC")
        If Not dv Is Nothing Then
            tdAO.Style("VISIBILITY") = "visible"
            tdAO.Style("display") = ""
            Common.FillDdl(cboAO, "NAME", "RAM_USER_ID", dv)
            Dim lstItem As New ListItem
            lstItem.Value = ""
            lstItem.Text = "---Select---"
            cboAO.Items.Insert(0, lstItem)
        Else
            tdAO.Style("display") = "none"
            tdAO.Style("VISIBILITY") = "hidden"
        End If
        objPR = Nothing
    End Sub
    Public Sub dtgSCList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgSCList.PageIndexChanged
        dtgSCList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgSCList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub Bindgrid(Optional ByVal pSorted As Boolean = False)

        Dim ds As DataSet
        Dim dsTemp As DataSet
        Dim strReliefOn As String
        strReliefOn = cboAO.SelectedValue
        strReliefOn = ""
        dsTemp = objSC.getSCListForApproval(txtSCNo.Text, txtDateFr.Text, txtDateTo.Text, strReliefOn, , SCStatus.Submitted & "," & SCStatus.PendingAppr, "", , txtStaffID.Text, txtStaffName.Text, cboDeptName.SelectedValue)
        ds = GetAllClaimAmt(dsTemp)

        '//for sorting asc or desc
        Dim dvViewSC As DataView
        dvViewSC = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewSC.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSC.Sort += " DESC"
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgSCList, dvViewSC)
            cmdReset.Disabled = False
            Dim objApp As New ApprWorkFlow
            'If objApp.checkMassApp(Session("UserID")) = 1 Then
            '    cmdMassApp.Enabled = True
            'Else
            '    cmdMassApp.Enabled = False
            '    cmdMassApp.Visible = False
            'End If
            cmdMassApp.Enabled = True
            cmdMassApp.Visible = True
            dtgSCList.DataSource = dvViewSC
            dtgSCList.DataBind()
        Else
            cmdReset.Disabled = True
            cmdMassApp.Enabled = False
            dtgSCList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ViewState("PageCount") = dtgSCList.PageCount
    End Sub

    Private Function GetAllClaimAmt(ByVal dsTemp As DataSet) As DataSet
        Dim i As Integer
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim dtr As DataRow
        dt.Columns.Add("SCM_CLAIM_INDEX", Type.GetType("System.Int32"))
        dt.Columns.Add("SCM_CLAIM_DOC_NO", Type.GetType("System.String"))
        dt.Columns.Add("CDM_DEPT_NAME", Type.GetType("System.String"))
        dt.Columns.Add("SCM_CREATED_DATE", Type.GetType("System.DateTime"))
        dt.Columns.Add("SCM_SUBMIT_DATE", Type.GetType("System.DateTime"))
        dt.Columns.Add("ENT_AMT", Type.GetType("System.Decimal"))
        dt.Columns.Add("L_MILEAGE_AMT", Type.GetType("System.Decimal"))
        dt.Columns.Add("CLAIM_AMT", Type.GetType("System.Decimal"))
        dt.Columns.Add("STATUS_DESC", Type.GetType("System.String"))
        dt.Columns.Add("SCM_STATUS", Type.GetType("System.String"))
        dt.Columns.Add("STAFF_NAME", Type.GetType("System.String"))
        dt.Columns.Add("SCM_STAFF_ID", Type.GetType("System.String"))

        For i = 0 To dsTemp.Tables(0).Rows.Count - 1
            dtr = dt.NewRow
            dtr("SCM_CLAIM_INDEX") = dsTemp.Tables(0).Rows(i)("SCM_CLAIM_INDEX")
            dtr("SCM_CLAIM_DOC_NO") = dsTemp.Tables(0).Rows(i)("SCM_CLAIM_DOC_NO")
            dtr("SCM_CREATED_DATE") = dsTemp.Tables(0).Rows(i)("SCM_CREATED_DATE")
            dtr("SCM_SUBMIT_DATE") = dsTemp.Tables(0).Rows(i)("SCM_SUBMIT_DATE")
            Dim dsTrans As DataSet = objSC.getClaimSummaryDt(dsTemp.Tables(0).Rows(i)("SCM_CLAIM_DOC_NO"), "Trans")
            Dim dsEnt As DataSet = objSC.getClaimSummaryDt(dsTemp.Tables(0).Rows(i)("SCM_CLAIM_DOC_NO"), "Ent")
            dtr("ENT_AMT") = dsEnt.Tables(0).Rows(0)("N_ENT")
            dtr("L_MILEAGE_AMT") = dsTrans.Tables(0).Rows(0)("CAR_AMT") + dsTrans.Tables(0).Rows(0)("BK_AMT")
            dtr("CLAIM_AMT") = objSC.getTotalClaimAmt(dsTemp.Tables(0).Rows(i)("SCM_CLAIM_DOC_NO"))
            dtr("STATUS_DESC") = dsTemp.Tables(0).Rows(i)("STATUS_DESC")
            dtr("SCM_STATUS") = dsTemp.Tables(0).Rows(i)("SCM_STATUS")
            dtr("STAFF_NAME") = dsTemp.Tables(0).Rows(i)("STAFF_NAME")
            dtr("SCM_STAFF_ID") = dsTemp.Tables(0).Rows(i)("SCM_STAFF_ID")
            dtr("CDM_DEPT_NAME") = dsTemp.Tables(0).Rows(i)("CDM_DEPT_NAME")
            dt.Rows.Add(dtr)
        Next

        ds.Tables.Add(dt)
        Return ds
    End Function

    Private Sub dtgSCList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgSCList.ItemCreated
        '//this line must be included
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgSCList, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgSCList.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "SCM_SUBMIT_DATE"
        Bindgrid(True)
    End Sub

    Private Sub dtgSCList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgSCList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim chk As CheckBox
            chk = e.Item.Cells(EnumSC.icCheckBox).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            '//to dynamic build hyperlink
            Dim lnkScNo As HyperLink
            lnkScNo = e.Item.Cells(EnumSC.icSCNoLink).FindControl("lnkScNo")
            lnkScNo.NavigateUrl = dDispatcher.direct("StaffClaim", "SCApprDetail.aspx", "caller=approval&AO=" & strAO & "&relief=" & blnRelief & "&PageID=" & strPageId & "&index=" & dv("SCM_CLAIM_INDEX") & "&SCNo=" & dv("SCM_CLAIM_DOC_NO"))
            lnkScNo.Text = dv("SCM_CLAIM_DOC_NO")
            'e.Item.Cells(EnumSC.icCreatedDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("SCM_CREATED_DATE"))
            e.Item.Cells(EnumSC.icCreatedDate).Text = Format(CDate(dv("SCM_CREATED_DATE")), "dd/MM/yyyy")
            'e.Item.Cells(EnumSC.icSubmitDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("SCM_SUBMIT_DATE"))
            e.Item.Cells(EnumSC.icSubmitDate).Text = Format(CDate(dv("SCM_SUBMIT_DATE")), "dd/MM/yyyy")
            e.Item.Cells(EnumSC.icEnt).Text = Format(dv("ENT_AMT"), "#,##0.00")
            e.Item.Cells(EnumSC.icTrans).Text = Format(dv("L_MILEAGE_AMT"), "#,##0.00")
            e.Item.Cells(EnumSC.icTotal).Text = Format(dv("CLAIM_AMT"), "##,##0.00")
        End If
    End Sub
    Private Sub cboAO_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboAO.SelectedIndexChanged
        cmdSearch_Click(sender, e)
    End Sub

    Private Sub cmdMassApp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMassApp.Click
        Dim strArySC(0), strMsg(0) As String
        Dim strArySCIndex(0), strAO As String
        Dim objPO As New PurchaseOrder_AO 'PurchaseOrder
        Dim dgItem As DataGridItem
        Dim blnRelief As Boolean
        Dim strReliefOn As String
        For Each dgItem In dtgSCList.Items
            Dim chkSel As CheckBox
            chkSel = dgItem.Cells(EnumSC.icCheckBox).FindControl("chkSelection")
            If chkSel.Checked Then
                Common.Insert2Ary(strArySC, dgItem.Cells(EnumSC.icSCNo).Text)
                Common.Insert2Ary(strArySCIndex, dgItem.Cells(EnumSC.icIndex).Text)
                'Common.Insert2Ary(strArySCIndex, dtgSCList.DataKeys(dgItem.ItemIndex))
            End If
        Next
        If cboAO.SelectedIndex <= 0 Then
            strAO = Session("UserId")
            blnRelief = False
            strReliefOn = ""
        Else
            strAO = cboAO.SelectedValue
            blnRelief = True
            strReliefOn = ""
        End If

        objSC.MassApprovalSC(strArySC, strArySCIndex, strAO, strMsg, blnRelief, strReliefOn)
        If strMsg.Length > 0 Then
            Dim intLoop, intCnt As Integer
            Dim strMsg1 As String
            intCnt = strMsg.Length
            For intLoop = 0 To intCnt - 1
                If intLoop = 0 Then
                    strMsg1 = strMsg(intLoop)
                Else
                    strMsg1 = strMsg1 & """& vbCrLf & """ & strMsg(intLoop)
                End If
            Next
            Common.NetMsgbox(Me, strMsg1)
        End If
        Bindgrid()
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_SearchSC_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("StaffClaim", "SearchSC_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("StaffClaim", "SearchSC_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"

    End Sub
End Class


