
Imports System.IO
Imports AgoraLegacy
Imports eProcure.Component

Public Class InvoiceTrackingList
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim App As New AppGlobals
    Dim objTrac As New Tracking
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdPdf As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblRemark As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSaveInv As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdPrint As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden

    Dim dtPR As DataTable
    Dim dtDocMatch As DataTable
    Dim dtDocMatchWDO As DataTable
    'Protected WithEvents cmdMassApp As System.Web.UI.WebControls.Button
    Dim objGlobal As New AppGlobals

    Dim strInvAppr2 As String

    Dim IPPTimeFr1 As String = System.Configuration.ConfigurationManager.AppSettings.Get("IPPTimeFr1")
    Dim IPPTimeTo1 As String = System.Configuration.ConfigurationManager.AppSettings.Get("IPPTimeTo1")
    Dim IPPTimeFr2 As String = System.Configuration.ConfigurationManager.AppSettings.Get("IPPTimeFr2")
    Dim IPPTimeTo2 As String = System.Configuration.ConfigurationManager.AppSettings.Get("IPPTimeTo2")
    Dim TimeNow As String = DateTime.Now.ToLocalTime.ToString("HH:mm")



    Public Enum EnumInv
        icChk = 0
        icInvNo = 1
        icInvType = 2
        icIndex = 3
        icPayDate = 4
        icPoIndex = 5
        icVenId = 6
        icVenName = 7

        'Jules 2018.10.16
        icBaseAmt = 8
        icCurrency = 9 '8
        icAmount = 10 '9
        icDoc = 11 '10
        icPayTerm = 12 '11
        icBuyer = 13 '12
        icDept = 14 '13
        icPrinted = 15 '14
        icStatusCode = 16 '15
        icStatus = 17 '16
        icRemarks = 18 '17
        icAppDate = 19 ' 18
        icBillMethod = 20 '19
        icPymtMethod = 21 '20
        icDocType = 22 '21
        icContract = 23 '22
        icInvoiceNo = 24 '23
        icGSTInv = 25 '24

        'Zulham21112018
        icIPPPymtMethod = 27
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents cboFolder As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtDocNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVendorName As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtBuyer As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtDept As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdApprove As System.Web.UI.WebControls.Button
    Protected WithEvents cmdMark As System.Web.UI.WebControls.Button
    Protected WithEvents lblType As System.Web.UI.WebControls.Label
    Protected WithEvents dtgInvoice As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtStartDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEndDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Dim blnIPPUserGroup As Boolean
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub InvoiceTrackingList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        'Dim oriPageCount = Session("PageCount")
        Dim oriPageCount = 10
        Dim objDash As New Dashboard
        Session("PageCount") = 10

        'Jules 2018.07.16
        If Not Session("MixUserRole") Is Nothing Then
            Dim strRoles As New ArrayList()
            strRoles = Session("MixUserRole")
            If strRoles.Count > 0 Then
                For i As Integer = 0 To strRoles.Count - 1
                    If Replace(strRoles(i).ToString.ToUpper, " ", "") = "FINANCEMANAGER" Then
                        trTotalAmt.Visible = True
                        blnPaging = False
                        Exit For
                    End If
                Next
            End If
        End If
        'End modification.

        SetGridProperty(dtgInvoice)
        Session("PageCount") = oriPageCount
        SetGridProperty(dtgInvoice) 'Michelle (6/9/2007) - To rectify no. of records display per page according to user's setting
        'blnPaging = False 'mimi 20180703 'Jules 2018.07.16 commented. This is only for FM.

        Dim objCompany As New Companies
        strInvAppr2 = objCompany.GetInvApprMode(Session("CompanyId"))

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
            ViewState("role") = getUserRole(blnFO, blnFM)
            App.FillCodeTable(ddlCurr, CodeTable.Currency)
            'Zulham 12072018 - PAMB
            blnIPPUserGroup = objDB.Exist("SELECT '*' FROM  users_usrgrp WHERE uu_usrgrp_id IN ('IPP FM','IPP FO','E2P FO','E2P FM') AND uu_coy_id = '" & Session("CompanyID") & "' AND uu_user_id = '" & Session("UserID") & "'")

            ViewState("blnIPPUserGroup") = blnIPPUserGroup
            'If ViewState("role") = 3 Then Me.Response.Redirect(dDispatcher.direct("Tracking", "InvoiceVerified.aspx", "pageid=" & strPageId))
            'Michelle (8/10/2011) - Issue 986 
            blnFM = objDash.ChkFMCanApprove()
            ViewState("blnFM") = blnFM
            GenerateTab()

            Select Case ViewState("role")
                Case 2 ' is FO only
                    addListItem("New", "1")
                    addListItem("Sent", "2")
                    'addListItem("Trash", "5") - Michelle (CR0007)
                    addListItem("Archive", "5")
                    cmdApprove.Visible = False
                    cmdPreviewInvoice.Visible = False 'Jules 2018.07.18 - Only FM should be able to view.
                    'cmdMassApp.Visible = False
                    If Request.QueryString("folder") = "S" Then
                        ViewState("status") = "2,3,5"
                        cboFolder.SelectedItem.Value = "2"
                        cmdApprove.Visible = False
                        cmdSave.Visible = False
                        cmdMark.Visible = False
                        cmdReset.Visible = False
                        cmdSubmit.Visible = False
                        cmdPreviewInvoice.Visible = False
                        ViewState("IPPstatus") = "12,13"
                        ViewState("invfrom") = "sent"
                    ElseIf Request.QueryString("folder") = "API" Then
                        ViewState("status") = "2,3,5"
                        ViewState("invfrom") = "app/rej"
                        ViewState("IPPstatus") = "13,14"
                    Else

                        ViewState("status") = "1"
                        'Zulham 29062018 - PAMB
                        If Session("CompanyID").ToString.ToUpper <> "PAMB" Then
                            ViewState("IPPstatus") = "11,12"
                        Else
                            ViewState("IPPstatus") = "11,12,19"
                        End If
                        'End
                        ViewState("invfrom") = "new"
                        cboFolder.SelectedItem.Value = "1"
                    End If
                    cmdMark.Visible = False

                Case 3 ' is FM only
                    addListItem("Approval", "3")
                    addListItem("Payment", "4")
                    'addListItem("Trash", "5") - Michelle (CR0007)
                    addListItem("Archive", "5")
                    cboFolder.SelectedItem.Value = "3"
                    cmdSubmit.Visible = False
                    ' cmdMassApp.Visible = True - Michelle (CR0014) 
                    cmdMark.Visible = False
                    ViewState("status") = "2"
                    ViewState("invfrom") = "new"
                    ViewState("IPPstatus") = "12"
                Case 4 ' is both

                    'craven comment out to prevent both fixed role
                    'addListItem("New", "1")
                    'addListItem("Sent", "2")
                    'addListItem("Approval", "3")
                    'addListItem("Payment", "4")
                    ''addListItem("Trash", "5") - Michelle (CR0007)
                    'addListItem("Archive", "5")
                    ''cboFolder.SelectedItem.Value = "1"
                    'cmdApprove.Visible = False
                    ''cmdMassApp.Visible = False
                    'cmdMark.Visible = False
                    ''ViewState("status") = "1"
                    'If Request.QueryString("folder") = "S" Then
                    '    ViewState("status") = "2,3,5"
                    '    cboFolder.SelectedItem.Value = "2"
                    'Else
                    '    ViewState("status") = "1"
                    '    cboFolder.SelectedItem.Value = "1"
                    'End If
                    'craven comment out to prevent both fixed role END
                    addListItem("New", "1")
                    addListItem("Sent", "2")
                    'addListItem("Trash", "5") - Michelle (CR0007)
                    addListItem("Archive", "5")
                    cmdApprove.Visible = False
                    'cmdMassApp.Visible = False
                    If Request.QueryString("folder") = "S" Then
                        ViewState("status") = "2,3,5"
                        cboFolder.SelectedItem.Value = "2"
                        cmdApprove.Visible = False
                        cmdSave.Visible = False
                        cmdMark.Visible = False
                        cmdReset.Visible = False
                        cmdSubmit.Visible = False
                        cmdPreviewInvoice.Visible = False
                        ViewState("IPPstatus") = "12,13"
                        ViewState("invfrom") = "sent"
                    ElseIf Request.QueryString("folder") = "API" Then
                        ViewState("status") = "2,3,5"
                        ViewState("invfrom") = "app/rej"
                        ViewState("IPPstatus") = "13,14"
                    Else

                        ViewState("status") = "1"
                        ViewState("IPPstatus") = "11,12"
                        ViewState("invfrom") = "new"
                        cboFolder.SelectedItem.Value = "1"
                    End If
                    cmdMark.Visible = False
            End Select

            Session("role") = ViewState("role")
            Session("strurl") = strCallFrom

            'Jules 2018.10.11            
            objGlobal.FillAnalysisCode("L1", cboFundType)
            Common.SelDdl("ITNP", cboFundType)

            If Not blnIPPUserGroup Then
                dtgInvoice.Columns(EnumInv.icDoc).Visible = False
                ddlPaymentMode.ClearSelection()
                objGlobal.FillCodeTable(ddlPaymentMode, CodeTable.PaymentMethod)
                ddlPaymentMode.SelectedIndex = 0
            End If
            'End modification.

            SortDateDesc()

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
        cmdSave.Attributes.Add("onClick", "return resetSummary(0,1);")

        'Michelle (24/8/2007) - Remove the popup as it displays the wrong message
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOnePdf('chkSelection');")
        cmdDelete.ImageUrl = dDispatcher.direct("Plugins/images", "archive.GIF")
        cmdPdf.ImageUrl = dDispatcher.direct("Plugins/images", "pdf.bmp")
        cmdSaveInv.ImageUrl = dDispatcher.direct("Plugins/images", "i_save.gif")
        cmdPrint.ImageUrl = dDispatcher.direct("Plugins/images", "i_printer.gif")
        If Request.QueryString("folder") = "API" Then
            dtgInvoice.Columns(EnumInv.icStatus).Visible = True
            dtgInvoice.Columns(EnumInv.icChk).Visible = False
            dtgInvoice.Columns(EnumInv.icInvNo).HeaderText = "Document No."
            cmdSave.Visible = False
            cmdApprove.Visible = False
            cmdReset.Visible = False
            cmdSubmit.Visible = False
        End If
        Dim blnRelief As Boolean
        Dim objippmain As New IPPMain
        If objippmain.CheckRelief Then
            blnRelief = False ' temp set to false unless user want to use this function - michael
            Session("Relief") = blnRelief
        Else
            blnRelief = False
            Session("Relief") = blnRelief
        End If
        If blnIPPUserGroup = True Then
            cmdPreviewInvoice.Visible = False
        End If

        'Zulham 06082018 - PAMB
        If Session("CompanyID").ToString.ToUpper = "PAMB" Then
            lblAction.Text = "Fill in the search criteria and click Search button to list the relevant new Invoice. Click the Document No. to view the Invoice Details."
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

    Private Sub addListItem(ByVal strText As String, ByVal strVal As String)
        Dim lstItem As New ListItem
        lstItem.Value = strVal
        lstItem.Text = strText
        cboFolder.Items.Add(lstItem)
    End Sub
    'mimi 2018-06-08 : inv processing
    Protected Sub totalamount(sender As Object, e As EventArgs)
        Dim dgitem As DataGridItem
        Dim chk As CheckBox
        Dim dtr As DataRow
        Dim strtotalamt As Decimal

        For Each dgitem In dtgInvoice.Items
            chk = dgitem.FindControl("chkselection")
            If chk.Checked Then
                'Jules 2018.10.15
                'strtotalamt += CDec(dgitem.Cells(9).Text)
                strtotalamt += CDec(dgitem.Cells(EnumInv.icBaseAmt).Text)
                'End modification.
            End If
        Next
        txtTotalAmt.Text = Format(strtotalamt, "###,##0.00")
    End Sub


    Function bindgridINV() As String
        Dim ds As New DataSet
        Dim IPPstatus As String
        IPPstatus = ViewState("IPPstatus")
        'Jules 2018.10.11
        'Zulham 18072018 - PAMB
        'Zulham 13/07/2018 - PAMB
        'ds = objTrac.getInvoiceTracking_NoDocMatch(Common.Parse(txtDocNo.Text.Replace("\", "\\\\")), txtVendorName.Text, "", "", ViewState("status"), IPPstatus, ViewState("invfrom"), ddldocType.SelectedItem.Value, , , , ddlCurr.SelectedItem.Value, txtFundtype.Text, ddlCompResident.SelectedItem.Value, txtAmountFrom.Text, txtAmountTo.Text, txtDueDate.Text, ddlPaymentMode.SelectedItem.Value)
        ds = objTrac.getInvoiceTracking_NoDocMatch(Common.Parse(txtDocNo.Text.Replace("\", "\\\\")), txtVendorName.Text, "", "", ViewState("status"), IPPstatus, ViewState("invfrom"), ddldocType.SelectedItem.Value, , , , ddlCurr.SelectedItem.Value, cboFundType.SelectedValue, ddlCompResident.SelectedItem.Value, txtAmountFrom.Text, txtAmountTo.Text, txtDueDate.Text, ddlPaymentMode.SelectedItem.Value)
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
        intPageRecordCnt = ds.Tables(0).Rows.Count

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgInvoice, dvViewSample)

            dtgInvoice.DataSource = dvViewSample
            hidSummary.Value = ""
            hidControl.Value = ""
            If Request.QueryString("folder") <> "S" Then
                If ViewState("role") = "3" Then
                    FolderChanged(True, False, True, False, False, True, False)
                ElseIf ViewState("role") = "2" Then
                    FolderChanged(True, True, False, False, False, True, False)
                End If

            End If

            dtgInvoice.DataBind()
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
            Dim drDocMatchRow As DataRow

            Dim strTemp As String = ""
            Dim strURL As String
            Dim intPoIndex As Long
            Dim intLoop, intCnt As Integer
            Dim objDB As New EAD.DBCom

            Dim lnkDocNo As HyperLink
            lnkDocNo = e.Item.FindControl("lnkINVNo")
            If Not IsDBNull(dv("IM_PO_INDEX")) Then
                intPoIndex = dv("IM_PO_INDEX")

                If objTrac.chkInvContract(intPoIndex) = True Then
                    lnkDocNo.Text = dv("IM_INVOICE_NO") & " (C)"
                    e.Item.Cells(EnumInv.icContract).Text = "Y"
                Else
                    lnkDocNo.Text = dv("IM_INVOICE_NO") & " (NC)"
                    e.Item.Cells(EnumInv.icContract).Text = "N"
                End If
            Else
                lnkDocNo.Text = dv("IM_INVOICE_NO")
                e.Item.Cells(EnumInv.icContract).Text = "IPP"
            End If



            If cmdSubmit.Visible = True Then
                ViewState("cmd") = 1
            ElseIf cmdApprove.Visible = True Then
                ViewState("cmd") = 2
            End If

            'lnkDocNo.Attributes.Add("onclick", "PopWindow('../Invoice/PreviewInvoice.aspx?pageid=" & strPageId & "&INVNO=" & Common.parseNull(dv("IM_INVOICE_NO")) & "&freeze=0&vcomid=" & Common.parseNull(dv("IM_S_COY_ID")) & "',  '', 'resizable=no,scrollbars=yes,width=600,height=600,status=no,menubar=no');")
            'lnkDocNo.NavigateUrl = "#"
            'Zulham 12072018 - PAMB
            If Request.QueryString("folder") = "API" Then
                lnkDocNo.NavigateUrl = dDispatcher.direct("Tracking", "InvoiceDetails.aspx", "cmd=" & ViewState("cmd") & "&role=" & ViewState("role") & "&Frm=" & "InvoiceTrackingList" & "&pageid=" & strPageId & "&INVNO=" & Server.UrlEncode(Common.parseNull(dv("IM_INVOICE_NO"))) & "&vcomid=" & Common.parseNull(dv("IM_S_COY_ID")) & "&folder=API&status=" & Common.parseNull(dv("IM_INVOICE_STATUS")))
            Else
                lnkDocNo.NavigateUrl = dDispatcher.direct("Tracking", "InvoiceDetails.aspx", "cmd=" & ViewState("cmd") & "&role=" & ViewState("role") & "&Frm=" & "InvoiceTrackingList" & "&pageid=" & strPageId & "&INVNO=" & Server.UrlEncode(Common.parseNull(dv("IM_INVOICE_NO"))) & "&vcomid=" & Common.parseNull(dv("IM_S_COY_ID")) & "&folder=" & Getfoldertype() & "&status=" & Common.parseNull(dv("IM_INVOICE_STATUS")))
            End If
            'End

            ' Start here

            'Michelle (6/9/2007) - To cater for 1 PR to multiple POs
            'dtPR = objTrac.getRelatedPR(intPoIndex)

            '' ''dtPR = objTrac.getRelatedPR_PO(dv("POM_PO_NO"), Session("CompanyID"))
            '' ''For Each drRow In dtPR.Rows
            '' ''    strURL = "<a href=" & dDispatcher.direct("PO", "PRDetail.aspx", "caller=" & "OTHER" & "&PageID=" & strPageId & _
            '' ''        "&index=" & drRow("PRM_PR_INDEX") & "&PRNo=" & drRow("PRM_PR_NO")) & ">" & drRow("PRM_PR_NO") & "</a>"
            '' ''    strTemp &= strURL & "<BR>"
            '' ''Next

            Dim PRIndex = dv("PRM_PR_INDEX")
            Dim i As Integer
            If Not IsDBNull(PRIndex) Then
                Dim curPRIndex = PRIndex.Split(",")
                For i = 0 To curPRIndex.Length - 1
                    If Common.parseNull(curPRIndex(i)) <> "" Then
                        Dim strPR_No As String = CStr(objDB.GetVal("SELECT PRM_PR_NO FROM PR_MSTR WHERE PRM_COY_ID = '" & Session("CompanyID") & "' AND PRM_PR_INDEX = '" & curPRIndex(i) & "'"))

                        'strURL = "<a href=" & dDispatcher.direct("PO", "PRDetail.aspx", "caller=" & "OTHER" & "&PageID=" & strPageId & _
                        strURL = "<a href=" & dDispatcher.direct("PR", "PRDetail.aspx", "caller=" & "InvTracking" & "&PageID=" & strPageId &
                                "&index=" & curPRIndex(i) & "&PRNo=" & strPR_No) & ">" & strPR_No & "</a>"
                        strTemp &= strURL & "<BR>"
                    End If
                Next i
            End If

            strTemp &= "<a href=" & dDispatcher.direct("PO", "PODetail.aspx", "Frm=" & "InvoiceTrackingList" & "&caller=" & "buyer" & "&PO_INDEX=" & dv("IM_PO_INDEX") & "&PO_NO=" & dv("POM_PO_NO") & "&BCoyID=" & Session("CompanyID") & "&side=" & "other" & "&filetype=" & "2" & "&folder=" & Getfoldertype() & "&pageid=" & strPageId) & ">" & dv("POM_PO_NO") & "</a><BR>"


            Dim DONo = dv("CDM_DO_NO")
            Dim GRNNo = dv("CDM_GRN_NO")
            Dim PONo = dv("CDM_PO_NO")
            If Not IsDBNull(DONo) Then
                Dim curDoNo = DONo.Split(",")
                Dim curPONo = PONo.split(",")
                Dim curGRNNo As Array
                If Not IsDBNull(GRNNo) Then
                    curGRNNo = GRNNo.Split(",")
                Else
                    curGRNNo = Nothing
                End If
                For i = 0 To curDoNo.Length - 1
                    strURL = dDispatcher.direct("DO", "PreviewDODetails.aspx", "caller=buyer" & "&DONo=" & curDoNo(i) & "&POIdx=" & intPoIndex & "&SCoyID=" & dv("CDM_S_COY_ID") & "&PageID=" & strPageId)
                    strTemp &= "<a href=""#"" onclick=""PopWindow('" & strURL & "')"">" & curDoNo(i) & "</a><BR>"

                    If curGRNNo IsNot Nothing Then
                        'strURL = dDispatcher.direct("Report", "PreviewGRN.aspx", "GRNNo=" & curGRNNo(i) & "&PONo=" & Common.parseNull(dv("CDM_PO_NO")) & "&DONo=" & Common.parseNull(curDoNo(i)) & "&pageid=" & strPageId & "&BCoyID=" & Session("CompanyID"))
                        strURL = dDispatcher.direct("Report", "PreviewGRN.aspx", "GRNNo=" & curGRNNo(i) & "&PONo=" & Common.parseNull(curPONo(i)) & "&DONo=" & Common.parseNull(curDoNo(i)) & "&pageid=" & strPageId & "&BCoyID=" & Session("CompanyID"))
                        strTemp &= "<a href=""#"" onclick=""PopWindow('" & strURL & "')"">" & curGRNNo(i) & "</a><BR>"
                    End If
                Next i
            End If


            Dim dvMatch As DataView = dv.DataView
            ' End

            'Jules 2018.10.16
            If Not IsDBNull(dv("INVAMT_INMYR")) Then
                e.Item.Cells(EnumInv.icBaseAmt).Text = Format(CDbl(dv("INVAMT_INMYR")), "###,##0.00")
            End If
            'End modification.

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

            'Zulham 13082018 - PAMB
            Session("Index") = dv("IM_INVOICE_INDEX")

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
                                        If strRemarks = "" Then strRemarks = "-"
                                        lblRemarks.Text &= intSeq & ". " & strRemarks & "<br>"
                                End Select
                            ElseIf intSeq = intAction Then
                                If strRemarks = "" Then strRemarks = "-"
                                lblRemarks.Text &= intSeq & ". " & strRemarks & "<br>"
                            Else
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
                    lblRemarks.Visible = False

                Case "2" ' sent
                    e.Item.Cells(EnumInv.icChk).Visible = False
                    e.Item.Cells(EnumInv.icPayTerm).Visible = False
                    e.Item.Cells(EnumInv.icAppDate).Visible = False

                Case "3" ' approval
                    e.Item.Cells(EnumInv.icPayTerm).Visible = False
                    e.Item.Cells(EnumInv.icAppDate).Visible = False

                Case "4" ' payment

                    e.Item.Cells(EnumInv.icDoc).Visible = False

                    ' Check if is highest FM approval

                    Dim blnHighestFMApproval As Boolean = objTrac.HighestFMApprLevel(e.Item.Cells(EnumInv.icIndex).Text)

                    'Michelle (24/8/2007) - To enable the 'chk' to True if the logon user is the FM and the invoice
                    If Not blnHighestFMApproval Then
                        If ViewState("role") = 3 Or ViewState("role") = 4 Then
                            Dim strSql, strInvAppr
                            '' ''Dim objDb As New EAD.DBCom
                            strSql = "SELECT CM_INV_APPR FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Session("CompanyID") & "'"
                            Dim tDS As DataSet = objDB.FillDs(strSql)
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

            'for IPP
            If dv("DOC_TYPE") = "IPP" Then
                'e.Item.Cells(EnumInv.icPayDate).Text = ""
                'Zulham 12072018 - PAMB
                If Request.QueryString("folder") = "API" Then
                    lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "IPPApprovalDetail.aspx", "mode=read&pagefrm=api&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO")) & "&index=" & dv("IM_INVOICE_INDEX"))
                ElseIf Request.QueryString("folder") = "S" Then
                    lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "IPPApprovalDetail.aspx", "mode=read&pagefrm=s&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO")) & "&index=" & dv("IM_INVOICE_INDEX"))
                ElseIf ViewState("role") = "3" Then
                    lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "IPPApprovalDetail.aspx", "pagefrm=verified&DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO")) & "&index=" & dv("IM_INVOICE_INDEX"))
                Else
                    lnkDocNo.NavigateUrl = dDispatcher.direct("IPP", "IPPApprovalDetail.aspx", "DocumentNo=" & Server.UrlEncode(dv("IM_INVOICE_NO")) & "&index=" & dv("IM_INVOICE_INDEX"))
                End If
                'End
                Select Case e.Item.Cells(EnumInv.icInvType).Text
                    Case "INV"
                        e.Item.Cells(EnumInv.icInvType).Text = "Invoice"
                    Case "BILL"
                        e.Item.Cells(EnumInv.icInvType).Text = "Bill"
                    Case "CN"
                        e.Item.Cells(EnumInv.icInvType).Text = "Credit Note"
                    Case "DN"
                        e.Item.Cells(EnumInv.icInvType).Text = "Debit Note"
                End Select
                Select Case dv("IM_INVOICE_STATUS")
                    Case "14"
                        e.Item.Cells(EnumInv.icStatus).Text = "Rejected"
                    Case "13"
                        e.Item.Cells(EnumInv.icStatus).Text = "Approved"
                End Select
            Else
                e.Item.Cells(EnumInv.icInvType).Text = "Invoice"
            End If

            'Zulham 21112018
            Select Case e.Item.Cells(EnumInv.icIPPPymtMethod).Text
                Case "IBG"
                    e.Item.Cells(EnumInv.icIPPPymtMethod).Text = "LOCAL BANK TRANSFER-(RM)"
                Case "TT"
                    e.Item.Cells(EnumInv.icIPPPymtMethod).Text = "TELEGRAPHIC TRANSFER-(FOREIGN CURRENCY)"
                Case "BC"
                    e.Item.Cells(EnumInv.icIPPPymtMethod).Text = "CHEQUE-(RM)"
                Case "BD"
                    e.Item.Cells(EnumInv.icIPPPymtMethod).Text = "BANK DRAFT-(FOREIGN CURRENCY)"
                Case "CO"
                    e.Item.Cells(EnumInv.icIPPPymtMethod).Text = "CASHIER'S ORDER-(RM)"
            End Select

        ElseIf e.Item.ItemType = ListItemType.Header Then
            Select Case cboFolder.SelectedItem.Value
                Case "1", "3"
                    e.Item.Cells(EnumInv.icPayTerm).Visible = False
                    e.Item.Cells(EnumInv.icAppDate).Visible = False
                Case "2"
                    e.Item.Cells(EnumInv.icPayTerm).Visible = False
                    e.Item.Cells(EnumInv.icAppDate).Visible = False
                    e.Item.Cells(EnumInv.icChk).Visible = False
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
                ViewState("status") = "2,3,4,5"
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
        dtItem.Columns.Add("DocType", Type.GetType("System.String"))
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
        dtItem.Columns.Add("Contract", Type.GetType("System.String"))

        Dim dtr As DataRow
        Dim objCn As New CreditNote
        Dim strInv As String = ""
        Dim strInvFail As String = ""
        Dim strInvFailAck As String = ""
        Session("invListFailAck") = Nothing

        If blnFMMassAppr Then
            Dim dtInvoiceTracking As DataTable = CType(ViewState("dtInvoiceTracking"), DataTable)

            For Each drItem As DataRow In dtInvoiceTracking.Rows
                dtr = dtItem.NewRow
                'to get the document type 
                dtr("DocType") = drItem.Item("DOC_TYPE")
                If dtr("DocType") = "EPROC" Then
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
                    dtr("Contract") = ""

                    If strType = "2" Then
                        dtr("Submitted") = Session("UserId")
                        dtr("InvStatus") = invStatus.PendingAppr
                    ElseIf strType = "3" Then
                        dtr("AppDate") = Date.Today.ToString
                        dtr("InvStatus") = invStatus.Approved
                    End If
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
                    dtr("DocType") = dgItem.Cells(EnumInv.icDocType).Text
                    bln = True
                    If dtr("DocType") = "EPROC" Then
                        'dtr("InvNo") = CType(dgItem.FindControl("lnkINVNo"), HyperLink).Text
                        dtr("InvNo") = dgItem.Cells(EnumInv.icInvoiceNo).Text

                        If objCn.chkInvPendingAckCN(dtr("InvNo"), dgItem.Cells(EnumInv.icVenId).Text) = True Then
                            bln = False
                            If strInvFailAck = "" Then
                                strInvFailAck &= dtr("InvNo")
                            Else
                                strInvFailAck &= "," & dtr("InvNo")
                            End If
                        Else
                            If strType <> "4" Or (strType = "4" And dgItem.Cells(EnumInv.icStatusCode).Text <> invStatus.Paid) Then
                                bln = True
                                strInv &= dtr("InvNo") & ","
                            Else
                                bln = False
                                strInvFail &= dtr("InvNo") & ","
                            End If
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
                        dtr("Contract") = dgItem.Cells(EnumInv.icContract).Text
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
                    End If
                    If bln Then
                        'dtItem.Rows.Add(dtr)
                        'Add by craven to combine the loop become 1 
                        If dtr.Item("DocType") = "EPROC" Then
                            If strType = "2" Or strType = "3" Or strType = "4" Then
                                Dim blnHighestLevel As Boolean
                                ' Dim strMsg As String = objTrac.ApproveInvoice(Common.Parse(dtr("InvNo")), Common.Parse(dtr("Vendor")), IIf(strType = "2", "FO", "FM"), Common.Parse(dtr("FinRemark")), False, blnHighestLevel) - Michelle (24/12/2007) - To cater for single quote
                                'Dim strMsg As String = objTrac.ApproveInvoice(Common.Parse(dtr("InvNo")), Common.Parse(dtr("Vendor")), IIf(strType = "2", "FO", "FM"), dtr("FinRemark"), False, blnHighestLevel)

                                Dim strMsg As String
                                If strInvAppr2 = "N" Then
                                    strMsg = objTrac.ApproveInvoice(Common.Parse(dtr("InvNo")), Common.Parse(dtr("Vendor")), IIf(strType = "2", "FO", "FM"), dtr("FinRemark"), True, blnHighestLevel, , True)
                                Else
                                    strMsg = objTrac.ApproveInvoice(Common.Parse(dtr("InvNo")), Common.Parse(dtr("Vendor")), IIf(strType = "2", "FO", "FM"), dtr("FinRemark"), False, blnHighestLevel, , True)
                                End If

                                If strMsg <> "" Then
                                    Common.NetMsgbox(Me, strMsg)
                                    Session("invList") = ""
                                    Exit Function
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
                            End If
                            If strType = "1" Then 'save                       
                                objTrac.updateAppRemark(dtr("InvIndex"), dtr("FinRemark"))
                            End If

                        End If

                        'for ipp
                        If dgItem.Cells(EnumInv.icDocType).Text = "IPP" Then
                            ' build remark 
                            'Michelle (24/4/2012)- Issue 1590
                            Dim strRemark As String = ""
                            If strType = "1" Then 'save                       
                                strRemark = CType(dgItem.FindControl("txtRemark"), TextBox).Text
                            Else
                                'If CType(dgItem.FindControl("txtRemark"), TextBox).Text.IndexOf("Approved : ") <> 0 Then
                                If strType = "2" Then
                                    strRemark = FormatAORemark("verify")
                                Else
                                    strRemark = FormatAORemark("approve")
                                End If


                                'strRemark = FormatAORemark("approve")
                                'Zulham 14112018
                                strRemark = strRemark & IIf(CType(dgItem.FindControl("txtRemark"), TextBox).Text = "", "Submitted", CType(dgItem.FindControl("txtRemark"), TextBox).Text)
                                'End If

                            End If
                            'strRemark = strRemark & CType(dgItem.FindControl("txtRemark"), TextBox).Text

                            'if is TT pop up error message 

                            Dim objippmain As New IPPMain
                            Dim intApprGrpIndex As String
                            Dim objDb As New EAD.DBCom
                            Dim dsApprIPPDetails As New DataSet
                            Dim i As Integer
                            Dim strBillInvApprBy As String
                            Dim strDocOwner As String
                            Dim objUsers As New Users
                            Dim blnIPPO As Boolean

                            intApprGrpIndex = objDb.GetVal("SELECT DISTINCT FA_APPROVAL_GRP_INDEX FROM finance_approval WHERE FA_INVOICE_INDEX = '" & dgItem.Cells(EnumInv.icIndex).Text & "' ")
                            dsApprIPPDetails = objippmain.getApprIPPDetail(dgItem.Cells(EnumInv.icInvoiceNo).Text, dgItem.Cells(EnumInv.icIndex).Text, Common.Parse(HttpContext.Current.Session("CompanyID")))
                            strDocOwner = objDb.GetVal("SELECT im_created_by FROM invoice_mstr WHERE im_invoice_index = '" & dgItem.Cells(EnumInv.icIndex).Text & "' ")
                            blnIPPO = objUsers.checkUserFixedRole("'IPP Officer'", strDocOwner)

                            If objippmain.checkDept() = False Then
                                strInvFail = objGlobal.GetErrorMessage("00046")
                                Exit For
                            End If
                            If strType = "2" Then
                                If chk.Checked = True Then
                                    If CType(dgItem.FindControl("txtRemark"), TextBox).Text = "" And blnIPPO = False Then
                                        'strInvFail = "Invoice " & CType(dgItem.FindControl("lnkINVNo"), HyperLink).Text & " required Approval Remarks."
                                        'Zulham 03122018
                                        'strInvFail = "Invoice " & dgItem.Cells(EnumInv.icInvoiceNo).Text & " required Approval Remarks."
                                        'Exit For
                                        ' Else
                                    End If
                                    'check line item exchange rate & currency for related company except HLISB
                                    'mimi 2018-04-24 : remove hardcode HLB to PAMB
                                    For i = 0 To dsApprIPPDetails.Tables(0).Rows.Count - 1
                                        'Zulham 14072018 - PAMB
                                        If UCase(dsApprIPPDetails.Tables(0).Rows(i).Item("id_pay_for")) <> Session("CompanyID").ToString.ToUpper Then
                                            'Zulham 23/08/2017
                                            'get pob company's country
                                            Dim strCountry = objDb.GetVal("SELECT ic_country FROM ipp_company WHERE ic_other_b_coy_code = '" & UCase(dsApprIPPDetails.Tables(0).Rows(i).Item("id_pay_for")) & "'")

                                            If dsApprIPPDetails.Tables(0).Rows(i).Item("id_dr_exchange_rate") Is System.DBNull.Value And strCountry.ToString.ToUpper <> "MY" Then
                                                'strInvFail = "Invoice " & CType(dgItem.FindControl("lnkINVNo"), HyperLink).Text & " required Exchange Rate for line item."
                                                strInvFail = "Invoice " & dgItem.Cells(EnumInv.icInvoiceNo).Text & " required Exchange Rate for line item."
                                                GoTo jjj
                                            End If
                                        ElseIf UCase(dsApprIPPDetails.Tables(0).Rows(i).Item("id_pay_for")) = "HLISB" Then
                                            objTrac.updateHLISBCurrency(dsApprIPPDetails.Tables(0).Rows(i).Item("ID_Invoice_No"), dsApprIPPDetails.Tables(0).Rows(i).Item("id_invoice_line"), dsApprIPPDetails.Tables(0).Rows(i).Item("id_s_coy_id"))
                                        End If
                                    Next
                                End If
                            End If
                            If strType = "3" Or strType = "2" Then
                                Dim objDoc As New IPPMain
                                If chk.Checked = True Then
                                    'Zulham 08042015 Case 8603
                                    If (dgItem.Cells(EnumInv.icPymtMethod).Text = "TT" Or dgItem.Cells(EnumInv.icPymtMethod).Text.ToUpper.Contains("NOSTRO")) And strType <> "2" And ViewState("role") = "3" Then
                                        If objippmain.exchangeRate(dgItem.Cells(EnumInv.icIndex).Text) Then
                                            'strInvFail = "Invoice " & CType(dgItem.FindControl("lnkINVNo"), HyperLink).Text & " required exchange rate."
                                            strInvFail = "Invoice " & dgItem.Cells(EnumInv.icInvoiceNo).Text & " required exchange rate."
                                            Exit For
                                        End If

                                    End If

                                    If ((CDate(TimeNow) >= CDate(IPPTimeFr1) And CDate(TimeNow) <= CDate(IPPTimeTo1)) Or (CDate(TimeNow) >= CDate(IPPTimeFr2) And CDate(TimeNow) <= CDate(IPPTimeTo2))) And ViewState("role") = "3" Then
                                        strInvFail = objGlobal.GetErrorMessage("00258") & dgItem.Cells(EnumInv.icInvoiceNo).Text 'CType(dgItem.FindControl("lnkINVNo"), HyperLink).Text
                                        Exit For
                                    End If

                                    If objDoc.CheckBankCode(dgItem.Cells(EnumInv.icIndex).Text) Then
                                        strInvFail = objGlobal.GetErrorMessage("00323") & dgItem.Cells(EnumInv.icInvoiceNo).Text 'CType(dgItem.FindControl("lnkINVNo"), HyperLink).Text
                                        Exit For
                                    End If
                                    'Zulham 14072018 - PAMB
                                    'if approval limit < invoice total = invoice goes to the next person 
                                    Dim blnCanApprove = objDoc.canApprove(CDec(dgItem.Cells(EnumInv.icAmount).Text))
                                    If objDoc.ApproveIPPDoc(dgItem.Cells(EnumInv.icIndex).Text, strRemark, Session("Relief"), ViewState("role"), dgItem.Cells(EnumInv.icPymtMethod).Text, , dgItem.Cells(EnumInv.icInvType).Text, IPPTimeFr1, intApprGrpIndex, strBillInvApprBy, currentStatus:=dgItem.Cells(EnumInv.icStatusCode).Text, blnCanApprove:=blnCanApprove) = True Then

                                        'Zulham 03122018
                                        If strType = 2 Then
                                            objDoc.sendMailToIPPOfficer(dgItem.Cells(EnumInv.icIndex).Text, 2, "verify")
                                        End If

                                        strInv &= dgItem.Cells(EnumInv.icInvoiceNo).Text & "," 'CType(dgItem.FindControl("lnkINVNo"), HyperLink).Text & ","
                                    End If

                                End If
                            End If
                            If strType = "1" Then 'save
                                objTrac.updateAppRemark(dgItem.Cells(EnumInv.icIndex).Text, strRemark)
                                'objTrac.updateAppRemark(dgItem.Cells(EnumInv.icIndex).Text, strRemark)
                            End If
                        End If
                        ' END CRAVEN
                    End If
                End If
            Next
jjj:
        End If

        Me.Session.Add("invList", strInv)
        If strInvFail <> "" Then
            Me.Session.Add("invListFail", strInvFail)
        End If
        If strInvFailAck <> "" Then
            Me.Session.Add("invListFailAck", strInvFailAck)
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
                'dtr("InvNo") = CType(dgItem.FindControl("lnkINVNo"), HyperLink).Text
                dtr("InvNo") = dgItem.Cells(EnumInv.icInvoiceNo).Text
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
        Dim strTmp As String = ""
        Dim strMsg As String = ""
        Dim intMsg As Integer

        ' FO approval
        If chkOnlyContract(intMsg) = True Then
            bindInv("2", True, False)
            If Len(Session("invList")) > 0 Then
                If ViewState("blnIPPUserGroup") = False Then
                    strTmp = Mid(Session("invList"), 1, Len(Session("invList")) - 1)
                Else
                    strTmp = Mid(Session("invList"), 1, Len(Session("invList")) - 1)
                End If

                strMsg = objGlobal.GetErrorMessage("00043") & " " & strTmp & " " & objGlobal.GetErrorMessage("00044")

                If Len(Session("invListFailAck")) > 0 Then
                    strMsg = strMsg & """ & vbCrLf & """ & "Mass Verify is not allowed for " & Session("invListFailAck") & " due to pending Credit Note acknowledgement."
                End If
                Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "Frm=" & "InvoiceTrackingList" & "&pageid=" & strPageId))
                If Len(Session("invListFail")) > 0 Then
                    Common.NetMsgbox(Me, Session("invListFail"), MsgBoxStyle.Information)
                End If
            ElseIf Len(Session("invListFail")) > 0 Then
                Common.NetMsgbox(Me, Session("invListFail"), MsgBoxStyle.Information)
            ElseIf Len(Session("invListFailAck")) > 0 Then
                strMsg = "Mass Verify is not allowed for " & Session("invListFailAck") & " due to pending Credit Note acknowledgement."
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            End If
            Session("invListFail") = Nothing
            Session("invList") = Nothing
            Session("invListFailAck") = Nothing

            If ViewState("blnIPPUserGroup") = False Then
                cmdSubmit.Visible = False
            Else
                cmdSubmit.Visible = True
            End If
            bindgridINV()
        Else
            If intMsg = 1 Then
                Common.NetMsgbox(Me, "Mass Verification is only allowed for Contract (C) INV.", MsgBoxStyle.Information)
            ElseIf intMsg = 3 Then
                Common.NetMsgbox(Me, "Credit Note/Debit Note cannot precede the invoices it's tied to.", MsgBoxStyle.Information)
            Else
                Common.NetMsgbox(Me, "Item's GST Purchase Tax Code value is not set [" & ViewState("ErrInvNo") & "].", MsgBoxStyle.Information)
            End If
        End If
    End Sub

    Private Sub cmdApprove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApprove.Click
        Dim strTmp As String = ""
        Dim strMsg As String = ""
        Dim strFullMsg As String = ""

        'Zulham 19/02/2016 - IPP Stage 4 Phase 2
        For Each dgItem As DataGridItem In dtgInvoice.Items
            Dim chk = dgItem.FindControl("chkSelection")
            Dim cboPay As DropDownList
            Dim objDoc As New IPPMain
            Dim objDB As New EAD.DBCom
            If chk.Checked Then
                'dim 
                ''Zulham 05/01/2016 - IPP STAGE 4 PHASE 2 (CR)
                ''if the tied invoice is rejected or void, can't submit the credit note
                Dim docNo = objDB.GetVal("select im_invoice_no from invoice_mstr where im_invoice_index = '" & dgItem.Cells(EnumInv.icIndex).Text & "'")
                Dim boolCantReject As Boolean = False
                Dim dsRefNo As New DataSet
                Dim objippmain As New IPPMain
                'Zulham 05/01/2016 - IPP STAGE 4 Phase 2
                'check for invoices status that are tied to CN/DN
                If objippmain.checkStatus(docNo, dgItem.Cells(EnumInv.icVenId).Text, dgItem.Cells(EnumInv.icStatusCode).Text) = False Then
                    strMsg = "Credit Note/Debit Note cannot precede the invoices it's tied to."
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                    Exit Sub
                End If
            End If
        Next

        bindInv("1", False, False)
        bindInv("3", False, False)
        If Len(Session("invList")) > 0 Then
            strTmp = Mid(Session("invList"), 1, Len(Session("invList")) - 1)
            'strTmp = Mid(Session("invList"), 1, Len(Session("invList")))
            strMsg = objGlobal.GetErrorMessage("00043") & " " & strTmp & " " & objGlobal.GetErrorMessage("00044")
            If Len(Session("invListFailAck")) > 0 Then
                strMsg = strMsg & """ & vbCrLf & """ & "Mass Verify is not allowed for " & Session("invListFailAck") & " due to pending Credit Note acknowledgement."
            End If
            If Len(Session("invListFail")) > 0 Then
                Common.NetMsgbox(Me, strMsg & """ & vbCrLf & """ & Session("invListFail"), MsgBoxStyle.Information)
            Else
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            End If

        ElseIf Len(Session("invListFail")) > 0 Then
            Common.NetMsgbox(Me, Session("invListFail"), MsgBoxStyle.Information)
        ElseIf Len(Session("invListFailAck")) > 0 Then
            strMsg = "Mass Verify is not allowed for " & Session("invListFailAck") & " due to pending Credit Note acknowledgement."
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        End If
        Session("invListFail") = Nothing
        Session("invList") = Nothing
        bindgridINV()
        'Response.Redirect(dDispatcher.direct("Tracking", "TrackConfirm.aspx", "pageid=" & strPageId & "&type=" & "A"))
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
                'dr("INVNO") = CType(dgItem.FindControl("lnkINVNo"), HyperLink).Text
                dr("INVNO") = dgItem.Cells(EnumInv.icInvoiceNo).Text
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
                'dr("INVNO") = CType(dgItem.FindControl("lnkINVNo"), HyperLink).Text
                dr("INVNO") = dgItem.Cells(EnumInv.icInvoiceNo).Text
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

        If ViewState("role") = "2" And Request.QueryString("folder") = "S" And Request.QueryString("ipp") <> "y" Then
            Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" &
                                "<li><div class=""space""></div></li>" &
                                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & """><span>New Invoice</span></a></li>" &
                                                    "<li><div class=""space""></div></li>" &
                                                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId & "&folder=S&status=1") & """><span>Verified Invoice</span></a></li>" &
                                                    "<li><div class=""space""></div></li>" &
                                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" &
                                                    "<li><div class=""space""></div></li>" &
                                                    "</ul><div></div></div>"
        ElseIf ViewState("role") = "2" And Request.QueryString("folder") = "API" And Request.QueryString("ipp") <> "y" Then
            Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" &
                                            "<li><div class=""space""></div></li>" &
                                                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & """><span>New Invoice</span></a></li>" &
                                                                "<li><div class=""space""></div></li>" &
                                                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId & "&folder=S&status=1") & """><span>Verified Invoice</span></a></li>" &
                                                                "<li><div class=""space""></div></li>" &
                                                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" &
                                                                "<li><div class=""space""></div></li>" &
                                                                "</ul><div></div></div>"
        ElseIf ViewState("role") = "2" Then
            If Request.QueryString("ipp") = "y" And Request.QueryString("folder") = "S" Then
                Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" &
                                   "<li><div class=""space""></div></li>" &
                                                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & """><span>New Invoice</span></a></li>" &
                                                       "<li><div class=""space""></div></li>" &
                                                       "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId & "&folder=S&status=1&ipp=y") & """><span>Verified Invoice</span></a></li>" &
                                                       "<li><div class=""space""></div></li>" &
                                                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" &
                                                       "<li><div class=""space""></div></li>" &
                                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "IPPFVPendingFYFA.aspx", "pageid=" & strPageId) & """><span>Pending FYFA</span></a></li>" &
                                                       "<li><div class=""space""></div></li>" &
                                                       "</ul><div></div></div>"
            ElseIf Request.QueryString("ipp") = "y" And Request.QueryString("folder") = "N" Then
                Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" &
                              "<li><div class=""space""></div></li>" &
                                                  "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & """><span>New Invoice</span></a></li>" &
                                                  "<li><div class=""space""></div></li>" &
                                                  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId & "&folder=S&status=1&ipp=y") & """><span>Verified Invoice</span></a></li>" &
                                                  "<li><div class=""space""></div></li>" &
                                                  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" &
                                                  "<li><div class=""space""></div></li>" &
                                                   "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "IPPFVPendingFYFA.aspx", "pageid=" & strPageId) & """><span>Pending FYFA</span></a></li>" &
                                                  "<li><div class=""space""></div></li>" &
                                                  "</ul><div></div></div>"
            Else
                If blnIPPUserGroup = True Then
                    Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" &
                    "<li><div class=""space""></div></li>" &
                   "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & """><span>New Invoice</span></a></li>" &
                   "<li><div class=""space""></div></li>" &
                   "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId & "&folder=S&status=1&ipp=y") & """><span>Verified Invoice</span></a></li>" &
                   "<li><div class=""space""></div></li>" &
                   "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" &
                   "<li><div class=""space""></div></li>" &
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPP", "IPPFVPendingFYFA.aspx", "pageid=" & strPageId) & """><span>Pending FYFA</span></a></li>" &
                  "<li><div class=""space""></div></li>" &
                   "</ul><div></div></div>"

                Else
                    Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" &
                                                  "<li><div class=""space""></div></li>" &
                                                                      "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & """><span>New Invoice</span></a></li>" &
                                                                      "<li><div class=""space""></div></li>" &
                                                                      "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId & "&folder=S&status=1") & """><span>Verified Invoice</span></a></li>" &
                                                                      "<li><div class=""space""></div></li>" &
                                                                      "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" &
                                                                      "<li><div class=""space""></div></li>" &
                                                                      "</ul><div></div></div>"
                End If


            End If



        ElseIf ViewState("role") = "3" Then
            'Zulham 07122018 - PAMB
            If ViewState("blnIPPUserGroup") Then
                Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" &
                    "<li><div class=""space""></div></li>" &
                                        "<li><a class=""t_entity_btn_selected"" href=""#""><span>Verified Invoice</span></a></li>" &
                                        "<li><div class=""space""></div></li>" &
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceVerified.aspx", "pageid=" & strPageId) & """><span>Approved Invoice</span></a></li>" &
                                        "<li><div class=""space""></div></li>" &
                                        "</ul><div></div></div>"
            Else
                Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" &
                    "<li><div class=""space""></div></li>" &
                                        "<li><a class=""t_entity_btn_selected"" href=""#""><span>Verified Invoice</span></a></li>" &
                                        "<li><div class=""space""></div></li>" &
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceVerified.aspx", "pageid=" & strPageId) & """><span>Approved Invoice</span></a></li>" &
                                        "<li><div class=""space""></div></li>" &
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" &
                                        "<li><div class=""space""></div></li>" &
                                        "</ul><div></div></div>"
            End If

            'Michelle (8/10/2011) - Issue 986
            'ElseIf ViewState("role") = "4" And Request.QueryString("folder") = "S" Then
            '    Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" & _
            '            "<li><div class=""space""></div></li>" & _
            '                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & """><span>New Invoice</span></a></li>" & _
            '                                "<li><div class=""space""></div></li>" & _
            '                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId & "&folder=S&status=1") & """><span>Verified Invoice</span></a></li>" & _
            '                                "<li><div class=""space""></div></li>" & _
            '                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceVerified.aspx", "pageid=" & strPageId) & """><span>Approved Invoice</span></a></li>" & _
            '                                "<li><div class=""space""></div></li>" & _
            '                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
            '                                "<li><div class=""space""></div></li>" & _
            '                                "</ul><div></div></div>"
            'ElseIf ViewState("role") = "4" Then
            '    Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" & _
            '            "<li><div class=""space""></div></li>" & _
            '                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & """><span>New Invoice</span></a></li>" & _
            '                                "<li><div class=""space""></div></li>" & _
            '                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId & "&folder=S&status=1") & """><span>Verified Invoice</span></a></li>" & _
            '                                "<li><div class=""space""></div></li>" & _
            '                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceVerified.aspx", "pageid=" & strPageId) & """><span>Approved Invoice</span></a></li>" & _
            '                                "<li><div class=""space""></div></li>" & _
            '                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
            '                                "<li><div class=""space""></div></li>" & _
            '                                "</ul><div></div></div>"

        ElseIf ViewState("role") = "4" And Request.QueryString("folder") = "S" Then
            If ViewState("blnFM") Then 'ie' FM can do approval
                Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" &
                        "<li><div class=""space""></div></li>" &
                                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & """><span>New Invoice</span></a></li>" &
                                            "<li><div class=""space""></div></li>" &
                                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId & "&folder=S&status=1") & """><span>Verified Invoice</span></a></li>" &
                                            "<li><div class=""space""></div></li>" &
                                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceVerified.aspx", "pageid=" & strPageId) & """><span>Approved Invoice</span></a></li>" &
                                            "<li><div class=""space""></div></li>" &
                                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" &
                                            "<li><div class=""space""></div></li>" &
                                            "</ul><div></div></div>"
            Else 'ie FM cannot do approval, equivalent to FO role
                Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" &
                                    "<li><div class=""space""></div></li>" &
                                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & """><span>New Invoice</span></a></li>" &
                                                        "<li><div class=""space""></div></li>" &
                                                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId & "&folder=S&status=1") & """><span>Verified Invoice</span></a></li>" &
                                                        "<li><div class=""space""></div></li>" &
                                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" &
                                                        "<li><div class=""space""></div></li>" &
                                                        "</ul><div></div></div>"
            End If
        ElseIf ViewState("role") = "4" Then
            If ViewState("blnFM") Then 'ie' FM can do approval
                Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" &
                          "<li><div class=""space""></div></li>" &
                                              "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & """><span>New Invoice</span></a></li>" &
                                              "<li><div class=""space""></div></li>" &
                                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId & "&folder=S&status=1") & """><span>Verified Invoice</span></a></li>" &
                                              "<li><div class=""space""></div></li>" &
                                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceVerified.aspx", "pageid=" & strPageId) & """><span>Approved Invoice</span></a></li>" &
                                              "<li><div class=""space""></div></li>" &
                                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" &
                                              "<li><div class=""space""></div></li>" &
                                              "</ul><div></div></div>"
            Else
                Session("w_InvTracking_tabs") = "<div class=""t_entity""><ul>" &
                        "<li><div class=""space""></div></li>" &
                                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & """><span>New Invoice</span></a></li>" &
                                            "<li><div class=""space""></div></li>" &
                                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId & "&folder=S&status=1") & """><span>Verified Invoice</span></a></li>" &
                                            "<li><div class=""space""></div></li>" &
                                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" &
                                            "<li><div class=""space""></div></li>" &
                                            "</ul><div></div></div>"
            End If
        End If

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

        'Zulham 24012018 -PAMB
        'Check for FO seq
        Dim seq = 0
        seq = objDb.GetVal("Select DISTINCT agfo_seq 
                            From approval_grp_fo, finance_approval
                            Where FA_INVOICE_INDEX = '" & Session("Index") & "' 
                            And FA_APPROVAL_GRP_INDEX = agfo_grp_index
                            And (agfo_fo = '" & Session("UserID") & "' or agfo_a_fo = '" & Session("UserID") & "') ")

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
                    strRemark = "Fin Approval" & strTempRemark & ": "
                End If
        End Select
        Return strRemark
    End Function

    Private Sub cmdPreviewInvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPreviewInvoice.Click
        Dim strscript As New System.Text.StringBuilder
        Dim dtItem As New DataTable
        Dim chk As CheckBox
        Dim strInvNo As String
        Dim strVen, strVen2 As String
        Dim strBuyer As String
        Dim strVenInvNo As String


        For Each dgItem As DataGridItem In dtgInvoice.Items
            chk = dgItem.FindControl("chkSelection")

            If chk.Checked Then
                'strInvNo = CType(dgItem.FindControl("lnkINVNo"), HyperLink).Text '& "'" '& strInvNo
                strInvNo = dgItem.Cells(EnumInv.icInvoiceNo).Text
                strVen = dgItem.Cells(EnumInv.icVenId).Text '& "'" '& strVen
                strVen2 = "'" & dgItem.Cells(EnumInv.icVenId).Text & "'," & strVen2
                strVenInvNo = "'" & strVen & strInvNo & "'," & strVenInvNo

            End If
        Next
        'strInvNo = Mid(strInvNo, 1, strInvNo.Length - 1)
        strVen = Mid(strVen2, 1, strVen2.Length - 1)
        strVenInvNo = Mid(strVenInvNo, 1, strVenInvNo.Length - 1)

        strscript.Append("<script language=""javascript"">")
        'strscript.Append("PopWindow('" & dDispatcher.direct("Report", "PreviewInvoice-multi.aspx", "vcomid=" & strVen.Replace("'", "\'") & "&INVNO=" & strInvNo.Replace("'", "\'") & "&BCoyID=" & Session("CompanyId") & "')"))
        strscript.Append("PopWindow('" & dDispatcher.direct("Report", "PreviewInvoice-multi.aspx", "vcomid=" & strVen.Replace("'", "\'") & "&VENINVNO=" & strVenInvNo.Replace("'", "\'") & "&BCoyID=" & Session("CompanyId") & "')"))
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())
        'cmdPreviewInvoice.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewInvoice-multi.aspx", "vcomid=" & Request(Trim("vcomid")) & "&INVNO=" & Request(Trim("INVNO"))) & "&BCoyID=" & Session("CompanyId") & "')")
    End Sub

    Private Function chkOnlyContract(ByRef intMsg As Integer) As Boolean
        Dim chk As CheckBox
        Dim objGst As New GST
        Dim objTra As New Tracking

        If objGst.chkGSTCOD() = True Then
            For Each dgItem As DataGridItem In dtgInvoice.Items
                chk = dgItem.FindControl("chkSelection")

                If chk.Checked Then

                    'Zulham 19/02/2016 - IPP Stage 4 Phase 2
                    'CN/DN can't precede the invoice it's tied to
                    'Zulham 05/01/2016 - IPP STAGE 4 Phase 2
                    'check for invoices status that are tied to CN/DN
                    Dim objippmain As New IPPMain
                    If objippmain.checkStatus(dgItem.Cells(EnumInv.icInvoiceNo).Text, dgItem.Cells(EnumInv.icVenId).Text, dgItem.Cells(EnumInv.icStatusCode).Text) = False Then
                        intMsg = 3
                        Return False
                    End If

                    If dgItem.Cells(EnumInv.icContract).Text = "N" And dgItem.Cells(EnumInv.icGSTInv).Text = "Y" Then
                        If objTra.chkInvTaxCode(dgItem.Cells(EnumInv.icIndex).Text) = True Then
                            intMsg = 1
                            Return False
                        End If
                    Else
                        If dgItem.Cells(EnumInv.icGSTInv).Text = "Y" Then
                            If objTra.chkInvTaxCode(dgItem.Cells(EnumInv.icIndex).Text, True) = True Then
                                ViewState("ErrInvNo") = dgItem.Cells(EnumInv.icInvoiceNo).Text
                                intMsg = 2
                                Return False
                            End If
                        End If
                    End If
                End If
            Next
        End If

        Return True
    End Function
End Class