Imports eProcure.Component
Imports AgoraLegacy

Partial Public Class CostAllocCodeSearch
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals
    Dim strCostAllocCode As String = ""
    Dim strCostAllocDesc As String = ""
    Dim strCostAllocStatus As String = "A"
    Dim hidAmount As String

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objCostAlloc As New IPP

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCostAllocCode)
        strCostAllocCode = Me.Request.QueryString("id") 'get from Raise IPP Screen
        hidAmount = Request.QueryString("Amount") 'get from Raise IPP screen
        hidopenerID.Value = Me.Request.QueryString("txtid") 'get from Raise IPP Screen
        hidopenerHIDID.Value = Me.Request.QueryString("hidid")
        hidopenerValID.Value = Me.Request.QueryString("hidvalid")
        hidopenerbtn.Value = Me.Request.QueryString("hidbtnid")

        If Not IsPostBack Then
            cmdClose.Attributes.Add("onclick", "selectOne();")
            Me.txtCostAllocCode.Text = strCostAllocCode
            objCostAlloc.GetCostAlloc(strCostAllocCode, strCostAllocDesc)
            Bindgrid()
            dtgCostAllocCode.CurrentPageIndex = 0
            cmdClose.Attributes.Add("onclick", "selectOne();")
        End If
        objCostAlloc = Nothing
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objCostAlloc As New IPP
        Dim ds As New DataSet
       
        strCostAllocCode = Me.txtCostAllocCode.Text
        strCostAllocDesc = Me.txtCostAllocCodeDesc.Text

        ds = objCostAlloc.GetCostAlloc(strCostAllocCode, strCostAllocDesc)


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
            CostAllocCode.Style("display") = "inline"
            resetDatagridPageIndex(dtgCostAllocCode, dvViewSample)
            dtgCostAllocCode.DataSource = dvViewSample
            dtgCostAllocCode.DataBind()
        Else
            dtgCostAllocCode.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            If Not Page.IsPostBack Then
                NoRecord.Style("display") = "inline"
            End If
            dtgCostAllocCode.DataBind()

        End If
        ' add for above checking
        ViewState("PageCount") = dtgCostAllocCode.PageCount

        objCostAlloc = Nothing
        ds = Nothing
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgCostAllocCode.SortCommand
        Grid_SortCommand(sender, e)
        dtgCostAllocCode.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgCostAllocCode_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCostAllocCode.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgCostAllocCode, e)
        intPageRecordCnt = ViewState("RecordCount")
    End Sub

    Private Sub dtgCostAllocCode_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCostAllocCode.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim dvMatch As DataView = dv.DataView
            Dim rb As RadioButton = e.Item.FindControl("rbtnSelection")
            If strCostAllocCode <> "" And strCostAllocCode = dv("CAM_CA_CODE") Then
                rb.Checked = True
                hidCostAlloc.Value = strCostAllocCode
            End If
            rb.Attributes.Add("OnClick", "SelectOneOnly(" & rb.ClientID & ", " & "'dtgCostAllocCode'" & ",'" & dv("CAM_CA_CODE") & "')")          

            e.Item.Cells(1).Text = "<A href=""#"" onclick=""PopWindow('" & dDispatcher.direct("IPP", "CostAllocDetail.aspx", "CostAllocCode=" & dv("CAM_CA_CODE")) & "')"" ><font color=#0000ff>" & Common.parseNull(dv("CAM_CA_CODE")) & "</font></A>"
        End If
       
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgCostAllocCode.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "CAM_INDEX"
        strCostAllocCode = Me.txtCostAllocCode.Text
        strCostAllocDesc = Me.txtCostAllocCodeDesc.Text
        Bindgrid()
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language=""javascript"">window.close();</script>")
    End Sub

    Private Sub dtgCostAllocCode_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCostAllocCode.PageIndexChanged
        Dim objCostAlloc As New IPP

        dtgCostAllocCode.CurrentPageIndex = e.NewPageIndex
        objCostAlloc.GetCostAlloc(strCostAllocCode, strCostAllocDesc)
        Bindgrid()
        Session("action") = ""
        objCostAlloc = Nothing
    End Sub

End Class