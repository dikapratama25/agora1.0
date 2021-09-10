Imports AgoraLegacy
Imports eProcure.Component

Public Class BuyerCatSearch
    Inherits AgoraLegacy.AppBaseClass
    Dim objGlobal As New AppGlobals
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDB As New EAD.DBCom
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

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strNewCSS = "both"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgItem)
        If Not Page.IsPostBack Then
            'objGlobal.FillCommodityType(Me.cboCommodityType)
            'Me.dtgItem.CurrentPageIndex = 0
            BindBuyerCat()
        End If

        cmdPR.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
        Session("urlreferer") = "BuyerCatSearch"
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
        Dim i As Integer = 0
        Dim strItem As String = ""
        Dim aryItemType As New ArrayList

        If Me.txtCommodity.Text <> "" Then
            strCommodity = Me.hidCommodity.Value
        End If
        If Me.cboCatalogueBuyer.SelectedItem.Text <> "---Select---" Then
            strCatBuyer = Me.cboCatalogueBuyer.SelectedValue
        End If
        If Me.chkSpot.Checked = True Then
            aryItemType.Add("SP")
        End If

        If Me.chkStock.Checked = True Then
            aryItemType.Add("ST")
        End If

        If Me.chkMRO.Checked = True Then
            aryItemType.Add("MI")
        End If

        If cboCatalogueBuyer.SelectedItem.Text <> "---Select---" Then
            ds = objBUYERCAT.getBuyerCatItems1(cboCatalogueBuyer.SelectedItem.Value, strCommodity, txtItemName.Text, True, txtItemCode.Text, aryItemType)

        Else
            If cboCatalogueBuyer.Items.Count > 1 Then
                For i = 1 To cboCatalogueBuyer.Items.Count - 1
                    strItem = strItem & cboCatalogueBuyer.Items(i).Value & ","
                Next
                strItem = Mid(strItem, 1, Len(strItem) - 1)
            Else
                strItem = "0"
            End If
            ds = objBUYERCAT.getBuyerCatItemsByCombo1(strItem, strCommodity, txtItemName.Text, True, txtItemCode.Text, aryItemType)

        End If
        'ds = objBUYERCAT.getBuyerCatItems(cboCatalogueBuyer.SelectedItem.Value, strCommodity, txtItemName.Text, True)
        Dim dvViewItem As DataView
        dvViewItem = ds.Tables(0).DefaultView

        dvViewItem.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewItem.Sort += " DESC"

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count
        If ViewState("intPageRecordCnt") > 0 Then
            cmdPR.Visible = True
            resetDatagridPageIndex(dtgItem, dvViewItem)

            dtgItem.DataSource = dvViewItem
            dtgItem.DataBind()
        Else
            cmdPR.Visible = False
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
            Session("UrlLocation") = "BuyerCatSearch"
            lnkCode.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=B&pageid=" & strPageId & "&pid=" & dv("PM_PRODUCT_CODE") & "&index=&draft=0") & "')")
            lnkCode.Text = dv("PM_VENDOR_ITEM_CODE")

            If Common.parseNull(dv("PM_LAST_TXN_TAX")) = 0 Then
                e.Item.Cells(7).Text = ""
            End If

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

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "PM_PRODUCT_DESC"
        Bindgrid()
    End Sub

    Private Function checkMandatory(ByRef strMsg As String) As Boolean
        strMsg = ""
        Dim objPR As New PR
        Dim intBCM As String
        intBCM = CInt(objPR.checkBCM)
        If intBCM > 0 Then
            If Not objPR.checkUserAccExist() Then
                strMsg = "You are not assigned to any Budget Account Code. ""&vbCrLf&""Please contact the Finance Manager. "
            End If
        End If
        If strMsg <> "" Then
            checkMandatory = False
        Else
            checkMandatory = True
        End If
        objPR = Nothing
    End Function

    Private Sub cmdPR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPR.Click
        'Dim strMsg As String
        Dim dgItem As DataGridItem
        Dim i, j As Integer
        'Dim chkItem As CheckBox
        'Dim dtVendor As DataTable
        'Dim objPO As New PurchaseOrder
        Dim aryProdCode As New ArrayList
        'Dim aryVendor As New ArrayList
        Dim strMsg As String = ""

        'If ChkVendor(strMsg, aryProdCode, dtVendor) Then
        If checkMandatory(strMsg) Then
            If ChkVendor(strMsg, aryProdCode) Then
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

                ''Get the common vendor list
                'Dim strPrefer = dtVendor.Rows(0).Item("Prefer")
                'Dim str1st = dtVendor.Rows(0).Item("1st")
                'Dim str2nd = dtVendor.Rows(0).Item("2nd")
                'Dim str3rd = dtVendor.Rows(0).Item("3rd")

                ''Check whether there's only 1 record selected
                'If dtVendor.Rows.Count = 1 Then
                '    If strPrefer <> "" Then aryVendor.Add(strPrefer)
                '    If str1st <> "" Then aryVendor.Add(str1st)
                '    If str2nd <> "" Then aryVendor.Add(str2nd)
                '    If str3rd <> "" Then aryVendor.Add(str3rd)
                'Else
                '    For j = 0 To dtVendor.Rows.Count - 1
                '        Dim strPreferCheck As Boolean = True
                '        Dim str1stCheck As Boolean = True
                '        Dim str2ndCheck As Boolean = True
                '        Dim str3rdCheck As Boolean = True

                '        strPrefer = dtVendor.Rows(j).Item("Prefer")
                '        str1st = dtVendor.Rows(j).Item("1st")
                '        str2nd = dtVendor.Rows(j).Item("2nd")
                '        str3rd = dtVendor.Rows(j).Item("3rd")

                '        For i = 0 To dtVendor.Rows.Count - 1
                '            If i <> j Then
                '                'Compare the Prefer vendor of the 1st row with other rows
                '                If strPrefer <> "" And _
                '                    (strPrefer = dtVendor.Rows(i).Item("Prefer") Or _
                '                     strPrefer = dtVendor.Rows(i).Item("1st") Or _
                '                     strPrefer = dtVendor.Rows(i).Item("2nd") Or _
                '                     strPrefer = dtVendor.Rows(i).Item("3rd")) Then
                '                Else
                '                    strPreferCheck = False
                '                    'If i = dtVendor.Rows.Count - 1 Then 'ie check until the last row
                '                    '    aryVendor.Add(strPrefer)
                '                    'End If
                '                End If

                '                'Compare the 1st alternative vendor of the 1st row with other rows
                '                If str1st <> "" And _
                '                    (str1st = dtVendor.Rows(i).Item("Prefer") Or _
                '                     str1st = dtVendor.Rows(i).Item("1st") Or _
                '                     str1st = dtVendor.Rows(i).Item("2nd") Or _
                '                     str1st = dtVendor.Rows(i).Item("3rd")) Then
                '                Else
                '                    str1stCheck = False
                '                    'If i = dtVendor.Rows.Count - 1 Then 'ie check until the last row
                '                    '    aryVendor.Add(str1st)
                '                    'End If
                '                End If

                '                'Compare the 2nd alternative vendor of the 1st row with other rows
                '                If str2nd <> "" And _
                '                    (str2nd = dtVendor.Rows(i).Item("Prefer") Or _
                '                     str2nd = dtVendor.Rows(i).Item("1st") Or _
                '                     str2nd = dtVendor.Rows(i).Item("2nd") Or _
                '                     str2nd = dtVendor.Rows(i).Item("3rd")) Then
                '                Else
                '                    str2ndCheck = False
                '                    'If i = dtVendor.Rows.Count - 1 Then 'ie check until the last row
                '                    '    aryVendor.Add(str2nd)
                '                    'End If
                '                End If

                '                'Compare the 3rd alternative vendor of the 1st row with other rows
                '                If str3rd <> "" And _
                '                    (str3rd = dtVendor.Rows(i).Item("Prefer") Or _
                '                     str3rd = dtVendor.Rows(i).Item("1st") Or _
                '                     str3rd = dtVendor.Rows(i).Item("2nd") Or _
                '                     str3rd = dtVendor.Rows(i).Item("3rd")) Then
                '                Else
                '                    str3rdCheck = False
                '                    'If i = dtVendor.Rows.Count - 1 Then 'ie check until the last row
                '                    '    aryVendor.Add(str3rd)
                '                    'End If
                '                End If
                '            End If
                '        Next
                '        If strPreferCheck = True Then aryVendor.Add(strPrefer)
                '        If str1stCheck = True Then aryVendor.Add(str1st)
                '        If str2ndCheck = True Then aryVendor.Add(str2nd)
                '        If str3rdCheck = True Then aryVendor.Add(str3rd)

                '    Next

                'End If
                Session("ProdList") = aryProdCode
                'Session("VendorList") = aryVendor
                'If aryVendor.Count = 0 Then
                '    strMsg = "No common vendor."
                '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                '    Exit Sub
                'End If
                Response.Redirect(dDispatcher.direct("PR", "RaisePR.aspx", "frm=BuyerCat&pageid=" & strPageId))
            Else
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            End If
        Else
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
        End If

    End Sub

    'Private Function ChkVendor(ByRef strMsg As String, ByRef aryProdCodeList As ArrayList, ByRef dtVendorList As DataTable) As Boolean
    Private Function ChkVendor(ByRef strMsg As String, ByRef aryProdCodeList As ArrayList) As Boolean

        Dim chkItem, chkItem1 As CheckBox
        Dim blnFound As Boolean
        'Dim dtVendor As New DataTable
        Dim aryProdCode As New ArrayList
        Dim dtr As DataRow
        Dim i, j As Integer

        '' store the vendor of the current row
        'Dim strCPrefer As String
        'Dim strC1st As String
        'Dim strC2nd As String
        'Dim strC3rd As String

        '' store the vendor of the row being checked
        'Dim strPPrefer As String
        'Dim strP1st As String
        'Dim strP2nd As String
        'Dim strP3rd As String

        Dim strVendor As String
        Dim strsql As String

        'dtVendor.Columns.Add("Prefer", Type.GetType("System.String"))
        'dtVendor.Columns.Add("1st", Type.GetType("System.String"))
        'dtVendor.Columns.Add("2nd", Type.GetType("System.String"))
        'dtVendor.Columns.Add("3rd", Type.GetType("System.String"))

        'Do a loop to check whether there are any common vendors
        For i = 0 To dtgItem.Items.Count() - 1
            chkItem = dtgItem.Items(i).Cells(EnumList.icChk).FindControl("chkSelection")

            If chkItem.Checked Then
                strVendor = ""
                strVendor = IIf((dtgItem.Items(i).Cells(9).Text = "" Or dtgItem.Items(i).Cells(9).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(9).Text)
                

                strsql = "SELECT * FROM COMPANY_MSTR WHERE CM_STATUS <> 'A'  "
                strsql &= "AND (CM_COY_ID = '" & strVendor & "') "
                If objDb.Exist(strsql) > 0 Then
                    strMsg = "Company is currently inactive."
                    Return False
                    Exit Function
                End If

                If i = 0 Then 'keep track of the 1st row that is checked
                    aryProdCode.Add(New String() {dtgItem.Items(i).Cells(EnumList.icProductCode).Text, "", "", "", ""})
                    'dtr = dtVendor.NewRow()
                    'dtr("Prefer") = strCPrefer
                    'dtr("1st") = strC1st
                    'dtr("2nd") = strC2nd
                    'dtr("3rd") = strC3rd
                    'dtVendor.Rows.Add(dtr)
                Else
                    'For j = 0 To i - 1 'loop thru from the 1st checked row to the row before the current row
                    '    chkItem1 = dtgItem.Items(j).Cells(EnumList.icChk).FindControl("chkSelection")
                    '    If chkItem1.Checked Then
                    '        strPPrefer = ""
                    '        strP1st = ""
                    '        strP2nd = ""
                    '        strP3rd = ""
                    '        strPPrefer = IIf((dtgItem.Items(j).Cells(EnumList.icPreferCode).Text = "" Or dtgItem.Items(j).Cells(EnumList.icPreferCode).Text = "&nbsp;"), "", dtgItem.Items(j).Cells(EnumList.icPreferCode).Text)
                    '        strP1st = IIf((dtgItem.Items(j).Cells(EnumList.ic1stCode).Text = "" Or dtgItem.Items(j).Cells(EnumList.ic1stCode).Text = "&nbsp;"), "", dtgItem.Items(j).Cells(EnumList.ic1stCode).Text)
                    '        strP2nd = IIf((dtgItem.Items(j).Cells(EnumList.ic2ndCode).Text = "" Or dtgItem.Items(j).Cells(EnumList.ic2ndCode).Text = "&nbsp;"), "", dtgItem.Items(j).Cells(EnumList.ic2ndCode).Text)
                    '        strP3rd = IIf((dtgItem.Items(j).Cells(EnumList.ic3rdCode).Text = "" Or dtgItem.Items(j).Cells(EnumList.ic3rdCode).Text = "&nbsp;"), "", dtgItem.Items(j).Cells(EnumList.ic3rdCode).Text)
                    '        'If strPPrefer <> strCPrefer And strPPrefer <> strC1st And strPPrefer <> strC2nd And strPPrefer <> strC3rd And _
                    '        '    strP1st <> strCPrefer And strP1st <> strC1st And strP1st <> strC2nd And strP1st <> strC3rd And _
                    '        '    strP2nd <> strCPrefer And strP2nd <> strC1st And strP2nd <> strC2nd And strP2nd <> strC3rd And _
                    '        '    strP3rd <> strCPrefer And strP3rd <> strC1st And strP3rd <> strC2nd And strP3rd <> strC3rd Then
                    '        '    strMsg = "No common vendor."
                    '        '    Return False
                    '        '    Exit Function
                    '        'End If
                    '        If strCPrefer <> "" Then
                    '            If strPPrefer <> "" Then
                    '                If strCPrefer = strPPrefer Then Exit For
                    '            End If

                    '            If strP1st <> "" Then
                    '                If strCPrefer = strP1st Then Exit For
                    '            End If

                    '            If strP2nd <> "" Then
                    '                If strCPrefer = strP2nd Then Exit For
                    '            End If

                    '            If strP3rd <> "" Then
                    '                If strCPrefer = strP3rd Then Exit For
                    '            End If

                    '        End If

                    '        If strC1st <> "" Then
                    '            If strPPrefer <> "" Then
                    '                If strC1st = strPPrefer Then Exit For
                    '            End If

                    '            If strP1st <> "" Then
                    '                If strC1st = strP1st Then Exit For
                    '            End If

                    '            If strP2nd <> "" Then
                    '                If strC1st = strP2nd Then Exit For
                    '            End If

                    '            If strP3rd <> "" Then
                    '                If strC1st = strP3rd Then Exit For
                    '            End If
                    '        End If

                    '        If strC2nd <> "" Then
                    '            If strPPrefer <> "" Then
                    '                If strC2nd = strPPrefer Then Exit For
                    '            End If
                    '            If strP1st <> "" Then
                    '                If strC2nd = strP1st Then Exit For
                    '            End If

                    '            If strP2nd <> "" Then
                    '                If strC2nd = strP2nd Then Exit For
                    '            End If

                    '            If strP3rd <> "" Then
                    '                If strC2nd = strP3rd Then Exit For
                    '            End If
                    '        End If

                    '        If strC3rd <> "" Then
                    '            If strPPrefer <> "" Then
                    '                If strC3rd = strPPrefer Then Exit For
                    '            End If

                    '            If strP1st <> "" Then
                    '                If strC3rd = strP1st Then Exit For
                    '            End If

                    '            If strP2nd <> "" Then
                    '                If strC3rd = strP2nd Then Exit For
                    '            End If

                    '            If strP3rd <> "" Then
                    '                If strC3rd = strP3rd Then Exit For
                    '            End If
                    '        End If
                    '        strMsg = "No common vendor."
                    '        Return False
                    '        Exit Function
                    '        'End If
                    '    End If
                    'Next
                    'keep the Product Code of the selected record into an array
                    aryProdCode.Add(New String() {dtgItem.Items(i).Cells(EnumList.icProductCode).Text, "", "", "", ""})

                    ''keep the vendors of the selected record into a dataset
                    'dtr = dtVendor.NewRow()
                    'dtr("Prefer") = strCPrefer
                    'dtr("1st") = strC1st
                    'dtr("2nd") = strC2nd
                    'dtr("3rd") = strC3rd
                    'dtVendor.Rows.Add(dtr)
                End If
            End If
        Next

        'If there is only 1 row selected, assign the selected row to the array
        'If dtVendor.Rows.Count = 0 Then
        '    dtr = dtVendor.NewRow()
        '    dtr("Prefer") = strCPrefer
        '    dtr("1st") = strC1st
        '    dtr("2nd") = strC2nd
        '    dtr("3rd") = strC3rd
        '    dtVendor.Rows.Add(dtr)
        'End If
        'dtVendorList = dtVendor
        aryProdCodeList = aryProdCode
        Return True
    End Function
End Class