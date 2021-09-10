Imports AgoraLegacy
Imports eProcure.Component

Public Class ContractCatalogueApproval
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
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkBuyerPending As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkBuyerReject As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkHubPending As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents dtgCatalogue As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtVendor As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateFr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents chkHubReject As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkHubApprove As System.Web.UI.WebControls.CheckBox

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
        SetGridProperty(dtgCatalogue)

        If Not IsPostBack Then
            lblTitle.Text = "Contract Catalogue Approval"
            chkBuyerPending.Checked = True
            Bindgrid()
        End If
    End Sub

    Private Function getStatusList() As String
        Dim strStatus As String = ""
        If chkBuyerPending.Checked Then
            strStatus = IIf(strStatus = "", CatalogueStatus.BuyerPending, strStatus & "," & CatalogueStatus.BuyerPending)
        End If

        If chkBuyerReject.Checked Then
            strStatus = IIf(strStatus = "", CatalogueStatus.BuyerRejected, strStatus & "," & CatalogueStatus.BuyerRejected)
        End If

        If chkHubPending.Checked Then
            strStatus = IIf(strStatus = "", CatalogueStatus.HubPending, strStatus & "," & CatalogueStatus.HubPending)
        End If

        ' ai chu add on 04/10/2005
        ' SR form raised by user1 - to allow PM to view rejected/approved contract catalogue
        If chkHubReject.Checked Then
            strStatus = IIf(strStatus = "", CatalogueStatus.Rejected, strStatus & "," & CatalogueStatus.Rejected)
        End If

        If chkHubApprove.Checked Then
            strStatus = IIf(strStatus = "", CatalogueStatus.Approved, strStatus & "," & CatalogueStatus.Approved)
        End If

        getStatusList = strStatus
    End Function

    Private Function Bindgrid() As String
        Dim objCat As New ContCat
        Dim ds As New DataSet
        ds = objCat.getBuyerContractCatalogueList(getStatusList, txtCode.Text, txtDesc.Text, txtVendor.Text, txtDateFr.Text, txtDateTo.Text)

        intPageRecordCnt = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgCatalogue, dvViewSample)
            dtgCatalogue.DataSource = dvViewSample
            dtgCatalogue.DataBind()
        Else
            Common.NetMsgbox(Me, MsgNoRecord)
            dtgCatalogue.DataBind()
        End If
        ' add for above checking
        viewstate("PageCount") = dtgCatalogue.PageCount
        objCat = Nothing
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

            ' ai chu modified on 13/10/2005
            ' need to pass the Upload Group Index so that item can be retrieved from CONT_DIST_UPLOADITEMS table
            ' CDUM_Upload_Index
            Select Case strStatus
                Case "2"
                    'lnkCode.NavigateUrl = "ContractCatalogue.aspx?rej=" & intBuyerReject & "&comp=B&mode=mod&type=B&code=" & lnkCode.Text & "&buyerstatus=" & strStatus & "&pageid=" & strPageId & "&index=" & Common.parseNull(dv("CDUM_Upload_Index"))
                    lnkCode.NavigateUrl = dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "rej=" & intBuyerReject & "&comp=B&mode=mod&type=B&code=" & lnkCode.Text & "&buyerstatus=" & strStatus & "&pageid=" & strPageId & "&index=" & Common.parseNull(dv("CDUM_Upload_Index")))

                Case "3", "4", "5", "6"
                    'lnkCode.NavigateUrl = "ContractCatalogue.aspx?comp=B&mode=mod&type=B&code=" & lnkCode.Text & "&buyerstatus=" & strStatus & "&pageid=" & strPageId & "&index=" & Common.parseNull(dv("CDUM_Upload_Index"))
                    lnkCode.NavigateUrl = dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "comp=B&mode=mod&type=B&code=" & lnkCode.Text & "&buyerstatus=" & strStatus & "&pageid=" & strPageId & "&index=" & Common.parseNull(dv("CDUM_Upload_Index")))

                    'Case "6"
                    '    lnkCode.NavigateUrl = "ContractCatalogue.aspx?comp=B&mode=mod&type=A&code=" & lnkCode.Text & "&buyerstatus=" & strStatus & "&pageid=" & strPageId & "&index=" & Common.parseNull(dv("CDUM_Upload_Index"))
            End Select
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
