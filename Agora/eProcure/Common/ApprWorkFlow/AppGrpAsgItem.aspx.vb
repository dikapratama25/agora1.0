Imports AgoraLegacy
Imports eProcure.Component


Public Class AppGrpAsgItem
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objAdm As New Admin
    Dim objApp As New ApprWorkFlow
    Dim objGlobal As New AppGlobals
    'Dim chkAll As CheckBox

    Protected WithEvents cmd_search As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_clear1 As System.Web.UI.WebControls.Button
    Protected WithEvents rbItemCode As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents lblDept As System.Web.UI.WebControls.Label
    Protected WithEvents lblIQCType As System.Web.UI.WebControls.Label
    Protected WithEvents dtgAsgItem As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtItemCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtItemName As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddl_Type As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddl_Approval As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Hide_Add2 As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Dim ds As DataSet

    Public Enum EnumItem
        icChk = 0
        icItemIndex = 1
        icItemCode = 2
        icItemDesc = 3
        icVendorItemCode = 4
        icUOM = 5
        icProdCode = 6
        icStatus = 7
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

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        MyBase.Page_Load(sender, e)
        SetGridProperty(dtgAsgItem)
        If Not IsPostBack Then
            GenerateTab()
            cmd_save.Enabled = False
            ddl_Type.SelectedValue = "IQC"
            ViewState("ApprType") = "IQC"
            BindGroup()
        End If

        Session("UrlLocation") = "AppGrpAsgItem"
    End Sub

    Sub BindGroup()
        Dim cbolist As New ListItem
        Dim dvCustom As DataView

        dvCustom = objApp.getcboApp("AGM_TYPE = '" & ViewState("ApprType") & "'")

        ddl_Approval.Items.Clear()
        If Not dvCustom Is Nothing Then
            Common.FillDdl(ddl_Approval, "AGM_GRP_NAME", "AGM_GRP_INDEX", dvCustom)
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        ddl_Approval.Items.Insert(0, cbolist)

    End Sub

    Sub dtgAsgItem_Page(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dtgAsgItem.PageIndexChanged
        ViewState("SortExpression") = "PM_PRODUCT_INDEX"
        dtgAsgItem.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub

    Sub SortCommand_Click(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles dtgAsgItem.SortCommand

        Grid_SortCommand(sender, e)
        ViewState("SortExpression") = "PM_PRODUCT_INDEX"
        dtgAsgItem.CurrentPageIndex = 0
        Bindgrid()

    End Sub

    Private Function Bindgrid(Optional ByVal pSorted As Boolean = False) As String
        Dim ds As DataSet

        ds = objApp.SearchItemForAssign(txtItemCode.Text, txtItemName.Text, ViewState("ApprIndex"), rbItemCode.SelectedValue, lblIQCType.Text)
        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            dtgAsgItem.DataSource = dvViewSample
            dtgAsgItem.DataBind()
            cmd_save.Enabled = True
        Else
            Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00006"), MsgBoxStyle.Information)
            dtgAsgItem.DataBind()
            cmd_save.Enabled = False
        End If

    End Function

    Private Sub dtgAsgItem_ItemCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAsgItem.ItemCreated

        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgAsgItem, e)

        If e.Item.ItemType = ListItemType.Header Then
            Dim chkAll As CheckBox = e.Item.FindControl("chkAll")
            If rbItemCode.SelectedValue = "2" Then
                chkAll.Checked = True
            End If
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If

    End Sub

    Private Sub dtgAsgItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAsgItem.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            Dim lnkItem As HyperLink
            lnkItem = e.Item.FindControl("lnkItem")
            lnkItem.NavigateUrl = "javascript:;"
            lnkItem.Attributes.Add("onclick", "return PopWindow('" & dDispatcher.direct("Product", "ProductDetail.aspx", "companyType=B&pageid=" & strPageId & "&pid=" & dv("PM_PRODUCT_CODE") & "&index=&draft=0") & "')")
            lnkItem.Text = dv("PM_VENDOR_ITEM_CODE")

            If dv("CHECKED") = "Y" Then
                chk.Checked = True
            Else
                chk.Checked = False
            End If

        End If
    End Sub

    Private Sub cmd_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_search.Click

        If ddl_Approval.SelectedIndex = 0 Then
            ViewState("ApprIndex") = ""
            Common.NetMsgbox(Me, "Please select approval group.", MsgBoxStyle.Information)
            dtgAsgItem.DataSource = Nothing
            dtgAsgItem.DataBind()
            cmd_save.Enabled = False
        Else
            ViewState("ApprIndex") = ddl_Approval.SelectedValue
            dtgAsgItem.CurrentPageIndex = 0
            Bindgrid()
        End If
       
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_ApprWFAsgItem_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("ApprWorkFlow", "ApprovalWorkFlow.aspx", "pageid=" & strPageId) & """><span>Approval Group</span></a></li>" & _
                "<li><div class=""space""></div></li>" & _
                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("ApprWorkFlow", "AppGrpAsg.aspx", "pageid=" & strPageId) & """><span>Approval Officer Assignment</span></a></li>" & _
                "<li><div class=""space""></div></li>" & _
                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("ApprWorkFlow", "AppGrpAsgPur.aspx", "pageid=" & strPageId) & """><span>User Assignment</span></a></li>" & _
                "<li><div class=""space""></div></li>" & _
                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("ApprWorkFlow", "AppGrpAsgItem.aspx", "pageid=" & strPageId) & """><span>Item Assignment</span></a></li>" & _
                "<li><div class=""space""></div></li>" & _
                "</ul><div></div></div>"

    End Sub

    Private Sub ddl_Approval_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddl_Approval.SelectedIndexChanged
        rbItemCode.SelectedValue = "2"

        If ddl_Approval.SelectedItem.Text = "---Select---" Then
            lblDept.Text = ""
            lblIQCType.Text = ""
            ViewState("ApprIndex") = ""
            dtgAsgItem.DataSource = Nothing
            dtgAsgItem.DataBind()
            cmd_save.Enabled = False
        Else
            lblDept.Text = objApp.getdeptname(ddl_Approval.SelectedItem.Value)
            lblIQCType.Text = objApp.getIQCType(ddl_Approval.SelectedItem.Value)

            If objApp.ChkIQCOfficerType(ddl_Approval.SelectedValue) Then
                ViewState("SortExpression") = "PM_PRODUCT_INDEX"
                dtgAsgItem.CurrentPageIndex = 0
                ViewState("ApprIndex") = ddl_Approval.SelectedValue
                Bindgrid(0)
            Else
                lblDept.Text = ""
                lblIQCType.Text = ""
                ddl_Approval.SelectedIndex = 0
                ViewState("ApprIndex") = ""
                dtgAsgItem.DataSource = Nothing
                dtgAsgItem.DataBind()
                cmd_save.Enabled = False
                Common.NetMsgbox(Me, "Selected Group Approval Officer assignment has not yet completed.", MsgBoxStyle.Information)
            End If
        End If

    End Sub
       
    Private Sub cmd_clear1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_clear1.Click
        txtItemCode.Text = ""
        txtItemName.Text = ""
        ddl_Approval.SelectedIndex = 0
        lblDept.Text = ""
        lblIQCType.Text = ""
        ViewState("ApprIndex") = ""
        dtgAsgItem.DataSource = Nothing
        dtgAsgItem.DataBind()
        cmd_save.Enabled = False
        rbItemCode.SelectedValue = "1"

    End Sub

    Private Sub cmd_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_save.Click
        Dim intMsg As Integer
        Dim dsItem As New DataSet

        dsItem = bindItem()

        If dsItem.Tables(0).Rows.Count > 0 Or dsItem.Tables(1).Rows.Count > 0 Then
            Dim objUsers As New Users
            objApp.RemoveItemForApprGrp(dsItem.Tables(1))
            intMsg = objApp.InsertItemForApprGrp(dsItem.Tables(0))
            Select Case intMsg
                Case WheelMsgNum.Save
                    Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
                    dtgAsgItem.CurrentPageIndex = 0
                    Bindgrid()

                Case WheelMsgNum.NotSave
                    Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
            End Select
        End If
    End Sub

    Private Sub rbItemCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbItemCode.SelectedIndexChanged
        If ddl_Approval.SelectedItem.Text = "---Select---" Then
            lblDept.Text = ""
            lblIQCType.Text = ""
            ViewState("ApprIndex") = ""
            Common.NetMsgbox(Me, "Please select approval group.", MsgBoxStyle.Information)
            dtgAsgItem.DataSource = Nothing
            dtgAsgItem.DataBind()
            cmd_save.Enabled = False
        Else
            ViewState("SortExpression") = "PM_PRODUCT_INDEX"
            dtgAsgItem.CurrentPageIndex = 0
            lblDept.Text = objApp.getdeptname(ddl_Approval.SelectedItem.Value)
            lblIQCType.Text = objApp.getIQCType(ddl_Approval.SelectedItem.Value)
            ViewState("ApprIndex") = ddl_Approval.SelectedValue
            Bindgrid(0)
          
        End If
    End Sub

    Private Function bindItem() As DataSet
        Dim dtItem As New DataTable
        Dim dtDelete As New DataTable
        Dim ds As New DataSet

        dtItem.Columns.Add("AGP_GRP_INDEX", Type.GetType("System.String"))
        dtItem.Columns.Add("AGP_PRODUCT_CODE", Type.GetType("System.String"))

        dtDelete.Columns.Add("AGP_GRP_INDEX", Type.GetType("System.String"))
        dtDelete.Columns.Add("AGP_PRODUCT_CODE", Type.GetType("System.String"))

        Dim dtr As DataRow
        Dim dgItem As DataGridItem
        For Each dgItem In dtgAsgItem.Items
            Dim chkSelection As CheckBox = dgItem.Cells(0).FindControl("chkSelection")
            If chkSelection.Checked Then
                dtr = dtItem.NewRow
                dtr("AGP_GRP_INDEX") = ddl_Approval.SelectedValue
                dtr("AGP_PRODUCT_CODE") = dgItem.Cells(EnumItem.icProdCode).Text
                dtItem.Rows.Add(dtr)

            Else    'remove section
                dtr = dtDelete.NewRow
                dtr("AGP_GRP_INDEX") = ddl_Approval.SelectedValue
                dtr("AGP_PRODUCT_CODE") = dgItem.Cells(EnumItem.icProdCode).Text
                dtDelete.Rows.Add(dtr)
            End If
        Next
        ds.Tables.Add(dtItem)
        ds.Tables.Add(dtDelete)
        bindItem = ds
    End Function
    
End Class
