Imports eProcure.Component
Imports AgoraLegacy

Partial Public Class BranchCodeSearch
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals
    Dim strBranchCode As String = ""
    Dim strBranchDesc As String = ""
    Dim strCompID As String = ""
    'Dim strBranchStatus As String = "A"

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objBranch As New IPP

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgBranch)
        strBranchCode = Me.Request.QueryString("id") 'get from Raise IPP Screen
        hidopenerID.Value = Me.Request.QueryString("txtid") 'get from Raise IPP Screen
        hidopenerHIDID.Value = Me.Request.QueryString("hidid")
        hidopenerValID.Value = Me.Request.QueryString("hidvalid")
        hidopenerbtn.Value = Me.Request.QueryString("hidbtnid")
        strCompID = Me.Request.QueryString("compid")

        If Not IsPostBack Then
            cmdClose.Attributes.Add("onclick", "selectOne();")
            Me.txtBranchCode.Text = strBranchCode          
            Bindgrid()
            dtgBranch.CurrentPageIndex = 0
            cmdClose.Attributes.Add("onclick", "selectOne();")
        End If
        objBranch = Nothing
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objBranch As New IPP
        Dim ds As New DataSet

        strBranchCode = Me.txtBranchCode.Text
        strBranchDesc = Me.txtBranchDesc.Text

        ds = objBranch.GetBranchInfo(strBranchCode, strBranchDesc, Request.QueryString("compid"))


        intPageRecordCnt = ds.Tables(0).Rows.Count

        ViewState("RecordCount") = intPageRecordCnt
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            NoRecord.Style("display") = "none"
            BranchCode.Style("display") = "inline"
            resetDatagridPageIndex(dtgBranch, dvViewSample)
            dtgBranch.DataSource = dvViewSample
            dtgBranch.DataBind()
        Else
            dtgBranch.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            If Not Page.IsPostBack Then
                NoRecord.Style("display") = "inline"
            End If
            dtgBranch.DataBind()

        End If
        ' add for above checking
        ViewState("PageCount") = dtgBranch.PageCount

        objBranch = Nothing
        ds = Nothing
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgBranch.SortCommand
        Grid_SortCommand(sender, e)
        dtgBranch.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgBranch_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgBranch.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgBranch, e)
        intPageRecordCnt = ViewState("RecordCount")
    End Sub

    Private Sub dtgBranch_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgBranch.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim rb As RadioButton = e.Item.FindControl("rbtnSelection")
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            If strBranchCode <> "" And strBranchCode = dv("CDM_DEPT_CODE") Then
                rb.Checked = True
                hidBranch.Value = strBranchCode
            End If
            rb.Attributes.Add("OnClick", "SelectOneOnly(" & rb.ClientID & ", " & "'dtgBranch'" & ",'" & dv("CDM_DEPT_CODE") & "')")

        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgBranch.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "CDM_DEPT_CODE"
        strBranchCode = Me.txtBranchCode.Text
        strBranchDesc = Me.txtBranchDesc.Text
        Bindgrid()
    End Sub

    Private Sub dtgBranch_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgBranch.PageIndexChanged
        Dim objBranch As New IPP

        dtgBranch.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
        Session("action") = ""
        objBranch = Nothing
    End Sub

End Class