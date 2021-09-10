Imports AgoraLegacy
Imports eProcure.Component
Public Class GRNList
    Inherits AgoraLegacy.AppBaseClass
    Dim objDO As New DeliveryOrder
    Dim dDispatcher = New dispatcher

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

    Protected WithEvents dtgGRNList As System.Web.UI.WebControls.DataGrid

    ' Dim strMode, strDONo, strLocID, strBCoyID, strPONo As String
    Public Enum EnumPO
        icDONoLink
        icDODate
        icDOIndex
        icPONo
        icPODate
        icVendorName

    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load ', Me.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnPaging = True
        If Not Page.IsPostBack Then
            GenerateTab()
            SetGridProperty(dtgGRNList)
            dtgGRNList.CurrentPageIndex = 0
            ViewState("SortAscending") = "yes"
            ViewState("SortExpression") = "DOM_DO_DATE"
            BindGRN(True)

        End If

    End Sub

    Private Sub BindGRN(Optional ByVal pSorted As Boolean = False)

        Dim txt_PONo As String = txtPONumber.Text
        Dim txt_DONo As String = txtDONumber.Text
        'Dim objDO As New Dashboard
        Dim objDO As New DeliveryOrder
        Dim ds As DataSet = New DataSet
        'ds = objDO.GetInDO()
        ds = objDO.GetDO2(txt_DONo, txt_PONo)

        Dim dvViewInDO As DataView
        dvViewInDO = ds.Tables(0).DefaultView
        dvViewInDO.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewInDO.Sort += " DESC"

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgGRNList, dvViewInDO)
            dtgGRNList.DataSource = dvViewInDO
            dtgGRNList.DataBind()
        Else
            dtgGRNList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ViewState("intPageRecordCnt") = dtgGRNList.PageCount
    End Sub
    Private Sub dtgGRNList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGRNList.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgGRNList, e)
    End Sub
    Private Sub dtgGRNList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGRNList.ItemDataBound
        Dim objDB As New EAD.DBCom
        Dim ds As New DataSet
        Dim strsql, strDONo As String
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkDONum As HyperLink
            lnkDONum = e.Item.Cells(EnumPO.icDONoLink).FindControl("lnkDONum")
            'Get DO No.
            'strsql = "Select DOM_DO_NO, DOM_DO_Index, DOM_D_ADDR_CODE from DO_MSTR where DOM_DO_STATUS = '1' and DOM_PO_INDEX = " & dv("POM_PO_INDEX")
            'ds = objDB.FillDs(strsql)
            'lnkDONum.NavigateUrl = "AddGRN1.aspx?Frm=GRNList&Mode=New&PONo=" & dv("POM_PO_No") & "&DONo=" & dv("DOM_DO_NO") & "&DOIndex=" & dv("DOM_DO_Index") & "&pageid=" & strPageId & "&Level=created"
            lnkDONum.NavigateUrl = dDispatcher.direct("GRN", "AddGRN1.aspx", "Frm=GRNList&Mode=New&PONo=" & dv("POM_PO_No") & "&DONo=" & dv("DOM_DO_NO") & "&DOIndex=" & dv("DOM_DO_Index") & "&pageid=" & strPageId & "&Level=created" & "&vendor=" & dv("CM_COY_NAME"))
            'lnkDONum.NavigateUrl = "AddDO.aspx?Frm=GRNList.aspx&DONo=" & ds.Tables(0).Rows(0).Item(0) & "&DOIdx=" & ds.Tables(0).Rows(0).Item(1) & "&POIdx=" & dv("POM_PO_Index") & "&mode=Edit&pageid=" & strPageId & "&LocID=" & ds.Tables(0).Rows(0).Item(2) & "&PONo=" & dv("POM_PO_NO") & "&BCoy=" & dv("POM_B_COY_ID")
            lnkDONum.Text = dv("DOM_DO_NO")
            e.Item.Cells(EnumPO.icDODate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("DOM_DO_DATE"))
            e.Item.Cells(EnumPO.icPODate).Text = Common.FormatWheelDate(WheelDateFormat.LongDate, dv("POM_PO_DATE"))

            'Michelle (23/1/2013) - Issue 1727
            If objDO.withAttach(dv("DOM_DO_NO"), dv("DOM_S_COY_ID")) Then
                Dim imgAttach As New System.Web.UI.WebControls.Image
                imgAttach.BorderWidth = System.Web.UI.WebControls.Unit.Point(0)
                imgAttach.ImageUrl = dDispatcher.direct("Plugins/images", "clip_icon.gif")
                lnkDONum.Controls.Add(imgAttach)
                e.Item.Cells(EnumPO.icDONoLink).Controls.Add(imgAttach)
            End If
            'e.Item.Cells(EnumPO.icPONo).Text = dv("POM_PO_NO")

            'e.Item.Cells(EnumPO.icVendorName).Text = dv("CM_COY_NAME")
        End If
    End Sub

    Sub SortCommandGRN_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgGRNList.SortCommand
        Grid_SortCommand(sender, e)
        dtgGRNList.CurrentPageIndex = 0
        BindGRN(True)
    End Sub

    Private Sub dtgGRNList_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgGRNList.PageIndexChanged
        dtgGRNList.CurrentPageIndex = e.NewPageIndex
        BindGRN(True)
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
        '"<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn_selected"" href=""#""><span>Issue GRN</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "<li><a class=""t_entity_btn"" href=""GRNSearch.aspx?pageid=" & strPageId & """><span>GRN Listing</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                    "</ul><div></div></div>"
        'Session("w_SearchGRN_tabs") = "<div class=""t_entity"">" & _
        '            "<a class=""t_entity_btn_selected"" href=""#""><span>Issue GRN</span></a>" & _
        '            "<a class=""t_entity_btn"" href=""GRNSearch.aspx?pageid=" & strPageId & """><span>GRN Listing</span></a>" & _
        '            "</div>"
        Session("w_SearchGRN_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn_selected"" href=""#""><span>Issue GRN</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("GRN", "GRNSearch.aspx", "pageid=" & strPageId) & """><span>GRN Listing</span></a></li>" & _
                    "<li><div class=""space""></div></li>" & _
                    "</ul><div></div></div>"
    End Sub

    Private Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        Me.txtDONumber.Text = ""
        Me.txtPONumber.Text = ""
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "DOM_DO_DATE"

        SetGridProperty(dtgGRNList)
        BindGRN()
    End Sub
End Class