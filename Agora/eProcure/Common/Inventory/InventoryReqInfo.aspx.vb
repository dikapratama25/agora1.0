'Copyright © 2011 STRATEQ GLOBAL SERVICES. All rights reserved.
Imports AgoraLegacy
Imports eProcure.Component


Public Class InventoryReqInfo
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents lblIRNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblIssueDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblIssueTo As System.Web.UI.WebControls.Label
    Protected WithEvents lblQty As System.Web.UI.WebControls.Label
    Protected WithEvents lblRefNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblRemark As System.Web.UI.WebControls.Label
    Protected WithEvents lblDepartment As System.Web.UI.WebControls.Label
    Protected WithEvents lblSection As System.Web.UI.WebControls.Label
    Protected WithEvents lblRequestor As System.Web.UI.WebControls.Label
    Protected WithEvents lblApprovedDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label

    Protected WithEvents dtgAppFlow As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtgItem As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdPrint As System.Web.UI.HtmlControls.HtmlInputButton

    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objINV As New Inventory
    Dim objGLO As New AppGlobals
    Dim aryInvReq As New ArrayList()
    Dim intNumeric As Boolean = True
    Dim strMsg As String = ""
    Dim LocDesc, SubLocDesc As String

    Enum EnumIRD
        icItemCode
        icItemName
        icUom
        icMthIssue
        icLastMth
        icQty
        icLoc
        icSubLoc
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
        blnPaging = False
        blnSorting = False
        SetGridProperty(dtgItem)
        SetGridProperty(dtgAppFlow)
        If Not Page.IsPostBack Then
            DisplayData()
            GenerateTab()
            Bindgrid()
            BindgridApprFlow()
        End If

        cmdPrint.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewIRSlip.aspx", "IRNo=" & Trim(Request.QueryString("IRNo")) & "&CoyID=" & Session("CompanyID") & "") & "')")

        If Request.QueryString("caller") = "SearchInventoryReq_All" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "SearchInventoryReq_All.aspx", "pageid=" & strPageId)
        ElseIf Request.QueryString("caller") = "InventoryReqList" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "InventoryReqList.aspx", "pageid=" & strPageId)
        ElseIf Request.QueryString("caller") = "MRSListing" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "MRSListing.aspx", "pageid=" & strPageId)
        ElseIf Request.QueryString("caller") = "MRSListing_Requestor" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "MRSListing_Requestor.aspx", "pageid=" & strPageId)
        ElseIf Request.QueryString("caller") = "Dashboard" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId)
        End If

    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        If Request.QueryString("caller") = "SearchInventoryReq_All" Then
            Session("w_InventoryReq_tabs") = "<div class=""t_entity""><ul>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "SearchInventoryReq_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "SearchInventoryReq_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "</ul><div></div></div>"

        ElseIf Request.QueryString("caller") = "InventoryReqList" Or Request.QueryString("caller") = "Dashboard" Then
            Session("w_InventoryReq_tabs") = "<div class=""t_entity""><ul>" & _
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

        ElseIf Request.QueryString("caller") = "MRSListing" Then
            Session("w_InventoryReq_tabs") = "<div class=""t_entity""><ul>" & _
                                 "<li><div class=""space""></div></li>" & _
                                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "SearchMRS_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "SearchMRS_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                                 "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "MRSListing.aspx", "pageid=" & strPageId) & """><span>MRS Listing</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                                 "</ul><div></div></div>"

        ElseIf Request.QueryString("caller") = "MRSListing_Requestor" Then
            Session("w_InventoryReq_tabs") = "<div class=""t_entity""><ul>" & _
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
        Dim ds As DataSet = New DataSet
        Dim strLoc, strSubLoc As String
        strLoc = ""
        strSubLoc = "" 

        ds = objINV.getInventoryItemInfoFiltered(Request.QueryString("IRNo"), True)
        Dim dvViewItem As DataView
        dvViewItem = ds.Tables(0).DefaultView

        dvViewItem.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewItem.Sort += " DESC"

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count
        If ViewState("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtgItem, dvViewItem)

            dtgItem.DataSource = dvViewItem
            dtgItem.DataBind()
        Else
            dtgItem.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        If objDb.GetVal("SELECT CM_LOCATION_STOCK FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Session("CompanyId") & "'") = "N" Then
            dtgItem.Columns(EnumIRD.icLoc).Visible = False
            dtgItem.Columns(EnumIRD.icSubLoc).Visible = False
        Else
            dtgItem.Columns(EnumIRD.icLoc).Visible = True
            dtgItem.Columns(EnumIRD.icSubLoc).Visible = True
        End If

        dtgItem.Columns(EnumIRD.icMthIssue).Visible = True
        dtgItem.Columns(EnumIRD.icLastMth).Visible = True

        'If Request.QueryString("caller") = "SearchInventoryReq_All" Then
        '    dtgItem.Columns(EnumIRD.icMthIssue).Visible = True
        '    dtgItem.Columns(EnumIRD.icLastMth).Visible = True
        '    dtgItem.Columns(EnumIRD.icLoc).Visible = False
        '    dtgItem.Columns(EnumIRD.icSubLoc).Visible = False

        '    'ElseIf Request.QueryString("caller") = "InventoryReqList" Or Request.QueryString("caller") = "Dashboard" Then
        '    '    If objDb.GetVal("SELECT CM_LOCATION_STOCK FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Session("CompanyId") & "'") = "N" Then
        '    '        dtgItem.Columns(EnumIRD.icLoc).Visible = False
        '    '        dtgItem.Columns(EnumIRD.icSubLoc).Visible = False
        '    '    Else
        '    '        dtgItem.Columns(EnumIRD.icLoc).Visible = True
        '    '        dtgItem.Columns(EnumIRD.icSubLoc).Visible = True
        '    '    End If

        '    '    dtgItem.Columns(EnumIRD.icMthIssue).Visible = True
        '    '    dtgItem.Columns(EnumIRD.icLastMth).Visible = True
        'Else
        '    If objDb.GetVal("SELECT CM_LOCATION_STOCK FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Session("CompanyId") & "'") = "N" Then
        '        dtgItem.Columns(EnumIRD.icLoc).Visible = False
        '        dtgItem.Columns(EnumIRD.icSubLoc).Visible = False
        '    Else
        '        dtgItem.Columns(EnumIRD.icLoc).Visible = True
        '        dtgItem.Columns(EnumIRD.icSubLoc).Visible = True
        '    End If

        '    dtgItem.Columns(EnumIRD.icMthIssue).Visible = True
        '    dtgItem.Columns(EnumIRD.icLastMth).Visible = True
        'End If

        ViewState("PageCount") = dtgItem.PageCount
    End Function

    Private Sub dtgItem_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgItem, e)

        objINV.GetLocationDesc(LocDesc, SubLocDesc)
        LocDesc = IIf(LocDesc = "", "Location", "" + LocDesc + "")
        SubLocDesc = IIf(SubLocDesc = "", "Sub Location", "" + SubLocDesc + "")

        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(EnumIRD.icLoc).Text = "" + LocDesc
            e.Item.Cells(EnumIRD.icSubLoc).Text = "" + SubLocDesc
        End If
        
    End Sub

    Private Sub dtgItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            If IsDBNull(dv("LM_LOCATION")) Then
                e.Item.Cells(EnumIRD.icLoc).Text = ""
            Else
                e.Item.Cells(EnumIRD.icLoc).Text = dv("LM_LOCATION")
            End If

            If IsDBNull(dv("LM_SUB_LOCATION")) Then
                e.Item.Cells(EnumIRD.icSubLoc).Text = ""
            Else
                e.Item.Cells(EnumIRD.icSubLoc).Text = dv("LM_SUB_LOCATION")
            End If
        End If
    End Sub

    Sub DisplayData()
        Dim dsAllInfo As New DataSet
        Dim strDeptName As String
        dsAllInfo = objINV.getIRHeaderDetail(Me.Request.QueryString("IRNo"))
        If Common.parseNull(dsAllInfo.Tables(0).Rows(0)("IRM_IR_URGENT")) = "Y" Then
            lblIRNo.Text = Me.Request.QueryString("IRNo") & " (Urgent)"
        Else
            lblIRNo.Text = Me.Request.QueryString("IRNo")
        End If

        ViewState("IRM_IR_INDEX") = Common.parseNull(dsAllInfo.Tables(0).Rows(0)("IRM_IR_INDEX"))
        lblIssueDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dsAllInfo.Tables(0).Rows(0)("IRM_IR_DATE"))
        lblIssueTo.Text = Common.parseNull(dsAllInfo.Tables(0).Rows(0)("IRM_IR_ISSUE_TO"))

        strDeptName = objDb.GetVal("SELECT CDM_DEPT_NAME FROM COMPANY_DEPT_MSTR WHERE CDM_COY_ID = '" & Session("CompanyID") & "' " & _
                    "AND CDM_DEPT_CODE = '" & dsAllInfo.Tables(0).Rows(0)("IRM_IR_DEPARTMENT") & "' ")
        lblDepartment.Text = strDeptName

        lblRefNo.Text = Common.parseNull(dsAllInfo.Tables(0).Rows(0)("IRM_IR_REF_NO"))
        lblRemark.Text = Common.parseNull(dsAllInfo.Tables(0).Rows(0)("IRM_IR_REMARK"))
        If IsDBNull(dsAllInfo.Tables(0).Rows(0)("CS_SEC_NAME")) Then
            lblSection.Text = Common.parseNull(dsAllInfo.Tables(0).Rows(0)("IRM_IR_SECTION"))
        Else
            lblSection.Text = Common.parseNull(dsAllInfo.Tables(0).Rows(0)("IRM_IR_SECTION")) & " : " & dsAllInfo.Tables(0).Rows(0)("CS_SEC_NAME")
        End If

        lblRequestor.Text = Common.parseNull(dsAllInfo.Tables(0).Rows(0)("IRM_IR_REQUESTOR_NAME"))
        If IsDBNull(dsAllInfo.Tables(0).Rows(0)("IRM_IR_APPROVED_DATE")) Then
            lblApprovedDate.Text = ""
        Else
            lblApprovedDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dsAllInfo.Tables(0).Rows(0)("IRM_IR_APPROVED_DATE"))
        End If
        If Common.parseNull(dsAllInfo.Tables(0).Rows(0)("IRM_IR_STATUS")) = "1" Then
            lblStatus.Text = "Submitted"
        ElseIf Common.parseNull(dsAllInfo.Tables(0).Rows(0)("IRM_IR_STATUS")) = "2" Then
            lblStatus.Text = "Approved"
        ElseIf Common.parseNull(dsAllInfo.Tables(0).Rows(0)("IRM_IR_STATUS")) = "3" Then
            lblStatus.Text = "Pending Approval"
        ElseIf Common.parseNull(dsAllInfo.Tables(0).Rows(0)("IRM_IR_STATUS")) = "4" Then
            lblStatus.Text = "Rejected"
        Else
            lblStatus.Text = ""
        End If

    End Sub

    Private Sub BindgridApprFlow()
        Dim objInv As New Inventory
        Dim dsAppFlow As New DataSet
        dsAppFlow = objInv.getApprFlow(ViewState("IRM_IR_INDEX"), , "MRS")
        objInv = Nothing
        dtgAppFlow.DataSource = dsAppFlow.Tables(0)
        dtgAppFlow.DataBind()
    End Sub

    Private Sub dtgAppFlow_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAppFlow.ItemCreated
        Grid_ItemCreated(sender, e)
    End Sub

    Private Sub dtgAppFlow_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAppFlow.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim intTotalCell, intLoop As Integer
            If dv("IRA_Seq") - 1 = dv("IRA_AO_Action") Then
                intTotalCell = e.Item.Cells.Count - 1
                For intLoop = 0 To intTotalCell
                    e.Item.Cells(intLoop).Font.Bold = True
                Next
            End If

            If dv("IRA_APPROVAL_TYPE") = 1 Then
                e.Item.Cells(3).Text = "Approval"
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
                e.Item.Cells(4).Text = Format(CDate(dv("IRA_ACTION_DATE")), "dd/MM/yyyy") & " " & Format(dv("IRA_ACTION_DATE"), "HH:mm:ss")
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
        Dim ds As DataSet = pr.getUserAttach("AO", tb_iqc, CStr(ViewState("IRM_IR_INDEX")), id1, id2, seq)

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
End Class
