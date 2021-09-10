Imports AgoraLegacy
Imports eProcure.Component

Public Class SCApprovalSetup
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDB As New EAD.DBCom
    Dim objGlobal As New AppGlobals
    Dim objStaffClaim As New eProcStaffClaim

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

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cboApproval As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblSC As System.Web.UI.WebControls.Label
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

        If Not IsPostBack Then
            intRow = 0
            ViewState("index") = Request.QueryString("index")
            ViewState("scno") = Request.QueryString("scno")
            ViewState("sm") = Request.QueryString("sm")
            lblTitle.Text = "Raise Staff Claim"
            lblSC.Text = ViewState("scno")
            GenerateTab()

            Dim dsApprovalList As New DataSet
            dsApprovalList = objStaffClaim.getSCApprFlow()

            If dsApprovalList.Tables(0).Rows.Count > 0 Then
                Common.FillDdl(cboApproval, "AGM_GRP_NAME", "AGM_GRP_INDEX", dsApprovalList)
                lblMsg.Visible = False
                If dsApprovalList.Tables(0).Rows.Count = 1 Then '//Display approving officer if only has one approval workflow
                    cboApproval.SelectedIndex = 1
                    cboApproval_SelectedIndexChanged(sender, e)
                Else
                    If cboApproval.SelectedItem.Value <> "" Then
                        Bindgrid()
                    Else
                        lblRemark.Text = ""
                        dtgAO.DataBind()
                    End If
                End If
            Else
                cboApproval.Visible = False
                lblRemark.Visible = False
                lblMsg.Visible = True
                lblMsg.Text = "There is no Approval Flow defined for you."
                cmdSubmit.Visible = False
                rfvApproval.Enabled = False
            End If
        End If
        lnkBack.NavigateUrl = dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "pageid=" & strPageId & "&index=" & ViewState("index"))
        Session("urlreferer") = "SCApprovalSetup"
    End Sub

    Private Sub Bindgrid()
        Dim dsAO As New DataSet
        Dim dvViewSample As DataView

        dsAO = objStaffClaim.getSCAOList(cboApproval.SelectedItem.Value)
        ViewState("intPageRecordCnt") = dsAO.Tables(0).Rows.Count
        dvViewSample = dsAO.Tables(0).DefaultView
        intPageRecordCnt = ViewState("intPageRecordCnt")

        dtgAO.DataSource = dvViewSample
        dtgAO.DataBind()

        If intPageRecordCnt > 0 Then
            displayRemark()
        Else
            lblRemark.Text = ""
        End If

    End Sub

    Public Sub dtgAO_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgAO.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub cboApproval_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboApproval.SelectedIndexChanged
        intRow = 0

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

            intRow = intRow + 1
            e.Item.Cells(enumApp.icLevel).Text = intRow

            Dim lblType As Label
            lblType = e.Item.FindControl("lblType")
            lblType.Text = "Approval"

            Dim lblRelief As Label
            lblRelief = e.Item.FindControl("lblRelief")
            lblRelief.Text = Common.parseNull(dv("AGA_RELIEF_IND"))

        End If
    End Sub

    Private Sub dtgAO_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAO.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgAO, e)
    End Sub

    Private Sub displayRemark()
        Dim str As String

        str = "<ul type='disc'>"
        str &= "<li><strong>Approval</strong> - SC will be sent to the respective approving officer for approval </li></ul>"

        lblRemark.Text = str
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        If cboApproval.SelectedItem.Value <> "" Then
            Dim dtAO As New DataTable
            Dim intMsg As Integer

            intMsg = objStaffClaim.submitSC(ViewState("index"), cboApproval.SelectedItem.Value, ViewState("sm")) 'mimi : 28/03/2017

            Select Case intMsg
                Case WheelMsgNum.Save
                    lblMsg.Text = ""
                    cmdSubmit.Visible = False
                    Session("urlreferer") = Nothing
                    Common.NetMsgbox(Me, "Staff Claim Number " & lblSC.Text & " " & objGlobal.GetErrorMessage("00024"), dDispatcher.direct("StaffClaim", "TransportationClaim.aspx", "pageid=" & strPageId))

                Case WheelMsgNum.NotSave
                    Common.NetMsgbox(Me, objGlobal.GetErrorMessage("00007"), MsgBoxStyle.Information)
            End Select
        End If
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_Staff_Claim_tabs") = "<div class=""t_entity""><ul>" & _
                                       "<li><div class=""space""></div></li>" & _
                                       "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("StaffClaim", "TransportationClaim.aspx", "pageid=" & strPageId) & """><span>Staff Claim Documents</span></a></li>" & _
                                       "<li><div class=""space""></div></li>" & _
                                       "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("StaffClaim", "StaffClaimTrackingList.aspx", "pageid=" & strPageId) & """><span>Staff Claim Listing</span></a></li>" & _
                                       "<li><div class=""space""></div></li>" & _
                                       "</ul><div></div></div>"

    End Sub

End Class
