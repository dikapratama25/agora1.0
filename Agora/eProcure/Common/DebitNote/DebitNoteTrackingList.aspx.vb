
Imports System.IO
Imports AgoraLegacy
Imports eProcure.Component

Public Class DebitNoteTrackingList
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objTrac As New Tracking
    Dim objDn As New DebitNote
    Dim dtPR As DataTable
    Dim dtDocMatch As DataTable
    Dim dtDocMatchWDO As DataTable
    Dim objGlobal As New AppGlobals
    Dim strInvApprMode As String

    Public Enum EnumDN
        icDnNo
        icDnDate
        icInvNo
        icVendor
        icCurrency
        icAmount
        icDnIndex
        icInvIndex
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSearch2 As System.Web.UI.WebControls.Button
    Protected WithEvents ddlCurr As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtDocNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVendorName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtInvNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateFr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblType As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRemark As System.Web.UI.WebControls.Label
    Protected WithEvents lblField As System.Web.UI.WebControls.Label
    Protected WithEvents dtgDebitNote As System.Web.UI.WebControls.DataGrid
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents tr1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr2 As System.Web.UI.HtmlControls.HtmlTableRow
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub DebitNoteTrackingList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        Dim oriPageCount = Session("PageCount")
        Dim objDash As New Dashboard
        Session("PageCount") = 10

        SetGridProperty(dtgDebitNote)
        Session("PageCount") = oriPageCount
        SetGridProperty(dtgDebitNote) 'To rectify no. of records display per page according to user's setting

        'Dim objCompany As New SSO.Component.Companies
        'strInvApprMode = objCompany.GetInvApprMode(Session("CompanyId"))

        If Not IsPostBack Then

            Dim strFO, strFM As String
            Dim blnFO, blnFM As Boolean
            Dim objUsers As New Users
            Dim objDB As New EAD.DBCom

            'Check user whether he/she is Finance Officer or Finance Manager or both
            strFO = System.Enum.GetName(GetType(FixedRole), FixedRole.Finance_Officer)
            strFM = System.Enum.GetName(GetType(FixedRole), FixedRole.Finance_Manager)
            strFO = "'" & Replace(strFO, "_", " ") & "'"
            strFM = "'" & Replace(strFM, "_", " ") & "'"
            blnFO = objUsers.checkUserFixedRole(strFO)
            blnFM = objUsers.checkUserFixedRole(strFM)
            ViewState("role") = getUserRole(blnFO, blnFM)

            'Load Currency
            objGlobal.FillCodeTable(ddlCurr, CodeTable.Currency)

            'Generate Tabs
            GenerateTab()

            Select Case ViewState("role")
                Case 2 'FO only
                    If Request.QueryString("action") = "S" Then
                        changeScreen(1)
                        ViewState("button") = "none"
                        ViewState("status") = "2,3"
                        'ViewState("status") = "2,3,4"
                        ViewState("dnfrom") = "sent"
                    ElseIf Request.QueryString("action") = "API" Then
                        changeScreen(1)
                        ViewState("button") = "none"
                        'ViewState("status") = "2,3,4"
                        ViewState("status") = "2,3"
                        ViewState("dnfrom") = "app/rej"
                    Else
                        changeScreen(2)
                        ViewState("button") = "verify"
                        ViewState("status") = "1"
                        ViewState("dnfrom") = "new"
                    End If

                Case 3 'FM only
                    changeScreen(2)
                    ViewState("button") = "approve"
                    ViewState("status") = "2"
                    ViewState("dnfrom") = "new"

                Case 4 'Both
                    If Request.QueryString("action") = "S" Then
                        changeScreen(1)
                        ViewState("button") = "2,3"
                        ViewState("status") = "2,3"
                        'ViewState("button") = "2,3,4"
                        'ViewState("status") = "2,3,4"
                        ViewState("dnfrom") = "sent"
                    ElseIf Request.QueryString("action") = "API" Then
                        changeScreen(1)
                        ViewState("button") = "2,3"
                        ViewState("status") = "2,3"
                        'ViewState("button") = "2,3,4"
                        'ViewState("status") = "2,3,4"
                        ViewState("dnfrom") = "app/rej"
                    Else
                        changeScreen(2)
                        ViewState("button") = "2,3,4"
                        ViewState("status") = "1"
                        ViewState("dnfrom") = "new"
                    End If

            End Select

            Session("role") = ViewState("role")
            Session("strurl") = strCallFrom
            SortDateDesc()
        End If

    End Sub

    Private Sub changeScreen(ByVal intType As Integer)
        If intType = 1 Then
            lblField.Text = "Invoice No."
            txtVendorName.Style("display") = "none"
            txtInvNo.Style("display") = ""
            tr1.Style("display") = "none"
            tr2.Style("display") = ""
        Else
            lblField.Text = "Vendor Name"
            txtVendorName.Style("display") = ""
            txtInvNo.Style("display") = "none"
            tr1.Style("display") = ""
            tr2.Style("display") = "none"
        End If
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
        Dim ds As New DataSet

        If DateDiff(DateInterval.Month, CDate(txtDateFr.Text), CDate(txtDateTo.Text)) > 6 Then
            Common.NetMsgbox(Me, "Date Range " & objGlobal.GetErrorMessage("00015") & " 6 months")
            'vldsum.InnerHtml = "<li>Date Range " & objGlobal.GetErrorMessage("00015") & " 6 months</li>"
            Return False
        End If

        If Request.QueryString("action") = "S" Or Request.QueryString("action") = "API" Then
            ds = objDn.getDebitNoteTracking(txtDocNo.Text, "", "", ViewState("status"), txtInvNo.Text, txtDateFr.Text, txtDateTo.Text)
        Else
            ds = objDn.getDebitNoteTracking(txtDocNo.Text, txtVendorName.Text, ddlCurr.SelectedValue, ViewState("status"))
        End If
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

            'Dim hidCode As HtmlInputHidden
            'hidCode = e.Item.FindControl("hidCode")
            'hidCode.Value = dv("IM_INVOICE_NO")

            'Dim txtQ As TextBox
            'txtQ = e.Item.FindControl("txtQ")

            'If hidControl.Value = "" Then
            '    hidControl.Value = hidCode.ClientID & "-" & txtQ.ClientID
            'Else
            '    hidControl.Value &= "," & hidCode.ClientID & "-" & txtQ.ClientID
            'End If

            'Dim chk As CheckBox
            'chk = e.Item.Cells(0).FindControl("chkSelection")
            'chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            'Dim drMatch() As DataRow
            'Dim drRow As DataRow
            'Dim drDocMatchRow As DataRow

            'Dim strTemp As String = ""
            'Dim strURL As String
            'Dim intPoIndex As Long
            'Dim intLoop, intCnt As Integer
            'Dim objDB As New EAD.DBCom

            'If cmdSubmit.Visible = True Then
            '    ViewState("cmd") = 1
            'ElseIf cmdApprove.Visible = True Then
            '    ViewState("cmd") = 2
            'End If

            Dim lnkDnNo As HyperLink
            lnkDnNo = e.Item.FindControl("lnkDnNo")
            lnkDnNo.Text = dv("DNM_DN_NO")
            lnkDnNo.NavigateUrl = dDispatcher.direct("DebitNote", "DebitNoteDetails.aspx", "cmd=" & ViewState("button") & "&role=" & ViewState("role") & "&Frm=" & "DebitNoteTrackingList" & "&pageid=" & strPageId & "&DNNO=" & Common.parseNull(dv("DNM_DN_NO")) & "&vcomid=" & Common.parseNull(dv("DNM_DN_S_COY_ID")) & "&status=" & Common.parseNull(dv("DNM_DN_STATUS")))

            e.Item.Cells(EnumDN.icDnDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("DNM_DN_DATE"))

            Dim lnkInvNo As HyperLink
            lnkInvNo = e.Item.FindControl("lnkInvNo")
            lnkInvNo.Text = dv("DNM_INV_NO")
            'lnkInvNo.NavigateUrl = dDispatcher.direct("Tracking", "InvoiceDetails.aspx", "cmd=" & ViewState("cmd") & "&role=" & ViewState("role") & "&Frm=" & "DebitNoteTrackingList" & "&pageid=" & strPageId & "&DNNO=" & Common.parseNull(dv("DNM_DN_NO")) & "&vcomid=" & Common.parseNull(dv("DNM_DN_S_COY_ID")) & "&folder=API&status=" & Common.parseNull(dv("DNM_DN_STATUS")))
            lnkInvNo.NavigateUrl = dDispatcher.direct("Tracking", "InvoiceDetails.aspx", "Frm=DebitNoteTrackingList" & "&pageid=" & strPageId & "&role=" & ViewState("role") & "&INVNO=" & Common.parseNull(dv("DNM_INV_NO")) & "&vcomid=" & Common.parseNull(dv("DNM_DN_S_COY_ID")) & "&folder=P&status=4")

            If Not IsDBNull(dv("AMOUNT")) Then
                e.Item.Cells(EnumDN.icAmount).Text = Format(CDbl(dv("AMOUNT")), "###,##0.00")
            End If

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
    End Sub

    Private Sub dtgInvoice_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgDebitNote.PageIndexChanged
        dtgDebitNote.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgDebitNote.CurrentPageIndex = 0
        SortDateDesc2()
    End Sub

    Private Sub cmdSearch2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch2.Click
        dtgDebitNote.CurrentPageIndex = 0
        SortDateDesc2()
    End Sub

    '//Sort the record by Date Desc when user click button/Dropdownlist other than Datagrid header and paging
    Sub SortDateDesc()
        ViewState("SortExpression") = "DNM_DN_DATE"
        ViewState("SortAscending") = "no"
        txtDateTo.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, DateTime.Today)
        txtDateFr.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, DateTime.Today.AddMonths(-6)) '.ToString
        BindGrid()
    End Sub

    Sub SortDateDesc2()
        ViewState("SortExpression") = "DNM_DN_DATE"
        ViewState("SortAscending") = "no"
        BindGrid()
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        If ViewState("role") = "2" Then
            If Request.QueryString("action") = "S" Then
                Session("w_DnTracking_tabs") = "<div class=""t_entity""><ul>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId) & """><span>New Debit Note</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=S") & """><span>Verified Debit Note</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "</ul><div></div></div>"
            Else
                Session("w_DnTracking_tabs") = "<div class=""t_entity""><ul>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId) & """><span>New Debit Note</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=S") & """><span>Verified Debit Note</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "</ul><div></div></div>"

            End If
        ElseIf ViewState("role") = "3" Then
            Session("w_DnTracking_tabs") = "<div class=""t_entity""><ul>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn_selected"" href=""#""><span>Verified Debit Note</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteVerified.aspx", "pageid=" & strPageId) & """><span>Approved Debit Note</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "</ul><div></div></div>"

        ElseIf ViewState("role") = "4" And Request.QueryString("folder") = "S" Then
            If ViewState("blnFM") Then 'FM can do approval
                Session("w_DnTracking_tabs") = "<div class=""t_entity""><ul>" & _
                        "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId) & """><span>New Debit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=S") & """><span>Verified Debit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteVerified.aspx", "pageid=" & strPageId) & """><span>Approved Debit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "</ul><div></div></div>"
            Else 'FM cannot do approval, equivalent to FO role
                Session("w_DnTracking_tabs") = "<div class=""t_entity""><ul>" & _
                                    "<li><div class=""space""></div></li>" & _
                                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId) & """><span>New Debit Note</span></a></li>" & _
                                                        "<li><div class=""space""></div></li>" & _
                                                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=S") & """><span>Verified Debit Note</span></a></li>" & _
                                                        "<li><div class=""space""></div></li>" & _
                                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                                                        "<li><div class=""space""></div></li>" & _
                                                        "</ul><div></div></div>"
            End If
        ElseIf ViewState("role") = "4" Then
            If ViewState("blnFM") Then 'FM can do approval
                Session("w_DnTracking_tabs") = "<div class=""t_entity""><ul>" & _
                          "<li><div class=""space""></div></li>" & _
                                              "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId) & """><span>New Debit Note</span></a></li>" & _
                                              "<li><div class=""space""></div></li>" & _
                                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=S") & """><span>Verified Debit Note</span></a></li>" & _
                                              "<li><div class=""space""></div></li>" & _
                                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteVerified.aspx", "pageid=" & strPageId) & """><span>Approved Debit Note</span></a></li>" & _
                                              "<li><div class=""space""></div></li>" & _
                                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                                              "<li><div class=""space""></div></li>" & _
                                              "</ul><div></div></div>"

            Else 'FM cannot do approval, equivalent to FO role
                Session("w_DnTracking_tabs") = "<div class=""t_entity""><ul>" & _
                        "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId) & """><span>New Debit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNoteTrackingList.aspx", "pageid=" & strPageId & "&action=S") & """><span>Verified Debit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DebitNote", "DebitNotePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Debit Note</span></a></li>" & _
                                            "<li><div class=""space""></div></li>" & _
                                            "</ul><div></div></div>"
            End If
        End If
    End Sub
End Class