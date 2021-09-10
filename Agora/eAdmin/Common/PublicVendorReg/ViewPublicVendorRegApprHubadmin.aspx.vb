''*************************************************************************************
'Created By:  Ya Li
'Date:  24/05/2005
'Screen:  View Public Vendor Registration Approval
'Purpose:  Allows Hub Admin user view the public vendor registration and search by
'          Company ID, Company Name and also Status. 

'**************************************************************************************

Imports AgoraLegacy
Imports SSO.Component
Public Class ViewPublicVendorRegApprHubadmin
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtComID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtComName As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents chkPending As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkReject As System.Web.UI.WebControls.CheckBox
    Protected WithEvents dtgVendorRegAppr As System.Web.UI.WebControls.DataGrid
    Protected WithEvents chkApprove As System.Web.UI.WebControls.CheckBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim objHubAdmin As New  HubAdmin
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgVendorRegAppr)

    End Sub

    Private Function BindGrid(Optional ByVal pSorted As Boolean = False) As String
        Dim ds As DataSet = New DataSet
        Dim record As Integer
        Dim strStatus As String = ""
        Dim intchk As Integer

        If chkPending.Checked = True Then
            strStatus = strStatus & VendorRegApprStatus.Pending
        End If
        If chkApprove.Checked = True Then
            strStatus = strStatus & VendorRegApprStatus.Approved
        End If
        If chkReject.Checked = True Then
            strStatus = strStatus & VendorRegApprStatus.Reject
        End If

        If Len(strStatus) <> 0 Then
            Dim strtemp, strtemp2, strtemp3 As String
            Dim i As Integer
            i = 0
            strtemp3 = strStatus
            For intchk = 0 To Len(strtemp3) - 1
                i = i + 1
                strtemp2 = Mid(strtemp3, i, 1)
                If Len(strStatus) <> (intchk + 1) Then
                    If Len(strStatus) > 1 Then
                        strtemp2 = strtemp2 & ","
                    End If
                End If
                strtemp = strtemp & strtemp2
            Next
            strStatus = "(" & strtemp & ")"
        End If

        ds = objHubAdmin.getVendorRegAppr(txtComID.Text, txtComName.Text, strStatus)
        record = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = viewstate("SortExpression")

        If viewstate("SortAscending") = "no" Then dvViewSample.Sort += " DESC"
        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If viewstate("action") = "del" Then
            If dtgVendorRegAppr.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgVendorRegAppr.PageSize = 0 Then
                dtgVendorRegAppr.CurrentPageIndex = dtgVendorRegAppr.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        If intPageRecordCnt > 0 Then

            ' check when user re-enter search criteria and click on other page without click search button
            If dtgVendorRegAppr.CurrentPageIndex > (dvViewSample.Count \ dtgVendorRegAppr.PageSize) Then
                dtgVendorRegAppr.CurrentPageIndex = IIf((dvViewSample.Count \ dtgVendorRegAppr.PageSize) = 1, 0, (dvViewSample.Count \ dtgVendorRegAppr.PageSize))
            ElseIf dtgVendorRegAppr.CurrentPageIndex = (dvViewSample.Count \ dtgVendorRegAppr.PageSize) Then
                If viewstate("PageCount") = (dvViewSample.Count \ dtgVendorRegAppr.PageSize) Then
                    'user does not re-enter search criteria 
                    dtgVendorRegAppr.CurrentPageIndex = IIf((dvViewSample.Count \ dtgVendorRegAppr.PageSize) = 0, 0, (dvViewSample.Count \ dtgVendorRegAppr.PageSize) - 1)
                Else
                    If (dvViewSample.Count Mod dtgVendorRegAppr.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dtgVendorRegAppr.CurrentPageIndex = IIf((dvViewSample.Count \ dtgVendorRegAppr.PageSize) = 1, 0, (dvViewSample.Count \ dtgVendorRegAppr.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        dtgVendorRegAppr.CurrentPageIndex = (dvViewSample.Count \ dtgVendorRegAppr.PageSize)
                    End If
                End If

            End If
            '--------------------------------

            dtgVendorRegAppr.DataSource = dvViewSample
            dtgVendorRegAppr.DataBind()

        Else
            dtgVendorRegAppr.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)

        End If

        ' add for above checking
        viewstate("PageCount") = dtgVendorRegAppr.PageCount

    End Function

    Public Sub OnPageIndexChanged_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        sender.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        BindGrid(True)
    End Sub

    Private Sub dtgPublishedRFP_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgVendorRegAppr.ItemCreated
        Grid_ItemCreated(dtgVendorRegAppr, e)
    End Sub

    Private Sub dtgVendorRegAppr_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgVendorRegAppr.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Dim dtgItem As DataGridItem
            Dim i As Integer

            Dim ComID As String = e.Item.Cells(0).Text
            Dim strStatus As String = e.Item.Cells(3).Text

            'e.Item.Cells(0).Text = "<a href=""PublicVendorRegAppr.aspx?ComID=" & ComID & " "" >" & Common.parseNull(dv("CM_COY_ID")) & "</a>"
            If strStatus = "Pending" Then
                'e.Item.Cells(0).Text = "<a href=""../Companies/coCompanyDetail.aspx?mode=regappr&ComID=" & ComID & " "" >" & Common.parseNull(dv("CM_COY_ID")) & "</a>"
                e.Item.Cells(0).Text = "<a href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "mode=regappr&ComID=" & ComID & "") & " >" & Common.parseNull(dv("CM_COY_ID")) & "</a>"
            Else
                'e.Item.Cells(0).Text = "<a href=""../Companies/coCompanyDetail.aspx?ComID=" & ComID & "&status=" & strStatus & " "" >" & Common.parseNull(dv("CM_COY_ID")) & "</a>"
                e.Item.Cells(0).Text = "<a href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "ComID=" & ComID & "&status=" & strStatus & "") & " >" & Common.parseNull(dv("CM_COY_ID")) & "</a>"

            End If
        End If
    End Sub

    Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        txtComID.Text = ""
        txtComName.Text = ""
        chkPending.Checked = False
        chkReject.Checked = False

        BindGrid()
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        BindGrid()
    End Sub

    Private Sub chkApprove_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApprove.CheckedChanged

    End Sub
End Class
