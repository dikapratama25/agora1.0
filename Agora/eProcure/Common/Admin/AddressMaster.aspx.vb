Imports AgoraLegacy
Imports eProcure.Component

Public Class AddressMaster
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Protected WithEvents cmd_Search As System.Web.UI.WebControls.Button
    Protected WithEvents lblCurrentIndex As System.Web.UI.WebControls.Label
    Protected WithEvents cmd_Add As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Modify As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Delete As System.Web.UI.WebControls.Button
    Protected WithEvents txt_Code As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_City As System.Web.UI.WebControls.TextBox
    Protected WithEvents cbo_State As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cbo_Country As System.Web.UI.WebControls.DropDownList

    Dim intRecord As Integer
    Dim intTotRecord As Integer
    Protected WithEvents dtgAddress As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblPageCount As System.Web.UI.WebControls.Label
    Dim strType As String
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Dim intTotPage As Integer
    Protected WithEvents trT As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trP As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents hidID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidMode As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidItem As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidAddrId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidAddrDesc As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
	Protected WithEvents txt_Address As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSelect As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hiddiv As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidAction As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lblAction As System.Web.UI.WebControls.Label

    'Dim strMode As String

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
        cmd_Add.Enabled = False
        cmd_Delete.Enabled = False
        cmd_Modify.Enabled = False
        Dim alButtonList As ArrayList
        alButtonList = New ArrayList
        alButtonList.Add(cmd_Add)
        htPageAccess.Add("add", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmd_Modify)
        htPageAccess.Add("update", alButtonList)
        alButtonList = New ArrayList
        alButtonList.Add(cmd_Delete)
        htPageAccess.Add("delete", alButtonList)
        CheckButtonAccess()
        '//additional checking
        If intPageRecordCnt > 0 Then
            Select Case viewState("Mode")
                Case "T"
                    cmd_Modify.Enabled = blnCanUpdate
                    cmd_Delete.Enabled = blnCanDelete
                    cmdReset.Disabled = False
                Case "P"
                    cmdSelect.Disabled = False
            End Select
        Else
            cmd_Delete.Enabled = False
            cmd_Modify.Enabled = False
            cmdReset.Disabled = True
        End If
        alButtonList.Clear()
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgAddress)
        Dim objGlobal As New AppGlobals
        If Not IsPostBack Then
            ViewState("Type") = Me.Request.QueryString("type")
            ViewState("Type2") = Me.Request.QueryString("type2")
            ViewState("Mode") = Me.Request.QueryString("mod") ' P - PR; T - Tab
            If ViewState("Mode") = "P" Then
                hidID.Value = Request.QueryString("id")
                hidMode.Value = Request.QueryString("mode")
                hidAddrId.Value = Request.QueryString("txtDelivery")
            Else
                ViewState("Side") = Request.Params("side")
                GenerateTab()
            End If
            'objGlobal.FillCodeTable(cbo_State, CodeTable.State)
            objGlobal.FillCodeTable(cbo_Country, CodeTable.Country)
            objGlobal.FillState(cbo_State, cbo_Country.SelectedItem.Value)
            '//because No DataGrid display when page first loaded

            cmd_Delete.Enabled = False
            cmd_Modify.Enabled = False
            cmdReset.Disabled = True

            If ViewState("Type2") = "RPO" Then
                dtgAddress.CurrentPageIndex = 0
                Bindgrid()
            End If
        End If

        If ViewState("Type") = "B" Then
            Me.hiddiv.Style("display") = "none"
            lblTitle.Text = "Billing Address"
            Me.lblAction.Text = "Fill in the search criteria and click Search button to list the relevant billing addresses. Click the Add button to add new billing address."
            trP.Visible = False
            trT.Visible = True
        ElseIf ViewState("Type") = "D" Then
            Me.hiddiv.Style("display") = "none"
            If ViewState("Mode") = "T" Then
                lblTitle.Text = "Delivery Address"
                Me.lblAction.Text = "Fill in the search criteria and click Search button to list the relevant delivery addresses. Click the Add button to add new delivery address."
                trP.Visible = False
                trT.Visible = True
            Else
                Me.hiddiv.Style("display") = ""
                Me.hidAction.Style("display") = "none"
                lblTitle.Text = "Select Delivery Address List"
                trP.Visible = True
                trT.Visible = False
            End If
        End If
        '//to attach javascript to button onclick event
        cmd_Delete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection','delete');")
        cmd_Modify.Attributes.Add("onclick", "return CheckOnlyOne('chkSelection');")
    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim objAdmin As New Admin
        Dim ds As New DataSet
        'ds = objAdmin.PopulateAddr(viewState("Type"), txt_Code.Text, txt_City.Text, cbo_State.SelectedItem.Value, True)
        ds = objAdmin.PopulateAddr(ViewState("Type"), txt_Code.Text, txt_Address.Text, txt_City.Text, IIf(cbo_State.SelectedItem.Value = "n.a.", "", cbo_State.SelectedItem.Value), True, cbo_Country.SelectedItem.Value)
        'ds = objAdmin.PopulateAddr(viewState("Type"), txt_Code.Text, txt_City.Text, "")
        intPageRecordCnt = ds.Tables(0).Rows.Count

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        '//these only needed if you can select a grid item and click delete button
        '//to solve paging problem. eg. 21 records found (3 pages) and user at the third page, 
        '//then user delete one record. //total record = 20 (2 pages), 
        '//but currentpageindex=3, total page=2, system cannot find page 3, runtime error occured
        If ViewState("action") = "del" Then
            If dtgAddress.CurrentPageIndex > 0 And ds.Tables(0).Rows.Count Mod dtgAddress.PageSize = 0 Then
                dtgAddress.CurrentPageIndex = dtgAddress.CurrentPageIndex - 1
                ViewState("action") = ""
            End If
        End If

        If intPageRecordCnt > 0 Then
            resetDatagridPageIndex(dtgAddress, dvViewSample)
            dtgAddress.DataSource = dvViewSample
            dtgAddress.DataBind()
        Else
            dtgAddress.DataBind()
            Common.NetMsgbox(Me, MsgNoRecord)
        End If
        ' add for above checking
        ViewState("PageCount") = dtgAddress.PageCount
    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        dtgAddress.CurrentPageIndex = 0
        Bindgrid(True)
    End Sub

    Private Sub cmd_Add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Add.Click
        Me.Response.Redirect(dDispatcher.direct("Admin", "AddAdds.aspx", "type=" & ViewState("Type") & "&mode=add&side=" & ViewState("Side") & "&pageid=" & strPageId))

    End Sub

    Private Sub cmd_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Search.Click
        dtgAddress.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Sub cmd_Modify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Modify.Click
        Dim dgItem As DataGridItem
        Dim strCode As String
        Dim chkItem As CheckBox
        For Each dgItem In dtgAddress.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                strCode = CType(dgItem.FindControl("lblAddrCode"), Label).Text
                Me.Response.Redirect(dDispatcher.direct("Admin", "AddAdds.aspx", "type=" & ViewState("Type") & "&side=" & ViewState("Side") & "&mode=update&code=" & Server.UrlEncode(strCode) & "&pageid=" & strPageId))

            End If
        Next
    End Sub

    Private Sub cmd_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Delete.Click
        Dim dgItem As DataGridItem
        Dim strCode As String
        Dim chkItem As CheckBox
        Dim objAdmin As New Admin
        Dim intMsgNo As Integer
        Dim strMsg As String
        Dim strNotDeleted As String

        Dim dt As New DataTable
        Dim dr As DataRow
        dt.Columns.Add("Code", Type.GetType("System.String"))

        For Each dgItem In dtgAddress.Items
            chkItem = dgItem.FindControl("chkSelection")
            If chkItem.Checked Then
                dr = dt.NewRow
                dr("Code") = CType(dgItem.FindControl("lblAddrCode"), Label).Text
                dt.Rows.Add(dr)
            End If
        Next

        intMsgNo = objAdmin.delAddress(dt, ViewState("Type"), strNotDeleted)

        If strNotDeleted = "" Then
            Select Case intMsgNo
                Case WheelMsgNum.Delete
                    strMsg = MsgRecordDelete
                Case WheelMsgNum.NotDelete
                    strMsg = MsgRecordNotDelete
            End Select
        Else
            strMsg = "Delivery address(s) not deleted successfully as it has outstanding DO(s)." & """& vbCrLf & """ & strNotDeleted
        End If

        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        ViewState("action") = "del"
        Bindgrid()
    End Sub

    Private Sub dtgAddress_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAddress.ItemCreated
        '//this line must be included
        Grid_ItemCreated(dtgAddress, e)
        '//to add a JavaScript to CheckAll button
        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            If ViewState("Mode") = "T" Then
                chkAll.Attributes.Add("onclick", "selectAll();")
            Else
                chkAll.Visible = False
            End If
        End If
    End Sub

    Private Sub dtgAddress_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAddress.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim lnkAddrCode As HyperLink
            Dim lbl As Label = e.Item.FindControl("lblSelection")
            Dim chk As CheckBox = e.Item.FindControl("chkSelection")

            lnkAddrCode = e.Item.FindControl("lnkAddrCode")
            lnkAddrCode.NavigateUrl = "" & dDispatcher.direct("Admin", "AddAdds.aspx", "mode=update&side=" & ViewState("Side") & "&pageid=" & strPageId & "&code=" & dv("AM_ADDR_CODE") & "&type=" & ViewState("Type"))
            lnkAddrCode.Text = dv("AM_ADDR_CODE")

            If ViewState("Mode") = "T" Then
                '//to add JavaScript to Check Box
                chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")
                '//to dynamic build hyperlink
                lnkAddrCode.NavigateUrl = "" & dDispatcher.direct("Admin", "AddAdds.aspx", "mode=update&side=" & ViewState("Side") & "&pageid=" & strPageId & "&code=" & dv("AM_ADDR_CODE") & "&type=" & ViewState("Type"))
                lbl.Visible = False
            ElseIf ViewState("Mode") = "P" Then
                lnkAddrCode.NavigateUrl = ""
                lbl.Text = "<input type=radio name='Myid' value=""" & CType(e.Item.FindControl("lblAddrCode"), Label).Text & """ onclick=""Chk('" & CType(e.Item.FindControl("lblAddrCode"), Label).Text & "', '" & Replace(dv("Address"), "'", "\'") & "')"">"
                chk.Visible = False
            End If
        End If
    End Sub

    Private Sub dtgAddress_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgAddress.PageIndexChanged
        dtgAddress.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        'If ViewState("Side") = "BUYER" Then
        '    If ViewState("Type") = "B" Then
        '        Session("w_Address_tabs") = "<div class=""t_entity""><ul>" & _
        '        "<li><div class=""space""></div></li>" & _
        '                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=BUYER&mode=&modify&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=D&mod=T&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                              "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYERtype=B&mod=T&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                           "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Department</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                            "</ul><div></div></div>"
        '    ElseIf ViewState("Type") = "D" Then
        '        Session("w_Address_tabs") = "<div class=""t_entity""><ul>" & _
        '        "<li><div class=""space""></div></li>" & _
        '                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=BUYER&mode=&modify&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                              "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=D&mod=T&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYERtype=B&mod=T&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                           "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Department</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                            "</ul><div></div></div>"
        '    End If
        'Else
        '    If ViewState("Type") = "B" Then
        '        Session("w_Address_tabs") = "<div class=""t_entity""><ul>" & _
        '        "<li><div class=""space""></div></li>" & _
        '                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=BUYER&mode=modify&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=D&mod=T&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                              "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=B&mod=T&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                           "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Department</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SalesInfo.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Sales Info</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SoftwareApp.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Software</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "QualityStd.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Quality Standards</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                            "</ul><div></div></div>"
        '    ElseIf ViewState("Type") = "D" Then
        '        Session("w_Address_tabs") = "<div class=""t_entity""><ul>" & _
        '        "<li><div class=""space""></div></li>" & _
        '                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=BUYER&mode=modify&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                              "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=D&mod=T&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=B&mod=T&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                           "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Department</span></a></li>" & _
        '                             "<li><div class=""space""></div></li>" & _
        '                            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SalesInfo.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Sales Info</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SoftwareApp.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Software</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "QualityStd.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Quality Standards</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '                            "</ul><div></div></div>"

        '    End If
        'End If

        If ViewState("Side") = "BUYER" Then
            If ViewState("Type") = "B" Then
                Session("w_Address_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=BUYER&mode=modify&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=D&mod=T&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=B&mod=T&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Department</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "</ul><div></div></div>"
            ElseIf ViewState("Type") = "D" Then
                Session("w_Address_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=BUYER&mode=modify&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=D&mod=T&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=B&mod=T&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Department</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                                  "</ul><div></div></div>"
            End If
        Else
            If ViewState("Type") = "B" Then
                Session("w_Address_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=BUYER&mode=modify&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=D&mod=T&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=B&mod=T&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Department</span></a></li>" & _
                              "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SalesInfo.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Sales Info</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SoftwareApp.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Software</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "QualityStd.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Quality Standards</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                    "</ul><div></div></div>"
            ElseIf ViewState("Type") = "D" Then
                Session("w_Address_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=BUYER&mode=modify&pageid=" & strPageId) & """><span>Company Details</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Parameters</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=D&mod=T&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                                  "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=B&mod=T&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
                                  "<li><div class=""space""></div></li>" & _
                              "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=BUYER&pageid=" & strPageId) & """><span>Department</span></a></li>" & _
                              "<li><div class=""space""></div></li>" & _
                                    "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SalesInfo.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Sales Info</span></a></li>" & _
                                    "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "SoftwareApp.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Software</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "QualityStd.aspx", "side=VENDOR&pageid=" & strPageId) & """><span>Quality Standards</span></a></li>" & _
                                "<li><div class=""space""></div></li>" & _
                                     "</ul><div></div></div>"

            End If
        End If


    End Sub

    Private Sub cbo_Country_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Country.SelectedIndexChanged
        Dim objGlobal As New AppGlobals
        objGlobal.FillState(cbo_State, cbo_Country.SelectedItem.Value)
    End Sub

End Class
