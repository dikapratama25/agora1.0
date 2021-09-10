Imports AgoraLegacy
Imports eProcure.Component


Public Class MRSDetail
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents lblMRSNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblMRSDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblIssueTo As System.Web.UI.WebControls.Label
    Protected WithEvents lblQty As System.Web.UI.WebControls.Label
    Protected WithEvents lblRefNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblRemark As System.Web.UI.WebControls.Label
    Protected WithEvents lblDepartment As System.Web.UI.WebControls.Label
    Protected WithEvents lblSection As System.Web.UI.WebControls.Label
    Protected WithEvents lblRequestor As System.Web.UI.WebControls.Label
    Protected WithEvents lblIssuedDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents lblFileInt As System.Web.UI.WebControls.Label

    Protected WithEvents txtIssueRemark As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAckRemark As System.Web.UI.WebControls.TextBox

    Protected WithEvents tr_IssueRemark As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_AckRemark As System.Web.UI.HtmlControls.HtmlTableRow

    Protected WithEvents dtgAppFlow As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtgItem As System.Web.UI.WebControls.DataGrid

    Protected WithEvents cmdPrint As System.Web.UI.HtmlControls.HtmlInputButton

    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Dim dsAllInfo, ds As DataSet
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objINV As New Inventory
    Dim objGLO As New AppGlobals
    Dim aryInvReq As New ArrayList()
    Dim Court_loc As Integer = 0
    Dim intNumeric As Boolean = True
    Dim strMsg As String = ""
    Dim LocDesc, SubLocDesc As String

    Enum EnumMRS
        icItemCode
        icItemName
        icUOM
        icStatus
        icMthIssue
        icLastMth
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
        ''cmd_save.Enabled = False
        ''Dim alButtonList As ArrayList
        ''alButtonList = New ArrayList
        ''alButtonList.Add(cmd_save)
        ''htPageAccess.Add("add", alButtonList)
        ''htPageAccess.Add("update", alButtonList)
        ''CheckButtonAccess()
        'cmd_Reset.Enabled = blnCanAdd Or blnCanUpdate Or blnCanDelete
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgItem)
        SetGridProperty(dtgAppFlow)
        If Not Page.IsPostBack Then
            objINV.GetLocationDesc(LocDesc, SubLocDesc)
            dsAllInfo = objINV.getMRSForAppr(Request.QueryString("MRSNo"))
            DisplayData()
            GenerateTab()
            Bindgrid()
            BindgridApprFlow()
        End If

        cmdPrint.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewMRSSlip.aspx", "MRSNo=" & Trim(Request.QueryString("MRSNo")) & "&CoyID=" & Session("CompanyID") & "") & "')")

        If Request.QueryString("caller") = "SearchInventoryReq_All" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "SearchInventoryReq_All.aspx", "pageid=" & strPageId)
        ElseIf Request.QueryString("caller") = "SearchMRS_All" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "SearchMRS_All.aspx", "pageid=" & strPageId)
        ElseIf Request.QueryString("caller") = "MRSListing" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "MRSListing.aspx", "pageid=" & strPageId)
        ElseIf Request.QueryString("caller") = "MRSListing_Requestor" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "MRSListing_Requestor.aspx", "pageid=" & strPageId)
        ElseIf Request.QueryString("caller") = "InventoryReqList" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "InventoryReqList.aspx", "pageid=" & strPageId)
        ElseIf Request.QueryString("caller") = "RISearch" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "ReturnInwardSearch.aspx", "pageid=" & strPageId)
        ElseIf Request.QueryString("caller") = "RIAckSearch" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "ReturnInwardAckSearch.aspx", "pageid=" & strPageId)
        ElseIf Request.QueryString("caller") = "RIAckListing" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "ReturnInwardAckListing.aspx", "pageid=" & strPageId)
        End If

    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        If Request.QueryString("caller") = "SearchMRS_All" Then
            Session("w_MRSDetail_tabs") = "<div class=""t_entity""><ul>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "SearchMRS_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "SearchMRS_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "MRSListing.aspx", "pageid=" & strPageId) & """><span>MRS Listing</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "</ul><div></div></div>"

        ElseIf Request.QueryString("caller") = "MRSListing" Then
            Session("w_MRSDetail_tabs") = "<div class=""t_entity""><ul>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "SearchMRS_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "SearchMRS_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "MRSListing.aspx", "pageid=" & strPageId) & """><span>MRS Listing</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "</ul><div></div></div>"

        ElseIf Request.QueryString("caller") = "MRSListing_Requestor" Then
            Session("w_MRSDetail_tabs") = "<div class=""t_entity""><ul>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "InventoryReq.aspx", "pageid=" & strPageId) & """><span>Inventory Requisition</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "InventoryReqList.aspx", "pageid=" & strPageId) & """><span>Inventory Requisition Listing</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "MRSAckListing.aspx", "pageid=" & strPageId) & """><span>MRS Acknowledge Listing</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "MRSListing_Requestor.aspx", "pageid=" & strPageId) & """><span>MRS Listing</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "</ul><div></div></div>"

        ElseIf Request.QueryString("caller") = "InventoryReqList" Then
            Session("w_MRSDetail_tabs") = "<div class=""t_entity""><ul>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "InventoryReq.aspx", "pageid=" & strPageId) & """><span>Inventory Requisition</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "InventoryReqList.aspx", "pageid=" & strPageId) & """><span>Inventory Requisition Listing</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "MRSAckListing.aspx", "pageid=" & strPageId) & """><span>MRS Acknowledge Listing</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "MRSListing_Requestor.aspx", "pageid=" & strPageId) & """><span>MRS Listing</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "</ul><div></div></div>"

        ElseIf Request.QueryString("caller") = "RISearch" Then
            Session("w_MRSDetail_tabs") = "<div class=""t_entity""><ul>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "ReturnInwardListing.aspx", "pageid=" & strPageId) & """><span>Return Inward</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "ReturnInwardSearch.aspx", "type=Listing&pageid=" & strPageId) & """><span>Return Inward Listing</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "</ul><div></div></div>"

        ElseIf Request.QueryString("caller") = "RIAckSearch" Then
            Session("w_MRSDetail_tabs") = "<div class=""t_entity""><ul>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "ReturnInwardAckListing.aspx", "pageid=" & strPageId) & """><span>Return Inward Ack</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "ReturnInwardAckSearch.aspx", "type=Listing&pageid=" & strPageId) & """><span>Return Inward Ack/Rejected Listing</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "</ul><div></div></div>"

        ElseIf Request.QueryString("caller") = "RIAckListing" Then
            Session("w_MRSDetail_tabs") = "<div class=""t_entity""><ul>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "ReturnInwardAckListing.aspx", "pageid=" & strPageId) & """><span>Return Inward Ack</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "ReturnInwardAckSearch.aspx", "type=Listing&pageid=" & strPageId) & """><span>Return Inward Ack/Rejected Listing</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "</ul><div></div></div>"

        ElseIf Request.QueryString("caller") = "SearchInventoryReq_All" Then
            Session("w_MRSDetail_tabs") = "<div class=""t_entity""><ul>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "SearchInventoryReq_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "SearchInventoryReq_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "</ul><div></div></div>"

        End If

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgItem.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Public Sub MyDataGrid_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgItem.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim dvViewMRS As DataView
        dvViewMRS = dsAllInfo.Tables("INVENTORY_REQUISITION_SLIP_DETAILS").DefaultView
        'dvViewSample(0)(0)
        If pSorted Then
            dvViewMRS.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewMRS.Sort += " DESC"
        End If

        intPageRecordCnt = dsAllInfo.Tables("INVENTORY_REQUISITION_SLIP_DETAILS").Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            dtgItem.DataSource = dvViewMRS
            dtgItem.DataBind()
        Else
            dtgItem.DataBind()
        End If

    End Function

    Private Sub dtgItem_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemCreated
        Grid_ItemCreated(dtgItem, e)

    End Sub

    Private Sub dtgItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim objInv As New Inventory
            Dim decMthBal, decLastMthAve As Decimal

            If dv("IRSD_IRS_STATUS") = "2" Then
                e.Item.Cells(EnumMRS.icStatus).Text = "Issued"
            ElseIf dv("IRSD_IRS_STATUS") = "6" Then
                e.Item.Cells(EnumMRS.icStatus).Text = "Rejected"
            ElseIf dv("IRSD_IRS_STATUS") = "7" Then
                e.Item.Cells(EnumMRS.icStatus).Text = "Issued, Rejected"
            End If

            Court_loc = Court_loc + 1
            Dim strJQLoc As String = "jqLoc" & Court_loc
            If DisplayLocPopup(strJQLoc, dv("IRSD_IRS_LINE")) Then
                e.Item.Cells(EnumMRS.icQty).Text = dv("IRSD_QTY") & "<span style=""cursor:default;"" class=""" & strJQLoc & """><IMG src=""" & dDispatcher.direct("Plugins/images", "d_icon.gif") & """></span>"
            Else
                e.Item.Cells(EnumMRS.icQty).Text = dv("IRSD_QTY")
            End If

            'decMthBal = objInv.getMonthStockBalance(dv("IM_ITEM_CODE"), ViewState("IRM_CREATED_BY"))
            'e.Item.Cells(EnumMRS.icMthIssue).Text = Format(decMthBal, "###0.00")

            'decLastMthAve = objInv.getLast3MthAve(dv("IM_ITEM_CODE"), ViewState("IRSM_IRS_DEPARTMENT"))
            'e.Item.Cells(EnumMRS.icLastMth).Text = Format(decLastMthAve, "###0.00")

            objInv = Nothing

        End If
    End Sub

    Sub DisplayData()
        Dim dtHeader As New DataTable
        dtHeader = dsAllInfo.Tables(0)
        If Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_URGENT")) = "Y" Then
            lblMRSNo.Text = Me.Request.QueryString("MRSNo") & " (Urgent)"
        Else
            lblMRSNo.Text = Me.Request.QueryString("MRSNo")
        End If

        ViewState("MRSNo") = Me.Request.QueryString("MRSNo")
        ViewState("IRM_IR_INDEX") = Common.parseNull(dtHeader.Rows(0)("IRM_IR_INDEX"))
        ViewState("IRSM_IRS_INDEX") = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_INDEX"))
        lblMRSDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dtHeader.Rows(0)("IRSM_IRS_DATE"))
        lblIssueTo.Text = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_ISSUE_TO"))

        ViewState("IRM_CREATED_BY") = dtHeader.Rows(0)("IRM_CREATED_BY")
        ViewState("IRSM_IRS_DEPARTMENT") = dtHeader.Rows(0)("IRSM_IRS_DEPARTMENT")
        lblDepartment.Text = dtHeader.Rows(0)("CDM_DEPT_NAME") 'Department

        lblRefNo.Text = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_REF_NO"))
        lblRemark.Text = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_REMARK"))
        If IsDBNull(dtHeader.Rows(0)("CS_SEC_NAME")) Then
            lblSection.Text = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_SECTION"))
        Else
            lblSection.Text = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_SECTION")) & " : " & dtHeader.Rows(0)("CS_SEC_NAME")
        End If
        lblRequestor.Text = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_REQUESTOR_NAME"))
        If IsDBNull(dtHeader.Rows(0)("IRSM_IRS_APPROVED_DATE")) Then
            lblIssuedDate.Text = ""
        Else
            lblIssuedDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dtHeader.Rows(0)("IRSM_IRS_APPROVED_DATE"))
        End If
        If Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_STATUS")) = "2" Then
            lblStatus.Text = "Issued"
        ElseIf Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_STATUS")) = "3" Then
            lblStatus.Text = "Acknowledged"
        ElseIf Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_STATUS")) = "4" Then
            lblStatus.Text = "Auto-Acknowledged"
        ElseIf Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_STATUS")) = "5" Then
            lblStatus.Text = "Cancelled"
        ElseIf Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_STATUS")) = "6" Then
            lblStatus.Text = "Rejected"
        ElseIf Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_STATUS")) = "7" Then
            lblStatus.Text = "Partial Issued"
        ElseIf Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_STATUS")) = "1" Then
            lblStatus.Text = "New"
        Else
            lblStatus.Text = ""
        End If

        txtIssueRemark.Text = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_ISSUE_REMARK"))
        txtAckRemark.Text = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_ACKCANCEL_REMARK"))

        If Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_STATUS")) = "1" Then
            tr_IssueRemark.Style("display") = "none"
            tr_AckRemark.Style("display") = "none"
        ElseIf Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_STATUS")) = "2" Or Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_STATUS")) = "6" Or Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_STATUS")) = "7" Then
            tr_AckRemark.Style("display") = "none"
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

    Private Sub BindgridApprFlow()
        Dim objInv As New Inventory
        ds = objInv.getApprFlow(ViewState("IRM_IR_INDEX"), , "MRS", ViewState("IRSM_IRS_INDEX"))
        objInv = Nothing
        dtgAppFlow.DataSource = ds.Tables(0)
        dtgAppFlow.DataBind()
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
            'End If

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

    Function DisplayLocPopup(ByVal strJQ As String, ByVal intLine As Integer) As Boolean
        Dim ds As New DataSet
        Dim i As Integer
        Dim aryTemp As New ArrayList()
        Dim strLot, strLoc, strSubLoc, strQty As String

        strJQ = Replace(strJQ, "jq", "")
        ds = objINV.getMRSLot(ViewState("MRSNo"), intLine)
        aryTemp.Clear()

        If ds.Tables(0).Rows.Count > 0 Then
            For i = 0 To ds.Tables(0).Rows.Count - 1
                strLot = Common.parseNull(ds.Tables(0).Rows(i).Item("DOL_LOT_NO"))
                strLoc = Common.parseNull(ds.Tables(0).Rows(i).Item("LM_LOCATION"))
                strSubLoc = Common.parseNull(ds.Tables(0).Rows(i).Item("LM_SUB_LOCATION"))
                strQty = Common.parseNull(ds.Tables(0).Rows(i).Item("IRSL_LOT_QTY"))

                aryTemp.Add(New String() {strLot, strLoc, strSubLoc, strQty})
            Next

            ContructRow(strJQ, aryTemp)
        Else
            Return False
        End If

        DisplayLocPopup = True

    End Function

    Private Function ContructRow(ByVal strTemp As String, ByVal aryVolume As ArrayList) As String
        Dim strrow, strtable As String
        Dim i As Integer
        strrow = ""

        For i = 0 To aryVolume.Count - 1
            strrow &= "Lot: " & aryVolume(i)(0) & ", Qty: " & aryVolume(i)(3) & "," & IIf(LocDesc = "", "Location", LocDesc) & ": " & aryVolume(i)(1) & ", " & IIf(SubLocDesc = "", "Sub-Location", SubLocDesc) & ": " & aryVolume(i)(2) & "<BR>"
        Next

        strtable = strrow

        Session("jqPopup") = Session("jqPopup") & "$('.jq" & strTemp & "').CreateBubblePopup({innerHtml: '" & strtable & "',position:'left', align: 'middle', innerHtmlStyle: { 'text-align':'left' },themeName:'all-black',themePath:'../../Common/Plugins/images/jquerybubblepopup-theme'});"

    End Function
End Class
