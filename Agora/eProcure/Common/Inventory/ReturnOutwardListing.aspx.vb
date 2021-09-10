Imports AgoraLegacy
Imports eProcure.Component

Public Class ReturnOutwardListing
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button

    Dim objDO As New DeliveryOrder
    Dim strPONo As String
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strGRNType As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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
        icGRNNo = 0
        icCreateDate = 1
        icReceiveDate = 2
        icVendorName = 3
        icPONo = 4
        icPODate = 5
        icDONo = 6
        icDODate = 7
        icAcceptBy = 8
        'icStatus = 9
        icLevel2User = 9
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        htPageAccess.Add("add", alButtonList)
        htPageAccess.Add("update", alButtonList)
        CheckButtonAccess()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '' ''If Not Page.IsPostBack Then
        '' ''    GenerateTab()
        '' ''End If
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgGRN)

        If Not IsPostBack Then
            GenerateTab()
            Session("strurl") = strCallFrom
            txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
            txt_enddate.Text = DateTime.Now.ToShortDateString()
        End If

        '' ''SetGridProperty(dtgGRN)
        intPageRecordCnt = ViewState("intPageRecordCnt")
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

            Dim lnkDONum, lnkPONum, lnkGRNNum As HyperLink
            lnkDONum = e.Item.FindControl("lnkDONum")
            lnkDONum.Text = dv("DOM_DO_NO")
            lnkDONum.NavigateUrl = "javascript:;"

            lnkDONum.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("DO", "PreviewDODetails.aspx", "caller=buyer&DONo=" & dv("DOM_DO_NO") & "&POIdx=" & dv("DOM_PO_Index") & "&pageid=" & strPageId & "&SCoyID=" & dv("POM_S_Coy_ID")) & "')")
            
            lnkGRNNum = e.Item.FindControl("lnkGRNNum")
            lnkGRNNum.Text = dv("GM_GRN_NO")
            lnkGRNNum.NavigateUrl = dDispatcher.direct("Inventory", "ReturnOutward.aspx", "Frm=ROListing&GRNNo=" & dv("GM_GRN_NO") & "&BCoyID=" & Session("CompanyID") & "&type=" & strGRNType & "&pageid=" & strPageId)
            
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
                lnkPONum.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=ROListing&caller=buyer&PO_INDEX=" & dv("DOM_PO_Index") & "&PO_NO=" & dv("POM_PO_NO") & "&BCoyID=" & Session("CompanyID") & "&side=other&filetype=2&pageid=" & strPageId)
                e.Item.Cells(EnumGRNSearch.icPODate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("POM_PO_DATE"))
            End If

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
        Dim intTotRecord, intchk As Integer
        Dim objInv As New Inventory
        Dim strMsg As String
        Dim objGlobal As New AppGlobals
        Dim comparedt As Date
        comparedt = DateAdd("m", -6, CDate(txt_enddate.Text))
        intchk = 0

        If CDate(txt_startdate.Text) < comparedt Then
            strMsg = "Start/ End date " & objGlobal.GetErrorMessage("00015") & " 6 months"
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Return False
        End If

        '//Retrieve Data from Database
        Dim dsDO As DataSet = New DataSet
        dsDO = objInv.GetGRNPartial(txtGRNNo.Text, txtPONo.Text, txt_startdate.Text, txt_enddate.Text, txtVendorName.Text)

        '//for sorting asc or desc
        Dim dvViewDO As DataView
        dvViewDO = dsDO.Tables(0).DefaultView

        dvViewDO.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewDO.Sort += " DESC"

        If ViewState("action") = "del" Then
            If dtgGRN.CurrentPageIndex > 0 And dsDO.Tables(0).Rows.Count Mod dtgGRN.PageSize = 0 Then
                dtgGRN.CurrentPageIndex = dtgGRN.CurrentPageIndex - 1
                ViewState("action") = ""
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

        ViewState("PageCount") = dtgGRN.PageCount
    End Function

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgGRN.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "POM_PO_DATE"
        Bindgrid()
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_SearchROListing_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "ReturnOutwardListing.aspx", "pageid=" & strPageId) & """><span>Return Outward</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "ReturnOutwardSearch.aspx", "pageid=" & strPageId) & """><span>Return Outward Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"

    End Sub

    Private Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        Me.txtGRNNo.Text = ""
        Me.txtPONo.Text = ""
        txtVendorName.Text = ""
        Me.txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
        Me.txt_enddate.Text = DateTime.Now.ToShortDateString()
    End Sub
End Class
