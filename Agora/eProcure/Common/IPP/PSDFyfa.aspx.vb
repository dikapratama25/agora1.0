Imports eProcure.Component
Imports AgoraLegacy

Partial Public Class PSDFyfa
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

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
            docno = docno.Replace("\", "\\\\")
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

        ds = objIPP.PopulateFyfa(docno, doctype, docsdt, docedt, strVen)

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
        'If e.Item.ItemType = ListItemType.Header Then
        '    Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
        '    chkAll.Attributes.Add("onclick", "selectAll();")
        'End If

    End Sub
    Sub dtgIPPList_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgIPPList.PageIndexChanged
        dtgIPPList.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub dtgIPPList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPPList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dvMatch As DataView = dv.DataView
            'Zulham 12072018 - PAMB
            Select Case e.Item.Cells(EnumIPP.Status).Text
                Case 10
                    e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "IPPEntry.aspx", "mode=modify&urlrefereripp=" & Session("urlreferer") & "&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
                Case 14
                    e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "PSDAcceptanceDetails.aspx", "Frm=PSDFyFa&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
                Case Else
                    e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "PSDAcceptanceDetails.aspx", "Frm=PSDRecv&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
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
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_IPP_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "PSDRECV.aspx", "mode=new&type=Listing&pageid=" & strPageId) & """><span>PSD Acceptance List</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "PSDAcceptReject.aspx", "mode=new&type=Listing&pageid=" & strPageId) & """><span>PSD Accepted/Rejected Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPP", "PSDFyfa.aspx", "type=Listing&mode=new&pageid=" & strPageId) & """><span>Pending FYFA</span></a></li>" & _
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
    'Private Sub cmdSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelectAll.Click
    '    Dim chkbox As ListItem
    'End Sub

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

    Private Function bindInv(ByVal strType As String, ByVal blnSend As Boolean, ByVal blnFMMassAppr As Boolean)
        Dim dtItem As New DataTable
        dtItem = getInvDt()

        Dim dtr As DataRow
        Dim strInv As String = ""
        Dim strInvFail As String = ""

    End Function

    Public Function getInvDt() As DataTable
        Dim dtitem As New DataTable
        If Session("dtInvoice") Is Nothing Then
            dtitem.Columns.Add("IM_INVOICE_INDEX", Type.GetType("System.String"))
            dtitem.Columns.Add("IM_INVOICE_NO", Type.GetType("System.String"))
            dtitem.Columns.Add("IM_INVOICE_TYPE", Type.GetType("System.Int32"))
            dtitem.Columns.Add("IM_DOC_DATE", Type.GetType("System.String"))
            dtitem.Columns.Add("IM_PRCS_SENT", Type.GetType("System.String"))
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
End Class