Imports eProcure.Component
Imports AgoraLegacy

Partial Public Class GLCodeSearch
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strGLCode As String = ""
    Dim strGLDesc As String = ""
    Dim strGLStatus As String = "A"
    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objGLCode As New IPP

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgGLCode)
        If Me.Request.QueryString("id") <> "" Or Me.Request.QueryString("id") <> Nothing Then
            If InStr(Me.Request.QueryString("id").ToString, ":") Then
                strGLCode = Me.Request.QueryString("id").ToString.Substring(0, InStr(Me.Request.QueryString("id").ToString, ":") - 1)
            Else
                strGLCode = Common.Parse(Me.Request.QueryString("id")) 'get from Raise IPP Screen
            End If
        End If
   

        hidopenerID.Value = Me.Request.QueryString("txtid") 'get from Raise IPP Screen
        hidopenerHIDID.Value = Me.Request.QueryString("hidid")
        hidopenerbtn.Value = Me.Request.QueryString("hidbtnid")
        hidopenerValID.Value = Me.Request.QueryString("hidvalid")

        If Not IsPostBack Then
            cmdClose.Attributes.Add("onclick", "selectOne();")
            Me.txtGLCode.Text = strGLCode
            objGLCode.GetGLCode(strGLCode, strGLDesc, strGLStatus)
            Bindgrid()
            dtgGLCode.CurrentPageIndex = 0
            cmdClose.Attributes.Add("onclick", "selectOne();")
        End If
        objGLCode = Nothing
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objGLCode As New IPP
        Dim ds As New DataSet

        strGLCode = Me.txtGLCode.Text
        strGLDesc = Me.txtGLCodeDesc.Text

        ds = objGLCode.GetGLCode(strGLCode, strGLDesc, strGLStatus)


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
            GLCode.Style("display") = "inline"
            resetDatagridPageIndex(dtgGLCode, dvViewSample)
            dtgGLCode.DataSource = dvViewSample
            dtgGLCode.DataBind()
        Else
            dtgGLCode.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            If Not Page.IsPostBack Then
                NoRecord.Style("display") = "inline"
            End If
            dtgGLCode.DataBind()

        End If
        ' add for above checking
        ViewState("PageCount") = dtgGLCode.PageCount

        objGLCode = Nothing
        ds = Nothing
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgGLCode.SortCommand
        Grid_SortCommand(sender, e)
        dtgGLCode.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgGLCode_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGLCode.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgGLCode, e)
        intPageRecordCnt = ViewState("RecordCount")
    End Sub

    Private Sub dtgGLCode_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGLCode.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim rb As RadioButton = e.Item.FindControl("rbtnSelection")
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)            
            If strGLCode <> "" And strGLCode = dv("CBG_B_GL_CODE") Then
                rb.Checked = True
                hidGLCode.Value = strGLCode & ":" & dv("CBG_B_GL_DESC").ToString.Replace("'", "")
            End If

            rb.Attributes.Add("OnClick", "SelectOneOnly(" & rb.ClientID & ", " & "'dtgGLCode'" & ",'" & dv("CBG_B_GL_CODE").ToString.Replace("'", "\'") & "','" & dv("CBG_B_GL_DESC").ToString.Replace("'", "\'") & "')")
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgGLCode.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "CBG_B_GL_CODE"
        strGLCode = Me.txtGLCode.Text
        strGLDesc = Me.txtGLCodeDesc.Text
        Bindgrid()
    End Sub

    

    Private Sub dtgGLCode_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgGLCode.PageIndexChanged
        Dim objGLCode As New IPP

        dtgGLCode.CurrentPageIndex = e.NewPageIndex
        objGLCode.GetGLCode(strGLCode, strGLDesc, strGLStatus)
        Bindgrid()
        Session("action") = ""
        objGLCode = Nothing
    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Response.Write("<script language=""javascript"">window.close();</script>")
    End Sub
End Class