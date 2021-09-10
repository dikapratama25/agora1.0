'//Attachment Ind
Imports AgoraLegacy
Imports eProcure.Component
Public Class SearchInventoryReq_All
    Inherits AgoraLegacy.AppBaseClass
    'Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtDateFr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBuyer As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkApproved As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkReject As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents vldDateFtDateTo As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents vldDateFr As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents vldDateTo As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblVendor As System.Web.UI.WebControls.Label
    Protected WithEvents lblStartDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblEndDate As System.Web.UI.WebControls.Label

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals

    Public Enum EnumIR
        icIRNo
        icIRDate
        icApprovedDate
        icIssueTo
        icDept
        icRefNo
        icRemark
        icStatus
        icMRSNo
    End Enum
    'Dim strCaller As String
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
        blnCheckBox = False
        SetGridProperty(dtgIRList)

        MyBase.Page_Load(sender, e)

        If Not Page.IsPostBack Then
            txtDateFr.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
            hidDateS.Value = txtDateFr.Text
            txtDateTo.Text = DateTime.Now.ToShortDateString()
            hidDateE.Value = txtDateTo.Text
            Session("strURL") = strCallFrom
            GenerateTab()
        End If
    End Sub

    Public Sub dtgIRList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgIRList.PageIndexChanged
        dtgIRList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgIRList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim objInv As New Inventory 'PurchaseOrder

        '//Retrieve Data from Database
        Dim ds As DataSet
        Dim strAOAction As String = ""
        Dim strInclude As String = ""
        Dim strIncludeHold As String = ""
        Dim strMsg As String
        Dim comparedt As Date
        comparedt = DateAdd("m", -6, CDate(txtDateTo.Text))

        If CDate(txtDateFr.Text) < comparedt Then
            strMsg = "Start/ End date " & objGlobal.GetErrorMessage("00015") & " 6 months"
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Return False
        End If

        If chkApproved.Checked Then
            strAOAction = "Approved"
        End If

        If chkInclude.Checked Then
            strInclude = "Included"
        End If

        If chkReject.Checked Then
            strAOAction = "Rejected"
        End If

        If chkApproved.Checked And chkReject.Checked Then strAOAction = ""
        ds = objInv.GetInvReqApprList(txtIRNo.Text, "", "", "", txtDateFr.Text, txtDateTo.Text, "app", strAOAction, strInclude)
  
        '//for sorting asc or desc
        Dim dvViewIR As DataView
        dvViewIR = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewIR.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewIR.Sort += " DESC"
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgIRList, dvViewIR)
            dtgIRList.DataSource = dvViewIR
            dtgIRList.DataBind()
        Else
            dtgIRList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ViewState("PageCount") = dtgIRList.PageCount
    End Function

    Private Sub dtgIRList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIRList.ItemCreated
        Grid_ItemCreated(dtgIRList, e)
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgIRList.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "IRM_IR_DATE"
        Bindgrid(True)
    End Sub

    Private Sub dtgIRList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIRList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim strURL As String
            Dim i As Integer
            Dim dsMrs As New DataSet
            Dim intStatus, strMRSNo As String
            Dim objInv As New Inventory

            Dim lnkIRNo As HyperLink
            lnkIRNo = e.Item.Cells(EnumIR.icIRNo).FindControl("lnkIRNo")
            lnkIRNo.NavigateUrl = dDispatcher.direct("Inventory", "InventoryReqInfo.aspx", "caller=SearchInventoryReq_All&pageid=" & strPageId & "&index=" & dv("IRM_IR_INDEX") & "&IRNo=" & dv("IRM_IR_NO"))
            lnkIRNo.Text = dv("IRM_IR_NO")
      
            If Common.parseNull(dv("IRM_IR_URGENT")) = "Y" Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "U Icon.gif")
                e.Item.Cells(EnumIR.icIRNo).Controls.Add(imgAttach)
            End If

            If IsDBNull(dv("IRM_IR_DATE")) Then
                e.Item.Cells(EnumIR.icIRDate).Text = ""
            Else
                e.Item.Cells(EnumIR.icIRDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IRM_IR_DATE"))
            End If

            If IsDBNull(dv("IRM_IR_APPROVED_DATE")) Then
                e.Item.Cells(EnumIR.icApprovedDate).Text = ""
            Else
                e.Item.Cells(EnumIR.icApprovedDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IRM_IR_APPROVED_DATE"))
            End If

            dsMrs = objInv.GetMRSNoAllIR(dv("IRM_IR_NO"))
            If dsMrs.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsMrs.Tables(0).Rows.Count - 1
                    'strMRSNo &= dsMrs.Tables(0).Rows(i)("IRSM_IRS_NO") & "<br/>"
                    strMRSNo &= "<A href=""" & dDispatcher.direct("Inventory", "MRSDetail.aspx", "caller=SearchInventoryReq_All&pageid=" & strPageId & "&index=" & dsMrs.Tables(0).Rows(i)("IRSM_IRS_INDEX") & "&MRSNo=" & dsMrs.Tables(0).Rows(i)("IRSM_IRS_NO")) & """ ><font color=#0000ff>" & dsMrs.Tables(0).Rows(i)("IRSM_IRS_NO") & "</font></A><br/>"
                Next
                '"<a href=""" & dDispatcher.direct("PR", "SearchPR_Cancellation_ByPO.aspx", "pageid=" & strPageId) & """><span>Cancelled PR</span></a>"
                e.Item.Cells(EnumIR.icMRSNo).Text = strMRSNo
            End If
            'Dim objPR As New PurchaseReq2
            'If objPR.HasAttachment(dv("POM_PO_No"), "PO") Then
            '    Dim imgAttach As New System.Web.UI.WebControls.Image
            '    imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
            '    imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")
            '    e.Item.Cells(EnumPO.icPONo).Controls.Add(imgAttach)
            'End If

            'If Not IsDBNull(dv("POM_RFQ_INDEX")) AndAlso CStr(dv("POM_RFQ_INDEX")) <> "" Then
            '    Dim lnkRFQ As New HyperLink
            '    Dim strRFQNo, strRFQName As String
            '    objPR.getRFQName(dv("POM_RFQ_INDEX"), strRFQNo, strRFQName)
            '    lnkRFQ.ImageUrl = dDispatcher.direct("Plugins/images", "Q-3 Icon (10x10).jpg")
            '    'Michelle (5/4/2013) - Issue 1879
            '    'If dv("POM_PO_STATUS") = 1 Or dv("POM_PO_STATUS") = 2 Or dv("POM_PO_STATUS") = 3 _
            '    '    Or dv("POM_PO_STATUS") = 4 Or dv("POM_PO_STATUS") = 5 Or dv("POM_PO_STATUS") = 6 Then
            '    '    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "viewQoute.aspx", "Frm=SearchPO_All&pageid=" & strPageId & "&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Num=" & strRFQNo & "&vcomid=" & Common.parseNull(dv("POM_S_Coy_ID")) & "&side=quote")
            '    '    lnkRFQ.ToolTip = "Click here to view quotation details"
            '    'Else
            '    lnkRFQ.NavigateUrl = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=SearchPO_All&pageid=" & strPageId & "&RFQ_Num=" & strRFQNo & "&RFQ_ID=" & dv("POM_RFQ_INDEX") & "&RFQ_Name=" & Server.UrlEncode(strRFQName) & "&side=other")
            '    lnkRFQ.ToolTip = "Click here to view quotation comparison"
            '    'End If
            '    e.Item.Cells(EnumPO.icPONo).Controls.Add(lnkRFQ)
            '    Session("strurl") = strCallFrom
            'End If
            'objPR = Nothing
            'e.Item.Cells(EnumPO.icCreationDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("POM_CREATED_Date"))

            'If Not IsDBNull(dv("PO_AMT")) Then
            '    e.Item.Cells(EnumPO.icAmt).Text = Format(dv("PO_AMT"), "#,##0.00")
            'End If


            ''If intStatus = PRStatus.ConvertedToPO Then
            ''    If e.Item.Cells(EnumPO.icVendor).Text <> "Multiple Vendors" Then
            ''        e.Item.Cells(EnumPO.icStatus).Text = e.Item.Cells(EnumPO.icStatus).Text & Space(4) & _
            ''        "<a href='PODetail.aspx?caller=buyer&pageid=" & strPageId & "&PO_INDEX=" & Common.parseNull(dv("PRM_PO_INDEX")) & "&PO_NO=" & Common.parseNull(dv("PO_NO")) & "&BCoyID=" & Session("CompanyID") & "&side=other&filetype=2'>" & "(PO #" & Common.parseNull(dv("PO_NO")) & ")</a>"

            ''    Else
            ''        '-----------Added Name to Url By Praveen on 23/07/2007 and on 01/08/2007(Addesd Caller)
            ''        e.Item.Cells(EnumPO.icStatus).Text = e.Item.Cells(EnumPO.icStatus).Text & Space(4) & _
            ''       "<a href='POViewB3.aspx?caller=" & strCaller & "&pageid=" & strPageId & "&PO_INDEX=" & Common.parseNull(dv("PRM_PO_INDEX")) & "&prid=" & dv("PRM_PR_No") & "&PO_NO=" & Common.parseNull(dv("PO_NO")) & "&BCoyID=" & Session("CompanyID") & "&side=other&filetype=2'>" & "(Multiple POs)<font color=#0000ff></font></a>"

            ''    End If
            ''End If

            '' ai chu add
            ''--------New Code Added for MultiPle Vendors By Praveen on 18/07/2007
            ''If e.Item.Cells(EnumPO.icVendor).Text <> "Multiple Vendors" Then
            'e.Item.Cells(EnumPO.icVendor).Text = "<a href='" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "pageid=" & strPageId & "&v_com_id=" & Common.parseNull(dv("POM_S_Coy_ID"))) & "'>" & e.Item.Cells(EnumPO.icVendor).Text & "</a>"
            ''Else '
            ''    'e.Item.Cells(EnumPO.icVendor).Text = "<a href='../PR/Multi_VendorDetails.aspx?pageid=" & strPageId & "&v_com_id=" & Common.parseNull(dv("PRM_S_Coy_ID")) & "'>" & e.Item.Cells(EnumPO.icVendor).Text & "</A>"
            ''    e.Item.Cells(EnumPO.icVendor).Text = "<a href='../PR/Multi_VendorDetails.aspx?caller=" & strCaller & "&pageid=" & strPageId & "&PRNum=" & Common.parseNull(dv("PRM_PR_NO")) & "'>" & e.Item.Cells(EnumPO.icVendor).Text & "</A>"
            ''End If
            ''--------End 
            ''PRM_STATUS_CHANGED_BY
            ''If intStatus = PRStatus.CancelledBy Or intStatus = PRStatus.RejectedBy Or intStatus = PRStatus.HeldBy Then
            ''    If IsDBNull(dv("CHANGED_BY_NAME")) Then
            ''        e.Item.Cells(EnumPO.icStatus).Text = e.Item.Cells(EnumPO.icStatus).Text & " (" & Common.parseNull(dv("PRM_STATUS_CHANGED_BY")) & ")"
            ''    Else
            ''        e.Item.Cells(EnumPO.icStatus).Text = e.Item.Cells(EnumPO.icStatus).Text & " (" & dv("CHANGED_BY_NAME") & ")"
            ''    End If

            ''End If

            'If intStatus = POStatus_new.HeldBy Then
            '    e.Item.Cells(EnumPO.icStatus).Text = "Held by " & Common.parseNull(dv("NAME")) & ""
            'End If

            'Dim objPO As New PurchaseOrder
            'Dim objDB As New EAD.DBCom
            ''Get PR Number for PO converted from PR
            'Dim COUNT As Integer
            'Dim ARRAY(100) As String
            'Dim STR As String
            'Dim i As Integer
            'Dim strPR_Index, strPRD_RFQ_Index, strRFQ_No As String
            'Dim lnkPRNo As HyperLink
            'lnkPRNo = e.Item.Cells(EnumPO.icPRNo).FindControl("lnkPRNo")

            ''Session("urlrefererPO") = "PRAll"
            'strPRD_RFQ_Index = objDB.GetVal("SELECT IFNULL(POM_RFQ_INDEX,'') AS POM_RFQ_INDEX FROM po_mstr WHERE pom_po_no = '" & dv("POM_PO_No") & "' AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")

            'If strPRD_RFQ_Index <> "" Then
            '    strRFQ_No = objDB.GetVal("SELECT IFNULL(RM_RFQ_NO,'') AS RM_RFQ_NO  FROM rfq_mstr WHERE RM_RFQ_ID = '" & strPRD_RFQ_Index & "' AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'")

            '    objPO.GetPRNoAllRFQ(strRFQ_No, ARRAY, COUNT)
            '    'objPO.GetPRNo1(dv("POM_PO_INDEX"), ARRAY, COUNT)

            '    If ARRAY(0) <> "" Then
            '        For i = 0 To COUNT - 1
            '            'lnkPRNo.Text = ARRAY(i)
            '            strPR_Index = objDB.GetVal("SELECT PRM_PR_INDEX FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO = '" & ARRAY(i) & "'")
            '            If strPR_Index <> "" Then
            '                'STR = STR & "<A>" & ARRAY(i) & "</A><br>"
            '                'STR = STR & "<A href=""#"" onclick=" & dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&index=" & strPR_Index & "&PRNO=" & ARRAY(i) & "&type=mod&mode=bc") & "><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
            '                'lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&index=" & strPR_Index & "&PRNO=" & ARRAY(i) & "&type=mod&mode=bc") & "')"
            '                STR = STR & "<A href=""" & dDispatcher.direct("PR", "PRDetail.aspx", "caller=SearchPO_All&pageid=" & strPageId & "&index=" & strPR_Index & "&PRNO=" & ARRAY(i) & "&type=mod&mode=bc") & """ ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
            '            End If
            '        Next
            '    End If
            'Else
            '    objPO.GetPRNoAll(dv("POM_PO_No"), ARRAY, COUNT)
            '    'objPO.GetPRNo1(dv("POM_PO_INDEX"), ARRAY, COUNT)

            '    If ARRAY(0) <> "" Then
            '        For i = 0 To COUNT - 1
            '            'lnkPRNo.Text = ARRAY(i)
            '            strPR_Index = objDB.GetVal("SELECT PRM_PR_INDEX FROM PR_MSTR WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRM_PR_NO = '" & ARRAY(i) & "'")
            '            If strPR_Index <> "" Then
            '                'STR = STR & "<A>" & ARRAY(i) & "</A><br>"
            '                'STR = STR & "<A href=""#"" onclick=" & dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&index=" & strPR_Index & "&PRNO=" & ARRAY(i) & "&type=mod&mode=bc") & "><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
            '                'lnkPRNo.NavigateUrl = dDispatcher.direct("PR", "PRDetail.aspx", "pageid=" & strPageId & "&index=" & strPR_Index & "&PRNO=" & ARRAY(i) & "&type=mod&mode=bc") & "')"
            '                STR = STR & "<A href=""" & dDispatcher.direct("PR", "PRDetail.aspx", "caller=SearchPO_All&pageid=" & strPageId & "&index=" & strPR_Index & "&PRNO=" & ARRAY(i) & "&type=mod&mode=bc") & """ ><font color=#0000ff>" & ARRAY(i) & "</font></A><br/>"
            '            End If
            '        Next
            '    End If
            'End If
            'e.Item.Cells(EnumPO.icPRNo).Text = STR

            'dblTotal = dblTotal + dv("PR_AMT")
        End If
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_SearchIRAll_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "SearchInventoryReq_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "SearchInventoryReq_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
    End Sub
End Class


