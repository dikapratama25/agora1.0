Imports eProcure.Component
Imports AgoraLegacy

Partial Public Class ViewIPPAuditTrails
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals
    Dim strDocNo As String = ""
    Dim strStatus As String = ""
    'Zulham 05112018
    Dim objDB As New EAD.DBCom

    'Dim strCCStatus As String = "A"

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objCostCentre As New IPP

        MyBase.Page_Load(sender, e)
        strNewCSS = "true"
        SetGridProperty(dtgIPPAudit)
        strDocNo = Me.Request.QueryString("docno") 'get from Raise IPP Screen
        strStatus = Me.Request.QueryString("docstatus") 'get from Raise IPP Screen
        ViewState("InvIdx") = Me.Request.QueryString("docidx")
        ViewState("Module") = Me.Request.QueryString("module")

        If Not IsPostBack Then
            Me.lblDocNo.Text = strDocNo
            Me.lblStatus.Text = strStatus.Replace("IPP", "E2P") 'Zulham 13072018 - PAMB
            Bindgrid()
            dtgIPPAudit.CurrentPageIndex = 0
        End If
        objCostCentre = Nothing
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objIPPAudit As New IPP
        Dim objBill As New Billing
        Dim ds As New DataSet

        strDocNo = Me.lblDocNo.Text
        strStatus = Me.lblStatus.Text

        If ViewState("Module") = "Billing" Then
            ds = objBill.GetBillAudit(ViewState("InvIdx"))
        Else
            ds = objIPPAudit.GetIPPAudit(ViewState("InvIdx"))
        End If

        Dim dgItem As DataGridItem
        Dim i As Integer = 1

        intPageRecordCnt = ds.Tables(0).Rows.Count

        ViewState("RecordCount") = intPageRecordCnt
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            ' NoRecord.Style("display") = "none"
            CostCentre.Style("display") = "inline"
            resetDatagridPageIndex(dtgIPPAudit, dvViewSample)
            dtgIPPAudit.DataSource = dvViewSample
            dtgIPPAudit.DataBind()
        Else
            dtgIPPAudit.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            dtgIPPAudit.DataBind()

        End If
        ' add for above checking
        ViewState("PageCount") = dtgIPPAudit.PageCount
        cmdClose.Attributes.Add("onclick", "closed();")
        objIPPAudit = Nothing
        ds = Nothing
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgIPPAudit.SortCommand
        Grid_SortCommand(sender, e)
        dtgIPPAudit.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgIPPAudit_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPPAudit.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgIPPAudit, e)
        intPageRecordCnt = ViewState("RecordCount")
    End Sub

    Private Sub dtgIPPAudit_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgIPPAudit.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim rb As RadioButton = e.Item.FindControl("rbtnSelection")
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            'Zulham 05112018
            Dim userName = objDB.GetVal("select um_user_name from user_mstr where um_user_id = '" & dv("itl_user_id") & "'")
            e.Item.Cells(0).Text = userName
            'e.Item.Cells(0).Text = e.Item.Cells(0).Text.Replace("IPP", "E2P") 'Zulham 12072018 - PAMB

        End If
    End Sub

    Private Sub dtgIPPAudit_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgIPPAudit.PageIndexChanged
        dtgIPPAudit.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
        Session("action") = ""
    End Sub

End Class