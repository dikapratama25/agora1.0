Imports eProcure.Component
Imports AgoraLegacy

Partial Public Class CostCentreSearch
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals
    Dim strCostCentre As String = ""
    Dim strCCDesc As String = ""
    Dim strCCStatus As String = "A"

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objCostCentre As New IPP

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCostCentre)
        strCostCentre = Me.Request.QueryString("id") 'get from Raise IPP Screen
        hidopenerID.Value = Me.Request.QueryString("txtid") 'get from Raise IPP Screen
        hidopenerHIDID.Value = Me.Request.QueryString("hidid")
        hidopenerbtn.Value = Me.Request.QueryString("hidbtnid")
        hidopenerValID.Value = Me.Request.QueryString("hidvalid")
        'Session("ID") = Me.Request.QueryString("id")

        If Not IsPostBack Then
            'If Session("ID") <> "" Then
            '    objCostCentre.GetCostCentreInfo(Session("ID"), "", strCCStatus)
            'Else
            '    Me.txtCostCentreCode.Text = strCostCentre
            '    objCostCentre.GetCostCentreInfo(strCostCentre, strCCDesc, strCCStatus)
            'End If

            'Bindgrid()
            'dtgCostCentre.CurrentPageIndex = 0
            cmdClose.Attributes.Add("onclick", "selectOne();")
            Me.txtCostCentreCode.Text = strCostCentre
            Bindgrid()
            dtgCostCentre.CurrentPageIndex = 0
            cmdClose.Attributes.Add("onclick", "selectOne();")
        End If
        'cmdClose.Attributes.Add("onclick", "selectOne();")
        objCostCentre = Nothing
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objCostCentre As New IPP
        Dim ds As New DataSet

        strCostCentre = Me.txtCostCentreCode.Text
        strCCDesc = Me.txtCostCentreDesc.Text

        If Session("ID") <> "" Then
            ds = objCostCentre.GetCostCentreInfo(Session("ID"), "", strCCStatus)
        Else

            ds = objCostCentre.GetCostCentreInfo(strCostCentre, strCCDesc, strCCStatus)
        End If
        ' ds = objCostCentre.GetCostCentreInfo(strCostCentre, strCCDesc, strCCStatus)


        Dim dgItem As DataGridItem
        Dim i As Integer = 1

        intPageRecordCnt = ds.Tables(0).Rows.Count

        ViewState("RecordCount") = intPageRecordCnt
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            NoRecord.Style("display") = "none"
            CostCentre.Style("display") = "inline"
            resetDatagridPageIndex(dtgCostCentre, dvViewSample)
            dtgCostCentre.DataSource = dvViewSample
            dtgCostCentre.DataBind()
        Else
            dtgCostCentre.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            If Not Page.IsPostBack Then
                NoRecord.Style("display") = "inline"
            End If
            dtgCostCentre.DataBind()

        End If
        ' add for above checking
        ViewState("PageCount") = dtgCostCentre.PageCount

        objCostCentre = Nothing
        ds = Nothing
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgCostCentre.SortCommand
        Grid_SortCommand(sender, e)
        dtgCostCentre.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgCostCentre_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCostCentre.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgCostCentre, e)
        intPageRecordCnt = ViewState("RecordCount")
    End Sub

    Private Sub dtgCostCentre_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCostCentre.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim rb As RadioButton = e.Item.FindControl("rbtnSelection")
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            If strCostCentre <> "" And strCostCentre = dv("CCCode") Then
                rb.Checked = True
                hidCC.Value = strCostCentre
            End If
            rb.Attributes.Add("OnClick", "SelectOneOnly(" & rb.ClientID & ", " & "'dtgCostCentre'" & ",'" & dv("CCCode") & "')")

        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgCostCentre.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "CCCode"
        strCostCentre = Me.txtCostCentreCode.Text
        strCCDesc = Me.txtCostCentreDesc.Text
        Bindgrid()
    End Sub

    Private Sub dtgCostCentre_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCostCentre.PageIndexChanged
        Dim objCostCentre As New IPP

        dtgCostCentre.CurrentPageIndex = e.NewPageIndex
        objCostCentre.GetCostCentreInfo(strCostCentre, strCCDesc, strCCStatus)
        Bindgrid()
        Session("action") = ""
        objCostCentre = Nothing
    End Sub

End Class