Imports AgoraLegacy
Imports eProcure.Component

Public Class InvDetailFTN
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim blnPrintRemark As Boolean
    'Protected WithEvents back_view As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents cmd_back As Global.System.Web.UI.WebControls.Button
    Dim blnPrintCustField As Boolean
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txt_remark As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_st As System.Web.UI.WebControls.Label
    Protected WithEvents dtg_invDetail As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lbl_InvNum As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_req As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_date As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_contect As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_gst As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_cur_code As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_attention As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_YourRef As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Payterm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_sm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_bill As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_pm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_OurRef As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Public Enum EnumInvDet
        icLineItem = 0
        icDesc = 1
        icUOM = 2
        icQty = 3
        icUPrice = 4
        icTotal = 5
        icTax = 6
        iTerms = 7
    End Enum
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        ViewState("line") = 0
        MyBase.Page_Load(sender, e)
        blnCheckBox = False
        blnPaging = False
        blnSorting = False
        SetGridProperty(dtg_invDetail)
        If Not IsPostBack Then

            Dim objval As New InvValue
            Dim objinv As New Invoice
            Dim BILL_METH As String = Request(Trim("bill_meth"))
            If Request(Trim("shipamt")) <> "" Then
                ViewState("ShipAmt") = CDbl(Request(Trim("shipamt")))
            Else
                ViewState("ShipAmt") = 0
            End If
            'If BILL_METH = "FPO" Then
            objval.doc_num = Request(Trim("doc"))
            Bindgrid()
            'ElseIf BILL_METH = "DO" Then
            '    objval.doc_num = Request(Trim("doc"))
            '    Bindgrid()
            'ElseIf BILL_METH = "GRN" Then
            '    objval.doc_num = Request(Trim("doc"))
            '    Bindgrid()
            'End If
            AddRowtotal()
            objval.Inv_no = ""
            objval.B_com_id = Request(Trim("bcomid"))
            objinv.get_invmstr2(objval, BILL_METH, Request(Trim("bcomid")))
            Dim yourref As String = Request(Trim("billdoc"))
            Me.lbl_InvNum.Text = "<B>To Be Assigned</B>"
            Me.lbl_date.Text = Date.Today
            Me.lbl_YourRef.Text = yourref
            Me.lbl_Payterm.Text = objval.pt
            Me.lbl_pm.Text = objval.pm
            Me.lbl_bill.Text = objval.adds
            'Me.txt_remark.Text = objval.remark????
            Me.lbl_req.Text = objval.create_by
            Me.lbl_contect.Text = objval.phone
            Me.lbl_cur_code.Text = objval.cur
            Me.lbl_attention.Text = ""
            Me.lbl_st.Text = objval.st
            Me.lbl_sm.Text = objval.sm
            Me.lbl_OurRef.Text = Session("ourref")
            Me.txt_remark.Text = Session("remarks")
            Me.lbl_gst.Text = objval.tax_reg_no
            Me.lbl_attention.Text = objval.V_attn

        End If

        'Me.cmd_back.Attributes.Add("onclick", "window.close(); ")
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim objINV As New Invoice
        Dim ds As DataSet

        'ds = objINV.get_invDetail(Request(Trim("Doc")), Request(Trim("bill_meth")), Request(Trim("bcomid")))
        Dim strInv As String
        Dim blnAllowInv As Boolean
        If Request(Trim("bill_meth")) = "FPO" Then
            ds = objINV.getUnInvoicePOLine(Request(Trim("Doc")), Request(Trim("bcomid")), blnAllowInv, strInv)
        ElseIf Request(Trim("bill_meth")) = "DO" Then
            ds = objINV.getUnInvoiceDOLine(Request(Trim(" poidx")), Request(Trim("Doc")), Request(Trim("bcomid")), blnAllowInv, strInv)
        ElseIf Request(Trim("bill_meth")) = "GRN" Then
            ds = objINV.getUnInvoiceGRNLine(Request(Trim(" poidx")), Request(Trim("Doc")), Request(Trim("bcomid")), blnAllowInv, strInv)
        End If

        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView
        If pSorted Then
            dvViewPR.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewPR.Sort += " DESC"
        End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured

        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then

            dtg_invDetail.DataSource = dvViewPR
            Dim objPR As New PurchaseReq2
            'If objPR.Need2PrintCField(ds.Tables(0).Rows(0)("POM_PO_INDEX")) Then '.Rows(0)("POD_PR_INDEX")) Then
            '    blnPrintCustField = True
            'Else
            '    blnPrintCustField = False
            'End If
            'If objPR.Need2PrintRemark(ds.Tables(0).Rows(0)("POM_PO_INDEX")) Then '
            '    blnPrintRemark = True
            'Else
            '    blnPrintRemark = False
            'End If
            'Modified by Joon on 18th Oct 2011 for issue 1001
            If objPR.Need2PrintPOCField(ds.Tables(0).Rows(0)("POM_PO_INDEX"), IIf(IsDBNull(ds.Tables(0).Rows(0)("POD_PR_INDEX")), 0, ds.Tables(0).Rows(0)("POD_PR_INDEX"))) Then '.Rows(0)("POD_PR_INDEX")) Then
                blnPrintCustField = True
            Else
                blnPrintCustField = False
            End If
            If objPR.Need2PrintPORemark(ds.Tables(0).Rows(0)("POM_PO_INDEX")) Then '
                blnPrintRemark = True
            Else
                blnPrintRemark = False
            End If

            If blnPrintCustField Then
                'addDataGridColumn(ds.Tables(0).Rows(0)("POD_PR_LINE"), ds.Tables(0).Rows(0)("POD_PR_INDEX"))
                addDataGridColumn(ds.Tables(0).Rows(0)("POD_PO_LINE"), ds.Tables(0).Rows(0)("POM_PO_INDEX"), IIf(IsDBNull(ds.Tables(0).Rows(0)("POD_PR_INDEX")), 0, ds.Tables(0).Rows(0)("POD_PR_INDEX")))
            End If
            dtg_invDetail.DataBind()

        Else
            'dtgDept.DataSource = ""
            dtg_invDetail.DataBind()

            Common.NetMsgbox(Me, MsgNoRecord)
            'intTotPage = 0
        End If
        'If Session("Env") = "FTN" Then
        '    Me.dtg_invDetail.Columns(7).Visible = False
        '    'Me.dtg_invDetail.Columns(8).Visible = False
        'Else
        '    Me.dtg_invDetail.Columns(7).Visible = True
        '    'Me.dtg_invDetail.Columns(8).Visible = True
        'End If
        Me.dtg_invDetail.Columns(7).Visible = False
    End Function

    Private Sub addDataGridColumn(ByVal pr_line As String, ByVal pr_num As String, Optional ByVal pr_idx As Integer = 0)
        Dim objinv As New Invoice
        Dim dsCustom As New DataSet
        Dim dsCustomField As New DataSet
        Dim ds As New DataSet
        Dim rowcnt As Integer
        Dim i As Integer
        ds = objinv.get_CustFieldGrn(pr_line, pr_num, pr_idx)
        ViewState("pr_line") = pr_line
        ViewState("pr_num") = pr_num
        rowcnt = ds.Tables(0).Rows.Count

        'ReDim dvwCustom(rowcnt)
        'ReDim strCustomDefault(rowcnt)

        '//dynamicly add template column
        For i = 0 To rowcnt - 1
            Dim col As BoundColumn = New BoundColumn
            ' col.DataField = "PCD_FIELD_VALUE"
            col.HeaderText = ds.Tables(0).Rows(i)("PCM_FIELD_NAME")
            Me.dtg_invDetail.Columns.AddAt(dtg_invDetail.Columns.Count - 1, col)
        Next

    End Sub


    Sub AddRowtotal() 'add total row 
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row3 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row4 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        For intL = 0 To Me.dtg_invDetail.Columns.Count - 1
            addCell(row)
        Next

        row.Cells(4).Text = "Sub Total"
        row.Cells(4).HorizontalAlign = HorizontalAlign.Right
        row.Cells(4).Font.Bold = True

        row.Cells(5).Text = Format(ViewState("total"), "#,##0.00")
        row.Cells(5).HorizontalAlign = HorizontalAlign.Right
        row.Cells(5).Font.Bold = True

        'row.Cells(6).Text = Format(viewstate("gst"), "#,##0.00")
        'row.Cells(6).HorizontalAlign = HorizontalAlign.Right
        'row.Cells(6).Font.Bold = True

        Me.dtg_invDetail.Controls(0).Controls.Add(row)


        For intL = 0 To Me.dtg_invDetail.Columns.Count - 1
            addCell(row2)
        Next

        row2.Cells(4).Text = "Tax"
        row2.Cells(4).HorizontalAlign = HorizontalAlign.Right
        row2.Cells(4).Font.Bold = True

        row2.Cells(5).Text = Format(ViewState("gst"), "#,##0.00")
        row2.Cells(5).HorizontalAlign = HorizontalAlign.Right
        row2.Cells(5).Font.Bold = True

        Me.dtg_invDetail.Controls(0).Controls.Add(row2)

        For intL = 0 To Me.dtg_invDetail.Columns.Count - 1
            addCell(row3)
        Next

        row3.Cells(4).Text = "Shipping & Handling"
        row3.Cells(4).HorizontalAlign = HorizontalAlign.Right
        row3.Cells(4).Font.Bold = True

        row3.Cells(5).Text = Format(CDbl(ViewState("ShipAmt")), "#,##0.00")
        row3.Cells(5).HorizontalAlign = HorizontalAlign.Right
        row3.Cells(5).Font.Bold = True

        Me.dtg_invDetail.Controls(0).Controls.Add(row3)

        For intL = 0 To Me.dtg_invDetail.Columns.Count - 1
            addCell(row4)
        Next

        row4.Cells(4).Text = "Grand Total"
        row4.Cells(4).HorizontalAlign = HorizontalAlign.Right
        row4.Cells(4).Font.Bold = True

        row4.Cells(5).Text = Format(ViewState("total") + ViewState("gst") + ViewState("ShipAmt"), "#,##0.00")
        row4.Cells(5).HorizontalAlign = HorizontalAlign.Right
        row4.Cells(5).Font.Bold = True

        Me.dtg_invDetail.Controls(0).Controls.Add(row4)

    End Sub

    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub

    Private Sub dtg_invDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_invDetail.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dblSubTotal As Double
            Dim objinv As New Invoice
            Dim dsCustom As New DataSet
            Dim dsCustomField As New DataSet
            Dim ds As New DataSet
            Dim rowcnt As Integer
            Dim i As Integer

            dblSubTotal = Common.parseNull(dv("QTY"), 0) * Common.parseNull(dv("POD_UNIT_COST"), 0)
            e.Item.Cells(EnumInvDet.icTotal).Text = Format(dblSubTotal, "#,##0.00")
            e.Item.Cells(EnumInvDet.icUPrice).Text = Format(dv("POD_UNIT_COST"), "#,##0.0000")
            'ViewState("total") = ViewState("total") + dblSubTotal
            ViewState("total") = ViewState("total") + CDbl(Format(dblSubTotal, "#,##0.00"))
            e.Item.Cells(EnumInvDet.icTax).Text = Format(dblSubTotal / 100 * Common.parseNull(dv("POD_GST")), "#,##0.00")
            'ViewState("gst") = ViewState("gst") + dblSubTotal / 100 * Common.parseNull(dv("POD_GST"))
            ViewState("gst") = ViewState("gst") + CDbl(Format((dblSubTotal / 100 * Common.parseNull(dv("POD_GST"))), "#,##0.00"))
            e.Item.Cells(EnumInvDet.icLineItem).Text = Common.parseNull(dv("POD_PO_LINE"))
            '??? why call viewstate (by Moo)
            'ds = objinv.get_CustFieldGrn(VIEWSTATE("pr_line"), VIEWSTATE("pr_num"))
            If blnPrintCustField Then
                ds = objinv.get_CustFieldGrn(dv("POD_PO_LINE"), dv("POM_PO_INDEX"), IIf(IsDBNull(dv("POD_PR_INDEX")), 0, dv("POD_PR_INDEX")))
                rowcnt = ds.Tables(0).Rows.Count

                For i = 0 To rowcnt - 1
                    e.Item.Cells(EnumInvDet.iTerms + i).Text = ds.Tables(0).Rows(i)("PCD_FIELD_VALUE")
                Next
            End If
        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_SearchGInv_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Invoice", "InvList.aspx", "pageid=" & strPageId) & """><span>Issue Invoice</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Invoice", "invoiceView.aspx", "pageid=" & strPageId) & """><span>Invoice Listing</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                            "</ul><div></div></div>"
        'Session("w_SearchGInv_tabs") = "<div class=""t_entity"">" & _
        '            "<a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Invoice", "InvList.aspx", "pageid=" & strPageId) & """><span>Issue Invoice</span></a>" & _
        '            "<a class=""t_entity_btn"" href=""" & dDispatcher.direct("Invoice", "invoiceView.aspx", "pageid=" & strPageId) & """><span>Invoice Listing</span></a>" & _
        '            "</div>"

    End Sub
End Class
