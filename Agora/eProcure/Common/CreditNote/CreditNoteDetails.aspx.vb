Imports AgoraLegacy
Imports eProcure.Component
Imports System.drawing
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class CreditNoteDetails
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim blnPrintRemark As Boolean
    Dim dblInvoiceAmount, prevAppType As String
    Dim aryPTaxCode As New ArrayList()
    Dim objGlobal As New AppGlobals
    Dim count As Integer = 0
    Dim strInvAppr As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dtgCnDetail As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblCnNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblCnDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblBillTo As System.Web.UI.WebControls.Label
    Protected WithEvents lblVenRemarks As System.Web.UI.WebControls.Label
    Protected WithEvents lblInvNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblCurr As System.Web.UI.WebControls.Label
    Protected WithEvents lblInvAmt As System.Web.UI.WebControls.Label
    Protected WithEvents lblRelatedCN As System.Web.UI.WebControls.Label
    Protected WithEvents lblTotalRelatedCNAmt As System.Web.UI.WebControls.Label
    Protected WithEvents cmdPreviewCN As System.Web.UI.WebControls.Button
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtRemark As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents tblApproval As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tr1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trMessage As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAck As System.Web.UI.WebControls.Button
    Protected WithEvents back As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents tr_curr As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trPreview As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents hidPM As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents pnlAttach As System.Web.UI.WebControls.Panel
    Dim strFrm, strName As String

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Enum EnumCn
        icLineItem
        icDesc
        icUOM
        icQty
        icUPrice
        icTotal
        icGstRate
        icGstAmt
        icTaxCode
        icRemarks
        icHidGstRate
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Request.QueryString("Frm") <> "Dashboard" Then
            CheckButtonAccess(True)
        End If
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        blnCheckBox = False
        blnPaging = False
        blnSorting = False

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCnDetail)

        Dim objCompany As New Companies
        Dim objTrac As New Tracking

        ViewState("line") = 0

        strFrm = Me.Request.QueryString("Frm")
        strName = Me.Request.QueryString("Name")

        If Not IsPostBack Then
            Dim objval As New CnValue
            Dim objCn As New CreditNote

            objval.Cn_No = Request(Trim("CNNO"))
            objval.V_Com_Id = Request(Trim("vcomid"))
            objCn.getCnMstr(objval)

            ViewState("Cn_Index") = objval.Cn_Index
            ViewState("Ven_Id") = objval.V_Com_Id
            'As discussion, remove shipping handling - CH - 2 Mar 2015
            'ViewState("ShipAmt") = objval.Ship_Amt
            ViewState("status") = objval.Status

            Bindgrid()
            If intPageRecordCnt > 0 Then
                AddRowtotal()
            End If

            Me.lblCnNo.Text = objval.Cn_No
            Me.lblCnDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objval.Cn_Date)
            Me.lblBillTo.Text = objval.Adds
            Me.lblVenRemarks.Text = objval.Remark
            Me.lblInvNo.Text = objval.Inv_No
            Me.lblDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objval.Inv_Date)
            Me.lblCurr.Text = objval.Cur
            Me.lblInvAmt.Text = Format(objval.Inv_Amt, "#,###,##0.00")
            Me.lblRelatedCN.Text = objval.Related_Doc
            Me.lblTotalRelatedCNAmt.Text = Format(objval.Related_Doc_Amt, "#,###,##0.00")
            Me.txtRemark.Text = objval.Ack_Remark
            dblInvoiceAmount = objval.Inv_Amt
            GenerateTab()

        End If

        displayAttachFile()

        If ViewState("status") = "1" Then
            txtRemark.Enabled = True
            'tblApproval.Style.Item("display") = "inline"
            Tr1.Style.Item("display") = "inline"
        Else
            txtRemark.Enabled = False
            'tblApproval.Style.Item("display") = "none"
            Tr1.Style.Item("display") = "none"
        End If

        cmdPreviewCN.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewCreditNote.aspx", "SCoyID=" & Request(Trim("vcomid")) & "&CN_No=" & Request(Trim("CNNO"))) & "&BCoyID=" & Session("CompanyId") & "')")

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim objCn As New CreditNote
        Dim ds As DataSet
        Dim dvViewDn As DataView

        ds = objCn.getCnDetail(Request(Trim("CNNO")), Request(Trim("vcomid")))
        dvViewDn = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewDn.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewDn.Sort += " DESC"
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            dtgCnDetail.DataSource = dvViewDn
            dtgCnDetail.DataBind()

        Else
            dtgCnDetail.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

    End Function

    Private Sub displayAttachFile()
        Dim objCn As New CreditNote
        Dim objFile As New FileManagement
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        'dsAttach = objCn.getCNTempAttach(Request(Trim("CNNO")), "I", Request(Trim("vcomid"))
        'Issue 7480 - CH - 23 Mar 2015 (No.35))
        dsAttach = objCn.getCNAttachment(Request(Trim("CNNO")), Request(Trim("vcomid")))

        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "CN", EnumUploadFrom.FrontOff)

                Dim lblBr As New Label
                Dim lblFile As New Label

                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"
                pnlAttach.Controls.Add(lblFile)
                pnlAttach.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFile)
        End If

    End Sub

    Sub AddRowtotal() 'add total row 
        Dim intL As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row1 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row3 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row4 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row5 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        For intL = 0 To Me.dtgCnDetail.Columns.Count - 3
            addCell(row)
        Next

        row.Cells(EnumCn.icUPrice - 1).ColumnSpan = 2
        row.Cells(EnumCn.icUPrice - 1).Text = "Sub Total"
        row.Cells(EnumCn.icUPrice - 1).HorizontalAlign = HorizontalAlign.Right
        row.Cells(EnumCn.icUPrice - 1).Font.Bold = True

        row.Cells(EnumCn.icTotal - 1).Text = Format(ViewState("total"), "#,##0.00")
        row.Cells(EnumCn.icTotal - 1).HorizontalAlign = HorizontalAlign.Right
        row.Cells(EnumCn.icTotal - 1).Font.Bold = True

        row.BackColor = Color.FromName("#f4f4f4")
        Me.dtgCnDetail.Controls(0).Controls.Add(row)

        For intL = 0 To Me.dtgCnDetail.Columns.Count - 3
            addCell(row1)
        Next

        row1.Cells(EnumCn.icUPrice - 1).ColumnSpan = 2
        row1.Cells(EnumCn.icUPrice - 1).Text = "SST Amount"
        row1.Cells(EnumCn.icUPrice - 1).HorizontalAlign = HorizontalAlign.Right
        row1.Cells(EnumCn.icUPrice - 1).Font.Bold = True

        row1.Cells(EnumCn.icTotal - 1).Text = Format(ViewState("gst"), "#,##0.00")
        row1.Cells(EnumCn.icTotal - 1).HorizontalAlign = HorizontalAlign.Right
        row1.Cells(EnumCn.icTotal - 1).Font.Bold = True

        row1.BackColor = Color.FromName("#f4f4f4")
        Me.dtgCnDetail.Controls(0).Controls.Add(row1)

        'As discussion, remove shipping handling - CH - 2 Mar 2015
        'For intL = 0 To Me.dtgCnDetail.Columns.Count - 3
        '    addCell(row2)
        'Next

        'row2.Cells(EnumCn.icUPrice - 1).ColumnSpan = 2

        'row2.Cells(EnumCn.icTotal - 1).Text = Format(CDbl(ViewState("total") + ViewState("gst")), "#,##0.00")
        'row2.Cells(EnumCn.icTotal - 1).HorizontalAlign = HorizontalAlign.Right
        'row2.Cells(EnumCn.icTotal - 1).Font.Bold = True

        'row2.BackColor = Color.FromName("#f4f4f4")
        'Me.dtgCnDetail.Controls(0).Controls.Add(row2)

        'For intL = 0 To Me.dtgCnDetail.Columns.Count - 3
        '    addCell(row3)
        'Next

        'row3.Cells(EnumCn.icUPrice - 1).ColumnSpan = 2
        'row3.Cells(EnumCn.icUPrice - 1).Text = "Shipping & Handling"
        'row3.Cells(EnumCn.icUPrice - 1).HorizontalAlign = HorizontalAlign.Right
        'row3.Cells(EnumCn.icUPrice - 1).Font.Bold = True

        'row3.Cells(EnumCn.icTotal - 1).Text = Format(CDbl(ViewState("ShipAmt")), "#,##0.00")
        'row3.Cells(EnumCn.icTotal - 1).HorizontalAlign = HorizontalAlign.Right
        'row3.Cells(EnumCn.icTotal - 1).Font.Bold = True

        'row3.BackColor = Color.FromName("#f4f4f4")
        'Me.dtgCnDetail.Controls(0).Controls.Add(row3)

        For intL = 0 To Me.dtgCnDetail.Columns.Count - 3
            addCell(row5)
        Next

        row5.Cells(EnumCn.icUPrice - 1).ColumnSpan = 2
        row5.Cells(EnumCn.icUPrice - 1).Text = "Grand Total"
        row5.Cells(EnumCn.icUPrice - 1).HorizontalAlign = HorizontalAlign.Right
        row5.Cells(EnumCn.icUPrice - 1).Font.Bold = True

        'As discussion, remove shipping handling - CH - 2 Mar 2015
        'row5.Cells(EnumCn.icTotal - 1).Text = Format((((ViewState("total") + ViewState("gst") + ViewState("ShipAmt")) / 100) * CInt(ViewState("Tax"))) + (ViewState("total") + ViewState("gst") + ViewState("ShipAmt")), "#,##0.00")
        row5.Cells(EnumCn.icTotal - 1).Text = Format((((ViewState("total") + ViewState("gst")) / 100) * CInt(ViewState("Tax"))) + (ViewState("total") + ViewState("gst")), "#,##0.00")
        row5.Cells(EnumCn.icTotal - 1).HorizontalAlign = HorizontalAlign.Right
        row5.Cells(EnumCn.icTotal - 1).Font.Bold = True

        row5.BackColor = Color.FromName("#f4f4f4")
        Me.dtgCnDetail.Controls(0).Controls.Add(row5)

    End Sub

    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub

    Private Sub dtgDnDetail_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCnDetail.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            Dim cell As TableCell
            Dim lbl As New Label

            If ViewState("status") = "1" Then
                cell = e.Item.Cells(EnumCn.icTaxCode)
                lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("Tracking", "InvTaxCode.aspx", "pageid=" & strPageId & "&id=ddlTaxCode") & "', '', 'scrollbars=yes,resizable=yes,width=500,height=300,status=no,menubar=no');"">" & e.Item.Cells(EnumCn.icTaxCode).Text & "</a>"
                cell.Controls.Add(lbl)
            End If
        End If

        dtgCnDetail.AllowSorting = False
        Grid_ItemCreated(dtgCnDetail, e)
    End Sub

    Private Sub dtgCnDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCnDetail.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dblSubTotal As Double
            Dim j As Integer
            Dim lstItem As New ListItem

            Dim ddlTaxCode As DropDownList
            Dim lblTaxCode As Label
            ddlTaxCode = e.Item.Cells(EnumCn.icTaxCode).FindControl("ddlTaxCode")
            lblTaxCode = e.Item.Cells(EnumCn.icTaxCode).FindControl("lblTaxCode")

            If ViewState("status") = "1" Then
                'Issue 7480 - CH - 24 Mar 2015 (No.56)
                If Common.parseNull(dv("CND_GST_RATE")) = "" Or Common.parseNull(dv("CND_GST_RATE")) = "N/A" Then
                    ddlTaxCode.Items.Clear()
                    'Issue 7480 - CH - 24 Mar 2015 (No.56)
                    lstItem.Value = "NS"
                    lstItem.Text = "NS"
                    ddlTaxCode.Items.Insert(0, lstItem)
                    ddlTaxCode.Enabled = False
                Else
                    'Jules 2018.10.08 - SST - default to corresponding tax code.
                    'objGlobal.FillTaxCode(ddlTaxCode, , "P", )
                    objGlobal.FillTaxCode(ddlTaxCode, Common.parseNull(dv("CND_GST_RATE")), "P",,, True)
                    'End modification.

                    If aryPTaxCode.Count > 0 Then
                        For j = 0 To aryPTaxCode.Count - 1
                            If Common.parseNull(dv("CND_CN_LINE")) = aryPTaxCode(j)(0) Then
                                ddlTaxCode.SelectedValue = aryPTaxCode(j)(1)
                            End If
                        Next
                    Else
                        'Jules 2018.10.08 - SST - Added checking
                        If Common.parseNull(dv("CND_GST_INPUT_TAX_CODE")) <> "" Then
                            ddlTaxCode.SelectedValue = dv("CND_GST_INPUT_TAX_CODE")
                        End If
                        'End modification.
                    End If

                    If ddlTaxCode.SelectedValue = "N/A" Then
                        ddlTaxCode.Enabled = False
                    Else
                        ddlTaxCode.Enabled = True
                    End If
                End If

                ddlTaxCode.Visible = True
                lblTaxCode.Visible = False
            Else
                ddlTaxCode.Visible = False
                lblTaxCode.Visible = True

                lblTaxCode.Text = Common.parseNull(dv("CND_GST_INPUT_TAX_CODE"))
            End If

            dblSubTotal = Common.parseNull(dv("CND_QTY"), 0) * Common.parseNull(dv("CND_UNIT_COST"), 0)

            'Jules 2018.10.08
            If dblSubTotal = 0 Then
                ddlTaxCode.SelectedIndex = 0
                ddlTaxCode.Enabled = False
            End If
            'End modification.

            e.Item.Cells(EnumCn.icTotal).Text = Format(dblSubTotal, "#,##0.00")
            e.Item.Cells(EnumCn.icUPrice).Text = Format(dv("CND_UNIT_COST"), "#,##0.0000")
            ViewState("total") = ViewState("total") + CDbl(Format(dblSubTotal, "##0.00"))
            'Issue 7480 - CH - 24 Mar 2015 (No.56)
            If Common.parseNull(dv("CND_GST_RATE")) = "NS" Or Common.parseNull(dv("CND_GST_RATE")) = "" Or Common.parseNull(dv("CND_GST_RATE")) = "N/A" Then
                e.Item.Cells(EnumCn.icGstAmt).Text = "0.00"
                ViewState("gst") = ViewState("gst") + 0
            Else
                'Issue 7480 - CH - 23 Mar 2015 (No.38)
                e.Item.Cells(EnumCn.icGstAmt).Text = Format(dblSubTotal / 100 * Common.parseNull(dv("TAX")), "#,##0.00")
                ViewState("gst") = ViewState("gst") + CDbl(Format((dblSubTotal / 100 * Common.parseNull(dv("TAX"))), "###0.00"))
            End If
        End If
    End Sub

    Private Sub cmdAck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAck.Click
        Dim objCn As New CreditNote
        Dim strMsg As String

        lblMsg.Text = ""
        strMsg = ""
        If chkPurchaseTaxCode(strMsg) = True Then
            SavePurchaseTaxCode()
            strMsg = objCn.ackCreditNote(ViewState("Cn_Index"), lblCnNo.Text, txtRemark.Text, aryPTaxCode)

            Dim strurl As String = ""
            If strMsg <> "" Then
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Else
                If strFrm = "Dashboard" Then
                     strurl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & "Dashboard" & "&pageid=" & strPageId)
                ElseIf strFrm = "CreditNoteTrackingList" Then
                    strurl = dDispatcher.direct("CreditNote", "CreditNoteTrackingList.aspx", "Frm=" & "CreditNoteTrackingList" & "&pageid=" & strPageId)
                End If
                cmdSave.Visible = False

                Common.NetMsgbox(Me, "Credit Note Number " & Request(Trim("CNNO")) & " has been acknowledged.", strurl)
            End If
        Else
            lblMsg.Text = strMsg
        End If

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim strMsg As String

        lblMsg.Text = ""
        strMsg = ""

        If chkPurchaseTaxCode(strMsg) = True Then
            Dim objCn As New CreditNote
            SavePurchaseTaxCode()
            objCn.updateCnInfo(ViewState("Cn_Index"), txtRemark.Text, aryPTaxCode)
            resetDataGrid()
            Common.NetMsgbox(Me, MsgRecordSave, Request.Url.AbsoluteUri, MsgBoxStyle.Information)
        Else
            lblMsg.Text = strMsg
        End If
        

    End Sub

    Public Sub cmd_next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles back.ServerClick

        Dim strurl As String = Session("strurl")
        If strFrm = "Dashboard" Then
            If strName = "Buyer" Then
                strurl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & "Dashboard" & "&pageid=" & strPageId)

            ElseIf strName = "FMnAO" Then
                strurl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & "Dashboard" & "&pageid=" & strPageId)
            End If

        ElseIf strFrm = "CreditNoteTrackingList" Then
            strurl = dDispatcher.direct("CreditNote", "CreditNoteTrackingList.aspx", "Frm=" & "CreditNoteTrackingList" & "&pageid=" & strPageId)

        ElseIf strFrm = "CreditNoteAckTrackingList" Then
            strurl = dDispatcher.direct("CreditNote", "CreditNoteAckTrackingList.aspx", "Frm=" & "CreditNoteAckTrackingList" & "&pageid=" & strPageId)

        ElseIf strFrm = "InvoiceDetails" Then
            strurl = dDispatcher.direct("Tracking", "InvoiceDetails.aspx", "Frm=" & Request.QueryString("callFrm") & "&Name=" & Request.QueryString("Name") & "&vcomid=" & Request.QueryString("vcomid") & "&doc=" & Request.QueryString("doc") & "&INVNO=" & Request.QueryString("INVNO") & "&folder=" & Request.QueryString("folder") & "&pageid=" & strPageId)
        End If

        Session("strurl") = ""
        Session("status_dis") = ""
        Response.Redirect(strurl)

    End Sub

    Sub SavePurchaseTaxCode()
        Dim dgitem As DataGridItem
        Dim ddlTaxCode As DropDownList
        For Each dgitem In dtgCnDetail.Items
            ddlTaxCode = dgitem.FindControl("ddlTaxCode")
            aryPTaxCode.Add(New String() {dgitem.Cells(EnumCn.icLineItem).Text, ddlTaxCode.SelectedValue})
        Next

    End Sub

    Public Function chkPurchaseTaxCode(ByRef strMsg As String) As Boolean
        chkPurchaseTaxCode = True
        Dim dgitem As DataGridItem
        Dim ddlTaxCode As DropDownList
        Dim objGst As New GST

        strMsg = "<ul type='disc'>"

        For Each dgitem In dtgCnDetail.Items
            ddlTaxCode = dgitem.FindControl("ddlTaxCode")

            If CDec(dgitem.Cells(EnumCn.icTotal).Text) > 0 Then
                If dgitem.Cells(EnumCn.icHidGstRate).Text = "N/A" Or dgitem.Cells(EnumCn.icHidGstRate).Text = "" Then
                Else

                    If objGst.chkValidTaxCode(dgitem.Cells(EnumCn.icHidGstRate).Text, ddlTaxCode.SelectedValue, "P") = False Then
                        strMsg &= "<li>" & dgitem.Cells(EnumCn.icLineItem).Text & ". Invalid Purchase Tax Code.<ul type='disc'></ul></li>"
                        chkPurchaseTaxCode = False
                    End If
                End If
            Else
                If dgitem.Cells(EnumCn.icHidGstRate).Text = "N/A" Or dgitem.Cells(EnumCn.icHidGstRate).Text = "" Then
                Else
                    If ddlTaxCode.SelectedValue <> "" Then
                        strMsg &= "<li>" & dgitem.Cells(EnumCn.icLineItem).Text & ". Invalid Purchase Tax Code.<ul type='disc'></ul></li>"
                        chkPurchaseTaxCode = False
                    End If
                End If
            End If
        Next

        strMsg &= "</ul>"

    End Function


    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        If strFrm = "Dashboard" And Request.QueryString("dpage") = "AllDashBoard" Then
            Session("w_CnTrackingList_tabs") = "<div class=""t_entity""><ul>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("CreditNote", "CreditNoteTrackingList.aspx", "pageid=" & strPageId) & """><span>New Credit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("CreditNote", "CreditNoteAckTrackingList.aspx", "pageid=" & strPageId) & """><span>Acknowledged Credit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "</ul><div></div></div>"

        ElseIf strFrm = "InvoiceDetails" Then
            Session("w_CnTrackingList_tabs") = Session("w_InvTracking_tabs")

        ElseIf strFrm = "CreditTrackingList" Or strFrm = "Dashboard" Then
            Session("w_CnTrackingList_tabs") = "<div class=""t_entity""><ul>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("CreditNote", "CreditNoteTrackingList.aspx", "pageid=" & strPageId) & """><span>New Credit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("CreditNote", "CreditNoteAckTrackingList.aspx", "pageid=" & strPageId) & """><span>Acknowledged Credit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "</ul><div></div></div>"

        ElseIf strFrm = "CreditNoteAckTrackingList" Then
            Session("w_CnTrackingList_tabs") = "<div class=""t_entity""><ul>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("CreditNote", "CreditNoteTrackingList.aspx", "pageid=" & strPageId) & """><span>New Credit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("CreditNote", "CreditNoteAckTrackingList.aspx", "pageid=" & strPageId) & """><span>Acknowledged Credit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "</ul><div></div></div>"
        
        End If
    End Sub

    Sub resetDataGrid()
        ViewState("total") = 0
        ViewState("gst") = 0
        Bindgrid()
        If intPageRecordCnt > 0 Then
            AddRowtotal()
        End If
    End Sub

    Private Sub cmdPreviewCN_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPreviewCN.Click
        resetDataGrid()
    End Sub
End Class
