'Copyright © 2011 STRATEQ GLOBAL SERVICES. All rights reserved.
Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions
Public Class InventoryReqListFTN
    Inherits AgoraLegacy.AppBaseClass
    Dim objGlobal As New AppGlobals
    Dim objINV As New Inventory
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim aryInvReq As New ArrayList()

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgItem)
        If Not IsPostBack Then
            GenerateTab()
        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_InventoryReq_tabs") = "<div class=""t_entity""><ul>" & _
                                    "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "InventoryReq.aspx", "pageid=" & strPageId) & """><span>Inventory Requisition</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "InventoryReqList.aspx", "") & """><span>Inventory Requisition Listing</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                   "</ul><div></div></div>"


    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgItem.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Public Sub MyDataGrid_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgItem.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim ds As DataSet = New DataSet

        ds = objINV.getInventoryTransFiltered(Me.txtIRNumber.Text, Me.txtIssue.Text, Me.txt_startdate.Text, Me.txt_enddate.Text, Me.txtDepartment.Text)
        Dim dvViewItem As DataView
        dvViewItem = ds.Tables(0).DefaultView

        dvViewItem.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewItem.Sort += " DESC"

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count
        If ViewState("intPageRecordCnt") > 0 Then
            resetDatagridPageIndex(dtgItem, dvViewItem)

            dtgItem.DataSource = dvViewItem
            dtgItem.DataBind()
        Else
            dtgItem.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtgItem.PageCount
    End Function

    Private Sub dtgItem_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgItem, e)
    End Sub

    Private Sub dtgItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkCode As HyperLink
            Dim IRNo, IRDate, IssueTo, RefNo, Remark, Department As String

            e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("IT_TRANS_DATE"))

            IRNo = Common.parseNull(dv("IT_TRANS_REF_NO"))
            IRDate = Common.parseNull(dv("IT_TRANS_DATE"))
            IssueTo = Common.parseNull(dv("IT_ADDITION_INFO"))
            Department = Common.parseNull(dv("IT_ADDITION_INFO1"))
            RefNo = Common.parseNull(dv("IT_REF_NO"))
            Remark = Common.parseNull(dv("IT_REMARK"))

            lnkCode = e.Item.Cells(0).FindControl("lnkCode")
            lnkCode.NavigateUrl = "" & dDispatcher.direct("Inventory", "InventoryReqInfo.aspx", "IRNo=" & Server.UrlEncode(IRNo) & "&IRDate=" & IRDate & "&IssueTo=" & Server.UrlEncode(IssueTo) & "&RefNo=" & Server.UrlEncode(RefNo) & "&Remark=" & Server.UrlEncode(Remark) & "&department=" & Server.UrlEncode(Department) & "")
            'lnkCode.NavigateUrl = "javascript:;"
            'Session("UrlLocation") = "InventoryReqList"

            'lnkCode.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=B&pageid=" & strPageId & "&pid=" & dv("IT_TRANS_REF_NO") & "&index=&draft=0") & "')")
            lnkCode.Text = dv("IT_TRANS_REF_NO")

        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgItem.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "IT_TRANS_REF_NO"
        Bindgrid()
    End Sub
End Class