Imports AgoraLegacy
Imports eProcure.Component

Public Class VendorRFQList1
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmd_Search As System.Web.UI.WebControls.Button
    Protected WithEvents lblCurrentIndex As System.Web.UI.WebControls.Label
    Protected WithEvents txt_Num As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_com_name As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmd_clear As System.Web.UI.WebControls.Button
    Dim objrfq As New RFQ
    Protected WithEvents dtg_VendorList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ddl_folder As System.Web.UI.WebControls.DropDownList
    Protected WithEvents dtg_quote As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtg_trash As System.Web.UI.WebControls.DataGrid
    Protected WithEvents img_delete As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmd_emp_trash As System.Web.UI.WebControls.Button
    Protected WithEvents lbl_title As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_disc As System.Web.UI.WebControls.Label
    Protected WithEvents Imagebutton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents img_pdf As System.Web.UI.WebControls.ImageButton
    Protected WithEvents img_pdf2 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents hidAddItem As System.Web.UI.HtmlControls.HtmlInputHidden

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Enum VendorDisStatus
        Deletetrash = 2
        trash = 1
        sent = 0

    End Enum
    Public Enum VenEnum
        Chk = 0
        RfqNo = 1
        RfqName = 2
        CreOn = 3
        ExpDate = 4
        CoyName = 5
        Status = 6
        RfqID = 7

    End Enum
    Public Enum QuoEnum
        Chk = 0
        ActQuoNum = 1
        RfqNo = 2
        RfqName = 3
        CreOn = 4
        Offer = 5
        CoyName = 6
        Status = 7
        RfqID = 8
    End Enum
    Public Enum TraEnum
        Chk = 0
        RfqNo = 1
        CreOn = 2
        ExpDate = 3
        CoyName = 4
        Status = 5
        CHECKDOC = 6
        RfqID = 7
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        img_delete.Enabled = False
        Imagebutton1.Enabled = False
        cmd_emp_trash.Enabled = False

        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(img_delete)
        alButtonList.Add(Imagebutton1)
        alButtonList.Add(cmd_emp_trash)
        htPageAccess.Add("delete", alButtonList)

        If intPageRecordCnt > 0 Then
            img_delete.Enabled = blnCanDelete
            Imagebutton1.Enabled = blnCanDelete
            cmd_emp_trash.Enabled = blnCanDelete
        Else
            img_delete.Enabled = False
            Imagebutton1.Enabled = False
            cmd_emp_trash.Enabled = False
        End If

        CheckButtonAccess()
        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        'Put user code to initialize the page here
        Dim objrfq As New RFQ
        MyBase.Page_Load(sender, e)
        blnPaging = True
        blnSorting = True
        If Not Page.IsPostBack Then
            ' ai chu add on 19/10/2005
            ' need to clear all session for quotation header detail
            Session("con_person") = ""
            Session("con_num") = ""
            Session("email") = ""
            Session("validity") = ""
            Session("shipterm") = ""
            Session("shipmode") = ""
            Session("BackToStep1") = False

            Me.cmd_emp_trash.Attributes.Add("onclick", "return cmdAddClick();")
            ViewState("SortAscending") = "no"
            If Request(Trim("goto")) = "" Then
                dtg_quote.Visible = False
                Me.dtg_trash.Visible = False
                dtg_VendorList.Visible = True
                SetGridProperty(Me.dtg_VendorList)
                ViewState("SortExpression") = "RM_Created_On"
                Bindgrid(dtg_VendorList)
                Session("strurl") = dDispatcher.direct("RFQ", "VendorRFQList.aspx", "&pageid=" & strPageId & "&goto=1")
            Else
                ddl_folder.SelectedIndex = Request(Trim("goto")) - 1
                If Session("strurl") = "" Then Session("strurl") = dDispatcher.direct("RFQ", "VendorRFQList.aspx", "&pageid=" & strPageId & "&goto=" & Request(Trim("goto")))
                If Request(Trim("goto")) = "1" Then
                    Me.img_pdf.Visible = True
                    Me.img_pdf2.Visible = True
                    dtg_quote.Visible = False
                    Me.dtg_trash.Visible = False
                    dtg_VendorList.Visible = True
                    SetGridProperty(Me.dtg_VendorList)
                    ViewState("SortExpression") = "RM_Created_On"
                    Bindgrid(dtg_VendorList)
                ElseIf Request(Trim("goto")) = "2" Then

                    Me.img_pdf.Visible = True
                    Me.img_pdf2.Visible = True
                    dtg_quote.Visible = True
                    Me.dtg_trash.Visible = False
                    dtg_VendorList.Visible = False
                    Common.SelDdl("1", Me.ddl_folder)
                    SetGridProperty(Me.dtg_quote)
                    ViewState("SortExpression") = "RRM_Created_On"
                    Bindgrid(dtg_quote)
                ElseIf Request(Trim("goto")) = "3" Then
                    Me.img_pdf.Visible = False
                    Me.img_pdf2.Visible = False
                    Common.SelDdl("2", Me.ddl_folder)
                    dtg_quote.Visible = False
                    Me.dtg_trash.Visible = True
                    dtg_VendorList.Visible = False
                    Common.SelDdl("2", Me.ddl_folder)
                    SetGridProperty(Me.dtg_trash)
                    ViewState("SortExpression") = "RM_Created_On"
                    Bindgrid(dtg_trash)
                End If
            End If
        End If
        img_delete.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete.gif")
        img_pdf.ImageUrl = dDispatcher.direct("Plugins/images", "pdf.bmp")
        Imagebutton1.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete.gif")
        img_pdf2.ImageUrl = dDispatcher.direct("Plugins/images", "pdf.bmp")

    End Sub

    Private Function Bindgrid(ByVal dg_id As DataGrid, Optional ByVal pSorted As Boolean = False) As String

        Dim ds As New DataSet
        Dim com_name As String
        Dim docnum As String = txt_Num.Text
        Dim v_display As Integer = 0
        Dim V_RFQ_Status As Integer = 0
        com_name = Me.txt_com_name.Text

        If Me.ddl_folder.SelectedItem.Value = "0" Then
            Imagebutton1.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
            img_delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")

            Me.lbl_title.Text = "RFQ Inbox Folder"
            cmd_emp_trash.Visible = False
            ds = objrfq.get_buyerInfo(docnum, com_name, v_display, V_RFQ_Status)
            Me.lbl_disc.Text = "This list provides a summary of the quotation activity between you and your Buyer. You can respond to a new RFQ or go to your sent folder to view previously quoted items. Click on a RFQ Number to view it. To reply, click on the reply link in the status column."
        ElseIf Me.ddl_folder.SelectedItem.Value = "1" Then
            Imagebutton1.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection2','delete');")
            img_delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection2','delete');")

            cmd_emp_trash.Visible = False
            ds = objrfq.get_qoute(docnum, com_name, v_display, V_RFQ_Status)
            Me.lbl_title.Text = "RFQ Sent Folder"
            Me.lbl_disc.Text = "All quotations submitted are listed in this folder."
        ElseIf Me.ddl_folder.SelectedItem.Value = "2" Then
            Imagebutton1.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection3','delete');")
            img_delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection3','delete');")
            cmd_emp_trash.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection3','delete');")

            'cmd_emp_trash.Visible = True
            Me.lbl_title.Text = "RFQ Trash Folder"
            Me.lbl_disc.Text = "To remove items permanently from this folder, select the item and click on the trash icon."
            ds = objrfq.get_trash(docnum, com_name, v_display, V_RFQ_Status)
        End If

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)
        'If pSorted Then
        dvViewSample.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" And viewstate("SortExpression") <> "" Then dvViewSample.Sort += " DESC"
        'End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If dg_id.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dg_id.PageSize = 0 Then
                dg_id.CurrentPageIndex = dg_id.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        viewstate("intPageRecordCnt") = intPageRecordCnt

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dg_id, dvViewSample)
            dg_id.DataSource = dvViewSample
            dg_id.DataBind()
            If Me.ddl_folder.SelectedItem.Value = "2" Then
                cmd_emp_trash.Visible = True
            End If
        Else
            dg_id.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            'intTotPage = 0
            If Me.ddl_folder.SelectedItem.Value = "2" Then
                cmd_emp_trash.Visible = False
            End If
        End If
        viewstate("PageCount") = dg_id.PageCount
    End Function

    Public Sub dtg_VendorList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        Bindgrid(sender)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        sender.CurrentPageIndex = 0
        Bindgrid(sender, True)
    End Sub

    Private Sub dtg_VendorList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_VendorList.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtg_VendorList, e)

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
            e.Item.Cells(VenEnum.CreOn).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("RM_Created_On"))

            e.Item.Cells(VenEnum.ExpDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("RM_Expiry_Date"))
            Dim lbl_rfqnum As Label
            lbl_rfqnum = e.Item.FindControl("lbl_rfqnum")
            lbl_rfqnum.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQDetail.aspx", "pageid=" & strPageId & "&page=1&goto=1&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&vcom_id=" & Session("CompanyId")) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"

            If objrfq.HasAttachment2(Common.parseNull(Common.parseNull(dv("RM_RFQ_No"))), objrfq.get_comid(dv("RM_RFQ_ID"))) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "clip_icon.gif")
                e.Item.Cells(VenEnum.RfqNo).Controls.Add(imgAttach)
            End If

            'add by Moo
            '//CreateQuotation.aspx - add RFQ_Id
            Dim lbl_status As Label
            lbl_status = e.Item.FindControl("lbl_status")
            If Not IsDBNull(dv("RVM_V_RFQ_Status")) Then

                If dv("RM_Expiry_Date") >= Now.Today Then
                    If dv("RVM_V_RFQ_Status") = "0" Then
                        lbl_status.Text = "New, <A href=""" & dDispatcher.direct("RFQ", "CreateQuotation.aspx", "pageid=" & strPageId & " &bcomid=" & dv("RM_Coy_ID") & "&RFQ_No=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID"))) & " "" ><font color=#0000ff>Reply</font></A>"
                    ElseIf dv("RVM_V_RFQ_Status") = "1" Then
                        lbl_status.Text = "Replied, <A href=""" & dDispatcher.direct("RFQ", "CreateQuotation.aspx", "pageid=" & strPageId & " &bcomid=" & dv("RM_Coy_ID") & "&RFQ_No=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&edit=1 ") & " "" ><font color=#0000ff>Resubmit</font></A>"

                    End If
                Else
                    If dv("RVM_V_RFQ_Status") = "0" Then
                        lbl_status.Text = "Expired"
                    ElseIf dv("RVM_V_RFQ_Status") = "1" Then
                        lbl_status.Text = "Expired, <A  href=""" & dDispatcher.direct("RFQ", "ViewQoute.aspx", "pageid=" & strPageId & " &RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & " &vcomid=" & Session("CompanyID")) & " "" ><font color=#0000ff>Replied</font></A>"
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub dtg_quote_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_quote.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtg_quote, e)

        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll2 As CheckBox = e.Item.FindControl("chkAll2")
            chkAll2.Attributes.Add("onclick", "selectAll2();")
        End If
    End Sub

    Private Sub dtg_quote_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_quote.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk2 As CheckBox
            chk2 = e.Item.Cells(QuoEnum.Chk).FindControl("chkSelection2")
            chk2.Attributes.Add("onclick", "checkChild2('" & chk2.ClientID & "')")
            e.Item.Cells(QuoEnum.CreOn).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("RRM_Created_On"))

            e.Item.Cells(QuoEnum.Offer).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("RRM_Offer_Till"))
            Dim lbl_quo As Label
            lbl_quo = e.Item.FindControl("lbl_quo")
            lbl_quo.Text = "<A href=""" & dDispatcher.direct("RFQ", "ViewQoute.aspx", "pageid=" & strPageId & " &qou_num=" & Common.parseNull(dv("RRM_Actual_Quot_Num")) & "&RFQ_ID=" & Common.parseNull(dv("RRM_RFQ_ID")) & "&vcomid=" & Session("CompanyID")) & "  "" ><font color=#0000ff>" & Common.parseNull(dv("RRM_Actual_Quot_Num")) & "</font></A>"

            Dim lbl_status As Label
            lbl_status = e.Item.FindControl("lbl_status1")
            If Common.parseNull(dv("RM_Status")) = "1" Then
                lbl_status.Text = "Sent"
            ElseIf Common.parseNull(dv("RM_Status")) = "2" Then
                If objrfq.check_po(Common.parseNull(dv("RM_RFQ_ID"))) = True Then
                    lbl_status.Text = "Sent, PO created"
                Else
                    lbl_status.Text = "Sent"
                End If

            End If

            If objrfq.HasAttachment2(Common.parseNull(Common.parseNull(dv("RM_RFQ_No"))), objrfq.get_comid(dv("RM_RFQ_ID"))) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")
                e.Item.Cells(QuoEnum.RfqNo).Controls.Add(imgAttach)
            End If

            If objrfq.HasAttachmentQuote(Common.parseNull(Common.parseNull(dv("RM_RFQ_No"))), Session("CompanyID")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")
                e.Item.Cells(QuoEnum.ActQuoNum).Controls.Add(imgAttach)

            End If
            Dim lbl_RFQ_Num As Label
            lbl_RFQ_Num = e.Item.FindControl("lbl_RFQ_Num")
            lbl_RFQ_Num.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQDetail.aspx", "pageid=" & strPageId & "&page=1&goto=2&RFQ_Num=" & Common.parseNull(Common.parseNull(dv("RM_RFQ_No"))) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&vcom_id=" & Session("CompanyId")) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"
        End If
    End Sub

    Private Sub dtg_trash_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_trash.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk3 As CheckBox
            chk3 = e.Item.Cells(TraEnum.Chk).FindControl("chkSelection3")
            chk3.Attributes.Add("onclick", "checkChild3('" & chk3.ClientID & "')")

            Dim Label1 As Label
            Label1 = e.Item.FindControl("Label1")
            Label1.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQDetail.aspx", "pageid=" & strPageId & "&page=1&goto=3&RFQ_Num=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&vcom_id" & Session("CompanyId")) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"

            e.Item.Cells(TraEnum.RfqID).Text = dv("RM_RFQ_ID")

            e.Item.Cells(TraEnum.CreOn).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("RM_Created_On"))
            e.Item.Cells(TraEnum.ExpDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("RM_Expiry_Date"))

            If objrfq.HasAttachment2(Common.parseNull(dv("RM_RFQ_No")), objrfq.get_comid(dv("RM_RFQ_ID"))) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")
                e.Item.Cells(TraEnum.RfqNo).Controls.Add(imgAttach)

            ElseIf objrfq.HasAttachmentQuote(Common.parseNull(dv("RM_RFQ_No")), Session("CompanyID")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")
                e.Item.Cells(TraEnum.RfqNo).Controls.Add(imgAttach)
            End If

            Dim lbl_status As Label
            lbl_status = e.Item.FindControl("lbl_status2")
            Dim prefix As String
            Dim doctype As String

            '//Remark By MOO
            'prefix = Common.parseNull(dv("RM_Prefix"))            
            'doctype = objrfq.get_doctype(prefix)
            doctype = Common.parseNull(dv("DocType"))

            If doctype.ToUpper = "RFQ" Then
                If Common.parseNull(dv("RM_Expiry_Date")) <= Now.Today Then
                    If Common.parseNull(dv("RVM_V_RFQ_Status")) = "0" Then
                        lbl_status.Text = "Not Replied"
                    ElseIf Common.parseNull(dv("RVM_V_RFQ_Status")) = "1" Then
                        lbl_status.Text = "Replied"
                    End If
                Else
                    If Common.parseNull(dv("RVM_V_RFQ_Status")) = "0" Then
                        lbl_status.Text = "Expired,Not Replied"
                    ElseIf Common.parseNull(dv("RVM_V_RFQ_Status")) = "1" Then
                        lbl_status.Text = "Expired, Replied"
                    End If
                End If
                e.Item.Cells(TraEnum.CHECKDOC).Text = "RFQ"
            ElseIf doctype.ToUpper = "Quotation".ToUpper Then
                e.Item.Cells(TraEnum.CHECKDOC).Text = "QOUTE"
                lbl_status.Text = "Sent"
            End If
        End If
    End Sub

    Private Sub dtg_trash_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_trash.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtg_trash, e)

        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll3 As CheckBox = e.Item.FindControl("chkAll3")
            chkAll3.Attributes.Add("onclick", "selectAll3();")
        End If
    End Sub

    Private Sub img_delete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles img_delete.Click
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

        If Me.ddl_folder.SelectedItem.Value = "0" Then
            dg_tem = Me.dtg_VendorList
            j = 7
        ElseIf Me.ddl_folder.SelectedItem.Value = "1" Then
            dg_tem = Me.dtg_quote
            j = 8
        ElseIf Me.ddl_folder.SelectedItem.Value = "2" Then
            dg_tem = Me.dtg_trash
            j = 7
        End If

        For Each dgItem In dg_tem.Items
            Dim dv As DataRowView = CType(dgItem.DataItem, DataRowView)
            objval.RFQ_ID = dgItem.Cells(j).Text
            If Me.ddl_folder.SelectedItem.Value = "0" Then
                chkItem = dgItem.FindControl("chkSelection")
            ElseIf Me.ddl_folder.SelectedItem.Value = "1" Then
                chkItem = dgItem.FindControl("chkSelection2")
            ElseIf Me.ddl_folder.SelectedItem.Value = "2" Then
                chkItem = dgItem.FindControl("chkSelection3")
                DOCTYPE = dgItem.Cells(6).Text
            End If

            If chkItem.Checked Then
                strSQL = objrfq.Vendor_add2trash(objval, ddl_folder.SelectedItem.Value, DOCTYPE)
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
        Bindgrid(dg_tem)
    End Function

    Private Sub ddl_folder_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddl_folder.SelectedIndexChanged
        ViewState("SortAscending") = "no"
        If Me.ddl_folder.SelectedItem.Value = "0" Then
            ViewState("SortExpression") = "RM_Created_On"
            Session("strurl") = dDispatcher.direct("RFQ", "VendorRFQList.aspx", "&pageid=" & strPageId & "&goto=1")
            SetGridProperty(Me.dtg_VendorList)
            dtg_quote.Visible = False
            Me.dtg_trash.Visible = False
            dtg_VendorList.Visible = True
            Bindgrid(dtg_VendorList)
        ElseIf Me.ddl_folder.SelectedItem.Value = "1" Then
            ViewState("SortExpression") = "RRM_Created_On"
            Session("strurl") = dDispatcher.direct("RFQ", "VendorRFQList.aspx", "&pageid=" & strPageId & "&goto=2")
            SetGridProperty(Me.dtg_quote)
            dtg_VendorList.Visible = False
            Me.dtg_trash.Visible = False
            dtg_quote.Visible = True
            Bindgrid(dtg_quote)
        ElseIf Me.ddl_folder.SelectedItem.Value = "2" Then
            ViewState("SortExpression") = "RM_Created_On"
            Session("strurl") = dDispatcher.direct("RFQ", "VendorRFQList.aspx", "&pageid=" & strPageId & "&goto=3")
            SetGridProperty(Me.dtg_trash)
            dtg_VendorList.Visible = False
            Me.dtg_trash.Visible = True
            dtg_quote.Visible = False
            Bindgrid(dtg_trash)
        End If
    End Sub

    Private Sub cmd_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Search.Click
        ViewState("SortAscending") = "no"
        If Me.ddl_folder.SelectedItem.Value = "0" Then
            ViewState("SortExpression") = "RM_Created_On"
            SetGridProperty(Me.dtg_VendorList)
            dtg_quote.Visible = False
            Me.dtg_trash.Visible = False
            dtg_VendorList.Visible = True
            Bindgrid(dtg_VendorList)
        ElseIf Me.ddl_folder.SelectedItem.Value = "1" Then
            ViewState("SortExpression") = "RRM_Created_On"
            SetGridProperty(Me.dtg_quote)
            dtg_VendorList.Visible = False
            Me.dtg_trash.Visible = False
            dtg_quote.Visible = True
            Bindgrid(dtg_quote)
        ElseIf Me.ddl_folder.SelectedItem.Value = "2" Then
            ViewState("SortExpression") = "RM_Created_On"
            SetGridProperty(Me.dtg_trash)
            dtg_VendorList.Visible = False
            Me.dtg_trash.Visible = True
            dtg_quote.Visible = False
            Bindgrid(dtg_trash)
        End If
    End Sub

    Private Sub cmd_clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_clear.Click
        Me.txt_Num.Text = ""
        Me.txt_com_name.Text = ""
    End Sub

    Private Sub cmd_emp_trash_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_emp_trash.Click

        If hidAddItem.Value = "1" Then
        ElseIf hidAddItem.Value = "0" Then
            Exit Sub
        End If

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
        dg_tem = Me.dtg_trash

        j = 7

        For Each dgItem In dg_tem.Items
            Dim dv As DataRowView = CType(dgItem.DataItem, DataRowView)
            objval.RFQ_ID = dgItem.Cells(j).Text
            DOCTYPE = dgItem.Cells(6).Text

            strSQL = objrfq.Vendor_add2trash(objval, ddl_folder.SelectedItem.Value, DOCTYPE)
            If strSQL <> "" Then
                Common.Insert2Ary(strAryQuery, strSQL)
            End If

            i = i + 1
        Next

        objDB.BatchExecute(strAryQuery)

        If dtg_trash.CurrentPageIndex <> 0 Then
            dtg_trash.CurrentPageIndex = dtg_trash.CurrentPageIndex - 1
        End If
        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        Bindgrid(dtg_trash)
    End Sub

    Private Sub Imagebutton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Imagebutton1.Click
        goto_trash()
    End Sub

    Private Sub img_pdf_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles img_pdf.Click
        printPDF()
    End Sub

    Private Sub img_pdf2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles img_pdf2.Click
        printPDF()
    End Sub

    Private Sub printPDF()
        Dim dtPrint As New DataTable
        Dim dr As DataRow
        Session("dtprint") = Nothing

        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim lnkDocNo As HyperLink
        Dim i, cnt As Integer
        Dim ds As New DataSet
        Dim strType As String

        dtPrint.Columns.Add("rfqid", Type.GetType("System.String")) '//doc no
        dtPrint.Columns.Add("vcomid", Type.GetType("System.String"))
        dtPrint.Columns.Add("type", Type.GetType("System.String")) '//type

        Select Case ddl_folder.SelectedItem.Value
            Case "0"
                For Each dgItem In dtg_VendorList.Items
                    chk = dgItem.FindControl("chkSelection")
                    If chk.Checked Then
                        dr = dtPrint.NewRow
                        dr("rfqid") = dgItem.Cells(VenEnum.RfqID).Text
                        dr("vcomid") = Session("CompanyId")
                        dr("type") = "RFQ"
                        dtPrint.Rows.Add(dr)
                    End If
                Next

            Case "1"
                For Each dgItem In dtg_quote.Items
                    chk = dgItem.FindControl("chkSelection2")
                    If chk.Checked Then
                        dr = dtPrint.NewRow
                        dr("rfqid") = dgItem.Cells(QuoEnum.RfqID).Text
                        dr("vcomid") = Session("CompanyId")
                        dr("type") = "QUO"
                        dtPrint.Rows.Add(dr)
                    End If
                Next

            Case "2"
                For Each dgItem In dtg_trash.Items
                    chk = dgItem.FindControl("chkSelection3")
                    If chk.Checked Then
                        strType = dgItem.Cells(TraEnum.CHECKDOC).Text
                        dr = dtPrint.NewRow
                        dr("rfqid") = dgItem.Cells(TraEnum.RfqID).Text
                        dr("vcomid") = Session("CompanyId")
                        Select Case strType
                            Case "RFQ"
                                dr("type") = "RFQ"
                            Case "QOUTE"
                                dr("type") = "QUO"
                        End Select
                        dtPrint.Rows.Add(dr)
                    End If
                Next
        End Select

        If dtPrint.Rows.Count > 0 Then
            Session("dtprint") = dtPrint
            Dim vbs As String
            vbs = vbs & "<script language=""javascript"">"
            vbs = vbs & vbLf & "PDFWindow('" & strPageId & "');"
            vbs = vbs & "</script>"
            If (Not Me.IsStartupScriptRegistered("pdfwindow")) Then
                Me.RegisterStartupScript("pdfwindow", vbs)
            End If
        End If
    End Sub
End Class
