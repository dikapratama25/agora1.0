Imports AgoraLegacy
Imports eProcure.Component


Public Class ExceedBCMPO
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

        If Not IsPostBack Then
            trApp.Visible = False
            lblRemark.Visible = False
            lblRemarkHeader.Text = "Your purchase has exceeded the current budget limit for the selected accounts under " & _
                                "your profile. You may request for a budget top up through a finance manager " & _
                                "in this form, or cancel the PO. An email will be sent to your finance manager. "
            lblTitle.Text = "Requisition Exceeds Budget"
            ViewState("poid") = Request.QueryString("poid")
            Dim dblPoCost As Double = Request.QueryString("POCost")
            ViewState("currency") = Request.QueryString("currency")
            ViewState("modePR") = Request.QueryString("modePR")
            ViewState("modeRFQFromPR_Index") = Request.QueryString("moderfqfromprindex")
            ViewState("modeRFQFromPR_Index_draft") = Request.QueryString("moderfqfromprindexdraft")
            ViewState("rfqnum") = Request.QueryString("rfqnum")

            Dim objBudget As New BudgetControl
            Dim strBCM As String
            Dim dtBCM As New DataTable

            Dim objDB As New EAD.DBCom
            Dim strRFQ_No, strPR_Index, strGrp_Index
            Dim strDept_Code As String = ""

            If ViewState("modePR") = "pr" And (ViewState("modeRFQFromPR_Index") <> "" Or ViewState("modeRFQFromPR_Index_draft") <> "") Then
                strPR_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRM_PR_INDEX),""'"")) AS CHAR(2000)) AS PRM_PR_INDEX FROM PR_DETAILS, PR_MSTR WHERE PRM_COY_ID = PRD_COY_ID AND PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND (PRD_CONVERT_TO_DOC = '" & ViewState("rfqnum") & "')")
                strGrp_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRA_APPROVAL_GRP_INDEX),""'"")) AS CHAR(2000)) AS PRA_APPROVAL_GRP_INDEX FROM PR_APPROVAL WHERE PRA_FOR = 'PR' AND PRA_PR_INDEX IN (" & strPR_Index & ")")
                strDept_Code = objDB.GetVal("SELECT IFNULL(CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",AGM_DEPT_CODE),""'"")) AS CHAR(2000)),'') AS AGM_DEPT_CODE FROM APPROVAL_GRP_MSTR WHERE AGM_DEPT_CODE IS NOT NULL AND AGM_DEPT_CODE <> '' AND AGM_GRP_INDEX IN (" & strGrp_Index & ")")
            ElseIf ViewState("modePR") = "pr" And (ViewState("modeRFQFromPR_Index") = "" Or ViewState("modeRFQFromPR_Index_draft") = "") Then
                Dim strPO_No As String = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRD_CONVERT_TO_DOC),""'"")) AS CHAR(2000)) AS PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_CONVERT_TO_DOC = '" & ViewState("poid") & "'")
                strPR_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRM_PR_INDEX),""'"")) AS CHAR(2000)) AS PRM_PR_INDEX FROM PR_DETAILS, PR_MSTR WHERE PRM_COY_ID = PRD_COY_ID AND PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND (PRD_CONVERT_TO_DOC IN (" & strPO_No & "))")
                strGrp_Index = objDB.GetVal("SELECT CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRA_APPROVAL_GRP_INDEX),""'"")) AS CHAR(2000)) AS PRA_APPROVAL_GRP_INDEX FROM PR_APPROVAL WHERE PRA_FOR = 'PR' AND PRA_PR_INDEX IN (" & strPR_Index & ")")
                strDept_Code = objDB.GetVal("SELECT IFNULL(CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",AGM_DEPT_CODE),""'"")) AS CHAR(2000)),'') AS AGM_DEPT_CODE FROM APPROVAL_GRP_MSTR WHERE AGM_DEPT_CODE IS NOT NULL AND AGM_DEPT_CODE <> '' AND AGM_GRP_INDEX IN (" & strGrp_Index & ")")
            End If

            blnExceed = objBudget.checkBCMPO(ViewState("poid"), dtBCM, strBCM)
            If Not blnExceed Then
                'Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "msg=0&pageid=" & strPageId & "&poid=" & ViewState("poid") & "&prcost=" & dblPoCost))
                ' Response.Redirect(dDispatcher.direct("PO", "POApprovalSetup.aspx", "msg=0&pageid=" & strPageId & "&poid=" & ViewState("poid") & "&pocost=" & dblPoCost & "&dept=" & strDept_Code & "&prindex=" & strPR_Index))
                Response.Redirect(dDispatcher.direct("PO", "POApprovalSetup.aspx", "msg=0&pageid=" & strPageId & "&poid=" & ViewState("poid") & "&pocost=" & dblPoCost & "&dept=" & strDept_Code & "&prindex=" & strPR_Index & "&Frm=" & Request.QueryString("Frm")))
            Else
                If strBCM = "2" Then ' advisory mode
                    'Response.Redirect(dDispatcher.direct("PR", "ApprovalSetup.aspx", "msg=1&pageid=" & strPageId & "&poid=" & ViewState("poid") & "&prcost=" & dblPoCost))
                    ' Response.Redirect(dDispatcher.direct("PO", "POApprovalSetup.aspx", "msg=1&pageid=" & strPageId & "&poid=" & ViewState("poid") & "&pocost=" & dblPoCost & "&dept=" & strDept_Code & "&prindex=" & strPR_Index))
                    Response.Redirect(dDispatcher.direct("PO", "POApprovalSetup.aspx", "msg=1&pageid=" & strPageId & "&poid=" & ViewState("poid") & "&pocost=" & dblPoCost & "&dept=" & strDept_Code & "&prindex=" & strPR_Index & "&Frm=" & Request.QueryString("Frm")))
                Else ' absolute mode
                    Dim objPR As New PR
                    Dim dsApprovalList As New DataSet
                    dsApprovalList = objPR.getAppovalList("D", dblPoCost)
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

                'lnkBack.NavigateUrl = dDispatcher.direct("PR", "RaisePR.aspx", "type=list&pageid=" & strPageId & "&poid=" & ViewState("poid"))
                lnkBack.NavigateUrl = dDispatcher.direct("PO", "RaisePO.aspx", "type=mod&mode=po&pageid=" & strPageId & "&poid=" & ViewState("poid"))
                Dim _sql = "Select IFNULL(POM_PO_TYPE,'') from po_mstr where pom_po_no = '" & ViewState("poid") & "' and pom_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'"
                Dim _isFFPO = objDB.GetVal(_sql)
                If Not _isFFPO = "" Then
                    If _isFFPO = "Y" Then
                        lnkBack.NavigateUrl = dDispatcher.direct("PO", "RaiseFFPO.aspx", "mode=po&type=mod&poid=" & ViewState("poid") & "&pageid=" & strPageId)
                    End If
                End If
                'lnkBack.NavigateUrl = dDispatcher.direct("PR", "RaisePR.aspx", "type=mod&mode=bc&pageid=" & strPageId & "&prid=" & ViewState("prid"))
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
        blnExceed = objBudget.checkBCM(ViewState("poid"), dtBCM, strBCM)
        objPR.RequestBudgetTopup(strBCM, lblDeptName.Text, txtRemark.Text, ViewState("currency"), dtBCM)
        lblRemarkHeader.Text = "Your request has been sent to the Finance Manager."
        lblRemark.Text = txtRemark.Text
        lblRemark.Visible = True
        txtRemark.Visible = False
        cmdSubmit.Visible = False
    End Sub
End Class
