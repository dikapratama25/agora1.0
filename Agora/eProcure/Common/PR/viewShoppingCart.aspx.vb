Imports AgoraLegacy
Imports eProcure.Component


Public Class viewShoppingCart
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Dim intRecord As Integer
    Dim intTotRecord As Integer
    Dim strType As String   
    Dim intTotPage As Integer
    Dim blnMultipleSupplier As Boolean
    Public strIndexList As String
    Public StrVenList As String
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents cboSupplier As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents dtgShopping As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cboPR As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmdAddPR As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Update As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Raise As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Delete As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden

    Dim objDB As New EAD.DBCom
    Public strven1 As New ArrayList

    'Dim blnCmdSave As Boolean
    'Dim blnCmdRaisePR As Boolean
    'Dim blnCmdDelete As Boolean
    'Dim blnCmdAddToPR As Boolean

    Public Enum EnumShoppingCart
        icChkBox = 0
        icBuyerItemCode = 1
        icVendorItemCode = 2
        icVendorCode = 3
        icItemDesc = 4
        icUOM = 5
        icQty = 6
        icCurrencyCode = 7
        icPrice = 8
        icTotal = 9
        icRemark = 10
        icSCoyID = 11
        icProductCode = 12
    End Enum

#Region " Web Form Designer Generated Code "


    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmd_Update)
        alButtonList.Add(cmd_Raise)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmd_Raise)
        alButtonList.Add(cmdAddPR)
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmd_Delete)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        cmd_Update.Enabled = blnCanUpdate And viewstate("blnCmdSave")
        cmd_Raise.Enabled = blnCanAdd And blnCanUpdate And viewstate("blnCmdRaisePR")
        cmd_Delete.Enabled = blnCanDelete And viewstate("blnCmdDelete")
        cmdAddPR.Enabled = blnCanAdd And blnCanUpdate And viewstate("blnCmdAddToPR")
        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim strvendor As String
        MyBase.Page_Load(sender, e)
        lblTitle.Text = "Shopping Cart"
        SetGridProperty(dtgShopping)
        dtgShopping.PageSize = 30
        If Not Page.IsPostBack Then
            viewstate("blnCmdSave") = True
            viewstate("blnCmdRaisePR") = True
            viewstate("blnCmdDelete") = True
            viewstate("blnCmdAddToPR") = True
            viewState("type") = Request.QueryString("type")
            strvendor = Request.QueryString("SC_S_COY_ID")

            If Request.QueryString("type") = "" Then
                viewState("type") = "tab"
            End If

            Dim dsVendor As New DataSet
            Dim objShopping As New ShoppingCart
            objShopping.FillSCVendor(cboSupplier)
            objShopping = Nothing

            Select Case viewstate("type")
                Case "PR"
                    Dim objPR As New PR
                    cmd_Update.Visible = False
                    cmd_Raise.Visible = False
                    cmd_Delete.Visible = False
                    viewstate("blnCmdSave") = False
                    viewstate("blnCmdRaisePR") = False
                    viewstate("blnCmdDelete") = False
                    lnkBack.Visible = True
                    viewState("Supplier") = Request.QueryString("supplier")
                    viewState("Currency") = Request.QueryString("currency")
                    viewState("prid") = Request.QueryString("prid")
                    lnkBack.NavigateUrl = dDispatcher.direct("PR", "RaisePR.aspx", "pageid=" & strPageId & "&type=list&prid=" & ViewState("prid"))
                    Common.SelDdl(viewstate("Supplier"), cboSupplier, True, True)
                    'cboSupplier.Enabled = False

                    ' if requested supplier does not exists in the shopping cart
                    If cboSupplier.SelectedItem.Value.ToLower <> CStr(viewState("Supplier")).ToLower Then
                        Dim ds As New DataSet
                        ds = objPR.getVendorDetail(viewState("Supplier"))
                        If ds.Tables(0).Rows.Count > 0 Then
                            Dim lstItem As New ListItem
                            lstItem.Value = viewstate("Supplier")
                            lstItem.Text = Common.parseNull(ds.Tables(0).Rows(0)("CM_COY_NAME"))
                            cboSupplier.Items.Add(lstItem)
                            Common.SelDdl(viewstate("Supplier"), cboSupplier, True, True)
                        End If
                    End If

                    Dim dsPR As New DataSet
                    'dsPR = objPR.PopulatePRList(PRStatus.Draft, cboSupplier.SelectedItem.Value)
                    dsPR = objPR.PopulatePRList(PRStatus.Draft, viewState("Supplier"))

                    If dsPR.Tables(0).Rows.Count > 0 Then
                        Common.FillDdl(cboPR, "PRM_PR_NO", "PRM_PR_NO", dsPR)
                        cmdAddPR.Enabled = True
                        viewstate("blnCmdAddToPR") = True
                        Common.SelDdl(viewState("prid"), cboPR, True, True)
                        cboPR.Enabled = False
                    End If
                    objPR = Nothing
                Case "tab"
                    lnkBack.Visible = False
                    fillNil(cboPR)
                    cmdAddPR.Enabled = False
                    cmd_Raise.Enabled = False
                    viewstate("blnCmdAddToPR") = False
                    viewstate("blnCmdRaisePR") = False

                Case "view"
                    lnkBack.Visible = True
                    fillNil(cboPR)
                    cmdAddPR.Enabled = False
                    cmd_Raise.Enabled = False
                    viewstate("blnCmdAddToPR") = False
                    viewstate("blnCmdRaisePR") = False
            End Select
            'hidSummary.Value = "Remarks-" & txtRemark.ClientID
            Bindgrid(0, True)

            ' Dim blnMultipleSupplier As Boolean = True
            ''---------------New Code Added for CM_MULTI_PO on 04/July/2007  Start------------
            Dim Strmulti As String
            Dim strSql1 As String
            Dim dd As New System.Web.UI.WebControls.DropDownList
            Dim dds As DataTable
            strSql1 = "Select  CM_MULTI_PO FROM COMPANY_MSTR where CM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
            'dds = objDB.FillDt(strSql1)
            Dim tDS As DataSet = objDB.FillDs(strSql1)
            If tDS.Tables(0).Rows.Count > 0 Then
                Strmulti = tDS.Tables(0).Rows(0).Item("CM_MULTI_PO")
            End If
            If Strmulti = "Y" Then
                blnMultipleSupplier = True
            Else
                blnMultipleSupplier = False
            End If
            ''---------------New Code Added for CM_MULTI_PO on 04/July/2007  END------------   

            '--------------New code Added becoz Msgbox(No Record Found) is repeating 2 times,
            '---- Added if Condition(intPageRecordCnt)  By Praveen on 31/07/2007
            If intPageRecordCnt <> 0 Then
                If blnMultipleSupplier Then cboSupplier_SelectedIndexChanged(sender, e)
            End If
            'Updated here on 27/08/2007 
            'Michelle (30/8/2007) - check for multiple supplier If cboSupplier.SelectedIndex > 0 Then
            If cboSupplier.SelectedIndex > 0 And Not blnMultipleSupplier Then
                cboSupplier.Enabled = False
            Else
                cboSupplier.Enabled = True
            End If
            'End 
            '------End upto this end if.
        End If

        'cmdAddPR.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
        'cmd_Raise.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
        cmd_Delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")

        cmd_Update.Attributes.Add("onClick", "return resetSummary(0,0);")
        cmd_Raise.Attributes.Add("onClick", "return checkAtLeastOneResetSummary('chkSelection','',0,0);")
        cmdAddPR.Attributes.Add("onClick", "return checkAtLeastOneResetSummary('chkSelection','',0,0);")

        

    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objShopping As New ShoppingCart
        Dim ds As New DataSet
        ds = objShopping.PopulateShoppingCart(cboSupplier.SelectedItem.Value)
        objShopping = Nothing
        intPageRecordCnt = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        If pSorted Then
            dvViewSample.Sort = viewstate("SortExpression")
            If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If dtgShopping.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgShopping.PageSize = 0 Then
                dtgShopping.CurrentPageIndex = dtgShopping.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        '//datagrid.pageCount only got value after databind
        If intPageRecordCnt > 0 Then
            intTotPage = dtgShopping.PageCount
            cmd_Delete.Enabled = True
            cmd_Update.Enabled = True
            viewstate("blnCmdDelete") = True
            viewstate("blnCmdSave") = True
            'cmd_Raise.Enabled = True
            dtgShopping.DataSource = dvViewSample
            hidControl.Value = ""
            hidSummary.Value = ""
            dtgShopping.DataBind()
        Else
            intTotPage = 0
            ' ai chu add 12/09/2005
            ' no record, disable save button and delete button
            cmd_Delete.Enabled = False
            cmd_Update.Enabled = False
            cmd_Raise.Enabled = False
            viewstate("blnCmdDelete") = False
            viewstate("blnCmdSave") = False
            viewstate("blnCmdRaisePR") = False
            Common.NetMsgbox(Me, MsgNoRecord)
            dtgShopping.DataBind()
            cmdAddPR.Enabled = False
            viewstate("blnCmdAddToPR") = False
        End If
    End Function

    Public Sub dtgshopping_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgShopping.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(dtgShopping.CurrentPageIndex, True)
    End Sub

    Private Sub dtgShopping_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgShopping.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgShopping, e)

        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
            'chkAll.Attributes.Add("onclick", "CheckAll1();")
        End If
    End Sub

    Private Sub dtgShopping_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgShopping.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox

            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim lnkCoyName As HyperLink
            Dim strvendor As String
            strvendor = Common.parseNull(dv("SC_S_COY_ID"))
            lnkCoyName = e.Item.FindControl("lnkCoyName")
            lnkCoyName.Text = Common.parseNull(dv("CM_COY_NAME"))
            lnkCoyName.NavigateUrl = dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "pageid=" & strPageId & "&v_com_id=" & Common.parseNull(dv("SC_S_COY_ID")))

            'Dim lblSupplierCode As Label
            'lblSupplierCode = e.Item.FindControl("lblSupplierCode")
            'lblSupplierCode.Text = Common.parseNull(dv("SC_S_COY_ID"))

            'Dim lblProductCode As Label
            'lblProductCode = e.Item.FindControl("lblProductCode")
            'lblProductCode.Text = Common.parseNull(dv("SC_PRODUCT_CODE"))

            Dim txtRemark As TextBox
            txtRemark = e.Item.FindControl("txtRemark")
            txtRemark.Text = Common.parseNull(dv("SC_REMARK"))
            txtRemark.Attributes.Add("onKeyDown", "limitText (this, 400);")

            Dim txtQty As TextBox
            txtQty = e.Item.FindControl("txtQty")
            txtQty.Text = Common.parseNull(dv("SC_Quantity"))
            If txtQty.Text = "" Then txtQty.Text = "1"

            Dim revQty As RegularExpressionValidator
            revQty = e.Item.FindControl("revQty")
            revQty.ValidationExpression = "(?!^0*$)^\d{1,5}?$" '"\d{1,5}" '"^\d+$" ' "^\d+\.+\d+$|^\d+$"
            revQty.ControlToValidate = "txtQty"
            revQty.ErrorMessage = "Invalid quantity"
            revQty.Display = ValidatorDisplay.Dynamic

            Dim hidCode As HtmlInputHidden
            hidCode = e.Item.FindControl("hidCode")

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

            Dim txtPrice As TextBox
            txtPrice = e.Item.FindControl("txtPrice")

            Dim revPrice As RegularExpressionValidator
            Dim dblPrice As Double = 0
            If Not IsDBNull(dv("SC_UNIT_COST")) And (e.Item.Cells(EnumShoppingCart.icPrice).Text = "" Or e.Item.Cells(EnumShoppingCart.icPrice).Text = "&nbsp;") Then
                If CDbl(dv("SC_UNIT_COST")) <> 0 Then
                    txtPrice.Visible = False
                    e.Item.Cells(EnumShoppingCart.icPrice).Text = Format(Common.parseNull(dv("SC_UNIT_COST")), "###,###,##0.0000")
                    dblPrice = CDbl(dv("SC_UNIT_COST"))
                Else
                    txtPrice.Text = "0.0000"
                    revPrice = e.Item.FindControl("revPrice")
                    revPrice.ValidationExpression = "(?!^0*$)(?!^0*\.0*$)^\d{1,14}(\.\d{1,4})?$" '"^\d+\.+\d+$|^\d+$"
                    revPrice.ControlToValidate = "txtPrice"
                    revPrice.ErrorMessage = "Item Price is over limit/expecting numeric value."
                End If
            Else
                txtPrice.Text = "0.0000"
                revPrice = e.Item.FindControl("revPrice")
                revPrice.ValidationExpression = "^\d+\.+\d+$|^\d+$"
                revPrice.ControlToValidate = "txtPrice"
                revPrice.ErrorMessage = "Item Price is over limit/expecting numeric value."
            End If

            ' total
            Dim txtAmount As TextBox
            txtAmount = e.Item.FindControl("txtAmount")
            If IsDBNull(dv("TOTAL")) Then
                'txtAmount.Text = "0.0000"
                '-------New Code for TOTAL Amount in the grid added  by Praveen on 27/07/2007
                txtAmount.Text = Format(CDbl(txtQty.Text) * CDbl(dblPrice), "###,###,##0.000")
                '--------End the 
            Else
                txtAmount.Text = Format(dv("TOTAL"), "###,###,##0.0000")
            End If
            '-----end testing 

            'txtQ.Attributes.Add("onBlur", "resetValue('" & txtQ.ClientID & "', 1)")
            txtQty.Attributes.Add("onblur", "return calculateTotal('" & _
                                txtQty.ClientID & "', '" & dblPrice & "', '" & _
                                txtAmount.ClientID & "');")
        End If
    End Sub

    Private Sub cmd_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Delete.Click
        Dim dgItem As DataGridItem
        Dim objShopping As New ShoppingCart
        Dim strIndex As String
        Dim chkItem As CheckBox
        Dim i As Integer = 0
        Dim strSQL As String
        Dim strAryQuery(0) As String

        For Each dgItem In dtgShopping.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                strIndex = dtgShopping.DataKeys.Item(i)
                strSQL = objShopping.delShoppingCartItem(strIndex)
                Common.Insert2Ary(strAryQuery, strSQL)
            End If
            i = i + 1
        Next
        objDB.BatchExecute(strAryQuery)
        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        objShopping.FillSCVendor(cboSupplier)
        objShopping = Nothing
        Bindgrid()
    End Sub

    Private Sub cmd_Update_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Update.Click
        Dim dgItem As DataGridItem
        Dim objShopping As New ShoppingCart
        Dim strIndex As String
        Dim dblUnitPrice As Double
        Dim intQty As Integer
        Dim strRemark As String
        Dim strAryQuery(0) As String
        Dim strSQL As String
        Dim i As Integer = 0

        Dim txtPrice As TextBox
        Dim lblPrice As Label
        For Each dgItem In dtgShopping.Items
            strIndex = dtgShopping.DataKeys.Item(i) 'CType(dgItem.FindControl("lblIndex"), Label).Text
            txtPrice = dgItem.FindControl("txtPrice")

            If dgItem.Cells(EnumShoppingCart.icPrice).Text <> "" Then
                dblUnitPrice = CDbl(dgItem.Cells(EnumShoppingCart.icPrice).Text)
            Else
                dblUnitPrice = CDbl(CType(dgItem.FindControl("txtPrice"), TextBox).Text)
            End If
            intQty = IIf(CType(dgItem.FindControl("txtQty"), TextBox).Text.Trim = "", -1, CType(dgItem.FindControl("txtQty"), TextBox).Text.Trim)
            strRemark = CType(dgItem.FindControl("txtRemark"), TextBox).Text
            strSQL = objShopping.getModShoppingCartSQL(strIndex, dblUnitPrice, intQty, strRemark)
            Common.Insert2Ary(strAryQuery, strSQL)
            i = i + 1
        Next
        objShopping = Nothing
        objDB.BatchExecute(strAryQuery)
        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        Bindgrid()
    End Sub

    Private Sub cmd_Raise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Raise.Click
        Dim strMsg As String
        If Page.IsValid Then 'And checkMandatory(Nothing, strMsg) Then
            Dim dgItem As DataGridItem
            Dim objShopping As New ShoppingCart
            Dim strIndex As String
            Dim strCurrency As String = ""
            Dim strVendor As String = ""
            Dim strven1 As New ArrayList
            Dim strVendorName As String = ""
            Dim i As Integer = 0
            Dim j As Integer
            Dim chkItem As CheckBox
            Dim txtPrice As TextBox
            Dim dblUnitPrice As Double
            Dim intQty As Integer
            Dim strRemark As String
            Dim strSQL As String
            Dim strAryQuery(0) As String
            strIndexList = ""
            Dim Strmulti As String = ""
            Dim strSql1 As String

            'Michelle (12/3/2008) - CR0022
            strSql1 = "Select  CM_MULTI_PO FROM COMPANY_MSTR where CM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
            Dim tDS As DataSet = objDB.FillDs(strSql1)
            If tDS.Tables(0).Rows.Count > 0 Then
                Strmulti = tDS.Tables(0).Rows(0).Item("CM_MULTI_PO")
            End If
            If cboSupplier.SelectedItem.Value = "" And Strmulti <> "Y" Then
                Common.NetMsgbox(Me, "No multiple Vendors are allowed for a PR.", MsgBoxStyle.Exclamation)
                Bindgrid()
                Exit Sub
            End If

            For Each dgItem In dtgShopping.Items
                chkItem = dgItem.FindControl("chkSelection")
                If chkItem.Checked Then
                    If checkMandatory(dgItem.Cells(EnumShoppingCart.icSCoyID).Text, strMsg) Then
                        strIndex = dtgShopping.DataKeys.Item(i)
                        'strIndex = dtgShopping.DataKeys.Item("SC_CART_INDEX")
                        If strCurrency = "" Then
                            strCurrency = dgItem.Cells(EnumShoppingCart.icCurrencyCode).Text
                        ElseIf strCurrency.ToLower <> dgItem.Cells(EnumShoppingCart.icCurrencyCode).Text.ToLower Then
                            Common.NetMsgbox(Me, "Please select item with same currency.", MsgBoxStyle.Exclamation)
                            Bindgrid()
                            Exit Sub
                        End If
                        'If strVendor = "" Then
                        '    'strVendor = CType(dgItem.FindControl("lblSupplierCode"), Label).Text 'dgItem.Cells(EnumShoppingCart.icVendorCode).Text
                        '    strVendor = dgItem.Cells(EnumShoppingCart.icSCoyID).Text
                        '    ' strVendorName = dgItem.Cells(EnumShoppingCart.icVendorCode).Text
                        '    strVendorName = CType(dgItem.FindControl("lnkCoyName"), HyperLink).Text
                        'ElseIf strVendor.ToLower <> dgItem.Cells(EnumShoppingCart.icSCoyID).Text.ToLower Then 'CType(dgItem.FindControl("lblSupplierCode"), Label).Text
                        '    Common.NetMsgbox(Me, "Please select item from same vendor.", MsgBoxStyle.Exclamation)
                        '    Bindgrid()
                        '    Exit Sub
                        'End If

                        strVendor = dgItem.Cells(EnumShoppingCart.icSCoyID).Text
                        strVendorName = CType(dgItem.FindControl("lnkCoyName"), HyperLink).Text
                        txtPrice = dgItem.FindControl("txtPrice")
                        If dgItem.Cells(EnumShoppingCart.icPrice).Text = "" Then
                            dblUnitPrice = CDbl(CType(dgItem.FindControl("txtPrice"), TextBox).Text)
                            If dblUnitPrice <= 0 Then
                                Common.NetMsgbox(Me, "Unit price must be more than 0.", MsgBoxStyle.Exclamation)
                                Bindgrid()
                                Exit Sub
                            End If
                        End If
                        ' for update purpose
                        If dgItem.Cells(EnumShoppingCart.icPrice).Text <> "" Then
                            dblUnitPrice = CDbl(dgItem.Cells(EnumShoppingCart.icPrice).Text)
                        Else
                            dblUnitPrice = CDbl(CType(dgItem.FindControl("txtPrice"), TextBox).Text)
                        End If
                        intQty = IIf(CType(dgItem.FindControl("txtQty"), TextBox).Text.Trim = "", 0, CType(dgItem.FindControl("txtQty"), TextBox).Text.Trim)
                        strRemark = CType(dgItem.FindControl("txtRemark"), TextBox).Text
                        strSQL = objShopping.getModShoppingCartSQL(strIndex, dblUnitPrice, intQty, strRemark)
                        Common.Insert2Ary(strAryQuery, strSQL)
                        strIndexList &= strIndex & ","
                        'strVendor &= StrVenList & ","
                    Else
                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        Bindgrid()
                        Exit Sub
                    End If
                End If
                i = i + 1
            Next
            objShopping = Nothing
            objDB.BatchExecute(strAryQuery)
            Me.Session.Add("ItemIndexList", strIndexList)
            Response.Redirect(dDispatcher.direct("PR", "RaisePR.aspx", "type=cart&vendor=" & strVendor & "&currency=" & strCurrency & "&venname=" & strVendorName & "&pageid=" & strPageId))
            'Response.Redirect("RaisePR.aspx?type=cart&vendor=" & strven1 & "&currency=" & strCurrency & "&venname=" & strVendorName & "&pageid=" & strPageId)
        Else
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Bindgrid()
        End If

    End Sub

    Private Sub cboSupplier_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSupplier.SelectedIndexChanged
        dtgShopping.CurrentPageIndex = 0
        Bindgrid()
        Dim Strmulti As String
        Dim strSql1 As String
        strSql1 = "Select  CM_MULTI_PO FROM COMPANY_MSTR where CM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
        Dim tDS As DataSet = objDB.FillDs(strSql1)
        If tDS.Tables(0).Rows.Count > 0 Then
            Strmulti = tDS.Tables(0).Rows(0).Item("CM_MULTI_PO")
        End If
        If Strmulti = "Y" Then
            blnMultipleSupplier = True
        Else
            blnMultipleSupplier = False
        End If
        If cboSupplier.SelectedItem.Value = "" And Not blnMultipleSupplier Then
            'cmd_Raise.Enabled = True  'CR0020 - Reset the Raise button
            'viewstate("blnCmdRaisePR") = True
            cmd_Raise.Enabled = False
            viewstate("blnCmdRaisePR") = False
            cmdAddPR.Enabled = False
            viewstate("blnCmdAddToPR") = False
            fillNil(cboPR)

        Else

            If viewstate("type") = "tab" Then
                Dim objPR As New PR
                Dim dsPR As New DataSet
                dsPR = objPR.PopulatePRList("1", cboSupplier.SelectedItem.Value)

                If dsPR.Tables(0).Rows.Count > 0 Then
                    Common.FillDdl(cboPR, "PRM_PR_NO", "PRM_PR_NO", dsPR)
                    cmdAddPR.Enabled = True
                    viewstate("blnCmdAddToPR") = True
                    viewstate("Currency") = objPR.getPRCurrency(cboPR.SelectedItem.Value)
                Else
                    fillNil(cboPR)
                    cmdAddPR.Enabled = False
                    viewstate("blnCmdAddToPR") = False
                End If
                cmd_Raise.Enabled = True
                viewstate("blnCmdRaisePR") = True
                objPR = Nothing
            End If
        End If
    End Sub

    Private Sub fillNil(ByRef pdropdownlist As DropDownList)
        pdropdownlist.Items.Clear()
        Dim list As New ListItem
        list.Value = ""
        list.Text = "---Nil---"
        pdropdownlist.Items.Add(list)
    End Sub

    Private Sub cmdAddPR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddPR.Click
        Dim strMsg As String
        Dim dgItem As DataGridItem
        Dim objPR As New PR
        Dim chkItem As CheckBox
        Dim strAryExist As String
        Dim strAryItemAdd As String
        Dim strProductCode As String
        Dim i As Integer = 0

        viewstate("Currency") = objPR.getPRCurrency(cboPR.SelectedItem.Value)

        Dim dtItem As New DataTable
        dtItem.Columns.Add("PRId", Type.GetType("System.String"))
        dtItem.Columns.Add("CartIndex", Type.GetType("System.String"))
        dtItem.Columns.Add("ProductCode", Type.GetType("System.String"))
        dtItem.Columns.Add("Qty", Type.GetType("System.Int32"))
        dtItem.Columns.Add("Price", Type.GetType("System.Double"))
        dtItem.Columns.Add("Remark", Type.GetType("System.String"))
        dtItem.Columns.Add("SupplierCompanyId", Type.GetType("System.String"))

        Dim dtr As DataRow
        strAryExist = ""
        strAryItemAdd = ""
        For Each dgItem In dtgShopping.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then

                If checkMandatory(dgItem.Cells(EnumShoppingCart.icSCoyID).Text, strMsg) Then

                    dtr = dtItem.NewRow()
                    dtr("PRId") = cboPR.SelectedItem.Value
                    dtr("CartIndex") = dtgShopping.DataKeys.Item(i)

                    If UCase(viewstate("Currency")) <> UCase(dgItem.Cells(EnumShoppingCart.icCurrencyCode).Text) Then
                        Common.NetMsgbox(Me, "Different currency.", MsgBoxStyle.Exclamation)
                        Bindgrid()
                        Exit Sub
                    End If

                    If dgItem.Cells(EnumShoppingCart.icPrice).Text <> "" Then
                        dtr("Price") = CDbl(dgItem.Cells(EnumShoppingCart.icPrice).Text)
                    Else
                        dtr("Price") = CDbl(CType(dgItem.FindControl("txtPrice"), TextBox).Text)
                    End If

                    If Not IsNumeric(CType(dgItem.FindControl("txtQty"), TextBox).Text) Then CType(dgItem.FindControl("txtQty"), TextBox).Text = "0"

                    dtr("Qty") = CDbl(CType(dgItem.FindControl("txtQty"), TextBox).Text)
                    dtr("Remark") = CType(dgItem.FindControl("txtRemark"), TextBox).Text
                    dtr("ProductCode") = dgItem.Cells(EnumShoppingCart.icProductCode).Text ' CType(dgItem.FindControl("lblProductCode"), Label).Text
                    dtr("SupplierCompanyId") = dgItem.Cells(EnumShoppingCart.icSCoyID).Text
                    dtItem.Rows.Add(dtr)
                Else
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Bindgrid()
                    Exit Sub
                End If
            End If
            i = i + 1
        Next

        objPR.insertPRItem(dtItem, strAryExist, strAryItemAdd)
        objPR = Nothing

        If viewstate("type") = "PR" Then
            Response.Redirect(dDispatcher.direct("PR", "RaisePR.aspx", "pageid=" & strPageId & "&type=list&vendor=" & ViewState("Supplier") & "&currency=" & ViewState("currency") & "&prid=" & ViewState("prid")))
        Else
            Me.Session.Add("ItemExist", strAryExist)
            Me.Session.Add("ItemAdd", strAryItemAdd)
            Response.Redirect(dDispatcher.direct("PR", "PRConfirm.aspx", "type=I&prid=" & cboPR.SelectedItem.Value & "&pageid=" & strPageId))
        End If
    End Sub

    Private Function checkMandatory(ByVal pSupplier As String, ByRef strMsg As String) As Boolean
        strMsg = ""
        Dim objAdmin As New Admin
        Dim objUser As New Users

        ' ai chu modified on 29/09/2005
        ' PM or PO can buy not approved vendor's item
        If Not objUser.IsPurchasing Then
            'If Not objAdmin.isApprovedVendor(Session("CompanyId"), cboSupplier.SelectedItem.Value) Then
            If Not objAdmin.isApprovedVendor(Session("CompanyId"), pSupplier) Then
                strMsg = "This company is no longer an approved vendor! "
            End If
        End If
        objUser = Nothing

        Dim objPR As New PR
        Dim intBCM As String
        intBCM = CInt(objPR.checkBCM)
        If intBCM > 0 Then
            If Not objPR.checkUserAccExist() Then
                If strMsg <> "" Then
                    strMsg &= """&vbCrLf&"""
                End If
                strMsg &= "You are not assigned to any Budget Account Code. ""&vbCrLf&""Please contact the Finance Manager. "
            End If
        End If

        objPR = Nothing
        objAdmin = Nothing

        If strMsg <> "" Then
            checkMandatory = False
        Else
            checkMandatory = True
        End If
    End Function


End Class
