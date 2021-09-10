'//Attachment Ind
Imports AgoraLegacy
Imports eProcure.Component
Public Class StaffClaimTracking_All
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objSC As New eProcStaffClaim
    Dim strCaller As String
    
    Public Enum EnumSC
        icSCNo
        icDocDate
        icSubmitDate
        icStaffId
        icStaff
        icDept
        icAmt
        icStatus
        icSCNo1
        icSCIndex
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
    'Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
    '    CheckButtonAccess(True)
    'End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Put user code to initialize the page here
        blnCheckBox = False
        SetGridProperty(dtgScList)
        MyBase.Page_Load(sender, e)

        strCaller = UCase(Request.QueryString("caller"))
        strPageId = Request.QueryString("pageid")
        If Not Page.IsPostBack Then
            cboDeptName.Items.Clear()
            Common.FillDefault(cboDeptName, "COMPANY_DEPT_MSTR", "CDM_DEPT_NAME", "CDM_DEPT_CODE", "---Select---", " CDM_DELETED<>'Y' AND CDM_COY_ID='" & Session("CompanyId") & "'")
            'Session("strURL") = strCallFrom
            Session("urlreferer") = "SCTrankingList_All"
        End If

    End Sub

    Public Sub dtgScList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgScList.PageIndexChanged
        dtgScList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgScList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub Bindgrid(Optional ByVal pSorted As Boolean = False)

        Dim dsTemp As DataSet
        Dim ds As DataSet
        Dim strStatus As String = ""

        chk_condition(strStatus)
        dsTemp = objSC.getStaffClaimTracking_All(txtSCNo.Text, strStatus, txtDateFr.Text, txtDateTo.Text, txtStaffId.Text, txtStaffName.Text, cboDeptName.SelectedValue)
        ds = GetAllClaimAmt(dsTemp)
        '//for sorting asc or desc
        Dim dvViewSc As DataView
        dvViewSc = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewSc.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSc.Sort += " DESC"
        End If
        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgScList, dvViewSc)
            dtgScList.DataSource = dvViewSc
            dtgScList.DataBind()
        Else
            dtgScList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtgScList.PageCount
    End Sub

    Private Function GetAllClaimAmt(ByVal dsTemp As DataSet) As DataSet
        Dim i As Integer
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim dtr As DataRow
        dt.Columns.Add("SCM_CLAIM_INDEX", Type.GetType("System.Int32"))
        dt.Columns.Add("SCM_CLAIM_DOC_NO", Type.GetType("System.String"))
        dt.Columns.Add("SCM_CREATED_DATE", Type.GetType("System.DateTime"))
        dt.Columns.Add("SCM_SUBMIT_DATE", Type.GetType("System.DateTime"))
        dt.Columns.Add("ENT_AMT", Type.GetType("System.Decimal"))
        dt.Columns.Add("L_MILEAGE_AMT", Type.GetType("System.Decimal"))
        dt.Columns.Add("CLAIM_AMT", Type.GetType("System.Decimal"))
        dt.Columns.Add("STATUS_DESC", Type.GetType("System.String"))
        dt.Columns.Add("SCM_STATUS", Type.GetType("System.String"))
        dt.Columns.Add("STAFF_NAME", Type.GetType("System.String"))
        dt.Columns.Add("SCM_STAFF_ID", Type.GetType("System.String"))
        dt.Columns.Add("CDM_DEPT_NAME", Type.GetType("System.String"))

        For i = 0 To dsTemp.Tables(0).Rows.Count - 1
            dtr = dt.NewRow
            dtr("SCM_CLAIM_INDEX") = dsTemp.Tables(0).Rows(i)("SCM_CLAIM_INDEX")
            dtr("SCM_CLAIM_DOC_NO") = dsTemp.Tables(0).Rows(i)("SCM_CLAIM_DOC_NO")
            dtr("SCM_CREATED_DATE") = dsTemp.Tables(0).Rows(i)("SCM_CREATED_DATE")
            dtr("SCM_SUBMIT_DATE") = dsTemp.Tables(0).Rows(i)("SCM_SUBMIT_DATE")
            dtr("CLAIM_AMT") = objSC.getTotalClaimAmt(dsTemp.Tables(0).Rows(i)("SCM_CLAIM_DOC_NO"))
            dtr("STATUS_DESC") = dsTemp.Tables(0).Rows(i)("STATUS_DESC")
            dtr("SCM_STATUS") = dsTemp.Tables(0).Rows(i)("SCM_STATUS")
            dtr("SCM_STAFF_ID") = dsTemp.Tables(0).Rows(i)("SCM_STAFF_ID")
            dtr("STAFF_NAME") = dsTemp.Tables(0).Rows(i)("STAFF_NAME")
            dtr("CDM_DEPT_NAME") = dsTemp.Tables(0).Rows(i)("CDM_DEPT_NAME")
            dt.Rows.Add(dtr)
        Next

        ds.Tables.Add(dt)
        Return ds
    End Function

    Private Sub dtgScList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgScList.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgScList, e)
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgScList.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "SCM_CREATED_DATE"
        Bindgrid(True)
    End Sub

    Private Sub dtgScList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgScList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkSCNo As HyperLink

            lnkSCNo = e.Item.Cells(EnumSC.icSCNo).FindControl("lnkSCNo")
            If dv("SCM_STATUS") = SCStatus.DraftSC Then
                lnkSCNo.Text = dv("SCM_CLAIM_DOC_NO")
            Else
                lnkSCNo.Text = dv("SCM_CLAIM_DOC_NO")
                lnkSCNo.NavigateUrl = dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & dv("SCM_CLAIM_INDEX") & "&pageid=" & strPageId)
            End If

            e.Item.Cells(EnumSC.icDocDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("SCM_CREATED_DATE"))
            e.Item.Cells(EnumSC.icSubmitDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("SCM_SUBMIT_DATE"))
            e.Item.Cells(EnumSC.icAmt).Text = Format(dv("CLAIM_AMT"), "##,##0.00")

        End If
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Sub chk_condition(ByRef strStatus As String)
        If chkDraft.Checked = True Then
            strStatus = IIf(strStatus = "", SCStatus.DraftSC, strStatus & "," & SCStatus.DraftSC)
        End If
        If chkSubmit.Checked Then
            strStatus = IIf(strStatus = "", SCStatus.Submitted, strStatus & "," & SCStatus.Submitted)
        End If
        If chkPending.Checked Then
            strStatus = IIf(strStatus = "", SCStatus.PendingAppr, strStatus & "," & SCStatus.PendingAppr)
        End If
        If chkApproved.Checked Then
            strStatus = IIf(strStatus = "", SCStatus.Approved, strStatus & "," & SCStatus.Approved)
        End If
        If chkRejected.Checked Then
            strStatus = IIf(strStatus = "", SCStatus.Rejected, strStatus & "," & SCStatus.Rejected)
        End If
        If strStatus = "" Then
            strStatus = SCStatus.DraftSC & "," & _
             SCStatus.Submitted & "," & _
             SCStatus.PendingAppr & "," & _
             SCStatus.Approved & "," & _
             SCStatus.Rejected
        End If
    End Sub
End Class


