'//Comment Added by Moo(09/09/2005)
'//This page has a dropdown list called ddl_folder
'//0 - sent, 1 - Draft, 2 - Inbox, 3 - Trash
'//To redirect back to this page from other page, it use querystring call 'Edit'
'//The following is the mapping of 'Edit' value and 'ddl_folder' value
'//Edit     ddl_folder
'//1        1
'//2        -/0  ???? not sure
'//3        2
'//4        3
'//empty    0     

'//Comment Added by Ai Chu(24/21/2006)
'//----------------------------------------------------------
'//|    Folder  |   RM_Status                   |   RM_B_Display_Status     |
'//----------------------------------------------------------
'//|     Sent   |   0:Sub,1:Replied,2:PO        |   0                       |
'//----------------------------------------------------------
'//|    Draft   |   3                           |   0                       |
'//----------------------------------------------------------
'//|    Inbox   |   1,2                         |   0                       |
'//----------------------------------------------------------
'//|    Trash   |   0,1,2,3                     |   1                       |
'//----------------------------------------------------------


Imports AgoraLegacy
Imports eProcure.Component

Imports System.Drawing

Public Class RFQ_List_SEH
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDB As New EAD.DBCom
    Protected WithEvents chkValid As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkExpired As System.Web.UI.WebControls.CheckBox

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents lblCurrentIndex As System.Web.UI.WebControls.Label
    Protected WithEvents cmd_Search As System.Web.UI.WebControls.Button
    Protected WithEvents dtg_VendorList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ddl_folder As System.Web.UI.WebControls.DropDownList
    Dim objrfq As New RFQ
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
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDuplicate As System.Web.UI.WebControls.Button
    'Protected WithEvents img_delete As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmd_clear As System.Web.UI.WebControls.Button
    'Protected WithEvents Imagebutton1 As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents ImageButton2 As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents ImageButton3 As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents img_pdf As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents Imagebutton4 As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents img_pdf2 As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents ImageButton5 As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents ImageButton6 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents hidAddItem As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents txt_startdate As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_enddate As System.Web.UI.WebControls.TextBox
    Protected WithEvents CompareValidator1 As System.Web.UI.WebControls.CompareValidator
    Dim chkRFQ_ID As String
    Dim objGlobal As New AppGlobals

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
        Chk = 0
        RFQ_No = 1
        RFQ_Name = 2
        Quo_No = 3
        Offer = 4
        COY_NAME = 5
        Currency = 6
        TotValue = 7
        rfqno = 8
        rfqid = 9
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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdDelete.Enabled = False
        cmdDuplicate.Enabled = False

        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdDelete)
        alButtonList.Add(cmdDuplicate)
        htPageAccess.Add("delete", alButtonList)
        alButtonList = New ArrayList
        htPageAccess.Add("add", alButtonList)
        CheckButtonAccess()
        If intPageRecordCnt > 0 Then
            cmdDelete.Enabled = blnCanDelete
            cmdDuplicate.Enabled = blnCanAdd
        Else
            cmdDelete.Enabled = False
            cmdDuplicate.Enabled = False
        End If

        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)

        If Not Page.IsPostBack Then
            GenerateTab()
            '' ''cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
            '' ''cmdDuplicate.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
            Dim msg As String
            Dim rfq_no As String
            msg = Request(Trim("edit"))
            rfq_no = Request(Trim("RFQ_Num"))

            '' ''DATA_GRID()

            txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
            txt_enddate.Text = DateTime.Now.ToShortDateString()
        End If

    End Sub

    Private Function Bindgrid(ByVal dg_id As DataGrid, Optional ByVal pSorted As Boolean = False) As String
        Dim ds As New DataSet
        Dim DocNum As String
        Dim VenName As String = txt_VenName.Text
        Dim v_display As Integer = 0
        Dim V_RFQ_Status As Integer = 0
        Dim strValid As String = ""
        Dim strExpired As String = ""
        DocNum = Me.txt_DocNum.Text

        Dim strMsg As String
        Dim comparedt As Date
        'comparedt = DateAdd("m", -6, DateTime.Now.ToShortDateString())
        comparedt = DateAdd("m", -6, CDate(txt_enddate.Text))

        If CDate(txt_startdate.Text) < comparedt Then
            'strMsg = "Start date " & objGlobal.GetErrorMessage("00015") & " 6 months"
            strMsg = "Date range " & objGlobal.GetErrorMessage("00015") & " 6 months."
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Return False
        End If

        '' ''cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        '' ''cmdDuplicate.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")

        If chkValid.Checked = True Then
            strValid = "valid"
        End If

        If chkExpired.Checked = True Then
            strExpired = "expired"
        End If

        'If VenName <> "" Then
        ds = objrfq.GetRFQListAllWithVendor(DocNum, VenName, strValid, strExpired, Me.txt_startdate.Text, Me.txt_enddate.Text)
        'Else
        'ds = objrfq.GetRFQListAll(DocNum)
        'End If

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

    'Private Sub dtg_Draft_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) ' Handles dtg_Draft.ItemDataBound
    '    If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
    '        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
    '        '//to add JavaScript to Check Box
    '        Dim objrfq As New RFQ
    '        Dim chk2 As CheckBox
    '        chk2 = e.Item.Cells(DraEnum.Chk).FindControl("chkSelection2")
    '        chk2.Attributes.Add("onclick", "checkChild2('" & chk2.ClientID & "')")
    '        ' e.Item.Cells(1).Text =
    '        e.Item.Cells(DraEnum.rfqno).Text = Common.parseNull(dv("RM_RFQ_No"))
    '        e.Item.Cells(DraEnum.Cre_On).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Common.parseNull(dv("RM_Created_On")))
    '        e.Item.Cells(DraEnum.Exp_Date).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Common.parseNull(dv("RM_Expiry_Date")))
    '        Dim array2(100) As String
    '        Dim COUNT As Integer
    '        Dim i As Integer
    '        Dim ARRAY(100) As String
    '        Dim lbl_rfqnum2 As Label
    '        lbl_rfqnum2 = e.Item.FindControl("lbl_rfqnum2")
    '        lbl_rfqnum2.Text = "<A href=""RFQDetail.aspx?page=1&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"
    '        If objrfq.HasAttachment(Common.parseNull(dv("RM_RFQ_No"))) Then
    '            Dim imgAttach As New System.Web.UI.WebControls.Image
    '            imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
    '            imgAttach.ImageUrl = "../Images/clip_icon.gif"
    '            e.Item.Cells(DraEnum.RFQ_No).Controls.Add(imgAttach)
    '        End If

    '        ' ai chu add
    '        Dim hidRfqId2 As HtmlInputHidden
    '        hidRfqId2 = e.Item.FindControl("hidRfqId2")
    '        hidRfqId2.Value = Common.parseNull(dv("RM_RFQ_ID"))

    '        'e.Item.Cells(3).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Common.parseNull(dv("RM_Expiry_Date")))
    '        Dim STR As String
    '        Dim com_name(100) As String
    '        Dim com_id(100) As String
    '        Dim COUNT2 As Integer
    '        Dim com_str As String
    '        objrfq.get_vendorlist(Common.parseNull(dv("RM_RFQ_ID")), com_name, com_id, COUNT2, com_str)
    '        objrfq.get_vendorName(Common.parseNull(dv("RM_RFQ_ID")), ARRAY, array2, COUNT, com_str)
    '        If com_name(0) <> "" Then
    '            For i = 0 To COUNT2 - 1
    '                STR = STR & "<A href=""Comlist.aspx?pageid=" & strPageId & "&rfq_name=" & Server.UrlEncode(e.Item.Cells(DraEnum.RFQ_Name).Text) & "&list_no=" & com_id(i) & "&list_name=" & com_name(i) & """ ><font color=#0000ff>" & com_name(i) & "</font></A><br>"
    '            Next
    '        End If
    '        If ARRAY(0) <> "" Then

    '            For i = 0 To COUNT - 1
    '                STR = STR & "<A href=""RFQ_VendorDetail.aspx?pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&v_com_id=" & array2(i) & """ ><font color=#0000ff>" & ARRAY(i) & "</font></A><br>"
    '            Next

    '        End If

    '        If STR = "" Then
    '            STR = "No vendors are found"
    '        End If
    '        e.Item.Cells(DraEnum.VenList).Text = STR
    '        'Dim lbl_rfqnum As Label
    '        'lbl_rfqnum = e.Item.FindControl("lbl_rfqnum")
    '        'lbl_rfqnum.Text = "<A href=""RFQDetail.aspx?RFQ_No=" & e.Item.Cells(2).Text & " "" ><font color=#0000ff>" & dv("RM_RFQ_Name") & "</font></A>"

    '        Dim link_edit As HyperLink
    '        link_edit = e.Item.FindControl("link_edit")
    '        'link_edit.NavigateUrl = "Create_RFQ.aspx?RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&edit=1"
    '        link_edit.NavigateUrl = "Create_RFQ.aspx?edit=1&pageid=" & strPageId & "&draft=1&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No"))
    '    End If

    'End Sub

    Public Sub dtg_VendorList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        ' viewstate("rfq_id") = ""
        viewstate("total") = 0
        Bindgrid(sender)

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        sender.CurrentPageIndex = 0
        ' viewstate("rfq_id") = ""
        viewstate("total") = 0
        Bindgrid(sender, True)

    End Sub

    Private Sub dtg_VendorList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_VendorList.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(sender, e)

        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If

    End Sub

    'Private Sub dtg_VendorList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_VendorList.ItemDataBound
    '    If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
    '        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
    '        '//to add JavaScript to Check Box
    '        Dim chk As CheckBox
    '        chk = e.Item.Cells(VenEnum.Chk).FindControl("chkSelection")
    '        chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
    '        e.Item.Cells(VenEnum.rfqno).Text = Common.parseNull(dv("RM_RFQ_No"))
    '        Dim array2(100) As String
    '        Dim objrfq As New RFQ
    '        Dim COUNT As Integer
    '        Dim COUNT2 As Integer
    '        Dim com_name(100) As String
    '        Dim ARRAY(100) As String
    '        Dim com_id(100) As String
    '        Dim i As Integer
    '        e.Item.Cells(VenEnum.Cre_On).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Common.parseNull(dv("RM_Created_On")))
    '        e.Item.Cells(VenEnum.Exp_Date).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Common.parseNull(dv("RM_Expiry_Date")))

    '        ' ai chu add
    '        Dim hidRfqId As HtmlInputHidden
    '        hidRfqId = e.Item.FindControl("hidRfqId")
    '        hidRfqId.Value = Common.parseNull(dv("RM_RFQ_ID"))

    '        Dim STR As String
    '        Dim com_str As String
    '        objrfq.get_vendorlist(Common.parseNull(dv("RM_RFQ_ID")), com_name, com_id, COUNT2, com_str)
    '        objrfq.get_vendorName(Common.parseNull(dv("RM_RFQ_ID")), ARRAY, array2, COUNT, com_str)
    '        If com_name(0) <> "" Then
    '            ' for 
    '            For i = 0 To COUNT2 - 1
    '                STR = STR & "<A href=""" & dDispatcher.direct("RFQ", "Comlist.aspx", "pageid=" & strPageId & "&list_name=" & com_name(i) & "&rfq_name=" & Server.UrlEncode(e.Item.Cells(VenEnum.RFQ_Name).Text) & "&list_no=" & com_id(i)) & """ ><font color=#0000ff>" & com_name(i) & "</font></A><br>"
    '            Next
    '        End If
    '        If ARRAY(0) <> "" Then
    '            For i = 0 To COUNT - 1
    '                STR = STR & "<A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "Frm=RFQ_List&pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&v_com_id=" & array2(i)) & """ ><font color=#0000ff>" & ARRAY(i) & "</font></A><br>"
    '            Next
    '        End If

    '        If objrfq.HasAttachment(Common.parseNull(dv("RM_RFQ_No"))) Then
    '            Dim imgAttach As New System.Web.UI.WebControls.Image
    '            imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
    '            imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")

    '            e.Item.Cells(VenEnum.RFQ_No1).Controls.Add(imgAttach)
    '        End If

    '        If STR = "" Then
    '            STR = "No vendors are found"
    '        End If
    '        e.Item.Cells(VenEnum.VenList).Text = STR

    '        Dim lbl_rfqnum As Label
    '        lbl_rfqnum = e.Item.FindControl("lbl_rfqnum")
    '        'lbl_rfqnum.Text = "<A href=""RFQDetail.aspx?Frm=RFQ_Outstg_List&pageid=" & strPageId & "&page=1&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"
    '        'e.Item.Cells(5).Text = "<A href=""RFQDetail.aspx?RFQ_No=" & e.Item.Cells(2).Text & " "" ><font color=#0000ff>" & Common.parseNull(dv("RVDLM_List_Name") & "</font></A>"
    '        Dim lbl_status As Label
    '        lbl_status = e.Item.FindControl("lbl_status")
    '        Dim check As Boolean = objrfq.check_v_status(Common.parseNull(dv("RM_RFQ_ID"))) ' 1=response
    '        If dv("RM_Status") = "3" Then
    '            lbl_status.Text = "Draft"
    '            lbl_rfqnum.Text = "<A href=""" & dDispatcher.direct("RFQ", "Create_RFQ.aspx", "Frm=RFQ_List&pageid=" & strPageId & "&page=1&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID"))) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"
    '        Else
    '            If IsDBNull(dv("RM_Expiry_Date")) Then

    '            Else
    '                lbl_rfqnum.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQDetail.aspx", "Frm=RFQ_List&pageid=" & strPageId & "&page=1&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID"))) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"

    '                If Common.parseNull(dv("RM_Expiry_Date")) >= Date.Today Then
    '                    If check = False Then
    '                        lbl_status.Text = "Sent"
    '                        'lbl_rfqnum.Text = "<A href=""RFQDetail.aspx?Frm=RFQ_List&pageid=" & strPageId & "&page=1&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"

    '                    ElseIf check = True Then
    '                        If dv("RM_RFQ_OPTION") = 0 Then 'open
    '                            lbl_status.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=RFQ_List&pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Server.UrlEncode(Common.parseNull(dv("RM_RFQ_Name")))) & """ ><font color=#0000ff>View Response</font></A>"
    '                            'lbl_rfqnum.Text = "<A href=""RFQDetail.aspx?Frm=RFQ_List&pageid=" & strPageId & "&page=1&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"

    '                        ElseIf dv("RM_RFQ_OPTION") = 1 Then  'close
    '                            lbl_status.Text = "Response"
    '                        End If
    '                    End If
    '                Else
    '                    If check = False Then
    '                        lbl_status.Text = "No Response, Expired"
    '                    ElseIf check = True Then
    '                        lbl_status.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=RFQ_List&pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Server.UrlEncode(Common.parseNull(dv("RM_RFQ_Name")))) & """ ><font color=#0000ff>View Response</font></A>, Expired"
    '                    End If
    '                End If
    '            End If
    '        End If
    '    End If
    'End Sub

    Private Sub dtg_VendorList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_VendorList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(VenEnum.Chk).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            e.Item.Cells(VenEnum.rfqno).Text = Common.parseNull(dv("RM_RFQ_No"))
            Dim array2(100) As String
            Dim objrfq As New RFQ
            Dim COUNT As Integer
            Dim COUNT2 As Integer
            Dim com_name(100) As String
            Dim ARRAY(100) As String
            Dim com_id(100) As String
            Dim i As Integer

            e.Item.Cells(VenEnum.Cre_On).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Common.parseNull(dv("RM_Created_On")))
            e.Item.Cells(VenEnum.Exp_Date).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Common.parseNull(dv("RM_Expiry_Date")))

            ' ai chu add
            Dim hidRfqId As HtmlInputHidden
            hidRfqId = e.Item.FindControl("hidRfqId")
            hidRfqId.Value = Common.parseNull(dv("RM_RFQ_ID"))

            Dim STR As String
            Dim com_str As String
            objrfq.get_vendorlist(Common.parseNull(dv("RM_RFQ_ID")), com_name, com_id, COUNT2, com_str)
            objrfq.get_vendorName(Common.parseNull(dv("RM_RFQ_ID")), ARRAY, array2, COUNT, com_str)
            If com_name(0) <> "" Then
                ' for 
                For i = 0 To COUNT2 - 1
                    STR = STR & "<A href=""" & dDispatcher.direct("RFQ", "Comlist.aspx", "pageid=" & strPageId & "&rfq_id=" & Common.parseNull(dv("RM_RFQ_ID")) & "&list_name=" & com_name(i) & "&rfq_name=" & Server.UrlEncode(e.Item.Cells(VenEnum.RFQ_Name).Text) & "&list_no=" & com_id(i)) & """ ><font color=#0000ff>" & com_name(i) & "</font></A><br>"
                Next
            End If
            If ARRAY(0) <> "" Then
                For i = 0 To COUNT - 1
                    STR = STR & "<A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "Frm=RFQ_List&pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&v_com_id=" & array2(i)) & """ ><font color=#0000ff>" & ARRAY(i) & "</font></A><br>"
                Next
            End If

            If objrfq.HasAttachment(Common.parseNull(dv("RM_RFQ_No")), "E") Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")

                e.Item.Cells(VenEnum.RFQ_No1).Controls.Add(imgAttach)
            End If

            If STR = "" Then
                STR = "No vendors are found"
            End If
            e.Item.Cells(VenEnum.VenList).Text = STR

            Dim lbl_rfqnum As Label
            lbl_rfqnum = e.Item.FindControl("lbl_rfqnum")
            'lbl_rfqnum.Text = "<A href=""RFQDetail.aspx?Frm=RFQ_Outstg_List&pageid=" & strPageId & "&page=1&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"
            'e.Item.Cells(5).Text = "<A href=""RFQDetail.aspx?RFQ_No=" & e.Item.Cells(2).Text & " "" ><font color=#0000ff>" & Common.parseNull(dv("RVDLM_List_Name") & "</font></A>"
            Dim lbl_status As Label
            lbl_status = e.Item.FindControl("lbl_status")
            Dim check As Boolean = objrfq.check_v_status(Common.parseNull(dv("RM_RFQ_ID"))) ' 1=response
            If dv("RM_Status") = "3" Then   'Draft
                lbl_status.Text = "Draft"
                lbl_rfqnum.Text = "<A href=""" & dDispatcher.direct("RFQ", "Create_RFQ.aspx", "Frm=RFQ_List&pageid=" & strPageId & "&page=1&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID"))) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"
            Else
                If IsDBNull(dv("RM_Expiry_Date")) Then

                Else
                    lbl_rfqnum.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQDetail.aspx", "Frm=RFQ_List&pageid=" & strPageId & "&page=1&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID"))) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"

                    If Common.parseNull(dv("RM_Expiry_Date")) >= Date.Today Then    'Not expired
                        If check = False Then   'No response
                            lbl_status.Text = "Sent"
                            'lbl_rfqnum.Text = "<A href=""RFQDetail.aspx?Frm=RFQ_List&pageid=" & strPageId & "&page=1&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"

                        ElseIf check = True Then    'Response
                            If dv("RM_RFQ_OPTION") = 0 Or dv("RM_RFQ_OPTION") = 1 Then 'open
                                lbl_status.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQComSummary.aspx", "disabled=N&Frm=RFQ_List&RFQType=V&pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Server.UrlEncode(Common.parseNull(dv("RM_RFQ_Name")))) & """ ><font color=#0000ff>View Response</font></A>"
                                '' ''If dv("STAT") = 0 Then

                                'Dim strRFQ_No As String = objDB.GetVal("SELECT PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & Common.parseNull(dv("RM_RFQ_No")) & "'")
                                'If strRFQ_No <> "" Then

                                'Dim strRFQ_Index As String
                                'strRFQ_Index = objDB.GetVal("SELECT RM_RFQ_NO FROM RFQ_MSTR, RFQ_DETAIL " & _
                                '                " WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                '                " RM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' AND " & _
                                '                " (RD_PRODUCT_DESC NOT IN ( " & _
                                '                " SELECT POD_PRODUCT_DESC FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND " & _
                                '                " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = ( " & _
                                '                " SELECT RM_RFQ_ID FROM RFQ_MSTR WHERE RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                '                " RM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "') )) LIMIT 1 ")

                                Dim strRFQ_Index As String
                                strRFQ_Index = objDB.GetVal("SELECT COUNT(RM_RFQ_NO) " & _
                                                " FROM RFQ_MSTR, RFQ_DETAIL " & _
                                                " WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                                " RM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' " & _
                                                " HAVING COUNT(RM_RFQ_NO) > " & _
                                                " (SELECT COUNT(POM_PO_NO) FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND " & _
                                                " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = '" & Common.parseNull(dv("RM_RFQ_ID")) & "') ")

                                If strRFQ_Index <> "" Then
                                    Dim strRFQ_Index2 As String
                                    strRFQ_Index2 = objDB.GetVal("SELECT COUNT(POM_PO_NO) FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND " & _
                                                    " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = '" & Common.parseNull(dv("RM_RFQ_ID")) & "'")

                                    If strRFQ_Index2 <> "0" Then
                                        lbl_status.Text = lbl_status.Text & "<BR>" & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("RFQ", "RFQPOList.aspx", "pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "") & "'); ""><font color=#0000ff>Partial PO</font></A>"
                                    End If

                                Else
                                    Dim strRFQ_Index2 As String
                                    'strRFQ_Index2 = objDB.GetVal("SELECT COUNT(RM_RFQ_NO) " & _
                                    '                " FROM RFQ_MSTR, RFQ_DETAIL " & _
                                    '                " WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                    '                " RM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' " & _
                                    '                " HAVING COUNT(RM_RFQ_NO) <= " & _
                                    '                " (SELECT COUNT(POM_PO_NO) FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND " & _
                                    '                " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = '" & Common.parseNull(dv("RM_RFQ_ID")) & "') ")

                                    'strRFQ_Index2 = objDB.GetVal("SELECT RM_RFQ_NO " & _
                                    '                " FROM RFQ_MSTR, RFQ_DETAIL, PO_MSTR, PO_DETAILS " & _
                                    '                " WHERE RD_RFQ_LINE = POD_RFQ_ITEM_LINE AND POM_RFQ_INDEX = RM_RFQ_ID AND POM_B_COY_ID = POD_COY_ID AND POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' AND RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                    '                " RM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' " & _
                                    '                " ")

                                    strRFQ_Index2 = objDB.GetVal("SELECT RM_RFQ_NO FROM RFQ_MSTR, RFQ_DETAIL " & _
                                                    " WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                                    " RM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' AND RD_RFQ_LINE NOT IN ( " & _
                                                    " SELECT DISTINCT POD_RFQ_ITEM_LINE FROM PO_MSTR, PO_DETAILS " & _
                                                    " WHERE POM_B_COY_ID = POD_COY_ID AND " & _
                                                    " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                                    " POM_RFQ_INDEX = '" & Common.parseNull(dv("RM_RFQ_ID")) & "') ")

                                    If strRFQ_Index2 <> "" Then
                                        lbl_status.Text = lbl_status.Text & "<BR>" & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("RFQ", "RFQPOList.aspx", "pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "") & "'); ""><font color=#0000ff>Partial PO</font></A>"
                                    Else
                                        lbl_status.Text = lbl_status.Text & "<BR>" & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("RFQ", "RFQPOList.aspx", "pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "") & "'); ""><font color=#0000ff>Full PO</font></A>"
                                    End If


                                End If
                                'End If
                                '' ''End If
                                'lbl_rfqnum.Text = "<A href=""RFQDetail.aspx?Frm=RFQ_List&pageid=" & strPageId & "&page=1&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"

                                'ElseIf dv("RM_RFQ_OPTION") = 1 Then  'close
                                '    lbl_status.Text = "Response"
                            End If
                        End If
                    Else    'Expired
                        If check = False Then   'No response
                            'lbl_status.Text = "No Response, Expired"
                            lbl_status.Text = "Sent" & "<BR>" & " Expired"

                        ElseIf check = True Then
                            If dv("RM_Reqd_Quote_Validity") >= Date.Today Then
                                lbl_status.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQComSummary.aspx", "disabled=N&Frm=RFQ_List&RFQType=V&pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Server.UrlEncode(Common.parseNull(dv("RM_RFQ_Name")))) & """ ><font color=#0000ff>View Response</font></A><BR><font color=#000000>Expired</font>"
                                '' ''If dv("STAT") = 0 Then
                                'Dim strRFQ_No As String = objDB.GetVal("SELECT PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & Common.parseNull(dv("RM_RFQ_No")) & "'")
                                'If strRFQ_No <> "" Then

                                'Dim strRFQ_Index As String
                                'strRFQ_Index = objDB.GetVal("SELECT RM_RFQ_NO FROM RFQ_MSTR, RFQ_DETAIL " & _
                                '                " WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                '                " RM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' AND " & _
                                '                " (RD_PRODUCT_DESC NOT IN ( " & _
                                '                " SELECT POD_PRODUCT_DESC FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND " & _
                                '                " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = ( " & _
                                '                " SELECT RM_RFQ_ID FROM RFQ_MSTR WHERE RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                '                " RM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "') )) LIMIT 1 ")

                                Dim strRFQ_Index As String
                                strRFQ_Index = objDB.GetVal("SELECT COUNT(RM_RFQ_NO) " & _
                                                " FROM RFQ_MSTR, RFQ_DETAIL " & _
                                                " WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                                " RM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' " & _
                                                " HAVING COUNT(RM_RFQ_NO) > " & _
                                                " (SELECT COUNT(POM_PO_NO) FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND " & _
                                                " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = '" & Common.parseNull(dv("RM_RFQ_ID")) & "') ")

                                If strRFQ_Index <> "" Then
                                    Dim strRFQ_Index2 As String
                                    strRFQ_Index2 = objDB.GetVal("SELECT COUNT(POM_PO_NO) FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND " & _
                                                    " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = '" & Common.parseNull(dv("RM_RFQ_ID")) & "'")

                                    If strRFQ_Index2 <> "0" Then
                                        lbl_status.Text = lbl_status.Text & "<BR>" & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("RFQ", "RFQPOList.aspx", "pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "") & "'); ""><font color=#0000ff>Partial PO</font></A>"
                                    End If

                                Else
                                    Dim strRFQ_Index2 As String
                                    'strRFQ_Index2 = objDB.GetVal("SELECT COUNT(RM_RFQ_NO) " & _
                                    '                " FROM RFQ_MSTR, RFQ_DETAIL " & _
                                    '                " WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                    '                " RM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' " & _
                                    '                " HAVING COUNT(RM_RFQ_NO) <= " & _
                                    '                " (SELECT COUNT(POM_PO_NO) FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND " & _
                                    '                " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = '" & Common.parseNull(dv("RM_RFQ_ID")) & "') ")

                                    'strRFQ_Index2 = objDB.GetVal("SELECT RM_RFQ_NO " & _
                                    '                " FROM RFQ_MSTR, RFQ_DETAIL, PO_MSTR, PO_DETAILS " & _
                                    '                " WHERE RD_RFQ_LINE = POD_RFQ_ITEM_LINE AND POM_RFQ_INDEX = RM_RFQ_ID AND POM_B_COY_ID = POD_COY_ID AND POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' AND RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                    '                " RM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' " & _
                                    '                " ")

                                    strRFQ_Index2 = objDB.GetVal("SELECT RM_RFQ_NO FROM RFQ_MSTR, RFQ_DETAIL " & _
                                                    " WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                                    " RM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' AND RD_RFQ_LINE NOT IN ( " & _
                                                    " SELECT DISTINCT POD_RFQ_ITEM_LINE FROM PO_MSTR, PO_DETAILS " & _
                                                    " WHERE POM_B_COY_ID = POD_COY_ID AND " & _
                                                    " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                                    " POM_RFQ_INDEX = '" & Common.parseNull(dv("RM_RFQ_ID")) & "') ")

                                    If strRFQ_Index2 <> "" Then
                                        lbl_status.Text = lbl_status.Text & "<BR>" & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("RFQ", "RFQPOList.aspx", "pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "") & "'); ""><font color=#0000ff>Partial PO</font></A>"
                                    Else
                                        lbl_status.Text = lbl_status.Text & "<BR>" & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("RFQ", "RFQPOList.aspx", "pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "") & "'); ""><font color=#0000ff>Full PO</font></A>"
                                    End If
                                End If
                                'End If
                                'dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "Frm=RFQ_List&pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&v_com_id=" & array2(i))
                                '' ''End If
                            Else
                                lbl_status.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Appr=Y&Frm=RFQ_List&RFQType=S&pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Server.UrlEncode(Common.parseNull(dv("RM_RFQ_Name")))) & """ ><font color=#0000ff>Sent</font></A><BR><font color=#000000>Expired</font>"
                                '' ''If dv("STAT") = 0 Then
                                'Dim strRFQ_No As String = objDB.GetVal("SELECT PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & Common.parseNull(dv("RM_RFQ_No")) & "'")
                                'If strRFQ_No <> "" Then

                                'Dim strRFQ_Index As String
                                'strRFQ_Index = objDB.GetVal("SELECT RM_RFQ_NO FROM RFQ_MSTR, RFQ_DETAIL " & _
                                '                " WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                '                " RM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' AND " & _
                                '                " (RD_PRODUCT_DESC NOT IN ( " & _
                                '                " SELECT POD_PRODUCT_DESC FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND " & _
                                '                " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = ( " & _
                                '                " SELECT RM_RFQ_ID FROM RFQ_MSTR WHERE RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                '                " RM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "') )) LIMIT 1 ")

                                Dim strRFQ_Index As String
                                strRFQ_Index = objDB.GetVal("SELECT COUNT(RM_RFQ_NO) " & _
                                                " FROM RFQ_MSTR, RFQ_DETAIL " & _
                                                " WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                                " RM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' " & _
                                                " HAVING COUNT(RM_RFQ_NO) > " & _
                                                " (SELECT COUNT(POM_PO_NO) FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND " & _
                                                " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = '" & Common.parseNull(dv("RM_RFQ_ID")) & "') ")

                                If strRFQ_Index <> "" Then
                                    Dim strRFQ_Index2 As String
                                    strRFQ_Index2 = objDB.GetVal("SELECT COUNT(POM_PO_NO) FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND " & _
                                                    " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = '" & Common.parseNull(dv("RM_RFQ_ID")) & "'")

                                    If strRFQ_Index2 <> "0" Then
                                        lbl_status.Text = lbl_status.Text & "<BR>" & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("RFQ", "RFQPOList.aspx", "pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "") & "'); ""><font color=#0000ff>Partial PO</font></A>"
                                    End If

                                Else
                                    Dim strRFQ_Index2 As String
                                    'strRFQ_Index2 = objDB.GetVal("SELECT COUNT(RM_RFQ_NO) " & _
                                    '                " FROM RFQ_MSTR, RFQ_DETAIL " & _
                                    '                " WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                    '                " RM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' " & _
                                    '                " HAVING COUNT(RM_RFQ_NO) <= " & _
                                    '                " (SELECT COUNT(POM_PO_NO) FROM PO_MSTR, PO_DETAILS WHERE POM_B_COY_ID = POD_COY_ID AND " & _
                                    '                " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = '" & Common.parseNull(dv("RM_RFQ_ID")) & "') ")

                                    'strRFQ_Index2 = objDB.GetVal("SELECT RM_RFQ_NO " & _
                                    '                " FROM RFQ_MSTR, RFQ_DETAIL, PO_MSTR, PO_DETAILS " & _
                                    '                " WHERE RD_RFQ_LINE = POD_RFQ_ITEM_LINE AND POM_RFQ_INDEX = RM_RFQ_ID AND POM_B_COY_ID = POD_COY_ID AND POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND POM_RFQ_INDEX = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' AND RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                    '                " RM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' " & _
                                    '                " ")

                                    strRFQ_Index2 = objDB.GetVal("SELECT RM_RFQ_NO FROM RFQ_MSTR, RFQ_DETAIL " & _
                                                    " WHERE RM_RFQ_ID = RD_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                                    " RM_RFQ_ID = '" & Common.parseNull(dv("RM_RFQ_ID")) & "' AND RD_RFQ_LINE NOT IN ( " & _
                                                    " SELECT DISTINCT POD_RFQ_ITEM_LINE FROM PO_MSTR, PO_DETAILS " & _
                                                    " WHERE POM_B_COY_ID = POD_COY_ID AND " & _
                                                    " POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND " & _
                                                    " POM_RFQ_INDEX = '" & Common.parseNull(dv("RM_RFQ_ID")) & "') ")

                                    If strRFQ_Index2 <> "" Then
                                        lbl_status.Text = lbl_status.Text & "<BR>" & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("RFQ", "RFQPOList.aspx", "pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "") & "'); ""><font color=#0000ff>Partial PO</font></A>"
                                    Else
                                        lbl_status.Text = lbl_status.Text & "<BR>" & "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("RFQ", "RFQPOList.aspx", "pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "") & "'); ""><font color=#0000ff>Full PO</font></A>"
                                    End If
                                End If
                                'End If
                                'dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "Frm=RFQ_List&pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&v_com_id=" & array2(i))
                                '' ''End If
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub dtg_Qoute_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) ' Handles dtg_Qoute.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        ' Grid_ItemCreated(dtg_Qoute, e)

        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll3 As CheckBox = e.Item.FindControl("chkAll3")
            chkAll3.Attributes.Add("onclick", "selectAll3();")
        End If

        'e.Item.Attributes.Add("style", "CURSOR:hand")
        '  e.Item.Attributes.Add("Onclick", "selectRow(" & e.Item.ItemIndex & ");")

    End Sub

    'Private Sub dtg_Qoute_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) 'Handles dtg_Qoute.ItemDataBound
    '    If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
    '        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
    '        '//to add JavaScript to Check Box   

    '        count = ViewState("total")

    '        Dim chk3 As CheckBox
    '        chk3 = e.Item.Cells(QuoEnum.Chk).FindControl("chkSelection3")
    '        chk3.Visible = False
    '        chk3.Enabled = False

    '        Dim lbl_name As Label
    '        Dim lbl_rfqnum3 As Label
    '        lbl_name = e.Item.FindControl("lbl_name")
    '        lbl_rfqnum3 = e.Item.FindControl("lbl_rfqnum3")

    '        If CStr(Common.parseNull(dv("RM_RFQ_ID"))) <> chkRFQ_ID Then
    '            lbl_name.Text = Common.parseNull(dv("RM_RFQ_Name"))
    '            lbl_rfqnum3.Text = "<A href=""RFQDetail.aspx?pageid=" & strPageId & "&page=1&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"
    '            total = count Mod 2
    '            count = count + 1
    '            If objrfq.HasAttachment(Common.parseNull(dv("RM_RFQ_No"))) Then
    '                Dim imgAttach As New System.Web.UI.WebControls.Image
    '                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
    '                imgAttach.ImageUrl = "../Images/clip_icon.gif"
    '                e.Item.Cells(QuoEnum.RFQ_No).Controls.Add(imgAttach)
    '            End If
    '        End If
    '        e.Item.Cells(QuoEnum.COY_NAME).Text = "<A href=""RFQ_VendorDetail.aspx?pageid=" & strPageId & "&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&v_com_id=" & dv("RVM_V_Company_ID") & """ ><font color=#0000ff>" & dv("CM_COY_NAME") & "</font></A><br>"
    '        ViewState("total") = count

    '        If total = 0 Then
    '            e.Item.BackColor = Color.LightYellow()
    '        Else
    '            e.Item.BackColor = Color.FromName("#f6f9fe")
    '        End If
    '        chkRFQ_ID = CStr(Common.parseNull(dv("RM_RFQ_ID")))

    '        ' ai chu add
    '        Dim hidRfqId3 As HtmlInputHidden
    '        hidRfqId3 = e.Item.FindControl("hidRfqId3")
    '        hidRfqId3.Value = ""

    '        Dim lbl_QuoValidity As Label
    '        lbl_QuoValidity = e.Item.FindControl("lbl_QuoValidity")
    '        Dim lbl_qouteNum As Label
    '        lbl_qouteNum = e.Item.FindControl("lbl_qouteNum")

    '        If IsDBNull(Common.parseNull(dv("RRM_Actual_Quot_Num"))) Or Common.parseNull(dv("RRM_Actual_Quot_Num")) = "" Or Common.parseNull(dv("RRM_Actual_Quot_Num")) = " " Then
    '            lbl_qouteNum.Text = "N/A"
    '        Else
    '            ' ai chu add on 12/10/2005
    '            ' for close RFQ, buyer is not suppose to view the quotation that not yet expired
    '            ' RM_Expiry_Date, RM_RFQ_OPTION
    '            If Not IsDBNull(dv("RM_Expiry_Date")) Then
    '                If dv("RM_RFQ_OPTION") = "1" And dv("RM_Expiry_Date") >= Today Then
    '                    lbl_qouteNum.Text = Common.parseNull(dv("RRM_Actual_Quot_Num"))
    '                Else
    '                    lbl_qouteNum.Text = "<A href=""ViewQoute.aspx?pageid=" & strPageId & "&qou_num=" & Common.parseNull(dv("RRM_Actual_Quot_Num")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&vcomid=" & Common.parseNull(dv("RRM_V_Company_ID")) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RRM_Actual_Quot_Num")) & "</font></A>"
    '                End If
    '            End If

    '            hidRfqId3.Value = Common.parseNull(dv("RRM_V_Company_ID"))
    '            chk3.Visible = True
    '            chk3.Enabled = True

    '            ' ai chu add on 06/12/2005
    '            ' SR U30025 - to add in idcication for quotation received which is converted to PR
    '            ' Remarks: if more than 1 PR raised from the Quotation, system will redirect user to a listing
    '            '          page to list all PR number raised from the quotation number, else system will redirect
    '            '          user to PR Details screen 

    '            Dim objPR As New PR
    '            Dim dsPR As DataSet
    '            dsPR = objPR.getPRFromQuot(dv("RRM_Actual_Quot_Num"), dv("RRM_V_Company_ID"), Session("CompanyId"))

    '            Dim lnkPR As New HyperLink
    '            'Dim strRFQNo, strRFQName As String
    '            'objPR.getRFQName(dv("PRM_RFQ_INDEX"), strRFQNo, strRFQName)
    '            lnkPR.ImageUrl = "../Images/i_PR2.gif"
    '            If Not dsPR Is Nothing Then
    '                If dsPR.Tables(0).Rows.Count <> 0 Then
    '                    If dsPR.Tables(0).Rows.Count = 1 Then
    '                        lnkPR.NavigateUrl = "../PO/PRDetail.aspx?caller=OTHER&PageId=" & strPageId & "&index=" & dsPR.Tables(0).Rows(0)("PRM_PR_INDEX") & "&PRNo=" & dsPR.Tables(0).Rows(0)("PRM_PR_NO")
    '                    ElseIf dsPR.Tables(0).Rows.Count > 1 Then
    '                        lnkPR.NavigateUrl = "../PR/PRList.aspx?type=PR&PageId=" & strPageId & "&CoyId=" & dv("RRM_V_Company_ID") & "&DocNo=" & Server.UrlEncode(dv("RRM_Actual_Quot_Num"))
    '                    End If
    '                    lnkPR.ToolTip = "Click here to view PR"
    '                    e.Item.Cells(QuoEnum.Quo_No).Controls.Add(lnkPR)
    '                End If
    '                Session("strurl") = strCallFrom
    '            End If
    '        End If

    '        chk3.Attributes.Add("onclick", "checkChild3('" & chk3.ClientID & "')")
    '        If IsDBNull(dv("RRM_Offer_Till")) Then
    '            lbl_QuoValidity.Text = "N/A"
    '        Else
    '            lbl_QuoValidity.Text = Common.parseNull(dv("RRM_Offer_Till"))
    '        End If
    '        Dim lbl_total As Label
    '        lbl_total = e.Item.FindControl("lbl_total")

    '        If objrfq.HasAttachmentQuote(Common.parseNull(dv("RM_RFQ_No")), Common.parseNull(dv("RRM_V_Company_ID"))) Then
    '            Dim imgAttach As New System.Web.UI.WebControls.Image
    '            imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
    '            imgAttach.ImageUrl = "../Images/clip_icon.gif"
    '            e.Item.Cells(QuoEnum.Quo_No).Controls.Add(imgAttach)

    '        End If

    '        If IsDBNull(dv("RRM_TotalValue")) Then
    '            lbl_total.Text = "N/A"
    '        Else
    '            ' lbl_total.Text = Common.parseNull(dv("RRM_Currency_Code")) & " " & Format(Common.parseNull(dv("RRM_TotalValue")), "#,##0.00")
    '            '*****28/01/05 Esther *******
    '            If Not IsDBNull(dv("RM_Expiry_Date")) Then
    '                If dv("RM_RFQ_OPTION") = "1" And dv("RM_Expiry_Date") >= Today Then
    '                    lbl_total.Text = "-"
    '                Else
    '                    lbl_total.Text = Format(Common.parseNull(dv("RRM_TotalValue")), "#,##0.00")
    '                End If
    '            End If
    '        End If

    '        '  e.Item.Cells(EnumInvSearch.icAmount).Text = Format(Common.parseNull(dv("IM_INVOICE_TOTAL")), "#,##0.00")
    '    End If
    'End Sub

    'Sub ShowStats()
    '    lblCurrentIndex.Text = record & " Record(s) Found"
    '    ' lblPageCount.Text = "Total Page is " & MyDataGrid.PageCount
    'End Sub

    Private Sub dtg_Trash_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) 'Handles dtg_Trash.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(sender, e)

        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll4 As CheckBox = e.Item.FindControl("chkAll4")
            chkAll4.Attributes.Add("onclick", "selectAll4();")
        End If

    End Sub

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

    Private Sub cmd_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Search.Click
        dtg_VendorList.CurrentPageIndex = 0
        DATA_GRID()
    End Sub

    Sub DATA_GRID()
        ViewState("SortExpression") = "RM_Created_On"
        ViewState("SortAscending") = "no"
        DisplayExtraFunction()
    End Sub

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

        Dim strPO_No As String
        For Each dgitem In dg_id.Items
            chk = dgitem.FindControl("chkSelection")
            strStatus = dgitem.FindControl("lbl_status")
            'If chk.Checked Then
            '    If strStatus.Text = "Draft" Then
            '        dtr = delete.NewRow()
            '        dtr("B_NEW_STATUS") = B_NEW_STATUS
            '        dtr("B_CURR_STATUS") = B_CURR_STATUS
            '        dtr("rfq_no") = dgitem.Cells(7).Text
            '        delete.Rows.Add(dtr)
            '    Else
            '        Common.NetMsgbox(Me, "RFQ that has not expired cannot be removed.", MsgBoxStyle.Information)
            '        Exit Function
            '    End If
            'End If

            If chk.Checked Then
                strPO_No = objrfq.get_po_no(dgitem.Cells(7).Text)
                If strPO_No = "" Then
                    dtr = delete.NewRow()
                    dtr("B_NEW_STATUS") = B_NEW_STATUS
                    dtr("B_CURR_STATUS") = B_CURR_STATUS
                    dtr("rfq_no") = dgitem.Cells(7).Text
                    delete.Rows.Add(dtr)
                Else
                    'Common.NetMsgbox(Me, "RFQ that has not expired cannot be removed.", MsgBoxStyle.Information)
                    Common.NetMsgbox(Me, dgitem.Cells(7).Text & " that is tied to PO.", MsgBoxStyle.Information)
                    Exit Function
                End If
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
        Me.chkValid.Checked = False
        Me.chkExpired.Checked = False
        Me.txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
        Me.txt_enddate.Text = DateTime.Now.ToShortDateString()
    End Sub

    Private Sub Imagebutton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) ' Handles Imagebutton1.Click
        goto_trash()
    End Sub

    Function copy_rfq()
        Dim i As Integer = 1
        Dim objrfq As New RFQ
        Dim objrfq_ext As New RFQ_Ext
        Dim rfq_name As String
        Dim dg_id As DataGrid
        Dim Bdisplaystatus As String = "0"
        Dim rm_status As String = "3"

        Dim str_rfqid As String
        Dim j As Integer

        dg_id = dtg_VendorList

        Dim dgitem As DataGridItem
        Dim chk As CheckBox
        Dim DS As New DataSet
        Dim dtr As DataRow
        Dim delete As New DataTable
        Dim new_rfq_id As String
        Dim blnItemDeleted As Boolean = False
        Dim blnDuplicate As Boolean = False
        Dim strDuplicateName As String

        For Each dgitem In dg_id.Items
            chk = dgitem.FindControl("chkSelection")

            If chk.Checked Then
                If objrfq.checkItemDeleted(dgitem.Cells(9).Text) Then
                    blnItemDeleted = True
                End If
            End If
        Next

        If Not blnItemDeleted Then
            For Each dgitem In dg_id.Items
                chk = dgitem.FindControl("chkSelection")

                If chk.Checked Then
                    rfq_name = dgitem.Cells(2).Text

                    Do Until objrfq.get_rfqName(rfq_name & "(copy " & i & ")") = True
                        i = i + 1
                    Loop

                    new_rfq_id = objrfq_ext.copy_rfq(rfq_name & "(copy " & i & ")", dgitem.Cells(9).Text, Bdisplaystatus, rm_status)
                    ' ai chu modified on 26/11/2005
                    ' if new_rfq_id = "-1" - duplicate rfq number found; maybe caused by user change last used number in company parameter
                    If new_rfq_id <> "-1" Then
                        'If str_rfqid = "" Then
                        '    str_rfqid = "'" & new_rfq_id & "' "
                        'Else
                        '    str_rfqid = str_rfqid & ",'" & new_rfq_id & "' "
                        'End If
                        If str_rfqid = "" Then
                            str_rfqid = "'" & new_rfq_id & "' "
                            strDuplicateName = "'" & rfq_name & "' "
                        Else
                            str_rfqid = str_rfqid & ",'" & new_rfq_id & "' "
                            strDuplicateName = strDuplicateName & ",'" & rfq_name & "' "
                        End If
                    Else
                        blnDuplicate = True
                    End If

                End If
            Next
            If Not blnDuplicate Then
                'Response.Redirect("cofCopy.aspx?rfq_id=" & str_rfqid & "&pageid=" & strPageId & "")
                Common.NetMsgbox(Me, strDuplicateName & "is/are successfully duplicated.", dDispatcher.direct("RFQ", "RFQ_List.aspx", "pageid=" & strPageId))
            Else
                Common.NetMsgbox(Me, "Duplicate transaction number found.""&vbCrLf&""Please contact your Administrator to rectify the problem.", MsgBoxStyle.Information)
                'Bindgrid(dtg_Draft)
            End If
        Else
            Common.NetMsgbox(Me, "Unable to copy as some of the items have been deleted from the List Price Catalogue.", MsgBoxStyle.Information)
        End If
    End Function

    'Private Sub ImageButton2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
    '    copy_rfq()
    'End Sub

    'Private Sub ImageButton3_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton3.Click
    '    copy_rfq()
    'End Sub

    'Sub submition()
    '    Dim strrfqid As String
    '    Dim chk As CheckBox
    '    Dim dgitem As DataGridItem
    '    Dim send_rfq_no As String
    '    For Each dgitem In dtg_Draft.Items
    '        chk = dgitem.FindControl("chkSelection2")
    '        If chk.Checked Then
    '            If objrfq.send(dgitem.Cells(9).Text) = "2" Then
    '                If send_rfq_no = "" Then
    '                    send_rfq_no = dgitem.Cells(8).Text
    '                Else
    '                    send_rfq_no = send_rfq_no & "," & dgitem.Cells(8).Text
    '                End If
    '            Else
    '                If strrfqid = "" Then
    '                    strrfqid = dgitem.Cells(9).Text
    '                Else
    '                    strrfqid = strrfqid & "," & dgitem.Cells(9).Text
    '                End If
    '            End If
    '        End If
    '    Next

    '    Dim vbs As String
    '    vbs = vbs & "<script language=""javascript"">"
    '    vbs = vbs & "window.open('RFQSubmit.aspx?pageid=" & strPageId & "&rfqsend=" & send_rfq_no & "&rfq_id=" & strrfqid & "','','width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes')"
    '    vbs = vbs & "</script>"
    '    Dim rndKey As New Random
    '    Me.RegisterStartupScript(rndKey.Next.ToString, vbs)
    'End Sub

    Sub submition()
        Dim strrfqid As String
        Dim chk As CheckBox
        Dim dgitem As DataGridItem
        Dim send_rfq_no As String
        For Each dgitem In dtg_VendorList.Items
            chk = dgitem.FindControl("chkSelection2")
            If chk.Checked Then
                If objrfq.send(dgitem.Cells(9).Text) = "2" Then
                    If send_rfq_no = "" Then
                        send_rfq_no = dgitem.Cells(8).Text
                    Else
                        send_rfq_no = send_rfq_no & "," & dgitem.Cells(8).Text
                    End If
                Else
                    If strrfqid = "" Then
                        strrfqid = dgitem.Cells(9).Text
                    Else
                        strrfqid = strrfqid & "," & dgitem.Cells(9).Text
                    End If
                End If
            End If
        Next

        Dim vbs As String
        vbs = vbs & "<script language=""javascript"">"
        vbs = vbs & "window.open('" & dDispatcher.direct("RFQ", "RFQSubmit.aspx", "pageid=" & strPageId & "&rfqsend=" & send_rfq_no & "&rfq_id=" & strrfqid) & "','','width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes')"
        vbs = vbs & "</script>"
        Dim rndKey As New Random
        Me.RegisterStartupScript(rndKey.Next.ToString, vbs)
    End Sub
    'Private Sub ImageButton5_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton5.Click
    '    submition()
    'End Sub

    'Private Sub ImageButton6_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton6.Click
    '    submition()
    'End Sub

    ' ai chu add - print to pdf function
    'Private Sub img_pdf_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles img_pdf.Click
    '    printPDF()
    'End Sub

    'Private Sub printPDF()
    '    Dim dtPrint As New DataTable
    '    Dim dr As DataRow
    '    Session("dtprint") = Nothing

    '    Dim dgItem As DataGridItem
    '    Dim chk As CheckBox
    '    Dim hidRfqId As HtmlInputHidden
    '    Dim lnkDocNo As HyperLink
    '    Dim i, cnt As Integer
    '    Dim ds As New DataSet

    '    dtPrint.Columns.Add("rfqid", Type.GetType("System.String")) '//doc no
    '    dtPrint.Columns.Add("vcomid", Type.GetType("System.String"))
    '    dtPrint.Columns.Add("type", Type.GetType("System.String")) '//type

    '    Select Case ddl_folder.SelectedItem.Value
    '        Case "0"
    '            For Each dgItem In dtg_VendorList.Items
    '                chk = dgItem.FindControl("chkSelection")
    '                hidRfqId = dgItem.FindControl("hidRfqId")
    '                If chk.Checked Then
    '                    ds = objrfq.rfq_COMNAME(hidRfqId.Value, "")
    '                    If ds.Tables(0).Rows.Count > 0 Then
    '                        For i = 0 To ds.Tables(0).Rows.Count - 1
    '                            dr = dtPrint.NewRow
    '                            dr("rfqid") = dgItem.Cells(VenEnum.RFQ_ID).Text
    '                            dr("vcomid") = ds.Tables(0).Rows(i)("RIV_S_Coy_ID")
    '                            dr("type") = "RFQ"
    '                            dtPrint.Rows.Add(dr)
    '                        Next
    '                    End If
    '                End If
    '            Next

    '        Case "2"
    '            For Each dgItem In dtg_Qoute.Items
    '                chk = dgItem.FindControl("chkSelection3")
    '                hidRfqId = dgItem.FindControl("hidRfqId3") ' vendor id
    '                If chk.Checked Then
    '                    dr = dtPrint.NewRow
    '                    dr("rfqid") = dgItem.Cells(QuoEnum.rfqid).Text
    '                    dr("vcomid") = hidRfqId.Value
    '                    dr("type") = "QUO"
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

    'Private Sub img_pdf2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles img_pdf2.Click
    '    printPDF()
    'End Sub

    Sub DisplayExtraFunction()

        Session("strurl") = dDispatcher.direct("RFQ", "RFQ_List.aspx", "pageid=" & strPageId)
        SetGridProperty(Me.dtg_VendorList)
        Bindgrid(Me.dtg_VendorList)

    End Sub

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        goto_trash()
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        submition()
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
        '             "<li><a class=""t_entity_btn_selected"" href=""RFQ_List.aspx?pageid=" & strPageId & """><span>RFQ Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '              "<li><a class=""t_entity_btn"" href=""RFQ_Quote.aspx?pageid=" & strPageId & """><span>Quotation</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '            "</ul><div></div></div>"
        Session("w_CreateRFQ_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "Create_RFQ.aspx", "pageid=" & strPageId) & """><span>Raise RFQ</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "RFQ_Outstg_List.aspx", "pageid=" & strPageId) & """><span>Outstanding RFQ</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("RFQ", "RFQ_List.aspx", "pageid=" & strPageId) & """><span>RFQ Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                      "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "RFQ_Quote.aspx", "pageid=" & strPageId) & """><span>Quotation</span></a></li>" & _
                      "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
    End Sub

    Protected Sub cmdDuplicate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDuplicate.Click
        copy_rfq()
    End Sub

End Class
