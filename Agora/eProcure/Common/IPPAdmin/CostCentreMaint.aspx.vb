Imports AgoraLegacy
Imports eProcure.Component

Public Class CostCentreMaint
    Inherits AgoraLegacy.AppBaseClass
    Dim objGlobal As New AppGlobals
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim intCostCentreIndicator As Integer
    Dim strCostCentre As String
    Dim strCostCentreDesc As String
    Dim strCostCentreStatus As String
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
        Dim objCC As New IPP
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgCostCentre)
        ' If Not IsPostBack Then

        If Not Page.IsPostBack Then
            cmdDelete.Visible = False
            cmdModify.Visible = False
            Session("action") = ""
            objCC.GetCostCentreInfo(strCostCentre, strCostCentreDesc, strCostCentreStatus)

            'Chee Hong - 17/12/2014 - Only allow HLB IPP Admin for action (IPP GST Stage 2A)
            If strDefIPPCompID <> "" And strDefIPPCompID <> Session("CompanyId") Then
                cmdAdd.Visible = False
            Else
                cmdAdd.Visible = True
            End If

            'Zulham 08/05/2018 - PAMB
            If Session("CompanyID").ToString.ToUpper <> "HLB" AndAlso Session("CompanyID").ToString.ToUpper <> "HLISB" Then
                ddlcOmp.Items.Clear()
                ddlcOmp.Items.Insert(0, New ListItem("--Select--", 0))
                ddlcOmp.Items.Insert(1, New ListItem(Session("CompanyID").ToString.ToUpper, 1))
            End If
            'End

        End If

        If Session("action") = "Modify" Or Session("action") = "Add" Or Session("action") = "Update" Then
            objCC.GetCostCentreInfo(strCostCentre, strCostCentreDesc, strCostCentreStatus)
            Bindgrid()
            Session("action") = ""
        End If

        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOneDtg('chkSelection','delete');")
        cmdModify.Attributes.Add("onclick", "return CheckAtLeastOneDtg('chkSelection','modify');")
        'cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        objCC = Nothing

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objCC As New IPP
        Dim ds As New DataSet
        strCostCentre = Me.txtCostCentreCode.Text
        strCostCentreDesc = Me.txtDescription.Text

        If strCostCentre <> "" Or strCostCentreDesc <> "" Or chkActive.Checked = True Or chkInactive.Checked = True Or ddlcOmp.SelectedIndex <> 0 Then
            ds = objCC.GetCostCentreInfoSearch(strCostCentre, strCostCentreDesc, chkActive.Checked, chkInactive.Checked, Me.ddlcOmp.SelectedItem.Text)
        Else
            'ds = objAdmin.PopulateAddr(viewState("Type"), txt_Code.Text, txt_City.Text, cbo_State.SelectedItem.Value, True)

            'Zulham 14052018 - PAMB
            'Added a parameter for Company
            If Session("CompanyID").ToString.ToUpper <> "HLB" AndAlso Session("CompanyID").ToString.ToUpper <> "HLISB" Then
                ds = objCC.PopulateCostCentre(Session("CompanyID"))
            Else
                ds = objCC.PopulateCostCentre()
            End If

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
            If dtgCostCentre.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgCostCentre.PageSize = 0 Then
                dtgCostCentre.CurrentPageIndex = dtgCostCentre.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        If intPageRecordCnt > 0 Then
            cmdDelete.Visible = True
            cmdModify.Visible = True
            NoRecord.Style("display") = "none"
            CostCtr.Style("display") = "inline"
            resetDatagridPageIndex(dtgCostCentre, dvViewSample)
            dtgCostCentre.DataSource = dvViewSample
            dtgCostCentre.DataBind()
        Else
            '20110628-Jules
            cmdModify.Visible = False
            cmdDelete.Visible = False
            dtgCostCentre.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            If Not Page.IsPostBack Then
                NoRecord.Style("display") = "inline"
            End If
            'CostCtr.Style("display") = "none"
            'dtgCostCentre.ShowHeader = True
            dtgCostCentre.DataBind()
            'Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00006"))
        End If
		
        ' add for above checking
        ViewState("PageCount") = dtgCostCentre.PageCount
        objCC = Nothing

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
        strFileName = dDispatcher.direct("IPPAdmin", "CostCentreAddCode.aspx", "mode=Add")
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
        For Each grdItem In dtgCostCentre.Items
            Dim chkSelection As CheckBox = grdItem.Cells(0).FindControl("chkSelection")
            If chkSelection.Checked Then
                Dim strFileName As String
                Dim strscript As New System.Text.StringBuilder
                strscript.Append("<script language=""javascript"">")
                strFileName = dDispatcher.direct("IPPAdmin", "CostCentreAddCode.aspx", "mode=Modify&CC_Code=" & Server.UrlEncode(grdItem.Cells(2).Text) & "&CC_Desc=" & Server.UrlEncode(grdItem.Cells(3).Text) & "&CC_Status=" & Server.UrlEncode(grdItem.Cells(5).Text) & "&CC_Comp=" & Server.UrlEncode(grdItem.Cells(4).Text))
                'strFileName = dDispatcher.direct("IPPAdmin", "CostCentreAddCode.aspx", "mode=Update&Code=" & Server.UrlEncode(grdItem.Cells(2).Text) & "&Desc=" & Server.UrlEncode(grdItem.Cells(3).Text) & "&dindex=" & dtgCostCentre.DataKeys.Item(grdItem.ItemIndex) & "&pageid=" & strPageId)
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
        Dim objCC As New IPP
        Dim dt As New DataTable
        Dim dr As DataRow

        dt.Columns.Add("CCCode", Type.GetType("System.String"))
        dt.Columns.Add("CCComp", Type.GetType("System.String"))
        Session("action") = ""
        For Each dgItem In dtgCostCentre.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                dr = dt.NewRow
                dr("CCCode") = dgItem.Cells.Item(2).Text
                dr("CCComp") = dgItem.Cells.Item(4).Text
                dt.Rows.Add(dr)
            End If
        Next
        intMsgNo = objCC.DeleteCostCentre(dt)

        If intMsgNo = -99 Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00018"), MsgBoxStyle.Information)
            'Common.NetMsgbox(Me, "Deletion is not allowed as there is transaction record tied to it.", MsgBoxStyle.Information)

        ElseIf intMsgNo = WheelMsgNum.Delete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00004"), MsgBoxStyle.Information)

        ElseIf intMsgNo = WheelMsgNum.NotDelete Then
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00008"), MsgBoxStyle.Information)
        End If

        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)

        objCC.GetCostCentreInfo(strCostCentre, strCostCentreDesc, strCostCentreStatus)
        Bindgrid()
        Session("action") = ""

        objCC = Nothing

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgCostCentre.SortCommand
        Grid_SortCommand(sender, e)
        dtgCostCentre.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgCostCentre_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCostCentre.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgCostCentre, e)
        intPageRecordCnt = ViewState("RecordCount")
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dtgCostCentre_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgCostCentre.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

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
        dtgCostCentre.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "CC_CC_CODE"
        Bindgrid()
    End Sub

    Private Sub dtgCostCentre_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCostCentre.PageIndexChanged
        Dim objCC As New IPP

        dtgCostCentre.CurrentPageIndex = e.NewPageIndex
        objCC.GetCostCentreInfo(strCostCentre, strCostCentreDesc, strCostCentreStatus)
        Bindgrid()
        Session("action") = ""

        objCC = Nothing

    End Sub

    'Private Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.ServerClick
    '    ddlcOmp.SelectedIndex = 0
    'End Sub
End Class