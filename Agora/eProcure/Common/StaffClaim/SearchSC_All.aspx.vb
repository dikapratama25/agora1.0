'//Attachment Ind
Imports AgoraLegacy
Imports eProcure.Component
Public Class SearchSC_All
    Inherits AgoraLegacy.AppBaseClass
    'Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtDateFr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateTo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtBuyer As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkApproved As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkReject As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents vldDateFtDateTo As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents vldDateFr As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents vldDateTo As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    'Protected WithEvents lblVendor As System.Web.UI.WebControls.Label
    Protected WithEvents lblStartDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblEndDate As System.Web.UI.WebControls.Label

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objSc As New eProcStaffClaim

    Public Enum EnumSC
        icSCNo
        icSubmitDate
        icStaff
        icDept
        icAmt
        icStatus
        icSCNo1
        icSCIndex
    End Enum
    'Dim strCaller As String
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
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        blnCheckBox = False
        SetGridProperty(dtgSCList)

        MyBase.Page_Load(sender, e)

        If Not Page.IsPostBack Then
            Session("strURL") = strCallFrom
            Common.FillDefault(cboDeptName, "COMPANY_DEPT_MSTR", "CDM_DEPT_NAME", "CDM_DEPT_CODE", "---Select---", " CDM_DELETED<>'Y' AND CDM_COY_ID='" & Session("CompanyId") & "'")
            GenerateTab()
        End If
        Session("urlreferer") = "SearchSCAll"
    End Sub

    Public Sub dtgPOList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgSCList.PageIndexChanged
        dtgSCList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgSCList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub Bindgrid(Optional ByVal pSorted As Boolean = False)

        Dim objPO As New PurchaseOrder_AO 'PurchaseOrder

        'Retrieve Data from Database
        Dim ds As DataSet
        Dim dsTemp As DataSet
        Dim strAOAction As String = ""
        Dim strInclude As String = ""

        If chkApproved.Checked Then
            strAOAction = "Approved"
        End If

        If chkInclude.Checked Then
            strInclude = "Included"
        End If

        If chkReject.Checked Then
            strAOAction = "Rejected"
        End If

        If chkApproved.Checked And chkReject.Checked Then strAOAction = ""
        dsTemp = objSc.getSCListForApproval(txtSCNo.Text, txtDateFr.Text, txtDateTo.Text, "", "app", "", strAOAction, strInclude, txtStaffId.Text, txtStaffName.Text, cboDeptName.SelectedValue)
        ds = GetAllClaimAmt(dsTemp)

        '//for sorting asc or desc
        Dim dvViewSC As DataView
        dvViewSC = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewSC.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSC.Sort += " DESC"
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgSCList, dvViewSC)
            dtgSCList.DataSource = dvViewSC
            dtgSCList.DataBind()
        Else
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
            dtr("STAFF_NAME") = dsTemp.Tables(0).Rows(i)("STAFF_NAME")
            dtr("CDM_DEPT_NAME") = dsTemp.Tables(0).Rows(i)("CDM_DEPT_NAME")
            dt.Rows.Add(dtr)
        Next

        ds.Tables.Add(dt)
        Return ds
    End Function

    Private Sub dtgSCList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgSCList.ItemCreated
        Grid_ItemCreated(dtgSCList, e)
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgSCList.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "SCM_CREATED_DATE"
        Bindgrid(True)
    End Sub

    Private Sub dtgSCList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgSCList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkSCNo As HyperLink

            lnkSCNo = e.Item.Cells(EnumSC.icSCNo).FindControl("lnkSCNo")
            lnkSCNo.NavigateUrl = dDispatcher.direct("StaffClaim", "SCApprDetail.aspx", "pageid=" & strPageId & "&index=" & dv("SCM_CLAIM_INDEX") & "&SCNo=" & dv("SCM_CLAIM_DOC_NO"))
            lnkSCNo.Text = dv("SCM_CLAIM_DOC_NO")

            e.Item.Cells(EnumSC.icSubmitDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("SCM_SUBMIT_DATE"))
            e.Item.Cells(EnumSC.icAmt).Text = Format(dv("CLAIM_AMT"), "##,##0.00")

        End If
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
 
        Session("w_SearchSCAll_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("StaffClaim", "SearchSC_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("StaffClaim", "SearchSC_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
    End Sub
End Class


