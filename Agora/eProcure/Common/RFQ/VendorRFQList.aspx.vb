Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing
Imports System.IO
Imports MySql.Data.MySqlClient
Imports Microsoft.Reporting.WebForms

Public Class VendorRFQList
    Inherits AgoraLegacy.AppBaseClass
    'Dim objDO As New Dashboard
    Dim objrfq As New RFQ
    Dim dDispatcher As New AgoraLegacy.dispatcher
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    ''Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    ''Protected WithEvents Table4 As System.Web.UI.HtmlControls.HtmlTable
    'Dim objinv As New Invoice
    'Dim paid As Double
    'Dim strMode As String
    'Dim ordered_amount As Double
    'Protected WithEvents dtg_InvList As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents dtg_inv2 As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents cmd_createInv As System.Web.UI.WebControls.Button
    'Protected WithEvents cmd_submit As System.Web.UI.WebControls.Button
    'Protected WithEvents tblSearchResult As System.Web.UI.HtmlControls.HtmlTable
    ''Protected WithEvents txt_DocNo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents back_view As System.Web.UI.HtmlControls.HtmlAnchor
    'Protected WithEvents back As System.Web.UI.HtmlControls.HtmlAnchor

    ''Protected WithEvents txt_bcom As System.Web.UI.WebControls.TextBox
    'Protected WithEvents lblStep1 As System.Web.UI.WebControls.Label
    'Protected WithEvents lblStep2 As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    'Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    'Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    'Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    'Dim strDONo, strVMode, strLocID, strGRNNo, strBCoyID, strPONo, strFrm, strtemp As String
    'Dim intPOIdx As Integer\
    Protected WithEvents dtgOutstandingRFQ As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
   
    'Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
    '    'img_delete.Enabled = False
    '    'Imagebutton1.Enabled = False
    '    'cmd_emp_trash.Enabled = False

    '    Dim alButtonList As ArrayList
    '    alButtonList = New ArrayList
    '    'alButtonList.Add(img_delete)
    '    'alButtonList.Add(Imagebutton1)
    '    alButtonList.Add(cmdDelete)
    '    htPageAccess.Add("delete", alButtonList)

    '    If intPageRecordCnt > 0 Then
    '        cmdDelete.Enabled = blnCanDelete 'img_delete.Enabled = blnCanDelete
    '        'Imagebutton1.Enabled = blnCanDelete
    '        'cmd_emp_trash.Enabled = blnCanDelete
    '    Else
    '        cmdDelete.Enabled = False 'img_delete.Enabled = False
    '        'Imagebutton1.Enabled = False
    '        'cmd_emp_trash.Enabled = False
    '    End If

    '    CheckButtonAccess()
    '    alButtonList.Clear()
    'End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)

        If Not Page.IsPostBack Then
            GenerateTab()
            Me.cmdDelete.Attributes.Add("onclick", "return cmdAddClick();")
            SetGridProperty(dtgOutstandingRFQ)
            dtgOutstandingRFQ.CurrentPageIndex = 0
            ViewState("SortAscendingOutstandingRFQ") = "no"
            ViewState("SortExpressionOutstandingRFQ") = "Creation Date"
            ViewState("SortAscending") = "no"
            ViewState("SortExpression") = "Creation Date"
            BindgridOutstandingRFQ()

        End If
    End Sub

    Private Function BindgridOutstandingRFQ(Optional ByVal pSorted As Boolean = False) As String
        Dim intTotRecord As Integer
        'Dim objDO As New Dashboard
        Dim com_name As String = ""
        Dim docnum As String = ""
        Dim v_display As Integer = 0
        Dim V_RFQ_Status As Integer = 0

        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")

        '//Retrieve Data from Database
        Dim dsOutStandingRFQ As DataSet = New DataSet
        dsOutStandingRFQ = objrfq.get_buyerInfo2(docnum, com_name, v_display, V_RFQ_Status) ' objDO.GetOutstandingRFQ()

        '//for sorting asc or desc
        Dim dvViewOutStandingRFQ As DataView
        dvViewOutStandingRFQ = dsOutStandingRFQ.Tables(0).DefaultView
        dvViewOutStandingRFQ.Sort = ViewState("SortExpression") 'ViewState("SortExpressionOutstandingRFQ")
        'If ViewState("SortAscendingOutstandingRFQ") = "no" And ViewState("SortExpressionOutstandingRFQ") <> "" Then dvViewOutStandingRFQ.Sort += " DESC"
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewOutStandingRFQ.Sort += " DESC"
        If ViewState("actionOutstandingRFQ") = "del" Then
            If dtgOutstandingRFQ.CurrentPageIndex > 0 And dsOutStandingRFQ.Tables(0).Rows.Count Mod dtgOutstandingRFQ.PageSize = 0 Then
                dtgOutstandingRFQ.CurrentPageIndex = dtgOutstandingRFQ.CurrentPageIndex - 1
                ViewState("actionOutstandingRFQ") = ""
            End If
        End If
        intTotRecord = dsOutStandingRFQ.Tables(0).Rows.Count
        'intPageRecordCnt3 = intTotRecord
        Session("PageRecordOutstandingRFQ") = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        'resetDashboardDatagridPageIndex(dtgOutstandingRFQ, dvViewOutStandingRFQ, "OutstandingRFQ")
        If Session("PageRecordOutstandingRFQ") > 0 Then
            resetDatagridPageIndex(dtgOutstandingRFQ, dvViewOutStandingRFQ)
            dtgOutstandingRFQ.DataSource = dvViewOutStandingRFQ
            dtgOutstandingRFQ.DataBind()
        Else
            'If intTotRecord = 0 Then
            '    dtgOutstandingRFQ.ShowHeader = False
            '    dtgOutstandingRFQ.PagerStyle.CssClass = "db_displaynone"
            'End If
            dtgOutstandingRFQ.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            cmdDelete.Enabled = False
        End If

        ' add for above checking
        ViewState("PageCountOutstandingRFQ") = dtgOutstandingRFQ.PageCount
    End Function

    Sub dtgOutstandingRFQ_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgOutstandingRFQ.PageIndexChanged
        dtgOutstandingRFQ.CurrentPageIndex = e.NewPageIndex
        BindgridOutstandingRFQ()
    End Sub

    Private Sub dtgOutStandingRFQ_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingRFQ.ItemCreated
        intPageRecordCnt = Session("PageRecordOutstandingRFQ")
        Grid_ItemCreated(sender, e)
        'intPageRecordCnt = ViewState("intPageRecordCnt")
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Sub SortCommandOutStandingRFQ_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        'Grid_SortCommandDashboard(sender, e, "OutstandingRFQ")
        Grid_SortCommand(sender, e)
        dtgOutstandingRFQ.CurrentPageIndex = 0
        BindgridOutstandingRFQ(True)
    End Sub
    'Private Function Bindgrid(ByVal dg_id As DataGrid, Optional ByVal pSorted As Boolean = False) As String

    '    Dim ds As New DataSet
    '    Dim com_name As String
    '    Dim docnum As String '= txt_Num.Text
    '    Dim v_display As Integer = 0
    '    Dim V_RFQ_Status As Integer = 0
    '    'com_name = Me.txt_com_name.Text

    '    cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection2','delete');")

    '    'cmd_emp_trash.Visible = False
    '    ds = objrfq.get_qoute(docnum, com_name, v_display, V_RFQ_Status)
    '    'Me.lbl_title.Text = "RFQ Sent Folder"
    '    Me.lbl_disc.Text = "All quotations submitted are listed in this folder."

    '    Dim dvViewSample As DataView
    '    dvViewSample = ds.Tables(0).DefaultView
    '    'dvViewSample(0)(0)
    '    'If pSorted Then
    '    dvViewSample.Sort = ViewState("SortExpression")
    '    If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"
    '    'End If

    '    '//these only needed if you can select a grid item and click delete button
    '    '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
    '    '//then user delete one record. //total record = 20 (2 pages), 
    '    '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
    '    If ViewState("action") = "del" Then
    '        If dg_id.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dg_id.PageSize = 0 Then
    '            dg_id.CurrentPageIndex = dg_id.CurrentPageIndex - 1
    '            ViewState("action") = ""
    '        End If
    '    End If

    '    intPageRecordCnt = ds.Tables(0).Rows.Count
    '    ViewState("intPageRecordCnt") = intPageRecordCnt

    '    If intPageRecordCnt > 0 Then
    '        resetDatagridPageIndex(dg_id, dvViewSample)
    '        dg_id.DataSource = dvViewSample
    '        dg_id.DataBind()
    '        If Me.ddl_folder.SelectedItem.Value = "2" Then
    '            cmd_emp_trash.Visible = True
    '        End If
    '    Else
    '        dg_id.DataBind()
    '        Common.NetMsgbox(Me, MsgNoRecord)
    '        'intTotPage = 0
    '        If Me.ddl_folder.SelectedItem.Value = "2" Then
    '            cmd_emp_trash.Visible = False
    '        End If
    '    End If
    '    ViewState("PageCount") = dg_id.PageCount
    'End Function

    'Public Sub dtg_VendorList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
    '    sender.CurrentPageIndex = e.NewPageIndex
    '    Bindgrid(sender)
    'End Sub

    'Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
    '    Grid_SortCommand(sender, e)
    '    sender.CurrentPageIndex = 0
    '    Bindgrid(sender, True)
    'End Sub

    'Private Sub dtg_VendorList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_VendorList.ItemCreated
    '    intPageRecordCnt = viewstate("intPageRecordCnt")
    '    Grid_ItemCreated(dtg_VendorList, e)

    '    '//to add a JavaScript to CheckAll button
    '    If e.Item.ItemType = ListItemType.Header Then
    '        Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
    '        chkAll.Attributes.Add("onclick", "selectAll();")
    '    End If
    'End Sub

    'Private Sub dtg_VendorList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_VendorList.ItemDataBound
    '    If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
    '        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
    '        '//to add JavaScript to Check Box
    '        Dim chk As CheckBox
    '        chk = e.Item.Cells(VenEnum.Chk).FindControl("chkSelection")
    '        chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
    '        e.Item.Cells(VenEnum.CreOn).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("RM_Created_On"))

    '        e.Item.Cells(VenEnum.ExpDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("RM_Expiry_Date"))
    '        Dim lbl_rfqnum As Label
    '        lbl_rfqnum = e.Item.FindControl("lbl_rfqnum")
    '        lbl_rfqnum.Text = "<A href=""RFQDetail.aspx?pageid=" & strPageId & "&page=1&goto=1&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&vcom_id=" & Session("CompanyId") & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"

    '        If objrfq.HasAttachment2(Common.parseNull(Common.parseNull(dv("RM_RFQ_No"))), objrfq.get_comid(dv("RM_RFQ_ID"))) Then
    '            Dim imgAttach As New System.Web.UI.WebControls.Image
    '            imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
    '            imgAttach.ImageUrl = "../Images/clip_icon.gif"
    '            e.Item.Cells(VenEnum.RfqNo).Controls.Add(imgAttach)
    '        End If

    '        'add by Moo
    '        '//CreateQuotation.aspx - add RFQ_Id
    '        Dim lbl_status As Label
    '        lbl_status = e.Item.FindControl("lbl_status")
    '        If Not IsDBNull(dv("RVM_V_RFQ_Status")) Then

    '            If dv("RM_Expiry_Date") >= Now.Today Then
    '                If dv("RVM_V_RFQ_Status") = "0" Then
    '                    lbl_status.Text = "New, <A href=""CreateQuotation.aspx?pageid=" & strPageId & " &bcomid=" & dv("RM_Coy_ID") & "&RFQ_No=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & " "" ><font color=#0000ff>Reply</font></A>"
    '                ElseIf dv("RVM_V_RFQ_Status") = "1" Then
    '                    lbl_status.Text = "Replied, <A href=""CreateQuotation.aspx?pageid=" & strPageId & " &bcomid=" & dv("RM_Coy_ID") & "&RFQ_No=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&edit=1 "" ><font color=#0000ff>Resubmit</font></A>"
    '                End If
    '            Else
    '                If dv("RVM_V_RFQ_Status") = "0" Then
    '                    lbl_status.Text = "Expired"
    '                ElseIf dv("RVM_V_RFQ_Status") = "1" Then
    '                    lbl_status.Text = "Expired, <A  href=""ViewQoute.aspx?pageid=" & strPageId & " &RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & " &vcomid=" & Session("CompanyID") & " "" ><font color=#0000ff>Replied</font></A>"
    '                End If
    '            End If
    '        End If
    '    End If
    'End Sub

    ''Private Sub dtg_quote_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_quote.ItemCreated
    ''    intPageRecordCnt = viewstate("intPageRecordCnt")
    ''    Grid_ItemCreated(dtg_quote, e)

    ''    '//to add a JavaScript to CheckAll button
    ''    If e.Item.ItemType = ListItemType.Header Then
    ''        Dim chkAll2 As CheckBox = e.Item.FindControl("chkAll2")
    ''        chkAll2.Attributes.Add("onclick", "selectAll2();")
    ''    End If
    ''End Sub

    ''Private Sub dtg_quote_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_quote.ItemDataBound

    ''    If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
    ''        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
    ''        '//to add JavaScript to Check Box
    ''        Dim chk2 As CheckBox
    ''        chk2 = e.Item.Cells(QuoEnum.Chk).FindControl("chkSelection2")
    ''        chk2.Attributes.Add("onclick", "checkChild2('" & chk2.ClientID & "')")
    ''        e.Item.Cells(QuoEnum.CreOn).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("RRM_Created_On"))

    ''        e.Item.Cells(QuoEnum.Offer).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("RRM_Offer_Till"))
    ''        Dim lbl_quo As Label
    ''        lbl_quo = e.Item.FindControl("lbl_quo")
    ''        lbl_quo.Text = "<A href=""ViewQoute.aspx?pageid=" & strPageId & " &qou_num=" & Common.parseNull(dv("RRM_Actual_Quot_Num")) & "&RFQ_ID=" & Common.parseNull(dv("RRM_RFQ_ID")) & "&vcomid=" & Session("CompanyID") & "  "" ><font color=#0000ff>" & Common.parseNull(dv("RRM_Actual_Quot_Num")) & "</font></A>"

    ''        Dim lbl_status As Label
    ''        lbl_status = e.Item.FindControl("lbl_status1")
    ''        If Common.parseNull(dv("RM_Status")) = "1" Then
    ''            lbl_status.Text = "Sent"
    ''        ElseIf Common.parseNull(dv("RM_Status")) = "2" Then
    ''            If objrfq.check_po(Common.parseNull(dv("RM_RFQ_ID"))) = True Then
    ''                lbl_status.Text = "Sent, PO created"
    ''            Else
    ''                lbl_status.Text = "Sent"
    ''            End If

    ''        End If

    ''        If objrfq.HasAttachment2(Common.parseNull(Common.parseNull(dv("RM_RFQ_No"))), objrfq.get_comid(dv("RM_RFQ_ID"))) Then
    ''            Dim imgAttach As New System.Web.UI.WebControls.Image
    ''            imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
    ''            imgAttach.ImageUrl = "../Images/clip_icon.gif"
    ''            e.Item.Cells(QuoEnum.RfqNo).Controls.Add(imgAttach)
    ''        End If

    ''        If objrfq.HasAttachmentQuote(Common.parseNull(Common.parseNull(dv("RM_RFQ_No"))), Session("CompanyID")) Then
    ''            Dim imgAttach As New System.Web.UI.WebControls.Image
    ''            imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
    ''            imgAttach.ImageUrl = "../Images/clip_icon.gif"
    ''            e.Item.Cells(QuoEnum.ActQuoNum).Controls.Add(imgAttach)

    ''        End If
    ''        Dim lbl_RFQ_Num As Label
    ''        lbl_RFQ_Num = e.Item.FindControl("lbl_RFQ_Num")
    ''        lbl_RFQ_Num.Text = "<A href=""RFQDetail.aspx?pageid=" & strPageId & "&page=1&goto=2&RFQ_Num=" & Common.parseNull(Common.parseNull(dv("RM_RFQ_No"))) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&vcom_id=" & Session("CompanyId") & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"
    ''    End If
    ''End Sub

    ''Private Sub dtg_trash_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_trash.ItemDataBound

    ''    If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
    ''        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
    ''        '//to add JavaScript to Check Box
    ''        Dim chk3 As CheckBox
    ''        chk3 = e.Item.Cells(TraEnum.Chk).FindControl("chkSelection3")
    ''        chk3.Attributes.Add("onclick", "checkChild3('" & chk3.ClientID & "')")

    ''        Dim Label1 As Label
    ''        Label1 = e.Item.FindControl("Label1")
    ''        Label1.Text = "<A href=""RFQDetail.aspx?pageid=" & strPageId & "&page=1&goto=3&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&vcom_id" & Session("CompanyId") & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"
    ''        e.Item.Cells(TraEnum.RfqID).Text = dv("RM_RFQ_ID")

    ''        e.Item.Cells(TraEnum.CreOn).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("RM_Created_On"))
    ''        e.Item.Cells(TraEnum.ExpDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("RM_Expiry_Date"))

    ''        If objrfq.HasAttachment2(Common.parseNull(dv("RM_RFQ_No")), objrfq.get_comid(dv("RM_RFQ_ID"))) Then
    ''            Dim imgAttach As New System.Web.UI.WebControls.Image
    ''            imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
    ''            imgAttach.ImageUrl = "../Images/clip_icon.gif"
    ''            e.Item.Cells(TraEnum.RfqNo).Controls.Add(imgAttach)

    ''        ElseIf objrfq.HasAttachmentQuote(Common.parseNull(dv("RM_RFQ_No")), Session("CompanyID")) Then
    ''            Dim imgAttach As New System.Web.UI.WebControls.Image
    ''            imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
    ''            imgAttach.ImageUrl = "../Images/clip_icon.gif"
    ''            e.Item.Cells(TraEnum.RfqNo).Controls.Add(imgAttach)
    ''        End If

    ''        Dim lbl_status As Label
    ''        lbl_status = e.Item.FindControl("lbl_status2")
    ''        Dim prefix As String
    ''        Dim doctype As String

    ''        '//Remark By MOO
    ''        'prefix = Common.parseNull(dv("RM_Prefix"))            
    ''        'doctype = objrfq.get_doctype(prefix)
    ''        doctype = Common.parseNull(dv("DocType"))

    ''        If doctype.ToUpper = "RFQ" Then
    ''            If Common.parseNull(dv("RM_Expiry_Date")) <= Now.Today Then
    ''                If Common.parseNull(dv("RVM_V_RFQ_Status")) = "0" Then
    ''                    lbl_status.Text = "Not Replied"
    ''                ElseIf Common.parseNull(dv("RVM_V_RFQ_Status")) = "1" Then
    ''                    lbl_status.Text = "Replied"
    ''                End If
    ''            Else
    ''                If Common.parseNull(dv("RVM_V_RFQ_Status")) = "0" Then
    ''                    lbl_status.Text = "Expired,Not Replied"
    ''                ElseIf Common.parseNull(dv("RVM_V_RFQ_Status")) = "1" Then
    ''                    lbl_status.Text = "Expired, Replied"
    ''                End If
    ''            End If
    ''            e.Item.Cells(TraEnum.CHECKDOC).Text = "RFQ"
    ''        ElseIf doctype.ToUpper = "Quotation".ToUpper Then
    ''            e.Item.Cells(TraEnum.CHECKDOC).Text = "QOUTE"
    ''            lbl_status.Text = "Sent"
    ''        End If
    ''    End If
    ''End Sub

    ''Private Sub dtg_trash_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_trash.ItemCreated
    ''    intPageRecordCnt = viewstate("intPageRecordCnt")
    ''    Grid_ItemCreated(dtg_trash, e)

    ''    '//to add a JavaScript to CheckAll button
    ''    If e.Item.ItemType = ListItemType.Header Then
    ''        Dim chkAll3 As CheckBox = e.Item.FindControl("chkAll3")
    ''        chkAll3.Attributes.Add("onclick", "selectAll3();")
    ''    End If
    ''End Sub

    'Private Sub img_delete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles img_delete.Click
    '    goto_trash()
    'End Sub

    'Function goto_trash()
    '    Dim dgItem As DataGridItem
    '    Dim dg_tem As New DataGrid
    '    Dim objrfq As New RFQ
    '    Dim objval As New RFQ_User
    '    Dim objDB As New EAD.DBCom
    '    Dim strAryQuery(0) As String
    '    Dim strSQL As String
    '    Dim txtPrice As TextBox
    '    Dim lblPrice As Label
    '    Dim chkItem As CheckBox
    '    Dim ckhtemp As CheckBox
    '    Dim DOCTYPE As String
    '    Dim j As Integer
    '    Dim i As Integer = 0

    '    If Me.ddl_folder.SelectedItem.Value = "0" Then
    '        dg_tem = Me.dtg_VendorList
    '        j = 7
    '    ElseIf Me.ddl_folder.SelectedItem.Value = "1" Then
    '        dg_tem = Me.dtg_quote
    '        j = 8
    '    ElseIf Me.ddl_folder.SelectedItem.Value = "2" Then
    '        dg_tem = Me.dtg_trash
    '        j = 7
    '    End If

    '    For Each dgItem In dg_tem.Items
    '        Dim dv As DataRowView = CType(dgItem.DataItem, DataRowView)
    '        objval.RFQ_ID = dgItem.Cells(j).Text
    '        If Me.ddl_folder.SelectedItem.Value = "0" Then
    '            chkItem = dgItem.FindControl("chkSelection")
    '        ElseIf Me.ddl_folder.SelectedItem.Value = "1" Then
    '            chkItem = dgItem.FindControl("chkSelection2")
    '        ElseIf Me.ddl_folder.SelectedItem.Value = "2" Then
    '            chkItem = dgItem.FindControl("chkSelection3")
    '            DOCTYPE = dgItem.Cells(6).Text
    '        End If

    '        If chkItem.Checked Then
    '            strSQL = objrfq.Vendor_add2trash(objval, ddl_folder.SelectedItem.Value, DOCTYPE)
    '            Common.Insert2Ary(strAryQuery, strSQL)
    '        End If

    '        i = i + 1
    '    Next

    '    objDB.BatchExecute(strAryQuery)
    '    Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
    '    If i = dg_tem.Items.Count Then
    '        If dg_tem.CurrentPageIndex <> 0 Then
    '            dg_tem.CurrentPageIndex = dg_tem.CurrentPageIndex - 1
    '        End If
    '    End If
    '    Bindgrid(dg_tem)
    'End Function

    'Private Sub ddl_folder_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddl_folder.SelectedIndexChanged
    '    viewstate("SortAscending") = "no"
    '    If Me.ddl_folder.SelectedItem.Value = "0" Then
    '        viewstate("SortExpression") = "RM_Created_On"
    '        Session("strurl") = "VendorRFQList.aspx?&pageid=" & strPageId & "&goto=1"
    '        SetGridProperty(Me.dtg_VendorList)
    '        'dtg_quote.Visible = False
    '        'Me.dtg_trash.Visible = False
    '        dtg_VendorList.Visible = True
    '        Bindgrid(dtg_VendorList)
    '    ElseIf Me.ddl_folder.SelectedItem.Value = "1" Then
    '        viewstate("SortExpression") = "RRM_Created_On"
    '        Session("strurl") = "VendorRFQList.aspx?&pageid=" & strPageId & "&goto=2"
    '        'SetGridProperty(Me.dtg_quote)
    '        dtg_VendorList.Visible = False
    '        'Me.dtg_trash.Visible = False
    '        'dtg_quote.Visible = True
    '        Bindgrid(dtg_quote)
    '    ElseIf Me.ddl_folder.SelectedItem.Value = "2" Then
    '        viewstate("SortExpression") = "RM_Created_On"
    '        Session("strurl") = "VendorRFQList.aspx?&pageid=" & strPageId & "&goto=3"
    '        'SetGridProperty(Me.dtg_trash)
    '        dtg_VendorList.Visible = False
    '        'Me.dtg_trash.Visible = True
    '        'dtg_quote.Visible = False
    '        Bindgrid(dtg_trash)
    '    End If
    'End Sub

    'Private Sub cmd_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Search.Click
    '    viewstate("SortAscending") = "no"
    '    If Me.ddl_folder.SelectedItem.Value = "0" Then
    '        viewstate("SortExpression") = "RM_Created_On"
    '        SetGridProperty(Me.dtg_VendorList)
    '        'dtg_quote.Visible = False
    '        'Me.dtg_trash.Visible = False
    '        dtg_VendorList.Visible = True
    '        Bindgrid(dtg_VendorList)
    '    ElseIf Me.ddl_folder.SelectedItem.Value = "1" Then
    '        viewstate("SortExpression") = "RRM_Created_On"
    '        'SetGridProperty(Me.dtg_quote)
    '        dtg_VendorList.Visible = False
    '        'Me.dtg_trash.Visible = False
    '        'dtg_quote.Visible = True
    '        'Bindgrid(dtg_quote)
    '    ElseIf Me.ddl_folder.SelectedItem.Value = "2" Then
    '        viewstate("SortExpression") = "RM_Created_On"
    '        'SetGridProperty(Me.dtg_trash)
    '        dtg_VendorList.Visible = False
    '        'Me.dtg_trash.Visible = True
    '        'dtg_quote.Visible = False
    '        'Bindgrid(dtg_trash)
    '    End If
    'End Sub

    'Private Sub cmd_clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_clear.Click
    '    Me.txt_Num.Text = ""
    '    Me.txt_com_name.Text = ""
    'End Sub

    'Private Sub cmd_emp_trash_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_emp_trash.Click

    '    If hidAddItem.Value = "1" Then
    '    ElseIf hidAddItem.Value = "0" Then
    '        Exit Sub
    '    End If

    '    Dim dgItem As DataGridItem
    '    Dim dg_tem As New DataGrid
    '    Dim objrfq As New RFQ
    '    Dim objval As New RFQ_User
    '    Dim objDB As New EAD.DBCom
    '    Dim strAryQuery(0) As String
    '    Dim strSQL As String
    '    Dim txtPrice As TextBox
    '    Dim lblPrice As Label
    '    Dim chkItem As CheckBox
    '    Dim ckhtemp As CheckBox
    '    Dim DOCTYPE As String
    '    Dim j As Integer
    '    Dim i As Integer = 0
    '    dg_tem = Me.dtg_trash

    '    j = 7

    '    For Each dgItem In dg_tem.Items
    '        Dim dv As DataRowView = CType(dgItem.DataItem, DataRowView)
    '        objval.RFQ_ID = dgItem.Cells(j).Text
    '        DOCTYPE = dgItem.Cells(6).Text

    '        strSQL = objrfq.Vendor_add2trash(objval, ddl_folder.SelectedItem.Value, DOCTYPE)
    '        If strSQL <> "" Then
    '            Common.Insert2Ary(strAryQuery, strSQL)
    '        End If

    '        i = i + 1
    '    Next

    '    objDB.BatchExecute(strAryQuery)

    '    If dtg_trash.CurrentPageIndex <> 0 Then
    '        dtg_trash.CurrentPageIndex = dtg_trash.CurrentPageIndex - 1
    '    End If
    '    Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
    '    Bindgrid(dtg_trash)
    'End Sub

    'Private Sub Imagebutton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Imagebutton1.Click
    '    goto_trash()
    'End Sub

    'Private Sub img_pdf_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles img_pdf.Click
    '    printPDF()
    'End Sub

    'Private Sub img_pdf2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles img_pdf2.Click
    '    printPDF()
    'End Sub

    'Private Sub printPDF()
    '    Dim dtPrint As New DataTable
    '    Dim dr As DataRow
    '    Session("dtprint") = Nothing

    '    Dim dgItem As DataGridItem
    '    Dim chk As CheckBox
    '    Dim lnkDocNo As HyperLink
    '    Dim i, cnt As Integer
    '    Dim ds As New DataSet
    '    Dim strType As String

    '    dtPrint.Columns.Add("rfqid", Type.GetType("System.String")) '//doc no
    '    dtPrint.Columns.Add("vcomid", Type.GetType("System.String"))
    '    dtPrint.Columns.Add("type", Type.GetType("System.String")) '//type

    '    Select Case ddl_folder.SelectedItem.Value
    '        Case "0"
    '            For Each dgItem In dtg_VendorList.Items
    '                chk = dgItem.FindControl("chkSelection")
    '                If chk.Checked Then
    '                    dr = dtPrint.NewRow
    '                    dr("rfqid") = dgItem.Cells(VenEnum.RfqID).Text
    '                    dr("vcomid") = Session("CompanyId")
    '                    dr("type") = "RFQ"
    '                    dtPrint.Rows.Add(dr)
    '                End If
    '            Next

    '        Case "1"
    '            For Each dgItem In dtg_quote.Items
    '                chk = dgItem.FindControl("chkSelection2")
    '                If chk.Checked Then
    '                    dr = dtPrint.NewRow
    '                    dr("rfqid") = dgItem.Cells(QuoEnum.RfqID).Text
    '                    dr("vcomid") = Session("CompanyId")
    '                    dr("type") = "QUO"
    '                    dtPrint.Rows.Add(dr)
    '                End If
    '            Next

    '        Case "2"
    '            For Each dgItem In dtg_trash.Items
    '                chk = dgItem.FindControl("chkSelection3")
    '                If chk.Checked Then
    '                    strType = dgItem.Cells(TraEnum.CHECKDOC).Text
    '                    dr = dtPrint.NewRow
    '                    dr("rfqid") = dgItem.Cells(TraEnum.RfqID).Text
    '                    dr("vcomid") = Session("CompanyId")
    '                    Select Case strType
    '                        Case "RFQ"
    '                            dr("type") = "RFQ"
    '                        Case "QOUTE"
    '                            dr("type") = "QUO"
    '                    End Select
    '                    dtPrint.Rows.Add(dr)
    '                End If
    '            Next
    '    End Select

    '    If dtPrint.Rows.Count > 0 Then
    '        Session("dtprint") = dtPrint
    '        Dim vbs As String
    '        vbs = vbs & "<script language=""javascript"">"
    '        vbs = vbs & vbLf & "PDFWindow('" & strPageId & "');"
    '        vbs = vbs & "</script>"
    '        If (Not Me.IsStartupScriptRegistered("pdfwindow")) Then
    '            Me.RegisterStartupScript("pdfwindow", vbs)
    '        End If
    '    End If
    'End Sub

    Protected Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        goto_trash()
    End Sub

    Function goto_trash()
        Dim dgItem As DataGridItem
        Dim dg_tem As New DataGrid
        Dim objrfq As New RFQ
        Dim objval As New RFQ_User
        Dim objDB As New EAD.DBCom
        Dim strAryQuery(0) As String
        Dim strSQL As String
        Dim txtPrice As TextBox
        Dim lblPrice As Label
        Dim chkItem As CheckBox
        Dim ckhtemp As CheckBox
        Dim DOCTYPE As String
        Dim j As Integer
        Dim i As Integer = 0

        'If Me.ddl_folder.SelectedItem.Value = "0" Then
        dg_tem = Me.dtgOutstandingRFQ
        j = 2
        'ElseIf Me.ddl_folder.SelectedItem.Value = "1" Then
        'dg_tem = Me.dtg_quote
        'j = 8
        'ElseIf Me.ddl_folder.SelectedItem.Value = "2" Then
        'dg_tem = Me.dtg_trash
        'j = 7
        'End If

        For Each dgItem In dg_tem.Items
            Dim dv As DataRowView = CType(dgItem.DataItem, DataRowView)
            objval.RFQ_ID = dgItem.Cells(j).Text
            'If Me.ddl_folder.SelectedItem.Value = "0" Then
            chkItem = dgItem.FindControl("chkSelection")
            'ElseIf Me.ddl_folder.SelectedItem.Value = "1" Then
            'chkItem = dgItem.FindControl("chkSelection2")
            'ElseIf Me.ddl_folder.SelectedItem.Value = "2" Then
            'chkItem = dgItem.FindControl("chkSelection3")
            'DOCTYPE = dgItem.Cells(6).Text
            'End If

            If chkItem.Checked Then
                strSQL = objrfq.Vendor_add2trash(objval, "0", DOCTYPE)
                Common.Insert2Ary(strAryQuery, strSQL)
            End If

            i = i + 1
        Next

        objDB.BatchExecute(strAryQuery)
        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        If i = dg_tem.Items.Count Then
            If dg_tem.CurrentPageIndex <> 0 Then
                dg_tem.CurrentPageIndex = dg_tem.CurrentPageIndex - 1
            End If
        End If
        BindgridOutstandingRFQ()
    End Function

    'Private Function Bindgrid(ByVal dg_id As DataGrid, Optional ByVal pSorted As Boolean = False) As String

    '    Dim ds As New DataSet
    '    Dim com_name As String = ""
    '    Dim docnum As String = "" 'txt_Num.Text
    '    Dim v_display As Integer = 0
    '    Dim V_RFQ_Status As Integer = 0
    '    'com_name = Me.txt_com_name.Text

    '    'If Me.ddl_folder.SelectedItem.Value = "0" Then
    '    'Imagebutton1.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
    '    cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")

    '    'Me.lbl_title.Text = "RFQ Inbox Folder"
    '    'cmd_emp_trash.Visible = False
    '    ds = objrfq.get_buyerInfo(docnum, com_name, v_display, V_RFQ_Status)
    '    'Me.lbl_disc.Text = "This list provides a summary of the quotation activity between you and your Buyer. You can respond to a new RFQ or go to your sent folder to view previously quoted items. Click on a RFQ Number to view it. To reply, click on the reply link in the status column."
    '    'ElseIf Me.ddl_folder.SelectedItem.Value = "1" Then
    '    'Imagebutton1.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection2','delete');")
    '    'img_delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection2','delete');")

    '    'cmd_emp_trash.Visible = False
    '    'ds = objrfq.get_qoute(docnum, com_name, v_display, V_RFQ_Status)
    '    'Me.lbl_title.Text = "RFQ Sent Folder"
    '    'Me.lbl_disc.Text = "All quotations submitted are listed in this folder."
    '    'ElseIf Me.ddl_folder.SelectedItem.Value = "2" Then
    '    'Imagebutton1.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection3','delete');")
    '    'img_delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection3','delete');")
    '    'cmd_emp_trash.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection3','delete');")

    '    ''cmd_emp_trash.Visible = True
    '    'Me.lbl_title.Text = "RFQ Trash Folder"
    '    'Me.lbl_disc.Text = "To remove items permanently from this folder, select the item and click on the trash icon."
    '    'ds = objrfq.get_trash(docnum, com_name, v_display, V_RFQ_Status)
    '    'End If

    '    Dim dvViewSample As DataView
    '    dvViewSample = ds.Tables(0).DefaultView
    '    'dvViewSample(0)(0)
    '    'If pSorted Then
    '    dvViewSample.Sort = ViewState("SortExpression")
    '    If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"
    '    'End If

    '    '//these only needed if you can select a grid item and click delete button
    '    '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
    '    '//then user delete one record. //total record = 20 (2 pages), 
    '    '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
    '    If ViewState("action") = "del" Then
    '        If dg_id.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dg_id.PageSize = 0 Then
    '            dg_id.CurrentPageIndex = dg_id.CurrentPageIndex - 1
    '            ViewState("action") = ""
    '        End If
    '    End If

    '    intPageRecordCnt = ds.Tables(0).Rows.Count
    '    ViewState("intPageRecordCnt") = intPageRecordCnt

    '    If intPageRecordCnt > 0 Then
    '        resetDatagridPageIndex(dg_id, dvViewSample)
    '        dg_id.DataSource = dvViewSample
    '        dg_id.DataBind()
    '        'If Me.ddl_folder.SelectedItem.Value = "2" Then
    '        '    cmd_emp_trash.Visible = True
    '        'End If
    '    Else
    '        dg_id.DataBind()
    '        Common.NetMsgbox(Me, MsgNoRecord)
    '        'intTotPage = 0
    '        'If Me.ddl_folder.SelectedItem.Value = "2" Then
    '        '    cmd_emp_trash.Visible = False
    '        'End If
    '    End If
    '    ViewState("PageCount") = dg_id.PageCount
    'End Function

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn_selected"" href=""#""><span>Outstanding RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn"" href=""RFQSearch.aspx?pageid=" & strPageId & """><span>Quotation Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "</ul><div></div></div>"
        Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""#""><span>Outstanding RFQ</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "VendorRFQListExp.aspx", "pageid=" & strPageId) & """><span>Expired / Rejected RFQ</span></a></li>" & _
                             "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "RFQSearch.aspx", "pageid=" & strPageId) & """><span>Quotation Listing</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "</ul><div></div></div>"
    End Sub

    Private Sub dtgOutstandingRFQ_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgOutstandingRFQ.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim chk As CheckBox

            Dim lnkRFQNo
            lnkRFQNo = e.Item.FindControl("lnkRFQNum")
            lnkRFQNo.Text = dv("RFQ Number")

            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            'lnkRFQNo.NavigateUrl = "../RFQ/RFQDetail.aspx?pageid=21&page=1&goto=1&RFQ_Num=" & dv("RFQ Number") & "&RFQ_ID=" & dv("RM_RFQ_ID") & "&vcom_id=" & Session("CompanyId")
            strPageId = 98

            'lnkRFQNo.NavigateUrl = "../RFQ/CreateQuotationNew.aspx?pageid=" & strPageId & "&RFQ_No=" & dv("RFQ Number") & "&RFQ_ID=" & dv("RM_RFQ_ID") & "&bcomid=" & dv("RM_Coy_ID")
            If e.Item.Cells(5).Text >= Date.Today And dv("RM_B_DISPLAY_STATUS") = "0" Then
                lnkRFQNo.NavigateUrl = dDispatcher.direct("RFQ", "CreateQuotationNew.aspx", "Frm=VendorRFQList&pageid=" & strPageId & "&RFQ_No=" & Server.UrlEncode(dv("RFQ Number")) & "&RFQ_ID=" & dv("RM_RFQ_ID") & "&bcomid=" & dv("RM_Coy_ID"))
            Else
                If dv("RM_B_DISPLAY_STATUS") = "0" Then
                    lnkRFQNo.NavigateUrl = dDispatcher.direct("RFQ", "RFQDetail.aspx", "Frm=VendorRFQList&pageid=" & strPageId & "&page=1&goto=2&RFQ_Num=" & Server.UrlEncode(dv("RFQ Number")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&vcom_id=" & Session("CompanyId"))
                Else
                    Dim imgAttach As New System.Web.UI.WebControls.Image
                    imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                    imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "Deleted RFQ Icon.gif")
                    e.Item.Cells(1).Controls.Add(imgAttach)
                End If

            End If


            'lbl_status.Text = "New, <A href=""CreateQuotation.aspx?pageid=" & strPageId & " &bcomid=" & dv("RM_Coy_ID") & "&RFQ_No=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & " "" ><font color=#0000ff>Reply</font></A>"


            e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Creation Date"))
            e.Item.Cells(5).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("Expiry Date"))

            If objrfq.HasAttachmentVen(dv("RFQ Number"), dv("RM_Coy_ID"), "E") Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(1).Controls.Add(imgAttach)
            End If
        End If
    End Sub
End Class
