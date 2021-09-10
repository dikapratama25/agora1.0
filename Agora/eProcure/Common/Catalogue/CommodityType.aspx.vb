Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions
Partial Public Class CommodityType
    Inherits AgoraLegacy.AppBaseClass
    Dim objGlobal As New AppGlobals
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgItem)

        If IsPostBack Then

            'Me.cmd_back.Attributes.Add("onclick", "window.opener.document.getElementById('btnHidden1').click(); window.close(); ")
        End If

    End Sub
    Public Sub MyDataGrid_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgItem.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub
    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgItem.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        If Page.IsValid Then
            dtgItem.CurrentPageIndex = 0
            ViewState("SortAscending") = "yes"
            ViewState("SortExpression") = "Level_1"
            Bindgrid()
        End If
        
    End Sub

    Private Sub Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False)
        Dim ds As DataSet = New DataSet
        Dim objCommType As New CommType
        Dim sSearch As String = ""

        sSearch = Me.txtSearch.Text

        ds = objCommType.searchBuyerCatItems(Me.txtSearch.Text)
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
    End Sub

    Private Sub dtgItem_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgItem, e)
    End Sub

    Private Sub dtgItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkCode As HyperLink
            lnkCode = e.Item.Cells(0).FindControl("lnkCode")
            lnkCode.NavigateUrl = "javascript:;"

            lnkCode.Attributes.Add("onclick", " pass_target('" & dv("CT_ID") & "', '" & e.Item.Cells(4).Text & "'); ")
            lnkCode.Text = dv("CT_CODE")

            'Dim dvBuyerCat As DataView
            'Dim lblVendor As Label = e.Item.Cells(EnumList.icVendor).FindControl("lblVendor")
            'Dim strPrefer As String = e.Item.Cells(EnumList.icPrefer).Text
            'Dim str1st As String = e.Item.Cells(EnumList.ic1st).Text
            'Dim str2nd As String = e.Item.Cells(EnumList.ic2nd).Text
            'Dim str3rd As String = e.Item.Cells(EnumList.ic3rd).Text

            'lblVendor.Text = ""
            'If Common.parseNull(strPrefer) <> "" And Common.parseNull(strPrefer) <> "&nbsp;" Then lblVendor.Text = strPrefer
            'If Common.parseNull(str1st) <> "" And Common.parseNull(str1st) <> "&nbsp;" Then
            '    If lblVendor.Text <> "" Then
            '        lblVendor.Text = lblVendor.Text & "<br>" & str1st
            '    Else
            '        lblVendor.Text = str1st
            '    End If
            'End If
            'If Common.parseNull(str2nd) <> "" And Common.parseNull(str2nd) <> "&nbsp;" Then
            '    If lblVendor.Text <> "" Then
            '        lblVendor.Text = lblVendor.Text & "<br>" & str2nd
            '    Else
            '        lblVendor.Text = str2nd
            '    End If
            'End If
            'If Common.parseNull(str3rd) <> "" And Common.parseNull(str3rd) <> "&nbsp;" Then
            '    If lblVendor.Text <> "" Then
            '        lblVendor.Text = lblVendor.Text & "<br>" & str3rd
            '    Else
            '        lblVendor.Text = str3rd
            '    End If
            'End If
        End If
    End Sub

End Class