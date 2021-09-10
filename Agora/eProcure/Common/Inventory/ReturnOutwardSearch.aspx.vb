Imports AgoraLegacy
Imports eProcure.Component


Public Class ReturnOutwardSearch
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strGRNType As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents txtRONo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVendorName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtItemCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_startdate As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_enddate As System.Web.UI.WebControls.TextBox
    Protected WithEvents dtg_ROList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Public Enum EnumROView
        icRONo = 0
        icRODate = 1
        icVendorName = 2
        icGRNNo = 3
        icGRNDate = 4
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtg_ROList)
        If Not Page.IsPostBack Then
            GenerateTab()
            txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
            txt_enddate.Text = DateTime.Now.ToShortDateString()
        End If

        intPageRecordCnt = ViewState("intPageRecordCnt")
    End Sub

    Public Sub dtg_ROList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtg_ROList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtg_ROList.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        ''//Retrieve Data from Database
        Dim strMsg As String
        Dim objGlobal As New AppGlobals
        Dim comparedt As Date
        comparedt = DateAdd("m", -6, CDate(txt_enddate.Text))

        If CDate(txt_startdate.Text) < comparedt Then
            strMsg = "Start/ End date " & objGlobal.GetErrorMessage("00015") & " 6 months"
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Return False
        End If

        Dim ds As New DataSet
        Dim objInventory As New Inventory

        ds = objInventory.ROList(txtRONo.Text, txtVendorName.Text, txtItemCode.text, txt_startdate.Text, txt_enddate.Text)

        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView

        dvViewPR.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        If ViewState("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtg_ROList, dvViewPR)
            dtg_ROList.DataSource = dvViewPR
            dtg_ROList.DataBind()
        Else
            dtg_ROList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtg_ROList.PageCount
    End Function

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtg_ROList.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "IROM_RO_NO"
        Bindgrid(0)
    End Sub

    Private Sub dtg_ROList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_ROList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            e.Item.Cells(EnumROView.icRODate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("IROM_RO_DATE"))

            Dim lnkRONo, lnkGRNNo As HyperLink
            lnkRONo = e.Item.FindControl("lnkRONo")
            lnkRONo.Text = dv("IROM_RO_NO")
            lnkRONo.NavigateUrl = dDispatcher.direct("Inventory", "ReturnOutwardDetail.aspx", "pageid=" & strPageId & "&RO_NO=" & dv("IROM_RO_NO") & "&frm=ROSearch")

            lnkGRNNo = e.Item.FindControl("lnkGRNNo")
            lnkGRNNo.Text = dv("IROM_GRN_NO")
            lnkGRNNo.NavigateUrl = dDispatcher.direct("GRN", "GRNDetails.aspx", "Frm=ROSearch&GRNNo=" & dv("IROM_GRN_NO") & "&BCoyID=" & Session("CompanyID") & "&type=" & strGRNType & "&pageid=" & strPageId)

            e.Item.Cells(EnumROView.icGRNDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("GM_DATE_RECEIVED"))
        End If
    End Sub

    Private Sub dtg_ROList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_ROList.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtg_ROList, e)
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_SearchRO_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "ReturnOutwardListing.aspx", "pageid=" & strPageId) & """><span>Return Outward</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "ReturnOutwardSearch.aspx", "pageid=" & strPageId) & """><span>Return Outward Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"

    End Sub

    Private Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        Me.txtVendorName.Text = ""
        Me.txtRONo.Text = ""
        Me.txtItemCode.Text = ""
        Me.txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
        Me.txt_enddate.Text = DateTime.Now.ToShortDateString()
    End Sub
End Class
