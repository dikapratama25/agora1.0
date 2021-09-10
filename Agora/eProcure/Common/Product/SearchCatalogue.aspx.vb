'''''''''''''''''''''''''''''''''''''''''''''''''''
' Author          : Moo
' Description     : To provide catalogue search - List Price, Contract Price and Discount Price                   
' Remark          : Can add items to shopping cart, buyer catalogue,favourite list and RFQ draft
' Created Date    : 23/10/2004
' Called          : 1. Direct Search Catalogue Link  2. From Buyer Catalogue/Favourite List/RFQ Draft 
'                   3. Link at the Search Result(Contract/Discount/List)
' Modification History:
'
'   Author          Date            Reason      Comment
'   ---------       ---------       --------    ---------
'
'''''''''''''''''''''''''''''''''''''''''''''''''''
Imports AgoraLegacy
Imports eProcure.Component

Public Class SearchCatalogue
    Inherits AgoraLegacy.AppBaseClass
  

    Dim intTotRecord As Integer
    Dim intTotPage As Integer
    Dim strRole As String
    Dim strType As String
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cboSearchType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtSearchVal As System.Web.UI.WebControls.TextBox
    Protected WithEvents cboAppVendor As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents lnkAdv As System.Web.UI.WebControls.HyperLink
    Protected WithEvents txtProdId As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtProdModel As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtProdDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVendor As System.Web.UI.WebControls.TextBox
    Protected WithEvents cboAppVendor1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtProdCat As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVIC As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtProdBrand As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBIC As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdAdvSearch As System.Web.UI.WebControls.Button
    Protected WithEvents lnkNormal As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkCont As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkDist As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkList As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkAll As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblCont As System.Web.UI.WebControls.Label
    Protected WithEvents lblDist As System.Web.UI.WebControls.Label
    Protected WithEvents lblList As System.Web.UI.WebControls.Label
    Protected WithEvents lblAll As System.Web.UI.WebControls.Label
    Protected WithEvents dtgProduct As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cboFavourite As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmdAddFav As System.Web.UI.WebControls.Button
    Protected WithEvents cboBuyCat As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmdAddBuyC As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAddSC As System.Web.UI.WebControls.Button
    Protected WithEvents divNormal As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents divAdv As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmd_Adv_Clear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents trLegend As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tdBFHeader As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tdFavList As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tdBuyerCat As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents txtRole As System.Web.UI.HtmlControls.HtmlInputHidden
    Dim strGrpInx As String
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Public Enum EnumProduct
        icChkBox
        icCat
        icQty
        icProdCode
        icBItemCode
        icSCoyName
        icVItemCode
        icBrand
        icModel
        icCategory
        icDesc
        icUOM
        icCostWtC
        icCost
        icSCoyId
        icCurrency
        icGrpIndex
    End Enum
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
        SetGridProperty(dtgProduct)
        Dim objUser As New Users

        If objUser.IsPurchasing Then
            'tdBuyerCat.Visible = True
            'tdBuyerCat.Style("display") = ""
            strRole = "PM"
        Else
            'tdBuyerCat.Visible = False
            'tdBuyerCat.Style("display") = "none"
            strRole = "BUYER"
        End If
        txtRole.Value = strRole
        objUser = Nothing
        If Request.QueryString("search") <> String.Empty Then
            Session("search") = Request.QueryString("search")
        End If

        If Session("search") = "adv" Then
            divAdv.Style("display") = "inline"
            divNormal.Style("display") = "none"
            Session("search") = "adv"
            lblTitle.Text = "Catalogue Advance Search"
            lnkNormal.NavigateUrl = dDispatcher.direct("Product", "SearchCatalogue.aspx", "search=normal&pageid=" & strPageId)
        Else
            divAdv.Style("display") = "none"
            divNormal.Style("display") = "inline"
            Session("search") = "normal"
            lblTitle.Text = "Catalogue Search"
            lnkAdv.NavigateUrl = dDispatcher.direct("Product", "SearchCatalogue.aspx", "search=adv&pageid=" & strPageId)
        End If

        cmdAddFav.Attributes.Add("onclick", "return reCheckAtLeastOne('chkSelection','');")
        cmdAddBuyC.Attributes.Add("onclick", "return reCheckAtLeastOne('chkSelection','');")
        cmdAddSC.Attributes.Add("onclick", "return reCheckAtLeastOne('chkSelection','');")
        'cmdAddFav.Attributes.Add("onClick", "return checkAtLeastOneResetSummary('chkSelection','',0,0);")
        'cmdAddBuyC.Attributes.Add("onClick", "return checkAtLeastOneResetSummary('chkSelection','',0,0);")
        'cmdAddSC.Attributes.Add("onClick", "return checkAtLeastOneResetSummary('chkSelection','',0,0);")
        'cboSearchType.Attributes.Add("onclick", "Test();")

        If Not Page.IsPostBack Then
            '//because No DataGrid display when page first loaded
            cmdReset.Disabled = True
            '//add List Item
            '//Cannot add at HTML mode because user selection will lost after post back
            cboSearchType.Items.Clear()
            addListItem("Item ID", "PI")
            addListItem("Item Description", "PD")
            addListItem("Item Category", "PC")
            addListItem("Item Brand", "PB")
            addListItem("Item Model", "PM")
            addListItem("Vendor", "VE")
            addListItem("Vendor Item Code", "VI")
            addListItem("Buyer Item Code", "BI")


            '// click on the Link on top of the search result
            If Request.QueryString("Cat") <> String.Empty Then
                '//for normal search
                Common.SelDdl(Request.QueryString("ST"), cboSearchType, True, True)
                cboSearchType_IndexChg()
                If Session("search") <> "adv" Then
                    If Request.QueryString("ST") = "VE" Then
                        If strRole <> "PM" Then
                            'txtSearchVal.Text = Request.QueryString("SV")
                            Common.SelDdl(Request.QueryString("SV"), cboAppVendor)
                        Else
                            txtSearchVal.Text = Request.QueryString("SV")
                        End If
                    Else
                        txtSearchVal.Text = Request.QueryString("SV")
                    End If
                Else
                    '//for advance search
                    cboSearchType_IndexChg()
                    Dim strSVal() As String
                    strSVal = Session("SearchVal")
                    txtProdId.Text = strSVal(0)
                    txtProdDesc.Text = strSVal(1)
                    txtProdCat.Text = strSVal(2)
                    txtProdBrand.Text = strSVal(3)
                    txtProdModel.Text = strSVal(4)
                    'txtVendor.Text = strSVal(5)
                    If strRole <> "PM" Then
                        'txtSearchVal.Text = Request.QueryString("SV")
                        Common.SelDdl(strSVal(5), cboAppVendor1)
                    Else
                        txtVendor.Text = strSVal(5)
                    End If
                    txtVIC.Text = strSVal(6)
                    txtBIC.Text = strSVal(7)
                End If
                '//end
                ViewState("Cat") = Request.QueryString("Cat")
                dtgProduct.CurrentPageIndex = 0
                Bindgrid()
            Else
                cboSearchType_IndexChg()
            End If
            'BCM_GRP_DESC 
            '//try to disable button when no listitem
            'objProduct.getFavList().Count
            Dim objBuyerCat As New BuyerCat
            Dim dv As DataView
            dv = objBuyerCat.Favlist_bindFav
            If Not dv Is Nothing Then
                Common.FillDdl(cboFavourite, "NAME", "FLM_LIST_INDEX", dv)
                cmdAddFav.Enabled = True
            Else
                cmdAddFav.Enabled = False
            End If
            cboFavourite.Items.Insert(0, "---Select---")

            If strRole = "PM" Then
                Dim objProduct As New Products
                dv = objProduct.getBuyerCat
                objProduct = Nothing
                If Not dv Is Nothing Then
                    Common.FillDdl(cboBuyCat, "BCM_GRP_DESC", "BCM_CAT_INDEX", dv)
                    cmdAddBuyC.Enabled = True
                Else
                    cmdAddBuyC.Enabled = False
                End If
                cboBuyCat.Items.Insert(0, "---Select---")
            End If
            objBuyerCat = Nothing

            '//to cater for redirection from buyer cat
            '//because of "contract item","list item" is shown as hyperlink, it was not consider as postback
            '//to keep pass in "buyer cat id", need to append to those hyperlink as querystring
            '//but if user change the buyer cat dropdown list, user selected value will always be override
            '//by query string, ViewState("ignore") is set in cboBuyCat.onIndexSelectedChange
            If Request.QueryString("BCIdx") <> "" Then
                Common.SelDdl(Request.QueryString("BCIdx"), cboBuyCat)
            End If

            If Request.QueryString("FVIdx") <> "" Then
                Common.SelDdl(Request.QueryString("FVIdx"), cboFavourite)
            End If
        End If
        lnkAdv.NavigateUrl = dDispatcher.direct("Product", "SearchCatalogue.aspx", "search=adv")
        lnkNormal.NavigateUrl = dDispatcher.direct("Product", "SearchCatalogue.aspx", "search=normal")
    End Sub

    Sub dtgProduct_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgProduct.PageIndexChanged
        dtgProduct.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgProduct.SortCommand
        Grid_SortCommand(sender, e)
        dtgProduct.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String


        Dim ds, dsFP, dsCP, dsDP As DataSet
        Dim intFPCnt, intCPCnt, intDPCnt As Int32
        Dim strURL, strSearchVal(), strSearchType As String
        '//Retrieve All Catalogue when search button is click to get Items count
        '//Retrieve only catalogue in ViewState("Cat") when result link clicked to reduce data retrieval process
        If Session("search") = "adv" Then
            ReDim strSearchVal(7)
            'ReDim strSearchValTemp(7)
            strSearchVal(0) = txtProdId.Text
            strSearchVal(1) = txtProdDesc.Text
            strSearchVal(2) = txtProdCat.Text
            strSearchVal(3) = txtProdBrand.Text
            strSearchVal(4) = txtProdModel.Text
            If strRole <> "PM" Then
                strSearchVal(5) = cboAppVendor1.SelectedItem.Value
            Else
                strSearchVal(5) = txtVendor.Text
            End If

            strSearchVal(6) = txtVIC.Text
            strSearchVal(7) = txtBIC.Text
            'strSearchVal.CopyTo(strSearchValTemp, 0)
            'strSearchValTemp = strSearchVal
            Session("SearchVal") = strSearchVal
            strSearchType = ""
        Else
            ReDim strSearchVal(0)
            'ReDim strSearchValTemp(0)
            'strSearchVal(0) = txtSearchVal.Text
            strSearchType = cboSearchType.SelectedItem.Value

            If strSearchType = "VE" And strRole <> "PM" Then
                strSearchVal(0) = cboAppVendor.SelectedItem.Value
            Else
                strSearchVal(0) = txtSearchVal.Text
            End If

            'strSearchValTemp = strSearchVal
            'strSearchVal.CopyTo(strSearchValTemp, 0)
            'strSearchValTemp(0) = strSearchVal(0)
        End If
        Dim objProduct As New Products
        If ViewState("Cat") = String.Empty Or ViewState("Cat") = "ALL" Then
            dsFP = objProduct.getProduct("FP", strSearchType, strSearchVal, intFPCnt, Session("search"))
            dsDP = objProduct.getProduct("DP", strSearchType, strSearchVal, intDPCnt, Session("search"))
            dsCP = objProduct.getProduct("CP", strSearchType, strSearchVal, intCPCnt, Session("search"))
            Session("FPCnt") = intFPCnt
            Session("DPCnt") = intDPCnt
            Session("CPCnt") = intCPCnt
        Else
            Select Case ViewState("Cat")
                Case "CP"
                    dsCP = objProduct.getProduct("CP", strSearchType, strSearchVal, intCPCnt, Session("search"))
                    intFPCnt = Convert.ToInt32(Session("FPCnt"))
                    intDPCnt = Convert.ToInt32(Session("DPCnt"))
                Case "DP"
                    dsDP = objProduct.getProduct("DP", strSearchType, strSearchVal, intDPCnt, Session("search"))
                    intFPCnt = Convert.ToInt32(Session("FPCnt"))
                    intCPCnt = Convert.ToInt32(Session("CPCnt"))
                Case "FP"
                    dsFP = objProduct.getProduct("FP", strSearchType, strSearchVal, intFPCnt, Session("search"))
                    intCPCnt = Convert.ToInt32(Session("CPCnt"))
                    intDPCnt = Convert.ToInt32(Session("DPCnt"))
            End Select
        End If
        '//End Retrieval
        objProduct = Nothing
        '//Set URL if records return
        '//query string --> type and id is for buyer cat
        strURL = dDispatcher.direct("Product", "SearchCatalogue.aspx", "pageid=" & strPageId & "&ST=" & cboSearchType.SelectedItem.Value & "&SV=" & strSearchVal(0)) 'txtSearchVal.Text '& "&FVIdx=" & cboFavourite.SelectedValue        

        If strRole = "PM" Then
            strURL = strURL & "&BCIdx=" & cboBuyCat.SelectedValue
        End If

        If intCPCnt > 0 Then
            If ViewState("Cat") = String.Empty Then ViewState("Cat") = "CP"
            lnkCont.NavigateUrl = strURL & "&Cat=CP"
        Else
            lnkCont.NavigateUrl = ""
        End If

        If intDPCnt > 0 Then
            If ViewState("Cat") = String.Empty Then ViewState("Cat") = "DP"
            lnkDist.NavigateUrl = strURL & "&Cat=DP"
        Else
            lnkDist.NavigateUrl = ""
        End If

        If intFPCnt > 0 Then
            If ViewState("Cat") = String.Empty Then ViewState("Cat") = "FP"
            lnkList.NavigateUrl = strURL & "&Cat=FP"
        Else
            lnkList.NavigateUrl = ""
        End If

        If intCPCnt > 0 Or intDPCnt > 0 Or intFPCnt > 0 Then
            If ViewState("Cat") = String.Empty Then ViewState("Cat") = "ALL"
            lnkAll.NavigateUrl = strURL & "&Cat=ALL"
        Else
            lnkAll.NavigateUrl = ""
        End If
        '//End - Set URL if records return

        '//set Total Record return
        '//(-) to indicate no allow to view/buy items
        If intCPCnt = -1 Then
            lblCont.Text = "(-) Items"
        Else
            lblCont.Text = "(" & intCPCnt & ") Items"
        End If

        If intDPCnt = -1 Then
            lblDist.Text = "(-) Items"
        Else
            lblDist.Text = "(" & intDPCnt & ") Items"
        End If

        If intFPCnt = -1 Then
            lblList.Text = "(-) Items"
        Else
            lblList.Text = "(" & intFPCnt & ") Items"
        End If

        Dim intTotItem As Integer
        If intCPCnt >= 0 Or intDPCnt >= 0 Or intFPCnt >= 0 Then
            If intCPCnt > 0 Then intTotItem += intCPCnt
            If intDPCnt > 0 Then intTotItem += intDPCnt
            If intFPCnt > 0 Then intTotItem += intFPCnt
            lblAll.Text = "(" & intTotItem & ") Items"
        Else
            lblAll.Text = "(-) Items"
        End If
        '//set Total Record return

        '//to select which catalogue to display
        '//Seq - Contract, Discount then Fixed
        '//Mean=> If got Contract, display contract else display discount or Fixed
        If ViewState("Cat") = "CP" Then
            ds = dsCP
            ChangeURLText("CP")
            dtgProduct.Columns(EnumProduct.icCat).Visible = False
            trLegend.Style("display") = "none"
        ElseIf ViewState("Cat") = "DP" Then
            ds = dsDP
            ChangeURLText("DP")
            dtgProduct.Columns(EnumProduct.icCat).Visible = False
            trLegend.Style("display") = "none"
        ElseIf ViewState("Cat") = "FP" Then
            ds = dsFP
            ChangeURLText("FP")
            dtgProduct.Columns(EnumProduct.icCat).Visible = False
            trLegend.Style("display") = "none"
        Else
            If Not dsCP Is Nothing Then
                ds = dsCP
                If Not dsDP Is Nothing Then
                    ds.Merge(dsDP)
                End If
                If Not dsFP Is Nothing Then
                    ds.Merge(dsFP)
                End If
            ElseIf Not dsDP Is Nothing Then
                ds = dsDP
                If Not dsFP Is Nothing Then
                    ds.Merge(dsFP)
                End If
            ElseIf Not dsFP Is Nothing Then
                ds = dsFP
            End If
            ChangeURLText("ALL")
            dtgProduct.Columns(EnumProduct.icCat).Visible = True
            trLegend.Style("display") = ""
        End If

        '//for sorting asc or desc
        Dim dvViewDept As DataView

        If Not ds Is Nothing Then
            dvViewDept = ds.Tables(0).DefaultView
            'If pSorted Then
            '//set default sort field
            If viewstate("SortExpression") Is Nothing Then
                viewstate("SortExpression") = "PM_PRODUCT_CODE"
                viewstate("SortAscending") = "yes"
            End If

            dvViewDept.Sort = viewstate("SortExpression")
            If viewstate("SortAscending") = "no" Then dvViewDept.Sort += " DESC"

            intTotRecord = ds.Tables(0).Rows.Count
            intPageRecordCnt = intTotRecord
            viewstate("intPageRecordCnt") = intPageRecordCnt
        Else
            intTotRecord = 0
        End If

        '//bind datagrid

        If intTotRecord > 0 Then
            resetDatagridPageIndex(dtgProduct, dvViewDept)

            'intTotPage = dtgProduct.PageCount
            '//mean Enable, can't use button.Enabled because this is a HTML button
            cmdReset.Disabled = False
            tblSearchResult.Style("display") = ""
            dtgProduct.DataSource = dvViewDept
            tdFavList.Style("display") = ""
            'tdBuyerCat.Style("display") = "none"
            If strRole = "PM" Then
                tdBuyerCat.Style("display") = ""
            Else
                tdBuyerCat.Style("display") = "none"
            End If

            tdBFHeader.Style("display") = ""
            cmdAddSC.Enabled = True
            dtgProduct.DataBind()
        Else
            'dtgProduct.DataSource = ""
            tblSearchResult.Style("display") = ""
            tdFavList.Style("display") = "none"
            tdBuyerCat.Style("display") = "none"
            tdBFHeader.Style("display") = "none"
            cmdReset.Disabled = True
            cmdAddSC.Enabled = False
            'cmdAddBuyC.Enabled = False
            'cmdAddFav.Enabled = False
            dtgProduct.DataBind()
            'Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ' add for above checking
        viewstate("PageCount") = dtgProduct.PageCount
    End Function

    Private Sub addListItem(ByVal pText, ByVal pVal)
        Dim lstItem As New ListItem
        lstItem.Value = pVal
        lstItem.Text = pText
        cboSearchType.Items.Add(lstItem)
    End Sub

    Private Sub ChangeURLText(ByVal strCatType As String)
        If strCatType = "CP" Then
            lnkCont.Text = "<B>Contract Price Items</B>"
            lnkCont.NavigateUrl = ""
            lnkDist.Text = "[Discount Price Items]"
            lnkList.Text = "[List Price Items ]"
            lnkAll.Text = "[All]"
        ElseIf strCatType = "DP" Then
            lnkDist.NavigateUrl = ""
            lnkCont.Text = "[Contract Price Items]"
            lnkDist.Text = "<B>Discount Price Items</B>"
            lnkList.Text = "[List Price Items]"
            lnkAll.Text = "[All]"
        ElseIf strCatType = "FP" Then
            lnkList.NavigateUrl = ""
            lnkCont.Text = "[Contract Price Items]"
            lnkDist.Text = "[Discount Price Items]"
            lnkList.Text = "<B>List Price Items</B>"
            lnkAll.Text = "[All]"
        Else
            lnkCont.Text = "[Contract Price Items]"
            lnkDist.Text = "[Discount Price Items]"
            lnkList.Text = "[List Price Items]"
            lnkAll.Text = "<B>All</B>"
            lnkAll.NavigateUrl = ""
        End If

    End Sub
    Private Sub dtgProduct_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgProduct.ItemCreated
        '//this line must be included
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgProduct, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub


    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgProduct.CurrentPageIndex = 0
        '//reset Catalogue Type if Search button clicked
        ViewState("Cat") = ""
        Bindgrid()
    End Sub
    Private Sub dtgProduct_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgProduct.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            '//to dynamic build HyperLink
            Dim lnkProductCode As HyperLink
            lnkProductCode = e.Item.Cells(EnumProduct.icProdCode).FindControl("lnkProductCode")
            lnkProductCode.NavigateUrl = "javascript:;"

            ' ai chu modified on 13/10/2005
            Select Case UCase(Common.parseNull(dv("CAT")))
                Case "CP", "DP" ' retrieved from CONTRACT_DIST_ITEMS table
                    lnkProductCode.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=B&pageid=" & strPageId & "&pid=" & dv("PM_PRODUCT_CODE") & "&index=" & Common.parseNull(dv("CDM_GROUP_INDEX")) & "&draft=0") & "')")
                Case Else ' retrieved from Product_Mstr table
                    lnkProductCode.Attributes.Add("Product", "return PopWindow('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=B&pageid=" & strPageId & "&pid=" & dv("PM_PRODUCT_CODE") & "&index=&draft=0") & "')")
            End Select

            lnkProductCode.Text = dv("PM_PRODUCT_CODE")
            Dim lblCost As Label = e.Item.Cells(EnumProduct.icCostWtC).FindControl("lblCost")
            If IsDBNull(dv("PM_UNIT_COST")) Then
                lblCost.Text = "N/A"
                e.Item.Cells(EnumProduct.icCost).Text = 0
            Else
                lblCost.Text = dv("PM_CURRENCY_CODE") & " " & Format$(dv("PM_UNIT_COST"), "#,##0.0000")
            End If

            If Common.parseNull(dv("PM_PRODUCT_BRAND")) = "" Then
                e.Item.Cells(EnumProduct.icBrand).Text = "-"
            End If
            If Common.parseNull(dv("PM_PRODUCT_MODEL")) = "" Then
                e.Item.Cells(EnumProduct.icModel).Text = "-"
            End If

            ' ai chu add
            e.Item.Cells(EnumProduct.icSCoyName).Text = "<a href='" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "pageid=" & strPageId & "&v_com_id=" & Common.parseNull(dv("CM_COY_ID"))) & "'>" & e.Item.Cells(EnumProduct.icSCoyName).Text & "</a>"

            Dim txtQty As TextBox
            txtQty = e.Item.FindControl("txtQty")
            txtQty.Text = ""

            Dim revQty As RegularExpressionValidator
            revQty = e.Item.FindControl("revQty")
            revQty.ValidationExpression = "(?!^0*$)^\d{1,5}?$" '"\d{1,5}" '"^\d+$" ' "^\d+\.+\d+$|^\d+$"
            revQty.ControlToValidate = "txtQty"
            revQty.ErrorMessage = "Invalid quantity"
            revQty.Display = ValidatorDisplay.Dynamic

            'e.Item.Cells(1).Te = "<a href='addDept.aspx'>" & e.Item.Cells(1).Text & "</a>"
        End If
    End Sub  
    Private Sub cmdAddFav_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddFav.Click
        Dim dtProduct As DataTable
        If cboFavourite.SelectedIndex > 0 Then
            dtProduct = ParseDataGrid()
            Dim objProduct As New Products
            objProduct.addToFavList(cboFavourite.SelectedItem.Value, dtProduct)
            objProduct = Nothing
            'Common.NetMsgbox(Me, "")
            Session("dt") = dtProduct
            Response.Redirect(dDispatcher.direct("Product", "DisplayAddedItem.aspx", "pageid=" & strPageId & "&type=F&id=" & cboFavourite.SelectedItem.Value))
            'Dim dr As DataRow
            'For Each dr In dtProduct.Rows
            '    Response.Write(dr(0) & "-" & dr(1) & "-" & dr(2) & "<br>")
            'Next
        Else
            Common.NetMsgbox(Me, "Please select a Favourite List.")
        End If
    End Sub

    Private Sub cmdAddBuyC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddBuyC.Click
        Dim dtProduct As DataTable
        Dim strTemp(), strBuyerCatCode, strBuyerCatName As String
        If cboBuyCat.SelectedIndex > 0 Then
            dtProduct = ParseDataGrid()

            ' ai chu add on 12/09/2005
            If checkSameItem(dtProduct) Then
                Common.NetMsgbox(Me, "Cannot select same item for same buyer catalogue.")
                Exit Sub
            End If
            Dim objProduct As New Products
            objProduct.addToBuyerCat(cboBuyCat.SelectedItem.Value, dtProduct)
            objProduct = Nothing
            'Common.NetMsgbox(Me, "")
            Session("dt") = dtProduct
            strTemp = Split(cboBuyCat.SelectedItem.Text, ":")
            strBuyerCatCode = strTemp(0).Trim
            strBuyerCatName = strTemp(1).Trim
            Response.Redirect(dDispatcher.direct("Product", "DisplayAddedItem.aspx", "pageid=" & strPageId & "&type=B&id=" & cboBuyCat.SelectedItem.Value & "&code=" & Server.UrlEncode(strBuyerCatCode) & "&name=" & Server.UrlEncode(strBuyerCatName)))
        Else
            Common.NetMsgbox(Me, "Please select a Buyer Catalogue.")
        End If
    End Sub

    ' ai chu add on 12/09/2005
    ' same item cannot be added into same buyer catalogue
    ' checking for buyer catalogue only
    Private Function checkSameItem(ByVal dt As DataTable) As Boolean
        Dim i, j As Integer
        Dim strItemCode As String = ""

        For i = 0 To dt.Rows.Count - 1
            If strItemCode.IndexOf(dt.Rows(i)("PRODUCT_CODE")) = -1 Then ' item not exist in current selection
                strItemCode &= dt.Rows(i)("PRODUCT_CODE") & ","
            Else
                Return True
            End If
        Next
        Return False
    End Function

    Private Function ParseDataGrid() As DataTable
        Dim dtProduct As New DataTable
        Dim dr As DataRow
        dtProduct.Columns.Add("PRODUCT_CODE", Type.GetType("System.String")) '//product code
        dtProduct.Columns.Add("S_COY_ID", Type.GetType("System.String")) '//supplier id
        dtProduct.Columns.Add("VENDOR_ITEM_CODE", Type.GetType("System.String")) '//Vendor ITem Code
        dtProduct.Columns.Add("SC_UNIT_COST", Type.GetType("System.Double")) '//unit cost
        dtProduct.Columns.Add("GRP_INDEX", Type.GetType("System.Int32")) '//group index
        dtProduct.Columns.Add("CAT_TYPE", Type.GetType("System.String")) '//unit cost
        dtProduct.Columns.Add("msg", Type.GetType("System.String"))  '//error msg

        Dim grdItem As DataGridItem
        For Each grdItem In dtgProduct.Items
            Dim chkSel As CheckBox = grdItem.Cells(EnumProduct.icChkBox).FindControl("chkSelection")
            If chkSel.Checked Then
                dr = dtProduct.NewRow
                dr("PRODUCT_CODE") = dtgProduct.DataKeys(grdItem.ItemIndex)
                dr("SC_UNIT_COST") = IIf(grdItem.Cells(EnumProduct.icCost).Text = "0", -1, grdItem.Cells(EnumProduct.icCost).Text)
                If grdItem.Cells(EnumProduct.icCat).Text <> "LP" Then
                    dr("GRP_INDEX") = grdItem.Cells(EnumProduct.icGrpIndex).Text 'IIf(grdItem.Cells(EnumProduct.icGrpIndex).Text = "0", -1, grdItem.Cells(EnumProduct.icGrpIndex).Text)
                Else
                    dr("GRP_INDEX") = -1
                End If
                dr("CAT_TYPE") = grdItem.Cells(EnumProduct.icCat).Text
                dr("VENDOR_ITEM_CODE") = grdItem.Cells(EnumProduct.icVItemCode).Text
                dr("S_COY_ID") = grdItem.Cells(EnumProduct.icSCoyId).Text
                dtProduct.Rows.Add(dr)
            End If
        Next
        Return dtProduct
    End Function

    Private Sub cmdAddSC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddSC.Click
        Dim dtProduct As DataTable
        Dim objProduct As New Products
        dtProduct = objProduct.getSCSchema()    
        dtProduct.Columns.Add("GRP_INDEX", Type.GetType("System.Int32")) '//group index
        dtProduct.Columns.Add("CAT_TYPE", Type.GetType("System.String"))  '//LP,CP,DP
        dtProduct.Columns.Add("msg", Type.GetType("System.String"))  '//error msg
        Dim dr As DataRow
        Dim grdItem As DataGridItem
        For Each grdItem In dtgProduct.Items
            Dim chkSel As CheckBox = grdItem.Cells(0).FindControl("chkSelection")
            If chkSel.Checked Then
                'SC_S_COY_ID,SC_PRODUCT_CODE,SC_VENDOR_ITEM_CODE,SC_PRODUCT_DESC,SC_UNIT_COST, SC_QUANTITY, SC_CURRENCY_CODE, SC_UOM   
                dr = dtProduct.NewRow
                dr("S_COY_ID") = grdItem.Cells(EnumProduct.icSCoyId).Text
                dr("PRODUCT_CODE") = dtgProduct.DataKeys(grdItem.ItemIndex)
                dr("VENDOR_ITEM_CODE") = grdItem.Cells(EnumProduct.icVItemCode).Text
                dr("SC_PRODUCT_DESC") = grdItem.Cells(EnumProduct.icDesc).Text
                dr("SC_UNIT_COST") = IIf(grdItem.Cells(EnumProduct.icCost).Text = "0", -1, grdItem.Cells(EnumProduct.icCost).Text)
                dr("SC_QUANTITY") = IIf(CType(grdItem.Cells(EnumProduct.icQty).FindControl("txtQty"), TextBox).Text = "", -1, CType(grdItem.Cells(EnumProduct.icQty).FindControl("txtQty"), TextBox).Text)
                dr("SC_CURRENCY_CODE") = grdItem.Cells(EnumProduct.icCurrency).Text
                dr("SC_UOM") = grdItem.Cells(EnumProduct.icUOM).Text

                '//modify by Moo
                If grdItem.Cells(EnumProduct.icCat).Text <> "LP" Then
                    dr("GRP_INDEX") = grdItem.Cells(EnumProduct.icGrpIndex).Text 'IIf(grdItem.Cells(EnumProduct.icGrpIndex).Text = "0", -1, grdItem.Cells(EnumProduct.icGrpIndex).Text)
                Else
                    dr("GRP_INDEX") = -1
                End If
                dr("CAT_TYPE") = grdItem.Cells(EnumProduct.icCat).Text
                dtProduct.Rows.Add(dr)
            End If
        Next
        ' ai chu add on 12/09/2005
        If checkSameItem(dtProduct) Then
            Common.NetMsgbox(Me, "Cannot select same item.")
            Exit Sub
        End If

        objProduct.addToShopCart(0, dtProduct)
        objProduct = Nothing
        Session("dt") = dtProduct
        Response.Redirect(dDispatcher.direct("Product", "DisplayAddedItem.aspx", "pageid=" & strPageId & "&type=SC"))

    End Sub


    Private Sub cmdAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdvSearch.Click
        dtgProduct.CurrentPageIndex = 0
        '//reset Catalogue Type if Search button clicked
        ViewState("Cat") = ""
        Bindgrid()
    End Sub

    'Private Sub cmdAdvClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdvClear.Click
    '    txtProdId.Text = ""
    '    txtProdDesc.Text = ""
    '    txtProdCat.Text = ""
    '    txtProdBrand.Text = ""
    '    txtProdModel.Text = ""
    '    txtVendor.Text = ""
    '    txtVIC.Text = ""
    '    txtBIC.Text = ""
    'End Sub
    Sub cboSearchType_IndexChg()

        Dim objAdmin As New Admin
        Dim lstItem As New ListItem
        Dim ds As DataSet
        If Not Page.IsPostBack Then
            ds = objAdmin.searchvendor("AV", "", "")
        End If

        If Session("search") = "adv" Then
            'txtVendor.Visible = True
            txtVendor.Style("display") = "inline"
            'cboAppVendor1.Visible = False
            cboAppVendor1.Style("display") = "none"
            If Not Page.IsPostBack Then
                cboAppVendor1.Items.Clear()
                Common.FillDdl(cboAppVendor1, "CM_COY_NAME", "CV_S_COY_ID", ds.Tables(0).DefaultView)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                cboAppVendor1.Items.Insert(0, lstItem)
            End If
        Else
            'txtSearchVal.Visible = True
            txtSearchVal.Style("display") = "inline"
            'cboAppVendor.Visible = False
            cboAppVendor.Style("display") = "none"
            If Not Page.IsPostBack Then
                cboAppVendor.Items.Clear()
                Common.FillDdl(cboAppVendor, "CM_COY_NAME", "CV_S_COY_ID", ds.Tables(0).DefaultView)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                cboAppVendor.Items.Insert(0, lstItem)
            End If
        End If

        If strRole <> "PM" Then
            '//vendor
            If Session("search") = "adv" Then
                'txtVendor.Visible = False
                txtVendor.Style("display") = "none"
                'cboAppVendor1.Visible = True
                cboAppVendor1.Style("display") = "inline"
            Else
                If cboSearchType.SelectedValue = "VE" Then
                    'txtSearchVal.Visible = False
                    txtSearchVal.Style("display") = "none"
                    'cboAppVendor.Visible = True
                    cboAppVendor.Style("display") = "inline"
                End If
            End If
        End If
    End Sub
    Private Sub cboSearchType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSearchType.SelectedIndexChanged
        cboSearchType_IndexChg()
    End Sub

   
End Class


