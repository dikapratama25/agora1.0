
Imports AgoraLegacy
Imports eProcure.Component
Imports System.text
Public Class usCommodity
    Inherits AgoraLegacy.AppBaseClass
    Dim objDb As New EAD.DBCom
    Dim objUsers As New Users
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim chkAll As CheckBox

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents txtUserID As System.Web.UI.WebControls.Label
    'Protected WithEvents lbl_result As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdReset As System.Web.UI.HtmlControls.HtmlInputButton
    'Protected WithEvents txtUserName As System.Web.UI.WebControls.Label
    Protected WithEvents dgAddr As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cboUser As System.Web.UI.WebControls.DropDownList
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyBase.Page_Load(sender, e)
        SetGridProperty(dgComm)
        dgComm.AllowPaging = False

        If Not IsPostBack Then
            cmdSave.Enabled = False
            GenerateTab()
            BindUser()
            'If Session("Env") <> "FTN" Then 'Non FTN
            '    Me.lblAction.Text = "Step 1: Create, modify or delete User Account.<br />Step 2: Assign Delivery Address to selected User Account.<br />Step 3: Assign Billing Address to selected User Account.<br />Step 4: Assign Finance Viewing Department to selected User Account.<br /><b>=></b> Step 5: Assign Commodity Type to selected User Account."

            'Else
            '    Me.lblAction.Text = "Step 1: Create, modify or delete User Account.<br />Step 2: Assign Delivery Address to selected User Account.<br />Step 3: Assign Billing Address to selected User Account.<br /><b>=></b> Step 4: Assign Commodity Type to selected User Account."

            'End If
            Me.lblAction.Text = "Step 1: Create, modify or delete User Account.<br />Step 2: Assign Delivery Address to selected User Account.<br />Step 3: Assign Billing Address to selected User Account.<br />Step 4: Assign Finance Viewing Department to selected User Account.<br /><b>=></b> Step 5: Assign Commodity Type to selected User Account."
        End If
    End Sub

    Public Function BindUser()
        Dim cbolist As New ListItem
        Dim objUsr As New Users
        Dim dvCustom As DataView

        dvCustom = objUsr.getUsr
        cboUser.Items.Clear()
        If Not dvCustom Is Nothing Then
            Common.FillDdl(cboUser, "USER", "UM_USER_ID", dvCustom)
        End If
        cbolist.Value = ""
        cbolist.Text = "---Select---"
        cboUser.Items.Insert(0, cbolist)

    End Function

    Public Sub SortCommand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        Grid_SortCommand(sender, e)
        'ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "CT_NAME"
        dgComm.CurrentPageIndex = 0
        Bindgrid()
    End Sub

    Private Function Bindgrid(Optional ByVal pPage As Integer = -1, Optional ByVal pSorted As Boolean = False) As String
        Dim objUsers As New Users
        Dim ds As New DataSet
        ds = objUsers.GetCommodity(ViewState("CTUserID"))
        intPageRecordCnt = ds.Tables(0).Rows.Count
        ViewState("intPageRecordCnt") = intPageRecordCnt

        Dim dvViewSample As DataView
        dvViewSample = ds.Tables(0).DefaultView
        dvViewSample.Sort = ViewState("SortExpression")
        If ViewState("SortAscending") = "no" Then dvViewSample.Sort += " DESC"

        If intPageRecordCnt > 0 Then
            dgComm.DataSource = dvViewSample
            dgComm.DataBind()
            cmdSave.Enabled = True
        Else
            'Common.NetMsgbox(Me, MsgNoRecord)
            dgComm.DataBind()
            cmdSave.Enabled = False
        End If

    End Function

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        'If Session("Env") <> "FTN" Then 'Non FTN
        '    Session("w_UserAddr_tabs") = "<div class=""t_entity""><ul>" & _
        '                                "<li><div class=""space""></div></li>" & _
        '                                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usSearchUser.aspx", "pageid=" & strPageId) & """><span>User Account</span></a></li>" & _
        '                                 "<li><div class=""space""></div></li>" & _
        '                                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=D&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
        '                                 "<li><div class=""space""></div></li>" & _
        '                                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=B&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
        '                                 "<li><div class=""space""></div></li>" & _
        '                                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usFinVwDept.aspx", "pageid=" & strPageId) & """><span>Finance Viewing Department</span></a></li>" & _
        '                                 "<li><div class=""space""></div></li>" & _
        '                                 "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Users", "usCommodity.aspx", "pageid=" & strPageId) & """><span>Commodity Assignment</span></a></li>" & _
        '                                 "<li><div class=""space""></div></li>" & _
        '                                 "</ul><div></div></div>"
        'Else
        '    Session("w_UserAddr_tabs") = "<div class=""t_entity""><ul>" & _
        '                                "<li><div class=""space""></div></li>" & _
        '                                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usSearchUser.aspx", "pageid=" & strPageId) & """><span>User Account</span></a></li>" & _
        '                                 "<li><div class=""space""></div></li>" & _
        '                                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=D&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
        '                                 "<li><div class=""space""></div></li>" & _
        '                                 "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=B&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
        '                                 "<li><div class=""space""></div></li>" & _
        '                                 "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Users", "usCommodity.aspx", "pageid=" & strPageId) & """><span>Commodity Assignment</span></a></li>" & _
        '                                 "<li><div class=""space""></div></li>" & _
        '                                 "</ul><div></div></div>"
        'End If

        Session("w_UserAddr_tabs") = "<div class=""t_entity""><ul>" & _
                    "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usSearchUser.aspx", "pageid=" & strPageId) & """><span>User Account</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=D&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usDeliveryAddr.aspx", "AddrType=B&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Users", "usFinVwDept.aspx", "pageid=" & strPageId) & """><span>Finance Viewing Department</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Users", "usCommodity.aspx", "pageid=" & strPageId) & """><span>Commodity Assignment</span></a></li>" & _
                     "<li><div class=""space""></div></li>" & _
                     "</ul><div></div></div>"
        

    End Sub

    Private Sub cboUser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboUser.SelectedIndexChanged
        If cboUser.SelectedIndex > 0 Then
            ViewState("SortAscending") = "yes"
            ViewState("SortExpression") = "CT_NAME"
            dgComm.CurrentPageIndex = 0
            ViewState("CTUserID") = cboUser.SelectedValue
            Bindgrid()
        Else
            ViewState("CTUserID") = ""
            cmdSave.Enabled = False
            dgComm.DataSource = Nothing
            dgComm.DataBind()
        End If

    End Sub

    Private Sub dgComm_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgComm.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dgComm, e)

        If e.Item.ItemType = ListItemType.Header Then
            chkAll = e.Item.FindControl("chkAll")
            chkAll.Attributes.Add("onclick", "selectAll();")
        End If
    End Sub

    Private Sub dgComm_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgComm.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim chk As CheckBox
            chk = e.Item.Cells(0).FindControl("chkSelection")
            chk.Attributes.Add("onclick", "checkChild('" & chk.ClientID & "')")

            If e.Item.Cells(4).Text = "Y" Then
                chk.Checked = True
            Else
                chkAll.Checked = False
            End If
            e.Item.Cells(4).Text = IIf(e.Item.Cells(4).Text = "Y", "Yes", "No")
        End If
    End Sub

    Private Sub dgComm_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgComm.PageIndexChanged
        'ViewState("SortAscending") = "yes"
        ViewState("SortExpression") = "CT_NAME"
        dgComm.CurrentPageIndex = e.NewPageIndex
        Bindgrid(0)
    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim intMsg As Integer
        Dim dsItem As New DataSet

        dsItem = bindItem()

        If dsItem.Tables(0).Rows.Count > 0 Or dsItem.Tables(1).Rows.Count > 0 Then
            Dim objUsers As New Users
            objUsers.RemoveCommodityType(dsItem.Tables(1))
            intMsg = objUsers.insertCommodityType(dsItem.Tables(0))
            Select Case intMsg
                Case WheelMsgNum.Save
                    Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Information)
                    dgComm.CurrentPageIndex = 0
                    Bindgrid()

                Case WheelMsgNum.NotSave
                    Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
            End Select
        End If
    End Sub

    Private Function bindItem() As DataSet
        Dim dtItem As New DataTable
        Dim dtDelete As New DataTable
        Dim ds As New DataSet
        Dim blnDiscount As Boolean
        Dim strPrice As String
        Dim strRemark As String

        dtItem.Columns.Add("UA_COY_ID", Type.GetType("System.String"))
        dtItem.Columns.Add("UA_USER_ID", Type.GetType("System.String"))
        dtItem.Columns.Add("UA_FIXED_ROLE", Type.GetType("System.String"))
        dtItem.Columns.Add("UA_TYPE", Type.GetType("System.String"))
        dtItem.Columns.Add("UA_ASSIGN_VALUE", Type.GetType("System.String"))

        dtDelete.Columns.Add("UA_COY_ID", Type.GetType("System.String"))
        dtDelete.Columns.Add("UA_USER_ID", Type.GetType("System.String"))
        dtDelete.Columns.Add("UA_FIXED_ROLE", Type.GetType("System.String"))
        dtDelete.Columns.Add("UA_TYPE", Type.GetType("System.String"))
        dtDelete.Columns.Add("UA_ASSIGN_VALUE", Type.GetType("System.String"))

        Dim dtr As DataRow
        Dim dgItem As DataGridItem
        For Each dgItem In dgComm.Items
            Dim chkSelection As CheckBox = dgItem.Cells(0).FindControl("chkSelection")
            If chkSelection.Checked Then
                dtr = dtItem.NewRow
                dtr("UA_COY_ID") = Session("CompanyId")
                dtr("UA_USER_ID") = cboUser.SelectedValue
                dtr("UA_FIXED_ROLE") = "Purchasing Officer"
                dtr("UA_TYPE") = "CT"
                dtr("UA_ASSIGN_VALUE") = dgItem.Cells(3).Text
                dtItem.Rows.Add(dtr)

            Else    'remove commodity type
                dtr = dtDelete.NewRow
                dtr("UA_COY_ID") = Session("CompanyId")
                dtr("UA_USER_ID") = cboUser.SelectedValue
                dtr("UA_FIXED_ROLE") = "Purchasing Officer"
                dtr("UA_TYPE") = "CT"
                dtr("UA_ASSIGN_VALUE") = dgItem.Cells(3).Text
                dtDelete.Rows.Add(dtr)
            End If
        Next
        ds.Tables.Add(dtItem)
        ds.Tables.Add(dtDelete)
        bindItem = ds
    End Function
End Class