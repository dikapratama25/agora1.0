Imports AgoraLegacy
Imports eProcure.Component
Imports System.drawing
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient


Partial Public Class IPPAOApprovalDetail
    Inherits AgoraLegacy.AppBaseClass
    Dim dblInvoiceAmount, prevAppType As String
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals
    Dim objAppExcel As New AppExcel
    Dim objDb As New EAD.DBCom
    Dim VenIdx As Integer = 0
    Dim IPPTimeFr1 As String = System.Configuration.ConfigurationManager.AppSettings.Get("IPPTimeFr1")
    Dim IPPTimeTo1 As String = System.Configuration.ConfigurationManager.AppSettings.Get("IPPTimeTo1")
    Dim IPPTimeFr2 As String = System.Configuration.ConfigurationManager.AppSettings.Get("IPPTimeFr2")
    Dim IPPTimeTo2 As String = System.Configuration.ConfigurationManager.AppSettings.Get("IPPTimeTo2")
    Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A - CH - 5 Feb 2015
    Dim TimeNow As String = DateTime.Now.ToLocalTime.ToString("HH:mm")
    Dim strFrm As String
    Dim _exceedCutOffDt As String = ""
    Dim dvViewInput, dvViewOutput As DataView

    'Zulham 13112018 - PAMB
    Dim objAdmin As New Admin
    Dim dsExchangeRate As New DataSet

    Public Enum EnumSubDocDet
        subDocNo = 0
        subDocDate = 1
        subDocAmt = 2
        subEmpty = 3
        subDocGSTValue = 4
    End Enum

    Public Enum EnumIPPDet
        ippSNo = 0
        ippPayFor = 1
        ippReimbursement = 2 'Zulham Sept 18, 2014
        ippInvoiceNo = 3
        ippDesc = 4
        ippUOM = 5
        ippQty = 6
        ippUnitPrice = 7
        'Zulham 13112018
        ippForeignAmt = 8
        ippAmt = 9
        'Zulham Sept 18, 2014
        ippGSTAmount = 10
        ippInputTax = 11
        ippOutputTax = 12
        'End
        ippGL = 13
        'Zulham 11072018 - Moved to here
        ippCostCentre = 14
        'Zulham 11072018 - PAMB
        ippGift = 15
        ippCategory = 16
        'End
        'Zulham 11072018 - PAMB
        ippACode1 = 17
        ippACode2 = 18
        ippACode3 = 19
        ippACode4 = 20
        ippACode5 = 21
        ippACode8 = 22
        ippACode9 = 23
        'End
        ippAssetGrp = 24
        ippAssetSub = 25
        ippGLRuleCat = 26
        ippBranch = 27

        'Zulham 11072018 - PAMB
        ippWHT = 28
        'End
        ippCostAlloc = 29
        'Zulham 12082018 - PAMB
        ippCurrency = 30

    End Enum

    Private Sub Page_AbortTransaction(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.AbortTransaction

    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim objIPP As New IPP
        Dim ObjDoc As New IPPMain
        Dim objIPPDetail As New IPPDetails
        Dim blnRelief As Boolean
        Dim intApprGrpIndex As String

        strFrm = Me.Request.QueryString("Frm")
        Session("DocNo") = Request.QueryString("DocumentNo") 'Zulham 14072018 - PAMB
        Session("Index") = Request.QueryString("index")
        'ViewState("gstTotal") = 0

        intApprGrpIndex = objDb.GetVal("SELECT DISTINCT FA_APPROVAL_GRP_INDEX FROM finance_approval WHERE FA_INVOICE_INDEX = '" & Session("Index") & "' ")

        VenIdx = objDb.GetVal("SELECT im_s_coy_id FROM invoice_mstr WHERE im_invoice_index = '" & Session("Index") & "'")

        'Zulham 16/02/2016 - IPP Stage 4 Phase 2
        ViewState("VenIdx") = VenIdx

        'blnPaging = False
        blnSorting = False

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgApprvFlow, "N")
        SetGridProperty(dtgSubDocDetail, "Y")
        SetGridProperty(dtgDocDetail, "N")
        Session("AO") = Session("UserID")
        Dim objUsers As New Users
        Dim blnIPPAO As Boolean

        blnIPPAO = objUsers.checkUserFixedRole("'IPP Approving Officer'")
        ViewState("role") = getUserRole(blnIPPAO)
        If ObjDoc.CheckRelief(intApprGrpIndex) Then
            blnRelief = False ' temp set to false unless user want to use this function - michael
            Session("Relief") = blnRelief
        Else
            blnRelief = False
            Session("Relief") = blnRelief
        End If

        GenerateTab()
        renderIPPApprFlow()
        ViewState("total") = 0

        Bindgrid()
        If ViewState("intPageRecordCnt") > 0 Then
            AddRowtotal()
        End If

        ViewState("line") = 0
        If Not Page.IsPostBack Then

            'Zulham 10/04/2018 - PAMB
            Dim objPO1 As New PurchaseOrder_Buyer

            If Session("strItem") Is Nothing Then
                'Delete those temp records created in the previous session (incase user exit IE without proper log off)
                objPO1.delete_Attachment(Session.SessionID)
            End If
            cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & File1Int.ClientID & "');")
            'End

            ViewState("isResident") = ""
            objIPPDetail = objIPP.GetApprIPPDetails(Session("index"), Session("DocNo").ToString.Replace("\", "\\"), Common.Parse(HttpContext.Current.Session("CompanyID"))) 'Zulham 14072018 - PAMB
            If objIPPDetail.DocType = "CN" Or objIPPDetail.DocType = "DN" Then
                Me.dtgDocDetail.Columns(EnumIPPDet.ippInvoiceNo).Visible = True
            Else
                Me.dtgDocDetail.Columns(EnumIPPDet.ippInvoiceNo).Visible = False
            End If
            If objIPPDetail.DocType = "INV" Then
                Me.lblDocType.Text = "Invoice"
            ElseIf objIPPDetail.DocType = "CN" Then
                Me.lblDocType.Text = "Credit Note"
            ElseIf objIPPDetail.DocType = "DN" Then
                Me.lblDocType.Text = "Debit Note"
            ElseIf objIPPDetail.DocType = "BILL" Then
                Me.lblDocType.Text = "Bill"
            ElseIf objIPPDetail.DocType = "LETTER" Then
                Me.lblDocType.Text = "Letter"
            End If
            Me.lblMasterDoc.Text = IIf(objIPPDetail.MasterDoc = "Y", "Yes", "No")
            ViewState("MasterDoc") = objIPPDetail.MasterDoc
            Me.lblDocNo.Text = objIPPDetail.DocNo
            Me.lblDocDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objIPPDetail.DocDate)
            Me.lblManualPONo.Text = objIPPDetail.ManualPONo
            Me.lblStatus.Text = objIPPDetail.StatusDescription
            Me.lblCurrency.Text = objIPPDetail.Currency
            Me.lblPaymentAmt.Text = Format(objIPPDetail.PaymentAmt, "Standard")

            If objIPPDetail.PaymentMethod = "BC" Or objIPPDetail.PaymentMethod = "TT" Then
                If objIPPDetail.BankerChequeNo = "" Then
                    Me.lblPaymentMethod.Text = objIPPDetail.PaymentMethod & "[ - ]"
                Else
                    Me.lblPaymentMethod.Text = objIPPDetail.PaymentMethod & "[BC#" & objIPPDetail.BankerChequeNo & "]"
                End If

            Else
                Me.lblPaymentMethod.Text = objIPPDetail.PaymentMethod
                'Zulham 02/03/2015 IPP GST Stage 2B
                If lblPaymentMethod.Text.Trim.ToLower = "nostro" Then
                    lblPaymentMethod.Text = "Nostro Expenses"
                End If
            End If

            'Zulham 23112018
            Select Case objIPPDetail.PaymentMethod
                Case "IBG"
                    lblPaymentMethodFull.Text = "LOCAL BANK TRANSFER-(RM)"
                Case "TT"
                    lblPaymentMethodFull.Text = "TELEGRAPHIC TRANSFER-(FOREIGN CURRENCY)"
                Case "BC"
                    lblPaymentMethodFull.Text = "CHEQUE-(RM)"
                Case "BD"
                    lblPaymentMethodFull.Text = "BANK DRAFT-(FOREIGN CURRENCY)"
                Case "CO"
                    lblPaymentMethodFull.Text = "CASHIER'S ORDER-(RM)"
            End Select

            'Zulham 05/01/2016 - Stage 4 Phase 2
            'get the current document status
            Session("Status") = objIPPDetail.Status.Trim

            'Zulham 09072018 - PAMB
            'Me.txtTax.Text = objIPPDetail.WHTTax
            'Me.rbtnWHTOpt.SelectedValue = objIPPDetail.WHTOpt
            'Me.txtNoWHT.Text = objIPPDetail.WHTReason
            'End

            'Me.lblVendor.Text = objIPPDetail.Vendor
            If objIPPDetail.GSTRegNo <> "" Then
                Me.lblVendor.Text = objIPPDetail.Vendor & " (" & objIPPDetail.GSTRegNo & ")"
            Else
                Me.lblVendor.Text = objIPPDetail.Vendor
            End If
            ViewState("VendorName") = objIPPDetail.Vendor
            Me.lblVendorAddr.Text = Replace(objIPPDetail.VAddr, "<BR>", "<BR>&nbsp;") & "<BR>&nbsp;"
            Me.txtRemarks.Text = objIPPDetail.Remarks
            Me.txtExchangeRate.Text = objIPPDetail.ExchangeRate
            txtLateSubmit.Text = objIPPDetail.LateSubmitReason

            'Me.lblBankNameAccountNo.Text = objIPPDetail.BankNameAccountNo
            'Zulham 17042015 IPP GST Stage 2B
            If Not objIPPDetail.BankNameAccountNo = "[]" Then
                Me.lblBankNameAccountNo.Text = objIPPDetail.BankNameAccountNo
            Else
                Me.lblBankNameAccountNo.Text = ""
            End If

            Me.lblDocDueDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objIPPDetail.DocDueDate)
            If objIPPDetail.CreditTerm <> "" Then
                Me.lblCreditTerm.Text = objIPPDetail.CreditTerm & " days"
            Else
                Me.lblCreditTerm.Text = ""
            End If

            txtRemarks.Enabled = False

            'zulham 17/02/2016 - IPP Stage 4 Phase 2
            'Show employee reg ID
            Dim coyType = objDb.GetVal("select ifnull(ic_coy_type,'') 'coyType' from ipp_company where ic_index = '" & VenIdx & "'")
            Dim empRegId = objDb.GetVal("select ifnull(ic_business_reg_no,'') 'regNo' from ipp_company where ic_index = '" & VenIdx & "'")
            If coyType.ToString.Trim = "E" Then
                If empRegId.ToString.Trim.Length > 0 Then
                    Me.lblVendor.Text = objIPPDetail.Vendor & " (" & empRegId & ")"
                Else
                    Me.lblVendor.Text = objIPPDetail.Vendor
                End If
            End If
            'Show total w/o GST & Gst Amount
            If Not objIPPDetail.TotalAmtNoGST.Trim = "" Then Me.lblTotalAmtNoGST.Text = FormatNumber(objIPPDetail.TotalAmtNoGST, 2)
            If Not objIPPDetail.GSTAmt.Trim = "" Then Me.lblGSTAmt.Text = FormatNumber(objIPPDetail.GSTAmt, 2)


            'Check if it's employee
            Dim compType = objDb.GetVal("SELECT IFNULL(ic_COY_TYPE, '') FROM IPP_COMPANY WHERE ic_coy_name = '" & Common.Parse(objIPPDetail.Vendor) & "'")
            'Zulham 13/10/2015 - Added condition that is coytype = 'V'
            ViewState("isResident") = objDb.GetVal("SELECT ic_resident_type FROM IPP_company WHERE ic_coy_type = 'V' and ic_coy_name =  '" & Common.Parse(objIPPDetail.Vendor) & "'")
            If compType = "E" Then
                Me.lblJobGrade_Val.Text = objIPPDetail.jobGrade
                Me.trJobGrade.Visible = True
            Else
                Me.lblJobGrade.Text = ""
                Me.lblJobGrade_Val.Text = ""
                Me.trJobGrade.Visible = False
            End If
            'END

            'Zulham 01042015 Case 8603
            'check for payment method TT to hide the button        
            If objIPPDetail.PaymentMethod = "TT" Or objIPPDetail.PaymentMethod.ToUpper.Contains("NOSTRO") Then
                If ViewState("role") = 2 Then
                    TRExchangeRate.Visible = False
                    'Zulham 09072018 - PAMB
                    'txtTax.Enabled = False
                    'rbtnWHTOpt.Enabled = False
                    'txtNoWHT.Enabled = False
                    'End
                ElseIf ViewState("role") = 3 Then
                    TRExchangeRate.Visible = True
                    'Zulham 09072018 - PAMB
                    'txtTax.Enabled = False
                    'rbtnWHTOpt.Enabled = False
                    'txtNoWHT.Enabled = False
                    'End
                    cmdAppIPP.Attributes.Add("onclick", "return validate();")
                End If
            Else
                TRExchangeRate.Visible = False
                'Zulham 09072018 - PAMB
                'txtTax.Enabled = False
                'rbtnWHTOpt.Enabled = False
                'txtNoWHT.Enabled = False
                'End
            End If

            'Zulham 01042015 Case 8603
            If objIPPDetail.PaymentMethod = "RENTAS" Or objIPPDetail.PaymentMethod = "TT" Or objIPPDetail.PaymentMethod.ToUpper.Contains("NOSTRO") Then
                Me.txtBenficiaryDetails.Text = objIPPDetail.BeneficiaryDetails
            Else
                Me.txtBenficiaryDetails.Enabled = False
            End If

            objIPPDetail = Nothing

            If ((CDate(TimeNow) >= CDate(IPPTimeFr1) And CDate(TimeNow) <= CDate(IPPTimeTo1)) Or (CDate(TimeNow) >= CDate(IPPTimeFr2) And CDate(TimeNow) <= CDate(IPPTimeTo2))) And ViewState("role") = "3" Then
                cmdAppIPP.Visible = False
                cmdRejectIPP.Visible = False
            End If

            'for read only 
            If Request.QueryString("mode") = "read" Then
                txtRemarks.Enabled = False
                'Zulham 09072018 - PAMB
                'txtTax.Enabled = False
                'txtNoWHT.Enabled = False
                'End
                txtApprRejRemark.Visible = False
                Label43.Visible = False
                cmdAppIPP.Visible = False
                cmdRejectIPP.Visible = False
                cmdViewAudit.Visible = True
                'Zulham 06082018 - PAMB
                If Session("CompanyID").ToString.ToUpper = "PAMB" Then lblAction.Text = ""
            End If
        End If

        'Zulham 10/04/2018 - PAMB
        displayAttachFile()
        'End

        'Sub Document Detail Panel
        ViewState("totalSubAmt") = 0
        If ViewState("MasterDoc") = "Y" Then
            tbSubDoc.Style("Display") = ""
            BindgridSubDoc()
            If ViewState("intPageRecordCnt2") > 0 Then
                AddRowtotalSubDoc()
            End If
        Else
            tbSubDoc.Style("Display") = "none"
        End If

        If Request.QueryString("frm") = "dashboard" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx")
        End If

        'Check for GST
        Dim documentDate = objDb.GetVal("SELECT IFNULL(im_doc_date,'') 'im_doc_date' FROM invoice_mstr WHERE im_invoice_index = '" & Session("Index") & "'")
        Dim createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
        Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
        'Dim _exceedCutOffDt As String = ""

        'Zulham 13082015
        'Additional condition for company's effective date
        Dim effectiveDate = objDb.GetVal("SELECT IFNULL(ic_gst_eff_date, '') FROM IPP_COMPANY WHERE ic_index = '" & ViewState("VenIdx") & "'")

        Dim strIsGst As String
        If Trim(effectiveDate) <> "" Then
            If CDate(createdDate) >= CDate(_cutoffDate) And CDate(createdDate) >= CDate(effectiveDate) Then
                _exceedCutOffDt = "Yes"
                If lblVendor.Text <> "" Then
                    Dim GSTRegNo = objDb.GetVal("SELECT IFNULL(IC_TAX_REG_NO, '') FROM IPP_COMPANY WHERE ic_index = '" & ViewState("VenIdx") & "'")
                    If GSTRegNo <> "" And CDate(documentDate) >= CDate(_cutoffDate) And CDate(documentDate) >= CDate(effectiveDate) Then
                        strIsGst = "Yes"
                    Else
                        strIsGst = "No"
                    End If
                Else
                    strIsGst = "Yes"
                End If
            Else
                strIsGst = "No"
            End If
        Else
            strIsGst = "No"
        End If
        'Zulham 19082015
        If CDate(createdDate) >= CDate(_cutoffDate) Then
            _exceedCutOffDt = "Yes"
        End If

        Dim Asset As New PurchaseOrder_Buyer
        If strIsGst = "Yes" Then
            dtgDocDetail.Columns(EnumIPPDet.ippReimbursement).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippGSTAmount).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippInputTax).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippOutputTax).Visible = True
            dtgSubDocDetail.Columns(EnumIPPDet.ippInvoiceNo).Visible = True
        ElseIf _exceedCutOffDt = "Yes" Then
            dtgDocDetail.Columns(EnumIPPDet.ippReimbursement).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippGSTAmount).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippInputTax).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippOutputTax).Visible = True
            dtgSubDocDetail.Columns(EnumIPPDet.ippInvoiceNo).Visible = True
        Else
            dtgDocDetail.Columns(EnumIPPDet.ippReimbursement).Visible = False
            dtgDocDetail.Columns(EnumIPPDet.ippGSTAmount).Visible = False
            dtgDocDetail.Columns(EnumIPPDet.ippInputTax).Visible = False
            dtgDocDetail.Columns(EnumIPPDet.ippOutputTax).Visible = False
            dtgSubDocDetail.Columns(EnumIPPDet.ippInvoiceNo).Visible = False
        End If
        dtgDocDetail.Columns(EnumIPPDet.ippCostAlloc).Visible = False 'Hide cost allocation
        'End

        'Zulham PAMB - 11/04/2018
        If Not hidFileIndex.Value = "" Then
            removeAttachment()
        End If

        cmdRejectIPP.Attributes.Add("onclick", "return confirmReject();")

        'Zulham 02112018 - PAMB
        Dim objAdmin As New Admin
        Dim exchangeRate As Decimal = 1
        Dim dsExchangeRate As New DataSet : dsExchangeRate = objAdmin.getexrate(lblCurrency.Text, "")
        If Not dsExchangeRate.Tables(0).Rows.Count = 0 Then
            exchangeRate = CDec(dsExchangeRate.Tables(0).Rows(0).Item(2))
        End If

        If ObjDoc.canApprove(CDec(lblPaymentAmt.Text), CDbl(exchangeRate)) Then
            cmdAppIPP.Text = "Approve"
            lblAction.Text = "Click the Approve button to approve the E2P Document or Reject button to reject the E2P Document."
        Else
            cmdAppIPP.Text = "Endorse"
            lblAction.Text = "Click the Endorse button to endorse the E2P Document or Reject button to reject the E2P Document."
        End If

    End Sub

    Private Sub renderIPPApprFlow()
        Dim objDoc As New IPPMain

        'Zulham 13082018 - PAMB
        'Dim ds As DataSet = objDoc.getIPPApprFlow(Session("Index"), Common.Parse(HttpContext.Current.Session("CompanyID")))
        Dim ds As DataSet = objDoc.getE2PApprFlow(Session("Index"), Common.Parse(HttpContext.Current.Session("CompanyID")))

        ViewState("DocApprFlow") = ds
        intPageRecordCnt = ds.Tables(0).Rows.Count

        'ViewState("RecordCount") = intPageRecordCnt
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            dtgApprvFlow.DataSource = ds.Tables(0).DefaultView
            dtgApprvFlow.DataBind()
        Else
            dtgApprvFlow.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        objDoc = Nothing
    End Sub

    Private Function BindgridSubDoc(Optional ByVal pSorted As Boolean = False) As String
        Dim objIPPmain As New IPPMain
        Dim ds As New DataSet
        Dim dvViewIPP As DataView

        ds = objIPPmain.getSubDocDetail(Session("index"))
        dvViewIPP = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewIPP.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewIPP.Sort += " DESC"
        End If

        ViewState("intPageRecordCnt2") = ds.Tables(0).Rows.Count

        '//bind datagrid
        If ViewState("intPageRecordCnt2") > 0 Then
            dtgSubDocDetail.DataSource = dvViewIPP
            dtgSubDocDetail.DataBind()
        Else
            dtgSubDocDetail.DataSource = dvViewIPP
            dtgSubDocDetail.DataBind()
        End If

        If ds.Tables(0).Rows.Count > 10 Then
            ViewState("totalSubAmt") = 0
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                If Not Common.parseNull(ds.Tables(0).Rows(i).Item("ISD_DOC_GST_VALUE")).ToString.Trim = "" Then
                    ViewState("totalSubAmt") = ViewState("totalSubAmt") + ds.Tables(0).Rows(i).Item("ISD_DOC_AMT") + ds.Tables(0).Rows(i).Item("ISD_DOC_GST_VALUE")
                Else
                    ViewState("totalSubAmt") = ViewState("totalSubAmt") + ds.Tables(0).Rows(i).Item("ISD_DOC_AMT")
                End If
            Next
        End If

    End Function

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objIPPmain As New IPPMain
        Dim objGst As New GST
        Dim ds As New DataSet
        Dim dvViewIPP As DataView

        ds = objIPPmain.getApprIPPDetail(Session("DocNo").ToString.Replace("\", "\\"), Session("index"), Common.Parse(HttpContext.Current.Session("CompanyID")))  'Zulham 14072018 - PAMB
        ViewState("DocDetails") = ds
        dvViewIPP = ds.Tables(0).DefaultView

        'intPageRecordCnt = ds.Tables(0).Rows.Count

        'ViewState("RecordCount") = intPageRecordCnt
        If pSorted Then
            dvViewIPP.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewIPP.Sort += " DESC"
        End If

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        'Chee Hong - 19 May 2015 (Performance Issue)
        'Get dataview of Input/ Output Tax Code before databind
        Dim dsInput, dsOutput As New DataSet
        dsInput = objGst.GetTaxCode_forIPP(, "P", "IPP")
        dsOutput = objGst.GetTaxCode_forIPP(, "S", "IPP")
        dvViewInput = dsInput.Tables(0).DefaultView
        dvViewOutput = dsOutput.Tables(0).DefaultView
        ViewState("predefinedOutputTax") = objDb.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'REVERSE_CHARGE_OUTPUT' AND IP_COY_ID = '" & Session("CompanyId") & "'")

        '//bind datagrid
        If Not Page.IsPostBack Then
            If ViewState("intPageRecordCnt") > 0 Then
                dtgDocDetail.DataSource = dvViewIPP
                dtgDocDetail.DataBind()
            Else
                dtgDocDetail.DataSource = dvViewIPP
                dtgDocDetail.DataBind()
                Common.NetMsgbox(Me, MsgNoRecord)
            End If
        End If

        ' If Not Page.IsPostBack Then
        'ViewState("displaytotal") = ViewState("total")
        'End If
    End Function

    Private Sub dtgSubDocDetail_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgSubDocDetail.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt2")
        Grid_ItemCreated(sender, e)
    End Sub

    Public Sub dtgSubDocDetail_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        Dim s As DataGrid = sender
        Dim id As String = s.ID.ToString.Trim

        BindgridSubDoc(False)
        If ViewState("intPageRecordCnt2") > 0 Then
            AddRowtotalSubDoc()
        End If
    End Sub

    Private Sub dtgDocDetail_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDocDetail.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(sender, e)
    End Sub

    Public Sub dtgDocDetail_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        Dim s As DataGrid = sender
        Dim id As String = s.ID.ToString.Trim

        Bindgrid(False)
        If ViewState("intPageRecordCnt") > 0 Then
            AddRowtotal()
        End If
    End Sub

    Private Sub dtgApprvFlow_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgApprvFlow.ItemCreated
        Grid_ItemCreated_WORecCnt(sender, e)
    End Sub

    Private Sub dtgApprvFlow_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgApprvFlow.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim intTotalCell, intLoop As Integer

            ViewState("HighestAppr") = dv("FA_Seq")

            If Not IsDBNull(dv("FA_ACTION_DATE")) Then
                e.Item.Cells(2).Text = dv("FA_ACTION_DATE")
            End If

            e.Item.Cells(0).Text = e.Item.Cells(0).Text.Replace("IPP", "E2P")

        End If
    End Sub

    Private Sub dtgSubDocDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgSubDocDetail.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dblAmount, dblGSTAmount As Double

            dblAmount = Common.parseNull(dv("ISD_DOC_AMT"), 0)
            e.Item.Cells(EnumSubDocDet.subDocAmt).Text = Format(dblAmount, "#,##0.00")

            dblGSTAmount = Common.parseNull(dv("ISD_DOC_GST_VALUE"), 0)
            e.Item.Cells(EnumSubDocDet.subEmpty).Text = Format(dblGSTAmount, "#,##0.00")

            ViewState("totalSubAmt") = ViewState("totalSubAmt") + dblAmount + dblGSTAmount

            If IsDBNull(dv("ISD_DOC_DATE")) Then
                e.Item.Cells(EnumSubDocDet.subDocDate).Text = ""
            Else
                e.Item.Cells(EnumSubDocDet.subDocDate).Text = Format(dv("ISD_DOC_DATE"), "dd/MM/yyyy")
            End If

        End If
    End Sub

    Private Sub dtgDocDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDocDetail.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            'Zulham 13112018
            Dim dblAmount, dblAmount2 As Double

            dblAmount = Common.parseNull(dv("ID_RECEIVED_QTY"), 0) * Common.parseNull(dv("ID_UNIT_COST"), 0)
            e.Item.Cells(EnumIPPDet.ippUnitPrice).Text = Format(dv("ID_UNIT_COST"), "#,##0.00")

            'Zulham 13112018
            If Common.parseNull(dv("IM_CURRENCY_CODE"), "") <> "MYR" Then
                dsExchangeRate = objAdmin.getexrate(Common.parseNull(dv("IM_CURRENCY_CODE"), ""), "")
                dblAmount2 = (Common.parseNull(dv("ID_RECEIVED_QTY"), 0) * Common.parseNull(dv("ID_UNIT_COST"), 0)) * dsExchangeRate.Tables(0).Rows(0).Item(2)
                e.Item.Cells(EnumIPPDet.ippAmt).Text = Format(dblAmount2, "#,##0.00")
                e.Item.Cells(EnumIPPDet.ippForeignAmt).Text = Format(dblAmount, "#,##0.00")
                ViewState("total2") += dblAmount
                ViewState("total") += dblAmount2
            Else
                e.Item.Cells(EnumIPPDet.ippAmt).Text = Format(dblAmount, "#,##0.00")
                ViewState("total2") += dblAmount
            End If


            If UCase(Common.parseNull(dv("ID_PAY_FOR"))) = UCase(Session("CompanyId")) Then
                e.Item.Cells(EnumIPPDet.ippPayFor).Text = "Own Company"
            Else
                e.Item.Cells(EnumIPPDet.ippPayFor).Text = Common.parseNull(dv("ID_PAY_FOR"))
                e.Item.Cells(EnumIPPDet.ippPayFor).ToolTip = Common.parseNull(dv("IM_S_COY_NAME"))
            End If
            'Zulham 23012019
            e.Item.Cells(EnumIPPDet.ippGL).Text = Common.parseNull(dv("ID_B_GL_CODE")) & ":" & Common.parseNull(dv("CBG_B_GL_DESC"))
            e.Item.Cells(EnumIPPDet.ippCostAlloc).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("IPP", "CostAllocDetail.aspx", "Amount=" & dblAmount & "&invindex=" & Session("Index") & "&frm=approval&CostAllocCode=" & dv("ID_COST_ALLOC_CODE")) & "&InvLine=" & e.Item.Cells(EnumIPPDet.ippSNo).Text & "&VenIdx=" & VenIdx & "')"" ><font color=#0000ff>" & Common.parseNull(dv("ID_COST_ALLOC_CODE")) & "</font></A>"
            e.Item.Cells(EnumIPPDet.ippAssetGrp).ToolTip = Common.parseNull(dv("ID_ASSET_GROUP_DESC"))
            e.Item.Cells(EnumIPPDet.ippAssetSub).ToolTip = Common.parseNull(dv("ID_ASSET_SUB_GROUP_DESC"))
            e.Item.Cells(EnumIPPDet.ippCostCentre).ToolTip = Common.parseNull(dv("ID_COST_CENTER_DESC"))

            'Zulham 24102018
            Dim costCenterDesc = objDb.GetVal("SELECT cc_CC_desc FROM cost_centre WHERE cc_coy_id = '" & Session("CompanyID") & "' and cc_cc_code = '" & Common.parseNull(dv("ID_COST_CENTER")) & "'")
            e.Item.Cells(EnumIPPDet.ippCostCentre).Text = Common.parseNull(dv("ID_COST_CENTER")) & ":" & IIf(Common.parseNull(dv("ID_COST_CENTER_DESC")) = "", costCenterDesc, Common.parseNull(dv("ID_COST_CENTER_DESC")))

            'Zulham 10072018 - PAMB
            'Some condition for withholding tax
            If Common.parseNull(dv("id_withholding_opt")).ToString = "3" Then
                e.Item.Cells(EnumIPPDet.ippWHT).Text = Common.parseNull(dv("id_withholding_remarks"))
            End If
            'End
            'Zulham 11022019
            'Get the details for every analysis_codes
            Dim objIPPMain As New IPPMain
            If Not e.Item.Cells(EnumIPPDet.ippACode1).Text = "&nbsp;" And Not e.Item.Cells(EnumIPPDet.ippACode1).Text.Trim = "" Then
                e.Item.Cells(EnumIPPDet.ippACode1).Text = objIPPMain.getAnalysisCodeTypeAhead("L1", strUserInput:=e.Item.Cells(EnumIPPDet.ippACode1).Text.Trim).Tables(0).Rows(0).Item(0).ToString
            End If
            If Not e.Item.Cells(EnumIPPDet.ippACode2).Text = "&nbsp;" And Not e.Item.Cells(EnumIPPDet.ippACode2).Text.Trim = "" Then
                e.Item.Cells(EnumIPPDet.ippACode2).Text = objIPPMain.getAnalysisCodeTypeAhead("L2", strUserInput:=e.Item.Cells(EnumIPPDet.ippACode2).Text.Trim).Tables(0).Rows(0).Item(0).ToString
            End If
            If Not e.Item.Cells(EnumIPPDet.ippACode3).Text = "&nbsp;" And Not e.Item.Cells(EnumIPPDet.ippACode3).Text.Trim = "" Then
                e.Item.Cells(EnumIPPDet.ippACode3).Text = objIPPMain.getAnalysisCodeTypeAhead("L3", strUserInput:=e.Item.Cells(EnumIPPDet.ippACode3).Text.Trim).Tables(0).Rows(0).Item(0).ToString
            End If
            If Not e.Item.Cells(EnumIPPDet.ippACode4).Text = "&nbsp;" And Not e.Item.Cells(EnumIPPDet.ippACode4).Text.Trim = "" Then
                e.Item.Cells(EnumIPPDet.ippACode4).Text = objIPPMain.getAnalysisCodeTypeAhead("L4", strUserInput:=e.Item.Cells(EnumIPPDet.ippACode4).Text.Trim).Tables(0).Rows(0).Item(0).ToString
            End If
            If Not e.Item.Cells(EnumIPPDet.ippACode5).Text = "&nbsp;" And Not e.Item.Cells(EnumIPPDet.ippACode5).Text.Trim = "" Then
                e.Item.Cells(EnumIPPDet.ippACode5).Text = objIPPMain.getAnalysisCodeTypeAhead("L5", strUserInput:=e.Item.Cells(EnumIPPDet.ippACode5).Text.Trim).Tables(0).Rows(0).Item(0).ToString
            End If
            If Not e.Item.Cells(EnumIPPDet.ippACode8).Text = "&nbsp;" And Not e.Item.Cells(EnumIPPDet.ippACode8).Text.Trim = "" Then
                e.Item.Cells(EnumIPPDet.ippACode8).Text = objIPPMain.getAnalysisCodeTypeAhead("L8", strUserInput:=e.Item.Cells(EnumIPPDet.ippACode8).Text.Trim).Tables(0).Rows(0).Item(0).ToString
            End If
            If Not e.Item.Cells(EnumIPPDet.ippACode9).Text = "&nbsp;" And Not e.Item.Cells(EnumIPPDet.ippACode9).Text.Trim = "" Then
                e.Item.Cells(EnumIPPDet.ippACode9).Text = objIPPMain.getAnalysisCodeTypeAhead("L9", strUserInput:=e.Item.Cells(EnumIPPDet.ippACode9).Text.Trim).Tables(0).Rows(0).Item(0).ToString
            End If
            'End

            'Zulham Sept 17, 2014
            Dim ddlInputTax, ddlOutputTax As DropDownList
            Dim lblGSTAmount As TextBox
            Dim GST As New GST
            'Get the predefined ouputtax
            'Dim predefinedOutputTax = objDb.GetVal("select if(ic_gst_input_tax_code is null, '', ic_gst_input_tax_code) 'ic_gst_input_tax_code' from ipp_company where ic_coy_name ='" & Common.Parse(Me.lblVendor.Text) & "'")
            'Dim predefinedOutputTax = objDb.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'REVERSE_CHARGE_OUTPUT'")
            'Chee Hong - 19 May 2015 (Performance issue)
            Dim predefinedOutputTax As String = ViewState("predefinedOutputTax")

            Dim objGlobal As New AgoraLegacy.AppGlobals
            ddlInputTax = e.Item.FindControl("ddlInputTax")
            ddlOutputTax = e.Item.FindControl("ddlOutputTax")
            lblGSTAmount = e.Item.FindControl("lblGSTAmount")
            lblGSTAmount.Style("text-align") = "Right"
            'Chee Hong - 19 May 2015 (Performance issue)
            'GST.FillTaxCode_forIPP(ddlInputTax, "", "P")
            'GST.FillTaxCode_forIPP(ddlOutputTax, "", "S")
            'Fill dataview into tax code dropdownlist
            ddlInputTax.Items.Clear()
            Dim lstItem As New ListItem
            If Not dvViewInput Is Nothing Then
                Common.FillDdl(ddlInputTax, "GST", "TM_TAX_CODE", dvViewInput)
                lstItem.Value = ""
                'Zulham 24102018 - PAMB SST
                'lstItem.Text = "---Select---"
                ''ddlInputTax.Items.Insert(0, lstItem)
            Else
                lstItem.Value = "N/A"
                lstItem.Text = "N/A"
                ddlInputTax.Items.Clear()
                ddlInputTax.Items.Insert(0, lstItem)
                Common.SelDdl(lstItem.Value, ddlInputTax, True, True)
            End If

            ddlOutputTax.Items.Clear()
            'Zulham 10012019
            dvViewOutput = Nothing
            If Not dvViewOutput Is Nothing Then
                Common.FillDdl(ddlOutputTax, "GST", "TM_TAX_CODE", dvViewOutput)
                lstItem.Value = ""
                'Zulham 24102018 - PAMB SST
                'lstItem.Text = "---Select---"
                'ddlOutputTax.Items.Insert(0, lstItem)
            Else
                lstItem.Value = "N/A"
                lstItem.Text = "N/A"
                ddlOutputTax.Items.Clear()
                ddlOutputTax.Items.Insert(0, lstItem)
                Common.SelDdl(lstItem.Value, ddlOutputTax, True, True)
            End If

            If Common.parseNull(dv("id_gst_input_tax_code")) <> "" Then
                If Not dv("id_gst_input_tax_code").ToString = "0" And Not dv("id_gst_input_tax_code").ToString = "N/A" Then
                    If ViewState("InputTax") Is Nothing Then
                        ddlInputTax.SelectedValue = dv("id_gst_input_tax_code").ToString.Trim
                    Else
                        ddlInputTax.SelectedValue = ViewState("InputTax").ToString.Trim
                    End If
                    If dv("id_gst_input_tax_code").ToString.Trim.Contains("NR") Then
                        ddlInputTax.Enabled = False
                        lblGSTAmount.Enabled = False
                        'Zulham 09042015 IPP GST Stage 1
                    ElseIf dv("id_gst_input_tax_code").ToString.Trim.Contains("N/A") Then
                        ddlInputTax.Items.Add(New ListItem("N/A", "N/A"))
                        ddlInputTax.Enabled = False
                        ddlInputTax.SelectedValue = "N/A"
                    End If
                Else
                    'Zulham 09042015 IPP GST Stage 1
                    If ViewState("InputTax") Is Nothing Then
                        ddlInputTax.Items.Add(New ListItem("N/A", "N/A"))
                        ddlInputTax.Enabled = False
                        ddlInputTax.SelectedValue = "N/A"
                    Else
                        ddlInputTax.SelectedValue = ViewState("InputTax").ToString.Trim
                    End If
                End If
            Else
                'Zulham 09042015 IPP GST Stage 1
                ddlInputTax.Items.Add(New ListItem("N/A", "N/A"))
                ddlInputTax.Enabled = False
                ddlInputTax.SelectedValue = "N/A"
            End If

            'Zulham 10012019
            'If Common.parseNull(dv("id_gst_output_tax_code")) <> "" Then
            '    If Not dv("id_gst_output_tax_code").ToString = "0" And Not dv("id_gst_output_tax_code").ToString = "N/A" Then
            '        If ViewState("OutputTax") Is Nothing Then
            '            ddlOutputTax.SelectedValue = dv("id_gst_output_tax_code").ToString.Trim
            '        Else
            '            ddlOutputTax.SelectedValue = ViewState("OutputTax").ToString.Trim
            '        End If
            '    Else
            '        'Zulham 09042015 IPP GST Stage 1
            '        If ViewState("OutputTax") Is Nothing Then
            '            ddlOutputTax.Items.Add(New ListItem("N/A", "N/A"))
            '            ddlOutputTax.Enabled = False
            '            ddlOutputTax.SelectedValue = "N/A"
            '        Else
            '            ddlOutputTax.SelectedValue = ViewState("OutputTax").ToString.Trim
            '        End If
            '    End If
            'Else
            'Zulham 09042015 IPP GST Stage 1
            ddlOutputTax.Items.Add(New ListItem("N/A", "N/A"))
            ddlOutputTax.SelectedValue = "N/A"
            ddlOutputTax.Enabled = False
            'End If

            If dv("id_gst_value") IsNot DBNull.Value Then lblGSTAmount.Text = Format(dv("id_gst_value"), "#,##0.00") Else lblGSTAmount.Text = Format(0, "#,##0.00")
            'If Not lblGSTAmount.Text = "" Then ViewState("gstTotal") = ViewState("gstTotal") + CInt(lblGSTAmount.Text)
            'If Common.parseNull(dv("id_gst_input_tax_code")) = "N/A" Then

            If Common.parseNull(dv("id_gst_input_tax_code")) <> "" Then
                If Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Contains("(") Then
                    'Zulham 07112018
                    'If Not Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Contains("0") Then
                    If Not lblGSTAmount.Text = "" Then ViewState("gstTotal") = ViewState("gstTotal") + CDec(lblGSTAmount.Text)
                    'End If
                End If
            End If

            If Common.parseNull(dv("id_gst_input_tax_code")) <> "" Then
                If Common.parseNull(ddlOutputTax.SelectedItem.Text).ToString.Contains("(") Then
                    'Zulham 07112018
                    'If Not Common.parseNull(ddlOutputTax.SelectedItem.Text).ToString.Contains("0") Then
                    'Zulham 06052015 IPP GST Stage 1
                    If Not ddlInputTax.SelectedItem.Text.ToString.Trim.Contains("TX4") Then
                        If Not lblGSTAmount.Text = "" Then ViewState("gstTotal") = ViewState("gstTotal") - CDec(lblGSTAmount.Text)
                    End If
                    'End If
                End If
            End If

            'Zulham 07052015 IPP GST Stage 1
            ' TX4 Change Request
            'Zulham 04/04/2016 - IM5/IM6 Enhancement
            If ddlInputTax.SelectedItem.Text.Contains("IM2") Or ddlInputTax.SelectedItem.Text.Contains("IM1") _
            Or ddlInputTax.SelectedItem.Text.Contains("IM3") Or ddlInputTax.SelectedItem.Text.Contains("N/A") _
            Or ddlInputTax.SelectedItem.Text.Contains("TX4") Or ddlInputTax.SelectedItem.Text.Contains("IM5") Then
                ddlOutputTax.Enabled = False
            End If

            If IsDBNull(dv("id_gst_input_tax_code")) Then
                ddlInputTax.Enabled = False
                lblGSTAmount.ReadOnly = True
            End If
            If IsDBNull(dv("ID_GST_REIMB")) Then
                e.Item.Cells(EnumIPPDet.ippReimbursement).Text = "N/A"
            ElseIf dv("ID_GST_REIMB") = "R" Then
                e.Item.Cells(EnumIPPDet.ippReimbursement).Text = "Reimbursement"
            ElseIf dv("ID_GST_REIMB") = "D" And Not dv("ID_PAY_FOR").ToString.ToUpper = "HLISB" Then
                e.Item.Cells(EnumIPPDet.ippReimbursement).Text = "Disbursement"
                ddlInputTax.Enabled = False
            ElseIf dv("ID_GST_REIMB") = "D" And dv("ID_PAY_FOR").ToString.ToUpper = "HLISB" Then
                e.Item.Cells(EnumIPPDet.ippReimbursement).Text = "Disbursement"
            End If

            ddlInputTax.Attributes.Add("onchange", "return taxChange('" & ddlInputTax.ClientID & "','" & ddlOutputTax.ClientID & "','" & dblAmount & "','" & lblGSTAmount.ClientID & "','" & predefinedOutputTax & "');")
            ddlOutputTax.Attributes.Add("onchange", "return taxChange('" & ddlInputTax.ClientID & "','" & ddlOutputTax.ClientID & "','" & dblAmount & "','" & lblGSTAmount.ClientID & "','" & predefinedOutputTax & "');")
            ddlInputTax.Attributes.Add("onkeypress", "return isDecimalKey(event);")
            'End

            'Zulham 07112018
            ddlInputTax.Enabled = False
            lblGSTAmount.Enabled = False

        End If
    End Sub

    Sub AddRowtotalSubDoc() 'add total row 
        Dim intL As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        For intL = 0 To Me.dtgSubDocDetail.Columns.Count - 1
            addCell(row)
        Next

        If ViewState("totalSubAmt") Is Nothing Then
            ViewState("totalSubAmt") = ViewState("totalSubwGST")
        ElseIf ViewState("totalSubAmt") = 0 Then
            ViewState("totalSubAmt") = ViewState("totalSubwGST")
        End If

        row.Cells(EnumSubDocDet.subDocDate).Text = "Total :"
        row.Cells(EnumSubDocDet.subDocDate).HorizontalAlign = HorizontalAlign.Right
        row.Cells(EnumSubDocDet.subDocDate).Font.Bold = True
        row.Cells(EnumSubDocDet.subDocDate).BorderStyle = BorderStyle.None

        row.Cells(EnumSubDocDet.subDocAmt).Text = Format(ViewState("totalSubAmt"), "#,##0.00")
        row.Cells(EnumSubDocDet.subDocAmt).HorizontalAlign = HorizontalAlign.Center
        row.Cells(EnumSubDocDet.subDocAmt).Font.Bold = True
        row.Cells(EnumSubDocDet.subDocAmt).Font.Underline = True
        row.Cells(EnumSubDocDet.subDocAmt).Font.Overline = True
        row.Cells(EnumSubDocDet.subDocAmt).HorizontalAlign = HorizontalAlign.Right
        row.Cells(EnumSubDocDet.subDocAmt).BorderStyle = BorderStyle.None

        row.Cells(EnumSubDocDet.subDocNo).BorderStyle = BorderStyle.None
        row.Cells(EnumSubDocDet.subEmpty).BorderStyle = BorderStyle.None

        row.CssClass = "linespacing2"
        row.BorderStyle = BorderStyle.None
        row.BackColor = Drawing.Color.White
        Me.dtgSubDocDetail.Controls(0).Controls.Add(row)

        If ViewState("totalSubwGST") Is Nothing Then
            ViewState("totalSubwGST") = ViewState("totalSubAmt")
        End If

        ViewState("totalSubAmt") = 0

    End Sub

    Sub AddRowtotal() 'add total row 
        Dim gstAmount = 0.0
        Dim intL As Integer
        Dim dgItem As DataGridItem
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim IsResident = ViewState("isResident")
        'Zulham 13112018
        Dim currencyCode As String = ""

        For intL = 0 To Me.dtgDocDetail.Columns.Count - 1
            addCell(row)
        Next

        Dim predefinedOutputTax As String = objDb.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'REVERSE_CHARGE_OUTPUT' AND IP_COY_ID = '" & Session("CompanyId") & "'")
        'Zulham 07052015 IPP GST Stage 1
        Dim predefinedTX4OutputTax As String = objDb.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'GIFT_LUCKY_OUTPUT' AND IP_COY_ID = '" & Session("CompanyId") & "'")
        'Zulham 04/04/2016 - IM5/IM6 Enhancement
        Dim predefinedIM5OutputTax As String = objDb.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'REVERSE_CHARGE_OUTPUT_IM6' AND IP_COY_ID = '" & Session("CompanyId") & "'")

        'Zulham 13112018
        currencyCode = objDb.GetVal("select im_currency_code from invoice_mstr where im_invoice_index =" & Session("Index"))
        dsExchangeRate = objAdmin.getexrate(Common.parseNull(Trim(currencyCode), ""), "")

        'To calculate the total value
        If ViewState("total").ToString = "0" Then
            ViewState("gstTotal") = 0
            For Each dgItem In dtgDocDetail.Items
                Dim gst As New GST
                Dim ddlInputTax, ddlOutputTax As DropDownList
                Dim lblGSTAmount As TextBox
                ddlInputTax = dgItem.FindControl("ddlInputTax")
                ddlOutputTax = dgItem.FindControl("ddlOutputTax")
                lblGSTAmount = dgItem.FindControl("lblGSTAmount")

                'Dim predefinedOutputTax = objDb.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'REVERSE_CHARGE_OUTPUT'")
                ''Zulham 07052015 IPP GST Stage 1
                'Dim predefinedTX4OutputTax = objDb.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'GIFT_LUCKY_OUTPUT'")

                If ddlInputTax.SelectedValue.Trim = "IM2" Then
                    ddlOutputTax.SelectedValue = predefinedOutputTax
                    ddlOutputTax.Enabled = False
                    'Zulham 12082018 - PAMB
                    lblGSTAmount.Text = (CDec(dgItem.Cells(EnumIPPDet.ippAmt).Text) * ddlInputTax.SelectedItem.Text.Split("(")(1).Substring(0, 1)) / 100
                    'Zulham 07052015 IPP GST Stage 1
                    'TX4 change request
                ElseIf ddlInputTax.SelectedValue.Trim = "TX4" Then
                    ddlOutputTax.SelectedValue = predefinedTX4OutputTax
                    ddlOutputTax.Enabled = False
                    'Zulham 04/04/2016 - im5/im6 enhancement
                ElseIf ddlInputTax.SelectedValue.Trim = "IM5" Then
                    ddlOutputTax.SelectedValue = predefinedIM5OutputTax
                    ddlOutputTax.Enabled = False
                ElseIf ddlInputTax.SelectedValue.Trim = "IM3" Or ddlInputTax.SelectedValue.Trim = "IM1" Then
                    Dim lstItem As New ListItem
                    lstItem.Value = "N/A"
                    lstItem.Text = "N/A"
                    ddlOutputTax.Items.Insert(0, lstItem)
                    ddlOutputTax.SelectedValue = lstItem.Value
                    ddlOutputTax.Enabled = False
                Else
                    'Zulham 03112018
                    'If ddlOutputTax.Items(0).Text = "N/A" Then ddlOutputTax.Items.RemoveAt(0)
                    If ddlOutputTax.SelectedItem.Text <> "N/A" And ddlInputTax.SelectedItem.Text <> "N/A" Then
                        ddlOutputTax.Enabled = True
                    Else
                        ddlOutputTax.Enabled = False
                    End If
                    If ddlOutputTax.SelectedItem.Text = "N/A" And (ddlInputTax.SelectedValue.Trim = "IM3" Or ddlInputTax.SelectedValue.Trim = "IM1" Or IsResident = "Y") Then
                        ddlOutputTax.Enabled = False
                    End If
                End If
                If Common.parseNull(ddlInputTax.SelectedItem.Text) <> "" Then
                    If Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Contains("(") Then
                        'If Not Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Contains("0") Then
                        If Not lblGSTAmount.Text = "" Then ViewState("gstTotal") += CDec(lblGSTAmount.Text)
                        'End If
                    End If
                End If

                If Common.parseNull(ddlInputTax.SelectedItem.Text) <> "" Then
                    If Common.parseNull(ddlOutputTax.SelectedItem.Text).ToString.Contains("(") Then
                        'If Not Common.parseNull(ddlOutputTax.SelectedItem.Text).ToString.Contains("0") Then
                        'Zulham 07052015 IPP GST Stage 1
                        ' TX4 Change Request
                        If Not ddlInputTax.SelectedValue.Contains("TX4") Then
                                If Not lblGSTAmount.Text = "" Then ViewState("gstTotal") -= CDec(lblGSTAmount.Text)
                            End If

                        'End If
                    End If
                End If

                'ViewState("total") += dgItem.Cells(8).Text
                'ViewState("total2") += dgItem.Cells(8).Text
                'Zulham 12082018 - PAMB
                ViewState("total") += dgItem.Cells(EnumIPPDet.ippQty).Text * dgItem.Cells(EnumIPPDet.ippUnitPrice).Text
                ViewState("total2") += dgItem.Cells(EnumIPPDet.ippQty).Text * dgItem.Cells(EnumIPPDet.ippUnitPrice).Text
            Next
        End If
        'End

        If Not ViewState("gstTotal") Is Nothing Then
            If Not ViewState("gstTotal").ToString.Length = 0 Then
                'Zulham 13112018
                If Common.parseNull(Trim(currencyCode), "") <> "MYR" Then
                    gstAmount = ViewState("gstTotal") * dsExchangeRate.Tables(0).Rows(0).Item(2)
                Else
                    gstAmount = ViewState("gstTotal")
                End If
            End If
        End If

        'Zulham 09102018 - PAMB SST
        row.Cells(EnumIPPDet.ippDesc).Text = "Total :"
        row.Cells(EnumIPPDet.ippDesc).HorizontalAlign = HorizontalAlign.Center
        row.Cells(EnumIPPDet.ippDesc).Font.Bold = True
        row.Cells(EnumIPPDet.ippDesc).BorderStyle = BorderStyle.None

        row.Cells(EnumIPPDet.ippAmt).Text = Format(ViewState("total") + gstAmount, "#,##0.00")
        row.Cells(EnumIPPDet.ippAmt).HorizontalAlign = HorizontalAlign.Center
        row.Cells(EnumIPPDet.ippAmt).Font.Bold = True
        row.Cells(EnumIPPDet.ippAmt).Font.Underline = True
        row.Cells(EnumIPPDet.ippAmt).Font.Overline = True
        row.Cells(EnumIPPDet.ippAmt).HorizontalAlign = HorizontalAlign.Right
        row.Cells(EnumIPPDet.ippAmt).BorderStyle = BorderStyle.None

        If Common.parseNull(Trim(currencyCode), "") <> "MYR" Then
            row.Cells(EnumIPPDet.ippForeignAmt).Text = Format((ViewState("total") + gstAmount) / dsExchangeRate.Tables(0).Rows(0).Item(2), "#,##0.00")
            row.Cells(EnumIPPDet.ippForeignAmt).HorizontalAlign = HorizontalAlign.Center
            row.Cells(EnumIPPDet.ippForeignAmt).Font.Bold = True
            row.Cells(EnumIPPDet.ippForeignAmt).Font.Underline = True
            row.Cells(EnumIPPDet.ippForeignAmt).Font.Overline = True
            row.Cells(EnumIPPDet.ippForeignAmt).HorizontalAlign = HorizontalAlign.Right
            row.Cells(EnumIPPDet.ippForeignAmt).BorderStyle = BorderStyle.None
        End If

        row.Cells(EnumIPPDet.ippSNo).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippPayFor).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippInvoiceNo).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippDesc).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippQty).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippUOM).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippGL).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippAssetGrp).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippAssetSub).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippCostAlloc).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippGLRuleCat).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippBranch).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippCostCentre).BorderStyle = BorderStyle.None

        'Zulham Sept 18, 2014
        row.Cells(EnumIPPDet.ippGSTAmount).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippInputTax).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippOutputTax).BorderStyle = BorderStyle.None
        'End

        'Zulham 14072018 - PAMB
        row.Cells(EnumIPPDet.ippReimbursement).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippGift).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippCategory).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippACode1).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippACode2).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippACode3).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippACode4).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippACode5).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippACode8).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippACode9).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippWHT).BorderColor = Color.Transparent
        'End

        'Zulham 13112018
        row.Cells(EnumIPPDet.ippForeignAmt).BorderColor = Color.Transparent

        row.CssClass = "linespacing2"
        row.BorderStyle = BorderStyle.None
        row.BackColor = Drawing.Color.White
        Me.dtgDocDetail.Controls(0).Controls.Add(row)

        ViewState("total") = 0

    End Sub

    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub
    Private Sub GenerateTab()

        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Jan 15, 2014
        Dim strStatus = ""
        If Not Session("Index") Is Nothing Then
            strStatus = objDb.GetVal("SELECT im_invoice_status FROM invoice_mstr WHERE im_invoice_index = '" & Session("Index") & "'")
        End If
        'End
        If strStatus.ToString = "16" Then
            Session("w_IPP_tabs") = "<div class=""t_entity""><ul>" & _
               "<li><div class=""space""></div></li>" & _
               "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPP", "IPPApprovalList.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
               "<li><div class=""space""></div></li>" & _
               "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "IPPApprovalList_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
               "<li><div class=""space""></div></li>" & _
               "</ul><div></div></div>"
        Else
            Session("w_IPP_tabs") = "<div class=""t_entity""><ul>" & _
               "<li><div class=""space""></div></li>" & _
               "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "IPPApprovalList.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
               "<li><div class=""space""></div></li>" & _
               "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPP", "IPPApprovalList_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
               "<li><div class=""space""></div></li>" & _
               "</ul><div></div></div>"
        End If
      
        If strFrm = "ApprList" Then
            lnkBack.NavigateUrl = dDispatcher.direct("IPP", "IPPApprovalList.aspx", "pageid=" & strPageId)
            Session("w_IPP_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPP", "IPPApprovalList.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "IPPApprovalList_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
        Else
            lnkBack.NavigateUrl = dDispatcher.direct("IPP", "IPPApprovalList_All.aspx", "pageid=" & strPageId)
        End If

    End Sub
    Private Function FormatAORemark(ByVal strAction As String) As String
        Dim strRemark, strTempRemark, strUserName As String
        If Session("relief") Then
            Dim objUsers As New Users
            Dim objUser As New User
            objUsers.GetUserDetail(Session("AO"), Session("CompanyId"), objUser)
            strUserName = objUser.Name
            objUsers = Nothing
            objUser = Nothing
            strTempRemark = "(On Behalf of " & strUserName & ") "
        Else
            strTempRemark = ""
        End If

        'Zulham 12122018 -PAMB
        'Check for AO seq
        Dim seq = 0
        seq = objDb.GetVal("Select DISTINCT aga_seq 
                            From approval_grp_ao, finance_approval
                            Where FA_INVOICE_INDEX = '" & Session("Index") & "' 
                            And FA_APPROVAL_GRP_INDEX = aga_grp_index
                            And (aga_ao = '" & Session("UserID") & "' OR aga_a_ao = '" & Session("UserID") & "') ")

        Select Case strAction
            Case "approve"
                strRemark = "Approved " & strTempRemark & ": "
            Case "reject"
                strRemark = "Rejected" & strTempRemark & ": "
            Case "verify"
                'Zulham 13082018 -PAMB
                If seq = 1 Then
                    strRemark = "Verified" & strTempRemark & ": "
                Else
                    strRemark = "Dept Approval" & strTempRemark & ": "
                End If
        End Select
        Return strRemark
    End Function

    Private Sub cmdAppIPP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAppIPP.Click
        Dim strRemark, strUrl As String
        Dim objipp As New IPP
        Dim objippmain As New IPPMain
        Dim strMsg As String
        Dim itemTotal As Double = 0.0
        Dim gstTotal As Double = 0.0
        Dim dgItem As DataGridItem
        Dim aryDocDetails As New ArrayList
        'for checking the payment method TT
        If lblPaymentMethod.Text.Contains("TT") Then lblPaymentMethod.Text = "TT"
        strRemark = FormatAORemark("verify")
        'Zulham 04112018
        strRemark = strRemark & IIf(txtApprRejRemark.Text <> "", txtApprRejRemark.Text, "Submitted")

        'Zulham 21082015 - reset ViewState("GSTTotalIM1IM3") value to 0
        ViewState("GSTTotalIM1IM3") = 0

        'Zulham 21092018 - e2p UAT
        'If txtApprRejRemark.Text = "" Then
        '    Common.NetMsgbox(Me, "Please enter Remarks.", MsgBoxStyle.Information)
        'Else
        Dim objDoc As New IPPMain
        Dim intApprGrpIndex As String

        'Zulham 05/01/2016 - IPP STAGE 4 Phase 2
        'check for invoices status that are tied to CN/DN
        If objippmain.checkStatus(lblDocNo.Text.Trim, VenIdx, Session("Status")) = False Then
            strMsg = "Credit Note/Debit Note cannot precede the invoices it's tied to."
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            BindgridSubDoc()
            AddRowtotalSubDoc()
            Exit Sub
        End If

        'ValidaTION
        For Each dgItem In dtgDocDetail.Items
            Dim ddlInputTax, ddlOutputTax As DropDownList
            Dim lblGSTAmount As TextBox
            Dim GST As New GST
            Dim objGlobal As New AgoraLegacy.AppGlobals
            ddlInputTax = dgItem.FindControl("ddlInputTax")
            ddlOutputTax = dgItem.FindControl("ddlOutputTax")
            lblGSTAmount = dgItem.FindControl("lblGSTAmount")
            'Zulham 01042015 Case 8603
            If lblPaymentMethod.Text <> "TT" Or Not lblPaymentMethod.Text.ToUpper.Contains("NOSTRO") Then
                'Zulham 12082018 - PAMB
                If dgItem.Cells(EnumIPPDet.ippPayFor).Text <> "Own Company" Then
                    If _exceedCutOffDt <> "Yes" Then
                        'Zulham 12082018 - PAMB
                        If dgItem.Cells(EnumIPPDet.ippCurrency).Text = "" Or dgItem.Cells(EnumIPPDet.ippCurrency).Text = "&nbsp;" Then
                            strMsg = "Currency for Line item " & objGlobal.GetErrorMessage("00001")
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                            BindgridSubDoc()
                            AddRowtotalSubDoc()
                            Exit Sub
                        End If
                        'Zulham 12082018 - PAMB
                        If CType(dgItem.FindControl("txtLineExchangeRate"), TextBox).Text = "" And (dgItem.Cells(EnumIPPDet.ippCurrency).Text <> "" And dgItem.Cells(EnumIPPDet.ippCurrency).Text <> "&nbsp;" And dgItem.Cells(EnumIPPDet.ippCurrency).Text <> "MYR") Then
                            strMsg = objGlobal.GetErrorMessage("00038") & " for Line item."
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                            BindgridSubDoc()
                            AddRowtotalSubDoc()
                            Exit Sub
                        End If
                        'Zulham 12082018 - PAMB
                        If dgItem.Cells(5).Text <> "MYR" Then
                            aryDocDetails.Add(New String() {dgItem.Cells(EnumIPPDet.ippSNo).Text, dgItem.Cells(EnumIPPDet.ippPayFor).Text, dgItem.Cells(EnumIPPDet.ippCurrency).Text, CType(dgItem.FindControl("txtLineExchangeRate"), TextBox).Text})
                        Else
                            aryDocDetails.Add(New String() {dgItem.Cells(EnumIPPDet.ippSNo).Text, dgItem.Cells(EnumIPPDet.ippPayFor).Text, dgItem.Cells(EnumIPPDet.ippCurrency).Text, 1})
                        End If
                    ElseIf _exceedCutOffDt = "Yes" Then
                        aryDocDetails.Add(New String() {dgItem.Cells(EnumIPPDet.ippSNo).Text, dgItem.Cells(EnumIPPDet.ippPayFor).Text, "", 1, lblGSTAmount.Text, IIf(ddlInputTax.SelectedValue.ToString = "0", "N/A", ddlInputTax.SelectedValue.ToString), IIf(ddlOutputTax.SelectedValue.ToString = "0", "N/A", ddlOutputTax.SelectedValue)})
                    End If
                Else
                    If _exceedCutOffDt <> "Yes" Then
                        'Zulham 12082018 - PAMB
                        'Zulham 17082015
                        'Additional items for non-gst invoice
                        aryDocDetails.Add(New String() {dgItem.Cells(EnumIPPDet.ippSNo).Text, dgItem.Cells(EnumIPPDet.ippPayFor).Text, "", "", 0, "N/A", "N/A"})
                    ElseIf _exceedCutOffDt = "Yes" Then
                        aryDocDetails.Add(New String() {dgItem.Cells(EnumIPPDet.ippSNo).Text, dgItem.Cells(EnumIPPDet.ippPayFor).Text, "", "", lblGSTAmount.Text, IIf(ddlInputTax.SelectedValue.ToString = "0", "N/A", ddlInputTax.SelectedValue.ToString), IIf(ddlOutputTax.SelectedValue.ToString = "0", "N/A", ddlOutputTax.SelectedValue)})
                    End If
                End If
            Else
                If _exceedCutOffDt <> "Yes" Then
                    'Zulham 12082018 - PAMB
                    'Zulham 17082015
                    'Additional items for non-gst invoice
                    aryDocDetails.Add(New String() {dgItem.Cells(EnumIPPDet.ippSNo).Text, dgItem.Cells(EnumIPPDet.ippPayFor).Text, dgItem.Cells(EnumIPPDet.ippCurrency).Text, "", 0, "N/A", "N/A"})
                ElseIf _exceedCutOffDt = "Yes" Then
                    aryDocDetails.Add(New String() {dgItem.Cells(EnumIPPDet.ippSNo).Text, dgItem.Cells(EnumIPPDet.ippPayFor).Text, dgItem.Cells(EnumIPPDet.ippQty).Text, "", lblGSTAmount.Text, IIf(ddlInputTax.SelectedValue.ToString = "0", "N/A", ddlInputTax.SelectedValue.ToString), IIf(ddlOutputTax.SelectedValue.ToString = "0", "N/A", ddlOutputTax.SelectedValue)})
                End If
            End If

            'Zulham 12082018 - PAMB
            itemTotal += dgItem.Cells(EnumIPPDet.ippQty).Text * dgItem.Cells(EnumIPPDet.ippUnitPrice).Text
            'Zulham Sept 17, 2014
            'Zulham 10082018 - PAMB
            '•	Both Input Tax Code and Output Tax Code must be same range of tax percentage selected before.
            Dim IsResident = ViewState("isResident")
            'IPP Gst Stage 2A - CH - 11 Feb 2015
            'If strDefIPPCompID = "" Then
            '    IsResident = objDb.GetVal("SELECT ifnull(ic_resident_type,'') FROM IPP_company WHERE ic_coy_id =  '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
            'Else
            '    IsResident = objDb.GetVal("SELECT ifnull(ic_resident_type,'') FROM IPP_company WHERE ic_coy_id =  '" & Common.Parse(strDefIPPCompID) & "'")
            'End If

            'Zulham 31102018
            'If Not ddlInputTax.SelectedValue = "" And Not ddlOutputTax.SelectedValue = "" Then
            '    If ddlInputTax.SelectedItem.Text.Contains("(") And ddlOutputTax.SelectedItem.Text.Contains("(") Then
            '        If Not (ddlInputTax.SelectedItem.Text.Contains("IM1") Or ddlInputTax.SelectedItem.Text.Contains("IM3")) Then
            '            If CDec(ddlInputTax.SelectedItem.Text.Split("(")(1).Substring(0, 1)) <>
            '                    CDec(ddlOutputTax.SelectedItem.Text.Split("(")(1).Substring(0, 1)) Then
            '                strMsg = "Input Tax Code and Output Tax Code must be the same."
            '                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            '                BindgridSubDoc()
            '                AddRowtotalSubDoc()
            '                Exit Sub
            '            ElseIf lblGSTAmount.Text.Length = 0 Then
            '                strMsg = "Invalid GST Amount."
            '                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            '                BindgridSubDoc()
            '                AddRowtotalSubDoc()
            '                Exit Sub
            '            ElseIf CDec(ddlInputTax.SelectedItem.Text.Split("(")(1).Substring(0, 1)) > CDec(0) And lblGSTAmount.Text = 0.0 Then
            '                strMsg = "Invalid GST Amount."
            '                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            '                BindgridSubDoc()
            '                AddRowtotalSubDoc()
            '                Exit Sub
            '            ElseIf CDec(ddlInputTax.SelectedItem.Text.Split("(")(1).Substring(0, 1)) = CDec(0) And CDec(lblGSTAmount.Text) > CDec(0.0) Then
            '                strMsg = "Invalid GST Amount."
            '                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            '                BindgridSubDoc()
            '                AddRowtotalSubDoc()
            '                Exit Sub
            '            End If
            '        End If
            '    ElseIf (ddlInputTax.SelectedItem.Text.Contains("IM1") Or ddlInputTax.SelectedItem.Text.Contains("IM3")) Then
            '        If lblGSTAmount.Text.Length = 0 Then
            '            strMsg = "Invalid GST Amount."
            '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            '            BindgridSubDoc()
            '            AddRowtotalSubDoc()
            '            Exit Sub
            '        ElseIf CDec(lblGSTAmount.Text) = CDec(0) Then
            '            strMsg = "Invalid GST Amount."
            '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            '            BindgridSubDoc()
            '            AddRowtotalSubDoc()
            '            Exit Sub
            '        End If
            '    ElseIf ddlInputTax.SelectedItem.Text.Contains("IM2") Then
            '        If lblGSTAmount.Text.Length = 0 Then
            '            strMsg = "Invalid GST Amount."
            '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            '            BindgridSubDoc()
            '            AddRowtotalSubDoc()
            '            Exit Sub
            '        ElseIf CDec(lblGSTAmount.Text) = CDec(0) Then
            '            strMsg = "Invalid GST Amount."
            '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            '            BindgridSubDoc()
            '            AddRowtotalSubDoc()
            '            Exit Sub
            '        ElseIf ddlOutputTax.SelectedValue = 0 Then
            '            strMsg = "Invalid GST Input Tax selection."
            '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            '            BindgridSubDoc()
            '            AddRowtotalSubDoc()
            '            Exit Sub
            '        End If
            '        'Zulham 12082018 - PAMB
            '        'ElseIf ddlInputTax.SelectedItem.Text.Contains("%") And ddlOutputTax.SelectedItem.Text.Contains("N/A") And Not IsResident = "Y" Then
            '        '    strMsg = "Invalid GST Output Tax selection."
            '        '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            '        '    BindgridSubDoc()
            '        '    AddRowtotalSubDoc()
            '        '    Exit Sub
            '        'ElseIf ddlInputTax.SelectedItem.Text.Contains("%") And Not ddlOutputTax.SelectedItem.Text.Contains("%") And Not IsResident = "Y" Then
            '        '    strMsg = "Invalid GST Tax selection."
            '        '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            '        '    BindgridSubDoc()
            '        '    AddRowtotalSubDoc()
            '        '    Exit Sub
            '        'ElseIf Not ddlInputTax.SelectedItem.Text.Contains("%") And ddlOutputTax.SelectedItem.Text.Contains("%") And Not IsResident = "Y" Then
            '        '    strMsg = "Invalid GST Tax selection."
            '        '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            '        '    BindgridSubDoc()
            '        '    AddRowtotalSubDoc()
            '        '    Exit Sub
            '        'End
            '    Else
            '        If ddlInputTax.SelectedItem.Text.ToUpper.Contains("SELECT") Then
            '            strMsg = "Invalid GST Input Tax selection."
            '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            '            BindgridSubDoc()
            '            AddRowtotalSubDoc()
            '            Exit Sub
            '        End If
            '        'Output TaxCode is not applicable to Resident Companies. Resident output tax will always be N/A
            '        If Not IsResident = "Y" Then
            '            If lblGSTAmount.Text.Length > 0 Then
            '                If CDec(lblGSTAmount.Text) > CDec(0) Then
            '                    strMsg = "Invalid GST Amount."
            '                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            '                    BindgridSubDoc()
            '                    AddRowtotalSubDoc()
            '                    Exit Sub
            '                End If
            '            End If
            '        End If
            '    End If
            'ElseIf ddlOutputTax.SelectedValue = "" Then
            '    strMsg = "Invalid GST Output Tax selection."
            '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            '    BindgridSubDoc()
            '    AddRowtotalSubDoc()
            '    Exit Sub
            'End If
            ''End

            If Common.parseNull(ddlInputTax.SelectedItem.Text) <> "" Then
                If Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Contains("(") Then
                    'Zulham 07112018
                    'If Not Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Contains("0") Then
                    If Not lblGSTAmount.Text = "" Then gstTotal = ViewState("gstTotal") + CDec(lblGSTAmount.Text)

                    'Zulham Case 8317 12022015
                    If Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Trim.Contains("IM1") Or
                            Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Trim.Contains("IM3") Then
                        If Not lblGSTAmount.Text = "" Then
                            ViewState("GSTTotalIM1IM3") += CDec(lblGSTAmount.Text)
                        End If
                    End If

                    'End If
                End If
            End If

            If Common.parseNull(ddlInputTax.SelectedItem.Text) <> "" Then
                If Common.parseNull(ddlOutputTax.SelectedItem.Text).ToString.Contains("(") Then
                    'Zulham 07112018
                    'If Not Common.parseNull(ddlOutputTax.SelectedItem.Text).ToString.Contains("0") Then
                    If Not lblGSTAmount.Text = "" Then gstTotal = ViewState("gstTotal") - CDec(lblGSTAmount.Text)
                    'End If
                End If
            End If
        Next

        'Zulham 12022015 Case 8317
        If ViewState("GSTTotalIM1IM3") Is Nothing Then ViewState("GSTTotalIM1IM3") = 0.0

        If dtgSubDocDetail.Items.Count > 0 Then
            'Zulham 21042015 IPP GST Stage 1 
            If Not FormatNumber(CDec(ViewState("totalSubwGST")), 2) = FormatNumber(CDec(ViewState("gstTotal") + itemTotal), 2) Then
                strMsg = "Invoice Amounts must tally."
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                BindgridSubDoc()
                AddRowtotalSubDoc()
                Exit Sub
            End If
        Else
            Dim val = FormatNumber(CDec(ViewState("gstTotal") + itemTotal - ViewState("GSTTotalIM1IM3")), 2)
            'Zulham 21042015 IPP GST Stage 1 
            If Not FormatNumber(CDec(lblPaymentAmt.Text), 2) = FormatNumber(CDec(ViewState("gstTotal") + itemTotal - ViewState("GSTTotalIM1IM3")), 2) Then
                strMsg = "Invoice Amounts must tally."
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                BindgridSubDoc()
                AddRowtotalSubDoc()
                Exit Sub
            End If
        End If

        'Throw New Exception
        'END

        intApprGrpIndex = objDb.GetVal("SELECT DISTINCT FA_APPROVAL_GRP_INDEX FROM finance_approval WHERE FA_INVOICE_INDEX = '" & Session("Index") & "' ")

        If objDoc.checkDept() = False Then
            objipp.Message(Me, "00046", MsgBoxStyle.Information)
            Exit Sub
        End If
        If objDoc.CheckBankCode(Session("Index")) Then
            strMsg = objGlobal.GetErrorMessage("00323")
            Common.NetMsgbox(Me, strMsg, lnkBack.NavigateUrl, MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        'Zulham 01042015 Case 8603
        If (lblPaymentMethod.Text = "TT" Or lblPaymentMethod.Text.ToUpper.Contains("NOSTRO")) And ViewState("role") = 3 Then
            If txtExchangeRate.Text = "" Then
                objipp.Message(Me, "00038", MsgBoxStyle.Information)
                Exit Sub
            End If
            If objippmain.IsDecimal(txtExchangeRate.Text) = False Then
                objipp.Message(Me, "00039", MsgBoxStyle.Information)
                Exit Sub
            End If
            'Zulham 11012019
            If objDoc.ApproveIPPDoc(Session("Index"), strRemark, Session("Relief"), ViewState("role"), lblPaymentMethod.Text, txtExchangeRate.Text, lblDocType.Text, IPPTimeFr1, intApprGrpIndex, , aryDocDetails:=Nothing) = True Then
                strMsg = objGlobal.GetErrorMessage("00043") & " " & Session("DocNo") & " " & objGlobal.GetErrorMessage("00044")
                Common.NetMsgbox(Me, strMsg, lnkBack.NavigateUrl, MsgBoxStyle.Exclamation)
            Else
                strMsg = objGlobal.GetErrorMessage("00007")
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            End If
        Else
            If objDoc.CheckBankCode(Session("Index")) Then
                strMsg = objGlobal.GetErrorMessage("00323")
                Common.NetMsgbox(Me, strMsg, lnkBack.NavigateUrl, MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            'Zulham 20102018 - PAMB
            Dim objAdmin As New Admin
            Dim exchangeRate As Decimal = 1
            Dim dsExchangeRate As New DataSet : dsExchangeRate = objAdmin.getexrate(lblCurrency.Text, "")
            If Not dsExchangeRate.Tables(0).Rows.Count = 0 Then
                exchangeRate = CDec(dsExchangeRate.Tables(0).Rows(0).Item(2))
            End If

            'Zulham 04072018 - Check for approval limit
            'approval limit <invoice total= invoice goes to the next person 
            Dim blnCanApprove = objDoc.canApprove(CDec(lblPaymentAmt.Text), CDbl(exchangeRate))
            'Zulham 11012019
            If objDoc.ApproveIPPDoc(Session("Index"), strRemark, Session("Relief"), ViewState("role"), lblPaymentMethod.Text, , lblDocType.Text, IPPTimeFr1, intApprGrpIndex, , aryDocDetails:=Nothing, blnCanApprove:=blnCanApprove) = True Then

                'Zulham 03122018
                If Not blnCanApprove Then
                    objDoc.sendMailToIPPOfficer(Session("Index"), "6", "verify")
                End If

                If objDoc.isHighestLevel(Session("Index")) Then
                    strMsg = objGlobal.GetErrorMessage("00043") & " " & Session("DocNo") & " " & objGlobal.GetErrorMessage("00044").Replace("verified", "endorsed/approved")
                Else
                    'Zulham 04112018
                    If blnCanApprove Then
                        strMsg = objGlobal.GetErrorMessage("00043") & " " & Session("DocNo") & " " & objGlobal.GetErrorMessage("00051").Replace("verified", "endorsed/approved")
                    Else
                        strMsg = objGlobal.GetErrorMessage("00043") & " " & Session("DocNo") & " " & objGlobal.GetErrorMessage("00051").Replace("verified", "endorsed")
                    End If
                End If
                Common.NetMsgbox(Me, strMsg, lnkBack.NavigateUrl, MsgBoxStyle.Exclamation)
            Else
                strMsg = objGlobal.GetErrorMessage("00007")
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            End If
        End If
        cmdRejectIPP.Visible = False
        cmdAppIPP.Visible = False
        'End If
    End Sub

    Private Sub cmdRejectIPP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRejectIPP.Click
        Dim objDoc As New IPPMain
        Dim objipp As New IPP
        Dim strRemark As String
        Dim strMsg As String
        If objDoc.checkDept() = False Then
            objipp.Message(Me, "00046", MsgBoxStyle.Information)
            Exit Sub
        End If
        strRemark = FormatAORemark("reject")
        strRemark = strRemark & txtApprRejRemark.Text

        'Zulham 16/02/2016 - IPP Stage 4 Phase 2
        'Invoices tied to any CN/DN cant be rejected
        Dim boolCantReject As Boolean = False
        Dim dsRefNo As New DataSet
        dsRefNo = objDoc.getCNDNRefNo(Session("DocNo").ToString.Replace("\", "\\"), ViewState("VenIdx")) 'Zulham 14072018 - PAMB
        If Not dsRefNo Is Nothing Then
            If Not dsRefNo.Tables(0).Rows.Count = 0 Then
                For i As Integer = 0 To dsRefNo.Tables(0).Rows.Count - 1
                    If lblDocNo.Text = dsRefNo.Tables(0).Rows(i).Item(0).ToString.Trim Then
                        boolCantReject = True
                        Exit For
                    End If
                Next
            End If
        End If

        If boolCantReject = True Then
            Common.NetMsgbox(Me, "This invoice is tied to a Credit Note/Debit Note.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If txtApprRejRemark.Text = "" Then
            Common.NetMsgbox(Me, "Please enter Remarks.", MsgBoxStyle.Information)
        Else
            If objDoc.RejectIPPDoc(Session("Index"), strRemark, Session("Relief"), ViewState("role")) = True Then
                strMsg = objGlobal.GetErrorMessage("00043") & " " & Session("DocNo") & " " & objGlobal.GetErrorMessage("00045")
                Common.NetMsgbox(Me, strMsg, lnkBack.NavigateUrl, MsgBoxStyle.Information)
            Else
                objipp.Message(Me, "00007", MsgBoxStyle.Information)
            End If
            cmdRejectIPP.Visible = False
            cmdAppIPP.Visible = False
        End If
    End Sub
    Private Function getUserRole(ByVal blnAO As Boolean) As Integer
        If blnAO = True Then 'And blnFo = False And blnFM = False Then
            getUserRole = 5
            'ElseIf blnFo = False And blnFM = True Then
            '    getUserRole = 3
            'ElseIf blnFo = True And blnFM = False Then
            '    getUserRole = 2
        Else
            getUserRole = 1
        End If
    End Function

    Public Function addLeadingZero(ByVal value As String, ByVal place As String) As String
        Dim leadingZero As String = ""
        Dim i As Integer
        Dim totalplace As Integer
        Dim valuelength As String
        valuelength = value.Length
        totalplace = CInt(place) - CInt(valuelength)
        For i = 0 To totalplace
            leadingZero &= "0"
        Next
        Return leadingZero & value.ToString
    End Function
    Private Sub cmdViewAudit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdViewAudit.Click
        Dim strscript As New System.Text.StringBuilder
        Dim objIPPDetails As New IPPDetails
        Dim strFileName As String

        strscript.Append("<script language=""javascript"">")
        'Zulham 12072018 - PAMB
        strFileName = dDispatcher.direct("IPP", "ViewIPPAuditTrails.aspx", "pageid=" & strPageId & "&docno=" & Server.UrlEncode(Session("DocNo")) & "&docstatus=" & Server.UrlEncode(lblStatus.Text) & "&docidx=" & Session("Index"))
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'520px');")
        'strscript.Append("document.getElementById('btnhidden3').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script1", strscript.ToString())
    End Sub

#Region "PAMB"
    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
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
                objFile.FileUpload(File1Int, EnumUploadType.DocAttachmentTemp, "IPP", EnumUploadFrom.FrontOff, Session("Index"), , , , , , "I")
            ElseIf File1Int.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            objPR = Nothing
            objFile = Nothing
        End If

        displayAttachFile()
        bindgrid()

    End Sub
    Private Sub displayAttachFile()

        Dim objIPP As New IPP
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        dsAttach = objIPP.getIPPTempAttach(Session("Index"), "I")

        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                Dim objFile As New FileManagement
                'Zulham 19072018 - PAMB
                strURL = objFile.getAttachPathE2P(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "IPP", EnumUploadFrom.FrontOff)
                objFile = Nothing
                Dim lblBr As New Label
                Dim lblFile As New Label
                Dim lnk As New ImageButton
                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"
                lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete2.gif")
                lnk.ID = drvAttach(i)("CDA_ATTACH_INDEX")
                lnk.CausesValidation = False

                lnk.Attributes.Add("onclick", "removeFile('" & lnk.ID & "')")

                pnlAttach.Controls.Add(lblFile)

                If drvAttach(i)("CDA_STATUS") = "N" Then
                    pnlAttach.Controls.Add(lnk)
                End If

                pnlAttach.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFile)
        End If
        objIPP = Nothing

    End Sub
    Private Sub removeAttachment()
        Dim objPO As New PurchaseOrder_Buyer

        objPO.delete_Attachment_Temp(hidFileIndex.Value)

        displayAttachFile()
        hidFileIndex.Value = ""
        objPO = Nothing
    End Sub
#End Region

End Class