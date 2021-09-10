Imports System.Data.SqlClient
Imports AgoraLegacy
Imports eProcure.Component

Public Class adddept
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
   
    Dim strMode As String
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txt_deptCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents vldDeptCode As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents txt_deptName As System.Web.UI.WebControls.TextBox
    Protected WithEvents vldDeptName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDelete As System.Web.UI.WebControls.Button
    Protected WithEvents cmdadd As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents cboApproval As System.Web.UI.WebControls.DropDownList
    Protected WithEvents dtgAO As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRemark As System.Web.UI.WebControls.Label
    Protected WithEvents hidrow As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cboApprovalType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TRIPP As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cboIPPApproval As System.Web.UI.WebControls.DropDownList

    'Zulham 16072018 - PAMB
    Protected WithEvents dtgAO_NR As System.Web.UI.WebControls.DataGrid

    Dim intIndex As Integer
    'Zulham 11102018 - PAMB
    Dim objDb As New EAD.DBCom


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
        MyBase.Page_Load(sender, e)
        blnSorting = False
        blnPaging = False
        SetGridProperty(dtgAO)
        'Zulham 16072018
        SetGridProperty(dtgAO_NR)

        'strMode = Me.Request.QueryString("mode")
        If ViewState("mode") = "add" Then
            strMode = "add"
        ElseIf ViewState("mode") = "modify" Then
            strMode = "update"
        Else
            strMode = Me.Request.QueryString("mode")
        End If
        intIndex = Me.Request.QueryString("index")
        'Get IPP approval index
        'getIPPApprovalindex(intIndex)
        'vldSumm.Visible = True


        If Not Page.IsPostBack Then
            Dim objAppWF As New ApprWorkFlow

            Dim dsApprovalList As New DataSet
            Dim intCnt As Integer
            Dim i As Integer

            If strMode = "update" Then
                If Request.QueryString("approvaltype") = "IPP" Then
                    cboApprovalType.SelectedValue = Request.QueryString("approvaltype")
                    Me.hidrow.Style("display") = "none"
                    Me.TRIPP.Style("display") = ""
                Else
                    cboApprovalType.SelectedValue = "Invoice"
                    Me.hidrow.Style("display") = ""
                    Me.TRIPP.Style("display") = "none"
                End If
            End If


            ViewState("Side") = Request.Params("side")
            GenerateTab()
            If cboApprovalType.SelectedItem.Value = "Invoice" Then
                dsApprovalList = objAppWF.getPaymentAppGrpList()
                intCnt = dsApprovalList.Tables(0).Rows.Count

                If dsApprovalList.Tables(0).Rows.Count > 0 Then
                    cboApproval.Items.Clear()
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
                    lblMsg.Text = "There is no approval list available."
                End If
            Else
                dsApprovalList = objAppWF.getIPPAppGrpList()
                dsApprovalList = objAppWF.getIPPAppGrpList()
                If dsApprovalList.Tables(0).Rows.Count > 0 Then
                    cboIPPApproval.Items.Clear()
                    Common.FillDdl(cboIPPApproval, "AGM_GRP_NAME", "AGM_GRP_INDEX", dsApprovalList)
                    Dim lstItem As New ListItem
                    ' Add ---Select---
                    lstItem.Value = ""
                    lstItem.Text = "---Select---"
                    cboIPPApproval.Items.Insert(0, lstItem)
                    lblMsg.Visible = False
                    If dsApprovalList.Tables(0).Rows.Count = 1 Then '//Display approving officer if only has one approval workflow
                        cboIPPApproval.SelectedIndex = 1
                        cboIPPApproval_SelectedIndexChanged(sender, e)
                    End If
                Else
                    cboIPPApproval.Visible = False
                    lblRemark.Visible = False
                    lblMsg1.Visible = True
                    lblMsg1.Text = "There is no approval list available."
                End If
            End If                        

            If strMode = "update" Then
                Me.txt_deptCode.Text = Me.Request.QueryString("deptcode")
                Me.txt_deptCode.Enabled = False
                Me.txt_deptName.Text = Me.Request.QueryString("deptname")
                ViewState("oldname") = Me.Request.QueryString("deptname")
                Me.txt_deptName.Enabled = True

                If Request.QueryString("appgrpindex") <> "" Then
                    Try
                        If cboApprovalType.SelectedItem.Value = "Invoice" Then
                            'Me.cboApproval.SelectedValue = Request.QueryString("appgrpindex")
                        End If
                    Catch ex As Exception
                        Me.cboApproval.SelectedIndex = 0
                    End Try
                    Bindgrid()
                End If

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
    Sub getIPPApprovalindex(ByVal index As String)
        Dim objdb As New EAD.DBCom
        ViewState("IPPApprovalIndex") = objdb.Get1ColumnCheckNull("COMPANY_DEPT_MSTR", "CDM_IPP_APPROVAL_GRP_INDEX", " WHERE CDM_DEPT_INDEX='" & intIndex & "'")
    End Sub

    Private Function Bindgrid() As String
        Dim objApp As New ApprWorkFlow
        Dim dsAO As New DataSet
        Dim dvViewSample As DataView

        If cboApprovalType.SelectedItem.Value = "Invoice" Then
            If cboApproval.Visible = True Then

                'Zulham 09102018 - PAMB P2P
                If Not ViewState("deptCode") Is Nothing Then
                    ViewState("deptCode") = ViewState("deptCode") & ",'" & cboApproval.SelectedItem.Value & "'"
                End If

                If cboApproval.SelectedItem.Value <> "" Then
                    'Zulham 09102018 - PAMB
                    'dsAO = objApp.getPaymentAppList(ViewState("deptCode"), "Y")
                    dsAO = Nothing
                    If Not ViewState("deptCode") Is Nothing Then
                        dsAO = objDb.FillDs("Select  AGM_GRP_INDEX, agm_grp_name From APPROVAL_GRP_MSTR WHERE AGM_RESIDENT ='Y' and AGM_GRP_INDEX in (" & ViewState("deptCode") & ")")
                    Else
                        dsAO = objDb.FillDs("Select  AGM_GRP_INDEX, agm_grp_name From APPROVAL_GRP_MSTR WHERE AGM_RESIDENT ='Y' and AGM_GRP_INDEX in (" & cboApproval.SelectedValue & ")")
                        ViewState("deptCode") = "" & cboApproval.SelectedValue & ""
                    End If
                    'dtgAO.Columns(5).Visible = True
                    If dsAO.Tables(0).Rows.Count > 0 Then
                        ViewState("groupIndex_R") = cboApproval.SelectedValue
                        ViewState("intPageRecordCnt") = dsAO.Tables(0).Rows.Count
                        dvViewSample = dsAO.Tables(0).DefaultView
                        intPageRecordCnt = ViewState("intPageRecordCnt")
                        dtgAO.DataSource = dvViewSample
                        dtgAO.DataBind()
                    End If
                    'ViewState("blnSubmit") = False

                    'Zulham 09102018 - PAMB
                    'dsAO = objApp.getPaymentAppList(ViewState("deptCode"), "N")
                    If Not ViewState("deptCode") Is Nothing Then
                        dsAO = objDb.FillDs("Select  AGM_GRP_INDEX, agm_grp_name From APPROVAL_GRP_MSTR WHERE AGM_RESIDENT ='N' and AGM_GRP_INDEX in (" & ViewState("deptCode") & ")")
                    Else
                        dsAO = objDb.FillDs("Select  AGM_GRP_INDEX, agm_grp_name From APPROVAL_GRP_MSTR WHERE AGM_RESIDENT ='N' and AGM_GRP_INDEX in (" & cboApproval.SelectedValue & ")")
                        ViewState("deptCode") = "" & cboApproval.SelectedValue & ""
                    End If
                    'dtgAO_NR.Columns(5).Visible = True
                    If dsAO.Tables(0).Rows.Count > 0 Then
                        ViewState("groupIndex_NR") = cboApproval.SelectedValue
                        ViewState("intPageRecordCnt_NR") = dsAO.Tables(0).Rows.Count
                        dvViewSample = dsAO.Tables(0).DefaultView
                        intPageRecordCnt = ViewState("intPageRecordCnt")
                        dtgAO_NR.DataSource = dvViewSample
                        dtgAO_NR.DataBind()
                    End If
                    ViewState("blnSubmit") = False
                    'End
                Else
                    'Zulham 16072018 - PAMB
                    Dim objDb As New EAD.DBCom
                    'Zulham 11102018 - PAMB
                    Dim dsDept As New DataSet
                    Dim idx As String = ""
                    dsDept = objDb.FillDs("Select AGM_GRP_INDEX From APPROVAL_GRP_MSTR JOIN COMPANY_DEPT_MSTR ON AGM_GRP_INDEX = CDM_APPROVAL_GRP_INDEX WHERE AGM_RESIDENT ='Y' and cdm_dept_code = '" & txt_deptCode.Text & "'")
                    If Not dsDept.Tables(0).Rows.Count = 0 Then
                        For i As Integer = 0 To dsDept.Tables(0).Rows.Count - 1
                            If idx = "" Then
                                idx = "'" & dsDept.Tables(0).Rows(i).Item(0).ToString & "'"
                            Else
                                idx = idx & ",'" & dsDept.Tables(0).Rows(i).Item(0).ToString & "'"
                            End If
                        Next
                    End If
                    If idx <> "" Then
                        'Zulham 11102018 - PAMB
                        ViewState("deptCode") = idx
                        'dsAO = objApp.getPaymentAppList(idx, "Y")
                        dsAO = objDb.FillDs("Select  AGM_GRP_INDEX, agm_grp_name From APPROVAL_GRP_MSTR JOIN COMPANY_DEPT_MSTR ON AGM_GRP_INDEX = CDM_APPROVAL_GRP_INDEX WHERE AGM_RESIDENT ='Y' and cdm_dept_code = '" & txt_deptCode.Text & "'")
                        'dtgAO.Columns(5).Visible = True
                        If dsAO.Tables(0).Rows.Count > 0 Then
                            ViewState("groupIndex_R") = idx 'Zulham 18072018 - PAMB
                            ViewState("intPageRecordCnt") = dsAO.Tables(0).Rows.Count
                            dvViewSample = dsAO.Tables(0).DefaultView
                            intPageRecordCnt = ViewState("intPageRecordCnt")
                            dtgAO.DataSource = dvViewSample
                            dtgAO.DataBind()
                        End If
                    Else
                        dtgAO.DataSource = Nothing
                        dtgAO.DataBind()
                    End If

                    'Zulham 12102018 - PAMB 
                    'idx = objDb.GetVal("Select cdm_approval_grp_index From APPROVAL_GRP_MSTR JOIN COMPANY_DEPT_MSTR ON AGM_GRP_INDEX = CDM_APPROVAL_GRP_INDEX WHERE AGM_RESIDENT ='N' and cdm_dept_code = '" & txt_deptCode.Text & "'")
                    dsDept = objDb.FillDs("Select AGM_GRP_INDEX From APPROVAL_GRP_MSTR JOIN COMPANY_DEPT_MSTR ON AGM_GRP_INDEX = CDM_APPROVAL_GRP_INDEX WHERE AGM_RESIDENT ='N' and cdm_dept_code = '" & txt_deptCode.Text & "'")
                    If Not dsDept.Tables(0).Rows.Count = 0 Then
                        For i As Integer = 0 To dsDept.Tables(0).Rows.Count - 1
                            If idx = "" Then
                                idx = "'" & dsDept.Tables(0).Rows(i).Item(0).ToString & "'"
                            Else
                                idx = idx & ",'" & dsDept.Tables(0).Rows(i).Item(0).ToString & "'"
                            End If
                        Next
                    End If
                    If idx <> "" Then
                        'Zulham 09102018 - PAMB
                        ViewState("deptCode") = "" & idx & ""

                        'dsAO = objApp.getPaymentAppList(idx, "N")
                        dsAO = objDb.FillDs("Select  AGM_GRP_INDEX, agm_grp_name From APPROVAL_GRP_MSTR JOIN COMPANY_DEPT_MSTR ON AGM_GRP_INDEX = CDM_APPROVAL_GRP_INDEX WHERE AGM_RESIDENT ='N' and cdm_dept_code = '" & txt_deptCode.Text & "'")
                        'dtgAO_NR.Columns(5).Visible = True
                        If dsAO.Tables(0).Rows.Count > 0 Then
                            ViewState("groupIndex_NR") = idx 'Zulham 18072018 - PAMB
                            ViewState("intPageRecordCnt_NR") = dsAO.Tables(0).Rows.Count
                            dvViewSample = dsAO.Tables(0).DefaultView
                            intPageRecordCnt = ViewState("intPageRecordCnt")
                            dtgAO_NR.DataSource = dvViewSample
                            dtgAO_NR.DataBind()
                        End If
                    Else
                        dtgAO_NR.DataSource = Nothing
                        dtgAO_NR.DataBind()
                    End If

                    ViewState("blnSubmit") = False
                End If
            End If
        Else
            If cboIPPApproval.Visible = True Then
                If cboIPPApproval.SelectedItem.Value <> "" Then
                    dsAO = objApp.getIPPAppList(cboIPPApproval.SelectedItem.Value)
                    'Zulham 15102018 - PAMB SST
                    'dtgAO.Columns(5).Visible = False
                    ViewState("intPageRecordCnt") = dsAO.Tables(0).Rows.Count
                    dvViewSample = dsAO.Tables(0).DefaultView
                    intPageRecordCnt = ViewState("intPageRecordCnt")
                    dtgAO.DataSource = dvViewSample
                    dtgAO.DataBind()
                    ViewState("blnSubmit") = False
                Else
                    dtgAO.DataSource = Nothing
                    dtgAO.DataBind()
                End If
            End If
        End If

        'Zulham 15102018 - PAMB SST
        dtgAO.Columns(4).Visible = False
        dtgAO_NR.Columns(4).Visible = False

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
            'Zulham 18072018 - PAMB
            dtgAO_NR.DataBind()
            ViewState("groupIndex_NR") = 0
            ViewState("groupIndex_R") = 0
        End If
    End Sub

    Private Sub dtgAO_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAO.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            'Zulham 11102018
            'Static intRow As Integer
            'intRow = intRow + 1
            'e.Item.Cells(0).Text = intRow

            'If IsDBNull(dv("UM_APP_LIMIT")) Then
            '    dv("UM_APP_LIMIT") = 0
            'End If
            Dim objApp As New ApprWorkFlow
            Dim strIdx() = ViewState("deptCode").ToString.Split(",")
            Dim lblFO As Label = e.Item.Cells(1).FindControl("lblFO")
            Dim lblFM As Label = e.Item.Cells(2).FindControl("lblFM")
            Dim dsAO As New DataSet : dsAO = objApp.getPaymentAppList(dv("AGM_GRP_INDEX"), "Y")
            If Not dsAO.Tables(0).Rows.Count = 0 Then
                For i As Integer = 0 To dsAO.Tables(0).Rows.Count - 1
                    If lblFO.Text = "" Then
                        lblFO.Text = dsAO.Tables(0).Rows(i).Item("AO_NAME").ToString
                    Else
                        lblFO.Text = lblFO.Text & "<br>" & dsAO.Tables(0).Rows(i).Item("AO_NAME").ToString
                    End If
                Next
                lblFM.Text = dsAO.Tables(0).Rows(0).Item("FM_NAME").ToString
            End If
        End If
    End Sub

    Private Sub dtgAO_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAO.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgAO, e)

    End Sub

    Public Sub dtgAO_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgAO.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub
#Region "PAMB"
    'Zulham 16072018 
    Private Sub dtgAO_NR_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAO_NR.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)

            'Zulham 11102018
            'Static intRow As Integer
            'intRow = intRow + 1
            'e.Item.Cells(0).Text = intRow

            'If IsDBNull(dv("UM_APP_LIMIT")) Then
            '    dv("UM_APP_LIMIT") = 0
            'End If
            Dim objApp As New ApprWorkFlow
            Dim strIdx() = ViewState("deptCode").ToString.Split(",")
            Dim lblFO As Label = e.Item.Cells(1).FindControl("lblFO")
            Dim lblFM As Label = e.Item.Cells(2).FindControl("lblFM")
            Dim dsAO As New DataSet : dsAO = objApp.getPaymentAppList(dv("AGM_GRP_INDEX"), "N")
            If Not dsAO.Tables(0).Rows.Count = 0 Then
                For i As Integer = 0 To dsAO.Tables(0).Rows.Count - 1
                    If lblFO.Text = "" Then
                        lblFO.Text = dsAO.Tables(0).Rows(i).Item("AO_NAME").ToString
                    Else
                        lblFO.Text = lblFO.Text & "<br>" & dsAO.Tables(0).Rows(i).Item("AO_NAME").ToString
                    End If
                Next
                lblFM.Text = dsAO.Tables(0).Rows(0).Item("FM_NAME").ToString
            End If
        End If
    End Sub

    Private Sub dtgAO_NR_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAO_NR.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgAO_NR, e)
    End Sub

    Public Sub dtgAO_NR_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgAO.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub
    'End
#End Region
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        'vl(True)
        Dim strDCode, strDName, strPaymentGrpIndex, strIPPPaymentGrpIndex As String
        'Dim strRedirect As String
        Dim strMsg As String
        Dim intMsgNo As Integer
        'vldSumm.Visible = True


        Dim objAdmin As New Admin
        strDCode = Me.txt_deptCode.Text
        strDName = Me.txt_deptName.Text
        If cboApprovalType.SelectedItem.Value = "Invoice" Then
            strPaymentGrpIndex = Me.cboApproval.SelectedValue
        Else
            strIPPPaymentGrpIndex = Me.cboIPPApproval.SelectedValue
        End If

        If strMode = "add" Then
            'intMsgNo = objAdmin.addDept(strDCode, strDName, strPaymentGrpIndex, strIPPPaymentGrpIndex)
            'Zulham 14082018 - PAMB
            'Zulham 15102018 - PAMB
            If ViewState("groupIndex_NR") Is Nothing Then ViewState("groupIndex_NR") = ""
            If ViewState("groupIndex_R") Is Nothing Then ViewState("groupIndex_R") = ""

            'Zulham 14082018 - PAMB
            'Zulham 15102018 - PAMB
            If ViewState("groupIndex_NR") = "" Then
                strMsg = "Please Select workflow For both Resident And Non-Resident"
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                Exit Sub
            End If
            If ViewState("groupIndex_R") = "" Then
                strMsg = "Please Select workflow For both Resident And Non-Resident"
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                Exit Sub
            End If

            'Zulham 11102018 - PAMB SST
            If Not ViewState("deptCode") Is Nothing Then
                Dim objDB As New EAD.DBCom
                Dim str() = ViewState("deptCode").ToString.Split(",")
                For i As Integer = 0 To str.Length - 1
                    Dim residentType = objDB.GetVal("Select agm_resident FROM approval_grp_mstr WHERE agm_grp_index = '" & str(i).ToString.ToString.Replace("'", "") & "'")
                    If residentType = "Y" Then
                        intMsgNo = objAdmin.addDept(strDCode, strDName, str(i).ToString.ToString.Replace("'", ""), "", 0)
                    Else
                        intMsgNo = objAdmin.addDept(strDCode, strDName, 0, "", str(i).ToString.ToString.Replace("'", ""))
                    End If
                Next
            End If

            Select Case intMsgNo
                Case WheelMsgNum.Save
                    'Zulham 14082018 - PAMB
                    objAdmin.delDept(strDCode)
                    strMsg = MsgRecordSave
                    If cboApproval.Visible = True Then cboApproval.SelectedIndex = 0
                    ViewState("mode") = "modify"
                    txt_deptCode.Enabled = False
                Case WheelMsgNum.Duplicate
                    strMsg = MsgRecordDuplicate
                Case WheelMsgNum.NotSave
                    strMsg = MsgRecordNotSave
            End Select
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        Else
            'Zulham 16072018 - PAMB
            'intMsgNo = objAdmin.moddept(strDCode, strDName, ViewState("oldname"), strPaymentGrpIndex, strIPPPaymentGrpIndex, 0)
            'Zulham 15102018 - PAMB
            If ViewState("groupIndex_NR") Is Nothing Then ViewState("groupIndex_NR") = ""
            If ViewState("groupIndex_R") Is Nothing Then ViewState("groupIndex_R") = ""

            'Zulham 18072018 - PAMB
            'Zulham 15102018 - PAMB
            If ViewState("groupIndex_NR") = "" Then
                strMsg = "Please select workflow for both Resident and Non-Resident"
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                Exit Sub
            End If
            If ViewState("groupIndex_R") = "" Then
                strMsg = "Please select workflow for both Resident and Non-Resident"
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                Exit Sub
            End If

            'Zulham 15102018 - PAMB SST
            If Not ViewState("deptCode") Is Nothing Then
                Dim objDB As New EAD.DBCom
                Dim str() = ViewState("deptCode").ToString.Split(",")
                For i As Integer = 0 To str.Length - 1
                    Dim residentType = objDB.GetVal("SELECT agm_resident FROM approval_grp_mstr WHERE agm_grp_index = " & str(i).ToString.Replace("'", "") & "")
                    If residentType = "Y" Then
                        intMsgNo = objAdmin.moddept(strDCode, strDName, ViewState("oldname"), str(i).ToString.Replace("'", ""), strIPPPaymentGrpIndex, 0)
                    Else
                        intMsgNo = objAdmin.moddept(strDCode, strDName, ViewState("oldname"), 0, strIPPPaymentGrpIndex, str(i).ToString.Replace("'", ""))
                    End If
                Next
            End If

            Select Case intMsgNo
                Case WheelMsgNum.Save
                    'Zulham 14082018 - PAMB
                    objAdmin.delDept(strDCode)
                    strMsg = MsgRecordSave
                Case WheelMsgNum.Duplicate
                    strMsg = MsgRecordDuplicate
                Case WheelMsgNum.NotSave
                    strMsg = MsgRecordNotSave
            End Select
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        End If

        objAdmin = Nothing
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

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
                Common.NetMsgbox(Me, MsgRecordDelete, dDispatcher.direct("Admin", "DeptSetup.aspx", "side=" & ViewState("Side") & "&pageid=" & strPageId), MsgBoxStyle.Information)
            Case WheelMsgNum.NotDelete
                strMsg = MsgRecordNotDelete
                Common.NetMsgbox(Me, MsgRecordNotDelete, MsgBoxStyle.Information)
        End Select

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
        Session("w_AddDept_tabs") = "<div class=""t_entity""><ul>" &
                       "<li><div class=""space""></div></li>" &
                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Companies", "coCompanyDetail.aspx", "side=BUYER&mode=modify" & "&pageid=" & strPageId) & """><span>Company Details</span></a></li>" &
                       "<li><div class=""space""></div></li>" &
                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "BComParam.aspx", "side=BUYER" & "&pageid=" & strPageId) & """><span>Parameters</span></a></li>" &
                       "<li><div class=""space""></div></li>" &
                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=D&mod=T" & "&pageid=" & strPageId) & """><span>Delivery Address</span></a></li>" &
                       "<li><div class=""space""></div></li>" &
                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Admin", "AddressMaster.aspx", "side=BUYER&type=B&mod=T" & "&pageid=" & strPageId) & """><span>Billing Address</span></a></li>" &
                       "<li><div class=""space""></div></li>" &
                       "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Admin", "DeptSetup.aspx", "side=BUYER" & "&pageid=" & strPageId) & """><span>Department</span></a></li>" &
                       "<li><div class=""space""></div></li>" &
                       "</ul><div></div></div>"
    End Sub

    Private Sub cboApprovalType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboApprovalType.SelectedIndexChanged
        Dim dsApprovalList As New DataSet
        Dim objAppWF As New ApprWorkFlow
        If cboApprovalType.SelectedItem.Value = "Invoice" Then
            Me.TRIPP.Style("display") = "none"
            Me.hidrow.Style("display") = ""
            dsApprovalList = objAppWF.getPaymentAppGrpList()
            If dsApprovalList.Tables(0).Rows.Count > 0 Then
                cboApproval.Items.Clear()
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
                If Request.QueryString("appgrpindex") <> "0" Then
                    cboApproval.SelectedValue = Request.QueryString("appgrpindex")
                End If
            Else
                cboApproval.Visible = False
                lblRemark.Visible = False
                lblMsg.Visible = True
                lblMsg.Text = "There is no approval list available."
            End If
        Else
            Me.TRIPP.Style("display") = ""
            Me.hidrow.Style("display") = "none"
            dsApprovalList = objAppWF.getIPPAppGrpList()
            If dsApprovalList.Tables(0).Rows.Count > 0 Then
                cboIPPApproval.Items.Clear()
                Common.FillDdl(cboIPPApproval, "AGM_GRP_NAME", "AGM_GRP_INDEX", dsApprovalList)
                Dim lstItem As New ListItem
                ' Add ---Select---
                lstItem.Value = ""
                lstItem.Text = "---Select---"
                cboIPPApproval.Items.Insert(0, lstItem)
                lblMsg.Visible = False
                If dsApprovalList.Tables(0).Rows.Count = 1 Then '//Display approving officer if only has one approval workflow
                    cboIPPApproval.SelectedIndex = 1
                    cboIPPApproval_SelectedIndexChanged(sender, e)
                End If
                If ViewState("IPPApprovalIndex") <> "" Then
                    cboIPPApproval.SelectedValue = ViewState("IPPApprovalIndex")                    
                End If
            Else
                cboIPPApproval.Visible = False
                lblRemark.Visible = False
                lblMsg1.Visible = True
                lblMsg1.Text = "There is no approval list available."
            End If
        End If
        Bindgrid()

    End Sub

    Private Sub cboIPPApproval_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboIPPApproval.SelectedIndexChanged
        If Not Page.IsPostBack Then Exit Sub

        If cboIPPApproval.SelectedItem.Value <> "" Then
            Bindgrid()
        Else
            lblRemark.Text = ""
            dtgAO.DataBind()
        End If
    End Sub
End Class
