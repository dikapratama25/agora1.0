
Imports System.IO
Imports AgoraLegacy
Imports eProcure.Component

Public Class IPPApprovalList
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Dim objTrac As New Tracking
    Protected WithEvents lblRemark As System.Web.UI.WebControls.Label
    Protected WithEvents lblBillInvoiceApprBy As System.Web.UI.WebControls.Label
    Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden

    Dim objGlobal As New AppGlobals
    Dim strInvAppr2 As String
    Dim IPPTimeFr1 As String = System.Configuration.ConfigurationManager.AppSettings.Get("IPPTimeFr1")
    Dim IPPTimeTo1 As String = System.Configuration.ConfigurationManager.AppSettings.Get("IPPTimeTo1")
    Dim IPPTimeFr2 As String = System.Configuration.ConfigurationManager.AppSettings.Get("IPPTimeFr2")
    Dim IPPTimeTo2 As String = System.Configuration.ConfigurationManager.AppSettings.Get("IPPTimeTo2")
    Dim TimeNow As String = DateTime.Now.ToLocalTime.ToString("HH:mm")

    Public Enum EnumApprIPP
        ippChk = 0
        ippDocNo = 1
        ippDocType = 2
        ippDate = 3
        ippVendor = 4
        ippCurrency = 5
        ippPymtAmt = 6
        ippPymtMeth = 7
        ippIndex = 8
        ippBillInvApprBy = 9
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub IPPApprovalList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgIPP)
        PopulateTypeAhead()
        GenerateTab()
        If Not IsPostBack Then
            Dim strFO, strFM As String
            Dim blnFO, blnFM As Boolean
            Dim objUsers As New Users
            Dim blnIPPAO As Boolean
            strFO = System.Enum.GetName(GetType(FixedRole), FixedRole.Finance_Officer)
            strFM = System.Enum.GetName(GetType(FixedRole), FixedRole.Finance_Manager)
            strFO = "'" & Replace(strFO, "_", " ") & "'"
            strFM = "'" & Replace(strFM, "_", " ") & "'"

            blnFO = objUsers.checkUserFixedRole(strFO)
            blnFM = objUsers.checkUserFixedRole(strFM)
            blnIPPAO = objUsers.checkUserFixedRole("'IPP Approving Officer'")
            ViewState("role") = getUserRole(blnFO, blnFM, blnIPPAO)
            bindgrid()
        End If

    End Sub
    Private Function getUserRole(ByVal blnFo As Boolean, ByVal blnFM As Boolean, ByVal blnAO As Boolean) As Integer
        If blnAO = True Then
            getUserRole = 5
        ElseIf blnFo = False And blnFM = True Then
            getUserRole = 3
        ElseIf blnFo = True And blnFM = False Then
            getUserRole = 2
        Else
            getUserRole = 1
        End If
    End Function
    Function bindgrid() As String

        Dim ds As New DataSet
        Dim objippmain As New IPPMain
        'Zulham 13072018 - PAMB
        ds = objippmain.getApprovalList(txtDocNo.Text.Replace("\", "\\\\"), ddlDocType.SelectedItem.Value, txtDocStartDate.Value, txtDocEndDate.Value, txtVendor.Value)

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        intPageRecordCnt = ds.Tables(0).Rows.Count
        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgIPP, dvViewSample)
            dtgIPP.DataSource = dvViewSample
            dtgIPP.DataBind()
        Else
            dtgIPP.DataSource = dvViewSample
            dtgIPP.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ViewState("intPageRecordCnt") = dtgIPP.PageCount
    End Function
    Private Sub dtgIPP_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPP.ItemCreated

        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgIPP, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub
    Private Sub dtgIPP_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPP.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Select Case e.Item.Cells(EnumApprIPP.ippDocType).Text
                Case "INV"
                    e.Item.Cells(EnumApprIPP.ippDocType).Text = "Invoice"
                Case "BILL"
                    e.Item.Cells(EnumApprIPP.ippDocType).Text = "Bill"
                Case "CN"
                    e.Item.Cells(EnumApprIPP.ippDocType).Text = "Credit Note"
                Case "DN"
                    e.Item.Cells(EnumApprIPP.ippDocType).Text = "Debit Note"
            End Select

            'format date
            e.Item.Cells(EnumApprIPP.ippDate).Text = e.Item.Cells(EnumApprIPP.ippDate).Text.Substring(0, 10)

            'Zulham 21112018
            Select Case e.Item.Cells(EnumApprIPP.ippPymtMeth).Text
                Case "IBG"
                    e.Item.Cells(EnumApprIPP.ippPymtMeth).Text = "LOCAL BANK TRANSFER-(RM)"
                Case "TT"
                    e.Item.Cells(EnumApprIPP.ippPymtMeth).Text = "TELEGRAPHIC TRANSFER-(FOREIGN CURRENCY)"
                Case "BC"
                    e.Item.Cells(EnumApprIPP.ippPymtMeth).Text = "CHEQUE-(RM)"
                Case "BD"
                    e.Item.Cells(EnumApprIPP.ippPymtMeth).Text = "BANK DRAFT-(FOREIGN CURRENCY)"
                Case "CO"
                    e.Item.Cells(EnumApprIPP.ippPymtMeth).Text = "CASHIER'S ORDER-(RM)"
            End Select

            Dim lnkDocNo As HyperLink
            lnkDocNo = e.Item.FindControl("lnkINVNo")
            lnkDocNo.Text = dv("IM_INVOICE_NO")
            'Zulham 12072018 - PAMB
            lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "IPPAOApprovalDetail.aspx", "frm=ApprList&index=" & dv("IM_INVOICE_INDEX") & "&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO")) & "&pageid=")

        End If
    End Sub
    Public Sub dtgIPP_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgIPP.PageIndexChanged
        dtgIPP.CurrentPageIndex = e.NewPageIndex
        bindgrid()
    End Sub
    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgIPP.SortCommand
        Grid_SortCommand(sender, e)
        dtgIPP.CurrentPageIndex = 0
        bindgrid()
    End Sub
    Public Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        bindgrid()
    End Sub
    Public Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        txtDocStartDate.Value = ""
        txtDocEndDate.Value = ""
        ddlDocType.SelectedIndex = 0
        txtDocNo.Text = ""
        txtVendor.Value = ""
    End Sub
    Private Function bindInv()
        Dim dtItem As New DataTable
        dtItem.Columns.Add("DocType", Type.GetType("System.String"))
        dtItem.Columns.Add("InvIndex", Type.GetType("System.Int32"))
        dtItem.Columns.Add("DocNo", Type.GetType("System.String"))
        dtItem.Columns.Add("Vendor", Type.GetType("System.String"))
        dtItem.Columns.Add("Currency", Type.GetType("System.String"))
        dtItem.Columns.Add("AppDate", Type.GetType("System.String"))
        dtItem.Columns.Add("Amount", Type.GetType("System.String"))

        Dim dtr As DataRow
        Dim strInv As String = ""
        Dim strInvFail As String = ""

        Dim chk As CheckBox
        Dim bln As Boolean

        For Each dgItem As DataGridItem In dtgIPP.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                'dtr = dtItem.NewRow
                'dtr("DocType") = dgItem.Cells(EnumApprIPP.ippDocType).Text
                bln = True

                'Zulham 05/01/2016 - IPP STAGE 4 Phase 2
                'check for invoices status that are tied to CN/DN
                Dim objDb2 As New EAD.DBCom
                Dim objippmain2 As New IPPMain
                Dim invStatus = objDb2.GetVal("select im_invoice_status from invoice_mstr where im_invoice_index = '" & dgItem.Cells(8).Text & "'")
                Dim vendID = objDb2.GetVal("select im_s_Coy_id from invoice_mstr where im_invoice_index = '" & dgItem.Cells(8).Text & "'")
                Dim docNo = objDb2.GetVal("select im_invoice_no from invoice_mstr where im_invoice_index = '" & dgItem.Cells(8).Text & "'")

                If objippmain2.checkStatus(docNo.ToString.Trim, vendID, invStatus) = False Then
                    Dim strMsg = "Credit Note/Debit Note cannot precede the invoices it's tied to."
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                    Exit Function
                End If

                If bln Then
                    'If dgItem.Cells(EnumApprIPP.ippDocType).Text = "IPP" Then
                    ' build remark 
                    Dim strRemark As String = ""
                    Dim strBillInvoiceApprBy As String = ""

                    'Zulham 14112018
                    Session("Index") = dgItem.Cells(EnumApprIPP.ippIndex).Text

                    'Zulham 14112018
                    strRemark = CType(dgItem.FindControl("txtRemarks"), TextBox).Text
                    If strRemark = "" Then
                        strRemark = "Submitted"
                    End If
                    'Zulham 10102018 - PAMB
                    'If strRemark = "" Then
                    '    vldsum.InnerHtml = "<li>Remarks " & objGlobal.GetErrorMessage("00001") & "</li>"
                    '    Exit Function
                    'End If

                    If strRemark <> "" Then
                        strRemark = FormatAORemark("verify") & strRemark
                    End If

                    'strBillInvoiceApprBy = CType(dgItem.FindControl("txtBillInvoiceApprBy"), TextBox).Text

                    'If strBillInvoiceApprBy = "" Then
                    '    vldsum.InnerHtml = "<li>Bill/Invoice Approved by " & objGlobal.GetErrorMessage("00001") & "</li>"
                    '    Exit Function
                    'End If
                    'if is TT pop up error message 

                    Dim objippmain As New IPPMain
                    Dim intApprGrpIndex As String
                    Dim objDb As New EAD.DBCom
                    intApprGrpIndex = objDb.GetVal("SELECT DISTINCT FA_APPROVAL_GRP_INDEX FROM finance_approval WHERE FA_INVOICE_INDEX = '" & dgItem.Cells(EnumApprIPP.ippIndex).Text & "' ")

                    If objippmain.checkDept() = False Then
                        strInvFail = objGlobal.GetErrorMessage("00046")
                        Exit For
                    End If
                    'If strType = "3" Or strType = "2" Then
                    Dim objDoc As New IPPMain
                    If chk.Checked = True Then
                        'If dgItem.Cells(EnumApprIPP.ippPymtMeth).Text = "TT" Then
                        '    If objippmain.exchangeRate(dgItem.Cells(EnumApprIPP.ippIndex).Text) Then
                        '        strInvFail = "Invoice " & CType(dgItem.FindControl("lnkINVNo"), HyperLink).Text & " required exchange rate."
                        '        Exit For
                        '    End If
                        'End If

                        If ((CDate(TimeNow) >= CDate(IPPTimeFr1) And CDate(TimeNow) <= CDate(IPPTimeTo1)) Or (CDate(TimeNow) >= CDate(IPPTimeFr2) And CDate(TimeNow) <= CDate(IPPTimeTo2))) And ViewState("role") = "3" Then
                            strInvFail = objGlobal.GetErrorMessage("00258") & CType(dgItem.FindControl("lnkINVNo"), HyperLink).Text
                            Exit For
                        End If

                        If objDoc.CheckBankCode(dgItem.Cells(EnumApprIPP.ippIndex).Text) Then
                            strInvFail = objGlobal.GetErrorMessage("00323") & dgItem.Cells(EnumApprIPP.ippDocNo).Text
                            Exit For
                        End If

                        'Zulham 13082018 - PAMB
                        'approval limit < invoice total = invoice goes to the next person 
                        Dim blnCanApprove = objDoc.canApprove(CDec(dgItem.Cells(EnumApprIPP.ippPymtAmt).Text))

                        If objDoc.ApproveIPPDoc(dgItem.Cells(EnumApprIPP.ippIndex).Text, strRemark, Session("Relief"), ViewState("role"), dgItem.Cells(EnumApprIPP.ippPymtMeth).Text, , dgItem.Cells(EnumApprIPP.ippDocType).Text, IPPTimeFr1, intApprGrpIndex, strBillInvoiceApprBy, blnCanApprove:=blnCanApprove) = True Then

                            'Zulham 03122018
                            If Not blnCanApprove Then
                                objDoc.sendMailToIPPOfficer(Session("Index"), "6", "verify")
                            End If

                            strInv &= CType(dgItem.FindControl("lnkINVNo"), HyperLink).Text & ","
                        End If

                    End If
                    'End If
                    'If strType = "1" Then 'save
                    '    objTrac.updateAppRemark(dgItem.Cells(EnumApprIPP.ippIndex).Text, strRemark)
                    'End If
                    'End If
                End If
            End If
        Next

        Me.Session.Add("invList", strInv)
        If strInvFail <> "" Then
            Me.Session.Add("invListFail", strInvFail)
        End If

    End Function

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_IPP_tabs") = "<div class=""t_entity""><ul>" &
        "<li><div class=""space""></div></li>" &
                 "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPP", "IPPApprovalList.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" &
                 "<li><div class=""space""></div></li>" &
                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "IPPApprovalList_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" &
                 "<li><div class=""space""></div></li>" &
                 "</ul><div></div></div>"


    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim strTmp As String = ""
        Dim strMsg As String = ""
        vldsum.InnerHtml = ""

        bindInv()

        If Len(Session("invList")) > 0 Then
            strTmp = Mid(Session("invList"), 1, Len(Session("invList")) - 1)
            strMsg = objGlobal.GetErrorMessage("00043") & " " & strTmp & " " & objGlobal.GetErrorMessage("00044")
            Common.NetMsgbox(Me, strMsg, dDispatcher.direct("IPP", "IPPApprovalList.aspx", "Frm=" & "IPPApprovalList" & "&pageid=" & strPageId))
            If Len(Session("invListFail")) > 0 Then
                Common.NetMsgbox(Me, Session("invListFail"), MsgBoxStyle.Information)
            End If
        ElseIf Len(Session("invListFail")) > 0 Then
            Common.NetMsgbox(Me, Session("invListFail"), MsgBoxStyle.Information)
        End If
        Session("invListFail") = Nothing
        Session("invList") = Nothing
        'cmdSubmit.Visible = False
        bindgrid()
    End Sub
    Sub PopulateTypeAhead()
        Dim typeahead As String
        Dim i, count As Integer
        Dim content, content2 As String
        Dim strCompID As String
        Dim vtypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=IPPCOY")


        content &= "$(""#txtVendor"").autocomplete(""" & vtypeahead & """, {" & vbCrLf &
        "width: 200," & vbCrLf &
        "scroll: true," & vbCrLf &
        "selectFirst: false" & vbCrLf &
        "});" & vbCrLf


        typeahead = "<script language=""javascript"">" & vbCrLf &
      "<!--" & vbCrLf &
        "$(document).ready(function(){" & vbCrLf &
        content & vbCrLf &
        "});" & vbCrLf &
        "-->" & vbCrLf &
        "</script>"


        Session("typeahead") = typeahead
    End Sub
    Private Function FormatAORemark(ByVal strAction As String) As String
        Dim strRemark, strTempRemark, strUserName As String
        'Zulham 13082018 - PAMB
        Dim objDb As New EAD.DBCom
        If Session("relief") Then
            Dim objUsers As New Users
            Dim objUser As New User
            objUsers.GetUserDetail(Session("AO"), Session("CompanyId"), objUser)
            strUserName = objUser.Name
            objUsers = Nothing
            objUser = Nothing
            strTempRemark = "(On Behalf of " & strUserName & ") "
        Else
            strTempRemark = ""
        End If

        'Zulham 13082018 -PAMB
        'Check for AO seq
        Dim seq = 0
        seq = objDb.GetVal("Select DISTINCT aga_seq 
                            From approval_grp_ao, finance_approval
                            Where FA_INVOICE_INDEX = '" & Session("Index") & "' 
                            And FA_APPROVAL_GRP_INDEX = aga_grp_index
                            And aga_ao = '" & Session("UserID") & "' ")

        Select Case strAction
            Case "approve"
                strRemark = "Approved " & strTempRemark & ": "
            Case "reject"
                strRemark = "Rejected" & strTempRemark & ": "
            Case "verify"
                'Zulham 13082018 -PAMB
                If seq = 1 Then
                    strRemark = "Verified" & strTempRemark & ": "
                Else
                    strRemark = "Dept Approval" & strTempRemark & ": "
                End If
        End Select
        Return strRemark
    End Function
End Class