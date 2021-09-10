Imports AgoraLegacy
Imports eProcure.Component
Imports System.drawing
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Partial Public Class IPPDocument
    Inherits AgoraLegacy.AppBaseClass
    Dim dblInvoiceAmount, prevAppType As String
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objdb As New EAD.DBCom
    Dim VenIdx As Integer = 0
    Dim _exceedCutOffDt As String = ""
    Dim strIsGst As String = ""
    Dim dvViewInput, dvViewOutput As DataView

    Public Enum EnumSubDocDet
        subDocNo = 0
        subDocDate = 1
        subDocAmt = 2
        subEmpty = 3
    End Enum

    Public Enum EnumIPPDet
        ippSNo = 0
        ippPayFor = 1
        ippReimbursement = 2 'Zulham Sept 22, 2014
        ippInvoiceNo = 3
        ippDesc = 4
        ippUOM = 5
        ippCurrency = 6
        ippQty = 7
        ippUnitPrice = 8
        ippAmt = 9
        ippAmt2 = 10
        'Zulham Sept 18, 2014
        ippGSTAmount = 12
        'Zulham 12/01/2016 - IPP Stage 4 Phase 2
        ippForeignGSTAmount = 11
        ippInputTax = 13
        ippOutputTax = 14
        'End
        ippGL = 15
        ippCostCentre = 16 'Zulham 11072018 - PAMB
        'Zulham 11072018 - PAMB
        ippGift = 17
        ippCategory = 18
        'End
        'Zulham 11072018 - PAMB
        ippACode1 = 19
        ippACode2 = 20
        ippACode3 = 21
        ippACode4 = 22
        ippACode5 = 23
        ippACode8 = 24
        ippACode9 = 25
        'End
        ippAssetGrp = 26
        ippAssetSub = 27
        ippGLRuleCat = 28
        ippBranch = 29
        'Zulham 11072018 - PAMB
        ippWHT = 30
        'End
        ippCostAlloc = 31
        ippExchangeRate = 32
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objIPP As New IPP
        Dim objIPPDetails As New IPPDetails
        Dim strFrm As String


        strFrm = Me.Request.QueryString("Frm")
        Session("DocNo") = Request.QueryString("DocumentNo") 'Zulham 16072018 - PAMB
        Session("InvIdx") = Request.QueryString("index")

        VenIdx = objdb.GetVal("SELECT im_s_coy_id FROM invoice_mstr WHERE im_invoice_index = '" & Session("InvIdx") & "'")

        'blnPaging = False
        blnSorting = False

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgApprvFlow, "N")
        SetGridProperty(dtgDocDetail, "N")
        SetGridProperty(dtgSubDocDetail, "Y")
        'renderIPPApprFlow()

        ViewState("total") = 0
        ViewState("total2") = 0
        ViewState("gstTotal") = 0

        Bindgrid()

        If ViewState("intPageRecordCnt") > 0 Then           
            AddRowtotal()
        End If
        ViewState("line") = 0

        If Request.QueryString("Frm") = "IPPEnq" Or Request.QueryString("Frm") = "WHTReceipt" Then
            objIPPDetails = objIPP.GetIPPEnqDetails(Session("DocNo").ToString.Replace("\", "\\"), Common.Parse(HttpContext.Current.Session("CompanyID")), Session("InvIdx")) 'Zulham 19072018 - PAMB
        ElseIf Request.QueryString("Frm") = "PSDSent" Then
            objIPPDetails = objIPP.GetIPPDetails(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")), Session("InvIdx"), Request.QueryString("Frm"), Request.QueryString("CreatedBy"))
        Else
            objIPPDetails = objIPP.GetIPPDetails(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")), Session("InvIdx"), Request.QueryString("Frm"))
        End If

        'Michelle (12/5/2014) - Prevent show approval flow (conversion error) when status is 'Void'
        If objIPPDetails.Status <> "15" Then renderIPPApprFlow()

        ' If objIPPDetails IsNot Nothing Then
        If objIPPDetails.DocType = "INV" Then
            Me.lblDocType.Text = "Invoice"
        ElseIf objIPPDetails.DocType = "CN" Then
            Me.lblDocType.Text = "Credit Note"
        ElseIf objIPPDetails.DocType = "DN" Then
            Me.lblDocType.Text = "Debit Note"
        ElseIf objIPPDetails.DocType = "BILL" Then
            Me.lblDocType.Text = "Bill"
        ElseIf objIPPDetails.DocType = "LETTER" Then
            Me.lblDocType.Text = "Letter"
        End If
        Me.lblMasterDoc.Text = IIf(objIPPDetails.MasterDoc = "Y", "Yes", "No")
        ViewState("MasterDoc") = objIPPDetails.MasterDoc
        Me.lblDocNo.Text = objIPPDetails.DocNo
        Me.lblDocDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objIPPDetails.DocDate)
        'Me.lblDocReceivedDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objIPPDetails.DocReceivedDate)
        Me.lblDocDueDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objIPPDetails.DocDueDate)
        Me.lblPRCSSentDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objIPPDetails.PRCSSentDate)
        Me.lblPRCSReceivedDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objIPPDetails.PRCSReceivedDate)
        'Me.lblBankerChequeNo.Text = objIPPDetails.BankerChequeNo
        Me.lblManualPONo.Text = objIPPDetails.ManualPONo
        Me.lblStatus.Text = objIPPDetails.StatusDescription

        'Zulham 15/02/2016 - IPP Stage 4 Phase 2
        If Not objIPPDetails.TotalAmtNoGST.Trim = "" Then Me.lblTotalAmtNoGST.Text = FormatNumber(objIPPDetails.TotalAmtNoGST, 2)
        If Not objIPPDetails.GSTAmt.Trim = "" Then Me.lblGSTAmt.Text = FormatNumber(objIPPDetails.GSTAmt, 2)

        Me.lblCurrency.Text = objIPPDetails.Currency
        'Zulham 29032015 Case 8603
        If (objIPPDetails.PaymentMethod = "TT" Or objIPPDetails.PaymentMethod.ToUpper.Contains("NOSTRO")) And objIPPDetails.Status = "4" Then
            Me.lblPaymentAmt.Text = objIPPDetails.PaymentAmt & " (MYR" & Format(CDec(objIPPDetails.PaymentAmt) * CDec(objIPPDetails.ExchangeRate), "Standard") & " @ " & objIPPDetails.ExchangeRate & ")"
        Else
            Me.lblPaymentAmt.Text = objIPPDetails.PaymentAmt
        End If

        If objIPPDetails.PaymentMethod = "BC" Or objIPPDetails.PaymentMethod = "TT" Then
            If objIPPDetails.BankerChequeNo = "" Then
                Me.lblPaymentMethod.Text = objIPPDetails.PaymentMethod & "[ - ]"
            Else
                Me.lblPaymentMethod.Text = objIPPDetails.PaymentMethod & " [BC#" & objIPPDetails.BankerChequeNo & "]"
            End If

        Else
            Me.lblPaymentMethod.Text = objIPPDetails.PaymentMethod
            'Zulham 02/03/2015 IPP GST Stage 2B
            If Me.lblPaymentMethod.Text.Trim.ToLower = "nostro" Then
                lblPaymentMethod.Text = "Nostro Expenses"
            End If
        End If

        'Zulham 23112018
        Select Case objIPPDetails.PaymentMethod
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

        'Zulham 09072018 - PAMB
        'Me.txtTax.Text = objIPPDetails.WHTTax
        'Me.rbtnWHTOpt.SelectedValue = objIPPDetails.WHTOpt
        'Me.txtNoWHT.Text = objIPPDetails.WHTReason
        'End
        If objIPPDetails.GSTRegNo <> "" Then
            Me.lblVendor.Text = objIPPDetails.Vendor & " (" & objIPPDetails.GSTRegNo & ")"
        Else
            Me.lblVendor.Text = objIPPDetails.Vendor
        End If

        'Zulham 15/02/2016 - IPP Stage 4 Phase 2
        'Show employee reg ID
        Dim coyType = objdb.GetVal("select ifnull(ic_coy_type,'') 'coyType' from ipp_company where ic_index = '" & VenIdx & "'")
        Dim empRegId = objdb.GetVal("select ifnull(ic_business_reg_no,'') 'regNo' from ipp_company where ic_index = '" & VenIdx & "'")
        If coyType.ToString.Trim = "E" Then
            If empRegId.ToString.Trim.Length > 0 Then
                Me.lblVendor.Text = objIPPDetails.Vendor & " (" & empRegId & ")"
            Else
                Me.lblVendor.Text = objIPPDetails.Vendor
            End If
        End If

        Me.lblVendorAddr.Text = Replace(objIPPDetails.VAddr, "<BR>", "<BR>&nbsp;") & "<BR>&nbsp;"
        Me.txtRemarks.Text = objIPPDetails.Remarks
        Me.txtLateReason.Text = objIPPDetails.LateSubmitReason
        ' Me.txtRemarks1.Text = objIPPDetails.BillInvApprBy 'Zulham

        If objIPPDetails.PaymentMethod = "BC" Then
            Me.Label14.Text = "Bank Code :"
        Else
            Me.Label14.Text = "Bank Code[Bank A/C No.] :"
        End If

        'Zulham 17042015 IPP GST Stage 2B
        If Not objIPPDetails.BankNameAccountNo = "[]" Then
            Me.lblBankNameAccountNo.Text = objIPPDetails.BankNameAccountNo
        Else
            Me.lblBankNameAccountNo.Text = ""
        End If

        Me.lblPaymentDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objIPPDetails.PaymentDate)
        Me.lblPaymentNo.Text = objIPPDetails.PaymentNo
        'zulham 29032015 Case 8603
        If objIPPDetails.PaymentMethod = "RENTAS" Or objIPPDetails.PaymentMethod = "TT" Or objIPPDetails.PaymentMethod.ToUpper.Contains("NOSTRO") Then
            Me.txtBenficiaryDetails.Text = objIPPDetails.BeneficiaryDetails
        Else
            Me.txtBenficiaryDetails.Enabled = False
        End If

        If objIPPDetails.CreditTerm <> "" Then
            Me.lblCreditTerm.Text = objIPPDetails.CreditTerm & " days"
        Else
            Me.lblCreditTerm.Text = ""
        End If

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

        'objIPP = Nothing
        'objIPPDetails = Nothing

        If Request.QueryString("Frm") = "dashboard" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx")

        ElseIf Request.QueryString("Frm") = "IPPList" Then
            lnkBack.NavigateUrl = dDispatcher.direct("IPP", "IPPList.aspx")

        ElseIf Request.QueryString("Frm") = "IPPEnq" Then
            lnkBack.NavigateUrl = dDispatcher.direct("IPP", "IPPEnq.aspx")
        ElseIf Request.QueryString("Frm") = "PSDAcceptRejList" Then
            lnkBack.NavigateUrl = dDispatcher.direct("IPP", "PSDAcceptReject.aspx")
        ElseIf Request.QueryString("Frm") = "PSDSent" Then
            lnkBack.NavigateUrl = dDispatcher.direct("IPP", "PSDSent.aspx")
        ElseIf Request.QueryString("Frm") = "PSDRecv" Then
            lnkBack.NavigateUrl = dDispatcher.direct("IPP", "PSDRecv.aspx")
        ElseIf Request.QueryString("Frm") = "PSDFyFa" Then
            lnkBack.NavigateUrl = dDispatcher.direct("IPP", "PSDFyFa.aspx")
        ElseIf Request.QueryString("Frm") = "PSDAcceptRejList,dashboard" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashboard.aspx")
        ElseIf Request.QueryString("Frm") = "EnterBC" Then
            lnkBack.NavigateUrl = dDispatcher.direct("IPP", "BCNo.aspx")
        ElseIf Request.QueryString("Frm") = "WHTReceipt" Then
            lnkBack.NavigateUrl = dDispatcher.direct("IPP", "WHTReceipt.aspx")
        Else
            lnkBack.Visible = False
        End If

        If Request.QueryString("Frm") = "IPPEnq" Or Request.QueryString("Frm") = "WHTReceipt" Then
            Session("w_AddPO_tabs") = Nothing
        End If


        'If Request.QueryString("Frm") = "PSDAcceptRejList" Or Request.QueryString("Frm") = "PSDAcceptRejList,dashboard" Then
        If Request.QueryString("Frm") <> "IPPEnq" And Request.QueryString("Frm") <> "WHTReceipt" Then
            GenerateTab()
        End If
        'End If

        'Check for GST
        Dim documentDate = objdb.GetVal("SELECT IFNULL(im_doc_date,'') 'im_doc_date' FROM invoice_mstr WHERE im_invoice_index = '" & Session("InvIdx") & "'")
        Dim createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
        Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")

        'Zulham 13082015
        'Additional condition for company's effective date
        Dim effectiveDate = objdb.GetVal("SELECT IFNULL(ic_gst_eff_date, '') FROM IPP_COMPANY WHERE ic_coy_name = '" & Common.Parse(objIPPDetails.Vendor) & "'")

        If Trim(effectiveDate) <> "" Then
            If CDate(createdDate) >= CDate(_cutoffDate) And CDate(createdDate) >= CDate(effectiveDate) Then
                _exceedCutOffDt = "Yes"
                If lblVendor.Text <> "" Then
                    Dim GSTRegNo = objdb.GetVal("SELECT IFNULL(IC_TAX_REG_NO, '') FROM IPP_COMPANY WHERE ic_coy_name = '" & Common.Parse(objIPPDetails.Vendor) & "'")
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
        'Zulham 20082015
        If CDate(createdDate) >= CDate(_cutoffDate) Then
            _exceedCutOffDt = "Yes"
        End If

        Dim Asset As New PurchaseOrder_Buyer
        If strIsGst = "Yes" Then
            dtgDocDetail.Columns(EnumIPPDet.ippReimbursement).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippForeignGSTAmount).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippGSTAmount).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippInputTax).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippOutputTax).Visible = True
            dtgSubDocDetail.Columns(EnumIPPDet.ippInvoiceNo).Visible = True
        ElseIf _exceedCutOffDt = "Yes" Then
            dtgDocDetail.Columns(EnumIPPDet.ippReimbursement).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippForeignGSTAmount).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippGSTAmount).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippInputTax).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippOutputTax).Visible = True
            dtgSubDocDetail.Columns(EnumIPPDet.ippInvoiceNo).Visible = True
        Else
            dtgDocDetail.Columns(EnumIPPDet.ippReimbursement).Visible = False
            dtgDocDetail.Columns(EnumIPPDet.ippForeignGSTAmount).Visible = False
            dtgDocDetail.Columns(EnumIPPDet.ippGSTAmount).Visible = False
            dtgDocDetail.Columns(EnumIPPDet.ippInputTax).Visible = False
            dtgDocDetail.Columns(EnumIPPDet.ippOutputTax).Visible = False
            dtgSubDocDetail.Columns(EnumIPPDet.ippInvoiceNo).Visible = False
        End If

        'Jules 2018.07.11 - PAMB
        'Zulham 05/05/2016 - Changed the index from 20 to 21
        'dtgDocDetail.Columns(21).Visible = False 'Hide cost allocation
        'End
        dtgDocDetail.Columns(EnumIPPDet.ippCurrency).Visible = False 'Hide Currency Code
        dtgDocDetail.Columns(EnumIPPDet.ippForeignGSTAmount).Visible = False 'FCY GST 
        dtgDocDetail.Columns(EnumIPPDet.ippCostAlloc).Visible = False 'Hide Cost Allocation        
        dtgDocDetail.Columns(EnumIPPDet.ippExchangeRate).Visible = False 'Hide Exchange Rate 
        'End modification.

        'Zulham PAMB 11/04/2018
        displayAttachFile()
        'End

        objIPP = Nothing
        objIPPDetails = Nothing

    End Sub

    Private Sub renderIPPApprFlow()
        Dim objDoc As New IPPMain

        'Zulham 13082018 - PAMB
        'Dim ds As DataSet = objDoc.getIPPApprFlow(Session("Index"), Common.Parse(HttpContext.Current.Session("CompanyID")))
        Dim ds As DataSet = objDoc.getE2PApprFlow(Session("InvIdx"), Common.Parse(HttpContext.Current.Session("CompanyID")))

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
            'Common.NetMsgbox(Me, MsgNoRecord)
        End If
        objDoc = Nothing
    End Sub

    Private Function BindgridSubDoc(Optional ByVal pSorted As Boolean = False) As String
        Dim objIPPmain As New IPPMain
        Dim ds As New DataSet
        Dim dvViewIPP As DataView

        ds = objIPPmain.getSubDocDetail(Session("InvIdx"))
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
        Dim objIPP As New IPP
        Dim objGst As New GST
        Dim ds As DataSet
        Dim dvViewIPP As DataView
        Dim objIPPDet As New IPPDetails

        If Request.QueryString("Frm") = "IPPEnq" Or Request.QueryString("Frm") = "WHTReceipt" Then
            ds = objIPP.IPPEnq_detail(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")), VenIdx)
        ElseIf Request.QueryString("Frm") = "PSDAcceptRejList" Or Request.QueryString("Frm") = "PSDAcceptRejList,dashboard" Or Request.QueryString("Frm") = "EnterBC" Then
            ds = objIPP.ipp_detail(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")), VenIdx, Request.QueryString("Frm"))
        ElseIf Request.QueryString("Frm") = "PSDSent" Then
            ds = objIPP.ipp_detail(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")), VenIdx, , Request.QueryString("CreatedBy"))
        Else
            ds = objIPP.ipp_detail(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")), VenIdx)
        End If

        dvViewIPP = ds.Tables(0).DefaultView

        'intPageRecordCnt = ds.Tables(0).Rows.Count
        

        If pSorted Then
            dvViewIPP.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewIPP.Sort += " DESC"
        End If

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count 'intPageRecordCnt
        '//bind datagrid

        'Chee Hong - 19 May 2015 (Performance Issue)
        'Get dataview of Input/ Output Tax Code before databind
        Dim dsInput, dsOutput As New DataSet
        dsInput = objGst.GetTaxCode_forIPP(, "P", "IPP")
        dsOutput = objGst.GetTaxCode_forIPP(, "S", "IPP")
        dvViewInput = dsInput.Tables(0).DefaultView
        dvViewOutput = dsOutput.Tables(0).DefaultView

        If ds.Tables(0).Rows.Count > 0 Then
            dtgDocDetail.DataSource = dvViewIPP
            dtgDocDetail.DataBind()
        Else
            dtgDocDetail.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ' *** If either Credit/Debit Note, display Invoice No. column
        If Request.QueryString("Frm") = "IPPEnq" Or Request.QueryString("Frm") = "WHTReceipt" Then
            objIPPDet = objIPP.GetIPPEnqDetails(Session("DocNo").ToString.Replace("\", "\\"), Common.Parse(HttpContext.Current.Session("CompanyID")), Session("InvIdx")) 'Zulham 19072018 - PAMB
        ElseIf Request.QueryString("Frm") = "PSDSent" Then
            objIPPDet = objIPP.GetIPPDetails(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")), Session("InvIdx"), Request.QueryString("Frm"), Request.QueryString("CreatedBy"))
        Else
            objIPPDet = objIPP.GetIPPDetails(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")), Session("InvIdx"), Request.QueryString("Frm"))
        End If

        'If objIPPDet IsNot Nothing Then
        If objIPPDet.DocType = "CN" Or objIPPDet.DocType = "DN" Then
            Me.dtgDocDetail.Columns(EnumIPPDet.ippInvoiceNo).Visible = True
        Else
            Me.dtgDocDetail.Columns(EnumIPPDet.ippInvoiceNo).Visible = False
        End If
        'End If

        'Zulham 29032015 Case 8603
        'Zulham 14022019
        If (objIPPDet.PaymentMethod = "TT" Or objIPPDet.PaymentMethod.ToUpper.Contains("NOSTRO") Or objIPPDet.PaymentMethod.ToUpper.Contains("BD")) And (objIPPDet.Status = "13" Or objIPPDet.Status = "4") Then
            Me.dtgDocDetail.Columns(EnumIPPDet.ippAmt).Visible = True
        ElseIf (objIPPDet.PaymentMethod = "TT" Or objIPPDet.PaymentMethod.ToUpper.Contains("NOSTRO") Or objIPPDet.PaymentMethod.ToUpper.Contains("BD")) Then
            Me.dtgDocDetail.Columns(EnumIPPDet.ippAmt).Visible = True
            'Me.dtgDocDetail.Columns(EnumIPPDet.ippAmt2).Visible = False
        Else
            Me.dtgDocDetail.Columns(EnumIPPDet.ippAmt).Visible = False
            Me.dtgDocDetail.Columns(EnumIPPDet.ippAmt2).Visible = True
        End If

        'Return

        objIPP = Nothing

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

    Private Sub dtgSubDocDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgSubDocDetail.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dblAmount, dblGSTAmount As Double

            If IsDBNull(dv("ISD_DOC_DATE")) Then
                e.Item.Cells(EnumSubDocDet.subDocDate).Text = ""
            Else
                e.Item.Cells(EnumSubDocDet.subDocDate).Text = Format(dv("ISD_DOC_DATE"), "dd/MM/yyyy")
            End If

            dblAmount = Common.parseNull(dv("ISD_DOC_AMT"), 0)
            e.Item.Cells(EnumSubDocDet.subDocAmt).Text = Format(dblAmount, "#,##0.00")

            dblGSTAmount = Common.parseNull(dv("ISD_DOC_GST_VALUE"), 0)
            If CDec(dblGSTAmount) > CDec(0) Then
                e.Item.Cells(EnumSubDocDet.subEmpty).Text = Format(dblGSTAmount, "#,##0.00")
            Else
                e.Item.Cells(EnumSubDocDet.subEmpty).Text = "0.00"
            End If

            ViewState("totalSubAmt") = ViewState("totalSubAmt") + dblAmount + dblGSTAmount
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
    Private Sub dtgApprvFlow_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgApprvFlow.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim intTotalCell, intLoop As Integer

            If Not IsDBNull(dv("FA_ACTION_DATE")) Then
                e.Item.Cells(2).Text = dv("FA_ACTION_DATE")
            End If

            'Zulham 09072018 - PAMB
            e.Item.Cells(0).Text = e.Item.Cells(0).Text.Replace("IPP", "E2P")

        End If
    End Sub

    Private Sub dtgDocDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDocDetail.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dblAmount As Double
            Dim dblAmount2 As Double
            Dim GST As New GST

            'Zulham 29032015 Case 8603
            'Zulham 14022019
            If (dv("IM_PAYMENT_TERM") = "TT" Or dv("IM_PAYMENT_TERM").ToString.ToUpper.Contains("NOSTRO") Or dv("IM_PAYMENT_TERM") = "BD") Then
                dblAmount2 = (Common.parseNull(dv("ID_RECEIVED_QTY"), 0) * Common.parseNull(dv("ID_UNIT_COST"), 0)) * Common.parseNull(dv("IM_EXCHANGE_RATE"), 0)
                dblAmount = Common.parseNull(dv("ID_RECEIVED_QTY"), 0) * Common.parseNull(dv("ID_UNIT_COST"), 0)
                e.Item.Cells(EnumIPPDet.ippAmt).Text = Format(dblAmount, "#,##0.00")
                e.Item.Cells(EnumIPPDet.ippAmt2).Text = Format(dblAmount2, "#,##0.00")
                ViewState("total2") = ViewState("total2") + dblAmount2
                ViewState("total") = ViewState("total") + dblAmount
                e.Item.Cells(EnumIPPDet.ippCostAlloc).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("IPP", "CostAllocDetail.aspx", "CostAllocCode=" & dv("ID_COST_ALLOC_CODE")) & "&Amount=" & dblAmount & "&InvIdx=" & dv("IM_INVOICE_INDEX") & "&InvLine=" & e.Item.Cells(EnumIPPDet.ippSNo).Text & "&VenIdx=" & VenIdx & "')"" ><font color=#0000ff>" & Common.parseNull(dv("ID_COST_ALLOC_CODE")) & "</font></A>"
            ElseIf dv("IM_PAYMENT_TERM") = "TT" Or dv("IM_PAYMENT_TERM").ToString.ToUpper.Contains("NOSTRO") Or dv("IM_PAYMENT_TERM") = "BD" Then
                dblAmount = Common.parseNull(dv("ID_RECEIVED_QTY"), 0) * Common.parseNull(dv("ID_UNIT_COST"), 0)
                e.Item.Cells(EnumIPPDet.ippAmt).Text = Format(dblAmount, "#,##0.00")
                ViewState("total") = ViewState("total") + dblAmount
                e.Item.Cells(EnumIPPDet.ippCostAlloc).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("IPP", "CostAllocDetail.aspx", "CostAllocCode=" & dv("ID_COST_ALLOC_CODE")) & "&Amount=" & dblAmount & "&InvIdx=" & dv("IM_INVOICE_INDEX") & "&InvLine=" & e.Item.Cells(EnumIPPDet.ippSNo).Text & "&VenIdx=" & VenIdx & "')"" ><font color=#0000ff>" & Common.parseNull(dv("ID_COST_ALLOC_CODE")) & "</font></A>"
            Else
                dblAmount2 = Common.parseNull(dv("ID_RECEIVED_QTY"), 0) * Common.parseNull(dv("ID_UNIT_COST"), 0)
                e.Item.Cells(EnumIPPDet.ippAmt2).Text = Format(dblAmount2, "#,##0.00")
                ViewState("total2") = ViewState("total2") + dblAmount2
                e.Item.Cells(EnumIPPDet.ippCostAlloc).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("IPP", "CostAllocDetail.aspx", "CostAllocCode=" & dv("ID_COST_ALLOC_CODE")) & "&Amount=" & dblAmount2 & "&InvIdx=" & dv("IM_INVOICE_INDEX") & "&InvLine=" & e.Item.Cells(EnumIPPDet.ippSNo).Text & "&VenIdx=" & VenIdx & "')"" ><font color=#0000ff>" & Common.parseNull(dv("ID_COST_ALLOC_CODE")) & "</font></A>"
            End If

            e.Item.Cells(EnumIPPDet.ippUnitPrice).Text = Format(dv("ID_UNIT_COST"), "#,##0.00")

            'Zulham 10072018 - PAMB
            'Some condition for withholding tax
            If Common.parseNull(dv("id_withholding_opt")).ToString = "3" Then
                e.Item.Cells(EnumIPPDet.ippWHT).Text = Common.parseNull(dv("id_withholding_remarks"))
            End If
            'End

            If UCase(Common.parseNull(dv("ID_PAY_FOR"))) = UCase(Session("CompanyId")) Then
                e.Item.Cells(EnumIPPDet.ippPayFor).Text = "Own Company"
            ElseIf UCase(Common.parseNull(dv("ID_PAY_FOR"))) = UCase("HLISB") Then
                e.Item.Cells(EnumIPPDet.ippExchangeRate).ForeColor = Color.White
            Else
                e.Item.Cells(EnumIPPDet.ippPayFor).Text = Common.parseNull(dv("ID_PAY_FOR"))
                e.Item.Cells(EnumIPPDet.ippPayFor).ToolTip = objdb.GetVal("SELECT ic_coy_name FROM ipp_company WHERE ic_other_b_coy_code = '" & dv("ID_PAY_FOR") & "'")
            End If

            e.Item.Cells(EnumIPPDet.ippAssetGrp).ToolTip = Common.parseNull(dv("ID_ASSET_GROUP_DESC"))
            e.Item.Cells(EnumIPPDet.ippAssetSub).ToolTip = Common.parseNull(dv("ID_ASSET_SUB_GROUP_DESC"))
            e.Item.Cells(EnumIPPDet.ippCostCentre).ToolTip = Common.parseNull(dv("ID_COST_CENTER_DESC"))
            e.Item.Cells(EnumIPPDet.ippGL).Text = Common.parseNull(dv("ID_B_GL_CODE")) & ":" & Common.parseNull(dv("CBG_B_GL_DESC"))

            'Zulham 24102018
            Dim costCenterDesc = objdb.GetVal("SELECT cc_CC_desc FROM cost_centre WHERE cc_coy_id = '" & Session("CompanyID") & "' and cc_cc_code = '" & Common.parseNull(dv("ID_COST_CENTER")) & "'")
            e.Item.Cells(EnumIPPDet.ippCostCentre).Text = Common.parseNull(dv("ID_COST_CENTER")) & ":" & IIf(Common.parseNull(dv("ID_COST_CENTER_DESC")) = "", costCenterDesc, Common.parseNull(dv("ID_COST_CENTER_DESC")))


            'Zulham Sept 17, 2014
            Dim ddlInputTax, ddlOutputTax As DropDownList
            Dim lblGSTAmount As Label
            Dim lblFcyGSTAmt As Label
            Dim objGlobal As New AgoraLegacy.AppGlobals
            ddlInputTax = e.Item.FindControl("ddlInputTax")
            ddlOutputTax = e.Item.FindControl("ddlOutputTax")
            lblGSTAmount = e.Item.FindControl("lblGSTAmount")
            lblFcyGSTAmt = e.Item.FindControl("lblForeignGSTAmount")
            'Zulham 12/01/2015 - IPP Stage 4 Phase 2
            'Show gst amount in fcy and myr 
            Dim myrGST, fcyGST As Double
            myrGST = (Common.parseNull(dv("id_gst_value"), 0) * Common.parseNull(dv("IM_EXCHANGE_RATE"), 0))
            fcyGST = Common.parseNull(dv("id_gst_value"), 0)
            If Not dv("IM_currency_code") Is Nothing Then
                If Not dv("IM_currency_code") Is DBNull.Value Then
                    'Zulham 09/05/2016 - IM5/IM6 Enhancement
                    'IM5 GST Value should be shown in MYR column
                    If Not dv("id_gst_input_tax_code") Is DBNull.Value Then
                        If Not Common.parseNull(dv("IM_currency_code"), "") = "MYR" And Not Trim(dv("id_gst_input_tax_code")) = "IM5" Then
                            If Not myrGST.ToString.Trim = "0" Then e.Item.Cells(EnumIPPDet.ippGSTAmount).Text = Format(myrGST, "#,##0.00")
                            e.Item.Cells(EnumIPPDet.ippForeignGSTAmount).Text = Format(fcyGST, "#,##0.00")
                            lblFcyGSTAmt.Text = Format(fcyGST, "#,##0.00")
                        Else
                            e.Item.Cells(EnumIPPDet.ippGSTAmount).Text = Format(fcyGST, "#,##0.00")
                            lblGSTAmount.Text = Format(fcyGST, "#,##0.00")
                        End If
                    End If
                Else
                    e.Item.Cells(EnumIPPDet.ippGSTAmount).Text = Format(fcyGST, "#,##0.00")
                    lblGSTAmount.Text = Format(fcyGST, "#,##0.00")
                End If
            Else
                e.Item.Cells(EnumIPPDet.ippGSTAmount).Text = Format(fcyGST, "#,##0.00")
                lblGSTAmount.Text = Format(fcyGST, "#,##0.00")
            End If

            'Chee Hong - 19 May 2015 (Performance issue)
            'GST.FillTaxCode_forIPP(ddlInputTax, "", "P")
            'GST.FillTaxCode_forIPP(ddlOutputTax, "", "S")
            'Fill dataview into tax code dropdownlist
            ddlInputTax.Items.Clear()
            Dim lstItem As New ListItem
            If Not dvViewInput Is Nothing Then
                Common.FillDdl(ddlInputTax, "GST", "TM_TAX_CODE", dvViewInput)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                'Zulham 08102018 - PAMB SST
                'ddlInputTax.Items.Insert(0, lstItem)
            Else
                lstItem.Value = "N/A"
                lstItem.Text = "N/A"
                ddlInputTax.Items.Clear()
                ddlInputTax.Items.Insert(0, lstItem)
                Common.SelDdl(lstItem.Value, ddlInputTax, True, True)
            End If

            ddlOutputTax.Items.Clear()
            If Not dvViewOutput Is Nothing Then
                Common.FillDdl(ddlOutputTax, "GST", "TM_TAX_CODE", dvViewOutput)
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                'Zulham 08102018 - PAMB SST
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
                    ddlInputTax.SelectedValue = dv("id_gst_input_tax_code").ToString.Trim
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
                    ddlInputTax.Items.Add(New ListItem("N/A", "N/A"))
                    ddlInputTax.Enabled = False
                    ddlInputTax.SelectedValue = "N/A"
                End If
            Else
                'Zulham 09042015 IPP GST Stage 1
                ddlInputTax.Items.Add(New ListItem("N/A", "N/A"))
                ddlInputTax.Enabled = False
                ddlInputTax.SelectedValue = "N/A"
            End If
            'Zulham 08102018 - PAMB SST
            If Common.parseNull(dv("id_gst_output_tax_code")) <> "" Then
                If Not dv("id_gst_output_tax_code").ToString = "0" And Not dv("id_gst_output_tax_code").ToString = "N/A" Then
                    ddlOutputTax.SelectedValue = dv("id_gst_output_tax_code").ToString.Trim
                Else
                    'Zulham 09042015 IPP GST Stage 1
                    ddlOutputTax.Items.Add(New ListItem("N/A", "N/A"))
                    ddlOutputTax.Enabled = False
                    ddlOutputTax.SelectedValue = "N/A"
                End If
            Else
                ddlOutputTax.Items.Add(New ListItem("N/A", "N/A"))
                ddlOutputTax.SelectedValue = "N/A"
            End If

            'Zulham 18/02/2016 - IPP Stage 4 Phase 2
            'Comemnted the line below
            'lblGSTAmount.Text = Common.parseNull(dv("id_gst_value"), 0.0)
            'If Not lblGSTAmount.Text = "" Then ViewState("gstTotal") = ViewState("gstTotal") + CInt(lblGSTAmount.Text)

            If Common.parseNull(dv("id_gst_input_tax_code")) <> "" Then
                If Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Contains("(") Then
                    'Zulham 13112018
                    'If Not Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Contains("0") Then
                    If Not lblGSTAmount.Text = "" Then ViewState("gstTotal") = ViewState("gstTotal") + CDec(lblGSTAmount.Text)
                    'End If
                    If Not Common.parseNull(dv("IM_currency_code"), "") = "MYR" And ddlInputTax.SelectedItem.Text.ToString.Trim.Contains("TX4") Then
                        If Not lblFcyGSTAmt.Text = "" Then ViewState("gstTotal") = ViewState("gstTotal") + CDec(lblFcyGSTAmt.Text)
                    ElseIf Not Common.parseNull(dv("IM_currency_code"), "") = "MYR" And ddlOutputTax.SelectedItem.Text.ToString.Trim.Contains("N/A") Then
                        If Not lblFcyGSTAmt.Text = "" Then ViewState("gstTotal") = ViewState("gstTotal") + CDec(lblFcyGSTAmt.Text)
                    End If
                End If
            End If

            If Common.parseNull(dv("id_gst_input_tax_code")) <> "" Then
                If Common.parseNull(ddlOutputTax.SelectedItem.Text).ToString.Contains("(") Then
                    'Zulham 13112018
                    'If Not Common.parseNull(ddlOutputTax.SelectedItem.Text).ToString.Contains("0") Then
                    'Zulham 06052015 IPP GST Stage 1
                    If Not ddlInputTax.SelectedItem.Text.ToString.Trim.Contains("TX4") Then
                        If Not lblGSTAmount.Text = "" Then ViewState("gstTotal") = ViewState("gstTotal") - CDec(lblGSTAmount.Text)
                    End If
                    'End If
                End If
            End If

            ddlInputTax.Enabled = False
            ddlOutputTax.Enabled = False

            'End
            If IsDBNull(dv("ID_GST_REIMB")) Then
                e.Item.Cells(EnumIPPDet.ippReimbursement).Text = "N/A"
            ElseIf dv("ID_GST_REIMB") = "R" Then
                e.Item.Cells(EnumIPPDet.ippReimbursement).Text = "Reimbursement"
            ElseIf dv("ID_GST_REIMB") = "D" Then
                e.Item.Cells(EnumIPPDet.ippReimbursement).Text = "Disbursement"
                ddlInputTax.Enabled = False
            Else
                e.Item.Cells(EnumIPPDet.ippReimbursement).Text = "N/A"
            End If

            'Zulham 27052015 IPP GST Stage 1
            'Hide the exchange rate if it's 1
            If e.Item.Cells(EnumIPPDet.ippPayFor).Text = "Own Company" Then
                e.Item.Cells(EnumIPPDet.ippExchangeRate).Text = ""
            End If

        End If
    End Sub

    Sub AddRowtotalSubDoc() 'add total row 
        Dim intL As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        For intL = 0 To Me.dtgSubDocDetail.Columns.Count - 1
            addCell(row)
        Next

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
        row.Cells(4).BorderStyle = BorderStyle.None

        row.CssClass = "linespacing2"
        row.BorderStyle = BorderStyle.None
        row.BackColor = Drawing.Color.White
        Me.dtgSubDocDetail.Controls(0).Controls.Add(row)

        ViewState("totalSubAmt") = 0

    End Sub

    Sub AddRowtotal() 'add total row 
        Dim gstAmount = 0.0
        Dim intL As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        For intL = 0 To Me.dtgDocDetail.Columns.Count - 1
            addCell(row)
        Next

        If Not ViewState("gstTotal") Is Nothing Then
            If Not ViewState("gstTotal").ToString.Length = 0 Then
                gstAmount = ViewState("gstTotal")
            End If
        End If

        row.Cells(EnumIPPDet.ippUnitPrice).Text = "Total :"
        row.Cells(EnumIPPDet.ippUnitPrice).HorizontalAlign = HorizontalAlign.Center
        row.Cells(EnumIPPDet.ippUnitPrice).Font.Bold = True
        row.Cells(EnumIPPDet.ippUnitPrice).BorderColor = Color.Transparent
        'If Me.lblPaymentMethod.Text = "TT" Then
        row.Cells(EnumIPPDet.ippAmt).Text = Format(ViewState("total") + gstAmount, "#,##0.00")
        row.Cells(EnumIPPDet.ippAmt).HorizontalAlign = HorizontalAlign.Center
        row.Cells(EnumIPPDet.ippAmt).Font.Bold = True
        row.Cells(EnumIPPDet.ippAmt).Font.Underline = True
        row.Cells(EnumIPPDet.ippAmt).Font.Overline = True
        row.Cells(EnumIPPDet.ippAmt).HorizontalAlign = HorizontalAlign.Right
        row.Cells(EnumIPPDet.ippAmt).BorderColor = Color.Transparent

        'End If

        row.Cells(EnumIPPDet.ippAmt2).Text = Format(ViewState("total2") + gstAmount, "#,##0.00")
        row.Cells(EnumIPPDet.ippAmt2).HorizontalAlign = HorizontalAlign.Center
        row.Cells(EnumIPPDet.ippAmt2).Font.Bold = True
        row.Cells(EnumIPPDet.ippAmt2).Font.Underline = True
        row.Cells(EnumIPPDet.ippAmt2).Font.Overline = True
        row.Cells(EnumIPPDet.ippAmt2).HorizontalAlign = HorizontalAlign.Right
        row.Cells(EnumIPPDet.ippAmt2).BorderColor = Color.Transparent

        row.Cells(EnumIPPDet.ippSNo).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippPayFor).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippInvoiceNo).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippDesc).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippQty).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippUOM).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippGL).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippAssetGrp).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippAssetSub).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippCostAlloc).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippGLRuleCat).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippBranch).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippCostCentre).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippCurrency).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippExchangeRate).BorderColor = Color.Transparent

        'Zulham Sept 18, 2014
        row.Cells(EnumIPPDet.ippGSTAmount).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippInputTax).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippOutputTax).BorderColor = Color.Transparent
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

        row.CssClass = "linespacing2"
        row.BorderStyle = BorderStyle.None
        row.BackColor = Drawing.Color.Transparent
        Me.dtgDocDetail.Controls(0).Controls.Add(row)

        ViewState("total") = 0
    End Sub

    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub
    Private Sub GenerateTab()

        Dim objUsers As New Users

        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        'Get current User's role
        '
        'If objUsers.checkUserFixedRole("'IPP Officer'", Common.parseNull(HttpContext.Current.Session("UserID"))) Then

        'End If

        If Request.QueryString("Frm") = "PSDAcceptRejList" Or Request.QueryString("Frm") = "PSDAcceptRejList,dashboard" Then
            Session("w_AddPO_tabs") = "<div class=""t_entity""><ul>" & _
               "<li><div class=""space""></div></li>" & _
               "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "PSDRECV.aspx", "mode=new&type=Listing&pageid=" & strPageId) & """><span>PSD Acceptance List</span></a></li>" & _
               "<li><div class=""space""></div></li>" & _
               "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPP", "PSDAcceptReject.aspx", "mode=new&type=Listing&pageid=" & strPageId) & """><span>PSD Accepted/Rejected Listing</span></a></li>" & _
               "<li><div class=""space""></div></li>" & _
               "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "PSDFyfa.aspx", "type=Listing&mode=new&pageid=" & strPageId) & """><span>Pending FYFA</span></a></li>" & _
               "<li><div class=""space""></div></li>" & _
               "</ul><div></div></div>"
        ElseIf Me.Request.QueryString("Frm") = "PSDFyFa" Then
            Session("w_AddPO_tabs") = "<div class=""t_entity""><ul>" & _
              "<li><div class=""space""></div></li>" & _
                          "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "PSDRECV.aspx", "mode=new&type=Listing&pageid=" & strPageId) & """><span>PSD Acceptance List</span></a></li>" & _
                          "<li><div class=""space""></div></li>" & _
                          "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "PSDAcceptReject.aspx", "mode=new&type=Listing&pageid=" & strPageId) & """><span>PSD Accepted/Rejected Listing</span></a></li>" & _
                          "<li><div class=""space""></div></li>" & _
                          "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPP", "PSDFyfa.aspx", "type=Listing&mode=new&pageid=" & strPageId) & """><span>Pending FYFA</span></a></li>" & _
                          "<li><div class=""space""></div></li>" & _
                          "</ul><div></div></div>"
        ElseIf objUsers.checkUserFixedRole("'Finance Officer'", Common.parseNull(HttpContext.Current.Session("UserID"))) Then
            Session("w_AddPO_tabs") = Nothing
        Else
            Session("w_AddPO_tabs") = "<div class=""t_entity""><ul>" &
                 "<li><div class=""space""></div></li>" &
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "IPPEntry.aspx", "mode=new&pageid=" & strPageId) & """><span>E2P Document</span></a></li>" &
                             "<li><div class=""space""></div></li>" &
                             "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPP", "IPPList.aspx", "type=Listing&mode=new&pageid=" & strPageId) & """><span>E2P Document Listing</span></a></li>" &
                             "<li><div class=""space""></div></li>" &
                             "</ul><div></div></div>"
        End If

    End Sub

    Private Sub cmdViewAudit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdViewAudit.Click
        Dim strscript As New System.Text.StringBuilder
        Dim objIPPDetails As New IPPDetails
        Dim strFileName As String

        strscript.Append("<script language=""javascript"">")
        'Zulham 13072018 - PAMB
        strFileName = dDispatcher.direct("IPP", "ViewIPPAuditTrails.aspx", "pageid=" & strPageId & "&docno=" & Server.UrlEncode(Session("DocNo")) & "&docstatus=" & Server.UrlEncode(lblStatus.Text) & "&docidx=" & Session("InvIdx"))
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'520px');")
        'strscript.Append("document.getElementById('btnhidden3').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script1", strscript.ToString())
    End Sub
#Region "PAMB"
    Private Sub displayAttachFile()

        Dim objIPP As New IPP
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        dsAttach = objIPP.getIPPTempAttach(Session("InvIdx"), "I")

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
#End Region
End Class