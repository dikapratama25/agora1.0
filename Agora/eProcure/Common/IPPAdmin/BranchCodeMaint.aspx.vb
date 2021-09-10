Imports AgoraLegacy
Imports eProcure.Component

Public Class BranchCodeMaint
    Inherits AgoraLegacy.AppBaseClass
    Dim objGlobal As New AppGlobals
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strCoyBranch As String
    Dim strBranchName As String
    Dim strBranchStatus As String
    Dim blnchkActive As Boolean
    Dim blnchkInactive As Boolean
    Dim strBranchGLCode As String
    Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A

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
        Dim objBranch As New IPP
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCoyBranch)

        If Not Page.IsPostBack Then
            Session("action") = ""
            dtgCoyBranch.CurrentPageIndex = 0
            Bindgrid()
        End If

        If Session("action") = "Modify" Or Session("action") = "Add" Or Session("action") = "Update" Then
            Bindgrid()
            Session("action") = ""
        End If

        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOneDtg('chkSelection','delete');")
        cmdModify.Attributes.Add("onclick", "return CheckAtLeastOneDtg('chkSelection','modify');")
        'cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        objBranch = Nothing

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objBranch As New IPP
        Dim ds As New DataSet
        strCoyBranch = Me.txtHOBRCode.Text
        strBranchName = Me.txtHOBRName.Text
        blnchkActive = Me.chkActive.Checked
        blnchkInactive = Me.chkInactive.Checked
        strBranchGLCode = Me.txtGLCode.Text

        ds = objBranch.GetHOBR(IIf(Me.ddlCompType.SelectedValue.ToString = "0", "", ddlCompType.SelectedItem.Text), IIf(ddlHOBR.SelectedValue.ToString = "0", "", Me.ddlHOBR.SelectedItem.Text), strCoyBranch, strBranchName, chkActive.Checked, chkInactive.Checked, strBranchGLCode)

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
            If dtgCoyBranch.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgCoyBranch.PageSize = 0 Then
                dtgCoyBranch.CurrentPageIndex = dtgCoyBranch.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        If intPageRecordCnt > 0 Then
            NoRecord.Style("display") = "none"
            CoyBranch.Style("display") = "inline"
            resetDatagridPageIndex(dtgCoyBranch, dvViewSample)
            dtgCoyBranch.DataSource = dvViewSample
            dtgCoyBranch.DataBind()
            cmdModify.Visible = True
            cmdDelete.Visible = True
        Else
            '20110628-Jules
            cmdModify.Visible = False
            cmdDelete.Visible = False
            dtgCoyBranch.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            If Not Page.IsPostBack Then
                NoRecord.Style("display") = "inline"
            End If
            'CoyBranch.Style("display") = "none"
            'dtgCoyBranch.ShowHeader = True
            dtgCoyBranch.DataBind()
            'Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00006"))
        End If
        ' add for above checking
        ViewState("PageCount") = dtgCoyBranch.PageCount
        objBranch = Nothing

        'Chee Hong - 17/12/2014 - Only allow HLB IPP Admin for action (IPP GST Stage 2A)
        If strDefIPPCompID <> "" And strDefIPPCompID <> Session("CompanyId") Then
            cmdAdd.Visible = False
            cmdModify.Visible = False
            cmdDelete.Visible = False
        Else
            cmdAdd.Visible = True
            cmdModify.Visible = True
            cmdDelete.Visible = True
        End If

    End Function

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Session("action") = ""

        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("IPPAdmin", "BranchCodeAdd.aspx", "mode=Add")
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
        For Each grdItem In dtgCoyBranch.Items
            Dim chkSelection As CheckBox = grdItem.Cells(0).FindControl("chkSelection")
            If chkSelection.Checked Then
                Dim strFileName As String
                Dim strscript As New System.Text.StringBuilder
                strscript.Append("<script language=""javascript"">")
                strFileName = dDispatcher.direct("IPPAdmin", "BranchCodeAdd.aspx", "mode=Modify&cbm_index=" & Server.UrlEncode(grdItem.Cells(8).Text))
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
        Dim objBranch As New IPP
        Dim dt As New DataTable
        Dim dr As DataRow

        dt.Columns.Add("CBCoy", Type.GetType("System.String"))
        dt.Columns.Add("CBCode", Type.GetType("System.String"))
        dt.Columns.Add("CBM_Index", Type.GetType("System.Int32"))
        Session("action") = ""
        For Each dgItem In dtgCoyBranch.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                dr = dt.NewRow
                dr("CBCoy") = dgItem.Cells.Item(2).Text
                dr("CBCode") = dgItem.Cells.Item(4).Text
                dr("CBM_Index") = dgItem.Cells.Item(8).Text
                dt.Rows.Add(dr)
            End If
        Next

        intMsgNo = objBranch.DeleteCompBranchMstr(dt)

        If intMsgNo = -99 Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00009"), MsgBoxStyle.Information)

        ElseIf intMsgNo = WheelMsgNum.Delete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00004"), MsgBoxStyle.Information)

        ElseIf intMsgNo = WheelMsgNum.NotDelete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00008"), MsgBoxStyle.Information)
        End If

        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)

        Bindgrid()
        Session("action") = ""

        objBranch = Nothing

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgCoyBranch.SortCommand
        Grid_SortCommand(sender, e)
        dtgCoyBranch.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgCoyBranch_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCoyBranch.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgCoyBranch, e)
        intPageRecordCnt = ViewState("RecordCount")
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dtgCoyBranch_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCoyBranch.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            If e.Item.Cells(4).Text = "A" Then
                e.Item.Cells(4).Text = "Active"
            ElseIf e.Item.Cells(4).Text = "I" Then
                e.Item.Cells(4).Text = "Inactive"
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
        dtgCoyBranch.CurrentPageIndex = 0
        ViewState("SortAscending") = "no"
        ViewState("SortExpression") = "CBM_BRANCH_CODE"
        Bindgrid()
    End Sub

    Private Sub dtgCoyBranch_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCoyBranch.PageIndexChanged
        Dim objBranch As New IPP

        dtgCoyBranch.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
        Session("action") = ""

        objBranch = Nothing

    End Sub
End Class