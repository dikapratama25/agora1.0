Imports AgoraLegacy
Imports eProcure.Component
Public Class POList
    Inherits AgoraLegacy.AppBaseClass


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
    ' Dim strMode, strDONo, strLocID, strBCoyID, strPONo As String
    Public Enum EnumPO
        icPONoLink
        icCreationDate
        icDueDate
        icCoName
        icDA
        icTot
        icQty
    End Enum
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnPaging = True
        If Not Page.IsPostBack Then
            GenerateTab()
            SetGridProperty(dtgPO)
            ViewState("SortAscending") = "no"
            ViewState("SortExpression") = "POM_PO_Date"
            dtgPO.CurrentPageIndex = 0
            BindPO(True)

        End If

    End Sub

    Private Sub BindPO(Optional ByVal pSorted As Boolean = False)

        Dim objDO As New DeliveryOrder
        Dim ds As DataSet
        Dim dvViewPO As DataView

        ds = objDO.GetOutStandingPOWithDAddress()
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
            strsql = "Select DOM_DO_NO, DOM_DO_Index, DOM_D_ADDR_CODE from DO_MSTR where DOM_DO_STATUS = '1' and DOM_PO_INDEX = " & dv("POM_PO_INDEX") & " and DOM_D_ADDR_CODE = '" & dv("POD_D_ADDR_CODE") & "' "
            ds = objDB.FillDs(strsql)
            If ds.Tables(0).Rows.Count = 0 Then 'With Draft DO
                'lnkPONo.NavigateUrl = "AddDO.aspx?Frm=POList&Mode=New&PONo=" & dv("POM_PO_No") & "&BCoy=" & dv("POM_B_COY_ID") & "&POIdx=" & dv("POM_PO_Index") & "&pageid=" & strPageId
                lnkPONo.NavigateUrl = dDispatcher.direct("DO", "AddDO.aspx", "Frm=POList&Mode=New&PONo=" & dv("POM_PO_No") & "&BCoy=" & dv("POM_B_COY_ID") & "&POIdx=" & dv("POM_PO_Index") & "&pageid=" & strPageId & "&DA=" & dv("POD_D_ADDR_CODE"))
            Else
                'lnkPONo.NavigateUrl = "AddDO.aspx?Frm=POList&Mode=Edit&DONo=" & ds.Tables(0).Rows(0).Item(0) & "&DOIdx=" & ds.Tables(0).Rows(0).Item(1) & "&POIdx=" & dv("POM_PO_Index") & "&pageid=" & strPageId & "&LocID=" & ds.Tables(0).Rows(0).Item(2) & "&PONo=" & dv("POM_PO_NO") & "&BCoy=" & dv("POM_B_COY_ID")
                'lnkPONo.NavigateUrl = "AddDO.aspx?Frm=POList&Mode=Edit&DONo=" & ds.Tables(0).Rows(0).Item(0) & "&POIdx=" & dv("POM_PO_Index") & "&pageid=" & strPageId & "&LocID=" & ds.Tables(0).Rows(0).Item(2) & "&PONo=" & dv("POM_PO_NO") & "&BCoy=" & dv("POM_B_COY_ID")
                lnkPONo.NavigateUrl = dDispatcher.direct("DO", "AddDO.aspx", "Frm=POList&Mode=Edit&DONo=" & ds.Tables(0).Rows(0).Item(0) & "&POIdx=" & dv("POM_PO_Index") & "&pageid=" & strPageId & "&LocID=" & ds.Tables(0).Rows(0).Item(2) & "&PONo=" & dv("POM_PO_NO") & "&BCoy=" & dv("POM_B_COY_ID"))

            End If
            lnkPONo.Text = dv("POM_PO_No")
            e.Item.Cells(EnumPO.icCreationDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("POM_PO_Date"))
            e.Item.Cells(EnumPO.icDueDate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("DUE_Date"))
            e.Item.Cells(EnumPO.icCoName).Text = dv("CM_COY_NAME")
            e.Item.Cells(EnumPO.icDA).Text = dv("POD_D_ADDR_CODE")
            e.Item.Cells(EnumPO.icTot).Text = dv("Tot")
            e.Item.Cells(EnumPO.icQty).Text = dv("Outs")
        End If
    End Sub
    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgPO.SortCommand
        Grid_SortCommand(sender, e)
        dtgPO.CurrentPageIndex = 0
        BindPO(True)
    End Sub
    Private Sub dtgPO_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgPO.PageIndexChanged
        dtgPO.CurrentPageIndex = e.NewPageIndex
        BindPO(True)
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Session("w_POList_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn_selected"" href=""POList.aspx?pageid=" & strPageId & """><span>Issue DO</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn"" href=""searchDO.aspx?pageid=" & strPageId & """><span>DO Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "</ul><div></div></div>"
        'Session("w_POList_tabs") = "<div class=""t_entity"">" & _
        '                   "<a class=""t_entity_btn_selected"" href=""POList.aspx?pageid=" & strPageId & """><span>Issue DO</span></a>" & _
        '                   "<a class=""t_entity_btn"" href=""searchDO.aspx?pageid=" & strPageId & """><span>DO Listing</span></a>" & _
        '                   "</div>"
        Session("w_POList_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                          "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("DO", "POList.aspx", "pageid=" & strPageId) & """><span>Issue DO</span></a></li>" & _
                          "<li><div class=""space""></div></li>" & _
                          "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("DO", "searchDO.aspx", "pageid=" & strPageId) & """><span>DO Listing</span></a></li>" & _
                          "<li><div class=""space""></div></li>" & _
                          "</ul><div></div></div>"
    End Sub

End Class
