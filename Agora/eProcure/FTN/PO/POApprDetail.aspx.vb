'//Outstanding
'//Calc By - Product /SubTotal
'// GST - in %
'//default - Subtotal , 0%
'//To find if the b_product_name is completely null for a PO (from D_PR),approveao.asp line 206
'//

Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing

Public Class POApprDetailFTN
    Inherits AgoraLegacy.AppBaseClass
    'Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblPO As System.Web.UI.WebControls.Label
    Protected WithEvents lblReqName As System.Web.UI.WebControls.Label
    Protected WithEvents lblPODate As System.Web.UI.WebControls.Label
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents lblBillAddr As System.Web.UI.WebControls.Label
    Protected WithEvents lblInternalRemark As System.Web.UI.WebControls.Label
    Protected WithEvents lblVendor As System.Web.UI.WebControls.Label
    Protected WithEvents lblPORemark As System.Web.UI.WebControls.Label
    Protected WithEvents lblFile As System.Web.UI.WebControls.Label
    Protected WithEvents lblCurrency As System.Web.UI.WebControls.Label
    Protected WithEvents lblPT As System.Web.UI.WebControls.Label
    Protected WithEvents lblPM As System.Web.UI.WebControls.Label
    Protected WithEvents lblST As System.Web.UI.WebControls.Label
    Protected WithEvents lblSM As System.Web.UI.WebControls.Label
    Protected WithEvents dtgAppFlow As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtgPOList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdDup As System.Web.UI.WebControls.Button
    Protected WithEvents cmdCancel As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblBuyer As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblMsg2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblcheck As System.Web.UI.WebControls.Label

    Dim dt As New DataTable
    Dim intTotRecord As Integer
    Dim intRow As Integer
    Dim intRemarkCol As Integer = 20 '19
    Dim intCnt As Integer
    Dim strC() As String
    Dim dsAllInfo, ds As DataSet
    Dim total As Double = 0
    Dim blnCustomField As Boolean = False
    Dim strGSTBy, intPRStatus As String
    Dim dblGSTPer As Double
    Dim dblNoTaxTotal, dblTaxTotal, dblTotalGst As Double
    Dim intGSTcnt, intNoGSTcnt, intTotItem As Integer
    Dim strRFQIndex As String
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim CrDate As Date

    Public Enum EnumPO
        icSNo
        icProdItem
        'icVendorItem
        'icGLCode
        icCategoryCode
        'icTaxCode
        icProdDesc
        'icMOQ
        'icMPQ
        'icRFQQty
        'icQtyToPer
        icQty
        icUOM
        icCost
        icSubTotal
        icGSTRate 'Jules
        icTax
        icGstTaxCode 'Stage 3 Enhancement (GST-0010) - 20/07/2015 - CH
        icBCM
        icDAddr
        icETD
        '  icWTerm
    End Enum

    Protected WithEvents test As System.Web.UI.WebControls.Panel
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Dim strCaller As String
    Protected WithEvents txtRemark As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdAppPO As System.Web.UI.WebControls.Button
    Protected WithEvents cmdRejectPO As System.Web.UI.WebControls.Button
    Protected WithEvents cmdHoldPO As System.Web.UI.WebControls.Button
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents tblApproval As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents trButton As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents trMessage As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents cmdPOReport As System.Web.UI.WebControls.Button
    Protected WithEvents btnHidden1 As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents txtRemarkCR As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblRemarkCR As System.Web.UI.WebControls.Label
    Protected WithEvents lblAttach As System.Web.UI.WebControls.Label
    Protected WithEvents txtExternal As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdUpload As System.Web.UI.WebControls.Button
    Protected WithEvents File1 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents pnlAttach As System.Web.UI.WebControls.Panel
    Protected WithEvents lblFileAO As System.Web.UI.WebControls.Label
    Dim dtBCM As DataTable
    Protected WithEvents hidClientId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidTotalClientId As System.Web.UI.HtmlControls.HtmlInputHidden

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
        CheckButtonAccess(True)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'If Not Page.IsPostBack Then
        MyBase.Page_Load(sender, e)

        Response.Expires = -1
        Response.AddHeader("cache-control", "private")
        Response.AddHeader("pragma", "no-cache")
        Response.CacheControl = "no-cache"

        blnPaging = False
        blnSorting = False
        SetGridProperty(dtgAppFlow)
        SetGridProperty(dtgPOList)

        If Not Page.IsPostBack Then
            GenerateTab()
            Dim objPO As New PurchaseOrder_AO
            dsAllInfo = objPO.getPOForAppr(Request.QueryString("PONO"), Request.QueryString("index"))

            'Dim companyID As String = Session("CompanyId")
            'dsAllInfo = objPR.getPR(Request.QueryString("PRNO"), Request.QueryString("index"), companyID)
            objPO = Nothing
            '//For BCM
            '//load List for whole company - for temporary only
            '//because cannot think of other better solution
            '//other reason-that particular acct may assigned to buyer when he rause PR
            '//but later it was taken out
            Dim objBCM As New BudgetControl
            'Michelle (21/9/2010) - Temporarily disable the BCM module (ExclBCM)
            'dtBCM = objBCM.getBCMListByCompany()
            objBCM = Nothing

            'Session("iDay") = "0"

            renderPRHeader()
            renderPRApprFlow()
            renderPRDetail()


            'Michelle (eBiz/303) - Allow AO to do attachment
            cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & File1.ClientID & "');")

            '//Default, override if necessary
            'lblTitle.Text = "Purchase Requisition Details"
            tblApproval.Style("display") = "none"
            tblBuyer.Style("display") = "none"
            'strCaller = UCase(Request.QueryString("caller"))
            'Select Case strCaller
            '    Case "AO" '//AO-Approved List 
            '        'lnkBack.NavigateUrl = "SearchPR_All.aspx?caller=" & strCaller & "&pageId=" & strPageId
            '        lnkBack.NavigateUrl = "javascript:history.back();"
            '        Me.EnableViewState = False
            'Case "PR" '//AO-Consolidate PR
            '    lnkBack.NavigateUrl = "PRConsolidation.aspx?pageId=" & strPageId
            '    Me.EnableViewState = False
            'Case "APPROVAL" '//AO-New Requisition
            '    Dim objBudget As New BudgetControl
            '    Dim dtBCM As New DataTable
            '    Dim strBCM As String
            '    Dim blnExceed As Boolean
            'Michelle (21/9/2010) - Temporarily disable the BCM module (ExclBCM)
            'blnExceed = objBudget.checkBCM(Request.QueryString("PRNO"), dtBCM, strBCM)

            'objBudget = Nothing

            'Michelle (21/9/2010) - Temporarily disable the BCM module (ExclBCM)
            'If blnExceed Then
            '    Common.NetMsgbox(Me, "PR value is more than the Operating Budget Amount.", MsgBoxStyle.Exclamation)
            'End If

            'lblTitle.Text = "Purchase Requisition Approval Details"
            If ViewState("ApprType") = "2" Then
                'lblTitle.Text = lblTitle.Text & " (Endorsement)"
                cmdAppPO.Text = "Endorse PR"
            End If
            tblApproval.Style("display") = ""
            '"PRDetail.aspx?caller=approval&AO=" & strAO & "&relief=" & blnRelief & "&PageID=" & strPageId & "&index=" & dv("POM_PR_Index") & "&PRNo=" & dv("POM_PR_No")
            lnkBack.NavigateUrl = dDispatcher.direct("PO", "SearchPO_AO.aspx", "pageId=" & strPageId)
            '//Can approve or not
            If ViewState("ApprType") = "1" And ViewState("ApprLimit") < dblNoTaxTotal + dblTaxTotal + dblTotalGst Then
                trButton.Style("display") = "none"
                trMessage.Style("display") = ""
                lblMsg.Text = "The amount has exceeded your approval limit."
            Else
                trButton.Style("display") = ""
                trMessage.Style("display") = "none"
            End If

            If intPRStatus = PRStatus.CancelledBy Then
                trButton.Style("display") = "none"
                trMessage.Style("display") = ""
                lblMsg.Text = "Purchase Requisition has been cancelled by the user."
            Else
                trButton.Style("display") = ""
                trMessage.Style("display") = "none"
            End If

            'Case "BUYER" '//buyer - View Requisitions
            '    '//buyer can only view own PR
            '    tblBuyer.Style("display") = ""
            '    If intPRStatus = PRStatus.Submitted Then
            '        cmdCancel.Visible = True
            '        lblRemarkCR.Visible = True
            '        txtRemarkCR.Visible = True
            '    End If
            If Request.QueryString("Frm") = "Dashboard" Then
                lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
            Else
                lnkBack.NavigateUrl = dDispatcher.direct("PO", "SearchPO_AO.aspx", "caller=" & strCaller & "&pageId=" & strPageId)
            End If
            '    Case "ADMIN" '//buyer admin - View Requisition
            '        '//admin can search by Buyer
            '        '//admin can view all PR
            '        'lnkBack.NavigateUrl = "SearchPR_ALL.aspx?caller=" & strCaller & "&pageId=" & strPageId
            '        lnkBack.NavigateUrl = "javascript:history.back();"
            '        Me.EnableViewState = False
            '    Case "OTHER"
            '        lnkBack.NavigateUrl = "javascript:history.back();"
            '        Me.EnableViewState = False
            'End Select
            hidSummary.Value = "Remarks-" & txtRemark.ClientID
        End If
        txtRemark.Attributes.Add("onKeyDown", "limitText (this, 1000);")
        cmdAppPO.Attributes.Add("onClick", "return confirmApprove('" & LCase(cmdAppPO.Text.Replace(" PO", "")) & "');")
        'cmdAppPO.Attributes.Add("onClick", "return confirmApprove('" & LCase(cmdAppPO.Text.Replace(" PO", "")) & "','" & Session("iDay") & "');")
        'cmdAppPO.Attributes.Add("onfocus", "fireHid();")
        Dim strMsg As String
        validateDatagrid(strMsg)
        cmdHoldPO.Attributes.Add("onClick", "return resetSummary(1,0);")
        cmdRejectPO.Attributes.Add("onClick", "return confirmReject();")
        cmdCancel.Attributes.Add("onClick", "return confirm('Are you sure that you want to cancel this PR ?');")

        displayAttachFile()

        'If Session("Env") = "FTN" Then
        '    Me.dtgPOList.Columns(2).Visible = False
        '    Me.dtgPOList.Columns(9).Visible = False
        '    Me.dtgAppFlow.Columns(2).Visible = False
        '    'Me.dtgPOList.Columns(5).Visible = False
        '    'Me.dtgPOList.Columns(11).Visible = False
        'Else
        '    Me.dtgPOList.Columns(2).Visible = True
        '    Me.dtgPOList.Columns(9).Visible = True
        '    Me.dtgAppFlow.Columns(2).Visible = True
        '    'Me.dtgPOList.Columns(4).Visible = True
        '    'Me.dtgPOList.Columns(5).Visible = True
        '    'Me.dtgPOList.Columns(11).Visible = True
        'End If
        ''Jules #6973
        'Dim objGST As New GST
        'Dim chkGST = objGST.chkGST(lblVendor.Text)
        'Dim blnGSTCOD = objGST.chkGSTCOD()
        'If chkGST <> "" And blnGSTCOD Then
        '    ViewState("isGST") = True
        'Else
        '    ViewState("isGST") = False
        '    Me.dtgPOList.Columns(EnumPO.icGSTRate).Visible = False
        'End If


        Me.dtgPOList.Columns(2).Visible = False
        Me.dtgPOList.Columns(11).Visible = False '10 'Jules 'from 9
        Me.dtgAppFlow.Columns(2).Visible = False

        'Check Access Status
        If Session("AccessStatus") = "Limited" Then
            cmdAppPO.Enabled = False
        End If

    End Sub
    Public Function CreateFileLinks(ByVal userID As Object, ByVal altUserID As Object, ByVal seq As String) As DataTable

        Dim id1 As String = ""
        '1. if the pass in user is is not same as current login user id, return nothing
        If Not IsDBNull(userID) Then
            id1 = CStr(userID)
        End If

        Dim id2 As String = ""
        If Not IsDBNull(altUserID) Then
            id2 = CStr(altUserID)
        End If

        Dim pr As PR = New PR
        Dim ds As DataSet = pr.getUserAttach("AO", "PO", CStr(ViewState("POIndex")), id1, id2, seq)

        Dim dt As DataTable = ds.Tables(0)

        Dim table As DataTable = New DataTable
        Dim urlCol As DataColumn = New DataColumn("Hyperlink")
        table.Columns.Add(urlCol)

        Dim fileMgr As New FileManagement
        Dim count As Integer = 1
        For Each row As DataRow In dt.Rows

            Dim strFile As String = row.Item("UA_ATTACH_FILENAME")
            Dim strFile1 As String = row.Item("UA_HUB_FILENAME")
            Dim url As String = fileMgr.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.UserAttachment, "", EnumUploadFrom.FrontOff)

            Dim r As DataRow = table.NewRow
            r.Item("Hyperlink") = CStr(count) + ") " + url
            table.Rows.Add(r)
            count = count + 1
        Next

        Return table
    End Function

    Private Sub GenerateAttachmentColumn()
        'Show the last column 
        dtgAppFlow.Columns(dtgAppFlow.Columns.Count - 1).Visible = True
    End Sub

    Private Sub renderPRHeader()
        Dim dtHeader As New DataTable
        Dim strBillAddr As String

        dtHeader = dsAllInfo.Tables("PO_MSTR")
        If dtHeader.Rows.Count > 0 Then
            'lblPO.Text = Common.parseNull(dtHeader.Rows(0)("POM_PO_NO"))

            Session("lblPO") = Common.parseNull(dtHeader.Rows(0)("POM_PO_NO"))
            If Common.parseNull(dtHeader.Rows(0)("POM_URGENT")) = "1" Then
                lblPO.Text = Session("lblPO") + " (Urgent)"
            Else
                lblPO.Text = Session("lblPO")
            End If

            ViewState("poid") = Common.parseNull(dtHeader.Rows(0)("POM_PO_NO"))
            lblReqName.Text = Common.parseNull(dtHeader.Rows(0)("POM_BUYER_NAME"), "")

            If Not IsDBNull(dtHeader.Rows(0)("POM_SUBMIT_DATE")) Then
                lblPODate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dtHeader.Rows(0)("POM_SUBMIT_DATE"))
            Else
                lblPODate.Text = ""
            End If

            Session("CrDate") = Common.FormatWheelDate(WheelDateFormat.LongDate, dtHeader.Rows(0)("POM_CREATED_DATE"))

            lblStatus.Text = Common.parseNull(dtHeader.Rows(0)("STATUS_DESC"), "")
            strBillAddr = Common.parseNull(dtHeader.Rows(0)("POM_B_ADDR_LINE1"))

            If Not IsDBNull(dtHeader.Rows(0)("POM_B_ADDR_LINE2")) AndAlso dtHeader.Rows(0)("POM_B_ADDR_LINE2") <> "" Then
                strBillAddr = strBillAddr & "<BR>" & dtHeader.Rows(0)("POM_B_ADDR_LINE2")
            End If

            If Not IsDBNull(dtHeader.Rows(0)("POM_B_ADDR_LINE3")) AndAlso dtHeader.Rows(0)("POM_B_ADDR_LINE3") <> "" Then
                strBillAddr = strBillAddr & "<BR>" & dtHeader.Rows(0)("POM_B_ADDR_LINE3")
            End If

            If Not IsDBNull(dtHeader.Rows(0)("POM_B_POSTCODE")) AndAlso dtHeader.Rows(0)("POM_B_POSTCODE") <> "" Then
                strBillAddr = strBillAddr & "<BR>" & dtHeader.Rows(0)("POM_B_POSTCODE")
            End If

            If Not IsDBNull(dtHeader.Rows(0)("POM_B_CITY")) AndAlso dtHeader.Rows(0)("POM_B_CITY") <> "" Then
                strBillAddr = strBillAddr & " " & dtHeader.Rows(0)("POM_B_CITY")
            End If

            If Not IsDBNull(dtHeader.Rows(0)("STATE")) AndAlso dtHeader.Rows(0)("STATE") <> "" Then
                strBillAddr = strBillAddr & "<BR>" & dtHeader.Rows(0)("STATE")
            End If

            If Not IsDBNull(dtHeader.Rows(0)("CT")) AndAlso dtHeader.Rows(0)("CT") <> "" Then
                strBillAddr = strBillAddr & " " & dtHeader.Rows(0)("CT")
            End If

            lblBillAddr.Text = strBillAddr & "<P>"
            lblInternalRemark.Text = Common.parseNull(dtHeader.Rows(0)("POM_INTERNAL_REMARK"), "")
            lblVendor.Text = Common.parseNull(dtHeader.Rows(0)("POM_S_COY_NAME"))
            ViewState("S_COY_ID") = Common.parseNull(dtHeader.Rows(0)("POM_S_COY_ID"))
            lblCurrency.Text = Common.parseNull(dtHeader.Rows(0)("POM_CURRENCY_CODE"))
            lblPORemark.Text = Common.parseNull(dtHeader.Rows(0)("POM_External_Remark"), "")
            lblPT.Text = Common.parseNull(dtHeader.Rows(0)("POM_PAYMENT_TERM"))
            lblPM.Text = Common.parseNull(dtHeader.Rows(0)("POM_PAYMENT_METHOD"))
            lblSM.Text = Common.parseNull(dtHeader.Rows(0)("POM_SHIPMENT_MODE"))
            lblST.Text = Common.parseNull(dtHeader.Rows(0)("POM_SHIPMENT_TERM"))
            ViewState("Requestor") = Common.parseNull(dtHeader.Rows(0)("POM_BUYER_ID"), "")
            ViewState("ShipAmt") = dtHeader.Rows(0)("POM_SHIP_AMT")

            '//Rename of Datagrid header should done before DataBind()
            strRFQIndex = Common.parseNull(dtHeader.Rows(0)("POM_RFQ_INDEX"))
            If strRFQIndex = "" Then
                'dtgPOList.Columns(EnumPO.icRFQQty).Visible = False
                'dtgPOList.Columns(EnumPO.icQtyToPer).Visible = False
                dtgPOList.Columns(EnumPO.icQty).HeaderText = "Qty"
            Else
                'dtgPOList.Columns(EnumPO.icRFQQty).Visible = True
                'dtgPOList.Columns(EnumPO.icQtyToPer).Visible = True
                'dtgPOList.Columns(EnumPO.icQty).HeaderText = "PR Qty"
                dtgPOList.Columns(EnumPO.icQty).HeaderText = "Qty"
            End If

            '//POM_GST for display only
            ' dblGSTPer = Common.parseNull(dtHeader.Rows(0)("POM_GST"), 0)
            dblGSTPer = 0
            ViewState("POIndex") = dtHeader.Rows(0)("POM_PO_Index")
            intPRStatus = dtHeader.Rows(0)("POM_PO_Status")

            If Not IsDBNull(dtHeader.Rows(0)("POM_RFQ_INDEX")) Then
                cmdDup.Enabled = False
            End If
        End If

        Dim dvFile As DataView
        Dim intLoop As Integer
        Dim strFile, strFile1, strURL, strTemp As String
        dvFile = dsAllInfo.Tables("COMPANY_DOC_ATTACHMENT").DefaultView
        If dvFile.Count > 0 Then
            For intLoop = 0 To dvFile.Count - 1
                strFile = dvFile(intLoop)("CDA_ATTACH_FILENAME")
                strFile1 = dvFile(intLoop)("CDA_HUB_FILENAME")

                '*************************meilai 25/2/05****************************
                'strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & "&doctype=PR>" & strFile & "</A>"
                Dim objFile As New FileManagement
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "PO", EnumUploadFrom.FrontOff)
                objFile = Nothing
                '*************************meilai************************************
                If strTemp = "" Then
                    strTemp = "&nbsp;" & intLoop + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDA_FILESIZE") & "KB)"
                Else
                    strTemp = strTemp & "<BR>&nbsp;" & intLoop + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDA_FILESIZE") & "KB)"
                End If

                ''//test only
                'Dim lnk As New ImageButton
                'Dim br As New Label
                'lnk.ImageUrl = "../images/i_delete2.gif"
                'lnk.ID = dvFile(intLoop)("CDA_ATTACH_INDEX")
                'br.Text = "<BR>"
                ''deleteAttach
                '' addressof
                ''lnk.ToolTip = ""
                ''Dim e1 As ImageClickEventArgs
                ''Dim ee As New EventArgs
                'AddHandler lnk.Click, AddressOf deleteAttach
                'test.Controls.Add(lnk)
                'test.Controls.Add(br)
            Next
        Else
            strTemp = "No Files Attached"
        End If
        lblFile.Text = strTemp

        '//For File Attachemnt
    End Sub
    Private Sub renderPRApprFlow()
        Dim objPR As New PurchaseReq2
        ds = objPR.getApprFlow(ViewState("POIndex"), "PO")
        objPR = Nothing
        'dtgAppFlow.DataSource = ds.Tables(0).DefaultView
        dtgAppFlow.DataSource = ds.Tables(0)
        dtgAppFlow.DataBind()
    End Sub

    Private Sub renderPRDetail()
        If dsAllInfo.Tables("PR_CUSTOM_FIELD_MSTR").Rows.Count > 0 Then
            blnCustomField = True
            dt = New DataTable
            dt = dsAllInfo.Tables("PR_CUSTOM_FIELD_DETAILS")
            addDataGridColumn()
        End If
        Bindgrid(False)

        '//RFQ
        Dim intSubTotalCol As Integer
        Dim dblSubTotal, dblGrandTotal As Double

        If strRFQIndex = "" Then
            intSubTotalCol = EnumPO.icSubTotal - 1
        Else
            intSubTotalCol = EnumPO.icSubTotal
        End If

        If ViewState("isGST") Then
            intSubTotalCol = intSubTotalCol + 1
        End If

        'If strGSTBy = "product" Then
        dblSubTotal = dblNoTaxTotal + dblTaxTotal
        dblGrandTotal = dblSubTotal + dblTotalGst + ViewState("ShipAmt")

        AddRow(intSubTotalCol, "Sub Total", CDbl(Format(dblSubTotal, "#0.00")), True)
        'Jules #6973
        If ViewState("isGST") Then
            AddRow(intSubTotalCol, "GST Amount", CDbl(Format(dblTotalGst, "#0.00")), True)
        Else
            AddRow(intSubTotalCol, "Tax", CDbl(Format(dblTotalGst, "#0.00")), True)
        End If '#6973 end.

        AddRow(intSubTotalCol, "Shipping & Handling", CDbl(Format(ViewState("ShipAmt"), "#0.00")), True)
        AddRow(intSubTotalCol, "Grand Total ", CDbl(Format(dblGrandTotal, "#0.00")), False)
        dtgPOList.Columns(EnumPO.icTax).Visible = True


        If ViewState("CurrentAppSeq") = ViewState("HighestAppr") Then
            ViewState("ISHighestLevel") = True
        Else
            ViewState("ISHighestLevel") = False
        End If
    End Sub

    Private Sub renderPRAdd()
        Dim objPO As New PurchaseOrder_AO
        dsAllInfo = objPO.getPOForAppr(Request.QueryString("PONO"), Request.QueryString("index"))

        'Dim companyID As String = Session("CompanyId")
        'dsAllInfo = objPR.getPR(Request.QueryString("PRNO"), Request.QueryString("index"), companyID)
        objPO = Nothing

        If dsAllInfo.Tables("PR_CUSTOM_FIELD_MSTR").Rows.Count > 0 Then
            blnCustomField = True
            dt = New DataTable
            dt = dsAllInfo.Tables("PR_CUSTOM_FIELD_DETAILS")
            addDataGridColumn()
        End If
        Bindgrid(False)

        '//RFQ
        Dim intSubTotalCol As Integer
        Dim dblSubTotal, dblGrandTotal As Double

        If strRFQIndex = "" Then
            intSubTotalCol = EnumPO.icSubTotal - 1
        Else
            intSubTotalCol = EnumPO.icSubTotal
        End If

        'If strGSTBy = "product" Then
        dblSubTotal = dblNoTaxTotal + dblTaxTotal
        dblGrandTotal = dblSubTotal + dblTotalGst + ViewState("ShipAmt")

        AddRow(intSubTotalCol, "Sub Total", CDbl(Format(dblSubTotal, "#0.00")), True)

        'Jules #6973
        If ViewState("isGST") Then
            AddRow(intSubTotalCol + 1, "GST Amount", CDbl(Format(dblTotalGst, "#0.00")), True)
        Else
            AddRow(intSubTotalCol, "Tax", CDbl(Format(dblTotalGst, "#0.00")), True)
        End If '#6973 end.

        AddRow(intSubTotalCol, "Shipping & Handling", CDbl(Format(ViewState("ShipAmt"), "#0.00")), True)
        AddRow(intSubTotalCol, "Grand Total ", CDbl(Format(dblGrandTotal, "#0.00")), False)
        dtgPOList.Columns(EnumPO.icTax).Visible = True

        If ViewState("CurrentAppSeq") = ViewState("HighestAppr") Then
            ViewState("ISHighestLevel") = True
        Else
            ViewState("ISHighestLevel") = False
        End If
    End Sub

    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub

    Sub AddRow(ByVal intCell As Integer, ByVal strLabel As String, ByVal dblTotal As Double, ByVal blnShowGST As Boolean)
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer

        intTotalCol = intRemarkCol + intCnt - 1

        For intL = 0 To intCell + 1 'intCell 
            addCell(row)
        Next
        '//13+3-5=11
        '//16-11 = 5

        intColToRemain = 5           '//col bf label, label, total,gst,col after total

        row.Cells(0).ColumnSpan = 3 ' intCell
        row.Cells(1).Text = strLabel
        row.Cells(1).Font.Bold = True
        row.Cells(1).HorizontalAlign = HorizontalAlign.Right
        row.Cells(3).HorizontalAlign = HorizontalAlign.Right
        row.Cells(3).Text = Format(dblTotal, "#,##0.00")
        row.Cells(3).Font.Bold = True

        row.Cells(1).ColumnSpan = 3
        row.BackColor = Color.FromName("#f4f4f4")
        dtgPOList.Controls(0).Controls.Add(row)
    End Sub
    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim dvViewPR As DataView
        dvViewPR = dsAllInfo.Tables("PO_DETAILS").DefaultView
        'dvViewSample(0)(0)
        If pSorted Then
            dvViewPR.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewPR.Sort += " DESC"
        End If

        intPageRecordCnt = dsAllInfo.Tables("PO_DETAILS").Rows.Count
        'Jules #6973
        Dim objGST As New GST
        'Dim chkGST = objGST.chkGST(ViewState("S_COY_ID"))
        Dim blnGSTCOD = objGST.chkGSTCOD()
        'If chkGST <> "" And blnGSTCOD Then
        If blnGSTCOD Then
            ViewState("isGST") = True
        Else
            ViewState("isGST") = False
            Me.dtgPOList.Columns(EnumPO.icGSTRate).Visible = False
            'Stage 3 Enhancement (GST-0010) - 20/07/2015 - CH
            Me.dtgPOList.Columns(EnumPO.icGstTaxCode).Visible = False
        End If

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            dtgPOList.DataSource = dvViewPR
            dtgPOList.DataBind()
        Else
            dtgPOList.DataBind()
        End If

    End Function

    Private Sub dtgPOList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPOList.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then     
            If ViewState("isGST") Then
                e.Item.Cells(EnumPO.icTax).Text = "GST Amount"
            Else
                e.Item.Cells(EnumPO.icTax).Text = "Tax"
            End If
        End If

        '//this line must be included
        Grid_ItemCreated(dtgPOList, e)
    End Sub

    Private Sub dtgPOList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPOList.ItemDataBound
        Dim i As Integer
        Dim intPRLine As Integer
        Dim strFieldNo, strDAddr As String
        Dim dr() As DataRow
        Dim dblAmt, dblGstAmt As Decimal

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            CrDate = Session("CrDate")
            'e.Item.Cells(EnumPO.icETD).Text = CrDate.AddDays(Common.parseNull(dv("POD_ETD")))

            Dim txtEstDate As TextBox
            txtEstDate = e.Item.FindControl("txtEstDate")
            txtEstDate.Text = CrDate.AddDays(Common.parseNull(dv("POD_ETD")))

            Dim sClientId As String, sTotalClient As String
            'simply use one of the client id to get all client ID
            sTotalClient = hidClientId.Value
            sClientId = Mid(txtEstDate.ClientID, InStr(txtEstDate.ClientID, "_") + 1, InStr(Mid(txtEstDate.ClientID, InStr(txtEstDate.ClientID, "_") + 1), "_") - 1) & "|"
            If Not sTotalClient.Contains(sClientId) Then
                hidClientId.Value = hidClientId.Value & sClientId
                hidTotalClientId.Value = hidTotalClientId.Value + 1
            End If
            'txtEstDate.Attributes.Add("onblur", "return fireHid();")

            Dim lblCDate As TextBox
            lblCDate = e.Item.FindControl("lblCDate")
            lblCDate.Text = Session("CrDate")

            Dim lblProductCode As Label
            lblProductCode = e.Item.FindControl("lblProductCode")
            lblProductCode.Text = Common.parseNull(dv("POD_PRODUCT_CODE"))

            strDAddr = Common.parseNull(dv("POD_D_ADDR_LINE1"))
            If Not IsDBNull(dv("POD_D_ADDR_LINE2")) AndAlso dv("POD_D_ADDR_LINE2") <> "" Then
                strDAddr = strDAddr & vbCrLf & dv("POD_D_ADDR_LINE2")
            End If

            If Not IsDBNull(dv("POD_D_ADDR_LINE3")) AndAlso dv("POD_D_ADDR_LINE3") <> "" Then
                strDAddr = strDAddr & vbCrLf & dv("POD_D_ADDR_LINE3")
            End If
            If Not IsDBNull(dv("POD_D_POSTCODE")) Then
                strDAddr = strDAddr & vbCrLf & dv("POD_D_POSTCODE")
            End If

            If Not IsDBNull(dv("POD_D_CITY")) Then
                strDAddr = strDAddr & " " & dv("POD_D_CITY")
            End If

            If Not IsDBNull(dv("STATE")) AndAlso dv("STATE") <> "" Then
                strDAddr = strDAddr & vbCrLf & dv("STATE")
            End If

            If Not IsDBNull(dv("CT")) Then
                strDAddr = strDAddr & " " & dv("CT")
            End If

            e.Item.Cells(EnumPO.icCost).Text = Format(Common.parseNull(dv("POD_UNIT_COST"), 0), "#,##0.0000")
            '2015-06-22: CH: Rounding issue (Prod issue)
            'dblAmt = Common.parseNull(dv("POD_UNIT_COST"), 0) * Common.parseNull(dv("POD_ORDERED_QTY"), 0)
            dblAmt = CDec(Format(Common.parseNull(dv("POD_UNIT_COST"), 0) * Common.parseNull(dv("POD_ORDERED_QTY"), 0), "###0.00"))

            'Jules #6973
            e.Item.Cells(EnumPO.icGSTRate).Text = Common.parseNull(dv("GST_RATE"), "N/A")
            'Stage 3 Enhancement (GST-0010) - 20/07/2015 - CH
            e.Item.Cells(EnumPO.icGstTaxCode).Text = Common.parseNull(dv("POD_GST_INPUT_TAX_CODE"), "N/A")

            If Common.parseNull(dv("POD_GST"), 0) = 0 Then
                'e.Item.Cells(EnumPO.icCost).Text = e.Item.Cells(EnumPO.icCost).Text & " (no tax)"
                e.Item.Cells(EnumPO.icCost).Text = e.Item.Cells(EnumPO.icCost).Text

                dblNoTaxTotal = dblNoTaxTotal + dblAmt
                intNoGSTcnt += 1
                intTotItem += 1
                '//for testing
                If Common.parseNull(dv("POD_TAX_VALUE"), 0) = 0 Then
                    e.Item.Cells(EnumPO.icTax).Text = "0.00" '"n.a."
                Else
                    e.Item.Cells(EnumPO.icTax).Text = Format(dv("POD_TAX_VALUE"), "#,##0.00")
                End If
            Else
                dblTaxTotal = dblTaxTotal + dblAmt
                intGSTcnt += 1
                intTotItem += 1
                '//for testing
                'dblGstAmt = dblAmt * (Common.parseNull(dv("POD_GST"), 0) / 100)
                'e.Item.Cells(EnumPO.icTax).Text = Format(dblGstAmt, "#,##0.00")
                If Common.parseNull(dv("POD_TAX_VALUE"), 0) = 0 Then
                    dblGstAmt = dblAmt * (Common.parseNull(dv("POD_GST"), 0) / 100)
                Else
                    dblGstAmt = Common.parseNull(dv("POD_TAX_VALUE"), 0)
                End If
                e.Item.Cells(EnumPO.icTax).Text = Format(dblGstAmt, "#,##0.00")
            End If

            '2015-06-22: CH: Rounding issue (Prod issue)
            'dblTotalGst = dblTotalGst + dblGstAmt
            dblTotalGst = dblTotalGst + CDec(format(dblGstAmt, "###0.00"))

            e.Item.Cells(EnumPO.icSubTotal).Text = Format(dblAmt, "#,##0.00")

            e.Item.Cells(EnumPO.icDAddr).ToolTip = strDAddr
            e.Item.Cells(EnumPO.icDAddr).Text = Common.parseNull(dv("POD_D_ADDR_CODE"))
            If blnCustomField Then
                intPRLine = Convert.ToInt32(e.Item.Cells(EnumPO.icSNo).Text)
                Dim lblCustom As Label
                Try
                    Dim typename As String = e.GetType().ToString()
                    'e.Item.Cells

                    If e.Item.Cells(intRemarkCol).Controls(0).GetType Is GetType(Label) Then
                        For i = 0 To intCnt - 1
                            strFieldNo = strC(i)
                            lblCustom = e.Item.Cells(intRemarkCol + i).Controls(0)
                            dr = dt.Select("PCD_FIELD_NO=" & strFieldNo & " AND PCD_PR_LINE=" & intPRLine)
                            If dr.Length > 0 Then
                                lblCustom.Text = Common.parseNull(dr(0)("PCD_FIELD_VALUE"))
                            End If
                        Next
                    End If
                Catch ex As Exception
                    'ignore
                End Try
            End If

            '//For BCM
            If Not IsDBNull(dv("POD_ACCT_INDEX")) Then
                If Not dtBCM Is Nothing Then
                    Dim drTemp As DataRow()
                    drTemp = dtBCM.Select("Acct_Index=" & dv("POD_ACCT_INDEX"))
                    If drTemp.Length > 0 Then
                        e.Item.Cells(EnumPO.icBCM).Text = drTemp(0)("Acct_List")
                    End If
                End If
            End If
        End If
    End Sub

    Private Function validateDatagrid(ByRef strMsg As String) As Boolean
        Dim dgItem As DataGridItem
        Dim iCount As Integer = 1
        validateDatagrid = True
        strMsg = "<ul type='disc'>"

        'Session("iDay") = "0"
        For Each dgItem In dtgPOList.Items
            Dim txtEstDate As TextBox
            txtEstDate = dgItem.FindControl("txtEstDate")

            Dim lblCDate As TextBox
            lblCDate = dgItem.FindControl("lblCDate")

            If Not IsDate(txtEstDate.Text) Then
                strMsg &= "<li>Item " & iCount & ". Invalid Est. Date of Delivery (dd/mm/yyyy).</li>"
                validateDatagrid = False
            Else
                Dim diffDay As TimeSpan
                Dim iDay As Integer
                Dim txtDate1 As Date = CType(lblCDate.Text, Date)
                Dim txtDate2 As Date = CType(txtEstDate.Text, Date)

                diffDay = txtDate2.Subtract(txtDate1)
                iDay = CInt(diffDay.Days)

                'If iDay < 2 Then
                '    Session("iDay") = "1"
                '    'cmdAppPO.Attributes.Add("onClick", "return confirmApprove('" & LCase(cmdAppPO.Text.Replace(" PO", "")) & "','" & ViewState("iDay") & "');")
                '    'strMsg &= "<li>Item " & iCount & ". Est. Date of Delivery Less Than 2 Days.</li>"
                '    'validateDatagrid = False
                'End If

                'Dim txtDate1 As Date = CType(lblCDate.Text, Date)
                'Dim txtDate2 As Date = CType(txtEstDate.Text, Date)
                'If Day(txtDate2) - Day(txtDate1) <= 0 Then
                '    strMsg &= "<li>Invalid Est. Date of Delivery (dd/mm/yyyy).</li>"
                '    validateDatagrid = False
                'End If
            End If
            iCount = iCount + 1
        Next
        'cmdAppPO.Attributes.Remove("onClick")
        'cmdAppPO.Attributes.Add("onClick", "return fireHid(); return confirmApprove('" & LCase(cmdAppPO.Text.Replace(" PO", "")) & "','" & Session("iDay") & "');")
        strMsg &= "</ul>"
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub dtgAppFlow_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAppFlow.ItemCreated
        Grid_ItemCreated(sender, e)
    End Sub


    Private Sub addDataGridColumn()
        Dim i As Integer
        Dim dtC As New DataTable
        dtC = dsAllInfo.Tables("PR_CUSTOM_FIELD_MSTR")

        intCnt = dtC.Rows.Count
        '//dynamicly add template column
        ReDim strC(intCnt)
        For i = 0 To intCnt - 1
            Dim col As TemplateColumn = New TemplateColumn
            col.ItemTemplate = New dgTemplate(dtC.Rows(i)("PCM_FIELD_NO"), 5)
            col.HeaderText = dtC.Rows(i)("PCM_FIELD_NAME")
            dtgPOList.Columns.AddAt(dtgPOList.Columns.Count - 1, col)
            strC(i) = dtC.Rows(i)("PCM_FIELD_NO")
        Next
    End Sub

    Private Sub dtgAppFlow_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAppFlow.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim intTotalCell, intLoop As Integer
            If dv("PRA_Seq") - 1 = dv("PRA_AO_Action") Then
                intTotalCell = e.Item.Cells.Count - 1
                For intLoop = 0 To intTotalCell
                    e.Item.Cells(intLoop).Font.Bold = True
                Next

                ViewState("CurrentAppSeq") = dv("PRA_Seq")
                ViewState("ApprType") = dv("PRA_APPROVAL_TYPE")

                '//if relief ao, how???, CANNOT USER SESSION("USERID")/
                'If strCaller = "APPROVAL" Then
                If UCase(dv("PRA_AO")) = UCase(Request.QueryString("AO")) Then
                    ViewState("ApprLimit") = Common.parseNull(dv("AO_LIMIT"), 0)
                Else
                    If Not IsDBNull(dv("PRA_A_AO")) Then
                        ViewState("ApprLimit") = Common.parseNull(dv("AAO_LIMIT"), 0)
                    Else
                        ViewState("ApprLimit") = 0
                    End If
                    'End If
                    '//Approval Type
                    '//ap=endorsement then header text=Purchase Requisition Approval (Endorsement)
                End If
            End If

            ViewState("HighestAppr") = dv("PRA_Seq")
            If dv("PRA_APPROVAL_TYPE") = 1 Then
                e.Item.Cells(3).Text = "Approval"
            Else
                e.Item.Cells(3).Text = "Endorsement"
            End If

            If IsDBNull(dv("AAO_NAME")) Then
                e.Item.Cells(2).Text = "-"
            End If

            If UCase(Common.parseNull(dv("PRA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("PRA_AO"))) Then
                e.Item.Cells(1).Font.Bold = True
            ElseIf UCase(Common.parseNull(dv("PRA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("PRA_A_AO"))) Then
                e.Item.Cells(2).Font.Bold = True
            End If

            If Not IsDBNull(dv("PRA_ACTION_DATE")) Then
                e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("PRA_ACTION_DATE"))
            End If
        End If
    End Sub
    Private Function FormatAORemark(ByVal strAction As String) As String
        Dim strRemark, strTempRemark, strUserName As String
        If Request.QueryString("relief") Then
            Dim objUsers As New Users
            Dim objUser As New User
            objUsers.GetUserDetail(Request.QueryString("AO"), Session("CompanyId"), objUser)
            strUserName = objUser.Name
            objUsers = Nothing
            objUser = Nothing
            strTempRemark = "(On Behalf of " & strUserName & ") "
        Else
            strTempRemark = ""
        End If

        Select Case strAction
            Case "approve"
                If ViewState("ApprType") = "1" Then
                    strRemark = "Approved " & strTempRemark & ": "
                Else
                    strRemark = "Endorsed " & strTempRemark & ": "
                End If
            Case "reject"
                strRemark = "Rejected" & strTempRemark & ": "
            Case "hold"
                strRemark = "Held" & strTempRemark & ": "
        End Select
        Return strRemark
    End Function

    Private Sub cmdAppPO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAppPO.Click
        '//1. check whether PO created
        '//"Select * from M_PR where PR_number = '" & strPRNum & "' and PR_Status = 4"
        '//2. AO Remark = Approved/Endorsed : Remark by User
        '//3.check whether this a highest level of AO, if yes, check consolidator
        '//if consolidator, no PO created else PO created
        '//4.
        Dim strRemark, strMsg, strUrl As String
        Dim dsPODetail As New DataSet

        strRemark = FormatAORemark("approve")
        strRemark = strRemark & txtRemark.Text

        Dim opt As Integer
        If validateDatagrid(strMsg) Then
            Dim objPO As New PurchaseOrder_AO
            dsPODetail = bindPODetail()
            strMsg = objPO.ApprovePO(Session("lblPO"), ViewState("POIndex"), ViewState("CurrentAppSeq"), ViewState("ISHighestLevel"), strRemark, ViewState("Requestor"), Request.QueryString("relief"), ViewState("ApprType"), ViewState("S_COY_ID"), dsPODetail)
            'If Request.QueryString("Frm") <> "" Then
            strUrl = lnkBack.NavigateUrl
            'Else
            'strUrl = "SearchPO_AO.aspx?pageid=" & strPageId
            'End If
            Common.NetMsgbox(Me, strMsg, strUrl, MsgBoxStyle.Exclamation)
            cmdRejectPO.Visible = False
            cmdAppPO.Visible = False
        Else
            If strMsg <> "" Then
                lblMsg2.Text = strMsg
            Else
                lblMsg2.Text = ""
            End If
        End If

    End Sub

    Private Function bindPODetail() As DataSet
        Dim ds As New DataSet
        Dim dtDetails As New DataTable

        dtDetails.Columns.Add("PONo", Type.GetType("System.String"))
        dtDetails.Columns.Add("Line", Type.GetType("System.Int32"))
        dtDetails.Columns.Add("ProductCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("VendorItemCode", Type.GetType("System.String"))
        dtDetails.Columns.Add("ProductDesc", Type.GetType("System.String"))
        dtDetails.Columns.Add("ETD", Type.GetType("System.Int32"))

        Dim dtrd As DataRow
        Dim dgItem As DataGridItem
        For Each dgItem In dtgPOList.Items
            dtrd = dtDetails.NewRow()
            dtrd("PONo") = ViewState("poid")
            dtrd("Line") = dgItem.Cells(EnumPO.icSNo).Text
            dtrd("ProductCode") = CType(dgItem.FindControl("lblProductCode"), Label).Text
            dtrd("VendorItemCode") = IIf(dgItem.Cells(EnumPO.icProdItem).Text = "", "", dgItem.Cells(EnumPO.icProdItem).Text)
            dtrd("ProductDesc") = dgItem.Cells(EnumPO.icProdDesc).Text

            Dim txtEstDate As TextBox
            txtEstDate = dgItem.FindControl("txtEstDate")

            Dim lblCDate As TextBox
            lblCDate = dgItem.FindControl("lblCDate")

            If IsDate(txtEstDate.Text) Then
                Dim txtDate1 As Date = CType(lblCDate.Text, Date)
                Dim txtDate2 As Date = CType(txtEstDate.Text, Date)
                Dim diffDay As TimeSpan

                diffDay = txtDate2.Subtract(txtDate1)
                dtrd("ETD") = CInt(diffDay.Days)
            End If

            dtDetails.Rows.Add(dtrd)
        Next
        ds.Tables.Add(dtDetails)
        bindPODetail = ds
    End Function

    Private Sub cmdRejectPO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRejectPO.Click
        Dim objPO As New PurchaseOrder_AO
        Dim strRemark, strMsg, strUrl As String
        strRemark = FormatAORemark("reject")
        strRemark = strRemark & txtRemark.Text

        If txtRemark.Text = "" Then
            Common.NetMsgbox(Me, "Please enter Remarks.", MsgBoxStyle.Information)
        Else
            strMsg = objPO.RejectPO(Session("lblPO"), ViewState("POIndex"), ViewState("CurrentAppSeq"), strRemark, Request.QueryString("AO"), Request.QueryString("relief"))
            objPO = Nothing
            ''If Request.QueryString("Frm") <> "" Then
            strUrl = lnkBack.NavigateUrl
            'Else
            '    strUrl = "SearchPO_AO.aspx?pageid=" & strPageId
            'End If
            Common.NetMsgbox(Me, strMsg, strUrl, MsgBoxStyle.Exclamation)
            cmdRejectPO.Visible = False
            cmdAppPO.Visible = False
        End If
        renderPRAdd()
    End Sub

    Private Sub cmdHoldPO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHoldPO.Click
        Dim objPO As New PurchaseOrder_AO
        Dim strRemark, strMsg As String
        Dim dsPODetail As New DataSet
        strRemark = FormatAORemark("hold")
        strRemark = strRemark & txtRemark.Text

        ' Michelle (CR0010) - Force user to enter Remarks
        'strMsg = objPR.HoldPR(lblPO.Text, ViewState("PRIndex"), ViewState("CurrentAppSeq"), strRemark, Request.QueryString("AO"), Request.QueryString("relief"))
        'objPR = Nothing
        'Common.NetMsgbox(Me, strMsg, "SearchPR_AO.aspx?pageid=" & strPageId, MsgBoxStyle.Exclamation)

        If txtRemark.Text = "" Then
            Common.NetMsgbox(Me, "Please enter Remarks.", MsgBoxStyle.Information)
        Else
            dsPODetail = bindPODetail()
            strMsg = objPO.HoldPO(Session("lblPO"), ViewState("PRIndex"), ViewState("CurrentAppSeq"), strRemark, Request.QueryString("AO"), Request.QueryString("relief"), dsPODetail)
            objPO = Nothing
            Common.NetMsgbox(Me, strMsg, dDispatcher.direct("PO", "SearchPO_AO.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
        End If

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        ' Michelle (CR0010) - Force user to enter Remarks
        If txtRemarkCR.Text = "" Then
            Common.NetMsgbox(Me, "Please enter Cancel Remarks.", MsgBoxStyle.Information)
        Else
            Dim objPR As New PurchaseReq2
            Dim strMsg As String
            strMsg = objPR.CancelPR(Session("lblPO"), ViewState("PRIndex"), ViewState("CurrentAppSeq"), txtRemarkCR.Text)
            objPR = Nothing
            Common.NetMsgbox(Me, strMsg, dDispatcher.direct("PO", "SearchPO_All.aspx", "caller=BUYER&pageid=" & strPageId), MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Sub btnHidden1_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden1.ServerClick
        'Dim strMsg As String
        'validateDatagrid(strMsg)
        Dim dgItem As DataGridItem
        lblcheck.Text = ""

        For Each dgItem In dtgPOList.Items
            Dim txtEstDate As TextBox
            txtEstDate = dgItem.FindControl("txtEstDate")

            Dim lblCDate As TextBox
            lblCDate = dgItem.FindControl("lblCDate")

            If Not IsDate(txtEstDate.Text) Then
            Else
                Dim diffDay As TimeSpan
                Dim iDay As Integer
                Dim txtDate1 As Date = CType(lblCDate.Text, Date)
                Dim txtDate2 As Date = CType(txtEstDate.Text, Date)

                diffDay = txtDate2.Subtract(txtDate1)
                iDay = CInt(diffDay.Days)

                If iDay < 2 Then
                    lblcheck.Text = iDay
                    'Session("iDay") = "1"
                    'cmdAppPO.Attributes.Add("onClick", "return confirmApprove('" & LCase(cmdAppPO.Text.Replace(" PO", "")) & "','" & ViewState("iDay") & "');")
                    'strMsg &= "<li>Item " & iCount & ". Est. Date of Delivery Less Than 2 Days.</li>"
                    'validateDatagrid = False
                End If

                'Dim txtDate1 As Date = CType(lblCDate.Text, Date)
                'Dim txtDate2 As Date = CType(txtEstDate.Text, Date)
                'If Day(txtDate2) - Day(txtDate1) <= 0 Then
                '    strMsg &= "<li>Invalid Est. Date of Delivery (dd/mm/yyyy).</li>"
                '    validateDatagrid = False
                'End If
            End If
        Next
    End Sub

    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        If File1.Value <> "" Then
            Dim objFile As New FileManagement
            Dim objPR As New PR
            Dim objDB As New EAD.DBCom

            ' Restrict user upload size
            Dim dblMaxFileSize As Double = Session("FileSize")

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(File1.PostedFile.FileName)

            If Len(sFileName) > 50 Then
                Common.NetMsgbox(Me, "File name exceeds 46 character")
            ElseIf File1.PostedFile.ContentLength > 0 And File1.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                objFile.FileUpload(File1, EnumUploadType.UserAttachment, "PO", EnumUploadFrom.FrontOff, ViewState("POIndex"), , "AO", Nothing, ViewState("CurrentAppSeq"))
            ElseIf File1.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            objPR = Nothing
            objFile = Nothing
        End If
        displayAttachFile()
        renderPRApprFlow()
        renderPRAdd()
    End Sub

    Private Sub displayAttachFile()
        Dim objPR As New PR
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        'dsAttach = objPR.getPRTempAttach(Session.SessionID)
        dsAttach = objPR.getUserAttach("AO", "PO", ViewState("POIndex"), ViewState("CurrentAppSeq"))

        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("UA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("UA_HUB_FILENAME")
                '**********************meilai 25/02/2005****************** 
                'strURL = "<A HREF=../FileDownload.aspx?pb=" & viewstate("postback") & "&file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & ">" & strFile & "</A>"
                Dim objFile As New FileManagement
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.UserAttachment, "", EnumUploadFrom.FrontOff)
                objFile = Nothing
                '**********************meilai*****************************
                Dim lblBrAO As New Label
                Dim lblBr As New Label
                Dim lblFileAO As New Label
                Dim lnk As New ImageButton

                lblFileAO.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("UA_FILESIZE") & "KB) "
                lblBrAO.Text = "<BR>"
                lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete2.gif")
                lnk.ID = drvAttach(i)("UA_ATTACH_INDEX")
                lnk.CausesValidation = False
                AddHandler lnk.Click, AddressOf deleteAttach

                pnlAttach.Controls.Add(lblFileAO)
                pnlAttach.Controls.Add(lnk)
                pnlAttach.Controls.Add(lblBrAO)
            Next
            pnlAttach.Controls.Add(New System.Web.UI.HtmlControls.HtmlGenericControl)
        Else
            Dim lblFileAO As New Label
            lblFileAO.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFileAO)
        End If
        objPR = Nothing
    End Sub

    Private Sub deleteAttach(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objPR As New PR
        objPR.deleteUserAttachment(CType(sender, ImageButton).ID)
        displayAttachFile()
        objPR = Nothing
        renderPRApprFlow()
        renderPRAdd()
    End Sub
    Private Function checkMandatory(ByRef strMsg As String) As Boolean
        strMsg = ""
        Dim objPR As New PR
        Dim intBCM As String
        intBCM = CInt(objPR.checkBCM)
        If intBCM > 0 Then
            If Not objPR.checkUserAccExist() Then
                strMsg = "You are not assigned to any Budget Account Code. ""&vbCrLf&""Please contact the Finance Manager. "
            End If
        End If
        If strMsg <> "" Then
            checkMandatory = False
        Else
            checkMandatory = True
        End If
        objPR = Nothing
    End Function

    Private Sub cmdDup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDup.Click
        Dim objPR As New PurchaseReq2
        Dim strMsg, strMsg1 As String
        If checkMandatory(strMsg1) Then
            strMsg = objPR.DuplicatePR(Session("lblPO"), ViewState("PRIndex"))
            If strMsg <> "" Then
                Common.NetMsgbox(Me, strMsg, dDispatcher.direct("PO", "SearchPO_All.aspx", "caller=BUYER&pageid=" & strPageId), MsgBoxStyle.Exclamation)
            End If
        Else
            Common.NetMsgbox(Me, strMsg1, MsgBoxStyle.Exclamation)
        End If
        objPR = Nothing
    End Sub

    Private Sub cmdPOReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPOReport.Click
        Response.Redirect(dDispatcher.direct("PR", "PRReport.aspx", "Pageid=" & strPageId & "&prno=" & Request.QueryString("PRNO") & "&index=" & ViewState("PRIndex")))
        '= "<a href=../PO/PRDetail.aspx?caller=OTHER&PageID=16&index=" & dv("POM_PR_INDEX") & "&PRNo=" & dv("POM_PR_NO") & ">" & dv("POM_PR_NO") & "</a>"
    End Sub


    'Author : Yik Foong
    'Date : 15 Oct 2009
    'Description : Return the GL Code and GL Description in format ([GL CODE]) [GL DESCRIPTION]
    'Parameters : 
    '	1. glcode : The GL Code value returned from database
    '	2. gldesc : The GL Description value returned from database
    '
    'Return :  a string represent GL Code and GL Description in 
    '		([GL CODE]) [GL_DESCRIPTION] ; 
    '		Return empty string  if both GL Code and GL Description are null 
    '		Return GL Code only if GL Description is null
    '
    Public Function GenerateGLString(ByVal glcode As Object, ByVal gldesc As Object) As String
        Dim strGLCode As String = String.Empty
        Dim strDesc As String = String.Empty

        If Not IsDBNull(glcode) Then
            strGLCode = CStr(glcode)
        End If

        If Not IsDBNull(gldesc) Then
            strDesc = CStr(gldesc)
        End If

        If strGLCode.Equals("") Then
            Return ""
        Else
            Return " (" & strGLCode & ") " & strDesc                'default return value
        End If

    End Function
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Session("w_POApprDetail_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn_selected"" href=""SearchPO_AO.aspx?pageid=" & strPageId & """><span>Approval List</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""SearchPO_ALL.aspx?pageid=" & strPageId & """><span>Approved / Rejected Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '              "</ul><div></div></div>"
        Session("w_POApprDetail_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "SearchPO_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "SearchPO_ALL.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                      "</ul><div></div></div>"
    End Sub

End Class


