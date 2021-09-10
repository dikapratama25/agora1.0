Imports AgoraLegacy
Imports eProcure.Component

Public Class InventoryLocMaint
    Inherits AgoraLegacy.AppBaseClass
    Dim objGlobal As New AppGlobals
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim intLocIndicator As Integer
    Dim strLoc As String
    Dim strSLoc As String
    Protected WithEvents lblLocLabel As System.Web.UI.WebControls.Label
    Protected WithEvents lblSubLocLabel As System.Web.UI.WebControls.Label
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
        cmdAdd.Enabled = False
        cmdDelete.Enabled = False
        cmdModify.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdModify)
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdDelete)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        '//additional checking
        If ViewState("PageCount") > 0 Then
            cmdModify.Enabled = blnCanUpdate
            cmdDelete.Enabled = blnCanDelete
        Else
            cmdDelete.Enabled = False
            cmdModify.Enabled = False
            'cmdReset.Disabled = True
        End If
        alButtonList.Clear()

    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objInv As New Inventory
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgLocation)
        ' If Not IsPostBack Then
        
        If Not Page.IsPostBack Then
            Session("action") = ""
            objInv.GetLocationInfo(strLoc, strSLoc, intLocIndicator)
            dtgLocation.CurrentPageIndex = 0
            Bindgrid()
            If intLocIndicator > 1 Then 'Sub Location is defined
                Me.dtgLocation.Columns(3).Visible = True
            Else
                Me.dtgLocation.Columns(3).Visible = False
            End If
        End If

        If Session("action") = "Modify" Or Session("action") = "Add" Or Session("action") = "Update" Then
            objInv.GetLocationInfo(strLoc, strSLoc, intLocIndicator)
            Bindgrid()
            'ChangeHeaderText()
            If intLocIndicator > 1 Then 'Sub Location is defined
                Me.dtgLocation.Columns(3).Visible = True
            Else
                Me.dtgLocation.Columns(3).Visible = False
            End If
            Session("action") = ""
        End If
        ChangeHeaderText()
        'End If
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        objInv = Nothing

    End Sub

    Private Sub ChangeHeaderText()
        Dim objInv As New Inventory
        Dim strLoc1 As String = " (Location Label Description)"
        Dim strSLoc1 As String = " (Sub Location Label Description)"
        objInv.GetLocationInfo(strLoc, strSLoc, intLocIndicator)

        Me.dtgLocation.Columns(2).HeaderText = strLoc & strLoc1
        Me.dtgLocation.Columns(3).HeaderText = strSLoc & strSLoc1
        Me.lblLocLabel.Text = strLoc & strLoc1
        Me.lblSubLocLabel.Text = strSLoc & strSLoc1
        objInv = Nothing

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objInv As New Inventory
        Dim ds As New DataSet
        'ds = objAdmin.PopulateAddr(viewState("Type"), txt_Code.Text, txt_City.Text, cbo_State.SelectedItem.Value, True)
        ds = objInv.PopulateLocation
        'ds = objAdmin.PopulateAddr(viewState("Type"), txt_Code.Text, txt_City.Text, "")
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
            If dtgLocation.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgLocation.PageSize = 0 Then
                dtgLocation.CurrentPageIndex = dtgLocation.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        If intPageRecordCnt > 0 Then
            NoRecord.Style("display") = "none"
            Loc.Style("display") = "inline"
            resetDatagridPageIndex(dtgLocation, dvViewSample)
            dtgLocation.DataSource = dvViewSample
            dtgLocation.DataBind()
        Else
            NoRecord.Style("display") = "inline"
            Loc.Style("display") = "none"
            'dtgLocation.ShowHeader = True
            'dtgLocation.DataBind()
            'Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00006"))
        End If
        ' add for above checking
        ViewState("PageCount") = dtgLocation.PageCount
        objInv = Nothing

    End Function

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim grdItem As DataGridItem
        '//Loop datagrid item
        Session("action") = ""
        For Each grdItem In dtgLocation.Items
            Dim chkSelection As CheckBox = grdItem.Cells(0).FindControl("chkSelection")
            If chkSelection.Checked Then
                Dim strFileName As String
                Dim strscript As New System.Text.StringBuilder
                strscript.Append("<script language=""javascript"">")
                strFileName = dDispatcher.direct("Inventory", "InventoryAddLocation.aspx", "mode=Update&Loc=" & Server.UrlEncode(grdItem.Cells(2).Text) & "&SubLoc=" & Server.UrlEncode(grdItem.Cells(3).Text) & "&dindex=" & dtgLocation.DataKeys.Item(grdItem.ItemIndex) & "&pageid=" & strPageId)
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
        Dim strCode As String
        Dim chkItem As CheckBox
        Dim objAdmin As New Admin
        Dim intMsgNo As Integer
        Dim strMsg As String
        Dim strNotDeleted As String
        Dim objInv As New Inventory

        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add("LocIndex", Type.GetType("System.String"))
        Session("action") = ""
        For Each dgItem In dtgLocation.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                dr = dt.NewRow
                dr("LocIndex") = dtgLocation.DataKeys.Item(dgItem.ItemIndex)
                dt.Rows.Add(dr)
            End If
        Next
        intMsgNo = objInv.DeleteLocation(dt)

        If intMsgNo = -99 Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00009"), MsgBoxStyle.Information)

        ElseIf intMsgNo = WheelMsgNum.Delete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00004"), MsgBoxStyle.Information)

        ElseIf intMsgNo = WheelMsgNum.NotDelete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00008"), MsgBoxStyle.Information)
        End If

        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)

        objInv.GetLocationInfo(strLoc, strSLoc, intLocIndicator)
        Bindgrid()
        ChangeHeaderText()
        If intLocIndicator > 1 Then 'Sub Location is defined
            Me.dtgLocation.Columns(3).Visible = True
        Else
            Me.dtgLocation.Columns(3).Visible = False
        End If
        Session("action") = ""

        objInv = Nothing

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgLocation.SortCommand
        Grid_SortCommand(sender, e)
        dtgLocation.CurrentPageIndex = 0
        Bindgrid(True)
        ChangeHeaderText()
    End Sub

    Private Sub dtgLocation_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgLocation.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgLocation, e)
        intPageRecordCnt = ViewState("RecordCount")
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
        ChangeHeaderText()
    End Sub

    Private Sub dtgLocation_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgLocation.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            '//to dynamic build hyoerlink
            'Dim lnkDeptCode As HyperLink
            'lnkDeptCode = e.Item.FindControl("lnkDeptCode")
            'Dim aa As String
            'lnkDeptCode.NavigateUrl = "" & dDispatcher.direct("Admin", "addDept.aspx", "mode=update&pageid=" & strPageId & "&deptcode=" & dv("CDM_DEPT_CODE") & "&deptname=" & dv("CDM_DEPT_NAME") & "&appgrpindex=" & objDb.ReturnArrayValue(dv("CDM_APPROVAL_GRP_INDEX"), UBound(dv("CDM_APPROVAL_GRP_INDEX"))))
            'lnkDeptCode.Text = dv("CDM_DEPT_CODE")

        End If
        ChangeHeaderText()
    End Sub

    Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        Session("action") = ""
        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("Inventory", "InventoryLocDesc.aspx")
        strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
        strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','300px');")
        strscript.Append("document.getElementById('btnHidden1').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script13", strscript.ToString())

    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Session("action") = ""

        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("Inventory", "InventoryAddLocation.aspx", "mode=Add")
        strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
        strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','300px');")
        strscript.Append("document.getElementById('btnHidden1').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script13", strscript.ToString())

    End Sub

    Private Sub dtgLocation_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgLocation.PageIndexChanged
        Dim objInv As New Inventory

        dtgLocation.CurrentPageIndex = e.NewPageIndex
        objInv.GetLocationInfo(strLoc, strSLoc, intLocIndicator)
        Bindgrid()
        ChangeHeaderText()
        If intLocIndicator > 1 Then 'Sub Location is defined
            Me.dtgLocation.Columns(3).Visible = True
        Else
            Me.dtgLocation.Columns(3).Visible = False
        End If
        Session("action") = ""

        objInv = Nothing

    End Sub

End Class