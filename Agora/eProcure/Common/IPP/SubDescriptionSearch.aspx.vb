Imports eProcure.Component
Imports AgoraLegacy

Partial Public Class SubDescriptionSearch
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strGLCode As String = ""
    Dim strGLDesc As String = ""
    Dim strGLStatus As String = "A"
    Dim strSubDescCode As String = ""
    Dim strSql As String = ""
    Dim str As String
    Dim rowIdx As String
    Dim invalidDesc As Boolean = False
    Dim ds As New DataSet
    Dim objGLCode As New IPP
    Dim objIPPMain As New IPPMain
    Dim objDb As New EAD.DBCom
    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objGLCode As New IPP

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgSubDesc)
        If Me.Request.QueryString("GLCode") <> "" Or Me.Request.QueryString("GLCode") <> Nothing Then
            If InStr(Me.Request.QueryString("GLCode").ToString, ":") Then
                strGLCode = Me.Request.QueryString("GLCode").ToString.Substring(0, InStr(Me.Request.QueryString("GLCode").ToString, ":") - 1)
            Else
                strGLCode = Common.Parse(Me.Request.QueryString("GLCode"))
            End If
        End If

        strSubDescCode = Request.QueryString("SubDescCode")
        If Not Session("SelectedSubDesc") Is Nothing And strSubDescCode = "" Then strSubDescCode = Session("SelectedSubDesc")
        rowIdx = Request.QueryString("i")
        hidopenerID.Value = "txtRuleCategory" & rowIdx
        hidopenerHIDID.Value = "hidRuleCategory2" & rowIdx
        hidopenerbtn.Value = "btnSubDescCode" & rowIdx
        hidopenerValID.Value = "hidRuleCategoryVal" & rowIdx
        If Not IsPostBack Then
            'cmdClose.Attributes.Add("onclick", "selectOne();")
            Me.txtGLCode.Text = strGLCode
            Bindgrid(strSubDescCode, )
            dtgSubDesc.CurrentPageIndex = 0
            cmdClose.Attributes.Add("onclick", "selectOne();")
        End If
        objGLCode = Nothing
    End Sub

    Private Function Bindgrid(ByVal subDescCode As String, Optional ByVal pSorted As Boolean = False, Optional ByVal isSearch As Boolean = False) As String

        If strSubDescCode = "" Then
            Label1.Text = "GL Code :"
        End If
        If Not subDescCode = "" Then
            If Not Session("rowIdx") Is Nothing Then
                If Session("rowIdx") = rowIdx Then
                    strSql = "SELECT igc_glrule_category_index, igc_glrule_category, igc_glrule_category_remark FROM ipp_glrule,ipp_glrule_category, Ipp_glrule_gl" & _
                             " WHERE ig_glrule_index = igc_glrule_index And ig_coy_id = igc_coy_id " & _
                             "AND ig_glrule_index = igg_glrule_index AND igc_glrule_category_active = 'Y' And igg_gl_code LIKE '" & strGLCode & "' and igc_glrule_category like '%" & Replace(strSubDescCode, ",", "") & "%' GROUP BY igc_glrule_category"
                    ds = objDb.FillDs(strSql)
                    If ds.Tables(0).Rows.Count = 0 Then 'Same row, changed glCode
                        ds = objIPPMain.getRuleCategory(strGLCode)
                    End If
                ElseIf strSubDescCode <> "" And strGLCode <> "" Then
                    strSql = "SELECT igc_glrule_category_index, igc_glrule_category, igc_glrule_category_remark FROM ipp_glrule,ipp_glrule_category, Ipp_glrule_gl" & _
                             " WHERE ig_glrule_index = igc_glrule_index And ig_coy_id = igc_coy_id " & _
                             "AND ig_glrule_index = igg_glrule_index AND igc_glrule_category_active = 'Y' And igg_gl_code LIKE '" & strGLCode & "' and igc_glrule_category like '%" & Replace(strSubDescCode, ",", "") & "%' GROUP BY igc_glrule_category"
                    ds = objDb.FillDs(strSql)
                    If ds.Tables(0).Rows.Count = 0 Then 'Invalid strSubDescCode
                        ds = objIPPMain.getRuleCategory(strGLCode)
                    End If
                ElseIf strGLCode.Trim = txtGLCode.Text.Trim Then
                    ds = objIPPMain.getRuleCategory(strGLCode)
                Else
                    ds = objIPPMain.getRuleCategory(strGLCode)
                End If
            ElseIf strSubDescCode <> "" And strGLCode <> "" Then
                strSql = "SELECT igc_glrule_category_index, igc_glrule_category, igc_glrule_category_remark FROM ipp_glrule,ipp_glrule_category, Ipp_glrule_gl" & _
                         " WHERE ig_glrule_index = igc_glrule_index And ig_coy_id = igc_coy_id " & _
                         "AND ig_glrule_index = igg_glrule_index AND igc_glrule_category_active = 'Y' And igg_gl_code LIKE '" & strGLCode & "' and igc_glrule_category like '%" & Replace(strSubDescCode, ",", "") & "%' GROUP BY igc_glrule_category"
                ds = objDb.FillDs(strSql)
                If ds.Tables(0).Rows.Count = 0 Then 'Invalid strSubDescCode
                    ds = objIPPMain.getRuleCategory(strGLCode)
                End If
            ElseIf strSubDescCode <> "" And strGLCode <> "" And txtGLCode.Text = strSubDescCode Then
                strSql = "SELECT igc_glrule_category_index, igc_glrule_category, igc_glrule_category_remark FROM ipp_glrule,ipp_glrule_category, Ipp_glrule_gl" & _
                         " WHERE ig_glrule_index = igc_glrule_index And ig_coy_id = igc_coy_id " & _
                         "AND ig_glrule_index = igg_glrule_index AND igc_glrule_category_active = 'Y' And igg_gl_code LIKE '" & strGLCode & "' and igc_glrule_category like '%" & Replace(strSubDescCode, ",", "") & "%' GROUP BY igc_glrule_category"
                ds = objDb.FillDs(strSql)
                If ds.Tables(0).Rows.Count = 0 Then 'Invalid strSubDescCode
                    ds = objIPPMain.getRuleCategory(strGLCode)
                End If
            Else
                ds = objIPPMain.getRuleCategory(strGLCode)
            End If
        Else
            ds = objIPPMain.getRuleCategory(strGLCode)
        End If

        If isSearch Then
            ds = objIPPMain.getRuleCategory(strGLCode)
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
            NoRecord.Style("display") = "none"
            GLCode.Style("display") = "inline"
            resetDatagridPageIndex(dtgSubDesc, dvViewSample)
            dtgSubDesc.DataSource = dvViewSample
            dtgSubDesc.DataBind()
        Else
            dtgSubDesc.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            If Not Page.IsPostBack Then
                NoRecord.Style("display") = "inline"
            End If
            dtgSubDesc.DataBind()
        End If
        If Not isSearch Then
            If Not Session("SelectedSubDesc") Is Nothing And strSubDescCode = "" Then
                If Not Session("SelectedSubDesc") = "" Then
                    hidSubDescCode.Value = Session("SelectedSubDesc")
                End If
            ElseIf strSubDescCode <> "" And invalidDesc = False Then
                If Not strGLCode = "" Then
                    If Not strSubDescCode.Length < hidSubDescCode.Value.Length Then hidSubDescCode.Value = strSubDescCode 'for incomplete entry
                End If
            ElseIf invalidDesc = True Then
                Me.txtGLCode.Text = strGLCode
                Label1.Text = "GL Code :"
            End If

        End If
        ' add for above checking
        ViewState("PageCount") = dtgSubDesc.PageCount

        objGLCode = Nothing
        ds = Nothing
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgSubDesc.SortCommand
        Grid_SortCommand(sender, e)
        dtgSubDesc.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgSubDesc_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgSubDesc.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgSubDesc, e)
        intPageRecordCnt = ViewState("RecordCount")
    End Sub

    Private Sub dtgSubDesc_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgSubDesc.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim rb As RadioButton = e.Item.FindControl("rbtnSelection")
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            If strGLCode <> "" And strGLCode = dv("igc_glrule_category") Then
                rb.Checked = True
                hidSubDescCode.Value = strGLCode & ":" & dv("igc_glrule_category").ToString.Replace("'", "")
            End If
            If ds.Tables(0).Rows.Count = 1 Then
                rb.Checked = True
                hidSubDescCode.Value = dv("igc_glrule_category").ToString.Replace("'", "\'")
            End If
            rb.Attributes.Add("OnClick", "SelectOneOnly(" & rb.ClientID & ", " & "'dtgSubDesc'" & ",'" & dv("igc_glrule_category").ToString.Replace("'", "\'") & "','""""')")
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgSubDesc.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "igc_glrule_category"
        If InStr(Me.Request.QueryString("GLCode").ToString, ":") Then
            strGLCode = Me.Request.QueryString("GLCode").ToString.Substring(0, InStr(Me.Request.QueryString("GLCode").ToString, ":") - 1)
        Else
            strGLCode = Common.Parse(Me.Request.QueryString("GLCode"))
        End If
        If strGLCode Is Nothing Then strGLCode = ""
        Me.txtGLCode.Text = strGLCode
        Bindgrid(strSubDescCode, , True)
    End Sub

    Private Sub dtgSubDesc_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgSubDesc.PageIndexChanged
        Dim objGLCode As New IPP
        dtgSubDesc.CurrentPageIndex = e.NewPageIndex
        objGLCode.GetGLCode(strGLCode, strGLDesc, strGLStatus)
        Bindgrid(strSubDescCode, )
        Session("action") = ""
        objGLCode = Nothing
    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        'Session("SelectedSubDesc") = hidSubDescCode.Value
        'Session("rowIdx") = rowIdx
        Response.Write("<script language=""javascript"">window.close();</script>")
    End Sub
End Class