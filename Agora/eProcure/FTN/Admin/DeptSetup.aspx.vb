Imports AgoraLegacy
Imports eProcure.Component
Public Class DeptSetupFTN
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents cmdClear As System.Web.UI.WebControls.Button
    Protected WithEvents lblCurrentIndex As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageCount As System.Web.UI.WebControls.Label    
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
        If intPageRecordCnt > 0 Then
            cmdDelete.Enabled = blnCanDelete
            cmdModify.Enabled = blnCanUpdate
            '//mean Enable, can't use button.Enabled because this is a HTML button
            cmdReset.Disabled = False
        Else
            cmdDelete.Enabled = False
            cmdModify.Enabled = False
            cmdReset.Disabled = True
        End If
        alButtonList.Clear()
    End Sub
    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        '//for button access right

        MyBase.Page_Load(sender, e)
        ViewState("Side") = Request.Params("side")
        SetGridProperty(dtgDept)
        GenerateTab()
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        cmdModify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")

    End Sub

    Sub dtgDept_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgDept.PageIndexChanged
        dtgDept.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgDept.SortCommand
        Grid_SortCommand(sender, e)
        dtgDept.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim strDeptCode As String
        Dim strDeptName As String

        strDeptCode = Me.txtDeptCode.Text
        strDeptName = Me.txtDeptName.Text

        Dim objAdmin As New Admin

        '//Retrieve Data from Database
        Dim ds As DataSet = New DataSet
        ds = objAdmin.searchdept(strDeptCode, strDeptName)

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
            If dtgDept.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgDept.PageSize = 0 Then
                dtgDept.CurrentPageIndex = dtgDept.CurrentPageIndex - 1
                viewstate("action") = ""
            End If
        End If

        intPageRecordCnt = ds.Tables(0).Rows.Count
        '//bind datagrid

        '//datagrid.pageCount only got value after databind
        If intPageRecordCnt > 0 Then

            resetDatagridPageIndex(dtgDept, dvViewDept)

            dtgDept.DataSource = dvViewDept
            dtgDept.DataBind()
        Else
            dtgDept.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
            'intTotPage = 0
        End If

        ' add for above checking
        viewstate("PageCount") = dtgDept.PageCount

    End Function

    Private Sub dtgDept_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDept.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgDept, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    'Sub ShowStats()
    '    lblCurrentIndex.Text = intTotRecord & " record(s) found  ,"
    '    lblPageCount.Text = intTotPage & " page(s) found"
    'End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim grdItem As DataGridItem
        'Me.Response.Redirect("adddept.aspx?mode=add & DIndex=" & dtgDept.DataKeys.Item(grdItem.ItemIndex) & " &pageid=" & strPageId)
        Me.Response.Redirect(dDispatcher.direct("Admin", "adddept.aspx", "side=" & Request.QueryString("side") & "&mode=add&pageid=" & strPageId))

    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dtgDept.CurrentPageIndex = 0
        ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "CDM_DEPT_CODE"
        Bindgrid()
    End Sub

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim grdItem As DataGridItem
        '//Loop datagrid item
        For Each grdItem In dtgDept.Items
            Response.Write(grdItem.Cells(1).Text & "<br>")
            Dim chkSelection As CheckBox = grdItem.Cells(0).FindControl("chkSelection")
            If chkSelection.Checked Then
                'Me.Response.Redirect(dDispatcher.direct("Admin", "adddept.aspx", "side=" & Request.QueryString("side") & "&mode=update&dindex=" & dtgDept.DataKeys.Item(grdItem.ItemIndex) & "&pageid=" & strPageId & "&deptcode=" & Server.UrlEncode(grdItem.Cells(2).Text) & "&deptname=" & Server.UrlEncode(grdItem.Cells(3).Text) & "&appgrpindex=" & Server.UrlEncode(grdItem.Cells(4).Text)))
                Me.Response.Redirect(dDispatcher.direct("Admin", "adddept.aspx", "side=" & Request.QueryString("side") & "&mode=update&dindex=" & dtgDept.DataKeys.Item(grdItem.ItemIndex) & "&pageid=" & strPageId & "&deptcode=" & Server.UrlEncode(grdItem.Cells(2).Text) & "&deptname=" & Server.UrlEncode(grdItem.Cells(3).Text) & "&appgrpindex=" & Server.UrlEncode(grdItem.Cells(4).Text)))
            End If
        Next
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim grdItem As DataGridItem
        Dim intMsg As Integer
        Dim dtNewDept As New DataTable
        Dim drDept As DataRow
        dtNewDept.Columns.Add("Dept_CODE", Type.GetType("System.String")) '//product code
        dtNewDept.Columns.Add("Dept_Index", Type.GetType("System.Int32")) '//supplier id

        For Each grdItem In dtgDept.Items
            Dim chkSelection As CheckBox = grdItem.Cells(0).FindControl("chkSelection")

            If chkSelection.Checked Then
                Dim deptcode As String = grdItem.Cells(2).Text
                drDept = dtNewDept.NewRow
                drDept("Dept_Code") = grdItem.Cells(2).Text
                drDept("Dept_Index") = dtgDept.DataKeys.Item(grdItem.ItemIndex)
                dtNewDept.Rows.Add(drDept)
            End If
        Next

        Dim objAdmin As New Admin
        intMsg = objAdmin.delCdept(dtNewDept)
        If intMsg = -99 Then
            Common.NetMsgbox(Me, "Deletion is not allow.", MsgBoxStyle.Information)
        ElseIf intMsg = WheelMsgNum.Delete Then
            Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        ElseIf intMsg = WheelMsgNum.NotDelete Then
            Common.NetMsgbox(Me, MsgRecordNotDelete, MsgBoxStyle.Information)
        End If
        viewstate("action") = "del"
        Bindgrid()
    End Sub
    'Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
    '    Dim grdItem As DataGridItem

    '    For Each grdItem In dtgDept.Items
    '        Dim chkSelection As CheckBox = grdItem.Cells(0).FindControl("chkSelection")

    '        If chkSelection.Checked Then
    '            Dim objAdmin As New Admin
    '            Dim deptcode As String = grdItem.Cells(2).Text
    '            objAdmin.deldept(deptcode)
    '        End If
    '    Next
    '    'Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
    '    viewstate("action") = "del"
    '    Bindgrid()
    'End Sub

    Private Sub dtgDept_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgDept.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            '//to add JavaScript to Check Box
            Dim chk As CheckBox
            Dim arrVal As String = ""
            Dim objDb As New EAD.DBCom
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            '//to dynamic build hyoerlink
            Dim lnkDeptCode As HyperLink
            lnkDeptCode = e.Item.FindControl("lnkDeptCode")
            Dim aa As String
            'Michelle (23/9/2010) - To cater for MYSQL
            'lnkDeptCode.NavigateUrl = "addDept.aspx?mode=update&pageid=" & strPageId & "&deptcode=" & _
            'dv("CDM_DEPT_CODE") & "&deptname=" & dv("CDM_DEPT_NAME") & " &appgrpindex=" & Server.UrlEncode(dv("CDM_APPROVAL_GRP_INDEX"))
            'lnkDeptCode.NavigateUrl = "" & dDispatcher.direct("Admin", "addDept.aspx", "side=" & Request.QueryString("side") & "&mode=update&pageid=" & strPageId & "&deptcode=" & dv("CDM_DEPT_CODE") & "&deptname=" & dv("CDM_DEPT_NAME") & "&appgrpindex=" & objDb.ReturnArrayValue(dv("CDM_APPROVAL_GRP_INDEX"), UBound(dv("CDM_APPROVAL_GRP_INDEX"))))
            'Modified by Joon on 13 Sept 2011
            lnkDeptCode.NavigateUrl = "" & dDispatcher.direct("Admin", "addDept.aspx", "side=" & Request.QueryString("side") & "&mode=update&pageid=" & strPageId & "&deptcode=" & dv("CDM_DEPT_CODE") & "&deptname=" & dv("CDM_DEPT_NAME") & "&appgrpindex=" & dv("CDM_APPROVAL_GRP_INDEX"))
            lnkDeptCode.Text = dv("CDM_DEPT_CODE")

            'e.Item.Cells(1).Te = "<a href='addDept.aspx'>" & e.Item.Cells(1).Text & "</a>"
        End If
    End Sub


    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        If ViewState("Side") = "BUYER" Then
            Session("w_DeptSetup_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                                   "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=BUYER&mode=modify" & "&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
                                   "<li><div class=""space""></div></li>" & _
                                   "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "side=BUYER" & "&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
                                   "<li><div class=""space""></div></li>" & _
                                   "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=D&mod=T" & "&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
                                   "<li><div class=""space""></div></li>" & _
                                   "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=B&mod=T" & "&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
                                   "<li><div class=""space""></div></li>" & _
                                   "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=BUYER" & "&pageid=" & strPageId) & """><span>Department</span></a></li>" & _
                                   "<li><div class=""space""></div></li>" & _
                  "</ul><div></div></div>"
        Else
            Session("w_DeptSetup_tabs") = "<div class=""t_entity""><ul>" & _
                                  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=BUYER&mode=modify&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=D&mod=T&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=B&mod=T&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                              "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Department</span></a></li>" & _
                              "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SalesInfo.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Sales Info</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SoftwareApp.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Software</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "QualityStd.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Quality Standards</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                 "</ul><div></div></div>"

        End If

    End Sub

End Class


