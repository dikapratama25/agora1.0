Imports AgoraLegacy
Imports eProcure.Component

Public Class RFQSearch
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
    Protected WithEvents ddl_folder As System.Web.UI.WebControls.DropDownList
    Protected WithEvents dtg_quote As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lbl_disc As System.Web.UI.WebControls.Label
    Protected WithEvents hidAddItem As System.Web.UI.HtmlControls.HtmlInputHidden

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Dim strFrm As String

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
        cmdDelete.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdDelete)
       
        htPageAccess.Add("delete", alButtonList)

        If intPageRecordCnt > 0 Then
            cmdDelete.Enabled = blnCanDelete
         
        Else
            cmdDelete.Enabled = False
          
        End If

        CheckButtonAccess()
        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        
        If Not Page.IsPostBack Then
            GenerateTab()
            Me.cmdDelete.Attributes.Add("onclick", "return cmdAddClick();")
            SetGridProperty(Me.dtg_quote)
            dtg_quote.CurrentPageIndex = 0
            ViewState("SortAscendingOutstandingRFQ") = "no"
            ViewState("SortExpressionOutstandingRFQ") = "Creation Date"
            ViewState("SortAscending") = "no"
            ViewState("SortExpression") = "RM_Created_On"
            Bindgrid(dtg_quote)
        End If
    End Sub

    Sub dtg_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dtg_quote.CurrentPageIndex = e.NewPageIndex
        Bindgrid(dtg_quote)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        sender.CurrentPageIndex = 0
        Bindgrid(sender, True)
    End Sub

    Private Function Bindgrid(ByVal dg_id As DataGrid, Optional ByVal pSorted As Boolean = False) As String

        Dim ds As New DataSet
        Dim com_name As String
        Dim docnum As String = txt_Num.Text
        Dim v_display As Integer = 0
        Dim V_RFQ_Status As Integer = 0
        com_name = Me.txt_com_name.Text

        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection2','delete');")
        'ds = objrfq.get_buyerInfo2(docnum, com_name, v_display, V_RFQ_Status)
        'ds = objrfq.get_qoute(docnum, com_name, v_display, V_RFQ_Status)
        ds = objrfq.get_RFQ(docnum, com_name, v_display, V_RFQ_Status)

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
            'intTotPage = 0
        End If
        ViewState("PageCount") = dg_id.PageCount
    End Function

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
        Dim lblQuo As Label

        dg_tem = Me.dtg_quote
        j = 8

        For Each dgItem In dg_tem.Items
            Dim dv As DataRowView = CType(dgItem.DataItem, DataRowView)
            objval.RFQ_ID = dgItem.Cells(j).Text

            chkItem = dgItem.FindControl("chkSelection2")
            lblQuo = dgItem.FindControl("lbl_quo") ' dgItem.FindControl("lbl_quo")

            If chkItem.Checked Then
                If lblQuo.ToolTip = "" Then
                    strSQL = objrfq.Vendor_add2trash(objval, "0", DOCTYPE)

                Else
                    strSQL = objrfq.Vendor_add2trash(objval, "1", DOCTYPE)
                End If
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
            e.Item.Cells(QuoEnum.CreOn).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("RM_Created_On"))
            e.Item.Cells(QuoEnum.Offer).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("RRM_Offer_Till"))

            Dim lbl_quo As Label
            lbl_quo = e.Item.FindControl("lbl_quo")
            lbl_quo.Text = "<A href=""" & dDispatcher.direct("RFQ", "ViewQoute.aspx", "Frm=RFQSearch&pageid=" & strPageId & " &qou_num=" & Common.parseNull(dv("RRM_Actual_Quot_Num")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&vcomid=" & Session("CompanyID")) & "  "" ><font color=#0000ff>" & Common.parseNull(dv("RRM_Actual_Quot_Num")) & "</font></A>"
            lbl_quo.ToolTip = Common.parseNull(dv("RRM_Actual_Quot_Num"))

            Dim lbl_status As Label
            lbl_status = e.Item.FindControl("lbl_status1")
            If Not IsDBNull(dv("RVM_V_RFQ_Status")) Then
                If dv("RM_Expiry_Date") >= Now.Today Then
                    If dv("RVM_V_RFQ_Status") = "0" Then
                        lbl_status.Text = "New, <A href=""" & dDispatcher.direct("RFQ", "CreateQuotationNew.aspx", "Frm=RFQSearch&pageid=" & strPageId & " &bcomid=" & dv("RM_Coy_ID") & "&RFQ_No=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID"))) & " "" ><font color=#0000ff>Reply</font></A>"
                    ElseIf dv("RVM_V_RFQ_Status") = "1" Then
                        If dv("RM_B_DISPLAY_STATUS") = "0" Then
                            lbl_status.Text = "Replied, <A href=""" & dDispatcher.direct("RFQ", "CreateQuotationNew.aspx", "Frm=RFQSearch&pageid=" & strPageId & " &bcomid=" & dv("RM_Coy_ID") & "&RFQ_No=" & Common.parseNull(dv("RM_RFQ_No")) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&edit=1") & " "" ><font color=#0000ff>Resubmit</font></A>"
                        Else
                            lbl_status.Text = "Deleted, Replied"
                        End If


                    End If
                Else
                    If dv("RVM_V_RFQ_Status") = "0" Then
                        lbl_status.Text = "Expired"
                    ElseIf dv("RVM_V_RFQ_Status") = "1" Then
                        'lbl_status.Text = "Expired, <A  href=""" & dDispatcher.direct("RFQ", "ViewQoute.aspx", "Frm=RFQSearch&pageid=" & strPageId & " &RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&RFQ_No=" & Common.parseNull(dv("RM_RFQ_No")) & "&vcomid=" & Session("CompanyID")) & " "" ><font color=#0000ff>Replied</font></A>"

                        If dv("RM_B_DISPLAY_STATUS") = "0" Then
                            lbl_status.Text = "Expired, Replied"
                        Else
                            lbl_status.Text = "Expired, Deleted, Replied"
                        End If
                    End If
                    'If Common.parseNull(dv("RM_Status")) = "1" Then
                    '    lbl_status.Text = "Sent"
                    'ElseIf Common.parseNull(dv("RM_Status")) = "2" Then
                    '    If objrfq.check_po(Common.parseNull(dv("RM_RFQ_ID"))) = True Then
                    '        lbl_status.Text = "Sent, PO created"
                    '    Else
                    '        lbl_status.Text = "Sent"
                    '    End If
                    'End If
                End If
            End If

            If objrfq.HasAttachment2(Common.parseNull(Common.parseNull(dv("RM_RFQ_No"))), objrfq.get_comid(dv("RM_RFQ_ID")), "E") Then
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
            If dv("RM_B_DISPLAY_STATUS") = "0" Then
                lbl_RFQ_Num.Text = "<A href=""" & dDispatcher.direct("RFQ", "RFQDetail.aspx", "Frm=RFQSearch&pageid=" & strPageId & "&page=1&goto=2&RFQ_Num=" & Common.parseNull(Common.parseNull(dv("RM_RFQ_No"))) & "&RFQ_ID=" & Common.parseNull(dv("RM_RFQ_ID")) & "&vcom_id=" & Session("CompanyId")) & " "" ><font color=#0000ff>" & Common.parseNull(dv("RM_RFQ_No")) & "</font></A>"
            Else
                lbl_RFQ_Num.Text = Common.parseNull(Common.parseNull(dv("RM_RFQ_No")))
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/Images", "Deleted RFQ Icon.gif")
                e.Item.Cells(2).Controls.Add(imgAttach)
            End If
        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                        "<li><a class=""t_entity_btn"" href=""VendorRFQList.aspx?pageid=" & strPageId & """><span>Outstanding RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                        "<li><a class=""t_entity_btn_selected"" href=""RFQSearch.aspx?pageid=" & strPageId & """><span>Quotation Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                        "</ul><div></div></div>"
        Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "VendorRFQList.aspx", "pageid=" & strPageId) & """><span>Outstanding RFQ</span></a>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "VendorRFQListExp.aspx", "pageid=" & strPageId) & """><span>Expired / Rejected RFQ</span></a>" & _
                                "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""#""><span>Quotation Listing</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                                "</ul><div></div></div>"
    End Sub

    Protected Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        goto_trash()
    End Sub

    Private Sub cmd_Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Search.Click
        dtg_quote.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "RM_Created_On"
        SetGridProperty(Me.dtg_quote)
        Bindgrid(dtg_quote)
    End Sub

    Private Sub cmd_clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_clear.Click
        Me.txt_Num.Text = ""
        Me.txt_com_name.Text = ""
    End Sub
End Class