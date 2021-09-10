Imports eProcure.Component
Imports AppCommon

Imports System.Data.SqlClient
Public Class ReportFormat1
    Inherits AppCommon.AppBaseClass
    Dim objDb As New DBAccess.EAD.DBCom
    Protected WithEvents cboCompany As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ValSDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValEDate As System.Web.UI.WebControls.RequiredFieldValidator
    Dim objReport As New eProcure.Component.Report
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    'Protected WithEvents optCom As System.Web.UI.WebControls.RadioButton
    'Protected WithEvents optSelectOne As System.Web.UI.WebControls.RadioButton
    Public Const MsgInvalidDate As String = "Start Date Must Be Earlier Than Or Equal To End Date"
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Radio1 As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    'Protected WithEvents optCom As System.Web.UI.HtmlControls.HtmlInputRadioButton
    'Protected WithEvents optSelectOne As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents label1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents cboCompany1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents optCom As System.Web.UI.WebControls.RadioButton
    Protected WithEvents optSelectOne As System.Web.UI.WebControls.RadioButton
    Protected WithEvents optCom1 As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents optSelectOne1 As System.Web.UI.HtmlControls.HtmlInputRadioButton
    'Protected WithEvents optSelectOneac As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Public Const MsgSelection As String = "Please Select a Vendor"
    Dim dispatcher As New Dispatcher.dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents label As System.Web.UI.WebControls.Label
    Protected WithEvents txtSDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEndDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Dim strType As String

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
        
        'Put user code to initialize the page here
        If Not IsPostBack Then
            BindData()
            optCom.Checked = True
            cboCompany.Enabled = False
        End If

        strType = Me.Request.QueryString("type")

        'If optCom.Checked = True Then
        '    cboCompany.Enabled = False
        '    cboCompany.SelectedIndex = -1

        'ElseIf optSelectOne.Checked = True Then
        '    'cboCompany.SelectedIndex = -1
        '    cboCompany.Enabled = True


        'End If


        Select Case strType
            Case "PO"
                lblHeader.Text = "Detail Of PO By Company"

            Case "PR"
                lblHeader.Text = "Detail Of PR By Company"

            Case "RFQ"
                lblHeader.Text = "Summary Of PO, PR and RFQ By Company"
        End Select

        'optSelectOne.Attributes.Add("onclick", "return Verifycheck();")
        lnkBack.NavigateUrl = Dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId)

    End Sub

    'Public Function BindData()
    '    Dim dvcustom As DropDownList
    '    dvcustom = objReport.DisplayVendor()
    '    cboCompany.Items.Clear()
    '    If Not dvcustom Is Nothing Then
    '        Dim cbolist As New ListItem
    '        Common.FillDdl(cboCompany, "AGM_CONSOLIDATOR", "RAM_ASSIGN_INDEX", dvcustom)
    '        cbolist.Value = ""
    '        cbolist.Text = "---Select---"
    '        cboCompany.Items.Insert(0, cbolist)
    '    End If

    'End Function

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim sdate, edate As String
        'sdate = txtSDate.Text
        'edate = txtEndDate.Text

        Dim crParam As New Hashtable
        'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
        crParam.Add("userIDParam", Session("UserID"))

        If optCom.Checked Then
            crParam.Add("VendorCompanyIDParam", "All")
        Else
            crParam.Add("VendorCompanyIDParam", cboCompany.SelectedValue)
        End If

        'crParam.Add("BCOYID", "MBB")
        'crParam.Add("userIDParam", "MOO")
        'crParam.Add("dateFromParam", txtSDate.Text)
        'crParam.Add("dateToParam", txtEndDate.Text)
        'crParam.Add("dateFrom", txtSDate.Text)
        'crParam.Add("dateTo", txtEndDate.Text)
        Dim startdate As String = Format(CDate(txtSDate.Text), "MM/dd/yyyy")
        Dim Enddate As String = Format(CDate(txtEndDate.Text), "MM/dd/yyyy")


        ' Dim Endate

        Select Case strType
            Case "PO"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                crParam.Add("dateFromParam", startdate)
                crParam.Add("dateToParam", Enddate)
                eProcure.Component.ReportViewer.popCrystalReport(Me, "detailOfPOByCompany2.rpt", crParam)
            Case "PR"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                crParam.Add("dateFromParam", startdate)
                crParam.Add("dateToParam", Enddate)
                eProcure.Component.ReportViewer.popCrystalReport(Me, "detailOfPRByCompany.rpt", crParam)
            Case "RFQ"
                crParam.Add("BCOYID", Session("CompanyIdToken"))
                crParam.Add("dateFrom", startdate)
                crParam.Add("dateTo", Enddate)
                eProcure.Component.ReportViewer.popCrystalReport(Me, "SummaryOfPOPRRFQByCompany2.rpt", crParam)
                'Wheel.Components.ReportViewer.popCrystalReport(Me, "SummaryOfPOPRRFQByCompany2.rpt", crParam)

        End Select

        'Wheel.Components.ReportViewer.popCrystalReport(Me, "report1.rpt")
        'CRviewer.popCrystalReport(Me, "SAMPLE.rpt", crParam)

        'If optCom.Checked Then

        '    Response.Redirect("ReportSelection.aspx?pageid=" & strPageId & "&sd=" & txtSDate.Text & "&ed=" & txtEndDate.Text & "&ven=" & optCom.ID)

        'Else
        '    Response.Redirect("ReportSelection.aspx?pageid=" & strPageId & "&sd=" & txtSDate.Text & "&ed=" & txtEndDate.Text & "&ven=" & cboCompany.SelectedItem.Value)
        'End If

        'If sdate < edate Then
        '    'Dim strselect As String = cboCompany.SelectedItem.Text
        '    'viewstate("valselect") = strselect
        '    'Response.Write("End Date cannot earlier than Start Date!")

        '    'If optCom.Checked Then
        '    '    Response.Redirect("ReportSelection.aspx?pageid=" & strPageId & "&sd=" & txtSDate.Text & "&ed=" & txtEndDate.Text & "&ven=" & optCom.Text)



        '    '    'ElseIf optSelectOne.Checked Then

        '    '    'Response.Redirect("ReportSelection.aspx?pageid=" & strPageId & "&sd=" & txtSDate.Text & "&ed=" & txtEndDate.Text & "&ven=" & cboCompany.SelectedItem.Value)
        '    '    'Response.Redirect("ReportSelection.aspx?pageid=" & strPageId & "&sd=" & txtSDate.Text & "&ed=" & txtEndDate.Text & "&ven=" & viewstate("valselect"))
        '    'Else
        '    '    Response.Redirect("ReportSelection.aspx?pageid=" & strPageId & "&sd=" & txtSDate.Text & "&ed=" & txtEndDate.Text & "&ven=" & cboCompany.SelectedItem.Value)
        '    '    'Common.NetMsgbox(Me, MsgSelection, MsgBoxStyle.Information, "Wheel")
        '    'End If

        'Else
        '    Common.NetMsgbox(Me, MsgInvalidDate, MsgBoxStyle.Information, "Wheel")

        'End If
        'Response.Redirect("ReportSelection.aspx?pageid=" & strPageId & "&sd=" & txtSDate.Text & "&ed=" & txtEndDate.Text & "&ven=" & cboCompany.SelectedItem.Value)


    End Sub

    Public Function BindData()
        Dim dvcustom As DataSet
        Dim objreport As New Report


        'dvcustom = objreport.DisplayVendor(True)
        dvcustom = objreport.DisplayVendor(True)
        '  cboCompany.Items.Clear()
        If Not dvcustom Is Nothing Then
            Dim cbolist As New ListItem
            ' Common.FillDdl(cboCompany, "AGM_CONSOLIDATOR", "RAM_ASSIGN_INDEX", dvcustom)
            'Common.FillDdl(cboCompany, "CV_S_COY_ID", "CV_S_COY_ID", dvcustom, "--- Select ---")
            Common.FillDdl(cboCompany, "CV_S_COY_ID", "CV_S_COY_ID", dvcustom)
            cbolist.Value = ""
            cbolist.Text = "---Select---"
            cboCompany.Items.Insert(0, cbolist)
            'Dim lstItem As New ListItem
            'lstItem.Value = ""
            'lstItem.Text = "---Select---"
            'cboCompany.Items.Insert(0, lstItem)
        End If
        objreport = Nothing

    End Function

    Private Sub optCom_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optCom.CheckedChanged
        cboCompany.Enabled = False
        cboCompany.SelectedIndex = -1
    End Sub

    
    Private Sub optSelectOne_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optSelectOne.CheckedChanged
        cboCompany.Enabled = True
    End Sub

    'Private Sub cmdClear_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.ServerClick
    '    txtSDate.Text = ""
    '    txtEndDate.Text = ""
    'End Sub
End Class
