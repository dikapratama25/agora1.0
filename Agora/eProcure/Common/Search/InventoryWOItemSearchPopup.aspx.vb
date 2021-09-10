Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions
Public Class InventoryWOItemSearchPopup
    Inherits AgoraLegacy.AppBaseClass
    Dim objGlobal As New AppGlobals
    Dim objINV As New Inventory
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim aryInvReq, aryInvTrans, aryWO As New ArrayList()
    Dim Type As String

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgItem)

        Type = Me.Request.QueryString("type")

        If Not IsPostBack Then
            'LoadLoc()
            'Me.ddl_SubLoc.Items.Insert(0, "---Select---")
            'Me.cmd_back.Attributes.Add("onclick", "window.opener.document.getElementById('btnHidden1').click(); window.close(); ")
            Me.cmd_back.Attributes.Add("onclick", "window.close(); ")
            Populate()
        End If

        cmdSave.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")

    End Sub

    'Private Sub LoadLoc()
    '    Dim objInv As New Inventory
    '    Dim ds As New DataSet

    '    ds = objInv.PopLocation

    '    Me.ddl_Loc.Items.Clear()
    '    Me.ddl_Loc.DataSource = ds.Tables(0)
    '    Me.ddl_Loc.DataTextField = "LM_LOCATION"
    '    Me.ddl_Loc.DataBind()
    '    Me.ddl_Loc.Items.Insert(0, "---Select---")

    '    objInv = Nothing

    'End Sub

    'Private Sub ddl_Loc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Loc.SelectedIndexChanged
    '    Dim objInv As New Inventory
    '    Dim ds As New DataSet

    '    ds = objInv.PopLocation(Me.ddl_Loc.SelectedItem.Text)

    '    Me.ddl_SubLoc.Items.Clear()
    '    Me.ddl_SubLoc.DataSource = ds.Tables(0)
    '    Me.ddl_SubLoc.DataTextField = "LM_SUB_LOCATION"
    '    Me.ddl_SubLoc.DataValueField = "LM_SUB_LOCATION"
    '    Me.ddl_SubLoc.DataBind()
    '    Me.ddl_SubLoc.Items.Insert(0, "---Select---")

    '    objInv = Nothing
    'End Sub

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
        Dim strLoc, strSubLoc As String
        strLoc = ""
        strSubLoc = ""
        'If Me.ddl_Loc.SelectedItem.Text <> "---Select---" Then
        '    strLoc = Me.ddl_Loc.SelectedValue
        'End If
        'If Me.ddl_SubLoc.SelectedItem.Text <> "---Select---" Then
        '    strSubLoc = Me.ddl_SubLoc.SelectedValue
        'End If

        ds = objINV.getInventoryItemFilteredWO(Me.txtItemCode.Text, Me.txtItemName.Text, "", "")
        Dim dvViewItem As DataView
        dvViewItem = ds.Tables(0).DefaultView

        dvViewItem.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewItem.Sort += " DESC"

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count
        If ViewState("intPageRecordCnt") > 0 Then
            cmdSave.Visible = True
            resetDatagridPageIndex(dtgItem, dvViewItem)

            dtgItem.DataSource = dvViewItem
            dtgItem.DataBind()
        Else

            cmdSave.Visible = False
            dtgItem.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtgItem.PageCount
    End Function

    Private Sub dtgItem_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgItem, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dtgItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            'Dim strPrefer As String = e.Item.Cells(EnumList.icPrefer).Text
            'Dim str1st As String = e.Item.Cells(EnumList.ic1st).Text
            'Dim str2nd As String = e.Item.Cells(EnumList.ic2nd).Text
            'Dim str3rd As String = e.Item.Cells(EnumList.ic3rd).Text

        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgItem.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "IM_ITEM_CODE"
        Bindgrid()
    End Sub

    Sub Populate()
        Dim typeahead As String
        Dim content As String = ""
        Dim nametypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=name")
        Dim codetypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=code")


        content &= "$(""#txtItemName"").autocomplete(""" & nametypeahead & """, {" & vbCrLf & _
        "width: 260," & vbCrLf & _
        "scroll: true," & vbCrLf & _
        "selectFirst: false" & vbCrLf & _
        "});" & vbCrLf & _
        "$(""#txtItemName"").result(function(event, data, formatted) {" & vbCrLf & _
        "if (data)" & vbCrLf & _
        "$(""#txtItemCode"").val(data[1]);" & vbCrLf & _
        "});" & vbCrLf & _
        "$(""#txtItemCode"").autocomplete(""" & codetypeahead & """, {" & vbCrLf & _
        "width: 260," & vbCrLf & _
        "scroll: true," & vbCrLf & _
        "selectFirst: false" & vbCrLf & _
        "});" & vbCrLf & _
        "$(""#txtItemCode"").result(function(event, data, formatted) {" & vbCrLf & _
        "if (data)" & vbCrLf & _
        "$(""#txtItemName"").val(data[1]);" & vbCrLf & _
        "});" & vbCrLf

        typeahead = "<script language=""javascript"">" & vbCrLf & _
          "<!--" & vbCrLf & _
            "$(document).ready(function(){" & vbCrLf & _
            content & vbCrLf & _
            "});" & vbCrLf & _
            "-->" & vbCrLf & _
            "</script>"
        Session("typeahead") = typeahead
    End Sub

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        'Dim strMsg As String

        'Dim dgItem As DataGridItem
        Dim i, j As Integer
        Dim chkItem As CheckBox
        Dim ItemCode, ItemName, ItemQty, ItemLoc, ItemLot, ItemSubLoc As String
        'Dim dtVendor As DataTable
        ''Dim objPO As New PurchaseOrder
        'Dim aryProdCode As New ArrayList
        'Dim aryVendor As New ArrayList
        'Dim strMsg As String = ""

        For i = 0 To dtgItem.Items.Count() - 1
            chkItem = dtgItem.Items(i).Cells(0).FindControl("chkSelection")

            If chkItem.Checked Then
                If Type <> "trans" And Type <> "WO" Then

                    aryInvReq = Session("aryInvReq")
                    For j = 0 To aryInvReq.Count - 1
                        ItemCode = IIf((dtgItem.Items(i).Cells(1).Text = "" Or dtgItem.Items(i).Cells(1).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(1).Text)
                        ItemName = IIf((dtgItem.Items(i).Cells(2).Text = "" Or dtgItem.Items(i).Cells(2).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(2).Text)
                        'ItemQty = IIf((dtgItem.Items(i).Cells(5).Text = "" Or dtgItem.Items(i).Cells(5).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(5).Text)
                        ItemLoc = IIf((dtgItem.Items(i).Cells(3).Text = "" Or dtgItem.Items(i).Cells(3).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(3).Text)
                        ItemSubLoc = IIf((dtgItem.Items(i).Cells(4).Text = "" Or dtgItem.Items(i).Cells(4).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(4).Text)
                        If aryInvReq(j)(0) = "" Or aryInvReq(j)(0) = "---Select---" Then
                            aryInvReq(j)(0) = ItemCode
                            aryInvReq(j)(1) = ItemName
                            aryInvReq(j)(2) = ""
                            aryInvReq(j)(3) = ItemLoc
                            aryInvReq(j)(4) = ItemSubLoc
                            Exit For
                        ElseIf j = aryInvReq.Count - 1 Then
                            aryInvReq.Add(New String() {ItemCode, ItemName, ItemQty, ItemLoc, ItemSubLoc, ""})
                            Exit For
                        End If
                    Next
                ElseIf Type = "WO" Then

                    aryWO = Session("aryWO")
                    For j = 0 To aryWO.Count - 1
                        ItemCode = IIf((dtgItem.Items(i).Cells(1).Text = "" Or dtgItem.Items(i).Cells(1).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(1).Text)
                        ItemName = IIf((dtgItem.Items(i).Cells(2).Text = "" Or dtgItem.Items(i).Cells(2).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(2).Text)
                        ItemLot = IIf((dtgItem.Items(i).Cells(3).Text = "" Or dtgItem.Items(i).Cells(3).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(3).Text)
                        ItemLoc = IIf((dtgItem.Items(i).Cells(4).Text = "" Or dtgItem.Items(i).Cells(4).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(4).Text)
                        ItemSubLoc = IIf((dtgItem.Items(i).Cells(5).Text = "" Or dtgItem.Items(i).Cells(5).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(5).Text)
                        If aryWO(j)(0) = "" Or aryWO(j)(0) = "---Select---" Then
                            aryWO(j)(0) = ItemCode
                            aryWO(j)(1) = ItemName
                            aryWO(j)(2) = ""
                            aryWO(j)(3) = ItemLot
                            aryWO(j)(4) = ItemLoc
                            aryWO(j)(5) = ItemSubLoc
                            Exit For
                        ElseIf j = aryWO.Count - 1 Then
                            aryWO.Add(New String() {ItemCode, ItemName, ItemQty, ItemLot, ItemLoc, ItemSubLoc, ""})
                            Exit For
                        End If
                    Next
                Else
                    aryInvTrans = Session("aryInvTrans")
                    For j = 0 To aryInvTrans.Count - 1
                        ItemCode = IIf((dtgItem.Items(i).Cells(1).Text = "" Or dtgItem.Items(i).Cells(1).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(1).Text)
                        ItemName = IIf((dtgItem.Items(i).Cells(2).Text = "" Or dtgItem.Items(i).Cells(2).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(2).Text)
                        'ItemQty = IIf((dtgItem.Items(i).Cells(5).Text = "" Or dtgItem.Items(i).Cells(5).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(5).Text)
                        ItemLoc = IIf((dtgItem.Items(i).Cells(3).Text = "" Or dtgItem.Items(i).Cells(3).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(3).Text)
                        ItemSubLoc = IIf((dtgItem.Items(i).Cells(4).Text = "" Or dtgItem.Items(i).Cells(4).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(4).Text)
                        If aryInvTrans(j)(0) = "" Or aryInvTrans(j)(0) = "---Select---" Then
                            aryInvTrans(j)(0) = ItemCode
                            aryInvTrans(j)(1) = ItemName
                            aryInvTrans(j)(2) = ""
                            aryInvTrans(j)(3) = ItemLoc
                            aryInvTrans(j)(4) = ItemSubLoc
                            Exit For
                        ElseIf j = aryInvTrans.Count - 1 Then
                            aryInvTrans.Add(New String() {ItemCode, ItemName, ItemQty, ItemLoc, ItemSubLoc, "---Select---", "---Select---", ""})
                            Exit For
                        End If
                    Next
                End If
            End If
        Next
        Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00003"), MsgBoxStyle.Information)
    End Sub
End Class