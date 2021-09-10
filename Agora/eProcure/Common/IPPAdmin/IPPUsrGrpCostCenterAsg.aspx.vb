
Imports AgoraLegacy
Imports eProcure.Component
Imports eProcurement
Imports System.drawing

Public Class IPPUsrGrpCostCenterAsg
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSearch As System.Web.UI.WebControls.TextBox
    Protected WithEvents imbSearch As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmbGroup As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents totrows As System.Web.UI.WebControls.Label
    Protected WithEvents lblAction As System.Web.UI.WebControls.Label

    Private ds As DataSet
    Dim dvwPackage As DataView
    Dim dDispatcher As New AgoraLegacy.dispatcher


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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdAdd.Enabled = False
 
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmdAdd)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
  
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmdDelete)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        '//additional checking
        'If intPageRecordCnt > 0 Then
        '    cmdDelete.Enabled = blnCanDelete         
        'Else
        '    cmdDelete.Enabled = False
        'End If
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        SetGridProperty(dgUser)

        If Not Page.IsPostBack Then
            'Dim objUser As New Users
            'If objUser.getCompanyType() = "VENDOR" Then
            '    Me.dgUser.Columns(3).Visible = False
            '    Me.dgUser.Columns(4).Visible = False
            '    ViewState("Side") = "VENDOR"
            'Else
            '    ViewState("Side") = "BUYER"
            '    Dim intDept As Integer
            '    Dim objDBAccess As New EAD.DBCom
            '    intDept = objDBAccess.GetCount("company_dept_mstr", " where cdm_coy_id = '" & Session("CompanyID") & "' and cdm_deleted = 'N'")
            '    If intDept = 0 Then Me.dgUser.Columns(4).Visible = False

            '    Me.lblAction.Text = "<b>=></b> Step 1: Create, modify or delete User Group.<br />Step 2: Assign Branch to the User Group.<br>Step 3: Assigned Cost Center to User Group.<br>Step 4: Assign User to the User Group.<br>Step 5: Assign Commodity Type to selected User Account."

            'End If

            GenerateTab()
            PopulateTypeAhead()
            dgUser.CurrentPageIndex = 0
            Bindgrid()
        End If

        'ViewState("pageid") = Request.Params("pageid")
        cmdDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim strUserGroup As String
        Dim strUserName As String

        strUserGroup = Me.txtUserGroup.Text
        'strUserName = Me.txtUserName.Text

        Dim objIPP As New IPP
        Dim objUserRoles As New UserRoles
        Dim ds As DataSet = New DataSet
        Dim strUserRoles As String

        ds = objIPP.GetIPPUserGroupCC(Common.Parse(Common.parseNull(hid1.Value)), Common.Parse(Common.parseNull(hid2.Value)))

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured

        If dgUser.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dgUser.PageSize = 0 Then
            dgUser.CurrentPageIndex = dgUser.CurrentPageIndex - 1
            ViewState("action") = ""
        End If


        intPageRecordCnt = ds.Tables(0).Rows.Count


        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dgUser, dvViewSample)
            cmdDelete.Enabled = True
            dgUser.DataSource = dvViewSample
            dgUser.DataBind()
        Else

            cmdDelete.Enabled = False
            Common.NetMsgbox(Me, MsgNoRecord)
            dgUser.DataBind()
        End If

        ViewState("PageCount") = dgUser.PageCount
    End Function

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        dgUser.CurrentPageIndex = 0
        If txtUserGroup.Text = "" Then
            hid1.Value = ""
        End If

        If txtCC.Text = "" Then
            hid2.Value = ""
        End If

        Bindgrid()
    End Sub

    Private Sub dgUser_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgUser.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dgUser, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")

          
        End If
    End Sub

    Private Sub dgUser_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgUser.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            'to add JavaScript to Check Box
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
            Dim ds As New DataSet
            Dim objIPP As New IPP
            Dim intLoop As Integer
            Dim lblUserGroup As Label = e.Item.Cells(3).FindControl("lblUserGroup")

            Dim lblCCDesc As Label = e.Item.Cells(2).FindControl("lblCCDesc")

            Dim val As String = e.Item.Cells(4).Text

            ds = objIPP.GetIPPUserGroupCC2(Common.Parse(Common.parseNull(hid1.Value)), Common.Parse(Common.parseNull(val)))

            If Not ds Is Nothing Then
                For intLoop = 0 To ds.Tables(0).Rows.Count - 1
                    If intLoop = 0 Then
                        lblUserGroup.Text &= ds.Tables(0).Rows(intLoop).Item("IUM_GRP_NAME")
                        'lblCCDesc.Text &= ds.Tables(0).Rows(intLoop).Item("cc_cc_desc")
                    Else
                        lblUserGroup.Text &= "<br>"
                        lblUserGroup.Text &= ds.Tables(0).Rows(intLoop).Item("IUM_GRP_NAME")
                        'lblCCDesc.Text &= "<br>"
                        'lblCCDesc.Text &= ds.Tables(0).Rows(intLoop).Item("cc_cc_desc")
                    End If

                Next

            End If

        End If
    End Sub

    
    Sub dgUser_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dgUser.PageIndexChanged
        dgUser.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        'Dim objUser As New Users
        If txtUserGroup.Text = "" Then
       
            Common.NetMsgbox(Me, "User Group is required.", MsgBoxStyle.Information)
            'Exit Sub
        Else
            'objUser = Nothing
            'Me.lbl_add_mod.Text = "add"
            Dim strscript As New System.Text.StringBuilder
            Dim objipp As New IPPMain
            Dim strFileName As String
            strscript.Append("<script language=""javascript"">")
            'strscript.Append("PopWindow(""" & dDispatcher.direct("IPP", "IPPEntryPop.aspx", "docno=" & txtDocNo.Text & "&doctype=" & ddlDocType.SelectedItem.Text & "&vencomp=" & Server.UrlEncode(txtVendor.Text)) & "&olddocno=" & hid4.Value & """);")
            strFileName = dDispatcher.direct("IPPAdmin", "IPPUsrGrpCostCenterAdd.aspx", "pageid=" & strPageId & "&grpno=" & hid1.Value)
            strFileName = Server.UrlEncode(strFileName)
            strscript.Append("ShowDialog(""" & dDispatcher.direct("IPPAdmin", "Dialog.aspx", "page=" & strFileName) & """,'520px');")
            strscript.Append("document.getElementById('btnhidden').click();")
            strscript.Append("</script>")
            RegisterStartupScript("script411", strscript.ToString())
        End If
        Bindgrid()

    End Sub

    Public Function FillCheckBoxGrid(ByVal pInString As String, _
                                     ByRef pDataGrid As DataGrid) As Boolean

        Dim lngLoop As Long
        Dim ary() As String = Split(pInString, ",")
        Dim varItem As DataGridItem

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
        Dim chkItem As CheckBox
        Dim objIPP As New IPP

        For Each dgItem In dgUser.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then

                If objIPP.DelIPPUserGroupCostCenter(hid1.Value, dgItem.Cells(1).Text) = False Then
                    Exit For
                End If

            End If

        Next
        Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
        ViewState("action") = "del"
        Bindgrid()
        objIPP = Nothing
    End Sub

    Private Sub dgUser_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgUser.SortCommand
        Grid_SortCommand(source, e)
        dgUser.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If


        'Zulham 08/05/2018 - PAMB
        If Session("CompanyID").ToString.ToUpper = "PAMB" Then
            Session("w_SearchUser_tabs") = "<div class=""t_entity""><ul>" &
             "<li><div class=""space""></div></li>" &
             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPPAdmin", "IPPUserGroup.aspx", "pageid=" & strPageId) & """><span>User Group</span></a></li>" &
             "<li><div class=""space""></div></li>" &
             "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPPAdmin", "IPPUsrGrpCostCenterAsg.aspx", "pageid=" & strPageId) & """><span>Cost Center Assignment</span></a></li>" &
             "<li><div class=""space""></div></li>" &
             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPPAdmin", "IPPUsrGrpUserAsg.aspx", "pageid=" & strPageId) & """><span>User Assignment</span></a></li>" &
             "<li><div class=""space""></div></li>" &
             "</ul><div></div></div>"
        Else
            Session("w_SearchUser_tabs") = "<div class=""t_entity""><ul>" &
             "<li><div class=""space""></div></li>" &
             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPPAdmin", "IPPUserGroup.aspx", "pageid=" & strPageId) & """><span>User Group</span></a></li>" &
             "<li><div class=""space""></div></li>" &
             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPPAdmin", "IPPUsrGrpBranchAsg.aspx", "pageid=" & strPageId) & """><span>Branch Assignment</span></a></li>" &
             "<li><div class=""space""></div></li>" &
             "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("IPPAdmin", "IPPUsrGrpCostCenterAsg.aspx", "pageid=" & strPageId) & """><span>Cost Center Assignment</span></a></li>" &
             "<li><div class=""space""></div></li>" &
             "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("IPPAdmin", "IPPUsrGrpUserAsg.aspx", "pageid=" & strPageId) & """><span>User Assignment</span></a></li>" &
             "<li><div class=""space""></div></li>" &
             "</ul><div></div></div>"
        End If

    End Sub
    Sub PopulateTypeAhead()
        Dim typeahead As String
        Dim i, count As Integer
        Dim content, content2 As String
        Dim strCompID As String
        Dim typeahead1 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=UserGroup&frm=IPPUsrGrpCCAsg")
        Dim typeahead2 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=CostCentre2&frm=IPPUsrGrpCCAsg")


        content &= "$(""#txtUserGroup"").autocomplete(""" & typeahead1 & """, {" & vbCrLf & _
        "width: 200," & vbCrLf & _
        "scroll: true," & vbCrLf & _
        "selectFirst: false" & vbCrLf & _
         "}).result(function(event, data, formatted) {" & vbCrLf & _
            "if (data)" & vbCrLf & _
            "$(""#hid1"").val(data[1]);" & vbCrLf & _
             "});" & vbCrLf & _
             "$(""#txtUserGroup"").blur(function() {" & vbCrLf & _
        "var hidven = document.getElementById(""hid1"").value;" & vbCrLf & _
        "if(hidven == """")" & vbCrLf & _
        "{" & vbCrLf & _
        "$(""#txtUserGroup"").val("""");" & vbCrLf & _
         "}" & vbCrLf & _
        "});" & vbCrLf & _
        "$(""#txtCC"").autocomplete(""" & typeahead2 & """, {" & vbCrLf & _
        "width: 200," & vbCrLf & _
        "scroll: true," & vbCrLf & _
        "selectFirst: false" & vbCrLf & _
        "}).result(function(event, data, formatted) {" & vbCrLf & _
            "if (data)" & vbCrLf & _
            "$(""#hid2"").val(data[1]);" & vbCrLf & _
             "});" & vbCrLf & _
        "$(""#txtCC"").blur(function() {" & vbCrLf & _
        "var hidCC = document.getElementById(""hid2"").value;" & vbCrLf & _
        "if(hidCC == """")" & vbCrLf & _
        "{" & vbCrLf & _
        "$(""#txtCC"").val("""");" & vbCrLf & _
         "}" & vbCrLf & _
        "});" & vbCrLf
       
        typeahead = "<script language=""javascript"">" & vbCrLf & _
      "<!--" & vbCrLf & _
        "$(document).ready(function(){" & vbCrLf & _
        content & vbCrLf & _
        "});" & vbCrLf & _
        "-->" & vbCrLf & _
        "</script>"


        Session("typeaheadIPPCCAsg") = typeahead
    End Sub

    Private Sub btnhidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhidden.Click
        Bindgrid()
    End Sub
End Class


