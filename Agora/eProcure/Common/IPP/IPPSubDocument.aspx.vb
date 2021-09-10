Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing

Public Class IPPSubDocument
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents txt_desc As System.Web.UI.WebControls.TextBox
    Protected WithEvents dtg_SubDoc As System.Web.UI.WebControls.DataGrid
    Protected WithEvents onchange As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents cmd_search As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_back As System.Web.UI.WebControls.Button
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lbl_check As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1 As System.Web.UI.WebControls.Label
    Protected WithEvents txt_item_desc As System.Web.UI.WebControls.TextBox
    Protected WithEvents cboCommodityType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents dt_rfq As System.Web.UI.WebControls.Table
    Protected WithEvents cmd_freeformClear As System.Web.UI.WebControls.Button
    Protected WithEvents lblamt As System.Web.UI.WebControls.Label
    Protected WithEvents lblGSTAmt As System.Web.UI.WebControls.Label
    Protected WithEvents hiddtg_cat As Global.System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents hiddtg_SubDoc As Global.System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents chkSpot As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkStock As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkMRO As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtItemCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents hidCommodity As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtCommodity As System.Web.UI.WebControls.TextBox
    Protected trGSTAmount As System.Web.UI.HtmlControls.HtmlTableRow

    Dim dDispatcher As New AgoraLegacy.dispatcher

    Dim appGlo As New AppGlobals
    Dim total As Integer
    Dim check_qty As Integer
    Dim objGlobal As New AppGlobals
    Dim dta As Table
    Dim strMsg As String = ""
    Dim rfq_name As String
    Dim back As String
    Dim objDB As New EAD.DBCom
    Dim _exceedCutOffDt As String = ""
    Dim documentDate As String = ""
    Dim strIsGst As String
    Dim invIdx As String = ""
    Dim vendorIdx As String = ""


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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strNewCSS = "both"
        MyBase.Page_Load(sender, e)
        blnPaging = False
        SetGridProperty(Me.dtg_SubDoc)
        If Not Page.IsPostBack Then
            If Not Request.QueryString("LineNo") Is Nothing Then
                'Tis coming from edit button
                AddRow(1)
                BindRulesCat(Request.QueryString("LineNo"))
                Me.dtg_SubDoc.Columns(0).Visible = False
            Else
                AddRow(10)
                SetGridProperty(dtg_SubDoc)
                dtg_SubDoc.DataBind()
            End If
            Me.cmd_back.Attributes.Add("onclick", "window.close();")
        End If
        lbl_check.Text = ""
        Session("CurrentScreen") = "AddItem"
        Session("CatItemAdd_PONo") = Request.QueryString("PoNo")
        'Check for GST
        If Session("DocNo") Is Nothing Then
            Session("DocNo") = Session("DocNo_Amt").ToString.Split("|")(2)
        End If
        If Not Session("DocNo") Is Nothing Then
            documentDate = objDB.GetVal("SELECT IFNULL(im_doc_date,'') 'im_doc_date' FROM invoice_mstr WHERE im_invoice_index = '" & Session("DocNo") & "'")
        End If
        Dim createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
        Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")

        'Zulham 13082015
        'Additional condition for company's effective date
        Dim coyId = objDB.GetVal("SELECT IFNULL(im_s_coy_id,'') 'im_s_coy_id' FROM invoice_mstr WHERE im_invoice_index = '" & Session("DocNo") & "'")
        Dim effectiveDate = objDB.GetVal("SELECT IFNULL(ic_gst_eff_date, '') FROM IPP_COMPANY WHERE ic_index = '" & Common.Parse(coyId) & "'")

        'Check Existing Doc No
        If Trim(effectiveDate) <> "" Then
            If CDate(createdDate) >= CDate(_cutoffDate) And CDate(createdDate) >= CDate(effectiveDate) Then
                _exceedCutOffDt = "Yes"
                invIdx = Session("DocNo")
                vendorIdx = objDB.GetVal("select im_s_coy_id from invoice_mstr where im_invoice_index = " & invIdx)
                If vendorIdx <> "" And documentDate <> "" Then
                    Dim GSTRegNo = objDB.GetVal("SELECT IFNULL(IC_TAX_REG_NO, '') FROM IPP_COMPANY WHERE ic_index = '" & vendorIdx & "'")
                    If GSTRegNo <> "" And CDate(documentDate) >= CDate(_cutoffDate) And CDate(documentDate) >= CDate(effectiveDate) Then
                        strIsGst = "Yes"
                    Else
                        strIsGst = "No"
                    End If
                Else
                    strIsGst = "Yes"
                End If
            Else
                strIsGst = "No"
            End If
        Else
            strIsGst = "No"
        End If

        'Zulham 19082015
        If CDate(createdDate) >= CDate(_cutoffDate) Then
            _exceedCutOffDt = "Yes"
        End If

        Dim Asset As New PurchaseOrder_Buyer
        If strIsGst = "Yes" Then
            dtg_SubDoc.Columns(4).Visible = True
            trGSTAmount.Visible = True
        ElseIf _exceedCutOffDt = "Yes" Then
            dtg_SubDoc.Columns(4).Visible = True
            trGSTAmount.Visible = True
        Else
            dtg_SubDoc.Columns(4).Visible = False
            trGSTAmount.Visible = False
        End If
        'End
        calculateTotal()
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objpro As New Products
        Dim ds As New DataSet
        Dim ary(0) As String
        Dim objrfq As New RFQ
        Dim strCommodity As String = ""
        Dim strCatalogueBuyer As String = ""
        Dim aryProdCode As New ArrayList, i As Integer
        Dim strProdList As String = ""
        Dim aryItemType As New ArrayList

        If Me.txtCommodity.Text <> "" Then
            strCommodity = Me.hidCommodity.Value
        End If

        If Me.chkSpot.Checked = True Then
            aryItemType.Add("SP")
        End If

        If Me.chkStock.Checked = True Then
            aryItemType.Add("ST")
        End If

        If Me.chkMRO.Checked = True Then
            aryItemType.Add("MI")
        End If

        Dim objBUYERCAT As New BuyerCat
        Dim strItem As String = ""

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"


        ViewState("intPageRecordCnt") = dvViewSample.Count
    End Function

    Public Sub dtg_cat_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        Bindgrid()

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        sender.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub BindRulesCat(ByVal slineno As Integer)
        Dim dgItemFF As DataGridItem
        Dim amt As Double = 0.0
        Dim GSTAmt As Double = 0.0
        If Not Session("SubDocument") Is Nothing Then
            If CType(Session("SubDocument"), ArrayList).Count > 0 Then
                For i As Integer = 0 To CType(Session("SubDocument"), ArrayList).Count - 1
                    If CType(Session("SubDocument"), ArrayList)(i)(0).ToString = slineno Then
                        For Each dgItemFF In dtg_SubDoc.Items
                            Dim lineNo, index As Label
                            Dim txt_desc As TextBox
                            Dim txtdocDate As New HtmlInputText
                            Dim txtAmount, txtGSTAmount As New TextBox
                            lineNo = dgItemFF.FindControl("LineNo")
                            txt_desc = dgItemFF.FindControl("txt_desc")
                            txtdocDate = dgItemFF.FindControl("txtdocDate")
                            txtAmount = dgItemFF.FindControl("txtAmount")
                            txtGSTAmount = dgItemFF.FindControl("txtGSTAmount")
                            index = dgItemFF.FindControl("index")

                            lineNo.Text = slineno
                            txt_desc.Text = CType(Session("SubDocument"), ArrayList)(i)(1).ToString
                            txtdocDate.Value = CType(Session("SubDocument"), ArrayList)(i)(2).ToString
                            amt = CType(Session("SubDocument"), ArrayList)(i)(3)
                            txtAmount.Text = Format(amt, "#.00")
                            index.Text = CType(Session("SubDocument"), ArrayList)(i)(4).ToString
                            GSTAmt = CType(Session("SubDocument"), ArrayList)(i)(5)
                            If GSTAmt = 0.0 Then
                                txtGSTAmount.Text = "0.00"
                            ElseIf CDec(GSTAmt) < CDec(1) Then
                                txtGSTAmount.Text = GSTAmt.ToString
                            Else
                                txtGSTAmount.Text = Format(GSTAmt, "#.00")
                            End If

                            index.Visible = False
                        Next
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub cmd_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_search.Click
    End Sub

    Private Sub clear_All()
        txtItemCode.Text = ""
        txt_item_desc.Text = ""
        txtCommodity.Text = ""
        hidCommodity.Value = ""
        chkSpot.Checked = False
        chkStock.Checked = False
        chkMRO.Checked = False
    End Sub

    Private Sub cmd_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_clear.Click
        Call clear_All()
    End Sub

    Private Sub cmd_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Save.Click
        Dim chkItem As CheckBox
        Dim arySubDoc As New ArrayList
        Dim i, j As Integer
        Dim objDoc As New IPPMain

        If Not validation() Then
            Exit Sub
        End If

        If Not Session("SubDocument") Is Nothing Then
            If CType(Session("SubDocument"), ArrayList).Count > 0 Then
                arySubDoc = Session("SubDocument")
            End If
        End If
        Dim dgItemFF As DataGridItem
        For Each dgItemFF In dtg_SubDoc.Items
            Dim lineNo, index As Label
            Dim txt_desc As TextBox
            Dim txtdocDate As New HtmlInputText
            Dim txtAmount, txtGSTAmount As New TextBox
            lineNo = dgItemFF.FindControl("LineNo")
            txt_desc = dgItemFF.FindControl("txt_desc")
            txtdocDate = dgItemFF.FindControl("txtdocDate")
            txtAmount = dgItemFF.FindControl("txtAmount")
            txtGSTAmount = dgItemFF.FindControl("txtGSTAmount")
            If txtGSTAmount.Text = "N/A" Then txtGSTAmount.Text = 0.0
            index = dgItemFF.FindControl("index")
            If Request.QueryString("LineNo") Is Nothing Then
                If txt_desc.Text <> "" Then
                    If _exceedCutOffDt = "Yes" And strIsGst = "Yes" Then
                        arySubDoc.Add(New String() {lineNo.Text, txt_desc.Text, txtdocDate.Value, txtAmount.Text, index.Text, txtGSTAmount.Text})
                    ElseIf _exceedCutOffDt = "Yes" And strIsGst = "No" Then
                        arySubDoc.Add(New String() {lineNo.Text, txt_desc.Text, txtdocDate.Value, txtAmount.Text, index.Text, 0.0})
                    Else
                        arySubDoc.Add(New String() {lineNo.Text, txt_desc.Text, txtdocDate.Value, txtAmount.Text, index.Text, 0.0})
                    End If
                End If
            Else
                For row As Integer = 0 To CType(Session("SubDocument"), ArrayList).Count - 1
                    If CType(Session("SubDocument"), ArrayList)(row)(0).ToString = Request.QueryString("LineNo") Then
                        arySubDoc(row)(0) = lineNo.Text
                        arySubDoc(row)(1) = txt_desc.Text
                        arySubDoc(row)(2) = txtdocDate.Value
                        arySubDoc(row)(3) = txtAmount.Text
                        arySubDoc(row)(4) = index.Text
                        If _exceedCutOffDt = "Yes" And strIsGst = "Yes" Then
                            arySubDoc(row)(5) = txtGSTAmount.Text
                        ElseIf _exceedCutOffDt = "Yes" And strIsGst = "No" Then
                            arySubDoc(row)(5) = txtGSTAmount.Text
                        Else
                            arySubDoc(row)(5) = 0.0
                        End If
                    End If
                Next
            End If
        Next
        Session("SubDocument") = arySubDoc

        'Save Data into DB
        SaveIPPSubDoc()

        If Not Session("DocNo") Is Nothing Then
            Dim arySubDoc2 As New ArrayList
            Dim dsSubDoc As New DataSet : dsSubDoc = objDoc.GetSubDoc(Session("DocNo"))
            Dim totalAmt As Double : totalAmt = 0.0
            Dim rownum As Integer = 1
            If dsSubDoc.Tables(0).Rows.Count > 0 Then
                For row As Integer = 0 To dsSubDoc.Tables(0).Rows.Count - 1
                    If _exceedCutOffDt = "Yes" And strIsGst = "Yes" Then
                        arySubDoc2.Add(New String() {rownum, dsSubDoc.Tables(0).Rows(row).Item("isd_doc_no"), dsSubDoc.Tables(0).Rows(row).Item("isd_doc_date"), dsSubDoc.Tables(0).Rows(row).Item("isd_doc_amt"), dsSubDoc.Tables(0).Rows(row).Item("isd_sub_doc_index"), dsSubDoc.Tables(0).Rows(row).Item("isd_doc_gst_value")})
                    ElseIf _exceedCutOffDt = "Yes" And strIsGst = "No" Then
                        arySubDoc2.Add(New String() {rownum, dsSubDoc.Tables(0).Rows(row).Item("isd_doc_no"), dsSubDoc.Tables(0).Rows(row).Item("isd_doc_date"), dsSubDoc.Tables(0).Rows(row).Item("isd_doc_amt"), dsSubDoc.Tables(0).Rows(row).Item("isd_sub_doc_index"), dsSubDoc.Tables(0).Rows(row).Item("isd_doc_gst_value")})

                    Else
                        arySubDoc2.Add(New String() {rownum, dsSubDoc.Tables(0).Rows(row).Item("isd_doc_no"), dsSubDoc.Tables(0).Rows(row).Item("isd_doc_date"), dsSubDoc.Tables(0).Rows(row).Item("isd_doc_amt"), dsSubDoc.Tables(0).Rows(row).Item("isd_sub_doc_index"), 0.0})
                    End If
                    rownum = rownum + 1
                Next
            End If
            Session("SubDocument") = arySubDoc2
        End If

        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)

        If Not Request.QueryString("LineNo") Is Nothing Then
            'Tis coming from edit button
            AddRow(1)
            BindRulesCat(Request.QueryString("LineNo"))
            Me.dtg_SubDoc.Columns(0).Visible = False
        End If

    End Sub

    Protected Sub SaveIPPSubDoc()
        Try
            Dim objDoc As New IPPMain
            objDoc.SaveIPPSubDoc(Session("SubDocument"), Session("DocNo"))
        Catch ex As Exception
            Common.TrwExp(ex)
        End Try
    End Sub

    Sub AddRow(ByVal intRow As Integer)
        Dim dt As New DataTable
        Dim row As DataRow
        Dim i, y As Integer
        dt.Columns.Add("txt_desc", GetType(String))
        For i = 1 To intRow
            row = dt.NewRow()
            dt.Rows.Add(row)
            Dim txtTotal As New TextBox
        Next
        dtg_SubDoc.DataSource = dt
        dtg_SubDoc.DataBind()
        buildjava()
    End Sub

    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub

    Private Sub dtg_SubDoc_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_SubDoc.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lineNo As Label
            Dim txtAmount As TextBox
            lineNo = e.Item.FindControl("lineNo")
            txtAmount = e.Item.FindControl("txtAmount")
            lineNo.Text = e.Item.ItemIndex + 1
            txtAmount.Attributes.Add("onkeypress", "return isDecimalKey(event);")
        End If
    End Sub

    Sub buildjava()
        Dim typeahead As String
        typeahead = "<script language=""javascript"">" & vbCrLf & _
          "<!--" & vbCrLf & _
            "$(document).ready(function(){" & vbCrLf & _
            Session("content") & vbCrLf & _
            "});" & vbCrLf & _
            "-->" & vbCrLf & _
            "</script>"
        Session("typeahead") = typeahead
    End Sub
    Public Sub calculateTotal()
        Dim item As DataGridItem
        Dim total As Double : total = 0.0
        Dim amount As Double : amount = 0.0
        Dim GSTAmount As Double : GSTAmount = 0.0
        Dim GSTTotal As Double : total = 0.0
        For Each item In dtg_SubDoc.Items
            If Not CType(item.FindControl("txtAmount"), TextBox).Text.Length = 0 Then
                total += CType(item.FindControl("txtAmount"), TextBox).Text
                amount = CType(item.FindControl("txtAmount"), TextBox).Text
                CType(item.FindControl("txtAmount"), TextBox).Text = Format(amount, "#.00")
            End If
            If _exceedCutOffDt = "Yes" And strIsGst = "Yes" Then
                If Not CType(item.FindControl("txtGSTAmount"), TextBox).Text.Length = 0 Then
                    GSTTotal += CType(item.FindControl("txtGSTAmount"), TextBox).Text
                    GSTAmount = CType(item.FindControl("txtGSTAmount"), TextBox).Text
                    If GSTAmount = 0.0 Then
                        CType(item.FindControl("txtGSTAmount"), TextBox).Text = "0.00"
                    ElseIf CDec(GSTAmount) < CDec(1) Then
                        CType(item.FindControl("txtGSTAmount"), TextBox).Text = CType(item.FindControl("txtGSTAmount"), TextBox).Text
                    Else
                        CType(item.FindControl("txtGSTAmount"), TextBox).Text = Format(GSTAmount, "#.00")
                    End If
                End If
            Else
                CType(item.FindControl("txtGSTAmount"), TextBox).Text = "N/A"
                CType(item.FindControl("txtGSTAmount"), TextBox).Enabled = False
            End If
        Next
        lbl1.Text = "GST Total :"
        If Not total = 0.0 Then
            If Not GSTTotal = 0.0 Then
                total = total + GSTTotal
            End If
        End If

        If Not total = 0.0 Then lblamt.Text = Format(total, "#.00") Else lblamt.Text = "0.00"
        If GSTTotal >= 1.0 Then
            lblGSTAmt.Text = Format(GSTTotal, "#.00")
        ElseIf GSTTotal < 1.0 And GSTTotal > 0.0 Then
            lblGSTAmt.Text = FormatNumber(GSTTotal, 2)
        Else
            lblGSTAmt.Text = "0.00"
        End If
    End Sub
    Public Function validation() As Boolean
        Try

            'SubDoc Validation
            Dim strSql = ""
            Dim result = ""
            Dim subdocAmt As Double
            Dim ds As DataSet
            Dim i As Integer = 0
            Dim vendIdx As Integer
            If Not Session("DocNo_Amt") Is Nothing Then
                For Each dgItemFF As DataGridItem In dtg_SubDoc.Items

                    'Get items
                    Dim lineNo, index As Label
                    Dim txt_desc, txt_desc2 As TextBox
                    Dim txtdocDate As New HtmlInputText
                    Dim txtAmount, txtGSTAmount As New TextBox
                    Dim counter As Integer = 0
                    lineNo = dgItemFF.FindControl("LineNo")
                    txt_desc = dgItemFF.FindControl("txt_desc")
                    txtdocDate = dgItemFF.FindControl("txtdocDate")
                    txtAmount = dgItemFF.FindControl("txtAmount")
                    index = dgItemFF.FindControl("index")
                    txtGSTAmount = dgItemFF.FindControl("txtGSTAmount")

                    'Validation for 2 items wtih te same num
                    For Each dgItem As DataGridItem In dtg_SubDoc.Items
                        If counter > 1 Then Exit For
                        txt_desc2 = dgItem.FindControl("txt_desc")
                        If Not txt_desc2.Text = "" And Not txt_desc.Text = "" Then
                            If txt_desc2.Text = txt_desc.Text Then
                                counter += 1
                            End If
                        End If
                    Next

                    If CDec(counter) > CDec(1) Then
                        Common.NetMsgbox(Me, "Sub Doc No cannot be duplicated.", MsgBoxStyle.Information)
                        Return False
                        Exit Function
                    End If

                    'Items are cumpolsary
                    If txt_desc.Text = "" And txtdocDate.Value = "" And txtAmount.Text = "" And i = 0 And txtGSTAmount.Text = "" Then
                        Common.NetMsgbox(Me, "Please insert Sub Doc No,Sub Doc Date, and Sub Doc Amount.", MsgBoxStyle.Information)
                        Return False
                        Exit Function
                    End If

                    If txt_desc.Text = "" And txtdocDate.Value <> "" And txtAmount.Text <> "" And txtGSTAmount.Text <> "" Then
                        Common.NetMsgbox(Me, "Please insert Sub Doc No.", MsgBoxStyle.Information)
                        Return False
                        Exit Function
                    ElseIf txt_desc.Text <> "" And txtdocDate.Value = "" And txtAmount.Text <> "" And txtGSTAmount.Text <> "" Then
                        Common.NetMsgbox(Me, "Please insert Sub Doc Date.", MsgBoxStyle.Information)
                        Return False
                        Exit Function
                    ElseIf txt_desc.Text <> "" And txtdocDate.Value <> "" And txtAmount.Text = "" And txtGSTAmount.Text <> "" Then
                        Common.NetMsgbox(Me, "Please insert Sub Doc Amount.", MsgBoxStyle.Information)
                        Return False
                        Exit Function
                    ElseIf txt_desc.Text <> "" And (txtdocDate.Value = "" And txtAmount.Text = "" And txtGSTAmount.Text = "") Then
                        Common.NetMsgbox(Me, "Please insert Sub Doc Date and Amount.", MsgBoxStyle.Information)
                        Return False
                        Exit Function
                    ElseIf txt_desc.Text <> "" And txtdocDate.Value <> "" And txtAmount.Text <> "" And txtGSTAmount.Text = "" Then
                        If _exceedCutOffDt = "Yes" And strIsGst = "Yes" Then
                            Common.NetMsgbox(Me, "Please insert Sub Doc GST Amount.", MsgBoxStyle.Information)
                            Return False
                            Exit Function
                        End If
                    End If

                    If _exceedCutOffDt = "Yes" And strIsGst = "No" Then
                        If txtGSTAmount.Text.Trim.Length > 0 Then
                            If Not txtGSTAmount.Text = "N/A" Then
                                If CDec(txtGSTAmount.Text.Trim) > CDec(0) Then
                                    Common.NetMsgbox(Me, "GST Amount should be 0.", MsgBoxStyle.Information)
                                    Return False
                                    Exit Function
                                End If
                            End If
                        End If
                    End If

                    'Check Similar Doc No
                    Dim invoiceindex As Integer
                    If Session("DocNo_Amt").ToString.Split("|")(0).Trim.ToString = txt_desc.Text Then
                        Common.NetMsgbox(Me, "Sub Document No must not equal to Master Document No.", MsgBoxStyle.Information)
                        Return False
                        Exit Function
                    End If
                    'Check Existing Doc No
                    If Session("DocNo") Is Nothing And Session("DocNo_Amt").ToString.Split("|")(2) IsNot Nothing Then
                        Session("DocNo") = Session("DocNo_Amt").ToString.Split("|")(2)
                    End If
                    If Not Session("DocNo") Is Nothing Then
                        'Jules 2018.07.11 - PAMB Allow "\" and "#"
                        result = objDB.GetVal("select isd_sub_doc_index from ipp_sub_doc where isd_mstr_doc_index = " & Session("DocNo") & " and isd_doc_no = '" & Common.Parse(Replace(Replace(txt_desc.Text, "\", "\\"), "#", "\#")) & "'")
                        If Not result = "" Then
                            If index.Text = "" Then 'for new
                                Common.NetMsgbox(Me, "Sub Document No must not already exist in the system.", MsgBoxStyle.Information)
                                Return False
                                Exit Function
                            ElseIf index.Text <> CDec(result) Then 'for editing
                                Common.NetMsgbox(Me, "Sub Document No must not already exist in the system.", MsgBoxStyle.Information)
                                Return False
                                Exit Function
                            End If
                        End If
                    End If

                    If Not result = "" Then
                        If index.Text <> CDec(result) Then
                            vendIdx = objDB.GetVal("select im_s_coy_id from invoice_mstr where im_invoice_index = " & Session("DocNo"))
                            'Jules 2018.07.11 - Allow "\" and "#"
                            ''Same vendor - two diff invoices - sub doc no in one of the invoices <> the other invoice no
                            strSql = "SELECT im_invoice_index, IM_INVOICE_NO FROM invoice_mstr WHERE im_invoice_index IN (SELECT DISTINCT(isd_mstr_doc_index) FROM ipp_SUB_DOC WHERE " &
                             "ISD_DOC_NO LIKE '" & Common.Parse(Replace(Replace(txt_desc.Text, "\", "\\"), "#", "\#")) & "') AND im_s_coy_id = '" & vendIdx & "' AND im_b_coy_id = '" & Common.Parse(Common.Parse(HttpContext.Current.Session("CompanyID"))) & "' AND IM_PO_INDEX IS NULL"
                            ds = objDB.FillDs(strSql)
                            If ds.Tables(0).Rows.Count > 0 Then
                                Common.NetMsgbox(Me, "Sub Document No must not already exist in the system.")
                                Session("SubDocument") = Nothing
                                Exit Function
                            End If

                            ''Same vendor - two diff invoices - sub doc no in one of the invoices <> sub doc no in the other invoice no
                            strSql = "SELECT IM_INVOICE_NO FROM invoice_mstr WHERE im_invoice_index IN (SELECT DISTINCT(isd_mstr_doc_index) FROM ipp_SUB_DOC WHERE ISD_DOC_NO LIKE '" & Common.Parse(Replace(Replace(txt_desc.Text, "\", "\\"), "#", "\#")) & "') AND " &
                                     "im_s_coy_id = '" & vendIdx & "' AND im_b_coy_id = '" & Common.Parse(Common.Parse(HttpContext.Current.Session("CompanyID"))) & "' AND IM_PO_INDEX IS NULL"
                            ds = objDB.FillDs(strSql)
                            If ds.Tables(0).Rows.Count > 0 Then
                                Common.NetMsgbox(Me, "Sub Document No must not already exist in the system.")
                                Session("SubDocument") = Nothing
                                Exit Function
                            End If

                            strSql = "SELECT IM_INVOICE_NO FROM invoice_mstr WHERE im_invoice_no IN ('" & Common.Parse(Replace(Replace(txt_desc.Text, "\", "\\"), "#", "\#")) & "') AND " &
                                     "im_s_coy_id = '" & vendIdx & "' AND im_b_coy_id = '" & Common.Parse(Common.Parse(HttpContext.Current.Session("CompanyID"))) & "' " &
                                     "AND IM_PO_INDEX IS NULL"
                            ds = objDB.FillDs(strSql)
                            If ds.Tables(0).Rows.Count > 0 Then
                                Common.NetMsgbox(Me, "Sub Document No must not equal to Master Document No.")
                                Session("SubDocument") = Nothing
                                Exit Function
                            End If
                        End If
                    End If

                    i += 1

                Next
            End If
            'End
            Return True
        Catch ex As Exception

        End Try
    End Function
End Class
