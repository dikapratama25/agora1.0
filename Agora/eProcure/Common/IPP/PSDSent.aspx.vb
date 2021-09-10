Imports eProcure.Component
Imports AgoraLegacy

Partial Public Class PSDSent
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objdb As New EAD.DBCom
    Dim objGlobal As New AppGlobals

    Public Enum EnumIPP
        DocNo = 2
        DocType = 3
        DocDate = 4
        Vendor = 6
        BankCode = 7
        BankAccount = 8
        Currency = 9
        PayAmount = 10
        PayMethod = 11
        PayDate = 13
        Status = 12
        Prcs = 15
        Indicator = 15
    End Enum
    Public Enum EnumInv
        icChk = 0
        icInvNo = 1
        icInvType = 2
        icIndex = 3
        icPayDate = 4
        icPoIndex = 5
        icVenId = 6
        icVenName = 7
        icCurrency = 8
        icAmount = 9
        icDoc = 10
        icPayTerm = 11
        icBuyer = 12
        icDept = 13
        icPrinted = 14
        icStatusCode = 15
        icStatus = 16
        icRemarks = 17
        icAppDate = 18
        icBillMethod = 19
        icPymtMethod = 20
        icDocType = 21
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objIPP As New IPP
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgIPPList)
        Session("dsIPPDocDetail") = Nothing
        Session("dsIPPDocDetailCount") = Nothing
        Session("urlreferer") = "IPPList"
        If Not IsPostBack Then
            GenerateTab()
            PopulateTypeAhead()
            Bindgrid()
            Me.txtPRCSSentDate.Value = String.Format("{0:dd/MM/yyyy}", Date.Now) 'date will be defaulted to the current date
        End If
        objIPP = Nothing
        cmdSave.Attributes.Add("onclick", "checkHeader();")
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objIPP As New IPP
        Dim ds As New DataSet
        Dim i As Integer
        Dim docstatus As String = ""
        Dim doctype, docno, docsdt, docedt, strVen As String
        If txtVendor.Value <> "" Then
            strVen = Common.Parse(txtVendor.Value)
        End If
        If txtDocNo.Text <> "" Then
            docno = txtDocNo.Text
            docno = docno.Replace("\", "\\\\") 'Zulham 13072018
        End If
        If txtDocStartDate.Value <> "" Then
            docsdt = txtDocStartDate.Value
        End If
        If txtDocEndDate.Value <> "" Then
            docedt = txtDocEndDate.Value
        End If
        If ddlDocType.SelectedIndex <> 0 Then
            doctype = ddlDocType.SelectedItem.Value
        End If
        If docstatus <> "" Then
            docstatus = docstatus.Substring(0, docstatus.Length - 1)
        End If

        ds = objIPP.PopulatePSDList(docno, doctype, docsdt, docedt, strVen)

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
        Else
            dtgIPPList.DataSource = dvViewSample
            dtgIPPList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
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
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If

    End Sub
    Sub dtgIPPList_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgIPPList.PageIndexChanged
        dtgIPPList.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub dtgIPPList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPPList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dvMatch As DataView = dv.DataView
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            'Zulham 12072018 - PAMB
            Select Case e.Item.Cells(EnumIPP.Status).Text
                Case 10
                    e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "IPPEntry.aspx", "mode=modify&urlrefereripp=" & Session("urlreferer") & "&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO")) & "&Ind=" & dv("IND")) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
                Case 14
                    e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "IPPEntry.aspx", "mode=modify&status=reject&urlrefereripp=" & Session("urlreferer") & "&index=" & Server.UrlEncode(dv("IM_INVOICE_INDEX")) & "&DocumentNo=" & dv("IM_INVOICE_NO") & "&Ind=" & dv("IND")) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
                Case Else
                    e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "IPPDocument.aspx", "Frm=PSDSent&index=" & dv("IM_INVOICE_INDEX") & "&CreatedBy=" & dv("IM_CREATED_BY") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
            End Select
            e.Item.Cells(EnumIPP.DocDate).Text = e.Item.Cells(EnumIPP.DocDate).Text.Substring(0, 10)
            e.Item.Cells(EnumIPP.PayAmount).Text = Format(dv("IM_INVOICE_TOTAL"), "#,##0.00")
            e.Item.Cells(EnumIPP.PayDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("IM_payment_date"))
            Select Case e.Item.Cells(EnumIPP.Status).Text
                Case 4
                    e.Item.Cells(EnumIPP.Status).Text = "Paid"
                Case 10
                    e.Item.Cells(EnumIPP.Status).Text = "Draft"
                Case 11, 12
                    e.Item.Cells(EnumIPP.Status).Text = "Submitted"
                Case 13
                    e.Item.Cells(EnumIPP.Status).Text = "Approved"
                Case 14
                    e.Item.Cells(EnumIPP.Status).Text = "Rejected"
                Case 15
                    e.Item.Cells(EnumIPP.Status).Text = "Void"
            End Select

            Select Case e.Item.Cells(EnumIPP.DocType).Text
                Case "INV"
                    e.Item.Cells(EnumIPP.DocType).Text = "Invoice"
                Case "BILL"
                    e.Item.Cells(EnumIPP.DocType).Text = "Bill"
                Case "CN"
                    e.Item.Cells(EnumIPP.DocType).Text = "Credit Note"
                Case "DN"
                    e.Item.Cells(EnumIPP.DocType).Text = "Debit Note"
            End Select

        End If

    End Sub
    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgIPPList.CurrentPageIndex = 0
        Bindgrid()
    End Sub
    Private Sub GenerateTab()
        'no tabs needed
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        dtgIPPList.CurrentPageIndex = 0
        Bindgrid()
    End Sub
    Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        txtDocStartDate.Value = ""
        txtDocEndDate.Value = ""
        ddlDocType.SelectedIndex = 0
        txtDocNo.Text = ""
        txtVendor.Value = ""
        Dim chkbox As ListItem
    End Sub
    Private Sub cmdSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelectAll.Click
        Dim chkbox As ListItem
    End Sub

    Sub PopulateTypeAhead()
        Dim typeahead As String
        Dim i, count As Integer
        Dim content, content2 As String
        Dim strCompID As String
        Dim vtypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=IPPCOY")


        content &= "$(""#txtVendor"").autocomplete(""" & vtypeahead & """, {" & vbCrLf & _
        "width: 200," & vbCrLf & _
        "scroll: true," & vbCrLf & _
        "selectFirst: false" & vbCrLf & _
        "});" & vbCrLf


        typeahead = "<script language=""javascript"">" & vbCrLf & _
      "<!--" & vbCrLf & _
        "$(document).ready(function(){" & vbCrLf & _
        content & vbCrLf & _
        "});" & vbCrLf & _
        "-->" & vbCrLf & _
        "</script>"
        Session("typeahead2") = typeahead
    End Sub

    Private Sub cmdSavePrcsDate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSavePrcsDate.Click
        RecordPSDSentDate()
    End Sub
    Public Function RecordPSDSentDate() As DataTable
        'Do some checking here
        'Conditions:
        'selected date that is = public hols is unacceptable
        Dim dtHols As New DataTable
        dtHols.Columns.Add("Date")
        Dim objIPP As New IPP
        Dim dsHols As DataSet : dsHols = objIPP.GetHolidays
        dsHols.Tables.Add(dtHols)
        If dtHols.Rows.Count > 0 Then
            For Each item As DataRow In dtHols.Rows
                If String.Format("{0:dd/MM/yyyy}", item("Date")) = Me.txtPRCSSentDate.Value Then
                    Dim strJScript As String
                    strJScript = "<script language=javascript>"
                    strJScript += "window.alert('PSD Sent Date must be a working day.');"
                    strJScript += "</script>"
                    Response.Write(strJScript)
                    Exit Function
                End If
            Next
        End If
        'selected date must be later than current date
        Dim selectedDt As New DateTime
        selectedDt = txtPRCSSentDate.Value
        If selectedDt < String.Format("{0:dd/MM/yyyy}", Date.Now) Then
            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.alert('PSD Sent Date cannot be lesser than current date.');"
            strJScript += "</script>"
            Response.Write(strJScript)
            Exit Function
        End If
        'if selected date fell on weekend
        If selectedDt.DayOfWeek = 6 Or selectedDt.DayOfWeek = 7 Then
            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.alert('PSD Sent Date must be a working day.');"
            strJScript += "</script>"
            Response.Write(strJScript)
            Exit Function
        End If

        'Zulham July 29, 2013
        'Selected date must be either today's date or 1 day later
        Dim _0 = Format(Date.Now, "dd/MM/yyyy")
        Dim _count As Integer = 0
        For Each item As DataRow In dtHols.Rows
            Dim _dt2 = String.Format("{0:dd/MM/yyyy}", item("hm_date"))
            For i As Integer = 0 To dtHols.Rows.Count - 1
                Dim _dt3 = DateAdd(DateInterval.Day, i + 1, _0)
                If DateAdd(DateInterval.Day, i + 1, _0) = _dt2 Then
                    _count += 1
                End If
            Next
        Next
        If DateDiff(DateInterval.Day, _0, selectedDt) > 1 Then
            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.alert('PSD Sent Date must be either current date or + 1 working day.');"
            strJScript += "</script>"
            Response.Write(strJScript)
            Exit Function
        ElseIf DateDiff(DateInterval.Day, _0, selectedDt) > 4 And Now.DayOfWeek = 5 Then
            'Fri, Sat, Sun, Mon - Date valid until Monday only
            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.alert('PSD Sent Date must be either current date or + 1 working day.');"
            strJScript += "</script>"
            Response.Write(strJScript)
            Exit Function
        ElseIf _count <> 0 Then 'Some hols 
            Dim _dtAdded As Date : _dtAdded = DateAdd(DateInterval.Day, _count + 1, _0)
            If _dtAdded.DayOfWeek = DayOfWeek.Saturday Or _dtAdded.DayOfWeek = DayOfWeek.Sunday Then 'add two more days (sat + sun)
                _dtAdded = DateAdd(DateInterval.Day, 2, _dtAdded)
            End If
            If selectedDt > _dtAdded Then
                Dim strJScript As String
                strJScript = "<script language=javascript>"
                strJScript += "window.alert('PSD Sent Date must be either current date or + 1 working day.');"
                strJScript += "</script>"
                Response.Write(strJScript)
            End If
        End If
        'End

        Dim dtr As DataRow
        Dim strInv As String = ""
        Dim strInvFail As String = "", strType As String = ""
        Dim chk As CheckBox
        Dim bln As Boolean
        Dim dt As DataTable : dt = Me.getInvDt()

        For Each dgItem As DataGridItem In dtgIPPList.Items
            chk = dgItem.FindControl("chkSelection")
            Dim cboPay As DropDownList
            If chk.Checked Then

                'Zulham 19/02/2016 - IPP Stage 4 Phase 2
                'CN/DN can't precede the invoice it's tied to
                'Zulham 05/01/2016 - IPP STAGE 4 Phase 2
                'check for invoices status that are tied to CN/DN
                Dim objippmain As New IPPMain
                Dim vendID = objdb.GetVal("select im_s_Coy_id from invoice_mstr where im_invoice_index = '" & dgItem.Cells(1).Text & "'")
                Dim invStatus = objdb.GetVal("select im_invoice_status from invoice_mstr where im_invoice_index = '" & dgItem.Cells(1).Text & "'")
                Dim docNo = objdb.GetVal("select im_invoice_no from invoice_mstr where im_invoice_index = '" & dgItem.Cells(1).Text & "'")
                If objippmain.checkStatus(docNo.ToString.Trim, vendID, invStatus) = False Then
                    Common.NetMsgbox(Me, "Credit Note/Debit Note cannot precede the invoices it's tied to.", MsgBoxStyle.Exclamation)
                    Exit Function
                End If

                dgItem.Cells(5).Text = String.Format(Me.txtPRCSSentDate.Value.ToString, "dd/MM/yyyy")

                Dim row As DataRow = dt.NewRow
                row("IM_INVOICE_NO") = dgItem.Cells(2).Text
                row("IM_INVOICE_INDEX") = dgItem.Cells(1).Text
                row("IM_PRCS_SENT") = dgItem.Cells(5).Text
                If dgItem.Cells(14).Text <> "&nbsp;" Then row("IM_PRCS_RECV") = dgItem.Cells(14).Text Else row("IM_PRCS_RECV") = ""
                row("IND") = dgItem.Cells(EnumIPP.Indicator).Text
                dt.Rows.Add(row)

                'chk.Checked = False
            End If

            Me.Session.Add("invList", strInv)
            If strInvFail <> "" Then
                Me.Session.Add("invListFail", strInvFail)
            End If
        Next
        dt.AcceptChanges()
        Session("dtInvoice") = dt
        Return dt
        'table3.Visible = False
    End Function
    Private Sub cmdPickRecord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPickRecord.Click
        If Not Me.dtgIPPList.Items.Count = 0 Then
            PopulateSentDate()
        End If
    End Sub
    Private Sub cmdMassPickRecord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMassPickRecord.Click
        If Not Me.dtgIPPList.Items.Count = 0 Then
            Session("MassUpdate") = True
            PopulateSentDate()
        End If
    End Sub
    Public Sub PopulateSentDate()

        Dim chk As CheckBox

        Dim counter As Integer = 0
        For Each item As DataGridItem In dtgIPPList.Items
            chk = item.FindControl("chkSelection")
            If chk.Checked Then
                counter += 1
            End If
            If counter > 1 And Session("MassUpdate") = False Then
                Exit Sub
            End If
        Next

        If counter = 0 Then
            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.alert('Please select a record!');"
            strJScript += "</script>"
            Response.Write(strJScript)
            Exit Sub
        Else
            Me.txtPRCSSentDate.Value = String.Format("{0:dd/MM/yyyy}", Date.Now) 'date will be defaulted to the current date
            table3.Visible = True
        End If

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        'Codes to insert record into db       
        'RecordPSDSentDate()
        Dim dt As New DataTable
        Dim ds As New DataSet

        If RecordPSDSentDate() Is Nothing Then 'For failed validation
            Exit Sub
        Else
            dt = RecordPSDSentDate()
        End If

        Dim ologicIPP As New IPP
        Dim strInv, strMsg2 As String
        Dim chk As CheckBox
        For Each row As DataRow In dt.Rows
            'skip this if the received date has been recorded
            
            Dim sentDate = ""
            If row("IM_PRCS_RECV").ToString = "" Then
                'Dim convDate As String : convDate = CType(row("IM_PRCS_SENT"), Date).Year & "-" & CType(row("IM_PRCS_SENT"), Date).Month & "-" & CType(row("IM_PRCS_SENT"), Date).Day & " 00:00:00"
                If ologicIPP.UpdatePRCSSentDate(CType(row("IM_INVOICE_INDEX"), Integer), row("IM_PRCS_SENT")) = True Then
                    strInv &= strInv & objdb.GetVal("select im_invoice_no from invoice_mstr where im_invoice_index = '" & CType(row("IM_INVOICE_INDEX"), Integer) & "' ") & ","
                End If

                If row("IND") = "Y" Then
                    ologicIPP.AddAuditTrailRecord(CType(row("IM_INVOICE_INDEX"), Integer), "IPP Teller", "Enter PSD Sent Date")
                Else
                    ologicIPP.AddAuditTrailRecord(CType(row("IM_INVOICE_INDEX"), Integer), "IPP Verifier", "Enter PSD Sent Date")
                End If

                For Each dgItem As DataGridItem In dtgIPPList.Items
                    chk = dgItem.FindControl("chkSelection")
                    Dim cboPay As DropDownList
                    If chk.Checked Then
                        chk.Checked = False
                    End If
                Next

            Else
                Dim strJScript As String
                strJScript = "<script language=javascript>"
                strJScript += "window.alert('Cannot change the sent date as the received date has been entered!');"
                strJScript += "</script>"
                Response.Write(strJScript)
                Exit Sub
            End If

        Next

                dt.AcceptChanges()
                'ds.Tables.Add(dt)
                'Me.Session.Add("invSucessUpdatePSDRecv", UpdatePSDRecv)
                If strInv <> "" Then
                    strInv = strInv.Substring(0, strInv.Length - 1)
                    ' Me.Session.Add("invListFail", strInv)
            strMsg2 = objGlobal.GetErrorMessage("00358")
                    Common.NetMsgbox(Me, strMsg2, MsgBoxStyle.Information)
                Else
            strMsg2 = objGlobal.GetErrorMessage("00359")
                    Common.NetMsgbox(Me, strMsg2, MsgBoxStyle.Information)
                End If
                strInv = ""
                Session("dtInvoice") = Nothing
    End Sub

    Private Function bindInv(ByVal strType As String, ByVal blnSend As Boolean, ByVal blnFMMassAppr As Boolean)
        Dim dtItem As New DataTable
        dtItem = getInvDt()

        Dim dtr As DataRow
        Dim strInv As String = ""
        Dim strInvFail As String = ""

        Dim chk As CheckBox
        Dim bln As Boolean
        Dim dt As DateTime
        For Each dgItem As DataGridItem In dtgIPPList.Items
            chk = dgItem.FindControl("chkSelection")
            Dim cboPay As DropDownList
            If chk.Checked Or (strType = "1" And chk.Checked = False) Then
                dgItem.Cells(5).Text = Format("{0}:dd/MM/yyyy", Me.txtPRCSSentDate.ToString)
            End If
            Me.Session.Add("invList", strInv)
            If strInvFail <> "" Then
                Me.Session.Add("invListFail", strInvFail)
            End If
        Next

    End Function

    Public Function getInvDt() As DataTable
        Dim dtitem As New DataTable        
        'If Session("dtInvoice") Is Nothing Then
        dtitem.Columns.Add("IM_INVOICE_INDEX", Type.GetType("System.String"))
        dtitem.Columns.Add("IM_INVOICE_NO", Type.GetType("System.String"))
        dtitem.Columns.Add("IM_INVOICE_TYPE", Type.GetType("System.Int32"))
        dtitem.Columns.Add("IM_DOC_DATE", Type.GetType("System.String"))
        dtitem.Columns.Add("IM_PRCS_SENT", Type.GetType("System.String"))
        dtitem.Columns.Add("IM_PRCS_RECV", Type.GetType("System.String"))
        dtitem.Columns.Add("IM_S_COY_NAME", Type.GetType("System.String"))
        dtitem.Columns.Add("IM_BANK_CODE", Type.GetType("System.String"))
        dtitem.Columns.Add("IM_BANK_ACCT", Type.GetType("System.Int32"))
        dtitem.Columns.Add("IM_CURRENCY_CODE", Type.GetType("System.String"))
        dtitem.Columns.Add("IM_INVOICE_TOTAL", Type.GetType("System.String"))
        dtitem.Columns.Add("IM_PAYMENT_TERM", Type.GetType("System.String"))
        dtitem.Columns.Add("IND", Type.GetType("System.String"))
        Session("dtInvoice") = dtitem
        'Else
        '    dtitem = Session("dtInvoice")
        'End If

        Return dtitem

    End Function

End Class