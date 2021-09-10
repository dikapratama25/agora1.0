Imports AgoraLegacy
Imports eProcure.Component

Public Class IPPApprovalSetup
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

        'Zulham 30012019
        If Session("dsDOC") IsNot Nothing Then
            ViewState("dsDOC") = Session("dsDOC")
        Else
            Session("dsDOC") = ViewState("dsDOC")
        End If

        dsDoc = Session("dsDOC")

        ViewState("frm") = Request.QueryString("frm")
        ViewState("olddocno") = Request.QueryString("olddocno")
        lblIPP.Text = Request.QueryString("docno")

        'Zulham 18072018 - PAMB
        'Zulham 05072018 - PAMP
        'Added query for invoice_total
        ViewState("invoiceTotal") = objdb.GetVal("SELECT im_invoice_total FROM invoice_mstr WHERE im_invoice_no = '" & ViewState("olddocno").ToString.Replace("\", "\\") & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND IM_S_COY_ID = '" & dsDoc.Tables(0).Rows(0)("VenCompIDX") & "'")

        If Session("DocNo") Is Nothing Or Session("DocNo") = 0 Then
            Session("DocNo") = objdb.GetVal("SELECT im_invoice_index FROM invoice_mstr WHERE im_invoice_no = '" & ViewState("olddocno").ToString.Replace("\", "\\") & "' and im_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND IM_S_COY_ID = '" & dsDoc.Tables(0).Rows(0)("VenCompIDX") & "'")
        End If

        'viewstate("blnCutPO") = False
        If Not IsPostBack Then
            intRow = 0
            lblTitle.Text = "Approval Setup"


            Dim objIPP As New IPPMain
            Dim dsApprovalList As New DataSet
            Dim intCnt As Integer
            Dim i As Integer
            Dim strConsolidator As String = ""

            'Zulham 26062018 - PAMB
            'Get residence status
            Dim strIsResident = objdb.GetVal("SELECT ifnull(ic_resident_type,"""") FROM IPP_company WHERE ic_index =  '" & Common.Parse(Request.QueryString("venIdx")) & "'")
            ViewState("isResident") = strIsResident

            'Zulham 21092018 - PAMB
            'UAT U00003 
            Dim objAdmin As New Admin
            Dim curr = objdb.GetVal("SELECT im_currency_code FROM invoice_mstr WHERE im_invoice_index =" & Session("DocNo") & "")
            Dim total = objdb.GetVal("SELECT im_invoice_total FROM invoice_mstr WHERE im_invoice_index =" & Session("DocNo") & "")
            Dim dsExchangeRate As New DataSet : dsExchangeRate = objAdmin.getexrate(Common.parseNull(curr), "")
            'Zulham 26092018 - PAMB 
            'UAT U00003
            If Not dsExchangeRate.Tables(0).Rows.Count = 0 Then
                dsApprovalList = objIPP.getIPPApprovalWorkflowList("E2P", strIsResident, CDec(total), CDec(dsExchangeRate.Tables(0).Rows(0).Item(2)))
                ViewState("exchangeRate") = CDec(dsExchangeRate.Tables(0).Rows(0).Item(2))
            Else
                dsApprovalList = objIPP.getIPPApprovalWorkflowList("E2P", strIsResident, CDec(total))
                ViewState("exchangeRate") = 1
            End If
            'End

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
        ' _Yap Comment Out
        ' lnkBack.NavigateUrl = dDispatcher.direct("PR", "RaisePR.aspx", "type=list&prid=" & ViewState("prid") & "&pageid=" & strPageId)

        lnkBack.NavigateUrl = dDispatcher.direct("IPP", "IPPEntry.aspx", "mode=modify&urlrefereripp=" & Session("urlrefereripp") & "&index=" & Session("DocNo") & "&DocumentNo=" & lblIPP.Text & "&pageid=" & strPageId)


    End Sub

    Private Function Bindgrid() As String
        Dim objIPP As New IPPMain
        Dim dsWorkflow As New DataSet
        Dim dvViewSample As DataView

        'Zulham 22012019 - PAMB
        'Workflow for PAMB
        dsWorkflow = objIPP.getIPPApprovalWorkflow(cboApproval.SelectedItem.Value, "E2P", CDec(ViewState("invoiceTotal") * ViewState("exchangeRate")))


        ViewState("intPageRecordCnt") = dsWorkflow.Tables(0).Rows.Count
        dvViewSample = dsWorkflow.Tables(0).DefaultView
        intPageRecordCnt = viewstate("intPageRecordCnt")

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

            '   
        End If


    End Sub

    Private Sub dtgAO_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgAO.ItemCreated
        intPageRecordCnt = viewstate("intPageRecordCnt")
        Grid_ItemCreated(dtgAO, e)
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        If cboApproval.SelectedItem.Value <> "" Then
            Dim objIPP As New IPPMain
            Dim objPurchaseReq2 As New PurchaseReq2
            Dim dtAO As New DataTable
            Dim strConsolidator As String
            Dim intMsg As Integer
            Dim objGlobal As New AppGlobals         
            Dim strMsg As String

            cmdSubmit.Visible = False

            'Zulham 05072018 - PAMB
            'added necessary info for approval
            'Zulham 26092018 - PAMB
            'UAT U00003
            If objIPP.SaveIPPDoc(Session("dsDOC"), ViewState("frm"), Session("Action"), Session("DocNo"), ViewState("olddocno"), cboApproval.SelectedItem.Value, "E2P", ViewState("isResident"), ViewState("exchangeRate")) Then
                strMsg = objGlobal.GetErrorMessage("00024")
                strMsg = "Document " & strMsg
                If Session("urlrefereripp") Is Nothing Or Session("urlrefereripp") = "" Then
                    Common.NetMsgbox(Me, strMsg, dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId), MsgBoxStyle.Information)

                    'Zulham 31012019
                    Dim strAgent = Request.UserAgent
                    If strAgent.ToString.IndexOf("compatible") = -1 Then
                        Response.Redirect(dDispatcher.direct("Dashboard", "AllDashBoard.aspx", "pageid=" & strPageId))
                    End If

                Else
                    Common.NetMsgbox(Me, strMsg, dDispatcher.direct("IPP", "IPPList.aspx", "pageid=" & strPageId), MsgBoxStyle.Information)
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
            dtr("prid") = viewstate("prid")
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
