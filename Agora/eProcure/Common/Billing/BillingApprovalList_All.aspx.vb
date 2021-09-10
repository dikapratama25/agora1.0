'Zulham 30-01-2015 IPP-GST Stage 2A
Imports AgoraLegacy
Imports eProcure.Component
Public Class BillingApprovalList_All
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

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
        Status = 8
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
        SetGridProperty(dtgIPPList)
        MyBase.Page_Load(sender, e)
        PopulateTypeAhead()
        GenerateTab()
        If Not Page.IsPostBack Then
            txtDocEndDate.Value = Common.FormatWheelDate(WheelDateFormat.LongDate, DateTime.Today)
            txtDocStartDate.Value = Common.FormatWheelDate(WheelDateFormat.LongDate, DateTime.Today.AddMonths(-6)) '.ToString
        End If
        chkdocstatus.Items(0).Enabled = False
        chkdocstatus.Items(1).Enabled = False

    End Sub
    Public Sub dtgIPPList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgIPPList.PageIndexChanged
        dtgIPPList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgIPPList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub
    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False)
        Dim objIPP As New Billing
        Dim ds As New DataSet
        Dim i As Integer
        Dim docstatus As String = ""
        Dim doctype, docno, docsdt, docedt As String
        Dim verifiedsdt As String = ""
        Dim verifiededt As String = ""

        If vldsum.InnerHtml <> "" Then
            vldsum.InnerHtml = ""
        End If
        If DateDiff(DateInterval.Month, CDate(txtDocStartDate.Value), CDate(txtDocEndDate.Value)) > 6 Then
            vldsum.InnerHtml = "<li>Date Range " & objGlobal.GetErrorMessage("00015") & " 6 months</li>"
            Return False
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
        If txtVerifiedStartDate.Value <> "" Then
            verifiedsdt = txtVerifiedStartDate.Value
        End If
        If txtVerifiedEndDate.Value <> "" Then
            verifiededt = txtVerifiedEndDate.Value
        End If
        For i = 0 To chkdocstatus.Items.Count - 1
            If chkdocstatus.Items(i).Selected Then
                docstatus &= chkdocstatus.Items(i).Value & ","
            End If
        Next
        If docstatus <> "" Then
            docstatus = docstatus.Substring(0, docstatus.Length - 1)
        End If

        ds = objIPP.PopulateBillingApprRejList(docno, doctype, docstatus, docsdt, docedt, txtVendor.Value, verifiedsdt, verifiededt)

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
    End Function



    Private Sub dtgIPPList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPPList.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgIPPList, e)
        intPageRecordCnt = ViewState("RecordCount")
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Bindgrid(True)
    End Sub
    Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        txtDocEndDate.Value = Common.FormatWheelDate(WheelDateFormat.LongDate, DateTime.Today)
        txtDocStartDate.Value = Common.FormatWheelDate(WheelDateFormat.LongDate, DateTime.Today.AddMonths(-6)) '.ToString
        ddlDocType.SelectedIndex = 0
        txtDocNo.Text = ""
        Dim chkbox As ListItem
        For Each chkbox In chkdocstatus.Items
            chkbox.Selected = False
        Next
    End Sub


    Private Sub dtgIPPList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPPList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dvMatch As DataView = dv.DataView

            'INSERT INTO STATUS_MSTR (STATUS_NO, STATUS_DESC, STATUS_REMARK, STATUS_TYPE, STATUS_DELETED) 
            'VALUES ('6', 'Billed', 'Billed', 'BIL', 'N');
            e.Item.Cells(EnumIPP.DocNo).Text = "<A href=""" & dDispatcher.direct("BILLING", "BILLINGAOApprovalDetail.aspx", "mode=read&frm=ApprListAll&index=" & dv("bM_INVOICE_INDEX") & "&DocumentNo=" & dv("bm_INVOICE_NO")) & """ ><font color=#0000ff>" & Common.parseNull(dv("bm_INVOICE_NO")) & "</font></A>"
            e.Item.Cells(EnumIPP.DocDate).Text = e.Item.Cells(EnumIPP.DocDate).Text.Substring(0, 10)
            e.Item.Cells(EnumIPP.PayAmount).Text = Format(CDbl(e.Item.Cells(EnumIPP.PayAmount).Text), "#.00")
            Select Case e.Item.Cells(EnumIPP.Status).Text
                Case 4
                    e.Item.Cells(EnumIPP.Status).Text = "Paid"
                Case 6
                    e.Item.Cells(EnumIPP.Status).Text = "Billed"
                Case 3
                    e.Item.Cells(EnumIPP.Status).Text = "Approved"
                Case 4
                    e.Item.Cells(EnumIPP.Status).Text = "Rejected"
                Case 5
                    e.Item.Cells(EnumIPP.Status).Text = "Void"
                Case 2
                    e.Item.Cells(EnumIPP.Status).Text = "Submitted"
            End Select
            Select Case e.Item.Cells(EnumIPP.DocType).Text
                Case "INV"
                    e.Item.Cells(EnumIPP.DocType).Text = "Invoice"
                Case "NON"
                    e.Item.Cells(EnumIPP.DocType).Text = "Non Invoice"
                    'Zulham 27/07/2017 - IPP Stage 3
                Case "CN"
                    e.Item.Cells(EnumIPP.DocType).Text = "Credit Note"
                Case "DN"
                    e.Item.Cells(EnumIPP.DocType).Text = "Debit Note"
                Case "CA"
                    e.Item.Cells(EnumIPP.DocType).Text = "Credit Advice"
                Case "DA"
                    e.Item.Cells(EnumIPP.DocType).Text = "Debit Advice"
                Case "CNN"
                    e.Item.Cells(EnumIPP.DocType).Text = "Credit Note(Non-Invoice)"
                Case "DNN"
                    e.Item.Cells(EnumIPP.DocType).Text = "Debit Note(Non-Invoice)"

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
                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Billing", "BillingApprovalList.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                 "<li><div class=""space""></div></li>" & _
                 "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Billing", "BillingApprovalList_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                 "<li><div class=""space""></div></li>" & _
                 "</ul><div></div></div>"


    End Sub

    Private Sub cmdSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSelectAll.Click
        Dim chkbox As ListItem
        For Each chkbox In chkdocstatus.Items
            chkbox.Selected = True
            chkdocstatus.Items(0).Selected = False
            chkdocstatus.Items(1).Selected = False
        Next
    End Sub
    Sub PopulateTypeAhead()
        Dim typeahead As String
        Dim i, count As Integer
        Dim content, content2 As String
        Dim strCompID As String
        Dim vtypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=IPPCOY&module=billing")


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
End Class


