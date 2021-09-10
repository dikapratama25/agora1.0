Imports AgoraLegacy
Imports eProcure.Component


Public Class ReturnInward
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.WebControls.Button
    Protected WithEvents dtgMRSDtl As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblMRSNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblRequestorName As System.Web.UI.WebControls.Label
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents lblMRSIssueDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblMRSDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblIssueTo As System.Web.UI.WebControls.Label
    Protected WithEvents lblSection As System.Web.UI.WebControls.Label
    Protected WithEvents lblRefNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblDept As System.Web.UI.WebControls.Label
    Protected WithEvents txtRemark As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_check As System.Web.UI.WebControls.Label

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strFrm, strMRSNo As String 
    Dim objINV As New Inventory
    Dim dsAllInfo, ds As DataSet

    Public Enum EnumRI
        icItemCode
        icItemName
        icUom
        icLotNo
        icLoc
        icSubLoc
        icQty
        icReturnedQty
        icOutQty
        icReturnQty
        icRemarks
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
        SetGridProperty(dtgMRSDtl)

        strPageId = Session("strPageId")
        strMRSNo = Me.Request.QueryString("MRS_NO")
        strFrm = Me.Request.QueryString("frm")
        If strFrm = "RIListing" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "ReturnInwardListing.aspx", "pageid=" & strPageId)
        ElseIf strFrm = "Dashboard" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "frm=Dashboard&pageid=0")
        End If
        lblMRSNo.Text = strMRSNo

        If Not Page.IsPostBack Then
            dsAllInfo = objINV.MRSInfo(lblMRSNo.Text)
            PopulateMRSHeader()
            GenerateTab()
            Bindgrid(True)
        End If
    End Sub

    Sub ButtonProperty(ByVal blnEnable As Boolean)
        ViewState("blnButtonState") = blnEnable
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgMRSDtl.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgMRSDtl_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgMRSDtl.ItemCreated
        Grid_ItemCreated(dtgMRSDtl, e)
    End Sub

    Private Sub dtgMRSDtl_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgMRSDtl.ItemDataBound
        Dim dsMRSDtl As DataSet
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim txtRejQty As TextBox
            txtRejQty = e.Item.FindControl("txtReject")
            txtRejQty.Text = "0.00"

            Dim rev_qtycancel As RegularExpressionValidator
            rev_qtycancel = e.Item.FindControl("rev_qtycancel")
            rev_qtycancel.ControlToValidate = "txtReject"
            rev_qtycancel.ValidationExpression = "^\d{1,6}(?:\.(?:\d{0,1})?\d)?$" '"^\d+$"
            rev_qtycancel.ErrorMessage = "Expecting numeric value."
            rev_qtycancel.Display = ValidatorDisplay.None
            rev_qtycancel.EnableClientScript = "False"

            Dim txtQ As TextBox
            txtQ = e.Item.FindControl("txtQ")

            Dim txtRemark As TextBox
            txtRemark = e.Item.FindControl("txtDtlRemarks")
            txtRemark.Attributes.Add("onKeyDown", "limitText (this, 400);")

            Dim lblItemLine, lblInvIndex, lblLocIndex, lblLotIndex As Label
            lblItemLine = e.Item.FindControl("lblItemLine")
            lblInvIndex = e.Item.FindControl("lblInvIndex")
            lblLocIndex = e.Item.FindControl("lblLocIndex")
            lblLotIndex = e.Item.FindControl("lblLotIndex")

            lblItemLine.Text = Common.parseNull(dv("IRSD_IRS_LINE"))
            lblInvIndex.Text = Common.parseNull(dv("IRSD_INVENTORY_INDEX"))
            lblLocIndex.Text = Common.parseNull(dv("LM_LOCATION_INDEX"))
            lblLotIndex.Text = Common.parseNull(dv("IRSL_LOT_INDEX"))

            e.Item.Cells(EnumRI.icOutQty).Text = Format(CDec(e.Item.Cells(EnumRI.icQty).Text) - (e.Item.Cells(EnumRI.icReturnedQty).Text), "###0.00")

            'Dim txtReject As TextBox
            'txtReject = e.Item.FindControl("txtReject")

            'txtReject.Text = e.Item.Cells(EnumRI.icOutQty).Text

            If e.Item.Cells(EnumRI.icOutQty).Text = 0 Then
                e.Item.Cells(EnumRI.icReturnQty).Enabled = False
            End If
        End If
    End Sub

    Public Sub dtgMRSDtl_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgMRSDtl.ItemCommand
        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        Dim strName As String
        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

        If (e.CommandSource).CommandName = "" Then
            strFileName = ""
            strName = ""
        End If
    End Sub

    Private Sub PopulateMRSHeader()
        Dim dtHeader As New DataTable
        dtHeader = dsAllInfo.Tables("INVENTORY_REQUISITION_SLIP_MSTR")

        If dtHeader.Rows.Count > 0 Then
            lblMRSDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_DATE")))
            lblIssueTo.Text = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_ISSUE_TO"))
            lblSection.Text = Common.parseNull(dtHeader.Rows(0)("CS_SEC_NAME"))
            lblRefNo.Text = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_REF_NO"))
            lblDept.Text = Common.parseNull(dtHeader.Rows(0)("CDM_DEPT_NAME"))
            lblStatus.Text = Common.parseNull(dtHeader.Rows(0)("STATUS_DESC"))
            lblRequestorName.Text = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_REQUESTOR_NAME"))
            If IsDBNull(dtHeader.Rows(0)("IRSM_IRS_APPROVED_DATE")) Then
                lblMRSIssueDate.Text = ""
            Else
                lblMRSIssueDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_APPROVED_DATE")))
            End If
            txtRemark.Text = Common.parseNull(dtHeader.Rows(0)("IRSM_IRS_REMARK"))
        End If
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim strMsg As String
        Dim objGlobal As New AppGlobals 
        Dim objInventory As New Inventory

        Dim dvViewPR As DataView
        dvViewPR = dsAllInfo.Tables("INVENTORY_REQUISITION_SLIP_DETAILS").DefaultView

        dvViewPR.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"

        ViewState("intPageRecordCnt") = dsAllInfo.Tables("INVENTORY_REQUISITION_SLIP_DETAILS").Rows.Count

        If ViewState("intPageRecordCnt") > 0 Then
            'resetDatagridPageIndex(dtgMRSDtl, dvViewPR)
            dtgMRSDtl.DataSource = dvViewPR
            dtgMRSDtl.DataBind()
        Else
            dtgMRSDtl.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        'ViewState("PageCount") = dtgMRSDtl.PageCount
    End Function

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_ReturnInward_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "ReturnInwardListing.aspx", "pageid=" & strPageId) & """><span>Return Inward</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "ReturnInwardSearch.aspx", "type=Listing&pageid=" & strPageId) & """><span>Return Inward Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
    End Sub

    Private Function validateDatagrid(ByRef strMsg As String) As Boolean
        validateDatagrid = True
        strMsg = "<ul type='disc'>"
        Dim txtQ As TextBox
        Dim txtRemark, txtReject As TextBox
        Dim dgItem As DataGridItem
        Dim blnRejectQty As Boolean = False
        For Each dgItem In dtgMRSDtl.Items

            txtRemark = dgItem.FindControl("txtDtlRemarks")
            txtQ = dgItem.FindControl("txtQ")

            If Not Common.checkMaxLength(txtRemark.Text, 400) Then
                strMsg &= "<li>Remarks is over limit.<ul type='disc'></ul></li>"
                txtQ.Text = "?"
                validateDatagrid = False
            Else
                txtQ.Text = ""
            End If

            txtReject = dgItem.FindControl("txtReject")

            If CDec(txtReject.Text) > CDec(dgItem.Cells(EnumRI.icOutQty).Text) Then
                strMsg &= "<li>" & "" & "Return quantity cannot more than remaining quantity.<ul type='disc'></ul></li>"
                validateDatagrid = False
            End If

            If CDec(txtReject.Text) > 0 Then
                blnRejectQty = True
            End If
        Next

        If blnRejectQty = False Then
            strMsg &= "<li>" & "" & "Must return at least one quantity.<ul type='disc'></ul></li>"
            validateDatagrid = False
        End If

        strMsg &= "</ul>"
    End Function

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Dim dgItem As DataGridItem
        For Each dgItem In dtgMRSDtl.Items
            Dim txtRejQty As TextBox
            Dim txtRemark As TextBox

            txtRejQty = dgItem.FindControl("txtReject")
            txtRejQty.Text = "0.00"

            txtRemark = dgItem.FindControl("txtDtlRemarks")
            txtRemark.Text = ""
        Next
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim ds As New DataSet
        Dim strMsg As String = ""
        Dim strNewRINo As String = ""
        If Page.IsValid And validateDatagrid(strMsg) Then
            BindRI(ds)

            If objINV.RISubmit(ds, strNewRINo) = True Then
                If strFrm = "RIListing" Then
                    Common.NetMsgbox(Me, "RI Number " & strNewRINo & " has been successfully submitted", dDispatcher.direct("Inventory", "ReturnInwardListing.aspx", "pageid=" & strPageId), MsgBoxStyle.Exclamation)
                ElseIf strFrm = "Dashboard" Then
                    Common.NetMsgbox(Me, "RI Number " & strNewRINo & " has been successfully submitted", dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "frm=Dashboard&pageid=0"), MsgBoxStyle.Exclamation)
                End If
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

    Private Sub BindRI(ByRef ds As DataSet)
        Dim dtRIMstr As New DataTable
        Dim dtr As DataRow

        dtRIMstr.Columns.Add("IRIM_IR_NO", Type.GetType("System.String"))
        dtr = dtRIMstr.NewRow
        dtr("IRIM_IR_NO") = lblMRSNo.Text

        dtRIMstr.Rows.Add(dtr)
        ds.Tables.Add(dtRIMstr)

        Dim dgItem As DataGridItem
        Dim dtrd As DataRow
        Dim i As Integer = 0
        Dim dtRIDtls As New DataTable

        'dtRIDtls.Columns.Add("IRID_RI_LINE", Type.GetType("System.String"))
        dtRIDtls.Columns.Add("IRID_IR_LINE", Type.GetType("System.String"))
        dtRIDtls.Columns.Add("IRID_INVENTORY_INDEX", Type.GetType("System.String"))
        dtRIDtls.Columns.Add("IRID_INVENTORY_NAME", Type.GetType("System.String"))
        dtRIDtls.Columns.Add("IRID_UOM", Type.GetType("System.String"))
        dtRIDtls.Columns.Add("IRID_QTY", Type.GetType("System.Double"))
        dtRIDtls.Columns.Add("IRID_LOT_INDEX", Type.GetType("System.String"))
        dtRIDtls.Columns.Add("IRID_LOCATION_INDEX", Type.GetType("System.String"))
        dtRIDtls.Columns.Add("IRID_REMARK", Type.GetType("System.String"))

        For Each dgItem In dtgMRSDtl.Items
            dtr = dtRIDtls.NewRow
            Dim txtRemark, txtReject As TextBox
            Dim lblItemLine, lblInvIndex, lblLocIndex, lblLotIndex As Label

            txtRemark = dgItem.FindControl("txtDtlRemarks")
            lblItemLine = dgItem.FindControl("lblItemLine")
            lblInvIndex = dgItem.FindControl("lblInvIndex")
            txtReject = dgItem.FindControl("txtReject")
            lblLocIndex = dgItem.FindControl("lblLocIndex")
            lblLotIndex = dgItem.FindControl("lblLotIndex")

            If txtReject.Text > 0 Then
                'dtr("IRID_RI_LINE") = i + 1
                dtr("IRID_IR_LINE") = lblItemLine.Text
                dtr("IRID_INVENTORY_INDEX") = lblInvIndex.Text
                dtr("IRID_INVENTORY_NAME") = dgItem.Cells(EnumRI.icItemName).Text
                dtr("IRID_UOM") = dgItem.Cells(EnumRI.icUom).Text
                dtr("IRID_QTY") = txtReject.Text
                dtr("IRID_LOT_INDEX") = lblLotIndex.Text
                dtr("IRID_LOCATION_INDEX") = lblLocIndex.Text
                dtr("IRID_REMARK") = txtRemark.Text

                dtRIDtls.Rows.Add(dtr)
                i = i + 1
            End If
        Next
        ds.Tables.Add(dtRIDtls)

    End Sub
End Class
