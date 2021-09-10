Imports AgoraLegacy
Imports eProcure.Component

Public Class ContractCatalogueList
    Inherits AgoraLegacy.AppBaseClass

    Public Enum EnumCat
        icChk = 0
        icCode = 1
        icDesc = 2
        icCoyName = 3
        icStartDate = 4
        icEndDate = 5
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
    Protected WithEvents dtgCatalogue As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents divApprove As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents chkHubApprove As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkBuyerReject As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkBuyerPending As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkReject As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkHubPending As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtBuyer As System.Web.UI.WebControls.TextBox
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtDateFr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkDraft As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cmdSelectAll As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents divApprove2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdItem As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents trDiscount As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdModify)
        'alButtonList.Add(cmdItem)
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdDelete)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        cmdAdd.Enabled = blnCanAdd And viewstate("blnCmdAdd")
        cmdModify.Enabled = blnCanUpdate And viewstate("blnCmdModify")
        'cmdItem.Enabled = blnCanAdd And blnCanUpdate And blnCmdItem
        cmdDelete.Enabled = blnCanDelete And viewstate("blnCmdDelete")
        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCatalogue)
        If Not IsPostBack Then
            viewstate("blnCmdAdd") = True
            viewstate("blnCmdModify") = True
            viewstate("blnCmdItem") = True
            viewstate("blnCmdDelete") = True
            viewstate("type") = Request.QueryString("type")
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            cmdItem.Enabled = False
            viewstate("blnCmdModify") = False
            viewstate("blnCmdDelete") = False
            viewstate("blnCmdItem") = False

            If viewstate("type") = "" Then
                viewstate("type") = "D"
            End If

            Select Case viewstate("type")
                Case "A" ' view approved contract catalogue
                    lblTitle.Text = "Contract Catalogue"
                    divApprove.Visible = False
                    divApprove2.Visible = False
                    cmdSelectAll.Visible = False
                    'cmdCreate.Visible = False
                    trDiscount.Visible = False
                    Bindgrid()
                Case "D" ' view not yet approved contract catalogue
                    lblTitle.Text = "Contract Catalogue Maintenance"
                    divApprove.Visible = True
                    divApprove2.Visible = True
            End Select
        End If
        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        cmdItem.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
    End Sub

    Private Function getStatusList() As String
        Dim strStatus As String = ""
        If chkDraft.Checked Then
            strStatus = IIf(strStatus = "", CatalogueStatus.Draft, strStatus & "," & CatalogueStatus.Draft)
        End If

        If chkBuyerPending.Checked Then
            strStatus = IIf(strStatus = "", CatalogueStatus.BuyerPending, strStatus & "," & CatalogueStatus.BuyerPending)
        End If

        If chkBuyerReject.Checked Then
            strStatus = IIf(strStatus = "", CatalogueStatus.BuyerRejected, strStatus & "," & CatalogueStatus.BuyerRejected)
        End If

        If chkHubPending.Checked Then
            strStatus = IIf(strStatus = "", CatalogueStatus.HubPending, strStatus & "," & CatalogueStatus.HubPending)
        End If

        If chkHubApprove.Checked Then
            strStatus = IIf(strStatus = "", CatalogueStatus.Approved, strStatus & "," & CatalogueStatus.Approved)
        End If

        If chkReject.Checked Then
            strStatus = IIf(strStatus = "", CatalogueStatus.Rejected, strStatus & "," & CatalogueStatus.Rejected)
        End If

        getStatusList = strStatus
    End Function

    Private Function Bindgrid() As String
        Dim objCat As New ContCat
        Dim ds As New DataSet
        Select Case viewstate("type")
            Case "D"
                ds = objCat.getContractCatalogueList(getStatusList, txtCode.Text, txtDesc.Text, txtBuyer.Text, "D", txtDateFr.Text, txtDateTo.Text)
            Case "A"
                ds = objCat.getCatalogueList(Session("CompanyId"), "A", "C", txtCode.Text, txtDesc.Text, txtBuyer.Text, txtDateFr.Text, txtDateTo.Text)
        End Select

        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        ''//these only needed if you can select a grid item and click delete button
        ''//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        ''//then user delete one record. //total record = 20 (2 pages), 
        ''//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If dtgCatalogue.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgCatalogue.PageSize = 0 Then
                dtgCatalogue.CurrentPageIndex = dtgCatalogue.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        If viewstate("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtgCatalogue, dvViewSample)
            dtgCatalogue.DataSource = dvViewSample
            dtgCatalogue.DataBind()
            cmdModify.Enabled = True
            cmdDelete.Enabled = True
            cmdItem.Enabled = True
            viewstate("blnCmdModify") = True
            viewstate("blnCmdDelete") = True
            viewstate("blnCmdItem") = True
        Else
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            cmdItem.Enabled = False
            viewstate("blnCmdModify") = False
            viewstate("blnCmdDelete") = False
            viewstate("blnCmdItem") = False
            Common.NetMsgbox(Me, MsgNoRecord)
            dtgCatalogue.DataBind()
        End If
        ' add for above checking
        viewstate("PageCount") = dtgCatalogue.PageCount
        objCat = Nothing
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_SortCommand(sender, e)
        dtgCatalogue.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgCatalogue_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgCatalogue, e)
        If viewstate("type") <> "A" Then
            If e.Item.ItemType = ListItemType.Header Then
                Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
                chkAll.Attributes.Add("onclick", "selectAll();")
            End If
        End If
    End Sub

    Private Sub dtgCatalogue_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkCode As HyperLink
            lnkCode = e.Item.FindControl("lnkCode")
            lnkCode.Text = dv("CDUM_Contract_Code")

            Dim lblIndex As Label
            lblIndex = e.Item.FindControl("lblIndex")
            lblIndex.Text = Common.parseNull(dv("CDUM_Upload_Index"))

            If viewstate("type") <> "A" Then
                Dim chk As CheckBox
                chk = e.Item.Cells(0).FindControl("chkSelection")
                chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            Else
                'lnkCode.NavigateUrl = "ContractCatalogue.aspx?comp=V&mode=mod&type=A&code=" & lnkCode.Text & "&pageid=" & strPageId & "&index=" & Common.parseNull(dv("CDUM_Upload_Index"))
                lnkCode.NavigateUrl = dDispatcher.direct("Catalogue", "ContractCatalogueItem.aspx", "pageid=" & strPageId & "&index=" & Common.parseNull(dv("CDUM_Upload_Index")))
            End If

            e.Item.Cells(EnumCat.icStartDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, e.Item.Cells(EnumCat.icStartDate).Text)
            e.Item.Cells(EnumCat.icEndDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, e.Item.Cells(EnumCat.icEndDate).Text)

            Dim intBuyerReject As Integer
            Dim intHubReject As Integer
            Dim strStatus As String

            intBuyerReject = CInt(e.Item.Cells(EnumCat.icBReject).Text)
            intHubReject = CInt(e.Item.Cells(EnumCat.icHReject).Text)
            strStatus = e.Item.Cells(EnumCat.icUploadStatus).Text

            'Select Case strStatus
            '    Case "1"
            '        lnkCode.NavigateUrl = "ContractCatalogue.aspx?comp=V&mode=mod&type=D&code=" & lnkCode.Text & "&pageid=" & strPageId

            '    Case "2", "4"
            '        lnkCode.NavigateUrl = "ContractCatalogue.aspx?comp=V&mode=mod&type=R&code=" & lnkCode.Text & "&pageid=" & strPageId

            '    Case "3"
            '        If (intBuyerReject = 1 And intHubReject = 0) Or (intBuyerReject = 2 And intHubReject = 1) Or (intBuyerReject = 1 And intHubReject = 1) Then
            '            lnkCode.NavigateUrl = "ContractCatalogue.aspx?comp=V&mode=mod&type=S&code=" & lnkCode.Text & "&pageid=" & strPageId
            '        Else
            '            lnkCode.NavigateUrl = "ContractCatalogue.aspx?comp=V&mode=mod&type=R&code=" & lnkCode.Text & "&pageid=" & strPageId
            '        End If

            '    Case "5"
            '        If (intBuyerReject = 0 And intHubReject = 1) Or (intBuyerReject = 1 And intHubReject = 1) Then
            '            lnkCode.NavigateUrl = "ContractCatalogue.aspx?comp=V&mode=mod&type=S&code=" & lnkCode.Text & "&pageid=" & strPageId
            '        Else
            '            lnkCode.NavigateUrl = "ContractCatalogue.aspx?comp=V&mode=mod&type=R&code=" & lnkCode.Text & "&pageid=" & strPageId
            '        End If

            '    Case "6"
            '        lnkCode.NavigateUrl = "ContractCatalogue.aspx?comp=V&mode=mod&type=A&code=" & lnkCode.Text & "&pageid=" & strPageId
            'End Select

            If ViewState("type") = "A" Then
                e.Item.Cells(EnumCat.icStatus).Visible = False
                e.Item.Cells(EnumCat.icChk).Visible = False
            End If

        ElseIf e.Item.ItemType = ListItemType.Header Then
            If ViewState("type") = "A" Then
                e.Item.Cells(EnumCat.icStatus).Visible = False
                e.Item.Cells(EnumCat.icChk).Visible = False
            End If
        End If
    End Sub

    'Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Response.Redirect("ContractCatalogue.aspx?comp=V&mode=add&type=D")
    'End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgCatalogue.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgCatalogue_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCatalogue.PageIndexChanged
        dtgCatalogue.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox
        Dim strStatus As String
        Dim intBuyerReject As Integer
        Dim intHubReject As Integer
        Dim strCode As String
        Dim strMsg As String = "Only draft contract group or contract group rejected by buyer (1 time) can be modified."

        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                ' ai chu modified on 05/10/2005
                ' SR U30031 
                intBuyerReject = CInt(dgItem.Cells(EnumCat.icBReject).Text)
                intHubReject = CInt(dgItem.Cells(EnumCat.icHReject).Text)
                strStatus = dgItem.Cells(EnumCat.icUploadStatus).Text
                strCode = CType(dgItem.FindControl("lnkCode"), HyperLink).Text

                Select Case strStatus
                    Case "1"
                        'Response.Redirect("AddCatalogue.aspx?code=" & Server.UrlEncode(strCode) & "&type=" & viewstate("type") & "&mode=mod&pageid=" & strPageId & "&status=" & strStatus)
                        Response.Redirect(dDispatcher.direct("Catalogue", "AddCatalogue.aspx", "code=" & Server.UrlEncode(strCode) & "&type=" & ViewState("type") & "&mode=mod&pageid=" & strPageId & "&status=" & strStatus))

                    Case "2", "4", "6"
                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)

                    Case "3"
                        If (intBuyerReject = 1 And intHubReject = 0) Or (intBuyerReject = 2 And intHubReject = 1) Or (intBuyerReject = 2 And intHubReject = 0) Or (intBuyerReject = 1 And intHubReject = 1) Then
                            'Response.Redirect("AddCatalogue.aspx?code=" & Server.UrlEncode(strCode) & "&type=" & viewstate("type") & "&mode=mod&pageid=" & strPageId & "&status=" & strStatus)
                            Response.Redirect(dDispatcher.direct("Catalogue", "AddCatalogue.aspx", "code=" & Server.UrlEncode(strCode) & "&type=" & ViewState("type") & "&mode=mod&pageid=" & strPageId & "&status=" & strStatus))
                        Else
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        End If

                    Case "5"
                        If (intBuyerReject = 0 And intHubReject = 1) Or (intBuyerReject = 1 And intHubReject = 1) Then
                            'Response.Redirect("AddCatalogue.aspx?code=" & Server.UrlEncode(strCode) & "&type=" & viewstate("type") & "&mode=mod&pageid=" & strPageId & "&status=" & strStatus)
                            Response.Redirect(dDispatcher.direct("Catalogue", "AddCatalogue.aspx", "code=" & Server.UrlEncode(strCode) & "&type=" & ViewState("type") & "&mode=mod&pageid=" & strPageId & "&status=" & strStatus))
                        Else
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        End If

                End Select
            End If

            'If chkItem.Checked Then
            '    Select Case strStatus
            '        Case "1", "3"
            '            Response.Redirect("AddCatalogue.aspx?code=" & strCode & "&type=" & viewstate("type") & "&mode=mod&pageid=" & strPageId & "&status=" & strStatus)
            '        Case Else
            '            Common.NetMsgbox(Me, "Only draft contract group or contract group rejected by buyer (1 time) can be modified.", MsgBoxStyle.Information)

            '    End Select
            'End If
        Next
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox
        Dim strStatus As String
        Dim intBuyerReject As Integer
        Dim intHubReject As Integer
        Dim blnDelete As Boolean = True
        Dim strError As String = "There is item(s) which cannot be deleted. Please re-select the item(s)."

        Dim objCat As New ContCat
        Dim dtMaster As New DataTable
        Dim dtr As DataRow
        dtMaster.Columns.Add("index", Type.GetType("System.Int32"))

        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")

            If chkItem.Checked Then
                intBuyerReject = CInt(dgItem.Cells(EnumCat.icBReject).Text)
                intHubReject = CInt(dgItem.Cells(EnumCat.icHReject).Text)
                strStatus = dgItem.Cells(EnumCat.icUploadStatus).Text

                Select Case strStatus
                    Case "1"
                        dtr = dtMaster.NewRow()
                        dtr("index") = CType(dgItem.FindControl("lblIndex"), Label).Text
                        dtMaster.Rows.Add(dtr)

                        'Case "3"
                        'If (intBuyerReject = 1 And intHubReject = 0) Or (intBuyerReject = 2 And intHubReject = 0) Or (intBuyerReject = 2 And intHubReject = 1) Or (intBuyerReject = 2 And intHubReject = 1) Or (intBuyerReject = 1 And intHubReject = 1) Then
                        '    dtr = dtMaster.NewRow()
                        '    dtr("index") = CType(dgItem.FindControl("lblIndex"), Label).Text
                        '    dtMaster.Rows.Add(dtr)
                        'Else
                        '    blnDelete = False
                        '    GoTo DeleteCatalogue
                        'End If
                    Case Else
                        blnDelete = False
                        GoTo DeleteCatalogue
                End Select
            End If
        Next

DeleteCatalogue:
        If blnDelete Then
            objCat.deleteDraftContract(dtMaster)
            viewstate("action") = "del"
            Bindgrid()
            Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        Else
            Common.NetMsgbox(Me, strError, MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        'Response.Redirect("AddCatalogue.aspx?type=" & ViewState("type") & "&mode=add&pageid=" & strPageId)
        Response.Redirect(dDispatcher.direct("Catalogue", "AddCatalogue.aspx", "type=" & ViewState("type") & "&mode=add&pageid=" & strPageId))
    End Sub

    Private Sub cmdItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdItem.Click
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox
        Dim strRedirect As String
        Dim strCode As String
        Dim intBuyerReject As Integer
        Dim intHubReject As Integer
        Dim strStatus As String
        Dim strIndex As String

        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")

            If chkItem.Checked Then
                intBuyerReject = CInt(dgItem.Cells(EnumCat.icBReject).Text)
                intHubReject = CInt(dgItem.Cells(EnumCat.icHReject).Text)
                strStatus = dgItem.Cells(EnumCat.icUploadStatus).Text
                strCode = CType(dgItem.FindControl("lnkCode"), HyperLink).Text

                ' ai chu modified on 13/10/2005
                ' need to pass upload group index so that item can be retrieved from CONT_DIST_UPLOADITEMS table
                strIndex = CType(dgItem.FindControl("lblIndex"), Label).Text

                Select Case strStatus
                    Case "1"
                        'strRedirect = "ContractCatalogue.aspx?comp=V&mode=mod&type=D&code=" & Server.UrlEncode(strCode) & "&pageid=" & strPageId & "&index=" & strIndex
                        strRedirect = dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "comp=V&mode=mod&type=D&code=" & Server.UrlEncode(strCode) & "&pageid=" & strPageId & "&index=" & strIndex)

                    Case "2", "4"
                        'strRedirect = "ContractCatalogue.aspx?comp=V&mode=mod&type=R&code=" & Server.UrlEncode(strCode) & "&pageid=" & strPageId & "&index=" & strIndex
                        strRedirect = dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "comp=V&mode=mod&type=R&code=" & Server.UrlEncode(strCode) & "&pageid=" & strPageId & "&index=" & strIndex)

                    Case "3"
                        If (intBuyerReject = 1 And intHubReject = 0) Or (intBuyerReject = 2 And intHubReject = 1) Or (intBuyerReject = 2 And intHubReject = 0) Or (intBuyerReject = 1 And intHubReject = 1) Then
                            'strRedirect = "ContractCatalogue.aspx?comp=V&mode=mod&type=S&code=" & Server.UrlEncode(strCode) & "&pageid=" & strPageId & "&index=" & strIndex
                            strRedirect = dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "comp=V&mode=mod&type=S&code=" & Server.UrlEncode(strCode) & "&pageid=" & strPageId & "&index=" & strIndex)
                        Else
                            'strRedirect = "ContractCatalogue.aspx?comp=V&mode=mod&type=R&code=" & Server.UrlEncode(strCode) & "&pageid=" & strPageId & "&index=" & strIndex
                            strRedirect = dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "comp=V&mode=mod&type=R&code=" & Server.UrlEncode(strCode) & "&pageid=" & strPageId & "&index=" & strIndex)
                        End If

                    Case "5"
                        If (intBuyerReject = 0 And intHubReject = 1) Or (intBuyerReject = 1 And intHubReject = 1) Then
                            'strRedirect = "ContractCatalogue.aspx?comp=V&mode=mod&type=S&code=" & Server.UrlEncode(strCode) & "&pageid=" & strPageId & "&index=" & strIndex
                            strRedirect = dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "comp=V&mode=mod&type=S&code=" & Server.UrlEncode(strCode) & "&pageid=" & strPageId & "&index=" & strIndex)
                        Else
                            'strRedirect = "ContractCatalogue.aspx?comp=V&mode=mod&type=R&code=" & Server.UrlEncode(strCode) & "&pageid=" & strPageId & "&index=" & strIndex
                            strRedirect = dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "comp=V&mode=mod&type=R&code=" & Server.UrlEncode(strCode) & "&pageid=" & strPageId & "&index=" & strIndex)
                        End If

                    Case "6"
                        'strRedirect = "ContractCatalogue.aspx?comp=V&mode=mod&type=A&code=" & Server.UrlEncode(strCode) & "&pageid=" & strPageId & "&index=" & strIndex
                        strRedirect = dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "comp=V&mode=mod&type=A&code=" & Server.UrlEncode(strCode) & "&pageid=" & strPageId & "&index=" & strIndex)
                End Select
                Response.Redirect(strRedirect)
            End If
        Next
    End Sub
End Class
