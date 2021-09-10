'Copyright © 2013 STRATEQ GLOBAL SERVICES. All rights reserved.
Imports AgoraLegacy
Imports eProcure.Component
Public Class ReportMatrix
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents btnColEdit As New System.Web.UI.WebControls.ButtonColumn
    Protected WithEvents btnColDelete As New System.Web.UI.WebControls.ButtonColumn
    Protected WithEvents btnColEdit2 As New System.Web.UI.WebControls.ButtonColumn
    Protected WithEvents btnColDelete2 As New System.Web.UI.WebControls.ButtonColumn


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtRpt As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_result As System.Web.UI.WebControls.Label
    Protected WithEvents dtgUser As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtgRpt As System.Web.UI.WebControls.DataGrid
    Protected WithEvents rdUser As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rdRpt As System.Web.UI.WebControls.RadioButton
    Protected WithEvents Textbox3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUserName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUserID As System.Web.UI.WebControls.TextBox
    Protected WithEvents tbUser As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents trUser As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents xtRpt As System.Web.UI.WebControls.TextBox
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents tbRpt As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents cmdSearchUser As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSearchRpt As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAssign As System.Web.UI.WebControls.Button
    Protected WithEvents Rpt As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Usr As System.Web.UI.HtmlControls.HtmlTableRow

    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        InitializeComponent()

    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        blnSorting = False
        SetGridProperty(dtgRpt)
        SetGridProperty(dtgUser)
        IsAuthentication()

        ViewState("Mode") = Request.Params("mode")

        If Not Page.IsPostBack Then
            If ViewState("Mode") = "User" Then
                rdUser.Checked = True
            ElseIf ViewState("Mode") = "Rpt" Then
                rdRpt.Checked = True
                tbUser.Style.Item("Display") = ""
                tbUser.Style.Item("Display") = "none"
            End If

            Rpt.Style("display") = "none"
            Usr.Style("display") = "none"
        End If

    End Sub

    Private Sub Search(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearchUser.Click, cmdSearchRpt.Click
        dtgRpt.CurrentPageIndex = 0
        dtgUser.CurrentPageIndex = 0
        'ViewState("duplic") = ""
        If rdUser.Checked = True Then
            Bindgrid(dtgUser)
            dtgUser.DataBind()
            'Rpt.Visible = False
            If dtgUser.Items.Count > 0 Then
                Usr.Style("display") = ""
            End If
            Rpt.Style("display") = "none"
        Else
            Bindgrid(dtgRpt)
            'Rpt.Visible = True
            If dtgRpt.Items.Count > 0 Then
                Rpt.Style("display") = ""
            End If
            Usr.Style("display") = "none"
            dtgRpt.DataBind()
        End If
    End Sub

    Private Function Filter(ByRef pDt As DataTable, ByVal pKey As String) As DataTable
        Dim dt As DataTable
        Dim sSelect As String = "RM_REPORT_NAME" & Common.ParseSQL(pKey)
        dt = pDt.Clone
        For Each row As DataRow In pDt.Select(sSelect, "", DataViewRowState.CurrentRows)
            dt.ImportRow(row)
        Next
        Return dt
    End Function

    Private Function Bindgrid(ByVal source As DataGrid, Optional ByVal pSorted As Boolean = False) As String
        Dim strUserName As String
        Dim strUserID As String
        Dim strRpt As String

        strUserName = txtUserName.Text
        strUserID = txtUserID.Text
        strRpt = txtRpt.Text

        Dim objRpt As New Report

        Dim ds As DataSet = New DataSet
        Dim dvViewUser As DataView

        If rdUser.Checked Then
            ds = objRpt.getReportMatrixByType("U", strUserName, strUserID)
            dvViewUser = ds.Tables(0).DefaultView
            intPageRecordCnt = ds.Tables(0).Rows.Count
        Else
            ds = objRpt.getReportMatrixByType("R", strRpt)
            dvViewUser = ds.Tables(0).DefaultView
            intPageRecordCnt = ds.Tables(0).Rows.Count
        End If


        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(source, dvViewUser)
            source.DataSource = dvViewUser
            source.DataBind()
        Else
            source.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ViewState("PageCount") = source.PageCount
    End Function

    Private Sub dtgUser_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgUser.ItemDataBound

        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(3).ColumnSpan = 2
            e.Item.Cells(4).Visible = False
            e.Item.Cells(3).Text = "Function"
        ElseIf e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            CheckButtonRight(e)
            Dim rpt As Repeater
            rpt = e.Item.FindControl("sub")
            rpt.DataSource = GetRptByUser("URpt", e.Item.Cells(1).Text)
            rpt.DataBind()

            If CType(e.Item.Cells(2).Controls(1), Repeater).Items.Count = 0 Then
                e.Item.Cells(2).Text = "None"
            End If
        End If
    End Sub

    Private Sub dtgRpt_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgRpt.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(3).ColumnSpan = 2
            e.Item.Cells(4).Visible = False
            e.Item.Cells(3).Text = "Function"
        ElseIf e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            CheckButtonRight(e)
            Dim rptUserName As Repeater
            Dim rptUserID As Repeater

            rptUserName = e.Item.FindControl("rptUserName")
            rptUserID = e.Item.FindControl("rptUserID")

            rptUserName.DataSource = GetRptByUser("RUser", e.Item.Cells(5).Text)
            rptUserName.DataBind()
            rptUserID.DataSource = GetRptByUser("RUser", e.Item.Cells(5).Text)
            rptUserID.DataBind()

            If CType(e.Item.Cells(1).Controls(1), Repeater).Items.Count = 0 Then
                e.Item.Cells(1).Text = ""
                e.Item.Cells(2).Text = ""
            End If

        End If
    End Sub

    Private Sub CheckButtonRight(ByVal e)
        Dim bcUpdate As LinkButton
        Dim bcDelete As LinkButton

        'If rdUser.Checked Then
        '    bcUpdate = CType(e.Item.Cells(5).Controls(0), LinkButton)
        '    bcDelete = CType(e.Item.Cells(6).Controls(0), LinkButton)
        'Else
        bcUpdate = CType(e.Item.Cells(3).Controls(0), LinkButton)
        bcDelete = CType(e.Item.Cells(4).Controls(0), LinkButton)
        'End If


        If blnCanUpdate = False Then
            bcUpdate.Enabled = False
        End If

        If blnCanDelete = False Then
            bcDelete.Enabled = False
        Else
            bcDelete.Attributes.Add("Onclick", "return CustomMsg('deleteUA');")
        End If

    End Sub

    Private Sub dtgRpt_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgRpt.ItemCommand
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim strRptIndex As String = e.Item.Cells(5).Text
            Dim strRptName As String = e.Item.Cells(0).Text

            If e.CommandName = "Modify" Then
                Response.Redirect(dDispatcher.direct("Report", "ReportMatrixAssRpt.aspx", "pageid=" & strPageId & "&RptIndex=" & strRptIndex & "&RptName=" & Server.UrlEncode(strRptName)))
            ElseIf e.CommandName = "Delete" Then
                Delete("R", strRptIndex)
            End If
        End If
    End Sub

    Private Sub dtgUser_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgUser.ItemCommand
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim strUserID As String = e.Item.Cells(1).Text
            Dim strUserName As String = e.Item.Cells(0).Text

            If e.CommandName = "Modify" Then
                Response.Redirect(dDispatcher.direct("Report", "ReportMatrixAssUser.aspx", "pageid=" & strPageId & "&UserID=" & strUserID & "&UserName=" & Server.UrlEncode(strUserName)))
            ElseIf e.CommandName = "Delete" Then
                Delete("U", strUserID)
            End If
        End If
    End Sub

    Private Sub Delete(ByVal delBy As String, ByVal pParam As String)
        Dim objRpt As New Report
        If objRpt.DelRptMatrix(delBy, pParam) Then
            ViewState("duplic") = ""
            Common.NetMsgbox(Me, "Record removed.", MsgBoxStyle.Information)
            If delBy = "U" Then
                Bindgrid(dtgUser)
            Else
                Bindgrid(dtgRpt)
            End If
        Else
            Common.NetMsgbox(Me, "No record removed.", MsgBoxStyle.Information)
        End If
    End Sub

    Public Sub PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        source.CurrentPageIndex = e.NewPageIndex
        ViewState("duplic") = ""
        Bindgrid(source)

    End Sub

    Public Sub itemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        Grid_ItemCreated(sender, e)
    End Sub

    Private Sub rdUser_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdUser.CheckedChanged
        If rdUser.Checked Then
            tbUser.Style.Item("Display") = ""
            tbRpt.Style.Item("Display") = "none"
            Rpt.Style("display") = "none"
            'Usr.Style("display") = "none"
            'dtgRpt.DataBind()
        End If
    End Sub

    Private Sub rdRpt_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdRpt.CheckedChanged
        If rdRpt.Checked Then
            tbRpt.Style.Item("Display") = ""
            tbUser.Style.Item("Display") = "none"
            'Rpt.Style("display") = "none"
            Usr.Style("display") = "none"
            'dtgUser.DataBind()
        End If
    End Sub
    Private Function GetRptByUser(ByVal pBytype As String, ByVal param1 As String) As DataSet
        Dim objRpt As New Report
        Dim ds As DataSet
        ds = objRpt.getReportMatrixByType(pBytype, param1)
        Return ds
        objRpt = Nothing
    End Function

    Private Sub cmdAssign_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAssign.Click
        Response.Redirect(dDispatcher.direct("Report", "ReportMatrixAssMultiUser.aspx", "pageid=" & strPageId))
    End Sub
End Class
