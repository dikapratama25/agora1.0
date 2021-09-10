'Copyright © 2013 STRATEQ GLOBAL SERVICES. All rights reserved.
'//Attachment Ind
Imports AgoraLegacy
Imports eProcure.Component
Public Class StaffClaimTrackingList
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals
    Dim objSc As New eProcStaffClaim

    Public Enum EnumSC
        icScNo
        icCreationDate
        icSubmitDate
        icAmount
        icStatus
    End Enum
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents txtScNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents dtgScList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents chkPending As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkDraft As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkSubmit As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkApproved As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkRejected As System.Web.UI.WebControls.CheckBox

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        SetGridProperty(dtgScList)

        MyBase.Page_Load(sender, e)

        If Not Page.IsPostBack Then
            GenerateTab()
            SortDateDesc()
            Session("urlreferer") = "SCTrankingList"
        End If
    End Sub

    Public Sub dtgScList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgScList.PageIndexChanged
        dtgScList.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgScList.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub Bindgrid()
        Dim dstemp As New DataSet
        Dim ds As New DataSet
        Dim strStatus As String = ""

        If chkDraft.Checked Then
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

        dstemp = objSc.getStaffClaimTracking(txtScNo.Text, strStatus)
        ds = GetAllClaimAmt(dstemp)

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If ViewState("action") = "del" Then
            If dtgScList.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgScList.PageSize = 0 Then
                dtgScList.CurrentPageIndex = dtgScList.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgScList, dvViewSample)
            dtgScList.DataSource = dvViewSample
            dtgScList.DataBind()
        Else
            dtgScList.DataBind()
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00006"))
        End If

        ' add for above checking
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
        dt.Columns.Add("CLAIM_AMT", Type.GetType("System.Decimal"))
        dt.Columns.Add("STATUS_DESC", Type.GetType("System.String"))
        dt.Columns.Add("SCM_STATUS", Type.GetType("System.String"))

        For i = 0 To dsTemp.Tables(0).Rows.Count - 1
            dtr = dt.NewRow
            dtr("SCM_CLAIM_INDEX") = dsTemp.Tables(0).Rows(i)("SCM_CLAIM_INDEX")
            dtr("SCM_CLAIM_DOC_NO") = dsTemp.Tables(0).Rows(i)("SCM_CLAIM_DOC_NO")
            dtr("SCM_CREATED_DATE") = dsTemp.Tables(0).Rows(i)("SCM_CREATED_DATE")
            dtr("SCM_SUBMIT_DATE") = dsTemp.Tables(0).Rows(i)("SCM_SUBMIT_DATE")
            dtr("CLAIM_AMT") = objSc.getTotalClaimAmt(dsTemp.Tables(0).Rows(i)("SCM_CLAIM_DOC_NO"))
            dtr("STATUS_DESC") = dsTemp.Tables(0).Rows(i)("STATUS_DESC")
            dtr("SCM_STATUS") = dsTemp.Tables(0).Rows(i)("SCM_STATUS")
            dt.Rows.Add(dtr)
        Next

        ds.Tables.Add(dt)
        Return ds
    End Function

    Private Sub dtgScList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgScList.ItemCreated
        Grid_ItemCreated(dtgScList, e)
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgScList.CurrentPageIndex = 0
        SortDateDesc()
    End Sub

    '//Sort the record by Date Desc when user click button/Dropdownlist other than Datagrid header and paging
    Sub SortDateDesc()
        ViewState("SortExpression") = "SCM_CREATED_DATE"
        ViewState("SortAscending") = "no"
        Bindgrid()
    End Sub

    Private Sub dtgScList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgScList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkScNo As HyperLink
            Dim strForm As String
            lnkScNo = e.Item.FindControl("lnkScNo")
            lnkScNo.Text = dv("SCM_CLAIM_DOC_NO")
            If SCStatus.DraftSC = dv("SCM_STATUS") Or SCStatus.Rejected = dv("SCM_STATUS") Then
                strForm = objSc.getFirstClaimForm(dv("SCM_CLAIM_DOC_NO"))
                If strForm = "Allowance" Then
                    lnkScNo.NavigateUrl = dDispatcher.direct("StaffClaim", "AllowanceClaim.aspx", "index=" & dv("SCM_CLAIM_INDEX") & "&pageid=" & strPageId)
                ElseIf strForm = "Hardship" Then
                    lnkScNo.NavigateUrl = dDispatcher.direct("StaffClaim", "HardShipClaim.aspx", "index=" & dv("SCM_CLAIM_INDEX") & "&pageid=" & strPageId)
                ElseIf strForm = "Overtime" Then
                    lnkScNo.NavigateUrl = dDispatcher.direct("StaffClaim", "OverTimeClaim.aspx", "index=" & dv("SCM_CLAIM_INDEX") & "&pageid=" & strPageId)
                ElseIf strForm = "Ent" Then
                    lnkScNo.NavigateUrl = dDispatcher.direct("StaffClaim", "EntertainmentClaim.aspx", "index=" & dv("SCM_CLAIM_INDEX") & "&pageid=" & strPageId)
                ElseIf strForm = "Transportation" Then
                    lnkScNo.NavigateUrl = dDispatcher.direct("StaffClaim", "TransportationClaim.aspx", "index=" & dv("SCM_CLAIM_INDEX") & "&pageid=" & strPageId)
                ElseIf strForm = "Other" Then
                    lnkScNo.NavigateUrl = dDispatcher.direct("StaffClaim", "OtherClaim.aspx", "index=" & dv("SCM_CLAIM_INDEX") & "&pageid=" & strPageId)
                ElseIf strForm = "Outstation" Then
                    lnkScNo.NavigateUrl = dDispatcher.direct("StaffClaim", "OutstationClaim.aspx", "index=" & dv("SCM_CLAIM_INDEX") & "&pageid=" & strPageId)
                End If
            Else
                lnkScNo.NavigateUrl = dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & dv("SCM_CLAIM_INDEX") & "&pageid=" & strPageId)
            End If

            'e.Item.Cells(EnumSC.icCreationDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("SCM_CREATED_DATE"))
            e.Item.Cells(EnumSC.icCreationDate).Text = Format(CDate(dv("SCM_CREATED_DATE")), "dd/MM/yyyy")

            If Not IsDBNull(dv("SCM_SUBMIT_DATE")) Then
                'e.Item.Cells(EnumSC.icSubmitDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("SCM_SUBMIT_DATE"))
                e.Item.Cells(EnumSC.icSubmitDate).Text = Format(CDate(dv("SCM_SUBMIT_DATE")), "dd/MM/yyyy")
            End If

            e.Item.Cells(EnumSC.icAmount).Text = Format(dv("CLAIM_AMT"), "##,##0.00")

        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_Staff_Claim_tabs") = "<div class=""t_entity""><ul>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("StaffClaim", "TransportationClaim.aspx", "pageid=" & strPageId) & """><span>Staff Claim Documents</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""#""><span>Staff Claim Listing</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "</ul><div></div></div>"

    End Sub
End Class


