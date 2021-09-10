Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Public Class RFQDetailFTN
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lbl_exp As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_req_qout As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_cur As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_pt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Con_person As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_pm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_con_num As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_sm As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_email As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_st As System.Web.UI.WebControls.Label
    Protected WithEvents txt_remark As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Name As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Num As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label

    Protected WithEvents dtg_rfqdetail As System.Web.UI.WebControls.DataGrid
    Protected WithEvents pnlAttach2 As System.Web.UI.WebControls.Panel
    Protected WithEvents cmd_pre As System.Web.UI.HtmlControls.HtmlAnchor
    Dim objFile As New FileManagement
    Protected WithEvents cmdPreview As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblExtFile As System.Web.UI.WebControls.Label
    Protected WithEvents lblRemark As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim objrfq As New RFQ
    Dim objval As New RFQ_User
    Dim i As Integer
    Dim strFrm As String
    Protected WithEvents A1 As System.Web.UI.HtmlControls.HtmlAnchor

    Public Enum RfqEnum
        No = 0
        Desc = 1
        UOM = 2
        Quantity = 3
        Time = 4
        Warranty = 5
    End Enum
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' cmd_pre2 = false rfq buyer side 
        ' cmd_pre=true  history.back can use in both side 
        MyBase.Page_Load(sender, e)
        ' for enable viewstate = false
        blnPaging = False
        blnSorting = False
        SetGridProperty(dtg_rfqdetail)
        strFrm = Me.Request.QueryString("Frm")
        'If strFrm = "RFQSearch" Then
        '    lnkBack.NavigateUrl = "RFQSearch.aspx?&pageid=" & strPageId
        'ElseIf strFrm = "RFQ_Outstg_List" Then
        '    lnkBack.NavigateUrl = "RFQ_Outstg_List.aspx?&pageid=" & strPageId
        'ElseIf strFrm = "RFQ_List" Then
        '    lnkBack.NavigateUrl = "RFQ_List.aspx?&pageid=" & strPageId
        'ElseIf strFrm = "RFQ_Quote" Then
        '    lnkBack.NavigateUrl = "RFQ_Quote.aspx?&pageid=" & strPageId
        'ElseIf strFrm = "Dashboard" Then
        '    lnkBack.NavigateUrl = "../Dashboard/" & Request.QueryString("dpage") & ".aspx?pageid=" & strPageId
        'End If

        If strFrm = "RFQComSummary" And (Me.Request.QueryString("SubFrm") = "RFQ_List" Or Me.Request.QueryString("SubFrm") = "RFQ_Outstg_List" Or Me.Request.QueryString("SubFrm") = "Dashboard") Then
            Session("disable") = "N"
        End If

        Dim objrfq1 As New RFQ
        'If Not Page.IsPostBack Then
        If strFrm = "RFQSearch" Or strFrm = "RFQ_Outstg_List" Or strFrm = "RFQ_List" Or strFrm = "RFQ_Quote" Or strFrm = "ConvertPRList" Then
            GenerateTab()
        End If

        ViewState("vendor") = Request.QueryString("vcom_id")
        If Not IsNothing(ViewState("vendor")) Then
            cmdPreview.Visible = True
        Else
            cmdPreview.Visible = True 'False
        End If
        ViewState("callfrom") = Request(Trim("side"))
        ViewState("goto") = Request(Trim("goto"))

        ViewState("RFQ_ID") = Me.Request(Trim("RFQ_ID"))
        Dim rfq_num As String = Me.Request(Trim("RFQ_Num"))

        If strFrm = "ConvertPRList" Then
            Me.lblHead.Visible = False
        Else
            Me.lblHead.Visible = True
        End If

        Dim objread As New RFQ_User
        objrfq.read_rfqMstr(objread, "", ViewState("RFQ_ID"), rfq_num)
        Me.lbl_Num.Text = rfq_num
        Me.lbl_cur.Text = objread.cur_code
        Me.lbl_Name.Text = objread.RFQ_Name
        Me.lbl_con_num.Text = objread.phone
        ViewState("RFQ_ID") = objread.RFQ_ID
        Session("BCoyID") = objread.bcom_id
        Me.lbl_Con_person.Text = objread.con_person
        Me.lbl_exp.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objread.exp_date)
        Me.lbl_email.Text = objread.email
        Me.lbl_req_qout.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, objread.RFQ_Req_date)
        Dim pay_term As String = objrfq1.get_codemstr(objread.pay_term, "PT")
        Dim pay_type As String = objrfq1.get_codemstr(objread.pay_type, "PM")
        Dim ship_mode As String = objrfq1.get_codemstr(objread.ship_mode, "SM")
        Dim ship_term As String = objrfq1.get_codemstr(objread.ship_term, "ST")
        Me.lbl_pt.Text = pay_term
        Me.lbl_pm.Text = pay_type
        Me.lbl_sm.Text = ship_mode
        Me.lbl_st.Text = ship_term
        Me.txt_remark.Text = objread.remark
        Bindgrid(0)

        If objread.RFQStatus = "3" Then
            cmdPreview.Visible = False
        End If

        'If objread.RFQStatus <> "3" And objread.BDisplayStatus = "0" Then
        '    cmdPreview.Visible = True
        'Else
        '    cmdPreview.Visible = False
        'End If
        'Else
        '    If ViewState("SortExpression") = "" Then
        '        Bindgrid(0)
        '    Else
        '        Bindgrid(0, True)
        '    End If

        'End If
        displayAttachFile2()
        intPageRecordCnt = ViewState("intPageRecordCnt")
        If strFrm = "RFQ_Outstg_List" Or strFrm = "RFQ_List" Or strFrm = "RFQ_Quote" Or strFrm = "RFQComSummary" Or strFrm = "Dashboard" Or strFrm = "transtracking" Or strFrm = "ConvertPRList" Then
            Me.cmdPreview.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewRFQ.aspx", "VendorRequired=F&BCoyID=" & Session("BCoyID") & "&SCoyID=" & Request.QueryString("vcom_id") & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num"))) & "')")

        Else
            Me.cmdPreview.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewRFQ.aspx", "VendorRequired=T&BCoyID=" & Session("BCoyID") & "&SCoyID=" & Request.QueryString("vcom_id") & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num"))) & "')")

        End If
        'Me.cmdPreview.Attributes.Add("onclick", "PopWindow('RFQReport.aspx?pageid=" & strPageId & "&RFQ_ID=" & ViewState("RFQ_ID") & "&freeze=0&vcom_id=" & ViewState("vendor") & "');")
        'cmdPreviewDO1.Attributes.Add("onclick", "PopWindow('RFQReport.aspx?pageid=" & strPageId & " &RFQ_ID=" & viewstate("RFQ_ID") & "')")

        If Request.QueryString("frm") = "RFQComSummary" Then
            A1.Attributes.Remove("onclick")
            If Request.QueryString("Appr") = "Y" Then
                A1.HRef = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Appr=Y&Frm=" & Request(Trim("SubFrm")) & "&RFQType=" & Request.QueryString("RFQType") & "&pageid=" & Request(Trim("pageid")) & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")))
            Else
                A1.HRef = dDispatcher.direct("RFQ", "RFQComSummary.aspx", "Frm=" & Request(Trim("SubFrm")) & "&RFQType=" & Request.QueryString("RFQType") & "&pageid=" & Request(Trim("pageid")) & "&RFQ_Num=" & Me.Request(Trim("RFQ_Num")) & "&RFQ_ID=" & Me.Request(Trim("RFQ_ID")) & "&RFQ_Name=" & Request(Trim("RFQ_Name")))
            End If

        End If

        If Request.QueryString("frm") = "VendorRFQListExp" Or Request.QueryString("frm") = "RFQSearch" Then
            lblExtFile.Text = "File(s) Attached "
            lblRemark.Text = "Remarks"
        End If
    End Sub


    Public Sub dtg_rfqdetail_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        ' dtg_rfqdetail.CurrentPageIndex = e.NewPageIndex
        ' startindex = e.NewPageIndex * dtg_rfqdetail.PageSize
        dtg_rfqdetail.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub

    Private Sub displayAttachFile2()
        Dim objRFQ As New RFQ
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String
        objRFQ.get_comid(Me.Request(Trim("RFQ_ID")))

        dsAttach = objRFQ.getRFQTempAttach(Me.Request(Trim("RFQ_Num")), objRFQ.get_comid(Me.Request(Trim("RFQ_ID"))))

        pnlAttach2.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                '*************************meilai 25/2/05****************************
                'strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & "&doctype=RFQ>" & strFile & "</A>"
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "RFQ", EnumUploadFrom.FrontOff)
                '*************************meilai************************************
                Dim lblBr As New Label
                Dim lblFile As New Label

                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"
                pnlAttach2.Controls.Add(lblFile)
                pnlAttach2.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach2.Controls.Add(lblFile)
        End If

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(dtg_rfqdetail.CurrentPageIndex, True)
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False)
        Dim vs As String = viewState("RFQ_ID")
        Dim objrfq As New RFQ
        Dim objval As New RFQ_User
        Dim ds As System.Data.DataSet
        ds = objrfq.get_RFQDetail(vs)
        Dim dvViewSample As DataView
        viewstate("intPageRecordCnt") = ds.Tables(0).Rows.Count
        dvViewSample = ds.Tables(0).DefaultView
        intPageRecordCnt = ds.Tables(0).Rows.Count
        If pSorted Then
            dvViewSample.Sort = viewstate("SortExpression")
            If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        End If

        If viewstate("action") = "del" Then
            If dtg_rfqdetail.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtg_rfqdetail.PageSize = 0 Then
                dtg_rfqdetail.CurrentPageIndex = dtg_rfqdetail.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If
        'ViewState("S_COY_ID") = dvViewSample("RD_Coy_ID")
        dtg_rfqdetail.DataSource = dvViewSample
        dtg_rfqdetail.DataBind()
        'If Session("Env") = "FTN" Then
        '    Me.dtg_rfqdetail.Columns(5).Visible = False
        'Else
        '    Me.dtg_rfqdetail.Columns(5).Visible = True
        'End If
        Me.dtg_rfqdetail.Columns(5).Visible = False
    End Function



    'Sub ShowStats()
    '    lblCurrentIndex.Text = record & " Record(s) Found"
    '    ' lblPageCount.Text = "Total Page is " & dtg_rfqdetail.PageCount
    'End Sub


    'Private Sub cmd_pre_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim a As String = Session("a")
    '    Dim value As String = Session("RFQ_cur_value")
    '    If Request(Trim("page")) = "1" Then
    '        'Response.Redirect("RFQ_List.aspx?edit=1")
    '        Response.Redirect("RFQ_List.aspx?edit=1&pageid=" & strPageId & " ")
    '    Else
    '        'Me.Response.Redirect("Create_RFQ2.aspx?RFQ_name=" & Me.lbl_Name.Text & "&RFQ_option=" & a & "&RFQ_cur_value=" & value & "")
    '        Me.Response.Redirect("Create_RFQ2.aspx?RFQ_name=" & Me.lbl_Name.Text & "&RFQ_option=" & a & "&RFQ_cur_value=" & value & "&pageid=" & strPageId & " ")
    '    End If

    'End Sub

    'Private Sub cmd_pre3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    '    Response.Redirect("VendorRFQList.aspx?goto=" & viewstate("goto") & "&pageid=" & strPageId & " ")

    'End Sub

    Private Sub dtg_rfqdetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_rfqdetail.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            If dv("RD_Delivery_Lead_Time") = 0 Then
                e.Item.Cells(RfqEnum.Time).Text = "Ex-Stock"

            End If

            e.Item.Cells(RfqEnum.No).Text = e.Item.ItemIndex + 1 'dv("RD_RFQ_Line")
            If IsDBNull(dv("RD_Warranty_Terms")) Then
                e.Item.Cells(RfqEnum.Warranty).Text = "0"
            Else
                If dv("RD_Warranty_Terms") = 0 Then
                    e.Item.Cells(RfqEnum.Warranty).Text = "0"
                End If

            End If
        End If
    End Sub
    'Sub dtg_rfqdetail_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtg_rfqdetail.PageIndexChanged
    '    dtg_rfqdetail.CurrentPageIndex = e.NewPageIndex
    '    Bindgrid(0, True)
    'End Sub


    Private Sub dtg_rfqdetail_ItemCreated1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_rfqdetail.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtg_rfqdetail, e)

    End Sub

    Private Sub dtg_rfqdetail_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtg_rfqdetail.SelectedIndexChanged

    End Sub

    Private Sub cmd_pre_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_pre.ServerClick
        If ViewState("callfrom") = "rfqsum" Then
            Dim strurl As String = Session("quoteurl")
            Session("quoteurl") = ""
            Response.Redirect(strurl)
        Else
            Dim strurl As String = Session("strurl")
            Session("strurl") = ""
            Response.Redirect(strurl)
        End If

    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'If strFrm = "RFQSearch" Then
        '    Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                        "<li><a class=""t_entity_btn"" href=""VendorRFQList.aspx?pageid=" & strPageId & """><span>Outstanding RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                        "<li><a class=""t_entity_btn_selected"" href=""RFQSearch.aspx?pageid=" & strPageId & """><span>Quotation Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                        "</ul><div></div></div>"

        'ElseIf strFrm = "RFQ_Outstg_List" Then
        '    Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""Create_RFQ.aspx?pageid=" & strPageId & """><span>Raise RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn_selected"" href=""RFQ_Outstg_List.aspx?pageid=" & strPageId & """><span>Outstanding RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""RFQ_List.aspx?pageid=" & strPageId & """><span>RFQ Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""RFQ_Quote.aspx?pageid=" & strPageId & """><span>Quotation</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "</ul><div></div></div>"

        'ElseIf strFrm = "RFQ_List" Then
        '    Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""Create_RFQ.aspx?pageid=" & strPageId & """><span>Raise RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""RFQ_Outstg_List.aspx?pageid=" & strPageId & """><span>Outstanding RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn_selected"" href=""RFQ_List.aspx?pageid=" & strPageId & """><span>RFQ Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""RFQ_Quote.aspx?pageid=" & strPageId & """><span>Quotation</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "</ul><div></div></div>"

        'ElseIf strFrm = "RFQ_Quote" Then
        '    Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""Create_RFQ.aspx?pageid=" & strPageId & """><span>Raise RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""RFQ_Outstg_List.aspx?pageid=" & strPageId & """><span>Outstanding RFQ</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn"" href=""RFQ_List.aspx?pageid=" & strPageId & """><span>RFQ Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "<li><a class=""t_entity_btn_selected"" href=""RFQ_Quote.aspx?pageid=" & strPageId & """><span>Quotation</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                     "</ul><div></div></div>"
        'End If
        If strFrm = "RFQSearch" Then
            Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("RFQ", "VendorRFQList.aspx", "pageid=" & strPageId) & """><span>Outstanding RFQ</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("RFQ", "RFQSearch.aspx", "pageid=" & strPageId) & """><span>Quotation Listing</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "</ul><div></div></div>"

        ElseIf strFrm = "RFQ_Outstg_List" Then
            Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
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

        ElseIf strFrm = "RFQ_List" Then
            Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
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

        ElseIf strFrm = "RFQ_Quote" Then
            Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
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

        ElseIf strFrm = "ConvertPRList" Then
            Session("w_RFQ_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PR", "ConvertPR.aspx", "pageid=" & strPageId) & """><span>Convert PR</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PR", "ConvertPRListing.aspx", "pageid=" & strPageId) & """><span>Convert PR Listing</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
        End If
    End Sub

    'Protected Sub cmdPreview_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPreview.ServerClick
    '    If strFrm = "RFQSearch" Then
    '        'previewRFQ()

    '    Else

    '    End If

    'End Sub

    Private Sub previewRFQ()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("BCoyID"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT   RFQ_MSTR.RM_RFQ_ID, RFQ_MSTR.RM_Coy_ID, RFQ_MSTR.RM_RFQ_No, RFQ_MSTR.RM_RFQ_Name, " _
                            & "RFQ_MSTR.RM_Expiry_Date, RFQ_MSTR.RM_Status, RFQ_MSTR.RM_Remark, RFQ_MSTR.RM_Created_By, " _
                            & "RFQ_MSTR.RM_Created_On, RFQ_MSTR.RM_Currency_Code, RFQ_MSTR.RM_Payment_Term, " _
                            & "RFQ_MSTR.RM_Payment_Type, RFQ_MSTR.RM_Shipment_Term, RFQ_MSTR.RM_Shipment_Mode, " _
                            & "RFQ_MSTR.RM_Prefix, RFQ_MSTR.RM_B_Display_Status, RFQ_MSTR.RM_Reqd_Quote_Validity, " _
                            & "RFQ_MSTR.RM_Contact_Person, RFQ_MSTR.RM_Contact_Number, RFQ_MSTR.RM_Email, " _
                            & "RFQ_MSTR.RM_RFQ_OPTION, RFQ_MSTR.RM_VEN_DISTR_LIST_INDEX, RFQ_DETAIL.RD_RFQ_ID, " _
                            & "RFQ_DETAIL.RD_Coy_ID, RFQ_DETAIL.RD_RFQ_Line, RFQ_DETAIL.RD_Product_Code, " _
                            & "RFQ_DETAIL.RD_Vendor_Item_Code, RFQ_DETAIL.RD_Quantity, RFQ_DETAIL.RD_Product_Desc, " _
                            & "RFQ_DETAIL.RD_UOM, RFQ_DETAIL.RD_Delivery_Lead_Time, RFQ_DETAIL.RD_Warranty_Terms, " _
                            & "RFQ_DETAIL.RD_Product_Name, RFQ_INVITED_VENLIST.RIV_RFQ_ID, RFQ_INVITED_VENLIST.RIV_S_Coy_ID, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_Coy_Name, RFQ_INVITED_VENLIST.RIV_S_Addr_Line1, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_Addr_Line2, RFQ_INVITED_VENLIST.RIV_S_Addr_Line3, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_PostCode, RFQ_INVITED_VENLIST.RIV_S_City, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_State, RFQ_INVITED_VENLIST.RIV_S_Country, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_Phone, RFQ_INVITED_VENLIST.RIV_S_Fax, " _
                            & "RFQ_INVITED_VENLIST.RIV_S_Email, COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, " _
                            & "COMPANY_MSTR.CM_COY_TYPE, COMPANY_MSTR.CM_PARENT_COY_ID, COMPANY_MSTR.CM_ACCT_NO, " _
                            & "COMPANY_MSTR.CM_BANK, COMPANY_MSTR.CM_BRANCH, COMPANY_MSTR.CM_ADDR_LINE1, " _
                            & "COMPANY_MSTR.CM_ADDR_LINE2, COMPANY_MSTR.CM_ADDR_LINE3, COMPANY_MSTR.CM_POSTCODE, " _
                            & "COMPANY_MSTR.CM_CITY, COMPANY_MSTR.CM_STATE, COMPANY_MSTR.CM_COUNTRY, " _
                            & "COMPANY_MSTR.CM_PHONE, COMPANY_MSTR.CM_FAX, COMPANY_MSTR.CM_EMAIL, " _
                            & "COMPANY_MSTR.CM_COY_LOGO, COMPANY_MSTR.CM_BUSINESS_REG_NO, " _
                            & "COMPANY_MSTR.CM_TAX_REG_NO, COMPANY_MSTR.CM_PAYMENT_TERM, " _
                            & "COMPANY_MSTR.CM_PAYMENT_METHOD, COMPANY_MSTR.CM_ACTUAL_TERMSANDCONDFILE, " _
                            & "COMPANY_MSTR.CM_HUB_TERMSANDCONDFILE, COMPANY_MSTR.CM_PWD_DURATION, " _
                            & "COMPANY_MSTR.CM_TAX_CALC_BY, COMPANY_MSTR.CM_CURRENCY_CODE, " _
                            & "COMPANY_MSTR.CM_BCM_SET, COMPANY_MSTR.CM_BUDGET_FROM_DATE, " _
                            & "COMPANY_MSTR.CM_BUDGET_TO_DATE, COMPANY_MSTR.CM_RFQ_OPTION, " _
                            & "COMPANY_MSTR.CM_LICENCE_PACKAGE, COMPANY_MSTR.CM_LICENSE_USERS, " _
                            & "COMPANY_MSTR.CM_SUB_START_DT, COMPANY_MSTR.CM_SUB_END_DT, " _
                            & "COMPANY_MSTR.CM_LICENSE_PRODUCTS, COMPANY_MSTR.CM_FINDEPT_MODE, " _
                            & "COMPANY_MSTR.CM_PRIV_LABELING, COMPANY_MSTR.CM_SKINS_ID, COMPANY_MSTR.CM_TRAINING, " _
                            & "COMPANY_MSTR.CM_STATUS, COMPANY_MSTR.CM_DELETED, COMPANY_MSTR.CM_MOD_BY, " _
                            & "COMPANY_MSTR.CM_MOD_DT, COMPANY_MSTR.CM_ENT_BY, COMPANY_MSTR.CM_ENT_DT, " _
                            & "COMPANY_MSTR.CM_SKU, COMPANY_MSTR.CM_TRANS_NO, COMPANY_MSTR.CM_CONTACT, " _
                            & "COMPANY_MSTR.CM_REPORT_USERS, COMPANY_MSTR.CM_INV_APPR, COMPANY_MSTR.CM_MULTI_PO, " _
                            & "COMPANY_MSTR.CM_BA_CANCEL, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
                            & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS CMState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_INVITED_VENLIST.RIV_S_State) AND (CODE_CATEGORY = 's') AND " _
                            & "(CODE_VALUE = RFQ_INVITED_VENLIST.RIV_S_Country)) AS SupplierAddrState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_INVITED_VENLIST.RIV_S_Country) AND (CODE_CATEGORY = 'ct')) " _
                            & "AS SupplierAddrCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Payment_Term) AND (CODE_CATEGORY = 'pt')) " _
                            & "AS RFQ_PaymentTerm, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Payment_Type) AND (CODE_CATEGORY = 'pm')) " _
                            & "AS RFQ_PaymentMethod, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Shipment_Term) AND (CODE_CATEGORY = 'St')) " _
                            & "AS RFQ_ShipmentTerm, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = RFQ_MSTR.RM_Shipment_Mode) AND (CODE_CATEGORY = 'sm')) " _
                            & "AS RFQ_ShipmentMode, " _
                            & "(SELECT   CM_BUSINESS_REG_NO " _
                            & "FROM      COMPANY_MSTR AS B " _
                            & "WHERE   (RFQ_INVITED_VENLIST.RIV_S_Coy_ID = CM_COY_ID)) AS sUPPBUSSREGNO " _
                            & "FROM      RFQ_MSTR INNER JOIN " _
                            & "RFQ_DETAIL ON RFQ_MSTR.RM_RFQ_ID = RFQ_DETAIL.RD_RFQ_ID INNER JOIN " _
                            & "RFQ_INVITED_VENLIST ON RFQ_MSTR.RM_RFQ_ID = RFQ_INVITED_VENLIST.RIV_RFQ_ID INNER JOIN " _
                            & "COMPANY_MSTR ON RFQ_MSTR.RM_Coy_ID = COMPANY_MSTR.CM_COY_ID " _
                            & "WHERE (RFQ_MSTR.RM_RFQ_No = @prmRFQNum) AND (RFQ_MSTR.RM_Coy_ID = @prmBCoyID) AND " _
                            & "(RFQ_INVITED_VENLIST.RIV_S_Coy_ID = @prmVCoyID)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmVCoyID", Request.QueryString("vcom_id")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmBCoyID", Session("BCoyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmRFQNum", Me.Request(Trim("RFQ_Num"))))

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewRFQ_FTN_DataTablePreviewRFQ", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = appPath & "RFQ\PreviewRFQ-FTN2.rdlc"
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "par1"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            'Dim deviceInfo As String = _
            '     "<DeviceInfo>" + _
            '         "  <OutputFormat>EMF</OutputFormat>" + _
            '         "  <PageWidth>8.27in</PageWidth>" + _
            '         "  <PageHeight>11in</PageHeight>" + _
            '         "  <MarginTop>0.25in</MarginTop>" + _
            '         "  <MarginLeft>0.25in</MarginLeft>" + _
            '         "  <MarginRight>0.25in</MarginRight>" + _
            '         "  <MarginBottom>0.25in</MarginBottom>" + _
            '         "</DeviceInfo>"
            Dim deviceInfo As String = _
               "<DeviceInfo>" + _
                   "  <OutputFormat>EMF</OutputFormat>" + _
                   "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

            Dim fs As New FileStream(appPath & "RFQ\RFQReport.PDF", FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('RFQReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
            strJScript += "</script>"
            Response.Write(strJScript)

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub
End Class
