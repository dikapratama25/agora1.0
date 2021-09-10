Imports AgoraLegacy
Imports eProcure.Component
Public Class InventoryAdjust
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
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
        If Not Page.IsPostBack Then
            GenerateTab()
            LoadLoc()
            Me.ddl_SubLoc.Items.Insert(0, "---Select---")
            ChangeHeaderText()
        End If
       
    End Sub

    Private Sub LoadLoc()
        Dim objInv As New Inventory
        Dim ds As New DataSet

        ds = objInv.PopLocation

        Me.ddl_Loc.Items.Clear()
        Me.ddl_Loc.DataSource = ds.Tables(0)
        Me.ddl_Loc.DataTextField = "LM_LOCATION"
        Me.ddl_Loc.DataBind()
        Me.ddl_Loc.Items.Insert(0, "---Select---")

        objInv = Nothing

    End Sub

    Private Sub ChangeHeaderText()
        Dim objInv As New Inventory

        objInv.GetLocationInfo(strLoc, strSLoc, intLocIndicator)

        dtgInv.Columns(2).HeaderText = strLoc
        dtgInv.Columns(3).HeaderText = strSLoc
        Me.lblLoc.Text = strLoc
        Me.lblSubLoc.Text = strSLoc

        If intLocIndicator > 1 Then 'Sub Location is defined
            Me.dtgInv.Columns(3).Visible = True
            lblSubLoc.Visible = True
            Me.ddl_SubLoc.Visible = True
        Else
            Me.dtgInv.Columns(3).Visible = False
            lblSubLoc.Visible = False
            Me.ddl_SubLoc.Visible = False
        End If
        objInv = Nothing

    End Sub

    Private Sub BindInv(Optional ByVal pSorted As Boolean = False)
        Dim objInv As New Inventory
        Dim ds As DataSet
        Dim dvViewInv As DataView

        ds = objInv.GetInvDetails(Me.txtItemCode.Text, Me.txtItemName.Text, Me.ddl_Loc.SelectedItem.Text, Me.ddl_SubLoc.SelectedItem.Text)
        dvViewInv = ds.Tables(0).DefaultView
        dvViewInv.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewInv.Sort += " DESC"

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgInv, dvViewInv)
            dtgInv.DataSource = dvViewInv
            dtgInv.DataBind()
        Else
            dtgInv.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ViewState("PageCount") = dtgInv.PageCount
        objInv = Nothing

    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_Adj_tabs") = "<div class=""t_entity""><ul>" & _
                            "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "InventoryAdjust.aspx", "pageid=" & strPageId) & """><span>Adjustment</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "InventoryAdjustList.aspx", "pageid=" & strPageId) & """><span>Adjustment Listing</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                            "</ul><div></div></div>"
    
    End Sub

    Private Sub dtgInv_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInv.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgInv, e)
        ChangeHeaderText()
    End Sub

    Private Sub dtgInv_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgInv.ItemDataBound
        Dim objInv As New Inventory
        Dim objGlobal As New AppGlobals
        Dim intPhysicalQty As Integer

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim txtPhysicalQty As TextBox

            txtPhysicalQty = e.Item.FindControl("txtPhysicalQty")

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
            Dim revPhysicalQty As RegularExpressionValidator
            revPhysicalQty = e.Item.FindControl("revPhysicalQty")
            revPhysicalQty.ValidationExpression = "^\d+$"
            revPhysicalQty.ControlToValidate = "txtPhysicalQty"
            revPhysicalQty.ErrorMessage = hidCode.Value & " - " & Me.dtgInv.Columns(5).HeaderText & " " & objGlobal.GetErrorMessage("00013")
            revPhysicalQty.Text = "?"
            revPhysicalQty.Display = ValidatorDisplay.Dynamic

            Dim reqVal_Qty As RequiredFieldValidator
            reqVal_Qty = e.Item.FindControl("reqVal_Qty")
            reqVal_Qty.ControlToValidate = "txtPhysicalQty"
            reqVal_Qty.ErrorMessage = hidCode.Value & " - " & Me.dtgInv.Columns(5).HeaderText & " " & objGlobal.GetErrorMessage("00001")
            reqVal_Qty.Text = "?"
            reqVal_Qty.Display = ValidatorDisplay.Dynamic

            Dim reqRemarks As RequiredFieldValidator
            reqRemarks = e.Item.FindControl("reqRemarks")
            reqRemarks.ControlToValidate = "txtRemarks"
            reqRemarks.ErrorMessage = hidCode.Value & " - " & Me.dtgInv.Columns(6).HeaderText & " " & objGlobal.GetErrorMessage("00001")
            reqRemarks.Text = "?"
            reqRemarks.Display = ValidatorDisplay.Dynamic

            Dim txtRemark As TextBox
            txtRemark = e.Item.FindControl("txtRemarks")
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
        End If

        objInv = Nothing
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

    Private Function ValidateDataGrid(ByRef strMsg As String) As Boolean
        Dim objGlobal As New AppGlobals
        Dim dgItem As DataGridItem
        Dim txtRemark As TextBox
        Dim txtQ As TextBox

        ValidateDataGrid = True
        strMsg = "<ul type='disc'>"
        For Each dgItem In dtgInv.Items
            txtRemark = dgItem.FindControl("txtRemarks")
            txtQ = dgItem.FindControl("txtQ")

            'If CType(dgItem.FindControl("txtPhysicalQty"), TextBox).Text = "" Then
            '    ValidateDataGrid = False
            '    Common.NetMsgbox(Me, Me.dtgInv.Columns(5).HeaderText & objGlobal.GetErrorMessage("00001"), MsgBoxStyle.Information)
            '    Exit Function
            'End If
            'If CType(dgItem.FindControl("txtRemarks"), TextBox).Text = "" Then
            '    ValidateDataGrid = False
            '    Common.NetMsgbox(Me, Me.dtgInv.Columns(6).HeaderText & " " & objGlobal.GetErrorMessage("00001"), MsgBoxStyle.Information)
            'End If
            If Not Common.checkMaxLength(txtRemark.Text, 400) Then
                strMsg &= "<li>Remarks is over limit.<ul type='disc'></ul></li>"
                txtQ.Text = "?"
                ValidateDataGrid = False
            Else
                txtQ.Text = ""
            End If
        Next
        strMsg &= "</ul>"
        objGlobal = Nothing

    End Function

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "IM_ITEM_CODE"
        dtgInv.CurrentPageIndex = 0
        BindInv(True)
        ChangeHeaderText()
    End Sub

    Private Sub InventoryAdjust_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.cmd_Save.Enabled = False
        If ViewState("intPageRecordCnt") > 0 Then
            Me.cmd_Save.Enabled = True
        End If
    End Sub

    Private Sub cmd_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Save.Click
        Dim objInv As New Inventory
        Dim objGlobal As New AppGlobals
        Dim strMsg As String

        Dim dsInv As DataSet
        lblMsg.Text = ""
        If ValidateDataGrid(strMsg) Then
            dsInv = BindData(True)
            If dsInv.Tables(0).Rows.Count > 0 Or dsInv.Tables(1).Rows.Count > 0 Then
                If objInv.SaveInventoryAdjustment(dsInv) = True Then
                    Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00003"), MsgBoxStyle.Information)
                    BindInv(True)
                    ChangeHeaderText()
                    lblMsg.Text = ""
                Else
                    Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00007"), MsgBoxStyle.Information)
                End If
            Else

            End If

        Else
            If strMsg <> "" Then
                lblMsg.Text = strMsg
            Else
                lblMsg.Text = ""
            End If
        End If
        objInv = Nothing
        objGlobal = Nothing

    End Sub

    Private Function BindData(ByRef pass As Boolean) As DataSet
        Dim ds As New DataSet
        Dim dtData As New DataTable
        Dim dtMstr As New DataTable
        Dim dtr As DataRow

        dtMstr.Columns.Add("ID_INVENTORY_INDEX", Type.GetType("System.String"))
        dtMstr.Columns.Add("ID_LOCATION_INDEX", Type.GetType("System.String"))
        dtMstr.Columns.Add("ID_INVENTORY_QTY", Type.GetType("System.String"))

        dtData.Columns.Add("IT_INVENTORY_INDEX", Type.GetType("System.String"))
        dtData.Columns.Add("IT_TRANS_TYPE", Type.GetType("System.String"))
        dtData.Columns.Add("IT_TRANS_QTY", Type.GetType("System.String"))
        dtData.Columns.Add("IT_TO_LOCATION_INDEX", Type.GetType("System.String"))
        dtData.Columns.Add("IT_ADDITION_INFO", Type.GetType("System.String"))
        dtData.Columns.Add("IT_REMARK", Type.GetType("System.String"))
        dtData.Columns.Add("IT_INVENTORY_NAME", Type.GetType("System.String"))

        Dim dgItem As DataGridItem
        For Each dgItem In dtgInv.Items
            Dim txtPass As TextBox

            txtPass = dgItem.FindControl("txtPhysicalQty")

            dtr = dtMstr.NewRow()
            dtr("ID_INVENTORY_INDEX") = dgItem.Cells(7).Text
            dtr("ID_LOCATION_INDEX") = dgItem.Cells(8).Text
            dtr("ID_INVENTORY_QTY") = CInt(Val(txtPass.Text))
            dtMstr.Rows.Add(dtr)

            dtr = dtData.NewRow()
            dtr("IT_INVENTORY_INDEX") = dgItem.Cells(7).Text
            dtr("IT_TRANS_TYPE") = "ADJ"
            dtr("IT_TRANS_QTY") = CInt(Val(txtPass.Text))
            dtr("IT_TO_LOCATION_INDEX") = dgItem.Cells(8).Text
            dtr("IT_ADDITION_INFO") = CInt(Val(dgItem.Cells(4).Text))
            Dim txtRemark As TextBox
            txtRemark = dgItem.FindControl("txtRemarks")
            dtr("IT_REMARK") = txtRemark.Text
            dtr("IT_INVENTORY_NAME") = dgItem.Cells(1).Text
            dtData.Rows.Add(dtr)
        Next
        ds.Tables.Add(dtMstr)
        ds.Tables.Add(dtData)

        BindData = ds
    End Function

    Private Sub ddl_Loc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Loc.SelectedIndexChanged
        Dim objInv As New Inventory
        Dim ds As New DataSet

        ds = objInv.PopLocation(Me.ddl_Loc.SelectedItem.Text)

        Me.ddl_SubLoc.Items.Clear()
        Me.ddl_SubLoc.DataSource = ds.Tables(0)
        Me.ddl_SubLoc.DataTextField = "LM_SUB_LOCATION"
        Me.ddl_SubLoc.DataValueField = "LM_LOCATION_INDEX"
        Me.ddl_SubLoc.DataBind()
        Me.ddl_SubLoc.Items.Insert(0, "---Select---")

        objInv = Nothing
    End Sub
End Class