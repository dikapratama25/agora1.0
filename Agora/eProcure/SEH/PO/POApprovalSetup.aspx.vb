Imports AgoraLegacy
Imports eProcure.Component

Public Class POApprovalSetupSEH
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDB As New EAD.DBCom

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
    Protected WithEvents lblpo As System.Web.UI.WebControls.Label
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
            lblTitle.Text = "Raise PO"
            ViewState("poid") = Request.QueryString("poid")
            ViewState("pocost") = Request.QueryString("pocost")
            ViewState("msg") = Request.QueryString("msg")
            ViewState("type") = Request.QueryString("type")
            ViewState("dept") = Request.QueryString("dept")
            ViewState("prindex") = Request.QueryString("prindex")
            lblpo.Text = ViewState("poid")

            Dim objPR As New PR
            ViewState("ApprovalType") = objPR.getApprovalType


            'Get latest Exchange Rate
            ViewState("appcurrency") = Request.QueryString("currency")
            Dim CURR_RATE As Decimal
            Dim COMP_CURR, TEMP As String
            TEMP = objDB.GetVal("SELECT CE_RATE FROM COMPANY_EXCHANGERATE WHERE CE_COY_ID = '" & Session("CompanyId") & "' AND CE_CURRENCY_CODE = '" & ViewState("appcurrency") & "' AND CE_DELETED='N' AND CE_VALID_FROM <= CURRENT_DATE() AND CE_VALID_TO >= CURRENT_DATE()")
            If TEMP = "" Then
                COMP_CURR = objDB.GetVal("SELECT CM_CURRENCY_CODE FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Session("CompanyId") & "'")

                If COMP_CURR = ViewState("appcurrency") Then
                    CURR_RATE = 1
                    Session("blnRate") = True
                Else
                    CURR_RATE = 0
                    Session("blnRate") = False
                End If
            Else
                Session("blnRate") = True
                CURR_RATE = CDec(TEMP)
            End If

            ViewState("pocost") = CDec(ViewState("pocost")) * CURR_RATE

            Dim dsApprovalList As New DataSet
            Dim intCnt As Integer
            Dim i As Integer
            Dim strConsolidator As String = ""
            dsApprovalList = objPR.getAppovalList("A", CDbl(ViewState("pocost")), "PO", True, ViewState("dept"))
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
                'lstItem.Value = ""
                'lstItem.Text = "---Select---"
                'cboApproval.Items.Insert(0, lstItem)
                lblMsg.Visible = False
                If dsApprovalList.Tables(0).Rows.Count = 1 Then '//Display approving officer if only has one approval workflow
                    cboApproval.SelectedIndex = 1
                    cboApproval_SelectedIndexChanged(sender, e)
                Else
                    If cboApproval.SelectedItem.Value <> "" Then
                        Bindgrid()
                    Else

                        lblRemark.Text = ""
                        trConsolidator.Visible = False
                        dtgAO.DataBind()

                    End If

                    If ViewState("prindex") <> Nothing Or ViewState("prindex") <> "" Then
                        Dim strAGM_Name As String = objDB.GetVal("SELECT IFNULL(AGM_GRP_NAME,'') AS AGM_GRP_INDEX FROM APPROVAL_GRP_MSTR  WHERE AGM_TYPE = 'PR' AND AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND AGM_GRP_INDEX = (SELECT DISTINCT PRA_APPROVAL_GRP_INDEX FROM PR_APPROVAL WHERE PRA_PR_INDEX IN (" & ViewState("prindex") & ") AND PRA_FOR = 'PR' LIMIT 1) LIMIT 1")
                        If strAGM_Name <> "" Then
                            For i = 0 To dsApprovalList.Tables(0).Rows.Count - 1
                                If Common.parseNull(dsApprovalList.Tables(0).Rows(i)("AGM_GRP_NAME")) = strAGM_Name Then
                                    cboApproval.SelectedIndex = i
                                    cboApproval_SelectedIndexChanged(sender, e)
                                    Exit For
                                End If
                                'lblRemark.Text = ""
                                'trConsolidator.Visible = False
                                'dtgAO.DataBind()
                            Next
                        Else
                            'lblRemark.Text = ""
                            'trConsolidator.Visible = False
                            'dtgAO.DataBind()
                        End If

                    Else
                        'lblRemark.Text = ""
                        'trConsolidator.Visible = False
                        'dtgAO.DataBind()
                    End If
                End If
            Else
                cboApproval.Visible = False
                lblRemark.Visible = False
                lblMsg.Visible = True
                lblMsg.Text = "There is no approval list available for this PR/ PO because the sequence of approving officers do not have the approval limit to approve it."
                cmdSubmit.Visible = False
                rfvApproval.Enabled = False
            End If
        End If

        Dim _sql = "Select IFNULL(POM_PO_TYPE,'') from po_mstr where pom_po_no = '" & ViewState("poid") & "' and pom_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'"
        Dim _isFFPO = objDB.GetVal(_sql)
        If Not _isFFPO = "" Then
            If _isFFPO = "Y" Then
                lblTitle.Text = "Raise Free Form Purchase Order"
                If Not Request.QueryString("Frm") Is Nothing Then
                    If Request.QueryString("Frm") = "Dashboard" Then
                        Session("urlreferer") = "_FFPODashboard"
                    ElseIf Request.QueryString("Frm") = "" Then
                        Session("urlreferer") = "POViewB2"
                    Else
                        Session("urlreferer") = Request.QueryString("Frm")
                    End If
                Else
                    Session("urlreferer") = "POViewB2"
                End If
                lnkBack.NavigateUrl = dDispatcher.direct("PO", "RaiseFFPO.aspx", "mode=po&type=mod&poid=" & ViewState("poid") & "&pageid=" & strPageId)
            Else
                lnkBack.NavigateUrl = dDispatcher.direct("PO", "RaisePO.aspx", "mode=po&type=mod&poid=" & ViewState("poid") & "&pageid=" & strPageId)
            End If
        Else
            lnkBack.NavigateUrl = dDispatcher.direct("PO", "RaisePO.aspx", "mode=po&type=mod&poid=" & ViewState("poid") & "&pageid=" & strPageId)
        End If
        GenerateTab()
    End Sub

    Private Function Bindgrid() As String
        Dim objPR As New PR
        Dim dsAO As New DataSet
        Dim dvViewSample As DataView

        dsAO = objPR.getAOList(cboApproval.SelectedItem.Value, "PO")
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
        intRow = 0
        ViewState("blnCutPO") = False
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
                    If CDbl(ViewState("pocost")) < CDbl(dv("UM_APP_LIMIT")) Then
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
                    ' If CDbl(viewstate("pocost")) < CDbl(dv("UM_APP_LIMIT")) Then
                    If CDbl(ViewState("pocost")) <= CDbl(dv("UM_APP_LIMIT")) Then
                        lblType.Text = "Approval"
                        'viewstate("blnSubmit") = True
                    Else
                        lblType.Text = "Endorsement"
                    End If

                Case "B+C"
                    'Michelle (10/8/2007) - should be 'Approval' if PR cost is the same as the limit
                    'If CDbl(viewstate("pocost")) < CDbl(dv("UM_APP_LIMIT")) Then
                    If CDbl(ViewState("pocost")) <= CDbl(dv("UM_APP_LIMIT")) Then
                        If ViewState("blnCutPO") = True Then
                            lblType.Text = "None"
                        Else
                            lblType.Text = "Approval"
                            ViewState("blnCutPO") = True
                            'viewstate("blnSubmit") = True
                        End If
                    Else
                        lblType.Text = "Endorsement"
                    End If

                Case "A"
                    If CDbl(ViewState("pocost")) < CDbl(dv("UM_APP_LIMIT")) Then
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
        str &= "<li><strong>Approval</strong> - PR/ PO will be sent to the respective approving officer for approval </li> "
        str &= "<li><strong>Endorsement</strong> - PR/ PO will be sent to the respective approving officer for endorsement </li> "
        str &= "<li><strong>None</strong> - Approving officer has no authority to approve/endorse this PR/ PO</li></ul>"

        ' End Select

        lblRemark.Text = str
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim objPO As New PurchaseOrder_Buyer
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
                strVendor = objPurchaseReq2.getPRVendor(ViewState("poid"))
                If strVendor = Nothing Then
                    Common.NetMsgbox(Me, "Not allow to have Consolidator for this PR", MsgBoxStyle.Exclamation)
                    Exit Sub
                End If
            End If

            strAryConsolidator = strConsolidator.Split(",")
            'intMsg = objPR.updatePRStatus(ViewState("poid"), PRStatus.Submitted, strAryConsolidator(cboApproval.SelectedIndex - 1), dtAO, ViewState("msg"))
            If ViewState("prindex") <> Nothing Or ViewState("prindex") <> "" Then
                intMsg = objPO.submitPO(ViewState("poid"), dtAO, ViewState("msg"), True, , True)
            Else
                intMsg = objPO.submitPO(ViewState("poid"), dtAO, ViewState("msg"), True)
            End If

            Select Case intMsg
                'Case WheelMsgNum.NotSave
                '    'Response.Redirect("PRConfirm.aspx?msg=2&type=A&poid=" & ViewState("poid"))
                '    Common.NetMsgbox(Me, "Purchase Order Number " & ViewState("poid") & " has been submitted.", Session("urlreferer") & ".aspx?type=tab&pageid=" & strPageId)
                Case WheelMsgNum.Save

                    intMsg = objPO.updatePRStatus(ViewState("prindex"), ViewState("msg"))

                    Dim objBudget As New BudgetControl
                    Dim dtBCMChk As New DataTable
                    Dim strBCM As String
                    Dim blnExceed As Boolean
                    blnExceed = objBudget.checkBCMPO(ViewState("poid"), dtBCMChk, strBCM)

                    Dim strConfirm As String
                    If ViewState("msg") = "1" And blnExceed = True Then
                        strConfirm = "PO value is more than the Operating Budget Amount."
                    Else
                        strConfirm = ""
                    End If

                    'Response.Redirect("PRConfirm.aspx?msg=" & ViewState("msg") & "&type=A&poid=" & ViewState("poid"))
                    If Session("urlreferer") = "RFQComSummary" Then
                        Common.NetMsgbox(Me, "Purchase Order Number " & ViewState("poid") & " has been submitted." & """ & vbCrLf & """ & strConfirm & "", Session("urlrefererForRFQ"))
                    ElseIf Request.QueryString("Frm") = "Dashboard" Then
                        Common.NetMsgbox(Me, "Purchase Order Number " & ViewState("poid") & " has been submitted." & """ & vbCrLf & """ & strConfirm & "", dDispatcher.direct("Dashboard", Request.QueryString("dpage") & ".aspx", "type=tab&pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "BuyerCatalogueSearch" Then
                        Common.NetMsgbox(Me, "Purchase Order Number " & ViewState("poid") & " has been submitted." & """ & vbCrLf & """ & strConfirm & "", dDispatcher.direct("Search", Session("urlreferer") & ".aspx", "type=tab&pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "ConCatSearch" Then
                        Common.NetMsgbox(Me, "Purchase Order Number " & ViewState("poid") & " has been submitted." & """ & vbCrLf & """ & strConfirm & "", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "RaiseFFPO" Then
                        Common.NetMsgbox(Me, "Purchase Order Number " & ViewState("poid") & " has been submitted." & """ & vbCrLf & """ & strConfirm & "", dDispatcher.direct("PO", "RaiseFFPO.aspx", "mode=po&type=mod&poid=" & ViewState("poid") & "&pageid=" & strPageId))
                    ElseIf Session("urlreferer") = "_FFPODashboard" Then
                        Common.NetMsgbox(Me, "Purchase Order Number " & ViewState("poid") & " has been submitted." & """ & vbCrLf & """ & strConfirm & "", dDispatcher.direct("Dashboard", "AllDashBoard" & ".aspx", "type=tab&pageid=" & strPageId))
                    Else
                        Common.NetMsgbox(Me, "Purchase Order Number " & ViewState("poid") & " has been submitted." & """ & vbCrLf & """ & strConfirm & "", dDispatcher.direct("PO", Session("urlreferer") & ".aspx", "type=tab&pageid=" & strPageId))

                    End If
            End Select
        End If
    End Sub

    Private Function bindAO() As DataTable
        Dim dgItem As DataGridItem
        Dim dtAO As New DataTable
        Dim i As Integer = 0
        dtAO.Columns.Add("poid", Type.GetType("System.String"))
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
            dtr("poid") = ViewState("poid")
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

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If
        'Session("w_AddPOAppr_tabs") = "<div class=""t_entity""><ul>" & _
        '    "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn_selected"" href=""POViewB2.aspx?pageid=" & strPageId & """><span>Purchase Order</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '             "<li><a class=""t_entity_btn"" href=""POViewB2Cancel.aspx?type=Listing&pageid=" & strPageId & """><span>Purchase Order Cancellation</span></a></li>" & _
        '                         "<li><div class=""space""></div></li>" & _
        '             "</ul><div></div></div>"

        Dim showFFPO = objDB.GetVal("SELECT CM_FFPO_CONTROL FROM sso.company_mstr WHERE cm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'")

        If showFFPO.ToString.ToUpper = "Y" Then
            If Not Session("urlreferer") = "BuyerCatalogueSearch" Then
                Session("w_AddPOAppr_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
            "<li><div class=""space""></div></li>" & _
            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB2Cancel.aspx", "type=Listing&pageid=" & strPageId) & """><span>Purchase Order Cancellation</span></a></li>" & _
            "<li><div class=""space""></div></li>" & _
            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "RAISEFFPO.aspx", "type=Listing&pageid=" & strPageId) & """><span>Free Form Purchase Order</span></a></li>" & _
            "<li><div class=""space""></div></li>" & _
            "</ul><div></div></div>"
            Else
                Session("w_AddPOAppr_tabs") = "<div class=""t_entity""><ul>" & _
             "<li><div class=""space""></div></li>" & _
            "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
            "<li><div class=""space""></div></li>" & _
            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB2Cancel.aspx", "type=Listing&pageid=" & strPageId) & """><span>Purchase Order Cancellation</span></a></li>" & _
            "<li><div class=""space""></div></li>" & _
            "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "RAISEFFPO.aspx", "type=Listing&pageid=" & strPageId) & """><span>Free Form Purchase Order</span></a></li>" & _
            "<li><div class=""space""></div></li>" & _
            "</ul><div></div></div>"
            End If
        Else
            Session("w_AddPOAppr_tabs") = "<div class=""t_entity""><ul>" & _
            "<li><div class=""space""></div></li>" & _
                         "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("PO", "POViewB2.aspx", "pageid=" & strPageId) & """><span>Purchase Order</span></a></li>" & _
                         "<li><div class=""space""></div></li>" & _
                         "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("PO", "POViewB2Cancel.aspx", "type=Listing&pageid=" & strPageId) & """><span>Purchase Order Cancellation</span></a></li>" & _
                         "<li><div class=""space""></div></li>" & _
                         "</ul><div></div></div>"
        End If
    End Sub

End Class
