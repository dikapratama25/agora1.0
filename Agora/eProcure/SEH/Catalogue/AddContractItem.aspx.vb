Imports AgoraLegacy
Imports eProcure.Component


Public Class AddContractItem_SEH
    Inherits AgoraLegacy.AppBaseClass

    Public Enum EnumCat
        icChk = 0
        icItemCode = 1
        icItemDesc = 2
        icItemType = 3
        icUOM = 4
        icCurrency = 5
        icPrice = 6
        icTax = 7
        icGSTRate = 8
        icGSTCode = 9
        icRemarks = 10
        icProdCode = 11
    End Enum

    Public Enum EnumCatMod
        icItemCode = 0
        icItemDesc = 1
        icUOM = 2
        icCurrency = 3
        icPrice = 4
        icTax = 5
        icGSTRate = 6
        icGSTCode = 7
        icRemarks = 8
        icProdCode = 9
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents dtgCatalogue As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents dtgCatalogueMod As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClose As System.Web.UI.WebControls.Button
    Protected WithEvents txtVendorItemCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblBuyer As System.Web.UI.WebControls.Label
    Protected WithEvents lblDesc As System.Web.UI.WebControls.Label
    Protected WithEvents lblCodeLabel As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents cboCommodityType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents chkSpot As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkStock As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkMRO As System.Web.UI.WebControls.CheckBox
    Protected WithEvents trSearch As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSearchCriteria1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSearchCriteria2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents trAddItem As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trModItem As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents hidCommodity As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtCommodity As System.Web.UI.WebControls.TextBox
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If intPageRecordCnt > 0 Then
            cmdSave.Enabled = True
        Else
            cmdSave.Enabled = False
        End If

    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objGlobal As New AppGlobals
        'Put user code to initialize the page here
        strNewCSS = "both"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCatalogue)
        SetGridProperty(dtgCatalogueMod)

        If Not IsPostBack Then
            ViewState("mode") = Request.QueryString("mode")
            ViewState("index") = Request.QueryString("index")
            ViewState("strIndexList") = Session("ContractItemCodeList")

            'Get Vendor belong to this contract catalogue
            Dim objDb As New EAD.DBCom
            ViewState("ContractVCoyId") = objDb.GetVal("SELECT CDM_S_COY_ID FROM CONTRACT_DIST_MSTR WHERE CDM_GROUP_INDEX = '" & ViewState("index") & "'")

            'Check whether vendor company is GST registration company or not
            ViewState("strGstRegNo") = objDb.GetVal("SELECT IFNULL(CM_TAX_REG_NO,'') FROM COMPANY_MSTR WHERE CM_COY_ID = '" & ViewState("ContractVCoyId") & "'")
            objDb = Nothing

            'Check whether it is before GST/ after GST
            Dim objGst As New GST
            ViewState("GSTCOD") = objGst.chkGSTCOD()
            objGst = Nothing

            'Get Default values of GST Rate & GST Tax Code from approved vendor 
            Dim objAdmin As New Admin
            Dim strGstRate, strGstTaxCode As String
            Dim dsGst As DataSet = objAdmin.searchvendor("AV", ViewState("ContractVCoyId"), "")
            If dsGst.Tables(0).Rows.Count > 0 Then
                strGstRate = Common.parseNull(dsGst.Tables(0).Rows(0)("CV_GST_RATE"))
                strGstTaxCode = Common.parseNull(dsGst.Tables(0).Rows(0)("CV_GST_TAX_CODE"))
            Else
                strGstRate = "STD"
                strGstTaxCode = ""
            End If
            ViewState("GstRate") = strGstRate
            ViewState("GstTaxCode") = strGstTaxCode

            If ViewState("mode") = "add" Then
                lblTitle.Text = "Assign Items"
                trSearch.Visible = True
                trSearchCriteria1.Visible = True
                trSearchCriteria2.Visible = True
                trAddItem.Visible = True
                trModItem.Visible = False
                cmdSave.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")

            Else
                lblTitle.Text = "Modify Items"
                trSearch.Visible = False
                trSearchCriteria1.Visible = False
                trSearchCriteria2.Visible = False
                trAddItem.Visible = False
                trModItem.Visible = True
                dtgCatalogue.CurrentPageIndex = 0
                ViewState("SortAscending") = "yes"
                ViewState("SortExpression") = "CDI_VENDOR_ITEM_CODE"
                BindgridMod()
                cmdSave.Attributes.Clear()
            End If

            DisplayUserCheckBtn()
        Else
            fillTaxCode()
        End If

    End Sub

    Private Function Bindgrid() As String
        Dim objCat As New ContCat
        Dim objCat_Ext As New ContCat_Ext
        Dim ds As New DataSet
        Dim strCommodity As String = ""
        Dim aryItemType As New ArrayList
        Dim strSpt, strStk, strMro As String

        If Me.txtCommodity.Text <> "" Then
            strCommodity = Me.hidCommodity.Value
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

        ds = objCat_Ext.getConCatItemList(ViewState("index"), txtVendorItemCode.Text, txtDesc.Text, strCommodity, aryItemType, strSpt, strStk, strMro)

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgCatalogue, dvViewSample)

            dtgCatalogue.DataSource = dvViewSample
            dtgCatalogue.DataBind()
            ViewState("blnCmdContract") = True
            cmdSave.Enabled = True
        Else
            Common.NetMsgbox(Me, MsgNoRecord)
            ViewState("blnCmdContract") = False
            dtgCatalogue.DataBind()
            cmdSave.Enabled = False
        End If

        If ViewState("GSTCOD") = True Then
            dtgCatalogue.Columns(EnumCat.icTax).Visible = False
            dtgCatalogue.Columns(EnumCat.icGSTRate).Visible = True
            dtgCatalogue.Columns(EnumCat.icGSTCode).Visible = True
        Else
            dtgCatalogue.Columns(EnumCat.icTax).Visible = True
            dtgCatalogue.Columns(EnumCat.icGSTRate).Visible = False
            dtgCatalogue.Columns(EnumCat.icGSTCode).Visible = False
        End If

        ' add for above checking
        ViewState("PageCount") = dtgCatalogue.PageCount
        objCat = Nothing
    End Function

    Private Function BindgridMod() As String
        Dim objCat As New ContCat
        Dim ds As New DataSet
        Dim strAryList() As String

        If Not ViewState("strIndexList") Is Nothing Then
            strAryList = CStr(ViewState("strIndexList").Substring(0, ViewState("strIndexList").Length - 1)).Split(",")
        End If
        ds = objCat.getContractItem(strAryList, ViewState("index"))

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgCatalogueMod, dvViewSample)

            dtgCatalogueMod.DataSource = dvViewSample
            dtgCatalogueMod.DataBind()
            ViewState("blnCmdContract") = True
            cmdSave.Enabled = True
        Else
            Common.NetMsgbox(Me, MsgNoRecord)
            ViewState("blnCmdContract") = False
            dtgCatalogueMod.DataBind()
            cmdSave.Enabled = False
        End If

        If ViewState("GSTCOD") = True Then
            dtgCatalogueMod.Columns(EnumCatMod.icTax).Visible = False
            dtgCatalogueMod.Columns(EnumCatMod.icGSTRate).Visible = True
            dtgCatalogueMod.Columns(EnumCatMod.icGSTCode).Visible = True
        Else
            dtgCatalogueMod.Columns(EnumCatMod.icTax).Visible = True
            dtgCatalogueMod.Columns(EnumCatMod.icGSTRate).Visible = False
            dtgCatalogueMod.Columns(EnumCatMod.icGSTCode).Visible = False
        End If

        ' add for above checking
        ViewState("PageCount") = dtgCatalogueMod.PageCount
        objCat = Nothing
    End Function

    Public Sub SortCommandMode_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgCatalogueMod.CurrentPageIndex = 0
        BindgridMod()
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgCatalogue.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgCatalogue.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "PM_Vendor_Item_Code"
        Bindgrid()
    End Sub

    Private Sub dtgCatalogue_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgCatalogue, e)

        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dtgCatalogue_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogue.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lstItem As New ListItem
            Dim lstItem2 As New ListItem

            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(EnumCat.icChk).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim objrfq As New RFQ
            Dim objGlobal As New AppGlobals
            Dim ddl_tax As New DropDownList
            ddl_tax = e.Item.FindControl("ddl_tax")
            objGlobal.FillTax(ddl_tax)

            Dim ddl_GSTRate As New DropDownList
            Dim ddl_GSTCode As New DropDownList
            Dim lbl_GSTRate As New Label
            ddl_GSTRate = e.Item.FindControl("ddl_GSTRate")
            ddl_GSTCode = e.Item.FindControl("ddl_GSTCode")
            lbl_GSTRate = e.Item.FindControl("lbl_GSTRate")

            If ViewState("strGstRegNo") <> "" Then
                ddl_GSTRate.Enabled = True
                ddl_GSTCode.Enabled = True
                objGlobal.FillGST(ddl_GSTRate, False)

                Common.SelDdl(ViewState("GstRate"), ddl_GSTRate)
                lbl_GSTRate.Text = ViewState("GstRate")

                If ViewState("GstRate") <> "" Then
                    objGlobal.FillTaxCode(ddl_GSTCode, ViewState("GstRate"), "P", , False)
                Else
                    objGlobal.FillTaxCode(ddl_GSTCode, "STD", "P", , True)
                End If
                Common.SelDdl(ViewState("GstTaxCode"), ddl_GSTCode)

                If ddl_GSTCode.SelectedValue = "N/A" Then
                    ddl_GSTCode.Enabled = False
                Else
                    ddl_GSTCode.Enabled = True
                End If
            Else
                ddl_GSTRate.Items.Clear()
                ddl_GSTCode.Items.Clear()
                lstItem.Value = "N/A"
                lstItem.Text = "N/A"
                ddl_GSTRate.Items.Insert(0, lstItem)
                lstItem2.Value = "NR"
                lstItem2.Text = "NR"
                ddl_GSTCode.Items.Insert(0, lstItem2)
                lbl_GSTRate.Text = "N/A"
                ddl_GSTRate.Enabled = False
                ddl_GSTCode.Enabled = False
            End If

            Dim ddl_curr As New DropDownList
            ddl_curr = e.Item.FindControl("ddl_curr")
            objGlobal.FillCodeTable(ddl_curr, CodeTable.Currency)
            Common.SelDdl(dv("CM_CURRENCY_CODE"), ddl_curr)

            Dim rev_price As RegularExpressionValidator
            rev_price = e.Item.FindControl("rev_price")
            rev_price.ControlToValidate = "txt_price"
            rev_price.ValidationExpression = "^\d{1,10}(\.\d{1,4})?$"
            rev_price.ErrorMessage = "Contract Price is expecting numeric value."
            rev_price.Text = "?"
            rev_price.Display = ValidatorDisplay.Dynamic

            If dv("PM_ITEM_TYPE") = "SP" Then
                e.Item.Cells(EnumCat.icItemType).Text = "Spot"

            ElseIf dv("PM_ITEM_TYPE") = "ST" Then
                e.Item.Cells(EnumCat.icItemType).Text = "Stock"

            ElseIf dv("PM_ITEM_TYPE") = "MI" Then
                e.Item.Cells(EnumCat.icItemType).Text = "MRO"
            End If
        End If
    End Sub

    Private Function validateDatagrid(ByRef strMsg As String) As Boolean
        validateDatagrid = True
        Dim dgItem As DataGridItem
        Dim chkSelection As CheckBox
        Dim ddl_GSTRate As DropDownList
        Dim txtRemark As TextBox
        Dim txtQ As TextBox
        Dim txtGSTQ As TextBox

        strMsg = "<ul type='disc'>"
        For Each dgItem In dtgCatalogue.Items
            chkSelection = dgItem.FindControl("chkSelection")
            ddl_GSTRate = dgItem.FindControl("ddl_GSTRate")
            txtRemark = dgItem.FindControl("txt_remark")
            txtQ = dgItem.FindControl("txtQ")
            txtGSTQ = dgItem.FindControl("txtGSTQ")

            If ViewState("GSTCOD") = True Then
                If chkSelection.Checked And ddl_GSTRate.SelectedValue = "" Then
                    strMsg &= "<li>GST Rate is required.<ul type='disc'></ul></li>"
                    txtGSTQ.Text = "?"
                    validateDatagrid = False
                Else
                    txtGSTQ.Text = ""
                End If
            End If

            If Not Common.checkMaxLength(txtRemark.Text, 400) Then
                strMsg &= "<li>Remarks is over limit.<ul type='disc'></ul></li>"
                txtQ.Text = "?"
                validateDatagrid = False
            Else
                txtQ.Text = ""
            End If
        Next
        strMsg &= "</ul>"
    End Function

    Private Function validateContractPrice() As Boolean
        validateContractPrice = True
        Dim dgItem As DataGridItem
        Dim i As Integer

        For Each dgItem In dtgCatalogue.Items
            Dim chkSelection As CheckBox = dgItem.Cells(0).FindControl("chkSelection")
            If chkSelection.Checked Then
                If CType(dgItem.FindControl("txt_price"), TextBox).Text = "" Then
                    i = i + 1
                End If
            End If
        Next

        If i > 0 Then
            validateContractPrice = False
            Common.NetMsgbox(Me, "Please enter Contract Price for the selected item.", MsgBoxStyle.Information)
            Exit Function
        End If

    End Function

    Private Sub dtgCatalogue_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCatalogue.PageIndexChanged
        dtgCatalogue.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language='javascript'> { window.close();}</script>")

    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim strMsg As String
        Dim intMsg As Integer

        strMsg = ""
        lblMsg.Text = ""
        If ViewState("mode") = "add" Then
            If Page.IsValid And validateDatagrid(strMsg) And validateContractPrice() Then
                Dim dtItem As New DataTable
                dtItem = bindCatalogueItem()

                If dtItem.Rows.Count > 0 Then
                    Dim objCat As New ContCat
                    intMsg = objCat.insertCatalogueItem(dtItem)

                    Select Case intMsg
                        Case WheelMsgNum.Save
                            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
                            dtgCatalogue.CurrentPageIndex = 0
                            Bindgrid()

                        Case WheelMsgNum.NotSave
                            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                    End Select
                End If

            Else
                If strMsg <> "" Then
                    lblMsg.Text = strMsg
                Else
                    lblMsg.Text = ""
                End If
            End If

        ElseIf ViewState("mode") = "mod" Then
            If Page.IsValid And validateDatagrid1(strMsg) And validateContractPrice1() Then
                Dim dtItem As New DataTable
                dtItem = bindCatalogueItemMod()

                If dtItem.Rows.Count > 0 Then
                    Dim objCat As New ContCat

                    intMsg = objCat.updateCatalogueItem(dtItem)

                    Select Case intMsg
                        Case WheelMsgNum.Save
                            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
                            dtgCatalogueMod.CurrentPageIndex = 0
                            BindgridMod()

                        Case WheelMsgNum.NotSave
                            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                    End Select
                End If

            Else
                If strMsg <> "" Then
                    lblMsg.Text = strMsg
                Else
                    lblMsg.Text = ""
                End If
            End If
        End If

    End Sub

    Private Function bindCatalogueItem() As DataTable
        Dim dtItem As New DataTable
        Dim objDb As New EAD.DBCom

        dtItem.Columns.Add("Index", Type.GetType("System.Int32"))
        dtItem.Columns.Add("ProductCode", Type.GetType("System.String"))
        dtItem.Columns.Add("VendorItemCode", Type.GetType("System.String"))
        dtItem.Columns.Add("ProductDesc", Type.GetType("System.String"))
        dtItem.Columns.Add("UOM", Type.GetType("System.String"))
        dtItem.Columns.Add("UnitCost", Type.GetType("System.Double"))
        dtItem.Columns.Add("Remark", Type.GetType("System.String"))
        dtItem.Columns.Add("GST", Type.GetType("System.Double"))
        dtItem.Columns.Add("GSTRate", Type.GetType("System.String"))
        dtItem.Columns.Add("GSTTaxCode", Type.GetType("System.String"))
        dtItem.Columns.Add("CurrencyCode", Type.GetType("System.String"))

        Dim dtr As DataRow
        Dim dgItem As DataGridItem
        For Each dgItem In dtgCatalogue.Items
            Dim chkSelection As CheckBox = dgItem.Cells(0).FindControl("chkSelection")
            If chkSelection.Checked Then
                dtr = dtItem.NewRow
                dtr("Index") = ViewState("index")
                dtr("ProductCode") = dgItem.Cells(EnumCat.icProdCode).Text
                dtr("VendorItemCode") = dgItem.Cells(EnumCat.icItemCode).Text
                dtr("ProductDesc") = dgItem.Cells(EnumCat.icItemDesc).Text
                dtr("UOM") = dgItem.Cells(EnumCat.icUOM).Text

                Dim txtPrice As TextBox
                txtPrice = dgItem.FindControl("txt_price")
                dtr("UnitCost") = txtPrice.Text

                Dim txtRemark As TextBox
                txtRemark = dgItem.FindControl("txt_remark")
                dtr("Remark") = txtRemark.Text


                'dtr("GST") = IIf(CType(dgItem.FindControl("ddl_tax"), DropDownList).SelectedItem.Text <> "N/A" And CType(dgItem.FindControl("ddl_tax"), DropDownList).SelectedItem.Text <> "---Select---", CType(dgItem.FindControl("ddl_tax"), DropDownList).SelectedItem.Text, 0)

                Dim ddl_GSTRate As DropDownList
                ddl_GSTRate = dgItem.FindControl("ddl_GSTRate")

                'dtr("GST") = objDb.GetVal("SELECT CODE_VALUE FROM CODE_MSTR WHERE CODE_CATEGORY = 'GST' AND CODE_ABBR = '" & ddl_GSTRate.SelectedValue & "'")
                'dtr("GSTRate") = ddl_GSTRate.SelectedValue

                Dim ddl_GSTCode As DropDownList
                ddl_GSTCode = dgItem.FindControl("ddl_GSTCode")
                'dtr("GSTTaxCode") = ddl_GSTCode.SelectedValue

                If ViewState("GSTCOD") = True Then
                    dtr("GST") = 0
                    dtr("GSTRate") = ddl_GSTRate.SelectedValue
                    dtr("GSTTaxCode") = ddl_GSTCode.SelectedValue
                Else
                    dtr("GST") = IIf(CType(dgItem.FindControl("ddl_tax"), DropDownList).SelectedItem.Text <> "N/A" And CType(dgItem.FindControl("ddl_tax"), DropDownList).SelectedItem.Text <> "---Select---", CType(dgItem.FindControl("ddl_tax"), DropDownList).SelectedItem.Text, 0)
                    dtr("GSTRate") = ""
                    dtr("GSTTaxCode") = ""
                End If

                Dim ddlcurr As New DropDownList
                ddlcurr = dgItem.FindControl("ddl_curr")
                dtr("CurrencyCode") = ddlcurr.SelectedItem.Value

                dtItem.Rows.Add(dtr)
            End If
        Next

        bindCatalogueItem = dtItem
    End Function

    Private Function bindCatalogueItemMod() As DataTable
        Dim objDb As New EAD.DBCom
        Dim dtItem As New DataTable

        dtItem.Columns.Add("Index", Type.GetType("System.Int32"))
        dtItem.Columns.Add("ProductCode", Type.GetType("System.String"))
        dtItem.Columns.Add("UnitCost", Type.GetType("System.Double"))
        dtItem.Columns.Add("Remark", Type.GetType("System.String"))
        dtItem.Columns.Add("GSTRate", Type.GetType("System.String"))
        dtItem.Columns.Add("GSTTaxCode", Type.GetType("System.String"))
        dtItem.Columns.Add("GST", Type.GetType("System.Double"))
        dtItem.Columns.Add("CurrencyCode", Type.GetType("System.String"))

        Dim dtr As DataRow
        Dim dgItem As DataGridItem
        For Each dgItem In dtgCatalogueMod.Items
            dtr = dtItem.NewRow
            dtr("Index") = ViewState("index")
            dtr("ProductCode") = dgItem.Cells(9).Text

            Dim txtPrice As TextBox
            txtPrice = dgItem.FindControl("txt_price1")
            dtr("UnitCost") = txtPrice.Text

            Dim txtRemark As TextBox
            txtRemark = dgItem.FindControl("txt_remark1")
            dtr("Remark") = txtRemark.Text

            'dtr("GST") = IIf(CType(dgItem.FindControl("ddl_tax1"), DropDownList).SelectedItem.Text <> "N/A" And CType(dgItem.FindControl("ddl_tax1"), DropDownList).SelectedItem.Text <> "---Select---", CType(dgItem.FindControl("ddl_tax1"), DropDownList).SelectedItem.Text, 0)

            Dim ddl_GSTRate As DropDownList
            ddl_GSTRate = dgItem.FindControl("ddl_GSTRate1")

            'dtr("GST") = objDb.GetVal("SELECT CODE_VALUE FROM CODE_MSTR WHERE CODE_CATEGORY = 'GST' AND CODE_ABBR = '" & ddl_GSTRate.SelectedValue & "'")
            'dtr("GSTRate") = ddl_GSTRate.SelectedValue

            Dim ddl_GSTCode As DropDownList
            ddl_GSTCode = dgItem.FindControl("ddl_GSTCode1")
            'dtr("GSTTaxCode") = ddl_GSTCode.SelectedValue

            If ViewState("GSTCOD") = True Then
                dtr("GST") = 0
                dtr("GSTRate") = ddl_GSTRate.SelectedValue
                dtr("GSTTaxCode") = ddl_GSTCode.SelectedValue
            Else
                dtr("GST") = IIf(CType(dgItem.FindControl("ddl_tax1"), DropDownList).SelectedItem.Text <> "N/A" And CType(dgItem.FindControl("ddl_tax1"), DropDownList).SelectedItem.Text <> "---Select---", CType(dgItem.FindControl("ddl_tax1"), DropDownList).SelectedItem.Text, 0)
                dtr("GSTRate") = ""
                dtr("GSTTaxCode") = ""
            End If

            Dim ddlcurr As New DropDownList
            ddlcurr = dgItem.FindControl("ddl_curr1")
            dtr("CurrencyCode") = ddlcurr.SelectedItem.Value

            dtItem.Rows.Add(dtr)
        Next

        bindCatalogueItemMod = dtItem
    End Function

    Private Sub dtgCatalogueMod_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogueMod.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgCatalogueMod, e)
    End Sub

    Private Sub dtgCatalogueMod_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCatalogueMod.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim objrfq As New RFQ
            Dim objGlobal As New AppGlobals
            Dim lstItem As New ListItem
            Dim lstItem2 As New ListItem
            ' Start: Un-Comment by yAP: Why previously is commented out
            Dim ddl_tax As New DropDownList
            ddl_tax = e.Item.FindControl("ddl_tax1")
            objGlobal.FillTax(ddl_tax)
            ' End:
            'Common.SelDdl(CStr(CInt(dv("CDI_GST"))), ddl_tax, False)

            Dim ddl_curr As New DropDownList
            ddl_curr = e.Item.FindControl("ddl_curr1")
            objGlobal.FillCodeTable(ddl_curr, CodeTable.Currency)
            Common.SelDdl(dv("CDI_CURRENCY_CODE"), ddl_curr)

            Dim ddl_GSTRate1 As New DropDownList
            Dim ddl_GSTCode1 As New DropDownList
            Dim lbl_GSTRate1 As New Label
            ddl_GSTRate1 = e.Item.FindControl("ddl_GSTRate1")
            lbl_GSTRate1 = e.Item.FindControl("lbl_GSTRate1")
            ddl_GSTCode1 = e.Item.FindControl("ddl_GSTCode1")

            If ViewState("strGstRegNo") <> "" Then
                If IsDBNull(dv("CDI_GST_RATE")) Then
                    objGlobal.FillGST(ddl_GSTRate1)
                    lbl_GSTRate1.Text = ""

                    If lbl_GSTRate1.Text = "" Then
                        'Jules 2014.09.04 - Added this. Otherwise, if no GST Rate selected, Tax Code defaulted to first record.
                        objGlobal.FillTaxCode(ddl_GSTCode1, Common.parseNull(dv("CDI_GST_RATE")), "P", , True)
                    Else
                        objGlobal.FillTaxCode(ddl_GSTCode1, Common.parseNull(dv("CDI_GST_RATE")), "P", , False)
                    End If
                Else
                    objGlobal.FillGST(ddl_GSTRate1, False, ViewState("ContractVCoyId"))
                    Common.SelDdl(dv("CDI_GST_RATE"), ddl_GSTRate1)
                    lbl_GSTRate1.Text = dv("CDI_GST_RATE")
                    objGlobal.FillTaxCode(ddl_GSTCode1, Common.parseNull(dv("CDI_GST_RATE")), "P", , False)
                End If

                'objGlobal.FillTaxCode(ddl_GSTCode1, Common.parseNull(dv("CDI_GST_RATE")), "P", , False)
                If Not IsDBNull(dv("CDI_GST_TAX_CODE")) Then
                    Common.SelDdl(dv("CDI_GST_TAX_CODE"), ddl_GSTCode1)
                End If

                If Common.parseNull(dv("CDI_GST_RATE")) = "EX" Then
                    ddl_GSTCode1.Enabled = False
                Else
                    ddl_GSTCode1.Enabled = True
                End If
            Else
                ddl_GSTRate1.Items.Clear()
                ddl_GSTCode1.Items.Clear()
                lstItem.Value = "N/A"
                lstItem.Text = "N/A"
                ddl_GSTRate1.Items.Insert(0, lstItem)
                lstItem2.Value = "NR"
                lstItem2.Text = "NR"
                ddl_GSTCode1.Items.Insert(0, lstItem2)
                lbl_GSTRate1.Text = "N/A"
                ddl_GSTRate1.Enabled = False
                ddl_GSTCode1.Enabled = False
            End If

            Dim rev_price As RegularExpressionValidator
            rev_price = e.Item.FindControl("rev_price1")
            rev_price.ControlToValidate = "txt_price1"
            rev_price.ValidationExpression = "^\d{1,10}(\.\d{1,4})?$"
            rev_price.ErrorMessage = "Contract Price is expecting numeric value."
            rev_price.Text = "?"
            rev_price.Display = ValidatorDisplay.Dynamic

            Dim txtRemark As TextBox
            txtRemark = e.Item.FindControl("txt_remark1")
            txtRemark.Text = dv("CDI_REMARK")

            Dim txtPrice As TextBox
            txtPrice = e.Item.FindControl("txt_price1")
            If Not IsDBNull(dv("CDI_UNIT_COST")) Then
                'Stage 3 Bug fix (GST-0022) - 06/07/2015 - CH
                'txtPrice.Text = Format(CDbl(dv("CDI_UNIT_COST")), "###,###,##0.0000")
                txtPrice.Text = Format(CDbl(dv("CDI_UNIT_COST")), "###0.0000")
            Else
                txtPrice.Text = "0.0000"
            End If
        End If
    End Sub

    Private Sub dtgCatalogueMod_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCatalogueMod.PageIndexChanged
        dtgCatalogueMod.CurrentPageIndex = e.NewPageIndex
        BindgridMod()
    End Sub

    Private Function validateDatagrid1(ByRef strMsg As String) As Boolean
        validateDatagrid1 = True
        Dim dgItem As DataGridItem
        Dim ddl_GSTRate1 As DropDownList
        Dim txtRemark As TextBox
        Dim txtQ As TextBox
        Dim txtGSTQ1 As TextBox

        strMsg = "<ul type='disc'>"
        For Each dgItem In dtgCatalogueMod.Items
            ddl_GSTRate1 = dgItem.FindControl("ddl_GSTRate1")
            txtRemark = dgItem.FindControl("txt_remark1")
            txtQ = dgItem.FindControl("txtQ1")
            txtGSTQ1 = dgItem.FindControl("txtGSTQ1")

            If ViewState("GSTCOD") = True Then
                If ddl_GSTRate1.SelectedValue = "" Then
                    strMsg &= "<li>GST Rate is required.<ul type='disc'></ul></li>"
                    txtGSTQ1.Text = "?"
                    validateDatagrid1 = False
                Else
                    txtGSTQ1.Text = ""
                End If
            End If
            
            If Not Common.checkMaxLength(txtRemark.Text, 400) Then
                strMsg &= "<li>Remarks is over limit.<ul type='disc'></ul></li>"
                txtQ.Text = "?"
                validateDatagrid1 = False
            Else
                txtQ.Text = ""
            End If
        Next
        strMsg &= "</ul>"
    End Function

    Private Function validateContractPrice1() As Boolean
        validateContractPrice1 = True
        Dim dgItem As DataGridItem
        Dim i As Integer

        For Each dgItem In dtgCatalogueMod.Items
            If CType(dgItem.FindControl("txt_price1"), TextBox).Text = "" Then
                i = i + 1
            End If
        Next

        If i > 0 Then
            validateContractPrice1 = False
            Common.NetMsgbox(Me, "Please enter Contract Price for all the item(s).", MsgBoxStyle.Information)
            Exit Function
        End If

    End Function

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

    Private Sub fillTaxCode()
        Dim dgItem As DataGridItem
        Dim objGlobal As New AppGlobals
        Dim i As Integer

        If ViewState("mode") = "add" Then
            For Each dgItem In dtgCatalogue.Items
                If CType(dgItem.FindControl("ddl_GSTRate"), DropDownList).SelectedValue <> CType(dgItem.FindControl("lbl_GSTRate"), Label).Text Then
                    objGlobal.FillTaxCode(CType(dgItem.FindControl("ddl_GSTCode"), DropDownList), CType(dgItem.FindControl("ddl_GSTRate"), DropDownList).SelectedValue, "P", , False)
                    CType(dgItem.FindControl("lbl_GSTRate"), Label).Text = CType(dgItem.FindControl("ddl_GSTRate"), DropDownList).SelectedValue

                    If CType(dgItem.FindControl("ddl_GSTCode"), DropDownList).SelectedValue = "N/A" Then
                        CType(dgItem.FindControl("ddl_GSTCode"), DropDownList).Enabled = False
                    Else
                        CType(dgItem.FindControl("ddl_GSTCode"), DropDownList).Enabled = True
                    End If
                End If
            Next
        Else
            For Each dgItem In dtgCatalogueMod.Items
                If CType(dgItem.FindControl("ddl_GSTRate1"), DropDownList).SelectedValue <> CType(dgItem.FindControl("lbl_GSTRate1"), Label).Text Then
                    objGlobal.FillTaxCode(CType(dgItem.FindControl("ddl_GSTCode1"), DropDownList), CType(dgItem.FindControl("ddl_GSTRate1"), DropDownList).SelectedValue, "P", , False)
                    CType(dgItem.FindControl("lbl_GSTRate1"), Label).Text = CType(dgItem.FindControl("ddl_GSTRate1"), DropDownList).SelectedValue

                    If CType(dgItem.FindControl("ddl_GSTCode1"), DropDownList).SelectedValue = "N/A" Then
                        CType(dgItem.FindControl("ddl_GSTCode1"), DropDownList).Enabled = False
                    Else
                        CType(dgItem.FindControl("ddl_GSTCode1"), DropDownList).Enabled = True
                    End If
                End If
            Next
        End If
    End Sub
End Class
