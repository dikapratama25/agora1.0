Imports AgoraLegacy
Imports eProcure.Component


Public Class ReturnInwardListing
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    'Dim blnMsg As Boolean

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents txtMRSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtIssueTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDepartment As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAccCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_startdate As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_enddate As System.Web.UI.WebControls.TextBox
    Protected WithEvents dtg_RIList As System.Web.UI.WebControls.DataGrid
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
    Public Enum EnumMRSView
        icMRSNo = 0
        icMRSDate = 1
        icAccCode = 2
        icIssueTo = 3
        icDepartment = 4
        icRefNo = 5
        icRemark = 6
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtg_RIList)
        If Not Page.IsPostBack Then
            GenerateTab()
            txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
            txt_enddate.Text = DateTime.Now.ToShortDateString()
        End If


        intPageRecordCnt = ViewState("intPageRecordCnt")
    End Sub

    Public Sub dtg_RIList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtg_RIList.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtg_RIList.CurrentPageIndex = 0
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

        ds = objInventory.MRSList(txtMRSNo.Text, txtIssueTo.Text, txtDepartment.Text, txtAccCode.Text, txt_startdate.Text, txt_enddate.Text)

        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView

        dvViewPR.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        If ViewState("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtg_RIList, dvViewPR)
            dtg_RIList.DataSource = dvViewPR
            dtg_RIList.DataBind()
        Else
            dtg_RIList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtg_RIList.PageCount
    End Function

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtg_RIList.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "IRSM_IRS_NO"
        Bindgrid(0)

    End Sub

    Private Sub dtg_RIList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_RIList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            e.Item.Cells(EnumMRSView.icMRSDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("IRSM_IRS_DATE"))

            Dim lnkMRSNo As HyperLink
            lnkMRSNo = e.Item.Cells(EnumMRSView.icMRSNo).FindControl("lnkMRSNo")
            lnkMRSNo.Text = dv("IRSM_IRS_NO")
            lnkMRSNo.NavigateUrl = dDispatcher.direct("Inventory", "ReturnInward.aspx", "pageid=" & strPageId & "&MRS_NO=" & dv("IRSM_IRS_NO") & "&frm=RIListing")
        End If
    End Sub

    Private Sub dtg_RIList_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_RIList.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtg_RIList, e)
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_ReturnInward_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "ReturnInwardListing.aspx", "pageid=" & strPageId) & """><span>Return Inward</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "ReturnInwardSearch.aspx", "type=Listing&pageid=" & strPageId) & """><span>Return Inward Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
    End Sub

    Private Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        Me.txtMRSNo.Text = ""
        Me.txtIssueTo.Text = ""
        Me.txtDepartment.Text = ""
        Me.txtAccCode.Text = ""
        Me.txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
        Me.txt_enddate.Text = DateTime.Now.ToShortDateString()
    End Sub
End Class
