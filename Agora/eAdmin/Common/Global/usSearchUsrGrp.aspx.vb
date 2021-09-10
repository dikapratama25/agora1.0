Imports AgoraLegacy
Imports SSO.Component
Public Class usSearchUsrGrp
    Inherits AgoraLegacy.AppBaseClass
    Dim objDb As New  EAD.DBCom
    Dim dDispatcher As New AgoraLegacy.dispatcher
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents lbl_result As System.Web.UI.WebControls.Label
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents dgUsrGrp As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtUsrGrpID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUsrGrpName As System.Web.UI.WebControls.TextBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(dgUsrGrp)

        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim strUsrGrpID As String
        Dim strUsrGrpName As String

        strUsrGrpID = Me.txtUsrGrpID.Text
        strUsrGrpName = Me.txtUsrGrpName.Text

        Dim objUsers As New Users

        '//Retrieve Data from Database
        Dim ds As DataSet = New DataSet
        ds = objUsers.SearchUserGroup(strUsrGrpID, strUsrGrpName)

        '//for sorting asc or desc
        Dim dvViewDept As DataView
        dvViewDept = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)
        'If pSorted Then
        dvViewDept.Sort = viewstate("SortExpression")
        If viewstate("SortAscending") = "no" Then dvViewDept.Sort += " DESC"
        'End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If dgUsrGrp.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dgUsrGrp.PageSize = 0 Then
                dgUsrGrp.CurrentPageIndex = dgUsrGrp.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        If intPageRecordCnt > 0 Then
            ' check when user re-enter search criteria and click on other page without click search button
            If dgUsrGrp.CurrentPageIndex > (dvViewDept.Count \ dgUsrGrp.PageSize) Then
                dgUsrGrp.CurrentPageIndex = IIf((dvViewDept.Count \ dgUsrGrp.PageSize) = 1, 0, (dvViewDept.Count \ dgUsrGrp.PageSize))
            ElseIf dgUsrGrp.CurrentPageIndex = (dvViewDept.Count \ dgUsrGrp.PageSize) Then
                If viewstate("PageCount") = (dvViewDept.Count \ dgUsrGrp.PageSize) Then
                    'user does not re-enter search criteria 
                    dgUsrGrp.CurrentPageIndex = IIf((dvViewDept.Count \ dgUsrGrp.PageSize) = 0, 0, (dvViewDept.Count \ dgUsrGrp.PageSize) - 1)
                Else
                    If (dvViewDept.Count Mod dgUsrGrp.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dgUsrGrp.CurrentPageIndex = IIf((dvViewDept.Count \ dgUsrGrp.PageSize) = 1, 0, (dvViewDept.Count \ dgUsrGrp.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        dgUsrGrp.CurrentPageIndex = (dvViewDept.Count \ dgUsrGrp.PageSize)
                    End If
                End If
            End If
            '--------------------------------
            'intTotPage = dtgDept.PageCount
            cmdDelete.Enabled = True
            cmdModify.Enabled = True
            '//mean Enable, can't use button.Enabled because this is a HTML button
            cmdReset.Disabled = False
            dgUsrGrp.DataSource = dvViewDept
            dgUsrGrp.DataBind()
        Else
            'dtgDept.DataSource = ""
            cmdDelete.Enabled = False
            cmdModify.Enabled = False
            cmdReset.Disabled = True
            dgUsrGrp.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            'intTotPage = 0
        End If
        ' add for above checking
        viewstate("PageCount") = dgUsrGrp.PageCount
        'ShowStats()
    End Function

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dgUsrGrp.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dtgDept_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgUsrGrp.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dgUsrGrp, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dgUsrGrp_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgUsrGrp.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            'to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            'to dynamic build hyoerlink
            Dim lnkUsrGrpID As HyperLink
            lnkUsrGrpID = e.Item.FindControl("lnkUsrGrpID")

            'lnkUsrGrpID.NavigateUrl = "UsUsrGrp.aspx?mode=modify&usrgrpid=" & _
            lnkUsrGrpID.NavigateUrl = dDispatcher.direct("Global", "UsUsrGrp.aspx", "mode=modify&usrgrpid=" & Server.UrlEncode(dv("UGM_USRGRP_ID")) & "&Package=" & Server.UrlEncode(dv("UGM_APP_PKG")))
            'Server.UrlEncode(dv("UGM_USRGRP_ID")) & "&Package=" & _
            'Server.UrlEncode(dv("UGM_APP_PKG"))
            lnkUsrGrpID.Text = dv("UGM_USRGRP_ID")

            Dim lblAppPackage As Label
            lblAppPackage = e.Item.FindControl("lblAppPackage")
            lblAppPackage.Text = dv("UGM_APP_PKG")

            '' ai chu modify on 30/08/2005
            '' user request to change fixed role from Admin to Super Admin
            'If dv("UGM_FIXED_ROLE") = "Admin" Then
            '    e.Item.Cells(4).Text = "Super Admin"
            'Else
            e.Item.Cells(4).Text = e.Item.Cells(4).Text.Substring(0, 1).ToUpper & e.Item.Cells(4).Text.Substring(1, e.Item.Cells(4).Text.Length - 1).ToLower
            'End If

        End If

    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgUsrGrp.SortCommand
        Grid_SortCommand(sender, e)
        dgUsrGrp.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub


    Public Sub dgUsrGrp_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgUsrGrp.PageIndexChanged
        dgUsrGrp.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub New()

    End Sub

    Private Sub cmdModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim strUsrGrpId, strPackage As String
        Dim chkItem As CheckBox


        For Each dgItem In dgUsrGrp.Items
            chkItem = dgItem.FindControl("chkSelection")
            strPackage = dgItem.Cells(3).Text
            If chkItem.Checked Then
                strUsrGrpId = CType(dgItem.FindControl("lnkUsrGrpID"), HyperLink).Text
                'Me.Response.Redirect("UsUsrGrp.aspx?mode=modify&usrgrpid=" & Server.UrlEncode(strUsrGrpId) & "&Package=" & Server.UrlEncode(strPackage))
                Me.Response.Redirect(dDispatcher.direct("Global", "UsUsrGrp.aspx", "mode=modify&usrgrpid=" & Server.UrlEncode(strUsrGrpId) & "&Package=" & Server.UrlEncode(strPackage)))

            End If
        Next
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        'Response.Redirect("UsUsrGrp.aspx?mode=add")
        Response.Redirect(dDispatcher.direct("Global", "UsUsrGrp.aspx", "mode=add"))
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim strUsrGrpId As String
        Dim strAppPkgId As String
        Dim chkItem As CheckBox
        Dim objUsrGrp As New UserGroups
        Dim blnDeleteFlag As Boolean = False

        For Each dgItem In dgUsrGrp.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                strUsrGrpId = CType(dgItem.FindControl("lnkUsrGrpID"), HyperLink).Text
                strAppPkgId = CType(dgItem.FindControl("lblAppPackage"), Label).Text
                If objUsrGrp.DelUserGroup(strUsrGrpId, strAppPkgId) = False Then
                    Common.NetMsgbox(Me, objUsrGrp.Message, MsgBoxStyle.Information)
                    Exit For
                End If
                blnDeleteFlag = True
            End If
        Next
        If blnDeleteFlag = True Then Common.NetMsgbox(Me, objUsrGrp.Message, MsgBoxStyle.Information)

        viewstate("action") = "del"
        Bindgrid()
        objUsrGrp = Nothing
    End Sub
End Class
