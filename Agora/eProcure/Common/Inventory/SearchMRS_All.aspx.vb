'Copyright © 2013 STRATEQ GLOBAL SERVICES. All rights reserved.
'//Attachment Ind
Imports AgoraLegacy
Imports eProcure.Component
Public Class SearchMRS_All
    Inherits AgoraLegacy.AppBaseClass
    'Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtDateFr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBuyer As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkApproved As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkReject As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents vldDateFtDateTo As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents vldDateFr As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents vldDateTo As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblVendor As System.Web.UI.WebControls.Label
    Protected WithEvents lblStartDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblEndDate As System.Web.UI.WebControls.Label

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals

    Public Enum EnumMRS
        icMRSNo
        icMRSDate
        icMRSIssuedDate
        icAccCode
        icIssueTo
        icDept
        icRefNo
        icRemark
        icStatus
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
        SetGridProperty(dtgMRSList)

        MyBase.Page_Load(sender, e)

        If Not Page.IsPostBack Then
            txtDateFr.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
            hidDateS.Value = txtDateFr.Text
            txtDateTo.Text = DateTime.Now.ToShortDateString()
            hidDateE.Value = txtDateTo.Text
            txtIssueDateFr.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
            hidIssueDateS.Value = txtIssueDateFr.Text
            txtIssueDateTo.Text = DateTime.Now.ToShortDateString()
            hidIssueDateE.Value = txtIssueDateTo.Text
            Session("strURL") = strCallFrom
            GenerateTab()
        End If
    End Sub

    Public Sub dtgMRSList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgMRSList.PageIndexChanged
        dtgMRSList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgMRSList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim objInv As New Inventory 'PurchaseOrder
        Dim strMsg As String

        '//Retrieve Data from Database
        Dim ds As DataSet
        Dim aryRemarks As New ArrayList
        Dim comparedt As Date
        comparedt = DateAdd("m", -6, CDate(txtDateTo.Text))

        If CDate(txtDateFr.Text) < comparedt Then
            strMsg = "Start/ End date " & objGlobal.GetErrorMessage("00015") & " 6 months"
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Return False
        End If

        comparedt = DateAdd("m", -6, CDate(txtIssueDateTo.Text))

        If CDate(txtIssueDateFr.Text) < comparedt Then
            strMsg = "Issued Start/ End date " & objGlobal.GetErrorMessage("00015") & " 6 months"
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Return False
        End If

        If chkIssued.Checked Then
            aryRemarks.Add("Issued")
            aryRemarks.Add("Acknowledged")
            aryRemarks.Add("Auto- Acknowledged")
            aryRemarks.Add("Cancelled")
            aryRemarks.Add("Partial Issued")
        End If

        If chkReject.Checked Then
            aryRemarks.Add("Rejected")
        End If

        If chkIssued.Checked And chkReject.Checked Then aryRemarks = Nothing
        ds = objInv.GetMRSList(txtMRSNo.Text, txtAccCode.Text, "", "", txtDateFr.Text, txtDateTo.Text, txtIssueDateFr.Text, txtIssueDateTo.Text, aryRemarks, "Ind", IIf(chkIncludeRejMRS.Checked, "Y", "N"))

        '//for sorting asc or desc
        Dim dvViewMRS As DataView
        dvViewMRS = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewMRS.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewMRS.Sort += " DESC"
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgMRSList, dvViewMRS)
            dtgMRSList.DataSource = dvViewMRS
            dtgMRSList.DataBind()
        Else
            dtgMRSList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ViewState("PageCount") = dtgMRSList.PageCount
    End Function

    Private Sub dtgMRSList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgMRSList.ItemCreated
        Grid_ItemCreated(dtgMRSList, e)
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgMRSList.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "IRSM_IRS_DATE"
        Bindgrid(True)
    End Sub

    Private Sub dtgMRSList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgMRSList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkMRSNo As HyperLink
            lnkMRSNo = e.Item.Cells(EnumMRS.icMRSNo).FindControl("lnkMRSNo")
            lnkMRSNo.NavigateUrl = dDispatcher.direct("Inventory", "MRSDetail.aspx", "caller=SearchMRS_All&pageid=" & strPageId & "&index=" & dv("IRSM_IRS_INDEX") & "&MRSNo=" & dv("IRSM_IRS_NO"))
            lnkMRSNo.Text = dv("IRSM_IRS_NO")

            If Common.parseNull(dv("IRSM_IRS_URGENT")) = "Y" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(EnumMRS.icMRSNo).Controls.Add(imgAttach)
            End If

            If IsDBNull(dv("IRSM_IRS_DATE")) Then
                e.Item.Cells(EnumMRS.icMRSDate).Text = ""
            Else
                e.Item.Cells(EnumMRS.icMRSDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IRSM_IRS_DATE"))
            End If

            If IsDBNull(dv("IRSM_IRS_APPROVED_DATE")) Then
                e.Item.Cells(EnumMRS.icMRSIssuedDate).Text = ""
            Else
                e.Item.Cells(EnumMRS.icMRSIssuedDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IRSM_IRS_APPROVED_DATE"))
            End If

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

        Session("w_SearchMRSAll_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "SearchMRS_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "SearchMRS_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "MRSListing.aspx", "pageid=" & strPageId) & """><span>MRS Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
    End Sub
End Class


