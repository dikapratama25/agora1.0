Imports AgoraLegacy
Imports eProcure.Component



Public Class ContractCatSearchSEH
    Inherits AgoraLegacy.AppBaseClass
    Dim objGlobal As New AppGlobals
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strOversea, strItemType, strVendor, strCurr, strChkVendor As String

    Public Enum EnumList
        icChk = 0
        icVendItemCode = 1
        icProductCode = 2
        icItemName = 3
        icConRefNo = 4
        icConDesc = 5
        icVendor = 6
        icUOM = 7
        icCurrency = 8
        icPrice = 9
        icGST = 10
        icGSTRate = 11
        icGSTTaxCode = 12
        icSpec = 13
        ic1stSpec = 14
        ic2ndSpec = 15
        ic3rdSpec = 16
        icRemarks = 17
        icGIndex = 18
        icOversea = 19
        icItemType = 20
        icVendorId = 21
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strNewCSS = "both"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgItem)
        If Not Page.IsPostBack Then
            ' objGlobal.FillCommodityType(Me.txtCommodity)
            Me.cmd_back.Attributes.Add("onclick", "window.close();")
            DisplayUserCheckBtn()
            BindConCat()
            ViewState("Concat") = Request.QueryString("frm")
            ViewState("Show") = Request.QueryString("show")

            If ViewState("Concat") = "Concat" Then
                lblTitle.Text = "Add Item"
                RowHide1.Style.Item("display") = "none"
                RowHide2.Style.Item("display") = "none"
                RowHide3.Style.Item("display") = "none"
            End If

        End If

        Session("CurrentScreen") = "AddItem"

        strOversea = Request.QueryString("Oversea")
        strItemType = Request.QueryString("ItemType")
        strVendor = Request.QueryString("Vendor")
        strCurr = Request.QueryString("Curr")
        strChkVendor = Request.QueryString("chkvendor")

        If strItemType = "SP" Then
            'chkSpot.Enabled = True
            chkStock.Enabled = False
            chkMRO.Enabled = False
        ElseIf strItemType = "ST" Then
            chkSpot.Enabled = False
            'chkStock.Enabled = True
            chkMRO.Enabled = False
        ElseIf strItemType = "MI" Then
            chkSpot.Enabled = False
            chkStock.Enabled = False
            'chkMRO.Enabled = True
        End If

        cmdPR.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
        cmdPO.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
        cmd_Save.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','Save');")
        Session("urlreferer") = "ConCatSearch"
    End Sub

    Private Sub CtrlButton()
        Dim objUsers As New Users
        Dim objUserDetails As New User

        objUserDetails = objUsers.GetUserDetails(Session("UserId"), Session("CompanyId"))
        If objUsers.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Buyer) Then
            cmdPR.Visible = True
            cmdPO.Visible = False
        ElseIf objUsers.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Purchasing_Officer) Or objUsers.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Purchasing_Manager) Then
            cmdPR.Visible = False
            cmdPO.Visible = True
        End If
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
        Dim objCat_Ext As New ContCat_Ext
        Dim ds As New DataSet
        Dim strCommodity As String = ""
        Dim aryItemType As New ArrayList
        Dim intCatalogue As Integer = 0
        Dim strSpt, strStk, strMro As String

        If Me.txtCommodity.Text <> "" Then
            strCommodity = Me.hidCommodity.Value
        End If

        If Me.cboCatalogue.SelectedItem.Text <> "---Select---" And cboCatalogue.SelectedIndex > 0 Then
            intCatalogue = Me.cboCatalogue.SelectedValue
        Else
            intCatalogue = 0
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

        If txtVendor.Text = "" Then
            hidVendor.Text = ""
        End If

        ds = objCat_Ext.getConCatSearchList(intCatalogue, txtItemCode.Text, txtItemName.Text, strCommodity, aryItemType, hidVendor.Text, strSpt, strStk, strMro, cboConcatDesc.SelectedItem.Value, aryItemType, strOversea, strCurr, strVendor, strChkVendor)

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            If ViewState("Concat") = "Concat" Then
                CtrlButton()
                cmd_Save.Visible = True
                cmd_back.Visible = True
            Else
                CtrlButton()
                cmd_Save.Visible = False
                cmd_back.Visible = False
            End If
            resetDatagridPageIndex(dtgItem, dvViewSample)
            dtgItem.DataSource = dvViewSample
            dtgItem.DataBind()
        Else
            'If ViewState("Concat") = "Concat" Then
            '    CtrlButton()
            '    cmd_Save.Visible = False
            '    cmd_back.Visible = False
            'Else
            '    CtrlButton()
            '    cmd_Save.Visible = True
            '    cmd_back.Visible = True
            'End If
            cmdPR.Visible = False
            cmdPO.Visible = False
            cmd_Save.Visible = False
            cmd_back.Visible = False
            Common.NetMsgbox(Me, MsgNoRecord)
            dtgItem.DataBind()
        End If

        If ViewState("Show") = "no" Then
            cmdPO.Visible = False
            cmdPR.Visible = False
        End If

        ' add for above checking
        ViewState("PageCount") = dtgItem.PageCount
        objCat_Ext = Nothing
    End Function

    Private Sub BindConCat()
        Dim dsCat As New DataSet
        Dim cbolist As New ListItem
        Dim objCat As New ContCat

        dsCat = objCat.getConRefNoSearch() 'objCat.getConRefNo()
        cboCatalogue.Items.Clear()

        If Not dsCat Is Nothing Then
            cboCatalogue.Enabled = True
            Common.FillDdl(cboCatalogue, "CDM_GROUP_CODE", "CDM_GROUP_INDEX", dsCat)
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboCatalogue.Items.Insert(0, cbolist)

        'bind description        
        Dim dsDesc As New DataSet
        Dim cboDesclist As New ListItem

        dsDesc = objCat.getConDesc() 'objCat.getConRefNo()
        cboConcatDesc.Items.Clear()

        If Not dsDesc Is Nothing Then
            cboConcatDesc.Enabled = True
            Common.FillDdl(cboConcatDesc, "CDM_GROUP_DESC", "CDM_GROUP_DESC", dsDesc)
        End If
        cboDesclist.Value = ""
        cboDesclist.Text = "---Select---"
        cboConcatDesc.Items.Insert(0, cboDesclist)

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
            Session("UrlLocation") = "ConCatSearch"
            lnkCode.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=B&pageid=" & strPageId & "&pid=" & dv("CDI_PRODUCT_CODE") & "&index=&draft=0&ref=" & dv(3) & "") & "')")
            'lnkCode.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=B&pageid=" & strPageId & "&pid=" & dv("CDI_PRODUCT_CODE") & "&index=&draft=0") & "')")
            'lnkCode.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=B&pageid=" & strPageId & "&pid=" & dv("CDI_PRODUCT_CODE") & "&index=" & dv("CDI_GROUP_INDEX") & "&draft=0") & "')")

            lnkCode.Text = dv("CDI_VENDOR_ITEM_CODE")

            e.Item.Cells(EnumList.icPrice).Text = Format(CDbl(e.Item.Cells(EnumList.icPrice).Text), "###,###,##0.0000")
            e.Item.Cells(EnumList.icGST).Text = Format(CDbl(e.Item.Cells(EnumList.icGST).Text), "##0")
            If dv("CDI_GST") <= 0 Then
                e.Item.Cells(EnumList.icGST).Text = ""
            End If

            Dim lblProductDesc As Label
            lblProductDesc = e.Item.FindControl("lblProductDesc")
            lblProductDesc.Text = Common.parseNull(dv("CDI_PRODUCT_DESC"))

            Dim lblVenID As Label
            lblVenID = e.Item.FindControl("lblVenID")
            lblVenID.Text = Common.parseNull(dv("CDM_S_COY_ID"))

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
        ViewState("SortExpression") = "CDI_PRODUCT_DESC"
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
        Dim aryProdCode As New ArrayList
        Dim strMsg As String = ""

        If checkMandatory(strMsg) Then
            If ChkVendor(strMsg, aryProdCode) Then
                Session("ProdList") = aryProdCode
                Response.Redirect(dDispatcher.direct("PR", "RaisePR.aspx", "frm=ConCat&pageid=" & strPageId))

            Else
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            End If
        Else
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Function ChkVendorPO(ByRef strMsg As String, ByRef aryProdCodeList As ArrayList, ByRef aryVenList As ArrayList) As Boolean

        Dim chkItem, chkItem1 As CheckBox
        Dim aryProdCode, aryVen As New ArrayList
        Dim strOverseaP, strItemTypeP, strVendorP, strCurrP, strVenCurr As String
        Dim i, j, k As Integer
        Dim objDb As New EAD.DBCom

        k = 0
        'Do a loop to check whether there are any common vendors
        For i = 0 To dtgItem.Items.Count() - 1
            chkItem = dtgItem.Items(i).Cells(EnumList.icChk).FindControl("chkSelection")

            If chkItem.Checked Then
                If k = 0 Then
                    strOverseaP = dtgItem.Items(i).Cells(EnumList.icOversea).Text
                    strItemTypeP = dtgItem.Items(i).Cells(EnumList.icItemType).Text
                    strVendorP = dtgItem.Items(i).Cells(EnumList.icVendor).Text
                    strCurrP = dtgItem.Items(i).Cells(EnumList.icCurrency).Text
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

                    If strVendorP = dtgItem.Items(i).Cells(EnumList.icVendor).Text Then

                    Else
                        strMsg = "Selected items are not from the same vendor."
                        Return False
                        Exit Function
                    End If

                    If strCurrP = dtgItem.Items(i).Cells(EnumList.icCurrency).Text Then

                    Else
                        If strItemTypeP <> "ST" Then
                            strMsg = "Selected items are not from the same currency AND item's currency doesn't match with contract vendor's currency."
                            Return False
                            Exit Function
                        End If
                    End If
                End If

                If strItemTypeP = "ST" Then
                    strVenCurr = objDb.GetVal("SELECT CM_CURRENCY_CODE FROM COMPANY_MSTR WHERE CM_COY_ID = '" & dtgItem.Items(i).Cells(EnumList.icVendorId).Text & "'")
                    If strVenCurr <> dtgItem.Items(i).Cells(EnumList.icCurrency).Text Then
                        strMsg = "Selected items are not from the same currency AND item's currency doesn't match with contract vendor's currency."
                        Return False
                        Exit Function
                    End If

                    strVenCurr = objDb.GetVal("SELECT IFNULL(PV_CURR,'') AS PV_CURR FROM PIM_VENDOR WHERE PV_PRODUCT_INDEX = (SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & dtgItem.Items(i).Cells(EnumList.icProductCode).Text & "' AND PM_S_COY_ID = '" & Session("CompanyId") & "') AND PV_CURR = '" & dtgItem.Items(i).Cells(EnumList.icCurrency).Text & "' AND PV_S_COY_ID = '" & dtgItem.Items(i).Cells(EnumList.icVendorId).Text & "'")
                    If strVenCurr <> dtgItem.Items(i).Cells(EnumList.icCurrency).Text Then
                        strMsg = "Selected items do not have common currency AND item's currency doesn't match with contract vendor's currency."
                        Return False
                        Exit Function
                    End If
                End If

                'aryProdCode.Add(dtgItem.Items(i).Cells(EnumList.icProductCode).Text)
                aryProdCode.Add(New String() {dtgItem.Items(i).Cells(EnumList.icProductCode).Text, "", "", "", dtgItem.Items(i).Cells(EnumList.icGIndex).Text, ""})
                aryVen.Add(CType(dtgItem.Items(i).FindControl("lblVenID"), Label).Text)
                k = k + 1
            End If
        Next

        aryProdCodeList = aryProdCode
        aryVenList = aryVen

        Return True
    End Function

    'Private Function ChkVendor(ByRef strMsg As String, ByRef aryProdCodeList As ArrayList, ByRef dtVendorList As DataTable) As Boolean
    Private Function ChkVendor(ByRef strMsg As String, ByRef aryProdCodeList As ArrayList) As Boolean

        Dim chkItem, chkItem1 As CheckBox
        Dim aryProdCode As New ArrayList
        Dim strOverseaP, strItemTypeP, strVendorP, strCurrP, strVenCurr As String
        Dim i, j, k As Integer
        Dim objDb As New EAD.DBCom

        k = 0
        'Do a loop to check whether there are any common vendors
        For i = 0 To dtgItem.Items.Count() - 1
            chkItem = dtgItem.Items(i).Cells(EnumList.icChk).FindControl("chkSelection")
            
            If chkItem.Checked Then
                If k = 0 Then
                    strOverseaP = dtgItem.Items(i).Cells(EnumList.icOversea).Text
                    strItemTypeP = dtgItem.Items(i).Cells(EnumList.icItemType).Text
                    'strVendorP = dtgItem.Items(i).Cells(EnumList.icVendor).Text
                    strCurrP = dtgItem.Items(i).Cells(EnumList.icCurrency).Text
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

                    'If strVendorP = dtgItem.Items(i).Cells(EnumList.icVendor).Text Then

                    'Else
                    '    strMsg = "Selected items are not from the same vendor."
                    '    Return False
                    '    Exit Function
                    'End If

                    If strCurrP = dtgItem.Items(i).Cells(EnumList.icCurrency).Text Then

                    Else
                        strMsg = "Selected items are not from the same currency AND item's currency doesn't match with contract vendor's currency."
                        Return False
                        Exit Function
                    End If
                End If

                strVenCurr = objDb.GetVal("SELECT CM_CURRENCY_CODE FROM COMPANY_MSTR WHERE CM_COY_ID = '" & dtgItem.Items(i).Cells(EnumList.icVendorId).Text & "'")
                If strVenCurr <> dtgItem.Items(i).Cells(EnumList.icCurrency).Text Then
                    strMsg = "Selected items are not from the same currency AND item's currency doesn't match with contract vendor's currency."
                    Return False
                    Exit Function
                End If

                aryProdCode.Add(New String() {dtgItem.Items(i).Cells(EnumList.icProductCode).Text, "", "", "", dtgItem.Items(i).Cells(EnumList.icGIndex).Text, ""})
                k = k + 1
            End If

        Next

        aryProdCodeList = aryProdCode
        Return True
    End Function

    Private Sub cmd_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Save.Click
        Dim chkItem As CheckBox
        Dim aryProdCode As New ArrayList
        Dim i As Integer

        aryProdCode = Session("ProdList")
        cmd_Save.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','Save');")
        For i = 0 To dtgItem.Items.Count() - 1
            chkItem = dtgItem.Items(i).Cells(0).FindControl("chkSelection")

            If chkItem.Checked Then
                If i = 0 Then 'keep track of the 1st row that is checked
                    aryProdCode.Add(New String() {dtgItem.Items(i).Cells(EnumList.icProductCode).Text, "", "", "", dtgItem.Items(i).Cells(EnumList.icGIndex).Text, ""})
                Else
                    aryProdCode.Add(New String() {dtgItem.Items(i).Cells(EnumList.icProductCode).Text, "", "", "", dtgItem.Items(i).Cells(EnumList.icGIndex).Text, ""})
                End If
            End If
        Next

        Session("ProdList") = aryProdCode
        Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
        cmdSearch_Click(cmdSearch, New System.EventArgs())
    End Sub

    Private Sub cmdPO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPO.Click
        Dim aryProdCode, aryVendor As New ArrayList
        Dim strMsg As String = ""

        If checkMandatory(strMsg) Then
            If ChkVendorPO(strMsg, aryProdCode, aryVendor) Then
                Session("ProdList") = aryProdCode
                Session("VendorList") = aryVendor
                Response.Redirect(dDispatcher.direct("PO", "RaisePO.aspx", "type=new&mode=cc&pageid=90"))

            Else
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
            End If
        Else
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation)
        End If
    End Sub

    Sub DisplayUserCheckBtn()
        Dim objUser_Ext As New Users_Ext
        Dim objUsers As New Users
        Dim objUserDetails As New User
        Dim dsStk As New DataSet

        dsStk = objUser_Ext.GetUserStockType(Session("UserId"), Session("CompanyId"))
        objUserDetails = objUsers.GetUserDetails(Session("UserId"), Session("CompanyId"))

        If objUsers.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Buyer) Then
            chkSpot.Enabled = True
            chkStock.Enabled = True
            chkMRO.Enabled = True
        Else
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
        End If

    End Sub
End Class