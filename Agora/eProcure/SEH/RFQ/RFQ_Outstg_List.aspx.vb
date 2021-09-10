Imports AgoraLegacy
Imports eProcure.Component

Imports System.Drawing

Public Class RFQ_Outstg_List_SEH
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

        '' ''cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        ' '' ''cmdDuplicate.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")

        'If VenName <> "" Then
        '    ds = objRFQ.GetOutstandingRFQListWithVendor(DocNum, VenName)
        'Else
        '    ds = objRFQ.GetOutstandingRFQList(DocNum)
        'End If

        ds = objRFQ.GetOutstandingRFQListWithVendor(DocNum, VenName)

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

    Public Sub dtg_VendorList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        ' viewstate("rfq_id") = ""
        ViewState("total") = 0
        Bindgrid(sender)

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        sender.CurrentPageIndex = 0
        ' viewstate("rfq_id") = ""
        ViewState("total") = 0
        Bindgrid(sender, True)

    End Sub

    Private Sub dtg_VendorList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_VendorList.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(sender, e)

        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If

    End Sub

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
                    STR = STR & "<A href=""" & dDispatcher.direct("RFQ", "Comlist.aspx", "pageid=" & strPageId & "&list_name=" & com_name(i) & "&rfq_id=" & Common.parseNull(dv("RM_RFQ_ID")) & "&rfq_name=" & Server.UrlEncode(e.Item.Cells(VenEnum.RFQ_Name).Text) & "&list_no=" & com_id(i)) & """ ><font color=#0000ff>" & com_name(i) & "</font></A><br>"
                Next
            End If
            If ARRAY(0) <> "" Then
                For i = 0 To COUNT - 1
                    STR = STR & "<A href=""" & dDispatcher.direct("RFQ", "RFQ_VendorDetail.aspx", "Frm=RFQ_Outstg_List&pageid=" & strPageId & "&RFQ_Num=" & Server.UrlEncode(Common.parseNull(dv("RM_RFQ_No"))) & "&v_com_id=" & array2(i)) & """ ><font color=#0000ff>" & ARRAY(i) & "</font></A><br>"
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
            If dv("RM_Status") = "3" Then
                If Common.parseNull(dv("RM_Expiry_Date")) < Date.Today Then
                    Dim strTemp As String = "Draft" & "<BR>" & "Expired"
                    lbl_status.Text = strTemp
                Else
                    lbl_status.Text = "Draft"
                End If

                lbl_rfqnum.Text = "<A href=""" & dDispatcher.direct("RFQ", "Create_RFQ.aspx", "Frm=RFQ_Outstg_List&pageid=" & strPageId & "&page=1&RFQ_Num=" & Server.UrlEncode(Common.parseNull(dv("RM_RFQ_No"))) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID"))) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"
            Else
                If IsDBNull(dv("RM_Expiry_Date")) Then

                Else
                    lbl_rfqnum.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQDetail.aspx", "Frm=RFQ_Outstg_List&pageid=" & strPageId & "&page=1&RFQ_Num=" & Server.UrlEncode(Common.parseNull(dv("RM_RFQ_No"))) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID"))) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"
                    If Common.parseNull(dv("RM_Expiry_Date")) >= Date.Today Then
                        If check = False Then
                            lbl_status.Text = "Sent"
                            ' lbl_rfqnum.Text = "<A href=""RFQDetail.aspx?Frm=RFQ_Outstg_List&pageid=" & strPageId & "&page=1&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"

                        ElseIf check = True Then
                            If dv("RM_RFQ_OPTION") = 0 Then 'open
                                lbl_status.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQComSummary.aspx", "disabled=N&Frm=RFQ_Outstg_List&RFQType=V&pageid=" & strPageId & "&RFQ_Num=" & Server.UrlEncode(Common.parseNull(dv("RM_RFQ_No"))) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Server.UrlEncode(Common.parseNull(dv("RM_RFQ_Name")))) & """ ><font color=#0000ff>View Response</font></A>"
                                'lbl_rfqnum.Text = "<A href=""RFQDetail.aspx?Frm=RFQ_Outstg_List&pageid=" & strPageId & "&page=1&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"

                            ElseIf dv("RM_RFQ_OPTION") = 1 Then  'close
                                'lbl_rfqnum.Text = "<A href=""RFQDetail.aspx?Frm=RFQ_Outstg_List&pageid=" & strPageId & "&page=1&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"
                                lbl_status.Text = "Response"
                            End If
                        End If
                    Else
                        If check = False Then
                            lbl_status.Text = "No Response, Expired"
                        ElseIf check = True Then
                            lbl_status.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQComSummary.aspx", "disabled=N&Frm=RFQ_Outstg_List&pageid=" & strPageId & "&RFQ_Num=" & Server.UrlEncode(Common.parseNull(dv("RM_RFQ_No"))) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_Name=" & Server.UrlEncode(Common.parseNull(dv("RM_RFQ_Name")))) & """ ><font color=#0000ff>View Response</font></A>, Expired"
                        End If
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub cmd_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Search.Click
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
            If chk.Checked Then
                ' If Me.ddl_folder.SelectedItem.Value = "0" Then
                'If strStatus.Text = "Draft" Then
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
                Common.NetMsgbox(Me, strDuplicateName & "is/are successfully duplicated.", dDispatcher.direct("RFQ", "RFQ_Outstg_List.aspx", "pageid=" & strPageId))
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
            chk = dgitem.FindControl("chkSelection")
            If chk.Checked Then
                If objRFQ.send(dgitem.Cells(9).Text) = "2" Then
                    If send_rfq_no = "" Then
                        send_rfq_no = dgitem.Cells(7).Text
                    Else
                        send_rfq_no = send_rfq_no & "," & dgitem.Cells(7).Text
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
        vbs = vbs & "window.open('" & dDispatcher.direct("RFQ", "RFQSubmit.aspx", "pageid=" & strPageId & "&rfqsend=" & send_rfq_no & "&rfq_id=" & strrfqid) & " ','','width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes')"
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
        'If Me.ddl_folder.SelectedItem.Value = "0" Then ' sent
        Session("strurl") = dDispatcher.direct("RFQ", "RFQ_List.aspx", "pageid=" & strPageId) '& "&edit=2"
        SetGridProperty(Me.dtg_VendorList)
        Bindgrid(Me.dtg_VendorList)
        '    dtg_VendorList.Visible = True
        '    dtg_VendorList.Enabled = True
        '    'dtg_Draft.Visible = False
        '    'dtg_Draft.Enabled = False
        '    'dtg_Qoute.Visible = False
        '    'dtg_Qoute.Enabled = False
        '    'dtg_Trash.Visible = False
        '    'dtg_Trash.Enabled = False
        '    'Me.ImageButton5.Visible = False
        '    'Me.ImageButton6.Visible = False
        '    'Me.img_pdf.Visible = True
        '    'Me.img_pdf2.Visible = True
        '    SetGridProperty(Me.dtg_VendorList)
        '    Bindgrid(Me.dtg_VendorList)
        '    'img_pdf.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
        '    'img_pdf2.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
        'ElseIf Me.ddl_folder.SelectedItem.Value = "1" Then 'draft
        '    Session("strurl") = "RFQ_List.aspx?pageid=" & strPageId & "&edit=1"
        '    'Me.img_pdf.Visible = False
        '    'Me.img_pdf2.Visible = False
        '    'Me.ImageButton5.Visible = True
        '    'Me.ImageButton6.Visible = True
        '    dtg_VendorList.Visible = False
        '    dtg_VendorList.Enabled = False
        '    'dtg_Draft.Visible = True
        '    'dtg_Draft.Enabled = True
        '    'dtg_Qoute.Visible = False
        '    'dtg_Qoute.Enabled = False
        '    'dtg_Trash.Visible = False
        '    'dtg_Trash.Enabled = False
        '    'SetGridProperty(Me.dtg_Draft)
        '    'Bindgrid(Me.dtg_Draft)
        'ElseIf Me.ddl_folder.SelectedItem.Value = "2" Then 'inbox
        '    Session("strurl") = "RFQ_List.aspx?pageid=" & strPageId & "&edit=3"
        '    'Me.img_pdf.Visible = True
        '    'Me.img_pdf2.Visible = True
        '    'Me.ImageButton5.Visible = False
        '    'Me.ImageButton6.Visible = False
        '    dtg_VendorList.Visible = False
        '    dtg_VendorList.Enabled = False
        '    'dtg_Draft.Visible = False
        '    'dtg_Draft.Enabled = False
        '    'dtg_Qoute.Visible = True
        '    'dtg_Qoute.Enabled = True
        '    'dtg_Trash.Visible = False
        '    'dtg_Trash.Enabled = False
        '    'SetGridProperty(Me.dtg_Qoute)
        '    'Bindgrid(Me.dtg_Qoute)
        '    'img_pdf.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection3');")
        '    'img_pdf2.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection3');")
        'ElseIf Me.ddl_folder.SelectedItem.Value = "3" Then 'Trash
        '    Session("strurl") = "RFQ_List.aspx?pageid=" & strPageId & "&edit=4"
        '    'Me.img_pdf.Visible = False
        '    'Me.img_pdf2.Visible = False
        '    'Me.ImageButton5.Visible = False
        '    'Me.ImageButton6.Visible = False
        '    dtg_VendorList.Visible = False
        '    dtg_VendorList.Enabled = False
        '    'dtg_Draft.Visible = False
        '    'dtg_Draft.Enabled = False
        '    'dtg_Qoute.Visible = False
        '    'dtg_Qoute.Enabled = False
        '    'dtg_Trash.Visible = True
        '    'dtg_Trash.Enabled = True
        '    'SetGridProperty(Me.dtg_Trash)
        '    'Bindgrid(Me.dtg_Trash)
        '    'img_pdf.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection4');")
        'End If
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        goto_trash()
    End Sub

    'Private Sub cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
    '    submition()
    'End Sub

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
        '             "<li><a class=""t_entity_btn_selected"" href=""RFQ_Outstg_List.aspx?pageid=" & strPageId & """><span>Outstanding RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""RFQ_List.aspx?pageid=" & strPageId & """><span>RFQ Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '              "<li><a class=""t_entity_btn"" href=""RFQ_Quote.aspx?pageid=" & strPageId & """><span>Quotation</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '            "</ul><div></div></div>"
        Session("w_CreateRFQ_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "Create_RFQ.aspx", "pageid=" & strPageId) & """><span>Raise RFQ</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("RFQ", "RFQ_Outstg_List.aspx", "pageid=" & strPageId) & """><span>Outstanding RFQ</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "RFQ_List.aspx", "pageid=" & strPageId) & """><span>RFQ Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "RFQ_Quote.aspx", "pageid=" & strPageId) & """><span>Quotation</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                   "</ul><div></div></div>"
    End Sub

    Protected Sub cmdDuplicate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDuplicate.Click
        copy_rfq()
    End Sub

End Class