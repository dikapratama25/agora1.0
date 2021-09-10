Imports eProcure.Component
Imports AgoraLegacy

Partial Public Class BCNo
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objdb As New EAD.DBCom
    Dim objGlobal As New AppGlobals

    Public Enum EnumIPP
        DocNo = 2
        DocType = 3
        DocDate = 4
        Vendor = 5
        Currency = 6
        PayAmount = 7
        PayDate = 8
        PayNo = 9
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
            PopulateTypeAhead()
            Bindgrid()
        End If
        objIPP = Nothing

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objIPP As New IPP
        Dim ds As New DataSet
        Dim i As Integer
        Dim docstatus As String = ""
        Dim doctype, docno, docsdt, docedt, strVen As String

        ViewState("venidx") = hid6.Value

        If txtVendor.Value <> "" Then
            strVen = Common.Parse(txtVendor.Value)
        End If
        If txtDocNo.Text <> "" Then
            docno = txtDocNo.Text
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

        ds = objIPP.PopulateBCList(docno, doctype, docsdt, docedt, strVen, ViewState("venidx"))

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
            'Me.txtBCNo.Attributes.Add("onkeypress", "return isNumberKey(event);")
            e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "IPPDocument.aspx", "Frm=EnterBC&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & dv("IM_INVOICE_NO")) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
            e.Item.Cells(EnumIPP.DocDate).Text = e.Item.Cells(EnumIPP.DocDate).Text.Substring(0, 10)
            e.Item.Cells(EnumIPP.PayAmount).Text = Format(dv("IM_INVOICE_TOTAL"), "#,##0.00")
            e.Item.Cells(EnumIPP.PayDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("IM_payment_date"))
            CType(e.Item.FindControl("txtBCNo"), TextBox).Text = dv("im_cheque_no").ToString
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
        "});" & vbCrLf & _
        "$(""#txtVendor"").result(function(event,data,item) {" & vbCrLf & _
        "if (data)" & vbCrLf & _
        "$(""#hid6"").val(data[1]);" & vbCrLf & _
        "$(""#btnGetAdd"").trigger('click');" & vbCrLf & _
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

    Private Sub btnsAve_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim _IPP As New IPP
        Dim strInv, strMsg2 As String

        For Each dtgItem As DataGridItem In Me.dtgIPPList.Items
            If CType(dtgItem.FindControl("txtBCNo"), TextBox).Text <> "" Then
                'Nothing happened? Better start inserting the data into the respective table.
                If _IPP.EditBankersCheque(CType(dtgItem.FindControl("txtBCNo"), TextBox).Text, CType(dtgItem.Cells(1).Text, String)) = True Then
                    strInv &= strInv & objdb.GetVal("select im_invoice_no from invoice_mstr where im_invoice_index = '" & CType(dtgItem.Cells(1).Text, String) & "' ") & ","
                End If
                'Not to forget the auditTrail
                _IPP.AddAuditTrailRecord(CType(dtgItem.Cells(1).Text, String), "Finance Verifier", "Update BC No.")
            End If           
        Next
            If strInv <> "" Then
                    strInv = strInv.Substring(0, strInv.Length - 1)
                    ' Me.Session.Add("invListFail", strInv)
                    strMsg2 = objGlobal.GetErrorMessage("00003")
                    Common.NetMsgbox(Me, strMsg2, MsgBoxStyle.Information)
                Else
                    strMsg2 = objGlobal.GetErrorMessage("00007")
                    Common.NetMsgbox(Me, strMsg2, MsgBoxStyle.Information)
                End If
        strInv = ""
        Bindgrid()
    End Sub
End Class