

Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing
Public Class PRDetailTransTracking
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
    Dim intPRStatus As String
    'Dim dblGSTPer As Double
    Dim dblNoTaxTotal, dblTaxTotal, dblTotalGst As Double
    Dim dblTotalAmt As Double
    Dim intGSTcnt, intNoGSTcnt, intTotItem As Integer
    Dim strRFQIndex As String
    Dim CrDate As Date
    Dim strPRType As String

    Public Enum EnumPR
        icSNo
        'icBuyerItem
        icVendorItem
        'icGLCode
        'icCategoryCode
        'icTaxCode
        icAssetCode '2
        icProdDesc
        icPONo
        'icMOQ
        'icMPQ
        'icRFQQty
        'icQtyToPer
        icQty
        icUOM
        icCurrency
        icCost
        icSubTotal
        'icAmount
        icTax
        icGstRate
        icGstAmt
        icGstTaxCode
        icBCM
        icDAddr
        icETD
        icWTerm
        icRemark    '14
    End Enum

    Protected WithEvents test As System.Web.UI.WebControls.Panel
    'Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Dim strCaller As String
    'Protected WithEvents txtRemark As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cmdAppPR As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdRejectPR As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdHoldPR As System.Web.UI.WebControls.Button
    'Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    'Protected WithEvents tblApproval As System.Web.UI.HtmlControls.HtmlTable
    'Protected WithEvents trButton As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    'Protected WithEvents trMessage As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    'Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents cmdPrReport As System.Web.UI.WebControls.Button
    'Protected WithEvents lblConsolidator As System.Web.UI.WebControls.Label
    'Protected WithEvents trConsolidator As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents txtRemarkCR As System.Web.UI.WebControls.TextBox
    'Protected WithEvents lblRemarkCR As System.Web.UI.WebControls.Label
    'Protected WithEvents lblAttach As System.Web.UI.WebControls.Label
    'Protected WithEvents txtExternal As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cmdUpload As System.Web.UI.WebControls.Button
    'Protected WithEvents File1 As System.Web.UI.HtmlControls.HtmlInputFile
    'Protected WithEvents pnlAttach As System.Web.UI.WebControls.Panel
    'Protected WithEvents lblFileAO As System.Web.UI.WebControls.Label
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
        'CheckButtonAccess(True)
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
        '//setting grid property
        SetGridProperty(dtgAppFlow)
        SetGridProperty(dtgPRList)

        If Not Page.IsPostBack Then
            Dim objPR As New PurchaseReq2
            dsAllInfo = objPR.getPRInfo(Request.QueryString("PRNO"), Request.QueryString("index"))
            objPR = Nothing
            '//For BCM
            '//load List for whole company - for temporary only
            '//because cannot think of other better solution
            '//other reason-that particular acct may assigned to buyer when he rause PR
            '//but later it was taken out
            Dim objBCM As New BudgetControl
            'Michelle (21/9/2010) - Temporarily disable the BCM module (ExclBCM)
            'dtBCM = objBCM.getBCMListByCompany()
            dtBCM = objBCM.getBCMListByCompanyNew()
            objBCM = Nothing

            'Chee Hong (28/5/2014) - Issue 1882 - BCM On/Off Setting
            Dim objPR_PR As New PR
            ViewState("BCM") = CInt(objPR_PR.checkBCM)
            objPR_PR = Nothing

            renderPRHeader()
            renderPRApprFlow()
            renderPRDetail()

            ''Michelle (eBiz/303) - Allow AO to do attachment

            ''//Default, override if necessary
            'lblTitle.Text = "Purchase Requisition Details"
            'tblBuyer.Style("display") = "none"
            'strCaller = UCase(Request.QueryString("caller"))
            ''//although case "ao", "buyer","admin" having same setting
            ''//we write it at different case to cater for future change of setting
            'Select Case strCaller
            '    Case "AO" '//AO-Approved List 
            '        'lnkBack.NavigateUrl = "SearchPR_All.aspx?caller=" & strCaller & "&pageId=" & strPageId
            '        lnkBack.NavigateUrl = "javascript:history.back();"
            '        Me.EnableViewState = False
            '    Case "PR" '//AO-Consolidate PR
            '        lnkBack.NavigateUrl = dDispatcher.direct("PO", "PRConsolidation.aspx", "pageId=" & strPageId)
            '        Me.EnableViewState = False
            '    Case "APPROVAL" '//AO-New Requisition
            '        Dim objBudget As New BudgetControl
            '        Dim dtBCM As New DataTable
            '        Dim strBCM As String
            '        Dim blnExceed As Boolean
            '        'Michelle (21/9/2010) - Temporarily disable the BCM module (ExclBCM)
            '        'blnExceed = objBudget.checkBCM(Request.QueryString("PRNO"), dtBCM, strBCM)

            '        objBudget = Nothing

            '        'Michelle (21/9/2010) - Temporarily disable the BCM module (ExclBCM)
            '        'If blnExceed Then
            '        '    Common.NetMsgbox(Me, "PR value is more than the Operating Budget Amount.", MsgBoxStyle.Exclamation)
            '        'End If

            '        lblTitle.Text = "Purchase Requisition Approval Details"
            '        If ViewState("ApprType") = "2" Then
            '            lblTitle.Text = lblTitle.Text & " (Endorsement)"
            '        End If
            '        '"PRDetail.aspx?caller=approval&AO=" & strAO & "&relief=" & blnRelief & "&PageID=" & strPageId & "&index=" & dv("PRM_PR_Index") & "&PRNo=" & dv("PRM_PR_No")
            '        lnkBack.NavigateUrl = dDispatcher.direct("PO", "SearchPR_AO.aspx", "pageId=" & strPageId)
            '        '//Can approve or not
            '        'If ViewState("ApprType") = "1" And ViewState("ApprLimit") < dblNoTaxTotal + dblTaxTotal + dblTotalGst Then
            '        'Else
            '        '    trButton.Style("display") = ""
            '        '    trMessage.Style("display") = "none"
            '        'End If

            '        'If intPRStatus = PRStatus.CancelledBy Then
            '        '    trButton.Style("display") = "none"
            '        '    trMessage.Style("display") = ""
            '        '    lblMsg.Text = "Purchase Requisition has been cancelled by the user."
            '        'Else
            '        '    trButton.Style("display") = ""
            '        '    trMessage.Style("display") = "none"
            '        'End If

            '    Case "BUYER" '//buyer - View Requisitions
            '        '//buyer can only view own PR
            '        tblBuyer.Style("display") = ""
            '        If intPRStatus = PRStatus.Submitted Then
            '            cmdCancel.Visible = True
            '        End If
            '        lnkBack.NavigateUrl = dDispatcher.direct("PO", "SearchPR_ALL.aspx", "caller=" & strCaller & "&pageId=" & strPageId)
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
            'hidSummary.Value = "Remarks-" & txtRemark.ClientID
        End If
        'txtRemark.Attributes.Add("onKeyDown", "limitText (this, 1000);")
        Image1.ImageUrl = dDispatcher.direct("Plugins/images", "collapse_up.gif")
        Image2.ImageUrl = dDispatcher.direct("Plugins/images", "collapse_up.gif")
        cmdCancel.Attributes.Add("onClick", "return confirm('Are you sure that you want to cancel this PR ?');")
        If Session("urlreferer") = "PRAll" Then 'Purchase Request screen
            cmdDup.Visible = True
            cmdCancel.Visible = False
            lblRemarkCR.Visible = False
            txtRemarkCR.Visible = False
            lnkBack.NavigateUrl = dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId)

        ElseIf Session("urlreferer") = "PRCancel" Then  'Purchase Request Cancellation screen
            cmdDup.Visible = False
            cmdCancel.Visible = True
            lblRemarkCR.Visible = True
            txtRemarkCR.Visible = True
            lnkBack.NavigateUrl = dDispatcher.direct("PR", "SearchPR_Cancellation.aspx", "pageid=" & strPageId)

        ElseIf Session("urlreferer") = "Dashboard" Then
            cmdDup.Visible = True
            cmdCancel.Visible = False
            lblRemarkCR.Visible = False
            txtRemarkCR.Visible = False
            lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)

        ElseIf Session("urlreferer") = "PRRej" Then
            cmdDup.Visible = False
            cmdCancel.Visible = False
            lblRemarkCR.Visible = False
            txtRemarkCR.Visible = False
            lnkBack.NavigateUrl = dDispatcher.direct("PR", "SearchApp_All.aspx", "pageid=" & strPageId)

        End If
        If Request.QueryString("Frm") = "transtracking" Then
            cmdDup.Visible = False
            cmdCancel.Visible = False
            lblRemarkCR.Visible = False
            txtRemarkCR.Visible = False
            lnkBack.NavigateUrl = dDispatcher.direct("Tracking", "TransTracking.aspx", "coytype=" & Request.QueryString("coytype") & "&pageid=" & strPageId)
        End If

        'added by michael
        strCaller = UCase(Request.QueryString("caller"))
        If strCaller = "ADMIN" Then
            lnkBack.NavigateUrl = dDispatcher.direct("PR", "ViewPR_All.aspx", "caller=" & strCaller & "&pageid=" & strPageId & "")
        End If
        'Yik Foong
        'Always display the uploaded file(s)
        'displayAttachFile()
        'renderPRApprFlow()

        Dim Asset As New PurchaseOrder_Buyer
        If Asset.AssetGroupMstr = False Then
            dtgPRList.Columns(EnumPR.icAssetCode).Visible = False
        End If
    End Sub

    'Author: Yik Foong
    'Date: 10 July 2009
    'Construct a table the contain 1 column that store hyperlinks of the attachment
    '
    'Argument:
    '	userID: The attachment belong to this user ID ( pass in from the 'Approving Officer' column )
    'Returned Table info:
    '	Columns : 1
    '	Name : Hyperlink
    '	Data Stored : html hyperlink tag that contain the link to the file download
    '
    'Note: The column's name should not changed. If the column name have any changes,
    '		please change also the PO/PRDetail.aspx data binding value. The .aspx file 
    '		will retrieve value base on the column name.
    '		This method is called by the DataList inside the last column of the table.
    '		For detail, see PO/PRDetail.aspx
    '
    'Binding expression : <%# CreateFileLinks( DataBinder.Eval( Container.DataItem, "PRA_AO")) %>
    '
    '	DataBinder.Eval( Container.DataItem, "PRA_AO") : return the value of the column 'PRA_AO'(userID) of current row,
    '	this value is pass to function CreateFileLinks()
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




    'Set the Attachment column Visible = true
    'Note: The Attachment column should always be the last column of the table,
    '	else, need change the column index 
    Private Sub GenerateAttachmentColumn()
        'Show the last column 
        dtgAppFlow.Columns(dtgAppFlow.Columns.Count - 1).Visible = True
    End Sub

    Private Sub renderPRHeader()
        Dim dtHeader As New DataTable
        Dim objGst As New GST
        'Dim strBillAddr As String

        dtHeader = dsAllInfo.Tables("PR_MSTR")
        If dtHeader.Rows.Count > 0 Then
            lblPR.Text = Common.parseNull(dtHeader.Rows(0)("PRM_PR_NO"))
            lblStatus.Text = Common.parseNull(dtHeader.Rows(0)("STATUS_DESC"), "nil")
            lblReqName.Text = Common.parseNull(dtHeader.Rows(0)("PRM_REQ_NAME"), "nil")
            If Not IsDBNull(dtHeader.Rows(0)("PRM_SUBMIT_DATE")) Then
                lblPRDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dtHeader.Rows(0)("PRM_SUBMIT_DATE"))
            Else
                lblPRDate.Text = "nil"
            End If
            lblReqCon.Text = Common.parseNull(dtHeader.Rows(0)("PRM_REQ_PHONE"), "nil")
            lblAtt.Text = Common.parseNull(dtHeader.Rows(0)("PRM_S_ATTN"), "nil")
            txtInternal.Text = Common.parseNull(dtHeader.Rows(0)("PRM_INTERNAL_REMARK"), "nil")
            txtExternal.Text = Common.parseNull(dtHeader.Rows(0)("PRM_External_Remark"), "nil")
            'ViewState("Consolidator") = Common.parseNull(dtHeader.Rows(0)("PRM_CONSOLIDATOR"), "")
            Session("CrDate") = Common.FormatWheelDate(WheelDateFormat.LongDate, dtHeader.Rows(0)("PRM_CREATED_DATE"))
            strPRType = Common.parseNull(dtHeader.Rows(0)("PRM_PR_TYPE"), "")
            ViewState("Requestor") = Common.parseNull(dtHeader.Rows(0)("PRM_BUYER_ID"), "")
            If strPRType = "CC" Then  'Contract Catalogue PR
                dtgPRList.Columns(EnumPR.icPONo).HeaderText = "PO. No."
                dtgPRList.Columns(EnumPR.icCost).HeaderText = "Contract Price"
            Else
                dtgPRList.Columns(EnumPR.icPONo).HeaderText = "Converted"
                dtgPRList.Columns(EnumPR.icCost).HeaderText = "Last Txn. Price"
            End If

            ViewState("PRIndex") = dtHeader.Rows(0)("PRM_PR_Index")
            intPRStatus = dtHeader.Rows(0)("PRM_PR_Status")
            If Not IsDBNull(dtHeader.Rows(0)("PRM_RFQ_INDEX")) Then
                cmdDup.Enabled = False
            End If

            If Not IsDBNull(dtHeader.Rows(0)("PRM_CREATED_DATE")) Then
                ViewState("GstPR") = objGst.chkGSTCOD(Format(dtHeader.Rows(0)("PRM_CREATED_DATE"), "dd/MM/yyyy"))
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
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "PR", EnumUploadFrom.FrontOff)
                objFile = Nothing
                '*************************meilai************************************
                If strTemp = "" Then
                    strTemp = "&nbsp;" & intLoop + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDA_FILESIZE") & "KB)"
                Else
                    strTemp = strTemp & "<BR>&nbsp;" & intLoop + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDA_FILESIZE") & "KB)"
                End If
            Next
        Else
            strTemp = "No Files Attached"
        End If
        lblFile.Text = strTemp

        '//For File Attachemnt
    End Sub
    Private Sub renderPRApprFlow()
        Dim objPR As New PurchaseReq2
        ds = objPR.getApprFlow(ViewState("PRIndex"), "PR")
        objPR = Nothing
        'dtgAppFlow.DataSource = ds.Tables(0).DefaultView
        'If Session("Env") <> "FTN" Then
        '    dtgAppFlow.Columns(2).Visible = True
        'Else
        '    dtgAppFlow.Columns(2).Visible = False
        'End If
        dtgAppFlow.Columns(2).Visible = True
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
        Dim dblSubTotal, dblGrandTotal As Double

        dblSubTotal = dblTotalAmt
        dblGrandTotal = dblSubTotal + dblTotalGst

        If dsAllInfo.Tables("PR_DETAILS").Rows.Count > 0 Then
            AddRow("Sub Total", CDbl(Format(dblSubTotal, "#0.00")))
            If ViewState("GstPR") = True Then
                AddRow("SST Amount", CDbl(Format(dblTotalGst, "#0.00")))
                Me.dtgPRList.Columns(EnumPR.icTax).Visible = False
                Me.dtgPRList.Columns(EnumPR.icGstRate).Visible = True
                Me.dtgPRList.Columns(EnumPR.icGstAmt).Visible = True
                Me.dtgPRList.Columns(EnumPR.icGstTaxCode).Visible = True
            Else
                AddRow("Tax", CDbl(Format(dblTotalGst, "#0.00")))
                Me.dtgPRList.Columns(EnumPR.icTax).Visible = True
                Me.dtgPRList.Columns(EnumPR.icGstRate).Visible = False
                Me.dtgPRList.Columns(EnumPR.icGstAmt).Visible = False
                Me.dtgPRList.Columns(EnumPR.icGstTaxCode).Visible = False
            End If

            'AddRow("GST Amount", CDbl(Format(dblTotalGst, "#0.00")))
            AddRow("Grand Total ", CDbl(Format(dblGrandTotal, "#0.00")))
        End If

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

    'Sub AddRow(ByVal intCell As Integer, ByVal strLabel As String, ByVal dblTotal As Double, ByVal blnShowGST As Boolean)
    '    'adding totals row
    '    Dim intL, intColToRemain As Integer
    '    Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
    '    Dim intTotalCol As Integer

    '    intTotalCol = intRemarkCol + intCnt - 1

    '    For intL = 0 To intTotalCol
    '        addCell(row)
    '    Next
    '    '//13+3-5=11
    '    '//16-11 = 5

    '    If strGSTBy = "product" And blnShowGST Then
    '        intColToRemain = 5           '//col bf label, label, total,gst,col after total
    '    Else
    '        intColToRemain = 4           '//col bf label, label, total,col after total
    '    End If

    '    For intL = 0 To intTotalCol - intColToRemain
    '        row.Cells.RemoveAt(1)
    '    Next

    '    row.Cells(0).ColumnSpan = intCell - 1
    '    'Dim dg As DataGridItem
    '    'total = 0
    '    'For Each dg In dtgPRList.Items
    '    '    total += Decimal.Parse(dg.Cells(intCell).Text)
    '    'Next
    '    row.Cells(1).Text = strLabel
    '    row.Cells(1).Font.Bold = True
    '    row.Cells(2).HorizontalAlign = HorizontalAlign.Right
    '    row.Cells(2).Text = Format(dblTotal, "#,##0.00")
    '    row.Cells(2).Font.Bold = True

    '    If strGSTBy = "product" And blnShowGST Then
    '        row.Cells(3).Font.Bold = True
    '        row.Cells(3).HorizontalAlign = HorizontalAlign.Right
    '        If dblTotalGst = 0 Then
    '            row.Cells(3).Text = "0.00"              '"n.a."
    '        Else
    '            row.Cells(3).Text = Format(dblTotalGst, "#,##0.00")
    '        End If
    '        row.Cells(4).ColumnSpan = intTotalCol - (intCell - 1)
    '    Else
    '        row.Cells(3).ColumnSpan = intTotalCol - (intCell - 1)
    '    End If

    '    'row.CssClass = "BODY"
    '    row.BackColor = Color.FromName("#f4f4f4")
    '    dtgPRList.Controls(0).Controls.Add(row)
    'End Sub
    'Sub AddRow(ByVal strLabel As String, ByVal dblTotal As Double)
    '    'adding totals row
    '    Dim intL, intColToRemain As Integer
    '    Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
    '    Dim intTotalCol As Integer
    '    Dim intCell As Integer
    '    intCell = EnumPR.icSubTotal

    '    intTotalCol = EnumPR.icRemark + intCnt - 1

    '    For intL = 0 To intTotalCol
    '        addCell(row)
    '    Next

    '    intColToRemain = 4
    '    For intL = 0 To intTotalCol - intColToRemain
    '        row.Cells.RemoveAt(1)
    '    Next

    '    row.Cells(0).ColumnSpan = intCell - 1
    '    row.Cells(1).Text = strLabel
    '    row.Cells(1).Font.Bold = True
    '    row.Cells(1).HorizontalAlign = HorizontalAlign.Right
    '    row.Cells(2).HorizontalAlign = HorizontalAlign.Right
    '    row.Cells(2).Text = Format(dblTotal, "#,##0.00")
    '    row.Cells(2).Font.Bold = True

    '    row.Cells(3).ColumnSpan = intTotalCol - (intCell - 1)
    '    row.BackColor = Color.FromName("#f4f4f4")
    '    dtgPRList.Controls(0).Controls.Add(row)

    'End Sub
    Sub AddRow(ByVal strLabel As String, ByVal dblTotal As Double)
        'adding totals row
        Dim intL, intColToRemain As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer
        Dim intCell As Integer
        intCell = EnumPR.icSubTotal

        intTotalCol = EnumPR.icRemark + intCnt - 1

        For intL = 0 To intTotalCol
            addCell(row)
        Next

        intColToRemain = 4
        For intL = 0 To intTotalCol - intColToRemain
            row.Cells.RemoveAt(1)
        Next

        row.Cells(0).ColumnSpan = intCell - 7
        row.Cells(1).ColumnSpan = intCell - 3
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

    End Sub
    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim dvViewPR As DataView
        dvViewPR = dsAllInfo.Tables("PR_DETAILS").DefaultView

        'dvViewSample(0)(0)
        If pSorted Then
            dvViewPR.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"
        End If

        intPageRecordCnt = dsAllInfo.Tables("PR_DETAILS").Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt
        '//bind datagrid
        If intPageRecordCnt > 0 Then
            'resetDatagridPageIndex(dtgPRList, dvViewPR)
            dtgPRList.DataSource = dvViewPR
            dtgPRList.DataBind()
        Else
            dtgPRList.DataBind()
        End If
        'If Session("Env") = "FTN" Then
        '    Me.dtgPRList.Columns(EnumPR.icBCM).Visible = False
        '    Me.dtgPRList.Columns(EnumPR.icWTerm).Visible = False
        'Else
        '    Me.dtgPRList.Columns(EnumPR.icBCM).Visible = True
        '    Me.dtgPRList.Columns(EnumPR.icWTerm).Visible = True
        'End If
        If ViewState("BCM") <= 0 Then
            Me.dtgPRList.Columns(EnumPR.icBCM).Visible = False
        Else
            Me.dtgPRList.Columns(EnumPR.icBCM).Visible = True
        End If
        'Me.dtgPRList.Columns(EnumPR.icBCM).Visible = True
        Me.dtgPRList.Columns(EnumPR.icWTerm).Visible = True
        ViewState("PageCount") = dtgPRList.PageCount
    End Function

    Private Sub dtgPRList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPRList.ItemCreated
        '//this line must be included
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgPRList, e)
    End Sub

    Private Sub dtgPRList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPRList.ItemDataBound
        Dim i As Integer
        Dim intPRLine As Integer
        Dim strFieldNo, strDAddr As String
        Dim dr() As DataRow
        Dim dblAmt, dblGstAmt As Double
        Dim objDB As New EAD.DBCom
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            If strPRType = "CC" Then  'Contract Catalogue PR
                'e.Item.Cells(EnumPR.icPONo).Text = Common.parseNull(dv("PRD_CONVERT_TO_DOC"), "") 'Show PO number
                If Common.parseNull(dv("PRD_CONVERT_TO_DOC"), "") <> "" Then
                    e.Item.Cells(EnumPR.icPONo).Text = Common.parseNull(dv("PRD_CONVERT_TO_DOC"), "") 'Show PO number
                Else
                    Dim strPO_No As String = objDB.GetVal("SELECT POM_PO_NO FROM PO_MSTR WHERE POM_PO_INDEX = '" & dv("PRM_PO_INDEX") & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                    If strPO_No <> "" Then
                        e.Item.Cells(EnumPR.icPONo).Text = strPO_No
                    End If

                End If
            Else
                If Not IsDBNull(dv("PRD_CONVERT_TO_IND")) Then
                    If dv("PRD_CONVERT_TO_IND") = "PO" Then
                        e.Item.Cells(EnumPR.icPONo).Text = "Yes"
                    Else
                        Dim strPO_No As String = objDB.GetVal("SELECT POM_PO_NO FROM PO_MSTR, RFQ_MSTR WHERE POM_RFQ_INDEX = RM_RFQ_ID AND rm_rfq_no = '" & dv("PRD_CONVERT_TO_DOC") & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")
                        If strPO_No <> "" Then
                            e.Item.Cells(EnumPR.icPONo).Text = "Yes"
                        Else
                            e.Item.Cells(EnumPR.icPONo).Text = "No"
                        End If
                    End If
                Else
                    e.Item.Cells(EnumPR.icPONo).Text = "No"
                End If

            End If



            CrDate = Session("CrDate")
            e.Item.Cells(EnumPR.icETD).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, CrDate.AddDays(Common.parseNull(dv("PRD_ETD"))))

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
            '2015-06-18: CH: Rounding issue (Prod issue)
            'dblAmt = Common.parseNull(dv("PRD_UNIT_COST"), 0) * Common.parseNull(dv("PRD_ORDERED_QTY"), 0)
            dblAmt = CDec(Format(Common.parseNull(dv("PRD_UNIT_COST"), 0) * Common.parseNull(dv("PRD_ORDERED_QTY"), 0), "###0.00"))
            e.Item.Cells(EnumPR.icSubTotal).Text = Format(dblAmt, "#,##0.00")
            If Common.parseNull(dv("PRD_GST"), 0) = 0 Then
                e.Item.Cells(EnumPR.icTax).Text = "0.00"
                e.Item.Cells(EnumPR.icGstAmt).Text = "0.00"
            Else
                dblGstAmt = dblAmt * (Common.parseNull(dv("PRD_GST"), 0) / 100)
                e.Item.Cells(EnumPR.icTax).Text = Format(dblGstAmt, "#,##0.00")
                e.Item.Cells(EnumPR.icGstAmt).Text = Format(dblGstAmt, "#,##0.00")
            End If
            dblTotalAmt = dblTotalAmt + dblAmt
            '2015-06-18: CH: Rounding issue (Prod issue)
            'dblTotalGst = dblTotalGst + dblGstAmt
            dblTotalGst = dblTotalGst + CDec(Format(dblGstAmt, "###0.00"))

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
                        'e.Item.Cells(EnumPR.icBCM).Text = drTemp(0)("Acct_List")
                        e.Item.Cells(EnumPR.icBCM).Text = Mid(drTemp(0)("Acct_List"), 1, 10)
                        e.Item.Cells(EnumPR.icBCM).ToolTip = drTemp(0)("Acct_List")
                    End If
                End If
            End If

            'GST Input Tax Code
            'Stage 3 Enhancement (GST-0010) - 14/07/2015 - CH
            If Common.parseNull(dv("PRD_GST_INPUT_TAX_CODE")) <> "" Then
                e.Item.Cells(EnumPR.icGstTaxCode).Text = dv("PRD_GST_INPUT_TAX_CODE")
            Else
                e.Item.Cells(EnumPR.icGstTaxCode).Text = "N/A"
            End If
        End If
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

    'Private Sub cmdAppPR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAppPR.Click
    '    '//1. check whether PO created
    '    '//"Select * from M_PR where PR_number = '" & strPRNum & "' and PR_Status = 4"
    '    '//2. AO Remark = Approved/Endorsed : Remark by User
    '    '//3.check whether this a highest level of AO, if yes, check consolidator
    '    '//if consolidator, no PO created else PO created
    '    '//4.
    '    Dim strRemark, strMsg As String

    '    strRemark = FormatAORemark("approve")
    '    strRemark = strRemark & txtRemark.Text

    '    Dim objPR As New PurchaseReq2
    '    strMsg = objPR.ApprovePR(lblPR.Text, ViewState("PRIndex"), ViewState("CurrentAppSeq"), ViewState("ISHighestLevel"), ViewState("Consolidator"), strRemark, ViewState("Requestor"), Request.QueryString("relief"), ViewState("ApprType"))
    '    Common.NetMsgbox(Me, strMsg, dDispatcher.direct("PO", "SearchPR_AO.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
    'End Sub


    'Private Sub cmdRejectPR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRejectPR.Click
    '    Dim objPR As New PurchaseReq2
    '    Dim strRemark, strMsg As String
    '    strRemark = FormatAORemark("reject")
    '    strRemark = strRemark & txtRemark.Text
    '    ' Michelle (CR0010) - Force user to enter Remarks
    '    'strMsg = objPR.RejectPR(lblPR.Text, ViewState("PRIndex"), ViewState("CurrentAppSeq"), strRemark, Request.QueryString("AO"), Request.QueryString("relief"))
    '    'objPR = Nothing
    '    'Common.NetMsgbox(Me, strMsg, "SearchPR_AO.aspx?pageid=" & strPageId, MsgBoxStyle.Exclamation)

    '    If txtRemark.Text = "" Then
    '        Common.NetMsgbox(Me, "Please enter Remarks.", MsgBoxStyle.Information)
    '    Else
    '        strMsg = objPR.RejectPR(lblPR.Text, ViewState("PRIndex"), ViewState("CurrentAppSeq"), strRemark, Request.QueryString("AO"), Request.QueryString("relief"))
    '        objPR = Nothing
    '        Common.NetMsgbox(Me, strMsg, dDispatcher.direct("PO", "SearchPR_AO.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
    '    End If
    'End Sub

    'Private Sub cmdHoldPR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHoldPR.Click
    '    Dim objPR As New PurchaseReq2
    '    Dim strRemark, strMsg As String
    '    strRemark = FormatAORemark("hold")
    '    strRemark = strRemark & txtRemark.Text

    '    ' Michelle (CR0010) - Force user to enter Remarks
    '    'strMsg = objPR.HoldPR(lblPR.Text, ViewState("PRIndex"), ViewState("CurrentAppSeq"), strRemark, Request.QueryString("AO"), Request.QueryString("relief"))
    '    'objPR = Nothing
    '    'Common.NetMsgbox(Me, strMsg, "SearchPR_AO.aspx?pageid=" & strPageId, MsgBoxStyle.Exclamation)

    '    If txtRemark.Text = "" Then
    '        Common.NetMsgbox(Me, "Please enter Remarks.", MsgBoxStyle.Information)
    '    Else
    '        strMsg = objPR.HoldPR(lblPR.Text, ViewState("PRIndex"), ViewState("CurrentAppSeq"), strRemark, Request.QueryString("AO"), Request.QueryString("relief"))
    '        objPR = Nothing
    '        Common.NetMsgbox(Me, strMsg, dDispatcher.direct("PO", "SearchPR_AO.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)

    '    End If

    'End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        ' Michelle (CR0010) - Force user to enter Remarks
        If txtRemarkCR.Text = "" Then
            txtRemarkCR.Focus()
            Common.NetMsgbox(Me, "Please enter Cancel Remarks.", MsgBoxStyle.Information)
        Else
            Dim objPR As New PurchaseReq2
            Dim strMsg As String
            strMsg = objPR.CancelPR(lblPR.Text, ViewState("PRIndex"), ViewState("CurrentAppSeq"), txtRemarkCR.Text)
            objPR = Nothing
            Common.NetMsgbox(Me, strMsg, dDispatcher.direct("PR", "SearchPR_Cancellation.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
        End If
    End Sub
    'Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
    '    If File1.Value <> "" Then
    '        Dim objFile As New FileManagement
    '        Dim objPR As New PR
    '        Dim objDB As New EAD.DBCom

    '        ' Restrict user upload size
    '        Dim dblMaxFileSize As Double = Val(objDB.GetVal("SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_TYPE = 'USER' AND SP_PARAM_NAME = 'DocAttachSize'"))

    '        Dim sFileName As String
    '        sFileName = System.IO.Path.GetFileName(File1.PostedFile.FileName)

    '        If Len(sFileName) > 50 Then
    '            Common.NetMsgbox(Me, "File name exceeds 46 character")
    '        ElseIf File1.PostedFile.ContentLength > 0 And File1.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
    '            objFile.FileUpload(File1, EnumUploadType.UserAttachment, "PR", EnumUploadFrom.FrontOff, ViewState("PRIndex"), , "AO", Nothing, ViewState("CurrentAppSeq"))
    '        ElseIf File1.PostedFile.ContentLength = 0 Then
    '            Common.NetMsgbox(Me, "0 byte document or file not found")
    '        Else
    '            Common.NetMsgbox(Me, "File exceeds maximum file size")
    '        End If

    '        objPR = Nothing
    '        objFile = Nothing
    '    End If
    '    displayAttachFile()
    '    renderPRApprFlow()
    'End Sub

    Private Sub displayAttachFile()
        Dim objPR As New PR
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String

        'dsAttach = objPR.getPRTempAttach(Session.SessionID)
        dsAttach = objPR.getUserAttach("AO", "PR", ViewState("PRIndex"), ViewState("CurrentAppSeq"))

        'pnlAttach.Controls.Clear()
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

                'pnlAttach.Controls.Add(lblFileAO)
                'pnlAttach.Controls.Add(lnk)
                'pnlAttach.Controls.Add(lblBrAO)
            Next
            'pnlAttach.Controls.Add(New System.Web.UI.HtmlControls.HtmlGenericControl)
        Else
            Dim lblFileAO As New Label
            lblFileAO.Text = "No Files Attached"
            'pnlAttach.Controls.Add(lblFileAO)
        End If
        objPR = Nothing
    End Sub

    Private Sub deleteAttach(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objPR As New PR
        objPR.deleteUserAttachment(CType(sender, ImageButton).ID)
        displayAttachFile()
        objPR = Nothing
        renderPRApprFlow()
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
            strMsg = objPR.DuplicatePR1(lblPR.Text, ViewState("PRIndex"))
            If strMsg <> "" Then
                Common.NetMsgbox(Me, strMsg, dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)

            End If
        Else
            Common.NetMsgbox(Me, strMsg1, MsgBoxStyle.Exclamation)
        End If
        objPR = Nothing
    End Sub

    Private Sub cmdPrReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrReport.Click
        Response.Redirect(dDispatcher.direct("PR", "PRReport.aspx", "Pageid=" & strPageId & "&prno=" & Request.QueryString("PRNO") & "&index=" & ViewState("PRIndex")))
        '= "<a href=../PO/PRDetail.aspx?caller=OTHER&PageID=16&index=" & dv("PRM_PR_INDEX") & "&PRNo=" & dv("PRM_PR_NO") & ">" & dv("PRM_PR_NO") & "</a>"
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

End Class