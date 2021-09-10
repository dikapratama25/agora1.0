Imports AgoraLegacy
Imports eProcure.Component

Imports System.Drawing
Public Class Add_Catalogue_Item_SEH

    Inherits AgoraLegacy.AppBaseClass
    'Protected WithEvents lbl_cur As System.Web.UI.WebControls.Label
    Protected WithEvents txt_vendor_com As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_desc As System.Web.UI.WebControls.TextBox
    Protected WithEvents dtg_Cat As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtg_freeform As System.Web.UI.WebControls.DataGrid
    Protected WithEvents onchange As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents cmd_search As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear As System.Web.UI.WebControls.Button
    'Protected WithEvents lbl_RFQ_Name As System.Web.UI.WebControls.Label
    Protected WithEvents cmd_Save As System.Web.UI.WebControls.Button
    Dim rfq_name As String
    Dim back As String
    'Protected WithEvents lbl_rfq_number As System.Web.UI.WebControls.Label
    Protected WithEvents cmd_back As System.Web.UI.WebControls.Button
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lbl_check As System.Web.UI.WebControls.Label
    Protected WithEvents hidStk As System.Web.UI.WebControls.Label
    Protected WithEvents ddl_vendor_com As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txt_item_desc As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cboCommodityType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents chkVCI As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkPCI As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkFreeform As System.Web.UI.WebControls.CheckBox
    Protected WithEvents dt_rfq As System.Web.UI.WebControls.Table
    Protected WithEvents cmd_freeformClear As System.Web.UI.WebControls.Button
    Protected WithEvents hidCommodity As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtCommodity As System.Web.UI.WebControls.TextBox
    Dim a As New AppGlobals
    Dim total As Integer
    Dim check_qty As Integer
    Dim objGlobal As New AppGlobals
    Dim dta As Table
    Dim strMsg As String = ""
    Dim strQtyErr As String
    'Dim row As New DataRow

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Enum CartEnum
        Chk = 0
        Desc = 1
        UOM = 2
        Cost = 3
        Coy_Name = 4
        Qty = 5
        Time = 6
        'Warranty = 7
        Pro_code = 7
        Coy_ID = 8
        VIC = 9
        Stk_type = 10
        Prod_For = 11
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmd_Save.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmd_Save)
        htPageAccess.Add("add", alButtonList)

        If ViewState("intPageRecordCnt") > 0 Then
            cmd_Save.Enabled = blnCanAdd
            cmd_Save.Enabled = blnCanUpdate
        Else
            cmd_Save.Enabled = False
        End If

        '  cmd_createInv.Enabled = blnCanAdd
        CheckButtonAccess()
        alButtonList.Clear()
        'If chkFreeform.Checked Then chkFreeform_CheckedChanged(chkFreeform, New System.EventArgs())

    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strNewCSS = "both"
        MyBase.Page_Load(sender, e)
        blnPaging = True
        SetGridProperty(dtg_Cat)
        blnPaging = False
        SetGridProperty(dtg_freeform)

        If Not Page.IsPostBack Then
            'Michelle (4/2/2013) - Issue 1846
            strQtyErr = objGlobal.GetErrorMessage("00342")
            ViewState("ValQtyMsg") = strQtyErr
            'objGlobal.FillCommodityType(Me.cboCommodityType)
            Me.cmd_back.Attributes.Add("onclick", "window.close();")

            Dim objpro As New Products
            Dim objrfq As New RFQ
            Dim ary(100) As String

            Dim user_type As Boolean
            Dim ds2 As New DataSet
            user_type = objrfq.chkPurchasingRole()
            hidStk.Text = ""
            If user_type = False Then
                Dim objAdmin As New Admin
                ds2 = objAdmin.searchvendor("AV", "", "")
                objAdmin = Nothing
                Common.FillDdl(ddl_vendor_com, "CM_COY_NAME", "CV_S_COY_ID", ds2.Tables(0).DefaultView)

                Dim ls As New ListItem
                ls.Text = "---Select---"
                ddl_vendor_com.Items.Insert(0, ls)
                Me.txt_vendor_com.Visible = False
            Else
                Me.ddl_vendor_com.Visible = False
            End If

            

            'Bindgrid(0)
            'AddRow(10)
            Me.cmd_Save.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")

            'If Session("rfq_id") = "" Then
            '    Session("rfq_id") = Request(Trim("RFQ_ID"))
            'End If
            'If Session("rfq_name") = "" Then
            '    Session("rfq_name") = Me.Request.QueryString("RFQ_Name")
            'End If


            'ViewState("rfq_cur_value") = Me.Request.QueryString("RFQ_Cur_value")
            'Dim rfq_cur_text As String
            'rfq_cur_text = ViewState("rfq_cur_value")
            'Me.lbl_cur.Text = rfq_cur_text
            'Me.lbl_RFQ_Name.Text = ViewState("rfq_name")
            cmd_Save.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','Save');")
            'Michelle
            'objGLO.GetErrorMessage("00020")
        End If
        'chkFreeform.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','Save');")
        lbl_check.Text = ""
        Me.chkPCI.Attributes.Add("OnClick", "UncheckFreeForm();")
        Me.chkFreeform.Attributes.Add("OnClick", "ShowFreeForm();")
        Me.cmd_clear.Attributes.Add("onClick", "ClearAll();")
        'Me.cmd_search.Attributes.Add("onClick", "ClearAll();")
        Me.cmd_freeformClear.Attributes.Add("onClick", "ClearAll(""freeform"");")

        If chkFreeform.Checked = True Then
            dtg_Cat.DataSource = Nothing
            dtg_Cat.DataBind()
            Dim strscript As New System.Text.StringBuilder
            strscript.Append("<script language=""javascript"">")
            strscript.Append("ShowFreeForm();")
            strscript.Append("</script>")
            RegisterStartupScript("script14", strscript.ToString())
        End If

        If chkVCI.Checked = False And chkPCI.Checked = True Then
            dtg_Cat.Columns(4).Visible = False
        Else
            dtg_Cat.Columns(4).Visible = True
        End If

        'lbl_cur.Text = ""
        'lbl_RFQ_Name.Text = ""

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objpro As New Products_Ext
        Dim ds As New DataSet
        Dim ary(0) As String
        Dim objrfq As New RFQ
        Dim user_type As Boolean
        Dim objRfq2 As New RFQ_User
        Dim strCurrency, strFor As String
        Dim strCommodity As String = ""
        Dim aryProdCode As New ArrayList, i As Integer
        Dim strProdList As String = ""

        If Me.txtCommodity.Text <> "" Then
            strCommodity = Me.hidCommodity.Value
        End If

        user_type = objrfq.chkPurchasingRole()
        If user_type = False Then
            If ddl_vendor_com.SelectedItem.Text <> "---Select---" Then
                ary(0) = Me.ddl_vendor_com.SelectedItem.Value
            End If
        Else
            ary(0) = Me.txt_vendor_com.Text
        End If

        'objrfq.read_rfqMstr(objRfq2, Session("rfq_name"), Session("rfq_id"), Session("rfq_num"))
        'strCurrency = objRfq2.cur_code
        If chkVCI.Checked And chkPCI.Checked Then
            strFor = "VB"
        ElseIf chkVCI.Checked Then
            strFor = "V"
        ElseIf chkPCI.Checked Then
            strFor = "B"
        End If

        'Get ProductCode from Session array 
        aryProdCode = Session("RFQProdList")
        For i = 0 To aryProdCode.Count - 1
            If strProdList = "" Then
                strProdList = "'" & aryProdCode(i)(0) & "'"
            Else
                strProdList &= ", '" & aryProdCode(i)(0) & "'"
            End If
        Next
        If strProdList = "" Then strProdList = "''"

        ds = objpro.search_Catalogue(strFor, "VE", ary, txt_item_desc.Text, strCommodity, , strProdList)

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView

        dvViewSample.RowFilter = "PM_PRODUCT_DESC LIKE '%" + Me.txt_item_desc.Text + "%'"

        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        If ViewState("action") = "del" Then
            If dtg_Cat.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtg_Cat.PageSize = 0 Then
                dtg_Cat.CurrentPageIndex = dtg_Cat.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        intPageRecordCnt = dvViewSample.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        If intPageRecordCnt > 0 Then
            If Not Page.IsPostBack Then
                dvViewSample.Sort = "PM_PRODUCT_DESC"
            End If

            resetDatagridPageIndex(dtg_Cat, dvViewSample)

            dtg_Cat.DataSource = dvViewSample
            dtg_Cat.DataBind()
        Else
            dtg_Cat.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtg_Cat.PageCount
    End Function

    Public Sub dtg_cat_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        Bindgrid()

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        sender.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtg_Cat_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_Cat.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(CartEnum.Chk).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim txt_qty As TextBox
            Dim rvl_qty As RegularExpressionValidator
            rvl_qty = e.Item.FindControl("rvl_qty")
            txt_qty = e.Item.FindControl("txt_qty")
            txt_qty.Text = ""
            rvl_qty.ControlToValidate = "txt_qty"
            rvl_qty.ValidationExpression = "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$" '"^([1-9]\d{0,5}\.\d{0,2}|[1-9]\d{0,5}|[0]\d{0,5}\.[1-9]\d{0,1})$" '"^(\d|,)*\.?\d{0,2}$" '"(?!^0*$)^\d{1,5}?$" '"(?!^0*$)^\d{1,14}?$"
            'rvl_qty.ErrorMessage = "Invalid quantity. Range should be from 0.1 to 999999.99"
            rvl_qty.ErrorMessage = ViewState("ValQtyMsg")
            rvl_qty.Text = "?"
            rvl_qty.Display = ValidatorDisplay.Dynamic
            Dim txt_delivery As TextBox
            txt_delivery = e.Item.FindControl("txt_delivery")
            txt_delivery.Text = "0"

            Dim rvl_delivery_time As RegularExpressionValidator
            rvl_delivery_time = e.Item.FindControl("rvl_delivery_time")
            rvl_delivery_time.ControlToValidate = "txt_delivery"
            rvl_delivery_time.ValidationExpression = "^\d+$"
            rvl_delivery_time.ErrorMessage = "Delivery Lead Time is expecting numeric value."
            rvl_delivery_time.Text = "?"
            rvl_delivery_time.Display = ValidatorDisplay.Dynamic

            If IsDBNull(dv("PM_UNIT_COST")) Then
                e.Item.Cells(CartEnum.Cost).Text = "N/A"
            Else
                e.Item.Cells(CartEnum.Cost).Text = Common.parseNull(dv("PM_CURRENCY_CODE")) & " " & Format(dv("PM_UNIT_COST"), "#,##0.00")
            End If

            Dim ddlstock As DropDownList = e.Item.Cells(CartEnum.Stk_type).FindControl("ddl_stock_2")
            Dim lblstock As Label = e.Item.Cells(CartEnum.Stk_type).FindControl("lbl_stock")

            'ddlstock = dtg_Cat.FindControl("ddl_stock_2")
            'lblstock = dtg_Cat.FindControl("lbl_stock")

            If dv("PM_PRODUCT_FOR") = "B" Then
                ddlstock.Visible = False
                lblstock.Visible = True

                If dv("PM_ITEM_TYPE") = "SP" Then
                    lblstock.Text = "Spot"
                ElseIf dv("PM_ITEM_TYPE") = "ST" Then
                    lblstock.Text = "Stock"
                Else
                    lblstock.Text = "MRO"
                End If
            Else
                LoadStkDdl(ddlstock)
                ddlstock.Visible = True
                lblstock.Visible = False
            End If

        End If
    End Sub

    Private Sub dtg_Cat_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_Cat.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtg_Cat, e)

        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub LoadStkDdl(ByRef ddl As DropDownList)
        Dim ds As New DataSet
        Dim objDb As New EAD.DBCom

        ds = objDb.FillDs("SELECT UM_STK_TYPE_SPOT, UM_STK_TYPE_STOCK, UM_STK_TYPE_MRO FROM user_mstr WHERE um_user_id = '" & Session("UserId") & "' AND um_coy_id = '" & Session("CompanyId") & "'")
        'ddl.Items.Add(New ListItem("---Select---", ""))
        If ds.Tables(0).Rows(0)("UM_STK_TYPE_SPOT") = "Y" Then
            ddl.Items.Add(New ListItem("Spot", "SP"))
        End If

        If ds.Tables(0).Rows(0)("UM_STK_TYPE_STOCK") = "Y" Then
            ddl.Items.Add(New ListItem("Stock", "ST"))
        End If

        If ds.Tables(0).Rows(0)("UM_STK_TYPE_MRO") = "Y" Then
            ddl.Items.Add(New ListItem("MRO", "MI"))
        End If

        'ddl.Items.Insert(0, "---Select---")

    End Sub

    Private Sub cmd_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_search.Click
        dtg_Cat.CurrentPageIndex = 0
        Bindgrid(0)
    End Sub
    Private Sub cmd_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_clear.Click
        Me.txt_vendor_com.Text = ""
        txt_item_desc.Text = ""
        'If Not Me.ddl_vendor_com Is Nothing Then Me.ddl_vendor_com.SelectedIndex = "0"
        txtCommodity.Text = ""
        chkVCI.Checked = True
        chkPCI.Checked = False
        chkFreeform.Checked = False
    End Sub

    Private Sub cmd_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Save.Click
        Dim b As Integer

        For b = 0 To dtg_freeform.Items.Count() - 1
            Dim txt_delivery As TextBox
            Dim txt_qty As TextBox
            Dim txt_desc As TextBox

            txt_delivery = dtg_freeform.Items(b).Cells(3).FindControl("txt_delivery")
            txt_qty = dtg_freeform.Items(b).Cells(2).FindControl("txt_qty")
            txt_desc = dtg_freeform.Items(b).Cells(0).FindControl("txt_desc")

            If (txt_desc.Text = "" And txt_qty.Text <> "") Or (txt_desc.Text = "" And txt_delivery.Text <> "") Then
                Dim reqDesc As RequiredFieldValidator
                reqDesc = dtg_freeform.Items(b).Cells(0).FindControl("reqDesc")
                reqDesc.ControlToValidate = "txt_desc"
                reqDesc.ErrorMessage = "Require Description"
                reqDesc.Text = "?"
                reqDesc.Display = ValidatorDisplay.Dynamic
                reqDesc.Enabled = True
                reqDesc.Visible = True
            Else
                Dim reqDesc As RequiredFieldValidator
                reqDesc = dtg_freeform.Items(b).Cells(0).FindControl("reqDesc")
                reqDesc.ControlToValidate = "txt_desc"
                reqDesc.Display = ValidatorDisplay.None
                reqDesc.Enabled = False
                reqDesc.Visible = False
            End If
        Next

        If Not chkFreeform.Checked = True Then
            If chkOverseaItem() = 1 Then
                Common.NetMsgbox(Me, "Selected items cannot be a mixture of oversea and local items.")
                Exit Sub
            End If
        End If

        If Page.IsValid And validateInputs() Then

            Dim aryProdCode As ArrayList = Session("RFQProdList")
            Dim dgItem, dgItemFF As DataGridItem
            Dim objrfq As New RFQ_Ext
            Dim objval As New RFQ_User
            Dim objDB As New EAD.DBCom
            Dim strAryQuery(0) As String
            Dim strSQL, strStk, strStk2 As String
            Dim chkItem As CheckBox
            rfq_name = ViewState("rfq_name")
            Dim rfq_id As String = Session("rfq_id")
            'Dim cur As String = Request.QueryString("RFQ_Cur_value")
            Dim i As Integer = 0
            Dim DS As New DataSet
            Dim dtr As DataRow
            Dim catItem As New DataTable
            catItem.Columns.Add("product_name", Type.GetType("System.String"))
            catItem.Columns.Add("RFQ_Name", Type.GetType("System.String"))
            catItem.Columns.Add("RFQ_ID", Type.GetType("System.String"))
            'catItem.Columns.Add("cur_code", Type.GetType("System.String"))
            catItem.Columns.Add("Header_Ind", Type.GetType("System.String"))
            catItem.Columns.Add("item_desc", Type.GetType("System.String"))
            catItem.Columns.Add("uom", Type.GetType("System.String"))
            catItem.Columns.Add("vendor_Id", Type.GetType("System.String"))
            catItem.Columns.Add("product_ID", Type.GetType("System.String"))
            catItem.Columns.Add("VIC", Type.GetType("System.String"))
            catItem.Columns.Add("Quantity", Type.GetType("System.Decimal")) 'catItem.Columns.Add("Quantity", Type.GetType("System.String"))
            catItem.Columns.Add("Delivery_Lead_Time", Type.GetType("System.String"))
            catItem.Columns.Add("stock_type", Type.GetType("System.String"))

            strStk = ""
            strStk2 = ""

            If hidStk.Text <> "" Then
                strStk = hidStk.Text
            End If

            If chkFreeform.Checked Then
                For Each dgItemFF In dtg_freeform.Items
                    Dim txt_desc, txt_qty, txt_delivery As TextBox
                    Dim ddlUom As DropDownList
                    Dim ddlStock As DropDownList
                    txt_desc = dgItemFF.FindControl("txt_desc")
                    txt_qty = dgItemFF.FindControl("txt_qty")
                    txt_delivery = dgItemFF.FindControl("txt_delivery")
                    ddlUom = dgItemFF.FindControl("ddl_uom")
                    ddlStock = dgItemFF.FindControl("ddl_stock")
                    If txt_desc.Text <> "" Then
                        dtr = catItem.NewRow()
                        dtr("product_name") = ""
                        dtr("RFQ_Name") = rfq_name
                        dtr("RFQ_ID") = rfq_id
                        dtr("item_desc") = txt_desc.Text
                        dtr("vendor_Id") = ""
                        dtr("product_ID") = ""
                        dtr("VIC") = ""
                        dtr("uom") = ddlUom.SelectedItem.Text()
                        dtr("Quantity") = IIf(txt_qty.Text = "", 0, txt_qty.Text)
                        dtr("Delivery_Lead_Time") = IIf(txt_delivery.Text = "", 0, txt_delivery.Text)
                        dtr("stock_type") = ddlStock.SelectedItem.Value()
                        If strStk = "" Then
                            strStk = ddlStock.SelectedItem.Value()
                        Else
                            strStk2 = ddlStock.SelectedItem.Value()
                        End If
                        catItem.Rows.Add(dtr)
                    End If

                    If strStk <> "" And strStk2 <> "" Then
                        If strStk <> strStk2 Then
                            Common.NetMsgbox(Me, "Selected items are not from the same item type.")
                            Exit Sub
                        End If
                    End If
                Next
                'chkFreeform.Checked = False
                'chkVCI.Checked = True
            Else
                For Each dgItem In dtg_Cat.Items

                    chkItem = dgItem.FindControl("chkSelection")
                    Dim ddlStock2 As DropDownList
                    Dim lbl_alert As Label
                    lbl_alert = dgItem.FindControl("lbl_alert")
                    ddlStock2 = dgItem.FindControl("ddl_stock_2")
                    If chkItem.Checked Then
                        Me.cmd_Save.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','Save');")
                        dtr = catItem.NewRow()
                        dtr("product_name") = dtg_Cat.DataKeys.Item(i)
                        dtr("RFQ_Name") = rfq_name
                        dtr("RFQ_ID") = rfq_id
                        'dtr("cur_code") = objrfq.count_item2(Me.lbl_RFQ_Name.Text) + 1
                        dtr("Header_Ind") = 0
                        dtr("item_desc") = dgItem.Cells(CartEnum.Desc).Text
                        dtr("uom") = dgItem.Cells(CartEnum.UOM).Text
                        dtr("vendor_Id") = dgItem.Cells(CartEnum.Coy_ID).Text
                        dtr("product_ID") = dgItem.Cells(CartEnum.Pro_code).Text
                        dtr("VIC") = dgItem.Cells(CartEnum.VIC).Text

                        'If CType(dgItem.FindControl("txt_qty"), TextBox).Text = "" Then
                        '    lbl_alert.Text = "*"
                        '    lbl_alert.Visible = True
                        '    lbl_alert.ForeColor = Color.Red
                        '    check_qty = 1
                        '    lbl_check.Text = "Quantity is required."
                        '    lbl_check.Visible = True
                        '    lbl_check.ForeColor = Color.Red
                        'End If

                        dtr("Quantity") = IIf(CType(dgItem.FindControl("txt_qty"), TextBox).Text = "", 0, CType(dgItem.FindControl("txt_qty"), TextBox).Text)
                        dtr("Delivery_Lead_Time") = CType(dgItem.FindControl("txt_delivery"), TextBox).Text
                        If dgItem.Cells(CartEnum.Prod_For).Text = "V" Then
                            dtr("stock_type") = ddlStock2.SelectedItem.Value()

                            If strStk = "" Then
                                strStk = ddlStock2.SelectedItem.Value()
                            Else
                                strStk2 = ddlStock2.SelectedItem.Value()
                            End If
                        Else
                            If CType(dgItem.FindControl("lbl_stock"), Label).Text = "Spot" Then
                                dtr("stock_type") = "SP"
                                If strStk = "" Then
                                    strStk = "SP"
                                Else
                                    strStk2 = "SP"
                                End If
                            ElseIf CType(dgItem.FindControl("lbl_stock"), Label).Text = "Stock" Then
                                dtr("stock_type") = "ST"
                                If strStk = "" Then
                                    strStk = "ST"
                                Else
                                    strStk2 = "ST"
                                End If
                            Else
                                dtr("stock_type") = "MI"
                                If strStk = "" Then
                                    strStk = "MI"
                                Else
                                    strStk2 = "MI"
                                End If
                            End If
                        End If

                        If strStk <> "" And strStk2 <> "" Then
                            If strStk <> strStk2 Then
                                Common.NetMsgbox(Me, "Selected items are not from the same item type.")
                                Exit Sub
                            End If
                        End If

                        catItem.Rows.Add(dtr)

                        aryProdCode.Add(dgItem.Cells(CartEnum.Pro_code).Text)
                    Else
                        lbl_alert.Text = ""
                        lbl_alert.Visible = False
                    End If
                    i = i + 1
                Next

            End If
            If catItem.Rows.Count() > 0 Then
                'If check_qty = 1 Then

                '    Exit Sub
                'End If
                DS.Tables.Add(catItem)
                strSQL = objrfq.add_RFQCat_TEMP(DS)
                lbl_check.Text = ""
                hidStk.Text = strStk
                Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
                If chkFreeform.Checked Then
                    'AddRow(10)
                    chkFreeform_CheckedChanged(chkFreeform, New System.EventArgs())
                Else
                    Session("RFQProdList") = aryProdCode
                    cmd_search_Click(cmd_search, New System.EventArgs())
                End If

            End If
            'Bindgrid(True)
            'Me.onchange.Value = "0"
        Else
            If strMsg <> "" Then
                lbl_check.Text = strMsg
            Else
                lbl_check.Text = ""
            End If
        End If

    End Sub

    Private Function validateInputs() As Boolean
        Dim b As Integer
        Dim check As Boolean = True
        For b = 0 To dtg_freeform.Items.Count() - 1
            Dim txt_delivery As TextBox
            Dim txt_qty As TextBox
            Dim txt_desc As TextBox

            txt_delivery = dtg_freeform.Items(b).Cells(3).FindControl("txt_delivery")
            txt_qty = dtg_freeform.Items(b).Cells(2).FindControl("txt_qty")
            txt_desc = dtg_freeform.Items(b).Cells(0).FindControl("txt_desc")

            If txt_desc.Text <> "" And txt_desc.Text.Length > 500 Then
                strMsg = "<ul type='disc'><li>Item " & Val(b) + 1 & " Description should be less than 500 characters.</li></ul>"
                check = False
            End If
        Next

        If check = False Then
            Return False
        Else
            Return True
        End If

    End Function

    Private Function chkOverseaItem() As Integer
        Dim objDb As New EAD.DBCom
        Dim objrfq_ext As New RFQ_Ext
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox
        Dim strOversea, strOversea2 As String
        Dim i As Integer = 0
        Dim intMsg As Integer = 0

        strOversea = objrfq_ext.GetOverseaofItem()

        For Each dgItem In dtg_Cat.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                If i = 0 Then
                    If strOversea <> "" Then
                        strOversea2 = objDb.GetVal("SELECT PM_OVERSEA FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE='" & dgItem.Cells(CartEnum.Pro_code).Text & "'")
                        If strOversea <> strOversea2 Then
                            intMsg = 1
                            Exit For
                        End If
                    Else
                        strOversea = objDb.GetVal("SELECT PM_OVERSEA FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE='" & dgItem.Cells(CartEnum.Pro_code).Text & "'")
                    End If
                Else
                    strOversea2 = objDb.GetVal("SELECT PM_OVERSEA FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE='" & dgItem.Cells(CartEnum.Pro_code).Text & "'")
                    If strOversea <> strOversea2 Then
                        intMsg = 1
                        Exit For
                    End If
                End If

                i = i + 1
            End If
        Next

        Return intMsg
    End Function

    Sub bind_table(ByVal row As Integer)
        Dim dr As TableRow
        Dim i, j As Integer

        For i = 0 To row - 1 'add  row
            dr = New TableRow

            For j = 0 To 4
                dr.Cells.Add(New TableCell)
            Next

            If i Mod 2 = 0 Then
                dr.BackColor = Color.FromName("#fdfdfd")
            Else
                dr.BackColor = Color.FromName("#f6f9fe")
            End If

            dr.HorizontalAlign = HorizontalAlign.Center
            Dim dv_uom As DataView

            Dim txt_item As New TextBox ' item des
            Dim ddl_uom As New DropDownList
            Dim txt_qty As New TextBox 'quantity
            Dim day As New TextBox 'Delivery Lead Time 
            Dim rvl_qty As New RegularExpressionValidator
            Dim rvl_delivery_time As New RegularExpressionValidator

            txt_item.Width = System.Web.UI.WebControls.Unit.Pixel(300)
            txt_item.TextMode = TextBoxMode.MultiLine
            txt_item.ID = "txt_item" & i
            txt_item.Rows = 2

            txt_item.MaxLength = 500
            txt_qty.MaxLength = 5
            txt_qty.CssClass = "numerictxtbox"
            day.MaxLength = 3
            day.CssClass = "numerictxtbox"

            dr.Cells(0).Controls.Add(txt_item)

            dr.Cells(0).HorizontalAlign = HorizontalAlign.Left
            dr.Cells(1).HorizontalAlign = HorizontalAlign.Left
            dr.Cells(2).HorizontalAlign = HorizontalAlign.Right
            dr.Cells(3).HorizontalAlign = HorizontalAlign.Right
            Dim txtQ As New TextBox
            txtQ.CssClass = "lblnumerictxtbox"
            txtQ.ID = "txtQ" & i
            dr.Cells(0).Controls.Add(txtQ)
            txtQ.Visible = False
            txtQ.Attributes.Add("onKeyDown", "limitText (this, 400);")
            txtQ.ForeColor = Color.Red
            ddl_uom.Width = System.Web.UI.WebControls.Unit.Pixel(150)
            dv_uom = a.GetCodeTableView(CodeTable.Uom)
            Common.FillDdl(ddl_uom, "CODE_DESC", "CODE_ABBR", dv_uom)
            dr.Cells(1).Width = Unit.Pixel(150)
            dr.Cells(1).Controls.Add(ddl_uom)
            txt_qty.ID = "txtqty" & i
            dr.Cells(2).Controls.Add(txt_qty)

            rvl_qty.ControlToValidate = "txtqty" & i
            rvl_qty.ValidationExpression = "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$" '"^([1-9]\d{0,5}\.\d{0,2}|[1-9]\d{0,5}|[0]\d{0,5}\.[1-9]\d{0,1})$" '"(?!^0*$)^\d{1,14}?$"
            'rvl_qty.ErrorMessage = "Line item No." & i + 1 & " - Invalid Quantity. Range should be from 0.1 to 999999.99"
            rvl_qty.ErrorMessage = "Line item No." & i + 1 & " - " & ViewState("ValQtyMsg")
            rvl_qty.Text = "?"
            rvl_qty.Display = ValidatorDisplay.Dynamic
            rvl_qty.ID = "rvl_qty" & i
            dr.Cells(2).Controls.Add(rvl_qty)

            day.Text = "0"
            day.ID = "day" & i
            dr.Cells(3).Controls.Add(day)
            rvl_delivery_time.ControlToValidate = "day" & i
            rvl_delivery_time.ValidationExpression = "^\d+$"
            rvl_delivery_time.ErrorMessage = "Line item No." & i + 1 & " - Delivery Lead Time(days) is expecting numeric value."
            rvl_delivery_time.Text = "?"
            rvl_delivery_time.Display = ValidatorDisplay.Dynamic
            rvl_delivery_time.ID = "rvl_delivery_time" & i
            dr.Cells(3).Controls.Add(rvl_delivery_time)

            CType(dr.Cells(0).Controls(0), TextBox).CssClass = "listtxtbox"
            For j = 1 To 3
                Select Case j
                    Case 1
                        CType(dr.Cells(j).Controls(0), DropDownList).Width = System.Web.UI.WebControls.Unit.Percentage(80)
                        CType(dr.Cells(j).Controls(0), DropDownList).CssClass = "ddl"
                    Case 2
                        CType(dr.Cells(j).Controls(0), TextBox).Width = System.Web.UI.WebControls.Unit.Pixel(50)
                        CType(dr.Cells(j).Controls(0), TextBox).CssClass = "numerictxtbox"

                    Case 3
                        CType(dr.Cells(j).Controls(0), TextBox).Width = System.Web.UI.WebControls.Unit.Pixel(40)
                        CType(dr.Cells(j).Controls(0), TextBox).CssClass = "numerictxtbox"

                End Select
            Next

            dt_rfq.Rows.Add(dr)
        Next

        total = dt_rfq.Rows.Count - 1
        ViewState("total") = total
        dta = dt_rfq
    End Sub
    Private Sub chkFreeform_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFreeform.CheckedChanged
        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        strscript.Append("ShowFreeForm();")
        strscript.Append("</script>")
        RegisterStartupScript("script113", strscript.ToString())
        If chkFreeform.Checked Then
            cmd_search.Visible = False
            cmd_clear.Visible = False
            cmd_freeformClear.Visible = True
            AddRow(10)
        Else
            cmd_search.Visible = True
            cmd_clear.Visible = True
            cmd_freeformClear.Visible = False
        End If
        'bind_table(10)
        'AddRow(10, 4)

    End Sub
    Sub AddRow(ByVal intRow As Integer)
        'Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim dt As New DataTable
        Dim row As DataRow
        Dim i, y As Integer
        dt.Columns.Add("txt_desc", GetType(String))
        For i = 1 To intRow
            'dt.Columns.Add(New DataColumn("Desc", GetType(System.String)))
            row = dt.NewRow()
            'row.Item("txt_desc") = "abc"
            dt.Rows.Add(row)
            'row = dt.NewRow()
            'row.Item("txt_desc") = "ccc"
            'dt.Rows.Add(row)
            'For y = 1 To intCell
            '    addCell(row)
            'Next
            Dim txtTotal As New TextBox
            'txtTotal.ID = "Total"
            'txtTotal.Text = Format(30, "#,##0.00")
            'txtTotal.CssClass = "lblnumerictxtbox"
            'txtTotal.ReadOnly = True
            'txtTotal.Width = System.Web.UI.WebControls.Unit.Pixel(120)
            'txtTotal.Font.Bold = True
            'row.Cells(2).Controls.Add(txtTotal)
            'row.Cells(1).Text = "abc"
            'row.Cells(2).HorizontalAlign = HorizontalAlign.Right
            'row.Cells(1).Font.Bold = True
            'dtg_freeform.Controls(0).Controls.Add(row)
        Next
        dtg_freeform.DataSource = dt
        dtg_freeform.DataBind()

    End Sub
    'End Sub
    'Sub AddRow(ByVal intCell As Integer, ByVal strLabel As String, ByVal dblTotal As Double, ByVal blnShowGST As Boolean)
    '    'adding totals row
    '    Dim intL, intColToRemain As Integer
    '    Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
    '    Dim intTotalCol As Integer

    '    intTotalCol = EnumShoppingCart.icRemark + intCnt - 1

    '    For intL = 0 To intTotalCol
    '        addCell(row)
    '    Next

    '    If ViewState("GST") = "product" And blnShowGST Then
    '        intColToRemain = 6 '//col bf label, label, total,gst,col after total
    '    Else
    '        intColToRemain = 5 '//col bf label, label, total,col after total
    '    End If

    '    For intL = 0 To intTotalCol - intColToRemain
    '        row.Cells.RemoveAt(1)
    '    Next

    '    row.Cells(0).ColumnSpan = intCell - 1

    '    Dim txtTotal As New TextBox
    '    txtTotal.ID = "Total"
    '    txtTotal.Text = Format(dblTotal, "#,##0.00")
    '    txtTotal.CssClass = "lblnumerictxtbox"
    '    txtTotal.ReadOnly = True
    '    txtTotal.Width = System.Web.UI.WebControls.Unit.Pixel(120)
    '    txtTotal.Font.Bold = True
    '    row.Cells(2).Controls.Add(txtTotal)
    '    row.Cells(1).Text = strLabel
    '    row.Cells(2).HorizontalAlign = HorizontalAlign.Right
    '    row.Cells(1).Font.Bold = True

    '    If dblTotal <> 0 Then
    '        ViewState("POCost") = dblTotal
    '    End If

    '    Dim txtTax As New TextBox
    '    If ViewState("GST") = "product" And blnShowGST Then
    '        txtTax.ID = "Tax"
    '        txtTax.CssClass = "lblnumerictxtbox"
    '        txtTax.ReadOnly = True
    '        txtTax.Width = System.Web.UI.WebControls.Unit.Pixel(100)
    '        txtTax.Font.Bold = True
    '        row.Cells(4).HorizontalAlign = HorizontalAlign.Right
    '        If dblTotalGst = 0 Then
    '            txtTax.Text = "0.0000"
    '        Else
    '            txtTax.Text = Format(dblTotalGst, "#,##0.00")
    '        End If
    '        row.Cells(4).Controls.Add(txtTax)
    '        row.Cells(5).ColumnSpan = intTotalCol - (intCell - 1)
    '    Else
    '        row.Cells(4).ColumnSpan = intTotalCol - (intCell - 2)
    '    End If

    '    If hidCost.Value = "" Then
    '        hidCost.Value = ViewState("POCost")
    '    End If

    '    row.BackColor = Color.FromName("#f4f4f4")
    '    dtgShopping.Controls(0).Controls.Add(row)

    '    If ViewState("GST") = "product" And blnShowGST Then
    '        hidTax.Value = txtTax.ClientID
    '    End If
    '    hidTotal.Value = hidTotal.Value & "," & txtTotal.ClientID
    'End Sub

    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub

    Private Sub dtg_freeform_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_freeform.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim txt_qty As TextBox
            Dim txt_desc As TextBox
            txt_desc = e.Item.FindControl("txt_desc")

            txt_desc.Attributes.Add("onKeyDown", "limitText (this, 500);")

            txt_qty = e.Item.FindControl("txt_qty")
            'txt_qty.Text = Common.parseNull(dv("RD_Quantity"))


            Dim val_Qty As RegularExpressionValidator
            val_Qty = e.Item.FindControl("val_Qty")
            val_Qty.ControlToValidate = "txt_qty"
            val_Qty.ValidationExpression = "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$" '"^([1-9]\d{0,5}\.\d{0,2}|[1-9]\d{0,5}|[0]\d{0,5}\.[1-9]\d{0,1})$" '"^(\d|,)*\.?\d{0,2}$"
            val_Qty.ErrorMessage = ViewState("ValQtyMsg")
            val_Qty.Text = "?"
            val_Qty.Display = ValidatorDisplay.Dynamic




            'Dim objrfq As New RFQ
            Dim ddl_uom As New DropDownList
            'Dim lbl_uom As Label
            Dim dv_uom As DataView
            'lbl_uom = e.Item.FindControl("lbl_uom")
            ddl_uom = e.Item.FindControl("ddl_uom")

            dv_uom = a.GetCodeTableView(CodeTable.Uom)
            Common.FillDdl(ddl_uom, "CODE_DESC", "CODE_ABBR", dv_uom)

            Dim ddlstock As DropDownList = e.Item.Cells(4).FindControl("ddl_stock")
            LoadStkDdl(ddlstock)
            'LoadStkDdl(ddl_stock_2)

            'Dim objGlobal As New AppGlobals
            'Dim lbl_uom As Label
            'Dim txt_desc As TextBox
            'Dim lbl_desc As Label
            'Dim lbl_limit As Label

            'txt_desc = e.Item.FindControl("txt_desc")
            'lbl_uom = e.Item.FindControl("lbl_uom")
            'lbl_desc = e.Item.FindControl("lbl_desc")
            'ddl_uom = e.Item.FindControl("ddl_uom")
            'txt_desc.Text = Common.parseNull(dv("RD_Product_Desc"))


            Dim txt_delivery As TextBox
            txt_delivery = e.Item.FindControl("txt_delivery")
            'txt_delivery.Text = Common.parseNull(dv("RD_Delivery_Lead_Time"))

            Dim val_delivery As RegularExpressionValidator
            val_delivery = e.Item.FindControl("val_delivery")
            val_delivery.ControlToValidate = "txt_delivery"
            val_delivery.ValidationExpression = "^\d+$"
            val_delivery.ErrorMessage = "Delivery Lead Time is expecting numeric value."
            val_delivery.Text = "?"
            val_delivery.Display = ValidatorDisplay.Dynamic

        End If

    End Sub

    Private Sub chkPCI_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPCI.CheckedChanged
        If chkPCI.Checked Then
            chkFreeform.Checked = False
            cmd_search.Visible = True
            dtg_Cat.DataSource = Nothing
            dtg_Cat.DataBind()
            dtg_freeform.DataSource = Nothing
            dtg_freeform.DataBind()
            'Bindgrid()
        End If
    End Sub

    Private Sub chkVCI_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkVCI.CheckedChanged
        If chkVCI.Checked Then
            chkFreeform.Checked = False
            cmd_search.Visible = True
            dtg_Cat.DataSource = Nothing
            dtg_Cat.DataBind()
            dtg_freeform.DataSource = Nothing
            dtg_freeform.DataBind()

        End If
    End Sub
End Class
