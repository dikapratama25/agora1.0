Imports AgoraLegacy
Imports eProcure.Component
Imports System.drawing
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient


Partial Public Class PSDAcceptanceDetails
    Inherits AgoraLegacy.AppBaseClass
    Dim dblInvoiceAmount, prevAppType As String
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objdb As New EAD.DBCom
    Dim VenIdx As Integer = 0
    Dim objGlobal As New AppGlobals
    Dim _exceedCutOffDt As String = ""
    Dim strIsGst As String

    Public Enum EnumSubDocDet
        subDocNo = 0
        subDocDate = 1
        subDocAmt = 2
        subEmpty = 3
        subGST = 4
    End Enum

    Public Enum EnumIPPDet
        ippSNo = 0
        ippPayFor = 1
        ippReimbursement = 2
        ippInvoiceNo = 3
        ippDesc = 4
        ippUOM = 5
        ippQty = 6
        ippUnitPrice = 7
        ippAmt = 8
        ippAmt2 = 9
        ippGSTAmount = 10
        ippInputTax = 11
        ippOutputTax = 12
        ippGL = 13
        ippAssetGrp = 14
        ippAssetSub = 15
        ippGLRuleCat = 16
        ippBranch = 17
        ippCostCentre = 18
        'Zulham 13072018 - PAMB
        ippGift = 19
        ippCategory = 20
        ippACode1 = 21
        ippACode2 = 22
        ippACode3 = 23
        ippACode4 = 24
        ippACode5 = 25
        ippACode8 = 26
        ippACode9 = 27
        ippWHT = 28
        'End
        ippCostAlloc = 29
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objIPP As New IPP
        Dim ObjDoc As New IPPMain
        Dim objIPPDetails As New IPPDetails
        Dim strFrm As String
        Dim blnRelief As Boolean
        Dim intApprGrpIndex As String

        strFrm = Me.Request.QueryString("Frm")
        'Zulham 12072018 - PAMB
        Session("DocNo") = Request.QueryString("DocumentNo").Replace("\", "\\")
        Session("InvIdx") = Request.QueryString("index")

        intApprGrpIndex = objdb.GetVal("SELECT DISTINCT FA_APPROVAL_GRP_INDEX FROM finance_approval WHERE FA_INVOICE_INDEX = '" & Session("InvIdx") & "' ")
        VenIdx = objdb.GetVal("SELECT im_s_coy_id FROM invoice_mstr WHERE im_invoice_index = '" & Session("InvIdx") & "'")
        ViewState("VenIdx") = VenIdx
        MyBase.Page_Load(sender, e)
        'Zulham 14072018 - PAMB
        SetGridProperty(dtgApprvFlow, "N")
        SetGridProperty(dtgDocDetail, "N")
        SetGridProperty(dtgSubDocDetail, "N")
        'End
        renderIPPApprFlow()
        ViewState("total") = 0
        ViewState("total2") = 0
        'IPP Stage 2A - CH - 6 Mar 2015
        ViewState("gstTotal") = 0
        Bindgrid()
        GenerateTab()
        If ViewState("intPageRecordCnt") > 0 Then
            AddRowtotal()
        End If
        ViewState("line") = 0

        If ObjDoc.CheckRelief(intApprGrpIndex) Then
            blnRelief = False ' temp set to false unless user want to use this function - michael
            Session("Relief") = blnRelief
        Else
            blnRelief = False
            Session("Relief") = blnRelief
        End If
        If Request.QueryString("Frm") = "IPPEnq" Then
            objIPPDetails = objIPP.GetIPPEnqDetails(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")), Session("InvIdx"))
        Else
            objIPPDetails = objIPP.GetIPPDetails(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")), Session("InvIdx"), "PSDAcceptanceDetails")
        End If
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
        Dim dtInV As New DataTable : dtInV = objIPP.GetInvMstr(Session("InvIdx"))
        Dim sentDate = dtInV.Rows(0).Item("im_prcs_sent").ToString
        lblpsdSentDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, sentDate)
        Dim DueDate = dtInV.Rows(0).Item("im_due_date").ToString
        lblDocDueDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, DueDate)
        Me.lblManualPONo.Text = objIPPDetails.ManualPONo
        Me.lblStatus.Text = objIPPDetails.StatusDescription

        Me.lblCurrency.Text = objIPPDetails.Currency
        If objIPPDetails.PaymentMethod = "TT" And objIPPDetails.Status = 4 Then
            Me.lblPaymentAmt.Text = objIPPDetails.PaymentAmt & " (MYR" & Format(CDec(objIPPDetails.PaymentAmt) * CDec(objIPPDetails.ExchangeRate), "Standard") & " @ " & objIPPDetails.ExchangeRate & ")"
        Else
            Me.lblPaymentAmt.Text = objIPPDetails.PaymentAmt
        End If

        'Me.lblPaymentMethod.Text = objIPPDetails.PaymentMethod
        If objIPPDetails.PaymentMethod = "BC" Or objIPPDetails.PaymentMethod = "TT" Then
            If objIPPDetails.BankerChequeNo = "" Then
                Me.lblPaymentMethod.Text = objIPPDetails.PaymentMethod & "[ - ]"
            Else
                Me.lblPaymentMethod.Text = objIPPDetails.PaymentMethod & "[BC#" & objIPPDetails.BankerChequeNo & "]"
            End If
        Else
            'Zulham 26032015 Case 8603
            If objIPPDetails.PaymentMethod.ToUpper.Contains("NOSTRO") Then
                Me.lblPaymentMethod.Text = "Nostro Expenses"
            Else
                Me.lblPaymentMethod.Text = objIPPDetails.PaymentMethod
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

        'Zulham 05/01/2016 - Stage 4 Phase 2
        'get the current document status
        Session("Status") = objIPPDetails.Status.Trim

        'Zulham 13072018 - PAMB
        'Me.txtTax.Text = objIPPDetails.WHTTax
        'Me.rbtnWHTOpt.SelectedValue = objIPPDetails.WHTOpt
        'Me.txtNoWHT.Text = objIPPDetails.WHTReason
        'End

        'Me.lblVendor.Text = objIPPDetails.Vendor
        If objIPPDetails.GSTRegNo <> "" Then
            Me.lblVendor.Text = objIPPDetails.Vendor & " (" & objIPPDetails.GSTRegNo & ")"
        Else
            Me.lblVendor.Text = objIPPDetails.Vendor
        End If

        'zulham 17/02/2016 - IPP Stage 4 Phase 2
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
        'Show total w/o GST & Gst Amount
        If Not objIPPDetails.TotalAmtNoGST.Trim = "" Then Me.lblTotalAmtNoGST.Text = FormatNumber(objIPPDetails.TotalAmtNoGST, 2)
        If Not objIPPDetails.GSTAmt.Trim = "" Then Me.lblGSTAmt.Text = FormatNumber(objIPPDetails.GSTAmt, 2)

        Me.lblVendorAddr.Text = Replace(objIPPDetails.VAddr, "<BR>", "<BR>&nbsp;") & "<BR>&nbsp;"
        Me.txtRemarks.Text = objIPPDetails.Remarks
        Me.txtLateReason.Text = objIPPDetails.LateSubmitReason

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

        If Request.QueryString("Frm") = "dashboard" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx")

        ElseIf Request.QueryString("Frm") = "PSDRecv" Then
            lnkBack.NavigateUrl = dDispatcher.direct("IPP", "PSDRecv.aspx")

        ElseIf Request.QueryString("Frm") = "PSDFyFa" Then
            lnkBack.NavigateUrl = dDispatcher.direct("IPP", "PSDFyfa.aspx")
            Me.cmdAccept.Text = "Resubmit"
            'lnkBack.Visible = False
        End If

        cmdReject.Attributes.Add("onclick", "return confirmReject();")

        Dim documentDate = objdb.GetVal("SELECT IFNULL(im_doc_date,'') 'im_doc_date' FROM invoice_mstr WHERE im_invoice_index = '" & Session("InvIdx") & "'")
        Dim createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
        Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")

        'Zulham 13082015
        'Additional condition for company's effective date
        Dim effectiveDate = objdb.GetVal("SELECT IFNULL(ic_gst_eff_date, '') FROM IPP_COMPANY WHERE ic_coy_name = '" & Common.Parse(objIPPDetails.Vendor) & "'")

        If Trim(effectiveDate) <> "" Then
            If CDate(createdDate) >= CDate(_cutoffDate) And CDate(createdDate) >= CDate(effectiveDate) Then
                _exceedCutOffDt = "Yes"
                If lblVendor.Text <> "" And Not documentDate = "" Then
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
        'Zulham 19082015
        If CDate(createdDate) >= CDate(_cutoffDate) Then
            _exceedCutOffDt = "Yes"
        End If

        'Zulham 13072018
        Dim Asset As New PurchaseOrder_Buyer
        If strIsGst = "Yes" Then
            dtgDocDetail.Columns(EnumIPPDet.ippReimbursement).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippGSTAmount).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippInputTax).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippOutputTax).Visible = True
        ElseIf _exceedCutOffDt = "Yes" Then
            dtgDocDetail.Columns(EnumIPPDet.ippReimbursement).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippGSTAmount).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippInputTax).Visible = True
            dtgDocDetail.Columns(EnumIPPDet.ippOutputTax).Visible = True
        Else
            dtgDocDetail.Columns(EnumIPPDet.ippReimbursement).Visible = False
            dtgDocDetail.Columns(EnumIPPDet.ippGSTAmount).Visible = False
            dtgDocDetail.Columns(EnumIPPDet.ippInputTax).Visible = False
            dtgDocDetail.Columns(EnumIPPDet.ippOutputTax).Visible = False
        End If
        dtgDocDetail.Columns(EnumIPPDet.ippCostAlloc).Visible = False 'Hide cost allocation
        'End

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
        Dim ds As DataSet
        Dim dvViewIPP As DataView
        Dim objIPPDet As New IPPDetails

        If Request.QueryString("Frm") = "IPPEnq" Then
            ds = objIPP.IPPEnq_detail(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")), VenIdx)
        Else
            ds = objIPP.ipp_detail(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")), VenIdx, "PSDAcceptanceDetails")
        End If
        Session("PSDIPPDoc") = ds
        dvViewIPP = ds.Tables(0).DefaultView


        If pSorted Then
            dvViewIPP.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewIPP.Sort += " DESC"
        End If

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count 'intPageRecordCnt
        '//bind datagrid

        If ds.Tables(0).Rows.Count > 0 Then
            dtgDocDetail.DataSource = dvViewIPP
            dtgDocDetail.DataBind()
        Else
            dtgDocDetail.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ' *** If either Credit/Debit Note, display Invoice No. column
        If Request.QueryString("Frm") = "IPPEnq" Then
            objIPPDet = objIPP.GetIPPEnqDetails(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")), Session("InvIdx"))
        Else
            objIPPDet = objIPP.GetIPPDetails(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")), Session("InvIdx"), "PSDAcceptanceDetails")
        End If

        'If objIPPDet IsNot Nothing Then
        If objIPPDet.DocType = "CN" Or objIPPDet.DocType = "DN" Then
            Me.dtgDocDetail.Columns(EnumIPPDet.ippInvoiceNo).Visible = True
        Else
            Me.dtgDocDetail.Columns(EnumIPPDet.ippInvoiceNo).Visible = False
        End If
        'End If

        'Zulham 26032015 Case 8603
        'Zulham 13112018
        If (objIPPDet.PaymentMethod = "TT" Or objIPPDet.PaymentMethod = "NOSTRO" Or objIPPDet.PaymentMethod = "BD") And (objIPPDet.Status = "13" Or objIPPDet.Status = "4") Then
            Me.dtgDocDetail.Columns(EnumIPPDet.ippAmt).Visible = True
        ElseIf (objIPPDet.PaymentMethod = "TT" Or objIPPDet.PaymentMethod = "NOSTRO" Or objIPPDet.PaymentMethod = "BD") Then
            Me.dtgDocDetail.Columns(EnumIPPDet.ippAmt).Visible = True
            Me.dtgDocDetail.Columns(EnumIPPDet.ippAmt2).Visible = False
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

    Private Sub dtgDocDetail_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDocDetail.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
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

            If Not IsDBNull(dv("FA_ACTION_DATE")) Then
                e.Item.Cells(2).Text = dv("FA_ACTION_DATE")
            End If
            e.Item.Cells(0).Text = e.Item.Cells(0).Text.Replace("IPP", "E2P") 'Zulham 13072018 - PAMB
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
            ViewState("totalSubAmt") = ViewState("totalSubAmt") + dblAmount

            dblGSTAmount = Common.parseNull(dv("ISD_DOC_GST_VALUE"), 0)
            e.Item.Cells(EnumSubDocDet.subEmpty).Text = Format(dblGSTAmount, "#,##0.00")
            e.Item.Cells(EnumSubDocDet.subEmpty).HorizontalAlign = HorizontalAlign.Right

            ViewState("totalSubAmt") = ViewState("totalSubAmt") + dblGSTAmount

        End If

    End Sub

    Private Sub dtgDocDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDocDetail.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dblAmount As Double
            Dim dblAmount2 As Double
            Dim GST As New GST

            'Zulham 26032015 Case 8603
            'Zulham 13112018
            If (dv("IM_PAYMENT_TERM") = "TT" Or dv("IM_PAYMENT_TERM") = "NOSTRO" Or dv("IM_PAYMENT_TERM") = "BD") And (dv("IM_INVOICE_STATUS") = "13" Or dv("IM_INVOICE_STATUS") = "4") Then
                dblAmount2 = (Common.parseNull(dv("ID_RECEIVED_QTY"), 0) * Common.parseNull(dv("ID_UNIT_COST"), 0)) * Common.parseNull(dv("IM_EXCHANGE_RATE"), 0)
                dblAmount = Common.parseNull(dv("ID_RECEIVED_QTY"), 0) * Common.parseNull(dv("ID_UNIT_COST"), 0)
                e.Item.Cells(EnumIPPDet.ippAmt).Text = Format(dblAmount, "#,##0.00")
                e.Item.Cells(EnumIPPDet.ippAmt2).Text = Format(dblAmount2, "#,##0.00")
                ViewState("total2") = ViewState("total2") + dblAmount2
                ViewState("total") = ViewState("total") + dblAmount
                e.Item.Cells(EnumIPPDet.ippCostAlloc).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("IPP", "CostAllocDetail.aspx", "CostAllocCode=" & dv("ID_COST_ALLOC_CODE")) & "&Amount=" & dblAmount & "&InvIdx=" & dv("IM_INVOICE_INDEX") & "&InvLine=" & e.Item.Cells(EnumIPPDet.ippSNo).Text & "&VenIdx=" & VenIdx & "')"" ><font color=#0000ff>" & Common.parseNull(dv("ID_COST_ALLOC_CODE")) & "</font></A>"
            ElseIf (dv("IM_PAYMENT_TERM") = "TT" Or dv("IM_PAYMENT_TERM") = "NOSTRO" Or dv("IM_PAYMENT_TERM") = "BD") Then
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

            If UCase(Common.parseNull(dv("ID_PAY_FOR"))) = UCase(Session("CompanyId")) Then
                e.Item.Cells(EnumIPPDet.ippPayFor).Text = "Own Company"
            Else
                e.Item.Cells(EnumIPPDet.ippPayFor).Text = Common.parseNull(dv("ID_PAY_FOR"))
                e.Item.Cells(EnumIPPDet.ippPayFor).ToolTip = objdb.GetVal("SELECT ic_coy_name FROM ipp_company WHERE ic_other_b_coy_code = '" & dv("ID_PAY_FOR") & "'")
            End If
            e.Item.Cells(EnumIPPDet.ippAssetGrp).ToolTip = Common.parseNull(dv("ID_ASSET_GROUP_DESC"))
            e.Item.Cells(EnumIPPDet.ippAssetSub).ToolTip = Common.parseNull(dv("ID_ASSET_SUB_GROUP_DESC"))
            e.Item.Cells(EnumIPPDet.ippCostCentre).ToolTip = Common.parseNull(dv("ID_COST_CENTER_DESC"))
            e.Item.Cells(EnumIPPDet.ippGL).Text = Common.parseNull(dv("ID_B_GL_CODE")) & ":" & Common.parseNull(dv("CBG_B_GL_DESC"))
            'e.Item.Cells(EnumIPPDet.ippCostCentre).Text = Common.parseNull(dv("ID_COST_CENTER_2"))

            'Zulham Sept 17, 2014
            Dim ddlInputTax, ddlOutputTax As DropDownList
            Dim lblGSTAmount As Label
            Dim objGlobal As New AgoraLegacy.AppGlobals
            ddlInputTax = e.Item.FindControl("ddlInputTax")
            ddlOutputTax = e.Item.FindControl("ddlOutputTax")
            lblGSTAmount = e.Item.FindControl("lblGSTAmount")

            GST.FillTaxCode_forIPP(ddlInputTax, "", "P")
            GST.FillTaxCode_forIPP(ddlOutputTax, "", "S")

            'Zulham 10072018 - PAMB
            'Some condition for withholding tax
            If Common.parseNull(dv("id_withholding_opt")).ToString = "3" Then
                e.Item.Cells(EnumIPPDet.ippWHT).Text = Common.parseNull(dv("id_withholding_remarks"))
            End If
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

            If Common.parseNull(dv("id_gst_input_tax_code")) <> "" Then
                If Not dv("id_gst_input_tax_code").ToString = "0" Then
                    ddlInputTax.SelectedValue = dv("id_gst_input_tax_code").ToString.Trim
                    If dv("id_gst_input_tax_code").ToString.Trim.Contains("NR") Then
                        ddlInputTax.Enabled = False
                        lblGSTAmount.Enabled = False
                    End If
                Else
                    ddlInputTax.Items.Add(New ListItem("N/A", 0))
                    ddlInputTax.Enabled = False
                    ddlInputTax.SelectedValue = 0
                End If
            Else
                ddlInputTax.Items.Add(New ListItem("N/A", 0))
                ddlInputTax.Enabled = False
                ddlInputTax.SelectedValue = 0
            End If

            'Zulham 18012019
            'If Common.parseNull(dv("id_gst_output_tax_code")) <> "" Then
            '    If Not dv("id_gst_output_tax_code").ToString = "0" And Not dv("id_gst_output_tax_code").ToString = "N/A" Then
            '        ddlOutputTax.SelectedValue = dv("id_gst_output_tax_code").ToString.Trim
            '    Else
            ddlOutputTax.Items.Add(New ListItem("N/A", 0))
            ddlOutputTax.Enabled = False
            ddlOutputTax.SelectedValue = 0
            '    End If
            'Else
            '    ddlOutputTax.Items.Add(New ListItem("N/A", 0))
            '    ddlOutputTax.SelectedValue = 0
            'End If

            lblGSTAmount.Text = Common.parseNull(dv("id_gst_value"), 0.0)
            'If Not lblGSTAmount.Text = "" Then ViewState("gstTotal") = ViewState("gstTotal") + CInt(lblGSTAmount.Text)

            If Common.parseNull(dv("id_gst_input_tax_code")) <> "" Then
                If Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Contains("(") Then
                    'Zulham 13112018
                    'If Not Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Contains("0") Then
                    If Not lblGSTAmount.Text = "" Then ViewState("gstTotal") = ViewState("gstTotal") + CDec(lblGSTAmount.Text)
                    'End If
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
            lblGSTAmount.Enabled = True

            'End
            If IsDBNull(dv("ID_GST_REIMB")) Then
                e.Item.Cells(EnumIPPDet.ippReimbursement).Text = "N/A"
            ElseIf dv("ID_GST_REIMB") = "R" Then
                e.Item.Cells(EnumIPPDet.ippReimbursement).Text = "Reimbursement"
            ElseIf dv("ID_GST_REIMB") = "D" Then
                e.Item.Cells(EnumIPPDet.ippReimbursement).Text = "Disbursement"
                ddlInputTax.Enabled = False
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
        row.Cells(EnumSubDocDet.subGST).BorderStyle = BorderStyle.None

        row.CssClass = "linespacing2"
        row.BorderStyle = BorderStyle.None
        row.BackColor = Drawing.Color.White
        Me.dtgSubDocDetail.Controls(0).Controls.Add(row)

        ViewState("totalSubAmt") = 0

    End Sub

    Sub AddRowtotal() 'add total row 
        Dim intL As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim dgItem As DataGridItem
        Dim gstAmount = 0.0
        For intL = 0 To Me.dtgDocDetail.Columns.Count - 1
            addCell(row)
        Next

        If ViewState("total") = 0 And ViewState("total2") = 0 And gstAmount = 0 Then
            ViewState("total") = 0
            For Each dgItem In dtgDocDetail.Items

                Dim ddlInputTax, ddlOutputTax As DropDownList
                Dim lblGSTAmount As Label
                ddlInputTax = dgItem.FindControl("ddlInputTax")
                ddlOutputTax = dgItem.FindControl("ddlOutputTax")
                lblGSTAmount = dgItem.FindControl("lblGSTAmount")

                If Common.parseNull(ddlInputTax.SelectedItem.Text) <> "" Then
                    If Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Contains("(") Then
                        If Not Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Contains("0") Then
                            If Not lblGSTAmount.Text = "" Then ViewState("gstTotal") += CDec(lblGSTAmount.Text)
                        End If
                    End If
                End If

                If Common.parseNull(ddlInputTax.SelectedItem.Text) <> "" Then
                    If Common.parseNull(ddlOutputTax.SelectedItem.Text).ToString.Contains("(") Then
                        If Not Common.parseNull(ddlOutputTax.SelectedItem.Text).ToString.Contains("0") Then
                            If Not lblGSTAmount.Text = "" Then ViewState("gstTotal") -= CDec(lblGSTAmount.Text)
                        End If
                    End If
                End If
                ViewState("total") += dgItem.Cells(8).Text
                ViewState("total2") += dgItem.Cells(8).Text
            Next
            If Not ViewState("totalSubAmt") = 0 Then ViewState("totalSubwGST") = ViewState("totalSubAmt")
            AddRowtotalSubDoc()
        End If

        If Not ViewState("gstTotal") Is Nothing Then
            If Not ViewState("gstTotal").ToString.Length = 0 Then
                gstAmount = ViewState("gstTotal")
            End If
        End If

        row.Cells(EnumIPPDet.ippUnitPrice).Text = "Total :"
        row.Cells(EnumIPPDet.ippUnitPrice).HorizontalAlign = HorizontalAlign.Center
        row.Cells(EnumIPPDet.ippUnitPrice).Font.Bold = True
        row.Cells(EnumIPPDet.ippUnitPrice).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippAmt).Text = Format(ViewState("total") + gstAmount, "#,##0.00")
        row.Cells(EnumIPPDet.ippAmt).HorizontalAlign = HorizontalAlign.Center
        row.Cells(EnumIPPDet.ippAmt).Font.Bold = True
        row.Cells(EnumIPPDet.ippAmt).Font.Underline = True
        row.Cells(EnumIPPDet.ippAmt).Font.Overline = True
        row.Cells(EnumIPPDet.ippAmt).HorizontalAlign = HorizontalAlign.Right
        row.Cells(EnumIPPDet.ippAmt).BorderColor = Color.Transparent

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
        row.Cells(EnumIPPDet.ippGLRuleCat).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippAssetGrp).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippAssetSub).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippCostAlloc).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippBranch).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippCostCentre).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippReimbursement).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippGSTAmount).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippInputTax).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippOutputTax).BorderColor = Color.Transparent

        'Zulham 14072018 - PAMB
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

    Private Sub cmdAccept_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAccept.Click
        'status 12
        If lblPaymentMethod.Text.Contains("TT") Then lblPaymentMethod.Text = "TT"
        'if Me.txtPRCSSentDate.Value = "", prompt error message
        If Me.txtPRCSSentDate.Value.ToString = "" Then
            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.alert('Please select PSD Received Date to approve!');"
            strJScript += "</script>"
            Response.Write(strJScript)
            Exit Sub
        End If

        'if receivedDt < sentDt, prompt error message
        If CDate(txtPRCSSentDate.Value) < CDate(lblpsdSentDate.Text) Then
            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.alert('PSD Received Date should not be earlier than PSD Sent Date for Document No." & lblDocNo.Text & "');"
            strJScript += "</script>"
            Response.Write(strJScript)
            Exit Sub
        End If

        ''if receivedDt falls on hols, prompt error message
        'Dim dtHols As New DataTable
        'dtHols.Columns.Add("Date")
        Dim objIPP As New IPP
        Dim dsHols As New DataSet
        dsHols = objIPP.GetHolidays
        'dsHols.Tables.Add(dtHols)
        If dsHols.Tables(0).Rows.Count > 0 Then
            For Each item As DataRow In dsHols.Tables(0).Rows
                If String.Format("{0:dd/MM/yyyy}", item("hm_date")) = Me.txtPRCSSentDate.Value Then
                    Dim strJScript As String
                    strJScript = "<script language=javascript>"
                    strJScript += "window.alert('PSD Received Date must be a working day');"
                    strJScript += "</script>"
                    Response.Write(strJScript)
                    Exit Sub
                End If
            Next
        End If

        'if receivedDt < todaysDt, prompt error message
        Dim selectedDt As New DateTime
        selectedDt = txtPRCSSentDate.Value
        If selectedDt > String.Format("{0:dd/MM/yyyy}", Date.Now) Then
            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.alert('PSD Received Date cannot be later than today');"
            strJScript += "</script>"
            Response.Write(strJScript)
            Exit Sub
        End If

        'if selected date fell on weekend
        If selectedDt.DayOfWeek = 6 Or selectedDt.DayOfWeek = 0 Then
            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.alert('PSD Received Date must be a working day');"
            strJScript += "</script>"
            Response.Write(strJScript)
            Exit Sub
        End If

        'check backdate if user set date < today date
        Dim TodayDt As New DateTime
        Dim BackDt As New DateTime
        Dim intDay, i As Integer
        Dim blnCheck, blnValid As Boolean
        TodayDt = String.Format("{0:dd/MM/yyyy}", Date.Now)
        If selectedDt < TodayDt Then
            blnValid = False
            intDay = DateDiff(DateInterval.Day, selectedDt, TodayDt)
            For i = 0 To intDay
                BackDt = TodayDt.AddDays(-(i + 1))
                If dsHols.Tables(0).Rows.Count > 0 Then
                    blnCheck = True
                    For Each item As DataRow In dsHols.Tables(0).Rows
                        If String.Format("{0:dd/MM/yyyy}", item("hm_date")) = BackDt Then
                            blnCheck = False
                        End If
                    Next
                    If blnCheck = True Then
                        blnValid = True
                        Exit For
                    End If
                Else
                    blnValid = True
                    Exit For
                End If
            Next

            If blnValid = False Then
                Dim strJScript As String
                strJScript = "<script language=javascript>"
                strJScript += "window.alert('PSD Received Date must be a working day');"
                strJScript += "</script>"
                Response.Write(strJScript)
                Exit Sub
            Else
                If selectedDt <> BackDt Then
                    Dim strJScript As String
                    strJScript = "<script language=javascript>"
                    strJScript += "window.alert('Document is allowed to be backdated only 1 working day.');"
                    strJScript += "</script>"
                    Response.Write(strJScript)
                    Exit Sub
                End If
            End If
        End If

        'use that pre existing approval method (of ippmain class)
        Dim strRemark, strUrl As String
        'Dim objipp As New IPP
        Dim objippmain As New IPPMain
        Dim strMsg As String

        'Zulham 05/01/2016 - IPP STAGE 4 Phase 2
        'check for invoices status that are tied to CN/DN
        If objippmain.checkStatus(lblDocNo.Text.Trim, VenIdx, Session("Status")) = False Then
            strMsg = "Credit Note/Debit Note cannot precede the invoices it's tied to."
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            BindgridSubDoc()
            AddRowtotalSubDoc()
            Exit Sub
        End If

        'for checking the payment method TT
        strRemark = FormatAORemark("approve")

        strRemark = strRemark & txtApprRejRemark.Text
        Dim objDoc As New IPPMain
        Dim intApprGrpIndex As String
        'Zulham 02012018
        Dim intCurrentStatus As Int16 = 0
        intApprGrpIndex = objdb.GetVal("SELECT DISTINCT FA_APPROVAL_GRP_INDEX FROM finance_approval WHERE FA_INVOICE_INDEX = '" & Session("InvIdx") & "' ")
        'Zulham 02012018
        intCurrentStatus = objdb.GetVal("SELECT DISTINCT IM_INVOICE_STATUS FROM INVOICE_MSTR WHERE IM_INVOICE_INDEX = '" & Session("InvIdx") & "' ")

        If objDoc.checkDept() = False Then
            objIPP.Message(Me, "00046", MsgBoxStyle.Information)
            Exit Sub
        End If
        If lblPaymentMethod.Text = "TT" And ViewState("role") = 3 Then
            If objDoc.ApproveIPPDoc(Session("InvIdx"), strRemark, Session("Relief"), "FinanceTeller", lblPaymentMethod.Text) = True Then
                'strMsg = objGlobal.GetErrorMessage("00043") & " " & Session("DocNo") & " " & objGlobal.GetErrorMessage("00044")
                strMsg = objGlobal.GetErrorMessage("00360")
                Common.NetMsgbox(Me, strMsg, lnkBack.NavigateUrl, MsgBoxStyle.Exclamation)
            Else
                'strMsg = objGlobal.GetErrorMessage("00007")
                strMsg = objGlobal.GetErrorMessage("00361")
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            End If
        Else
            If objDoc.CheckBankCode(Session("InvIdx")) Then
                strMsg = objGlobal.GetErrorMessage("00323")
                Common.NetMsgbox(Me, strMsg, lnkBack.NavigateUrl, MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            If Not Me.cmdAccept.Text = "Resubmit" Then
                'Zulham 02012018
                If objDoc.ApproveIPPDoc(Session("InvIdx"), strRemark, Session("Relief"), "FinanceTeller", lblPaymentMethod.Text, currentStatus:=intCurrentStatus) = True Then

                    'Zulham 03122018
                    objDoc.sendMailToIPPOfficer(Session("InvIdx"), "5", "submit", from:="PSD Received")

                    If objDoc.isHighestLevel(Session("InvIdx")) Then
                        'strMsg = objGlobal.GetErrorMessage("00043") & " " & Session("DocNo") & " " & objGlobal.GetErrorMessage("00044")
                        strMsg = objGlobal.GetErrorMessage("00360")
                    Else
                        'strMsg = objGlobal.GetErrorMessage("00043") & " " & Session("DocNo") & " " & objGlobal.GetErrorMessage("00051")
                        strMsg = objGlobal.GetErrorMessage("00360")
                    End If
                    Common.NetMsgbox(Me, strMsg, lnkBack.NavigateUrl, MsgBoxStyle.Exclamation)
                Else
                    'strMsg = objGlobal.GetErrorMessage("00007")
                    strMsg = objGlobal.GetErrorMessage("00361")
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                End If
            Else
                objIPP.ApproveResubmittedDoc(Session("InvIdx"), strRemark, Session("Relief"), "FinanceTeller", lblPaymentMethod.Text)

                'Zulham 03122018
                objDoc.sendMailToIPPOfficer(Session("InvIdx"), "5", "submit", from:="PSD Received")

            End If
        End If

        'update recv date
        If objIPP.UpdatePRCSRecvDate(Session("InvIdx"), Common.ConvertDate(Me.txtPRCSSentDate.Value)) = True _
        And objIPP.AddAuditTrailRecord(Session("InvIdx"), "Finance Teller", "Enter PSD Received Date") = True Then
            If Me.cmdAccept.Text = "Resubmit" Then
                strMsg = objGlobal.GetErrorMessage("00362")
                Common.NetMsgbox(Me, strMsg, lnkBack.NavigateUrl, MsgBoxStyle.Exclamation)
            End If

        End If

        'add data into trans_log


        cmdReject.Visible = False
        cmdAccept.Visible = False
    End Sub
    Private Sub cmdReject_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReject.Click
        'status 14
        Dim objDoc As New IPPMain
        Dim objipp As New IPP
        Dim strRemark As String
        Dim strMsg As String
        If objDoc.checkDept() = False Then
            objipp.Message(Me, "00046", MsgBoxStyle.Information)
            Exit Sub
        End If

        'Zulham 16/02/2016 - IPP Stage 4 Phase 2
        'Invoices tied to any CN/DN cant be rejected
        Dim boolCantReject As Boolean = False
        Dim dsRefNo As New DataSet
        dsRefNo = objDoc.getCNDNRefNo(Session("DocNo"), ViewState("VenIdx"))
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


        strRemark = FormatAORemark("reject")
        strRemark = strRemark & txtApprRejRemark.Text
        If txtApprRejRemark.Text = "" Then
            Common.NetMsgbox(Me, "Please enter Remarks.", MsgBoxStyle.Information)
        Else
            If objDoc.RejectIPPDoc(Session("InvIdx"), strRemark, Session("Relief"), "FinanceTeller") = True Then
                strMsg = objGlobal.GetErrorMessage("00043") & " " & Session("DocNo") & " " & objGlobal.GetErrorMessage("00045")
                Common.NetMsgbox(Me, strMsg, lnkBack.NavigateUrl, MsgBoxStyle.Information)
            Else
                objipp.Message(Me, "00007", MsgBoxStyle.Information)
            End If
            cmdReject.Visible = False
            cmdAccept.Visible = False
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

        Select Case strAction
            Case "approve"
                strRemark = "Approved " & strTempRemark & ": "
            Case "reject"
                strRemark = "Rejected" & strTempRemark & ": "
        End Select

        Return strRemark
    End Function
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        If Me.Request.QueryString("Frm") = "PSDRecv" Then
            Session("w_IPP_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPP", "PSDRECV.aspx", "mode=new&type=Listing&pageid=" & strPageId) & """><span>PSD Acceptance List</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "PSDAcceptReject.aspx", "mode=new&type=Listing&pageid=" & strPageId) & """><span>PSD Accepted/Rejected Listing</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "PSDFyfa.aspx", "type=Listing&mode=new&pageid=" & strPageId) & """><span>Pending FYFA</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "</ul><div></div></div>"
        ElseIf Me.Request.QueryString("Frm") = "PSDFyFa" Then
            Session("w_IPP_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "PSDRECV.aspx", "mode=new&type=Listing&pageid=" & strPageId) & """><span>PSD Acceptance List</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "PSDAcceptReject.aspx", "mode=new&type=Listing&pageid=" & strPageId) & """><span>PSD Accepted/Rejected Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPP", "PSDFyfa.aspx", "type=Listing&mode=new&pageid=" & strPageId) & """><span>Pending FYFA</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
        Else

            'Jan 15, 2014 - Get invoice status
            Dim _invstatus = objdb.GetVal("SELECT im_invoice_status FROM invoice_mstr WHERE im_invoice_index = '" & Session("InvIdx") & "'")
            'End

            Select Case _invstatus.ToString
                Case "17"
                    Session("w_IPP_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPP", "PSDRECV.aspx", "mode=new&type=Listing&pageid=" & strPageId) & """><span>PSD Acceptance List</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "PSDAcceptReject.aspx", "mode=new&type=Listing&pageid=" & strPageId) & """><span>PSD Accepted/Rejected Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "PSDFyfa.aspx", "type=Listing&mode=new&pageid=" & strPageId) & """><span>Pending FYFA</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
                Case "14"
                    Session("w_IPP_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "PSDRECV.aspx", "mode=new&type=Listing&pageid=" & strPageId) & """><span>PSD Acceptance List</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "PSDAcceptReject.aspx", "mode=new&type=Listing&pageid=" & strPageId) & """><span>PSD Accepted/Rejected Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPP", "PSDFyfa.aspx", "type=Listing&mode=new&pageid=" & strPageId) & """><span>Pending FYFA</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
                Case Else
                    Session("w_IPP_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "PSDRECV.aspx", "mode=new&type=Listing&pageid=" & strPageId) & """><span>PSD Acceptance List</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPP", "PSDAcceptReject.aspx", "mode=new&type=Listing&pageid=" & strPageId) & """><span>PSD Accepted/Rejected Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "PSDFyfa.aspx", "type=Listing&mode=new&pageid=" & strPageId) & """><span>Pending FYFA</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
            End Select
        End If
        
    End Sub

    Public Function ValidateDate(Optional ByVal SentDt As String = "") As String
        Dim msg As String = ""
        'selected date must be later than current date
        Dim selectedDt As New DateTime
        selectedDt = txtPRCSSentDate.Value

        If selectedDt > String.Format("{0:dd/MM/yyyy}", Date.Now) Then
            Return "1"
            Exit Function
        End If

        If selectedDt < String.Format("{0:dd/MM/yyyy}", SentDt) Then
            Return "2"
            Exit Function
        End If

        Dim dtHols As New DataTable
        dtHols.Columns.Add("Date")
        Dim objIPP As New IPP
        Dim dsHols As DataSet : dsHols = objIPP.GetHolidays
        dsHols.Tables.Add(dtHols)
        If dtHols.Rows.Count > 0 Then
            For Each item As DataRow In dtHols.Rows
                If String.Format("{0:dd/MM/yyyy}", item("Date")) = Me.txtPRCSSentDate.Value Then
                    Return "3"
                End If
            Next
        End If

        'if selected date fell on weekend
        If selectedDt.DayOfWeek = 6 Or selectedDt.DayOfWeek = 0 Then
            Return "4"
        End If

    End Function

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