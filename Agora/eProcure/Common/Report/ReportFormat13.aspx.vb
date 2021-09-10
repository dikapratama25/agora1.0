Imports AgoraLegacy
Imports eProcure.Component
Imports System.Configuration.ConfigurationManager
Public Class ReportFormat13
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    'Protected WithEvents label2 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    'Protected WithEvents cboUserName As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents ValUserName As System.Web.UI.WebControls.RequiredFieldValidator
    'Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    'Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    'Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    'Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton

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
        MyBase.Page_Load(sender, e)

        If Not IsPostBack Then
            viewstate("type") = Request.QueryString("type")
            BindData()

            Select Case UCase(viewstate("type"))
                Case "R13O"
                    lblHeader.Text = "R13 Opex Purchase Requisition Pending Approval By Approver Sort By Branch Code For Branch"
                Case "R13C"
                    lblHeader.Text = "R13 Capex Purchase Requisition Pending Approval By Approver Sort By Branch Code For Branch"
                Case "R14O"
                    lblHeader.Text = "R14 Opex Purchase Requisition Pending Approval By Approver Sort By Branch Code For Head Office"
                Case "R14C"
                    lblHeader.Text = "R14 Capex Purchase Requisition Pending Approval By Approver Sort By Branch Code For Head Office"
                Case "R15O"
                    lblHeader.Text = "R15 Opex Invoices Pending Approval For Approver Sort By Vendor"
                Case "R15C"
                    lblHeader.Text = "R15 Capex Invoices Pending Approval For Approver Sort By Vendor"
            End Select
        End If
        lnkBack.NavigateUrl = "ReportSelection.aspx?pageid=" & strPageId
    End Sub
    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim strReportName As String = ""
        Dim reportURL As String
        Dim strParam As String = ""

        'Jules 2016.01.11 - EPROC FITR.
        Dim objDbcom As New EAD.DBCom
        Dim strTaxCodes As String = ""
        Dim decRecoverablePercentage As Decimal = 0
        'mimi - 17/01/11 - Fitr enhancement
        Dim strRecoverablePercentage As String
        Dim strIRRecoverablePercentage As String

        Dim strTaxSQL As String = "SELECT FTC_TAX_CODE FROM fitr_tax_code WHERE FTC_MODULE='eProcure' "
        Dim ds As DataSet = objDbcom.FillDs(strTaxSQL)
        If ds.Tables(0).Rows.Count > 0 Then
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                If strTaxCodes = "" Then
                    strTaxCodes = ds.Tables(0).Rows(i)("FTC_TAX_CODE")
                Else
                    strTaxCodes &= "," & ds.Tables(0).Rows(i)("FTC_TAX_CODE")
                End If
            Next
        End If

        'mimi 09/01/2017 - fitr_mstr enhancement
        decRecoverablePercentage = objDbcom.GetVal("SELECT fm_fitr_recoverable FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")

        'mimi - 17/01/11 - Fitr enhancement
        strRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_recoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
        strIRRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_irrecoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")

        'Dim crParam As New Hashtable

        'crParam.Add("userIDParam", Session("UserID"))
        'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        'crParam.Add("FOFMID", cboUserName.SelectedValue)
        'strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&FOFMID=" & cboUserName.SelectedValue
        strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&FOFMID=" & cboUserName.SelectedValue & "&taxCodes=" & strTaxCodes & "&recPerc=" & decRecoverablePercentage & "&RecoverablePercentage=" & strRecoverablePercentage & "&IRRecoverablePercentage=" & strIRRecoverablePercentage
        'end - mimi
        'End.    

        Select Case UCase(ViewState("type"))
            Case "R13O"
                strReportName = "R13OpexPRPendingApprvByBranch-Branch.rpt"
            Case "R13C"
                strReportName = "R13CapexPRPendingApprvByBranch-Branch.rpt"
            Case "R14O"
                strReportName = "R14OpexPRPendingApprvByBranch-HQ.rpt"
            Case "R14C"
                strReportName = "R14CapexPRPendingApprvByBranch-HQ.rpt"
            Case "R15O"
                strReportName = "R15OpexInvoicePendingApproval.rpt"
            Case "R15C"
                strReportName = "R15CapexInvoicePendingApproval.rpt"
        End Select


        reportURL = "../Report/ReportViewer.aspx?rpt=" & strReportName & strParam & "&rptType=" & cboReportType.SelectedIndex

        Dim jscript As String = String.Empty
        jscript &= "<script language=""Javascript"">" & vbCrLf
        jscript &= "x = screen.width -200;" & vbCrLf
        jscript &= "y = screen.height - 200;" & vbCrLf
        jscript &= "var props = 'scrollBars=yes, resizable=yes, toolbar=no, menubar=no, location=no, directories=no, top=0, left=0, width=' + x + ', height=' + y ;" & vbCrLf
        jscript &= "window.location = """ & reportURL & """;" & vbCrLf
        jscript &= "//-->" & vbCrLf
        jscript &= "</script>" & vbCrLf
        Dim rndKey As New Random
        Me.ClientScript.RegisterStartupScript(GetType(ReportFormat13), rndKey.Next.ToString, jscript)
    End Sub

    'Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
    '    Dim crParam As New Hashtable

    '    crParam.Add("userIDParam", Session("UserID"))
    '    crParam.Add("buyerCompanyIDParam", Session("CompanyID"))

    '    Select Case UCase(viewstate("type"))
    '        Case "R13O"
    '            crParam.Add("FOFMID", cboUserName.SelectedValue)
    '            ReportViewer.popCrystalReport(Me, "R13OpexPRPendingApprvByBranch-Branch.rpt", crParam)
    '        Case "R13C"
    '            crParam.Add("FOFMID", cboUserName.SelectedValue)
    '            ReportViewer.popCrystalReport(Me, "R13CapexPRPendingApprvByBranch-Branch.rpt", crParam)
    '        Case "R14O"
    '            crParam.Add("FOFMID", cboUserName.SelectedValue)
    '            ReportViewer.popCrystalReport(Me, "R14OpexPRPendingApprvByBranch-HQ.rpt", crParam)
    '        Case "R14C"
    '            crParam.Add("FOFMID", cboUserName.SelectedValue)
    '            ReportViewer.popCrystalReport(Me, "R14CapexPRPendingApprvByBranch-HQ.rpt", crParam)
    '        Case "R15O"
    '            crParam.Add("FOFMID", cboUserName.SelectedValue)
    '            ReportViewer.popCrystalReport(Me, "R15OpexInvoicePendingApproval.rpt", crParam)
    '        Case "R15C"
    '            crParam.Add("FOFMID", cboUserName.SelectedValue)
    '            ReportViewer.popCrystalReport(Me, "R15CapexInvoicePendingApproval.rpt", crParam)
    '    End Select
    'End Sub

    Public Function BindData()
        Dim cbolist As New ListItem
        Dim dvCustom As DataView

        cbolist.Value = ""
        cbolist.Text = "---Select---"

        If UCase(ViewState("type")) = "R15O" Or UCase(ViewState("type")) = "R15C" Then 'For FO & FM
            dvCustom = getcboUser("FO")
        Else
            dvCustom = getcboUser("AO")
        End If

        Common.FillDdl(cboUserName, "two", "UM_USER_ID", dvCustom)
    End Function
    Public Function getcboUser(ByVal Type As String) As DataView
        Dim strSql As String
        Dim drw As DataView
        Dim objDB As New EAD.DBCom

        'strSql = "Select UM_USER_ID, UM_USER_NAME, " & _
        '                     "(UM_USER_ID + ' : ' + UM_USER_NAME) as two " & _
        '                     "FROM USERS_USRGRP R,USER_GROUP_MSTR GM, USER_MSTR M " & _
        '                     "where R.UU_COY_ID ='" & Session("CompanyID") & "' " & _
        '                     "AND M.UM_USER_ID = R.UU_USER_ID AND M.UM_COY_ID=R.UU_COY_ID AND M.UM_DELETED<>'Y' AND M.UM_STATUS='A' " & _
        '                     "AND M.UM_COY_ID='" & Session("CompanyID") & "' " & _
        '                     "AND UU_USER_ID = M.UM_USER_ID AND UU_USRGRP_ID = GM.UGM_USRGRP_ID AND "
        strSql = "Select UM_USER_ID, UM_USER_NAME, " & _
                    objDB.Concat(" : ", "", "UM_USER_ID", "UM_USER_NAME") & " as two " & _
                    "FROM USERS_USRGRP R,USER_GROUP_MSTR GM, USER_MSTR M " & _
                    "where R.UU_COY_ID ='" & Session("CompanyID") & "' " & _
                    "AND M.UM_USER_ID = R.UU_USER_ID AND M.UM_COY_ID=R.UU_COY_ID AND M.UM_DELETED<>'Y' AND M.UM_STATUS='A' " & _
                    "AND M.UM_COY_ID='" & Session("CompanyID") & "' " & _
                    "AND UU_USER_ID = M.UM_USER_ID AND UU_USRGRP_ID = GM.UGM_USRGRP_ID AND "

        If Type = "AO" Then
            strSql = strSql + "GM.UGM_FIXED_ROLE = 'Approving Officer'"
        Else
            strSql = strSql + "(GM.UGM_FIXED_ROLE = 'Finance Manager' or GM.UGM_FIXED_ROLE = 'Finance Officer') "
        End If

        strSql = strSql + " group by UM_USER_ID, UM_USER_NAME order by UM_USER_ID"

        drw = objDB.GetView(strSql)
        Return drw
    End Function

End Class