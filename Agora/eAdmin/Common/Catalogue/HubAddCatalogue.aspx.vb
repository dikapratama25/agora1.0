Imports AgoraLegacy

Public Class HubAddCatalogue
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents txtCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents revCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents revDesc As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtStartDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents revStartDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents txtEndDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents trCode As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDate As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trCodeRead As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDateRead As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents hidDelete As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents cboBuyer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents revBuyer As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents trBuyer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents revEndDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents lblEndDateMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblCodeLabel As System.Web.UI.WebControls.Label
    Protected WithEvents cmdItem As System.Web.UI.WebControls.Button
    Protected WithEvents cmdCompany As System.Web.UI.WebControls.Button
    Protected WithEvents cvDateNow As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblClear As System.Web.UI.WebControls.Label
    Dim dDispatcher = New dispatcher

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

        If Not IsPostBack Then
            viewstate("mode") = Request.QueryString("mode")
            viewstate("cattype") = Request.QueryString("cattype")
            cvDateNow.ValueToCompare = Date.Today.ToShortDateString
            Dim objCat As New ContCat

            Select Case viewstate("cattype")
                Case "C"
                    cmdCompany.Visible = False
                    cmdItem.Text = "Contract Group Item"
                    lblTitle.Text = "Contract Group"
                    lblCodeLabel.Text = "Contract Ref. No."
                    revCode.ErrorMessage = "Contract Ref. No. is required"
                    Dim dsCat As New DataSet
                    dsCat = objCat.getBuyerCompany(Session("CompanyIdToken"))
                    Common.FillDdl(cboBuyer, "CM_COY_NAME", "CM_COY_ID", dsCat)

                    Dim lstItem As New ListItem
                    lstItem.Value = ""
                    lstItem.Text = "---Select---"
                    cboBuyer.Items.Insert(0, lstItem)
                    lblClear.Visible = False

                Case "D"
                    cmdItem.Text = "Discount Group Item"
                    lblCodeLabel.Text = "Discount Group Code"
                    revCode.ErrorMessage = "Discount Group Code is required"
                    trBuyer.Visible = False
                    revEndDate.Enabled = False
                    revBuyer.Enabled = False
                    lblTitle.Text = "Discount Group"
                    lblEndDateMsg.Visible = False
                    'lblClear.Text = "<A onclick='return clearEndDate()'><IMG style='CURSOR: hand' src='../Images/i_calendar_clear.gif' align='absBottom' alt='Clear End Date'></A>"
                    lblClear.Text = "<A onclick='return clearEndDate()'><IMG style='CURSOR: hand' src='" & dDispatcher.direct("Plugins/Images", "i_calendar_clear.gif") & "' align='absBottom' alt='Clear End Date'></A>"
            End Select

            If viewstate("mode") = "mod" Then
                Select Case viewstate("cattype")
                    Case "D"
                        lblHeader.Text = "Modify Discount Group"
                    Case "C"
                        lblHeader.Text = "Modify Contract Group"
                End Select

                viewstate("index") = Request.QueryString("index")
                displayMaster()
                cmdReset.Value = "Reset"
            Else
                Select Case viewstate("cattype")
                    Case "D"
                        lblHeader.Text = "Add Discount Group"
                    Case "C"
                        lblHeader.Text = "Add Contract Group"
                End Select

                cmdReset.Value = "Clear"
                cmdDelete.Visible = False
                cmdCompany.Visible = False
                cmdItem.Visible = False
            End If
        End If
        'cmdReset.Attributes.Add("onclick", "return resetForm();")
        'lnkBack.NavigateUrl = "HubCatalogueList.aspx?cattype=" & viewstate("cattype") & "&pageid=" & strPageId
        lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "HubCatalogueList.aspx", "cattype=" & ViewState("cattype") & "&pageid=" & strPageId)
        cmdDelete.Attributes.Add("onclick", "return confirm('" & MsgForDeleteButton & "');")
    End Sub

    Private Function displayMaster()
        Dim objCat As New ContCat
        Dim ds As New DataSet
        ds = objCat.getSingleCatalogue(viewstate("index"), viewstate("cattype"))

        If ds.Tables(0).Rows.Count > 0 Then
            viewstate("oldCode") = Common.parseNull(ds.Tables(0).Rows(0)("CDM_GROUP_CODE"))
            txtCode.Text = Common.parseNull(ds.Tables(0).Rows(0)("CDM_GROUP_CODE"))
            txtDesc.Text = Common.parseNull(ds.Tables(0).Rows(0)("CDM_GROUP_DESC"))
            txtStartDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds.Tables(0).Rows(0)("CDM_START_DATE"))
            txtEndDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, ds.Tables(0).Rows(0)("CDM_END_DATE"))
            If viewstate("cattype") = "C" Then
                Common.SelDdl(Common.parseNull(ds.Tables(0).Rows(0)("CDC_B_COY_ID")), cboBuyer, True, True)
            End If
        End If
    End Function

    Private Function saveCatalogueMaster() As Integer
        Dim dtMaster As New DataTable
        dtMaster.Columns.Add("index", Type.GetType("System.Int32"))
        dtMaster.Columns.Add("GroupCode", Type.GetType("System.String"))
        dtMaster.Columns.Add("GroupDesc", Type.GetType("System.String"))
        dtMaster.Columns.Add("VendorID", Type.GetType("System.String"))
        dtMaster.Columns.Add("CatType", Type.GetType("System.String"))
        dtMaster.Columns.Add("StartDate", Type.GetType("System.String"))
        dtMaster.Columns.Add("EndDate", Type.GetType("System.String"))
        If viewstate("cattype") = "C" Then
            dtMaster.Columns.Add("BuyerID", Type.GetType("System.String"))
        End If

        Dim dtr As DataRow
        dtr = dtMaster.NewRow()
        If Not viewstate("index") Is Nothing Then
            dtr("index") = viewstate("index")
        Else
            dtr("index") = 0
        End If

        dtr("GroupCode") = txtCode.Text
        dtr("GroupDesc") = txtDesc.Text
        dtr("CatType") = viewstate("cattype")
        dtr("VendorID") = HttpContext.Current.Session("CompanyIdToken")
        dtr("StartDate") = txtStartDate.Text
        dtr("EndDate") = txtEndDate.Text
        If viewstate("cattype") = "C" Then
            dtr("BuyerID") = cboBuyer.SelectedItem.Value
        End If
        dtMaster.Rows.Add(dtr)

        Dim objCat As New ContCat
        Dim intMsg As Integer
        Select Case viewstate("mode")
            Case "add"
                intMsg = objCat.insertCatalogue(dtMaster)
                viewstate("index") = objCat.getLatestIndex("DC")
            Case "mod"
                intMsg = objCat.updateCatalogue(dtMaster, viewstate("oldCode"))
        End Select
        saveCatalogueMaster = intMsg
    End Function

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim intMsg As Integer
        intMsg = saveCatalogueMaster()
        Select Case intMsg
            Case WheelMsgNum.Save
                cmdItem.Visible = True
                viewstate("mode") = "mod"
                If viewstate("cattype") = "D" Then
                    cmdCompany.Visible = True
                End If
                Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
            Case WheelMsgNum.NotSave
                Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
            Case WheelMsgNum.Duplicate
                Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Information)
        End Select
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim objCat As New ContCat
        Dim dtMaster As New DataTable
        dtMaster.Columns.Add("index", Type.GetType("System.Int32"))
        dtMaster.Columns.Add("CoyId", Type.GetType("System.String"))

        Dim dtr As DataRow
        dtr = dtMaster.NewRow()
        dtr("index") = viewstate("index")
        dtr("CoyId") = Session("CompanyIdToken")
        dtMaster.Rows.Add(dtr)

        If objCat.deleteContract(dtMaster, Session("CompanyIdToken")) = WheelMsgNum.Delete Then
            'Common.NetMsgbox(Me, MsgRecordDelete, "HubCatalogueList.aspx?cattype=" & viewstate("cattype") & "&pageid=" & strPageId, MsgBoxStyle.Information)
            Common.NetMsgbox(Me, MsgRecordDelete, dDispatcher.direct("Catalogue", "HubCatalogueList.aspx", "cattype=" & ViewState("cattype") & "&pageid=" & strPageId), MsgBoxStyle.Information)
        Else
            Common.NetMsgbox(Me, RecordDeleteNotAllowed, MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub cmdCompany_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCompany.Click
        'Response.Redirect("HubCompanyAssign.aspx?index=" & ViewState("index") & "&caller=cat&pageid=" & strPageId)
        Response.Redirect(dDispatcher.direct("Catalogue", "HubCompanyAssign.aspx", "index=" & ViewState("index") & "&caller=cat&pageid=" & strPageId))
    End Sub

    Private Sub cmdItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdItem.Click
        'Response.Redirect("HubCatalogueDetail.aspx?cattype=" & viewstate("cattype") & "&index=" & viewstate("index") & "&pageid=" & strPageId)
        Response.Redirect(dDispatcher.direct("Catalogue", "HubCatalogueDetail.aspx", "cattype=" & ViewState("cattype") & "&index=" & ViewState("index") & "&pageid=" & strPageId))
    End Sub

    'Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    revCode.IsValid = True
    '    revDesc.IsValid = True
    '    revStartDate.IsValid = True
    '    revEndDate.IsValid = True
    '    cvDate.IsValid = True
    '    cvDateNow.IsValid = True
    'End Sub
End Class
