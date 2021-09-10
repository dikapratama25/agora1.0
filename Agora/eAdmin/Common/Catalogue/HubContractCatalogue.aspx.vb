Imports AgoraLegacy

Public Class HubContractCatalogue
    Inherits AgoraLegacy.AppBaseClass

    Public Enum EnumCat
        icItemCode = 0
        icVendorItemCode = 1
        icItemDesc = 2
        icCurrency = 3
        icPrice = 4
        icUOM = 5
        icRemark = 6
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblDesc As System.Web.UI.WebControls.Label
    Protected WithEvents lblStartDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblEndDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendor As System.Web.UI.WebControls.Label
    Protected WithEvents dtgCatalogue As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdApprove As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReject As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents trApprove As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents hidDelete As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblBuyer As System.Web.UI.WebControls.Label
    Protected WithEvents lblRejCnt As System.Web.UI.WebControls.Label
    Protected WithEvents trApprove2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
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
        SetGridProperty(dtgCatalogue)

        If Not IsPostBack Then
            viewstate("status") = Request.QueryString("status")
            viewstate("code") = Request.QueryString("code")
            viewstate("rej") = Request.QueryString("rej")
            viewstate("index") = Request.QueryString("index")
            lblTitle.Text = "Contract Group Items"
            'lnkBack.NavigateUrl = "HubCatalogueApproval.aspx?pageid=" & strPageId
            lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "HubCatalogueApproval.aspx", "pageid=" & strPageId)

            Select Case viewstate("status")
                Case "4" ' Pending Approval From Hub Admin
                    trApprove.Visible = True
                    trApprove2.Visible = True
                Case "5" ' Resubmission
                    trApprove.Visible = False
                    trApprove2.Visible = False
            End Select

            displayMaster()
            Bindgrid()
        End If
        cmdReject.Attributes.Add("onClick", "return confirmReject(0,1);")
        cmdApprove.Attributes.Add("onClick", "return resetSummary(0,1);")
        cmdSave.Attributes.Add("onClick", "return resetSummary(0,1);")
    End Sub

    Private Function displayMaster()
        Dim objCat As New ContCat
        Dim ds As New DataSet
        ds = objCat.getSingleContractCatalogue(viewstate("code"), "H")

        If ds.Tables(0).Rows.Count > 0 Then
            lblCode.Text = viewstate("code")
            lblDesc.Text = Common.parseNull(ds.Tables(0).Rows(0)("CDUM_Contract_Desc"))
            lblStartDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, ds.Tables(0).Rows(0)("CDUM_Start_Date"))
            lblEndDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, ds.Tables(0).Rows(0)("CDUM_End_Date"))
            viewstate("index") = ds.Tables(0).Rows(0)("CDUM_Upload_Index")
            lblVendor.Text = Common.parseNull(ds.Tables(0).Rows(0)("VendorName"))
            lblBuyer.Text = Common.parseNull(ds.Tables(0).Rows(0)("BuyerName"))
            lblRejCnt.Text = ds.Tables(0).Rows(0)("CDUM_H_Reject_Cnt")
        End If
    End Function

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objCat As New ContCat
        Dim ds As New DataSet
        ds = objCat.getContractCatalogueItemList(viewstate("index"), "S")
        intPageRecordCnt = ds.Tables(0).Rows.Count
        viewstate("intPageRecordCnt") = intPageRecordCnt

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            dtgCatalogue.DataSource = dvViewSample
            hidControl.Value = ""
            hidSummary.Value = ""
            dtgCatalogue.DataBind()
        Else
            Common.NetMsgbox(Me, MsgNoRecord)
            dtgCatalogue.DataBind()
        End If
    End Function

    Private Sub dtgCatalogue_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgCatalogue, e)
    End Sub

    Private Sub dtgCatalogue_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim hidCode As HtmlInputHidden
            hidCode = e.Item.FindControl("hidCode")
            hidCode.Value = dv("CDUI_Product_Code")

            Dim lnkCode As HyperLink
            lnkCode = e.Item.FindControl("lnkCode")
            lnkCode.Text = dv("CDUI_Product_Code")
            ' ai chu modified on 13/10/2005
            ' item retrieved from CONT_DIST_UPLOADITEMS
            'lnkCode.Attributes.Add("onclick", "window.open('ProductDetail.aspx?companyType=V&pid=" & lnkCode.Text & "&index=" & viewstate("index") & "&draft=1',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');")
            lnkCode.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("Catalogue", "ProductDetail.aspx", "companyType=V&pid=" & lnkCode.Text & "&index=" & ViewState("index") & "&draft=1',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');"))
            lnkCode.NavigateUrl = "#"

            e.Item.Cells(EnumCat.icPrice).Text = Format(CDbl(e.Item.Cells(EnumCat.icPrice).Text), "###,###,##0.0000")

            Dim lblRemark As Label
            lblRemark = e.Item.FindControl("lblRemark")
            lblRemark.Text = Common.parseNull(dv("CDUI_Remark"))

            Dim txtRemark As TextBox
            txtRemark = e.Item.FindControl("txtRemark")
            txtRemark.Attributes.Add("onKeyDown", "limitText (this, 400);")


            Dim lnkRemark As New ImageButton
            lnkRemark = e.Item.FindControl("lnkRemark")
            'lnkRemark.ImageUrl = "../images/i_remarks.gif"
            lnkRemark.ImageUrl = dDispatcher.direct("Plugins/Images", "i_remarks.gif")
            lnkRemark.CausesValidation = False
            lnkRemark.ToolTip = "View Remarks History"
            'lnkRemark.Attributes.Add("onclick", "window.open('viewRemark.aspx?index=" & viewstate("index") & "&code=" & viewstate("code") & "&pid=" & lnkCode.Text & "',  '', 'resizable=no,scrollbars=yes,width=600,height=400,status=no,menubar=no');")
            lnkRemark.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("Catalogue", "viewRemark.aspx", "index=" & ViewState("index") & "&code=" & ViewState("code") & "&pid=" & lnkCode.Text & "',  '', 'resizable=no,scrollbars=yes,width=600,height=400,status=no,menubar=no');"))

            If lblRemark.Text = "" Then
                lnkRemark.Visible = False
            Else
                lnkRemark.Visible = True
            End If

            If viewstate("status") = "4" Then
                txtRemark.Text = Common.parseNull(dv("Remark2"))
                txtRemark.Visible = True
                lblRemark.Visible = False
            Else
                txtRemark.Visible = False
            End If

            ' for '?' purpose
            Dim txtQ As TextBox
            txtQ = e.Item.FindControl("txtQ")

            If hidSummary.Value = "" Then
                hidSummary.Value = "Remarks-" & txtRemark.ClientID
            Else
                hidSummary.Value &= ",Remarks-" & txtRemark.ClientID
            End If

            If hidControl.Value = "" Then
                hidControl.Value = hidCode.ClientID & "-" & txtQ.ClientID
            Else
                hidControl.Value &= "," & hidCode.ClientID & "-" & txtQ.ClientID
            End If
        End If
    End Sub

    Private Sub dtgCatalogue_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCatalogue.PageIndexChanged
        dtgCatalogue.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgCatalogue.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub cmdApprove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApprove.Click
        updateContractStatusByHub(CatalogueStatus.Approved)
    End Sub

    Private Function updateContractStatusByHub(ByVal strStatus As String)
        Dim objCat As New ContCat
        Dim dtRemark As New DataTable
        dtRemark.Columns.Add("index", Type.GetType("System.Int32"))
        dtRemark.Columns.Add("ProductCode", Type.GetType("System.String"))
        dtRemark.Columns.Add("Remark", Type.GetType("System.String"))
        dtRemark.Columns.Add("UserRole", Type.GetType("System.String"))

        Dim dtr As DataRow
        Dim dgItem As DataGridItem
        For Each dgItem In dtgCatalogue.Items
            dtr = dtRemark.NewRow
            dtr("index") = viewstate("index")
            dtr("ProductCode") = CType(dgItem.FindControl("lnkCode"), HyperLink).Text
            dtr("UserRole") = "H"

            Dim txtRemark As TextBox
            txtRemark = dgItem.FindControl("txtRemark")
            dtr("Remark") = txtRemark.Text
            dtRemark.Rows.Add(dtr)
        Next

        objCat.updateContractStatusBy(strStatus, viewstate("code"), dtRemark, Session("CompanyIdToken"), viewstate("rej") = "0", viewstate("rej") <> "0")
        If strStatus = "5" Then ' rejected or discarded
            If viewstate("rej") = "0" Then
                'Response.Redirect("HubCatalogueConfirm.aspx?act=0&code=" & viewstate("code") & "&pageid=" & strPageId)
                Response.Redirect(dDispatcher.direct("Catalogue", "HubCatalogueConfirm.aspx", "act=0&code=" & ViewState("code") & "&pageid=" & strPageId))
            Else
                'Response.Redirect("HubCatalogueConfirm.aspx?act=2&code=" & ViewState("code") & "&pageid=" & strPageId)
                Response.Redirect(dDispatcher.direct("Catalogue", "HubCatalogueConfirm.aspx", "act=2&code=" & ViewState("code") & "&pageid=" & strPageId))
            End If
        Else ' approve
            'Response.Redirect("HubCatalogueConfirm.aspx?act=1&code=" & viewstate("code") & "&pageid=" & strPageId)
            Response.Redirect(dDispatcher.direct("Catalogue", "HubCatalogueConfirm.aspx", "act=1&code=" & ViewState("code") & "&pageid=" & strPageId))
        End If

        'Common.NetMsgbox(Me, MsgRecordSave, "HubCatalogueApproval.aspx?pageid=" & strPageId, MsgBoxStyle.Information, "Wheel")
    End Function

    Private Sub cmdReject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReject.Click
        updateContractStatusByHub(CatalogueStatus.Rejected)
    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objCat As New ContCat
        Dim dtRemark As New DataTable
        dtRemark.Columns.Add("index", Type.GetType("System.Int32"))
        dtRemark.Columns.Add("ProductCode", Type.GetType("System.String"))
        dtRemark.Columns.Add("Remark", Type.GetType("System.String"))

        Dim dtr As DataRow
        Dim dgItem As DataGridItem
        For Each dgItem In dtgCatalogue.Items
            dtr = dtRemark.NewRow
            dtr("index") = viewstate("index")
            dtr("ProductCode") = CType(dgItem.FindControl("lnkCode"), HyperLink).Text

            Dim txtRemark As TextBox
            txtRemark = dgItem.FindControl("txtRemark")
            dtr("Remark") = txtRemark.Text
            dtRemark.Rows.Add(dtr)
        Next

        objCat.updateContractCatalogueItemRemark(dtRemark)
        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
    End Sub
End Class
