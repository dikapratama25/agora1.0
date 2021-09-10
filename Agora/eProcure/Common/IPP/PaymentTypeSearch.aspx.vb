Imports eProcure.Component
Imports AgoraLegacy

Partial Public Class PaymentTypeSearch
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals
    Public Enum PaymentType
        chk = 0
        type = 1
        desc = 2
        status = 3
        index = 4
    End Enum
    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Dim objAssetGrp As New IPP
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgPaymentType)
        'strAssetCode = Me.Request.QueryString("AssetGroup") 'get from Raise IPP Screen

        If Not IsPostBack Then                      
            Bindgrid()
        End If
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        cmdClear.Attributes.Add("onclick", "Reset();")
        'objAssetGrp = Nothing
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objippmain As New IPPMain
        Dim objipp As New IPP
        Dim ds As New DataSet
        Dim status As String
        If chkStatus.Items.Item(1).Selected = False And chkStatus.Items.Item(0).Selected = False Then
            status = ""
        ElseIf chkStatus.Items.Item(1).Selected = True And chkStatus.Items.Item(0).Selected = True Then
            status = ""
        Else
            status = chkStatus.SelectedItem.Value
        End If

        ds = objippmain.getPaymentType(Common.Parse(txtPaymentType.Text), Common.Parse(txtDesc.Text), status)

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("RecordCount") = intPageRecordCnt
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            dtgPaymentType.DataSource = dvViewSample
            dtgPaymentType.DataBind()
        Else
            dtgPaymentType.DataBind()
            objipp.Message(Me, "00006", MsgBoxStyle.Information)
        End If
        ' add for above checking
        ViewState("PageCount") = dtgPaymentType.PageCount
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgPaymentType.SortCommand
        Grid_SortCommand(sender, e)
        dtgPaymentType.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgPaymentType_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPaymentType.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgPaymentType, e)
        intPageRecordCnt = ViewState("RecordCount")
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dtgPaymentType_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgPaymentType.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim chk As CheckBox
            chk = e.Item.FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click        
        Bindgrid()
    End Sub
    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        strFileName = dDispatcher.direct("IPP", "AddPaymentType.aspx", "mode=new")
        strscript.Append("<script language=""javascript"">")
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog('" & dDispatcher.direct("Admin", "Dialog.aspx", "page=" & strFileName) & "','530px');")
        strscript.Append("document.getElementById('btnHidden').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())
    End Sub
    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim delsql, sql As String
        Dim errmsg As String = ""
        Dim objdb As New EAD.DBCom
        For Each dgItem In dtgPaymentType.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                'check if any transaction tie to it
                sql = "SELECT im_pymt_type_index FROM invoice_mstr WHERE " & _
                "im_pymt_type_index IS NOT NULL " & _
                "AND im_pymt_type_index = '" & dgItem.Cells(PaymentType.index).Text & "'"
                If objdb.Exist(sql) Then
                    errmsg &= dgItem.Cells(PaymentType.type).Text & ","
                Else
                    'delete the row
                    delsql = "DELETE FROM PAYMENT_TYPE WHERE PT_INDEX='" & dgItem.Cells(PaymentType.index).Text & "'"
                    objdb.Execute(delsql)
                End If
                
            End If
        Next
        If errmsg <> "" Then
            errmsg = errmsg.Remove(errmsg.Length - 1, 1)
            errmsg = objGlobal.GetErrorMessage("00047") & " " & errmsg & "."
            Common.NetMsgbox(Me, errmsg, MsgBoxStyle.Information)
        Else
            errmsg = objGlobal.GetErrorMessage("00004")
            Common.NetMsgbox(Me, errmsg, MsgBoxStyle.Information)
        End If
        Bindgrid(True)
    End Sub
    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim index As String
        For Each dgItem In dtgPaymentType.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                index = dgItem.Cells(PaymentType.index).Text
            End If
        Next
        strFileName = dDispatcher.direct("IPP", "AddPaymentType.aspx", "mode=modify&index=" & index)
        strscript.Append("<script language=""javascript"">")
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog('" & dDispatcher.direct("Admin", "Dialog.aspx", "page=" & strFileName) & "','530px');")
        strscript.Append("document.getElementById('btnHidden').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())
    End Sub
    Public Sub btnHidden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHidden.Click
        Bindgrid(True)
    End Sub
    Public Sub dtgPaymentType_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgPaymentType.PageIndexChanged
        dtgPaymentType.CurrentPageIndex = e.NewPageIndex
    End Sub
End Class