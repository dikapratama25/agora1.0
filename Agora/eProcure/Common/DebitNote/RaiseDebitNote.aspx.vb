'Jules 2015.02.02 Agora Stage 2
Imports AgoraLegacy
Imports eProcure.Component
Imports System.Web.UI

Public Class RaiseDebitNote
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals    
    Dim aryPTaxCode As New ArrayList()
    Dim intRow As Integer

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dtg_DebitNoteDetail As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lnkBack As Global.System.Web.UI.WebControls.HyperLink    
    Protected WithEvents cmdView As System.Web.UI.WebControls.Button
#End Region

    Public Enum EnumDebitNote
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
        SetGridProperty(dtg_DebitNoteDetail)

        Dim objDB As New EAD.DBCom
        Dim objCompany As New Companies
        Dim objTrac As New Tracking
        Dim objDN As New DebitNote

        If Not IsPostBack Then

            Dim objval As New InvValue
            Dim objinv As New Invoice
            Dim strAllDN As String, strDN() As String, strTempDN As String = ""
            Dim i As Integer
            Dim decDNAmt As Decimal
            'Issue 7480 - CH - 23 Mar 2015 (No.35)
            'First delete temp attachment for DN 
            objDN.delDNAttachTemp(, Session.SessionID)

            objval.Inv_no = Request(Trim("INVNO"))
            objval.Vcom_id = Session("CompanyId")
            objinv.get_invmstr(objval)
            ViewState("b_coy_id") = objDB.GetVal("SELECT IM_B_COY_ID FROM INVOICE_MSTR WHERE IM_INVOICE_NO='" & objval.Inv_no & "' AND IM_S_COY_ID='" & objval.Vcom_id & "'")
            ViewState("isResident") = objDB.GetVal("select IFNULL(cm_resident,'') from company_mstr where cm_coy_id ='" & objval.Vcom_id & "'") 'Jules 2018.10.19

            Bindgrid()
            If intPageRecordCnt > 0 Then
                Me.lblInvoiceAmount.Text = Format(((ViewState("total") + ViewState("gst") + ViewState("ShipAmt")) / 100) * CInt(ViewState("Tax")), "#,##0.00")
            End If
            ViewState("po_no") = objval.po_no
            'Me.lblInvNo.Text = Request(Trim("INVNO"))
            ViewState("Inv_no") = Request(Trim("INVNO"))
            Me.lblInvNo.Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewInvoice.aspx", "pageid=" & strPageId & "&INVNO=" & Request(Trim("INVNO")) & "&vcomid=" & Session("CompanyId") & "&BCoyID=" & ViewState("b_coy_id")) & "')"" ><font color=#0000ff>" & Request(Trim("INVNO")) & "</font></A>"
            Me.lblDebitNoteDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Now)
            Me.lblInvoiceDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objval.create_on)
            Me.lblBuyerAddress.Text = objval.adds
            Me.lblCurrency.Text = objval.cur
            Dim InvAmt As Decimal = objDB.GetVal("SELECT IM_INVOICE_TOTAL FROM INVOICE_MSTR WHERE IM_INVOICE_NO='" & objval.Inv_no & "' AND IM_S_COY_ID='" & objval.Vcom_id & "'")
            Me.lblInvoiceAmount.Text = Format(InvAmt, "#,##0.00")
            'Me.lblRelatedDebitNote.Text = objDN.getRelatedDebitNotes(ViewState("b_coy_id"), Session("CompanyId"), Request(Trim("INVNO")))
            strAllDN = objDN.getRelatedDebitNotes(ViewState("b_coy_id"), Session("CompanyId"), Request(Trim("INVNO")))
            If strAllDN <> "" Then
                strDN = Split(strAllDN, ",")
                For i = 0 To strDN.Length - 1
                    If strTempDN = "" Then
                        strTempDN = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewDebitNote.aspx", "pageid=" & strPageId & "&DN_No=" & strDN(i) & "&BCoyID=" & ViewState("b_coy_id") & "&SCoyID=" & Session("CompanyId") & "')"" ><font color=#0000ff>" & strDN(i)) & "</font></A>"
                    Else
                        strTempDN &= ", " & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewDebitNote.aspx", "pageid=" & strPageId & "&DN_No=" & strDN(i) & "&BCoyID=" & ViewState("b_coy_id") & "&SCoyID=" & Session("CompanyId") & "')"" ><font color=#0000ff>" & strDN(i)) & "</font></A>"
                    End If
                Next
                Me.lblRelatedDebitNote.Text = strTempDN
            Else
                Me.lblRelatedDebitNote.Text = ""
            End If
            decDNAmt = objDN.getRelatedDebitNoteTotal(ViewState("b_coy_id"), Session("CompanyId"), Request(Trim("INVNO"))) 'Jules 2015-Feb-23 Agora Stage 2
            Me.lblRelatedDebitNoteAmt.Text = Format(decDNAmt, "##,##0.00")
            Me.lblNetAmt.Text = Format(InvAmt + decDNAmt, "##,##0.00")

            'If Session("urlreferer") = "RaiseDebitNote" Then
            lnkBack.NavigateUrl = dDispatcher.direct("DebitNote", "DebitCreditNoteList.aspx", "pageid=" & strPageId)
            'End If
            GenerateTab()

            cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & File1Int.ClientID & "');")
            Me.cmdView.Enabled = False
        End If
        body1.Attributes.Add("onLoad", "refreshDatagrid(); calculateGrandTotal(); calculateAllIndividualTotal(); ")

        displayAttachFile()
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim objinv As New Invoice
        Dim ds As DataSet
        Dim dvViewInv As DataView

        intRow = 0
        ds = objinv.GetInvDetailForCNDN(Request(Trim("INVNO")), Session("CompanyId"))
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
            dtg_DebitNoteDetail.DataSource = dvViewInv
            dtg_DebitNoteDetail.DataBind()
        Else
            dtg_DebitNoteDetail.DataBind()
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

    Private Sub dtg_DebitNoteDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_DebitNoteDetail.ItemDataBound
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
            'ddlGSTRate = e.Item.Cells(EnumDebitNote.icGSTRate).FindControl("ddlGSTRate")
            ddlTaxCode = e.Item.Cells(EnumDebitNote.icGSTTaxCode).FindControl("ddlTaxCode")
            txtQty = e.Item.Cells(EnumDebitNote.icQty).FindControl("txtQty")
            txtPrice = e.Item.Cells(EnumDebitNote.icQty).FindControl("txtPrice")
            txtAmount = e.Item.Cells(EnumDebitNote.icQty).FindControl("txtAmount")
            txtGSTValue = e.Item.Cells(EnumDebitNote.icQty).FindControl("txtGSTValue")
            txtRemark = e.Item.Cells(EnumDebitNote.icQty).FindControl("txtRemark")
            txtGST = e.Item.Cells(EnumDebitNote.icGSTAmount).FindControl("txtGST")

            Dim valQty As RegularExpressionValidator
            valQty = e.Item.FindControl("valQty")
            valQty.ControlToValidate = "txtQty"
            valQty.ValidationExpression = "^\d{1,10}(\.\d{1,2})?$$" '"^([1-9]\d{0,5}\.\d{0,2}|[1-9]\d{0,5}|[0]\d{0,5}\.[1-9]\d{0,1})$"
            valQty.ErrorMessage = CStr(intRow + 1) & ". " & "Qty is expecting numeric value."
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
                'objGlobal.FillGST(ddlGSTRate, True, Session("CompanyId"))
                'objGlobal.FillTaxCode(ddlTaxCode, , "S", , True)
                'Common.SelDdl("STD", ddlGSTRate)

                objGlobal.FillTaxCode(ddlTaxCode, Common.parseNull(dv("ID_GST_RATE")), "S", , True)
                If ddlTaxCode.SelectedValue = "N/A" Then
                    ddlTaxCode.Enabled = False
                Else
                    ddlTaxCode.Enabled = True
                End If
            Else
                'Issue 7480 - CH - 24 Mar 2015 (No.56)
                lstItem.Value = "N/A"
                lstItem.Text = "N/A"
                ddlTaxCode.Items.Clear()
                ddlTaxCode.Items.Insert(0, lstItem)
                ddlTaxCode.Enabled = False
            End If
            ddlTaxCode.Visible = True

            'txtQty.Text = Common.parseNull(dv("ID_RECEIVED_QTY"), 0)
            dblSubTotal = Common.parseNull(dv("ID_RECEIVED_QTY"), 0) * Common.parseNull(dv("ID_UNIT_COST"), 0) + Common.parseNull(dv("ID_GST_VALUE"), 0)
            'txtAmount.Text = Format(dblSubTotal, "#,##0.00")
            'txtPrice.Text = Format(dv("ID_UNIT_COST"), "#,##0.0000")
            'txtAmount.Text = Format(0, "#,##0.00")
            'txtPrice.Text = Format(0, "#,##0.0000")
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
                e.Item.Cells(EnumDebitNote.icGSTRate).Text = "N/A"
            Else
                e.Item.Cells(EnumDebitNote.icGSTRate).Text = dv("ID_GST_RATE")
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

            txtQty.Attributes.Add("onblur", "return calculateTotal('" &
                           txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" & txtAmount.ClientID & "', '" & txtGST.ClientID & "', '" & txtGSTValue.ClientID & "');")

            txtPrice.Attributes.Add("onfocus", "return focusControl('" &
                                     txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" & txtAmount.ClientID & "', '" & txtGST.ClientID & "', '" & txtGSTValue.ClientID & "');")

            txtPrice.Attributes.Add("onblur", "return calculateTotal('" &
                           txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" & txtAmount.ClientID & "', '" & txtGST.ClientID & "', '" & txtGSTValue.ClientID & "');")

            'ddlGSTRate.Attributes.Add("onchange", "return calculateTotal('" & _
            '               txtQty.ClientID & "', '" & txtPrice.ClientID & "', '" & txtAmount.ClientID & "', '" & ddlGSTRate.ClientID & "', '" & txtGSTValue.ClientID & "');")

            Dim strDNDetails As New ArrayList()
            If Session("strDNDetails") IsNot Nothing Then
                strDNDetails = Session("strDNDetails")
                If (CInt(e.Item.Cells(EnumDebitNote.icItemLine).Text) - 1) <= strDNDetails.Count - 1 Then
                    txtQty.Text = strDNDetails(CInt(e.Item.Cells(EnumDebitNote.icItemLine).Text) - 1)(0)
                    txtPrice.Text = strDNDetails(CInt(e.Item.Cells(EnumDebitNote.icItemLine).Text) - 1)(1)
                    'ddlGSTRate.SelectedValue = strDNDetails(CInt(e.Item.Cells(EnumDebitNote.icItemLine).Text) - 1)(2)
                    ddlTaxCode.SelectedValue = strDNDetails(CInt(e.Item.Cells(EnumDebitNote.icItemLine).Text) - 1)(2)
                    txtRemark.Text = strDNDetails(CInt(e.Item.Cells(EnumDebitNote.icItemLine).Text) - 1)(3)
                End If
            End If
            If ViewState("remarks") IsNot Nothing Then
                txtRemarks.Text = ViewState("remarks")
            End If

            intRow = intRow + 1
        End If
    End Sub

    Private Function SubmitDN()
        Dim objDB As New EAD.DBCom
        Dim objinv As New Invoice
        Dim objDN As New DebitNote
        Dim objval As New InvValue
        Dim DS As New DataSet
        Dim dtr As DataRow
        Dim dtrd As DataRow
        Dim DNmstr As New DataTable
        Dim DNDetails As New DataTable
        Dim strDN As String
        Dim strbcomid As String

        DNmstr.Columns.Add("b_com_id", Type.GetType("System.String"))
        DNmstr.Columns.Add("po_no", Type.GetType("System.String"))
        DNmstr.Columns.Add("dn_type", Type.GetType("System.String"))
        DNmstr.Columns.Add("inv_no", Type.GetType("System.String"))
        DNmstr.Columns.Add("currency", Type.GetType("System.String"))
        DNmstr.Columns.Add("exchange_rate", Type.GetType("System.Double"))
        DNmstr.Columns.Add("remark", Type.GetType("System.String"))
        DNmstr.Columns.Add("total", Type.GetType("System.Double"))

        dtr = DNmstr.NewRow()
        dtr("b_com_id") = ViewState("b_coy_id")
        dtr("po_no") = ViewState("po_no")
        dtr("dn_type") = IIf(ViewState("TaxInvoice") = "Y", "DN", "DA")
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
        dtr("total") = hidDNTotal.Value

        DNDetails.Columns.Add("inv_line_no", Type.GetType("System.Int32"))
        DNDetails.Columns.Add("qty", Type.GetType("System.Double"))
        DNDetails.Columns.Add("unit_price", Type.GetType("System.Double"))
        DNDetails.Columns.Add("gst_rate", Type.GetType("System.String"))
        DNDetails.Columns.Add("output_tax_code", Type.GetType("System.String"))
        DNDetails.Columns.Add("line_remark", Type.GetType("System.String"))
        DNmstr.Rows.Add(dtr)

        Dim dgItem As DataGridItem
        Dim ddlGSTRate As DropDownList
        Dim ddlTaxCode As DropDownList
        Dim txtQty As TextBox
        Dim txtPrice As TextBox
        Dim txtAmount As TextBox
        Dim txtGSTValue As TextBox
        Dim txtRemark As TextBox

        For Each dgItem In dtg_DebitNoteDetail.Items
            ddlGSTRate = dgItem.Cells(EnumDebitNote.icGSTRate).FindControl("ddlGSTRate")
            ddlTaxCode = dgItem.Cells(EnumDebitNote.icGSTTaxCode).FindControl("ddlTaxCode")
            txtQty = dgItem.Cells(EnumDebitNote.icQty).FindControl("txtQty")
            txtPrice = dgItem.Cells(EnumDebitNote.icUnitPrice).FindControl("txtPrice")
            txtAmount = dgItem.Cells(EnumDebitNote.icAmount).FindControl("txtAmount")
            txtGSTValue = dgItem.Cells(EnumDebitNote.icGSTAmount).FindControl("txtGSTValue")
            txtRemark = dgItem.Cells(EnumDebitNote.icRemarks).FindControl("txtRemark")

            dtrd = DNDetails.NewRow()
            dtrd("inv_line_no") = dgItem.Cells(EnumDebitNote.icItemLine).Text
            dtrd("qty") = txtQty.Text
            dtrd("unit_price") = txtPrice.Text
            dtrd("gst_rate") = dgItem.Cells(EnumDebitNote.icGSTRate).Text
            dtrd("output_tax_code") = ddlTaxCode.SelectedValue
            dtrd("line_remark") = txtRemark.Text

            DNDetails.Rows.Add(dtrd)
        Next

        DS.Tables.Add(DNmstr)
        DS.Tables.Add(DNDetails)

        Dim strMsgLine As String = ""

        If Not objDN.Update_DebitNoteMstr(DS, strDN, strbcomid, ViewState("isResident")) Then 'Jules 2018.10.19
            Common.NetMsgbox(Me, MsgTransDup, dDispatcher.direct("DebitNote", "DebitNoteList.aspx", "pageid=" & strPageId))
        Else
            If InStr(strDN, ",") > 0 Then
                strMsgLine = "Debit Note Number " & strDN & " has been generated."

            Else
                If strDN = "Generated" Then
                    strMsgLine = "Debit Note Number has been generated."
                Else
                    strMsgLine = "Debit Note Number " & strDN & " has been generated."
                End If

            End If

            Me.lblDebitNoteNum.Text = strDN
            ViewState("DnNo") = strDN
            Common.NetMsgbox(Me, strMsgLine)
            Me.cmdSubmit.Visible = False
            Me.cmdreset.Visible = False
            Me.cmdView.Visible = True
            Me.cmdView.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewDebitNote.aspx", "SCoyID=" & Session("CompanyId") & "&DN_NO=" & strDN & "&BCoyID=" & ViewState("b_coy_id")) & "')") 'Jules 2015-Feb-23 Agora Stage 2
            Me.cmdSubmit.Enabled = False
            Me.cmdUpload.Enabled = False
            Me.cmdView.Enabled = True
            lblMsg.Text = "" 'Jules 2015.02.16 - Agora Stage 2
        End If
    End Function

    Private Function validateDatagrid(ByRef strMsg As String) As Boolean
        Dim objDn As New DebitNote
        Dim blnQty As Boolean
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

        For Each dgItem In dtg_DebitNoteDetail.Items
            'ddlGSTRate = dgItem.Cells(EnumDebitNote.icGSTRate).FindControl("ddlGSTRate")
            ddlTaxCode = dgItem.Cells(EnumDebitNote.icGSTTaxCode).FindControl("ddlTaxCode")
            txtQty = dgItem.Cells(EnumDebitNote.icQty).FindControl("txtQty")
            txtPrice = dgItem.Cells(EnumDebitNote.icUnitPrice).FindControl("txtPrice")
            txtAmount = dgItem.Cells(EnumDebitNote.icAmount).FindControl("txtAmount")
            txtGSTValue = dgItem.Cells(EnumDebitNote.icGSTAmount).FindControl("txtGSTValue")
            txtRemark = dgItem.Cells(EnumDebitNote.icRemarks).FindControl("txtRemark")

            'Check if Qty & Unit Price both are zero, then without select Tax Code is allowed.
            If CDec(txtQty.Text) = 0 And CDec(txtPrice.Text) = 0 Then
            Else 'Else either one is zero, then error
                If CDec(txtQty.Text) = 0 Then
                    strMsg &= "<li>Line " & dgItem.Cells(EnumDebitNote.icItemLine).Text & ". Item Qty/ Unit Price value is required if either one is more than zero.<ul type='disc'></ul></li>"
                    validateDatagrid = False
                End If

                If CDec(txtPrice.Text) = 0 Then
                    strMsg &= "<li>Line " & dgItem.Cells(EnumDebitNote.icItemLine).Text & ". Item Qty/ Unit Price value is required if either one is more than zero.<ul type='disc'></ul></li>"
                    validateDatagrid = False
                End If
            End If

            If ViewState("TaxInvoice") = "Y" Then
                'If ddlGSTRate.SelectedIndex = 0 Then
                '    strMsg &= "<li>GST Rate " & objGlobal.GetErrorMessage("00001") & "<ul type='disc'></ul></li>"
                '    validateDatagrid = False
                'End If

                'If ddlTaxCode.SelectedIndex = 0 Then
                '    strMsg &= "<li>GST Tax Code " & objGlobal.GetErrorMessage("00001") & "<ul type='disc'></ul></li>"
                '    validateDatagrid = False
                'End If

                If txtQty.Text = 0 And txtPrice.Text = 0 Then
                    If ddlTaxCode.SelectedIndex > 0 Then
                        strMsg &= "<li>Line " & dgItem.Cells(EnumDebitNote.icItemLine).Text & ". Invalid SST tax code found.<ul type='disc'></ul></li>"
                        validateDatagrid = False
                    End If
                Else
                    If ddlTaxCode.SelectedIndex = 0 Then
                        strMsg &= "<li>Line " & dgItem.Cells(EnumDebitNote.icItemLine).Text & ". SST Tax Code " & objGlobal.GetErrorMessage("00001") & "<ul type='disc'></ul></li>"
                        validateDatagrid = False
                    End If
                End If
            End If

            'If validateDatagrid = True Then
            'CH - 23 Jul 2015 : Check outstanding Qty of each item 
            objDn.chkDnItemQty(ViewState("Inv_no"), dgItem.Cells(EnumDebitNote.icItemLine).Text, txtQty.Text, blnQty)
            If blnQty = False Then
                'strMsg &= "<li>Line " & dgItem.Cells(EnumDebitNote.icItemLine).Text & ". Total of all Debit Note Item Qty has exceed invoice item Qty.<ul type='disc'></ul></li>"
                strMsg &= "<li>Line " & dgItem.Cells(EnumDebitNote.icItemLine).Text & ". Debit Note Item Qty has exceed invoice item Qty.<ul type='disc'></ul></li>"
                validateDatagrid = False
            End If
            'End If

        Next
        strMsg &= "</ul>"
    End Function

    Private Function validateDNAmount() As Boolean
        Dim objCN As New CreditNote
        Dim decLatestAmt As Decimal
        validateDNAmount = True

        'CH - Issue 7480 (14 Apr 2015) : Cannot submit DN with zero amount (Grand Total)
        If CDec(Me.hidDNTotal.Value) = 0 Then
            Common.NetMsgbox(Me, "Debit Note Grand Total must not be zero amount.")
            validateDNAmount = False
        End If

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
                'objFile.FileUpload(File1Int, EnumUploadType.DocAttachment, "DN", EnumUploadFrom.FrontOff, Session.SessionID, , , , , , "I")
                'Issue 7480 - CH - 23 Mar 2015 (No.35)
                objFile.FileUpload(File1Int, EnumUploadType.DocAttachmentTemp, "DN", EnumUploadFrom.FrontOff, Session.SessionID, , , , , , "I")
            ElseIf File1Int.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            objFile = Nothing
        End If
        displayAttachFile()

        Dim strDNDetails As New ArrayList()
        ViewState("remarks") = txtRemarks.Text

        Dim ddlGSTRate, ddlTaxCode As DropDownList
        Dim txtQty, txtPrice, txtRemark As TextBox
        Dim dgItem As DataGridItem
        For Each dgItem In dtg_DebitNoteDetail.Items
            'ddlGSTRate = dgItem.Cells(EnumDebitNote.icGSTRate).FindControl("ddlGSTRate")
            ddlTaxCode = dgItem.Cells(EnumDebitNote.icGSTTaxCode).FindControl("ddlTaxCode")
            txtQty = dgItem.Cells(EnumDebitNote.icQty).FindControl("txtQty")
            txtPrice = dgItem.Cells(EnumDebitNote.icUnitPrice).FindControl("txtPrice")
            txtRemark = dgItem.Cells(EnumDebitNote.icRemarks).FindControl("txtRemark")

            'strDNDetails.Add(New String() {txtQty.Text, txtPrice.Text, ddlGSTRate.SelectedValue, ddlTaxCode.SelectedValue, txtRemark.Text})
            strDNDetails.Add(New String() {txtQty.Text, txtPrice.Text, ddlTaxCode.SelectedValue, txtRemark.Text})
        Next

        Session("strDNDetails") = strDNDetails

        Bindgrid()
    End Sub

    Private Sub displayAttachFile()       
        Dim objDN As New DebitNote
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL As String

        'Issue 7480 - CH - 24 Mar 2015 (No.35)
        If ViewState("DnNo") <> "" Then '
            dsAttach = objDN.getDNAttachment(ViewState("DnNo"), Session("CompanyId"))
        Else
            dsAttach = objDN.getDNTempAttach(Session.SessionID, "I")
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
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "DN", EnumUploadFrom.FrontOff)
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
                If ViewState("DnNo") <> "" Then
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
        objDN = Nothing
    End Sub

    Private Sub deleteAttach(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objDN As New DebitNote

        'Issue 7480 - CH - 23 Mar 2015 (No.35)
        objDN.delDNAttachTemp(CType(sender, ImageButton).ID)

        displayAttachFile()
        objDN = Nothing
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim strMsg As String = ""       
        If validateDatagrid(strMsg) Then
            If validateDNAmount() Then
                SubmitDN()
            End If
        Else
            If strMsg <> "" Then
                lblMsg.Text = strMsg
            Else
                lblMsg.Text = ""
            End If
        End If
    End Sub
End Class