'Zulham 30-01-2015 IPP-GST Stage 2A
Imports AgoraLegacy
Imports eProcure.Component
Imports System.drawing
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient


Partial Public Class BillingAOApprovalDetail
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
    Dim TimeNow As String = DateTime.Now.ToLocalTime.ToString("HH:mm")
    Dim strFrm As String
    Dim _exceedCutOffDt As String = ""

    Public Enum EnumSubDocDet
        subDocNo = 0
        subDocDate = 1
        subDocAmt = 2
        subEmpty = 3
        subDocGSTValue = 4
    End Enum

    Public Enum EnumIPPDet
        ippSNo = 0
        'Zulham IPP Stage 3 - 20/07/2017
        ippRefNo = 1
        ''
        ippDesc = 2 '1
        ippUOM = 3 '2
        ippQty = 4 '3
        ippUnitPrice = 5 '4
        ippAmt = 6 '5
        ippGSTAmount = 7 '6
        ippInputTax = 8 '7
        ippOutputTax = 9 '8
        ippGL = 10 '9
        ippGLRuleCat = 11 '10
        ippBranch = 12 '11
        ippCostCentre = 13 '12
    End Enum

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim objIPP As New Billing
        Dim ObjDoc As New IPPMain
        Dim objIPPDetail As New BillingDetails
        Dim blnRelief As Boolean
        Dim intApprGrpIndex As String

        strFrm = Me.Request.QueryString("Frm")
        Session("DocNo") = Request.QueryString("DocumentNo")
        Session("Index") = Request.QueryString("index")

        intApprGrpIndex = objDb.GetVal("SELECT DISTINCT BA_APPROVAL_GRP_INDEX FROM billing_approval WHERE BA_bill_INDEX = '" & Session("Index") & "' ")

        VenIdx = objDb.GetVal("SELECT bm_s_coy_id FROM billing_mstr WHERE bm_invoice_index = '" & Session("Index") & "'")

        'Zulham 2017/08/17 - IPP Stage 3
        ViewState("venidx") = VenIdx

        blnSorting = False

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgApprvFlow, "N")
        SetGridProperty(dtgDocDetail, "N")
        Session("AO") = Session("UserID")
        Dim objUsers As New Users
        Dim blnIPPAO As Boolean

        'Zulham IPP Stage 3 - 20/07/2017
        Dim docType = objDb.GetVal("SELECT trim(bm_invoice_type) FROM billing_mstr WHERE bm_invoice_index = '" & Session("Index") & "'")
        If docType = "CN" Or docType = "DN" Or docType = "CNN" Or docType = "DNN" Then
            Me.dtgDocDetail.Columns(EnumIPPDet.ippRefNo).Visible = True
        Else
            Me.dtgDocDetail.Columns(EnumIPPDet.ippRefNo).Visible = False
        End If

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
        'If Not Page.IsPostBack Then
        ViewState("isResident") = ""
        objIPPDetail = objIPP.GetApprBillingDetails(Session("index"), Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")))
        'Zulham 27/07/2017 - IPP Stage 3
        Select Case objIPPDetail.DocType
            Case "INV"
                Me.lblDocType.Text = "Invoice"
            Case "NON"
                Me.lblDocType.Text = "Non Invoice"
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
        End Select
        'End If

        Me.lblDocNo.Text = objIPPDetail.DocNo
        Me.lblDocDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objIPPDetail.DocDate)
        Me.lblManualPONo.Text = objIPPDetail.ManualPONo
        Me.lblStatus.Text = objIPPDetail.StatusDescription
        Me.lblCurrency.Text = objIPPDetail.Currency
        If Not objIPPDetail.Currency = "MYR" Then
            TRExchangeRate.Visible = True
        End If
        Me.lblPaymentAmt.Text = Format(objIPPDetail.PaymentAmt, "Standard")

        Me.lblNoGSTAmount.Text = Format(objIPPDetail.PaymentAmtWthGST, "Standard")

        Me.lblVendor.Text = objIPPDetail.Vendor
        Me.lblVendorAddr.Text = Replace(objIPPDetail.VAddr, "<BR>", "<BR>&nbsp;") & "<BR>&nbsp;"

        'Zulham 03/08/2017 - IPP Stage 3
        Me.txtRemarks.Text = objIPPDetail.Remarks
        If Not Page.IsPostBack Then
            Me.txtExchangeRate.Text = objIPPDetail.ExchangeRate
        End If

        Me.lblDocDueDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objIPPDetail.DocDueDate)

        txtRemarks.Enabled = False

        objIPPDetail = Nothing

        If ((CDate(TimeNow) >= CDate(IPPTimeFr1) And CDate(TimeNow) <= CDate(IPPTimeTo1)) Or (CDate(TimeNow) >= CDate(IPPTimeFr2) And CDate(TimeNow) <= CDate(IPPTimeTo2))) And ViewState("role") = "3" Then
            cmdAppIPP.Visible = False
            cmdRejectIPP.Visible = False
        End If

        If Request.QueryString("mode") = "read" Then
            txtRemarks.Enabled = False
            txtApprRejRemark.Visible = False
            Label43.Visible = False
            cmdAppIPP.Visible = False
            cmdRejectIPP.Visible = False
            cmdViewAudit.Visible = True
        End If
        'End If

        'Check for GST
        Dim documentDate = objDb.GetVal("SELECT IFNULL(bm_created_on,'') 'bm_created_on' FROM billing_mstr WHERE bm_invoice_index = '" & Session("Index") & "'")
        Dim createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
        Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
        'Dim _exceedCutOffDt As String = ""
        Dim strIsGst As String
        If CDate(documentDate) >= CDate(_cutoffDate) Then
            _exceedCutOffDt = "Yes"
            If lblVendor.Text <> "" Then
                Dim GSTRegNo = objDb.GetVal("SELECT IFNULL(IC_TAX_REG_NO, '') FROM IPP_COMPANY WHERE ic_coy_name = '" & Me.lblVendor.Text & "'")
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
            dtgDocDetail.Columns(9).Visible = True
            dtgDocDetail.Columns(10).Visible = True
            dtgDocDetail.Columns(11).Visible = True
        ElseIf _exceedCutOffDt = "Yes" Then
            dtgDocDetail.Columns(2).Visible = True
            dtgDocDetail.Columns(9).Visible = True
            dtgDocDetail.Columns(10).Visible = True
            dtgDocDetail.Columns(11).Visible = True
        Else
            dtgDocDetail.Columns(2).Visible = False
            dtgDocDetail.Columns(9).Visible = False
            dtgDocDetail.Columns(10).Visible = False
            dtgDocDetail.Columns(11).Visible = False
        End If

        cmdRejectIPP.Attributes.Add("onclick", "return confirmReject();")
    End Sub

    Private Sub renderIPPApprFlow()
        Dim objDoc As New Billing

        Dim ds As DataSet = objDoc.getbillingApprFlow(Session("Index"), Common.Parse(HttpContext.Current.Session("CompanyID")))
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
        Dim objIPPmain As New Billing
        Dim ds As New DataSet
        Dim dvViewIPP As DataView

        ds = objIPPmain.getApprBillingDetail(Session("DocNo"), Session("index"), Common.Parse(HttpContext.Current.Session("CompanyID")))
        ViewState("DocDetails") = ds
        dvViewIPP = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewIPP.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewIPP.Sort += " DESC"
        End If

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        '//bind datagrid
        If Not Page.IsPostBack Then
            If ViewState("intPageRecordCnt") > 0 Then
                ViewState("gstTotal") = 0
                dtgDocDetail.DataSource = dvViewIPP
                dtgDocDetail.DataBind()
            Else
                dtgDocDetail.DataSource = dvViewIPP
                dtgDocDetail.DataBind()
                Common.NetMsgbox(Me, MsgNoRecord)
            End If
        End If
    End Function

    Public Sub dtgSubDocDetail_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
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

            'ViewState("HighestAppr") = dv("FA_Seq")

            'If Not IsDBNull(dv("FA_ACTION_DATE")) Then
            '    e.Item.Cells(2).Text = dv("FA_ACTION_DATE")
            'End If

        End If
    End Sub

    Private Sub dtgDocDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDocDetail.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dblAmount As Double

            dblAmount = Common.parseNull(dv("BM_RECEIVED_QTY"), 0) * Common.parseNull(dv("BM_UNIT_COST"), 0)
            e.Item.Cells(EnumIPPDet.ippAmt).Text = Format(dblAmount, "#,##0.00")
            e.Item.Cells(EnumIPPDet.ippUnitPrice).Text = Format(dv("BM_UNIT_COST"), "#,##0.00")
            ViewState("total") = ViewState("total") + dblAmount

            e.Item.Cells(EnumIPPDet.ippGL).ToolTip = Common.parseNull(dv("CBG_B_GL_DESC"))
            e.Item.Cells(EnumIPPDet.ippCostCentre).ToolTip = Common.parseNull(dv("BM_COST_CENTER_DESC"))

            'Zulham Sept 17, 2014
            Dim ddlInputTax, ddlOutputTax As DropDownList
            Dim lblGSTAmount As TextBox
            Dim GST As New GST
            'Get the predefined ouputtax
            Dim predefinedOutputTax = objDb.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'REVERSE_CHARGE_OUTPUT'")

            Dim objGlobal As New AgoraLegacy.AppGlobals
            ddlInputTax = e.Item.FindControl("ddlInputTax")
            ddlOutputTax = e.Item.FindControl("ddlOutputTax")
            lblGSTAmount = e.Item.FindControl("lblGSTAmount")
            lblGSTAmount.Style("text-align") = "Right"
            GST.FillTaxCode_forIPP(ddlInputTax, "", "P")
            GST.FillTaxCode_forIPP(ddlOutputTax, "", "S")

            If Common.parseNull(dv("bm_gst_input_tax_code")) <> "" Then
                If Not dv("bm_gst_input_tax_code").ToString = "0" Then
                    If ViewState("InputTax") Is Nothing Then
                        ddlInputTax.SelectedValue = dv("bm_gst_input_tax_code").ToString.Trim
                    Else
                        ddlInputTax.SelectedValue = ViewState("InputTax").ToString.Trim
                    End If
                    If dv("bm_gst_input_tax_code").ToString.Trim.Contains("NR") Then
                        ddlInputTax.Enabled = False
                        lblGSTAmount.Enabled = False
                    End If
                Else
                    If ViewState("InputTax") Is Nothing Then
                        ddlInputTax.Items.Add(New ListItem("N/A", 0))
                        ddlInputTax.Enabled = False
                        ddlInputTax.SelectedValue = 0
                    Else
                        ddlInputTax.SelectedValue = ViewState("InputTax").ToString.Trim
                    End If
                End If
            Else
                ddlInputTax.Items.Add(New ListItem("N/A", 0))
                ddlInputTax.Enabled = False
                ddlInputTax.SelectedValue = 0
            End If
            If Common.parseNull(dv("bm_gst_output_tax_code")) <> "" Then
                If Not dv("bm_gst_output_tax_code").ToString = "0" And Not dv("bm_gst_output_tax_code").ToString = "N/A" Then
                    If ViewState("OutputTax") Is Nothing Then
                        ddlOutputTax.SelectedValue = dv("bm_gst_output_tax_code").ToString.Trim
                    Else
                        ddlOutputTax.SelectedValue = ViewState("OutputTax").ToString.Trim
                    End If
                Else
                    If ViewState("OutputTax") Is Nothing Then
                        ddlOutputTax.Items.Add(New ListItem("N/A", 0))
                        ddlOutputTax.Enabled = False
                        ddlOutputTax.SelectedValue = 0
                    Else
                        ddlOutputTax.SelectedValue = ViewState("OutputTax").ToString.Trim
                    End If
                End If
            Else
                ddlOutputTax.Items.Add(New ListItem("N/A", 0))
                ddlOutputTax.SelectedValue = 0
                ddlOutputTax.Enabled = False
            End If

            If dv("bm_gst_value") IsNot DBNull.Value Then lblGSTAmount.Text = Format(dv("bm_gst_value"), "#,##0.00") Else lblGSTAmount.Text = Format(0, "#,##0.00")

            If Common.parseNull(dv("bm_gst_input_tax_code")) <> "" Then
                If Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Contains("(") Then
                    If Not Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Contains("0") Then
                        If Not lblGSTAmount.Text = "" Then ViewState("gstTotal") = ViewState("gstTotal") + CDec(lblGSTAmount.Text)
                    End If
                End If
            End If

            If Common.parseNull(dv("bm_gst_output_tax_code")) <> "" Then
                If Common.parseNull(ddlOutputTax.SelectedItem.Text).ToString.Contains("(") Then
                    If Not Common.parseNull(ddlOutputTax.SelectedItem.Text).ToString.Contains("0") Then
                        If Not lblGSTAmount.Text = "" Then ViewState("gstTotal") = ViewState("gstTotal") + CDec(lblGSTAmount.Text)
                    End If
                End If
            End If

            If ddlInputTax.SelectedItem.Text.Contains("IM2") Or ddlInputTax.SelectedItem.Text.Contains("IM1") _
            Or ddlInputTax.SelectedItem.Text.Contains("IM3") Or ddlInputTax.SelectedItem.Text.Contains("N/A") Then
                ddlOutputTax.Enabled = False
            End If

            If IsDBNull(dv("bm_gst_input_tax_code")) Then
                ddlInputTax.Enabled = False
                lblGSTAmount.ReadOnly = True
            End If
            ddlInputTax.Attributes.Add("onchange", "return taxChange('" & ddlInputTax.ClientID & "','" & ddlOutputTax.ClientID & "','" & dblAmount & "','" & lblGSTAmount.ClientID & "','" & predefinedOutputTax & "');")
            ddlOutputTax.Attributes.Add("onchange", "return taxChange('" & ddlInputTax.ClientID & "','" & ddlOutputTax.ClientID & "','" & dblAmount & "','" & lblGSTAmount.ClientID & "','" & predefinedOutputTax & "');")
            ddlInputTax.Attributes.Add("onkeypress", "return isDecimalKey(event);")
            'End
            ddlInputTax.Enabled = False
            ddlOutputTax.Enabled = False
            lblGSTAmount.ReadOnly = True

        End If
    End Sub
    Sub AddRowtotal() 'add total row 
        Dim gstAmount = 0.0
        Dim intL As Integer
        Dim dgItem As DataGridItem
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim IsResident = ViewState("isResident")

        For intL = 0 To Me.dtgDocDetail.Columns.Count - 1
            addCell(row)
        Next

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

                Dim predefinedOutputTax = objDb.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'REVERSE_CHARGE_OUTPUT'")
                If ddlInputTax.SelectedValue.Contains("IM2") Then
                    ddlOutputTax.SelectedValue = predefinedOutputTax
                    ddlOutputTax.Enabled = False
                    lblGSTAmount.Text = (CDec(dgItem.Cells(5).Text) * ddlInputTax.SelectedItem.Text.Split("(")(1).Substring(0, 1)) / 100
                ElseIf ddlInputTax.SelectedValue.Contains("IM3") Or ddlInputTax.SelectedValue.Contains("IM1") Then
                    Dim lstItem As New ListItem
                    lstItem.Value = "N/A"
                    lstItem.Text = "N/A"
                    ddlOutputTax.Items.Insert(0, lstItem)
                    ddlOutputTax.SelectedValue = lstItem.Value
                    ddlOutputTax.Enabled = False
                Else
                    If ddlOutputTax.Items(0).Text = "N/A" Then ddlOutputTax.Items.RemoveAt(0)
                    If ddlOutputTax.SelectedItem.Text <> "N/A" Or ddlInputTax.SelectedItem.Text <> "N/A" Then
                        ddlOutputTax.Enabled = True
                    Else
                        ddlOutputTax.Enabled = False
                    End If
                    If ddlOutputTax.SelectedItem.Text = "N/A" And (ddlInputTax.SelectedValue.Contains("IM3") Or ddlInputTax.SelectedValue.Contains("IM1") Or IsResident = "Y") Then
                        ddlOutputTax.Enabled = False
                    End If
                End If
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
                            If Not lblGSTAmount.Text = "" Then ViewState("gstTotal") += CDec(lblGSTAmount.Text)
                        End If
                    End If
                End If

                ViewState("total") += dgItem.Cells(5).Text
                ViewState("total2") += dgItem.Cells(5).Text

                ddlOutputTax.Enabled = False
                ddlInputTax.Enabled = False
            Next
        End If
        'End

        If Not ViewState("gstTotal") Is Nothing Then
            If Not ViewState("gstTotal").ToString.Length = 0 Then
                gstAmount = ViewState("gstTotal")
            End If
        End If

        row.Cells(EnumIPPDet.ippUnitPrice).Text = "Total :"
        row.Cells(EnumIPPDet.ippUnitPrice).HorizontalAlign = HorizontalAlign.Center
        row.Cells(EnumIPPDet.ippUnitPrice).Font.Bold = True
        row.Cells(EnumIPPDet.ippUnitPrice).BorderStyle = BorderStyle.None

        row.Cells(EnumIPPDet.ippAmt).Text = Format(ViewState("total") + gstAmount, "#,##0.00")
        row.Cells(EnumIPPDet.ippAmt).HorizontalAlign = HorizontalAlign.Center
        row.Cells(EnumIPPDet.ippAmt).Font.Bold = True
        row.Cells(EnumIPPDet.ippAmt).Font.Underline = True
        row.Cells(EnumIPPDet.ippAmt).Font.Overline = True
        row.Cells(EnumIPPDet.ippAmt).HorizontalAlign = HorizontalAlign.Right
        row.Cells(EnumIPPDet.ippAmt).BorderStyle = BorderStyle.None

        row.Cells(EnumIPPDet.ippSNo).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippDesc).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippQty).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippUOM).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippGL).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippGLRuleCat).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippBranch).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippCostCentre).BorderStyle = BorderStyle.None

        'Zulham Sept 18, 2014
        row.Cells(EnumIPPDet.ippGSTAmount).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippInputTax).BorderStyle = BorderStyle.None
        row.Cells(EnumIPPDet.ippOutputTax).BorderStyle = BorderStyle.None
        'End

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
               "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Billing", "BillingApprovalList.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
               "<li><div class=""space""></div></li>" & _
               "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Billing", "BillingApprovalList_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
               "<li><div class=""space""></div></li>" & _
               "</ul><div></div></div>"
        Else
            Session("w_IPP_tabs") = "<div class=""t_entity""><ul>" & _
               "<li><div class=""space""></div></li>" & _
               "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Billing", "BillingApprovalList.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
               "<li><div class=""space""></div></li>" & _
               "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Billing", "BillingApprovalList_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
               "<li><div class=""space""></div></li>" & _
               "</ul><div></div></div>"
        End If

        If strFrm = "ApprList" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Billing", "BillingApprovalList.aspx", "pageid=" & strPageId)
            Session("w_IPP_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Billing", "BillingApprovalList.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Billing", "BillingApprovalList_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
        Else
            lnkBack.NavigateUrl = dDispatcher.direct("Billing", "BillingApprovalList_All.aspx", "pageid=" & strPageId)
        End If

        'Zulham 15/02/2015 8317
        If Request.QueryString("Frm") = "Dashboard" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx")
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
            Case "verify"
                strRemark = "Verified" & strTempRemark & ": "
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
        strRemark = FormatAORemark("verify")
        strRemark = strRemark & txtApprRejRemark.Text

        If txtApprRejRemark.Text = "" Then
            Common.NetMsgbox(Me, "Please enter Remarks.", MsgBoxStyle.Information)
        ElseIf TRExchangeRate.Visible And txtExchangeRate.Text.Trim = "" Then
            Common.NetMsgbox(Me, "Please enter exchange rate.", MsgBoxStyle.Information)
        Else
            Dim objDoc As New Billing
            Dim intApprGrpIndex As String

            'ValidaTION
            For Each dgItem In dtgDocDetail.Items
                Dim ddlInputTax, ddlOutputTax As DropDownList
                Dim lblGSTAmount As TextBox
                Dim GST As New GST
                Dim objGlobal As New AgoraLegacy.AppGlobals
                ddlInputTax = dgItem.FindControl("ddlInputTax")
                ddlOutputTax = dgItem.FindControl("ddlOutputTax")
                lblGSTAmount = dgItem.FindControl("lblGSTAmount")

                'Zulham 03/08/3017 - 
                itemTotal += dgItem.Cells(EnumIPPDet.ippQty).Text * dgItem.Cells(EnumIPPDet.ippUnitPrice).Text
                'Zulham Sept 17, 2014

                '•	Both Input Tax Code and Output Tax Code must be same range of tax percentage selected before.
                Dim IsResident = ""

                Dim CoyId As String
                Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
                If strDefIPPCompID = "" Then
                    CoyId = HttpContext.Current.Session("CompanyID")
                Else
                    CoyId = strDefIPPCompID
                End If

                IsResident = objDb.GetVal("SELECT ic_resident_type FROM IPP_company WHERE ic_coy_id =  '" & Common.Parse(CoyId) & "'")

                If Not ddlInputTax.SelectedValue = "" And Not ddlOutputTax.SelectedValue = "" Then
                    If ddlInputTax.SelectedItem.Text.Contains("(") And ddlOutputTax.SelectedItem.Text.Contains("(") Then
                        If Not (ddlInputTax.SelectedItem.Text.Contains("IM1") Or ddlInputTax.SelectedItem.Text.Contains("IM3")) Then
                            If CDec(ddlInputTax.SelectedItem.Text.Split("(")(1).Substring(0, 1)) <> _
                            CDec(ddlOutputTax.SelectedItem.Text.Split("(")(1).Substring(0, 1)) Then
                                strMsg = "Input Tax Code and Output Tax Code must be the same."
                                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                                BindgridSubDoc()
                                Exit Sub
                            ElseIf lblGSTAmount.Text.Length = 0 Then
                                strMsg = "Invalid GST Amount."
                                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                                BindgridSubDoc()
                                Exit Sub
                            ElseIf CDec(ddlInputTax.SelectedItem.Text.Split("(")(1).Substring(0, 1)) > CDec(0) And lblGSTAmount.Text = 0.0 Then
                                strMsg = "Invalid GST Amount."
                                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                                BindgridSubDoc()
                                Exit Sub
                            ElseIf CDec(ddlInputTax.SelectedItem.Text.Split("(")(1).Substring(0, 1)) = CDec(0) And CDec(lblGSTAmount.Text) > CDec(0.0) Then
                                strMsg = "Invalid GST Amount."
                                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                                BindgridSubDoc()
                                Exit Sub
                            End If
                        End If
                    ElseIf (ddlInputTax.SelectedItem.Text.Contains("IM1") Or ddlInputTax.SelectedItem.Text.Contains("IM3")) Then
                        If lblGSTAmount.Text.Length = 0 Then
                            strMsg = "Invalid GST Amount."
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                            BindgridSubDoc()
                            Exit Sub
                        ElseIf CDec(lblGSTAmount.Text) = CDec(0) Then
                            strMsg = "Invalid GST Amount."
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                            BindgridSubDoc()
                            Exit Sub
                        End If
                    ElseIf ddlInputTax.SelectedItem.Text.Contains("IM2") Then
                        If lblGSTAmount.Text.Length = 0 Then
                            strMsg = "Invalid GST Amount."
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                            BindgridSubDoc()
                            Exit Sub
                        ElseIf CDec(lblGSTAmount.Text) = CDec(0) Then
                            strMsg = "Invalid GST Amount."
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                            BindgridSubDoc()
                            Exit Sub
                        ElseIf ddlOutputTax.SelectedValue = 0 Then
                            strMsg = "Invalid GST Input Tax selection."
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                            BindgridSubDoc()
                            Exit Sub
                        End If
                    ElseIf ddlInputTax.SelectedItem.Text.Contains("%") And ddlOutputTax.SelectedItem.Text.Contains("N/A") And Not IsResident = "Y" Then
                        strMsg = "Invalid GST Output Tax selection."
                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                        BindgridSubDoc()
                        Exit Sub
                    ElseIf ddlInputTax.SelectedItem.Text.Contains("%") And Not ddlOutputTax.SelectedItem.Text.Contains("%") And Not IsResident = "Y" Then
                        strMsg = "Invalid GST Tax selection."
                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                        BindgridSubDoc()
                        Exit Sub
                    ElseIf Not ddlInputTax.SelectedItem.Text.Contains("%") And ddlOutputTax.SelectedItem.Text.Contains("%") And Not IsResident = "Y" Then
                        strMsg = "Invalid GST Tax selection."
                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                        BindgridSubDoc()
                        Exit Sub
                    Else
                        If ddlInputTax.SelectedItem.Text.ToUpper.Contains("SELECT") Then
                            strMsg = "Invalid GST Input Tax selection."
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                            BindgridSubDoc()
                            Exit Sub
                        End If
                        'Output TaxCode is not applicable to Resident Companies. Resident output tax will always be N/A
                        If Not IsResident = "Y" Then
                            If lblGSTAmount.Text.Length > 0 Then
                                If CDec(lblGSTAmount.Text) > CDec(0) Then
                                    strMsg = "Invalid GST Amount."
                                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                                    BindgridSubDoc()
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If
                ElseIf ddlOutputTax.SelectedValue = "" Then
                    strMsg = "Invalid GST Output Tax selection."
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                    BindgridSubDoc()
                    Exit Sub
                End If
                'End

                If Common.parseNull(ddlInputTax.SelectedItem.Text) <> "" Then
                    If Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Contains("(") Then
                        If Not Common.parseNull(ddlInputTax.SelectedItem.Text).ToString.Contains("0") Then
                            If Not lblGSTAmount.Text = "" Then gstTotal = ViewState("gstTotal") + CDec(lblGSTAmount.Text)
                        End If
                    End If
                End If

                If Common.parseNull(ddlInputTax.SelectedItem.Text) <> "" Then
                    If Common.parseNull(ddlOutputTax.SelectedItem.Text).ToString.Contains("(") Then
                        If Not Common.parseNull(ddlOutputTax.SelectedItem.Text).ToString.Contains("0") Then
                            If Not lblGSTAmount.Text = "" Then gstTotal = ViewState("gstTotal") + CDec(lblGSTAmount.Text)
                        End If
                    End If
                End If

            Next

            'Exit Sub
            'END

            'Zulham 22./06/2017
            'IPP Stage 3
            If (Me.lblDocType.Text = "Credit Note" Or Me.lblDocType.Text = "Debit Note" Or Me.lblDocType.Text = "Credit Note(Non-Invoice)" Or Me.lblDocType.Text = "Debit Note(Non-Invoice)") Then
                Dim dsDetails As New DataSet
                Dim strSql As String = ""
                strSql = "SELECT DISTINCT '*' FROM billing_mstr a, billing_details b " & _
                         "WHERE(a.bm_invoice_no = b.bm_ref_no) " & _
                         "AND b.bm_invoice_no = '" & Me.lblDocNo.Text & "' " & _
                         "AND b.bm_s_coy_id = '" & Common.Parse(ViewState("venidx")) & "' " & _
                         "AND a.bm_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " & _
                         "AND a.bm_invoice_status in ('2','4')"
                dsDetails = objDb.FillDs(strSql)
                If Not dsDetails Is Nothing Then
                    If Not dsDetails.Tables(0).Rows.Count = 0 Then
                        Common.NetMsgbox(Me, "Credit Note/Debit Note cannot precede the invoices it's tied to.", MsgBoxStyle.Exclamation)
                        Exit Sub
                    End If
                End If
            End If

            intApprGrpIndex = objDb.GetVal("SELECT DISTINCT bA_APPROVAL_GRP_INDEX FROM billing_approval WHERE BA_bill_INDEX = '" & Session("Index") & "' ")

            If objippmain.checkDept() = False Then
                objipp.Message(Me, "00046", MsgBoxStyle.Information)
                Exit Sub
            End If

            If objDoc.ApproveBillingDoc(Session("Index"), strRemark, Session("Relief"), ViewState("role"), "", txtExchangeRate.Text, lblDocType.Text, IPPTimeFr1, intApprGrpIndex, , aryDocDetails) = True Then
                'If objippmain.isHighestLevel(Session("Index")) Then
                strMsg = objGlobal.GetErrorMessage("00043") & " " & Session("DocNo") & " " & objGlobal.GetErrorMessage("00044")
                'Else
                'strMsg = objGlobal.GetErrorMessage("00043") & " " & Session("DocNo") & " " & objGlobal.GetErrorMessage("00051")
                'End If
            Common.NetMsgbox(Me, strMsg, lnkBack.NavigateUrl, MsgBoxStyle.Exclamation)
            Else
            strMsg = objGlobal.GetErrorMessage("00007")
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        End If
            'End If
            cmdRejectIPP.Visible = False
            cmdAppIPP.Visible = False
        End If
    End Sub

    Private Sub cmdRejectIPP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRejectIPP.Click
        Dim objDoc As New IPPMain
        Dim objipp As New IPP
        Dim objBilling As New Billing
        Dim strRemark As String
        Dim strMsg As String
        If objDoc.checkDept() = False Then
            objipp.Message(Me, "00046", MsgBoxStyle.Information)
            Exit Sub
        End If
        strRemark = FormatAORemark("reject")
        strRemark = strRemark & txtApprRejRemark.Text

        'Zulham 17/08/2017 - IPP Stage 3
        'Can't reject if the doc no is being referred to
        Dim dsDetails As New DataSet
        Dim strSql As String = ""
        strSql = "SELECT DISTINCT '*' FROM billing_mstr a, billing_details b " & _
                 "WHERE(a.bm_invoice_no = b.bm_invoice_no) " & _
                 "AND b.bm_ref_no = '" & Me.lblDocNo.Text & "' " & _
                 "AND b.bm_s_coy_id = '" & Common.Parse(ViewState("venidx")) & "' " & _
                 "AND a.bm_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " & _
                 "AND a.bm_invoice_status in ('2')"
        dsDetails = objDb.FillDs(strSql)
        If Not dsDetails Is Nothing Then
            If Not dsDetails.Tables(0).Rows.Count = 0 Then
                Common.NetMsgbox(Me, "Please reject the Debit Note/Credit Note it's tied to first.", MsgBoxStyle.Exclamation)
                Exit Sub
            End If
        End If


        If txtApprRejRemark.Text = "" Then
            Common.NetMsgbox(Me, "Please enter Remarks.", MsgBoxStyle.Information)
        Else
            If objBilling.RejectBillingDoc(Session("Index"), strRemark, Session("Relief"), ViewState("role")) = True Then
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
        strFileName = dDispatcher.direct("IPP", "ViewIPPAuditTrails.aspx", "pageid=" & strPageId & "&docno=" & Session("DocNo") & "&docstatus=" & Server.UrlEncode(lblStatus.Text) & "&docidx=" & Session("Index") & "&module=Billing")
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog(""" & dDispatcher.direct("IPP", "Dialog.aspx", "page=" & strFileName) & """,'520px');")
        strscript.Append("</script>")
        RegisterStartupScript("script1", strscript.ToString())
    End Sub
End Class