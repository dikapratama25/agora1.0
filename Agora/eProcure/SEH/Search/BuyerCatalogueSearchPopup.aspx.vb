Imports AgoraLegacy
Imports eProcure.Component

Imports System.Text.RegularExpressions
Public Class BuyerCatalogueSearchPopupSEH
    Inherits AgoraLegacy.AppBaseClass
    Dim objGlobal As New AppGlobals
    Dim objDB As New EAD.DBCom
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents dtgItem As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtItemName As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents hidVendor As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidVendorCode As System.Web.UI.HtmlControls.HtmlInputHidden

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
        If Request.QueryString("selVendorCode") <> "" And Request.QueryString("selVendorCode") <> "---Select---" Then
            hidVendorCode.Value = Request.QueryString("selVendorCode")
        Else
            hidVendorCode.Value = ""
        End If
        hidSingleVendor.Value = Request.QueryString("selSingleVendor")
        hidOversea.Value = Request.QueryString("oversea")
        hidItemType.Value = Request.QueryString("itemtype")
        If Request.QueryString("selVendor") <> "" Then
            Session("selVendor") = Request.QueryString("selVendor")
            Session("selSingleVendor") = Request.QueryString("selSingleVendor")
            If Request.QueryString("selVendorCode") <> "" And Request.QueryString("selVendorCode") <> "---Select---" Then
                Session("selVendorCode") = Request.QueryString("selVendorCode")
            Else
                hidVendorCode.Value = ""
            End If
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
        Dim objBUYERCAT As New BuyerCat_Ext
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

        ds = objBUYERCAT.getBuyerCatItemsFiltered(hidVendor.Value, strProdList, cboCatalogueBuyer.SelectedItem.Value, strCommodity, txtItemName.Text, True, hidItemType.Value, hidOversea.Value, hidVendorCode.Value)
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

            'Dim dvBuyerCat As DataView
            Dim lblVendor As Label = e.Item.Cells(EnumList.icVendor).FindControl("lblVendor")
            Dim strPrefer As String = e.Item.Cells(EnumList.icPrefer).Text
            Dim str1st As String = e.Item.Cells(EnumList.ic1st).Text
            Dim str2nd As String = e.Item.Cells(EnumList.ic2nd).Text
            Dim str3rd As String = e.Item.Cells(EnumList.ic3rd).Text

            lblVendor.Text = ""
            Dim dsVendor As New DataSet
            Dim i As Integer
            Dim strTemp As String

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
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgItem.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "PM_PRODUCT_DESC"
        Bindgrid()
    End Sub

    Private Function ChkVendor(ByRef strMsg As String, ByRef aryProdCodeList As ArrayList) As Boolean
        Dim chkItem As CheckBox
        Dim aryProdCode As New ArrayList
        Dim aryProdCodeAdd As New ArrayList
        Dim aryProdVen As New ArrayList
        Dim aryProdVenTemp As New ArrayList
        Dim objBuyerCat_Ext As New BuyerCat_Ext
        Dim i, k As Integer
        Dim strProdCode As String
        Dim iProdIndex As Integer
        Dim dsVen As New DataSet
        Dim VenTrue As Boolean = False
        Dim strsql As String

        Dim aryProdCodeCurrent As ArrayList = Session("ProdList")

        For i = 0 To aryProdCodeCurrent.Count - 1
            aryProdCode.Add(aryProdCodeCurrent(i))
        Next

        'Do a loop to check whether there are any common vendors
        For i = 0 To dtgItem.Items.Count() - 1
            chkItem = dtgItem.Items(i).Cells(EnumList.icChk).FindControl("chkSelection")

            If chkItem.Checked Then
                strProdCode = dtgItem.Items(i).Cells(EnumList.icProductCode).Text
                iProdIndex = objDB.GetVal("SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & strProdCode & "'")

                strsql = "SELECT * FROM " & _
                    "(SELECT CASE WHEN PV_VENDOR_TYPE = 'P' THEN 0 ELSE PV_VENDOR_TYPE END AS PV_VENDOR_TYPE, PV_S_COY_ID " & _
                    "FROM PIM_VENDOR WHERE PV_PRODUCT_INDEX = " & iProdIndex & " ORDER BY PV_VENDOR_TYPE) tb GROUP BY PV_S_COY_ID ORDER BY PV_VENDOR_TYPE"

                dsVen = objDB.FillDs(strsql)

                'Check for item whether company is inactive or not 
                If dsVen.Tables(0).Rows.Count > 0 Then
                    For k = 0 To dsVen.Tables(0).Rows.Count - 1
                        If Common.parseNull(dsVen.Tables(0).Rows(k)("PV_S_COY_ID")) = "" Then
                            strMsg = objGlobal.GetErrorMessage("00189") 'Please select item with vendor.
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

            End If
        Next

        'aryProdVen = Nothing
        'aryProdVen = objBuyerCat_Ext.chkCommonVendor(aryProdCode)

        'If aryProdVen.Count = 0 Then
        '    strMsg = "No common vendor."
        '    Return False
        '    Exit Function
        'End If

        'If aryProdCode.Count > 1 Then
        '    aryProdVen = objBuyerCat_Ext.chkCommonVenCode(aryProdCode, aryProdVen)

        '    If aryProdVen.Count = 0 Then
        '        strMsg = "No common vendor code."
        '        Return False
        '        Exit Function
        '    End If
        'End If

        'aryProdVenList = aryProdVen
        aryProdCodeList = aryProdCode

        Return True
    End Function

    Protected Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        'Dim strMsg As String
        Dim objBuyerCat_Ext As New BuyerCat_Ext
        Dim dgItem As DataGridItem
        Dim i As Integer
        Dim chkItem As CheckBox
        'Dim objPO As New PurchaseOrder
        Dim aryProdCode As New ArrayList
        Dim aryVendor As New ArrayList
        Dim aryTempVen As New ArrayList
        Dim aryTemp As New ArrayList
        Dim strMsg As String = ""

        If ChkVendor(strMsg, aryProdCode) Then
            aryVendor = aryTempVen

            'Combine All array
            Dim aryProdCodeCurrent As ArrayList = Session("ProdList")
            Dim aryVendorCurrent As ArrayList
            'Dim aryVendorCurrent As ArrayList = Session("VendorList")

            'For i = 0 To aryProdCode.Count - 1
            '    aryProdCodeCurrent.Add(aryProdCode(i))
            'Next

            '' ''For i = 0 To aryProdCodeCurrent.Count - 1
            '' ''    aryProdCode.Add(aryProdCodeCurrent(i))
            '' ''Next

            'For i = 0 To aryVendorCurrent.Count - 1
            '    aryVendor.Add(aryVendorCurrent(i))
            'Next
            'End If

            aryVendorCurrent = Nothing
            aryVendorCurrent = objBuyerCat_Ext.chkCommonVendor(aryProdCode)

            If aryVendorCurrent.Count = 0 Then
                aryVendorCurrent = Nothing
                strMsg = "No common vendor."
                Common.NetMsgbox(Me, "No common vendor.", MsgBoxStyle.Information)
                Exit Sub
            End If

            If aryProdCode.Count > 1 Then
                aryVendorCurrent = objBuyerCat_Ext.chkCommonVenCode(aryProdCode, aryVendorCurrent)

                If aryVendorCurrent.Count = 0 Then
                    Common.NetMsgbox(Me, "No common vendor code.", MsgBoxStyle.Information)
                    Exit Sub
                End If
            End If

            Session("ProdList") = aryProdCode 'aryProdCode
            Session("VendorList") = aryVendorCurrent
            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
            cmdSearch_Click(cmdSearch, New System.EventArgs())
        Else
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
        End If
    End Sub
End Class