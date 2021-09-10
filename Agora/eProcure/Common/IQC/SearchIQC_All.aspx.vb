'//Attachment Ind
Imports AgoraLegacy
Imports eProcure.Component
Public Class SearchIQC_All
    Inherits AgoraLegacy.AppBaseClass
    'Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtVendor As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkApproved As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkReject As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents vldDateFtDateTo As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents vldDateFr As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents vldDateTo As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents dtgIQCList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblVendor As System.Web.UI.WebControls.Label
    Protected WithEvents lblStartDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblEndDate As System.Web.UI.WebControls.Label

    Dim dDispatcher As New AgoraLegacy.dispatcher

    Public Enum EnumIQC
        icIQCNo
        icItemCode
        icItemName
        icIQCDate
        icQty
        icVendor
        icManu
        icAppStatus
        icIQCStatus
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
        SetGridProperty(dtgIQCList)

        MyBase.Page_Load(sender, e)

        If Not Page.IsPostBack Then
            Session("strURL") = strCallFrom
            getIQCList()
            GenerateTab()
        End If

        Session("urlreferer") = "SearchIQCAll"
    End Sub

    Public Sub dtgIQCList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgIQCList.PageIndexChanged
        dtgIQCList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgIQCList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim objInv As New Inventory

        '//Retrieve Data from Database
        Dim ds As DataSet
        Dim strIQCtype As String = ""
        Dim aryAllDate As New ArrayList
        Dim aryMyStatus As New ArrayList
        Dim aryIQCStatus As New ArrayList
        Dim s As String
        strIQCtype = cboIQC.SelectedValue

        'Date
        aryAllDate.Add(txtIQCDateFr.Text) 'Start Date
        aryAllDate.Add(txtIQCDateTo.Text) 'End Date
        aryAllDate.Add(txtExpDateFr.Text) 'Expiry Start Date
        aryAllDate.Add(txtExpDateTo.Text) 'Expiry End Date

        'Status
        If chkApproved.Checked Then
            aryMyStatus.Add("App")
            aryMyStatus.Add("Ver")
        End If

        If chkReject.Checked Then
            aryMyStatus.Add("Rej")
        End If

        If chkRetest.Checked Then
            aryMyStatus.Add("Ret")
        End If

        If chkClosed.Checked Then
            aryIQCStatus.Add("1")
            aryIQCStatus.Add("2")
            aryIQCStatus.Add("3")
        End If

        If chkRejected.Checked Then
            aryIQCStatus.Add("4")
        End If

        If chkOutstand.Checked Then
            aryIQCStatus.Add("0")
        End If

        If chkApproved.Checked And chkReject.Checked And chkRetest.Checked Then aryMyStatus = Nothing
        If chkClosed.Checked And chkRejected.Checked And chkOutstand.Checked Then aryIQCStatus = Nothing
        ds = objInv.getIQCListForApproval(txtIQCNo.Text, txtItemCode.Text, txtVendor.Text, txtManuName.Text, strIQCtype, aryAllDate, aryMyStatus, aryIQCStatus, IIf(chkRetest.Checked, "Y", "N"))

        '//for sorting asc or desc
        Dim dvViewIQC As DataView
        dvViewIQC = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewIQC.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewIQC.Sort += " DESC"
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgIQCList, dvViewIQC)
            dtgIQCList.DataSource = dvViewIQC
            dtgIQCList.DataBind()
        Else
            dtgIQCList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ViewState("PageCount") = dtgIQCList.PageCount
    End Function



    Private Sub dtgIQCList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIQCList.ItemCreated
        Grid_ItemCreated(dtgIQCList, e)
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgIQCList.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "GM_DATE_RECEIVED"
        Bindgrid(True)
    End Sub

    Private Sub dtgIQCList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIQCList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            'Dim strURL As String
            'Dim lnkPONo As HyperLink
            'Dim intStatus As String

            ' To dynamic build hyperlink
            Dim lnkIQCNo As HyperLink
            lnkIQCNo = e.Item.Cells(EnumIQC.icIQCNo).FindControl("lnkIQCNo")
            lnkIQCNo.NavigateUrl = dDispatcher.direct("IQC", "IQCDetail.aspx", "caller=approval&AO=" & Session("UserId") & "&relief=false&PageID=" & strPageId & "&index=" & dv("IVL_VERIFY_LOT_INDEX") & "&IQCNo=" & dv("IVL_IQC_NO"))
            lnkIQCNo.Text = dv("IVL_IQC_NO")

            e.Item.Cells(EnumIQC.icIQCDate).Text = Format(CDate(dv("GM_DATE_RECEIVED")), "dd/MM/yyyy")

            If dv("APP_STATUS") = "Retest" Then
                e.Item.Cells(EnumIQC.icAppStatus).Text = "Retest"
            Else
                If dv("RETEST") = "Y" Then
                    e.Item.Cells(EnumIQC.icAppStatus).Text = dv("APP_STATUS") & " (Retest)"
                Else
                    e.Item.Cells(EnumIQC.icAppStatus).Text = dv("APP_STATUS")
                End If
            End If

        End If
    End Sub

    Private Sub getIQCList()
        Dim cbolist As New ListItem
        Dim objAdmin As New Admin
        Dim dvCustom As DataView

        dvCustom = objAdmin.getIQCTestType()

        cboIQC.Items.Clear()
        If Not dvCustom Is Nothing Then
            Common.FillDdl(cboIQC, "CPA_PARAM_LABEL", "CPA_PARAM_LABEL", dvCustom)
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboIQC.Items.Insert(0, cbolist)

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
  
        Session("w_SearchIQCAll_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IQC", "SearchIQC_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IQC", "SearchIQC_All.aspx", "pageid=" & strPageId) & """><span>Closed / Outstanding Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
    End Sub
End Class


