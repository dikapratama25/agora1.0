Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing
Public Class viewItem1
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents dg_viewitem As System.Web.UI.WebControls.DataGrid


    Protected WithEvents ddl_cur As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txt_rfq_name As System.Web.UI.WebControls.TextBox
    Protected WithEvents onchange As System.Web.UI.HtmlControls.HtmlInputHidden
    Dim back As String = 0
    Dim a As New AppGlobals
    Dim rfq_name As String
    Dim index As String
    Protected WithEvents cmd_update As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_delete As System.Web.UI.WebControls.Button
    Protected WithEvents lblExStock As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_rfq_number As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents RangeValidator1 As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents cmd_back As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents rvl_RFQName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cmd_reset As System.Web.UI.WebControls.Button
    Protected WithEvents lbl_check As System.Web.UI.WebControls.Label
    Dim rfq_index As Integer
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

    Public Enum ItemEnum
        Chk = 0
        Desc = 1
        UOM = 2
        QTY = 3
        Time = 4
        Warranty = 5
        Index = 6
        Line = 7
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmd_update.Enabled = False
        cmd_delete.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmd_update)
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmd_delete)
        htPageAccess.Add("delete", alButtonList)

        CheckButtonAccess()
        If ViewState("intPageRecordCnt") > 0 Then
            cmd_delete.Enabled = blnCanDelete
            cmd_update.Enabled = blnCanUpdate
        Else
            cmd_update.Enabled = False
            cmd_delete.Enabled = False
            cmd_reset.Enabled = False
        End If
        cmd_reset.Enabled = Not (blnCanAdd And blnCanDelete)
        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        blnSorting = False
        Me.blnPaging = False

        SetGridProperty(dg_viewitem)

        If Not Page.IsPostBack Then


            Me.lbl_rfq_number.Text = Session("rfq_num")

            ' Me.lbl_rfq_number.Text = Session("frq_number")
            Me.cmd_delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
            Me.ddl_cur.Attributes.Add("onchange", "check();")
            ViewState("rfq_name") = Me.Request.QueryString("rfq_name")
            ViewState("rfq_name1") = Me.Request.QueryString("rfq_name")
            Dim dv_cur As DataView
            Dim objrfq As New RFQ
            Me.txt_rfq_name.Text = ViewState("rfq_name")
            index = Me.Request.QueryString("rfq_index")
            Bindgrid(True)
            dv_cur = a.GetCodeTableView(CodeTable.Currency)
            Common.FillDdl(ddl_cur, "CODE_DESC", "CODE_ABBR", dv_cur)
            Dim rfq_cur As String = Me.Request.QueryString("RFQ_Cur")
            ViewState("cur") = Me.Request.QueryString("RFQ_Cur")
            Common.SelDdl(rfq_cur, ddl_cur)

        End If
        lblExStock.Text = "+ 0 denotes Ex-Stock. "
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objview As New RFQ
        Dim ds As New DataSet
        rfq_name = Me.txt_rfq_name.Text
        ds = objview.dg_view(rfq_name, Me.lbl_rfq_number.Text)
        'intRecord = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If ViewState("action") = "del" Then
            If dg_viewitem.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dg_viewitem.PageSize = 0 Then
                dg_viewitem.CurrentPageIndex = dg_viewitem.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        dg_viewitem.DataSource = dvViewSample
        dg_viewitem.DataBind()
        If Session("Env") = "FTN" Then
            Me.dg_viewitem.Columns(5).Visible = False
        Else
            Me.dg_viewitem.Columns(5).Visible = True
        End If
        '//datagrid.pageCount only got value after databind

    End Function

    Private Sub dg_viewitem_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_viewitem.ItemCreated
        Grid_ItemCreated(sender, e)
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dg_viewitem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dg_viewitem.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(ItemEnum.Chk).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            Dim txt_qty As TextBox
            txt_qty = e.Item.FindControl("txt_qty")
            txt_qty.Text = Common.parseNull(dv("RD_Quantity"))

            Dim val_Qty As RegularExpressionValidator
            val_Qty = e.Item.FindControl("val_Qty")
            val_Qty.ControlToValidate = "txt_qty"
            val_Qty.ValidationExpression = "(?!^0*$)^\d{1,5}?$"
            val_Qty.ErrorMessage = "Invalid quantity."
            val_Qty.Text = "?"
            val_Qty.Display = ValidatorDisplay.Dynamic

            Dim objrfq As New RFQ
            Dim ddl_uom As New DropDownList
            Dim dv_uom As DataView
            Dim objGlobal As New AppGlobals
            Dim lbl_uom As Label
            Dim txt_desc As TextBox
            Dim lbl_desc As Label
            Dim lbl_limit As Label

            txt_desc = e.Item.FindControl("txt_desc")
            lbl_uom = e.Item.FindControl("lbl_uom")
            lbl_desc = e.Item.FindControl("lbl_desc")
            ddl_uom = e.Item.FindControl("ddl_uom")
            txt_desc.Text = Common.parseNull(dv("RD_Product_Desc"))

            If dv("RD_Product_Code") = "99999" Then ' free form
                txt_desc.Visible = True
                lbl_desc.Visible = False
                ddl_uom.Visible = True
                lbl_uom.Visible = False

                ddl_uom.Attributes.Add("Onchange", "check();")
                objGlobal.FillCodeTable(ddl_uom, CodeTable.Uom)

                Common.SelDdl(objrfq.get_UOMcode(dv("RD_UOM"), "UOM"), ddl_uom, True, True)
                lbl_uom.Text = "1"
            Else ' cat   
                lbl_uom.Visible = True
                ddl_uom.Visible = False
                txt_desc.Visible = False
                lbl_desc.Visible = True

                lbl_uom.Text = Common.parseNull(dv("RD_UOM"))
                lbl_desc.Text = Common.parseNull(dv("RD_Product_Desc"))
            End If

            Dim txt_delivery As TextBox
            txt_delivery = e.Item.FindControl("txt_delivery")
            txt_delivery.Text = Common.parseNull(dv("RD_Delivery_Lead_Time"))

            Dim val_delivery As RegularExpressionValidator
            val_delivery = e.Item.FindControl("val_delivery")
            val_delivery.ControlToValidate = "txt_delivery"
            val_delivery.ValidationExpression = "^\d+$"
            val_delivery.ErrorMessage = "Delivery Lead Time is expecting numeric value."
            val_delivery.Text = "?"
            val_delivery.Display = ValidatorDisplay.Dynamic

            Dim txt_warranty As TextBox
            txt_warranty = e.Item.FindControl("txt_warranty")
            txt_warranty.Text = Common.parseNull(dv("RD_Warranty_Terms"))
            Dim val_warranty As RegularExpressionValidator
            val_warranty = e.Item.FindControl("val_warranty")
            val_warranty.ControlToValidate = "txt_warranty"
            val_warranty.ValidationExpression = "^\d+$"
            val_warranty.ErrorMessage = "Warranty Terms is expecting numeric value."
            val_warranty.Text = "?"
            val_warranty.Display = ValidatorDisplay.Dynamic
        End If
    End Sub

    Private Function validateDatagrid(ByRef strMsg As String, ByVal text As String, ByVal no As String, ByRef qid As TextBox) As Boolean
        validateDatagrid = True
        Dim txtQ As TextBox
        Dim dr As TableRow

        If Not Common.checkMaxLength(text, 400) Then
            strMsg &= "<li>Item Description No." & no & " is over limit.</li>"
            '   qid.Text = "?"
            validateDatagrid = False
        End If
    End Function

    Private Sub cmd_delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_delete.Click
        Dim dgItem As DataGridItem
        Dim strindex As String
        Dim objrfq As New RFQ
        Dim LINE As Integer
        Dim chkItem As CheckBox
        For Each dgItem In dg_viewitem.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                strindex = CType(dgItem.FindControl("index"), Label).Text
                LINE = CInt(dgItem.Cells(ItemEnum.Line).Text)
                objrfq.Delete_item(strindex, LINE)
            End If

        Next
        Bindgrid(True)
    End Sub

    Private Sub cmd_back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_back.ServerClick
        Dim page As String
        page = Me.Request.QueryString("page")
        Session("back") = "1"
        If page = 1 Then
            Me.Response.Redirect("Create_RFQ.aspx?pageid=" & strPageId & "&RFQ_Name=" & Server.UrlEncode(Me.txt_rfq_name.Text) & "&RFQ_Cur=" & Me.ddl_cur.SelectedItem.Value & "&RFQ_Cur_text=" & Me.ddl_cur.SelectedItem.Text & "")
        ElseIf page = 2 Then
            Me.Response.Redirect("Create_RFQ2.aspx?pageid=" & strPageId & "&RFQ_Name=" & Server.UrlEncode(Me.txt_rfq_name.Text) & "&RFQ_Cur=" & Me.ddl_cur.SelectedItem.Value & "&RFQ_Cur_text=" & Me.ddl_cur.SelectedItem.Text & "")
        ElseIf page = 3 Then
            Me.Response.Redirect("Create_RFQ3.aspx?pageid=" & strPageId & "&RFQ_name=" & Server.UrlEncode(rfq_name) & "&RFQ_by=" & Session("UserId") & "&cur=" & Me.ddl_cur.SelectedItem.Text & "")
        End If
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dg_viewitem.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub cmd_update_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_update.Click
        Dim dgItem As DataGridItem
        Dim objrfq As New RFQ
        Dim objval As New RFQ_User
        Dim objDB As New EAD.DBCom
        Dim strAryQuery(0) As String
        Dim strSQL As String
        Dim txtPrice As TextBox
        Dim txt_desc As TextBox
        Dim lblPrice As Label
        Dim chkItem As CheckBox
        Dim txt_qty As TextBox
        Dim i As Integer
        Dim strMsg As String
        Dim strMsg2 As String
        Dim lbl_limit As Label

        rfq_name = Me.Request.QueryString("rfq_name")
        objval.RFQ_Name = Me.txt_rfq_name.Text
        objval.RFQ_Name2 = ViewState("rfq_name")
        objval.cur_code = ddl_cur.SelectedItem.Value
        objval.Header_Ind = 1
        objval.index = objrfq.get_index(rfq_name)
        strSQL = objrfq.update_RFQMSTR(objval, ViewState("rfq_name1"))

        If strSQL = "1" Then
            Common.NetMsgbox(Me, "RFQ Name Exist", MsgBoxStyle.Information)
            Exit Sub
        End If
        Common.Insert2Ary(strAryQuery, strSQL)

        For Each dgItem In dg_viewitem.Items
            i = i + 1
            objval.Header_Ind = 0
            objval.lineno = dgItem.Cells(ItemEnum.Line).Text
            lbl_limit = dgItem.FindControl("lbl_limit")
            txt_desc = dgItem.FindControl("txt_desc")
            If CType(dgItem.FindControl("txt_desc"), TextBox).Text <> "" Then
                objval.productdesc = txt_desc.Text
                If Page.IsValid And validateDatagrid(strMsg2, txt_desc.Text, i, txt_desc) Then
                    lbl_limit.Text = ""
                Else
                    lbl_limit.Text = "?"
                    lbl_limit.CssClass = "lblnumerictxtbox"
                    lbl_limit.ForeColor = Color.Red
                    strMsg = strMsg2
                End If
            End If
            If CType(dgItem.FindControl("lbl_desc"), Label).Text <> "" Then
                objval.productdesc = CType(dgItem.FindControl("lbl_desc"), Label).Text
            End If

            If CType(dgItem.FindControl("lbl_uom"), Label).Text = "1" Then
                objval.uom = CType(dgItem.FindControl("ddl_uom"), DropDownList).SelectedItem.Text
            Else
                objval.uom = CType(dgItem.FindControl("lbl_uom"), Label).Text
            End If

            objval.Quantity = CType(dgItem.FindControl("txt_qty"), TextBox).Text
            objval.index = CType(dgItem.FindControl("index"), Label).Text
            objval.Delivery_Lead_Time = CType(dgItem.FindControl("txt_delivery"), TextBox).Text
            objval.Warranty_Terms = CType(dgItem.FindControl("txt_warranty"), TextBox).Text
            strSQL = objrfq.update_item(objval)
            Common.Insert2Ary(strAryQuery, strSQL)
        Next
        If strMsg <> "" Then
            Me.lbl_check.Text = strMsg
            intPageRecordCnt = 1
        Else
            Me.lbl_check.Text = ""
            objDB.BatchExecute(strAryQuery)
            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
            Bindgrid(True)
            ViewState("rfq_name1") = Me.txt_rfq_name.Text
            ViewState("cur") = Me.ddl_cur.SelectedItem.Value
            Me.onchange.Value = "0"
        End If
    End Sub

    Private Sub cmd_reset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_reset.Click
        Dim dv_cur As DataView
        Me.lbl_check.Text = ""
        txt_rfq_name.Text = ViewState("rfq_name1")
        dv_cur = a.GetCodeTableView(CodeTable.Currency)
        Common.FillDdl(ddl_cur, "CODE_DESC", "CODE_ABBR", dv_cur)
        Dim rfq_cur As String = ViewState("cur")
        Common.SelDdl(rfq_cur, ddl_cur)
        Bindgrid()
    End Sub
End Class
