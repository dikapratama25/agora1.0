Imports eProcure.Component
Imports AgoraLegacy


Partial Public Class IPPList
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents txtDocStartDate As System.Web.UI.HtmlControls.HtmlInputText
    Protected WithEvents txtDocEndDate As System.Web.UI.HtmlControls.HtmlInputText

    Dim blnIPPOfficer As Boolean
    Dim blnIPPOfficerS As Boolean
    Dim objUsers As New Users
    Dim objGlobal As New AppGlobals

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
        PayDate = 11
        PayAdviceNo = 12
        Status = 13
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objIPP As New IPP
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgIPPList)
        Session("dsIPPDocDetail") = Nothing
        Session("dsIPPDocDetailCount") = Nothing
        Session("urlreferer") = "IPPList"

        blnIPPOfficer = objUsers.checkUserFixedRole("'IPP Officer(F)'")
        blnIPPOfficerS = objUsers.checkUserFixedRole("'IPP Officer'")

        'zulham 05042019 - multiinvoices
        If Not Session("strDocNo") Is Nothing Then
            Bindgrid()
            Session("strDocNo") = Nothing
        End If

        If Not IsPostBack Then
            GenerateTab()
            PopulateTypeAhead()
            txtDocStartDate.Value = DateAdd("m", -6, DateTime.Now.ToShortDateString())
            txtDocEndDate.Value = DateTime.Now.ToShortDateString()
        End If
        objIPP = Nothing
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objIPP As New IPP
        Dim ds As New DataSet
        Dim i As Integer
        Dim docstatus As String = ""
        Dim doctype, docno, docsdt, docedt, strVen As String
        Dim strMsg As String

        Dim comparedt As Date

        'Zulham 05042019 - multiinvoices
        If Session("strDocNo") Is Nothing Then
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
                docno = Common.Parse(docno.Replace("\", "\\\\")) 'Zulham 18072018 - PAMB
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
        Else
            docno = Session("strDocNo")
        End If
        If blnIPPOfficerS = True Then
            ds = objIPP.PopulateIPPListNew(docno, doctype, docstatus, docsdt, docedt, strVen, "IPPTeller", ViewState("venidx"))
        Else
            ds = objIPP.PopulateIPPListNew(docno, doctype, docstatus, docsdt, docedt, strVen, , ViewState("venidx"))
        End If


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
            strDocCreator = objDB.Get1Column("invoice_mstr", "im_created_by", " where im_invoice_index = " & dv("IM_INVOICE_INDEX") & "")
            'Zulham 13072018 - PAMB
            Select Case e.Item.Cells(EnumIPP.Status).Text
                Case 10
                    e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "IPPEntry.aspx", "mode=modify&urlrefereripp=" & Session("urlreferer") & "&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
                Case 14
                    If dv("IM_ROUTE_TO") = "" Then
                        e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "IPPEntry.aspx", "mode=modify&status=reject&urlrefereripp=" & Session("urlreferer") & "&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
                    Else
                        If dv("IM_ROUTE_TO") = strDocCreator Then
                            e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "PSDAcceptanceDetails.aspx", "Frm=PSDFyFa&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
                        Else
                            e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "IPPDocument.aspx", "Frm=IPPList&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
                        End If
                    End If
                Case Else
                    e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "IPPDocument.aspx", "Frm=IPPList&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
            End Select
            If IsDBNull(dv("ISD_DOC_NO")) Then
                e.Item.Cells(EnumIPP.SubDocNo).Text = "N/A"
            End If

            'Zulham 220402019 - REQ018
            If Not dv("IM_DOC_DATE") Is DBNull.Value Then
                e.Item.Cells(EnumIPP.DocDate).Text = e.Item.Cells(EnumIPP.DocDate).Text.Substring(0, 10)
            End If

            If Not IsDBNull(dv("ISD_DOC_AMT")) Then
                e.Item.Cells(EnumIPP.SubDocPayAmt).Text = Format(dv("ISD_DOC_AMT"), "#,##0.00")
            End If
            e.Item.Cells(EnumIPP.PayAmount).Text = Format(dv("IM_INVOICE_TOTAL"), "#,##0.00")
            e.Item.Cells(EnumIPP.PayDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("IM_payment_date"))
            'Zulham 08112018
            Select Case e.Item.Cells(EnumIPP.Status).Text
                Case 1
                    e.Item.Cells(EnumIPP.Status).Text = "New Invoice and submitted to FO"
                Case 2
                    e.Item.Cells(EnumIPP.Status).Text = "Pending approval from FM"
                Case 3
                    e.Item.Cells(EnumIPP.Status).Text = "Approved by FM"
                Case 19
                    e.Item.Cells(EnumIPP.Status).Text = "PSD Received"
                Case 4
                    e.Item.Cells(EnumIPP.Status).Text = "Paid"
                Case 10
                    e.Item.Cells(EnumIPP.Status).Text = "Draft"
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
                    e.Item.Cells(EnumIPP.Status).Text = "E2P Verified"
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
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_IPP_tabs") = "<div class=""t_entity""><ul>" &
        "<li><div class=""space""></div></li>" &
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "IPPEntry.aspx", "mode=new&pageid=" & strPageId) & """><span>E2P Document</span></a></li>" &
                    "<li><div class=""space""></div></li>" &
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPP", "IPPList.aspx", "type=Listing&mode=new&pageid=" & strPageId) & """><span>E2P Document Listing</span></a></li>" &
                    "<li><div class=""space""></div></li>" &
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
End Class