Imports AgoraLegacy
Imports eProcure.Component
Imports System.drawing
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class DebitNoteDetails
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
    Protected WithEvents dtgDnDetail As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtgAppFlow As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents cmdAppDn As System.Web.UI.WebControls.Button
    Protected WithEvents lblDnNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblDnDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblBillTo As System.Web.UI.WebControls.Label
    Protected WithEvents lblVenRemarks As System.Web.UI.WebControls.Label
    Protected WithEvents lblInvNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblCurr As System.Web.UI.WebControls.Label
    Protected WithEvents lblInvAmt As System.Web.UI.WebControls.Label
    Protected WithEvents lblRelatedDN As System.Web.UI.WebControls.Label
    Protected WithEvents lblTotalRelatedDNAmt As System.Web.UI.WebControls.Label
    Protected WithEvents cmdPreviewDN As System.Web.UI.WebControls.Button
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtRemark As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents tblApproval As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tr1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trMessage As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdMark As System.Web.UI.WebControls.Button
    Protected WithEvents back As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents tr_curr As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trPreview As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents hidPM As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cboPayMethod As System.Web.UI.WebControls.DropDownList
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

    Public Enum EnumDn
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
        SetGridProperty(dtgDnDetail)
        SetGridProperty(dtgAppFlow)

        Dim objCompany As New Companies
        Dim objTrac As New Tracking

        ViewState("line") = 0

        strFrm = Me.Request.QueryString("Frm")
        strName = Me.Request.QueryString("Name")

        If Not IsPostBack Then
            Dim objval As New DnValue
            Dim objDn As New DebitNote

            objval.Dn_No = Request(Trim("DNNO"))
            objval.V_Com_Id = Request(Trim("vcomid"))
            objDn.getDnMstr(objval)

            ViewState("Dn_Index") = objval.Dn_Index
            ViewState("Ven_Id") = objval.V_Com_Id
            'As discussion, remove shipping handling - CH - 2 Mar 2015
            'ViewState("ShipAmt") = objval.Ship_Amt

            Bindgrid()
            If intPageRecordCnt > 0 Then
                AddRowtotal()
            End If

            Me.lblDnNo.Text = objval.Dn_No
            Me.lblDnDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objval.Dn_Date)
            Me.lblBillTo.Text = objval.Adds
            Me.lblVenRemarks.Text = objval.Remark
            Me.lblInvNo.Text = objval.Inv_No
            Me.lblDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objval.Inv_Date)
            Me.lblCurr.Text = objval.Cur
            Me.lblInvAmt.Text = Format(objval.Inv_Amt, "#,###,##0.00")
            Me.lblRelatedDN.Text = objval.Related_Doc
            Me.lblTotalRelatedDNAmt.Text = Format(objval.Related_Doc_Amt, "#,###,##0.00")
            dblInvoiceAmount = objval.Inv_Amt
            renderDNApprFlow()
            GenerateTab()

        End If

        displayAttachFile()

        If ViewState("CurrentAppSeq") = ViewState("HighestAppr") Then
            ViewState("ISHighestLevel") = True
        Else
            ViewState("ISHighestLevel") = False
        End If

        strInvAppr = objCompany.GetInvApprMode(Session("CompanyId")) 'Jules 2018.10.19

        If Request.QueryString("cmd") = "verify" Or Request.QueryString("cmd") = "approve" Or Request.QueryString("cmd") = "pay" Then
            tblApproval.Style.Item("display") = "inline"

            If Request.QueryString("action") = "N" Then
                cmdMark.Visible = False
            ElseIf Request.QueryString("action") = "payment" Then
                cmdAppDn.Visible = False
            Else
                cmdMark.Visible = False
            End If
        Else
            tblApproval.Style.Item("display") = "none"
        End If

        Select Case Request.QueryString("role")
            Case 2 'FO only
                cmdAppDn.Text = "Verify Debit Note"
            Case 3 'FM only
                cmdAppDn.Text = "Approve Debit Note"
            Case 4 'Both
                If Request.QueryString("cmd") = "verify" Then
                    cmdAppDn.Text = "Verify Debit Note"
                ElseIf Request.QueryString("cmd") = "approve" Then
                    cmdAppDn.Text = "Approve Debit Note"
                End If
        End Select

        cmdPreviewDN.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewDebitNote.aspx", "SCoyID=" & Request(Trim("vcomid")) & "&DN_No=" & Request(Trim("DNNO"))) & "&BCoyID=" & Session("CompanyId") & "')")

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim objDn As New DebitNote
        Dim ds As DataSet
        Dim dvViewDn As DataView

        ds = objDn.getDnDetail(Request(Trim("DNNO")), Request(Trim("vcomid")))
        dvViewDn = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewDn.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewDn.Sort += " DESC"
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            dtgDnDetail.DataSource = dvViewDn
            dtgDnDetail.DataBind()

        Else
            dtgDnDetail.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

    End Function

    Private Sub displayAttachFile()
        Dim objDn As New DebitNote
        Dim objFile As New FileManagement
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        'dsAttach = objDn.getDNTempAttach(Request(Trim("DNNO")), "I", Request(Trim("vcomid")))
        'Issue 7480 - CH - 23 Mar 2015 (No.35)
        dsAttach = objDn.getDNAttachment(Request(Trim("DNNO")), Request(Trim("vcomid")))

        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "DN", EnumUploadFrom.FrontOff)

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

    Private Sub renderDNApprFlow()
        Dim objDn As New DebitNote
        Dim ds As DataSet

        ds = objDn.getDnApprFlow(Request("DNNO"), Request("vcomid"))

        dtgAppFlow.DataSource = ds.Tables(0).DefaultView
        dtgAppFlow.DataBind()

        objDn = Nothing
    End Sub

    Sub AddRowtotal() 'add total row 
        Dim intL As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row1 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row2 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row3 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row4 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim row5 As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)

        For intL = 0 To Me.dtgDnDetail.Columns.Count - 3
            addCell(row)
        Next

        row.Cells(EnumDn.icUPrice - 1).ColumnSpan = 2
        row.Cells(EnumDn.icUPrice - 1).Text = "Sub Total"
        row.Cells(EnumDn.icUPrice - 1).HorizontalAlign = HorizontalAlign.Right
        row.Cells(EnumDn.icUPrice - 1).Font.Bold = True

        row.Cells(EnumDn.icTotal - 1).Text = Format(ViewState("total"), "#,##0.00")
        row.Cells(EnumDn.icTotal - 1).HorizontalAlign = HorizontalAlign.Right
        row.Cells(EnumDn.icTotal - 1).Font.Bold = True

        row.BackColor = Color.FromName("#f4f4f4")
        Me.dtgDnDetail.Controls(0).Controls.Add(row)

        For intL = 0 To Me.dtgDnDetail.Columns.Count - 3
            addCell(row1)
        Next

        row1.Cells(EnumDn.icUPrice - 1).ColumnSpan = 2
        row1.Cells(EnumDn.icUPrice - 1).Text = "SST Amount"
        row1.Cells(EnumDn.icUPrice - 1).HorizontalAlign = HorizontalAlign.Right
        row1.Cells(EnumDn.icUPrice - 1).Font.Bold = True

        row1.Cells(EnumDn.icTotal - 1).Text = Format(ViewState("gst"), "#,##0.00")
        row1.Cells(EnumDn.icTotal - 1).HorizontalAlign = HorizontalAlign.Right
        row1.Cells(EnumDn.icTotal - 1).Font.Bold = True

        row1.BackColor = Color.FromName("#f4f4f4")
        Me.dtgDnDetail.Controls(0).Controls.Add(row1)

        'As discussion, remove shipping handling - CH - 2 Mar 2015
        'For intL = 0 To Me.dtgDnDetail.Columns.Count - 3
        '    addCell(row2)
        'Next

        'row2.Cells(EnumDn.icUPrice - 1).ColumnSpan = 2

        'row2.Cells(EnumDn.icTotal - 1).Text = Format(CDbl(ViewState("total") + ViewState("gst")), "#,##0.00")
        'row2.Cells(EnumDn.icTotal - 1).HorizontalAlign = HorizontalAlign.Right
        'row2.Cells(EnumDn.icTotal - 1).Font.Bold = True

        'row2.BackColor = Color.FromName("#f4f4f4")
        'Me.dtgDnDetail.Controls(0).Controls.Add(row2)

        'For intL = 0 To Me.dtgDnDetail.Columns.Count - 3
        '    addCell(row3)
        'Next

        'row3.Cells(EnumDn.icUPrice - 1).ColumnSpan = 2
        'row3.Cells(EnumDn.icUPrice - 1).Text = "Shipping & Handling"
        'row3.Cells(EnumDn.icUPrice - 1).HorizontalAlign = HorizontalAlign.Right
        'row3.Cells(EnumDn.icUPrice - 1).Font.Bold = True

        'row3.Cells(EnumDn.icTotal - 1).Text = Format(CDbl(ViewState("ShipAmt")), "#,##0.00")
        'row3.Cells(EnumDn.icTotal - 1).HorizontalAlign = HorizontalAlign.Right
        'row3.Cells(EnumDn.icTotal - 1).Font.Bold = True

        'row3.BackColor = Color.FromName("#f4f4f4")
        'Me.dtgDnDetail.Controls(0).Controls.Add(row3)

        For intL = 0 To Me.dtgDnDetail.Columns.Count - 3
            addCell(row5)
        Next

        row5.Cells(EnumDn.icUPrice - 1).ColumnSpan = 2
        row5.Cells(EnumDn.icUPrice - 1).Text = "Grand Total"
        row5.Cells(EnumDn.icUPrice - 1).HorizontalAlign = HorizontalAlign.Right
        row5.Cells(EnumDn.icUPrice - 1).Font.Bold = True
        'As discussion, remove shipping handling - CH - 2 Mar 2015
        'row5.Cells(EnumDn.icTotal - 1).Text = Format((((ViewState("total") + ViewState("gst") + ViewState("ShipAmt")) / 100) * CInt(ViewState("Tax"))) + (ViewState("total") + ViewState("gst") + ViewState("ShipAmt")), "#,##0.00")
        row5.Cells(EnumDn.icTotal - 1).Text = Format((((ViewState("total") + ViewState("gst")) / 100) * CInt(ViewState("Tax"))) + (ViewState("total") + ViewState("gst")), "#,##0.00")
        row5.Cells(EnumDn.icTotal - 1).HorizontalAlign = HorizontalAlign.Right
        row5.Cells(EnumDn.icTotal - 1).Font.Bold = True

        row5.BackColor = Color.FromName("#f4f4f4")
        Me.dtgDnDetail.Controls(0).Controls.Add(row5)

    End Sub

    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub

    Private Sub dtgDnDetail_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDnDetail.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            Dim cell As TableCell
            Dim lbl As New Label

            If (Request.QueryString("cmd") = "verify" Or Request.QueryString("cmd") = "approve") Then
                cell = e.Item.Cells(EnumDn.icTaxCode)
                lbl.Text = "<A href='javascript:;' onclick=""window.open('" & dDispatcher.direct("Tracking", "InvTaxCode.aspx", "pageid=" & strPageId & "&id=ddlTaxCode") & "', '', 'scrollbars=yes,resizable=yes,width=500,height=300,status=no,menubar=no');"">" & e.Item.Cells(EnumDn.icTaxCode).Text & "</a>"
                cell.Controls.Add(lbl)
            End If
        End If

        dtgDnDetail.AllowSorting = False
        Grid_ItemCreated(dtgDnDetail, e)
    End Sub

    Private Sub dtgDnDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDnDetail.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dblSubTotal As Double
            Dim j As Integer
            Dim lstItem As New ListItem

            dblSubTotal = Common.parseNull(dv("DND_QTY"), 0) * Common.parseNull(dv("DND_UNIT_COST"), 0)
            e.Item.Cells(EnumDn.icTotal).Text = Format(dblSubTotal, "#,##0.00")
            e.Item.Cells(EnumDn.icUPrice).Text = Format(dv("DND_UNIT_COST"), "#,##0.0000")

            Dim ddlTaxCode As DropDownList
            Dim lblTaxCode As Label
            ddlTaxCode = e.Item.Cells(EnumDn.icTaxCode).FindControl("ddlTaxCode")
            lblTaxCode = e.Item.Cells(EnumDn.icTaxCode).FindControl("lblTaxCode")

            If Request.QueryString("cmd") = "verify" Or Request.QueryString("cmd") = "approve" Then
                If Common.parseNull(dv("DND_GST_RATE")) = "" Or Common.parseNull(dv("DND_GST_RATE")) = "N/A" Then
                    ddlTaxCode.Items.Clear()
                    'Issue 7480 - CH - 24 Mar 2015 (No.56)
                    If dv("DND_QTY") > 0 Or dv("DND_UNIT_COST") > 0 Then
                        lstItem.Value = "NS"
                        lstItem.Text = "NS"
                    Else
                        lstItem.Value = "N/A"
                        lstItem.Text = "N/A"
                    End If
                    'lstItem.Value = "N/A"
                    'lstItem.Text = "N/A"
                    ddlTaxCode.Items.Insert(0, lstItem)
                    ddlTaxCode.Enabled = False
                Else
                    'Jules 2018.10.08 - SST
                    'objGlobal.FillTaxCode(ddlTaxCode, , "P")
                    objGlobal.FillTaxCode(ddlTaxCode, Common.parseNull(dv("DND_GST_RATE")), "P",,, True)
                    'End modification.

                    If aryPTaxCode.Count > 0 Then
                        For j = 0 To aryPTaxCode.Count - 1
                            If Common.parseNull(dv("DND_DN_LINE")) = aryPTaxCode(j)(0) Then
                                ddlTaxCode.SelectedValue = aryPTaxCode(j)(1)
                            End If
                        Next
                    Else
                        'Issue 7480 - CH - 24/2/2015
                        'ddlTaxCode.SelectedValue = dv("DND_GST_INPUT_TAX_CODE")
                        If dblSubTotal > 0 Then
                            'Jules 2018.10.08 - Check if already have tax code. Otherwise, stick to default value.
                            If Common.parseNull(dv("DND_GST_INPUT_TAX_CODE")) <> "" Then
                                ddlTaxCode.SelectedValue = Common.parseNull(dv("DND_GST_INPUT_TAX_CODE"))
                            End If
                            'End modification.
                        Else
                            ddlTaxCode.SelectedValue = ""
                        End If
                    End If

                    If ddlTaxCode.SelectedValue = "N/A" Then
                        ddlTaxCode.Enabled = False
                    Else
                        If dblSubTotal > 0 Then
                            ddlTaxCode.Enabled = True
                        Else
                            ddlTaxCode.Enabled = False
                        End If
                        'ddlTaxCode.Enabled = True
                    End If
                End If

                ddlTaxCode.Visible = True
                lblTaxCode.Visible = False
            Else
                ddlTaxCode.Visible = False
                lblTaxCode.Visible = True

                lblTaxCode.Text = Common.parseNull(dv("DND_GST_INPUT_TAX_CODE"))
            End If

            
            ViewState("total") = ViewState("total") + CDbl(Format(dblSubTotal, "##0.00"))
            'Issue 7480 - CH - 24 Mar 2015 (No.56)
            If Common.parseNull(dv("DND_GST_RATE")) = "NS" Or Common.parseNull(dv("DND_GST_RATE")) = "" Or Common.parseNull(dv("DND_GST_RATE")) = "N/A" Then
                e.Item.Cells(EnumDn.icGstAmt).Text = "0.00"
                ViewState("gst") = ViewState("gst") + 0
            Else
                'Issue 7480 - CH - 23 Mar 2015 (No.38)
                e.Item.Cells(EnumDn.icGstAmt).Text = Format(dblSubTotal / 100 * Common.parseNull(dv("TAX")), "#,##0.00")
                ViewState("gst") = ViewState("gst") + CDbl(Format((dblSubTotal / 100 * Common.parseNull(dv("TAX"))), "###0.00"))
            End If
        End If
    End Sub

    Private Sub dtgAppFlow_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAppFlow.ItemCreated
        Grid_ItemCreated(sender, e)
    End Sub

    Private Sub dtgAppFlow_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAppFlow.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim intTotalCell, intLoop As Integer
            If dv("DNA_Seq") - 1 = dv("DNA_AO_Action") Then
                intTotalCell = e.Item.Cells.Count - 1
                For intLoop = 0 To intTotalCell
                    e.Item.Cells(intLoop).Font.Bold = True
                Next

                ViewState("CurrentAppSeq") = dv("DNA_Seq")
                ViewState("ApprType") = dv("DNA_APPROVAL_TYPE")
            End If

            ViewState("HighestAppr") = dv("DNA_Seq")

            e.Item.Cells(3).Text = "Approval"

            If IsDBNull(dv("AAO_NAME")) Then
                e.Item.Cells(2).Text = "-"
            End If

            If UCase(Common.parseNull(dv("DNA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("DNA_AO"))) Then
                e.Item.Cells(1).Font.Bold = True
            ElseIf UCase(Common.parseNull(dv("DNA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("DNA_A_AO"))) Then
                e.Item.Cells(2).Font.Bold = True
            End If

            If Not IsDBNull(dv("DNA_ACTION_DATE")) Then
                e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("DNA_ACTION_DATE")) & " " & Format(CDate(dv("DNA_ACTION_DATE")), "HH:mm:ss")
            End If

            If Not IsDBNull(dv("DNA_AO_REMARK")) Then
                e.Item.Cells(5).Text = dv("DNA_AO_REMARK")
            End If

        End If
    End Sub
    Private Sub cmdMark_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMark.Click
        Dim objDn As New DebitNote
        Dim strMsg As String

        strMsg = objDn.payDebitNote(ViewState("Dn_Index"), lblDnNo.Text)

        Dim strurl As String = ""
        If strMsg <> "" Then
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        Else
            'If strFrm = "Dashboard" Then
            '    strurl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & "Dashboard" & "&pageid=" & strPageId)
            'ElseIf strFrm = "CreditNoteTrackingList" Then
            '    strurl = dDispatcher.direct("CreditNote", "CreditNoteTrackingList.aspx", "Frm=" & "CreditNoteTrackingList" & "&pageid=" & strPageId)
            'End If
            'cmdSave.Visible = False

            If strFrm = "Dashboard" Then
                If Request.QueryString("action") = "N" Then
                    strurl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & "Dashboard" & "&pageid=" & strPageId)
                Else
                    strurl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & "Dashboard" & "&pageid=" & strPageId)
                End If
            ElseIf strFrm = "DebitNoteTrackingList" Then
                strurl = dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "Frm=" & "InvoiceTrackingList" & "&pageid=" & strPageId & "&action=" & Request.QueryString("action"))

            ElseIf strFrm = "DebitNoteVerifiedTrackingList" Then
                strurl = dDispatcher.direct("DebitNote", "DebitNoteVerifiedTrackingList.aspx", "Frm=" & "DebitNoteVerifiedTrackingList" & "&pageid=" & strPageId)
            ElseIf strFrm = "DebitNoteVerified" Then
                strurl = dDispatcher.direct("DebitNote", "DebitNoteVerified.aspx", "pageid=" & strPageId)
            ElseIf strFrm = "DebitNotePaidTrackingList" Then
                strurl = dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId)
            End If
            cmdAppDn.Visible = False
            cmdSave.Visible = False
            cmdMark.Visible = False

            Common.NetMsgbox(Me, "Debit Note Number " & Request(Trim("DNNO")) & " has been paid.", strurl)
        End If

        'Dim objTrac As New Tracking
        'objTrac.updateAppRemark(ViewState("InvoiceIndex"), txtRemark.Text)
        'Dim blnApproved As Boolean
        'lblMsg.Text = ""

        'If Request.QueryString("folder") = "N" Then
        '    blnApproved = payment("FO")
        'ElseIf Request.QueryString("folder") = "A" Then
        '    blnApproved = payment("FM")
        'End If
        'Dim strurl As String = ""

        'If blnApproved Then
        '    If strFrm = "Dashboard" Then
        '        If Request.QueryString("folder") = "N" Then
        '            strurl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & "Dashboard" & "&pageid=" & strPageId)
        '        Else
        '            strurl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & "Dashboard" & "&pageid=" & strPageId)
        '        End If
        '    ElseIf strFrm = "InvoiceTrackingList" Then
        '        strurl = dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "Frm=" & "InvoiceTrackingList" & "&pageid=" & strPageId)

        '    ElseIf strFrm = "InvoiceVerifiedTrackingList" Then
        '        strurl = dDispatcher.direct("Tracking", "InvoiceVerifiedTrackingList.aspx", "Frm=" & "InvoiceVerifiedTrackingList" & "&pageid=" & strPageId)
        '    ElseIf strFrm = "InvoiceVerified" Then
        '        strurl = dDispatcher.direct("Tracking", "InvoiceVerified.aspx", "pageid=" & strPageId)
        '    ElseIf strFrm = "InvoicePaidTrackingList" Then
        '        strurl = dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId)
        '    End If
        '    cmdAppDn.Visible = False

        '    Common.NetMsgbox(Me, "Invoice Number " & Request(Trim("INVNO")) & " has been mark as paid.", strurl)
        'End If

    End Sub
    Private Sub cmdAppInv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAppDn.Click
        'updating her to save and approve   Srihari
        Dim objDn As New DebitNote
        Dim strMsg As String

        lblMsg.Text = ""
        If chkPurchaseTaxCode(strMsg) = True Then
            SavePurchaseTaxCode()

            'Jules 2018.10.19
            Dim blnFM As Boolean
            Dim ds As DataSet
            ds = objDn.getDnApprFlow(Request("DNNO"), Request("vcomid"))
            If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    If ds.Tables(0).Rows(i)("DNA_AGA_TYPE") = "FM" Then
                        blnFM = True
                        Exit For
                    End If
                Next
            End If
            If Not blnFM Then
                strMsg = "There is no Finance Manager assigned to this workflow."
                lblMsg.Text = strMsg
            Else 'original code
                objDn.updateAppRemark(ViewState("Dn_Index"), txtRemark.Text, , , aryPTaxCode)
            End If
            'End modification.

            Dim blnApproved As Boolean
            If Request.QueryString("cmd") = "verify" Then
                blnApproved = approval("FO")
            ElseIf Request.QueryString("cmd") = "approve" Then
                blnApproved = approval("FM")
            End If
            Dim strurl As String = ""

            If blnApproved Then
                If strFrm = "Dashboard" Then
                    If Request.QueryString("action") = "N" Then
                        strurl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & "Dashboard" & "&pageid=" & strPageId)
                    Else
                        strurl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & "Dashboard" & "&pageid=" & strPageId)
                    End If
                ElseIf strFrm = "DebitNoteTrackingList" Then
                    strurl = dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "Frm=" & "InvoiceTrackingList" & "&pageid=" & strPageId & "&action=" & Request.QueryString("action"))

                ElseIf strFrm = "DebitNoteVerifiedTrackingList" Then
                    strurl = dDispatcher.direct("DebitNote", "DebitNoteVerifiedTrackingList.aspx", "Frm=" & "DebitNoteVerifiedTrackingList" & "&pageid=" & strPageId)
                ElseIf strFrm = "DebitNoteVerified" Then
                    strurl = dDispatcher.direct("DebitNote", "DebitNoteVerified.aspx", "pageid=" & strPageId)
                ElseIf strFrm = "DebitNotePaidTrackingList" Then
                    strurl = dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId)
                End If
                cmdAppDn.Visible = False
                cmdSave.Visible = False

                If ViewState("ISHighestLevel") = True Then
                    Common.NetMsgbox(Me, "Debit Note Number " & Request(Trim("DNNO")) & " has been approved for payment.", strurl)
                Else
                    If Request.QueryString("cmd") = "verify" Then
                        Common.NetMsgbox(Me, "Debit Note Number " & Request(Trim("DNNO")) & " has been verified.", strurl)
                    ElseIf Request.QueryString("cmd") = "approve" Then
                        Common.NetMsgbox(Me, "Debit Note Number " & Request(Trim("DNNO")) & " has been approved.", strurl)
                    End If
                End If
            End If
        Else
            lblMsg.Text = strMsg
        End If

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim objDn As New DebitNote
        Dim strMsg As String

        lblMsg.Text = ""
        SavePurchaseTaxCode()
        If chkPurchaseTaxCode(strMsg) = True Then
            objDn.updateAppRemark(ViewState("Dn_Index"), txtRemark.Text, , , aryPTaxCode)
            Common.NetMsgbox(Me, MsgRecordSave, Request.Url.AbsoluteUri, MsgBoxStyle.Information)
        Else
            lblMsg.Text = strMsg
        End If

        resetDataGrid()

    End Sub
    Private Function payment(ByVal strRole As String) As Boolean
        Dim objTrac As New Tracking
        Dim dtItem As New DataTable
        dtItem.Columns.Add("InvNo", Type.GetType("System.String"))
        dtItem.Columns.Add("PoIndex", Type.GetType("System.String"))
        dtItem.Columns.Add("Vendor", Type.GetType("System.String"))
        dtItem.Columns.Add("FinRemark", Type.GetType("System.String"))
        dtItem.Columns.Add("InvStatus", Type.GetType("System.Int32"))
        dtItem.Columns.Add("Submitted", Type.GetType("System.String"))
        dtItem.Columns.Add("AppDate", Type.GetType("System.String"))
        dtItem.Columns.Add("PayTerm", Type.GetType("System.String"))
        dtItem.Columns.Add("BillMethod", Type.GetType("System.String"))

        Dim dtr As DataRow
        Dim blnSend As Boolean

        dtr = dtItem.NewRow
        dtr("InvNo") = Me.lblInvNo.Text

        dtr("PoIndex") = ViewState("po_index")
        dtr("Vendor") = ViewState("vendorId")
        dtr("FinRemark") = txtRemark.Text
        dtr("InvStatus") = 0
        dtr("Submitted") = ""
        dtr("AppDate") = ""
        dtr("PayTerm") = ""
        dtr("BillMethod") = ViewState("billing_method")

        dtr("AppDate") = Date.Today.ToString
        dtr("InvStatus") = invStatus.Paid
        blnSend = False

        dtItem.Rows.Add(dtr)
        objTrac.updateInvoice(dtItem, blnSend)
        Return True
    End Function
    Private Function approval(ByVal strRole As String) As Boolean

        Dim objTrac As New Tracking
        Dim objDn As New DebitNote

        Dim strMsg As String

        'Jules 2018.10.19
        If strInvAppr = "N" Then
            strMsg = objDn.ApproveDN(ViewState("Dn_Index"), Common.Parse(Me.lblDnNo.Text), ViewState("CurrentAppSeq"), ViewState("ISHighestLevel"), strRole, txtRemark.Text, True)
        Else 'original code
            strMsg = objDn.ApproveDN(ViewState("Dn_Index"), Common.Parse(Me.lblDnNo.Text), ViewState("CurrentAppSeq"), ViewState("ISHighestLevel"), strRole, txtRemark.Text, False)
        End If
        'End modification.


        If strMsg <> "" Then
            Common.NetMsgbox(Me, strMsg)
            Return False
        End If

        Return True
    End Function

    Public Sub cmd_next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles back.ServerClick

        Dim strurl As String = Session("strurl")
        If strFrm = "Dashboard" Then
            If strName = "Buyer" Then
                strurl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & "Dashboard" & "&pageid=" & strPageId)

            ElseIf strName = "FMnAO" Then
                strurl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & "Dashboard" & "&pageid=" & strPageId)
            End If

        ElseIf strFrm = "DebitNoteTrackingList" Then
            strurl = dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "Frm=" & "DebitNoteTrackingList" & "&pageid=" & strPageId)

        ElseIf strFrm = "DebitNoteVerifiedTrackingList" Then
            strurl = dDispatcher.direct("DebitNote", "DebitNoteVerifiedTrackingList.aspx", "Frm=" & "DebitNoteVerifiedTrackingList" & "&pageid=" & strPageId)

        ElseIf strFrm = "DebitNotePaidTrackingList" Then
            strurl = dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "Frm=" & "DebitNotePaidTrackingList" & "&pageid=" & strPageId)

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
        For Each dgitem In dtgDnDetail.Items
            ddlTaxCode = dgitem.FindControl("ddlTaxCode")
            aryPTaxCode.Add(New String() {dgitem.Cells(EnumDn.icLineItem).Text, ddlTaxCode.SelectedValue})
        Next

    End Sub

    Public Function chkPurchaseTaxCode(ByRef strMsg As String) As Boolean
        chkPurchaseTaxCode = True
        Dim dgitem As DataGridItem
        Dim ddlTaxCode As DropDownList
        Dim objGst As New GST

        strMsg = "<ul type='disc'>"

        For Each dgitem In dtgDnDetail.Items
            ddlTaxCode = dgitem.FindControl("ddlTaxCode")

            If CDec(dgitem.Cells(EnumDn.icTotal).Text) > 0 Then
                If dgitem.Cells(EnumDn.icHidGstRate).Text = "" Or dgitem.Cells(EnumDn.icHidGstRate).Text = "N/A" Then
                Else
                    If objGst.chkValidTaxCode(dgitem.Cells(EnumDn.icHidGstRate).Text, ddlTaxCode.SelectedValue, "P") = False Then
                        strMsg &= "<li>" & dgitem.Cells(EnumDn.icLineItem).Text & ". Invalid Purchase Tax Code.<ul type='disc'></ul></li>"
                        chkPurchaseTaxCode = False
                    End If
                End If
            Else
                If dgitem.Cells(EnumDn.icHidGstRate).Text = "" Or dgitem.Cells(EnumDn.icHidGstRate).Text = "N/A" Then
                Else
                    If ddlTaxCode.SelectedValue <> "" Then
                        strMsg &= "<li>" & dgitem.Cells(EnumDn.icLineItem).Text & ". Invalid Purchase Tax Code.<ul type='disc'></ul></li>"
                        chkPurchaseTaxCode = False
                    End If
                End If
            End If
            'If dgitem.Cells(EnumDn.icHidGstRate).Text = "" Or dgitem.Cells(EnumDn.icHidGstRate).Text = "N/A" Then
            'Else
            '    If objGst.chkValidTaxCode(dgitem.Cells(EnumDn.icHidGstRate).Text, ddlTaxCode.SelectedValue, "P") = False Then
            '        strMsg &= "<li>" & dgitem.Cells(EnumDn.icLineItem).Text & ". Invalid Purchase Tax Code.<ul type='disc'></ul></li>"
            '        chkPurchaseTaxCode = False
            '    End If
            'End If
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
            If Session("role") = "3" Then
                Session("w_DnTracking_tabs") = "<div class=""t_entity""><ul>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=N&status=1") & """><span>Verified Debit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteVerified.aspx", "pageid=" & strPageId) & """><span>Approved Debit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "</ul><div></div></div>"
            Else
                Session("w_DnTracking_tabs") = "<div class=""t_entity""><ul>" & _
                            "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteVerified.aspx", "pageid=" & strPageId) & """><span>Verified Debit Note</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "</ul><div></div></div>"
            End If

        ElseIf strFrm = "InvoiceDetails" Then
            Session("w_DnTracking_tabs") = Session("w_InvTracking_tabs")

        ElseIf strFrm = "DebitNoteTrackingList" Or strFrm = "Dashboard" Then
            If Session("role") = "3" Then
                Session("w_DnTracking_tabs") = "<div class=""t_entity""><ul>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&folder=" & "N" & "&status=" & "1") & """><span>Verified Debit Note</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteVerified.aspx", "pageid=" & strPageId) & """><span>Approved Debit Note</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "</ul><div></div></div>"
            Else
                If Request.QueryString("cmd") = "none" Then
                    Session("w_DnTracking_tabs") = "<div class=""t_entity""><ul>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=N&status=1") & """><span>New Debit Note</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=S&status=1") & """><span>Verified Debit Note</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "</ul><div></div></div>"
                    'ElseIf Request.QueryString("cmd") = "API" Then
                    '    Session("w_DnTracking_tabs") = "<div class=""t_entity""><ul>" & _
                    '                                "<li><div class=""space""></div></li>" & _
                    '                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=N&status=1") & """><span>New Debit Note</span></a></li>" & _
                    '                                "<li><div class=""space""></div></li>" & _
                    '                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=S&status=1") & """><span>Verified Debit Note</span></a></li>" & _
                    '                                "<li><div class=""space""></div></li>" & _
                    '                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                    '                                "<li><div class=""space""></div></li>" & _
                    '                                "</ul><div></div></div>"
                Else
                    Session("w_DnTracking_tabs") = "<div class=""t_entity""><ul>" & _
                                                "<li><div class=""space""></div></li>" & _
                                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=N&status=1") & """><span>New Debit Note</span></a></li>" & _
                                                "<li><div class=""space""></div></li>" & _
                                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=S&status=1") & """><span>Verified Debit Note</span></a></li>" & _
                                                "<li><div class=""space""></div></li>" & _
                                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                                                "<li><div class=""space""></div></li>" & _
                                                "</ul><div></div></div>"
                End If

            End If


            'ElseIf strFrm = "InvoiceVerifiedTrackingList" Then
            '    Session("w_DnTracking_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=N&status=1") & """><span>New Debit Note</span></a></li>" & _
            '            "<li><div class=""space""></div></li>" & _
            '            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteVerifiedTrackingList.aspx", "pageid=" & strPageId) & """><span>Verified Debit Note</span></a></li>" & _
            '            "<li><div class=""space""></div></li>" & _
            '            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
            '            "<li><div class=""space""></div></li>" & _
            '            "</ul><div></div></div>"

        ElseIf strFrm = "DebitNotePaidTrackingList" Then
            If Request.QueryString("role") = "3" Then 'ie FM
                Session("w_DnTracking_tabs") = "<div class=""t_entity""><ul>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=N&status=1") & """><span>Verified Debit Note</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteVerified.aspx", "pageid=" & strPageId) & """><span>Approved Debit Note</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                                "</ul><div></div></div>"
            Else
                Session("w_DnTracking_tabs") = "<div class=""t_entity""><ul>" & _
                        "<li><div class=""space""></div></li>" & _
                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=N&status=1") & """><span>New Debit Note</span></a></li>" & _
                        "<li><div class=""space""></div></li>" & _
                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=S&status=1") & """><span>Verified Debit Note</span></a></li>" & _
                        "<li><div class=""space""></div></li>" & _
                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                        "<li><div class=""space""></div></li>" & _
                        "</ul><div></div></div>"
            End If
        ElseIf strFrm = "DebitNoteVerified" Then
            If Session("role") = "3" Then
                Session("w_DnTracking_tabs") = "<div class=""t_entity""><ul>" & _
                        "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=N&status=1") & """><span>Verified Debit Note</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteVerified.aspx", "pageid=" & strPageId) & """><span>Approved Debit Note</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "</ul><div></div></div>"
            Else
                Session("w_DnTracking_tabs") = "<div class=""t_entity""><ul>" & _
                            "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteVerified.aspx", "pageid=" & strPageId) & """><span>Verified Debit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "</ul><div></div></div>"
            End If
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

    Private Sub cmdPreviewDN_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPreviewDN.Click
        resetDataGrid()
    End Sub
End Class
