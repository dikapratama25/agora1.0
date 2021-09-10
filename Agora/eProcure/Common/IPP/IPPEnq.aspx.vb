Imports eProcure.Component
Imports AgoraLegacy
Imports System.IO


Partial Public Class IPPEnq
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher


    Public Enum EnumIPP
        DocNo = 0
        SubDocNo = 1
        DocType = 2
        DocDate = 3
        Vendor = 4
        BankCode = 5
        BankAccount = 6
        Currency = 7
        SubDocPayAmt = 8
        PayAmount = 9
        PayMethod = 10
        PSDSent = 11
        PSDRecv = 12
        PayDate = 13 '10
        PayAdviceNo = 14 '11
        Status = 15 '12
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objIPP As New IPP
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgIPPList)
        Session("dsIPPDocDetail") = Nothing
        Session("dsIPPDocDetailCount") = Nothing
        Session("urlreferer") = "IPPEnq"
        PopulateTypeAhead()

        objIPP = Nothing
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objIPP As New IPP
        Dim ds As New DataSet
        Dim i As Integer
        Dim docstatus As String = ""
        Dim doctype, docno, docsdt, docedt, strVen, paysdt, payedt, payadv, strVenAddr, psdsentsdt, psdsentedt As String
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
            docno = Common.Parse(docno.Replace("\", "\\\\")) 'Zulham 18072018
        End If
        If txtDocStartDate.Value <> "" And txtPayStartDate.Value = "" And txtPSDSentStartDate.Value = "" Then
            docsdt = txtDocStartDate.Value
        End If
        If txtDocEndDate.Value <> "" And txtPayEndDate.Value = "" And txtPSDSentEndDate.Value = "" Then
            docedt = txtDocEndDate.Value
        End If
        If txtDocStartDate.Value = "" And txtPayStartDate.Value <> "" And txtPSDSentStartDate.Value = "" Then
            paysdt = txtPayStartDate.Value
        End If
        If txtDocEndDate.Value = "" And txtPayEndDate.Value <> "" And txtPSDSentEndDate.Value = "" Then
            payedt = txtPayEndDate.Value
        End If
        If txtPSDSentStartDate.Value <> "" And txtDocEndDate.Value = "" And txtPayEndDate.Value = "" Then
            psdsentsdt = txtPSDSentStartDate.Value
        End If
        If txtPSDSentEndDate.Value <> "" And txtDocEndDate.Value = "" And txtPayEndDate.Value = "" Then
            psdsentedt = txtPSDSentEndDate.Value
        End If
        If txtPayAdv.Text <> "" Then
            payadv = Common.Parse(txtPayAdv.Text) 'Zulham 18072018 - PAMB
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
            If Roles(_count).ToString = "IPP Officer" Or Roles(_count).ToString = "IPP Approving Officer" Then 'of source dept
                'Get the dept code for this particular user
                Dim objdb As New EAD.DBCom
                Dim deptCode = objdb.GetVal("select um_dept_id from user_mstr where um_user_id='" & Session("UserId") & "' and um_coy_id = '" & Session("CompanyId") & "'")
                ds = objIPP.PopulateIPPEnqList(docno, payadv, docstatus, docsdt, docedt, paysdt, payedt, strVen, strVenAddr, deptCode, psdsentsdt, psdsentedt, ViewState("venidx"))
                'Zulham 12112018
                'Exit For
                'Zulham 05122018
            ElseIf Roles(_count).ToString = "IPP Officer(F)" Then  'of finance;no restrictions
                ds = objIPP.PopulateIPPEnqList(docno, payadv, docstatus, docsdt, docedt, paysdt, payedt, strVen, strVenAddr, , psdsentsdt, psdsentedt, ViewState("venidx"))
            End If
        Next

        intPageRecordCnt = ds.Tables(0).Rows.Count

        ViewState("RecordCount") = intPageRecordCnt
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgIPPList, dvViewSample)
            dtgIPPList.DataSource = dvViewSample
            dtgIPPList.DataBind()
            btnExcel.Visible = True
        Else
            dtgIPPList.DataSource = dvViewSample
            dtgIPPList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            btnExcel.Visible = False
        End If
        ' add for above checking
        ViewState("PageCount") = dtgIPPList.PageCount
        objIPP = Nothing
        ds = Nothing
    End Function

    Private Sub dtgIPPList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPPList.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgIPPList, e)
        intPageRecordCnt = ViewState("RecordCount")
    End Sub

    Sub dtgIPPList_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgIPPList.PageIndexChanged
        dtgIPPList.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub dtgIPPList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPPList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dvMatch As DataView = dv.DataView
            Select Case e.Item.Cells(EnumIPP.Status).Text
                'Zulham 13072018 - PAMB
                Case 10, 15
                    '    e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "IPPEntry.aspx", "mode=modify&urlrefereripp=" & Session("urlreferer") & "&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & dv("IM_INVOICE_NO")) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
                Case 14
                    e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "IPPDocument.aspx", "Frm=IPPEnq&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
                Case 4, 11, 12, 13, 16, 17, 18, 19
                    e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "IPPDocument.aspx", "Frm=IPPEnq&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
                    'Zulham 18/08/2017 - IPP Stage 3
                Case 3, 6
                    e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("Billing", "BillingDocument.aspx", "Frm=IPPEnq&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
                    ''
            End Select
            If Common.parseNull(dv("ISD_DOC_NO")) = "" Then
                e.Item.Cells(EnumIPP.SubDocNo).Text = "N/A"
            End If

            'Zulham 23042019 - REQ018
            If Not IsDBNull(dv("IM_DOC_DATE")) Then
                e.Item.Cells(EnumIPP.DocDate).Text = e.Item.Cells(EnumIPP.DocDate).Text.Substring(0, 10)
            End If

            If Not IsDBNull(dv("ISD_DOC_AMT")) Then
                e.Item.Cells(EnumIPP.SubDocPayAmt).Text = Format(dv("ISD_DOC_AMT"), "#,##0.00")
            End If
            e.Item.Cells(EnumIPP.PayAmount).Text = Format(dv("IM_INVOICE_TOTAL"), "#,##0.00")
            e.Item.Cells(EnumIPP.PayDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("IM_payment_date"))
            e.Item.Cells(EnumIPP.PSDSent).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("IM_prcs_sent"))
            e.Item.Cells(EnumIPP.PSDRecv).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("IM_prcs_Recv"))
            'Zulham 08112018
            Select Case e.Item.Cells(EnumIPP.Status).Text
                Case 4
                    e.Item.Cells(EnumIPP.Status).Text = "Paid"
                Case 10
                    If e.Item.Cells(EnumIPP.PSDSent).Text <> "" Then
                        e.Item.Cells(EnumIPP.Status).Text = "Draft[R]"
                    Else
                        e.Item.Cells(EnumIPP.Status).Text = "Draft"
                    End If
                Case 11
                    e.Item.Cells(EnumIPP.Status).Text = "Finance Verified"
                Case 12
                    e.Item.Cells(EnumIPP.Status).Text = "Finance Approved"
                Case 13
                    e.Item.Cells(EnumIPP.Status).Text = "FM Approved"
                Case 14
                    e.Item.Cells(EnumIPP.Status).Text = "Rejected"
                Case 15
                    e.Item.Cells(EnumIPP.Status).Text = "Void"
                Case 16
                    e.Item.Cells(EnumIPP.Status).Text = "Submitted"
                Case 17
                    e.Item.Cells(EnumIPP.Status).Text = "Department Approved"
                Case 18
                    e.Item.Cells(EnumIPP.Status).Text = "E2P Verified" 'Zulham 13072018 - PAMB
                    'Zulham 18/08/2017 - IPP Stage 3
                Case 3
                    e.Item.Cells(EnumIPP.Status).Text = "Approved"
                Case 6
                    e.Item.Cells(EnumIPP.Status).Text = "Billed"
                    ''
                'Zulham 18072018 - PAMB
                Case 19
                    e.Item.Cells(EnumIPP.Status).Text = "PSD Received"
                    'End
            End Select

            'Zulham 21112018
            Select Case e.Item.Cells(EnumIPP.PayMethod).Text
                Case "IBG"
                    e.Item.Cells(EnumIPP.PayMethod).Text = "LOCAL BANK TRANSFER-(RM)"
                Case "TT"
                    e.Item.Cells(EnumIPP.PayMethod).Text = "TELEGRAPHIC TRANSFER-(FOREIGN CURRENCY)"
                Case "BC"
                    e.Item.Cells(EnumIPP.PayMethod).Text = "CHEQUE-(RM)"
                Case "BD"
                    e.Item.Cells(EnumIPP.PayMethod).Text = "BANK DRAFT-(FOREIGN CURRENCY)"
                Case "CO"
                    e.Item.Cells(EnumIPP.PayMethod).Text = "CASHIER'S ORDER-(RM)"
            End Select

        End If

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgIPPList.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim docsd, doced, paysd, payed, psdsentsd, psdsented As Date

        If txtDocStartDate.Value = "" And txtDocEndDate.Value = "" And txtPayStartDate.Value = "" And txtPayEndDate.Value = "" And txtDocNo.Text = "" And txtPayAdv.Text = "" And txtVendor.Value = "" And txtVendorAddr.Value = "" And txtPSDSentStartDate.Value = "" And txtPSDSentEndDate.Value = "" Then
            Common.NetMsgbox(Me, "Please enter either the Document Date Range or the Payment Date Range or PSD Sent Date Range")
            Exit Sub
        End If

        If txtDocStartDate.Value <> "" Then
            docsd = txtDocStartDate.Value
        End If
        If txtDocEndDate.Value <> "" Then
            doced = txtDocEndDate.Value
        End If
        If txtPayStartDate.Value <> "" Then
            paysd = txtPayStartDate.Value
        End If
        If txtPayEndDate.Value <> "" Then
            payed = txtPayEndDate.Value
        End If
        If txtPSDSentStartDate.Value <> "" Then
            psdsentsd = txtPSDSentStartDate.Value
        End If
        If txtPSDSentEndDate.Value <> "" Then
            psdsented = txtPSDSentEndDate.Value
        End If
        If txtDocStartDate.Value <> "" And txtDocEndDate.Value <> "" And txtPayStartDate.Value <> "" And txtPayEndDate.Value <> "" And txtPSDSentStartDate.Value <> "" And txtPSDSentEndDate.Value <> "" Then
            Common.NetMsgbox(Me, "Can only enter either the Document Date Range or the Payment Date Range or PSD Sent Date Range")
            dtgIPPList.DataBind()
            btnExcel.Visible = False
        ElseIf txtDocStartDate.Value <> "" And txtDocEndDate.Value <> "" And txtPayStartDate.Value <> "" And txtPayEndDate.Value <> "" Then
            Common.NetMsgbox(Me, "Can only enter either the Document Date Range or the Payment Date Range")
            dtgIPPList.DataBind()
            btnExcel.Visible = False
        ElseIf txtDocStartDate.Value <> "" And txtDocEndDate.Value <> "" And txtPSDSentStartDate.Value <> "" And txtPSDSentEndDate.Value <> "" Then
            Common.NetMsgbox(Me, "Can only enter either the Document Date Range or PSD Sent Date Range")
            dtgIPPList.DataBind()
            btnExcel.Visible = False
        ElseIf txtPayStartDate.Value <> "" And txtPayEndDate.Value <> "" And txtPSDSentStartDate.Value <> "" And txtPSDSentEndDate.Value <> "" Then
            Common.NetMsgbox(Me, "Can only enter either the Payment Date Range or PSD Sent Date Range")
            dtgIPPList.DataBind()
            btnExcel.Visible = False
        ElseIf (DateDiff("m", docsd, doced) > 6) Or (DateDiff("m", paysd, payed) > 6) Or (DateDiff("m", psdsentsd, psdsented) > 6) Then
            Common.NetMsgbox(Me, "Date range must be within 6 months.")
            dtgIPPList.DataBind()
            btnExcel.Visible = False
        Else
            Bindgrid()
        End If
    End Sub

    Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        txtDocStartDate.Value = ""
        txtDocEndDate.Value = ""
        txtPayStartDate.Value = ""
        txtPayEndDate.Value = ""
        'ddlDocType.SelectedIndex = 0
        txtPayAdv.Text = ""
        txtDocNo.Text = ""
        txtVendor.Value = ""

        Me.txtPSDSentEndDate.Value = ""
        Me.txtPSDSentStartDate.Value = ""
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
        Dim vtypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=IPPCOY&frm=ippenq")


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
        Dim objIPP As New IPP
        Dim ds As New DataSet
        Dim i As Integer
        Dim docstatus As String = ""
        Dim doctype, docno, docsdt, docedt, strVen, paysdt, payedt, payadv, strVenAddr As String
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
        If txtDocStartDate.Value <> "" And txtPayStartDate.Value = "" Then
            docsdt = txtDocStartDate.Value
        End If
        If txtDocEndDate.Value <> "" And txtPayEndDate.Value = "" Then
            docedt = txtDocEndDate.Value
        End If
        If txtDocStartDate.Value = "" And txtPayStartDate.Value <> "" Then
            paysdt = txtPayStartDate.Value
        End If
        If txtDocEndDate.Value = "" And txtPayEndDate.Value <> "" Then
            payedt = txtPayEndDate.Value
        End If
        If txtPayAdv.Text <> "" Then
            payadv = txtPayAdv.Text
        End If
        For i = 0 To chkdocstatus.Items.Count - 1
            If chkdocstatus.Items(i).Selected Then
                docstatus &= chkdocstatus.Items(i).Value & ","
            End If
        Next
        If docstatus <> "" Then
            docstatus = docstatus.Substring(0, docstatus.Length - 1)
        End If

        'ds = objIPP.PopulateIPPEnqList(docno, payadv, docstatus, docsdt, docedt, paysdt, payedt, strVen, strVenAddr)
        Dim Roles As New ArrayList : Roles = Session("MixUserRole")
        For _count As Integer = 0 To Roles.Count - 1
            If Roles(_count).ToString = "IPP Officer" Or Roles(_count).ToString = "IPP Approving Officer" Then 'of source dept
                'Get the dept code for this particular user
                Dim objdb As New EAD.DBCom
                Dim deptCode = objdb.GetVal("select um_dept_id from user_mstr where um_user_id='" & Session("UserId") & "' and um_coy_id = '" & Session("CompanyId") & "'")
                ds = objIPP.PopulateIPPEnqList(docno, payadv, docstatus, docsdt, docedt, paysdt, payedt, strVen, strVenAddr, deptCode, txtPSDSentStartDate.Value, txtPSDSentEndDate.Value, ViewState("venidx"))
            Else 'of finance;no restrictions
                ds = objIPP.PopulateIPPEnqList(docno, payadv, docstatus, docsdt, docedt, paysdt, payedt, strVen, strVenAddr, "", txtPSDSentStartDate.Value, txtPSDSentEndDate.Value, ViewState("venidx"))
            End If
        Next

        strFileName = "IPP Document Enquiry Search Result" & "(" & Format(Now, "ddMMMyyyy") & ").xls"
        Dim attachment As String = "attachment;filename=" & strFileName
        Response.ClearContent()
        Response.AddHeader("Content-Disposition", attachment)
        Response.ContentType = "application/vnd.ms-excel"

        Dim dc As DataColumn

        Response.Write("Document No.") '0
        Response.Write(vbTab + "Sub-Document No.") '1
        Response.Write(vbTab + "Teller ID") '2
        Response.Write(vbTab + "Document Date") '3
        Response.Write(vbTab + "Vendor") '4
        Response.Write(vbTab + "Bank Code") '5
        Response.Write(vbTab + "Bank Account") '6
        Response.Write(vbTab + "Currency") '7
        Response.Write(vbTab + "Sub-Document Payment Amount") '8
        Response.Write(vbTab + "Payment Amount") '9
        Response.Write(vbTab + "PSD Sent Date") '10
        Response.Write(vbTab + "Payment Method") '11
        Response.Write(vbTab + "Payment Date") '12
        Response.Write(vbTab + "Payment Advice No.") '13
        Response.Write(vbTab + "Status") '14
        Response.Write(vbCrLf)

        Dim dr As DataRow
        For Each dr In ds.Tables(0).Rows
            For i = 0 To ds.Tables(0).Columns.Count - 1
                If i = 0 Then
                    Response.Write(dr.Item(i + 1).ToString)
                End If
                If (i >= 1) And (i <= 13) Then
                    Response.Write(vbTab + dr.Item(i + 1).ToString)
                End If
                If i = 14 Then
                    If dr.Item(i + 1).ToString = "4" Then
                        Response.Write(vbTab + "Paid")
                    ElseIf dr.Item(i + 1).ToString = "10" Then
                        Response.Write(vbTab + "Draft")
                    ElseIf dr.Item(i + 1).ToString = "11" Then
                        Response.Write(vbTab + "PSD Received")
                    ElseIf dr.Item(i + 1).ToString = "12" Then
                        Response.Write(vbTab + "Finance Verified")
                    ElseIf dr.Item(i + 1).ToString = "13" Then
                        Response.Write(vbTab + "Approved")
                    ElseIf dr.Item(i + 1).ToString = "14" Then
                        Response.Write(vbTab + "Rejected")
                    ElseIf dr.Item(i + 1).ToString = "15" Then
                        Response.Write(vbTab + "Void")
                    ElseIf dr.Item(i + 1).ToString = "16" Then
                        Response.Write(vbTab + "Submitted")
                    ElseIf dr.Item(i + 1).ToString = "17" Then
                        Response.Write(vbTab + "PSD Sent")
                    ElseIf dr.Item(i + 1).ToString = "18" Then
                        Response.Write(vbTab + "IPP Verified")
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