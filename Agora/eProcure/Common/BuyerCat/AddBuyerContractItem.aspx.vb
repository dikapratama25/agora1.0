Imports AgoraLegacy
Imports eProcure.Component

Public Class AddBuyerContractItem
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents dtgItem As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents rdAll As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rdSelected As System.Web.UI.WebControls.RadioButton
    Protected WithEvents cboVendor As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Reset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents cboContract As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents rfvVendor As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents rfvContract As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldSummary As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Enum Items
        icChk = 0
        icVIC = 1
        icDesc = 2
        icUOM = 3
        icCurrency = 4
        icPrice = 5
        icItemCode = 6
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgItem)

        If Not IsPostBack Then
            viewstate("mode") = UCase(Request.QueryString("mode"))
            viewstate("index") = Request.QueryString("index")
            viewstate("code") = Request.QueryString("code")
            viewstate("name") = Request.QueryString("name")
            viewstate("blnCanAdd") = False
            viewstate("blnCanDelete") = False
            Select Case viewstate("mode")
                Case "A"
                    lblHeader.Text = "Add Contract Item"
                    lblTitle.Text = "Add Contract Item"
                    cmdSave.Text = "Save"
                    rdAll.Text = "Add All Contract Item"
                    rdSelected.Text = "Add Selected Contract Item"
                Case "D"
                    lblHeader.Text = "Delete Contract Item"
                    lblTitle.Text = "Delete Contract Item"
                    cmdSave.Text = "Delete"
                    rdAll.Text = "Delete All Contract Item"
                    rdSelected.Text = "Delete Selected Contract Item"
            End Select
            bindVendor()
            bindContract()
            rdAll.Checked = True
            lnkBack.NavigateUrl = dDispatcher.direct("BuyerCat", "ItemCatalogue.aspx", "PageId=" & strPageId & "&index=" & ViewState("index") & "&code=" & ViewState("code") & "&name=" & ViewState("name"))
        End If
        cboVendor.Attributes.Add("onclick", "enableCboContract(1);")
        cboVendor.Attributes.Add("onblur", "enableCboContract(0);")
    End Sub

    Private Sub bindVendor()
        Dim objCat As New ContCat
        Dim ds As New DataSet
        ds = objCat.getVendorWithValidContract(viewstate("mode"), viewstate("index"))
        cboVendor.Items.Clear()
        If ds.Tables(0).Rows.Count > 0 Then
            Common.FillDdl(cboVendor, "CM_COY_NAME", "CM_COY_ID", ds.Tables(0).DefaultView, "")
        End If

        Dim lstItem As New ListItem
        lstItem.Value = ""
        lstItem.Text = "---Select---"
        cboVendor.Items.Insert(0, lstItem)

        objCat = Nothing
    End Sub

    Private Sub bindContract()
        Dim objCat As New ContCat
        Dim ds As New DataSet
        cboContract.Items.Clear()
        ds = objCat.getContractGroupRef(cboVendor.SelectedValue, viewstate("mode"), viewstate("index"))
        If ds.Tables(0).Rows.Count > 0 Then
            Common.FillDdl(cboContract, "GROUP_DESC", "CDM_GROUP_INDEX", ds.Tables(0).DefaultView)
        End If
        Dim lstItem As New ListItem
        lstItem.Value = ""
        lstItem.Text = "---Select---"
        cboContract.Items.Insert(0, lstItem)
        objCat = Nothing
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim dt As DataTable
        Dim objBuyerCat As New BuyerCat

        dt = objBuyerCat.getContractItemForBuyerCatalogue(cboContract.SelectedValue, viewstate("mode"))
        Dim dvViewItem As DataView
        dvViewItem = dt.DefaultView

        dvViewItem.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewItem.Sort += " DESC"

        If viewstate("action") = "del" Then
            If dtgItem.CurrentPageIndex > 0 And dt.Rows.Count Mod dtgItem.PageSize = 0 Then
                dtgItem.CurrentPageIndex = dtgItem.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        intPageRecordCnt = dt.Rows.Count
        viewstate("intPageRecordCnt") = intPageRecordCnt
        'Session("table") = ds.Tables(0)

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgItem, dvViewItem)
            cmdSave.Enabled = True
            viewstate("blnCanAdd") = True
            viewstate("blnCanDelete") = True
            cmd_Reset.Disabled = False
            dtgItem.DataSource = dvViewItem
            dtgItem.DataBind()
        Else
            cmdSave.Enabled = False
            viewstate("blnCanAdd") = False
            viewstate("blnCanDelete") = False
            cmd_Reset.Disabled = True
            dtgItem.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            bindContract()
        End If

        ' add for above checking
        viewstate("PageCount") = dtgItem.PageCount
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdSave.Enabled = False
        Dim alButtonList As ArrayList

        If viewstate("mode") = "A" Then
            alButtonList = New ArrayList
            alButtonList.Add(cmdSave)
            htPageAccess.Add("add", alButtonList)
            alButtonList = New ArrayList
            alButtonList.Add(cmdSave)
            htPageAccess.Add("update", alButtonList)
        Else
            alButtonList = New ArrayList
            alButtonList.Add(cmdSave)
            htPageAccess.Add("delete", alButtonList)
        End If

        CheckButtonAccess()
        '//additional checking
        If viewstate("mode") = "A" Then
            cmdSave.Enabled = (blnCanAdd Or blnCanUpdate) And viewstate("blnCanAdd")
        Else
            cmdSave.Enabled = blnCanDelete And viewstate("blnCanDelete")
        End If
    End Sub

    Private Sub dtgItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            If rdAll.Checked Then
                e.Item.Cells(Items.icChk).Visible = False
            Else
                e.Item.Cells(Items.icChk).Visible = True
            End If
        ElseIf e.Item.ItemType = ListItemType.Header Then
            If rdAll.Checked Then
                e.Item.Cells(Items.icChk).Visible = False
            Else
                e.Item.Cells(Items.icChk).Visible = True
            End If
        End If
    End Sub

    Private Sub dtgItem_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgItem.PageIndexChanged
        dtgItem.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub dtgItem_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgItem.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgItem, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub cboVendor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboVendor.SelectedIndexChanged
        bindContract()
        dtgItem.DataBind()        
        If cboVendor.SelectedIndex <= 0 Then
            viewstate("blnCanAdd") = False
            viewstate("blnCanDelete") = False
        Else
            If cboContract.SelectedIndex <= 0 Then
                viewstate("blnCanAdd") = False
                viewstate("blnCanDelete") = False
            End If
        End If

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        Bindgrid(dtgItem.CurrentPageIndex, True)
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        viewstate("Type") = IIf(rdAll.Checked, "A", "S")
        Bindgrid()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim dtProduct As New DataTable
        Dim objProduct As New Products
        Dim dr As DataRow
        Dim i As Integer
        Dim objBuyerCat As New BuyerCat

        Select Case viewstate("mode")
            Case "A" ' Add
                If viewstate("Type") = "A" Then

                    'Dim dt As DataTable
                    dtProduct = objBuyerCat.getContractItemForBuyerCatalogue(cboContract.SelectedValue, viewstate("mode"))
                    'For i = 0 To dt.Rows.Count - 1
                    '    dr = dtProduct.NewRow
                    '    dr("PRODUCT_CODE") = dt.Rows(i)("CDI_PRODUCT_CODE")
                    '    dr("SC_UNIT_COST") = dt.Rows(i)("CDI_UNIT_COST")
                    '    dr("GRP_INDEX") = cboContract.SelectedValue 'viewstate("index")
                    '    dr("CAT_TYPE") = "CP"
                    '    dr("VENDOR_ITEM_CODE") = dt.Rows(i)("CDI_VENDOR_ITEM_CODE")
                    '    dr("S_COY_ID") = cboVendor.SelectedValue
                    '    dtProduct.Rows.Add(dr)
                    'Next

                Else
                    dtProduct.Columns.Add("PRODUCT_CODE", Type.GetType("System.String")) '//product code
                    dtProduct.Columns.Add("S_COY_ID", Type.GetType("System.String")) '//supplier id
                    dtProduct.Columns.Add("VENDOR_ITEM_CODE", Type.GetType("System.String")) '//Vendor ITem Code
                    dtProduct.Columns.Add("SC_UNIT_COST", Type.GetType("System.Double")) '//unit cost
                    dtProduct.Columns.Add("GRP_INDEX", Type.GetType("System.Int32")) '//group index
                    dtProduct.Columns.Add("CAT_TYPE", Type.GetType("System.String")) '//unit cost
                    dtProduct.Columns.Add("msg", Type.GetType("System.String"))  '//error msg

                    Dim grdItem As DataGridItem
                    For Each grdItem In dtgItem.Items
                        Dim chkSel As CheckBox = grdItem.Cells(Items.icChk).FindControl("chkSelection")
                        If chkSel.Checked Then
                            dr = dtProduct.NewRow
                            dr("PRODUCT_CODE") = grdItem.Cells(Items.icItemCode).Text
                            dr("SC_UNIT_COST") = IIf(grdItem.Cells(Items.icPrice).Text = "0", -1, grdItem.Cells(Items.icPrice).Text)
                            dr("GRP_INDEX") = cboContract.SelectedValue ' viewstate("index")
                            dr("CAT_TYPE") = "CP"
                            dr("VENDOR_ITEM_CODE") = grdItem.Cells(Items.icVIC).Text
                            dr("S_COY_ID") = cboVendor.SelectedValue
                            dtProduct.Rows.Add(dr)
                        End If
                    Next
                End If

                objProduct.addToBuyerCat(viewstate("index"), dtProduct)
                objProduct = Nothing
                Session("dt") = dtProduct
                Response.Redirect(dDispatcher.direct("Product", "DisplayAddedItem.aspx", "pageid=" & strPageId & "&type=" & "B" & "&id=" & ViewState("index") & "&type1=BF&code=" & Server.UrlEncode(ViewState("code")) & "&name=" & Server.UrlEncode(ViewState("name"))))

            Case "D" ' Delete
                If viewstate("Type") = "A" Then
                    If objBuyerCat.deleteBuyerItemByGrpIndex(cboContract.SelectedValue) = WheelMsgNum.Delete Then
                        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
                    Else
                        Common.NetMsgbox(Me, MsgRecordNotDelete, MsgBoxStyle.Information)
                    End If
                    'Dim dt As DataTable = Session("table")
                    'For i = 0 To dt.Rows.Count - 1
                    '    dr = dtProduct.NewRow
                    '    dr("index") = viewstate("index")
                    '    dr("code") = dt.Rows(i)("CDI_PRODUCT_CODE")
                    '    dr("source") = "CP"
                    '    dtProduct.Rows.Add(dr)
                    'Next
                Else
                    dtProduct.Columns.Add("index", Type.GetType("System.String")) '//product code
                    dtProduct.Columns.Add("code", Type.GetType("System.String")) '//supplier id
                    dtProduct.Columns.Add("source", Type.GetType("System.String")) '//Vendor ITem Code

                    Dim grdItem As DataGridItem
                    For Each grdItem In dtgItem.Items
                        Dim chkSel As CheckBox = grdItem.Cells(Items.icChk).FindControl("chkSelection")
                        If chkSel.Checked Then
                            dr = dtProduct.NewRow
                            dr("index") = viewstate("index")
                            dr("code") = grdItem.Cells(Items.icItemCode).Text
                            dr("source") = "CP"
                            dtProduct.Rows.Add(dr)
                        End If
                    Next

                    If objBuyerCat.delBuyerItem(dtProduct) = WheelMsgNum.Delete Then
                        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
                    Else
                        Common.NetMsgbox(Me, MsgRecordNotDelete, MsgBoxStyle.Information)
                    End If
                End If

                viewstate("action") = "del"
                Bindgrid()
        End Select
        objBuyerCat = Nothing
    End Sub

    Private Sub rdAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdAll.CheckedChanged
        If rdAll.Checked Then
            bind()
        End If
    End Sub

    Private Sub rdSelected_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdSelected.CheckedChanged
        If rdSelected.Checked Then
            bind()
        End If
    End Sub

    Private Sub bind()
        viewstate("Type") = IIf(rdAll.Checked, "A", "S")
        If cboVendor.SelectedValue <> "" And cboContract.SelectedValue <> "" Then
            Bindgrid()           
        Else
            dtgItem.DataBind()
            viewstate("blnCanAdd") = False
            viewstate("blnCanDelete") = False
        End If
    End Sub

    Private Sub cboContract_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboContract.SelectedIndexChanged
        bind()
    End Sub
End Class
