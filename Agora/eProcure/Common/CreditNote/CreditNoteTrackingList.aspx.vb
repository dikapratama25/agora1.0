'Copyright © 2013 STRATEQ GLOBAL SERVICES. All rights reserved.
'//Attachment Ind
Imports AgoraLegacy
Imports eProcure.Component
Public Class CreditNoteTrackingList
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals

    Public Enum EnumCN
        icCnNo
        icCnDate
        icInvNo
        icVendor
        icCurr
        icAmount
        icIndex
    End Enum
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents txtCnNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVendor As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlCurr As System.Web.UI.WebControls.DropDownList
    Protected WithEvents dtgCnList As System.Web.UI.WebControls.DataGrid


    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        CheckButtonAccess(True)
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        blnCheckBox = False
        SetGridProperty(dtgCnList)

        MyBase.Page_Load(sender, e)

        If Not Page.IsPostBack Then
            Session("strURL") = strCallFrom
            'Load Currency
            objGlobal.FillCodeTable(ddlCurr, CodeTable.Currency)
            GenerateTab()
            SortDateDesc()
        End If
    End Sub

    Public Sub dtgCnList_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCnList.PageIndexChanged
        dtgCnList.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgCnList.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Function Bindgrid() As String
        Dim ds As New DataSet
        Dim objCn As New CreditNote

        ds = objCn.getCebitNoteTracking(txtCnNo.Text, txtVendor.Text, ddlCurr.SelectedValue, "1")

        ViewState("intPageRecordCnt") = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If ViewState("action") = "del" Then
            If dtgCnList.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgCnList.PageSize = 0 Then
                dtgCnList.CurrentPageIndex = dtgCnList.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgCnList, dvViewSample)
            dtgCnList.DataSource = dvViewSample
            dtgCnList.DataBind()
        Else
            dtgCnList.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ' add for above checking
        ViewState("PageCount") = dtgCnList.PageCount
    End Function

    Private Sub dtgCnList_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCnList.ItemCreated
        Grid_ItemCreated(dtgCnList, e)
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgCnList.CurrentPageIndex = 0
        SortDateDesc()
    End Sub

    '//Sort the record by Date Desc when user click button/Dropdownlist other than Datagrid header and paging
    Sub SortDateDesc()
        ViewState("SortExpression") = "CNM_CN_DATE"
        ViewState("SortAscending") = "no"
        BindGrid()
    End Sub

    Private Sub dtgCnList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCnList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkCnNo As HyperLink
            lnkCnNo = e.Item.FindControl("lnkCnNo")
            lnkCnNo.Text = dv("CNM_CN_NO")
            lnkCnNo.NavigateUrl = dDispatcher.direct("CreditNote", "CreditNoteDetails.aspx", "cmd=" & ViewState("button") & "&role=" & ViewState("role") & "&Frm=" & "CreditNoteTrackingList" & "&pageid=" & strPageId & "&CNNO=" & Common.parseNull(dv("CNM_CN_NO")) & "&vcomid=" & Common.parseNull(dv("CNM_CN_S_COY_ID")) & "&status=" & Common.parseNull(dv("CNM_CN_STATUS")))

            e.Item.Cells(EnumCN.icCnDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("CNM_CN_DATE"))

            Dim lnkInvNo As HyperLink
            lnkInvNo = e.Item.FindControl("lnkInvNo")
            lnkInvNo.Text = dv("CNM_INV_NO")
            lnkInvNo.NavigateUrl = dDispatcher.direct("Tracking", "InvoiceDetails.aspx", "Frm=CreditNoteTrackingList" & "&pageid=" & strPageId & "&role=3&INVNO=" & Common.parseNull(dv("CNM_INV_NO")) & "&vcomid=" & Common.parseNull(dv("CNM_CN_S_COY_ID")) & "&folder=P&status=4")

            If Not IsDBNull(dv("AMOUNT")) Then
                e.Item.Cells(EnumCN.icAmount).Text = Format(CDbl(dv("AMOUNT")), "###,##0.00")
            End If

        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_CnTrackingList_tabs") = "<div class=""t_entity""><ul>" & _
        "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("CreditNote", "CreditNoteTrackingList.aspx", "pageid=" & strPageId) & """><span>New Credit Note</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("CreditNote", "CreditNoteAckTrackingList.aspx", "pageid=" & strPageId) & """><span>Acknowledged Credit Note</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
    End Sub
End Class


