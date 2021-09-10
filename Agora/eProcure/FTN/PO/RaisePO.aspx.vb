Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions

Imports System.Drawing
Public Class RaisePOFTN
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim keepAr As New ArrayList
    Dim keepArPost As New ArrayList
    Dim PO_Type As String = ""

    Public Enum EnumShoppingCart
        icChk = 0
        icNo = 1
        'icBuyerItemCode = 2
        icProductCode = 2
        icVendorItemCode = 3
        icGLCode = 4
        icGLCode1 = 5
        icCategoryCode1 = 6
        icTaxCode = 7
        icItemDesc = 8 '8
        'icMOQ = 10 '9
        'icMPQ = 11 '10
        icRfqQty = 9 '11
        icTolerance = 10 '12
        icQty = 11 '13
        icUOM = 12 '14
        icPrice = 13 '15
        icTotal = 14 '16
        'icTax = 15
        icGSTRate = 15
        icTax = 16
        icGstTaxCode = 17 'Stage 3 Enhancement (GST-0010) - 14/07/2015 - CH 
        icSource = 18 '17 '17 'Jules 20140807: From here onwards, add 1
        icCategoryCode = 19 '18
        icCDGroup = 20 '19
        icBudget = 21 '20
        icDelivery = 22 '21
        icEstDate = 23 '22
        icWarranty = 24 '23
        ' Yap : Re-Add this 2 columns
        icMOQ = 25 '24 '9
        icMPQ = 26 '25 '10
        icRemark = 27 '26 '25        
    End Enum
    Dim dvwDelivery, dvwGLCode, dvwCategoryCode As DataView
    Dim dvwBill As DataView
    Dim dvwCustom() As DataView
    Dim dvwCus As New DataView
    Dim dtBCM, dtGLCode, dtCategoryCode As DataTable
    Dim strCustomDefault() As String
    Dim strDeliveryDefault As String
    Dim strBillDefault As String
    Dim strCountryDefault As String
    Dim intRow As Integer
    Dim intCnt As Integer
    Dim dblTotal As Double = 0
    Dim dblSubTotal As Double = 0
    Dim dblNoTaxTotal, dblTaxTotal, dblTotalGst As Double
    Dim intGSTcnt, intNoGSTcnt, intTotItem As Integer
    Dim dvwCustomItem As DataView
    Dim objDB As New EAD.DBCom
    Dim objGlobal As New AppGlobals
    Dim blnItem As Boolean
    Dim i As Integer
    Dim intCount As Integer
    Dim strIndexList As String
    Dim blnValid As Boolean
    Dim strDefDelivery As String
    Dim blnGSTCOD As Boolean
    Dim chkGST As String

    Dim aryProdCodeNew As New ArrayList
    Dim aryVendorNew As New ArrayList
    Dim dtVendorNew As New DataTable
    Protected WithEvents body1 As HtmlGenericControl
    Protected WithEvents btnHidden1 As System.Web.UI.WebControls.Button
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblPONo As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents revPaymentTerm As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents revPaymentMethod As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents revShipmentTerm As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents revShipmentMode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents txtBillPostCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents revPostcode As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents txtBillCity As System.Web.UI.WebControls.TextBox
    Protected WithEvents cboState As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboCountry As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblFile As System.Web.UI.WebControls.Label
    Protected WithEvents lblAttach As System.Web.UI.WebControls.Label
    Protected WithEvents pnlAttach As System.Web.UI.WebControls.Panel
    'Protected WithEvents chkCustom As System.Web.UI.WebControls.CheckBox
    'Protected WithEvents chkRemark As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkUrgent As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdRemove As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents File1 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents hidTotal As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidTax As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidCnt As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidCost As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidNewPO As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidAddItem As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidSupplier As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidApproval As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidDelete As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hid1 As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hid2 As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hid3 As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hid4 As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hid5 As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hid6 As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hid7 As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblSupplier As System.Web.UI.WebControls.Label
    Protected WithEvents txtRequestedName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFreightTerm As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRequestedContact As System.Web.UI.WebControls.TextBox
    Protected WithEvents cboPayTerm As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboPayMethod As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboShipmentTerm As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboShipmentMode As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboVendor As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtShipVia As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblCurrencyCode As System.Web.UI.WebControls.Label
    Protected WithEvents txtAttention As System.Web.UI.WebControls.TextBox
    Protected WithEvents cboBillCode As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents cboDelivery As System.Web.UI.WebControls.DropDownList
    Protected WithEvents hidDelCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBillAdd1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBillAdd2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBillAdd3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtInternal As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtExternal As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdUpload As System.Web.UI.WebControls.Button
    Protected WithEvents dtgShopping As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdRaise As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDupPOLine As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSetup As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button

    Protected WithEvents hidClientId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidTotalClientId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidGST As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidOneVendor As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtShippingHandling As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblTax As System.Web.UI.WebControls.Label

    Dim cmdDelivery As HtmlInputButton
    Dim strQtyErr As String

    Dim kk As Integer = 0

    'Dim blnCmdAdd As Boolean
    'Dim blnCmdDelete As Boolean
    'Dim blnCmdRaise As Boolean
    'Dim blnCmdUpload As Boolean
    'Dim blnCmdRemove As Boolean
    'Dim blnCmdSetup As Boolean

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
        'alButtonList.Add(cmdSubmit)
        alButtonList.Add(cmdSetup)
        alButtonList.Add(cmdUpload)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        alButtonList.Add(cmdRaise)
        ' alButtonList.Add(cmdSubmit)
        alButtonList.Add(cmdSetup)
        alButtonList.Add(cmdUpload)
        alButtonList.Add(cmdRemove)
        alButtonList.Add(cmdDupPOLine)
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdDelete)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        cmdRaise.Enabled = blnCanUpdate And blnCanAdd And ViewState("blnCmdRaise")
        'cmdSubmit.Enabled = blnCanUpdate And blnCanAdd And ViewState("blnCmdSubmit")
        cmdSetup.Enabled = blnCanAdd And blnCanUpdate And ViewState("blnCmdSetup")
        cmdAdd.Enabled = blnCanAdd And blnCanUpdate And ViewState("blnCmdAdd")
        cmdUpload.Enabled = blnCanAdd And blnCanUpdate And ViewState("blnCmdUpload")
        cmdRemove.Enabled = blnCanUpdate And ViewState("blnCmdRemove")
        cmdDupPOLine.Enabled = blnCanUpdate And ViewState("blnCmdRemove")
        cmdDelete.Enabled = blnCanDelete And ViewState("blnCmdDelete")
        alButtonList.Clear()

        If ViewState("modePR") = "pr" Then
            cmdRaise.Enabled = False
            cmdUpload.Enabled = False
        End If
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)

        'Response.Expires = -1
        'Response.AddHeader("cache-control", "private")
        'Response.AddHeader("pragma", "no-cache")
        'Response.CacheControl = "no-cache"

        'If Not Request.QueryString("frm") = "PC" And Session("CurrentScreen") = "RemoveItem" And Not IsPostBack Then
        '    Session("CurrentScreen") = ""
        '    Exit Sub
        'End If

        blnPaging = False
        blnSorting = False
        SetGridProperty(dtgShopping)

        Dim strUserRole As String
        aryProdCodeNew.Clear()
        dtVendorNew.Clear()
        dtVendorNew.Columns.Add("Prefer", Type.GetType("System.String"))
        dtVendorNew.Columns.Add("1st", Type.GetType("System.String"))
        dtVendorNew.Columns.Add("2nd", Type.GetType("System.String"))
        dtVendorNew.Columns.Add("3rd", Type.GetType("System.String"))

        If Session("CurrentScreen") <> "RemoveItem" Then
            Session("strItem") = Nothing
            Session("strItemCust") = Nothing
            Session("strItemHead") = Nothing
        End If

        Dim objGST As New GST
        ViewState("isGST") = objGST.chkGSTCOD()
        hidGST.Value = ViewState("isGST")

        If Not IsPostBack Then
            Session("strPutOnce") = ""
            strQtyErr = objGlobal.GetErrorMessage("00342")
            ViewState("ValQtyMsg") = strQtyErr

            Session("CurrentScreen") = ""
            GenerateTab()
            'Construct URL for remove / delete
            If Request.QueryString("frm") = "PC" Then
                'Construct URL For PC
                Session("RaisePOURL") = dDispatcher.direct("PO", "RaisePO.aspx", "frm=PC&pageid=" & strPageId)

            Else
                'Construct URL For My PO
                Session("RaisePOURL") = dDispatcher.direct("PO", "RaisePO.aspx", "pageid=" & strPageId & "&index=" & Request.QueryString("index") & "&poid=" & Request.QueryString("poid") & "&type=" & Request.QueryString("type") & "&mode=" & Request.QueryString("mode"))

            End If

            'If Session("Env") = "FTN" Then
            '    Me.cmdSubmit.Visible = "True"
            '    Me.cmdSetup.Visible = "False"
            'Else
            '    Me.cmdSubmit.Visible = "False"
            '    Me.cmdSetup.Visible = "True"
            'End If

            ViewState("blnCmdDelete") = True
            If Request.QueryString("frm") = "PC" Then 'ie From Purchasing Catalogue
                ViewState("type") = "new"
                ViewState("mode") = "po"
            ElseIf Not Request.QueryString("type") = "" Then
                If ViewState("mode") = "rfq" Then
                    ViewState("blnCmdDelete") = False
                End If
                ViewState("type") = Request.QueryString("type")
                ViewState("mode") = Request.QueryString("mode")
            End If
            ViewState("strPageId") = strPageId
            ViewState("blnCmdAdd") = True

            ViewState("blnCmdRaise") = True
            'ViewState("blnCmdSubmit") = True
            ViewState("blnCmdUpload") = True
            ViewState("blnCmdRemove") = True
            ViewState("blnCmdSetup") = True
            ViewState("postback") = 0
            ViewState("SuppId") = ""
            Dim objAdmin As New Admin
            Dim objPR As New PR
            Dim dsDelivery As New DataSet
            Dim dsBill As New DataSet
            Dim objBudget As New BudgetControl

            objGlobal.FillCodeTable(cboPayTerm, CodeTable.PaymentTerm)
            objGlobal.FillCodeTable(cboPayMethod, CodeTable.PaymentMethod)
            objGlobal.FillCodeTable(cboShipmentTerm, CodeTable.ShipmentTerm)
            Common.SelDdl("99", cboShipmentTerm, True, True)
            objGlobal.FillCodeTable(cboShipmentMode, CodeTable.ShipmentMode)
            Common.SelDdl("99", cboShipmentMode, True, True)
            objGlobal.FillAddress(cboBillCode, "B")
            objGlobal.FillCodeTable(cboCountry, CodeTable.Country)
            ViewState("CountryDefault") = cboCountry.SelectedItem.Value
            objGlobal.FillState(cboState, cboCountry.SelectedItem.Value)
            ViewState("blnAllowFreeForm") = objAdmin.getAllowFreeForm

            ViewState("blnBill") = 1
            If ViewState("blnAllowFreeForm") Then
                Dim lstItem As New ListItem
                lstItem.Value = "F"
                lstItem.Text = "Free Form"
                cboBillCode.Items.Add(lstItem)
            ElseIf cboBillCode.Items.Count = 1 Then
                ViewState("blnBill") = 0
            End If

            dsDelivery = objAdmin.PopulateAddr("D", "", "", "")
            dvwDelivery = dsDelivery.Tables(0).DefaultView
            strDeliveryDefault = objAdmin.user_Default_Add_ByDefault("D")
            ViewState("strDeliveryDefault") = strDeliveryDefault

            dsBill = objAdmin.PopulateAddr("B", "", "", "")
            dvwBill = dsBill.Tables(0).DefaultView
            strBillDefault = objAdmin.user_Default_Add_ByDefault("B")
            ViewState("strBillDefault") = strBillDefault

            'Common.FillDdl(cboDelivery, "FullAddress", "AM_ADDR_CODE", dvwDelivery)
            'Common.SelDdl(strDeliveryDefault, cboDelivery, True, True)
            'If cboDelivery.Items.Count = 0 Then
            '    Dim lstItem As New ListItem
            '    lstItem.Value = ""
            '    lstItem.Text = "---Select---"
            '    cboDelivery.Items.Insert(0, lstItem)
            '    ViewState("blnDelivery") = 0
            'Else
            '    strDefDelivery = Mid(cboDelivery.SelectedItem.Text, 1, 10)
            '    ViewState("blnDelivery") = 1
            'End If



            Dim dsBCM As New DataSet

            'Michelle (21/9/2010) - Temporarily disable the BCM module (ExclBCM)
            'viewstate("BCM") = CInt(objPR.checkBCM)
            'If Session("Env") <> "FTN" Then
            '    ViewState("BCM") = CInt(objPR.checkBCM)
            '    ViewState("BCM") = 0
            '    If ViewState("BCM") > 0 Then
            '        dsBCM = objBudget.getBCMListByUser(Session("UserId"))
            '        dtBCM = dsBCM.Tables(0)
            '    End If
            'End If

            Dim strGST As String
            cmdRaise.Attributes.Add("onclick", "return InitialValidation();")
            cmdSubmit.Attributes.Add("onclick", "return InitialValidation();")
            'cmdRaise.Attributes.Add("onclick", "return validateQty();")

            If ViewState("type") = "new" Then
                'cmdAdd.Visible = False
                'cmdRemove.Visible = False
                cmdDelete.Visible = False
                'cmdDupPOLine.Visible = False
                'ViewState("blnCmdAdd") = True
                'ViewState("blnCmdRemove") = True
                'ViewState("blnCmdDelete") = False
                ViewState("Vendor") = Request.QueryString("vendor")
                lblTitle.Text = "Raise Purchase Order"
                lblPONo.Text = "To Be Allocated By System"
                'ViewState("Currency") = Request.QueryString("currency")
                Common.SelDdl(objAdmin.user_Default_Add("B"), cboBillCode, True, True)
                fillAddress()
                enableBill(False)

                ' assign data to each textbox/label
                lblDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Today.Date)
                Dim dsCurrency As New DataSet
                'lblCurrencyCode.Text = ViewState("Currency")

                BindVendorList()

                If ViewState("strGST") = "" Then
                    ViewState("strGST") = "0"
                End If
                strIndexList = Session("ItemIndexList")
                Session.Remove("ItemIndexList")
                'chkCustom.Checked = True
                'chkRemark.Checked = True

                If Session("urlreferer") = "" Then
                    lnkBack.NavigateUrl = dDispatcher.direct("PO", "viewShoppingCart.aspx", "type=tab&pageid=" & strPageId)
                ElseIf Session("urlreferer") = "ConCatSearch" Then
                    lnkBack.NavigateUrl = dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId)
                Else
                    lnkBack.NavigateUrl = dDispatcher.direct("Search", Session("urlreferer") & ".aspx", "type=tab&pageid=" & strPageId)
                End If

                Select Case ViewState("mode")
                    Case "po", "cc"
                        dtgShopping.Columns(EnumShoppingCart.icRfqQty).Visible = False
                        dtgShopping.Columns(EnumShoppingCart.icTolerance).Visible = False
                        dtgShopping.Columns(EnumShoppingCart.icQty).HeaderText = "Quantity"
                    Case "rfq"
                        ViewState("rfqid") = Request.QueryString("rfqid")
                        ViewState("rfqnum") = Request(Trim("RFQ_Num"))
                        ViewState("quono") = Request.QueryString("quono")
                        ViewState("rfqname") = Request.QueryString("rfqname")

                        Dim objRFQ As New RFQ
                        Dim objRfq2 As New RFQ_User
                        objRFQ.read_rfqMstr(objRfq2, ViewState("rfqname"), ViewState("rfqid"), ViewState("rfqnum"))
                        Common.SelDdl(objRfq2.pay_term, cboPayTerm, True, True)
                        Common.SelDdl(objRfq2.pay_type, cboPayMethod, True, True)

                        ' ai chu modified on 16/11/2005
                        ' for shipment mode or shipment term, need to be retrieved from RFQ_REPLIES_MSTR 
                        Dim strShipMode, strShipTerm As String
                        objRFQ.getQuotationShipment(ViewState("rfqid"), strShipMode, strShipTerm)
                        Common.SelDdl(strShipMode, cboShipmentMode, True, True)
                        Common.SelDdl(strShipTerm, cboShipmentTerm, True, True)
                        objRFQ = Nothing
                        objRfq2 = Nothing

                        lnkBack.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "disabled=N&Frm=" & Request(Trim("SubFrm")) & "&RFQType=" & Request.QueryString("RFQType") & "&RFQ_Num=" & ViewState("rfqnum") & "&RFQ_Name=" & Server.UrlEncode(ViewState("rfqname")) & "&RFQ_ID=" & ViewState("rfqid") & "&pageid=" & strPageId & "&dpage=" & Request.QueryString("dpage"))

                        dtgShopping.Columns(EnumShoppingCart.icRfqQty).Visible = False
                        dtgShopping.Columns(EnumShoppingCart.icTolerance).Visible = False
                        dtgShopping.Columns(EnumShoppingCart.icQty).HeaderText = "PO Qty"

                        dtgShopping.Columns(EnumShoppingCart.icChk).Visible = False
                        dtgShopping.Columns(EnumShoppingCart.icVendorItemCode).Visible = False

                        Session("ProdList") = aryProdCodeNew
                        hidOneVendor.Value = 1

                        Dim strRFQ_No As String = objDB.GetVal("SELECT PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "'")
                        'If strRFQ_No <> "" Then
                        '    ViewState("modePR") = "pr"
                        'End If

                        If strRFQ_No <> "" Then
                            ViewState("modePR") = "pr"
                            ViewState("modeRFQFromPR_Index") = objDB.GetVal("SELECT PRM_PR_INDEX FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO = '" & objDB.GetVal("SELECT PRD_PR_NO FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "'") & "'")

                            Dim strUrgent As String = objDB.GetVal("SELECT IFNULL(PRM_URGENT, '0') AS PRM_URGENT FROM PR_DETAILS, PR_MSTR WHERE PRD_PR_NO = PRM_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "' ORDER BY PRM_URGENT DESC LIMIT 1")
                            If strUrgent = "1" Then
                                chkUrgent.Checked = True
                            Else
                                chkUrgent.Checked = False
                            End If
                        Else
                            ViewState("modeRFQFromPR_Index") = ""
                        End If
                End Select

            ElseIf ViewState("type") = "mod" Then
                'lnkBack.NavigateUrl = "../PO/SearchPR_All.aspx?caller=buyer&pageid=7" 'viewstate("urlreferer")
                If Session("urlreferer") = "" Then
                    lnkBack.NavigateUrl = dDispatcher.direct("PO", "SearchPR_All.aspx", "caller=buyer&pageid=" & strPageId)
                    If Me.Request.QueryString("Frm") = "Dashboard" Then
                        lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
                        'Session("urlreferer") = "Dashboard" & Request.QueryString("dpage")
                        Session("urlreferer") = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
                    End If
                Else
                    If Me.Request.QueryString("Frm") = "Dashboard" Then
                        lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
                        'Session("urlreferer") = "Dashboard" & Request.QueryString("dpage")
                        Session("urlreferer") = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
                    ElseIf Session("urlreferer") = "ConCatSearch" Then
                        lnkBack.NavigateUrl = dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId)
                    ElseIf Session("urlreferer") = "BuyerCatalogueSearch" Then
                        lnkBack.NavigateUrl = dDispatcher.direct("Search", "BuyerCatalogueSearch.aspx", "type=tab&pageid=" & strPageId)
                    ElseIf Session("urlreferer") = "RFQComSummary" Then
                        lnkBack.NavigateUrl = dDispatcher.direct("RFQ", "RFQ_List.aspx", "type=tab&pageid=" & strPageId)
                    Else
                        lnkBack.NavigateUrl = dDispatcher.direct("PO", Session("urlreferer") & ".aspx", "type=tab&pageid=" & strPageId)
                    End If
                End If

                cmdDelete.Visible = True
                ViewState("blnCmdDelete") = True
                ViewState("poid") = Request.QueryString("poid")

                lblPONo.Text = ViewState("poid")
                hidNewPO.Value = ViewState("poid")
                lblTitle.Text = "Raise Purchase Order"

                If IsNumeric(ViewState("POM_RFQ_INDEX")) Then
                    Session("ProdList") = aryProdCodeNew
                    hidOneVendor.Value = 1
                Else
                    GetCommonVendorAndProduct()
                    hidOneVendor.Value = 0
                End If

                dtgShopping.Columns(EnumShoppingCart.icRfqQty).Visible = False
                dtgShopping.Columns(EnumShoppingCart.icTolerance).Visible = False
                dtgShopping.Columns(EnumShoppingCart.icQty).HeaderText = "Quantity"

                Dim strPO_No As String = objDB.GetVal("SELECT PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("poid") & "'")
                If strPO_No <> "" Then
                    ViewState("modePR") = "pr"

                End If

                Dim strRFQ_No As String = objDB.GetVal("SELECT RM_RFQ_NO FROM PO_MSTR, RFQ_MSTR WHERE POM_RFQ_INDEX = RM_RFQ_ID AND POM_PO_NO = '" & ViewState("poid") & "'")
                If strRFQ_No <> "" Then
                    strRFQ_No = objDB.GetVal("SELECT PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & strRFQ_No & "'")
                    If strRFQ_No <> "" Then
                        ViewState("modePR") = "pr"
                        Dim strUrgent As String = objDB.GetVal("SELECT PRM_URGENT FROM PR_DETAILS, PR_MSTR WHERE PRD_PR_NO = PRM_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "' LIMIT 1")
                        If strUrgent = "1" Then
                            chkUrgent.Checked = True
                        Else
                            chkUrgent.Checked = False
                        End If
                    End If
                End If

                'If Session("PO_Type") = Nothing Then
                '    PO_Type = objDB.GetVal("SELECT IFNULL(POD_SOURCE,'') FROM PO_DETAILS WHERE POD_PO_NO = '" & ViewState("poid") & "'")
                '    If PO_Type = "CC" Then
                '        Session("PO_Type") = "PO_Type"
                '        ViewState("mode") = "cc"
                '    End If
                'Else
                '    ViewState("mode") = "cc"
                'End If

                PO_Type = objDB.GetVal("SELECT IFNULL(POD_SOURCE,'') FROM PO_DETAILS WHERE POD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POD_PO_NO = '" & ViewState("poid") & "'")
                If PO_Type = "CP" Or PO_Type = "CC" Then
                    ViewState("mode") = "cc"
                End If

                Select Case ViewState("mode")
                    Case "po"
                    Case "rfq"
                        strRFQ_No = objDB.GetVal("SELECT PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "'")
                        If strRFQ_No <> "" Then
                            ViewState("modePR") = "pr"
                            Dim strUrgent As String = objDB.GetVal("SELECT PRM_URGENT FROM PR_DETAILS, PR_MSTR WHERE PRD_PR_NO = PRM_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "' LIMIT 1")
                            If strUrgent = "1" Then
                                chkUrgent.Checked = True
                            Else
                                chkUrgent.Checked = False
                            End If
                        End If
                End Select

                Dim objPO2 As New PurchaseOrder_Buyer
                objPO2.delete_Attachment(ViewState("poid"))
                'insert those attachment records
                objPO2.insert_Attachment(ViewState("poid"))
                objPO2 = Nothing
            End If

            ' FTN Version no custom fields
            'addDataGridColumn()

            If ViewState("strGST") = "0" Then
                ViewState("GST") = "product"
            Else
                ViewState("GST") = "subtotal"
            End If


            Dim objPO As New PurchaseOrder
            Dim objPO1 As New PurchaseOrder_Buyer
            If objPO.isConvertedFromRFQ(ViewState("poid")) Then
                ViewState("ListingFromRFQ") = "True"
            End If

            'Jules #6973            
            chkGST = objGST.chkGST(cboVendor.SelectedValue)
            blnGSTCOD = objGST.chkGSTCOD()
            'If chkGST <> "" And blnGSTCOD Then
            If blnGSTCOD Then
                ViewState("isGST") = True
            Else
                ViewState("isGST") = False
            End If

            'Bindgrid()
            'hidTotal.Value = ""

            If Session("strItem") Is Nothing Then
                'Delete those temp records created in the previous session (incase user exit IE without proper log off)
                objPO1.delete_Attachment(Session.SessionID)
            End If
            cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & File1.ClientID & "');")

            Bindgrid()
            hidTotal.Value = ""

            objAdmin = Nothing
            objPR = Nothing
            objBudget = Nothing
        Else
            ViewState("postback") += 1
            If Session("CurrentScreen") = "AddItem" Then
                'Dim dtrd As DataRow
                Dim chkItem As CheckBox
                Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark As TextBox
                Dim lblItemLine, txtBudget, txtDelivery As Label
                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                Dim cboGLCode, cboCust, cboGSTRate, cboGSTTaxCode As DropDownList
                Dim strItem, strItemCust As New ArrayList()
                Dim dgItem, dgItem2 As DataGridItem
                Dim k As Integer = 0

                For Each dgItem In dtgShopping.Items
                    txtQty = dgItem.FindControl("txtQty")
                    'cboGLCode = dgItem.FindControl("cboGLCode")
                    txtPrice = dgItem.FindControl("txtPrice")
                    'txtBudget = dgItem.FindControl("txtBudget")
                    'hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                    txtDelivery = dgItem.FindControl("txtDelivery")
                    hidDelCode = dgItem.FindControl("hidDelCode")
                    txtEstDate = dgItem.FindControl("txtEstDate")
                    txtWarranty = dgItem.FindControl("txtWarranty")
                    txtRemark = dgItem.FindControl("txtRemark")
                    'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                    cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")
                    cboGSTRate = dgItem.FindControl("cboGSTRate")

                    chkItem = dgItem.FindControl("chkSelection")
                    If chkItem.Checked Then
                        If Session("CurrentScreen") = "AddItem" Then
                            'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                            'strItem.Add(New String() {"", txtQty.Text, txtPrice.Text, "", "", txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
                            strItem.Add(New String() {"", txtQty.Text, txtPrice.Text, "", "", txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGSTRate.SelectedIndex, cboGSTTaxCode.SelectedIndex})
                        End If
                    Else
                        'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                        'strItem.Add(New String() {"", txtQty.Text, txtPrice.Text, "", "", txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
                        strItem.Add(New String() {"", txtQty.Text, txtPrice.Text, "", "", txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGSTRate.SelectedIndex, cboGSTTaxCode.SelectedIndex})
                    End If

                    If (ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr") Or ViewState("modeRFQFromPR_Index") <> "" Then
                        Dim objAdmin As New Admin

                        If ViewState("modeRFQFromPR_Index") <> "" Then
                            dvwCus = objAdmin.getCustomFieldPR("", "PR", ViewState("modeRFQFromPR_Index"))
                        Else
                            dvwCus = objAdmin.getCustomField("", "PO")
                        End If

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
                    End If

                    k += 1
                Next
                Session("strItem") = strItem
                Session("strItemCust") = strItemCust

                Bindgrid()
                hidTotal.Value = ""
                Session("CurrentScreen") = ""
            End If
            'If cboDelivery.Items.Count = 0 Then
            '    Dim lstItem As New ListItem
            '    lstItem.Value = ""
            '    lstItem.Text = "---Select---"
            '    cboDelivery.Items.Insert(0, lstItem)
            '    ViewState("blnDelivery") = 0
            'Else
            '    strDefDelivery = Mid(cboDelivery.SelectedItem.Text, 1, 10)
            '    ViewState("blnDelivery") = 1
            'End If
            'Bindgrid()
        End If

        lblMsg.Text = ""
        hidTotal.Value = ""
        'addDGTotal()

        If ViewState("type") = "new" Then
            Select Case ViewState("mode")
                Case "po", "cc"
                    'displayAttachFile()
                Case "rfq"
                    cmdRemove.Visible = False
                    cmdDupPOLine.Visible = False
                    cmdAdd.Visible = False
                    'If ViewState("modePR") <> "pr" Then
                    '    displayAttachFile()
                    'End If
            End Select
        ElseIf ViewState("type") = "mod" Then
            'displayAttachFile()
            Select Case ViewState("mode")
                Case "po"
                Case "rfq"
            End Select
        End If

        If ViewState("ListingFromRFQ") = "True" Then
            cmdRemove.Visible = False
            cmdDupPOLine.Visible = False
            cmdAdd.Visible = False

            dtgShopping.Columns(EnumShoppingCart.icChk).Visible = False
            dtgShopping.Columns(EnumShoppingCart.icVendorItemCode).Visible = False
        End If

        dtgShopping.Columns(EnumShoppingCart.icMOQ).Visible = False
        dtgShopping.Columns(EnumShoppingCart.icMPQ).Visible = False

        txtInternal.Attributes.Add("onKeyDown", "limitText (this, 1000);")
        txtExternal.Attributes.Add("onKeyDown", "limitText (this, 1000);")
        'cmdRemove.Attributes.Add("onclick", "RemoveItemCheck(); return CheckAtLeastOne('chkSelection','delete');")
        cmdRemove.Attributes.Add("onclick", "return RemoveItemCheck();")
        cmdDupPOLine.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        cmdDelete.Attributes.Add("onclick", "return confirm('" & MsgForDeleteButton & "');")
        displayAttachFile()

        body1.Attributes.Add("onLoad", "refreshDatagrid(); calculateGrandTotal(); " & ViewState("body_loaditemcreated"))
        ViewState("body_loaditemcreated") = ""

        Session("keepItem") = ""
        Session("keepAr") = New ArrayList()

        Call cboBillCode_Load()
        'Dim objGlobalState As New AppGlobals
        'objGlobalState.FillState(cboState, cboCountry.SelectedItem.Value)
        'objGlobalState = Nothing

        If ViewState("modePR") = "pr" Then
            cmdRaise.Enabled = False
            cmdAdd.Visible = False
            cmdRemove.Visible = False
            cmdDelete.Visible = False
            cmdDupPOLine.Visible = False

            txtShippingHandling.ReadOnly = True
            cboVendor.Enabled = False
            cboPayTerm.Enabled = False
            cboPayMethod.Enabled = False
            cboShipmentTerm.Enabled = False
            cboShipmentMode.Enabled = False
            txtShipVia.ReadOnly = True
            txtAttention.ReadOnly = True
            cboBillCode.Enabled = False
            txtInternal.ReadOnly = True
            txtExternal.ReadOnly = True
            cmdUpload.Enabled = False
            chkUrgent.Enabled = False
        End If

    End Sub

    Private Sub GetCommonVendorAndProduct()
        'Get the common vendor list
        If dtVendorNew.Rows.Count > 0 Then
            Dim strPrefer = dtVendorNew.Rows(0).Item("Prefer")
            Dim str1st = dtVendorNew.Rows(0).Item("1st")
            Dim str2nd = dtVendorNew.Rows(0).Item("2nd")
            Dim str3rd = dtVendorNew.Rows(0).Item("3rd")

            'Check whether there's only 1 record selected
            If dtVendorNew.Rows.Count = 1 Then
                If strPrefer <> "" Then aryVendorNew.Add(strPrefer)
                If str1st <> "" Then aryVendorNew.Add(str1st)
                If str2nd <> "" Then aryVendorNew.Add(str2nd)
                If str3rd <> "" Then aryVendorNew.Add(str3rd)
            Else
                For i = 1 To dtVendorNew.Rows.Count - 1
                    'Compare the Prefer vendor of the 1st row with other rows
                    If strPrefer <> "" And
                        (strPrefer = dtVendorNew.Rows(i).Item("Prefer") Or
                         strPrefer = dtVendorNew.Rows(i).Item("1st") Or
                         strPrefer = dtVendorNew.Rows(i).Item("2nd") Or
                         strPrefer = dtVendorNew.Rows(i).Item("3rd")) Then
                        If i = dtVendorNew.Rows.Count - 1 Then 'ie check until the last row
                            aryVendorNew.Add(strPrefer)
                        End If
                    End If

                    'Compare the 1st alternative vendor of the 1st row with other rows
                    If str1st <> "" And
                        (str1st = dtVendorNew.Rows(i).Item("Prefer") Or
                         str1st = dtVendorNew.Rows(i).Item("1st") Or
                         str1st = dtVendorNew.Rows(i).Item("2nd") Or
                         str1st = dtVendorNew.Rows(i).Item("3rd")) Then
                        If i = dtVendorNew.Rows.Count - 1 Then 'ie check until the last row
                            aryVendorNew.Add(str1st)
                        End If
                    End If

                    'Compare the 2nd alternative vendor of the 1st row with other rows
                    If str2nd <> "" And
                        (str2nd = dtVendorNew.Rows(i).Item("Prefer") Or
                         str2nd = dtVendorNew.Rows(i).Item("1st") Or
                         str2nd = dtVendorNew.Rows(i).Item("2nd") Or
                         str2nd = dtVendorNew.Rows(i).Item("3rd")) Then
                        If i = dtVendorNew.Rows.Count - 1 Then 'ie check until the last row
                            aryVendorNew.Add(str2nd)
                        End If
                    End If

                    'Compare the 3rd alternative vendor of the 1st row with other rows
                    If str3rd <> "" And
                        (str3rd = dtVendorNew.Rows(i).Item("Prefer") Or
                         str3rd = dtVendorNew.Rows(i).Item("1st") Or
                         str3rd = dtVendorNew.Rows(i).Item("2nd") Or
                         str3rd = dtVendorNew.Rows(i).Item("3rd")) Then
                        If i = dtVendorNew.Rows.Count - 1 Then 'ie check until the last row
                            aryVendorNew.Add(str3rd)
                        End If
                    End If
                Next
            End If
        End If

        Session("ProdList") = aryProdCodeNew
        Session("VendorList") = aryVendorNew

    End Sub

    Private Sub BindVendorList()
        Dim aryVendor As New ArrayList
        aryVendor = Session("VendorList")

        Dim dv As DataView
        Dim cbolist As New ListItem
        Dim objPO As New PurchaseOrder_Buyer

        Dim strCC As String
        If ViewState("mode") = "cc" Then
            strCC = "cc"
        Else
            strCC = ""
        End If

        dv = objPO.getVendorList(aryVendor, strCC)
        cboVendor.Items.Clear()

        If Not dv Is Nothing Then
            cboVendor.Enabled = True
            Common.FillDdl(cboVendor, "CM_COY_NAME", "CM_COY_ID", dv)
        End If

        If dv.Count > 1 Then
            cbolist.Value = ""
            cbolist.Text = "---Select---"
            cboVendor.Items.Insert(0, cbolist)
            cboVendor.SelectedIndex = 0
            cboPayTerm.SelectedIndex = 0
            cboPayMethod.SelectedIndex = 0
        Else
            cboVendor.SelectedIndex = 1
            GetVendorDetail()
        End If
    End Sub

    Private Sub GetVendorDetail()
        ' get supplier data
        Dim dsVendor As New DataSet
        Dim strPayTerm As String
        Dim strPayMethod As String
        Dim objPR As New PR

        dsVendor = objPR.getVendorDetail(cboVendor.SelectedValue)
        If dsVendor.Tables(0).Rows.Count > 0 Then
            ViewState("strGST") = Common.parseNull(dsVendor.Tables(0).Rows(0)("CM_TAX_CALC_BY"))
            lblCurrencyCode.Text = dsVendor.Tables(0).Rows(0)("CM_CURRENCY_CODE")
            strPayTerm = CStr(Common.parseNull(dsVendor.Tables(0).Rows(0)("CV_Payment_Term")))
            strPayMethod = CStr(Common.parseNull(dsVendor.Tables(0).Rows(0)("CV_Payment_Method")))
            Common.SelDdl(strPayTerm, cboPayTerm) ', True, True)
            Common.SelDdl(strPayMethod, cboPayMethod) ', True, True)
            Session("BillingMethod") = dsVendor.Tables(0).Rows(0)("CV_BILLING_METHOD")

            If IsDBNull(Session("BillingMethod")) Then
                'If Session("Env") = "FTN" Then
                '    ViewState("BillingMethod") = "GRN"
                'Else
                '    ViewState("BillingMethod") = "FPO"
                'End If
                Session("BillingMethod") = "GRN"
            End If

        End If
    End Sub
    'Private Sub addDGTotal()
    '    Dim intSubTotal As Integer
    '    If ViewState("type") = "rfq" Then
    '        intSubTotal = EnumShoppingCart.icPrice - 1
    '    ElseIf ViewState("type") = "cart" Then
    '        intSubTotal = EnumShoppingCart.icPrice - 3
    '    Else
    '        If ViewState("listtype") = "rfq" Then
    '            intSubTotal = EnumShoppingCart.icPrice
    '        Else
    '            intSubTotal = EnumShoppingCart.icPrice - 2
    '        End If
    '    End If

    '    If ViewState("GST") = "product" Then
    '        AddRow(intSubTotal, "Subtotal", CDbl(Format(dblNoTaxTotal, "#.00")), True)
    '        AddRow(intSubTotal, "Total (w/Tax)", CDbl(Format(dblNoTaxTotal, "#.00")) + CDbl(Format(dblTotalGst, "#.00")), False)
    '        hidCnt.Value = 2
    '    Else '//subtotal
    '        dtgShopping.Columns(EnumShoppingCart.icTax).Visible = False
    '        If ViewState("intNoGSTcnt") = ViewState("intTotItem") Then '//no gst
    '            AddRow(intSubTotal, "Subtotal (Not Taxable)", CDbl(Format(dblNoTaxTotal, "#.00")), False)
    '            AddRow(intSubTotal, "Total", CDbl(Format(dblNoTaxTotal, "#.00")), False)
    '            hidCnt.Value = 2
    '        ElseIf ViewState("intGSTcnt") = ViewState("intTotItem") Then '//all gst
    '            AddRow(intSubTotal, "Subtotal (Taxable)", CDbl(Format(dblTaxTotal, "#.00")), False)
    '            AddRow(intSubTotal, "Tax", CDbl(Format(dblTotalGst, "#.00")), False)
    '            AddRow(intSubTotal, "Total", CDbl(Format(dblTaxTotal, "#.00")) + CDbl(Format(dblTotalGst, "#.00")), False)
    '            hidCnt.Value = 3
    '        Else 'mix
    '            AddRow(intSubTotal, "Subtotal (Not Taxable)", CDbl(Format(dblNoTaxTotal, "#.00")), False)
    '            AddRow(intSubTotal, "Subtotal (Taxable)", CDbl(Format(dblTaxTotal, "#.00")), False)
    '            AddRow(intSubTotal, "Tax", CDbl(Format(dblTotalGst, "#.00")), False)
    '            AddRow(intSubTotal, "Total", CDbl(Format(dblNoTaxTotal, "#.00")) + CDbl(Format(dblTaxTotal, "#.00")) + CDbl(Format(dblTotalGst, "#.00")), False)
    '            hidCnt.Value = 4
    '        End If
    '    End If
    'End Sub

    Private Sub fillAddress()
        Dim i As Integer
        Dim dvwBilling As DataView
        Dim dsBilling As New DataSet
        Dim objAdmin As New Admin

        dsBilling = objAdmin.PopulateAddr("B", cboBillCode.SelectedItem.Value, "", "")
        objAdmin = Nothing
        dvwBilling = dsBilling.Tables(0).DefaultView
        For i = 0 To dvwBilling.Count - 1
            If dvwBilling.Table.Rows(i)("AM_ADDR_CODE") = cboBillCode.SelectedItem.Value Then
                ' ai chu modify because data for conversion may be null
                txtBillAdd1.Text = Common.parseNull(dvwBilling.Table.Rows(i)("AM_ADDR_LINE1"))
                txtBillAdd2.Text = Common.parseNull(dvwBilling.Table.Rows(i)("AM_ADDR_LINE2"))
                txtBillAdd3.Text = Common.parseNull(dvwBilling.Table.Rows(i)("AM_ADDR_LINE3"))
                txtBillPostCode.Text = Common.parseNull(dvwBilling.Table.Rows(i)("AM_POSTCODE"))
                txtBillCity.Text = Common.parseNull(dvwBilling.Table.Rows(i)("AM_CITY"))
                Common.SelDdl(Common.parseNull(dvwBilling.Table.Rows(i)("AM_COUNTRY")), cboCountry, True, True)
                objGlobal.FillState(cboState, Common.parseNull(dvwBilling.Table.Rows(i)("AM_COUNTRY")))
                Common.SelDdl(Common.parseNull(dvwBilling.Table.Rows(i)("AM_STATE")), cboState, True, True)
                Exit For
            End If
        Next
    End Sub

    Private Sub fillAddressPR(ByVal strPR_NO As String)
        Dim i As Integer
        Dim dvwBilling As DataView
        Dim dsBilling As New DataSet
        Dim objAdmin As New Admin

        dsBilling = objAdmin.PopulateAddressPR(HttpContext.Current.Session("CompanyId"), cboBillCode.SelectedItem.Value, strPR_NO)

        objAdmin = Nothing
        dvwBilling = dsBilling.Tables(0).DefaultView
        For i = 0 To dvwBilling.Count - 1
            If dvwBilling.Table.Rows(i)("AM_ADDR_CODE") = cboBillCode.SelectedItem.Value Then
                ' ai chu modify because data for conversion may be null
                txtBillAdd1.Text = Common.parseNull(dvwBilling.Table.Rows(i)("AM_ADDR_LINE1"))
                txtBillAdd2.Text = Common.parseNull(dvwBilling.Table.Rows(i)("AM_ADDR_LINE2"))
                txtBillAdd3.Text = Common.parseNull(dvwBilling.Table.Rows(i)("AM_ADDR_LINE3"))
                txtBillPostCode.Text = Common.parseNull(dvwBilling.Table.Rows(i)("AM_POSTCODE"))
                txtBillCity.Text = Common.parseNull(dvwBilling.Table.Rows(i)("AM_CITY"))
                Common.SelDdl(Common.parseNull(dvwBilling.Table.Rows(i)("AM_COUNTRY")), cboCountry, True, True)
                Common.SelDdl(Common.parseNull(dvwBilling.Table.Rows(i)("AM_STATE")), cboState, True, True)
                Exit For
            End If
        Next
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1) As String
        Dim objShopping As New ShoppingCart
        Dim dsItem As New DataSet, dsTemp As New DataSet
        Dim dvViewSample As DataView
        Dim aryProdCode As New ArrayList
        Dim strProdList As String = ""
        Dim strItemHead As New ArrayList()
        'cmdAdd.Attributes.Add("onclick", "cmdAddClick(); ")

        If ViewState("type") = "new" Then
            Select Case ViewState("mode")
                Case "cc"
                    strProdList = "''"
                    aryProdCode = Session("ProdList")
                    dsItem = objShopping.getPRItemList("ConCat", strProdList, "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4))
                    For i = 0 To aryProdCode.Count - 1
                        If strProdList = "" Then
                            strProdList = "'" & aryProdCode(i)(0) & "'"
                            dsTemp = objShopping.getPRItemList("ConCat", strProdList, "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4))
                            dsItem.Tables(0).Merge(dsTemp.Tables(0))
                        Else
                            strProdList &= ", '" & aryProdCode(i)(0) & "'"
                            dsTemp = objShopping.getPRItemList("ConCat", aryProdCode(i)(0), "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4))
                            dsItem.Tables(0).Merge(dsTemp.Tables(0))
                        End If

                    Next

                    displayAttachFile()

                    ViewState("intPageRecordCnt") = dsItem.Tables(0).Rows.Count
                    dvViewSample = dsItem.Tables(0).DefaultView
                Case "po"
                    strProdList = "''"
                    dsItem = objShopping.getPOItemList("PC", strProdList, "")
                    aryProdCode = Session("ProdList")
                    For i = 0 To aryProdCode.Count - 1
                        If strProdList = "" Then
                            strProdList = "'" & aryProdCode(i) & "'"
                            dsTemp = objShopping.getPOItemList("PC", strProdList, "")
                            dsItem.Tables(0).Merge(dsTemp.Tables(0))
                        Else
                            strProdList &= ", '" & aryProdCode(i) & "'"
                            dsTemp = objShopping.getPOItemList("PC", aryProdCode(i), "")
                            dsItem.Tables(0).Merge(dsTemp.Tables(0))
                        End If
                    Next

                    displayAttachFile()

                    ViewState("intPageRecordCnt") = dsItem.Tables(0).Rows.Count
                    dvViewSample = dsItem.Tables(0).DefaultView
                Case "rfq"
                    strProdList = ""

                    dsItem = objShopping.getPOItemList("RFQ", strIndexList, ViewState("rfqid"), ViewState("Vendor"), Session("RFQItemList"))
                    If Session("CurrentScreen") = "AddItem" Or Session("CurrentScreen") = "RemoveItem" Then
                        aryProdCode = Session("ProdList")
                        For i = 0 To aryProdCode.Count - 1
                            If Not objShopping.ProductCodeAlreadyExist(ViewState("rfqid"), aryProdCode(i), "RFQ") Then
                                If strProdList = "" Then
                                    strProdList = "'" & aryProdCode(i) & "'"
                                Else
                                    strProdList &= ", '" & aryProdCode(i) & "'"
                                End If
                            End If
                        Next
                        If Not strProdList = "" Then
                            dsTemp = objShopping.getPOItemList("PC", strProdList, ViewState("poid"))
                            dsItem.Tables(0).Merge(dsTemp.Tables(0))
                        End If
                    End If

                    ViewState("intPageRecordCnt") = dsItem.Tables(0).Rows.Count
                    dvViewSample = dsItem.Tables(0).DefaultView

                    If ViewState("modePR") = "pr" Then
                        txtExternal.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_EXTERNAL_REMARK"))

                        Dim strShowColumn As String
                        strShowColumn = objDB.GetVal("SELECT PRM_PRINT_CUSTOM_FIELDS FROM PR_MSTR, PR_DETAILS WHERE PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "'")

                        'If Common.parseNull(strShowColumn) = "1" Then
                        '    chkCustom.Checked = True
                        'Else
                        '    chkCustom.Checked = False
                        'End If

                        strShowColumn = objDB.GetVal("SELECT PRM_PRINT_REMARK FROM PR_MSTR, PR_DETAILS WHERE PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "'")

                        'If Common.parseNull(strShowColumn) = "1" Then
                        '    chkRemark.Checked = True
                        'Else
                        '    chkRemark.Checked = False
                        'End If

                        Dim objWheelFile As New FileManagement
                        Dim strTermFile, strAttachIndex, strAttachRFQIndex As String
                        Dim pQuery(0), pQueryE(0) As String
                        Dim strSql, strNo, strPRNo As String

                        'strTermFile = objWheelFile.copyTermCondToPO(strNewRFQNo)

                        strPRNo = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRD_PR_NO),""'"")) AS CHAR(2000)) AS PRD_PR_NO FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "'")

                        strAttachIndex = objWheelFile.copyPRAttachToPOMulti(strPRNo, strNo, , "PRInt")
                        strAttachRFQIndex = objWheelFile.copyPRAttachToPOMulti(ViewState("rfqnum"), strNo, , "RFQ")

                        If Session("strPutOnce") = "" Then
                            If strAttachIndex <> "" Then
                                strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT_TEMP(CDA_COY_ID,CDA_DOC_NO, " _
                                & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE,CDA_STATUS) " _
                                & "SELECT CDA_COY_ID,'" & Session.SessionID & "','PO',CDA_HUB_FILENAME," _
                                & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE,'' FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " _
                                & "AND (CDA_DOC_NO IN (" & strPRNo & ") OR CDA_DOC_NO = '" & ViewState("rfqnum") & "') AND (CDA_DOC_TYPE='PR' OR CDA_DOC_TYPE='RFQ') AND (CDA_ATTACH_INDEX IN (" & strAttachIndex & ")) "
                                Common.Insert2Ary(pQuery, strSql)
                                objDB.BatchExecute(pQuery)
                                displayAttachFile()
                            End If

                            If strAttachRFQIndex <> "" Then
                                strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT_TEMP(CDA_COY_ID,CDA_DOC_NO, " _
                                & "CDA_DOC_TYPE,CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE,CDA_STATUS) " _
                                & "SELECT CDA_COY_ID,'" & Session.SessionID & "','PO',CDA_HUB_FILENAME," _
                                & "CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE,'' FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' " _
                                & "AND (CDA_DOC_NO IN (" & strPRNo & ") OR CDA_DOC_NO = '" & ViewState("rfqnum") & "') AND (CDA_DOC_TYPE='PR' OR CDA_DOC_TYPE='RFQ') AND (CDA_ATTACH_INDEX IN (" & strAttachRFQIndex & ")) "
                                Common.Insert2Ary(pQueryE, strSql)
                                objDB.BatchExecute(pQueryE)
                                displayAttachFile()
                            End If
                            Session("strPutOnce") = "Y"
                        End If

                    End If

                    If ViewState("modeRFQFromPR_Index") <> "" Then
                        Dim strPR_In_Remark As String
                        strPR_In_Remark = objDB.GetVal("SELECT CAST(GROUP_CONCAT(PRM_INTERNAL_REMARK SEPARATOR '. ') AS CHAR(2000)) AS PRM_INTERNAL_REMARK FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO IN (SELECT PRD_PR_NO FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "')")

                        txtInternal.Text = strPR_In_Remark
                    End If

                    Dim strRFQ_Curr As String
                    'strRFQ_Curr = objDB.GetVal("SELECT (SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY = 'CU' AND CODE_DELETED = 'N' AND CODE_ABBR = RM_CURRENCY_CODE) AS RM_CURRENCY_CODE FROM RFQ_MSTR WHERE rm_RFQ_NO = '" & ViewState("rfqnum") & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                    strRFQ_Curr = objDB.GetVal("SELECT RM_CURRENCY_CODE FROM RFQ_MSTR WHERE rm_RFQ_NO = '" & ViewState("rfqnum") & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                    lblCurrencyCode.Text = strRFQ_Curr
            End Select

            If Session("strItemHead") IsNot Nothing Then
                strItemHead = Session("strItemHead")
                txtAttention.Text = strItemHead(0)(0)
                txtInternal.Text = strItemHead(0)(1)
                txtExternal.Text = strItemHead(0)(2)
                cboBillCode.SelectedIndex = strItemHead(0)(3)
                fillAddress()

                cboShipmentTerm.SelectedIndex = strItemHead(0)(4)
                cboShipmentMode.SelectedIndex = strItemHead(0)(5)
                txtShipVia.Text = strItemHead(0)(6)
                chkUrgent.Checked = strItemHead(0)(7)

                txtShippingHandling.Text = strItemHead(0)(8)

                cboVendor.SelectedIndex = strItemHead(0)(9)
                cboPayTerm.SelectedIndex = strItemHead(0)(10)
                cboPayMethod.SelectedIndex = strItemHead(0)(11)
            End If

        ElseIf ViewState("type") = "mod" Then
            Select Case ViewState("mode")
                Case "cc"
                    strProdList = ""
                    dsItem = objShopping.getPOItemList("PO", "", ViewState("poid"))

                    'strProdList = "''"
                    'aryProdCode = Session("ProdList")
                    'dsItem = objShopping.getPRItemList("ConCat", "", "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4))

                    If Session("CurrentScreen") = "AddItem" Or Session("CurrentScreen") = "RemoveItem" Then
                        aryProdCode = Session("ProdList")
                        For i = dsItem.Tables(1).Rows.Count To aryProdCode.Count - 1
                            If strProdList = "" Then
                                strProdList = "'" & aryProdCode(i)(0) & "'"
                                dsTemp = objShopping.getPRItemList("ConCat", strProdList, "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4))
                                dsItem.Tables(1).Merge(dsTemp.Tables(0))
                                keepAr.Add(New String() {aryProdCode(i)(0), aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4)})
                            Else
                                strProdList &= ", '" & aryProdCode(i)(0) & "'"
                                dsTemp = objShopping.getPRItemList("ConCat", aryProdCode(i)(0), "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4))
                                dsItem.Tables(1).Merge(dsTemp.Tables(0))
                                keepAr.Add(New String() {aryProdCode(i)(0), aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4)})
                            End If
                        Next
                        Session("keepAr") = keepAr
                        If Not strProdList = "" Then
                            Session("keepItem") = strProdList
                        End If
                    End If

                Case "po", "rfq"
                    strProdList = ""
                    'Session("keepAr") = keepAr
                    dsItem = objShopping.getPOItemList("PO", "", ViewState("poid"))
                    If Session("CurrentScreen") = "AddItem" Or Session("CurrentScreen") = "RemoveItem" Then

                        aryProdCode = Session("ProdList")
                        'aryProdCode = Session("ProdListAdd")
                        For i = dsItem.Tables(1).Rows.Count To aryProdCode.Count - 1
                            'Strip PO
                            'If Not objShopping.ProductCodeAlreadyExist(ViewState("poid"), aryProdCode(i), "PO") Then
                            If strProdList = "" Then
                                strProdList = "'" & aryProdCode(i) & "'"
                                dsTemp = objShopping.getPOItemList("PC", aryProdCode(i), ViewState("poid"))
                                dsItem.Tables(1).Merge(dsTemp.Tables(0))
                                keepAr.Add(aryProdCode(i))
                            Else
                                strProdList &= ", '" & aryProdCode(i) & "'"
                                dsTemp = objShopping.getPOItemList("PC", aryProdCode(i), ViewState("poid"))
                                dsItem.Tables(1).Merge(dsTemp.Tables(0))
                                keepAr.Add(aryProdCode(i))
                            End If
                            'End If
                        Next
                        Session("keepAr") = keepAr
                        If Not strProdList = "" Then
                            '    dsTemp = objShopping.getPOItemList("PC", strProdList, ViewState("poid"))
                            '    dsItem.Tables(1).Merge(dsTemp.Tables(0))
                            Session("keepItem") = strProdList
                            '    Session("ForVendor") = strProdList
                        End If
                    End If
            End Select

            'If Session("CurrentScreen") = "VendorSelect" Then
            '    If Not Session("ForVendor") = "" Then
            '        dsTemp = objShopping.getPOItemList("PC", Session("ForVendor"), ViewState("poid"))
            '        dsItem.Tables(1).Merge(dsTemp.Tables(0))
            '    End If
            'End If

            If Not Page.IsPostBack Then
                'If keepAr.Count > 0 Then
                keepArPost = Session("keepAr")
                'End If
                'If Session("CurrentScreen") = "AddItem" Or Session("CurrentScreen") = "RemoveItem" Then
                'If Not Session("keepItem"). = "" Then
                'If Not IsNothing(keepArPost) Then
                '    '    dsTemp = objShopping.getPOItemList("PC", Session("keepItem"), ViewState("poid"))
                '    '    dsItem.Tables(1).Merge(dsTemp.Tables(0))
                '    '    Session("keepItem") = ""

                '    For i = 0 To keepArPost.Count - 1
                '        dsTemp = objShopping.getPOItemList("PC", keepArPost(i), ViewState("poid"))
                '        dsItem.Tables(1).Merge(dsTemp.Tables(0))
                '    Next
                '    Session("keepItem") = ""
                '    Session("keepAr") = New ArrayList()
                'End If

                If Not IsNothing(keepArPost) Then
                    If ViewState("type") = "new" Or ViewState("type") = "mod" Then
                        Select Case ViewState("mode")
                            Case "po", "rfq"
                                For i = 0 To keepArPost.Count - 1
                                    dsTemp = objShopping.getPOItemList("PC", keepArPost(i), ViewState("poid"))
                                    dsItem.Tables(1).Merge(dsTemp.Tables(0))
                                Next
                                Session("keepItem") = ""
                                Session("keepAr") = New ArrayList()
                            Case "cc"
                                For i = 0 To keepArPost.Count - 1
                                    dsTemp = objShopping.getPRItemList("ConCat", keepArPost(i)(0), "", "", Nothing, keepArPost(i)(1), keepArPost(i)(2), keepArPost(i)(3), keepArPost(i)(4))
                                    dsItem.Tables(1).Merge(dsTemp.Tables(0))
                                Next
                        End Select
                    End If

                    Session("keepItem") = ""
                    Session("keepAr") = New ArrayList()
                End If


                If dsItem.Tables(0).Rows.Count > 0 Then
                    ViewState("Vendor") = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_COY_ID"))
                    'lblSupplier.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_COY_NAME"))
                    ViewState("POM_S_COY_ID") = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_COY_ID"))
                    ViewState("POM_S_COY_NAME") = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_COY_NAME"))
                    ViewState("POM_RFQ_INDEX") = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_RFQ_INDEX"))
                    If IsNumeric(ViewState("POM_RFQ_INDEX")) Then
                        objGlobal.FillOneVendor(cboVendor, Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_COY_ID")))
                    Else
                        Select Case ViewState("mode")
                            Case "cc"
                                objGlobal.FillVendorViaProductCode(cboVendor, ViewState("poid"), Session("keepItem"), "cc")
                            Case "po", "rfq"
                                If ViewState("modePR") = "pr" Then
                                    objGlobal.FillVendorViaProductCode(cboVendor, ViewState("poid"), Session("keepItem"), "bc")
                                Else
                                    objGlobal.FillVendorViaProductCode(cboVendor, ViewState("poid"), Session("keepItem"))
                                End If

                        End Select
                    End If
                    hidSupplier.Value = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_COY_ID"))
                    lblDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dsItem.Tables(0).Rows(0)("POM_CREATED_DATE"))
                    'txtRequestedName.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_BUYER_NAME"))
                    'txtRequestedContact.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_BUYER_PHONE"))
                    'txtFreightTerm.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_FREIGHT_TERMS"))
                    txtShipVia.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_SHIP_VIA"))
                    txtAttention.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_ATTN"))
                    lblCurrencyCode.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_CURRENCY_CODE"))
                    ViewState("Currency") = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_CURRENCY_CODE"))
                    txtInternal.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_INTERNAL_REMARK"))
                    txtExternal.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_EXTERNAL_REMARK"))
                    Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_COY_ID")), cboVendor, True, True)
                    Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_PAYMENT_TERM")), cboPayTerm, False, True)
                    Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_SHIPMENT_TERM")), cboShipmentTerm, False, True)
                    Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_PAYMENT_METHOD")), cboPayMethod, False, True)
                    Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_SHIPMENT_MODE")), cboShipmentMode, False, True)
                    txtShippingHandling.Text = Format(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_SHIP_AMT")), "###,###,##0.00")

                    If cboVendor.SelectedValue <> "" Then
                        Dim objGST As New GST
                        chkGST = objGST.chkGST(cboVendor.SelectedValue)
                        blnGSTCOD = objGST.chkGSTCOD()
                        'If chkGST <> "" And blnGSTCOD Then
                        If blnGSTCOD Then
                            ViewState("isGST") = True
                        Else
                            ViewState("isGST") = False
                        End If
                    End If

                    If Session("strItemHead") IsNot Nothing Then
                        strItemHead = Session("strItemHead")
                        txtAttention.Text = strItemHead(0)(0)
                        txtInternal.Text = strItemHead(0)(1)
                        txtExternal.Text = strItemHead(0)(2)
                        cboBillCode.SelectedIndex = strItemHead(0)(3)
                        fillAddress()

                        cboShipmentTerm.SelectedIndex = strItemHead(0)(4)
                        cboShipmentMode.SelectedIndex = strItemHead(0)(5)
                        txtShipVia.Text = strItemHead(0)(6)
                        chkUrgent.Checked = strItemHead(0)(7)

                        txtShippingHandling.Text = strItemHead(0)(8)

                        cboVendor.SelectedIndex = strItemHead(0)(9)
                        cboPayTerm.SelectedIndex = strItemHead(0)(10)
                        cboPayMethod.SelectedIndex = strItemHead(0)(11)
                    End If

                    enableBill(False)
                    Session("keepItem") = ""
                    Session("keepAr") = New ArrayList()
                    'If Common.parseNull(dsItem.Tables(0).Rows(0)("POM_PRINT_CUSTOM_FIELDS")) = "1" Then
                    '    chkCustom.Checked = True
                    'Else
                    '    chkCustom.Checked = False
                    'End If
                    'If Common.parseNull(dsItem.Tables(0).Rows(0)("POM_PRINT_REMARK")) = "1" Then
                    '    chkRemark.Checked = True
                    'Else
                    '    chkRemark.Checked = False
                    'End If

                    If Common.parseNull(dsItem.Tables(0).Rows(0)("POM_URGENT")) = "1" Then
                        chkUrgent.Checked = True
                    Else
                        chkUrgent.Checked = False
                    End If

                    'ViewState("strGST") = dsItem.Tables(0).Rows(0)("POM_GST")
                    'If ViewState("strGST") = "0" Then
                    '    ViewState("GST") = "product"
                    'Else
                    '    ViewState("GST") = "subtotal"
                    'End If

                    ' check company allow free form
                    If ViewState("blnAllowFreeForm") = False And Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_ADDR_CODE")) = "F" Then
                        Dim objAdmin As New Admin
                        Common.SelDdl(objAdmin.user_Default_Add("B"), cboBillCode, True, True)
                        fillAddress()
                    Else
                        Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_ADDR_CODE")), cboBillCode, True, True)
                        txtBillAdd1.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_ADDR_LINE1"))
                        txtBillAdd2.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_ADDR_LINE2"))
                        txtBillAdd3.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_ADDR_LINE3"))
                        txtBillPostCode.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_POSTCODE"))
                        txtBillCity.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_CITY"))
                        Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_STATE")), cboState, True, True)
                        Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_COUNTRY")), cboCountry, True, True)
                        enableBill(True)
                    End If

                    ' Yap_As Michelle agreed to display the code only
                    If ViewState("modePR") = "pr" Then
                        cboBillCode.SelectedItem.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_B_ADDR_CODE"))
                    End If

                    If IsDBNull(dsItem.Tables(0).Rows(0)("POM_RFQ_INDEX")) Then ' created from SHOPPING_CART
                        ViewState("listtype") = "cart"
                    Else ' created from RFQ
                        If dsItem.Tables(0).Rows(0)("POM_RFQ_INDEX") = 0 Then ' created from SHOPPING_CART
                            ViewState("listtype") = "cart"
                        Else
                            ViewState("listtype") = "rfq"
                        End If
                    End If

                    If ViewState("listtype") = "cart" Then
                        cmdAdd.Visible = True
                        cmdRemove.Visible = True
                        cmdDupPOLine.Visible = True
                        'cmdAdd.Attributes.Add("onclick", "return cmdAddClick();")
                        ViewState("blnCmdAdd") = True
                        ViewState("blnCmdRemove") = True
                    Else
                        ''cmdAdd.Visible = False
                        ''cmdRemove.Visible = False
                        'cmdDupPOLine.Visible = False
                        'ViewState("blnCmdAdd") = False
                        'ViewState("blnCmdRemove") = False
                        'cmdAdd.Attributes.Add("onclick", "return cmdAddClick();")
                        'dtgShopping.Columns(EnumShoppingCart.icRfqQty).Visible = True
                        'dtgShopping.Columns(EnumShoppingCart.icTolerance).Visible = True
                        'dtgShopping.Columns(EnumShoppingCart.icRfqQty).HeaderText = "RFQ Qty"
                        'dtgShopping.Columns(EnumShoppingCart.icQty).HeaderText = "PO Qty"
                    End If
                End If
            End If
            ViewState("intPageRecordCnt") = dsItem.Tables(1).Rows.Count
            dvViewSample = dsItem.Tables(1).DefaultView
            dvwCustomItem = dsItem.Tables(2).DefaultView

            Select Case ViewState("mode")
                Case "po"
                Case "rfq"
            End Select
        End If

        intPageRecordCnt = ViewState("intPageRecordCnt")

        intRow = 0

        dtgShopping.DataSource = dvViewSample
        dtgShopping.DataBind()
        objShopping = Nothing
        'If Session("Env") = "FTN" Then
        '    Me.dtgShopping.Columns(6).Visible = False
        '    Me.dtgShopping.Columns(7).Visible = False
        '    Me.dtgShopping.Columns(5).Visible = False
        '    Me.dtgShopping.Columns(22).Visible = False
        'Else
        '    Me.dtgShopping.Columns(6).Visible = True
        '    Me.dtgShopping.Columns(7).Visible = True
        '    Me.dtgShopping.Columns(5).Visible = True
        '    Me.dtgShopping.Columns(22).Visible = True
        'End If
        Me.dtgShopping.Columns(6).Visible = False
        Me.dtgShopping.Columns(7).Visible = False
        Me.dtgShopping.Columns(5).Visible = False
        Me.dtgShopping.Columns(24).Visible = False

        If ViewState("isGST") = True Then
            'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
            Me.dtgShopping.Columns(EnumShoppingCart.icGstTaxCode).Visible = True
            Me.dtgShopping.Columns(EnumShoppingCart.icGSTRate).Visible = True
            Me.lblTax.Text = "GST Amount"
        Else
            'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
            Me.dtgShopping.Columns(EnumShoppingCart.icGstTaxCode).Visible = False
            Me.dtgShopping.Columns(EnumShoppingCart.icGSTRate).Visible = False
        End If

    End Function

    Private Function encodeCustomField(ByVal strValue As String) As String
        strValue = Server.UrlEncode(strValue)
        encodeCustomField = strValue
    End Function

    Private Function decodeCustomField(ByVal strValue As String) As String
        strValue = Server.UrlDecode(strValue)
        decodeCustomField = strValue
    End Function

    Private Sub dtgShopping_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgShopping.ItemDataBound
        Dim objPO As New PurchaseOrder
        Dim objPO1 As New PurchaseOrder_Buyer

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objGlobal As New AppGlobals
            Dim i As Integer
            Dim lstItem As New ListItem
            Dim GLCodelstItem As New ListItem
            Dim CategoryCodelstItem As New ListItem
            Dim dblAmt, dblGstAmt As Decimal
            Dim objAdmin As New Admin

            Dim strItem As New ArrayList()
            Dim strItemCust As New ArrayList()

            Dim chk As CheckBox, sClientId As String, sTotalClient As String
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            'simply use one of the client id to get all client ID
            sTotalClient = hidClientId.Value
            sClientId = Mid(chk.ClientID, InStr(chk.ClientID, "_") + 1, InStr(Mid(chk.ClientID, InStr(chk.ClientID, "_") + 1), "_") - 1) & "|"
            If Not sTotalClient.Contains(sClientId) Then
                hidClientId.Value = hidClientId.Value & sClientId
                hidTotalClientId.Value = hidTotalClientId.Value + 1
            End If

            Dim txtDelivery As Label
            txtDelivery = e.Item.FindControl("txtDelivery")
            txtDelivery.Text = strDefDelivery
            txtDelivery.Width = System.Web.UI.WebControls.Unit.Pixel(50)

            Dim hidDelCode As TextBox
            'Dim cboDelivery As DropDownList
            hidDelCode = e.Item.FindControl("hidDelCode")
            'cboDelivery = e.Item.FindControl("cboDelivery")
            hidDelCode.Text = ""


            Dim lblProductCode As Label
            lblProductCode = e.Item.FindControl("lblProductCode")
            lblProductCode.Text = Common.parseNull(dv("PRODUCTCODE"))

            Dim lblProductGrp As Label
            lblProductGrp = e.Item.FindControl("lblProductGrp")
            lblProductGrp.Text = Common.parseNull(dv("CDM_GROUP_INDEX"))


            Dim lblProductDesc As Label
            lblProductDesc = e.Item.FindControl("lblProductDesc")
            lblProductDesc.Text = Common.parseNull(dv("PRODUCTDESC"))
            lblProductDesc.Width = System.Web.UI.WebControls.Unit.Pixel(150)

            e.Item.Cells(EnumShoppingCart.icNo).Text = intRow + 1
            Dim str1 As String
            Dim strSCoyId As String = ""
            Try
                If ViewState("type") = "new" Then
                    Select Case ViewState("mode")
                        Case "po", "cc"
                            If Not IsDBNull(e.Item.DataItem("Supplierid")) Then strSCoyId = e.Item.DataItem("Supplierid").ToString
                        Case "rfq"
                            If Not IsDBNull(e.Item.DataItem("PRD_S_COY_ID")) Then strSCoyId = e.Item.DataItem("PRD_S_COY_ID").ToString
                    End Select
                ElseIf ViewState("type") = "mod" Then
                    If Not IsDBNull(e.Item.DataItem("PRD_S_COY_ID")) Then strSCoyId = e.Item.DataItem("PRD_S_COY_ID").ToString
                    Select Case ViewState("mode")
                        Case "po"
                        Case "rfq"
                    End Select
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
            'Michelle (14/5/2011) - To limit the qty to 5 digits, consistence with the rest
            'revQty.ValidationExpression = "(?!^0*$)^\d{1,10}?$" '"\d[1,9]{1,5}" '"^\d+$" ' "^\d+\.+\d+$|^\d+$" '(?!^0*$)
            'revQty.ValidationExpression = "(?!^0*$)^\d{1,5}?$"
            revQty.ValidationExpression = "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$"
            revQty.ControlToValidate = "txtQty"
            'revQty.ErrorMessage = "Invalid quantity"
            revQty.ErrorMessage = ViewState("ValQtyMsg") '"Invalid quantity. Range should be from 0.01 to 999999.99"
            revQty.Text = "?"
            revQty.Display = ValidatorDisplay.Dynamic

            'Dim revETD As RegularExpressionValidator
            'revETD = e.Item.FindControl("revETD")
            'revETD.ValidationExpression = "(?!^0*$)^\d{1,5}?$" '"\d[1,9]{1,5}" '"^\d+$" ' "^\d+\.+\d+$|^\d+$" '(?!^0*$)
            'revETD.ControlToValidate = "txtEstDate"
            'revETD.ErrorMessage = CStr(intRow + 1) & ". Invalid Est. Date of Delivery (days)"
            'revETD.Text = "?"
            'revETD.Display = ValidatorDisplay.Dynamic

            Dim revRange As RangeValidator
            revRange = e.Item.FindControl("revRange")
            If ViewState("type") = "new" Then
                Select Case ViewState("mode")
                    Case "po", "cc"
                        revRange.Enabled = False
                    Case "rfq"
                End Select
            ElseIf ViewState("type") = "mod" Then
                Select Case ViewState("mode")
                    Case "po"
                        revRange.Enabled = False
                    Case "rfq"
                End Select
            End If

            Dim revPriceRange As RangeValidator
            revPriceRange = e.Item.FindControl("revPriceRange")
            Dim QuoPrice As Double
            If IsDBNull(dv("UNITCOST")) Then
                QuoPrice = Format(0, "###,###,##0.0000")
            Else
                QuoPrice = Format(dv("UNITCOST"), "###,###,##0.0000")
            End If

            Dim txtPrice As TextBox
            Dim revPrice As RegularExpressionValidator
            txtPrice = e.Item.FindControl("txtPrice")
            revPrice = e.Item.FindControl("revPrice")
            'revPrice.ValidationExpression = "(?!^0*$)(?!^0*\.0*$)^\d{1,12}(\.\d{1,4})?$" '"\d[1,9]{1,5}" '"^\d+$" ' "^\d+\.+\d+$|^\d+$" '(?!^0*$)
            'revPrice.ControlToValidate = "txtPrice"
            'revPrice.ErrorMessage = "Invalid Unit Price"
            'revPrice.Text = "?"
            'revPrice.Display = ValidatorDisplay.Dynamic
            If IsDBNull(dv("UNITCOST")) Then
                'txtPrice.Text = Format(0, "###,###,##0.0000")
                txtPrice.Text = ""
            Else
                txtPrice.Text = Format(dv("UNITCOST"), "###,###,##0.0000")
            End If
            If ViewState("mode") = "cc" Then
                'txtPrice.ReadOnly = True
                txtPrice.Enabled = False
            End If

            ''Jules 2014.07.14 GST Enhancement
            Dim cboGSTRate, cboGSTTaxCode As DropDownList
            Dim objGST As New GST
            Dim lstItem2 = New ListItem
            cboGSTRate = e.Item.FindControl("cboGSTRate")
            cboGSTTaxCode = e.Item.FindControl("cboGSTTaxCode")
            objGlobal.FillGST(cboGSTRate)
            'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
            objGlobal.FillTaxCode(cboGSTTaxCode, , "P")

            If ViewState("isGST") And (ViewState("mode") = "rfq" Or ViewState("mode") = "po" Or ViewState("mode") = "cc") Then
                If dv("GSTRATE") = "N/A" Or objGST.chkGST(cboVendor.SelectedValue) = "" Then
                    lstItem = New ListItem
                    'cboGSTRate.Items.Clear()
                    lstItem.Value = "N/A"
                    lstItem.Text = "N/A"
                    cboGSTRate.Items.Insert(0, lstItem)
                ElseIf dv("GSTRATE") = "" Then
                    Common.SelDdl("STD", cboGSTRate)
                Else
                    Common.SelDdl(dv("GSTRATE"), cboGSTRate)
                End If

                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                If dv("GSTRATE") = "N/A" Or objGST.chkGST(cboVendor.SelectedValue) = "" Then
                    lstItem2.Value = "NR"
                    lstItem2.Text = "NR"
                    cboGSTTaxCode.Items.Clear()
                    cboGSTTaxCode.Items.Insert(0, lstItem2)
                Else
                    If Common.parseNull(dv("GstTaxCode")) = "" Then
                        Common.SelDdl("", cboGSTTaxCode) 'Default to 'Select' option
                    Else
                        Common.SelDdl(dv("GstTaxCode"), cboGSTTaxCode)
                    End If
                End If
            Else
                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                lstItem2.Value = "NR"
                lstItem2.Text = "NR"
                cboGSTTaxCode.Items.Clear()
                cboGSTTaxCode.Items.Insert(0, lstItem2)
                cboGSTTaxCode.Enabled = False
            End If

            If ViewState("mode") = "cc" Or objGST.chkGST(cboVendor.SelectedValue) = "" Then
                cboGSTRate.Enabled = False
                cboGSTTaxCode.Enabled = False
            End If
            ''end.

            Dim lblNoTax As Label
            lblNoTax = e.Item.FindControl("lblNoTax")

            Dim hidTaxPerc As TextBox
            Dim hidTaxID As TextBox

            Dim strPerc, strTaxID As String

            hidTaxPerc = e.Item.FindControl("hidTaxPerc")
            hidTaxID = e.Item.FindControl("hidTaxID")

            If ViewState("type") = "new" Then
                Select Case ViewState("mode")
                    Case "cc"
                        Dim strGSTID As String
                        If ViewState("isGST") <> True Then
                            strPerc = Common.parseNull(dv("GST"))
                        Else
                            objGST.getGSTInfobyRate(cboGSTRate.SelectedValue, strPerc, strGSTID)
                        End If
                        If IsNumeric(strPerc) Then
                            hidTaxPerc.Text = strPerc
                            hidTaxID.Text = strTaxID
                        Else
                            hidTaxPerc.Text = 0
                            strPerc = 0
                        End If
                    Case "po"
                        If cboVendor.SelectedIndex <> 0 Or Not (Mid(cboVendor.SelectedValue, 1, 2) = "--") Then
                            'strPerc = objPO1.get_TaxPerc(e.Item.Cells(EnumShoppingCart.icProductCode).Text, cboVendor.SelectedItem.Value, strTaxID)
                            objGST.getGSTInfobyRate(cboGSTRate.SelectedValue, strPerc, strTaxID)
                            If IsNumeric(strPerc) Then
                                hidTaxPerc.Text = strPerc
                                hidTaxID.Text = strTaxID
                            Else
                                hidTaxPerc.Text = 0
                                strPerc = 0
                            End If
                            hidTaxPerc.Text = strPerc
                        Else
                            hidTaxPerc.Text = 0
                            hidTaxID.Text = 0
                        End If
                    Case "rfq"
                        If ViewState("isGST") = True Then
                            Dim strGSTID As String
                            objGST.getGSTInfobyRate(dv("GSTRATE"), strPerc, strGSTID)
                        Else
                            strPerc = dv("GST")
                        End If
                        If IsNumeric(strPerc) Then
                            hidTaxPerc.Text = strPerc
                            hidTaxID.Text = strTaxID
                        Else
                            hidTaxPerc.Text = 0
                            strPerc = 0
                        End If
                        hidTaxPerc.Text = strPerc
                        strTaxID = 0
                End Select
            ElseIf ViewState("type") = "mod" Then
                Select Case ViewState("mode")
                    Case "cc"
                        hidTaxPerc.Text = Common.parseNull(dv("GST"))
                    Case "po", "rfq"
                        ''if previously is from RFQ, then have to counter check with RFQ, not Vendor
                        If ViewState("ListingFromRFQ") = "True" Then
                            strPerc = dv("GST")
                            If IsNumeric(strPerc) Then
                                hidTaxPerc.Text = strPerc
                                hidTaxID.Text = strTaxID
                            Else
                                hidTaxPerc.Text = 0
                                strPerc = 0
                            End If
                            hidTaxPerc.Text = strPerc
                            strTaxID = 0
                        Else
                            If cboVendor.SelectedIndex <> 0 Or Not (Mid(cboVendor.SelectedValue, 1, 2) = "--") Then
                                'strPerc = objPO1.get_TaxPerc(e.Item.Cells(EnumShoppingCart.icProductCode).Text, cboVendor.SelectedItem.Value, strTaxID)
                                objGST.getGSTInfobyRate(cboGSTRate.SelectedValue, strPerc, strTaxID)
                                If IsNumeric(strPerc) Then
                                    hidTaxPerc.Text = strPerc
                                    hidTaxID.Text = strTaxID
                                Else
                                    hidTaxPerc.Text = 0
                                    strPerc = 0
                                End If
                                hidTaxPerc.Text = strPerc
                            Else
                                hidTaxPerc.Text = 0
                                hidTaxID.Text = 0
                            End If
                        End If
                        If ViewState("modePR") = "pr" Then
                            hidTaxPerc.Text = Common.parseNull(dv("GST"))
                        End If

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
            dblAmt = CDec(txtAmount.Text)
            If ViewState("GST") = "subtotal" Then
                'If IsDBNull(dv("GST")) Then
                '    dblTaxTotal = dblTaxTotal + dblAmt
                '    ViewState("intGSTcnt") += 1
                '    ViewState("intTotItem") += 1
                '    dblGstAmt = dblAmt * (0 / 100)
                '    txtGSTAmt = e.Item.FindControl("txtGSTAmt")
                '    txtGSTAmt.Text = Format(dblGstAmt, "###,##0.00")
                'ElseIf dv("GST") = 0 Then
                '    dblTaxTotal = dblTaxTotal + dblAmt
                '    ViewState("intGSTcnt") += 1
                '    ViewState("intTotItem") += 1
                '    dblGstAmt = dblAmt * (0 / 100)
                '    txtGSTAmt = e.Item.FindControl("txtGSTAmt")
                '    txtGSTAmt.Text = Format(dblGstAmt, "###,##0.00")
                'Else
                '    dblTaxTotal = dblTaxTotal + dblAmt
                '    ViewState("intGSTcnt") += 1
                '    ViewState("intTotItem") += 1
                '    dblGstAmt = dblAmt * (dv("GST") / 100)
                '    txtGSTAmt = e.Item.FindControl("txtGSTAmt")
                '    txtGSTAmt.Text = Format(dblGstAmt, "###,##0.00")
                'End If

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
            hidGSTAmt.Value = dblGstAmt


            txtQty.Attributes.Add("onfocus", "return focusControl('" &
                                IIf(ViewState("GST") = "product", 0, 1) & "', '" &
                                txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" &
                                txtAmount.ClientID & "', '" & txtGSTAmt.ClientID & "', '" & cboGSTRate.ClientID & "', '" &
                                hidTaxPerc.ClientID & "', '" & Common.parseNull(dv("GST")) & "');")
            txtQty.Attributes.Add("onblur", "return calculateTotal('" &
                                IIf(ViewState("GST") = "product", 0, 1) & "', '" &
                                txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" &
                                txtAmount.ClientID & "', '" & txtGSTAmt.ClientID & "', '" & cboGSTRate.ClientID & "', '" &
                                hidTaxPerc.ClientID & "', '" & Common.parseNull(dv("GST")) & "','1');")

            txtPrice.Attributes.Add("onfocus", "return clearQuestionMark(); return focusControl('" &
                               IIf(ViewState("GST") = "product", 0, 1) & "', '" &
                               txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" &
                               txtAmount.ClientID & "', '" & txtGSTAmt.ClientID & "', '" & cboGSTRate.ClientID & "', '" &
                               hidTaxPerc.ClientID & "', '" & Common.parseNull(dv("GST")) & "');")
            txtPrice.Attributes.Add("onblur", "return calculateTotal('" &
                                IIf(ViewState("GST") = "product", 0, 1) & "', '" &
                                txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" &
                                txtAmount.ClientID & "', '" & txtGSTAmt.ClientID & "', '" & cboGSTRate.ClientID & "', '" &
                                hidTaxPerc.ClientID & "', '" & Common.parseNull(dv("GST")) & "','1');")

            cboGSTRate.Attributes.Add("onchange", "return calculateTotal('" &
                                  IIf(ViewState("GST") = "product", 0, 1) & "', '" &
                                  txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" &
                                  txtAmount.ClientID & "', '" & txtGSTAmt.ClientID & "', '" & cboGSTRate.ClientID & "', '" &
                                  hidTaxPerc.ClientID & "', '" & Common.parseNull(dv("GST")) & "','1');")

            '2015-06-22: CH: Rounding issue (Prod issue)
            'dblTotalGst = dblTotalGst + dblGstAmt
            dblTotalGst = dblTotalGst + CDec(Format(dblGstAmt, "###0.00"))

            Dim lblItemLine As Label
            lblItemLine = e.Item.FindControl("lblItemLine")
            lblItemLine.Text = dv("ITEMLINE")

            'If Session("Env") <> "FTN" Then
            '    If ViewState("BCM") > 0 Then
            '        Dim cboBudget As DropDownList
            '        cboBudget = e.Item.FindControl("cboBudget")
            '        Dim objBudget As New BudgetControl
            '        Common.FillDdl(cboBudget, "Acct_list", "Acct_Index", dtBCM)
            '        If Not IsDBNull(dv("ACCT")) Then
            '            Common.SelDdl(dv("ACCT"), cboBudget, True, True)
            '        End If

            '        Dim cmdBudget As HtmlInputButton
            '        cmdBudget = e.Item.FindControl("cmdBudget")
            '        cmdBudget.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("PR", "Budget.aspx", "pageid=" & strPageId & "&id=" & cboBudget.ClientID) & "', '', 'scrollbars=yes,resizable=yes,width=400,height=400,status=no,menubar=no')")
            '    Else
            '        e.Item.Cells(EnumShoppingCart.icBudget).Visible = False
            '    End If
            'Else
            '    e.Item.Cells(EnumShoppingCart.icBudget).Visible = False
            'End If
            e.Item.Cells(EnumShoppingCart.icBudget).Visible = False

            'If ViewState("type") = "cart" Then
            '    hidDelCode = e.Item.FindControl("hidDelCode")
            '    If hidDelCode.Text = "" Then
            '        hidDelCode.Text = cboDelivery.SelectedValue
            '    End If
            'End If

            'If Session("Env") <> "FTN" Then
            '    'Michelle (eBiz/303) 
            '    Dim cboGLCode, cboCategoryCode As DropDownList
            '    Dim dsGLCode As New DataSet
            '    Dim dsCategoryCode As New DataSet


            '    'Adding item into GL Code combo box
            '    cboGLCode = e.Item.FindControl("cboGLCode")
            '    dsGLCode = objAdmin.PopulateGLCode(Common.parseNull(dv("PRODUCTCODE")), Common.parseNull(dv("ITEMCODE")))
            '    dvwGLCode = dsGLCode.Tables(0).DefaultView

            '    Common.FillDdl(cboGLCode, "DESCRIPTION", "GL Code", dvwGLCode)
            '    If Not IsDBNull(dv("GLCode")) And Common.parseNull(dv("GLCode")) <> "" Then
            '        Common.SelDdl(dv("GLCode"), cboGLCode, True, True)
            '    Else
            '        If cboGLCode.Items.Count = 0 Then
            '            GLCodelstItem.Value = ""
            '            GLCodelstItem.Text = "Not Applicable"
            '            cboGLCode.Items.Insert(0, GLCodelstItem)
            '            cboGLCode.SelectedIndex = 0
            '        End If
            '    End If

            '    '====================================================
            '    '16 Oct 2009
            '    'Yik Foong
            '    'Added a button to the GL code column when the dropdown list contain more 
            '    'than 1 item.
            '    'When the button is clicked, open the GL code search page
            '    'If there are more than 1 items in the drop downlist, show the search button, 
            '    'Else hide the button
            '    Dim glSearch As HtmlInputButton = e.Item.FindControl("btnGLSearch")
            '    If Not glSearch Is Nothing And cboGLCode.Items.Count <= 1 Then
            '        glSearch.Visible = False
            '    Else
            '        glSearch.Visible = True
            '    End If

            '    If glSearch.Visible Then
            '        'open search page

            '        Dim glCbo As DropDownList = e.Item.FindControl("cboGLCode")
            '        If Not glCbo Is Nothing Then
            '            Dim url As String = dDispatcher.direct("PR", "SearchGLCode.aspx", "id=" & glCbo.ClientID)
            '            glSearch.Attributes.Add("onclick", "window.open('" & url & "', '', 'scrollbars=yes,resizable=yes,width=400,height=400,status=no,menubar=no')")
            '        End If

            '    End If
            '    '====================================================


            '    'Adding item into Category combo box
            '    cboCategoryCode = e.Item.FindControl("cboCategoryCode")
            '    dsCategoryCode = objAdmin.PopulateCategoryCode(Common.parseNull(dv("PRODUCTCODE")), Common.parseNull(dv("ITEMCODE")))
            '    dvwCategoryCode = dsCategoryCode.Tables(0).DefaultView
            '    Common.FillDdl(cboCategoryCode, "Category Code", "Category Code", dvwCategoryCode)

            '    If Not IsDBNull(dv("CategoryCode")) And Common.parseNull(dv("CategoryCode")) <> "" Then
            '        Common.SelDdl(dv("CategoryCode"), cboCategoryCode, True, True)
            '    Else
            '        If cboCategoryCode.Items.Count = 0 Then
            '            CategoryCodelstItem.Value = ""
            '            CategoryCodelstItem.Text = "Not Applicable"
            '            cboCategoryCode.Items.Insert(0, CategoryCodelstItem)
            '            cboCategoryCode.SelectedIndex = 0
            '        End If
            '    End If

            '    '====================================================
            '    '16 Oct 2009
            '    'Added a button to the category column when the dropdown list contain more 
            '    'than 1 item.
            '    'When the button is clicked, open the category search page
            '    'If there are more than 1 items in the drop downlist, show the search button, 
            '    'Else hide the button
            '    Dim catSearch As HtmlInputButton = e.Item.FindControl("btnCatSearch")
            '    If Not catSearch Is Nothing Then
            '        If cboCategoryCode.Items.Count <= 1 Then
            '            catSearch.Visible = False
            '        Else
            '            catSearch.Visible = True
            '        End If
            '    End If

            '    If catSearch.Visible Then
            '        'open search category code page
            '        Dim url As String = dDispatcher.direct("PR", "SearchCategoryCode.aspx", "id=" & cboCategoryCode.ClientID)
            '        catSearch.Attributes.Add("onclick", "window.open('" & url & "', '', 'scrollbars=yes,resizable=yes,width=400,height=400,status=no,menubar=no')")
            '    End If
            '    '====================================================
            'End If

            If ViewState("type") = "new" Then
                hidDelCode.Text = ViewState("strDeliveryDefault")
                txtDelivery.Text = Mid(objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), ViewState("strDeliveryDefault"), "D"), 1, 10)

                'If ViewState("modePR") = "pr" Then
                '    cboBillCode.SelectedItem.Value = ViewState("strBillDefault")
                '    cboBillCode.SelectedItem.Text = ViewState("strBillDefault")
                '    enableBill(True)
                '    fillAddressPR()
                '    enableBill(False)
                'End If

                Select Case ViewState("mode")
                    Case "po"
                    Case "rfq"
                End Select
            ElseIf ViewState("type") = "mod" Then
                'Common.SelDdl(Common.Parse(dv("DADDRCODE")), cboDelivery, True, True)
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
                    Case "po"
                    Case "rfq"
                End Select
            End If

            'Dim hidcboDelivery As DropDownList
            'hidcboDelivery = e.Item.FindControl("hidcboDelivery")
            'Common.FillDdl(hidcboDelivery, "FullAddress", "AM_ADDR_CODE", dvwDelivery)
            'If hidcboDelivery.Items.Count = 0 Then
            '    lstItem.Value = ""
            '    lstItem.Text = "---Select---"
            '    hidcboDelivery.Items.Insert(0, lstItem)
            '    ViewState("blnDelivery") = 0
            'Else
            '    ViewState("blnDelivery") = 1
            'End If

            Dim cmdDelivery As HtmlInputButton
            cmdDelivery = e.Item.FindControl("cmdDelivery")
            cmdDelivery.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("Admin", "AddressMaster.aspx", "pageid=" & strPageId & "&mod=P&type2=RPO&type=D&id=" & hidDelCode.ClientID & "&txtDelivery=" & txtDelivery.ClientID) & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no')")

            'cmdDelivery.Attributes.Add("onclick", "selectAll();")

            ' custom field
            Dim cboCustom As DropDownList
            Dim j As Integer
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
                            Case "po"
                            Case "rfq"
                        End Select
                    ElseIf ViewState("type") = "mod" Then
                        For j = 0 To dvwCustomItem.Table.Rows.Count - 1
                            If dvwCustomItem.Table.Rows(j)("PCD_PR_LINE") = lblItemLine.Text And dvwCustomItem.Table.Rows(j)("PCD_FIELD_NO") = dtgShopping.Columns(i).SortExpression Then
                                Common.SelDdl(encodeCustomField(dvwCustomItem.Table.Rows(j)("PCD_FIELD_VALUE")), cboCustom, True, True)
                                blnSetDefault = True
                                Exit For
                            End If
                        Next
                        Select Case ViewState("mode")
                            Case "po"
                            Case "rfq"
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

            txtEstDate = e.Item.FindControl("txtEstDate")
            txtWarranty = e.Item.FindControl("txtWarranty")
            'Michelle (2/8/2010) - To include the defaul EDD
            Dim objDB As New EAD.DBCom
            Dim txtDate1 As Date = CType(lblDate.Text, Date)
            Dim intEDD As Integer = Val(objDB.GetVal("SELECT CV_EDD FROM COMPANY_VENDOR WHERE CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CV_S_COY_ID = '" & strSCoyId & "'"))
            If intEDD > 0 Then
                txtEstDate.Text = txtDate1.AddDays(intEDD)
                txtEstDate.ReadOnly() = True
                txtEstDate.ForeColor = Color.Gray
                txtEstDate.Enabled = False
            Else
                txtEstDate.Text = txtDate1.AddDays(intEDD)
                txtEstDate.ReadOnly() = False
            End If

            'Dim revETD As RegularExpressionValidator
            'revETD = e.Item.FindControl("revETD")
            'revETD.ValidationExpression = "(?!^0*$)^\d{1,3}?$"
            'revETD.ControlToValidate = "txtEstDate"
            'revETD.ErrorMessage = "Invalid Est. Date of Delivery (days)"
            'revETD.Display = ValidatorDisplay.Dynamic
            'revETD.Text = "?"

            Dim revWarranty As RegularExpressionValidator
            revWarranty = e.Item.FindControl("revWarranty")
            revWarranty.ValidationExpression = "\d{1,5}"
            revWarranty.ControlToValidate = "txtWarranty"
            revWarranty.ErrorMessage = "Invalid Warranty Terms (mths)"
            revWarranty.Display = ValidatorDisplay.Dynamic
            revWarranty.Text = "?"

            If ViewState("type") = "new" Then
                Select Case ViewState("mode")
                    Case "po", "cc"
                        Dim iEDD As Integer
                        iEDD = objPO1.get_EDDPerc(e.Item.Cells(EnumShoppingCart.icProductCode).Text, cboVendor.SelectedItem.Value)
                        If iEDD = 0 Then
                            iEDD = 1
                        End If
                        If txtEstDate.ReadOnly = False Then txtEstDate.Text = txtDate1.AddDays(iEDD)
                        txtWarranty.Text = "0"
                    Case "rfq"
                        aryProdCodeNew.Add(Common.parseNull(dv("PRODUCTCODE")))

                        If txtEstDate.Enabled = True Then txtEstDate.Text = txtDate1.AddDays(Common.parseNull(dv("ETD")))
                        txtWarranty.Text = Common.parseNull(dv("WARRANTYTERMS"))
                        'txtWarranty.CssClass = "lblnumerictxtbox"
                        dtgShopping.Columns(EnumShoppingCart.icWarranty).ItemStyle.HorizontalAlign = HorizontalAlign.Right
                End Select
            ElseIf ViewState("type") = "mod" Then
                Dim dtr As DataRow
                Dim tem As Integer
                If IsDBNull(dv("ETD")) Then
                    tem = 1
                Else
                    tem = dv("ETD")
                End If
                If ViewState("listtype") = "rfq" Then
                    If txtEstDate.ReadOnly = False Then txtEstDate.Text = txtDate1.AddDays(tem)
                    txtWarranty.Text = Common.parseNull(dv("WARRANTYTERMS"))
                    'txtWarranty.CssClass = "lblnumerictxtbox"
                    dtgShopping.Columns(EnumShoppingCart.icWarranty).ItemStyle.HorizontalAlign = HorizontalAlign.Right
                Else
                    If txtEstDate.ReadOnly = False Then txtEstDate.Text = txtDate1.AddDays(tem)
                    txtWarranty.Text = Common.parseNull(dv("WARRANTYTERMS"))
                End If

                'Because Type=List will load data from database, not array, so we need to store array into session for add item

                'keep the vendors of the selected record into a dataset
                dtr = dtVendorNew.NewRow()
                dtr("Prefer") = ""
                dtr("1st") = ""
                dtr("2nd") = ""
                dtr("3rd") = ""
                If IsNumeric(ViewState("POM_RFQ_INDEX")) Then
                    objPO1.getVendorViaPO(ViewState("poid"), dtr("Prefer"))
                Else
                    objPO1.getVendorViaProductCode(Common.parseNull(dv("PRODUCTCODE")), dtr("Prefer"), dtr("1st"), dtr("2nd"), dtr("3rd"))
                End If

                dtVendorNew.Rows.Add(dtr)

                Select Case ViewState("mode")
                    Case "cc"
                        aryProdCodeNew.Add(New String() {Common.parseNull(dv("PRODUCTCODE")), Common.parseNull(dv("PRODUCTDESC")), Common.parseNull(dv("UOM")), Common.parseNull(dv("COMMODITY")), ""})
                    Case "po", "rfq"
                        'keep the Product Code of the selected record into an array
                        aryProdCodeNew.Add(Common.parseNull(dv("PRODUCTCODE")))
                End Select
            End If

            'If txtEstDate.Text = "0" Then txtEstDate.Text = ""

            Dim txtRemark As TextBox
            txtRemark = e.Item.FindControl("txtRemark")
            txtRemark.Text = Common.parseNull(dv("REMARK"))
            txtRemark.Attributes.Add("onKeyDown", "limitText (this, 400);")

            intRow = intRow + 1

            If ViewState("modeRFQFromPR_Index") <> "" Then
                Dim strPR_Index As String = objDB.GetVal("SELECT IFNULL(RD_PR_LINE_INDEX, '') AS RD_PR_LINE_INDEX FROM RFQ_MSTR, RFQ_DETAIL WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_RFQ_NO = '" & ViewState("rfqnum") & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND RD_RFQ_LINE = '" & Common.parseNull(dv("ITEMINDEX")) & "'")
                Dim strPR_Remark As String = objDB.GetVal("SELECT IFNULL(PRD_REMARK, '') AS PRD_REMARK FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")

                txtRemark.Text = strPR_Remark

                Dim strPR_Del As String = objDB.GetVal("SELECT PRD_D_ADDR_CODE FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")

                hidDelCode.Text = strPR_Del
                txtDelivery.Text = Mid(objAdmin.GetSpecificAddrPR(HttpContext.Current.Session("CompanyId"), strPR_Del, "D", strPR_Index), 1, 10)

                'PRM_B_ADDR_CODE, PRM_B_ADDR_LINE1, PRM_B_ADDR_LINE2, PRM_B_LINE_ADDR_LINE3, 
                'PRM_B_POSTCODE, PRM_B_STATE, PRM_B_CITY, PRM_B_COUNTRY

                Dim strPR_Bill As String = objDB.GetVal("SELECT PRM_B_ADDR_CODE FROM PR_MSTR, PR_DETAILS WHERE PRD_COY_ID = PRM_COY_ID AND PRD_PR_NO = PRM_PR_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")
                cboBillCode.SelectedItem.Value = strPR_Bill
                cboBillCode.SelectedItem.Text = strPR_Bill
                enableBill(True)
                fillAddressPR(objDB.GetVal("SELECT PRM_PR_NO FROM PR_MSTR, PR_DETAILS WHERE PRD_COY_ID = PRM_COY_ID AND PRD_PR_NO = PRM_PR_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'"))
                enableBill(False)
            End If

            If Session("strItem") IsNot Nothing Then
                strItem = Session("strItem")
                If (CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1) <= strItem.Count - 1 Then
                    'cboGLCode.SelectedIndex = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(0)
                    txtQty.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(1)
                    txtPrice.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(2)
                    'txtBudget.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(3)
                    'hidBudgetCode.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(4)
                    'If Not dtBCM Is Nothing Then
                    '    Dim drTemp As DataRow()
                    '    drTemp = dtBCM.Select("Acct_Index=" & hidBudgetCode.Text)
                    '    If drTemp.Length > 0 Then
                    '        txtBudget.Text = Mid(drTemp(0)("Acct_List"), 1, 10)
                    '    End If
                    'End If
                    txtDelivery.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(5)
                    hidDelCode.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(6)
                    txtDelivery.Text = Mid(objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), Common.Parse(strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(6)), "D"), 1, 10)
                    txtEstDate.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(7)
                    txtWarranty.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(8)
                    txtRemark.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(9)
                    cboGSTRate.SelectedIndex = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(10)
                    cboGSTTaxCode.SelectedIndex = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(11)
                End If
            End If

            If ViewState("modePR") = "pr" Then
                e.Item.Cells(EnumShoppingCart.icGLCode).Enabled = False
                e.Item.Cells(EnumShoppingCart.icGLCode1).Enabled = False
                e.Item.Cells(EnumShoppingCart.icCategoryCode1).Enabled = False
                e.Item.Cells(EnumShoppingCart.icTaxCode).Enabled = False
                e.Item.Cells(EnumShoppingCart.icRfqQty).Enabled = False
                e.Item.Cells(EnumShoppingCart.icTolerance).Enabled = False
                e.Item.Cells(EnumShoppingCart.icQty).Enabled = False
                e.Item.Cells(EnumShoppingCart.icUOM).Enabled = False
                e.Item.Cells(EnumShoppingCart.icPrice).Enabled = False
                e.Item.Cells(EnumShoppingCart.icTotal).Enabled = False
                e.Item.Cells(EnumShoppingCart.icTax).Enabled = False
                e.Item.Cells(EnumShoppingCart.icSource).Enabled = False
                e.Item.Cells(EnumShoppingCart.icCDGroup).Enabled = False
                e.Item.Cells(EnumShoppingCart.icBudget).Enabled = False
                e.Item.Cells(EnumShoppingCart.icDelivery).Enabled = False
                cmdDelivery = e.Item.FindControl("cmdDelivery")
                cmdDelivery.Disabled = True
                e.Item.Cells(EnumShoppingCart.icEstDate).Enabled = False
                e.Item.Cells(EnumShoppingCart.icWarranty).Enabled = False
                e.Item.Cells(EnumShoppingCart.icMOQ).Enabled = False
                e.Item.Cells(EnumShoppingCart.icMPQ).Enabled = False
                e.Item.Cells(EnumShoppingCart.icRemark).Enabled = False
            End If

        ElseIf e.Item.ItemType = ListItemType.Header Then
            If ViewState("BCM") <= 0 Then
                e.Item.Cells(EnumShoppingCart.icBudget).Visible = False
            End If
            If ViewState("modePR") = "pr" Then
                e.Item.Cells(EnumShoppingCart.icDelivery).Enabled = False
            End If
        End If

        dtgShopping.Columns(EnumShoppingCart.icMOQ).Visible = False
        dtgShopping.Columns(EnumShoppingCart.icMPQ).Visible = False

        objGlobal = Nothing
    End Sub

    Private Function rebindDDL(ByRef pddl As DropDownList)
        Dim i As Integer
        For i = 0 To pddl.Items.Count - 1
            pddl.Items(i).Value = Server.UrlEncode(pddl.Items(i).Value)
        Next
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

            If ViewState("isGST") Then
                e.Item.Cells(EnumShoppingCart.icTax).Text = "GST Amount"
            Else
                e.Item.Cells(EnumShoppingCart.icTax).Text = "Tax"
            End If

            If ViewState("type") = "new" Or ViewState("type") = "mod" Then
                Select Case ViewState("mode")
                    Case "cc"
                        e.Item.Cells(EnumShoppingCart.icPrice).Text = "Contract Price"
                    Case "po"
                    Case "rfq"
                End Select
            End If

            Dim i As Integer
            Dim str As String
            For i = EnumShoppingCart.icBudget To dtgShopping.Columns.Count - 2
                Dim cell As TableCell
                Dim lbl As New Label
                cell = e.Item.Cells(i)
                If i = EnumShoppingCart.icBudget Then
                    lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("PR", "Budget.aspx", "pageid=" & strPageId) & "&id=cboBudget&mode=all', '', 'scrollbars=yes,resizable=yes,width=400,height=400,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                ElseIf i = EnumShoppingCart.icDelivery Then
                    lbl.Text = "Delivery Address"
                    lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("Admin", "AddressMaster.aspx", "pageid=" & strPageId) & "&mod=P&type2=RPO&type=D&id=cboDelivery&mode=all', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                ElseIf i = EnumShoppingCart.icEstDate Then
                    'lbl.Text = "<A href='javascript:;' onclick=""window.open('AddWarrantyTerms.aspx?pageid=" & strPageId & "&type=E&id=txtEstDate', '', 'resizable=no,width=400,height=200,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                    lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendarForAll.aspx", "id=txtEstDate") & "','cal','width=180,height=155,left=270,top=180')"">" & e.Item.Cells(i).Text & "</A>"
                    'lbl.Text = e.Item.Cells(i).Text
                ElseIf i = EnumShoppingCart.icWarranty Then
                    If ViewState("type") <> "rfq" And ViewState("listtype") <> "rfq" Then
                        lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("PR", "AddWarrantyTerms.aspx", "pageid=" & strPageId) & "&type=W&id=txtWarranty', '', 'resizable=no,width=400,height=200,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                    Else
                        lbl.Text = e.Item.Cells(i).Text
                    End If
                ElseIf i > EnumShoppingCart.icWarranty Then
                    str = dDispatcher.direct("Admin", "CustomFieldValue.aspx", "pageid=" & strPageId & "&mod=P&value=" & dtgShopping.Columns(i).SortExpression & "&name=" & Server.UrlEncode(dtgShopping.Columns(i).HeaderText) & "&id=" & dtgShopping.Columns(i).SortExpression)
                    lbl.Text = "<A href='javascript:;' onclick=""window.open(&quot;" & str & "&quot;, &quot;&quot;, &quot;scrollbars=yes,resizable=yes,width=400,height=400,status=no,menubar=no&quot;);"">" & e.Item.Cells(i).Text & "</a>"
                End If
                cell.Controls.Add(lbl)
            Next
        End If

        'If ViewState("type") = "cart" Or ViewState("type") = "rfq" Then
        '    e.Item.Cells(EnumShoppingCart.icChk).Visible = False
        'End If
        '//this line must be included
        dtgShopping.AllowSorting = False

        txtShippingHandling.Attributes.Add("onfocus", "return calculateGrandTotal();")
        txtShippingHandling.Attributes.Add("onblur", "return calculateGrandTotal();")
        ''''calculateTotal('0', 'dtgShopping_" & dtgShopping.ClientID & "_txtQty', 'dtgShopping_" & dtgShopping.ClientID & "_txtPrice', 'dtgShopping_" & dtgShopping.ClientID & "_txtAmount', 'dtgShopping_" & dtgShopping.ClientID & "_txtGSTAmt', 'dtgShopping_" & dtgShopping.ClientID & "_hidtaxperc', '0','1');

        'CType(dgItem.FindControl("lblProductCode"), Label).ClientI()

        Grid_ItemCreated(dtgShopping, e)
        'Dim chk As CheckBox
        'chk = e.Item.Cells(0).FindControl("chkSelection")
        ViewState("body_loaditemcreated") = "calculateAllIndividualTotal(); "
        ''body1.Attributes.Add("onLoad", "calculateTotal('0', 'dtgShopping_" & dtgShopping.ClientID & "_txtQty', 'dtgShopping_" & dtgShopping.ClientID & "_txtPrice', 'dtgShopping_" & dtgShopping.ClientID & "_txtAmount', 'dtgShopping_" & dtgShopping.ClientID & "_txtGSTAmt', 'dtgShopping_" & dtgShopping.ClientID & "_hidtaxperc', '0','1'); ")
        ''dtgShopping.ClientID

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

    Private Function bindPO() As DataSet
        Dim ds As New DataSet
        Dim i As Integer

        ' columns for PR_MSTR table
        Dim dtMaster As New DataTable
        dtMaster.Columns.Add("PONo", Type.GetType("System.String"))
        'dtMaster.Columns.Add("ReqName", Type.GetType("System.String"))
        'dtMaster.Columns.Add("ReqPhone", Type.GetType("System.String"))
        dtMaster.Columns.Add("VendorID", Type.GetType("System.String"))
        dtMaster.Columns.Add("Attn", Type.GetType("System.String"))
        dtMaster.Columns.Add("FreightTerms", Type.GetType("System.String"))
        dtMaster.Columns.Add("PaymentType", Type.GetType("System.String"))
        dtMaster.Columns.Add("ShipmentTerm", Type.GetType("System.String"))
        dtMaster.Columns.Add("ShipmentMode", Type.GetType("System.String"))
        dtMaster.Columns.Add("CurrencyCode", Type.GetType("System.String"))
        dtMaster.Columns.Add("ExchangeRate", Type.GetType("System.Double"))
        dtMaster.Columns.Add("PaymentTerm", Type.GetType("System.String"))
        dtMaster.Columns.Add("ShipVia", Type.GetType("System.String"))
        dtMaster.Columns.Add("InternalRemark", Type.GetType("System.String"))
        dtMaster.Columns.Add("ExternalRemark", Type.GetType("System.String"))
        dtMaster.Columns.Add("Urgent", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrCode", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrLine1", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrLine2", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrLine3", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrPostCode", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrState", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrCity", Type.GetType("System.String"))
        dtMaster.Columns.Add("BillAddrCountry", Type.GetType("System.String"))
        dtMaster.Columns.Add("PrintCustom", Type.GetType("System.String"))
        dtMaster.Columns.Add("PrintRemark", Type.GetType("System.String"))
        dtMaster.Columns.Add("ShipAmt", Type.GetType("System.String"))
        'dtMaster.Columns.Add("GST", Type.GetType("System.String"))
        dtMaster.Columns.Add("POCost", Type.GetType("System.Double"))
        dtMaster.Columns.Add("RfqIndex", Type.GetType("System.String"))
        dtMaster.Columns.Add("QuoNo", Type.GetType("System.String"))
        dtMaster.Columns.Add("POM_EXCHANGE_RATE", Type.GetType("System.Double"))
        dtMaster.Columns.Add("BillingMethod", Type.GetType("System.String"))
        
        Dim dtr As DataRow
        dtr = dtMaster.NewRow()
        dtr("PONo") = ViewState("poid")
        'dtr("ReqName") = txtRequestedName.Text
        'dtr("ReqPhone") = txtRequestedContact.Text
        dtr("VendorID") = cboVendor.SelectedItem.Value
        dtr("Attn") = txtAttention.Text
        'dtr("FreightTerms") = txtFreightTerm.Text
        dtr("PaymentType") = cboPayMethod.SelectedItem.Text
        dtr("ShipmentTerm") = cboShipmentTerm.SelectedItem.Text
        dtr("ShipmentMode") = cboShipmentMode.SelectedItem.Text
        dtr("CurrencyCode") = lblCurrencyCode.Text
        dtr("PaymentTerm") = cboPayTerm.SelectedItem.Text
        dtr("ShipVia") = txtShipVia.Text
        dtr("InternalRemark") = txtInternal.Text
        dtr("ExternalRemark") = txtExternal.Text
        dtr("BillAddrCode") = cboBillCode.SelectedItem.Value
        dtr("BillAddrLine1") = txtBillAdd1.Text
        dtr("BillAddrLine2") = txtBillAdd2.Text
        dtr("BillAddrLine3") = txtBillAdd3.Text
        dtr("BillAddrPostCode") = txtBillPostCode.Text
        dtr("BillAddrState") = cboState.SelectedItem.Value
        dtr("BillAddrCity") = txtBillCity.Text
        dtr("BillAddrCountry") = cboCountry.SelectedItem.Value
        'dtr("PrintCustom") = IIf(chkCustom.Checked, "1", "0")
        dtr("Urgent") = IIf(chkUrgent.Checked, "1", "0")
        dtr("ExchangeRate") = 1
        dtr("BillingMethod") = Session("BillingMethod")
        'dtr("GST") = IIf(ViewState("GST") = "product", "0", "1")
        'dtr("PrintRemark") = IIf(chkRemark.Checked, "1", "0")
        'Michelle (14/5/2011) - To remove the comma
        'dtr("ShipAmt") = txtShippingHandling.Text
        dtr("ShipAmt") = Replace(txtShippingHandling.Text, ",", "")
        If hidCost.Value = "" Then
            hidCost.Value = "0"
        End If
        'ViewState("POCost") = CDbl(Format(CDbl(hidCost.Value), "#.00"))
        'dtr("POCost") = CDbl(ViewState("POCost"))

        If ViewState("type") = "new" Then
            Select Case ViewState("mode")
                Case "po", "cc"
                    dtr("RfqIndex") = "NULL"
                    dtr("QuoNo") = ""
                Case "rfq"
                    dtr("RfqIndex") = ViewState("rfqid")
                    dtr("QuoNo") = ViewState("quono")
            End Select
        ElseIf ViewState("type") = "mod" Then
            dtr("RfqIndex") = "NULL"
            dtr("QuoNo") = ""
            Select Case ViewState("mode")
                Case "po"
                Case "rfq"
            End Select
        End If

        'dtMaster.Rows.Add(dtr)
        'ds.Tables.Add(dtMaster)

        ' columns for PR_DETAILS
        Dim dtDetails As New DataTable
        dtDetails.Columns.Add("Line", Type.GetType("System.Int32"))
        dtDetails.Columns.Add("ProductCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("VendorItemCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("TaxCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("ProductDesc", Type.GetType("System.String"))
        dtDetails.Columns.Add("UOM", Type.GetType("System.String"))
        dtDetails.Columns.Add("Qty", Type.GetType("System.Double"))
        dtDetails.Columns.Add("UnitCost", Type.GetType("System.Double"))
        dtDetails.Columns.Add("ETD", Type.GetType("System.Int32"))
        dtDetails.Columns.Add("Remark", Type.GetType("System.String"))
        dtDetails.Columns.Add("GST", Type.GetType("System.String"))
        dtDetails.Columns.Add("DeliveryAddr", Type.GetType("System.String"))
        dtDetails.Columns.Add("AcctIndex", Type.GetType("System.String"))
        dtDetails.Columns.Add("ProductType", Type.GetType("System.String"))
        dtDetails.Columns.Add("Source", Type.GetType("System.String"))
        dtDetails.Columns.Add("TAXID", Type.GetType("System.String"))
        dtDetails.Columns.Add("MOQ", Type.GetType("System.String"))
        dtDetails.Columns.Add("MPQ", Type.GetType("System.String"))
        dtDetails.Columns.Add("CDGroup", Type.GetType("System.String"))
        dtDetails.Columns.Add("POD_RFQ_ITEM_LINE", Type.GetType("System.String"))

        ' _Yap: For Interface
        dtDetails.Columns.Add("ItemCode", Type.GetType("System.String"))

        dtDetails.Columns.Add("SelectedGST", Type.GetType("System.String"))
        dtDetails.Columns.Add("GSTTaxAmount", Type.GetType("System.String"))
        'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
        dtDetails.Columns.Add("GSTTaxCode", Type.GetType("System.String"))

        'If Session("Env") <> "FTN" Then
        '    dtDetails.Columns.Add("WarrantyTerms", Type.GetType("System.Int32"))
        '    dtDetails.Columns.Add("CDGroup", Type.GetType("System.String"))
        '    'dtDetails.Columns.Add("ItemCode", Type.GetType("System.String"))
        '    dtDetails.Columns.Add("CategoryCode", Type.GetType("System.String"))
        '    dtDetails.Columns.Add("GLCode", Type.GetType("System.String"))
        'End If
        dtDetails.Columns.Add("RfqQty", Type.GetType("System.Int32"))
        dtDetails.Columns.Add("QtyTolerance", Type.GetType("System.Int32"))
        dtDetails.Columns.Add("SupplierCompanyId", Type.GetType("System.String"))

        Dim dtrd As DataRow
        Dim dgItem As DataGridItem
        For Each dgItem In dtgShopping.Items

            blnItem = True
            dtrd = dtDetails.NewRow()
            dtrd("Line") = dgItem.Cells(EnumShoppingCart.icNo).Text

            Dim lblItemLine As Label
            lblItemLine = dgItem.FindControl("lblItemLine")

            If ViewState("type") = "new" Then
                Select Case ViewState("mode")
                    Case "po", "cc"
                    Case "rfq"
                        dtrd("POD_RFQ_ITEM_LINE") = lblItemLine.Text
                End Select
            ElseIf ViewState("type") = "mod" Then
                Select Case ViewState("mode")
                    Case "po"
                    Case "rfq"
                        dtrd("POD_RFQ_ITEM_LINE") = lblItemLine.Text
                End Select
            End If

            If ViewState("ListingFromRFQ") = "True" Then
                dtrd("POD_RFQ_ITEM_LINE") = lblItemLine.Text
            End If

            dtrd("ProductCode") = dgItem.Cells(EnumShoppingCart.icProductCode).Text
            dtrd("VendorItemCode") = IIf(dgItem.Cells(EnumShoppingCart.icVendorItemCode).Text = "", "", dgItem.Cells(EnumShoppingCart.icVendorItemCode).Text)
            'dtr("TaxCode") = IIf(dgItem.Cells(EnumShoppingCart.icTaxCode).Text = "&nbsp;", "", dgItem.Cells(EnumShoppingCart.icTaxCode).Text)
            dtrd("ProductDesc") = CType(dgItem.FindControl("lblProductDesc"), Label).Text 'dgItem.Cells(EnumShoppingCart.icItemDesc).Text
            dtrd("UOM") = dgItem.Cells(EnumShoppingCart.icUOM).Text

            dtrd("MOQ") = dgItem.Cells(EnumShoppingCart.icMOQ).Text
            dtrd("MPQ") = dgItem.Cells(EnumShoppingCart.icMPQ).Text

            ' _Yap: For Interface
            dtrd("ItemCode") = IIf(dgItem.Cells(EnumShoppingCart.icVendorItemCode).Text = "", "", dgItem.Cells(EnumShoppingCart.icVendorItemCode).Text)

            Dim txtQty As TextBox ' HtmlInputText
            txtQty = dgItem.FindControl("txtQty")
            'If IsNumeric(txtQty.Text) And Regex.IsMatch(Trim(txtQty.Text), "(?!^0*$)^\d{1,10}?$") Then
            If IsNumeric(txtQty.Text) And Regex.IsMatch(Trim(txtQty.Text), "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$") Then
                dtrd("Qty") = txtQty.Text
            Else
                dtrd("Qty") = 1.0
                blnValid = False
            End If

            Dim txtPrice As TextBox
            Dim uPrice As Decimal
            txtPrice = dgItem.FindControl("txtPrice")

            uPrice = CDec(txtPrice.Text)
            If IsNumeric(uPrice) And Regex.IsMatch(Trim(uPrice), "(?!^0*$)(?!^0*\.0*$)^\d{1,12}(\.\d{1,4})?$") Then
                dtrd("UnitCost") = txtPrice.Text
            Else
                dtrd("UnitCost") = 0
                'blnValid = False
            End If



            Dim hidTaxPerc As TextBox
            Dim cboGSTRate, cboGSTTaxCode As DropDownList
            Dim txtAmount, txtGST As TextBox, txtGSTAmt As TextBox
            'hidTaxID = dgItem.FindControl("hidTaxID")
            hidTaxPerc = dgItem.FindControl("hidTaxPerc")
            cboGSTRate = dgItem.FindControl("cboGSTRate")
            txtAmount = dgItem.FindControl("txtAmount")
            'dtrd("GST") = CDbl((dtrd("Qty") * dtrd("UnitCost")) * hidTaxPerc.Text)

            '2015-06-22: CH: Rounding issue (Prod issue)
            'txtAmount.Text = CDbl(dtrd("Qty") * dtrd("UnitCost"))
            txtAmount.Text = CDec(Format(dtrd("Qty") * dtrd("UnitCost"), "###0.00"))
            txtGST = dgItem.FindControl("txtGST")
            txtGSTAmt = dgItem.FindControl("txtGSTAmt")

            Dim strPercI, strTaxIDI As String
            Dim objGSTI As New GST
            objGSTI.getGSTInfobyRate(cboGSTRate.SelectedValue, strPercI, strTaxIDI)

            'If txtGST.Text = "" Then
            If txtGSTAmt.Text = "" Then
                dtrd("GST") = 0
            Else
                'dtrd("GST") = CDbl(txtGST.Text)
                'dtrd("GST") = CDbl(hidTaxPerc.Text)
                dtrd("GST") = CDbl(IIf(strPercI = "", 0, strPercI))
            End If

            If ViewState("isGST") Then
                dtrd("SelectedGST") = CType(dgItem.FindControl("cboGSTRate"), DropDownList).SelectedItem.Value
            Else
                dtrd("SelectedGST") = ""
            End If

            'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
            cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")
            dtrd("GSTTaxCode") = cboGSTTaxCode.SelectedValue

            'dtrd("GSTTaxAmount") = CDbl(hidTaxPerc.Text * txtAmount.Text / 100) 'CType(txtGSTAmt.Text, Double)
            'dtrd("GSTTaxAmount") = CDbl(Format(CDbl((hidTaxPerc.Text * txtAmount.Text / 100)), "#0.00"))
            '2015-06-22: CH: Rounding issue (Prod issue)
            'dtrd("GSTTaxAmount") = CDbl(Format(CDbl((dtrd("GST") * txtAmount.Text / 100)), "#0.00"))
            dtrd("GSTTaxAmount") = CDec(Format(CDbl((dtrd("GST") * txtAmount.Text / 100)), "#0.00"))
            'If hidTaxID.Text = "" Then
            '    dtrd("TAXID") = 0
            'Else
            '    dtrd("TAXID") = hidTaxID.Text 'dgItem.Cells(EnumShoppingCart.icTax).Text
            'End If
            'dtrd("GST") = hidTaxPerc.Text

            'ViewState("POCost") = ViewState("POCost") + CDbl(txtGST.Text * txtAmount.Text / 100) + (dtrd("Qty") * dtrd("UnitCost"))
            'ViewState("POCost") = ViewState("POCost") + CDbl(hidTaxPerc.Text * txtAmount.Text / 100) + (dtrd("Qty") * dtrd("UnitCost"))
            '2015-06-22: CH: Rounding issue (Prod issue)
            'ViewState("POCost") = ViewState("POCost") + CDbl(dtrd("GST") * txtAmount.Text / 100) + (dtrd("Qty") * dtrd("UnitCost"))
            ViewState("POCost") = ViewState("POCost") + CDbl(Format(dtrd("GST") * CDec(txtAmount.Text) / 100, "###0.00")) + CDec(txtAmount.Text)
            ' yAP: Temporary not to deploy
            'ViewState("POCost") = ViewState("POCost") + CDbl(Format(CDbl((hidTaxPerc.Text * txtAmount.Text / 100)), "#0.00")) + CDbl(Format(CDbl(txtAmount.Text), "#0.00"))

            Dim txtEstDate As TextBox
            txtEstDate = dgItem.FindControl("txtEstDate")
            'If IsNumeric(txtEstDate.Text) And Regex.IsMatch(Trim(txtEstDate.Text), "(?!^0*$)^\d{1,3}?$") Then
            '    dtrd("ETD") = CInt(txtEstDate.Text)
            'Else
            '    dtrd("ETD") = 0
            '    blnValid = False
            'End If

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


            Dim txtRemark As TextBox
            txtRemark = dgItem.FindControl("txtRemark")
            dtrd("Remark") = txtRemark.Text

            Dim hidDelCode As TextBox
            hidDelCode = dgItem.FindControl("hidDelCode")
            dtrd("DeliveryAddr") = hidDelCode.Text

            dtrd("ProductType") = ""

            Select Case ViewState("mode")
                Case "cc"
                    dtrd("Source") = "CC"
                    dtrd("CDGroup") = CType(dgItem.FindControl("lblProductGrp"), Label).Text
                Case "po"
                    dtrd("Source") = "PC"
                Case "rfq"
                    dtrd("Source") = ""
            End Select

            'If ViewState("type") = "new" Then
            '    Select Case ViewState("mode")
            '        Case "cc"
            '            dtrd("Source") = "CC"
            '            dtrd("CDGroup") = CType(dgItem.FindControl("lblProductGrp"), Label).Text
            '        Case "po"
            '            dtrd("Source") = "PC"
            '        Case "rfq"
            '            dtrd("Source") = ""
            '    End Select
            'ElseIf ViewState("type") = "mod" Then
            '    dtrd("Source") = ""
            '    Select Case ViewState("mode")
            '        Case "cc"
            '            dtrd("CDGroup") = CType(dgItem.FindControl("lblProductGrp"), Label).Text
            '        Case "po"
            '        Case "rfq"
            '    End Select
            'End If

            'If Session("Env") <> "FTN" Then

            '    dtrd("CategoryCode") = IIf(dgItem.Cells(EnumShoppingCart.icCategoryCode).Text = "&nbsp;", "", dgItem.Cells(EnumShoppingCart.icCategoryCode).Text)
            '    dtrd("GLCode") = IIf(dgItem.Cells(EnumShoppingCart.icGLCode).Text = "&nbsp;", "", dgItem.Cells(EnumShoppingCart.icGLCode).Text)

            '    Dim txtWarranty As TextBox
            '    txtWarranty = dgItem.FindControl("txtWarranty")
            '    If IsNumeric(txtWarranty.Text) Then
            '        dtrd("WarrantyTerms") = CInt(txtWarranty.Text)
            '    Else
            '        dtrd("WarrantyTerms") = 0
            '    End If

            '    Dim cboGLCode, cboCategoryCode As DropDownList
            '    cboGLCode = dgItem.FindControl("cboGLCode")
            '    If cboGLCode.Items.Count > 0 Then
            '        dtrd("GLCode") = cboGLCode.SelectedItem.Value
            '    Else
            '        dtrd("GLCode") = ""
            '    End If

            '    cboCategoryCode = dgItem.FindControl("cboCategoryCode")
            '    If cboCategoryCode.Items.Count > 0 Then
            '        dtrd("CategoryCode") = cboCategoryCode.SelectedItem.Value
            '    Else
            '        dtrd("CategoryCode") = ""
            '    End If

            '    If ViewState("BCM") > 0 Then
            '        Dim cboBudget As DropDownList
            '        cboBudget = dgItem.FindControl("cboBudget")
            '        If cboBudget.Items.Count > 0 Then
            '            dtrd("AcctIndex") = cboBudget.SelectedItem.Value
            '        Else
            '            dtrd("AcctIndex") = ""
            '        End If
            '    Else
            '        dtrd("AcctIndex") = ""
            '    End If

            '    If dgItem.Cells(EnumShoppingCart.icSource).Text = "&nbsp;" Then
            '        dtrd("Source") = ""
            '    Else
            '        dtrd("Source") = dgItem.Cells(EnumShoppingCart.icSource).Text
            '    End If

            '    If dgItem.Cells(EnumShoppingCart.icCDGroup).Text = "&nbsp;" Then
            '        dtrd("CDGroup") = ""
            '    Else
            '        dtrd("CDGroup") = dgItem.Cells(EnumShoppingCart.icCDGroup).Text
            '    End If

            'End If

            'If ViewState("type") = "rfq" Or ViewState("listtype") = "rfq" Then
            '    dtrd("RfqQty") = CInt(dgItem.Cells(EnumShoppingCart.icRfqQty).Text)
            '    dtrd("QtyTolerance") = CInt(dgItem.Cells(EnumShoppingCart.icTolerance).Text)
            'Else
            '    dtrd("RfqQty") = 0
            '    dtrd("QtyTolerance") = 0
            'End If
            '----New Code Added by  praveen To get Records in Multi_vendordetails.aspx and POVIEWB3.aspx when PR Raised  '
            '---from RFQ(when viewstate("type") = "rfq") and to rectify the error in RaisePR when click on Pr no  Suppilier id on 21.08.2007

            'dtr("SupplierCompanyId") = dgItem.Attributes("SuppId").ToString
            If ViewState("type") = "new" Then
                Select Case ViewState("mode")
                    Case "po", "cc"
                        dtrd("SupplierCompanyId") = dgItem.Attributes("SuppId").ToString
                    Case "rfq"
                        dtrd("SupplierCompanyId") = ViewState("Vendor")
                End Select
            ElseIf ViewState("type") = "mod" Then
                dtrd("SupplierCompanyId") = dgItem.Attributes("SuppId").ToString
                Select Case ViewState("mode")
                    Case "po"
                    Case "rfq"
                End Select
            End If

            '---End The Code 

            dtDetails.Rows.Add(dtrd)
        Next
        dtr("POCost") = CDbl(ViewState("POCost") + dtr("ShipAmt"))
        dtMaster.Rows.Add(dtr)
        ds.Tables.Add(dtMaster)
        ds.Tables.Add(dtDetails)
        ViewState("POCost") = 0

        '' columns for PR_CUSTOM_FIELD_MSTR
        'Dim dtCustomMaster As New DataTable
        'Dim objAdmin As New Admin
        'dvwCus = objAdmin.getCustomField("")
        'dtCustomMaster.Columns.Add("FieldNo", Type.GetType("System.Int32"))
        'dtCustomMaster.Columns.Add("FieldName", Type.GetType("System.String"))
        'If Not dvwCus Is Nothing Then
        '    For i = 0 To dvwCus.Count - 1
        '        dtr = dtCustomMaster.NewRow
        '        dtr("FieldNo") = dvwCus.Table.Rows(i)("CF_FIELD_NO")
        '        dtr("FieldName") = dvwCus.Table.Rows(i)("CF_FIELD_NAME")
        '        dtCustomMaster.Rows.Add(dtr)
        '    Next
        'End If
        'objAdmin = Nothing
        'ds.Tables.Add(dtCustomMaster)

        '' columns for PR_CUSTOM_FIELD_DETAILS
        'Dim dtCustomDetails As New DataTable
        'dtCustomDetails.Columns.Add("Line", Type.GetType("System.Int32"))
        'dtCustomDetails.Columns.Add("FieldNo", Type.GetType("System.Int32"))
        'dtCustomDetails.Columns.Add("FieldValue", Type.GetType("System.String"))
        'If Not dvwCus Is Nothing Then
        '    For i = 0 To dvwCus.Count - 1
        '        For Each dgItem In dtgShopping.Items
        '            dtr = dtCustomDetails.NewRow
        '            dtr("Line") = dgItem.Cells(EnumShoppingCart.icNo).Text
        '            dtr("FieldNo") = dvwCus.Table.Rows(i)("CF_FIELD_NO")

        '            Dim cboCustom As DropDownList
        '            cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)
        '            dtr("FieldValue") = decodeCustomField(cboCustom.SelectedItem.Value)
        '            dtCustomDetails.Rows.Add(dtr)
        '        Next
        '    Next
        'End If
        'ds.Tables.Add(dtCustomDetails)

        bindPO = ds
    End Function

    'Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click

    '    If lblPONo.Text = "To Be Allocated By System" Then
    '        Me.cmdRaise_Click(cmdRaise, New System.EventArgs())
    '        Dim AA As String
    '        AA = "DD"
    '    Else
    '        Dim objPO As New PurchaseOrder
    '        'intMsg = objPO.submitPO(lblPONo.Text, PRStatus.Submitted, ViewState("msg"))

    '    End If
    'End Sub

    Private Sub SavePO()
        Dim dsPO As New DataSet
        Dim objPO As New PurchaseOrder
        Dim objPO1 As New PurchaseOrder_Buyer
        Dim strMsg As String = ""
        blnValid = True
        blnItem = False

        If validateDatagrid(strMsg) Then
            dsPO = bindPO()            

            If ViewState("type") = "new" Then
                Dim strNewPO As String
                Dim intMsg As Integer

                If blnValid Then
                    intMsg = objPO1.insertPO(dsPO, strNewPO, False, False)
                    objPO = Nothing

                    Select Case intMsg
                        Case WheelMsgNum.Save
                            'Response.Redirect("../PR/PRConfirm.aspx?pageid=" & strPageId & "&type=S&prid=" & strNewPO)
                            'Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
                            If Session("urlreferer") = "" Then
                                Common.NetMsgbox(Me, "Purchase Order Number " & strNewPO & " has been created.", dDispatcher.direct("PR", "viewShoppingCart.aspx", "type=tab&pageid=" & strPageId))
                            Else
                                If Session("urlreferer") = "RFQComSummary" Then
                                    Common.NetMsgbox(Me, "Purchase Order Number " & strNewPO & " has been created.", dDispatcher.direct("RFQ", "RFQ_List.aspx", "type=tab&pageid=" & strPageId))
                                ElseIf Session("urlreferer") = "ConCatSearch" Then
                                    Common.NetMsgbox(Me, "Purchase Order Number " & strNewPO & " has been created.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                                Else
                                    Common.NetMsgbox(Me, "Purchase Order Number " & strNewPO & " has been created.", dDispatcher.direct("Search", Session("urlreferer") & ".aspx", "type=tab&pageid=" & strPageId))
                                End If
                            End If
                            lblPONo.Text = strNewPO
                            cmdRaise.Visible = "False"
                            'Chk whether the Submit button should be enable
                        Case WheelMsgNum.NotSave
                            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                        Case WheelMsgNum.Duplicate
                            Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)
                        Case -1
                            'Common.NetMsgbox(Me, lblSupplier.Text & " company is currently inactive.", MsgBoxStyle.Information)
                            Common.NetMsgbox(Me, "Company is currently inactive.", MsgBoxStyle.Information)
                        Case -2
                            'Common.NetMsgbox(Me, lblSupplier.Text & " company is being deleted.", MsgBoxStyle.Information)
                            Common.NetMsgbox(Me, "Company is being deleted.", MsgBoxStyle.Information)
                    End Select
                End If
                Select Case ViewState("mode")
                    Case "po"
                    Case "rfq"
                End Select
            ElseIf ViewState("type") = "mod" Then
                If blnValid Then
                    objPO1.updatePO(dsPO, False, False)
                    objPO = Nothing
                    If Request.QueryString("Frm") = "Dashboard" Then
                        Common.NetMsgbox(Me, "Purchase Order Number " & lblPONo.Text & " has been updated.", dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "type=tab&pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "" Then
                        Common.NetMsgbox(Me, "Purchase Order Number " & lblPONo.Text & " has been updated.", dDispatcher.direct("PR", "viewShoppingCart.aspx", "type=tab&pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "RFQComSummary" Then
                        Common.NetMsgbox(Me, "Purchase Order Number " & lblPONo.Text & " has been updated.", dDispatcher.direct("RFQ", "RFQ_List.aspx", "type=tab&pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "BuyerCatalogueSearch" Then
                        Common.NetMsgbox(Me, "Purchase Order Number " & lblPONo.Text & " has been updated.", dDispatcher.direct("Search", "BuyerCatalogueSearch.aspx", "type=tab&pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "ConCatSearch" Then
                        Common.NetMsgbox(Me, "Purchase Order Number " & lblPONo.Text & " has been created.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                    Else
                        Common.NetMsgbox(Me, "Purchase Order Number " & lblPONo.Text & " has been updated.", dDispatcher.direct("PO", Session("urlreferer") & ".aspx", "type=tab&pageid=" & strPageId))
                    End If

                End If
                Select Case ViewState("mode")
                    Case "po"
                    Case "rfq"
                End Select
            End If

        Else
            If strMsg <> "" Then
                lblMsg.Text = strMsg
            Else
                lblMsg.Text = ""
            End If
        End If
    End Sub

    Private Sub cmdRaise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRaise.Click
        SavePO()
        'Dim dsPO As New DataSet
        'Dim objPO As New PurchaseOrder
        'Dim strMsg As String = ""
        'blnValid = True
        'blnItem = False

        'If validateDatagrid(strMsg) Then
        '    dsPO = bindPO()
        '    Select Case ViewState("type")
        '        Case "cart", "rfq"
        '            Dim strNewPO As String
        '            Dim intMsg As Integer
        '            If hidApproval.Value = "1" Then
        '                If Page.IsValid Then
        '                    If ViewState("blnBill") <> 0 And cboBillCode.SelectedItem.Value = "" Then
        '                        lblMsg.Text = "<ul type='disc'><li>Bill To is required.<ul type='disc'></ul></li></ul>"
        '                    Else
        '                        lblMsg.Text = ""
        '                        intMsg = objPO.insertPO(dsPO, strNewPO)
        '                        objPO = Nothing
        '                        'redirect to ExceedBCM before approval page
        '                        Select Case intMsg
        '                            Case WheelMsgNum.Save
        '                                If blnItem Then ' item exists
        '                                    If Session("Env") <> "FTN" Then
        '                                        If ViewState("BCM") > 0 Then
        '                                            If checkMandatory(strMsg) Then
        '                                                Response.Redirect("ExceedBCM.aspx?pageid=" & strPageId & "&prid=" & strNewPO & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.00") & "&currency=" & ViewState("Currency"))
        '                                            Else
        '                                                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        '                                            End If
        '                                        Else
        '                                            'Response.Redirect("ApprovalSetup.aspx?pageid=" & strPageId & "&msg=0&prid=" & strNewPO & "&pocost=" & Format((CDbl(ViewState("POCost")) * CDbl(lblRate.Text)), "#0.00") & "&currency=" & ViewState("Currency"))
        '                                            Response.Redirect("ApprovalSetup.aspx?pageid=" & strPageId & "&msg=0&prid=" & strNewPO & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.00") & "&currency=" & ViewState("Currency"))
        '                                        End If
        '                                    Else
        '                                        'Response.Redirect("ApprovalSetup.aspx?pageid=" & strPageId & "&msg=0&prid=" & strNewPO & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.00") & "&currency=" & ViewState("Currency"))
        '                                        lblPONo.Text = strNewPO
        '                                        cmdRaise.Visible = "False"
        '                                        SubmitPO()
        '                                    End If
        '                                Else
        '                                    Common.NetMsgbox(Me, "There are no items in this PR.", MsgBoxStyle.Information)
        '                                End If

        '                            Case WheelMsgNum.NotSave
        '                                Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
        '                            Case WheelMsgNum.Duplicate
        '                                Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)
        '                            Case -1
        '                                Common.NetMsgbox(Me, lblSupplier.Text & " company is currently inactive.", MsgBoxStyle.Information)
        '                            Case -2
        '                                Common.NetMsgbox(Me, lblSupplier.Text & " company is being deleted.", MsgBoxStyle.Information)
        '                        End Select
        '                    End If
        '                End If
        '            Else
        '                If blnValid Then
        '                    intMsg = objPO.insertPO(dsPO, strNewPO)
        '                    objPO = Nothing
        '                    'redirect to confirm page
        '                    Select Case intMsg
        '                        Case WheelMsgNum.Save
        '                            'Response.Redirect("../PR/PRConfirm.aspx?pageid=" & strPageId & "&type=S&prid=" & strNewPO)
        '                            'Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        '                            Common.NetMsgbox(Me, "Purchase Order Number " & strNewPO & " has been created.")
        '                            lblPONo.Text = strNewPO
        '                            cmdRaise.Visible = "False"
        '                            'Chk whether the Submit button should be enable

        '                        Case WheelMsgNum.NotSave
        '                            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
        '                        Case WheelMsgNum.Duplicate
        '                            Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)
        '                        Case -1
        '                            Common.NetMsgbox(Me, lblSupplier.Text & " company is currently inactive.", MsgBoxStyle.Information)

        '                        Case -2
        '                            Common.NetMsgbox(Me, lblSupplier.Text & " company is being deleted.", MsgBoxStyle.Information)

        '                    End Select
        '                End If
        '            End If

        '        Case "list"
        '            If hidApproval.Value = "1" Then
        '                If Page.IsValid Then
        '                    If ViewState("blnBill") <> 0 And cboBillCode.SelectedItem.Value = "" Then
        '                        lblMsg.Text = "<ul type='disc'><li>Bill To is required.<ul type='disc'></ul></li></ul>"
        '                    Else
        '                        lblMsg.Text = ""
        '                        'objPR.updatePR(dsPR)
        '                        'objPR = Nothing
        '                        If blnItem Then ' item exists
        '                            If Session("Env") <> "FTN" Then
        '                                If ViewState("BCM") > 0 Then
        '                                    If checkMandatory(strMsg) Then
        '                                        Response.Redirect("ExceedBCM.aspx?prid=" & hidNewPO.Value & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.00") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId)
        '                                    Else
        '                                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        '                                    End If
        '                                Else
        '                                    Response.Redirect("ApprovalSetup.aspx?msg=0&prid=" & hidNewPO.Value & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.00") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId)
        '                                End If
        '                            Else
        '                                Response.Redirect("ApprovalSetup.aspx?msg=0&prid=" & hidNewPO.Value & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.00") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId)
        '                            End If

        '                        Else
        '                            Common.NetMsgbox(Me, "There are no items in this PR.", MsgBoxStyle.Information)
        '                        End If
        '                    End If
        '                End If
        '            Else
        '                If blnValid Then
        '                    'objPR.updatePR(dsPR)
        '                    'objPR = Nothing
        '                    'redirect to listing page
        '                    Response.Redirect("../PO/SearchPR_all.aspx?caller=buyer&pageid=7")
        '                End If
        '            End If
        '    End Select
        'Else
        '    If strMsg <> "" Then
        '        lblMsg.Text = strMsg
        '    Else
        '        lblMsg.Text = ""
        '    End If
        'End If

    End Sub

    Sub SubmitPO()
        Dim dt As New DataTable
        Dim objPO As New PurchaseOrder
        Dim objPO1 As New PurchaseOrder_Buyer
        Dim intIndex, intMsg As Integer

        'If Session("Env") = "FTN" Then
        '    dt = objPO1.getPOApprFlow(True)
        'Else
        '    dt = objPO1.getPOApprFlow(False)
        'End If
        dt = objPO1.getPOApprFlow(True)

        If dt.Rows.Count = 0 Then
            Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
            Exit Sub
        ElseIf dt.Rows.Count > 1 Then
            '      Common.NetMsgbox(Me, "Please proceed to Approval Setup to select the Approval Flow.", MsgBoxStyle.Information)
            Response.Redirect(dDispatcher.direct("PO", "POApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&type=" & ViewState("type") & "&poid=" & lblPONo.Text & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.00") & "&Frm=" & Me.Request.QueryString("Frm") & "&dpage=" & Request.QueryString("dpage") & "&currency=" & ViewState("Currency")))
            Exit Sub
        Else
            intMsg = objPO1.submitPO(lblPONo.Text, dt, , , False)
            Select Case intMsg
                Case WheelMsgNum.Save
                    If Session("urlreferer") = "" Then
                        Common.NetMsgbox(Me, "Purchase Order Number " & lblPONo.Text & " has been submitted.", dDispatcher.direct("PO", "viewShoppingCart.aspx", "type=tab&pageid=" & strPageId))
                    Else
                        If Session("urlreferer") = "RFQComSummary" Then
                            Common.NetMsgbox(Me, "Purchase Order Number " & lblPONo.Text & " has been created.", Session("urlrefererForRFQ"))
                        ElseIf Session("urlreferer") = "BuyerCatalogueSearch" Then
                            Common.NetMsgbox(Me, "Purchase Order Number " & lblPONo.Text & " has been submitted.", dDispatcher.direct("Search", Session("urlreferer") & ".aspx", "type=tab&pageid=" & strPageId))
                        ElseIf Request.QueryString("Frm") = "Dashboard" Then
                            Common.NetMsgbox(Me, "Purchase Order Number " & lblPONo.Text & " has been submitted.", dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "type=tab&pageid=" & strPageId))
                        ElseIf Session("urlreferer") = "ConCatSearch" Then
                            Common.NetMsgbox(Me, "Purchase Order Number " & lblPONo.Text & " has been created.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                        Else
                            Common.NetMsgbox(Me, "Purchase Order Number " & lblPONo.Text & " has been submitted.", dDispatcher.direct("PO", Session("urlreferer") & ".aspx", "type=tab&pageid=" & strPageId))
                        End If

                    End If


                    'lblPONo.Text = strNewPO
                    cmdRaise.Visible = "False"
                    cmdSetup.Visible = "False"
                    cmdSubmit.Visible = False

                Case WheelMsgNum.NotSave
                    Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                Case WheelMsgNum.Delete
                    Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

            End Select
        End If
    End Sub

    'Sub AddRow(ByVal intCell As Integer, ByVal strLabel As String, ByVal dblTotal As Double, ByVal blnShowGST As Boolean)
    '    'adding totals row
    '    Dim intL, intColToRemain As Integer
    '    Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
    '    Dim intTotalCol As Integer

    '    intTotalCol = EnumShoppingCart.icRemark + intCnt - 1

    '    For intL = 0 To intTotalCol
    '        addCell(row)
    '    Next

    '    If ViewState("GST") = "product" And blnShowGST Then
    '        intColToRemain = 6 '//col bf label, label, total,gst,col after total
    '    Else
    '        intColToRemain = 5 '//col bf label, label, total,col after total
    '    End If

    '    For intL = 0 To intTotalCol - intColToRemain
    '        row.Cells.RemoveAt(1)
    '    Next

    '    row.Cells(0).ColumnSpan = intCell - 1

    '    Dim txtTotal As New TextBox
    '    txtTotal.ID = "Total"
    '    txtTotal.Text = Format(dblTotal, "#,##0.00")
    '    txtTotal.CssClass = "lblnumerictxtbox"
    '    txtTotal.ReadOnly = True
    '    txtTotal.Width = System.Web.UI.WebControls.Unit.Pixel(120)
    '    txtTotal.Font.Bold = True
    '    row.Cells(2).Controls.Add(txtTotal)
    '    row.Cells(1).Text = strLabel
    '    row.Cells(2).HorizontalAlign = HorizontalAlign.Right
    '    row.Cells(1).Font.Bold = True

    '    If dblTotal <> 0 Then
    '        ViewState("POCost") = dblTotal
    '    End If

    '    Dim txtTax As New TextBox
    '    If ViewState("GST") = "product" And blnShowGST Then
    '        txtTax.ID = "Tax"
    '        txtTax.CssClass = "lblnumerictxtbox"
    '        txtTax.ReadOnly = True
    '        txtTax.Width = System.Web.UI.WebControls.Unit.Pixel(100)
    '        txtTax.Font.Bold = True
    '        row.Cells(4).HorizontalAlign = HorizontalAlign.Right
    '        If dblTotalGst = 0 Then
    '            txtTax.Text = "0.0000"
    '        Else
    '            txtTax.Text = Format(dblTotalGst, "#,##0.00")
    '        End If
    '        row.Cells(4).Controls.Add(txtTax)
    '        row.Cells(5).ColumnSpan = intTotalCol - (intCell - 1)
    '    Else
    '        row.Cells(4).ColumnSpan = intTotalCol - (intCell - 2)
    '    End If

    '    If hidCost.Value = "" Then
    '        hidCost.Value = ViewState("POCost")
    '    End If

    '    row.BackColor = Color.FromName("#f4f4f4")
    '    dtgShopping.Controls(0).Controls.Add(row)

    '    If ViewState("GST") = "product" And blnShowGST Then
    '        hidTax.Value = txtTax.ClientID
    '    End If
    '    hidTotal.Value = hidTotal.Value & "," & txtTotal.ClientID
    'End Sub

    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub

    Private Sub cboBillCode_Load()
        If cboBillCode.SelectedItem.Text = "Free Form" Then
            enableBill(True)
        Else
            enableBill(False)
        End If
    End Sub

    Private Sub cboBillCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboBillCode.SelectedIndexChanged
        If cboBillCode.SelectedItem.Value = "F" Or cboBillCode.SelectedItem.Value = "" Then
            If cboBillCode.SelectedItem.Value = "" Then
                enableBill(False)
            Else
                enableBill(True)
            End If

            txtBillAdd1.Text = ""
            txtBillAdd2.Text = ""
            txtBillAdd3.Text = ""
            txtBillPostCode.Text = ""
            txtBillCity.Text = ""
            cboState.SelectedIndex = 0
            cboCountry.SelectedIndex = 0 '"" 'viewstate("CountryDefault")
        Else
            enableBill(False)
            fillAddress()
        End If
    End Sub

    Private Sub enableBill(ByVal bln As Boolean)
        txtBillAdd1.Enabled = bln
        txtBillAdd2.Enabled = bln
        txtBillAdd3.Enabled = bln
        txtBillPostCode.Enabled = bln
        txtBillCity.Enabled = bln
        cboState.Enabled = bln
        cboCountry.Enabled = bln
    End Sub

    Private Sub cboCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCountry.SelectedIndexChanged
        Dim objGlobal As New AppGlobals
        If cboCountry.SelectedItem.Value <> "" Then
            objGlobal.FillState(cboState, cboCountry.SelectedItem.Value)
        End If
        objGlobal = Nothing
    End Sub

    Private Sub updatePODIdx()
        Dim dgItem As DataGridItem
        Dim objPO As New PurchaseOrder_Buyer
        Dim sProductCode As String
        Dim strSQL As String
        Dim sPodIndex As Integer
        Dim strAryQuery(0) As String

        'Update the grid PODIndex Sync with DB
        For Each dgItem In dtgShopping.Items
            sProductCode = CInt(CType(dgItem.FindControl("lblProductCode"), Label).Text)
            sPodIndex = CInt(dgItem.Cells(EnumShoppingCart.icNo).Text)

            strSQL = objPO.updatePODIndex(hidNewPO.Value, sProductCode, sPodIndex)
            Common.Insert2Ary(strAryQuery, strSQL)
            objDB.BatchExecute(strAryQuery)
        Next
    End Sub

    Private Sub cmdRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemove.Click
        Dim dgItem As DataGridItem
        Dim objPO As New PurchaseOrder
        Dim objPO1 As New PurchaseOrder_Buyer
        Dim objShopping As New ShoppingCart
        Dim sProductCode As String, sFullClientId As String, sClientId As String
        Dim sProductDesc As String = ""
        Dim chkItem As CheckBox
        Dim i As Integer = 0
        Dim strAryQuery(0) As String
        Dim strSQL, strSQL2 As String
        Dim sPodIndex As Integer
        Dim InternalCount As Integer = 0
        ' Update the grid PODIndex Sync with DB
        ' This must run and update first
        updatePODIdx()

        For Each dgItem In dtgShopping.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then

                If CType(dgItem.FindControl("lblProductCode"), Label).Text = "&nbsp;" Or CType(dgItem.FindControl("lblProductCode"), Label).Text = "" Then
                    sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
                    sProductDesc = CType(dgItem.FindControl("lblProductDesc"), Label).Text
                    sFullClientId = CType(dgItem.FindControl("lblProductCode"), Label).ClientID
                Else
                    sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
                    sFullClientId = CType(dgItem.FindControl("lblProductCode"), Label).ClientID
                End If

                'sProductCode = CInt(CType(dgItem.FindControl("lblProductCode"), Label).Text)
                'sFullClientId = CType(dgItem.FindControl("lblProductCode"), Label).ClientID
                sPodIndex = CInt(dgItem.Cells(EnumShoppingCart.icNo).Text)

                If ViewState("type") = "new" Then
                    Select Case ViewState("mode")
                        Case "po", "cc"
                            strSQL = objPO1.deletePOItemSQL(hidNewPO.Value, sProductCode, sPodIndex)
                        Case "rfq"
                            strSQL = objPO1.deleteRFQItemSQL(ViewState("rfqid"), sProductCode)
                    End Select
                ElseIf ViewState("type") = "mod" Then
                    strSQL = objPO1.deletePOItemSQL(hidNewPO.Value, sProductCode, sPodIndex)
                    strSQL2 = objPO1.updatePOHeaderSQL(hidNewPO.Value, sProductCode, sPodIndex)
                    Common.Insert2Ary(strAryQuery, strSQL2)
                    Select Case ViewState("mode")
                        Case "po"
                        Case "rfq"
                    End Select
                End If

                sClientId = Mid(sFullClientId, InStr(sFullClientId, "_") + 1, InStr(Mid(sFullClientId, InStr(sFullClientId, "_") + 1), "_") - 1) & "|"
                hidClientId.Value = hidClientId.Value.Replace(sClientId, "")
                'hidClientId.Value = hidClientId.Value & sClientId  ctl05|
                hidTotalClientId.Value = hidTotalClientId.Value - 1
                Dim GetPO As Boolean
                GetPO = objPO1.get_PO(hidNewPO.Value, sProductCode, sPodIndex)
                If GetPO = False Then
                    If ViewState("mode") <> "cc" Then
                        Session("ProdList") = objShopping.RemoveProductCodeFromList(sProductCode, Session("ProdList"))
                    Else
                        Session("ProdList") = objShopping.RemovePRProductCodeFromList(sProductCode, Session("ProdList"), i, sProductDesc, InternalCount)
                    End If
                End If
                'Session("ProdList") = objShopping.RemoveProductCodeFromList(sProductCode, Session("ProdList"))
                Common.Insert2Ary(strAryQuery, strSQL)
            End If
            i = i + 1
        Next

        InternalCount = 0

        If objDB.BatchExecute(strAryQuery) Then
            'hidCost.Value = objPR.updatePOCost(hidNewPO.Value)
            Common.NetMsgbox(Me, "Item(s) Deleted.", MsgBoxStyle.Information)
            Dim dsDelivery As New DataSet
            Dim objAdmin As New Admin
            Dim objBudget As New BudgetControl
            dsDelivery = objAdmin.PopulateAddr("D", "", "", "")
            dvwDelivery = dsDelivery.Tables(0).DefaultView
            strDeliveryDefault = objAdmin.user_Default_Add("D")
            Session("CurrentScreen") = "RemoveItem"
            'If Session("Env") <> "FTN" Then
            '    Dim dsBCM As New DataSet
            '    dsBCM = objBudget.getBCMListByUser(Session("UserId"))
            '    dtBCM = dsBCM.Tables(0)
            '    Bindgrid()
            '    hidTotal.Value = ""
            '    'addDGTotal()
            '    hid2.Value = ""
            '    objAdmin = Nothing
            '    objBudget = Nothing
            'Else

            '    'Update the grid PODIndex Sync with DB
            '    If ViewState("type") = "new" Then
            '        Select Case ViewState("mode")
            '            Case "po"
            '            Case "rfq"
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

            '                'sProductCode = CInt(CType(dgItem.FindControl("lblProductCode"), Label).Text)
            '                sPodIndex = CInt(dgItem.Cells(EnumShoppingCart.icNo).Text)


            '                Dim GetPO As Boolean
            '                GetPO = objPO1.get_PO(hidNewPO.Value, sProductCode, sPodIndex)
            '                'If GetPO = False Then
            '                '    Session("ProdList") = objShopping.RemoveProductCodeFromList(sProductCode, Session("ProdList"))
            '                'End If

            '                If GetPO = False Then
            '                    If ViewState("mode") <> "cc" Then
            '                        Session("ProdList") = objShopping.RemoveProductCodeFromList(sProductCode, Session("ProdList"))
            '                    Else
            '                        Session("ProdList") = objShopping.RemovePRProductCodeFromList(sProductCode, Session("ProdList"), j, sProductDesc, InternalCount)
            '                    End If
            '                End If
            '            End If
            '            j = j + 1
            '        Next
            '        updatePODIdx()
            '        Select Case ViewState("mode")
            '            Case "po"
            '            Case "rfq"
            '        End Select
            '    End If

            '    Bindgrid()
            'End If
            'Update the grid PODIndex Sync with DB
            If ViewState("type") = "new" Then
                Select Case ViewState("mode")
                    Case "po"
                    Case "rfq"
                End Select
            ElseIf ViewState("type") = "mod" Then
                Dim j As Integer = 0
                For Each dgItem In dtgShopping.Items
                    chkItem = dgItem.FindControl("chkSelection")
                    If chkItem.Checked Then

                        If CType(dgItem.FindControl("lblProductCode"), Label).Text = "&nbsp;" Or CType(dgItem.FindControl("lblProductCode"), Label).Text = "" Then
                            sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
                            sProductDesc = CType(dgItem.FindControl("lblProductDesc"), Label).Text
                        Else
                            sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
                        End If

                        'sProductCode = CInt(CType(dgItem.FindControl("lblProductCode"), Label).Text)
                        sPodIndex = CInt(dgItem.Cells(EnumShoppingCart.icNo).Text)


                        Dim GetPO As Boolean
                        GetPO = objPO1.get_PO(hidNewPO.Value, sProductCode, sPodIndex)
                        'If GetPO = False Then
                        '    Session("ProdList") = objShopping.RemoveProductCodeFromList(sProductCode, Session("ProdList"))
                        'End If

                        If GetPO = False Then
                            If ViewState("mode") <> "cc" Then
                                Session("ProdList") = objShopping.RemoveProductCodeFromList(sProductCode, Session("ProdList"))
                            Else
                                Session("ProdList") = objShopping.RemovePRProductCodeFromList(sProductCode, Session("ProdList"), j, sProductDesc, InternalCount)
                            End If
                        End If
                    End If
                    j = j + 1
                Next
                updatePODIdx()
                Select Case ViewState("mode")
                    Case "po"
                    Case "rfq"
                End Select
            End If

            'Dim dtrd As DataRow
            'Dim chkItem As CheckBox
            Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark As TextBox
            Dim lblItemLine, txtBudget, txtDelivery As Label
            'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
            Dim cboGLCode, cboCust, cboGSTRate, cboGSTTaxCode As DropDownList
            Dim strItem, strItemCust As New ArrayList()
            'Dim dgItem As DataGridItem
            Dim dgItem2 As DataGridItem
            Dim k As Integer = 0
            Dim strItemHead As New ArrayList()

            strItemHead.Add(New String() {txtAttention.Text, txtInternal.Text, txtExternal.Text, cboBillCode.SelectedIndex, cboShipmentTerm.SelectedIndex, cboShipmentMode.SelectedIndex, txtShipVia.Text, chkUrgent.Checked, txtShippingHandling.Text, cboVendor.SelectedIndex, cboPayTerm.SelectedIndex, cboPayMethod.SelectedIndex})

            For Each dgItem In dtgShopping.Items
                txtQty = dgItem.FindControl("txtQty")
                'cboGLCode = dgItem.FindControl("cboGLCode")
                txtPrice = dgItem.FindControl("txtPrice")
                'txtBudget = dgItem.FindControl("txtBudget")
                'hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                txtDelivery = dgItem.FindControl("txtDelivery")
                hidDelCode = dgItem.FindControl("hidDelCode")
                txtEstDate = dgItem.FindControl("txtEstDate")
                txtWarranty = dgItem.FindControl("txtWarranty")
                txtRemark = dgItem.FindControl("txtRemark")
                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                cboGSTRate = dgItem.FindControl("cboGSTRate")
                cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")

                chkItem = dgItem.FindControl("chkSelection")
                If chkItem.Checked Then
                Else
                    'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                    'strItem.Add(New String() {"", txtQty.Text, txtPrice.Text, "", "", txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
                    strItem.Add(New String() {"", txtQty.Text, txtPrice.Text, "", "", txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGSTRate.SelectedIndex, cboGSTTaxCode.SelectedIndex})
                End If

                If (ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr") Or ViewState("modeRFQFromPR_Index") <> "" Then
                    'Dim objAdmin As New Admin

                    If ViewState("modeRFQFromPR_Index") <> "" Then
                        dvwCus = objAdmin.getCustomFieldPR("", "PR", ViewState("modeRFQFromPR_Index"))
                    Else
                        dvwCus = objAdmin.getCustomField("", "PO")
                    End If

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
                End If

                k += 1
            Next
            Session("strItem") = strItem
            Session("strItemCust") = strItemCust
            Session("strItemHead") = strItemHead

            Bindgrid()
        End If
        objPO = Nothing

        If ViewState("type") = "new" Then
            Select Case ViewState("mode")
                Case "po", "cc"
                    Response.Redirect(Session("RaisePOURL"))
                Case "rfq"
            End Select
        ElseIf ViewState("type") = "mod" Then
            Response.Redirect(Session("RaisePOURL"))
            Select Case ViewState("mode")
                Case "po"
                Case "rfq"
            End Select
        End If

    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

        Dim objPO As New PurchaseOrder_Buyer
        objPO.deletePO(hidNewPO.Value)

        If Session("urlreferer") = "" Then
            Common.NetMsgbox(Me, "PO Deleted.", dDispatcher.direct("PO", "SearchPR_all.aspx", "caller=buyer&pageid=7"), MsgBoxStyle.Information)

        ElseIf Session("urlreferer") = "DashboardPurMgr" Then
            Common.NetMsgbox(Me, "PO Deleted.", dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId), MsgBoxStyle.Information)

        ElseIf Session("urlreferer") = "RFQComSummary" Then
            Common.NetMsgbox(Me, "Purchase Order Number " & lblPONo.Text & " has been deleted.", dDispatcher.direct("RFQ", "RFQ_List.aspx", "type=tab&pageid=" & strPageId))

        ElseIf Session("urlreferer") = "BuyerCatalogueSearch" Then
            Common.NetMsgbox(Me, "Purchase Order Number " & lblPONo.Text & " has been deleted.", dDispatcher.direct("Search", "BuyerCatalogueSearch.aspx", "type=tab&pageid=" & strPageId))

        ElseIf Session("urlreferer") = "ConCatSearch" Then
            Common.NetMsgbox(Me, "Purchase Order Number " & lblPONo.Text & " has been deleted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))

        Else
            Common.NetMsgbox(Me, "PO Deleted.", dDispatcher.direct("PO", Session("urlreferer") & ".aspx", "pageid=" & strPageId), MsgBoxStyle.Information)
        End If

        objPO = Nothing
    End Sub

    Private Function checkMandatory(ByRef strMsg As String) As Boolean
        strMsg = ""
        Dim objPR As New PR
        If Not objPR.checkUserAccExist() Then
            'strMsg = "You are not assigned to any Budget Account Code. " & vbCrLf & "Please contact the Finance Manager. "
            strMsg = "You are not assigned to any Budget Account Code. ""&vbCrLf&""Please contact the Finance Manager. "
        End If

        If ViewState("blnBill") = 0 Then
            If strMsg <> "" Then
                strMsg &= """&vbCrLf&"""
            End If
            strMsg &= "Please ask your Buyer Admin to add in a Billing Address! "
        End If

        If ViewState("blnDelivery") = 0 Then
            If strMsg <> "" Then
                strMsg &= """&vbCrLf&"""
            End If
            strMsg &= "Please ask your Buyer Admin to add in a Delivery Address! "
        End If

        If strMsg <> "" Then
            checkMandatory = False
        Else
            checkMandatory = True
        End If
        objPR = Nothing
    End Function

    Private Sub cmdSetup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSetup.Click
        'If lblPONo.Text = "To Be Allocated By System" Then
        '    Me.cmdRaise_Click(cmdRaise, New System.EventArgs())
        '    Dim AA As String
        '    AA = "DD"
        'Else
        '    Dim objPO As New PurchaseOrder
        '    'intMsg = objPO.submitPO(lblPONo.Text, PRStatus.Submitted, ViewState("msg"))

        'End If

        If Page.IsValid Then
            Dim strMsg As String = ""
            If validateDatagrid(strMsg) Then
                blnItem = False
                If ViewState("blnBill") <> 0 And cboBillCode.SelectedItem.Value = "" And ViewState("modePR") <> "pr" Then
                    lblMsg.Text = "<ul type='disc'><li>Bill To is required.<ul type='disc'></ul></li></ul>"
                Else
                    lblMsg.Text = ""
                    Dim dsPR As New DataSet
                    Dim objPR As New PR

                    Dim intMsg As Integer
                    dsPR = bindPO()

                    Select Case ViewState("type")
                        Case "cart", "rfq"
                            Dim strNewPO As String
                            intMsg = objPR.insertPR(dsPR, strNewPO)
                            Select Case intMsg
                                Case WheelMsgNum.Save
                                    If blnItem Then ' item exists
                                        'If Session("Env") = "FTN" Then
                                        '    If ViewState("BCM") > 0 Then
                                        '        If checkMandatory(strMsg) Then
                                        '            Response.Redirect(dDispatcher.direct("PR", "ExceedBCM.aspx", "poid=" & strNewPO & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId))
                                        '        Else
                                        '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                                        '        End If
                                        '    Else
                                        '        Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "msg=0&poid=" & strNewPO & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId))
                                        '    End If
                                        'Else
                                        '    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "msg=0&poid=" & strNewPO & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId))
                                        'End If
                                        If ViewState("BCM") > 0 Then
                                            If checkMandatory(strMsg) Then
                                                Response.Redirect(dDispatcher.direct("PR", "ExceedBCM.aspx", "poid=" & strNewPO & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId))
                                            Else
                                                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                                            End If
                                        Else
                                            Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "msg=0&poid=" & strNewPO & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId))
                                        End If
                                    Else
                                        Common.NetMsgbox(Me, "There are no items in this PR.", MsgBoxStyle.Information)
                                    End If

                                Case WheelMsgNum.NotSave
                                    Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                                Case WheelMsgNum.Duplicate
                                    Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)
                                Case -1

                                    If lblSupplier.Text <> "(Multiple Vendors)" Then
                                        Common.NetMsgbox(Me, lblSupplier.Text & " company is currently inactive.", MsgBoxStyle.Information)
                                    Else
                                        Common.NetMsgbox(Me, "One of the vendor company is currently inactive.", MsgBoxStyle.Information)
                                    End If

                                Case -2

                                    If lblSupplier.Text <> "(Multiple Vendors)" Then
                                        Common.NetMsgbox(Me, lblSupplier.Text & " company is being deleted.", MsgBoxStyle.Information)
                                    Else
                                        Common.NetMsgbox(Me, "One of the vendor company is being deleted.", MsgBoxStyle.Information)
                                    End If


                            End Select
                        Case "list"
                            objPR.updatePR(dsPR)
                            If blnItem Then ' item exists
                                If ViewState("BCM") > 0 Then
                                    If checkMandatory(strMsg) Then
                                        Response.Redirect(dDispatcher.direct("PR", "ExceedBCM.aspx", "pageid=" & strPageId & "&poid=" & hidNewPO.Value & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                    Else
                                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                                    End If
                                Else
                                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "msg=0&poid=" & hidNewPO.Value & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId))
                                End If
                            Else
                                Common.NetMsgbox(Me, "There are no items in this PR.", MsgBoxStyle.Information)
                            End If

                    End Select
                End If
            Else
                If strMsg <> "" Then
                    lblMsg.Text = strMsg
                Else
                    lblMsg.Text = ""
                End If
            End If
        End If
    End Sub

    'Private Function checkUserAccExist() As Boolean
    '    Dim ds As New DataSet
    '    Dim objPR As New PR
    '    ds = objPR.getUserAcc
    '    If ds.Tables(0).Rows.Count > 0 Then
    '        checkUserAccExist = True
    '    Else
    '        checkUserAccExist = False
    '    End If
    '    objPR = Nothing
    'End Function

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
                    objFile.FileUpload(File1, EnumUploadType.DocAttachmentTemp, "PO", EnumUploadFrom.FrontOff, Session.SessionID)
                    Select Case ViewState("mode")
                        Case "po"
                        Case "rfq"
                    End Select
                ElseIf ViewState("type") = "mod" Then
                    objFile.FileUpload(File1, EnumUploadType.DocAttachmentTemp, "PO", EnumUploadFrom.FrontOff, ViewState("poid"))
                    Select Case ViewState("mode")
                        Case "po"
                        Case "rfq"
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
        Dim lblItemLine, txtBudget, txtDelivery As Label
        'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
        Dim cboGLCode, cboCust, cboGSTRate, cboGSTTaxCode As DropDownList
        Dim strItem, strItemCust As New ArrayList()
        Dim dgItem As DataGridItem
        Dim dgItem2 As DataGridItem
        Dim k As Integer = 0
        Dim strItemHead As New ArrayList()

        strItemHead.Add(New String() {txtAttention.Text, txtInternal.Text, txtExternal.Text, cboBillCode.SelectedIndex, cboShipmentTerm.SelectedIndex, cboShipmentMode.SelectedIndex, txtShipVia.Text, chkUrgent.Checked, txtShippingHandling.Text, cboVendor.SelectedIndex, cboPayTerm.SelectedIndex, cboPayMethod.SelectedIndex})

        For Each dgItem In dtgShopping.Items
            txtQty = dgItem.FindControl("txtQty")
            'cboGLCode = dgItem.FindControl("cboGLCode")
            txtPrice = dgItem.FindControl("txtPrice")
            'txtBudget = dgItem.FindControl("txtBudget")
            'hidBudgetCode = dgItem.FindControl("hidBudgetCode")
            txtDelivery = dgItem.FindControl("txtDelivery")
            hidDelCode = dgItem.FindControl("hidDelCode")
            txtEstDate = dgItem.FindControl("txtEstDate")
            txtWarranty = dgItem.FindControl("txtWarranty")
            txtRemark = dgItem.FindControl("txtRemark")
            'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
            cboGSTRate = dgItem.FindControl("cboGSTRate")
            cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")

            chkItem = dgItem.FindControl("chkSelection")
            'If chkItem.Checked Then
            'Else
            '    strItem.Add(New String() {"", txtQty.Text, txtPrice.Text, "", "", txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
            'End If
            'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
            'strItem.Add(New String() {"", txtQty.Text, txtPrice.Text, "", "", txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
            strItem.Add(New String() {"", txtQty.Text, txtPrice.Text, "", "", txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGSTRate.SelectedIndex, cboGSTTaxCode.SelectedIndex})

            If (ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr") Or ViewState("modeRFQFromPR_Index") <> "" Then
                Dim objAdmin As New Admin

                If ViewState("modeRFQFromPR_Index") <> "" Then
                    dvwCus = objAdmin.getCustomFieldPR("", "PR", ViewState("modeRFQFromPR_Index"))
                Else
                    dvwCus = objAdmin.getCustomField("", "PO")
                End If

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
            End If

            k += 1
        Next
        Session("strItem") = strItem
        Session("strItemCust") = strItemCust
        Session("strItemHead") = strItemHead

        Bindgrid()
    End Sub

    Private Sub displayAttachFile()
        Dim objPO As New PurchaseOrder_Buyer
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        If ViewState("type") = "new" Then
            dsAttach = objPO.getPOTempAttach(Session.SessionID)
            Select Case ViewState("mode")
                Case "po"
                Case "rfq"
            End Select
        ElseIf ViewState("type") = "mod" Then
            dsAttach = objPO.getPOTempAttach(ViewState("poid"))
            Select Case ViewState("mode")
                Case "po"
                Case "rfq"
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
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "PO", EnumUploadFrom.FrontOff)
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
                If ViewState("modePR") <> "pr" Then
                    pnlAttach.Controls.Add(lnk)
                End If
                pnlAttach.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFile)
        End If
        objPO = Nothing
    End Sub

    Private Sub deleteAttach(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Dim objPR As New PR
        'If ViewState("type") = "new" Then
        '    objPR.deletePRAttachment(CType(sender, ImageButton).ID)
        '    Select Case ViewState("mode")
        '        Case "po"
        '        Case "rfq"
        '    End Select
        'ElseIf ViewState("type") = "mod" Then
        '    objPR.deletePRAttachment(CType(sender, ImageButton).ID)
        '    Select Case ViewState("mode")
        '        Case "po"
        '        Case "rfq"
        '    End Select
        'End If

        'displayAttachFile()
        'objPR = Nothing

        Dim objPO As New PurchaseOrder_Buyer
        If ViewState("type") = "new" Then
            objPO.delete_Attachment_Temp(CType(sender, ImageButton).ID)
            Select Case ViewState("mode")
                Case "po"
                Case "rfq"
            End Select
        ElseIf ViewState("type") = "mod" Then
            objPO.delete_Attachment_Temp(CType(sender, ImageButton).ID)
            Select Case ViewState("mode")
                Case "po"
                Case "rfq"
            End Select
        End If

        displayAttachFile()
        objPO = Nothing
    End Sub

    Private Function validateDatagrid(ByRef strMsg As String) As Boolean
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

        If cboVendor.SelectedIndex = 0 And Mid(cboVendor.SelectedValue, 1, 2) = "--" Then
            strMsg &= "<li>Vendor is required.</li>"
            validateDatagrid = False
        End If

        ' YapCL 2011Mar08: Issue 56
        If cboVendor.SelectedIndex = 0 And Mid(cboVendor.SelectedItem.Text, 1, 2) = "--" Then
            strMsg &= "<li>Vendor is required.</li>"
            validateDatagrid = False
        End If

        If cboVendor.SelectedIndex <> 0 Then
            Dim strRec_No As String = objDB.GetVal("SELECT CV_B_COY_ID FROM COMPANY_VENDOR WHERE CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CV_S_COY_ID = '" & cboVendor.SelectedItem.Value & "'")
            If strRec_No = "" Then
                strMsg &= "<li>Vendor not approved.</li>"
                validateDatagrid = False
            End If
        End If

        If cboPayTerm.SelectedIndex = 0 Then
            strMsg &= "<li>Payment Term is required.</li>"
            validateDatagrid = False
        End If

        If cboPayMethod.SelectedIndex = 0 Then
            strMsg &= "<li>Payment Method is required.</li>"
            validateDatagrid = False
        End If
        If cboShipmentTerm.SelectedIndex = 0 Then
            strMsg &= "<li>Shipment Term is required.</li>"
            validateDatagrid = False
        End If
        If cboShipmentMode.SelectedIndex = 0 Then
            strMsg &= "<li>Shipment Mode is required.</li>"
            validateDatagrid = False
        End If
        If cboBillCode.SelectedIndex = 0 And ViewState("modePR") <> "pr" Then
            strMsg &= "<li>Bill To is required.</li>"
            validateDatagrid = False
        End If

        ' YapCL 2011Mar08: Issue 23
        'If IsNumeric(txtShippingHandling.Text) = False Or Regex.IsMatch(Trim(txtShippingHandling.Text), "^\d{1,10}(\.\d{1,2})?$") = False Then
        'Michelle (14/5/2011) - To exclude the comma when validating
        If IsNumeric(txtShippingHandling.Text) = False Or Regex.IsMatch(Replace(Trim(txtShippingHandling.Text), ",", ""), "^\d{1,10}(\.\d{1,2})?$") = False Then
            strMsg &= "<li>Shipping & Handling is expecting numeric value.</li>"
            validateDatagrid = False
        End If

        ' YapCL 2011Mar28: Issue 182
        Dim dgItem As DataGridItem
        Dim txtprice As TextBox
        Dim strSQL As String
        Dim getQuoPrice As DataSet
        Dim getQuo As DataSet
        Dim QuoPrice As Decimal
        Dim Quo, s_Coy As String
        Dim objPO As New PurchaseOrder
        Dim objPO1 As New PurchaseOrder_Buyer
        Dim revPriceRange As RangeValidator
        Dim txtPriceCost As TextBox
        Dim revPrice As RegularExpressionValidator

        For Each dgItem In dtgShopping.Items
            txtPriceCost = dgItem.FindControl("txtPrice")
            revPriceRange = dgItem.FindControl("revPriceRange")
            revPrice = dgItem.FindControl("revPrice")

            If Left(txtPriceCost.Text, 1) = "," Then
                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Invalid unit price.<ul type='disc'></ul></li>"
                revPriceRange.Text = "?"
                validateDatagrid = False
            Else
                'Stage 3 Enhancement (GST-0019) - 22/07/2015 - CH
                revPriceRange.Text = ""

                If IsNumeric(txtPriceCost.Text) And Regex.IsMatch(Trim(CDec(txtPriceCost.Text)), "(?!^0*$)(?!^0*\.0*$)^\d{1,12}(\.\d{1,4})?$") Then
                    revPrice.Text = ""
                Else
                    strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Invalid unit price.<ul type='disc'></ul></li>"
                    revPrice.Text = "?"
                    validateDatagrid = False
                End If
            End If

            Dim txtEstDate As TextBox
            txtEstDate = dgItem.FindControl("txtEstDate")
            If Not IsDate(txtEstDate.Text) Then
                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Est. Date of Delivery must be greater than PO creation date (dd/mm/yyyy).</li>"
                validateDatagrid = False
            Else
                Dim txtDate1 As Date = CType(lblDate.Text, Date)
                Dim txtDate2 As Date = CType(txtEstDate.Text, Date)
                If ViewState("type") = "new" Then
                    Select Case ViewState("mode")
                        Case "po", "cc"
                            If txtDate2 <= txtDate1 Then
                                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Est. Date of Delivery must be greater than PO creation date (dd/mm/yyyy).</li>"
                                validateDatagrid = False
                            End If
                        Case "rfq"
                            If txtDate2 < txtDate1 Then
                                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Est. Date of Delivery must be greater than PO creation date (dd/mm/yyyy).</li>"
                                validateDatagrid = False
                            End If
                    End Select
                ElseIf ViewState("type") = "mod" Then
                    getQuo = objPO1.get_PO_Quo(ViewState("poid"))
                    If getQuo.Tables(0).Rows.Count > 0 Then 'getQuo.Tables.Count > 0 Then
                        Quo = Common.parseNull(getQuo.Tables(0).Rows(0)("pom_quotation_no"))
                        If Quo <> "" Then
                            If txtDate2 < txtDate1 Then
                                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Est. Date of Delivery must be greater than PO creation date (dd/mm/yyyy).</li>"
                                validateDatagrid = False
                            End If
                        ElseIf txtDate2 <= txtDate1 Then
                            strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Est. Date of Delivery must be greater than PO creation date (dd/mm/yyyy).</li>"
                            validateDatagrid = False
                        End If
                    End If
                    Select Case ViewState("mode")
                        Case "po"
                        Case "rfq"
                    End Select
                End If
            End If

            If ViewState("type") = "new" Then
                Select Case ViewState("mode")
                    Case "po"
                    Case "rfq"
                        txtprice = dgItem.FindControl("txtprice")
                        revPriceRange = dgItem.FindControl("revPriceRange")
                        getQuoPrice = objPO1.GetQuotationPrice(ViewState("rfqnum"), ViewState("quono"), HttpContext.Current.Session("CompanyId"), dgItem.Cells(EnumShoppingCart.icNo).Text, Request.QueryString("vendor"), dgItem.Cells(EnumShoppingCart.icVendorItemCode).Text, CType(dgItem.FindControl("lblProductDesc"), Label).Text)
                        If getQuoPrice.Tables(0).Rows.Count > 0 Then
                            QuoPrice = Common.parseNull(getQuoPrice.Tables(0).Rows(0)("rrd_unit_price"))
                            If ViewState("modePR") <> "pr" And (CDec(txtprice.Text) < 0 Or CDec(txtprice.Text) > QuoPrice) Then

                                strMsg &= "<li> Price item " & dgItem.Cells(EnumShoppingCart.icNo).Text & " is outside quotation range.</li>"
                                revPriceRange.Text = "?"
                                validateDatagrid = False
                            Else
                                revPriceRange.Text = ""
                            End If
                        End If
                End Select
            ElseIf ViewState("type") = "mod" Then
                getQuo = objPO1.get_PO_Quo(ViewState("poid"))
                If getQuo.Tables(0).Rows.Count > 0 Then
                    Quo = Common.parseNull(getQuo.Tables(0).Rows(0)("pom_quotation_no"))
                    s_Coy = Common.parseNull(getQuo.Tables(0).Rows(0)("POM_S_COY_ID"))
                    If Quo <> "" Then
                        txtprice = dgItem.FindControl("txtprice")
                        revPriceRange = dgItem.FindControl("revPriceRange")
                        getQuoPrice = objPO1.GetQuotationPrice(ViewState("rfqnum"), Quo, HttpContext.Current.Session("CompanyId"), dgItem.Cells(EnumShoppingCart.icNo).Text, s_Coy, dgItem.Cells(EnumShoppingCart.icVendorItemCode).Text, CType(dgItem.FindControl("lblProductDesc"), Label).Text)
                        If getQuoPrice.Tables(0).Rows.Count > 0 Then
                            QuoPrice = Common.parseNull(getQuoPrice.Tables(0).Rows(0)("rrd_unit_price"))
                            If ViewState("modePR") <> "pr" And (CDec(txtprice.Text) < 0 Or CDec(txtprice.Text) > QuoPrice) Then

                                strMsg &= "<li> Price item " & dgItem.Cells(EnumShoppingCart.icNo).Text & " is outside quotation range.</li>"
                                revPriceRange.Text = "?"
                                validateDatagrid = False
                            Else
                                revPriceRange.Text = ""
                            End If
                        End If

                    End If
                End If
                Select Case ViewState("mode")
                    Case "po"
                    Case "rfq"
                End Select
            End If

            Dim strGstRegNo As String
            Dim objGST As New GST
            'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
            Dim cboGSTRate, cboGSTTaxCode As DropDownList
            cboGSTRate = dgItem.FindControl("cboGSTRate")
            cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")

            If ViewState("isGST") = True Then
                Select Case ViewState("mode")
                    Case "bc"
                    Case "cc"
                        If cboVendor.SelectedItem.Value <> "" And cboVendor.SelectedItem.Value <> "&nbsp;" Then
                            strGstRegNo = objGST.chkGST(cboVendor.SelectedItem.Value)
                            If strGstRegNo <> "" AndAlso (cboGSTRate.SelectedItem.Value = "N/A" Or cboGSTRate.SelectedItem.Value = "" Or cboGSTRate.SelectedItem.Value = "&nbsp;") Then
                                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". GST Rate is required for GST registered vendor.<ul type='disc'></ul></li>"
                                validateDatagrid = False
                            End If
                        End If
                End Select

                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                If ViewState("mode") = "cc" Then
                    If cboVendor.SelectedItem.Value <> "" And cboVendor.SelectedItem.Value <> "&nbsp;" Then
                        strGstRegNo = objGST.chkGST(cboVendor.SelectedItem.Value)
                        If strGstRegNo <> "" AndAlso (cboGSTTaxCode.SelectedItem.Value = "NR" Or cboGSTTaxCode.SelectedItem.Value = "" Or cboGSTTaxCode.SelectedItem.Value = "&nbsp;") Then
                            strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". GST Tax Code is required.<ul type='disc'></ul></li>"
                            validateDatagrid = False
                        End If
                    End If
                Else
                    If cboGSTRate.SelectedItem.Value <> "" And cboGSTRate.SelectedItem.Value <> "N/A" Then
                        If objGST.chkValidTaxCode(cboGSTRate.SelectedItem.Value, cboGSTTaxCode.SelectedValue, "P") = False Then
                            strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Invalid Purchase Tax Code.<ul type='disc'></ul></li>"
                            validateDatagrid = False
                        End If
                    End If
                End If
            End If
        Next

        'If hid.SelectedIndex = 0 Then
        '    strMsg &= "<li>Bill To is required.</li>"
        '    validateDatagrid = False
        'End If

        Dim txtRemark As TextBox
        Dim txtQ As TextBox
        Dim cboCategory, cboDelivery, cboBudget As DropDownList
        Dim strItemType, strDelivery, strBudgetIndex


        strItemType = ""

        Dim GridItem As DataGridItem
        Dim PrefDS As DataSet
        Dim ProdCode, ProdGrp As String
        Dim strPPrefer As String = cboVendor.SelectedValue
        Dim strCPrefer As String
        Dim strC1st As String
        Dim strC2nd As String
        Dim strC3rd As String
        Dim CVendor As String
        Dim GVendor As Integer
        For Each GridItem In dtgShopping.Items

        Next

        For Each dgItem In dtgShopping.Items
            'Michelle (eBiz/354/09) - Check for category code
            'If UCase(HttpContext.Current.Session("CompanyId")) = "HLB" Then
            'mimi 2018-04-24 : remove hardcode HLB to PAMB
            If UCase(HttpContext.Current.Session("CompanyId")) = "PAMB" Then
                cboCategory = dgItem.FindControl("cboCategoryCode")
                If strItemType = "" Then
                    strItemType = UCase(Mid(cboCategory.SelectedValue, 1, 1))
                Else
                    If strItemType <> UCase(Mid(cboCategory.SelectedValue, 1, 1)) Then
                        strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". PR has item(s) which have a mixture of OPEX and CAPEX category. Please make sure the PR has either CAPEX or OPEX item only and not both.<ul type='disc'></ul></li>"
                        validateDatagrid = False
                    End If
                End If
                If Mid(cboCategory.SelectedValue, 1, 1) = "C" Then
                    cboDelivery = dgItem.FindControl("cboDelivery")
                    strDelivery = cboDelivery.SelectedValue
                    cboBudget = dgItem.FindControl("cboBudget")
                    strBudgetIndex = cboBudget.SelectedValue
                    strSQL = "Select * FROM LOC_MAPPING, ACCOUNT_MSTR where LM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' "
                    strSQL &= "and LM_ADDR_CODE = '" & strDelivery & "' "
                    strSQL &= "and LM_ACCT_CODE = AM_ACCT_CODE and AM_ACCT_INDEX = " & strBudgetIndex
                    If objDB.Exist(strSQL) <> 1 Then
                        strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Item Line has a mis-match of delivery address. Please select the correct delivery address for the selected budget account.<ul type='disc'></ul></li>"
                        validateDatagrid = False
                    End If
                End If
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

            Dim checkempty As String
            If ViewState("type") = "new" Then
                Select Case ViewState("mode")
                    Case "po"
                        If ViewState("type") = "new" Then
                            Select Case ViewState("mode")
                                Case "po"
                                Case "rfq"
                                    getQuo = objPO1.get_PO_Quo(ViewState("poid"))
                                    If getQuo.Tables(0).Rows.Count > 0 Then
                                        Quo = Common.parseNull(getQuo.Tables(0).Rows(0)("pom_quotation_no"))
                                        If Quo <> "" Then
                                            GoTo ContinueLoop
                                        End If
                                    End If
                            End Select
                        ElseIf ViewState("type") = "mod" Then
                            Select Case ViewState("mode")
                                Case "po" ', "cc"
                                    getQuo = objPO1.get_PO_Quo(ViewState("poid"))
                                    If getQuo.Tables(0).Rows.Count > 0 Then
                                        Quo = Common.parseNull(getQuo.Tables(0).Rows(0)("pom_quotation_no"))
                                        If Quo <> "" Then
                                            GoTo ContinueLoop
                                        End If
                                    End If
                                Case "rfq"
                                    getQuo = objPO1.get_PO_Quo(ViewState("poid"))
                                    If getQuo.Tables(0).Rows.Count > 0 Then
                                        Quo = Common.parseNull(getQuo.Tables(0).Rows(0)("pom_quotation_no"))
                                        If Quo <> "" Then
                                            GoTo ContinueLoop
                                        End If
                                    End If
                            End Select
                        End If

                        'Dim checkempty As String
                        checkempty = Trim(CType(dgItem.FindControl("lblProductCode"), Label).Text)
                        If checkempty = "" Then
                            ProdCode = ""
                        Else
                            ProdCode = CInt(CType(dgItem.FindControl("lblProductCode"), Label).Text)
                        End If
                        If ProdCode <> "" And ViewState("modePR") <> "pr" Then
                            Dim objGlobal2 As New AppGlobals
                            PrefDS = objGlobal2.GetVenPrefViaProductCode(ProdCode)
                            If PrefDS.Tables.Count > 0 Then
                                strCPrefer = Common.parseNull(PrefDS.Tables(0).Rows(0)("pm_PREFER_S_COY_ID"))
                                strC1st = Common.parseNull(PrefDS.Tables(0).Rows(0)("pm_1ST_S_COY_ID"))
                                strC2nd = Common.parseNull(PrefDS.Tables(0).Rows(0)("pm_2ND_S_COY_ID"))
                                strC3rd = Common.parseNull(PrefDS.Tables(0).Rows(0)("pm_3RD_S_COY_ID"))
                                CVendor = strCPrefer + "_" + strC1st + "_" + strC2nd + "_" + strC3rd
                                If strCPrefer <> "" Then
                                    GVendor = InStr(1, CVendor, strPPrefer, CompareMethod.Text)
                                    If GVendor = 0 Then
                                        strMsg &= "<li> Item " & dgItem.Cells(EnumShoppingCart.icNo).Text & " is not from selected vendor.<ul type='disc'></ul></li>"
                                        'txtQ.Text = "?"
                                        validateDatagrid = False
                                    End If
                                End If
                            End If
                        End If
                    Case "cc"


                        checkempty = Trim(CType(dgItem.FindControl("lblProductCode"), Label).Text)
                        If checkempty = "" Then
                            ProdCode = ""
                        Else
                            ProdCode = CInt(CType(dgItem.FindControl("lblProductCode"), Label).Text)
                        End If

                        checkempty = Trim(CType(dgItem.FindControl("lblProductGrp"), Label).Text)
                        If checkempty = "" Then
                            ProdGrp = ""
                        Else
                            ProdGrp = CInt(CType(dgItem.FindControl("lblProductGrp"), Label).Text)
                        End If

                        If ProdCode <> "" Then
                            Dim objGlobal2 As New AppGlobals
                            PrefDS = objGlobal2.GetVenViaProductCode(ProdCode, ProdGrp)
                            If PrefDS.Tables.Count > 0 Then
                                strCPrefer = Common.parseNull(PrefDS.Tables(0).Rows(0)("CDM_S_COY_ID"))
                                CVendor = strCPrefer
                                If strCPrefer <> "" Then
                                    GVendor = InStr(1, CVendor, strPPrefer, CompareMethod.Text)
                                    If GVendor = 0 Then
                                        strMsg &= "<li> Item " & dgItem.Cells(EnumShoppingCart.icNo).Text & " is not from selected vendor.<ul type='disc'></ul></li>"
                                        'txtQ.Text = "?"
                                        validateDatagrid = False
                                    End If
                                End If
                            End If
                        End If
                    Case "rfq"
                End Select
            ElseIf ViewState("type") = "mod" Then
                If ViewState("type") = "new" Then
                    Select Case ViewState("mode")
                        Case "po"
                        Case "rfq"
                            getQuo = objPO1.get_PO_Quo(ViewState("poid"))
                            If getQuo.Tables(0).Rows.Count > 0 Then
                                Quo = Common.parseNull(getQuo.Tables(0).Rows(0)("pom_quotation_no"))
                                If Quo <> "" Then
                                    GoTo ContinueLoop
                                End If
                            End If
                    End Select
                ElseIf ViewState("type") = "mod" Then
                    Select Case ViewState("mode")
                        Case "po" ', "cc"
                            getQuo = objPO1.get_PO_Quo(ViewState("poid"))
                            If getQuo.Tables(0).Rows.Count > 0 Then
                                Quo = Common.parseNull(getQuo.Tables(0).Rows(0)("pom_quotation_no"))
                                If Quo <> "" Then
                                    GoTo ContinueLoop
                                End If
                            End If
                        Case "rfq"
                            getQuo = objPO1.get_PO_Quo(ViewState("poid"))
                            If getQuo.Tables(0).Rows.Count > 0 Then
                                Quo = Common.parseNull(getQuo.Tables(0).Rows(0)("pom_quotation_no"))
                                If Quo <> "" Then
                                    GoTo ContinueLoop
                                End If
                            End If
                    End Select
                End If

                checkempty = Trim(CType(dgItem.FindControl("lblProductCode"), Label).Text)
                If checkempty = "" Then
                    ProdCode = ""
                Else
                    ProdCode = CInt(CType(dgItem.FindControl("lblProductCode"), Label).Text)
                End If

                checkempty = Trim(CType(dgItem.FindControl("lblProductGrp"), Label).Text)
                If checkempty = "" Then
                    ProdGrp = ""
                Else
                    ProdGrp = CType(dgItem.FindControl("lblProductGrp"), Label).Text
                End If

                Select Case ViewState("mode")
                    Case "cc"
                        If ProdCode <> "" Then
                            Dim objGlobal2 As New AppGlobals
                            PrefDS = objGlobal2.GetVenViaProductCode(ProdCode, ProdGrp)
                            If PrefDS.Tables.Count > 0 Then
                                strCPrefer = Common.parseNull(PrefDS.Tables(0).Rows(0)("CDM_S_COY_ID"))
                                CVendor = strCPrefer
                                If strCPrefer <> "" Then
                                    GVendor = InStr(1, CVendor, strPPrefer, CompareMethod.Text)
                                    If GVendor = 0 Then
                                        strMsg &= "<li> Item " & dgItem.Cells(EnumShoppingCart.icNo).Text & " is not from selected vendor.<ul type='disc'></ul></li>"
                                        'txtQ.Text = "?"
                                        validateDatagrid = False
                                    End If
                                End If
                            End If
                        End If
                    Case "po", "rfq"
                        If ProdCode <> "" And ViewState("modePR") <> "pr" Then
                            Dim objGlobal2 As New AppGlobals
                            PrefDS = objGlobal2.GetVenPrefViaProductCode(ProdCode)
                            If PrefDS.Tables.Count > 0 Then
                                strCPrefer = Common.parseNull(PrefDS.Tables(0).Rows(0)("pm_PREFER_S_COY_ID"))
                                strC1st = Common.parseNull(PrefDS.Tables(0).Rows(0)("pm_1ST_S_COY_ID"))
                                strC2nd = Common.parseNull(PrefDS.Tables(0).Rows(0)("pm_2ND_S_COY_ID"))
                                strC3rd = Common.parseNull(PrefDS.Tables(0).Rows(0)("pm_3RD_S_COY_ID"))
                                CVendor = strCPrefer + "_" + strC1st + "_" + strC2nd + "_" + strC3rd
                                If strCPrefer <> "" Then
                                    GVendor = InStr(1, CVendor, strPPrefer, CompareMethod.Text)
                                    If GVendor = 0 Then
                                        strMsg &= "<li> Item " & dgItem.Cells(EnumShoppingCart.icNo).Text & " is not from selected vendor.<ul type='disc'></ul></li>"
                                        'txtQ.Text = "?"
                                        validateDatagrid = False
                                    End If
                                End If
                            End If
                        End If
                End Select
            End If
ContinueLoop: Next
        strMsg &= "</ul>"
    End Function


    'Private Sub cmdDupPOLine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDupPOLine.Click
    '    Dim dgItem As DataGridItem
    '    Dim objPR As New PR
    '    'Dim objPR1 As New PurchaseReq2
    '    Dim intLine As Integer
    '    Dim chkItem As CheckBox
    '    Dim i As Integer = 0
    '    Dim strAryQuery(0) As String
    '    Dim strSQL, strMsg As String

    '    For Each dgItem In dtgShopping.Items
    '        chkItem = dgItem.FindControl("chkSelection")
    '        If chkItem.Checked Then
    '            intLine = CInt(CType(dgItem.FindControl("lblItemLine"), Label).Text)
    '            strMsg = objPR.DupPRItem(hidNewPO.Value, intLine)
    '            Exit For
    '        End If
    '    Next

    '    If strMsg = "1" Then
    '        'hidCost.Value = objPR.updatePOCost(hidNewPO.Value)
    '        Common.NetMsgbox(Me, "Item(s) Duplicated.", MsgBoxStyle.Information)
    '        Dim dsDelivery As New DataSet
    '        Dim objAdmin As New Admin
    '        Dim objBudget As New BudgetControl
    '        dsDelivery = objAdmin.PopulateAddr("D", "", "", "")
    '        dvwDelivery = dsDelivery.Tables(0).DefaultView
    '        strDeliveryDefault = objAdmin.user_Default_Add("D")

    '        Dim dsBCM As New DataSet
    '        dsBCM = objBudget.getBCMListByUser(Session("UserId"))
    '        dtBCM = dsBCM.Tables(0)
    '        Bindgrid()
    '        hidTotal.Value = ""
    '        'addDGTotal()
    '        hid2.Value = ""
    '        objAdmin = Nothing
    '        objBudget = Nothing
    '    End If
    '    objPR = Nothing
    'End Sub

    Private Sub cmdDupPOLine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDupPOLine.Click
        Dim dgItem As DataGridItem
        Dim objPO As New PurchaseOrder_Buyer
        Dim intLine As Integer
        Dim chkItem As CheckBox
        Dim i As Integer = 0
        Dim strAryQuery(0) As String
        Dim strSQL, strMsg As String
        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        Dim strName As String

        If ViewState("type") = "new" Then

            Select Case ViewState("mode")
                Case "cc"
                    For Each dgItem In dtgShopping.Items
                        chkItem = dgItem.FindControl("chkSelection")
                        If chkItem.Checked Then

                            Dim aryProdCodeCurrent As ArrayList = Session("ProdList")
                            aryProdCodeCurrent.Add(New String() {dgItem.Cells(EnumShoppingCart.icProductCode).Text, CType(dgItem.FindControl("lblProductDesc"), Label).Text, "", "", CType(dgItem.FindControl("lblProductGrp"), Label).Text})
                            Session("ProdList") = aryProdCodeCurrent

                            'intLine = CInt(CType(dgItem.FindControl("lblItemLine"), Label).Text)
                            'strMsg = objPO.DupPOItem(hidNewPO.Value, intLine)
                            Exit For
                        End If
                    Next
                Case "po", "rfq"
                    For Each dgItem In dtgShopping.Items
                        chkItem = dgItem.FindControl("chkSelection")
                        If chkItem.Checked Then

                            Dim aryProdCodeCurrent As ArrayList = Session("ProdList")
                            aryProdCodeCurrent.Add(dgItem.Cells(EnumShoppingCart.icProductCode).Text)
                            Session("ProdList") = aryProdCodeCurrent

                            'intLine = CInt(CType(dgItem.FindControl("lblItemLine"), Label).Text)
                            'strMsg = objPO.DupPOItem(hidNewPO.Value, intLine)
                            Exit For
                        End If
                    Next
            End Select

            'Dim dtrd As DataRow
            'Dim chkItem As CheckBox
            Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark As TextBox
            Dim lblItemLine, txtBudget, txtDelivery As Label
            'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
            Dim cboGLCode, cboCust, cboGSTRate, cboGSTTaxCode As DropDownList
            Dim strItem, strItemCust As New ArrayList()
            'Dim dgItem As DataGridItem
            Dim dgItem2 As DataGridItem
            Dim k As Integer = 0

            For Each dgItem In dtgShopping.Items
                txtQty = dgItem.FindControl("txtQty")
                'cboGLCode = dgItem.FindControl("cboGLCode")
                txtPrice = dgItem.FindControl("txtPrice")
                'txtBudget = dgItem.FindControl("txtBudget")
                'hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                txtDelivery = dgItem.FindControl("txtDelivery")
                hidDelCode = dgItem.FindControl("hidDelCode")
                txtEstDate = dgItem.FindControl("txtEstDate")
                txtWarranty = dgItem.FindControl("txtWarranty")
                txtRemark = dgItem.FindControl("txtRemark")
                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                cboGSTRate = dgItem.FindControl("cboGSTRate")
                cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")

                chkItem = dgItem.FindControl("chkSelection")
                'If chkItem.Checked Then
                'Else
                '    strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text})
                'End If
                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                'strItem.Add(New String() {"", txtQty.Text, txtPrice.Text, "", "", txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
                strItem.Add(New String() {"", txtQty.Text, txtPrice.Text, "", "", txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGSTRate.SelectedIndex, cboGSTTaxCode.SelectedIndex})

                If (ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr") Or ViewState("modeRFQFromPR_Index") <> "" Then
                    Dim objAdmin As New Admin

                    If ViewState("modeRFQFromPR_Index") <> "" Then
                        dvwCus = objAdmin.getCustomFieldPR("", "PR", ViewState("modeRFQFromPR_Index"))
                    Else
                        dvwCus = objAdmin.getCustomField("", "PO")
                    End If

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
                End If

                k += 1
            Next
            Session("strItem") = strItem
            Session("strItemCust") = strItemCust

            Bindgrid()
        ElseIf ViewState("type") = "mod" Then
            Select Case ViewState("mode")
                Case "cc"
                    For Each dgItem In dtgShopping.Items
                        chkItem = dgItem.FindControl("chkSelection")
                        If chkItem.Checked Then
                            Session("CurrentScreen") = "AddItem"
                            Dim aryProdCodeCurrent As ArrayList = Session("ProdList")
                            aryProdCodeCurrent.Add(New String() {dgItem.Cells(EnumShoppingCart.icProductCode).Text, CType(dgItem.FindControl("lblProductDesc"), Label).Text, "", "", CType(dgItem.FindControl("lblProductGrp"), Label).Text})
                            Session("ProdList") = aryProdCodeCurrent

                            'intLine = CInt(CType(dgItem.FindControl("lblItemLine"), Label).Text)
                            'strMsg = objPO.DupPOItem(hidNewPO.Value, intLine)
                            Exit For
                        End If
                    Next
                Case "po", "rfq"

                    For Each dgItem In dtgShopping.Items
                        chkItem = dgItem.FindControl("chkSelection")
                        If chkItem.Checked Then
                            Session("CurrentScreen") = "AddItem"
                            Dim aryProdCodeCurrent As ArrayList = Session("ProdList")
                            aryProdCodeCurrent.Add(dgItem.Cells(EnumShoppingCart.icProductCode).Text)
                            Session("ProdList") = aryProdCodeCurrent

                            'intLine = CInt(CType(dgItem.FindControl("lblItemLine"), Label).Text)
                            'strMsg = objPO.DupPOItem(hidNewPO.Value, intLine)
                            Exit For
                        End If
                    Next
            End Select
            strscript.Append("<script language=""javascript"">")
            strscript.Append("document.getElementById('btnHidden1').click();")
            strscript.Append("</script>")
            RegisterStartupScript("script4", strscript.ToString())
            'btnHidden1_Click(sender, e)
        End If

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
        objPO = Nothing
    End Sub

    Private Sub cboVendor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboVendor.SelectedIndexChanged
        If cboVendor.SelectedIndex <> 0 Then
            GetVendorDetail()
            'Session("CurrentScreen") = "VendorSelect"

            'Dim dtrd As DataRow
            Dim chkItem As CheckBox
            Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark As TextBox
            Dim lblItemLine, txtBudget, txtDelivery As Label
            'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
            Dim cboGLCode, cboCust, cboGSTRate, cboGSTTaxCode As DropDownList
            Dim strItem, strItemCust As New ArrayList()
            Dim dgItem, dgItem2 As DataGridItem
            Dim k As Integer = 0
            'Dim strItemHead As New ArrayList()

            'strItemHead.Add(New String() {txtAttention.Text})

            For Each dgItem In dtgShopping.Items
                txtQty = dgItem.FindControl("txtQty")
                'cboGLCode = dgItem.FindControl("cboGLCode")
                txtPrice = dgItem.FindControl("txtPrice")
                'txtBudget = dgItem.FindControl("txtBudget")
                'hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                txtDelivery = dgItem.FindControl("txtDelivery")
                hidDelCode = dgItem.FindControl("hidDelCode")
                txtEstDate = dgItem.FindControl("txtEstDate")
                txtWarranty = dgItem.FindControl("txtWarranty")
                txtRemark = dgItem.FindControl("txtRemark")
                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                cboGSTRate = dgItem.FindControl("cboGSTRate")
                cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")

                chkItem = dgItem.FindControl("chkSelection")
                'If chkItem.Checked Then
                'Else
                '    strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text})
                'End If
                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                'strItem.Add(New String() {"", txtQty.Text, txtPrice.Text, "", "", txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})
                strItem.Add(New String() {"", txtQty.Text, txtPrice.Text, "", "", txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboGSTRate.SelectedIndex, cboGSTTaxCode.SelectedIndex})

                If (ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr") Or ViewState("modeRFQFromPR_Index") <> "" Then
                    Dim objAdmin As New Admin

                    If ViewState("modeRFQFromPR_Index") <> "" Then
                        dvwCus = objAdmin.getCustomFieldPR("", "PR", ViewState("modeRFQFromPR_Index"))
                    Else
                        dvwCus = objAdmin.getCustomField("", "PO")
                    End If

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
                End If

                k += 1
            Next
            Session("strItem") = strItem
            Session("strItemCust") = strItemCust
            'Session("strItemHead") = strItemHead

            Bindgrid()
        End If

    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Session("w_AddPO_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn_selected"" href=""POViewB2.aspx?pageid=" & strPageId & """><span>Purchase Order</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""POViewB2Cancel.aspx?type=Listing&pageid=" & strPageId & """><span>Purchase Order Cancellation</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '             "</ul><div></div></div>"      
        Session("w_AddPO_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB2Cancel.aspx", "type=Listing&pageid=" & strPageId) & """><span>Purchase Order Cancellation</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
    End Sub

    'Private Sub cboDelivery_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboDelivery.SelectedIndexChanged
    '    strDefDelivery = Mid(cboDelivery.SelectedValue, 1, 10)
    'End Sub

    Protected Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click

        Dim dsPO As New DataSet
        Dim objPO As New PurchaseOrder
        Dim objPO1 As New PurchaseOrder_Buyer
        Dim strMsg As String = ""

        Dim dt As New DataTable

        blnValid = True
        blnItem = False
        If validateDatagrid(strMsg) Then
            dsPO = bindPO()
            If ViewState("type") = "new" Then
                Dim strNewPO As String
                Dim intMsg As Integer

                If blnValid = True Then
                    If ViewState("blnBill") <> 0 And cboBillCode.SelectedIndex = 0 And ViewState("modePR") <> "pr" Then
                        lblMsg.Text = "<ul type='disc'><li>Bill To is required.<ul type='disc'></ul></li></ul>"
                    Else
                        lblMsg.Text = ""

                        'If Session("Env") = "FTN" Then
                        '    dt = objPO1.getPOApprFlow(True)
                        'Else
                        '    dt = objPO1.getPOApprFlow(False)
                        'End If
                        dt = objPO1.getPOApprFlow(True)

                        Dim CheckApprB4 As Boolean = True
                        If dt.Rows.Count = 0 Then
                            CheckApprB4 = False
                        End If

                        'If Not IsNumeric(Mid(lblPONo.Text, 3)) Then
                        If lblPONo.Text = "To Be Allocated By System" And CheckApprB4 = True Then
                            intMsg = objPO1.insertPO(dsPO, strNewPO, False, False)
                        Else

                            'If Session("Env") = "FTN" Then
                            '    dt = objPO1.getPOApprFlow(True)
                            'Else
                            '    dt = objPO1.getPOApprFlow(False)
                            'End If
                            dt = objPO1.getPOApprFlow(True)

                            If dt.Rows.Count = 0 Then
                                Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                Exit Sub
                            ElseIf dt.Rows.Count > 1 Then
                                '      Common.NetMsgbox(Me, "Please proceed to Approval Setup to select the Approval Flow.", MsgBoxStyle.Information)
                                Response.Redirect(dDispatcher.direct("PO", "POApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&type=" & ViewState("type") & "&poid=" & lblPONo.Text & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.00") & "&Frm=" & Me.Request.QueryString("Frm") & "&dpage=" & Request.QueryString("dpage") & "&currency=" & ViewState("Currency")))
                                Exit Sub
                            Else
                                objPO1.updatePO(dsPO, False, False)
                                intMsg = WheelMsgNum.Save
                                strNewPO = lblPONo.Text
                            End If
                        End If
                        objPO = Nothing
                        'redirect to ExceedBCM before approval page
                        Select Case intMsg
                            Case WheelMsgNum.Save
                                If blnItem Then ' item exists
                                    'If Session("Env") <> "FTN" Then
                                    '    If ViewState("BCM") > 0 Then
                                    '        If checkMandatory(strMsg) Then
                                    '            Response.Redirect(dDispatcher.direct("PR", "ExceedBCM.aspx", "pageid=" & strPageId & "&prid=" & strNewPO & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                    '        Else
                                    '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                                    '        End If
                                    '    Else
                                    '        'Response.Redirect("ApprovalSetup.aspx?pageid=" & strPageId & "&msg=0&prid=" & strNewPO & "&pocost=" & Format((CDbl(ViewState("POCost")) * CDbl(lblRate.Text)), "#0.00") & "&currency=" & ViewState("Currency"))
                                    '        Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & strNewPO & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                    '    End If
                                    'Else
                                    '    'Response.Redirect("ApprovalSetup.aspx?pageid=" & strPageId & "&msg=0&prid=" & strNewPO & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.00") & "&currency=" & ViewState("Currency"))
                                    '    lblPONo.Text = strNewPO
                                    '    'cmdRaise.Visible = "False"
                                    '    SubmitPO()
                                    'End If
                                    lblPONo.Text = strNewPO
                                    SubmitPO()
                                Else
                                    Common.NetMsgbox(Me, "There are no items in this PO.", MsgBoxStyle.Information)
                                End If

                            Case WheelMsgNum.NotSave
                                Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                            Case WheelMsgNum.Duplicate
                                Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)
                            Case -1
                                'Common.NetMsgbox(Me, lblSupplier.Text & " company is currently inactive.", MsgBoxStyle.Information)
                                Common.NetMsgbox(Me, "Company is currently inactive.", MsgBoxStyle.Information)
                            Case -2
                                'Common.NetMsgbox(Me, lblSupplier.Text & " company is being deleted.", MsgBoxStyle.Information)
                                Common.NetMsgbox(Me, "Company is being deleted.", MsgBoxStyle.Information)
                        End Select
                    End If
                End If
                Select Case ViewState("mode")
                    Case "po"
                    Case "rfq"
                End Select
            ElseIf ViewState("type") = "mod" Then
                If blnValid = True Then
                    If ViewState("blnBill") <> 0 And cboBillCode.SelectedItem.Value = "" And ViewState("modePR") <> "pr" Then
                        lblMsg.Text = "<ul type='disc'><li>Bill To is required.<ul type='disc'></ul></li></ul>"
                    Else
                        lblMsg.Text = ""
                        'objPR.updatePR(dsPR)
                        'objPR = Nothing
                        If blnItem Then ' item exists
                            'If Session("Env") <> "FTN" Then
                            '    If ViewState("BCM") > 0 Then
                            '        If checkMandatory(strMsg) Then

                            '            If ViewState("modePR") <> "pr" Then
                            '                'If ViewState("ListingFromRFQ") <> "True" And ViewState("mode") <> "rfq" Then
                            '                If ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr" Then
                            '                    objPO1.updatePO(dsPO, True)
                            '                Else
                            '                    objPO1.updatePO(dsPO, False)
                            '                End If
                            '                'objPO1.updatePO(dsPO, True)
                            '            End If

                            '            Response.Redirect(dDispatcher.direct("PR", "ExceedBCM.aspx", "prid=" & hidNewPO.Value & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId))
                            '        Else
                            '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                            '        End If
                            '    Else
                            '        Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "msg=0&prid=" & hidNewPO.Value & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId))
                            '    End If
                            'Else
                            '    'Response.Redirect("ApprovalSetup.aspx?msg=0&prid=" & hidNewPO.Value & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.00") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId)
                            '    If ViewState("modePR") <> "pr" Then
                            '        objPO1.updatePO(dsPO, False)
                            '    End If
                            '    SubmitPO()
                            'End If
                            'If ViewState("modePR") <> "pr" Then
                            '    objPO1.updatePO(dsPO, False, False)
                            'End If
                            If ViewState("modePR") <> "pr" Then
                                'If ViewState("ListingFromRFQ") <> "True" And ViewState("mode") <> "rfq" Then
                                If ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr" Then
                                    objPO1.updatePO(dsPO, True, False)
                                Else
                                    objPO1.updatePO(dsPO, False, False)
                                End If
                            Else
                                objPO1.updatePO(dsPO, False, False)
                                'objPO1.updatePO(dsPO, True)
                            End If
                            SubmitPO()
                        Else
                            Common.NetMsgbox(Me, "There are no items in this PR.", MsgBoxStyle.Information)
                        End If
                    End If
                End If
            End If
        Else
            If strMsg <> "" Then
                lblMsg.Text = strMsg
            Else
                lblMsg.Text = ""
            End If
        End If
    End Sub

    Public Sub btnHidden1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden1.Click

    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        Dim strName As String

        Select Case ViewState("mode")
            Case "cc"
                strscript.Append("<script language=""javascript"">")
                'strFileName = dDispatcher.direct("PR", "Add_Buyer_Catalogue_Item.aspx", "pageid=" & strPageId & "&RFQ_Name=&RFQ_Cur_value=&RFQ_Cur_Text=")
                strFileName = dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId & "&frm=Concat&show=no")
                strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
                strscript.Append("ShowDialog('" & dDispatcher.direct("RFQ", "Dialog.aspx", "page=" & strFileName) & "','560px');")
                strscript.Append("document.getElementById('btnHidden1').click();")
                strscript.Append("</script>")
                RegisterStartupScript("script13", strscript.ToString())
            Case "po", "rfq"
                Session("POAddItem") = "CloseDirect"
                'strName = "selVendor" & cboVendor.SelectedValue & "&Raise=" & "RaisePO"
                'Michelle (14/5/2011) - to add in '='
                strName = "selVendor=" & cboVendor.SelectedValue & "&Raise=" & "RaisePO"

                strscript.Append("<script language=""javascript"">")
                strFileName = dDispatcher.direct("Search", "BuyerCatalogueSearchPopup.aspx", strName)
                strFileName = Server.UrlEncode(strFileName)
                strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','580px');")
                strscript.Append("document.getElementById('btnHidden1').click();")
                strscript.Append("</script>")
                RegisterStartupScript("script3", strscript.ToString())
        End Select




    End Sub
End Class
