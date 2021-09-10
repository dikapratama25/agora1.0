Imports AgoraLegacy
Imports eProcure.Component


Public Class GLAnalysisRule
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objGlobal As New AppGlobals
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents btnHidden1 As System.Web.UI.WebControls.Button
    Protected WithEvents dtgGL As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtRuleCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Dim ds As DataSet
    Dim strPrevRule As String
    Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID") 'Modified for IPP GST Stage 2A

    Enum enumRule
        icChk
        icRuleCode
        icCat
        icRemark
        icGL
        icDesc
        icIndex
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
        SetGridProperty(dtgGL)

        If Not IsPostBack Then
            cmdSearch.Enabled = True
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

    Sub dtggl_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgGL.PageIndexChanged
        dtgGL.CurrentPageIndex = e.NewPageIndex
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim ds As DataSet
        Dim objIPPMain As New IPPMain

        ds = objIPPMain.GetGLRuleCodeList(txtRuleCode.Text)

        '//for sorting asc or desc
        Dim dvViewGL As DataView
        dvViewGL = ds.Tables(0).DefaultView

        If pSorted Then
            dvViewGL.Sort = ViewState("SortExpression")
            If ViewState("SortAscending") = "no" And ViewState("SortExpression") <> "" Then dvViewGL.Sort += " DESC"
        End If


        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt
        strPrevRule = ""
        '//bind datagrid
        If intPageRecordCnt > 0 Then
            cmdModify.Enabled = True
            cmdDelete.Enabled = True
            resetDatagridPageIndex(dtgGL, dvViewGL)
            dtgGL.DataSource = dvViewGL
            dtgGL.DataBind()
            dtgGL.Visible = True
        Else
            cmdModify.Enabled = False
            cmdDelete.Enabled = False
            dtgGL.Visible = False
            Common.NetMsgbox(Me, MsgNoRecord)
        End If

        ViewState("PageCount") = dtgGL.PageCount
    End Function

    Private Sub dtggl_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGL.ItemCreated

        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgGL, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgGL.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub dtggl_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgGL.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim i As Integer
            Dim strRemark() As String
            Dim strRemarks As String

            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(enumRule.icChk).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            If strPrevRule = "" Then
                e.Item.Cells(enumRule.icRuleCode).Text = dv("IG_GLRULE_CODE")
                strPrevRule = dv("IG_GLRULE_CODE")
                chk.Visible = True
            Else
                If strPrevRule = dv("IG_GLRULE_CODE") Then
                    e.Item.Cells(enumRule.icRuleCode).Text = ""
                    chk.Visible = False
                Else
                    e.Item.Cells(enumRule.icRuleCode).Text = dv("IG_GLRULE_CODE")
                    strPrevRule = dv("IG_GLRULE_CODE")
                    chk.Visible = True
                End If
            End If

            If Not IsDBNull(dv("IGC_GLRULE_CATEGORY_REMARK")) Then
                strRemark = Split(dv("IGC_GLRULE_CATEGORY_REMARK"), Chr(13))
                strRemarks = ""
                For i = 0 To strRemark.Length - 1
                    If strRemarks = "" Then
                        strRemarks &= strRemark(i)
                    Else
                        strRemarks &= "<br>" & strRemark(i)
                    End If
                Next
                e.Item.Cells(enumRule.icRemark).Text = strRemarks
            End If

        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgGL.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim strFileName As String
        Dim strscript As New System.Text.StringBuilder

        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("IPPAdmin", "AddGLAnalysis.aspx", "pageid=" & strPageId & "&mode=new")
        strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
        strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','500px','600px');")
        strscript.Append("document.getElementById('btnHidden1').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script13", strscript.ToString())

    End Sub

    Private Sub cmdModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim chk As CheckBox
        Dim strFileName, strIndex As String
        Dim strscript As New System.Text.StringBuilder

        For Each dgItem In dtgGL.Items
            chk = dgItem.FindControl("chkSelection")
            If chk.Checked Then
                strIndex = dgItem.Cells(enumRule.icIndex).Text
                Exit For
            End If
        Next

        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("IPPAdmin", "AddGLAnalysis.aspx", "pageid=" & strPageId & "&mode=edit&ruleindex=" & strIndex)
        strFileName = Replace(Server.UrlEncode(strFileName), "'", "%27")
        strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','500px','600px');")
        strscript.Append("document.getElementById('btnHidden1').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script13", strscript.ToString())

    End Sub

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim strMsg As String
        Dim dtNewRule As New DataTable
        Dim drRule As DataRow
        Dim dsDelRule As New DataSet
        dtNewRule.Columns.Add("RuleIndex", Type.GetType("System.String")) '//product code

        For Each dgItem In dtgGL.Items
            Dim chkSelection As CheckBox = dgItem.Cells(enumRule.icChk).FindControl("chkSelection")

            If chkSelection.Checked Then
                drRule = dtNewRule.NewRow
                drRule("RuleIndex") = dgItem.Cells(enumRule.icIndex).Text
                dtNewRule.Rows.Add(drRule)
            End If
        Next

        dsDelRule.Tables.Add(dtNewRule)

        Dim objIPPMain As New IPPMain
        strMsg = objIPPMain.DeleteGLRuleCode(dsDelRule)
        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        dtgGL.CurrentPageIndex = 0
        Bindgrid()

    End Sub

    Public Sub btnHidden1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHidden1.Click
        If dtgGL.Visible = True Then
            dtgGL.CurrentPageIndex = 0
            Bindgrid()
        End If
    End Sub
End Class
