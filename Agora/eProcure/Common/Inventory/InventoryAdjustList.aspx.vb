
Imports AgoraLegacy
Imports eProcure.Component
Public Class InventoryAdjustList
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim intLocIndicator As Integer
    Dim strLoc As String
    Dim strSLoc As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objInv As New Inventory

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgInv)
        If Not Page.IsPostBack Then
            GenerateTab()
            LoadLoc()
            Me.ddl_SubLoc.Items.Insert(0, "---Select---")
            ChangeHeaderText()
        End If
    End Sub

    Private Sub LoadLoc()
        Dim objInv As New Inventory
        Dim ds As New DataSet

        ds = objInv.PopLocation

        Me.ddl_Loc.Items.Clear()
        Me.ddl_Loc.DataSource = ds.Tables(0)
        Me.ddl_Loc.DataTextField = "LM_LOCATION"
        Me.ddl_Loc.DataBind()
        Me.ddl_Loc.Items.Insert(0, "---Select---")

        objInv = Nothing

    End Sub

    Private Sub ChangeHeaderText()
        Dim objInv As New Inventory

        objInv.GetLocationInfo(strLoc, strSLoc, intLocIndicator)

        dtgInv.Columns(2).HeaderText = strLoc
        dtgInv.Columns(3).HeaderText = strSLoc
        Me.lblLoc.Text = strLoc
        Me.lblSubLoc.Text = strSLoc

        If intLocIndicator > 1 Then 'Sub Location is defined
            Me.dtgInv.Columns(3).Visible = True
            lblSubLoc.Visible = True
            Me.ddl_SubLoc.Visible = True
        Else
            Me.dtgInv.Columns(3).Visible = False
            lblSubLoc.Visible = False
            Me.ddl_SubLoc.Visible = False
        End If
        objInv = Nothing

    End Sub

    Private Sub BindInv(Optional ByVal pSorted As Boolean = False)
        Dim objInv As New Inventory
        Dim ds As DataSet
        Dim dvViewInv As DataView

        ds = objInv.GetInvAdjustmentList("ADJ", Me.txtItemCode.Text, Me.txtItemName.Text, Me.txt_startdate.Text, Me.txt_enddate.Text, Me.ddl_Loc.SelectedItem.Text, Me.ddl_SubLoc.SelectedItem.Text)
        dvViewInv = ds.Tables(0).DefaultView
        dvViewInv.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewInv.Sort += " DESC"

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgInv, dvViewInv)
            dtgInv.DataSource = dvViewInv
            dtgInv.DataBind()
        Else
            dtgInv.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ViewState("PageCount") = dtgInv.PageCount
        objInv = Nothing

    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_Adj_tabs") = "<div class=""t_entity""><ul>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "InventoryAdjust.aspx", "pageid=" & strPageId) & """><span>Adjustment</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "InventoryAdjustList.aspx", "pageid=" & strPageId) & """><span>Adjustment Listing</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                            "</ul><div></div></div>"

    End Sub

    Private Sub dtgInv_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInv.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgInv, e)
        ChangeHeaderText()
    End Sub

    Private Sub dtgInv_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInv.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            e.Item.Cells(7).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("IT_TRANS_DATE"))

        End If
    End Sub

    Private Sub dtgInv_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgInv.PageIndexChanged
        dtgInv.CurrentPageIndex = e.NewPageIndex
        BindInv(True)
        ChangeHeaderText()
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgInv.SortCommand
        Grid_SortCommand(sender, e)
        dtgInv.CurrentPageIndex = 0
        BindInv(True)
        ChangeHeaderText()
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Dim objGlobal As New AppGlobals

        If CompareDate() Then
            ViewState("SortAscending") = "no"
            ViewState("SortExpression") = "IT_TRANS_DATE"
            dtgInv.CurrentPageIndex = 0
            BindInv(True)
            ChangeHeaderText()

        Else
            Common.NetMsgbox(Me, Replace(Me.lblEndDt.Text, ":", "") & " " & objGlobal.GetErrorMessage("00017") & " " & Replace(Me.lblStartDt.Text, ":", "") & ".", MsgBoxStyle.Information)
        End If
        objGlobal = Nothing

    End Sub

    Private Sub ddl_Loc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Loc.SelectedIndexChanged
        Dim objInv As New Inventory
        Dim ds As New DataSet

        ds = objInv.PopLocation(Me.ddl_Loc.SelectedItem.Text)

        Me.ddl_SubLoc.Items.Clear()
        Me.ddl_SubLoc.DataSource = ds.Tables(0)
        Me.ddl_SubLoc.DataTextField = "LM_SUB_LOCATION"
        Me.ddl_SubLoc.DataValueField = "LM_LOCATION_INDEX"
        Me.ddl_SubLoc.DataBind()
        Me.ddl_SubLoc.Items.Insert(0, "---Select---")

        objInv = Nothing
    End Sub

    Private Function CompareDate() As Boolean
        Dim dtStartDt As Date
        Dim dtEndDt As Date

        CompareDate = True

        If Me.txt_startdate.Text <> "" Then
            dtStartDt = Me.txt_startdate.Text
        End If
        If Me.txt_enddate.Text <> "" Then
            dtEndDt = CDate(Me.txt_enddate.Text)
        End If

        If Me.txt_startdate.Text <> "" And Me.txt_enddate.Text <> "" Then
            If dtEndDt < dtStartDt Then
                CompareDate = False
            End If
        Else
            CompareDate = True
        End If

    End Function
End Class