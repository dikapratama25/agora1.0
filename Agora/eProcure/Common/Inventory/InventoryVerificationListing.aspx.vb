Imports AgoraLegacy
Imports eProcure.Component
Public Class InventoryVerificationListing
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
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgInv)

        If Not Page.IsPostBack Then
            GenerateTab()
            Me.txt_enddate.Text = Format(Now, "dd/MM/yyyy")
            Me.txt_startdate.Text = Format(DateAdd(DateInterval.Month, -1, Now.Date), "dd/MM/yyyy")
        End If
        ChangeHeaderText()
    End Sub

    Private Sub ChangeHeaderText()
        Dim objInv As New Inventory

        objInv.GetLocationInfo(strLoc, strSLoc, intLocIndicator)

        dtgInv.Columns(4).HeaderText = strLoc
        dtgInv.Columns(5).HeaderText = strSLoc
        If intLocIndicator > 1 Then 'Sub Location is defined
            Me.dtgInv.Columns(5).Visible = True
        Else
            Me.dtgInv.Columns(5).Visible = False
        End If
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

    Private Sub BindInv(Optional ByVal pSorted As Boolean = False)
        Dim objInv As New Inventory
        Dim ds As DataSet
        Dim dvViewInv As DataView

        ds = objInv.GetFullInvVerify(Me.txtNo.Text, Me.txtVendor.Text, Me.txt_startdate.Text, Me.txt_enddate.Text)
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

    Private Sub dtgInv_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInv.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgInv, e)
    End Sub

    Private Sub dtgInv_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInv.ItemDataBound
        Dim objInv As New Inventory
        Dim intPass As Integer
        Dim intFail As Integer

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim txtPass As TextBox
            Dim txtFail As TextBox

            txtPass = e.Item.FindControl("txtPassQty")
            txtFail = e.Item.FindControl("txtFailQty")

            e.Item.Cells(3).Text = objInv.GetInvVerifyItemName(dv("IV_GRN_NO"), dv("IV_INVENTORY_INDEX"), dv("IV_LOCATION_INDEX")) 'Passed Qty
            e.Item.Cells(7).Text = objInv.GetInvVerifyDetailsQty(dv("IV_GRN_NO"), dv("IV_INVENTORY_INDEX"), dv("IV_LOCATION_INDEX"), "VP") 'Passed Qty
            e.Item.Cells(8).Text = objInv.GetInvVerifyDetailsQty(dv("IV_GRN_NO"), dv("IV_INVENTORY_INDEX"), dv("IV_LOCATION_INDEX"), "VF") 'Failed Qty

        End If
        objInv = Nothing
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

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_InvList_tabs") = "<div class=""t_entity""><ul>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "InventoryVerification.aspx", "pageid=" & strPageId) & """><span>Status Update</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "InventoryVerificationListing.aspx", "pageid=" & strPageId) & """><span>Status Listing</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                            "</ul><div></div></div>"
        'Session("w_SearchGInv_tabs") = "<div class=""t_entity"">" & _
        '            "<a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Invoice", "InvList.aspx", "pageid=" & strPageId) & """><span>Issue Invoice</span></a>" & _
        '            "<a class=""t_entity_btn"" href=""" & dDispatcher.direct("Invoice", "invoiceView.aspx", "pageid=" & strPageId) & """><span>Invoice Listing</span></a>" & _
        '            "</div>"
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Dim objGlobal As New AppGlobals

        If CompareDate() Then
            ViewState("SortAscending") = "no"
            ViewState("SortExpression") = "IV_GRN_NO"
            dtgInv.CurrentPageIndex = 0
            BindInv(True)
            ChangeHeaderText()
        Else
            Common.NetMsgbox(Me, Replace(Me.lblEndDt.Text, ":", "") & " " & objGlobal.GetErrorMessage("00017") & " " & Replace(Me.lblStartDt.Text, ":", "") & ".", MsgBoxStyle.Information)
        End If
        objGlobal = Nothing

        'If intLocIndicator > 1 Then 'Sub Location is defined
        '    Me.dtgInv.Columns(3).Visible = True
        'Else
        '    Me.dtgInv.Columns(3).Visible = False
        'End If
    End Sub

    Private Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        Me.txtNo.Text = ""
        Me.txtVendor.Text = ""
        Me.txt_enddate.Text = Format(Now, "dd/MM/yyyy")
        Me.txt_startdate.Text = Format(DateAdd(DateInterval.Month, -1, Now.Date), "dd/MM/yyyy")
    End Sub
End Class