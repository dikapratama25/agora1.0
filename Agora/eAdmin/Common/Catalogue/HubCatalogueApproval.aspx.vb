Imports AgoraLegacy
Imports SSO.Component
Public Class HubCatalogueApproval
    Inherits AgoraLegacy.AppBaseClass

    Public Enum EnumCat
        icCode = 0
        icDesc = 1
        icCoyName = 2
        icStartDate = 3
        icEndDate = 4
        icSubmissionDate = 5
        icStatus = 6
        icBReject = 7
        icHReject = 8
        icUploadStatus = 9
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBuyer As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkHubPending As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkReject As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents dtgCatalogue As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtDateFr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Dim dDispatcher As New AgoraLegacy.dispatcher

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        Dim objComs As New Companies
        Dim strCompanyType As String
        strCompanyType = objComs.GetCompanyType(Session("CompanyIdToken"))
        If strCompanyType.ToUpper = "VENDOR" Or strCompanyType.ToUpper = "BOTH" Then
            SetGridProperty(dtgCatalogue)
            If Not IsPostBack Then
                lblTitle.Text = "Contract Catalogue Approval"
                chkHubPending.Checked = True
                cmdSearch_Click(sender, e)
            End If
        Else
            Dim strMsg As String
            Dim objCat As New ContCat
            strMsg = objCat.BuyerDisallowedMsg("C", True)
            'Common.NetMsgbox(Me, strMsg, "../Homepage.aspx", MsgBoxStyle.Exclamation)
            Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Initial", "Homepage.aspx"), MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Function getStatusList() As String
        Dim strStatus As String = ""
        If chkHubPending.Checked Then
            strStatus = IIf(strStatus = "", CatalogueStatus.HubPending, strStatus & "," & CatalogueStatus.HubPending)
        End If

        If chkReject.Checked Then
            strStatus = IIf(strStatus = "", CatalogueStatus.Rejected, strStatus & "," & CatalogueStatus.Rejected)
        End If

        getStatusList = strStatus
    End Function

    Private Function Bindgrid() As String
        Dim objCat As New ContCat
        Dim ds As New DataSet
        ds = objCat.getContractCatalogueList(getStatusList, txtCode.Text, txtDesc.Text, txtBuyer.Text, "H", txtDateFr.Text, txtDateTo.Text)
        intPageRecordCnt = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            ' check when user re-enter search criteria and click on other page without click search button
            If dtgCatalogue.CurrentPageIndex > (dvViewSample.Count \ dtgCatalogue.PageSize) Then
                dtgCatalogue.CurrentPageIndex = IIf((dvViewSample.Count \ dtgCatalogue.PageSize) = 1, 0, (dvViewSample.Count \ dtgCatalogue.PageSize))
            ElseIf dtgCatalogue.CurrentPageIndex = (dvViewSample.Count \ dtgCatalogue.PageSize) Then
                If viewstate("PageCount") = (dvViewSample.Count \ dtgCatalogue.PageSize) Then
                    'user does not re-enter search criteria 
                    dtgCatalogue.CurrentPageIndex = IIf((dvViewSample.Count \ dtgCatalogue.PageSize) = 0, 0, (dvViewSample.Count \ dtgCatalogue.PageSize) - 1)
                Else
                    If (dvViewSample.Count Mod dtgCatalogue.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dtgCatalogue.CurrentPageIndex = IIf((dvViewSample.Count \ dtgCatalogue.PageSize) = 1, 0, (dvViewSample.Count \ dtgCatalogue.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        dtgCatalogue.CurrentPageIndex = (dvViewSample.Count \ dtgCatalogue.PageSize)
                    End If
                End If
            End If
            '--------------------------------
            dtgCatalogue.DataSource = dvViewSample
            dtgCatalogue.DataBind()
        Else
            Common.NetMsgbox(Me, MsgNoRecord)
            dtgCatalogue.DataBind()
        End If
        ' add for above checking
        viewstate("PageCount") = dtgCatalogue.PageCount
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgCatalogue.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgCatalogue_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemCreated
        Grid_ItemCreated(dtgCatalogue, e)
    End Sub

    Private Sub dtgCatalogue_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkCode As HyperLink
            lnkCode = e.Item.FindControl("lnkCode")
            lnkCode.Text = dv("CDUM_Contract_Code")

            e.Item.Cells(EnumCat.icStartDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, e.Item.Cells(EnumCat.icStartDate).Text)
            e.Item.Cells(EnumCat.icEndDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, e.Item.Cells(EnumCat.icEndDate).Text)
            e.Item.Cells(EnumCat.icSubmissionDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, e.Item.Cells(EnumCat.icSubmissionDate).Text)

            Dim intBuyerReject As Integer
            Dim intHubReject As Integer
            Dim strStatus As String

            intBuyerReject = CInt(e.Item.Cells(EnumCat.icBReject).Text)
            intHubReject = CInt(e.Item.Cells(EnumCat.icHReject).Text)
            strStatus = e.Item.Cells(EnumCat.icUploadStatus).Text

            lnkCode.NavigateUrl = "HubContractCatalogue.aspx?code=" & lnkCode.Text & "&status=" & strStatus & "&rej=" & intHubReject & "&pageid=" & strPageId
            lnkCode.NavigateUrl = dDispatcher.direct("Catalogue", "HubContractCatalogue.aspx", "code=" & lnkCode.Text & "&status=" & strStatus & "&rej=" & intHubReject & "&pageid=" & strPageId)
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgCatalogue.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgCatalogue_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCatalogue.PageIndexChanged
        dtgCatalogue.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub
End Class
