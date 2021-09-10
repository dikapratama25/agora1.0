Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing
Imports System.IO
Public Class InventoryVerification
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Public Enum EnumInv
        icGRNNoLink
        icCoyName
        icPoNo
        icDoNo
        icCreationDate
        icReceivedDate
        icIndex
    End Enum
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
            ViewState("SortAscending") = "no"
            ViewState("SortExpression") = "IV_GRN_NO"
            dtgInv.CurrentPageIndex = 0
            BindInv(True)
        End If
    End Sub

    Private Sub BindInv(Optional ByVal pSorted As Boolean = False)
        Dim objInv As New Inventory
        Dim ds As DataSet
        Dim dvViewInv As DataView

        ds = objInv.GetInvVerify
        dvViewInv = ds.Tables(0).DefaultView
        dvViewInv.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewInv.Sort += " DESC"

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgInv, dvViewInv)
            dtgInv.DataSource = dvViewInv
            dtgInv.DataBind()
            'Else
            '    dtgInv.DataBind()
            'Common.NetMsgbox(Me, MsgNoRecord)
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
        Session("w_InvList_tabs") = "<div class=""t_entity""><ul>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "InventoryVerification.aspx", "pageid=" & strPageId) & """><span>Status Update</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "InventoryVerificationListing.aspx", "pageid=" & strPageId) & """><span>Status Listing</span></a></li>" & _
                            "<li><div class=""space""></div></li>" & _
                            "</ul><div></div></div>"
        'Session("w_SearchGInv_tabs") = "<div class=""t_entity"">" & _
        '            "<a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Invoice", "InvList.aspx", "pageid=" & strPageId) & """><span>Issue Invoice</span></a>" & _
        '            "<a class=""t_entity_btn"" href=""" & dDispatcher.direct("Invoice", "invoiceView.aspx", "pageid=" & strPageId) & """><span>Invoice Listing</span></a>" & _
        '            "</div>"
    End Sub

    Private Sub dtgInv_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInv.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgInv, e)
    End Sub

    Private Sub dtgInv_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInv.ItemDataBound
        Dim objDB As New EAD.DBCom

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkGRNNo As HyperLink
            lnkGRNNo = e.Item.Cells(EnumInv.icGRNNoLink).FindControl("lnkGRNNo")
            lnkGRNNo.NavigateUrl = dDispatcher.direct("Inventory", "InventoryVerificationDetails.aspx", "Frm=InventoryVerification&Mode=New&GRNNo=" & dv("IV_GRN_NO") & "&Vendor=" & dv("CM_COY_NAME") & "&GRNDate=" & Common.FormatWheelDate(WheelDateFormat.LongDate, dv("GM_CREATED_DATE")) & "&ReceivedDate=" & Common.FormatWheelDate(WheelDateFormat.LongDate, dv("GM_DATE_RECEIVED")) & "&pageid=" & strPageId)
            lnkGRNNo.Text = dv("IV_GRN_NO")
            e.Item.Cells(EnumInv.icCoyName).Text = dv("CM_COY_NAME")
            e.Item.Cells(EnumInv.icPoNo).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewPO.aspx", "pageid=" & strPageId & "&PO_No=" & Common.parseNull(dv("POM_PO_NO")) & "&BCoyID=" & dv("IM_COY_ID")) & "')"")><font color=#0000ff>" & dv("POM_PO_NO") & "</font></A>"
            'e.Item.Cells(EnumInv.icDoNo).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewDO.aspx", "pageid=" & strPageId & "&DONO=" & dv("DOM_DO_NO") & "&SCoyID=" & dv("GM_S_COY_ID") & "&PO_NO=" & dv("POM_PO_NO")) & "')"" ><font color=#0000ff>" & dv("DOM_DO_NO") & "</font></A>"
            e.Item.Cells(EnumInv.icDoNo).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("DO", "PreviewDODetails.aspx", "caller=" & "buyer" & "&DONo=" & dv("DOM_DO_NO") & "&POIdx=" & dv("POM_PO_INDEX") & "&SCoyID=" & dv("GM_S_COY_ID") & "&PageID=" & strPageId) & "')"" ><font color=#0000ff>" & dv("DOM_DO_NO") & "</font></A>"
            e.Item.Cells(EnumInv.icCreationDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("GM_CREATED_DATE"))
            e.Item.Cells(EnumInv.icReceivedDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("GM_DATE_RECEIVED"))
            e.Item.Cells(EnumInv.icIndex).Text = dv("IV_VERIFY_INDEX")
        End If
    End Sub

    Private Sub dtgInv_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgInv.PageIndexChanged
        dtgInv.CurrentPageIndex = e.NewPageIndex
        BindInv(True)
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgInv.SortCommand
        Grid_SortCommand(sender, e)
        dtgInv.CurrentPageIndex = 0
        BindInv(True)
    End Sub
End Class