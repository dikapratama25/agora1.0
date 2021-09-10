Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class AddDO
    Inherits AgoraLegacy.AppBaseClass
    Dim strDtl As String
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim blnFrmAttchment As Boolean

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'Private Sub Page_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
    '    If ViewState("Mode") = "Edit" Then
    '        objDO.deleteTempDOAttachment(0, ViewState("DONo1"), "H", "", True)
    '    Else
    '        objDO.deleteTempDOAttachment(0, Session.SessionID, "H", "", True)
    '    End If

    'End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents lblDONo As System.Web.UI.WebControls.Label
    Protected WithEvents lblPONo As System.Web.UI.WebControls.Label
    Protected WithEvents lblDraft As System.Web.UI.WebControls.Label
    Protected WithEvents lblDevlDate As System.Web.UI.WebControls.Label
    Protected WithEvents cboPONo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboDelvAdd As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblDelvAdd As System.Web.UI.WebControls.Label
    Protected WithEvents lblPayTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblPayMthd As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipTerm As System.Web.UI.WebControls.Label
    Protected WithEvents lblShipMthd As System.Web.UI.WebControls.Label
    Protected WithEvents lblCustName As System.Web.UI.WebControls.Label
    Protected WithEvents lblDelTerm As System.Web.UI.WebControls.Label
    Protected WithEvents txtOurRefNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAWBillNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOurRefDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFreCarier As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFreAmt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRemarks As System.Web.UI.WebControls.TextBox
    Protected WithEvents dtgDODtl As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblSummPO As System.Web.UI.WebControls.Label
    Protected WithEvents lblPONum As System.Web.UI.WebControls.Label
    Protected WithEvents DtgDoSumm As System.Web.UI.WebControls.DataGrid
    Protected WithEvents btnhidden As System.Web.UI.WebControls.Button
    Protected WithEvents cmdsave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDeleteDO As System.Web.UI.WebControls.Button
    Protected WithEvents cmdsubmit As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents FileDoc As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents pnlAttach As System.Web.UI.WebControls.Panel
    Protected WithEvents cmdUpload As System.Web.UI.WebControls.Button
    Protected WithEvents tr_delTerm As System.Web.UI.HtmlControls.HtmlTableRow
    'Michelle (11/10/2010) - Remove all the tr
    'Protected WithEvents tr1 As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents tr2 As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents tr3 As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents tr4 As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents tr5 As System.Web.UI.HtmlControls.HtmlTableRow
    'Protected WithEvents tr6 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Dim dtDO As DataTable
    Dim objDO As New DeliveryOrder
    Dim strMode, strDONo, strLocID, strBCoyID, strPONo, strFrm, strDA As String
    Dim intPOIdx, intDOIdx, intOutStandingCnt As Integer
    Dim intTotRecord, intTotRecord1 As Integer
    Protected WithEvents revFreight As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents cmdPreviewDO As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents Pass As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents Fail As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lbl_check As System.Web.UI.WebControls.Label

    Public Enum EnumAddDO
        poLine = 0
        '---Added the new column in the grid "dtg_POList" by Praveen on 17/07/2007
        lineno = 1
        VendorItemCode = 2  'Original VendorItemcode=1
        Desc = 3  'Original Desc=2
        Uom = 4
        Edd = 5
        WarTerm = 6
        Mpq = 7
        Moq = 8
        Ordered = 9
        O_Standing = 10
        Shipped = 11
        LotNo = 12
        Remark = 13
        ProductCode = 14
    End Enum
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

        cmdsave.Enabled = False
        cmdDeleteDO.Enabled = False
        cmdsubmit.Enabled = False
        Dim alButtonList As ArrayList
        'Modified by Joon on 11 oct 2010
        If ViewState("Mode") = "Edit" Then 'strMode
            alButtonList = New ArrayList
            alButtonList.Add(cmdsave)
            alButtonList.Add(cmdsubmit)
            htPageAccess.Add("update", alButtonList)
            alButtonList = New ArrayList
            alButtonList.Add(cmdDeleteDO)
            htPageAccess.Add("delete", alButtonList)
        ElseIf ViewState("Mode") = "New" Then
            alButtonList = New ArrayList
            alButtonList.Add(cmdsave)
            alButtonList.Add(cmdsubmit)
            htPageAccess.Add("add", alButtonList)
        End If
        CheckButtonAccess()
        If ViewState("Mode") = "New" Then
            If ViewState("ZeroQty") = "T" Then
                cmdsave.Enabled = True
                cmdsubmit.Enabled = True
                cmdReset.Disabled = False
            Else
                'Michelle (22/1/2013) - Issue 1727
                'If intOutStandingCnt = intTotRecord Then
                If intOutStandingCnt = intTotRecord And Not blnFrmAttchment Then
                    If strDtl <> "created" And strDtl <> "Error" Then
                        cmdsave.Enabled = False
                        cmdsubmit.Enabled = False
                        cmdReset.Disabled = True
                        'meilai----change the value for reset to clear******
                        cmdReset.Value = "Clear"
                    Else
                        cmdsave.Enabled = True
                        cmdsubmit.Enabled = True
                        cmdReset.Disabled = False
                        'meilai----change the value for reset to clear******
                        cmdReset.Value = "Clear"
                    End If

                Else
                    'cmdsave.Enabled = True
                    'cmdsubmit.Enabled = True
                    'cmdReset.Disabled = False
                    cmdsave.Enabled = blnCanAdd
                    cmdsubmit.Enabled = blnCanAdd
                    cmdReset.Disabled = Not blnCanAdd
                End If
            End If

        Else
            cmdsave.Enabled = blnCanUpdate
            cmdsubmit.Enabled = blnCanUpdate
            cmdReset.Disabled = Not blnCanUpdate
            'cmdsave.Enabled = True
            'cmdsubmit.Enabled = True
            'cmdReset.Disabled = False
            End If
        'cmdClear.Enabled = blnCanAdd Or blnCanUpdate Or blnCanDelete

        'displayAttachFile()
        blnFrmAttchment = False
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        If Not Page.IsPostBack Then
            Session("submit") = False
            GenerateTab()
            strMode = Me.Request.QueryString("Mode")
            ViewState("Mode") = strMode
            Session("aryLot") = Nothing
            'Michelle (22/1/2013) - Issue 1727
            If ViewState("Mode") = "Edit" Then
                objDO.deleteTempDOAttachment(0, Me.Request.QueryString("DONo"), "H", "", True)
                objDO.deleteTempDOAttachment(0, Me.Request.QueryString("DONo"), "D", "", True)
            Else
                objDO.deleteTempDOAttachment(0, Session.SessionID, "H", "", True)
                objDO.deleteTempDOAttachment(0, Session.SessionID, "D", "", True)
            End If
        End If

        intOutStandingCnt = 0

        strDONo = Me.Request.QueryString("DONo")
        strLocID = Me.Request.QueryString("LocID")
        intDOIdx = Me.Request.QueryString("DOIdx")
        intPOIdx = Me.Request.QueryString("POIdx")
        strPONo = Me.Request.QueryString("PONo")
        strBCoyID = Me.Request.QueryString("BCoy")
        strFrm = Me.Request.QueryString("Frm")
        Session("strDA") = Me.Request.QueryString("DA")

        Session("strLocID") = strLocID

        blnPaging = False
        blnSorting = False
        SetGridProperty(dtgDODtl)
        SetGridProperty(DtgDoSumm)
        lblPONo.Text = strPONo
        If strFrm = "POList" Then
            'lnkBack.NavigateUrl = "POList.aspx?pageid=" & strPageId
            lnkBack.NavigateUrl = dDispatcher.direct("DO", "POList.aspx", "pageid=" & strPageId)
            'Michelle (30/9/2010) - To link back to Dashboard if calling page is from Dashboard
        ElseIf strFrm = "Dashboard" Then
            'lnkBack.NavigateUrl = "../Dashboard/Vendor.aspx?pageid=0"
            lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=0")
        Else
            'lnkBack.NavigateUrl = "searchDO.aspx?pageid=" & strPageId
            lnkBack.NavigateUrl = dDispatcher.direct("DO", "searchDO.aspx", "pageid=" & strPageId)
        End If

        If Not IsPostBack Then
            Session("blnLotNo") = False
            FuncInvisible()
            'If strDONo = "" Then
            '    strDONo = objDO.DONum(intPOIdx)
            '    If strDONo = "" Then
            '        lblDONo.Text = "To be Allocated"
            '        lblDraft.Visible = False
            '    Else
            '        lblDONo.Text = strDONo
            '        lblDraft.Visible = True
            '    End If

            'Else
            '    lblDONo.Text = strDONo
            '    lblDraft.Visible = True
            'End If
            If strMode = "New" Then
                'strDONo = objDO.DONum(intPOIdx)
                'If strDONo = "" Then
                lblDONo.Text = "To be Allocated"
                lblDraft.Visible = False
                'Else
                '    lblDONo.Text = strDONo
                '    lblDraft.Visible = True
                'End If
            Else
                lblDONo.Text = strDONo
                ViewState("DONo1") = strDONo
                lblDraft.Visible = True
            End If
            '           objDO.GetOutStandingPO(cboPONo)
            lblDevlDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Now())
            If strMode = "New" Then
                cmdDeleteDO.Visible = False
                cmdPreviewDO.Visible = False
                lblSummPO.Visible = False
                'lblPONum.Visible = False
                DtgDoSumm.Visible = False

            ElseIf strMode = "Edit" Then
                ViewState("BCoyID") = strBCoyID
                cmdDeleteDO.Visible = True
                ' ai chu modified on 12/10/2005
                ' draft DO cannot be previewed
                cmdPreviewDO.Visible = False
                objDO.filterDevlAdd(cboDelvAdd, strPONo, intPOIdx)
                Common.SelDdl(strLocID, cboDelvAdd, False)
                'cboDelvAdd.SelectedItem.Text = strLocID
                cboDelvAdd.Enabled = False
                FuncVisible()
                hidSummary.Value = "Remarks-" & txtRemarks.ClientID
                Bindgrid(strLocID)
            End If
            ShowPOLines()

        Else
            hidSummary.Value = "Remarks-" & txtRemarks.ClientID
        End If

        cmdDeleteDO.Attributes.Add("onclick", "return confirm('" & MsgForDeleteButton & "');")
        txtRemarks.Attributes.Add("onKeyDown", "limitText (this, 1000);")
        cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & FileDoc.ClientID & "');")
        displayAttachFile()

        'cmdsave.Attributes.Add("onClick", "return resetSummary('" & txtRemarks.ClientID & "',1000);")
        'cmdsave.Attributes.Add("onClick", "return resetSummary(1,1);")
        'cmdsubmit.Attributes.Add("onClick", "return resetSummary(1,1);")
        '    ShowPOLines()
    End Sub

    Private Function validateDatagrid(ByRef strMsg As String) As Boolean
        validateDatagrid = True
        strMsg = "<ul type='disc'>"
        Dim txtQ As TextBox
        Dim txtRemarkI As TextBox
        Dim txtShiped As TextBox
        Dim btn_lot As Button
        Dim lblLotNo As Label
        Dim dgItem As DataGridItem

        'If strDONo = "" Or strDONo Is Nothing Then
        '    If strMode = "Edit" Then
        '        strDONo = ViewState("DONo1")
        '    Else
        '        If ViewState("DONo") <> "" Then
        '            strDONo = ViewState("DONo1")
        '        Else
        '            strDONo = Session.SessionID
        '        End If
        '    End If
        'End If

        If ViewState("DONo1") <> "" Then
            strDONo = ViewState("DONo1")
        Else
            strDONo = Session.SessionID
        End If

        If Not Common.checkMaxLength(txtRemarks.Text, 1000) Then
            strMsg &= "<li>Remarks is over limit.<ul type='disc'></ul></li>"
            validateDatagrid = False
        End If

        For Each dgItem In dtgDODtl.Items
            txtShiped = dgItem.FindControl("txtShiped")
            txtRemarkI = dgItem.FindControl("txtDtlRemarks")
            txtQ = dgItem.FindControl("txtQ")
            lblLotNo = dgItem.FindControl("lblLotNo")
            btn_lot = dgItem.FindControl("btn_lot")

            If Not Common.checkMaxLength(txtRemarkI.Text, 400) Then
                strMsg &= "<li>" & dgItem.Cells(EnumAddDO.poLine).Text & ". Item remarks is over limit.<ul type='disc'></ul></li>"
                txtQ.Text = "?"
                validateDatagrid = False
            Else
                txtQ.Text = ""
            End If

            If Session("blnLotNo") = True Then
                If txtShiped.Text <> "" Then
                    If lblLotNo.Text = 0 And txtShiped.Text <> 0 Then
                        strMsg &= "<li>" & dgItem.Cells(EnumAddDO.poLine).Text & ". Item Total Lot No must be more than 0.<ul type='disc'></ul></li>"
                        validateDatagrid = False
                    Else
                        If btn_lot.Enabled = True Then
                            If Not objDO.chkIQCWithAttachment(dgItem.Cells(EnumAddDO.VendorItemCode).Text, dgItem.Cells(EnumAddDO.ProductCode).Text, dgItem.Cells(EnumAddDO.poLine).Text, strDONo, Session("aryLot"), ViewState("Mode")) Then
                                strMsg &= "<li>Please attach document for item line " & dgItem.Cells(EnumAddDO.poLine).Text & ".<ul type='disc'></ul></li>"
                                validateDatagrid = False
                            End If
                        End If
                    End If
                End If
            End If
        Next
        strMsg &= "</ul>"

    End Function

    Private Function Bindgrid(Optional ByVal strAddrCode As String = "") As String
        Dim PONo As String
        Dim POIndex As Integer
        Dim AddrCode As String
        Dim dsAllInfo, dsDOSumm As DataSet
        Dim strDetails, strMstr As String
        Dim dtDetails As New DataTable
        Dim dtMstr As New DataTable
        Dim strETD, strDelCode, strDelTerm As String
        Dim objDb As New EAD.DBCom
        Dim CboAdd As Boolean

        PONo = strPONo
        POIndex = intPOIdx
        If strAddrCode <> "" Then
            AddrCode = strAddrCode
            CboAdd = False
        Else
            AddrCode = cboDelvAdd.SelectedItem.Text
            CboAdd = True
        End If
        If ViewState("Mode") = "New" Then  'Modified by Joon on 12th Oct 2010
            dsAllInfo = objDO.GetPODetails(PONo, POIndex, AddrCode, ViewState("BCoyID"))
            If Not dsAllInfo Is Nothing Then
                dtMstr = dsAllInfo.Tables("PO_MSTR")
                If dtMstr.Rows.Count > 0 Then
                    lblPayTerm.Text = Common.parseNull(dtMstr.Rows(0)("POM_Payment_TERM"), "Not Applicable")
                    lblShipTerm.Text = Common.parseNull(dtMstr.Rows(0)("POM_Shipment_Term"), "Not Applicable")
                    lblPayMthd.Text = Common.parseNull(dtMstr.Rows(0)("POM_PAYMENT_METHOD"), "Not Applicable")
                    lblShipMthd.Text = Common.parseNull(dtMstr.Rows(0)("POM_Shipment_Mode"), "Not Applicable")
                    lblCustName.Text = Common.parseNull(dtMstr.Rows(0)("POM_BUYER_NAME"), "Not Applicable")
                    FillDelvAdd(dsAllInfo.Tables("PO_DETAILS"), CboAdd)

                    If Common.parseNull(dtMstr.Rows(0)("POM_DEL_CODE")) = "" Then
                        tr_delTerm.Style("display") = "none"
                    Else
                        strDelCode = Common.parseNull(dtMstr.Rows(0)("POM_DEL_CODE"))
                        strDelTerm = objDb.GetVal("SELECT IFNULL(CONCAT(CDT_DEL_CODE, ' (', CDT_DEL_NAME, ')'),'') FROM COMPANY_DELIVERY_TERM " &
                                                "WHERE CDT_COY_ID = '" & dtMstr.Rows(0)("POM_B_COY_ID") & "' AND CDT_DEL_CODE='" & Common.Parse(strDelCode) & "'")
                        lblDelTerm.Text = strDelTerm
                        tr_delTerm.Style("display") = ""
                    End If
                End If
            End If
        Else
            dsAllInfo = objDO.ShowDOdetails(strDONo, PONo, POIndex, strLocID, ViewState("BCoyID"))

            If Not dsAllInfo Is Nothing Then
                dtMstr = dsAllInfo.Tables("PO_MSTR")
                dtDetails = dsAllInfo.Tables("PO_DETAILS")
                '//add by MOO
                dtDO = dsAllInfo.Tables("DO_DETAILS")
                If dtMstr.Rows.Count > 0 Then
                    lblPayTerm.Text = Common.parseNull(dtMstr.Rows(0)("POM_Payment_TERM"), "Not Applicable")
                    lblShipTerm.Text = Common.parseNull(dtMstr.Rows(0)("POM_Shipment_Term"), "Not Applicable")
                    lblPayMthd.Text = Common.parseNull(dtMstr.Rows(0)("POM_PAYMENT_METHOD"), "Not Applicable")
                    lblShipMthd.Text = Common.parseNull(dtMstr.Rows(0)("POM_Shipment_Mode"), "Not Applicable")
                    txtAWBillNo.Text = Common.parseNull(dtMstr.Rows(0)("DOM_Waybill_No"))
                    txtFreCarier.Text = Common.parseNull(dtMstr.Rows(0)("DOM_Freight_Carrier"))
                    txtRemarks.Text = Common.parseNull(dtMstr.Rows(0)("DOM_DO_Remarks"))
                    txtOurRefNo.Text = Common.parseNull(dtMstr.Rows(0)("DOM_S_Ref_No"))
                    txtOurRefDate.Text = Common.parseNull(dtMstr.Rows(0)("DOM_S_Ref_Date"))
                    'txtFreAmt.Text = Common.parseNull(dtMstr.Rows(0)("DOM_Freight_Amt"))
                    If Not IsDBNull(dtMstr.Rows(0)("DOM_Freight_Amt")) AndAlso CStr(dtMstr.Rows(0)("DOM_Freight_Amt")) <> "" Then
                        txtFreAmt.Text = Format$(dtMstr.Rows(0)("DOM_Freight_Amt"), "##0.00")
                    End If
                    lblCustName.Text = Common.parseNull(dtMstr.Rows(0)("POM_BUYER_NAME"), "Not Applicable")

                    If Common.parseNull(dtMstr.Rows(0)("POM_DEL_CODE")) = "" Then
                        tr_delTerm.Style("display") = "none"
                    Else
                        strDelCode = Common.parseNull(dtMstr.Rows(0)("POM_DEL_CODE"))
                        strDelTerm = objDb.GetVal("SELECT IFNULL(CONCAT(CDT_DEL_CODE, ' (', CDT_DEL_NAME, ')'),'') FROM COMPANY_DELIVERY_TERM " &
                                                "WHERE CDT_COY_ID = '" & dtMstr.Rows(0)("POM_B_COY_ID") & "' AND CDT_DEL_CODE='" & Common.Parse(strDelCode) & "'")
                        lblDelTerm.Text = strDelTerm
                        tr_delTerm.Style("display") = ""
                    End If

                    FillDelvAdd(dsAllInfo.Tables("PO_DETAILS"), CboAdd)
                End If
            End If
        End If

        'Check for Delivery Term setup
        If IsDBNull(dsAllInfo.Tables("PO_DETAILS").Rows(0)("POD_ITEM_TYPE")) Then
            Session("blnLotNo") = False
        Else
            If objDb.Exist("SELECT '*' FROM COMPANY_DELIVERY_TERM WHERE CDT_COY_ID = '" & ViewState("BCoyID") & "'") And dsAllInfo.Tables("PO_DETAILS").Rows(0)("POD_ITEM_TYPE") = "ST" Then
                Session("blnLotNo") = True
            Else
                Session("blnLotNo") = False
            End If
        End If

        Dim dvViewDO As DataView
        dvViewDO = dsAllInfo.Tables("PO_DETAILS").DefaultView
        dvViewDO.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewDO.Sort += " DESC"

        intTotRecord = dsAllInfo.Tables("PO_DETAILS").Rows.Count
        intPageRecordCnt = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        ViewState("POMDODte") = dtMstr.Rows(0)("POM_CREATED_DATE")
        ViewState("POMBillMthd") = dtMstr.Rows(0)("POM_Billing_Method")
        ViewState("POMPOIdx") = dtMstr.Rows(0)("POM_PO_Index")
        If intTotRecord > 0 Then
            dtgDODtl.Visible = True
            dtgDODtl.DataSource = dvViewDO
            dtgDODtl.DataBind()
            'If Session("Env") = "FTN" Then
            '    Me.dtgDODtl.Columns(6).Visible = False
            '    Me.dtgDODtl.Columns(7).Visible = False
            '    Me.dtgDODtl.Columns(8).Visible = False
            'Else
            '    Me.dtgDODtl.Columns(6).Visible = True
            '    Me.dtgDODtl.Columns(7).Visible = True
            '    Me.dtgDODtl.Columns(8).Visible = True
            'End If
            Me.dtgDODtl.Columns(6).Visible = True
            Me.dtgDODtl.Columns(7).Visible = True
            Me.dtgDODtl.Columns(8).Visible = True

            If Session("blnLotNo") = True Then
                Me.dtgDODtl.Columns(EnumAddDO.LotNo).Visible = True
            Else
                Me.dtgDODtl.Columns(EnumAddDO.LotNo).Visible = False
            End If
        Else
            dtgDODtl.Visible = False
        End If

        '//if all items have outstanding = 0, disable save and submit


        Dim dvViewDOSumm As DataView
        dsDOSumm = objDO.GetDOSumm(POIndex)
        dvViewDOSumm = dsDOSumm.Tables(0).DefaultView
        dvViewDOSumm.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewDOSumm.Sort += " DESC"

        intTotRecord1 = dsDOSumm.Tables(0).Rows.Count
        intPageRecordCnt = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        If intTotRecord1 > 0 Then
            'intTotPage = dtgDO.PageCount
            DtgDoSumm.Visible = True
            lblSummPO.Visible = True
            lblPONum.Visible = True
            lblPONum.Text = PONo
            DtgDoSumm.DataSource = dvViewDOSumm
            DtgDoSumm.DataBind()
        Else
            lblSummPO.Visible = False
            lblPONum.Visible = False
            DtgDoSumm.Visible = False
        End If
    End Function
#Region " Datagrid dtgDODtl"

    Private Sub dtgDODtl_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgDODtl.ItemCommand
        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        Dim strName, strmode As String
        strmode = ViewState("Mode")
        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

        If Session("submit") = True Then
            strDONo = ViewState("DONo1")
        Else
            If strDONo = "" Or strDONo Is Nothing Then
                If strmode = "Edit" Then
                    strDONo = ViewState("DONo1")
                End If
            End If
        End If

        If (e.CommandSource).CommandName = "" Then
            Dim txtQty As TextBox

            txtQty = e.Item.FindControl("txtShiped")
            strName = "shipqty=" & txtQty.Text & "&DONo=" & Server.UrlEncode(strDONo) & "&PONo=" & Server.UrlEncode(strPONo) & "&itemline=" & Server.UrlEncode(e.Item.Cells(EnumAddDO.lineno).Text) & "&poline=" & Server.UrlEncode(e.Item.Cells(EnumAddDO.poLine).Text) & "&submit=" & Session("submit") & "&itemcode=" & Server.UrlEncode(e.Item.Cells(EnumAddDO.ProductCode).Text) & "&mode=" & Server.UrlEncode(strmode) & "&BCoyId=" & Server.UrlEncode(Request.QueryString("BCoy")) & ""

            strscript.Append("<script language=""javascript"">")
            strFileName = dDispatcher.direct("DO", "LotDeliveryOrder.aspx", strName)
            strFileName = Server.UrlEncode(strFileName)
            'strscript.Append("PopWindow('" & dDispatcher.direct("DO", "LotDeliveryOrder.aspx", strName) & "');")
            strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','400px');")
            strscript.Append("document.getElementById('btnhidden').click();")
            strscript.Append("</script>")
            RegisterStartupScript("script3", strscript.ToString())

        End If

    End Sub
    Private Sub dtgDODtl_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDODtl.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgDODtl, e)

        ' ai chu add on 27/09/2005
        ' if create new DO, column for <Shipped> changed to <To Be Shipped>
        If e.Item.ItemType = ListItemType.Header Then
            'If strMode = "New" Then
            '    e.Item.Cells(EnumAddDO.Shipped).Text = "To Be Shipped"
            'Else
            '    e.Item.Cells(EnumAddDO.Shipped).Text = "Ship"
            'End If
        End If
    End Sub

    Private Sub dtgDODtl_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDODtl.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dt As DataTable
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim txtShip As TextBox
            txtShip = e.Item.FindControl("txtShiped")
            Dim btn_lot As Button
            btn_lot = e.Item.FindControl("btn_lot")
            'New Code Adding to get the Lineno in the Grid "dtg_POList" By Praveen on 16/07/2007

            e.Item.Cells(1).Text = e.Item.DataSetIndex + 1
            'End the Code 

            'used the POM_PO_Date to calculate teh EDD

            '************meilai edit 3/1/2005*************************************
            'If IsDBNull(e.Item.Cells(4).Text) Or e.Item.Cells(4).Text = "0" Then
            'e.Item.Cells(4).Text = "Ex-Stock"
            'Else
            'e.Item.Cells(4).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, DateAdd("D", dv("POD_ETD"), viewstate("POMDODte")))
            'End If
            If IsDBNull(e.Item.Cells(EnumAddDO.Edd).Text) Or e.Item.Cells(EnumAddDO.Edd).Text = "0.00" Then
                e.Item.Cells(EnumAddDO.Edd).Text = "Ex-Stock"
            Else
                e.Item.Cells(EnumAddDO.Edd).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, DateAdd("D", dv("POD_ETD"), ViewState("POMDODte")))
            End If

            'e.Item.Cells(9).Text = objDO.GetOutstd(dv("POD_Ordered_Qty"), viewstate("POMPOIdx"), dv("POD_PO_No"), dv("POD_Po_Line"), viewstate("POMBillMthd"))

            '**************meilai 3/1/2005**************************
            'e.Item.Cells(9).Text = Common.parseNull(dv("POD_ORDERED_QTY"), 0) - Common.parseNull(dv("POD_DELIVERED_QTY"), 0) - Common.parseNull(dv("POD_CANCELLED_QTY"), 0)
            e.Item.Cells(EnumAddDO.O_Standing).Text = Common.parseNull(dv("POD_ORDERED_QTY"), 0) - Common.parseNull(dv("POD_DELIVERED_QTY"), 0) - Common.parseNull(dv("POD_CANCELLED_QTY"), 0)

            If e.Item.Cells(EnumAddDO.O_Standing).Text = "0.00" Then
                txtShip.Enabled = False
                btn_lot.Enabled = False
                CType(e.Item.FindControl("txtDtlRemarks"), TextBox).Enabled = False
                intOutStandingCnt += 1
            Else
                CType(e.Item.FindControl("txtDtlRemarks"), TextBox).Enabled = True
                txtShip.Enabled = True
                btn_lot.Enabled = True
            End If
            '************************meilai 20041231 Compare Validator between txtShip And Outstanding***********
            'Dim txtShiped As TextBox
            'txtShiped = e.Item.FindControl("txtShiped")

            Dim lblLotNo As Label
            Dim aryTemp As New ArrayList()
            'Dim i, found, iError As Integer
            Dim dsTemp As New DataSet
            'aryTemp = Session("aryLot")
            lblLotNo = e.Item.FindControl("lblLotNo")

            'found = 0
            'iError = 0
            'If Not aryTemp Is Nothing Then
            '    For i = 0 To aryTemp.Count - 1
            '        If aryTemp(i)(8) = e.Item.Cells(EnumAddDO.lineno).Text Then
            '            If aryTemp(i)(0) <> "" And aryTemp(i)(1) <> "" And aryTemp(i)(2) <> "" And aryTemp(i)(3) <> "" Then
            '                If aryTemp(i)(6) = e.Item.Cells(2).Text And aryTemp(i)(8) = e.Item.Cells(EnumAddDO.lineno).Text Then
            '                    found = found + 1
            '                End If
            '            End If
            '        Else
            '            iError = iError + 1
            '        End If

            '    Next

            '    If iError >= found Then
            '        dsTemp = objDO.getDOLot(strDONo, e.Item.Cells(2).Text)
            '        If dsTemp.Tables(0).Rows.Count > 0 Then
            '            lblLotNo.Text = dsTemp.Tables(0).Rows.Count
            '        Else
            '            lblLotNo.Text = "0"
            '        End If
            '    Else
            '        lblLotNo.Text = found
            '    End If


            'Else
            '    If ViewState("Mode") = "Edit" Then
            '        dsTemp = objDO.getDOLot(strDONo, e.Item.Cells(2).Text)
            '        If dsTemp.Tables(0).Rows.Count > 0 Then
            '            lblLotNo.Text = dsTemp.Tables(0).Rows.Count
            '        Else
            '            lblLotNo.Text = "0"
            '        End If
            '    Else
            '        lblLotNo.Text = "0"
            '    End If

            'End If

            If ViewState("Mode") = "Edit" Then
                dsTemp = objDO.getDOLot(strDONo, e.Item.Cells(EnumAddDO.VendorItemCode).Text, e.Item.Cells(EnumAddDO.poLine).Text)
                If dsTemp.Tables(0).Rows.Count > 0 Then
                    lblLotNo.Text = dsTemp.Tables(0).Rows.Count
                Else
                    lblLotNo.Text = "0"
                End If
            Else
                lblLotNo.Text = "0"
            End If

            '****************************************************************************************************

            '//add by MOO
            If ViewState("Mode") = "Edit" Then
                Dim drResult As DataRow()
                Dim intShip As Integer
                drResult = dtDO.Select("DOD_PO_LINE=" & dv("POD_PO_LINE"))
                If drResult.Length > 0 Then
                    txtShip.Text = Common.parseNull(drResult(0)("DOD_SHIPPED_QTY"), 0)
                    CType(e.Item.FindControl("txtDtlRemarks"), TextBox).Text = Common.parseNull(drResult(0)("DOD_REMARKS"))
                    txtShip.Enabled = True
                Else
                    txtShip.Text = 0
                    'txtShip.Enabled = False
                End If
            Else
                '***************meilai 3/1/2005***************
                'txtShip.Text = e.Item.Cells(EnumAddDO.O_Standing).Text
                txtShip.Text = e.Item.Cells(EnumAddDO.O_Standing).Text
                '*********************************************
            End If

            'If strMode = "Edit" Then
            '    If e.Item.Cells(9).Text > txtShip.Text Then
            '        'txtShip.Enabled = False
            '    End If
            'End If

            ' ai chu add (for javascript)
            Dim hidCode As HtmlInputHidden
            hidCode = e.Item.FindControl("hidCode")
            hidCode.Value = dv("POD_Po_Line")

            Dim revShipped As RegularExpressionValidator
            revShipped = e.Item.FindControl("revShipped")
            revShipped.ValidationExpression = "^\d{1,6}(?:\.(?:\d{0,1})?\d)?$"
            revShipped.ControlToValidate = "txtShiped"
            revShipped.ErrorMessage = hidCode.Value & ". Quantity Shipped is over limit/expecting numeric value."
            revShipped.Text = "?"
            revShipped.Display = ValidatorDisplay.Dynamic

            Dim cvShipped As CompareValidator
            cvShipped = e.Item.FindControl("cvShipped")
            'cvShipped.ValueToCompare = e.Item.Cells(9).Text
            cvShipped.ValueToCompare = e.Item.Cells(EnumAddDO.O_Standing).Text
            'cvShipped.ControlToValidate = "txtShiped"
            cvShipped.Type = ValidationDataType.Double
            cvShipped.Operator = ValidationCompareOperator.LessThanEqual
            cvShipped.ErrorMessage = hidCode.Value & ". Quantity Shipped must Less than or equal to Outstanding Quantity."
            cvShipped.Text = "?"
            cvShipped.Display = ValidatorDisplay.Dynamic

            Dim txtRemark As TextBox
            txtRemark = e.Item.FindControl("txtDtlRemarks")
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
            ' ai chu add end

            'To validate txtprice when user press on delete & space infront the value
            txtShip.Attributes.Add("onBlur", "resetValue('" & txtShip.ClientID & "','1')")
        End If
    End Sub
    Private Sub DtgDoSumm_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DtgDoSumm.ItemCreated
        '//this line must be included
        Grid_ItemCreated(DtgDoSumm, e)
    End Sub
    Private Sub DtgDoSumm_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DtgDoSumm.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dt As DataTable
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            e.Item.Cells(0).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("date_created"))
            Dim lnkDONum, lnkPONum As HyperLink
            lnkDONum = e.Item.FindControl("lnkDONum")
            lnkDONum.Text = dv("DOM_DO_NO")
            lnkDONum.NavigateUrl = "javascript:;" '"DODetails.aspx?DONo=" & dv("DOM_DO_NO") & "&DOIdx=" & dv("DOM_DO_Index") & "&POIdx=" & dv("DOM_PO_Index") & "&pageid=" & strPageId & "&LocID=" & dv("DOM_D_ADDR_CODE") & "&BCoy=" & viewstate("BCoyID")
            'lnkDONum.Attributes.Add("onclick", "return PopWindow('DOReport.aspx?pageid=" & strPageId & "&DONo=" & dv("DOM_DO_NO") & "&SCoyID=" & Session("CompanyID") & "')")
            'lnkDONum.Attributes.Add("onclick", "return PopWindow('PreviewDO.aspx?pageid=" & strPageId & "&DONo=" & dv("DOM_DO_NO") & "&SCoyID=" & Session("CompanyID") & "')")
            'lnkDONum.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Report", "PreviewDO.aspx", "pageid=" & strPageId & "&DONo=" & dv("DOM_DO_NO") & "&SCoyID=" & Session("CompanyID") & "')"))
            lnkDONum.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Report", "PreviewDO.aspx", "pageid=" & strPageId & "&DONo=" & dv("DOM_DO_NO") & "&SCoyID=" & Session("CompanyID") & "&PO_NO=" & Me.Request.QueryString("PONo")) & "')")

            If objDO.withAttach(dv("DOM_DO_NO")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")
                lnkDONum.Controls.Add(imgAttach)
                e.Item.Cells(1).Controls.Add(imgAttach)
            End If

        ElseIf e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(1).Text = "DO Number"
            e.Item.Cells(2).Text = "Created By"
        End If
    End Sub

    Sub dtgDODtl_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgDODtl.PageIndexChanged
        dtgDODtl.CurrentPageIndex = e.NewPageIndex

    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgDODtl.SortCommand
        Grid_SortCommand(sender, e)
        dtgDODtl.CurrentPageIndex = 0

    End Sub
#End Region


    'Private Sub cboPONo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPONo.SelectedIndexChanged
    '    hidControl.Value = ""
    '    Dim PONo As String
    '    Dim POIndex As Integer
    '    Dim dsAllInfo, ds As DataSet
    '    Dim intRecordCnt As Integer

    '    If cboPONo.SelectedIndex <> 0 Then
    '        PONo = cboPONo.SelectedItem.Text
    '        POIndex = cboPONo.SelectedItem.Value

    '        Dim dtHeader As New DataTable
    '        dsAllInfo = objDO.filterDevlAdd(cboDelvAdd, PONo, POIndex)

    '        intRecordCnt = dsAllInfo.Tables(0).Rows.Count
    '        If intRecordCnt > 0 Then
    '            viewstate("BCoyID") = dsAllInfo.Tables(0).Rows(0)("POM_B_Coy_ID")
    '            'strBCoyID = viewstate("BCoyID")
    '            If intRecordCnt > 1 Then
    '                FuncInvisible()
    '                cboDelvAdd.Visible = True
    '                lblDelvAdd.Visible = False
    '                lblSummPO.Visible = False
    '                lblPONum.Visible = False
    '                DtgDoSumm.Visible = False
    '                dtgDODtl.Visible = False
    '            Else
    '                viewstate("AddrCode") = dsAllInfo.Tables(0).Rows(0)("POD_D_Addr_Code")
    '                'FillDelvAdd(dsAllInfo, False)
    '                lblDelvAdd.Visible = True
    '                cboDelvAdd.Visible = False

    '                If objDO.IsLocHasDraftDo(viewstate("AddrCode"), POIndex) Then
    '                    FuncInvisible("DELVA")
    '                    FillDelvAdd(dsAllInfo.Tables(0), False)
    '                    Common.NetMsgbox(Me, "Draft DO already created for this delivery address.")
    '                Else
    '                    Bindgrid(viewstate("AddrCode"))
    '                    FuncVisible()
    '                End If

    '            End If
    '        End If
    '    Else
    '        FuncInvisible()
    '    End If
    'End Sub
    Private Sub ShowPOLines()
        hidControl.Value = ""
        Dim PONo As String
        Dim POIndex As Integer
        Dim dsAllInfo, ds As DataSet
        Dim intRecordCnt As Integer

        Dim dtHeader As New DataTable
        dsAllInfo = objDO.filterDevlAdd(cboDelvAdd, strPONo, intPOIdx, Session("strDA"))

        intRecordCnt = dsAllInfo.Tables(0).Rows.Count
        If intRecordCnt > 0 Then
            ViewState("BCoyID") = dsAllInfo.Tables(0).Rows(0)("POM_B_Coy_ID")
            strBCoyID = ViewState("BCoyID")
            If intRecordCnt > 1 Then
                FuncInvisible()
                If ViewState("Mode") = "New" Then
                    cboDelvAdd.Visible = True
                    lblDelvAdd.Visible = False
                Else
                    cboDelvAdd.Visible = False
                    lblDelvAdd.Visible = True
                End If

                'lblSummPO.Visible = False
                'lblPONum.Visible = False
                'DtgDoSumm.Visible = False
                'dtgDODtl.Visible = False
                ''cboDelvAdd.Visible = True
                ''lblDelvAdd.Visible = False
                'lblSummPO.Visible = False
                'lblPONum.Visible = False
                'DtgDoSumm.Visible = False
                'dtgDODtl.Visible = False
            Else
                ViewState("AddrCode") = dsAllInfo.Tables(0).Rows(0)("POD_D_Addr_Code")
                'FillDelvAdd(dsAllInfo, False)
                lblDelvAdd.Visible = True
                cboDelvAdd.Visible = False

                If objDO.IsLocHasDraftDo(ViewState("AddrCode"), POIndex) Then
                    FuncInvisible("DELVA")
                    FillDelvAdd(dsAllInfo.Tables(0), False)
                    Common.NetMsgbox(Me, "Draft DO already created for this delivery address.")
                Else
                    Bindgrid(ViewState("AddrCode"))
                    FuncVisible()
                End If

            End If
            'If strDA <> "" Then
            '    If objDO.IsLocHasDraftDo(strDA, intPOIdx) Then
            '        Common.NetMsgbox(Me, "Draft DO already created for this delivery address.")
            '        FuncInvisible("DELVA")
            '    Else
            '        FuncVisible()
            '        Bindgrid(strDA)
            '    End If
            'Else
            '    FuncInvisible("DELVA")
            '    Me.dtgDODtl.Visible = False
            '    lblPayTerm.Text = ""
            '    lblShipTerm.Text = ""
            '    lblPayMthd.Text = ""
            '    lblShipMthd.Text = ""
            '    lblCustName.Text = ""
            '    lblDelvAdd.Text = ""
            '    txtAWBillNo.Text = ""
            '    txtOurRefNo.Text = ""
            '    txtFreCarier.Text = ""
            '    txtOurRefDate.Text = ""
            '    txtFreAmt.Text = ""
            '    txtRemarks.Text = ""

            'End If
        End If
        ' FuncInvisible()
    End Sub
    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        If FileDoc.Value <> "" Then
            Dim objFile As New FileManagement
            Dim strDocNo As String
            If ViewState("DONo1") <> "" Then
                strDocNo = ViewState("DONo1")
            Else
                strDocNo = Session.SessionID
            End If

            Dim objDB As New EAD.DBCom
            Dim dblMaxFileSize As Double = Session("FileSize")

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(FileDoc.PostedFile.FileName)

            'Jules 2018.09.13 - Increase length from 46 to 200.
            If Len(sFileName) > 205 Then
                Common.NetMsgbox(Me, "File name exceeds 200 characters")
            ElseIf objDO.chkDupDOAttach(strDocNo, System.IO.Path.GetFileName(FileDoc.PostedFile.FileName)) Then
                Common.NetMsgbox(Me, "Duplicate File")
            ElseIf FileDoc.PostedFile.ContentLength > 0 And FileDoc.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                objFile.FileUpload(FileDoc, EnumUploadType.DOAttachment, "DO", EnumUploadFrom.FrontOff, strDocNo, True, , , , , "H")
            ElseIf FileDoc.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            displayAttachFile()
            objFile = Nothing
            objDB = Nothing
        End If
    End Sub
    Private Sub displayAttachFile()
        Dim dsAttach As New DataSet
        Dim drvAttach As New DataView
        Dim i As Integer
        Dim objFile As New FileManagement
        Dim strFile, strFile1, strURL, strTemp As String
        If ViewState("DONo1") = "" Then
            dsAttach = objDO.getTempDOAttachment(Session.SessionID)
        Else
            dsAttach = objDO.getTempDOAttachment(ViewState("DONo1"))
        End If

        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                Dim strFilePath As String
                strFile = drvAttach(i)("CDDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDDA_HUB_FILENAME")
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DOAttachment, "DO", EnumUploadFrom.FrontOff)

                Dim lblBr As New Label
                Dim lblFile As New Label
                Dim lnk As New ImageButton
                lblFile.Text = "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDDA_FILESIZE") & "KB) "
                lblBr.Text = "<BR>"
                If cmdsave.Visible Then
                    lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete2.gif")
                Else
                    lnk.Visible = False
                    FileDoc.Disabled = True
                    cmdUpload.Enabled = False
                End If
                lnk.ID = drvAttach(i)("CDDA_ATTACH_INDEX")
                lnk.CausesValidation = False
                AddHandler lnk.Click, AddressOf deleteAttach

                pnlAttach.Controls.Add(lblFile)
                pnlAttach.Controls.Add(lnk)
                pnlAttach.Controls.Add(lblBr)
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFile)
        End If
        blnFrmAttchment = True
    End Sub
    Private Sub deleteAttach(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strDocNo, strStatus As String
        If ViewState("DONo1") <> "" Then
            strDocNo = ViewState("DONo1")
            strStatus = "U" 'ie delete the attachment that is already in the database or delete the new attachment of Draft DO
        Else
            strDocNo = Session.SessionID
            strStatus = "D" 'ie. delete those attachment that has not been updated into the database
        End If

        objDO.deleteTempDOAttachment(CType(sender, ImageButton).ID, strDocNo, "H", strStatus)
        displayAttachFile()
    End Sub

    Private Sub cboDelvAdd_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDelvAdd.SelectedIndexChanged
        If cboDelvAdd.SelectedIndex <> 0 Then
            If objDO.IsLocHasDraftDo(cboDelvAdd.SelectedValue, intPOIdx) Then
                Common.NetMsgbox(Me, "Draft DO already created for this delivery address.")
                FuncInvisible("DELVA")
            Else
                FuncVisible()
                Bindgrid()
            End If
        Else
            FuncInvisible("DELVA")
            Me.dtgDODtl.Visible = False
            lblPayTerm.Text = ""
            lblShipTerm.Text = ""
            lblPayMthd.Text = ""
            lblShipMthd.Text = ""
            lblCustName.Text = ""
            lblDelvAdd.Text = ""
            txtAWBillNo.Text = ""
            txtOurRefNo.Text = ""
            txtFreCarier.Text = ""
            txtOurRefDate.Text = ""
            txtFreAmt.Text = ""
            txtRemarks.Text = ""

        End If


    End Sub
#Region " Button Click "
    'Private Sub cmdsubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdsubmit.Click

    '    Dim dsDO As DataSet
    '    Dim Pass As Boolean = True
    '    Dim strMsg As String

    '    dsDO = BindDO(Pass)
    '    If viewstate("Mode") = "New" Then
    '        If Pass = True Then
    '            If objDO.DONew(dsDO, "Submit", strDONo, strMsg) = True Then
    '                If strMsg = "dup" Then
    '                    Common.NetMsgbox(Me, MsgTransDup, "SearchDO.aspx?pageid=" & strPageId)
    '                Else
    '                    Response.Redirect("DOMsgAdd.aspx?Frm=" & strFrm & "&status=2&pageid=" & strPageId & "&DONo=" & strDONo & "&HD=Add New DO&Dtl=created&Pass=" & Pass)
    '                End If
    '            Else
    '                If strMsg = "dup" Then
    '                    Common.NetMsgbox(Me, MsgTransDup, "SearchDO.aspx?pageid=" & strPageId)
    '                Else
    '                    Response.Redirect("DOMsgAdd.aspx?Frm=" & strFrm & "&status=1&pageid=" & strPageId & "&DONo=&HD=Add New DO&Dtl=" & strMsg & "&Pass=False")
    '                End If
    '            End If
    '        Else
    '            Response.Redirect("DOMsgAdd.aspx?Frm=" & strFrm & "&status=1&pageid=" & strPageId & "&DONo=&HD=Add New DO&Pass=" & Pass)
    '        End If
    '    Else
    '        If Pass = True Then
    '            If objDO.DOEdit(dsDO, "Submit", strDONo, viewstate("BCoyID"), strMsg) = True Then
    '                Response.Redirect("DOMsgAdd.aspx?Frm=" & strFrm & "&status=2&pageid=" & strPageId & "&DONo=" & strDONo & "&HD=Update DO&Dtl=submitted&Pass=" & Pass)
    '            Else
    '                Response.Redirect("DOMsgAdd.aspx?Frm=" & strFrm & "&status=1&pageid=" & strPageId & "&DONo=" & strDONo & "&HD=Update DO&Dtl=" & strMsg & "&Pass=False")
    '            End If
    '        Else
    '            Response.Redirect("DOMsgAdd.aspx?Frm=" & strFrm & "&status=1&pageid=" & strPageId & "&DONo=&HD=Update DO&Pass=" & Pass)
    '        End If
    '    End If
    'End Sub

    Private Sub cmdsubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdsubmit.Click
        'Modified by Joon on 5th oct 2010

        Dim dsDO As DataSet
        Dim Pass As Boolean = True
        Dim strMsg As String
        Dim strJScript As String
        'Dim strDtl As String

        strDtl = ""
        strJScript = ""
        lbl_check.Text = ""

        ViewState("DMode") = "Submit"
        dsDO = BindDO(Pass)
        If ViewState("Mode") = "New" Then
            If Pass = True And validateDatagrid(strMsg) Then
                ViewState("ZeroQty") = ""
                If objDO.DONew(dsDO, "Submit", strDONo, strMsg, Session("blnLotNo"), Session("aryLot")) = True Then
                    ViewState("DONo1") = strDONo
                    If strMsg = "dup" Then
                        'Common.NetMsgbox(Me, MsgTransDup, "SearchDO.aspx?pageid=" & strPageId)
                        Common.NetMsgbox(Me, MsgTransDup, dDispatcher.direct("DO", "SearchDO.aspx", "pageid=" & strPageId))
                    Else
                        'Response.Redirect("DOMsgAdd.aspx?Frm=" & strFrm & "&status=2&pageid=" & strPageId & "&DONo=" & strDONo & "&HD=Add New DO&Dtl=submitted&Pass=" & Pass)
                        strDtl = "created"
                        Session("submit") = True
                        Common.NetMsgbox(Me, "Delivery Order Number " & ViewState("DONo1") & " has been submitted.")
                        lblDONo.Text = ViewState("DONo1")
                        cmdsave.Visible = False
                        cmdsubmit.Visible = False
                        cmdDeleteDO.Visible = False
                        cmdReset.Visible = False
                        cmdPreviewDO.Visible = True
                        lblDraft.Visible = False
                        Me.cboDelvAdd.Enabled = False
                        txtAWBillNo.Enabled = False
                        txtOurRefNo.Enabled = False
                        txtFreCarier.Enabled = False
                        txtOurRefDate.Enabled = False
                        txtFreAmt.Enabled = False
                        txtRemarks.Enabled = False
                    End If
                Else
                    If strMsg = "dup" Then
                        'Common.NetMsgbox(Me, MsgTransDup, "SearchDO.aspx?pageid=" & strPageId)
                        Common.NetMsgbox(Me, MsgTransDup, dDispatcher.direct("DO", "SearchDO.aspx", "pageid=" & strPageId))
                    Else
                        'Michelle (4/11/2011) - Issue 1166
                        If strMsg = "outs" Then
                            strMsg = "DO has been submitted by other vendor"
                        End If
                        'Response.Redirect("DOMsgAdd.aspx?Frm=" & strFrm & "&status=1&pageid=" & strPageId & "&DONo=&HD=Add New DO&Dtl=" & strMsg & "&Pass=False")
                        Pass = False
                        strDtl = strMsg
                        cmdPreviewDO.Visible = False
                        Common.NetMsgbox(Me, strMsg)
                    End If
                End If
            Else
                cmdPreviewDO.Visible = False

                If strMsg <> "" Then
                    strDtl = "Error"
                    lbl_check.Text = strMsg
                Else
                    lbl_check.Text = ""
                End If

                'Response.Redirect("DOMsgAdd.aspx?Frm=" & strFrm & "&status=1&pageid=" & strPageId & "&DONo=&HD=Add New DO&Pass=" & Pass)
            End If
        Else
            If Pass = True And validateDatagrid(strMsg) Then
                ViewState("ZeroQty") = ""
                If objDO.DOEdit(dsDO, "Submit", ViewState("DONo1"), ViewState("BCoyID"), strMsg, Session("blnLotNo"), Session("aryLot")) = True Then
                    'Response.Redirect("DOMsgAdd.aspx?Frm=" & strFrm & "&status=2&pageid=" & strPageId & "&DONo=" & strDONo & "&HD=Update DO&Dtl=submitted&Pass=" & Pass)
                    strDtl = "submitted"
                    cmdsave.Visible = False
                    cmdsubmit.Visible = False
                    cmdDeleteDO.Visible = False
                    cmdReset.Visible = False
                    cmdPreviewDO.Visible = True
                    lblDraft.Visible = False
                    Session("submit") = True
                    Common.NetMsgbox(Me, "Delivery Order Number " & ViewState("DONo1") & " has been submitted.")
                Else
                    'Michelle (8/11/2011) - Issue 1172
                    'Common.NetMsgbox(Me, strMsg)
                    If strMsg = "99" Then
                        strDtl = strMsg
                        Common.NetMsgbox(Me, "This draft DO has been submitted by other vendor.")
                    Else
                        Common.NetMsgbox(Me, strMsg)
                        strDtl = strMsg
                    End If
                    Pass = False
                    cmdPreviewDO.Visible = False
                    'Response.Redirect("DOMsgAdd.aspx?Frm=" & strFrm & "&status=1&pageid=" & strPageId & "&DONo=" & strDONo & "&HD=Update DO&Dtl=" & strMsg & "&Pass=False")
                End If
            Else
                cmdPreviewDO.Visible = False

                If strMsg <> "" Then
                    strDtl = "Error"
                    lbl_check.Text = strMsg
                Else
                    lbl_check.Text = ""
                End If

                'Response.Redirect("DOMsgAdd.aspx?Frm=" & strFrm & "&status=1&pageid=" & strPageId & "&DONo=&HD=Update DO&Pass=" & Pass)
            End If
        End If

        If ViewState("DONo1") = "" And Pass = False Then
            cmdPreviewDO.Visible = False
        ElseIf ViewState("DONo1") <> "" And Pass = False Then
            If strDtl = "99" Then
                Common.NetMsgbox(Me, "You have already submitted Delivery Order Number " & ViewState("DONo1"))
            End If
            cmdPreviewDO.Visible = False
        Else
        End If
        Me.cmdPreviewDO.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewDO.aspx", "SCoyID=" & Session("CompanyID") & "&DONo=" & ViewState("DONo1") & "&PO_NO=" & Me.Request.QueryString("PONo")) & "')")
        'cmdPreviewDO.Attributes.Add("onclick", "PopWindow('DOReport.aspx?pageid=" & strPageId & "&DONo=" & strDONo & "&SCoyID=" & Session("CompanyID") & "')")
        'If status = "2" Then
        '    cmdPreviewDO.Visible = True
        'Else
        '    cmdPreviewDO.Visible = False
        'End If
        displayAttachFile()
    End Sub

    Private Sub PreviewDO()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT *, (SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_S_STATE) AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS SupplierAddrState," _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_S_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS SupplierAddrCtry, " _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_B_STATE) AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS BillAddrState, " _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_MSTR.POM_B_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS BillAddrCtry," _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_DETAILS.POD_D_STATE) AND (CODE_CATEGORY = 's') AND (CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS DelvAddrState, " _
                        & "(SELECT CODE_DESC FROM CODE_MSTR AS a WHERE (CODE_ABBR = PO_DETAILS.POD_D_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS DelvAddrCtry," _
                        & "(SELECT CM_COY_NAME FROM COMPANY_MSTR AS b WHERE (CM_COY_ID = PO_MSTR.POM_B_COY_ID)) AS BuyerCompanyName " _
                        & "FROM PO_MSTR INNER JOIN PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO AND PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID INNER JOIN DO_MSTR ON PO_MSTR.POM_PO_INDEX = DO_MSTR.DOM_PO_INDEX INNER JOIN DO_DETAILS ON DO_MSTR.DOM_DO_NO = DO_DETAILS.DOD_DO_NO AND DO_MSTR.DOM_S_COY_ID = DO_DETAILS.DOD_S_COY_ID AND PO_DETAILS.POD_PO_LINE = DO_DETAILS.DOD_PO_LINE INNER JOIN COMPANY_MSTR ON PO_MSTR.POM_S_COY_ID = COMPANY_MSTR.CM_COY_ID " _
                        & "WHERE (PO_MSTR.POM_S_COY_ID = @prmCoyID) AND (DO_MSTR.DOM_DO_NO = @prmDONo)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmDONo", ViewState("DONo1")))

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewDO_DataSetPreviewDO", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = appPath & "DO\PreveiwDO-FTN.rdlc"
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
                    Case "freightamt"
                        par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, Me.txtFreAmt.Text)

                    Case Else
                End Select
            Next
            localreport.SetParameters(par)
            localreport.Refresh()

            'Dim deviceInfo As String = _
            ' "<DeviceInfo>" + _
            '     "  <OutputFormat>EMF</OutputFormat>" + _
            '     "  <PageWidth>11in</PageWidth>" + _
            '     "  <PageHeight>8.5in</PageHeight>" + _
            '     "  <MarginTop>0.25in</MarginTop>" + _
            '     "  <MarginLeft>0.25in</MarginLeft>" + _
            '     "  <MarginRight>0.25in</MarginRight>" + _
            '     "  <MarginBottom>0.25in</MarginBottom>" + _
            '     "</DeviceInfo>"
            'Dim deviceInfo As String = _
            '             "<DeviceInfo>" + _
            '                 "  <OutputFormat>EMF</OutputFormat>" + _
            '                 "  <PageWidth>8.27in</PageWidth>" + _
            '                 "  <PageHeight>11in</PageHeight>" + _
            '                 "  <MarginTop>0.25in</MarginTop>" + _
            '                 "  <MarginLeft>0.25in</MarginLeft>" + _
            '                 "  <MarginRight>0.25in</MarginRight>" + _
            '                 "  <MarginBottom>0.25in</MarginBottom>" + _
            '                 "</DeviceInfo>"
            Dim deviceInfo As String = _
                "<DeviceInfo>" + _
                    "  <OutputFormat>EMF</OutputFormat>" + _
                    "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

            Dim fs As New FileStream(appPath & "DO\DOReport.PDF", FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('DOReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
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

    Private Sub cmdsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdsave.Click
        Dim dsDO As DataSet
        Dim PONo As String
        Dim POIndex As Integer
        Dim strDoNo, strMsg As String
        Dim pass As Boolean = True

        strDtl = ""
        lbl_check.Text = ""
        PONo = strPONo
        POIndex = intPOIdx
        strDoNo = lblDONo.Text
        ViewState("DMode") = "Save"
        dsDO = BindDO(pass)

        If ViewState("Mode") = "New" Then
            If pass = True And validateDatagrid(strMsg) Then
                ViewState("ZeroQty") = ""
                If objDO.DONew(dsDO, "Save", strDoNo, strMsg, Session("blnLotNo"), Session("aryLot")) = True Then
                    ViewState("DONo1") = strDoNo
                    If strMsg = "dup" Then
                        'Common.NetMsgbox(Me, MsgTransDup, "SearchDO.aspx?pageid=" & strPageId)
                        Common.NetMsgbox(Me, MsgTransDup, dDispatcher.direct("DO", "SearchDO.aspx", "pageid=" & strPageId))
                    Else
                        'Response.Redirect("DOMsgAdd.aspx?Frm=" & strFrm & "&status=1&pageid=" & strPageId & "&DONo=" & strDoNo & "&HD=Add New DO&Dtl=created&Pass=" & pass)
                        strDtl = "created"
                        Common.NetMsgbox(Me, "Delivery Order Number " & ViewState("DONo1") & " has been created.")
                        lblDONo.Text = ViewState("DONo1")
                        lblDraft.Visible = True
                        'Response.Redirect("AddDO.aspx?Frm=POList&Mode=Edit&PONo=" & strPONo & "&BCoy=" & strBCoyID & "&POIdx=" & intPOIdx & "&pageid=" & strPageId)
                        strMode = "Edit"
                        ViewState("Mode") = strMode
                        Me.cboDelvAdd.Enabled = False

                    End If
                Else
                    If strMsg = "dup" Then
                        'Common.NetMsgbox(Me, MsgTransDup, "SearchDO.aspx?pageid=" & strPageId)
                        Common.NetMsgbox(Me, MsgTransDup, dDispatcher.direct("DO", "SearchDO.aspx", "pageid=" & strPageId))
                    Else
                        'Response.Redirect("DOMsgAdd.aspx?Frm=" & strFrm & "&status=1&pageid=" & strPageId & "&DONo=&HD=Add New DO&Dtl=" & strMsg & "&Pass=False")
                        strDtl = strMsg
                        pass = False
                        Common.NetMsgbox(Me, strMsg)
                    End If
                End If
            Else
                If strMsg = "dup" Then
                    'Common.NetMsgbox(Me, MsgTransDup, "SearchDO.aspx?pageid=" & strPageId)
                    Common.NetMsgbox(Me, MsgTransDup, dDispatcher.direct("DO", "SearchDO.aspx", "pageid=" & strPageId))
                Else
                    If strMsg <> "" Then
                        strDtl = "Error"
                        lbl_check.Text = strMsg
                    Else
                        lbl_check.Text = ""
                    End If
                    'Response.Redirect("DOMsgAdd.aspx?Frm=" & strFrm & "&status=1&pageid=" & strPageId & "&DONo=&HD=Add New DO&Pass=" & pass)
                End If
            End If
        Else
            If pass = True And validateDatagrid(strMsg) Then
                ViewState("ZeroQty") = ""
                If objDO.DOEdit(dsDO, "Save", ViewState("DONo1"), ViewState("BCoyID"), strMsg, Session("blnLotNo"), Session("aryLot")) = True Then
                    'Common.NetMsgbox(Me, "Record Updated")
                    'Response.Redirect("DOMsgAdd.aspx?Frm=" & strFrm & "&status=1&pageid=" & strPageId & "&DONo=" & strDoNo & "&HD=Update DO&Dtl=updated&Pass=" & pass)
                    strDtl = "updated"
                    Common.NetMsgbox(Me, "Delivery Order Number " & ViewState("DONo1") & " has been updated.")
                    cmdsave.Enabled = True
                    cmdDeleteDO.Enabled = False
                    cmdsubmit.Enabled = True
                    cmdReset.Disabled = True
                    Me.cboDelvAdd.Visible = False
                Else
                    'Response.Redirect("DOMsgAdd.aspx?Frm=" & strFrm & "&status=1&pageid=" & strPageId & "&DONo=&HD=Update DO&Dtl=" & strMsg & "&Pass=False")
                    Common.NetMsgbox(Me, strMsg)
                    strDtl = strMsg
                    pass = False
                End If
            Else
                If strMsg <> "" Then
                    strDtl = "Error"
                    lbl_check.Text = strMsg
                Else
                    lbl_check.Text = ""
                End If
                'Response.Redirect("DOMsgAdd.aspx?Frm=" & strFrm & "&status=1&pageid=" & strPageId & "&DONo=&HD=Update DO&Pass=" & pass)
            End If
        End If

        cmdPreviewDO.Visible = False

        If ViewState("DONo1") = "" And pass = False Then
            cmdPreviewDO.Visible = False
        ElseIf ViewState("DONo1") <> "" And pass = False Then
            If strDtl = "99" Then
                Common.NetMsgbox(Me, "You have already submitted Delivery Order Number " & ViewState("DONo1"))
            End If
            cmdPreviewDO.Visible = False
        Else
        End If
        'ViewState("DONo1") = strDoNo
        'Response.Redirect("AddDO.aspx?Frm=POList&Mode=Edit&PONo=" & strPONo & "&BCoy=" & strBCoyID & "&POIdx=" & intPOIdx & "&pageid=" & strPageId)
        '    lnkPONo.NavigateUrl = "AddDO.aspx?Frm=POList&Mode=Edit&PONo=" & dv("POM_PO_No") & "&BCoy=" & dv("CM_COY_NAME") & "&POIdx=" & dv("POM_PO_Index") & "&pageid=" & strPageId

    End Sub

#End Region
#Region " private function "

    Private Sub FillDelvAdd(ByVal dtHeader As DataTable, ByVal CboAdd As Boolean)
        'Dim dtHeader As New DataTable
        Dim strDevlAdd As String
        'If Not dsAllInfo Is Nothing Then
        If Not dtHeader Is Nothing Then
            '//remark by Moo
            'If strMode = "Edit" Then
            '    dtHeader = dsAllInfo.Tables("PO_DETAILS")
            '    GoTo startFillAdd
            'End If
            'dtHeader = dsAllInfo.Tables("PO_DETAILS")
            'dtHeader = dsAllInfo.Tables(0)
            'If dtHeader.Rows.Count > 0 And dtHeader.Rows.Count = 1 Then
            'startFillAdd:
            strDevlAdd = Common.parseNull(dtHeader.Rows(0)("POD_D_Addr_Line1"))
            If Not IsDBNull(dtHeader.Rows(0)("POD_D_Addr_Line2")) AndAlso dtHeader.Rows(0)("POD_D_Addr_Line2") <> "" Then
                strDevlAdd = strDevlAdd & "<BR>" & dtHeader.Rows(0)("POD_D_Addr_Line2")
            End If

            If Not IsDBNull(dtHeader.Rows(0)("POD_D_Addr_Line3")) AndAlso dtHeader.Rows(0)("POD_D_Addr_Line3") <> "" Then
                strDevlAdd = strDevlAdd & "<BR>" & dtHeader.Rows(0)("POD_D_Addr_Line3")
            End If

            If Not IsDBNull(dtHeader.Rows(0)("POD_D_PostCode")) Then
                strDevlAdd = strDevlAdd & "<BR>" & dtHeader.Rows(0)("POD_D_PostCode")
            End If

            If Not IsDBNull(dtHeader.Rows(0)("POD_D_City")) Then
                strDevlAdd = strDevlAdd & " " & dtHeader.Rows(0)("POD_D_City")
            End If

            If Not IsDBNull(dtHeader.Rows(0)("POD_D_State_desc")) Then
                strDevlAdd = strDevlAdd & "<BR>" & dtHeader.Rows(0)("POD_D_State_desc")
            End If

            If Not IsDBNull(dtHeader.Rows(0)("POD_D_Country_desc")) Then
                strDevlAdd = strDevlAdd & " " & dtHeader.Rows(0)("POD_D_Country_desc")
            End If

            ViewState("DelAdd") = strDevlAdd
            lblDelvAdd.Text = strDevlAdd
            lblDelvAdd.Visible = True
            If CboAdd <> True Then
                cboDelvAdd.Visible = False
            Else
                cboDelvAdd.Visible = True
            End If

            'dtDOMstr.Columns.Add("POD_D_Addr_Line1", Type.GetType("System.String"))
            'dtDOMstr.Columns.Add("POD_D_Addr_Line2", Type.GetType("System.String"))
            'dtDOMstr.Columns.Add("POD_D_Addr_Line3", Type.GetType("System.String"))
            'dtDOMstr.Columns.Add("POD_D_State", Type.GetType("System.String"))
            'dtDOMstr.Columns.Add("POD_D_Country", Type.GetType("System.String"))
            'dtDOMstr.Columns.Add("POD_D_PostCode", Type.GetType("System.String"))
            'dtDOMstr.Columns.Add("POD_D_City", Type.GetType("System.String"))
            'dtDOMstr.Columns.Add("POD_B_COY_ID", Type.GetType("System.String"))
            ViewState("POD_D_Addr_Line1") = Common.parseNull(dtHeader.Rows(0)("POD_D_Addr_Line1"))
            ViewState("POD_D_Addr_Line2") = Common.parseNull(dtHeader.Rows(0)("POD_D_Addr_Line2"))
            ViewState("POD_D_Addr_Line3") = Common.parseNull(dtHeader.Rows(0)("POD_D_Addr_Line3"))
            ViewState("POD_D_State") = Common.parseNull(dtHeader.Rows(0)("POD_D_State"))
            ViewState("POD_D_Country") = Common.parseNull(dtHeader.Rows(0)("POD_D_Country"))
            ViewState("POD_D_PostCode") = Common.parseNull(dtHeader.Rows(0)("POD_D_PostCode"))
            ViewState("POD_D_City") = Common.parseNull(dtHeader.Rows(0)("POD_D_City"))
            ViewState("POD_B_COY_ID") = Common.parseNull(dtHeader.Rows(0)("POD_COY_ID"))
            '//remark by Moo
            '       ElseIf dtHeader.Rows.Count > 1 Then
            '          lblDelvAdd.Visible = False
            '         cboDelvAdd.Visible = True
            '    End If
        End If

    End Sub

    Private Function FuncInvisible(Optional ByVal strCallFrom As String = "PO")
        '//remark by Moo
        'lblPayTerm.Visible = False
        'lblPayMthd.Visible = False
        'lblShipTerm.Visible = False
        ''lblPayMthd.Visible = False
        'tr1.Style("display") = "none"
        'tr2.Style("display") = "none"
        'tr3.Style("display") = "none"
        'tr4.Style("display") = "none"
        'tr5.Style("display") = "none"
        'tr6.Style("display") = "none"
        'tr1.Attributes.Add("class", "tablecol")


        'lblDelvAdd.Text = ""
        'lblSummPO.Visible = False
        'lblPONum.Visible = False
        If strCallFrom = "PO" Then 'called when PO dropdownlist is selected
            cboDelvAdd.Visible = False
        End If
        '//add by Moo
        cmdsave.Enabled = False
        cmdsubmit.Enabled = False
        cmdReset.Disabled = True
        'dtgDODtl.Visible = False
        'lblSummPO.Visible = False
        'lblPONum.Visible = False
        'DtgDoSumm.Visible = False
    End Function
    Private Function FuncVisible()
        'lblPayTerm.Visible = True
        'lblPayMthd.Visible = True
        'lblShipTerm.Visible = True
        'lblPayMthd.Visible = True
        'tr1.Style("display") = ""
        'tr2.Style("display") = ""
        'tr3.Style("display") = ""
        'tr4.Style("display") = ""
        'tr5.Style("display") = ""
        'tr6.Style("display") = ""
    End Function
    Private Function BindDO(ByRef pass As Boolean) As DataSet
        Dim ds As New DataSet
        Dim dsAllInfo As New DataSet
        Dim i, j, intShipZeroCnt As Integer
        i = 1
        j = 0
        intShipZeroCnt = 0
        Dim POIndex As Integer
        Dim PONo As String
        Dim AddrCode As String
        POIndex = intPOIdx
        PONo = strPONo
        AddrCode = cboDelvAdd.SelectedItem.Text
        ' columns for DO_MSTR table
        Dim dtDOMstr As New DataTable

        lbl_check.Text = ""

        dtDOMstr.Columns.Add("DOM_DO_Date", Type.GetType("System.DateTime"))
        dtDOMstr.Columns.Add("DOM_S_Ref_No", Type.GetType("System.String"))
        dtDOMstr.Columns.Add("DOM_S_REF_DATE", Type.GetType("System.String"))
        dtDOMstr.Columns.Add("DOM_PO_INDEX", Type.GetType("System.Double"))
        dtDOMstr.Columns.Add("DOM_D_Addr_Code", Type.GetType("System.String"))
        dtDOMstr.Columns.Add("DOM_WAYBILL_NO", Type.GetType("System.String"))
        dtDOMstr.Columns.Add("DOM_FREIGHT_CARRIER", Type.GetType("System.String"))
        dtDOMstr.Columns.Add("DOM_FREIGHT_AMT", Type.GetType("System.String"))
        dtDOMstr.Columns.Add("DOM_DO_REMARKS", Type.GetType("System.String"))
        dtDOMstr.Columns.Add("DOM_CREATED_DATE", Type.GetType("System.DateTime"))
        dtDOMstr.Columns.Add("POD_PO_NO", Type.GetType("System.String"))
        dtDOMstr.Columns.Add("POD_D_Addr_Line1", Type.GetType("System.String"))
        dtDOMstr.Columns.Add("POD_D_Addr_Line2", Type.GetType("System.String"))
        dtDOMstr.Columns.Add("POD_D_Addr_Line3", Type.GetType("System.String"))
        dtDOMstr.Columns.Add("POD_D_State", Type.GetType("System.String"))
        dtDOMstr.Columns.Add("POD_D_Country", Type.GetType("System.String"))
        dtDOMstr.Columns.Add("POD_D_PostCode", Type.GetType("System.String"))
        dtDOMstr.Columns.Add("POD_D_City", Type.GetType("System.String"))
        dtDOMstr.Columns.Add("POD_B_COY_ID", Type.GetType("System.String"))
        'POM_B_Coy_ID
        Dim dtr As DataRow
        dtr = dtDOMstr.NewRow()
        dtr("DOM_DO_Date") = lblDevlDate.Text
        dtr("DOM_S_Ref_No") = txtOurRefNo.Text
        dtr("DOM_S_REF_DATE") = txtOurRefDate.Text
        dtr("DOM_PO_INDEX") = POIndex
        dtr("DOM_D_Addr_Code") = AddrCode

        'dsAllInfo = objDO.filterDevlAdd(cboDelvAdd, PONo, POIndex)
        dtr("POD_D_Addr_Line1") = ViewState("POD_D_Addr_Line1")
        dtr("POD_D_Addr_Line2") = ViewState("POD_D_Addr_Line2")
        dtr("POD_D_Addr_Line3") = ViewState("POD_D_Addr_Line3")
        dtr("POD_D_State") = ViewState("POD_D_State")
        dtr("POD_D_Country") = ViewState("POD_D_Country")
        dtr("POD_D_PostCode") = ViewState("POD_D_PostCode")
        dtr("POD_D_City") = ViewState("POD_D_City")
        dtr("POD_B_COY_ID") = ViewState("POD_B_COY_ID")

        dtr("DOM_WAYBILL_NO") = txtAWBillNo.Text
        dtr("DOM_FREIGHT_CARRIER") = txtFreCarier.Text
        dtr("DOM_FREIGHT_AMT") = txtFreAmt.Text
        dtr("DOM_DO_REMARKS") = txtRemarks.Text
        dtr("DOM_CREATED_DATE") = lblDevlDate.Text
        dtr("POD_PO_NO") = PONo
        'dtr("POD_B_COY_ID") = dsAllInfo.Tables(0).Rows(0)("POM_B_Coy_ID")
        dtDOMstr.Rows.Add(dtr)
        ds.Tables.Add(dtDOMstr)

        Dim dtDODtls As New DataTable
        'DOD_S_COY_ID,DOD_DO_NO,DOD_DO_LINE,DOD_PO_LINE,DOD_DO_QTY,DOD_SHIPPED_QTY,DOD_REMARKS
        dtDODtls.Columns.Add("DOD_DO_LINE", Type.GetType("System.String"))
        dtDODtls.Columns.Add("DOD_PO_LINE", Type.GetType("System.String"))
        dtDODtls.Columns.Add("DOD_DO_QTY", Type.GetType("System.String"))
        dtDODtls.Columns.Add("DOD_SHIPPED_QTY", Type.GetType("System.String"))
        dtDODtls.Columns.Add("DOD_REMARKS", Type.GetType("System.String"))
        dtDODtls.Columns.Add("DOD_Outstanding", Type.GetType("System.String"))
        dtDODtls.Columns.Add("DOD_LotNo", Type.GetType("System.String"))


        Dim dgItem As DataGridItem
        For Each dgItem In dtgDODtl.Items
            Dim txtShip As TextBox
            Dim intShipQty, intOutStnd As Decimal

            j += 1
            txtShip = dgItem.FindControl("txtShiped")
            intShipQty = CDec(txtShip.Text)
            intOutStnd = CDec(dgItem.Cells(9).Text)

            '//remark by Moo, check using validator
            'If intShipQty < 0 Or intShipQty > intOutStnd Then
            '    If intShipQty > intOutStnd Then
            '        Common.NetMsgbox(Me, " Shipped Quantity Should not be greater than the Outstanding Quantity")
            '        pass = False
            '    End If
            '    'If intOutStnd = 0 Then
            '    '    pass = False
            '    'End If
            '    Exit Function
            'End If



            '//only save item when ship> 0
            If intShipQty > 0 Then
                dtr = dtDODtls.NewRow
                dtr("DOD_PO_LINE") = dgItem.Cells(0).Text
                dtr("DOD_DO_LINE") = i
                dtr("DOD_DO_QTY") = dgItem.Cells(8).Text

                dtr("DOD_SHIPPED_QTY") = Common.parseNull(txtShip.Text, 0) 'cell(10)
                Dim txtRemark As TextBox
                txtRemark = dgItem.FindControl("txtDtlRemarks")
                dtr("DOD_REMARKS") = txtRemark.Text 'Cell(11)

                If Session("blnLotNo") = True Then
                    Dim lblLotNo As Label
                    lblLotNo = dgItem.FindControl("lblLotNo")
                    dtr("DOD_LotNo") = lblLotNo.Text
                End If

                i = i + 1
                dtDODtls.Rows.Add(dtr)
            Else
                intShipZeroCnt += 1
            End If
        Next

        If intShipZeroCnt = j Then
            pass = False
            ViewState("ZeroQty") = "T"
            If ViewState("DMode") = "Submit" Then
                Common.NetMsgbox(Me, "Please enter Ship quantity.")
                pass = False
            End If
        End If

        'If intShipZeroCnt > 0 Then
        '    If ViewState("DMode") = "Submit" Then
        '        ViewState("ZeroQty") = "T"
        '        Common.NetMsgbox(Me, "Please enter Ship quantity.")
        '        pass = False
        '    End If

        'End If

        ds.Tables.Add(dtDODtls)
        BindDO = ds
    End Function
#End Region

    Private Sub cmdDeleteDO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDeleteDO.Click
        Dim strMsg As String
        strMsg = objDO.DeleteDO(lblDONo.Text)
        If strFrm = "POList" Then
            'Common.NetMsgbox(Me, strMsg, "POList.aspx?pageid=" & strPageId)
            Common.NetMsgbox(Me, strMsg, dDispatcher.direct("DO", "POList.aspx", "pageid=" & strPageId))
        Else
            'Common.NetMsgbox(Me, strMsg, "searchDO.aspx?pageid=" & strPageId)
            Common.NetMsgbox(Me, strMsg, dDispatcher.direct("DO", "searchDO.aspx", "pageid=" & strPageId))
        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Session("w_AddDO_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn_selected"" href=""POList.aspx?pageid=" & strPageId & """><span>Issue DO</span></a></li>" & _
        '                            "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""searchDO.aspx?pageid=" & strPageId & """><span>DO Listing</span></a></li>" & _
        '                            "<li><div class=""space""></div></li>" & _
        '             "</ul><div></div></div>"
        'Session("w_AddDO_tabs") = "<div class=""t_entity"">" & _
        '     "<a class=""t_entity_btn_selected"" href=""POList.aspx?pageid=" & strPageId & """><span>Issue DO</span></a>" & _
        '     "<a class=""t_entity_btn"" href=""searchDO.aspx?pageid=" & strPageId & """><span>DO Listing</span></a>" & _
        '     "</div>"
        Session("w_AddDO_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DO", "POList.aspx", "pageid=" & strPageId) & """><span>Issue DO</span></a></li>" & _
            "<li><div class=""space""></div></li>" & _
          "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DO", "searchDO.aspx", "pageid=" & strPageId) & """><span>DO Listing</span></a></li>" & _
          "<li><div class=""space""></div></li>" & _
            "</ul><div></div></div>"
    End Sub

    'Protected Sub cmdPreviewDO_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPreviewDO.ServerClick
    '    PreviewDO()

    'End Sub
    Private Sub btnhidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhidden.Click
        Dim dgItem As DataGridItem
        'Bindgrid()

        Dim aryTemp As New ArrayList()
        Dim i, found, iError As Integer
        Dim dsTemp As New DataSet
        aryTemp = Session("aryLot")

        'found = 0
        iError = 0
        lbl_check.Text = ""
        If Not aryTemp Is Nothing Then
            For Each dgItem In dtgDODtl.Items
                found = 0

                For i = 0 To aryTemp.Count - 1
                    If aryTemp(i)(8) = dgItem.Cells(EnumAddDO.lineno).Text Then
                        If aryTemp(i)(0) <> "" And aryTemp(i)(1) <> "" And aryTemp(i)(2) <> "" And aryTemp(i)(3) <> "" Then
                            If aryTemp(i)(6) = dgItem.Cells(EnumAddDO.VendorItemCode).Text And aryTemp(i)(8) = dgItem.Cells(EnumAddDO.lineno).Text Then
                                found = found + 1
                            End If
                        End If
                    End If

                Next

                If found > 0 Then
                    CType(dgItem.FindControl("lblLotNo"), Label).Text = found
                Else
                    dsTemp = objDO.getDOLot(strDONo, dgItem.Cells(EnumAddDO.VendorItemCode).Text, dgItem.Cells(EnumAddDO.poLine).Text)
                    If dsTemp.Tables(0).Rows.Count > 0 Then
                        CType(dgItem.FindControl("lblLotNo"), Label).Text = dsTemp.Tables(0).Rows.Count
                    Else
                        CType(dgItem.FindControl("lblLotNo"), Label).Text = "0"
                    End If
                End If

            Next

            'For Each dgItem In dtgDODtl.Items

            '    For i = 0 To aryTemp.Count - 1
            '        If aryTemp(i)(8) = dgItem.Cells(EnumAddDO.lineno).Text Then
            '            If aryTemp(i)(0) <> "" And aryTemp(i)(1) <> "" And aryTemp(i)(2) <> "" And aryTemp(i)(3) <> "" Then
            '                If aryTemp(i)(6) = dgItem.Cells(EnumAddDO.VendorItemCode).Text And aryTemp(i)(8) = dgItem.Cells(EnumAddDO.lineno).Text Then
            '                    found = found + 1
            '                End If
            '            End If
            '        Else
            '            iError = iError + 1
            '        End If

            '    Next

            '    If iError >= found Then
            '        dsTemp = objDO.getDOLot(strDONo, dgItem.Cells(EnumAddDO.VendorItemCode).Text)
            '        If dsTemp.Tables(0).Rows.Count > 0 Then
            '            CType(dgItem.FindControl("lblLotNo"), Label).Text = dsTemp.Tables(0).Rows.Count
            '        Else
            '            CType(dgItem.FindControl("lblLotNo"), Label).Text = "0"
            '        End If
            '    Else
            '        CType(dgItem.FindControl("lblLotNo"), Label).Text = found
            '    End If

            'Next
        Else
            For Each dgItem In dtgDODtl.Items
                If ViewState("Mode") = "Edit" Then
                    dsTemp = objDO.getDOLot(strDONo, dgItem.Cells(EnumAddDO.VendorItemCode).Text, dgItem.Cells(EnumAddDO.poLine).Text)
                    If dsTemp.Tables(0).Rows.Count > 0 Then
                        CType(dgItem.FindControl("lblLotNo"), Label).Text = dsTemp.Tables(0).Rows.Count
                    Else
                        CType(dgItem.FindControl("lblLotNo"), Label).Text = "0"
                    End If
                Else
                    CType(dgItem.FindControl("lblLotNo"), Label).Text = "0"
                End If
            Next
        End If

    End Sub
End Class
