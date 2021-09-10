Imports AgoraLegacy
Imports eProcure.Component




Public Class BuyerCatalogueSearchSEH
    Inherits AgoraLegacy.AppBaseClass
    Dim objGlobal As New AppGlobals
    Protected WithEvents dtgItem As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtItemName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtItemCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents chkSpot As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkStock As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkMRO As System.Web.UI.WebControls.CheckBox

    Dim objDB As New EAD.DBCom
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Public Enum EnumList
        icChk = 0
        icVendItemCode = 1
        icProductCode = 2
        icItemName = 3
        icUOM = 4
        icCurrency = 5
        icLastTxnPrice = 6
        icQty = 7
        icVendor = 8
        icPrefer = 9
        ic1st = 10
        ic2nd = 11
        ic3rd = 12
        icPreferCode = 13
        ic1stCode = 14
        ic2ndCode = 15
        ic3rdCode = 16
        icSpec = 17
        ic1stSpec = 18
        ic2ndSpec = 19
        ic3rdSpec = 20
        icOversea = 21
        icItemType = 22
    End Enum
    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strNewCSS = "both"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgItem)
        If Not Page.IsPostBack Then
            'objGlobal.FillCommodityType(Me.cboCommodityType)
            'Me.dtgItem.CurrentPageIndex = 0
            BindBuyerCat()
            DisplayUserCheckBtn()
            'Bindgrid()
        End If
        cmdPO.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
        cmdRFQ.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
        Session("urlreferer") = "BuyerCatalogueSearch"
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
        'Dim objBUYERCAT As New BuyerCat
        Dim objBuyerCat_Ext As New BuyerCat_Ext
        Dim strCommodity, strCatBuyer As String
        Dim i As Integer = 0
        Dim strItem As String = ""
        Dim aryItemType As New ArrayList
        Dim strSpt, strStk, strMro As String

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

        If Me.chkSpot.Enabled = False Then
            strSpt = "N"
        Else
            strSpt = "Y"
        End If

        If Me.chkStock.Enabled = False Then
            strStk = "N"
        Else
            strStk = "Y"
        End If

        If Me.chkMRO.Enabled = False Then
            strMro = "N"
        Else
            strMro = "Y"
        End If

        If cboCatalogueBuyer.SelectedItem.Text <> "---Select---" Then
            ds = objBuyerCat_Ext.getBuyerCatItems(cboCatalogueBuyer.SelectedItem.Value, txtItemCode.Text, aryItemType, strCommodity, txtItemName.Text, True, strSpt, strStk, strMro, txtVendorName.Text)

        Else
            If cboCatalogueBuyer.Items.Count > 1 Then
                For i = 1 To cboCatalogueBuyer.Items.Count - 1
                    strItem = strItem & cboCatalogueBuyer.Items(i).Value & ","
                Next
                strItem = Mid(strItem, 1, Len(strItem) - 1)
            Else
                strItem = "0"
            End If
            ds = objBuyerCat_Ext.getBuyerCatItemsByCombo(strItem, txtItemCode.Text, strSpt, strStk, strMro, aryItemType, strCommodity, txtItemName.Text, True, txtVendorName.Text)

        End If
        'ds = objBUYERCAT.getBuyerCatItems(cboCatalogueBuyer.SelectedItem.Value, strCommodity, txtItemName.Text, True)
        Dim dvViewItem As DataView
        dvViewItem = ds.Tables(0).DefaultView

        dvViewItem.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewItem.Sort += " DESC"

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count
        If ViewState("intPageRecordCnt") > 0 Then
            cmdPO.Visible = True
            cmdRFQ.Visible = True
            resetDatagridPageIndex(dtgItem, dvViewItem)

            dtgItem.DataSource = dvViewItem
            dtgItem.DataBind()
        Else
            cmdPO.Visible = False
            cmdRFQ.Visible = False
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
            Dim strGroupCode As String = ""

            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim lnkCode As HyperLink
            lnkCode = e.Item.Cells(EnumList.icVendItemCode).FindControl("lnkCode")
            lnkCode.NavigateUrl = "javascript:;"
            Session("UrlLocation") = "BuyerCatalogueSearch"

            Dim objProd As New Products

            strGroupCode = objProd.GetConCatGroupCode(dv("PM_PRODUCT_CODE"), Session("CompanyId"))
            'lnkCode.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=B&pageid=" & strPageId & "&pid=" & dv("PM_PRODUCT_CODE") & "&index=&draft=0") & "')")
            lnkCode.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=B&pageid=" & strPageId & "&pid=" & dv("PM_PRODUCT_CODE") & "&index=&draft=0&ref=" & strGroupCode & "") & "')")

            lnkCode.Text = dv("PM_VENDOR_ITEM_CODE")

            Dim dvBuyerCat As DataView
            Dim lblVendor As Label = e.Item.Cells(EnumList.icVendor).FindControl("lblVendor")
            Dim strPrefer As String = e.Item.Cells(EnumList.icPrefer).Text
            Dim str1st As String = e.Item.Cells(EnumList.ic1st).Text
            Dim str2nd As String = e.Item.Cells(EnumList.ic2nd).Text
            Dim str3rd As String = e.Item.Cells(EnumList.ic3rd).Text

            lblVendor.Text = ""

            'Dim dtVendor As DataTable = New DataTable
            Dim dsVendor As New DataSet
            Dim i As Integer
            Dim vline, strTemp As String

            strTemp = "SELECT * FROM " & _
                    "(SELECT CASE WHEN PV_VENDOR_TYPE = 'P' THEN 0 ELSE PV_VENDOR_TYPE END AS PV_VENDOR_TYPE, a.CM_COY_NAME AS PV_S_COY_NAME, PV_S_COY_ID " & _
                    "FROM PIM_VENDOR LEFT JOIN COMPANY_MSTR a ON PV_S_COY_ID = a.CM_COY_ID WHERE PV_PRODUCT_INDEX = '" & dv("PM_PRODUCT_INDEX") & "' ORDER BY PV_VENDOR_TYPE) tb GROUP BY PV_S_COY_ID ORDER BY PV_VENDOR_TYPE "

            dsVendor = objDB.FillDs(strTemp)

            If dsVendor.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsVendor.Tables(0).Rows.Count - 1
                    If i = 0 Then
                        If Common.parseNull(dsVendor.Tables(0).Rows(i)("PV_S_COY_NAME")) <> "" And Common.parseNull(dsVendor.Tables(0).Rows(i)("PV_S_COY_NAME")) <> "&nbsp;" Then lblVendor.Text = dsVendor.Tables(0).Rows(i)("PV_S_COY_NAME")
                    Else
                        If Common.parseNull(dsVendor.Tables(0).Rows(i)("PV_S_COY_NAME")) <> "" And Common.parseNull(dsVendor.Tables(0).Rows(i)("PV_S_COY_NAME")) <> "&nbsp;" Then
                            If lblVendor.Text <> "" Then
                                lblVendor.Text = lblVendor.Text & "<br>" & dsVendor.Tables(0).Rows(i)("PV_S_COY_NAME")
                            Else
                                lblVendor.Text = dsVendor.Tables(0).Rows(i)("PV_S_COY_NAME")
                            End If
                        End If
                    End If

                Next
              
            End If

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

         

            Dim txtQty As TextBox
            txtQty = e.Item.FindControl("txtQty")
            If dv("PM_ITEM_TYPE") = "ST" Then
                txtQty.Visible = True
                txtQty.Text = "1.00"
            Else
                txtQty.Text = ""
                txtQty.Visible = False
            End If

            Dim lblSpec As Label = e.Item.Cells(EnumList.icSpec).FindControl("lblSpec")
            Dim str1stSpec As String = e.Item.Cells(EnumList.ic1stSpec).Text
            Dim str2ndSpec As String = e.Item.Cells(EnumList.ic2ndSpec).Text
            Dim str3rdSpec As String = e.Item.Cells(EnumList.ic3rdSpec).Text

            If Common.parseNull(str1stSpec) <> "" And Common.parseNull(str1stSpec) <> "&nbsp;" Then lblSpec.Text = str1stSpec
            If Common.parseNull(str2ndSpec) <> "" And Common.parseNull(str2ndSpec) <> "&nbsp;" Then
                If lblSpec.Text <> "" Then
                    lblSpec.Text = lblSpec.Text & "<br>" & str2ndSpec
                Else
                    lblSpec.Text = str2ndSpec
                End If
            End If
            If Common.parseNull(str3rdSpec) <> "" And Common.parseNull(str3rdSpec) <> "&nbsp;" Then
                If lblSpec.Text <> "" Then
                    lblSpec.Text = lblSpec.Text & "<br>" & str3rdSpec
                Else
                    lblSpec.Text = str3rdSpec
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

    Private Function ChkVendor(ByRef strMsg As String, ByRef aryProdCodeList As ArrayList, ByRef dtVendorList As DataTable, ByRef aryProdVenList As ArrayList, ByRef aryProdQtyList As ArrayList) As Boolean
        Dim chkItem, chkItem1 As CheckBox
        Dim txtQty As TextBox
        Dim blnFound As Boolean
        Dim dtVendor As New DataTable
        Dim aryProdCode As New ArrayList
        Dim aryProdQty As New ArrayList()
        Dim aryProdVen As New ArrayList
        Dim aryProdVenTemp As New ArrayList
        Dim objBuyerCat_Ext As New BuyerCat_Ext
        Dim dtr As DataRow
        Dim a, i, k As Integer
        Dim j As Integer = 0
        Dim c As Integer = 0
        Dim strProdCode As String
        Dim iProdIndex As Integer
        Dim dsVen As New DataSet
        Dim VenTrue As Boolean = False

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

        Dim strsql As String

        dtVendor.Columns.Add("Prefer", Type.GetType("System.String"))
        dtVendor.Columns.Add("1st", Type.GetType("System.String"))
        dtVendor.Columns.Add("2nd", Type.GetType("System.String"))
        dtVendor.Columns.Add("3rd", Type.GetType("System.String"))

        'Do a loop to check whether there are any common vendors
        For i = 0 To dtgItem.Items.Count() - 1
            chkItem = dtgItem.Items(i).Cells(EnumList.icChk).FindControl("chkSelection")

            If chkItem.Checked Then
                strProdCode = dtgItem.Items(i).Cells(EnumList.icProductCode).Text
                iProdIndex = objDB.GetVal("SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & strProdCode & "'")

                strsql = "SELECT * FROM " & _
                    "(SELECT CASE WHEN PV_VENDOR_TYPE = 'P' THEN 0 ELSE PV_VENDOR_TYPE END AS PV_VENDOR_TYPE, PV_S_COY_ID " & _
                    "FROM PIM_VENDOR WHERE PV_PRODUCT_INDEX = " & iProdIndex & " AND (PV_S_COY_ID <> '' AND PV_S_COY_ID IS NOT NULL) ORDER BY PV_VENDOR_TYPE) tb GROUP BY PV_S_COY_ID ORDER BY PV_VENDOR_TYPE"

                dsVen = objDB.FillDs(strsql)

                'Check for item whether company is inactive or not 
                If dsVen.Tables(0).Rows.Count > 0 Then
                    For k = 0 To dsVen.Tables(0).Rows.Count - 1
                        If Common.parseNull(dsVen.Tables(0).Rows(k)("PV_S_COY_ID")) = "" Then
                            strMsg = objGlobal.GetErrorMessage("00189") 'Please select item with vendor.
                            Return False
                            Exit Function
                        End If

                        strsql = "SELECT '*' FROM COMPANY_MSTR WHERE CM_STATUS <> 'A' "
                        strsql &= "AND CM_COY_ID = '" & dsVen.Tables(0).Rows(k)("PV_S_COY_ID") & "'"
                        If objDB.Exist(strsql) > 0 Then
                            strMsg = objGlobal.GetErrorMessage("00111") 'Company is currently inactive."
                            Return False
                            Exit Function
                        End If
                    Next
                Else
                    strMsg = objGlobal.GetErrorMessage("00189") 'Please select item with vendor.
                    Return False
                    Exit Function
                End If

                aryProdCode.Add(dtgItem.Items(i).Cells(EnumList.icProductCode).Text)

                txtQty = dtgItem.Items(i).Cells(EnumList.icQty).FindControl("txtQty")

                If txtQty.Visible = True Then
                    If Not System.Text.RegularExpressions.Regex.IsMatch(txtQty.Text, "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$") Then
                        strMsg = "Invalid quantity of selected items. Range should be from 0.01 to 999999.99"
                        Return False
                        Exit Function
                    End If
                    aryProdQty.Add(New String() {dtgItem.Items(i).Cells(EnumList.icProductCode).Text, txtQty.Text})
                End If

                'VenTrue = False

                'If a = 0 Then
                '    aryProdCode.Add(dtgItem.Items(i).Cells(EnumList.icProductCode).Text)
                '    For k = 0 To dsVen.Tables(0).Rows.Count - 1
                '        aryProdVen.Add(New String() {dsVen.Tables(0).Rows(k)("PV_S_COY_ID"), a, False})
                '        'aryProdVenTemp.Add(dsVen.Tables(0).Rows(k)("PV_S_COY_ID"))
                '    Next
                'Else
                '    For c = 0 To aryProdCode.Count - 1
                '        For j = 0 To dsVen.Tables(0).Rows.Count - 1 'loop thru from the 1st checked row to the row before the current row

                '            For k = 0 To aryProdVen.Count - 1
                '                If dsVen.Tables(0).Rows(j)("PV_S_COY_ID") <> "" And CInt(aryProdVen(k)(1)) = c Then
                '                    If dsVen.Tables(0).Rows(j)("PV_S_COY_ID") = aryProdVen(k)(0) Then
                '                        VenTrue = True
                '                        aryProdVen(k)(2) = True
                '                        aryProdVenTemp.Add(New String() {dsVen.Tables(0).Rows(j)("PV_S_COY_ID"), a, True})
                '                    End If

                '                End If
                '            Next

                '            If VenTrue = False Then
                '                strMsg = "No common vendor."
                '                Return False
                '                Exit Function
                '            End If

                '        Next
                '    Next

                '    'keep the Product Code of the selected record into an array
                '    aryProdCode.Add(dtgItem.Items(i).Cells(EnumList.icProductCode).Text)

                '    'keep the vendors of the selected record into a dataset
                '    For k = 0 To aryProdVenTemp.Count - 1
                '        aryProdVen.Add(New String() {aryProdVenTemp(k)(0), aryProdVenTemp(k)(1), aryProdVenTemp(k)(2)})
                '    Next

                '    aryProdVenTemp.Clear()
            End If




            'strCPrefer = ""
            'strC1st = ""
            'strC2nd = ""
            'strC3rd = ""
            'strCPrefer = IIf((dtgItem.Items(i).Cells(EnumList.icPreferCode).Text = "" Or dtgItem.Items(i).Cells(EnumList.icPreferCode).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(EnumList.icPreferCode).Text)
            'strC1st = IIf((dtgItem.Items(i).Cells(EnumList.ic1stCode).Text = "" Or dtgItem.Items(i).Cells(EnumList.ic1stCode).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(EnumList.ic1stCode).Text)
            'strC2nd = IIf((dtgItem.Items(i).Cells(EnumList.ic2ndCode).Text = "" Or dtgItem.Items(i).Cells(EnumList.ic2ndCode).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(EnumList.ic2ndCode).Text)
            'strC3rd = IIf((dtgItem.Items(i).Cells(EnumList.ic3rdCode).Text = "" Or dtgItem.Items(i).Cells(EnumList.ic3rdCode).Text = "&nbsp;"), "", dtgItem.Items(i).Cells(EnumList.ic3rdCode).Text)
            'If (strCPrefer = "" And strC1st = "" And strC2nd = "" And strC3rd = "") Then
            '    strMsg = "Please select item with vendor."
            '    Return False
            '    Exit Function
            'End If

            'strsql = "SELECT * FROM COMPANY_MSTR WHERE CM_STATUS <> 'A'  "
            'strsql &= "AND (CM_COY_ID = '" & strCPrefer & "' OR CM_COY_ID = '" & strC1st & "' OR CM_COY_ID = '" & strC2nd & "' OR CM_COY_ID = '" & strC3rd & "') "
            'If objDB.Exist(strsql) > 0 Then
            '    strMsg = "Company is currently inactive."
            '    Return False
            '    Exit Function
            'End If

            'If i = 0 Then 'keep track of the 1st row that is checked
            '    aryProdCode.Add(dtgItem.Items(i).Cells(EnumList.icProductCode).Text)
            '    dtr = dtVendor.NewRow()
            '    dtr("Prefer") = strCPrefer
            '    dtr("1st") = strC1st
            '    dtr("2nd") = strC2nd
            '    dtr("3rd") = strC3rd
            '    dtVendor.Rows.Add(dtr)
            'Else
            '    For j = 0 To i - 1 'loop thru from the 1st checked row to the row before the current row
            '        chkItem1 = dtgItem.Items(j).Cells(EnumList.icChk).FindControl("chkSelection")
            '        If chkItem1.Checked Then
            '            strPPrefer = ""
            '            strP1st = ""
            '            strP2nd = ""
            '            strP3rd = ""
            '            strPPrefer = IIf((dtgItem.Items(j).Cells(EnumList.icPreferCode).Text = "" Or dtgItem.Items(j).Cells(EnumList.icPreferCode).Text = "&nbsp;"), "", dtgItem.Items(j).Cells(EnumList.icPreferCode).Text)
            '            strP1st = IIf((dtgItem.Items(j).Cells(EnumList.ic1stCode).Text = "" Or dtgItem.Items(j).Cells(EnumList.ic1stCode).Text = "&nbsp;"), "", dtgItem.Items(j).Cells(EnumList.ic1stCode).Text)
            '            strP2nd = IIf((dtgItem.Items(j).Cells(EnumList.ic2ndCode).Text = "" Or dtgItem.Items(j).Cells(EnumList.ic2ndCode).Text = "&nbsp;"), "", dtgItem.Items(j).Cells(EnumList.ic2ndCode).Text)
            '            strP3rd = IIf((dtgItem.Items(j).Cells(EnumList.ic3rdCode).Text = "" Or dtgItem.Items(j).Cells(EnumList.ic3rdCode).Text = "&nbsp;"), "", dtgItem.Items(j).Cells(EnumList.ic3rdCode).Text)
            '            'If strPPrefer <> strCPrefer And strPPrefer <> strC1st And strPPrefer <> strC2nd And strPPrefer <> strC3rd And _
            '            '    strP1st <> strCPrefer And strP1st <> strC1st And strP1st <> strC2nd And strP1st <> strC3rd And _
            '            '    strP2nd <> strCPrefer And strP2nd <> strC1st And strP2nd <> strC2nd And strP2nd <> strC3rd And _
            '            '    strP3rd <> strCPrefer And strP3rd <> strC1st And strP3rd <> strC2nd And strP3rd <> strC3rd Then
            '            '    strMsg = "No common vendor."
            '            '    Return False
            '            '    Exit Function
            '            'End If
            '            If strCPrefer <> "" Then
            '                If strPPrefer <> "" Then
            '                    If strCPrefer = strPPrefer Then Exit For
            '                End If

            '                If strP1st <> "" Then
            '                    If strCPrefer = strP1st Then Exit For
            '                End If

            '                If strP2nd <> "" Then
            '                    If strCPrefer = strP2nd Then Exit For
            '                End If

            '                If strP3rd <> "" Then
            '                    If strCPrefer = strP3rd Then Exit For
            '                End If

            '            End If

            '            If strC1st <> "" Then
            '                If strPPrefer <> "" Then
            '                    If strC1st = strPPrefer Then Exit For
            '                End If

            '                If strP1st <> "" Then
            '                    If strC1st = strP1st Then Exit For
            '                End If

            '                If strP2nd <> "" Then
            '                    If strC1st = strP2nd Then Exit For
            '                End If

            '                If strP3rd <> "" Then
            '                    If strC1st = strP3rd Then Exit For
            '                End If
            '            End If

            '            If strC2nd <> "" Then
            '                If strPPrefer <> "" Then
            '                    If strC2nd = strPPrefer Then Exit For
            '                End If
            '                If strP1st <> "" Then
            '                    If strC2nd = strP1st Then Exit For
            '                End If

            '                If strP2nd <> "" Then
            '                    If strC2nd = strP2nd Then Exit For
            '                End If

            '                If strP3rd <> "" Then
            '                    If strC2nd = strP3rd Then Exit For
            '                End If
            '            End If

            '            If strC3rd <> "" Then
            '                If strPPrefer <> "" Then
            '                    If strC3rd = strPPrefer Then Exit For
            '                End If

            '                If strP1st <> "" Then
            '                    If strC3rd = strP1st Then Exit For
            '                End If

            '                If strP2nd <> "" Then
            '                    If strC3rd = strP2nd Then Exit For
            '                End If

            '                If strP3rd <> "" Then
            '                    If strC3rd = strP3rd Then Exit For
            '                End If
            '            End If
            '            strMsg = "No common vendor."
            '            Return False
            '            Exit Function
            '            'End If
            '        End If
            '    Next
            '    'keep the Product Code of the selected record into an array
            '    aryProdCode.Add(dtgItem.Items(i).Cells(EnumList.icProductCode).Text)

            '    'keep the vendors of the selected record into a dataset
            '    dtr = dtVendor.NewRow()
            '    dtr("Prefer") = strCPrefer
            '    dtr("1st") = strC1st
            '    dtr("2nd") = strC2nd
            '    dtr("3rd") = strC3rd
            '    dtVendor.Rows.Add(dtr)
            'End If
            'a = a + 1

            'End If
        Next

        aryProdVen = Nothing
        aryProdVen = objBuyerCat_Ext.chkCommonVendor(aryProdCode)

        If aryProdVen.Count = 0 Then
            strMsg = "No common vendor."
            Return False
            Exit Function
        End If

        If aryProdCode.Count > 1 Then
            aryProdVen = objBuyerCat_Ext.chkCommonVenCode(aryProdCode, aryProdVen)

            If aryProdVen.Count = 0 Then
                strMsg = "No common vendor code."
                Return False
                Exit Function
            End If
        End If

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
        aryProdVenList = aryProdVen
        aryProdCodeList = aryProdCode
        aryProdQtyList = aryProdQty
        Return True
    End Function

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

    Private Sub cmdPO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPO.Click
        'Dim strMsg As String
        Dim dgItem As DataGridItem
        Dim i, j, k As Integer
        Dim chkItem As CheckBox
        Dim dtVendor As DataTable
        'Dim objPO As New PurchaseOrder
        Dim aryProdCode As New ArrayList
        Dim aryVendor As New ArrayList
        Dim aryTempVen As New ArrayList
        Dim aryQty As New ArrayList()
        Dim aryTemp As New ArrayList
        Dim strMsg As String = ""
        Dim strsql As String

        If ChkSelectedItem(strMsg) Then

            If checkMandatory(strMsg) Then
                If ChkVendor(strMsg, aryProdCode, dtVendor, aryTempVen, aryQty) Then
                    'If aryProdCode.Count = 1 Then
                    '    For i = 0 To aryTempVen.Count - 1
                    '        aryVendor.Add(aryTempVen(i))
                    '    Next

                    'Else
                    '    For i = 0 To aryProdCode.Count - 1
                    '        For j = 0 To aryTempVen.Count - 1
                    '            If aryTempVen(j)(0) <> "" And aryTempVen(j)(2) = True And CInt(aryTempVen(j)(1)) = i Then
                    '                If aryVendor Is Nothing Or aryVendor.Count = 0 Then
                    '                    aryVendor.Add(aryTempVen(j)(0))
                    '                Else
                    '                    Dim strCheck As Boolean = False

                    '                    For k = 0 To aryVendor.Count - 1
                    '                        If aryTempVen(j)(0) = aryVendor(k) Then
                    '                            strCheck = True
                    '                        End If
                    '                    Next

                    '                    If strCheck = False Then aryVendor.Add(aryTempVen(j)(0))
                    '                End If

                    '            End If
                    '        Next
                    '    Next

                    'For i = 0 To aryTempVen.Count - 1
                    '    If aryProdCode(0) = aryTempVen(i)(0) Then
                    '        aryTemp.Add(aryTempVen(i)(2))
                    '    End If

                    'Next

                    ''Dim strCheck As Boolean = True

                    'For i = 0 To aryTemp.Count - 1
                    '    For j = 0 To aryProdCode.Count - 1
                    '        If aryProdCode(j) <> aryProdCode(0) Then
                    '            For k = 0 To aryTempVen.Count - 1
                    '                If aryProdCode(j) = aryTempVen(k)(0) Then
                    '                    If aryTemp(i) = aryTempVen(k)(2) Then
                    '                        strCheck = True
                    '                        Exit For
                    '                    End If

                    '                    strCheck = False
                    '                End If
                    '            Next
                    '        End If
                    '    Next

                    '    If strCheck = True Then aryVendor.Add(aryTemp(i))
                    'Next


                    'End If

                    Session("ProdList") = aryProdCode
                    Session("VendorList") = aryTempVen
                    Session("QtyList") = aryQty
                    Response.Redirect(dDispatcher.direct("PO", "RaisePO.aspx", "frm=PC&pageid=" & strPageId))

                    '    'Get the common vendor list
                    '    Dim strPrefer = dtVendor.Rows(0).Item("Prefer")
                    '    Dim str1st = dtVendor.Rows(0).Item("1st")
                    '    Dim str2nd = dtVendor.Rows(0).Item("2nd")
                    '    Dim str3rd = dtVendor.Rows(0).Item("3rd")

                    '    'Check whether there's only 1 record selected
                    '    If dtVendor.Rows.Count = 1 Then
                    '        If strPrefer <> "" Then aryVendor.Add(strPrefer)
                    '        If str1st <> "" Then aryVendor.Add(str1st)
                    '        If str2nd <> "" Then aryVendor.Add(str2nd)
                    '        If str3rd <> "" Then aryVendor.Add(str3rd)
                    '    Else
                    '        For j = 0 To dtVendor.Rows.Count - 1
                    '            Dim strPreferCheck As Boolean = True
                    '            Dim str1stCheck As Boolean = True
                    '            Dim str2ndCheck As Boolean = True
                    '            Dim str3rdCheck As Boolean = True

                    '            strPrefer = dtVendor.Rows(j).Item("Prefer")
                    '            str1st = dtVendor.Rows(j).Item("1st")
                    '            str2nd = dtVendor.Rows(j).Item("2nd")
                    '            str3rd = dtVendor.Rows(j).Item("3rd")

                    '            strsql = "SELECT * FROM COMPANY_MSTR WHERE CM_STATUS <> 'A'  "
                    '            strsql &= "AND (CM_COY_ID = '" & strPrefer & "' OR CM_COY_ID = '" & str1st & "' OR CM_COY_ID = '" & str2nd & "' OR CM_COY_ID = '" & str3rd & "') "
                    '            If objDB.Exist(strsql) > 0 Then
                    '                Common.NetMsgbox(Me, "Company is currently inactive.", MsgBoxStyle.Exclamation)
                    '                Exit Sub
                    '            End If

                    '            For i = 0 To dtVendor.Rows.Count - 1
                    '                If i <> j Then
                    '                    'Compare the Prefer vendor of the 1st row with other rows
                    '                    If strPrefer <> "" And _
                    '                        (strPrefer = dtVendor.Rows(i).Item("Prefer") Or _
                    '                         strPrefer = dtVendor.Rows(i).Item("1st") Or _
                    '                         strPrefer = dtVendor.Rows(i).Item("2nd") Or _
                    '                         strPrefer = dtVendor.Rows(i).Item("3rd")) Then
                    '                    Else
                    '                        strPreferCheck = False
                    '                        'If i = dtVendor.Rows.Count - 1 Then 'ie check until the last row
                    '                        '    aryVendor.Add(strPrefer)
                    '                        'End If
                    '                    End If

                    '                    'Compare the 1st alternative vendor of the 1st row with other rows
                    '                    If str1st <> "" And _
                    '                        (str1st = dtVendor.Rows(i).Item("Prefer") Or _
                    '                         str1st = dtVendor.Rows(i).Item("1st") Or _
                    '                         str1st = dtVendor.Rows(i).Item("2nd") Or _
                    '                         str1st = dtVendor.Rows(i).Item("3rd")) Then
                    '                    Else
                    '                        str1stCheck = False
                    '                        'If i = dtVendor.Rows.Count - 1 Then 'ie check until the last row
                    '                        '    aryVendor.Add(str1st)
                    '                        'End If
                    '                    End If

                    '                    'Compare the 2nd alternative vendor of the 1st row with other rows
                    '                    If str2nd <> "" And _
                    '                        (str2nd = dtVendor.Rows(i).Item("Prefer") Or _
                    '                         str2nd = dtVendor.Rows(i).Item("1st") Or _
                    '                         str2nd = dtVendor.Rows(i).Item("2nd") Or _
                    '                         str2nd = dtVendor.Rows(i).Item("3rd")) Then
                    '                    Else
                    '                        str2ndCheck = False
                    '                        'If i = dtVendor.Rows.Count - 1 Then 'ie check until the last row
                    '                        '    aryVendor.Add(str2nd)
                    '                        'End If
                    '                    End If

                    '                    'Compare the 3rd alternative vendor of the 1st row with other rows
                    '                    If str3rd <> "" And _
                    '                        (str3rd = dtVendor.Rows(i).Item("Prefer") Or _
                    '                         str3rd = dtVendor.Rows(i).Item("1st") Or _
                    '                         str3rd = dtVendor.Rows(i).Item("2nd") Or _
                    '                         str3rd = dtVendor.Rows(i).Item("3rd")) Then
                    '                    Else
                    '                        str3rdCheck = False
                    '                        'If i = dtVendor.Rows.Count - 1 Then 'ie check until the last row
                    '                        '    aryVendor.Add(str3rd)
                    '                        'End If
                    '                    End If
                    '                End If
                    '            Next
                    '            If strPreferCheck = True Then aryVendor.Add(strPrefer)
                    '            If str1stCheck = True Then aryVendor.Add(str1st)
                    '            If str2ndCheck = True Then aryVendor.Add(str2nd)
                    '            If str3rdCheck = True Then aryVendor.Add(str3rd)

                    '        Next

                    '    End If
                    '    Session("ProdList") = aryProdCode
                    '    Session("VendorList") = aryVendor
                    '    If aryVendor.Count = 0 Then
                    '        strMsg = "No common vendor."
                    '        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                    '        Exit Sub
                    '    End If
                    '    Response.Redirect(dDispatcher.direct("PO", "RaisePO.aspx", "frm=PC&pageid=" & strPageId))

                Else
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
                End If
            Else
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            End If
        Else
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
        End If

    End Sub
    Private Sub cmdRFQ_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRFQ.Click
        'Dim strMsg As String
        Dim dgItem As DataGridItem
        Dim i, j As Integer
        Dim chkItem As CheckBox
        'Dim dtVendor As DataTable
        'Dim objPO As New PurchaseOrder
        Dim aryProdCode As New ArrayList
        Dim strMsg As String = ""
        Dim strsql, iProdIndex As String

        If ChkSelectedItem(strMsg) Then
            For i = 0 To dtgItem.Items.Count() - 1
                chkItem = dtgItem.Items(i).Cells(EnumList.icChk).FindControl("chkSelection")
                If chkItem.Checked Then
                    Dim dsVendor As New DataSet

                    aryProdCode.Add(New String() {dtgItem.Items(i).Cells(EnumList.icProductCode).Text, ""})
                    iProdIndex = objDB.GetVal("SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & dtgItem.Items(i).Cells(EnumList.icProductCode).Text & "'")

                    strsql = "SELECT * FROM " & _
                        "(SELECT CASE WHEN PV_VENDOR_TYPE = 'P' THEN 0 ELSE PV_VENDOR_TYPE END AS PV_VENDOR_TYPE, PV_S_COY_ID " & _
                        "FROM PIM_VENDOR WHERE PV_PRODUCT_INDEX = " & iProdIndex & " ORDER BY PV_VENDOR_TYPE) tb GROUP BY PV_S_COY_ID ORDER BY PV_VENDOR_TYPE"

                    dsVendor = objDB.FillDs(strsql)

                    If dsVendor.Tables(0).Rows.Count > 0 Then
                        If Common.parseNull(dsVendor.Tables(0).Rows(0)("PV_S_COY_ID")) <> "" Then
                            For j = 0 To dsVendor.Tables(0).Rows.Count - 1
                                If j = 0 Then
                                    strsql = "CM_COY_ID = '" & dsVendor.Tables(0).Rows(j)("PV_S_COY_ID") & "' "
                                Else
                                    strsql &= "OR CM_COY_ID = '" & dsVendor.Tables(0).Rows(j)("PV_S_COY_ID") & "' "
                                End If
                            Next

                            strsql = "SELECT * FROM COMPANY_MSTR WHERE CM_STATUS <> 'A' AND (" & strsql & ") "
                            If objDB.Exist(strsql) > 0 Then
                                Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00111"), MsgBoxStyle.Exclamation)
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            Next
            Session("RFQProdList") = ""
            Session("RFQProdList") = aryProdCode
            Response.Redirect(dDispatcher.direct("RFQ", "Create_RFQ.aspx", "frm=BC&caller=PurchaserSearch&pageid=" & strPageId))
        Else
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
        End If
        

    End Sub

    Sub DisplayUserCheckBtn()
        Dim objUser As New Users_Ext
        Dim dsStk As New DataSet

        dsStk = objUser.GetUserStockType(Session("UserId"), Session("CompanyId"))

        If dsStk.Tables(0).Rows(0)("UM_STK_TYPE_SPOT") = "Y" Then
            chkSpot.Enabled = True
        Else
            chkSpot.Enabled = False
        End If

        If dsStk.Tables(0).Rows(0)("UM_STK_TYPE_STOCK") = "Y" Then
            chkStock.Enabled = True
        Else
            chkStock.Enabled = False
        End If

        If dsStk.Tables(0).Rows(0)("UM_STK_TYPE_MRO") = "Y" Then
            chkMRO.Enabled = True
        Else
            chkMRO.Enabled = False
        End If

    End Sub

    Private Function ChkSelectedItem(ByRef strMsg As String) As Boolean
        Dim chkItem As CheckBox
        Dim aryProdCode, aryVen As New ArrayList
        Dim strOverseaP, strItemTypeP As String
        Dim i, j As Integer
        j = 0

        strOverseaP = ""
        strItemTypeP = ""

        'Do a loop to check whether there are any common vendors
        For i = 0 To dtgItem.Items.Count() - 1
            ChkItem = dtgItem.Items(i).Cells(EnumList.icChk).FindControl("chkSelection")

            If ChkItem.Checked Then
                If j = 0 Then
                    strOverseaP = dtgItem.Items(i).Cells(EnumList.icOversea).Text
                    strItemTypeP = dtgItem.Items(i).Cells(EnumList.icItemType).Text
                Else
                    If strOverseaP = dtgItem.Items(i).Cells(EnumList.icOversea).Text Then

                    Else
                        strMsg = "Selected items cannot be a mixture of oversea and local items."
                        Return False
                        Exit Function
                    End If

                    If strItemTypeP = dtgItem.Items(i).Cells(EnumList.icItemType).Text Then

                    Else
                        strMsg = "Selected items are not from the same item type."
                        Return False
                        Exit Function
                    End If

                End If
                j = j + 1

            End If
        Next

        Return True
    End Function

End Class