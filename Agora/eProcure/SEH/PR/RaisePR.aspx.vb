Imports AgoraLegacy
Imports eProcure.Component

Imports System.Text.RegularExpressions

Imports System.Drawing

'If ViewState("type") = "new" Then
'    Select Case ViewState("mode")
'        Case "bc"
'        Case "cc"
'    End Select
'ElseIf ViewState("type") = "mod" Then
'    Select Case ViewState("mode")
'        Case "bc"
'        Case "cc"
'    End Select
'End If

Public Class RaisePRSEH
    Inherits AgoraLegacy.AppBaseClass

    Public Enum EnumShoppingCart
        icChk = 0
        icNo = 1
        icProductCode = 2
        icVendorItemCode = 3
        icGLCode = 4
        icGLCode1 = 5
        icCategoryCode1 = 6
        icAssetGroup = 7
        icItemDesc = 8 '7
        icQty = 9 '8
        icCommodity = 10 '9
        icDeliveryTerm = 11
        icUOM = 12 '11 '10
        icCurrency = 13 '12 '11
        icPrice = 14 '13 '12
        icTotal = 15 '14 '13
        icTaxRate = 16
        icTax = 17 '16 '15 '14
        icGstTaxCode = 18
        icSource = 19 '18 '17 '16 '15
        icCategoryCode = 20 '19 '18 '17 '16
        icVendor = 21 '20 '19 '18 '17
        icBudget = 22 '21 '20 '19 '18
        icDelivery = 23 '22 '21 '20 '19
        icItemType = 24 '23 '22
        icLead = 25 '24 '23
        icEstDate = 26 '25 '24 '21 '20
        icWarranty = 27 '26 '25 '22 '21
        icRemark = 28 '27 '26 '23 '22
    End Enum

    Dim dvwDelivery, dvwGLCode, dvwCategoryCode As DataView
    Dim dvwCustom() As DataView
    Dim dvwCus As New DataView
    Dim strCustomDefault() As String
    Dim strDeliveryDefault As String
    Dim intRow As Integer
    Dim intCnt As Integer
    Dim dblNoTaxTotal, dblTaxTotal, dblTotalGst As Double
    Dim dvwCustomItem As DataView
    Dim objDB As New EAD.DBCom
    Dim objGlobal As New AppGlobals
    Dim blnItem As Boolean
    Dim i As Integer
    Dim blnValid As Boolean
    Dim blnValidBill As Boolean
    Dim strDefDelivery As String
    Dim dtBCM, dtGLCode, dtCategoryCode As DataTable
    Dim objGLO As New AppGlobals

    Protected WithEvents hidDelCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents Extra1 As Global.System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Extra2 As Global.System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Extra3 As Global.System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents body1 As HtmlGenericControl
    Protected WithEvents hidClientId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidTotalClientId As System.Web.UI.HtmlControls.HtmlInputHidden

    'Protected WithEvents GSTLabel As System.Web.UI.HtmlControls.HtmlTableCell

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim aryProdCodeNew As New ArrayList

    Dim keepAr As New ArrayList
    Dim keepArPost As New ArrayList
    Dim strQtyErr As String

    Dim Common_Ext As New AppGlobals_Ext

    Dim kk As Integer = 0

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

    Private Property DynamicColumnAdded() As Boolean
        Get
            If ViewState("ColumnAdded") Is Nothing Then
                Return False
            Else
                Return True
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("ColumnAdded") = Value
        End Set
    End Property

    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        MyBase.LoadViewState(savedState)
        If Me.DynamicColumnAdded Then
            Me.addDataGridColumn()
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        alButtonList.Add(cmdRaise)
        alButtonList.Add(cmdSetup)
        alButtonList.Add(cmdUpload)
        alButtonList.Add(cmdUploadInt)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        alButtonList.Add(cmdRaise)
        alButtonList.Add(cmdSetup)
        alButtonList.Add(cmdUpload)
        alButtonList.Add(cmdUploadInt)
        alButtonList.Add(cmdRemove)
        alButtonList.Add(cmdDupPRLine)
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdDelete)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        cmdRaise.Enabled = blnCanUpdate And blnCanAdd And ViewState("blnCmdRaise")
        cmdSetup.Enabled = blnCanAdd And blnCanUpdate And ViewState("blnCmdSetup")
        cmdAdd.Enabled = blnCanAdd And blnCanUpdate And ViewState("blnCmdAdd")
        cmdUpload.Enabled = blnCanAdd And blnCanUpdate And ViewState("blnCmdUpload")
        cmdUploadInt.Enabled = blnCanAdd And blnCanUpdate And ViewState("blnCmdUploadInt")
        cmdRemove.Enabled = blnCanUpdate And ViewState("blnCmdRemove")
        cmdDupPRLine.Enabled = blnCanUpdate And ViewState("blnCmdRemove")
        cmdDelete.Enabled = blnCanDelete And ViewState("blnCmdDelete")
        alButtonList.Clear()
        buildjava()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        strNewCSS = "both"
        MyBase.Page_Load(sender, e)

        Response.Expires = -1
        Response.AddHeader("cache-control", "private")
        Response.AddHeader("pragma", "no-cache")
        Response.CacheControl = "no-cache"

        blnPaging = False
        blnSorting = False
        SetGridProperty(dtgShopping)

        ViewState("blnCutPR") = False

        Dim dsBCM As New DataSet
        Dim objBudget As New BudgetControl
        Dim objPR As New PR

        ViewState("BCM") = CInt(objPR.checkBCM)
        'ViewState("BCM") = 1
        If ViewState("BCM") > 0 Then
            dsBCM = objBudget.getBCMListByUserNew(Session("UserId"))
            dtBCM = dsBCM.Tables(0)
        End If

        If Session("CurrentScreen") <> "RemoveItem" Then
            Session("strItem") = Nothing
            Session("strItemCust") = Nothing
            Session("strItemHead") = Nothing
        End If

        If Not IsPostBack Then
            strQtyErr = objGlobal.GetErrorMessage("00342")
            ViewState("ValQtyMsg") = strQtyErr

            Session("CurrentScreen") = ""

            GenerateTab()

            If Request.QueryString("frm") = "BuyerCat" Then
                'Construct URL For BC
                Session("RaisePRURL") = dDispatcher.direct("PR", "RaisePR.aspx", "frm=BuyerCat&pageid=" & strPageId)
            ElseIf Request.QueryString("frm") = "ConCat" Then
                Session("RaisePRURL") = dDispatcher.direct("PR", "RaisePR.aspx", "frm=ConCat&pageid=" & strPageId)
            Else
                'Construct URL For My PR
                Session("RaisePRURL") = dDispatcher.direct("PR", "RaisePR.aspx", "pageid=" & strPageId & "&index=" & Request.QueryString("index") & "&prid=" & Request.QueryString("prid") & "&type=" & Request.QueryString("type") & "&mode=" & Request.QueryString("mode"))
            End If

            aryProdCodeNew.Clear()

            ViewState("strPageId") = strPageId
            ViewState("blnCmdAdd") = True
            ViewState("blnCmdRaise") = True
            ViewState("blnCmdUpload") = True
            ViewState("blnCmdUploadInt") = True
            ViewState("blnCmdRemove") = True
            ViewState("blnCmdSetup") = True
            ViewState("postback") = 0
            ViewState("SuppId") = ""

            If Request.QueryString("frm") = "BuyerCat" Then
                ViewState("type") = "new"
                ViewState("mode") = "bc"
            ElseIf Request.QueryString("frm") = "ConCat" Then
                ViewState("type") = "new"
                ViewState("mode") = "cc"
            ElseIf Not Request.QueryString("type") = "" Then
                ViewState("type") = Request.QueryString("type")
                ViewState("mode") = Request.QueryString("mode")
                'ViewState("mode2") = "bc2"
            End If
            If Request.QueryString("frm") = "bc" Then
                Session("ProdList") = Nothing
                Session("urlreferer") = "Dashboard"
                cmdDelete.Visible = False
            End If
            ViewState("strPageId") = strPageId
            lblDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Today.Date)

            If Session("urlreferer") = "BuyerCatSearch" Then
                lnkBack.NavigateUrl = dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId)
            ElseIf Session("urlreferer") = "ConCatSearch" Then
                lnkBack.NavigateUrl = dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId)
            ElseIf Session("urlreferer") = "PRAll" Then
                lnkBack.NavigateUrl = dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId)
            ElseIf Session("urlreferer") = "Dashboard" Then
                lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
            End If

            'Dim dsBCM As New DataSet
            'Dim objBudget As New BudgetControl
            'Dim objPR As New PR

            ' _Yap : Change to outside postback for reload page on Add Item
            'ViewState("BCM") = CInt(objPR.checkBCM)
            ''ViewState("BCM") = 1
            'If ViewState("BCM") > 0 Then
            '    dsBCM = objBudget.getBCMListByUserNew(Session("UserId"))
            '    dtBCM = dsBCM.Tables(0)
            'End If

            Dim objAdmin As New Admin
            Dim dsDelivery As New DataSet
            dsDelivery = objAdmin.PopulateAddr("D", "", "", "")
            dvwDelivery = dsDelivery.Tables(0).DefaultView
            strDeliveryDefault = objAdmin.user_Default_Add_ByDefault("D")
            ViewState("strDeliveryDefault") = strDeliveryDefault

            objGlobal.FillCodeTable(cboPayTerm, CodeTable.PaymentTerm)
            objGlobal.FillCodeTable(cboPayMethod, CodeTable.PaymentMethod)
            objGlobal.FillCodeTable(cboCurr, CodeTable.Currency, , "RFQ")

            If ViewState("type") = "new" Then
                lblTitle.Text = "Raise Purchase Request"
                lblPRNo.Text = "To Be Allocated By System"
                Dim PR_Req_Name As String = objDB.GetVal("SELECT IFNULL(UM_USER_NAME,'') FROM USER_MSTR WHERE UM_USER_ID = '" & Session("UserId") & "' AND UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                txtRequestedName.Text = PR_Req_Name

                Dim PR_Req_Tel As String = objDB.GetVal("SELECT IFNULL(UM_TEL_NO,'') FROM USER_MSTR WHERE UM_USER_ID = '" & Session("UserId") & "' AND UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                txtRequestedContact.Text = PR_Req_Tel

                chkCustomPR.Checked = True
                chkRemarkPR.Checked = True

                Select Case ViewState("mode")
                    Case "bc"
                End Select
            ElseIf ViewState("type") = "mod" Then
                cmdDelete.Visible = True
                ViewState("blnCmdDelete") = True
                ViewState("prid") = Request.QueryString("prid")
                lblPRNo.Text = ViewState("prid")
                hidNewPR.Value = ViewState("prid")
                lblTitle.Text = "Raise Purchase Request"

                Session("ProdList") = aryProdCodeNew

                Dim PR_Type As String = objDB.GetVal("SELECT IFNULL(PRM_PR_TYPE,'') FROM pr_mstr WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO = '" & ViewState("prid") & "'")
                If PR_Type = "CC" Then
                    ViewState("mode") = "cc"
                End If

                Select Case ViewState("mode")
                    Case "bc"
                End Select
            End If

            'If Session("Env") <> "FTN" Then
            '    addDataGridColumn()
            '    Session("NonFTN") = True
            'Else
            '    Session("NonFTN") = False
            'End If
            addDataGridColumn()
            Session("NonFTN") = True

            Bindgrid()
            cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & File1.ClientID & "');")
            cmdUploadInt.Attributes.Add("onclick", "return checkDocFile('doc','" & File1Int.ClientID & "');")
            cmdRaise.Attributes.Add("onclick", "return InitialValidation();")
            cmdSubmit.Attributes.Add("onclick", "return InitialValidation();")
        Else
            If Session("CurrentScreen") = "AddItem" Then
                'Dim dtrd As DataRow
                Dim chkItem As CheckBox
                Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark As TextBox
                Dim txtBudget, txtDelivery As Label
                Dim cboGLCode, ddlDelivery, ddl_stock As DropDownList
                Dim strItem, strItemCust As New ArrayList()
                Dim dgItem As DataGridItem
                Dim k As Integer = 0

                For Each dgItem In dtgShopping.Items
                    txtQty = dgItem.FindControl("txtQty")
                    cboGLCode = dgItem.FindControl("cboGLCode")
                    txtPrice = dgItem.FindControl("txtPrice")
                    txtBudget = dgItem.FindControl("txtBudget")
                    hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                    txtDelivery = dgItem.FindControl("txtDelivery")
                    hidDelCode = dgItem.FindControl("hidDelCode")
                    txtEstDate = dgItem.FindControl("txtEstDate")
                    txtWarranty = dgItem.FindControl("txtWarranty")
                    txtRemark = dgItem.FindControl("txtRemark")
                    ddlDelivery = dgItem.FindControl("ddlDelivery")
                    ddl_stock = dgItem.FindControl("ddl_stock")

                    chkItem = dgItem.FindControl("chkSelection")
                    If chkItem.Checked Then
                        If Session("CurrentScreen") = "AddItem" Then
                            strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, ddlDelivery.SelectedIndex, ddl_stock.SelectedIndex})
                        End If
                    Else
                        strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, ddlDelivery.SelectedIndex, ddl_stock.SelectedIndex})
                    End If

                    Dim objAdmin As New Admin

                    dvwCus = objAdmin.getCustomField("")

                    If Not dvwCus Is Nothing Then
                        For i = 0 To dvwCus.Count - 1
                            'For Each dgItem2 In dtgShopping.Items
                            Dim cboCustom As DropDownList
                            cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                            If chkItem.Checked Then
                                If Session("CurrentScreen") = "AddItem" Then
                                    strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                                End If
                            Else
                                strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                            End If
                            'Next
                        Next
                    End If

                    k += 1
                Next
                Session("strItem") = strItem
                Session("strItemCust") = strItemCust

                Bindgrid()
                Session("CurrentScreen") = ""
            End If
        End If

        body1.Attributes.Add("onLoad", "refreshDatagrid(); calculateGrandTotal(); calculateAllIndividualTotal();")
        ViewState("body_loaditemcreated") = ""

        cmdRemove.Attributes.Add("onclick", "return RemoveItemCheck();")
        cmdDupPRLine.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")

        txtInternal.Attributes.Add("onKeyDown", "limitText (this, 1000);")
        txtExternal.Attributes.Add("onKeyDown", "limitText (this, 1000);")
        displayAttachFile()
        displayAttachFileInt()

        Session("keepItem") = ""
        Session("keepAr") = New ArrayList()

        Dim Asset As New PurchaseOrder_Buyer
        If Asset.AssetGroupMstr = False Then
            dtgShopping.Columns(EnumShoppingCart.icAssetGroup).Visible = False
        End If

        If ViewState("mode") = "cc" Then
            cboPayTerm.Enabled = False
            cboPayMethod.Enabled = False
            cboCurr.Enabled = False
        End If

        'dtgShopping.Columns(EnumShoppingCart.icCurrency).Visible = False

        Dim objCheckGST As New GST
        ViewState("CheckGST") = objCheckGST.chkGSTCOD()
        If ViewState("CheckGST") = True Then
            GSTLabel.InnerText = "GST Amount :"
        Else
            GSTLabel.InnerText = "Tax:"
            dtgShopping.Columns(EnumShoppingCart.icTaxRate).Visible = False
        End If
        objCheckGST = Nothing
    End Sub

    Private Sub cmdDupPRLine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDupPRLine.Click
        Dim dgItem As DataGridItem
        Dim objPR As New PR
        Dim chkItem As CheckBox
        Dim i As Integer = 0
        Dim strAryQuery(0) As String
        Dim strscript As New System.Text.StringBuilder

        If ViewState("type") = "new" Then
            For Each dgItem In dtgShopping.Items
                chkItem = dgItem.FindControl("chkSelection")

                Dim txtCommodity As TextBox
                txtCommodity = dgItem.FindControl("txtCommodity")

                If chkItem.Checked Then

                    Dim aryProdCodeCurrent As ArrayList = Session("ProdList")
                    aryProdCodeCurrent.Add(New String() {dgItem.Cells(EnumShoppingCart.icProductCode).Text, CType(dgItem.FindControl("lblProductDesc"), TextBox).Text, CType(dgItem.FindControl("lblUOM"), Label).Text, txtCommodity.Text, CType(dgItem.FindControl("lblCDGroup"), Label).Text, ""})
                    Session("ProdList") = aryProdCodeCurrent

                    'intLine = CInt(CType(dgItem.FindControl("lblItemLine"), Label).Text)
                    'strMsg = objPO.DupPOItem(hidNewPO.Value, intLine)
                    Exit For
                End If
            Next
            Select Case ViewState("mode")
                Case "bc", "cc"
            End Select

            'Dim dtrd As DataRow
            'Dim chkItem As CheckBox
            Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark As TextBox
            Dim txtBudget, txtDelivery As Label
            Dim cboGLCode, ddlDelivery, ddl_stock As DropDownList
            Dim strItem, strItemCust As New ArrayList()
            'Dim dgItem As DataGridItem
            Dim k As Integer = 0

            For Each dgItem In dtgShopping.Items
                txtQty = dgItem.FindControl("txtQty")
                cboGLCode = dgItem.FindControl("cboGLCode")
                txtPrice = dgItem.FindControl("txtPrice")
                txtBudget = dgItem.FindControl("txtBudget")
                hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                txtDelivery = dgItem.FindControl("txtDelivery")
                hidDelCode = dgItem.FindControl("hidDelCode")
                txtEstDate = dgItem.FindControl("txtEstDate")
                txtWarranty = dgItem.FindControl("txtWarranty")
                txtRemark = dgItem.FindControl("txtRemark")
                ddlDelivery = dgItem.FindControl("ddlDelivery")
                ddl_stock = dgItem.FindControl("ddl_stock")

                chkItem = dgItem.FindControl("chkSelection")
                'If chkItem.Checked Then
                'Else
                '    strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text})
                'End If
                strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, ddlDelivery.SelectedIndex, ddl_stock.SelectedIndex})

                Dim objAdmin As New Admin

                dvwCus = objAdmin.getCustomField("")

                If Not dvwCus Is Nothing Then
                    For i = 0 To dvwCus.Count - 1
                        'For Each dgItem2 In dtgShopping.Items
                        Dim cboCustom As DropDownList
                        cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                        'If chkItem.Checked Then
                        'Else
                        '    strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                        'End If
                        strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                        'Next
                    Next
                End If

                k += 1
            Next
            Session("strItem") = strItem
            Session("strItemCust") = strItemCust

            Bindgrid()
        ElseIf ViewState("type") = "mod" Then
            For Each dgItem In dtgShopping.Items
                chkItem = dgItem.FindControl("chkSelection")

                Dim txtCommodity As TextBox
                txtCommodity = dgItem.FindControl("txtCommodity")

                If chkItem.Checked Then
                    Session("CurrentScreen") = "AddItem"
                    Dim aryProdCodeCurrent As ArrayList = Session("ProdList")
                    aryProdCodeCurrent.Add(New String() {dgItem.Cells(EnumShoppingCart.icProductCode).Text, CType(dgItem.FindControl("lblProductDesc"), TextBox).Text, CType(dgItem.FindControl("lblUOM"), Label).Text, txtCommodity.Text, CType(dgItem.FindControl("lblCDGroup"), Label).Text, ""})
                    Session("ProdList") = aryProdCodeCurrent

                    'intLine = CInt(CType(dgItem.FindControl("lblItemLine"), Label).Text)
                    'strMsg = objPO.DupPOItem(hidNewPO.Value, intLine)
                    Exit For
                End If
            Next
            Select Case ViewState("mode")
                Case "cc"
            End Select
            strscript.Append("<script language=""javascript"">")
            strscript.Append("document.getElementById('btnHidden1').click();")
            strscript.Append("</script>")
            RegisterStartupScript("script14", strscript.ToString())
            'btnHidden1_Click(sender, e)
        End If

        'For Each dgItem In dtgShopping.Items
        '    chkItem = dgItem.FindControl("chkSelection")
        '    If chkItem.Checked Then
        '        intLine = CInt(CType(dgItem.FindControl("lblItemLine"), Label).Text)
        '        strMsg = objPR.DupPRItem(hidNewPR.Value, intLine)
        '        Exit For
        '    End If
        'Next

        'If strMsg = "1" Then
        '    'hidCost.Value = objPR.updatePOCost(hidNewPO.Value)
        '    Common.NetMsgbox(Me, "Item(s) Duplicated.", MsgBoxStyle.Information)
        '    Dim dsDelivery As New DataSet
        '    Dim objAdmin As New Admin
        '    Dim objBudget As New BudgetControl
        '    dsDelivery = objAdmin.PopulateAddr("D", "", "", "")
        '    dvwDelivery = dsDelivery.Tables(0).DefaultView
        '    strDeliveryDefault = objAdmin.user_Default_Add("D")

        '    Dim dsBCM As New DataSet
        '    dsBCM = objBudget.getBCMListByUser(Session("UserId"))
        '    dtBCM = dsBCM.Tables(0)
        '    Bindgrid()
        '    hidTotal.Value = ""
        '    'addDGTotal()
        '    hid2.Value = ""
        '    objAdmin = Nothing
        '    objBudget = Nothing
        'End If
        objPR = Nothing
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1) As String
        Dim objShopping As New ShoppingCart
        Dim objShopping_Ext As New ShoppingCart_Ext
        Dim dsItem, dsTemp As New DataSet
        Dim dvViewSample As DataView
        Dim aryProdCode As New ArrayList
        Dim strProdList As String = ""
        Dim strItemHead As New ArrayList()

        If ViewState("type") = "new" Then

            strProdList = "''"
            dsItem = objShopping_Ext.getPRItemList("BuyerCat", strProdList, "")
            aryProdCode = Session("ProdList")

            Select Case ViewState("mode")
                Case "bc"
                    If IsNothing(aryProdCode) = False Then
                        For i = 0 To aryProdCode.Count - 1
                            If strProdList = "" Then
                                strProdList = "'" & aryProdCode(i)(0) & "'"
                                dsTemp = objShopping_Ext.getPRItemList("BuyerCat", strProdList, "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), "", aryProdCode(i)(5))
                                dsItem.Tables(0).Merge(dsTemp.Tables(0))
                            Else
                                strProdList &= ", '" & aryProdCode(i)(0) & "'"
                                dsTemp = objShopping_Ext.getPRItemList("BuyerCat", aryProdCode(i)(0), "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), "", aryProdCode(i)(5))
                                dsItem.Tables(0).Merge(dsTemp.Tables(0))
                            End If
                        Next
                    End If

                    displayAttachFile()
                    displayAttachFileInt()
                Case "cc"
                    For i = 0 To aryProdCode.Count - 1
                        If strProdList = "" Then
                            strProdList = "'" & aryProdCode(i)(0) & "'"
                            dsTemp = objShopping_Ext.getPRItemList("ConCat", strProdList, "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4), aryProdCode(i)(5))
                            dsItem.Tables(0).Merge(dsTemp.Tables(0))
                        Else
                            strProdList &= ", '" & aryProdCode(i)(0) & "'"
                            dsTemp = objShopping_Ext.getPRItemList("ConCat", aryProdCode(i)(0), "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4), aryProdCode(i)(5))
                            dsItem.Tables(0).Merge(dsTemp.Tables(0))
                        End If

                    Next

                    Dim objDB2 As New EAD.DBCom
                    Dim strPaymentTerm As String
                    Dim strPaymentMethod As String
                    Dim strCurrency As String

                    If aryProdCode.Count <> 0 Then
                        strPaymentTerm = objDB2.GetVal("SELECT CV_PAYMENT_TERM FROM COMPANY_VENDOR WHERE CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CV_S_COY_ID = (SELECT CDM_S_COY_ID AS VENDOR FROM CONTRACT_DIST_COY INNER JOIN CONTRACT_DIST_MSTR ON CDC_GROUP_INDEX = CDM_GROUP_INDEX INNER JOIN COMPANY_MSTR ON CM_COY_ID = CDM_S_COY_ID AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' INNER JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX=CDM_GROUP_INDEX LEFT JOIN PRODUCT_MSTR ON CDI_PRODUCT_CODE=PM_PRODUCT_CODE AND CDC_B_COY_ID = PM_S_COY_ID LEFT JOIN COMPANY_B_ITEM_CODE ON PM_PRODUCT_CODE = CBC_PRODUCT_CODE AND CBC_B_COY_ID = PM_S_COY_ID WHERE CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CDM_GROUP_INDEX = '" & aryProdCode(i - 1)(4) & "' AND PM_PRODUCT_CODE IN (" & aryProdCode(i - 1)(0) & ") AND PM_DELETED <> 'Y' )")
                        strPaymentMethod = objDB2.GetVal("SELECT CV_PAYMENT_METHOD FROM COMPANY_VENDOR WHERE CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CV_S_COY_ID = (SELECT CDM_S_COY_ID AS VENDOR FROM CONTRACT_DIST_COY INNER JOIN CONTRACT_DIST_MSTR ON CDC_GROUP_INDEX = CDM_GROUP_INDEX INNER JOIN COMPANY_MSTR ON CM_COY_ID = CDM_S_COY_ID AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' INNER JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX=CDM_GROUP_INDEX LEFT JOIN PRODUCT_MSTR ON CDI_PRODUCT_CODE=PM_PRODUCT_CODE AND CDC_B_COY_ID = PM_S_COY_ID LEFT JOIN COMPANY_B_ITEM_CODE ON PM_PRODUCT_CODE = CBC_PRODUCT_CODE AND CBC_B_COY_ID = PM_S_COY_ID WHERE CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CDM_GROUP_INDEX = '" & aryProdCode(i - 1)(4) & "' AND PM_PRODUCT_CODE IN (" & aryProdCode(i - 1)(0) & ") AND PM_DELETED <> 'Y' )")
                        strCurrency = objDB2.GetVal("SELECT CM_CURRENCY_CODE FROM COMPANY_MSTR WHERE CM_COY_ID = (SELECT CDM_S_COY_ID AS VENDOR FROM CONTRACT_DIST_COY INNER JOIN CONTRACT_DIST_MSTR ON CDC_GROUP_INDEX = CDM_GROUP_INDEX INNER JOIN COMPANY_MSTR ON CM_COY_ID = CDM_S_COY_ID AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' INNER JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX=CDM_GROUP_INDEX LEFT JOIN PRODUCT_MSTR ON CDI_PRODUCT_CODE=PM_PRODUCT_CODE AND CDC_B_COY_ID = PM_S_COY_ID LEFT JOIN COMPANY_B_ITEM_CODE ON PM_PRODUCT_CODE = CBC_PRODUCT_CODE AND CBC_B_COY_ID = PM_S_COY_ID WHERE CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CDM_GROUP_INDEX = '" & aryProdCode(i - 1)(4) & "' AND PM_PRODUCT_CODE IN (" & aryProdCode(i - 1)(0) & ") AND PM_DELETED <> 'Y' )")

                        cboPayTerm.SelectedValue = strPaymentTerm
                        cboPayMethod.SelectedValue = strPaymentMethod
                        cboCurr.SelectedValue = strCurrency
                    End If

                    displayAttachFile()
                    displayAttachFileInt()
            End Select

            If Session("strItemHead") IsNot Nothing Then
                strItemHead = Session("strItemHead")
                txtAttention.Text = strItemHead(0)(0)
                txtInternal.Text = strItemHead(0)(1)
                txtExternal.Text = strItemHead(0)(2)
                chkUrgent.Checked = strItemHead(0)(3)

                cboPayTerm.SelectedIndex = strItemHead(0)(4)
                cboPayMethod.SelectedIndex = strItemHead(0)(5)
                cboCurr.SelectedIndex = strItemHead(0)(6)
            End If

            ViewState("intPageRecordCnt") = dsItem.Tables(0).Rows.Count
            dvViewSample = dsItem.Tables(0).DefaultView

        ElseIf ViewState("type") = "mod" Then
            strProdList = "''"
            dsItem = objShopping_Ext.getPRItemList("PR", "", ViewState("prid"))
            If Session("CurrentScreen") = "AddItem" Or Session("CurrentScreen") = "RemoveItem" Then

                aryProdCode = Session("ProdList")
                If ViewState("type") = "new" Or ViewState("type") = "mod" Then
                    Select Case ViewState("mode")
                        Case "bc"
                            For i = dsItem.Tables(1).Rows.Count To aryProdCode.Count - 1
                                If strProdList = "" Then
                                    strProdList = "'" & aryProdCode(i)(0) & "'"
                                    dsTemp = objShopping_Ext.getPRItemList("BuyerCat", strProdList, "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), "", aryProdCode(i)(5))
                                    dsItem.Tables(1).Merge(dsTemp.Tables(0))
                                    keepAr.Add(New String() {aryProdCode(i)(0), aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4), aryProdCode(i)(5)})
                                Else
                                    strProdList &= ", '" & aryProdCode(i)(0) & "'"
                                    dsTemp = objShopping_Ext.getPRItemList("BuyerCat", aryProdCode(i)(0), "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), "", aryProdCode(i)(5))
                                    dsItem.Tables(1).Merge(dsTemp.Tables(0))
                                    keepAr.Add(New String() {aryProdCode(i)(0), aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4), aryProdCode(i)(5)})
                                End If
                            Next
                        Case "cc"
                            For i = dsItem.Tables(1).Rows.Count To aryProdCode.Count - 1
                                If strProdList = "" Then
                                    strProdList = "'" & aryProdCode(i)(0) & "'"
                                    dsTemp = objShopping_Ext.getPRItemList("ConCat", strProdList, "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4), aryProdCode(i)(5))
                                    dsItem.Tables(1).Merge(dsTemp.Tables(0))
                                    keepAr.Add(New String() {aryProdCode(i)(0), aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4), aryProdCode(i)(5)})
                                Else
                                    strProdList &= ", '" & aryProdCode(i)(0) & "'"
                                    dsTemp = objShopping_Ext.getPRItemList("ConCat", aryProdCode(i)(0), "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4), aryProdCode(i)(5))
                                    dsItem.Tables(1).Merge(dsTemp.Tables(0))
                                    keepAr.Add(New String() {aryProdCode(i)(0), aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4), aryProdCode(i)(5)})
                                End If
                            Next
                    End Select
                End If

                Session("keepAr") = keepAr
                If Not strProdList = "" Then
                    Session("keepItem") = strProdList
                End If
            End If

            If Not Page.IsPostBack Then
                keepArPost = Session("keepAr")
                If Not IsNothing(keepArPost) Then
                    If ViewState("type") = "new" Or ViewState("type") = "mod" Then
                        Select Case ViewState("mode")
                            Case "bc"
                                For i = 0 To keepArPost.Count - 1
                                    dsTemp = objShopping_Ext.getPRItemList("BuyerCat", keepArPost(i)(0), "", "", Nothing, keepArPost(i)(1), keepArPost(i)(2), keepArPost(i)(3), "", keepArPost(i)(5))
                                    dsItem.Tables(1).Merge(dsTemp.Tables(0))
                                Next
                            Case "cc"
                                For i = 0 To keepArPost.Count - 1
                                    dsTemp = objShopping_Ext.getPRItemList("ConCat", keepArPost(i)(0), "", "", Nothing, keepArPost(i)(1), keepArPost(i)(2), keepArPost(i)(3), keepArPost(i)(4), keepArPost(i)(5))
                                    dsItem.Tables(1).Merge(dsTemp.Tables(0))
                                Next
                        End Select
                    End If

                    Session("keepItem") = ""
                    Session("keepAr") = New ArrayList()
                End If

                If dsItem.Tables(0).Rows.Count > 0 Then
                    txtRequestedName.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_REQ_NAME"))
                    txtRequestedContact.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_REQ_PHONE"))
                    lblDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dsItem.Tables(0).Rows(0)("PRM_CREATED_DATE"))
                    txtAttention.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_S_ATTN"))
                    txtInternal.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_INTERNAL_REMARK"))
                    txtExternal.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_EXTERNAL_REMARK"))

                    Common_Ext.SelDdlbyVal(Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_PAYMENT_TERM")), cboPayTerm, False, True)
                    Common_Ext.SelDdlbyVal(Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_PAYMENT_TYPE")), cboPayMethod, False, True)
                    Common_Ext.SelDdlbyVal(Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_CURRENCY_CODE")), cboCurr, False, True)

                    If Session("strItemHead") IsNot Nothing Then
                        strItemHead = Session("strItemHead")
                        txtAttention.Text = strItemHead(0)(0)
                        txtInternal.Text = strItemHead(0)(1)
                        txtExternal.Text = strItemHead(0)(2)
                        chkUrgent.Checked = strItemHead(0)(3)

                        cboPayTerm.SelectedIndex = strItemHead(0)(4)
                        cboPayMethod.SelectedIndex = strItemHead(0)(5)
                        cboCurr.SelectedIndex = strItemHead(0)(6)
                    End If

                    Session("keepItem") = ""
                    Session("keepAr") = New ArrayList()
                    If Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_PRINT_CUSTOM_FIELDS")) = "1" Then
                        chkCustomPR.Checked = True
                    Else
                        chkCustomPR.Checked = False
                    End If
                    If Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_PRINT_REMARK")) = "1" Then
                        chkRemarkPR.Checked = True
                    Else
                        chkRemarkPR.Checked = False
                    End If
                    If Common.parseNull(dsItem.Tables(0).Rows(0)("PRM_URGENT")) = "1" Then
                        chkUrgent.Checked = True
                    Else
                        chkUrgent.Checked = False
                    End If
                End If
            End If
            ViewState("intPageRecordCnt") = dsItem.Tables(1).Rows.Count
            dvViewSample = dsItem.Tables(1).DefaultView
            dvwCustomItem = dsItem.Tables(2).DefaultView

            Select Case ViewState("mode")
                Case "bc"
            End Select
        End If

        intPageRecordCnt = ViewState("intPageRecordCnt")
        intRow = 0

        dtgShopping.DataSource = dvViewSample
        dtgShopping.DataBind()
        objShopping = Nothing

        'If Session("Env") = "FTN" Then
        '    Me.dtgShopping.Columns(14).Visible = False
        '    Me.dtgShopping.Columns(17).Visible = False
        '    Extra1.Style.Item("display") = "none"
        '    Extra2.Style.Item("display") = "none"
        '    Extra3.Style.Item("display") = "none"
        'Else
        '    Me.dtgShopping.Columns(14).Visible = True
        '    Me.dtgShopping.Columns(17).Visible = True
        '    Extra1.Style.Item("display") = ""
        '    Extra2.Style.Item("display") = ""
        '    Extra3.Style.Item("display") = ""
        'End If
        'Me.dtgShopping.Columns(17).Visible = True
        Select Case ViewState("mode")
            Case "cc"
                dtgShopping.Columns(EnumShoppingCart.icTaxRate).Visible = True
                dtgShopping.Columns(EnumShoppingCart.icTax).Visible = True
        End Select
        Me.dtgShopping.Columns(EnumShoppingCart.icBudget).Visible = True

        Extra1.Style.Item("display") = ""
        Extra2.Style.Item("display") = ""
        Extra3.Style.Item("display") = ""
    End Function

    Private Function encodeCustomField(ByVal strValue As String) As String
        'strValue = strValue.Replace(" ", "_")        
        strValue = Server.UrlEncode(strValue)
        encodeCustomField = strValue
    End Function

    Private Function decodeCustomField(ByVal strValue As String) As String
        'strValue = strValue.Replace("_", " ")
        strValue = Server.UrlDecode(strValue)
        decodeCustomField = strValue
    End Function

    Private Sub dtgShopping_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgShopping.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dblAmt, dblGstAmt As Double

            Dim objAdmin As New Admin
            Dim sClientId As String, sTotalClient As String

            Dim GLCodelstItem As New ListItem
            Dim CategoryCodelstItem As New ListItem

            Dim Global_Ext As New AppGlobals_Ext
            Dim ddlDelivery As DropDownList
            ddlDelivery = e.Item.Cells(0).FindControl("ddlDelivery")
            Global_Ext.FillDelTerm(ddlDelivery)
            If ddlDelivery IsNot Nothing Then
                For Each li As ListItem In ddlDelivery.Items
                    li.Attributes("title") = li.Text
                Next
            End If

            If ViewState("type") = "new" Then
                Select Case ViewState("mode")
                    Case "bc"
                End Select
            ElseIf ViewState("type") = "mod" Then
                Common_Ext.SelDdlbyVal(Common.parseNull(dv("PRD_DEL_CODE")), ddlDelivery, False, True)
            End If

            Dim strItem As New ArrayList()
            Dim strItemCust As New ArrayList()

            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            'simply use one of the client id to get all client ID
            sTotalClient = hidClientId.Value
            sClientId = Mid(chk.ClientID, InStr(chk.ClientID, "_") + 1, InStr(Mid(chk.ClientID, InStr(chk.ClientID, "_") + 1), "_") - 1) & "|"
            If Not sTotalClient.Contains(sClientId) Then
                hidClientId.Value = hidClientId.Value & sClientId
                hidTotalClientId.Value = hidTotalClientId.Value + 1
            End If

            Dim txtBudget As Label
            txtBudget = e.Item.FindControl("txtBudget")
            txtBudget.Text = ""
            txtBudget.Width = System.Web.UI.WebControls.Unit.Pixel(50)

            Dim hidBudgetCode As TextBox
            hidBudgetCode = e.Item.FindControl("hidBudgetCode")
            hidBudgetCode.Text = ""

            Dim txtDelivery As Label
            txtDelivery = e.Item.FindControl("txtDelivery")
            txtDelivery.Text = strDefDelivery
            txtDelivery.Width = System.Web.UI.WebControls.Unit.Pixel(50)

            Dim hidDelCode As TextBox
            hidDelCode = e.Item.FindControl("hidDelCode")
            hidDelCode.Text = ""

            Dim lblProductCode As Label
            lblProductCode = e.Item.FindControl("lblProductCode")
            lblProductCode.Text = Common.parseNull(dv("PRODUCTCODE"))

            Dim lblOversea As Label
            lblOversea = e.Item.FindControl("lblOversea")
            lblOversea.Text = Common.parseNull(dv("PRD_OVERSEA"))

            Dim lblGLCode As Label
            lblGLCode = e.Item.FindControl("lblGLCode")
            lblGLCode.Text = Common.parseNull(dv("GLCODE"))

            Dim lblCategoryCode As Label
            lblCategoryCode = e.Item.FindControl("lblCategoryCode")
            lblCategoryCode.Text = Common.parseNull(dv("CATEGORYCODE"))

            Dim lblCDGroup As Label
            lblCDGroup = e.Item.FindControl("lblCDGroup")
            lblCDGroup.Text = Common.parseNull(dv("CDM_GROUP_INDEX"))

            Dim lblVendor As Label
            lblVendor = e.Item.FindControl("lblVendor")
            lblVendor.Text = Common.parseNull(dv("VENDOR"))

            Dim lnkCode As HyperLink
            lnkCode = e.Item.Cells(EnumShoppingCart.icVendorItemCode).FindControl("VENDORITEMCODE")
            lnkCode.NavigateUrl = "javascript:;"
            Session("UrlLocation") = "BuyerCatalogueSearch"
            lnkCode.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=B&pageid=" & strPageId & "&pid=" & lblProductCode.Text & "&index=&draft=0") & "')")
            lnkCode.Text = dv("VENDORITEMCODE")

            Dim lblVENDORITEMCODE As Label
            lblVENDORITEMCODE = e.Item.FindControl("lblVENDORITEMCODE")
            lblVENDORITEMCODE.Text = Common.parseNull(dv("VENDORITEMCODE"))

            Dim lblProductDesc As TextBox
            lblProductDesc = e.Item.FindControl("lblProductDesc")
            lblProductDesc.Text = Common.parseNull(dv("PRODUCTDESC"))
            lblProductDesc.Width = System.Web.UI.WebControls.Unit.Pixel(250)
            'lblProductDesc.Height = System.Web.UI.WebControls.Unit.Pixel(250)

            lblProductDesc.Attributes.Add("onKeyDown", "limitText (this, 500);")
            lblProductDesc.Attributes.Add("style", "background-color:Transparent; overflow:hidden; width:190px")


            Dim charRows, chars, remaining As Integer
            Dim tbCOntent As String
            charRows = 0
            chars = 0
            tbCOntent = lblProductDesc.Text
            lblProductDesc.Columns = 30
            chars = tbCOntent.Length
            charRows = chars / lblProductDesc.Columns
            remaining = chars - charRows * lblProductDesc.Columns
            If (remaining = 0) Then
                lblProductDesc.Rows = charRows
                lblProductDesc.TextMode = TextBoxMode.MultiLine
            Else
                lblProductDesc.Rows = charRows + 1
                lblProductDesc.TextMode = TextBoxMode.MultiLine
            End If

            If lblProductCode.Text <> "" And lblProductCode.Text <> "&nbsp;" Then
                lblProductDesc.ReadOnly = True
                lblProductDesc.Attributes.Add("style", "background-color:Transparent; overflow:hidden; border:none; width:190px")
            Else
                lblProductDesc.ReadOnly = False
                lblProductDesc.Attributes.Add("style", "overflow:hidden; width:190px")
            End If



            Dim lblUOM As Label
            lblUOM = e.Item.FindControl("lblUOM")
            lblUOM.Text = Common.parseNull(dv("UOM"))

            Dim lblCurrency As Label
            lblCurrency = e.Item.FindControl("lblCurrency")
            lblCurrency.Text = Common.parseNull(dv("CURRENCY"))

            'If ViewState("type") = "new" Then
            '    If lblProductCode.Text <> "" And lblProductCode.Text <> "&nbsp;" Then
            '        Common.parseNull(dv("CURRENCY"))
            '    Else
            '        lblCurrency.Text = cboCurr.SelectedValue
            '    End If

            '    Select Case ViewState("mode")
            '        Case "bc"
            '    End Select
            'ElseIf ViewState("type") = "mod" Then
            '    Select Case ViewState("mode")
            '        Case "bc"
            '    End Select
            'End If

            If lblProductCode.Text <> "" And lblProductCode.Text <> "&nbsp;" Then
                Common.parseNull(dv("CURRENCY"))
            Else
                'lblCurrency.Text = ""
                If cboCurr.SelectedIndex <> 0 Then
                    lblCurrency.Text = cboCurr.SelectedValue
                Else
                    lblCurrency.Text = ""
                End If
            End If

            e.Item.Cells(EnumShoppingCart.icNo).Text = intRow + 1
            Dim strSCoyId As String = ""
            Try
                If ViewState("type") = "new" Then
                    Select Case ViewState("mode")
                        Case "bc"
                            If Not IsDBNull(e.Item.DataItem("Supplierid")) Then strSCoyId = e.Item.DataItem("Supplierid").ToString
                    End Select
                ElseIf ViewState("type") = "mod" Then
                    If Not IsDBNull(e.Item.DataItem("PRD_S_COY_ID")) Then strSCoyId = e.Item.DataItem("PRD_S_COY_ID").ToString
                End If

            Catch ex As Exception
            End Try
            e.Item.Attributes.Add("SuppId", strSCoyId)
            ViewState("SuppId") = strSCoyId

            Dim hidItemLine As HtmlInputHidden
            hidItemLine = e.Item.FindControl("hidItemLine")
            hidItemLine.Value = intRow + 1

            Dim txtQty As TextBox ' HtmlInputText
            txtQty = e.Item.FindControl("txtQty")
            txtQty.Text = Common.parseNull(dv("QUANTITY"))

            Dim revQty As RegularExpressionValidator
            revQty = e.Item.FindControl("revQty")
            'revQty.ValidationExpression = "(?!^0*$)^\d{1,5}?$"
            'revQty.ValidationExpression = "^([1-9]\d{0,5}\.\d{0,2}|[1-9]\d{0,5}|[0]\d{0,5}\.[1-9]\d{0,1})$"
            'revQty.ValidationExpression = "^[0-9]{1,6}(\.[0-9]{1,2})?$"
            revQty.ValidationExpression = "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$"
            revQty.ControlToValidate = "txtQty"
            'revQty.ErrorMessage = "Invalid quantity"
            revQty.ErrorMessage = ViewState("ValQtyMsg") '"Invalid quantity. Range should be from 0.01 to 999999.99"
            revQty.Text = "?"
            revQty.Display = ValidatorDisplay.Dynamic

            'Dim compValQty As CompareValidator
            'compValQty = e.Item.FindControl("compValQty")
            'compValQty.ValueToCompare = 0
            'compValQty.Operator = ValidationCompareOperator.NotEqual
            'compValQty.ControlToValidate = "txtQty"
            'compValQty.ErrorMessage = "Invalid quantity. Range should be from 0.01 to 999999.99"
            'compValQty.Text = "?"
            'compValQty.Display = ValidatorDisplay.Dynamic

            Dim appGlobal As New AppGlobals
            Dim ddl_uom As New DropDownList
            Dim dv_uom As DataView
            ddl_uom = e.Item.FindControl("ddl_uom")

            dv_uom = appGlobal.GetCodeTableView(CodeTable.Uom)
            Common.FillDdl(ddl_uom, "CODE_DESC", "CODE_ABBR", dv_uom)

            Common.SelDdl(dv("UOM"), ddl_uom, False, True)

            'Dim ddl_comm As New DropDownList
            'ddl_comm = e.Item.FindControl("ddl_comm")
            'appGlobal.FillCommodityType(ddl_comm)

            'Common.SelDdl(Common.parseNull(dv("COMMODITY")), ddl_comm, False, True)

            Populate(intRow)

            Dim txtPrice As TextBox
            txtPrice = e.Item.FindControl("txtPrice")
            If IsDBNull(dv("UNITCOST")) Then
                txtPrice.Text = ""
            Else
                txtPrice.Text = Format(dv("UNITCOST"), "###,###,##0.0000")
            End If

            Dim lblNoTax As Label
            lblNoTax = e.Item.FindControl("lblNoTax")

            Dim lblGSTRate As Label
            lblGSTRate = e.Item.FindControl("lblGSTRate")
            Select Case ViewState("mode")
                Case "bc"
                    lblGSTRate.Text = ""
                Case "cc"
                    lblGSTRate.Text = Common.parseNull(dv("GSTRateDesc"))
            End Select

            Dim hidTaxPerc, hidGSTRate As TextBox
            Dim hidTaxID As TextBox

            hidTaxPerc = e.Item.FindControl("hidTaxPerc")
            hidGSTRate = e.Item.FindControl("hidGSTRate")
            hidTaxID = e.Item.FindControl("hidTaxID")

            hidTaxPerc.Text = Common.parseNull(dv("GST"))
            hidGSTRate.Text = Common.parseNull(dv("GSTRate"))

            'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH
            Dim lblTaxCode As Label
            lblTaxCode = e.Item.FindControl("lblTaxCode")
            If Common.parseNull(dv("GSTRate")) <> "" Then
                If Not IsDBNull(dv("GstTaxCode")) Then
                    lblTaxCode.Text = dv("GstTaxCode")
                End If
            End If

            If ViewState("type") = "new" Then
                Select Case ViewState("mode")
                    Case "bc"
                End Select
            ElseIf ViewState("type") = "mod" Then
                Select Case ViewState("mode")
                    Case "bc"
                End Select
            End If

            Dim txtAmount As TextBox
            txtAmount = e.Item.FindControl("txtAmount")
            If IsDBNull(dv("AMOUNT")) Then
                txtAmount.Text = Format(0, "###,###,##0.00")
            Else
                txtAmount.Text = Format(dv("AMOUNT"), "###,###,##0.00")
            End If

            Dim txtGST As TextBox
            txtGST = e.Item.FindControl("txtGST")
            txtGST.Text = Common.parseNull(dv("GST"))

            Dim txtGSTAmt As TextBox
            txtGSTAmt = e.Item.FindControl("txtGSTAmt")
            dblAmt = CDbl(txtAmount.Text)

            ' Note on unknow use for "subtotal" _Yap
            If ViewState("GST") = "subtotal" Then
                dblTaxTotal = dblTaxTotal + dblAmt
                ViewState("intGSTcnt") += 1
                ViewState("intTotItem") += 1
                dblGstAmt = dblAmt * (hidTaxPerc.Text / 100)
                txtGSTAmt = e.Item.FindControl("txtGSTAmt")
                txtGSTAmt.Text = Format(dblGstAmt, "###,##0.00")
            Else
                dblNoTaxTotal = dblNoTaxTotal + dblAmt
                If dv("GST") = 0 Then
                    txtGSTAmt.Text = "0.00"
                Else
                    dblGstAmt = dblAmt * (dv("GST") / 100)
                    txtGSTAmt.Text = Format(dblGstAmt, "###,##0.00")
                End If
            End If

            Dim hidGSTAmt As HtmlInputHidden
            hidGSTAmt = e.Item.FindControl("hidGSTAmt")
            '2015-06-17: CH: Rounding issue (Prod issue)
            'hidGSTAmt.Value = dblGstAmt
            hidGSTAmt.Value = CDec(Format(dblGstAmt, "###0.00"))

            txtQty.Attributes.Add("onfocus", "return focusControl('" &
                                IIf(ViewState("GST") = "product", 0, 1) & "', '" &
                                txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" &
                                txtAmount.ClientID & "', '" & txtGSTAmt.ClientID & "', '" &
                                hidTaxPerc.ClientID & "', '" & Common.parseNull(dv("GST")) & "');")
            txtQty.Attributes.Add("onblur", "return calculateTotal('" &
                                IIf(ViewState("GST") = "product", 0, 1) & "', '" &
                                txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" &
                                txtAmount.ClientID & "', '" & txtGSTAmt.ClientID & "', '" &
                                hidTaxPerc.ClientID & "', '" & Common.parseNull(dv("GST")) & "','1');")

            'txtPrice.Attributes.Add("onfocus", "return focusControl('" & _
            '                   IIf(ViewState("GST") = "product", 0, 1) & "', '" & _
            '                   txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" & _
            '                   txtAmount.ClientID & "', '" & txtGSTAmt.ClientID & "', '" & _
            '                   hidTaxPerc.ClientID & "', '" & Common.parseNull(dv("GST")) & "');")
            'txtPrice.Attributes.Add("onblur", "return calculateTotal('" & _
            '                    IIf(ViewState("GST") = "product", 0, 1) & "', '" & _
            '                    txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" & _
            '                    txtAmount.ClientID & "', '" & txtGSTAmt.ClientID & "', '" & _
            '                    hidTaxPerc.ClientID & "', '" & Common.parseNull(dv("GST")) & "','1');")

            '2015-06-17: CH: Rounding issue (Prod issue)
            'dblTotalGst = dblTotalGst + dblGstAmt
            dblTotalGst = dblTotalGst + CDec(Format(dblGstAmt, "###0.00"))

            txtGSTAmt = e.Item.FindControl("txtGSTAmt")

            Dim txtCommodity As TextBox
            txtCommodity = e.Item.FindControl("txtCommodity")

            Dim hidCommodity As HtmlInputHidden
            hidCommodity = e.Item.FindControl("hidCommodity")

            Dim ddl_stock As DropDownList
            ddl_stock = e.Item.FindControl("ddl_stock")

            If ViewState("type") = "new" Then
                Common_Ext.SelDdlbyVal(Common.parseNull(dv("PRD_ITEM_TYPE")), ddl_stock, False, True)
                ' If spot, pre-select the first Delivery Term which is the local delivery term  as setup by Biz Hub.
                If ddl_stock.SelectedItem.Value = "SP" Then
                    ddlDelivery.SelectedIndex = 1
                End If
                Select Case ViewState("mode")
                    Case "bc"
                    Case "cc"
                        If ddl_stock.SelectedItem.Value = "SP" Then
                            ddlDelivery.Enabled = False
                        End If
                End Select
            ElseIf ViewState("type") = "mod" Then
                Common_Ext.SelDdlbyVal(Common.parseNull(dv("PRD_ITEM_TYPE")), ddl_stock, False, True)
                If Common.parseNull(dv("PRD_DEL_CODE")) = "" And ddl_stock.SelectedItem.Value = "SP" Then
                    ddlDelivery.SelectedIndex = 1
                End If
                Select Case ViewState("mode")
                    Case "bc"
                    Case "cc"
                        If ddl_stock.SelectedItem.Value = "SP" Then
                            ddlDelivery.Enabled = False
                        End If
                End Select
            End If

            Dim objDB2 As New EAD.DBCom
            If Common.Parse(dv("VENDORITEMCODE")) = "" Then
                hidCommodity.Value = objDB2.GetVal("SELECT CT_ID FROM COMMODITY_TYPE WHERE CT_NAME = '" & Common.Parse(dv("COMMODITY")) & "'")
                txtCommodity.Text = dv("COMMODITY")
                ddl_uom.Enabled = True
                'ddl_comm.Enabled = True
                txtCommodity.Enabled = True
                txtAmount.Text = "0.00"
                txtPrice.Text = "0.0000"
                txtGSTAmt.Text = "0.00"
                If ViewState("mode") <> "bc" Then
                    ddl_stock.Enabled = False
                End If
            Else
                hidCommodity.Value = objDB2.GetVal("SELECT CT_ID FROM COMMODITY_TYPE WHERE CT_NAME = '" & Common.Parse(dv("COMMODITY")) & "'")
                txtCommodity.Text = dv("COMMODITY")
                ddl_uom.Enabled = False
                'ddl_comm.Enabled = False
                txtCommodity.Enabled = False
                ddl_stock.Enabled = False

                'If lblVendor.Text <> "" And lblVendor.Text <> "&nbsp;" Then
                '    ViewState("Oversea") = lblOversea.Text
                '    ViewState("ItemType") = ddl_stock.SelectedItem.Value
                '    ViewState("Vendor") = lblVendor.Text
                'End If                
            End If
            ViewState("Oversea") = lblOversea.Text
            ViewState("ItemType") = ddl_stock.SelectedItem.Value
            ViewState("Vendor") = lblVendor.Text

            Dim lblItemLine As Label
            lblItemLine = e.Item.FindControl("lblItemLine")
            lblItemLine.Text = Common.parseNull(dv("ITEMLINE"))

            ' New added for the PR GL and category: Start
            Dim cboGLCode, cboCategoryCode As DropDownList
            Dim dsGLCode As New DataSet
            Dim dsCategoryCode As New DataSet

            Dim objDB As New EAD.DBCom
            Dim PFOR As String = objDB.GetVal("SELECT PM_PRODUCT_FOR FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & Common.parseNull(dv("PRODUCTCODE")) & "'")

            cboGLCode = e.Item.FindControl("cboGLCode")
            dsGLCode = objAdmin.PopulateGLCodeMstr(Common.parseNull(dv("PRODUCTCODE")), Common.parseNull(dv("GLCODE")), PFOR)
            dvwGLCode = dsGLCode.Tables(0).DefaultView

            Common.FillDdl(cboGLCode, "DESCRIPTION", "GL Code", dvwGLCode)
            If Not IsDBNull(dv("GLCode")) And Common.parseNull(dv("GLCode")) <> "" Then
                Common.SelDdl(dv("GLCode"), cboGLCode, True, True)
            Else
                If cboGLCode.Items.Count = 0 Then
                    GLCodelstItem.Value = ""
                    GLCodelstItem.Text = "Not Applicable"
                    cboGLCode.Items.Insert(0, GLCodelstItem)
                    cboGLCode.SelectedIndex = 0
                End If
            End If

            cboCategoryCode = e.Item.FindControl("cboCategoryCode")
            Dim PFORC As String = objDB.GetVal("SELECT PM_PRODUCT_FOR FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & Common.parseNull(dv("PRODUCTCODE")) & "'")

            dsCategoryCode = objAdmin.PopulateCategoryCodeMstr(Common.parseNull(dv("PRODUCTCODE")), Common.parseNull(dv("CategoryCode")), PFORC)
            dvwCategoryCode = dsCategoryCode.Tables(0).DefaultView
            Common.FillDdl(cboCategoryCode, "Category Code", "Category Code", dvwCategoryCode)


            If Not IsDBNull(dv("CategoryCode")) And Common.parseNull(dv("CategoryCode")) <> "" Then
                Common.SelDdl(dv("CategoryCode"), cboCategoryCode, True, True)
            Else
                If cboCategoryCode.Items.Count = 0 Then
                    CategoryCodelstItem.Value = ""
                    CategoryCodelstItem.Text = "Not Applicable"
                    cboCategoryCode.Items.Insert(0, CategoryCodelstItem)
                    cboCategoryCode.SelectedIndex = 0
                End If
            End If

            Dim Asset As New PurchaseOrder_Buyer
            If Asset.AssetGroupMstr = True Then
                Dim cboAssetGroup As DropDownList
                cboAssetGroup = e.Item.FindControl("cboAssetGroup")
                objGlobal.FillAssetGroup(cboAssetGroup)
                Common.SelDdl(dv("PRD_ASSET_GROUP"), cboAssetGroup, True, True)
            End If

            If lblVENDORITEMCODE.Text <> "" And lblVENDORITEMCODE.Text <> "&nbsp;" Then
                e.Item.Cells(EnumShoppingCart.icGLCode).Enabled = False
                e.Item.Cells(EnumShoppingCart.icGLCode1).Enabled = False
                e.Item.Cells(EnumShoppingCart.icCategoryCode1).Enabled = False
            End If
            ' New added for the PR GL and category: End

            ' BCM _Yap
            If ViewState("BCM") > 0 Then
                If Not IsDBNull(dv("ACCT")) And Common.parseNull(dv("ACCT")) <> "" And ViewState("type") = "mod" Then
                    If Not dtBCM Is Nothing Then
                        Dim drTemp As DataRow()
                        drTemp = dtBCM.Select("Acct_Index=" & dv("ACCT"))
                        If drTemp.Length > 0 Then
                            txtBudget.Text = Mid(drTemp(0)("Acct_List"), 1, 10)
                            hidBudgetCode.Text = Common.parseNull(dv("ACCT"))
                        End If
                    End If
                Else
                    If Not dtBCM Is Nothing And dtBCM.Rows.Count > 0 Then
                        txtBudget.Text = Mid(dtBCM.Rows(0)("Acct_List"), 1, 10)
                        hidBudgetCode.Text = dtBCM.Rows(0)("Acct_Index")
                    End If
                End If

                Dim cmdBudget As HtmlInputButton
                cmdBudget = e.Item.FindControl("cmdBudget")
                cmdBudget.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("PR", "Budget.aspx", "pageid=" & strPageId & "&id=" & txtBudget.ClientID & "&hidBudgetCode=" & hidBudgetCode.ClientID) & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no')")

            Else
                e.Item.Cells(EnumShoppingCart.icBudget).Visible = False
            End If

            If ViewState("type") = "new" Then
                hidDelCode.Text = ViewState("strDeliveryDefault")
                txtDelivery.Text = Mid(objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), ViewState("strDeliveryDefault"), "D"), 1, 10)
                Select Case ViewState("mode")
                    Case "bc"
                End Select
            ElseIf ViewState("type") = "mod" Then
                If IsDBNull(dv("DADDRCODE")) Then
                    hidDelCode.Text = ViewState("strDeliveryDefault")
                    txtDelivery.Text = Mid(objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), ViewState("strDeliveryDefault"), "D"), 1, 10)
                Else
                    'ViewState("strDeliveryDefault") = Common.Parse(dv("DADDRCODE"))
                    'hidDelCode.Text = ViewState("strDeliveryDefault")
                    hidDelCode.Text = Common.Parse(dv("DADDRCODE"))
                    txtDelivery.Text = Mid(objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), Common.Parse(dv("DADDRCODE")), "D"), 1, 10)
                End If
                Select Case ViewState("mode")
                    Case "bc"
                End Select
            End If

            Dim cmdDelivery As HtmlInputButton
            cmdDelivery = e.Item.FindControl("cmdDelivery")
            cmdDelivery.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("Admin", "AddressMaster.aspx", "pageid=" & strPageId & "&mod=P&type2=RPO&type=D&id=" & hidDelCode.ClientID & "&txtDelivery=" & txtDelivery.ClientID) & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no')")

            ' Custom Field
            Dim cboCustom As DropDownList
            Dim j As Integer
            'If e.Item.Cells(EnumShoppingCart.icRemark).Controls(0).GetType Is GetType(DropDownList) Then
            '    For i = EnumShoppingCart.icRemark To EnumShoppingCart.icRemark + intCnt - 1
            '        Dim blnSetDefault As Boolean
            '        blnSetDefault = False
            '        cboCustom = e.Item.Cells(i).Controls(0)
            '        Common.FillDdl(cboCustom, "CF_FIELD_VALUE", "CF_FIELD_VALUE", dvwCustom(i - EnumShoppingCart.icRemark))
            '        rebindDDL(cboCustom)

            '        Common.SelDdl(strCustomDefault(i - EnumShoppingCart.icRemark), cboCustom, True, True)
            '        blnSetDefault = True

            '        If blnSetDefault = False Then
            '            Common.SelDdl(strCustomDefault(i - EnumShoppingCart.icRemark), cboCustom, True, True)
            '        End If
            '    Next
            'End If


            'If ViewState("type") = "new" Then
            '    Select Case ViewState("mode")
            '        Case "bc"
            '        Case "cc"
            '    End Select
            'ElseIf ViewState("type") = "mod" Then
            '    Select Case ViewState("mode")
            '        Case "bc"
            '        Case "cc"
            '    End Select
            'End If

            If e.Item.Cells(EnumShoppingCart.icRemark).Controls(0).GetType Is GetType(DropDownList) Then
                For i = EnumShoppingCart.icRemark To EnumShoppingCart.icRemark + intCnt - 1
                    Dim blnSetDefault As Boolean
                    blnSetDefault = False
                    cboCustom = e.Item.Cells(i).Controls(0)
                    Common.FillDdl(cboCustom, "CF_FIELD_VALUE", "CF_FIELD_VALUE", dvwCustom(i - EnumShoppingCart.icRemark))
                    rebindDDL(cboCustom)

                    If ViewState("type") = "new" Then
                        Common.SelDdl(strCustomDefault(i - EnumShoppingCart.icRemark), cboCustom, True, True)
                        blnSetDefault = True
                        Select Case ViewState("mode")
                            Case "bc"
                            Case "cc"
                        End Select
                    ElseIf ViewState("type") = "mod" Then
                        For j = 0 To dvwCustomItem.Table.Rows.Count - 1
                            'If dvwCustomItem.Table.Rows(j)("PCD_PR_LINE") = lblItemLine.Text And dvwCustomItem.Table.Rows(j)("PCD_FIELD_NO") = dtgShopping.Columns(i).SortExpression Then
                            'If dvwCustomItem.Table.Rows(j)("PCD_PR_LINE") = e.Item.Cells(EnumShoppingCart.icNo).Text And dvwCustomItem.Table.Rows(j)("PCD_FIELD_NO") = dtgShopping.Columns(i).SortExpression Then
                            If dvwCustomItem.Table.Rows(j)("PCD_PR_LINE") = dv("ITEMLINE") And dvwCustomItem.Table.Rows(j)("PCD_FIELD_NO") = dtgShopping.Columns(i).SortExpression Then
                                Common.SelDdl(encodeCustomField(dvwCustomItem.Table.Rows(j)("PCD_FIELD_VALUE")), cboCustom, True, True)

                                'If blnSetDefault = False And Session("CurrentScreen") = "AddItem" Then
                                '    Common.SelDdl(strCustomDefault(i - EnumShoppingCart.icRemark), cboCustom, True, True)
                                'End If

                                blnSetDefault = True
                                Exit For
                            End If
                        Next
                        Select Case ViewState("mode")
                            Case "bc"
                            Case "cc"
                        End Select
                    End If

                    If blnSetDefault = False Then
                        Common.SelDdl(strCustomDefault(i - EnumShoppingCart.icRemark), cboCustom, True, True)
                    End If

                    If Session("strItemCust") IsNot Nothing Then
                        strItemCust = Session("strItemCust")
                        'For j = 0 To dvwCustomItem.Table.Rows.Count - 1
                        If (CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1) <= (strItemCust.Count / intCnt) - 1 Then
                            'cboCustom.SelectedIndex = strItemCust(intRow + i - CInt(EnumShoppingCart.icRemark))(1)
                            'If kk = 0 Then
                            '    kk = intRow
                            'Else
                            '    kk = intRow + 1
                            'End If
                            cboCustom.SelectedIndex = strItemCust(kk)(1)
                            kk = kk + 1
                        End If
                        'Next
                    End If
                Next
            End If

            Dim txtEstDate As TextBox
            Dim txtWarranty As TextBox
            Dim lblLead As Label

            txtEstDate = e.Item.FindControl("txtEstDate")
            txtWarranty = e.Item.FindControl("txtWarranty")
            lblLead = e.Item.FindControl("lblLead")
            'Michelle (2/8/2010) - To include the defaul EDD
            'Dim objDB As New EAD.DBCom
            Dim txtDate1 As Date = CType(lblDate.Text, Date)
            Dim intEDD As Integer = Val(objDB.GetVal("SELECT CV_EDD FROM COMPANY_VENDOR WHERE CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CV_S_COY_ID = '" & strSCoyId & "'"))

            If ViewState("type") = "new" Then
                If intEDD > 0 Then
                    txtEstDate.Text = txtDate1.AddDays(intEDD)
                    txtEstDate.ReadOnly() = True
                    txtEstDate.ForeColor = Color.Gray
                    txtEstDate.Enabled = False
                Else
                    txtEstDate.Text = txtDate1.AddDays(1)
                    txtEstDate.ReadOnly() = False
                End If
                '' ''lblLead.Text = intEDD
                Dim strLead As String
                strLead = objDB.GetVal("SELECT IFNULL(PV_LEAD_TIME, '') AS PV_LEAD_TIME FROM PIM_VENDOR WHERE PV_PRODUCT_INDEX = (SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PM_PRODUCT_CODE = '" & lblProductCode.Text & "')  AND PV_S_COY_ID = '" & e.Item.Cells(EnumShoppingCart.icVendor).Text & "' ")
                lblLead.Text = IIf(strLead = "", 0, strLead)

                If strLead <> "" And strLead <> "0" Then
                    txtEstDate.Text = txtDate1.AddDays(CInt(strLead))
                    txtEstDate.ReadOnly() = True
                    txtEstDate.ForeColor = Color.Gray
                    txtEstDate.Enabled = False
                End If

                Select Case ViewState("mode")
                    Case "bc"
                End Select
            ElseIf ViewState("type") = "mod" Then
                Dim tem As Integer
                If IsDBNull(dv("ETD")) Then
                    tem = 1
                    lblLead.Text = 0
                Else
                    tem = dv("ETD")
                    lblLead.Text = dv("PRD_LEAD_TIME")
                    'lblLead.Text = IIf(Common.parseNull(dv("PRD_LEAD_TIME")) = "", 0, dv("PRD_LEAD_TIME"))

                End If
                If txtEstDate.ReadOnly = False Then txtEstDate.Text = txtDate1.AddDays(tem)
                Select Case ViewState("mode")
                    Case "bc"
                End Select
            End If

            Dim revWarranty As RegularExpressionValidator
            revWarranty = e.Item.FindControl("revWarranty")
            revWarranty.ValidationExpression = "\d{1,5}"
            revWarranty.ControlToValidate = "txtWarranty"
            revWarranty.ErrorMessage = "Invalid Warranty Terms (mths)"
            revWarranty.Display = ValidatorDisplay.Dynamic
            revWarranty.Text = "?"

            If ViewState("type") = "new" Then
                Select Case ViewState("mode")
                    Case "bc"
                    Case "cc"
                End Select
            ElseIf ViewState("type") = "mod" Then
                txtWarranty.Text = Common.parseNull(dv("WARRANTYTERMS"))
                'txtWarranty.CssClass = "lblnumerictxtbox"
                Select Case ViewState("mode")
                    Case "bc"
                    Case "cc"
                End Select
            End If

            'keep the Product Code of the selected record into an array
            aryProdCodeNew.Add(New String() {Common.parseNull(dv("PRODUCTCODE")), Common.parseNull(dv("PRODUCTDESC")), Common.parseNull(dv("UOM")), Common.parseNull(dv("COMMODITY")), Common.parseNull(dv("CDM_GROUP_INDEX"))})
            Dim txtRemark As TextBox
            txtRemark = e.Item.FindControl("txtRemark")
            txtRemark.Text = Common.parseNull(dv("REMARK"))
            txtRemark.Attributes.Add("onKeyDown", "limitText (this, 400);")

            intRow = intRow + 1

            If Session("strItem") IsNot Nothing Then
                strItem = Session("strItem")
                If (CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1) <= strItem.Count - 1 Then
                    cboGLCode.SelectedIndex = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(0)
                    txtQty.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(1)
                    txtPrice.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(2)
                    'txtBudget.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(3)
                    hidBudgetCode.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(4)
                    If Not dtBCM Is Nothing Then
                        Dim drTemp As DataRow()
                        drTemp = dtBCM.Select("Acct_Index=" & hidBudgetCode.Text)
                        If drTemp.Length > 0 Then
                            txtBudget.Text = Mid(drTemp(0)("Acct_List"), 1, 10)
                        End If
                    End If
                    'txtDelivery.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(5)
                    hidDelCode.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(6)
                    txtDelivery.Text = Mid(objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), Common.Parse(strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(6)), "D"), 1, 10)
                    txtEstDate.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(7)
                    txtWarranty.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(8)
                    txtRemark.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(9)

                    ddlDelivery.SelectedIndex = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(10)
                    ddl_stock.SelectedIndex = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(11)
                End If
            End If
        ElseIf e.Item.ItemType = ListItemType.Header Then
            If ViewState("BCM") <= 0 Then
                e.Item.Cells(EnumShoppingCart.icBudget).Visible = False
            End If
        End If
        'objGlobal = Nothing
    End Sub

    Sub Populate(ByVal i As Integer)
        Dim content As String = ""
        Dim nametypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=Commodity")
        Dim j As String

        If Len(CStr((i + 2))) = 1 Then
            j = "0" & CStr((i + 2))
        Else
            j = (i + 2)
        End If

        content &= "$(""#dtgShopping_ctl" & j & "_txtCommodity"").autocomplete(""" & nametypeahead & """, {" & vbCrLf & _
        "width: 250," & vbCrLf & _
        "scroll: true," & vbCrLf & _
        "selectFirst: false" & vbCrLf & _
        "});" & vbCrLf & _
        "$(""#dtgShopping_ctl" & j & "_txtCommodity"").result(function(event, data, formatted) {" & vbCrLf & _
        "if(data)" & vbCrLf & _
        "$(""#dtgShopping_ctl" & j & "_hidCommodity"").val(data[1]);" & vbCrLf & _
        "}); " & vbCrLf

        Session("content") &= content
    End Sub

    Sub buildjava()
        Dim typeahead As String
        typeahead = "<script language=""javascript"">" & vbCrLf & _
          "<!--" & vbCrLf & _
            "$(document).ready(function(){" & vbCrLf & _
            Session("content") & vbCrLf & _
            "});" & vbCrLf & _
            "-->" & vbCrLf & _
            "</script>"
        Session("typeahead") = typeahead
    End Sub

    Private Function rebindDDL(ByRef pddl As DropDownList) As String
        Dim i As Integer
        For i = 0 To pddl.Items.Count - 1
            pddl.Items(i).Value = Server.UrlEncode(pddl.Items(i).Value)
        Next
        rebindDDL = ""
    End Function

    Public Sub dtgshopping_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgShopping.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub

    Private Sub dtgShopping_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgShopping.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        strPageId = ViewState("strPageId")

        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")

            Dim objCheckGST As New GST
            ViewState("CheckGST") = objCheckGST.chkGSTCOD()
            If ViewState("type") = "new" Or ViewState("type") = "mod" Then
                If ViewState("CheckGST") = True Then
                    e.Item.Cells(EnumShoppingCart.icTax).Text = "GST Amount"
                Else
                    e.Item.Cells(EnumShoppingCart.icTax).Text = "Tax"
                End If
                Select Case ViewState("mode")
                    Case "bc"
                        e.Item.Cells(EnumShoppingCart.icPrice).Text = "Last Txn. Price"
                    Case "cc"
                        e.Item.Cells(EnumShoppingCart.icPrice).Text = "Contract Price"
                End Select
            End If
            objCheckGST = Nothing

            Dim i As Integer
            Dim str As String
            For i = EnumShoppingCart.icBudget To dtgShopping.Columns.Count - 2
                Dim cell As TableCell
                Dim lbl As New Label
                cell = e.Item.Cells(i)
                If i = EnumShoppingCart.icBudget Then
                    lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("PR", "Budget.aspx", "pageid=" & strPageId & "&id=txtBudget&mode=all") & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                ElseIf i = EnumShoppingCart.icDelivery Then
                    lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("Admin", "AddressMaster.aspx", "pageid=" & strPageId) & "&mod=P&type2=RPO&type=D&id=cboDelivery&mode=all', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                ElseIf i = EnumShoppingCart.icItemType Then
                    lbl.Text = e.Item.Cells(i).Text
                ElseIf i = EnumShoppingCart.icLead Then
                    lbl.Text = e.Item.Cells(i).Text
                ElseIf i = EnumShoppingCart.icEstDate Then
                    'lbl.Text = e.Item.Cells(i).Text
                    lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendarForAll.aspx", "id=txtEstDate") & "','cal','width=180,height=155,left=270,top=180')"">" & e.Item.Cells(i).Text & "</A>"
                ElseIf i = EnumShoppingCart.icWarranty Then
                    lbl.Text = e.Item.Cells(i).Text
                ElseIf i > EnumShoppingCart.icWarranty Then
                    str = dDispatcher.direct("Admin", "CustomFieldValue.aspx", "pageid=" & strPageId & "&mod=P&module=PR&value=" & dtgShopping.Columns(i).SortExpression & "&name=" & Server.UrlEncode(dtgShopping.Columns(i).HeaderText) & "&id=" & dtgShopping.Columns(i).SortExpression)
                    lbl.Text = "<A href='javascript:;' onclick=""window.open(&quot;" & str & "&quot;, &quot;&quot;, &quot;scrollbars=yes,resizable=yes,width=400,height=400,status=no,menubar=no&quot;);"">" & e.Item.Cells(i).Text & "</a>"
                End If
                cell.Controls.Add(lbl)
            Next
        End If

        dtgShopping.AllowSorting = False
        Grid_ItemCreated(dtgShopping, e)
    End Sub

    Private Sub addDataGridColumn()
        Dim objAdmin As New Admin
        Dim dsCustom As New DataSet
        Dim dsCustomField As New DataSet
        Dim i As Integer
        dvwCus = objAdmin.getCustomField("")

        If Not dvwCus Is Nothing Then
            intCnt = dvwCus.Count
            ReDim dvwCustom(intCnt)
            ReDim strCustomDefault(intCnt)

            '//dynamicly add template column
            For i = 0 To intCnt - 1
                Dim col As TemplateColumn = New TemplateColumn
                col.ItemTemplate = New dgTemplate(dvwCus.Table.Rows(i)("CF_FIELD_NO"), 2)
                col.HeaderText = dvwCus.Table.Rows(i)("CF_FIELD_NAME")
                col.SortExpression = dvwCus.Table.Rows(i)("CF_FIELD_NO")
                dtgShopping.Columns.AddAt(dtgShopping.Columns.Count - 1, col)

                dsCustomField = objAdmin.Populate_customField(dvwCus.Table.Rows(i)("CF_FIELD_NO"), "")
                dvwCustom(i) = dsCustomField.Tables(0).DefaultView
                strCustomDefault(i) = encodeCustomField(objAdmin.CustomAddr(dvwCus.Table.Rows(i)("CF_FIELD_NO")))
            Next
            Me.DynamicColumnAdded = True
        End If
        objAdmin = Nothing
    End Sub

    Private Sub fillAddress()
        Dim i As Integer
        Dim dvwBilling As DataView
        Dim dsBilling As New DataSet
        Dim objAdmin As New Admin

        dsBilling = objAdmin.PopulateAddr("B", objAdmin.user_Default_Add("B"), "", "")

        dvwBilling = dsBilling.Tables(0).DefaultView
        For i = 0 To dvwBilling.Count - 1
            If dvwBilling.Table.Rows(i)("AM_ADDR_CODE") = objAdmin.user_Default_Add("B") Then
                ViewState("BillAdd1") = Common.parseNull(dvwBilling.Table.Rows(i)("AM_ADDR_LINE1"))
                ViewState("BillAdd2") = Common.parseNull(dvwBilling.Table.Rows(i)("AM_ADDR_LINE2"))
                ViewState("BillAdd3") = Common.parseNull(dvwBilling.Table.Rows(i)("AM_ADDR_LINE3"))
                ViewState("BillPostCode") = Common.parseNull(dvwBilling.Table.Rows(i)("AM_POSTCODE"))
                ViewState("BillCity") = Common.parseNull(dvwBilling.Table.Rows(i)("AM_CITY"))
                ViewState("Country") = Common.parseNull(dvwBilling.Table.Rows(i)("AM_COUNTRY"))
                'ViewState("State") = Common.parseNull(dvwBilling.Table.Rows(i)("AM_COUNTRY"))
                ViewState("State") = Common.parseNull(dvwBilling.Table.Rows(i)("AM_STATE"))
                Exit For
            End If
        Next
    End Sub

    Private Function bindPR() As DataSet
        Dim ds As New DataSet
        Dim objAdmin As New Admin

        Dim dtMaster As New DataTable
        dtMaster.Columns.Add("PRNo", Type.GetType("System.String"))
        dtMaster.Columns.Add("ReqName", Type.GetType("System.String"))
        dtMaster.Columns.Add("Attn", Type.GetType("System.String"))
        dtMaster.Columns.Add("ReqPhone", Type.GetType("System.String"))
        dtMaster.Columns.Add("InternalRemark", Type.GetType("System.String"))
        dtMaster.Columns.Add("ExternalRemark", Type.GetType("System.String"))
        dtMaster.Columns.Add("PrintCustom", Type.GetType("System.String"))
        dtMaster.Columns.Add("PrintRemark", Type.GetType("System.String"))
        dtMaster.Columns.Add("Urgent", Type.GetType("System.String"))
        dtMaster.Columns.Add("ShipmentTerm", Type.GetType("System.String"))
        dtMaster.Columns.Add("ShipmentMode", Type.GetType("System.String"))
        dtMaster.Columns.Add("PRType", Type.GetType("System.String"))
        dtMaster.Columns.Add("PRCost", Type.GetType("System.Double"))

        dtMaster.Columns.Add("BillAddrCode", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrLine1", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrLine2", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrLine3", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrPostCode", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrState", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrCity", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrCountry", Type.GetType("System.String"))

        dtMaster.Columns.Add("PaymentType", Type.GetType("System.String"))
        dtMaster.Columns.Add("PaymentTerm", Type.GetType("System.String"))
        dtMaster.Columns.Add("CurrencyCode", Type.GetType("System.String"))

        Dim dtr As DataRow
        Dim dtrd As DataRow
        dtr = dtMaster.NewRow()
        dtr("PRNo") = lblPRNo.Text 'ViewState("prid")
        dtr("ReqName") = txtRequestedName.Text
        dtr("ReqPhone") = txtRequestedContact.Text
        dtr("Attn") = txtAttention.Text
        dtr("InternalRemark") = txtInternal.Text
        dtr("ExternalRemark") = txtExternal.Text
        dtr("PrintCustom") = IIf(chkCustomPR.Checked, "1", "0")
        dtr("PrintRemark") = IIf(chkRemarkPR.Checked, "1", "0")
        dtr("Urgent") = IIf(chkUrgent.Checked, "1", "0")
        ' Set Both ShipmentTerm, ShipmentMode to 99 (Default)
        dtr("ShipmentTerm") = "99"
        dtr("ShipmentMode") = "99"
        dtr("PaymentType") = cboPayMethod.SelectedItem.Value
        dtr("PaymentTerm") = cboPayTerm.SelectedItem.Value
        dtr("CurrencyCode") = cboCurr.SelectedItem.Value

        dtr("PRType") = ""

        fillAddress()
        dtr("BillAddrCode") = objAdmin.user_Default_Add("B")
        dtr("BillAddrLine1") = ViewState("BillAdd1")
        dtr("BillAddrLine2") = ViewState("BillAdd2")
        dtr("BillAddrLine3") = ViewState("BillAdd3")
        dtr("BillAddrPostCode") = ViewState("BillPostCode")
        dtr("BillAddrState") = ViewState("State")
        dtr("BillAddrCity") = ViewState("BillCity")
        dtr("BillAddrCountry") = ViewState("Country")

        If IsDBNull(dtr("BillAddrCode")) = True Then
            blnValidBill = False
        Else
            blnValidBill = True
        End If

        If ViewState("type") = "new" Then
            Select Case ViewState("mode")
                Case "bc"
                Case "cc"
                    dtr("PRType") = "CC"
            End Select
        ElseIf ViewState("type") = "mod" Then
            Select Case ViewState("mode")
                Case "bc"
                Case "cc"
                    dtr("PRType") = "CC"
            End Select
        End If

        If hidCost.Value = "" Then
            hidCost.Value = "0"
        End If

        Dim dtDetails As New DataTable
        dtDetails.Columns.Add("Line", Type.GetType("System.Int32"))
        dtDetails.Columns.Add("ProductCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("VendorItemCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("ProductDesc", Type.GetType("System.String"))
        dtDetails.Columns.Add("Qty", Type.GetType("System.Double"))
        dtDetails.Columns.Add("Commodity", Type.GetType("System.String"))
        dtDetails.Columns.Add("UOM", Type.GetType("System.String"))
        dtDetails.Columns.Add("Currency", Type.GetType("System.String"))
        dtDetails.Columns.Add("UnitCost", Type.GetType("System.Double"))
        dtDetails.Columns.Add("GST", Type.GetType("System.String"))
        dtDetails.Columns.Add("GSTRate", Type.GetType("System.String"))
        'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH
        dtDetails.Columns.Add("GSTTaxCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("DeliveryAddr", Type.GetType("System.String"))
        dtDetails.Columns.Add("ETD", Type.GetType("System.Int32"))
        dtDetails.Columns.Add("WarrantyTerms", Type.GetType("System.Int32"))
        dtDetails.Columns.Add("Remark", Type.GetType("System.String"))
        dtDetails.Columns.Add("Source", Type.GetType("System.String"))
        dtDetails.Columns.Add("VendorID", Type.GetType("System.String"))
        dtDetails.Columns.Add("GLCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("CategoryCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("CDGroup", Type.GetType("System.String"))
        dtDetails.Columns.Add("AcctIndex", Type.GetType("System.String"))

        dtDetails.Columns.Add("AssetGroup", Type.GetType("System.String"))

        dtDetails.Columns.Add("LeadTime", Type.GetType("System.String"))
        dtDetails.Columns.Add("ItemType", Type.GetType("System.String"))
        dtDetails.Columns.Add("DeliveryTerm", Type.GetType("System.String"))
        dtDetails.Columns.Add("Oversea", Type.GetType("System.String"))

        Dim dgItem As DataGridItem
        For Each dgItem In dtgShopping.Items
            blnItem = True
            dtrd = dtDetails.NewRow

            dtrd("Line") = dgItem.Cells(EnumShoppingCart.icNo).Text
            dtrd("ProductCode") = dgItem.Cells(EnumShoppingCart.icProductCode).Text
            dtrd("VendorItemCode") = CType(dgItem.FindControl("lblVENDORITEMCODE"), Label).Text
            dtrd("ProductDesc") = CType(dgItem.FindControl("lblProductDesc"), TextBox).Text 'dgItem.Cells(EnumShoppingCart.icItemDesc).Text
            'dtrd("GLCODE") = CType(dgItem.FindControl("lblGLCode"), Label).Text
            'dtrd("CategoryCode") = CType(dgItem.FindControl("lblCategoryCode"), Label).Text
            dtrd("CategoryCode") = IIf(dgItem.Cells(EnumShoppingCart.icCategoryCode).Text = "&nbsp;", "", dgItem.Cells(EnumShoppingCart.icCategoryCode).Text)
            dtrd("GLCode") = IIf(dgItem.Cells(EnumShoppingCart.icGLCode).Text = "&nbsp;", "", dgItem.Cells(EnumShoppingCart.icGLCode).Text)
            dtrd("CDGroup") = CType(dgItem.FindControl("lblCDGroup"), Label).Text

            'Dim ddl_comm As New DropDownList
            'ddl_comm = dgItem.FindControl("ddl_comm")
            'dtrd("Commodity") = ddl_comm.SelectedItem.Value

            'Dim txtCommodity As TextBox
            'txtCommodity = dgItem.FindControl("txtCommodity")

            Dim cboGLCode, cboCategoryCode As DropDownList
            cboGLCode = dgItem.FindControl("cboGLCode")
            If cboGLCode.Items.Count > 0 Then
                dtrd("GLCode") = cboGLCode.SelectedItem.Value
            Else
                dtrd("GLCode") = ""
            End If

            cboCategoryCode = dgItem.FindControl("cboCategoryCode")
            If cboCategoryCode.Items.Count > 0 Then
                dtrd("CategoryCode") = cboCategoryCode.SelectedItem.Value
            Else
                dtrd("CategoryCode") = ""
            End If

            Dim hidCommodity As HtmlInputHidden
            hidCommodity = dgItem.FindControl("hidCommodity")
            dtrd("Commodity") = hidCommodity.Value

            Dim txtCommodity As TextBox
            txtCommodity = dgItem.FindControl("txtCommodity")
            dtrd("Commodity") = Val(objDB.GetVal("SELECT CT_ID FROM COMMODITY_TYPE WHERE CT_NAME = '" & Common.Parse(txtCommodity.Text) & "'"))


            dtrd("UOM") = CType(dgItem.FindControl("lblUOM"), Label).Text
            dtrd("Currency") = CType(dgItem.FindControl("lblCurrency"), Label).Text

            If ViewState("BCM") > 0 Then
                Dim hidBudgetCode As TextBox
                hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                If hidBudgetCode.Text <> "" And hidBudgetCode.Text <> "&nbsp;" Then
                    dtrd("AcctIndex") = hidBudgetCode.Text
                Else
                    dtrd("AcctIndex") = ""
                End If
            Else
                dtrd("AcctIndex") = ""
            End If

            Dim txtQty As TextBox ' HtmlInputText
            txtQty = dgItem.FindControl("txtQty")
            'If IsNumeric(txtQty.Text) And Regex.IsMatch(Trim(txtQty.Text), "(?!^0*$)^\d{1,5}?$") Then
            If IsNumeric(txtQty.Text) And Regex.IsMatch(Trim(txtQty.Text), "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$") Then
                dtrd("Qty") = txtQty.Text
            Else
                dtrd("Qty") = 1.0
                blnValid = False
            End If

            Dim txtPrice As TextBox
            txtPrice = dgItem.FindControl("txtPrice")
            If IsNothing(txtPrice) Then
                dtrd("UnitCost") = dgItem.Cells(EnumShoppingCart.icPrice).Text
            Else
                dtrd("UnitCost") = IIf(txtPrice.Text = "", "0", txtPrice.Text)
            End If

            dtrd("Source") = dgItem.Cells(EnumShoppingCart.icSource).Text
            dtrd("VendorID") = dgItem.Cells(EnumShoppingCart.icVendor).Text

            ' _Yap Get Cost
            Dim hidTaxPerc As TextBox
            Dim hidGSTRate As TextBox
            Dim txtAmount, txtGST As TextBox, txtGSTAmt As TextBox
            hidTaxPerc = dgItem.FindControl("hidTaxPerc")
            hidGSTRate = dgItem.FindControl("hidGSTRate")
            txtAmount = dgItem.FindControl("txtAmount")

            '2015-06-17: CH: Rounding issue (Prod issue)
            'txtAmount.Text = CDbl(dtrd("Qty") * dtrd("UnitCost"))
            txtAmount.Text = CDec(Format(dtrd("Qty") * dtrd("UnitCost"), "###0.00"))
            txtGST = dgItem.FindControl("txtGST")
            txtGSTAmt = dgItem.FindControl("txtGSTAmt")
            If txtGST.Text = "" Then
                dtrd("GST") = 0
            Else
                dtrd("GST") = CDbl(hidTaxPerc.Text)
            End If

            dtrd("GSTRate") = hidGSTRate.Text

            'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH
            Dim lblTaxCode As Label
            lblTaxCode = dgItem.FindControl("lblTaxCode")
            dtrd("GSTTaxCode") = lblTaxCode.Text

            '2015-06-17: CH: Rounding issue (Prod issue)
            'ViewState("PRCost") = ViewState("PRCost") + CDbl(hidTaxPerc.Text * txtAmount.Text / 100) + (dtrd("Qty") * dtrd("UnitCost"))
            ViewState("PRCost") = ViewState("PRCost") + CDec(Format(hidTaxPerc.Text * txtAmount.Text / 100, "###0.00")) + CDec(txtAmount.Text)

            Dim txtEstDate As TextBox
            txtEstDate = dgItem.FindControl("txtEstDate")
            If IsDate(txtEstDate.Text) Then
                Dim txtDate1 As Date = CType(lblDate.Text, Date)
                Dim txtDate2 As Date = CType(txtEstDate.Text, Date)
                Dim diffDay As TimeSpan

                diffDay = txtDate2.Subtract(txtDate1)
                dtrd("ETD") = CInt(diffDay.Days)
            Else
                dtrd("ETD") = 0
                blnValid = False
            End If

            Dim txtWarranty As TextBox
            txtWarranty = dgItem.FindControl("txtWarranty")
            If IsNumeric(txtWarranty.Text) Then
                dtrd("WarrantyTerms") = CInt(txtWarranty.Text)
            Else
                dtrd("WarrantyTerms") = 0
            End If

            Dim txtRemark As TextBox
            txtRemark = dgItem.FindControl("txtRemark")
            dtrd("Remark") = txtRemark.Text

            Dim hidDelCode As TextBox
            hidDelCode = dgItem.FindControl("hidDelCode")
            dtrd("DeliveryAddr") = hidDelCode.Text

            Dim Asset As New PurchaseOrder_Buyer
            If Asset.AssetGroupMstr = True Then
                Dim cboAssetGroup As DropDownList
                cboAssetGroup = dgItem.FindControl("cboAssetGroup")
                dtrd("AssetGroup") = cboAssetGroup.SelectedItem.Value
            Else
                dtrd("AssetGroup") = ""
            End If

            Dim lblLead As Label
            lblLead = dgItem.FindControl("lblLead")
            dtrd("LeadTime") = lblLead.Text

            Dim ddl_stock As DropDownList
            ddl_stock = dgItem.FindControl("ddl_stock")
            dtrd("ItemType") = ddl_stock.SelectedItem.Value

            Dim ddlDelivery As DropDownList
            ddlDelivery = dgItem.FindControl("ddlDelivery")
            dtrd("DeliveryTerm") = ddlDelivery.SelectedItem.Value

            Dim lblOversea As Label
            lblOversea = dgItem.FindControl("lblOversea")
            dtrd("Oversea") = lblOversea.Text

            dtDetails.Rows.Add(dtrd)

        Next
        dtr("PRCost") = CDbl(ViewState("PRCost"))
        dtMaster.Rows.Add(dtr)
        ds.Tables.Add(dtMaster)
        ds.Tables.Add(dtDetails)
        'ViewState("PRCost") = 0

        'If Session("Env") <> "FTN" Then
        '    ' Columns for PR_CUSTOM_FIELD_MSTR
        '    Dim dtCustomMaster As New DataTable
        '    dvwCus = objAdmin.getCustomField("")
        '    dtCustomMaster.Columns.Add("FieldNo", Type.GetType("System.Int32"))
        '    dtCustomMaster.Columns.Add("FieldName", Type.GetType("System.String"))
        '    If Not dvwCus Is Nothing Then
        '        For i = 0 To dvwCus.Count - 1
        '            dtr = dtCustomMaster.NewRow
        '            dtr("FieldNo") = dvwCus.Table.Rows(i)("CF_FIELD_NO")
        '            dtr("FieldName") = dvwCus.Table.Rows(i)("CF_FIELD_NAME")
        '            dtCustomMaster.Rows.Add(dtr)
        '        Next
        '    End If
        '    objAdmin = Nothing
        '    ds.Tables.Add(dtCustomMaster)

        '    ' Columns for PR_CUSTOM_FIELD_DETAILS
        '    Dim dtCustomDetails As New DataTable
        '    dtCustomDetails.Columns.Add("Line", Type.GetType("System.Int32"))
        '    dtCustomDetails.Columns.Add("FieldNo", Type.GetType("System.Int32"))
        '    dtCustomDetails.Columns.Add("FieldValue", Type.GetType("System.String"))
        '    If Not dvwCus Is Nothing Then
        '        For i = 0 To dvwCus.Count - 1
        '            For Each dgItem In dtgShopping.Items
        '                dtr = dtCustomDetails.NewRow
        '                dtr("Line") = dgItem.Cells(EnumShoppingCart.icNo).Text
        '                dtr("FieldNo") = dvwCus.Table.Rows(i)("CF_FIELD_NO")

        '                Dim cboCustom As DropDownList
        '                cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)
        '                dtr("FieldValue") = decodeCustomField(cboCustom.SelectedItem.Value)
        '                dtCustomDetails.Rows.Add(dtr)
        '            Next
        '        Next
        '    End If
        '    ds.Tables.Add(dtCustomDetails)
        'End If
        ' Columns for PR_CUSTOM_FIELD_MSTR
        Dim dtCustomMaster As New DataTable
        dvwCus = objAdmin.getCustomField("")
        dtCustomMaster.Columns.Add("FieldNo", Type.GetType("System.Int32"))
        dtCustomMaster.Columns.Add("FieldName", Type.GetType("System.String"))
        If Not dvwCus Is Nothing Then
            For i = 0 To dvwCus.Count - 1
                dtr = dtCustomMaster.NewRow
                dtr("FieldNo") = dvwCus.Table.Rows(i)("CF_FIELD_NO")
                dtr("FieldName") = dvwCus.Table.Rows(i)("CF_FIELD_NAME")
                dtCustomMaster.Rows.Add(dtr)
            Next
        End If
        objAdmin = Nothing
        ds.Tables.Add(dtCustomMaster)

        ' Columns for PR_CUSTOM_FIELD_DETAILS
        Dim dtCustomDetails As New DataTable
        dtCustomDetails.Columns.Add("Line", Type.GetType("System.Int32"))
        dtCustomDetails.Columns.Add("FieldNo", Type.GetType("System.Int32"))
        dtCustomDetails.Columns.Add("FieldValue", Type.GetType("System.String"))
        If Not dvwCus Is Nothing Then
            For i = 0 To dvwCus.Count - 1
                For Each dgItem In dtgShopping.Items
                    dtr = dtCustomDetails.NewRow
                    dtr("Line") = dgItem.Cells(EnumShoppingCart.icNo).Text
                    dtr("FieldNo") = dvwCus.Table.Rows(i)("CF_FIELD_NO")

                    Dim cboCustom As DropDownList
                    cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)
                    dtr("FieldValue") = decodeCustomField(cboCustom.SelectedItem.Value)
                    dtCustomDetails.Rows.Add(dtr)
                Next
            Next
        End If
        ds.Tables.Add(dtCustomDetails)
        bindPR = ds
    End Function

    Private Sub SavePR()
        Dim dsPR As New DataSet
        Dim objPR As New PR
        Dim objPR_Ext As New PR_Ext
        Dim strMsg As String = ""
        'Dim blnValid = True
        blnValid = True

        If ViewState("type") = "new" Then
            Select Case ViewState("mode")
                Case "bc"
                Case "cc"
            End Select
        ElseIf ViewState("type") = "mod" Then
            If Session("urlreferer") <> "Dashboard" Then
                Session("urlreferer") = "PRAll"
            End If
            Select Case ViewState("mode")
                Case "bc"
                Case "cc"
            End Select
        End If

        If validateDatagrid(strMsg) Then
            dsPR = bindPR()
            If ViewState("type") = "new" Then
                Dim strNewPR As String = ""
                Dim intMsg As Integer

                If blnValid And blnValidBill Then
                    intMsg = objPR_Ext.insertPR(dsPR, strNewPR, Session("NonFTN"))
                    objPR = Nothing

                    Select Case intMsg
                        Case WheelMsgNum.Save
                            If Session("urlreferer") = "PRAll" Then
                                Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been created.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                            ElseIf Session("urlreferer") = "Dashboard" Then
                                Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been created.", dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId))
                            ElseIf Session("urlreferer") = "BuyerCatSearch" Then
                                Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been created.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                            ElseIf Session("urlreferer") = "ConCatSearch" Then
                                Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been created.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                            End If
                            lblPRNo.Text = strNewPR
                            'cmdRaise.Visible = "False"
                            cmdRaise.Enabled = False
                            ViewState("blnCmdRaise") = False
                        Case WheelMsgNum.NotSave
                            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                        Case WheelMsgNum.Duplicate
                            Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)
                        Case -1
                            Common.NetMsgbox(Me, "Company is currently inactive.", MsgBoxStyle.Information)
                        Case -2
                            Common.NetMsgbox(Me, "Vendor is being deleted.", MsgBoxStyle.Information)
                    End Select
                Else
                    If blnValidBill = False Then
                        Common.NetMsgbox(Me, objGLO.GetErrorMessage("00031"), MsgBoxStyle.Information)
                    End If

                End If

                Select Case ViewState("mode")
                    Case "bc"
                End Select
            ElseIf ViewState("type") = "mod" Then
                If blnValid And blnValidBill Then
                    objPR_Ext.updatePR(dsPR, Session("NonFTN"))
                    objPR = Nothing

                    If Session("urlreferer") = "PRAll" Then
                        Common.NetMsgbox(Me, "Purchase Request Number " & lblPRNo.Text & " has been updated.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "Dashboard" Then
                        Common.NetMsgbox(Me, "Purchase Request Number " & lblPRNo.Text & " has been updated.", dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "BuyerCatSearch" Then
                        Common.NetMsgbox(Me, "Purchase Request Number " & lblPRNo.Text & " has been updated.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "ConCatSearch" Then
                        Common.NetMsgbox(Me, "Purchase Request Number " & lblPRNo.Text & " has been updated.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                    End If
                Else
                    If blnValidBill = False Then
                        Common.NetMsgbox(Me, objGLO.GetErrorMessage("00031"), MsgBoxStyle.Information)
                    End If
                End If

                Select Case ViewState("mode")
                    Case "bc"
                End Select
            End If
        Else
            'Stage 3 Bug fix (GST-0024) - 08/07/2015 - CH
            savePRItemInArray()

            If strMsg <> "" Then
                lblMsg.Text = strMsg
            Else
                lblMsg.Text = ""
            End If
        End If
    End Sub

    Private Sub cmdRaise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRaise.Click
        SavePR()
    End Sub

    Sub AddRow(ByVal intCell As Integer, ByVal strLabel As String, ByVal dblTotal As Double, ByVal blnShowGST As Boolean)

    End Sub

    Sub addCell(ByRef row As DataGridItem)
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        If ViewState("type") = "new" Or ViewState("type") = "mod" Then
            Select Case ViewState("mode")
                Case "bc"
                    Dim strFileName As String
                    Dim strscript As New System.Text.StringBuilder
                    strscript.Append("<script language=""javascript"">")
                    'strFileName = dDispatcher.direct("PR", "Add_Buyer_Catalogue_Item.aspx", "pageid=" & strPageId & "&RFQ_Name=&RFQ_Cur_value=&RFQ_Cur_Text=")
                    strFileName = dDispatcher.direct("PR", "Add_Buyer_Catalogue_Item.aspx", "pageid=" & strPageId & "&oversea=" & ViewState("Oversea") & "&itemtype=" & ViewState("ItemType") & "&vendor=" & ViewState("Vendor") & "&mode=" & ViewState("mode") & "")
                    strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
                    strscript.Append("ShowDialog('" & dDispatcher.direct("RFQ", "Dialog.aspx", "page=" & strFileName) & "','700px');")
                    strscript.Append("document.getElementById('btnHidden1').click();")
                    strscript.Append("</script>")
                    RegisterStartupScript("script13", strscript.ToString())
                Case "cc"
                    Dim strFileName As String
                    Dim strscript As New System.Text.StringBuilder
                    Dim strVen As String
                    strscript.Append("<script language=""javascript"">")
                    'strFileName = dDispatcher.direct("PR", "Add_Buyer_Catalogue_Item.aspx", "pageid=" & strPageId & "&RFQ_Name=&RFQ_Cur_value=&RFQ_Cur_Text=")
                    strFileName = dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId & "&frm=Concat&show=no&oversea=" & ViewState("Oversea") & "&itemtype=" & ViewState("ItemType") & "&vendor=" & "" & "&curr=" & cboCurr.SelectedValue & "&chkvendor=no")
                    strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
                    strscript.Append("ShowDialog('" & dDispatcher.direct("RFQ", "Dialog.aspx", "page=" & strFileName) & "','560px');")
                    strscript.Append("document.getElementById('btnHidden1').click();")
                    strscript.Append("</script>")
                    RegisterStartupScript("script13", strscript.ToString())
            End Select
        End If
    End Sub

    Private Sub updatePRDIdx()
        Dim dgItem As DataGridItem
        Dim objPR As New PR
        Dim sProductCode As String = ""
        Dim sProductDesc As String = ""
        Dim strSQL As String
        Dim sPrdIndex As Integer
        Dim strAryQuery(0) As String

        'Update the grid PRDIndex Sync with DB
        For Each dgItem In dtgShopping.Items
            If CType(dgItem.FindControl("lblProductCode"), Label).Text = "&nbsp;" Or CType(dgItem.FindControl("lblProductCode"), Label).Text = "" Then
                sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
                sProductDesc = CType(dgItem.FindControl("lblProductDesc"), TextBox).Text
            Else
                sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
            End If

            sPrdIndex = CInt(dgItem.Cells(EnumShoppingCart.icNo).Text)

            strSQL = objPR.updatePRDIndex(hidNewPR.Value, sProductCode, sPrdIndex, sProductDesc)
            Common.Insert2Ary(strAryQuery, strSQL)
            objDB.BatchExecute(strAryQuery)
        Next
    End Sub

    Private Sub cmdRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemove.Click
        Dim dgItem As DataGridItem
        Dim objPR As New PR
        Dim objPO1 As New PurchaseOrder_Buyer
        Dim objShopping As New ShoppingCart
        Dim sProductCode As String = ""
        Dim sFullClientId As String, sClientId As String
        Dim sProductDesc As String = ""
        Dim chkItem As CheckBox
        Dim i As Integer = 0
        Dim strAryQuery(0) As String
        Dim strSQL As String = ""
        Dim strSQL2 As String = ""
        Dim sPrdIndex As Integer
        Dim InternalCount As Integer = 0
        ' Update the grid PRDIndex Sync with DB
        ' This must run and update first
        updatePRDIdx()

        For Each dgItem In dtgShopping.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then

                If CType(dgItem.FindControl("lblProductCode"), Label).Text = "&nbsp;" Or CType(dgItem.FindControl("lblProductCode"), Label).Text = "" Then
                    sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
                    sProductDesc = CType(dgItem.FindControl("lblProductDesc"), TextBox).Text
                    sFullClientId = CType(dgItem.FindControl("lblProductCode"), Label).ClientID
                Else
                    sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
                    sFullClientId = CType(dgItem.FindControl("lblProductCode"), Label).ClientID
                End If

                sPrdIndex = CInt(dgItem.Cells(EnumShoppingCart.icNo).Text)

                If ViewState("type") = "new" Then
                    strSQL = objPR.deletePRItemSQL(hidNewPR.Value, sProductCode, sPrdIndex, sProductDesc)
                    Select Case ViewState("mode")
                        Case "bc"
                    End Select
                ElseIf ViewState("type") = "mod" Then
                    strSQL = objPR.deletePRItemSQL(hidNewPR.Value, sProductCode, sPrdIndex, sProductDesc)
                    strSQL2 = objPR.updatePRHeaderSQL(hidNewPR.Value, sProductCode, sPrdIndex, sProductDesc)
                    Common.Insert2Ary(strAryQuery, strSQL2)
                    Select Case ViewState("mode")
                        Case "pr"
                            ''Case "rfq"
                    End Select
                End If

                sClientId = Mid(sFullClientId, InStr(sFullClientId, "_") + 1, InStr(Mid(sFullClientId, InStr(sFullClientId, "_") + 1), "_") - 1) & "|"
                hidClientId.Value = hidClientId.Value.Replace(sClientId, "")
                hidTotalClientId.Value = hidTotalClientId.Value - 1
                Dim GetPR As Boolean
                GetPR = objPR.get_PR(hidNewPR.Value, sProductCode, sPrdIndex, sProductDesc)
                If GetPR = False Then
                    Session("ProdList") = objShopping.RemovePRProductCodeFromList(sProductCode, Session("ProdList"), i, sProductDesc, InternalCount)
                End If
                Common.Insert2Ary(strAryQuery, strSQL)
            End If
            i = i + 1
        Next

        InternalCount = 0

        If objDB.BatchExecute(strAryQuery) Then
            Common.NetMsgbox(Me, "Item(s) Deleted.", MsgBoxStyle.Information)
            Dim dsDelivery As New DataSet
            Dim objAdmin As New Admin
            Dim objBudget As New BudgetControl
            dsDelivery = objAdmin.PopulateAddr("D", "", "", "")
            dvwDelivery = dsDelivery.Tables(0).DefaultView
            strDeliveryDefault = objAdmin.user_Default_Add("D")
            Session("CurrentScreen") = "RemoveItem"
            'If Session("Env") <> "FTN" Then
            '    ' BCM 

            '    'Update the grid PRDIndex Sync with DB
            '    If ViewState("type") = "new" Then
            '        Select Case ViewState("mode")
            '            Case "bc"
            '            Case "cc"
            '        End Select
            '    ElseIf ViewState("type") = "mod" Then
            '        Dim j As Integer = 0
            '        For Each dgItem In dtgShopping.Items
            '            chkItem = dgItem.FindControl("chkSelection")
            '            If chkItem.Checked Then

            '                If CType(dgItem.FindControl("lblProductCode"), Label).Text = "&nbsp;" Or CType(dgItem.FindControl("lblProductCode"), Label).Text = "" Then
            '                    sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
            '                    sProductDesc = CType(dgItem.FindControl("lblProductDesc"), Label).Text
            '                Else
            '                    sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
            '                End If

            '                sPrdIndex = CInt(dgItem.Cells(EnumShoppingCart.icNo).Text)

            '                Dim GetPr As Boolean
            '                GetPr = objPR.get_PR(hidNewPR.Value, sProductCode, sPrdIndex, sProductDesc)
            '                If GetPr = False Then
            '                    Session("ProdList") = objShopping.RemovePRProductCodeFromList(sProductCode, Session("ProdList"), j, sProductDesc, InternalCount)
            '                End If
            '            End If
            '            j = j + 1
            '        Next
            '        updatePRDIdx()
            '        Select Case ViewState("mode")
            '            Case "bc"
            '            Case "cc"
            '        End Select
            '    End If

            '    Bindgrid()
            '    hidTotal.Value = ""
            '    hid2.Value = ""
            '    objAdmin = Nothing
            '    objBudget = Nothing
            'Else
            '    'Update the grid PRDIndex Sync with DB
            '    If ViewState("type") = "new" Then
            '        Select Case ViewState("mode")
            '            Case "bc"
            '            Case "cc"
            '        End Select
            '    ElseIf ViewState("type") = "mod" Then
            '        Dim j As Integer = 0
            '        For Each dgItem In dtgShopping.Items
            '            chkItem = dgItem.FindControl("chkSelection")
            '            If chkItem.Checked Then

            '                If CType(dgItem.FindControl("lblProductCode"), Label).Text = "&nbsp;" Or CType(dgItem.FindControl("lblProductCode"), Label).Text = "" Then
            '                    sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
            '                    sProductDesc = CType(dgItem.FindControl("lblProductDesc"), Label).Text
            '                Else
            '                    sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
            '                End If

            '                sPrdIndex = CInt(dgItem.Cells(EnumShoppingCart.icNo).Text)

            '                Dim GetPr As Boolean
            '                GetPr = objPR.get_PR(hidNewPR.Value, sProductCode, sPrdIndex, sProductDesc)
            '                If GetPr = False Then
            '                    Session("ProdList") = objShopping.RemovePRProductCodeFromList(sProductCode, Session("ProdList"), j, sProductDesc, InternalCount)
            '                End If
            '            End If
            '            j = j + 1
            '        Next
            '        updatePRDIdx()
            '        Select Case ViewState("mode")
            '            Case "bc"
            '            Case "cc"
            '        End Select
            '    End If

            '    Bindgrid()
            'End If
            ' BCM 

            'Update the grid PRDIndex Sync with DB
            If ViewState("type") = "new" Then
                Select Case ViewState("mode")
                    Case "bc"
                    Case "cc"
                End Select
            ElseIf ViewState("type") = "mod" Then
                Dim j As Integer = 0
                For Each dgItem In dtgShopping.Items
                    chkItem = dgItem.FindControl("chkSelection")
                    If chkItem.Checked Then

                        If CType(dgItem.FindControl("lblProductCode"), Label).Text = "&nbsp;" Or CType(dgItem.FindControl("lblProductCode"), Label).Text = "" Then
                            sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
                            sProductDesc = CType(dgItem.FindControl("lblProductDesc"), TextBox).Text
                        Else
                            sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
                        End If

                        sPrdIndex = CInt(dgItem.Cells(EnumShoppingCart.icNo).Text)

                        Dim GetPr As Boolean
                        GetPr = objPR.get_PR(hidNewPR.Value, sProductCode, sPrdIndex, sProductDesc)
                        If GetPr = False Then
                            Session("ProdList") = objShopping.RemovePRProductCodeFromList(sProductCode, Session("ProdList"), j, sProductDesc, InternalCount)
                        End If
                    End If
                    j = j + 1
                Next
                updatePRDIdx()
                Select Case ViewState("mode")
                    Case "bc"
                    Case "cc"
                End Select
            End If

            'Dim dtrd As DataRow
            'Dim chkItem As CheckBox
            Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark As TextBox
            Dim txtBudget, txtDelivery As Label
            Dim cboGLCode, ddlDelivery, ddl_stock As DropDownList
            Dim strItem, strItemCust As New ArrayList()
            'Dim dgItem As DataGridItem
            Dim k As Integer = 0
            Dim strItemHead As New ArrayList()

            strItemHead.Add(New String() {txtAttention.Text, txtInternal.Text, txtExternal.Text, chkUrgent.Checked, cboPayTerm.SelectedIndex, cboPayMethod.SelectedIndex, cboCurr.SelectedIndex})

            For Each dgItem In dtgShopping.Items
                txtQty = dgItem.FindControl("txtQty")
                cboGLCode = dgItem.FindControl("cboGLCode")
                txtPrice = dgItem.FindControl("txtPrice")
                txtBudget = dgItem.FindControl("txtBudget")
                hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                txtDelivery = dgItem.FindControl("txtDelivery")
                hidDelCode = dgItem.FindControl("hidDelCode")
                txtEstDate = dgItem.FindControl("txtEstDate")
                txtWarranty = dgItem.FindControl("txtWarranty")
                txtRemark = dgItem.FindControl("txtRemark")

                ddlDelivery = dgItem.FindControl("ddlDelivery")
                ddl_stock = dgItem.FindControl("ddl_stock")

                chkItem = dgItem.FindControl("chkSelection")
                If chkItem.Checked Then
                Else
                    strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, ddlDelivery.SelectedIndex, ddl_stock.SelectedIndex})
                End If

                'Dim objAdmin As New Admin

                dvwCus = objAdmin.getCustomField("")

                If Not dvwCus Is Nothing Then
                    For i = 0 To dvwCus.Count - 1
                        'For Each dgItem2 In dtgShopping.Items
                        Dim cboCustom As DropDownList
                        cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                        If chkItem.Checked Then
                        Else
                            strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                        End If
                        'Next
                    Next
                End If

                k += 1
            Next
            Session("strItem") = strItem
            Session("strItemCust") = strItemCust
            Session("strItemHead") = strItemHead

            Bindgrid()
            hidTotal.Value = ""
            hid2.Value = ""
            objAdmin = Nothing
            objBudget = Nothing
        End If
        objPR = Nothing

        If ViewState("type") = "new" Or ViewState("type") = "mod" Then
            Response.Redirect(Session("RaisePRURL"))
            Select Case ViewState("mode")
                Case "bc"
                Case "cc"
            End Select
        End If
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

        Dim objPR As New PR
        objPR.deletePR(hidNewPR.Value)

        If Session("urlreferer") = "BuyerCatSearch" Then
            Common.NetMsgbox(Me, "PR Deleted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "caller=buyer&pageid=" & strPageId), MsgBoxStyle.Information)
        ElseIf Session("urlreferer") = "ConCatSearch" Then
            Common.NetMsgbox(Me, "PR Deleted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "caller=buyer&pageid=" & strPageId), MsgBoxStyle.Information)
        ElseIf Session("urlreferer") = "PRAll" Then
            Common.NetMsgbox(Me, "PR Deleted.", dDispatcher.direct("PR", "SearchPR_all.aspx", "caller=buyer&pageid=" & strPageId), MsgBoxStyle.Information)
        ElseIf Session("urlreferer") = "Dashboard" Then
            Common.NetMsgbox(Me, "PR Deleted.", dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId))
        End If
        objPR = Nothing
    End Sub

    Private Function checkMandatory(ByRef strMsg As String) As Boolean
        strMsg = ""
        Dim objPR As New PR
        If Not objPR.checkUserAccExist() Then
            strMsg = "You are not assigned to any Budget Account Code. ""&vbCrLf&""Please contact the Finance Manager. "
        End If

        'If ViewState("blnBill") = 0 Then
        '    If strMsg <> "" Then
        '        strMsg &= """&vbCrLf&"""
        '    End If
        '    strMsg &= "Please ask your Buyer Admin to add in a Billing Address! "
        'End If

        'If ViewState("blnDelivery") = 0 Then
        '    If strMsg <> "" Then
        '        strMsg &= """&vbCrLf&"""
        '    End If
        '    strMsg &= "Please ask your Buyer Admin to add in a Delivery Address! "
        'End If

        If strMsg <> "" Then
            checkMandatory = False
        Else
            checkMandatory = True
        End If
        objPR = Nothing
    End Function

    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        If File1.Value <> "" Then
            Dim objFile As New FileManagement
            Dim objPR As New PR

            ' Restrict user upload size
            'Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))
            Dim dblMaxFileSize As Double = Session("FileSize")

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(File1.PostedFile.FileName)

            If Len(sFileName) > 50 Then
                Common.NetMsgbox(Me, "File name exceeds 46 character")
            ElseIf File1.PostedFile.ContentLength > 0 And File1.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                If ViewState("type") = "new" Then
                    objFile.FileUpload(File1, EnumUploadType.DocAttachment, "PR", EnumUploadFrom.FrontOff, Session.SessionID, , , , , , "E")
                    Select Case ViewState("mode")
                        Case "bc"
                    End Select
                ElseIf ViewState("type") = "mod" Then
                    objFile.FileUpload(File1, EnumUploadType.DocAttachment, "PR", EnumUploadFrom.FrontOff, ViewState("prid"), , , , , , "E")
                    Select Case ViewState("mode")
                        Case "bc"
                    End Select
                End If
            ElseIf File1.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            objPR = Nothing
            objFile = Nothing
        End If
        displayAttachFile()

        'Dim dtrd As DataRow
        Dim chkItem As CheckBox
        Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark As TextBox
        Dim txtBudget, txtDelivery As Label
        Dim cboGLCode, ddlDelivery, ddl_stock As DropDownList
        Dim strItem, strItemCust As New ArrayList()
        Dim dgItem As DataGridItem
        Dim k As Integer = 0
        Dim strItemHead As New ArrayList()

        strItemHead.Add(New String() {txtAttention.Text, txtInternal.Text, txtExternal.Text, chkUrgent.Checked, cboPayTerm.SelectedIndex, cboPayMethod.SelectedIndex, cboCurr.SelectedIndex})

        For Each dgItem In dtgShopping.Items
            txtQty = dgItem.FindControl("txtQty")
            cboGLCode = dgItem.FindControl("cboGLCode")
            txtPrice = dgItem.FindControl("txtPrice")
            txtBudget = dgItem.FindControl("txtBudget")
            hidBudgetCode = dgItem.FindControl("hidBudgetCode")
            txtDelivery = dgItem.FindControl("txtDelivery")
            hidDelCode = dgItem.FindControl("hidDelCode")
            txtEstDate = dgItem.FindControl("txtEstDate")
            txtWarranty = dgItem.FindControl("txtWarranty")
            txtRemark = dgItem.FindControl("txtRemark")
            ddlDelivery = dgItem.FindControl("ddlDelivery")
            ddl_stock = dgItem.FindControl("ddl_stock")

            chkItem = dgItem.FindControl("chkSelection")
            'If chkItem.Checked Then
            'Else
            '    strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
            'End If
            strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, ddlDelivery.SelectedIndex, ddl_stock.SelectedIndex})

            Dim objAdmin As New Admin
            dvwCus = objAdmin.getCustomField("")

            If Not dvwCus Is Nothing Then
                For i = 0 To dvwCus.Count - 1
                    'For Each dgItem2 In dtgShopping.Items
                    Dim cboCustom As DropDownList
                    cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                    If chkItem.Checked Then
                    Else
                        strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                    End If
                    'Next
                Next
            End If

            k += 1
        Next
        Session("strItem") = strItem
        Session("strItemCust") = strItemCust
        Session("strItemHead") = strItemHead

        Bindgrid()
    End Sub

    Private Sub cmdUploadInt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUploadInt.Click
        If File1Int.Value <> "" Then
            Dim objFile As New FileManagement
            Dim objPR As New PR

            ' Restrict user upload size
            'Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))
            Dim dblMaxFileSize As Double = Session("FileSize")

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(File1Int.PostedFile.FileName)

            If Len(sFileName) > 50 Then
                Common.NetMsgbox(Me, "File name exceeds 46 character")
            ElseIf File1Int.PostedFile.ContentLength > 0 And File1Int.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                If ViewState("type") = "new" Then
                    objFile.FileUpload(File1Int, EnumUploadType.DocAttachment, "PR", EnumUploadFrom.FrontOff, Session.SessionID, , , , , , "I")
                    Select Case ViewState("mode")
                        Case "bc"
                    End Select
                ElseIf ViewState("type") = "mod" Then
                    objFile.FileUpload(File1Int, EnumUploadType.DocAttachment, "PR", EnumUploadFrom.FrontOff, ViewState("prid"), , , , , , "I")
                    Select Case ViewState("mode")
                        Case "bc"
                    End Select
                End If
            ElseIf File1Int.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            objPR = Nothing
            objFile = Nothing
        End If
        displayAttachFileInt()

        'Dim dtrd As DataRow
        Dim chkItem As CheckBox
        Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark As TextBox
        Dim txtBudget, txtDelivery As Label
        Dim cboGLCode, ddlDelivery, ddl_stock As DropDownList
        Dim strItem, strItemCust As New ArrayList()
        Dim dgItem As DataGridItem
        Dim k As Integer = 0
        Dim strItemHead As New ArrayList()

        strItemHead.Add(New String() {txtAttention.Text, txtInternal.Text, txtExternal.Text, chkUrgent.Checked, cboPayTerm.SelectedIndex, cboPayMethod.SelectedIndex, cboCurr.SelectedIndex})

        For Each dgItem In dtgShopping.Items
            txtQty = dgItem.FindControl("txtQty")
            cboGLCode = dgItem.FindControl("cboGLCode")
            txtPrice = dgItem.FindControl("txtPrice")
            txtBudget = dgItem.FindControl("txtBudget")
            hidBudgetCode = dgItem.FindControl("hidBudgetCode")
            txtDelivery = dgItem.FindControl("txtDelivery")
            hidDelCode = dgItem.FindControl("hidDelCode")
            txtEstDate = dgItem.FindControl("txtEstDate")
            txtWarranty = dgItem.FindControl("txtWarranty")
            txtRemark = dgItem.FindControl("txtRemark")
            ddlDelivery = dgItem.FindControl("ddlDelivery")
            ddl_stock = dgItem.FindControl("ddl_stock")

            chkItem = dgItem.FindControl("chkSelection")
            'If chkItem.Checked Then
            'Else
            '    strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
            'End If
            strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, ddlDelivery.SelectedIndex, ddl_stock.SelectedIndex})

            Dim objAdmin As New Admin
            dvwCus = objAdmin.getCustomField("")

            If Not dvwCus Is Nothing Then
                For i = 0 To dvwCus.Count - 1
                    'For Each dgItem2 In dtgShopping.Items
                    Dim cboCustom As DropDownList
                    cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                    If chkItem.Checked Then
                    Else
                        strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                    End If
                    'Next
                Next
            End If

            k += 1
        Next
        Session("strItem") = strItem
        Session("strItemCust") = strItemCust
        Session("strItemHead") = strItemHead

        Bindgrid()
    End Sub

    Private Sub displayAttachFile()
        Dim objPR As New PR
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        If ViewState("type") = "new" Then
            dsAttach = objPR.getPRTempAttach(Session.SessionID, "E")
            Select Case ViewState("mode")
                Case "bc"
            End Select
        ElseIf ViewState("type") = "mod" Then
            dsAttach = objPR.getPRTempAttach(ViewState("prid"), "E")
            Select Case ViewState("mode")
                Case "bc"
            End Select
        End If

        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                '**********************meilai 25/02/2005****************** 
                'strURL = "<A HREF=../FileDownload.aspx?pb=" & viewstate("postback") & "&file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & ">" & strFile & "</A>"
                Dim objFile As New FileManagement
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "", EnumUploadFrom.FrontOff)
                objFile = Nothing
                '**********************meilai*****************************
                Dim lblBr As New Label
                Dim lblFile As New Label
                Dim lnk As New ImageButton
                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"
                lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete2.gif")
                lnk.ID = drvAttach(i)("CDA_ATTACH_INDEX")
                lnk.CausesValidation = False
                AddHandler lnk.Click, AddressOf deleteAttach

                pnlAttach.Controls.Add(lblFile)
                pnlAttach.Controls.Add(lnk)
                pnlAttach.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFile)
        End If
        objPR = Nothing
    End Sub

    Private Sub displayAttachFileInt()
        Dim objPR As New PR
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL As String

        If ViewState("type") = "new" Then
            dsAttach = objPR.getPRTempAttach(Session.SessionID, "I")
            Select Case ViewState("mode")
                Case "bc"
            End Select
        ElseIf ViewState("type") = "mod" Then
            dsAttach = objPR.getPRTempAttach(ViewState("prid"), "I")
            Select Case ViewState("mode")
                Case "bc"
            End Select
        End If

        pnlAttachInt.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                '**********************meilai 25/02/2005****************** 
                'strURL = "<A HREF=../FileDownload.aspx?pb=" & viewstate("postback") & "&file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & ">" & strFile & "</A>"
                Dim objFile As New FileManagement
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "", EnumUploadFrom.FrontOff)
                objFile = Nothing
                '**********************meilai*****************************
                Dim lblBr As New Label
                Dim lblFile As New Label
                Dim lnk As New ImageButton
                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"
                lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete2.gif")
                lnk.ID = drvAttach(i)("CDA_ATTACH_INDEX")
                lnk.CausesValidation = False
                AddHandler lnk.Click, AddressOf deleteAttachInt

                pnlAttachInt.Controls.Add(lblFile)
                pnlAttachInt.Controls.Add(lnk)
                pnlAttachInt.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttachInt.Controls.Add(lblFile)
        End If
        objPR = Nothing
    End Sub

    Private Sub deleteAttach(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objPR As New PR
        If ViewState("type") = "new" Then
            objPR.deletePRAttachment(CType(sender, ImageButton).ID)
            Select Case ViewState("mode")
                Case "bc"
            End Select
        ElseIf ViewState("type") = "mod" Then
            objPR.deletePRAttachment(CType(sender, ImageButton).ID)
            Select Case ViewState("mode")
                Case "bc"
            End Select
        End If
        displayAttachFile()
        objPR = Nothing
    End Sub

    Private Sub deleteAttachInt(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objPR As New PR
        If ViewState("type") = "new" Then
            objPR.deletePRAttachment(CType(sender, ImageButton).ID)
            Select Case ViewState("mode")
                Case "bc"
            End Select
        ElseIf ViewState("type") = "mod" Then
            objPR.deletePRAttachment(CType(sender, ImageButton).ID)
            Select Case ViewState("mode")
                Case "bc"
            End Select
        End If
        displayAttachFileInt()
        objPR = Nothing
    End Sub

    Private Function validateDatagrid(ByRef strMsg As String) As Boolean
        Dim strDeliveryCommonPrev As String = ""
        Dim strDeliveryCommon As String = ""
        validateDatagrid = True
        strMsg = "<ul type='disc'>"

        If Not Common.checkMaxLength(txtInternal.Text, 1000) Then
            strMsg &= "<li>For Internal Use is over limit.<ul type='disc'></ul></li>"
            validateDatagrid = False
        End If

        If Not Common.checkMaxLength(txtExternal.Text, 1000) Then
            strMsg &= "<li>Remarks is over limit.<ul type='disc'></ul></li>"
            validateDatagrid = False
        End If

        If cboPayTerm.SelectedIndex = 0 Then
            strMsg &= "<li>Payment Term is required.</li>"
            validateDatagrid = False
        End If

        If cboPayMethod.SelectedIndex = 0 Then
            strMsg &= "<li>Payment Method is required.</li>"
            validateDatagrid = False
        End If

        If cboCurr.SelectedIndex = 0 Then
            strMsg &= "<li>Currency Code is required.</li>"
            validateDatagrid = False
        End If

        Dim dgItem As DataGridItem
        Dim txtRemark As TextBox
        Dim txtQ As TextBox
        'Dim ddl_comm As DropDownList
        Dim hidBudgetCode As TextBox

        Dim commonDel As Boolean = False
        For Each dgItem In dtgShopping.Items

            'Dim hidDelCode As TextBox
            'hidDelCode = dgItem.FindControl("hidDelCode")
            'If hidDelCode.Text = "" Or hidDelCode.Text = "&nbsp;" Then
            '    strMsg &= "<li>Invalid Delivery Address.<ul type='disc'></ul></li>"
            '    validateDatagrid = False
            'End If

            Dim txtEstDate As TextBox
            txtEstDate = dgItem.FindControl("txtEstDate")
            If Not IsDate(txtEstDate.Text) Then
                'strMsg &= "<li>Invalid Est. Date of Delivery (dd/mm/yyyy).<ul type='disc'></ul></li>"
                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Est. Date of Delivery must be greater than PR creation date (dd/mm/yyyy).<ul type='disc'></ul></li>"
                validateDatagrid = False
            Else
                Dim txtDate1 As Date = CType(lblDate.Text, Date)
                Dim txtDate2 As Date = CType(txtEstDate.Text, Date)
                If txtDate2 <= txtDate1 Then
                    strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Est. Date of Delivery must be greater than PR creation date (dd/mm/yyyy).<ul type='disc'></ul></li>"
                    validateDatagrid = False
                End If
            End If

            Dim lblVendor As Label
            lblVendor = dgItem.FindControl("lblVendor")

            If lblVendor.Text <> "" And lblVendor.Text <> "&nbsp;" Then
                Dim strRec_No As String = objDB.GetVal("SELECT CV_B_COY_ID FROM COMPANY_VENDOR WHERE CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CV_S_COY_ID = '" & lblVendor.Text & "'")
                If strRec_No = "" Then
                    strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Vendor not approved.<ul type='disc'></ul></li>"
                    validateDatagrid = False
                End If
            End If

            Dim strGstRegNo As String
            Dim objGST As New GST
            Dim lblGSTRate, lblTaxCode As Label
            lblGSTRate = dgItem.FindControl("lblGSTRate")
            lblTaxCode = dgItem.FindControl("lblTaxCode")

            If ViewState("CheckGST") = True Then
                Select Case ViewState("mode")
                    Case "bc"
                    Case "cc"
                        If lblVendor.Text <> "" And lblVendor.Text <> "&nbsp;" Then
                            strGstRegNo = objGST.chkGST(lblVendor.Text)
                            If strGstRegNo <> "" AndAlso (lblGSTRate.Text = "N/A" Or lblGSTRate.Text = "" Or lblGSTRate.Text = "&nbsp;") Then
                                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". GST Rate is required for GST registered vendor.<ul type='disc'></ul></li>"
                                validateDatagrid = False
                            Else
                                If strGstRegNo <> "" AndAlso (lblTaxCode.Text = "N/A" Or lblTaxCode.Text = "" Or lblTaxCode.Text = "&nbsp;") Then
                                    strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". GST Tax Code is required.<ul type='disc'></ul></li>"
                                    validateDatagrid = False
                                End If
                            End If
                        End If
                End Select
            End If

            txtRemark = dgItem.FindControl("txtRemark")
            txtQ = dgItem.FindControl("txtQ")

            If Not Common.checkMaxLength(txtRemark.Text, 400) Then
                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Remarks is over limit.<ul type='disc'></ul></li>"
                txtQ.Text = "?"
                validateDatagrid = False
            Else
                txtQ.Text = ""
            End If

            'ddl_comm = dgItem.FindControl("ddl_comm")
            'If ddl_comm.SelectedItem.Text = "" Or ddl_comm.SelectedItem.Text = "---Select---" Then
            '    strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Commodity type not selected.<ul type='disc'></ul></li>"
            '    validateDatagrid = False
            'End If

            Dim txtCommodity As TextBox
            txtCommodity = dgItem.FindControl("txtCommodity")
            If txtCommodity.Text = "" Then
                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Commodity type is required.<ul type='disc'></ul></li>"
                validateDatagrid = False
            End If

            Dim strComm As String = objDB.GetVal("SELECT IFNULL(CT_NAME,'') FROM COMMODITY_TYPE WHERE CT_NAME = '" & Common.Parse(txtCommodity.Text) & "' LIMIT 1 ")
            If IsDBNull(strComm) Or strComm = "" Then
                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Invalid commodity type.<ul type='disc'></ul></li>"
                validateDatagrid = False
            End If

            Dim ddl_stock As DropDownList
            ddl_stock = dgItem.FindControl("ddl_stock")
            If ddl_stock.SelectedIndex = 0 Then
                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Item type is required.<ul type='disc'></ul></li>"
                validateDatagrid = False
            End If

            Dim ddlDelivery As DropDownList
            ddlDelivery = dgItem.FindControl("ddlDelivery")

            Dim lblProductCode As Label = dgItem.FindControl("lblProductCode")

            Dim lblOversea As Label
            lblOversea = dgItem.FindControl("lblOversea")
            'If ddl_stock.SelectedValue = "ST" And ddlDelivery.SelectedIndex = 0 Then
            If ddlDelivery.SelectedIndex = 0 Then
                ''strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Delivery Term is required for stock item.<ul type='disc'></ul></li>"
                'strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Delivery Term is required.<ul type='disc'></ul></li>"
                'validateDatagrid = False
            Else
                If lblOversea.Text = "N" Then
                    If (lblProductCode.Text <> "&nbsp;" And lblProductCode.Text <> "") Then
                        If cboCurr.SelectedValue = "MYR" And ddlDelivery.SelectedValue <> "3233" Then
                            strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". " & objGlobal.GetErrorMessage("00368") & "<ul type='disc'></ul></li>"
                            validateDatagrid = False
                        ElseIf cboCurr.SelectedValue <> "MYR" And ddlDelivery.SelectedValue = "3233" Then
                            strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". " & objGlobal.GetErrorMessage("00369") & "<ul type='disc'></ul></li>"
                            validateDatagrid = False
                        End If
                    Else
                        ' For FF Item
                        If cboCurr.SelectedValue <> "MYR" And ddlDelivery.SelectedValue = "3233" Then
                            strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". " & objGlobal.GetErrorMessage("00364") & "<ul type='disc'></ul></li>"
                            validateDatagrid = False
                        End If
                    End If
                ElseIf lblOversea.Text = "Y" Then
                    If ddlDelivery.SelectedValue = "3233" Then
                        strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". " & objGlobal.GetErrorMessage("00367") & "<ul type='disc'></ul></li>"
                        validateDatagrid = False
                    End If
                End If

                If strDeliveryCommonPrev = "" Then
                    strDeliveryCommon = ddlDelivery.SelectedIndex
                    strDeliveryCommonPrev = ddlDelivery.SelectedIndex
                Else
                    strDeliveryCommon = ddlDelivery.SelectedIndex
                End If
            End If

            If strDeliveryCommonPrev <> strDeliveryCommon And ViewState("mode") = "cc" Then
                If commonDel = False Then
                    commonDel = True
                    strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Common delivery term is required.<ul type='disc'></ul></li>"
                End If
                validateDatagrid = False
            End If

            If strDeliveryCommonPrev <> strDeliveryCommon And ViewState("mode") <> "cc" And ddl_stock.SelectedValue = "SP" Then
                If commonDel = False Then
                    commonDel = True
                    strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Common delivery term is required.<ul type='disc'></ul></li>"
                End If
                validateDatagrid = False
            End If
            'If Session("Env") <> "FTN" Then
            '    If ViewState("BCM") > 0 Then
            '        hidBudgetCode = dgItem.FindControl("hidBudgetCode")
            '        If hidBudgetCode.Text = "" Then
            '            strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Budget Account not selected.<ul type='disc'></ul></li>"
            '            validateDatagrid = False
            '        End If
            '    End If
            'End If
            If ViewState("BCM") > 0 Then
                hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                If hidBudgetCode.Text = "" Then
                    strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Budget Account not selected.<ul type='disc'></ul></li>"
                    validateDatagrid = False
                End If
            End If

            'Dim lblProductCode As Label = dgItem.FindControl("lblProductCode")
            Dim lblCurrency As Label = dgItem.FindControl("lblCurrency")
            'If ddl_stock.SelectedValue = "ST" And (lblProductCode.Text <> "&nbsp;" And lblProductCode.Text <> "") Then
            'If (lblCurrency.Text <> "&nbsp;" And lblCurrency.Text <> "") And (lblProductCode.Text <> "&nbsp;" And lblProductCode.Text <> "") Then
            If (lblCurrency.Text <> "&nbsp;" And lblCurrency.Text <> "") Then
                Dim strSuppCode As String
                Dim strCurrCode As String

                If cboCurr.SelectedValue <> lblCurrency.Text Then
                    strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Item's currency does not match with PR currency.<ul type='disc'></ul></li>"
                    validateDatagrid = False
                End If

                'strCurrCode = objDB.GetVal("SELECT IFNULL(PV_CURR, '') AS PV_SUPP_CODE FROM PIM_VENDOR WHERE PV_PRODUCT_INDEX = (SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PM_PRODUCT_CODE = '" & lblProductCode.Text & "')  AND PV_S_COY_ID = '" & dgItem.Cells(EnumShoppingCart.icVendor).Text & "' AND PV_CURR = '" & cboCurr.SelectedValue & "'")

                'If strCurrCode = "" Then
                '    strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". PR currency does not match with Item's last trasaction currency code.<ul type='disc'></ul></li>"
                '    validateDatagrid = False
                'End If

                'strSuppCode = objDB.GetVal("SELECT IFNULL(PV_SUPP_CODE, '') AS PV_SUPP_CODE FROM PIM_VENDOR WHERE PV_PRODUCT_INDEX = (SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PM_PRODUCT_CODE = '" & lblProductCode.Text & "')  AND PV_S_COY_ID = '" & dgItem.Cells(EnumShoppingCart.icVendor).Text & "' AND (PV_SUPP_CODE <> '' OR PV_SUPP_CODE IS NOT NULL) LIMIT 1")
                'If strSuppCode = "" Then
                '    strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Vendor code is not setup properly.<ul type='disc'></ul></li>"
                '    validateDatagrid = False
                'End If
            End If
        Next
        strMsg &= "</ul>"
    End Function

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_AddPR_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PR", "RaisePR.aspx", "pageid=" & strPageId & "&type=new&mode=bc&frm=bc") & """><span>Purchase Request</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId) & """><span>Purchase Request Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PR", "SearchPR_Cancellation.aspx", "pageid=" & strPageId) & """><span>Purchase Request Cancellation</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
    End Sub

    Public Sub btnHidden1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden1.Click

    End Sub

    'Stage 3 Bug fix (GST-0024) - 08/07/2015 - CH
    'Function to store all info in temp array
    Public Sub savePRItemInArray()
        'Dim dtrd As DataRow
        Dim chkItem As CheckBox
        Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark As TextBox
        Dim txtBudget, txtDelivery As Label
        Dim cboGLCode, ddlDelivery, ddl_stock As DropDownList
        Dim strItem, strItemCust As New ArrayList()
        Dim dgItem As DataGridItem
        Dim k As Integer = 0
        Dim strItemHead As New ArrayList()

        strItemHead.Add(New String() {txtAttention.Text, txtInternal.Text, txtExternal.Text, chkUrgent.Checked, cboPayTerm.SelectedIndex, cboPayMethod.SelectedIndex, cboCurr.SelectedIndex})

        For Each dgItem In dtgShopping.Items
            txtQty = dgItem.FindControl("txtQty")
            cboGLCode = dgItem.FindControl("cboGLCode")
            txtPrice = dgItem.FindControl("txtPrice")
            txtBudget = dgItem.FindControl("txtBudget")
            hidBudgetCode = dgItem.FindControl("hidBudgetCode")
            txtDelivery = dgItem.FindControl("txtDelivery")
            hidDelCode = dgItem.FindControl("hidDelCode")
            txtEstDate = dgItem.FindControl("txtEstDate")
            txtWarranty = dgItem.FindControl("txtWarranty")
            txtRemark = dgItem.FindControl("txtRemark")
            ddlDelivery = dgItem.FindControl("ddlDelivery")
            ddl_stock = dgItem.FindControl("ddl_stock")

            chkItem = dgItem.FindControl("chkSelection")
            'If chkItem.Checked Then
            'Else
            '    strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
            'End If
            strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, ddlDelivery.SelectedIndex, ddl_stock.SelectedIndex})

            Dim objAdmin As New Admin
            dvwCus = objAdmin.getCustomField("")

            If Not dvwCus Is Nothing Then
                For i = 0 To dvwCus.Count - 1
                    'For Each dgItem2 In dtgShopping.Items
                    Dim cboCustom As DropDownList
                    cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                    If chkItem.Checked Then
                    Else
                        strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                    End If
                    'Next
                Next
            End If

            k += 1
        Next
        Session("strItem") = strItem
        Session("strItemCust") = strItemCust
        Session("strItemHead") = strItemHead

        Bindgrid()
    End Sub

    Protected Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim dsPR As New DataSet
        Dim objPR As New PR
        Dim objPR_Ext As New PR_Ext
        Dim objPO1 As New PurchaseOrder_Buyer
        Dim strMsg As String = ""
        Dim blnExceed As Boolean

        Dim dt As New DataTable

        blnValid = True
        If validateDatagrid(strMsg) Then
            dsPR = bindPR()
            Dim dsapplist As New DataSet
            dsapplist = objPR.getAppovalList("A", CDbl(ViewState("PRCost")), "PR", True)
            If dsapplist.Tables(0).Rows.Count = 0 Then
                strMsg &= "<li>There is no approval list available for this PR because the sequence of approving officers do not have the approval limit to approve it.<ul type='disc'></ul></li>"
                lblMsg.Text = strMsg
                ViewState("PRCost") = Nothing
                Exit Sub
            End If

            'Michelle (11/1/2013) - Issue 1832
            Dim objAdmin As New Admin
            ViewState("blnGRNCtrl") = objAdmin.withGRNCtrl()
            If ViewState("blnGRNCtrl") Then
                'Check whether user is with SK fixed role
                Dim strFixRole As String
                Dim objUserRole As New UserRoles
                strFixRole = objUserRole.get_UserFixedRole()
                If strFixRole.IndexOf("Store Keeper") <> -1 Then
                    'Check whether there is outstanding GRN
                    Dim dsOVendor As New DataSet
                    Dim objDO As New DeliveryOrder
                    Dim intLoop As Integer
                    Dim blnWithOGRN As Boolean = False
                    dsOVendor = objDO.GetDO2("", "")
                    For intLoop = 0 To dsOVendor.Tables(0).Rows.Count - 1
                        If objDO.ChkOustdGRN(dsOVendor.Tables(0).Rows(intLoop)("DOM_S_COY_ID"), dsOVendor.Tables(0).Rows(intLoop)("DO_DAYS")) Then
                            Dim strMsg1 As String
                            strMsg1 = "Your access to submit PR is denied due to your account still has outstanding GRN pending your action."
                            strMsg1 &= """& vbCrLf & """ & "Please raise GRN Accept if you had received the physical delivery or services, else create GRN Reject if you had not received any physical delivery or services from the vendors."
                            strMsg1 &= """& vbCrLf & """ & "Please save your PR as draft, and create GRN Accept or Reject to obtain the access to submit PR for approval."
                            ViewState("PRCost") = Nothing
                            Common.NetMsgbox(Me, strMsg1, MsgBoxStyle.Exclamation)
                            Exit Sub
                        End If
                    Next
                End If

            End If
            'End If
            'End If

            If ViewState("type") = "new" Then
                Dim strNewPR As String = ""
                Dim intMsg As Integer

                If blnValid = True And blnValidBill = True Then
                    lblMsg.Text = ""

                    'If Session("Env") = "FTN" Then
                    '    dt = objPR.getPRApprFlow(True)
                    'Else
                    '    dt = objPR.getPRApprFlow(False)
                    'End If
                    dt = objPR.getPRApprFlow(False)

                    Dim CheckApprB4 As Boolean = True
                    If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                        CheckApprB4 = False
                    End If

                    'If Not IsNumeric(Mid(lblPONo.Text, 3)) Then
                    If lblPRNo.Text = "To Be Allocated By System" And CheckApprB4 = True Then
                        intMsg = objPR_Ext.insertPR(dsPR, strNewPR, Session("NonFTN"))
                    Else
                        'If Session("Env") = "FTN" Then
                        '    dt = objPR.getPRApprFlow(True)
                        'Else
                        '    dt = objPR.getPRApprFlow(False)
                        'End If
                        dt = objPR.getPRApprFlow(False)

                        If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                            Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                            Exit Sub
                        ElseIf dt.Rows.Count > 1 Then

                            If Request.QueryString("frm") = "BuyerCat" Then
                                Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & cboCurr.SelectedItem.Value))
                            ElseIf Request.QueryString("frm") = "ConCat" Then
                                Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & cboCurr.SelectedItem.Value))
                            ElseIf Not Request.QueryString("type") = "" Then
                                If Request.QueryString("mode") = "bc" Then
                                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & cboCurr.SelectedItem.Value))
                                Else
                                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & cboCurr.SelectedItem.Value))
                                End If
                            End If
                            Exit Sub
                        Else
                            objPR_Ext.updatePR(dsPR, Session("NonFTN"))
                            intMsg = WheelMsgNum.Save
                            strNewPR = lblPRNo.Text
                        End If
                    End If
                    'objPR = Nothing
                    'redirect to ExceedBCM before approval page
                    Select Case intMsg
                        Case WheelMsgNum.Save
                            If blnItem Then ' item exists

                                'If Session("Env") = "FTN" Then
                                '    dt = objPR.getPRApprFlow(True)
                                'Else
                                '    dt = objPR.getPRApprFlow(False)
                                'End If
                                dt = objPR.getPRApprFlow(False)

                                'If Session("Env") <> "FTN" Then
                                '    If ViewState("BCM") > 0 Then
                                '        If checkMandatory(strMsg) Then
                                '            Dim objBudget As New BudgetControl
                                '            Dim strBCM As String
                                '            Dim dtBCM As New DataTable
                                '            blnExceed = objBudget.checkBCM(strNewPR, dtBCM, strBCM)

                                '            'Dim dt As New DataTable
                                '            'Dim intMsg As Integer

                                '            If Not blnExceed Then
                                '                If Session("Env") = "FTN" Then
                                '                    dt = objPR.getPRApprFlow(True)
                                '                Else
                                '                    dt = objPR.getPRApprFlow(False)
                                '                End If

                                '                ViewState("mode") = Request.QueryString("mode")
                                '                If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                                '                    Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                '                    Exit Sub
                                '                ElseIf dt.Rows.Count > 1 Then
                                '                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                '                    Exit Sub
                                '                Else
                                '                    Dim dsAO As New DataSet
                                '                    Dim UM_APP_LIMIT As Double
                                '                    Dim PR_Type As String = ""
                                '                    Dim blnBuyerWNoWork As Boolean = False

                                '                    If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                                '                        blnBuyerWNoWork = True
                                '                    Else
                                '                        ViewState("ApprovalType") = objPR.getApprovalType
                                '                        dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                                '                        ' case Automated Approval
                                '                        If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                                '                            UM_APP_LIMIT = 0
                                '                        Else
                                '                            UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                                '                        End If

                                '                        ' A - Automated Approval
                                '                        ' B - Allow Lower Limit Endorsement
                                '                        ' C - Cut PO before end of Aproval List
                                '                        ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                                '                        Select Case ViewState("ApprovalType")
                                '                            Case "C"
                                '                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                '                                    If ViewState("blnCutPR") = True Then
                                '                                        PR_Type = "None"
                                '                                    Else
                                '                                        PR_Type = "Approval"
                                '                                        ViewState("blnCutPR") = True
                                '                                    End If
                                '                                Else
                                '                                    PR_Type = "None"
                                '                                End If

                                '                            Case "B"
                                '                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                '                                    PR_Type = "Approval"
                                '                                Else
                                '                                    PR_Type = "Endorsement"
                                '                                End If

                                '                            Case "B+C"
                                '                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                '                                    If ViewState("blnCutPR") = True Then
                                '                                        PR_Type = "None"
                                '                                    Else
                                '                                        PR_Type = "Approval"
                                '                                        ViewState("blnCutPR") = True
                                '                                    End If
                                '                                Else
                                '                                    PR_Type = "Endorsement"
                                '                                End If

                                '                            Case "A"
                                '                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                '                                    PR_Type = "Approval"
                                '                                Else
                                '                                    PR_Type = "None"
                                '                                End If

                                '                        End Select

                                '                        Select Case PR_Type
                                '                            Case "None"
                                '                                PR_Type = "0"
                                '                            Case "Approval"
                                '                                PR_Type = "1"
                                '                            Case "Endorsement"
                                '                                PR_Type = "2"
                                '                        End Select
                                '                    End If

                                '                    If blnBuyerWNoWork = True Then
                                '                        ViewState("msg") = Nothing
                                '                    Else
                                '                        ViewState("msg") = 0 ' Follow the ApprovalSetup Concept/Flow which is "0"
                                '                    End If

                                '                    intMsg = objPR.submitPR(strNewPR, PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)
                                '                    Select Case intMsg
                                '                        Case WheelMsgNum.Save
                                '                            If Session("urlreferer") = "BuyerCatSearch" Then
                                '                                Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                                '                            ElseIf Session("urlreferer") = "ConCatSearch" Then
                                '                                Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                                '                            ElseIf Session("urlreferer") = "PRAll" Then
                                '                                Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                                '                            End If

                                '                        Case WheelMsgNum.NotSave
                                '                            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                                '                        Case WheelMsgNum.Delete
                                '                            Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

                                '                    End Select
                                '                End If
                                '            Else

                                '                If strBCM = "2" Then
                                '                    If Session("Env") = "FTN" Then
                                '                        dt = objPR.getPRApprFlow(True)
                                '                    Else
                                '                        dt = objPR.getPRApprFlow(False)
                                '                    End If

                                '                    ViewState("mode") = Request.QueryString("mode")
                                '                    If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                                '                        Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                '                        Exit Sub
                                '                    ElseIf dt.Rows.Count > 1 Then
                                '                        Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=1&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                '                        Exit Sub
                                '                    Else
                                '                        Dim dsAO As New DataSet
                                '                        Dim UM_APP_LIMIT As Double
                                '                        Dim PR_Type As String = ""
                                '                        Dim blnBuyerWNoWork As Boolean = False

                                '                        If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                                '                            blnBuyerWNoWork = True
                                '                        Else
                                '                            ViewState("ApprovalType") = objPR.getApprovalType
                                '                            dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                                '                            ' case Automated Approval
                                '                            If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                                '                                UM_APP_LIMIT = 0
                                '                            Else
                                '                                UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                                '                            End If

                                '                            ' A - Automated Approval
                                '                            ' B - Allow Lower Limit Endorsement
                                '                            ' C - Cut PO before end of Aproval List
                                '                            ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                                '                            Select Case ViewState("ApprovalType")
                                '                                Case "C"
                                '                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                '                                        If ViewState("blnCutPR") = True Then
                                '                                            PR_Type = "None"
                                '                                        Else
                                '                                            PR_Type = "Approval"
                                '                                            ViewState("blnCutPR") = True
                                '                                        End If
                                '                                    Else
                                '                                        PR_Type = "None"
                                '                                    End If

                                '                                Case "B"
                                '                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                '                                        PR_Type = "Approval"
                                '                                    Else
                                '                                        PR_Type = "Endorsement"
                                '                                    End If

                                '                                Case "B+C"
                                '                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                '                                        If ViewState("blnCutPR") = True Then
                                '                                            PR_Type = "None"
                                '                                        Else
                                '                                            PR_Type = "Approval"
                                '                                            ViewState("blnCutPR") = True
                                '                                        End If
                                '                                    Else
                                '                                        PR_Type = "Endorsement"
                                '                                    End If

                                '                                Case "A"
                                '                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                '                                        PR_Type = "Approval"
                                '                                    Else
                                '                                        PR_Type = "None"
                                '                                    End If

                                '                            End Select

                                '                            Select Case PR_Type
                                '                                Case "None"
                                '                                    PR_Type = "0"
                                '                                Case "Approval"
                                '                                    PR_Type = "1"
                                '                                Case "Endorsement"
                                '                                    PR_Type = "2"
                                '                            End Select
                                '                        End If

                                '                        If blnBuyerWNoWork = True Then
                                '                            ViewState("msg") = Nothing
                                '                        Else
                                '                            ViewState("msg") = 0 ' Follow the ApprovalSetup Concept/Flow which is "0"
                                '                        End If

                                '                        intMsg = objPR.submitPR(strNewPR, PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)
                                '                        Select Case intMsg
                                '                            Case WheelMsgNum.Save
                                '                                If Session("urlreferer") = "BuyerCatSearch" Then
                                '                                    Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                                '                                ElseIf Session("urlreferer") = "ConCatSearch" Then
                                '                                    Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                                '                                ElseIf Session("urlreferer") = "PRAll" Then
                                '                                    Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                                '                                End If

                                '                            Case WheelMsgNum.NotSave
                                '                                Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                                '                            Case WheelMsgNum.Delete
                                '                                Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

                                '                        End Select
                                '                    End If
                                '                Else
                                '                    Response.Redirect(dDispatcher.direct("PR", "ExceedBCM.aspx", "pageid=" & strPageId & "&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&mode=" & ViewState("mode")))
                                '                End If
                                '            End If
                                '        Else
                                '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                                '        End If
                                '    Else
                                '        'If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                                '        '    Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                '        '    Exit Sub
                                '        'ElseIf dt.Rows.Count > 1 Then
                                '        '    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                                '        '    Exit Sub
                                '        'Else
                                '        lblPRNo.Text = strNewPR
                                '        SubmitPR()
                                '        'End If
                                '        'Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                                '    End If
                                'Else
                                '    'If ViewState("BCM") > 0 Then
                                '    'If checkMandatory(strMsg) Then
                                '    '    Response.Redirect(dDispatcher.direct("PR", "ExceedBCM.aspx", "pageid=" & strPageId & "&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&mode=" & ViewState("mode")))
                                '    'Else
                                '    '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                                '    'End If
                                '    'Else
                                '    'If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                                '    '    Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                '    '    Exit Sub
                                '    'ElseIf dt.Rows.Count > 1 Then
                                '    '    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                                '    '    Exit Sub
                                '    'Else
                                '    lblPRNo.Text = strNewPR
                                '    SubmitPR()
                                '    'End If
                                '    'Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                                '    'End If
                                'End If
                                If ViewState("BCM") > 0 Then
                                    If checkMandatory(strMsg) Then
                                        Dim objBudget As New BudgetControl
                                        Dim strBCM As String
                                        Dim dtBCM As New DataTable
                                        blnExceed = objBudget.checkBCM(strNewPR, dtBCM, strBCM)

                                        'Dim dt As New DataTable
                                        'Dim intMsg As Integer

                                        If Not blnExceed Then
                                            'If Session("Env") = "FTN" Then
                                            '    dt = objPR.getPRApprFlow(True)
                                            'Else
                                            '    dt = objPR.getPRApprFlow(False)
                                            'End If
                                            dt = objPR.getPRApprFlow(False)

                                            ViewState("mode") = Request.QueryString("mode")
                                            If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                                                Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                                Exit Sub
                                            ElseIf dt.Rows.Count > 1 Then
                                                If Request.QueryString("frm") = "BuyerCat" Then
                                                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & cboCurr.SelectedItem.Value))
                                                ElseIf Request.QueryString("frm") = "ConCat" Then
                                                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & cboCurr.SelectedItem.Value))
                                                ElseIf Not Request.QueryString("type") = "" Then
                                                    If Request.QueryString("mode") = "bc" Then
                                                        Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & cboCurr.SelectedItem.Value))
                                                    Else
                                                        Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & cboCurr.SelectedItem.Value))
                                                    End If
                                                End If
                                                Exit Sub
                                            Else
                                                Dim dsAO As New DataSet
                                                Dim UM_APP_LIMIT As Double
                                                Dim PR_Type As String = ""
                                                Dim blnBuyerWNoWork As Boolean = False

                                                If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                                                    blnBuyerWNoWork = True
                                                Else
                                                    ViewState("ApprovalType") = objPR.getApprovalType
                                                    dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                                                    ' case Automated Approval
                                                    If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                                                        UM_APP_LIMIT = 0
                                                    Else
                                                        UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                                                    End If

                                                    ' A - Automated Approval
                                                    ' B - Allow Lower Limit Endorsement
                                                    ' C - Cut PO before end of Aproval List
                                                    ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                                                    Select Case ViewState("ApprovalType")
                                                        Case "C"
                                                            If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                                                If ViewState("blnCutPR") = True Then
                                                                    PR_Type = "None"
                                                                Else
                                                                    PR_Type = "Approval"
                                                                    ViewState("blnCutPR") = True
                                                                End If
                                                            Else
                                                                PR_Type = "None"
                                                            End If

                                                        Case "B"
                                                            If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                                                PR_Type = "Approval"
                                                            Else
                                                                PR_Type = "Endorsement"
                                                            End If

                                                        Case "B+C"
                                                            If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                                                If ViewState("blnCutPR") = True Then
                                                                    PR_Type = "None"
                                                                Else
                                                                    PR_Type = "Approval"
                                                                    ViewState("blnCutPR") = True
                                                                End If
                                                            Else
                                                                PR_Type = "Endorsement"
                                                            End If

                                                        Case "A"
                                                            If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                                                PR_Type = "Approval"
                                                            Else
                                                                PR_Type = "None"
                                                            End If

                                                    End Select

                                                    Select Case PR_Type
                                                        Case "None"
                                                            PR_Type = "0"
                                                        Case "Approval"
                                                            PR_Type = "1"
                                                        Case "Endorsement"
                                                            PR_Type = "2"
                                                    End Select
                                                End If

                                                If blnBuyerWNoWork = True Then
                                                    ViewState("msg") = Nothing
                                                Else
                                                    ViewState("msg") = 0 ' Follow the ApprovalSetup Concept/Flow which is "0"
                                                End If

                                                intMsg = objPR.submitPR(strNewPR, PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)
                                                Select Case intMsg
                                                    Case WheelMsgNum.Save
                                                        If Session("urlreferer") = "BuyerCatSearch" Then
                                                            Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                                                        ElseIf Session("urlreferer") = "ConCatSearch" Then
                                                            Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                                                        ElseIf Session("urlreferer") = "PRAll" Then
                                                            Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                                                        End If

                                                    Case WheelMsgNum.NotSave
                                                        Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                                                    Case WheelMsgNum.Delete
                                                        Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

                                                End Select
                                            End If
                                        Else

                                            If strBCM = "2" Then
                                                'If Session("Env") = "FTN" Then
                                                '    dt = objPR.getPRApprFlow(True)
                                                'Else
                                                '    dt = objPR.getPRApprFlow(False)
                                                'End If
                                                dt = objPR.getPRApprFlow(False)

                                                ViewState("mode") = Request.QueryString("mode")
                                                If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                                                    Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                                    Exit Sub
                                                ElseIf dt.Rows.Count > 1 Then
                                                    If Request.QueryString("frm") = "BuyerCat" Then
                                                        Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=1&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & cboCurr.SelectedItem.Value))
                                                    ElseIf Request.QueryString("frm") = "ConCat" Then
                                                        Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=1&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & cboCurr.SelectedItem.Value))
                                                    ElseIf Not Request.QueryString("type") = "" Then
                                                        If Request.QueryString("mode") = "bc" Then
                                                            Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=1&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & cboCurr.SelectedItem.Value))
                                                        Else
                                                            Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=1&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & cboCurr.SelectedItem.Value))
                                                        End If
                                                    End If
                                                    Exit Sub
                                                Else
                                                    Dim dsAO As New DataSet
                                                    Dim UM_APP_LIMIT As Double
                                                    Dim PR_Type As String = ""
                                                    Dim blnBuyerWNoWork As Boolean = False

                                                    If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                                                        blnBuyerWNoWork = True
                                                    Else
                                                        ViewState("ApprovalType") = objPR.getApprovalType
                                                        dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                                                        ' case Automated Approval
                                                        If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                                                            UM_APP_LIMIT = 0
                                                        Else
                                                            UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                                                        End If

                                                        ' A - Automated Approval
                                                        ' B - Allow Lower Limit Endorsement
                                                        ' C - Cut PO before end of Aproval List
                                                        ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                                                        Select Case ViewState("ApprovalType")
                                                            Case "C"
                                                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                                                    If ViewState("blnCutPR") = True Then
                                                                        PR_Type = "None"
                                                                    Else
                                                                        PR_Type = "Approval"
                                                                        ViewState("blnCutPR") = True
                                                                    End If
                                                                Else
                                                                    PR_Type = "None"
                                                                End If

                                                            Case "B"
                                                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                                                    PR_Type = "Approval"
                                                                Else
                                                                    PR_Type = "Endorsement"
                                                                End If

                                                            Case "B+C"
                                                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                                                    If ViewState("blnCutPR") = True Then
                                                                        PR_Type = "None"
                                                                    Else
                                                                        PR_Type = "Approval"
                                                                        ViewState("blnCutPR") = True
                                                                    End If
                                                                Else
                                                                    PR_Type = "Endorsement"
                                                                End If

                                                            Case "A"
                                                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                                                    PR_Type = "Approval"
                                                                Else
                                                                    PR_Type = "None"
                                                                End If

                                                        End Select

                                                        Select Case PR_Type
                                                            Case "None"
                                                                PR_Type = "0"
                                                            Case "Approval"
                                                                PR_Type = "1"
                                                            Case "Endorsement"
                                                                PR_Type = "2"
                                                        End Select
                                                    End If

                                                    If blnBuyerWNoWork = True Then
                                                        ViewState("msg") = Nothing
                                                    Else
                                                        ViewState("msg") = 0 ' Follow the ApprovalSetup Concept/Flow which is "0"
                                                    End If

                                                    intMsg = objPR.submitPR(strNewPR, PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)
                                                    Select Case intMsg
                                                        Case WheelMsgNum.Save
                                                            If Session("urlreferer") = "BuyerCatSearch" Then
                                                                Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                                                            ElseIf Session("urlreferer") = "ConCatSearch" Then
                                                                Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                                                            ElseIf Session("urlreferer") = "PRAll" Then
                                                                Common.NetMsgbox(Me, "Purchase Request Number " & strNewPR & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                                                            End If

                                                        Case WheelMsgNum.NotSave
                                                            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                                                        Case WheelMsgNum.Delete
                                                            Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

                                                    End Select
                                                End If
                                            Else
                                                Response.Redirect(dDispatcher.direct("PR", "ExceedBCM.aspx", "pageid=" & strPageId & "&prid=" & strNewPR & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & cboCurr.SelectedItem.Value & "&mode=" & ViewState("mode")))
                                            End If
                                        End If
                                    Else
                                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                                    End If
                                Else
                                    'If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                                    '    Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                    '    Exit Sub
                                    'ElseIf dt.Rows.Count > 1 Then
                                    '    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                                    '    Exit Sub
                                    'Else
                                    lblPRNo.Text = strNewPR
                                    SubmitPR()
                                    'End If
                                    'Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & strNewPR & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                                End If
                            Else
                                Common.NetMsgbox(Me, "There are no items in this PR.", MsgBoxStyle.Information)
                            End If

                        Case WheelMsgNum.NotSave
                            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                        Case WheelMsgNum.Duplicate
                            Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)
                        Case -1
                            Common.NetMsgbox(Me, "Company is currently inactive.", MsgBoxStyle.Information)
                        Case -2
                            Common.NetMsgbox(Me, "Vendor is being deleted.", MsgBoxStyle.Information)
                    End Select
                Else
                    If blnValidBill = False Then
                        Common.NetMsgbox(Me, objGLO.GetErrorMessage("00031"), MsgBoxStyle.Information)
                    End If
                End If

                Select Case ViewState("mode")
                    Case "bc"
                End Select
            ElseIf ViewState("type") = "mod" Then
                If blnValid = True And blnValidBill = True Then
                    lblMsg.Text = ""
                    If blnItem Then ' item exists

                        'If Session("Env") = "FTN" Then
                        '    dt = objPR.getPRApprFlow(True)
                        'Else
                        '    dt = objPR.getPRApprFlow(False)
                        'End If
                        dt = objPR.getPRApprFlow(False)

                        'If Session("Env") <> "FTN" Then
                        '    If ViewState("BCM") > 0 Then
                        '        If checkMandatory(strMsg) Then
                        '            objPR.updatePR(dsPR, Session("NonFTN"))

                        '            Dim objBudget As New BudgetControl
                        '            Dim strBCM As String
                        '            Dim dtBCM As New DataTable
                        '            blnExceed = objBudget.checkBCM(hidNewPR.Value, dtBCM, strBCM)

                        '            'Dim dt As New DataTable
                        '            Dim intMsg As Integer

                        '            If Not blnExceed Then
                        '                If Session("Env") = "FTN" Then
                        '                    dt = objPR.getPRApprFlow(True)
                        '                Else
                        '                    dt = objPR.getPRApprFlow(False)
                        '                End If

                        '                ViewState("mode") = Request.QueryString("mode")
                        '                If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                        '                    Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                        '                    Exit Sub
                        '                ElseIf dt.Rows.Count > 1 Then
                        '                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                        '                    Exit Sub
                        '                Else
                        '                    Dim dsAO As New DataSet
                        '                    Dim UM_APP_LIMIT As Double
                        '                    Dim PR_Type As String = ""
                        '                    Dim blnBuyerWNoWork As Boolean = False

                        '                    If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                        '                        blnBuyerWNoWork = True
                        '                    Else
                        '                        ViewState("ApprovalType") = objPR.getApprovalType
                        '                        dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                        '                        ' case Automated Approval
                        '                        If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                        '                            UM_APP_LIMIT = 0
                        '                        Else
                        '                            UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                        '                        End If

                        '                        ' A - Automated Approval
                        '                        ' B - Allow Lower Limit Endorsement
                        '                        ' C - Cut PO before end of Aproval List
                        '                        ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                        '                        Select Case ViewState("ApprovalType")
                        '                            Case "C"
                        '                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                        '                                    If ViewState("blnCutPR") = True Then
                        '                                        PR_Type = "None"
                        '                                    Else
                        '                                        PR_Type = "Approval"
                        '                                        ViewState("blnCutPR") = True
                        '                                    End If
                        '                                Else
                        '                                    PR_Type = "None"
                        '                                End If

                        '                            Case "B"
                        '                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                        '                                    PR_Type = "Approval"
                        '                                Else
                        '                                    PR_Type = "Endorsement"
                        '                                End If

                        '                            Case "B+C"
                        '                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                        '                                    If ViewState("blnCutPR") = True Then
                        '                                        PR_Type = "None"
                        '                                    Else
                        '                                        PR_Type = "Approval"
                        '                                        ViewState("blnCutPR") = True
                        '                                    End If
                        '                                Else
                        '                                    PR_Type = "Endorsement"
                        '                                End If

                        '                            Case "A"
                        '                                If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                        '                                    PR_Type = "Approval"
                        '                                Else
                        '                                    PR_Type = "None"
                        '                                End If

                        '                        End Select

                        '                        Select Case PR_Type
                        '                            Case "None"
                        '                                PR_Type = "0"
                        '                            Case "Approval"
                        '                                PR_Type = "1"
                        '                            Case "Endorsement"
                        '                                PR_Type = "2"
                        '                        End Select
                        '                    End If

                        '                    If blnBuyerWNoWork = True Then
                        '                        ViewState("msg") = Nothing
                        '                    Else
                        '                        ViewState("msg") = 0 ' Follow the ApprovalSetup Concept/Flow which is "0"
                        '                    End If

                        '                    intMsg = objPR.submitPR(hidNewPR.Value, PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)
                        '                    Select Case intMsg
                        '                        Case WheelMsgNum.Save
                        '                            If Session("urlreferer") = "BuyerCatSearch" Then
                        '                                Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                        '                            ElseIf Session("urlreferer") = "ConCatSearch" Then
                        '                                Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                        '                            ElseIf Session("urlreferer") = "PRAll" Then
                        '                                Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                        '                            End If

                        '                        Case WheelMsgNum.NotSave
                        '                            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                        '                        Case WheelMsgNum.Delete
                        '                            Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

                        '                    End Select
                        '                End If
                        '            Else

                        '                If strBCM = "2" Then
                        '                    If Session("Env") = "FTN" Then
                        '                        dt = objPR.getPRApprFlow(True)
                        '                    Else
                        '                        dt = objPR.getPRApprFlow(False)
                        '                    End If

                        '                    ViewState("mode") = Request.QueryString("mode")
                        '                    If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                        '                        Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                        '                        Exit Sub
                        '                    ElseIf dt.Rows.Count > 1 Then
                        '                        Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=1&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                        '                        Exit Sub
                        '                    Else
                        '                        Dim dsAO As New DataSet
                        '                        Dim UM_APP_LIMIT As Double
                        '                        Dim PR_Type As String = ""
                        '                        Dim blnBuyerWNoWork As Boolean = False

                        '                        If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                        '                            blnBuyerWNoWork = True
                        '                        Else
                        '                            ViewState("ApprovalType") = objPR.getApprovalType
                        '                            dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                        '                            ' case Automated Approval
                        '                            If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                        '                                UM_APP_LIMIT = 0
                        '                            Else
                        '                                UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                        '                            End If

                        '                            ' A - Automated Approval
                        '                            ' B - Allow Lower Limit Endorsement
                        '                            ' C - Cut PO before end of Aproval List
                        '                            ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                        '                            Select Case ViewState("ApprovalType")
                        '                                Case "C"
                        '                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                        '                                        If ViewState("blnCutPR") = True Then
                        '                                            PR_Type = "None"
                        '                                        Else
                        '                                            PR_Type = "Approval"
                        '                                            ViewState("blnCutPR") = True
                        '                                        End If
                        '                                    Else
                        '                                        PR_Type = "None"
                        '                                    End If

                        '                                Case "B"
                        '                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                        '                                        PR_Type = "Approval"
                        '                                    Else
                        '                                        PR_Type = "Endorsement"
                        '                                    End If

                        '                                Case "B+C"
                        '                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                        '                                        If ViewState("blnCutPR") = True Then
                        '                                            PR_Type = "None"
                        '                                        Else
                        '                                            PR_Type = "Approval"
                        '                                            ViewState("blnCutPR") = True
                        '                                        End If
                        '                                    Else
                        '                                        PR_Type = "Endorsement"
                        '                                    End If

                        '                                Case "A"
                        '                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                        '                                        PR_Type = "Approval"
                        '                                    Else
                        '                                        PR_Type = "None"
                        '                                    End If

                        '                            End Select

                        '                            Select Case PR_Type
                        '                                Case "None"
                        '                                    PR_Type = "0"
                        '                                Case "Approval"
                        '                                    PR_Type = "1"
                        '                                Case "Endorsement"
                        '                                    PR_Type = "2"
                        '                            End Select
                        '                        End If

                        '                        intMsg = objPR.submitPR(hidNewPR.Value, PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)
                        '                        Select Case intMsg
                        '                            Case WheelMsgNum.Save
                        '                                If Session("urlreferer") = "BuyerCatSearch" Then
                        '                                    Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                        '                                ElseIf Session("urlreferer") = "ConCatSearch" Then
                        '                                    Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                        '                                ElseIf Session("urlreferer") = "PRAll" Then
                        '                                    Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                        '                                End If

                        '                            Case WheelMsgNum.NotSave
                        '                                Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                        '                            Case WheelMsgNum.Delete
                        '                                Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

                        '                        End Select
                        '                    End If
                        '                Else
                        '                    Response.Redirect(dDispatcher.direct("PR", "ExceedBCM.aspx", "prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId & "&mode=" & ViewState("mode")))
                        '                End If
                        '            End If
                        '        Else
                        '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        '        End If
                        '    Else
                        '        'If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                        '        '    Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                        '        '    Exit Sub
                        '        'ElseIf dt.Rows.Count > 1 Then
                        '        '    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                        '        '    Exit Sub
                        '        'Else
                        '        objPR.updatePR(dsPR, Session("NonFTN"))
                        '        SubmitPR()
                        '        'End If
                        '        'Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId))
                        '    End If
                        'Else
                        '    'If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                        '    '    Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                        '    '    Exit Sub
                        '    'ElseIf dt.Rows.Count > 1 Then
                        '    '    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                        '    '    Exit Sub
                        '    'Else
                        '    objPR.updatePR(dsPR, Session("NonFTN"))
                        '    SubmitPR()
                        '    'End If
                        'End If
                        If ViewState("BCM") > 0 Then
                            If checkMandatory(strMsg) Then
                                objPR_Ext.updatePR(dsPR, Session("NonFTN"))

                                Dim objBudget As New BudgetControl
                                Dim strBCM As String
                                Dim dtBCM As New DataTable
                                blnExceed = objBudget.checkBCM(hidNewPR.Value, dtBCM, strBCM)

                                'Dim dt As New DataTable
                                Dim intMsg As Integer

                                If Not blnExceed Then
                                    'If Session("Env") = "FTN" Then
                                    '    dt = objPR.getPRApprFlow(True)
                                    'Else
                                    '    dt = objPR.getPRApprFlow(False)
                                    'End If
                                    dt = objPR.getPRApprFlow(False)

                                    ViewState("mode") = Request.QueryString("mode")
                                    If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                                        Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                        Exit Sub
                                    ElseIf dt.Rows.Count > 1 Then
                                        If Request.QueryString("frm") = "BuyerCat" Then
                                            Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & cboCurr.SelectedItem.Value))
                                        ElseIf Request.QueryString("frm") = "ConCat" Then
                                            Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & cboCurr.SelectedItem.Value))
                                        ElseIf Not Request.QueryString("type") = "" Then
                                            If Request.QueryString("mode") = "bc" Then
                                                Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & cboCurr.SelectedItem.Value))
                                            Else
                                                Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & cboCurr.SelectedItem.Value))
                                            End If
                                        End If
                                        Exit Sub
                                    Else
                                        Dim dsAO As New DataSet
                                        Dim UM_APP_LIMIT As Double
                                        Dim PR_Type As String = ""
                                        Dim blnBuyerWNoWork As Boolean = False

                                        If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                                            blnBuyerWNoWork = True
                                        Else
                                            ViewState("ApprovalType") = objPR.getApprovalType
                                            dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                                            ' case Automated Approval
                                            If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                                                UM_APP_LIMIT = 0
                                            Else
                                                UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                                            End If

                                            ' A - Automated Approval
                                            ' B - Allow Lower Limit Endorsement
                                            ' C - Cut PO before end of Aproval List
                                            ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                                            Select Case ViewState("ApprovalType")
                                                Case "C"
                                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                                        If ViewState("blnCutPR") = True Then
                                                            PR_Type = "None"
                                                        Else
                                                            PR_Type = "Approval"
                                                            ViewState("blnCutPR") = True
                                                        End If
                                                    Else
                                                        PR_Type = "None"
                                                    End If

                                                Case "B"
                                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                                        PR_Type = "Approval"
                                                    Else
                                                        PR_Type = "Endorsement"
                                                    End If

                                                Case "B+C"
                                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                                        If ViewState("blnCutPR") = True Then
                                                            PR_Type = "None"
                                                        Else
                                                            PR_Type = "Approval"
                                                            ViewState("blnCutPR") = True
                                                        End If
                                                    Else
                                                        PR_Type = "Endorsement"
                                                    End If

                                                Case "A"
                                                    If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                                        PR_Type = "Approval"
                                                    Else
                                                        PR_Type = "None"
                                                    End If

                                            End Select

                                            Select Case PR_Type
                                                Case "None"
                                                    PR_Type = "0"
                                                Case "Approval"
                                                    PR_Type = "1"
                                                Case "Endorsement"
                                                    PR_Type = "2"
                                            End Select
                                        End If

                                        If blnBuyerWNoWork = True Then
                                            ViewState("msg") = Nothing
                                        Else
                                            ViewState("msg") = 0 ' Follow the ApprovalSetup Concept/Flow which is "0"
                                        End If

                                        intMsg = objPR.submitPR(hidNewPR.Value, PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)
                                        Select Case intMsg
                                            Case WheelMsgNum.Save
                                                If Session("urlreferer") = "BuyerCatSearch" Then
                                                    Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                                                ElseIf Session("urlreferer") = "ConCatSearch" Then
                                                    Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                                                ElseIf Session("urlreferer") = "PRAll" Then
                                                    Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                                                End If

                                            Case WheelMsgNum.NotSave
                                                Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                                            Case WheelMsgNum.Delete
                                                Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

                                        End Select
                                    End If
                                Else

                                    If strBCM = "2" Then
                                        'If Session("Env") = "FTN" Then
                                        '    dt = objPR.getPRApprFlow(True)
                                        'Else
                                        '    dt = objPR.getPRApprFlow(False)
                                        'End If
                                        dt = objPR.getPRApprFlow(False)

                                        ViewState("mode") = Request.QueryString("mode")
                                        If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                                            Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                            Exit Sub
                                        ElseIf dt.Rows.Count > 1 Then
                                            If Request.QueryString("frm") = "BuyerCat" Then
                                                Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=1&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & cboCurr.SelectedItem.Value))
                                            ElseIf Request.QueryString("frm") = "ConCat" Then
                                                Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=1&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & cboCurr.SelectedItem.Value))
                                            ElseIf Not Request.QueryString("type") = "" Then
                                                If Request.QueryString("mode") = "bc" Then
                                                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=1&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & cboCurr.SelectedItem.Value))
                                                Else
                                                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=1&prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & cboCurr.SelectedItem.Value))
                                                End If
                                            End If

                                            Exit Sub
                                        Else
                                            Dim dsAO As New DataSet
                                            Dim UM_APP_LIMIT As Double
                                            Dim PR_Type As String = ""
                                            Dim blnBuyerWNoWork As Boolean = False

                                            If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                                                blnBuyerWNoWork = True
                                            Else
                                                ViewState("ApprovalType") = objPR.getApprovalType
                                                dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                                                ' case Automated Approval
                                                If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                                                    UM_APP_LIMIT = 0
                                                Else
                                                    UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                                                End If

                                                ' A - Automated Approval
                                                ' B - Allow Lower Limit Endorsement
                                                ' C - Cut PO before end of Aproval List
                                                ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                                                Select Case ViewState("ApprovalType")
                                                    Case "C"
                                                        If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                                            If ViewState("blnCutPR") = True Then
                                                                PR_Type = "None"
                                                            Else
                                                                PR_Type = "Approval"
                                                                ViewState("blnCutPR") = True
                                                            End If
                                                        Else
                                                            PR_Type = "None"
                                                        End If

                                                    Case "B"
                                                        If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                                            PR_Type = "Approval"
                                                        Else
                                                            PR_Type = "Endorsement"
                                                        End If

                                                    Case "B+C"
                                                        If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) <= CDbl(UM_APP_LIMIT) Then
                                                            If ViewState("blnCutPR") = True Then
                                                                PR_Type = "None"
                                                            Else
                                                                PR_Type = "Approval"
                                                                ViewState("blnCutPR") = True
                                                            End If
                                                        Else
                                                            PR_Type = "Endorsement"
                                                        End If

                                                    Case "A"
                                                        If CDbl(Format(CDbl(ViewState("PRCost")), "#0.0000")) < CDbl(UM_APP_LIMIT) Then
                                                            PR_Type = "Approval"
                                                        Else
                                                            PR_Type = "None"
                                                        End If

                                                End Select

                                                Select Case PR_Type
                                                    Case "None"
                                                        PR_Type = "0"
                                                    Case "Approval"
                                                        PR_Type = "1"
                                                    Case "Endorsement"
                                                        PR_Type = "2"
                                                End Select
                                            End If

                                            intMsg = objPR.submitPR(hidNewPR.Value, PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)
                                            Select Case intMsg
                                                Case WheelMsgNum.Save
                                                    If Session("urlreferer") = "BuyerCatSearch" Then
                                                        Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                                                    ElseIf Session("urlreferer") = "ConCatSearch" Then
                                                        Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                                                    ElseIf Session("urlreferer") = "PRAll" Then
                                                        Common.NetMsgbox(Me, "Purchase Request Number " & hidNewPR.Value & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                                                    End If

                                                Case WheelMsgNum.NotSave
                                                    Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                                                Case WheelMsgNum.Delete
                                                    Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

                                            End Select
                                        End If
                                    Else
                                        Response.Redirect(dDispatcher.direct("PR", "ExceedBCM.aspx", "prid=" & hidNewPR.Value & "&prcost=" & Format(CDbl(ViewState("PRCost")), "#0.0000") & "&currency=" & cboCurr.SelectedItem.Value & "&pageid=" & strPageId & "&mode=" & ViewState("mode")))
                                    End If
                                End If
                            Else
                                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                            End If
                        Else
                            'If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                            '    Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                            '    Exit Sub
                            'ElseIf dt.Rows.Count > 1 Then
                            '    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency")))
                            '    Exit Sub
                            'Else
                            objPR_Ext.updatePR(dsPR, Session("NonFTN"))
                            SubmitPR()
                            'End If
                            'Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "msg=0&prid=" & hidNewPR.Value & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId))
                        End If
                    Else
                        Common.NetMsgbox(Me, "There are no items in this PR.", MsgBoxStyle.Information)
                    End If
                Else
                    If blnValidBill = False Then
                        Common.NetMsgbox(Me, objGLO.GetErrorMessage("00031"), MsgBoxStyle.Information)
                    End If
                End If

                Select Case ViewState("mode")
                    Case "bc"
                End Select
            End If
        Else
            'Stage 3 Bug fix (GST-0024) - 08/07/2015 - CH
            savePRItemInArray()

            If strMsg <> "" Then
                lblMsg.Text = strMsg
            Else
                lblMsg.Text = ""
            End If
        End If
    End Sub

    Sub SubmitPR()
        Dim dt As New DataTable
        Dim objPR As New PR
        Dim objPR2 As New PurchaseReq2
        Dim objPO1 As New PurchaseOrder_Buyer
        Dim intMsg As Integer

        'If Session("Env") = "FTN" Then
        '    dt = objPR.getPRApprFlow(True)
        'Else
        '    dt = objPR.getPRApprFlow(False)
        'End If
        dt = objPR.getPRApprFlow(False)

        If ViewState("type") = "new" Then
            Select Case ViewState("mode")
                Case "bc"
                Case "cc"
            End Select
        ElseIf ViewState("type") = "mod" Then
            If Session("urlreferer") <> "Dashboard" Then
                Session("urlreferer") = "PRAll"
            End If
            Select Case ViewState("mode")
                Case "bc"
                Case "cc"
            End Select
        End If

        If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
            Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
            Exit Sub
        ElseIf dt.Rows.Count > 1 Then 'more than 1 approval flow
            If Request.QueryString("frm") = "BuyerCat" Then
                Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=0&prid=" & lblPRNo.Text & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & cboCurr.SelectedItem.Value))
            ElseIf Request.QueryString("frm") = "ConCat" Then
                Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=0&prid=" & lblPRNo.Text & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & cboCurr.SelectedItem.Value))
            ElseIf Not Request.QueryString("type") = "" Then
                If Request.QueryString("mode") = "bc" Then
                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=bc&pageid=" & strPageId & "&msg=0&prid=" & lblPRNo.Text & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & cboCurr.SelectedItem.Value))
                Else
                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "mode=cc&pageid=" & strPageId & "&msg=0&prid=" & lblPRNo.Text & "&prcost=" & Format((CDbl(ViewState("PRCost"))), "#0.00") & "&currency=" & cboCurr.SelectedItem.Value))
                End If
            End If
            Exit Sub
        Else ' no approval flow or 1 approval workflow only
            Dim dsAO As New DataSet
            Dim UM_APP_LIMIT As Double
            Dim PR_Type As String = ""
            Dim blnBuyerWNoWork As Boolean = False

            If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                blnBuyerWNoWork = True
            Else
                ViewState("ApprovalType") = objPR.getApprovalType
                dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                ' case Automated Approval
                If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                    UM_APP_LIMIT = 0
                Else
                    UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                End If

                ' A - Automated Approval
                ' B - Allow Lower Limit Endorsement
                ' C - Cut PO before end of Aproval List
                ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                Select Case ViewState("ApprovalType")
                    Case "C"
                        If CDbl(ViewState("prcost")) < CDbl(UM_APP_LIMIT) Then
                            If ViewState("blnCutPR") = True Then
                                PR_Type = "None"
                            Else
                                PR_Type = "Approval"
                                ViewState("blnCutPR") = True
                            End If
                        Else
                            PR_Type = "None"
                        End If

                    Case "B"
                        If CDbl(ViewState("prcost")) <= CDbl(UM_APP_LIMIT) Then
                            PR_Type = "Approval"
                        Else
                            PR_Type = "Endorsement"
                        End If

                    Case "B+C"
                        If CDbl(ViewState("prcost")) <= CDbl(UM_APP_LIMIT) Then
                            If ViewState("blnCutPR") = True Then
                                PR_Type = "None"
                            Else
                                PR_Type = "Approval"
                                ViewState("blnCutPR") = True
                            End If
                        Else
                            PR_Type = "Endorsement"
                        End If

                    Case "A"
                        If CDbl(ViewState("prcost")) < CDbl(UM_APP_LIMIT) Then
                            PR_Type = "Approval"
                        Else
                            PR_Type = "None"
                        End If

                End Select

                Select Case PR_Type
                    Case "None"
                        PR_Type = "0"
                    Case "Approval"
                        PR_Type = "1"
                    Case "Endorsement"
                        PR_Type = "2"
                End Select
            End If
            'added by michael
            If blnBuyerWNoWork = True Then
                ViewState("msg") = Nothing
            Else
                ViewState("msg") = 0 ' Follow the ApprovalSetup Concept/Flow which is "0"
            End If
            ''''''''''''''''''''''''''
            intMsg = objPR.submitPR(lblPRNo.Text, PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)

            'If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
            '    Dim strRemark, strMsg, strSql As String

            '    'strRemark = FormatAORemark("approve")
            '    'strRemark = strRemark & "System Auto Approval"

            '    strRemark = ""

            '    strSql = "SELECT PRM_PR_INDEX FROM PR_MSTR WHERE PRM_COY_ID= '" & Session("CompanyId") & "' AND PRM_PR_NO ='" & lblPRNo.Text & "'"
            '    ViewState("PRIndex") = objDB.GetVal(strSql)

            '    'strMsg = objPR2.ApprovePR(lblPRNo.Text, ViewState("PRIndex"), ViewState("CurrentAppSeq"), ViewState("ISHighestLevel"), ViewState("Consolidator"), strRemark, ViewState("Requestor"), Request.QueryString("relief"), ViewState("ApprType"))
            '    strMsg = objPR2.ApprovePR(lblPRNo.Text, ViewState("PRIndex"), 1, True, String.Empty, strRemark, Session("UserId"), False, 1, "BCNoWorkFlow")
            'End If

            Select Case intMsg
                Case WheelMsgNum.Save
                    If Session("urlreferer") = "BuyerCatSearch" Then
                        Common.NetMsgbox(Me, "Purchase Request Number " & lblPRNo.Text & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "ConCatSearch" Then
                        Common.NetMsgbox(Me, "Purchase Request Number " & lblPRNo.Text & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "PRAll" Then
                        Common.NetMsgbox(Me, "Purchase Request Number " & lblPRNo.Text & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "Dashboard" Then
                        Common.NetMsgbox(Me, "Purchase Request Number " & lblPRNo.Text & " has been submitted.", dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId))
                        'lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
                    End If

                    If Request.QueryString("frm") = "bc" Then
                        cmdAdd.Visible = False
                        cmdRemove.Visible = False
                        cmdDupPRLine.Visible = False
                    End If

                    cmdRaise.Visible = False
                    cmdSetup.Visible = False
                    cmdSubmit.Visible = False

                Case WheelMsgNum.NotSave
                    Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                Case WheelMsgNum.Delete
                    Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

            End Select
        End If
    End Sub

    Private Sub cboCurr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCurr.SelectedIndexChanged
        Dim chkItem As CheckBox
        Dim dgItem As DataGridItem
        Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark As TextBox
        Dim txtBudget, txtDelivery As Label
        Dim cboGLCode, ddlDelivery, ddl_stock As DropDownList
        Dim strItem, strItemCust As New ArrayList()
        Dim k As Integer = 0
        Dim strItemHead As New ArrayList()
        Dim objAdmin As New Admin

        'If cboCurr.SelectedIndex <> 0 Then
        strItemHead.Add(New String() {txtAttention.Text, txtInternal.Text, txtExternal.Text, chkUrgent.Checked, cboPayTerm.SelectedIndex, cboPayMethod.SelectedIndex, cboCurr.SelectedIndex})

        For Each dgItem In dtgShopping.Items
            txtQty = dgItem.FindControl("txtQty")
            cboGLCode = dgItem.FindControl("cboGLCode")
            txtPrice = dgItem.FindControl("txtPrice")
            txtBudget = dgItem.FindControl("txtBudget")
            hidBudgetCode = dgItem.FindControl("hidBudgetCode")
            txtDelivery = dgItem.FindControl("txtDelivery")
            hidDelCode = dgItem.FindControl("hidDelCode")
            txtEstDate = dgItem.FindControl("txtEstDate")
            txtWarranty = dgItem.FindControl("txtWarranty")
            txtRemark = dgItem.FindControl("txtRemark")

            ddlDelivery = dgItem.FindControl("ddlDelivery")
            ddl_stock = dgItem.FindControl("ddl_stock")

            chkItem = dgItem.FindControl("chkSelection")
            'If chkItem.Checked Then
            'Else
            '    strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, ddlDelivery.SelectedIndex, ddl_stock.SelectedIndex})
            'End If
            strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, ddlDelivery.SelectedIndex, ddl_stock.SelectedIndex})

            'Dim objAdmin As New Admin

            dvwCus = objAdmin.getCustomField("")

            If Not dvwCus Is Nothing Then
                For i = 0 To dvwCus.Count - 1
                    'For Each dgItem2 In dtgShopping.Items
                    Dim cboCustom As DropDownList
                    cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                    If chkItem.Checked Then
                    Else
                        strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                    End If
                    'Next
                Next
            End If

            k += 1
        Next
        Session("strItem") = strItem
        Session("strItemCust") = strItemCust
        Session("strItemHead") = strItemHead

        Bindgrid()

        objAdmin = Nothing
        'Else
        'End If
    End Sub
End Class
