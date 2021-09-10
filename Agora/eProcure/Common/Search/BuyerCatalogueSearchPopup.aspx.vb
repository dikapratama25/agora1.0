Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions
Public Class BuyerCatalogueSearchPopup
    Inherits AgoraLegacy.AppBaseClass
    Dim objGlobal As New AppGlobals
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents dtgItem As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtItemName As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents hidVendor As System.Web.UI.HtmlControls.HtmlInputHidden

    Public Enum EnumList
        icChk = 0
        icVendItemCode = 1
        icProductCode = 2
        icItemName = 3
        icUOM = 4
        icCurrency = 5
        icLastTxnPrice = 6
        icVendor = 7
        icPrefer = 8
        ic1st = 9
        ic2nd = 10
        ic3rd = 11
        icPreferCode = 12
        ic1stCode = 13
        ic2ndCode = 14
        ic3rdCode = 15
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strNewCSS = "both"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgItem)
        hidVendor.Value = Request.QueryString("selVendor")
        hidSingleVendor.Value = Request.QueryString("selSingleVendor")
        If Request.QueryString("selVendor") <> "" Then
            Session("selVendor") = Request.QueryString("selVendor")
            Session("selSingleVendor") = Request.QueryString("selSingleVendor")
        End If

        Session("Raise") = Request.QueryString("Raise")

        If Not IsPostBack Then
            'objGlobal.FillCommodityType(Me.cboCommodityType)
            BindBuyerCat()
            'Me.cmd_back.Attributes.Add("onclick", "window.opener.document.getElementById('btnHidden1').click(); window.close(); ")
        End If

        cmdSave.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")

        Session("CurrentScreen") = "AddItem"


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
        Dim objBUYERCAT As New BuyerCat
        Dim strCommodity, strCatBuyer As String
        Dim aryProdCode As New ArrayList, i As Integer
        Dim strProdList As String = ""

        If Me.txtCommodity.Text <> "" Then
            strCommodity = Me.hidCommodity.Value
        End If
        If Me.cboCatalogueBuyer.SelectedItem.Text <> "---Select---" Then
            strCatBuyer = Me.cboCatalogueBuyer.SelectedValue
        End If

        'Get ProductCode from Session array 
        aryProdCode = Session("ProdList")
        For i = 0 To aryProdCode.Count - 1
            If strProdList = "" Then
                strProdList = "'" & aryProdCode(i) & "'"
            Else
                strProdList &= ", '" & aryProdCode(i) & "'"
            End If
        Next

        If Session("Raise") = "RaisePO" Then
            strProdList = "" 'Yap : Changed in order to allow the duplicate items
        End If

        ds = objBUYERCAT.getBuyerCatItemsFiltered(hidVendor.Value, strProdList, cboCatalogueBuyer.SelectedItem.Value, strCommodity, txtItemName.Text, True)
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

    Private Sub BindBuyerCat()
        Dim dv As DataView
        Dim cbolist As New ListItem
        Dim objcat As New BuyerCat

        dv = objcat.getBuyerCatByUser()
        cboCatalogueBuyer.Items.Clear()

        If Not dv Is Nothing Then
            cboCatalogueBuyer.Enabled = True
            Common.FillDdl(cboCatalogueBuyer, "BCM_GRP_DESC", "BCM_CAT_INDEX", dv)
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboCatalogueBuyer.Items.Insert(0, cbolist)
    End Sub

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

            Dim lnkCode As HyperLink
            lnkCode = e.Item.Cells(EnumList.icVendItemCode).FindControl("lnkCode")
            lnkCode.NavigateUrl = "javascript:;"
            Session("UrlLocation") = "BuyerCatalogueSearchPopup"

            lnkCode.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=B&pageid=" & strPageId & "&pid=" & dv("PM_PRODUCT_CODE") & "&index=&draft=0") & "')")
            lnkCode.Text = dv("PM_VENDOR_ITEM_CODE")

            Dim dvBuyerCat As DataView
            Dim lblVendor As Label = e.Item.Cells(EnumList.icVendor).FindControl("lblVendor")
            Dim strPrefer As String = e.Item.Cells(EnumList.icPrefer).Text
            Dim str1st As String = e.Item.Cells(EnumList.ic1st).Text
            Dim str2nd As String = e.Item.Cells(EnumList.ic2nd).Text
            Dim str3rd As String = e.Item.Cells(EnumList.ic3rd).Text

            lblVendor.Text = ""
            If Common.parseNull(strPrefer) <> "" And Common.parseNull(strPrefer) <> "&nbsp;" Then lblVendor.Text = strPrefer
            If Common.parseNull(str1st) <> "" And Common.parseNull(str1st) <> "&nbsp;" Then
                If lblVendor.Text <> "" Then
                    lblVendor.Text = lblVendor.Text & "<br>" & str1st
                Else
                    lblVendor.Text = str1st
                End If
            End If
            If Common.parseNull(str2nd) <> "" And Common.parseNull(str2nd) <> "&nbsp;" Then
                If lblVendor.Text <> "" Then
                    lblVendor.Text = lblVendor.Text & "<br>" & str2nd
                Else
                    lblVendor.Text = str2nd
                End If
            End If
            If Common.parseNull(str3rd) <> "" And Common.parseNull(str3rd) <> "&nbsp;" Then
                If lblVendor.Text <> "" Then
                    lblVendor.Text = lblVendor.Text & "<br>" & str3rd
                Else
                    lblVendor.Text = str3rd
                End If
            End If
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgItem.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "PM_PRODUCT_DESC"
        Bindgrid()
    End Sub

    Private Function ChkVendor(ByRef strMsg As String, ByRef aryProdCodeList As ArrayList, ByRef dtVendorList As DataTable) As Boolean
        Dim chkItem, chkItem1 As CheckBox
        Dim blnFound As Boolean
        Dim dtVendor As New DataTable
        Dim aryProdCode As New ArrayList
        Dim aryProdCodeAdd As New ArrayList
        Dim dtr As DataRow
        Dim i, j As Integer

        ' store the vendor of the current row
        Dim strCPrefer As String
        Dim strC1st As String
        Dim strC2nd As String
        Dim strC3rd As String

        ' store the vendor of the row being checked
        Dim strPPrefer As String
        Dim strP1st As String
        Dim strP2nd As String
        Dim strP3rd As String

        dtVendor.Columns.Add("Prefer", Type.GetType("System.String"))
        dtVendor.Columns.Add("1st", Type.GetType("System.String"))
        dtVendor.Columns.Add("2nd", Type.GetType("System.String"))
        dtVendor.Columns.Add("3rd", Type.GetType("System.String"))

        'Do a loop to check whether there are any common vendors
        For i = 0 To dtgItem.Items.Count() - 1
            chkItem = dtgItem.Items(i).Cells(EnumList.icChk).FindControl("chkSelection")

            If chkItem.Checked Then
                strCPrefer = ""
                strC1st = ""
                strC2nd = ""
                strC3rd = ""
                strCPrefer = IIf((dtgItem.Items(i).Cells(EnumList.icPreferCode).Text = "" Or dtgItem.Items(i).Cells(EnumList.icPreferCode).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(EnumList.icPreferCode).Text)
                strC1st = IIf((dtgItem.Items(i).Cells(EnumList.ic1stCode).Text = "" Or dtgItem.Items(i).Cells(EnumList.ic1stCode).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(EnumList.ic1stCode).Text)
                strC2nd = IIf((dtgItem.Items(i).Cells(EnumList.ic2ndCode).Text = "" Or dtgItem.Items(i).Cells(EnumList.ic2ndCode).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(EnumList.ic2ndCode).Text)
                strC3rd = IIf((dtgItem.Items(i).Cells(EnumList.ic3rdCode).Text = "" Or dtgItem.Items(i).Cells(EnumList.ic3rdCode).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(EnumList.ic3rdCode).Text)
                If (strCPrefer = "" And strC1st = "" And strC2nd = "" And strC3rd = "") Then
                    strMsg = "Please select item with vendor."
                    Return False
                    Exit Function
                End If
                If i = 0 Then 'keep track of the 1st row that is checked
                    aryProdCode.Add(dtgItem.Items(i).Cells(EnumList.icProductCode).Text)
                    'aryProdCodeAdd.Add(dtgItem.Items(i).Cells(EnumList.icProductCode).Text)
                    dtr = dtVendor.NewRow()
                    dtr("Prefer") = strCPrefer
                    dtr("1st") = strC1st
                    dtr("2nd") = strC2nd
                    dtr("3rd") = strC3rd
                    dtVendor.Rows.Add(dtr)
                Else
                    For j = 0 To i - 1 'loop thru from the 1st checked row to the row before the current row
                        chkItem1 = dtgItem.Items(j).Cells(EnumList.icChk).FindControl("chkSelection")
                        If chkItem1.Checked Then
                            strPPrefer = ""
                            strP1st = ""
                            strP2nd = ""
                            strP3rd = ""
                            strPPrefer = IIf((dtgItem.Items(j).Cells(EnumList.icPreferCode).Text = "" Or dtgItem.Items(j).Cells(EnumList.icPreferCode).Text = "&nbsp;"), "", dtgItem.Items(j).Cells(EnumList.icPreferCode).Text)
                            strP1st = IIf((dtgItem.Items(j).Cells(EnumList.ic1stCode).Text = "" Or dtgItem.Items(j).Cells(EnumList.ic1stCode).Text = "&nbsp;"), "", dtgItem.Items(j).Cells(EnumList.ic1stCode).Text)
                            strP2nd = IIf((dtgItem.Items(j).Cells(EnumList.ic2ndCode).Text = "" Or dtgItem.Items(j).Cells(EnumList.ic2ndCode).Text = "&nbsp;"), "", dtgItem.Items(j).Cells(EnumList.ic2ndCode).Text)
                            strP3rd = IIf((dtgItem.Items(j).Cells(EnumList.ic3rdCode).Text = "" Or dtgItem.Items(j).Cells(EnumList.ic3rdCode).Text = "&nbsp;"), "", dtgItem.Items(j).Cells(EnumList.ic3rdCode).Text)
                            If strPPrefer <> strCPrefer And strPPrefer <> strC1st And strPPrefer <> strC2nd And strPPrefer <> strC3rd And _
                                strP1st <> strCPrefer And strP1st <> strC1st And strP1st <> strC2nd And strP1st <> strC3rd And _
                                strP2nd <> strCPrefer And strP2nd <> strC1st And strP2nd <> strC2nd And strP2nd <> strC3rd And _
                                strP3rd <> strCPrefer And strP3rd <> strC1st And strP3rd <> strC2nd And strP3rd <> strC3rd Then
                                strMsg = "No common vendor."
                                Return False
                                Exit Function
                            End If
                        End If
                    Next
                    'keep the Product Code of the selected record into an array
                    aryProdCode.Add(dtgItem.Items(i).Cells(EnumList.icProductCode).Text)
                    'aryProdCodeAdd.Add(dtgItem.Items(i).Cells(EnumList.icProductCode).Text)

                    'keep the vendors of the selected record into a dataset
                    dtr = dtVendor.NewRow()
                    dtr("Prefer") = strCPrefer
                    dtr("1st") = strC1st
                    dtr("2nd") = strC2nd
                    dtr("3rd") = strC3rd
                    dtVendor.Rows.Add(dtr)
                End If
            End If
        Next

        dtVendorList = dtVendor
        aryProdCodeList = aryProdCode
        'Session("ProdListAdd") = aryProdCodeAdd
        Return True
    End Function

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        'Dim strMsg As String

        Dim dgItem As DataGridItem
        Dim i As Integer
        Dim chkItem As CheckBox
        Dim dtVendor As DataTable
        'Dim objPO As New PurchaseOrder
        Dim aryProdCode As New ArrayList
        Dim aryVendor As New ArrayList
        Dim strMsg As String = ""

        If ChkVendor(strMsg, aryProdCode, dtVendor) Then
            'dtPO.Columns.Add("PRODUCT_CODE", Type.GetType("System.String"))

            'For Each dgItem In dtgItem.Items
            '    chkItem = dgItem.FindControl("chkSelection")
            '    If chkItem.Checked Then
            '        '        dr = dtPO.NewRow
            '        '        dr("PRODUCT_CODE") = dgItem.Cells.Item(EnumList.icProductCode).Text
            '        '        dtPO.Rows.Add(dr)
            '        aryProdCode.Add(dgItem.Cells.Item(EnumList.icProductCode).Text)
            '        'i = i + 1
            '    End If
            'Next

            'objPO.addToShopCart(0, dtProduct)
            'Session("dt") = dtProduct

            'Get the common vendor list
            Dim strPrefer = dtVendor.Rows(0).Item("Prefer")
            Dim str1st = dtVendor.Rows(0).Item("1st")
            Dim str2nd = dtVendor.Rows(0).Item("2nd")
            Dim str3rd = dtVendor.Rows(0).Item("3rd")

            'Check whether there's only 1 record selected
            If dtVendor.Rows.Count = 1 Then
                If strPrefer <> "" Then aryVendor.Add(strPrefer)
                If str1st <> "" Then aryVendor.Add(str1st)
                If str2nd <> "" Then aryVendor.Add(str2nd)
                If str3rd <> "" Then aryVendor.Add(str3rd)
            Else
                For i = 1 To dtVendor.Rows.Count - 1
                    'Compare the Prefer vendor of the 1st row with other rows
                    If strPrefer <> "" And _
                        (strPrefer = dtVendor.Rows(i).Item("Prefer") Or _
                         strPrefer = dtVendor.Rows(i).Item("1st") Or _
                         strPrefer = dtVendor.Rows(i).Item("2nd") Or _
                         strPrefer = dtVendor.Rows(i).Item("3rd")) Then
                        If i = dtVendor.Rows.Count - 1 Then 'ie check until the last row
                            aryVendor.Add(strPrefer)
                        End If
                    End If

                    'Compare the 1st alternative vendor of the 1st row with other rows
                    If str1st <> "" And _
                        (str1st = dtVendor.Rows(i).Item("Prefer") Or _
                         str1st = dtVendor.Rows(i).Item("1st") Or _
                         str1st = dtVendor.Rows(i).Item("2nd") Or _
                         str1st = dtVendor.Rows(i).Item("3rd")) Then
                        If i = dtVendor.Rows.Count - 1 Then 'ie check until the last row
                            aryVendor.Add(str1st)
                        End If
                    End If

                    'Compare the 2nd alternative vendor of the 1st row with other rows
                    If str2nd <> "" And _
                        (str2nd = dtVendor.Rows(i).Item("Prefer") Or _
                         str2nd = dtVendor.Rows(i).Item("1st") Or _
                         str2nd = dtVendor.Rows(i).Item("2nd") Or _
                         str2nd = dtVendor.Rows(i).Item("3rd")) Then
                        If i = dtVendor.Rows.Count - 1 Then 'ie check until the last row
                            aryVendor.Add(str2nd)
                        End If
                    End If

                    'Compare the 3rd alternative vendor of the 1st row with other rows
                    If str3rd <> "" And _
                        (str3rd = dtVendor.Rows(i).Item("Prefer") Or _
                         str3rd = dtVendor.Rows(i).Item("1st") Or _
                         str3rd = dtVendor.Rows(i).Item("2nd") Or _
                         str3rd = dtVendor.Rows(i).Item("3rd")) Then
                        If i = dtVendor.Rows.Count - 1 Then 'ie check until the last row
                            aryVendor.Add(str3rd)
                        End If
                    End If
                Next
            End If

            'Combine All array
            'aryProdCode.Add(dtgItem.Items(i).Cells(EnumList.icProductCode).Text)
            Dim aryProdCodeCurrent As ArrayList = Session("ProdList")

            'If IsNumeric(hidSingleVendor.Value) AndAlso Not hidSingleVendor.Value = 1 Then
            Dim aryVendorCurrent As ArrayList = Session("VendorList")
            'For i = 0 To aryProdCodeCurrent.Count - 1
            '    aryProdCode.Add(aryProdCodeCurrent(i))
            'Next

            For i = 0 To aryProdCode.Count - 1
                aryProdCodeCurrent.Add(aryProdCode(i))
            Next

            For i = 0 To aryVendorCurrent.Count - 1
                aryVendor.Add(aryVendorCurrent(i))
            Next
            'End If


            Session("ProdList") = aryProdCodeCurrent 'aryProdCode
            Session("VendorList") = aryVendor
            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
            cmdSearch_Click(cmdSearch, New System.EventArgs())
        Else
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
        End If
    End Sub
End Class