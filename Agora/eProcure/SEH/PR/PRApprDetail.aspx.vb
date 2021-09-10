'//Outstanding
'//Calc By - Product /SubTotal
'// GST - in %
'//default - Subtotal , 0%
'//To find if the b_product_name is completely null for a PO (from D_PR),approveao.asp line 206
'//

Imports AgoraLegacy
Imports eProcure.Component

Imports System.Drawing

Public Class PRApprDetail_SEH
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Dim dt As New DataTable
    'Dim intTotRecord As Integer
    'Dim intRow As Integer
    'Dim intRemarkCol As Integer = 20 '19
    Dim intCnt As Integer
    Dim strC() As String
    Dim dsAllInfo, ds As DataSet
    'Dim total As Double = 0
    Dim blnCustomField As Boolean = False
    'Dim strGSTBy, 
    Dim intPRStatus As String
    'Dim dblGSTPer As Double
    Dim dblNoTaxTotal, dblTaxTotal, dblTotalGst As Double
    Dim intGSTcnt, intNoGSTcnt, intTotItem As Integer
    Dim strRFQIndex As String
    Dim CrDate As Date
    'Protected WithEvents trAdmin As System.Web.UI.HtmlControls.HtmlTableRow

    Dim objDB As New EAD.DBCom
    Dim CURR_RATE As Decimal
    Dim COMP_CURR, TEMP As String

    Public Enum EnumPR
        icSNo
        icVendor
        icVendorItem
        icGLCode
        icCategoryCode
        'icTaxCode
        icAssetCode '5
        icProdDesc
        'icMOQ
        'icMPQ
        'icRFQQty
        'icQtyToPer
        icQty
        icCommodity
        icDelCode
        icUOM
        icCurrency
        icCost
        icSubTotal
        icAmount
        icTax
        icGstRate
        icGstAmt
        icGstTaxCode
        icBCM
        icDAddr
        icItemType
        icLeadTime
        icETD
        icWTerm
        icRemark
    End Enum

    Protected WithEvents test As System.Web.UI.WebControls.Panel
    Dim strCaller As String
    Dim dtBCM As DataTable
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
        SetGridProperty(dtgPRList)

        If Not Page.IsPostBack Then
            GenerateTab()
            Dim objPR_Ext As New PurchaseReq2_Ext
            Dim objPR_PR As New PR
            dsAllInfo = objPR_Ext.getPR(Request.QueryString("PRNO"), Request.QueryString("index"))

            'Dim companyID As String = Session("CompanyId")
            'dsAllInfo = objPR.getPR(Request.QueryString("PRNO"), Request.QueryString("index"), companyID)
            objPR_Ext = Nothing
            '//For BCM
            '//load List for whole company - for temporary only
            '//because cannot think of other better solution
            '//other reason-that particular acct may assigned to buyer when he rause PR
            '//but later it was taken out
            Dim objBCM As New BudgetControl
            'Michelle (21/9/2010) - Temporarily disable the BCM module (ExclBCM)
            ViewState("BCM") = CInt(objPR_PR.checkBCM)
            'ViewState("BCM") = 1
            'dtBCM = objBCM.getBCMListByCompany()
            dtBCM = objBCM.getBCMListByCompanyNew()

            objBCM = Nothing

            renderPRHeader()
            renderPRApprFlow()
            renderPRDetail()

            cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & File1.ClientID & "');")

            ''//Default, override if necessary
            'lblTitle.Text = "Purchase Requisition Details"
            tblApproval.Style("display") = "none"
            tblBuyer.Style("display") = "none"
            'strCaller = UCase(Request.QueryString("caller"))
            'trAdmin.Style("display") = ""
            ''//although case "ao", "buyer","admin" having same setting
            ''//we write it at different case to cater for future change of setting
            'Select Case strCaller
            'Case "AO" '//AO-Approved List 
            '    'lnkBack.NavigateUrl = "SearchPR_All.aspx?caller=" & strCaller & "&pageId=" & strPageId
            '    lnkBack.NavigateUrl = "javascript:history.back();"
            '    Me.EnableViewState = False
            'Case "PR" '//AO-Consolidate PR
            '    lnkBack.NavigateUrl = dDispatcher.direct("PO", "PRConsolidation.aspx", "pageId=" & strPageId)
            '    Me.EnableViewState = False
            'Case "APPROVAL" '//AO-New Requisition
            '    Dim objBudget As New BudgetControl
            '    Dim dtBCM As New DataTable
            '    Dim strBCM As String
            '    Dim blnExceed As Boolean
            '    'Michelle (21/9/2010) - Temporarily disable the BCM module (ExclBCM)
            '    'blnExceed = objBudget.checkBCM(Request.QueryString("PRNO"), dtBCM, strBCM)

            '    objBudget = Nothing

            'Michelle (21/9/2010) - Temporarily disable the BCM module (ExclBCM)
            'If blnExceed Then
            '    Common.NetMsgbox(Me, "PR value is more than the Operating Budget Amount.", MsgBoxStyle.Exclamation)
            'End If

            ' Reopen Budget Msg
            Dim objBudget As New BudgetControl
            Dim dtBCMChk As New DataTable
            Dim strBCM As String
            Dim blnExceed As Boolean
            blnExceed = objBudget.checkBCM(Request.QueryString("PRNO"), dtBCMChk, strBCM)

            objBudget = Nothing

            If blnExceed Then
                Common.NetMsgbox(Me, "PR value is more than the Operating Budget Amount.", MsgBoxStyle.Exclamation)
            End If

            'lblTitle.Text = "Purchase Requisition Approval Details"
            'If Session("Env") <> "FTN" And ViewState("ApprType") = "2" Then
            '    'lblTitle.Text = lblTitle.Text & " (Endorsement)"
            '    cmdAppPR.Text = "Endorse PR"
            'End If
            If ViewState("ApprType") = "2" Then
                'lblTitle.Text = lblTitle.Text & " (Endorsement)"
                cmdAppPR.Text = "Endorse PR"
            End If
            tblApproval.Style("display") = ""
            '"PRDetail.aspx?caller=approval&AO=" & strAO & "&relief=" & blnRelief & "&PageID=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNo=" & dv("PRM_PR_No")
            'lnkBack.NavigateUrl = dDispatcher.direct("PR", "SearchPR_AO.aspx", "pageId=" & strPageId)

            If Session("urlreferer") = "Dashboard" Then
                lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
            ElseIf Session("urlreferer") = "SearchPRAO" Then
                lnkBack.NavigateUrl = dDispatcher.direct("PR", "SearchPR_AO.aspx", "pageId=" & strPageId)
            End If

            '//Can approve or not
            'If Session("Env") <> "FTN" And ViewState("ApprType") = "1" And ViewState("ApprLimit") < dblNoTaxTotal + dblTaxTotal + dblTotalGst Then
            '    trButton.Style("display") = "none"
            '    trMessage.Style("display") = ""
            '    lblMsg.Text = "The amount has exceeded your approval limit."
            'Else
            '    trButton.Style("display") = ""
            '    trMessage.Style("display") = "none"
            'End If

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
                lblMsg.Text = "Purchase Request has been cancelled by the user."
            Else
                trButton.Style("display") = ""
                trMessage.Style("display") = "none"
            End If

            'Case "BUYER" '//buyer - View Requisitions
            '    '//buyer can only view own PR
            '    tblBuyer.Style("display") = ""
            '    trAdmin.Style("display") = "none"
            '    If intPRStatus = PRStatus.Submitted Then
            '        cmdCancel.Visible = True
            '        lblRemarkCR.Visible = True
            '        txtRemarkCR.Visible = True
            '    End If
            '    lnkBack.NavigateUrl = dDispatcher.direct("PO", "SearchPR_ALL.aspx", "caller=" & strCaller & "&pageId=" & strPageId)
            'Case "ADMIN" '//buyer admin - View Requisition
            '    '//admin can search by Buyer
            '    '//admin can view all PR
            '    'lnkBack.NavigateUrl = "SearchPR_ALL.aspx?caller=" & strCaller & "&pageId=" & strPageId
            '    lnkBack.NavigateUrl = "javascript:history.back();"
            '    Me.EnableViewState = False
            'Case "OTHER"
            '    lnkBack.NavigateUrl = "javascript:history.back();"
            '    Me.EnableViewState = False
            'End Select
            hidSummary.Value = "Remarks-" & txtRemark.ClientID
        End If
        txtRemark.Attributes.Add("onKeyDown", "limitText (this, 900);")
        cmdAppPR.Attributes.Add("onClick", "return confirmApprove('" & LCase(cmdAppPR.Text.Replace(" PR", "")) & "');")
        cmdHoldPR.Attributes.Add("onClick", "return resetSummary(1,0);")
        cmdRejectPR.Attributes.Add("onClick", "return confirmReject();")
        cmdCancel.Attributes.Add("onClick", "return confirm('Are you sure that you want to cancel this PR ?');")

        'Yik Foong
        'Always display the uploaded file(s)
        displayAttachFile()
        'renderPRApprFlow()

        'If Session("Env") = "FTN" Then
        '    'Me.dtgPOList.Columns(2).Visible = False
        '    'Me.dtgPOList.Columns(9).Visible = False
        '    Me.dtgAppFlow.Columns(2).Visible = False
        'Else
        '    'Me.dtgPOList.Columns(2).Visible = True
        '    'Me.dtgPOList.Columns(9).Visible = True
        '    Me.dtgAppFlow.Columns(2).Visible = True
        'End If
        Me.dtgAppFlow.Columns(2).Visible = True

        'Check Access Status
        If Session("AccessStatus") = "Limited" Then
            cmdAppPR.Enabled = False
        End If

        Dim Asset As New PurchaseOrder_Buyer
        If Asset.AssetGroupMstr = False Then
            dtgPRList.Columns(EnumPR.icAssetCode).Visible = False
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
        '2. get data about the attachment
        Dim ds As DataSet = pr.getUserAttach("AO", "PR", CStr(ViewState("PRIndex")), id1, id2, seq)

        'If userID.Trim.ToLower <> CStr(ds.Tables(0).Columns("UA_USER_ID")).Trim.ToLower Then
        '	Return Nothing
        'End If


        '3. get the first table in the returned data set
        Dim dt As DataTable = ds.Tables(0)

        '4. create a datatable, and add a column into the table
        Dim table As DataTable = New DataTable
        Dim urlCol As DataColumn = New DataColumn("Hyperlink")
        table.Columns.Add(urlCol)

        '5. loop each rows of the dataset
        Dim fileMgr As New FileManagement
        Dim count As Integer = 1
        For Each row As DataRow In dt.Rows

            '6. generate the url that download the file
            Dim strFile As String = row.Item("UA_ATTACH_FILENAME")
            Dim strFile1 As String = row.Item("UA_HUB_FILENAME")
            Dim url As String = fileMgr.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.UserAttachment, "", EnumUploadFrom.FrontOff)

            '7. create a row from the newly created table, and add the hyperlink string inside
            Dim r As DataRow = table.NewRow
            r.Item("Hyperlink") = CStr(count) + ") " + url
            table.Rows.Add(r)
            count = count + 1
        Next

        Return table
    End Function

    Private Sub GenerateAttachmentColumn()
        dtgAppFlow.Columns(dtgAppFlow.Columns.Count - 1).Visible = True
    End Sub

    Private Sub renderPRHeader()
        Dim dtHeader As New DataTable
        Dim objGst As New GST
        'Dim strBillAddr As String

        dtHeader = dsAllInfo.Tables("PR_MSTR")
        If dtHeader.Rows.Count > 0 Then
            'lblPRNo.Text = Common.parseNull(dtHeader.Rows(0)("PRM_PR_NO"))

            Session("lblPRNo") = Common.parseNull(dtHeader.Rows(0)("PRM_PR_NO"))
            If Common.parseNull(dtHeader.Rows(0)("PRM_URGENT")) = "1" Then
                lblPRNo.Text = Session("lblPRNo") + " (Urgent)"
            Else
                lblPRNo.Text = Session("lblPRNo")
            End If

            lblReqName.Text = Common.parseNull(dtHeader.Rows(0)("PRM_REQ_NAME"), "nil")

            lblReqCon.Text = Common.parseNull(dtHeader.Rows(0)("PRM_REQ_PHONE"), "nil")
            lblAtt.Text = Common.parseNull(dtHeader.Rows(0)("PRM_S_ATTN"), "nil")
            lblCurr.Text = Common.parseNull(dtHeader.Rows(0)("PRM_CURRENCY_CODE"), "nil")
            lblPayTerm.Text = Common.parseNull(dtHeader.Rows(0)("PT"), "nil")
            lblPayMethod.Text = Common.parseNull(dtHeader.Rows(0)("PM"), "nil")
            'If Not IsDBNull(dtHeader.Rows(0)("PRM_PR_DATE")) Then
            '    lblPRDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dtHeader.Rows(0)("PRM_PR_DATE"))
            'Else
            '    lblPRDate.Text = "nil"
            'End If

            Session("CrDate") = Common.FormatWheelDate(WheelDateFormat.LongDate, dtHeader.Rows(0)("PRM_CREATED_DATE"))
            ViewState("PRM_PR_Type") = Common.parseNull(dtHeader.Rows(0)("PRM_PR_Type"), "nil")

            lblStatus.Text = Common.parseNull(dtHeader.Rows(0)("STATUS_DESC"), "nil")

            If UCase(Common.parseNull(dtHeader.Rows(0)("STATUS_DESC"), "")) = "HELD BY" Then
                lblStatus.Text = "Held By " & dtHeader.Rows(0)("NAME")
            End If

            'strBillAddr = Common.parseNull(dtHeader.Rows(0)("PRM_B_ADDR_LINE1"))

            'If Not IsDBNull(dtHeader.Rows(0)("PRM_B_ADDR_LINE2")) AndAlso dtHeader.Rows(0)("PRM_B_ADDR_LINE2") <> "" Then
            '    strBillAddr = strBillAddr & "<BR>" & dtHeader.Rows(0)("PRM_B_ADDR_LINE2")
            'End If

            'If Not IsDBNull(dtHeader.Rows(0)("PRM_B_ADDR_LINE3")) AndAlso dtHeader.Rows(0)("PRM_B_ADDR_LINE3") <> "" Then
            '    strBillAddr = strBillAddr & "<BR>" & dtHeader.Rows(0)("PRM_B_ADDR_LINE3")
            'End If

            'If Not IsDBNull(dtHeader.Rows(0)("PRM_B_POSTCODE")) AndAlso dtHeader.Rows(0)("PRM_B_POSTCODE") <> "" Then
            '    strBillAddr = strBillAddr & "<BR>" & dtHeader.Rows(0)("PRM_B_POSTCODE")
            'End If

            'If Not IsDBNull(dtHeader.Rows(0)("PRM_B_CITY")) AndAlso dtHeader.Rows(0)("PRM_B_CITY") <> "" Then
            '    strBillAddr = strBillAddr & " " & dtHeader.Rows(0)("PRM_B_CITY")
            'End If

            'If Not IsDBNull(dtHeader.Rows(0)("STATE")) AndAlso dtHeader.Rows(0)("STATE") <> "" Then
            '    strBillAddr = strBillAddr & "<BR>" & dtHeader.Rows(0)("STATE")
            'End If

            'If Not IsDBNull(dtHeader.Rows(0)("CT")) AndAlso dtHeader.Rows(0)("CT") <> "" Then
            '    strBillAddr = strBillAddr & " " & dtHeader.Rows(0)("CT")
            'End If

            'lblBillAddr.Text = strBillAddr & "<P>"
            txtInternal.Text = Common.parseNull(dtHeader.Rows(0)("PRM_INTERNAL_REMARK"), "nil")
            'lblVendor.Text = Common.parseNull(dtHeader.Rows(0)("PRM_S_COY_NAME"))
            ViewState("PR Vendor") = Common.parseNull(dtHeader.Rows(0)("PRM_S_COY_NAME"))

            'lblCurrency.Text = Common.parseNull(dtHeader.Rows(0)("PRM_CURRENCY_CODE"))
            'lblExRate.Text = Common.parseNull(dtHeader.Rows(0)("PRM_EXCHANGE_RATE"), "0")
            txtExternal.Text = Common.parseNull(dtHeader.Rows(0)("PRM_External_Remark"), "nil")
            'lblPT.Text = Common.parseNull(dtHeader.Rows(0)("PT"))
            'lblPM.Text = Common.parseNull(dtHeader.Rows(0)("PM"))
            'lblSM.Text = Common.parseNull(dtHeader.Rows(0)("SC"))
            'lblST.Text = Common.parseNull(dtHeader.Rows(0)("ST"))
            'ViewState("Consolidator") = Common.parseNull(dtHeader.Rows(0)("PRM_CONSOLIDATOR"), "")
            ViewState("Requestor") = Common.parseNull(dtHeader.Rows(0)("PRM_BUYER_ID"), "")

            'If ViewState("Consolidator") = "" Then
            '    trConsolidator.Visible = False
            'Else
            '    Dim objUser As New User
            '    Dim objUsers As New Users
            '    If objUsers.GetUserDetail(ViewState("Consolidator"), Session("CompanyId"), objUser) Then
            '        lblConsolidator.Text = objUser.Name
            '        trConsolidator.Visible = True
            '    End If
            '    objUser = Nothing
            '    objUsers = Nothing
            'End If

            'If IsDBNull(dtHeader.Rows(0)("CM_TAX_CALC_BY")) Then
            '    strGSTBy = "SubTotal"
            'Else
            '    If dtHeader.Rows(0)("CM_TAX_CALC_BY") = "0" Then
            '        strGSTBy = "product"
            '    Else
            '        strGSTBy = "subtotal"
            '    End If
            'End If

            ''//RFQ
            ''//Rename of Datagrid header should done before DataBind()
            'strRFQIndex = Common.parseNull(dtHeader.Rows(0)("PRM_RFQ_INDEX"))
            'If strRFQIndex = "" Then
            '    dtgPRList.Columns(EnumPR.icRFQQty).Visible = False
            '    dtgPRList.Columns(EnumPR.icQtyToPer).Visible = False
            '    dtgPRList.Columns(EnumPR.icQty).HeaderText = "Qty"
            'Else
            '    dtgPRList.Columns(EnumPR.icRFQQty).Visible = True
            '    dtgPRList.Columns(EnumPR.icQtyToPer).Visible = True
            '    dtgPRList.Columns(EnumPR.icQty).HeaderText = "PR Qty"
            'End If

            ''//PRM_GST for display only
            'dblGSTPer = Common.parseNull(dtHeader.Rows(0)("PRM_GST"), 0)
            ViewState("PRIndex") = dtHeader.Rows(0)("PRM_PR_Index")
            'intPRStatus = dtHeader.Rows(0)("PRM_PR_Status")

            If Not IsDBNull(dtHeader.Rows(0)("PRM_RFQ_INDEX")) Then
                cmdDup.Enabled = False
            End If

            If Not IsDBNull(dtHeader.Rows(0)("PRM_SUBMIT_DATE")) Then
                ViewState("GstPR") = objGst.chkGSTCOD(Format(dtHeader.Rows(0)("PRM_SUBMIT_DATE"), "dd/MM/yyyy"))
            End If

            ViewState("PRM_PR_Type") = Common.parseNull(dtHeader.Rows(0)("PRM_PR_Type"))
        End If

        Dim dvFile As DataView
        Dim intLoop, intCount As Integer
        Dim strFile, strFile1, strURL, strTemp, strTempInt As String
        dvFile = dsAllInfo.Tables("COMPANY_DOC_ATTACHMENT").DefaultView
        If dvFile.Count > 0 Then
            intLoop = 0
            For intLoop = 0 To dvFile.Count - 1
                If dvFile(intLoop)("CDA_TYPE") = "E" Then
                    strFile = dvFile(intLoop)("CDA_ATTACH_FILENAME")
                    strFile1 = dvFile(intLoop)("CDA_HUB_FILENAME")

                    '*************************meilai 25/2/05****************************
                    'strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & "&doctype=PR>" & strFile & "</A>"
                    Dim objFile As New FileManagement
                    strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "PR", EnumUploadFrom.FrontOff)
                    objFile = Nothing
                    '*************************meilai************************************
                    If strTemp = "" Then
                        strTemp = "&nbsp;" & intCount + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDA_FILESIZE") & "KB)"
                        intCount = intCount + 1
                    Else
                        strTemp = strTemp & "<BR>&nbsp;" & intCount + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDA_FILESIZE") & "KB)"
                        intCount = intCount + 1
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
                End If

            Next

            intCount = 0
            For intLoop = 0 To dvFile.Count - 1
                If dvFile(intLoop)("CDA_TYPE") = "I" Then
                    strFile = dvFile(intLoop)("CDA_ATTACH_FILENAME")
                    strFile1 = dvFile(intLoop)("CDA_HUB_FILENAME")

                    Dim objFile As New FileManagement
                    strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "PR", EnumUploadFrom.FrontOff)
                    objFile = Nothing
                    If strTempInt = "" Then
                        strTempInt = "&nbsp;" & intCount + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDA_FILESIZE") & "KB)"
                        intCount = intCount + 1
                    Else
                        strTempInt = strTempInt & "<BR>&nbsp;" & intCount + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDA_FILESIZE") & "KB)"
                        intCount = intCount + 1
                    End If

                End If

            Next
        Else
            strTemp = "No Files Attached"
            strTempInt = "No Files Attached"
        End If
        lblFile.Text = strTemp
        lblFileInt.Text = strTempInt

        ''//For File Attachemnt
    End Sub

    Private Sub renderPRApprFlow()
        Dim objPR As New PurchaseReq2
        ds = objPR.getApprFlow(ViewState("PRIndex"), "PR")
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
            intSubTotalCol = EnumPR.icSubTotal - 1
        Else
            intSubTotalCol = EnumPR.icSubTotal
        End If

        'If strGSTBy = "product" Then
        dblSubTotal = dblNoTaxTotal + dblTaxTotal
        dblGrandTotal = dblSubTotal + dblTotalGst + ViewState("ShipAmt")

        If dsAllInfo.Tables("PR_DETAILS").Rows.Count > 0 Then
            AddRow(intSubTotalCol, "Sub Total", CDbl(Format(dblSubTotal, "#0.00")), True)
            If ViewState("GstPR") = True Then
                AddRow(intSubTotalCol, "GST Amount", CDbl(Format(dblTotalGst, "#0.00")), True)
            Else
                AddRow(intSubTotalCol, "Tax", CDbl(Format(dblTotalGst, "#0.00")), True)
            End If
            ''AddRow(intSubTotalCol, "Shipping & Handling", CDbl(Format(ViewState("ShipAmt"), "#0.00")), True)
            AddRow(intSubTotalCol, "Grand Total ", CDbl(Format(dblGrandTotal, "#0.00")), False)
            'dtgPRList.Columns(EnumPR.icTax).Visible = True
        End If

        If ViewState("CurrentAppSeq") = ViewState("HighestAppr") Then
            ViewState("ISHighestLevel") = True
        Else
            ViewState("ISHighestLevel") = False
        End If

        ''//RFQ
        'Dim intSubTotalCol As Integer
        'If strRFQIndex = "" Then
        '    intSubTotalCol = EnumPR.icSubTotal - 2
        'Else
        '    intSubTotalCol = EnumPR.icSubTotal
        'End If
        ''//For Sub Total and Total with/without Tax
        ''Dim dblNoTaxTotal, dblTaxTotal, dblTotalGst As Double
        'If strGSTBy = "product" Then
        '    AddRow(intSubTotalCol, "Subtotal", dblNoTaxTotal, True)
        '    AddRow(intSubTotalCol, "Total (w/Tax)", CDbl(Format(dblNoTaxTotal, "#0.00")) + CDbl(Format(dblTotalGst, "#0.00")), False)
        'Else          '//subtotal
        '    If intNoGSTcnt = intTotItem Then             '//no gst
        '        AddRow(intSubTotalCol, "Subtotal (Not Taxable)", dblNoTaxTotal, False)
        '        AddRow(intSubTotalCol, "Total", CDbl(Format(dblNoTaxTotal, "#0.00")), False)
        '    ElseIf intGSTcnt = intTotItem Then           '//all gst
        '        AddRow(intSubTotalCol, "Subtotal (Taxable)", dblTaxTotal, False)
        '        AddRow(intSubTotalCol, "Tax", dblTotalGst, False)
        '        AddRow(intSubTotalCol, "Total", CDbl(Format(dblTaxTotal, "#0.00")) + CDbl(Format(dblTotalGst, "#0.00")), False)
        '    Else             'mix
        '        AddRow(intSubTotalCol, "Subtotal (Not Taxable)", dblNoTaxTotal, False)
        '        AddRow(intSubTotalCol, "Subtotal (Taxable)", dblTaxTotal, False)
        '        AddRow(intSubTotalCol, "Tax", dblTotalGst, False)
        '        AddRow(intSubTotalCol, "Total", CDbl(Format(dblNoTaxTotal, "#0.00")) + CDbl(Format(dblTaxTotal, "#0.00")) + CDbl(Format(dblTotalGst, "#0.00")), False)
        '    End If
        'End If

        ''//Show/Hide 'Tax' column
        'If strGSTBy = "product" Then
        '    dtgPRList.Columns(EnumPR.icTax).Visible = True
        '    'intRemarkCol += 1
        'Else
        '    dtgPRList.Columns(EnumPR.icTax).Visible = False
        'End If

        'If ViewState("CurrentAppSeq") = ViewState("HighestAppr") Then
        '    ViewState("ISHighestLevel") = True
        'Else
        '    ViewState("ISHighestLevel") = False
        'End If
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
        intCell = EnumPR.icSubTotal

        intTotalCol = EnumPR.icRemark + intCnt - 1

        For intL = 0 To intTotalCol
            addCell(row)
        Next

        intColToRemain = 4
        For intL = 0 To intTotalCol - intColToRemain
            row.Cells.RemoveAt(1)
        Next

        row.Cells(0).ColumnSpan = intCell - 9 '8
        Dim Asset As New PurchaseOrder_Buyer
        row.Cells(1).ColumnSpan = intCell - 6 '2
        If Asset.AssetGroupMstr = False Then
            row.Cells(1).ColumnSpan = intCell - 7
        End If

        If ViewState("PRM_PR_Type") = "CC" Then
            row.Cells(1).ColumnSpan = intCell - 5 '2
            If Asset.AssetGroupMstr = False Then
                row.Cells(1).ColumnSpan = intCell - 6
            End If
        Else

        End If

        row.Cells(1).Text = strLabel
        row.Cells(1).Font.Bold = True
        row.Cells(1).HorizontalAlign = HorizontalAlign.Right
        row.Cells(2).ColumnSpan = 2
        row.Cells(2).HorizontalAlign = HorizontalAlign.Right
        row.Cells(2).Text = Format(dblTotal, "#,##0.00")
        row.Cells(2).Font.Bold = True

        row.Cells(3).ColumnSpan = intTotalCol - (intCell - 8)
        row.BackColor = Color.FromName("#f4f4f4")
        dtgPRList.Controls(0).Controls.Add(row)

        ' ''adding totals row
        ''Dim intL, intColToRemain As Integer
        ''Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        ''Dim intTotalCol As Integer

        ''intTotalCol = EnumPR.icRemark + intCnt - 1

        ''For intL = 0 To intCell + 1
        ''    addCell(row)
        ''Next

        ''intColToRemain = 5

        ''row.Cells(0).ColumnSpan = 3
        ''row.Cells(1).Text = strLabel
        ''row.Cells(1).Font.Bold = True
        ''row.Cells(1).HorizontalAlign = HorizontalAlign.Right
        ''row.Cells(3).HorizontalAlign = HorizontalAlign.Right
        ''row.Cells(3).Text = Format(dblTotal, "#,##0.00")
        ''row.Cells(3).Font.Bold = True

        ''row.Cells(1).ColumnSpan = 3
        ''row.BackColor = Color.FromName("#f4f4f4")
        ''dtgPRList.Controls(0).Controls.Add(row)


        ''adding totals row
        'Dim intL, intColToRemain As Integer
        'Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        'Dim intTotalCol As Integer

        'intTotalCol = intRemarkCol + intCnt - 1

        'For intL = 0 To intTotalCol
        '    addCell(row)
        'Next
        ''//13+3-5=11
        ''//16-11 = 5

        'If strGSTBy = "product" And blnShowGST Then
        '    intColToRemain = 5           '//col bf label, label, total,gst,col after total
        'Else
        '    intColToRemain = 4           '//col bf label, label, total,col after total
        'End If

        'For intL = 0 To intTotalCol - intColToRemain
        '    row.Cells.RemoveAt(1)
        'Next

        'row.Cells(0).ColumnSpan = intCell - 1
        ''Dim dg As DataGridItem
        ''total = 0
        ''For Each dg In dtgPRList.Items
        ''    total += Decimal.Parse(dg.Cells(intCell).Text)
        ''Next
        'row.Cells(1).Text = strLabel
        'row.Cells(1).Font.Bold = True
        'row.Cells(2).HorizontalAlign = HorizontalAlign.Right
        'row.Cells(2).Text = Format(dblTotal, "#,##0.00")
        'row.Cells(2).Font.Bold = True

        'If strGSTBy = "product" And blnShowGST Then
        '    row.Cells(3).Font.Bold = True
        '    row.Cells(3).HorizontalAlign = HorizontalAlign.Right
        '    If dblTotalGst = 0 Then
        '        row.Cells(3).Text = "0.00"              '"n.a."
        '    Else
        '        row.Cells(3).Text = Format(dblTotalGst, "#,##0.00")
        '    End If
        '    row.Cells(4).ColumnSpan = intTotalCol - (intCell - 1)
        'Else
        '    row.Cells(3).ColumnSpan = intTotalCol - (intCell - 1)
        'End If

        ''row.CssClass = "BODY"
        'row.BackColor = Color.FromName("#f4f4f4")
        'dtgPRList.Controls(0).Controls.Add(row)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim dvViewPR As DataView
        dvViewPR = dsAllInfo.Tables("PR_DETAILS").DefaultView
        'dvViewSample(0)(0)
        If pSorted Then
            dvViewPR.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewPR.Sort += " DESC"
        End If

        intPageRecordCnt = dsAllInfo.Tables("PR_DETAILS").Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            dtgPRList.DataSource = dvViewPR
            dtgPRList.DataBind()
        Else
            dtgPRList.DataBind()
        End If

        'If Session("Env") = "FTN" Then
        '    Me.dtgPRList.Columns(EnumPR.icBCM).Visible = False
        '    Me.dtgPRList.Columns(EnumPR.icWTerm).Visible = False
        'Else
        '    If ViewState("BCM") <= 0 Then
        '        Me.dtgPRList.Columns(EnumPR.icBCM).Visible = False
        '    Else
        '        Me.dtgPRList.Columns(EnumPR.icBCM).Visible = True
        '    End If
        '    Me.dtgPRList.Columns(EnumPR.icWTerm).Visible = True
        'End If

        If ViewState("GstPR") = True Then
            Me.dtgPRList.Columns(EnumPR.icTax).Visible = False
            Me.dtgPRList.Columns(EnumPR.icGstRate).Visible = True
            Me.dtgPRList.Columns(EnumPR.icGstAmt).Visible = True
            Me.dtgPRList.Columns(EnumPR.icGstTaxCode).Visible = True
        Else
            Me.dtgPRList.Columns(EnumPR.icTax).Visible = True
            Me.dtgPRList.Columns(EnumPR.icGstRate).Visible = False
            Me.dtgPRList.Columns(EnumPR.icGstAmt).Visible = False
            Me.dtgPRList.Columns(EnumPR.icGstTaxCode).Visible = False
        End If

        If ViewState("BCM") <= 0 Then
            Me.dtgPRList.Columns(EnumPR.icBCM).Visible = False
        Else
            Me.dtgPRList.Columns(EnumPR.icBCM).Visible = True
        End If
        Me.dtgPRList.Columns(EnumPR.icWTerm).Visible = True

    End Function

    Private Sub dtgPRList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPRList.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgPRList, e)

        If e.Item.ItemType = ListItemType.Header Then
            If ViewState("PRM_PR_Type") <> "CC" Then
                e.Item.Cells(EnumPR.icCost).Text = "Last Txn. Price"
            Else
                e.Item.Cells(EnumPR.icCost).Text = "Contract Price"
            End If
        End If
    End Sub

    Private Sub dtgPRList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPRList.ItemDataBound
        Dim i As Integer
        Dim objDB As New EAD.DBCom
        Dim intPRLine As Integer
        Dim strFieldNo, strDAddr As String
        Dim dr() As DataRow
        Dim dblAmt, dblGstAmt As Double
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            CrDate = Session("CrDate")
            e.Item.Cells(EnumPR.icETD).Text = CrDate.AddDays(Common.parseNull(dv("PRD_ETD")))


            'Michelle (3/12/2011) - To cater for old data, show the Vendor from the PR mstr if the pr is raisd from RFQ free form
            If ViewState("PRM_PR_Type") = "CC" Then
                e.Item.Cells(EnumPR.icVendor).Visible = True
                If Common.parseNull(dv("CM_COY_NAME")) = "" Then
                    e.Item.Cells(EnumPR.icVendor).Text = Common.parseNull(ViewState("PR Vendor"))
                End If
            Else
                e.Item.Cells(EnumPR.icVendor).Visible = False
            End If

            strDAddr = Common.parseNull(dv("PRD_D_ADDR_LINE1"))
            If Not IsDBNull(dv("PRD_D_ADDR_LINE2")) AndAlso dv("PRD_D_ADDR_LINE2") <> "" Then
                strDAddr = strDAddr & vbCrLf & dv("PRD_D_ADDR_LINE2")
            End If

            If Not IsDBNull(dv("PRD_D_ADDR_LINE3")) AndAlso dv("PRD_D_ADDR_LINE3") <> "" Then
                strDAddr = strDAddr & vbCrLf & dv("PRD_D_ADDR_LINE3")
            End If
            If Not IsDBNull(dv("PRD_D_POSTCODE")) Then
                strDAddr = strDAddr & vbCrLf & dv("PRD_D_POSTCODE")
            End If

            If Not IsDBNull(dv("PRD_D_CITY")) Then
                strDAddr = strDAddr & " " & dv("PRD_D_CITY")
            End If

            If Not IsDBNull(dv("STATE")) AndAlso dv("STATE") <> "" Then
                strDAddr = strDAddr & vbCrLf & dv("STATE")
            End If

            If Not IsDBNull(dv("CT")) Then
                strDAddr = strDAddr & " " & dv("CT")
            End If

            e.Item.Cells(EnumPR.icGstRate).Text = Common.parseNull(dv("GST_RATE"))
            e.Item.Cells(EnumPR.icCost).Text = Format(Common.parseNull(dv("PRD_UNIT_COST"), 0), "#,##0.0000")
            '2015-06-17: CH: Rounding issue (Prod issue)
            'dblAmt = Common.parseNull(dv("PRD_UNIT_COST"), 0) * Common.parseNull(dv("PRD_ORDERED_QTY"), 0)
            dblAmt = CDec(Format(Common.parseNull(dv("PRD_UNIT_COST"), 0) * Common.parseNull(dv("PRD_ORDERED_QTY"), 0), "###0.00"))

            If Common.parseNull(dv("PRD_GST"), 0) = 0 Then
                'e.Item.Cells(EnumPO.icCost).Text = e.Item.Cells(EnumPO.icCost).Text & " (no tax)"
                e.Item.Cells(EnumPR.icCost).Text = e.Item.Cells(EnumPR.icCost).Text
                dblNoTaxTotal = dblNoTaxTotal + dblAmt
                intNoGSTcnt += 1
                intTotItem += 1
                '//for testing
                e.Item.Cells(EnumPR.icTax).Text = "0.00"                    '"n.a."
                e.Item.Cells(EnumPR.icGstAmt).Text = "0.00"
            Else
                dblTaxTotal = dblTaxTotal + dblAmt
                intGSTcnt += 1
                intTotItem += 1
                '//for testing
                dblGstAmt = dblAmt * (Common.parseNull(dv("PRD_GST"), 0) / 100)
                e.Item.Cells(EnumPR.icTax).Text = Format(dblGstAmt, "#,##0.00")
                e.Item.Cells(EnumPR.icGstAmt).Text = Format(dblGstAmt, "#,##0.00")
            End If

            '2015-06-17: CH: Rounding issue (Prod issue)
            'dblTotalGst = dblTotalGst + dblGstAmt
            dblTotalGst = dblTotalGst + CDec(Format(dblGstAmt, "###0.00"))

            e.Item.Cells(EnumPR.icSubTotal).Text = Format(dblAmt, "#,##0.00")

            'If IsDBNull(dv("PRD_CT_ID")) Then
            '    e.Item.Cells(EnumPR.icCommodity).Text = ""
            'Else
            '    e.Item.Cells(EnumPR.icCommodity).Text = objDB.GetVal("SELECT CT_NAME FROM COMMODITY_TYPE WHERE CT_ID = '" & dv("PRD_CT_ID") & "'")
            'End If

            If IsDBNull(dv("PRD_ITEM_TYPE")) Then
                e.Item.Cells(EnumPR.icItemType).Text = ""
            Else
                If Common.parseNull(dv("PRD_ITEM_TYPE")) = "SP" Then
                    e.Item.Cells(EnumPR.icItemType).Text = "Spot"
                ElseIf Common.parseNull(dv("PRD_ITEM_TYPE")) = "ST" Then
                    e.Item.Cells(EnumPR.icItemType).Text = "Stock"
                ElseIf Common.parseNull(dv("PRD_ITEM_TYPE")) = "MI" Then
                    e.Item.Cells(EnumPR.icItemType).Text = "MRO"
                End If
            End If

            e.Item.Cells(EnumPR.icDAddr).ToolTip = strDAddr
            e.Item.Cells(EnumPR.icDAddr).Text = Common.parseNull(dv("PRD_D_ADDR_CODE"))
            If blnCustomField Then
                intPRLine = Convert.ToInt32(e.Item.Cells(EnumPR.icSNo).Text)
                Dim lblCustom As Label
                Try
                    Dim typename As String = e.GetType().ToString()
                    'e.Item.Cells

                    If e.Item.Cells(EnumPR.icRemark).Controls(0).GetType Is GetType(Label) Then
                        For i = 0 To intCnt - 1
                            strFieldNo = strC(i)
                            lblCustom = e.Item.Cells(EnumPR.icRemark + i).Controls(0)
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
            If Not IsDBNull(dv("PRD_ACCT_INDEX")) Then
                If Not dtBCM Is Nothing Then
                    Dim drTemp As DataRow()
                    drTemp = dtBCM.Select("Acct_Index=" & dv("PRD_ACCT_INDEX"))
                    If drTemp.Length > 0 Then
                        e.Item.Cells(EnumPR.icBCM).Text = Mid(drTemp(0)("Acct_List"), 1, 10)
                        e.Item.Cells(EnumPR.icBCM).ToolTip = drTemp(0)("Acct_List")
                    End If
                End If
            End If

            'GST Tax Code
            'Stage 3 Enhancement (GST-0010) - 14/07/2015 - CH
            If Common.parseNull(dv("PRD_GST_INPUT_TAX_CODE")) <> "" Then
                e.Item.Cells(EnumPR.icGstTaxCode).Text = dv("PRD_GST_INPUT_TAX_CODE")
            Else
                e.Item.Cells(EnumPR.icGstTaxCode).Text = "N/A"
            End If
        ElseIf e.Item.ItemType = ListItemType.Header Then
            If ViewState("BCM") <= 0 Then
                e.Item.Cells(EnumPR.icBCM).Visible = False
            End If
            If ViewState("PRM_PR_Type") <> "CC" Then
                e.Item.Cells(EnumPR.icVendor).Visible = False
            Else
                e.Item.Cells(EnumPR.icVendor).Visible = True
            End If
        End If

        ''intGSTcnt, intNoGSTcnt, intTotItem 
        'e.Item.Cells(EnumPR.icCost).Text = Format(Common.parseNull(dv("PRD_UNIT_COST"), 0), "#,##0.0000")
        'dblAmt = Common.parseNull(dv("PRD_UNIT_COST"), 0) * Common.parseNull(dv("PRD_ORDERED_QTY"), 0)
        'If strGSTBy = "subtotal" Then
        '    If Common.parseNull(dv("PRD_GST"), 0) = 0 Then
        '        e.Item.Cells(EnumPR.icCost).Text = e.Item.Cells(EnumPR.icCost).Text & " (no tax)"
        '        dblNoTaxTotal = dblNoTaxTotal + dblAmt
        '        intNoGSTcnt += 1
        '        intTotItem += 1
        '        '//for testing
        '        e.Item.Cells(EnumPR.icTax).Text = "0.0000"                    '"n.a."
        '    Else
        '        dblTaxTotal = dblTaxTotal + dblAmt
        '        intGSTcnt += 1
        '        intTotItem += 1
        '        '//for testing
        '        dblGstAmt = dblAmt * (Common.parseNull(dv("PRD_GST"), 0) / 100)
        '        e.Item.Cells(EnumPR.icTax).Text = Format(dblGstAmt, "#,##0.0000")
        '    End If
        'Else
        '    dblNoTaxTotal = dblNoTaxTotal + dblAmt
        '    If dv("PRD_GST") = 0 Then
        '        e.Item.Cells(EnumPR.icTax).Text = "0.0000"                    '"n.a."
        '    Else
        '        dblGstAmt = dblAmt * (Common.parseNull(dv("PRD_GST"), 0) / 100)
        '        e.Item.Cells(EnumPR.icTax).Text = Format(dblGstAmt, "#,##0.0000")
        '    End If
        'End If

        'dblTotalGst = dblTotalGst + dblGstAmt

        ''   Dim dblNoTaxTotal, dblTaxTotal, dblTotalGst As Double
        'e.Item.Cells(EnumPR.icSubTotal).Text = Format(dblAmt, "#,##0.0000")

        'e.Item.Cells(EnumPR.icDAddr).ToolTip = strDAddr
        'e.Item.Cells(EnumPR.icDAddr).Text = Common.parseNull(dv("PRD_D_ADDR_CODE"))
        'If blnCustomField Then
        '    intPRLine = Convert.ToInt32(e.Item.Cells(EnumPR.icSNo).Text)
        '    Dim lblCustom As Label
        '    Try
        '        Dim typename As String = e.GetType().ToString()
        '        'e.Item.Cells

        '        If e.Item.Cells(intRemarkCol).Controls(0).GetType Is GetType(Label) Then
        '            For i = 0 To intCnt - 1
        '                strFieldNo = strC(i)
        '                lblCustom = e.Item.Cells(intRemarkCol + i).Controls(0)
        '                dr = dt.Select("PCD_FIELD_NO=" & strFieldNo & " AND PCD_PR_LINE=" & intPRLine)
        '                If dr.Length > 0 Then
        '                    lblCustom.Text = Common.parseNull(dr(0)("PCD_FIELD_VALUE"))
        '                End If
        '            Next
        '        End If
        '    Catch ex As Exception
        '        'ignore
        '    End Try
        'End If

        ''//For BCM
        'If Not IsDBNull(dv("PRD_ACCT_INDEX")) Then
        '    If Not dtBCM Is Nothing Then
        '        Dim drTemp As DataRow()
        '        drTemp = dtBCM.Select("Acct_Index=" & dv("PRD_ACCT_INDEX"))
        '        If drTemp.Length > 0 Then
        '            e.Item.Cells(EnumPR.icBCM).Text = drTemp(0)("Acct_List")
        '        End If
        '    End If
        'End If
    End Sub

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
            dtgPRList.Columns.AddAt(dtgPRList.Columns.Count - 1, col)
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
                If strCaller = "APPROVAL" Then
                    If UCase(dv("PRA_AO")) = UCase(Request.QueryString("AO")) Then
                        ViewState("ApprLimit") = Common.parseNull(dv("AO_LIMIT"), 0)
                    Else
                        If Not IsDBNull(dv("PRA_A_AO")) Then
                            ViewState("ApprLimit") = Common.parseNull(dv("AAO_LIMIT"), 0)
                        Else
                            ViewState("ApprLimit") = 0
                        End If
                    End If
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

    Private Sub cmdAppPR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAppPR.Click
        '//1. check whether PO created
        '//"Select * from M_PR where PR_number = '" & strPRNum & "' and PR_Status = 4"
        '//2. AO Remark = Approved/Endorsed : Remark by User
        '//3.check whether this a highest level of AO, if yes, check consolidator
        '//if consolidator, no PO created else PO created
        '//4.
        Dim strRemark, strMsg As String
        Dim objPR As New PurchaseReq2
        Dim objPR_Ext As New PurchaseReq2_Ext
        Dim chk As Boolean

        strRemark = FormatAORemark("approve")
        strRemark = strRemark & txtRemark.Text
        If ViewState("PRM_PR_Type") <> "CC" Then
            chk = objPR.ValidatePOUserAssign(Session("lblPRNo"))
        Else
            chk = True
        End If

        If ViewState("PRM_PR_Type") = "CC" Then
            'Get latest Exchange Rate            
            TEMP = objDB.GetVal("SELECT CE_RATE FROM company_exchangerate WHERE CE_COY_ID = '" & Session("CompanyId") & "' AND CE_CURRENCY_CODE = '" & lblCurr.Text & "' AND CE_DELETED='N' AND CE_VALID_FROM <= CURRENT_DATE() AND CE_VALID_TO >= CURRENT_DATE()")
            If TEMP = "" Then
                COMP_CURR = objDB.GetVal("SELECT CM_CURRENCY_CODE FROM company_mstr WHERE CM_COY_ID = '" & Session("CompanyId") & "'")

                If COMP_CURR = lblCurr.Text Then
                    CURR_RATE = 1
                    'validateDatagrid = True
                Else
                    CURR_RATE = 0
                    Common.NetMsgbox(Me, "PO creation is not allowed due to invalid / expired exchange rate.")
                    Exit Sub
                End If
            Else
                'validateDatagrid = True
                CURR_RATE = CDec(TEMP)
            End If
            CURR_RATE = Format(CURR_RATE, "##0.000000")
        Else
            CURR_RATE = Format(1, "##0.000000")
        End If

        If chk = True Then
            strMsg = objPR_Ext.ApprovePR(Session("lblPRNo"), ViewState("PRIndex"), ViewState("CurrentAppSeq"), ViewState("ISHighestLevel"), ViewState("Consolidator"), strRemark, ViewState("Requestor"), Request.QueryString("relief"), ViewState("ApprType"), "", "APP", CURR_RATE)

            If Session("urlreferer") = "Dashboard" Then
                Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
            Else
                Common.NetMsgbox(Me, strMsg, dDispatcher.direct("PR", "SearchPR_AO.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
            End If

            'strUrl = lnkBack.NavigateUrl
            'Common.NetMsgbox(Me, strMsg, strUrl, MsgBoxStyle.Exclamation)
            cmdRejectPR.Visible = False
            cmdAppPR.Visible = False
            cmdHoldPR.Visible = False
        Else
            Common.NetMsgbox(Me, "No Commodity Type assign to Purchase Officer.")
            renderPRAdd()
            cmdRejectPR.Visible = True
            cmdAppPR.Visible = True
            cmdHoldPR.Visible = False
        End If
    End Sub

    Private Sub cmdRejectPR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRejectPR.Click
        Dim objPR As New PurchaseReq2
        Dim strRemark, strMsg As String
        strRemark = FormatAORemark("reject")
        strRemark = strRemark & txtRemark.Text

        If txtRemark.Text = "" Then
            Common.NetMsgbox(Me, "Please enter Remarks.", MsgBoxStyle.Information)
        Else
            strMsg = objPR.RejectPR(Session("lblPRNo"), ViewState("PRIndex"), ViewState("CurrentAppSeq"), strRemark, Request.QueryString("AO"), Request.QueryString("relief"))
            objPR = Nothing
            'strUrl = lnkBack.NavigateUrl
            'Common.NetMsgbox(Me, strMsg, strUrl, MsgBoxStyle.Exclamation)
            Common.NetMsgbox(Me, strMsg, dDispatcher.direct("PR", "SearchPR_AO.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
            cmdRejectPR.Visible = False
            cmdAppPR.Visible = False
            cmdHoldPR.Visible = False
        End If
        renderPRAdd()
    End Sub

    Private Sub cmdHoldPR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHoldPR.Click
        'Dim objPR As New PurchaseReq2
        'Dim strRemark, strMsg As String
        'strRemark = FormatAORemark("hold")
        'strRemark = strRemark & txtRemark.Text

        '' Michelle (CR0010) - Force user to enter Remarks
        ''strMsg = objPR.HoldPR(lblPR.Text, ViewState("PRIndex"), ViewState("CurrentAppSeq"), strRemark, Request.QueryString("AO"), Request.QueryString("relief"))
        ''objPR = Nothing
        ''Common.NetMsgbox(Me, strMsg, "SearchPR_AO.aspx?pageid=" & strPageId, MsgBoxStyle.Exclamation)

        'If txtRemark.Text = "" Then
        '    Common.NetMsgbox(Me, "Please enter Remarks.", MsgBoxStyle.Information)
        'Else
        '    strMsg = objPR.HoldPR(lblPR.Text, ViewState("PRIndex"), ViewState("CurrentAppSeq"), strRemark, Request.QueryString("AO"), Request.QueryString("relief"))
        '    objPR = Nothing
        '    Common.NetMsgbox(Me, strMsg, dDispatcher.direct("PO", "SearchPR_AO.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)

        'End If

        Dim objPR As New PurchaseReq2
        Dim strRemark, strMsg, strUrl As String
        strRemark = FormatAORemark("hold")
        strRemark = strRemark & txtRemark.Text

        If txtRemark.Text = "" Then
            Common.NetMsgbox(Me, "Please enter Remarks.", MsgBoxStyle.Information)
        Else
            strMsg = objPR.HoldPR(Session("lblPRNo"), ViewState("PRIndex"), ViewState("CurrentAppSeq"), strRemark, Request.QueryString("AO"), Request.QueryString("relief"))
            objPR = Nothing
            strUrl = lnkBack.NavigateUrl
            Common.NetMsgbox(Me, strMsg, strUrl, MsgBoxStyle.Exclamation)
            'cmdRejectPR.Visible = False
            'cmdAppPR.Visible = False
            'cmdHoldPR.Visible = False
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        ' Michelle (CR0010) - Force user to enter Remarks
        'If txtRemarkCR.Text = "" Then
        '    Common.NetMsgbox(Me, "Please enter Cancel Remarks.", MsgBoxStyle.Information)
        'Else
        '    Dim objPR As New PurchaseReq2
        '    Dim strMsg As String
        '    strMsg = objPR.CancelPR(lblPRNo.Text, ViewState("PRIndex"), ViewState("CurrentAppSeq"), txtRemarkCR.Text)
        '    objPR = Nothing
        '    Common.NetMsgbox(Me, strMsg, dDispatcher.direct("PR", "SearchPR_All.aspx", "caller=BUYER&pageid=" & strPageId), MsgBoxStyle.Exclamation)
        'End If
    End Sub

    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        If File1.Value <> "" Then
            Dim objFile As New FileManagement
            Dim objPR As New PR
            Dim objDB As New EAD.DBCom

            ' Restrict user upload size
            Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'USER' AND SP_PARAM_NAME = 'DocAttachSize'"))

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(File1.PostedFile.FileName)

            If Len(sFileName) > 50 Then
                Common.NetMsgbox(Me, "File name exceeds 46 character")
            ElseIf File1.PostedFile.ContentLength > 0 And File1.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                objFile.FileUpload(File1, EnumUploadType.UserAttachment, "PR", EnumUploadFrom.FrontOff, ViewState("PRIndex"), , "AO", Nothing, ViewState("CurrentAppSeq"))
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
        dsAttach = objPR.getUserAttach("AO", "PR", ViewState("PRIndex"), ViewState("CurrentAppSeq"))

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

    Private Sub renderPRAdd()
        'Dim objPO As New PurchaseOrder_AO
        'dsAllInfo = objPO.getPOForAppr(Request.QueryString("PRNO"), Request.QueryString("index"))

        Dim objPR As New PurchaseReq2_Ext
        dsAllInfo = objPR.getPR(Request.QueryString("PRNO"), Request.QueryString("index"))

        'Dim companyID As String = Session("CompanyId")
        'dsAllInfo = objPR.getPR(Request.QueryString("PRNO"), Request.QueryString("index"), companyID)
        objPR = Nothing

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
            intSubTotalCol = EnumPR.icSubTotal - 1
        Else
            intSubTotalCol = EnumPR.icSubTotal
        End If

        'If strGSTBy = "product" Then
        dblSubTotal = dblNoTaxTotal + dblTaxTotal
        dblGrandTotal = dblSubTotal + dblTotalGst + ViewState("ShipAmt")

        AddRow(intSubTotalCol, "Sub Total", CDbl(Format(dblSubTotal, "#0.00")), True)
        If ViewState("GstPR") = True Then
            AddRow(intSubTotalCol, "GST Amount", CDbl(Format(dblTotalGst, "#0.00")), True)
        Else
            AddRow(intSubTotalCol, "Tax", CDbl(Format(dblTotalGst, "#0.00")), True)
        End If
        'AddRow(intSubTotalCol, "Shipping & Handling", CDbl(Format(ViewState("ShipAmt"), "#0.00")), True)
        AddRow(intSubTotalCol, "Grand Total ", CDbl(Format(dblGrandTotal, "#0.00")), False)
        'dtgPRList.Columns(EnumPR.icTax).Visible = True

        If ViewState("CurrentAppSeq") = ViewState("HighestAppr") Then
            ViewState("ISHighestLevel") = True
        Else
            ViewState("ISHighestLevel") = False
        End If
    End Sub

    Private Function checkMandatory(ByRef strMsg As String) As Boolean
        'strMsg = ""
        'Dim objPR As New PR
        'Dim intBCM As String
        'intBCM = CInt(objPR.checkBCM)
        'If intBCM > 0 Then
        '    If Not objPR.checkUserAccExist() Then
        '        strMsg = "You are not assigned to any Budget Account Code. ""&vbCrLf&""Please contact the Finance Manager. "
        '    End If
        'End If
        'If strMsg <> "" Then
        '    checkMandatory = False
        'Else
        '    checkMandatory = True
        'End If
        'objPR = Nothing
    End Function

    Private Sub cmdDup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDup.Click
        'Dim objPR As New PurchaseReq2
        'Dim strMsg, strMsg1 As String
        'If checkMandatory(strMsg1) Then
        '    strMsg = objPR.DuplicatePR(lblPRNo.Text, ViewState("PRIndex"))
        '    If strMsg <> "" Then
        '        Common.NetMsgbox(Me, strMsg, dDispatcher.direct("PR", "SearchPR_All.aspx", "caller=BUYER&pageid=" & strPageId), MsgBoxStyle.Exclamation)

        '    End If
        'Else
        '    Common.NetMsgbox(Me, strMsg1, MsgBoxStyle.Exclamation)
        'End If
        'objPR = Nothing
    End Sub

    Private Sub cmdPrReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrReport.Click
        'Response.Redirect(dDispatcher.direct("PR", "PRReport.aspx", "Pageid=" & strPageId & "&prno=" & Request.QueryString("PRNO") & "&index=" & ViewState("PRIndex")))
        ''= "<a href=../PO/PRDetail.aspx?caller=OTHER&PageID=16&index=" & dv("PRM_PR_INDEX") & "&PRNo=" & dv("PRM_PR_NO") & ">" & dv("PRM_PR_NO") & "</a>"
    End Sub

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
        Session("w_SearchPRAO_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PR", "SearchPR_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PR", "SearchApp_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
    End Sub
End Class


