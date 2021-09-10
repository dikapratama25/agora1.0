'Zulham 30-01-2015 IPP-GST Stage 2A
Imports AgoraLegacy
Imports eProcure.Component

Public Class BillingApprovalSetup
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
    Dim objdb As New EAD.DBCom


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cboApproval As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblIPP As System.Web.UI.WebControls.Label
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

        Dim dsDoc As New DataSet
        dsDoc = Session("dsDOC")
        ViewState("frm") = Request.QueryString("frm")
        ViewState("olddocno") = Request.QueryString("olddocno")
        lblIPP.Text = Request.QueryString("docno")

        If Session("DocNo") Is Nothing Or Session("DocNo") = 0 Then
            Session("DocNo") = objdb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & ViewState("olddocno") & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND IM_S_COY_ID = '" & dsDoc.Tables(0).Rows(0)("VenCompIDX") & "'")
        End If

        If Not IsPostBack Then
            intRow = 0
            lblTitle.Text = "Approval Setup"


            Dim objIPP As New IPPMain


            Dim dsApprovalList As New DataSet
            Dim intCnt As Integer
            Dim i As Integer
            Dim strConsolidator As String = ""
            dsApprovalList = objIPP.getIPPApprovalWorkflowList

            If dsApprovalList.Tables(0).Rows.Count > 0 Then
                Common.FillDdl(cboApproval, "AGM_GRP_NAME", "AGB_GRP_INDEX", dsApprovalList)
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
            End If
        End If
        lnkBack.NavigateUrl = dDispatcher.direct("Billing", "BillingEntry.aspx", "mode=modify&urlrefereripp=" & Session("urlrefereripp") & "&index=" & Session("DocNo") & "&DocumentNo=" & lblIPP.Text & "&pageid=" & strPageId)
    End Sub

    Private Function Bindgrid() As String
        Dim objIPP As New IPPMain
        Dim dsWorkflow As New DataSet
        Dim dvViewSample As DataView

        dsWorkflow = objIPP.getIPPApprovalWorkflow(cboApproval.SelectedItem.Value)
        ViewState("intPageRecordCnt") = dsWorkflow.Tables(0).Rows.Count
        dvViewSample = dsWorkflow.Tables(0).DefaultView
        intPageRecordCnt = ViewState("intPageRecordCnt")

        dtgAO.DataSource = dvViewSample
        dtgAO.DataBind()
        ViewState("blnSubmit") = False

    End Function

    Public Sub dtgAO_Page(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dtgAO.CurrentPageIndex = e.NewPageIndex
        Bindgrid()
    End Sub

    Private Sub cboApproval_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboApproval.SelectedIndexChanged
        If cboApproval.SelectedItem.Value <> "" Then
            Bindgrid()
        Else
            'lblRemark.Text = ""
            'trConsolidator.Visible = False
            dtgAO.DataBind()
        End If
    End Sub

    Private Sub dtgAO_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAO.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim dv As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim strAAName, strAAName2, strAAName3, strAAName4 As String


            If Common.parseNull(dv("AAO_NAME")) = "" Then
                strAAName = ""
            Else
                strAAName = Common.parseNull(dv("AAO_NAME")) & "<br>"
            End If


            If Common.parseNull(dv("AAO_NAME2")) = "" Then
                strAAName2 = ""
            Else
                strAAName2 = Common.parseNull(dv("AAO_NAME2")) & "<br>"
            End If


            If Common.parseNull(dv("AAO_NAME3")) = "" Then
                strAAName3 = ""
            Else
                strAAName3 = Common.parseNull(dv("AAO_NAME3")) & "<br>"
            End If


            If Common.parseNull(dv("AAO_NAME4")) = "" Then
                strAAName4 = ""
            Else
                strAAName4 = Common.parseNull(dv("AAO_NAME4")) & "<br>"
            End If

            e.Item.Cells(enumApp.icAAOName).Text = strAAName & _
                                                strAAName2 & _
                                                strAAName3 & _
                                                strAAName4
        End If
    End Sub

    Private Sub dtgAO_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAO.ItemCreated
        intPageRecordCnt = ViewState("intPageRecordCnt")
        Grid_ItemCreated(dtgAO, e)
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        If cboApproval.SelectedItem.Value <> "" Then
            Dim objIPP As New Billing
            Dim objPurchaseReq2 As New PurchaseReq2
            Dim dtAO As New DataTable
            Dim strConsolidator As String
            Dim intMsg As Integer
            Dim objGlobal As New AppGlobals
            Dim strMsg As String
            cmdSubmit.Visible = False

            If objIPP.SaveBillingDoc(Session("dsDOC"), ViewState("frm"), Session("Action"), Session("DocNo"), ViewState("olddocno"), cboApproval.SelectedItem.Value) Then
                strMsg = objGlobal.GetErrorMessage("00024")
                strMsg = "Document " & strMsg
                If Session("urlrefereripp") Is Nothing Or Session("urlrefereripp") = "" Then
                    Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId), MsgBoxStyle.Information)
                Else
                    Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Billing", "BillingList.aspx", "pageid=" & strPageId), MsgBoxStyle.Information)
                End If
                Session("urlrefereripp") = Nothing
            Else
                strMsg = objGlobal.GetErrorMessage("00002")
                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                Exit Sub

            End If
            Session("dsDOC") = Nothing
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
