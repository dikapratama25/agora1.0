Imports AgoraLegacy
Imports eProcure.Component


Public Class ReturnOutward
    Inherits AgoraLegacy.AppBaseClass

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strFrm, strGRNNo As String
    Dim objINV As New Inventory
    Dim intRow As Integer
    Dim dsAllInfo, ds As DataSet
    Dim objGlobal As New AppGlobals
    Dim objDB As New EAD.DBCom

    Public Enum EnumRO
        icLine
        icItemCode
        icItemName
        icUOM
        icMPQ
        icOrderQty
        icReceivedQty
        icRejectedQty
        icReturnedQty
        icRemainingQty
        icIQCQty
        icRejIQCQty
        icReturnQty
        icRemarks
        icPOLine
        icQC
        icPrevRemainingQty
    End Enum

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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdSubmit.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdSubmit)
        alButtonList.Add(cmdReset)
        htPageAccess.Add("update", alButtonList)
        htPageAccess.Add("add", alButtonList)
        If Request.QueryString("frm") <> "Dashboard" Then
            CheckButtonAccess()
        End If
        cmdSubmit.Enabled = True
        cmdReset.Enabled = True
        If Not ViewState("blnButtonState") Then
            ButtonProperty(False)
        End If
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnPaging = False
        SetGridProperty(dtgGRNDtl)

        strPageId = Session("strPageId")
        strGRNNo = Me.Request.QueryString("GRNNo")
        strFrm = Me.Request.QueryString("frm")
        If strFrm = "ROListing" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "ReturnOutwardListing.aspx", "pageid=" & strPageId)
        ElseIf strFrm = "Dashboard" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "frm=Dashboard&pageid=0")
        End If
        lblGRNNo.Text = strGRNNo

        If Not Page.IsPostBack Then
            Session("arySetROLot") = Nothing
            dsAllInfo = objINV.GRNInfo(lblGRNNo.Text)
            PopulateGRNHeader()
            GenerateTab()
            Bindgrid(True)
        End If
    End Sub

    Sub ButtonProperty(ByVal blnEnable As Boolean)
        ViewState("blnButtonState") = blnEnable
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgGRNDtl.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgGRNDtl_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGRNDtl.ItemCreated
        Grid_ItemCreated(dtgGRNDtl, e)
    End Sub

    Private Sub dtgGRNDtl_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGRNDtl.ItemDataBound
        Dim dtgGRNDtl As DataSet
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim iqcQty, iqcRejQty As Decimal
            Dim lblQty As Label
            lblQty = e.Item.FindControl("lblQty")
            lblQty.Text = "0.00"

            'Dim rev_qtyreturn As RegularExpressionValidator
            'rev_qtyreturn = e.Item.FindControl("rev_qtyreturn")
            'rev_qtyreturn.ControlToValidate = "txtReturn"
            'rev_qtyreturn.ValidationExpression = "^\d{1,6}(?:\.(?:\d{0,1})?\d)?$" '"^\d+$"
            'rev_qtyreturn.ErrorMessage = "Expecting numeric value."
            'rev_qtyreturn.Display = ValidatorDisplay.None
            'rev_qtyreturn.EnableClientScript = "False"

            Dim txtQ As TextBox
            txtQ = e.Item.FindControl("txtQ")

            Dim btn_lot As Button
            btn_lot = e.Item.FindControl("btn_lot")

            Dim txtRemark As TextBox
            txtRemark = e.Item.FindControl("txtDtlRemarks")
            txtRemark.Attributes.Add("onKeyDown", "limitText (this, 400);")

            e.Item.Cells(EnumRO.icLine).Text = intRow + 1

            If IsDBNull(dv("GD_RETURN_QTY")) Then
                e.Item.Cells(EnumRO.icReturnedQty).Text = "0.00"
            Else
                e.Item.Cells(EnumRO.icReturnedQty).Text = Format(dv("GD_RETURN_QTY"), "###0.00")
            End If

            If IsDBNull(dv("GD_RETURN_QTY")) Then
                e.Item.Cells(EnumRO.icRemainingQty).Text = CDec(dv("OUTSTANDING"))
                e.Item.Cells(EnumRO.icPrevRemainingQty).Text = CDec(dv("OUTSTANDING"))
            Else
                e.Item.Cells(EnumRO.icRemainingQty).Text = CDec(dv("OUTSTANDING")) - CDec(dv("GD_RETURN_QTY"))
                e.Item.Cells(EnumRO.icPrevRemainingQty).Text = CDec(dv("OUTSTANDING")) - CDec(dv("GD_RETURN_QTY"))
            End If


            e.Item.Cells(EnumRO.icQC).Text = objINV.chkIQCForGRN(strGRNNo, dv("POD_VENDOR_ITEM_CODE"))

            If e.Item.Cells(EnumRO.icQC).Text = "Y" Then
                iqcQty = 0
                iqcQty = objINV.chkIQCLotQty(strGRNNo, dv("POD_VENDOR_ITEM_CODE"), dv("GD_PO_LINE"))
                e.Item.Cells(EnumRO.icIQCQty).Text = iqcQty

                iqcRejQty = 0
                iqcRejQty = objINV.chkIQCLotQty(strGRNNo, dv("POD_VENDOR_ITEM_CODE"), dv("GD_PO_LINE"), , , , "Rej")
                e.Item.Cells(EnumRO.icRejIQCQty).Text = iqcRejQty

                If CDec(e.Item.Cells(EnumRO.icRemainingQty).Text) = 0 Or (iqcQty = 0 And iqcRejQty = 0) Then
                    btn_lot.Enabled = False
                End If
            Else
                e.Item.Cells(EnumRO.icIQCQty).Text = "N/A"
                e.Item.Cells(EnumRO.icRejIQCQty).Text = "N/A"

                If CDec(e.Item.Cells(EnumRO.icRemainingQty).Text) = 0 Then
                    btn_lot.Enabled = False
                End If
            End If

            

            'Dim cboLotNo As DropDownList
            'cboLotNo = e.Item.FindControl("cboLotNo")
            'objGlobal.FillLot(cboLotNo, e.Item.Cells(EnumRO.icItemCode).Text, lblGRNNo.Text, lblPONumber.Text, e.Item.Cells(EnumRO.icLine).Text)


            'Dim strSql As String
            'strSql = " SELECT GROUP_CONCAT(DISTINCT(ZZZ.LOC)) AS LOC FROM (SELECT CONCAT(LM_LOCATION, ':', LM_SUB_LOCATION) AS LOC FROM GRN_MSTR "
            'strSql &= " INNER JOIN GRN_DETAILS ON GM_GRN_NO = GD_GRN_NO AND GD_B_COY_ID = GM_B_COY_ID "
            'strSql &= " INNER JOIN GRN_LOT ON GL_GRN_NO = GD_GRN_NO AND GL_B_COY_ID = GD_B_COY_ID AND GL_PO_LINE = GD_PO_LINE "
            'strSql &= " INNER JOIN PO_MSTR ON GM_PO_INDEX = POM_PO_INDEX AND GM_B_COY_ID = POM_B_COY_ID "
            'strSql &= " INNER JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID = POD_COY_ID "
            'strSql &= " INNER JOIN DO_MSTR ON DOM_DO_INDEX = GM_DO_INDEX "
            'strSql &= " INNER JOIN DO_DETAILS ON DOD_S_COY_ID = DOM_S_COY_ID AND DOD_DO_NO = DOM_DO_NO AND DOD_PO_LINE = GL_PO_LINE "
            'strSql &= " INNER JOIN DO_LOT ON DOL_COY_ID = DOM_S_COY_ID AND DOL_DO_NO = DOD_DO_NO AND DOL_LOT_INDEX = GL_LOT_INDEX "
            'strSql &= " INNER JOIN LOCATION_MSTR ON LM_COY_ID = GL_B_COY_ID AND LM_LOCATION_INDEX = GL_LOCATION_INDEX "
            'strSql &= " WHERE GD_GRN_NO = '" & lblGRNNo.Text & "' AND POM_PO_NO = '" & lblPONumber.Text & "' "
            'strSql &= " AND POD_VENDOR_ITEM_CODE = '" & e.Item.Cells(EnumRO.icItemCode).Text & "') ZZZ "

            'Dim strLocation As String = objDB.GetVal(strSql)

            'Dim lblLocation As Label
            'lblLocation = e.Item.FindControl("lblLocation")
            'lblLocation.Text = strLocation

            intRow = intRow + 1

            ' '' ''Dim lblItemLine, lblInvIndex, lblLocIndex As Label
            ' '' ''lblItemLine = e.Item.FindControl("lblItemLine")
            ' '' ''lblInvIndex = e.Item.FindControl("lblInvIndex")
            ' '' ''lblLocIndex = e.Item.FindControl("lblLocIndex")

            ' '' ''lblItemLine.Text = Common.parseNull(dv("IRSD_IRS_LINE"))
            ' '' ''lblInvIndex.Text = Common.parseNull(dv("IRSD_INVENTORY_INDEX"))
            ' '' ''lblLocIndex.Text = Common.parseNull(dv("LM_LOCATION_INDEX"))
        End If
    End Sub

    Public Sub dtgGRNDtl_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgGRNDtl.ItemCommand
        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        Dim strName As String
        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

        If (e.CommandSource).CommandName = "" Then
            strName = "rqty=" & e.Item.Cells(EnumRO.icPrevRemainingQty).Text & "&grnno=" & lblGRNNo.Text & "&pono=" & lblPONumber.Text & "&poline=" & e.Item.Cells(EnumRO.icPOLine).Text & "&qc=" & e.Item.Cells(EnumRO.icQC).Text & "&itemline=" & e.Item.Cells(EnumRO.icLine).Text & "&itemcode=" & Server.UrlEncode(e.Item.Cells(EnumRO.icItemCode).Text) & "&itemname=" & Server.UrlEncode(e.Item.Cells(EnumRO.icItemName).Text) & ""

            strscript.Append("<script language=""javascript"">")
            strFileName = dDispatcher.direct("Inventory", "ROLotMaster.aspx", strName)
            strFileName = Server.UrlEncode(strFileName)
            strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','400px');")
            strscript.Append("document.getElementById('btnhidden2').click();")
            strscript.Append("</script>")
            RegisterStartupScript("script3", strscript.ToString())
        End If
    End Sub

    Private Sub PopulateGRNHeader()
        Dim ds As DataSet

        Dim dtHeader As New DataTable
        dtHeader = dsAllInfo.Tables("GRN_MSTR")

        If dtHeader.Rows.Count > 0 Then
            lblGRNNo.Text = dtHeader.Rows(0)("GM_GRN_NO")
            lblGRNDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dtHeader.Rows(0)("GM_CREATED_DATE"))
            lblVendor.Text = dtHeader.Rows(0)("CM_COY_NAME")
            lblPONumber.Text = dtHeader.Rows(0)("POM_PO_NO")
            lblPOCreationDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dtHeader.Rows(0)("POM_CREATED_DATE"))
            lblDONo.Text = dtHeader.Rows(0)("DOM_DO_NO")
            lblDODate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dtHeader.Rows(0)("DOM_DO_DATE"))
            lblActualGoodRecDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dtHeader.Rows(0)("GM_DATE_RECEIVED"))
        End If
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        ''//Retrieve Data from Database
        ''Dim strMsg As String
        Dim objGlobal As New AppGlobals
        Dim ds As New DataSet
        Dim objInventory As New Inventory
        intRow = 0
        Dim dvViewGRN As DataView
        dvViewGRN = dsAllInfo.Tables("GRN_DETAILS").DefaultView

        dvViewGRN.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewGRN.Sort += " DESC"

        ViewState("intPageRecordCnt") = dsAllInfo.Tables("GRN_DETAILS").Rows.Count

        If ViewState("intPageRecordCnt") > 0 Then
            dtgGRNDtl.DataSource = dvViewGRN
            dtgGRNDtl.DataBind()
        Else
            dtgGRNDtl.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtgGRNDtl.PageCount
    End Function

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_ReturnInward_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "ReturnOutwardListing.aspx", "pageid=" & strPageId) & """><span>Return Outward</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "ReturnOutwardSearch.aspx", "pageid=" & strPageId) & """><span>Return Outward Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
    End Sub

    Private Function validateDatagrid(ByRef strMsg As String) As Boolean
        validateDatagrid = True
        strMsg = "<ul type='disc'>"
        Dim txtQ As TextBox
        Dim txtRemark As TextBox
        Dim lblQty As Label
        Dim dgItem As DataGridItem
        Dim blnReturnQty As Boolean = False
        Dim aryLot As New ArrayList
        Dim i As Integer
        Dim dsQty As DataSet
        Dim decStkQty, decIQCQty, decAppIQCQty, decRejIQCQty As Decimal
        aryLot = Session("arySetROLot")
        For Each dgItem In dtgGRNDtl.Items

            txtRemark = dgItem.FindControl("txtDtlRemarks")
            txtQ = dgItem.FindControl("txtQ")

            If Not Common.checkMaxLength(txtRemark.Text, 400) Then
                strMsg &= "<li>Remarks is over limit.<ul type='disc'></ul></li>"
                txtQ.Text = "?"
                validateDatagrid = False
            Else
                txtQ.Text = ""
            End If

            lblQty = dgItem.FindControl("lblQty")

            If CDec(lblQty.Text) > 0 Then
                blnReturnQty = True

                For i = 0 To aryLot.Count - 1
                    If aryLot(i)(0) <> "---Select---" And aryLot(i)(1) <> "" And aryLot(i)(4) = dgItem.Cells(EnumRO.icItemCode).Text And aryLot(i)(5) = dgItem.Cells(EnumRO.icLine).Text Then
                        dsQty = objINV.PopLotInfoByRO(strGRNNo, lblPONumber.Text, dgItem.Cells(EnumRO.icPOLine).Text, dgItem.Cells(EnumRO.icItemCode).Text, aryLot(i)(0), aryLot(i)(2), aryLot(i)(3))
                        If CDec(IIf(aryLot(i)(1) = "", 0, aryLot(i)(1))) > CDec(dsQty.Tables(0).Rows(0)("GL_LOT_RECEIVED_QTY")) Then
                            strMsg &= "<li>Return qty cannot be more than remaining qty.<ul type='disc'></ul></li>"
                            validateDatagrid = False
                        End If

                        decStkQty = 0
                        decStkQty = objINV.getLotBalance(dgItem.Cells(EnumRO.icItemCode).Text, aryLot(i)(0), aryLot(i)(2), aryLot(i)(3), True)
                        If CDec(IIf(aryLot(i)(1) = "", 0, aryLot(i)(1))) > decStkQty Then
                            strMsg &= "<li>Return qty cannot be more than stock balance.<ul type='disc'></ul></li>"
                            validateDatagrid = False
                        End If

                        If dgItem.Cells(EnumRO.icQC).Text = "Y" Then
                            decAppIQCQty = 0
                            decAppIQCQty = objINV.chkIQCLotQty(strGRNNo, dgItem.Cells(EnumRO.icItemCode).Text, , aryLot(i)(0), aryLot(i)(2), aryLot(i)(3), , True)

                            decRejIQCQty = 0
                            decRejIQCQty = objINV.chkIQCLotQty(strGRNNo, dgItem.Cells(EnumRO.icItemCode).Text, , aryLot(i)(0), aryLot(i)(2), aryLot(i)(3), "Rej", True)

                            If decAppIQCQty > 0 Then
                                If CDec(IIf(aryLot(i)(1) = "", 0, aryLot(i)(1))) > decAppIQCQty Then
                                    strMsg &= "<li>Return qty cannot be more than Approved IQC qty.<ul type='disc'></ul></li>"
                                    validateDatagrid = False
                                End If
                            Else
                                If decRejIQCQty > 0 Then
                                    If CDec(IIf(aryLot(i)(1) = "", 0, aryLot(i)(1))) > decRejIQCQty Then
                                        strMsg &= "<li>Return qty cannot be more than Rejected IQC qty.<ul type='disc'></ul></li>"
                                        validateDatagrid = False
                                    End If
                                Else
                                    If CDec(IIf(aryLot(i)(1) = "", 0, aryLot(i)(1))) > decAppIQCQty Then
                                        strMsg &= "<li>Return qty cannot be more than Approved IQC qty.<ul type='disc'></ul></li>"
                                        validateDatagrid = False
                                    End If
                                End If       
                            End If
                        End If
                    End If
                Next

            End If
        Next

        If blnReturnQty = False Then
            strMsg &= "<li>" & "" & "There must be at least one return quantity.<ul type='disc'></ul></li>"
            validateDatagrid = False
        End If

        strMsg &= "</ul>"

    End Function

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Dim dgItem As DataGridItem

        lbl_check.Text = ""
        For Each dgItem In dtgGRNDtl.Items
            'Dim txtReturn As TextBox
            Dim txtRemark As TextBox

            'txtReturn = dgItem.FindControl("txtReturn")
            'txtReturn.Text = "0.00"

            txtRemark = dgItem.FindControl("txtDtlRemarks")
            txtRemark.Text = ""
        Next
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim ds As New DataSet
        Dim strMsg As String = ""
        Dim strNewRONo As String = ""
        lbl_check.Text = ""

        If Page.IsValid And validateDatagrid(strMsg) Then
            BindRO(ds)

            If objINV.ROSubmit(ds, strNewRONo, Session("arySetROLot")) = True Then
                Common.NetMsgbox(Me, "RO Number " & strNewRONo & " has been successfully submitted", dDispatcher.direct("Inventory", "ReturnOutwardListing.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
            Else
                Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
            End If
        Else
            If strMsg <> "" Then
                lbl_check.Text = strMsg
            Else
                lbl_check.Text = ""
            End If
        End If
    End Sub

    Private Sub BindRO(ByRef ds As DataSet)
        Dim dtROMstr As New DataTable
        dtROMstr.Columns.Add("IROM_GRN_NO", Type.GetType("System.String"))
        dtROMstr.Columns.Add("IROM_PO_NO", Type.GetType("System.String"))

        Dim dtr As DataRow
        dtr = dtROMstr.NewRow

        dtr("IROM_GRN_NO") = lblGRNNo.Text
        dtr("IROM_PO_NO") = lblPONumber.Text

        dtROMstr.Rows.Add(dtr)
        ds.Tables.Add(dtROMstr)

        Dim dgItem As DataGridItem
        Dim dtrd As DataRow
        Dim i As Integer = 0
        Dim dtRODtls As New DataTable

        dtRODtls.Columns.Add("IROD_RO_LINE", Type.GetType("System.String"))
        dtRODtls.Columns.Add("IROD_LINE", Type.GetType("System.String"))
        dtRODtls.Columns.Add("IROD_PO_LINE", Type.GetType("System.String"))
        dtRODtls.Columns.Add("IROD_ITEM_CODE", Type.GetType("System.String"))
        dtRODtls.Columns.Add("IROD_INVENTORY_NAME", Type.GetType("System.String"))
        dtRODtls.Columns.Add("IROD_QTY", Type.GetType("System.Double"))
        dtRODtls.Columns.Add("IROD_REMARK", Type.GetType("System.String"))

        For Each dgItem In dtgGRNDtl.Items
            dtr = dtRODtls.NewRow
            Dim txtRemark As TextBox
            Dim lblQty As Label

            lblQty = dgItem.FindControl("lblQty")
            txtRemark = dgItem.FindControl("txtDtlRemarks")

            If CDec(lblQty.Text) > 0 Then
                dtr("IROD_RO_LINE") = i + 1
                dtr("IROD_LINE") = dgItem.Cells(EnumRO.icLine).Text
                dtr("IROD_PO_LINE") = dgItem.Cells(EnumRO.icPOLine).Text
                dtr("IROD_ITEM_CODE") = dgItem.Cells(EnumRO.icItemCode).Text
                dtr("IROD_INVENTORY_NAME") = dgItem.Cells(EnumRO.icItemName).Text
                dtr("IROD_QTY") = lblQty.Text
                dtr("IROD_REMARK") = txtRemark.Text

                dtRODtls.Rows.Add(dtr)
                i = i + 1
            End If
        Next
        ds.Tables.Add(dtRODtls)

    End Sub

    Private Sub btnhidden2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhidden2.Click
        Dim dgItem As DataGridItem
        Dim aryROLot As New ArrayList
        Dim i, j As Integer
        Dim ifound As Integer
        Dim decQty As Decimal
        aryROLot = Session("arySetROLot")

        If Not Session("arySetROLot") Is Nothing Then
            For Each dgItem In dtgGRNDtl.Items
                decQty = 0
                For i = 0 To aryROLot.Count - 1
                    If dgItem.Cells(EnumRO.icLine).Text = aryROLot(i)(5) And dgItem.Cells(EnumRO.icItemCode).Text = aryROLot(i)(4) And aryROLot(i)(0) <> "---Select---" And aryROLot(i)(1) <> "" Then

                        decQty = decQty + CDec(aryROLot(i)(1))
                    End If
                Next

                CType(dgItem.FindControl("lblQty"), Label).Text = Format(decQty, "###0.00")
                dgItem.Cells(EnumRO.icRemainingQty).Text = Format(CDec(dgItem.Cells(EnumRO.icPrevRemainingQty).Text) - decQty, "###0.00")
            Next
        End If
    End Sub
End Class
