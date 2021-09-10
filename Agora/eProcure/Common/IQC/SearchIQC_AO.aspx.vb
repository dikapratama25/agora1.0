'//Outstanding
'//RFQ Ind
'//Attachment Ind

Imports AgoraLegacy
Imports eProcure.Component
Public Class SearchIQC_AO
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals
    Dim strCaller As String
    Public Enum EnumIQC
        icIQCNoLink
        icItemCode
        icItemName
        icGRNNo
        icSubmitDate
        icLotQty
        icLotNo
        icVendor
        icManu
        icIQCType
    End Enum
    Dim strAO As String
    'Protected WithEvents lblVendor As System.Web.UI.WebControls.Label
    Dim blnRelief As Boolean
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
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgIQCList)

        strAO = Session("UserID")
        blnRelief = False

        If Not Page.IsPostBack Then
            GenerateTab()
            getIQCList()
            Bindgrid()
        End If

        Session("urlreferer") = "SearchIQCAO"
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
        Dim objIQC As New Inventory
        Dim ds As DataSet
        Dim strIQCType As String = ""

        strIQCType = cboIQC.SelectedValue
        ds = objIQC.GetInvVerifyLotApprList(txtIQCNo.Text, txtVendor.Text, txtItemCode.Text, txtManu.Text, txtDateFr.Text, txtDateTo.Text, cboIQC.SelectedValue)

        '//for sorting asc or desc
        Dim dvViewIQC As DataView
        dvViewIQC = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewIQC.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewIQC.Sort += " DESC"
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgIQCList, dvViewIQC)
            dtgIQCList.DataSource = dvViewIQC
            dtgIQCList.DataBind()
        Else
            dtgIQCList.DataBind()
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00006"))
        End If
        ViewState("PageCount") = dtgIQCList.PageCount
    End Function

    Private Sub dtgIQCList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIQCList.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgIQCList, e)
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgIQCList.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "GM_DATE_RECEIVED"
        Bindgrid(True)
    End Sub

    Private Sub dtgPRList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIQCList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            ' To dynamic build hyperlink
            Dim lnkIQCNo As HyperLink
            lnkIQCNo = e.Item.Cells(EnumIQC.icIQCNoLink).FindControl("lnkIQCNo")
            lnkIQCNo.NavigateUrl = dDispatcher.direct("IQC", "IQCApprDetail.aspx", "caller=approval&AO=" & strAO & "&relief=" & blnRelief & "&PageID=" & strPageId & "&index=" & dv("IVL_VERIFY_LOT_INDEX") & "&IQCNo=" & dv("IVL_IQC_NO"))
            lnkIQCNo.Text = dv("IVL_IQC_NO")

            e.Item.Cells(EnumIQC.icSubmitDate).Text = Format(CDate(dv("GM_DATE_RECEIVED")), "dd/MM/yyyy")
        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_SearchPRAO_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IQC", "SearchIQC_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IQC", "SearchIQC_All.aspx", "pageid=" & strPageId) & """><span>Closed / Outstanding Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
    End Sub
End Class


