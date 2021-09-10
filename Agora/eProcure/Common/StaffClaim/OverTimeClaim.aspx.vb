Imports AgoraLegacy
Imports eProcure.Component

Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Drawing

Public Class OverTimeClaim
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
    blnCheck5 As Boolean = True, blnCheck6 As Boolean = True
    Dim aryNewOverTime As New ArrayList()
    Dim strErrEmpty, strErrEmpty2, strErrEmpty3, strErrEmpty4, strErrEmpty5, strErrCheck, strErrCheck2, strErrCheck3, strErrCheck4, strErrCheck5, strErrCheck6 As String

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
            Session("aryNewOverTime") = Nothing
            lblScNo.Text = "To Be Allocated By System"
            ViewState("index") = Request.QueryString("index")
            ViewState("mode") = Request.QueryString("mode")
            ViewState("Row") = 1
            BuildRow()
            ConstructTableOverTime()
            GenerateTab()
            GenerateFormLink()
        End If

        lblMsg.Text = ""
        cmdDupLine.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")

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
        ddlSelect.SelectedValue = "Overtime"
    End Sub

    Private Sub BuildRow()
        Dim i, intCount As Integer
        Dim dsOverTime As DataSet
        Dim objUser As New Users
        Dim objUserDetails As New User

        BindDdl()
        aryNewOverTime.Clear()

        dsOverTime = objStaffClaim.GetOverTimeDetails(ViewState("index"))

        If dsOverTime.Tables("MSTR").Rows.Count > 0 Then
            lblDocDate.Text = Format(dsOverTime.Tables("MSTR").Rows(0)("SCM_CREATED_DATE"), "dd/MM/yyyy")
            ViewState("scno") = Common.parseNull(dsOverTime.Tables("MSTR").Rows(0)("SCM_CLAIM_DOC_NO"))
            lblScNo.Text = ViewState("scno")
            lblStatus.Text = Common.parseNull(dsOverTime.Tables("MSTR").Rows(0)("STATUS_DESC"))

            objUserDetails = objUser.GetUserDetails(dsOverTime.Tables("MSTR").Rows(0)("SCM_STAFF_ID"), Session("CompanyId"))
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
                If dsOverTime.Tables("MSTR").Rows(0)("SCM_STATUS") <> SCStatus.DraftSC And dsOverTime.Tables("MSTR").Rows(0)("SCM_STATUS") <> SCStatus.Rejected Then
                    cmdAddClaim.Visible = False
                    cmdDupLine.Visible = False
                    cmdSave.Visible = False
                    cmdSaveSummary.Visible = False
                    cmdClear.Visible = False
                    ViewState("SCmode") = "v"
                    trInfo.Style("display") = "none"
                End If
            End If
            If dsOverTime.Tables("DETAILS").Rows.Count > ViewState("Row") Then
                intCount = dsOverTime.Tables("DETAILS").Rows.Count
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
            'Checkbox, Date From, Time From, Date To, Time To, Purpose, Total Hrs/Mins, Times, Meal Allowance
            aryNewOverTime.Add(New String() {"N", "", "00:00", "", "00:00", "", "", "", ""})
        Next

        'Put all data in array if record found
        If dsOverTime.Tables("DETAILS").Rows.Count > 0 Then
            For i = 0 To dsOverTime.Tables("DETAILS").Rows.Count - 1
                aryNewOverTime(i)(1) = Common.parseNull(dsOverTime.Tables("DETAILS").Rows(i)("SCOD_DATE_FROM")) 'Date From
                aryNewOverTime(i)(2) = Common.parseNull(dsOverTime.Tables("DETAILS").Rows(i)("SCOD_FROM_TIME")) 'Time From
                aryNewOverTime(i)(3) = Common.parseNull(dsOverTime.Tables("DETAILS").Rows(i)("SCOD_DATE_TO")) 'Date To
                aryNewOverTime(i)(4) = Common.parseNull(dsOverTime.Tables("DETAILS").Rows(i)("SCOD_TO_TIME")) 'Time To 
                aryNewOverTime(i)(5) = Common.parseNull(dsOverTime.Tables("DETAILS").Rows(i)("SCOD_PURPOSE")) 'Purpose
                aryNewOverTime(i)(6) = Common.parseNull(dsOverTime.Tables("DETAILS").Rows(i)("SCOD_TOTAL_HOUR_MIN")) 'Total Hrs/Mins
                aryNewOverTime(i)(7) = Common.parseNull(dsOverTime.Tables("DETAILS").Rows(i)("SCOD_TIMES")) 'Times
                aryNewOverTime(i)(8) = Common.parseNull(dsOverTime.Tables("DETAILS").Rows(i)("SCOD_MEAL_ALLOWANCE")) 'Meal Allowance
            Next
        End If

    End Sub

    Private Sub ConstructTableOverTime()
        Dim table, strTotalHourMin As String
        Dim strrow As String = ""
        Dim i, c, intHour, intMin, count As Integer
        Dim blnChk As Boolean = True
        Dim intTotalHour, intTotalMin As Integer
        Dim aryTimes As New ArrayList
        aryTimes.Add("1.5x")
        aryTimes.Add("2.0x")

        count = aryNewOverTime.Count
        hidItemLine.Value = count

        For i = 0 To count - 1
            If aryNewOverTime(i)(6) = "" Then
                strTotalHourMin = ""
            Else
                intTotalHour = CInt(Left(aryNewOverTime(i)(6), 2))
                intTotalMin = CInt(Right(aryNewOverTime(i)(6), 2))
                strTotalHourMin = intTotalHour.ToString & ":" & Format(intTotalMin, "00").ToString
            End If

            strrow &= "<tr>"

            strrow &= "<td align=""center"" class=""tablecol"">"
            If aryNewOverTime(i)(0) = "Y" Then
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
            strrow &= "<input style=""width:80px;margin-right:0px; "" contentEditable=""false"" class=""txtbox"" type=""text"" id=""txtDateFr" & i & """ name=""txtDateFr" & i & """ value=""" & aryNewOverTime(i)(1) & """>"
            strrow &= "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateFr" & i) & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            strrow &= "</td>"

            'Time From
            strrow &= "<td class=""tablecol"">"
            strrow &= "<select class=""ddl"" style=""width:45%;margin-right:0px;"" onchange =""hourMinCalculation(" & i & ");"" id=""cboHourFrom" & i & """ name=""cboHourFrom" & i & """>"
            For intHour = 0 To 23
                If Left(aryNewOverTime(i)(2), 2) = Format(intHour, "00").ToString Then
                    strrow &= "<option value=""" & Format(intHour, "00").ToString & """ selected=""selected"">" & Format(intHour, "00").ToString & "</option>"
                Else
                    strrow &= "<option value=""" & Format(intHour, "00").ToString & """>" & Format(intHour, "00").ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "<select class=""ddl"" style=""width:45%;margin-right:0px;"" onchange =""hourMinCalculation(" & i & ");"" id=""cboMinFrom" & i & """ name=""cboMinFrom" & i & """>"
            For intMin = 0 To 59
                If Right(aryNewOverTime(i)(2), 2) = Format(intMin, "00").ToString Then
                    strrow &= "<option value=""" & Format(intMin, "00").ToString & """ selected=""selected"">" & Format(intMin, "00").ToString & "</option>"
                Else
                    strrow &= "<option value=""" & Format(intMin, "00").ToString & """>" & Format(intMin, "00").ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "</td>"

            'Date To
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:80px;margin-right:0px; "" contentEditable=""false"" class=""txtbox"" type=""text"" id=""txtDateTo" & i & """ name=""txtDateTo" & i & """ value=""" & aryNewOverTime(i)(3) & """>"
            strrow &= "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateTo" & i) & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            strrow &= "</td>"

            'Time To
            strrow &= "<td class=""tablecol"">"
            strrow &= "<select class=""ddl"" style=""width:45%;margin-right:0px;"" onchange =""hourMinCalculation(" & i & ");"" id=""cboHourTo" & i & """ name=""cboHourTo" & i & """>"
            For intHour = 0 To 23
                If Left(aryNewOverTime(i)(4), 2) = Format(intHour, "00").ToString Then
                    strrow &= "<option value=""" & Format(intHour, "00").ToString & """ selected=""selected"">" & Format(intHour, "00").ToString & "</option>"
                Else
                    strrow &= "<option value=""" & Format(intHour, "00").ToString & """>" & Format(intHour, "00").ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "<select class=""ddl"" style=""width:45%;margin-right:0px;"" onchange =""hourMinCalculation(" & i & ");"" id=""cboMinTo" & i & """ name=""cboMinTo" & i & """>"
            For intMin = 0 To 59
                If Right(aryNewOverTime(i)(4), 2) = Format(intMin, "00").ToString Then
                    strrow &= "<option value=""" & Format(intMin, "00").ToString & """ selected=""selected"">" & Format(intMin, "00").ToString & "</option>"
                Else
                    strrow &= "<option value=""" & Format(intMin, "00").ToString & """>" & Format(intMin, "00").ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "</td>"

            'Purpose
            strrow &= "<td class=""tablecol"">"
            strrow &= "<textarea rows=""2"" style=""font-family:Arial,Verdana;font-size:11px;width:200px;margin-right:0px; "" maxlength=""500"" id=""txtPurpose" & i & """ name=""txtPurpose" & i & """>" & aryNewOverTime(i)(5) & "</textarea>"
            strrow &= "</td>"

            'Total Hrs/Mins
            strrow &= "<td class=""tablecol"">"
            strrow &= "<label id=""lblHour" & i & """ style=""width:100%;margin-right:0px;"">" & strTotalHourMin & "</label>"
            strrow &= "</td>"

            'Times
            strrow &= "<td class=""tablecol"">"
            strrow &= "<select class=""ddl"" style=""width:100%;margin-right:0px;"" id=""cboTimes" & i & """ name=""cboTimes" & i & """>"
            strrow &= "<option value="""" selected=""selected"">" & "--Select--" & "</option>"
            For c = 0 To aryTimes.Count - 1
                If aryNewOverTime(i)(7) = aryTimes(c) Then
                    strrow &= "<option value=""" & aryTimes(c).ToString & """ selected=""selected"">" & aryTimes(c).ToString & "</option>"
                Else
                    strrow &= "<option value=""" & aryTimes(c).ToString & """>" & aryTimes(c).ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "</td>"

            'Meal Allowance
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:100px;margin-right:0px;"" onkeypress=""return isDecimalKey(event)"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtMA" & i & """ name=""txtMA" & i & """ value=""" & aryNewOverTime(i)(8) & """>"
            strrow &= "</td>"

            strrow &= "</tr>"
        Next

        Dim a As String = "<span class=""errormsg"">*</span>"
        table = "<table cellspacing=""0"" cellpadding=""0"" width=""980px"" border=""0"">" & _
                "<tr class=""TableHeader"">" & _
                "<td margin-right:0px;"" width=""30px"" align=""center""><input type=""checkbox"" id=""chkAll"" " & IIf(blnChk = True, "checked", "") & " name=""chkAll"" onclick=""selectAll();""></td>" & _
                "<td margin-right:0px;"" width=""30px"">Line</td>" & _
                "<td margin-right:0px;"" width=""120px"">Date From" & a & "</td>" & _
                "<td margin-right:0px;"" width=""100px"">Time From" & a & "</td>" & _
                "<td margin-right:0px;"" width=""120px"">Date To" & a & "</td>" & _
                "<td margin-right:0px;"" width=""100px"">Time To" & a & "</td>" & _
                "<td margin-right:0px;"" width=""200px"">Purpose" & a & "</td>" & _
                "<td margin-right:0px;"" width=""100px"">Total Hours/Mins</td>" & _
                "<td margin-right:0px;"" width=""100px"">Times" & a & "</td>" & _
                "<td margin-right:0px;"" width=""100px"" align=""right"">Meal Allowance</td></tr>" & _
                strrow & _
                "</table>"

        Session("ConstructTableOverTime") = table
        Session("aryNewOverTime") = aryNewOverTime
    End Sub

    Private Function calHourMin(ByVal strDateFrom As String, ByVal strTimeFrom As String, ByVal strDateTo As String, ByVal strTimeTo As String) As String
        Dim intHourFrom, intMinFrom, intHourTo, intMinTo As Integer
        Dim intHour, intMin As Integer
        Dim dtFrom, dtTo As DateTime
        Dim ts As TimeSpan
        Dim second, min, hour As Double

        intHourFrom = CInt(Left(strTimeFrom, 2))
        intMinFrom = CInt(Right(strTimeFrom, 2))
        intHourTo = CInt(Left(strTimeTo, 2))
        intMinTo = CInt(Right(strTimeTo, 2))

        calHourMin = ""
        If strDateFrom <> "" And strDateTo <> "" Then
            dtFrom = DateTime.ParseExact(strDateFrom & " " & strTimeFrom & ":00", "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)
            dtTo = DateTime.ParseExact(strDateTo & " " & strTimeTo & ":00", "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)

            If dtFrom = dtTo Then
                Return calHourMin
            Else
                If dtTo > dtFrom Then
                    ts = dtTo - dtFrom
                    second = ts.TotalSeconds
                    min = Math.Floor(second / 60)
                    second = second Mod 60
                    hour = Math.Floor(min / 60)
                    min = min Mod 60
                    calHourMin = Format(hour, "00").ToString & ":" & Format(min, "00").ToString
                End If
            End If
        End If

        Return calHourMin
    End Function

    Private Sub cmdAddClaim_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddClaim.Click
        Rebuild(False)
        BuildLine()
        ConstructTableOverTime()
    End Sub

    Private Sub Rebuild(Optional ByVal blnChk As Boolean = True)
        Dim i As Integer
        Dim parsedDate As Date
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

        aryNewOverTime = Session("aryNewOverTime")
        For i = 0 To aryNewOverTime.Count - 1
            aryNewOverTime(i)(0) = IIf(Request.Form("chkSelection" & i) = "on", "Y", "N") 'Checkbox
            aryNewOverTime(i)(1) = Request.Form("txtDateFr" & i) 'Date From
            aryNewOverTime(i)(2) = Request.Form("cboHourFrom" & i) & ":" & Request.Form("cboMinFrom" & i) 'Time From 
            aryNewOverTime(i)(3) = Request.Form("txtDateTo" & i) 'Date To
            aryNewOverTime(i)(4) = Request.Form("cboHourTo" & i) & ":" & Request.Form("cboMinTo" & i) 'Time To 
            aryNewOverTime(i)(5) = Request.Form("txtPurpose" & i) 'Purpose
            aryNewOverTime(i)(6) = calHourMin(aryNewOverTime(i)(1), aryNewOverTime(i)(2), aryNewOverTime(i)(3), aryNewOverTime(i)(4)) 'Total Hrs/Mins
            aryNewOverTime(i)(7) = Request.Form("cboTimes" & i) 'Times
            aryNewOverTime(i)(8) = Request.Form("txtMA" & i) 'Meal Allowance
        Next

        If blnChk = True Then
            For i = 0 To aryNewOverTime.Count - 1
                'Date, Total Hour & Times is required if either one has value
                If aryNewOverTime(i)(1) <> "" Or aryNewOverTime(i)(3) <> "" Or aryNewOverTime(i)(5) <> "" Or aryNewOverTime(i)(6) <> "" Or aryNewOverTime(i)(7) <> "" Or aryNewOverTime(i)(8) <> "" Then
                    'Error if Date From is empty
                    If aryNewOverTime(i)(1) = "" Then
                        blnEmpty = True
                        strErrEmpty &= IIf(strErrEmpty = "", i + 1, "," & i + 1)
                    Else
                        'Error if Date From is invalid
                        If DateDiff(DateInterval.Day, CDate(aryNewOverTime(i)(1)), Today.Now()) > 110 Then
                            blnCheck = True
                            strErrCheck &= IIf(strErrCheck = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if Date To is empty
                    If aryNewOverTime(i)(3) = "" Then
                        blnEmpty5 = True
                        strErrEmpty5 &= IIf(strErrEmpty5 = "", i + 1, "," & i + 1)
                    Else
                        'Error if Date To greater than today's date
                        If CDate(aryNewOverTime(i)(3)) > Today.Now() Then
                            blnCheck3 = True
                            strErrCheck3 &= IIf(strErrCheck3 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if date from greater than date to
                    If (blnEmpty = False And blnEmpty5 = False) Then
                        If CDate(aryNewOverTime(i)(1)) > CDate(aryNewOverTime(i)(3)) Then
                            blnCheck6 = True
                            strErrCheck6 &= IIf(strErrCheck6 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if Purpose is empty
                    If aryNewOverTime(i)(5) = "" Then
                        blnEmpty2 = True
                        strErrEmpty2 &= IIf(strErrEmpty2 = "", i + 1, "," & i + 1)
                    Else
                        If Not Common.checkMaxLength(aryNewOverTime(i)(5), 500) Then
                            blnCheck5 = True
                            strErrCheck5 &= IIf(strErrCheck5 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if Total Hour is empty
                    If aryNewOverTime(i)(6) = "" Then
                        blnEmpty3 = True
                        strErrEmpty3 &= IIf(strErrEmpty3 = "", i + 1, "," & i + 1)
                    Else
                        If aryNewOverTime(i)(6) = "00:00" Then
                            blnCheck4 = True
                            strErrCheck4 &= IIf(strErrCheck4 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if Times is empty
                    If aryNewOverTime(i)(7) = "" Then
                        blnEmpty4 = True
                        strErrEmpty4 &= IIf(strErrEmpty4 = "", i + 1, "," & i + 1)
                    End If

                    'Error if Meal Allowance is invalid
                    If aryNewOverTime(i)(8) <> "" Then
                        If chkDec(aryNewOverTime(i)(8)) = False Then
                            blnCheck2 = True
                            strErrCheck2 &= IIf(strErrCheck2 = "", i + 1, "," & i + 1)
                        End If
                    End If
                End If
            Next
        End If

    End Sub

    Private Function chkDec(ByVal strValue As String) As Boolean
        If IsNumeric(strValue) And Regex.IsMatch(Trim(strValue), "(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,2})?$") Then
            chkDec = True
        Else
            chkDec = False
        End If
    End Function

    Private Sub BuildLine()

        'Checkbox, Date From, Time From, Date To, Time To, Purpose, Total Hrs/Mins, Times, Meal Allowance
        aryNewOverTime.Add(New String() {"N", "", "00:00", "", "00:00", "", "", "", ""})

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
        If blnEmpty2 = True Then
            strMsg &= displayMsg(strErrEmpty2, "Purpose " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty3 = True Then
            strMsg &= displayMsg(strErrEmpty3, "Valid Time From/ Time To " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty4 = True Then
            strMsg &= displayMsg(strErrEmpty4, "Times " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty5 = True Then
            strMsg &= displayMsg(strErrEmpty5, "Date To " & objGlo.GetErrorMessage("00001"))
        End If
        If blnCheck = True Then
            strMsg &= displayMsg(strErrCheck, "Date From has exceeded 110-days policy.")
        End If
        If blnCheck2 = True Then
            strMsg &= displayMsg(strErrCheck2, "Meal Allowance " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck3 = True Then
            strMsg &= displayMsg(strErrCheck3, "Date To " & objGlo.GetErrorMessage("00015") & " today's date.")
        End If
        If blnCheck4 = True Then
            strMsg &= displayMsg(strErrCheck4, "Total Hours/Mins cannot be zero.")
        End If
        If blnCheck5 = True Then
            strMsg &= displayMsg(strErrCheck5, "Purpose is over limit.")
        End If
        If blnCheck6 = True Then
            strMsg &= displayMsg(strErrCheck6, "Date From should be <= Date To.")
        End If

        strFullMsg &= strMsg & "</ul>"

        If strMsg <> "" Then
            ConstructTableOverTime()
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

        For i = 0 To aryNewOverTime.Count - 1
            If aryNewOverTime(i)(0) = "Y" Then
                'Checkbox, Date From, Time From, Date To, Time To, Purpose, Total Hrs/Mins, Times, Meal Allowance
                aryDup.Add(New String() {"N", aryNewOverTime(i)(1), aryNewOverTime(i)(2), aryNewOverTime(i)(3), aryNewOverTime(i)(4), aryNewOverTime(i)(6)})
            End If
        Next

        If aryDup.Count > 0 Then
            For i = 0 To aryDup.Count - 1
                'Checkbox, Date From, Time From, Date To, Time To, Purpose, Total Hrs/Mins, Times, Meal Allowance
                aryNewOverTime.Add(New String() {"N", aryDup(i)(1), aryDup(i)(2), aryDup(i)(3), aryDup(i)(4), "", aryDup(i)(5), "", ""})
            Next
        End If

        ConstructTableOverTime()
    End Sub

    Private Sub Save(ByVal strMode As String)
        Dim strNewIndex As String = "", strNewSCNo As String = ""
        Dim ds As New DataSet
        Dim blnRecord As Boolean
        BindOT(ds, blnRecord)

        If blnRecord = True Then
            If ViewState("index") <> "" Then
                If objStaffClaim.UpdateOverTime(ds, ViewState("scno"), ViewState("index")) Then
                    If strMode = "Summary" Then
                        Common.NetMsgbox(Me, "Staff Claim Number " & ViewState("scno") & " has been updated.", dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                    Else
                        Common.NetMsgbox(Me, "Staff Claim Number " & ViewState("scno") & " has been updated.", dDispatcher.direct("StaffClaim", "OutstationClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                    End If
                Else
                    Common.NetMsgbox(Me, objGlo.GetErrorMessage("00007"), MsgBoxStyle.Information)
                End If
            Else
                If objStaffClaim.chkDocExistMonth = True Then
                    Common.NetMsgbox(Me, "You have already created Staff Claim document for this month.", MsgBoxStyle.Information)
                Else
                    If objStaffClaim.SaveOverTime(ds, strNewSCNo, strNewIndex) Then
                        ViewState("index") = strNewIndex
                        ViewState("scno") = strNewSCNo
                        If strMode = "Summary" Then
                            Common.NetMsgbox(Me, "Staff Claim Number " & strNewSCNo & " has been created.", dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                        Else
                            Common.NetMsgbox(Me, "Staff Claim Number " & strNewSCNo & " has been created.", dDispatcher.direct("StaffClaim", "OutstationClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                        End If
                    Else
                        Common.NetMsgbox(Me, objGlo.GetErrorMessage("00007"), MsgBoxStyle.Information)
                    End If
                End If
            End If
        Else
            Common.NetMsgbox(Me, objGlo.GetErrorMessage("00131"), MsgBoxStyle.Information)
        End If
        
        ConstructTableOverTime()

    End Sub

    Private Sub BindOT(ByRef ds As DataSet, ByRef blnRecord As Boolean)
        Dim dtOTMstr As New DataTable
        Dim dtr As DataRow

        blnRecord = False 'By Default
        dtOTMstr.Columns.Add("DEPT_CODE", Type.GetType("System.String"))
        dtr = dtOTMstr.NewRow
        dtr("DEPT_CODE") = lblDeptId.Text

        dtOTMstr.Rows.Add(dtr)
        ds.Tables.Add(dtOTMstr)

        Dim i As Integer, intLine As Integer = 1
        Dim dtOTDtls As New DataTable
        Dim dtrd As DataRow

        dtOTDtls.Columns.Add("LINE_NO", Type.GetType("System.String"))
        dtOTDtls.Columns.Add("DATE_FROM", Type.GetType("System.String"))
        dtOTDtls.Columns.Add("FROM_TIME", Type.GetType("System.String"))
        dtOTDtls.Columns.Add("DATE_TO", Type.GetType("System.String"))
        dtOTDtls.Columns.Add("TO_TIME", Type.GetType("System.String"))
        dtOTDtls.Columns.Add("PURPOSE", Type.GetType("System.String"))
        dtOTDtls.Columns.Add("TOTAL_HOUR_MIN", Type.GetType("System.String"))
        dtOTDtls.Columns.Add("TIMES", Type.GetType("System.String"))
        dtOTDtls.Columns.Add("MEAL_ALLOWANCE", Type.GetType("System.String"))

        For i = 0 To aryNewOverTime.Count - 1
            If aryNewOverTime(i)(1) <> "" Then
                blnRecord = True
                dtrd = dtOTDtls.NewRow

                dtrd("LINE_NO") = intLine
                dtrd("DATE_FROM") = aryNewOverTime(i)(1) 'Date From
                dtrd("FROM_TIME") = aryNewOverTime(i)(2) & ":00" 'Time From 
                dtrd("DATE_TO") = aryNewOverTime(i)(3) 'Date To
                dtrd("TO_TIME") = aryNewOverTime(i)(4) & ":00"  'Time To 
                dtrd("PURPOSE") = IIf(aryNewOverTime(i)(5) = "", "", aryNewOverTime(i)(5)) 'Purpose
                dtrd("TOTAL_HOUR_MIN") = aryNewOverTime(i)(6) & ":00" 'Total Hours/Mins
                dtrd("TIMES") = aryNewOverTime(i)(7) 'Times
                dtrd("MEAL_ALLOWANCE") = IIf(aryNewOverTime(i)(8) = "", "", aryNewOverTime(i)(8)) 'Meal Allowance

                dtOTDtls.Rows.Add(dtrd)
                intLine = intLine + 1
            End If
        Next

        If blnRecord = False And ViewState("index") <> "" Then
            blnRecord = objStaffClaim.chkClaimFormData(ViewState("scno"), "Overtime")
        End If

        ds.Tables.Add(dtOTDtls)
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
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "HardShipClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Hardship Claim Form</strong></a></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "OverTimeClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><font color=""green""><strong>Overtime Claim Form</strong></font></a></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "OutstationClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Outstation Claim Form</strong></a></span></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "OtherClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Other Claim Form</strong></a></span></td>" & _
        "</tr>" & _
        "</table>" & _
        "</td></tr>"
        Session("w_SC_Links") = strTableLink

    End Sub
End Class
