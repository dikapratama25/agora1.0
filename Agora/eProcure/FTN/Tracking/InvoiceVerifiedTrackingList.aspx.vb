
Imports System.IO
Imports AgoraLegacy
Imports eProcure.Component

Partial Public Class InvoiceVerifiedTrackingListFTN
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Dim objTrac As New Tracking
    'Protected WithEvents cmdDelete As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents cmdPdf As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblRemark As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdSaveInv As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents cmdPrint As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    'Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    'Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    Dim dtPR As DataTable
    'Protected WithEvents cmdMassApp As System.Web.UI.WebControls.Button
    Dim objGlobal As New AppGlobals

    Public Enum EnumInv
        icChk = 0
        icInvNo = 1
        icIndex = 2
        icPayDate = 3
        icPoIndex = 4
        icVenId = 5
        icVenName = 6
        icCurrency = 7
        icAmount = 8
        icDoc = 9
        icPayTerm = 10
        icBuyer = 11
        icDept = 12
        icPrinted = 13
        icStatusCode = 14
        icStatus = 15
        icRemarks = 16
        icAppDate = 17
        icBillMethod = 18
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    'Protected WithEvents cboFolder As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents txtDocNo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtVendorName As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtBuyer As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtDept As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdApprove As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdMark As System.Web.UI.WebControls.Button
    Protected WithEvents lblType As System.Web.UI.WebControls.Label
    'Protected WithEvents dtgInvoice As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtStartDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEndDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)

        Dim oriPageCount = Session("PageCount")
        Session("PageCount") = 10

        SetGridProperty(dtgInvoice)
        Session("PageCount") = oriPageCount
        SetGridProperty(dtgInvoice) 'Michelle (6/9/2007) - To rectify no. of records display per page according to user's setting

        If Not IsPostBack Then
            GenerateTab()
            Dim strFO, strFM As String
            Dim blnFO, blnFM As Boolean
            Dim objUsers As New Users
            strFO = System.Enum.GetName(GetType(FixedRole), FixedRole.Finance_Officer)
            strFM = System.Enum.GetName(GetType(FixedRole), FixedRole.Finance_Manager)
            strFO = "'" & Replace(strFO, "_", " ") & "'"
            strFM = "'" & Replace(strFM, "_", " ") & "'"

            blnFO = objUsers.checkUserFixedRole(strFO)
            blnFM = objUsers.checkUserFixedRole(strFM)
            ViewState("role") = getUserRole(blnFO, blnFM)

            Select Case ViewState("role")
                Case 2 ' is FO only
                    addListItem("New", "1")
                    addListItem("Sent", "2")
                    'addListItem("Trash", "5") - Michelle (CR0007)
                    addListItem("Archive", "5")
                    cboFolder.SelectedItem.Value = "2"
                    ViewState("status") = "2,3,5"
                    'cboFolder_SelectedIndexChanged(sender, e)
                    cmdApprove.Visible = False
                    'cmdMassApp.Visible = False
                    cmdMark.Visible = False
                    'ViewState("status") = "1"
                Case 3 ' is FM only
                    addListItem("Approval", "3")
                    addListItem("Payment", "4")
                    'addListItem("Trash", "5") - Michelle (CR0007)
                    addListItem("Archive", "5")
                    cboFolder.SelectedItem.Value = "3"
                    ViewState("status") = "2"
                    cmdSubmit.Visible = False
                    ' cmdMassApp.Visible = True - Michelle (CR0014) 
                    cmdMark.Visible = False
                    'ViewState("status") = "2"
                Case 4 ' is both
                    addListItem("New", "1")
                    addListItem("Sent", "2")
                    addListItem("Approval", "3")
                    addListItem("Payment", "4")
                    'addListItem("Trash", "5") - Michelle (CR0007)
                    addListItem("Archive", "5")
                    cboFolder.SelectedItem.Value = "2"
                    ViewState("status") = "2,3,5"
                    'cboFolder_SelectedIndexChanged(sender, e)
                    cmdApprove.Visible = False
                    'cmdMassApp.Visible = False
                    cmdMark.Visible = False
                    'ViewState("status") = "1"
            End Select
            Session("strurl") = strCallFrom
            'ButtonDislayChanged()
            SortDateDesc()
            'lblType.Text = "Invoice Tracking (" & cboFolder.SelectedItem.Text & ")"
            'lblRemark.Text = "This list provides a summary of the Invoice/Payment activity between your company and your vendors."

        End If

        If ViewState("role") = "3" Or ViewState("role") = "4" Then
            'cmdMassApp.Enabled = True - Michelle (CR0014)
        Else

            Dim objInv As New Invoice
            If objInv.checkMassApp(Session("UserID")) = 1 Then
                'cmdMassApp.Visible = True - Michelle (CR0014)
                'cmdMassApp.Enabled = True - Michelle (CR0014)
            Else
                'cmdMassApp.Enabled = False
            End If
        End If

        If ViewState("PageCount") = 0 Then
            'cmdMassApp.Enabled = False
        End If

        'cmdMassApp.Attributes.Add("onclick", "return CustomMsg('InvMassAppr');")

        cmdPrint.Attributes.Add("onclick", "return CheckAtLeastOnePrint('chkSelection');")
        cmdPdf.Attributes.Add("onclick", "return CheckAtLeastOnePdf('chkSelection');")
        cmdSaveInv.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
        cmdSubmit.Attributes.Add("onclick", "return checkAtLeastOneResetSummary('chkSelection','',0,1);")
        cmdMark.Attributes.Add("onclick", "return checkAtLeastOneResetSummary('chkSelection','',0,1);")
        cmdApprove.Attributes.Add("onclick", "return checkAtLeastOneResetSummary('chkSelection','',0,1);")
        cmdSave.Attributes.Add("onClick", "return resetSummary(0,1);")

        'Michelle (24/8/2007) - Remove the popup as it displays the wrong message
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOnePdf('chkSelection');")
        cmdDelete.ImageUrl = dDispatcher.direct("Plugins/images", "archive.GIF")
        cmdPdf.ImageUrl = dDispatcher.direct("Plugins/images", "pdf.bmp")
        cmdSaveInv.ImageUrl = dDispatcher.direct("Plugins/images", "i_save.gif")
        cmdPrint.ImageUrl = dDispatcher.direct("Plugins/images", "i_printer.gif")


    End Sub

    Private Function getUserRole(ByVal blnFo As Boolean, ByVal blnFM As Boolean) As Integer
        If blnFo And blnFM Then
            getUserRole = 4
        ElseIf blnFo = False And blnFM = True Then
            getUserRole = 3
        ElseIf blnFo = True And blnFM = False Then
            getUserRole = 2
        Else
            getUserRole = 1
        End If
    End Function

    Private Sub addListItem(ByVal strText As String, ByVal strVal As String)
        Dim lstItem As New ListItem
        lstItem.Value = strVal
        lstItem.Text = strText
        cboFolder.Items.Add(lstItem)
    End Sub

    Function bindgridINV() As String

        Dim ds As New DataSet
        ''ds = objTrac.getInvoiceTracking(txtDocNo.Text, txtVendorName.Text, txtBuyer.Text, txtDept.Text, viewstate("status"))
        ds = objTrac.getInvoiceTracking(txtDocNo.Text, txtVendorName.Text, "", "", ViewState("status"), , , , False)

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If ViewState("action") = "del" Then
            If dtgInvoice.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgInvoice.PageSize = 0 Then
                dtgInvoice.CurrentPageIndex = dtgInvoice.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        'viewstate("dtInvoiceTracking") = dvViewSample.Table

        intPageRecordCnt = ds.Tables(0).Rows.Count

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgInvoice, dvViewSample)

            dtgInvoice.DataSource = dvViewSample
            hidSummary.Value = ""
            hidControl.Value = ""
            dtgInvoice.DataBind()
            ButtonDislayChanged()
        Else
            FolderChanged(False, False, False, False, False, False, False)
            dtgInvoice.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ' add for above checking
        ViewState("PageCount") = dtgInvoice.PageCount
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgInvoice.CurrentPageIndex = 0
        bindgridINV()
    End Sub

    Private Sub dtgInvoice_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInvoice.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim hidCode As HtmlInputHidden
            hidCode = e.Item.FindControl("hidCode")
            hidCode.Value = dv("IM_INVOICE_NO")

            Dim txtQ As TextBox
            txtQ = e.Item.FindControl("txtQ")

            If hidControl.Value = "" Then
                hidControl.Value = hidCode.ClientID & "-" & txtQ.ClientID
            Else
                hidControl.Value &= "," & hidCode.ClientID & "-" & txtQ.ClientID
            End If

            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim drMatch() As DataRow
            Dim drRow As DataRow
            Dim strTemp As String = ""
            Dim strURL As String
            Dim intPoIndex As Long
            Dim intLoop, intCnt As Integer

            Dim dvMatch As DataView = dv.DataView
            intCnt = dvMatch.Count - 1

            Dim lnkDocNo As HyperLink
            lnkDocNo = e.Item.FindControl("lnkINVNo")
            lnkDocNo.Text = dv("IM_INVOICE_NO")

            '            lnkDocNo.Attributes.Add("onclick", "PopWindow('../Invoice/PreviewInvoice.aspx?pageid=" & strPageId & "&INVNO=" & Common.parseNull(dv("IM_INVOICE_NO")) & "&freeze=0&vcomid=" & Common.parseNull(dv("IM_S_COY_ID")) & "',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');")
            'lnkDocNo.NavigateUrl = "#"
            lnkDocNo.NavigateUrl = dDispatcher.direct("Tracking", "InvoiceDetails.aspx", "Frm=" & "InvoiceVerifiedTrackingList" & "&pageid=" & strPageId & "&INVNO=" & Common.parseNull(dv("IM_INVOICE_NO")) & "&vcomid=" & Common.parseNull(dv("IM_S_COY_ID")) & "&folder=" & Getfoldertype() & "&status=" & Common.parseNull(dv("IM_INVOICE_STATUS")))

            intPoIndex = dv("IM_PO_INDEX")
            'Michelle (6/9/2007) - To cater for 1 PR to multiple POs
            'dtPR = objTrac.getRelatedPR(intPoIndex)
            dtPR = objTrac.getRelatedPR_PO(dv("POM_PO_NO"), Session("CompanyID"))

            For Each drRow In dtPR.Rows
                strURL = "<a href=" & dDispatcher.direct("PO", "PRDetail.aspx", "caller=" & "OTHER" & "&PageID=" & strPageId & _
                    "&index=" & drRow("PRM_PR_INDEX") & "&PRNo=" & drRow("PRM_PR_NO")) & ">" & drRow("PRM_PR_NO") & "</a>"
                strTemp &= strURL & "<BR>"
            Next

            strTemp &= "<a href=" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=" & "InvoiceVerifiedTrackingList" & "&caller=" & "buyer" & "&PO_INDEX=" & dv("IM_PO_INDEX") & "&PO_NO=" & dv("POM_PO_NO") & "&BCoyID=" & Session("CompanyID") & "&side=" & "other" & "&filetype=" & "2" & "&pageid=" & strPageId) & ">" & dv("POM_PO_NO") & "</a><BR>"
            'strURL = "../DO/DODetails.aspx?caller=buyer&pageid=" & strPageId & "&DONo=" & dv("CDM_DO_NO") & "&POIdx=" & intPoIndex & "&SCoyID=" & dv("CDM_S_COY_ID")
            'strURL = dDispatcher.direct("Report", "PreviewDO.aspx", "caller=" & "buyer" & "&pageid=" & strPageId & "&DONo=" & dv("CDM_DO_NO") & "&POIdx=" & intPoIndex & "&SCoyID=" & dv("CDM_S_COY_ID") & "&PO_NO=" & dv("POM_PO_NO"))
            strURL = dDispatcher.direct("DO", "PreviewDODetails.aspx", "caller=buyer" & "&DONo=" & dv("CDM_DO_NO") & "&POIdx=" & intPoIndex & "&SCoyID=" & dv("CDM_S_COY_ID") & "&PageID=" & strPageId)
            strTemp &= "<a href=""#"" onclick=""PopWindow('" & strURL & "')"">" & dv("CDM_DO_NO") & "</a><BR>"
            'strTemp &= "<a href=../GRN/GRNDetails.aspx?Frm=InvoiceVerifiedTrackingList&GRNNo=" & dv("CDM_GRN_NO") & "&BCoyID=" & Session("CompanyID") & "&type=other&pageid=" & strPageId & ">" & dv("CDM_GRN_NO") & "</a><BR>"
            strURL = dDispatcher.direct("Report", "PreviewGRN.aspx", "GRNNo=" & dv("CDM_GRN_NO") & "&PONo=" & Common.parseNull(dv("POM_PO_NO")) & "&DONo=" & Common.parseNull(dv("CDM_DO_NO")) & "&pageid=" & strPageId & "&BCoyID=" & Session("CompanyID"))
            strTemp &= "<a href=""#"" onclick=""PopWindow('" & strURL & "')"">" & dv("CDM_GRN_NO") & "</a><BR>"

            ''For intLoop = 0 To intCnt
            ''For Each drRow In drMatch
            'If Not IsDBNull(dvMatch(intLoop)("CDM_DO_NO")) Then
            '    '//SR : AS0021, MOo, 26/10/2005
            '    '//strURL = "../DO/DOReport.aspx?pageid=" & strPageId & "&DONo=" & dvMatch(intLoop)("CDM_DO_NO") & "&POIdx=" & intPoIndex & "&SCoyID=" & dvMatch(intLoop)("CDM_S_COY_ID")
            '    strURL = "../DO/DODetails.aspx?caller=buyer&pageid=" & strPageId & "&DONo=" & dvMatch(intLoop)("CDM_DO_NO") & "&POIdx=" & intPoIndex & "&SCoyID=" & dvMatch(intLoop)("CDM_S_COY_ID")
            '    strTemp &= "<a href=""#"" onclick=""PopWindow('" & strURL & "')"">" & dvMatch(intLoop)("CDM_DO_NO") & "</a><BR>"

            '    If Not IsDBNull(dvMatch(intLoop)("CDM_GRN_NO")) Then
            '        strTemp &= "<a href=../GRN/GRNDetails.aspx?GRNNo=" & dvMatch(intLoop)("CDM_GRN_NO") & "&BCoyID=" & Session("CompanyID") & "&type=other&pageid=" & strPageId & ">" & dvMatch(intLoop)("CDM_GRN_NO") & "</a><BR>"
            '    End If
            'End If
            ''Next

            If Not IsDBNull(dv("IM_INVOICE_TOTAL")) Then
                e.Item.Cells(EnumInv.icAmount).Text = Format(CDbl(dv("IM_INVOICE_TOTAL")), "###,##0.00")
            End If
            e.Item.Cells(EnumInv.icDoc).Text = strTemp
            e.Item.Cells(EnumInv.icPayDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("IM_PAYMENT_DATE"))

            Dim txtRemark As TextBox
            txtRemark = e.Item.FindControl("txtRemark")
            txtRemark.Attributes.Add("onKeyDown", "limitText (this, 400);")
            txtRemark.Visible = False

            Dim lblRemarks As Label
            lblRemarks = e.Item.FindControl("lblRemarks")
            lblRemarks.Text = ""

            'lblRemarks.Text = Common.parseNull(dv("IM_FINANCE_REMARKS"))

            Dim ds As DataSet = objTrac.getFinanceApprRemarks(Common.parseNull(dv("IM_INVOICE_INDEX"), 0))

            If Not ds Is Nothing Then
                If Not ds.Tables(0) Is Nothing Then
                    If ds.Tables(0).Rows.Count > 0 Then

                        For Each dr As DataRow In ds.Tables(0).Rows
                            Dim intSeq As Integer = Common.parseNull(dr("FA_SEQ"), 0)
                            Dim intAction As Integer = Common.parseNull(dr("FA_AO_ACTION"), 0)
                            Dim strRemarks As String = Common.parseNull(dr("FA_AO_REMARK"), "")

                            If intSeq - 1 = intAction Then

                                Select Case Getfoldertype()
                                    Case "N", "A"
                                        txtRemark.Text = strRemarks
                                        txtRemark.Visible = True
                                    Case Else
                                        'txtRemark.Text = strRemarks
                                        'txtRemark.Visible = True
                                        If strRemarks = "" Then strRemarks = "-"
                                        lblRemarks.Text &= intSeq & ". " & strRemarks & "<br>"
                                End Select
                            ElseIf intSeq = intAction Then
                                'txtRemark.Text = strRemarks
                                'txtRemark.Visible = True
                                If strRemarks = "" Then strRemarks = "-"
                                lblRemarks.Text &= intSeq & ". " & strRemarks & "<br>"
                            Else
                                'txtRemark.Text = strRemarks
                                'txtRemark.Visible = True
                                If strRemarks = "" Then strRemarks = "-"
                                lblRemarks.Text &= intSeq & ". " & strRemarks & "<br>"
                            End If
                        Next
                    End If
                End If
            End If

            If hidSummary.Value = "" Then
                hidSummary.Value = "Finance Remarks-" & txtRemark.ClientID
            Else
                hidSummary.Value &= ",Finance Remarks-" & txtRemark.ClientID
            End If


            Dim cboPay As DropDownList
            cboPay = e.Item.FindControl("cboPay")

            Dim lblPay As Label
            lblPay = e.Item.FindControl("lblPay")

            Select Case cboFolder.SelectedItem.Value
                Case "1" ' new
                    e.Item.Cells(EnumInv.icPayTerm).Visible = False
                    e.Item.Cells(EnumInv.icAppDate).Visible = False
                    'lblRemarks.Visible = False

                Case "2" ' sent
                    e.Item.Cells(EnumInv.icPayTerm).Visible = False
                    e.Item.Cells(EnumInv.icAppDate).Visible = False
                    'lblRemarks.Visible = False
                    'If e.Item.Cells(EnumInv.icStatusCode).Text = invStatus.Paid Or e.Item.Cells(EnumInv.icStatusCode).Text = invStatus.Approved Then
                    '    txtRemark.Visible = False
                    'Else
                    '    lblRemarks.Visible = False
                    'End If

                Case "3" ' approval
                    e.Item.Cells(EnumInv.icPayTerm).Visible = False
                    e.Item.Cells(EnumInv.icAppDate).Visible = False
                    'lblRemarks.Visible = False
                    'If e.Item.Cells(EnumInv.icStatusCode).Text = invStatus.Paid Then
                    '    txtRemark.Visible = False
                    'Else
                    '    lblRemarks.Visible = False
                    'End If

                Case "4" ' payment

                    e.Item.Cells(EnumInv.icDoc).Visible = False

                    ' Check if is highest FM approval

                    Dim blnHighestFMApproval As Boolean = objTrac.HighestFMApprLevel(e.Item.Cells(EnumInv.icIndex).Text)

                    'If e.Item.Cells(EnumInv.icStatusCode).Text = invStatus.PendingAppr Or _
                    '    e.Item.Cells(EnumInv.icStatusCode).Text = invStatus.Paid Or _
                    '    (Not blnHighestFMApproval) Then

                    'Michelle (24/8/2007) - To enable the 'chk' to True if the logon user is the FM and the invoice
                    If Not blnHighestFMApproval Then
                        If ViewState("role") = 3 Or ViewState("role") = 4 Then
                            Dim strSql, strInvAppr
                            Dim objDb As New EAD.DBCom
                            strSql = "SELECT CM_INV_APPR FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Session("CompanyID") & "'"
                            Dim tDS As DataSet = objDb.FillDs(strSql)
                            If tDS.Tables(0).Rows.Count > 0 Then
                                strInvAppr = Common.parseNull(tDS.Tables(0).Rows(0).Item("CM_INV_APPR"))
                            End If
                            If strInvAppr = "N" Then
                                blnHighestFMApproval = True
                            End If
                        End If
                    End If


                    If Not blnHighestFMApproval Or e.Item.Cells(EnumInv.icStatusCode).Text = invStatus.PendingAppr Then
                        chk.Enabled = False

                        'If e.Item.Cells(EnumInv.icStatusCode).Text = invStatus.PendingAppr Then
                        '    lblRemarks.Visible = False
                        'Else
                        '    txtRemark.Visible = False
                        'End If

                        'End If

                        'e.Item.Cells(EnumInv.icPayTerm).Visible = False
                        lblPay.Text = ""
                        cboPay.Visible = False

                        e.Item.Cells(EnumInv.icAppDate).Text = ""
                    Else
                        objGlobal.FillCodeTable(cboPay, CodeTable.PaymentMethod)
                        cboPay.Items.RemoveAt(0)
                        If Not IsDBNull(dv("IM_PAYMENT_TERM")) Then
                            Common.SelDdl(Common.parseNull(dv("IM_PAYMENT_TERM")), cboPay, True, True)
                        Else
                            Common.SelDdl(Common.parseNull(dv("POM_PAYMENT_METHOD")), cboPay, False, True)
                        End If
                        lblPay.Text = cboPay.SelectedItem.Text

                        If Not IsDBNull(dv("IM_FM_APPROVED_DATE")) Then
                            e.Item.Cells(EnumInv.icAppDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("IM_FM_APPROVED_DATE"))
                        End If
                        lblPay.Text = cboPay.SelectedItem.Text

                        If e.Item.Cells(EnumInv.icStatusCode).Text = invStatus.Paid Then
                            'chk.Enabled = False - Michelle (CR0005) - To enable FM to select those that are paid
                            chk.Enabled = True
                            'txtRemark.Visible = False
                            cboPay.Visible = False
                        Else
                            If objTrac.HighestFMApprLevel(e.Item.Cells(EnumInv.icIndex).Text) Then
                                'lblRemarks.Visible = False
                                lblPay.Visible = False
                            Else
                                'txtRemark.Visible = False
                                cboPay.Visible = False
                                lblPay.Visible = False
                            End If
                        End If
                    End If

                Case "5" ' archive - trash
                    objGlobal.FillCodeTable(cboPay, CodeTable.PaymentMethod)
                    cboPay.Items.RemoveAt(0)
                    If Not IsDBNull(dv("IM_PAYMENT_TERM")) Then
                        Common.SelDdl(Common.parseNull(dv("IM_PAYMENT_TERM")), cboPay, True, True)
                    Else
                        Common.SelDdl(Common.parseNull(dv("POM_PAYMENT_METHOD")), cboPay, False, True)
                    End If
                    lblPay.Text = cboPay.SelectedItem.Text
                    cboPay.Visible = False

                    If Not IsDBNull(dv("IM_FM_APPROVED_DATE")) Then
                        e.Item.Cells(EnumInv.icAppDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("IM_FM_APPROVED_DATE"))
                    End If
                    'e.Item.Cells(EnumInv.icPayTerm).Visible = False
                    'e.Item.Cells(EnumInv.icAppDate).Visible = False

                    'txtRemark.Visible = False
            End Select

            If Common.parseNull(dv("IM_PRINTED")) = "1" Then
                e.Item.Cells(EnumInv.icPrinted).Text = "Yes"
            Else
                e.Item.Cells(EnumInv.icPrinted).Text = "No"
            End If

        ElseIf e.Item.ItemType = ListItemType.Header Then
            Select Case cboFolder.SelectedItem.Value
                Case "1", "2", "3"
                    e.Item.Cells(EnumInv.icPayTerm).Visible = False
                    e.Item.Cells(EnumInv.icAppDate).Visible = False
                Case "4"
                    e.Item.Cells(EnumInv.icDoc).Visible = False
                Case "5"
            End Select
        End If

    End Sub

    Private Function Getfoldertype() As String
        Select Case cboFolder.SelectedItem.Value
            Case "1" ' new
                Return "N"

            Case "2" ' sent
                Return "S"

            Case "3" ' approval
                Return "A"

            Case "4" ' payment
                Return "P"

            Case "5" ' archive - trash
                Return "T"

            Case Else
                Return ""
        End Select
    End Function


    Private Sub dtgInvoice_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInvoice.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgInvoice, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dtgInvoice_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgInvoice.PageIndexChanged
        dtgInvoice.CurrentPageIndex = e.NewPageIndex
        bindgridINV()
    End Sub

    Private Function FolderChanged(ByVal blnSave As Boolean, ByVal blnSubmit As Boolean, ByVal blnApprove As Boolean, ByVal blnMark As Boolean, ByVal blnDelete As Boolean, ByVal blnReset As Boolean, ByVal blnMassApp As Boolean)
        cmdSave.Visible = blnSave
        cmdSubmit.Visible = blnSubmit
        'cmdMassApp.Visible = blnMassApp - Michelle (CR0014)
        cmdApprove.Visible = blnApprove
        cmdMark.Visible = blnMark
        cmdDelete.Visible = blnDelete
        cmdReset.Visible = blnReset
    End Function

    Private Function ButtonDislayChanged()
        Select Case cboFolder.SelectedItem.Value
            Case "1" ' New
                Dim objInv As New Invoice
                Dim blnMassApp As Boolean

                If objInv.checkMassApp(Session("UserID")) = 1 Then
                    blnMassApp = True
                Else
                    blnMassApp = False
                End If

                FolderChanged(True, True, False, False, False, True, blnMassApp)
                ViewState("status") = "1"
                'lblRemark.Text = "This list provides a summary of the Invoice/Payment activity between your company and your vendors."
            Case "2" ' Sent
                'FolderChanged(True, False, False, False, False, True, False)
                FolderChanged(False, False, False, False, False, False, False)
                ViewState("status") = "2,3,5"
                'lblRemark.Text = "This list provides a summary of the Invoice/Payment activity between your company and your vendors."
            Case "3" ' Approval
                FolderChanged(True, False, True, False, False, True, True)
                ViewState("status") = "2"
                'lblRemark.Text = "This list provides a summary of the Invoice/Payment activity between your company and your vendors."
            Case "4" ' Payment
                'FolderChanged(True, False, False, True, True, True, False)
                FolderChanged(False, False, False, True, True, True, False)
                ViewState("status") = "3,4"
                'lblRemark.Text = "Items in this folder are Approved Invoices awaiting for payment."
            Case "5" ' archive - Trash
                FolderChanged(False, False, False, False, False, False, False)
                ViewState("status") = ""
                'lblRemark.Text = "This is your temporary archive folder. Items removed from this folder will no longer be available for viewing ."
        End Select

    End Function

    Private Sub cboFolder_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFolder.SelectedIndexChanged

        ButtonDislayChanged()
        'lblType.Text = "Invoice Tracking (" & cboFolder.SelectedItem.Text & ")"
        dtgInvoice.CurrentPageIndex = 0
        SortDateDesc()
    End Sub

    Private Function bindInv(ByVal strType As String, ByVal blnSend As Boolean, ByVal blnFMMassAppr As Boolean)
        Dim dtItem As New DataTable
        dtItem.Columns.Add("InvIndex", Type.GetType("System.Int32"))
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
        Dim strInv As String = ""
        Dim strInvFail As String = ""

        If blnFMMassAppr Then
            Dim dtInvoiceTracking As DataTable = CType(ViewState("dtInvoiceTracking"), DataTable)

            For Each drItem As DataRow In dtInvoiceTracking.Rows

                dtr = dtItem.NewRow
                dtr("InvNo") = CType(drItem.Item("IM_INVOICE_NO"), String)

                strInv &= dtr("InvNo") & ","

                dtr("InvIndex") = CType(IIf(IsDBNull(drItem.Item("IM_INVOICE_INDEX")), "", drItem.Item("IM_INVOICE_INDEX")), Integer)
                dtr("PoIndex") = CType(IIf(IsDBNull(drItem.Item("IM_PO_INDEX")), "", drItem.Item("IM_PO_INDEX")), String)
                dtr("Vendor") = CType(IIf(IsDBNull(drItem.Item("IM_S_COY_ID")), "", drItem.Item("IM_S_COY_ID")), String)
                dtr("FinRemark") = CType(IIf(IsDBNull(drItem.Item("IM_FINANCE_REMARKS")), "", drItem.Item("IM_FINANCE_REMARKS")), String) 'CType(dgItem.FindControl("txtRemark"), TextBox).Text
                dtr("InvStatus") = 0
                dtr("Submitted") = ""
                dtr("AppDate") = ""
                dtr("PayTerm") = ""
                dtr("BillMethod") = CType(IIf(IsDBNull(drItem.Item("POM_BILLING_METHOD")), "", drItem.Item("POM_BILLING_METHOD")), String)

                If strType = "2" Then
                    dtr("Submitted") = Session("UserId")
                    dtr("InvStatus") = invStatus.PendingAppr
                ElseIf strType = "3" Then
                    dtr("AppDate") = Date.Today.ToString
                    dtr("InvStatus") = invStatus.Approved
                End If

                dtItem.Rows.Add(dtr)
            Next

        Else
            Dim chk As CheckBox
            Dim bln As Boolean

            For Each dgItem As DataGridItem In dtgInvoice.Items
                chk = dgItem.FindControl("chkSelection")
                Dim cboPay As DropDownList
                If chk.Checked Or (strType = "1" And chk.Checked = False) Then
                    dtr = dtItem.NewRow
                    dtr("InvNo") = CType(dgItem.FindControl("lnkINVNo"), HyperLink).Text
                    If strType <> "4" Or (strType = "4" And dgItem.Cells(EnumInv.icStatusCode).Text <> invStatus.Paid) Then
                        bln = True
                        strInv &= dtr("InvNo") & ","
                    Else
                        bln = False
                        strInvFail &= dtr("InvNo") & ","
                    End If

                    dtr("InvIndex") = dgItem.Cells(EnumInv.icIndex).Text
                    dtr("PoIndex") = dgItem.Cells(EnumInv.icPoIndex).Text
                    dtr("Vendor") = dgItem.Cells(EnumInv.icVenId).Text
                    dtr("FinRemark") = CType(dgItem.FindControl("txtRemark"), TextBox).Text
                    dtr("InvStatus") = 0
                    dtr("Submitted") = ""
                    dtr("AppDate") = ""
                    dtr("PayTerm") = ""
                    dtr("BillMethod") = dgItem.Cells(EnumInv.icBillMethod).Text
                    Select Case strType
                        Case "1" ' Save
                            If cboFolder.SelectedItem.Value = "4" Then
                                cboPay = dgItem.FindControl("cboPay")
                                If cboPay.SelectedItem Is Nothing Then
                                    dtr("PayTerm") = ""
                                Else
                                    dtr("PayTerm") = cboPay.SelectedItem.Value
                                End If
                            End If
                        Case "2" ' submit button FO
                            dtr("Submitted") = Session("UserId")
                            dtr("InvStatus") = invStatus.PendingAppr
                        Case "3" ' approve button FM
                            dtr("AppDate") = Date.Today.ToString
                            dtr("InvStatus") = invStatus.Approved
                        Case "4" ' paid button
                            dtr("InvStatus") = invStatus.Paid
                            cboPay = dgItem.FindControl("cboPay")
                            dtr("PayTerm") = cboPay.SelectedItem.Value
                    End Select
                    If bln Then
                        dtItem.Rows.Add(dtr)
                    End If
                End If
            Next
        End If

        For Each dtr In dtItem.Rows
            If strType = "2" Or strType = "3" Or strType = "4" Then
                Dim blnHighestLevel As Boolean
                ' Dim strMsg As String = objTrac.ApproveInvoice(Common.Parse(dtr("InvNo")), Common.Parse(dtr("Vendor")), IIf(strType = "2", "FO", "FM"), Common.Parse(dtr("FinRemark")), False, blnHighestLevel) - Michelle (24/12/2007) - To cater for single quote
                Dim strMsg As String = objTrac.ApproveInvoice(Common.Parse(dtr("InvNo")), Common.Parse(dtr("Vendor")), IIf(strType = "2", "FO", "FM"), dtr("FinRemark"), False, blnHighestLevel)

                If strMsg <> "" Then
                    Common.NetMsgbox(Me, strMsg)
                    'Exit Function
                Else
                    blnSend = False  'Michelle (CR0016) - To prevent send email again
                End If
                If blnHighestLevel Then
                    Dim dtItemInv As New DataTable
                    dtItemInv.Columns.Add("InvNo", Type.GetType("System.String"))
                    dtItemInv.Columns.Add("PoIndex", Type.GetType("System.String"))
                    dtItemInv.Columns.Add("Vendor", Type.GetType("System.String"))
                    dtItemInv.Columns.Add("FinRemark", Type.GetType("System.String"))
                    dtItemInv.Columns.Add("InvStatus", Type.GetType("System.Int32"))
                    dtItemInv.Columns.Add("Submitted", Type.GetType("System.String"))
                    dtItemInv.Columns.Add("AppDate", Type.GetType("System.String"))
                    dtItemInv.Columns.Add("PayTerm", Type.GetType("System.String"))
                    dtItemInv.Columns.Add("BillMethod", Type.GetType("System.String"))

                    Dim dtrInv As DataRow
                    Dim blnSendInv As Boolean

                    dtrInv = dtItemInv.NewRow
                    dtrInv("InvNo") = dtr("InvNo")

                    dtrInv("PoIndex") = dtr("PoIndex")
                    dtrInv("Vendor") = dtr("Vendor")
                    dtrInv("FinRemark") = dtr("FinRemark")
                    dtrInv("InvStatus") = 0
                    dtrInv("Submitted") = ""
                    dtrInv("AppDate") = ""
                    dtrInv("PayTerm") = ""
                    dtrInv("BillMethod") = dtr("BillMethod")

                    dtrInv("AppDate") = Date.Today.ToString
                    dtrInv("InvStatus") = invStatus.Approved
                    blnSend = False

                    dtItemInv.Rows.Add(dtrInv)
                    objTrac.updateInvoice(dtItemInv, blnSend)
                End If


                ' -= Start editing by Kelwin 03.04.2008 =-
                'objTrac.updateInvoice(dtItem, blnSend)

                Dim dtItemInv2 As New DataTable
                dtItemInv2.Columns.Add("InvNo", Type.GetType("System.String"))
                dtItemInv2.Columns.Add("PoIndex", Type.GetType("System.String"))
                dtItemInv2.Columns.Add("Vendor", Type.GetType("System.String"))
                dtItemInv2.Columns.Add("FinRemark", Type.GetType("System.String"))
                dtItemInv2.Columns.Add("InvStatus", Type.GetType("System.Int32"))
                dtItemInv2.Columns.Add("Submitted", Type.GetType("System.String"))
                dtItemInv2.Columns.Add("AppDate", Type.GetType("System.String"))
                dtItemInv2.Columns.Add("PayTerm", Type.GetType("System.String"))
                dtItemInv2.Columns.Add("BillMethod", Type.GetType("System.String"))

                Dim dtrInv2 As DataRow

                dtrInv2 = dtItemInv2.NewRow

                dtrInv2("InvNo") = dtr("InvNo")
                dtrInv2("PoIndex") = dtr("PoIndex")
                dtrInv2("Vendor") = dtr("Vendor")
                dtrInv2("FinRemark") = dtr("FinRemark")
                dtrInv2("InvStatus") = dtr("InvStatus")
                dtrInv2("Submitted") = dtr("Submitted")
                dtrInv2("AppDate") = dtr("AppDate")
                dtrInv2("PayTerm") = dtr("PayTerm")
                dtrInv2("BillMethod") = dtr("BillMethod")

                dtItemInv2.Rows.Add(dtrInv2)
                objTrac.updateInvoice(dtItemInv2, blnSend)

                ' -= End editing by Kelwin 03.04.2008 =-

            End If
            '----New Code Added by praveen on 23.08.2007------
            If strType = "1" Then 'save
                '  objTrac.updateAppRemark(dtr("InvIndex"), Common.Parse(dtr("FinRemark"))) - Michelle (24/12/2007) - To cater for single quote
                objTrac.updateAppRemark(dtr("InvIndex"), dtr("FinRemark"))
            End If
            '----Testing 
            'ElseIf strType = "1" Then 'save
            'objTrac.updateAppRemark(dtr("InvIndex"), Common.Parse(dtr("FinRemark")))
            'Else  ' Mark as Paid
            'objTrac.updateInvoice(dtItem, blnSend)
            'End If
            '----End Testing///End the code by Praveen on 23.08.2007
        Next

        Me.Session.Add("invList", strInv)
        If strInvFail <> "" Then
            Me.Session.Add("invListFail", strInvFail)
        End If

    End Function

    Private Function bindInvTrash()
        Dim dtItem As New DataTable
        dtItem.Columns.Add("InvNo", Type.GetType("System.String"))
        dtItem.Columns.Add("Vendor", Type.GetType("System.String"))
        dtItem.Columns.Add("FinRemark", Type.GetType("System.String"))

        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim dtr As DataRow
        Dim strInv As String = ""
        Dim strInvFail As String = ""
        For Each dgItem In dtgInvoice.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                dtr = dtItem.NewRow
                dtr("InvNo") = CType(dgItem.FindControl("lnkINVNo"), HyperLink).Text
                dtr("Vendor") = dgItem.Cells(EnumInv.icVenId).Text
                dtr("FinRemark") = CType(dgItem.FindControl("txtRemark"), TextBox).Text
                '---------NEw Changes in the If Condition becoz Error Getting while click on CmdDelete Buttonn
                '-------by Praveen on 23.08.2007
                ' If dgItem.Cells(EnumInv.icStatusCode).Text = invStatus.Paid Then

                ' Michelle (CR0005) - Comment off the checking
                ' If Not dgItem.Cells(EnumInv.icStatusCode).Text = invStatus.Paid Then
                '---End The Changes 
                strInv &= dtr("InvNo") & ","
                dtItem.Rows.Add(dtr)
                '  Else
                '     strInvFail &= dtr("InvNo") & ","
                ' End If
            End If
        Next
        objTrac.trashInv(dtItem)
        Me.Session.Add("invList", strInv)
        Me.Session.Add("invListFail", strInvFail)
    End Function

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        bindInv("1", False, False)
        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        bindgridINV()
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        ' FO approval
        bindInv("2", True, False)
        Response.Redirect(dDispatcher.direct("Tracking", "TrackConfirm.aspx", "pageid=" & strPageId & "&type=" & "S"))
    End Sub

    Private Sub cmdApprove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApprove.Click
        bindInv("1", False, False)
        bindInv("3", False, False)
        Response.Redirect(dDispatcher.direct("Tracking", "TrackConfirm.aspx", "pageid=" & strPageId & "&type=" & "A"))
    End Sub

    Private Sub cmdMark_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMark.Click
        bindInv("4", False, False)
        Response.Redirect(dDispatcher.direct("Tracking", "TrackConfirm.aspx", "pageid=" & strPageId & "&type=" & "P"))
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgInvoice.CurrentPageIndex = 0
        SortDateDesc()
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdDelete.Click
        bindInvTrash()
        Response.Redirect(dDispatcher.direct("Tracking", "TrackConfirm.aspx", "pageid=" & strPageId & "&type=" & "T"))
    End Sub

    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdPrint.Click
        Dim dtPrint As New DataTable
        Dim dr As DataRow
        Session("dtprint") = Nothing
        dtPrint.Columns.Add("INVNO", Type.GetType("System.String")) '//invoice no
        dtPrint.Columns.Add("vcomid", Type.GetType("System.String")) '//cendor id
        dtPrint.Columns.Add("type", Type.GetType("System.String")) '//type

        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim dtr As DataRow
        Dim strInv As String = ""
        For Each dgItem In dtgInvoice.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                dr = dtPrint.NewRow
                dr("INVNO") = CType(dgItem.FindControl("lnkINVNo"), HyperLink).Text
                dr("vcomid") = dgItem.Cells(EnumInv.icVenId).Text
                dr("type") = "INV"
                dtPrint.Rows.Add(dr)
            End If
        Next
        Session("dtprint") = dtPrint
        objTrac.printInv(dtPrint)

        Dim vbs As String
        vbs = vbs & "<script language=""javascript"">"
        'vbs = vbs & "Call MsgBox(""" & msg & """, " & Style & ", """ & title & """)"
        vbs = vbs & vbLf & "PrintWindow('" & strPageId & "');"
        vbs = vbs & "</script>"
        If (Not Me.IsStartupScriptRegistered("printwindow()")) Then
            Me.RegisterStartupScript("printwindow", vbs)
        End If
    End Sub

    Private Function writeInvoice()
        Dim ds As New DataSet
        Dim strIndex As String = ""
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim dtr As DataRow
        Dim strPath As String = "C:\InvTemp\"
        Dim strInv As String = ""
        For Each dgItem In dtgInvoice.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                strIndex &= dgItem.Cells(EnumInv.icIndex).Text & ","
            End If
        Next
        ds = objTrac.getInvoiceSave(strIndex.Substring(0, strIndex.Length - 1))
        Dim FILE_NAME As String = strPath & Format(Date.Now, "ddMMyyyyHHmmss") & ".txt" '"C:\PDF\test.htm"

        If Not Directory.Exists(strPath) Then
            Directory.CreateDirectory(strPath)
        End If

        Dim sw As New StringWriter
        Dim i, j As Integer

        For i = 0 To ds.Tables(0).Rows.Count - 1
            For j = 1 To ds.Tables(0).Columns.Count - 1
                If j = ds.Tables(0).Columns.Count - 1 Then
                    sw.WriteLine(Common.parseNull(ds.Tables(0).Rows(i)(j)))
                Else
                    sw.Write(Common.parseNull(ds.Tables(0).Rows(i)(j)) & "|")
                End If
            Next
        Next

        Dim fs As StreamWriter = New StreamWriter(FILE_NAME)
        fs.Write(sw.ToString)
        fs.Close()

        'Dim vbs As String
        'vbs = vbs & "<script language=""javascript"">"
        ''vbs = vbs & "Call MsgBox(""" & msg & """, " & Style & ", """ & title & """)"
        ''vbs = vbs & vbLf & "PopWindow('../FileDownload1.aspx?FilePath=" & FILE_NAME & "');"
        'Dim obj As New FileDownload1
        Session.Add("FilePath", FILE_NAME)
        Response.Redirect(dDispatcher.direct("Initial", "FileDownload1.aspx", "PageId=" & strPageId))
        'context.Items("FilePath") = FILE_NAME
        'Server.Transfer("../FileDownload1.aspx?PageId=" & strPageId)
        'vbs = vbs & vbLf & "PopWindow('../FileDownload1.aspx');"
        'vbs = vbs & "</script>"
        'If (Not Me.IsStartupScriptRegistered("popwindow")) Then
        '    Me.RegisterStartupScript("popwindow", vbs)
        'End If

        'openFile(FILE_NAME)

    End Function

    Private Function openFile(ByVal strFileName As String)
        Dim filename As String = Path.GetFileName(strFileName)
        Dim disHeader As String
        Response.Clear()
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition", "attachment; filename=""" & filename & """")
        Response.Flush()
        Response.WriteFile(strFileName)
        Response.End()
        'Dim adoApp As Object
        'adoApp = Server.CreateObject("aspSmartUpload.SmartUpload")
        'adoApp = Nothing
    End Function

    Private Sub cmdSaveInv_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdSaveInv.Click
        writeInvoice()
    End Sub

    Private Sub cmdPdf_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdPdf.Click
        Dim dtPrint As New DataTable
        Dim dr As DataRow
        Session("dtprint") = Nothing
        dtPrint.Columns.Add("INVNO", Type.GetType("System.String")) '//invoice no
        dtPrint.Columns.Add("vcomid", Type.GetType("System.String")) '//cendor id
        dtPrint.Columns.Add("type", Type.GetType("System.String")) '//type

        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim dtr As DataRow
        Dim strInv As String = ""
        For Each dgItem In dtgInvoice.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                dr = dtPrint.NewRow
                dr("INVNO") = CType(dgItem.FindControl("lnkINVNo"), HyperLink).Text
                dr("vcomid") = dgItem.Cells(EnumInv.icVenId).Text
                dr("type") = "INV"
                dtPrint.Rows.Add(dr)
            End If
        Next
        Session("dtprint") = dtPrint
        Dim vbs As String
        vbs = vbs & "<script language=""javascript"">"
        'vbs = vbs & "Call MsgBox(""" & msg & """, " & Style & ", """ & title & """)"
        vbs = vbs & vbLf & "PDFWindow('" & strPageId & "');"
        vbs = vbs & "</script>"
        'Dim rndKey As New Random
        If (Not Me.IsStartupScriptRegistered("pdfwindow")) Then
            Me.RegisterStartupScript("pdfwindow", vbs)
        End If
    End Sub
    '//Sort the record by Date Desc when user click button/Dropdownlist other than Datagrid header and paging
    Sub SortDateDesc()
        ViewState("SortExpression") = "IM_PAYMENT_DATE"
        ViewState("SortAscending") = "no"
        bindgridINV()
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        'Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn"" href=""InvoiceTrackingList.aspx?pageid=" & strPageId & "&folder=N" & "&status=1" & """><span>New Invoice</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn_selected"" href=""#""><span>Verified Invoice</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn"" href=""InvoicePaidTrackingList.aspx?pageid=" & strPageId & """><span>Paid Invoice</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "</ul><div></div></div>"
        Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId & "&folder=" & "N" & "&status=" & "1") & """><span>New Invoice</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""#""><span>Verified Invoice</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                             "</ul><div></div></div>"
    End Sub
End Class