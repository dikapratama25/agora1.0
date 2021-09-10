Imports AgoraLegacy
Imports AgoraLegacyAppGlobals
Imports eProcure.Component
Imports System.Text.RegularExpressions

Imports System.Drawing
Imports SSO
Public Class RaiseFFPO
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim keepAr As New ArrayList
    Dim keepArPost As New ArrayList
    Dim aryProdCodeNew_All As New ArrayList
    Dim PO_Type As String = ""
    Dim strIsGst As String = ""
    Dim clsGST As GST
    Dim boolchkGSTCod As Boolean

    Public Enum EnumShoppingCart
        icChk = 0
        icNo = 1
        icProductCode = 2
        icVendorItemCode = 3

        'Jules 2018.07.14 - Added Gift & Analysis Codes
        icGift = 4
        icGift1 = 5
        icFundType = 6
        icPersonCode = 7
        icProjectCode = 8
        icGLCode = 9 ' 4
        icGLCode1 = 10 '5
        icCategoryCode1 = 11 '6
        icTaxCode = 12 '7
        icAssetGroup = 13 '8
        icItemDesc = 14 '9
        icMOQ = 15 '10
        icMPQ = 16 ' 11
        icRfqQty = 17 '12
        icTolerance = 18 '13
        icQty = 19 '14
        icUOM = 20 '15
        icGST = 21 '16
        icGSTValue = 22 '17
        icPrice = 23 '18
        icTotal = 24 '19
        icTax = 25 '20
        icGstTaxCode = 26 '21 'Stage 3 Enhancement (GST-0010) - 14/07/2015 - CH
        icSource = 27 '22 '21
        icCategoryCode = 28 '23 '22
        icBudget = 29 '24 '23
        icDelivery = 30 '25 '24
        icEstDate = 31 '26 '25
        icWarranty = 32 '27 '26
        icRemark = 33 '28 '27
        'End modification.
    End Enum

    Dim dvwDelivery, dvwAssetGroup, dvwGLCode, dvwCategoryCode As DataView
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
    Dim gstReg As String = ""
    Dim i As Integer
    Dim intCount As Integer
    Dim strIndexList As String
    Dim blnValid As Boolean
    Dim strDefDelivery As String
    Dim strQtyErr As String
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
    Protected WithEvents lblTax As System.Web.UI.WebControls.Label
    Protected WithEvents pnlAttach As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlAttachInt As System.Web.UI.WebControls.Panel
    Protected WithEvents chkCustomPR As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkRemarkPR As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkUrgent As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdRemove As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdUploadPO As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents File1 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents File1Int As System.Web.UI.HtmlControls.HtmlInputFile
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
    Protected WithEvents hidTaxCalcBy As System.Web.UI.WebControls.HiddenField
    Protected WithEvents hidGSTCode As System.Web.UI.WebControls.HiddenField
    Protected WithEvents hidIsGST As System.Web.UI.WebControls.HiddenField
    Protected WithEvents hidexceedCutOffDt As System.Web.UI.WebControls.HiddenField
    Protected WithEvents hidBillingMethod As System.Web.UI.WebControls.HiddenField
    Protected WithEvents Hidden6 As System.Web.UI.WebControls.HiddenField
    Protected WithEvents lblDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblSupplier As System.Web.UI.WebControls.Label
    Protected WithEvents txtRequestedName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFreightTerm As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRequestedContact As System.Web.UI.WebControls.TextBox
    Protected WithEvents cboPayTerm As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboPayMethod As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboShipmentTerm As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboShipmentMode As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtVendor As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtShipVia As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblCurrencyCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAttention As System.Web.UI.WebControls.TextBox
    Protected WithEvents cboBillCode As System.Web.UI.WebControls.DropDownList
    Protected WithEvents hidDelCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBillAdd1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBillAdd2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBillAdd3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtInternal As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtExternal As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdUpload As System.Web.UI.WebControls.Button
    Protected WithEvents cmdUploadInt As System.Web.UI.WebControls.Button
    Protected WithEvents btnGetAdd As System.Web.UI.WebControls.Button
    Protected WithEvents dtgShopping As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdRaise As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDupPOLine As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSetup As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents hidClientId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidTotalClientId As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents hidOneVendor As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtShippingHandling As System.Web.UI.WebControls.TextBox

    Protected WithEvents divHide As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents btnHidden2 As System.Web.UI.WebControls.Button 'Jules 2018.08.03
    Protected WithEvents ddlGST As System.Web.UI.WebControls.DropDownList 'Jules 2018.10.07
    Dim cmdDelivery As HtmlInputButton

    Dim kk As Integer = 0
    Dim dtFundType, dtPersonCode, dtProjectCode As DataTable 'Jules 2018.10.18
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
            If ViewState("modePR") = "pr" Then
                Me.addDataGridColumn(True)
            Else
                Me.addDataGridColumn(False)
            End If
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
        alButtonList.Add(cmdDupPOLine)
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
        cmdDupPOLine.Enabled = blnCanUpdate And ViewState("blnCmdRemove")
        cmdDelete.Enabled = blnCanDelete And ViewState("blnCmdDelete")
        btnGetAdd.Enabled = True 'Oct 2
        alButtonList.Clear()

        If ViewState("modePR") = "pr" Then
            cmdRaise.Enabled = False
        End If
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)

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

        'Jules 2018.10.18
        Dim objAC As New IPP
        Dim dsFundType, dsPersonCode, dsProjectCode, dsGLCode As New DataSet
        dsFundType = objAC.GetAnalysisCode("", "", "L1", "O", "eProcure")

        dtFundType = New DataTable
        dtFundType.Columns.Add("AC_ANALYSIS_CODE", Type.GetType("System.String"))
        dtFundType.Columns.Add("AC_ANALYSIS_CODE_DESC", Type.GetType("System.String"))
        dtFundType.Columns.Add("AC_DEPT_CODE", Type.GetType("System.String"))
        dtFundType.Columns.Add("AC_STATUS", Type.GetType("System.String"))
        dtFundType = dsFundType.Tables(0)

        dsPersonCode = objAC.GetAnalysisCode("", "", "L9", "O", "eProcure")

        dtPersonCode = New DataTable
        dtPersonCode.Columns.Add("AC_ANALYSIS_CODE", Type.GetType("System.String"))
        dtPersonCode.Columns.Add("AC_ANALYSIS_CODE_DESC", Type.GetType("System.String"))
        dtPersonCode.Columns.Add("AC_DEPT_CODE", Type.GetType("System.String"))
        dtPersonCode.Columns.Add("AC_STATUS", Type.GetType("System.String"))
        dtPersonCode = dsPersonCode.Tables(0)
        dsProjectCode = objAC.GetAnalysisCode("", "", "L8", "O", "eProcure")

        dtProjectCode = New DataTable
        dtProjectCode.Columns.Add("AC_ANALYSIS_CODE", Type.GetType("System.String"))
        dtProjectCode.Columns.Add("AC_ANALYSIS_CODE_DESC", Type.GetType("System.String"))
        dtProjectCode.Columns.Add("AC_DEPT_CODE", Type.GetType("System.String"))
        dtProjectCode.Columns.Add("AC_STATUS", Type.GetType("System.String"))
        dtProjectCode = dsProjectCode.Tables(0)

        Dim strSQL As String
        strSQL = "Select CBG_B_GL_CODE as 'GLCODE',  " &
                    objDB.Concat("", "", "CBG_B_GL_DESC", objDB.Concat(" (", ")", "CBG_B_GL_CODE")) & " as DESCRIPTION " &
                    " from COMPANY_B_GL_CODE where CBG_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' order by  CBG_B_GL_CODE "

        dsGLCode = objDB.FillDs(strSQL)
        If dsGLCode IsNot Nothing Then
            dtGLCode = New DataTable
            dtGLCode.Columns.Add("GLCODE", Type.GetType("System.String"))
            dtGLCode.Columns.Add("DESCRIPTION", Type.GetType("System.String"))
            dtGLCode = dsGLCode.Tables(0)
        End If
        'End modification.


        Dim objPR As New PR
        Dim objBudget As New BudgetControl

        If Session("CurrentScreen") <> "RemoveItem" And Session("CurrentScreen") <> "POBatchUpload" Then
            Session("strItem") = Nothing
            Session("strItemCust") = Nothing
            Session("strItemHead") = Nothing
        End If
        If (Session("CurrentScreen") = "RemoveItem" Or Session("CurrentScreen") = "POBatchUpload") Then
        Else
            If ViewState("_isDuplicated") Is Nothing Then
                ViewState("strDuplicatedItem") = Nothing
                Session("strItem") = Nothing
                Session("strItemCust") = Nothing
                ViewState("strDuplicatedCustItem") = Nothing
                Session("aryProdCodeNew_All") = Nothing
            End If
        End If
        'If ViewState("_isDuplicated") Is Nothing And Session("CurrentScreen") <> "RemoveItem" Then
        '    ViewState("strDuplicatedItem") = Nothing
        '    Session("strItem") = Nothing
        '    Session("strItemCust") = Nothing
        '    ViewState("strDuplicatedCustItem") = Nothing
        '    Session("aryProdCodeNew_All") = Nothing
        'End If
        If Not Session("ItemDeleted") Is Nothing Then
            If Session("ItemDeleted") = "Yes" Then
                Common.NetMsgbox(Me, "Item(s) has been removed.", MsgBoxStyle.Information)
                Session("aryProdCodeNew_All") = Nothing
            End If
        End If

        If Not IsPostBack Then

            If Session("ItemDeleted") Is Nothing Then
                Session("ProdList") = Nothing
            End If

            Session("strPutOnce") = ""
            strQtyErr = objGlobal.GetErrorMessage("00342")
            ViewState("ValQtyMsg") = strQtyErr


            GenerateTab()
            'Construct URL for remove / delete
            If Request.QueryString("frm") = "PC" Then
                'Construct URL For PC
                Session("RaisePOURL") = dDispatcher.direct("PO", "RaisePO.aspx", "frm=PC&pageid=" & strPageId)
            Else
                'Construct URL For My PO
                Session("RaisePOURL") = dDispatcher.direct("PO", "RaisePO.aspx", "pageid=" & strPageId & "&index=" & Request.QueryString("index") & "&poid=" & Request.QueryString("poid") & "&type=" & Request.QueryString("type") & "&mode=" & Request.QueryString("mode"))
                If Session("ItemDeleted") Is Nothing Then
                    Session("ProdList") = Nothing
                    Session("CurrentScreen") = ""
                End If
            End If

            ViewState("blnCmdDelete") = True
            If Request.QueryString("type") = "mod" Then
                ViewState("type") = Request.QueryString("type")
            Else
                ViewState("type") = "new"
            End If

            If Not Session("NewPoInfo") Is Nothing Then
                ViewState("type") = Session("NewPoInfo").ToString.Split("|")(1)
            End If

            ViewState("mode") = "po"

            ViewState("strPageId") = strPageId
            ViewState("blnCmdAdd") = True

            ViewState("blnCmdRaise") = True
            ViewState("blnCmdUpload") = True
            ViewState("blnCmdUploadInt") = True
            ViewState("blnCmdRemove") = True
            ViewState("blnCmdSetup") = True
            ViewState("postback") = 0
            ViewState("SuppId") = ""
            Dim objAdmin As New Admin
            Dim dsDelivery As New DataSet
            Dim dsBill As New DataSet

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
            PopulateVenTypeAhead()
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

            Dim strGST As String
            cmdRaise.Attributes.Add("onclick", "return InitialValidation();")
            cmdSubmit.Attributes.Add("onclick", "return InitialValidation();")

            divHide.Style("display") = ""
            If ViewState("type") = "new" Then

                cmdDelete.Visible = False
                ViewState("Vendor") = Request.QueryString("vendor")
                lblTitle.Text = "Raise Free Form Purchase Order"
                lblPONo.Text = "To Be Allocated By System"
                Common.SelDdl(objAdmin.user_Default_Add("B"), cboBillCode, True, True)
                fillAddress()
                enableBill(False)

                ' assign data to each textbox/label
                lblDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Today.Date)
                Dim dsCurrency As New DataSet

                'BindVendorList()

                If ViewState("strGST") = "" Then
                    ViewState("strGST") = "0"
                End If
                strIndexList = Session("ItemIndexList")
                Session.Remove("ItemIndexList")
                chkCustomPR.Checked = True
                chkRemarkPR.Checked = True

                If Session("urlreferer") = "" Then
                    lnkBack.NavigateUrl = dDispatcher.direct("PO", "viewShoppingCart.aspx", "type=tab&pageid=" & strPageId)
                ElseIf Session("urlreferer") = "ConCatSearch" Then
                    lnkBack.NavigateUrl = dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId)
                ElseIf Session("urlreferer") = "POViewB2" Then
                    lnkBack.NavigateUrl = dDispatcher.direct("PO", Session("urlreferer") & ".aspx", "type=tab&pageid=" & strPageId)
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

                'Jules 2019.03.25 - To avoid wrongly attaching files from other PO(s) in the same session.
                Dim objPOB As New PurchaseOrder_Buyer
                objPOB.delete_Attachment2(Session.SessionID)

            ElseIf ViewState("type") = "mod" Then
                If Session("urlreferer") = "" Then
                    lnkBack.NavigateUrl = dDispatcher.direct("PO", "SearchPR_All.aspx", "caller=buyer&pageid=" & strPageId)
                    If Me.Request.QueryString("Frm") = "Dashboard" Then
                        lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
                        Session("urlreferer") = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
                    End If
                Else
                    If Me.Request.QueryString("Frm") = "Dashboard" Then
                        lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
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

                If Not Session("NewPoInfo") Is Nothing Then
                    ViewState("poid") = Session("NewPoInfo").ToString.Split("|")(0)
                End If

                lblPONo.Text = ViewState("poid")
                hidNewPO.Value = ViewState("poid")
                lblTitle.Text = "Raise Free Form Purchase Order"

                If IsNumeric(ViewState("POM_RFQ_INDEX")) Then
                    Session("ProdList") = aryProdCodeNew
                    hidOneVendor.Value = 1
                ElseIf Session("ItemDeleted") Is Nothing Then
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
                        ViewState("rfqnum") = strRFQ_No
                        ViewState("modeRFQFromPR_Index_draft") = objDB.GetVal("SELECT PRM_PR_INDEX FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO = '" & objDB.GetVal("SELECT PRD_PR_NO FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & strRFQ_No & "'") & "'")

                        Dim strUrgent As String = objDB.GetVal("SELECT PRM_URGENT FROM PR_DETAILS, PR_MSTR WHERE PRD_PR_NO = PRM_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "' LIMIT 1")
                        If strUrgent = "1" Then
                            chkUrgent.Checked = True
                        Else
                            chkUrgent.Checked = False
                        End If

                    Else
                        ViewState("modeRFQFromPR_Index_draft") = ""
                    End If
                End If

                PO_Type = objDB.GetVal("SELECT IFNULL(POD_SOURCE,'') FROM PO_DETAILS WHERE pod_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND POD_PO_NO = '" & ViewState("poid") & "'")
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

            ViewState("GST") = "subtotal"

            Dim dsBCM As New DataSet
            ViewState("BCM") = CInt(objPR.checkBCM)
            If ViewState("BCM") > 0 Then
                If ViewState("modePR") = "pr" Then
                    dtBCM = objBudget.getBCMListByCompanyNew()
                Else
                    dsBCM = objBudget.getBCMListByUserNew(Session("UserId"), "")
                    dtBCM = dsBCM.Tables(0)
                End If
            End If

            Dim objPO As New PurchaseOrder
            Dim objPO1 As New PurchaseOrder_Buyer
            If objPO.isConvertedFromRFQ(ViewState("poid")) Then
                ViewState("ListingFromRFQ") = "True"
            End If

            If ViewState("modePR") = "pr" Then
                addDataGridColumn(True)
                chkCustomPR.Enabled = False
                chkRemarkPR.Enabled = False
            Else
                addDataGridColumn(False)
            End If

            If Session("strItem") Is Nothing Then
                'Delete those temp records created in the previous session (incase user exit IE without proper log off)
                objPO1.delete_Attachment(Session.SessionID)
            End If
            cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & File1.ClientID & "');")
            cmdUploadInt.Attributes.Add("onclick", "return checkDocFile('doc','" & File1Int.ClientID & "');")

            GetVendorDetail()
            Bindgrid()
            hidTotal.Value = ""

            objAdmin = Nothing
            objPR = Nothing
            objBudget = Nothing
            Session("ItemDeleted") = Nothing

        ElseIf Session("NewPoInfo") IsNot Nothing Then
            Session("ItemDeleted") = Nothing
            PopulateVenTypeAhead()
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
                Session("ProdList") = Nothing
            End If

            ViewState("blnCmdDelete") = True

            'Type will always be mod since the page is being reloaded
            ViewState("type") = "mod"

            If Not Session("NewPoInfo") Is Nothing Then
                ViewState("type") = Session("NewPoInfo").ToString.Split("|")(1)
            End If

            ViewState("mode") = "po"

            ViewState("strPageId") = strPageId
            ViewState("blnCmdAdd") = True

            ViewState("blnCmdRaise") = True
            ViewState("blnCmdUpload") = True
            ViewState("blnCmdUploadInt") = True
            ViewState("blnCmdRemove") = True
            ViewState("blnCmdSetup") = True
            ViewState("postback") = 1
            ViewState("SuppId") = ""
            Dim objAdmin As New Admin
            Dim dsDelivery As New DataSet
            Dim dsBill As New DataSet

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

            Dim strGST As String
            cmdRaise.Attributes.Add("onclick", "return InitialValidation();")
            cmdSubmit.Attributes.Add("onclick", "return InitialValidation();")
            divHide.Style("display") = ""
            If ViewState("type") = "new" Then

                cmdDelete.Visible = False
                ViewState("Vendor") = Request.QueryString("vendor")
                lblTitle.Text = "Raise Free Form Purchase Order"
                lblPONo.Text = "To Be Allocated By System"
                Common.SelDdl(objAdmin.user_Default_Add("B"), cboBillCode, True, True)
                fillAddress()
                enableBill(False)

                ' assign data to each textbox/label
                lblDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Today.Date)
                Dim dsCurrency As New DataSet

                'BindVendorList()

                If ViewState("strGST") = "" Then
                    ViewState("strGST") = "0"
                End If
                strIndexList = Session("ItemIndexList")
                Session.Remove("ItemIndexList")
                chkCustomPR.Checked = True
                chkRemarkPR.Checked = True

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
                If Session("urlreferer") = "" Then
                    lnkBack.NavigateUrl = dDispatcher.direct("PO", "SearchPR_All.aspx", "caller=buyer&pageid=" & strPageId)
                    If Me.Request.QueryString("Frm") = "Dashboard" Then
                        lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
                        Session("urlreferer") = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
                    End If
                Else
                    If Me.Request.QueryString("Frm") = "Dashboard" Then
                        lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
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

                If Not Session("NewPoInfo") Is Nothing Then
                    ViewState("poid") = Session("NewPoInfo").ToString.Split("|")(0)
                End If

                lblPONo.Text = ViewState("poid")
                hidNewPO.Value = ViewState("poid")
                lblTitle.Text = "Raise Free Form Purchase Order"

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
                        ViewState("rfqnum") = strRFQ_No
                        ViewState("modeRFQFromPR_Index_draft") = objDB.GetVal("SELECT PRM_PR_INDEX FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO = '" & objDB.GetVal("SELECT PRD_PR_NO FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & strRFQ_No & "'") & "'")

                        Dim strUrgent As String = objDB.GetVal("SELECT PRM_URGENT FROM PR_DETAILS, PR_MSTR WHERE PRD_PR_NO = PRM_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "' LIMIT 1")
                        If strUrgent = "1" Then
                            chkUrgent.Checked = True
                        Else
                            chkUrgent.Checked = False
                        End If

                    Else
                        ViewState("modeRFQFromPR_Index_draft") = ""
                    End If
                End If

                PO_Type = objDB.GetVal("SELECT IFNULL(POD_SOURCE,'') FROM PO_DETAILS WHERE pod_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND POD_PO_NO = '" & ViewState("poid") & "'")
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

            ViewState("GST") = "subtotal"

            Dim dsBCM As New DataSet
            ViewState("BCM") = CInt(objPR.checkBCM)
            If ViewState("BCM") > 0 Then
                If ViewState("modePR") = "pr" Then
                    dtBCM = objBudget.getBCMListByCompanyNew()
                Else
                    dsBCM = objBudget.getBCMListByUserNew(Session("UserId"), "")
                    dtBCM = dsBCM.Tables(0)
                End If
            End If

            Dim objPO As New PurchaseOrder
            Dim objPO1 As New PurchaseOrder_Buyer
            If objPO.isConvertedFromRFQ(ViewState("poid")) Then
                ViewState("ListingFromRFQ") = "True"
            End If

            If ViewState("modePR") = "pr" Then
                addDataGridColumn(True)
                chkCustomPR.Enabled = False
                chkRemarkPR.Enabled = False
            End If

            If Session("strItem") Is Nothing Then
                'Delete those temp records created in the previous session (incase user exit IE without proper log off)
                'objPO1.delete_Attachment(Session.SessionID)
            End If
            cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & File1.ClientID & "');")
            cmdUploadInt.Attributes.Add("onclick", "return checkDocFile('doc','" & File1Int.ClientID & "');")

            If Not lblPONo.Text.Contains("To") Then
                Bindgrid()
            End If

            GetVendorDetail()
            hidTotal.Value = ""

            objAdmin = Nothing
            objPR = Nothing
            objBudget = Nothing
        Else
            Session("ItemDeleted") = Nothing
            If Not Session("CatItemAdd_PONo") Is Nothing Then
                If Not Session("CatItemAdd_PONo").ToString.Trim.Contains("To") Then
                Else
                End If
            Else
            End If
            If Not Session("BatchUpload_Reload") Is Nothing Then
                If Session("BatchUpload_Reload") = "Yes" And ViewState("poid") Is Nothing Then
                    ViewState("poid") = lblPONo.Text
                    Dim _0 = ViewState("type")
                End If
            End If
            ViewState("postback") += 1

            Dim dsBCM As New DataSet
            ViewState("BCM") = CInt(objPR.checkBCM)
            If ViewState("BCM") > 0 Then
                If ViewState("modePR") = "pr" Then
                    dtBCM = objBudget.getBCMListByCompanyNew()
                Else
                    dsBCM = objBudget.getBCMListByUserNew(Session("UserId"), "")
                    dtBCM = dsBCM.Tables(0)
                End If
            End If

            If Session("CurrentScreen") = "AddItem" Then 'May 6, 2014
                Dim chkItem As CheckBox
                Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark, txtGSTAmt, txtGSTAmount As TextBox
                Dim lblItemLine, txtBudget, txtDelivery As Label
                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                'Dim cboGLCode, cboCust, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList
                Dim cboCust, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList 'Jules 2018.10.24 U00019
                Dim strItem, strItemCust As New ArrayList()
                Dim dgItem, dgItem2 As DataGridItem
                Dim k As Integer = 0
                Dim cboGift As DropDownList  ', cboFundType, cboPersonCode, cboProjectCode As DropDownList  'Jules 2018.07.14; Jules 2018.10.18
                Dim hidFundType, hidPersonCode, hidProjectCode, hidGLCode As TextBox 'Jules 2018.10.18

                For Each dgItem In dtgShopping.Items
                    txtQty = dgItem.FindControl("txtQty")
                    'cboGLCode = dgItem.FindControl("cboGLCode") 'Jules 2018.10.24 U00019
                    txtPrice = dgItem.FindControl("txtPrice")
                    txtBudget = dgItem.FindControl("txtBudget")
                    hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                    txtDelivery = dgItem.FindControl("txtDelivery")
                    hidDelCode = dgItem.FindControl("hidDelCode")
                    txtEstDate = dgItem.FindControl("txtEstDate")
                    txtWarranty = dgItem.FindControl("txtWarranty")
                    txtRemark = dgItem.FindControl("txtRemark")
                    cboCategoryCode = dgItem.FindControl("cboCategoryCode")
                    cboUOM = dgItem.FindControl("ddl_uom")
                    cboAssetGroup = dgItem.FindControl("cboAssetGroup")
                    txtGSTAmt = dgItem.FindControl("txtGSTAmt")
                    ddlGST = dgItem.FindControl("ddlGST")
                    txtGSTAmount = dgItem.FindControl("txtGSTAmount")
                    'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                    cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")

                    'Jules 2018.07.14
                    cboGift = dgItem.FindControl("cboGift")

                    'Jules 2018.10.18
                    'cboFundType = dgItem.FindControl("cboFundType")
                    'cboPersonCode = dgItem.FindControl("cboPersonCode")
                    'cboProjectCode = dgItem.FindControl("cboProjectCode")
                    hidFundType = dgItem.FindControl("hidFundType")
                    hidPersonCode = dgItem.FindControl("hidPersonCode")
                    hidProjectCode = dgItem.FindControl("hidProjectCode")
                    hidGLCode = dgItem.FindControl("hidGLCode")
                    'End modification.

                    chkItem = dgItem.FindControl("chkSelection")
                    If chkItem.Checked Then
                        If Session("CurrentScreen") = "AddItem" Then
                            'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text})

                            'Jules 2018.07.14
                            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedIndex})
                            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
                            strItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18
                        End If
                    Else
                        'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                        'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text})

                        'Jules 2018.07.14
                        'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedIndex})
                        'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
                        strItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18
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
                                Dim cboCustom As DropDownList
                                cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                                If chkItem.Checked Then
                                    If Session("CurrentScreen") = "AddItem" Then
                                        strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                                    End If
                                Else
                                    strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                                End If
                            Next
                        End If
                    End If

                    k += 1
                Next
                Session("strItem") = strItem
                Session("strItemCust") = strItemCust
                GetVendorDetail()
                Bindgrid()
                hidTotal.Value = ""
                Session("CurrentScreen") = ""
                Session("ProdList") = Nothing

            End If

        End If

        lblMsg.Text = ""
        hidTotal.Value = ""

        If ViewState("type") = "new" Then
            Select Case ViewState("mode")
                Case "po", "cc"
                Case "rfq"
                    cmdRemove.Visible = False
                    cmdDupPOLine.Visible = False
                    cmdAdd.Visible = False
            End Select
        ElseIf ViewState("type") = "mod" Then
            Select Case ViewState("mode")
                Case "po"
                Case "rfq"
            End Select
        End If

        If ViewState("ListingFromRFQ") = "True" Then
            cmdDupPOLine.Visible = False
            cmdRemove.Visible = False
            cmdAdd.Visible = False

            dtgShopping.Columns(EnumShoppingCart.icChk).Visible = False
            dtgShopping.Columns(EnumShoppingCart.icVendorItemCode).Visible = False
        End If

        dtgShopping.Columns(EnumShoppingCart.icMOQ).Visible = False
        txtInternal.Attributes.Add("onKeyDown", "limitText (this, 1000);")
        txtExternal.Attributes.Add("onKeyDown", "limitText (this, 1000);")
        cmdRemove.Attributes.Add("onclick", "return RemoveItemCheck();")
        cmdDupPOLine.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        cmdDelete.Attributes.Add("onclick", "return confirm('" & MsgForDeleteButton & "');")

        displayAttachFile()
        displayAttachFileInt()

        body1.Attributes.Add("onLoad", "refreshDatagrid(); calculateGrandTotal(); " & ViewState("body_loaditemcreated"))
        'ViewState("body_loaditemcreated") = ""

        Session("keepItem") = ""
        Session("keepAr") = New ArrayList()

        Call cboBillCode_Load()
        If ViewState("modePR") = "pr" Then
            cmdRaise.Enabled = False
            cmdAdd.Visible = False
            cmdRemove.Visible = False
            cmdDupPOLine.Visible = False
            cmdDelete.Visible = False

            txtShippingHandling.ReadOnly = True
            cboPayTerm.Enabled = False
            cboPayMethod.Enabled = False
            cboShipmentTerm.Enabled = False
            cboShipmentMode.Enabled = False
            txtShipVia.ReadOnly = True
            txtAttention.ReadOnly = True
            cboBillCode.Enabled = False
            chkUrgent.Enabled = False
        End If

        'Getting the ProdArray
        'Zulham Oct 25, 2013
        If Not Session("ProdList") Is Nothing Then
            If CType(Session("ProdList"), ArrayList).ToArray.Length = 0 Then
                Session("ProdList") = Nothing
            End If
        End If
        Dim aryProdCode As New ArrayList
        Dim dgItemFF As DataGridItem
        Try
            If dtgShopping.Items.Count > 0 Then
                If Not Page.IsPostBack Then
                    Dim objShopping As New ShoppingCart
                    Dim dsIterate As New DataTable
                    For Each dgItemFF In dtgShopping.Items
                        Dim txt_desc As TextBox
                        Dim ddl_uom As New DropDownList
                        txt_desc = dgItemFF.FindControl("lblProductDesc")
                        ddl_uom = dgItemFF.FindControl("ddl_uom")
                        If txt_desc.Text <> "" Then
                            'Oct 2, 2013
                            Dim _dsItem = objShopping.getPOItemList("PO", "", ViewState("poid"), "", Nothing, "", True)
                            dsIterate = _dsItem.Tables(1)
                            If dgItemFF.Cells(1).Text > dsIterate.Rows.Count Then
                                aryProdCode.Add(New String() {dgItemFF.Cells(1).Text, txt_desc.Text, ddl_uom.SelectedItem.Text, "0", "0"})
                            Else
                                aryProdCode.Add(New String() {dgItemFF.Cells(1).Text, txt_desc.Text, ddl_uom.SelectedItem.Text, "0", ""})
                            End If
                        End If
                    Next
                    Session("ProdList") = aryProdCode
                ElseIf Session("ProdList") Is Nothing Then
                    Dim objShopping As New ShoppingCart
                    Dim dsIterate As New DataTable
                    For Each dgItemFF In dtgShopping.Items
                        Dim txt_desc As TextBox
                        Dim ddl_uom As New DropDownList
                        txt_desc = dgItemFF.FindControl("lblProductDesc")
                        ddl_uom = dgItemFF.FindControl("ddl_uom")
                        If txt_desc.Text <> "" Then
                            Dim _dsItem = objShopping.getPOItemList("PO", "", ViewState("poid"), "", Nothing, "", True)
                            dsIterate = _dsItem.Tables(1)
                            If dgItemFF.Cells(1).Text > dsIterate.Rows.Count Then
                                aryProdCode.Add(New String() {dgItemFF.Cells(1).Text, txt_desc.Text, ddl_uom.SelectedItem.Text, "0", "0"})
                            Else
                                aryProdCode.Add(New String() {dgItemFF.Cells(1).Text, txt_desc.Text, ddl_uom.SelectedItem.Text, "0", ""})
                            End If
                        End If
                    Next
                    Session("ProdList") = aryProdCode
                    'ElseIf Session("ProdList")(0) = "" Then
                    '    Session("ProdList") = Nothing
                    '    Dim objShopping As New ShoppingCart
                    '    Dim dsIterate As New DataTable
                    '    For Each dgItemFF In dtgShopping.Items
                    '        Dim txt_desc As TextBox
                    '        Dim ddl_uom As New DropDownList
                    '        txt_desc = dgItemFF.FindControl("lblProductDesc")
                    '        ddl_uom = dgItemFF.FindControl("ddl_uom")
                    '        If txt_desc.Text <> "" Then
                    '            aryProdCode.Add(New String() {dgItemFF.Cells(1).Text, txt_desc.Text, ddl_uom.SelectedItem.Text, "0", ""})
                    '        End If
                    '    Next
                    '    Session("ProdList") = aryProdCode
                ElseIf Session("ProdList")(0)(0) = "" Then
                    Session("ProdList") = Nothing
                    Dim objShopping As New ShoppingCart
                    Dim dsIterate As New DataTable
                    For Each dgItemFF In dtgShopping.Items
                        Dim txt_desc As TextBox
                        Dim ddl_uom As New DropDownList
                        txt_desc = dgItemFF.FindControl("lblProductDesc")
                        ddl_uom = dgItemFF.FindControl("ddl_uom")
                        If txt_desc.Text <> "" Then
                            aryProdCode.Add(New String() {dgItemFF.Cells(1).Text, txt_desc.Text, ddl_uom.SelectedItem.Text, "0", ""})
                        End If
                    Next
                    Session("ProdList") = aryProdCode
                Else
                    Session("ProdList") = Session("ProdList")
                End If
            Else
                Session("ProdList") = Nothing
            End If
        Catch ex As Exception
        End Try
        'End
        Dim createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
        Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
        Dim _exceedCutOffDt As String = ""
        If CDate(createdDate) >= CDate(_cutoffDate) Then
            _exceedCutOffDt = "Yes"
            lblTax.Text = "SST Amount :"
            If txtVendor.Text <> "" Then
                Dim GSTRegNo = objDB.GetVal("SELECT IFNULL(CM_TAX_REG_NO, '') FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Hidden6.Value & "'")
                If GSTRegNo <> "" Then
                    strIsGst = "Yes"
                Else
                    strIsGst = "No"
                End If
            Else
                strIsGst = "Yes"
            End If
        Else
            _exceedCutOffDt = "No"
            strIsGst = "No"
        End If
        hidIsGST.Value = strIsGst
        hidexceedCutOffDt.Value = _exceedCutOffDt
        Session("NewPoInfo") = Nothing
        Session("ItemDeleted") = Nothing
        Dim Asset As New PurchaseOrder_Buyer
        If Asset.AssetGroupMstr = False Then
            dtgShopping.Columns(EnumShoppingCart.icAssetGroup).Visible = False
        End If

        'Jules 2018.07.14 - PAMB - Replaced 18=, 19=, 20=
        If strIsGst = "Yes" Then
            dtgShopping.Columns(EnumShoppingCart.icPrice).Visible = False '18
            dtgShopping.Columns(EnumShoppingCart.icTotal).Visible = True '19
            dtgShopping.Columns(EnumShoppingCart.icTax).Visible = True '20
        ElseIf _exceedCutOffDt = "Yes" Then
            dtgShopping.Columns(EnumShoppingCart.icPrice).Visible = False '18
            dtgShopping.Columns(EnumShoppingCart.icTotal).Visible = True '19
            dtgShopping.Columns(EnumShoppingCart.icTax).Visible = True '20
        Else
            dtgShopping.Columns(EnumShoppingCart.icTotal).Visible = False '19
            dtgShopping.Columns(EnumShoppingCart.icTax).Visible = False '20
            dtgShopping.Columns(EnumShoppingCart.icPrice).Visible = True '18
        End If
    End Sub
    Private Sub GetProdArray()

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

    Private Sub GetVendorDetail()
        ' get supplier data
        Dim dsVendor As New DataSet
        Dim strPayTerm As String
        Dim strPayMethod As String
        Dim objPR As New PR
        If Not Session("VendorID") Is Nothing Then Hidden6.Value = Session("VendorID")
        dsVendor = objPR.getVendorDetail(Hidden6.Value)
        If dsVendor.Tables(0).Rows.Count > 0 Then
            txtVendor.Text = dsVendor.Tables(0).Rows(0)("CM_COY_NAME")
            ViewState("strGST") = Common.parseNull(dsVendor.Tables(0).Rows(0)("CM_TAX_CALC_BY"))
            lblCurrencyCode.Text = dsVendor.Tables(0).Rows(0)("CM_CURRENCY_CODE")
            strPayTerm = CStr(Common.parseNull(dsVendor.Tables(0).Rows(0)("CV_Payment_Term")))
            strPayMethod = CStr(Common.parseNull(dsVendor.Tables(0).Rows(0)("CV_Payment_Method")))
            Common.SelDdl(strPayTerm, cboPayTerm)
            Common.SelDdl(strPayMethod, cboPayMethod)
            Session("BillingMethod") = dsVendor.Tables(0).Rows(0)("CV_BILLING_METHOD")
            If IsDBNull(Session("BillingMethod")) Then
                Session("BillingMethod") = "FPO"
            End If
        End If
        Session("VendorID") = Nothing
    End Sub

    Private Sub fillAddress()
        Dim i As Integer
        Dim dvwBilling As DataView
        Dim dsBilling As New DataSet
        Dim objAdmin As New Admin
        Dim objGlobalDdl As New AppGlobals
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
                objGlobalDdl.FillState(cboState, Common.parseNull(dvwBilling.Table.Rows(i)("AM_COUNTRY")))
                Common.SelDdl(Common.parseNull(dvwBilling.Table.Rows(i)("AM_STATE")), cboState, True, True)
                Exit For
            End If
        Next
        objGlobalDdl = Nothing
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
        'cmdAdd.Attributes.Add("onclick", "cmdAddC\lick(); ")
        Dim i As Integer = 0
        If ViewState("type") = "new" Then
            Select Case ViewState("mode")
                Case "cc"
                    strProdList = "''"
                    aryProdCode = Session("ProdList")
                    dsItem = objShopping.getPRItemList("ConCat", strProdList, "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4))
                    For i = 0 To aryProdCode.Count - 1
                        If strProdList = "" Then
                            'If Not aryProdCode(i)(0).ToString = "&" Then
                            strProdList = "'" & aryProdCode(i)(0) & "'"
                            dsTemp = objShopping.getPRItemList("ConCat", strProdList, "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4))
                            dsItem.Tables(0).Merge(dsTemp.Tables(0))
                            'End If
                        Else
                            'If Not aryProdCode(i)(0).ToString = "&" Then
                            strProdList &= ", '" & aryProdCode(i)(0) & "'"
                            dsTemp = objShopping.getPRItemList("ConCat", aryProdCode(i)(0), "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3), aryProdCode(i)(4))
                            dsItem.Tables(0).Merge(dsTemp.Tables(0))
                            'End If
                        End If
                    Next

                    displayAttachFile()
                    displayAttachFileInt()

                    ViewState("intPageRecordCnt") = dsItem.Tables(0).Rows.Count
                    dvViewSample = dsItem.Tables(0).DefaultView
                Case "po"
                    strProdList = "''"
                    dsItem = objShopping.getPOItemList("PC", strProdList, "")
                    If Not Session("ProdList") Is Nothing Then
                        If Session("ProdList").ToString <> "" Then
                            aryProdCode = Session("ProdList")
                            For i = 0 To aryProdCode.Count - 1
                                If strProdList = "" Then
                                    strProdList = "'" & aryProdCode(i)(1) & "'"
                                    If lblPONo.Text = "To Be Allocated By System" Then 'Unsaved data, so the index = ""
                                        dsTemp = objShopping.getPRItemList("BuyerCat", "", "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3))
                                    Else
                                        dsTemp = objShopping.getPRItemList("BuyerCat", aryProdCode(i)(0), "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3))
                                    End If
                                    dsItem.Tables(0).Merge(dsTemp.Tables(0))
                                Else
                                    strProdList &= ", '" & aryProdCode(i)(1) & "'"
                                    If lblPONo.Text = "To Be Allocated By System" Then 'Unsaved data, so the index = ""
                                        dsTemp = objShopping.getPRItemList("BuyerCat", "", "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3))
                                    Else
                                        dsTemp = objShopping.getPRItemList("BuyerCat", aryProdCode(i)(0), "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3))
                                    End If
                                    dsItem.Tables(0).Merge(dsTemp.Tables(0))
                                End If
                            Next
                        End If
                    End If
                    displayAttachFile()
                    displayAttachFileInt()
                    ViewState("intPageRecordCnt") = dsItem.Tables(0).Rows.Count
                    dvViewSample = dsItem.Tables(0).DefaultView
                    Dim popop = dvViewSample
                Case "rfq"
                    strProdList = ""

                    dsItem = objShopping.getPOItemList("RFQ", strIndexList, ViewState("rfqid"), ViewState("Vendor"), Session("RFQItemList"), ViewState("modeRFQFromPR_Index"))
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

                        If Common.parseNull(strShowColumn) = "1" Then
                            chkCustomPR.Checked = True
                        Else
                            chkCustomPR.Checked = False
                        End If

                        strShowColumn = objDB.GetVal("SELECT PRM_PRINT_REMARK FROM PR_MSTR, PR_DETAILS WHERE PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = PRM_COY_ID AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "'")

                        If Common.parseNull(strShowColumn) = "1" Then
                            chkRemarkPR.Checked = True
                        Else
                            chkRemarkPR.Checked = False
                        End If

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
                                displayAttachFileInt()
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
                                displayAttachFileInt()
                            End If
                            Session("strPutOnce") = "Y"
                        End If
                    End If

                    If ViewState("modeRFQFromPR_Index") <> "" Then
                        dvwCustomItem = dsItem.Tables(1).DefaultView

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

                cboPayTerm.SelectedIndex = strItemHead(0)(10)
                cboPayMethod.SelectedIndex = strItemHead(0)(11)

                lblCurrencyCode.Text = strItemHead(0)(12)
            End If

        ElseIf ViewState("type") = "mod" Then
            Select Case ViewState("mode")
                Case "cc"
                    strProdList = "''"
                    dsItem = objShopping.getPOItemList("PO", "", ViewState("poid"))
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
                    'Sept192013
                    If Session("CurrentScreen") IsNot Nothing Then
                        If Session("CurrentScreen") = "AddItem" And ViewState("poid") = Nothing Then
                            If Not lblPONo.Text.Trim.Contains("To") Then
                                ViewState("poid") = lblPONo.Text
                            End If
                        End If
                    End If
                    dsItem = objShopping.getPOItemList("PO", "", ViewState("poid"), "", Nothing, "", True)
                    If Session("CurrentScreen") = "AddItem" Or Session("CurrentScreen") = "RemoveItem" Or ViewState("ChangedRate") = "1" Then 'Jules 2019.01.14 - Added ChangedRate
                        aryProdCode = Session("ProdList")
                        Dim aryProdCodeNew As New ArrayList

                        'this is for additional items 
                        For i = 0 To aryProdCode.Count - 1
                            If aryProdCode(i)(0) = "" Then
                                aryProdCodeNew.Add(New String() {"", aryProdCode(i)(1), aryProdCode(i)(2), "0", "0"})
                                Continue For 'Jules 2019.01.14
                            End If

                            'Jules 2019.01.14
                            If Session("CurrentScreen") = "AddItem" AndAlso aryProdCode(i)(0) <> "" AndAlso aryProdCode(i)(4) = "0" Then
                                aryProdCodeNew.Add(New String() {"", aryProdCode(i)(1), aryProdCode(i)(2), "0", "0"})
                                Continue For
                            End If
                            'End modification.

                            If Session("CurrentScreen") = "RemoveItem" Or ViewState("ChangedRate") = "1" Then  'Jules 2019.01.14 - Added ChangedRate
                                If aryProdCode(i)(4) = "0" Then
                                    aryProdCodeNew.Add(New String() {"", aryProdCode(i)(1), aryProdCode(i)(2), "0", "0"})
                                End If
                            End If
                        Next

                        If Session("CurrentScreen") = "RemoveItem" Then
                            Session("aryProdCodeNew_All") = Nothing
                        End If
                        'For unsaved po_details
                        If Not Session("aryProdCodeNew_All") Is Nothing Then
                            aryProdCodeNew_All = CType(Session("aryProdCodeNew_All"), ArrayList)
                        End If
                        If Not aryProdCodeNew.Count = 0 Then
                            If aryProdCodeNew_All.Count = 0 Then
                                aryProdCodeNew_All = aryProdCodeNew
                            Else
                                For i = 0 To aryProdCodeNew.Count - 1
                                    aryProdCodeNew_All.Add(New String() {"", aryProdCodeNew(i)(1), aryProdCodeNew(i)(2), "0", "0"})
                                Next
                            End If
                            aryProdCode = aryProdCodeNew_All
                            Session("aryProdCodeNew_All") = aryProdCodeNew_All
                        End If

                        For i = 0 To aryProdCode.Count - 1
                            If strProdList = "" Then
                                strProdList = "'" & aryProdCode(i)(1) & "'"
                                dsTemp = objShopping.getPRItemList("BuyerCat", aryProdCode(i)(0), "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3))
                                dsItem.Tables(1).Merge(dsTemp.Tables(0))
                            Else
                                strProdList &= ", '" & aryProdCode(i)(1) & "'"
                                dsTemp = objShopping.getPRItemList("BuyerCat", aryProdCode(i)(0), "", "", Nothing, aryProdCode(i)(1), aryProdCode(i)(2), aryProdCode(i)(3))
                                dsItem.Tables(1).Merge(dsTemp.Tables(0))
                            End If
                        Next

                        Try
                            dvViewSample = dsItem.Tables(1).DefaultView
                            For row As Integer = 0 To dvViewSample.Count - 1
                                dvViewSample(row)("productdesc") = CType(Session("ProdList"), ArrayList)(row)(1)
                            Next
                        Catch ex As Exception

                        End Try


                        Session("keepAr") = keepAr
                        If Not strProdList = "" Then
                            Session("keepItem") = strProdList
                        End If
                        ViewState("ChangedRate") = "0" 'Jules 2019.01.14
                    End If
            End Select

            keepArPost = Session("keepAr")

            If Not IsNothing(keepArPost) Then
                If ViewState("type") = "new" Or ViewState("type") = "mod" Then
                    Select Case ViewState("mode")
                        Case "po", "rfq"
                            For i = 0 To keepArPost.Count - 1
                                dsTemp = objShopping.getPOItemList("PC", keepArPost(i)(0), ViewState("poid"))
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
                Me.txtVendor.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_COY_NAME"))
                ViewState("POM_S_COY_ID") = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_COY_ID"))
                Hidden6.Value = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_COY_ID"))
                ViewState("POM_S_COY_NAME") = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_COY_NAME"))
                ViewState("POM_RFQ_INDEX") = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_RFQ_INDEX"))
                ViewState("POM_RFQ_INDEX") = 1

                hidSupplier.Value = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_COY_ID"))
                lblDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dsItem.Tables(0).Rows(0)("POM_CREATED_DATE"))
                If txtShipVia.Text = "" Then
                    txtShipVia.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_SHIP_VIA"))
                End If
                If txtAttention.Text = "" Then
                    txtAttention.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_S_ATTN"))
                End If
                lblCurrencyCode.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_CURRENCY_CODE"))
                ViewState("Currency") = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_CURRENCY_CODE"))
                If txtInternal.Text = "" Then
                    txtInternal.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_INTERNAL_REMARK"))
                End If
                If txtExternal.Text = "" Then
                    txtExternal.Text = Common.parseNull(dsItem.Tables(0).Rows(0)("POM_EXTERNAL_REMARK"))
                End If

                Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_PAYMENT_TERM")), cboPayTerm, False, True)
                Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_SHIPMENT_TERM")), cboShipmentTerm, False, True)
                Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_PAYMENT_METHOD")), cboPayMethod, False, True)
                Common.SelDdl(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_SHIPMENT_MODE")), cboShipmentMode, False, True)

                If Not dsItem.Tables(0).Rows(0)("POM_SHIPMENT_TERM") Is Nothing And Not dsItem.Tables(0).Rows(0)("POM_SHIPMENT_MODE") Is Nothing Then
                    If dsItem.Tables(0).Rows(0)("POM_SHIPMENT_TERM") = "99" Then
                        cboShipmentTerm.SelectedValue = dsItem.Tables(0).Rows(0)("POM_SHIPMENT_TERM")
                    End If
                    If dsItem.Tables(0).Rows(0)("POM_SHIPMENT_MODE") = "99" Then
                        cboShipmentMode.SelectedValue = dsItem.Tables(0).Rows(0)("POM_SHIPMENT_MODE")
                    End If
                End If

                txtShippingHandling.Text = Format(Common.parseNull(dsItem.Tables(0).Rows(0)("POM_SHIP_AMT")), "###,###,##0.00")

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

                    cboPayTerm.SelectedIndex = strItemHead(0)(10)
                    cboPayMethod.SelectedIndex = strItemHead(0)(11)

                    lblCurrencyCode.Text = strItemHead(0)(12)
                End If


                enableBill(False)
                Session("keepItem") = ""
                Session("keepAr") = New ArrayList()
                If Common.parseNull(dsItem.Tables(0).Rows(0)("POM_PRINT_CUSTOM_FIELDS")) = "1" Then
                    chkCustomPR.Checked = True
                Else
                    chkCustomPR.Checked = False
                End If
                If Common.parseNull(dsItem.Tables(0).Rows(0)("POM_PRINT_REMARK")) = "1" Then
                    chkRemarkPR.Checked = True
                Else
                    chkRemarkPR.Checked = False
                End If

                If Common.parseNull(dsItem.Tables(0).Rows(0)("POM_URGENT")) = "1" Then
                    chkUrgent.Checked = True
                Else
                    chkUrgent.Checked = False
                End If

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
                    ViewState("blnCmdAdd") = True
                    ViewState("blnCmdRemove") = True
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

        'Jules 2018.07.14 - PAMB
        'Me.dtgShopping.Columns(6).Visible = True
        'Me.dtgShopping.Columns(7).Visible = False
        'Me.dtgShopping.Columns(5).Visible = True
        'Me.dtgShopping.Columns(24).Visible = True
        Me.dtgShopping.Columns(EnumShoppingCart.icCategoryCode1).Visible = True
        Me.dtgShopping.Columns(EnumShoppingCart.icTaxCode).Visible = False
        Me.dtgShopping.Columns(EnumShoppingCart.icGLCode1).Visible = True
        Me.dtgShopping.Columns(EnumShoppingCart.icBudget).Visible = True
        'End modification.
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
        Dim objDB As New EAD.DBCom
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

                Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
                Dim objGlobal As New AppGlobals
                Dim i As Integer
                Dim lstItem As New ListItem
                Dim GLCodelstItem As New ListItem
                Dim CategoryCodelstItem As New ListItem
                Dim dblAmt, dblGstAmt As Double
                Dim objAdmin As New Admin

                Dim strItem, strDuplicatedItem As New ArrayList()
                Dim strItemCust, strDupliItemCust As New ArrayList()

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
                lblProductCode.Text = IIf(Common.parseNull(dv("PRODUCTCODE")) = "&nbsp;", "", Common.parseNull(dv("PRODUCTCODE")))

                'Dim lblProductGrp As Label
                'lblProductGrp = e.Item.FindControl("lblProductGrp")
                'lblProductGrp.Text = IIf(Common.parseNull(dv("CDM_GROUP_INDEX")) = "&nbsp;", "", Common.parseNull(dv("CDM_GROUP_INDEX")))

                'e.Item.Cells(EnumShoppingCart.icCDGroup).Text = Common.parseNull(dv("CDM_GROUP_INDEX"))


                Dim lblProductDesc As TextBox
                lblProductDesc = e.Item.FindControl("lblProductDesc")
                lblProductDesc.Text = Common.parseNull(dv("PRODUCTDESC"))
                lblProductDesc.Width = System.Web.UI.WebControls.Unit.Pixel(300)

                Dim appGlobal As New AppGlobals
                Dim ddl_uom As New DropDownList
                Dim dv_uom As DataView
                ddl_uom = e.Item.FindControl("ddl_uom")

                dv_uom = appGlobal.GetCodeTableView(CodeTable.Uom)
                Common.FillDdl(ddl_uom, "CODE_DESC", "CODE_ABBR", dv_uom)

                Common.SelDdl(dv("UOM"), ddl_uom, False, True)
                e.Item.Cells(EnumShoppingCart.icNo).Text = intRow + 1
                Dim str1 As String
                Dim strSCoyId As String = ""
                Try
                    If ViewState("type") = "new" Then
                        Select Case ViewState("mode")
                            Case "po", "cc"
                                'If Not IsDBNull(e.Item.DataItem("Supplierid")) Then strSCoyId = e.Item.DataItem("Supplierid").ToString
                            Case "rfq"
                                If Not IsDBNull(e.Item.DataItem("PRD_S_COY_ID")) Then strSCoyId = e.Item.DataItem("PRD_S_COY_ID").ToString
                        End Select
                    ElseIf ViewState("type") = "mod" Then
                        'If Not IsDBNull(e.Item.DataItem("PRD_S_COY_ID")) Then strSCoyId = e.Item.DataItem("PRD_S_COY_ID").ToString
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
                revQty.ValidationExpression = "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$"
                revQty.ControlToValidate = "txtQty"
                revQty.ErrorMessage = e.Item.Cells(EnumShoppingCart.icNo).Text & ". " & ViewState("ValQtyMsg") '"Invalid quantity. Range should be from 0.01 to 999999.99"
                revQty.Text = "?"
                revQty.Display = ValidatorDisplay.Dynamic

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

                If IsDBNull(dv("UNITCOST")) Then
                    txtPrice.Text = ""
                Else
                    txtPrice.Text = Format(dv("UNITCOST"), "###,###,##0.0000")
                End If
                If ViewState("mode") = "cc" Then
                    txtPrice.ReadOnly = True
                End If

                Dim lblNoTax As Label
                lblNoTax = e.Item.FindControl("lblNoTax")

                Dim hidTaxPerc As TextBox
                Dim hidTaxID As TextBox

                Dim strPerc, strTaxID As String

                hidTaxPerc = e.Item.FindControl("hidtaxperc")
                hidTaxID = e.Item.FindControl("hidTaxID")

                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                Dim ddlGST, cboGSTTaxCode As DropDownList
                Dim txtGSTAmount As TextBox
                ddlGST = e.Item.FindControl("ddlGST")

                'Jules 2019.01.14
                If ddlGST.ClientID = ViewState("CurrentRate") Then
                    ddlGST.Focus()
                End If
                'End modification.

                cboGSTTaxCode = e.Item.FindControl("cboGSTTaxCode")
                txtGSTAmount = e.Item.FindControl("txtGSTAmount")

                If Not Hidden6.Value = "" Then
                    gstReg = objDB.GetVal("SELECT IFNULL(CM_TAX_REG_NO, '') FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Hidden6.Value & "'")
                End If

                'Jules 2018.07.23
                If IsDBNull(dv("POD_TAX_VALUE")) Then
                    txtGSTAmount.Text = "0.00"
                Else
                    txtGSTAmount.Text = dv("POD_TAX_VALUE")
                End If
                'End modification.

                If Not gstReg.Length = 0 Then

                    strIsGst = "Yes"
                    Dim clsGlobal As New AgoraLegacy.AppGlobals
                    clsGlobal.FillGST(ddlGST, True)

                    'Jules 2018.07.17 - PAMB
                    'If Not IsDBNull(dv("pod_gst_rate")) Then
                    '    If Not dv("pod_gst_rate") = "" And Not dv("pod_gst_rate").ToString = "0" Then
                    '        ddlGST.SelectedValue = dv("pod_gst_rate")
                    '        txtGSTAmount.Text = dv("pod_tax_value")
                    '    End If
                    'Else
                    '    ddlGST.SelectedIndex = 0
                    'End If
                    'objGlobal.FillGST(ddlGST, True)
                    'If Not dv("pod_gst_rate") = "" Then
                    '    ddlGST.SelectedValue = dv("pod_gst_rate")
                    '    txtGSTAmount.Text = dv("pod_tax_value")
                    'End If

                    'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                    'objGlobal.FillTaxCode(cboGSTTaxCode, , "P")
                    'If Common.parseNull(dv("GstTaxCode")) = "" Then
                    '    Common.SelDdl("", cboGSTTaxCode) 'Default to 'Select' option
                    'Else
                    '    Common.SelDdl(dv("GstTaxCode"), cboGSTTaxCode)
                    'End If

                    Dim objGST As New GST
                    Dim strGSTRegNo As String
                    Dim appVendorGSTRate As String = ""
                    Dim lstItemRate, lstItemTaxCode As New ListItem
                    strGSTRegNo = objGST.chkGST(Hidden6.Value)
                    If strGSTRegNo <> "" Then
                        objGlobal.FillGST(ddlGST, False)
                        If Common.parseNull(dv("pod_gst_rate")) = "" Then
                            appVendorGSTRate = objPO1.getApprovedVendorGSTRate(Hidden6.Value)
                            If appVendorGSTRate = "" Then
                                'Common.SelDdl("STD", ddlGST)
                            Else
                                Common.SelDdl(appVendorGSTRate, ddlGST)
                            End If
                        Else
                            Common.SelDdl(dv("pod_gst_rate"), ddlGST)
                        End If

                        'Jules 2018.10.08
                        If Common.parseNull(dv("pod_gst_rate")) <> "" Then
                            objGlobal.FillTaxCode(cboGSTTaxCode, Common.parseNull(dv("pod_gst_rate")), "P", ,, True)
                        ElseIf appVendorGSTRate <> "" Then
                            objGlobal.FillTaxCode(cboGSTTaxCode, appVendorGSTRate, "P", ,, True)
                        Else 'original code
                            objGlobal.FillTaxCode(cboGSTTaxCode, , "P")
                        End If
                        'End modification.

                        If Common.parseNull(dv("GstTaxCode")) = "" Then
                            'Common.SelDdl("", cboGSTTaxCode) 'Default to 'Select' option
                        Else
                            Common.SelDdl(dv("GstTaxCode"), cboGSTTaxCode)
                        End If

                        If ViewState("mode") = "cc" Then
                            ddlGST.Enabled = False
                            cboGSTTaxCode.Enabled = False
                        Else
                            ddlGST.Enabled = True
                            cboGSTTaxCode.Enabled = True
                        End If
                    Else
                        ddlGST.Items.Clear()
                        lstItemRate.Value = "N/A"
                        lstItemRate.Text = "N/A"
                        ddlGST.Items.Insert(0, lstItemRate)
                        ddlGST.Enabled = False

                        'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                        cboGSTTaxCode.Items.Clear()
                        lstItemTaxCode.Value = "NS"
                        lstItemTaxCode.Text = "NS"
                        cboGSTTaxCode.Items.Insert(0, lstItemTaxCode)
                        cboGSTTaxCode.Enabled = False
                    End If
                    'End modification.


                Else
                    strIsGst = "No"
                    ddlGST.Items.Insert(0, "N/A")
                    ddlGST.SelectedIndex = 0
                    ddlGST.SelectedValue = 0
                    ddlGST.Enabled = False
                    txtGSTAmount.Text = 0.0

                    'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                    cboGSTTaxCode.Items.Insert(0, "NS")
                    cboGSTTaxCode.SelectedIndex = 0
                    cboGSTTaxCode.SelectedValue = 0
                    cboGSTTaxCode.Enabled = False
                End If

                If ViewState("type") = "new" Then
                    Select Case ViewState("mode")
                        Case "cc"
                            hidTaxPerc.Text = Common.parseNull(dv("GST"))
                        Case "po"
                            If txtVendor.Text.Length > 0 Then
                                strPerc = objPO1.get_TaxPerc(e.Item.Cells(EnumShoppingCart.icProductCode).Text, txtVendor.Text, strTaxID)
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
                    End Select
                ElseIf ViewState("type") = "mod" Then
                    Select Case ViewState("mode")
                        Case "cc"
                            hidTaxPerc.Text = Common.parseNull(dv("GST"))
                        Case "po", "rfq"
                            ''if previously is from RFQ, then have to counter check with RFQ, not Vendor
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

                'check whether gst is applicable or otherwise
                Dim strGstCOD = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
                If lblDate.Text <> "" Then
                    If CDate(lblDate.Text) >= CDate(strGstCOD) Then
                        boolchkGSTCod = True
                    Else
                        boolchkGSTCod = False
                    End If
                Else
                    If Date.Now() >= CDate(strGstCOD) Then
                        boolchkGSTCod = True
                    Else
                        boolchkGSTCod = False
                    End If
                End If
                If boolchkGSTCod Then
                    strIsGst = "Yes"
                Else
                    strIsGst = "No"
                End If

                Dim createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
                If createdDate > System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate") Then
                    If txtVendor.Text <> "" Then
                        Dim GSTRegNo = objDB.GetVal("SELECT IFNULL(CM_TAX_REG_NO, '') FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Hidden6.Value & "'")
                        If GSTRegNo <> "" Then
                            strIsGst = "Yes"
                        Else
                            strIsGst = "No"
                        End If
                    Else
                        strIsGst = "Yes"
                    End If
                End If

                hidIsGST.Value = strIsGst 'Used as an indicator for jscript method calculateGrandTotal()

                Dim txtGSTAmt As TextBox
                txtGSTAmt = e.Item.FindControl("txtGSTAmt")
                dblAmt = CDec(txtAmount.Text)
                If ViewState("GST") = "subtotal" Then
                    dblTaxTotal = dblTaxTotal + dblAmt
                    ViewState("intGSTcnt") += 1
                    ViewState("intTotItem") += 1
                    If Not IsDBNull(dv("GST")) Then
                        hidTaxPerc.Text = dv("GST")
                    End If
                    dblGstAmt = dblAmt * (hidTaxPerc.Text / 100)
                    dblGstAmt = Math.Round(CType(dblGstAmt, Double), 2, MidpointRounding.AwayFromZero)
                    txtGSTAmt = e.Item.FindControl("txtGSTAmt")
                    txtGSTAmt.Text = Format(dblGstAmt, "###,##0.00")
                Else
                    dblNoTaxTotal = dblNoTaxTotal + dblAmt
                    If dv("GST") = 0 Then
                        txtGSTAmt.Text = "0.00"
                    Else
                        dblGstAmt = dblAmt * (dv("GST") / 100)
                        dblGstAmt = Math.Round(CType(dblGstAmt, Double), 2, MidpointRounding.AwayFromZero)
                        txtGSTAmt.Text = Format(dblGstAmt, "###,##0.00")
                    End If
                End If
                Dim hidGSTAmt As HtmlInputHidden
                hidGSTAmt = e.Item.FindControl("hidGSTAmt")
                hidGSTAmt.Value = dblGstAmt

                txtQty.Attributes.Add("onfocus", "return focusControl('" &
                                    IIf(ViewState("GST") = "product", 0, 1) & "', '" &
                                    txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" &
                                    txtAmount.ClientID & "', '" & ddlGST.ClientID & "', '" &
                                    txtGSTAmount.ClientID & "', '" & Common.parseNull(dv("POD_TAX_VALUE")) & "', '" & hidTaxPerc.ClientID & "', '" & txtGSTAmt.ClientID & "');")
                txtQty.Attributes.Add("onblur", "return calculateTotal('" &
                                    IIf(ViewState("GST") = "product", 0, 1) & "', '" &
                                    txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" &
                                    txtAmount.ClientID & "', '" & ddlGST.ClientID & "', '" &
                                    txtGSTAmount.ClientID & "', '" & Common.parseNull(dv("POD_TAX_VALUE")) & "', '" & hidTaxPerc.ClientID & "', '" & txtGSTAmt.ClientID & "','1','" & strIsGst & "');")
                txtPrice.Attributes.Add("onfocus", "return clearQuestionMark(); return focusControl('" &
                                   IIf(ViewState("GST") = "product", 0, 1) & "', '" &
                                   txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" &
                                   txtAmount.ClientID & "', '" & ddlGST.ClientID & "', '" &
                                   txtGSTAmount.ClientID & "', '" & Common.parseNull(dv("POD_TAX_VALUE")) & "', '" & hidTaxPerc.ClientID & "', '" & txtGSTAmt.ClientID & "');")
                txtPrice.Attributes.Add("onblur", "return calculateTotal('" &
                                    IIf(ViewState("GST") = "product", 0, 1) & "', '" &
                                    txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" &
                                    txtAmount.ClientID & "', '" & ddlGST.ClientID & "', '" &
                                    txtGSTAmount.ClientID & "', '" & Common.parseNull(dv("POD_TAX_VALUE")) & "', '" & hidTaxPerc.ClientID & "', '" & txtGSTAmt.ClientID & "','1','" & strIsGst & "');")
                'ddlGST.Attributes.Add("onchange", "return calculateTotalWithGST('" &
                '                   IIf(ViewState("GST") = "product", 0, 1) & "', '" &
                '                   txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" &
                '                   txtAmount.ClientID & "', '" & ddlGST.ClientID & "', '" &
                '                   txtGSTAmount.ClientID & "', '" & Common.parseNull(dv("POD_TAX_VALUE")) & "', '" & hidTaxPerc.ClientID & "', '" & txtGSTAmt.ClientID & "','1');")

                '2015-06-22: CH: Rounding issue (Prod issue)
                'dblTotalGst = dblTotalGst + dblGstAmt
                dblTotalGst = dblTotalGst + CDec(Format(dblGstAmt, "###0.00"))


                Dim lblItemLine As Label
                lblItemLine = e.Item.FindControl("lblItemLine")
                lblItemLine.Text = dv("ITEMLINE")

                Dim hidAssetGroupNo As TextBox
                hidAssetGroupNo = e.Item.FindControl("hidAssetGroupNo")

                'Dim objDB As New EAD.DBCom
                If ViewState("BCM") > 0 Then
                    If ViewState("type") = "new" Then
                        Select Case ViewState("mode")
                            Case "po", "cc"
                                'Jules 2018.10.24 U00044
                                'If Not dtBCM Is Nothing And dtBCM.Rows.Count > 0 Then
                                '    txtBudget.Text = Mid(dtBCM.Rows(0)("Acct_List"), 1, 10)
                                '    txtBudget.Attributes("title") = dtBCM.Rows(0)("Acct_List")
                                '    hidBudgetCode.Text = dtBCM.Rows(0)("Acct_Index")
                                'End If
                            Case "rfq"

                                If ViewState("modeRFQFromPR_Index") <> "" Then
                                    Dim strPR_Index As String = objDB.GetVal("SELECT IFNULL(RD_PR_LINE_INDEX, '') AS RD_PR_LINE_INDEX FROM RFQ_MSTR, RFQ_DETAIL WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_RFQ_NO = '" & ViewState("rfqnum") & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND RD_RFQ_LINE = '" & Common.parseNull(dv("ITEMINDEX")) & "'")
                                    Dim strPR_ACCT As String = objDB.GetVal("SELECT IFNULL(PRD_ACCT_INDEX, '') AS PRD_ACCT_INDEX FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")

                                    If Not IsDBNull(strPR_ACCT) And Common.parseNull(strPR_ACCT) <> "" Then
                                        If Not dtBCM Is Nothing Then
                                            Dim drTemp As DataRow()
                                            drTemp = dtBCM.Select("Acct_Index=" & strPR_ACCT)
                                            If drTemp.Length > 0 Then
                                                txtBudget.Text = Mid(drTemp(0)("Acct_List"), 1, 10)
                                                hidBudgetCode.Text = Common.parseNull(strPR_ACCT)
                                                txtBudget.Attributes("title") = drTemp(0)("Acct_List")
                                            End If
                                        End If
                                    Else
                                        'Jules 2018.10.24 - U000444
                                        'If Not dtBCM Is Nothing And dtBCM.Rows.Count > 0 Then
                                        '    txtBudget.Text = Mid(dtBCM.Rows(0)("Acct_List"), 1, 10)
                                        '    txtBudget.Attributes("title") = dtBCM.Rows(0)("Acct_List")
                                        '    hidBudgetCode.Text = dtBCM.Rows(0)("Acct_Index")
                                        'End If
                                    End If
                                Else
                                    If Not IsDBNull(dv("ACCT")) And Common.parseNull(dv("ACCT")) <> "" Then
                                        If Not dtBCM Is Nothing Then
                                            Dim drTemp As DataRow()
                                            drTemp = dtBCM.Select("Acct_Index=" & dv("ACCT"))
                                            If drTemp.Length > 0 Then
                                                txtBudget.Text = Mid(drTemp(0)("Acct_List"), 1, 10)
                                                txtBudget.Attributes("title") = drTemp(0)("Acct_List")
                                                hidBudgetCode.Text = Common.parseNull(dv("ACCT"))
                                            End If
                                        End If
                                    Else
                                        'Jules 2018.10.24 - U000444
                                        'If Not dtBCM Is Nothing And dtBCM.Rows.Count > 0 Then
                                        '    txtBudget.Text = Mid(dtBCM.Rows(0)("Acct_List"), 1, 10)
                                        '    txtBudget.Attributes("title") = dtBCM.Rows(0)("Acct_List")
                                        '    hidBudgetCode.Text = dtBCM.Rows(0)("Acct_Index")
                                        'End If
                                    End If
                                End If
                        End Select
                    ElseIf ViewState("type") = "mod" Then
                        If Not IsDBNull(dv("ACCT")) And Common.parseNull(dv("ACCT")) <> "" Then
                            If Not dtBCM Is Nothing Then
                                Dim drTemp As DataRow()
                                drTemp = dtBCM.Select("Acct_Index=" & dv("ACCT"))
                                If drTemp.Length > 0 Then
                                    txtBudget.Text = Mid(drTemp(0)("Acct_List"), 1, 10)
                                    txtBudget.Attributes("title") = drTemp(0)("Acct_List")
                                    hidBudgetCode.Text = Common.parseNull(dv("ACCT"))
                                End If
                            End If
                        Else
                            'Jules 2018.10.18 - Commented out to force user to select
                            'If Not dtBCM Is Nothing And dtBCM.Rows.Count > 0 Then
                            '    txtBudget.Text = Mid(dtBCM.Rows(0)("Acct_List"), 1, 10)
                            '    hidBudgetCode.Text = dtBCM.Rows(0)("Acct_Index")
                            '    txtBudget.Attributes("title") = dtBCM.Rows(0)("Acct_List")
                            'End If
                        End If
                        Select Case ViewState("mode")
                            Case "po"
                            Case "rfq"
                        End Select
                    End If



                    Dim cmdBudget As HtmlInputButton
                    cmdBudget = e.Item.FindControl("cmdBudget")
                    cmdBudget.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("PR", "Budget.aspx", "pageid=" & strPageId & "&id=" & txtBudget.ClientID & "&hidBudgetCode=" & hidBudgetCode.ClientID) & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no')")
                Else
                    e.Item.Cells(EnumShoppingCart.icBudget).Visible = False
                End If

                'Jules 2018.10.18
                'Jules 2018.05.04 - PAMB Scrum 2                
                'Dim cboGift, cboFundType, cboPersonCode, cboProjectCode As DropDownList
                Dim cboGift As DropDownList
                cboGift = e.Item.FindControl("cboGift")
                If Not IsDBNull(dv("GIFT")) And Common.parseNull(dv("GIFT")) <> "" Then
                    Common.SelDdl(dv("GIFT"), cboGift, True, True)
                End If

                'cboFundType = e.Item.FindControl("cboFundType")
                'objGlobal.FillAnalysisCode("L1", cboFundType)

                'If Not IsDBNull(dv("FUNDTYPE")) And Common.parseNull(dv("FUNDTYPE")) <> "" Then
                '    Common.SelDdl(dv("FUNDTYPE"), cboFundType, True, True)
                'End If

                'cboPersonCode = e.Item.FindControl("cboPersonCode")
                'objGlobal.FillAnalysisCode("L9", cboPersonCode)

                ''Jules 2018.09.05
                'Dim PersonCodelstItem As New ListItem
                'PersonCodelstItem.Value = "N/A"
                'PersonCodelstItem.Text = "N/A"
                'cboPersonCode.Items.Insert(1, PersonCodelstItem)
                ''End modification.

                'If Not IsDBNull(dv("PERSONCODE")) And Common.parseNull(dv("PERSONCODE")) <> "" Then
                '    Common.SelDdl(dv("PERSONCODE"), cboPersonCode, True, True)
                'End If

                'cboProjectCode = e.Item.FindControl("cboProjectCode")
                'objGlobal.FillAnalysisCode("L8", cboProjectCode)
                'If Not IsDBNull(dv("PROJECTCODE")) And Common.parseNull(dv("PROJECTCODE")) <> "" Then
                '    Common.SelDdl(dv("PROJECTCODE"), cboProjectCode, True, True)
                'End If
                Dim txtFundType, txtPersonCode, txtProjectCode, txtGLCode As Label
                txtFundType = e.Item.FindControl("txtFundType")
                txtFundType.Text = ""
                txtFundType.Width = System.Web.UI.WebControls.Unit.Pixel(50)

                txtPersonCode = e.Item.FindControl("txtPersonCode")
                txtPersonCode.Text = ""
                txtPersonCode.Width = System.Web.UI.WebControls.Unit.Pixel(50)

                txtProjectCode = e.Item.FindControl("txtProjectCode")
                txtProjectCode.Text = ""
                txtProjectCode.Width = System.Web.UI.WebControls.Unit.Pixel(50)

                txtGLCode = e.Item.FindControl("txtGLCode")
                txtGLCode.Text = ""
                txtGLCode.Width = System.Web.UI.WebControls.Unit.Pixel(50)

                Dim hidFundType, hidPersonCode, hidProjectCode, hidGLCode As TextBox
                hidFundType = e.Item.FindControl("hidFundType")
                hidFundType.Text = ""

                hidPersonCode = e.Item.FindControl("hidPersonCode")
                hidPersonCode.Text = ""

                hidProjectCode = e.Item.FindControl("hidProjectCode")
                hidProjectCode.Text = ""

                hidGLCode = e.Item.FindControl("hidGLCode")
                hidGLCode.Text = ""

                If Common.parseNull(dv("FUNDTYPEDESC")) <> "" Then
                    txtFundType.Text = dv("FUNDTYPEDESC")
                End If

                If Common.parseNull(dv("FUNDTYPE")) <> "" Then
                    hidFundType.Text = dv("FUNDTYPE")
                End If

                If Common.parseNull(dv("PERSONCODEDESC")) <> "" Then
                    txtPersonCode.Text = dv("PERSONCODEDESC")
                End If

                If Common.parseNull(dv("PERSONCODE")) <> "" Then
                    hidPersonCode.Text = dv("PERSONCODE")
                End If

                If Common.parseNull(dv("PROJECTCODEDESC")) <> "" Then
                    txtProjectCode.Text = dv("PROJECTCODEDESC")
                End If

                If Common.parseNull(dv("PROJECTCODE")) <> "" Then
                    hidProjectCode.Text = dv("PROJECTCODE")
                End If

                Dim cmdFundType As HtmlInputButton
                cmdFundType = e.Item.FindControl("cmdFundType")
                cmdFundType.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("PR", "AnalysisCode.aspx", "pageid=" & strPageId & "&id=" & txtFundType.ClientID & "&hidAnalysisCode=" & hidFundType.ClientID & "&dept=L1") & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no')")

                Dim cmdPersonCode As HtmlInputButton
                cmdPersonCode = e.Item.FindControl("cmdPersonCode")
                cmdPersonCode.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("PR", "AnalysisCode.aspx", "pageid=" & strPageId & "&id=" & txtPersonCode.ClientID & "&hidAnalysisCode=" & hidPersonCode.ClientID & "&dept=L9") & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no')")

                Dim cmdProjectCode As HtmlInputButton
                cmdProjectCode = e.Item.FindControl("cmdProjectCode")
                cmdProjectCode.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("PR", "AnalysisCode.aspx", "pageid=" & strPageId & "&id=" & txtProjectCode.ClientID & "&hidAnalysisCode=" & hidProjectCode.ClientID & "&dept=L8") & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no')")

                Dim cmdGLCode As HtmlInputButton
                cmdGLCode = e.Item.FindControl("cmdGLCode")
                cmdGLCode.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("PR", "GLCodeSearchP2P.aspx", "pageid=" & strPageId & "&id=" & txtGLCode.ClientID & "&hidGLCode=" & hidGLCode.ClientID) & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no')")
                txtGLCode.Attributes.Add("onchange", "return setGiftDDLState('" & hidGLCode.ClientID & "', '" & cboGift.ClientID & "');") 'Jules 2018.08.06
                'End modification.

                'Michelle (eBiz/303) 
                'Dim cboGLCode, cboCategoryCode, cbouom, cboAssetGroup1 As DropDownList
                Dim cboCategoryCode, cbouom, cboAssetGroup1 As DropDownList 'Jules 2018.10.24 U00019
                Dim dsGLCode As New DataSet
                Dim dsCategoryCode As New DataSet

                'Dim objDB As New EAD.DBCom
                Dim PFOR As String = objDB.GetVal("SELECT PM_PRODUCT_FOR FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & Common.parseNull(dv("PRODUCTCODE")) & "'")

                'Jules 2018.10.24 U00019
                'cboGLCode = e.Item.FindControl("cboGLCode")
                'dsGLCode = objAdmin.PopulateGLCodeMstr(Common.parseNull(dv("PRODUCTCODE")), Common.parseNull(dv("GLCODE")), PFOR)
                'dvwGLCode = dsGLCode.Tables(0).DefaultView

                'Common.FillDdl(cboGLCode, "DESCRIPTION", "GL Code", dvwGLCode)
                'End modification.

                'Jules 2018.10.24 U00019
                ''Jules 2018.08.07
                'GLCodelstItem.Value = ""
                'GLCodelstItem.Text = "---Select---"
                'cboGLCode.Items.Insert(0, GLCodelstItem)
                'cboGLCode.SelectedIndex = 0
                'End modification.

                If Not IsDBNull(dv("GLCode")) And Common.parseNull(dv("GLCode")) <> "" Then
                    If ViewState("modeRFQFromPR_Index") <> "" Then
                        Dim strPR_Index As String = objDB.GetVal("SELECT IFNULL(RD_PR_LINE_INDEX, '') AS RD_PR_LINE_INDEX FROM RFQ_MSTR, RFQ_DETAIL WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_RFQ_NO = '" & ViewState("rfqnum") & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND RD_RFQ_LINE = '" & Common.parseNull(dv("ITEMINDEX")) & "'")
                        Dim strGLCode As String = objDB.GetVal("SELECT IFNULL(PRD_B_GL_CODE, '') AS PRD_ACCT_INDEX FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")

                        'Jules 2018.10.24 U00019
                        'Common.SelDdl(strGLCode, cboGLCode, True, True)
                        hidGLCode.Text = strGLCode
                        If Not dtGLCode Is Nothing AndAlso hidGLCode.Text <> "" Then
                            Dim drGLCodeTemp As DataRow()
                            drGLCodeTemp = dtGLCode.Select("GLCODE='" & hidGLCode.Text & "'")
                            If drGLCodeTemp.Length > 0 Then
                                txtGLCode.Text = drGLCodeTemp(0)("DESCRIPTION")
                            End If
                        End If
                        'End modification.

                        'Jules 2018.07.14 - PAMB
                        If IsDBNull(dv("GIFT")) OrElse Common.parseNull(dv("GIFT")) = "" Then
                            Dim strGift As String = objDB.GetVal("SELECT IFNULL(PRD_GIFT, 'N') AS GIFT FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")
                            Common.SelDdl(strGift, cboGift, True, True)
                        End If

                        'Jules 2018.10.18 - Commented out.
                        'If IsDBNull(dv("FUNDTYPE")) OrElse Common.parseNull(dv("FUNDTYPE")) = "" Then
                        '    Dim strFundType As String = objDB.GetVal("SELECT IFNULL((SELECT AC_ANALYSIS_CODE FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = PRD_FUND_TYPE),'') AS FUNDTYPE FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")
                        '    Common.SelDdl(strFundType, cboFundType, True, True)
                        'End If

                        'If IsDBNull(dv("PERSONCODE")) OrElse Common.parseNull(dv("PERSONCODE")) = "" Then
                        '    Dim strPersonCode As String = objDB.GetVal("SELECT IFNULL((SELECT AC_ANALYSIS_CODE FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = PRD_PERSON_CODE),'') AS PERSONCODE FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")
                        '    Common.SelDdl(strPersonCode, cboPersonCode, True, True)
                        'End If

                        'If IsDBNull(dv("PROJECTCODE")) OrElse Common.parseNull(dv("PROJECTCODE")) = "" Then
                        '    Dim strProjectCode As String = objDB.GetVal("SELECT IFNULL((SELECT AC_ANALYSIS_CODE FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = PRD_PROJECT_CODE),'') AS PROJECTCODE FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")
                        '    Common.SelDdl(strProjectCode, cboProjectCode, True, True)
                        'End If
                        'End modification.
                    Else
                        'Jules 2018.10.24 U00019
                        'Common.SelDdl(dv("GLCode"), cboGLCode, True, True)
                        hidGLCode.Text = dv("GLCode")
                        If Not dtGLCode Is Nothing AndAlso hidGLCode.Text <> "" Then
                            Dim drGLCodeTemp As DataRow()
                            drGLCodeTemp = dtGLCode.Select("GLCODE='" & hidGLCode.Text & "'")
                            If drGLCodeTemp.Length > 0 Then
                                txtGLCode.Text = drGLCodeTemp(0)("DESCRIPTION")
                            End If
                        End If
                        'End modification
                    End If
                Else
                    If ViewState("modeRFQFromPR_Index") <> "" Then
                        Dim strPR_Index As String = objDB.GetVal("SELECT IFNULL(RD_PR_LINE_INDEX, '') AS RD_PR_LINE_INDEX FROM RFQ_MSTR, RFQ_DETAIL WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_RFQ_NO = '" & ViewState("rfqnum") & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND RD_RFQ_LINE = '" & Common.parseNull(dv("ITEMINDEX")) & "'")
                        Dim strGLCode As String = objDB.GetVal("SELECT IFNULL(PRD_B_GL_CODE, '') AS PRD_ACCT_INDEX FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")

                        'Jules 2018.10.24 U00019
                        'Common.SelDdl(strGLCode, cboGLCode, True, True)
                        hidGLCode.Text = dv("GLCode")
                        If Not dtGLCode Is Nothing AndAlso hidGLCode.Text <> "" Then
                            Dim drGLCodeTemp As DataRow()
                            drGLCodeTemp = dtGLCode.Select("GLCODE='" & hidGLCode.Text & "'")
                            If drGLCodeTemp.Length > 0 Then
                                txtGLCode.Text = drGLCodeTemp(0)("DESCRIPTION")
                            End If
                        End If
                        'End modification

                        'Jules 2018.07.14 - PAMB
                        If IsDBNull(dv("GIFT")) OrElse Common.parseNull(dv("GIFT")) = "" Then
                            Dim strGift As String = objDB.GetVal("SELECT IFNULL(PRD_GIFT, 'N') AS GIFT FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")
                            Common.SelDdl(strGift, cboGift, True, True)
                        End If

                        'Jules 2018.10.18 - Commented out.
                        'If IsDBNull(dv("FUNDTYPE")) OrElse Common.parseNull(dv("FUNDTYPE")) = "" Then
                        '    Dim strFundType As String = objDB.GetVal("SELECT IFNULL((SELECT AC_ANALYSIS_CODE FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = PRD_FUND_TYPE),'') AS FUNDTYPE FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")
                        '    Common.SelDdl(strFundType, cboFundType, True, True)
                        'End If

                        'If IsDBNull(dv("PERSONCODE")) OrElse Common.parseNull(dv("PERSONCODE")) = "" Then
                        '    Dim strPersonCode As String = objDB.GetVal("SELECT IFNULL((SELECT AC_ANALYSIS_CODE FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = PRD_PERSON_CODE),'') AS PERSONCODE FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")
                        '    Common.SelDdl(strPersonCode, cboPersonCode, True, True)
                        'End If

                        'If IsDBNull(dv("PROJECTCODE")) OrElse Common.parseNull(dv("PROJECTCODE")) = "" Then
                        '    Dim strProjectCode As String = objDB.GetVal("SELECT IFNULL((SELECT AC_ANALYSIS_CODE FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = PRD_PROJECT_CODE),'') AS PROJECTCODE FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")
                        '    Common.SelDdl(strProjectCode, cboProjectCode, True, True)
                        'End If
                        'End modification.

                        '    'Jules 2018.10.24 U00019
                        'ElseIf cboGLCode.Items.Count = 0 Then
                        '    GLCodelstItem.Value = ""
                        '    GLCodelstItem.Text = "Not Applicable"
                        '    cboGLCode.Items.Insert(0, GLCodelstItem)
                        '    cboGLCode.SelectedIndex = 0
                        'End modification.

                    Else 'Jules 2018.07.14 - PAMB - If raise RFQ without PR.
                        'Jules 2018.10.18 - Commented out.
                        'If Not IsDBNull(dv("FUNDTYPE")) AndAlso Common.parseNull(dv("FUNDTYPE")) = "INTP" Then
                        '    Common.SelDdl(dv("FUNDTYPE"), cboFundType, True, True)
                        'End If
                        'End modification.
                    End If
                End If

                'Jules 2018.10.24 U00019
                'cboGLCode.Attributes.Add("onchange", "return setGiftDDLState('" & cboGLCode.ClientID & "', '" & cboGift.ClientID & "');") 'Jules 2018.08.07

                Dim Asset As New PurchaseOrder_Buyer
                If Asset.AssetGroupMstr = True Then
                    Dim cboAssetGroup As DropDownList
                    Dim dvCustom As DataView

                    cboAssetGroup = e.Item.FindControl("cboAssetGroup")

                    objGlobal.FillAssetGroup(cboAssetGroup)
                    If Not IsDBNull(dv("POD_ASSET_GROUP")) And Common.parseNull(dv("POD_ASSET_GROUP")) <> "" Then
                        If ViewState("modeRFQFromPR_Index") <> "" Then
                            Dim strPR_Index As String = objDB.GetVal("SELECT IFNULL(RD_PR_LINE_INDEX, '') AS RD_PR_LINE_INDEX FROM RFQ_MSTR, RFQ_DETAIL WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_RFQ_NO = '" & ViewState("rfqnum") & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND RD_RFQ_LINE = '" & Common.parseNull(dv("ITEMINDEX")) & "'")
                            Dim strAssetGroup As String = objDB.GetVal("SELECT IFNULL(PRD_ASSET_GROUP, '') AS PRD_ASSET_GROUP FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")
                            Common.SelDdl(strAssetGroup, cboAssetGroup, True, True)
                        Else
                            Common.SelDdl(dv("POD_ASSET_GROUP"), cboAssetGroup, True, True)
                        End If
                    Else
                        If ViewState("modeRFQFromPR_Index") <> "" Then
                            Dim strPR_Index As String = objDB.GetVal("SELECT IFNULL(RD_PR_LINE_INDEX, '') AS RD_PR_LINE_INDEX FROM RFQ_MSTR, RFQ_DETAIL WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_RFQ_NO = '" & ViewState("rfqnum") & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND RD_RFQ_LINE = '" & Common.parseNull(dv("ITEMINDEX")) & "'")
                            Dim strAssetGroup As String = objDB.GetVal("SELECT IFNULL(PRD_ASSET_GROUP, '') AS PRD_ASSET_GROUP FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")
                            Common.SelDdl(strAssetGroup, cboAssetGroup, True, True)
                        Else
                            Common.SelDdl(dv("POD_ASSET_GROUP"), cboAssetGroup, True, True)
                        End If
                    End If

                    hidAssetGroupNo.Text = Common.parseNull(dv("POD_ASSET_NO"))
                    If ViewState("type") = "new" Then
                        Select Case ViewState("mode")
                            Case "po", "cc"
                            Case "rfq"
                                If ViewState("modeRFQFromPR_Index") <> "" Then
                                    Dim strPR_Index As String = objDB.GetVal("SELECT IFNULL(RD_PR_LINE_INDEX, '') AS RD_PR_LINE_INDEX FROM RFQ_MSTR, RFQ_DETAIL WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_RFQ_NO = '" & ViewState("rfqnum") & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND RD_RFQ_LINE = '" & Common.parseNull(dv("ITEMINDEX")) & "'")
                                    Dim strPR_Asset_No As String = objDB.GetVal("SELECT IFNULL(PRD_ASSET_NO, '') AS PRD_ASSET_NO FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")
                                    hidAssetGroupNo.Text = Common.parseNull(strPR_Asset_No)
                                End If
                        End Select
                    ElseIf ViewState("type") = "mod" Then

                    End If
                End If

                cboCategoryCode = e.Item.FindControl("cboCategoryCode")

                'Jules 2018.07.31 - Set to N/A
                'Dim PFORC As String = objDB.GetVal("SELECT PM_PRODUCT_FOR FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & Common.parseNull(dv("PRODUCTCODE")) & "'")

                'dsCategoryCode = objAdmin.PopulateCategoryCodeMstr(Common.parseNull(dv("PRODUCTCODE")), Common.parseNull(dv("CategoryCode")), PFORC)
                'dvwCategoryCode = dsCategoryCode.Tables(0).DefaultView
                'Common.FillDdl(cboCategoryCode, "Category Code", "Category Code", dvwCategoryCode)

                'If Not IsDBNull(dv("CategoryCode")) And Common.parseNull(dv("CategoryCode")) <> "" Then
                '    If ViewState("modeRFQFromPR_Index") <> "" Then
                '        Dim strPR_Index As String = objDB.GetVal("SELECT IFNULL(RD_PR_LINE_INDEX, '') AS RD_PR_LINE_INDEX FROM RFQ_MSTR, RFQ_DETAIL WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_RFQ_NO = '" & ViewState("rfqnum") & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND RD_RFQ_LINE = '" & Common.parseNull(dv("ITEMINDEX")) & "'")
                '        Dim strCatCode As String = objDB.GetVal("SELECT IFNULL(PRD_B_CATEGORY_CODE, '') AS PRD_ACCT_INDEX FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")
                '        Common.SelDdl(strCatCode, cboCategoryCode, True, True)
                '    Else
                '        Common.SelDdl(dv("CategoryCode"), cboCategoryCode, True, True)
                '    End If
                'Else
                '    If ViewState("modeRFQFromPR_Index") <> "" Then
                '        Dim strPR_Index As String = objDB.GetVal("SELECT IFNULL(RD_PR_LINE_INDEX, '') AS RD_PR_LINE_INDEX FROM RFQ_MSTR, RFQ_DETAIL WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_RFQ_NO = '" & ViewState("rfqnum") & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND RD_RFQ_LINE = '" & Common.parseNull(dv("ITEMINDEX")) & "'")
                '        Dim strCatCode As String = objDB.GetVal("SELECT IFNULL(PRD_B_CATEGORY_CODE, '') AS PRD_ACCT_INDEX FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")
                '        Common.SelDdl(strCatCode, cboCategoryCode, True, True)
                '    ElseIf cboCategoryCode.Items.Count = 0 Then
                CategoryCodelstItem.Value = ""
                CategoryCodelstItem.Text = "N/A" '"Not Applicable"
                cboCategoryCode.Items.Insert(0, CategoryCodelstItem)
                cboCategoryCode.SelectedIndex = 0
                '    End If
                'End If
                'End modification.

                If ViewState("type") = "new" Then
                    hidDelCode.Text = ViewState("strDeliveryDefault")
                    txtDelivery.Text = Mid(objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), ViewState("strDeliveryDefault"), "D"), 1, 10)
                    txtDelivery.Attributes("title") = objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), ViewState("strDeliveryDefault"), "D")
                    Select Case ViewState("mode")
                        Case "po"
                        Case "rfq"
                    End Select
                ElseIf ViewState("type") = "mod" Then
                    If IsDBNull(dv("DADDRCODE")) Then
                        hidDelCode.Text = ViewState("strDeliveryDefault")
                        txtDelivery.Text = Mid(objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), ViewState("strDeliveryDefault"), "D"), 1, 10)
                        txtDelivery.Attributes("title") = objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), ViewState("strDeliveryDefault"), "D")
                    Else
                        ViewState("strDeliveryDefault") = Common.Parse(dv("DADDRCODE"))
                        hidDelCode.Text = ViewState("strDeliveryDefault")
                        txtDelivery.Text = Mid(objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), Common.Parse(dv("DADDRCODE")), "D"), 1, 10)
                        txtDelivery.Attributes("title") = objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), ViewState("strDeliveryDefault"), "D")

                    End If
                    Select Case ViewState("mode")
                        Case "po"
                        Case "rfq"
                    End Select
                End If
                Dim cmdDelivery As HtmlInputButton
                cmdDelivery = e.Item.FindControl("cmdDelivery")
                cmdDelivery.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("Admin", "AddressMaster.aspx", "pageid=" & strPageId & "&mod=P&type2=RPO&type=D&id=" & hidDelCode.ClientID & "&txtDelivery=" & txtDelivery.ClientID) & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no')")

                'custom field
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
                                    If ViewState("modePR") = "pr" Then
                                        For j = 0 To dvwCustomItem.Table.Rows.Count - 1
                                            If dvwCustomItem.Table.Rows(j)("PCD_PR_LINE") = dv("ITEMLINE") And dvwCustomItem.Table.Rows(j)("PCD_FIELD_NO") = dtgShopping.Columns(i).SortExpression Then
                                                Common.SelDdl(encodeCustomField(dvwCustomItem.Table.Rows(j)("PCD_FIELD_VALUE")), cboCustom, True, True)
                                                blnSetDefault = True
                                                Exit For
                                            End If
                                        Next
                                    End If
                            End Select
                        ElseIf ViewState("type") = "mod" Then
                            For j = 0 To dvwCustomItem.Table.Rows.Count - 1
                                If dvwCustomItem.Table.Rows(j)("PCD_PR_LINE") = dv("ITEMLINE") And dvwCustomItem.Table.Rows(j)("PCD_FIELD_NO") = dtgShopping.Columns(i).SortExpression Then
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

                        If ViewState("modePR") = "pr" Then
                            e.Item.Cells(i).Enabled = False
                        End If

                        'Okt 28, 2013
                        If Session("strItemCust") Is Nothing Then
                            Dim strAryLst As New ArrayList
                            For j = 0 To dvwCustomItem.Table.Rows.Count - 1
                                strAryLst.Add(New String() {j, encodeCustomField(dvwCustomItem.Table.Rows(j)("PCD_FIELD_VALUE"))})
                            Next
                            Session("strItemCust") = strAryLst
                        End If
                        Try
                            If Session("strItemCust") IsNot Nothing Then
                                strItemCust = Session("strItemCust")
                                If (CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1) <= (strItemCust.Count / intCnt) - 1 Then
                                    cboCustom.SelectedIndex = strItemCust(intRow + i - CInt(EnumShoppingCart.icRemark))(1)
                                End If
                            End If
                            If ViewState("strDuplicatedCustItem") IsNot Nothing Then
                                strItemCust = Session("strItemCust")
                                strDupliItemCust = ViewState("strDuplicatedCustItem")
                                If (CInt(e.Item.Cells(EnumShoppingCart.icNo).Text)) = (strItemCust.Count / intCnt) + 1 Then
                                    Dim _po = (4 * intRow) + intRow + i - CInt(EnumShoppingCart.icRemark)
                                    cboCustom.SelectedIndex = strDupliItemCust(0)(1)
                                End If
                            End If
                        Catch ex As Exception

                        End Try
                        'End

                    Next
                End If

                Dim txtEstDate As TextBox
                Dim txtWarranty As TextBox

                txtEstDate = e.Item.FindControl("txtEstDate")
                txtWarranty = e.Item.FindControl("txtWarranty")
                'Michelle (2/8/2010) - To include the defaul EDD

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
                            iEDD = objPO1.get_EDDPerc(e.Item.Cells(EnumShoppingCart.icProductCode).Text, txtVendor.Text) 'cboVendor.SelectedItem.Value)
                            If iEDD = 0 Then
                                iEDD = 1
                            End If
                            If txtEstDate.ReadOnly = False Then txtEstDate.Text = txtDate1.AddDays(iEDD)
                            txtWarranty.Text = "0"
                        Case "rfq"
                            aryProdCodeNew.Add(Common.parseNull(dv("PRODUCTCODE")))

                            If txtEstDate.Enabled = True Then txtEstDate.Text = txtDate1.AddDays(Common.parseNull(dv("ETD")))
                            txtWarranty.Text = Common.parseNull(dv("WARRANTYTERMS"))
                            txtWarranty.CssClass = "lblnumerictxtbox"
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
                        txtWarranty.CssClass = "lblnumerictxtbox"
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

                Dim txtRemark As TextBox
                txtRemark = e.Item.FindControl("txtRemark")
                txtRemark.Text = Common.parseNull(dv("REMARK"))

                intRow = intRow + 1

                If ViewState("modeRFQFromPR_Index") <> "" Then
                    Dim strPR_Index As String = objDB.GetVal("SELECT IFNULL(RD_PR_LINE_INDEX, '') AS RD_PR_LINE_INDEX FROM RFQ_MSTR, RFQ_DETAIL WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_RFQ_NO = '" & ViewState("rfqnum") & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND RD_RFQ_LINE = '" & Common.parseNull(dv("ITEMINDEX")) & "'")
                    Dim strPR_Remark As String = objDB.GetVal("SELECT IFNULL(PRD_REMARK, '') AS PRD_REMARK FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")

                    txtRemark.Text = strPR_Remark

                    Dim strPR_Del As String = objDB.GetVal("SELECT PRD_D_ADDR_CODE FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")

                    hidDelCode.Text = strPR_Del
                    txtDelivery.Text = Mid(objAdmin.GetSpecificAddrPR(HttpContext.Current.Session("CompanyId"), strPR_Del, "D", strPR_Index), 1, 10)

                    Dim strPR_Bill As String = objDB.GetVal("SELECT PRM_B_ADDR_CODE FROM PR_MSTR, PR_DETAILS WHERE PRD_COY_ID = PRM_COY_ID AND PRD_PR_NO = PRM_PR_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'")
                    cboBillCode.SelectedItem.Value = strPR_Bill
                    cboBillCode.SelectedItem.Text = strPR_Bill
                    enableBill(True)
                    fillAddressPR(objDB.GetVal("SELECT PRM_PR_NO FROM PR_MSTR, PR_DETAILS WHERE PRD_COY_ID = PRM_COY_ID AND PRD_PR_NO = PRM_PR_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = '" & strPR_Index & "'"))
                    enableBill(False)
                End If

                txtRemark.Attributes.Add("onKeyDown", "limitText (this, 400);")

                cbouom = e.Item.FindControl("ddl_uom")
                cboAssetGroup1 = e.Item.FindControl("cboAssetGroup")

                If Session("strItem") IsNot Nothing Then
                    strItem = Session("strItem")
                    If (CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1) <= strItem.Count - 1 Then
                        'Jules 2018.10.24 U00019
                        'cboGLCode.SelectedIndex = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(0)
                        hidGLCode.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(0)
                        If Not dtGLCode Is Nothing AndAlso hidGLCode.Text <> "" Then
                            Dim drGLCodeTemp As DataRow()
                            drGLCodeTemp = dtGLCode.Select("GLCODE='" & hidGLCode.Text & "'")
                            If drGLCodeTemp.Length > 0 Then
                                txtGLCode.Text = drGLCodeTemp(0)("DESCRIPTION")
                            End If
                        End If
                        'End modification.
                        txtQty.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(1)
                        txtPrice.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(2)
                        hidBudgetCode.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(4)

                        'Jules 2018.07.16 - PAMB - Error if Budget Code is no longer valid.
                        'If Not dtBCM Is Nothing Then
                        If Not dtBCM Is Nothing AndAlso hidBudgetCode.Text <> "" Then
                            Dim drTemp As DataRow()
                            drTemp = dtBCM.Select("Acct_Index=" & hidBudgetCode.Text)
                            If drTemp.Length > 0 Then
                                txtBudget.Text = Mid(drTemp(0)("Acct_List"), 1, 10)
                                txtBudget.Attributes("title") = drTemp(0)("Acct_List")
                            End If
                        End If
                        hidDelCode.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(6)
                        txtDelivery.Attributes("title") = objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(6), "D") '  
                        txtDelivery.Text = Mid(objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), Common.Parse(strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(6)), "D"), 1, 10)
                        txtEstDate.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(7)
                        txtWarranty.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(8)
                        txtRemark.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(9)
                        cboCategoryCode.SelectedIndex = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(10)
                        cbouom.SelectedIndex = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(11)
                        cboAssetGroup1.SelectedIndex = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(12)
                        txtGSTAmt.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(13)
                        ddlGST.SelectedValue = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(14)
                        txtGSTAmount.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(15)
                        'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH

                        objGlobal.FillTaxCode(cboGSTTaxCode, ddlGST.SelectedValue, "P", , , True)
                        cboGSTTaxCode.SelectedValue = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(16)

                        'Jules 2018.07.14 - PAMB
                        cboGift.SelectedIndex = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(17)

                        'Jules 2018.10.18
                        'cboFundType.SelectedIndex = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(18)
                        'cboPersonCode.SelectedIndex = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(19)
                        'cboProjectCode.SelectedIndex = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(20)
                        hidFundType.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(18)
                        If hidFundType.Text <> "" Then
                            If dtFundType IsNot Nothing AndAlso hidFundType.Text <> "" Then
                                Dim drFundType As DataRow()
                                drFundType = dtFundType.Select("AC_ANALYSIS_CODE='" & hidFundType.Text & "'")
                                If drFundType.Length > 0 Then
                                    txtFundType.Text = drFundType(0)("AC_ANALYSIS_CODE_DESC") & " : " & hidFundType.Text
                                End If
                            Else
                                txtFundType.Text = objDB.GetVal("SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), IFNULL(AC_ANALYSIS_CODE, '')) FROM analysis_code WHERE AC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND AC_DEPT_CODE = 'L1' AND AC_STATUS = 'O' AND AC_ANALYSIS_CODE ='" & hidFundType.Text & "'")
                            End If
                        End If
                        hidPersonCode.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(19)
                        If hidPersonCode.Text <> "" Then
                            If dtPersonCode IsNot Nothing AndAlso hidPersonCode.Text <> "" Then
                                Dim drPersonCode As DataRow()
                                drPersonCode = dtPersonCode.Select("AC_ANALYSIS_CODE='" & hidPersonCode.Text & "'")
                                If drPersonCode.Length > 0 Then
                                    txtPersonCode.Text = drPersonCode(0)("AC_ANALYSIS_CODE_DESC") & " : " & hidPersonCode.Text
                                End If
                            Else
                                txtPersonCode.Text = objDB.GetVal("SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), IFNULL(AC_ANALYSIS_CODE, '')) FROM analysis_code WHERE AC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND AC_DEPT_CODE = 'L9' AND AC_STATUS = 'O' AND AC_ANALYSIS_CODE ='" & hidPersonCode.Text & "'")
                            End If
                        End If
                        hidProjectCode.Text = strItem(CInt(e.Item.Cells(EnumShoppingCart.icNo).Text) - 1)(20)
                        If hidProjectCode.Text <> "" Then
                            If dtProjectCode IsNot Nothing AndAlso hidProjectCode.Text <> "" Then
                                Dim drProjectCode As DataRow()
                                drProjectCode = dtProjectCode.Select("AC_ANALYSIS_CODE='" & hidProjectCode.Text & "'")
                                If drProjectCode.Length > 0 Then
                                    txtProjectCode.Text = drProjectCode(0)("AC_ANALYSIS_CODE_DESC") & " : " & hidProjectCode.Text
                                End If
                            Else
                                txtProjectCode.Text = objDB.GetVal("SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), IFNULL(AC_ANALYSIS_CODE, '')) FROM analysis_code WHERE AC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND AC_DEPT_CODE = 'L8' AND AC_STATUS = 'O' AND AC_ANALYSIS_CODE ='" & hidProjectCode.Text & "'")
                            End If
                        End If
                        'End modification.

                        Dim itemTotal As Double = 0.0
                        itemTotal = txtPrice.Text * txtQty.Text
                        itemTotal = Math.Round(CType(itemTotal, Double), 2, MidpointRounding.AwayFromZero)
                        txtAmount.Text = Format(itemTotal, "###,##0.00")

                        'Jules 2018.08.03 - Always re-calculate GST in case SST Rate was changed due to postback
                        If ddlGST.Enabled = True Then
                            Dim reCalcGST As Double = 0.0
                            Dim taxPerc As Integer = 0
                            taxPerc = objDB.GetVal("SELECT TAX_PERC FROM TAX WHERE tax_code='" & ddlGST.SelectedValue & "'")
                            reCalcGST = itemTotal * (taxPerc / 100)
                            reCalcGST = Math.Round(CType(reCalcGST, Double), 2, MidpointRounding.AwayFromZero)
                            txtGSTAmount.Text = Format(reCalcGST, "###,##0.00")
                        End If
                        'End modification.
                    End If
                End If


                If ViewState("strDuplicatedItem") IsNot Nothing Then
                    strDuplicatedItem = ViewState("strDuplicatedItem")
                    strItem = Session("strItem")
                    If (CInt(e.Item.Cells(EnumShoppingCart.icNo).Text)) = strItem.Count + 1 Then

                        'Jules 2018.10.24 U00019
                        'cboGLCode.SelectedIndex = strDuplicatedItem(0)(0)
                        hidGLCode.Text = strDuplicatedItem(0)(0)
                        If Not dtGLCode Is Nothing AndAlso hidGLCode.Text <> "" Then
                            Dim drGLCodeTemp As DataRow()
                            drGLCodeTemp = dtGLCode.Select("GLCODE='" & hidGLCode.Text & "'")
                            If drGLCodeTemp.Length > 0 Then
                                txtGLCode.Text = drGLCodeTemp(0)("DESCRIPTION")
                            End If
                        End If
                        'End modification.

                        txtQty.Text = strDuplicatedItem(0)(1)
                        txtPrice.Text = strDuplicatedItem(0)(2)
                        hidBudgetCode.Text = strDuplicatedItem(0)(4)
                        If Not dtBCM Is Nothing Then
                            Dim drTemp As DataRow()
                            drTemp = dtBCM.Select("Acct_Index=" & hidBudgetCode.Text)
                            If drTemp.Length > 0 Then
                                txtBudget.Text = Mid(drTemp(0)("Acct_List"), 1, 10)
                                txtBudget.Attributes("title") = drTemp(0)("Acct_List")
                            End If
                        End If
                        hidDelCode.Text = strDuplicatedItem(0)(6)
                        txtDelivery.Text = Mid(objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), Common.Parse(strDuplicatedItem(0)(6)), "D"), 1, 10)
                        txtDelivery.Attributes("title") = objAdmin.GetSpecificAddr(HttpContext.Current.Session("CompanyId"), strDuplicatedItem(0)(6), "D") '  
                        txtEstDate.Text = strDuplicatedItem(0)(7)
                        txtWarranty.Text = strDuplicatedItem(0)(8)
                        txtRemark.Text = strDuplicatedItem(0)(9)
                        cboCategoryCode.SelectedIndex = strDuplicatedItem(0)(10)
                        cbouom.SelectedIndex = strDuplicatedItem(0)(11)
                        cboAssetGroup1.SelectedIndex = strDuplicatedItem(0)(12)
                        txtGSTAmt.Text = strDuplicatedItem(0)(13)
                        ViewState("strDuplicatedItem") = Nothing
                        ddlGST.SelectedValue = strDuplicatedItem(0)(14)
                        txtGSTAmount.Text = strDuplicatedItem(0)(15)
                        'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                        cboGSTTaxCode.SelectedValue = strDuplicatedItem(0)(16)

                        'Jules 2018.07.14 - PAMB
                        cboGift.SelectedIndex = strDuplicatedItem(0)(17)

                        'Jules 2018.10.18
                        'cboFundType.SelectedIndex = strDuplicatedItem(0)(18)
                        'cboPersonCode.SelectedIndex = strDuplicatedItem(0)(19)
                        'cboProjectCode.SelectedIndex = strDuplicatedItem(0)(20)
                        hidFundType.Text = strDuplicatedItem(0)(18)
                        If hidFundType.Text <> "" Then
                            If dtFundType IsNot Nothing AndAlso hidFundType.Text <> "" Then
                                Dim drFundType As DataRow()
                                drFundType = dtFundType.Select("AC_ANALYSIS_CODE='" & hidFundType.Text & "'")
                                If drFundType.Length > 0 Then
                                    txtFundType.Text = drFundType(0)("AC_ANALYSIS_CODE_DESC") & " : " & hidFundType.Text
                                End If
                            Else
                                txtFundType.Text = objDB.GetVal("SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), IFNULL(AC_ANALYSIS_CODE, '')) FROM analysis_code WHERE AC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND AC_DEPT_CODE = 'L1' AND AC_STATUS = 'O' AND AC_ANALYSIS_CODE ='" & hidFundType.Text & "'")
                            End If
                        End If
                        hidPersonCode.Text = strDuplicatedItem(0)(19)
                        If hidPersonCode.Text <> "" Then
                            If dtPersonCode IsNot Nothing AndAlso hidPersonCode.Text <> "" Then
                                Dim drPersonCode As DataRow()
                                drPersonCode = dtPersonCode.Select("AC_ANALYSIS_CODE='" & hidPersonCode.Text & "'")
                                If drPersonCode.Length > 0 Then
                                    txtPersonCode.Text = drPersonCode(0)("AC_ANALYSIS_CODE_DESC") & " : " & hidPersonCode.Text
                                End If
                            Else
                                txtPersonCode.Text = objDB.GetVal("SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), IFNULL(AC_ANALYSIS_CODE, '')) FROM analysis_code WHERE AC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND AC_DEPT_CODE = 'L9' AND AC_STATUS = 'O' AND AC_ANALYSIS_CODE ='" & hidPersonCode.Text & "'")
                            End If
                        End If
                        hidProjectCode.Text = strDuplicatedItem(0)(20)
                        If hidProjectCode.Text <> "" Then
                            If dtProjectCode IsNot Nothing AndAlso hidProjectCode.Text <> "" Then
                                Dim drProjectCode As DataRow()
                                drProjectCode = dtProjectCode.Select("AC_ANALYSIS_CODE='" & hidProjectCode.Text & "'")
                                If drProjectCode.Length > 0 Then
                                    txtProjectCode.Text = drProjectCode(0)("AC_ANALYSIS_CODE_DESC") & " : " & hidProjectCode.Text
                                End If
                            Else
                                txtProjectCode.Text = objDB.GetVal("SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), IFNULL(AC_ANALYSIS_CODE, '')) FROM analysis_code WHERE AC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND AC_DEPT_CODE = 'L8' AND AC_STATUS = 'O' AND AC_ANALYSIS_CODE ='" & hidProjectCode.Text & "'")
                            End If
                        End If
                        'End modification.

                        Dim itemTotal As Double = 0.0
                        itemTotal = txtPrice.Text * txtQty.Text
                        itemTotal = Math.Round(CType(itemTotal, Double), 2, MidpointRounding.AwayFromZero)
                        txtAmount.Text = Format(itemTotal, "###,##0.00")
                    End If
                End If

                'Tooltip
                If cboCategoryCode IsNot Nothing Then
                    For Each li As ListItem In cboCategoryCode.Items
                        li.Attributes("title") = li.Text
                    Next
                End If

                'Jules 2018.10.24 U00019
                'If cboGLCode IsNot Nothing Then
                '    For Each li As ListItem In cboGLCode.Items
                '        li.Attributes("title") = li.Text
                '    Next
                'End If
                'End modification.

                'Jules 2018.10.24 U00019
                'Jules 2018.07.14 - PAMB - Do not allow user to select Gift if CAPEX.           
                'If cboGLCode.SelectedValue.ToString <> "" AndAlso cboGLCode.SelectedValue.ToString.Substring(0, 1) = "1" Then
                If hidGLCode.Text <> "" AndAlso hidGLCode.Text.ToString.Substring(0, 1) = "1" Then
                    cboGift.Enabled = False
                End If
                'End modification.

                If ViewState("modePR") = "pr" Then
                    e.Item.Cells(EnumShoppingCart.icGLCode).Enabled = False
                    e.Item.Cells(EnumShoppingCart.icGLCode1).Enabled = False
                    e.Item.Cells(EnumShoppingCart.icCategoryCode1).Enabled = False
                    e.Item.Cells(EnumShoppingCart.icTaxCode).Enabled = False
                    e.Item.Cells(EnumShoppingCart.icRfqQty).Enabled = False
                    e.Item.Cells(EnumShoppingCart.icTolerance).Enabled = False
                    e.Item.Cells(EnumShoppingCart.icQty).Enabled = False
                    e.Item.Cells(EnumShoppingCart.icUOM).Enabled = False
                    e.Item.Cells(EnumShoppingCart.icTotal).Enabled = False
                    e.Item.Cells(EnumShoppingCart.icTax).Enabled = False
                    e.Item.Cells(EnumShoppingCart.icSource).Enabled = False
                    'e.Item.Cells(EnumShoppingCart.icCDGroup).Enabled = False
                    e.Item.Cells(EnumShoppingCart.icBudget).Enabled = False
                    Dim cmdBudget As HtmlInputButton
                    cmdBudget = e.Item.FindControl("cmdBudget")
                    cmdBudget.Disabled = True
                    e.Item.Cells(EnumShoppingCart.icDelivery).Enabled = False
                    cmdDelivery = e.Item.FindControl("cmdDelivery")
                    cmdDelivery.Disabled = True
                    e.Item.Cells(EnumShoppingCart.icEstDate).Enabled = False
                    e.Item.Cells(EnumShoppingCart.icWarranty).Enabled = False
                    e.Item.Cells(EnumShoppingCart.icMOQ).Enabled = False
                    e.Item.Cells(EnumShoppingCart.icAssetGroup).Enabled = False

                    'Jules 2018.07.14 - PAMB
                    e.Item.Cells(EnumShoppingCart.icGift).Enabled = False
                    e.Item.Cells(EnumShoppingCart.icGift1).Enabled = False
                    e.Item.Cells(EnumShoppingCart.icFundType).Enabled = False
                    e.Item.Cells(EnumShoppingCart.icPersonCode).Enabled = False
                    e.Item.Cells(EnumShoppingCart.icProjectCode).Enabled = False
                    'End modification.
                End If

            ElseIf e.Item.ItemType = ListItemType.Header Then
                If ViewState("BCM") <= 0 Then
                    e.Item.Cells(EnumShoppingCart.icBudget).Visible = False
                Else
                    If ViewState("modePR") = "pr" Then
                        e.Item.Cells(EnumShoppingCart.icBudget).Enabled = False
                    End If

                End If
                If ViewState("modePR") = "pr" Then
                    e.Item.Cells(EnumShoppingCart.icDelivery).Enabled = False
                    For i = EnumShoppingCart.icRemark To EnumShoppingCart.icRemark + intCnt - 1
                        e.Item.Cells(i).Enabled = False
                    Next

                End If

            End If
        Catch ex As Exception

        End Try
        dtgShopping.Columns(EnumShoppingCart.icMOQ).Visible = False
        'dtgShopping.Columns(EnumShoppingCart.icCDGroup).Visible = False
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

        Try
            If e.Item.ItemType = ListItemType.Header Then
                Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
                chkAll.Attributes.Add("onclick", "selectAll();")

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

                'Jules 2018.10.18
                For i = EnumShoppingCart.icFundType To EnumShoppingCart.icProjectCode
                    Dim cell As TableCell
                    Dim lbl As New Label
                    cell = e.Item.Cells(i)
                    If i = EnumShoppingCart.icFundType Then
                        lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("PR", "AnalysisCode.aspx", "pageid=" & strPageId & "&id=txtFundType&mode=all&dept=L1") & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                    ElseIf i = EnumShoppingCart.icPersonCode Then
                        lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("PR", "AnalysisCode.aspx", "pageid=" & strPageId & "&id=txtPersonCode&mode=all&dept=L9") & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                    ElseIf i = EnumShoppingCart.icProjectCode Then
                        lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("PR", "AnalysisCode.aspx", "pageid=" & strPageId & "&id=txtProjectCode&mode=all&dept=L8") & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                    End If
                    cell.Controls.Add(lbl)
                Next

                For i = EnumShoppingCart.icGLCode To EnumShoppingCart.icGLCode1
                    Dim cell As TableCell
                    Dim lbl As New Label
                    cell = e.Item.Cells(i)
                    If i = EnumShoppingCart.icGLCode Then
                        lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("PR", "GLCodeSearchP2P.aspx", "pageid=" & strPageId & "&id=txtGLCode&mode=all") & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                    ElseIf i = EnumShoppingCart.icGLCode1 Then
                        lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("PR", "GLCodeSearchP2P.aspx", "pageid=" & strPageId & "&id=txtGLCode&mode=all") & "', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                    End If
                    cell.Controls.Add(lbl)
                Next
                'End modification.

                For i = EnumShoppingCart.icBudget To dtgShopping.Columns.Count - 2
                    Dim cell As TableCell
                    Dim lbl As New Label
                    cell = e.Item.Cells(i)
                    If i = EnumShoppingCart.icBudget Then
                        lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("PR", "Budget.aspx", "pageid=" & strPageId) & "&id=cboBudget&mode=all', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                    ElseIf i = EnumShoppingCart.icDelivery Then
                        lbl.Text = "Delivery Address"
                        lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("Admin", "AddressMaster.aspx", "pageid=" & strPageId) & "&mod=P&type2=RPO&type=D&id=cboDelivery&mode=all', '', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                    ElseIf i = EnumShoppingCart.icEstDate Then
                        'lbl.Text = "<A href='javascript:;' onclick=""window.open('AddWarrantyTerms.aspx?pageid=" & strPageId & "&type=E&id=txtEstDate', '', 'resizable=no,width=400,height=200,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                        lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendarForAll.aspx", "id=txtEstDate") & "','cal','width=180,height=155,left=270,top=180')"">" & e.Item.Cells(i).Text & "</A>"
                        'lbl.Text = e.Item.Cells(i).Text
                    ElseIf i = EnumShoppingCart.icWarranty Then
                        If ViewState("type") <> "rfq" And ViewState("listtype") <> "rfq" And ViewState("modePR") <> "pr" Then
                            lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("PR", "AddWarrantyTerms.aspx", "pageid=" & strPageId) & "&type=W&id=txtWarranty', '', 'resizable=no,width=400,height=200,status=no,menubar=no');"">" & e.Item.Cells(i).Text & "</a>"
                        Else
                            lbl.Text = e.Item.Cells(i).Text
                        End If
                    ElseIf i > EnumShoppingCart.icWarranty Then
                        str = dDispatcher.direct("Admin", "CustomFieldValue.aspx", "pageid=" & strPageId & "&mod=P&module=PO&value=" & dtgShopping.Columns(i).SortExpression & "&name=" & Server.UrlEncode(dtgShopping.Columns(i).HeaderText) & "&id=" & dtgShopping.Columns(i).SortExpression)
                        lbl.Text = "<A href='javascript:;' onclick=""window.open(&quot;" & str & "&quot;, &quot;&quot;, &quot;scrollbars=yes,resizable=yes,width=400,height=400,status=no,menubar=no&quot;);"">" & e.Item.Cells(i).Text & "</a>"
                    End If
                    cell.Controls.Add(lbl)
                Next
            End If

            '//this line must be included
            dtgShopping.AllowSorting = False

            txtShippingHandling.Attributes.Add("onfocus", "return calculateGrandTotal();")
            txtShippingHandling.Attributes.Add("onblur", "return calculateGrandTotal();")

            Grid_ItemCreated(dtgShopping, e)
            'ViewState("body_loaditemcreated") = "calculateAllIndividualTotal(); "

        Catch ex As Exception

        End Try

    End Sub

    Private Sub addDataGridColumn(Optional ByVal FrmPR As Boolean = False)
        Dim objAdmin As New Admin
        Dim dsCustom As New DataSet
        Dim dsCustomField As New DataSet
        Dim i As Integer
        Dim strPRIndex As String

        If FrmPR = True Then
            If ViewState("modeRFQFromPR_Index") = "" Then
                strPRIndex = objDB.GetVal("SELECT POM_PO_INDEX FROM PO_MSTR WHERE POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_PO_NO = '" & ViewState("poid") & "' ")
                dvwCus = objAdmin.getCustomFieldPR("", "PO", strPRIndex)
            Else
                ' 24Nov2011 _Yap: Using 'PR' to get back the PR custom fields and save a set to PO.
                dvwCus = objAdmin.getCustomFieldPR("", "PR", ViewState("modeRFQFromPR_Index"))
            End If

        Else
            dvwCus = objAdmin.getCustomField("", "PO") 'Oct 2, 2013 Commented as it causes problem if u're useing the batch upload
        End If

        If Not dvwCus Is Nothing Then
            intCnt = dvwCus.Count
            ReDim dvwCustom(intCnt)
            ReDim strCustomDefault(intCnt)

            '//dynamically add template column

            For i = 0 To intCnt - 1
                Dim col As TemplateColumn = New TemplateColumn
                col.ItemTemplate = New dgTemplate(dvwCus.Table.Rows(i)("CF_FIELD_NO"), 2)
                col.HeaderText = dvwCus.Table.Rows(i)("CF_FIELD_NAME")
                col.SortExpression = dvwCus.Table.Rows(i)("CF_FIELD_NO")
                dtgShopping.Columns.AddAt(dtgShopping.Columns.Count - 1, col)

                If FrmPR = True Then
                    If ViewState("modeRFQFromPR_Index") = "" Then
                        strPRIndex = objDB.GetVal("SELECT POM_PO_INDEX FROM PO_MSTR WHERE POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_PO_NO = '" & ViewState("poid") & "' ")
                        dsCustomField = objAdmin.Populate_customFieldPR(dvwCus.Table.Rows(i)("CF_FIELD_NO"), "", "PO", strPRIndex)
                    Else
                        ViewState("modeRFQFromPR_Index2") = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRM_PR_INDEX),""'"")) AS CHAR(2000)) AS PRM_PR_INDEX FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO IN (" & objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRD_PR_NO),""'"")) AS CHAR(2000)) AS PRD_PR_NO FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "'") & ")")
                        dsCustomField = objAdmin.Populate_customFieldPR2(dvwCus.Table.Rows(i)("CF_FIELD_NO"), "", "PR", ViewState("modeRFQFromPR_Index2"))
                    End If
                Else
                    dsCustomField = objAdmin.Populate_customField(dvwCus.Table.Rows(i)("CF_FIELD_NO"), "", "PO")
                End If

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
        dtMaster.Columns.Add("GST", Type.GetType("System.String"))
        dtMaster.Columns.Add("POCost", Type.GetType("System.Double"))
        dtMaster.Columns.Add("RfqIndex", Type.GetType("System.String"))
        dtMaster.Columns.Add("QuoNo", Type.GetType("System.String"))
        dtMaster.Columns.Add("POM_EXCHANGE_RATE", Type.GetType("System.Double"))
        dtMaster.Columns.Add("BillingMethod", Type.GetType("System.String"))
        Dim dtr As DataRow
        dtr = dtMaster.NewRow()
        If ViewState("poid") IsNot Nothing Then
            dtr("PONo") = ViewState("poid")
        ElseIf lblPONo.Text <> "To Be Allocated By System" Then
            dtr("PONo") = Me.lblPONo.Text
        End If
        dtr("VendorID") = Hidden6.Value
        dtr("Attn") = txtAttention.Text
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
        dtr("PrintCustom") = IIf(chkCustomPR.Checked, "1", "0")
        dtr("Urgent") = IIf(chkUrgent.Checked, "1", "0")
        dtr("ExchangeRate") = 1

        'Jules 2018.08.03 
        If Session("BillingMethod") Is Nothing Then
            dtr("BillingMethod") = hidBillingMethod.Value
        End If
        dtr("BillingMethod") = Session("BillingMethod")
        dtr("GST") = "1"
        dtr("PrintRemark") = IIf(chkRemarkPR.Checked, "1", "0")
        'Michelle (14/5/2011) - To remove the comma
        dtr("ShipAmt") = Replace(txtShippingHandling.Text, ",", "")
        If hidCost.Value = "" Then
            hidCost.Value = "0"
        End If
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
        dtDetails.Columns.Add("TaxValue", Type.GetType("System.String"))
        dtDetails.Columns.Add("DeliveryAddr", Type.GetType("System.String"))
        dtDetails.Columns.Add("AcctIndex", Type.GetType("System.String"))
        dtDetails.Columns.Add("ProductType", Type.GetType("System.String"))
        dtDetails.Columns.Add("Source", Type.GetType("System.String"))
        dtDetails.Columns.Add("TAXID", Type.GetType("System.String"))
        dtDetails.Columns.Add("MOQ", Type.GetType("System.String"))
        dtDetails.Columns.Add("MPQ", Type.GetType("System.String"))
        dtDetails.Columns.Add("CDGroup", Type.GetType("System.String"))
        dtDetails.Columns.Add("POD_RFQ_ITEM_LINE", Type.GetType("System.String"))
        dtDetails.Columns.Add("ItemCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("WarrantyTerms", Type.GetType("System.Int32"))

        dtDetails.Columns.Add("CategoryCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("GLCode", Type.GetType("System.String"))

        dtDetails.Columns.Add("RfqQty", Type.GetType("System.Int32"))
        dtDetails.Columns.Add("QtyTolerance", Type.GetType("System.Int32"))
        dtDetails.Columns.Add("SupplierCompanyId", Type.GetType("System.String"))

        dtDetails.Columns.Add("AssetGroup", Type.GetType("System.String"))
        dtDetails.Columns.Add("AssetGroupNo", Type.GetType("System.String"))

        dtDetails.Columns.Add("SelectedGST", Type.GetType("System.String"))
        dtDetails.Columns.Add("GSTTaxAmount", Type.GetType("System.String"))
        'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
        dtDetails.Columns.Add("GSTTaxCode", Type.GetType("System.String"))

        'Jules 2018.05.07 - PAMB Scrum 2 & 3
        dtDetails.Columns.Add("Gift", Type.GetType("System.String"))
        dtDetails.Columns.Add("FundType", Type.GetType("System.String"))
        dtDetails.Columns.Add("PersonCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("ProjectCode", Type.GetType("System.String"))
        'End modification.

        Dim dtrd As DataRow
        Dim dgItem As DataGridItem

        'Jules 2019.01.07 - To ensure PO Cost is accurate.
        Dim decItemPrice As Decimal = 0
        ViewState("POCost") = 0
        'End modification.

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

            dtrd("ProductCode") = IIf(dgItem.Cells(EnumShoppingCart.icProductCode).Text = "&nbsp;", "", dgItem.Cells(EnumShoppingCart.icProductCode).Text)
            dtrd("VendorItemCode") = IIf(dgItem.Cells(EnumShoppingCart.icVendorItemCode).Text = "" Or dgItem.Cells(EnumShoppingCart.icVendorItemCode).Text = "&nbsp;",
                                        "", dgItem.Cells(EnumShoppingCart.icVendorItemCode).Text)
            dtrd("ProductDesc") = CType(dgItem.FindControl("lblProductDesc"), TextBox).Text
            dtrd("UOM") = CType(dgItem.FindControl("ddl_uom"), DropDownList).SelectedItem.Text

            dtrd("MOQ") = dgItem.Cells(EnumShoppingCart.icMOQ).Text
            dtrd("MPQ") = dgItem.Cells(EnumShoppingCart.icMPQ).Text

            ' _Yap: For Interface
            dtrd("ItemCode") = IIf(dgItem.Cells(EnumShoppingCart.icVendorItemCode).Text = "", "", dgItem.Cells(EnumShoppingCart.icVendorItemCode).Text)

            Dim txtQty As TextBox ' HtmlInputText
            txtQty = dgItem.FindControl("txtQty")
            If IsNumeric(txtQty.Text) And Regex.IsMatch(Trim(txtQty.Text), "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$") Then
                dtrd("Qty") = txtQty.Text
            Else
                dtrd("Qty") = 1.0
                'blnValid = False
            End If

            Dim txtPrice As TextBox
            Dim uPrice As Decimal
            txtPrice = dgItem.FindControl("txtPrice")

            uPrice = CDec(txtPrice.Text)
            If IsNumeric(uPrice) And Regex.IsMatch(Trim(uPrice), "(?!^0*$)(?!^0*\.0*$)^\d{1,12}(\.\d{1,4})?$") Then
                dtrd("UnitCost") = txtPrice.Text
            Else
                dtrd("UnitCost") = 0
            End If

            Dim hidTaxPerc As TextBox
            Dim txtAmount, txtGST As TextBox, txtGSTAmt As TextBox
            Dim ddlGST, cboGSTTaxCode As DropDownList

            hidTaxPerc = dgItem.FindControl("hidtaxperc")
            txtAmount = dgItem.FindControl("txtAmount")
            ddlGST = dgItem.FindControl("ddlGST")

            '2015-06-22: CH: Rounding issue (Prod issue)
            'txtAmount.Text = CDbl(dtrd("Qty") * dtrd("UnitCost"))
            txtAmount.Text = CDec(Format(dtrd("Qty") * dtrd("UnitCost"), "###0.00"))
            txtGST = dgItem.FindControl("txtGSTAmt")
            txtGSTAmt = dgItem.FindControl("txtGSTAmt")

            'If "Non-GST" Then
            'Inserted amount for GST would be in percentage
            If txtGST.Text = "" Then txtGST.Text = 0
            If txtAmount.Text <> 0 Then
                '2015-06-22: CH: Rounding issue (Prod issue)
                'hidTaxPerc.Text = (CDec(txtGST.Text) * 100) / CDec(txtAmount.Text)
                hidTaxPerc.Text = CDec(Format((CDec(txtGST.Text) * 100) / CDec(txtAmount.Text), "###0.00"))
            Else
                txtGST.Text = ""
            End If
            If txtGST.Text = "" Then
                dtrd("GST") = 0
                dtrd("TaxValue") = 0
            Else
                'dtrd("GST") = CDbl(txtGST.Text)
                dtrd("GST") = CDbl(hidTaxPerc.Text)
                dtrd("TaxValue") = txtGST.Text
            End If
            'Else

            'Inserted amount for GST would be in percentage
            If txtAmount.Text <> 0 Then

                'Jules 2018.07.23 - Removed checking for index = 0 (index = 0 value is STD)
                'If Not ddlGST.SelectedItem.Text.Contains("EX") And Not ddlGST.SelectedItem.Text.Contains("ZERO") And Not ddlGST.SelectedIndex = 0 Then
                If ddlGST.Enabled AndAlso Not ddlGST.SelectedItem.Text.Contains("EX") AndAlso Not ddlGST.SelectedItem.Text.Contains("ZERO") Then
                    '2015-06-22: CH: Rounding issue (Prod issue)
                    'hidTaxPerc.Text = (CDec(ddlGST.SelectedItem.Text.Split("(")(1).Replace("%", "").Replace(")", "").Trim) / 100) * CDec(txtAmount.Text)
                    hidTaxPerc.Text = CDec(Format((CDec(ddlGST.SelectedItem.Text.Split("(")(1).Replace("%", "").Replace(")", "").Trim) / 100) * CDec(txtAmount.Text), "###0.00"))
                    dtrd("GST") = CDec(ddlGST.SelectedItem.Text.Split("(")(1).Replace("%", "").Replace(")", "").Trim)
                Else
                    hidTaxPerc.Text = 0.0
                End If
            Else
                hidTaxPerc.Text = 0.0
            End If
            If hidTaxPerc.Text = "" Then
                dtrd("GSTTaxAmount") = 0
            Else
                dtrd("GSTTaxAmount") = CDbl(hidTaxPerc.Text)
            End If
            'End If           

            'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
            cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")
            dtrd("GSTTaxCode") = cboGSTTaxCode.SelectedValue

            'ViewState("POCost") = ViewState("POCost") + CDbl(hidTaxPerc.Text * txtAmount.Text / 100) + (dtrd("Qty") * dtrd("UnitCost"))
            '2015-06-22: CH: Rounding issue (Prod issue)
            'ViewState("POCost") = ViewState("POCost") + CDbl(hidTaxPerc.Text) + (dtrd("Qty") * dtrd("UnitCost"))
            ViewState("POCost") = ViewState("POCost") + CDec(hidTaxPerc.Text) + CDec(txtAmount.Text)
            decItemPrice += CDec(hidTaxPerc.Text) + CDec(txtAmount.Text) 'Jules 2019.01.07 - To ensure PO cost is accurate.

            If Not Hidden6.Value = "" Then
                gstReg = objDB.GetVal("SELECT IFNULL(CM_TAX_REG_NO, '') FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Hidden6.Value & "'")
            End If
            If gstReg = "" Then
                dtrd("SelectedGST") = "N/A"
            Else
                dtrd("SelectedGST") = ddlGST.SelectedValue
            End If

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

            'Jules 2018.07.31 - Set to N/A
            'dtrd("CategoryCode") = IIf(dgItem.Cells(EnumShoppingCart.icCategoryCode).Text = "&nbsp;", "", dgItem.Cells(EnumShoppingCart.icCategoryCode).Text)
            dtrd("CategoryCode") = IIf(dgItem.Cells(EnumShoppingCart.icCategoryCode).Text = "&nbsp;" Or dgItem.Cells(EnumShoppingCart.icCategoryCode).Text = "N/A", "", dgItem.Cells(EnumShoppingCart.icCategoryCode).Text)
            'End modification.

            'dtrd("GLCode") = IIf(dgItem.Cells(EnumShoppingCart.icGLCode).Text = "&nbsp;", "", dgItem.Cells(EnumShoppingCart.icGLCode).Text) 'Jules 2018.10.24 U00019

            'Jules 2018.10.18
            'Jules 2018.07.14 - PAMB Scrum 3                       
            'Dim cboGift, cboFundType, cboPersonCode, cboProjectCode As DropDownList
            Dim cboGift As DropDownList
            cboGift = dgItem.FindControl("cboGift")
            dtrd("Gift") = cboGift.SelectedItem.Value

            'cboFundType = dgItem.FindControl("cboFundType")
            'dtrd("FundType") = cboFundType.SelectedItem.Value

            'cboPersonCode = dgItem.FindControl("cboPersonCode")
            'dtrd("PersonCode") = cboPersonCode.SelectedItem.Value

            'cboProjectCode = dgItem.FindControl("cboProjectCode")
            'dtrd("ProjectCode") = cboProjectCode.SelectedItem.Value
            Dim hidFundType, hidPersonCode, hidProject, hidGLCode As TextBox
            hidFundType = dgItem.FindControl("hidFundType")
            dtrd("FundType") = hidFundType.Text

            hidPersonCode = dgItem.FindControl("hidPersonCode")
            dtrd("PersonCode") = hidPersonCode.Text

            hidProject = dgItem.FindControl("hidProjectCode")
            dtrd("ProjectCode") = hidProject.Text

            hidGLCode = dgItem.FindControl("hidGLCode")
            dtrd("GLCode") = hidGLCode.Text
            'End modification.

            Dim txtWarranty As TextBox
            txtWarranty = dgItem.FindControl("txtWarranty")
            If IsNumeric(txtWarranty.Text) Then
                dtrd("WarrantyTerms") = CInt(txtWarranty.Text)
            Else
                dtrd("WarrantyTerms") = 0
            End If

            'Jules 2018.10.24 U00019
            'Dim cboGLCode, cboCategoryCode As DropDownList
            'cboGLCode = dgItem.FindControl("cboGLCode")
            'If cboGLCode.Items.Count > 0 Then
            '    dtrd("GLCode") = cboGLCode.SelectedItem.Value
            'Else
            '    dtrd("GLCode") = ""
            'End If
            Dim cboCategoryCode As DropDownList
            'End modification.

            cboCategoryCode = dgItem.FindControl("cboCategoryCode")
            If cboCategoryCode.Items.Count > 0 Then
                dtrd("CategoryCode") = cboCategoryCode.SelectedItem.Value
            Else
                dtrd("CategoryCode") = ""
            End If

            Dim Asset As New PurchaseOrder_Buyer
            If Asset.AssetGroupMstr = True Then
                Dim cboAssetGroup As DropDownList
                cboAssetGroup = dgItem.FindControl("cboAssetGroup")
                dtrd("AssetGroup") = cboAssetGroup.SelectedItem.Value

                Dim hidAssetGroupNo As TextBox
                hidAssetGroupNo = dgItem.FindControl("hidAssetGroupNo")
                If hidAssetGroupNo.Text <> "" And hidAssetGroupNo.Text <> "&nbsp;" Then
                    dtrd("AssetGroupNo") = hidAssetGroupNo.Text
                Else
                    dtrd("AssetGroupNo") = ""
                End If
            Else
                dtrd("AssetGroup") = ""
                dtrd("AssetGroupNo") = ""
            End If

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

            If dgItem.Cells(EnumShoppingCart.icSource).Text = "&nbsp;" Then
                dtrd("Source") = ""
            Else
                dtrd("Source") = dgItem.Cells(EnumShoppingCart.icSource).Text
            End If

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

        'Jules 2019.01.07 - To ensure PO cost is accurate.
        If CDec(ViewState("POCost")) <> decItemPrice Then
            dtr("POCost") = decItemPrice + CDec(dtr("ShipAmt"))
            ViewState("POCost") = decItemPrice + CDec(dtr("ShipAmt"))
        Else
            dtr("POCost") = CDbl(ViewState("POCost") + dtr("ShipAmt"))
        End If
        'End modification.

        dtMaster.Rows.Add(dtr)
        ds.Tables.Add(dtMaster)
        ds.Tables.Add(dtDetails)

        If ViewState("type") = "new" Or ViewState("type") = "mod" Then
            Select Case ViewState("mode")
                Case "cc"
                Case "po"
                Case "rfq"
            End Select
        End If

        If (ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr") Or ViewState("modeRFQFromPR_Index") <> "" Then
            Dim dtCustomMaster As New DataTable
            Dim objAdmin As New Admin
            Dim strPRIndex As String

            If ViewState("modeRFQFromPR_Index") <> "" Then
                dvwCus = objAdmin.getCustomFieldPR("", "PR", ViewState("modeRFQFromPR_Index"))
            Else
                dvwCus = objAdmin.getCustomField("", "PO")
            End If

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

            ' columns for PR_CUSTOM_FIELD_DETAILS
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
        End If

        'End If
        bindPO = ds
    End Function

    Private Sub SavePO()
        Dim dsPO As New DataSet
        Dim objPO As New PurchaseOrder
        Dim objPO1 As New PurchaseOrder_Buyer
        Dim strMsg As String = ""
        blnValid = True
        blnItem = False

        If validateDatagrid(strMsg) AndAlso validateSSTRates(strMsg) Then 'Jules 2018.10.08 - SST
            dsPO = bindPO()
            If ViewState("type") = "new" Then
                Dim strNewPO As String
                Dim intMsg As Integer

                If blnValid Then
                    If ViewState("modePR") = "pr" Then
                        If ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr" Then
                            intMsg = objPO1.insertPO(dsPO, strNewPO, True)
                        Else
                            intMsg = objPO1.insertPO(dsPO, strNewPO, False)
                        End If
                    Else
                        If ViewState("ListingFromRFQ") <> "True" And ViewState("mode") <> "rfq" Then
                            intMsg = objPO1.insertPO(dsPO, strNewPO, True, True, True)
                        Else
                            intMsg = objPO1.insertPO(dsPO, strNewPO, False)
                        End If
                    End If
                    objPO = Nothing

                    Select Case intMsg
                        Case WheelMsgNum.Save
                            If Session("urlreferer") = "" Then
                                Common.NetMsgbox(Me, "Purchase Order Number " & strNewPO & " has been created.", dDispatcher.direct("PR", "viewShoppingCart.aspx", "type=tab&pageid=" & strPageId))
                            Else
                                If Session("urlreferer") = "RFQComSummary" Then
                                    Common.NetMsgbox(Me, "Purchase Order Number " & strNewPO & " has been created.", dDispatcher.direct("RFQ", "RFQ_List.aspx", "type=tab&pageid=" & strPageId))
                                ElseIf Session("urlreferer") = "ConCatSearch" Then
                                    Common.NetMsgbox(Me, "Purchase Order Number " & strNewPO & " has been created.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                                Else
                                    Common.NetMsgbox(Me, "Purchase Order Number " & strNewPO & " has been created.", dDispatcher.direct("PO", "RaiseFFPO" & ".aspx", "type=tab&pageid=" & strPageId))
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
                            Common.NetMsgbox(Me, "Company is currently inactive.", MsgBoxStyle.Information)
                        Case -2
                            Common.NetMsgbox(Me, "Company is being deleted.", MsgBoxStyle.Information)
                    End Select
                End If
                Select Case ViewState("mode")
                    Case "po"
                    Case "rfq"
                End Select
            ElseIf ViewState("type") = "mod" Then
                If blnValid Then
                    If ViewState("modePR") = "pr" Then
                        If ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr" Then
                            objPO1.updatePO(dsPO, True)
                        Else
                            objPO1.updatePO(dsPO, False)
                        End If
                    Else
                        If ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr" Then
                            objPO1.updatePO(dsPO, True)
                        Else
                            objPO1.updatePO(dsPO, False)
                        End If
                    End If
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
            Dim chkItem As CheckBox
            Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark, txtGSTAmt, txtGSTAmount As TextBox
            Dim lblItemLine, txtBudget, txtDelivery As Label
            'Dim cboGLCode, cboCust, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList
            Dim cboCust, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList 'Jules 2018.10.24 U00019
            Dim cboGift As DropDownList ', cboFundType, cboPersonCode, cboProjectCode As DropDownList 'Jules 2018.07.14 s- PAMB; Jules 2018.10.18
            Dim hidFundType, hidPersonCode, hidProjectCode, hidGLCode As TextBox 'Jules 2018.10.18
            Dim strItem, strItemCust As New ArrayList()
            Dim dgItem As DataGridItem
            Dim dgItem2 As DataGridItem
            Dim k As Integer = 0
            Dim strItemHead As New ArrayList()

            strItemHead.Add(New String() {txtAttention.Text, txtInternal.Text, txtExternal.Text, cboBillCode.SelectedIndex, cboShipmentTerm.SelectedIndex, cboShipmentMode.SelectedIndex, txtShipVia.Text, chkUrgent.Checked, txtShippingHandling.Text, Session("venId"), cboPayTerm.SelectedIndex, cboPayMethod.SelectedIndex, lblCurrencyCode.Text})

            For Each dgItem In dtgShopping.Items
                txtQty = dgItem.FindControl("txtQty")
                'cboGLCode = dgItem.FindControl("cboGLCode") 'Jules 2018.10.24 U00019
                txtPrice = dgItem.FindControl("txtPrice")
                txtBudget = dgItem.FindControl("txtBudget")
                hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                txtDelivery = dgItem.FindControl("txtDelivery")
                hidDelCode = dgItem.FindControl("hidDelCode")
                txtEstDate = dgItem.FindControl("txtEstDate")
                txtWarranty = dgItem.FindControl("txtWarranty")
                txtRemark = dgItem.FindControl("txtRemark")
                cboCategoryCode = dgItem.FindControl("cboCategoryCode")
                cboUOM = dgItem.FindControl("ddl_uom")
                cboAssetGroup = dgItem.FindControl("cboAssetGroup")
                txtGSTAmt = dgItem.FindControl("txtGSTAmt")
                ddlGST = dgItem.FindControl("ddlGST")
                txtGSTAmount = dgItem.FindControl("txtGSTAmount")
                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")

                'Jules 2018.07.14 - PAMB
                cboGift = dgItem.FindControl("cboGift")

                'Jules 2018.10.18
                'cboFundType = dgItem.FindControl("cboFundType")
                'cboPersonCode = dgItem.FindControl("cboPersonCode")
                'cboProjectCode = dgItem.FindControl("cboProjectCode")
                hidFundType = dgItem.FindControl("hidFundType")
                hidPersonCode = dgItem.FindControl("hidPersonCode")
                hidProjectCode = dgItem.FindControl("hidProjectCode")
                hidGLCode = dgItem.FindControl("hidGLCode")
                'End modification.

                chkItem = dgItem.FindControl("chkSelection")
                'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})

                'Jules 2018.07.14 - PAMB
                'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedIndex})
                'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
                strItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18

                If (ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr") Or ViewState("modeRFQFromPR_Index") <> "" Then
                    Dim objAdmin As New Admin

                    If ViewState("modeRFQFromPR_Index") <> "" Then
                        dvwCus = objAdmin.getCustomFieldPR("", "PR", ViewState("modeRFQFromPR_Index"))
                    Else
                        dvwCus = objAdmin.getCustomField("", "PO")
                    End If

                    If Not dvwCus Is Nothing Then
                        For i = 0 To dvwCus.Count - 1
                            Dim cboCustom As DropDownList
                            cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                            If chkItem.Checked Then
                            Else
                                strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                            End If
                        Next
                    End If
                End If

                k += 1
            Next
            Session("strItem") = strItem
            Session("strItemCust") = strItemCust
            Session("strItemHead") = strItemHead

            Bindgrid()

            If strMsg <> "" Then
                lblMsg.Text = strMsg
            Else
                lblMsg.Text = ""
            End If
        End If
    End Sub

    Private Sub cmdRaise_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRaise.Click
        SavePO()
        If lblMsg.Text = "" Then
            Session("ProdList") = Nothing
            Session("aryProdCodeNew_All") = Nothing
            Session("keepAr") = Nothing
            Session("strItem") = Nothing
            Session("strItemCust") = Nothing
            Session("strItemHead") = Nothing
        End If
    End Sub

    Sub SubmitPO()
        Dim dt As New DataTable
        Dim objPO As New PurchaseOrder
        Dim objPO1 As New PurchaseOrder_Buyer
        Dim intIndex, intMsg As Integer

        Dim strRFQ_No, strPR_Index, strGrp_Index
        Dim strDept_Code As String = ""

        If ViewState("modePR") = "pr" And (ViewState("modeRFQFromPR_Index") <> "" Or ViewState("modeRFQFromPR_Index_draft") <> "") Then
            strPR_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRM_PR_INDEX),""'"")) AS CHAR(2000)) AS PRM_PR_INDEX FROM PR_DETAILS, PR_MSTR WHERE PRM_COY_ID = PRD_COY_ID AND PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND (PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "')")
            strGrp_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRA_APPROVAL_GRP_INDEX),""'"")) AS CHAR(2000)) AS PRA_APPROVAL_GRP_INDEX FROM PR_APPROVAL WHERE PRA_FOR = 'PR' AND PRA_PR_INDEX IN (" & strPR_Index & ")")
            strDept_Code = objDB.GetVal("SELECT IFNULL(CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",AGM_DEPT_CODE),""'"")) AS CHAR(2000)),'') AS AGM_DEPT_CODE FROM APPROVAL_GRP_MSTR WHERE AGM_DEPT_CODE IS NOT NULL AND AGM_DEPT_CODE <> '' AND AGM_GRP_INDEX IN (" & strGrp_Index & ")")
        ElseIf ViewState("modePR") = "pr" And (ViewState("modeRFQFromPR_Index") = "" Or ViewState("modeRFQFromPR_Index_draft") = "") Then
            Dim strPO_No As String = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRD_CONVERT_TO_DOC),""'"")) AS CHAR(2000)) AS PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("poid") & "'")
            strPR_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRM_PR_INDEX),""'"")) AS CHAR(2000)) AS PRM_PR_INDEX FROM PR_DETAILS, PR_MSTR WHERE PRM_COY_ID = PRD_COY_ID AND PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND (PRD_CONVERT_TO_DOC IN (" & strPO_No & "))")
            strGrp_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRA_APPROVAL_GRP_INDEX),""'"")) AS CHAR(2000)) AS PRA_APPROVAL_GRP_INDEX FROM PR_APPROVAL WHERE PRA_FOR = 'PR' AND PRA_PR_INDEX IN (" & strPR_Index & ")")
            strDept_Code = objDB.GetVal("SELECT IFNULL(CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",AGM_DEPT_CODE),""'"")) AS CHAR(2000)),'') AS AGM_DEPT_CODE FROM APPROVAL_GRP_MSTR WHERE AGM_DEPT_CODE IS NOT NULL AND AGM_DEPT_CODE <> '' AND AGM_GRP_INDEX IN (" & strGrp_Index & ")")
        End If

        dt = objPO1.getPOApprFlow(True, strDept_Code)

        If dt.Rows.Count = 0 Then
            Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
            Exit Sub
        ElseIf dt.Rows.Count > 1 Then
            Response.Redirect(dDispatcher.direct("PO", "POApprovalSetup.aspx", "pageid=" & strPageId & "&mode=" & ViewState("mode") & "&msg=0&type=" & ViewState("type") & "&poid=" & lblPONo.Text & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.00") & "&Frm=" & Me.Request.QueryString("Frm") & "&dpage=" & Request.QueryString("dpage") & "&currency=" & ViewState("Currency") & "&dept=" & strDept_Code & "&prindex=" & strPR_Index))
            Exit Sub
        Else
            intMsg = objPO1.submitPO(lblPONo.Text, dt, "0")
            Select Case intMsg
                Case WheelMsgNum.Save
                    If ViewState("modePR") = "pr" Then
                        intMsg = objPO1.updatePRStatus(strPR_Index, ViewState("msg"))
                    End If

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
            cboCountry.SelectedIndex = 0
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
        Dim sProductCode As Integer = 0
        Dim dbItemDesc As String = ""
        Dim strSQL As String
        Dim sPodIndex As Integer
        Dim strAryQuery(0) As String
        Dim ds As ArrayList = Nothing

        If Not Session("ProdList") Is Nothing Then
            ds = Session("ProdList")
            For i As Integer = 0 To ds.Count - 1
                dbItemDesc = ds(i)(1)
                sPodIndex = i + 1
                strSQL = objPO.updatePODIndex(hidNewPO.Value, sProductCode, sPodIndex, True, dbItemDesc)
                Common.Insert2Ary(strAryQuery, strSQL)
                objDB.BatchExecute(strAryQuery)
            Next
        End If

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
        Dim strSQL As String
        Dim sPodIndex As Integer
        Dim InternalCount As Integer = 0

        For row As Integer = 0 To CType(Session("ProdList"), ArrayList).Count - 1
            For Each dgItem In dtgShopping.Items
                If row = dgItem.ItemIndex Then
                    CType(Session("ProdList"), ArrayList)(row)(1) = CType(dgItem.FindControl("lblProductDesc"), TextBox).Text
                End If
            Next
        Next

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

                sPodIndex = CType(dgItem.FindControl("lblItemLine"), Label).Text

                If ViewState("type") = "new" Then
                    Select Case ViewState("mode")
                        Case "po", "cc"
                            strSQL = objPO1.deletePOItemSQL(0, 0, 0)
                        Case "rfq"
                            strSQL = objPO1.deleteRFQItemSQL(ViewState("rfqid"), sProductCode)
                    End Select
                ElseIf ViewState("type") = "mod" Then
                    strSQL = objPO1.deletePOItemSQL(hidNewPO.Value, 0, sPodIndex, True)
                    Select Case ViewState("mode")
                        Case "po"
                        Case "rfq"
                    End Select
                End If

                sClientId = Mid(sFullClientId, InStr(sFullClientId, "_") + 1, InStr(Mid(sFullClientId, InStr(sFullClientId, "_") + 1), "_") - 1) & "|"
                hidClientId.Value = hidClientId.Value.Replace(sClientId, "")
                hidTotalClientId.Value = hidTotalClientId.Value - 1
                Dim GetPO As Boolean
                GetPO = False
                If GetPO = False Then
                    If ViewState("mode") <> "cc" Then
                        Session("ProdList") = RemovePRProductCodeFromList(sProductCode, Session("ProdList"), i, sProductDesc, InternalCount)
                    End If
                End If
                Common.Insert2Ary(strAryQuery, strSQL)
            End If
            i = i + 1
        Next

        InternalCount = 0
        Session("CurrentScreen") = "RemoveItem"
        If Not strAryQuery(0) Is Nothing Then
            If objDB.BatchExecute(strAryQuery) Then
                Common.NetMsgbox(Me, "Item(s) has been removed.", MsgBoxStyle.Information)
                Session("ItemDeleted") = Nothing
                Session("ItemDeleted") = "Yes"
                Dim dsDelivery As New DataSet
                Dim objAdmin As New Admin
                Dim objBudget As New BudgetControl
                dsDelivery = objAdmin.PopulateAddr("D", "", "", "")
                dvwDelivery = dsDelivery.Tables(0).DefaultView
                strDeliveryDefault = objAdmin.user_Default_Add("D")
                Session("CurrentScreen") = "RemoveItem"

                Dim dsBCM As New DataSet
                dsBCM = objBudget.getBCMListByUser(Session("UserId"))
                dtBCM = dsBCM.Tables(0)

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
                                sProductDesc = CType(dgItem.FindControl("lblProductDesc"), TextBox).Text
                            Else
                                sProductCode = CType(dgItem.FindControl("lblProductCode"), Label).Text
                            End If

                            sPodIndex = CInt(dgItem.Cells(EnumShoppingCart.icNo).Text)


                            Dim GetPO As Boolean
                            GetPO = objPO1.get_PO(hidNewPO.Value, 0, sPodIndex, True)

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

                Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark, txtGSTAmt, lblProductDesc, txtGSTAmount As TextBox
                Dim lblItemLine, txtBudget, txtDelivery As Label
                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                'Dim cboGLCode, cboCust, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList
                Dim cboCust, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList 'Jules 2018.10.24 U00019
                Dim strItem, strItemCust As New ArrayList()
                Dim dgItem2 As DataGridItem
                Dim k As Integer = 0
                Dim strItemHead As New ArrayList()
                Dim cboGift As DropDownList ', cboFundType, cboPersonCode, cboProjectCode As DropDownList 'Jules 2018.10.18
                Dim hidFundType, hidPersonCode, hidProjectCode, hidGLCode As TextBox 'Jules 2018.10.18

                strItemHead.Add(New String() {txtAttention.Text, txtInternal.Text, txtExternal.Text, cboBillCode.SelectedIndex, cboShipmentTerm.SelectedIndex, cboShipmentMode.SelectedIndex, txtShipVia.Text, chkUrgent.Checked, txtShippingHandling.Text, Session("venId"), cboPayTerm.SelectedIndex, cboPayMethod.SelectedIndex, lblCurrencyCode.Text})

                For Each dgItem In dtgShopping.Items
                    txtQty = dgItem.FindControl("txtQty")
                    'cboGLCode = dgItem.FindControl("cboGLCode") 'Jules 2018.10.24 U00019
                    txtPrice = dgItem.FindControl("txtPrice")
                    txtBudget = dgItem.FindControl("txtBudget")
                    hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                    txtDelivery = dgItem.FindControl("txtDelivery")
                    hidDelCode = dgItem.FindControl("hidDelCode")
                    txtEstDate = dgItem.FindControl("txtEstDate")
                    txtWarranty = dgItem.FindControl("txtWarranty")
                    txtRemark = dgItem.FindControl("txtRemark")
                    cboCategoryCode = dgItem.FindControl("cboCategoryCode")
                    cboUOM = dgItem.FindControl("ddl_uom")
                    cboAssetGroup = dgItem.FindControl("cboAssetGroup")
                    txtGSTAmt = dgItem.FindControl("txtGSTAmt")
                    lblProductDesc = dgItem.FindControl("lblProductDesc")
                    ddlGST = dgItem.FindControl("ddlGST")
                    txtGSTAmount = dgItem.FindControl("txtGSTAmount")
                    'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                    cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")

                    'Jules 2018.07.14 - PAMB
                    cboGift = dgItem.FindControl("cboGift")

                    'Jules 2018.10.18
                    'cboFundType = dgItem.FindControl("cboFundType")
                    'cboPersonCode = dgItem.FindControl("cboPersonCode")
                    'cboProjectCode = dgItem.FindControl("cboProjectCode")
                    hidFundType = dgItem.FindControl("hidFundType")
                    hidPersonCode = dgItem.FindControl("hidPersonCode")
                    hidProjectCode = dgItem.FindControl("hidProjectCode")
                    hidGLCode = dgItem.FindControl("hidGLCode")
                    'End modification.

                    chkItem = dgItem.FindControl("chkSelection")
                    If Not chkItem.Checked Then
                        'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                        'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text})

                        'Jules 2018.07.14
                        'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedIndex})
                        'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
                        strItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18
                    End If

                    If (ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr") Or ViewState("modeRFQFromPR_Index") <> "" Then

                        If ViewState("modeRFQFromPR_Index") <> "" Then
                            dvwCus = objAdmin.getCustomFieldPR("", "PR", ViewState("modeRFQFromPR_Index"))
                        Else
                            dvwCus = objAdmin.getCustomField("", "PO")
                        End If

                        If Not dvwCus Is Nothing Then
                            For i = 0 To dvwCus.Count - 1
                                Dim cboCustom As DropDownList
                                cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                                If chkItem.Checked Then
                                Else
                                    strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                                End If
                            Next
                        End If
                    End If

                    k += 1
                Next
                Session("strItem") = strItem
                Session("strItemCust") = strItemCust
                Session("strItemHead") = strItemHead


                hidTotal.Value = ""
                hid2.Value = ""
                objAdmin = Nothing
                objBudget = Nothing
            End If
        Else
            If Session("ProdList") Is Nothing Then
                Throw New Exception
            End If
            Session("CurrentScreen") = "RemoveItem"
            Session("ItemDeleted") = Nothing
            Session("ItemDeleted") = "Yes"
        End If

        objPO = Nothing
        If ViewState("poid") Is Nothing And Not lblPONo.Text.Trim.Contains("To") Then
            ViewState("poid") = lblPONo.Text
        End If
        updatePODIdx()
        Dim type As String = ""
        If ViewState("type") = "new" Then
            type = "new"
        Else
            type = "mod"
        End If
        Session("VendorID") = Hidden6.Value
        Dim redirect As String
        redirect = dDispatcher.direct("PO", "RaiseFFPO.aspx", "pageid=" & Request.QueryString("pageid") & "&index=" & Request.QueryString("index") & "&poid=" & lblPONo.Text & "&mode=po&type=" & type)
        Response.Redirect(redirect)

    End Sub
    Public Function RemovePRProductCodeFromList(ByVal sProductCode As String, ByVal aryProdCode As ArrayList, ByVal sProductIndex As Integer, Optional ByVal sProductDesc As String = "", Optional ByRef InternalCount As Integer = 0) As ArrayList
        Dim i As Integer
        If sProductCode = "" Then
            For i = 0 To aryProdCode.Count - 1
                'check deleted item index aganist line no 
                If aryProdCode(i)(0) = sProductIndex + 1 Then
                    aryProdCode.RemoveAt(i)
                    RemovePRProductCodeFromList = aryProdCode
                    Exit For
                End If
            Next
        Else
            For i = 0 To aryProdCode.Count - 1
                If aryProdCode(i)(0) = sProductCode AndAlso i = sProductIndex - InternalCount Then
                    aryProdCode.RemoveAt(i)
                    RemovePRProductCodeFromList = aryProdCode
                    InternalCount = InternalCount + 1
                    Exit For
                End If
            Next
        End If
        RemovePRProductCodeFromList = aryProdCode
    End Function
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
        ViewState("_isDuplicated") = "Yes"
        Try
            If ViewState("type") = "new" Then

                Select Case ViewState("mode")
                    Case "cc"
                        For Each dgItem In dtgShopping.Items
                            chkItem = dgItem.FindControl("chkSelection")
                            If chkItem.Checked Then

                                Dim aryProdCodeCurrent As ArrayList = Session("ProdList")
                                Session("ProdList") = aryProdCodeCurrent
                                Exit For
                            End If
                        Next
                    Case "po", "rfq"
                        Dim o = dtgShopping.Items.Count
                        Dim aryProdCodeCurrent As ArrayList = Session("ProdList")
                        Dim intItem As Integer = aryProdCodeCurrent.Count 'CH - 1 Oct 2015: Get count of item
                        For Each dgItem In dtgShopping.Items
                            chkItem = dgItem.FindControl("chkSelection")
                            aryProdCodeCurrent(dgItem.ItemIndex)(1) = CType(dgItem.FindControl("lblProductDesc"), TextBox).Text
                            If chkItem.Checked Then
                                intItem = intItem + 1
                                aryProdCodeCurrent.Add(New String() {intItem, CType(dgItem.FindControl("lblProductDesc"), TextBox).Text, 0, "0", ""})
                                'aryProdCodeCurrent.Add(New String() {"", CType(dgItem.FindControl("lblProductDesc"), TextBox).Text, 0, "0", ""})
                                Session("ProdList") = aryProdCodeCurrent
                            End If
                        Next
                End Select

                Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark, txtGSTAmt, txtGSTAmount As TextBox
                Dim lblItemLine, txtBudget, txtDelivery As Label
                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                'Dim cboGLCode, cboCust, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList
                Dim cboCust, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList 'Jules 2018.10.24 U00019
                Dim strItem, strItemCust, strDuplicatedItem, strDuplicatedItemCust As New ArrayList()
                Dim dgItem2 As DataGridItem
                Dim k As Integer = 0
                Dim cboGift As DropDownList ', cboFundType, cboPersonCode, cboProjectCode As DropDownList 'Jules 2018.07.14 PAMB; Jules 2018.10.18
                Dim hidFundtype, hidPersonCode, hidProjectCode, hidGLCode As TextBox 'Jules 2018.10.18

                For Each dgItem In dtgShopping.Items
                    txtQty = dgItem.FindControl("txtQty")
                    'cboGLCode = dgItem.FindControl("cboGLCode") 'Jules 2018.10.24 U00019
                    cboCategoryCode = dgItem.FindControl("cboCategoryCode")
                    txtPrice = dgItem.FindControl("txtPrice")
                    txtBudget = dgItem.FindControl("txtBudget")
                    hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                    txtDelivery = dgItem.FindControl("txtDelivery")
                    hidDelCode = dgItem.FindControl("hidDelCode")
                    txtEstDate = dgItem.FindControl("txtEstDate")
                    txtWarranty = dgItem.FindControl("txtWarranty")
                    txtRemark = dgItem.FindControl("txtRemark")
                    cboUOM = dgItem.FindControl("ddl_uom")
                    cboAssetGroup = dgItem.FindControl("cboAssetGroup")
                    txtGSTAmt = dgItem.FindControl("txtGSTAmt")
                    ddlGST = dgItem.FindControl("ddlGST")
                    txtGSTAmount = dgItem.FindControl("txtGSTAmount")
                    'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                    cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")

                    'Jules 2018.07.14 - PAMB
                    cboGift = dgItem.FindControl("cboGift")

                    'Jules 2018.10.18
                    'cboFundType = dgItem.FindControl("cboFundType")
                    'cboPersonCode = dgItem.FindControl("cboPersonCode")
                    'cboProjectCode = dgItem.FindControl("cboProjectCode")
                    hidFundtype = dgItem.FindControl("hidFundType")
                    hidPersonCode = dgItem.FindControl("hidPersonCode")
                    hidProjectCode = dgItem.FindControl("hidProjectCode")
                    hidGLCode = dgItem.FindControl("hidGLCode")
                    'End modification.

                    chkItem = dgItem.FindControl("chkSelection")
                    If chkItem.Checked Then
                        'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                        'strDuplicatedItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text})

                        'Jules 2018.07.14 - PAMB
                        'strDuplicatedItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedIndex})
                        'strDuplicatedItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
                        strDuplicatedItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, hidFundtype.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18
                    End If
                    'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                    'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text})

                    'Jules 2018.07.14 - PAMB
                    'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedIndex})
                    'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
                    strItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, hidFundtype.Text, hidPersonCode.Text, hidProjectCode.Text})

                    If (ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr") Or ViewState("modeRFQFromPR_Index") <> "" Then
                        Dim objAdmin As New Admin

                        If ViewState("modeRFQFromPR_Index") <> "" Then
                            dvwCus = objAdmin.getCustomFieldPR("", "PR", ViewState("modeRFQFromPR_Index"))
                        Else
                            dvwCus = objAdmin.getCustomField("", "PO")
                        End If

                        If Not dvwCus Is Nothing Then
                            For i = 0 To dvwCus.Count - 1
                                Dim cboCustom As DropDownList
                                cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)
                                If chkItem.Checked Then
                                    strDuplicatedItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                                End If
                                strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                            Next
                        End If
                    End If

                    k += 1
                Next
                ViewState("strDuplicatedItem") = strDuplicatedItem
                Session("strItem") = strItem
                Session("strItemCust") = strItemCust
                ViewState("strDuplicatedCustItem") = strDuplicatedItemCust
                Bindgrid()
            ElseIf ViewState("type") = "mod" Then
                Select Case ViewState("mode")
                    Case "cc"
                        For Each dgItem In dtgShopping.Items
                            chkItem = dgItem.FindControl("chkSelection")
                            If chkItem.Checked Then
                                Session("CurrentScreen") = "AddItem"
                                Dim aryProdCodeCurrent As ArrayList = Session("ProdList")
                                Session("ProdList") = aryProdCodeCurrent
                                Exit For
                            End If
                        Next
                    Case "po", "rfq"
                        For Each dgItem In dtgShopping.Items
                            Dim aryProdCodeCurrent As ArrayList = Session("ProdList")
                            chkItem = dgItem.FindControl("chkSelection")
                            aryProdCodeCurrent(dgItem.ItemIndex)(1) = CType(dgItem.FindControl("lblProductDesc"), TextBox).Text
                            If chkItem.Checked Then
                                Session("CurrentScreen") = "AddItem"
                                aryProdCodeCurrent.Add(New String() {"", CType(dgItem.FindControl("lblProductDesc"), TextBox).Text, 0, "0", ""})
                                Session("ProdList") = aryProdCodeCurrent
                            End If
                        Next
                End Select
                Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark, txtGSTAmt, txtGSTAmount As TextBox
                Dim lblItemLine, txtBudget, txtDelivery As Label
                'Dim cboGLCode, cboCust, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList
                Dim cboCust, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList 'Jules 2018.10.24 U00019
                Dim cboGift As DropDownList ', cboFundType, cboPersonCode, cboProjectCode As DropDownList 'Jules 2018.07.14; Jules 2018.10.18
                Dim hidFundType, hidPersonCode, hidProjectCode, hidGLCode As TextBox 'Jules 2018.10.18
                Dim strItem, strItemCust, strDuplicatedItem, strDuplicatedItemCust As New ArrayList()
                Dim dgItem2 As DataGridItem
                Dim k As Integer = 0
                strscript.Append("<script language=""javascript"">")
                strscript.Append("document.getElementById('btnHidden1').click();")
                strscript.Append("</script>")
                RegisterStartupScript("script4", strscript.ToString())
                For Each dgItem In dtgShopping.Items
                    txtQty = dgItem.FindControl("txtQty")
                    'cboGLCode = dgItem.FindControl("cboGLCode") 'Jules 2018.10.24 U00019
                    cboCategoryCode = dgItem.FindControl("cboCategoryCode")
                    txtPrice = dgItem.FindControl("txtPrice")
                    txtBudget = dgItem.FindControl("txtBudget")
                    hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                    txtDelivery = dgItem.FindControl("txtDelivery")
                    hidDelCode = dgItem.FindControl("hidDelCode")
                    txtEstDate = dgItem.FindControl("txtEstDate")
                    txtWarranty = dgItem.FindControl("txtWarranty")
                    txtRemark = dgItem.FindControl("txtRemark")
                    cboUOM = dgItem.FindControl("ddl_uom")
                    cboAssetGroup = dgItem.FindControl("cboAssetGroup")
                    txtGSTAmt = dgItem.FindControl("txtGSTAmt")
                    ddlGST = dgItem.FindControl("ddlGST")
                    txtGSTAmount = dgItem.FindControl("txtGSTAmount")
                    'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                    cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")

                    'Jules 2018.07.14 - PAMB
                    cboGift = dgItem.FindControl("cboGift")

                    'Jules 2018.10.18
                    'cboFundType = dgItem.FindControl("cboFundType")
                    'cboPersonCode = dgItem.FindControl("cboPersonCode")
                    'cboProjectCode = dgItem.FindControl("cboProjectCode")
                    hidFundType = dgItem.FindControl("hidFundType")
                    hidPersonCode = dgItem.FindControl("hidPersonCode")
                    hidProjectCode = dgItem.FindControl("hidProjectCode")
                    hidGLCode = dgItem.FindControl("hidGLCode")
                    'End modification.

                    chkItem = dgItem.FindControl("chkSelection")
                    If chkItem.Checked Then
                        'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                        'strDuplicatedItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text})

                        'Jules 2018.07.14
                        'strDuplicatedItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedIndex})
                        'strDuplicatedItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
                        strDuplicatedItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18

                    End If

                    'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                    'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text})

                    'Jules 2018.07.14 - PAMB
                    'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedIndex})
                    'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
                    strItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18

                    If (ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr") Or ViewState("modeRFQFromPR_Index") <> "" Then
                        Dim objAdmin As New Admin

                        If ViewState("modeRFQFromPR_Index") <> "" Then
                            dvwCus = objAdmin.getCustomFieldPR("", "PR", ViewState("modeRFQFromPR_Index"))
                        Else
                            dvwCus = objAdmin.getCustomField("", "PO")
                        End If

                        If Not dvwCus Is Nothing Then
                            For i = 0 To dvwCus.Count - 1
                                Dim cboCustom As DropDownList
                                cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                                If chkItem.Checked Then
                                    strDuplicatedItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                                End If

                                strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                            Next
                        End If
                    End If

                    k += 1
                Next
                ViewState("strDuplicatedItem") = strDuplicatedItem
                Session("strItem") = strItem
                Session("strItemCust") = strItemCust
                ViewState("strDuplicatedCustItem") = strDuplicatedItemCust

            End If
            objPO = Nothing
        Catch ex As Exception

        End Try
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
            strMsg = "You are not assigned to any Budget Account Code. ""&vbCrLf&""Please contact the Finance Manager. "
        End If

        If ViewState("blnBill") = 0 Then
            If strMsg <> "" Then
                strMsg &= """&vbCrLf&"""
            End If
            strMsg &= "Please ask your Buyer Admin to add in a Billing Address! "
        End If

        If strMsg <> "" Then
            checkMandatory = False
        Else
            checkMandatory = True
        End If
        objPR = Nothing
    End Function

    Private Sub cmdSetup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSetup.Click
        If Page.IsValid Then
            Dim strMsg As String = ""
            If validateDatagrid(strMsg) AndAlso validateSSTRates(strMsg) Then 'Jules 2018.09.12 - SST
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
                            intMsg = objPR.insertPR(dsPR, strNewPO, True)
                            Select Case intMsg
                                Case WheelMsgNum.Save
                                    If blnItem Then ' item exists
                                        Response.Redirect(dDispatcher.direct("PO", "POApprovalSetup.aspx", "msg=0&poid=" & strNewPO & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId))
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
                                        Response.Redirect(dDispatcher.direct("PO", "ExceedBCMPO.aspx", "pageid=" & strPageId & "&poid=" & hidNewPO.Value & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.0000") & "&currency=" & ViewState("Currency")))
                                    Else
                                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                                    End If
                                Else
                                    Response.Redirect(dDispatcher.direct("PO", "POApprovalSetup.aspx", "msg=0&poid=" & hidNewPO.Value & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId))
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

    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        If File1.Value <> "" Then
            Dim objFile As New FileManagement
            Dim objPR As New PR

            ' Restrict user upload size
            Dim dblMaxFileSize As Double = Session("FileSize")

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(File1.PostedFile.FileName)

            'Jules 2018.09.13 - Increase length from 46 to 200.
            If Len(sFileName) > 205 Then
                Common.NetMsgbox(Me, "File name exceeds 200 characters")
            ElseIf File1.PostedFile.ContentLength > 0 And File1.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                If ViewState("type") = "new" Then
                    'Jules 2018.03.21 - To avoid missing attachment issue.
                    'objFile.FileUpload(File1, EnumUploadType.DocAttachmentTemp, "PO", EnumUploadFrom.FrontOff, Session.SessionID)
                    objFile.FileUpload(File1, EnumUploadType.DocAttachment, "PO", EnumUploadFrom.FrontOff, Session.SessionID)
                    Select Case ViewState("mode")
                        Case "po"
                        Case "rfq"
                    End Select
                ElseIf ViewState("type") = "mod" Then
                    'Jules 2018.03.21 - To avoid missing attachment issue.
                    'objFile.FileUpload(File1, EnumUploadType.DocAttachmentTemp, "PO", EnumUploadFrom.FrontOff, ViewState("poid"))
                    objFile.FileUpload(File1, EnumUploadType.DocAttachment, "PO", EnumUploadFrom.FrontOff, ViewState("poid"))
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

        Dim chkItem As CheckBox
        Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark, txtGSTAmt, txtGSTAmount As TextBox
        Dim lblItemLine, txtBudget, txtDelivery As Label
        'Dim cboGLCode, cboCust, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList
        Dim cboCust, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList 'Jules 2018.10.24
        Dim cboGift As DropDownList ', cboFundType, cboPersonCode, cboProjectCode As DropDownList 'Jules 2018.07.14 - PAMB; Jules 2018.10.18
        Dim hidFundType, hidPersonCode, hidProjectCode, hidGLCode As TextBox 'Jules 2018.10.18
        Dim strItem, strItemCust As New ArrayList()
        Dim dgItem As DataGridItem
        Dim dgItem2 As DataGridItem
        Dim k As Integer = 0
        Dim strItemHead As New ArrayList()

        strItemHead.Add(New String() {txtAttention.Text, txtInternal.Text, txtExternal.Text, cboBillCode.SelectedIndex, cboShipmentTerm.SelectedIndex, cboShipmentMode.SelectedIndex, txtShipVia.Text, chkUrgent.Checked, txtShippingHandling.Text, Session("venId"), cboPayTerm.SelectedIndex, cboPayMethod.SelectedIndex, lblCurrencyCode.Text})

        For Each dgItem In dtgShopping.Items
            txtQty = dgItem.FindControl("txtQty")
            'cboGLCode = dgItem.FindControl("cboGLCode") 'Jules 2018.10.24 U00019
            txtPrice = dgItem.FindControl("txtPrice")
            txtBudget = dgItem.FindControl("txtBudget")
            hidBudgetCode = dgItem.FindControl("hidBudgetCode")
            txtDelivery = dgItem.FindControl("txtDelivery")
            hidDelCode = dgItem.FindControl("hidDelCode")
            txtEstDate = dgItem.FindControl("txtEstDate")
            txtWarranty = dgItem.FindControl("txtWarranty")
            txtRemark = dgItem.FindControl("txtRemark")
            cboCategoryCode = dgItem.FindControl("cboCategoryCode")
            cboUOM = dgItem.FindControl("ddl_uom")
            cboAssetGroup = dgItem.FindControl("cboAssetGroup")
            txtGSTAmt = dgItem.FindControl("txtGSTAmt")
            ddlGST = dgItem.FindControl("ddlGST")
            txtGSTAmount = dgItem.FindControl("txtGSTAmount")
            'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
            cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")

            'Jules 2018.07.14 - PAMB
            cboGift = dgItem.FindControl("cboGift")

            'Jules 2018.10.18
            'cboFundType = dgItem.FindControl("cboFundType")
            'cboPersonCode = dgItem.FindControl("cboPersonCode")
            'cboProjectCode = dgItem.FindControl("cboProjectCode")
            hidFundType = dgItem.FindControl("hidFundType")
            hidPersonCode = dgItem.FindControl("hidPersonCode")
            hidProjectCode = dgItem.FindControl("hidProjectCode")
            hidGLCode = dgItem.FindControl("hidGLCode")
            'End modification.

            chkItem = dgItem.FindControl("chkSelection")
            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})

            'Jules 2018.07.14 - PAMB
            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedIndex})
            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
            strItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18

            If (ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr") Or ViewState("modeRFQFromPR_Index") <> "" Then
                Dim objAdmin As New Admin

                If ViewState("modeRFQFromPR_Index") <> "" Then
                    dvwCus = objAdmin.getCustomFieldPR("", "PR", ViewState("modeRFQFromPR_Index"))
                Else
                    dvwCus = objAdmin.getCustomField("", "PO")
                End If

                If Not dvwCus Is Nothing Then
                    For i = 0 To dvwCus.Count - 1
                        Dim cboCustom As DropDownList
                        cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                        If chkItem.Checked Then
                        Else
                            strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                        End If
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

    Private Sub cmdUploadInt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUploadInt.Click
        If File1Int.Value <> "" Then
            Dim objFile As New FileManagement
            Dim objPR As New PR

            ' Restrict user upload size
            Dim dblMaxFileSize As Double = Session("FileSize")

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(File1Int.PostedFile.FileName)

            'Jules 2018.09.13 - Increase length from 46 to 200.
            If Len(sFileName) > 205 Then
                Common.NetMsgbox(Me, "File name exceeds 200 characters")
            ElseIf File1Int.PostedFile.ContentLength > 0 And File1Int.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                If ViewState("type") = "new" Then
                    'Jules 2019.03.21 - To avoid missing attachment issue.
                    'objFile.FileUpload(File1Int, EnumUploadType.DocAttachmentTemp, "PO", EnumUploadFrom.FrontOff, Session.SessionID, , , , , , "I")
                    objFile.FileUpload(File1Int, EnumUploadType.DocAttachment, "PO", EnumUploadFrom.FrontOff, Session.SessionID, , , , , , "I")
                    Select Case ViewState("mode")
                        Case "po"
                        Case "rfq"
                    End Select
                ElseIf ViewState("type") = "mod" Then
                    ''Jules 2019.03.21 - To avoid missing attachment issue.
                    'objFile.FileUpload(File1Int, EnumUploadType.DocAttachmentTemp, "PO", EnumUploadFrom.FrontOff, ViewState("poid"), , , , , , "I")
                    objFile.FileUpload(File1Int, EnumUploadType.DocAttachment, "PO", EnumUploadFrom.FrontOff, ViewState("poid"), , , , , , "I")
                    Select Case ViewState("mode")
                        Case "po"
                        Case "rfq"
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

        Dim chkItem As CheckBox
        Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark, txtGSTAmt, txtGSTAmount As TextBox
        Dim lblItemLine, txtBudget, txtDelivery As Label
        'Dim cboGLCode, cboCust, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList
        Dim cboCust, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList 'Jules 2018.10.24 U00019
        Dim cboGift As DropDownList ', cboFundType, cboPersonCode, cboProjectCode As DropDownList 'Jules 2018.07.14 - PAMB; Jules 2018.10.18
        Dim hidFundType, hidPersonCode, hidProjectCode, hidGLCode As TextBox 'Jules 2018.10.18
        Dim strItem, strItemCust As New ArrayList()
        Dim dgItem As DataGridItem
        Dim dgItem2 As DataGridItem
        Dim k As Integer = 0
        Dim strItemHead As New ArrayList()

        strItemHead.Add(New String() {txtAttention.Text, txtInternal.Text, txtExternal.Text, cboBillCode.SelectedIndex, cboShipmentTerm.SelectedIndex, cboShipmentMode.SelectedIndex, txtShipVia.Text, chkUrgent.Checked, txtShippingHandling.Text, Session("venId"), cboPayTerm.SelectedIndex, cboPayMethod.SelectedIndex, lblCurrencyCode.Text})

        For Each dgItem In dtgShopping.Items
            txtQty = dgItem.FindControl("txtQty")
            'cboGLCode = dgItem.FindControl("cboGLCode") 'Jules 2018.10.24 U00019
            txtPrice = dgItem.FindControl("txtPrice")
            txtBudget = dgItem.FindControl("txtBudget")
            hidBudgetCode = dgItem.FindControl("hidBudgetCode")
            txtDelivery = dgItem.FindControl("txtDelivery")
            hidDelCode = dgItem.FindControl("hidDelCode")
            txtEstDate = dgItem.FindControl("txtEstDate")
            txtWarranty = dgItem.FindControl("txtWarranty")
            txtRemark = dgItem.FindControl("txtRemark")
            cboCategoryCode = dgItem.FindControl("cboCategoryCode")
            cboUOM = dgItem.FindControl("ddl_uom")
            cboAssetGroup = dgItem.FindControl("cboAssetGroup")
            txtGSTAmt = dgItem.FindControl("txtGSTAmt")
            ddlGST = dgItem.FindControl("ddlGST")
            txtGSTAmount = dgItem.FindControl("txtGSTAmount")
            'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
            cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")

            'Jules 2018.07.14
            cboGift = dgItem.FindControl("cboGift")

            'Jules 2018.10.18
            'cboFundType = dgItem.FindControl("cboFundType")
            'cboPersonCode = dgItem.FindControl("cboPersonCode")
            'cboProjectCode = dgItem.FindControl("cboProjectCode")
            hidFundType = dgItem.FindControl("hidFundType")
            hidPersonCode = dgItem.FindControl("hidPersonCode")
            hidProjectCode = dgItem.FindControl("hidProjectCode")
            hidGLCode = dgItem.FindControl("hidGLCode")
            'End modification.

            chkItem = dgItem.FindControl("chkSelection")
            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})

            'Jules 2018.07.14 - PAMB
            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedIndex})
            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
            strItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18

            If (ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr") Or ViewState("modeRFQFromPR_Index") <> "" Then
                Dim objAdmin As New Admin

                If ViewState("modeRFQFromPR_Index") <> "" Then
                    dvwCus = objAdmin.getCustomFieldPR("", "PR", ViewState("modeRFQFromPR_Index"))
                Else
                    dvwCus = objAdmin.getCustomField("", "PO")
                End If

                If Not dvwCus Is Nothing Then
                    For i = 0 To dvwCus.Count - 1
                        Dim cboCustom As DropDownList
                        cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                        If chkItem.Checked Then
                        Else
                            strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                        End If
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
            'Jules 2019.03.21 - To avoid missing attachment issue.
            'dsAttach = objPO.getPOTempAttach(Session.SessionID)
            dsAttach = objPO.getPOAttachment(Session.SessionID)
            'End modification.

            Select Case ViewState("mode")
                Case "po"
                Case "rfq"
            End Select
        ElseIf ViewState("type") = "mod" Then
            'Jules 2019.03.21 - To avoid missing attachment issue.
            'dsAttach = objPO.getPOTempAttach(ViewState("poid"))
            dsAttach = objPO.getPOAttachment(ViewState("poid"))
            'End modification.

            Select Case ViewState("mode")
                Case "po"
                Case "rfq"
            End Select
        End If

        If Not Session("NewPoInfo") Is Nothing Then
            'Jules 2019.03.21 - To avoid missing attachment issue.
            'dsAttach = objPO.getPOTempAttach(Session.SessionID, "E")
            dsAttach = objPO.getPOAttachment(Session.SessionID, "E")
            'End modification.
        End If
        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                '**********************meilai 25/02/2005****************** 
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
                Else
                    If drvAttach(i)("CDA_STATUS") = "N" Then
                        pnlAttach.Controls.Add(lnk)
                    End If
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

    Private Sub displayAttachFileInt()
        Dim objPO As New PurchaseOrder_Buyer
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        If ViewState("type") = "new" Then
            'Jules 2019.03.21 - To avoid missing attachment issue.
            'dsAttach = objPO.getPOTempAttach(Session.SessionID, "I")
            dsAttach = objPO.getPOAttachment(Session.SessionID, "I")
            'End modification.

            Select Case ViewState("mode")
                Case "po"
                Case "rfq"
            End Select
        ElseIf ViewState("type") = "mod" Then
            'Jules 2019.03.21 - To avoid missing attachment issue.
            'dsAttach = objPO.getPOTempAttach(ViewState("poid"), "I")
            dsAttach = objPO.getPOAttachment(ViewState("poid"), "I")
            'End modification.

            Select Case ViewState("mode")
                Case "po"
                Case "rfq"
            End Select
        End If

        'Zulham Oct 3,2013
        If Not Session("NewPoInfo") Is Nothing Then
            'Jules 2019.03.21 - To avoid missing attachment issue.
            'dsAttach = objPO.getPOTempAttach(Session.SessionID, "I")
            dsAttach = objPO.getPOAttachment(Session.SessionID, "I")
            'End modification.
        End If
        'End

        pnlAttachInt.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                '**********************meilai 25/02/2005****************** 
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
                AddHandler lnk.Click, AddressOf deleteAttachInt

                pnlAttachInt.Controls.Add(lblFile)
                If ViewState("modePR") <> "pr" Then
                    pnlAttachInt.Controls.Add(lnk)
                Else
                    If drvAttach(i)("CDA_STATUS") = "N" Then
                        pnlAttachInt.Controls.Add(lnk)
                    End If
                End If
                pnlAttachInt.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttachInt.Controls.Add(lblFile)
        End If
        objPO = Nothing
    End Sub

    Private Sub deleteAttach(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objPO As New PurchaseOrder_Buyer
        If ViewState("type") = "new" Then
            'Jules 2019.03.26
            'objPO.delete_Attachment_Temp(CType(sender, ImageButton).ID)
            objPO.delete_Attachment_byIndex(CType(sender, ImageButton).ID)
            'End modification.

            Select Case ViewState("mode")
                Case "po"
                Case "rfq"
            End Select
        ElseIf ViewState("type") = "mod" Then
            'Jules 2019.03.26
            'objPO.delete_Attachment_Temp(CType(sender, ImageButton).ID)
            objPO.delete_Attachment_byIndex(CType(sender, ImageButton).ID)
            'End modification.
            Select Case ViewState("mode")
                Case "po"
                Case "rfq"
            End Select
        End If

        displayAttachFile()
        objPO = Nothing
    End Sub

    Private Sub deleteAttachInt(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objPO As New PurchaseOrder_Buyer
        If ViewState("type") = "new" Then
            'Jules 2019.03.26 
            'objPO.delete_Attachment_Temp(CType(sender, ImageButton).ID)
            objPO.delete_Attachment_byIndex(CType(sender, ImageButton).ID)
            'End modification.
            Select Case ViewState("mode")
                Case "po"
                Case "rfq"
            End Select
        ElseIf ViewState("type") = "mod" Then
            'Jules 2019.03.26 
            'objPO.delete_Attachment_Temp(CType(sender, ImageButton).ID)
            objPO.delete_Attachment_byIndex(CType(sender, ImageButton).ID)
            'End modification.
            Select Case ViewState("mode")
                Case "po"
                Case "rfq"
            End Select
        End If

        displayAttachFileInt()
        objPO = Nothing
    End Sub

    Private Function validateDatagrid(ByRef strMsg As String) As Boolean
        validateDatagrid = True
        strMsg = "<ul type='disc'>"

        If dtgShopping.Items.Count = 0 Then
            strMsg &= "<li>Must have at least one item.<ul type='disc'></ul></li>"
            validateDatagrid = False
        End If

        If Not Common.checkMaxLength(txtInternal.Text, 1000) Then
            strMsg &= "<li>For Internal Use is over limit.<ul type='disc'></ul></li>"
            validateDatagrid = False
        End If

        If Not Common.checkMaxLength(txtExternal.Text, 1000) Then
            strMsg &= "<li>Remarks is over limit.<ul type='disc'></ul></li>"
            validateDatagrid = False
        End If

        If txtVendor.Text.Length = 0 Then
            strMsg &= "<li>Vendor is required.</li>"
            validateDatagrid = False
        End If

        If txtVendor.Text.Length <> 0 Then
            Dim strRec_No As String = objDB.GetVal("SELECT CV_B_COY_ID FROM COMPANY_VENDOR WHERE CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CV_S_COY_ID = '" & Hidden6.Value & "'")
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
        Dim revPrice As RegularExpressionValidator
        Dim txtPriceCost As TextBox
        Dim ddlGST, cboGSTTaxCode As DropDownList
        Dim objGST As New gst
        For Each dgItem In dtgShopping.Items

            'Check the selected GST
            If strIsGst = "Yes" Then
                ddlGST = dgItem.FindControl("ddlGST")
                cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")
                If ddlGST.SelectedItem.Text.ToUpper.Contains("SELECT") Then
                    strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Please select the SST percentage.</li>"
                    validateDatagrid = False
                End If

                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                If ddlGST.SelectedItem.Value <> "" And ddlGST.SelectedItem.Value <> "N/A" Then
                    If objGST.chkValidTaxCode(ddlGST.SelectedItem.Value, cboGSTTaxCode.SelectedValue, "P") = False Then
                        strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Invalid Purchase Tax Code.<ul type='disc'></ul></li>"
                        validateDatagrid = False
                    End If
                End If
            End If
            'End

            txtPriceCost = dgItem.FindControl("txtPrice")
            revPriceRange = dgItem.FindControl("revPriceRange")
            revPrice = dgItem.FindControl("revPrice")

            'Jules 2018.08.01 - Check Item Name for special characters
            Dim lblProductDesc As TextBox
            lblProductDesc = dgItem.FindControl("lblProductDesc")
            If lblProductDesc.Text.Contains("<") OrElse lblProductDesc.Text.Contains(">") Then
                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Item Name cannot contain '<' or '>'.</li>"
                validateDatagrid = False
            End If
            'End modification.

            'Jules 2018.10.24 U00019
            'Jules 2018.08.06
            'Dim ddlGLCode, cboCategoryCode As DropDownList
            'ddlGLCode = dgItem.FindControl("cboGLCode")
            'If ddlGLCode.SelectedItem.Text = "---Select---" Then
            '    strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". GL Code is required.<ul type='disc'></ul></li>"
            '    validateDatagrid = False
            'End If
            Dim hidGLCode As TextBox
            hidGLCode = dgItem.FindControl("hidGLCode")
            If hidGLCode.Text = "" Then
                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". GL Code is required.<ul type='disc'></ul></li>"
                validateDatagrid = False
            End If

            Dim cboCategoryCode As DropDownList
            'End modification.

            If Left(txtPriceCost.Text, 1) = "," Then
                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Invalid unit price.<ul type='disc'></ul></li>"
                revPriceRange.Text = "?"
                validateDatagrid = False
            Else
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
                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Invalid Est. Date of Delivery (dd/mm/yyyy).</li>"
                validateDatagrid = False
            Else
                Dim txtDate1 As Date = CType(lblDate.Text, Date)
                Dim txtDate2 As Date = CType(txtEstDate.Text, Date)
                If ViewState("type") = "new" Then
                    Select Case ViewState("mode")
                        Case "po", "cc"
                            If txtDate2 <= txtDate1 Then
                                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Invalid Est. Date of Delivery (dd/mm/yyyy).</li>"
                                validateDatagrid = False
                            End If
                        Case "rfq"
                            If txtDate2 < txtDate1 Then
                                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Invalid Est. Date of Delivery (dd/mm/yyyy).</li>"
                                validateDatagrid = False
                            End If
                    End Select
                ElseIf ViewState("type") = "mod" Then
                    getQuo = objPO1.get_PO_Quo(ViewState("poid"))
                    If getQuo.Tables(0).Rows.Count > 0 Then 'getQuo.Tables.Count > 0 Then
                        Quo = Common.parseNull(getQuo.Tables(0).Rows(0)("pom_quotation_no"))
                        If Quo <> "" Then
                            If txtDate2 < txtDate1 Then
                                strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Invalid Est. Date of Delivery (dd/mm/yyyy).</li>"
                                validateDatagrid = False
                            End If
                        ElseIf txtDate2 <= txtDate1 Then
                            strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Invalid Est. Date of Delivery (dd/mm/yyyy).</li>"
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
                        txtprice = dgItem.FindControl("txtprice")
                        revPriceRange = dgItem.FindControl("revPriceRange")
                    Case "rfq"
                        txtprice = dgItem.FindControl("txtprice")
                        revPriceRange = dgItem.FindControl("revPriceRange")
                        getQuoPrice = objPO1.GetQuotationPrice(ViewState("rfqnum"), ViewState("quono"), HttpContext.Current.Session("CompanyId"), dgItem.Cells(EnumShoppingCart.icNo).Text, Request.QueryString("vendor"), dgItem.Cells(EnumShoppingCart.icVendorItemCode).Text, CType(dgItem.FindControl("lblProductDesc"), TextBox).Text)
                        If getQuoPrice.Tables(0).Rows.Count > 0 Then
                            QuoPrice = Common.parseNull(getQuoPrice.Tables(0).Rows(0)("rrd_unit_price"))
                            If (CDec(txtprice.Text) < 0 Or CDec(txtprice.Text) > QuoPrice) Then
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
                        getQuoPrice = objPO1.GetQuotationPrice(ViewState("rfqnum"), Quo, HttpContext.Current.Session("CompanyId"), dgItem.Cells(EnumShoppingCart.icNo).Text, s_Coy, dgItem.Cells(EnumShoppingCart.icVendorItemCode).Text, CType(dgItem.FindControl("lblProductDesc"), TextBox).Text)
                        If getQuoPrice.Tables(0).Rows.Count > 0 Then
                            QuoPrice = Common.parseNull(getQuoPrice.Tables(0).Rows(0)("rrd_unit_price"))
                            If (CDec(txtprice.Text) < 0 Or CDec(txtprice.Text) > QuoPrice) Then

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
                        Dim txtprice1 As TextBox
                        txtprice1 = dgItem.FindControl("txtprice")
                        Dim _0 = CDec(txtprice1.Text)
                        If _0 <= 0 Then
                        End If
                    Case "rfq"
                End Select
            End If
        Next

        Dim txtRemark As TextBox
        Dim txtQ As TextBox
        Dim cboCategory, cboDelivery As DropDownList
        Dim strItemType, strDelivery, strBudgetIndex
        Dim hidBudgetCode As TextBox

        strItemType = ""

        Dim GridItem As DataGridItem
        Dim PrefDS As DataSet
        Dim ProdCode, ProdGrp As String
        Dim strPPrefer As String = Session("venId")
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
            'mimi 2018-04-24 : remove hardcode HLB to PAMB
            'If UCase(HttpContext.Current.Session("CompanyId")) = "HLB" Then

            'Jules 2018.07.31 - Category Code always set to N/A
            'If UCase(HttpContext.Current.Session("CompanyId")) = "PAMB" Then
            '    cboCategory = dgItem.FindControl("cboCategoryCode")
            '    If strItemType = "" Then
            '        strItemType = UCase(Mid(cboCategory.SelectedValue, 1, 1))
            '    Else
            '        If strItemType <> UCase(Mid(cboCategory.SelectedValue, 1, 1)) Then
            '            strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". PO has item(s) which have a mixture of OPEX and CAPEX category. Please make sure the PO has either CAPEX or OPEX item only and not both.<ul type='disc'></ul></li>"
            '            validateDatagrid = False
            '        End If
            '    End If
            'End If
            'End modification.

            If ViewState("BCM") > 0 Then
                hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                If hidBudgetCode.Text = "" Then
                    strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Budget Account not selected.<ul type='disc'></ul></li>"
                    validateDatagrid = False
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

            'Jules 2018.10.24 U00019
            'Dim cboGLCode, cboCategoryCode As DropDownList
            'cboGLCode = dgItem.FindControl("cboGLCode")
            Dim cboCategoryCode As DropDownList
            cboCategoryCode = dgItem.FindControl("cboCategoryCode")
            'End modification.

            'Jules 2018.07.31 - Category Code always set to N/A.
            'Dim strCat As String = objDB.GetVal("SELECT IFNULL(GROUP_CONCAT(CBGC_B_CATEGORY_CODE), '') AS CBGC_B_CATEGORY_CODE FROM COMPANY_B_GL_CODE_CATEGORY_CODE WHERE CBGC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CBGC_B_GL_CODE = '" & cboGLCode.SelectedValue.ToString & "' AND (SELECT COUNT(CBGC_B_GL_CODE) FROM COMPANY_B_GL_CODE_CATEGORY_CODE WHERE CBGC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND CBGC_B_GL_CODE = '" & cboGLCode.SelectedValue.ToString & "') > 0")
            'If strCat <> "" Then
            '    If cboGLCode.SelectedItem.ToString <> "Not Applicable" And Not strCat.Contains(cboCategoryCode.SelectedItem.ToString) Then
            '        strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". " & objGlobal.GetErrorMessage("00370") & "<ul type='disc'></ul></li>"
            '        validateDatagrid = False
            '    End If
            'End If            
            'End modification.

            'Jules 2018.10.24 U00019
            'Jules 2018.05.03 - PAMB Scrum 2
            'If cboGLCode.SelectedValue.ToString <> "" AndAlso cboGLCode.SelectedItem.Text <> "---Select---" AndAlso validateDatagrid = True Then
            Dim hidGLCode As TextBox
            hidGLCode = dgItem.FindControl("hidGLCode")

            If hidGLCode.Text <> "" AndAlso validateDatagrid = True Then 'End modification.
                'Jules 2018.10.18 - Changed from dropdownlist to textbox
                'Dim cboFundType, cboPersonCode, cboProjectCode As DropDownList
                'cboFundType = dgItem.FindControl("cboFundType")
                'cboPersonCode = dgItem.FindControl("cboPersonCode")
                'cboProjectCode = dgItem.FindControl("cboProjectCode")
                Dim hidFundType, hidPersonCode, hidProjectCode As TextBox
                hidFundType = dgItem.FindControl("hidFundType")
                hidPersonCode = dgItem.FindControl("hidPersonCode")
                hidProjectCode = dgItem.FindControl("hidProjectCode")

                'Dim strSqlAC As String = "SELECT IFNULL(CBGCAC_ANALYSIS_CODE1, '') AS CBGCAC_ANALYSIS_CODE1, IFNULL(CBGCAC_ANALYSIS_CODE8, '') AS CBGCAC_ANALYSIS_CODE8, IFNULL(CBGCAC_ANALYSIS_CODE9, '') AS CBGCAC_ANALYSIS_CODE9 FROM company_b_gl_code_analysis_code WHERE CBGCAC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND CBGCAC_B_GL_CODE = '" & cboGLCode.SelectedValue.ToString & "'"
                Dim strSqlAC As String = "SELECT IFNULL(CBGCAC_ANALYSIS_CODE1, '') AS CBGCAC_ANALYSIS_CODE1, IFNULL(CBGCAC_ANALYSIS_CODE8, '') AS CBGCAC_ANALYSIS_CODE8, IFNULL(CBGCAC_ANALYSIS_CODE9, '') AS CBGCAC_ANALYSIS_CODE9 FROM company_b_gl_code_analysis_code WHERE CBGCAC_B_COY_ID ='" & HttpContext.Current.Session("CompanyId") & "' AND CBGCAC_B_GL_CODE = '" & hidGLCode.Text & "'" 'Jules 2018.10.24 U00019
                Dim dsAnalysisCodes As DataSet = objDB.FillDs(strSqlAC)
                If dsAnalysisCodes.Tables(0).Rows.Count > 0 Then
                    'If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE1").ToString = "M" And cboFundType.SelectedIndex = 0 Then
                    If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE1").ToString = "M" And hidFundType.Text = "" Then
                        strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". " & objGlobal.GetErrorMessage("00374") & "<ul type='disc'></ul></li>"
                        validateDatagrid = False
                    End If
                    'If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE9").ToString = "M" And cboPersonCode.SelectedIndex = 0 Then
                    If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE9").ToString = "M" And hidPersonCode.Text = "" Then
                        strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". " & objGlobal.GetErrorMessage("00375") & "<ul type='disc'></ul></li>"
                        validateDatagrid = False
                    End If
                    'If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE8").ToString = "M" And cboProjectCode.SelectedIndex = 0 Then
                    If dsAnalysisCodes.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE8").ToString = "M" And hidProjectCode.Text = "" Then
                        strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". " & objGlobal.GetErrorMessage("00376") & "<ul type='disc'></ul></li>"
                        validateDatagrid = False
                    End If
                End If
                'End modification.
            End If
            'End modification.

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
                If checkempty = "" Or checkempty = "&nbsp;" Then
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

    Private Sub GenerateTab()
        Dim _role As New UserRoles
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Dim showFFPO = objDB.GetVal("SELECT CM_FFPO_CONTROL FROM company_mstr WHERE cm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'")
        Dim _compType = objDB.GetVal("SELECT CM_COY_TYPE FROM company_mstr WHERE cm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'")

        Dim _userRole = _role.get_UserFixedRole()
        If showFFPO.ToString.ToUpper = "Y" And (_userRole.ToString.ToUpper.Contains("PURCHASING MANAGER") Or _userRole.ToString.ToUpper.Contains("PURCHASING OFFICER")) And _compType.ToString.ToUpper = "BUYER" Then
            Session("w_AddPO_tabs") = "<div class=""t_entity""><ul>" &
            "<li><div class=""space""></div></li>" &
                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" &
                 "<li><div class=""space""></div></li>" &
                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB2Cancel.aspx", "type=Listing&pageid=" & strPageId) & """><span>Purchase Order Cancellation</span></a></li>" &
                 "<li><div class=""space""></div></li>" &
                 "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "RAISEFFPO.aspx", "type=Listing&pageid=" & strPageId) & """><span>Free Form Purchase Order</span></a></li>" &
                 "<li><div class=""space""></div></li>" &
                 "</ul><div></div></div>"
        Else
            Session("w_AddPO_tabs") = "<div class=""t_entity""><ul>" &
                       "<li><div class=""space""></div></li>" &
                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" &
                            "<li><div class=""space""></div></li>" &
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB2Cancel.aspx", "type=Listing&pageid=" & strPageId) & """><span>Purchase Order Cancellation</span></a></li>" &
                            "<li><div class=""space""></div></li>" &
                            "</ul><div></div></div>"
        End If
    End Sub

    Protected Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click

        Dim dsPO As New DataSet
        Dim objPO As New PurchaseOrder
        Dim objPO1 As New PurchaseOrder_Buyer
        Dim strMsg As String = ""
        Dim strRFQ_No, strPR_Index, strGrp_Index
        Dim strDept_Code As String = ""

        Dim dt As New DataTable

        blnValid = True
        blnItem = False
        If validateDatagrid(strMsg) AndAlso validateSSTRates(strMsg) Then 'Jules 2018.09.19 - SST
            AddAssetGroup() 'Zulham Nov 21, 2013
            dsPO = bindPO()
            Dim dsapplist As New DataSet
            Dim objPR As New PR
            dsapplist = objPR.getAppovalList("A", CDbl(ViewState("POCost")), "PO", True)
            If dsapplist.Tables(0).Rows.Count = 0 Then
                strMsg &= "<li>There is no approval list available for this PO because the sequence of approving officers do not have the approval limit to approve it.<ul type='disc'></ul></li>"
                lblMsg.Text = strMsg
                ViewState("POCost") = Nothing
                Exit Sub
            End If
            'Michelle (19/2/2014) - Issue 2694
            Dim strInvAppr As String = ""
            strInvAppr = objDB.GetVal("SELECT CM_INV_APPR FROM COMPANY_MSTR WHERE CM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
            If strInvAppr = "Y" Then 'Check whether the department is tied with approval flow
                If objDB.Exist("SELECT '*' FROM USER_MSTR " &
                            "INNER JOIN COMPANY_DEPT_MSTR ON CDM_DEPT_CODE = UM_DEPT_ID AND CDM_COY_ID = UM_COY_ID " &
                            "INNER JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = CDM_APPROVAL_GRP_INDEX WHERE UM_USER_ID ='" & HttpContext.Current.Session("UserID") & "' " &
                            "AND UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'") = 0 Then
                    strMsg &= "<li>There is no Invoice Approval List for your department. Please contact the administrator.<ul type='disc'></ul></li>"
                    lblMsg.Text = strMsg
                    ViewState("POCost") = Nothing
                    Exit Sub
                End If
            End If

            If ViewState("type") = "new" Then
                Dim strNewPO As String
                Dim intMsg As Integer

                If blnValid = True Then
                    If ViewState("blnBill") <> 0 And cboBillCode.SelectedIndex = 0 And ViewState("modePR") <> "pr" Then
                        lblMsg.Text = "<ul type='disc'><li>Bill To is required.<ul type='disc'></ul></li></ul>"
                    Else
                        lblMsg.Text = ""

                        If ViewState("modePR") = "pr" And (ViewState("modeRFQFromPR_Index") <> "" Or ViewState("modeRFQFromPR_Index_draft") <> "") Then
                            strPR_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRM_PR_INDEX),""'"")) AS CHAR(2000)) AS PRM_PR_INDEX FROM PR_DETAILS, PR_MSTR WHERE PRM_COY_ID = PRD_COY_ID AND PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND (PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "')")
                            strGrp_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRA_APPROVAL_GRP_INDEX),""'"")) AS CHAR(2000)) AS PRA_APPROVAL_GRP_INDEX FROM PR_APPROVAL WHERE PRA_FOR = 'PR' AND PRA_PR_INDEX IN (" & strPR_Index & ")")
                            strDept_Code = objDB.GetVal("SELECT IFNULL(CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",AGM_DEPT_CODE),""'"")) AS CHAR(2000)),'') AS AGM_DEPT_CODE FROM APPROVAL_GRP_MSTR WHERE AGM_DEPT_CODE IS NOT NULL AND AGM_DEPT_CODE <> '' AND AGM_GRP_INDEX IN (" & strGrp_Index & ")")
                        ElseIf ViewState("modePR") = "pr" And (ViewState("modeRFQFromPR_Index") = "" Or ViewState("modeRFQFromPR_Index_draft") = "") Then
                            Dim strPO_No As String = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRD_CONVERT_TO_DOC),""'"")) AS CHAR(2000)) AS PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("poid") & "'")
                            strPR_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRM_PR_INDEX),""'"")) AS CHAR(2000)) AS PRM_PR_INDEX FROM PR_DETAILS, PR_MSTR WHERE PRM_COY_ID = PRD_COY_ID AND PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND (PRD_CONVERT_TO_DOC IN (" & strPO_No & "))")
                            strGrp_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRA_APPROVAL_GRP_INDEX),""'"")) AS CHAR(2000)) AS PRA_APPROVAL_GRP_INDEX FROM PR_APPROVAL WHERE PRA_FOR = 'PR' AND PRA_PR_INDEX IN (" & strPR_Index & ")")
                            strDept_Code = objDB.GetVal("SELECT IFNULL(CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",AGM_DEPT_CODE),""'"")) AS CHAR(2000)),'') AS AGM_DEPT_CODE FROM APPROVAL_GRP_MSTR WHERE AGM_DEPT_CODE IS NOT NULL AND AGM_DEPT_CODE <> '' AND AGM_GRP_INDEX IN (" & strGrp_Index & ")")
                        End If

                        dt = objPO1.getPOApprFlow(True, strDept_Code)
                        Dim CheckApprB4 As Boolean = True
                        If dt.Rows.Count = 0 Then
                            CheckApprB4 = False
                        End If

                        If lblPONo.Text = "To Be Allocated By System" And CheckApprB4 = True Then
                            If ViewState("modePR") = "pr" Then
                                If ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr" Then
                                    intMsg = objPO1.insertPO(dsPO, strNewPO, True)
                                Else
                                    If ViewState("modeRFQFromPR_Index") = "" Then
                                        intMsg = objPO1.insertPO(dsPO, strNewPO, False)
                                    Else
                                        intMsg = objPO1.insertPO(dsPO, strNewPO, True)
                                    End If
                                End If
                            Else
                                If ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr" Then
                                    intMsg = objPO1.insertPO(dsPO, strNewPO, True, True, True)
                                Else
                                    intMsg = objPO1.insertPO(dsPO, strNewPO, False)
                                End If
                            End If
                        Else
                            If ViewState("modePR") = "pr" And (ViewState("modeRFQFromPR_Index") <> "" Or ViewState("modeRFQFromPR_Index_draft") <> "") Then
                                strPR_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRM_PR_INDEX),""'"")) AS CHAR(2000)) AS PRM_PR_INDEX FROM PR_DETAILS, PR_MSTR WHERE PRM_COY_ID = PRD_COY_ID AND PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND (PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "')")
                                strGrp_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRA_APPROVAL_GRP_INDEX),""'"")) AS CHAR(2000)) AS PRA_APPROVAL_GRP_INDEX FROM PR_APPROVAL WHERE PRA_FOR = 'PR' AND PRA_PR_INDEX IN (" & strPR_Index & ")")
                                strDept_Code = objDB.GetVal("SELECT IFNULL(CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",AGM_DEPT_CODE),""'"")) AS CHAR(2000)),'') AS AGM_DEPT_CODE FROM APPROVAL_GRP_MSTR WHERE AGM_DEPT_CODE IS NOT NULL AND AGM_DEPT_CODE <> '' AND AGM_GRP_INDEX IN (" & strGrp_Index & ")")
                            ElseIf ViewState("modePR") = "pr" And (ViewState("modeRFQFromPR_Index") = "" Or ViewState("modeRFQFromPR_Index_draft") = "") Then
                                Dim strPO_No As String = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRD_CONVERT_TO_DOC),""'"")) AS CHAR(2000)) AS PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("poid") & "'")
                                strPR_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRM_PR_INDEX),""'"")) AS CHAR(2000)) AS PRM_PR_INDEX FROM PR_DETAILS, PR_MSTR WHERE PRM_COY_ID = PRD_COY_ID AND PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND (PRD_CONVERT_TO_DOC IN (" & strPO_No & "))")
                                strGrp_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRA_APPROVAL_GRP_INDEX),""'"")) AS CHAR(2000)) AS PRA_APPROVAL_GRP_INDEX FROM PR_APPROVAL WHERE PRA_FOR = 'PR' AND PRA_PR_INDEX IN (" & strPR_Index & ")")
                                strDept_Code = objDB.GetVal("SELECT IFNULL(CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",AGM_DEPT_CODE),""'"")) AS CHAR(2000)),'') AS AGM_DEPT_CODE FROM APPROVAL_GRP_MSTR WHERE AGM_DEPT_CODE IS NOT NULL AND AGM_DEPT_CODE <> '' AND AGM_GRP_INDEX IN (" & strGrp_Index & ")")
                            End If

                            dt = objPO1.getPOApprFlow(True, strDept_Code)

                            If dt.Rows.Count = 0 Then
                                Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                Exit Sub
                            ElseIf dt.Rows.Count > 1 Then
                                Response.Redirect(dDispatcher.direct("PO", "POApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&type=" & ViewState("type") & "&poid=" & lblPONo.Text & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.00") & "&Frm=" & Me.Request.QueryString("Frm") & "&dpage=" & Request.QueryString("dpage") & "&currency=" & ViewState("Currency") & "&dept=" & strDept_Code & "&prindex=" & strPR_Index))
                                Exit Sub
                            Else
                                If ViewState("modePR") = "pr" Then
                                    If ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr" Then
                                        objPO1.updatePO(dsPO, True)
                                    Else
                                        objPO1.updatePO(dsPO, False)
                                    End If
                                Else
                                    If ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr" Then
                                        objPO1.updatePO(dsPO, True)
                                    Else
                                        objPO1.updatePO(dsPO, False)
                                    End If
                                End If
                                intMsg = WheelMsgNum.Save
                                strNewPO = lblPONo.Text
                            End If
                        End If
                        objPO = Nothing
                        'redirect to ExceedBCM before approval page
                        Select Case intMsg
                            Case WheelMsgNum.Save
                                If blnItem Then ' item exists
                                    If ViewState("BCM") > 0 Then
                                        If checkMandatory(strMsg) Then
                                            If Not Request.QueryString("Frm") Is Nothing Then
                                                Response.Redirect(dDispatcher.direct("PO", "ExceedBCMPO.aspx", "pageid=" & strPageId & "&poid=" & strNewPO & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&modePR=" & ViewState("modePR") & "&moderfqfromprindex=" & ViewState("modeRFQFromPR_Index") & "&rfqnum=" & ViewState("rfqnum") & "&moderfqfromprindexdraft=" & ViewState("modeRFQFromPR_Index_draft") & "&Frm=" & Request.QueryString("Frm")))
                                            Else
                                                Response.Redirect(dDispatcher.direct("PO", "ExceedBCMPO.aspx", "pageid=" & strPageId & "&poid=" & strNewPO & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&modePR=" & ViewState("modePR") & "&moderfqfromprindex=" & ViewState("modeRFQFromPR_Index") & "&rfqnum=" & ViewState("rfqnum") & "&moderfqfromprindexdraft=" & ViewState("modeRFQFromPR_Index_draft") & "&Frm=RaiseFFPO"))
                                            End If
                                        Else
                                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                                        End If
                                    Else
                                        lblPONo.Text = strNewPO
                                        SubmitPO()
                                    End If
                                Else
                                    Common.NetMsgbox(Me, "There are no items in this PO.", MsgBoxStyle.Information)
                                End If

                            Case WheelMsgNum.NotSave
                                Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                            Case WheelMsgNum.Duplicate
                                Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)
                            Case -1
                                Common.NetMsgbox(Me, "Company is currently inactive.", MsgBoxStyle.Information)
                            Case -2
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
                        If blnItem Then ' item exists

                            If ViewState("BCM") > 0 Then
                                If checkMandatory(strMsg) Then

                                    If ViewState("modePR") <> "pr" Then
                                        If ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr" Then
                                            objPO1.updatePO(dsPO, True)
                                        Else
                                            objPO1.updatePO(dsPO, False)
                                        End If
                                    Else
                                        objPO1.updatePO(dsPO, False)
                                    End If

                                    If ViewState("modePR") = "pr" And (ViewState("modeRFQFromPR_Index") <> "" Or ViewState("modeRFQFromPR_Index_draft") <> "") Then
                                        strPR_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRM_PR_INDEX),""'"")) AS CHAR(2000)) AS PRM_PR_INDEX FROM PR_DETAILS, PR_MSTR WHERE PRM_COY_ID = PRD_COY_ID AND PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND (PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "')")
                                        strGrp_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRA_APPROVAL_GRP_INDEX),""'"")) AS CHAR(2000)) AS PRA_APPROVAL_GRP_INDEX FROM PR_APPROVAL WHERE PRA_FOR = 'PR' AND PRA_PR_INDEX IN (" & strPR_Index & ")")
                                        strDept_Code = objDB.GetVal("SELECT IFNULL(CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",AGM_DEPT_CODE),""'"")) AS CHAR(2000)),'') AS AGM_DEPT_CODE FROM APPROVAL_GRP_MSTR WHERE AGM_DEPT_CODE IS NOT NULL AND AGM_DEPT_CODE <> '' AND AGM_GRP_INDEX IN (" & strGrp_Index & ")")
                                    ElseIf ViewState("modePR") = "pr" And (ViewState("modeRFQFromPR_Index") = "" Or ViewState("modeRFQFromPR_Index_draft") = "") Then
                                        Dim strPO_No As String = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRD_CONVERT_TO_DOC),""'"")) AS CHAR(2000)) AS PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("poid") & "'")
                                        strPR_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRM_PR_INDEX),""'"")) AS CHAR(2000)) AS PRM_PR_INDEX FROM PR_DETAILS, PR_MSTR WHERE PRM_COY_ID = PRD_COY_ID AND PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND (PRD_CONVERT_TO_DOC IN (" & strPO_No & "))")
                                        strGrp_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRA_APPROVAL_GRP_INDEX),""'"")) AS CHAR(2000)) AS PRA_APPROVAL_GRP_INDEX FROM PR_APPROVAL WHERE PRA_FOR = 'PR' AND PRA_PR_INDEX IN (" & strPR_Index & ")")
                                        strDept_Code = objDB.GetVal("SELECT IFNULL(CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",AGM_DEPT_CODE),""'"")) AS CHAR(2000)),'') AS AGM_DEPT_CODE FROM APPROVAL_GRP_MSTR WHERE AGM_DEPT_CODE IS NOT NULL AND AGM_DEPT_CODE <> '' AND AGM_GRP_INDEX IN (" & strGrp_Index & ")")
                                    End If

                                    dt = objPO1.getPOApprFlow(True, strDept_Code)

                                    If dt.Rows.Count = 0 Then
                                        Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                                        Exit Sub
                                    End If
                                    Session("strItem") = Nothing
                                    Session("strItemCust") = Nothing
                                    Session("strItemHead") = Nothing
                                    Response.Redirect(dDispatcher.direct("PO", "ExceedBCMPO.aspx", "poid=" & hidNewPO.Value & "&pocost=" & Format(CDbl(ViewState("POCost")), "#0.0000") & "&currency=" & ViewState("Currency") & "&pageid=" & strPageId & "&Frm=" & Request.QueryString("Frm") & "&dpage=AllDashBoard"))
                                Else
                                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                                End If
                            Else
                                If ViewState("modePR") <> "pr" Then
                                    If ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr" Then
                                        objPO1.updatePO(dsPO, True)
                                    Else
                                        objPO1.updatePO(dsPO, False)
                                    End If
                                Else
                                    objPO1.updatePO(dsPO, False)
                                End If
                                SubmitPO()
                            End If

                        Else
                            Common.NetMsgbox(Me, "There are no items in this PR.", MsgBoxStyle.Information)
                        End If
                    End If
                End If
            End If

            Session("strItem") = Nothing
            Session("strItemCust") = Nothing
            Session("strItemHead") = Nothing
        Else
            Dim chkItem As CheckBox
            Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark, txtGSTAmt, txtGSTAmount As TextBox
            Dim lblItemLine, txtBudget, txtDelivery As Label
            'Dim cboGLCode, cboCust, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList
            Dim cboCust, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList 'Jules 2018.10.24 U00019
            Dim cboGift As DropDownList ', cboFundType, cboPersonCode, cboProjectCode As DropDownList 'Jules 2018.07.14 - PAMB; Jules 2018.10.18
            Dim hidFundType, hidPersonCode, hidProjectCode, hidGLCode As TextBox 'Jules 2018.10.18
            Dim strItem, strItemCust As New ArrayList()
            Dim dgItem As DataGridItem
            Dim dgItem2 As DataGridItem
            Dim k As Integer = 0
            Dim strItemHead As New ArrayList()

            strItemHead.Add(New String() {txtAttention.Text, txtInternal.Text, txtExternal.Text, cboBillCode.SelectedIndex, cboShipmentTerm.SelectedIndex, cboShipmentMode.SelectedIndex, txtShipVia.Text, chkUrgent.Checked, txtShippingHandling.Text, Session("venId"), cboPayTerm.SelectedIndex, cboPayMethod.SelectedIndex, lblCurrencyCode.Text})

            For Each dgItem In dtgShopping.Items
                txtQty = dgItem.FindControl("txtQty")
                'cboGLCode = dgItem.FindControl("cboGLCode") 'Jules 2018.10.24 U00019
                txtPrice = dgItem.FindControl("txtPrice")
                txtBudget = dgItem.FindControl("txtBudget")
                hidBudgetCode = dgItem.FindControl("hidBudgetCode")
                txtDelivery = dgItem.FindControl("txtDelivery")
                hidDelCode = dgItem.FindControl("hidDelCode")
                txtEstDate = dgItem.FindControl("txtEstDate")
                txtWarranty = dgItem.FindControl("txtWarranty")
                txtRemark = dgItem.FindControl("txtRemark")
                cboCategoryCode = dgItem.FindControl("cboCategoryCode")
                cboUOM = dgItem.FindControl("ddl_uom")
                cboAssetGroup = dgItem.FindControl("cboAssetGroup")
                txtGSTAmt = dgItem.FindControl("txtGSTAmt")
                ddlGST = dgItem.FindControl("ddlGST")
                txtGSTAmount = dgItem.FindControl("txtGSTAmount")
                'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")

                'Jules 2018.07.14 - PAMB
                cboGift = dgItem.FindControl("cboGift")

                'Jules 2018.10.18
                'cboFundType = dgItem.FindControl("cboFundType")
                'cboPersonCode = dgItem.FindControl("cboPersonCode")
                'cboProjectCode = dgItem.FindControl("cboProjectCode")
                hidFundType = dgItem.FindControl("hidFundType")
                hidPersonCode = dgItem.FindControl("hidPersonCode")
                hidProjectCode = dgItem.FindControl("hidProjectCode")
                hidGLCode = dgItem.FindControl("hidGLCode")
                'End modification.

                chkItem = dgItem.FindControl("chkSelection")
                'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text})

                'Jules 2018.07.14 - PAMB
                'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedIndex})
                'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
                strItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18

                If (ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr") Or ViewState("modeRFQFromPR_Index") <> "" Then
                    Dim objAdmin As New Admin

                    If ViewState("modeRFQFromPR_Index") <> "" Then
                        dvwCus = objAdmin.getCustomFieldPR("", "PR", ViewState("modeRFQFromPR_Index"))
                    Else
                        dvwCus = objAdmin.getCustomField("", "PO")
                    End If

                    If Not dvwCus Is Nothing Then
                        For i = 0 To dvwCus.Count - 1
                            Dim cboCustom As DropDownList
                            cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                            If chkItem.Checked Then
                            Else
                                strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                            End If
                        Next
                    End If
                End If

                k += 1
            Next
            Session("strItem") = strItem
            Session("strItemCust") = strItemCust
            Session("strItemHead") = strItemHead

            Bindgrid()

            If strMsg <> "" Then
                lblMsg.Text = strMsg
            Else
                lblMsg.Text = ""
            End If
        End If
        Session("ProdList") = Nothing
        Session("aryProdCodeNew_All") = Nothing
        Session("keepAr") = Nothing
        'Session("strItem") = Nothing
        'Session("strItemCust") = Nothing
        'Session("strItemHead") = Nothing
    End Sub

    Public Sub btnHidden1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden1.Click
        If Session("CurrentScreen") = "POBatchUpload" Then
            Bindgrid()
        End If
    End Sub

    'Jules 2018.08.03
    Public Sub btnHidden2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden2.Click
        GetVendorDetail()
        Dim chkItem As CheckBox
        Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark, txtGSTAmt, txtGSTAmount As TextBox
        Dim txtBudget, txtDelivery As Label
        'Dim cboGLCode, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList
        Dim cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList 'Jules 2018.10.24 U00019
        Dim cboGift As DropDownList ', cboFundType, cboPersonCode, cboProjectCode As DropDownList 'Jules 2018.10.18
        Dim hidFundType, hidPersonCode, hidProjectCode, hidGLCode As TextBox 'Jules 2018.10.18
        Dim strItem, strItemCust As New ArrayList()
        Dim dgItem As DataGridItem
        Dim k As Integer = 0
        Dim strItemHead As New ArrayList()

        strItemHead.Add(New String() {txtAttention.Text, txtInternal.Text, txtExternal.Text, cboBillCode.SelectedIndex, cboShipmentTerm.SelectedIndex, cboShipmentMode.SelectedIndex, txtShipVia.Text, chkUrgent.Checked, txtShippingHandling.Text, Session("venId"), cboPayTerm.SelectedIndex, cboPayMethod.SelectedIndex, lblCurrencyCode.Text})

        For Each dgItem In dtgShopping.Items
            txtQty = dgItem.FindControl("txtQty")
            'cboGLCode = dgItem.FindControl("cboGLCode") 'Jules 2018.10.24 U00019
            txtPrice = dgItem.FindControl("txtPrice")
            txtBudget = dgItem.FindControl("txtBudget")
            hidBudgetCode = dgItem.FindControl("hidBudgetCode")
            txtDelivery = dgItem.FindControl("txtDelivery")
            hidDelCode = dgItem.FindControl("hidDelCode")
            txtEstDate = dgItem.FindControl("txtEstDate")
            txtWarranty = dgItem.FindControl("txtWarranty")
            txtRemark = dgItem.FindControl("txtRemark")
            cboCategoryCode = dgItem.FindControl("cboCategoryCode")
            cboUOM = dgItem.FindControl("ddl_uom")
            cboAssetGroup = dgItem.FindControl("cboAssetGroup")
            txtGSTAmt = dgItem.FindControl("txtGSTAmt")
            ddlGST = dgItem.FindControl("ddlGST")
            txtGSTAmount = dgItem.FindControl("txtGSTAmount")
            cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")
            cboGift = dgItem.FindControl("cboGift")

            'Jules 2018.10.18
            'cboFundType = dgItem.FindControl("cboFundType")
            'cboPersonCode = dgItem.FindControl("cboPersonCode")
            'cboProjectCode = dgItem.FindControl("cboProjectCode")
            hidFundType = dgItem.FindControl("hidFundType")
            hidPersonCode = dgItem.FindControl("hidPersonCode")
            hidProjectCode = dgItem.FindControl("hidProjectCode")
            hidGLCode = dgItem.FindControl("hidGLCode")
            'End modification.

            chkItem = dgItem.FindControl("chkSelection")

            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
            strItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18

            If (ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr") Or ViewState("modeRFQFromPR_Index") <> "" Then
                Dim objAdmin As New Admin

                If ViewState("modeRFQFromPR_Index") <> "" Then
                    dvwCus = objAdmin.getCustomFieldPR("", "PR", ViewState("modeRFQFromPR_Index"))
                Else
                    dvwCus = objAdmin.getCustomField("", "PO")
                End If

                If Not dvwCus Is Nothing Then
                    For i = 0 To dvwCus.Count - 1
                        Dim cboCustom As DropDownList
                        cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                        If chkItem.Checked Then
                        Else
                            strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                        End If
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
    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        Dim strName As String

        Select Case ViewState("mode")
            Case "po", "rfq"
                Session("POAddItem") = "CloseDirect"
                strName = "selVendor=" & Session("strCompID") & "&Raise=" & "RaisePO" & "&PoNo=" & lblPONo.Text
                strscript.Append("<script language=""javascript"">")
                strFileName = dDispatcher.direct("PO", "Add_PO_Catalogue_Item.aspx", strName)
                strFileName = Server.UrlEncode(strFileName)
                strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','580px');")
                strscript.Append("document.getElementById('btnHidden1').click();")
                strscript.Append("</script>")
                RegisterStartupScript("script3", strscript.ToString())
        End Select
    End Sub
    Sub PopulateVenTypeAhead()
        '
        Dim ventypeahead As String
        Dim i, count As Integer
        Dim vencontent, content2 As String
        Dim strCompID As String
        Dim vtypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=FFPO")
        vencontent &= "$(""#txtVendor"").autocomplete(""" & vtypeahead & "&compid=" & strCompID & """, {" & vbCrLf &
        "selectFirst: false" & vbCrLf &
        "}).result(function(event,data,item) {" & vbCrLf &
        "if (data)" & vbCrLf &
        "document.getElementById(""Hidden6"").value = data[1];" & vbCrLf &
        "$(""#txtVendor"").val(data[0]);" & vbCrLf &
        "$(""#lblCurrencyCode"").val(data[2]);" & vbCrLf &
        "$(""#cboPayTerm"").val(data[3]);" & vbCrLf &
        "$(""#cboPayMethod"").val(data[4]);" & vbCrLf &
        "$(""#hidBillingMethod"").val(data[5]);" & vbCrLf &
        "$(""#hidTaxCalcBy"").val(data[6]);" & vbCrLf &
        "$(""#hidGSTCode"").val(data[7]);" & vbCrLf &
        "//$(""#btnGetAdd"").trigger('click');" & vbCrLf &
        "$(""#btnHidden2"").trigger('click');" & vbCrLf &
        "});" & vbCrLf
        '
        ' for edit purpose
        If Session("Action") = "Edit" Then
            ventypeahead = "<script language=""javascript"">" & vbCrLf &
                      "<!--" & vbCrLf &
                        "$(document).ready(function(){" & vbCrLf &
                        vencontent & vbCrLf &
                        "});" & vbCrLf &
                        "-->" & vbCrLf &
                        "</script>"
        Else
            ventypeahead = "<script language=""javascript"">" & vbCrLf &
          "<!--" & vbCrLf &
            "$(document).ready(function(){" & vbCrLf &
            vencontent & vbCrLf &
            "});" & vbCrLf &
            "-->" & vbCrLf &
            "</script>"

        End If

        Session("ventypeahead") = ventypeahead
        If Not Hidden6.Value.ToString = "" Then
            Session("VendorID") = Hidden6.Value
        End If
        If Not hidBillingMethod.Value.ToString = "" Then ViewState("strGST") = hidBillingMethod.Value
        If Not hidBillingMethod.Value.ToString = "" Then Session("BillingMethod") = hidBillingMethod.Value
        If IsDBNull(Session("BillingMethod")) Then
            Session("BillingMethod") = "FPO"
        End If

    End Sub

    Private Sub cmdUploadPO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUploadPO.Click
        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        Dim strName As String
        Select Case ViewState("mode")
            Case "po", "rfq"
                SavePOHeaderDetail()
                Session("POAddItem") = "CloseDirect"
                strName = "selVendor=" & Session("strCompID") & "&Raise=" & "UploadPO" & "&vencomp=" & Hidden6.Value & "&docno=" & lblPONo.Text
                Session("FFPOAddrCode") = Me.cboBillCode.SelectedItem.Text.Trim()
                strscript.Append("<script language=""javascript"">")
                strFileName = dDispatcher.direct("PO", "POBatchUpload.aspx", strName)
                strFileName = Server.UrlEncode(strFileName)
                strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','580px');")
                strscript.Append("document.getElementById('btnHidden1').click();")
                strscript.Append("</script>")
                RegisterStartupScript("script3", strscript.ToString())
        End Select
    End Sub

    Private Sub SavePOHeaderDetail()
        Dim txtQty, txtPrice, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark, txtGSTAmt, lblProductDesc, txtGSTAmount As TextBox
        Dim lblItemLine, txtBudget, txtDelivery As Label
        'Dim cboGLCode, cboCust, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList
        Dim cboCust, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST, cboGSTTaxCode As DropDownList 'Jules 2018.10.24 U00019
        Dim strItem, strItemCust As New ArrayList()
        Dim dgItem As DataGridItem
        Dim k As Integer = 0
        Dim objAdmin As New Admin
        Dim strItemHead As New ArrayList()
        Dim cboGift As DropDownList ', cboFundType, cboPersonCode, cboProjectCode 'Jules 2018.07.14; Jules 2018.10.18
        Dim hidFundType, hidPersonCode, hidProjectCode, hidGLCode As TextBox 'Jules 2018.10.18

        strItemHead.Add(New String() {txtAttention.Text, txtInternal.Text, txtExternal.Text, cboBillCode.SelectedIndex, cboShipmentTerm.SelectedIndex, cboShipmentMode.SelectedIndex, txtShipVia.Text, chkUrgent.Checked, txtShippingHandling.Text, Session("venId"), cboPayTerm.SelectedIndex, cboPayMethod.SelectedIndex, lblCurrencyCode.Text})

        For Each dgItem In dtgShopping.Items
            txtQty = dgItem.FindControl("txtQty")
            'cboGLCode = dgItem.FindControl("cboGLCode") 'Jules 2018.10.24 U00019
            txtPrice = dgItem.FindControl("txtPrice")
            txtBudget = dgItem.FindControl("txtBudget")
            hidBudgetCode = dgItem.FindControl("hidBudgetCode")
            txtDelivery = dgItem.FindControl("txtDelivery")
            hidDelCode = dgItem.FindControl("hidDelCode")
            txtEstDate = dgItem.FindControl("txtEstDate")
            txtWarranty = dgItem.FindControl("txtWarranty")
            txtRemark = dgItem.FindControl("txtRemark")
            cboCategoryCode = dgItem.FindControl("cboCategoryCode")
            cboUOM = dgItem.FindControl("ddl_uom")
            cboAssetGroup = dgItem.FindControl("cboAssetGroup")
            txtGSTAmt = dgItem.FindControl("txtGSTAmt")
            lblProductDesc = dgItem.FindControl("lblProductDesc")
            ddlGST = dgItem.FindControl("ddlGST")
            txtGSTAmount = dgItem.FindControl("txtGSTAmount")
            'Stage 3 Enhancement (GST-0010) - 24/07/2015 - CH
            cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")

            'Jules 2018.07.14
            cboGift = dgItem.FindControl("cboGift")

            'Jules 2018.10.18
            'cboFundType = dgItem.FindControl("cboFundType")
            'cboPersonCode = dgItem.FindControl("cboPersonCode")
            'cboProjectCode = dgItem.FindControl("cboProjectCode")
            hidFundType = dgItem.FindControl("hidFundType")
            hidPersonCode = dgItem.FindControl("hidPersonCode")
            hidProjectCode = dgItem.FindControl("hidProjectCode")
            hidGLCode = dgItem.FindControl("hidGLCode")
            'End modification.

            'Stage 3 Enhancement (GST-0010) - 24/07/2015 - CH
            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text})

            'Jules 2018.07.14 - PAMB
            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedIndex})
            'strItem.Add(New String() {cboGLCode.SelectedIndex, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, cboFundType.SelectedIndex, cboPersonCode.SelectedIndex, cboProjectCode.SelectedIndex})
            strItem.Add(New String() {hidGLCode.Text, txtQty.Text, txtPrice.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt.Text, ddlGST.SelectedValue, txtGSTAmount.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18

            If (ViewState("ListingFromRFQ") <> "True" And ViewState("modePR") <> "pr") Or ViewState("modeRFQFromPR_Index") <> "" Then

                If ViewState("modeRFQFromPR_Index") <> "" Then
                    dvwCus = objAdmin.getCustomFieldPR("", "PR", ViewState("modeRFQFromPR_Index"))
                Else
                    dvwCus = objAdmin.getCustomField("", "PO")
                End If

                If Not dvwCus Is Nothing Then
                    For i = 0 To dvwCus.Count - 1
                        Dim cboCustom As DropDownList
                        cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)
                        strItemCust.Add(New String() {k, cboCustom.SelectedIndex})

                        'If chkItem.Checked Then
                        'Else
                        '    strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                        'End If
                    Next
                End If
            End If

            k += 1
        Next
        Session("strItem") = strItem
        Session("strItemCust") = strItemCust
        Session("strItemHead") = strItemHead
    End Sub


    Private Sub btnGetAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetAdd.Click

        Dim dsCompDetail As New DataSet
        Dim objIPPMain As New IPPMain

        If txtVendor.Text <> "" Then
            GetVendorDetail()
        End If

    End Sub

    Public Sub AddAssetGroup()
        Try
            Dim strsql = ""
            Dim ds As New DataSet
            Dim strAryQuery(0) As String

            strsql = "SELECT POD_PO_NO, POD_PO_LINE, POD_ASSET_GROUP FROM PO_DETAILS WHERE POD_PO_NO = '" & lblPONo.Text & "' "
            strsql &= "AND (POD_ASSET_GROUP IS NOT NULL AND POD_ASSET_GROUP <> '') AND POD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            ds = objDB.FillDs(strsql)

            Dim strAssetNo As String
            Dim intAssetIncrementNo As Integer
            intAssetIncrementNo = 1
            For i = 0 To ds.Tables(0).Rows.Count - 1
                Common.parseNull(ds.Tables(0).Rows(i)("POD_ASSET_GROUP"))
                Dim strParam As String = objDB.GetVal("SELECT IFNULL(CP_PARAM_VALUE,'') AS CP_PARAM_VALUE FROM COMPANY_PARAM WHERE CP_PARAM_TYPE = '" & ds.Tables(0).Rows(i)("POD_ASSET_GROUP") & "' AND CP_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                If strParam = "" Then
                    strsql = "INSERT INTO COMPANY_PARAM (CP_COY_ID, CP_PARAM_NAME, CP_PARAM_VALUE, CP_PARAM_TYPE, CP_APP_PKG) VALUES ( "
                    strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                    strsql &= "'Prefix', "
                    strsql &= "'" & ds.Tables(0).Rows(i)("POD_ASSET_GROUP") & "', "
                    strsql &= "'" & ds.Tables(0).Rows(i)("POD_ASSET_GROUP") & "', "
                    strsql &= "'eProcure' ) "
                    Common.Insert2Ary(strAryQuery, strsql)

                    strsql = "INSERT INTO COMPANY_PARAM (CP_COY_ID, CP_PARAM_NAME, CP_PARAM_VALUE, CP_PARAM_TYPE, CP_APP_PKG) VALUES ( "
                    strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
                    strsql &= "'Last Used No', "
                    strsql &= "'00000000', "
                    strsql &= "'" & ds.Tables(0).Rows(i)("POD_ASSET_GROUP") & "', "
                    strsql &= "'eProcure' ) "
                    Common.Insert2Ary(strAryQuery, strsql)

                    objDB.BatchExecute(strAryQuery)
                End If
            Next

        Catch ex As Exception

        End Try
    End Sub

    'Jules 2018.07.14 - PAMB
    Protected Sub cboGLCode_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim cboGLCode, cboGift As New DropDownList
        Dim dgitem As DataGridItem = CType((CType(sender, Control)).Parent.Parent, DataGridItem)
        cboGLCode = dgitem.FindControl("cboGLCode")
        cboGift = dgitem.FindControl("cboGift")

        If cboGLCode.SelectedValue.ToString.Substring(0, 1) = "1" Then
            cboGift.SelectedValue = "N"
            cboGift.Enabled = False
        Else
            cboGift.Enabled = True
        End If
    End Sub

    'Jules 2018.10.08 - SST
    Private Function validateSSTRates(ByRef strMsg As String) As Boolean
        validateSSTRates = True

        Dim dgItem As DataGridItem
        Dim objGST As New GST
        Dim strGstRegNo As String = ""
        If Request.QueryString("vendor") IsNot Nothing Then
            ViewState("SSTRegNo") = objGST.chkGST(Request.QueryString("vendor"))
            strGstRegNo = objGST.chkGST(Request.QueryString("vendor"))
        ElseIf ViewState("POM_S_COY_ID") IsNot Nothing Then
            ViewState("SSTRegNo") = objGST.chkGST(ViewState("POM_S_COY_ID"))
            strGstRegNo = objGST.chkGST(ViewState("POM_S_COY_ID"))
        ElseIf Hidden6.Value <> "" Then
            ViewState("SSTRegNo") = objGST.chkGST(Hidden6.Value)
            strGstRegNo = objGST.chkGST(ViewState(Hidden6.Value))
        Else
            ViewState("SSTRegNo") = objGST.chkGST()
            strGstRegNo = objGST.chkGST()
        End If

        If ViewState("SSTRegNo") <> "" Then
            Dim strGSTRate As String = ""
            Dim blnSST As Boolean = False


            For Each dgItem In dtgShopping.Items
                Dim ddlGST As DropDownList
                ddlGST = dgItem.FindControl("ddlGST")

                If strGSTRate <> "" AndAlso ddlGST.SelectedValue <> strGSTRate Then
                    If strGSTRate = "ST6" Then
                        If ddlGST.SelectedValue = "ST5" OrElse ddlGST.SelectedValue = "ST10" Then
                            strMsg &= "<ul type='disc'><li> Sales Tax item(s) and Service Tax item(s) cannot be combined in one document.</li></ul>"
                            validateSSTRates = False
                            Return validateSSTRates
                        End If
                        blnSST = True
                    ElseIf strGSTRate = "ST5" OrElse strGSTRate = "ST10" Then
                        If ddlGST.SelectedValue = "ST6" Then
                            strMsg &= "<ul type='disc'><li> Sales Tax item(s) and Service Tax item(s) cannot be combined in one document.</li></ul>"
                            validateSSTRates = False
                            Return validateSSTRates
                        End If
                        blnSST = True
                    End If
                End If
                strGSTRate = ddlGST.SelectedValue
            Next

            'Check whether tax code is valid.
            For Each dgItem In dtgShopping.Items
                Dim cboGSTTaxCode As DropDownList
                Dim ddlGST As DropDownList
                cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")
                ddlGST = dgItem.FindControl("ddlGST")

                If cboGSTTaxCode.SelectedValue = "" Then
                    If strMsg = "" OrElse strMsg = "<ul type='disc'></ul>" Then
                        strMsg = "<ul type='disc'><li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Purchase Tax Code is required.</li>"
                    Else
                        strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Purchase Tax Code is required.</li>"
                    End If
                    validateSSTRates = False
                Else
                    If objGST.chkValidTaxCode(ddlGST.SelectedItem.Value, cboGSTTaxCode.SelectedValue, "P") = False Then
                        If strMsg = "" OrElse strMsg = "<ul type='disc'></ul>" Then
                            strMsg = "<ul type='disc'><li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Invalid Purchase Tax Code.</li>"
                        Else
                            strMsg &= "<li>" & dgItem.Cells(EnumShoppingCart.icNo).Text & ". Invalid Purchase Tax Code.</li>"
                        End If
                        validateSSTRates = False
                    End If
                End If
            Next

            If strMsg <> "" Then
                strMsg &= "</ul>"
            End If

        End If
    End Function

    'Jules 2018.10.08
    Public Sub ddlGST_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGST.SelectedIndexChanged
        Dim dtgItem As DataGridItem = DirectCast(DirectCast(sender, Control).NamingContainer, DataGridItem)
        Dim ddlGST As DropDownList = dtgItem.FindControl("ddlGST")
        Dim cboGSTTaxCode As DropDownList = dtgItem.FindControl("cboGSTTaxCode")
        Dim LineNo As Integer = dtgItem.Cells(EnumShoppingCart.icNo).Text
        Dim objGlobal As New AppGlobals
        Dim newTaxRate As String = ""
        Dim newTaxCode As String = ""

        objGlobal.FillTaxCode(cboGSTTaxCode, ddlGST.SelectedValue, "P", , , True)

        If cboGSTTaxCode.SelectedValue = "N/A" Then
            cboGSTTaxCode.Enabled = False
        Else
            cboGSTTaxCode.Enabled = True
        End If
        newTaxRate = ddlGST.SelectedValue
        newTaxCode = cboGSTTaxCode.SelectedValue

        ViewState("CurrentRate") = ddlGST.ClientID 'Jules 2019.01.14
        If ddlGST.SelectedValue <> "" Then
            Dim TaxPerc As Decimal = 0.0
            TaxPerc = objDB.GetVal("SELECT IF(TAX_PERC='',0,TAX_PERC) AS TAX_PERC FROM TAX WHERE TAX_CODE = '" & ddlGST.SelectedValue & "'")

            Dim txtQty As TextBox = dtgItem.FindControl("txtQty")
            Dim txtPrice As TextBox = dtgItem.FindControl("txtPrice")
            Dim txtGSTAmount As TextBox = dtgItem.FindControl("txtGSTAmount")
            Dim txtGSTAmt As TextBox = dtgItem.FindControl("txtGSTAmt")
            Dim txtGST As TextBox = dtgItem.FindControl("txtGST")
            Dim hidtaxperc As TextBox = dtgItem.FindControl("hidtaxperc")

            txtGST.Text = FormatNumber(TaxPerc, 2)
            hidtaxperc.Text = txtGST.Text

            If txtQty.Text <> "" AndAlso txtPrice.Text <> "" Then
                dtgItem.Cells(EnumShoppingCart.icTax).Enabled = True
                txtGSTAmount.Text = CDec(txtQty.Text) * CDec(txtPrice.Text) * TaxPerc / 100
                txtGSTAmount.Text = FormatNumber(txtGSTAmount.Text, 2)
                txtGSTAmt.Text = txtGSTAmount.Text
                dtgItem.Cells(EnumShoppingCart.icTax).Enabled = False
            End If
        End If

        'ddlGST.Focus() 'Jules 2019.01.14 commented.

        Dim chkItem As CheckBox
        Dim txtQty1, txtPrice1, hidBudgetCode, hidDelCode, txtEstDate, txtWarranty, txtRemark, txtGSTAmt1, txtGSTAmount1 As TextBox
        Dim lblItemLine, txtBudget, txtDelivery As Label
        Dim cboCust, cboCategoryCode, cboUOM, cboAssetGroup, ddlGST1, cboGSTTaxCode1 As DropDownList
        Dim cboGift As DropDownList
        Dim hidFundType, hidPersonCode, hidProjectCode, hidGLCode As TextBox
        Dim strItem, strItemCust As New ArrayList()
        Dim dgItem As DataGridItem
        Dim dgItem2 As DataGridItem
        Dim LineNum As Integer
        Dim k As Integer = 0
        Dim strItemHead As New ArrayList()

        strItemHead.Add(New String() {txtAttention.Text, txtInternal.Text, txtExternal.Text, cboBillCode.SelectedIndex, cboShipmentTerm.SelectedIndex, cboShipmentMode.SelectedIndex, txtShipVia.Text, chkUrgent.Checked, txtShippingHandling.Text, Session("venId"), cboPayTerm.SelectedIndex, cboPayMethod.SelectedIndex, lblCurrencyCode.Text})

        For Each dgItem In dtgShopping.Items
            txtQty1 = dgItem.FindControl("txtQty")
            txtPrice1 = dgItem.FindControl("txtPrice")
            txtBudget = dgItem.FindControl("txtBudget")
            hidBudgetCode = dgItem.FindControl("hidBudgetCode")
            txtDelivery = dgItem.FindControl("txtDelivery")
            hidDelCode = dgItem.FindControl("hidDelCode")
            txtEstDate = dgItem.FindControl("txtEstDate")
            txtWarranty = dgItem.FindControl("txtWarranty")
            txtRemark = dgItem.FindControl("txtRemark")
            cboCategoryCode = dgItem.FindControl("cboCategoryCode")
            cboUOM = dgItem.FindControl("ddl_uom")
            cboAssetGroup = dgItem.FindControl("cboAssetGroup")
            txtGSTAmt1 = dgItem.FindControl("txtGSTAmt")
            ddlGST = dgItem.FindControl("ddlGST")
            txtGSTAmount1 = dgItem.FindControl("txtGSTAmount")
            cboGSTTaxCode = dgItem.FindControl("cboGSTTaxCode")
            cboGift = dgItem.FindControl("cboGift")
            hidFundType = dgItem.FindControl("hidFundType")
            hidPersonCode = dgItem.FindControl("hidPersonCode")
            hidProjectCode = dgItem.FindControl("hidProjectCode")
            hidGLCode = dgItem.FindControl("hidGLCode")
            chkItem = dgItem.FindControl("chkSelection")
            LineNum = dgItem.Cells(EnumShoppingCart.icNo).Text

            If LineNum = LineNo Then
                strItem.Add(New String() {hidGLCode.Text, txtQty1.Text, txtPrice1.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt1.Text, newTaxRate, txtGSTAmount1.Text, newTaxCode, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18
            Else
                strItem.Add(New String() {hidGLCode.Text, txtQty1.Text, txtPrice1.Text, txtBudget.Text, hidBudgetCode.Text, txtDelivery.Text, hidDelCode.Text, txtEstDate.Text, txtWarranty.Text, txtRemark.Text, cboCategoryCode.SelectedIndex, cboUOM.SelectedIndex, cboAssetGroup.SelectedIndex, txtGSTAmt1.Text, ddlGST.SelectedValue, txtGSTAmount1.Text, cboGSTTaxCode.SelectedValue, cboGift.SelectedIndex, hidFundType.Text, hidPersonCode.Text, hidProjectCode.Text}) 'Jules 2018.10.18
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
                        Dim cboCustom As DropDownList
                        cboCustom = dgItem.Cells(EnumShoppingCart.icRemark + i).Controls(0)

                        If chkItem.Checked Then
                        Else
                            strItemCust.Add(New String() {k, cboCustom.SelectedIndex})
                        End If
                    Next
                End If
            End If

            k += 1
        Next
        Session("strItem") = strItem
        Session("strItemCust") = strItemCust
        Session("strItemHead") = strItemHead
        ViewState("ChangedRate") = "1" 'Jules 2019.01.14
        Bindgrid()

    End Sub
End Class
