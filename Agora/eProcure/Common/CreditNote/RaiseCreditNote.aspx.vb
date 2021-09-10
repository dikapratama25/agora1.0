'Jules 2015.02.02 Agora Stage 2
Imports AgoraLegacy
Imports eProcure.Component
Imports System.Web.UI

Public Class RaiseCreditNote
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals
    Dim aryPTaxCode As New ArrayList()
    Dim intRow As Integer

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub   
#End Region

    Public Enum EnumCreditNote
        icItemLine = 0
        icItemName = 1
        icUOM = 2
        icQty = 3
        icUnitPrice = 4
        icAmount = 5
        icGSTRateDesc = 6
        icGSTAmount = 7
        icGSTTaxCode = 8
        icRemarks = 9
        icGSTRate = 10
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)

        blnPaging = False
        blnSorting = False
        SetGridProperty(dtg_CreditNoteDetail)

        Dim objDB As New EAD.DBCom
        Dim objCompany As New Companies
        Dim objTrac As New Tracking
        Dim objCN As New CreditNote
        Dim a As New AppGlobals
        Dim strAllCN As String, strCN() As String, strTempCN As String = ""
        Dim i As Integer
        Dim decCNAmt As Decimal

        If Not IsPostBack Then

            Dim objval As New InvValue
            Dim objinv As New Invoice

            'Issue 7480 - CH - 23 Mar 2015 (No.35)
            'First delete temp attachment for CN 
            objCN.delCNAttachTemp(, Session.SessionID)

            objval.Inv_no = Request(Trim("INVNO"))
            objval.Vcom_id = Session("CompanyId")
            objinv.get_invmstr(objval)
            ViewState("b_coy_id") = objDB.GetVal("SELECT IM_B_COY_ID FROM INVOICE_MSTR WHERE IM_INVOICE_NO='" & objval.Inv_no & "' AND IM_S_COY_ID='" & objval.Vcom_id & "'")

            Bindgrid()
            If intPageRecordCnt > 0 Then
                Me.lblInvoiceAmount.Text = Format(((ViewState("total") + ViewState("gst") + ViewState("ShipAmt")) / 100) * CInt(ViewState("Tax")), "#,##0.00")
            End If
            ViewState("po_no") = objval.po_no
            ViewState("Inv_no") = Request(Trim("INVNO"))
            'Me.lblInvNo.Text = Request(Trim("INVNO"))
            Me.lblInvNo.Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewInvoice.aspx", "pageid=" & strPageId & "&INVNO=" & Request(Trim("INVNO")) & "&vcomid=" & Session("CompanyId") & "&BCoyID=" & ViewState("b_coy_id")) & "')"" ><font color=#0000ff>" & Request(Trim("INVNO")) & "</font></A>"
            ViewState("INVNO") = Request(Trim("INVNO"))
            Me.lblCreditNoteDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Now)
            Me.lblInvoiceDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objval.create_on)
            Me.lblBuyerAddress.Text = objval.adds
            Me.lblCurrency.Text = objval.cur
            Dim InvAmt As Decimal = objDB.GetVal("SELECT IM_INVOICE_TOTAL FROM INVOICE_MSTR WHERE IM_INVOICE_NO='" & objval.Inv_no & "' AND IM_S_COY_ID='" & objval.Vcom_id & "'")
            Me.lblInvoiceAmount.Text = Format(InvAmt, "#,##0.00")
            Me.hidInvAmt.Value = ViewState("total")
            'Me.lblRelatedCreditNote.Text = objCN.getRelatedCreditNotes(ViewState("b_coy_id"), Session("CompanyId"), Request(Trim("INVNO")))
            strAllCN = objCN.getRelatedCreditNotes(ViewState("b_coy_id"), Session("CompanyId"), Request(Trim("INVNO")))
            If strAllCN <> "" Then
                strCN = Split(strAllCN, ",")
                For i = 0 To strCN.Length - 1
                    If strTempCN = "" Then
                        strTempCN = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewCreditNote.aspx", "pageid=" & strPageId & "&CN_No=" & strCN(i) & "&BCoyID=" & ViewState("b_coy_id") & "&SCoyID=" & Session("CompanyId") & "')"" ><font color=#0000ff>" & strCN(i)) & "</font></A>"
                    Else
                        strTempCN &= ", " & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewCreditNote.aspx", "pageid=" & strPageId & "&CN_No=" & strCN(i) & "&BCoyID=" & ViewState("b_coy_id") & "&SCoyID=" & Session("CompanyId") & "')"" ><font color=#0000ff>" & strCN(i)) & "</font></A>"
                    End If
                Next
                Me.lblRelatedCreditNote.Text = strTempCN
            Else
                Me.lblRelatedCreditNote.Text = ""
            End If
            decCNAmt = objCN.getRelatedCreditNoteTotal(ViewState("b_coy_id"), Session("CompanyId"), Request(Trim("INVNO")))
            Me.lblRelatedCreditNoteAmt.Text = Format(decCNAmt, "#,##0.00")
            Me.hidRelatedCNTotal.Value = Format(decCNAmt, "#,##0.00")
            Me.lblNetAmt.Text = Format(InvAmt - decCNAmt, "##,##0.00")

            'If Session("urlreferer") = "RaiseCreditNote" Then
            lnkBack.NavigateUrl = dDispatcher.direct("DebitNote", "DebitCreditNoteList.aspx", "pageid=" & strPageId)
            'End If
            GenerateTab()

            cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & File1Int.ClientID & "');")
            Me.cmdView.Enabled = False ' Jules 2015.02.17 - Agora Stage 2
        End If
        body1.Attributes.Add("onLoad", "refreshDatagrid(); calculateGrandTotal(); calculateAllIndividualTotal(); ")

        displayAttachFile()
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim objInv As New Invoice
        Dim ds As DataSet
        Dim dvViewInv As DataView

        intRow = 0
        ds = objInv.GetInvDetailForCNDN(Request(Trim("INVNO")), Session("CompanyId"))
        dvViewInv = ds.Tables(0).DefaultView
        ViewState("TaxInvoice") = ds.Tables(0).Rows(0).Item("IM_GST_INVOICE")

        If ViewState("TaxInvoice") = "Y" Then
            hidGst.Value = True
        End If

        If pSorted Then
            dvViewInv.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewInv.Sort += " DESC"
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            dtg_CreditNoteDetail.DataSource = dvViewInv
            dtg_CreditNoteDetail.DataBind()
        Else
            dtg_CreditNoteDetail.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

    End Function

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Jules 2015.02.16 - Agora Stage 2
        Session("w_Debit_Credit_tabs") = "<div class=""t_entity""><ul>" &
         "<li><div class=""space""></div></li>" &
                            "<li><a class=""t_entity_btn_selected"" href=""#""><span>Debit Note / Credit Note</span></a></li>" &
                            "<li><div class=""space""></div></li>" &
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteListing.aspx", "pageid=" & strPageId) & """><span>Debit Note Listing</span></a></li>" &
                            "<li><div class=""space""></div></li>" &
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("CreditNote", "CreditNoteListing.aspx", "pageid=" & strPageId) & """><span>Credit Note Listing</span></a></li>" &
                            "<li><div class=""space""></div></li>" &
                           "</ul><div></div></div>"
    End Sub

    Private Sub dtg_CreditNoteDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_CreditNoteDetail.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dblSubTotal As Double
            Dim objinv As New Invoice
            Dim dsCustom As New DataSet
            Dim dsCustomField As New DataSet
            Dim ds As New DataSet
            Dim lstItem As New ListItem
            Dim objTrac As New Tracking

            'Dim ddlGSTRate As DropDownList
            Dim ddlTaxCode As DropDownList
            Dim txtQty As TextBox
            Dim txtPrice As TextBox
            Dim txtAmount As TextBox
            Dim txtGSTValue As TextBox
            Dim txtRemark As TextBox
            Dim txtGST As TextBox
            'ddlGSTRate = e.Item.Cells(EnumCreditNote.icGSTRate).FindControl("ddlGSTRate")
            ddlTaxCode = e.Item.Cells(EnumCreditNote.icGSTTaxCode).FindControl("ddlTaxCode")
            txtQty = e.Item.Cells(EnumCreditNote.icQty).FindControl("txtQty")
            txtPrice = e.Item.Cells(EnumCreditNote.icQty).FindControl("txtPrice")
            txtAmount = e.Item.Cells(EnumCreditNote.icQty).FindControl("txtAmount")
            txtGSTValue = e.Item.Cells(EnumCreditNote.icQty).FindControl("txtGSTValue")
            txtRemark = e.Item.Cells(EnumCreditNote.icQty).FindControl("txtRemark")
            txtGST = e.Item.Cells(EnumCreditNote.icGSTAmount).FindControl("txtGST")

            Dim valQty As RegularExpressionValidator
            valQty = e.Item.FindControl("valQty")
            valQty.ControlToValidate = "txtQty"
            valQty.ValidationExpression = "^\d{1,10}(\.\d{1,2})?$$" '"^([1-9]\d{0,5}\.\d{0,2}|[1-9]\d{0,5}|[0]\d{0,5}\.[1-9]\d{0,1})$"
            valQty.ErrorMessage = CStr(intRow + 1) & ". " & ViewState("ValQtyMsg")
            valQty.Text = "?"
            valQty.Display = ValidatorDisplay.Dynamic

            Dim revPrice As RegularExpressionValidator
            revPrice = e.Item.FindControl("revPrice")
            revPrice.ControlToValidate = "txtPrice"
            revPrice.ValidationExpression = "^\d{1,10}(\.\d{1,4})?$"
            revPrice.ErrorMessage = CStr(intRow + 1) & ". " & "Unit price is expecting numeric value."
            revPrice.Text = "?"
            revPrice.Display = ValidatorDisplay.Dynamic

            If ViewState("TaxInvoice") = "Y" Then
                objGlobal.FillTaxCode(ddlTaxCode, Common.parseNull(dv("ID_GST_RATE")), "S", , True)
                If ddlTaxCode.SelectedValue = "N/A" Then
                    ddlTaxCode.Enabled = False
                Else
                    ddlTaxCode.Enabled = True
                End If
            Else
                lstItem.Value = "N/A"
                lstItem.Text = "N/A"
                ddlTaxCode.Items.Clear()
                ddlTaxCode.Items.Insert(0, lstItem)
                ddlTaxCode.Enabled = False
            End If
            'ddlTaxCode.Visible = True

            'txtQty.Text = Common.parseNull(dv("ID_RECEIVED_QTY"), 0)
            dblSubTotal = Common.parseNull(dv("ID_RECEIVED_QTY"), 0) * Common.parseNull(dv("ID_UNIT_COST"), 0) + Common.parseNull(dv("ID_GST_VALUE"), 0)
            'txtAmount.Text = Format(dblSubTotal, "#,##0.00")
            'txtPrice.Text = Format(dv("ID_UNIT_COST"), "#,##0.0000")
            ViewState("total") = ViewState("total") + CDbl(Format(dblSubTotal, "#,##0.00"))
            'txtGSTValue.Text = Format(dblSubTotal / 100 * Common.parseNull(dv("ID_GST")), "#,##0.00")
            ViewState("gst") = ViewState("gst") + CDbl(Format((dblSubTotal / 100 * Common.parseNull(dv("ID_GST"))), "#,##0.00"))
            ViewState("ShipAmt") = Format(dv("IM_SHIP_AMT"), "#,##0.00")
            txtQty.Text = "0.00"
            txtAmount.Text = "0.00"
            txtPrice.Text = "0.00"
            txtGSTValue.Text = "0.00"

            If IsDBNull(dv("ID_GST")) Then
                txtGST.Text = 0
            Else
                If ViewState("TaxInvoice") = "Y" Then
                    txtGST.Text = dv("ID_GST")
                Else
                    txtGST.Text = 0
                End If
            End If
            txtGST.Style("display") = "none"

            If IsDBNull(dv("ID_GST_RATE")) Then
                e.Item.Cells(EnumCreditNote.icGSTRate).Text = "N/A"
            Else
                e.Item.Cells(EnumCreditNote.icGSTRate).Text = dv("ID_GST_RATE")
            End If

            If IsDBNull(dv("IM_WITHHOLDING_TAX")) Then
                ViewState("Tax") = 0
            Else
                ViewState("Tax") = dv("IM_WITHHOLDING_TAX")
            End If

            Dim sClientId As String, sTotalClient As String

            sTotalClient = hidClientId.Value

            Dim CheckInstr As String
            CheckInstr = Mid(txtAmount.ClientID, InStr(txtAmount.ClientID, "_") + 1, Len(txtAmount.ClientID))

            sClientId = Mid(CheckInstr,
            InStr(CheckInstr, "_") + 1,
            InStr(Mid(CheckInstr,
            InStr(CheckInstr, "_") + 1), "_") - 1) & "|"


            If Not sTotalClient.Contains(sClientId) Then
                hidClientId.Value = hidClientId.Value & sClientId
                hidTotalClientId.Value = hidTotalClientId.Value + 1
            End If

            txtQty.Attributes.Add("onfocus", "return focusControl('" &
              txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" & txtAmount.ClientID & "', '" & txtGST.ClientID & "', '" & txtGSTValue.ClientID & "');")

            txtQty.Attributes.Add("onkeyup", "return calculateTotal('" &
                           txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" & txtAmount.ClientID & "', '" & txtGST.ClientID & "', '" & txtGSTValue.ClientID & "');")

            txtPrice.Attributes.Add("onfocus", "return focusControl('" &
                                     txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" & txtAmount.ClientID & "', '" & txtGST.ClientID & "', '" & txtGSTValue.ClientID & "');")

            txtPrice.Attributes.Add("onkeyup", "return calculateTotal('" &
                           txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" & txtAmount.ClientID & "', '" & txtGST.ClientID & "', '" & txtGSTValue.ClientID & "');")

            'ddlGSTRate.Attributes.Add("onchange", "return calculateTotal('" & _
            '               txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" & txtAmount.ClientID & "', '" & ddlGSTRate.ClientID & "', '" & txtGSTValue.ClientID & "');")

            Dim strCNDetails As New ArrayList()
            If Session("strCNDetails") IsNot Nothing Then
                strCNDetails = Session("strCNDetails")
                If (CInt(e.Item.Cells(EnumCreditNote.icItemLine).Text) - 1) <= strCNDetails.Count - 1 Then
                    txtQty.Text = strCNDetails(CInt(e.Item.Cells(EnumCreditNote.icItemLine).Text) - 1)(0)
                    txtPrice.Text = strCNDetails(CInt(e.Item.Cells(EnumCreditNote.icItemLine).Text) - 1)(1)
                    ddlTaxCode.SelectedValue = strCNDetails(CInt(e.Item.Cells(EnumCreditNote.icItemLine).Text) - 1)(2)
                    txtRemark.Text = strCNDetails(CInt(e.Item.Cells(EnumCreditNote.icItemLine).Text) - 1)(3)
                    'ddlGSTRate.SelectedValue = strCNDetails(CInt(e.Item.Cells(EnumCreditNote.icItemLine).Text) - 1)(2)
                    'ddlTaxCode.SelectedValue = strCNDetails(CInt(e.Item.Cells(EnumCreditNote.icItemLine).Text) - 1)(3)
                    'txtRemark.Text = strCNDetails(CInt(e.Item.Cells(EnumCreditNote.icItemLine).Text) - 1)(4)
                End If
            End If
            If ViewState("remarks") IsNot Nothing Then
                txtRemarks.Text = ViewState("remarks")
            End If

            intRow = intRow + 1
        End If
    End Sub

    Private Function SubmitCN()
        Dim objDB As New EAD.DBCom
        Dim objinv As New Invoice
        Dim objCN As New CreditNote
        Dim objval As New InvValue
        Dim DS As New DataSet
        Dim dtr As DataRow
        Dim dtrd As DataRow
        Dim CNmstr As New DataTable
        Dim CNDetails As New DataTable
        Dim strCN As String
        Dim strbcomid As String
        Dim strMsg As String = ""

        CNmstr.Columns.Add("b_com_id", Type.GetType("System.String"))
        CNmstr.Columns.Add("po_no", Type.GetType("System.String"))
        CNmstr.Columns.Add("cn_type", Type.GetType("System.String"))
        CNmstr.Columns.Add("inv_no", Type.GetType("System.String"))
        CNmstr.Columns.Add("currency", Type.GetType("System.String"))
        CNmstr.Columns.Add("exchange_rate", Type.GetType("System.Double"))
        CNmstr.Columns.Add("remark", Type.GetType("System.String"))
        CNmstr.Columns.Add("total", Type.GetType("System.Double")) 'Jules 2015.02.16 - Agora Stage 2
        CNmstr.Columns.Add("inv_total", Type.GetType("System.Double")) 'CH - 13 Apr 2015 - Inv Total

        dtr = CNmstr.NewRow()
        dtr("b_com_id") = ViewState("b_coy_id")
        dtr("po_no") = ViewState("po_no")
        dtr("cn_type") = IIf(ViewState("TaxInvoice") = "Y", "CN", "CA")
        dtr("inv_no") = ViewState("Inv_no")
        dtr("currency") = Me.lblCurrency.Text
        'Issue 7480 - CH - 23 Mar 2015 (No.47)
        'dtr("exchange_rate") = Common.parseNull(objDB.GetVal("SELECT IFNULL(pom_exchange_rate,1) FROM po_mstr WHERE pom_po_no='" & ViewState("po_no") & "' AND pom_s_coy_id='" & Session("CompanyId") & "' AND pom_b_coy_id='" & ViewState("b_coy_id") & "'"), 0)
        'Get latest Exchange Rate
        Dim decCurRate As Decimal
        Dim strCoyCur, strTemp As String
        strTemp = objDB.GetVal("SELECT CE_RATE FROM company_exchangerate WHERE CE_COY_ID = '" & Session("CompanyId") & "' AND CE_CURRENCY_CODE = '" & Me.lblCurrency.Text & "' AND CE_DELETED='N' AND CE_VALID_FROM <= CURRENT_DATE() AND CE_VALID_TO >= CURRENT_DATE()")
        If strTemp = "" Then
            strCoyCur = objDB.GetVal("SELECT CM_CURRENCY_CODE FROM company_mstr WHERE CM_COY_ID = '" & Session("CompanyId") & "'")
            If strCoyCur = Me.lblCurrency.Text Then
                decCurRate = 1
            Else
                decCurRate = 0
            End If
        Else
            decCurRate = CDec(strTemp)
        End If
        dtr("exchange_rate") = decCurRate
        dtr("remark") = txtRemarks.Text
        dtr("total") = hidCNTotal.Value 'Jules 2015.02.16 - Agora Stage 2
        dtr("inv_total") = hidInvAmt.Value 'CH - 13 Apr 2015 - Inv Total

        CNDetails.Columns.Add("inv_line_no", Type.GetType("System.Int32"))
        CNDetails.Columns.Add("qty", Type.GetType("System.Double"))
        CNDetails.Columns.Add("unit_price", Type.GetType("System.Double"))
        CNDetails.Columns.Add("gst_rate", Type.GetType("System.String"))
        CNDetails.Columns.Add("output_tax_code", Type.GetType("System.String"))
        CNDetails.Columns.Add("remarks", Type.GetType("System.String")) 'Issue 7480 - CH - 23 Mar 2015 (No.42)
        CNDetails.Columns.Add("amt", Type.GetType("System.Double"))
        CNDetails.Columns.Add("gst_amt", Type.GetType("System.Double"))
        CNmstr.Rows.Add(dtr)

        Dim dgItem As DataGridItem
        'Dim ddlGSTRate As DropDownList
        Dim ddlTaxCode As DropDownList
        Dim txtQty As TextBox
        Dim txtPrice As TextBox
        Dim txtAmount As TextBox
        Dim txtGSTValue As TextBox
        Dim txtRemark As TextBox

        For Each dgItem In dtg_CreditNoteDetail.Items
            'ddlGSTRate = dgItem.Cells(EnumCreditNote.icGSTRate).FindControl("ddlGSTRate")
            ddlTaxCode = dgItem.Cells(EnumCreditNote.icGSTTaxCode).FindControl("ddlTaxCode")
            txtQty = dgItem.Cells(EnumCreditNote.icQty).FindControl("txtQty")
            txtPrice = dgItem.Cells(EnumCreditNote.icUnitPrice).FindControl("txtPrice")
            txtAmount = dgItem.Cells(EnumCreditNote.icAmount).FindControl("txtAmount")
            txtGSTValue = dgItem.Cells(EnumCreditNote.icGSTAmount).FindControl("txtGSTValue")
            txtRemark = dgItem.Cells(EnumCreditNote.icRemarks).FindControl("txtRemark")

            dtrd = CNDetails.NewRow()
            dtrd("inv_line_no") = dgItem.Cells(EnumCreditNote.icItemLine).Text
            dtrd("qty") = txtQty.Text
            dtrd("unit_price") = txtPrice.Text
            dtrd("gst_rate") = dgItem.Cells(EnumCreditNote.icGSTRate).Text
            dtrd("output_tax_code") = ddlTaxCode.SelectedValue
            dtrd("remarks") = txtRemark.Text 'Issue 7480 - CH - 23 Mar 2015 (No.42)
            dtrd("amt") = txtAmount.Text
            dtrd("gst_amt") = txtGSTValue.Text

            CNDetails.Rows.Add(dtrd)
        Next

        DS.Tables.Add(CNmstr)
        DS.Tables.Add(CNDetails)

        Dim strMsgLine As String = ""

        If Not objCN.Update_CreditNoteMstr(DS, strCN, strbcomid, strMsg) Then
            If strMsg <> "" Then
                If strMsg = "ErrorAmt" Then
                    Common.NetMsgbox(Me, "All CN Total Amount cannot bigger than the Invoice Total Amount.")
                Else
                     lblMsg.Text = strMsg
                End If
            Else
                Common.NetMsgbox(Me, MsgTransDup, dDispatcher.direct("CreditNote", "CreditNoteList.aspx", "pageid=" & strPageId))
            End If
        Else
            If InStr(strCN, ",") > 0 Then
                strMsgLine = "Credit Note Number " & strCN & " has been generated."

            Else
                If strCN = "Generated" Then
                    strMsgLine = "Credit Note Number has been generated."
                Else
                    strMsgLine = "Credit Note Number " & strCN & " has been generated."
                End If
            End If

            Me.lblCreditNoteNum.Text = strCN
            ViewState("CnNo") = strCN
            Common.NetMsgbox(Me, strMsgLine)
            Me.cmdSubmit.Visible = False
            Me.cmdreset.Visible = False
            Me.cmdView.Visible = True
            Me.cmdView.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewCreditNote.aspx", "SCoyID=" & Session("CompanyId") & "&CN_No=" & strCN & "&BCoyID=" & ViewState("b_coy_id")) & "')") 'Jules 2015-Feb-24 Agora Stage 2
            Me.cmdSubmit.Enabled = False
            Me.cmdUpload.Enabled = False
            Me.cmdView.Enabled = True
            lblMsg.Text = "" 'Jules 2015.02.16 - Agora Stage 2
        End If
    End Function

    Private Function validateCNAmount() As Boolean
        Dim objCN As New CreditNote
        Dim decLatestAmt As Decimal
        validateCNAmount = True

        'CH - Issue 7480 (10 Apr 2015) : Current Amt + CN Related Total Amt cannot bigger than Invoice total amt
        decLatestAmt = objCN.getRelatedCreditNoteTotal(ViewState("b_coy_id"), Session("CompanyId"), Request(Trim("INVNO")))
        If Me.hidGrandTotal.Value = 0 Then
            Common.NetMsgbox(Me, "Credit Note Grand Total must not be zero amount.")
            validateCNAmount = False
        Else
            If Me.hidGrandTotal.Value > (CDec(Me.hidInvAmt.Value) - decLatestAmt) Then
                Common.NetMsgbox(Me, "All CN Total Amount cannot bigger than the Invoice Total Amount.")
                validateCNAmount = False
            End If
        End If
        
        Me.lblRelatedCreditNoteAmt.Text = Format(decLatestAmt, "##,###,##0.00")
        Me.hidRelatedCNTotal.Value = Format(decLatestAmt, "##0.00")
        Me.lblNetAmt.Text = Format(CDec(Me.hidInvAmt.Value) - decLatestAmt, "##,###,##0.00")

    End Function

    Private Function validateDatagrid(ByRef strMsg As String) As Boolean
        Dim objCn As New CreditNote
        Dim blnQty, blnUP As Boolean
        Dim decAmt As Decimal
        Dim decOutAmt As Decimal

        validateDatagrid = True
        strMsg = "<ul type='disc'>"

        If Not Common.checkMaxLength(txtRemarks.Text, 1000) Then
            strMsg &= "<li>Remarks is over limit.<ul type='disc'></ul></li>"
            validateDatagrid = False
        End If

        Dim dgItem As DataGridItem
        'Dim ddlGSTRate As DropDownList
        Dim ddlTaxCode As DropDownList
        Dim txtQty As TextBox
        Dim txtPrice As TextBox
        Dim txtAmount As TextBox
        Dim txtGSTValue As TextBox
        Dim txtRemark As TextBox

        For Each dgItem In dtg_CreditNoteDetail.Items
            'ddlGSTRate = dgItem.Cells(EnumCreditNote.icGSTRate).FindControl("ddlGSTRate")
            ddlTaxCode = dgItem.Cells(EnumCreditNote.icGSTTaxCode).FindControl("ddlTaxCode")
            txtQty = dgItem.Cells(EnumCreditNote.icQty).FindControl("txtQty")
            txtPrice = dgItem.Cells(EnumCreditNote.icUnitPrice).FindControl("txtPrice")
            txtAmount = dgItem.Cells(EnumCreditNote.icAmount).FindControl("txtAmount")
            txtGSTValue = dgItem.Cells(EnumCreditNote.icGSTAmount).FindControl("txtGSTValue")
            txtRemark = dgItem.Cells(EnumCreditNote.icRemarks).FindControl("txtRemark")

            'Check if Qty & Unit Price both are zero, then without select Tax Code is allowed.
            If CDec(txtQty.Text) = 0 And CDec(txtPrice.Text) = 0 Then
            Else 'Else either one is zero, then error
                If CDec(txtQty.Text) = 0 Then
                    strMsg &= "<li>Line " & dgItem.Cells(EnumCreditNote.icItemLine).Text & ". Item Qty/ Unit Price value is required if either one is more than zero.<ul type='disc'></ul></li>"
                    validateDatagrid = False
                End If

                If CDec(txtPrice.Text) = 0 Then
                    strMsg &= "<li>Line " & dgItem.Cells(EnumCreditNote.icItemLine).Text & ". Item Qty/ Unit Price value is required if either one is more than zero.<ul type='disc'></ul></li>"
                    validateDatagrid = False
                End If
            End If

            If ViewState("TaxInvoice") = "Y" Then
                'If Invoice GST, must select valid GST Rate
                'If ddlGSTRate.SelectedIndex = 0 Then
                '    strMsg &= "<li>GST Rate " & objGlobal.GetErrorMessage("00001") & "<ul type='disc'></ul></li>"
                '    validateDatagrid = False
                'End If

                'If ddlTaxCode.SelectedIndex = 0 Then
                '    strMsg &= "<li>GST Tax Code " & objGlobal.GetErrorMessage("00001") & "<ul type='disc'></ul></li>"
                '    validateDatagrid = False
                'End If

                'Check if Qty & Unit Price both are zero, then without select Tax Code is allowed.
                If txtQty.Text = 0 And txtPrice.Text = 0 Then
                    If ddlTaxCode.SelectedIndex > 0 Then
                        strMsg &= "<li>Line " & dgItem.Cells(EnumCreditNote.icItemLine).Text & ". Invalid SST tax code found.<ul type='disc'></ul></li>"
                        validateDatagrid = False
                    End If
                Else
                    If ddlTaxCode.SelectedIndex = 0 Then
                        strMsg &= "<li>Line " & dgItem.Cells(EnumCreditNote.icItemLine).Text & ". SST Tax Code " & objGlobal.GetErrorMessage("00001") & "<ul type='disc'></ul></li>"
                        validateDatagrid = False
                    End If
                End If
            End If

            'If validateDatagrid = True Then
            'CH - 13 Apr 2015 : Check outstanding Qty & Price of each item 
            objCn.chkCnItemQtyPrice(ViewState("INVNO"), dgItem.Cells(EnumCreditNote.icItemLine).Text, txtQty.Text, txtPrice.Text, blnQty, blnUP)
            If blnQty = False Then
                'strMsg &= "<li>Line " & dgItem.Cells(EnumCreditNote.icItemLine).Text & ". Total of all Credit Note Item Qty has exceed invoice item Qty.<ul type='disc'></ul></li>"
                strMsg &= "<li>Line " & dgItem.Cells(EnumCreditNote.icItemLine).Text & ". Credit Note Item Qty has exceed invoice item Qty.<ul type='disc'></ul></li>"
                validateDatagrid = False
            End If
            If blnUP = False Then
                strMsg &= "<li>Line " & dgItem.Cells(EnumCreditNote.icItemLine).Text & ". Credit Note Item Unit Price has exceed invoice item Unit Price.<ul type='disc'></ul></li>"
                'strMsg &= "<li>Line " & dgItem.Cells(EnumCreditNote.icItemLine).Text & ". Total of all Credit Note Item Unit Price has exceed invoice item Unit Price.<ul type='disc'></ul></li>"
                validateDatagrid = False
            End If

            If blnQty = True And blnUP = True Then
                decAmt = CDec(txtAmount.Text) + CDec(txtGSTValue.Text)
                decOutAmt = objCn.getCnLineItemAmt(ViewState("INVNO"), dgItem.Cells(EnumCreditNote.icItemLine).Text)
                If decAmt > decOutAmt Then
                    strMsg &= "<li>Line " & dgItem.Cells(EnumCreditNote.icItemLine).Text & ". Total of all Credit Note Item Amount has exceed invoice item Amount.<ul type='disc'></ul></li>"
                    validateDatagrid = False
                End If
            End If
            'End If

        Next

        strMsg &= "</ul>"
    End Function

    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        If File1Int.Value <> "" Then
            Dim objFile As New FileManagement
            ' Restrict user upload size
            'Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'PR' AND SP_PARAM_NAME = 'DocAttachSize'"))
            Dim dblMaxFileSize As Double = Session("FileSize")

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(File1Int.PostedFile.FileName)

            If Len(sFileName) > 50 Then
                Common.NetMsgbox(Me, "File name exceeds 46 character")
            ElseIf File1Int.PostedFile.ContentLength > 0 And File1Int.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                'Issue 7480 - CH - 23 Mar 2015 (No.35)
                objFile.FileUpload(File1Int, EnumUploadType.DocAttachmentTemp, "CN", EnumUploadFrom.FrontOff, Session.SessionID, , , , , , "I")
            ElseIf File1Int.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            objFile = Nothing
        End If
        displayAttachFile()

        Dim strCNDetails As New ArrayList()
        ViewState("remarks") = txtRemarks.Text

        Dim ddlGSTRate, ddlTaxCode As DropDownList
        Dim txtQty, txtPrice, txtRemark As TextBox
        Dim dgItem As DataGridItem
        For Each dgItem In dtg_CreditNoteDetail.Items
            'ddlGSTRate = dgItem.Cells(EnumCreditNote.icGSTRate).FindControl("ddlGSTRate")
            ddlTaxCode = dgItem.Cells(EnumCreditNote.icGSTTaxCode).FindControl("ddlTaxCode")
            txtQty = dgItem.Cells(EnumCreditNote.icQty).FindControl("txtQty")
            txtPrice = dgItem.Cells(EnumCreditNote.icQty).FindControl("txtPrice")
            txtRemark = dgItem.Cells(EnumCreditNote.icQty).FindControl("txtRemark")

            'strCNDetails.Add(New String() {txtQty.Text, txtPrice.Text, ddlGSTRate.SelectedValue, ddlTaxCode.SelectedValue, txtRemark.Text})
            strCNDetails.Add(New String() {txtQty.Text, txtPrice.Text, ddlTaxCode.SelectedValue, txtRemark.Text})
        Next

        Session("strCNDetails") = strCNDetails

        Bindgrid()
    End Sub

    Private Sub displayAttachFile()
        Dim objPR As New PR
        Dim objCN As New CreditNote
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL As String

        'Issue 7480 - CH - 24 Mar 2015 (No.35)
        If ViewState("CnNo") <> "" Then '
            dsAttach = objCN.getCNAttachment(ViewState("CnNo"), Session("CompanyId"))
        Else
            dsAttach = objCN.getCNTempAttach(Session.SessionID, "I")
        End If

        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                '**********************meilai 25/02/2005****************** 
                'strURL = "<A HREF=../FileDownload.aspx?pb=" & viewstate("postback") & "&file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & ">" & strFile & "</A>"
                Dim objFile As New FileManagement
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "CN", EnumUploadFrom.FrontOff)
                objFile = Nothing
                '**********************meilai*****************************
                Dim lblBr As New Label
                Dim lblFile As New Label
                Dim lnk As New ImageButton
                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"
                lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete2.gif")
                lnk.ID = drvAttach(i)("CDA_ATTACH_INDEX")
                lnk.CausesValidation = False
                AddHandler lnk.Click, AddressOf deleteAttach

                pnlAttach.Controls.Add(lblFile)
                'Issue 7480 - CH - 24 Mar 2015 (No.35)
                If ViewState("CnNo") <> "" Then
                Else
                    pnlAttach.Controls.Add(lnk)
                End If
                pnlAttach.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFile)
        End If
        objPR = Nothing
    End Sub

    Private Sub deleteAttach(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objCN As New CreditNote
        'Issue 7480 - CH - 2015 (No.35)
        'objCN.deleteCNAttachment(CType(sender, ImageButton).ID)
        objCN.delCNAttachTemp(CType(sender, ImageButton).ID)

        displayAttachFile()
        objCN = Nothing
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim strMsg As String = ""
        lblMsg.Text = ""

        If validateDatagrid(strMsg) Then
            If validateCNAmount() Then
                'Exit Sub
                SubmitCN()
            End If
            'If validateDatagrid(strMsg) Then
        Else
            If strMsg <> "" Then
                lblMsg.Text = strMsg
            Else
                lblMsg.Text = ""
            End If
        End If
    End Sub
End Class