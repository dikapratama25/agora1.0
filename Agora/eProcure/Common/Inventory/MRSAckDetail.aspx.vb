'Copyright © 2013 STRATEQ GLOBAL SERVICES. All rights reserved.

Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing

Public Class MRSAckDetail
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objDO As New DeliveryOrder
    Dim dt As New DataTable
    Dim intCnt As Integer
    Dim strC() As String
    Dim dsAllInfo, ds As DataSet
    Dim objInv As New Inventory
    Dim blnCustomField As Boolean = False
    Dim intPRStatus As String
    Dim dblNoTaxTotal, dblTaxTotal, dblTotalGst As Double
    Dim intGSTcnt, intNoGSTcnt, intTotItem As Integer
    Dim strRFQIndex As String
    Dim CrDate As Date
    'Protected WithEvents trAdmin As System.Web.UI.HtmlControls.HtmlTableRow

    Protected WithEvents test As System.Web.UI.WebControls.Panel
    Dim strCaller As String
    Dim dtBCM As DataTable
    Dim LocDesc, SubLocDesc As String

    Enum EnumMRS
        icItemCode
        icItemName
        icUom
        icStatus
        icQty
    End Enum
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
        SetGridProperty(dtgItem)

        If Not Page.IsPostBack Then
            Session("arySetMRSLot") = Nothing
            GenerateTab()
            Dim objInv As New Inventory
            dsAllInfo = objInv.getMRSForAppr(Request.QueryString("MRSNo"))
            objInv = Nothing

            renderMRSHeader()
            renderMRSApprFlow()
            renderMRSDetail()

            If Session("urlreferer") = "Dashboard" Then
                lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashboard.aspx", "pageId=" & strPageId)
            Else
                lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "MRSAckListing.aspx", "pageId=" & strPageId)
            End If

            hidSummary.Value = "Remarks-" & txtRemark.ClientID
            txtRemark.Attributes.Add("onKeyDown", "limitText (this, 900);")
            cmdAckMRS.Attributes.Add("onClick", "return confirmAcknowledge();")
            cmdCancelMRS.Attributes.Add("onClick", "return confirmCancel();")
        End If

        'Check Access Status
        If Session("AccessStatus") = "Limited" Then
            cmdAckMRS.Enabled = False
            cmdCancelMRS.Enabled = False
        End If

    End Sub

    Public Function CreateFileLinks(ByVal userID As Object, ByVal altUserID As Object, ByVal seq As String, ByVal tb_iqc As String) As DataTable
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
        Dim ds As DataSet
        ds = pr.getUserAttach("AO", tb_iqc, CStr(ViewState("IRM_IR_INDEX")), id1, id2, seq)
        'If tb_iqc = "IR" Then
        '    ds = pr.getUserAttach("AO", tb_iqc, CStr(ViewState("IRM_IR_INDEX")), id1, id2, seq)
        'ElseIf tb_iqc = "MRS" Then
        '    ds = objDb.FillDs("SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & Session("CompanyId") & "' AND CDA_DOC_NO='" & ViewState("MRSNo") & "' AND CDA_DOC_TYPE='MRS'")
        'Else
        '    ds = pr.getUserAttach("AO", tb_iqc, CStr(ViewState("IRM_IR_INDEX")), id1, id2, seq)
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
            Dim strFile As String
            Dim strFile1 As String
            strFile = row.Item("UA_ATTACH_FILENAME")
            strFile1 = row.Item("UA_HUB_FILENAME")
            'If tb_iqc = "MRS" Then
            '    strFile = row.Item("CDA_ATTACH_FILENAME")
            '    strFile1 = row.Item("CDA_HUB_FILENAME")
            'Else
            '    strFile = row.Item("UA_ATTACH_FILENAME")
            '    strFile1 = row.Item("UA_HUB_FILENAME")
            'End If

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

    Private Sub renderMRSHeader()
        Dim dtHeader As New DataTable
        Dim objInv2 As New Inventory
        dtHeader = dsAllInfo.Tables(0)
        If dtHeader.Rows.Count > 0 Then

            If Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_URGENT")) = "Y" Then
                lblMRSNo.Text = Me.Request.QueryString("MRSNo") & " (Urgent)"
            Else
                lblMRSNo.Text = Me.Request.QueryString("MRSNo")
            End If

            ViewState("MRSNo") = Me.Request.QueryString("MRSNo")
            ViewState("IRM_IR_INDEX") = Common.parseNull(dtHeader.Rows(0)("IRM_IR_INDEX"))
            ViewState("IRSM_IRS_INDEX") = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_INDEX"))
            lblMRSDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dtHeader.Rows(0)("IRSM_IRS_DATE"))
            lblIssue.Text = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_ISSUE_TO"))

            ViewState("IRSM_IRS_DEPARTMENT") = dtHeader.Rows(0)("IRSM_IRS_DEPARTMENT")
            lblDept.Text = dtHeader.Rows(0)("CDM_DEPT_NAME") 'Department

            lblRefNo.Text = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_REF_NO"))
            lblRemark.Text = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_REMARK"))
            If IsDBNull(dtHeader.Rows(0)("CS_SEC_NAME")) Then
                lblSection.Text = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_SECTION")) 'Section Code
            Else
                lblSection.Text = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_SECTION")) & " : " & dtHeader.Rows(0)("CS_SEC_NAME") 'Section Code : Section Name
            End If
            lblReqName.Text = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_REQUESTOR_NAME"))
            If IsDBNull(dtHeader.Rows(0)("IRSM_IRS_APPROVED_DATE")) Then
                lblIssuedDate.Text = ""
            Else
                lblIssuedDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dtHeader.Rows(0)("IRSM_IRS_APPROVED_DATE"))
            End If
            If Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_STATUS")) = "2" Then
                lblStatus.Text = "Issued"
            ElseIf Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_STATUS")) = "7" Then
                lblStatus.Text = "Partial Issued"
            Else
                lblStatus.Text = ""
            End If

        End If

        Dim dvFile As DataView
        Dim intLoop, intCount As Integer
        Dim strFile, strFile1, strURL, strTempInt As String
        dvFile = dsAllInfo.Tables("COMPANY_DOC_ATTACHMENT").DefaultView
        If dvFile.Count > 0 Then
            intCount = 0
            For intLoop = 0 To dvFile.Count - 1
                If Common.parseNull(dvFile(intLoop)("CDA_TYPE")) = "I" Then
                    strFile = dvFile(intLoop)("CDA_ATTACH_FILENAME")
                    strFile1 = dvFile(intLoop)("CDA_HUB_FILENAME")

                    Dim objFile As New FileManagement
                    strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "MRS", EnumUploadFrom.FrontOff)
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
            strTempInt = "No Files Attached"
        End If
        lblFileInt.Text = strTempInt

        '//For File Attachemnt

    End Sub

    Private Sub renderMRSApprFlow()
        Dim objInv As New Inventory
        ds = objInv.getApprFlow(ViewState("IRM_IR_INDEX"), , "MRS", ViewState("IRSM_IRS_INDEX"))
        objInv = Nothing
        dtgAppFlow.DataSource = ds.Tables(0)
        dtgAppFlow.DataBind()
    End Sub

    Private Sub renderMRSDetail()
        Bindgrid(False)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim dvViewIR As DataView
        dvViewIR = dsAllInfo.Tables("INVENTORY_REQUISITION_SLIP_DETAILS").DefaultView
        'dvViewSample(0)(0)
        If pSorted Then
            dvViewIR.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewIR.Sort += " DESC"
        End If

        intPageRecordCnt = dsAllInfo.Tables("INVENTORY_REQUISITION_SLIP_DETAILS").Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            dtgItem.DataSource = dvViewIR
            dtgItem.DataBind()
        Else
            dtgItem.DataBind()
        End If

    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub dtgAppFlow_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAppFlow.ItemCreated
        Grid_ItemCreated(sender, e)
    End Sub

    Private Sub dtgAppFlow_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAppFlow.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            'Dim intTotalCell, intLoop As Integer
            'If dv("IRA_Seq") - 1 = dv("IRA_AO_Action") Then
            '    intTotalCell = e.Item.Cells.Count - 1
            '    For intLoop = 0 To intTotalCell
            '        e.Item.Cells(intLoop).Font.Bold = True
            '    Next

            '    ViewState("CurrentAppSeq") = dv("IRA_Seq")
            '    ViewState("ApprType") = dv("IRA_APPROVAL_TYPE")

            '    ''//if relief ao, how???, CANNOT USER SESSION("USERID")/
            'End If

            ViewState("HighestAppr") = dv("IRA_Seq")
            If dv("IRA_APPROVAL_TYPE") = 1 Then
                e.Item.Cells(3).Text = "Approval"
            ElseIf dv("IRA_APPROVAL_TYPE") = 2 Then
                e.Item.Cells(3).Text = "Issue MRS"
            End If

            If IsDBNull(dv("AAO_NAME")) Then
                e.Item.Cells(2).Text = "-"
            End If

            If UCase(Common.parseNull(dv("IRA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("IRA_AO"))) Then
                e.Item.Cells(1).Font.Bold = True
            ElseIf UCase(Common.parseNull(dv("IRA_ACTIVE_AO"))) = UCase(Common.parseNull(dv("IRA_A_AO"))) Then
                e.Item.Cells(2).Font.Bold = True
            End If

            If Not IsDBNull(dv("IRA_ACTION_DATE")) Then
                e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IRA_ACTION_DATE")) & " " & Format(dv("IRA_ACTION_DATE"), "HH:mm:ss")
            End If
        End If
    End Sub

    Private Sub dtgItem_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemCreated
        Grid_ItemCreated(dtgItem, e)
    End Sub

    Private Sub dtgItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            If dv("IRSD_IRS_STATUS") = "2" Then
                e.Item.Cells(EnumMRS.icStatus).Text = "Issued"
            ElseIf dv("IRSD_IRS_STATUS") = "6" Then
                e.Item.Cells(EnumMRS.icStatus).Text = "Rejected"
            ElseIf dv("IRSD_IRS_STATUS") = "7" Then
                e.Item.Cells(EnumMRS.icStatus).Text = "Issued, Rejected"
            End If

        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_MRSAckDetail_tabs") = "<div class=""t_entity""><ul>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "InventoryReq.aspx", "pageid=" & strPageId) & """><span>Inventory Requisition</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "InventoryReqList.aspx", "pageid=" & strPageId) & """><span>Inventory Requisition Listing</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "MRSAckListing.aspx", "pageid=" & strPageId) & """><span>MRS Acknowledge Listing</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "MRSListing_Requestor.aspx", "pageid=" & strPageId) & """><span>MRS Listing</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "</ul><div></div></div>"
    End Sub

    Private Sub cmdAckMRS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAckMRS.Click
        Dim strMsg As String = ""
        Dim objInv As New Inventory
        Dim dsIRDetail As New DataSet

        strMsg = objInv.AcknowledgeMRS(ViewState("MRSNo"), ViewState("IRSM_IRS_INDEX"), txtRemark.Text)
        If Session("urlreferer") = "Dashboard" Then
            Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
        Else
            Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Inventory", "MRSAckListing.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
        End If

        cmdCancelMRS.Visible = False
        cmdAckMRS.Visible = False

    End Sub

    Private Sub cmdCancelMRS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancelMRS.Click
        Dim objInv As New Inventory
        Dim dsIRDetail As New DataSet
        Dim strMsg As String

        If txtRemark.Text = "" Then
            Common.NetMsgbox(Me, "Please enter Remarks.", MsgBoxStyle.Information)
        Else
            strMsg = objInv.CancelMRS(ViewState("MRSNo"), ViewState("IRSM_IRS_INDEX"), txtRemark.Text)
            If Session("urlreferer") = "Dashboard" Then
                Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
            Else
                Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Inventory", "MRSAckListing.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
            End If

            cmdCancelMRS.Visible = False
            cmdAckMRS.Visible = False
        End If

    End Sub
End Class


