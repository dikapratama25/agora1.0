Imports eProcure.Component
Imports AgoraLegacy

Partial Public Class AssetGroupSearch
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals
    Dim strAssetCode As String = ""
    Dim strAssetDesc As String = ""
    Dim strAssetType As String = ""
    Dim strAssetStatus As String = "A"

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objAssetGrp As New IPP

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgAssetGroup)
        strAssetCode = Me.Request.QueryString("AssetGroup") 'get from Raise IPP Screen
        hidopenerID.Value = Me.Request.QueryString("txtid") 'get from Raise IPP Screen
        hidopenerHIDID.Value = Me.Request.QueryString("hidid")
        hidopenerValID.Value = Me.Request.QueryString("hidvalid")
        hidopenerbtn.Value = Me.Request.QueryString("hidbtnid")

        If Me.Request.QueryString("type") = "S" Then
            strAssetType = "S"
        Else
            strAssetType = "A"
        End If

        If Not IsPostBack Then
            cmdClose.Attributes.Add("onclick", "selectOne();")
            Me.txtAssetGroupCode.Text = strAssetCode
            Bindgrid()
            dtgAssetGroup.CurrentPageIndex = 0
            cmdClose.Attributes.Add("onclick", "selectOne();")
        End If
        objAssetGrp = Nothing
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objAssetGrp As New IPP
        Dim ds As New DataSet

        strAssetCode = Me.txtAssetGroupCode.Text
        strAssetDesc = Me.txtAssetGroupDesc.Text

        ds = objAssetGrp.GetAssetGroupInfo(strAssetCode, strAssetDesc, strAssetType, strAssetStatus)


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
            AssetGroup.Style("display") = "inline"
            resetDatagridPageIndex(dtgAssetGroup, dvViewSample)
            dtgAssetGroup.DataSource = dvViewSample
            dtgAssetGroup.DataBind()
        Else
            dtgAssetGroup.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            If Not Page.IsPostBack Then
                NoRecord.Style("display") = "inline"
            End If
            dtgAssetGroup.DataBind()

        End If
        ' add for above checking
        ViewState("PageCount") = dtgAssetGroup.PageCount

        objAssetGrp = Nothing
        ds = Nothing
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgAssetGroup.SortCommand
        Grid_SortCommand(sender, e)
        dtgAssetGroup.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgAssetGroup_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAssetGroup.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgAssetGroup, e)
        intPageRecordCnt = ViewState("RecordCount")
    End Sub

    Private Sub dtgAssetGroup_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAssetGroup.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim rb As RadioButton = e.Item.FindControl("rbtnSelection")
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            If strAssetCode <> "" And strAssetCode = dv("AG_GROUP") Then
                rb.Checked = True
                hidAssetGrp.Value = strAssetCode
            End If
            rb.Attributes.Add("OnClick", "SelectOneOnly(" & rb.ClientID & ", " & "'dtgAssetGroup'" & ",'" & dv("AG_GROUP") & "')")
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgAssetGroup.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "AG_GROUP"
        strAssetCode = Me.txtAssetGroupCode.Text
        strAssetDesc = Me.txtAssetGroupDesc.Text
        Bindgrid()
    End Sub

    
    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language=""javascript"">window.close();</script>")
    End Sub

    Private Sub dtgAssetGroup_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgAssetGroup.PageIndexChanged
        Dim objAssetGrp As New IPP

        dtgAssetGroup.CurrentPageIndex = e.NewPageIndex
        objAssetGrp.GetAssetGroupInfo(strAssetCode, strAssetDesc, strAssetType, strAssetStatus)
        Bindgrid()
        Session("action") = ""
        objAssetGrp = Nothing
    End Sub

End Class