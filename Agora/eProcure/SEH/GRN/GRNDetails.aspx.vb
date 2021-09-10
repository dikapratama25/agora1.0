Imports AgoraLegacy
Imports eProcure.Component

Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class GRNDetailsSEH
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblPONo As System.Web.UI.WebControls.Label
    Protected WithEvents lblDONo As System.Web.UI.WebControls.Label
    Protected WithEvents lblGrnNo As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblCreatedBy As System.Web.UI.WebControls.Label
    Protected WithEvents lblDtREceived As System.Web.UI.WebControls.Label
    Protected WithEvents lblGRNDate As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdPreviewGRN As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Dim strFrm As String

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim objGRN As New GRN
    Dim objGRN_Ext As New GRN_Ext
    Dim objDO As New DeliveryOrder
    Dim strGRNNo, strBCoyID As String
    Dim intDOIndex As Integer
    Dim objINV As New Inventory
    Dim dDispatcher = New dispatcher
    Dim TotalCIF As Double = 0
    Dim TotalLandCost As Double = 0
    Dim Court_loc As Integer = 0
    Dim objFile As New FileManagement
    Dim LocDesc, SubLocDesc As String

    'Public Enum EnumGRNDet
    '    icPOLine
    '    '----New Column Added to Grid "dtgGrnDtl" by Praveen on 18/07/2007
    '    icLineNo
    '    '----End
    '    icVendorItemCode
    '    icDescription
    '    icUOM
    '    icMPQ
    '    icOrdered
    '    icShipped
    '    icReceivedQty
    '    icRejQty
    '    icRemarks
    '    icNoName
    'End Enum

    Public Enum EnumGRNDetStk
        icPOLine
        '----New Column Added to Grid "dtgGrnDtl" by Praveen on 18/07/2007
        icLineNo
        '----End
        icVendorItemCode
        icDescription
        icUOM
        icGLCode
        icCurrency
        icUnitPriceF
        icDiscAmt
        icOthCharges
        icUnitCostM
        icAmtF
        icAmtM
        icFactor
        icInland
        icCIF
        icDuties
        icLandCost
        icSpec1
        icSpec2
        icSpec3
        icLotNo
        icMPQ
        icOrdered
        icShipped
        icReceivedQty
        icRejQty
        icRemarks
        icNoName
    End Enum
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnSorting = False
        blnPaging = False
        'SetGridProperty(dtgGrnDtl)
        SetGridProperty(dtgGrnDtlStk)

        'Session("CompanyId") = "PPSB"  

        Dim LocDesc2 As String
        Dim SubLocDesc2 As String

        If Not IsPostBack Then
            Session("jqPopup") = ""
        End If

        objINV.GetLocationDesc(LocDesc, SubLocDesc)
        lblDL.Text = IIf(LocDesc = "", "Default Location", "Default " + LocDesc)
        lblSDL.Text = IIf(SubLocDesc = "", "Default Sub Location", "Default " + SubLocDesc)

        objINV.GetDefaultLocationDesc(LocDesc2, SubLocDesc2)
        lblDefaultLocation.Text = LocDesc2
        lblDefaultSubLocation.Text = SubLocDesc2

        BindGrid()
        GenerateTab()
        'lnkBack.NavigateUrl = "GRNSearch.aspx?type=" & Request.QueryString("type") & "&pageid=" & strPageId
        'lnkBack.NavigateUrl = "javascript:history.back();"
        strFrm = Me.Request.QueryString("Frm")
        If strFrm = "GRNSearch" Then
            'lnkBack.NavigateUrl = "GRNSearch.aspx?Frm=GRNDetails&pageid=" & strPageId
            lnkBack.NavigateUrl = dDispatcher.direct("GRN", "GRNSearch.aspx", "Frm=GRNDetails&pageid=" & strPageId)
            'Michelle (30/9/2010) - To link back to Dashboard if calling page is from Dashboard
        ElseIf strFrm = "Dashboard" Then
            'lnkBack.NavigateUrl = "AddGRN1.aspx?Frm=Dashboard&pageid=" & strPageId & "&DOIndex=" & Me.Request.QueryString("OriDOIndex") & "&DONo=" & Me.Request.QueryString("OriDONo") & "&PONo=" & Me.Request.QueryString("OriPONo") & "&mode=" & Request.QueryString("mode")
            lnkBack.NavigateUrl = dDispatcher.direct("GRN", "AddGRN1.aspx", "Frm=Dashboard&pageid=" & strPageId & "&DOIndex=" & Me.Request.QueryString("OriDOIndex") & "&DONo=" & Me.Request.QueryString("OriDONo") & "&PONo=" & Me.Request.QueryString("OriPONo") & "&mode=" & Request.QueryString("mode"))

        ElseIf strFrm = "AddGRN" Then
            'lnkBack.NavigateUrl = "AddGRN.aspx?Frm=GRNDetails&pageid=" & strPageId
            lnkBack.NavigateUrl = dDispatcher.direct("GRN", "AddGRN.aspx", "Frm=GRNDetails&pageid=" & strPageId)
        ElseIf strFrm = "InvoiceTrackingList" Then
            'lnkBack.NavigateUrl = "../Tracking/InvoiceTrackingList.aspx?Frm=GRNDetails&pageid=" & strPageId
            lnkBack.NavigateUrl = dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "Frm=GRNDetails&pageid=" & strPageId)
        ElseIf strFrm = "InvoiceVerifiedTrackingList" Then
            'lnkBack.NavigateUrl = "../Tracking/InvoiceVerifiedTrackingList.aspx?Frm=GRNDetails&pageid=" & strPageId
            lnkBack.NavigateUrl = dDispatcher.direct("Tracking", "InvoiceVerifiedTrackingList.aspx", "Frm=GRNDetails&pageid=" & strPageId)
        ElseIf strFrm = "InvoicePaidTrackingList" Then
            'lnkBack.NavigateUrl = "../Tracking/InvoicePaidTrackingList.aspx?Frm=GRNDetails&pageid=" & strPageId
            lnkBack.NavigateUrl = dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "Frm=GRNDetails&pageid=" & strPageId)
        ElseIf strFrm = "PODetail" Then
            If Me.Request.QueryString("SubFrm") = "GRNSearch" Then
                'lnkBack.NavigateUrl = "../PO/PODetail.aspx?Frm=GRNSearch&caller=buyer&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&side=other&filetype=2"
                lnkBack.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=GRNSearch&caller=buyer&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&side=other&filetype=2")
            ElseIf Me.Request.QueryString("SubFrm") = "InvList" Then
                'lnkBack.NavigateUrl = "../PO/PODetail.aspx?Frm=InvList&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&side=otherv&filetype=2"
                lnkBack.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=InvList&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&side=otherv&filetype=2")
            ElseIf Me.Request.QueryString("SubFrm") = "InvoiceTrackingList" Then
                'lnkBack.NavigateUrl = "../PO/PODetail.aspx?Frm=InvoiceTrackingList&caller=buyer&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&side=other&filetype=2"
                lnkBack.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=InvoiceTrackingList&caller=buyer&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&side=other&filetype=2")
            ElseIf Me.Request.QueryString("SubFrm") = "InvoiceVerifiedTrackingList" Then
                lnkBack.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=InvoiceVerifiedTrackingList&caller=buyer&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&side=other&filetype=2")

            ElseIf Me.Request.QueryString("SubFrm") = "InvoicePaidTrackingList" Then
                'lnkBack.NavigateUrl = "../PO/PODetail.aspx?Frm=InvoicePaidTrackingList&caller=buyer&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&side=other&filetype=2"
                lnkBack.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=InvoicePaidTrackingList&caller=buyer&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&side=other&filetype=2")
            ElseIf Me.Request.QueryString("SubFrm") = "POVIEWB2" Then
                'lnkBack.NavigateUrl = "../PO/PODetail.aspx?Frm=POViewB2&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&status=" & dv("POM_PO_STATUS") & "&Caller=POviewB2&side=b&filetype=2&type=" & Request(Trim("Type")) & "&poview=1"
                'lnkBack.NavigateUrl = "../PO/PODetail.aspx?Frm=POViewB2&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&Caller=POviewB2&side=b&filetype=2" & "&poview=1"
                lnkBack.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=POViewB2&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&Caller=POviewB2&side=b&filetype=2" & "&poview=1")
            ElseIf Me.Request.QueryString("SubFrm") = "POVendorList" Then
                'lnkBack.NavigateUrl = "../PO/PODetail.aspx?Frm=POViewB2&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&status=" & dv("POM_PO_STATUS") & "&Caller=POviewB2&side=b&filetype=2&type=" & Request(Trim("Type")) & "&poview=1"
                'lnkBack.NavigateUrl = "../PO/PODetail.aspx?Frm=POVendorList&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&side=v&filetype=2"
                lnkBack.NavigateUrl = dDispatcher.direct("PO", "PODetail.aspx", "Frm=POVendorList&PO_INDEX=" & Request.QueryString("poidx") & "&PO_NO=" & Request(Trim("PO_No")) & "&BCoyID=" & Request.QueryString("BCoyID") & "&pageid=" & strPageId & "&side=v&filetype=2")
            End If

        ElseIf strFrm = "AddGRN1" Then
            'lnkBack.NavigateUrl = "AddGRN1.aspx?Frm=GRNDetails&pageid=" & strPageId & "&DOIndex=" & Me.Request.QueryString("OriDOIndex") & "&DONo=" & Me.Request.QueryString("OriDONo") & "&PONo=" & Me.Request.QueryString("OriPONo") & "&mode=" & Request.QueryString("mode")
            lnkBack.NavigateUrl = dDispatcher.direct("GRN", "AddGRN1.aspx", "Frm=GRNDetails&pageid=" & strPageId & "&DOIndex=" & Me.Request.QueryString("OriDOIndex") & "&DONo=" & Me.Request.QueryString("OriDONo") & "&PONo=" & Me.Request.QueryString("OriPONo") & "&mode=" & Request.QueryString("mode"))
        ElseIf strFrm = "ROSearch" Then
            'lnkBack.NavigateUrl = "AddGRN1.aspx?Frm=GRNDetails&pageid=" & strPageId & "&DOIndex=" & Me.Request.QueryString("OriDOIndex") & "&DONo=" & Me.Request.QueryString("OriDONo") & "&PONo=" & Me.Request.QueryString("OriPONo") & "&mode=" & Request.QueryString("mode")
            lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "ReturnOutwardSearch.aspx", "pageid=" & strPageId)
        End If
        'lnkBack.NavigateUrl = "GRNSearch.aspx?type=grn&pageid=" & strPageId

        'Michelle (23/1/2013)- Issue 1727
        Dim dvFile As DataView
        Dim intLoop, intCount As Integer
        Dim strFile, strFile1, strURL, strTemp As String
        dvFile = objDO.getDOAttachment(ViewState("DONo"), ViewState("VCoyID")).Tables(0).DefaultView
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

        intPageRecordCnt = ViewState("intPageRecordCnt")
        Me.cmdPreviewGRN.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewGRN.aspx", "PONo=" & Session("PONo") & "&GRNNo=" & Session("GRNNo") & "&DONo=" & Session("DONo")) & "&BCoyID=" & Me.Request.QueryString("BCoyID") & "')")

    End Sub

    'Sub dtgGrnDtl_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgGrnDtl.PageIndexChanged
    '    dtgGrnDtl.CurrentPageIndex = e.NewPageIndex
    '    BindGrid()
    'End Sub

    Sub dtgGrnDtlStk_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgGrnDtlStk.PageIndexChanged
        dtgGrnDtlStk.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    'Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgGrnDtl.SortCommand
    '    Grid_SortCommand(sender, e)
    '    dtgGrnDtl.CurrentPageIndex = 0
    '    BindGrid()
    'End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgGrnDtlStk.SortCommand
        Grid_SortCommand(sender, e)
        dtgGrnDtlStk.CurrentPageIndex = 0
        BindGrid()
    End Sub
    'Private Sub dtgGrnDtl_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGrnDtl.ItemCreated
    '    '//this line must be included
    '    Grid_ItemCreated(dtgGrnDtl, e)
    'End Sub

    Private Sub dtgGrnDtlStk_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGrnDtlStk.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgGrnDtlStk, e)
    End Sub

    'Private Sub dtgGrnDtl_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGrnDtl.ItemDataBound
    '    If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
    '        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

    '        '------New code Adding to get the PoLine in the Griddtg "dtgGrnDtl" By Praveen on 18/07/2007
    '        e.Item.Cells(1).Text = e.Item.DataSetIndex + 1
    '        e.Item.Cells(EnumGRNDet.icReceivedQty).Text = dv("GD_RECEIVED_QTY") - dv("GD_REJECTED_QTY")
    '        '--------End

    '        '//to dynamic build hyperlink
    '        '//assume that if from buyer than "side" is empty
    '        '//if from vendor that "side"=v
    '        If Request.QueryString("side") <> "v" Then
    '            If dv("GM_GRN_LEVEL") = "2" Then
    '                Dim lnkLevel1 As New HyperLink
    '                'lnkLevel1.ImageUrl = "../Images/i_edit.gif"
    '                'lnkLevel1.NavigateUrl = "GRNACKDetail.aspx?GrnIdx=" & dv("GM_GRN_INDEX") & "&GrnPoLine=" & dv("GD_PO_LINE") & "&pageid=" & strPageId
    '                'lnkLevel1.ToolTip = "Click here to view first level GRN Details"
    '                'e.Item.Cells(0).Controls.Add(lnkLevel1)
    '                'i_edit.gif
    '                Dim lnkDetails As HyperLink
    '                lnkDetails = e.Item.FindControl("lnkDetails")
    '                'lnkDetails.ImageUrl = "../Images/i_edit.gif"
    '                lnkDetails.ImageUrl = dDispatcher.direct("Plugins/images", "i_edit.gif")
    '                'lnkDetails.NavigateUrl = "GRNACKDetail.aspx?GRNNo=" & Me.Request.QueryString("GRNNo") & "&GrnIdx=" & dv("GM_GRN_INDEX") & "&GrnPoLine=" & dv("GD_PO_LINE") & "&pageid=" & strPageId
    '                lnkDetails.NavigateUrl = dDispatcher.direct("GRN", "GRNACKDetail.aspx", "GRNNo=" & Me.Request.QueryString("GRNNo") & "&GrnIdx=" & dv("GM_GRN_INDEX") & "&GrnPoLine=" & dv("GD_PO_LINE") & "&pageid=" & strPageId)
    '            End If
    '        End If
    '    End If
    'End Sub

    Private Sub dtgGrnDtlStk_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGrnDtlStk.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            '------New code Adding to get the PoLine in the Griddtg "dtgGrnDtl" By Praveen on 18/07/2007
            e.Item.Cells(1).Text = e.Item.DataSetIndex + 1
            e.Item.Cells(EnumGRNDetStk.icReceivedQty).Text = dv("GD_RECEIVED_QTY") - dv("GD_REJECTED_QTY")
            '--------End

            e.Item.Cells(EnumGRNDetStk.icUnitPriceF).Text = Format(dv("POD_UNIT_COST"), "###,###,##0.0000")

            Court_loc = Court_loc + 1
            Dim strJQLoc As String = "jqLoc" & Court_loc
            If DisplayLocPopup(strJQLoc, dv("POD_PO_LINE")) Then
                e.Item.Cells(EnumGRNDetStk.icShipped).Text = dv("DOD_SHIPPED_QTY") & "<span style=""cursor:default;"" class=""" & strJQLoc & """><IMG src=""" & dDispatcher.direct("Plugins/images", "d_icon.gif") & """></span>"
            Else
                e.Item.Cells(EnumGRNDetStk.icShipped).Text = dv("DOD_SHIPPED_QTY")
            End If

            'Disoount Amount
            Dim DISC_AMT As Decimal
            Dim lbl_DiscAmt As Label
            DISC_AMT = CalDiscAmt(dv("POD_VENDOR_ITEM_CODE"), dv("POM_S_COY_ID"), dv("POD_ORDERED_QTY"))
            lbl_DiscAmt = e.Item.FindControl("lblDiscAmt")
            lbl_DiscAmt.Text = Format(DISC_AMT, "###,###,##0.00")

            'Amount F
            Dim lblAmountF As Label
            Dim dblAmtF As Decimal
            Dim dblOthCharge As Decimal

            If IsDBNull(dv("GD_OTH_CHARGE")) Then
                dblOthCharge = 0
                e.Item.Cells(EnumGRNDetStk.icOthCharges).Text = 0
            Else
                dblOthCharge = CDec(dv("GD_OTH_CHARGE"))
                e.Item.Cells(EnumGRNDetStk.icOthCharges).Text = Format(dv("GD_OTH_CHARGE"), "###,###,##0.00")
            End If
            lblAmountF = e.Item.FindControl("lblAmountF")
            dblAmtF = (CDec(dv("POD_UNIT_COST")) * (CDec(dv("GD_RECEIVED_QTY")) - CDec(dv("GD_REJECTED_QTY")))) + dblOthCharge
            lblAmountF.Text = Format(dblAmtF, "###,###,##0.00")

            'Amount M
            Dim lblAmountM As Label
            Dim dblAmtM As Decimal
            Dim dblRate As Decimal

            If IsDBNull(dv("GD_EXCHANGE_RATE")) Then
                dblRate = 1
            Else
                dblRate = CDec(dv("GD_EXCHANGE_RATE"))
            End If
            lblAmountM = e.Item.FindControl("lblAmountM")
            dblAmtM = dblAmtF * dblRate
            lblAmountM.Text = Format(dblAmtM, "###,###,##0.00")

            'GRN Factor
            Dim lblGRNFac As Label
            Dim dblFactor As Decimal
            Dim dblGRNFactor As Decimal

            If IsDBNull(dv("GD_FACTOR")) Then
                dblGRNFactor = 0
            Else
                dblGRNFactor = CDec(dv("GD_FACTOR"))
            End If
            lblGRNFac = e.Item.FindControl("lblGRNFac")
            dblFactor = dblAmtM * dblGRNFactor / 100
            lblGRNFac.Text = Format(dblFactor, "###,###,##0.0000")

            'Unit Cost(M)
            Dim UNIT_COST As Decimal
            Dim Qty As Decimal
            Qty = CDec(dv("GD_RECEIVED_QTY")) - CDec(dv("GD_REJECTED_QTY"))
            Dim lbl_UnitCost As Label
            If Qty = 0 Then
                UNIT_COST = (CDec(dv("POD_UNIT_COST")) * dblRate) + 0
            Else
                UNIT_COST = (CDec(dv("POD_UNIT_COST")) * dblRate) + (dblFactor / Qty)
            End If

            lbl_UnitCost = e.Item.FindControl("lblUnitCost")
            lbl_UnitCost.Text = Format(UNIT_COST, "###,###,##0.00")

            'CIF Values
            Dim CIF_VAL As Decimal
            Dim dblInland As Decimal
            Dim lblCIF As Label

            If IsDBNull(dv("GD_INLAND_CHARGE")) Then
                dblInland = 0
                e.Item.Cells(EnumGRNDetStk.icInland).Text = 0
            Else
                dblInland = CDec(dv("GD_INLAND_CHARGE"))
                e.Item.Cells(EnumGRNDetStk.icInland).Text = Format(dv("GD_INLAND_CHARGE"), "###,###,##0.00")
            End If
            CIF_VAL = dblFactor + dblInland
            lblCIF = e.Item.FindControl("lblCIF")
            lblCIF.Text = Format(CIF_VAL, "###,###,##0.00")

            If TotalCIF = 0 Then
                TotalCIF = Format(CIF_VAL, "##0.00")
            Else
                TotalCIF = Format(CIF_VAL, "##0.00") + TotalCIF
            End If

            'Landed Cost
            Dim LAND_COST As Decimal
            Dim dblDuties As Decimal
            Dim lbl_LandCost As Label

            If IsDBNull(dv("GD_DUTIES")) Then
                dblDuties = 0
                e.Item.Cells(EnumGRNDetStk.icDuties).Text = 0
            Else
                dblDuties = CDec(dv("GD_DUTIES"))
                e.Item.Cells(EnumGRNDetStk.icDuties).Text = Format(dv("GD_DUTIES"), "###,###,##0.00")
            End If
            LAND_COST = Format(CIF_VAL, "##0.00") + dblDuties
            lbl_LandCost = e.Item.FindControl("lblLandCost")
            lbl_LandCost.Text = Format(LAND_COST, "###,###,##0.00")

            If TotalLandCost = 0 Then
                TotalLandCost = Format(LAND_COST, "##0.00")
            Else
                TotalLandCost = Format(LAND_COST, "##0.00") + TotalLandCost
            End If

            If Session("blnGRNTrue") = False Then
                Dim lbl_LotNo As Label
                lbl_LotNo = e.Item.FindControl("lblLotNo")
                lbl_LotNo.Text = dv("DOD_DO_LOT_NO") & "<A onclick=""PopWindow('" & dDispatcher.direct("GRN", "GRNLotList.aspx", "pageid=" & strPageId) & "&DONo=" & Server.UrlEncode(dv("DOM_DO_NO")) & "&GRNNo=" & Me.Request.QueryString("GRNNo") & "&itemcode=" & Server.UrlEncode(dv("POD_Vendor_Item_code")) & "&poline=" & Server.UrlEncode(dv("POD_PO_LINE")) & "&SCoyID=" & dv("POM_S_COY_ID") & "&BCoyID=" & Session("CompanyId") & "');"" ><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "d_icon.gif") & "></A>"
            End If

            '//to dynamic build hyperlink
            '//assume that if from buyer than "side" is empty
            '//if from vendor that "side"=v
            If Request.QueryString("side") <> "v" Then
                If dv("GM_GRN_LEVEL") = "2" Then
                    Dim lnkLevel1 As New HyperLink
                    Dim lnkDetails As HyperLink
                    lnkDetails = e.Item.FindControl("lnkDetails")
                    lnkDetails.ImageUrl = dDispatcher.direct("Plugins/images", "i_edit.gif")
                    lnkDetails.NavigateUrl = dDispatcher.direct("GRN", "GRNACKDetail.aspx", "GRNNo=" & Me.Request.QueryString("GRNNo") & "&GrnIdx=" & dv("GM_GRN_INDEX") & "&GrnPoLine=" & dv("GD_PO_LINE") & "&pageid=" & strPageId)
                End If
            End If
        End If
    End Sub
    Private Function BindGrid()
        Dim dsGRNDtl As DataSet
        Dim intTotRecord, i As Integer

        strGRNNo = Me.Request.QueryString("GRNNo")
        strBCoyID = Me.Request.QueryString("BCoyID")
        intDOIndex = Request.QueryString("DOIdx")
        dsGRNDtl = objGRN_Ext.GetGRNHistory(strGRNNo, strBCoyID)

        If dsGRNDtl.Tables(0).Rows.Count > 0 Then
            lblPONo.Text = Common.parseNull(dsGRNDtl.Tables(0).Rows(0)("POM_PO_NO"))
            lblDONo.Text = Common.parseNull(dsGRNDtl.Tables(0).Rows(0)("DOM_DO_NO"))
            lblPODate.Text = Common.parseNull(dsGRNDtl.Tables(0).Rows(0)("POM_PO_DATE"))
            lblGrnNo.Text = strGRNNo

            Session("PONo") = Trim(lblPONo.Text)
            Session("DONo") = Trim(lblDONo.Text)
            Session("GRNNo") = Trim(lblGrnNo.Text)
            If dsGRNDtl.Tables(0).Rows.Count > 0 Then
                lblDtREceived.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dsGRNDtl.Tables(0).Rows(0)("GM_DATE_RECEIVED"))
                lblGRNDate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dsGRNDtl.Tables(0).Rows(0)("GM_CREATED_DATE"))
                lblCreatedBy.Text = dsGRNDtl.Tables(0).Rows(0)("GRN_Created_Name")

            End If

            '//for sorting asc or desc
            Dim dvViewGrnAck As DataView
            dvViewGrnAck = dsGRNDtl.Tables(0).DefaultView

            'If pSorted Then
            dvViewGrnAck.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" Then dvViewGrnAck.Sort += " DESC"

            intPageRecordCnt = dsGRNDtl.Tables(0).Rows.Count
            ViewState("intPageRecordCnt") = intPageRecordCnt

            ViewState("VCoyID") = dsGRNDtl.Tables(0).Rows(0)("POM_S_COY_ID")
            ViewState("DONo") = dsGRNDtl.Tables(0).Rows(0)("DOM_DO_NO")
            If ViewState("intPageRecordCnt") > 0 Then
                'intTotPage = dtgDO.PageCount
                'If dsGRNDtl.Tables(0).Rows(0)("POD_ITEM_TYPE") = "ST" Then
                '    dtgGrnDtlStk.DataSource = dvViewGrnAck
                '    dtgGrnDtlStk.DataBind()
                'Else
                '    dtgGrnDtl.DataSource = dvViewGrnAck
                '    dtgGrnDtl.DataBind()
                'End If

                dtgGrnDtlStk.DataSource = dvViewGrnAck
                dtgGrnDtlStk.DataBind()

                If Request.QueryString("side") <> "v" Then
                    If dvViewGrnAck(0)("GM_GRN_LEVEL") = "1" Then
                        'dtgGrnDtl.Columns(EnumGRNDet.icNoName).Visible = False
                        dtgGrnDtlStk.Columns(EnumGRNDetStk.icNoName).Visible = False
                    End If
                Else
                    'dtgGrnDtl.Columns(EnumGRNDet.icNoName).Visible = False
                    dtgGrnDtlStk.Columns(EnumGRNDetStk.icNoName).Visible = False
                End If
            End If


            If Not IsDBNull(dsGRNDtl.Tables(0).Rows(0)("POD_ITEM_TYPE")) Then
                If dsGRNDtl.Tables(0).Rows(0)("POD_ITEM_TYPE") = "ST" Then
                    Session("blnGRNTrue") = False
                Else
                    Session("blnGRNTrue") = True
                End If
            Else
                Session("blnGRNTrue") = True
            End If

            'Display default loc / sub loc 
            tr_SEH1.Style("display") = "none"
            For i = 0 To dsGRNDtl.Tables(0).Rows.Count - 1
                If Not IsDBNull(dsGRNDtl.Tables(0).Rows(i)("POD_ITEM_TYPE")) Then
                    If dsGRNDtl.Tables(0).Rows(i)("POD_ITEM_TYPE") = "ST" Or dsGRNDtl.Tables(0).Rows(i)("POD_ITEM_TYPE") = "MI" Then
                        tr_SEH1.Style("display") = ""
                        Exit For
                    End If
                End If
            Next

            If Session("blnGRNTrue") = True Then
                'dtgGrnDtl.Visible = True
                'dtgGrnDtlStk.Visible = False
                'lblGrn.Visible = False
                'lblGrnValue.Visible = False
                'tr_SEH1.Style("display") = "none"
                dtgGrnDtlStk.Columns(EnumGRNDetStk.icLotNo).Visible = False
            Else
                'dtgGrnDtl.Visible = False
                'dtgGrnDtlStk.Visible = True
                'lblGrn.Visible = True
                'lblGrnValue.Visible = True
                'tr_SEH1.Style("display") = ""
                dtgGrnDtlStk.Columns(EnumGRNDetStk.icLotNo).Visible = True
            End If

            lblGrnValue.Text = Format(TotalLandCost, "###,###,##0.00")
            AddRow()
        Else
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        'If intGRNLevel = "1" Then
        '    dtgGrnDtl.Columns(10).Visible = False
        'End If
        'Me.dtgGrnDtl.Columns(5).Visible = False
    End Function

    'Private Sub dtgGrnDtl_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtgGrnDtl.SelectedIndexChanged

    'End Sub

    Function DisplayLocPopup(ByVal strJQ As String, ByVal intPOLine As Integer) As Boolean
        Dim ds As New DataSet
        Dim i As Integer
        Dim aryTemp As New ArrayList()
        Dim strLoc, strSubLoc, strQty, strLotNo As String

        strJQ = Replace(strJQ, "jq", "")
        ds = objGRN_Ext.GetLocInventory(Session("GRNNo"), intPOLine)
        aryTemp.Clear()

        If ds.Tables(0).Rows.Count > 0 Then
            For i = 0 To ds.Tables(0).Rows.Count - 1
                strLoc = Common.parseNull(ds.Tables(0).Rows(i).Item("LM_LOCATION"))
                strSubLoc = Common.parseNull(ds.Tables(0).Rows(i).Item("LM_SUB_LOCATION"))
                strQty = Common.parseNull(ds.Tables(0).Rows(i).Item("GL_LOT_RECEIVED_QTY"))
                strLotNo = Common.parseNull(ds.Tables(0).Rows(i).Item("DOL_LOT_NO"))

                aryTemp.Add(New String() {strLoc, strSubLoc, strQty, strLotNo})
            Next

            ContructRow(strJQ, aryTemp)
        Else
            Return False
        End If

        DisplayLocPopup = True

        'strLoc = "A"
        'strSubLoc = "AB"
        'strQty = "12"

        'aryTemp.Add(New String() {strLoc, strSubLoc, strQty})

        'strLoc = "A"
        'strSubLoc = "ABC"
        'strQty = "7"

        'aryTemp.Add(New String() {strLoc, strSubLoc, strQty})
        'ContructRow(strJQ, aryTemp)

    End Function

    Private Function ContructRow(ByVal strTemp As String, ByVal aryVolume As ArrayList) As String
        Dim strrow, strtable As String
        Dim i As Integer


        For i = 0 To aryVolume.Count - 1
            'If aryVolume(i)(3) = strVCompLine Then
            strrow &= IIf(LocDesc = "", "Location", LocDesc) & ": " & aryVolume(i)(0) & ", " & IIf(SubLocDesc = "", "Sub Location", SubLocDesc) & ": " & aryVolume(i)(1) & ", Qty: " & aryVolume(i)(2) & ", Lot No: " & aryVolume(i)(3) & "<BR>"
            'End If
        Next

        strtable = strrow

        Session("jqPopup") = Session("jqPopup") & "$('.jq" & strTemp & "').CreateBubblePopup({innerHtml: '" & strtable & "',position:'left', align: 'middle', innerHtmlStyle: { 'text-align':'left' },themeName:'all-black',themePath:'../../Common/Plugins/images/jquerybubblepopup-theme'});"

    End Function

    'Private Sub PreviewGRN()
    '    Dim ds As New DataSet
    '    Dim conn As MySqlConnection = Nothing
    '    Dim cmd As MySqlCommand = Nothing
    '    Dim da As MySqlDataAdapter = Nothing
    '    Dim rdr As MySqlDataReader = Nothing
    '    Dim myConnectionString As String
    '    Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
    '    Dim objFile As New FileManagement
    '    Dim strImgSrc As String

    '    strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), System.AppDomain.CurrentDomain.BaseDirectory & "images\")

    '    Try

    '        myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

    '        conn = New MySqlConnection(myConnectionString)
    '        conn.Open()

    '        cmd = New MySqlCommand
    '        With cmd
    '            .Connection = conn
    '            .CommandType = CommandType.Text
    '            .CommandText = "SELECT   PO_MSTR.POM_PO_INDEX, PO_MSTR.POM_PO_NO, PO_MSTR.POM_B_COY_ID, PO_MSTR.POM_BUYER_ID, " _
    '                         & "PO_MSTR.POM_BUYER_NAME, PO_MSTR.POM_BUYER_PHONE, PO_MSTR.POM_BUYER_FAX, " _
    '                         & "PO_MSTR.POM_S_COY_ID, PO_MSTR.POM_S_COY_NAME, PO_MSTR.POM_S_ATTN, " _
    '                         & "PO_MSTR.POM_S_REMARK, PO_MSTR.POM_S_ADDR_LINE1, PO_MSTR.POM_S_ADDR_LINE2, " _
    '                         & "PO_MSTR.POM_S_ADDR_LINE3, PO_MSTR.POM_S_POSTCODE, PO_MSTR.POM_S_CITY, " _
    '                         & "PO_MSTR.POM_S_STATE, PO_MSTR.POM_S_COUNTRY, PO_MSTR.POM_S_PHONE, PO_MSTR.POM_S_FAX, " _
    '                         & "PO_MSTR.POM_S_EMAIL, PO_MSTR.POM_PO_DATE, PO_MSTR.POM_FREIGHT_TERMS, " _
    '                         & "PO_MSTR.POM_PAYMENT_TERM, PO_MSTR.POM_PAYMENT_METHOD, PO_MSTR.POM_SHIPMENT_MODE, " _
    '                         & "PO_MSTR.POM_SHIPMENT_TERM, PO_MSTR.POM_CURRENCY_CODE, PO_MSTR.POM_EXCHANGE_RATE, " _
    '                         & "PO_MSTR.POM_PAYMENT_TERM_CODE, PO_MSTR.POM_SHIP_VIA, PO_MSTR.POM_PO_STATUS, " _
    '                         & "PO_MSTR.POM_STATUS_CHANGED_BY, PO_MSTR.POM_STATUS_CHANGED_ON, " _
    '                         & "PO_MSTR.POM_EXTERNAL_REMARK, PO_MSTR.POM_CREATED_BY, PO_MSTR.POM_PO_COST, " _
    '                         & "PO_MSTR.POM_BILLING_METHOD, PO_MSTR.POM_PO_PREFIX, PO_MSTR.POM_B_ADDR_CODE, " _
    '                         & "PO_MSTR.POM_B_ADDR_LINE1, PO_MSTR.POM_B_ADDR_LINE2, PO_MSTR.POM_B_ADDR_LINE3, " _
    '                         & "PO_MSTR.POM_B_POSTCODE, PO_MSTR.POM_B_CITY, PO_MSTR.POM_B_STATE, " _
    '                         & "PO_MSTR.POM_B_COUNTRY, PO_MSTR.POM_FULFILMENT, PO_MSTR.POM_DEPT_INDEX, " _
    '                         & "PO_MSTR.POM_ACCEPTED_DATE, PO_MSTR.POM_DOWNLOADED_DATE, PO_MSTR.POM_ARCHIVE_IND, " _
    '                         & "PO_MSTR.POM_TERMANDCOND, PO_MSTR.POM_REFERENCE_NO, PO_MSTR.POM_EXTERNAL_IND, " _
    '                         & "PO_DETAILS.POD_COY_ID, PO_DETAILS.POD_PO_NO, PO_DETAILS.POD_PO_LINE, " _
    '                         & "PO_DETAILS.POD_PRODUCT_CODE, PO_DETAILS.POD_VENDOR_ITEM_CODE, " _
    '                         & "PO_DETAILS.POD_PRODUCT_DESC, PO_DETAILS.POD_UOM, PO_DETAILS.POD_ORDERED_QTY, " _
    '                         & "PO_DETAILS.POD_RECEIVED_QTY, PO_DETAILS.POD_REJECTED_QTY, PO_DETAILS.POD_DELIVERED_QTY, " _
    '                         & "PO_DETAILS.POD_CANCELLED_QTY, PO_DETAILS.POD_MIN_PACK_QTY, PO_DETAILS.POD_MIN_ORDER_QTY, " _
    '                         & "PO_DETAILS.POD_ETD, PO_DETAILS.POD_WARRANTY_TERMS, PO_DETAILS.POD_UNIT_COST, " _
    '                         & "PO_DETAILS.POD_REMARK, PO_DETAILS.POD_GST, PO_DETAILS.POD_PR_INDEX, " _
    '                         & "PO_DETAILS.POD_PR_LINE, PO_DETAILS.POD_ACCT_INDEX, PO_DETAILS.POD_PRODUCT_TYPE, " _
    '                         & "PO_DETAILS.POD_B_ITEM_CODE, PO_DETAILS.POD_SOURCE, PO_DETAILS.POD_D_ADDR_CODE, " _
    '                         & "PO_DETAILS.POD_D_ADDR_LINE1, PO_DETAILS.POD_D_ADDR_LINE2, PO_DETAILS.POD_D_ADDR_LINE3, " _
    '                         & "PO_DETAILS.POD_D_POSTCODE, PO_DETAILS.POD_D_CITY, PO_DETAILS.POD_D_STATE, " _
    '                         & "PO_DETAILS.POD_D_COUNTRY, PO_DETAILS.POD_B_CATEGORY_CODE, PO_DETAILS.POD_B_GL_CODE, " _
    '                         & "DO_MSTR.DOM_DO_INDEX, DO_MSTR.DOM_DO_NO, DO_MSTR.DOM_S_COY_ID, DO_MSTR.DOM_DO_DATE, " _
    '                         & "DO_MSTR.DOM_S_REF_NO, DO_MSTR.DOM_S_REF_DATE, DO_MSTR.DOM_PO_INDEX, " _
    '                         & "DO_MSTR.DOM_WAYBILL_NO, DO_MSTR.DOM_FREIGHT_CARRIER, DO_MSTR.DOM_FREIGHT_AMT, " _
    '                         & "DO_MSTR.DOM_DO_REMARKS, DO_MSTR.DOM_DO_STATUS, DO_MSTR.DOM_CREATED_DATE, " _
    '                         & "DO_MSTR.DOM_CREATED_BY, DO_MSTR.DOM_NOOFCOPY_PRINTED, DO_MSTR.DOM_DO_PREFIX, " _
    '                         & "DO_MSTR.DOM_D_ADDR_CODE, DO_MSTR.DOM_D_ADDR_LINE1, DO_MSTR.DOM_D_ADDR_LINE2, " _
    '                         & "DO_MSTR.DOM_D_ADDR_LINE3, DO_MSTR.DOM_D_POSTCODE, DO_MSTR.DOM_D_CITY, " _
    '                         & "DO_MSTR.DOM_D_STATE, DO_MSTR.DOM_D_COUNTRY, DO_MSTR.DOM_EXTERNAL_IND, " _
    '                         & "DO_MSTR.DOM_REFERENCE_NO, DO_DETAILS.DOD_S_COY_ID, DO_DETAILS.DOD_DO_NO, " _
    '                         & "DO_DETAILS.DOD_DO_LINE, DO_DETAILS.DOD_PO_LINE, DO_DETAILS.DOD_DO_QTY, " _
    '                         & "DO_DETAILS.DOD_SHIPPED_QTY, DO_DETAILS.DOD_REMARKS, GRN_MSTR.GM_GRN_INDEX, " _
    '                         & "GRN_MSTR.GM_GRN_NO, GRN_MSTR.GM_B_COY_ID, GRN_MSTR.GM_PO_INDEX, " _
    '                         & "GRN_MSTR.GM_DATE_RECEIVED, GRN_MSTR.GM_NOOFCOPY_PRINTED, GRN_MSTR.GM_DO_INDEX, " _
    '                         & "GRN_MSTR.GM_INVOICE_NO, GRN_MSTR.GM_GRN_PREFIX, GRN_MSTR.GM_S_COY_ID,  " _
    '                         & "GRN_MSTR.GM_GRN_STATUS, GRN_MSTR.GM_DOWNLOADED_DATE, GRN_MSTR.GM_GRN_LEVEL, " _
    '                         & "GRN_MSTR.GM_LEVEL2_USER, GRN_MSTR.GM_CREATED_BY, GRN_MSTR.GM_CREATED_DATE,  " _
    '                         & "GRN_DETAILS.GD_B_COY_ID, GRN_DETAILS.GD_GRN_NO, GRN_DETAILS.GD_PO_LINE,  " _
    '                         & "GRN_DETAILS.GD_RECEIVED_QTY, GRN_DETAILS.GD_REJECTED_QTY, GRN_DETAILS.GD_REMARKS,  " _
    '                         & "USER_MSTR.UM_AUTO_NO, USER_MSTR.UM_USER_ID, USER_MSTR.UM_DELETED,  " _
    '                         & "USER_MSTR.UM_PASSWORD, USER_MSTR.UM_USER_NAME, USER_MSTR.UM_COY_ID,  " _
    '                         & "USER_MSTR.UM_DEPT_ID, USER_MSTR.UM_EMAIL, USER_MSTR.UM_APP_LIMIT,  " _
    '                         & "USER_MSTR.UM_DESIGNATION, USER_MSTR.UM_TEL_NO, USER_MSTR.UM_FAX_NO,  " _
    '                         & "USER_MSTR.UM_USER_SUSPEND_IND, USER_MSTR.UM_PASSWORD_LAST_CHANGED,  " _
    '                         & "USER_MSTR.UM_NEW_PASSWORD_IND, USER_MSTR.UM_NEXT_EXPIRE_DT,  " _
    '                         & "USER_MSTR.UM_LAST_LOGIN_DT, USER_MSTR.UM_QUESTION, USER_MSTR.UM_ANSWER,  " _
    '                         & "USER_MSTR.UM_MASS_APP, USER_MSTR.UM_STATUS, USER_MSTR.UM_MOD_BY,  " _
    '                         & "USER_MSTR.UM_MOD_DT, USER_MSTR.UM_ENT_BY, USER_MSTR.UM_ENT_DATE, " _
    '                         & "USER_MSTR.UM_RECORD_COUNT, USER_MSTR.UM_EMAIL_CC, USER_MSTR.UM_INVOICE_APP_LIMIT, " _
    '                         & "USER_MSTR.UM_INVOICE_MASS_APP, COMPANY_MSTR.CM_COY_ID, COMPANY_MSTR.CM_COY_NAME, " _
    '                         & "COMPANY_MSTR.CM_COY_TYPE, COMPANY_MSTR.CM_PARENT_COY_ID, COMPANY_MSTR.CM_ACCT_NO, " _
    '                         & "COMPANY_MSTR.CM_BANK, COMPANY_MSTR.CM_BRANCH, COMPANY_MSTR.CM_ADDR_LINE1, " _
    '                         & "COMPANY_MSTR.CM_ADDR_LINE2, COMPANY_MSTR.CM_ADDR_LINE3, COMPANY_MSTR.CM_POSTCODE, " _
    '                         & "COMPANY_MSTR.CM_CITY, COMPANY_MSTR.CM_STATE, COMPANY_MSTR.CM_COUNTRY, " _
    '                         & "COMPANY_MSTR.CM_PHONE, COMPANY_MSTR.CM_FAX, COMPANY_MSTR.CM_EMAIL, " _
    '                         & "COMPANY_MSTR.CM_COY_LOGO, COMPANY_MSTR.CM_BUSINESS_REG_NO, " _
    '                         & "COMPANY_MSTR.CM_TAX_REG_NO, COMPANY_MSTR.CM_PAYMENT_TERM, " _
    '                         & "COMPANY_MSTR.CM_PAYMENT_METHOD, COMPANY_MSTR.CM_ACTUAL_TERMSANDCONDFILE, " _
    '                         & "COMPANY_MSTR.CM_HUB_TERMSANDCONDFILE, COMPANY_MSTR.CM_PWD_DURATION, " _
    '                         & "COMPANY_MSTR.CM_TAX_CALC_BY, COMPANY_MSTR.CM_CURRENCY_CODE, " _
    '                         & "COMPANY_MSTR.CM_BCM_SET, COMPANY_MSTR.CM_BUDGET_FROM_DATE, " _
    '                         & "COMPANY_MSTR.CM_BUDGET_TO_DATE, COMPANY_MSTR.CM_RFQ_OPTION, " _
    '                         & "COMPANY_MSTR.CM_LICENCE_PACKAGE, COMPANY_MSTR.CM_LICENSE_USERS, " _
    '                         & "COMPANY_MSTR.CM_SUB_START_DT, COMPANY_MSTR.CM_SUB_END_DT, " _
    '                         & "COMPANY_MSTR.CM_LICENSE_PRODUCTS, COMPANY_MSTR.CM_FINDEPT_MODE, " _
    '                         & "COMPANY_MSTR.CM_PRIV_LABELING, COMPANY_MSTR.CM_SKINS_ID, COMPANY_MSTR.CM_TRAINING, " _
    '                         & "COMPANY_MSTR.CM_STATUS, COMPANY_MSTR.CM_DELETED, COMPANY_MSTR.CM_MOD_BY, " _
    '                         & "COMPANY_MSTR.CM_MOD_DT, COMPANY_MSTR.CM_ENT_BY, COMPANY_MSTR.CM_ENT_DT, " _
    '                         & "COMPANY_MSTR.CM_SKU, COMPANY_MSTR.CM_TRANS_NO, COMPANY_MSTR.CM_CONTACT, " _
    '                         & "COMPANY_MSTR.CM_REPORT_USERS, COMPANY_MSTR.CM_INV_APPR, COMPANY_MSTR.CM_MULTI_PO,  " _
    '                         & "COMPANY_MSTR.CM_BA_CANCEL, " _
    '                         & "(SELECT CODE_DESC " _
    '                         & "FROM CODE_MSTR AS a " _
    '                         & "WHERE (CODE_ABBR = COMPANY_MSTR.CM_STATE) AND (CODE_CATEGORY = 's') AND " _
    '                         & "(CODE_VALUE = COMPANY_MSTR.CM_COUNTRY)) AS CMState, " _
    '                         & "(SELECT CODE_DESC " _
    '                         & "FROM CODE_MSTR AS a " _
    '                         & "WHERE (CODE_ABBR = COMPANY_MSTR.CM_COUNTRY) AND (CODE_CATEGORY = 'ct')) AS CMCtry, " _
    '                         & "(SELECT CODE_DESC " _
    '                         & "FROM CODE_MSTR AS a " _
    '                         & "WHERE (CODE_ABBR = DO_MSTR.DOM_D_STATE) AND (CODE_CATEGORY = 's') AND " _
    '                         & "(CODE_VALUE = DO_MSTR.DOM_D_COUNTRY)) AS DelvAddrState, " _
    '                         & "(SELECT CODE_DESC " _
    '                         & "FROM  CODE_MSTR AS a " _
    '                         & "WHERE (CODE_ABBR = DO_MSTR.DOM_D_COUNTRY) AND (CODE_CATEGORY = 'ct')) " _
    '                         & "AS DelvAddrCtry " _
    '                         & "FROM PO_MSTR INNER JOIN " _
    '                         & "PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO AND " _
    '                         & "PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID INNER JOIN " _
    '                         & "DO_MSTR ON PO_MSTR.POM_PO_INDEX = DO_MSTR.DOM_PO_INDEX INNER JOIN " _
    '                         & "DO_DETAILS ON DO_MSTR.DOM_DO_NO = DO_DETAILS.DOD_DO_NO AND  " _
    '                         & "DO_MSTR.DOM_S_COY_ID = DO_DETAILS.DOD_S_COY_ID AND  " _
    '                         & "PO_DETAILS.POD_PO_LINE = DO_DETAILS.DOD_PO_LINE INNER JOIN " _
    '                         & "GRN_MSTR ON PO_MSTR.POM_PO_INDEX = GRN_MSTR.GM_PO_INDEX AND  " _
    '                         & "DO_MSTR.DOM_DO_INDEX = GRN_MSTR.GM_DO_INDEX INNER JOIN " _
    '                         & "GRN_DETAILS ON GRN_MSTR.GM_GRN_NO = GRN_DETAILS.GD_GRN_NO AND  " _
    '                         & "GRN_MSTR.GM_B_COY_ID = GRN_DETAILS.GD_B_COY_ID AND  " _
    '                         & "DO_DETAILS.DOD_PO_LINE = GRN_DETAILS.GD_PO_LINE INNER JOIN " _
    '                         & "USER_MSTR ON GRN_MSTR.GM_CREATED_BY = USER_MSTR.UM_USER_ID " _
    '                         & "AND GRN_MSTR.GM_B_COY_ID = user_mstr.UM_COY_ID INNER JOIN " _
    '                         & "COMPANY_MSTR ON PO_MSTR.POM_B_COY_ID = COMPANY_MSTR.CM_COY_ID " _
    '                         & "WHERE (PO_MSTR.POM_PO_NO =@prmPONo) AND (GRN_MSTR.GM_GRN_NO = @prmGRN) AND " _
    '                         & "(DO_MSTR.DOM_DO_NO = @prmDONo)"
    '        End With

    '        da = New MySqlDataAdapter(cmd)
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmPONo", Session("PONo")))
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmGRN", Session("GRNNo")))
    '        da.SelectCommand.Parameters.Add(New MySqlParameter("@prmDONo", Session("DONo")))

    '        da.Fill(ds)
    '        Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PreviewGRN_DataSetPreviewGRN", ds.Tables(0))
    '        Dim localreport As New LocalReport
    '        localreport.DataSources.Clear()
    '        localreport.DataSources.Add(rptDataSource)
    '        localreport.ReportPath = appPath & "GRN\PreviewGRN-FTN.rdlc"
    '        localreport.EnableExternalImages = True

    '        ' If strImgSrc <> "" Then
    '        Dim I As Byte
    '        Dim GetParameter As String = ""
    '        Dim TotalParameter As Byte
    '        TotalParameter = localreport.GetParameters.Count
    '        Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
    '        'Dim paramlist As New Generic.List(Of ReportParameter)
    '        For I = 0 To localreport.GetParameters.Count - 1
    '            GetParameter = localreport.GetParameters.Item(I).Name
    '            Select Case LCase(GetParameter)
    '                Case "grn_logo"
    '                    par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings("ReportCoyLogoPath") & strImgSrc)
    '                Case Else
    '            End Select
    '        Next
    '        localreport.SetParameters(par)
    '        'End If

    '        localreport.Refresh()

    '        'Dim deviceInfo As String = _
    '        '     "<DeviceInfo>" + _
    '        '         "  <OutputFormat>EMF</OutputFormat>" + _
    '        '         "  <PageWidth>8.27in</PageWidth>" + _
    '        '         "  <PageHeight>11in</PageHeight>" + _
    '        '         "  <MarginTop>0.25in</MarginTop>" + _
    '        '         "  <MarginLeft>0.25in</MarginLeft>" + _
    '        '         "  <MarginRight>0.25in</MarginRight>" + _
    '        '         "  <MarginBottom>0.25in</MarginBottom>" + _
    '        '         "</DeviceInfo>"
    '        Dim deviceInfo As String = _
    '                        "<DeviceInfo>" + _
    '                            "  <OutputFormat>EMF</OutputFormat>" + _
    '                            "</DeviceInfo>"
    '        Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)

    '        Dim fs As New FileStream(appPath & "GRN\GRNReport.PDF", FileMode.Create)
    '        fs.Write(PDF, 0, PDF.Length)
    '        fs.Close()

    '        Dim strJScript As String
    '        strJScript = "<script language=javascript>"
    '        strJScript += "window.open('GRNReport.PDF',null,'height=600, width=730,status= no, resizable= yes, scrollbars=yes, toolbar=no,location=center,menubar=no, top=10, left=100');"
    '        strJScript += "</script>"
    '        Response.Write(strJScript)

    '    Catch ex As Exception
    '    Finally
    '        cmd = Nothing
    '        If Not IsNothing(rdr) Then
    '            rdr.Close()
    '        End If
    '        If Not IsNothing(conn) Then
    '            If conn.State = ConnectionState.Open Then
    '                conn.Close()
    '            End If
    '        End If
    '        conn = Nothing
    '    End Try
    'End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        If Request.QueryString("Frm") = "InvList" Or Me.Request.QueryString("SubFrm") = "InvList" Then
            '    Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                               "<li><a class=""t_entity_btn_selected"" href=""../Invoice/InvList.aspx?pageid=" & strPageId & """><span>Issue Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                               "<li><a class=""t_entity_btn"" href=""../Invoice/invoiceView.aspx?pageid=" & strPageId & """><span>Invoice Listing</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                               "</ul><div></div></div>"

            'ElseIf Request.QueryString("Frm") = "InvoiceTrackingList" Or Request.QueryString("SubFrm") = "InvoiceTrackingList" Then
            '    Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                                "<li><a class=""t_entity_btn_selected"" href=""../Tracking/InvoiceTrackingList.aspx?pageid=" & strPageId & "&folder=N" & "&status=1" & """><span>New Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                "<li><a class=""t_entity_btn"" href=""../Tracking/InvoiceVerifiedTrackingList.aspx?folder=S" & "&status=1&pageid=" & strPageId & """><span>Verified Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                "<li><a class=""t_entity_btn"" href=""../Tracking/InvoicePaidTrackingList.aspx?pageid=" & strPageId & """><span>Paid Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                "</ul><div></div></div>"

            'ElseIf Request.QueryString("Frm") = "InvoiceVerifiedTrackingList" Or Request.QueryString("SubFrm") = "InvoiceVerifiedTrackingList" Then
            '    Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                                "<li><a class=""t_entity_btn"" href=""../Tracking/InvoiceTrackingList.aspx?pageid=" & strPageId & "&folder=N" & "&status=1" & """><span>New Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                "<li><a class=""t_entity_btn_selected"" href=""../Tracking/InvoiceVerifiedTrackingList.aspx?folder=S" & "&status=1&pageid=" & strPageId & """><span>Verified Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                "<li><a class=""t_entity_btn"" href=""../Tracking/InvoicePaidTrackingList.aspx?pageid=" & strPageId & """><span>Paid Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                "</ul><div></div></div>"

            'ElseIf Request.QueryString("Frm") = "InvoicePaidTrackingList" Or Request.QueryString("SubFrm") = "InvoicePaidTrackingList" Then
            '    Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                                "<li><a class=""t_entity_btn"" href=""../Tracking/InvoiceTrackingList.aspx?pageid=" & strPageId & "&folder=N" & "&status=1" & """><span>New Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                "<li><a class=""t_entity_btn"" href=""../Tracking/InvoiceVerifiedTrackingList.aspx?folder=S" & "&status=1&pageid=" & strPageId & """><span>Verified Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                "<li><a class=""t_entity_btn_selected"" href=""../Tracking/InvoicePaidTrackingList.aspx?pageid=" & strPageId & """><span>Paid Invoice</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                                "</ul><div></div></div>"

            'ElseIf Request.QueryString("Frm") = "POVIEWB2" Or Request.QueryString("SubFrm") = "POVIEWB2" Then
            '    Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                         "<li><a class=""t_entity_btn_selected"" href=""../PO/POViewB2.aspx?pageid=" & strPageId & """><span>Purchase Order</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                         "<li><a class=""t_entity_btn"" href=""../PO/POViewB2Cancel.aspx?type=Listing&pageid=" & strPageId & """><span>Purchase Order Cancellation</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                        "</ul><div></div></div>"

            'ElseIf Request.QueryString("Frm") = "POVendorList" Or Request.QueryString("SubFrm") = "POVendorList" Then
            '    Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                         "<li><a class=""t_entity_btn"" href=""../PO/POViewB.aspx?pageid=" & strPageId & """><span>Purchase Order</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                         "<li><a class=""t_entity_btn_selected"" href=""../PO/POVendorList.aspx?pageid=" & strPageId & """><span>PO Listing</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                        "</ul><div></div></div>"

            'ElseIf Request.QueryString("Frm") = "AddGRN1" Then
            '    Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                            "<li><a class=""t_entity_btn_selected"" href=""GRNList.aspx?pageid=" & strPageId & """><span>Issue GRN</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                            "<li><a class=""t_entity_btn"" href=""GRNSearch.aspx?pageid=" & strPageId & """><span>GRN Listing</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                           "</ul><div></div></div>"
            'Else
            '    Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
            '    "<li><div class=""space""></div></li>" & _
            '                               "<li><a class=""t_entity_btn"" href=""GRNList.aspx?pageid=" & strPageId & """><span>Issue GRN</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                               "<li><a class=""t_entity_btn_selected"" href=""GRNSearch.aspx?pageid=" & strPageId & """><span>GRN Listing</span></a></li>" & _
            '                         "<li><div class=""space""></div></li>" & _
            '                              "</ul><div></div></div>"
            '    Session("w_SearchGRN_tabs") = "<div class=""t_entity"">" & _
            '                   "<a class=""t_entity_btn_selected"" href=""../Invoice/InvList.aspx?pageid=" & strPageId & """><span>Issue Invoice</span></a>" & _
            '                   "<a class=""t_entity_btn"" href=""../Invoice/invoiceView.aspx?pageid=" & strPageId & """><span>Invoice Listing</span></a>" & _
            '                   "</div>"

            'ElseIf Request.QueryString("Frm") = "InvoiceTrackingList" Or Request.QueryString("SubFrm") = "InvoiceTrackingList" Then
            '    Session("w_SearchGRN_tabs") = "<div class=""t_entity"">" & _
            '                                "<a class=""t_entity_btn_selected"" href=""../Tracking/InvoiceTrackingList.aspx?pageid=" & strPageId & "&folder=N" & "&status=1" & """><span>New Invoice</span></a>" & _
            '                                "<a class=""t_entity_btn"" href=""../Tracking/InvoiceVerifiedTrackingList.aspx?folder=S" & "&status=1&pageid=" & strPageId & """><span>Verified Invoice</span></a>" & _
            '                                "<a class=""t_entity_btn"" href=""../Tracking/InvoicePaidTrackingList.aspx?pageid=" & strPageId & """><span>Paid Invoice</span></a>" & _
            '                                "</div>"

            'ElseIf Request.QueryString("Frm") = "InvoiceVerifiedTrackingList" Or Request.QueryString("SubFrm") = "InvoiceVerifiedTrackingList" Then
            '    Session("w_SearchGRN_tabs") = "<div class=""t_entity"">" & _
            '                                "<a class=""t_entity_btn"" href=""../Tracking/InvoiceTrackingList.aspx?pageid=" & strPageId & "&folder=N" & "&status=1" & """><span>New Invoice</span></a>" & _
            '                                "<a class=""t_entity_btn_selected"" href=""../Tracking/InvoiceVerifiedTrackingList.aspx?folder=S" & "&status=1&pageid=" & strPageId & """><span>Verified Invoice</span></a>" & _
            '                                "<a class=""t_entity_btn"" href=""../Tracking/InvoicePaidTrackingList.aspx?pageid=" & strPageId & """><span>Paid Invoice</span></a>" & _
            '                                "</div>"

            'ElseIf Request.QueryString("Frm") = "InvoicePaidTrackingList" Or Request.QueryString("SubFrm") = "InvoicePaidTrackingList" Then
            '    Session("w_SearchGRN_tabs") = "<div class=""t_entity"">" & _
            '                                "<a class=""t_entity_btn"" href=""../Tracking/InvoiceTrackingList.aspx?pageid=" & strPageId & "&folder=N" & "&status=1" & """><span>New Invoice</span></a>" & _
            '                                "<a class=""t_entity_btn"" href=""../Tracking/InvoiceVerifiedTrackingList.aspx?folder=S" & "&status=1&pageid=" & strPageId & """><span>Verified Invoice</span></a>" & _
            '                                "<a class=""t_entity_btn_selected"" href=""../Tracking/InvoicePaidTrackingList.aspx?pageid=" & strPageId & """><span>Paid Invoice</span></a>" & _
            '                                "</div>"

            'ElseIf Request.QueryString("Frm") = "POVIEWB2" Or Request.QueryString("SubFrm") = "POVIEWB2" Then
            '    Session("w_SearchGRN_tabs") = "<div class=""t_entity"">" & _
            '                         "<a class=""t_entity_btn_selected"" href=""../PO/POViewB2.aspx?pageid=" & strPageId & """><span>Purchase Order</span></a>" & _
            '                         "<a class=""t_entity_btn"" href=""../PO/POViewB2Cancel.aspx?type=Listing&pageid=" & strPageId & """><span>Purchase Order Cancellation</span></a>" & _
            '                        "</div>"

            'ElseIf Request.QueryString("Frm") = "POVendorList" Or Request.QueryString("SubFrm") = "POVendorList" Then
            '    Session("w_SearchGRN_tabs") = "<div class=""t_entity"">" & _
            '                         "<a class=""t_entity_btn"" href=""../PO/POViewB.aspx?pageid=" & strPageId & """><span>Purchase Order</span></a>" & _
            '                         "<a class=""t_entity_btn_selected"" href=""../PO/POVendorList.aspx?pageid=" & strPageId & """><span>PO Listing</span></a>" & _
            '                        "</div>"

            'ElseIf Request.QueryString("Frm") = "AddGRN1" Then
            '    Session("w_SearchGRN_tabs") = "<div class=""t_entity"">" & _
            '                            "<a class=""t_entity_btn_selected"" href=""GRNList.aspx?pageid=" & strPageId & """><span>Issue GRN</span></a>" & _
            '                            "<a class=""t_entity_btn"" href=""GRNSearch.aspx?pageid=" & strPageId & """><span>GRN Listing</span></a>" & _
            '                           "</div>"
            'Else
            '    Session("w_SearchGRN_tabs") = "<div class=""t_entity"">" & _
            '                               "<a class=""t_entity_btn"" href=""GRNList.aspx?pageid=" & strPageId & """><span>Issue GRN</span></a>" & _
            '                               "<a class=""t_entity_btn_selected"" href=""GRNSearch.aspx?pageid=" & strPageId & """><span>GRN Listing</span></a>" & _
            '                              "</div>"
            'End If
            Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                           "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Invoice", "InvList.aspx", "pageid=" & strPageId) & """><span>Issue Invoice</span></a></li>" & _
                           "<li><div class=""space""></div></li>" & _
                           "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Invoice", "invoiceView.aspx", "pageid=" & strPageId) & """><span>Invoice Listing</span></a></li>" & _
                           "<li><div class=""space""></div></li>" & _
                           "</ul><div></div></div>"

        ElseIf Request.QueryString("Frm") = "InvoiceTrackingList" Or Request.QueryString("SubFrm") = "InvoiceTrackingList" Then
            Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & "&folder=N" & "&status=1" & """><span>New Invoice</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceVerifiedTrackingList.aspx", "folder=S" & "&status=1&pageid=" & strPageId) & """><span>Verified Invoice</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "</ul><div></div></div>"

        ElseIf Request.QueryString("Frm") = "InvoiceVerifiedTrackingList" Or Request.QueryString("SubFrm") = "InvoiceVerifiedTrackingList" Then
            Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & "&folder=N" & "&status=1" & """><span>New Invoice</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li>a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceVerifiedTrackingList.aspx", "folder=S" & "&status=1&pageid=" & strPageId) & """><span>Verified Invoice</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li>a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoiceVerifiedTrackingList.aspx", "folder=S" & "&status=1&pageid=" & strPageId) & """><span>Verified Invoice</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "</ul><div></div></div>"

        ElseIf Request.QueryString("Frm") = "InvoicePaidTrackingList" Or Request.QueryString("SubFrm") = "InvoicePaidTrackingList" Then
            Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceTrackingList.aspx", "pageid=" & strPageId) & "&folder=N" & "&status=1" & """><span>New Invoice</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Tracking", "InvoiceVerifiedTrackingList.aspx", "folder=S" & "&status=1&pageid=" & strPageId) & """><span>Verified Invoice</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Tracking", "InvoicePaidTrackingList.aspx", "pageid=" & strPageId) & """><span>Paid Invoice</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "</ul><div></div></div>"

        ElseIf Request.QueryString("Frm") = "POVIEWB2" Or Request.QueryString("SubFrm") = "POVIEWB2" Then
            Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                                 "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB2Cancel.aspx", "type=Listing", "pageid=" & strPageId) & """><span>Purchase Order Cancellation</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                                "</ul><div></div></div>"

        ElseIf Request.QueryString("Frm") = "POVendorList" Or Request.QueryString("SubFrm") = "POVendorList" Then
            Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                                 "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POVendorList.aspx", "pageid=" & strPageId) & """><span>PO Listing</span></a></li>" & _
                                 "<li><div class=""space""></div></li>" & _
                                "</ul><div></div></div>"

        ElseIf Request.QueryString("Frm") = "AddGRN1" Then
            Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("GRN", "GRNList.aspx", "pageid=" & strPageId) & """><span>Issue GRN</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("GRN", "GRNSearch.aspx", "pageid=" & strPageId) & """><span>GRN Listing</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                   "</ul><div></div></div>"

        ElseIf Request.QueryString("Frm") = "ROSearch" Then
            Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "ReturnOutwardListing.aspx", "pageid=" & strPageId) & """><span>Return Outward</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "ReturnOutwardSearch.aspx", "pageid=" & strPageId) & """><span>Return Outward Listing</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                    "</ul><div></div></div>"
        Else
            Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("GRN", "GRNList.aspx", "pageid=" & strPageId) & """><span>Issue GRN</span></a></li>" & _
                                       "<li><div class=""space""></div></li>" & _
                                       "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("GRN", "GRNSearch.aspx", "pageid=" & strPageId) & """><span>GRN Listing</span></a></li>" & _
                                       "<li><div class=""space""></div></li>" & _
                                      "</ul><div></div></div>"
        End If

    End Sub

    'Private Sub cmdPreviewGRN_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPreviewGRN.ServerClick
    '    PreviewGRN()
    'End Sub

    Sub AddRow()

        Dim intL, intColToRemain, intCell As Integer
        Dim row As DataGridItem = New DataGridItem(-1, -1, ListItemType.Separator)
        Dim intTotalCol As Integer
        intCell = EnumGRNDetStk.icInland

        For intL = 0 To 5
            addCell(row)
        Next

        row.Cells(0).BorderWidth = 0
        row.Cells(1).BorderWidth = 0

        row.Cells(2).BorderWidth = 0
        row.Cells(2).Text = "Total:"
        row.Cells(2).Font.Bold = True
        row.Cells(2).ColumnSpan = intCell - 1
        row.Cells(2).HorizontalAlign = HorizontalAlign.Right

        row.Cells(3).BorderWidth = 0
        row.Cells(3).Text = Format(TotalCIF, "###,###,##0.00")
        row.Cells(3).Font.Bold = True
        row.Cells(3).HorizontalAlign = HorizontalAlign.Right

        row.Cells(4).BorderWidth = 0

        row.Cells(5).BorderWidth = 0
        row.Cells(5).Text = Format(TotalLandCost, "###,###,##0.00")
        row.Cells(5).Font.Bold = True
        row.Cells(5).HorizontalAlign = HorizontalAlign.Right

        dtgGrnDtlStk.Controls(0).Controls.Add(row)


    End Sub

    Sub addCell(ByRef row As DataGridItem)
        Dim cell As TableCell = New TableCell
        row.Cells.Add(cell)
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
End Class
