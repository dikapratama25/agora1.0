Imports AgoraLegacy
Imports SSO.Component
Public Class coAllCompany
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents txtCompID As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents lbl_result As System.Web.UI.WebControls.Label
    Protected WithEvents cmdAdd As System.Web.UI.WebControls.Button
    Protected WithEvents cmdModify As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents txtCompName As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdActivate As System.Web.UI.WebControls.Button
    Protected WithEvents dgComp As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdDeactivate As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Dim objDb As  EAD.DBCom
    'Private lngRecNO As Long


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
        'SearchID.Value = ""
        'SearchKey.Value = ""
    End Sub

#End Region



    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(dgComp)

        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        cmdActivate.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','activate');")
        cmdDeactivate.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','deactive');")



    End Sub


    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim strCompID As String
        Dim strCompName As String

        strCompID = Me.txtCompID.Text
        strCompName = Me.txtCompName.Text

        Dim objComp As New Companies

        '//Retrieve Data from Database
        Dim ds As DataSet = New DataSet
        ds = objComp.SearchCompany(strCompID, strCompName)

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
            If dgComp.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dgComp.PageSize = 0 Then
                dgComp.CurrentPageIndex = dgComp.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        If intPageRecordCnt > 0 Then
            ' check when user re-enter search criteria and click on other page without click search button
            If dgComp.CurrentPageIndex > (dvViewDept.Count \ dgComp.PageSize) Then
                dgComp.CurrentPageIndex = IIf((dvViewDept.Count \ dgComp.PageSize) = 1, 0, (dvViewDept.Count \ dgComp.PageSize))
            ElseIf dgComp.CurrentPageIndex = (dvViewDept.Count \ dgComp.PageSize) Then
                If viewstate("PageCount") = (dvViewDept.Count \ dgComp.PageSize) Then
                    'user does not re-enter search criteria 
                    dgComp.CurrentPageIndex = IIf((dvViewDept.Count \ dgComp.PageSize) = 0, 0, (dvViewDept.Count \ dgComp.PageSize) - 1)
                Else
                    If (dvViewDept.Count Mod dgComp.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dgComp.CurrentPageIndex = IIf((dvViewDept.Count \ dgComp.PageSize) = 1, 0, (dvViewDept.Count \ dgComp.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        dgComp.CurrentPageIndex = (dvViewDept.Count \ dgComp.PageSize)
                    End If
                End If
            End If
            '--------------------------------
            cmdDelete.Enabled = True
            cmdActivate.Enabled = True
            cmdDeactivate.Enabled = True
            '//mean Enable, can't use button.Enabled because this is a HTML button
            cmdReset.Disabled = False
            dgComp.DataSource = dvViewDept
            dgComp.DataBind()
        Else
            cmdActivate.Enabled = False
            cmdDeactivate.Enabled = False
            cmdDelete.Enabled = False
            cmdReset.Disabled = True
            dgComp.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ' add for above checking
        viewstate("PageCount") = dgComp.PageCount
        'ShowStats()
    End Function

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dgComp.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub dgComp_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgComp.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dgComp, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgComp.SortCommand
        Grid_SortCommand(sender, e)
        dgComp.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub


    'Public Sub dgComp_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
    '    dgComp.CurrentPageIndex = e.NewPageIndex
    '    Bindgrid()
    'End Sub
    Public Sub dgComp_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgComp.PageIndexChanged
        dgComp.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub New()

    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Me.Response.Redirect(dDispatcher.direct("Users", "usUser.aspx", "mode=add"))
    End Sub

    Private Sub cmdModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim strCompID As String
        Dim chkItem As CheckBox
        For Each dgItem In dgComp.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                strCompID = CType(dgItem.FindControl("lnkUserID"), HyperLink).Text
                Me.Response.Redirect(dDispatcher.direct("Users", "usUser.aspx", "mode=modify&userid=" & strCompID))
            End If
        Next
    End Sub

    'Name       : FillCheckBoxGrid
    'Author     : Kaithim
    'Descption  : fill the datagrid check box with a string 
    'Remark     :
    'ReturnValue: 
    'LastUpadte : 19 Sep 2002
    'Version    : 1.00
    Public Function FillCheckBoxGrid(ByVal pInString As String, _
                                     ByRef pDataGrid As DataGrid) As Boolean

        Dim lngLoop As Long
        Dim ary() As String = Split(pInString, ",")
        Dim varItem As DataGridItem
        'For lngLoop = 0 To UBound(ary)
        '    For Each varItem In pDataGrid.Items
        '        If pDataGrid.DataKeys(varItem.ItemIndex).ToString = ary(lngLoop) Then
        '            Dim chk As CheckBox = varItem.Cells(0).Controls(0)
        '            chk.Checked = True
        '        End If
        '    Next
        'Next
        For lngLoop = 0 To UBound(ary)
            For Each varItem In pDataGrid.Items
                If pDataGrid.DataKeys(varItem.ItemIndex).ToString = ary(lngLoop) Then
                    Dim chk As CheckBox = varItem.Cells(0).FindControl("select")
                    chk.Checked = True
                End If
            Next
        Next
    End Function


    Private Sub cmdActivate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdActivate.Click

        Dim dgItem As DataGridItem
        Dim strCompid As String
        Dim chkItem As CheckBox
        Dim objComp As New Companies

        Try
            For Each dgItem In dgComp.Items
                chkItem = dgItem.FindControl("chkSelection")
                If chkItem.Checked Then
                    strCompid = dgItem.Cells(1).Text

                    If objComp.ActivateCompany(strCompid, True) Then
                        If UCase(dgItem.Cells(3).Text) <> "BUYER" Then
                            Dim objEmail As New Email
                            objEmail.sendNotification(EmailType.CompanyActivated, "", "", strCompid, "", "", "V")
                            objEmail.sendNotification(EmailType.CompanyActivated, "", "", strCompid, "", "", "B")
                            objEmail = Nothing
                        End If
                    End If
                End If
            Next
            Common.NetMsgbox(Me, "Company activated.", MsgBoxStyle.Information)
            Bindgrid()
        Catch errExp As CustomException
            Common.TrwExp(errExp)
        Catch errExp1 As Exception
            Common.TrwExp(errExp1)
        End Try
    End Sub

    Private Sub cmdDeactivate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDeactivate.Click
        Dim dgItem As DataGridItem
        Dim strCompid As String
        Dim chkItem As CheckBox
        Dim objComp As New Companies

        Try
            For Each dgItem In dgComp.Items
                chkItem = dgItem.FindControl("chkSelection")
                If chkItem.Checked Then
                    strCompid = dgItem.Cells(1).Text

                    ' ai chu add on 09/12/2005
                    ' SR AS0048 - Vendor company deactivation process
                    '           - remove the items inside Sopping cart to prevent item being created into PR
                    If objComp.ActivateCompany(strCompid, False) Then
                        Dim objSC As New ShoppingCart
                        objSC.deleteDeactivatedCompanyShoppingCartItem(strCompid)

                        If UCase(dgItem.Cells(3).Text) <> "BUYER" Then
                            Dim objEmail As New Email
                            objEmail.sendNotification(EmailType.CompanyDeactivated, "", "", strCompid, "", "", "V")
                            objEmail.sendNotification(EmailType.CompanyDeactivated, "", "", strCompid, "", "", "B")
                            objEmail = Nothing
                        End If

                        objSC = Nothing

                    End If
                End If
            Next
            Common.NetMsgbox(Me, "Company deactived.", MsgBoxStyle.Information)
            Bindgrid()
        Catch errExp As CustomException
            Common.TrwExp(errExp)
        Catch errExp1 As Exception
            Common.TrwExp(errExp1)
        End Try
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim strCompid As String
        Dim chkItem As CheckBox
        Dim objComp As New Companies

        Try
            For Each dgItem In dgComp.Items
                chkItem = dgItem.FindControl("chkSelection")
                If chkItem.Checked Then
                    strCompid = dgItem.Cells(1).Text
                    If objComp.DeleteCompany(strCompid) = False Then
                        Exit For
                    End If
                End If
            Next
            Common.NetMsgbox(Me, objComp.Message, MsgBoxStyle.Information)

            Dim strscript As New System.Text.StringBuilder
            strscript.Append("<script language=""javascript"">")
            'strscript.Append("parent.frames['menu'].location=""../menu.aspx?mode=show"";")
            'strscript.Append("parent.frames['menu'].location=""../menu.aspx"";")
            strscript.Append("parent.frames['menu'].location=""" & dDispatcher.direct("Initial", "menu.aspx") & """;")
            strscript.Append("</script>")
            RegisterStartupScript("script3", strscript.ToString())


            viewstate("action") = "del"
            Bindgrid()
        Catch errExp As CustomException
            Common.TrwExp(errExp)
        Catch errExp1 As Exception
            Common.TrwExp(errExp1)
        End Try
    End Sub

    Private Sub dgComp_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgComp.ItemDataBound


        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim chk As CheckBox
            chk = e.Item.FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

        End If
      
    End Sub
End Class


