Imports eProcure.Component
Imports AgoraLegacy


Partial Public Class PSDRecv
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    'Protected WithEvents txtDocStartDate As System.Web.UI.HtmlControls.HtmlInputText
    'Protected WithEvents txtDocEndDate As System.Web.UI.HtmlControls.HtmlInputText
    'Protected WithEvents txtPRCSSentDate As System.Web.UI.HtmlControls.HtmlInputText

    Dim objGlobal As New AppGlobals
    Dim objdb As New EAD.DBCom
    Public Enum EnumIPP
        DocNo = 2
        DocType = 3
        DocDate = 4
        SentDate = 5
        CreatedBy = 6
        ReceiveDate = 7
        Vendor = 8
        BankCode = 9
        BankAccount = 10
        Currency = 11
        PayAmount = 12
        PayMethod = 13
        Status = 14
        PayDate = 15
        'PayAdviceNo = 13

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
            PopulateSentDate()
        End If
        objIPP = Nothing
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
            docno = docno.Replace("\", "\\\\") 'Zulham 13072018 - PAMB
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

        ds = objIPP.PopulatePSDRecv(docno, doctype, docsdt, docedt, strVen)

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
                    e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "IPPEntry.aspx", "mode=modify&urlrefereripp=" & Session("urlreferer") & "&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
                Case 14
                    'e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "IPPEntry.aspx", "mode=modify&status=reject&urlrefereripp=" & Session("urlreferer") & "&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & dv("IM_INVOICE_NO")) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
                    e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "PSDAcceptanceDetails.aspx", "Frm=PSDFyFa&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
                Case Else
                    e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "PSDAcceptanceDetails.aspx", "Frm=PSDRecv&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
            End Select
            If Not e.Item.Cells(EnumIPP.DocDate).Text = "&nbsp;" Then e.Item.Cells(EnumIPP.DocDate).Text = e.Item.Cells(EnumIPP.DocDate).Text.Substring(0, 10)
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
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_IPP_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPP", "PSDRECV.aspx", "mode=new&type=Listing&pageid=" & strPageId) & """><span>PSD Acceptance List</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "PSDAcceptReject.aspx", "mode=new&type=Listing&pageid=" & strPageId) & """><span>PSD Accepted/Rejected Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "PSDFyfa.aspx", "type=Listing&mode=new&pageid=" & strPageId) & """><span>Pending FYFA</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
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
                dgItem.Cells(6).Text = String.Format(Me.txtPRCSSentDate.Value.ToString, "dd/MM/yyyy")
                chk.Checked = False

                Dim row As DataRow = dt.NewRow
                row("IM_INVOICE_INDEX") = dgItem.Cells(1).Text
                row("im_prcs_recv") = dgItem.Cells(6).Text
                dt.Rows.Add(row)

            End If

            Me.Session.Add("invList", strInv)
            If strInvFail <> "" Then
                Me.Session.Add("invListFail", strInvFail)
            End If
        Next

        dt.AcceptChanges()
        Session("dtInvoice") = dt
        'table3.Visible = False

    End Sub
    Private Sub cmdPickRecord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPickRecord.Click
        'PopulateSentDate()
    End Sub
    Private Sub cmdMassPickRecord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMassPickRecord.Click

        Dim chk As CheckBox
        Dim strInv, strInvFail As String
        Dim strMsg2 As String
        'Iterate thru the list, select only with checkbox checked
        Dim counter As Integer = 0
        For Each item As DataGridItem In dtgIPPList.Items
            chk = item.FindControl("chkSelection")
            If chk.Checked Then
                'Zulham June 18, 2013
                Dim strJScript As String
                Session("InvIdx") = item.Cells(1).Text
                Select Case ValidateDate(item.Cells(5).Text)
                    Case "1"
                        strJScript = "<script language=javascript>"
                        strJScript += "window.alert('PSD Received Date cannot be later than today');"
                        strJScript += "</script>"
                        Response.Write(strJScript)
                        Exit Sub
                    Case "2"
                        strJScript = "<script language=javascript>"
                        strJScript += "window.alert('PSD Received Date cannot be earlier than PSD Sent Date');"
                        strJScript += "</script>"
                        Response.Write(strJScript)
                        Exit Sub
                    Case "3"
                        strJScript = "<script language=javascript>"
                        strJScript += "window.alert('PSD Received Date must be a working day');"
                        strJScript += "</script>"
                        Response.Write(strJScript)
                        Exit Sub
                    Case "4", "5"
                        strJScript = "<script language=javascript>"
                        strJScript += "window.alert('PSD Received Date must be a working day');"
                        strJScript += "</script>"
                        Response.Write(strJScript)
                        Exit Sub
                    Case "6"
                        strJScript = "<script language=javascript>"
                        strJScript += "window.alert('Document is allowed to be backdated only 1 working day.');"
                        strJScript += "</script>"
                        Response.Write(strJScript)
                        Exit Sub
                End Select
                'End
                Session("InvIdx") = item.Cells(1).Text
                Session("DocNo") = objdb.GetVal("SELECT DISTINCT IM_Invoice_No FROM invoice_mstr WHERE Im_INVOICE_INDEX = '" & Session("InvIdx") & "' ") 'item.Cells(2).Text
                Session("DocNo") = Session("DocNo").ToString.Replace("\", "\\") 'Zulham 12072018 - PAMB
                'use that pre existing approval method (of ippmain class)
                Dim objIPP As New IPP
                Dim strRemark, strUrl As String
                'Dim objipp As New IPP
                Dim objippmain As New IPPMain
                Dim strMsg As String

                'for checking the payment method TT
                strRemark = FormatAORemark("approve")


                'Zulham 19/02/2016 - IPP Stage 4 Phase 2
                'CN/DN can't precede the invoice it's tied to
                'Zulham 05/01/2016 - IPP STAGE 4 Phase 2
                'check for invoices status that are tied to CN/DN
                Dim vendID = objdb.GetVal("select im_s_Coy_id from invoice_mstr where im_invoice_index = '" & item.Cells(1).Text & "'")
                Dim invStatus = objdb.GetVal("select im_invoice_status from invoice_mstr where im_invoice_index = '" & item.Cells(1).Text & "'")
                Dim docNo = objdb.GetVal("select im_invoice_no from invoice_mstr where im_invoice_index = '" & item.Cells(1).Text & "'")
                If objippmain.checkStatus(docNo, vendID, invStatus) = False Then
                    Common.NetMsgbox(Me, "Credit Note/Debit Note cannot precede the invoices it's tied to.", MsgBoxStyle.Exclamation)
                    Exit Sub
                End If

                'strRemark = strRemark & txtApprRejRemark.Text
                Dim objDoc As New IPPMain
                Dim intApprGrpIndex As String
                Dim objIPPDetails As New IPPDetails
                'Zulham 02012018
                Dim intCurrentStatus As Int16 = 0

                objIPPDetails = objIPP.GetIPPDetails(Session("DocNo"), Common.Parse(HttpContext.Current.Session("CompanyID")), Session("InvIdx"), "PSDReceived")
                intApprGrpIndex = objdb.GetVal("SELECT DISTINCT FA_APPROVAL_GRP_INDEX FROM finance_approval WHERE FA_INVOICE_INDEX = '" & Session("InvIdx") & "' ")

                'Zulham 02012018
                intCurrentStatus = objdb.GetVal("SELECT DISTINCT IM_INVOICE_STATUS FROM INVOICE_MSTR WHERE IM_INVOICE_INDEX = '" & Session("InvIdx") & "' ")

                If objDoc.checkDept() = False Then
                    objIPP.Message(Me, "00046", MsgBoxStyle.Information)
                    Exit Sub
                End If
                If objIPPDetails.PaymentMethod = "TT" And ViewState("role") = 3 Then
                    If objDoc.ApproveIPPDoc(Session("InvIdx"), strRemark, Session("Relief"), "FinanceTeller", objIPPDetails.PaymentMethod) = True Then
                        strMsg = objGlobal.GetErrorMessage("00043") & " " & Session("DocNo") & " " & objGlobal.GetErrorMessage("00044")
                        'strMsg = objGlobal.GetErrorMessage("00360")
                        'Common.NetMsgbox(Me, strMsg, lnkBack.NavigateUrl, MsgBoxStyle.Exclamation)
                    Else
                        strMsg = objGlobal.GetErrorMessage("00007")
                        'strMsg = objGlobal.GetErrorMessage("00361")
                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    End If
                Else
                    If objDoc.CheckBankCode(Session("InvIdx")) Then
                        strMsg = objGlobal.GetErrorMessage("00323")
                        'Common.NetMsgbox(Me, strMsg, lnkBack.NavigateUrl, MsgBoxStyle.Exclamation)
                        Exit Sub
                    End If
                    'Zulham 02012018
                    If objDoc.ApproveIPPDoc(Session("InvIdx"), strRemark, Session("Relief"), "FinanceTeller", objIPPDetails.PaymentMethod, currentStatus:=intCurrentStatus) = True Then

                        'Zulham 03122018
                        objDoc.sendMailToIPPOfficer(Session("InvIdx"), "5", "submit", from:="PSD Received")

                        If objDoc.isHighestLevel(Session("InvIdx")) Then
                            strMsg = objGlobal.GetErrorMessage("00043") & " " & Session("DocNo") & " " & objGlobal.GetErrorMessage("00044")
                            'strMsg = objGlobal.GetErrorMessage("00360")
                        Else
                            strMsg = objGlobal.GetErrorMessage("00043") & " " & Session("DocNo") & " " & objGlobal.GetErrorMessage("00051")
                            ' strMsg = objGlobal.GetErrorMessage("00360")
                        End If
                        'Common.NetMsgbox(Me, strMsg, lnkBack.NavigateUrl, MsgBoxStyle.Exclamation)
                    Else
                        strMsg = objGlobal.GetErrorMessage("00007")
                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    End If
                End If

                'update recv date
                If objIPP.UpdatePRCSRecvDate(Session("InvIdx"), Common.ConvertDate(Me.txtPRCSSentDate.Value)) = True Then
                    'Me.Session.Add("invSucessUpdatePSDRecv", Session("DocNo"))
                    strInv &= strInv & Session("DocNo") & ","
                    'Else
                    '    strInvFail &= strInvFail & Session("DocNo") & ","
                End If

                'add data into trans_log
                objIPP.AddAuditTrailRecord(Session("InvIdx"), "Finance Teller", "Enter PSD Received Date")

            End If
        Next

        'Me.Session.Add("invSucessUpdatePSDRecv", UpdatePSDRecv)
        If strInv <> "" Then
            strInv = strInv.Substring(0, strInv.Length - 1)
            ' Me.Session.Add("invListFail", strInv)
            strMsg2 = objGlobal.GetErrorMessage("00360")
            Common.NetMsgbox(Me, strMsg2, MsgBoxStyle.Information)
        Else
            strMsg2 = objGlobal.GetErrorMessage("00361")
            Common.NetMsgbox(Me, strMsg2, MsgBoxStyle.Information)
        End If
        Bindgrid()
        strInv = ""
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
    Public Sub PopulateSentDate()
        Me.txtPRCSSentDate.Value = String.Format("{0:dd/MM/yyyy}", Date.Now) 'date will be defaulted to the current date
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        'Codes to insert record into db
        Dim ds As New DataSet
        Dim dt As New DataTable : dt = getInvDt()
        Dim ologicIPP As New IPP
        For Each row As DataRow In dt.Rows
            Dim convDate As String : convDate = CType(row("im_prcs_recv"), Date).Year & "-" & CType(row("im_prcs_recv"), Date).Month & "-" & CType(row("im_prcs_recv"), Date).Day & " 00:00:00"
            ologicIPP.UpdatePRCSRecvDate(CType(row("IM_INVOICE_INDEX"), Integer), convDate)
        Next
        dt.AcceptChanges()
        ds.Tables.Add(dt)

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
                dgItem.Cells(6).Text = Format("{0}:dd/MM/yyyy", Me.txtPRCSSentDate.ToString)
            End If
            Me.Session.Add("invList", strInv)
            If strInvFail <> "" Then
                Me.Session.Add("invListFail", strInvFail)
            End If
        Next

    End Function

    Public Function getInvDt() As DataTable
        Dim dtitem As New DataTable
        If Session("dtInvoice") Is Nothing Then
            dtitem.Columns.Add("IM_INVOICE_INDEX", Type.GetType("System.String"))
            dtitem.Columns.Add("IM_INVOICE_NO", Type.GetType("System.String"))
            dtitem.Columns.Add("IM_INVOICE_TYPE", Type.GetType("System.Int32"))
            dtitem.Columns.Add("IM_DOC_DATE", Type.GetType("System.String"))
            dtitem.Columns.Add("im_prcs_recv", Type.GetType("System.String"))
            dtitem.Columns.Add("IM_S_COY_NAME", Type.GetType("System.String"))
            dtitem.Columns.Add("IM_BANK_CODE", Type.GetType("System.String"))
            dtitem.Columns.Add("IM_BANK_ACCT", Type.GetType("System.Int32"))
            dtitem.Columns.Add("IM_CURRENCY_CODE", Type.GetType("System.String"))
            dtitem.Columns.Add("IM_INVOICE_TOTAL", Type.GetType("System.String"))
            dtitem.Columns.Add("IM_PAYMENT_TERM", Type.GetType("System.String"))
            Session("dtInvoice") = dtitem
        Else
            dtitem = Session("dtInvoice")
        End If

        Return dtitem

    End Function
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

        'Dim dtHols As New DataTable
        'dtHols.Columns.Add("Date")
        Dim objIPP As New IPP
        Dim dsHols As New DataSet
        dsHols = objIPP.GetHolidays
        'dsHols.Tables.Add(dtHols)
        If dsHols.Tables(0).Rows.Count > 0 Then
            For Each item As DataRow In dsHols.Tables(0).Rows
                If String.Format("{0:dd/MM/yyyy}", item("hm_date")) = Me.txtPRCSSentDate.Value Then
                    Return "3"
                End If
            Next
        End If

        'if selected date fell on weekend
        If selectedDt.DayOfWeek = 6 Or selectedDt.DayOfWeek = 0 Then
            Return "4"
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
                If Not (BackDt.DayOfWeek = DayOfWeek.Saturday Or BackDt.DayOfWeek = DayOfWeek.Sunday) Then
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
                End If
            Next

            If blnValid = False Then
                 Return "5"
            Else
                If selectedDt <> BackDt Then
                    Return "6"
                End If
            End If
        End If

    End Function
End Class