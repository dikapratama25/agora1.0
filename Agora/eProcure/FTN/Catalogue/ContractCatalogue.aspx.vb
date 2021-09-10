Imports eProcure.Component
Imports AgoraLegacy


Public Class ContractCatalogueFTN1
    Inherits AgoraLegacy.AppBaseClass

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Public Enum EnumCat
        icChk = 0
        icCode = 1
        icDesc = 2
        icCoyName = 3
        icStartDate = 4
        icEndDate = 5
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVendor As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateFr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents dtgCatalogue As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents trContract As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents trDiscount As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdAdd As System.Web.UI.HtmlControls.HtmlInputButton 'System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents lblCodeLabel As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1 As System.Web.UI.WebControls.Label
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents btnHidden As System.Web.UI.WebControls.Button
    Protected WithEvents cmdViewCon As System.Web.UI.WebControls.Button


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
        If strCompanyType.ToUpper = "BUYER" Or strCompanyType.ToUpper = "BOTH" Then
            GenerateTab()
            SetGridProperty(dtgCatalogue)
            If Not IsPostBack Then
                cmdModify.Enabled = False
                cmdDelete.Enabled = False
                ViewState("cattype") = "C"
                cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
                cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
                cmdViewCon.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
            End If
        Else
            ViewState("cattype") = "C" ' Request.QueryString("cattype")
            Dim strMsg As String
            Dim objCat As New ContCat
            strMsg = objCat.BuyerDisallowedMsg1(ViewState("cattype"), False)
            'Common.NetMsgbox(Me, strMsg, "../Homepage.aspx", MsgBoxStyle.Exclamation)
            Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Initial", "Homepage.aspx"), MsgBoxStyle.Exclamation)
        End If
        cmdAdd.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Catalogue", "AddCatalogue.aspx", "cattype=" & ViewState("cattype") & "&mode=add&pageid=" & strPageId) & "')")
        If dtgCatalogue.Items.Count > 0 Then
            cmdViewCon.Enabled = True
        Else
            cmdViewCon.Enabled = False
        End If

    End Sub

    Private Function Bindgrid() As String
        Dim objCat As New ContCat
        Dim ds As New DataSet

        ds = objCat.getContractCatalogueList(txtVendor.Text, "C", ViewState("cattype"), txtCode.Text, txtDesc.Text, Session("CompanyIdToken"), txtDateFr.Text, txtDateTo.Text)
        intPageRecordCnt = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        ''//these only needed if you can select a grid item and click delete button
        ''//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        ''//then user delete one record. //total record = 20 (2 pages), 
        ''//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If ViewState("action") = "del" Then
            If dtgCatalogue.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgCatalogue.PageSize = 0 Then
                dtgCatalogue.CurrentPageIndex = dtgCatalogue.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        If intPageRecordCnt > 0 Then
            '' check when user re-enter search criteria and click on other page without click search button
            'If dtgCatalogue.CurrentPageIndex > (dvViewSample.Count \ dtgCatalogue.PageSize) Then
            '    dtgCatalogue.CurrentPageIndex = IIf((dvViewSample.Count \ dtgCatalogue.PageSize) = 1, 0, (dvViewSample.Count \ dtgCatalogue.PageSize))
            'ElseIf dtgCatalogue.CurrentPageIndex = (dvViewSample.Count \ dtgCatalogue.PageSize) Then
            '    If viewstate("PageCount") = (dvViewSample.Count \ dtgCatalogue.PageSize) Then
            '        'user does not re-enter search criteria 
            '        dtgCatalogue.CurrentPageIndex = IIf((dvViewSample.Count \ dtgCatalogue.PageSize) = 0, 0, (dvViewSample.Count \ dtgCatalogue.PageSize) - 1)
            '    Else
            '        If (dvViewSample.Count Mod dtgCatalogue.PageSize) = 0 Then
            '            ' total record = 10, 20, ...
            '            dtgCatalogue.CurrentPageIndex = IIf((dvViewSample.Count \ dtgCatalogue.PageSize) = 1, 0, (dvViewSample.Count \ dtgCatalogue.PageSize))
            '        Else
            '            ' total record = 11, 12, ...
            '            dtgCatalogue.CurrentPageIndex = (dvViewSample.Count \ dtgCatalogue.PageSize)
            '        End If
            '    End If
            'End If
            '--------------------------------

            cmdModify.Enabled = True
            cmdDelete.Enabled = True
            cmdViewCon.Enabled = True
            resetDatagridPageIndex(dtgCatalogue, dvViewSample)
            dtgCatalogue.DataSource = dvViewSample
            dtgCatalogue.DataBind()
        Else
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            cmdViewCon.Enabled = False
            Common.NetMsgbox(Me, MsgNoRecord)
            dtgCatalogue.DataBind()
        End If
        ' add for above checking
        ViewState("PageCount") = dtgCatalogue.PageCount
        objCat = Nothing
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgCatalogue.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgCatalogue_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemCreated
        Grid_ItemCreated(dtgCatalogue, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If

        If ViewState("cattype") = "D" Then
            dtgCatalogue.Columns(EnumCat.icCode).HeaderText = "Discount Group Code"
        Else
            dtgCatalogue.Columns(EnumCat.icCode).HeaderText = "Contract Ref. No."
        End If
    End Sub

    Private Sub dtgCatalogue_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim lnkCode As HyperLink
            lnkCode = e.Item.FindControl("lnkCode")
            lnkCode.Text = dv("CDM_GROUP_CODE")

            Dim lblIndex As Label
            lblIndex = e.Item.FindControl("lblIndex")
            lblIndex.Text = dv("CDM_GROUP_INDEX")

            e.Item.Cells(EnumCat.icStartDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, e.Item.Cells(EnumCat.icStartDate).Text)
            e.Item.Cells(EnumCat.icEndDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, e.Item.Cells(EnumCat.icEndDate).Text)

            If ViewState("cattype") = "D" Then
                e.Item.Cells(EnumCat.icCoyName).Visible = False
            Else
                'lnkCode.NavigateUrl = "HubCatalogueDetail.aspx?mode=mod&index=" & lblIndex.Text & "&cattype=" & viewstate("cattype") & "&pageid=" & strPageId
            End If
        ElseIf e.Item.ItemType = ListItemType.Header Then
            If ViewState("cattype") = "D" Then
                e.Item.Cells(EnumCat.icCoyName).Visible = False
            End If
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        ViewState("SortExpression") = "CDM_GROUP_CODE"
        ViewState("SortAscending") = "yes"
        dtgCatalogue.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgCatalogue_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCatalogue.PageIndexChanged
        dtgCatalogue.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    'Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
    '    'Response.Redirect("HubCatalogueDetail.aspx?mode=add&cattype=C&pageid=" & strPageId)
    '    Response.Redirect(dDispatcher.direct("Catalogue", "HubCatalogueDetail.aspx", "mode=add&cattype=C&pageid=" & strPageId))
    'End Sub

    'Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
    '    'Response.Redirect("HubAddCatalogue.aspx?cattype=" & viewstate("cattype") & "&mode=add&pageid=" & strPageId)
    '    'Response.Redirect()
    '    Dim strFileName As String
    '    Dim strscript As New System.Text.StringBuilder
    '    strscript.Append("<script language=""javascript"">")
    '    strFileName = dDispatcher.direct("Catalogue", "AddCatalogue.aspx", "cattype=" & ViewState("cattype") & "&mode=add&pageid=" & strPageId)
    '    strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
    '    strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "', '400px');")
    '    strscript.Append("document.getElementById('btnHidden').click();")
    '    strscript.Append("</script>")
    '    RegisterStartupScript("script3", strscript.ToString())
    'End Sub

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox

        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                Dim strFileName As String
                Dim strscript As New System.Text.StringBuilder
                strscript.Append("<script language=""javascript"">")
                strFileName = dDispatcher.direct("Catalogue", "AddCatalogue.aspx", "cattype=" & ViewState("cattype") & "&mode=mod&index=" & CType(dgItem.FindControl("lblIndex"), Label).Text & "&pageid=" & strPageId)
                'strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
                strscript.Append("PopWindow('" & strFileName & "');")
                strscript.Append("</script>")
                RegisterStartupScript("script13", strscript.ToString())
            End If
        Next
    End Sub

    'Public Sub btnHidden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHidden.Click
    '    ViewState("SortExpression") = "CDM_GROUP_CODE"
    '    ViewState("SortAscending") = "yes"
    '    dtgCatalogue.CurrentPageIndex = 0
    '    Bindgrid()
    'End Sub

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim objCat As New ContCat
        Dim chkItem As CheckBox
        'Dim strMsg As String
        Dim dtMaster As New DataTable
        dtMaster.Columns.Add("index", Type.GetType("System.Int32"))
        dtMaster.Columns.Add("CoyId", Type.GetType("System.String"))

        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                Dim dtr As DataRow
                dtr = dtMaster.NewRow()
                dtr("index") = CType(dgItem.FindControl("lblIndex"), Label).Text
                dtr("CoyId") = Session("CompanyId") 'dgItem.Cells(6).Text 
                dtMaster.Rows.Add(dtr)
            End If
        Next

        If objCat.deleteConContract(dtMaster) = WheelMsgNum.Delete Then
            Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        Else
            'strMsg = "Deletion is not allowed.""&vbCrLf&""It has outstanding PR(s)."
            'Common.NetMsgbox(Me, MsgRecordNotDelete, MsgBoxStyle.Information, "Wheel")
            Common.NetMsgbox(Me, RecordDeleteNotAllowed, MsgBoxStyle.Information)
        End If
        ViewState("action") = "del"
        Bindgrid()
    End Sub

    'Private Sub cmdCompany_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCompany.Click
    '    Dim dgItem As DataGridItem
    '    Dim chkItem As CheckBox

    '    For Each dgItem In dtgCatalogue.Items
    '        chkItem = dgItem.FindControl("chkSelection")
    '        If chkItem.Checked Then
    '            'Response.Redirect("HubCompanyAssign.aspx?caller=list&index=" & CType(dgItem.FindControl("lblIndex"), Label).Text & "&pageid=" & strPageId)
    '            Response.Redirect(dDispatcher.direct("Catalogue", "HubCompanyAssign.aspx", "caller=list&index=" & CType(dgItem.FindControl("lblIndex"), Label).Text & "&pageid=" & strPageId))
    '        End If
    '    Next
    'End Sub

    'Private Sub cmdItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdItem.Click
    '    Dim dgItem As DataGridItem
    '    Dim chkItem As CheckBox

    '    For Each dgItem In dtgCatalogue.Items
    '        chkItem = dgItem.FindControl("chkSelection")
    '        If chkItem.Checked Then
    '            'Response.Redirect("HubCatalogueDetail.aspx?cattype=" & viewstate("cattype") & "&index=" & CType(dgItem.FindControl("lblIndex"), Label).Text & "&pageid=" & strPageId)
    '            Response.Redirect(dDispatcher.direct("Catalogue", "HubCatalogueDetail.aspx", "cattype=" & ViewState("cattype") & "&index=" & CType(dgItem.FindControl("lblIndex"), Label).Text & "&pageid=" & strPageId))
    '        End If
    '    Next
    'End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_ConCat_tabs") = "<div class=""t_entity""><ul>" &
                                    "<li><div class=""space""></div></li>" &
                                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "pageid=" & strPageId) & """><span>Contract Catalogue</span></a></li>" &
                                    "<li><div class=""space""></div></li>" &
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "ContractItem.aspx", "pageid=" & strPageId) & """><span>Items Assignment</span></a></li>" &
                                    "<li><div class=""space""></div></li>" &
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "ConCat_AssignUser.aspx", "pageid=" & strPageId) & """><span>Users Assignment</span></a></li>" &
                                    "<li><div class=""space""></div></li>" &
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "ConCatBatchUploadDownload.aspx", "pageid=" & strPageId) & """><span>Batch Upload/Download</span></a></li>" &
                                    "<li><div class=""space""></div></li>" &
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Catalogue", "AuditTrail.aspx", "pageid=" & strPageId) & """><span>Audit</span></a></li>" &
                                    "<li><div class=""space""></div></li>" &
                                    "</ul><div></div></div>"

    End Sub
    Private Sub cmdViewCon_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdViewCon.Click
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox

        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                Dim strFileName As String
                Dim strscript As New System.Text.StringBuilder
                strscript.Append("<script language=""javascript"">")
                strFileName = dDispatcher.direct("Catalogue", "ContractItemList.aspx", "index=" & CType(dgItem.FindControl("lblIndex"), Label).Text & "&pageid=" & strPageId)
                'strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
                strscript.Append("PopWindow('" & strFileName & "');")
                strscript.Append("</script>")
                RegisterStartupScript("script13", strscript.ToString())
            End If
        Next
    End Sub

End Class