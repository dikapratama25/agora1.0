'Copyright © 2013 STRATEQ GLOBAL SERVICES. All rights reserved.
'//Attachment Ind
Imports AgoraLegacy
Imports eProcure.Component
Public Class CreditNoteAckTrackingList
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
    Protected WithEvents txtInvNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateFr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAckDateTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAckDateFr As System.Web.UI.WebControls.TextBox
    Protected WithEvents dtgCnList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents vldsum As System.Web.UI.HtmlControls.HtmlGenericControl

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

        If DateDiff(DateInterval.Month, CDate(txtDateFr.Text), CDate(txtDateTo.Text)) > 6 Then
            vldsum.InnerHtml = "<li>Date Range " & objGlobal.GetErrorMessage("00015") & " 6 months</li>"
            Return False
        Else
            vldsum.InnerHtml = ""
        End If

        If DateDiff(DateInterval.Month, CDate(txtAckDateFr.Text), CDate(txtAckDateTo.Text)) > 6 Then
            vldsum.InnerHtml = "<li>Ackknowledge Date Range " & objGlobal.GetErrorMessage("00015") & " 6 months</li>"
            Return False
        Else
            vldsum.InnerHtml = ""
        End If

        ds = objCn.getCebitNoteTracking(txtCnNo.Text, "", "", "2", txtInvNo.Text, txtDateFr.Text, txtDateTo.Text, txtAckDateFr.Text, txtAckDateTo.Text)

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
        SortDateDesc2()
    End Sub

    '//Sort the record by Date Desc when user click button/Dropdownlist other than Datagrid header and paging
    Sub SortDateDesc()
        ViewState("SortExpression") = "CNM_CREATED_DATE"
        ViewState("SortAscending") = "no"
        txtDateTo.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, DateTime.Today)
        txtDateFr.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, DateTime.Today.AddMonths(-6)) '.ToString
        txtAckDateTo.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, DateTime.Today)
        txtAckDateFr.Text = Common.FormatWheelDate(WheelDateFormat.LongDate, DateTime.Today.AddMonths(-6)) '.ToString
        Bindgrid()
    End Sub

    Sub SortDateDesc2()
        ViewState("SortExpression") = "CNM_CN_DATE"
        ViewState("SortAscending") = "no"
        Bindgrid()
    End Sub

    Private Sub dtgCnList_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCnList.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim lnkCnNo As HyperLink
            lnkCnNo = e.Item.FindControl("lnkCnNo")
            lnkCnNo.Text = dv("CNM_CN_NO")
            lnkCnNo.NavigateUrl = dDispatcher.direct("CreditNote", "CreditNoteDetails.aspx", "cmd=" & ViewState("button") & "&role=" & ViewState("role") & "&Frm=" & "CreditNoteAckTrackingList" & "&pageid=" & strPageId & "&CNNO=" & Common.parseNull(dv("CNM_CN_NO")) & "&vcomid=" & Common.parseNull(dv("CNM_CN_S_COY_ID")) & "&status=" & Common.parseNull(dv("CNM_CN_STATUS")))

            e.Item.Cells(EnumCN.icCnDate).Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, dv("CNM_CREATED_DATE"))

            Dim lnkInvNo As HyperLink
            lnkInvNo = e.Item.FindControl("lnkInvNo")
            lnkInvNo.Text = dv("CNM_INV_NO")
            lnkInvNo.NavigateUrl = dDispatcher.direct("Tracking", "InvoiceDetails.aspx", "Frm=CreditNoteAckTrackingList" & "&pageid=" & strPageId & "&role=3&INVNO=" & Common.parseNull(dv("CNM_INV_NO")) & "&vcomid=" & Common.parseNull(dv("CNM_CN_S_COY_ID")) & "&folder=P&status=4")

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
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("CreditNote", "CreditNoteTrackingList.aspx", "pageid=" & strPageId) & """><span>New Credit Note</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("CreditNote", "CreditNoteAckTrackingList.aspx", "pageid=" & strPageId) & """><span>Acknowledged Credit Note</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
    End Sub
End Class


