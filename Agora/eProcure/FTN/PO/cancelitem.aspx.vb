Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class cancelitemFTN
    Inherits AgoraLegacy.AppBaseClass
    'Dim intPOStatus As Integer
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Po_No As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_date As System.Web.UI.WebControls.Label
    Protected WithEvents dtg_POList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmd_fulcancel As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_submit As System.Web.UI.WebControls.Button
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents txt_remark As System.Web.UI.WebControls.TextBox
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents revRemark As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents cmd_View As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Public linevalue(0) As String
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strQtyErr As String
    Dim objGlobal As New AppGlobals
    'Public Enum EnumPOCanl
    '    '-----One column added in the grid(dtg_POList). The values  changed by Praveen on 18/07/2007
    '    icPOLine = 0
    '    iclineno = 1
    '    icItemCode = 2 'Original value for ictemcode=1 has been changed  for line no
    '    icProDesc = 3
    '    icUOM = 4
    '    icMPQ = 5
    '    icWaranty = 6
    '    icOrderQty = 7
    '    icRecQty = 8
    '    icRejQty = 9
    '    icOutstanding = 10
    '    icQtyCanl = 11
    '    icRemarks = 12
    'End Enum
    Public Enum EnumPOCanl
        '-----One column added in the grid(dtg_POList). The values  changed by Praveen on 18/07/2007
        icPOLine = 0
        iclineno = 1
        icItemCode = 2 'Original value for ictemcode=1 has been changed  for line no
        icProDesc = 3
        icUOM = 4
        'icMPQ = 5
        'icWaranty = 6
        icOrderQty = 5
        icOrderQty1 = 6 'if without this column, the Receive Qty will not appear
        icRecQty = 7
        icRejQty = 8
        icOutstanding = 9
        icQtyCanl = 10
        icRemarks = 11
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        '    cmd_fulcancel.Enabled = False
        cmd_submit.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmd_submit)
        ' htPageAccess.Add("add", alButtonList)
        '  alButtonList = New ArrayList
        '   alButtonList.Add(cmd_fulcancel)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmd_submit)
        htPageAccess.Add("update", alButtonList)

        If intPageRecordCnt > 0 Then
            '  cmd_fulcancel.Enabled = blnCanAdd
            cmd_submit.Enabled = blnCanAdd And blnCanUpdate
        Else
            ' cmd_fulcancel.Enabled = False
            cmd_submit.Enabled = False
        End If

        '  cmd_createInv.Enabled = blnCanAdd
        CheckButtonAccess()

        alButtonList.Clear()
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnPaging = False

        If strPageId Is Nothing Then
            If Request.QueryString("pageid") Is Nothing Then
                strPageId = 7
            Else
                strPageId = Request.QueryString("pageid")
            End If
        End If

        If Not IsPostBack Then
            strQtyErr = objGlobal.GetErrorMessage("00342")
            ViewState("ValQtyMsg") = strQtyErr

            'lnkBack.NavigateUrl = dDispatcher.direct("PO", "POViewB2Cancel.aspx", "pageid=" & strPageId)
            GenerateTab()
            SetGridProperty(dtg_POList)
            Dim check As Boolean
            Dim objPO As New PurchaseOrder
            'check = objPO.check_fulfilment(Request(Trim("po_no")))
            'If check = False Then
            '    Me.cmd_fulcancel.Visible = True
            'End If
            lbl_Po_No.Text = Request(Trim("po_no"))
            'lbl_date.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, Request(Trim("date")))
            hidSummary.Value = "CR Remarks-" & txt_remark.ClientID
            Bindgrid()
        Else
            hidSummary.Value = "CR Remarks-" & txt_remark.ClientID
            '   Bindgrid(Me.dtg_cr, 3)
        End If
        '//no need full cancel button anymore
        Me.cmd_fulcancel.Visible = False
        intPageRecordCnt = viewstate("intPageRecordCnt")
        lnkBack.NavigateUrl = dDispatcher.direct("PO", "POViewB2Cancel.aspx", "pageid=" & strPageId)
        txt_remark.Attributes.Add("onKeyDown", "limitText (this, 1000);")
        'cmd_submit.Attributes.Add("onClick", "javascript:if (confirmation()) { return resetSummary(1,1); } else { return false; }")
        'cmd_submit.Attributes.Add("onClick", "return resetSummary(1,1);")
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String


        Dim objPO As New PurchaseOrder_Buyer

        '//Retrieve Data from Database
        Dim ds As New DataSet
        Dim b_com_id = Request(Trim("BCoyID"))
        Dim vendor As String = Request(Trim("vendor"))
        'Dim objdb As New EAD.DBCom()
        ' this Function is only for buyer side (no need to pass b_com_id)
        ds = objPO.get_poDetail2(Me.lbl_Po_No.Text, vendor)


        Dim PO_No As String = ""

        Dim dvViewPR As DataView
        dvViewPR = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewPR.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewPR.Sort += " DESC"
        End If

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        '//bind datagrid
        If ViewState("intPageRecordCnt") > 0 Then
            'intTotPage = dtgDept.PageCount
            lbl_date.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, ds.Tables(0).Rows(0)("POM_PO_DATE"))
            ViewState("intPOStatus") = ds.Tables(0).Rows(0)("POM_PO_STATUS")
            dtg_POList.DataSource = dvViewPR
            dtg_POList.DataBind()
            If ViewState("count_outstd") = intPageRecordCnt Then
                Me.cmd_fulcancel.Visible = False
                Me.cmd_submit.Visible = False
                Me.Table2.Visible = False
            End If

        Else
            'dtgDept.DataSource = ""
            dtg_POList.DataBind()
            ' Common.NetMsgbox(Me, "No record found.")
            'intTotPage = 0
        End If
        'If Session("Env") = "FTN" Then
        '    Me.dtg_POList.Columns(6).Visible = False
        'Else
        '    Me.dtg_POList.Columns(6).Visible = True
        'End If
        Me.dtg_POList.Columns(6).Visible = False
    End Function


    Public Sub dtg_POList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        Dim s As String = sender.ToString.Trim

        Bindgrid(True)

        ' Bindgrid(sender, )
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtg_POList.CurrentPageIndex = 0

        Bindgrid(True)

    End Sub

    Private Sub dtg_POList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_POList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            e.Item.Cells(1).Text = e.Item.DataSetIndex + 1

            Dim cancel_item As Decimal
            If IsDBNull(dv("POD_CANCELLED_QTY")) Then
                cancel_item = 0
            Else
                cancel_item = CDec(dv("POD_CANCELLED_QTY"))

            End If

            'Dim intmax As Integer = CInt(dv("POD_ORDERED_QTY")) - cancel_item - CInt(dv("POD_RECEIVED_QTY") - dv("POD_REJECTED_QTY"))
            Dim intmax As Decimal = CDec(dv("POD_ORDERED_QTY")) - cancel_item - CDec(dv("POD_RECEIVED_QTY") - dv("POD_REJECTED_QTY"))

            Dim txt_qtycancel As TextBox
            txt_qtycancel = e.Item.FindControl("txt_qtycancel")
            txt_qtycancel.Text = "0"

            e.Item.Cells(EnumPOCanl.icOutstanding).Text = intmax 'OUTSTANDING
            If intmax = 0 Then
                '  txt_qtycancel.Enabled = False
                txt_qtycancel.Visible = False
                ViewState("count_outstd") = ViewState("count_outstd") + 1
                e.Item.Enabled = False
                e.Item.Visible = False
            End If

            If (ViewState("intPOStatus") = POStatus_new.Open) Or (ViewState("intPOStatus") = POStatus_new.Submitted) Or (ViewState("intPOStatus") = POStatus_new.NewPO) Then
                txt_qtycancel.Text = intmax
                txt_qtycancel.Enabled = False
            End If

            Dim hidCode As HtmlInputHidden
            hidCode = e.Item.FindControl("hidCode")
            hidCode.Value = dv("POD_PO_LINE")

            Dim rv_qtycancel As RangeValidator
            rv_qtycancel = e.Item.FindControl("rv_qtycancel")
            'Dim intMin As Integer
            Dim intMin As Decimal
            intMin = 0.0
            rv_qtycancel.MaximumValue = intmax
            rv_qtycancel.MinimumValue = intMin
            rv_qtycancel.ControlToValidate = "txt_qtycancel"
            rv_qtycancel.ErrorMessage = hidCode.Value & ". Quantity To Cancel must less than or equal to Outstanding value."
            rv_qtycancel.Text = "?"
            rv_qtycancel.Type = ValidationDataType.Double
            rv_qtycancel.Display = ValidatorDisplay.Dynamic


            Dim rev_qtycancel As RegularExpressionValidator
            rev_qtycancel = e.Item.FindControl("rev_qtycancel")
            rev_qtycancel.ControlToValidate = "txt_qtycancel"
            'rev_qtycancel.ValidationExpression = "^\d+$"
            'rev_qtycancel.ValidationExpression = "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$"
            rev_qtycancel.ValidationExpression = "^\d{1,6}(?:\.(?:\d{0,1})?\d)?$"
            'rev_qtycancel.ErrorMessage = hidCode.Value & ". Quantity To Cancel is over limit/expecting numeric value."
            rev_qtycancel.ErrorMessage = "Invalid Quantity. Range must be from 0.00 to 999999.99" 'ViewState("ValQtyMsg")
            rev_qtycancel.Text = "?"
            rev_qtycancel.Display = ValidatorDisplay.Dynamic

            Dim txtRemark As TextBox
            txtRemark = e.Item.FindControl("txt_remarkdetail")
            txtRemark.Attributes.Add("onKeyDown", "limitText (this, 400);")

            ' for '?' purpose
            Dim txtQ As TextBox
            txtQ = e.Item.FindControl("txtQ")

            If hidSummary.Value = "" Then
                hidSummary.Value = "Remarks-" & txtRemark.ClientID
            Else
                hidSummary.Value &= ",Remarks-" & txtRemark.ClientID
            End If

            If hidControl.Value = "" Then
                hidControl.Value = hidCode.ClientID & "-" & txtQ.ClientID
            Else
                hidControl.Value &= "," & hidCode.ClientID & "-" & txtQ.ClientID
            End If


            'To validate txtprice when user press on delete & space infront the value
            txt_qtycancel.Attributes.Add("onBlur", "resetValue('" & txt_qtycancel.ClientID & "','0')")
        End If
    End Sub

    Sub cancel_qty(ByVal meth As String)
        Dim OBJGLB As New AppGlobals
        Dim objpo As New PurchaseOrder_Buyer
        Dim objdb As New EAD.DBCom
        Dim array(0) As String
        Dim CR_num As String
        Dim prefix As String
        Dim txt_remarks As New TextBox
        Dim strArray(0) As String
        Dim dgitem As DataGridItem
        Dim strsql As String
        Dim check As String
        ' check = objpo.check_crexist(Request(Trim("INDEX")))
        Dim ds As New DataSet
        Dim dtr As DataRow
        Dim i As Integer
        Dim strMsg As String

        '----Table(0)------


        Dim cancellation As New DataTable
        cancellation.Columns.Add("INDEX", Type.GetType("System.String"))
        cancellation.Columns.Add("vendor", Type.GetType("System.String"))
        cancellation.Columns.Add("REMARK", Type.GetType("System.String"))
        dtr = cancellation.NewRow()

        dtr("INDEX") = Request(Trim("INDEX"))
        dtr("vendor") = Request(Trim("vendor"))
        dtr("REMARK") = Me.txt_remark.Text
        cancellation.Rows.Add(dtr)
        ds.Tables.Add(cancellation)
        check = "0"

        '--- table(1) 

        Dim fulfil As New DataTable
        fulfil.Columns.Add("po_no", Type.GetType("System.String"))
        fulfil.Columns.Add("status", Type.GetType("System.String"))

        Dim fulfilstatus As Integer = Fulfilment.Pending_Cancel_Ack
        Dim postatus1 As Integer
        If ViewState("intPOStatus") = POStatus_new.Open Or ViewState("intPOStatus") = POStatus_new.NewPO Then
            postatus1 = POStatus_new.Cancelled
        ElseIf ViewState("intPOStatus") = POStatus_new.Submitted Then
            postatus1 = POStatus_new.CancelledBy
        Else
            postatus1 = 0
        End If

        dtr = fulfil.NewRow()
        dtr("po_no") = Request(Trim("po_no"))
        If Request.QueryString("Status") = POStatus_new.Submitted Then
            dtr("status") = "0"
        Else
            dtr("status") = fulfilstatus
        End If
        '//dtr("Cancelled") - used to controlled full cancellation
        If meth = 1 Then 'FULL CANCELLATION (CLICK ON FULL CANCELLATION BUTTON)
            fulfil.Columns.Add("Cancelled", Type.GetType("System.String"))
            dtr("Cancelled") = postatus1
        ElseIf meth = 2 Then 'CLICK "QTY TO CANCEL"
            fulfil.Columns.Add("Cancelled", Type.GetType("System.String"))
            'If ViewState("intPOStatus") = POStatus_new.Open Or ViewState("intPOStatus") = POStatus_new.Submitted Or ViewState("intPOStatus") = POStatus_new.NewPO Then
            If postatus1 <> 0 Then
                dtr("Cancelled") = postatus1
            Else
                dtr("Cancelled") = ""
            End If
        End If

        fulfil.Rows.Add(dtr)
        ds.Tables.Add(fulfil)

        '--- table(2) 


        Dim crdetail As New DataTable

        crdetail.Columns.Add("lineno", Type.GetType("System.String"))
        crdetail.Columns.Add("qty_cancel", Type.GetType("System.String"))
        crdetail.Columns.Add("remarks", Type.GetType("System.String"))
        Dim txt_qtycancel As TextBox
        For Each dgitem In Me.dtg_POList.Items

            '--------New line code added to get the line no in the grid by Praveen on 18/07/2007
            dgitem.Cells(1).Text = dgitem.DataSetIndex + 1
            'linevalue(0) = dgitem.Cells(1).Text
            '------End

            txt_qtycancel = dgitem.FindControl("txt_qtycancel")
            dtr = crdetail.NewRow()
            txt_remarks = dgitem.FindControl("txt_remarkdetail")
            dtr("remarks") = txt_remarks.Text
            dtr("lineno") = dgitem.Cells(0).Text
            If meth = 1 Then
                dtr("qty_cancel") = dgitem.Cells(9).Text
                If txt_qtycancel.Text <> 0 And txt_qtycancel.Text <> "" Then
                    crdetail.Rows.Add(dtr)
                End If
            Else
                dtr("qty_cancel") = txt_qtycancel.Text
                If txt_qtycancel.Text <> 0 And txt_qtycancel.Text <> "" Then
                    crdetail.Rows.Add(dtr)
                End If
            End If
            'dtr("check") = check
            'Common.Insert2Ary(strArray, strsql)
        Next

        If crdetail.Rows.Count = 0 Then
            Common.NetMsgbox(Me, "Please cancel at least one item.")
            Exit Sub
        End If
        Dim CR_NO, strCancelType As String
        ds.Tables.Add(crdetail)
        'If Not objpo.update_cancellation(ds, CR_NO) Then
        If ViewState("intPOStatus") = POStatus_new.Submitted Then
            strCancelType = "CB" 'ie. cancel before send to Vendor
        Else
            strCancelType = ""
        End If

        'strMsg = objpo.update_cancellation(ds, CR_NO, strCancelType)
        'ViewState("cr_no") = CR_NO
        'Common.NetMsgbox(Me, strMsg)
        'If CR_NO <> "" Then
        '    Me.cmd_View.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewCR.aspx", "PO_No=" & Request(Trim("PO_No")) & "&cr_no=" & ViewState("cr_no") & "&BCoyID=" & Request(Trim("BCoyID"))) & "')")
        '    cmd_View.Visible = True
        '    cmd_submit.Visible = False
        'End If
        If Not objpo.update_cancellation(ds, CR_NO, strCancelType, strMsg, False) Then
            ViewState("cr_no") = CR_NO
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Exclamation, "Wheel")
        Else
            ViewState("cr_no") = CR_NO
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information, "Wheel")
            Me.cmd_View.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewCR.aspx", "PO_No=" & Request(Trim("PO_No")) & "&cr_no=" & ViewState("cr_no") & "&BCoyID=" & Request(Trim("BCoyID"))) & "')")

            cmd_View.Visible = True
            cmd_submit.Visible = False
        End If
    End Sub

    Private Sub cmd_fulcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_fulcancel.Click
        cancel_qty("1")
    End Sub

    Private Sub cmd_submit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_submit.Click
        If txt_remark.Text = "" Then
            txt_remark.Focus()
            Common.NetMsgbox(Me, "Please enter CR Remarks.", MsgBoxStyle.Information)
            Exit Sub
        End If
        cancel_qty("2")

    End Sub

    Private Sub dtgCustomField_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtg_POList.ItemCreated
        '//this line must be included
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtg_POList, e)
        '//to add a JavaScript to CheckAll button
        'If e.Item.ItemType = ListItemType.Header Then
        '    Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
        '    chkAll.Attributes.Add("onclick", "selectAll();")
        'End If
    End Sub

    Private Sub PreviewCR()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Request(Trim("BCoyID")), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT   PO_MSTR.POM_PO_INDEX, PO_MSTR.POM_PO_NO, PO_MSTR.POM_B_COY_ID, PO_MSTR.POM_BUYER_ID, " _
                            & "PO_MSTR.POM_BUYER_NAME, PO_MSTR.POM_BUYER_PHONE, PO_MSTR.POM_BUYER_FAX,  " _
                            & "PO_MSTR.POM_S_COY_ID, PO_MSTR.POM_S_COY_NAME, PO_MSTR.POM_S_ATTN,  " _
                            & "PO_MSTR.POM_S_REMARK, PO_MSTR.POM_S_ADDR_LINE1, PO_MSTR.POM_S_ADDR_LINE2,  " _
                            & "PO_MSTR.POM_S_ADDR_LINE3, PO_MSTR.POM_S_POSTCODE, PO_MSTR.POM_S_CITY, " _
                            & "PO_MSTR.POM_S_STATE, PO_MSTR.POM_S_COUNTRY, PO_MSTR.POM_S_PHONE, PO_MSTR.POM_S_FAX, " _
                            & "PO_MSTR.POM_S_EMAIL, PO_MSTR.POM_PO_DATE, PO_MSTR.POM_FREIGHT_TERMS, " _
                            & "PO_MSTR.POM_PAYMENT_TERM, PO_MSTR.POM_PAYMENT_METHOD, PO_MSTR.POM_SHIPMENT_MODE, " _
                            & "PO_MSTR.POM_SHIPMENT_TERM, PO_MSTR.POM_CURRENCY_CODE, PO_MSTR.POM_EXCHANGE_RATE, " _
                            & "PO_MSTR.POM_PAYMENT_TERM_CODE, PO_MSTR.POM_SHIP_VIA, PO_MSTR.POM_PO_STATUS,  " _
                            & "PO_MSTR.POM_STATUS_CHANGED_BY, PO_MSTR.POM_STATUS_CHANGED_ON,  " _
                            & "PO_MSTR.POM_EXTERNAL_REMARK, PO_MSTR.POM_CREATED_BY, PO_MSTR.POM_PO_COST, " _
                            & "PO_MSTR.POM_BILLING_METHOD, PO_MSTR.POM_PO_PREFIX, PO_MSTR.POM_B_ADDR_CODE, " _
                            & "PO_MSTR.POM_B_ADDR_LINE1, PO_MSTR.POM_B_ADDR_LINE2, PO_MSTR.POM_B_ADDR_LINE3, " _
                            & "PO_MSTR.POM_B_POSTCODE, PO_MSTR.POM_B_CITY, PO_MSTR.POM_B_STATE,  " _
                            & "PO_MSTR.POM_B_COUNTRY, PO_MSTR.POM_FULFILMENT, PO_MSTR.POM_DEPT_INDEX,  " _
                            & "PO_MSTR.POM_ACCEPTED_DATE, PO_MSTR.POM_DOWNLOADED_DATE, PO_MSTR.POM_ARCHIVE_IND, " _
                            & "PO_MSTR.POM_TERMANDCOND, PO_MSTR.POM_REFERENCE_NO, PO_MSTR.POM_EXTERNAL_IND,  " _
                            & "PO_MSTR.POM_PRINT_CUSTOM_FIELDS, PO_MSTR.POM_PRINT_REMARK, PO_DETAILS.POD_COY_ID, " _
                            & "PO_DETAILS.POD_PO_NO, PO_DETAILS.POD_PO_LINE, PO_DETAILS.POD_PRODUCT_CODE,  " _
                            & "PO_DETAILS.POD_VENDOR_ITEM_CODE, PO_DETAILS.POD_PRODUCT_DESC, PO_DETAILS.POD_UOM, " _
                            & "PO_DETAILS.POD_ORDERED_QTY, PO_DETAILS.POD_RECEIVED_QTY, PO_DETAILS.POD_REJECTED_QTY,  " _
                            & "PO_DETAILS.POD_DELIVERED_QTY, PO_DETAILS.POD_CANCELLED_QTY, PO_DETAILS.POD_MIN_PACK_QTY, " _
                            & "PO_DETAILS.POD_MIN_ORDER_QTY, PO_DETAILS.POD_ETD, PO_DETAILS.POD_WARRANTY_TERMS,  " _
                            & "PO_DETAILS.POD_UNIT_COST, PO_DETAILS.POD_REMARK, PO_DETAILS.POD_GST,  " _
                            & "PO_DETAILS.POD_PR_INDEX, PO_DETAILS.POD_PR_LINE, PO_DETAILS.POD_ACCT_INDEX,  " _
                            & "PO_DETAILS.POD_PRODUCT_TYPE, PO_DETAILS.POD_B_ITEM_CODE, PO_DETAILS.POD_SOURCE, " _
                            & "PO_DETAILS.POD_D_ADDR_CODE, PO_DETAILS.POD_D_ADDR_LINE1, PO_DETAILS.POD_D_ADDR_LINE2,  " _
                            & "PO_DETAILS.POD_D_ADDR_LINE3, PO_DETAILS.POD_D_POSTCODE, PO_DETAILS.POD_D_CITY, " _
                            & "PO_DETAILS.POD_D_STATE, PO_DETAILS.POD_D_COUNTRY, PO_DETAILS.POD_B_CATEGORY_CODE,  " _
                            & "PO_DETAILS.POD_B_GL_CODE, COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, " _
                            & "COMPANY_MSTR.CM_COY_TYPE, COMPANY_MSTR.CM_PARENT_COY_ID, COMPANY_MSTR.CM_ACCT_NO,  " _
                            & "COMPANY_MSTR.CM_BANK, COMPANY_MSTR.CM_BRANCH, COMPANY_MSTR.CM_ADDR_LINE1, " _
                            & "COMPANY_MSTR.CM_ADDR_LINE2, COMPANY_MSTR.CM_ADDR_LINE3, COMPANY_MSTR.CM_POSTCODE,  " _
                            & "COMPANY_MSTR.CM_CITY, COMPANY_MSTR.CM_STATE, COMPANY_MSTR.CM_COUNTRY, " _
                            & "COMPANY_MSTR.CM_PHONE, COMPANY_MSTR.CM_FAX, COMPANY_MSTR.CM_EMAIL, " _
                            & "COMPANY_MSTR.CM_COY_LOGO, COMPANY_MSTR.CM_BUSINESS_REG_NO, " _
                            & "COMPANY_MSTR.CM_TAX_REG_NO, COMPANY_MSTR.CM_PAYMENT_TERM,  " _
                            & "COMPANY_MSTR.CM_PAYMENT_METHOD, COMPANY_MSTR.CM_ACTUAL_TERMSANDCONDFILE, " _
                            & "COMPANY_MSTR.CM_HUB_TERMSANDCONDFILE, COMPANY_MSTR.CM_PWD_DURATION,  " _
                            & "COMPANY_MSTR.CM_TAX_CALC_BY, COMPANY_MSTR.CM_CURRENCY_CODE, " _
                            & "COMPANY_MSTR.CM_BCM_SET, COMPANY_MSTR.CM_BUDGET_FROM_DATE, " _
                            & "COMPANY_MSTR.CM_BUDGET_TO_DATE, COMPANY_MSTR.CM_RFQ_OPTION, " _
                            & "COMPANY_MSTR.CM_LICENCE_PACKAGE, COMPANY_MSTR.CM_LICENSE_USERS,  " _
                            & "COMPANY_MSTR.CM_SUB_START_DT, COMPANY_MSTR.CM_SUB_END_DT,  " _
                            & "COMPANY_MSTR.CM_LICENSE_PRODUCTS, COMPANY_MSTR.CM_FINDEPT_MODE,  " _
                            & "COMPANY_MSTR.CM_PRIV_LABELING, COMPANY_MSTR.CM_SKINS_ID, COMPANY_MSTR.CM_TRAINING,  " _
                            & "COMPANY_MSTR.CM_STATUS, COMPANY_MSTR.CM_DELETED, COMPANY_MSTR.CM_MOD_BY,  " _
                            & "COMPANY_MSTR.CM_MOD_DT, COMPANY_MSTR.CM_ENT_BY, COMPANY_MSTR.CM_ENT_DT, " _
                            & "COMPANY_MSTR.CM_SKU, COMPANY_MSTR.CM_TRANS_NO, COMPANY_MSTR.CM_CONTACT,  " _
                            & "COMPANY_MSTR.CM_REPORT_USERS, COMPANY_MSTR.CM_INV_APPR, COMPANY_MSTR.CM_MULTI_PO,  " _
                            & "COMPANY_MSTR.CM_BA_CANCEL, PO_CR_MSTR.PCM_CR_NO, PO_CR_MSTR.PCM_B_COY_ID,  " _
                            & "PO_CR_MSTR.PCM_S_COY_ID, PO_CR_MSTR.PCM_PO_INDEX, PO_CR_MSTR.PCM_CR_STATUS,  " _
                            & "PO_CR_MSTR.PCM_REQ_BY, PO_CR_MSTR.PCM_REQ_DATE, PO_CR_MSTR.PCM_CR_REMARKS,  " _
                            & "PO_CR_DETAILS.PCD_CR_NO, PO_CR_DETAILS.PCD_COY_ID, PO_CR_DETAILS.PCD_PO_LINE,  " _
                            & "PO_CR_DETAILS.PCD_CANCELLED_QTY, PO_CR_DETAILS.PCD_REMARKS, USER_MSTR.UM_AUTO_NO,  " _
                            & "USER_MSTR.UM_USER_ID, USER_MSTR.UM_DELETED, USER_MSTR.UM_PASSWORD,  " _
                            & "USER_MSTR.UM_USER_NAME, USER_MSTR.UM_COY_ID, USER_MSTR.UM_DEPT_ID,  " _
                            & "USER_MSTR.UM_EMAIL, USER_MSTR.UM_APP_LIMIT, USER_MSTR.UM_DESIGNATION,  " _
                            & "USER_MSTR.UM_TEL_NO, USER_MSTR.UM_FAX_NO, USER_MSTR.UM_USER_SUSPEND_IND,  " _
                            & "USER_MSTR.UM_PASSWORD_LAST_CHANGED, USER_MSTR.UM_NEW_PASSWORD_IND,  " _
                            & "USER_MSTR.UM_NEXT_EXPIRE_DT, USER_MSTR.UM_LAST_LOGIN_DT, USER_MSTR.UM_QUESTION,  " _
                            & "USER_MSTR.UM_ANSWER, USER_MSTR.UM_MASS_APP, USER_MSTR.UM_STATUS,  " _
                            & "USER_MSTR.UM_MOD_BY, USER_MSTR.UM_MOD_DT, USER_MSTR.UM_ENT_BY,  " _
                            & "USER_MSTR.UM_ENT_DATE, USER_MSTR.UM_RECORD_COUNT, USER_MSTR.UM_EMAIL_CC,  " _
                            & "USER_MSTR.UM_INVOICE_APP_LIMIT, USER_MSTR.UM_INVOICE_MASS_APP, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND  " _
                            & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS CMState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_S_STATE) AND (CODE_CATEGORY = 's') AND  " _
                            & "(CODE_VALUE = PO_MSTR.POM_S_COUNTRY)) AS SupplierAddrState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_S_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS SupplierAddrCtry, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_B_STATE) AND (CODE_CATEGORY = 's') AND  " _
                            & "(CODE_VALUE = PO_MSTR.POM_B_COUNTRY)) AS BillAddrState, " _
                            & "(SELECT   CODE_DESC " _
                            & "FROM      CODE_MSTR AS a " _
                            & "WHERE   (CODE_ABBR = PO_MSTR.POM_B_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS BillAddrCtry, " _
                            & "(SELECT   CM_BUSINESS_REG_NO " _
                            & "FROM      COMPANY_MSTR AS B " _
                            & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS sUPPBUSSREGNO, " _
                            & "(SELECT   CM_EMAIL " _
                            & "FROM      COMPANY_MSTR AS B " _
                            & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS SUPPEMAIL, " _
                            & "(SELECT   CM_PHONE " _
                            & "FROM      COMPANY_MSTR AS B " _
                            & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS SUPPPHONE, " _
                            & "(SELECT   um_User_name FROM User_MSTR AS B WHERE   (PCM_REQ_By = UM_User_ID AND PCM_B_COY_ID = UM_coy_ID)) AS PCMCRBUYERNAME " _
                            & "FROM      PO_MSTR INNER JOIN " _
                            & "PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO AND  " _
                            & "PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID INNER JOIN " _
                            & "COMPANY_MSTR ON PO_MSTR.POM_B_COY_ID = COMPANY_MSTR.CM_COY_ID INNER JOIN " _
                            & "PO_CR_MSTR ON PO_MSTR.POM_PO_INDEX = PO_CR_MSTR.PCM_PO_INDEX AND  " _
                            & "PO_MSTR.POM_B_COY_ID = PO_CR_MSTR.PCM_B_COY_ID INNER JOIN " _
                            & "PO_CR_DETAILS ON PO_CR_MSTR.PCM_CR_NO = PO_CR_DETAILS.PCD_CR_NO AND  " _
                            & "PO_CR_MSTR.PCM_B_COY_ID = PO_CR_DETAILS.PCD_COY_ID AND  " _
                            & "PO_DETAILS.POD_PO_LINE = PO_CR_DETAILS.PCD_PO_LINE INNER JOIN " _
                            & "USER_MSTR ON PO_MSTR.POM_BUYER_ID = USER_MSTR.UM_USER_ID AND  " _
                            & "PO_MSTR.POM_B_COY_ID = USER_MSTR.UM_COY_ID " _
                            & "WHERE   (PO_MSTR.POM_B_COY_ID = @prmCoyID) AND (PO_MSTR.POM_PO_NO = @prmPONo) AND  " _
                            & "(PO_CR_MSTR.PCM_CR_NO = @prmCRNo)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Request(Trim("BCoyID"))))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmPONo", Request(Trim("po_no"))))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCRNo", ViewState("cr_no")))

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewCR_FTN_DataTable1", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = appPath & "PO\PreviewCR-FTN.rdlc"
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            TotalParameter = localreport.GetParameters.Count
            Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
            'Dim paramlist As New Generic.List(Of ReportParameter)
            For I = 0 To localreport.GetParameters.Count - 1
                GetParameter = localreport.GetParameters.Item(I).Name
                Select Case LCase(GetParameter)
                    Case "par1"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            Dim deviceInfo As String = _
                "<DeviceInfo>" + _
                    "  <OutputFormat>EMF</OutputFormat>" + _
                    "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

            Dim fs As New FileStream(appPath & "PO\CRReport.PDF", FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('CRReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
            strJScript += "</script>"
            Response.Write(strJScript)

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Session("w_POCancel_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""POViewB2.aspx?pageid=" & strPageId & """><span>Purchase Order</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn_selected"" href=""POViewB2Cancel.aspx?type=Listing&pageid=" & strPageId & """><span>Purchase Order Cancellation</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '             "</ul><div></div></div>"
        Session("w_POCancel_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POViewB2Cancel.aspx", "type=Listing&pageid=" & strPageId) & """><span>Purchase Order Cancellation</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
    End Sub

    'Protected Sub cmd_view_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_View.Click
    '    PreviewCR()
    'End Sub
End Class