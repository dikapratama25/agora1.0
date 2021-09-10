Imports AgoraLegacy
Imports eProcure.Component
Imports System.Data
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports MySql.Data.MySqlClient
 'Jules 2015-Feb-03 for IPP GST Stage 2A

Public Class ReportFormatN9
    Inherits AgoraLegacy.AppBaseClass
    Dim dispatcher As New dispatcher
    Protected WithEvents trDailyGL As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trType As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trStartandEndDate As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trInvPendingApp As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ddlFO As System.Web.UI.WebControls.DropDownList
    Protected WithEvents trGLCode As Global.System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtStartDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEndDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFromGLCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtToGLCode As System.Web.UI.WebControls.TextBox
    Protected WithEvents rdbtnEmployee As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rdbtnVendor As System.Web.UI.WebControls.RadioButton
    'Protected WithEvents ValDate As System.Web.UI.WebControls.RequiredFieldValidator
    Dim gcnConn As MySqlConnection  'for Payment Advice
    Dim gstrErrMsg As String    'for Payment Advice
    Dim ConStr As String = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath")    'Payment Advice
    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lnkBack.NavigateUrl = dispatcher.direct("Report", "ReportSelectionI.aspx", "pageid=" & strPageId & "&type=" & Request.QueryString("type"))
        strNewCSS = "true"
        Dim Roles As New ArrayList : Roles = Session("MixUserRole")
        For Each str As String In Roles
            If str = "Report Administrator" Then
                lnkBack.NavigateUrl = dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId & "&type=" & Request.QueryString("type"))
            End If
        Next

        'Dim strCoyType As String

        Select Case Request.QueryString("type")
            Case "DailyGL"
                lblCriteria.Text = "As At Date"
                trDailyGL.Style("display") = ""
                'ValDate.ControlToValidate = "txtDate"
                trInvPendingApp.Style("display") = "none"
                If Not IsPostBack Then
                    txtDate.Text = DateTime.Today.AddDays(-1)
                End If


            Case "IPPB4Pay"
                Label1.Text = "Finance Officer"
                trDailyGL.Style("display") = "none"
                'ValDate.ControlToValidate = Nothing
                trInvPendingApp.Style("display") = ""
                trType.Style("display") = ""
                If Not Page.IsPostBack Then
                    Call LoadFO()
                End If

            Case "IPPAftPay"
                Label1.Text = "Finance Manager"
                trDailyGL.Style("display") = "none"
                'ValDate.ControlToValidate = Nothing
                trInvPendingApp.Style("display") = ""
                trType.Style("display") = ""
                If Not Page.IsPostBack Then
                    Call LoadFM()
                End If

            Case "IPPA0012"
                trDailyGL.Style("display") = "none"
                trInvPendingApp.Style("display") = "none"

            Case "IPPA0013"
                trDailyGL.Style("display") = "none"
                trInvPendingApp.Style("display") = "none"

            Case "IPPA0014"
                trDailyGL.Style("display") = "none"
                trInvPendingApp.Style("display") = "none"
                trStatus.Style("display") = ""
                trGLCode.Style("display") = ""
                trCode.Style("display") = ""
                trDate.Style("display") = ""

            Case "IPPA0015"
                trDailyGL.Style("display") = "none"
                trInvPendingApp.Style("display") = "none"

            Case "PayAdv"
                trDailyGL.Style("display") = "none"
                trInvPendingApp.Style("display") = "none"
                trPANo.Style("display") = ""
                trBCNo.Style("display") = ""
                trReportType.Style("display") = "none"
                cboReportType.SelectedValue = "PDF"
                lblAction.Visible = False

            Case "TaxInvoice"
                trDailyGL.Style("display") = "none"
                trInvPendingApp.Style("display") = "none"
                trTaxInvoice.Style("display") = ""
                'trBCNo.Style("display") = ""
                trReportType.Style("display") = "none"
                cboReportType.SelectedValue = "PDF"
                lblAction.Visible = False

                'Zulham Apr 16, 2013
            Case "IPPA0016"
                Label8.Text = "Start Date"
                Label11.Text = "End Date"
                trDailyGL.Style("display") = "none"
                trStatus.Style("display") = "none"
                trCode.Style("display") = "none"
                trInvPendingApp.Style("display") = "none"
                trPANo.Style("display") = "none"
                trBCNo.Style("display") = "none"
                trDate.Style("display") = ""
                'End
            Case "DebitNote"
                trDailyGL.Style("display") = "none"
                trInvPendingApp.Style("display") = "none"
                trPANo.Style("display") = "none"
                trBCNo.Style("display") = "none"
                trReportType.Style("display") = "none"
                cboReportType.SelectedValue = "PDF"
                trDebitNoteNo.Style("display") = ""
                lblAction.Visible = False

            Case "IPPS0017"
                trDailyGL.Style("display") = "none"
                trStatus.Style("display") = "none"
                trGLCode.Style("display") = "none"
                trCode.Style("display") = "none"
                trDate.Style("display") = "none"
                trInvPendingApp.Style("display") = "none"
                trPANo.Style("display") = "none"
                trBCNo.Style("display") = "none"
                trDate.Style("display") = "none"
                trDebitNoteNo.Style("display") = "none"
                trType.Style("display") = "none"
                trStartandEndDate.Style("display") = ""
                cboReportType.Enabled = False

            Case "DebitAdvice"
                trDailyGL.Style("display") = "none"
                trInvPendingApp.Style("display") = "none"
                trPANo.Style("display") = "none"
                trBCNo.Style("display") = "none"
                trReportType.Style("display") = "none"
                cboReportType.SelectedValue = "PDF"
                trDebitNoteNo.Style("display") = "none"
                trDebitAdviceNo.Style("display") = ""
                lblAction.Visible = False

                'Jules 2015.01.30 IPP Stage 2A
            Case "BillPendAppr"
                Label1.Text = "Approving Officer"
                trDailyGL.Style("display") = "none"
                'ValDate.ControlToValidate = Nothing
                trInvPendingApp.Style("display") = ""
                trType.Style("display") = "none"
                If Not Page.IsPostBack Then
                    Call LoadBillingAO()
                End If

            Case "DailyBillSumm"
                Label1.Text = "Approving Officer"
                trDailyGL.Style("display") = "none"
                'ValDate.ControlToValidate = Nothing
                trInvPendingApp.Style("display") = ""
                trType.Style("display") = "none"
                If Not Page.IsPostBack Then
                    Call LoadBillingAO()
                End If

                'Zulham 18062015 - Added menu for billing tax invoice
                'Zulham 10/08/2017 - 
            Case "BillingTaxInvoice"
                Label19.Text = "Tax Invoice Number"
                trDailyGL.Style("display") = "none"
                trTaxInvoice.Style("display") = ""
                trType.Style("display") = "none"
                trReportType.Style("display") = "none"
                cboReportType.SelectedValue = "PDF"
                lblAction.Visible = False

                'Zulham 7-08-2017 - IPP Stage 3
            Case "BillingCreditNote"
                Label19.Text = "Credit Note Number"
                trDailyGL.Style("display") = "none"
                trTaxInvoice.Style("display") = ""
                trType.Style("display") = "none"
                trReportType.Style("display") = "none"
                cboReportType.SelectedValue = "PDF"
                lblAction.Visible = False
            Case "BillingCreditAdvice"
                Label19.Text = "Credit Advice Number"
                trDailyGL.Style("display") = "none"
                trTaxInvoice.Style("display") = ""
                trType.Style("display") = "none"
                trReportType.Style("display") = "none"
                cboReportType.SelectedValue = "PDF"
                lblAction.Visible = False
            Case "BillingDebitNote"
                Label19.Text = "Debit Note Number"
                trDailyGL.Style("display") = "none"
                trTaxInvoice.Style("display") = ""
                trType.Style("display") = "none"
                trReportType.Style("display") = "none"
                cboReportType.SelectedValue = "PDF"
                lblAction.Visible = False
            Case "BillingDebitAdvice"
                Label19.Text = "Debit Advice Number"
                trDailyGL.Style("display") = "none"
                trTaxInvoice.Style("display") = ""
                trType.Style("display") = "none"
                trReportType.Style("display") = "none"
                cboReportType.SelectedValue = "PDF"
                lblAction.Visible = False
                '''

        End Select
        lblHeader.Text = Request.QueryString("rptname")
        'strCoyType = lblHeader.Text.Substring(1, 1)
        ViewState("rptName") = Request.QueryString("type")

    End Sub

    Private Sub LoadFO()

        ddlFO.DataSource = GetFOId()
        ddlFO.DataTextField = "FOName"
        ddlFO.DataValueField = "UM_USER_ID"
        ddlFO.DataBind()

        Dim lst As New ListItem("---All---", "")
        ddlFO.Items.Insert(0, lst)
    End Sub

    Private Function GetFOId() As DataView
        Dim objDb As New EAD.DBCom
        Dim strSQl As String

        strSQl = "SELECT DISTINCT UM_USER_ID,CONCAT(UM_USER_ID,' : ',UM_USER_NAME) AS FOName " &
                "FROM user_mstr " &
                "INNER JOIN users_usrgrp ON UU_USER_ID=UM_USER_ID AND UU_COY_ID=UM_COY_ID " &
                "INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID AND UGM_DELETED='N' " &
                "WHERE UGM_FIXED_ROLE = '" & FixedRole.Finance_Officer.ToString.Replace("_", " ") & "' AND UM_DELETED='N' " &
                "AND UU_COY_ID='" & Session("CompanyID") & "' " &
                "ORDER BY UM_USER_ID"

        Return objDb.GetView(strSQl)
    End Function

    Private Sub LoadFM()

        ddlFO.DataSource = GetFMId()
        ddlFO.DataTextField = "FMName"
        ddlFO.DataValueField = "UM_USER_ID"
        ddlFO.DataBind()

        Dim lst As New ListItem("---All---", "")
        ddlFO.Items.Insert(0, lst)
    End Sub

    Private Function GetFMId() As DataView
        Dim objDb As New EAD.DBCom
        Dim strSQl As String

        strSQl = "SELECT UM_USER_ID,CONCAT(UM_USER_ID,' : ',UM_USER_NAME) AS FMName " &
                "FROM user_mstr " &
                "INNER JOIN users_usrgrp ON UU_USER_ID=UM_USER_ID AND UU_COY_ID=UM_COY_ID " &
                "INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID AND UGM_DELETED='N' " &
                "WHERE UGM_FIXED_ROLE = '" & FixedRole.Finance_Manager.ToString.Replace("_", " ") & "' AND UM_DELETED='N' " &
                "AND UU_COY_ID='" & Session("CompanyID") & "' " &
                "ORDER BY UM_USER_ID"

        Return objDb.GetView(strSQl)
    End Function

    'Jules 2015.03.03 - IPP Billing Stage 2A
    Private Sub LoadBillingAO()

        ddlFO.DataSource = GetBillingAO()
        ddlFO.DataTextField = "AOName"
        ddlFO.DataValueField = "UM_USER_ID"
        ddlFO.DataBind()

        Dim lst As New ListItem("---All---", "")
        ddlFO.Items.Insert(0, lst)
    End Sub

    Private Function GetBillingAO() As DataView
        Dim objDb As New EAD.DBCom
        Dim strSQl As String
        Dim objUsers As New Users
        Dim blnFinance As Boolean = False
        Dim strRole As String = "Billing Approving Officer"

        blnFinance = objUsers.checkUserFixedRole("'Billing Approving Officer(F)'", Session("UserId"))
        ViewState("AOF") = blnFinance

        If blnFinance Then
            strSQl = "SELECT DISTINCT UM_USER_ID,CONCAT(UM_USER_ID,' : ',UM_USER_NAME) AS AOName " &
               "FROM user_mstr " &
               "INNER JOIN users_usrgrp ON UU_USER_ID=UM_USER_ID AND UU_COY_ID=UM_COY_ID " &
               "INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID AND UGM_DELETED='N' " &
               "WHERE UGM_FIXED_ROLE LIKE '" & strRole & "%' "
        Else
            strSQl = "SELECT DISTINCT UM_USER_ID,CONCAT(UM_USER_ID,' : ',UM_USER_NAME) AS AOName " &
               "FROM user_mstr " &
               "INNER JOIN users_usrgrp ON UU_USER_ID=UM_USER_ID AND UU_COY_ID=UM_COY_ID " &
               "INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID AND UGM_DELETED='N' " &
               "INNER JOIN company_dept_mstr ON cdm_dept_code = um_dept_id " &
               "WHERE UGM_FIXED_ROLE = 'Billing Approving Officer' " &
               "AND cdm_dept_code= (SELECT um_dept_id FROM user_mstr WHERE um_user_id='" & Session("UserId") & "' AND UM_COY_ID = '" & Session("CompanyId") & "')"
        End If

        strSQl &= "AND UM_DELETED='N' AND UU_COY_ID='" & Session("CompanyID") & "' " &
                "ORDER BY UM_USER_ID"

        Return objDb.GetView(strSQl)
    End Function

    Private Sub ExportToExcel_DailyGL()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim strDate As String = ""
        Dim strFileName As String = ""

        strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                'Zulham 04/03/2015 IPP GST Stage 1
                'changed GL_GL_DATE to gl_stamp_on
                'added condition for coyID
                .CommandText = "SELECT GL_DEPT_CODE as 'HO/BR',concat('=""', GL_CC_CODE, '""')  as 'Cost Center', GL_GL_CODE as 'GL Number', GL_GL_DESC as 'GL Description',  " _
                    & "FORMAT(IF(GL_DORC = 'D',GL_AMT,0),2) as GL_AMT_DEBIT, " _
                    & "FORMAT(IF(GL_DORC = 'C',GL_AMT,0),2) as GL_AMT_CREDIT, " _
                    & "GL_ITEM_DESC as 'Item Description', " _
                    & "GL_PAYMENT_NO as 'Payment Advice No', " _
                    & "GL_PAYMENT_MODE as 'Mode of Payment', " _
                    & "GL_CHEQUE_NO as 'Cheque NO', " _
                    & "GL_FO_ID as 'FO ID', " _
                    & "GL_FM_ID as 'FM ID', " _
                    & "GL_MEMO as 'GL MEMO', " _
                    & "GL_CURRENCY_TYPE as 'Currency', GL_CONVERSION_RATE as 'Exchange Rate' " _
                    & "FROM GL_Entry_daily " _
                    & "WHERE CAST(gl_stamp_on AS DATE) = '" & strDate & "' and gl_coy_id = '" & Session("CompanyID") & "' " _
                    & "ORDER  BY GL_DEPT_CODE, GL_CC_CODE, GL_GL_CODE"
            End With

            '& "INNER JOIN invoice_mstr ON im_invoice_index = gl_invoice_index " _
            '& "INNER JOIN invoice_details ON id_invoice_no = im_invoice_no AND id_s_coy_id = im_s_coy_id " _

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
            strFileName = "IPPD0002 - Daily GL Entries Listing" & "(" & Format(Now, "ddMMMyyyy") & ").xls"
            Dim attachment As String = "attachment;filename=" & strFileName
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"

            Dim dc As DataColumn
            Dim i As Integer

            i = 0
            For Each dc In ds.Tables(0).Columns
                If i > 0 Then
                    Response.Write(vbTab + dc.ColumnName)
                Else
                    Response.Write(dc.ColumnName)
                End If
                i += 1

            Next
            Response.Write(vbCrLf)

            Dim dr As DataRow
            For Each dr In ds.Tables(0).Rows
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    If i > 0 Then
                        Response.Write(vbTab + dr.Item(i).ToString)
                    Else
                        Response.Write(dr.Item(i).ToString)
                    End If
                Next
                Response.Write(vbCrLf)
            Next
            Response.End()

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try

    End Sub

    Private Sub ExportToPDF_DailyGL()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strUserId As String
        Dim strCoyName As String
        Dim strDate As String = ""
        Dim objDb As New EAD.DBCom
        Dim strFileName As String = ""

        strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")
        ViewState("txtDate") = strDate
        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                '& "FORMAT(IF(GL_DORC = 'D',GL_AMT,0),2) as GL_AMT_DEBIT, " _
                '                       & "FORMAT(IF(GL_DORC = 'C',GL_AMT,0),2) as GL_AMT_CREDIT, " _
                'Nov 7

                'Zulham 200152015 IPP GST Stage 1,2,2A
                'Get user's group
                Dim usrGrp = objDb.GetVal("Select uu_usrgrp_id from users_usrgrp where uu_coy_id = '" & Session("CompanyID") & "' and uu_user_id = '" & Session("UserId") & "' and (uu_usrgrp_id like '%Report%' or uu_usrgrp_id like '%rpt%')")

                'Zulham 04/03/2015 IPP GST Stage 1
                ' changed GL_GL_DATE to gl_stamp_on
                .CommandText = "SELECT GL_DEPT_CODE ,GL_CC_CODE, GL_GL_CODE , GL_GL_DESC,  " _
                        & "IF(GL_DORC = 'D',round(GL_AMT,2),0) as GL_AMT_DEBIT, " _
                        & "IF(GL_DORC = 'C',round(GL_AMT,2),0) as GL_AMT_CREDIT, " _
                        & "GL_ITEM_DESC, " _
                        & "GL_PAYMENT_NO, " _
                        & "GL_PAYMENT_MODE, " _
                        & "GL_CHEQUE_NO , " _
                        & "GL_FO_ID as FO_NAME , " _
                        & "GL_FM_ID as FM_NAME, GL_DORC,GL_MEMO, GL_CURRENCY_TYPE,GL_FC_TYPE, GL_CONVERSION_RATE, " _
                        & "(SELECT FORMAT(SUM(GL_AMT),2) FROM gl_entry_daily WHERE CAST(gl_stamp_on AS DATE) = '" & strDate & "' AND gl_dorc = 'd' and gl_coy_id = '" & Session("CompanyID") & "') AS TotalDebitAmt, " _
                        & "(SELECT FORMAT(SUM(GL_AMT),2) FROM gl_entry_daily WHERE CAST(gl_stamp_on AS DATE) = '" & strDate & "' AND gl_dorc = 'c' and gl_coy_id = '" & Session("CompanyID") & "') AS TotalCreditAmt " _
                        & "FROM GL_Entry_daily " _
                        & "WHERE CAST(gl_stamp_on AS DATE) = '" & strDate & "' and gl_coy_id = '" & Session("CompanyID") & "' "

                If usrGrp.ToString.ToUpper.Contains("BILLING") Then
                    .CommandText = .CommandText & " and gl_payment_no not like '%pa%' "
                Else
                    .CommandText = .CommandText & " and gl_payment_no like '%pa%' "
                End If

                .CommandText = .CommandText & "ORDER  BY GL_DEPT_CODE, GL_CC_CODE, GL_GL_CODE"

                '& "WHERE CAST(GL_GL_DATE AS DATE) = '" & strDate & "' " _
                ' & "INNER JOIN invoice_mstr ON im_invoice_index = gl_invoice_index " _
                '& "INNER JOIN invoice_details ON id_invoice_no = im_invoice_no AND id_s_coy_id = im_s_coy_id " _
                '& "IF(GL_DORC = 'D',(id_received_qty * id_unit_cost),0) as GL_AMT_DEBIT, " _
                '& "IF(GL_DORC = 'C',(id_received_qty * id_unit_cost),0) as GL_AMT_CREDIT, " _
                ' GL_FO_ID as 'Verifier',
                ' GL_FM_ID as 'Finanace Manager',
            End With

            da = New MySqlDataAdapter(cmd)


            ' da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyID")))
            strUserId = Session("UserName") 'Session("UserId")
            strCoyName = Session("CompanyName")

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("DailyGL_DailyGL_DatasetDailyGL", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)

            localreport.ReportPath = dispatcher.direct("Report", "DailyGL.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")

            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            If TotalParameter > 0 Then
                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
                For I = 0 To localreport.GetParameters.Count - 1
                    GetParameter = localreport.GetParameters.Item(I).Name
                    Select Case LCase(GetParameter)
                        Case "prmrequestedby"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

                        Case "logo"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                        Case "prmdate"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, Format(CDate(strDate), "dd/MM/yyyy"))

                        Case "prmbuyercoyname"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)

                        Case Else
                    End Select
                Next
                localreport.SetParameters(par)
            End If


            'Load sub report
            AddHandler localreport.SubreportProcessing, AddressOf SetSubReportDS
            AddHandler localreport.SubreportProcessing, AddressOf SetSubReportDS2
            localreport.Refresh()

            'localreport.Refresh()

            Dim deviceInfo As String =
                "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            strFileName = "IPPD0002 - Daily GL Entries Listing.pdf"
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()

            'Session("CompanyName") = Nothing
        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub

    Private Sub SetSubReportDS(ByVal sender As Object, ByVal e As SubreportProcessingEventArgs)

        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim strDate As String = ""
        'strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")
        strDate = ViewState("txtDate")
        myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";"

        conn = New MySqlConnection(myConnectionString)
        'conn.Open()
        'End If



        cmd = New MySqlCommand
        With cmd
            .Connection = conn
            .CommandType = CommandType.Text

            'Zulham 200152015 IPP GST Stage 1,2,2A
            'Get user's group
            Dim objDb As New EAD.DBCom
            Dim usrGrp = objDb.GetVal("Select uu_usrgrp_id from users_usrgrp where uu_coy_id = '" & Session("CompanyID") & "' and uu_user_id = '" & Session("UserId") & "' and (uu_usrgrp_id like '%Report%' or uu_usrgrp_id like '%rpt%')")

            If usrGrp.ToString.ToUpper.Contains("BILLING") Then
                'Zulham 04/03/2015 IPP GST Stage 1
                ' changed GL_GL_DATE to gl_stamp_on
                'added condition for CoyId
                .CommandText = "SELECT GL_DEPT_CODE,SUM(GL_AMT_DEBIT) AS GL_AMT_DEBIT,SUM(GL_AMT_CREDIT) AS GL_AMT_CREDIT ,GL_CURRENCY_TYPE,GL_FC_TYPE FROM (" _
                        & "SELECT GL_DEPT_CODE,IF(GL_DORC = 'D',SUM(GL_AMT),0) AS GL_AMT_DEBIT, IF(GL_DORC = 'C',SUM(GL_AMT),0) AS GL_AMT_CREDIT, " _
                        & "IF(GL_CURRENCY_TYPE='' AND GL_FC_TYPE='','MYR',GL_CURRENCY_TYPE) AS GL_CURRENCY_TYPE, " _
                        & "IF(GL_FC_TYPE='' AND GL_CURRENCY_TYPE='','MYR',GL_CURRENCY_TYPE) AS GL_FC_TYPE " _
                        & "FROM GL_Entry_daily " _
                        & "WHERE GL_DEPT_CODE = @BranchCode and CAST(gl_stamp_on AS DATE) = '" & strDate & "' and gl_coy_id = '" & Session("CompanyID") & "' " _
                        & "and gl_payment_no not like '%pa%' " _
                        & "GROUP BY GL_CURRENCY_TYPE,GL_FC_TYPE,gl_dorc,gl_dept_code " _
                        & "ORDER  BY GL_DEPT_CODE, GL_CC_CODE, GL_GL_CODE ) AS zzz " _
                        & "GROUP BY GL_CURRENCY_TYPE,GL_FC_TYPE"

                '.CommandText = .CommandText & " and gl_payment_no not like '%pa%' "
            Else
                'Zulham 04/03/2015 IPP GST Stage 1
                ' changed GL_GL_DATE to gl_stamp_on
                'added condition for CoyId
                .CommandText = "SELECT GL_DEPT_CODE,SUM(GL_AMT_DEBIT) AS GL_AMT_DEBIT,SUM(GL_AMT_CREDIT) AS GL_AMT_CREDIT ,GL_CURRENCY_TYPE,GL_FC_TYPE FROM (" _
                        & "SELECT GL_DEPT_CODE,IF(GL_DORC = 'D',SUM(GL_AMT),0) AS GL_AMT_DEBIT, IF(GL_DORC = 'C',SUM(GL_AMT),0) AS GL_AMT_CREDIT, " _
                        & "IF(GL_CURRENCY_TYPE='' AND GL_FC_TYPE='','MYR',GL_CURRENCY_TYPE) AS GL_CURRENCY_TYPE, " _
                        & "IF(GL_FC_TYPE='' AND GL_CURRENCY_TYPE='','MYR',GL_CURRENCY_TYPE) AS GL_FC_TYPE " _
                        & "FROM GL_Entry_daily " _
                        & "WHERE GL_DEPT_CODE = @BranchCode and CAST(gl_stamp_on AS DATE) = '" & strDate & "' and gl_coy_id = '" & Session("CompanyID") & "' " _
                        & "and gl_payment_no like '%pa%' " _
                        & "GROUP BY GL_CURRENCY_TYPE,GL_FC_TYPE,gl_dorc,gl_dept_code " _
                        & "ORDER  BY GL_DEPT_CODE, GL_CC_CODE, GL_GL_CODE ) AS zzz " _
                        & "GROUP BY GL_CURRENCY_TYPE,GL_FC_TYPE"

                '.CommandText = .CommandText & " and gl_payment_no like '%pa%' "
            End If
        End With

        da = New MySqlDataAdapter(cmd)
        If e.Parameters.Count > 0 Then
            'If e.Parameters(0).Values(0).ToString.Trim <> "" Then
            'da.SelectCommand.Parameters.Add(New MySqlParameter("@p_invoice_no", e.Parameters(0).Values(0).ToString))
            'da.SelectCommand.Parameters.Add(New MySqlParameter("@p_s_coy_name", e.Parameters(1).Values(0).ToString))
            da.SelectCommand.Parameters.Add(New MySqlParameter("@BranchCode", e.Parameters(0).Values(0).ToString))
            'da.SelectCommand.Parameters.Add(New MySqlParameter("@p_s_coy_name", e.Parameters(1).Values(0).ToString))
            'Else
            'e.Parameters.Item(0).("@p_invoice_no"))
            'da.SelectCommand.Parameters.RemoveAt("@p_s_coy_name")
            'da.SelectCommand.Parameters.Add(New MySqlParameter("@p_invoice_no", "0"))
            'da.SelectCommand.Parameters.Add(New MySqlParameter("@p_s_coy_name", "0"))
            'End If
        End If
        'da.SelectCommand.Parameters.Add(New MySqlParameter("@p_invoice_no", "INV000002"))
        'da.SelectCommand.Parameters.Add(New MySqlParameter("@p_s_coy_name", "Caroline Tang Ai Ling Company"))
        da.Fill(ds)

        Dim subDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("DailyGLSubReport_DailyGLSubReport_DatasetDailyGLSubReport", ds.Tables(0))
        e.DataSources.Add(subDataSource)

    End Sub
    Private Sub SetSubReportDS2(ByVal sender As Object, ByVal e As SubreportProcessingEventArgs)

        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim strDate As String = ""
        'strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")
        strDate = ViewState("txtDate")
        myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";"

        conn = New MySqlConnection(myConnectionString)
        'conn.Open()
        'End If



        cmd = New MySqlCommand
        With cmd
            .Connection = conn
            .CommandType = CommandType.Text

            'Zulham 30072015
            'Get user's group
            Dim objDb As New EAD.DBCom
            Dim usrGrp = objDb.GetVal("Select uu_usrgrp_id from users_usrgrp where uu_coy_id = '" & Session("CompanyID") & "' and uu_user_id = '" & Session("UserId") & "' and (uu_usrgrp_id like '%Report%' or uu_usrgrp_id like '%rpt%')")

            If usrGrp.ToString.ToUpper.Contains("BILLING") Then
                'Zulham 04/03/2015 IPP GST Stage 1
                ' changed GL_GL_DATE to gl_stamp_on
                'added condition for coyId
                .CommandText = "SELECT GL_DEPT_CODE,SUM(GL_AMT_DEBIT) AS GL_AMT_DEBIT,SUM(GL_AMT_CREDIT) AS GL_AMT_CREDIT ,GL_CURRENCY_TYPE,GL_FC_TYPE FROM (" _
                        & "SELECT GL_DEPT_CODE,IF(GL_DORC = 'D',SUM(GL_AMT),0) AS GL_AMT_DEBIT, IF(GL_DORC = 'C',SUM(GL_AMT),0) AS GL_AMT_CREDIT, " _
                        & "IF(GL_CURRENCY_TYPE='' AND GL_FC_TYPE='','MYR',GL_CURRENCY_TYPE) AS GL_CURRENCY_TYPE, " _
                        & "IF(GL_FC_TYPE='' AND GL_CURRENCY_TYPE='','MYR',GL_CURRENCY_TYPE) AS GL_FC_TYPE " _
                        & "FROM GL_Entry_daily " _
                        & "WHERE CAST(gl_stamp_on AS DATE) = '" & strDate & "' and gl_coy_id = '" & Session("CompanyID") & "' " _
                        & "and gl_payment_no not like '%pa%' " _
                        & "GROUP BY GL_CURRENCY_TYPE,GL_FC_TYPE,gl_dorc,gl_dept_code " _
                        & "ORDER  BY GL_DEPT_CODE, GL_CC_CODE, GL_GL_CODE ) AS zzz " _
                        & "GROUP BY GL_CURRENCY_TYPE,GL_FC_TYPE"

            Else
                'Zulham 04/03/2015 IPP GST Stage 1
                ' changed GL_GL_DATE to gl_stamp_on
                'added condition for coyId
                .CommandText = "SELECT GL_DEPT_CODE,SUM(GL_AMT_DEBIT) AS GL_AMT_DEBIT,SUM(GL_AMT_CREDIT) AS GL_AMT_CREDIT ,GL_CURRENCY_TYPE,GL_FC_TYPE FROM (" _
                        & "SELECT GL_DEPT_CODE,IF(GL_DORC = 'D',SUM(GL_AMT),0) AS GL_AMT_DEBIT, IF(GL_DORC = 'C',SUM(GL_AMT),0) AS GL_AMT_CREDIT, " _
                        & "IF(GL_CURRENCY_TYPE='' AND GL_FC_TYPE='','MYR',GL_CURRENCY_TYPE) AS GL_CURRENCY_TYPE, " _
                        & "IF(GL_FC_TYPE='' AND GL_CURRENCY_TYPE='','MYR',GL_CURRENCY_TYPE) AS GL_FC_TYPE " _
                        & "FROM GL_Entry_daily " _
                        & "WHERE CAST(gl_stamp_on AS DATE) = '" & strDate & "' and gl_coy_id = '" & Session("CompanyID") & "' " _
                        & "and gl_payment_no like '%pa%' " _
                        & "GROUP BY GL_CURRENCY_TYPE,GL_FC_TYPE,gl_dorc,gl_dept_code " _
                        & "ORDER  BY GL_DEPT_CODE, GL_CC_CODE, GL_GL_CODE ) AS zzz " _
                        & "GROUP BY GL_CURRENCY_TYPE,GL_FC_TYPE"

            End If




        End With

        da = New MySqlDataAdapter(cmd)
        'If e.Parameters.Count > 0 Then
        'If e.Parameters(0).Values(0).ToString.Trim <> "" Then
        'da.SelectCommand.Parameters.Add(New MySqlParameter("@p_invoice_no", e.Parameters(0).Values(0).ToString))
        'da.SelectCommand.Parameters.Add(New MySqlParameter("@p_s_coy_name", e.Parameters(1).Values(0).ToString))
        'da.SelectCommand.Parameters.Add(New MySqlParameter("@BranchCode", e.Parameters(0).Values(0).ToString))
        'da.SelectCommand.Parameters.Add(New MySqlParameter("@p_s_coy_name", e.Parameters(1).Values(0).ToString))
        'Else
        'e.Parameters.Item(0).("@p_invoice_no"))
        'da.SelectCommand.Parameters.RemoveAt("@p_s_coy_name")
        'da.SelectCommand.Parameters.Add(New MySqlParameter("@p_invoice_no", "0"))
        'da.SelectCommand.Parameters.Add(New MySqlParameter("@p_s_coy_name", "0"))
        'End If
        'End If
        'da.SelectCommand.Parameters.Add(New MySqlParameter("@p_invoice_no", "INV000002"))
        'da.SelectCommand.Parameters.Add(New MySqlParameter("@p_s_coy_name", "Caroline Tang Ai Ling Company"))
        da.Fill(ds)

        Dim subDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("DailyGLSubReport2_DatasetDailyGLSubReport", ds.Tables(0))
        e.DataSources.Add(subDataSource)

    End Sub
    Private Sub ExportToExcel_InvPendingApp()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        'Dim strDate As String = ""
        Dim strFileName As String = ""

        'strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")

        Try

            'Zulham 26/02/2015 Case 8317
            Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                'Zulham 26/02/2015 Case 8317
                .CommandText = "SELECT concat('=""', IM_INVOICE_NO, '""') AS 'Document No.',ic_bank_code AS 'Bank Name', concat('=""', ic_bank_acct, '""') AS 'Bank A/C No.',DATE_FORMAT(IM_DOC_DATE,'%d/%m/%Y') AS 'Document Date', IM_PAYMENT_TERM AS 'Payment Mode',DATE_FORMAT(IM_PRCS_SENT,'%d/%m/%Y') AS 'PSD Sent Date',DATE_FORMAT(IM_PRCS_RECV,'%d/%m/%Y') AS 'PSD Received Date'," &
                                "IM_CREATED_BY AS 'Teller ID',FORMAT(IF(IM_INVOICE_TYPE='CN',CONCAT('-',IM_INVOICE_TOTAL),IM_INVOICE_TOTAL),2) AS 'Purchase Amount', FORMAT(if(im_invoice_type = 'CN',concat('-',SUM(IFNULL(ID_GST_VALUE,0))),SUM(IFNULL(ID_GST_VALUE,0))),2) AS 'GST Amount', " &
                                "FORMAT((SUM(IFNULL(ID_GST_VALUE,0)) + SUM(ID_RECEIVED_QTY*ID_UNIT_COST)),2) AS 'Total Invoice Amount',IM_CURRENCY_CODE AS 'Currency', " &
                                "CONCAT(UM_USER_ID,' : ',UM_USER_NAME) AS 'FO ID',IM_S_COY_NAME AS 'Vendor Name' " &
                                "FROM invoice_mstr " &
                                "LEFT OUTER JOIN ipp_company ON IM_S_COY_ID=ic_index AND ic_coy_id='" & strDefIPPCompID & "' " &
                                "LEFT OUTER JOIN bank_code ON ic_bank_code=bc_bank_code AND bc_coy_id='" & strDefIPPCompID & "' " &
                                "INNER JOIN invoice_Details ON id_s_coy_id=im_s_coy_id AND id_invoice_no=im_invoice_no " &
                                "INNER JOIN user_mstr ON IM_STATUS_CHANGED_BY=UM_USER_ID AND IM_B_COY_ID=UM_COY_ID AND UM_DELETED='N' " &
                                "INNER JOIN users_usrgrp ON UU_USER_ID=UM_USER_ID AND UU_COY_ID=UM_COY_ID " &
                                "INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID AND UGM_DELETED='N' " &
                                "WHERE IM_INVOICE_TYPE IS NOT NULL AND UGM_FIXED_ROLE = '" & FixedRole.Finance_Officer.ToString.Replace("_", " ") & "' " &
                                "AND UGM_USRGRP_NAME LIKE 'IPP%' AND IM_B_COY_ID='" & Session("CompanyID") & "' AND IM_INVOICE_STATUS = 12"
                If ViewState("FO") <> "" Then
                    .CommandText = .CommandText & " AND IM_STATUS_CHANGED_BY='" & ViewState("FO") & "'"
                End If

                If rdbtnVendor.Checked Then
                    .CommandText = .CommandText & " AND ic_coy_type='V' "
                End If

                If rdbtnEmployee.Checked Then
                    .CommandText = .CommandText & " AND ic_coy_type='E' "
                End If

                .CommandText = .CommandText & " GROUP BY im_invoice_no, im_s_coy_id ORDER BY im_submit_date ASC "
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
            strFileName = "IPPD0004 - SummaryInvoicesPendingApprovalReport" & "(" & Format(Now, "ddMMMyyyy") & ").xls"
            Dim attachment As String = "attachment;filename=" & strFileName
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"

            Dim dc As DataColumn
            Dim i As Integer

            i = 0
            For Each dc In ds.Tables(0).Columns
                If i > 0 Then
                    Response.Write(vbTab + dc.ColumnName)
                Else
                    Response.Write(dc.ColumnName)
                End If
                i += 1

            Next
            Response.Write(vbCrLf)

            Dim dr As DataRow
            For Each dr In ds.Tables(0).Rows
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    If i > 0 Then
                        Response.Write(vbTab + dr.Item(i).ToString)
                    Else
                        Response.Write(dr.Item(i).ToString)
                    End If
                Next
                Response.Write(vbCrLf)
            Next
            Response.End()

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try

    End Sub

    Private Sub ExportToPDF_InvPendingApp()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strUserId As String
        Dim strCoyName As String
        'Dim strDate As String = ""
        Dim strFileName As String = ""

        'strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try

            'Zulham 26/02/2015 Case 8317
            Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                'Zulham 26/02/2015 Case 8317
                '.CommandText = "SELECT IM_INVOICE_NO,IM_S_COY_ID,IM_S_COY_NAME,ic_bank_acct,bc_bank_name,ic_bank_code,IM_DOC_DATE,IM_DUE_DATE,IM_PRCS_SENT,IM_PRCS_RECV," & _
                '            "IM_CREATED_BY, IM_CURRENCY_CODE,  IM_INVOICE_TOTAL, IM_INVOICE_STATUS, IM_INVOICE_TYPE, IM_STATUS_CHANGED_BY, UM_USER_ID, UM_USER_NAME, IM_B_COY_ID, IM_PAYMENT_TERM ,IF(im_invoice_type='CN',im_invoice_total,0) AS im_cn_total, " & _
                '            "IF(im_invoice_type<>'CN',im_invoice_total,0) AS im_invoice_no_cn_total, " & _
                '            "IF(im_payment_term = 'IBG' AND im_invoice_type<>'CN',im_invoice_total,0) AS im_invoice_total_ibg,IF(im_payment_term = 'CASA' AND im_invoice_type<>'CN',im_invoice_total,0) AS im_invoice_total_casa, " & _
                '            "IF(im_payment_term = 'IBG' AND im_invoice_type='CN',im_invoice_total,0) AS im_invoice_total_cn_ibg,IF(im_payment_term = 'CASA' AND im_invoice_type='CN',im_invoice_total,0) AS im_invoice_total_cn_casa, " & _
                '            "FORMAT(SUM(IFNULL(ID_GST_VALUE,0)),2) AS gst_amt, FORMAT(SUM(ID_RECEIVED_QTY*ID_UNIT_COST),2) AS total_wo_gst, " & _
                '            "IF(im_invoice_type='CN',SUM(ID_RECEIVED_QTY*ID_UNIT_COST),0) AS im_cn_total_no_gst, " & _
                '            "IF(im_invoice_type<>'CN',SUM(ID_RECEIVED_QTY*ID_UNIT_COST),0) AS im_invoice_no_cn_total_no_gst, " & _
                '            "IF(im_invoice_type='CN',SUM(IFNULL(ID_GST_VALUE,0)),0) AS im_cn_total_gst_amt, " & _
                '            "IF(im_invoice_type<>'CN',SUM(IFNULL(ID_GST_VALUE,0)),0) AS im_invoice_no_cn_total_gst_amt " & _
                '            "FROM invoice_mstr " & _
                '            "LEFT OUTER JOIN ipp_company ON IM_S_COY_ID=ic_index AND ic_coy_id='" & strDefIPPCompID & "' " & _
                '            "LEFT OUTER JOIN bank_code ON ic_bank_code=bc_bank_code AND bc_coy_id='" & strDefIPPCompID & "' " & _
                '            "INNER JOIN invoice_Details ON id_s_coy_id=im_s_coy_id AND id_invoice_no=im_invoice_no " & _
                '            "INNER JOIN user_mstr ON IM_STATUS_CHANGED_BY=UM_USER_ID AND IM_B_COY_ID=UM_COY_ID AND UM_DELETED='N' " & _
                '            "INNER JOIN users_usrgrp ON UU_USER_ID=UM_USER_ID AND UU_COY_ID=UM_COY_ID " & _
                '            "INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID AND UGM_DELETED='N' " & _
                '            "WHERE IM_INVOICE_TYPE IS NOT NULL AND UGM_FIXED_ROLE = '" & FixedRole.Finance_Officer.ToString.Replace("_", " ") & "' " & _
                '            "AND UGM_USRGRP_NAME LIKE 'IPP%' AND IM_B_COY_ID='" & Session("CompanyID") & "' AND IM_INVOICE_STATUS = 12"
                'If ViewState("FO") <> "" Then
                '    .CommandText = .CommandText & " AND IM_STATUS_CHANGED_BY='" & ViewState("FO") & "'"
                'End If

                'If rdbtnVendor.Checked Then
                '    .CommandText = .CommandText & " AND ic_coy_type='V' "
                'End If

                'If rdbtnEmployee.Checked Then
                '    .CommandText = .CommandText & " AND ic_coy_type='E' "
                'End If

                '.CommandText = .CommandText & " GROUP BY im_invoice_no, im_s_coy_id ORDER BY im_submit_date ASC "

                'Zulham 14/07/2016
                'IM5/Im6 Enhancement - Added additional condition for local gst amount and added new column called gt_amt_foreign
                .CommandText = "SELECT IM_INVOICE_NO,IM_S_COY_ID,IM_S_COY_NAME,ic_bank_acct,bc_bank_name,ic_bank_code,IM_DOC_DATE,IM_DUE_DATE,IM_PRCS_SENT,IM_PRCS_RECV, " &
                "IM_CREATED_BY, IM_CURRENCY_CODE,  IM_INVOICE_TOTAL, IM_INVOICE_STATUS, IM_INVOICE_TYPE, IM_STATUS_CHANGED_BY, UM_USER_ID, UM_USER_NAME,  " &
                "IM_B_COY_ID, IM_PAYMENT_TERM ,IF(im_invoice_type='CN',im_invoice_total,0) AS im_cn_total, IF(im_invoice_type<>'CN', IF(TRIM(id_gst_input_tax_code) NOT IN ('im1','im3'), SUM(ID_RECEIVED_QTY*ID_UNIT_COST) + sum(id_gst_value), SUM(ID_RECEIVED_QTY*ID_UNIT_COST)),0)  AS im_invoice_no_cn_total, " &
                "IF(im_payment_term = 'IBG' AND im_invoice_type<>'CN',im_invoice_total,0) AS im_invoice_total_ibg,IF " &
                "(im_payment_term = 'CASA' AND im_invoice_type<>'CN',im_invoice_total,0) AS im_invoice_total_casa, IF(im_payment_term = 'IBG' AND  " &
                "im_invoice_type='CN',im_invoice_total,0) AS im_invoice_total_cn_ibg,IF(im_payment_term = 'CASA' AND im_invoice_type='CN',im_invoice_total,0) " &
                "AS im_invoice_total_cn_casa, FORMAT(IF(im_invoice_type = 'CN',0 - SUM(IFNULL(ID_GST_VALUE,0)),SUM(IF(TRIM(id_gst_input_tax_code) = 'IM5' OR im_currency_code = 'MYR',IFNULL(ID_GST_VALUE,0),0))),2) AS 'gst_amt', FORMAT(SUM(ID_RECEIVED_QTY*ID_UNIT_COST),2) AS total_wo_gst,  " &
                "IF(im_invoice_type='CN',SUM(ID_RECEIVED_QTY*ID_UNIT_COST),0) AS im_cn_total_no_gst, IF(im_invoice_type<>'CN', " &
                "SUM(ID_RECEIVED_QTY*ID_UNIT_COST),0) AS im_invoice_no_cn_total_no_gst, IF(im_invoice_type='CN',SUM(IFNULL(ID_GST_VALUE,0)),0) AS  " &
                "im_cn_total_gst_amt, IF(im_invoice_type<>'CN' AND (im_currency_code = 'MYR' OR id_gst_input_Tax_code = 'IM5'),SUM(IFNULL(ID_GST_VALUE,0)),0) AS im_invoice_no_cn_total_gst_amt, id_gst_input_tax_code, id_gst_output_tax_code, " &
                "FORMAT(IF(im_invoice_type = 'CN' AND TRIM(id_gst_input_Tax_code) NOT IN ('im5','tx4') AND im_currency_code <> 'MYR',0 - SUM(IFNULL(ID_GST_VALUE,0)), " &
                "SUM(IF(TRIM(id_gst_input_tax_code) <> 'IM5' AND im_currency_code <> 'MYR' AND TRIM(id_gst_input_tax_code) <> 'TX4', " &
                "IFNULL(ID_GST_VALUE,0),0))),2) AS 'gst_amt_foreign'    " &
                "FROM invoice_mstr  " &
                "LEFT OUTER JOIN ipp_company ON IM_S_COY_ID=ic_index AND ic_coy_id='" & strDefIPPCompID & "' " &
                "LEFT OUTER JOIN bank_code ON ic_bank_code=bc_bank_code AND bc_coy_id='" & strDefIPPCompID & "'  " &
                "INNER JOIN invoice_Details ON id_s_coy_id=im_s_coy_id AND id_invoice_no=im_invoice_no  " &
                "INNER JOIN user_mstr ON IM_STATUS_CHANGED_BY=UM_USER_ID AND IM_B_COY_ID=UM_COY_ID AND UM_DELETED='N'  " &
                "INNER JOIN users_usrgrp ON UU_USER_ID=UM_USER_ID AND UU_COY_ID=UM_COY_ID  " &
                "INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID AND UGM_DELETED='N'  " &
                "WHERE IM_INVOICE_TYPE IS NOT NULL  " &
                "AND UGM_FIXED_ROLE = '" & FixedRole.Finance_Officer.ToString.Replace("_", " ") & "'  " &
                "AND UGM_USRGRP_NAME LIKE 'IPP%' AND IM_B_COY_ID='" & Session("CompanyID") & "' " &
                "AND IM_INVOICE_STATUS = 12 "

                If ViewState("FO") <> "" Then
                    .CommandText = .CommandText & " AND IM_STATUS_CHANGED_BY='" & ViewState("FO") & "'"
                End If

                If rdbtnVendor.Checked Then
                    .CommandText = .CommandText & " AND ic_coy_type='V' "
                End If

                If rdbtnEmployee.Checked Then
                    .CommandText = .CommandText & " AND ic_coy_type='E' "
                End If

                .CommandText = .CommandText & " GROUP BY im_invoice_no, im_s_coy_id ORDER BY im_submit_date ASC "


            End With

            da = New MySqlDataAdapter(cmd)
            strUserId = Session("UserName")
            strCoyName = Session("CompanyName")

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("InvPendingApp_dtInvPendingApp", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = dispatcher.direct("Report", "InvPendingApp.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")

            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            If TotalParameter > 0 Then
                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
                For I = 0 To localreport.GetParameters.Count - 1
                    GetParameter = localreport.GetParameters.Item(I).Name
                    Select Case LCase(GetParameter)
                        Case "prmrequestedby"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

                        Case "logo"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                        Case "prmtitle"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, Format(Now, "dd/MM/yyyy"))

                        Case "prmbuyercoyname"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)

                        Case Else
                    End Select
                Next
                localreport.SetParameters(par)
            End If

            localreport.Refresh()

            Dim deviceInfo As String =
                    "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            strFileName = "IPPD0004 - SummaryInvoicesPendingApprovalReport.pdf"
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()


        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub

    Private Sub ExportToExcel_InvReleased()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        'Dim strDate As String = ""
        Dim strFileName As String = ""

        'strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            'CheeHong 9/Mar/2015 Case 8317
            Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT concat('=""', IM_INVOICE_NO, '""')  AS 'Document No.',ic_bank_code AS 'Bank Name',concat('=""', ic_bank_acct, '""') AS 'Bank A/C No.'," &
                           "DATE_FORMAT(IM_DOC_DATE,'%d/%m/%Y') AS 'Document Date', " &
                           "DATE_FORMAT(IM_PRCS_SENT,'%d/%m/%Y') AS 'PSD Sent Date',DATE_FORMAT(IM_PRCS_RECV,'%d/%m/%Y') AS 'PSD Received Date'," &
                           "IM_CREATED_BY AS 'Teller ID',(SELECT FA_ACTIVE_AO FROM finance_approval WHERE IM_INVOICE_INDEX=FA_INVOICE_INDEX AND FA_AGA_TYPE='FO' ORDER BY FA_SEQ DESC LIMIT 1) AS 'Finance Officer ID'," &
                           "IM_PAYMENT_NO AS 'Payment Advice No.',IF(IM_CURRENCY_CODE='MYR','',IM_CURRENCY_CODE) AS 'Foreign Currency',IF(IM_CURRENCY_CODE='MYR','',FORMAT(SUM(id_received_qty*id_unit_cost),2)) AS 'Purchase Amount in Foreign Currency'," &
                           "'MYR' AS 'Local Currency', " &
                           "FORMAT(IF(IM_INVOICE_TYPE='CN',CONCAT('-',IF(IM_EXCHANGE_RATE IS NOT NULL,IF(im_payment_term in ('TT','NOSTRO'),(SUM(id_received_qty*id_unit_cost) * im_exchange_rate), " &
                           "(SUM(id_received_qty*id_unit_cost)))  - IF(im_payment_term in ('TT','NOSTRO'),(SUM(id_received_qty*id_unit_cost) * im_exchange_rate),(SUM(id_received_qty*id_unit_cost)))  * " &
                           "IF(IM_WITHHOLDING_OPT = 2,(IFNULL(IM_WITHHOLDING_TAX,0)/100),0),SUM(id_received_qty*id_unit_cost))), IF(IM_EXCHANGE_RATE IS NOT NULL,IF(im_payment_term in ('TT','NOSTRO'), " &
                           "(SUM(id_received_qty*id_unit_cost) * im_exchange_rate),(SUM(id_received_qty*id_unit_cost)))  - IF(im_payment_term in ('TT','NOSTRO'), " &
                           "(SUM(id_received_qty*id_unit_cost) * im_exchange_rate),(SUM(id_received_qty*id_unit_cost)))  * IF(IM_WITHHOLDING_OPT = 2, " &
                           "(IFNULL(IM_WITHHOLDING_TAX,0)/100),0),SUM(id_received_qty*id_unit_cost))),2) AS 'Purchase Amount in Local Currency', " &
                           "FORMAT(SUM(IFNULL(ID_GST_VALUE,0)),2) AS 'GST Amount', " &
                           "FORMAT(IF(IM_INVOICE_TYPE='CN',CONCAT('-',IF(IM_EXCHANGE_RATE IS NOT NULL,IF(im_payment_term in ('TT','NOSTRO'),(im_invoice_total * im_exchange_rate), " &
                           "(im_invoice_total))  - IF(im_payment_term in ('TT','NOSTRO'),(im_invoice_total * im_exchange_rate),(im_invoice_total))  * " &
                           "IF(IM_WITHHOLDING_OPT = 2,(IFNULL(IM_WITHHOLDING_TAX,0)/100),0),IM_INVOICE_TOTAL)), IF(IM_EXCHANGE_RATE IS NOT NULL,IF(im_payment_term in ('TT','NOSTRO'), " &
                           "(im_invoice_total * im_exchange_rate),(im_invoice_total))  - IF(im_payment_term in ('TT','NOSTRO'), " &
                           "(im_invoice_total * im_exchange_rate),(im_invoice_total))  * IF(IM_WITHHOLDING_OPT = 2, " &
                           "(IFNULL(IM_WITHHOLDING_TAX,0)/100),0),IM_INVOICE_TOTAL)),2) AS 'Total Invoice Amount in Local Currency', " &
                           "CONCAT(UM_USER_ID,' : ',UM_USER_NAME) AS 'FM ID',IM_PAYMENT_TERM AS 'Payment Mode',IM_S_COY_NAME AS 'Vendor Name',im_withholding_tax as 'WHT' " &
                           "FROM invoice_mstr " &
                           "LEFT OUTER JOIN ipp_company ON IM_S_COY_ID=ic_index AND ic_coy_id = '" & strDefIPPCompID & "' " &
                           "LEFT OUTER JOIN bank_code ON ic_bank_code=bc_bank_code AND bc_coy_id = '" & strDefIPPCompID & "' " &
                           "INNER JOIN invoice_Details ON id_s_coy_id=im_s_coy_id AND id_invoice_no=im_invoice_no " &
                           "INNER JOIN user_mstr ON IM_STATUS_CHANGED_BY=UM_USER_ID AND IM_B_COY_ID=UM_COY_ID AND UM_DELETED='N' " &
                           "INNER JOIN users_usrgrp ON UU_USER_ID=UM_USER_ID AND UU_COY_ID=UM_COY_ID " &
                           "INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID AND UGM_DELETED='N' " &
                           "WHERE ((IM_INVOICE_STATUS=13) " &
                           "OR (IM_INVOICE_STATUS=4 AND IM_PRINTED_IND IS NULL AND im_payment_term IN ('RENTAS','TT','NOSTRO'))) " &
                           "AND IM_INVOICE_TYPE IS NOT NULL AND UGM_FIXED_ROLE = '" & FixedRole.Finance_Manager.ToString.Replace("_", " ") & "' AND IM_B_COY_ID='" & Session("CompanyID") & "' "

                If ViewState("FM") <> "" Then
                    .CommandText = .CommandText & " AND IM_STATUS_CHANGED_BY='" & ViewState("FM") & "'"
                End If

                If rdbtnVendor.Checked Then
                    .CommandText = .CommandText & " AND ic_coy_type='V' "
                End If

                If rdbtnEmployee.Checked Then
                    .CommandText = .CommandText & " AND ic_coy_type='E' "
                End If

                .CommandText = .CommandText & " GROUP BY im_invoice_no, im_s_coy_id "
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
            strFileName = "IPPD0005 - SummaryInvoicesReleasedReport" & "(" & Format(Now, "ddMMMyyyy") & ").xls"
            Dim attachment As String = "attachment;filename=" & strFileName
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"

            Dim dc As DataColumn
            Dim i As Integer

            i = 0
            For Each dc In ds.Tables(0).Columns
                If i > 0 Then
                    Response.Write(vbTab + dc.ColumnName)
                Else
                    Response.Write(dc.ColumnName)
                End If
                i += 1

            Next
            Response.Write(vbCrLf)

            Dim dr As DataRow
            For Each dr In ds.Tables(0).Rows
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    If i > 0 Then
                        Response.Write(vbTab + dr.Item(i).ToString)
                    Else
                        Response.Write(dr.Item(i).ToString)
                    End If
                Next
                Response.Write(vbCrLf)
            Next
            Response.End()

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try

    End Sub

    Private Sub ExportToPDF_InvReleased()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strUserId As String
        Dim strCoyName As String
        'Dim strDate As String = ""
        Dim strFileName As String = ""

        'strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            'CheeHong 9/Mar/2015 Case 8317
            Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "select im_invoice_no,im_s_coy_id,im_s_coy_name,ic_bank_acct,bc_bank_name,ic_bank_code,im_doc_date,im_due_date,im_prcs_sent,im_prcs_recv,im_exchange_rate," &
                            "im_created_by,im_currency_code, " &
                            "if(im_payment_term = 'tt',(im_invoice_total * im_exchange_rate),(im_invoice_total)) - if(im_payment_term = 'tt',(im_invoice_total * im_exchange_rate),(im_invoice_total)) * if(im_withholding_opt = 2,(ifnull(im_withholding_tax,0)/100),0) as im_invoice_total, " &
                            "im_invoice_status,im_invoice_type,im_status_changed_by,um_user_id,um_user_name,im_b_coy_id,im_payment_term," &
                            "im_payment_no,(select fa_active_ao from finance_approval where im_invoice_index=fa_invoice_index and fa_aga_type='fo' order by fa_seq desc limit 1) as foid, " &
                            "if(im_currency_code='myr','',im_currency_code) as 'foreigncurrency',if(im_currency_code='myr','',format(im_invoice_total,2)) as 'foreignamount'," &
                            "if(im_payment_term = 'tt',(0),(im_invoice_total)) - if(im_payment_term = 'tt',(0),(im_invoice_total)) * " &
                            "if(im_withholding_opt = 2,(ifnull(im_withholding_tax,0)/100),0) as im_invoice_total_no_tt, "

                '"'myr' as 'localcurrency',if(im_exchange_rate is not null,if(im_payment_term = 'tt',(im_invoice_total * im_exchange_rate),(im_invoice_total)) - if(im_payment_term = 'tt',(im_invoice_total * im_exchange_rate),(im_invoice_total)) * if(im_withholding_opt = 2,(ifnull(im_withholding_tax,0)/100),0),im_invoice_total) as 'localamount',  " & _
                .CommandText = .CommandText & "REPLACE(FORMAT(IF(im_exchange_rate IS NOT NULL,IF(im_payment_term = 'tt',(SUM(id_received_qty*id_unit_cost) * im_exchange_rate)," &
                                "(SUM(id_received_qty*id_unit_cost))) - IF(im_payment_term = 'tt', (SUM(id_received_qty*id_unit_cost) * im_exchange_rate), " &
                                "(SUM(id_received_qty*id_unit_cost))) * IF(im_withholding_opt = 2,(IFNULL(im_withholding_tax,0)/100),0), " &
                                "SUM(id_received_qty*id_unit_cost)),2),',','') AS 'localamount', " &
                                "FORMAT(IF(im_invoice_type<>'CN',IF(im_exchange_rate IS NOT NULL, IF(TRIM(id_gst_input_tax_code) <> 'IM5', SUM(IFNULL(ID_GST_VALUE,0) * im_exchange_rate), SUM(IFNULL(ID_GST_VALUE,0))), SUM(IFNULL(ID_GST_VALUE,0))),0),2) AS 'gstamount', " &
                                "FORMAT(IF(im_invoice_type='CN',IF(im_exchange_rate IS NOT NULL,SUM(IFNULL(ID_GST_VALUE,0) * im_exchange_rate),SUM(IFNULL(ID_GST_VALUE,0))),0),2) AS 'gstamount_cn', " &
                            "FORMAT(IF(im_invoice_type<>'CN',IF(im_exchange_rate IS NOT NULL,IF(im_payment_term = 'tt',(im_invoice_total * im_exchange_rate),(im_invoice_total)) - IF(im_payment_term = 'tt', " &
                            "(im_invoice_total * im_exchange_rate),(im_invoice_total)) * IF(im_withholding_opt = 2,(IFNULL(im_withholding_tax,0)/100),0), im_invoice_total),0),2) AS 'totalinvamount', " &
                            "round(if(im_exchange_rate is not null,if(im_invoice_type<>'cn',if(im_payment_term = 'tt',(im_invoice_total * im_exchange_rate),(im_invoice_total)) - if(im_payment_term = 'tt',(im_invoice_total * im_exchange_rate),(im_invoice_total)) * if(im_withholding_opt = 2,(ifnull(im_withholding_tax,0)/100),0),0),if(im_invoice_type<>'cn',im_invoice_total,0)),2) as im_invoice_no_cn_total, " &
                            "FORMAT(ROUND(IF(im_exchange_rate IS NOT NULL,IF(im_invoice_type='cn',im_invoice_total*im_exchange_rate,0),IF(im_invoice_type='cn',im_invoice_total,0)),2),2) AS im_cn_total, " &
                            "if(im_payment_term = 'ibg' and im_invoice_type<>'cn',im_invoice_total,0) as im_invoice_total_ibg, " &
                            "if(im_payment_term = 'casa' and im_invoice_type<>'cn',im_invoice_total,0) as im_invoice_total_casa, " &
                            "if(im_payment_term = 'rentas' and im_invoice_type<>'cn',im_invoice_total,0) as im_invoice_total_rentas, " &
                            "if(im_payment_term = 'bc' and im_invoice_type<>'cn',im_invoice_total,0) as im_invoice_total_bc, " &
                            "if(im_payment_term = 'ibg' and im_invoice_type='cn',im_invoice_total,0) as im_invoice_total_cn_ibg, " &
                            "if(im_payment_term = 'casa' and im_invoice_type='cn',im_invoice_total,0) as im_invoice_total_cn_casa, " &
                            "if(im_payment_term = 'rentas' and im_invoice_type='cn',im_invoice_total,0) as im_invoice_total_cn_rentas, " &
                            "if(im_payment_term = 'bc' and im_invoice_type='cn',im_invoice_total,0) as im_invoice_total_cn_bc, " &
                            "format(if(im_payment_term = 'tt',(im_invoice_total * im_exchange_rate),(im_invoice_total)), 2) as im_invoice_total_convert, " &
                            "format(if((im_payment_term = 'tt' and im_withholding_opt = 2),(im_invoice_total * im_exchange_rate) * (im_withholding_tax/100),0),2) as im_withholding_tax_amt,im_withholding_tax,  " &
                            "ROUND(IF(im_exchange_rate IS NOT NULL,IF(im_invoice_type<>'cn',IF(im_payment_term = 'tt',(SUM(ID_RECEIVED_QTY*ID_UNIT_COST) * im_exchange_rate), " &
                            "(SUM(ID_RECEIVED_QTY*ID_UNIT_COST))) - IF(im_payment_term = 'tt',(SUM(ID_RECEIVED_QTY*ID_UNIT_COST) * im_exchange_rate),(SUM(ID_RECEIVED_QTY*ID_UNIT_COST))) * IF(im_withholding_opt = 2, " &
                            "(IFNULL(im_withholding_tax,0)/100),0),0),IF(im_invoice_type<>'cn',SUM(ID_RECEIVED_QTY*ID_UNIT_COST),0)),2) AS im_invoice_no_cn_total_no_gst, " &
                            "ROUND(IF(im_exchange_rate IS NOT NULL,IF(im_invoice_type='cn',SUM(ID_RECEIVED_QTY*ID_UNIT_COST)*im_exchange_rate,0),IF(im_invoice_type='cn',SUM(ID_RECEIVED_QTY*ID_UNIT_COST),0)),2) AS im_cn_total_no_gst, " &
                            "case when im_withholding_opt = 1 then 'company' " &
                            "when im_withholding_opt = 2 then 'vendor'  " &
                            "when im_withholding_opt = 3 then 'no wht' else '' end as im_withholding_opt " &
                            "from invoice_mstr " &
                            "left outer join ipp_company on im_s_coy_id=ic_index and ic_coy_id = '" & strDefIPPCompID & "' " &
                            "left outer join bank_code on ic_bank_code=bc_bank_code and bc_coy_id = '" & strDefIPPCompID & "' " &
                            "INNER JOIN invoice_Details ON id_s_coy_id=im_s_coy_id AND id_invoice_no=im_invoice_no " &
                            "inner join user_mstr on im_status_changed_by=um_user_id and im_b_coy_id=um_coy_id and um_deleted='n' " &
                            "inner join users_usrgrp on uu_user_id=um_user_id and uu_coy_id=um_coy_id " &
                            "inner join user_group_mstr on uu_usrgrp_id = ugm_usrgrp_id and ugm_deleted='n' " &
                            "where ((im_invoice_status=13) " &
                            "or (im_invoice_status=4 and im_printed_ind is null and im_payment_term in ('rentas','tt'))) " &
                            "AND IM_INVOICE_TYPE IS NOT NULL AND UGM_FIXED_ROLE = '" & FixedRole.Finance_Manager.ToString.Replace("_", " ") & "' AND IM_B_COY_ID='" & Session("CompanyID") & "' " &
                            "AND UGM_USRGRP_NAME LIKE 'IPP%' "
                If ViewState("FM") <> "" Then
                    .CommandText = .CommandText & " AND IM_STATUS_CHANGED_BY='" & ViewState("FM") & "'"
                End If

                If rdbtnVendor.Checked Then
                    .CommandText = .CommandText & " AND ic_coy_type='V' "
                End If

                If rdbtnEmployee.Checked Then
                    .CommandText = .CommandText & " AND ic_coy_type='E' "
                End If

                .CommandText = .CommandText & "GROUP BY im_invoice_no, im_s_coy_id  "
            End With

            da = New MySqlDataAdapter(cmd)
            strUserId = Session("UserName")
            strCoyName = Session("CompanyName")

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("InvReleased_dtInvReleased", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = dispatcher.direct("Report", "InvReleased.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")            

            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            If TotalParameter > 0 Then
                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
                For I = 0 To localreport.GetParameters.Count - 1
                    GetParameter = localreport.GetParameters.Item(I).Name
                    Select Case LCase(GetParameter)
                        Case "prmrequestedby"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

                        Case "logo"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                        Case "prmtitle"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, Format(Now, "dd/MM/yyyy"))

                        Case "prmbuyercoyname"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)

                        Case Else
                    End Select
                Next
                localreport.SetParameters(par)
            End If

            localreport.Refresh()

            Dim deviceInfo As String =
                    "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            strFileName = "IPPD0005 - SummaryInvoicesReleasedReport.pdf"
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()


        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub

    Private Sub ExportToExcel_ActiveVendor()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        'Dim strDate As String = ""
        Dim strFileName As String = ""

        'strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            'Zulham Case 8317 13022015
            Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                'Zulham Case 8317 13022015
                .CommandText = "SELECT ic_coy_name AS 'Name of Vendor / Employee / Billing to RC', " &
                               "ic_addr_line1 AS 'Address' , " &
                               "IF(C.CODE_DESC ='' OR ISNULL(C.CODE_DESC), '-', C.CODE_DESC)  AS 'Category', " &
                               "IF(ic_resident_type='Y', 'Resident', 'Non-Resident')  AS 'Resident Type', " &
                               "IF(ic_resident_type='Y', 'Malaysia', D.CODE_DESC)  AS 'Resident Country', " &
                               "IF(ic_business_reg_no='' OR ISNULL(ic_business_reg_no), '-', ic_business_reg_no) AS 'Business', " &
                               "IF(ic_payment_method='' OR ISNULL(ic_payment_method), '-', ic_payment_method) AS 'Payment', " &
                               "IF(ic_bank_code='' OR ISNULL(ic_bank_code), '-', ic_bank_code) AS 'Bank', " &
                               "IF(bc_bank_name='' OR ISNULL(bc_bank_name), '-', bc_bank_name) AS 'Bank Name', " &
                               "IF(ic_bank_acct='' OR ISNULL(ic_bank_acct), '-', CONCAT('=""', ic_bank_acct, '""')) AS 'Bank Account', " &
                               "IF(ic_mod_by='' OR ISNULL(ic_mod_by), '', ic_mod_by) AS 'UserID', " &
                               "DATE_FORMAT((IF(ic_mod_datetime='' OR ISNULL(ic_mod_datetime), '', ic_mod_datetime)),'%d/%m/%Y : %H:%i') AS 'Datetime' " &
                               "FROM(ipp_company) " &
                               "LEFT OUTER JOIN bank_code b ON bc_bank_code = ic_bank_code and bc_coy_id = '" & strDefIPPCompID & "' " &
                               "LEFT JOIN CODE_MSTR C ON C.CODE_CATEGORY = 'IPPCC' AND C.CODE_ABBR = ic_company_category " &
                               "LEFT JOIN CODE_MSTR D ON D.CODE_CATEGORY = 'CT' AND D.CODE_ABBR = ic_resident_country " &
                               "WHERE (ic_coy_id = '" & strDefIPPCompID & "') AND (ic_status='A') " &
                               "GROUP BY ic_coy_type, ic_coy_name, ic_addr_line1, ic_other_b_coy_code, ic_business_reg_no, " &
                               "ic_payment_method, ic_bank_code, bc_bank_name, ic_bank_acct, ic_mod_by, ic_mod_datetime " &
                               "ORDER BY FIELD(ic_coy_type, 'E', 'B', 'V'), ic_coy_name"

            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
            strFileName = "IPPA0012 - List of Active Vendors Report" & "(" & Format(Now, "ddMMMyyyy") & ").xls"
            Dim attachment As String = "attachment;filename=" & strFileName
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"

            Dim dc As DataColumn
            Dim i As Integer

            i = 0
            For Each dc In ds.Tables(0).Columns
                If i > 0 Then
                    Response.Write(vbTab + dc.ColumnName)
                Else
                    Response.Write(dc.ColumnName)
                End If
                i += 1

            Next
            Response.Write(vbCrLf)

            Dim dr As DataRow
            For Each dr In ds.Tables(0).Rows
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    If i > 0 Then
                        Response.Write(vbTab + dr.Item(i).ToString)
                    Else
                        Response.Write(dr.Item(i).ToString)
                    End If
                Next
                Response.Write(vbCrLf)
            Next
            Response.End()

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try

    End Sub

    Private Sub ExportToPDF_ActiveVendor()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strUserId As String
        Dim strCoyName As String
        Dim strDate As String = ""
        Dim strFileName As String = ""

        'strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            'Zulham Case 8317 13022015
            Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If


            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                'Zulham Case 8317 13022015
                .CommandText = "SELECT 	ic_coy_name, ic_other_b_coy_code, ic_status, ic_business_reg_no, " &
                               "ic_bank_acct, ic_payment_method, ic_bank_code, ic_addr_line1, " &
                               "ic_ent_by, ic_ent_datetime, ic_mod_by, ic_mod_datetime, bc_bank_name, " &
                               "ic_coy_id, ic_coy_type, b.code_desc as category, c.code_desc as resident_country, ic_resident_type " &
                               "FROM(ipp_company) " &
                               "LEFT OUTER JOIN bank_code ON bc_bank_code = ic_bank_code and bc_coy_id = '" & strDefIPPCompID & "' " &
                               "LEFT JOIN CODE_MSTR B ON B.CODE_CATEGORY = 'IPPCC' AND B.CODE_ABBR = ic_company_category " &
                               "LEFT JOIN CODE_MSTR C ON C.CODE_CATEGORY = 'CT' AND C.CODE_ABBR = ic_resident_country " &
                               "WHERE (ic_coy_id = '" & strDefIPPCompID & "') AND (ic_status='A') " &
                               "ORDER BY FIELD(ic_coy_type, 'E', 'B', 'V'), ic_coy_name"

            End With

            da = New MySqlDataAdapter(cmd)


            ' da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyID")))
            strUserId = Session("UserName") 'Session("UserId")
            strCoyName = Session("CompanyName")

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("VendorListing_VendorListing", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)

            localreport.ReportPath = dispatcher.direct("Report", "ActiveVendor.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")

            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            If TotalParameter > 0 Then
                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
                For I = 0 To localreport.GetParameters.Count - 1
                    GetParameter = localreport.GetParameters.Item(I).Name
                    Select Case LCase(GetParameter)
                        Case "prmrequestedby"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

                        Case "logo"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                        Case "prmdate"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, Format(Now, "dd/MM/yyyy"))

                        Case "prmbuyercoyname"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)

                        Case Else
                    End Select
                Next
                localreport.SetParameters(par)
            End If

            localreport.Refresh()

            Dim deviceInfo As String =
                "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            strFileName = "IPPA0012 - List of Active Vendors Report.pdf"
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()


        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub

    Private Sub ExportToExcel_InactiveVendor()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        'Dim strDate As String = ""
        Dim strFileName As String = ""

        'strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            'Zulham Case 8317 13022015
            Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                'Zulham Case 8317 13022015
                .CommandText = "SELECT ic_coy_name AS 'Name of Vendor / Employee / Billing to RC', " &
                               "ic_addr_line1 AS 'Address' , " &
                               "IF(B.CODE_DESC ='' OR ISNULL(B.CODE_DESC), '-', B.CODE_DESC)  AS 'Category', " &
                               "IF(ic_resident_type='Y', 'Resident', 'Non-Resident')  AS 'Resident Type', " &
                               "IF(ic_resident_type='Y', 'Malaysia', C.CODE_DESC)  AS 'Resident Country', " &
                               "IF(ic_business_reg_no='' OR ISNULL(ic_business_reg_no), '-', ic_business_reg_no) AS 'Business', " &
                               "IF(ic_payment_method='' OR ISNULL(ic_payment_method), '-', ic_payment_method) AS 'Payment', " &
                               "IF(ic_bank_code='' OR ISNULL(ic_bank_code), '-', ic_bank_code) AS 'Bank', " &
                               "IF(bc_bank_name='' OR ISNULL(bc_bank_name), '-', bc_bank_name) AS 'Bank Name', " &
                               "IF(ic_bank_acct='' OR ISNULL(ic_bank_acct), '-', CONCAT('=""', ic_bank_acct, '""')) AS 'Bank Account', " &
                               "IF(ic_mod_by='' OR ISNULL(ic_mod_by), '', ic_mod_by) AS 'UserID', " &
                               "DATE_FORMAT((IF(ic_mod_datetime='' OR ISNULL(ic_mod_datetime), '', ic_mod_datetime)),'%d/%m/%Y : %H:%i') AS 'Datetime', ic_remark as 'Deactivation Reason' " &
                               "FROM(ipp_company) " &
                               "LEFT OUTER JOIN bank_code ON bc_bank_code = ic_bank_code and bc_coy_id = '" & strDefIPPCompID & "' " &
                               "LEFT JOIN CODE_MSTR B ON B.CODE_CATEGORY = 'IPPCC' AND B.CODE_ABBR = ic_company_category " &
                               "LEFT JOIN CODE_MSTR C ON C.CODE_CATEGORY = 'CT' AND C.CODE_ABBR = ic_resident_country " &
                               "WHERE (ic_coy_id = '" & strDefIPPCompID & "') AND (ic_status='I') " &
                               "GROUP BY ic_coy_type, ic_coy_name, ic_addr_line1, ic_other_b_coy_code, ic_business_reg_no, " &
                               "ic_payment_method, ic_bank_code, bc_bank_name, ic_bank_acct, ic_mod_by, ic_mod_datetime " &
                               "ORDER BY FIELD(ic_coy_type, 'E', 'B', 'V'), ic_coy_name"

            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
            strFileName = "IPPA0013 - List of Inactive Vendors Report" & "(" & Format(Now, "ddMMMyyyy") & ").xls"
            Dim attachment As String = "attachment;filename=" & strFileName
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"

            Dim dc As DataColumn
            Dim i As Integer

            i = 0
            For Each dc In ds.Tables(0).Columns
                If i > 0 Then
                    Response.Write(vbTab + dc.ColumnName)
                Else
                    Response.Write(dc.ColumnName)
                End If
                i += 1

            Next
            Response.Write(vbCrLf)

            Dim dr As DataRow
            For Each dr In ds.Tables(0).Rows
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    If i > 0 Then
                        Response.Write(vbTab + dr.Item(i).ToString)
                    Else
                        Response.Write(dr.Item(i).ToString)
                    End If
                Next
                Response.Write(vbCrLf)
            Next
            Response.End()

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try

    End Sub

    Private Sub ExportToPDF_InactiveVendor()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strUserId As String
        Dim strCoyName As String
        Dim strDate As String = ""
        Dim strFileName As String = ""

        'strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            'Zulham Case 8317 13022015
            Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If


            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                'Zulham Case 8317 13022015
                .CommandText = "SELECT 	ic_coy_name, ic_other_b_coy_code, ic_status, ic_business_reg_no, " &
                               "ic_bank_acct, ic_payment_method, ic_bank_code, ic_addr_line1, " &
                               "ic_ent_by, ic_ent_datetime, ic_mod_by, ic_mod_datetime, bc_bank_name, " &
                               "ic_coy_id, ic_coy_type, ic_remark, b.code_desc as category, c.code_desc as resident_country, ic_resident_type " &
                               "FROM ipp_company a " &
                               "LEFT OUTER JOIN bank_code ON bc_bank_code = ic_bank_code and bc_coy_id = '" & strDefIPPCompID & "' " &
                               "LEFT JOIN CODE_MSTR B ON B.CODE_CATEGORY = 'IPPCC' AND B.CODE_ABBR = ic_company_category " &
                               "LEFT JOIN CODE_MSTR C ON C.CODE_CATEGORY = 'CT' AND C.CODE_ABBR = ic_resident_country " &
                               "WHERE (ic_coy_id = '" & strDefIPPCompID & "') AND (ic_status='I') " &
                               "ORDER BY FIELD(ic_coy_type, 'E', 'B', 'V'), ic_coy_name"

            End With

            da = New MySqlDataAdapter(cmd)


            ' da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyID")))
            strUserId = Session("UserName") 'Session("UserId")
            strCoyName = Session("CompanyName")

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("VendorListing_VendorListing", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)

            localreport.ReportPath = dispatcher.direct("Report", "InactiveVendor.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")

            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            If TotalParameter > 0 Then
                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
                For I = 0 To localreport.GetParameters.Count - 1
                    GetParameter = localreport.GetParameters.Item(I).Name
                    Select Case LCase(GetParameter)
                        Case "prmrequestedby"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

                        Case "logo"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                        Case "prmdate"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, Format(Now, "dd/MM/yyyy"))

                        Case "prmbuyercoyname"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)

                        Case Else
                    End Select
                Next
                localreport.SetParameters(par)
            End If

            localreport.Refresh()

            Dim deviceInfo As String =
                "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            strFileName = "IPPA0013 - List of Inactive Vendors Report.pdf"
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()


        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub

    Private Sub ExportToExcel_StaffClaim()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim strDate1 As String = ""
        Dim strDate2 As String = ""
        Dim strFileName As String = ""
        Dim strStatus As String = ""

        strDate1 = Format(CDate(txtSDate.Text), "yyyy-MM-dd")
        strDate2 = Format(CDate(txtEDate.Text), "yyyy-MM-dd")

        If (CheckBox1.Checked = True And CheckBox2.Checked = True) Or (CheckBox1.Checked = False And CheckBox2.Checked = False) Then
            strStatus = "A', 'I"
        ElseIf (CheckBox1.Checked = True And CheckBox2.Checked = False) Then
            strStatus = "A"
        ElseIf (CheckBox1.Checked = False And CheckBox2.Checked = True) Then
            strStatus = "I"
        End If

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            'Zulham Case 8317 13022015
            Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                'Jules 2014.09.29 commented
                'If txtCentreCode.Text = "" Then
                '    .CommandText = "SELECT IM_S_COY_NAME, IM_INVOICE_NO, IM_PRCS_SENT, IM_PRCS_RECV, " & _
                '             "ID_RECEIVED_QTY, ID_UNIT_COST, IM_EXCHANGE_RATE, " & _
                '             "ID_RECEIVED_QTY * ID_UNIT_COST AS 'AMOUNT', " & _
                '             "IM_PAYMENT_DATE, ID_BRANCH_CODE, ID_COST_CENTER, IM_CREATED_BY, " & _
                '             "ic_status, ID_B_GL_CODE, CBG_B_GL_DESC, IM_CURRENCY_CODE, ic_business_reg_no " & _
                '             "FROM(invoice_mstr) " & _
                '             "INNER JOIN ipp_company ON IM_S_COY_ID = ic_index " & _
                '             "INNER JOIN invoice_details ON IM_INVOICE_NO = ID_INVOICE_NO " & _
                '             "INNER JOIN company_b_gl_code ON ID_B_GL_CODE = CBG_B_GL_CODE " & _
                '             "WHERE	(ic_coy_id = '" & Session("CompanyID") & "') AND (ic_coy_type = 'E') AND " & _
                '             "(CAST(IM_PAYMENT_DATE AS DATE) BETWEEN '" & strDate1 & "' AND '" & strDate2 & "' ) " & _
                '             "AND ic_status IN ('" & strStatus & "') AND ID_BRANCH_CODE = '" & txtBranchCode.Text & "' " & _
                '             "ORDER BY ID_B_GL_CODE, IM_S_COY_NAME "
                'Else
                '    .CommandText = "SELECT IM_S_COY_NAME, IM_INVOICE_NO, IM_PRCS_SENT, IM_PRCS_RECV, " & _
                '             "ID_RECEIVED_QTY, ID_UNIT_COST, IM_EXCHANGE_RATE, " & _
                '             "ID_RECEIVED_QTY * ID_UNIT_COST AS 'AMOUNT', " & _
                '             "IM_PAYMENT_DATE, ID_BRANCH_CODE, ID_COST_CENTER, IM_CREATED_BY, " & _
                '             "ic_status, ID_B_GL_CODE, CBG_B_GL_DESC, IM_CURRENCY_CODE, ic_business_reg_no " & _
                '             "FROM(invoice_mstr) " & _
                '             "INNER JOIN ipp_company ON IM_S_COY_ID = ic_index " & _
                '             "INNER JOIN invoice_details ON IM_INVOICE_NO = ID_INVOICE_NO " & _
                '             "INNER JOIN company_b_gl_code ON ID_B_GL_CODE = CBG_B_GL_CODE " & _
                '             "WHERE	(ic_coy_id = '" & Session("CompanyID") & "') AND (ic_coy_type = 'E') AND " & _
                '             "(CAST(IM_PAYMENT_DATE AS DATE) BETWEEN '" & strDate1 & "' AND '" & strDate2 & "' ) " & _
                '             "AND ic_status IN ('" & strStatus & "') AND ID_BRANCH_CODE = '" & txtBranchCode.Text & "' AND ID_COST_CENTER = '" & txtCentreCode.Text & "' " & _
                '             "ORDER BY ID_B_GL_CODE, IM_S_COY_NAME "
                'End If
                'Jules -end of commented section
                '.CommandText = "SELECT ic_business_reg_no AS 'Staff ID', " & _
                '               "IM_S_COY_NAME AS 'Staff Name', " & _
                '               "FORMAT(ID_RECEIVED_QTY * ID_UNIT_COST, 2) AS 'Amount', " & _
                '               "IM_INVOICE_NO AS 'Invoice No', " & _
                '               "DATE_FORMAT(IM_PRCS_SENT,'%d-%M-%Y') AS 'PSD Send Date', " & _
                '               "DATE_FORMAT(IM_PRCS_RECV,'%d-%M-%Y') AS 'PSD Received', " & _
                '               "DATE_FORMAT(IM_PAYMENT_DATE,'%d-%M-%Y') AS 'Payment Date', " & _
                '               "ID_BRANCH_CODE AS 'Branch Code', " & _
                '               "ID_COST_CENTER AS 'Cost Center Code', " & _
                '               "IM_CREATED_BY AS 'Teller ID' " & _
                '               "FROM(invoice_mstr) " & _
                '               "INNER JOIN ipp_company  	ON IM_S_COY_ID = ic_index " & _
                '               "INNER JOIN invoice_details  	ON IM_INVOICE_NO = ID_INVOICE_NO " & _
                '               "INNER JOIN company_b_gl_code  	ON ID_B_GL_CODE = CBG_B_GL_CODE " & _
                '               "WHERE	(ic_coy_id = 'hlb') AND (ic_coy_type = 'E') AND " & _
                '               "(CAST(IM_PAYMENT_DATE AS DATE) BETWEEN '" & strDate1 & "' AND '" & strDate2 & "' ) " & _
                '               "AND ic_status IN ('" & strStatus & "') AND ID_BRANCH_CODE = '" & txtBranchCode.Text & "' AND ID_COST_CENTER = '" & txtCentreCode.Text & "' " & _
                '               "GROUP BY ID_B_GL_CODE " & _
                '               "ORDER BY ID_B_GL_CODE, IM_S_COY_NAME "

                'Zulham Case 8317 13022015
                '.CommandText = "SELECT IC_BUSINESS_REG_NO, IM_S_COY_NAME, IM_INVOICE_NO,  IFNULL(IC_ADDITIONAL_1,'') AS 'JOB GRADE', ID_B_GL_CODE, CBG_B_GL_DESC, ID_BRANCH_CODE, ID_COST_CENTER, " & _
                '            "IM_CURRENCY_CODE, ID_RECEIVED_QTY, ID_UNIT_COST, IM_EXCHANGE_RATE, ID_RECEIVED_QTY * ID_UNIT_COST AS 'AMOUNT', IM_PAYMENT_DATE, " & _
                '            "IM_PAYMENT_NO, IM_CREATED_BY, IC_STATUS, id_product_desc 'Transaction Description', id_gst_input_tax_code 'GST Tax Code', id_gst_value 'GST Amount' " & _
                '           "FROM invoice_mstr " & _
                '           "INNER JOIN ipp_company ON IM_S_COY_ID = ic_index " & _
                '           "INNER JOIN invoice_details ON IM_INVOICE_NO = ID_INVOICE_NO " & _
                '           "INNER JOIN company_b_gl_code ON ID_B_GL_CODE = CBG_B_GL_CODE AND CBG_B_COY_ID = '" & strDefIPPCompID & "' " & _
                '           "WHERE	(ic_coy_id = '" & strDefIPPCompID & "') AND (ic_coy_type = 'E') AND " & _
                '           "(CAST(IM_PAYMENT_DATE AS DATE) BETWEEN '" & strDate1 & "' AND '" & strDate2 & "' ) " & _
                '           "AND ic_status IN ('" & strStatus & "') "

                'IPP Stage 2A - CH - 6 Mar 2015
                .CommandText = "SELECT IC_BUSINESS_REG_NO, IM_S_COY_NAME, IM_INVOICE_NO,  IFNULL(IC_ADDITIONAL_1,'') AS 'JOB GRADE', ID_B_GL_CODE, CBG_B_GL_DESC, ID_BRANCH_CODE, ID_COST_CENTER, " &
                            "IM_CURRENCY_CODE, ID_RECEIVED_QTY, ID_UNIT_COST, IM_EXCHANGE_RATE, ID_RECEIVED_QTY * ID_UNIT_COST AS 'AMOUNT', IM_PAYMENT_DATE, " &
                            "IM_PAYMENT_NO, IM_CREATED_BY, IC_STATUS, id_product_desc 'Transaction Description', id_gst_input_tax_code 'GST Tax Code', id_gst_value 'GST Amount' " &
                           "FROM INVOICE_MSTR " &
                           "INNER JOIN IPP_COMPANY ON IM_S_COY_ID = IC_INDEX AND IC_COY_ID = '" & strDefIPPCompID & "' AND IC_COY_TYPE = 'E' " &
                           "INNER JOIN invoice_details ON IM_INVOICE_NO = ID_INVOICE_NO and im_s_coy_id = id_s_coy_id " &
                           "INNER JOIN company_b_gl_code ON ID_B_GL_CODE = CBG_B_GL_CODE AND CBG_B_COY_ID = '" & strDefIPPCompID & "' " &
                           "WHERE IM_B_COY_ID = '" & Session("CompanyId") & "' AND (CAST(IM_PAYMENT_DATE AS DATE) BETWEEN '" & strDate1 & "' AND '" & strDate2 & "' ) " &
                           "AND ic_status IN ('" & strStatus & "') "

                If txtBranchCode.Text <> "" Then
                    .CommandText = .CommandText & " AND ID_BRANCH_CODE = '" & txtBranchCode.Text & "' "
                End If

                If txtCentreCode.Text <> "" Then
                    .CommandText = .CommandText & " AND ID_COST_CENTER = '" & txtCentreCode.Text & "' "
                End If

                If txtFromGLCode.Text <> "" And txtToGLCode.Text <> "" Then
                    .CommandText = .CommandText & " AND ID_B_GL_CODE BETWEEN '" & txtFromGLCode.Text & "' AND '" & txtToGLCode.Text & "' "
                End If

                .CommandText = .CommandText & " ORDER BY ID_B_GL_CODE, IM_S_COY_NAME "
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
            strFileName = "IPPA0014 - Staff Claim by GL Account Code Report" & "(" & Format(Now, "ddMMMyyyy") & ").xls"
            Dim attachment As String = "attachment;filename=" & strFileName
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"

            Dim dc As DataColumn
            Dim i As Integer

            i = 0
            For Each dc In ds.Tables(0).Columns
                If i > 0 Then
                    Response.Write(vbTab + dc.ColumnName)
                Else
                    Response.Write(dc.ColumnName)
                End If
                i += 1

            Next
            Response.Write(vbCrLf)

            Dim dr As DataRow
            For Each dr In ds.Tables(0).Rows
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    If i > 0 Then
                        Response.Write(vbTab + dr.Item(i).ToString)
                    Else
                        Response.Write(dr.Item(i).ToString)
                    End If
                Next
                Response.Write(vbCrLf)
            Next
            Response.End()

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try

    End Sub

    Private Sub ExportToExcel_PaidStaffClaim()
        Dim ds, ds2, ds3 As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim strDate1 As String = ""
        Dim strDate2 As String = ""
        Dim intStartMonth, intEndMonth As Integer
        Dim lgStartYear, lgEndYear As Long
        Dim strFileName As String = ""
        Dim strStatus As String = ""

        strDate1 = Format(CDate(txtStartDate.Text), "yyyy-MM-dd")
        strDate2 = Format(CDate(txtEndDate.Text), "yyyy-MM-dd")

        'IPP Stage 2A - CH - 6 Mar 2015
        Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
        If strDefIPPCompID = "" Then
            strDefIPPCompID = HttpContext.Current.Session("CompanyID")
        End If

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text

                'Get Claims list
                '.CommandText = "SELECT DISTINCT CBG_B_GL_CODE,CBG_B_GL_DESC " & _
                '                "FROM ipp_company ic " & _
                '                "INNER JOIN INVOICE_MSTR im ON ic.ic_index=im.im_s_coy_id AND ic.ic_coy_id=im.im_b_coy_id " & _
                '                "INNER JOIN INVOICE_DETAILS AS id ON im.im_invoice_no = id.id_invoice_no AND im.im_s_coy_id = id.id_s_coy_id " & _
                '                "INNER JOIN company_b_gl_code cbg ON cbg.cbg_b_gl_code = id.id_b_gl_code AND cbg.cbg_b_coy_id='hlb' " & _
                '                "WHERE ic.ic_coy_type = 'E' AND ic.ic_coy_id = 'hlb' " & _
                '                "AND im.IM_INVOICE_STATUS = 4 AND im.IM_INVOICE_TYPE IS NOT NULL " & _
                '                "AND CAST(im.im_fm_approved_date AS DATE) BETWEEN '" & strDate1 & "' AND '" & strDate2 & "' " & _
                '                "ORDER BY CBG_B_GL_CODE "
                'IPP Stage 2A - CH - 6 Mar 2015
                .CommandText = "SELECT DISTINCT CBG_B_GL_CODE,CBG_B_GL_DESC " &
                            "FROM INVOICE_MSTR " &
                            "INNER JOIN INVOICE_DETAILS ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " &
                            "INNER JOIN IPP_COMPANY ON IC_INDEX = IM_S_COY_ID AND ic_coy_type = 'E' AND ic_coy_id = '" & strDefIPPCompID & "' " &
                            "INNER JOIN company_b_gl_code ON cbg_b_gl_code = id_b_gl_code AND cbg_b_coy_id = '" & strDefIPPCompID & "' " &
                            "WHERE IM_B_COY_ID = '" & Session("CompanyId") & "' " &
                            "AND IM_INVOICE_STATUS = 4 AND IM_INVOICE_TYPE IS NOT NULL " &
                            "AND CAST(im_fm_approved_date AS DATE) BETWEEN '" & strDate1 & "' AND '" & strDate2 & "' " &
                            "ORDER BY CBG_B_GL_CODE "
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)

            'Get Staff list
            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                '.CommandText = "SELECT ic.ic_index AS 'Staff ID', ic.ic_coy_name AS 'Staff Name', ic.IC_ADDITIONAL_1 AS 'Job Grade', " & _
                '                "FORMAT(SUM(id.id_received_qty * id.id_unit_cost + IFNULL(id.id_gst_value,0)),2) AS 'Total Amount' " & _
                '                "FROM ipp_company ic " & _
                '                "INNER JOIN INVOICE_MSTR im ON ic.ic_index=im.im_s_coy_id AND ic.ic_coy_id=im.im_b_coy_id " & _
                '                "INNER JOIN INVOICE_DETAILS AS id ON im.im_invoice_no = id.id_invoice_no AND im.im_s_coy_id = id.id_s_coy_id " & _
                '                "INNER JOIN company_b_gl_code cbg ON cbg.cbg_b_gl_code = id.id_b_gl_code AND cbg.cbg_b_coy_id='hlb' " & _
                '                "WHERE ic.ic_coy_type = 'E' AND ic.ic_coy_id = 'hlb' " & _
                '                "AND im.IM_INVOICE_STATUS = 4 AND im.IM_INVOICE_TYPE IS NOT NULL " & _
                '                "AND CAST(im.im_fm_approved_date AS DATE) BETWEEN '" & strDate1 & "' AND '" & strDate2 & "' " & _
                '                "GROUP BY ic.ic_index " & _
                '                "ORDER BY ic.ic_index "
                'IPP Stage 2A - CH - 6 Mar 2015
                .CommandText = "SELECT ic.ic_index AS 'Staff ID', ic.ic_coy_name AS 'Staff Name', ic.IC_ADDITIONAL_1 AS 'Job Grade', " &
                                "FORMAT(SUM(id.id_received_qty * id.id_unit_cost + IFNULL(id.id_gst_value,0)),2) AS 'Total Amount' " &
                                "FROM INVOICE_MSTR IM " &
                                "INNER JOIN INVOICE_DETAILS ID ON IM.IM_INVOICE_NO = ID.ID_INVOICE_NO AND IM.IM_S_COY_ID = ID.ID_S_COY_ID " &
                                "INNER JOIN ipp_company ic ON ic_index = im_s_coy_id AND ic.ic_coy_type = 'E' AND ic.ic_coy_id = '" & strDefIPPCompID & "' " &
                                "INNER JOIN company_b_gl_code cbg ON cbg.cbg_b_gl_code = id.id_b_gl_code AND cbg.cbg_b_coy_id='" & strDefIPPCompID & "' " &
                                "WHERE IM_B_COY_ID = '" & Session("CompanyId") & "' AND IM_INVOICE_STATUS = 4 AND im.IM_INVOICE_TYPE IS NOT NULL " &
                                "AND CAST(im.im_fm_approved_date AS DATE) BETWEEN '" & strDate1 & "' AND '" & strDate2 & "' " &
                                "GROUP BY ic.ic_index " &
                                "ORDER BY ic.ic_index "
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds2)

            'Get employee claim data
            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                '.CommandText = "SELECT ic.ic_index, FORMAT(SUM(id.id_received_qty * id.id_unit_cost + IFNULL(id.id_gst_value,0)),2) AS 'Amount', CBG_B_GL_CODE " & _
                '                "FROM ipp_company ic " & _
                '                "INNER JOIN INVOICE_MSTR im ON ic.ic_index=im.im_s_coy_id AND ic.ic_coy_id=im.im_b_coy_id " & _
                '                "INNER JOIN INVOICE_DETAILS AS id ON im.im_invoice_no = id.id_invoice_no AND im.im_s_coy_id = id.id_s_coy_id " & _
                '                "INNER JOIN company_b_gl_code cbg ON cbg.cbg_b_gl_code = id.id_b_gl_code AND cbg.cbg_b_coy_id='hlb' " & _
                '                "WHERE ic.ic_coy_type = 'E' AND ic.ic_coy_id = 'hlb' " & _
                '                "AND im.IM_INVOICE_STATUS = 4 AND im.IM_INVOICE_TYPE IS NOT NULL " & _
                '                "AND CAST(im.im_fm_approved_date AS DATE) BETWEEN '" & strDate1 & "' AND '" & strDate2 & "' " & _
                '                "GROUP BY ic.ic_index, CBG_B_GL_CODE "
                'IPP Stage 2A - CH - 6 Mar 2015
                .CommandText = "SELECT ic.ic_index, FORMAT(SUM(id.id_received_qty * id.id_unit_cost + IFNULL(id.id_gst_value,0)),2) AS 'Amount', CBG_B_GL_CODE " &
                            "FROM INVOICE_MSTR IM " &
                            "INNER JOIN INVOICE_DETAILS ID ON IM.IM_INVOICE_NO = ID.ID_INVOICE_NO AND IM.IM_S_COY_ID = ID.ID_S_COY_ID " &
                            "INNER JOIN IPP_COMPANY IC ON IC.IC_INDEX = IM.IM_S_COY_ID AND IC.IC_COY_ID = '" & strDefIPPCompID & "' AND IC.IC_COY_TYPE = 'E' " &
                            "INNER JOIN company_b_gl_code cbg ON cbg.cbg_b_gl_code = id.id_b_gl_code AND cbg.cbg_b_coy_id='" & strDefIPPCompID & "' " &
                            "WHERE IM.IM_B_COY_ID = '" & Session("CompanyId") & "' " &
                            "AND im.IM_INVOICE_STATUS = 4 AND im.IM_INVOICE_TYPE IS NOT NULL " &
                            "AND CAST(im.im_fm_approved_date AS DATE) BETWEEN '" & strDate1 & "' AND '" & strDate2 & "' " &
                            "GROUP BY ic.ic_index, CBG_B_GL_CODE "
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds3)



            strFileName = "IPPS0017 - Paid Staff Claim Report" & "(" & Format(Now, "ddMMMyyyy") & ").xls"
            Dim attachment As String = "attachment;filename=" & strFileName
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"

            Dim dc As DataColumn
            Dim i, j As Integer

            'Header
            Response.Write("No.")
            Response.Write(vbTab + "Staff ID")
            Response.Write(vbTab + "Staff Name")
            Response.Write(vbTab + "Job Grade")

            Dim datarow As DataRow
            For Each datarow In ds.Tables(0).Rows
                Response.Write(vbTab + datarow.Item("CBG_B_GL_CODE").ToString + " " + datarow.Item("CBG_B_GL_DESC").ToString)
            Next
            Response.Write(vbTab + "Total Amount")
            Response.Write(vbCrLf)
            'End Header

            'Content
            Dim dr As DataRow
            Dim dr2 As DataRow
            Dim dr3 As DataRow
            j = 0
            For Each dr2 In ds2.Tables(0).Rows
                j = j + 1
                Response.Write(j)
                Response.Write(vbTab + dr2.Item("Staff ID").ToString)
                Response.Write(vbTab + dr2.Item("Staff Name").ToString)
                Response.Write(vbTab + dr2.Item("Job Grade").ToString)

                Dim blnStamp As Boolean
                For Each dr In ds.Tables(0).Rows 'Go by GL Code
                    blnStamp = False
                    For Each dr3 In ds3.Tables(0).Rows 'Check if this user has this claim
                        If dr2.Item("Staff ID").ToString = dr3.Item("ic_index").ToString And dr3.Item("CBG_B_GL_CODE").ToString = dr.Item("CBG_B_GL_CODE").ToString Then
                            Response.Write(vbTab + dr3.Item("Amount").ToString)
                            blnStamp = True
                            Exit For
                        End If
                    Next
                    If Not blnStamp Then
                        Response.Write(vbTab + "0.00")
                    End If
                Next
                Response.Write(vbTab + dr2.Item("Total Amount").ToString)
                Response.Write(vbCrLf)
            Next
            Response.End()

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try

    End Sub

    Private Sub ExportToPDF_StaffClaim()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strUserId As String
        Dim strCoyName As String
        Dim strDate1 As String = ""
        Dim strDate2 As String = ""
        Dim strFileName As String = ""
        Dim strStatus As String = ""

        strDate1 = Format(CDate(txtSDate.Text), "yyyy-MM-dd")
        strDate2 = Format(CDate(txtEDate.Text), "yyyy-MM-dd")

        If (CheckBox1.Checked = True And CheckBox2.Checked = True) Or (CheckBox1.Checked = False And CheckBox2.Checked = False) Then
            strStatus = "A', 'I"
        ElseIf (CheckBox1.Checked = True And CheckBox2.Checked = False) Then
            strStatus = "A"
        ElseIf (CheckBox1.Checked = False And CheckBox2.Checked = True) Then
            strStatus = "I"
        End If

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            'Zulham Case 8317 13022015
            Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                'Jules 2014.09.29 commented - IPP Stage 1 Addendum
                'If txtCentreCode.Text = "" Then
                '    .CommandText = "SELECT IM_S_COY_NAME, IM_INVOICE_NO, IM_PRCS_SENT, IM_PRCS_RECV, " & _
                '             "ID_RECEIVED_QTY, ID_UNIT_COST, IM_EXCHANGE_RATE, " & _
                '             "ID_RECEIVED_QTY * ID_UNIT_COST AS 'AMOUNT', " & _
                '             "IM_PAYMENT_DATE, ID_BRANCH_CODE, ID_COST_CENTER, IM_CREATED_BY, " & _
                '             "ic_status, ID_B_GL_CODE, CBG_B_GL_DESC, IM_CURRENCY_CODE, ic_business_reg_no " & _
                '             "FROM(invoice_mstr) " & _
                '             "INNER JOIN ipp_company ON IM_S_COY_ID = ic_index " & _
                '             "INNER JOIN invoice_details ON IM_INVOICE_NO = ID_INVOICE_NO " & _
                '             "INNER JOIN company_b_gl_code ON ID_B_GL_CODE = CBG_B_GL_CODE " & _
                '             "WHERE	(ic_coy_id = '" & Session("CompanyID") & "') AND (ic_coy_type = 'E') AND " & _
                '             "(CAST(IM_PAYMENT_DATE AS DATE) BETWEEN '" & strDate1 & "' AND '" & strDate2 & "' ) " & _
                '             "AND ic_status IN ('" & strStatus & "') AND ID_BRANCH_CODE = '" & txtBranchCode.Text & "' " & _
                '             "ORDER BY ID_B_GL_CODE, IM_S_COY_NAME "
                'Else
                '    .CommandText = "SELECT IM_S_COY_NAME, IM_INVOICE_NO, IM_PRCS_SENT, IM_PRCS_RECV, " & _
                '             "ID_RECEIVED_QTY, ID_UNIT_COST, IM_EXCHANGE_RATE, " & _
                '             "ID_RECEIVED_QTY * ID_UNIT_COST AS 'AMOUNT', " & _
                '             "IM_PAYMENT_DATE, ID_BRANCH_CODE, ID_COST_CENTER, IM_CREATED_BY, " & _
                '             "ic_status, ID_B_GL_CODE, CBG_B_GL_DESC, IM_CURRENCY_CODE, ic_business_reg_no " & _
                '             "FROM(invoice_mstr) " & _
                '             "INNER JOIN ipp_company ON IM_S_COY_ID = ic_index " & _
                '             "INNER JOIN invoice_details ON IM_INVOICE_NO = ID_INVOICE_NO " & _
                '             "INNER JOIN company_b_gl_code ON ID_B_GL_CODE = CBG_B_GL_CODE " & _
                '             "WHERE	(ic_coy_id = '" & Session("CompanyID") & "') AND (ic_coy_type = 'E') AND " & _
                '             "(CAST(IM_PAYMENT_DATE AS DATE) BETWEEN '" & strDate1 & "' AND '" & strDate2 & "' ) " & _
                '             "AND ic_status IN ('" & strStatus & "') AND ID_BRANCH_CODE = '" & txtBranchCode.Text & "' AND ID_COST_CENTER = '" & txtCentreCode.Text & "' " & _
                '             "ORDER BY ID_B_GL_CODE, IM_S_COY_NAME "
                'End If

                'Zulham Case 8317 13022015
                '.CommandText = "SELECT iM_payment_no,id_product_desc, id_gst_input_tax_code, id_gst_value,IM_S_COY_NAME, IM_INVOICE_NO, IM_PRCS_SENT, IM_PRCS_RECV, " & _
                '            "ID_RECEIVED_QTY, ID_UNIT_COST, IM_EXCHANGE_RATE, " & _
                '            "ID_RECEIVED_QTY * ID_UNIT_COST AS 'AMOUNT', " & _
                '            "IM_PAYMENT_DATE, ID_BRANCH_CODE, ID_COST_CENTER, IM_CREATED_BY, " & _
                '            "ic_status, ID_B_GL_CODE, CBG_B_GL_DESC, IM_CURRENCY_CODE, ic_business_reg_no, IFNULL(IC_ADDITIONAL_1,'') AS JobGrade " & _
                '            "FROM(invoice_mstr) " & _
                '            "INNER JOIN ipp_company ON IM_S_COY_ID = ic_index " & _
                '            "INNER JOIN invoice_details ON IM_INVOICE_NO = ID_INVOICE_NO " & _
                '            "INNER JOIN company_b_gl_code ON ID_B_GL_CODE = CBG_B_GL_CODE AND CBG_B_COY_ID = '" & strDefIPPCompID & "' " & _
                '            "WHERE	(ic_coy_id = '" & strDefIPPCompID & "') AND (ic_coy_type = 'E') AND " & _
                '            "(CAST(IM_PAYMENT_DATE AS DATE) BETWEEN '" & strDate1 & "' AND '" & strDate2 & "' ) " & _
                '            "AND ic_status IN ('" & strStatus & "') "

                'IPP Stage 2A - CH - 6 Mar 2015
                .CommandText = "SELECT iM_payment_no,id_product_desc, id_gst_input_tax_code, id_gst_value,IM_S_COY_NAME, IM_INVOICE_NO, IM_PRCS_SENT, IM_PRCS_RECV, " &
                            "ID_RECEIVED_QTY, ID_UNIT_COST, IM_EXCHANGE_RATE, " &
                            "ID_RECEIVED_QTY * ID_UNIT_COST AS 'AMOUNT', " &
                            "IM_PAYMENT_DATE, ID_BRANCH_CODE, ID_COST_CENTER, IM_CREATED_BY, " &
                            "ic_status, ID_B_GL_CODE, CBG_B_GL_DESC, IM_CURRENCY_CODE, ic_business_reg_no, IFNULL(IC_ADDITIONAL_1,'') AS JobGrade " &
                            "FROM invoice_mstr " &
                            "INNER JOIN ipp_company ON IM_S_COY_ID = ic_index AND ic_coy_id = '" & strDefIPPCompID & "' AND ic_coy_type = 'E' " &
                            "INNER JOIN invoice_details ON IM_INVOICE_NO = ID_INVOICE_NO AND IM_S_COY_ID = ID_S_COY_ID " &
                            "INNER JOIN company_b_gl_code ON ID_B_GL_CODE = CBG_B_GL_CODE AND CBG_B_COY_ID = '" & strDefIPPCompID & "' " &
                            "WHERE IM_B_COY_ID = '" & Session("CompanyID") & "' " &
                            "AND (CAST(IM_PAYMENT_DATE AS DATE) BETWEEN '" & strDate1 & "' AND '" & strDate2 & "' ) " &
                            "AND ic_status IN ('" & strStatus & "') "


                If txtBranchCode.Text <> "" Then
                    .CommandText = .CommandText & " AND ID_BRANCH_CODE = '" & txtBranchCode.Text & "' "
                End If

                If txtCentreCode.Text <> "" Then
                    .CommandText = .CommandText & " AND ID_COST_CENTER = '" & txtCentreCode.Text & "' "
                End If

                If txtFromGLCode.Text <> "" And txtToGLCode.Text <> "" Then
                    .CommandText = .CommandText & " AND ID_B_GL_CODE BETWEEN '" & txtFromGLCode.Text & "' AND '" & txtToGLCode.Text & "' "
                End If
                .CommandText = .CommandText & " ORDER BY ID_B_GL_CODE, IM_S_COY_NAME "

            End With

            da = New MySqlDataAdapter(cmd)


            ' da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyID")))
            strUserId = Session("UserName") 'Session("UserId")
            strCoyName = Session("CompanyName")

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("StaffClaim_StaffClaim", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)

            localreport.ReportPath = dispatcher.direct("Report", "StaffClaim.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")            

            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            If TotalParameter > 0 Then
                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
                For I = 0 To localreport.GetParameters.Count - 1
                    GetParameter = localreport.GetParameters.Item(I).Name
                    Select Case LCase(GetParameter)
                        Case "prmrequestedby"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

                        Case "logo"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                        Case "prmdate1"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, Format(CDate(strDate1), "dd/MM/yyyy"))

                        Case "prmdate2"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, Format(CDate(strDate2), "dd/MM/yyyy"))

                        Case "prmbuyercoyname"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)

                        Case Else
                    End Select
                Next
                localreport.SetParameters(par)
            End If

            localreport.Refresh()

            Dim deviceInfo As String =
                "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            strFileName = "IPPA0014 - Staff Claim by GL Account Code Report.pdf"
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()


        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub

    Private Sub ExportToExcel_InvoiceAgeingSD()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        'Dim strDate As String = ""
        Dim strFileName As String = ""

        'strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                'Zulham Case 8317 13022015
                .CommandText = "SELECT INV.IM_INVOICE_NO AS 'Invoice No.', " &
                               "DATE_FORMAT(INV.IM_DOC_DATE,'%d-%M-%Y') AS 'Invoice Date', " &
                               "DATE_FORMAT(INV.IM_DUE_DATE,'%d-%M-%Y') AS 'Invoice Due Date', " &
                               "FORMAT(INV.IM_INVOICE_TOTAL,2) AS 'Amount', " &
                               "INV.IM_CURRENCY_CODE AS 'Currency Code', " &
                               "INV.IM_S_COY_NAME AS 'Vendor Name', " &
                               "DATE_FORMAT(INV.IM_PRCS_SENT,'%d-%M-%Y') AS 'PSD Send Date', " &
                               "DATE_FORMAT(INV.IM_PRCS_RECV,'%d-%M-%Y') AS 'PSD Received Date', " &
                               "INV.IM_CREATED_BY AS 'Teller ID', " &
                               "SM.STATUS_DESC AS 'Status' " &
                               "FROM invoice_mstr INV " &
                               "INNER JOIN status_mstr SM ON INV.im_invoice_status = SM.STATUS_NO " &
                               "INNER JOIN company_dept_mstr CDM ON CDM.cdm_dept_index = INV.im_dept_index " &
                               "INNER JOIN user_mstr UM ON UM.um_dept_id = cdm_dept_code " &
                               "WHERE INV.IM_B_COY_ID = '" & Session("CompanyID") & "' " &
                               "AND INV.IM_INVOICE_STATUS NOT IN ( '4', '14' , '15' ) " &
                               "AND INV.IM_INVOICE_TYPE IS NOT NULL " &
                               "AND UM.UM_USER_ID = '" & Session("UserId") & "' " &
                               "AND SM.STATUS_TYPE = 'INV' " &
                               "ORDER BY INV.IM_DUE_DATE"

            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
            strFileName = "IPPA0015 - Daily Oustanding Invoice Ageing Report by source department" & "(" & Format(Now, "ddMMMyyyy") & ").xls"
            Dim attachment As String = "attachment;filename=" & strFileName
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"

            Dim dc As DataColumn
            Dim i As Integer

            i = 0
            For Each dc In ds.Tables(0).Columns
                If i > 0 Then
                    Response.Write(vbTab + dc.ColumnName)
                Else
                    Response.Write(dc.ColumnName)
                End If
                i += 1

            Next
            Response.Write(vbCrLf)

            Dim dr As DataRow
            For Each dr In ds.Tables(0).Rows
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    If i > 0 Then
                        Response.Write(vbTab + dr.Item(i).ToString)
                    Else
                        Response.Write(dr.Item(i).ToString)
                    End If
                Next
                Response.Write(vbCrLf)
            Next
            Response.End()

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try

    End Sub

    Private Sub ExportToPDF_InvoiceAgeingSD()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strUserId As String
        Dim strCoyName As String
        Dim strDate As String = ""
        Dim strFileName As String = ""

        'strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = "SELECT INV.IM_INVOICE_NO, INV.IM_DOC_DATE, INV.IM_DUE_DATE, " &
                               "INV.IM_CURRENCY_CODE, INV.IM_INVOICE_TOTAL, IM_CREATED_BY, " &
                               "INV.IM_S_COY_NAME, INV.IM_PRCS_SENT, INV.IM_PRCS_RECV, SM.STATUS_DESC " &
                               "FROM invoice_mstr INV " &
                               "INNER JOIN status_mstr SM ON INV.im_invoice_status = SM.STATUS_NO " &
                               "INNER JOIN company_dept_mstr CDM ON CDM.cdm_dept_index = INV.im_dept_index " &
                               "INNER JOIN user_mstr UM ON UM.um_dept_id = cdm_dept_code " &
                                "WHERE INV.IM_B_COY_ID = '" & Session("CompanyID") & "' " &
                                "AND INV.IM_INVOICE_STATUS NOT IN ( '4', '14' , '15' ) " &
                                "AND INV.IM_INVOICE_TYPE IS NOT NULL " &
                               "AND UM.UM_USER_ID = '" & Session("UserId") & "' " &
                               "AND SM.STATUS_TYPE = 'INV' " &
                               "ORDER BY INV.IM_DUE_DATE"

            End With

            da = New MySqlDataAdapter(cmd)

            ' da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyID")))
            strUserId = Session("UserName") 'Session("UserId")
            strCoyName = Session("CompanyName")

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("InvoiceAgeingSD_InvoiceAgeingSD", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)

            localreport.ReportPath = dispatcher.direct("Report", "InvoiceAgeingSD.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")

            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            If TotalParameter > 0 Then
                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
                For I = 0 To localreport.GetParameters.Count - 1
                    GetParameter = localreport.GetParameters.Item(I).Name
                    Select Case LCase(GetParameter)

                        Case "prmdate"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, Format(Now, "dd/MM/yyyy"))

                        Case "prmrequestedby"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

                        Case "logo"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                        Case "prmbuyercoyname"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)

                        Case Else
                    End Select
                Next
                localreport.SetParameters(par)
            End If

            localreport.Refresh()

            Dim deviceInfo As String =
                "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            strFileName = "IPPA0015 - Daily Oustanding Invoice Ageing Report by source department.pdf"
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()


        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub

    Private Sub ExportToPDF_PaymentAdvice()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strUserId As String
        Dim strCoyName As String
        Dim strDate As String = ""
        Dim strFileName As String = ""
        Dim dtPA As New DataTable
        Dim dsPA As New DataSet
        Dim decBankCharges As Decimal
        Dim objDb As New EAD.DBCom
        Dim strPaymetTerm As String = ""
        Dim strVenID As String = ""
        Dim i As Integer
        Dim strWaiveBankCharges As String
        'strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                If txtBoxPA.Text <> "" Then
                    .CommandText = "SELECT IM_PAYMENT_NO AS PayAdviseNo, im_invoice_no, im_invoice_type, im_doc_date, " _
                            & "FORMAT(IF(im_payment_term = 'TT',(im_invoice_total * im_exchange_rate),(im_invoice_total)), 2) AS im_invoice_total, " _
                            & "im_s_coy_id,im_s_coy_name,ic_addr_line1, ic_addr_line2,ic_addr_line3,ic_postcode,ic_city, ic_bank_code,ic_bank_acct, " _
                            & "(SELECT CODE_DESC FROM code_mstr WHERE code_abbr = ic_state AND code_value = ic_country AND code_category = 'S' AND code_deleted = 'N') AS ic_state, (SELECT CODE_DESC FROM code_mstr WHERE code_abbr = ic_country AND code_category = 'CT' AND code_deleted = 'N') AS ic_country,ic_phone,ic_fax, " _
                            & "im_payment_term, IM_CHEQUE_NO, " _
                            & "FORMAT(IF((im_payment_term = 'TT' AND im_withholding_opt = 2),(im_invoice_total * im_exchange_rate) * (im_withholding_tax/100),0),2) AS im_withholding_tax, " _
                            & "CASE WHEN im_withholding_opt = 1 THEN 'Company' " _
                            & "WHEN im_withholding_opt = 2 THEN 'Vendor' " _
                            & "WHEN im_withholding_opt = 3 THEN 'No WHT' " _
                            & "ELSE '' END AS im_withholding_opt ,'' AS BankCharge,im_currency_code,im_invoice_index, FORMAT(im_invoice_total,2) AS im_invoice_total2,im_payment_date,im_currency_code,IFNULL(im_cheque_no,'') as im_cheque_no " _
                            & "FROM INVOICE_MSTR " _
                            & "INNER JOIN company_mstr ON im_b_coy_id = cm_coy_id " _
                            & "INNER JOIN ipp_company ON ic_index = im_s_coy_id " _
                            & "WHERE im_invoice_status = 4 AND im_invoice_type IS NOT NULL AND im_b_coy_id = '" & Session("CompanyID") & "' AND im_payment_date IS NOT NULL AND IM_PAYMENT_NO = '" & txtBoxPA.Text & "' " _
                            & "ORDER BY im_s_coy_name,im_payment_term,im_invoice_no "

                    '& "WHERE im_invoice_status = 4 AND im_invoice_type IS NOT NULL AND im_b_coy_id = 'hlb' AND im_payment_date IS NOT NULL AND (IM_PRINTED_IND IS NULL or IM_PRINTED_IND = 'T') AND IM_PAYMENT_NO = '" & txtBoxPA.Text & "' " _

                ElseIf txtBoxBC.Text <> "" Then
                    .CommandText = "SELECT IM_PAYMENT_NO AS PayAdviseNo, im_invoice_no, im_invoice_type, im_doc_date, " _
                            & "FORMAT(IF(im_payment_term = 'TT',(im_invoice_total * im_exchange_rate),(im_invoice_total)), 2) AS im_invoice_total, " _
                            & "im_s_coy_id,im_s_coy_name,ic_addr_line1, ic_addr_line2,ic_addr_line3,ic_postcode,ic_city, ic_bank_code,ic_bank_acct, " _
                            & "(SELECT CODE_DESC FROM code_mstr WHERE code_abbr = ic_state AND code_value = ic_country AND code_category = 'S' AND code_deleted = 'N') AS ic_state, (SELECT CODE_DESC FROM code_mstr WHERE code_abbr = ic_country AND code_category = 'CT' AND code_deleted = 'N') AS ic_country,ic_phone,ic_fax, " _
                            & "im_payment_term, IM_CHEQUE_NO, " _
                            & "FORMAT(IF((im_payment_term = 'TT' AND im_withholding_opt = 2),(im_invoice_total * im_exchange_rate) * (im_withholding_tax/100),0),2) AS im_withholding_tax, " _
                            & "CASE WHEN im_withholding_opt = 1 THEN 'Company' " _
                            & "WHEN im_withholding_opt = 2 THEN 'Vendor' " _
                            & "WHEN im_withholding_opt = 3 THEN 'No WHT' " _
                            & "ELSE '' END AS im_withholding_opt ,'' AS BankCharge,im_currency_code,im_invoice_index, FORMAT(im_invoice_total,2) AS im_invoice_total2,im_payment_date,im_currency_code,IFNULL(im_cheque_no,'') as im_cheque_no " _
                            & "FROM INVOICE_MSTR " _
                            & "INNER JOIN company_mstr ON im_b_coy_id = cm_coy_id " _
                            & "INNER JOIN ipp_company ON ic_index = im_s_coy_id " _
                            & "WHERE im_invoice_status = 4 AND im_invoice_type IS NOT NULL AND im_b_coy_id = '" & Session("CompanyID") & "' AND im_payment_date IS NOT NULL  AND IM_CHEQUE_NO = '" & txtBoxBC.Text & "' " _
                            & "ORDER BY im_s_coy_name,im_payment_term,im_invoice_no "

                    '& "WHERE im_invoice_status = 4 AND im_invoice_type IS NOT NULL AND im_b_coy_id = 'hlb' AND im_payment_date IS NOT NULL AND (IM_PRINTED_IND IS NULL or IM_PRINTED_IND = 'T') AND IM_CHEQUE_NO = '" & txtBoxBC.Text & "' " _
                End If

            End With

            da = New MySqlDataAdapter(cmd)

            ' da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyID")))
            strUserId = Session("UserName") 'Session("UserId")
            strCoyName = Session("CompanyName")

            da.Fill(ds)

            '----------------------------------------------------------

            dtPA.Columns.Add("PayAdviseNo", Type.GetType("System.String"))
            dtPA.Columns.Add("im_invoice_no", Type.GetType("System.String"))
            dtPA.Columns.Add("im_invoice_type", Type.GetType("System.String"))
            dtPA.Columns.Add("im_doc_date", Type.GetType("System.String"))
            dtPA.Columns.Add("im_invoice_total", Type.GetType("System.Decimal"))
            dtPA.Columns.Add("im_s_coy_id", Type.GetType("System.String"))
            dtPA.Columns.Add("im_s_coy_name", Type.GetType("System.String"))
            dtPA.Columns.Add("ic_addr_line1", Type.GetType("System.String"))
            dtPA.Columns.Add("ic_addr_line2", Type.GetType("System.String"))
            dtPA.Columns.Add("ic_addr_line3", Type.GetType("System.String"))
            dtPA.Columns.Add("ic_postcode", Type.GetType("System.String"))
            dtPA.Columns.Add("ic_city", Type.GetType("System.String"))
            dtPA.Columns.Add("ic_state", Type.GetType("System.String"))
            dtPA.Columns.Add("ic_country", Type.GetType("System.String"))
            dtPA.Columns.Add("ic_phone", Type.GetType("System.String"))
            dtPA.Columns.Add("ic_fax", Type.GetType("System.String"))
            dtPA.Columns.Add("im_payment_term", Type.GetType("System.String"))
            dtPA.Columns.Add("im_withholding_tax", Type.GetType("System.Decimal"))
            dtPA.Columns.Add("im_withholding_opt", Type.GetType("System.String"))
            dtPA.Columns.Add("BankCharge", Type.GetType("System.Decimal"))
            dtPA.Columns.Add("ic_bank_code", Type.GetType("System.String"))
            dtPA.Columns.Add("ic_bank_acct", Type.GetType("System.String"))
            dtPA.Columns.Add("im_invoice_index", Type.GetType("System.String"))
            dtPA.Columns.Add("im_currency_code", Type.GetType("System.String"))
            dtPA.Columns.Add("im_invoice_total2", Type.GetType("System.Decimal"))
            dtPA.Columns.Add("im_cheque_no", Type.GetType("System.String"))
            dtPA.Columns.Add("im_payment_date", Type.GetType("System.String"))

            For i = 0 To ds.Tables(0).Rows.Count - 1

                Dim dtr As DataRow
                dtr = dtPA.NewRow()
                strWaiveBankCharges = objDb.GetVal("SELECT IFNULL(ic_waive_charges,'') AS ic_waive_charges FROM ipp_company WHERE ic_Coy_type = 'V' and ic_index = '" & ds.Tables(0).Rows(i)("im_s_coy_id") & "' ")
                If strWaiveBankCharges = "N" Then
                    If ds.Tables(0).Rows(i)("im_payment_term") = "IBG" Then
                        decBankCharges = objDb.GetVal("SELECT IF(IFNULL(ip_param_value,'')='',0,ip_param_value) AS ip_param_value FROM ipp_parameter WHERE ip_coy_id = '" & Session("CompanyID") & "' AND ip_param = 'IBG_CHARGE'")
                    ElseIf ds.Tables(0).Rows(i)("im_payment_term") = "RENTAS" Then
                        decBankCharges = objDb.GetVal("SELECT IF(IFNULL(ip_param_value,'')='',0,ip_param_value) AS ip_param_value FROM ipp_parameter WHERE ip_coy_id = '" & Session("CompanyID") & "' AND ip_param = 'RENTAS_CHARGE'")
                    ElseIf ds.Tables(0).Rows(i)("im_payment_term") = "TT" And ds.Tables(0).Rows(i)("im_invoice_total") <= 10000 Then
                        decBankCharges = objDb.GetVal("SELECT IF(IFNULL(ip_param_value,'')='',0,ip_param_value) AS ip_param_value FROM ipp_parameter WHERE ip_coy_id = '" & Session("CompanyID") & "' AND ip_param = 'TT1_CHARGE'")
                    ElseIf ds.Tables(0).Rows(i)("im_payment_term") = "TT" And ds.Tables(0).Rows(i)("im_invoice_total") > 10000 Then
                        decBankCharges = objDb.GetVal("SELECT IF(IFNULL(ip_param_value,'')='',0,ip_param_value) AS ip_param_value FROM ipp_parameter WHERE ip_coy_id = '" & Session("CompanyID") & "' AND ip_param = 'TT2_CHARGE'")
                    Else
                        decBankCharges = 0
                    End If
                End If

                If ds.Tables(0).Rows(i)("im_invoice_type") = "CN" Then
                    If ds.Tables(0).Rows(i)("im_payment_term") = "TT" Then
                        ds.Tables(0).Rows(i)("im_invoice_total") = "-" & ds.Tables(0).Rows(i)("im_invoice_total")
                        ds.Tables(0).Rows(i)("im_invoice_total2") = "-" & ds.Tables(0).Rows(i)("im_invoice_total2")
                    Else
                        ds.Tables(0).Rows(i)("im_invoice_total") = "-" & ds.Tables(0).Rows(i)("im_invoice_total")
                    End If
                End If

                dtr("PayAdviseNo") = ds.Tables(0).Rows(i)("PayAdviseNo") 'strPANo
                dtr("im_invoice_no") = ds.Tables(0).Rows(i)("im_invoice_no")
                dtr("im_invoice_type") = ds.Tables(0).Rows(i)("im_invoice_type")
                dtr("im_doc_date") = Format(ds.Tables(0).Rows(i)("im_doc_date"), "dd-MM-yy")
                dtr("im_invoice_total") = ds.Tables(0).Rows(i)("im_invoice_total")
                dtr("im_s_coy_id") = ds.Tables(0).Rows(i)("im_s_coy_id")
                dtr("im_s_coy_name") = ds.Tables(0).Rows(i)("im_s_coy_name")
                dtr("ic_addr_line1") = ds.Tables(0).Rows(i)("ic_addr_line1")
                dtr("ic_addr_line2") = ds.Tables(0).Rows(i)("ic_addr_line2")
                dtr("ic_addr_line3") = ds.Tables(0).Rows(i)("ic_addr_line3")
                dtr("ic_postcode") = ds.Tables(0).Rows(i)("ic_postcode")
                dtr("ic_city") = ds.Tables(0).Rows(i)("ic_city")
                dtr("ic_state") = ds.Tables(0).Rows(i)("ic_state")
                dtr("ic_country") = ds.Tables(0).Rows(i)("ic_country")
                dtr("ic_phone") = ds.Tables(0).Rows(i)("ic_phone")
                dtr("ic_fax") = ds.Tables(0).Rows(i)("ic_fax")
                dtr("im_payment_term") = ds.Tables(0).Rows(i)("im_payment_term")
                dtr("im_withholding_tax") = ds.Tables(0).Rows(i)("im_withholding_tax")
                dtr("im_withholding_opt") = ds.Tables(0).Rows(i)("im_withholding_opt")
                dtr("BankCharge") = decBankCharges
                dtr("ic_bank_code") = ds.Tables(0).Rows(i)("ic_bank_code")
                dtr("ic_bank_acct") = ds.Tables(0).Rows(i)("ic_bank_acct")
                dtr("im_invoice_index") = ds.Tables(0).Rows(i)("im_invoice_index")
                dtr("im_currency_code") = ds.Tables(0).Rows(i)("im_currency_code")
                dtr("im_invoice_total2") = ds.Tables(0).Rows(i)("im_invoice_total2")
                dtr("im_cheque_no") = ds.Tables(0).Rows(i)("im_cheque_no")
                dtr("im_payment_date") = Format(ds.Tables(0).Rows(i)("im_payment_date"), "dd-MMM-yyyy")

                dtPA.Rows.Add(dtr)

                strVenID = ds.Tables(0).Rows(i)("im_s_coy_id")
                strPaymetTerm = ds.Tables(0).Rows(i)("im_payment_term")

            Next


            dsPA.Tables.Add(dtPA)
            '----------------------------------------------------------
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("PaymentAdvice_PaymentAdvice", dsPA.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)

            localreport.ReportPath = dispatcher.direct("Report", "PaymentAdvice.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")

            localreport.EnableExternalImages = True

            'Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            'IPP Gst Stage 2A - CH - 5 Mar 2015
            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            If TotalParameter > 0 Then
                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
                For i = 0 To localreport.GetParameters.Count - 1
                    GetParameter = localreport.GetParameters.Item(i).Name
                    Select Case LCase(GetParameter)
                        Case "prmbuyercoyname"
                            par(i) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)
                    End Select
                Next
                localreport.SetParameters(par)
            End If
            '------------------------------------

            localreport.Refresh()

            Dim deviceInfo As String =
                "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            strFileName = "Payment Advice.pdf"
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()


        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub

    Private Sub ExportToPDF_TaxInvoice()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strUserId As String
        Dim strCoyName As String
        Dim strDate As String = ""
        Dim strFileName As String = ""
        Dim dtDN As New DataTable
        Dim dsDN As New DataSet
        Dim decBankCharges As Decimal
        Dim objDb As New EAD.DBCom
        Dim strPaymetTerm As String = ""
        Dim strVenID As String = ""
        Dim i As Integer
        Dim strBRCoyName As String = ""
        Dim strVendorID As String = ""
        'strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            'Zulham Case 8317 13022015
            Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                'Zulham Case 8317 13022015
                .CommandText = "SELECT ID_DN_NO as DN_No, im_s_coy_id,id_product_desc, im_invoice_type, FORMAT(IF(im_payment_term = 'TT',(ID_RECEIVED_QTY * ID_UNIT_COST),(ID_RECEIVED_QTY * ID_UNIT_COST)) ,2) AS Amount, " _
                                 & "FORMAT(IF(im_payment_term = 'TT',(ID_RECEIVED_QTY * ID_UNIT_COST * im_exchange_rate),(ID_RECEIVED_QTY * ID_UNIT_COST )) ,2) AS AmountWithExg, " _
                                 & "im_currency_code,ic_coy_name,ic_addr_line1, ic_addr_line2,ic_addr_line3,ic_postcode,ic_city, (SELECT CODE_DESC FROM code_mstr WHERE code_abbr = ic_state AND code_value = ic_country AND code_category = 'S' AND code_deleted = 'N') as ic_state ,(SELECT CODE_DESC FROM code_mstr WHERE code_abbr = ic_country AND code_category = 'CT' AND code_deleted = 'N') AS ic_country,ic_phone,ic_fax, " _
                                 & "cm_coy_name, cm_addr_line1, cm_addr_line2, cm_addr_line3, cm_postcode, cm_city,  (SELECT CODE_DESC FROM code_mstr WHERE code_abbr = cm_state AND code_value = cm_country AND code_category = 'S' AND code_deleted = 'N') as cm_state, (SELECT CODE_DESC FROM code_mstr WHERE code_abbr = cm_country AND code_category = 'CT' AND code_deleted = 'N') as cm_country, cm_phone, cm_fax, IM_PAYMENT_NO,im_invoice_no, im_s_coy_name,im_created_by,im_exchange_rate,id_dr_exchange_rate,id_dr_currency,im_payment_term,  " _
                                 & "'TAX INVOICE' AS 'header', " _
                                 & "IF(im_payment_term = 'TT', id_gst_value * im_exchange_rate, " _
                                 & "IF(id_dr_exchange_rate IS NULL OR id_dr_exchange_rate = 0, id_gst_value, id_gst_value * id_dr_exchange_rate)) AS 'id_gst_value', " _
                                 & "IF(im_payment_term = 'TT', (id_gst_value * im_exchange_rate) + (ID_RECEIVED_QTY * ID_UNIT_COST * im_exchange_rate), " _
                                 & "IF(id_dr_exchange_rate IS NULL OR id_dr_exchange_rate = 0, id_gst_value + (ID_RECEIVED_QTY * ID_UNIT_COST), (id_gst_value * id_dr_exchange_rate) + (ID_RECEIVED_QTY * ID_UNIT_COST * id_dr_exchange_rate))) AS 'totalwithGST' " _
                                 & "FROM INVOICE_MSTR " _
                                 & "LEFT JOIN invoice_details ON id_invoice_no = im_invoice_no AND id_s_coy_id = im_s_coy_id " _
                                 & "INNER JOIN ipp_company ON ic_other_b_coy_code = id_pay_for AND ic_coy_id = '" & strDefIPPCompID & "' " _
                                 & "INNER JOIN company_mstr ON im_b_coy_id = cm_coy_id " _
                                 & "WHERE im_invoice_status = 4 AND id_doc_type_generated = 'TI' AND im_invoice_type IS NOT NULL AND im_b_coy_id = '" & Session("CompanyID") & "'  AND im_payment_date IS NOT NULL AND id_dn_no = '" & txtTaxInvoice.Text & "' ORDER BY ic_coy_name,im_s_coy_id "
            End With

            da = New MySqlDataAdapter(cmd)

            ' da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyID")))
            strUserId = Session("UserName") 'Session("UserId")
            strCoyName = Session("CompanyName")

            da.Fill(ds)

            '----------------------------------------------------------

            dtDN.Columns.Add("DNNo", Type.GetType("System.String"))
            dtDN.Columns.Add("im_s_coy_id", Type.GetType("System.String"))
            dtDN.Columns.Add("id_product_desc", Type.GetType("System.String"))
            dtDN.Columns.Add("im_invoice_type", Type.GetType("System.String"))
            dtDN.Columns.Add("im_currency_code", Type.GetType("System.String"))
            dtDN.Columns.Add("Amount", Type.GetType("System.Decimal"))
            dtDN.Columns.Add("AmountWithExg", Type.GetType("System.Decimal"))
            dtDN.Columns.Add("cm_coy_name", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_addr_line1", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_addr_line2", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_addr_line3", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_postcode", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_city", Type.GetType("System.String"))
            dtDN.Columns.Add("code_desc2", Type.GetType("System.String"))
            dtDN.Columns.Add("code_desc3", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_phone", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_fax", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_coy_name", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_addr_line1", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_addr_line2", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_addr_line3", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_postcode", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_city", Type.GetType("System.String"))
            dtDN.Columns.Add("code_desc", Type.GetType("System.String"))
            dtDN.Columns.Add("code_desc1", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_phone", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_fax", Type.GetType("System.String"))
            dtDN.Columns.Add("im_s_coy_name", Type.GetType("System.String"))
            dtDN.Columns.Add("im_invoice_no", Type.GetType("System.String"))
            dtDN.Columns.Add("im_payment_no", Type.GetType("System.String"))
            dtDN.Columns.Add("im_created_by", Type.GetType("System.String"))
            dtDN.Columns.Add("im_exchange_rate", Type.GetType("System.String"))
            dtDN.Columns.Add("id_dr_exchange_rate", Type.GetType("System.String"))
            dtDN.Columns.Add("id_dr_currency", Type.GetType("System.String"))
            dtDN.Columns.Add("im_payment_term", Type.GetType("System.String"))
            dtDN.Columns.Add("header", Type.GetType("System.String"))
            dtDN.Columns.Add("id_gst_value", Type.GetType("System.Decimal"))
            dtDN.Columns.Add("totalwithGST", Type.GetType("System.Decimal"))

            For i = 0 To ds.Tables(0).Rows.Count - 1

                Dim dtr As DataRow
                dtr = dtDN.NewRow()

                dtr("DNNo") = ds.Tables(0).Rows(i)("DN_NO") 'strDNNo
                dtr("im_s_coy_id") = ds.Tables(0).Rows(i)("im_s_coy_id")
                dtr("id_product_desc") = ds.Tables(0).Rows(i)("id_product_desc")
                dtr("im_invoice_type") = ds.Tables(0).Rows(i)("im_invoice_type")
                dtr("im_currency_code") = ds.Tables(0).Rows(i)("im_currency_code")
                dtr("Amount") = ds.Tables(0).Rows(i)("Amount")
                dtr("AmountWithExg") = ds.Tables(0).Rows(i)("AmountWithExg")
                dtr("cm_coy_name") = ds.Tables(0).Rows(i)("cm_coy_name")
                dtr("cm_addr_line1") = ds.Tables(0).Rows(i)("cm_addr_line1")
                dtr("cm_addr_line2") = ds.Tables(0).Rows(i)("cm_addr_line2")
                dtr("cm_addr_line3") = ds.Tables(0).Rows(i)("cm_addr_line3")
                dtr("cm_postcode") = ds.Tables(0).Rows(i)("cm_postcode")
                dtr("cm_city") = ds.Tables(0).Rows(i)("cm_city")
                dtr("code_desc2") = ds.Tables(0).Rows(i)("cm_state")
                dtr("code_desc3") = ds.Tables(0).Rows(i)("cm_country")
                dtr("cm_phone") = ds.Tables(0).Rows(i)("cm_phone")
                dtr("cm_fax") = ds.Tables(0).Rows(i)("cm_fax")
                dtr("ic_coy_name") = ds.Tables(0).Rows(i)("ic_coy_name")
                dtr("ic_addr_line1") = ds.Tables(0).Rows(i)("ic_addr_line1")
                dtr("ic_addr_line2") = ds.Tables(0).Rows(i)("ic_addr_line2")
                dtr("ic_addr_line3") = ds.Tables(0).Rows(i)("ic_addr_line3")
                dtr("ic_postcode") = ds.Tables(0).Rows(i)("ic_postcode")
                dtr("ic_city") = ds.Tables(0).Rows(i)("ic_city")
                dtr("code_desc") = ds.Tables(0).Rows(i)("ic_state")
                dtr("code_desc1") = ds.Tables(0).Rows(i)("ic_country")
                dtr("ic_phone") = ds.Tables(0).Rows(i)("ic_phone")
                dtr("ic_fax") = ds.Tables(0).Rows(i)("ic_fax")
                dtr("im_s_coy_name") = ds.Tables(0).Rows(i)("im_s_coy_name")
                dtr("im_invoice_no") = ds.Tables(0).Rows(i)("im_invoice_no")
                dtr("im_payment_no") = ds.Tables(0).Rows(i)("im_payment_no")
                dtr("im_created_by") = ds.Tables(0).Rows(i)("im_created_by")
                dtr("im_exchange_rate") = ds.Tables(0).Rows(i)("im_exchange_rate")
                dtr("id_dr_exchange_rate") = ds.Tables(0).Rows(i)("id_dr_exchange_rate")
                dtr("id_dr_currency") = ds.Tables(0).Rows(i)("id_dr_currency")
                dtr("im_payment_term") = ds.Tables(0).Rows(i)("im_payment_term")
                dtr("header") = ds.Tables(0).Rows(i)("header")
                If Not ds.Tables(0).Rows(i)("id_gst_value") Is DBNull.Value Then
                    dtr("id_gst_value") = ds.Tables(0).Rows(i)("id_gst_value")
                Else
                    dtr("id_gst_value") = 0.0
                End If
                If Not ds.Tables(0).Rows(i)("totalwithGST") Is DBNull.Value Then
                    dtr("totalwithGST") = ds.Tables(0).Rows(i)("totalwithGST")
                Else
                    dtr("totalwithGST") = 0.0
                End If
                dtDN.Rows.Add(dtr)

                strBRCoyName = ds.Tables(0).Rows(i)("ic_coy_name")
                strVendorID = ds.Tables(0).Rows(i)("im_s_coy_id")

            Next


            dsDN.Tables.Add(dtDN)
            '----------------------------------------------------------
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("dsDebitNote_DataTable1", dsDN.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)

            localreport.ReportPath = dispatcher.direct("Report", "TaxInvoice.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")

            localreport.EnableExternalImages = True

            'Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            'IPP Gst Stage 2A - CH - 5 Mar 2015
            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            If TotalParameter > 0 Then
                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
                For i = 0 To localreport.GetParameters.Count - 1
                    GetParameter = localreport.GetParameters.Item(i).Name
                    Select Case LCase(GetParameter)
                        Case "prmbuyercoyname"
                            par(i) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)
                    End Select
                Next
                localreport.SetParameters(par)
            End If
            '------------------------------------

            localreport.Refresh()

            Dim deviceInfo As String =
                "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            strFileName = "Tax Invoice.pdf"
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()


        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub

    Protected Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        ViewState("FO") = ""
        ViewState("FM") = ""

        If cboReportType.SelectedValue = "Excel" Then
            Select Case ViewState("rptName")
                Case "DailyGL"
                    If txtDate.Text.Trim = "" Then
                        Common.NetMsgbox(Me, "Date is required.")
                    Else
                        ExportToExcel_DailyGL()
                    End If

                Case "IPPB4Pay"
                    If ddlFO.SelectedItem.Value <> "" Then
                        ViewState("FO") = ddlFO.SelectedItem.Value
                    End If
                    If Not rdbtnVendor.Checked And Not rdbtnEmployee.Checked Then
                        Common.NetMsgbox(Me, "Type is required.")
                    Else
                        ExportToExcel_InvPendingApp()
                    End If


                Case "IPPAftPay"
                    If ddlFO.SelectedItem.Value <> "" Then
                        ViewState("FM") = ddlFO.SelectedItem.Value
                    End If
                    If Not rdbtnVendor.Checked And Not rdbtnEmployee.Checked Then
                        Common.NetMsgbox(Me, "Type is required.")
                    Else
                        ExportToExcel_InvReleased()
                    End If

                Case "IPPA0012"
                    ExportToExcel_ActiveVendor()

                Case "IPPA0013"
                    ExportToExcel_InactiveVendor()

                Case "IPPA0014"
                    'Jules 2014.09.29 - IPP Stage 1 Addendum
                    'If txtBranchCode.Text = "" Then
                    '    Common.NetMsgbox(Me, "Branch Code is required.")
                    '    'ElseIf txtCentreCode.Text = "" Then
                    '    '    Common.NetMsgbox(Me, "Cost Centre Code is required.")
                    'ElseIf txtSDate.Text.Trim = "" Or txtEDate.Text.Trim = "" Then
                    If txtFromGLCode.Text <> "" And txtToGLCode.Text = "" Then
                        Common.NetMsgbox(Me, "To GL Code is required.")
                    ElseIf txtToGLCode.Text <> "" And txtFromGLCode.Text = "" Then
                        Common.NetMsgbox(Me, "From GL Code is required.")
                    End If

                    If txtSDate.Text.Trim = "" Or txtEDate.Text.Trim = "" Then
                        Common.NetMsgbox(Me, "Date is required.")
                    ElseIf (DateDiff("m", txtSDate.Text, txtEDate.Text) > 6) Then
                        Common.NetMsgbox(Me, "Date range must not be more than 6 months.")
                    ElseIf (CDate(txtEDate.Text) < CDate(txtSDate.Text)) Then
                        Common.NetMsgbox(Me, "End date should be greater than the start date.")
                    Else
                        ExportToExcel_StaffClaim()
                    End If

                Case "IPPA0015"
                    ExportToExcel_InvoiceAgeingSD()

                Case "IPPA0016"
                    If Me.txtSDate.Text = "" Or Me.txtEDate.Text = "" Then
                        Common.NetMsgbox(Me, "Start Date and End Date are required.")
                    ElseIf (CDate(txtEDate.Text) < CDate(txtSDate.Text)) Then
                        Common.NetMsgbox(Me, "End date should be greater than the start date.")
                    Else
                        ExportToExcel_Top100Vendor()
                    End If

                Case "IPPS0017"
                    If Me.txtStartDate.Text = "" Or Me.txtEndDate.Text = "" Then
                        Common.NetMsgbox(Me, "Start Date and End Date are required.")
                    ElseIf (CDate(txtEndDate.Text) < CDate(txtStartDate.Text)) Then
                        Common.NetMsgbox(Me, "End date should be greater than the start date.")
                    Else
                        ExportToExcel_PaidStaffClaim()
                    End If
                    'Jules 2015.01.27 - IPP Stage 2A
                Case "BillPendAppr"
                    If ddlFO.SelectedItem.Value <> "" Then
                        ViewState("AO") = ddlFO.SelectedItem.Value
                    End If
                    ExportToExcel_BillPendingAppr()
                Case "DailyBillSumm"
                    If ddlFO.SelectedItem.Value <> "" Then
                        ViewState("AO") = ddlFO.SelectedItem.Value
                    End If
                    ExportToExcel_DailyBillingSummary()
            End Select

        ElseIf cboReportType.SelectedValue = "PDF" Then
            Select Case ViewState("rptName")
                Case "DailyGL"
                    If txtDate.Text.Trim = "" Then
                        Common.NetMsgbox(Me, "Date is required.")
                    Else
                        ExportToPDF_DailyGL()
                    End If

                Case "IPPB4Pay"
                    If ddlFO.SelectedItem.Value <> "" Then
                        ViewState("FO") = ddlFO.SelectedItem.Value
                    End If
                    If Not rdbtnVendor.Checked And Not rdbtnEmployee.Checked Then
                        Common.NetMsgbox(Me, "Type is required.")
                    Else
                        ExportToPDF_InvPendingApp()
                    End If

                Case "IPPAftPay"
                    If ddlFO.SelectedItem.Value <> "" Then
                        ViewState("FM") = ddlFO.SelectedItem.Value
                    End If
                    If Not rdbtnVendor.Checked And Not rdbtnEmployee.Checked Then
                        Common.NetMsgbox(Me, "Type is required.")
                    Else
                        ExportToPDF_InvReleased()
                    End If

                Case "IPPA0012"
                    ExportToPDF_ActiveVendor()

                Case "IPPA0013"
                    ExportToPDF_InactiveVendor()

                Case "IPPA0014"
                    ''Jules 2014.09.29 - IPP Stage 1 Addendum
                    'If txtBranchCode.Text = "" Then
                    '    Common.NetMsgbox(Me, "Branch Code is required.")
                    '    'ElseIf txtCentreCode.Text = "" Then
                    '    '    Common.NetMsgbox(Me, "Cost Centre Code is required.")
                    'ElseIf txtSDate.Text.Trim = "" Or txtEDate.Text.Trim = "" Then
                    If txtFromGLCode.Text <> "" And txtToGLCode.Text = "" Then
                        Common.NetMsgbox(Me, "To GL Code is required.")
                    ElseIf txtToGLCode.Text <> "" And txtFromGLCode.Text = "" Then
                        Common.NetMsgbox(Me, "From GL Code is required.")
                    End If
                    If txtSDate.Text.Trim = "" Or txtEDate.Text.Trim = "" Then
                        Common.NetMsgbox(Me, "Date is required.")
                    ElseIf (DateDiff("m", txtSDate.Text, txtEDate.Text) > 6) Then
                        Common.NetMsgbox(Me, "Date range must be within 6 months.")
                    ElseIf (CDate(txtEDate.Text) < CDate(txtSDate.Text)) Then
                        Common.NetMsgbox(Me, "End date should be greater than the start date.")
                    Else
                        ExportToPDF_StaffClaim()
                    End If

                Case "IPPA0015"
                    ExportToPDF_InvoiceAgeingSD()

                Case "PayAdv"
                    If txtBoxPA.Text.Trim = "" And txtBoxBC.Text.Trim = "" Then
                        Common.NetMsgbox(Me, "Payment Advice No. or Bankers Cheque No. is required.")
                    ElseIf txtBoxPA.Text.Trim <> "" And txtBoxBC.Text.Trim <> "" Then
                        Common.NetMsgbox(Me, "Please enter either Payment Advice No. or Bankers Cheque No.")
                    Else
                        ExportToPDF_PaymentAdvice()
                    End If
                Case "TaxInvoice"
                    If txtTaxInvoice.Text.Trim = "" Then
                        Common.NetMsgbox(Me, "Tax Invoice No. is required.")
                    Else
                        'Jules 2015.01.30 IPP Stage 2A
                        ExportToPDF_TaxInvoice_Billing()
                    End If
                    'Zulham 18062015 - Added a menu for billing tax invoice
                    'Zulham 10/08/2017 - IPP Stage 3
                Case "BillingTaxInvoice"
                    If txtTaxInvoice.Text.Trim = "" Then
                        Common.NetMsgbox(Me, "Tax Invoice No. is required.")
                    Else
                        ExportToPDF_TaxInvoice_Billing()
                    End If
                    'Zulham Apr 16, 2013
                Case "IPPA0016"
                    If Me.txtSDate.Text = "" Or Me.txtEDate.Text = "" Then
                        Common.NetMsgbox(Me, "Start Date and End Date are required.")
                    ElseIf (CDate(txtEDate.Text) < CDate(txtSDate.Text)) Then
                        Common.NetMsgbox(Me, "End date should be greater than the start date.")
                    Else
                        ExportToPDF_Top100Vendor()
                    End If
                    'End
                Case "DebitNote"
                    If txtDebitNoteNo.Text.Trim = "" Then
                        Common.NetMsgbox(Me, "Debit Note No. is required.")
                    Else
                        ExportToPDF_DebitNote()
                    End If
                Case "DebitAdvice"
                    If txtDebitAdviceNo.Text.Trim = "" Then
                        Common.NetMsgbox(Me, "Debit Advice No. is required.")
                    Else
                        ExportToPDF_DebitAdvice()
                    End If
                    'Jules 2015.01.30 IPP Stage 2A
                Case "BillPendAppr"
                    If ddlFO.SelectedItem.Value <> "" Then
                        ViewState("AO") = ddlFO.SelectedItem.Value
                    End If
                    ExportToPDF_BillPendingAppr()
                Case "DailyBillSumm"
                    If ddlFO.SelectedItem.Value <> "" Then
                        ViewState("AO") = ddlFO.SelectedItem.Value
                    End If
                    ExportToPDF_DailyBillingSummary()
                    'Zulham 08/08/2017 - ipp stage 3
                Case "BillingDebitNote"
                    If txtTaxInvoice.Text.Trim = "" Then
                        Common.NetMsgbox(Me, "Debit Note No. is required.")
                    Else
                        ExportToPDF_BillingReports("DN")
                    End If
                Case "BillingCreditNote"
                    If txtTaxInvoice.Text.Trim = "" Then
                        Common.NetMsgbox(Me, "Credit Note No. is required.")
                    Else
                        ExportToPDF_BillingReports("CN")
                    End If
                Case "BillingDebitAdvice"
                    If txtTaxInvoice.Text.Trim = "" Then
                        Common.NetMsgbox(Me, "Debit Advice No. is required.")
                    Else
                        ExportToPDF_BillingReports("DA")
                    End If
                Case "BillingCreditAdvice"
                    If txtTaxInvoice.Text.Trim = "" Then
                        Common.NetMsgbox(Me, "Credit Advice No. is required.")
                    Else
                        ExportToPDF_BillingReports("CA")
                    End If
                    ''''

            End Select

        End If
    End Sub

    Private Sub ExportToExcel_Top100Vendor()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim strDate1 As String = ""
        Dim strDate2 As String = ""
        Dim strFileName As String = ""
        Dim strStatus As String = ""

        strDate1 = Format(CDate(txtSDate.Text), "yyyy-MM-dd")
        strDate2 = Format(CDate(txtEDate.Text), "yyyy-MM-dd")

        If (CheckBox1.Checked = True And CheckBox2.Checked = True) Or (CheckBox1.Checked = False And CheckBox2.Checked = False) Then
            strStatus = "A', 'I"
        ElseIf (CheckBox1.Checked = True And CheckBox2.Checked = False) Then
            strStatus = "A"
        ElseIf (CheckBox1.Checked = False And CheckBox2.Checked = True) Then
            strStatus = "I"
        End If

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                With cmd
                    .Connection = conn
                    .CommandType = CommandType.Text
                    '.CommandText = "SELECT im_s_coy_name, im_currency_code,IF(im_currency_code <> 'MYR',CAST(ifnull(im_invoice_total,0) AS DECIMAL(20,2)),0) AS 'foreign currency amt', " _
                    '             & "IF(im_currency_code <> 'MYR',CAST(IFNULL(im_invoice_total,0)*im_exchange_rate AS DECIMAL(20,2)),im_invoice_total) AS 'total amount'" _
                    '             & "FROM(invoice_mstr) WHERE im_invoice_status = 4 AND im_b_coy_id = 'hlb' and im_payment_date between '" & Format(CDate(Me.txtSDate.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(Me.txtEDate.Text), "yyyy-MM-dd") & "' order by im_invoice_total desc"
                    '.CommandText = "SELECT im_s_coy_name, im_currency_code,IF(im_currency_code <> 'MYR'," _
                    '                                         & "FORMAT(ifnull(im_invoice_total,'0'),2),'0') AS 'foreign currency amt'," _
                    '                                         & "IF(im_currency_code <> 'MYR',FORMAT(IFNULL(im_invoice_total,0)*im_exchange_rate,2)," _
                    '                                         & "FORMAT(im_invoice_total,2)) AS 'total amount'" _
                    '                                         & "FROM(invoice_mstr) WHERE im_invoice_status = 4 AND im_b_coy_id = '" & Session("CompanyID") & "' AND " _
                    '                                         & "im_payment_date between '" & Format(CDate(Me.txtSDate.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(Me.txtEDate.Text), "yyyy-MM-dd") & "' ORDER BY im_currency_code ASC, im_invoice_total DESC"

                    '.CommandText = "SELECT im_s_coy_name, im_currency_code, FORMAT(z.ForeignCurrencyAmt,2) 'foreign currency amt', " _
                    '            & "FORMAT(z.TotalAmount,2) 'total amount' FROM " _
                    '            & " (SELECT im_s_coy_name, im_currency_code,SUM(IF(im_currency_code <> 'MYR', " _
                    '            & " IFNULL(im_invoice_total,'0'),'0')) AS 'ForeignCurrencyAmt', " _
                    '            & " SUM(IF(im_currency_code <> 'MYR',IFNULL(im_invoice_total,0)*im_exchange_rate, " _
                    '            & " im_invoice_total)) AS 'TotalAmount' " _
                    '            & " FROM(invoice_mstr) WHERE im_invoice_status = 4 AND im_po_index is null and im_b_coy_id = '" & Session("CompanyID") & "' AND " _
                    '            & " im_payment_date BETWEEN '" & Format(CDate(Me.txtSDate.Text), "yyyy-MM-dd") & "' AND im_po_index is null and " _
                    '            & " '" & Format(CDate(Me.txtEDate.Text), "yyyy-MM-dd") & "' GROUP BY im_s_coy_name,im_currency_code ORDER BY im_currency_code ASC) z ORDER BY im_currency_code ASC, " _
                    '            & " z.TotalAmount DESC LIMIT 100"

                    .CommandText = "SELECT im_s_coy_name, im_currency_code, FORMAT(z.ForeignCurrencyAmt,2) 'foreign currency amt', " _
                                & "FORMAT(z.TotalAmount,2) 'total amount' FROM " _
                                & " (SELECT im_s_coy_name, im_currency_code,SUM(IF(im_currency_code <> 'MYR', IFNULL(im_invoice_total,0),0) - " _
                                & " IF(im_currency_code <> 'MYR', IF(im_invoice_type = 'CN',IFNULL(im_invoice_total*2,0),0),0)) AS 'ForeignCurrencyAmt', " _
                                & " (SUM(IF(im_currency_code <> 'MYR',IFNULL(im_invoice_total,0)*im_exchange_rate, " _
                                & " im_invoice_total))) - " _
                                & " (SUM(IF(im_invoice_type = 'CN',IF(im_currency_code <> 'MYR',(IFNULL(im_invoice_total,0)*im_exchange_rate)*2, " _
                                & " im_invoice_total*2),0))) AS 'TotalAmount' " _
                                & " FROM(invoice_mstr) WHERE im_invoice_status = 4 AND im_po_index is null and im_b_coy_id = '" & Session("CompanyID") & "' AND " _
                                & " cast(im_payment_date as date) BETWEEN '" & Format(CDate(Me.txtSDate.Text), "yyyy-MM-dd") & "' and" _
                                & " '" & Format(CDate(Me.txtEDate.Text), "yyyy-MM-dd") & "' AND im_po_index is null GROUP BY im_s_coy_name ) z ORDER BY " _
                                & " z.TotalAmount DESC LIMIT 100"
                End With

            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
            strFileName = "IPPA0016 - Payment To Top 100 Vendor" & "(" & Format(Now, "ddMMMyyyy") & ").xls"
            Dim attachment As String = "attachment;filename=" & strFileName
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"

            Dim dc As DataColumn
            Dim i As Integer

            i = 0
            For Each dc In ds.Tables(0).Columns
                If i > 0 Then
                    Response.Write(vbTab + dc.ColumnName)
                Else
                    Response.Write(dc.ColumnName)
                End If
                i += 1

            Next
            Response.Write(vbCrLf)

            Dim dr As DataRow
            For Each dr In ds.Tables(0).Rows
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    If i > 0 Then
                        Response.Write(vbTab + dr.Item(i).ToString)
                    Else
                        Response.Write(dr.Item(i).ToString)
                    End If
                Next
                Response.Write(vbCrLf)
            Next
            Response.End()

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try

    End Sub
    Private Sub ExportToPDF_Top100Vendor()
        Dim ds, dsTop100MYR, dsTop100Foreign, dsTotalMYR, dsTotalForeign As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strUserId As String
        Dim strCoyName As String
        Dim strDate As String = ""
        Dim strFileName As String = ""

        strDate = Format(CDate(txtSDate.Text), "yyyy-MM-dd")

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand

            'Top vendor
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                '.CommandText = "SELECT im_s_coy_name, im_currency_code,IF(im_currency_code <> 'MYR',CAST(ifnull(im_invoice_total,0) AS DECIMAL(20,2)),0) AS 'foreign currency amt', " _
                '                 & "IF(im_currency_code <> 'MYR',CAST(IFNULL(im_invoice_total,0)*im_exchange_rate AS DECIMAL(20,2)),im_invoice_total) AS 'total amount'" _
                '                 & "FROM(invoice_mstr) WHERE im_invoice_status = 4 AND im_b_coy_id = 'hlb' and im_payment_date between '" & Format(CDate(Me.txtSDate.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(Me.txtEDate.Text), "yyyy-MM-dd") & "' order by im_invoice_total desc"
                '.CommandText = "SELECT im_s_coy_name, im_currency_code,IF(im_currency_code <> 'MYR'," _
                '                          & "FORMAT(IFNULL(im_invoice_total,'0'),2),'0') AS 'foreign currency amt'," _
                '                          & "IF(im_currency_code <> 'MYR',FORMAT(IFNULL(im_invoice_total,0)*im_exchange_rate,2)," _
                '                          & "FORMAT(im_invoice_total,2)) AS 'total amount'" _
                '                          & "FROM(invoice_mstr) WHERE im_invoice_status = 4 AND im_b_coy_id = '" & Session("CompanyID") & "' AND " _
                '                          & "im_payment_date between '" & Format(CDate(Me.txtSDate.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(Me.txtEDate.Text), "yyyy-MM-dd") & "' ORDER BY im_currency_code ASC, im_invoice_total DESC"

                .CommandText = "SELECT im_s_coy_name, im_currency_code, FORMAT(z.ForeignCurrencyAmt,2) 'foreign currency amt', " _
                            & "FORMAT(z.TotalAmount,2) 'total amount' FROM " _
                            & " (SELECT im_s_coy_name, im_currency_code,SUM(IF(im_currency_code <> 'MYR', IFNULL(im_invoice_total,0),0) - " _
                            & " IF(im_currency_code <> 'MYR', IF(im_invoice_type = 'CN',IFNULL(im_invoice_total*2,0),0),0)) AS 'ForeignCurrencyAmt', " _
                            & " (SUM(IF(im_currency_code <> 'MYR',IFNULL(im_invoice_total,0)*im_exchange_rate, " _
                            & " im_invoice_total))) - " _
                            & " (SUM(IF(im_invoice_type = 'CN',IF(im_currency_code <> 'MYR',(IFNULL(im_invoice_total,0)*im_exchange_rate)*2, " _
                            & " im_invoice_total*2),0))) AS 'TotalAmount' " _
                            & " FROM(invoice_mstr) WHERE im_invoice_status = 4 AND im_po_index is null and im_b_coy_id = '" & Session("CompanyID") & "' AND " _
                            & " cast(im_payment_date as date) BETWEEN '" & Format(CDate(Me.txtSDate.Text), "yyyy-MM-dd") & "' and" _
                            & " '" & Format(CDate(Me.txtEDate.Text), "yyyy-MM-dd") & "' AND im_po_index is null GROUP BY im_s_coy_name ) z ORDER BY " _
                            & " z.TotalAmount DESC LIMIT 100"

            End With

            da = New MySqlDataAdapter(cmd)

            strUserId = Session("UserName")
            strCoyName = Session("CompanyName")

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("Top100Vendor_Top100Vendor", ds.Tables(0))

            'Top 100 MYR
            With cmd
                .CommandType = CommandType.Text
                '.CommandText = "SELECT  Format(SUM(t.im_invoice_total),2) AS 'im_invoice_total' FROM " _
                '            & "(SELECT SUM(im_invoice_total) AS 'im_invoice_total' FROM invoice_mstr " _
                '            & "WHERE im_invoice_status = 4 AND im_currency_code IS NOT NULL AND im_po_index is null and im_b_coy_id = '" & Session("CompanyID") & "' AND im_currency_code = 'myr' " _
                '            & " and im_payment_date between '" & Format(CDate(Me.txtSDate.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(Me.txtEDate.Text), "yyyy-MM-dd") & "' ORDER BY im_invoice_total desc LIMIT 100) AS t"
                '.CommandText = "SELECT  Format(SUM(t.im_invoice_total),2) AS 'im_invoice_total' FROM " _
                '            & "(SELECT SUM(IF(im_currency_code <> 'MYR',IFNULL(im_invoice_total,0)*im_exchange_rate,im_invoice_total)) AS 'im_invoice_total' FROM invoice_mstr " _
                '            & "WHERE im_invoice_status = 4 AND im_currency_code IS NOT NULL AND im_po_index is null and im_b_coy_id = '" & Session("CompanyID") & "' " _
                '            & " and im_payment_date between '" & Format(CDate(Me.txtSDate.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(Me.txtEDate.Text), "yyyy-MM-dd") & "' ORDER BY im_invoice_total desc LIMIT 100) AS t"
                .CommandText = "SELECT FORMAT(SUM(kkk.im_invoice_total),2) AS 'im_invoice_total' FROM( " _
                                & "SELECT im_s_coy_name, im_currency_code, FORMAT(z.ForeignCurrencyAmt,2) 'foreign currency amt', " _
                                & "z.TotalAmount 'im_invoice_total' FROM " _
                                & "(SELECT im_s_coy_name, im_currency_code,SUM(IF(im_currency_code <> 'MYR', IFNULL(im_invoice_total,0),0) - " _
                                & "IF(im_currency_code <> 'MYR', IF(im_invoice_type = 'CN',IFNULL(im_invoice_total*2,0),0),0)) AS 'ForeignCurrencyAmt', " _
                                & " (SUM(IF(im_currency_code <> 'MYR',IFNULL(im_invoice_total,0)*im_exchange_rate, " _
                                & " im_invoice_total))) - " _
                                & " (SUM(IF(im_invoice_type = 'CN',IF(im_currency_code <> 'MYR',(IFNULL(im_invoice_total,0)*im_exchange_rate)*2, " _
                                & " im_invoice_total*2),0))) AS 'TotalAmount' " _
                                & "FROM(invoice_mstr) WHERE im_invoice_status = 4 AND im_po_index IS NULL AND im_b_coy_id = '" & Session("CompanyID") & "' AND " _
                                & "cast(im_payment_date as date) BETWEEN '" & Format(CDate(Me.txtSDate.Text), "yyyy-MM-dd") & "' AND " _
                                & "'" & Format(CDate(Me.txtEDate.Text), "yyyy-MM-dd") & "' AND im_po_index IS NULL GROUP BY im_s_coy_name ) z ORDER BY " _
                                & "z.TotalAmount DESC LIMIT 100) kkk "
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(dsTop100MYR)
            Dim rptdatasource2 As New Microsoft.Reporting.WebForms.ReportDataSource("Top100Vendor_MYR_MYRSub", dsTop100MYR.Tables(0))

            'Total MYR
            With cmd
                .CommandType = CommandType.Text
                '.CommandText = "SELECT Format(SUM(im_invoice_total),2) AS 'im_invoice_total' FROM invoice_mstr " _
                '            & "WHERE im_invoice_status = 4 AND im_currency_code IS NOT NULL AND im_b_coy_id = '" & Session("CompanyID") & "' AND im_currency_code = 'myr' " _
                '            & "and im_payment_date between '" & Format(CDate(Me.txtSDate.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(Me.txtEDate.Text), "yyyy-MM-dd") & "' ORDER BY im_s_coy_name "
                .CommandText = "SELECT FORMAT(SUM(zzz.TotalAmount),2) AS 'im_invoice_total' FROM " _
                             & " (Select(SUM(IF(im_currency_code <> 'MYR',IFNULL(im_invoice_total,0)*im_exchange_rate, " _
                             & " im_invoice_total))) - " _
                             & " (SUM(IF(im_invoice_type = 'CN',IF(im_currency_code <> 'MYR',(IFNULL(im_invoice_total,0)*im_exchange_rate)*2, " _
                             & " im_invoice_total*2),0))) AS 'TotalAmount' " _
                             & " FROM invoice_mstr " _
                             & "WHERE im_invoice_status = 4 AND im_po_index is null and im_currency_code IS NOT NULL AND im_b_coy_id = '" & Session("CompanyID") & "' " _
                             & "AND cast(im_payment_date as date) BETWEEN '" & Format(CDate(Me.txtSDate.Text), "yyyy-MM-dd") & "' " _
                             & "AND '" & Format(CDate(Me.txtEDate.Text), "yyyy-MM-dd") & "' GROUP BY im_currency_code  " _
                             & "ORDER BY im_currency_code ) zzz "
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(dsTotalMYR)
            Dim rptdatasource3 As New Microsoft.Reporting.WebForms.ReportDataSource("Top100Vendor_MYRTotal_MYRTotal", dsTotalMYR.Tables(0))

            'top 100 foreign
            With cmd
                .CommandType = CommandType.Text
                '.CommandText = "SELECT DISTINCT(im_currency_code), cast(SUM(im_invoice_total) as decimal (20,2)) as 'im_invoice_total' FROM invoice_mstr " _
                '            & "WHERE im_invoice_status = 4 AND im_currency_code IS NOT NULL AND im_b_coy_id = 'hlb' AND im_currency_code <> 'myr' " _
                '            & "and im_payment_date between " & Me.txtSDate.Text & " and " & Me.txtEDate.Text & " GROUP BY im_currency_code ORDER BY im_invoice_total desc LIMIT 100"
                '.CommandText = "SELECT table1.im_currency_code, Format(SUM(table1.im_invoice_total),2) 'im_invoice_total' FROM " _
                '            & "(SELECT im_currency_code, im_invoice_total FROM invoice_mstr WHERE im_currency_code <> 'myr' AND im_currency_code <> '' and im_b_coy_id = '" & Session("CompanyID") & "' " _
                '            & "and im_payment_date between '" & Format(CDate(Me.txtSDate.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(Me.txtEDate.Text), "yyyy-MM-dd") & "' ORDER BY im_currency_code,im_invoice_total DESC) table1 " _
                '            & "WHERE " _
                '            & "(SELECT COUNT(*) FROM " _
                '            & "(SELECT im_currency_code, im_invoice_total FROM invoice_mstr WHERE im_currency_code <> 'myr' AND im_currency_code <> '' and im_b_coy_id = '" & Session("CompanyID") & "' " _
                '            & "and im_payment_date between '" & Format(CDate(Me.txtSDate.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(Me.txtEDate.Text), "yyyy-MM-dd") & "' ORDER BY im_currency_code,im_invoice_total DESC) table2 WHERE table1.im_currency_code = table2.im_currency_code " _
                '            & "AND table2.im_invoice_total >= table1.im_invoice_total) <= 100 GROUP BY table1.im_currency_code"

                .CommandText = "SELECT im_currency_code, TotalAmount FROM ( SELECT im_currency_code,  " _
                            & " (SUM(IF(im_currency_code <> 'MYR',IFNULL(im_invoice_total,0)*im_exchange_rate, " _
                            & " im_invoice_total))) - " _
                            & " (SUM(IF(im_invoice_type = 'CN',IF(im_currency_code <> 'MYR',(IFNULL(im_invoice_total,0)*im_exchange_rate)*2, " _
                            & " im_invoice_total*2),0))) AS 'TotalAmount' " _
                            & " FROM invoice_mstr WHERE im_invoice_status = 4 AND " _
                            & "im_b_coy_id = '" & Session("CompanyID") & "' AND IM_PO_INDEX IS NULL AND cast(im_payment_date as date) BETWEEN '" & Format(CDate(Me.txtSDate.Text), "yyyy-MM-dd") & "' " _
                            & "AND '" & Format(CDate(Me.txtEDate.Text), "yyyy-MM-dd") & "' GROUP BY im_s_coy_name,im_currency_code LIMIT 100) zzz WHERE zzz.im_currency_code <> 'MYR' ORDER BY zzz.im_currency_code desc "
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(dsTop100Foreign)
            Dim rptdatasource4 As New Microsoft.Reporting.WebForms.ReportDataSource("Top100Vendor_TopFrgn_TopForeign", dsTop100Foreign.Tables(0))

            'total Foreign
            With cmd
                .CommandType = CommandType.Text
                '.CommandText = "SELECT DISTINCT(im_currency_code), Format(SUM(im_invoice_total),2) as 'im_invoice_total', Format(SUM(im_invoice_total * im_exchange_rate),2) as 'im_invoice_total' FROM invoice_mstr " _
                '            & "WHERE im_invoice_status = 4 AND im_currency_code IS NOT NULL AND im_b_coy_id = '" & Session("CompanyID") & "' " _
                '            & "and im_payment_date between '" & Format(CDate(Me.txtSDate.Text), "yyyy-MM-dd") & "' and '" & Format(CDate(Me.txtEDate.Text), "yyyy-MM-dd") & "' GROUP BY im_currency_code ORDER BY im_currency_code "

                '.CommandText = "SELECT DISTINCT(im_currency_code), IF(im_currency_code = 'MYR','-',FORMAT(SUM(im_invoice_total),2)) AS 'im_invoice_total', " _
                '            & "IF(im_currency_code = 'MYR',FORMAT(SUM(im_invoice_total),2),FORMAT(SUM(im_invoice_total * im_exchange_rate),2)) AS 'im_myr_total' FROM invoice_mstr " _
                '            & "WHERE im_invoice_status = 4 AND im_po_index is null and im_currency_code IS NOT NULL AND im_b_coy_id = 'hlb' " _
                '            & "AND im_payment_date BETWEEN '" & Format(CDate(Me.txtSDate.Text), "yyyy-MM-dd") & "' " _
                '            & "AND '" & Format(CDate(Me.txtEDate.Text), "yyyy-MM-dd") & "' GROUP BY im_currency_code " _
                '            & "ORDER BY im_currency_code"

                .CommandText = "SELECT im_currency_code, IF(im_currency_code = 'MYR',FORMAT(SUM(t.im_invoice_total),2),FORMAT(SUM(t.foreign_currency_amt),2)) 'im_invoice_total' " _
                                & ",FORMAT(SUM(t.im_invoice_total),2) 'im_myr_total' FROM ( " _
                                & "SELECT im_s_coy_name, im_currency_code, z.ForeignCurrencyAmt 'foreign_currency_amt', " _
                                & "z.TotalAmount 'im_invoice_total' FROM " _
                                & "(SELECT im_s_coy_name, im_currency_code,SUM(IF(im_currency_code <> 'MYR', " _
                                & "IF(im_invoice_type <> 'CN',IFNULL(im_invoice_total,'0'),'0') - IF(im_invoice_type = 'CN',IFNULL(im_invoice_total,'0'),'0'),'0')) AS 'ForeignCurrencyAmt', " _
                                & "(SUM(IF(im_currency_code <> 'MYR',IFNULL(im_invoice_total,0)*im_exchange_rate, " _
                                & "im_invoice_total))) - " _
                                & "(SUM(IF(im_invoice_type = 'CN',IF(im_currency_code <> 'MYR',(IFNULL(im_invoice_total,0)*im_exchange_rate)*2, " _
                                & "im_invoice_total*2),0))) AS 'TotalAmount' " _
                                & "FROM invoice_mstr WHERE im_invoice_status = 4 AND im_po_index IS NULL AND im_b_coy_id = '" & Session("CompanyID") & "' AND " _
                                & "cast(im_payment_date as date) BETWEEN '" & Format(CDate(Me.txtSDate.Text), "yyyy-MM-dd") & "' AND " _
                                & "'" & Format(CDate(Me.txtEDate.Text), "yyyy-MM-dd") & "' AND im_po_index IS NULL GROUP BY im_s_coy_name ) z ORDER BY " _
                                & "z.TotalAmount DESC) T GROUP BY im_currency_code "


            End With
            REM im_myr_total
            da = New MySqlDataAdapter(cmd)
            da.Fill(dsTotalForeign)
            Dim rptdatasource5 As New Microsoft.Reporting.WebForms.ReportDataSource("Top100Vendor_TotalFrgn_totalForeign", dsTotalForeign.Tables(0))

            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.DataSources.Add(rptdatasource2)
            localreport.DataSources.Add(rptdatasource3)
            localreport.DataSources.Add(rptdatasource4)
            localreport.DataSources.Add(rptdatasource5)
            localreport.ReportPath = dispatcher.direct("Report", "Top100Vendor.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")

            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            If TotalParameter > 0 Then
                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
                For I = 0 To localreport.GetParameters.Count - 1
                    GetParameter = localreport.GetParameters.Item(I).Name
                    Select Case LCase(GetParameter)
                        Case "startdt"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, Format(CDate(txtSDate.Text), "dd/MM/yyyy"))
                        Case "enddt"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, Format(CDate(txtEDate.Text), "dd/MM/yyyy"))
                        Case "monthyear"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, MonthName(CInt(txtSDate.Text.ToString.Substring(3, 2)), True) & " " & txtSDate.Text.ToString.Substring(6, 4))
                        Case "prmbuyercoyname"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)
                    End Select
                Next
                localreport.SetParameters(par)
            End If

            localreport.Refresh()

            Dim deviceInfo As String =
                "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            strFileName = "Top 100 Vendor.pdf"
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()


        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub

    Private Sub ExportToPDF_DebitNote()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strUserId As String
        Dim strCoyName As String
        Dim strDate As String = ""
        Dim strFileName As String = ""
        Dim dtDN As New DataTable
        Dim dsDN As New DataSet
        Dim decBankCharges As Decimal
        Dim objDb As New EAD.DBCom
        Dim strPaymetTerm As String = ""
        Dim strVenID As String = ""
        Dim i As Integer
        Dim strBRCoyName As String = ""
        Dim strVendorID As String = ""
        'strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            'CH - Issue 8317 - 9 Mar 2015
            Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text

                .CommandText = "SELECT ID_DN_NO as DN_No, im_s_coy_id,id_product_desc, im_invoice_type, FORMAT(IF(im_payment_term = 'TT',(ID_RECEIVED_QTY * ID_UNIT_COST),(ID_RECEIVED_QTY * ID_UNIT_COST)) ,2) AS Amount, " _
                       & "FORMAT(IF(im_payment_term = 'TT',(ID_RECEIVED_QTY * ID_UNIT_COST * im_exchange_rate),(ID_RECEIVED_QTY * ID_UNIT_COST )) ,2) AS AmountWithExg, " _
                       & "im_currency_code,ic_coy_name,ic_addr_line1, ic_addr_line2,ic_addr_line3,ic_postcode,ic_city, (SELECT CODE_DESC FROM code_mstr WHERE code_abbr = ic_state AND code_value = ic_country AND code_category = 'S' AND code_deleted = 'N') as ic_state ,(SELECT CODE_DESC FROM code_mstr WHERE code_abbr = ic_country AND code_category = 'CT' AND code_deleted = 'N') AS ic_country,ic_phone,ic_fax, " _
                       & "cm_coy_name, cm_addr_line1, cm_addr_line2, cm_addr_line3, cm_postcode, cm_city,  (SELECT CODE_DESC FROM code_mstr WHERE code_abbr = cm_state AND code_value = cm_country AND code_category = 'S' AND code_deleted = 'N') as cm_state, (SELECT CODE_DESC FROM code_mstr WHERE code_abbr = cm_country AND code_category = 'CT' AND code_deleted = 'N') as cm_country, cm_phone, cm_fax, IM_PAYMENT_NO,im_invoice_no, im_s_coy_name,im_created_by,im_exchange_rate,id_dr_exchange_rate,id_dr_currency,im_payment_term,im_payment_date  " _
                       & "FROM INVOICE_MSTR " _
                       & "LEFT JOIN invoice_details ON id_invoice_no = im_invoice_no AND id_s_coy_id = im_s_coy_id " _
                       & "INNER JOIN ipp_company ON ic_other_b_coy_code = id_pay_for AND ic_coy_id = '" & strDefIPPCompID & "' " _
                       & "INNER JOIN company_mstr ON im_b_coy_id = cm_coy_id " _
                       & "WHERE im_invoice_status = 4 AND im_invoice_type IS NOT NULL AND im_b_coy_id = '" & Session("CompanyId") & "' " _
                       & "AND im_payment_date IS NOT NULL and id_dn_no = '" & txtDebitNoteNo.Text & "' ORDER BY ic_coy_name,im_s_coy_id "


            End With

            da = New MySqlDataAdapter(cmd)

            ' da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyID")))
            strUserId = Session("UserName") 'Session("UserId")
            strCoyName = Session("CompanyName")

            da.Fill(ds)

            '----------------------------------------------------------

            dtDN.Columns.Add("DNNo", Type.GetType("System.String"))
            dtDN.Columns.Add("im_s_coy_id", Type.GetType("System.String"))
            dtDN.Columns.Add("id_product_desc", Type.GetType("System.String"))
            dtDN.Columns.Add("im_invoice_type", Type.GetType("System.String"))
            dtDN.Columns.Add("im_currency_code", Type.GetType("System.String"))
            dtDN.Columns.Add("Amount", Type.GetType("System.Decimal"))
            dtDN.Columns.Add("AmountWithExg", Type.GetType("System.Decimal"))
            dtDN.Columns.Add("cm_coy_name", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_addr_line1", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_addr_line2", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_addr_line3", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_postcode", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_city", Type.GetType("System.String"))
            dtDN.Columns.Add("code_desc2", Type.GetType("System.String"))
            dtDN.Columns.Add("code_desc3", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_phone", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_fax", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_coy_name", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_addr_line1", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_addr_line2", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_addr_line3", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_postcode", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_city", Type.GetType("System.String"))
            dtDN.Columns.Add("code_desc", Type.GetType("System.String"))
            dtDN.Columns.Add("code_desc1", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_phone", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_fax", Type.GetType("System.String"))
            dtDN.Columns.Add("im_s_coy_name", Type.GetType("System.String"))
            dtDN.Columns.Add("im_invoice_no", Type.GetType("System.String"))
            dtDN.Columns.Add("im_payment_no", Type.GetType("System.String"))
            dtDN.Columns.Add("im_created_by", Type.GetType("System.String"))
            dtDN.Columns.Add("im_exchange_rate", Type.GetType("System.String"))
            dtDN.Columns.Add("id_dr_exchange_rate", Type.GetType("System.String"))
            dtDN.Columns.Add("id_dr_currency", Type.GetType("System.String"))
            dtDN.Columns.Add("im_payment_term", Type.GetType("System.String"))
            dtDN.Columns.Add("im_payment_date", Type.GetType("System.String"))
            For i = 0 To ds.Tables(0).Rows.Count - 1

                Dim dtr As DataRow
                dtr = dtDN.NewRow()
                If ds.Tables(0).Rows(i)("im_invoice_type") = "CN" Then
                    ds.Tables(0).Rows(i)("Amount") = "-" & ds.Tables(0).Rows(i)("Amount")
                    ds.Tables(0).Rows(i)("AmountWithExg") = "-" & ds.Tables(0).Rows(i)("AmountWithExg")
                End If

                'strICState = GetVal("SELECT code_desc FROM code_mstr WHERE code_abbr = '" & ds.Tables(0).Rows(i)("ic_state") & "' AND code_category = 's'")
                'strCMState = GetVal("SELECT code_desc FROM code_mstr WHERE code_abbr = '" & ds.Tables(0).Rows(i)("cm_state") & "' AND code_category = 's'")

                dtr("DNNo") = ds.Tables(0).Rows(i)("DN_NO") 'strDNNo
                dtr("im_s_coy_id") = ds.Tables(0).Rows(i)("im_s_coy_id")
                dtr("id_product_desc") = ds.Tables(0).Rows(i)("id_product_desc")
                dtr("im_invoice_type") = ds.Tables(0).Rows(i)("im_invoice_type")
                dtr("im_currency_code") = ds.Tables(0).Rows(i)("im_currency_code")
                dtr("Amount") = ds.Tables(0).Rows(i)("Amount")
                dtr("AmountWithExg") = ds.Tables(0).Rows(i)("AmountWithExg")
                dtr("cm_coy_name") = ds.Tables(0).Rows(i)("cm_coy_name")
                dtr("cm_addr_line1") = ds.Tables(0).Rows(i)("cm_addr_line1")
                dtr("cm_addr_line2") = ds.Tables(0).Rows(i)("cm_addr_line2")
                dtr("cm_addr_line3") = ds.Tables(0).Rows(i)("cm_addr_line3")
                dtr("cm_postcode") = ds.Tables(0).Rows(i)("cm_postcode")
                dtr("cm_city") = ds.Tables(0).Rows(i)("cm_city")
                dtr("code_desc2") = ds.Tables(0).Rows(i)("cm_state")
                dtr("code_desc3") = ds.Tables(0).Rows(i)("cm_country")
                dtr("cm_phone") = ds.Tables(0).Rows(i)("cm_phone")
                dtr("cm_fax") = ds.Tables(0).Rows(i)("cm_fax")
                dtr("ic_coy_name") = ds.Tables(0).Rows(i)("ic_coy_name")
                dtr("ic_addr_line1") = ds.Tables(0).Rows(i)("ic_addr_line1")
                dtr("ic_addr_line2") = ds.Tables(0).Rows(i)("ic_addr_line2")
                dtr("ic_addr_line3") = ds.Tables(0).Rows(i)("ic_addr_line3")
                dtr("ic_postcode") = ds.Tables(0).Rows(i)("ic_postcode")
                dtr("ic_city") = ds.Tables(0).Rows(i)("ic_city")
                dtr("code_desc") = ds.Tables(0).Rows(i)("ic_state")
                dtr("code_desc1") = ds.Tables(0).Rows(i)("ic_country")
                dtr("ic_phone") = ds.Tables(0).Rows(i)("ic_phone")
                dtr("ic_fax") = ds.Tables(0).Rows(i)("ic_fax")
                dtr("im_s_coy_name") = ds.Tables(0).Rows(i)("im_s_coy_name")
                dtr("im_invoice_no") = ds.Tables(0).Rows(i)("im_invoice_no")
                dtr("im_payment_no") = ds.Tables(0).Rows(i)("im_payment_no")
                dtr("im_created_by") = ds.Tables(0).Rows(i)("im_created_by")
                dtr("im_exchange_rate") = ds.Tables(0).Rows(i)("im_exchange_rate")
                dtr("id_dr_exchange_rate") = ds.Tables(0).Rows(i)("id_dr_exchange_rate")
                dtr("id_dr_currency") = ds.Tables(0).Rows(i)("id_dr_currency")
                dtr("im_payment_term") = ds.Tables(0).Rows(i)("im_payment_term")
                dtr("im_payment_date") = Format(ds.Tables(0).Rows(i)("im_payment_date"), "dd-MMM-yyyy")
                dtDN.Rows.Add(dtr)

                strBRCoyName = ds.Tables(0).Rows(i)("ic_coy_name")
                strVendorID = ds.Tables(0).Rows(i)("im_s_coy_id")

            Next


            dsDN.Tables.Add(dtDN)
            '----------------------------------------------------------
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("DebitNote_DebitNote", dsDN.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)

            localreport.ReportPath = dispatcher.direct("Report", "DebitAdvice.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")

            localreport.EnableExternalImages = True

            'Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            'IPP Gst Stage 2A - CH - 5 Mar 2015
            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            If TotalParameter > 0 Then
                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
                For i = 0 To localreport.GetParameters.Count - 1
                    GetParameter = localreport.GetParameters.Item(i).Name
                    Select Case LCase(GetParameter)
                        Case "prmbuyercoyname"
                            par(i) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)
                    End Select
                Next
                localreport.SetParameters(par)
            End If
            '------------------------------------

            localreport.Refresh()

            Dim deviceInfo As String =
                "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            strFileName = "Debit Advice.pdf"
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()


        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub

    Private Sub ExportToPDF_DebitAdvice()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strUserId As String
        Dim strCoyName As String
        Dim strDate As String = ""
        Dim strFileName As String = ""
        Dim dtDN As New DataTable
        Dim dsDN As New DataSet
        Dim decBankCharges As Decimal
        Dim objDb As New EAD.DBCom
        Dim strPaymetTerm As String = ""
        Dim strVenID As String = ""
        Dim i As Integer
        Dim strBRCoyName As String = ""
        Dim strVendorID As String = ""
        'strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try

            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"

            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            'CH - Issue 8317 - 9 Mar 2015
            Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")
            If strDefIPPCompID = "" Then
                strDefIPPCompID = HttpContext.Current.Session("CompanyID")
            End If

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text

                .CommandText = "SELECT ID_DN_NO as DN_No, im_s_coy_id,id_product_desc, im_invoice_type, FORMAT(IF(im_payment_term = 'TT',(ID_RECEIVED_QTY * ID_UNIT_COST),(ID_RECEIVED_QTY * ID_UNIT_COST)) ,2) AS Amount, " _
                         & "FORMAT(IF(im_payment_term = 'TT',(ID_RECEIVED_QTY * ID_UNIT_COST * im_exchange_rate),(ID_RECEIVED_QTY * ID_UNIT_COST )) ,2) AS AmountWithExg, " _
                         & "im_currency_code,ic_coy_name,ic_addr_line1, ic_addr_line2,ic_addr_line3,ic_postcode,ic_city, (SELECT CODE_DESC FROM code_mstr WHERE code_abbr = ic_state AND code_value = ic_country AND code_category = 'S' AND code_deleted = 'N') as ic_state ,(SELECT CODE_DESC FROM code_mstr WHERE code_abbr = ic_country AND code_category = 'CT' AND code_deleted = 'N') AS ic_country,ic_phone,ic_fax, " _
                         & "cm_coy_name, cm_addr_line1, cm_addr_line2, cm_addr_line3, cm_postcode, cm_city,  (SELECT CODE_DESC FROM code_mstr WHERE code_abbr = cm_state AND code_value = cm_country AND code_category = 'S' AND code_deleted = 'N') as cm_state, (SELECT CODE_DESC FROM code_mstr WHERE code_abbr = cm_country AND code_category = 'CT' AND code_deleted = 'N') as cm_country, cm_phone, cm_fax, IM_PAYMENT_NO,im_invoice_no, im_s_coy_name,im_created_by,im_exchange_rate,id_dr_exchange_rate,id_dr_currency,im_payment_term,  " _
                         & "'DEBIT ADVICE' AS 'header', " _
                         & "IF(im_payment_term = 'TT', id_gst_value * im_exchange_rate, " _
                         & "IF(id_dr_exchange_rate IS NULL OR id_dr_exchange_rate = 0, id_gst_value, id_gst_value * id_dr_exchange_rate)) AS 'id_gst_value', " _
                         & "IF(im_payment_term = 'TT', (id_gst_value * im_exchange_rate) + (ID_RECEIVED_QTY * ID_UNIT_COST * im_exchange_rate), " _
                         & "IF(id_dr_exchange_rate IS NULL OR id_dr_exchange_rate = 0, id_gst_value + (ID_RECEIVED_QTY * ID_UNIT_COST), (id_gst_value * id_dr_exchange_rate) + (ID_RECEIVED_QTY * ID_UNIT_COST * id_dr_exchange_rate))) AS 'totalwithGST' " _
                         & "FROM INVOICE_MSTR " _
                         & "LEFT JOIN invoice_details ON id_invoice_no = im_invoice_no AND id_s_coy_id = im_s_coy_id " _
                         & "INNER JOIN ipp_company ON ic_other_b_coy_code = id_pay_for AND ic_coy_id = '" & strDefIPPCompID & "' " _
                         & "INNER JOIN company_mstr ON im_b_coy_id = cm_coy_id " _
                         & "WHERE im_invoice_status = 4 AND id_doc_type_generated = 'DA' AND im_invoice_type IS NOT NULL AND im_b_coy_id = '" & Session("CompanyId") & "'  AND im_payment_date IS NOT NULL AND id_dn_no = '" & txtDebitAdviceNo.Text & "' ORDER BY ic_coy_name,im_s_coy_id "

            End With

            da = New MySqlDataAdapter(cmd)

            ' da.SelectCommand.Parameters.Add(New MySqlParameter("@prmCoyID", Session("CompanyID")))
            strUserId = Session("UserName") 'Session("UserId")
            strCoyName = Session("CompanyName")

            da.Fill(ds)

            '----------------------------------------------------------

            dtDN.Columns.Add("DNNo", Type.GetType("System.String"))
            dtDN.Columns.Add("im_s_coy_id", Type.GetType("System.String"))
            dtDN.Columns.Add("id_product_desc", Type.GetType("System.String"))
            dtDN.Columns.Add("im_invoice_type", Type.GetType("System.String"))
            dtDN.Columns.Add("im_currency_code", Type.GetType("System.String"))
            dtDN.Columns.Add("Amount", Type.GetType("System.Decimal"))
            dtDN.Columns.Add("AmountWithExg", Type.GetType("System.Decimal"))
            dtDN.Columns.Add("cm_coy_name", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_addr_line1", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_addr_line2", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_addr_line3", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_postcode", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_city", Type.GetType("System.String"))
            dtDN.Columns.Add("code_desc2", Type.GetType("System.String"))
            dtDN.Columns.Add("code_desc3", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_phone", Type.GetType("System.String"))
            dtDN.Columns.Add("cm_fax", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_coy_name", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_addr_line1", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_addr_line2", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_addr_line3", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_postcode", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_city", Type.GetType("System.String"))
            dtDN.Columns.Add("code_desc", Type.GetType("System.String"))
            dtDN.Columns.Add("code_desc1", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_phone", Type.GetType("System.String"))
            dtDN.Columns.Add("ic_fax", Type.GetType("System.String"))
            dtDN.Columns.Add("im_s_coy_name", Type.GetType("System.String"))
            dtDN.Columns.Add("im_invoice_no", Type.GetType("System.String"))
            dtDN.Columns.Add("im_payment_no", Type.GetType("System.String"))
            dtDN.Columns.Add("im_created_by", Type.GetType("System.String"))
            dtDN.Columns.Add("im_exchange_rate", Type.GetType("System.String"))
            dtDN.Columns.Add("id_dr_exchange_rate", Type.GetType("System.String"))
            dtDN.Columns.Add("id_dr_currency", Type.GetType("System.String"))
            dtDN.Columns.Add("im_payment_term", Type.GetType("System.String"))
            dtDN.Columns.Add("header", Type.GetType("System.String"))
            dtDN.Columns.Add("id_gst_value", Type.GetType("System.Decimal"))
            dtDN.Columns.Add("totalwithGST", Type.GetType("System.Decimal"))

            For i = 0 To ds.Tables(0).Rows.Count - 1

                Dim dtr As DataRow
                dtr = dtDN.NewRow()
                'If ds.Tables(0).Rows(i)("im_invoice_type") = "CN" Then
                '    ds.Tables(0).Rows(i)("Amount") = "-" & ds.Tables(0).Rows(i)("Amount")
                '    ds.Tables(0).Rows(i)("AmountWithExg") = "-" & ds.Tables(0).Rows(i)("AmountWithExg")
                'End If

                'strICState = GetVal("SELECT code_desc FROM code_mstr WHERE code_abbr = '" & ds.Tables(0).Rows(i)("ic_state") & "' AND code_category = 's'")
                'strCMState = GetVal("SELECT code_desc FROM code_mstr WHERE code_abbr = '" & ds.Tables(0).Rows(i)("cm_state") & "' AND code_category = 's'")

                dtr("DNNo") = ds.Tables(0).Rows(i)("DN_NO") 'strDNNo
                dtr("im_s_coy_id") = ds.Tables(0).Rows(i)("im_s_coy_id")
                dtr("id_product_desc") = ds.Tables(0).Rows(i)("id_product_desc")
                dtr("im_invoice_type") = ds.Tables(0).Rows(i)("im_invoice_type")
                dtr("im_currency_code") = ds.Tables(0).Rows(i)("im_currency_code")
                dtr("Amount") = ds.Tables(0).Rows(i)("Amount")
                dtr("AmountWithExg") = ds.Tables(0).Rows(i)("AmountWithExg")
                dtr("cm_coy_name") = ds.Tables(0).Rows(i)("cm_coy_name")
                dtr("cm_addr_line1") = ds.Tables(0).Rows(i)("cm_addr_line1")
                dtr("cm_addr_line2") = ds.Tables(0).Rows(i)("cm_addr_line2")
                dtr("cm_addr_line3") = ds.Tables(0).Rows(i)("cm_addr_line3")
                dtr("cm_postcode") = ds.Tables(0).Rows(i)("cm_postcode")
                dtr("cm_city") = ds.Tables(0).Rows(i)("cm_city")
                dtr("code_desc2") = ds.Tables(0).Rows(i)("cm_state")
                dtr("code_desc3") = ds.Tables(0).Rows(i)("cm_country")
                dtr("cm_phone") = ds.Tables(0).Rows(i)("cm_phone")
                dtr("cm_fax") = ds.Tables(0).Rows(i)("cm_fax")
                dtr("ic_coy_name") = ds.Tables(0).Rows(i)("ic_coy_name")
                dtr("ic_addr_line1") = ds.Tables(0).Rows(i)("ic_addr_line1")
                dtr("ic_addr_line2") = ds.Tables(0).Rows(i)("ic_addr_line2")
                dtr("ic_addr_line3") = ds.Tables(0).Rows(i)("ic_addr_line3")
                dtr("ic_postcode") = ds.Tables(0).Rows(i)("ic_postcode")
                dtr("ic_city") = ds.Tables(0).Rows(i)("ic_city")
                dtr("code_desc") = ds.Tables(0).Rows(i)("ic_state")
                dtr("code_desc1") = ds.Tables(0).Rows(i)("ic_country")
                dtr("ic_phone") = ds.Tables(0).Rows(i)("ic_phone")
                dtr("ic_fax") = ds.Tables(0).Rows(i)("ic_fax")
                dtr("im_s_coy_name") = ds.Tables(0).Rows(i)("im_s_coy_name")
                dtr("im_invoice_no") = ds.Tables(0).Rows(i)("im_invoice_no")
                dtr("im_payment_no") = ds.Tables(0).Rows(i)("im_payment_no")
                dtr("im_created_by") = ds.Tables(0).Rows(i)("im_created_by")
                dtr("im_exchange_rate") = ds.Tables(0).Rows(i)("im_exchange_rate")
                dtr("id_dr_exchange_rate") = ds.Tables(0).Rows(i)("id_dr_exchange_rate")
                dtr("id_dr_currency") = ds.Tables(0).Rows(i)("id_dr_currency")
                dtr("im_payment_term") = ds.Tables(0).Rows(i)("im_payment_term")
                dtr("header") = ds.Tables(0).Rows(i)("header")
                If Not ds.Tables(0).Rows(i)("id_gst_value") Is DBNull.Value Then
                    dtr("id_gst_value") = ds.Tables(0).Rows(i)("id_gst_value")
                Else
                    dtr("id_gst_value") = 0.0
                End If
                If Not ds.Tables(0).Rows(i)("totalwithGST") Is DBNull.Value Then
                    dtr("totalwithGST") = ds.Tables(0).Rows(i)("totalwithGST")
                Else
                    dtr("totalwithGST") = 0.0
                End If
                dtDN.Rows.Add(dtr)

                strBRCoyName = ds.Tables(0).Rows(i)("ic_coy_name")
                strVendorID = ds.Tables(0).Rows(i)("im_s_coy_id")

            Next


            dsDN.Tables.Add(dtDN)
            '----------------------------------------------------------
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("dsDebitNote_DataTable1", dsDN.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)

            localreport.ReportPath = dispatcher.direct("Report", "DebitAdvice.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")

            localreport.EnableExternalImages = True

            'Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            'IPP Gst Stage 2A - CH - 5 Mar 2015
            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            If TotalParameter > 0 Then
                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
                For i = 0 To localreport.GetParameters.Count - 1
                    GetParameter = localreport.GetParameters.Item(i).Name
                    Select Case LCase(GetParameter)
                        Case "prmbuyercoyname"
                            par(i) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)
                    End Select
                Next
                localreport.SetParameters(par)
            End If
            '------------------------------------

            localreport.Refresh()

            Dim deviceInfo As String =
                "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            strFileName = "Debit Advice.pdf"
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()


        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub

    'Jules 2015.01.30 IPP Stage 2A
    Private Sub ExportToExcel_BillPendingAppr()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        'Dim strDate As String = ""
        Dim strFileName As String = ""


        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text

                'Mod by Jules 2015-Feb-03 for IPP GST Stage 2A
                .CommandText = "SELECT bm.BM_INVOICE_NO AS 'Document No.', bm.BM_DOC_DATE AS 'Document Date', bm.BM_CREATED_BY AS 'Teller ID', bm.BM_S_COY_NAME AS 'Customer', " &
                                "FORMAT(SUM(IF(bm.BM_CURRENCY_CODE='MYR',IFNULL(bd.BM_GST_VALUE,0),IFNULL(bd.BM_GST_VALUE,0) * IFNULL(bm.BM_EXCHANGE_RATE,1))),2) AS 'GST Amount', " &
                                "FORMAT(SUM(IF(bm.BM_CURRENCY_CODE='MYR',bd.BM_UNIT_COST*bd.BM_RECEIVED_QTY+IFNULL(bd.BM_GST_VALUE,0), " &
                                "(bd.BM_UNIT_COST*bd.BM_RECEIVED_QTY+IFNULL(bd.BM_GST_VALUE,0)) *IFNULL(bm.BM_EXCHANGE_RATE,1))),2) AS 'Invoice Amount (MYR)', " &
                                "FORMAT(SUM(IF(bm.BM_CURRENCY_CODE='MYR','', (bd.BM_UNIT_COST*bd.BM_RECEIVED_QTY+IFNULL(bd.BM_GST_VALUE,0)))),2) AS 'Invoice Amount (FCY)' "

                If ViewState("AOF") = True Then
                    .CommandText = .CommandText & "FROM BILLING_APPROVAL " &
                                 "INNER JOIN BILLING_MSTR bm ON BA_BILL_INDEX = BM_INVOICE_INDEX " &
                                 "INNER JOIN billing_details bd ON bm.BM_INVOICE_NO = bd.BM_INVOICE_NO AND bm.BM_S_COY_ID = bd.BM_S_COY_ID " &
                                 "WHERE(BA_SEQ - 1 = BA_AO_ACTION) And BM_INVOICE_STATUS = 2 "
                    If ViewState("AO") <> "" Then
                        .CommandText = .CommandText & "AND (BA_AO = '" & ViewState("AO") & "' OR (BA_A_AO = '" & ViewState("AO") & "' AND BA_RELIEF_IND = 'O')) AND BM_B_COY_ID = '" & Session("CompanyID") & "' "
                    End If
                Else
                    .CommandText = .CommandText & "FROM BILLING_APPROVAL " &
                                    "INNER JOIN BILLING_MSTR bm ON BA_BILL_INDEX = BM_INVOICE_INDEX " &
                                    "INNER JOIN billing_details bd ON bm.BM_INVOICE_NO = bd.BM_INVOICE_NO AND bm.BM_S_COY_ID = bd.BM_S_COY_ID " &
                                    "WHERE(BA_SEQ - 1 = BA_AO_ACTION) And BM_INVOICE_STATUS = 2 "

                    If ViewState("AO") <> "" Then
                        .CommandText = .CommandText & "AND (BA_AO = '" & ViewState("AO") & "' OR (BA_A_AO = '" & ViewState("AO") & "' AND BA_RELIEF_IND = 'O')) AND BM_B_COY_ID = '" & Session("CompanyID") & "' "
                    Else
                        .CommandText = .CommandText & "AND (BA_AO IN (SELECT um_user_id FROM user_mstr " &
                                    "INNER JOIN users_usrgrp ON UU_USER_ID=UM_USER_ID AND UU_COY_ID=UM_COY_ID " &
                                    "INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID AND UGM_DELETED='N' " &
                                    "INNER JOIN company_dept_mstr ON cdm_dept_code = um_dept_id " &
                                    "WHERE UGM_FIXED_ROLE = 'Billing Approving Officer' " &
                                    "AND cdm_dept_code= (SELECT um_dept_id FROM user_mstr WHERE um_user_id='" & Session("UserID") & "' AND um_coy_id='" & Session("CompanyID") & "')) " &
                                    "OR (BA_A_AO IN (SELECT um_user_id FROM user_mstr " &
                                    "INNER JOIN users_usrgrp ON UU_USER_ID=UM_USER_ID AND UU_COY_ID=UM_COY_ID " &
                                    "INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID AND UGM_DELETED='N' " &
                                    "INNER JOIN company_dept_mstr ON cdm_dept_code = um_dept_id " &
                                    "WHERE UGM_FIXED_ROLE = 'Billing Approving Officer' " &
                                    "AND cdm_dept_code= (SELECT um_dept_id FROM user_mstr WHERE um_user_id='" & Session("UserID") & "' AND um_coy_id='" & Session("CompanyID") & "')) " &
                                    "AND BA_RELIEF_IND = 'O')) AND BM_B_COY_ID = '" & Session("CompanyID") & "' "
                    End If
                End If
                .CommandText = .CommandText & "GROUP BY bm.BM_INVOICE_NO,bm.BM_B_COY_ID,bm.BM_S_COY_ID "
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
            strFileName = "IPPD-024 - BillingPendingApprovalReport" & "(" & Format(Now, "ddMMMyyyy") & ").xls"
            Dim attachment As String = "attachment;filename=" & strFileName
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"

            Dim dc As DataColumn
            Dim i, j As Integer

            Response.Write("No.")
            For Each dc In ds.Tables(0).Columns
                Response.Write(vbTab + dc.ColumnName)
            Next
            Response.Write(vbCrLf)

            j = 1
            Dim dr As DataRow
            For Each dr In ds.Tables(0).Rows
                Response.Write(j.ToString)
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    If i > 0 Then
                        Response.Write(vbTab + dr.Item(i).ToString)
                    Else
                        Response.Write(vbTab + dr.Item(i).ToString)
                    End If
                Next
                Response.Write(vbCrLf)
                j = j + 1
            Next
            Response.End()

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try

    End Sub

    'Jules 2015.01.30 IPP Stage 2A
    Private Sub ExportToPDF_BillPendingAppr()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strUserId As String
        Dim strCoyName As String
        'Dim strDate As String = ""
        Dim strFileName As String = ""

        'strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text

                'Mod by Jules 2015-Feb-03 for IPP GST Stage 2A
                .CommandText = "SELECT bm.BM_INVOICE_NO, DATE_FORMAT(bm.BM_DOC_DATE,'%d/%m/%Y') AS BM_DOC_DATE, bm.BM_CREATED_BY, bm.BM_S_COY_NAME, bm.BM_CURRENCY_CODE, " &
                                "IF(bm.BM_CURRENCY_CODE='MYR',FORMAT(SUM(IFNULL(bd.BM_GST_VALUE,0)),2),FORMAT(SUM(IFNULL(bd.BM_GST_VALUE,0) * IFNULL(bm.BM_EXCHANGE_RATE,1)),2)) AS GSTAmt, " &
                                "IF(bm.BM_CURRENCY_CODE='MYR',FORMAT(SUM(bd.BM_UNIT_COST*bd.BM_RECEIVED_QTY+IFNULL(bd.BM_GST_VALUE,0)),2), " &
                                "FORMAT(SUM((bd.BM_UNIT_COST*bd.BM_RECEIVED_QTY+IFNULL(bd.BM_GST_VALUE,0)) * IFNULL(bm.BM_EXCHANGE_RATE,1)),2)) AS InvAmtMYR, " &
                                "IF(bm.BM_CURRENCY_CODE='MYR','', FORMAT(SUM((bd.BM_UNIT_COST*bd.BM_RECEIVED_QTY+IFNULL(bd.BM_GST_VALUE,0))),2)) AS InvAmtFCY "
                If ViewState("AOF") = True Then
                    .CommandText = .CommandText & "FROM BILLING_APPROVAL " &
                                 "INNER JOIN BILLING_MSTR bm ON BA_BILL_INDEX = BM_INVOICE_INDEX " &
                                 "INNER JOIN billing_details bd ON bm.BM_INVOICE_NO = bd.BM_INVOICE_NO AND bm.BM_S_COY_ID = bd.BM_S_COY_ID " &
                                 "WHERE(BA_SEQ - 1 = BA_AO_ACTION) And BM_INVOICE_STATUS = 2 "
                    If ViewState("AO") <> "" Then
                        .CommandText = .CommandText & "AND (BA_AO = '" & ViewState("AO") & "' OR (BA_A_AO = '" & ViewState("AO") & "' AND BA_RELIEF_IND = 'O')) AND BM_B_COY_ID = '" & Session("CompanyID") & "' "
                    End If
                Else
                    .CommandText = .CommandText & "FROM BILLING_APPROVAL " &
                                    "INNER JOIN BILLING_MSTR bm ON BA_BILL_INDEX = BM_INVOICE_INDEX " &
                                    "INNER JOIN billing_details bd ON bm.BM_INVOICE_NO = bd.BM_INVOICE_NO AND bm.BM_S_COY_ID = bd.BM_S_COY_ID " &
                                    "WHERE(BA_SEQ - 1 = BA_AO_ACTION) And BM_INVOICE_STATUS = 2 "

                    If ViewState("AO") <> "" Then
                        .CommandText = .CommandText & "AND (BA_AO = '" & ViewState("AO") & "' OR (BA_A_AO = '" & ViewState("AO") & "' AND BA_RELIEF_IND = 'O')) AND BM_B_COY_ID = '" & Session("CompanyID") & "' "
                    Else
                        .CommandText = .CommandText & "AND (BA_AO IN (SELECT um_user_id FROM user_mstr " &
                                    "INNER JOIN users_usrgrp ON UU_USER_ID=UM_USER_ID AND UU_COY_ID=UM_COY_ID " &
                                    "INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID AND UGM_DELETED='N' " &
                                    "INNER JOIN company_dept_mstr ON cdm_dept_code = um_dept_id " &
                                    "WHERE UGM_FIXED_ROLE = 'Billing Approving Officer' " &
                                    "AND cdm_dept_code= (SELECT um_dept_id FROM user_mstr WHERE um_user_id='" & Session("UserID") & "' AND um_coy_id='" & Session("CompanyID") & "')) " &
                                    "OR (BA_A_AO IN (SELECT um_user_id FROM user_mstr " &
                                    "INNER JOIN users_usrgrp ON UU_USER_ID=UM_USER_ID AND UU_COY_ID=UM_COY_ID " &
                                    "INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID AND UGM_DELETED='N' " &
                                    "INNER JOIN company_dept_mstr ON cdm_dept_code = um_dept_id " &
                                    "WHERE UGM_FIXED_ROLE = 'Billing Approving Officer' " &
                                    "AND cdm_dept_code= (SELECT um_dept_id FROM user_mstr WHERE um_user_id='" & Session("UserID") & "' AND um_coy_id='" & Session("CompanyID") & "')) " &
                                    "AND BA_RELIEF_IND = 'O')) AND BM_B_COY_ID = '" & Session("CompanyID") & "' "
                    End If
                End If
                .CommandText = .CommandText & "GROUP BY bm.BM_INVOICE_NO,bm.BM_B_COY_ID,bm.BM_S_COY_ID "
            End With

            da = New MySqlDataAdapter(cmd)
            strUserId = Session("UserName")
            strCoyName = Session("CompanyName")

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("BillPendingApp_dtBillPendingApp", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = dispatcher.direct("Report", "BillPendingAppr.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")

            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            If TotalParameter > 0 Then
                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
                For I = 0 To localreport.GetParameters.Count - 1
                    GetParameter = localreport.GetParameters.Item(I).Name
                    Select Case LCase(GetParameter)
                        Case "prmrequestedby"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

                        Case "logo"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                        Case "prmtitle"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, Format(Now, "dd/MM/yyyy"))

                        Case "prmbuyercoyname"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)

                        Case Else
                    End Select
                Next
                localreport.SetParameters(par)
            End If

            localreport.Refresh()

            Dim deviceInfo As String =
                    "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            strFileName = "IPPD-024 - BillingPendingApprovalReport.pdf"
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()


        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub

    'Jules 2015.01.30 IPP Stage 2A
    Private Sub ExportToExcel_DailyBillingSummary()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        'Dim strDate As String = ""
        Dim strFileName As String = ""


        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text

                'Mod by Jules 2015-Feb-03 for IPP GST Stage 2A
                .CommandText = "SELECT bm.BM_INVOICE_NO AS 'Document No.', bm.BM_DOC_DATE AS 'Document Date', bm.BM_CREATED_BY AS 'Teller ID', " &
                                "bm_status_changed_by AS 'Finance Officer ID', bm.BM_S_COY_NAME AS 'Customer', " &
                                "FORMAT(SUM(IF(bm.BM_CURRENCY_CODE='MYR',bd.BM_UNIT_COST*bd.BM_RECEIVED_QTY+IFNULL(bd.BM_GST_VALUE,0), " &
                                "(bd.BM_UNIT_COST*bd.BM_RECEIVED_QTY+IFNULL(bd.BM_GST_VALUE,0)) *IFNULL(bm.BM_EXCHANGE_RATE,1))),2) AS 'Invoice Amount (MYR)', " &
                                "FORMAT(SUM(IF(bm.BM_CURRENCY_CODE='MYR','', (bd.BM_UNIT_COST*bd.BM_RECEIVED_QTY+IFNULL(bd.BM_GST_VALUE,0)))),2) AS 'Invoice Amount (FCY)', " &
                                "FORMAT(SUM(IF(bm.BM_CURRENCY_CODE='MYR',IFNULL(bd.BM_GST_VALUE,0),IFNULL(bd.BM_GST_VALUE,0) * IFNULL(bm.BM_EXCHANGE_RATE,1))),2) AS 'GST Amount' " &
                                "FROM billing_mstr bm " &
                                "INNER JOIN billing_details bd ON bm.BM_INVOICE_NO = bd.BM_INVOICE_NO AND bm.BM_S_COY_ID = bd.BM_S_COY_ID " &
                                "WHERE bm_b_coy_id = '" & Session("CompanyID") & "' AND bm_invoice_type IS NOT NULL AND bm_invoice_status = 3 "

                If ViewState("AO") <> "" Then
                    .CommandText = .CommandText & "AND bm_status_changed_by='" & ViewState("AO") & "' "
                Else
                    If ViewState("AOF") = False Then
                        .CommandText = .CommandText & " AND bm_status_changed_by IN " &
                                        "(SELECT um_user_id FROM user_mstr " &
                                        "INNER JOIN users_usrgrp ON UU_USER_ID=UM_USER_ID AND UU_COY_ID=UM_COY_ID " &
                                        "INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID AND UGM_DELETED='N' " &
                                        "INNER JOIN company_dept_mstr ON cdm_dept_code = um_dept_id " &
                                        "WHERE UGM_FIXED_ROLE = 'Billing Approving Officer' " &
                                        "AND cdm_dept_code= (SELECT um_dept_id FROM user_mstr WHERE um_user_id='" & Session("UserID") & "' AND um_coy_id='" & Session("CompanyID") & "')) "
                    End If
                End If
                .CommandText = .CommandText & "GROUP BY bm.BM_INVOICE_NO,bm.BM_B_COY_ID,bm.BM_S_COY_ID "
            End With

            da = New MySqlDataAdapter(cmd)
            da.Fill(ds)
            strFileName = "IPPD-025 - DailyBillingSummaryReport " & "(" & Format(Now, "ddMMMyyyy") & ").xls"
            Dim attachment As String = "attachment;filename=" & strFileName
            Response.ClearContent()
            Response.AddHeader("Content-Disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"

            Dim dc As DataColumn
            Dim i, j As Integer

            Response.Write("No.")
            For Each dc In ds.Tables(0).Columns
                Response.Write(vbTab + dc.ColumnName)
            Next
            Response.Write(vbCrLf)

            j = 1
            Dim dr As DataRow
            For Each dr In ds.Tables(0).Rows
                Response.Write(j.ToString)
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    If i > 0 Then
                        Response.Write(vbTab + dr.Item(i).ToString)
                    Else
                        Response.Write(vbTab + dr.Item(i).ToString)
                    End If
                Next
                Response.Write(vbCrLf)
                j += 1
            Next
            Response.End()

        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try

    End Sub

    'Jules 2015.01.30 IPP Stage 2A
    Private Sub ExportToPDF_DailyBillingSummary()
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory 'HttpContext.Current.Request.ApplicationPath
        Dim objFile As New FileManagement
        Dim strImgSrc As String
        Dim strUserId As String
        Dim strCoyName As String
        'Dim strDate As String = ""
        Dim strFileName As String = ""

        'strDate = Format(CDate(txtDate.Text), "yyyy-MM-dd")

        strImgSrc = objFile.getReportCoyLogo(EnumUploadFrom.FrontOff, Session("CompanyID"), dispatcher.direct("Plugins\images", "", "Report"), System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath"))

        Try
            myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";" '"server=10.228.201.110;UID=root;pwd=managedservices;database=eProcure;"
            conn = New MySqlConnection(myConnectionString)
            conn.Open()

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text

                'Mod by Jules 2015-Feb-03 for IPP GST Stage 2A
                .CommandText = "SELECT bm.BM_INVOICE_NO, DATE_FORMAT(bm.BM_DOC_DATE,'%d/%m/%Y') AS BM_DOC_DATE, bm.BM_CREATED_BY, bm.BM_S_COY_NAME, bm.BM_CURRENCY_CODE, bd.BM_UNIT_COST, bd.BM_RECEIVED_QTY, bm.BM_APPROVED_BY, bm.bm_status_changed_by, " &
                                "IF(bm.BM_CURRENCY_CODE='MYR',FORMAT(SUM(IFNULL(bd.BM_GST_VALUE,0)),2),FORMAT(SUM(IFNULL(bd.BM_GST_VALUE,0) * IFNULL(bm.BM_EXCHANGE_RATE,1)),2)) AS GSTAmt, " &
                                "IF(bm.BM_CURRENCY_CODE='MYR',FORMAT(SUM(bd.BM_UNIT_COST*bd.BM_RECEIVED_QTY+IFNULL(bd.BM_GST_VALUE,0)),2), " &
                                "FORMAT(SUM((bd.BM_UNIT_COST*bd.BM_RECEIVED_QTY+IFNULL(bd.BM_GST_VALUE,0)) * CAST((IFNULL(bm.BM_EXCHANGE_RATE,1)) AS DECIMAL(10,4))),2)) AS InvAmtMYR, " &
                                "IF(bm.BM_CURRENCY_CODE='MYR','', FORMAT(SUM((bd.BM_UNIT_COST*bd.BM_RECEIVED_QTY+IFNULL(bd.BM_GST_VALUE,0))),2)) AS InvAmtFCY " &
                                "FROM billing_mstr bm " &
                                "INNER JOIN billing_details bd ON bm.BM_INVOICE_NO = bd.BM_INVOICE_NO AND bm.BM_S_COY_ID = bd.BM_S_COY_ID " &
                                "WHERE bm_b_coy_id = '" & Session("CompanyID") & "' AND bm_invoice_type IS NOT NULL AND bm_invoice_status = 3 "

                If ViewState("AO") <> "" Then
                    .CommandText = .CommandText & "AND bm_status_changed_by='" & ViewState("AO") & "' "
                Else
                    If ViewState("AOF") = False Then
                        .CommandText = .CommandText & " AND bm_status_changed_by IN " &
                                        "(SELECT um_user_id FROM user_mstr " &
                                        "INNER JOIN users_usrgrp ON UU_USER_ID=UM_USER_ID AND UU_COY_ID=UM_COY_ID " &
                                        "INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID AND UGM_DELETED='N' " &
                                        "INNER JOIN company_dept_mstr ON cdm_dept_code = um_dept_id " &
                                        "WHERE UGM_FIXED_ROLE = 'Billing Approving Officer' " &
                                        "AND cdm_dept_code= (SELECT um_dept_id FROM user_mstr WHERE um_user_id='" & Session("UserID") & "' AND um_coy_id='" & Session("CompanyID") & "')) "
                    End If
                End If
                .CommandText = .CommandText & "GROUP BY bm.BM_INVOICE_NO,bm.BM_B_COY_ID,bm.BM_S_COY_ID "
            End With

            da = New MySqlDataAdapter(cmd)
            strUserId = Session("UserName")
            strCoyName = Session("CompanyName")

            da.Fill(ds)
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("DailyBillingSummary_dtDailyBillingSummary", ds.Tables(0))
            Dim localreport As New LocalReport
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.ReportPath = dispatcher.direct("Report", "DailyBillingSummary.rdlc", "Report") ' Server.MapPath("POSummary_pdf.rdlc")  'dispatcher.direct("Report", "POSummary_pdf.rdlc")            
            localreport.EnableExternalImages = True

            Dim I As Byte
            Dim GetParameter As String = ""
            Dim TotalParameter As Byte
            Dim strID As String = ""

            strID = System.Configuration.ConfigurationManager.AppSettings.Get("Env") & "ReportCoyLogoPath"
            TotalParameter = localreport.GetParameters.Count
            If TotalParameter > 0 Then
                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
                For I = 0 To localreport.GetParameters.Count - 1
                    GetParameter = localreport.GetParameters.Item(I).Name
                    Select Case LCase(GetParameter)
                        Case "prmrequestedby"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strUserId)

                        Case "logo"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, System.Configuration.ConfigurationManager.AppSettings(strID) & strImgSrc)

                        Case "prmtitle"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, Format(Now, "dd/MM/yyyy"))

                        Case "prmbuyercoyname"
                            par(I) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, strCoyName)

                        Case Else
                    End Select
                Next
                localreport.SetParameters(par)
            End If

            localreport.Refresh()

            Dim deviceInfo As String =
                    "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                    "</DeviceInfo>"
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            System.Threading.Thread.Sleep(1000) ' Sleep for 1 seconds
            strFileName = "IPPD-025 - DailyBillingSummary.pdf" 'Mod by Jules 2015-Feb-03 for IPP GST Stage 2A
            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()


        Catch ex As Exception
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try
    End Sub

    'Jules 2015.01.30 IPP Stage 2A
    Private Sub ExportToPDF_TaxInvoice_Billing()
        Dim objDb As New EAD.DBCom
        Dim strSQL, strTaxInv, strVendorCoyId As String
        Dim blnBillingST As Boolean = False
        Dim blnBillingSO As Boolean = False
        Dim blnIPP As Boolean = False
        Dim objUserRole As New UserRoles
        Dim objUsers As New Users
        Dim Roles As New ArrayList : Roles = Session("MixUserRole")

        For Each str As String In Roles
            If str = "Billing Officer" Then
                blnBillingST = objUsers.checkUserFixedRole("'" & str & "'")
            ElseIf str = "Billing Approving Officer" Then
                blnBillingSO = objUsers.checkUserFixedRole("'" & str & "'")
            ElseIf str = "IPP Approving Officer" Or str.Contains("Finance Manager") Or str = "IPP Officer(F)" Then
                blnIPP = True
            End If
        Next

        'Check for user dept
        If blnBillingSO Or blnBillingST Then
            strSQL = "SELECT '*' " &
            "FROM billing_mstr bm,billing_details bd,user_mstr,user_group_mstr,users_usrgrp " &
            "WHERE(bm.bm_invoicE_no = bd.bm_invoice_no) " &
            "AND um_user_id = bm_created_by " &
            "AND um_user_id = uu_user_id " &
            "AND uu_usrgrp_id = ugm_usrgrp_id " &
            "AND ugm_fixed_role IN ('Billing Officer','Billing Approving Officer') " &
            "AND bm_dn_no = '" & txtTaxInvoice.Text & "'"
        ElseIf blnIPP Then
            strSQL = "SELECT '*' " &
            "FROM billing_mstr bm, billing_details bd " &
            "WHERE(bm.bm_invoice_no = bd.bm_invoice_no)" &
            "AND bm.bm_s_coy_id = bd.bm_s_coy_id " &
            "AND bm_dn_no = '" & txtTaxInvoice.Text & "' " &
            "AND bm_remarks1 = 'IPP'"
        Else
            strSQL = "SELECT '*' FROM billing_details WHERE bm_dn_NO = '" & txtTaxInvoice.Text & "' "
        End If

        strTaxInv = objDb.GetVal(strSQL)
        If Not strTaxInv.Trim.Length = 0 Then
            printBillingTaxInvoice(txtTaxInvoice.Text)
        Else
            'Check for user dept
            If blnBillingSO Or blnBillingST Then
                strSQL = "SELECT '*' " &
                "FROM billing_mstr bm,billing_details bd,user_mstr,user_group_mstr,users_usrgrp " &
                "WHERE(bm.bm_invoicE_no = bd.bm_invoice_no) " &
                "AND um_user_id = bm_created_by " &
                "AND um_user_id = uu_user_id " &
                "AND uu_usrgrp_id = ugm_usrgrp_id " &
                "AND ugm_fixed_role IN ('Billing Officer','Billing Approving Officer') " &
                "AND bm_dn_no = '" & txtTaxInvoice.Text & "'"
            ElseIf blnIPP Then
                strSQL = "SELECT '*' " &
                "FROM billing_mstr bm, billing_details bd " &
                "WHERE(bm.bm_invoice_no = bd.bm_invoice_no)" &
                "AND bm.bm_s_coy_id = bd.bm_s_coy_id " &
                "AND bm_dn_no = '" & txtTaxInvoice.Text & "' " &
                "AND bm_remarks1 = 'IPP'"
            Else
                strSQL = "SELECT '*' FROM billing_details WHERE bm_dn_NO = '" & txtTaxInvoice.Text & "' "
            End If
            strTaxInv = objDb.GetVal(strSQL)
            If Not strTaxInv.Trim.Length = 0 Then
                printBillingTaxInvoice(txtTaxInvoice.Text)
            Else
                Common.NetMsgbox(Me, "Tax Invoice does not exist.")
            End If
        End If

    End Sub

    Private Sub printIPPTaxInvoice(ByVal taxInvoiceNo As String)

        'Dim rdlcPath As String = System.Configuration.ConfigurationManager.AppSettings.Get("rdlcPath")
        Dim da As MySqlDataAdapter = Nothing
        Dim dtDoc As New DataTable
        Dim dsDoc As New DataSet
        Dim ds As New DataSet
        Dim conn As MySqlConnection = Nothing
        Dim myConnectionString, strBRCoyName, strVendorID, coyid As String
        Dim strDt2 = Date.Today.ToString("dd") & Date.Today.ToString("MM") & Today.Year.ToString.Substring(2, 2)
        Dim objDb As New EAD.DBCom

        myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";"
        conn = New MySqlConnection(myConnectionString)
        conn.Open()

        Dim cmd = New MySqlCommand
        With cmd
            .Connection = conn
            .CommandType = CommandType.Text

            .CommandText = "SELECT ID_DN_NO as DN_No, im_s_coy_id,id_product_desc, im_invoice_type, FORMAT(IF(im_payment_term = 'TT',(ID_RECEIVED_QTY * ID_UNIT_COST),(ID_RECEIVED_QTY * ID_UNIT_COST)) ,2) AS Amount, " _
                     & "FORMAT(IF(im_payment_term = 'TT',(ID_RECEIVED_QTY * ID_UNIT_COST * im_exchange_rate),(ID_RECEIVED_QTY * ID_UNIT_COST )) ,2) AS AmountWithExg, " _
                     & "im_currency_code,ic_coy_name,ic_addr_line1, ic_addr_line2,ic_addr_line3,ic_postcode,ic_city, (SELECT CODE_DESC FROM code_mstr WHERE code_abbr = ic_state AND code_value = ic_country AND code_category = 'S' AND code_deleted = 'N') as ic_state ,(SELECT CODE_DESC FROM code_mstr WHERE code_abbr = ic_country AND code_category = 'CT' AND code_deleted = 'N') AS ic_country,ic_phone,ic_fax, " _
                     & "cm_coy_name, cm_addr_line1, cm_addr_line2, cm_addr_line3, cm_postcode, cm_city,  (SELECT CODE_DESC FROM code_mstr WHERE code_abbr = cm_state AND code_value = cm_country AND code_category = 'S' AND code_deleted = 'N') as cm_state, (SELECT CODE_DESC FROM code_mstr WHERE code_abbr = cm_country AND code_category = 'CT' AND code_deleted = 'N') as cm_country, cm_phone, cm_fax, IM_PAYMENT_NO,im_invoice_no, im_s_coy_name,im_created_by,IFNULL(im_exchange_rate,1) 'im_exchange_rate',IFNULL(id_dr_exchange_rate,1) 'id_dr_exchange_rate',IFNULL(id_dr_currency,'') 'id_dr_currency',im_payment_term, " _
                     & "'TAX INVOICE' AS 'header', " _
                     & "IF(id_gst_value IS NULL,0,IF(im_payment_term = 'TT', id_gst_value * im_exchange_rate, " _
                     & "IF(id_dr_exchange_rate IS NULL OR id_dr_exchange_rate = 0, id_gst_value, id_gst_value * id_dr_exchange_rate))) AS 'id_gst_value', " _
                     & "IF(id_gst_value IS NULL,0,IF(im_payment_term = 'TT', (id_gst_value * im_exchange_rate) + (ID_RECEIVED_QTY * ID_UNIT_COST * im_exchange_rate), " _
                     & "IF(id_dr_exchange_rate IS NULL OR id_dr_exchange_rate = 0, id_gst_value + (ID_RECEIVED_QTY * ID_UNIT_COST), (id_gst_value * id_dr_exchange_rate) + (ID_RECEIVED_QTY * ID_UNIT_COST * id_dr_exchange_rate)))) AS 'totalwithGST' " _
                     & "FROM INVOICE_MSTR " _
                     & "LEFT JOIN invoice_details ON id_invoice_no = im_invoice_no AND id_s_coy_id = im_s_coy_id " _
                     & "INNER JOIN ipp_company ON ic_other_b_coy_code = id_pay_for AND ic_coy_id = im_b_coy_id " _
                     & "INNER JOIN company_mstr ON im_b_coy_id = cm_coy_id " _
                     & "WHERE im_invoice_status = 4 AND im_invoice_type IS NOT NULL and id_gst_reimb in ('R','N/A') AND im_b_coy_id = '" & Session("CompanyId") & "'  AND im_payment_date IS NOT NULL and id_dn_no = '" & Common.Parse(taxInvoiceNo) & "' ORDER BY ic_coy_name,im_s_coy_id "
            'and im_invoice_type = 'INV'
        End With

        da = New MySqlDataAdapter(cmd)
        da.SelectCommand.CommandTimeout = 50000
        da.Fill(ds)
        If Not ds.Tables(0).Rows.Count = 0 Then
            dtDoc.Columns.Add("DNNo", Type.GetType("System.String"))
            dtDoc.Columns.Add("im_s_coy_id", Type.GetType("System.String"))
            dtDoc.Columns.Add("id_product_desc", Type.GetType("System.String"))
            dtDoc.Columns.Add("im_invoice_type", Type.GetType("System.String"))
            dtDoc.Columns.Add("im_currency_code", Type.GetType("System.String"))
            dtDoc.Columns.Add("Amount", Type.GetType("System.Decimal"))
            dtDoc.Columns.Add("AmountWithExg", Type.GetType("System.Decimal"))
            dtDoc.Columns.Add("cm_coy_name", Type.GetType("System.String"))
            dtDoc.Columns.Add("cm_addr_line1", Type.GetType("System.String"))
            dtDoc.Columns.Add("cm_addr_line2", Type.GetType("System.String"))
            dtDoc.Columns.Add("cm_addr_line3", Type.GetType("System.String"))
            dtDoc.Columns.Add("cm_postcode", Type.GetType("System.String"))
            dtDoc.Columns.Add("cm_city", Type.GetType("System.String"))
            dtDoc.Columns.Add("code_desc2", Type.GetType("System.String"))
            dtDoc.Columns.Add("code_desc3", Type.GetType("System.String"))
            dtDoc.Columns.Add("cm_phone", Type.GetType("System.String"))
            dtDoc.Columns.Add("cm_fax", Type.GetType("System.String"))
            dtDoc.Columns.Add("ic_coy_name", Type.GetType("System.String"))
            dtDoc.Columns.Add("ic_addr_line1", Type.GetType("System.String"))
            dtDoc.Columns.Add("ic_addr_line2", Type.GetType("System.String"))
            dtDoc.Columns.Add("ic_addr_line3", Type.GetType("System.String"))
            dtDoc.Columns.Add("ic_postcode", Type.GetType("System.String"))
            dtDoc.Columns.Add("ic_city", Type.GetType("System.String"))
            dtDoc.Columns.Add("code_desc", Type.GetType("System.String"))
            dtDoc.Columns.Add("code_desc1", Type.GetType("System.String"))
            dtDoc.Columns.Add("ic_phone", Type.GetType("System.String"))
            dtDoc.Columns.Add("ic_fax", Type.GetType("System.String"))
            dtDoc.Columns.Add("im_s_coy_name", Type.GetType("System.String"))
            dtDoc.Columns.Add("im_invoice_no", Type.GetType("System.String"))
            dtDoc.Columns.Add("im_payment_no", Type.GetType("System.String"))
            dtDoc.Columns.Add("im_created_by", Type.GetType("System.String"))
            dtDoc.Columns.Add("im_exchange_rate", Type.GetType("System.String"))
            dtDoc.Columns.Add("id_dr_exchange_rate", Type.GetType("System.String"))
            dtDoc.Columns.Add("id_dr_currency", Type.GetType("System.String"))
            dtDoc.Columns.Add("im_payment_term", Type.GetType("System.String"))
            dtDoc.Columns.Add("header", Type.GetType("System.String"))
            dtDoc.Columns.Add("id_gst_value", Type.GetType("System.Decimal"))
            dtDoc.Columns.Add("totalwithGST", Type.GetType("System.Decimal"))

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim dtr As DataRow
                dtr = dtDoc.NewRow()

                dtr("DNNo") = ds.Tables(0).Rows(i)("DN_NO")
                dtr("im_s_coy_id") = ds.Tables(0).Rows(i)("im_s_coy_id")
                dtr("id_product_desc") = ds.Tables(0).Rows(i)("id_product_desc")
                dtr("im_invoice_type") = ds.Tables(0).Rows(i)("im_invoice_type")
                dtr("im_currency_code") = ds.Tables(0).Rows(i)("im_currency_code")
                dtr("Amount") = ds.Tables(0).Rows(i)("Amount")
                dtr("AmountWithExg") = ds.Tables(0).Rows(i)("AmountWithExg")
                dtr("cm_coy_name") = ds.Tables(0).Rows(i)("cm_coy_name")
                dtr("cm_addr_line1") = ds.Tables(0).Rows(i)("cm_addr_line1")
                dtr("cm_addr_line2") = ds.Tables(0).Rows(i)("cm_addr_line2")
                dtr("cm_addr_line3") = ds.Tables(0).Rows(i)("cm_addr_line3")
                dtr("cm_postcode") = ds.Tables(0).Rows(i)("cm_postcode")
                dtr("cm_city") = ds.Tables(0).Rows(i)("cm_city")
                dtr("code_desc2") = ds.Tables(0).Rows(i)("cm_state")
                dtr("code_desc3") = ds.Tables(0).Rows(i)("cm_country")
                dtr("cm_phone") = ds.Tables(0).Rows(i)("cm_phone")
                dtr("cm_fax") = ds.Tables(0).Rows(i)("cm_fax")
                dtr("ic_coy_name") = ds.Tables(0).Rows(i)("ic_coy_name")
                dtr("ic_addr_line1") = ds.Tables(0).Rows(i)("ic_addr_line1")
                dtr("ic_addr_line2") = ds.Tables(0).Rows(i)("ic_addr_line2")
                dtr("ic_addr_line3") = ds.Tables(0).Rows(i)("ic_addr_line3")
                dtr("ic_postcode") = ds.Tables(0).Rows(i)("ic_postcode")
                dtr("ic_city") = ds.Tables(0).Rows(i)("ic_city")
                dtr("code_desc") = ds.Tables(0).Rows(i)("ic_state")
                dtr("code_desc1") = ds.Tables(0).Rows(i)("ic_country")
                dtr("ic_phone") = ds.Tables(0).Rows(i)("ic_phone")
                dtr("ic_fax") = ds.Tables(0).Rows(i)("ic_fax")
                dtr("im_s_coy_name") = ds.Tables(0).Rows(i)("im_s_coy_name")
                dtr("im_invoice_no") = ds.Tables(0).Rows(i)("im_invoice_no")
                dtr("im_payment_no") = ds.Tables(0).Rows(i)("im_payment_no")
                dtr("im_created_by") = ds.Tables(0).Rows(i)("im_created_by")
                dtr("im_exchange_rate") = ds.Tables(0).Rows(i)("im_exchange_rate")
                dtr("id_dr_exchange_rate") = ds.Tables(0).Rows(i)("id_dr_exchange_rate")
                dtr("id_dr_currency") = ds.Tables(0).Rows(i)("id_dr_currency")
                dtr("im_payment_term") = ds.Tables(0).Rows(i)("im_payment_term")
                dtr("header") = ds.Tables(0).Rows(i)("header")
                If Not ds.Tables(0).Rows(i)("id_gst_value") Is DBNull.Value Then
                    dtr("id_gst_value") = ds.Tables(0).Rows(i)("id_gst_value")
                Else
                    dtr("id_gst_value") = 0.0
                End If
                If Not ds.Tables(0).Rows(i)("totalwithGST") Is DBNull.Value Then
                    dtr("totalwithGST") = ds.Tables(0).Rows(i)("totalwithGST")
                Else
                    dtr("totalwithGST") = 0.0
                End If
                dtDoc.Rows.Add(dtr)

                strBRCoyName = ds.Tables(0).Rows(i)("ic_coy_name")
                strVendorID = ds.Tables(0).Rows(i)("im_s_coy_id")
            Next

            dsDoc.Tables.Add(dtDoc)

            Dim strFileName As String
            Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("dsDebitNote_DataTable1", dsDoc.Tables(0))
            Dim localreport As New LocalReport
            localreport.ReportPath = dispatcher.direct("Report", "Debit Note_wGST.rdlc", "Report")
            localreport.DataSources.Clear()
            localreport.DataSources.Add(rptDataSource)
            localreport.Refresh()

            'Zulham 21032015 
            Dim TotalParameter As Byte
            Dim GetParameter As String = ""
            TotalParameter = localreport.GetParameters.Count

            'Zulham 10062015 
            'Get GST ID for the buyer company
            Dim gstId As String = ""

            gstId = objDb.GetVal("Select ifnull(cm_tax_reg_no,'') from company_mstr where cm_coy_id = '" & Session("CompanyId") & "'")
            coyid = Common.parseNull(Session("CompanyId"))

            Dim lI As Byte
            If TotalParameter > 0 Then
                Dim par(TotalParameter - 1) As Microsoft.Reporting.WebForms.ReportParameter
                For lI = 0 To localreport.GetParameters.Count - 1
                    GetParameter = localreport.GetParameters.Item(lI).Name
                    Select Case LCase(GetParameter)
                        Case "coyid"
                            If coyid.ToUpper = "HLB" Then
                                par(lI) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, "HONG LEONG BANK BERHAD")
                            ElseIf coyid.ToUpper = "HLISB" Then
                                par(lI) = New Microsoft.Reporting.WebForms.ReportParameter(GetParameter, "HONG LEONG ISLAMIC BANK")
                            End If
                        Case Else
                    End Select
                Next
                localreport.SetParameters(par)
            End If

            Dim deviceInfo As String =
                            "<DeviceInfo>" +
                                "  <OutputFormat>EMF</OutputFormat>" +
                                "</DeviceInfo>"
            Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
            strFileName = ParseSpecialChar(taxInvoiceNo) & "-" & strDt2 & "_" & ParseSpecialChar(taxInvoiceNo) & "_Original.pdf"

            'Return PDF
            Me.Response.Clear()
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
            Me.Response.BinaryWrite(PDF)
            Me.Response.End()

        End If

    End Sub

    Public Shared Function ParseSpecialChar(ByVal pInString As String) As String

        If InStr(pInString, "*") > 0 Then
            pInString = Replace(pInString, "*", "")
        ElseIf InStr(pInString, "/") > 0 Then
            pInString = Replace(pInString, "/", "")
        ElseIf InStr(pInString, "\") > 0 Then
            pInString = Replace(pInString, "\", "")
        ElseIf InStr(pInString, "?") > 0 Then
            pInString = Replace(pInString, "?", "")
        ElseIf InStr(pInString, ":") > 0 Then
            pInString = Replace(pInString, ":", "")
        ElseIf InStr(pInString, "|") > 0 Then
            pInString = Replace(pInString, "|", "")
        ElseIf InStr(pInString, """") > 0 Then
            pInString = Replace(pInString, """", "")
        ElseIf InStr(pInString, "<") > 0 Then
            pInString = Replace(pInString, "<", "")
        ElseIf InStr(pInString, ">") > 0 Then
            pInString = Replace(pInString, ">", "")
        End If
        Return pInString
    End Function

    Private Sub printBillingTaxInvoice(ByVal taxInvoiceNo As String)
        'Dim rdlcPath As String = System.Configuration.ConfigurationManager.AppSettings.Get("rdlcPath")
        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim strDt2 = Date.Today.ToString("dd") & Date.Today.ToString("MM") & Today.Year.ToString.Substring(2, 2)
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim strAryQuery(0) As String
        Dim i, j As Integer
        Dim i2 As Integer
        Dim a As String = ""
        Dim strDNNo As String = ""
        Dim strDNPrefix As String = ""
        Dim strDNLastUsedNo As String = ""
        Dim strBRCoyName As String = ""
        Dim strVendorID As String = ""
        Dim intIncrement As Integer
        Dim strSQLDN As String
        Dim dsDN As New DataSet
        Dim billingNo As String
        Dim coyId As String

        myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";"

        conn = New MySqlConnection(myConnectionString)

        Try
            conn.Open()

            Dim dtDoc, dtDocForeign As New DataTable
            Dim dsDoc, dsDocForeign As New DataSet
            Dim ds As New DataSet

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text

                .CommandText = "SELECT if(bm_gst_value = 0.00,bm_gst_output_tax_code,'ZRE') as 'bm_gst_output_tax_code', bm.bm_currency_code, cm_tax_reg_no, bm.bm_s_coy_id, bd.bm_gst_value, ifnull(bm.bm_exchange_rate,1) 'bm_exchange_rate', bm.bm_created_by, bd.bm_invoice_no, bd.bm_dn_no,IF((SELECT DISTINCT tax_perc FROM tax_mstr,tax WHERE tax_code = tm_tax_rate AND tm_tax_code = bd.bm_gst_output_tax_code and bm.bm_b_coy_id = tm_coy_id) = 6.00,CONCAT('*  ', bm_product_desc),CONCAT('** ', bm_product_desc)) 'Description', " _
                & "bm_received_qty, bm_unit_cost, bm_received_qty * bm_unit_cost 'TotalRM', " _
                & "IF((SELECT DISTINCT tax_perc FROM tax_mstr,tax WHERE tax_code = tm_tax_rate AND tm_tax_code = bd.bm_gst_output_tax_code and bm.bm_b_coy_id = tm_coy_id) = 6.00, bm_received_qty * bm_unit_cost,0) 'GSTSumm6', " _
                & "IF((SELECT DISTINCT tax_perc FROM tax_mstr,tax WHERE tax_code = tm_tax_rate AND tm_tax_code = bd.bm_gst_output_tax_code and bm.bm_b_coy_id = tm_coy_id) = 6.00, bm_received_qty * bm_unit_cost * IF(TRIM(bm_exchange_rate) = '0' OR bm_exchange_rate IS NULL,0,bm_exchange_rate),0) 'ForeignGSTSumm6', " _
                & "IF((SELECT DISTINCT tax_perc FROM tax_mstr,tax WHERE tax_code = tm_tax_rate AND tm_tax_code = bd.bm_gst_output_tax_code and bm.bm_b_coy_id = tm_coy_id) = 0.00, bm_received_qty * bm_unit_cost,0) 'GSTSumm0', " _
                & "IF((SELECT DISTINCT tax_perc FROM tax_mstr,tax WHERE tax_code = tm_tax_rate AND tm_tax_code = bd.bm_gst_output_tax_code and bm.bm_b_coy_id = tm_coy_id) = 6.00, bm_gst_value,0) 'GSTSummTaxValue', " _
                & "IF((SELECT DISTINCT tax_perc FROM tax_mstr,tax WHERE tax_code = tm_tax_rate AND tm_tax_code = bd.bm_gst_output_tax_code and bm.bm_b_coy_id = tm_coy_id) = 6.00, bm_gst_value * IF(TRIM(bm_exchange_rate) = '0' OR bm_exchange_rate IS NULL,0,bm_exchange_rate) ,0) 'ForeignGSTSummTaxValue', " _
                & "bm_received_qty * bm_unit_cost * IF(TRIM(bm_exchange_rate) = '0' OR bm_exchange_rate IS NULL,0,bm_exchange_rate) 'TotalExcludeGSTMYR', " _
                & "bm_received_qty * bm_unit_cost * IF(TRIM(bm_exchange_rate) = '0' OR bm_exchange_rate IS NULL,0,bm_exchange_rate) + (bm_gst_value * IF(TRIM(bm_exchange_rate) = '0' OR bm_exchange_rate IS NULL,0,bm_exchange_rate)) 'TotalMYR', " _
                & "bm_received_qty * bm_unit_cost as 'TotalExcludeGSTForeign', " _
                & "ic.ic_coy_name 'ic_coy_name', " _
                & "CONCAT(TRIM(ic_addr_line1), " _
                & "IF(TRIM(ic_addr_line2) = '' OR ic_addr_line2 IS NULL, '', CONCAT(', \n',ic_addr_line2)), " _
                & "IF(TRIM(ic_addr_line3) = '' OR ic_addr_line3 IS NULL, '', CONCAT(', \n',ic_addr_line3)), " _
                & "IF(TRIM(ic_postcode) = '' OR ic_postcode IS NULL, '', CONCAT(', \n',ic_postcode)), " _
                & "IF(TRIM(ic_city) = '' OR ic_city IS NULL, '', CONCAT(', ',ic_city)), " _
                & "', \n', IFNULL((SELECT CODE_DESC FROM code_mstr WHERE code_abbr = ic_state AND code_value = ic_country AND code_category = 'S' AND code_deleted = 'N'),'n.a'), " _
                & "', ', IFNULL((SELECT CODE_DESC FROM code_mstr WHERE code_abbr = ic_country AND code_category = 'CT' AND code_deleted = 'N'),'n.a')) " _
                & "'PurchaserAddress', ic_phone, ic_fax, " _
                & "concat(cm.cm_coy_name,' (',cm_business_reg_no, ')') 'cm_coy_name', " _
                & "CONCAT(TRIM(cm_addr_line1), " _
                & "IF(TRIM(cm_addr_line2) = '' OR cm_addr_line2 IS NULL, '', CONCAT(', ',cm_addr_line2)),  " _
                & "IF(TRIM(cm_addr_line3) = '' OR cm_addr_line3 IS NULL, '', CONCAT(', ',cm_addr_line3)),  " _
                & "IF(TRIM(cm_postcode) = '' OR cm_postcode IS NULL, '', CONCAT(', ',cm_postcode)),  " _
                & "IF(TRIM(cm_city) = '' OR cm_city IS NULL, '', CONCAT(', ',cm_city)),  " _
                & "', ', IFNULL((SELECT CODE_DESC FROM code_mstr WHERE code_abbr = cm_state AND code_value = cm_country AND code_category = 'S' AND code_deleted = 'N'),'n.a'),  " _
                & "', ', IFNULL((SELECT CODE_DESC FROM code_mstr WHERE code_abbr = cm_country AND code_category = 'CT' AND code_deleted = 'N'),'n.a'), ' (',cm_business_reg_no, ')') " _
                & "'SupplierAddress', cm_phone, cm_fax, bm.bm_b_coy_id as 'bm_b_coy_id', bm.bm_invoice_index as 'bm_invoice_index', tax_perc " _
                & "FROM billing_mstr bm, billing_details bd, ipp_company ic, company_mstr cm, tax, tax_mstr  " _
                & "WHERE(bm_invoice_status = 6) " _
                & "AND bd.bm_invoice_no = bm.bm_invoice_no AND bd.bm_s_coy_id = bm.bm_s_coy_id  " _
                & "AND ic.ic_index = bd.bm_s_coy_id  " _
                & "AND cm_coy_id = bm_b_coy_id " _
                & "And bm_gst_output_tax_code = tm_tax_code " _
                & "AND tm_coy_id = bm.bm_b_coy_id " _
                & "AND tm_tax_rate = tax_code " _
                & "AND bd.bm_dn_no = '" & taxInvoiceNo.ToString & "' and bm_printed_ind ='Y' " _
                & "ORDER BY bm.bm_invoice_no ASC "

            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.CommandTimeout = 50000
            da.Fill(ds)

            dtDoc.Columns.Add("No", Type.GetType("System.String"))
            dtDoc.Columns.Add("Description", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_received_qty", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_unit_cost", Type.GetType("System.String"))
            dtDoc.Columns.Add("TotalRM", Type.GetType("System.String"))
            dtDoc.Columns.Add("GSTSumm6", Type.GetType("System.Decimal"))
            dtDoc.Columns.Add("GSTSumm0", Type.GetType("System.Decimal"))
            dtDoc.Columns.Add("GSTSummTaxValue", Type.GetType("System.String"))
            dtDoc.Columns.Add("ForeignGSTSumm6", Type.GetType("System.String"))
            dtDoc.Columns.Add("ForeignGSTSummTaxValue", Type.GetType("System.String"))
            dtDoc.Columns.Add("TotalExcludeGSTMYR", Type.GetType("System.String"))
            dtDoc.Columns.Add("TotalMYR", Type.GetType("System.String"))
            dtDoc.Columns.Add("TotalExcludeGSTForeign", Type.GetType("System.String"))
            dtDoc.Columns.Add("cm_coy_name", Type.GetType("System.String"))
            dtDoc.Columns.Add("SupplierAddress", Type.GetType("System.String"))
            dtDoc.Columns.Add("cm_phone", Type.GetType("System.String"))
            dtDoc.Columns.Add("cm_fax", Type.GetType("System.String"))
            dtDoc.Columns.Add("ic_coy_name", Type.GetType("System.String"))
            dtDoc.Columns.Add("PurchaserAddress", Type.GetType("System.String"))
            dtDoc.Columns.Add("ic_phone", Type.GetType("System.String"))
            dtDoc.Columns.Add("ic_fax", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_invoice_no", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_created_by", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_exchange_rate", Type.GetType("System.String"))
            dtDoc.Columns.Add("header", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_gst_value", Type.GetType("System.Decimal"))
            dtDoc.Columns.Add("bm_dn_no", Type.GetType("System.String"))
            dtDoc.Columns.Add("ConvertedTotal", Type.GetType("System.Decimal"))
            dtDoc.Columns.Add("ConvertedGST", Type.GetType("System.Decimal"))
            dtDoc.Columns.Add("cm_tax_reg_no", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_gst_output_tax_code", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_b_coy_id", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_invoice_index", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_currency_code", Type.GetType("System.String"))
            dtDoc.Columns.Add("tax_perc", Type.GetType("System.String"))

            For i = 0 To ds.Tables(0).Rows.Count - 1
                Dim dtr As DataRow
                dtr = dtDoc.NewRow()
                dtr("No") = i + 1 'ds.Tables(0).Rows(i)("No")
                dtr("Description") = ds.Tables(0).Rows(i)("Description")
                dtr("bm_received_qty") = ds.Tables(0).Rows(i)("bm_received_qty")
                dtr("bm_unit_cost") = ds.Tables(0).Rows(i)("bm_unit_cost")
                dtr("TotalRM") = ds.Tables(0).Rows(i)("TotalRM")
                dtr("GSTSumm6") = ds.Tables(0).Rows(i)("GSTSumm6")
                dtr("GSTSumm0") = ds.Tables(0).Rows(i)("GSTSumm0")
                dtr("GSTSummTaxValue") = ds.Tables(0).Rows(i)("GSTSummTaxValue")
                dtr("ForeignGSTSumm6") = ds.Tables(0).Rows(i)("ForeignGSTSumm6")
                dtr("ForeignGSTSummTaxValue") = ds.Tables(0).Rows(i)("ForeignGSTSummTaxValue")
                dtr("TotalExcludeGSTMYR") = ds.Tables(0).Rows(i)("TotalExcludeGSTMYR")
                dtr("TotalMYR") = ds.Tables(0).Rows(i)("TotalMYR")
                dtr("TotalExcludeGSTForeign") = ds.Tables(0).Rows(i)("TotalExcludeGSTForeign")
                dtr("cm_coy_name") = ds.Tables(0).Rows(i)("cm_coy_name")
                dtr("SupplierAddress") = ds.Tables(0).Rows(i)("SupplierAddress")
                dtr("cm_phone") = ds.Tables(0).Rows(i)("cm_phone")
                dtr("cm_fax") = ds.Tables(0).Rows(i)("cm_fax")
                dtr("ic_coy_name") = ds.Tables(0).Rows(i)("ic_coy_name")
                dtr("PurchaserAddress") = ds.Tables(0).Rows(i)("PurchaserAddress")
                dtr("ic_phone") = ds.Tables(0).Rows(i)("ic_phone")
                dtr("ic_fax") = ds.Tables(0).Rows(i)("ic_fax")
                dtr("bm_invoice_no") = ds.Tables(0).Rows(i)("bm_invoice_no")
                dtr("bm_created_by") = ds.Tables(0).Rows(i)("bm_created_by")
                dtr("bm_exchange_rate") = FormatNumber(ds.Tables(0).Rows(i)("bm_exchange_rate"), 4)
                dtr("header") = "Tax Invoice"
                dtr("bm_gst_value") = ds.Tables(0).Rows(i)("bm_gst_value")
                dtr("bm_dn_no") = ds.Tables(0).Rows(i)("bm_dn_no")
                dtr("ConvertedTotal") = CDec(ds.Tables(0).Rows(i)("bm_received_qty")) * CDec(ds.Tables(0).Rows(i)("bm_unit_cost")) _
                * CDec(ds.Tables(0).Rows(i)("bm_exchange_rate"))
                dtr("ConvertedGST") = CDec(ds.Tables(0).Rows(i)("bm_gst_value")) * CDec(ds.Tables(0).Rows(i)("bm_exchange_rate"))
                dtr("cm_tax_reg_no") = ds.Tables(0).Rows(i)("cm_tax_reg_no")
                dtr("bm_gst_output_tax_code") = ds.Tables(0).Rows(i)("bm_gst_output_tax_code")
                dtr("bm_invoice_index") = ds.Tables(0).Rows(i)("bm_invoice_index")
                dtr("bm_currency_code") = ds.Tables(0).Rows(i)("bm_currency_code")
                dtr("tax_perc") = ds.Tables(0).Rows(i)("tax_perc")
                dtDoc.Rows.Add(dtr)

                strBRCoyName = ds.Tables(0).Rows(i)("ic_coy_name")
                strVendorID = ds.Tables(0).Rows(i)("bm_s_coy_id")
                billingNo = ds.Tables(0).Rows(i)("bm_invoice_no")
                coyId = ds.Tables(0).Rows(i)("bm_b_coy_id")
                taxInvoiceNo = ds.Tables(0).Rows(i)("bm_dn_no")
            Next

            dsDoc.Tables.Add(dtDoc)
            dsDocForeign.Tables.Add(dtDocForeign)

            Dim strFileName As String
            If dsDoc.Tables(0).Rows.Count > 0 Then
                Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("dsBillingReports_DataTable1", dsDoc.Tables(0))
                Dim localreport As New LocalReport

                localreport.ReportPath = dispatcher.direct("Report", "BillingTaxInvoice.rdlc", "Report")
                localreport.DataSources.Clear()
                localreport.DataSources.Add(rptDataSource)
                localreport.Refresh()

                Dim deviceInfo As String =
                                "<DeviceInfo>" +
                                    "  <OutputFormat>EMF</OutputFormat>" +
                                    "</DeviceInfo>"
                Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
                strFileName = ParseSpecialChar(taxInvoiceNo) & "-" & strDt2 & ".pdf"

                'Return PDF
                Me.Response.Clear()
                Me.Response.ContentType = "application/pdf"
                Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
                Me.Response.BinaryWrite(PDF)
                Me.Response.End()

            End If

            dtDoc = Nothing
            dsDoc = Nothing
            ds = Nothing

        Catch ex As Exception
            'WriteErr(ex.Message)
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try

    End Sub
    'Zulham 08/08/2017 - ipp stage 3
    Private Sub ExportToPDF_BillingReports(Optional ByVal type As String = "")
        Dim objDb As New EAD.DBCom
        Dim strSQL, strTaxInv, strVendorCoyId As String
        Dim blnBillingST As Boolean = False
        Dim blnBillingSO As Boolean = False
        Dim objUserRole As New UserRoles
        Dim objUsers As New Users
        Dim Roles As New ArrayList : Roles = Session("MixUserRole")

        For Each str As String In Roles
            If str = "Billing Officer" Then
                blnBillingST = objUsers.checkUserFixedRole("'" & str & "'")
            ElseIf str = "Billing Approving Officer" Then
                blnBillingSO = objUsers.checkUserFixedRole("'" & str & "'")
            End If
        Next

        'Check for user dept
        If blnBillingSO Or blnBillingST Then
            strSQL = "SELECT '*' " & _
            "FROM billing_mstr bm,billing_details bd,user_mstr,user_group_mstr,users_usrgrp " & _
            "WHERE(bm.bm_invoicE_no = bd.bm_invoice_no) " & _
            "AND um_user_id = bm_created_by " & _
            "AND um_user_id = uu_user_id " & _
            "AND uu_usrgrp_id = ugm_usrgrp_id " & _
            "AND ugm_fixed_role IN ('Billing Officer','Billing Approving Officer') " & _
            "AND bm_dn_no = '" & txtTaxInvoice.Text & "'"
        Else
            strSQL = "SELECT '*' FROM billing_details WHERE bm_dn_NO = '" & txtTaxInvoice.Text & "' "
        End If

        strTaxInv = objDb.GetVal(strSQL)
        If Not strTaxInv.Trim.Length = 0 Then
            Select Case type
                Case "CN"
                    printBillingPDFReports(txtTaxInvoice.Text, type)
                Case "CA"
                    printBillingPDFReports(txtTaxInvoice.Text, type)
                Case "DN"
                    printBillingPDFReports(txtTaxInvoice.Text, type)
                Case "DA"
                    printBillingPDFReports(txtTaxInvoice.Text, type)
            End Select
        Else
            'Check for user dept
            If blnBillingSO Or blnBillingST Then
                strSQL = "SELECT '*' " & _
                "FROM billing_mstr bm,billing_details bd,user_mstr,user_group_mstr,users_usrgrp " & _
                "WHERE(bm.bm_invoicE_no = bd.bm_invoice_no) " & _
                "AND um_user_id = bm_created_by " & _
                "AND um_user_id = uu_user_id " & _
                "AND uu_usrgrp_id = ugm_usrgrp_id " & _
                "AND ugm_fixed_role IN ('Billing Officer','Billing Approving Officer') " & _
                "AND bm_dn_no = '" & txtTaxInvoice.Text & "'"
            Else
                strSQL = "SELECT '*' FROM billing_details WHERE bm_dn_NO = '" & txtTaxInvoice.Text & "' "
            End If
            strTaxInv = objDb.GetVal(strSQL)
            If Not strTaxInv.Trim.Length = 0 Then
                printBillingPDFReports(txtTaxInvoice.Text, type)
            Else
                Common.NetMsgbox(Me, IIf(type = "CN", "Credit Note", IIf(type = "DN", "Debit Note", IIf(type = "DA", "Devit Advice", "Credit Advice"))) & " does not exist.")
            End If
        End If

    End Sub
    Private Sub printBillingPDFReports(ByVal dnNo As String, ByVal docType As String)

        Dim conn As MySqlConnection = Nothing
        Dim cmd As MySqlCommand = Nothing
        Dim da As MySqlDataAdapter = Nothing
        Dim strDt2 = Date.Today.ToString("dd") & Date.Today.ToString("MM") & Today.Year.ToString.Substring(2, 2)
        Dim rdr As MySqlDataReader = Nothing
        Dim myConnectionString As String
        Dim strAryQuery(0) As String
        Dim i, j As Integer
        Dim i2 As Integer
        Dim a As String = ""
        Dim strDNNo As String = ""
        Dim strDNPrefix As String = ""
        Dim strDNLastUsedNo As String = ""
        Dim strBRCoyName As String = ""
        Dim strVendorID As String = ""
        Dim intIncrement As Integer
        Dim strSQLDN As String
        Dim dsDN As New DataSet
        Dim billingNo As String = ""
        myConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("eProcurePath") & ";"

        conn = New MySqlConnection(myConnectionString)

        Try
            conn.Open()

            Dim dtDoc, dtDocForeign As New DataTable
            Dim dsDoc, dsDocForeign As New DataSet
            Dim ds As New DataSet

            cmd = New MySqlCommand
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text
                If docType = "CN" Or docType = "DN" Then
                    .CommandText = "SELECT if(bm_gst_value = 0.00,bm_gst_output_tax_code,'ZRE') as 'bm_gst_output_tax_code', bm.bm_currency_code, cm_tax_reg_no, bm.bm_s_coy_id, bd.bm_gst_value, ifnull(bm.bm_exchange_rate,0) 'bm_exchange_rate', bm.bm_created_by, bd.bm_invoice_no, bd.bm_dn_no,IF((SELECT DISTINCT tax_perc FROM tax_mstr,tax WHERE tax_code = tm_tax_rate AND tm_tax_code = bd.bm_gst_output_tax_code AND tm_coy_id = bm.bm_b_coy_id) = 6.00,CONCAT('*  ', bm_product_desc),CONCAT('** ', bm_product_desc)) 'Description', " _
                    & "bm_received_qty, bm_unit_cost, bm_received_qty * bm_unit_cost 'TotalRM', " _
                    & "IF((SELECT DISTINCT tax_perc FROM tax_mstr,tax WHERE tax_code = tm_tax_rate AND tm_tax_code = bd.bm_gst_output_tax_code AND tm_coy_id = bm.bm_b_coy_id) = 6.00, bm_received_qty * bm_unit_cost,0) 'GSTSumm6', " _
                    & "IF((SELECT DISTINCT tax_perc FROM tax_mstr,tax WHERE tax_code = tm_tax_rate AND tm_tax_code = bd.bm_gst_output_tax_code AND tm_coy_id = bm.bm_b_coy_id) = 6.00, bm_received_qty * bm_unit_cost * IF(TRIM(bm.bm_exchange_rate) = '0' OR bm.bm_exchange_rate IS NULL,0,bm.bm_exchange_rate),0) 'ForeignGSTSumm6', " _
                    & "IF((SELECT DISTINCT tax_perc FROM tax_mstr,tax WHERE tax_code = tm_tax_rate AND tm_tax_code = bd.bm_gst_output_tax_code AND tm_coy_id = bm.bm_b_coy_id) = 0.00, bm_received_qty * bm_unit_cost,0) 'GSTSumm0', " _
                    & "IF((SELECT DISTINCT tax_perc FROM tax_mstr,tax WHERE tax_code = tm_tax_rate AND tm_tax_code = bd.bm_gst_output_tax_code AND tm_coy_id = bm.bm_b_coy_id) = 6.00, bm_gst_value,0) 'GSTSummTaxValue', " _
                    & "IF((SELECT DISTINCT tax_perc FROM tax_mstr,tax WHERE tax_code = tm_tax_rate AND tm_tax_code = bd.bm_gst_output_tax_code AND tm_coy_id = bm.bm_b_coy_id) = 6.00, bm_gst_value * IF(TRIM(bm.bm_exchange_rate) = '0' OR bm.bm_exchange_rate IS NULL,0,bm.bm_exchange_rate) ,0) 'ForeignGSTSummTaxValue', " _
                    & "bm_received_qty * bm_unit_cost * IF(TRIM(bm.bm_exchange_rate) = '0' OR bm.bm_exchange_rate IS NULL,0,bm.bm_exchange_rate) 'TotalExcludeGSTMYR', " _
                    & "bm_received_qty * bm_unit_cost * IF(TRIM(bm.bm_exchange_rate) = '0' OR bm.bm_exchange_rate IS NULL,0,bm.bm_exchange_rate) + (bm_gst_value * IF(TRIM(bm.bm_exchange_rate) = '0' OR bm.bm_exchange_rate IS NULL,0,bm.bm_exchange_rate)) 'TotalMYR', " _
                    & "bm_received_qty * bm_unit_cost as 'TotalExcludeGSTForeign', " _
                    & "ic.ic_coy_name 'ic_coy_name', " _
                    & "CONCAT(TRIM(ic_addr_line1), " _
                    & "IF(TRIM(ic_addr_line2) = '' OR ic_addr_line2 IS NULL, '', CONCAT(', \n',ic_addr_line2)), " _
                    & "IF(TRIM(ic_addr_line3) = '' OR ic_addr_line3 IS NULL, '', CONCAT(', \n',ic_addr_line3)), " _
                    & "IF(TRIM(ic_postcode) = '' OR ic_postcode IS NULL, '', CONCAT(', \n',ic_postcode)), " _
                    & "IF(TRIM(ic_city) = '' OR ic_city IS NULL, '', CONCAT(', ',ic_city)), " _
                    & "', \n', IFNULL((SELECT CODE_DESC FROM code_mstr WHERE code_abbr = ic_state AND code_value = ic_country AND code_category = 'S' AND code_deleted = 'N'),'n.a'), " _
                    & "', ', IFNULL((SELECT CODE_DESC FROM code_mstr WHERE code_abbr = ic_country AND code_category = 'CT' AND code_deleted = 'N'),'n.a')) " _
                    & "'PurchaserAddress', ic_phone, ic_fax, " _
                    & "concat(cm.cm_coy_name,' (',cm_business_reg_no, ')') 'cm_coy_name', " _
                    & "CONCAT(TRIM(cm_addr_line1), " _
                    & "IF(TRIM(cm_addr_line2) = '' OR cm_addr_line2 IS NULL, '', CONCAT(', ',cm_addr_line2)),  " _
                    & "IF(TRIM(cm_addr_line3) = '' OR cm_addr_line3 IS NULL, '', CONCAT(', ',cm_addr_line3)),  " _
                    & "IF(TRIM(cm_postcode) = '' OR cm_postcode IS NULL, '', CONCAT(', ',cm_postcode)),  " _
                    & "IF(TRIM(cm_city) = '' OR cm_city IS NULL, '', CONCAT(', ',cm_city)),  " _
                    & "', ', IFNULL((SELECT CODE_DESC FROM code_mstr WHERE code_abbr = cm_state AND code_value = cm_country AND code_category = 'S' AND code_deleted = 'N'),'n.a'),  " _
                    & "', ', IFNULL((SELECT CODE_DESC FROM code_mstr WHERE code_abbr = cm_country AND code_category = 'CT' AND code_deleted = 'N'),'n.a'), ' (',cm_business_reg_no, ')') " _
                    & "'SupplierAddress', cm_phone, cm_fax, bm.bm_b_coy_id as 'bm_b_coy_id', bm.bm_invoice_index as 'bm_invoice_index', tax_perc,bm2.bm_invoice_no 'invoice_no', DATE_FORMAT(bm2.bm_created_on,'%d-%m-%Y') 'invoice_date', cm_acct_no " _
                    & "FROM billing_mstr bm, billing_details bd, ipp_company ic, company_mstr cm, tax, tax_mstr, billing_mstr bm2  " _
                    & "WHERE(bm.bm_invoice_status = 6) " _
                    & "AND bd.bm_invoice_no = bm.bm_invoice_no AND bd.bm_s_coy_id = bm.bm_s_coy_id  " _
                    & "AND ic.ic_index = bd.bm_s_coy_id  " _
                    & "AND cm_coy_id = bm.bm_b_coy_id " _
                    & "AND bm_gst_output_tax_code = tm_tax_code " _
                    & "AND tm_coy_id = bm.bm_b_coy_id " _
                    & "AND tm_tax_rate = tax_code " _
                    & "AND bd.bm_ref_no = bm2.bm_invoice_no " _
                    & "AND bd.bm_dn_no = '" & dnNo & "' and bm.bm_printed_ind = 'Y' " _
                    & "ORDER BY bm.bm_invoice_no ASC "
                Else
                    .CommandText = "SELECT if(bm_gst_value = 0.00,bm_gst_output_tax_code,'ZRE') as 'bm_gst_output_tax_code', bm.bm_currency_code, cm_tax_reg_no, bm.bm_s_coy_id, bd.bm_gst_value, ifnull(bm.bm_exchange_rate,0) 'bm_exchange_rate', bm.bm_created_by, bd.bm_invoice_no, bd.bm_dn_no,IF((SELECT DISTINCT tax_perc FROM tax_mstr,tax WHERE tax_code = tm_tax_rate AND tm_tax_code = bd.bm_gst_output_tax_code AND tm_coy_id = bm_b_coy_id) = 6.00,CONCAT('*  ', bm_product_desc),CONCAT('** ', bm_product_desc)) 'Description', " _
                    & "bm_received_qty, bm_unit_cost, bm_received_qty * bm_unit_cost 'TotalRM', " _
                    & "IF((SELECT DISTINCT tax_perc FROM tax_mstr,tax WHERE tax_code = tm_tax_rate AND tm_tax_code = bd.bm_gst_output_tax_code AND tm_coy_id = bm_b_coy_id) = 6.00, bm_received_qty * bm_unit_cost,0) 'GSTSumm6', " _
                    & "IF((SELECT DISTINCT tax_perc FROM tax_mstr,tax WHERE tax_code = tm_tax_rate AND tm_tax_code = bd.bm_gst_output_tax_code AND tm_coy_id = bm_b_coy_id) = 6.00, bm_received_qty * bm_unit_cost * IF(TRIM(bm.bm_exchange_rate) = '0' OR bm.bm_exchange_rate IS NULL,0,bm.bm_exchange_rate),0) 'ForeignGSTSumm6', " _
                    & "IF((SELECT DISTINCT tax_perc FROM tax_mstr,tax WHERE tax_code = tm_tax_rate AND tm_tax_code = bd.bm_gst_output_tax_code AND tm_coy_id = bm_b_coy_id) = 0.00, bm_received_qty * bm_unit_cost,0) 'GSTSumm0', " _
                    & "IF((SELECT DISTINCT tax_perc FROM tax_mstr,tax WHERE tax_code = tm_tax_rate AND tm_tax_code = bd.bm_gst_output_tax_code AND tm_coy_id = bm_b_coy_id) = 6.00, bm_gst_value,0) 'GSTSummTaxValue', " _
                    & "IF((SELECT DISTINCT tax_perc FROM tax_mstr,tax WHERE tax_code = tm_tax_rate AND tm_tax_code = bd.bm_gst_output_tax_code AND tm_coy_id = bm_b_coy_id) = 6.00, bm_gst_value * IF(TRIM(bm.bm_exchange_rate) = '0' OR bm.bm_exchange_rate IS NULL,0,bm.bm_exchange_rate) ,0) 'ForeignGSTSummTaxValue', " _
                    & "bm_received_qty * bm_unit_cost * IF(TRIM(bm.bm_exchange_rate) = '0' OR bm.bm_exchange_rate IS NULL,0,bm.bm_exchange_rate) 'TotalExcludeGSTMYR', " _
                    & "bm_received_qty * bm_unit_cost * IF(TRIM(bm.bm_exchange_rate) = '0' OR bm.bm_exchange_rate IS NULL,0,bm.bm_exchange_rate) + (bm_gst_value * IF(TRIM(bm.bm_exchange_rate) = '0' OR bm.bm_exchange_rate IS NULL,0,bm.bm_exchange_rate)) 'TotalMYR', " _
                    & "bm_received_qty * bm_unit_cost as 'TotalExcludeGSTForeign', " _
                    & "ic.ic_coy_name 'ic_coy_name', " _
                    & "CONCAT(TRIM(ic_addr_line1), " _
                    & "IF(TRIM(ic_addr_line2) = '' OR ic_addr_line2 IS NULL, '', CONCAT(', \n',ic_addr_line2)), " _
                    & "IF(TRIM(ic_addr_line3) = '' OR ic_addr_line3 IS NULL, '', CONCAT(', \n',ic_addr_line3)), " _
                    & "IF(TRIM(ic_postcode) = '' OR ic_postcode IS NULL, '', CONCAT(', \n',ic_postcode)), " _
                    & "IF(TRIM(ic_city) = '' OR ic_city IS NULL, '', CONCAT(', ',ic_city)), " _
                    & "', \n', IFNULL((SELECT CODE_DESC FROM code_mstr WHERE code_abbr = ic_state AND code_value = ic_country AND code_category = 'S' AND code_deleted = 'N'),'n.a'), " _
                    & "', ', IFNULL((SELECT CODE_DESC FROM code_mstr WHERE code_abbr = ic_country AND code_category = 'CT' AND code_deleted = 'N'),'n.a')) " _
                    & "'PurchaserAddress', ic_phone, ic_fax, " _
                    & "concat(cm.cm_coy_name,' (',cm_business_reg_no, ')') 'cm_coy_name', " _
                    & "CONCAT(TRIM(cm_addr_line1), " _
                    & "IF(TRIM(cm_addr_line2) = '' OR cm_addr_line2 IS NULL, '', CONCAT(', ',cm_addr_line2)),  " _
                    & "IF(TRIM(cm_addr_line3) = '' OR cm_addr_line3 IS NULL, '', CONCAT(', ',cm_addr_line3)),  " _
                    & "IF(TRIM(cm_postcode) = '' OR cm_postcode IS NULL, '', CONCAT(', ',cm_postcode)),  " _
                    & "IF(TRIM(cm_city) = '' OR cm_city IS NULL, '', CONCAT(', ',cm_city)),  " _
                    & "', ', IFNULL((SELECT CODE_DESC FROM code_mstr WHERE code_abbr = cm_state AND code_value = cm_country AND code_category = 'S' AND code_deleted = 'N'),'n.a'),  " _
                    & "', ', IFNULL((SELECT CODE_DESC FROM code_mstr WHERE code_abbr = cm_country AND code_category = 'CT' AND code_deleted = 'N'),'n.a'), ' (',cm_business_reg_no, ')') " _
                    & "'SupplierAddress', cm_phone, cm_fax, bm.bm_b_coy_id as 'bm_b_coy_id', bm.bm_invoice_index as 'bm_invoice_index', tax_perc, cm_acct_no " _
                    & "FROM billing_mstr bm, billing_details bd, ipp_company ic, company_mstr cm, tax, tax_mstr " _
                    & "WHERE(bm.bm_invoice_status = 6) " _
                    & "AND bd.bm_invoice_no = bm.bm_invoice_no AND bd.bm_s_coy_id = bm.bm_s_coy_id  " _
                    & "AND ic.ic_index = bd.bm_s_coy_id  " _
                    & "AND cm_coy_id = bm.bm_b_coy_id " _
                    & "AND bm_gst_output_tax_code = tm_tax_code " _
                    & "AND tm_coy_id = bm.bm_b_coy_id " _
                    & "AND tm_tax_rate = tax_code " _
                    & "AND bd.bm_dn_no = '" & dnNo & "' and bm.bm_printed_ind = 'Y' " _
                    & "ORDER BY bm.bm_invoice_no ASC "
                End If


            End With

            da = New MySqlDataAdapter(cmd)
            da.SelectCommand.CommandTimeout = 50000
            da.Fill(ds)

            dtDoc.Columns.Add("No", Type.GetType("System.String"))
            dtDoc.Columns.Add("Description", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_received_qty", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_unit_cost", Type.GetType("System.String"))
            dtDoc.Columns.Add("TotalRM", Type.GetType("System.String"))
            dtDoc.Columns.Add("GSTSumm6", Type.GetType("System.Decimal"))
            dtDoc.Columns.Add("GSTSumm0", Type.GetType("System.Decimal"))
            dtDoc.Columns.Add("GSTSummTaxValue", Type.GetType("System.String"))
            dtDoc.Columns.Add("ForeignGSTSumm6", Type.GetType("System.String"))
            dtDoc.Columns.Add("ForeignGSTSummTaxValue", Type.GetType("System.String"))
            dtDoc.Columns.Add("TotalExcludeGSTMYR", Type.GetType("System.String"))
            dtDoc.Columns.Add("TotalMYR", Type.GetType("System.String"))
            dtDoc.Columns.Add("TotalExcludeGSTForeign", Type.GetType("System.String"))
            dtDoc.Columns.Add("cm_coy_name", Type.GetType("System.String"))
            dtDoc.Columns.Add("SupplierAddress", Type.GetType("System.String"))
            dtDoc.Columns.Add("cm_phone", Type.GetType("System.String"))
            dtDoc.Columns.Add("cm_fax", Type.GetType("System.String"))
            dtDoc.Columns.Add("ic_coy_name", Type.GetType("System.String"))
            dtDoc.Columns.Add("PurchaserAddress", Type.GetType("System.String"))
            dtDoc.Columns.Add("ic_phone", Type.GetType("System.String"))
            dtDoc.Columns.Add("ic_fax", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_invoice_no", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_created_by", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_exchange_rate", Type.GetType("System.String"))
            dtDoc.Columns.Add("header", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_gst_value", Type.GetType("System.Decimal"))
            dtDoc.Columns.Add("bm_dn_no", Type.GetType("System.String"))
            dtDoc.Columns.Add("ConvertedTotal", Type.GetType("System.Decimal"))
            dtDoc.Columns.Add("ConvertedGST", Type.GetType("System.Decimal"))
            dtDoc.Columns.Add("cm_tax_reg_no", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_gst_output_tax_code", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_b_coy_id", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_invoice_index", Type.GetType("System.String"))
            dtDoc.Columns.Add("bm_currency_code", Type.GetType("System.String"))
            dtDoc.Columns.Add("tax_perc", Type.GetType("System.String"))
            dtDoc.Columns.Add("invoice_date", Type.GetType("System.String"))
            dtDoc.Columns.Add("invoice_no", Type.GetType("System.String"))
            dtDoc.Columns.Add("cm_acct_no", Type.GetType("System.String"))



            For i = 0 To ds.Tables(0).Rows.Count - 1
                Dim dtr, dtrForeign As DataRow
                dtr = dtDoc.NewRow()
                dtr("No") = i + 1
                dtr("Description") = ds.Tables(0).Rows(i)("Description")
                dtr("bm_received_qty") = ds.Tables(0).Rows(i)("bm_received_qty")
                dtr("bm_unit_cost") = ds.Tables(0).Rows(i)("bm_unit_cost")
                dtr("TotalRM") = ds.Tables(0).Rows(i)("TotalRM")
                dtr("GSTSumm6") = ds.Tables(0).Rows(i)("GSTSumm6")
                dtr("GSTSumm0") = ds.Tables(0).Rows(i)("GSTSumm0")
                dtr("GSTSummTaxValue") = ds.Tables(0).Rows(i)("GSTSummTaxValue")
                dtr("ForeignGSTSumm6") = ds.Tables(0).Rows(i)("ForeignGSTSumm6")
                dtr("ForeignGSTSummTaxValue") = ds.Tables(0).Rows(i)("ForeignGSTSummTaxValue")
                dtr("TotalExcludeGSTMYR") = ds.Tables(0).Rows(i)("TotalExcludeGSTMYR")
                dtr("TotalMYR") = ds.Tables(0).Rows(i)("TotalMYR")
                dtr("TotalExcludeGSTForeign") = ds.Tables(0).Rows(i)("TotalExcludeGSTForeign")
                dtr("cm_coy_name") = ds.Tables(0).Rows(i)("cm_coy_name")
                dtr("SupplierAddress") = ds.Tables(0).Rows(i)("SupplierAddress")
                dtr("cm_phone") = ds.Tables(0).Rows(i)("cm_phone")
                dtr("cm_fax") = ds.Tables(0).Rows(i)("cm_fax")
                dtr("ic_coy_name") = ds.Tables(0).Rows(i)("ic_coy_name")
                dtr("PurchaserAddress") = ds.Tables(0).Rows(i)("PurchaserAddress")
                dtr("ic_phone") = ds.Tables(0).Rows(i)("ic_phone")
                dtr("ic_fax") = ds.Tables(0).Rows(i)("ic_fax")
                dtr("bm_invoice_no") = ds.Tables(0).Rows(i)("bm_invoice_no")
                dtr("bm_created_by") = ds.Tables(0).Rows(i)("bm_created_by")
                dtr("bm_exchange_rate") = FormatNumber(ds.Tables(0).Rows(i)("bm_exchange_rate"), 4)
                dtr("header") = "Tax Invoice"
                dtr("bm_gst_value") = ds.Tables(0).Rows(i)("bm_gst_value")
                dtr("bm_dn_no") = ds.Tables(0).Rows(i)("bm_dn_no")
                dtr("ConvertedTotal") = CDec(ds.Tables(0).Rows(i)("bm_received_qty")) * CDec(ds.Tables(0).Rows(i)("bm_unit_cost")) _
                * CDec(ds.Tables(0).Rows(i)("bm_exchange_rate"))
                dtr("ConvertedGST") = CDec(ds.Tables(0).Rows(i)("bm_gst_value")) * CDec(ds.Tables(0).Rows(i)("bm_exchange_rate"))
                dtr("cm_tax_reg_no") = ds.Tables(0).Rows(i)("cm_tax_reg_no")
                dtr("bm_gst_output_tax_code") = ds.Tables(0).Rows(i)("bm_gst_output_tax_code")
                dtr("bm_invoice_index") = ds.Tables(0).Rows(i)("bm_invoice_index")
                dtr("bm_currency_code") = ds.Tables(0).Rows(i)("bm_currency_code")
                dtr("tax_perc") = ds.Tables(0).Rows(i)("tax_perc")
                If docType = "CN" Or docType = "DN" Then
                    dtr("invoice_date") = ds.Tables(0).Rows(i)("invoice_date")
                    dtr("invoice_no") = ds.Tables(0).Rows(i)("invoice_no")
                Else
                    dtr("invoice_date") = Date.Now
                    dtr("invoice_no") = " "
                End If
                dtr("cm_acct_no") = ds.Tables(0).Rows(i)("cm_acct_no")

                dtDoc.Rows.Add(dtr)

                strBRCoyName = ds.Tables(0).Rows(i)("ic_coy_name")
                strVendorID = ds.Tables(0).Rows(i)("bm_s_coy_id")
                billingNo = ds.Tables(0).Rows(i)("bm_invoice_no")

            Next

            dsDoc.Tables.Add(dtDoc)
            dsDocForeign.Tables.Add(dtDocForeign)

            Dim strFileName As String
            If dsDoc.Tables(0).Rows.Count > 0 Then
                Dim rptDataSource As New Microsoft.Reporting.WebForms.ReportDataSource("dsBillingReports_DataTable1", dsDoc.Tables(0))
                Dim localreport As New LocalReport

                Select Case docType
                    Case "CN"
                        localreport.ReportPath = dispatcher.direct("Report", "BillingCreditNote.rdlc", "Report")
                    Case "DN"
                        localreport.ReportPath = dispatcher.direct("Report", "BillingDebitNote.rdlc", "Report")
                    Case "CA"
                        localreport.ReportPath = dispatcher.direct("Report", "BillingCreditAdvice.rdlc", "Report")
                    Case "DA"
                        localreport.ReportPath = dispatcher.direct("Report", "BillingDebitAdvice.rdlc", "Report")
                End Select
                localreport.DataSources.Clear()
                localreport.DataSources.Add(rptDataSource)
                localreport.Refresh()

                Dim deviceInfo As String = _
                                "<DeviceInfo>" + _
                                    "  <OutputFormat>EMF</OutputFormat>" + _
                                    "</DeviceInfo>"
                Dim PDF As Byte() = localreport.Render("PDF", deviceInfo, Nothing, Nothing, "pdf", Nothing, Nothing)
                strFileName = ParseSpecialChar(dnNo) & "-" & strDt2 & ".pdf"

                'Return PDF
                Me.Response.Clear()
                Me.Response.ContentType = "application/pdf"
                Me.Response.AddHeader("Content-disposition", "attachment; filename=" & strFileName)
                Me.Response.BinaryWrite(PDF)
                'Me.Response.End()
                HttpContext.Current.ApplicationInstance.CompleteRequest()
            End If

            dtDoc = Nothing
            dsDoc = Nothing
            ds = Nothing

        Catch ex As Exception
            Throw New Exception(ex.ToString)
        Finally
            cmd = Nothing
            If Not IsNothing(rdr) Then
                rdr.Close()
            End If
            If Not IsNothing(conn) Then
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
            conn = Nothing
        End Try

    End Sub
End Class
