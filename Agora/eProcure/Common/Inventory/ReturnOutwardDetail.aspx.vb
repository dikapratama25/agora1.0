Imports AgoraLegacy
Imports eProcure.Component

Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class ReturnOutwardDetail
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents cmdView As System.Web.UI.WebControls.Button


    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strFrm, strRONo As String
    Dim objINV As New Inventory
    Dim intRow As Integer
    Dim dsAllInfo, ds As DataSet
    Dim Court_loc As Integer
    Dim LocDesc, SubLocDesc As String

    Public Enum EnumRO
        icLine
        icItemCode
        icItemName
        icUOM
        icMPQ
        icOrderQty
        icReceivedQty
        icRejectedQty
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
        'cmdSubmit.Enabled = False
        'Dim alButtonList As ArrayList
        'alButtonList = New ArrayList
        'alButtonList.Add(cmdSubmit)
        'alButtonList.Add(cmdReset)
        'htPageAccess.Add("update", alButtonList)
        'htPageAccess.Add("add", alButtonList)
        'If Request.QueryString("frm") <> "Dashboard" Then
        '    CheckButtonAccess()
        'End If
        'cmdSubmit.Enabled = True
        'cmdReset.Enabled = True
        'If Not ViewState("blnButtonState") Then
        '    ButtonProperty(False)
        'End If
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnPaging = False
        SetGridProperty(dtgRODtl)

        strPageId = Session("strPageId")
        strRONo = Me.Request.QueryString("RO_NO")
        strFrm = Me.Request.QueryString("frm")
        If strFrm = "ROSearch" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Inventory", "ReturnOutwardSearch.aspx", "pageid=" & strPageId)
        ElseIf strFrm = "Dashboard" Then
            lnkBack.NavigateUrl = dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "frm=Dashboard&pageid=0")
        End If
        lblRONo.Text = strRONo

        If Not Page.IsPostBack Then
            objINV.GetLocationDesc(LocDesc, SubLocDesc)
            dsAllInfo = objINV.ROInfo(lblRONo.Text)
            PopulateGRNHeader()
            GenerateTab()
            Bindgrid(True)
        End If

        Me.cmdView.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewRODetails.aspx", "RO_No=" & Request(Trim("RO_NO")) & "&CoyID=" & Session("CompanyID") & "") & "')")
    End Sub

    Sub ButtonProperty(ByVal blnEnable As Boolean)
        ViewState("blnButtonState") = blnEnable
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgRODtl.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgRODtl_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgRODtl.ItemCreated
        Grid_ItemCreated(dtgRODtl, e)
    End Sub

    Private Sub dtgRODtl_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgRODtl.ItemDataBound
        Dim dtgRODtl As DataSet
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            e.Item.Cells(EnumRO.icLine).Text = intRow + 1
            intRow = intRow + 1

            Court_loc = Court_loc + 1
            Dim strJQLoc As String = "jqLoc" & Court_loc
            If DisplayLocPopup(strJQLoc, dv("IROD_RO_LINE")) Then
                e.Item.Cells(EnumRO.icReturnQty).Text = dv("IROD_QTY") & "<span style=""cursor:default;"" class=""" & strJQLoc & """><IMG src=""" & dDispatcher.direct("Plugins/images", "d_icon.gif") & """></span>"
            Else
                e.Item.Cells(EnumRO.icReturnQty).Text = dv("IROD_QTY")
            End If
        End If
    End Sub

    Public Sub dtgRODtl_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgRODtl.ItemCommand
        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        Dim strName As String
        Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

        If (e.CommandSource).CommandName = "" Then
            strFileName = ""
            strName = ""
        End If
    End Sub

    Private Sub PopulateGRNHeader()
        Dim strMsg As String

        Dim dtHeader As New DataTable
        dtHeader = dsAllInfo.Tables("INVENTORY_RETURN_OUTWARD_MSTR")

        If dtHeader.Rows.Count > 0 Then
            lblRONo.Text = dtHeader.Rows(0)("IROM_RO_NO")
            lblRODate.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dtHeader.Rows(0)("IROM_RO_DATE"))
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

        Court_loc = 0
        intRow = 0
        Dim dvViewPR As DataView
        dvViewPR = dsAllInfo.Tables("INVENTORY_RETURN_OUTWARD_DETAILS").DefaultView

        dvViewPR.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPR.Sort += " DESC"

        ViewState("intPageRecordCnt") = dsAllInfo.Tables("INVENTORY_RETURN_OUTWARD_DETAILS").Rows.Count

        If ViewState("intPageRecordCnt") > 0 Then
            dtgRODtl.DataSource = dvViewPR
            dtgRODtl.DataBind()
        Else
            dtgRODtl.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtgRODtl.PageCount
    End Function

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_ReturnInward_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "ReturnOutwardListing.aspx", "pageid=" & strPageId) & """><span>Return Outward</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "ReturnOutwardSearch.aspx", "pageid=" & strPageId) & """><span>Return Outward Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
    End Sub

    Function DisplayLocPopup(ByVal strJQ As String, ByVal intLine As Integer) As Boolean
        Dim ds As New DataSet
        Dim i As Integer
        Dim aryTemp As New ArrayList()
        Dim strLot, strLoc, strSubLoc, strQty As String

        strJQ = Replace(strJQ, "jq", "")
        ds = objINV.getROLot(strRONo, intLine)
        aryTemp.Clear()

        If ds.Tables(0).Rows.Count > 0 Then
            For i = 0 To ds.Tables(0).Rows.Count - 1
                strLot = Common.parseNull(ds.Tables(0).Rows(i).Item("DOL_LOT_NO"))
                strLoc = Common.parseNull(ds.Tables(0).Rows(i).Item("LM_LOCATION"))
                strSubLoc = Common.parseNull(ds.Tables(0).Rows(i).Item("LM_SUB_LOCATION"))
                strQty = Common.parseNull(ds.Tables(0).Rows(i).Item("IROL_LOT_QTY"))

                aryTemp.Add(New String() {strLot, strLoc, strSubLoc, strQty})
            Next

            ContructRow(strJQ, aryTemp)
        Else
            Return False
        End If

        DisplayLocPopup = True

    End Function

    Private Function ContructRow(ByVal strTemp As String, ByVal aryVolume As ArrayList) As String
        Dim strrow, strtable As String
        Dim i As Integer
        strrow = ""

        For i = 0 To aryVolume.Count - 1
            strrow &= "Lot: " & aryVolume(i)(0) & ", Qty: " & aryVolume(i)(3) & "," & IIf(LocDesc = "", "Location", LocDesc) & ": " & aryVolume(i)(1) & ", " & IIf(SubLocDesc = "", "Sub-Location", SubLocDesc) & ": " & aryVolume(i)(2) & "<BR>"
        Next

        strtable = strrow

        Session("jqPopup") = Session("jqPopup") & "$('.jq" & strTemp & "').CreateBubblePopup({innerHtml: '" & strtable & "',position:'left', align: 'middle', innerHtmlStyle: { 'text-align':'left' },themeName:'all-black',themePath:'../../Common/Plugins/images/jquerybubblepopup-theme'});"

    End Function
End Class
