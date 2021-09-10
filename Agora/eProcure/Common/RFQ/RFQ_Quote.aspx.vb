Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing
Public Class RFQ_Quote
    Inherits AgoraLegacy.AppBaseClass
    Dim objDO As New Dashboard
    Dim objRFQ As New RFQ
    Dim dDispatcher As New AgoraLegacy.dispatcher
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents lblCurrentIndex As System.Web.UI.WebControls.Label
    Protected WithEvents cmd_Search As System.Web.UI.WebControls.Button
    Protected WithEvents dtg_VendorList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ddl_folder As System.Web.UI.WebControls.DropDownList
    Dim objval As New RFQ_User
    Protected WithEvents txt_DocNum As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_VenName As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmd_NewRFQ As System.Web.UI.WebControls.Button
    'Protected WithEvents dtg_Draft As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents dtg_Qoute As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents dtg_Trash As System.Web.UI.WebControls.DataGrid
    Dim count As Integer
    Dim total As Integer
    'Protected WithEvents lbl_title As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_display As System.Web.UI.WebControls.Label
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDuplicate As System.Web.UI.WebControls.Button

    Protected WithEvents cmd_clear As System.Web.UI.WebControls.Button
    Protected WithEvents hidAddItem As System.Web.UI.HtmlControls.HtmlInputHidden
    Dim chkRFQ_ID As String
    Enum RFQSTATUS

        NOTINTRASH = 0
        TRASH = 1
        REMOVE_FROM_TRASH = 2

    End Enum
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region


    Public Enum VenEnum
        Chk = 0
        RFQ_No1 = 1
        RFQ_Name = 2
        Cre_On = 3
        Exp_Date = 4
        VenList = 5
        Status = 6
        rfqno = 7
        RFQ_No2 = 8
        RFQ_ID = 9
    End Enum
    Public Enum DraEnum
        Chk = 0
        RFQ_No = 1
        RFQ_Name = 2
        Cre_On = 3
        Exp_Date = 4
        VenList = 5
        Img = 6
        rfqno = 7
        RFQ_No2 = 8
        RFQ_ID = 9
    End Enum
    Public Enum QuoEnum
        'Chk = 0
        'RFQ_No = 1
        'RFQ_Name = 2
        'Quo_No = 3
        'Offer = 4
        'COY_NAME = 5
        'Currency = 6
        'TotValue = 7
        'rfqno = 8
        'rfqid = 9
        RFQ_No = 0
        RFQ_Name = 1
        Quo_No = 2
        Offer = 3
        COY_NAME = 4
        Currency = 5
        TotValue = 6
        rfqno = 7
        rfqid = 8
    End Enum
    Public Enum TraEnum
        Chk = 0
        RFQ_No = 1
        RFQ_Name = 2
        Cre_On = 3
        Exp_Date = 4
        VenList = 5
        status = 6
        rfqno = 7
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)

        If Not Page.IsPostBack Then
            GenerateTab()

            Dim msg As String
            Dim rfq_no As String
            msg = Request(Trim("edit"))
            rfq_no = Request(Trim("RFQ_Num"))
            DATA_GRID()

        End If

    End Sub

    Private Function Bindgrid(ByVal dg_id As DataGrid, Optional ByVal pSorted As Boolean = False) As String
        Dim ds As New DataSet
        Dim DocNum As String
        Dim VenName As String = txt_VenName.Text
        Dim v_display As Integer = 0
        Dim V_RFQ_Status As Integer = 0
        DocNum = Me.txt_DocNum.Text

        If VenName <> "" Then
            ds = objRFQ.GetQuoteListWithVendor(DocNum, VenName)
        Else
            ds = objRFQ.GetQuoteListAll(DocNum)
        End If

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)
        'If pSorted Then
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"
        'End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If ViewState("action") = "del" Then
            If dg_id.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dg_id.PageSize = 0 Then
                dg_id.CurrentPageIndex = dg_id.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dg_id, dvViewSample)

            dg_id.DataSource = dvViewSample
            dg_id.DataBind()

        Else
            dg_id.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)

        End If
        ViewState("PageCount") = dg_id.PageCount
        '//datagrid.pageCount only got value after databind

    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        sender.CurrentPageIndex = 0
        ' viewstate("rfq_id") = ""
        ViewState("total") = 0
        Bindgrid(sender, True)

    End Sub

    Private Sub dtg_Qoute_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_Qoute.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtg_Qoute, e)

        ''//to add a JavaScript to CheckAll button
        'If e.Item.ItemType = ListItemType.Header Then
        '    Dim chkAll3 As CheckBox = e.Item.FindControl("chkAll3")
        '    chkAll3.Attributes.Add("onclick", "selectAll3();")
        'End If

        'e.Item.Attributes.Add("style", "CURSOR:hand")
        '  e.Item.Attributes.Add("Onclick", "selectRow(" & e.Item.ItemIndex & ");")

    End Sub

    Private Sub dtg_Qoute_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_Qoute.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box   

            count = ViewState("total")

            'Dim chk3 As CheckBox
            'chk3 = e.Item.Cells(QuoEnum.Chk).FindControl("chkSelection3")
            'chk3.Visible = False
            'chk3.Enabled = False

            Dim lbl_name As Label
            Dim lbl_rfqnum3 As Label
            lbl_name = e.Item.FindControl("lbl_name")
            lbl_rfqnum3 = e.Item.FindControl("lbl_rfqnum3")

            If CStr(Common.parseNull(dv("RM_RFQ_ID"))) <> chkRFQ_ID Then
                lbl_name.Text = Common.parseNull(dv("RM_RFQ_Name"))
                lbl_rfqnum3.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQDetail.aspx", "Frm=RFQ_Quote&pageid=" & strPageId & "&page=1&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID"))) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"
                total = count Mod 2
                count = count + 1
                If objRFQ.HasAttachment(Common.parseNull(dv("RM_RFQ_No")), "E") Then
                    Dim imgAttach As New System.Web.UI.WebControls.Image
                    imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                    imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")
                    e.Item.Cells(QuoEnum.RFQ_No).Controls.Add(imgAttach)
                End If
            End If
            e.Item.Cells(QuoEnum.COY_NAME).Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "Frm=RFQ_Quote&pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&v_com_id=" & dv("RVM_V_Company_ID")) & """ ><font color=#0000ff>" & dv("CM_COY_NAME") & "</font></A><br>"
            ViewState("total") = count

            If total = 0 Then
                e.Item.BackColor = Color.LightYellow()
            Else
                e.Item.BackColor = Color.FromName("#f6f9fe")
            End If
            chkRFQ_ID = CStr(Common.parseNull(dv("RM_RFQ_ID")))

            ' ai chu add
            Dim hidRfqId3 As HtmlInputHidden
            hidRfqId3 = e.Item.FindControl("hidRfqId3")
            hidRfqId3.Value = ""

            Dim lbl_QuoValidity As Label
            lbl_QuoValidity = e.Item.FindControl("lbl_QuoValidity")
            Dim lbl_qouteNum As Label
            lbl_qouteNum = e.Item.FindControl("lbl_qouteNum")

            If IsDBNull(Common.parseNull(dv("RRM_Actual_Quot_Num"))) Or Common.parseNull(dv("RRM_Actual_Quot_Num")) = "" Or Common.parseNull(dv("RRM_Actual_Quot_Num")) = " " Then
                lbl_qouteNum.Text = "N/A"
            Else
                ' ai chu add on 12/10/2005
                ' for close RFQ, buyer is not suppose to view the quotation that not yet expired
                ' RM_Expiry_Date, RM_RFQ_OPTION
                If Not IsDBNull(dv("RM_Expiry_Date")) Then
                    If dv("RM_RFQ_OPTION") = "1" And dv("RM_Expiry_Date") >= Today Then
                        lbl_qouteNum.Text = Common.parseNull(dv("RRM_Actual_Quot_Num"))
                    Else
                        lbl_qouteNum.Text = "<A href=""" & dDispatcher.direct("RFQ", "ViewQoute.aspx", "Frm=RFQ_Quote&pageid=" & strPageId & "&qou_num=" & Common.parseNull(dv("RRM_Actual_Quot_Num")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Num=" & Request(Trim("RFQ_Num")) & "&vcomid=" & Common.parseNull(dv("RRM_V_Company_ID"))) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RRM_Actual_Quot_Num")) & "</font></A>"
                    End If
                End If

                hidRfqId3.Value = Common.parseNull(dv("RRM_V_Company_ID"))

                ' ai chu add on 06/12/2005
                ' SR U30025 - to add in idcication for quotation received which is converted to PR
                ' Remarks: if more than 1 PR raised from the Quotation, system will redirect user to a listing
                '          page to list all PR number raised from the quotation number, else system will redirect
                '          user to PR Details screen 

                Dim objPR As New PR
                Dim dsPR As DataSet
                dsPR = objPR.getPRFromQuot(dv("RRM_Actual_Quot_Num"), dv("RRM_V_Company_ID"), Session("CompanyId"))

                Dim lnkPR As New HyperLink
                'Dim strRFQNo, strRFQName As String
                'objPR.getRFQName(dv("PRM_RFQ_INDEX"), strRFQNo, strRFQName)
                lnkPR.ImageUrl = dDispatcher.direct("Plugins/images", "i_PR2.gif")
                If Not dsPR Is Nothing Then
                    If dsPR.Tables(0).Rows.Count <> 0 Then
                        If dsPR.Tables(0).Rows.Count = 1 Then
                            lnkPR.NavigateUrl = dDispatcher.direct("PO", "PRDetail.aspx", "caller=OTHER&PageId=" & strPageId & "&index=" & dsPR.Tables(0).Rows(0)("PRM_PR_INDEX") & "&PRNo=" & dsPR.Tables(0).Rows(0)("PRM_PR_NO"))
                        ElseIf dsPR.Tables(0).Rows.Count > 1 Then
                            lnkPR.NavigateUrl = dDispatcher.direct("PR", "PRList.aspx", "type=PR&PageId=" & strPageId & "&CoyId=" & dv("RRM_V_Company_ID") & "&DocNo=" & Server.UrlEncode(dv("RRM_Actual_Quot_Num")))
                        End If
                        lnkPR.ToolTip = "Click here to view PR"
                        e.Item.Cells(QuoEnum.Quo_No).Controls.Add(lnkPR)
                    End If
                    Session("strurl") = strCallFrom
                End If
            End If

            '  chk3.Attributes.Add("onclick", "checkChild3('" & chk3.ClientID & "')")
            If IsDBNull(dv("RRM_Offer_Till")) Then
                lbl_QuoValidity.Text = "N/A"
            Else
                lbl_QuoValidity.Text = Common.parseNull(dv("RRM_Offer_Till"))
            End If
            Dim lbl_total As Label
            lbl_total = e.Item.FindControl("lbl_total")

            If objRFQ.HasAttachmentQuote(Common.parseNull(dv("RM_RFQ_No")), Common.parseNull(dv("RRM_V_Company_ID"))) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")
                e.Item.Cells(QuoEnum.Quo_No).Controls.Add(imgAttach)

            End If

            If IsDBNull(dv("RRM_TotalValue")) Then
                lbl_total.Text = "N/A"
            Else
                ' lbl_total.Text = Common.parseNull(dv("RRM_Currency_Code")) & " " & Format(Common.parseNull(dv("RRM_TotalValue")), "#,##0.00")
                '*****28/01/05 Esther *******
                If Not IsDBNull(dv("RM_Expiry_Date")) Then
                    If dv("RM_RFQ_OPTION") = "1" And dv("RM_Expiry_Date") >= Today Then
                        lbl_total.Text = "-"
                    Else
                        lbl_total.Text = Format(Common.parseNull(dv("RRM_TotalValue")), "#,##0.00")
                    End If
                End If
            End If

            '  e.Item.Cells(EnumInvSearch.icAmount).Text = Format(Common.parseNull(dv("IM_INVOICE_TOTAL")), "#,##0.00")
        End If
    End Sub

    'Sub ShowStats()
    '    lblCurrentIndex.Text = record & " Record(s) Found"
    '    ' lblPageCount.Text = "Total Page is " & MyDataGrid.PageCount
    'End Sub

    'Private Sub dtg_Trash_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) 'Handles dtg_Trash.ItemCreated
    '    intPageRecordCnt = ViewState("intPageRecordCnt")
    '    Grid_ItemCreated(sender, e)

    '    '//to add a JavaScript to CheckAll button
    '    If e.Item.ItemType = ListItemType.Header Then
    '        Dim chkAll4 As CheckBox = e.Item.FindControl("chkAll4")
    '        chkAll4.Attributes.Add("onclick", "selectAll4();")
    '    End If

    'End Sub

    'Private Sub dtg_Trash_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) 'Handles dtg_Trash.ItemDataBound
    '    If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
    '        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
    '        '//to add JavaScript to Check Box
    '        Dim chk4 As CheckBox
    '        chk4 = e.Item.Cells(TraEnum.Chk).FindControl("chkSelection4")
    '        chk4.Attributes.Add("onclick", "checkChild4('" & chk4.ClientID & "')")
    '        e.Item.Cells(TraEnum.Cre_On).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Common.parseNull(dv("RM_Created_On")))
    '        e.Item.Cells(TraEnum.Exp_Date).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Common.parseNull(dv("RM_Expiry_Date")))
    '        Dim lbl_rfqnum4 As Label
    '        lbl_rfqnum4 = e.Item.FindControl("lbl_rfqnum4")
    '        If Not IsDBNull(Common.parseNull(dv("RM_RFQ_No"))) Then
    '            lbl_rfqnum4.Text = "<A href=""RFQDetail.aspx?pageid=" & strPageId & "&page=1&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"
    '        Else
    '            lbl_rfqnum4.Text = "-"
    '        End If
    '        e.Item.Cells(TraEnum.rfqno).Text = Common.parseNull(dv("RM_RFQ_No"))
    '        Dim array2(100) As String
    '        Dim COUNT As Integer
    '        Dim COUNT2 As Integer
    '        Dim i As Integer
    '        Dim ARRAY(100) As String
    '        Dim STR As String
    '        Dim com_name(100) As String
    '        Dim com_id(100) As String
    '        Dim com_str As String

    '        ' ai chu add
    '        Dim hidRfqId4 As HtmlInputHidden
    '        hidRfqId4 = e.Item.FindControl("hidRfqId4")
    '        hidRfqId4.Value = Common.parseNull(dv("RM_RFQ_ID"))

    '        objrfq.get_vendorlist(Common.parseNull(dv("RM_RFQ_ID")), com_name, com_id, COUNT2, com_str)
    '        objrfq.get_vendorName(Common.parseNull(dv("RM_RFQ_ID")), ARRAY, array2, COUNT, com_str)
    '        If com_name(0) <> "" Then
    '            For i = 0 To COUNT2 - 1
    '                STR = STR & "<A href=""Comlist.aspx?pageid=" & strPageId & "&list_name=" & com_name(i) & "&rfq_name=" & Server.UrlEncode(e.Item.Cells(TraEnum.RFQ_Name).Text) & "&list_no=" & com_id(i) & """ ><font color=#0000ff>" & com_name(i) & "</font></A><br>"
    '            Next

    '        End If
    '        If ARRAY(0) <> "" Then

    '            For i = 0 To COUNT - 1
    '                STR = STR & "<A href=""RFQ_VendorDetail.aspx?pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&v_com_id=" & array2(i) & """ ><font color=#0000ff>" & ARRAY(i) & "</font></A><br>"
    '            Next

    '        End If

    '        If objrfq.HasAttachmentQuote(Common.parseNull(dv("RM_RFQ_No")), array2(i)) Then
    '            Dim imgAttach As New System.Web.UI.WebControls.Image
    '            imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
    '            imgAttach.ImageUrl = "../Images/clip_icon.gif"
    '            e.Item.Cells(TraEnum.RFQ_No).Controls.Add(imgAttach)

    '        End If

    '        If objrfq.HasAttachment(Common.parseNull(dv("RM_RFQ_No"))) Then
    '            Dim imgAttach As New System.Web.UI.WebControls.Image
    '            imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
    '            imgAttach.ImageUrl = "../Images/clip_icon.gif"

    '            e.Item.Cells(TraEnum.RFQ_No).Controls.Add(imgAttach)
    '        End If

    '        If STR = "" Then
    '            STR = "No vendors are found"
    '        End If
    '        e.Item.Cells(TraEnum.VenList).Text = STR

    '        If Not IsDBNull(dv("RM_Status")) Then
    '            If Common.parseNull(dv("RM_Status")) = "0" Then
    '                e.Item.Cells(TraEnum.status).Text = "No Response, Expired"

    '            ElseIf Common.parseNull(dv("RM_Status")) = "1" Then
    '                e.Item.Cells(TraEnum.status).Text = "<A href=""RFQComSummary.aspx?pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Server.UrlEncode(Common.parseNull(dv("RM_RFQ_Name"))) & """ ><font color=#0000ff>View Response</font></A>, Expired"
    '            ElseIf Common.parseNull(dv("RM_Status")) = "2" Then
    '                e.Item.Cells(TraEnum.status).Text = "PR Raised,Expired"
    '            ElseIf Common.parseNull(dv("RM_Status")) = "3" Then
    '                If Not IsDBNull(dv("RM_Expiry_Date")) Then
    '                    If Common.parseNull(dv("RM_Expiry_Date")) > Now.Date Then
    '                        e.Item.Cells(TraEnum.status).Text = "Draft"
    '                    Else
    '                        e.Item.Cells(TraEnum.status).Text = "Draft,Expired"
    '                    End If
    '                Else
    '                    e.Item.Cells(TraEnum.status).Text = "N/A"
    '                End If
    '            End If

    '        Else

    '            e.Item.Cells(TraEnum.status).Text = "N/A"
    '        End If

    '    End If

    'End Sub

    Sub DATA_GRID()
      
        ViewState("SortExpression") = "RRM_Offer_Till"
        ViewState("SortAscending") = "no"
        DisplayExtraFunction()
    End Sub

    Sub DisplayExtraFunction()
        Session("strurl") = dDispatcher.direct("RFQ", "RFQ_List.aspx", "pageid=" & strPageId)
        SetGridProperty(Me.dtg_Qoute)
        Bindgrid(Me.dtg_Qoute)
    End Sub

    Private Sub cmd_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Search.Click
        dtg_Qoute.CurrentPageIndex = 0
        DATA_GRID()
    End Sub

    'Private Sub ddl_folder_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddl_folder.SelectedIndexChanged
    '    'viewstate("SortAscending") = "no"
    '    'viewstate("SortExpression") = "RM_Created_On"
    '    ' viewstate("rfq_id") = ""
    '    ViewState("total") = 0
    '    Me.txt_DocNum.Text = ""
    '    Me.txt_VenName.Text = ""
    '    DATA_GRID()
    'End Sub

    Private Sub cmd_NewRFQ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_NewRFQ.Click
        Session("back") = "0"
        Session("edit") = ""
        Session("RFQ_Num") = ""
        Session("saverfq2") = "0"
        Session("save") = ""
        'chk_option = 1 check rfq option from company_mstr 
        Response.Redirect(dDispatcher.direct("RFQ", "Create_RFQ.aspx", "pageid=" & strPageId & "&chk_option=1&edit=0"))
    End Sub

    'Private Sub cmd_emp_trash_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_emp_trash.Click
    '    If hidAddItem.Value = "1" Then
    '        'go to next step
    '    ElseIf hidAddItem.Value = "0" Then
    '        Exit Sub
    '    End If
    '    Dim objrfq As New RFQ
    '    Dim B_NEW_STATUS As Integer = Me.RFQSTATUS.REMOVE_FROM_TRASH
    '    Dim B_CURR_STATUS As Integer = Me.RFQSTATUS.TRASH
    '    Dim dgitem As DataGridItem
    '    Dim chk As CheckBox
    '    Dim DS As New DataSet
    '    Dim dtr As DataRow
    '    Dim delete As New DataTable
    '    delete.Columns.Add("B_NEW_STATUS", Type.GetType("System.String"))
    '    delete.Columns.Add("B_CURR_STATUS", Type.GetType("System.String"))
    '    delete.Columns.Add("rfq_no", Type.GetType("System.String"))

    '    For Each dgitem In dtg_Trash.Items

    '        dtr = delete.NewRow()
    '        dtr("B_NEW_STATUS") = B_NEW_STATUS
    '        dtr("B_CURR_STATUS") = B_CURR_STATUS
    '        dtr("rfq_no") = dgitem.Cells(TraEnum.rfqno).Text
    '        delete.Rows.Add(dtr)

    '    Next
    '    DS.Tables.Add(delete)
    '    objrfq.delete_trash(DS)
    '    If dtg_Trash.CurrentPageIndex <> 0 Then
    '        dtg_Trash.CurrentPageIndex = dtg_Trash.CurrentPageIndex - 1
    '    End If
    '    Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
    '    Bindgrid(Me.dtg_Trash)
    'End Sub

    Private Sub img_delete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) 'Handles img_delete.Click
        goto_trash()
    End Sub

    Function goto_trash()
        Dim objrfq As New RFQ
        Dim B_NEW_STATUS As Integer = Me.RFQSTATUS.TRASH
        Dim B_CURR_STATUS As Integer = Me.RFQSTATUS.NOTINTRASH
        Dim dg_id As DataGrid
        Dim i As Integer
        Dim strStatus As Label

        dg_id = dtg_VendorList

        Dim dgitem As DataGridItem
        Dim chk As CheckBox
        Dim DS As New DataSet
        Dim dtr As DataRow
        Dim delete As New DataTable

        delete.Columns.Add("B_NEW_STATUS", Type.GetType("System.String"))
        delete.Columns.Add("B_CURR_STATUS", Type.GetType("System.String"))
        delete.Columns.Add("rfq_no", Type.GetType("System.String"))

        For Each dgitem In dg_id.Items
            chk = dgitem.FindControl("chkSelection")
            strStatus = dgitem.FindControl("lbl_status")
            If chk.Checked Then
                ' If Me.ddl_folder.SelectedItem.Value = "0" Then
                If strStatus.Text = "Draft" Then
                    dtr = delete.NewRow()
                    dtr("B_NEW_STATUS") = B_NEW_STATUS
                    dtr("B_CURR_STATUS") = B_CURR_STATUS
                    dtr("rfq_no") = dgitem.Cells(7).Text
                    delete.Rows.Add(dtr)
                Else
                    Common.NetMsgbox(Me, "RFQ that has not expired cannot be removed.", MsgBoxStyle.Information)
                    Exit Function
                End If
                'If Format(CDate(dgitem.Cells(8).Text), "yyyyMMdd") < Format(Now.Today, "yyyyMMdd") Then
                '    'Format(CDate(Me.txt_exp.Text), "yyyyMMdd") < Format(Now.Today, "yyyyMMdd")
                '    dtr = delete.NewRow()
                '    dtr("B_NEW_STATUS") = B_NEW_STATUS
                '    dtr("B_CURR_STATUS") = B_CURR_STATUS
                '    dtr("rfq_no") = dgitem.Cells(7).Text
                '    delete.Rows.Add(dtr)
                '    i = i + 1
                'Else
                '    Common.NetMsgbox(Me, "RFQ that has not expired cannot be removed to the trash folder.", MsgBoxStyle.Information)
                '    Exit Function
                'End If

                'Else
                '    dtr = delete.NewRow()
                '    dtr("B_NEW_STATUS") = B_NEW_STATUS
                '    dtr("B_CURR_STATUS") = B_CURR_STATUS
                '    dtr("rfq_no") = dgitem.Cells(7).Text
                '    delete.Rows.Add(dtr)
                'End If

            End If
        Next
        DS.Tables.Add(delete)
        objrfq.delete_trash(DS)
        If i = dg_id.Items.Count Then
            If dg_id.CurrentPageIndex <> 0 Then
                dg_id.CurrentPageIndex = dg_id.CurrentPageIndex - 1
            End If
        End If
        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        Bindgrid(dg_id)
    End Function

    Private Sub cmd_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_clear.Click
        Me.txt_DocNum.Text = ""
        Me.txt_VenName.Text = ""
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Session("w_CreateRFQ_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""Create_RFQ.aspx?pageid=" & strPageId & """><span>Raise RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""RFQ_Outstg_List.aspx?pageid=" & strPageId & """><span>Outstanding RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""RFQ_List.aspx?pageid=" & strPageId & """><span>RFQ Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '              "<li><a class=""t_entity_btn_selected"" href=""RFQ_Quote.aspx?pageid=" & strPageId & """><span>Quotation</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '            "</ul><div></div></div>"
        Session("w_CreateRFQ_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "Create_RFQ.aspx", "pageid=" & strPageId) & """><span>Raise RFQ</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "RFQ_Outstg_List.aspx", "pageid=" & strPageId) & """><span>Outstanding RFQ</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "RFQ_List.aspx", "pageid=" & strPageId) & """><span>RFQ Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("RFQ", "RFQ_Quote.aspx", "pageid=" & strPageId) & """><span>Quotation</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                   "</ul><div></div></div>"
    End Sub

    Public Sub dtg_VendorList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        ' viewstate("rfq_id") = ""
        ViewState("total") = 0
        Bindgrid(sender)
    End Sub
End Class