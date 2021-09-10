Imports AgoraLegacy
Imports eProcure.Component

Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Drawing

Public Class HardShipClaim
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objStaffClaim As New eProcStaffClaim
    Dim objGlo As New AppGlobals
    Dim objDb As New EAD.DBCom
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSaveSummary As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAddClaim As System.Web.UI.WebControls.Button
    Protected WithEvents cmdDupLine As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidItemLine As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblUserName As System.Web.UI.WebControls.Label
    Protected WithEvents lblCompName As System.Web.UI.WebControls.Label
    Protected WithEvents lblDept As System.Web.UI.WebControls.Label
    Protected WithEvents lblDeptId As System.Web.UI.WebControls.Label
    Protected WithEvents lblScNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblDocDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents ddlSelect As System.Web.UI.WebControls.DropDownList
    Protected WithEvents trInfo As System.Web.UI.HtmlControls.HtmlTableRow

    Dim strMsg As String = ""
    Dim blnEmpty As Boolean = True, blnEmpty2 As Boolean = True, blnEmpty3 As Boolean = True, blnEmpty4 As Boolean = True, blnEmpty5 As Boolean = True
    Dim blnCheck As Boolean = True, blnCheck2 As Boolean = True, blnCheck3 As Boolean = True, blnCheck4 As Boolean = True, _
    blnCheck5 As Boolean = True, blnCheck6 As Boolean = True, blnCheck7 As Boolean = True, blnCheck8 As Boolean = True
    Dim aryNewHardShip As New ArrayList()
    Dim strErrEmpty, strErrEmpty2, strErrEmpty3, strErrEmpty4, strErrEmpty5 As String
    Dim strErrCheck, strErrCheck2, strErrCheck3, strErrCheck4, strErrCheck5, strErrCheck6, strErrCheck7, strErrCheck8 As String

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
        MyBase.Page_Load(sender, e)

        If Not Page.IsPostBack Then
            Session("aryNewHardShip") = Nothing
            lblScNo.Text = "To Be Allocated By System"
            ViewState("index") = Request.QueryString("index")
            ViewState("mode") = Request.QueryString("mode")
            ViewState("Row") = 1
            BuildRow()
            ConstructTableHardship()
            GenerateTab()
            GenerateFormLink()
        End If

        lblMsg.Text = ""
    End Sub

    Private Sub BindDdl()

        ddlSelect.Items.Clear()
        ddlSelect.Items.Add(New ListItem("Transportation Claim Form", "Transportation"))
        ddlSelect.Items.Add(New ListItem("Standby / Shift Allowance Claim Form", "Allowance"))
        ddlSelect.Items.Add(New ListItem("Entertainment Claim Form", "Entertain"))
        ddlSelect.Items.Add(New ListItem("Hardship Claim Form", "Hardship"))
        ddlSelect.Items.Add(New ListItem("Overtime Claim Form", "Overtime"))
        ddlSelect.Items.Add(New ListItem("Outstation Claim Form", "Outstation"))
        ddlSelect.Items.Add(New ListItem("Other Claim Form", "Other"))
        If ViewState("index") <> "" Then
            ddlSelect.Items.Add(New ListItem("Claim Summary Sheet", "Summary"))
        End If
        ddlSelect.SelectedValue = "Hardship"
    End Sub

    Private Sub BuildRow()
        Dim i, intCount As Integer
        Dim dsHardShip As DataSet
        Dim objUser As New Users
        Dim objUserDetails As New User

        BindDdl()
        aryNewHardShip.Clear()

        dsHardShip = objStaffClaim.GetHardshipDetails(ViewState("index"))

        If dsHardShip.Tables(0).Rows.Count > 0 Then
            lblDocDate.Text = Format(dsHardShip.Tables("MSTR").Rows(0)("SCM_CREATED_DATE"), "dd/MM/yyyy")
            ViewState("scno") = Common.parseNull(dsHardShip.Tables("MSTR").Rows(0)("SCM_CLAIM_DOC_NO"))
            lblScNo.Text = ViewState("scno")
            lblStatus.Text = Common.parseNull(dsHardShip.Tables("MSTR").Rows(0)("STATUS_DESC"))

            objUserDetails = objUser.GetUserDetails(dsHardShip.Tables("MSTR").Rows(0)("SCM_STAFF_ID"), Session("CompanyId"))
            lblUserName.Text = objUserDetails.Name & " (" & objUserDetails.UserID & ")" 'User Name
            lblCompName.Text = Session("CompanyName") 'Company Name
            lblDept.Text = Common.parseNull(objUserDetails.DeptName) 'Department Name
            lblDeptId.Text = Common.parseNull(objUserDetails.DeptID)
            If Session("urlreferer") = "SearchSCAO" Or Session("urlreferer") = "SearchSCAll" Or Session("urlreferer") = "SCTrankingList_All" Then
                cmdAddClaim.Visible = False
                cmdDupLine.Visible = False
                cmdSave.Visible = False
                cmdSaveSummary.Visible = False
                cmdClear.Visible = False
                ViewState("SCmode") = "v"
                trInfo.Style("display") = "none"
            Else
                If dsHardShip.Tables("MSTR").Rows(0)("SCM_STATUS") <> SCStatus.DraftSC And dsHardShip.Tables("MSTR").Rows(0)("SCM_STATUS") <> SCStatus.Rejected Then
                    cmdAddClaim.Visible = False
                    cmdDupLine.Visible = False
                    cmdSave.Visible = False
                    cmdSaveSummary.Visible = False
                    cmdClear.Visible = False
                    ViewState("SCmode") = "v"
                    trInfo.Style("display") = "none"
                End If
            End If
            If dsHardShip.Tables("DETAILS").Rows.Count > ViewState("Row") Then
                intCount = dsHardShip.Tables("DETAILS").Rows.Count
            Else
                intCount = ViewState("Row")
            End If
        Else
            objUserDetails = objUser.GetUserDetails(Session("UserId"), Session("CompanyId"))
            lblUserName.Text = objUserDetails.Name & " (" & objUserDetails.UserID & ")" 'User Name
            lblCompName.Text = Session("CompanyName") 'Company Name
            lblDept.Text = Common.parseNull(objUserDetails.DeptName) 'Department Name
            lblDeptId.Text = Common.parseNull(objUserDetails.DeptID)

            intCount = ViewState("Row")
        End If
    
        For i = 0 To intCount - 1
            'Checkbox, Date From, Time From, Date To, Time To, Project Code, Purpose, Break Hours, Total Hours, Rate, Total, CSR No, Call Day, Call Period, Call Follow Up
            aryNewHardShip.Add(New String() {"N", "", "00:00", "", "00:00", "", "", "0", "", "", "", "", "", "", ""})
        Next

        'Put all data in array if record found
        If dsHardShip.Tables("DETAILS").Rows.Count > 0 Then
            For i = 0 To dsHardShip.Tables("DETAILS").Rows.Count - 1
                aryNewHardShip(i)(1) = Common.parseNull(dsHardShip.Tables("DETAILS").Rows(i)("SCHD_DATE_FROM")) 'Date From
                aryNewHardShip(i)(2) = Common.parseNull(dsHardShip.Tables("DETAILS").Rows(i)("SCHD_FROM_TIME")) 'Time From 
                aryNewHardShip(i)(3) = Common.parseNull(dsHardShip.Tables("DETAILS").Rows(i)("SCHD_DATE_TO")) 'Date To
                aryNewHardShip(i)(4) = Common.parseNull(dsHardShip.Tables("DETAILS").Rows(i)("SCHD_TO_TIME")) 'Time To 
                aryNewHardShip(i)(5) = Common.parseNull(dsHardShip.Tables("DETAILS").Rows(i)("SCHD_PROJ_CODE")) 'Project Code
                aryNewHardShip(i)(6) = Common.parseNull(dsHardShip.Tables("DETAILS").Rows(i)("SCHD_PURPOSE")) 'Purpose
                aryNewHardShip(i)(7) = Common.parseNull(dsHardShip.Tables("DETAILS").Rows(i)("SCHD_BREAK_HOUR")) 'Break Hours
                aryNewHardShip(i)(8) = Common.parseNull(dsHardShip.Tables("DETAILS").Rows(i)("SCHD_TOTAL_HOUR")) 'Total Hours
                aryNewHardShip(i)(9) = Common.parseNull(dsHardShip.Tables("DETAILS").Rows(i)("SCHD_RATE")) 'Hardship Allowance Rate
                aryNewHardShip(i)(10) = Common.parseNull(dsHardShip.Tables("DETAILS").Rows(i)("SCHD_AMOUNT")) 'Amount
                aryNewHardShip(i)(11) = Common.parseNull(dsHardShip.Tables("DETAILS").Rows(i)("SCHD_CSR_NO")) 'CSR No
                aryNewHardShip(i)(12) = Common.parseNull(dsHardShip.Tables("DETAILS").Rows(i)("SCHD_CALL_DAY")) 'No of calls attended on that day
                aryNewHardShip(i)(13) = Common.parseNull(dsHardShip.Tables("DETAILS").Rows(i)("SCHD_CALL_PERIOD")) 'No of calls attended during HC period
                aryNewHardShip(i)(14) = Common.parseNull(dsHardShip.Tables("DETAILS").Rows(i)("SCHD_CALL_FOLLOW_UP")) 'No of calls to be follow up
            Next
        End If

    End Sub

    Private Sub ConstructTableHardship()
        Dim table As String
        Dim strrow As String = ""
        Dim i, c, intHour, intMin, count As Integer
        Dim blnChk As Boolean = True
        Dim aryBH, aryRate, aryMin As New ArrayList()
        aryRate.Add("17.00")
        aryRate.Add("23.00")

        aryBH.Add("1")
        aryBH.Add("2")

        aryMin.Add("00")
        aryMin.Add("15")
        aryMin.Add("30")
        aryMin.Add("45")

        count = aryNewHardShip.Count
        hidItemLine.Value = count
       
        For i = 0 To count - 1
            strrow &= "<tr>"

            strrow &= "<td align=""center"" class=""tablecol"">"
            If aryNewHardShip(i)(0) = "Y" Then
                strrow &= "<input type=""checkbox"" id=""chkSelection" & i & """ checked name=""chkSelection" & i & """ onclick=""checkChild('chkSelection" & i & "')"">"
            Else
                strrow &= "<input type=""checkbox"" id=""chkSelection" & i & """ name=""chkSelection" & i & """ onclick=""checkChild('chkSelection" & i & "')"">"
                blnChk = False
            End If
            strrow &= "</td>"

            strrow &= "<td class=""tablecol"" align=""center"">" & i + 1
            strrow &= "</td>"

            'Date From
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:80px;margin-right:0px; "" contentEditable=""false"" class=""txtbox"" type=""text"" id=""txtDateFr" & i & """ name=""txtDateFr" & i & """ value=""" & aryNewHardShip(i)(1) & """>"
            strrow &= "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateFr" & i) & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            strrow &= "</td>"

            'Time From
            strrow &= "<td class=""tablecol"">"
            strrow &= "<select class=""ddl"" style=""width:48%;margin-right:0px;"" onchange =""CalTotalHours(" & i & ");"" id=""cboHourFrom" & i & """ name=""cboHourFrom" & i & """>"
            For intHour = 0 To 23
                If Left(aryNewHardShip(i)(2), 2) = Format(intHour, "00").ToString Then
                    strrow &= "<option value=""" & Format(intHour, "00").ToString & """ selected=""selected"">" & Format(intHour, "00").ToString & "</option>"
                Else
                    strrow &= "<option value=""" & Format(intHour, "00").ToString & """>" & Format(intHour, "00").ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "<select class=""ddl"" style=""width:48%;margin-right:0px;"" onchange =""CalTotalHours(" & i & ");"" id=""cboMinFrom" & i & """ name=""cboMinFrom" & i & """>"
            For c = 0 To aryMin.Count - 1
                If Right(aryNewHardShip(i)(2), 2) = aryMin(c).ToString Then
                    strrow &= "<option value=""" & aryMin(c).ToString & """ selected=""selected"">" & aryMin(c).ToString & "</option>"
                Else
                    strrow &= "<option value=""" & aryMin(c).ToString & """>" & aryMin(c).ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "</td>"

            'Date To
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:80px;margin-right:0px; "" contentEditable=""false"" class=""txtbox"" type=""text"" id=""txtDateTo" & i & """ name=""txtDateTo" & i & """ value=""" & aryNewHardShip(i)(3) & """>"
            strrow &= "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateTo" & i) & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            strrow &= "</td>"

            'Time To
            strrow &= "<td class=""tablecol"">"
            strrow &= "<select class=""ddl"" style=""width:48%;margin-right:0px;"" onchange =""CalTotalHours(" & i & ");"" id=""cboHourTo" & i & """ name=""cboHourTo" & i & """>"
            For intHour = 0 To 23
                If Left(aryNewHardShip(i)(4), 2) = Format(intHour, "00").ToString Then
                    strrow &= "<option value=""" & Format(intHour, "00").ToString & """ selected=""selected"">" & Format(intHour, "00").ToString & "</option>"
                Else
                    strrow &= "<option value=""" & Format(intHour, "00").ToString & """>" & Format(intHour, "00").ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "<select class=""ddl"" style=""width:48%;margin-right:0px;"" onchange =""CalTotalHours(" & i & ");"" id=""cboMinTo" & i & """ name=""cboMinTo" & i & """>"
            For c = 0 To aryMin.Count - 1
                If Right(aryNewHardShip(i)(4), 2) = aryMin(c).ToString Then
                    strrow &= "<option value=""" & aryMin(c).ToString & """ selected=""selected"">" & aryMin(c).ToString & "</option>"
                Else
                    strrow &= "<option value=""" & aryMin(c).ToString & """>" & aryMin(c).ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "</td>"

            'Project Code
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:160px;margin-right:0px; "" maxlength=""50"" class=""txtbox"" type=""text"" id=""txtPC" & i & """ name=""txtPC" & i & """ value=""" & aryNewHardShip(i)(5) & """>"
            strrow &= "</td>"

            'Purpose
            strrow &= "<td class=""tablecol"">"
            strrow &= "<textarea rows=""2"" style=""font-family:Arial,Verdana;font-size:11px;width:200px;margin-right:0px; "" maxlength=""500"" id=""txtPurpose" & i & """ name=""txtPurpose" & i & """>" & aryNewHardShip(i)(6) & "</textarea>"
            strrow &= "</td>"

            'Break Hours
            strrow &= "<td class=""tablecol"">"
            strrow &= "<select class=""ddl"" style=""width:100%;margin-right:0px;"" onchange =""CalTotalHours(" & i & ")"" id=""cboBH" & i & """ name=""cboBH" & i & """>"
            strrow &= "<option value=""0"" selected=""selected"">" & "--Select--" & "</option>"
            For c = 0 To aryBH.Count - 1
                If aryNewHardShip(i)(7) = aryBH(c) Then
                    strrow &= "<option value=""" & aryBH(c).ToString & """ selected=""selected"">" & aryBH(c).ToString & "</option>"
                Else
                    strrow &= "<option value=""" & aryBH(c).ToString & """>" & aryBH(c).ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "</td>"

            'Total Hours
            strrow &= "<td class=""tablecol"">"
            strrow &= "<label id=""lblHour" & i & """ style=""width:100%;margin-right:0px; text-align:right;"">" & aryNewHardShip(i)(8) & "</label>"
            strrow &= "</td>"

            'Hardship Allw. Rate
            strrow &= "<td class=""tablecol"">"
            strrow &= "<select class=""ddl"" style=""width:100%;margin-right:0px;"" onchange =""CalTotal(" & i & ")"" id=""cboRate" & i & """ name=""cboRate" & i & """>"
            strrow &= "<option value="""" selected=""selected"">" & "--Select--" & "</option>"
            For c = 0 To aryRate.Count - 1
                If aryNewHardShip(i)(9) = aryRate(c).ToString Then
                    strrow &= "<option value=""" & aryRate(c).ToString & """ selected=""selected"">MYR" & aryRate(c).ToString & "</option>"
                Else
                    strrow &= "<option value=""" & aryRate(c).ToString & """>MYR" & aryRate(c).ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "</td>"

            'Total Hours * Rate
            strrow &= "<td class=""tablecol"">"
            strrow &= "<label id=""lblTotal" & i & """ style=""width:100%;margin-right:0px; text-align:right;"">" & aryNewHardShip(i)(10) & "</label>"
            strrow &= "</td>"

            'CSR No
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:120px;margin-right:0px; "" maxlength=""50"" class=""txtbox"" type=""text"" id=""txtCSRNo" & i & """ name=""txtCSRNo" & i & """ value=""" & aryNewHardShip(i)(11) & """>"
            strrow &= "</td>"

            'No of Call attended on that day
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:120px;margin-right:0px; "" onkeypress=""return isNumberKey(event)"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtCallDay" & i & """ name=""txtCallDay" & i & """ value=""" & aryNewHardShip(i)(12) & """>"
            strrow &= "</td>"

            'No of Call attended during HC period
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:120px;margin-right:0px; "" onkeypress=""return isNumberKey(event)"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtCallPeriod" & i & """ name=""txtCallPeriod" & i & """ value=""" & aryNewHardShip(i)(13) & """>"
            strrow &= "</td>"

            'No of Call to be follow up
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:120px;margin-right:0px; "" onkeypress=""return isNumberKey(event)"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtCallFollowUp" & i & """ name=""txtCallFollowUp" & i & """ value=""" & aryNewHardShip(i)(14) & """>"
            strrow &= "</td>"
            strrow &= "</tr>"

        Next

        Dim a As String = "<span class=""errormsg"">*</span>"
        table = "<table cellspacing=""0"" cellpadding=""0"" width=""1660px"" border=""0"">" & _
                           "<tr class=""TableHeader"">" & _
                           "<td margin-right:0px;"" width=""30px"" align=""center""><input type=""checkbox"" id=""chkAll"" " & IIf(blnChk = True, "checked", "") & " name=""chkAll"" onclick=""selectAll();""></td>" & _
                           "<td margin-right:0px;"" width=""30px"">Line</td>" & _
                           "<td margin-right:0px;"" width=""120px"">Date From" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""100px"">Time From" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""120px"">Date To" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""100px"">Time To" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""160px"">Project Code</td>" & _
                           "<td margin-right:0px;"" width=""200px"">Purpose" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""90px"">Meal Break Hours</td>" & _
                           "<td margin-right:0px;"" width=""70px"" align=""right"">Total Hours</td>" & _
                           "<td margin-right:0px;"" width=""90px"">Hardship Allw. Rate" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""70px"" align=""right"">Total Amount (MYR)</td>" & _
                           "<td margin-right:0px;"" width=""120px"">CSR No. or Consignment Note</td>" & _
                           "<td margin-right:0px;"" width=""120px"" align=""right"">No. of calls attended on that day</td>" & _
                           "<td margin-right:0px;"" width=""120px"" align=""right"">No. of calls attended during HC period</td>" & _
                           "<td margin-right:0px;"" width=""120px"" align=""right"">No. of calls to be follow up</td></tr>" & _
                           strrow & _
                           "</table>"

        Session("ConstructTableHardship") = table
        Session("aryNewHardShip") = aryNewHardShip
    End Sub

    Private Function chkInt(ByVal strValue As String) As Boolean
        If IsNumeric(strValue) And Regex.IsMatch(Trim(strValue), "^-?[0-9]+$") Then
            chkInt = True
        Else
            chkInt = False
        End If
    End Function

    Private Function CalTotalHours(ByVal strDateFrom As String, ByVal strTimeFrom As String, ByVal strDateTo As String, ByVal strTimeTo As String, ByVal strBH As String) As String
        Dim intHourFrom, intMinFrom, intHourTo, intMinTo As Integer
        Dim decHour, decMin As Decimal
        Dim decTotalHour As Decimal = 0
        Dim dtFrom, dtTo As DateTime
        Dim ts As TimeSpan

        intHourFrom = CInt(Left(strTimeFrom, 2))
        intMinFrom = CInt(Right(strTimeFrom, 2))
        intHourTo = CInt(Left(strTimeTo, 2))
        intMinTo = CInt(Right(strTimeTo, 2))

        CalTotalHours = ""
        If strDateFrom <> "" And strDateTo <> "" Then
            dtFrom = DateTime.ParseExact(strDateFrom & " " & strTimeFrom & ":00", "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)
            dtTo = DateTime.ParseExact(strDateTo & " " & strTimeTo & ":00", "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)

            If dtTo > dtFrom Then
                ts = dtTo - dtFrom
                decTotalHour = ts.TotalHours

                If decTotalHour > CDec(strBH) Then
                    decTotalHour = decTotalHour - CDec(strBH)
                Else
                    decTotalHour = 0
                End If

                CalTotalHours = CStr(decTotalHour)
            End If
        End If
    End Function

    Private Function CalTotal(ByVal strTotalHours As String, ByVal strRate As String) As String
        Dim decTotal As Decimal = 0
        CalTotal = ""

        If strTotalHours <> "" And strRate <> "" Then
            decTotal = CDec(strTotalHours) * CDec(strRate)
            CalTotal = CStr(Format(decTotal, "###0.00"))
        End If
    End Function

    Private Sub cmdAddClaim_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddClaim.Click
        Rebuild(False)
        BuildLine()
        ConstructTableHardship()
    End Sub

    Private Sub Rebuild(Optional ByVal blnChk As Boolean = True)
        Dim i As Integer
        blnEmpty = False
        blnEmpty2 = False
        blnEmpty3 = False
        blnEmpty4 = False
        blnEmpty5 = False
        blnCheck = False
        blnCheck2 = False
        blnCheck3 = False
        blnCheck4 = False
        blnCheck5 = False
        blnCheck6 = False
        blnCheck7 = False
        blnCheck8 = False

        aryNewHardShip = Session("aryNewHardShip")
        For i = 0 To aryNewHardShip.Count - 1
            aryNewHardShip(i)(0) = IIf(Request.Form("chkSelection" & i) = "on", "Y", "N") 'Checkbox
            aryNewHardShip(i)(1) = Request.Form("txtDateFr" & i) 'Date From
            aryNewHardShip(i)(2) = Request.Form("cboHourFrom" & i) & ":" & Request.Form("cboMinFrom" & i) 'Time From 
            aryNewHardShip(i)(3) = Request.Form("txtDateTo" & i) 'Date To
            aryNewHardShip(i)(4) = Request.Form("cboHourTo" & i) & ":" & Request.Form("cboMinTo" & i) 'Time To 
            aryNewHardShip(i)(5) = Request.Form("txtPC" & i) 'Project Code
            aryNewHardShip(i)(6) = Request.Form("txtPurpose" & i) 'Purpose
            aryNewHardShip(i)(7) = Request.Form("cboBH" & i) 'Break Hours
            aryNewHardShip(i)(8) = CalTotalHours(aryNewHardShip(i)(1), aryNewHardShip(i)(2), aryNewHardShip(i)(3), aryNewHardShip(i)(4), aryNewHardShip(i)(7)) 'Total Hours
            aryNewHardShip(i)(9) = Request.Form("cboRate" & i) 'Hardship Allowance Rate
            aryNewHardShip(i)(10) = CalTotal(aryNewHardShip(i)(8), aryNewHardShip(i)(9)) 'Amount
            aryNewHardShip(i)(11) = Request.Form("txtCSRNo" & i) 'CSR No
            aryNewHardShip(i)(12) = Request.Form("txtCallDay" & i) 'No of calls attended on that day
            aryNewHardShip(i)(13) = Request.Form("txtCallPeriod" & i) 'No of calls attended during HC period
            aryNewHardShip(i)(14) = Request.Form("txtCallFollowUp" & i) 'No of calls to be follow up
        Next

        If blnChk = True Then
            For i = 0 To aryNewHardShip.Count - 1
                If aryNewHardShip(i)(1) <> "" Or aryNewHardShip(i)(3) <> "" Or aryNewHardShip(i)(5) <> "" Or aryNewHardShip(i)(6) <> "" Or aryNewHardShip(i)(8) <> "" Or _
                aryNewHardShip(i)(9) <> "" Or aryNewHardShip(i)(11) <> "" Or aryNewHardShip(i)(12) <> "" Or aryNewHardShip(i)(13) <> "" Or aryNewHardShip(i)(14) <> "" Then
                    'Error if date from is empty
                    If aryNewHardShip(i)(1) = "" Then
                        blnEmpty = True
                        strErrEmpty &= IIf(strErrEmpty = "", i + 1, "," & i + 1)
                    Else
                        'Error if Date is invalid
                        If DateDiff(DateInterval.Day, CDate(aryNewHardShip(i)(1)), Today.Now()) > 110 Then
                            blnCheck = True
                            strErrCheck &= IIf(strErrCheck = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if date to is empty
                    If aryNewHardShip(i)(3) = "" Then
                        blnEmpty5 = True
                        strErrEmpty5 &= IIf(strErrEmpty5 = "", i + 1, "," & i + 1)
                    Else
                        'Error if Date greater than today's date
                        If CDate(aryNewHardShip(i)(3)) > Today.Now() Then
                            blnCheck5 = True
                            strErrCheck5 &= IIf(strErrCheck5 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if date from greater than date to
                    If (blnEmpty = False And blnEmpty5 = False) Then
                        If CDate(aryNewHardShip(i)(1)) > CDate(aryNewHardShip(i)(3)) Then
                            blnCheck8 = True
                            strErrCheck8 &= IIf(strErrCheck8 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if purpose is empty
                    If aryNewHardShip(i)(6) = "" Then
                        blnEmpty2 = True
                        strErrEmpty2 &= IIf(strErrEmpty2 = "", i + 1, "," & i + 1)
                    Else
                        If Not Common.checkMaxLength(aryNewHardShip(i)(6), 500) Then
                            blnCheck7 = True
                            strErrCheck7 &= IIf(strErrCheck7 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if time from/time to is empty
                    If aryNewHardShip(i)(8) = "" Then
                        blnEmpty3 = True
                        strErrEmpty3 &= IIf(strErrEmpty3 = "", i + 1, "," & i + 1)
                    End If

                    'Error if rate is empty
                    If aryNewHardShip(i)(9) = "" Then
                        blnEmpty4 = True
                        strErrEmpty4 &= IIf(strErrEmpty4 = "", i + 1, "," & i + 1)
                    End If

                    'Error if No of calls attended on that day is invalid
                    If aryNewHardShip(i)(12) <> "" Then
                        If chkInt(aryNewHardShip(i)(12)) = False Then
                            blnCheck2 = True
                            strErrCheck2 &= IIf(strErrCheck2 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if No of calls attended during HC period is invalid
                    If aryNewHardShip(i)(13) <> "" Then
                        If chkInt(aryNewHardShip(i)(13)) = False Then
                            blnCheck3 = True
                            strErrCheck3 &= IIf(strErrCheck3 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if No of calls to be follow up is invalid
                    If aryNewHardShip(i)(14) <> "" Then
                        If chkInt(aryNewHardShip(i)(14)) = False Then
                            blnCheck4 = True
                            strErrCheck4 &= IIf(strErrCheck4 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if amount is zero
                    If aryNewHardShip(i)(10) <> "" Then
                        If CDec(aryNewHardShip(i)(10)) > 0 Then
                        Else
                            blnCheck6 = True
                            strErrCheck6 &= IIf(strErrCheck6 = "", i + 1, "," & i + 1)
                        End If
                    End If
                End If
            Next
        End If

    End Sub

    Private Sub BuildLine()

        'Checkbox, Date From, Time From, Date To, Time To, Project Code, Purpose, Break Hours, Total Hours, Rate, Total, CSR No, Call Day, Call Period, Call Follow Up
        aryNewHardShip.Add(New String() {"N", "", "00:00", "", "00:00", "", "", "0", "", "", "", "", "", "", ""})

    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        ValidateFields("Save")
    End Sub

    Private Sub cmdSaveSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSaveSummary.Click
        ValidateFields("Summary")
    End Sub

    Private Sub ValidateFields(ByVal strMode As String)
        Dim blnGL As Boolean = True
        Dim strFullMsg As String = "<ul type='disc'>"
        Rebuild()

        If blnEmpty = True Then
            strMsg &= displayMsg(strErrEmpty, "Date From " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty5 = True Then
            strMsg &= displayMsg(strErrEmpty5, "Date To " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty2 = True Then
            strMsg &= displayMsg(strErrEmpty2, "Purpose " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty3 = True Then
            strMsg &= displayMsg(strErrEmpty3, "Valid Time From/Time To " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty4 = True Then
            strMsg &= displayMsg(strErrEmpty4, "Hardship Allw. Rate " & objGlo.GetErrorMessage("00001"))
        End If
        If blnCheck = True Then
            strMsg &= displayMsg(strErrCheck, "Date From has exceeded 110-days policy.")
        End If
        If blnCheck2 = True Then
            strMsg &= displayMsg(strErrCheck2, "No of calls attended on that day " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck3 = True Then
            strMsg &= displayMsg(strErrCheck3, "No of calls attended during HC period " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck4 = True Then
            strMsg &= displayMsg(strErrCheck4, "No of calls to be follow up " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck5 = True Then
            strMsg &= displayMsg(strErrCheck5, "Date To " & objGlo.GetErrorMessage("00015") & " today's date.")
        End If
        If blnCheck6 = True Then
            strMsg &= displayMsg(strErrCheck6, "Total Amount cannot be zero.")
        End If
        If blnCheck7 = True Then
            strMsg &= displayMsg(strErrCheck7, "Purpose is over limit.")
        End If
        If blnCheck8 = True Then
            strMsg &= displayMsg(strErrCheck8, "Date From should be <= Date To.")
        End If

        strFullMsg &= strMsg & "</ul>"

        If strMsg <> "" Then
            ConstructTableHardship()
            lblMsg.Text = strFullMsg
            Exit Sub
        Else
            lblMsg.Text = ""
            Save(strMode)
        End If
    End Sub

    Private Function displayMsg(ByVal strAllLine As String, ByVal strMsg As String) As String
        Dim strLine() As String, strFullMsg As String = ""
        Dim i As Integer
        strLine = strAllLine.Split(",")
        For i = 0 To strLine.Length - 1
            strFullMsg &= "<li>" & strLine(i) & ". " & strMsg & "<ul type='disc'></ul></li>"
        Next

        Return strFullMsg

    End Function

    Private Sub cmdDupLine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDupLine.Click
        Dim i As Integer
        Dim aryDup As New ArrayList()
        Rebuild(False)

        For i = 0 To aryNewHardShip.Count - 1
            If aryNewHardShip(i)(0) = "Y" Then
                'Date From, Time From, Date To, Time To, Project Code, Total Hours
                aryDup.Add(New String() {aryNewHardShip(i)(1), aryNewHardShip(i)(2), aryNewHardShip(i)(3), aryNewHardShip(i)(4), aryNewHardShip(i)(5), aryNewHardShip(i)(8)})
            End If
        Next

        If aryDup.Count > 0 Then
            For i = 0 To aryDup.Count - 1
                'Checkbox, Date From, Time From, Date To, Time To, Project Code, Purpose, Break Hours, Total Hours, Rate, Total, CSR No, Call Day, Call Period, Call Follow Up
                aryNewHardShip.Add(New String() {"N", aryDup(i)(0), aryDup(i)(1), aryDup(i)(2), aryDup(i)(3), aryDup(i)(4), "", "0", aryDup(i)(5), "", "", "", "", "", ""})
            Next
        End If

        ConstructTableHardship()
    End Sub

    Private Sub Save(ByVal strMode As String)
        Dim strNewIndex, strNewSCNo As String
        Dim ds As New DataSet
        Dim blnRecord As Boolean
        BindHS(ds, blnRecord)

        If blnRecord = True Then
            If ViewState("index") <> "" Then
                If objStaffClaim.UpdateHardship(ds, ViewState("scno"), ViewState("index")) Then
                    If strMode = "Summary" Then
                        Common.NetMsgbox(Me, "Staff Claim Number " & ViewState("scno") & " has been updated.", dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                    Else
                        Common.NetMsgbox(Me, "Staff Claim Number " & ViewState("scno") & " has been updated.", dDispatcher.direct("StaffClaim", "OverTimeClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                    End If
                Else
                    Common.NetMsgbox(Me, objGlo.GetErrorMessage("00007"), MsgBoxStyle.Information)
                End If
            Else
                If objStaffClaim.chkDocExistMonth = True Then
                    Common.NetMsgbox(Me, "You have already created Staff Claim document for this month.", MsgBoxStyle.Information)
                Else
                    If objStaffClaim.SaveHardship(ds, strNewSCNo, strNewIndex) Then
                        ViewState("index") = strNewIndex
                        ViewState("scno") = strNewSCNo
                        If strMode = "Summary" Then
                            Common.NetMsgbox(Me, "Staff Claim Number " & strNewSCNo & " has been created.", dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                        Else
                            Common.NetMsgbox(Me, "Staff Claim Number " & strNewSCNo & " has been created.", dDispatcher.direct("StaffClaim", "OverTimeClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                        End If
                    Else
                        Common.NetMsgbox(Me, objGlo.GetErrorMessage("00007"), MsgBoxStyle.Information)
                    End If
                End If
            End If
        Else
            Common.NetMsgbox(Me, objGlo.GetErrorMessage("00131"), MsgBoxStyle.Information)
        End If

        ConstructTableHardship()

    End Sub

    Private Sub BindHS(ByRef ds As DataSet, ByRef blnRecord As Boolean)
        Dim dtHSMstr As New DataTable
        Dim dtr As DataRow

        blnRecord = False 'By Default
        dtHSMstr.Columns.Add("DEPT_CODE", Type.GetType("System.String"))
        dtr = dtHSMstr.NewRow
        dtr("DEPT_CODE") = lblDeptId.Text

        dtHSMstr.Rows.Add(dtr)
        ds.Tables.Add(dtHSMstr)

        Dim i As Integer, intLine As Integer = 1
        Dim dtHSDtls As New DataTable
        Dim dtrd As DataRow

        dtHSDtls.Columns.Add("LINE_NO", Type.GetType("System.String"))
        dtHSDtls.Columns.Add("DATE_FROM", Type.GetType("System.String"))
        dtHSDtls.Columns.Add("FROM_TIME", Type.GetType("System.String"))
        dtHSDtls.Columns.Add("DATE_TO", Type.GetType("System.String"))
        dtHSDtls.Columns.Add("TO_TIME", Type.GetType("System.String"))
        dtHSDtls.Columns.Add("PROJ_BASED", Type.GetType("System.String"))
        dtHSDtls.Columns.Add("PROJ_CODE", Type.GetType("System.String"))
        dtHSDtls.Columns.Add("PURPOSE", Type.GetType("System.String"))
        dtHSDtls.Columns.Add("BREAK_HOUR", Type.GetType("System.String"))
        dtHSDtls.Columns.Add("TOTAL_HOUR", Type.GetType("System.String"))
        dtHSDtls.Columns.Add("RATE", Type.GetType("System.String"))
        dtHSDtls.Columns.Add("AMOUNT", Type.GetType("System.String"))
        dtHSDtls.Columns.Add("CSR_NO", Type.GetType("System.String"))
        dtHSDtls.Columns.Add("CALL_DAY", Type.GetType("System.String"))
        dtHSDtls.Columns.Add("CALL_PERIOD", Type.GetType("System.String"))
        dtHSDtls.Columns.Add("CALL_FOLLOW_UP", Type.GetType("System.String"))

        For i = 0 To aryNewHardShip.Count - 1
            If aryNewHardShip(i)(1) <> "" Then
                blnRecord = True
                dtrd = dtHSDtls.NewRow
                dtrd("LINE_NO") = intLine
                dtrd("DATE_FROM") = aryNewHardShip(i)(1) 'Date From
                dtrd("FROM_TIME") = aryNewHardShip(i)(2) & ":00" 'Time From 
                dtrd("DATE_TO") = aryNewHardShip(i)(3) 'Date To
                dtrd("TO_TIME") = aryNewHardShip(i)(4) & ":00" 'Time To 
                If aryNewHardShip(i)(5) <> "" Then
                    dtrd("PROJ_CODE") = aryNewHardShip(i)(5) 'Project Code
                    If aryNewHardShip(i)(5).ToString.Length > 5 Then
                        dtrd("PROJ_BASED") = "Y" 'Project Based
                    Else
                        dtrd("PROJ_BASED") = "N"
                    End If
                Else
                    dtrd("PROJ_BASED") = "N" 'Project Based
                    dtrd("PROJ_CODE") = "" 'Project Code
                End If
                dtrd("PURPOSE") = aryNewHardShip(i)(6) 'Purpose
                dtrd("BREAK_HOUR") = aryNewHardShip(i)(7) 'Break Hours
                dtrd("TOTAL_HOUR") = aryNewHardShip(i)(8) 'Total Hours
                dtrd("RATE") = aryNewHardShip(i)(9) 'Hardship Allowance Rate
                dtrd("AMOUNT") = aryNewHardShip(i)(10) 'Amount
                dtrd("CSR_NO") = IIf(aryNewHardShip(i)(11) = "", "", aryNewHardShip(i)(11)) 'CSR No
                dtrd("CALL_DAY") = IIf(aryNewHardShip(i)(12) = "", "", aryNewHardShip(i)(12)) 'No of calls attended on that day
                dtrd("CALL_PERIOD") = IIf(aryNewHardShip(i)(13) = "", "", aryNewHardShip(i)(13)) 'No of calls attended during HC period
                dtrd("CALL_FOLLOW_UP") = IIf(aryNewHardShip(i)(14) = "", "", aryNewHardShip(i)(14)) 'No of calls to be follow up

                dtHSDtls.Rows.Add(dtrd)
                intLine = intLine + 1
            End If
        Next

        If blnRecord = False And ViewState("index") <> "" Then
            blnRecord = objStaffClaim.chkClaimFormData(ViewState("scno"), "Hardship")
        End If

        ds.Tables.Add(dtHSDtls)
    End Sub

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        If Session("urlreferer") = "SCTrankingList_All" Then
            Session("w_Staff_Claim_tabs") = Nothing
        ElseIf Session("urlreferer") = "SearchSCAO" Then
            Session("w_Staff_Claim_tabs") = "<div class=""t_entity""><ul>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("StaffClaim", "SearchSC_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("StaffClaim", "SearchSC_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "</ul><div></div></div>"
        ElseIf Session("urlreferer") = "SearchSCAll" Then
            Session("w_Staff_Claim_tabs") = "<div class=""t_entity""><ul>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("StaffClaim", "SearchSC_AO.aspx", "pageid=" & strPageId) & """><span>Approval List</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("StaffClaim", "SearchSC_All.aspx", "pageid=" & strPageId) & """><span>Approved / Rejected Listing</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "</ul><div></div></div>"
        ElseIf Session("urlreferer") = "SCTrankingList" And ViewState("SCmode") = "v" Then
            Session("w_Staff_Claim_tabs") = "<div class=""t_entity""><ul>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("StaffClaim", "TransportationClaim.aspx", "pageid=" & strPageId) & """><span>Staff Claim Documents</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("StaffClaim", "StaffClaimTrackingList.aspx", "pageid=" & strPageId) & """><span>Staff Claim Listing</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "</ul><div></div></div>"
        Else
            Session("w_Staff_Claim_tabs") = "<div class=""t_entity""><ul>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("StaffClaim", "TransportationClaim.aspx", "pageid=" & strPageId) & """><span>Staff Claim Documents</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("StaffClaim", "StaffClaimTrackingList.aspx", "pageid=" & strPageId) & """><span>Staff Claim Listing</span></a></li>" & _
                                        "<li><div class=""space""></div></li>" & _
                                        "</ul><div></div></div>"
        End If

    End Sub

    Private Sub ddlSelect_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSelect.SelectedIndexChanged

        If ddlSelect.SelectedItem.Value = "Transportation" Then
            Me.Response.Redirect(dDispatcher.direct("StaffClaim", "TransportationClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
        ElseIf ddlSelect.SelectedItem.Value = "Allowance" Then
            Me.Response.Redirect(dDispatcher.direct("StaffClaim", "AllowanceClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
        ElseIf ddlSelect.SelectedItem.Value = "Entertain" Then
            Me.Response.Redirect(dDispatcher.direct("StaffClaim", "EntertainmentClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
        ElseIf ddlSelect.SelectedItem.Value = "Hardship" Then
            Me.Response.Redirect(dDispatcher.direct("StaffClaim", "HardShipClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
        ElseIf ddlSelect.SelectedItem.Value = "Overtime" Then
            Me.Response.Redirect(dDispatcher.direct("StaffClaim", "OverTimeClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
        ElseIf ddlSelect.SelectedItem.Value = "Summary" Then
            If Session("claimmode") = "app" Then
                Me.Response.Redirect(dDispatcher.direct("StaffClaim", "SCApprDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
            Else
                Me.Response.Redirect(dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
            End If
        ElseIf ddlSelect.SelectedItem.Value = "Other" Then
            Me.Response.Redirect(dDispatcher.direct("StaffClaim", "OtherClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
        ElseIf ddlSelect.SelectedItem.Value = "Outstation" Then
            Me.Response.Redirect(dDispatcher.direct("StaffClaim", "OutstationClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
        End If
    End Sub

    Private Sub GenerateFormLink()
        Dim strTableLink As String
        Dim strIcon As String = "<IMG style=""height:19px; vertical-align:middle; position: relative;"" src=" & dDispatcher.direct("Plugins/Images", "StaffClaimDoc.gif") & ">"

        strTableLink = "<tr><td colspan=""5"">" & _
        "<table cellspacing=""0"" cellpadding=""0"" class=""alltable"">" & _
        "<tr>"
        If ViewState("index") <> "" Then
            If Session("urlreferer") = "SearchSCAO" Or Session("urlreferer") = "SearchSCAll" Then
                strTableLink &= "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "SCApprDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Claim Summary Sheet</strong></a></span></td>"
            Else
                strTableLink &= "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Claim Summary Sheet</strong></a></span></td>"
            End If
        Else
            strTableLink &= "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><strong>Claim Summary Sheet</strong></span></td>"
        End If

        strTableLink &= "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "TransportationClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Transportation Claim Form</strong></a></span></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "AllowanceClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Standby/Shift Allw. Claim Form</strong></a></span></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "EntertainmentClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Entertainment Claim Form</strong></a></span></td>" & _
        "</tr>" & _
        "<tr>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "HardShipClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><font color=""green""><strong>Hardship Claim Form</strong></font></a></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "OverTimeClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Overtime Claim Form</strong></a></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "OutstationClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Outstation Claim Form</strong></a></span></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "OtherClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Other Claim Form</strong></a></span></td>" & _
        "</tr>" & _
        "</table>" & _
        "</td></tr>"
        Session("w_SC_Links") = strTableLink

    End Sub
End Class
