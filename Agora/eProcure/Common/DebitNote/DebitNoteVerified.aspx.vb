
Imports System.IO
Imports AgoraLegacy
Imports eProcure.Component

Partial Public Class DebitNoteVerified
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim App As New AppGlobals
    Dim objTrac As New Tracking
    Dim dtPR As DataTable
    'Protected WithEvents cmdMassApp As System.Web.UI.WebControls.Button
    Dim objGlobal As New AppGlobals

    Public Enum EnumDN
        icChk
        icDnNo
        icDnDate
        icInvNo
        icVenName
        icCurrency
        icAmount
        icPayTerm
        icIndex
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents txtDnNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtInvNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdMark As System.Web.UI.WebControls.Button
    Protected WithEvents lblType As System.Web.UI.WebControls.Label
    Protected WithEvents dtgDebitNote As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRemark As System.Web.UI.WebControls.Label
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtDateFr As Global.System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateTo As Global.System.Web.UI.WebControls.TextBox
    Protected WithEvents vldsum As System.Web.UI.HtmlControls.HtmlGenericControl

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)

        Dim oriPageCount = Session("PageCount")
        Dim objDash As New Dashboard
        Session("PageCount") = 10
        Session("PageCount") = oriPageCount
        SetGridProperty(dtgDebitNote) 'Michelle (6/9/2007) - To rectify no. of records display per page according to user's setting

        If Not IsPostBack Then
            Dim strFO, strFM As String
            Dim blnFO, blnFM As Boolean
            Dim objUsers As New Users
            Dim objDB As New EAD.DBCom
            strFO = System.Enum.GetName(GetType(FixedRole), FixedRole.Finance_Officer)
            strFM = System.Enum.GetName(GetType(FixedRole), FixedRole.Finance_Manager)
            strFO = "'" & Replace(strFO, "_", " ") & "'"
            strFM = "'" & Replace(strFM, "_", " ") & "'"

            blnFO = objUsers.checkUserFixedRole(strFO)
            blnFM = objUsers.checkUserFixedRole(strFM)
            blnFM = objDash.ChkFMCanApprove()
            ViewState("blnFM") = blnFM
            ViewState("role") = getUserRole(blnFO, blnFM)

            GenerateTab()
            Select Case ViewState("role") 'FO & FM will access this screen 
                Case 2 ' is FO only
                    cmdMark.Visible = False
                    'ViewState("invfrom") = "paid"
                    'ViewState("status") = "4"
                    ViewState("button") = "none"
                Case 3 ' is FM only
                    'cmdMark.Visible = False
                    ViewState("status") = "3"
                    ViewState("invfrom") = "InvApproved"
                    ViewState("button") = "none"
                Case 4 ' is both
                    cmdMark.Visible = False
                    ViewState("status") = "3"
                    ViewState("button") = "none"
            End Select

            Session("strurl") = strCallFrom
            SortDateDesc_FirstLoad()
        End If

        cmdMark.Attributes.Add("onclick", "return checkAtLeastOneResetSummary('chkSelection','',0,1);")
        cmdSave.Attributes.Add("onClick", "return resetSummary(0,1);")
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

    Function BindGrid() As String

        If DateDiff(DateInterval.Month, CDate(txtDateFr.Text), CDate(txtDateTo.Text)) > 6 Then
            vldsum.InnerHtml = "<li>Date Range " & objGlobal.GetErrorMessage("00015") & " 6 months</li>"
            Return False
        Else
            vldsum.InnerHtml = ""
        End If

        Dim objDn As New DebitNote
        Dim ds As New DataSet
        ds = objDn.getDebitNoteTracking(txtDnNo.Text, "", "", ViewState("status"), txtInvNo.Text, txtDateFr.Text, txtDateTo.Text)
        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If ViewState("action") = "del" Then
            If dtgDebitNote.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgDebitNote.PageSize = 0 Then
                dtgDebitNote.CurrentPageIndex = dtgDebitNote.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgDebitNote, dvViewSample)
            dtgDebitNote.DataSource = dvViewSample
            hidSummary.Value = ""
            hidControl.Value = ""
            dtgDebitNote.DataBind()
        Else
            dtgDebitNote.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ' add for above checking
        ViewState("PageCount") = dtgDebitNote.PageCount
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgDebitNote.CurrentPageIndex = 0
        BindGrid()
    End Sub

    Private Sub dtgDebitNote_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDebitNote.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim lnkDnNo As HyperLink
            lnkDnNo = e.Item.FindControl("lnkDnNo")
            lnkDnNo.Text = dv("DNM_DN_NO")
            lnkDnNo.NavigateUrl = dDispatcher.direct("DebitNote", "DebitNoteDetails.aspx", "cmd=" & ViewState("button") & "&role=" & ViewState("role") & "&Frm=" & "DebitNoteVerified" & "&pageid=" & strPageId & "&DNNO=" & Common.parseNull(dv("DNM_DN_NO")) & "&vcomid=" & Common.parseNull(dv("DNM_DN_S_COY_ID")) & "&status=" & Common.parseNull(dv("DNM_DN_STATUS")))

            e.Item.Cells(EnumDN.icDnDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("DNM_DN_DATE"))

            Dim lnkInvNo As HyperLink
            lnkInvNo = e.Item.FindControl("lnkInvNo")
            lnkInvNo.Text = dv("DNM_INV_NO")
            lnkInvNo.NavigateUrl = dDispatcher.direct("Tracking", "InvoiceDetails.aspx", "Frm=DebitNoteVerified" & "&pageid=" & strPageId & "&role=" & ViewState("role") & "&INVNO=" & Common.parseNull(dv("DNM_INV_NO")) & "&vcomid=" & Common.parseNull(dv("DNM_DN_S_COY_ID")) & "&folder=P&status=4")

            If Not IsDBNull(dv("AMOUNT")) Then
                e.Item.Cells(EnumDN.icAmount).Text = Format(CDbl(dv("AMOUNT")), "###,##0.00")
            End If

            Dim cboPay As DropDownList
            cboPay = e.Item.FindControl("cboPay")
            Dim lblPay As Label
            lblPay = e.Item.FindControl("lblPay")

            objGlobal.FillCodeTable(cboPay, CodeTable.PaymentMethod)
            cboPay.Items.RemoveAt(0)
            If Not IsDBNull(dv("DNM_PAYMENT_TERM")) Then
                Common.SelDdl(Common.parseNull(dv("DNM_PAYMENT_TERM")), cboPay, False, True)
            Else
                Common.SelDdl(Common.parseNull(dv("IM_PAYMENT_TERM")), cboPay, False, True)
            End If
            lblPay.Text = cboPay.SelectedItem.Text

            'If hidSummary.Value = "" Then
            '    hidSummary.Value = "Finance Remarks-" & txtRemark.ClientID
            'Else
            '    hidSummary.Value &= ",Finance Remarks-" & txtRemark.ClientID
            'End If

            'Dim cboPay As DropDownList
            'cboPay = e.Item.FindControl("cboPay")


        End If
    End Sub

    Private Sub dtgDebitNote_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDebitNote.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgDebitNote, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dtgDebitNote_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgDebitNote.PageIndexChanged
        dtgDebitNote.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Private Sub payDebitNote(ByVal strAction As String)
        Dim strDnMarkList As String = ""
        Dim objDn As New DebitNote
        Dim dtItem As New DataTable
        dtItem.Columns.Add("DnIndex", Type.GetType("System.Int32"))
        dtItem.Columns.Add("PayTerm", Type.GetType("System.String"))

        Dim dtr As DataRow
        Dim chk As CheckBox

        For Each dgItem As DataGridItem In dtgDebitNote.Items
            chk = dgItem.FindControl("chkSelection")
            Dim cboPay As DropDownList
            cboPay = dgItem.FindControl("cboPay")

            If chk.Checked Then
                If strDnMarkList = "" Then
                    'Issue 7480 - CH - 24 Mar 2015 (No.58)
                    'strDnMarkList &= dgItem.Cells(EnumDN.icDnNo).Text
                    strDnMarkList &= CType(dgItem.FindControl("lnkDnNo"), HyperLink).Text
                Else
                    'strDnMarkList &= dgItem.Cells(EnumDN.icDnNo).Text
                    strDnMarkList &= "," & CType(dgItem.FindControl("lnkDnNo"), HyperLink).Text
                End If
                dtr = dtItem.NewRow
                dtr("DnIndex") = dgItem.Cells(EnumDN.icIndex).Text
                dtr("PayTerm") = cboPay.SelectedItem.Text
                dtItem.Rows.Add(dtr)
            End If
        Next

        If strAction = "pay" Then
            objDn.updateDN(dtItem, True)
            Common.NetMsgbox(Me, "Debit Note Number(s) " & strDnMarkList & " has/have been marked as paid.", MsgBoxStyle.Information)
        Else
            objDn.updateDN(dtItem, False)
            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        End If

        BindGrid()
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgDebitNote.CurrentPageIndex = 0
        SortDateDesc()
    End Sub

    Sub SortDateDesc()
        ViewState("SortExpression") = "DNM_PAYMENT_DATE"
        ViewState("SortAscending") = "no"
        BindGrid()
    End Sub

    Sub SortDateDesc_FirstLoad()
        txtDateTo.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, DateTime.Today)
        txtDateFr.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, DateTime.Today.AddMonths(-6)) '.ToString
        ViewState("SortExpression") = "DNM_PAYMENT_DATE"
        ViewState("SortAscending") = "no"
        BindGrid()
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        If ViewState("role") = "3" Then  'FM
            Session("w_DnVer_tabs") = "<div class=""t_entity""><ul>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=" & "N" & "&status=" & "1") & """><span>Verified Debit Note</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteVerified.aspx", "pageid=" & strPageId) & """><span>Approved Debit Note</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "</ul><div></div></div>"
        ElseIf ViewState("role") = "2" Then  ' FO
            Session("w_DnVer_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=" & "N" & "&status=" & "1") & """><span>New Debit Note</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteVerified.aspx", "pageid=" & strPageId) & """><span>Verified Debit Note</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "</ul><div></div></div>"

        ElseIf ViewState("role") = "4" Then  ' FO & FM
            If ViewState("blnFM") Then 'ie' FM can do approval
                Session("w_DnVer_tabs") = "<div class=""t_entity""><ul>" & _
                                             "<li><div class=""space""></div></li>" & _
                                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=" & "N" & "&status=" & "1") & """><span>New Debit Note</span></a></li>" & _
                                             "<li><div class=""space""></div></li>" & _
                                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=S&status=1") & """><span>Verified Debit Note</span></a></li>" & _
                                             "<li><div class=""space""></div></li>" & _
                                             "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteVerified.aspx", "pageid=" & strPageId) & """><span>Approved Debit Note</span></a></li>" & _
                                             "<li><div class=""space""></div></li>" & _
                                             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                                             "<li><div class=""space""></div></li>" & _
                                             "</ul><div></div></div>"
            Else
                Session("w_DnVer_tabs") = "<div class=""t_entity""><ul>" & _
                                        "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=" & "N" & "&status=" & "1") & """><span>New Debit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteVerified.aspx", "pageid=" & strPageId) & """><span>Verified Debit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "</ul><div></div></div>"
            End If
        End If
    End Sub

    Private Sub cmdMark_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdMark.Click
        payDebitNote("pay")
    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        payDebitNote("save")
    End Sub
End Class