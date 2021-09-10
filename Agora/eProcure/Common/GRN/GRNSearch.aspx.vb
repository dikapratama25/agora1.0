Imports AgoraLegacy
Imports eProcure.Component
Public Class GRNSearch
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strGRNType As String
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    Dim objDO As New DeliveryOrder
    Dim strPONo As String
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cboDocType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtCreationDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_result As System.Web.UI.WebControls.Label
    Protected WithEvents txtNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVendorName As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkUnInv As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkInv As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkPendAck As System.Web.UI.WebControls.CheckBox
    Protected WithEvents dtgGRN As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdNewGRN As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAck As System.Web.UI.WebControls.Button
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents tdAck As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Public Enum EnumGRNSearch
        icPONo = 0
        icPODate = 1
        icDONo = 2
        icDODate = 3
        icGRNNo = 4
        icCreateDate = 5
        icReceiveDate = 6
        icVendorName = 7
        icAcceptBy = 8
        icStatus = 9
        icLevel2User = 10
    End Enum
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdAck.Enabled = False
        'cmdNewGRN.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdAck)
        'alButtonList.Add(cmdNewGRN)
        htPageAccess.Add("add", alButtonList)
        htPageAccess.Add("update", alButtonList)      
        CheckButtonAccess()
        If Not IsPostBack Then
            If UCase(strGRNType) = "GRNACK" Then
                cmdAck.Enabled = blnCanUpdate Or blnCanAdd
                'cmdNewGRN.Enabled = False
            Else
                cmdAck.Enabled = False
                'cmdNewGRN.Enabled = blnCanUpdate Or blnCanAdd
            End If           
        End If
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            GenerateTab()
        End If
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        'Session("CompanyID") = "PPSB"

        strGRNType = Me.Request.QueryString("Type")
        If Not IsPostBack Then
            If UCase(strGRNType) = "GRNACK" Then
                cmdAck.Visible = True
                'cmdNewGRN.Visible = False
            Else
                cmdAck.Visible = False
                'cmdNewGRN.Visible = True
            End If
            Session("strurl") = strCallFrom
            cboDocType.SelectedValue = "GRN"
        End If

        If UCase(strGRNType) = "GRNACK" Then
            'lblTitle.Text = "Goods Receipt Note Acknowledgement"
            dtgGRN.Columns(7).Visible = True
            tdAck.Style("display") = "none"
        Else
            'lblTitle.Text = "Goods Receipt Note Generation"
        End If
        SetGridProperty(dtgGRN)
        intPageRecordCnt = ViewState("intPageRecordCnt")
        cboDocType.Attributes.Add("onchange", "Check();")
    End Sub

    Sub dtgGRN_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgGRN.PageIndexChanged
        dtgGRN.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgGRN.SortCommand
        Grid_SortCommand(sender, e)
        dtgGRN.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub
    Private Sub dtgGRN_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGRN.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgGRN, e)
    End Sub

    Private Sub dtgGRN_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGRN.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            'chk = e.Item.Cells(0).FindControl("chkSelection")
            'chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            '//to dynamic build hyoerlink
            Dim lnkDONum, lnkPONum, lnkGRNNum As HyperLink
            lnkDONum = e.Item.FindControl("lnkDONum")
            lnkDONum.Text = dv("DOM_DO_NO")
            lnkDONum.NavigateUrl = "javascript:;"

            '//SR : AS0021, MOo, 26/10/2005
            '//lnkDONum.Attributes.Add("onclick", "return PopWindow('../DO/DOReport.aspx?DONo=" & _
            '//dv("DOM_DO_NO") & "&POInx=" & dv("DOM_PO_Index") & "&pageid=" & strPageId & _
            '//"&SCoyID=" & dv("POM_S_Coy_ID") & "')")

            lnkDONum.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("DO", "PreviewDODetails.aspx", "caller=buyer&DONo=" & _
                        dv("DOM_DO_NO") & "&POIdx=" & dv("DOM_PO_Index") & "&pageid=" & strPageId & _
                        "&SCoyID=" & dv("POM_S_Coy_ID")) & "')")
            'lnkDONum.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Report", "PreviewDO.aspx", "DONo=" & dv("DOM_DO_NO") & "&POInx=" & dv("DOM_PO_Index") & "&pageid=" & strPageId & "&SCoyID=" & dv("POM_S_Coy_ID") & "&PO_NO=" & dv("POM_PO_No")) & "')")
            'dv("DOM_DO_Index") & "&deptname=" & dv("CDM_DEPT_NAME")

            lnkGRNNum = e.Item.FindControl("lnkGRNNum")
            lnkGRNNum.Text = dv("GM_GRN_NO")
            'lnkGRNNum.NavigateUrl = "GRNDetails.aspx?DOIdx=" & dv("DOM_DO_INDEX") '& "&DONo=" & dv("DOM_DO_NO") & "&GRNNo=" & dv("GM_GRN_NO") & "&PONo=" & dv("POM_PO_NO")
            lnkGRNNum.NavigateUrl = dDispatcher.direct("GRN", "GRNDetails.aspx", "Frm=GRNSearch&GRNNo=" & dv("GM_GRN_NO") & "&BCoyID=" & Session("CompanyID") & "&type=" & strGRNType & "&pageid=" & strPageId)
            'e.Item.Cells(1).Te = "<a href='addDept.aspx'>" & e.Item.Cells(1).Text & "</a>"
            'e.Item.Cells(EnumGRN.icPOLine).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("GM_CREATED_DATE"))

            e.Item.Cells(EnumGRNSearch.icDODate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("DOM_DO_DATE"))
            e.Item.Cells(EnumGRNSearch.icCreateDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("GM_CREATED_DATE"))
            e.Item.Cells(EnumGRNSearch.icReceiveDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("GM_DATE_RECEIVED"))


            lnkPONum = e.Item.FindControl("lnkPONum")
            If strPONo = dv("POM_PO_No") Then
                strPONo = dv("POM_PO_No")
                lnkPONum.Text = ""
                e.Item.Cells(EnumGRNSearch.icPODate).Text = ""
            Else
                strPONo = dv("POM_PO_No")

                lnkPONum.Text = dv("POM_PO_No")
                '<a href=../PO/PODetail.aspx?PO_NO=" & dv("PO_NO") & "&side=b&filetype=2" & dv("PO_NO") & "</a>"
                lnkPONum.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=GRNSearch&caller=buyer&PO_INDEX=" & dv("DOM_PO_Index") & "&PO_NO=" & dv("POM_PO_NO") & "&BCoyID=" & Session("CompanyID") & "&side=other&filetype=2&pageid=" & strPageId)
                e.Item.Cells(EnumGRNSearch.icPODate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("POM_PO_DATE"))
            End If
            'Michelle (23/1/2013) - Issue 1727
            If objDO.withAttach(dv("DOM_DO_NO"), dv("POM_S_Coy_ID")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")
                lnkDONum.Controls.Add(imgAttach)
                e.Item.Cells(EnumGRNSearch.icDONo).Controls.Add(imgAttach)
            End If

        End If
    End Sub


    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim strDocType, strNo, strOurRef, strCreationDt, strVendorName, strStatus As String
        'Dim dteCreationDt, dteSubmittedDt As Date
        Dim intTotRecord As Integer
        Dim intchk As Integer
        Dim objGRN As New GRN
        intchk = 0
        If cboDocType.SelectedItem.Value <> "0" Then
            strDocType = cboDocType.SelectedItem.Value
        End If

        If txtNo.Text <> "" Then
            strNo = Trim(txtNo.Text)
        End If

        If txtCreationDate.Text <> "" Then
            strCreationDt = txtCreationDate.Text
        End If

        If txtVendorName.Text <> "" Then
            strVendorName = txtVendorName.Text
        End If

        If chkUnInv.Checked = True Then
            strStatus = strStatus & GRNStatus.Uninvoice
        End If

        If chkInv.Checked = True Then
            strStatus = strStatus & GRNStatus.Invoiced
        End If

        If chkPendAck.Checked = True Then
            strStatus = strStatus & GRNStatus.PendingACK
        End If

        If Len(strStatus) <> 0 Then
            Dim strTmp, strTmp1, strTmp2 As String
            Dim I As Integer
            I = 0
            strTmp2 = strStatus
            For intchk = 0 To Len(strTmp2) - 1
                I = I + 1
                strTmp1 = Mid(strTmp2, I, 1)
                If Len(strStatus) <> (intchk + 1) Then
                    If Len(strStatus) > 1 Then
                        strTmp1 = strTmp1 & ","
                    End If
                End If
                strTmp = strTmp & strTmp1
            Next
            strStatus = "(" & strTmp & ")"
        End If
        '//Retrieve Data from Database
        Dim dsDO As DataSet = New DataSet
        dsDO = objGRN.GetGRN(strDocType, strNo, strCreationDt, strVendorName, strStatus, strGRNType)

        '//for sorting asc or desc
        Dim dvViewDO As DataView
        dvViewDO = dsDO.Tables(0).DefaultView

        dvViewDO.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" And viewstate("SortExpression") <> "" Then dvViewDO.Sort += " DESC"

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If dtgGRN.CurrentPageIndex > 0 And dsDO.Tables(0).Rows.Count Mod dtgGRN.PageSize = 0 Then
                dtgGRN.CurrentPageIndex = dtgGRN.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If
        intTotRecord = dsDO.Tables(0).Rows.Count
        intPageRecordCnt = intTotRecord

        If intTotRecord > 0 Then
            resetDatagridPageIndex(dtgGRN, dvViewDO)
            dtgGRN.DataSource = dvViewDO
            dtgGRN.DataBind()
        Else
            dtgGRN.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ' add for above checking
        viewstate("PageCount") = dtgGRN.PageCount
    End Function

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgGRN.CurrentPageIndex = 0
        viewstate("SortAscending") = "no"
        viewstate("SortExpression") = "POM_PO_DATE"
        Bindgrid()
    End Sub

    Private Sub cmdNewGRN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNewGRN.Click
        '****************************meilai 15/2/2005**********************************************************
        'Response.Redirect("AddGRN.aspx?Mode=New&TYPE=" & strGRNType & "&pageid=" & strPageId)
        Response.Redirect(dDispatcher.direct("GRN", "AddGRN.aspx", "Mode=New&TYPE=" & strGRNType & "&pageid=" & strPageId & "&Level=created"))
        '****************************meilai********************************************************************
    End Sub

    Private Sub cmdAck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAck.Click
        '****************************meilai 15/2/2005**********************************************************
        'Response.Redirect("AddGRN.aspx?Mode=New&Type=" & strGRNType & "&pageid=" & strPageId)
        Response.Redirect(dDispatcher.direct("GRN", "AddGRN.aspx", "Mode=New&Type=" & strGRNType & "&pageid=" & strPageId & "&Level=acknowledged"))
        '****************************meilai********************************************************************
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        'Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn"" href=""GRNList.aspx?pageid=" & strPageId & """><span>Issue GRN</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn_selected"" href=""#""><span>GRN Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "</ul><div></div></div>"
        Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("GRN", "GRNList.aspx", "pageid=" & strPageId) & """><span>Issue GRN</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""#""><span>GRN Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"


    End Sub
End Class
