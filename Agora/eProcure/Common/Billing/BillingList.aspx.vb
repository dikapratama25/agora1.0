'Zulham 30-01-2015 IPP-GST Stage 2A
Imports eProcure.Component
Imports AgoraLegacy


Partial Public Class BillingList
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    'Protected WithEvents txtDocStartDate As System.Web.UI.HtmlControls.HtmlInputText
    'Protected WithEvents txtDocEndDate As System.Web.UI.HtmlControls.HtmlInputText

    Dim blnIPPOfficer As Boolean
    Dim blnIPPOfficerS As Boolean
    Dim objUsers As New Users
    Dim objGlobal As New AppGlobals

    Public Enum EnumBilling
        DocNo = 0
        DocType = 1
        DocDate = 2
        Vendor = 3
        BankCode = 4
        BankAccount = 5
        Currency = 6
        PayAmount = 7
        Status = 8
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objIPP As New IPP
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgIPPList)
        Session("dsIPPDocDetail") = Nothing
        Session("dsIPPDocDetailCount") = Nothing
        Session("urlreferer") = "BillingList"

        blnIPPOfficer = objUsers.checkUserFixedRole("'IPP Officer(F)'")
        blnIPPOfficerS = objUsers.checkUserFixedRole("'IPP Officer'")

        If Not IsPostBack Then
            GenerateTab()
            PopulateTypeAhead()
            txtDocStartDate.Value = DateAdd("m", -6, DateTime.Now.ToShortDateString())
            txtDocEndDate.Value = DateTime.Now.ToShortDateString()
        End If
        objIPP = Nothing
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objIPP As New Billing
        Dim objIPP2 As New IPP
        Dim ds As New DataSet
        Dim i As Integer
        Dim docstatus As String = ""
        Dim doctype, docno, docsdt, docedt, strVen As String
        Dim strMsg As String

        Dim comparedt As Date
        comparedt = DateAdd("m", -6, CDate(txtDocEndDate.Value))

        If CDate(txtDocStartDate.Value) < comparedt Then
            strMsg = "Document Start date " & objGlobal.GetErrorMessage("00015") & " 6 months"
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Return False
        End If

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
        For i = 0 To chkdocstatus.Items.Count - 1
            If chkdocstatus.Items(i).Selected Then
                docstatus &= chkdocstatus.Items(i).Value & ","
            End If
        Next
        If docstatus <> "" Then
            docstatus = docstatus.Substring(0, docstatus.Length - 1)
        End If

        'If blnIPPOfficerS = True Then
        '    'ds = objIPP.PopulateIPPList(docno, doctype, docstatus, docsdt, docedt, strVen, "IPPList", "IPPTeller", ViewState("venidx"))
        '    ds = objIPP2.PopulateIPPListNew(docno, doctype, docstatus, docsdt, docedt, strVen, "IPPTeller", ViewState("venidx"))
        'Else
        'ds = objIPP.PopulateIPPList(docno, doctype, docstatus, docsdt, docedt, strVen, , , ViewState("venidx"))
        ds = objIPP.PopulateBillingListNew(docno, doctype, docstatus, docsdt, docedt, strVen, , ViewState("venidx"))
        'End If


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
    End Sub
    Sub dtgIPPList_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgIPPList.PageIndexChanged
        dtgIPPList.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub dtgIPPList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPPList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dvMatch As DataView = dv.DataView
            Dim objDB As New EAD.DBCom
            Dim strDocCreator As String
            strDocCreator = objDB.Get1Column("billing_mstr", "bm_created_by", " where bm_invoice_index = " & dv("bM_INVOICE_INDEX") & "")
            Select Case e.Item.Cells(enumbilling.Status).Text
                Case 1
                    e.Item.Cells(EnumBilling.DocNo).Text = "<A href=""" & dDispatcher.direct("Billing", "BillingEntry.aspx", "mode=modify&urlrefereripp=" & Session("urlreferer") & "&index=" & dv("BM_INVOICE_INDEX") & "&DocumentNo=" & dv("BM_INVOICE_NO")) & """ ><font color=#0000ff>" & Common.parseNull(dv("BM_INVOICE_NO")) & "</font></A>"
                Case 4
                    'If dv("IM_ROUTE_TO") = "" Then
                    e.Item.Cells(EnumBilling.DocNo).Text = "<A href=""" & dDispatcher.direct("Billing", "BillingEntry.aspx", "mode=modify&status=reject&urlrefereripp=" & Session("urlreferer") & "&index=" & dv("BM_INVOICE_INDEX") & "&DocumentNo=" & dv("BM_INVOICE_NO")) & """ ><font color=#0000ff>" & Common.parseNull(dv("BM_INVOICE_NO")) & "</font></A>"
                    'Else
                    'If dv("IM_ROUTE_TO") = strDocCreator Then
                    'Else
                    '    e.Item.Cells(EnumBilling.DocNo).Text = "<A href=""" & dDispatcher.direct("Billing", "BillingDocument.aspx", "Frm=IPPList&index=" & dv("BM_INVOICE_INDEX") & "&DocumentNo=" & dv("BM_INVOICE_NO")) & """ ><font color=#0000ff>" & Common.parseNull(dv("BM_INVOICE_NO")) & "</font></A>"
                    'End If
                    'End If
                Case Else
                    e.Item.Cells(EnumBilling.DocNo).Text = "<A href=""" & dDispatcher.direct("Billing", "BillingDocument.aspx", "Frm=BillingList&index=" & dv("BM_INVOICE_INDEX") & "&DocumentNo=" & dv("BM_INVOICE_NO")) & """ ><font color=#0000ff>" & Common.parseNull(dv("BM_INVOICE_NO")) & "</font></A>"
            End Select
            e.Item.Cells(EnumBilling.DocDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("bM_created_on"))
            e.Item.Cells(EnumBilling.PayAmount).Text = Format(dv("BM_INVOICE_TOTAL"), "#,##0.00")
            Select Case e.Item.Cells(enumbilling.Status).Text
                Case 6
                    e.Item.Cells(EnumBilling.Status).Text = "Billed"
                Case 1
                    e.Item.Cells(EnumBilling.Status).Text = "Draft"
                Case 3
                    e.Item.Cells(EnumBilling.Status).Text = "Approved"
                Case 4
                    e.Item.Cells(EnumBilling.Status).Text = "Rejected"
                Case 5
                    e.Item.Cells(EnumBilling.Status).Text = "Void"
                Case 2
                    e.Item.Cells(EnumBilling.Status).Text = "Submitted"
            End Select

            Select Case e.Item.Cells(enumbilling.DocType).Text
                Case "INV"
                    e.Item.Cells(EnumBilling.DocType).Text = "Invoice"
                Case "NON"
                    e.Item.Cells(EnumBilling.DocType).Text = "Non-Invoice"
                Case "CN"
                    e.Item.Cells(EnumBilling.DocType).Text = "Credit Note"
                Case "DN"
                    e.Item.Cells(EnumBilling.DocType).Text = "Debit Note"
                Case "CA"
                    e.Item.Cells(EnumBilling.DocType).Text = "Credit Advice"
                Case "DA"
                    e.Item.Cells(EnumBilling.DocType).Text = "Debit Advice"
                Case "CNN"
                    e.Item.Cells(EnumBilling.DocType).Text = "Credit Note(Non-Invoice)"
                Case "DNN"
                    e.Item.Cells(EnumBilling.DocType).Text = "Debit Note(Non-Invoice)"
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
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Billing", "BillingEntry.aspx", "mode=new&pageid=" & strPageId) & """><span>Billing Document</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Billing", "BillingList.aspx", "type=Listing&mode=new&pageid=" & strPageId) & """><span>Billing Document Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        dtgIPPList.CurrentPageIndex = 0
        Bindgrid()
    End Sub
    Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        'txtDocStartDate.Value = ""
        'txtDocEndDate.Value = ""
        txtDocStartDate.Value = DateAdd("m", -6, DateTime.Now.ToShortDateString())
        txtDocEndDate.Value = DateTime.Now.ToShortDateString()
        ddlDocType.SelectedIndex = 0
        txtDocNo.Text = ""
        txtVendor.Value = ""
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
        Dim vtypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=IPPCOY&CoyType=V&module=billing")


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
End Class