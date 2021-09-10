Imports AgoraLegacy

Partial Class HolidayCalendar
    Inherits AgoraLegacy.AppBaseClass
    Dim ds As DataSet
    Dim lngRecNO As Long
    Dim dDispatcher As New AgoraLegacy.dispatcher
    'Dim msg As New Functions.Message
    Dim objGlobal As New AppGlobals


    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dgHoliday)

        If Not IsPostBack Then

            txtYear.Text = Now.ToString("yyyy")
            PopulateCodeTable()
            Bindgrid()
        End If

        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")


    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim strUserID As String
        Dim strUserName As String

        'strUserID = Me.txtUserID.Text
        'strUserName = Me.txtUserName.Text

        Dim objIPP As New IPP

        '//Retrieve Data from Database
        Dim ds As DataSet = New DataSet
        ds = objIPP.GetHoliday(txtYear.Text, cboCountry.SelectedValue, cboState.SelectedValue)

        '//for sorting asc or desc
        Dim dvViewDept As DataView
        dvViewDept = ds.Tables(0).DefaultView
        'dvViewSample(0)(0)
        'If pSorted Then
        dvViewDept.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewDept.Sort += " DESC"
        'End If

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If ViewState("action") = "del" Then
            If dgHoliday.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dgHoliday.PageSize = 0 Then
                dgHoliday.CurrentPageIndex = dgHoliday.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        '//bind datagrid

        '//datagrid.pageCount only got value after databind

        If intPageRecordCnt > 0 Then
            ' check when user re-enter search criteria and click on other page without click search button
            If dgHoliday.CurrentPageIndex > (dvViewDept.Count \ dgHoliday.PageSize) Then
                dgHoliday.CurrentPageIndex = IIf((dvViewDept.Count \ dgHoliday.PageSize) = 1, 0, (dvViewDept.Count \ dgHoliday.PageSize))
            ElseIf dgHoliday.CurrentPageIndex = (dvViewDept.Count \ dgHoliday.PageSize) Then
                If ViewState("PageCount") = (dvViewDept.Count \ dgHoliday.PageSize) Then
                    'user does not re-enter search criteria 
                    dgHoliday.CurrentPageIndex = IIf((dvViewDept.Count \ dgHoliday.PageSize) = 0, 0, (dvViewDept.Count \ dgHoliday.PageSize) - 1)
                Else
                    If (dvViewDept.Count Mod dgHoliday.PageSize) = 0 Then
                        ' total record = 10, 20, ...
                        dgHoliday.CurrentPageIndex = IIf((dvViewDept.Count \ dgHoliday.PageSize) = 1, 0, (dvViewDept.Count \ dgHoliday.PageSize))
                    Else
                        ' total record = 11, 12, ...
                        dgHoliday.CurrentPageIndex = (dvViewDept.Count \ dgHoliday.PageSize)
                    End If
                End If
            End If
            '--------------------------------
            'intTotPage = dtgDept.PageCount
            cmdDelete.Enabled = True
            cmdModify.Enabled = True


            dgHoliday.DataSource = dvViewDept
            dgHoliday.DataBind()
        Else

            cmdDelete.Enabled = False
            cmdModify.Enabled = False

            dgHoliday.DataBind()
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("1004"), MsgBoxStyle.Information)

            'intTotPage = 0
        End If
        ' add for above checking
        ViewState("PageCount") = dgHoliday.PageCount
        'ShowStats()
    End Function
    Private Sub PopulateCodeTable()
        objGlobal.FillCodeTable(cboCountry, CodeTable.Country)
        objGlobal.FillState(cboState, cboCountry.SelectedValue)
        cboState.Items(0).Text = "All States"
    End Sub
    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        If txtYear.Text = "" Then
            vldsum.InnerHtml = "<li>Year " & objGlobal.GetErrorMessage("00001") & "</li>"
            Exit Sub
        Else
            vldsum.InnerHtml = ""
        End If
        dgHoliday.CurrentPageIndex = 0
        Bindgrid()
    End Sub



    Protected Sub dgHoliday_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgHoliday.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
        Grid_ItemCreated(dgHoliday, e)
    End Sub

    Protected Sub dgHoliday_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgHoliday.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            e.Item.Cells(1).Text = CDate(e.Item.Cells(1).Text).ToString("dd/MM/yyyy")

        End If
    End Sub

    Protected Sub dgHoliday_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgHoliday.PageIndexChanged
        dgHoliday.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgHoliday.SortCommand
        Grid_SortCommand(sender, e)
        dgHoliday.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim objUser As New Users
        Dim strscript As New System.Text.StringBuilder

        strscript.Append("<script language=""javascript"">")
        strscript.Append("PopWindow(""" & dDispatcher.direct("Misc", "HolidayCalendarAdd.aspx", "pageid=" & ViewState("pageid") & "&mode=add&year=" & txtYear.Text & "&country=" & cboCountry.SelectedValue & "&state=" & cboState.SelectedValue & """);"))
        strscript.Append("</script>")
        RegisterStartupScript("script4", strscript.ToString())
        'If objUser.IsExceedUserLimit(Session("CompanyIdToken")) = False Then
        'If objUser.IsExceedAdminLimit(Session("CompanyIdToken")) = False Then
        '    'Me.Response.Redirect("UsUserHub2.aspx?pageid=" & ViewState("pageid") & "&mode=add")
        '    Me.Response.Redirect(dDispatcher.direct("Misc", "HolidayCalendarAdd.aspx", "pageid=" & ViewState("pageid") & "&mode=add"))
        'Else
        '    Common.NetMsgbox(Me, objUser.Message, MsgBoxStyle.Information)
        'End If

        'Bindgrid()

    End Sub

    Private Sub cmdModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim dgItem As DataGridItem
        Dim strHolidayIndex As String
        Dim chkItem As CheckBox
        Dim strscript As New System.Text.StringBuilder

        For Each dgItem In dgHoliday.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then

                strHolidayIndex = dgItem.Cells(4).Text
                'Me.Response.Redirect("UsUserHub2.aspx?mode=modify&userid=" & strUserid)
                'Me.Response.Redirect(dDispatcher.direct("Misc", "HolidayCalendarAdd.aspx", "mode=modify&userid=" & strUserid))
                strscript.Append("<script language=""javascript"">")
                strscript.Append("PopWindow(""" & dDispatcher.direct("Misc", "HolidayCalendarAdd.aspx", "pageid=" & ViewState("pageid") & "&mode=modify&lineno=1&year=" & txtYear.Text & "&index=" & strHolidayIndex & "&country=" & cboCountry.SelectedValue & "&state=" & cboState.SelectedValue & """);"))
                strscript.Append("</script>")
                RegisterStartupScript("script5", strscript.ToString())
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
    Public Function FillCheckBoxGrid(ByVal pInString As String,
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

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim dgItem As DataGridItem
        Dim strHolidayIndex As String
        Dim chkItem As CheckBox
        Dim objIPP As New IPP

        For Each dgItem In dgHoliday.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                strHolidayIndex = dgItem.Cells(4).Text

                If objIPP.DelHoliday(strHolidayIndex) = False Then
                    Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00007"), MsgBoxStyle.Information)
                    Exit For
                Else
                    Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00004"), MsgBoxStyle.Information)
                End If
            End If
        Next

        Bindgrid()

    End Sub

    Protected Sub cboCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCountry.SelectedIndexChanged
        objGlobal.FillState(cboState, cboCountry.SelectedItem.Value)

        If cboState.Items.Count > 1 Then
            cboState.Items(0).Text = "All States"
        End If

    End Sub

    Protected Sub cmdClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClear.Click
        objGlobal.FillCodeTable(cboCountry, CodeTable.Country)
        objGlobal.FillState(cboState, "MY")
        cboState.Items(0).Text = "All States"
        txtYear.Text = Now.ToString("yyyy")
    End Sub

    Protected Sub btnhidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhidden.Click
        Bindgrid()

    End Sub
End Class