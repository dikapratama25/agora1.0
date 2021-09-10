Imports AgoraLegacy
Imports eProcure.Component
Public Class IPPFVPendingFYFA
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents vldDateFtDateTo As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents vldDateFr As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents vldDateTo As System.Web.UI.WebControls.CustomValidator    
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents vldsum As System.Web.UI.HtmlControls.HtmlGenericControl
    Dim strCaller As String
    Dim objGlobal As New AppGlobals
    Public Enum EnumIPP

        DocNo = 0
        DocType = 1
        DocDate = 2
        Vendor = 3
        BankCode = 4
        BankAccount = 5
        Currency = 6
        PayAmount = 7
        PayMethod = 8
        PayDate = 9
        PayAdviceNo = 10
        Status = 11
    End Enum
    Dim strAO As String
    Protected WithEvents lblVendor As System.Web.UI.WebControls.Label
    Dim blnRelief As Boolean
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load     
        strNewCSS = "true"
        SetGridProperty(dtgIPPFVPendingFYFAList)
        MyBase.Page_Load(sender, e)
        PopulateTypeAhead()
        GenerateTab()
        'If Not Page.IsPostBack Then

        'End If
        Bindgrid(True)
      
    End Sub
    Public Sub dtgIPPFVPendingFYFAList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgIPPFVPendingFYFAList.PageIndexChanged
        dtgIPPFVPendingFYFAList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgIPPFVPendingFYFAList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub
    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False)
        Dim objIPP As New IPP
        Dim ds As New DataSet
        Dim i As Integer
        Dim docstatus As String = ""
        Dim doctype, docno, docsdt, docedt As String

        If vldsum.InnerHtml <> "" Then
            vldsum.InnerHtml = ""
        End If


        If txtDocNo.Text <> "" Then
            docno = txtDocNo.Text
            'Zulham 18072018 - PAMB
            docno = Common.Parse(docno.Replace("\", "\\\\")) 'Zulham 13072018 - PAMB
        End If
        
        If ddlDocType.SelectedIndex <> 0 Then
            doctype = ddlDocType.SelectedItem.Value
        End If
        
        If docstatus <> "" Then
            docstatus = docstatus.Substring(0, docstatus.Length - 1)
        End If

        ds = objIPP.PopulateIPPFVPendingFYFA(docno, doctype, docstatus, docsdt, docedt, txtVendor.Value)

        intPageRecordCnt = ds.Tables(0).Rows.Count

        ViewState("RecordCount") = intPageRecordCnt
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgIPPFVPendingFYFAList, dvViewSample)
            dtgIPPFVPendingFYFAList.DataSource = dvViewSample
            dtgIPPFVPendingFYFAList.DataBind()
        Else
            dtgIPPFVPendingFYFAList.DataSource = dvViewSample
            dtgIPPFVPendingFYFAList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If    
        ' add for above checking
        ViewState("PageCount") = dtgIPPFVPendingFYFAList.PageCount
    End Function



    Private Sub dtgIPPFVPendingFYFAList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPPFVPendingFYFAList.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgIPPFVPendingFYFAList, e)
        intPageRecordCnt = ViewState("RecordCount")
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Bindgrid(True)
    End Sub



    Private Sub dtgIPPList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPPFVPendingFYFAList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dvMatch As DataView = dv.DataView

            'Zulham 13072018 - PAMB
            e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("IPP", "IPPApprovalDetail.aspx", "pagefrm=FVPendingFYFA&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO"))) & """ ><font color=#0000ff>" & Common.parseNull(dv("IM_INVOICE_NO")) & "</font></A>"
            e.Item.Cells(EnumIPP.DocDate).Text = e.Item.Cells(EnumIPP.DocDate).Text.Substring(0, 10)
            e.Item.Cells(EnumIPP.PayAmount).Text = Format(CDbl(e.Item.Cells(EnumIPP.PayAmount).Text), "#.00")
            e.Item.Cells(EnumIPP.PayDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("IM_payment_date"))
            'Zulham 08112018
            Select Case e.Item.Cells(EnumIPP.Status).Text
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
                    e.Item.Cells(EnumIPP.Status).Text = "E2P Verified" 'Zulham 13072018 - PAMB
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

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_IPP_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & """><span>New Invoice</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId & "&folder=S&status=1&ipp=y") & """><span>Verified Invoice</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                         "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPP", "IPPFVPendingFYFA.aspx", "pageid=" & strPageId) & """><span>Pending FYFA</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "</ul><div></div></div>"


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

    Private Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click

        ddlDocType.SelectedIndex = 0
        txtDocNo.Text = ""
        Dim chkbox As ListItem

    End Sub
End Class


