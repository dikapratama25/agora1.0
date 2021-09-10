Imports AgoraLegacy
Imports eProcure.Component


Public Class POListS
    Inherits AgoraLegacy.AppBaseClass
    Dim strItemCode As String
    Dim strType As String

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

    Protected WithEvents dtgPO As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblTotalV As System.Web.UI.WebControls.Label

    Public Enum EnumPO
        icPONoLink
        icCreationDate
        icVendorName
        icStatus
        icQty
    End Enum
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnPaging = True
        strItemCode = Me.Request(Trim("itemcode"))
        strType = Me.Request(Trim("type"))
        If Not Page.IsPostBack Then
            SetGridProperty(dtgPO)
            ViewState("SortAscending") = "no"
            ViewState("SortExpression") = "POM_PO_Date"
            dtgPO.CurrentPageIndex = 0
            BindPO(True)
        End If
    End Sub

    Private Sub BindPO(Optional ByVal pSorted As Boolean = False)

        Dim objPO_Ext As New PurchaseOrder_Ext
        Dim ds As DataSet
        Dim dvViewPO As DataView

        If strType = "1" Then
            ds = objPO_Ext.GetPOListBalance(strItemCode)
        Else
            ds = objPO_Ext.GetPOListProgress(strItemCode)
        End If

        dvViewPO = ds.Tables(0).DefaultView
        dvViewPO.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewPO.Sort += " DESC"

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgPO, dvViewPO)
            dtgPO.DataSource = dvViewPO
            dtgPO.DataBind()
        Else
            dtgPO.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ViewState("PageCount") = dtgPO.PageCount
    End Sub

    Private Sub dtgPO_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPO.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgPO, e)
    End Sub

    Private Sub dtgPO_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPO.ItemDataBound
        Dim objDB As New EAD.DBCom
        Dim ds As New DataSet
        Dim strsql, strDONo As String
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkPONo As HyperLink
            lnkPONo = e.Item.Cells(EnumPO.icPONoLink).FindControl("lnkPONo")
            'Get DO No.
            'strsql = "Select DOM_DO_NO, DOM_DO_Index, DOM_D_ADDR_CODE from DO_MSTR where DOM_DO_STATUS = '1' and DOM_PO_INDEX = " & dv("POM_PO_INDEX") & " and DOM_D_ADDR_CODE = '" & dv("POD_D_ADDR_CODE") & "' "
            'ds = objDB.FillDs(strsql)
            'If ds.Tables(0).Rows.Count = 0 Then 'With Draft DO
            '    lnkPONo.NavigateUrl = dDispatcher.direct("DO", "AddDO.aspx", "Frm=POList&Mode=New&PONo=" & dv("POM_PO_No") & "&BCoy=" & dv("POM_B_COY_ID") & "&POIdx=" & dv("POM_PO_Index") & "&pageid=" & strPageId & "&DA=" & dv("POD_D_ADDR_CODE"))
            'Else
            '    lnkPONo.NavigateUrl = dDispatcher.direct("DO", "AddDO.aspx", "Frm=POList&Mode=Edit&DONo=" & ds.Tables(0).Rows(0).Item(0) & "&POIdx=" & dv("POM_PO_Index") & "&pageid=" & strPageId & "&LocID=" & ds.Tables(0).Rows(0).Item(2) & "&PONo=" & dv("POM_PO_NO") & "&BCoy=" & dv("POM_B_COY_ID"))

            'End If
            lnkPONo.Text = dv("POM_PO_No")
            e.Item.Cells(EnumPO.icCreationDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("POM_PO_Date"))
            e.Item.Cells(EnumPO.icQty).Text = CDec(dv("POD_ORDERED_QTY")) - (CDec(dv("POD_RECEIVED_QTY")) - CDec(dv("POD_REJECTED_QTY")))

            Select Case dv("POM_PO_STATUS")
                Case "0"
                    e.Item.Cells(EnumPO.icStatus).Text = "Draft"
                Case "7", "8"
                    e.Item.Cells(EnumPO.icStatus).Text = "Submitted for approval"
                Case "11"
                    e.Item.Cells(EnumPO.icStatus).Text = "Held"
                Case "1", "2", "9"
                    e.Item.Cells(EnumPO.icStatus).Text = "Approved by management"
                Case "3"
                    e.Item.Cells(EnumPO.icStatus).Text = "Accepted by vendor"
                Case "6"
                    e.Item.Cells(EnumPO.icStatus).Text = "Completed delivery and paid"
                Case "Cancelled", "Cancelled By"
                    e.Item.Cells(EnumPO.icStatus).Text = "Cancelled by buyer"
                Case "Rejected", "Rejected By"
                    e.Item.Cells(EnumPO.icStatus).Text = "Rejected by management / vendor"
                Case "Void"
                    e.Item.Cells(EnumPO.icStatus).Text = "Void draft PO"
            End Select

            lblTotalV.Text = CDec(lblTotalV.Text) + CDec(dv("POD_ORDERED_QTY")) - (CDec(dv("POD_RECEIVED_QTY")) - CDec(dv("POD_REJECTED_QTY")))

        End If
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgPO.SortCommand
        Grid_SortCommand(sender, e)
        dtgPO.CurrentPageIndex = 0
        BindPO(True)
    End Sub

    Private Sub dtgPO_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgPO.PageIndexChanged
        lblTotalV.Text = "0.00"
        dtgPO.CurrentPageIndex = e.NewPageIndex
        BindPO(True)
    End Sub

End Class
