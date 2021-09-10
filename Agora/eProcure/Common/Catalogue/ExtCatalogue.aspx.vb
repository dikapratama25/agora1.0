Imports AgoraLegacy
Imports eProcure.Component

Public Class ExtCatalogue
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
    'Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    'Protected WithEvents txtCode As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtVendor As System.Web.UI.WebControls.TextBox
    'Protected WithEvents hidVendor As System.Web.UI.WebControls.TextBox
    'Protected WithEvents revCode As System.Web.UI.WebControls.RequiredFieldValidator
    'Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    'Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    'Protected WithEvents revDesc As System.Web.UI.WebControls.RequiredFieldValidator
    'Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    'Protected WithEvents txtStartDate As System.Web.UI.WebControls.TextBox
    'Protected WithEvents revStartDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtEndDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClose As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    'Protected WithEvents hidDelete As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    'Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    'Protected WithEvents revBuyer As System.Web.UI.WebControls.RequiredFieldValidator
    'Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
    'Protected WithEvents trBuyer As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents revEndDate As System.Web.UI.WebControls.RequiredFieldValidator
    'Protected WithEvents lblEndDateMsg As System.Web.UI.WebControls.Label
    'Protected WithEvents lblCodeLabel As System.Web.UI.WebControls.Label
    'Protected WithEvents trStart As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents trEnd As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents trCode As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents trDesc As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cvDateNow As System.Web.UI.WebControls.CompareValidator
    'Protected WithEvents lblClear As System.Web.UI.WebControls.Label
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.

    Protected WithEvents dtgCatalogue As System.Web.UI.WebControls.DataGrid

    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    'Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
    '    Dim alButtonList As ArrayList
    '    alButtonList = New ArrayList
    '    alButtonList.Add(cmdSave)
    '    htPageAccess.Add("add", alButtonList)
    '    alButtonList = New ArrayList
    '    alButtonList.Add(cmdSave)
    '    htPageAccess.Add("update", alButtonList)
    '    'alButtonList = New ArrayList
    '    'alButtonList.Add(cmdDelete)
    '    'htPageAccess.Add("delete", alButtonList)
    '    CheckButtonAccess()
    '    cmdSave.Enabled = (blnCanAdd Or blnCanUpdate) And viewstate("blnCmdSave")
    '    'cmdDelete.Enabled = blnCanDelete And viewstate("blnCmdDelete")
    '    'cmdReset.Disabled = Not (blnCanAdd Or blnCanUpdate Or blnCanDelete)
    '    alButtonList.Clear()
    'End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCatalogue)

        If Not IsPostBack Then
            '' ''ViewState("mode") = Request.QueryString("mode")
            '' ''ViewState("cattype") = Request.QueryString("cattype")
            cvDateNow.ValueToCompare = Date.Today.ToShortDateString

            'Dim objCat As New ContCat

            'Select Case ViewState("cattype")
            '    Case "C"
            'cmdCompany.Visible = False
            'cmdItem.Text = "Contract Group Item"
            'lblTitle.Text = "Contract Group"
            'lblCodeLabel.Text = "Contract Ref. No."
            'revCode.ErrorMessage = "Contract Ref. No. is required"
            'Dim dsCat As New DataSet
            'dsCat = objCat.getBuyerCompany(Session("CompanyIdToken"))
            'Common.FillDdl(cboBuyer, "CM_COY_NAME", "CM_COY_ID", dsCat)

            'Dim lstItem As New ListItem
            'lstItem.Value = ""
            'lstItem.Text = "---Select---"
            'cboBuyer.Items.Insert(0, lstItem)
            'lblClear.Visible = False

            'Case "D"
            '    lblCodeLabel.Text = "Discount Group Code"
            '    revCode.ErrorMessage = "Discount Group Code is required"
            '    trBuyer.Visible = False
            '    revEndDate.Enabled = False
            '    revBuyer.Enabled = False
            '    lblTitle.Text = "Discount Group"
            '    lblEndDateMsg.Visible = False
            '    'lblClear.Text = "<A onclick='return clearEndDate()'><IMG style='CURSOR: hand' src='../Images/i_calendar_clear.gif' align='absBottom' alt='Clear End Date'></A>"
            '    lblClear.Text = "<A onclick='return clearEndDate()'><IMG style='CURSOR: hand' src='" & dDispatcher.direct("Plugins/Images", "i_calendar_clear.gif") & "' align='absBottom' alt='Clear End Date'></A>"
            'End Select

            '' ''If ViewState("mode") = "mod" Then
            '' ''    Select Case ViewState("cattype")
            '' ''        Case "D"
            '' ''            lblHeader.Text = "Modify Discount Group"
            '' ''        Case "C"
            '' ''            lblTitle.Text = "Modify Contract Catalogue"
            '' ''            lblHeader.Text = "Modify Contract"
            '' ''    End Select

            '' ''    ViewState("index") = Request.QueryString("index")
            '' ''    displayMaster()
            '' ''Else
            '' ''    Select Case ViewState("cattype")
            '' ''        Case "D"
            '' ''            lblHeader.Text = "Add Discount Group"
            '' ''        Case "C"
            '' ''            lblTitle.Text = "New Contract Catalogue"
            '' ''            lblHeader.Text = "New Contract"
            '' ''    End Select
            '' ''    Me.txtCode.Focus()
            '' ''End If

            Bindgrid()
        End If

        cmdSave.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
        'cmdReset.Attributes.Add("onclick", "return resetForm();")
        'lnkBack.NavigateUrl = "HubCatalogueList.aspx?cattype=" & viewstate("cattype") & "&pageid=" & strPageId
        'lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "HubCatalogueList.aspx", "cattype=" & ViewState("cattype") & "&pageid=" & strPageId)
        'cmdDelete.Attributes.Add("onclick", "return confirm('" & MsgForDeleteButton & "');")
    End Sub

    '' ''Private Function displayMaster()
    '' ''    Dim objCat As New ContCat
    '' ''    Dim ds As New DataSet
    '' ''    ds = objCat.getSingleConCatalogue(ViewState("index"))

    '' ''    If ds.Tables(0).Rows.Count > 0 Then
    '' ''        ViewState("oldCode") = Common.parseNull(ds.Tables(0).Rows(0)("CDM_GROUP_CODE"))
    '' ''        txtCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("CDM_GROUP_CODE"))
    '' ''        txtDesc.Text = Common.parseNull(ds.Tables(0).Rows(0)("CDM_GROUP_DESC"))
    '' ''        txtStartDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds.Tables(0).Rows(0)("CDM_START_DATE"))
    '' ''        txtEndDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds.Tables(0).Rows(0)("CDM_END_DATE"))
    '' ''        txtVendor.Text = Common.parseNull(ds.Tables(0).Rows(0)("CM_COY_NAME"))
    '' ''        hidVendor.Text = Common.parseNull(ds.Tables(0).Rows(0)("CDM_S_COY_ID"))
    '' ''        'If viewstate("cattype") = "C" Then
    '' ''        'Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("CDC_B_COY_ID")), cboBuyer, True, True)
    '' ''        'End If
    '' ''    End If
    '' ''End Function

    '' ''Private Function saveCatalogueMaster() As Integer
    '' ''    Dim dtMaster As New DataTable
    '' ''    dtMaster.Columns.Add("index", Type.GetType("System.Int32"))
    '' ''    dtMaster.Columns.Add("GroupCode", Type.GetType("System.String"))
    '' ''    dtMaster.Columns.Add("GroupDesc", Type.GetType("System.String"))
    '' ''    dtMaster.Columns.Add("VendorID", Type.GetType("System.String"))
    '' ''    dtMaster.Columns.Add("CatType", Type.GetType("System.String"))
    '' ''    dtMaster.Columns.Add("StartDate", Type.GetType("System.DateTime"))
    '' ''    dtMaster.Columns.Add("EndDate", Type.GetType("System.DateTime"))
    '' ''    'If viewstate("cattype") = "C" Then
    '' ''    dtMaster.Columns.Add("BuyerID", Type.GetType("System.String"))
    '' ''    'End If

    '' ''    Dim dtr As DataRow
    '' ''    dtr = dtMaster.NewRow()
    '' ''    If Not ViewState("index") Is Nothing Then
    '' ''        dtr("index") = ViewState("index")
    '' ''    Else
    '' ''        dtr("index") = 0
    '' ''    End If

    '' ''    dtr("GroupCode") = txtCode.Text
    '' ''    dtr("GroupDesc") = txtDesc.Text
    '' ''    dtr("CatType") = ViewState("cattype")
    '' ''    dtr("BuyerID") = HttpContext.Current.Session("CompanyId")
    '' ''    dtr("StartDate") = txtStartDate.Text
    '' ''    dtr("EndDate") = txtEndDate.Text
    '' ''    dtr("VendorID") = IIf(hidVendor.Text = "", "", hidVendor.Text) ' txtVendor.Text
    '' ''    'If viewstate("cattype") = "C" Then
    '' ''    '    dtr("BuyerID") = cboBuyer.SelectedItem.Value
    '' ''    'End If
    '' ''    dtMaster.Rows.Add(dtr)

    '' ''    Dim objCat As New ContCat
    '' ''    Dim intMsg As Integer
    '' ''    Select Case ViewState("mode")
    '' ''        Case "add"
    '' ''            intMsg = objCat.insertConCatalogue(dtMaster)
    '' ''            ViewState("index") = objCat.getLatestIndex("DC")
    '' ''        Case "mod"
    '' ''            intMsg = objCat.updateConCatalogue(dtMaster, ViewState("oldCode"))
    '' ''    End Select
    '' ''    saveCatalogueMaster = intMsg
    '' ''End Function

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox
        Dim strCat, strCheck As String
        Dim objCat As New ContCat

        For Each dgItem In dtgCatalogue.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                If strCat = "" Then
                    strCat = CType(dgItem.FindControl("lblIndex"), Label).Text
                Else
                    strCat = strCat & "," & CType(dgItem.FindControl("lblIndex"), Label).Text
                End If
            End If
        Next

        Dim ds As New DataSet

        strCat = strCat.Replace(",", "','")
        strCat = "'" & strCat & "'"

        strCheck = objCat.checkContractCatalogueList("IDX", "C", strCat, txtEndDate.Text)
        If Common.parseNull(strCheck) <> "" And Page.IsValid = True Then
            Common.NetMsgbox(Me, "Contract '" & strCheck & "' start date is greater than extended date.", MsgBoxStyle.Information)
        ElseIf Common.parseNull(strCheck) = "" And Page.IsValid = True Then

            Dim intMsg As Integer
            intMsg = objCat.saveCatalogueMasterEx("IDX", "C", strCat, txtEndDate.Text)
            Select Case intMsg
                Case WheelMsgNum.Save
                    Dim strscript As New System.Text.StringBuilder

                    strscript.Append("<script language=""javascript"">")
                    strscript.Append("ans=alert(""Record Saved."");")
                    strscript.Append("window.close(); window.opener.reloadPage();")
                    strscript.Append("</script>")
                    RegisterStartupScript("script13", strscript.ToString())

                Case WheelMsgNum.NotSave
                    Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                Case WheelMsgNum.Duplicate
                    Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Information)
            End Select
        End If
    End Sub

    '' ''Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
    '' ''    Dim intMsg As Integer
    '' ''    intMsg = saveCatalogueMaster()
    '' ''    Select Case intMsg
    '' ''        Case WheelMsgNum.Save
    '' ''            'cmdItem.Visible = True
    '' ''            ViewState("mode") = "mod"
    '' ''            If ViewState("cattype") = "D" Then
    '' ''                'cmdCompany.Visible = True
    '' ''            End If
    '' ''            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
    '' ''        Case WheelMsgNum.NotSave
    '' ''            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
    '' ''        Case WheelMsgNum.Duplicate
    '' ''            Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Information)
    '' ''    End Select
    '' ''End Sub

    'Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
    '    Dim objCat As New ContCat
    '    Dim dtMaster As New DataTable
    '    dtMaster.Columns.Add("index", Type.GetType("System.Int32"))
    '    dtMaster.Columns.Add("CoyId", Type.GetType("System.String"))

    '    Dim dtr As DataRow
    '    dtr = dtMaster.NewRow()
    '    dtr("index") = viewstate("index")
    '    dtr("CoyId") = Session("CompanyIdToken")
    '    dtMaster.Rows.Add(dtr)

    '    If objCat.deleteContract(dtMaster, Session("CompanyIdToken")) = WheelMsgNum.Delete Then
    '        'Common.NetMsgbox(Me, MsgRecordDelete, "HubCatalogueList.aspx?cattype=" & viewstate("cattype") & "&pageid=" & strPageId, MsgBoxStyle.Information)
    '        Common.NetMsgbox(Me, MsgRecordDelete, dDispatcher.direct("Catalogue", "HubCatalogueList.aspx", "cattype=" & ViewState("cattype") & "&pageid=" & strPageId), MsgBoxStyle.Information)
    '    Else
    '        Common.NetMsgbox(Me, RecordDeleteNotAllowed, MsgBoxStyle.Information)
    '    End If
    'End Sub

    'Private Sub cmdCompany_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCompany.Click
    '    'Response.Redirect("HubCompanyAssign.aspx?index=" & ViewState("index") & "&caller=cat&pageid=" & strPageId)
    '    Response.Redirect(dDispatcher.direct("Catalogue", "HubCompanyAssign.aspx", "index=" & ViewState("index") & "&caller=cat&pageid=" & strPageId))
    'End Sub

    'Private Sub cmdItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdItem.Click
    '    'Response.Redirect("HubCatalogueDetail.aspx?cattype=" & viewstate("cattype") & "&index=" & viewstate("index") & "&pageid=" & strPageId)
    '    Response.Redirect(dDispatcher.direct("Catalogue", "HubCatalogueDetail.aspx", "cattype=" & ViewState("cattype") & "&index=" & ViewState("index") & "&pageid=" & strPageId))
    'End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        'ViewState("CatalogueList") = Nothing
        Response.Write("<script language='javascript'> {window.close(); window.opener.reloadPage(); }</script>")
        'Response.Write("<script language='javascript'> {window.close(); }</script>")
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgCatalogue.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Function Bindgrid() As String
        Dim objCat As New ContCat
        Dim ds As New DataSet
        Dim strCat As String

        strCat = Request.QueryString("index").Replace(",", "','")
        strCat = "'" & strCat & "'"

        ds = objCat.getContractCatalogueList("", "IDX", "C", strCat, "", "", "", "")
        intPageRecordCnt = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgCatalogue, dvViewSample)
            dtgCatalogue.DataSource = dvViewSample
            dtgCatalogue.DataBind()
        Else
            Common.NetMsgbox(Me, MsgNoRecord)
            dtgCatalogue.DataBind()
        End If

        ViewState("PageCount") = dtgCatalogue.PageCount
        objCat = Nothing
    End Function

    Private Sub dtgCatalogue_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemCreated
        Grid_ItemCreated(dtgCatalogue, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Checked = True
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dtgCatalogue_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Checked = True
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim lnkCode As HyperLink
            lnkCode = e.Item.FindControl("lnkCode")
            lnkCode.Text = dv("CDM_GROUP_CODE")

            Dim lblIndex As Label
            lblIndex = e.Item.FindControl("lblIndex")
            lblIndex.Text = dv("CDM_GROUP_INDEX")

            e.Item.Cells(EnumCat.icStartDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, e.Item.Cells(EnumCat.icStartDate).Text)
            e.Item.Cells(EnumCat.icEndDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, e.Item.Cells(EnumCat.icEndDate).Text)

        End If
    End Sub

    Private Sub dtgCatalogue_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCatalogue.PageIndexChanged
        dtgCatalogue.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub
End Class
