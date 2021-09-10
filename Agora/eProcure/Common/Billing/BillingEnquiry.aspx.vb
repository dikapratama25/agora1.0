Imports eProcure.Component
Imports AgoraLegacy
Imports System.IO


Partial Public Class BillingEnquiry
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Public Enum EnumBill
        DocNo = 0
        TaxInvoiceNo = 1
        CreatedBy = 2
        DocDate = 3
        Vendor = 4
        BankCode = 5
        BankAccount = 6
        Currency = 7
        PayAmount = 8
        ReceiptDate = 9
        ReceiptNo = 10
        Status = 11
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objIPP As New IPP
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgBillList)
        Session("dsBillDocDetail") = Nothing
        Session("dsBillDocDetailCount") = Nothing
        Session("urlreferer") = "BillingEnq"
        PopulateTypeAhead()

        objIPP = Nothing
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objBill As New Billing
        Dim ds As New DataSet
        Dim i As Integer
        Dim docstatus As String = ""
        Dim doctype, docno, docsdt, docedt, strVen, strVenAddr As String
        ds = Nothing

        ViewState("venidx") = hid6.Value

        If txtVendor.Value <> "" Then
            strVen = Common.Parse(txtVendor.Value)
        End If
        If txtVendorAddr.Value <> "" Then
            strVenAddr = Common.Parse(txtVendorAddr.Value)
        End If
        If txtDocNo.Text <> "" Then
            docno = txtDocNo.Text
        End If
        If ddlDocType.SelectedIndex <> 0 Then
            doctype = ddlDocType.SelectedValue
        End If
        If txtDocStartDate.Value <> "" Then
            docsdt = txtDocStartDate.Value
        End If
        If txtDocEndDate.Value <> "" Then
            docedt = txtDocEndDate.Value
        End If
        For i = 0 To chkdocstatus.Items.Count - 1
            If chkdocstatus.Items(i).Selected Then
                docstatus &= chkdocstatus.Items(i).Value & ","
            End If
        Next
        If docstatus <> "" Then
            docstatus = docstatus.Substring(0, docstatus.Length - 1)
        End If

        'IPP teller and IPP Verifier can only see docs under their dept. Whereas Finance would be excluded from the restriction
        Dim Roles As New ArrayList : Roles = Session("MixUserRole")
        For _count As Integer = 0 To Roles.Count - 1
            If Roles(_count).ToString = "Billing Officer" Or Roles(_count).ToString = "Billing Approving Officer" Then 'of source dept
                'Get the dept code for this particular user
                Dim objdb As New EAD.DBCom
                Dim deptCode = objdb.GetVal("select um_dept_id from user_mstr where um_user_id='" & Session("UserId") & "' and um_coy_id = '" & Session("CompanyId") & "'")
                ds = objBill.PopulateBillEnqList(docno, docstatus, doctype, docsdt, docedt, strVen, strVenAddr, deptCode, ViewState("venidx"))
                Exit For
            Else 'of finance;no restrictions
                ds = objBill.PopulateBillEnqList(docno, docstatus, doctype, docsdt, docedt, strVen, strVenAddr, , ViewState("venidx"))
            End If
        Next

        intPageRecordCnt = ds.Tables(0).Rows.Count

        ViewState("RecordCount") = intPageRecordCnt
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgBillList, dvViewSample)
            dtgBillList.DataSource = dvViewSample
            dtgBillList.DataBind()
            btnExcel.Visible = True
        Else
            dtgBillList.DataSource = dvViewSample
            dtgBillList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            btnExcel.Visible = False
        End If
        ' add for above checking
        ViewState("PageCount") = dtgBillList.PageCount
        objBill = Nothing
        ds = Nothing
    End Function

    Private Sub dtgBillList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgBillList.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgBillList, e)
        intPageRecordCnt = ViewState("RecordCount")
    End Sub

    Sub dtgBillList_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgBillList.PageIndexChanged
        dtgBillList.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub dtgBillList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgBillList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dvMatch As DataView = dv.DataView
            'Zulham 18/08/2017 - IPP Stage 3
            Dim objdb As New EAD.DBCom
            '''
            Select Case e.Item.Cells(EnumBill.Status).Text
                Case 1, 5
                    'Case 14
                    '    e.Item.Cells(EnumBill.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "IPPDocument.aspx", "Frm=IPPEnq&index=" & dv("BM_INVOICE_INDEX") & "&DocumentNo=" & dv("BM_INVOICE_NO")) & """ ><font color=#0000ff>" & Common.parseNull(dv("BM_INVOICE_NO")) & "</font></A>"
                Case 3, 4, 6, 2
                    e.Item.Cells(EnumBill.DocNo).Text = "<A href=""" & dDispatcher.direct("Billing", "BillingDocument.aspx", "Frm=BillingEnq&index=" & dv("BM_INVOICE_INDEX") & "&DocumentNo=" & dv("BM_INVOICE_NO")) & """ ><font color=#0000ff>" & Common.parseNull(dv("BM_INVOICE_NO")) & "</font></A>"
            End Select

            e.Item.Cells(EnumBill.DocDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("BM_BILL_DATE"))
            e.Item.Cells(EnumBill.PayAmount).Text = Format(dv("BM_INVOICE_TOTAL"), "#,##0.00")

            'Zulham 18/08/2017 - IPP Stage 3
            If Not dv("BM_REMARKS1") Is DBNull.Value Then
                If Not Trim(dv("BM_REMARKS1")) = "" Then
                    If dv("BM_REMARKS1").ToString = "IPP" Then
                        Dim paymentAmt = objdb.GetVal("SELECT SUM(bm_received_qty * bm_unit_cost + bm_gst_value) AS 'payment_amt' FROM billing_details WHERE bm_invoice_no = '" & dv("bM_INVOICE_no") & "'")
                        e.Item.Cells(EnumBill.PayAmount).Text = Format(CDec(paymentAmt), "#,##0.00")
                    End If
                End If
            End If

            e.Item.Cells(EnumBill.ReceiptDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("BM_RECEIPT_DATE"))

            Select Case e.Item.Cells(EnumBill.Status).Text
                Case 1
                    e.Item.Cells(EnumBill.Status).Text = "Draft"
                Case 2
                    e.Item.Cells(EnumBill.Status).Text = "Submitted"
                Case 3
                    e.Item.Cells(EnumBill.Status).Text = "Approved"
                Case 4
                    e.Item.Cells(EnumBill.Status).Text = "Rejected"
                Case 5
                    e.Item.Cells(EnumBill.Status).Text = "Void"
                Case 6
                    e.Item.Cells(EnumBill.Status).Text = "Billed"
            End Select
        End If

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgBillList.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim docsd, doced As Date

        If txtDocStartDate.Value = "" And txtDocEndDate.Value = "" And txtDocNo.Text = "" And txtVendor.Value = "" And txtVendorAddr.Value = "" Then
            Common.NetMsgbox(Me, "Please enter either the Document Date Range or Document No or Vendor Name")
            Exit Sub
        End If

        If txtDocStartDate.Value <> "" Then
            docsd = txtDocStartDate.Value
        End If
        If txtDocEndDate.Value <> "" Then
            doced = txtDocEndDate.Value
        End If

        If (DateDiff("m", docsd, doced) > 6) Then
            Common.NetMsgbox(Me, "Date range must be within 6 months.")
            dtgBillList.DataBind()
            btnExcel.Visible = False
        Else
            Bindgrid()
        End If
    End Sub

    Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        txtDocStartDate.Value = ""
        txtDocEndDate.Value = ""
        txtDocNo.Text = ""
        txtVendor.Value = ""
        Me.txtVendorAddr.Value = ""
        hid6.Value = ""

        Dim chkbox As ListItem
        For Each chkbox In chkdocstatus.Items
            chkbox.Selected = False
        Next
    End Sub

    Private Sub cmdSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelectAll.Click
        Dim chkbox As ListItem
        For Each chkbox In chkdocstatus.Items
            chkbox.Selected = True
        Next
    End Sub

    Sub PopulateTypeAhead()
        Dim typeahead As String
        Dim i, count As Integer
        Dim content, content2 As String
        Dim strCompID As String
        Dim vtypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=IPPCOY&frm=ippenq&CoyType=V&module=billing")


        content &= "$(""#txtVendor"").autocomplete(""" & vtypeahead & """, {" & vbCrLf &
        "width: 200," & vbCrLf &
        "scroll: true," & vbCrLf &
        "selectFirst: false" & vbCrLf &
        "});" & vbCrLf &
        "$(""#txtVendor"").result(function(event,data,item) {" & vbCrLf &
        "if (data)" & vbCrLf &
        "$(""#hid6"").val(data[1]);" & vbCrLf &
        "$(""#btnGetAdd"").trigger('click');" & vbCrLf &
        "});" & vbCrLf


        typeahead = "<script language=""javascript"">" & vbCrLf &
      "<!--" & vbCrLf &
        "$(document).ready(function(){" & vbCrLf &
        content & vbCrLf &
        "});" & vbCrLf &
        "-->" & vbCrLf &
        "</script>"


        Session("typeahead2") = typeahead
    End Sub

    Private Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        ExportToExcel()
    End Sub

    Private Sub ExportToExcel()
        Dim objBill As New Billing
        Dim ds As New DataSet
        Dim i As Integer
        Dim docstatus As String = ""
        Dim doctype, docno, docsdt, docedt, strVen, strVenAddr As String
        Dim strFileName As String
        Dim Count As Integer

        If ViewState("venidx") Is Nothing Then ViewState("venidx") = hid6.Value

        If txtVendor.Value <> "" Then
            strVen = Common.Parse(txtVendor.Value)
        End If
        If txtVendorAddr.Value <> "" Then
            strVenAddr = Common.Parse(txtVendorAddr.Value)
        End If
        If txtDocNo.Text <> "" Then
            docno = txtDocNo.Text
        End If
        If ddlDocType.SelectedIndex <> 0 Then
            doctype = ddlDocType.SelectedValue
        End If
        If txtDocStartDate.Value <> "" Then
            docsdt = txtDocStartDate.Value
        End If
        If txtDocEndDate.Value <> "" Then
            docedt = txtDocEndDate.Value
        End If
        For i = 0 To chkdocstatus.Items.Count - 1
            If chkdocstatus.Items(i).Selected Then
                docstatus &= chkdocstatus.Items(i).Value & ","
            End If
        Next
        If docstatus <> "" Then
            docstatus = docstatus.Substring(0, docstatus.Length - 1)
        End If

        Dim Roles As New ArrayList : Roles = Session("MixUserRole")
        For _count As Integer = 0 To Roles.Count - 1
            If Roles(_count).ToString = "Billing Officer" Or Roles(_count).ToString = "Billing Approving Officer" Then 'of source dept
                'Get the dept code for this particular user
                Dim objdb As New EAD.DBCom
                Dim deptCode = objdb.GetVal("select um_dept_id from user_mstr where um_user_id='" & Session("UserId") & "' and um_coy_id = '" & Session("CompanyId") & "'")
                ds = objBill.PopulateBillEnqList(docno, docstatus, doctype, docsdt, docedt, strVen, strVenAddr, deptCode, ViewState("venidx"))
            Else 'of finance;no restrictions
                ds = objBill.PopulateBillEnqList(docno, docstatus, doctype, docsdt, docedt, strVen, strVenAddr, , ViewState("venidx"))
            End If
        Next

        strFileName = "Billing Document Enquiry Search Result" & "(" & Format(Now, "ddMMMyyyy") & ").xls"
        Dim attachment As String = "attachment;filename=" & strFileName
        Response.ClearContent()
        Response.AddHeader("Content-Disposition", attachment)
        Response.ContentType = "application/vnd.ms-excel"

        Dim dc As DataColumn

        Response.Write("Document No.") '0
        Response.Write(vbTab + "Teller ID") '1
        Response.Write(vbTab + "Document Date") '2
        Response.Write(vbTab + "Customer") '3
        Response.Write(vbTab + "Bank Code") '4
        Response.Write(vbTab + "Bank Account") '5
        Response.Write(vbTab + "Currency") '6
        Response.Write(vbTab + "Billing Amount") '7
        Response.Write(vbTab + "Receipt Date") '8
        Response.Write(vbTab + "Receipt No.") '9
        Response.Write(vbTab + "Status") '10
        Response.Write(vbCrLf)

        Dim dr As DataRow
        For Each dr In ds.Tables(0).Rows
            For i = 0 To ds.Tables(0).Columns.Count - 1
                If i = 0 Then
                    Response.Write(dr.Item(i + 1).ToString)
                End If
                'If i = 1 Then
                'If dr.Item(i + 1).ToString = "INV" Then
                'Response.Write(vbTab + "Invoice")
                'ElseIf dr.Item(i + 1).ToString = "BILL" Then
                'Response.Write(vbTab + "Bill")
                'ElseIf dr.Item(i + 1).ToString = "CN" Then
                'Response.Write(vbTab + "Credit Note")
                'ElseIf dr.Item(i + 1).ToString = "DN" Then
                'Response.Write(vbTab + "Debit Note")
                'Else
                'Response.Write(vbTab + dr.Item(i + 1).ToString)
                'End If
                'End If
                'If (i >= 2) And (i <= 10) Then
                If (i >= 1) And (i <= 9) Then
                    Response.Write(vbTab + dr.Item(i + 1).ToString)
                End If
                If i = 10 Then
                    If dr.Item(i + 1).ToString = "1" Then
                        Response.Write(vbTab + "Draft")
                    ElseIf dr.Item(i + 1).ToString = "2" Then
                        Response.Write(vbTab + "Submitted")
                    ElseIf dr.Item(i + 1).ToString = "3" Then
                        Response.Write(vbTab + "Approved")
                    ElseIf dr.Item(i + 1).ToString = "4" Then
                        Response.Write(vbTab + "Rejected")
                    ElseIf dr.Item(i + 1).ToString = "5" Then
                        Response.Write(vbTab + "Void")
                    ElseIf dr.Item(i + 1).ToString = "6" Then
                        Response.Write(vbTab + "Billed")
                    Else
                        Response.Write(vbTab + dr.Item(i + 1).ToString)
                    End If
                End If
            Next
            Response.Write(vbCrLf)
        Next
        Response.End()
    End Sub



End Class