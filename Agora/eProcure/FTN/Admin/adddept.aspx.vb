Imports System.Data.SqlClient
Imports AgoraLegacy
Imports eProcure.Component

Public Class adddeptFTN
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Dim strMode As String    

    Dim intIndex As Integer


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
        cmdSave.Enabled = False
        cmdDelete.Enabled = False
        Dim alButtonList As ArrayList
        If strMode = "update" Then
            alButtonList = New ArrayList
            alButtonList.Add(cmdSave)
            htPageAccess.Add("update", alButtonList)
            alButtonList = New ArrayList
            alButtonList.Add(cmdDelete)
            htPageAccess.Add("delete", alButtonList)
        ElseIf strMode = "add" Then
            alButtonList = New ArrayList
            alButtonList.Add(cmdSave)
            htPageAccess.Add("add", alButtonList)
        End If
        CheckButtonAccess()
        cmdClear.Disabled = Not (blnCanAdd Or blnCanUpdate Or blnCanDelete)
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'Session("CompanyId") = "demo"
        'Session("UserId") = "myongnc"
        MyBase.Page_Load(sender, e)
        blnSorting = False
        blnPaging = False
        SetGridProperty(dtgAO)

        'strMode = Me.Request.QueryString("mode")
        If ViewState("mode") = "add" Then
            strMode = "add"
        ElseIf ViewState("mode") = "modify" Then
            strMode = "update"
        Else
            strMode = Me.Request.QueryString("mode")
        End If
        intIndex = Me.Request.QueryString("dindex")
        'vldSumm.Visible = True


        If Not Page.IsPostBack Then
            Dim objAppWF As New ApprWorkFlow

            Dim dsApprovalList As New DataSet
            Dim intCnt As Integer
            Dim i As Integer

            'If Session("Env") = "FTN" Then Me.hidrow.Style("display") = "none"
            Me.hidrow.Style("display") = "none"

            ViewState("Side") = Request.Params("side")
            GenerateTab()
            dsApprovalList = objAppWF.getPaymentAppGrpList()
            intCnt = dsApprovalList.Tables(0).Rows.Count

            If dsApprovalList.Tables(0).Rows.Count > 0 Then
                Common.FillDdl(cboApproval, "AGM_GRP_NAME", "AGM_GRP_INDEX", dsApprovalList)
                Dim lstItem As New ListItem
                ' Add ---Select---
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                cboApproval.Items.Insert(0, lstItem)
                lblMsg.Visible = False
                If dsApprovalList.Tables(0).Rows.Count = 1 Then '//Display approving officer if only has one approval workflow
                    cboApproval.SelectedIndex = 1
                    cboApproval_SelectedIndexChanged(sender, e)
                End If
            Else
                cboApproval.Visible = False
                lblRemark.Visible = False
                lblMsg.Visible = True
                lblMsg.Text = "There is no approval list available for this PO because the sequence of approving officers do not have the approval limit to approve it."
            End If

            If strMode = "update" Then
                Me.txt_deptCode.Text = Me.Request.QueryString("deptcode")
                Me.txt_deptCode.Enabled = False
                Me.txt_deptName.Text = Me.Request.QueryString("deptname")
                ViewState("oldname") = Me.Request.QueryString("deptname")
                Me.txt_deptName.Enabled = True

                'If Session("Env") <> "FTN" Then
                '    If Request.QueryString("appgrpindex") > 0 Then
                '        Try
                '            Me.cboApproval.SelectedValue = Request.QueryString("appgrpindex")
                '        Catch ex As Exception
                '            Me.cboApproval.SelectedIndex = 0
                '        End Try
                '        Bindgrid()
                '    End If
                'Else
                '    Me.cboApproval.SelectedIndex = 0
                '    lblRemark.Text = ""
                '    dtgAO.DataBind()
                'End If
                Me.cboApproval.SelectedIndex = 0
                lblRemark.Text = ""
                dtgAO.DataBind()

                Me.cboApproval.Enabled = True

                Me.cmdDelete.Visible = True
                Me.cmdSave.Visible = True
                Me.cmdClear.Value = "Reset"
                lblHeader.Text = "Modify Department"
                cmdadd.Visible = False
            Else
                lblHeader.Text = "Add Department"
                Me.cmdClear.Value = "Clear"
                Me.cmdClear.Visible = False
            End If

        End If
        cmdDelete.Attributes.Add("onclick", "return confirm('" & MsgForDeleteButton & "');")
        cmdadd.CausesValidation = False
        lnkBack.NavigateUrl = "" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=" & Request.QueryString("side") & "&pageid=" & strPageId)

    End Sub

    Private Function Bindgrid() As String
        Dim objApp As New ApprWorkFlow
        Dim dsAO As New DataSet
        Dim dvViewSample As DataView

        If cboApproval.SelectedIndex > 0 Then
            dsAO = objApp.getPaymentAppList(cboApproval.SelectedItem.Value)
            viewstate("intPageRecordCnt") = dsAO.Tables(0).Rows.Count
            dvViewSample = dsAO.Tables(0).DefaultView
            intPageRecordCnt = viewstate("intPageRecordCnt")

            dtgAO.DataSource = dvViewSample
        Else
            dtgAO.DataSource = Nothing
        End If

        dtgAO.DataBind()
        viewstate("blnSubmit") = False

        If intPageRecordCnt > 0 Then
            'displayRemark()
        Else
            lblRemark.Text = ""
        End If

    End Function

    Private Sub cboApproval_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboApproval.SelectedIndexChanged

        If Not Page.IsPostBack Then Exit Sub

        If cboApproval.SelectedItem.Value <> "" Then
            Bindgrid()
        Else
            lblRemark.Text = ""
            dtgAO.DataBind()
        End If
    End Sub

    Private Sub dtgAO_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAO.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            Static intRow As Integer
            intRow = intRow + 1

            e.Item.Cells(0).Text = intRow

            If IsDBNull(dv("UM_APP_LIMIT")) Then
                dv("UM_APP_LIMIT") = 0
            End If
        End If
    End Sub

    Private Sub dtgAO_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAO.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgAO, e)
    End Sub

    Public Sub dtgAO_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgAO.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        'vl(True)
        Dim strDCode, strDName, strPaymentGrpIndex As String
        'Dim strRedirect As String
        Dim strMsg As String
        Dim intMsgNo As Integer
        'vldSumm.Visible = True


        Dim objAdmin As New Admin
        strDCode = Me.txt_deptCode.Text
        strDName = Me.txt_deptName.Text
        strPaymentGrpIndex = Me.cboApproval.SelectedValue

        'strRedirect = "DeptSetup.aspx?pageid=" & strPageId
        If strMode = "add" Then
            intMsgNo = objAdmin.addDept(strDCode, strDName, strPaymentGrpIndex)
            Select Case intMsgNo
                Case WheelMsgNum.Save
                    strMsg = MsgRecordSave
                    'txt_deptCode.Text = ""
                    'txt_deptName.Text = ""

                    'If Session("Env") <> "FTN" And cboApproval.Visible = True Then
                    '    cboApproval.SelectedIndex = 0
                    'End If

                    'cboApproval.SelectedIndex = 0
                    ViewState("mode") = "modify"
                    txt_deptCode.Enabled = False
                Case WheelMsgNum.Duplicate
                    strMsg = MsgRecordDuplicate
                Case WheelMsgNum.NotSave
                    strMsg = MsgRecordNotSave
            End Select
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        Else
            'intMsgNo = objAdmin.moddept(strDCode, strDName)
            intMsgNo = objAdmin.moddept(strDCode, strDName, viewstate("oldname"), strPaymentGrpIndex)
            'txt_deptName.Text = viewstate("oldname")

            Select Case intMsgNo
                Case WheelMsgNum.Save
                    strMsg = MsgRecordSave
                    'txt_deptCode.Text = ""
                    'txt_deptName.Text = ""
                    'cboApproval.SelectedIndex = 0
                    'ViewState("oldname") = ""
                    'Common.NetMsgbox(Me, strMsg, strRedirect, MsgBoxStyle.Information)
                Case WheelMsgNum.Duplicate
                    strMsg = MsgRecordDuplicate
                    'Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                Case WheelMsgNum.NotSave
                    strMsg = MsgRecordNotSave
                    'Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
            End Select
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        End If
        objAdmin = Nothing
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        'vl(True)
        Dim strDCode As String
        strDCode = Me.txt_deptCode.Text
        Dim objAdmin As New Admin
        Dim strMsg As String
        Dim intMsgNo As Integer

        Dim dtNewDept As New DataTable
        Dim drDept As DataRow
        dtNewDept.Columns.Add("Dept_CODE", Type.GetType("System.String")) '//product code
        dtNewDept.Columns.Add("Dept_Index", Type.GetType("System.Int32")) '//supplier id
        drDept = dtNewDept.NewRow
        drDept("Dept_Code") = strDCode
        drDept("Dept_Index") = intIndex
        dtNewDept.Rows.Add(drDept)
        intMsgNo = objAdmin.delCdept(dtNewDept)
        Select Case intMsgNo
            Case -99
                Common.NetMsgbox(Me, "Deletion is not allow.", MsgBoxStyle.Information)
            Case WheelMsgNum.Delete
                strMsg = MsgRecordDelete
                'Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Information)
                Common.NetMsgbox(Me, MsgRecordDelete, dDispatcher.direct("Admin", "DeptSetup.aspx", "side=" & ViewState("Side") & "&pageid=" & strPageId), MsgBoxStyle.Information)
                'Me.Response.Redirect("DeptSetup.aspx?pageid=" & strPageId)
            Case WheelMsgNum.NotDelete
                strMsg = MsgRecordNotDelete
                Common.NetMsgbox(Me, MsgRecordNotDelete, MsgBoxStyle.Information)
        End Select
        'Me.Response.Redirect("DeptSetup.aspx?pageid=" & strPageId)


    End Sub
    Private Sub cmdadd_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdadd.ServerClick
        ViewState("mode") = "add"
        strMode = "add"
        txt_deptCode.Text = ""
        txt_deptName.Text = ""
        txt_deptCode.Enabled = True
    End Sub
    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        Session("w_AddDept_tabs") = "<div class=""t_entity""><ul>" & _
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
    End Sub

End Class
