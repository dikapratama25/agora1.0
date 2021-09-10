Imports AgoraLegacy
Imports eProcure.Component

Public Class AssetGroupMaint
    Inherits AgoraLegacy.AppBaseClass
    Dim objGlobal As New AppGlobals
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strGroupCode As String
    Dim strGroupDesc As String
    Dim blnAsset As Boolean
    Dim blnSub As Boolean
    Dim strCodeType As String
    Dim strGroupStatus As String
    Dim blnchkActive As Boolean
    Dim blnchkInactive As Boolean
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdAdd.Enabled = True '20110628-default False
        cmdDelete.Enabled = True '20110628-default False
        cmdModify.Enabled = True '20110628-default False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdModify)
        htPageAccess.Add("modify", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdDelete)
        htPageAccess.Add("delete", alButtonList)
        'CheckButtonAccess()
        ''//additional checking
        'If ViewState("PageCount") > 0 Then
        '    cmdModify.Enabled = blnCanUpdate
        '    cmdDelete.Enabled = blnCanDelete
        'Else
        'cmdDelete.Enabled = False
        'cmdModify.Enabled = False
        'cmdReset.Disabled = True
        'End If
        alButtonList.Clear()

    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objAGroup As New IPP
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgAssetGroup)

        If Not Page.IsPostBack Then
            Session("action") = ""
            cmdDelete.Visible = False
            cmdModify.Visible = False
        End If

        If Session("action") = "Modify" Or Session("action") = "Add" Or Session("action") = "Update" Then  
            Bindgrid()
            Session("action") = ""
        End If

        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOneDtg('chkSelection','delete');")
        cmdModify.Attributes.Add("onclick", "return CheckAtLeastOneDtg('chkSelection','modify');")
        'cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        objAGroup = Nothing

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objAGroup As New IPP
        Dim ds As New DataSet
        strGroupCode = Me.txtGroupCode.Text
        strGroupDesc = Me.txtGroupDesc.Text
        blnAsset = Me.chkAsset.Checked
        blnSub = Me.chkSub.Checked
        blnchkActive = Me.chkActive.Checked
        blnchkInactive = Me.chkInactive.Checked

        If strGroupCode <> "" Or strGroupDesc <> "" Or chkAsset.Checked = True Or chkSub.Checked = True Or chkActive.Checked = True Or chkInactive.Checked = True Then
            ds = objAGroup.SearchAssetGroupInfo(strGroupCode, strGroupDesc, chkAsset.Checked, chkSub.Checked, chkActive.Checked, chkInactive.Checked)
        Else
            'ds = objAdmin.PopulateAddr(viewState("Type"), txt_Code.Text, txt_City.Text, cbo_State.SelectedItem.Value, True)
            ds = objAGroup.PopulateAssetGroup()
            'ds = objAdmin.PopulateAddr(viewState("Type"), txt_Code.Text, txt_City.Text, "")
            cmdModify.Visible = True
            cmdDelete.Visible = True
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count

        ViewState("RecordCount") = intPageRecordCnt
        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewSample.Sort += " DESC"

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If ViewState("action") = "del" Then
            If dtgAssetGroup.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgAssetGroup.PageSize = 0 Then
                dtgAssetGroup.CurrentPageIndex = dtgAssetGroup.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        If intPageRecordCnt > 0 Then
            cmdDelete.Visible = True
            cmdModify.Visible = True
            NoRecord.Style("display") = "none"
            AssetGroup.Style("display") = "inline"
            resetDatagridPageIndex(dtgAssetGroup, dvViewSample)
            dtgAssetGroup.DataSource = dvViewSample
            dtgAssetGroup.DataBind()
        Else
            '20110628-Jules
            cmdModify.Visible = False
            cmdDelete.Visible = False
            dtgAssetGroup.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            If Not Page.IsPostBack Then
                NoRecord.Style("display") = "inline"
            End If
            'AssetGroup.Style("display") = "none"
            'dtgAssetGroup.ShowHeader = True
            dtgAssetGroup.DataBind()
            'Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00006"))
        End If
        ' add for above checking
        ViewState("PageCount") = dtgAssetGroup.PageCount
        objAGroup = Nothing

    End Function

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Session("action") = ""

        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("IPPAdmin", "AssetGroupAdd.aspx", "mode=Add")
        strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
        strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','300px');")
        strscript.Append("document.getElementById('btnHidden1').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script13", strscript.ToString())

    End Sub

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim grdItem As DataGridItem
        '//Loop datagrid item
        Session("action") = ""
        For Each grdItem In dtgAssetGroup.Items
            Dim chkSelection As CheckBox = grdItem.Cells(0).FindControl("chkSelection")
            If chkSelection.Checked Then
                Dim strFileName As String
                Dim strscript As New System.Text.StringBuilder
                strscript.Append("<script language=""javascript"">")
                strFileName = dDispatcher.direct("IPPAdmin", "AssetGroupAdd.aspx", "mode=Modify&AG_Code=" & Server.UrlEncode(grdItem.Cells(2).Text) & "&AG_Desc=" & Server.UrlEncode(grdItem.Cells(3).Text) & "&Code_Type=" & Server.UrlEncode(grdItem.Cells(4).Text) & "&AG_Status=" & Server.UrlEncode(grdItem.Cells(5).Text))
                'strFileName = dDispatcher.direct("IPPAdmin", "CostCentreAddCode.aspx", "mode=Update&Code=" & Server.UrlEncode(grdItem.Cells(2).Text) & "&Desc=" & Server.UrlEncode(grdItem.Cells(3).Text) & "&dindex=" & dtgAssetGroup.DataKeys.Item(grdItem.ItemIndex) & "&pageid=" & strPageId)
                strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
                strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','300px');")
                strscript.Append("document.getElementById('btnHidden1').click();")
                strscript.Append("</script>")
                RegisterStartupScript("script13", strscript.ToString())
            End If
        Next
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim chkItem As CheckBox
        Dim objAdmin As New Admin
        Dim intMsgNo As Integer
        Dim strMsg As String
        Dim objAGroup As New IPP
        Dim dt As New DataTable
        Dim dr As DataRow

        dt.Columns.Add("AGCode", Type.GetType("System.String"))
        Session("action") = ""
        For Each dgItem In dtgAssetGroup.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                dr = dt.NewRow
                dr("AGCode") = dgItem.Cells.Item(2).Text
                dt.Rows.Add(dr)
            End If
        Next
        intMsgNo = objAGroup.DeleteAssetGroup(dt)

        If intMsgNo = -99 Then
            'Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00009"), MsgBoxStyle.Information)
            Common.NetMsgbox(Me, "Deletion is not allowed as there is transaction record tied to it.", MsgBoxStyle.Information)

        ElseIf intMsgNo = WheelMsgNum.Delete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00004"), MsgBoxStyle.Information)

        ElseIf intMsgNo = WheelMsgNum.NotDelete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00008"), MsgBoxStyle.Information)
        End If

        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)

        objAGroup.GetAssetGroupInfo(strGroupCode, strGroupDesc, strCodeType, strGroupStatus)
        Bindgrid()
        Session("action") = ""

        objAGroup = Nothing

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgAssetGroup.SortCommand
        Grid_SortCommand(sender, e)
        dtgAssetGroup.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgAssetGroup_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAssetGroup.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgAssetGroup, e)
        intPageRecordCnt = ViewState("RecordCount")
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dtgAssetGroup_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAssetGroup.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            If e.Item.Cells(4).Text = "A" Then
                e.Item.Cells(4).Text = "Asset Group"
            ElseIf e.Item.Cells(4).Text = "S" Then
                e.Item.Cells(4).Text = "Sub Group"
            End If
            If e.Item.Cells(5).Text = "A" Then
                e.Item.Cells(5).Text = "Active"
            ElseIf e.Item.Cells(5).Text = "I" Then
                e.Item.Cells(5).Text = "Inactive"
            End If

            '//to dynamic build hyoerlink
            'Dim lnkDeptCode As HyperLink
            'lnkDeptCode = e.Item.FindControl("lnkDeptCode")
            'Dim aa As String
            'lnkDeptCode.NavigateUrl = "" & dDispatcher.direct("Admin", "addDept.aspx", "mode=update&pageid=" & strPageId & "&deptcode=" & dv("CDM_DEPT_CODE") & "&deptname=" & dv("CDM_DEPT_NAME") & "&appgrpindex=" & objDb.ReturnArrayValue(dv("CDM_APPROVAL_GRP_INDEX"), UBound(dv("CDM_APPROVAL_GRP_INDEX"))))
            'lnkDeptCode.Text = dv("CDM_DEPT_CODE")

        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgAssetGroup.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "AG_GROUP"
        Bindgrid()
    End Sub

    Private Sub dtgAssetGroup_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgAssetGroup.PageIndexChanged
        Dim objAGroup As New IPP

        dtgAssetGroup.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
        Session("action") = ""

        objAGroup = Nothing

    End Sub
End Class