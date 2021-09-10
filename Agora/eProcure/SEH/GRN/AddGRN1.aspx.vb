Imports AgoraLegacy
Imports eProcure.Component

Imports System.Data
Imports System.Drawing
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
Public Class AddGRN1SEH
    Inherits AgoraLegacy.AppBaseClass
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents lblDONo1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblDONum As System.Web.UI.WebControls.Label
    'Protected WithEvents lblPONo As System.Web.UI.WebControls.Label
    Protected WithEvents txtReceivedDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblSummPO As System.Web.UI.WebControls.Label
    Protected WithEvents lblPONum As System.Web.UI.WebControls.Label
    Protected WithEvents DtgGRNSumm As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdAcceptAll As System.Web.UI.WebControls.Button
    Protected WithEvents cmdRejectAll As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents VldSumQty As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents DivGrn As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents DODtl As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents PODtl As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents cmdReset As System.Web.UI.WebControls.Button
    Protected WithEvents back As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdPreviewPO As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidSummary As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidControl As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtPO As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents revPONo As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_check As System.Web.UI.WebControls.Label
    Protected WithEvents lblFileAttach As System.Web.UI.WebControls.Label
    Protected WithEvents btnhidden As System.Web.UI.WebControls.Button
    Dim strMode, strDONo, strLocID, strBCoyID, strPONo, strFrm, strPODate, strtemp As String
    Dim intDOIdx As Integer

    Dim strVendor As String
    Dim objDO As New DeliveryOrder
    Dim objDb2 As New EAD.DBCom
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim objGRN As New GRN
    Dim objGRN_Ext As New GRN_Ext
    Dim strType As String
    Dim intItemRow As Integer = 0
    Dim blnButtonState As Boolean
    Dim blnRejectAll As Boolean
    Dim blnErr, blnReported As Boolean
    Dim strErr As String
    Dim dDispatcher = New dispatcher
    Dim objINV As New Inventory
    Dim objGLO As New AppGlobals
    Dim objFile As New FileManagement
    Dim TotalCIF As Double = 0
    Dim TotalLandCost As Double = 0
    'Dim cmdSub As HtmlInputButton

    'Protected WithEvents cmdSub As System.Web.UI.HtmlControls.HtmlInputButton

    'Public Enum EnumGRN
    '    icPOLine
    '    '----New column added in the Grig "dtgGRNDtl" By Praveen on 18/07/2007
    '    icLineNo '- Michelle (3/9/2007) - To keep the original PO Line no.
    '    '----End
    '    icBuyerItemCode
    '    icVendorItemCode
    '    icDescription
    '    icUOM
    '    icMPQ
    '    icOrdered
    '    icOutstanding
    '    icReceivedQty
    '    icRejQty1stLevel
    '    icRejQty
    '    icLocation
    '    icSubLocation
    '    icRemarks
    '    icShipQty
    'End Enum

    Public Enum EnumGRNStk
        icPOLine
        '----New column added in the Grig "dtgGRNDtl" By Praveen on 18/07/2007
        icLineNo '- Michelle (3/9/2007) - To keep the original PO Line no.
        '----End
        icBuyerItemCode
        icVendorItemCode
        icDescription
        icUOM
        icGLCode
        icCurrency
        icPercentFac
        icExchangeRate
        icUnitPrice
        icDiscPrice
        icOthCharges
        icUnitCost
        icAmountF
        icAmountM
        icGRNFactor
        icInlandCharges
        icCIF
        icDuties
        icLandCost
        icSpec1
        icSpec2
        icSpec3
        icLotNo
        icMPQ
        icOrdered
        icOutstanding
        icReceivedQty
        icRejQty1stLevel
        icRejQty
        icLocation
        icSubLocation
        icRemarks
        icShipQty
    End Enum

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdRejectAll.Enabled = False
        cmdSubmit.Enabled = False
        cmdAcceptAll.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdRejectAll)
        alButtonList.Add(cmdAcceptAll)
        alButtonList.Add(cmdSubmit)
        alButtonList.Add(cmdReset)
        htPageAccess.Add("update", alButtonList)
        htPageAccess.Add("add", alButtonList)
        'yAP: 21May2014; 
        'Comment out this portion, this to avoid error page encouter when checking on strPageID when check on Menu
        ''If Request.QueryString("Frm") <> "Dashboard" Then
        ''    CheckButtonAccess()
        ''End If
        If ViewState("Mode") = "New" Then
            cmdSubmit.Enabled = True
            cmdReset.Enabled = True
        ElseIf ViewState("Mode") = "GRN" Then
            cmdSubmit.Enabled = True
            cmdReset.Enabled = True
        Else
            cmdSubmit.Enabled = False
            cmdReset.Enabled = False
        End If
        '//Button state may change at many event (PO DDL change, DO DDL change, no data)
        '//so if the existing button state is false, need to double check because Button state 
        '//may be set to true by CheckButtonAccess
        If Not ViewState("blnButtonState") Then
            ButtonProperty(False)
        End If

        If Session("blnRate") = False Then
            cmdSubmit.Enabled = False
        End If
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'Session("CompanyID") = "demo"
        'Session("UserId") = "moofh"

        MyBase.Page_Load(sender, e)

        blnPaging = False
        'SetGridProperty(dtgGRNDtl)
        SetGridProperty(dtgGRNDtStk)
        SetGridProperty(DtgGRNSumm)
        strType = Me.Request.QueryString("Type")
        ViewState("GrnType") = strType
        strPageId = Session("strPageId")
        strDONo = Me.Request.QueryString("DONo")
        'strLocID = Me.Request.QueryString("LocID")
        intDOIdx = Me.Request.QueryString("DOIndex")
        'intPOIdx = Me.Request.QueryString("POIdx")
        strPONo = Me.Request.QueryString("PONo")
        'strBCoyID = Me.Request.QueryString("BCoy")
        strPODate = objDb2.GetVal("SELECT DATE_FORMAT(POM_PO_DATE, '%d/%m/%Y') AS POM_PO_DATE FROM PO_MSTR WHERE POM_PO_NO = '" & Common.Parse(strPONo) & "' AND POM_B_COY_ID = '" & Session("CompanyId") & "'")
        strFrm = Me.Request.QueryString("Frm")
        lblDONum.Text = strDONo
        lblPONo.Text = strPONo
        lblPODate.Text = strPODate

        Dim LocDesc As String
        Dim SubLocDesc As String
        Dim LocDesc2 As String
        Dim SubLocDesc2 As String

        objINV.GetLocationDesc(LocDesc, SubLocDesc)

        lblDL.Text = IIf(LocDesc = "", "Default Location :", "Default " + LocDesc + " :")
        lblSDL.Text = IIf(SubLocDesc = "", "Default Sub Location :", "Default " + SubLocDesc + " :")

        strVendor = Me.Request.QueryString("Vendor")
        lblVendor.Text = strVendor
        'txtReceivedDate.Text = Today()

        objINV.GetDefaultLocationDesc(LocDesc2, SubLocDesc2)
        lblDefaultLocation.Text = LocDesc2
        lblDefaultSubLocation.Text = SubLocDesc2


        If strFrm = "GRNList" Or strFrm = "GRNDetails" Then
            ViewState("strtemp") = "AddGRN1"
            'lnkBack.NavigateUrl = "GRNList.aspx?Frm=" & ViewState("strtemp") & "&pageid=" & strPageId
            lnkBack.NavigateUrl = dDispatcher.direct("GRN", "GRNList.aspx", "Frm=" & ViewState("strtemp") & "&pageid=" & strPageId)
            'Michelle (30/9/2010) - To link back to Dashboard if calling page is from Dashboard
        ElseIf strFrm = "Dashboard" Then
            ViewState("strtemp") = "Dashboard"
            'lnkBack.NavigateUrl = "../Dashboard/StoreKeeper.aspx?Frm=" & ViewState("strtemp") & "&pageid=0"
            lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & ViewState("strtemp") & "&pageid=0")
            'ElseIf strFrm = "GRNDetails" Then
            '    lnkBack.NavigateUrl = "GRNDetails.aspx?pageid=" & strPageId
        Else
            ViewState("strtemp") = ""
            'lnkBack.NavigateUrl = "GRNSearch.aspx?pageid=" & strPageId
            lnkBack.NavigateUrl = dDispatcher.direct("GRN", "GRNSearch.aspx", "pageid=" & strPageId)
        End If

        If Not Page.IsPostBack Then
            Session("blnRate") = True
            ViewState("blnSpGRN") = True 'To indicate whether all the items are Spot item (non inventory items)
            Session("arySetLocation") = Nothing

            PopulatePODDL(lblPONo.Text)
            DODDL_Change()

            GenerateTab()
            strMode = Me.Request.QueryString("mode")
            ViewState("Mode") = strMode

            If Session("blnRate") = False Then
                lbl_check.Text = "User is not allowed to issue GRN due to invalid / expired exchange rate."
                'Common.NetMsgbox(Me, "User is not allowed to issue GRN due to invalid / expired exchange rate.", MsgBoxStyle.Information)
            End If
        End If

        If UCase(strType) = "GRNACK" Then
            'lblTitle.Text = "Goods Receipt Note Acknowledgement"
            lblHeader.Text = "Goods Receipt Note Acknowledgement"
        Else
            'lblTitle.Text = "Goods Receipt Note Generation"
            lblHeader.Text = "Goods Receipt Note Generation"
        End If


        'cmdSubmit.Attributes.Add("onClick", "return resetSummary(0,1);")
        'cmdClear.CausesValidation = False

        vldSumm.Enabled = False
        vldSumm.Style("display") = "none"
        'Me.dtgGRNDtl.Columns(6).Visible = False

        'Michelle (23/1/2013)- Issue 1727
        Dim dvFile As DataView
        Dim intLoop, intCount As Integer
        Dim strFile, strFile1, strURL, strTemp As String
        dvFile = objDO.getDOAttachment(strDONo, ViewState("VCoyID")).Tables(0).DefaultView
        If dvFile.Count > 0 Then
            For intLoop = 0 To dvFile.Count - 1
                strFile = dvFile(intLoop)("CDDA_ATTACH_FILENAME")
                strFile1 = dvFile(intLoop)("CDDA_HUB_FILENAME")
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DOAttachment, "DO", EnumUploadFrom.FrontOff, , ViewState("VCoyID"))
                If strTemp = "" Then
                    strTemp = intCount + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDDA_FILESIZE") & "KB)"
                    intCount = intCount + 1
                Else
                    strTemp = strTemp & "<BR>&nbsp;" & intCount + 1 & ") " & strURL & " (" & dvFile(intLoop)("CDDA_FILESIZE") & "KB)"
                    intCount = intCount + 1
                End If

            Next
        Else
            strTemp = "No Files Attached"
        End If
        lblFileAttach.Text = strTemp

    End Sub

    Private Sub PopulatePODDL(ByVal strPONo As String)
        Dim ds As DataSet
        Dim blnValid As Boolean
        Dim strMsg As String
        If UCase(strType) = "GRNACK" Then
            'strMsg = "No Access to the PO Item's Delivery Address or"
            'strMsg &= """&vbCrLf&"""
            'strMsg &= "No GRN has been created for this PO."
            ds = objGRN.getPOListForGRNAck(lblPONo.Text, blnValid, strMsg)
        Else
            'strMsg = "No Access to the PO Item's Delivery Address or"
            'strMsg &= """&vbCrLf&"""
            'strMsg &= "No DO has been created for this PO."
            ds = objGRN.getPOListForOutsDO(lblPONo.Text, blnValid, strMsg)
        End If

        If blnValid Then
            If Not ds Is Nothing Then
                'Common.FillDdl(cboPONo, "POM_PO_No", "POM_PO_Index", ds)
                If ds.Tables(0).Rows.Count > 0 Then
                    ViewState("PoNo") = ds.Tables(0).Rows(0)("POM_PO_No")
                    ViewState("PoIndex") = ds.Tables(0).Rows(0)("POM_PO_Index")
                    ' ai chu modified on 26/09/2005
                    ' no need to fill Delivery Order dropdownlist if no GRN to be created from DO
                    ViewState("DOExist") = ds.Tables(0).Rows(0)("DoExists")
                    PODDL_Change(ds.Tables(0).Rows(0)("POM_PO_No"), ds.Tables(0).Rows(0)("POM_PO_Index"))
                    If ds.Tables(0).Rows(0)("DoExists") = 0 Then
                        If strMsg = "No Access to the PO Item's Delivery Address" Then
                            If strFrm = "GRNList" Or strFrm = "GRNDetails" Then
                                Common.NetMsgbox(Me, "No Access to the PO Item's Delivery Address.", dDispatcher.direct("GRN", "GRNList.aspx", "Frm=" & ViewState("strtemp") & "&pageid=" & strPageId))
                            ElseIf strFrm = "Dashboard" Then
                                Common.NetMsgbox(Me, "No Access to the PO Item's Delivery Address.", dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & ViewState("strtemp") & "&pageid=0"))
                            Else
                                Common.NetMsgbox(Me, "No Access to the PO Item's Delivery Address.", dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & ViewState("strtemp") & "&pageid=0"))
                            End If
                        Else
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        End If
                    End If
                End If
            End If
        Else
            Common.NetMsgbox(Me, "Invalid PO No.", MsgBoxStyle.Information)
            'NoPoSelected()
        End If

        'Dim lstItem As New ListItem
        'lstItem.Value = 0
        'lstItem.Text = "---Select---"
        'cboPONo.Items.Insert(0, lstItem)
    End Sub

    'Private Sub NoPoSelected()
    '    PODtl.Style("display") = "none"
    '    DODtl.Style("display") = "none"
    '    cmdPreviewPO.Disabled = True
    '    ButtonProperty(False)
    '    cboDONo.Items.Clear()
    '    Dim lstItem As New ListItem
    '    lstItem.Value = 0
    '    lstItem.Text = "---Select---"
    '    cboDONo.Items.Insert(0, lstItem)
    'End Sub

    Private Sub PODDL_Change(ByVal PoNo As String, ByVal POIdx As Integer)
        Dim intTotRecord As Integer
        Dim DONo As String
        Dim dsDO As DataSet
        'PONo = cboPONo.SelectedItem.Text
        'POIdx = cboPONo.SelectedItem.Value
        'If PONo = "---Select---" Then
        '    'Common.NetMsgbox(Me, "Please select a PO Number")
        '    PODtl.Style("display") = "none"
        '    DODtl.Style("display") = "none"
        '    cmdPreviewPO.Disabled = True
        '    ButtonProperty(False)
        '    cboDONo.Items.Clear()
        '    Dim lstItem As New ListItem
        '    lstItem.Value = 0
        '    lstItem.Text = "---Select---"
        '    cboDONo.Items.Insert(0, lstItem)
        'Else
        ShowGRNSummaryForPO(POIdx, PoNo)

        ' ai chu modified on 26/09/2005
        If ViewState("DOExist") <> 0 Then
            'cboDONo.Items.Clear()
            If UCase(ViewState("GrnType")) = "GRNACK" Then
                dsDO = objGRN.getDOListForGRNAck(POIdx)
            Else
                dsDO = objGRN.getDOListForGRNLevel1(POIdx)
            End If

            If Not dsDO Is Nothing Then
                'Common.FillDdl(cboDONo, "DOM_Do_No", "DOM_DO_INDEX", dsDO)
            End If

            'Dim lstItem As New ListItem
            'lstItem.Value = 0
            'lstItem.Text = "---Select---"
            'cboDONo.Items.Insert(0, lstItem)
        End If

        'Me.cmdPreviewPO.Attributes.Add("onclick", "window.open('../PO/POReport.aspx?pageid=" & strPageId & "&po_no=" & PoNo & "&side=b&BCoyID=" & Session("CompanyID") & "')")
        Me.cmdPreviewPO.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewPO.aspx", "PO_No=" & PoNo & "&BCoyID=" & Session("CompanyID")) & "')")

        PODtl.Style("display") = "inline"
        DODtl.Style("display") = "none"

        cmdPreviewPO.Disabled = False
        ButtonProperty(False)

        'If dsDO.Tables(0).Rows.Count = 0 Then
        '    'Common.NetMsgbox(Me, MsgNoRecord)
        '    lblSummPO.Visible = False
        '    lblPONum.Visible = False
        'Else
        '    lblSummPO.Visible = True
        '    lblPONum.Visible = True
        'End If
        'End If
    End Sub

    'Private Sub cboPONo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPONo.SelectedIndexChanged
    '    PODDL_Change()
    'End Sub

    Sub ShowGRNSummaryForPO(ByVal POIdx As Integer, ByVal PONo As String)
        Dim dvViewGRNSumm As DataView
        Dim dsGRNSumm As DataSet
        Dim strGrnNo As String = ""
        Dim intTotRecord As Integer

        dsGRNSumm = objGRN.GetGRNSumm(POIdx, strGrnNo)
        dvViewGRNSumm = dsGRNSumm.Tables(0).DefaultView
        dvViewGRNSumm.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewGRNSumm.Sort += " DESC"

        intTotRecord = dsGRNSumm.Tables(0).Rows.Count
        intPageRecordCnt = intTotRecord
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        PODtl.Style("display") = "inline"
        lblSummPO.Visible = True
        lblPONum.Visible = True
        lblPONum.Text = PONo
        If intTotRecord > 0 Then
            'intTotPage = dtgDO.PageCount           
            DtgGRNSumm.DataSource = dvViewGRNSumm
            hidControl.Value = ""
            hidSummary.Value = ""
            lblSummPO.Visible = True
            lblPONum.Visible = True
            DtgGRNSumm.DataBind()
        Else
            lblSummPO.Visible = False
            lblPONum.Visible = False
            DtgGRNSumm.DataBind()
        End If
    End Sub
    'Public Sub dtgDODtl_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
    '    dtgGRNDtl.CurrentPageIndex = e.NewPageIndex
    '    'Bindgrid()
    'End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        'dtgGRNDtl.CurrentPageIndex = 0
        dtgGRNDtStk.CurrentPageIndex = 0
        DODDL_Change()
    End Sub

    'Sub DtgGRNSumm_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles DtgGRNSumm.PageIndexChanged
    '    DtgGRNSumm.CurrentPageIndex = e.NewPageIndex
    '    'Bindgrid()
    'End Sub

    Sub SortCommand2_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DtgGRNSumm.SortCommand
        Grid_SortCommand(sender, e)
        DtgGRNSumm.CurrentPageIndex = 0
        ShowGRNSummaryForPO(ViewState("PoIndex"), ViewState("PoNo"))
    End Sub
    Private Sub DtgGRNSumm_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DtgGRNSumm.ItemCreated
        '//this line must be included
        Grid_ItemCreated(DtgGRNSumm, e)
    End Sub

    Private Sub DtgGRNSumm_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DtgGRNSumm.ItemDataBound
        Dim dsGRNDtl As DataSet
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            e.Item.Cells(0).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("GM_CREATED_DATE"))
            e.Item.Cells(1).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("GM_DATE_RECEIVED"))


            ''lnkGRNNum.NavigateUrl = "GRNDetails.aspx?DOIdx=" & dv("DOM_DO_INDEX") '& "&DONo=" & dv("DOM_DO_NO") & "&GRNNo=" & dv("GM_GRN_NO") & "&PONo=" & dv("POM_PO_NO")
            'e.Item.Cells(1).Text = "<A Href=GRNDetails.aspx?Frm=" & ViewState("strtemp") & "&pageid=" & strPageId & "&GRNNo=" & dv("GM_GRN_NO") & "&DOIdx=" & dv("DOM_DO_INDEX") & "&DONo=" & dv("DOM_DO_NO") & "&BCoyID=" & Session("CompanyID") & _
            '"&OriDONo=" & Me.Request.QueryString("DONo") & "&OriDOIndex=" & Me.Request.QueryString("DOIndex") & "&OriPONo=" & Me.Request.QueryString("PONo") & "&mode=" & ViewState("Mode") & "&type=" & UCase(ViewState("GrnType")) & ">" & e.Item.Cells(1).Text & "</A>"
            dsGRNDtl = objGRN.GetGRNHistory(dv("GM_GRN_NO"), Session("CompanyID"))
            If dsGRNDtl.Tables(0).Rows.Count > 0 Then
                'e.Item.Cells(1).Text = "<A href=""#"" onclick=""PopWindow('PreviewGRN.aspx?Frm=" & ViewState("strtemp") & "&pageid=" & strPageId & "&PONo=" & Common.parseNull(dsGRNDtl.Tables(0).Rows(0)("POM_PO_NO")) & "&GRNNo=" & dv("GM_GRN_NO") & "&DONo=" & Common.parseNull(dsGRNDtl.Tables(0).Rows(0)("DOM_DO_NO")) & "&BCoyID=" & Session("CompanyID") & "');"" >" & e.Item.Cells(1).Text & "</A>"
                e.Item.Cells(2).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("Report", "PreviewGRN.aspx", "Frm=" & ViewState("strtemp") & "&pageid=" & strPageId) & "&PONo=" & Common.parseNull(dsGRNDtl.Tables(0).Rows(0)("POM_PO_NO")) & "&GRNNo=" & dv("GM_GRN_NO") & "&DONo=" & Common.parseNull(dsGRNDtl.Tables(0).Rows(0)("DOM_DO_NO")) & "&BCoyID=" & Session("CompanyID") & "');"" >" & e.Item.Cells(2).Text & "</A>"
            End If

            'e.Item.Cells(2).Text = "<A Href=GRNDetails.aspx>" & e.Item.Cells(2).Text & "</A>"
        End If
    End Sub
    Sub ButtonProperty(ByVal blnEnable As Boolean)
        ViewState("blnButtonState") = blnEnable
        cmdAcceptAll.Enabled = blnEnable
        cmdRejectAll.Enabled = blnEnable
        'cmdSubmit.Enabled = blnEnable
        'cmdReset.Enabled = blnCanUpdate
    End Sub
    'Private Sub cboDONo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDONo.SelectedIndexChanged

    '    DODDL_Change()
    'End Sub
    Private Sub DODDL_Change()
        Dim POIdx, DOIdx, intTotRecord, intTotRecordds, POStatusDB, i As Integer
        Dim PONo, DONo As String
        Dim dsAllInfo As DataSet
        Dim Ordered_Quantity, Received_Quantity, Rejected_Quantity, OutStanding As Double
        PONo = ViewState("PoNo") 'cboPONo.SelectedItem.Text
        POIdx = ViewState("PoIndex") 'cboPONo.SelectedItem.Value
        DONo = lblDONum.Text
        DOIdx = intDOIdx

        If DONo = "---Select---" Then
            'Common.NetMsgbox(Me, "Please select a DO Number")
            DODtl.Style("display") = "none"
            ButtonProperty(False)
        Else
            Dim intLevel As Integer
            If UCase(ViewState("GrnType")) = "GRNACK" Then
                intLevel = 2
            Else
                intLevel = 1
            End If
            If Not objGRN.IsTieToLocation(DOIdx, intLevel) Then
                'Common.NetMsgbox(Me, "No Access to the PO Item's Delivery Address.")
                ButtonProperty(False)

                If strFrm = "GRNList" Or strFrm = "GRNDetails" Then
                    Common.NetMsgbox(Me, "No Access to the PO Item's Delivery Address.", dDispatcher.direct("GRN", "GRNList.aspx", "Frm=" & ViewState("strtemp") & "&pageid=" & strPageId))
                ElseIf strFrm = "Dashboard" Then
                    Common.NetMsgbox(Me, "No Access to the PO Item's Delivery Address.", dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & ViewState("strtemp") & "&pageid=0"))
                Else
                    Common.NetMsgbox(Me, "No Access to the PO Item's Delivery Address.", dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "Frm=" & ViewState("strtemp") & "&pageid=0"))
                End If

                Exit Sub
            End If
            txtReceivedDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, Now())
            If UCase(ViewState("GrnType")) = "GRNACK" Then
                dsAllInfo = objGRN.GetGRNDetails(DOIdx)
            Else
                dsAllInfo = objGRN_Ext.ShowDOdetails(DONo, DOIdx, POIdx)
                'dsAllInfo = objGRN.ShowDOdetails(DONo, DOIdx, POIdx)
                'dtgGRNDtl.Columns(EnumGRN.icRejQty1stLevel).Visible = False  'First level rejected
            End If

            Dim dvViewDO As DataView
            dvViewDO = dsAllInfo.Tables(0).DefaultView
            dvViewDO.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewDO.Sort += " DESC"

            intTotRecord = dsAllInfo.Tables(0).Rows.Count
            intPageRecordCnt = intTotRecord
            '//bind datagrid

            '//datagrid.pageCount only got value after databind


            If intTotRecord > 0 Then
                'intTotPage = dtgDO.PageCount
                DODtl.Style("display") = "inline"
                'dtgGRNDtl.DataSource = dvViewDO
                dtgGRNDtStk.DataSource = dvViewDO
                hidControl.Value = ""
                hidSummary.Value = ""
                'dtgGRNDtl.DataBind()
                dtgGRNDtStk.DataBind()

                ButtonProperty(True)
                ViewState("VCoyID") = dvViewDO(0)("POM_S_Coy_ID")
                If UCase(ViewState("GrnType")) = "GRNACK" Then
                    ViewState("GRNNo") = dvViewDO(0)("GM_GRN_NO")
                    ViewState("GRNIdx") = dvViewDO(0)("GM_GRN_INDEX")
                Else
                    ViewState("GRNNo") = ""
                    ViewState("GRNIdx") = 0
                    'dtgGRNDtl.Columns(EnumGRN.icRejQty1stLevel).Visible = False  'First level rejected
                    dtgGRNDtStk.Columns(EnumGRNStk.icRejQty1stLevel).Visible = False  'First level rejected
                End If
            Else
                ButtonProperty(False)
            End If
            'dtgGRNDtl.Columns(1).Visible = False  'Buyer Item Code
            dtgGRNDtStk.Columns(1).Visible = False  'Buyer Item Code
            'Me.dtgGRNDtl.Columns(6).Visible = False
            'Else
            '    DODtl.Style("display") = "none"
            '    Common.NetMsgbox(Me, "You are not authorised user.")
            'End If
            If Not IsDBNull(dsAllInfo.Tables(0).Rows(0)("POD_ITEM_TYPE")) Then
                If dsAllInfo.Tables(0).Rows(0)("POD_ITEM_TYPE") = "ST" Then
                    Session("blnGRNTrue") = False
                Else
                    Session("blnGRNTrue") = True
                End If
            Else
                Session("blnGRNTrue") = True
            End If

            tr_SEH1.Style("display") = "none"
            For i = 0 To dsAllInfo.Tables(0).Rows.Count - 1
                If Not IsDBNull(dsAllInfo.Tables(0).Rows(i)("POD_ITEM_TYPE")) Then
                    If dsAllInfo.Tables(0).Rows(i)("POD_ITEM_TYPE") = "ST" Or dsAllInfo.Tables(0).Rows(i)("POD_ITEM_TYPE") = "MI" Then
                        tr_SEH1.Style("display") = ""
                        Exit For
                    End If
                End If
            Next

            If Session("blnGRNTrue") = True Then 'Spot Item
                'tr_SEH1.Style("display") = "none"
                dtgGRNDtStk.Columns(EnumGRNStk.icLotNo).Visible = False
            Else
                'tr_SEH1.Style("display") = ""
                dtgGRNDtStk.Columns(EnumGRNStk.icLotNo).Visible = True
            End If

            lblGRNValue.Text = Format(TotalLandCost, "##,###,##0.00")

            'If Session("blnGRNTrue") = False Then
            AddRow()
            'End If

        End If
    End Sub

    Private Function validateDatagrid(ByRef strMsg As String) As Boolean
        validateDatagrid = True
        strMsg = "<ul type='disc'>"
        Dim txtQ As TextBox
        Dim txtRemark As TextBox
        Dim txtReject As TextBox
        Dim setLoc As Button
        Dim decRej As Decimal
        Dim dgItem As DataGridItem
        Dim ifound As Boolean
        Dim i As Integer
        Dim aryTemp As New ArrayList()

        'If Session("blnGRNTrue") = True Then
        '    For Each dgItem In dtgGRNDtl.Items

        '        txtRemark = dgItem.FindControl("txtDtlRemarks")
        '        txtQ = dgItem.FindControl("txtQ")

        '        If Not Common.checkMaxLength(txtRemark.Text, 400) Then
        '            strMsg &= "<li>" & dgItem.Cells(EnumGRN.icLineNo).Text & ". Remarks is over limit.<ul type='disc'></ul></li>"
        '            txtQ.Text = "?"
        '            validateDatagrid = False
        '        Else
        '            txtQ.Text = ""
        '        End If


        '    Next
        'Else
        For Each dgItem In dtgGRNDtStk.Items

            txtRemark = dgItem.FindControl("txtDtlRemarks")
            txtQ = dgItem.FindControl("txtQ")
            setLoc = dgItem.FindControl("cmdSub")
            txtReject = dgItem.FindControl("txtReject")

            If Not Common.checkMaxLength(txtRemark.Text, 400) Then
                strMsg &= "<li>" & dgItem.Cells(EnumGRNStk.icLineNo).Text & ". Remarks is over limit.<ul type='disc'></ul></li>"
                txtQ.Text = "?"
                validateDatagrid = False
            Else
                txtQ.Text = ""
            End If

            If txtReject.Text = "" Then
                decRej = 0
            Else
                If Not IsNumeric(txtReject.Text) Then
                    decRej = 0
                Else
                    decRej = CDec(txtReject.Text)
                End If
            End If

            If setLoc.Enabled = True Then
                If decRej > 0 And decRej <> CDec(dgItem.Cells(EnumGRNStk.icReceivedQty).Text) Then
                    aryTemp = Session("arySetLocation")

                    If aryTemp Is Nothing Then
                        strMsg &= "<li>Please set location for line no. " & dgItem.Cells(EnumGRNStk.icLineNo).Text & ".<ul type='disc'></ul></li>"
                        validateDatagrid = False
                    Else
                        ifound = False

                        For i = 0 To aryTemp.Count - 1
                            If dgItem.Cells(EnumGRNStk.icLineNo).Text = aryTemp(i)(8) And aryTemp(i)(2) <> "" Then
                                ifound = True
                            End If
                        Next

                        If ifound = False Then
                            strMsg &= "<li>Please set location for line no. " & dgItem.Cells(EnumGRNStk.icLineNo).Text & ".<ul type='disc'></ul></li>"
                            validateDatagrid = False
                        End If
                    End If
                End If
            End If

        Next
        'End If

        strMsg &= "</ul>"

    End Function

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim PONo, DONo, strNewGRNNo, strNewIQCNo As String
        Dim POIdx, DOIdx, intTotRecord As Integer
        Dim blnQC As Boolean
        Dim dsGRN, dsChk As DataSet
        blnRejectAll = True
        Dim blnGRNStk As Boolean = False
        PONo = ViewState("PoNo") ' cboPONo.SelectedItem.Text
        POIdx = ViewState("PoIndex") 'cboPONo.SelectedItem.Value
        DONo = lblDONum.Text
        DOIdx = intDOIdx
        If UCase(ViewState("GrnType")) = "GRNACK" Then
            '//remark by Moo
            strNewGRNNo = ViewState("GRNNo") 'cboGRNNo.SelectedItem.Text
        Else
            strNewGRNNo = ""
        End If

        Dim ds As New DataSet
        Dim dtGrnMstr As New DataTable
        dtGrnMstr.Columns.Add("SCoyID", Type.GetType("System.String"))
        dtGrnMstr.Columns.Add("PONo", Type.GetType("System.String"))
        dtGrnMstr.Columns.Add("DONo", Type.GetType("System.String"))
        dtGrnMstr.Columns.Add("GRNNo", Type.GetType("System.String"))
        dtGrnMstr.Columns.Add("POIndex", Type.GetType("System.Double"))
        dtGrnMstr.Columns.Add("DOIndex", Type.GetType("System.Double"))
        dtGrnMstr.Columns.Add("GRNIndex", Type.GetType("System.Double"))
        dtGrnMstr.Columns.Add("GRNReceivedDt", Type.GetType("System.DateTime"))
        Dim dtr As DataRow
        dtr = dtGrnMstr.NewRow
        If ViewState("VCoyID") Is Nothing Then
            dtr("SCoyID") = ""
        Else
            dtr("SCoyID") = ViewState("VCoyID")
        End If
        dtr("PONo") = ViewState("PoNo") 'cboPONo.SelectedItem.Text
        dtr("DONo") = lblDONum.Text 'cboDONo.SelectedItem.Text
        dtr("GRNNo") = ViewState("GRNNo")
        dtr("POIndex") = ViewState("PoIndex") 'cboPONo.SelectedItem.Value
        dtr("DOIndex") = intDOIdx 'cboDONo.SelectedItem.Value
        If ViewState("GRNIdx") Is Nothing Then
            dtr("GRNIndex") = 0
        Else
            dtr("GRNIndex") = ViewState("GRNIdx")
        End If
        dtr("GRNReceivedDt") = Me.txtReceivedDate.Text
        dtGrnMstr.Rows.Add(dtr)
        ds.Tables.Add(dtGrnMstr)
        BindGRN(ds)
        ' Michelle
        Dim strMsg As String = ""
        If Page.IsValid And validateDatagrid(strMsg) Then
            Dim Loc As String
            Loc = objINV.GetDafaultLocation("", "")
            'Michelle (14/5/2011) - Since objINV.GetDefaultLocation will return integer, need to cater for '0'
            'If Loc = "" Then
            'Michelle (13/10/2011) - Issue 1030
            'If Loc = "" Or Loc = "0" Then
            If (Loc = "" Or Loc = "0") And Not ViewState("blnSpGRN") Then
                Common.NetMsgbox(Me, objGLO.GetErrorMessage("00010"), MsgBoxStyle.Information)
                Exit Sub
            End If

            If Not blnErr Then
                '**********************************meilai 15/2/2005*************************
                Dim strLevelType As String = Me.Request.QueryString("Level")
                '**********************************meilai*****************************************
                Dim arySetLocation As New ArrayList()

                If Session("arySetLocation") Is Nothing Then
                    arySetLocation.Add(New String() {"", "", "", "", "", "", "", "", "", ""})
                Else
                    arySetLocation = Session("arySetLocation")
                End If

                If Not Session("blnGRNTrue") Is Nothing Then
                    If Session("blnGRNTrue") = False Then
                        blnGRNStk = True
                    End If
                End If

                If objGRN_Ext.GRNSubmit(ds, strNewGRNNo, UCase(ViewState("GrnType")), blnRejectAll, arySetLocation, blnGRNStk, blnQC, strNewIQCNo) = True Then
                    '**********************************meilai 15/2/2005*************************
                    'Response.Redirect("GRNMsg.aspx?GRNNo=" & strNewGRNNo & "&HD= New GRN&DONo=" & DONo & "&pageid=" & strPageId & "&type=" & UCase(viewstate("GrnType")))
                    'Response.Redirect("GRNMsg.aspx?GRNNo=" & strNewGRNNo & "&HD= New GRN&DONo=" & DONo & "&pageid=" & strPageId & "&type=" & UCase(viewstate("GrnType")) & "&LevelType=" & strLevelType)
                    '**********************************meilai*****************************************
                    If blnGRNStk = True And blnQC = True Then
                        strMsg = "IQC Number " & strNewIQCNo & " has been successfully created for GRN Number " & strNewGRNNo & "."
                        Common.NetMsgbox(Me, "GRN Number " & strNewGRNNo & " has been successfully " & strLevelType & " for DO Number " & DONo & "." & """& vbCrLf & """ & strMsg)
                    Else
                        Common.NetMsgbox(Me, "GRN Number " & strNewGRNNo & " has been successfully " & strLevelType & " for DO Number " & DONo)
                    End If


                    cmdPreviewGRN.Visible = True
                    Me.cmdPreviewGRN.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewGRN.aspx", "PONo=" & ViewState("PoNo") & "&GRNNo=" & strNewGRNNo & "&DONo=" & DONo) & "&BCoyID=" & Session("CompanyID") & "')")
                    cmdPreviewPO.Visible = False

                    strMode = "Edit"
                    ViewState("Mode") = strMode

                    'Dim dgItem As DataGridItem
                    'If Session("blnGRNTrue") = True Then
                    '    For Each dgItem In dtgGRNDtl.Items
                    '        dgItem.Cells(12).Enabled = False
                    '    Next
                    'Else
                    Recal(False)
                    'AddRow()

                    'End If

                    'Session("arySetLocation") = Nothing

                Else
                    If LCase(strNewGRNNo) = "dup" Then
                        'Common.NetMsgbox(Me, MsgTransDup, "GRNSearch.aspx?pageid=" & strPageId & "&type=" & UCase(ViewState("GrnType")))
                        Common.NetMsgbox(Me, MsgTransDup, dDispatcher.direct("GRN", "GRNSearch.aspx", "pageid=" & strPageId & "&type=" & UCase(ViewState("GrnType"))))

                    ElseIf LCase(strNewGRNNo) = "exist1" Or LCase(strNewGRNNo) = "generated" Then
                        'Common.NetMsgbox(Me, "GRN already tie to this DO.", "GRNSearch.aspx?pageid=" & strPageId & "&type=" & UCase(ViewState("GrnType")))
                        Common.NetMsgbox(Me, "GRN already tie to this DO.", dDispatcher.direct("GRN", "GRNSearch.aspx", "pageid=" & strPageId & "&type=" & UCase(ViewState("GrnType"))))

                    ElseIf LCase(strNewGRNNo) = "exist2" Then
                        'Common.NetMsgbox(Me, "This GRN has already been acknowledged.", "GRNSearch.aspx?pageid=" & strPageId & "&type=" & UCase(ViewState("GrnType")))
                        Common.NetMsgbox(Me, "This GRN has already been acknowledged.", dDispatcher.direct("GRN", "GRNSearch.aspx", "pageid=" & strPageId & "&type=" & UCase(ViewState("GrnType"))))

                    ElseIf strNewGRNNo = "LocationError" Then
                        Common.NetMsgbox(Me, objGLO.GetErrorMessage("00014"), MsgBoxStyle.Information)

                    ElseIf strNewGRNNo = "RejectError" Then
                        Common.NetMsgbox(Me, "Total quantity set in the location exceeds the balance quantity.", MsgBoxStyle.Information)

                    ElseIf strNewGRNNo = "ExistIQC" Then
                        Common.NetMsgbox(Me, "Duplicated IQC No. found.", MsgBoxStyle.Information)

                    ElseIf strNewGRNNo = "NoApproval" Then
                        Common.NetMsgbox(Me, "There is no IQC approval group tied to selected items.", MsgBoxStyle.Information)

                    ElseIf strNewGRNNo = "SetLocError" Then
                        Common.NetMsgbox(Me, "Set Location is not setup sucessfully.", MsgBoxStyle.Information)

                    Else
                        If strNewGRNNo <> "" Then
                            '**********************************meilai 15/2/2005*************************
                            'Response.Redirect("GRNMsg.aspx?GRNNo=" & strNewGRNNo & "&DONo=&pageid=" & strPageId & "&type=" & UCase(viewstate("GrnType")))
                            'Response.Redirect("GRNMsg.aspx?GRNNo=" & strNewGRNNo & "&DONo=&pageid=" & strPageId & "&type=" & UCase(ViewState("GrnType")) & "&LevelType=" & strLevelType)
                            If blnGRNStk = True And blnQC = True Then
                                strMsg = "GRN Number " & strNewGRNNo & " has been successfully " & strLevelType & " for DO Number " & DONo & "." & vbCrLf & "IQC Number " & strNewIQCNo & " has been successfully created for GRN Number " & strNewGRNNo & "."
                                Common.NetMsgbox(Me, strMsg)
                            Else
                                strMsg = "GRN Number " & strNewGRNNo & " has been successfully " & strLevelType & " for DO Number " & DONo
                                Common.NetMsgbox(Me, strMsg)
                            End If

                            strMode = "Edit"
                            ViewState("Mode") = strMode
                            '**********************************meilai******************************************
                        Else
                            Common.NetMsgbox(Me, "Error generating GRN.", MsgBoxStyle.Information)
                        End If

                    End If
                    'Response.Redirect("GRNMsg.aspx?GRNNo=" & strNewGRNNo & "&DONo=&pageid=" & strPageId & "&type=" & UCase(viewstate("GrnType")))
                End If
                'Else
                'Common.NetMsgbox(Me, "GRN already created for this DO. Please go back and refresh the page.")
                'End If
            Else
                lbl_check.Text = strErr
            End If
        Else
            If strMsg <> "" Then
                lbl_check.Text = strMsg
            Else
                lbl_check.Text = ""
            End If
        End If

    End Sub



    Private Sub BindGRN(ByRef ds As DataSet)
        Dim dgItem As DataGridItem
        Dim dtr As DataRow
        Dim i As Integer

        Recal()
        AddRow()

        Dim dtGrnDtls As New DataTable
        dtGrnDtls.Columns.Add("PO_LINE", Type.GetType("System.String"))
        dtGrnDtls.Columns.Add("Received_Qty", Type.GetType("System.Double"))
        dtGrnDtls.Columns.Add("Rejected_Qty", Type.GetType("System.Double"))
        dtGrnDtls.Columns.Add("REMARKS", Type.GetType("System.String"))
        dtGrnDtls.Columns.Add("Oth_Charge", Type.GetType("System.String"))
        dtGrnDtls.Columns.Add("Duties", Type.GetType("System.String"))
        dtGrnDtls.Columns.Add("Inland_Charge", Type.GetType("System.String"))
        dtGrnDtls.Columns.Add("Exchange_rate", Type.GetType("System.String"))
        dtGrnDtls.Columns.Add("Factor", Type.GetType("System.String"))
        dtGrnDtls.Columns.Add("UnitCost", Type.GetType("System.Decimal"))
        dtGrnDtls.Columns.Add("LandedCost", Type.GetType("System.Decimal"))

        i = 0
        blnErr = False
        blnReported = False
        lbl_check.Text = ""

        If Me.txtReceivedDate.Text <> "" And CDate(Me.txtReceivedDate.Text) > Today() Then
            blnErr = True
            strErr = strErr & "<li type='square'>" & lblActDt.Text & " " & objGLO.GetErrorMessage("00025") & "</li>"
        End If

        'If Session("blnGRNTrue") = True Then
        '    For Each dgItem In dtgGRNDtl.Items
        '        dtr = dtGrnDtls.NewRow
        '        Dim txtRemark As TextBox
        '        txtRemark = dgItem.FindControl("txtDtlRemarks")
        '        dtr("REMARKS") = txtRemark.Text 'Cell(10)

        '        'Michelle (3/9/2007) - to assign back the original PO Line No
        '        'dtr("PO_LINE") = dgItem.Cells(0).Text
        '        dtr("PO_LINE") = dgItem.Cells(1).Text
        '        dtr("Received_Qty") = dgItem.Cells(EnumGRN.icReceivedQty).Text
        '        Dim txtRejQty As TextBox
        '        txtRejQty = dgItem.FindControl("txtReject")

        '        'Michelle (CR0010)
        '        i = i + 1

        '        If Not IsNumeric(txtRejQty.Text) Then
        '            blnErr = True
        '            strErr = strErr & "<li type='square'>" & i & ". Reject Quantity is expecting numeric value.</li>"

        '        Else
        '            If dtr("Received_Qty") < Common.parseNull(txtRejQty.Text, 0) Then
        '                blnErr = True
        '                strErr = strErr & "<li type='square'>" & i & ". Reject Quantity is over limit.</li>"
        '            Else
        '                If Common.parseNull(txtRejQty.Text, 0) > 0 And dtr("REMARKS") = "" Then
        '                    blnErr = True
        '                    strErr = strErr & "<li type='square'>" & i & ". Remarks is required.</li>"
        '                End If

        '            End If


        '            dtr("Rejected_Qty") = Common.parseNull(txtRejQty.Text, 0) 'cell(9)

        '            If dtr("Received_Qty") <> dtr("Rejected_Qty") Then
        '                blnRejectAll = False
        '            End If
        '            dtGrnDtls.Rows.Add(dtr)

        '        End If
        '        'Dim cvRemark As RequiredFieldValidator
        '        'cvRemark = dgItem.FindControl("cvRemark")
        '        'cvRemark.ControlToValidate = "txtDtlRemarks"
        '        'cvRemark.ErrorMessage = i & ". Remarks is required. new "
        '        'cvRemark.Text = "?"
        '        'cvRemark.Display = ValidatorDisplay.Dynamic

        '    Next

        'Else
        For Each dgItem In dtgGRNDtStk.Items
            dtr = dtGrnDtls.NewRow
            Dim txtRemark As TextBox
            txtRemark = dgItem.FindControl("txtDtlRemarks")
            dtr("REMARKS") = txtRemark.Text 'Cell(10)

            'Michelle (3/9/2007) - to assign back the original PO Line No
            'dtr("PO_LINE") = dgItem.Cells(0).Text
            dtr("PO_LINE") = dgItem.Cells(1).Text
            dtr("Received_Qty") = dgItem.Cells(EnumGRNStk.icReceivedQty).Text
            Dim txtRejQty As TextBox
            txtRejQty = dgItem.FindControl("txtReject")

            'Michelle (CR0010)
            i = i + 1

            If Not IsNumeric(txtRejQty.Text) Then
                blnErr = True
                strErr = strErr & "<li type='square'>" & i & ". Reject Quantity is expecting numeric value.</li>"

            Else
                If dtr("Received_Qty") < Common.parseNull(txtRejQty.Text, 0) Then
                    blnErr = True
                    strErr = strErr & "<li type='square'>" & i & ". Reject Quantity is over limit.</li>"
                Else
                    If Common.parseNull(txtRejQty.Text, 0) > 0 And dtr("REMARKS") = "" Then
                        blnErr = True
                        strErr = strErr & "<li type='square'>" & i & ". Remarks is required.</li>"
                    End If

                End If


                dtr("Rejected_Qty") = Common.parseNull(txtRejQty.Text, 0) 'cell(9)

                If dtr("Received_Qty") <> dtr("Rejected_Qty") Then
                    blnRejectAll = False
                End If
                dtGrnDtls.Rows.Add(dtr)

            End If

            Dim txtOthCharge As TextBox
            txtOthCharge = dgItem.FindControl("txtOthCharges")
            dtr("Oth_Charge") = IIf(txtOthCharge.Text = "", 0, txtOthCharge.Text)

            Dim txtDuties As TextBox
            txtDuties = dgItem.FindControl("txtDuties")
            dtr("Duties") = IIf(txtDuties.Text = "", 0, txtDuties.Text)

            Dim txtInland As TextBox
            txtInland = dgItem.FindControl("txtInlandCharges")
            dtr("Inland_Charge") = IIf(txtInland.Text = "", 0, txtInland.Text)

            Dim lblRate As Label
            lblRate = dgItem.FindControl("lblRate")
            dtr("Exchange_rate") = Replace(lblRate.Text, ",", "")

            Dim lblPerFac As Label
            lblPerFac = dgItem.FindControl("lblPerFac")
            dtr("Factor") = Replace(lblPerFac.Text, ",", "")

            Dim lblUnitCost As Label
            lblUnitCost = dgItem.FindControl("lblUnitCost")
            dtr("UnitCost") = Replace(lblUnitCost.Text, ",", "")

            Dim lblLandCost As Label
            lblLandCost = dgItem.FindControl("lblLandCost")
            dtr("LandedCost") = Replace(lblLandCost.Text, ",", "")
        Next

        'End If

        ds.Tables.Add(dtGrnDtls)

    End Sub


    Private Sub cmdAcceptAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAcceptAll.Click
        Dim dgItem As DataGridItem

        For Each dgItem In dtgGRNDtStk.Items
            Dim txtRejQty As TextBox
            txtRejQty = dgItem.FindControl("txtReject")
            txtRejQty.Text = "0"
        Next

    End Sub

    Private Sub cmdRejectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRejectAll.Click
        Dim dgItem As DataGridItem

        For Each dgItem In dtgGRNDtStk.Items
            Dim txtRejQty As TextBox
            txtRejQty = dgItem.FindControl("txtReject")
            txtRejQty.Text = dgItem.Cells(EnumGRNStk.icReceivedQty).Text
        Next

    End Sub

    Public Sub dtgGRNDtStk_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgGRNDtStk.ItemCommand
        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        Dim strName As String
        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
        Dim qty As Decimal = 0

        If (e.CommandSource).CommandName = "" Then

            Dim txtRejQty As TextBox
            txtRejQty = e.Item.FindControl("txtReject")
            If txtRejQty.Text = "" Then
                qty = 0
            Else
                If Not IsNumeric(txtRejQty.Text) Then
                    qty = 0
                Else
                    qty = CDec(txtRejQty.Text)
                End If
            End If

            strName = "rjqty=" & qty & "&rqty=" & e.Item.Cells(EnumGRNStk.icReceivedQty).Text & "&DONo=" & Me.Request.QueryString("DONo") & "&vendor=" & ViewState("VCoyID") & "&item=" & Server.UrlEncode(e.Item.Cells(EnumGRNStk.icVendorItemCode).Text) & "&itemname=" & Server.UrlEncode(e.Item.Cells(EnumGRNStk.icDescription).Text) & "&poline=" & Server.UrlEncode(e.Item.Cells(EnumGRNStk.icPOLine).Text) & "&itemrow=" & Server.UrlEncode(e.Item.Cells(EnumGRNStk.icLineNo).Text) & ""

            strscript.Append("<script language=""javascript"">")
            strFileName = dDispatcher.direct("GRN", "LocationMaster.aspx", strName)
            strFileName = Server.UrlEncode(strFileName)
            strscript.Append("ShowDialog('" & dDispatcher.direct("GRN", "Dialog.aspx", "page=" & strFileName) & "','400px');")
            strscript.Append("document.getElementById('btnHidden').click();")
            strscript.Append("</script>")
            RegisterStartupScript("script3", strscript.ToString())
        End If
    End Sub

    Private Sub dtgGRNDtStk_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGRNDtStk.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim dt As DataTable
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim sClientId As String, sTotalClient As String

            Dim txtRejQty As TextBox
            txtRejQty = e.Item.FindControl("txtReject")
            txtRejQty.Text = "0"

            'simply use one of the client id to get all client ID
            sTotalClient = hidClientId.Value
            sClientId = Mid(txtRejQty.ClientID, InStr(txtRejQty.ClientID, "_") + 1, InStr(Mid(txtRejQty.ClientID, InStr(txtRejQty.ClientID, "_") + 1), "_") - 1) & "|"
            If Not sTotalClient.Contains(sClientId) Then
                hidClientId.Value = hidClientId.Value & sClientId
                hidTotalClientId.Value = hidTotalClientId.Value + 1
            End If

            Dim txtOthCharges As TextBox
            txtOthCharges = e.Item.FindControl("txtOthCharges")
            txtOthCharges.Text = "0"

            Dim txtInlandCharges As TextBox
            txtInlandCharges = e.Item.FindControl("txtInlandCharges")
            txtInlandCharges.Text = "0"

            Dim txtDuties As TextBox
            txtDuties = e.Item.FindControl("txtDuties")
            txtDuties.Text = "0"

            e.Item.Cells(EnumGRNStk.icOutstanding).Text = Common.parseNull(dv("POD_ORDERED_QTY"), 0) - Common.parseNull(dv("POD_DELIVERED_QTY"), 0) - Common.parseNull(dv("POD_CANCELLED_QTY"), 0)
            If UCase(ViewState("GrnType")) = "GRNACK" Then
                txtRejQty.Text = dv("GD_REJECTED_QTY")
            End If

            'cmdSub = e.Item.FindControl("cmdSub")
            'cmdSub.Attributes.Add("onclick", "window.open('" & dDispatcher.direct("GRN", "LocationMaster.aspx") & "')")

            'Dim strscript As New System.Text.StringBuilder
            'Dim strFileName As String
            'strscript.Append("<script language=""javascript"">")
            'strFileName = dDispatcher.direct("GRN", "LocationMaster.aspx")
            'strFileName = Server.UrlEncode(strFileName)
            'strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx") & "','250px');")
            ''strscript.Append("document.getElementById('btnHidden').click();")
            'strscript.Append("</script>")
            'RegisterStartupScript("script3", strscript.ToString())


            '-------New code Adding To Get The LineNo in the Grid "dtgGRNDtl" By Praveen on 18/07/2007
            'e.Item.Cells(1).Text = e.Item.DataSetIndex + 1
            '-------End The Code

            'Michelle (3/9/2007) - To display the resequence PO line No.
            Dim strPOLine, strOriPOLine As String
            Dim strSql1 As String

            Dim objDB As New EAD.DBCom

            strOriPOLine = e.Item.Cells(0).Text
            strSql1 = "Select count(*) as REC_CNT FROM PO_DETAILS where POD_PO_NO ='" & ViewState("PoNo") & "' and POD_COY_ID = '" & Session("CompanyID") & "'"
            strSql1 = strSql1 & " and POD_PO_LINE <= " & strOriPOLine
            Dim tDS As DataSet = objDB.FillDs(strSql1)

            If tDS.Tables(0).Rows.Count > 0 Then
                strPOLine = tDS.Tables(0).Rows(0).Item("REC_CNT")
            End If
            e.Item.Cells(0).Text = strPOLine
            e.Item.Cells(1).Text = strOriPOLine

            'Dim txtReject As TextBox
            'txtReject = e.Item.FindControl("txtReject")
            'txtReject.Text = "0"
            Dim hidCode As HtmlInputHidden
            hidCode = e.Item.FindControl("hidCode")
            hidCode.Value = dv("POD_Po_Line")

            Dim rev_qtycancel As RegularExpressionValidator
            rev_qtycancel = e.Item.FindControl("rev_qtycancel")
            rev_qtycancel.ControlToValidate = "txtReject"
            rev_qtycancel.ValidationExpression = "^\d{1,6}(?:\.(?:\d{0,1})?\d)?$" '"^\d+$"
            rev_qtycancel.ErrorMessage = "Reject Qty is expecting numeric value. Range must be from 0 to 999999.99"
            rev_qtycancel.Display = ValidatorDisplay.None
            rev_qtycancel.EnableClientScript = "False"

            Dim rev_othcharges As RegularExpressionValidator
            rev_othcharges = e.Item.FindControl("rev_othcharges")
            rev_othcharges.ControlToValidate = "txtOthCharges"
            rev_othcharges.ValidationExpression = "^\d{1,6}(?:\.(?:\d{0,1})?\d)?$" '"^\d+$"
            rev_othcharges.ErrorMessage = "Other Charges is expecting numeric value. Range must be from 0 to 999999.99"
            rev_othcharges.Display = ValidatorDisplay.None
            rev_othcharges.EnableClientScript = "False"

            Dim rev_inlandcharges As RegularExpressionValidator
            rev_inlandcharges = e.Item.FindControl("rev_inlandcharges")
            rev_inlandcharges.ControlToValidate = "txtInlandCharges"
            rev_inlandcharges.ValidationExpression = "^\d{1,6}(?:\.(?:\d{0,1})?\d)?$" '"^\d+$"
            rev_inlandcharges.ErrorMessage = "Inland Charges is expecting numeric value. Range must be from 0 to 999999.99"
            rev_inlandcharges.Display = ValidatorDisplay.None
            rev_inlandcharges.EnableClientScript = "False"

            Dim rev_duties As RegularExpressionValidator
            rev_duties = e.Item.FindControl("rev_duties")
            rev_duties.ControlToValidate = "txtDuties"
            rev_duties.ValidationExpression = "^\d{1,6}(?:\.(?:\d{0,1})?\d)?$" '"^\d+$"
            rev_duties.ErrorMessage = "Duties is expecting numeric value. Range must be from 0 to 999999.99"
            rev_duties.Display = ValidatorDisplay.None
            rev_duties.EnableClientScript = "False"

            e.Item.Cells(EnumGRNStk.icUnitPrice).Text = Format(dv("POD_UNIT_COST"), "#,##0.0000")

            'Get latest Exchange Rate
            Dim CURR_RATE, GRN_FAC As Decimal
            Dim COMP_CURR, TEMP As String
            TEMP = objDB.GetVal("SELECT CE_RATE FROM company_exchangerate WHERE CE_COY_ID = '" & Session("CompanyId") & "' AND CE_CURRENCY_CODE = '" & dv("POM_CURRENCY_CODE") & "' AND CE_DELETED='N' AND CE_VALID_FROM <= CURRENT_DATE() AND CE_VALID_TO >= CURRENT_DATE()")
            If TEMP = "" Then
                COMP_CURR = objDB.GetVal("SELECT CM_CURRENCY_CODE FROM company_mstr WHERE CM_COY_ID = '" & Session("CompanyId") & "'")

                If COMP_CURR = dv("POM_CURRENCY_CODE") Then
                    CURR_RATE = 1
                    Session("blnRate") = True
                Else
                    CURR_RATE = 0
                    Session("blnRate") = False
                End If
            Else
                Session("blnRate") = True
                CURR_RATE = CDec(TEMP)
            End If

            Dim lblRate As Label
            lblRate = e.Item.FindControl("lblRate")
            lblRate.Text = Format(CURR_RATE, "##0.000000")

            'Get latest GRN Factor
            If IsDBNull(dv("POM_DEL_CODE")) Then
                TEMP = ""
            Else
                TEMP = dv("POM_DEL_CODE")
            End If

            TEMP = objDB.GetVal("SELECT CDT_DEL_GRNFACTOR FROM COMPANY_DELIVERY_TERM WHERE CDT_COY_ID = '" & Session("CompanyId") & "' AND CDT_DEL_CODE = '" & Common.Parse(TEMP) & "' AND CDT_DELETED = 'N'")
            If TEMP = "" Then
                GRN_FAC = 0
            Else
                GRN_FAC = CDec(TEMP)
            End If

            Dim lblPerFac As Label
            lblPerFac = e.Item.FindControl("lblPerFac")
            lblPerFac.Text = Format(GRN_FAC, "##0.0000")

            'Disoount Amount
            Dim DISC_AMT As Decimal
            Dim lbl_DiscAmt As Label
            DISC_AMT = CalDiscAmt(dv("POD_PRODUCT_CODE"), dv("POM_S_COY_ID"), dv("POD_ORDERED_QTY"))
            lbl_DiscAmt = e.Item.FindControl("lblDiscAmt")
            lbl_DiscAmt.Text = Format(DISC_AMT, "##0.00")

            'Amount F
            Dim lblAmountF As Label
            Dim dblAmtF As Decimal
            lblAmountF = e.Item.FindControl("lblAmountF")
            dblAmtF = CDec(Format((CDec(dv("POD_UNIT_COST")) * (CDec(dv("DOD_SHIPPED_QTY")) - CDec(txtRejQty.Text))) + CDec(txtOthCharges.Text), "###0.00"))
            lblAmountF.Text = Format(dblAmtF, "##,###,##0.00")

            'Amount M
            Dim lblAmountM As Label
            Dim dblAmtM As Decimal
            lblAmountM = e.Item.FindControl("lblAmountM")
            '2015-06-24: CH: Rounding issue (Prod issue)
            'dblAmtM = dblAmtF * CURR_RATE
            dblAmtM = CDec(Format(dblAmtF * CURR_RATE, "###0.00"))
            lblAmountM.Text = Format(dblAmtM, "##,###,##0.00")

            'GRN Factor
            Dim lblGRNFac As Label
            Dim dblFactor As Decimal
            lblGRNFac = e.Item.FindControl("lblGRNFac")
            dblFactor = dblAmtM * GRN_FAC / 100
            lblGRNFac.Text = Format(dblFactor, "##,###,##0.0000")

            Dim UNIT_FACTOR As Decimal
            UNIT_FACTOR = Format(dblFactor, "####0.00") / (CDec(dv("DOD_SHIPPED_QTY")) - CDec(txtRejQty.Text))

            'Unit Cost(M)
            Dim UNIT_COST As Decimal
            Dim lbl_UnitCost As Label
            UNIT_COST = (CDec(dv("POD_UNIT_COST")) * CURR_RATE) + Format(UNIT_FACTOR, "####0.00")
            lbl_UnitCost = e.Item.FindControl("lblUnitCost")
            lbl_UnitCost.Text = Format(UNIT_COST, "##,###,##0.00")

            'CIF Values
            Dim CIF_VAL As Decimal
            Dim lblCIF As Label
            CIF_VAL = Format(dblFactor, "####0.00") + CDec(txtInlandCharges.Text)
            lblCIF = e.Item.FindControl("lblCIF")
            lblCIF.Text = Format(CIF_VAL, "##,###,##0.00")

            If TotalCIF = 0 Then
                TotalCIF = Format(CIF_VAL, "##0.00")
            Else
                TotalCIF = Format(CIF_VAL, "##0.00") + TotalCIF
            End If

            'Landed Cost
            Dim LAND_COST As Decimal
            Dim lbl_LandCost As Label
            LAND_COST = Format(CIF_VAL, "##0.00") + CDec(txtDuties.Text)
            lbl_LandCost = e.Item.FindControl("lblLandCost")
            lbl_LandCost.Text = Format(LAND_COST, "##,###,##0.00")

            If TotalLandCost = 0 Then
                TotalLandCost = Format(LAND_COST, "##0.00")
            Else
                TotalLandCost = Format(LAND_COST, "##0.00") + TotalLandCost
            End If


            Dim lbl_LotNo As Label
            lbl_LotNo = e.Item.FindControl("lblLotNo")
            lbl_LotNo.Text = dv("DOD_DO_LOT_NO") & "<A onclick=""PopWindow('" & dDispatcher.direct("GRN", "GRNLotList.aspx", "pageid=" & strPageId) & "&DONo=" & Server.UrlEncode(dv("DOM_DO_NO")) & "&itemcode=" & Server.UrlEncode(dv("POD_Vendor_Item_code")) & "&poline=" & Server.UrlEncode(dv("POD_PO_LINE")) & "&SCoyID=" & dv("POM_S_COY_ID") & "&BCoyID=" & Session("CompanyId") & "');"" ><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "d_icon.gif") & "></A>"

            Dim setLoc As Button
            setLoc = e.Item.FindControl("cmdSub")

            Dim SqlQuery, PM_ITEM_TYPE As String
            SqlQuery = " SELECT PM_ITEM_TYPE FROM PRODUCT_MSTR " & _
                       " WHERE PM_PRODUCT_CODE = '" & Common.parseNull(dv("POD_Product_Code")) & "' "
            PM_ITEM_TYPE = objDB.GetVal(SqlQuery)

            If PM_ITEM_TYPE = "SP" Or e.Item.Cells(EnumGRNStk.icVendorItemCode).Text = "&nbsp;" Or PM_ITEM_TYPE = "" Then
                setLoc.Enabled = False
                'Michelle (13/10/2011) - Issue 1030
            Else
                ViewState("blnSpGRN") = False
            End If

            Dim txtQ As TextBox
            txtQ = e.Item.FindControl("txtQ")

            Dim txtRemark As TextBox
            txtRemark = e.Item.FindControl("txtDtlRemarks")
            txtRemark.Attributes.Add("onKeyDown", "limitText (this, 400);")

            txtOthCharges.Attributes.Add("onfocus", "return focusControl('" & _
                                txtOthCharges.ClientID & "', '" & dv("POD_UNIT_COST") & "', '" & _
                                lbl_UnitCost.ClientID & "', '" & lblAmountF.ClientID & "', '" & _
                                CURR_RATE & "', '" & GRN_FAC & "', '" & _
                                dv("DOD_SHIPPED_QTY") & "', '" & txtRejQty.ClientID & "');")

            txtOthCharges.Attributes.Add("onblur", "return calculateTotal('" & _
                                txtOthCharges.ClientID & "', '" & dv("POD_UNIT_COST") & "', '" & _
                                lbl_UnitCost.ClientID & "', '" & lblAmountF.ClientID & "', '" & _
                                lblAmountM.ClientID & "', '" & lblGRNFac.ClientID & "', '" & _
                                CURR_RATE & "', '" & GRN_FAC & "', '" & _
                                txtInlandCharges.ClientID & "', '" & txtDuties.ClientID & "', '" & _
                                lblCIF.ClientID & "', '" & lbl_LandCost.ClientID & "', '" & _
                                dv("DOD_SHIPPED_QTY") & "', '" & txtRejQty.ClientID & "');")

            txtRejQty.Attributes.Add("onfocus", "return focusControl('" & _
                                txtOthCharges.ClientID & "', '" & dv("POD_UNIT_COST") & "', '" & _
                                lbl_UnitCost.ClientID & "', '" & lblAmountF.ClientID & "', '" & _
                                CURR_RATE & "', '" & GRN_FAC & "', '" & _
                                dv("DOD_SHIPPED_QTY") & "', '" & txtRejQty.ClientID & "');")

            txtRejQty.Attributes.Add("onblur", "return calculateTotal('" & _
                                txtOthCharges.ClientID & "', '" & dv("POD_UNIT_COST") & "', '" & _
                                lbl_UnitCost.ClientID & "', '" & lblAmountF.ClientID & "', '" & _
                                lblAmountM.ClientID & "', '" & lblGRNFac.ClientID & "', '" & _
                                CURR_RATE & "', '" & GRN_FAC & "', '" & _
                                txtInlandCharges.ClientID & "', '" & txtDuties.ClientID & "', '" & _
                                lblCIF.ClientID & "', '" & lbl_LandCost.ClientID & "', '" & _
                                dv("DOD_SHIPPED_QTY") & "', '" & txtRejQty.ClientID & "');")

            txtInlandCharges.Attributes.Add("onfocus", "return focusControl('" & _
                                txtOthCharges.ClientID & "', '" & dv("POD_UNIT_COST") & "', '" & _
                                lbl_UnitCost.ClientID & "', '" & lblAmountF.ClientID & "', '" & _
                                CURR_RATE & "', '" & GRN_FAC & "', '" & _
                                dv("DOD_SHIPPED_QTY") & "', '" & txtRejQty.ClientID & "');")

            txtInlandCharges.Attributes.Add("onblur", "return calculateTotal('" & _
                                txtOthCharges.ClientID & "', '" & dv("POD_UNIT_COST") & "', '" & _
                                lbl_UnitCost.ClientID & "', '" & lblAmountF.ClientID & "', '" & _
                                lblAmountM.ClientID & "', '" & lblGRNFac.ClientID & "', '" & _
                                CURR_RATE & "', '" & GRN_FAC & "', '" & _
                                txtInlandCharges.ClientID & "', '" & txtDuties.ClientID & "', '" & _
                                lblCIF.ClientID & "', '" & lbl_LandCost.ClientID & "', '" & _
                                dv("DOD_SHIPPED_QTY") & "', '" & txtRejQty.ClientID & "');")

            txtDuties.Attributes.Add("onfocus", "return focusControl('" & _
                                txtOthCharges.ClientID & "', '" & dv("POD_UNIT_COST") & "', '" & _
                                lbl_UnitCost.ClientID & "', '" & lblAmountF.ClientID & "', '" & _
                                CURR_RATE & "', '" & GRN_FAC & "', '" & _
                                dv("DOD_SHIPPED_QTY") & "', '" & txtRejQty.ClientID & "');")

            txtDuties.Attributes.Add("onblur", "return calculateTotal('" & _
                                txtOthCharges.ClientID & "', '" & dv("POD_UNIT_COST") & "', '" & _
                                lbl_UnitCost.ClientID & "', '" & lblAmountF.ClientID & "', '" & _
                                lblAmountM.ClientID & "', '" & lblGRNFac.ClientID & "', '" & _
                                CURR_RATE & "', '" & GRN_FAC & "', '" & _
                                txtInlandCharges.ClientID & "', '" & txtDuties.ClientID & "', '" & _
                                lblCIF.ClientID & "', '" & lbl_LandCost.ClientID & "', '" & _
                                dv("DOD_SHIPPED_QTY") & "', '" & txtRejQty.ClientID & "');")

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
    End Sub

    'Private Sub dtgGRNDtl_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGRNDtl.ItemCreated
    '    Grid_ItemCreated(sender, e)

    'End Sub

    Private Sub dtgGRNDtStk_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGRNDtStk.ItemCreated
        Grid_ItemCreated(sender, e)

    End Sub

    Private Function CalDiscAmt(ByVal strItem As String, ByVal strVenId As String, ByVal dblOdrQty As Double) As Double
        Dim dsAllPrice As New DataSet
        Dim i As Integer
        Dim dbl1stPrice, dblPrice As Double
        dsAllPrice = objGRN_Ext.GetGRNVolPrice(strVenId, strItem)

        If dsAllPrice.Tables(0).Rows.Count > 1 Then
            For i = 0 To dsAllPrice.Tables(0).Rows.Count - 1
                If i = 0 Then
                    dbl1stPrice = CDbl(dsAllPrice.Tables(0).Rows(i)("PVP_VOLUME_PRICE"))
                    dblPrice = CDbl(dsAllPrice.Tables(0).Rows(i)("PVP_VOLUME_PRICE"))
                Else
                    If dblOdrQty >= CDbl(dsAllPrice.Tables(0).Rows(i)("PVP_VOLUME")) Then
                        dblPrice = CDbl(dsAllPrice.Tables(0).Rows(i)("PVP_VOLUME_PRICE"))
                    End If
                End If
            Next

            CalDiscAmt = (dbl1stPrice - dblPrice) * dblOdrQty
        Else
            CalDiscAmt = 0
        End If

    End Function

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        If lblPONo.Text <> "" Then
            PopulatePODDL(Me.lblPONo.Text)

            'txtPO.Enabled = False
            vldSumm.Enabled = False
            vldSumm.Style("display") = "none"
        End If
    End Sub
    Private Sub cmdreset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click

        Dim dgItem As DataGridItem
        'For Each dgItem In dtgGRNDtl.Items
        '    Dim txtRejQty As TextBox
        '    txtRejQty = dgItem.FindControl("txtReject")
        '    txtRejQty.Text = "0"
        '    Dim txtRemark As TextBox
        '    txtRemark = dgItem.FindControl("txtDtlRemarks")
        '    txtRemark.Text = ""
        'Next

        For Each dgItem In dtgGRNDtStk.Items
            Dim txtRejQty As TextBox
            txtRejQty = dgItem.FindControl("txtReject")
            txtRejQty.Text = "0"

            Dim txtRemark As TextBox
            txtRemark = dgItem.FindControl("txtDtlRemarks")
            txtRemark.Text = ""

            Dim txtOthCharges As TextBox
            txtOthCharges = dgItem.FindControl("txtOthCharges")
            txtOthCharges.Text = "0"

            Dim txtInlandCharges As TextBox
            txtInlandCharges = dgItem.FindControl("txtInlandCharges")
            txtInlandCharges.Text = "0"

            Dim txtDuties As TextBox
            txtDuties = dgItem.FindControl("txtDuties")
            txtDuties.Text = "0"
        Next

        Recal()
        AddRow()
        txtReceivedDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, Now())
        Me.lbl_check.Text = ""


    End Sub
    Private Sub cmdClear_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.ServerClick
        vldSumm.Enabled = True
        vldSumm.Style("display") = "inline"
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn_selected"" href=""GRNList.aspx?pageid=" & strPageId & """><span>Issue GRN</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn"" href=""GRNSearch.aspx?pageid=" & strPageId & """><span>GRN Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                   "</ul><div></div></div>"
        'Session("w_SearchGRN_tabs") = "<div class=""t_entity"">" & _
        '                   "<a class=""t_entity_btn_selected"" href=""GRNList.aspx?pageid=" & strPageId & """><span>Issue GRN</span></a>" & _
        '                   "<a class=""t_entity_btn"" href=""GRNSearch.aspx?pageid=" & strPageId & """><span>GRN Listing</span></a>" & _
        '                  "</div>"
        Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                          "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("GRN", "GRNList.aspx", "pageid=" & strPageId) & """><span>Issue GRN</span></a></li>" & _
                          "<li><div class=""space""></div></li>" & _
                          "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("GRN", "GRNSearch.aspx", "pageid=" & strPageId) & """><span>GRN Listing</span></a></li>" & _
                          "<li><div class=""space""></div></li>" & _
                          "</ul><div></div></div>"
    End Sub

    Private Sub PreviewPO()
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
                .CommandText = "SELECT   PO_MSTR.POM_PO_INDEX, PO_MSTR.POM_PO_NO, PO_MSTR.POM_B_COY_ID, PO_MSTR.POM_BUYER_ID, " _
                             & "PO_MSTR.POM_BUYER_NAME, PO_MSTR.POM_BUYER_PHONE, PO_MSTR.POM_BUYER_FAX, " _
                             & "PO_MSTR.POM_S_COY_ID, PO_MSTR.POM_S_COY_NAME, PO_MSTR.POM_S_ATTN, " _
                             & "PO_MSTR.POM_S_REMARK, PO_MSTR.POM_S_ADDR_LINE1, PO_MSTR.POM_S_ADDR_LINE2, " _
                             & "PO_MSTR.POM_S_ADDR_LINE3, PO_MSTR.POM_S_POSTCODE, PO_MSTR.POM_S_CITY, " _
                             & "PO_MSTR.POM_S_STATE, PO_MSTR.POM_S_COUNTRY, PO_MSTR.POM_S_PHONE, PO_MSTR.POM_S_FAX, " _
                             & "PO_MSTR.POM_S_EMAIL, PO_MSTR.POM_PO_DATE, PO_MSTR.POM_FREIGHT_TERMS, " _
                             & "PO_MSTR.POM_PAYMENT_TERM, PO_MSTR.POM_PAYMENT_METHOD, PO_MSTR.POM_SHIPMENT_MODE, " _
                             & "PO_MSTR.POM_SHIPMENT_TERM, PO_MSTR.POM_CURRENCY_CODE, PO_MSTR.POM_EXCHANGE_RATE, " _
                             & "PO_MSTR.POM_PAYMENT_TERM_CODE, PO_MSTR.POM_SHIP_VIA, PO_MSTR.POM_PO_STATUS, " _
                             & "PO_MSTR.POM_STATUS_CHANGED_BY, PO_MSTR.POM_STATUS_CHANGED_ON, " _
                             & "PO_MSTR.POM_EXTERNAL_REMARK, PO_MSTR.POM_CREATED_BY, PO_MSTR.POM_PO_COST, " _
                             & "PO_MSTR.POM_BILLING_METHOD, PO_MSTR.POM_PO_PREFIX, PO_MSTR.POM_B_ADDR_CODE, " _
                             & "PO_MSTR.POM_B_ADDR_LINE1, PO_MSTR.POM_B_ADDR_LINE2, PO_MSTR.POM_B_ADDR_LINE3,  " _
                             & "PO_MSTR.POM_B_POSTCODE, PO_MSTR.POM_B_CITY, PO_MSTR.POM_B_STATE, " _
                             & "PO_MSTR.POM_B_COUNTRY, PO_MSTR.POM_FULFILMENT, PO_MSTR.POM_DEPT_INDEX, " _
                             & "PO_MSTR.POM_ACCEPTED_DATE, PO_MSTR.POM_DOWNLOADED_DATE, PO_MSTR.POM_ARCHIVE_IND, " _
                             & "PO_MSTR.POM_TERMANDCOND, PO_MSTR.POM_REFERENCE_NO, PO_MSTR.POM_EXTERNAL_IND, " _
                             & "PO_MSTR.POM_PRINT_REMARK, PO_MSTR.POM_PRINT_CUSTOM_FIELDS, PO_DETAILS.POD_COY_ID, " _
                             & "PO_DETAILS.POD_PO_NO, PO_DETAILS.POD_PO_LINE, PO_DETAILS.POD_PRODUCT_CODE, " _
                             & "PO_DETAILS.POD_VENDOR_ITEM_CODE, PO_DETAILS.POD_PRODUCT_DESC, PO_DETAILS.POD_UOM, " _
                             & "PO_DETAILS.POD_ORDERED_QTY, PO_DETAILS.POD_RECEIVED_QTY, PO_DETAILS.POD_REJECTED_QTY, " _
                             & "PO_DETAILS.POD_DELIVERED_QTY, PO_DETAILS.POD_CANCELLED_QTY, PO_DETAILS.POD_MIN_PACK_QTY, " _
                             & "PO_DETAILS.POD_MIN_ORDER_QTY, PO_DETAILS.POD_ETD, PO_DETAILS.POD_WARRANTY_TERMS, " _
                             & "PO_DETAILS.POD_UNIT_COST, PO_DETAILS.POD_REMARK, PO_DETAILS.POD_GST, " _
                             & "PO_DETAILS.POD_PR_INDEX, PO_DETAILS.POD_PR_LINE, PO_DETAILS.POD_ACCT_INDEX, " _
                             & "PO_DETAILS.POD_PRODUCT_TYPE, PO_DETAILS.POD_B_ITEM_CODE, PO_DETAILS.POD_SOURCE, " _
                             & "PO_DETAILS.POD_D_ADDR_CODE, PO_DETAILS.POD_D_ADDR_LINE1, PO_DETAILS.POD_D_ADDR_LINE2, " _
                             & "PO_DETAILS.POD_D_ADDR_LINE3, PO_DETAILS.POD_D_POSTCODE, PO_DETAILS.POD_D_CITY, " _
                             & "PO_DETAILS.POD_D_STATE, PO_DETAILS.POD_D_COUNTRY, PO_DETAILS.POD_B_CATEGORY_CODE, " _
                             & "PO_DETAILS.POD_B_GL_CODE, COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, " _
                             & "COMPANY_MSTR.CM_COY_TYPE, COMPANY_MSTR.CM_PARENT_COY_ID, COMPANY_MSTR.CM_ACCT_NO, " _
                             & "COMPANY_MSTR.CM_BANK, COMPANY_MSTR.CM_BRANCH, COMPANY_MSTR.CM_ADDR_LINE1, " _
                             & "COMPANY_MSTR.CM_ADDR_LINE2, COMPANY_MSTR.CM_ADDR_LINE3, COMPANY_MSTR.CM_POSTCODE, " _
                             & "COMPANY_MSTR.CM_CITY, COMPANY_MSTR.CM_STATE, COMPANY_MSTR.CM_COUNTRY, " _
                             & "COMPANY_MSTR.CM_PHONE, COMPANY_MSTR.CM_FAX, COMPANY_MSTR.CM_EMAIL, " _
                             & "COMPANY_MSTR.CM_COY_LOGO, COMPANY_MSTR.CM_BUSINESS_REG_NO, " _
                             & "COMPANY_MSTR.CM_TAX_REG_NO, COMPANY_MSTR.CM_PAYMENT_TERM, " _
                             & "COMPANY_MSTR.CM_PAYMENT_METHOD, COMPANY_MSTR.CM_ACTUAL_TERMSANDCONDFILE, " _
                             & "COMPANY_MSTR.CM_HUB_TERMSANDCONDFILE, COMPANY_MSTR.CM_PWD_DURATION, " _
                             & "COMPANY_MSTR.CM_TAX_CALC_BY, COMPANY_MSTR.CM_CURRENCY_CODE, " _
                             & "COMPANY_MSTR.CM_BCM_SET, COMPANY_MSTR.CM_BUDGET_FROM_DATE, " _
                             & "COMPANY_MSTR.CM_BUDGET_TO_DATE, COMPANY_MSTR.CM_RFQ_OPTION, " _
                             & "COMPANY_MSTR.CM_LICENCE_PACKAGE, COMPANY_MSTR.CM_LICENSE_USERS, " _
                             & "COMPANY_MSTR.CM_SUB_START_DT, COMPANY_MSTR.CM_SUB_END_DT, " _
                             & "COMPANY_MSTR.CM_LICENSE_PRODUCTS, COMPANY_MSTR.CM_FINDEPT_MODE, " _
                             & "COMPANY_MSTR.CM_PRIV_LABELING, COMPANY_MSTR.CM_SKINS_ID, COMPANY_MSTR.CM_TRAINING, " _
                             & "COMPANY_MSTR.CM_STATUS, COMPANY_MSTR.CM_DELETED, COMPANY_MSTR.CM_MOD_BY, " _
                             & "COMPANY_MSTR.CM_MOD_DT, COMPANY_MSTR.CM_ENT_BY, COMPANY_MSTR.CM_ENT_DT, " _
                             & "COMPANY_MSTR.CM_SKU, COMPANY_MSTR.CM_TRANS_NO, COMPANY_MSTR.CM_CONTACT, " _
                             & "COMPANY_MSTR.CM_REPORT_USERS, COMPANY_MSTR.CM_INV_APPR, COMPANY_MSTR.CM_MULTI_PO, " _
                             & "COMPANY_MSTR.CM_BA_CANCEL, USER_MSTR.UM_AUTO_NO, USER_MSTR.UM_USER_ID, " _
                             & "USER_MSTR.UM_DELETED, USER_MSTR.UM_PASSWORD, USER_MSTR.UM_USER_NAME, " _
                             & "USER_MSTR.UM_COY_ID, USER_MSTR.UM_DEPT_ID, USER_MSTR.UM_EMAIL, USER_MSTR.UM_APP_LIMIT, " _
                             & "USER_MSTR.UM_DESIGNATION, USER_MSTR.UM_TEL_NO, USER_MSTR.UM_FAX_NO, " _
                             & "USER_MSTR.UM_USER_SUSPEND_IND, USER_MSTR.UM_PASSWORD_LAST_CHANGED, " _
                             & "USER_MSTR.UM_NEW_PASSWORD_IND, USER_MSTR.UM_NEXT_EXPIRE_DT, " _
                             & "USER_MSTR.UM_LAST_LOGIN_DT, USER_MSTR.UM_QUESTION, USER_MSTR.UM_ANSWER, " _
                             & "USER_MSTR.UM_MASS_APP, USER_MSTR.UM_STATUS, USER_MSTR.UM_MOD_BY, " _
                             & "USER_MSTR.UM_MOD_DT, USER_MSTR.UM_ENT_BY, USER_MSTR.UM_ENT_DATE, " _
                             & "USER_MSTR.UM_RECORD_COUNT, USER_MSTR.UM_EMAIL_CC, USER_MSTR.UM_INVOICE_APP_LIMIT, " _
                             & "USER_MSTR.UM_INVOICE_MASS_APP, " _
                             & "(SELECT   CODE_DESC " _
                             & "FROM      CODE_MSTR AS a " _
                             & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
                             & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS CMState, " _
                             & "(SELECT   CODE_DESC " _
                             & "FROM      CODE_MSTR AS a " _
                             & "WHERE   (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
                             & "(SELECT   CODE_DESC " _
                             & "FROM      CODE_MSTR AS a " _
                             & "WHERE   (CODE_ABBR = PO_MSTR.POM_S_STATE) AND (CODE_CATEGORY = 's') AND " _
                             & "(CODE_VALUE = PO_MSTR.POM_S_COUNTRY)) AS SupplierAddrState, " _
                             & "(SELECT   CODE_DESC " _
                             & "FROM      CODE_MSTR AS a " _
                             & "WHERE   (CODE_ABBR = PO_MSTR.POM_S_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS SupplierAddrCtry, " _
                             & "(SELECT   CODE_DESC " _
                             & "FROM      CODE_MSTR AS a " _
                             & "WHERE   (CODE_ABBR = PO_MSTR.POM_B_STATE) AND (CODE_CATEGORY = 's') AND " _
                             & "(CODE_VALUE = PO_MSTR.POM_B_COUNTRY)) AS BillAddrState, " _
                             & "(SELECT   CODE_DESC " _
                             & "FROM      CODE_MSTR AS a " _
                             & "WHERE   (CODE_ABBR = PO_MSTR.POM_B_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS BillAddrCtry, " _
                             & "(SELECT   CODE_DESC " _
                             & "FROM      CODE_MSTR AS a " _
                             & "WHERE   (CODE_ABBR = PO_DETAILS.POD_D_STATE) AND (CODE_CATEGORY = 's') AND " _
                             & "(CODE_VALUE = PO_DETAILS.POD_D_COUNTRY)) AS DelvAddrState, " _
                             & "(SELECT   CODE_DESC " _
                             & "FROM      CODE_MSTR AS a " _
                             & "WHERE   (CODE_ABBR = PO_DETAILS.POD_D_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS DelvAddrCtry, " _
                             & "(SELECT   CM_BUSINESS_REG_NO " _
                             & "FROM      COMPANY_MSTR AS B " _
                             & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS sUPPBUSSREGNO, " _
                             & "(SELECT   CM_EMAIL " _
                             & "FROM      COMPANY_MSTR AS B " _
                             & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS SUPPEMAIL, " _
                             & "(SELECT   CM_PHONE " _
                             & "FROM      COMPANY_MSTR AS B " _
                             & "WHERE   (PO_MSTR.POM_S_COY_ID = CM_COY_ID)) AS SUPPPHONE, PO_MSTR.POM_SHIP_AMT " _
                             & "FROM      PO_MSTR INNER JOIN " _
                             & "PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO AND  " _
                             & "PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID INNER JOIN " _
                             & "COMPANY_MSTR ON PO_MSTR.POM_B_COY_ID = COMPANY_MSTR.CM_COY_ID INNER JOIN " _
                             & "USER_MSTR ON PO_MSTR.POM_BUYER_ID = USER_MSTR.UM_USER_ID AND " _
                             & "PO_MSTR.POM_B_COY_ID = USER_MSTR.UM_COY_ID " _
                             & "WHERE   (PO_MSTR.POM_B_COY_ID = @prmCoyID) AND (PO_MSTR.POM_PO_NO = @prmPONo)"
            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyID")))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@prmPONo", ViewState("PoNo")))

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewPO_DataSetPreviewPO", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = appPath & "PO\PreviewPO-FTN.rdlc"
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

            'Dim deviceInfo As String = _
            '     "<DeviceInfo>" + _
            '         "  <OutputFormat>EMF</OutputFormat>" + _
            '         "  <PageWidth>8.27in</PageWidth>" + _
            '         "  <PageHeight>11in</PageHeight>" + _
            '         "  <MarginTop>0.25in</MarginTop>" + _
            '         "  <MarginLeft>0.25in</MarginLeft>" + _
            '         "  <MarginRight>0.25in</MarginRight>" + _
            '         "  <MarginBottom>0.25in</MarginBottom>" + _
            '         "</DeviceInfo>"
            Dim deviceInfo As String = _
                "<DeviceInfo>" + _
                    "  <OutputFormat>EMF</OutputFormat>" + _
                    "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

            Dim fs As New FileStream(appPath & "PO\POReport.PDF", FileMode.Create)
            fs.Write(PDF, 0, PDF.Length)
            fs.Close()

            Dim strJScript As String
            strJScript = "<script language=javascript>"
            strJScript += "window.open('../PO/POReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
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

    Sub AddRow()

        Dim intL, intColToRemain, intCell As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer
        intCell = EnumGRNStk.icAmountM

        For intL = 0 To 5
            addCell(row)
        Next

        row.Cells(0).BorderWidth = 0
        row.Cells(0).Text = "Total:"
        row.Cells(0).Font.Bold = True
        row.Cells(0).ColumnSpan = intCell - 1
        row.Cells(0).HorizontalAlign = HorizontalAlign.Right

        row.Cells(1).BorderWidth = 0

        row.Cells(2).BorderWidth = 0

        row.Cells(3).BorderWidth = 0
        row.Cells(3).Text = Format(TotalCIF, "##,###,##0.00")
        row.Cells(3).ID = "TotalCIF"
        row.Cells(3).Font.Bold = True
        row.Cells(3).HorizontalAlign = HorizontalAlign.Right

        row.Cells(4).BorderWidth = 0

        row.Cells(5).BorderWidth = 0
        row.Cells(5).Text = Format(TotalLandCost, "##,###,##0.00")
        row.Cells(5).ID = "TotalLandCost"
        row.Cells(5).Font.Bold = True
        row.Cells(5).HorizontalAlign = HorizontalAlign.Right

        dtgGRNDtStk.Controls(0).Controls.Add(row)

        hidTotalCIFID.Value = row.Cells(3).ClientID
        hidTotalLandCostID.Value = row.Cells(5).ClientID

    End Sub

    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
    End Sub

    Sub Recal(Optional ByVal blnSet As Boolean = True)
        Dim dgItem As DataGridItem

        TotalLandCost = 0
        TotalCIF = 0
        For Each dgItem In dtgGRNDtStk.Items
            If blnSet = False Then
                dgItem.Cells(EnumGRNStk.icLocation).Enabled = False
            End If
            'lblGRNValue.Text = "99999.99"

            Dim lblPerFac As Label
            lblPerFac = dgItem.FindControl("lblPerFac")
            lblPerFac.Text = Format(CDec(lblPerFac.Text), "##0.0000")

            Dim lblRate As Label
            lblRate = dgItem.FindControl("lblRate")
            lblRate.Text = Format(CDec(lblRate.Text), "##0.000000")

            Dim txtOthCharges As TextBox
            txtOthCharges = dgItem.FindControl("txtOthCharges")
            Dim decOthCharges As Decimal
            If txtOthCharges.Text = "" Then
                txtOthCharges.Text = "0.00"
                decOthCharges = 0
            Else
                If Not IsNumeric(txtOthCharges.Text) Then
                    decOthCharges = 0
                Else
                    txtOthCharges.Text = Format(CDec(txtOthCharges.Text), "##0.00")
                    decOthCharges = Format(CDec(txtOthCharges.Text), "##0.00")
                End If
            End If

            Dim txtRejQty As TextBox
            txtRejQty = dgItem.FindControl("txtReject")
            Dim decRejQty As Decimal
            If txtRejQty.Text = "" Then
                txtRejQty.Text = "0.00"
                decRejQty = 0
            Else
                If Not IsNumeric(txtRejQty.Text) Then
                    decRejQty = 0
                Else
                    txtRejQty.Text = Format(CDec(txtRejQty.Text), "##0.00")
                    decRejQty = Format(CDec(txtRejQty.Text), "##0.00")
                End If
            End If

            Dim dblUnitPrice As Decimal = Replace(dgItem.Cells(EnumGRNStk.icUnitPrice).Text, ",", "")

            'Amount F
            Dim lblAmountF As Label
            Dim dblAmtF As Decimal
            lblAmountF = dgItem.FindControl("lblAmountF")
            dblAmtF = CDec(dblUnitPrice) * (CDec(dgItem.Cells(EnumGRNStk.icReceivedQty).Text) - decRejQty) + decOthCharges
            lblAmountF.Text = Format(dblAmtF, "##,###,##0.00")


            'Amount M
            Dim lblAmountM As Label
            Dim dblAmtM As Decimal
            lblAmountM = dgItem.FindControl("lblAmountM")
            dblAmtM = dblAmtF * CDec(lblRate.Text)
            lblAmountM.Text = Format(dblAmtM, "##,###,##0.00")

            'GRN Factor
            Dim lblGRNFac As Label
            Dim dblFactor As Decimal
            lblGRNFac = dgItem.FindControl("lblGRNFac")
            dblFactor = dblAmtM * CDec(lblPerFac.Text) / 100
            lblGRNFac.Text = Format(dblFactor, "##,###,##0.0000")

            Dim UNIT_FACTOR As Decimal
            Dim dblQty As Decimal
            dblQty = (CDec(dgItem.Cells(EnumGRNStk.icReceivedQty).Text) - decRejQty)
            If dblQty = 0 Then
                UNIT_FACTOR = 0
            Else
                UNIT_FACTOR = Format(dblFactor, "####0.00") / dblQty
            End If


            'Unit Cost(M)
            Dim UNIT_COST As Decimal
            Dim lbl_UnitCost As Label
            UNIT_COST = (CDec(dblUnitPrice) * CDec(lblRate.Text)) + Format(UNIT_FACTOR, "##0.00")
            lbl_UnitCost = dgItem.FindControl("lblUnitCost")
            lbl_UnitCost.Text = Format(UNIT_COST, "##,###,##0.00")

            Dim txtInlandCharges As TextBox
            txtInlandCharges = dgItem.FindControl("txtInlandCharges")
            Dim decInlandCharges As Decimal
            If txtInlandCharges.Text = "" Then
                txtInlandCharges.Text = "0.00"
                decInlandCharges = 0
            Else
                If Not IsNumeric(txtInlandCharges.Text) Then
                    decInlandCharges = 0
                Else
                    txtInlandCharges.Text = Format(CDec(txtInlandCharges.Text), "##0.00")
                    decInlandCharges = Format(CDec(txtInlandCharges.Text), "##0.00")
                End If
            End If

            'CIF Values
            Dim CIF_VAL As Decimal
            Dim lblCIF As Label
            CIF_VAL = Format(dblFactor, "##0.00") + decInlandCharges
            lblCIF = dgItem.FindControl("lblCIF")
            lblCIF.Text = Format(CIF_VAL, "##,###,##0.00")

            If TotalCIF = 0 Then
                TotalCIF = Format(CIF_VAL, "##0.00")
            Else
                TotalCIF = Format(CIF_VAL, "##0.00") + TotalCIF
            End If

            Dim txtDuties As TextBox
            txtDuties = dgItem.FindControl("txtDuties")
            Dim decDuties As Decimal
            If txtDuties.Text = "" Then
                txtDuties.Text = "0.00"
                decDuties = 0
            Else
                If Not IsNumeric(txtDuties.Text) Then
                    decDuties = 0
                Else
                    txtDuties.Text = Format(CDec(txtDuties.Text), "##0.00")
                    decDuties = Format(CDec(txtDuties.Text), "##0.00")
                End If
            End If

            'Landed Cost
            Dim LAND_COST As Decimal
            Dim lbl_LandCost As Label
            If txtDuties.Text = "" Then
                LAND_COST = Format(CIF_VAL, "##0.00") + 0
            Else
                LAND_COST = Format(CIF_VAL, "##0.00") + decDuties
            End If
            lbl_LandCost = dgItem.FindControl("lblLandCost")
            lbl_LandCost.Text = Format(LAND_COST, "##,###,##0.00")

            If TotalLandCost = 0 Then
                TotalLandCost = Format(LAND_COST, "##0.00")
            Else
                TotalLandCost = Format(LAND_COST, "##0.00") + TotalLandCost
            End If

        Next

        lblGRNValue.Text = Format(TotalLandCost, "##,###,##0.00")
    End Sub

    Private Sub btnhidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhidden.Click

        Recal()
        AddRow()

    End Sub
End Class