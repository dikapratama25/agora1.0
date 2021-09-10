Imports AgoraLegacy
Imports eProcure.Component


Public Class BranchCostCentreMaint
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objGlobal As New AppGlobals
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents btnHidden1 As System.Web.UI.WebControls.Button
    Protected WithEvents dtgBCC As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtBranchCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents cboCompany As System.Web.UI.WebControls.DropDownList
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Dim ds As DataSet
    Dim objIPPMain As New IPPMain
    Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A

    Enum enumBCC
        icChk
        icCoyID
        icBranchCode
        icBranchDesc
        icCostCentre
        icCCDesc
    End Enum

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdAdd.Enabled = False
        cmdModify.Enabled = False
        cmdDelete.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdModify)
        htPageAccess.Add("update", alButtonList)
        CheckButtonAccess()
        '//additional checking
        If intPageRecordCnt > 0 Then
            cmdModify.Enabled = blnCanUpdate
            cmdDelete.Enabled = blnCanUpdate
            '//mean Enable, can't use button.Enabled because this is a HTML button
            'cmdReset.Disabled = False
        Else
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            'cmdReset.Disabled = True
        End If
        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgBCC)

        If Not IsPostBack Then
            cmdModify.Enabled = False
            cmdDelete.Enabled = False

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
        End If

        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")

    End Sub

    Sub dtgBCC_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgBCC.PageIndexChanged
        dtgBCC.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False, Optional ByVal pMsg As Boolean = True) As String
        Dim ds As DataSet

        ds = objIPPMain.GetBranchCostCentreList(cboCompany.SelectedValue, txtBranchCode.Text)

        '//for sorting asc or desc
        Dim dvViewBCC As DataView
        dvViewBCC = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewBCC.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewBCC.Sort += " DESC"
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        '//bind datagrid
        If intPageRecordCnt > 0 Then
            cmdModify.Enabled = True
            cmdDelete.Enabled = True
            resetDatagridPageIndex(dtgBCC, dvViewBCC)
            dtgBCC.DataSource = dvViewBCC
            dtgBCC.DataBind()
            dtgBCC.Visible = True
        Else
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            dtgBCC.Visible = False
            If pMsg = True Then
                Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00006"))
            End If
        End If

        ViewState("PageCount") = dtgBCC.PageCount

    End Function

    Private Sub dtgBCC_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgBCC.ItemCreated

        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgBCC, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgBCC.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtgBCC_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgBCC.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(enumBCC.icChk).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim i As Integer
            Dim dsCCList As New DataSet
            Dim strCostCentre, strCCDesc As String
            strCostCentre = ""
            strCCDesc = ""

            dsCCList = objIPPMain.GetCCListByBranch(dv("BCC_COY_ID"), dv("BCC_BRANCH_CODE"))
            If dsCCList.Tables(0).Rows.Count > 0 Then
               
                For i = 0 To dsCCList.Tables(0).Rows.Count - 1
                    If strCostCentre = "" Then
                        strCostCentre &= dsCCList.Tables(0).Rows(i)("CC_CC_CODE")
                    Else
                        strCostCentre &= "<br/>" & dsCCList.Tables(0).Rows(i)("CC_CC_CODE")
                    End If

                    If strCCDesc = "" Then
                        strCCDesc &= dsCCList.Tables(0).Rows(i)("CC_CC_DESC")
                    Else
                        strCCDesc &= "<br/>" & dsCCList.Tables(0).Rows(i)("CC_CC_DESC")
                    End If
                Next             
            End If

            e.Item.Cells(enumBCC.icCostCentre).Text = strCostCentre
            e.Item.Cells(enumBCC.icCCDesc).Text = strCCDesc

        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgBCC.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder

        If cboCompany.SelectedValue = "" Then
            Common.NetMsgbox(Me, "Please select company.")
        Else
            strscript.Append("<script language=""javascript"">")
            strFileName = dDispatcher.direct("IPPAdmin", "AddBranchCostCentre.aspx", "pageid=" & strPageId & "&mode=add&coyid=" & cboCompany.SelectedValue)
            strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
            strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','500px','800px');")
            strscript.Append("document.getElementById('btnHidden1').click();")
            strscript.Append("</script>")
            RegisterStartupScript("script13", strscript.ToString())
        End If

    End Sub

    Private Sub cmdModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim strFileName, strIndex, strCoyId, strBranchCode As String
        Dim strscript As New System.Text.StringBuilder

        For Each dgItem In dtgBCC.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                strCoyId = dgItem.Cells(enumBCC.icCoyID).Text
                strBranchCode = dgItem.Cells(enumBCC.icBranchCode).Text
                Exit For
            End If
        Next

        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("IPPAdmin", "AddBranchCostCentre.aspx", "pageid=" & strPageId & "&mode=edit&coyid=" & strCoyId & "&branch=" & Server.UrlEncode(strBranchCode))
        strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
        strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','500px','800px');")
        strscript.Append("document.getElementById('btnHidden1').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script13", strscript.ToString())

    End Sub

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim strMsg As String
        Dim dtBCC As New DataTable
        Dim drBCC As DataRow
        Dim dsBCC As New DataSet
        dtBCC.Columns.Add("BCC_COY_ID", Type.GetType("System.String"))
        dtBCC.Columns.Add("BCC_BRANCH_CODE", Type.GetType("System.String"))

        For Each dgItem In dtgBCC.Items
            Dim chkSelection As CheckBox = dgItem.Cells(enumBCC.icChk).FindControl("chkSelection")

            If chkSelection.Checked Then
                drBCC = dtBCC.NewRow
                drBCC("BCC_COY_ID") = dgItem.Cells(enumBCC.icCoyID).Text
                drBCC("BCC_BRANCH_CODE") = dgItem.Cells(enumBCC.icBranchCode).Text
                dtBCC.Rows.Add(drBCC)
            End If
        Next

        dsBCC.Tables.Add(dtBCC)

        Dim objIPPMain As New IPPMain
        strMsg = objIPPMain.DeleteBranchWithCostCentre(dsBCC)
        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        dtgBCC.CurrentPageIndex = 0
        Bindgrid()

    End Sub

    Public Sub btnHidden1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHidden1.Click
        dtgBCC.CurrentPageIndex = 0
        Bindgrid(, False)
    End Sub
End Class
