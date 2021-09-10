Imports AgoraLegacy
Imports eProcure.Component


Public Class ExceedBCMSEH
    Inherits AgoraLegacy.AppBaseClass
    Dim blnExceed As Boolean
    Dim dDispatcher As New AgoraLegacy.dispatcher

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
        'Put user code to initialize the page here
        MyBase.Page_Load(sender, e)

        Dim objPR As New PR
        If Not IsPostBack Then
            trApp.Visible = False
            lblRemark.Visible = False
            lblRemarkHeader.Text = "Your purchase has exceeded the current budget limit for the selected accounts under " & _
                                "your profile. You may request for a budget top up through a finance manager " & _
                                "in this form, or cancel the PR. An email will be sent to your finance manager. "
            lblTitle.Text = "Requisition Exceeds Budget"
            ViewState("prid") = Request.QueryString("prid")
            Dim dblPrCost As Double = Request.QueryString("PRCost")
            ViewState("currency") = Request.QueryString("currency")

            Dim objBudget As New BudgetControl
            Dim strBCM As String
            Dim dtBCM As New DataTable
            blnExceed = objBudget.checkBCM(ViewState("prid"), dtBCM, strBCM)

            Dim dt As New DataTable
            Dim intMsg As Integer

            If Not blnExceed Then
                'If Session("Env") = "FTN" Then
                '    dt = objPR.getPRApprFlow(True)
                'Else
                '    dt = objPR.getPRApprFlow(False)
                'End If
                dt = objPR.getPRApprFlow(False)

                ViewState("mode") = Request.QueryString("mode")
                If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                    Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                    Exit Sub
                ElseIf dt.Rows.Count > 1 Then
                    Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=0&prid=" & ViewState("prid") & "&prcost=" & dblPrCost & "&currency=" & ViewState("currency")))
                    Exit Sub
                Else
                    Dim dsAO As New DataSet
                    Dim UM_APP_LIMIT As Double
                    Dim PR_Type As String = ""
                    Dim blnBuyerWNoWork As Boolean = False

                    If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                        blnBuyerWNoWork = True
                    Else
                        ViewState("ApprovalType") = objPR.getApprovalType
                        dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                        ' case Automated Approval
                        If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                            UM_APP_LIMIT = 0
                        Else
                            UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                        End If

                        ' A - Automated Approval
                        ' B - Allow Lower Limit Endorsement
                        ' C - Cut PO before end of Aproval List
                        ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                        Select Case ViewState("ApprovalType")
                            Case "C"
                                If CDbl(ViewState("prcost")) < CDbl(UM_APP_LIMIT) Then
                                    If ViewState("blnCutPR") = True Then
                                        PR_Type = "None"
                                    Else
                                        PR_Type = "Approval"
                                        ViewState("blnCutPR") = True
                                    End If
                                Else
                                    PR_Type = "None"
                                End If

                            Case "B"
                                If CDbl(ViewState("prcost")) <= CDbl(UM_APP_LIMIT) Then
                                    PR_Type = "Approval"
                                Else
                                    PR_Type = "Endorsement"
                                End If

                            Case "B+C"
                                If CDbl(ViewState("prcost")) <= CDbl(UM_APP_LIMIT) Then
                                    If ViewState("blnCutPR") = True Then
                                        PR_Type = "None"
                                    Else
                                        PR_Type = "Approval"
                                        ViewState("blnCutPR") = True
                                    End If
                                Else
                                    PR_Type = "Endorsement"
                                End If

                            Case "A"
                                If CDbl(ViewState("prcost")) < CDbl(UM_APP_LIMIT) Then
                                    PR_Type = "Approval"
                                Else
                                    PR_Type = "None"
                                End If

                        End Select

                        Select Case PR_Type
                            Case "None"
                                PR_Type = "0"
                            Case "Approval"
                                PR_Type = "1"
                            Case "Endorsement"
                                PR_Type = "2"
                        End Select
                    End If

                    intMsg = objPR.submitPR(ViewState("prid"), PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)
                    Select Case intMsg
                        Case WheelMsgNum.Save
                            If Session("urlreferer") = "BuyerCatSearch" Then
                                Common.NetMsgbox(Me, "Purchase Request Number " & ViewState("prid") & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                            ElseIf Session("urlreferer") = "ConCatSearch" Then
                                Common.NetMsgbox(Me, "Purchase Request Number " & ViewState("prid") & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                            ElseIf Session("urlreferer") = "PRAll" Then
                                Common.NetMsgbox(Me, "Purchase Request Number " & ViewState("prid") & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                            End If

                        Case WheelMsgNum.NotSave
                            Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                        Case WheelMsgNum.Delete
                            Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

                    End Select
                End If
                'Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "msg=0&pageid=" & strPageId & "&prid=" & ViewState("prid") & "&prcost=" & dblPrCost))
            Else
                If strBCM = "2" Then ' advisory mode

                    'If Session("Env") = "FTN" Then
                    '    dt = objPR.getPRApprFlow(True)
                    'Else
                    '    dt = objPR.getPRApprFlow(False)
                    'End If
                    dt = objPR.getPRApprFlow(False)

                    ViewState("mode") = Request.QueryString("mode")
                    If dt.Rows.Count = 0 And ViewState("mode") = "cc" Then
                        Common.NetMsgbox(Me, "There is no Approval Flow defined for you.", MsgBoxStyle.Exclamation)
                        Exit Sub
                    ElseIf dt.Rows.Count > 1 Then
                        Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "pageid=" & strPageId & "&msg=1&prid=" & ViewState("prid") & "&prcost=" & dblPrCost & "&currency=" & ViewState("currency")))
                        Exit Sub
                    Else
                        Dim dsAO As New DataSet
                        Dim UM_APP_LIMIT As Double
                        Dim PR_Type As String = ""
                        Dim blnBuyerWNoWork As Boolean = False

                        If dt.Rows.Count = 0 And ViewState("mode") = "bc" Then
                            blnBuyerWNoWork = True
                        Else
                            ViewState("ApprovalType") = objPR.getApprovalType
                            dsAO = objPR.getAOList(dt.Rows(0)("GrpIndex"))

                            ' case Automated Approval
                            If IsDBNull(Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))) Then
                                UM_APP_LIMIT = 0
                            Else
                                UM_APP_LIMIT = Common.parseNull(dsAO.Tables(0).Rows(0)("UM_APP_LIMIT"))
                            End If

                            ' A - Automated Approval
                            ' B - Allow Lower Limit Endorsement
                            ' C - Cut PO before end of Aproval List
                            ' B+C - Allow Lower Limit Endorsement + Cut PO before end of Aproval List
                            Select Case ViewState("ApprovalType")
                                Case "C"
                                    If CDbl(ViewState("prcost")) < CDbl(UM_APP_LIMIT) Then
                                        If ViewState("blnCutPR") = True Then
                                            PR_Type = "None"
                                        Else
                                            PR_Type = "Approval"
                                            ViewState("blnCutPR") = True
                                        End If
                                    Else
                                        PR_Type = "None"
                                    End If

                                Case "B"
                                    If CDbl(ViewState("prcost")) <= CDbl(UM_APP_LIMIT) Then
                                        PR_Type = "Approval"
                                    Else
                                        PR_Type = "Endorsement"
                                    End If

                                Case "B+C"
                                    If CDbl(ViewState("prcost")) <= CDbl(UM_APP_LIMIT) Then
                                        If ViewState("blnCutPR") = True Then
                                            PR_Type = "None"
                                        Else
                                            PR_Type = "Approval"
                                            ViewState("blnCutPR") = True
                                        End If
                                    Else
                                        PR_Type = "Endorsement"
                                    End If

                                Case "A"
                                    If CDbl(ViewState("prcost")) < CDbl(UM_APP_LIMIT) Then
                                        PR_Type = "Approval"
                                    Else
                                        PR_Type = "None"
                                    End If

                            End Select

                            Select Case PR_Type
                                Case "None"
                                    PR_Type = "0"
                                Case "Approval"
                                    PR_Type = "1"
                                Case "Endorsement"
                                    PR_Type = "2"
                            End Select
                        End If

                        intMsg = objPR.submitPR(ViewState("prid"), PRStatus.Submitted, Nothing, dt, ViewState("msg"), PR_Type, blnBuyerWNoWork)
                        Select Case intMsg
                            Case WheelMsgNum.Save
                                If Session("urlreferer") = "BuyerCatSearch" Then
                                    Common.NetMsgbox(Me, "Purchase Request Number " & ViewState("prid") & " has been submitted.", dDispatcher.direct("Search", "BuyerCatSearch.aspx", "pageid=" & strPageId))
                                ElseIf Session("urlreferer") = "ConCatSearch" Then
                                    Common.NetMsgbox(Me, "Purchase Request Number " & ViewState("prid") & " has been submitted.", dDispatcher.direct("Search", "ContractCatSearch.aspx", "pageid=" & strPageId))
                                ElseIf Session("urlreferer") = "PRAll" Then
                                    Common.NetMsgbox(Me, "Purchase Request Number " & ViewState("prid") & " has been submitted.", dDispatcher.direct("PR", "SearchPR_All.aspx", "pageid=" & strPageId))
                                End If

                            Case WheelMsgNum.NotSave
                                Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)
                            Case WheelMsgNum.Delete
                                Common.NetMsgbox(Me, MsgTransDup, MsgBoxStyle.Information)

                        End Select
                    End If
                    'Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "msg=1&pageid=" & strPageId & "&prid=" & ViewState("prid") & "&prcost=" & dblPrCost))
                Else ' absolute mode
                    ' Dim objPR As New PR
                    Dim dsApprovalList As New DataSet
                    dsApprovalList = objPR.getAppovalList("D", dblPrCost, "PR", True)
                    If dsApprovalList.Tables(0).Rows.Count > 0 Then
                        Common.FillDdl(cboApproval, "AGM_GRP_NAME", "AGA_GRP_INDEX", dsApprovalList)
                    End If

                    Dim objUsers As New Users
                    Dim objUser As New User
                    objUser = objUsers.GetUserDetails(Session("UserID"), Session("CompanyID"))
                    lblDeptName.Text = objUser.DeptName
                    lblBuyerName.Text = objUser.Name
                    lblBuyerID.Text = objUser.UserID
                    lblBuyerEmail.Text = objUser.Email
                    displayAccount(dtBCM)
                End If

                lnkBack.NavigateUrl = dDispatcher.direct("PR", "RaisePR.aspx", "type=mod&mode=bc&pageid=" & strPageId & "&prid=" & ViewState("prid"))
            End If
        End If
    End Sub

    Private Function displayAccount(ByVal dt As DataTable)
        Dim strAcc As String
        Dim i As Integer = 0
        strAcc = "<TABLE>"
        strAcc &= "<TR CLASS='GridHeader'>"
        strAcc &= "<TD WIDTH=40%>"
        strAcc &= "Budget Account Code"
        strAcc &= "</TD>"
        strAcc &= "<TD WIDTH=60%>"
        strAcc &= "Amount Exceed"
        strAcc &= "</TD>"
        strAcc &= "</TR>"

        For i = 0 To dt.Rows.Count - 1
            If i Mod 2 <> 0 Then
                strAcc &= "<TR bgcolor='#f6f9fe'>"
            Else
                strAcc &= "<TR CLASS='Grid'>"
            End If

            strAcc &= "<TD>"
            strAcc &= dt.Rows(i)("Acct_Code")
            strAcc &= "</TD>"
            strAcc &= "<TD ALIGN='right'>"
            strAcc &= ViewState("currency") & " "
            strAcc &= Format(CDbl(dt.Rows(i)("Acct_Amount")), "###,##0.00")
            strAcc &= "</TD>"
            strAcc &= "</TR>"
            i = i + 1
        Next

        strAcc &= "</TABLE>"
        lblAccount.Text = strAcc
    End Function

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim objPR As New PR
        Dim objBudget As New BudgetControl
        Dim strBCM As String
        Dim dtBCM As New DataTable

        Common.NetMsgbox(Me, "Your request has been sent to the Finance Manager.", dDispatcher.direct("PR", "RaisePR.aspx", "type=mod&mode=bc&pageid=" & strPageId & "&prid=" & ViewState("prid")))

        blnExceed = objBudget.checkBCM(ViewState("prid"), dtBCM, strBCM)
        objPR.RequestBudgetTopup(strBCM, lblDeptName.Text, txtRemark.Text, ViewState("currency"), dtBCM)
        lblRemarkHeader.Text = "Your request has been sent to the Finance Manager."
        lblRemark.Text = txtRemark.Text
        lblRemark.Visible = True
        txtRemark.Visible = False
        cmdSubmit.Visible = False
    End Sub
End Class
