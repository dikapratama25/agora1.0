Imports AgoraLegacy
Imports eProcure.Component

Public Class ApprovalSetup2
    Inherits AgoraLegacy.AppBaseClass

    Public Enum enumApp
        icLevel = 0
        icAO = 1
        icAOName = 2
        icAAO = 3
        icAAOName = 4
        icType = 5
    End Enum

    Public strAryConsolidator() As String
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents rfvApproval As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents lblConsolidator As System.Web.UI.WebControls.Label
    Protected WithEvents trConsolidator As System.Web.UI.HtmlControls.HtmlTableRow
    Dim intRow As Integer
    Dim dDispatcher As New AgoraLegacy.dispatcher


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cboApproval As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblPR As System.Web.UI.WebControls.Label
    Protected WithEvents dtgAO As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblRemark As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink

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
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)
        blnSorting = False
        blnPaging = False
        SetGridProperty(dtgAO)

        ViewState("blnCutPO") = False
        If Not IsPostBack Then
            intRow = 0
            lblTitle.Text = "Approval Setup"
            ViewState("prid") = Request.QueryString("prid")
            ViewState("prcost") = Request.QueryString("prcost")
            ViewState("msg") = Request.QueryString("msg")
            lblPR.Text = ViewState("prid")

            Dim objPR As New PR
            ViewState("ApprovalType") = objPR.getApprovalType

            Dim dsApprovalList As New DataSet
            Dim intCnt As Integer
            Dim i As Integer
            Dim strConsolidator As String = ""
            dsApprovalList = objPR.getAppovalList("A", CDbl(ViewState("prcost")), "PR", False)
            intCnt = dsApprovalList.Tables(0).Rows.Count
            If intCnt > 0 Then
                For i = 0 To intCnt - 1
                    strConsolidator &= dsApprovalList.Tables(0).Rows(i)("AGM_CONSOLIDATOR") & ","
                Next
                ViewState("consolidator") = strConsolidator.Substring(0, strConsolidator.Length - 1)
            Else
                ViewState("consolidator") = ""
            End If

            trConsolidator.Visible = False
            If dsApprovalList.Tables(0).Rows.Count > 0 Then
                Common.FillDdl(cboApproval, "AGM_GRP_NAME", "AGA_GRP_INDEX", dsApprovalList)
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
                lblMsg.Text = "There is no approval list available for this PR because the sequence of approving officers do not have the approval limit to approve it."
                cmdSubmit.Visible = False
                rfvApproval.Enabled = False
            End If
        End If
        ' _Yap Comment Out
        ' lnkBack.NavigateUrl = dDispatcher.direct("PR", "RaisePR.aspx", "type=list&prid=" & ViewState("prid") & "&pageid=" & strPageId)
        lnkBack.NavigateUrl = dDispatcher.direct("PR", "RaisePR.aspx", "mode=" & Request.QueryString("mode") & "&type=mod&prid=" & ViewState("prid") & "&pageid=" & strPageId)

    End Sub

    Private Function Bindgrid() As String
        Dim objPR As New PR
        Dim dsAO As New DataSet
        Dim dvViewSample As DataView

        dsAO = objPR.getAOList(cboApproval.SelectedItem.Value)
        ViewState("intPageRecordCnt") = dsAO.Tables(0).Rows.Count
        dvViewSample = dsAO.Tables(0).DefaultView
        intPageRecordCnt = ViewState("intPageRecordCnt")

        dtgAO.DataSource = dvViewSample
        dtgAO.DataBind()
        ViewState("blnSubmit") = False

        If intPageRecordCnt > 0 Then
            displayRemark()
        Else
            lblRemark.Text = ""
        End If

        If dsAO.Tables(1).Rows.Count > 0 Then
            If Common.parseNull(dsAO.Tables(1).Rows(0)("AGM_CONSOLIDATOR")) <> "" Then
                trConsolidator.Visible = True
                lblConsolidator.Text = Common.parseNull(dsAO.Tables(1).Rows(0)("UM_USER_NAME"))
            Else
                trConsolidator.Visible = False
            End If
        Else
            trConsolidator.Visible = False
        End If

    End Function

    Public Sub dtgAO_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgAO.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub cboApproval_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboApproval.SelectedIndexChanged
        If cboApproval.SelectedItem.Value <> "" Then
            Bindgrid()
        Else
            lblRemark.Text = ""
            trConsolidator.Visible = False
            dtgAO.DataBind()
        End If
    End Sub

    Private Sub dtgAO_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAO.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            intRow = intRow + 1
            e.Item.Cells(enumApp.icLevel).Text = intRow

            Dim lblLimit As Label
            lblLimit = e.Item.FindControl("lblLimit")
            lblLimit.Text = Common.parseNull(dv("UM_APP_LIMIT"))

            Dim lblType As Label
            lblType = e.Item.FindControl("lblType")

            Dim lblRelief As Label
            lblRelief = e.Item.FindControl("lblRelief")
            lblRelief.Text = Common.parseNull(dv("AGA_RELIEF_IND"))

            ' case Automated Approval
            If IsDBNull(dv("UM_APP_LIMIT")) Then
                dv("UM_APP_LIMIT") = 0
            End If

            ' A - Automated Approval
            ' B - Allow Lower Limit Endorsement
            ' C - Cut PO before end of Aproval List
            ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
            Select Case ViewState("ApprovalType")
                Case "C"
                    If CDbl(ViewState("prcost")) < CDbl(dv("UM_APP_LIMIT")) Then
                        If ViewState("blnCutPO") = True Then
                            lblType.Text = "None"
                        Else
                            lblType.Text = "Approval"
                            ViewState("blnCutPO") = True
                            'viewstate("blnSubmit") = True
                        End If
                    Else
                        lblType.Text = "None"
                    End If

                Case "B"
                    'Michelle (10/8/2007) - should be 'Approval' if PR cost is the same as the limit
                    ' If CDbl(viewstate("prcost")) < CDbl(dv("UM_APP_LIMIT")) Then
                    If CDbl(ViewState("prcost")) <= CDbl(dv("UM_APP_LIMIT")) Then
                        lblType.Text = "Approval"
                        'viewstate("blnSubmit") = True
                    Else
                        lblType.Text = "Approval"
                    End If

                Case "B+C"
                    'Michelle (10/8/2007) - should be 'Approval' if PR cost is the same as the limit
                    'If CDbl(viewstate("prcost")) < CDbl(dv("UM_APP_LIMIT")) Then
                    If CDbl(ViewState("prcost")) <= CDbl(dv("UM_APP_LIMIT")) Then
                        If ViewState("blnCutPO") = True Then
                            lblType.Text = "None"
                        Else
                            lblType.Text = "Approval"
                            ViewState("blnCutPO") = True
                            'viewstate("blnSubmit") = True
                        End If
                    Else
                        lblType.Text = "Approval"
                    End If

                Case "A"
                    If CDbl(ViewState("prcost")) < CDbl(dv("UM_APP_LIMIT")) Then
                        lblType.Text = "Approval"
                        'viewstate("blnSubmit") = True
                    Else
                        lblType.Text = "None"
                    End If

            End Select
        End If
    End Sub

    Private Sub dtgAO_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAO.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgAO, e)
    End Sub

    Private Sub displayRemark()
        Dim str As String
        'Select Case viewstate("ApprovalType")
        'Case "A"
        '    str = "* There are basically 2 types of approval for this automated approval sequence setup : "
        '    str &= "<ul type='disc'>"
        '    str &= "<li><strong>Approval</strong> - PR will be sent to the respective approving officer for approval</li> "
        '    str &= "<li><strong>None</strong> - Approving officer has no authority to approve/endorse this PR</li></ul>"

        'Case "B"
        '    str = "* There are basically 2 types of approval for this allow lower limit endorsement sequence setup : "
        '    str &= "<ul type='disc'>"
        '    str &= "<li><strong>Approval</strong> - PR will be sent to the respective approving officer for approval</li> "
        '    str &= "<li><strong>Endorsement</strong> - PR will be sent to the respective approving officer for endorsement</li></ul> "

        'Case "C"
        '    str = "* There are basically 2 types of approval for this cut PO before end of approval list sequence setup : "
        '    str &= "<ul type='disc'>"
        '    str &= "<li><strong>Approval</strong> - PR will be sent to the respective approving officer for approval</li> "
        '    str &= "<li><strong>None</strong> - Approving officer has no authority to approve/endorse this PR</li></ul>"

        'Case "B+C"
        str = "* There are basically 3 types of approval for this approval sequence setup : "
        str &= "<ul type='disc'>"
        str &= "<li><strong>Approval</strong> - PR will be sent to the respective approving officer for approval</li> "
        str &= "<li><strong>Endorsement</strong> - PR will be sent to the respective approving officer for endorsement</li> "
        str &= "<li><strong>None</strong> - Approving officer has no authority to approve/endorse this PR</li></ul>"

        ' End Select

        lblRemark.Text = str
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        If cboApproval.SelectedItem.Value <> "" Then
            Dim objPR As New PR
            Dim objPurchaseReq2 As New PurchaseReq2
            Dim dtAO As New DataTable
            Dim strConsolidator As String
            Dim intMsg As Integer
            dtAO = bindAO()
            strConsolidator = ViewState("consolidator")
            'Michelle (13/7/2007) - User is not allowed to select a list with Consolidator if the vendor id in the POM_S_COY_ID is blank (ie with mulitple PO)
            If trConsolidator.Visible = True Then 'ie with consolidator
                Dim strVendor As String
                strVendor = objPurchaseReq2.getPRVendor(ViewState("prid"))
                If strVendor = Nothing Then
                    Common.NetMsgbox(Me, "Not allow to have Consolidator for this PR", MsgBoxStyle.Exclamation)
                    Exit Sub
                End If
            End If

            strAryConsolidator = strConsolidator.Split(",")
            ' _Yap Comment Out
            ' intMsg = objPR.updatePRStatus(ViewState("prid"), PRStatus.Submitted, strAryConsolidator(cboApproval.SelectedIndex - 1), dtAO, ViewState("msg"))
            cmdSubmit.Visible = False
            intMsg = objPR.submitPR(ViewState("prid"), PRStatus.Submitted, Nothing, dtAO, ViewState("msg"), "", False, False)
            Select Case intMsg
                'Case WheelMsgNum.NotSave
                '    Response.Redirect(dDispatcher.direct("PR", "PRConfirm.aspx", "msg=2&type=A&prid=" & ViewState("prid")))
                'Case WheelMsgNum.Save
                '    Response.Redirect(dDispatcher.direct("PR", "PRConfirm.aspx", "msg=" & ViewState("msg") & "&type=A&prid=" & ViewState("prid")))
                'Case WheelMsgNum.Delete
                '    Response.Redirect(dDispatcher.direct("PR", "PRConfirm.aspx", "msg=3&type=A&prid=" & ViewState("prid")))

                Case WheelMsgNum.Save
                    If Session("urlreferer") = "BuyerCatSearch" Then
                        Common.NetMsgbox(Me, "Purchase Request Number " & ViewState("prid") & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "ConCatSearch" Then
                        Common.NetMsgbox(Me, "Purchase Request Number " & ViewState("prid") & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "PRAll" Then
                        Common.NetMsgbox(Me, "Purchase Request Number " & ViewState("prid") & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "Dashboard" Then
                        Common.NetMsgbox(Me, "Purchase Request Number " & ViewState("prid") & " has been submitted.", dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId))
                    End If
            End Select
            cmdSubmit.Visible = True
        End If
    End Sub

    Private Function bindAO() As DataTable
        Dim dgItem As DataGridItem
        Dim dtAO As New DataTable
        Dim i As Integer = 0
        dtAO.Columns.Add("prid", Type.GetType("System.String"))
        dtAO.Columns.Add("AO", Type.GetType("System.String"))
        dtAO.Columns.Add("AAO", Type.GetType("System.String"))
        dtAO.Columns.Add("Seq", Type.GetType("System.Int32"))
        dtAO.Columns.Add("Type", Type.GetType("System.String"))
        dtAO.Columns.Add("GrpIndex", Type.GetType("System.String"))
        dtAO.Columns.Add("Relief", Type.GetType("System.String"))

        For Each dgItem In dtgAO.Items
            Dim dtr As DataRow
            Dim strType As String
            dtr = dtAO.NewRow()
            dtr("prid") = ViewState("prid")
            dtr("AO") = dgItem.Cells(enumApp.icAO).Text
            dtr("AAO") = dgItem.Cells(enumApp.icAAO).Text

            If dtr("AAO") = "&nbsp;" Then
                dtr("AAO") = ""
            End If



            ' Todo: sTUDY THIS FUNCTION!!!!!


            strType = CType(dgItem.FindControl("lblType"), Label).Text
            Select Case strType
                Case "None"
                    dtr("Type") = "0"
                Case "Approval"
                    dtr("Type") = "1"
                    i = i + 1
                Case "Endorsement"
                    dtr("Type") = "2"
                    i = i + 1
            End Select

            dtr("Seq") = i 'dgItem.Cells(enumApp.icLevel).Text
            dtr("GrpIndex") = cboApproval.SelectedItem.Value
            dtr("Relief") = CType(dgItem.FindControl("lblRelief"), Label).Text

            If dtr("Type") <> "0" Then
                dtAO.Rows.Add(dtr)
            End If
        Next
        bindAO = dtAO
    End Function

End Class