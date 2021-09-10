'Zulham 30-01-2015 IPP-GST Stage 2A
Imports AgoraLegacy
Imports eProcure.Component
Imports System.drawing
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Partial Public Class BillingDocument
    Inherits AgoraLegacy.AppBaseClass
    Dim dblInvoiceAmount, prevAppType As String
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objdb As New EAD.DBCom
    Dim VenIdx As Integer = 0
    Dim _exceedCutOffDt As String = ""
    Dim strIsGst As String = ""

    Public Enum EnumSubDocDet
        subDocNo = 0
        subDocDate = 1
        subDocAmt = 2
        subEmpty = 3
    End Enum

    Public Enum EnumIPPDet
        ippSNo = 0
        'Zulham 28/07/2017 - IPP Stage 3
        ippRefNo = 1
        ''''
        ippDesc = 2 '1
        ippUOM = 3 '2
        ippCurrency = 4 '3
        ippQty = 5 '4
        ippUnitPrice = 6 '5
        ippAmt = 7 '6
        ippAmt2 = 8 '7
        ippGSTAmount = 10 '9
        'Zulham 13/01/2016 - IPP Stage 4 Phase 2
        ippForeignGSTAmount = 9 ' 8
        ippInputTax = 11 '10 '9
        ippOutputTax = 12 '11 '10
        ippGL = 13 '12 '11
        ippGLRuleCat = 14 '13 '12
        ippBranch = 15 '14 '13
        ippCostCentre = 16 '15 '14
        ippExchangeRate = 17 '16 '15
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objBilling As New Billing
        Dim objBillingDetails As New BillingDetails
        Dim strFrm As String
        Dim createdDate = ""
        Dim _cutoffDate = ""
        Dim dsHeader As New DataSet
        Dim strAddress = ""

        strFrm = Me.Request.QueryString("Frm")
        Session("DocNo") = Request.QueryString("DocumentNo")
        Session("InvIdx") = Request.QueryString("index")

        VenIdx = objdb.GetVal("SELECT bm_s_coy_id FROM billing_mstr WHERE bm_invoice_index = '" & Session("InvIdx") & "'")

        'blnPaging = False
        blnSorting = False

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgApprvFlow, "N")
        SetGridProperty(dtgDocDetail, "N")
        ViewState("total") = 0
        ViewState("total2") = 0
        ViewState("gstTotal") = 0

        dsHeader = objBilling.GetBillingDoc(Session("InvIdx")) ', Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")))
        'End If

        If dsHeader IsNot Nothing Then
            If Not dsHeader.Tables(0).Rows.Count = 0 Then
                'Michelle (12/5/2014) - Prevent show approval flow (conversion error) when status is 'Void'
                If dsHeader.Tables(0).Rows(0).Item("bm_invoice_Status").ToString <> 5 Then renderBillingApprFlow()

                Select Case dsHeader.Tables(0).Rows(0).Item("bm_invoice_type").ToString
                    Case "INV"
                        Me.lblDocType.Text = "Invoice"
                    Case "NON"
                        Me.lblDocType.Text = "Non Invoice"
                        'Zulham 27/07/2017 - IPP Stage 3
                    Case "CN"
                        Me.lblDocType.Text = "Credit Note"
                    Case "DN"
                        Me.lblDocType.Text = "Debit Note"
                    Case "CA"
                        Me.lblDocType.Text = "Credit Advice"
                    Case "DA"
                        Me.lblDocType.Text = "Debit Advice"
                    Case "CNN"
                        Me.lblDocType.Text = "Credit Note(Non-Invoice)"
                    Case "DNN"
                        Me.lblDocType.Text = "Debit Note(Non-Invoice)"
                        ''''
                End Select

                Me.lblDocNo.Text = dsHeader.Tables(0).Rows(0).Item("bm_invoice_no").ToString 'objBillingDetails.DocNo
                Select Case dsHeader.Tables(0).Rows(0).Item("bm_invoice_status").ToString
                    Case 1
                        Me.lblStatus.Text = "Draft"
                    Case 2
                        Me.lblStatus.Text = "Submitted"
                    Case 3
                        Me.lblStatus.Text = "Approved"
                    Case 4
                        Me.lblStatus.Text = "Rejected"
                    Case 5
                        Me.lblStatus.Text = "Void"
                    Case 6
                        Me.lblStatus.Text = "Billed"
                End Select

                If dsHeader.Tables(0).Rows(0).Item("bm_currency_code").ToString.Trim = "" Or _
                dsHeader.Tables(0).Rows(0).Item("bm_currency_code").ToString.Trim = "MYR" Then
                    Me.lblCurrency.Text = "MYR"
                Else
                    Me.lblCurrency.Text = dsHeader.Tables(0).Rows(0).Item("bm_currency_code").ToString
                    TRExchangeRate.Visible = True
                    If Not dsHeader.Tables(0).Rows(0).Item("bm_exchange_rate") Is DBNull.Value Then
                        txtExchangeRate.Text = Format(dsHeader.Tables(0).Rows(0).Item("bm_exchange_rate"), "#,###,0.0000")
                    Else
                        txtExchangeRate.Text = Format(0, "#,###,0.0000")
                    End If
                End If

                'Zulham 18/08/2017 - IPP Stage 3
                If Not dsHeader.Tables(0).Rows(0).Item("bm_remarks1") Is Nothing And Not dsHeader.Tables(0).Rows(0).Item("bm_remarks1") Is DBNull.Value Then
                    If Not Trim(dsHeader.Tables(0).Rows(0).Item("bm_remarks1")) = "" Then
                        If Trim(dsHeader.Tables(0).Rows(0).Item("bm_remarks1")) = "IPP" Then
                            'get sum from billing_details
                            Dim paymentAmt = objdb.GetVal("SELECT SUM(bm_received_qty * bm_unit_cost + bm_gst_value) AS 'payment_amt' FROM billing_details WHERE bm_invoice_no = '" & dsHeader.Tables(0).Rows(0)("bM_INVOICE_no") & "'")
                            Me.lblPaymentAmt.Text = paymentAmt
                            Me.lblPaymentAmt.Text = Format(CDec(paymentAmt), "#,###,0.00")
                            Dim paymentAmtNoGST = objdb.GetVal("SELECT SUM(bm_received_qty * bm_unit_cost ) AS 'payment_amt' FROM billing_details WHERE bm_invoice_no = '" & dsHeader.Tables(0).Rows(0)("bM_INVOICE_no") & "'")
                            lblPaymentAmtwthGST.Text = paymentAmtNoGST
                            Me.lblPaymentAmtwthGST.Text = Format(CDec(paymentAmtNoGST), "#,###,0.00")
                        End If
                    Else
                        Me.lblPaymentAmt.Text = dsHeader.Tables(0).Rows(0).Item("bm_invoice_total").ToString
                        Me.lblPaymentAmt.Text = Format(dsHeader.Tables(0).Rows(0)("bM_INVOICE_TOTAL"), "#,###,0.00")
                        If Not dsHeader.Tables(0).Rows(0)("bM_INVOICE_WTH_TOTAL") Is DBNull.Value Then
                            Me.lblPaymentAmtwthGST.Text = Format(dsHeader.Tables(0).Rows(0)("bM_INVOICE_WTH_TOTAL"), "#,###,0.00")
                        Else
                            Me.lblPaymentAmtwthGST.Text = "0.00"
                        End If
                    End If
                Else
                    Me.lblPaymentAmt.Text = dsHeader.Tables(0).Rows(0).Item("bm_invoice_total").ToString
                    Me.lblPaymentAmt.Text = Format(dsHeader.Tables(0).Rows(0)("bM_INVOICE_TOTAL"), "#,###,0.00")
                    If Not dsHeader.Tables(0).Rows(0)("bM_INVOICE_WTH_TOTAL") Is DBNull.Value Then
                        Me.lblPaymentAmtwthGST.Text = Format(dsHeader.Tables(0).Rows(0)("bM_INVOICE_WTH_TOTAL"), "#,###,0.00")
                    Else
                        Me.lblPaymentAmtwthGST.Text = "0.00"
                    End If
                End If


                Dim strGSTCode = ""
                Dim GST As New GST
                createdDate = dsHeader.Tables(0).Rows(0).Item("bm_created_on")
                _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
                If CDate(createdDate) > CDate(_cutoffDate) Then strGSTCode = GST.chkGST_ForIPP(ViewState("venidx"))

                If Not Common.parseNull(strGSTCode) = "" Then
                    Me.lblVendor.Text = dsHeader.Tables(0).Rows(0)("bM_S_COY_NAME") & "(" & strGSTCode & ")"
                Else
                    Me.lblVendor.Text = dsHeader.Tables(0).Rows(0)("bM_S_COY_NAME")
                End If

                Dim objGlobal As New AppGlobals
                strAddress = dsHeader.Tables(0).Rows(0)("bM_ADDR_LINE1") & "<BR>&nbsp;"
                If Not dsHeader.Tables(0).Rows(0)("bM_ADDR_LINE2").ToString.Trim = "" Then strAddress += dsHeader.Tables(0).Rows(0)("bM_ADDR_LINE2") & "<BR>&nbsp;"
                If Not dsHeader.Tables(0).Rows(0)("bM_ADDR_LINE3").ToString.Trim = "" Then strAddress += dsHeader.Tables(0).Rows(0)("bM_ADDR_LINE3") & "<BR>&nbsp;"
                If Not dsHeader.Tables(0).Rows(0)("bM_POSTCODE").ToString.Trim = "" Then strAddress += dsHeader.Tables(0).Rows(0)("bM_POSTCODE") & "<BR>&nbsp;"
                If Not dsHeader.Tables(0).Rows(0)("bM_CITY").ToString.Trim = "" Then strAddress += dsHeader.Tables(0).Rows(0)("bM_CITY") & "<BR>&nbsp;"
                If dsHeader.Tables(0).Rows(0)("bM_STATE").ToString.Trim = "" Or dsHeader.Tables(0).Rows(0)("bM_STATE").ToString.Trim = "n.a." Then
                Else
                    strAddress += objGlobal.getCodeDesc(CodeTable.State, dsHeader.Tables(0).Rows(0)("bM_STATE")) & "<BR>&nbsp;"
                End If
                'If Not dsHeader.Tables(0).Rows(0)("bM_STATE").ToString.Trim = "" Or Not dsHeader.Tables(0).Rows(0)("bM_STATE").ToString.Trim = "n.a." Then strAddress += objGlobal.getCodeDesc(CodeTable.State, dsHeader.Tables(0).Rows(0)("bM_STATE")) & "<BR>&nbsp;"
                If Not dsHeader.Tables(0).Rows(0)("bM_COUNTRY").ToString.Trim = "" Then strAddress += objGlobal.getCodeDesc(CodeTable.Country, dsHeader.Tables(0).Rows(0)("bM_COUNTRY"))

                Me.lblVendorAddr.Text = strAddress

                Me.txtRemarks.Text = dsHeader.Tables(0).Rows(0)("BM_REMARK")

            End If
        End If

        Bindgrid()

        If ViewState("intPageRecordCnt") > 0 Then
            AddRowtotal()
        End If
        ViewState("line") = 0

        objBilling = Nothing
        objBillingDetails = Nothing

        If Request.QueryString("Frm") = "Dashboard" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx")
        ElseIf Request.QueryString("Frm") = "BillingList" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Billing", "BillingList.aspx")
        ElseIf Request.QueryString("Frm") = "BillingEnq" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Billing", "BillingEnquiry.aspx")
            'Zulham 18/08/2017 - IPP Stage 3
        ElseIf Request.QueryString("Frm") = "IPPEnq" Then
            lnkBack.NavigateUrl = dDispatcher.direct("IPP", "IPPEnq.aspx")
            ''''
        Else
            lnkBack.Visible = False
        End If

        If Request.QueryString("Frm") = "BillingEnq" Then
            Session("w_AddPO_tabs") = Nothing
        End If


        'Check for GST
        Dim documentDate = objdb.GetVal("SELECT IFNULL(bm_created_on,'') 'bm_created_on' FROM billing_mstr WHERE bm_invoice_index = '" & Session("InvIdx") & "'")
        createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
        _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")

        If CDate(createdDate) >= CDate(_cutoffDate) Then
            _exceedCutOffDt = "Yes"
            If lblVendor.Text <> "" Then
                Dim GSTRegNo = objdb.GetVal("SELECT IFNULL(IC_TAX_REG_NO, '') FROM IPP_COMPANY WHERE ic_coy_name = '" & Me.lblVendor.Text & "'")
                If GSTRegNo <> "" And CDate(documentDate) >= CDate(_cutoffDate) Then
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
        Dim Asset As New PurchaseOrder_Buyer
        If strIsGst = "Yes" Then
            dtgDocDetail.Columns(2).Visible = True
            dtgDocDetail.Columns(11).Visible = True
            dtgDocDetail.Columns(12).Visible = True
            dtgDocDetail.Columns(13).Visible = True
        ElseIf _exceedCutOffDt = "Yes" Then
            dtgDocDetail.Columns(2).Visible = True
            dtgDocDetail.Columns(11).Visible = True
            dtgDocDetail.Columns(12).Visible = True
            dtgDocDetail.Columns(13).Visible = True
        Else
            dtgDocDetail.Columns(2).Visible = False
            dtgDocDetail.Columns(11).Visible = False
            dtgDocDetail.Columns(12).Visible = False
            dtgDocDetail.Columns(13).Visible = False
        End If

        'zulham 28/07/2017 - IPP Stage 3
        'dtgDocDetail.Columns(3).Visible = False
        dtgDocDetail.Columns(EnumIPPDet.ippCurrency).Visible = False
        ''Zulham 16/05/2016 - IM5/IM6 Enhancement
        ''COlumn index changed from 15 to 16
        'dtgDocDetail.Columns(16).Visible = False
        dtgDocDetail.Columns(EnumIPPDet.ippExchangeRate).Visible = False
        ''End

        'Zulham IPP Stage 3 - 20/07/2017
        Dim docType = objdb.GetVal("SELECT trim(bm_invoice_type) FROM billing_mstr WHERE bm_invoice_index = '" & Session("InvIdx") & "'")
        If docType = "CN" Or docType = "DN" Then
            Me.dtgDocDetail.Columns(EnumIPPDet.ippRefNo).Visible = True
        Else
            Me.dtgDocDetail.Columns(EnumIPPDet.ippRefNo).Visible = False
        End If

    End Sub

    Private Sub renderBillingApprFlow()
        Dim objDoc As New Billing

        Dim ds As DataSet = objDoc.getbillingApprFlow(Session("InvIdx"), Common.Parse(HttpContext.Current.Session("CompanyID")))

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
        Dim objBilling As New Billing
        Dim ds As DataSet
        Dim dvViewIPP As DataView
        Dim objBillingDet As New BillingDetails

        'Zulham 18/08/2017 - IPP Stage 3
        'If Request.QueryString("Frm") = "IPPEnq" Then
        '    'ds = objBilling.IPPEnq_detail(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")), VenIdx)
        'ElseIf Request.QueryString("Frm") = "PSDAcceptRejList" Or Request.QueryString("Frm") = "PSDAcceptRejList,dashboard" Or Request.QueryString("Frm") = "EnterBC" Then
        '    'ds = objBilling.ipp_detail(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")), VenIdx, Request.QueryString("Frm"))
        'ElseIf Request.QueryString("Frm") = "PSDSent" Then
        '    'ds = objBilling.ipp_detail(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")), VenIdx, , Request.QueryString("CreatedBy"))
        'Else
        ds = objBilling.billing_detail(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")), VenIdx)
        'End If
        ''''

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

        objBilling = Nothing

    End Function
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

            'e.Item.Cells(3).Text = "Approval"

            ''Michelle (3/10/2007) - To set the Approval Type as 'N/A' for those FOs with approval limit > Invoice amt
            ''                       For 'FM', the Approval Type is 'Approval'. 
            'If dv("FA_AGA_TYPE") = "FO" And (dblInvoiceAmount < Common.parseNull(dv("AO_LIMIT"), 0)) Then
            '    e.Item.Cells(3).Text = "N/A"
            'End If
            'If e.Item.Cells(3).Text = "N/A" And prevAppType = "Approval" Then
            '    e.Item.Cells(3).Text = "Approval"
            '    prevAppType = "Already Set"
            'End If
            'Dim strAAName, strAAName2, strAAName3, strAAName4 As String
            'Dim strBold As String = "<strong>"
            'Dim strBold2 As String = "</strong>"

            'If Common.parseNull(dv("AAO_NAME")) = "" Then
            '    strAAName = ""
            'Else
            '    strAAName = Common.parseNull(dv("AAO_NAME")) & "<br>"
            'End If


            'If Common.parseNull(dv("AAO_NAME2")) = "" Then
            '    strAAName2 = ""
            'Else
            '    strAAName2 = Common.parseNull(dv("AAO_NAME2")) & "<br>"
            'End If


            'If Common.parseNull(dv("AAO_NAME3")) = "" Then
            '    strAAName3 = ""
            'Else
            '    strAAName3 = Common.parseNull(dv("AAO_NAME3")) & "<br>"
            'End If


            'If Common.parseNull(dv("AAO_NAME4")) = "" Then
            '    strAAName4 = ""
            'Else
            '    strAAName4 = Common.parseNull(dv("AAO_NAME4")) & "<br>"
            'End If

            'If IsDBNull(dv("AAO_NAME")) And IsDBNull(dv("AAO_NAME2")) And IsDBNull(dv("AAO_NAME3")) And IsDBNull(dv("AAO_NAME4")) Then
            '    e.Item.Cells(2).Text = "-"
            'End If

            'If UCase(Common.parseNull(dv("FA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("FA_AO"))) Then
            '    e.Item.Cells(1).Font.Bold = True
            'ElseIf UCase(Common.parseNull(dv("FA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("FA_A_AO"))) Then
            '    'e.Item.Cells(2).Font.Bold = True
            '    strAAName = strBold & strAAName & strBold2
            'ElseIf UCase(Common.parseNull(dv("FA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("FA_A_AO_2"))) Then
            '    strAAName2 = strBold & strAAName2 & strBold2
            'ElseIf UCase(Common.parseNull(dv("FA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("FA_A_AO_3"))) Then
            '    strAAName3 = strBold & strAAName3 & strBold2
            'ElseIf UCase(Common.parseNull(dv("FA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("FA_A_AO_4"))) Then
            '    strAAName4 = strBold & strAAName4 & strBold2
            'End If

            'e.Item.Cells(2).Text = strAAName & _
            '                               strAAName2 & _
            '                               strAAName3 & _
            '                               strAAName4

            'If dv("FA_Seq") - 1 = dv("FA_AO_Action") Then
            '    intTotalCell = e.Item.Cells.Count - 1

            'If Request.QueryString("status") = invStatus.Hold Then
            '    e.Item.Cells(3).Text = "Hold"
            'End If

            'For intLoop = 0 To intTotalCell
            '    e.Item.Cells(intLoop).Font.Bold = True
            'Next

            'ViewState("CurrentAppSeq") = dv("FA_Seq")
            'ViewState("ApprType") = dv("FA_APPROVAL_TYPE")

            'If dv("FA_AGA_TYPE") = "FO" Then
            '    'If UCase(dv("FA_AO")) = UCase(Request.QueryString("AO")) Then
            '    If UCase(dv("FA_AO")) = UCase(Session("UserId")) Then
            '        ViewState("ApprLimit") = Common.parseNull(dv("AO_LIMIT"), 0)
            '    Else
            '        If Not IsDBNull(dv("FA_A_AO")) Then
            '            ViewState("ApprLimit") = Common.parseNull(dv("AAO_LIMIT"), 0)
            '        Else
            '            ViewState("ApprLimit") = 0
            '        End If
            '    End If
            'End If

            'If Not IsPostBack Then txtRemark.Text = Common.parseNull(dv("FA_AO_REMARK"), "")
            'End If

            'ViewState("HighestAppr") = dv("FA_Seq")

            'If IsDBNull(dv("AAO_NAME")) Then
            '    e.Item.Cells(2).Text = "-"
            'End If

            'If UCase(Common.parseNull(dv("FA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("FA_AO"))) Then
            '    e.Item.Cells(1).Font.Bold = True
            'ElseIf UCase(Common.parseNull(dv("FA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("FA_A_AO"))) Then
            '    e.Item.Cells(2).Font.Bold = True
            'End If


            If Not IsDBNull(dv("FA_ACTION_DATE")) Then
                'e.Item.Cells(2).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("FA_ACTION_DATE"))
                e.Item.Cells(2).Text = dv("FA_ACTION_DATE")

                'Else
                '    If dv("FA_Seq") <= dv("FA_AO_Action") Then
                '        e.Item.Cells(3).Text = "N/A"
                '    End If
            End If

            'If prevAppType <> "Already Set" Then
            '    prevAppType = e.Item.Cells(3).Text
            'End If

        End If
    End Sub

    Private Sub dtgDocDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDocDetail.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dblAmount As Double
            Dim dblAmount2 As Double
            Dim GST As New GST


            dblAmount2 = Common.parseNull(dv("BM_RECEIVED_QTY"), 0) * Common.parseNull(dv("BM_UNIT_COST"), 0)
            ViewState("strIsNostro") = objdb.GetVal("SELECT IFNULL(IC_nostro_flag, '') 'IC_nostro_flag' FROM IPP_COMPANY WHERE ic_index = '" & Common.parseNull(dv("BM_S_COY_ID"), 0) & "'")

            If Not lblCurrency.Text = "MYR" And TRExchangeRate.Visible = True Then
                If Not txtExchangeRate.Text.Trim = "" Then
                    e.Item.Cells(EnumIPPDet.ippAmt2).Text = Format(dblAmount2 * Me.txtExchangeRate.Text, "#,##0.00")
                End If
            End If

            If ViewState("strIsNostro") <> "Y" Then
                If Not lblCurrency.Text = "MYR" And TRExchangeRate.Visible = True Then
                    e.Item.Cells(EnumIPPDet.ippAmt).Text = Format(dblAmount2, "#,##0.00")
                    ViewState("total2") = ViewState("total2") + dblAmount2
                Else
                    e.Item.Cells(EnumIPPDet.ippAmt2).Text = Format(dblAmount2, "#,##0.00")
                    ViewState("total2") = ViewState("total2") + dblAmount2
                End If
            Else
                If Not lblCurrency.Text = "MYR" And TRExchangeRate.Visible = True Then
                    e.Item.Cells(EnumIPPDet.ippAmt).Text = Format(dblAmount2, "#,##0.00")
                    ViewState("total2") = ViewState("total2") + dblAmount2
                Else
                    e.Item.Cells(EnumIPPDet.ippAmt2).Text = Format(dblAmount2, "#,##0.00")
                    ViewState("total2") = ViewState("total2") + dblAmount2
                End If
                'e.Item.Cells(EnumIPPDet.ippAmt).Text = Format(dblAmount2, "#,##0.00")
                'ViewState("total2") = ViewState("total2") + dblAmount2
            End If


            e.Item.Cells(EnumIPPDet.ippUnitPrice).Text = Format(dv("BM_UNIT_COST"), "#,##0.00")


            e.Item.Cells(EnumIPPDet.ippCostCentre).ToolTip = Common.parseNull(dv("BM_COST_CENTER_DESC"))
            e.Item.Cells(EnumIPPDet.ippGL).Text = Common.parseNull(dv("BM_B_GL_CODE")) & ":" & Common.parseNull(dv("CBG_B_GL_DESC"))

            'Zulham Sept 17, 2014
            Dim ddlInputTax, ddlOutputTax As DropDownList
            Dim lblGSTAmount, lblForeignGSTAmount As Label 'Zulham 14/01/2016 - IPP Stage 4 Phase 2
            Dim objGlobal As New AgoraLegacy.AppGlobals
            ddlInputTax = e.Item.FindControl("ddlInputTax")
            ddlOutputTax = e.Item.FindControl("ddlOutputTax")
            lblGSTAmount = e.Item.FindControl("lblGSTAmount")
            lblForeignGSTAmount = e.Item.FindControl("lblForeignGSTAmount") 'Zulham 14/01/2016 - IPP Stage 4 Phase 2
            GST.FillTaxCode_forIPP(ddlInputTax, "", "P")
            GST.FillTaxCode_forIPP(ddlOutputTax, "", "S")

            If Common.parseNull(dv("BM_gst_input_tax_code")) <> "" Then
                If Not dv("BM_gst_input_tax_code").ToString = "0" Then
                    ddlInputTax.SelectedValue = dv("BM_gst_input_tax_code").ToString.Trim
                    If dv("BM_gst_input_tax_code").ToString.Trim.Contains("NR") Then
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
            If Common.parseNull(dv("BM_gst_output_tax_code")) <> "" Then
                If Not dv("BM_gst_output_tax_code").ToString = "0" And Not dv("BM_gst_output_tax_code").ToString = "N/A" Then
                    ddlOutputTax.SelectedValue = dv("BM_gst_output_tax_code").ToString.Trim
                Else
                    ddlOutputTax.Items.Add(New ListItem("N/A", 0))
                    ddlOutputTax.Enabled = False
                    ddlOutputTax.SelectedValue = 0
                End If
            Else
                ddlOutputTax.Items.Add(New ListItem("N/A", 0))
                ddlOutputTax.SelectedValue = 0
            End If

            lblGSTAmount.Text = Common.parseNull(dv("BM_gst_value"), 0.0)

            'zulham 14/01/2016 - IPP Stage 4 Phase 2
            If Not txtExchangeRate.Text.Trim.Length = 0 Then
                lblGSTAmount.Text = Format(Common.parseNull(dv("BM_gst_value") * txtExchangeRate.Text, 0.0), "#,##0.00")
                lblForeignGSTAmount.Text = Format(Common.parseNull(dv("BM_GST_VALUE"), 0.0), "#,##0.00")
            End If


            'If Not lblGSTAmount.Text = "" Then ViewState("gstTotal") = ViewState("gstTotal") + CInt(lblGSTAmount.Text)

            'If Common.parseNull(dv("BM_gst_input_tax_code")) <> "" Then
            '    If Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Contains("(") Then
            '        If Not Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Contains("0") Then
            '            If Not lblGSTAmount.Text = "" Then ViewState("gstTotal") = ViewState("gstTotal") + CDec(lblGSTAmount.Text)
            '        End If
            '    End If
            'End If

            If Common.parseNull(dv("BM_gst_output_tax_code")) <> "" Then
                If Common.parseNull(ddlOutputTax.SelectedItem.Text).ToString.Contains("(") Then
                    If Not Common.parseNull(ddlOutputTax.SelectedItem.Text).ToString.Contains("0") Then
                        If Not lblCurrency.Text = "MYR" And TRExchangeRate.Visible = True Then
                            If Not txtExchangeRate.Text.Trim = "" Then
                                If Not lblForeignGSTAmount.Text = "" Then ViewState("gstTotal") = ViewState("gstTotal") + CDec(lblForeignGSTAmount.Text)
                            Else
                                If Not lblGSTAmount.Text = "" Then ViewState("gstTotal") = ViewState("gstTotal") + CDec(lblGSTAmount.Text)
                            End If
                        Else
                            If Not lblGSTAmount.Text = "" Then ViewState("gstTotal") = ViewState("gstTotal") + CDec(lblGSTAmount.Text)
                        End If
                    End If
                End If
            End If

            ddlInputTax.Enabled = False
            ddlOutputTax.Enabled = False

            'End

        End If
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
        'Zulham 14/01/2016 - IPP Stage 4 Phase 2
        If Not txtExchangeRate.Visible = True Then
            'IPP Gst Stage 2A - CH - 13 Feb 2015
            row.Cells(EnumIPPDet.ippGSTAmount).Text = Format(ViewState("total") + gstAmount, "#,##0.00")
            row.Cells(EnumIPPDet.ippGSTAmount).HorizontalAlign = HorizontalAlign.Center
            row.Cells(EnumIPPDet.ippGSTAmount).Font.Bold = True
            row.Cells(EnumIPPDet.ippGSTAmount).Font.Underline = True
            row.Cells(EnumIPPDet.ippGSTAmount).Font.Overline = True
            row.Cells(EnumIPPDet.ippGSTAmount).HorizontalAlign = HorizontalAlign.Right
            row.Cells(EnumIPPDet.ippGSTAmount).BorderColor = Color.Transparent
        Else
            'Zulham 17/03/2016 - Case 10670
            row.Cells(EnumIPPDet.ippForeignGSTAmount).Text = Format(ViewState("total") + gstAmount, "#,##0.00")
            row.Cells(EnumIPPDet.ippForeignGSTAmount).HorizontalAlign = HorizontalAlign.Center
            row.Cells(EnumIPPDet.ippForeignGSTAmount).Font.Bold = True
            row.Cells(EnumIPPDet.ippForeignGSTAmount).Font.Underline = True
            row.Cells(EnumIPPDet.ippForeignGSTAmount).Font.Overline = True
            row.Cells(EnumIPPDet.ippForeignGSTAmount).HorizontalAlign = HorizontalAlign.Right
            row.Cells(EnumIPPDet.ippForeignGSTAmount).BorderColor = Color.Transparent
        End If
  
        'End If
        'If ViewState("strIsNostro") <> "Y" Then
        If Not lblCurrency.Text = "MYR" And TRExchangeRate.Visible = True Then
            row.Cells(EnumIPPDet.ippAmt).Text = Format(ViewState("total2") + gstAmount, "#,##0.00")
            row.Cells(EnumIPPDet.ippAmt).HorizontalAlign = HorizontalAlign.Center
            row.Cells(EnumIPPDet.ippAmt).Font.Bold = True
            row.Cells(EnumIPPDet.ippAmt).Font.Underline = True
            row.Cells(EnumIPPDet.ippAmt).Font.Overline = True
            row.Cells(EnumIPPDet.ippAmt).HorizontalAlign = HorizontalAlign.Right
            row.Cells(EnumIPPDet.ippAmt).BorderColor = Color.Transparent
        Else
            row.Cells(EnumIPPDet.ippAmt2).Text = Format(ViewState("total2") + gstAmount, "#,##0.00")
            row.Cells(EnumIPPDet.ippAmt2).HorizontalAlign = HorizontalAlign.Center
            row.Cells(EnumIPPDet.ippAmt2).Font.Bold = True
            row.Cells(EnumIPPDet.ippAmt2).Font.Underline = True
            row.Cells(EnumIPPDet.ippAmt2).Font.Overline = True
            row.Cells(EnumIPPDet.ippAmt2).HorizontalAlign = HorizontalAlign.Right
            row.Cells(EnumIPPDet.ippAmt2).BorderColor = Color.Transparent
        End If

        row.Cells(EnumIPPDet.ippSNo).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippDesc).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippQty).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippUOM).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippGL).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippGLRuleCat).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippBranch).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippCostCentre).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippCurrency).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippExchangeRate).BorderColor = Color.Transparent

        'Zulham Sept 18, 2014
        'IPP Gst Stage 2A - CH - 13 Feb 2015
        'row.Cells(EnumIPPDet.ippGSTAmount).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippAmt).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippInputTax).BorderColor = Color.Transparent
        row.Cells(EnumIPPDet.ippOutputTax).BorderColor = Color.Transparent
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
            Session("w_AddPO_tabs") = "<div class=""t_entity""><ul>" & _
                 "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "IPPEntry.aspx", "mode=new&pageid=" & strPageId) & """><span>IPP Document</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPP", "IPPList.aspx", "type=Listing&mode=new&pageid=" & strPageId) & """><span>IPP Document Listing</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                             "</ul><div></div></div>"

        End If

    End Sub

    Private Sub cmdViewAudit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdViewAudit.Click
        Dim strscript As New System.Text.StringBuilder
        Dim objBillingDetails As New IPPDetails
        Dim strFileName As String

        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("IPP", "ViewIPPAuditTrails.aspx", "pageid=" & strPageId & "&docno=" & Session("DocNo") & "&docstatus=" & Server.UrlEncode(lblStatus.Text) & "&docidx=" & Session("InvIdx") & "&module=Billing")
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'520px');")
        'strscript.Append("document.getElementById('btnhidden3').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script1", strscript.ToString())
    End Sub

End Class