Imports AgoraLegacy
Imports eProcure.Component
Public Class InventoryVerificationDetails
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strFrm As String
    Dim intLocIndicator As Integer
    Dim strLoc As String
    Dim strSLoc As String
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

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objInv As New Inventory

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgInv)
        Me.cmd_Save.Enabled = False
        If Not Page.IsPostBack Then
            GenerateTab()
            'objInv.GetLocationInfo(strLoc, strSLoc, intLocIndicator)
            ViewState("SortAscending") = "no"
            ViewState("SortExpression") = "IM_ITEM_CODE"
            dtgInv.CurrentPageIndex = 0
            BindInv(True)
            'If intLocIndicator > 1 Then 'Sub Location is defined
            '    Me.dtgInv.Columns(3).Visible = True
            'Else
            '    Me.dtgInv.Columns(3).Visible = False
            'End If
        End If
        strFrm = Me.Request.QueryString("Frm")

        Me.lblGRNNo.Text = Request.QueryString("GRNNo")
        Me.lblVendor.Text = Request.QueryString("Vendor")
        Me.lblGRNDate.Text = Request.QueryString("GRNDate")
        Me.lblReceivedDate.Text = Request.QueryString("ReceivedDate")
        ChangeHeaderText()

        If strFrm = "InventoryVerification" Then
            'lnkBack.NavigateUrl = "POList.aspx?pageid=" & strPageId
            lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "InventoryVerification.aspx", "pageid=" & strPageId)
            'Michelle (30/9/2010) - To link back to Dashboard if calling page is from Dashboard
        ElseIf strFrm = "Dashboard" Then
            'lnkBack.NavigateUrl = "../Dashboard/Vendor.aspx?pageid=0"
            lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=0")
        End If
        objInv = Nothing
    End Sub

    Private Sub ChangeHeaderText()
        Dim objInv As New Inventory

        objInv.GetLocationInfo(strLoc, strSLoc, intLocIndicator)

        dtgInv.Columns(2).HeaderText = strLoc
        dtgInv.Columns(3).HeaderText = strSLoc
        If intLocIndicator > 1 Then 'Sub Location is defined
            Me.dtgInv.Columns(3).Visible = True
        Else
            Me.dtgInv.Columns(3).Visible = False
        End If
        objInv = Nothing

    End Sub

    Private Sub BindInv(Optional ByVal pSorted As Boolean = False)
        Dim objInv As New Inventory
        Dim ds As DataSet
        Dim dvViewInv As DataView

        ds = objInv.GetInvVerifyDetails(Request.QueryString("GRNNo"))
        dvViewInv = ds.Tables(0).DefaultView
        dvViewInv.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewInv.Sort += " DESC"

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgInv, dvViewInv)
            dtgInv.DataSource = dvViewInv
            dtgInv.DataBind()
            'Else
            'dtgInv.DataBind()
            'Common.NetMsgbox(Me, MsgNoRecord)
            'If strFrm = "InventoryVerification" Then
            '    Response.Redirect(dDispatcher.direct("Inventory", "InventoryVerification.aspx", "pageid=" & strPageId))
            'ElseIf strFrm = "Dashboard" Then
            '    Response.Redirect(dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=0"))
            'End If
        End If
        ViewState("PageCount") = dtgInv.PageCount
        objInv = Nothing

    End Sub

    Private Sub dtgInv_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInv.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgInv, e)
        ChangeHeaderText()
    End Sub

    Private Sub dtgInv_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInv.ItemDataBound
        Dim objInv As New Inventory
        Dim objGlobal As New AppGlobals
        Dim intPass As Decimal 'Integer
        Dim intFail As Decimal 'Integer

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim txtPass As TextBox
            Dim txtFail As TextBox

            txtPass = e.Item.FindControl("txtPassQty")
            txtFail = e.Item.FindControl("txtFailQty")

            e.Item.Cells(5).Text = objInv.GetInvVerifyDetailsQty(Request.QueryString("GRNNo"), dv("IV_INVENTORY_INDEX"), dv("IV_LOCATION_INDEX"), "VP") 'Passed Qty
            e.Item.Cells(6).Text = objInv.GetInvVerifyDetailsQty(Request.QueryString("GRNNo"), dv("IV_INVENTORY_INDEX"), dv("IV_LOCATION_INDEX"), "VF") 'Failed Qty

            Dim hidCode As HtmlInputHidden
            hidCode = e.Item.FindControl("hidCode")
            hidCode.Value = dv("IM_INVENTORY_NAME") & " (" & dv("LM_LOCATION")
            If Not IsDBNull(dv("LM_SUB_LOCATION")) Then
                If dv("LM_SUB_LOCATION") <> "" Then
                    hidCode.Value = hidCode.Value & "," & dv("LM_SUB_LOCATION") & ")"
                Else
                    hidCode.Value = hidCode.Value & ")"
                End If
            Else
                hidCode.Value = hidCode.Value & ")"
            End If

            'Verify for Pass Qty
            Dim revPassQty As RegularExpressionValidator
            revPassQty = e.Item.FindControl("revPassQty")
            revPassQty.ValidationExpression = "^\d{1,6}(?:\.(?:\d{0,1})?\d)?$" '"^\d+$"
            revPassQty.ControlToValidate = "txtPassQty"
            revPassQty.ErrorMessage = hidCode.Value & " - " & Me.dtgInv.Columns(7).HeaderText & " " & objGlobal.GetErrorMessage("00013")
            revPassQty.Text = "?"
            revPassQty.Display = ValidatorDisplay.Dynamic

            'intPass = dv("IV_RECEIVE_QTY") - Val(e.Item.Cells(5).Text) - Val(e.Item.Cells(6).Text) - Val(txtFail.Text)

            'Dim cvPassQty As CompareValidator
            'cvPassQty = e.Item.FindControl("cvPassQty")
            ''cvShipped.ValueToCompare = e.Item.Cells(9).Text
            'cvPassQty.ValueToCompare = intPass
            ''cvShipped.ControlToValidate = "txtShiped"
            'cvPassQty.Type = ValidationDataType.Integer
            'cvPassQty.Operator = ValidationCompareOperator.LessThanEqual
            'cvPassQty.ErrorMessage = hidCode.Value & " - Pass Quantity is over limit."
            'cvPassQty.Text = "?"
            'cvPassQty.Display = ValidatorDisplay.Dynamic

            'Verify for Fail Qty
            Dim revFailQty As RegularExpressionValidator
            revFailQty = e.Item.FindControl("revFailQty")
            revFailQty.ValidationExpression = "^\d{1,6}(?:\.(?:\d{0,1})?\d)?$" '"^\d+$"
            revFailQty.ControlToValidate = "txtFailQty"
            revFailQty.ErrorMessage = hidCode.Value & " - " & Me.dtgInv.Columns(8).HeaderText & " " & objGlobal.GetErrorMessage("00013")
            revFailQty.Text = "?"
            revFailQty.Display = ValidatorDisplay.Dynamic

            'intFail = dv("IV_RECEIVE_QTY") - Val(e.Item.Cells(5).Text) - Val(e.Item.Cells(6).Text) - Val(txtPass.Text)

            'Dim cvFailQty As CompareValidator
            'cvFailQty = e.Item.FindControl("cvFailQty")
            ''cvShipped.ValueToCompare = e.Item.Cells(9).Text
            'cvFailQty.ValueToCompare = intFail
            ''cvShipped.ControlToValidate = "txtShiped"
            'cvFailQty.Type = ValidationDataType.Integer
            'cvFailQty.Operator = ValidationCompareOperator.LessThanEqual
            'cvFailQty.ErrorMessage = hidCode.Value & " - Fail Quantity is over limit."
            'cvFailQty.Text = "?"
            'cvFailQty.Display = ValidatorDisplay.Dynamic

            ' for '?' purpose
            Dim txtQ As TextBox
            txtQ = e.Item.FindControl("txtQ")

            If hidControl.Value = "" Then
                hidControl.Value = hidCode.ClientID & "-" & txtQ.ClientID
            Else
                hidControl.Value &= "," & hidCode.ClientID & "-" & txtQ.ClientID
            End If
            ' ai chu add end

            ''To validate txtprice when user press on delete & space infront the value
            'txtShip.Attributes.Add("onBlur", "resetValue('" & txtShip.ClientID & "','1')")
        End If
        'ChangeHeaderText()
        objInv = Nothing
        objGlobal = Nothing

    End Sub

    Private Sub dtgInv_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgInv.PageIndexChanged
        dtgInv.CurrentPageIndex = e.NewPageIndex
        BindInv(True)
        ChangeHeaderText()
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgInv.SortCommand
        Grid_SortCommand(sender, e)
        dtgInv.CurrentPageIndex = 0
        BindInv(True)
        ChangeHeaderText()
    End Sub

    Private Function BindData(ByRef pass As Boolean) As DataSet
        Dim ds As New DataSet
        Dim dtData As New DataTable
        Dim dtMstr As New DataTable
        Dim dtDetails As New DataTable
        Dim dtr As DataRow

        dtMstr.Columns.Add("IV_INVENTORY_INDEX", Type.GetType("System.String"))
        dtMstr.Columns.Add("IV_LOCATION_INDEX", Type.GetType("System.String"))
        dtMstr.Columns.Add("IV_VERIFY_INDEX", Type.GetType("System.String"))
        dtMstr.Columns.Add("IV_VERIFY_STATUS", Type.GetType("System.String"))

        dtData.Columns.Add("IT_INVENTORY_INDEX", Type.GetType("System.String"))
        dtData.Columns.Add("IT_TRANS_TYPE", Type.GetType("System.String"))
        dtData.Columns.Add("IT_TRANS_REF_NO", Type.GetType("System.String"))
        dtData.Columns.Add("IT_TRANS_QTY", Type.GetType("System.String"))
        dtData.Columns.Add("IT_TO_LOCATION_INDEX", Type.GetType("System.String"))
        dtData.Columns.Add("IT_REF_NO", Type.GetType("System.String"))
        dtData.Columns.Add("IV_VERIFY_STATUS", Type.GetType("System.String"))
        dtData.Columns.Add("IT_INVENTORY_NAME", Type.GetType("System.String"))

        dtDetails.Columns.Add("ID_INVENTORY_INDEX", Type.GetType("System.String"))
        dtDetails.Columns.Add("ID_LOCATION_INDEX", Type.GetType("System.String"))
        dtDetails.Columns.Add("ID_INVENTORY_QTY", Type.GetType("System.String"))

        Dim dgItem As DataGridItem
        For Each dgItem In dtgInv.Items
            Dim txtPass As TextBox
            Dim txtFail As TextBox
            Dim intPassQty As Decimal 'Integer
            Dim intFailQty As Decimal 'Integer
            Dim intReceiveQty As Decimal 'Integer
            Dim intTotalQty As Decimal 'Integer
            Dim intInvQty As Decimal 'Integer

            txtPass = dgItem.FindControl("txtPassQty")
            txtFail = dgItem.FindControl("txtFailQty")
            intPassQty = Val(txtPass.Text)
            intFailQty = Val(txtFail.Text)
            intReceiveQty = Val(dgItem.Cells(4).Text)

            intTotalQty = Val(dgItem.Cells(5).Text) + Val(dgItem.Cells(6).Text) + intPassQty + intFailQty
            If intReceiveQty = intTotalQty Then
                dtr = dtMstr.NewRow()
                dtr("IV_INVENTORY_INDEX") = dgItem.Cells(9).Text
                dtr("IV_LOCATION_INDEX") = dgItem.Cells(10).Text
                dtr("IV_VERIFY_INDEX") = dgItem.Cells(11).Text
                dtr("IV_VERIFY_STATUS") = "F"
                dtMstr.Rows.Add(dtr)
            End If

            If intPassQty > 0 Then
                dtr = dtData.NewRow()
                dtr("IT_INVENTORY_INDEX") = dgItem.Cells(9).Text
                dtr("IT_TRANS_TYPE") = "VP"
                dtr("IT_TRANS_REF_NO") = Request.QueryString("GRNNo")
                dtr("IT_TRANS_QTY") = intPassQty
                dtr("IT_TO_LOCATION_INDEX") = dgItem.Cells(10).Text
                dtr("IT_REF_NO") = dgItem.Cells(11).Text
                dtr("IT_INVENTORY_NAME") = dgItem.Cells(1).Text
                dtData.Rows.Add(dtr)
            End If

            If intFailQty > 0 Then
                dtr = dtData.NewRow()
                dtr("IT_INVENTORY_INDEX") = dgItem.Cells(9).Text
                dtr("IT_TRANS_TYPE") = "VF"
                dtr("IT_TRANS_REF_NO") = Request.QueryString("GRNNo")
                dtr("IT_TRANS_QTY") = intFailQty
                dtr("IT_TO_LOCATION_INDEX") = dgItem.Cells(10).Text
                dtr("IT_REF_NO") = dgItem.Cells(11).Text
                dtr("IT_INVENTORY_NAME") = dgItem.Cells(1).Text
                dtData.Rows.Add(dtr)
            End If

            intInvQty = intPassQty + intFailQty
            If intInvQty > 0 Then
                dtr = dtDetails.NewRow()
                dtr("ID_INVENTORY_INDEX") = dgItem.Cells(9).Text
                dtr("ID_LOCATION_INDEX") = dgItem.Cells(10).Text
                dtr("ID_INVENTORY_QTY") = intInvQty
                dtDetails.Rows.Add(dtr)
            End If
        Next
        ds.Tables.Add(dtMstr)
        ds.Tables.Add(dtData)
        ds.Tables.Add(dtDetails)

        BindData = ds
    End Function

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_InvList_tabs") = "<div class=""t_entity""><ul>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "InventoryVerification.aspx", "pageid=" & strPageId) & """><span>Status Update</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "InventoryVerificationListing.aspx", "pageid=" & strPageId) & """><span>Status Listing</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                            "</ul><div></div></div>"
        'Session("w_SearchGInv_tabs") = "<div class=""t_entity"">" & _
        '            "<a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Invoice", "InvList.aspx", "pageid=" & strPageId) & """><span>Issue Invoice</span></a>" & _
        '            "<a class=""t_entity_btn"" href=""" & dDispatcher.direct("Invoice", "invoiceView.aspx", "pageid=" & strPageId) & """><span>Invoice Listing</span></a>" & _
        '            "</div>"
    End Sub

    Private Sub cmd_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Save.Click
        Dim objInv As New Inventory
        Dim objGlobal As New AppGlobals
        Dim strMsg As String
        Dim strDirect As String

        Dim dsInv As DataSet
        lblMsg.Text = ""
        If validateDatagrid(strMsg) And validateQty() Then
            dsInv = BindData(True)
            If dsInv.Tables(0).Rows.Count > 0 Or dsInv.Tables(1).Rows.Count > 0 Or dsInv.Tables(2).Rows.Count > 0 Then
                If objInv.SaveInventoryVerify(dsInv) = True Then
                    BindInv(True)
                    'objInv.GetLocationInfo(strLoc, strSLoc, intLocIndicator)
                    'If intLocIndicator > 1 Then 'Sub Location is defined
                    '    Me.dtgInv.Columns(3).Visible = True
                    'Else
                    '    Me.dtgInv.Columns(3).Visible = False
                    'End If
                    ChangeHeaderText()
                    lblMsg.Text = ""
                    If strFrm = "InventoryVerification" Then
                        strDirect = dDispatcher.direct("Inventory", "InventoryVerification.aspx", "pageid=" & strPageId)
                    ElseIf strFrm = "Dashboard" Then
                        strDirect = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=0")
                    End If
                    If ViewState("intPageRecordCnt") > 0 Then
                        Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00003"), MsgBoxStyle.Information)

                    Else
                        Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00003"), strDirect, MsgBoxStyle.Information)

                    End If

                Else
                    Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00007"), MsgBoxStyle.Information)
                End If
            Else
                Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00003"), MsgBoxStyle.Information)
            End If

        Else
            If strMsg <> "" Then
                lblMsg.Text = strMsg
            Else
                lblMsg.Text = ""
            End If
        End If
        objInv = Nothing

    End Sub

    Private Function validateDatagrid(ByRef strMsg As String) As Boolean
        validateDatagrid = True
        Dim dgItem As DataGridItem
        Dim txtQ As TextBox
        Dim intTotalQty As Decimal 'Integer
        Dim intReceiveQty As Decimal 'Integer
        Dim intPassQty As Decimal 'Integer
        Dim intFailQty As Decimal 'Integer
        Dim objGlobal As New AppGlobals

        strMsg = "<ul type='disc'>"
        For Each dgItem In dtgInv.Items
            Dim txtPass As TextBox
            Dim txtFail As TextBox

            txtPass = dgItem.FindControl("txtPassQty")
            txtFail = dgItem.FindControl("txtFailQty")
            intPassQty = Val(txtPass.Text)
            intFailQty = Val(txtFail.Text)
            intReceiveQty = CDec(dgItem.Cells(4).Text)

            txtQ = dgItem.FindControl("txtQ")

            intTotalQty = Val(dgItem.Cells(5).Text) + Val(dgItem.Cells(6).Text) + intPassQty + intFailQty
            If intTotalQty > intReceiveQty Then
                validateDatagrid = False
                strMsg &= "<li>" & objGlobal.GetErrorMessage("00016") & " " & Me.dtgInv.Columns(7).HeaderText & " and " & Me.dtgInv.Columns(8).HeaderText & " " & objGlobal.GetErrorMessage("00015") & " " & Me.dtgInv.Columns(4).HeaderText & "<ul type='disc'></ul></li>"
                'Total of Pass Qty and Fail Qty must be less than or equal to Receive Qty.<ul type='disc'></ul></li>"
                txtQ.Text = "?"
                'Dim hidCode As HtmlInputHidden
                'hidCode = dgItem.FindControl("hidCode")
                'hidCode.Value = dgItem.Cells(1).Text & " (" & dgItem.Cells(2).Text
                'If dgItem.Cells(3).Text <> "" Then
                '    hidCode.Value = hidCode.Value & "," & hidCode.Value & dgItem.Cells(3).Text & ")"
                'Else
                '    hidCode.Value = hidCode.Value & ")"
                'End If

                'If hidControl.Value = "" Then
                '    hidControl.Value = hidCode.ClientID & "-" & txtQ.ClientID
                'Else
                '    hidControl.Value &= "," & hidCode.ClientID & "-" & txtQ.ClientID
                'End If
            End If
        Next
        strMsg &= "</ul>"
        objGlobal = Nothing
    End Function

    Private Function validateQty() As Boolean
        validateQty = True
        Dim dgItem As DataGridItem
        Dim i As Integer

        For Each dgItem In dtgInv.Items
            If CType(dgItem.FindControl("txtPassQty"), TextBox).Text = "" And _
                CType(dgItem.FindControl("txtFailQty"), TextBox).Text = "" Then
                i = i + 1
            End If
        Next

        If dtgInv.Items.Count = i Then
            validateQty = False
            Common.NetMsgbox(Me, "Please enter at least one item.", MsgBoxStyle.Information)

            Exit Function
        End If

    End Function

    Private Sub InventoryVerificationDetails_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.cmd_Save.Enabled = False
        If ViewState("intPageRecordCnt") > 0 Then
            Me.cmd_Save.Enabled = True

        End If
    End Sub
End Class