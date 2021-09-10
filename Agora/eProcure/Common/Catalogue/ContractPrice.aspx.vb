Imports AgoraLegacy
Imports eProcure.Component

Public Class ContractPrice
    Inherits AgoraLegacy.AppBaseClass
    Dim strIndexList As String
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Public Enum EnumCat
        icItemCode = 0
        icVendorItemCode = 1
        icItemDesc = 2
        icPrice = 3
        icContractPrice = 4
        icRemark = 5
        icGST = 6
        icUOM = 7
        icCurrency = 8
        icPrice2 = 9
        icRemark2 = 10
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblCode As System.Web.UI.WebControls.Label
    Protected WithEvents dtgCatalogue As System.Web.UI.WebControls.DataGrid
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDesc As System.Web.UI.WebControls.Label
    Protected WithEvents lblStartDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblEndDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblBuyer As System.Web.UI.WebControls.Label
    Protected WithEvents lblCodeLabel As System.Web.UI.WebControls.Label
    Protected WithEvents trBuyer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden

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
        alButtonList.Add(cmdSave)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdSave)
        htPageAccess.Add("update", alButtonList)
        CheckButtonAccess()
        cmdSave.Enabled = (blnCanAdd Or blnCanUpdate) And viewstate("blnCmdSave")
        cmdReset.Disabled = Not (blnCanAdd Or blnCanUpdate)
        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCatalogue)

        If Not IsPostBack Then
            viewstate("blnCmdSave") = True
            viewstate("type") = Request.QueryString("type")
            viewstate("mode") = Request.QueryString("mode")
            viewstate("index") = Request.QueryString("index")
            viewstate("code") = Request.QueryString("code")
            lblCode.Text = viewstate("code")
            viewstate("strIndexList") = Session("ContractItemCodeList")

            If viewstate("type") = "DC" Then
                lblTitle.Text = "Maintain Discount Price"
                lblHeader.Text = "Discount Group Header"
                lblCodeLabel.Text = "Discount Group Code"
                trBuyer.Visible = False
            Else
                lblTitle.Text = "Maintain Contract Price"
                lblHeader.Text = "Contract Group Header"
                lblCodeLabel.Text = "Contract Ref. No."
            End If

            displayMaster()
            Bindgrid()

            If viewstate("mode") = "add" Then
                'lnkBack.NavigateUrl = "AddContractItem.aspx?type=" & viewstate("type") & "&index=" & viewstate("index") & "&code=" & viewstate("code") & "&pageid=" & strPageId
                lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "AddContractItem.aspx", "type=" & ViewState("type") & "&index=" & ViewState("index") & "&code=" & ViewState("code") & "&pageid=" & strPageId)
            Else
                'lnkBack.NavigateUrl = "ContractCatalogue.aspx?type=" & ViewState("type") & "&mode=mod&code=" & ViewState("code") & "&comp=V&index=" & ViewState("index") & "&pageid=" & strPageId
                lnkBack.NavigateUrl = dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "type=" & ViewState("type") & "&mode=mod&code=" & ViewState("code") & "&comp=V&index=" & ViewState("index") & "&pageid=" & strPageId)
            End If
        End If
        'cmdSave.Attributes.Add("onClick", "return resetSummaryDG('txtRemark','hidCode','txtQ',400);")
        cmdSave.Attributes.Add("onClick", "return resetSummary(0,1);")
    End Sub

    Private Sub displayMaster()
        Dim objCat As New ContCat
        Dim dsCat As New DataSet
        If viewstate("type") = "DC" Then
            dsCat = objCat.getSingleCatalogue(viewstate("index"), "D")
        Else
            dsCat = objCat.getSingleContractCatalogue(viewstate("code"), "V")
        End If

        If dsCat.Tables(0).Rows.Count > 0 Then
            If viewstate("type") = "DC" Then
                viewstate("code") = Common.parseNull(dsCat.Tables(0).Rows(0)("CDM_GROUP_CODE"))
                lblCode.Text = Common.parseNull(dsCat.Tables(0).Rows(0)("CDM_GROUP_CODE"))
                lblDesc.Text = Common.parseNull(dsCat.Tables(0).Rows(0)("CDM_GROUP_DESC"))
                lblStartDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dsCat.Tables(0).Rows(0)("CDM_START_DATE"))
                lblEndDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dsCat.Tables(0).Rows(0)("CDM_END_DATE"))
            Else
                viewstate("index") = dsCat.Tables(0).Rows(0)("CDUM_Upload_Index")
                lblCode.Text = Common.parseNull(dsCat.Tables(0).Rows(0)("CDUM_Contract_Code"))
                lblDesc.Text = Common.parseNull(dsCat.Tables(0).Rows(0)("CDUM_Contract_Desc"))
                lblStartDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dsCat.Tables(0).Rows(0)("CDUM_Start_Date"))
                lblEndDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dsCat.Tables(0).Rows(0)("CDUM_End_Date"))
                lblBuyer.Text = Common.parseNull(dsCat.Tables(0).Rows(0)("CM_COY_NAME"))
            End If
        End If
        objCat = Nothing
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objCat As New ContCat
        Dim ds As New DataSet
        Dim strAryList() As String

        If Not viewstate("strIndexList") Is Nothing Then
            strAryList = CStr(viewstate("strIndexList").Substring(0, viewstate("strIndexList").Length - 1)).Split(",")
        End If

        If viewstate("mode") = "mod" And viewstate("type") = "DC" Then
            ds = objCat.getContractPriceItem("cat", strAryList, viewstate("index"))
        Else
            ds = objCat.getContractPriceItem(viewstate("mode"), strAryList, viewstate("index"))
        End If

        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count
        intPageRecordCnt = viewstate("intPageRecordCnt")

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        ''//datagrid.pageCount only got value after databind
        If intPageRecordCnt > 0 Then
            dtgCatalogue.DataSource = dvViewSample
            hidControl.Value = ""
            hidSummary.Value = ""
            dtgCatalogue.DataBind()
        End If
        objCat = Nothing
    End Function

    Private Function bindCatalogueItem() As DataTable
        Dim dtItem As New DataTable
        Dim strPrice As String
        Dim strRemark As String

        dtItem.Columns.Add("index", Type.GetType("System.Int32"))
        dtItem.Columns.Add("ProductCode", Type.GetType("System.String"))
        dtItem.Columns.Add("VendorItemCode", Type.GetType("System.String"))
        dtItem.Columns.Add("ProductDesc", Type.GetType("System.String"))
        dtItem.Columns.Add("UOM", Type.GetType("System.String"))
        dtItem.Columns.Add("UnitCost", Type.GetType("System.Double"))
        dtItem.Columns.Add("Remark", Type.GetType("System.String"))
        dtItem.Columns.Add("GST", Type.GetType("System.Double"))
        dtItem.Columns.Add("CurrencyCode", Type.GetType("System.String"))
        dtItem.Columns.Add("UserRole", Type.GetType("System.String"))
        dtItem.Columns.Add("Change", Type.GetType("System.Int32"))

        Dim dtr As DataRow
        Dim dgItem As DataGridItem
        For Each dgItem In dtgCatalogue.Items
            dtr = dtItem.NewRow
            dtr("index") = viewstate("index")
            dtr("ProductCode") = CType(dgItem.FindControl("lnkCode"), HyperLink).Text
            dtr("VendorItemCode") = dgItem.Cells(1).Text
            dtr("ProductDesc") = dgItem.Cells(2).Text
            dtr("GST") = dgItem.Cells(6).Text
            dtr("UOM") = dgItem.Cells(7).Text
            dtr("CurrencyCode") = dgItem.Cells(8).Text
            dtr("UserRole") = "V"

            Dim txtPrice As TextBox
            txtPrice = dgItem.FindControl("txtPrice")
            dtr("UnitCost") = txtPrice.Text

            Dim hidPrice As HtmlInputHidden
            hidPrice = dgItem.FindControl("hidPrice")

            Dim txtRemark As TextBox
            txtRemark = dgItem.FindControl("txtRemark")
            dtr("Remark") = txtRemark.Text

            If IsNumeric(hidPrice.Value) Then
                If CDbl(hidPrice.Value) = CDbl(txtPrice.Text) Then
                    dtr("Change") = 0
                Else
                    dtr("Change") = 1
                End If
            End If

            strPrice = dgItem.Cells(EnumCat.icPrice2).Text
            strRemark = dgItem.Cells(EnumCat.icRemark2).Text

            If txtPrice.Text <> strPrice Or txtRemark.Text <> strRemark Then
                dtItem.Rows.Add(dtr)
            End If
        Next

        bindCatalogueItem = dtItem
    End Function

    Private Sub dtgCatalogue_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgCatalogue, e)

        If viewstate("type") = "DC" Then
            dtgCatalogue.Columns(EnumCat.icContractPrice).HeaderText = "Discount Price<font color=red>*</font>"
        Else
            dtgCatalogue.Columns(EnumCat.icContractPrice).HeaderText = "Contract Price<font color=red>*</font>"
        End If
    End Sub

    Public Sub dtgCatalogue_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgCatalogue.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub dtgCatalogue_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkCode As HyperLink
            lnkCode = e.Item.FindControl("lnkCode")
            lnkCode.Text = dv("PRODUCT_CODE")

            ' ai chu modified on 13/10/2005
            ' if mode = add, item retrieved from Product_Mstr table
            ' else retrieved from CONT_DIST_UPLOADITEMS or CONTRACT_DIST_ITEMS
            If viewstate("mode") = "add" Then
                'lnkCode.Attributes.Add("onclick", "window.open('../Product/ProductDetail.aspx?companyType=V&pid=" & lnkCode.Text & "&index=&draft=0',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');")
                lnkCode.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=V&pid=" & lnkCode.Text & "&index=&draft=0',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');"))
            Else
                If ViewState("type") = "DC" Then
                    'lnkCode.Attributes.Add("onclick", "window.open('../Product/ProductDetail.aspx?companyType=V&pid=" & lnkCode.Text & "&index=" & ViewState("index") & "&draft=0',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');")
                    lnkCode.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=V&pid=" & lnkCode.Text & "&index=" & ViewState("index") & "&draft=0',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');"))
                Else
                    'lnkCode.Attributes.Add("onclick", "window.open('../Product/ProductDetail.aspx?companyType=V&pid=" & lnkCode.Text & "&index=" & ViewState("index") & "&draft=1',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');")
                    lnkCode.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=V&pid=" & lnkCode.Text & "&index=" & ViewState("index") & "&draft=1',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');"))
                End If
            End If

            lnkCode.NavigateUrl = "#"

            Dim hidCode As HtmlInputHidden
            hidCode = e.Item.FindControl("hidCode")
            hidCode.Value = dv("PRODUCT_CODE")

            If Not IsDBNull(dv("UNIT_COST")) Then
                e.Item.Cells(EnumCat.icPrice).Text = Format(CDbl(dv("UNIT_COST")), "###,###,##0.0000")
            Else
                e.Item.Cells(EnumCat.icPrice).Text = "0.0000"
            End If

            If IsDBNull(dv("GST")) Then
                e.Item.Cells(EnumCat.icGST).Text = "0.00"
            Else
                e.Item.Cells(EnumCat.icGST).Text = dv("GST")
            End If

            Dim txtPrice As TextBox
            txtPrice = e.Item.Cells(EnumCat.icContractPrice).FindControl("txtPrice")
            Dim hidPrice As HtmlInputHidden
            hidPrice = e.Item.FindControl("hidPrice")

            If Not IsDBNull(dv("CONTRACT_PRICE")) Then
                txtPrice.Text = Format(CDbl(dv("CONTRACT_PRICE")), "#0.0000")
            Else
                txtPrice.Text = "0.0000"
            End If

            If viewstate("mode") = "add" Then
                hidPrice.Value = "0.0000"
            Else
                If Not IsDBNull(dv("CONTRACT_PRICE")) Then
                    hidPrice.Value = Format(CDbl(dv("CONTRACT_PRICE")), "#0.0000")
                Else
                    hidPrice.Value = "0.0000"
                End If
            End If

            Dim revPrice As RegularExpressionValidator
            revPrice = e.Item.FindControl("revPrice")
            revPrice.ValidationExpression = "(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,4})?$"
            revPrice.ControlToValidate = "txtPrice"
            If viewstate("type") = "DC" Then
                revPrice.ErrorMessage = lnkCode.Text & ". Discount Price is over limit/expecting numeric value."
            Else
                revPrice.ErrorMessage = lnkCode.Text & ". Contract Price is over limit/expecting numeric value."
            End If

            revPrice.Text = "?"
            revPrice.Display = ValidatorDisplay.Dynamic

            Dim txtRemark As TextBox
            txtRemark = e.Item.Cells(EnumCat.icRemark).FindControl("txtRemark")
            txtRemark.Text = Common.parseNull(dv("REMARK"))
            txtRemark.Attributes.Add("onKeyDown", "limitText (this, 400);")

            Dim lnkRemark As New ImageButton
            lnkRemark = e.Item.FindControl("lnkRemark")
            'lnkRemark.ImageUrl = "../images/i_remarks.gif"
            lnkRemark.ImageUrl = dDispatcher.direct("Plugins/images", "i_remarks.gif")
            lnkRemark.CausesValidation = False
            lnkRemark.ToolTip = "View Remarks History"
            'lnkRemark.Attributes.Add("onclick", "window.open('viewRemark.aspx?index=" & viewstate("index") & "&code=" & viewstate("code") & "&pid=" & lnkCode.Text & "',  '', 'resizable=no,scrollbars=yes,width=600,height=400,status=no,menubar=no');")
            lnkRemark.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("Catalogue", "viewRemark.aspx", "index=" & ViewState("index") & "&code=" & ViewState("code") & "&pid=" & lnkCode.Text & "',  '', 'resizable=no,scrollbars=yes,width=600,height=400,status=no,menubar=no');"))
            If viewstate("type") <> "D" And viewstate("type") <> "DC" Then
                lnkRemark.Visible = True
            Else
                lnkRemark.Visible = False
            End If

            If Common.parseNull(dv("REMARK2")) = "" Then
                If Common.parseNull(dv("REMARK")) = "" Then
                    lnkRemark.Visible = False
                End If
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

            'To validate txtprice when user press on delete & space infront the value
            txtPrice.Attributes.Add("onBlur", "resetValue('" & txtPrice.ClientID & "','0.0000')")
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim dtItem As New DataTable
        dtItem = bindCatalogueItem()

        Dim objCat As New ContCat
        Select Case viewstate("mode")
            Case "add"
                If viewstate("type") = "DC" Then
                    objCat.insertCatalogueItem(dtItem, Session("CompanyId"))
                Else
                    objCat.insertContractCatalogueItem(dtItem)
                End If

            Case "mod"
                If viewstate("type") = "DC" Then
                    objCat.updateCatalogueItem(dtItem, Session("CompanyId"))
                Else
                    objCat.updateContractCatalogueItem(dtItem)
                End If

        End Select
        'Common.NetMsgbox(Me, MsgRecordSave, "ContractCatalogue.aspx?comp=V&mode=mod&type=" & viewstate("type") & "&code=" & viewstate("code") & "&index=" & viewstate("index") & "&pageid=" & strPageId, MsgBoxStyle.Information)
        Common.NetMsgbox(Me, MsgRecordSave, dDispatcher.direct("Catalogue", "ContractCatalogue.aspx", "comp=V&mode=mod&type=" & ViewState("type") & "&code=" & ViewState("code") & "&index=" & ViewState("index") & "&pageid=" & strPageId), MsgBoxStyle.Information)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgCatalogue.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgCatalogue_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCatalogue.PageIndexChanged
        dtgCatalogue.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    'Private Sub cmdReset_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.ServerClick
    '    Dim dgItem As DataGridItem
    '    For Each dgItem In dtgCatalogue.Items
    '        Dim revPrice As RegularExpressionValidator
    '        revPrice = dgItem.FindControl("revPrice")
    '        revPrice.IsValid = True
    '    Next
    'End Sub
End Class
