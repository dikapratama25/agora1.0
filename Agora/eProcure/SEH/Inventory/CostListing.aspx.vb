Imports AgoraLegacy
Imports eProcure.Component

Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient


Public Class CostListingSEH
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim intLocIndicator As Integer
    Dim strLoc As String
    Dim strSLoc As String
    Dim subLoc As String
    Dim objINV As New Inventory

    'Protected WithEvents optSort As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents optSort1 As System.Web.UI.WebControls.RadioButton
    Public Enum EnumCL
        ItemCode = 0
        ItemName = 1
        Ddate = 2
        OpenQty = 3
        OpenUP = 4
        OpenTValue = 5
        ReceiveQty = 6
        ReceiveUP = 7
        ReceiveTValue = 8
        IssueQty = 9
        IssueUp = 10
        IssueTValue = 11
        ClosingQty = 12
        ClosingUP = 13
        ClosingTValue = 14
        TransType = 15

    End Enum

    'Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
    '    CheckButtonAccess(True)
    'End Sub

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

        If Not Page.IsPostBack Then
            GenerateTab()
        End If

        strPageId = Session("strPageId")
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCos)

        If Not Page.IsPostBack Then
            txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
            hidDateS.Value = txt_startdate.Text
            txt_enddate.Text = DateTime.Now.ToShortDateString()
            hidDateE.Value = txt_enddate.Text
            Session("strURL") = strCallFrom
            'ChangeHeaderText()
        End If

    End Sub

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = Request("optSort") '"IM_ITEM_CODE"

        dtgCos.CurrentPageIndex = 0
        Bindgrid(True)
        'ChangeHeaderText()
        'Me.optSort.Items.FindByValue(ViewState("SortExpression")).Selected = True
        Me.cmdPrint.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewCostListing.aspx", "ItemCode=" & Me.txtItemCode.Text & "&ItemName=" & Me.txtItemName.Text & "&strStartDate=" & Me.txt_startdate.Text & "&strEndDate=" & Me.txt_enddate.Text & "&strInvType=" & Me.ddl_InvType.SelectedItem.Value & "&CoyID=" & Session("CompanyId")) & "')")
        'Me.cmdPrint.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewITSlip.aspx", "ITNo=IT000006") & "')")

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String

        Dim strMsg As String
        Dim objGlobal As New AppGlobals
        Dim comparedt As Date
        comparedt = DateAdd("m", -6, CDate(txt_enddate.Text))

        If CDate(txt_startdate.Text) < comparedt Then
            strMsg = "Start date " & objGlobal.GetErrorMessage("00015") & " 6 months"
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            Return False
        End If

        Dim ds As New DataSet
        Dim objInventory As New Inventory_Ext

        ds = objInventory.GetCostDetails(Me.txtItemCode.Text, Me.txtItemName.Text, Me.txt_startdate.Text, Me.txt_enddate.Text, Me.ddl_InvType.SelectedItem.Value)

        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView

        dvViewPR.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        If ViewState("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtgCos, dvViewPR)
            dtgCos.DataSource = dvViewPR
            dtgCos.DataBind()
        Else
            dtgCos.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtgCos.PageCount
        objINV = Nothing

    End Function

    Private Sub dtgCos_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCos.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgCos, e)
        'ChangeHeaderText()

        'If e.Row.RowType = DataControlRowType.DataRow Then

        '    Dim keyValue As String = grdOrderItems.DataKeys.Item(e.Row.DataItemIndex).Value

        '    Dim price As Decimal = 0

        'End If

    End Sub

    Private Sub dtgCos_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCos.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dvMatch As DataView = dv.DataView

            Select Case e.Item.Cells(EnumCL.TransType).Text
                Case "GRN"
                    e.Item.Cells(EnumCL.TransType).Text = "Good Received Note"
                Case "RO"
                    e.Item.Cells(EnumCL.TransType).Text = "Return Outward"
                Case "II"
                    e.Item.Cells(EnumCL.TransType).Text = "Inventory Issue"
                Case "RI"
                    e.Item.Cells(EnumCL.TransType).Text = "Return Inward"
                Case "WO"
                    e.Item.Cells(EnumCL.TransType).Text = "Write Off"
                Case "IIC"
                    e.Item.Cells(EnumCL.TransType).Text = "Inventory Issue Cancel"
                Case "WOC"
                    e.Item.Cells(EnumCL.TransType).Text = "Write Off Cancel"

            End Select
        End If
    End Sub

    Private Sub dtgCos_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCos.PageIndexChanged
        dtgCos.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
        'ChangeHeaderText()
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgCos.SortCommand
        Grid_SortCommand(sender, e)
        dtgCos.CurrentPageIndex = 0
        Bindgrid(True)
        'ChangeHeaderText()
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_SearchInvListing_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "InventoryListing.aspx", "pageid=" & strPageId) & """><span>Inventory Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "CostListing.aspx", "pageid=" & strPageId) & """><span>Cost Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"

    End Sub

    Private Sub InventoryListing_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.cmdPrint.Disabled = True
        If ViewState("intPageRecordCnt") > 0 Then
            Me.cmdPrint.Disabled = False
        End If
    End Sub
    Private Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        'clearTextBox()
        Me.txt_startdate.Text = DateAdd("m", -6, DateTime.Now.ToShortDateString())
        Me.txt_enddate.Text = DateTime.Now.ToShortDateString()
        Me.txtItemCode.Text = ""
        Me.txtItemName.Text = ""
        Me.ddl_InvType.SelectedIndex = 0
    End Sub


End Class