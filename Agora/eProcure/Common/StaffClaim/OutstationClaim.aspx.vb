Imports AgoraLegacy
Imports eProcure.Component

Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Drawing

Public Class OutstationClaim
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
    Dim blnEmpty As Boolean = True, blnEmpty2 As Boolean = True, blnEmpty3 As Boolean = True, blnEmpty4 As Boolean = True, _
    blnEmpty5 As Boolean = True, blnEmpty6 As Boolean = True, blnEmpty7 As Boolean = True, blnEmpty8 As Boolean = True
    Dim blnCheck As Boolean = True, blnCheck2 As Boolean = True, blnCheck3 As Boolean = True, blnCheck4 As Boolean = True, _
    blnCheck5 As Boolean = True, blnCheck6 As Boolean = True, blnCheck7 As Boolean = True, blnCheck8 As Boolean = True, blnCheck9 As Boolean = True, blnCheck10 As Boolean = True, blnCheck11 As Boolean = True
    Dim aryNewOut As New ArrayList()
    Dim strErrEmpty, strErrEmpty2, strErrEmpty3, strErrEmpty4, strErrEmpty5, strErrEmpty6, strErrEmpty7, strErrEmpty8 As String
    Dim strErrCheck, strErrCheck2, strErrCheck3, strErrCheck4, strErrCheck5, strErrCheck6, strErrCheck7, strErrCheck8, strErrCheck9, strErrCheck10, strErrCheck11 As String

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

            Session("aryNewOut") = Nothing
            lblScNo.Text = "To Be Allocated By System"
            ViewState("index") = Request.QueryString("index")
            ViewState("mode") = Request.QueryString("mode")
            ViewState("Row") = 1
            'DisplayUserDetails()
            BuildRow()
            ConstructTableOut()
            GenerateTab()
            GenerateFormLink()
        End If

        lblMsg.Text = ""
    End Sub

    Private Sub DisplayUserDetails()
        Dim objUser As New Users
        Dim objUserDetails As New User

        objUserDetails = objUser.GetUserDetails(Session("UserId"), Session("CompanyId"))
        lblUserName.Text = objUserDetails.Name 'User Name
        lblCompName.Text = Session("CompanyName") 'Company Name
        lblDept.Text = Common.parseNull(objUserDetails.DeptName) 'Department Name
        lblDeptId.Text = Common.parseNull(objUserDetails.DeptID)

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
        ddlSelect.SelectedValue = "Outstation"
    End Sub

    Private Sub BuildRow()
        Dim i, intCount As Integer
        Dim dsOut As DataSet
        Dim objUser As New Users
        Dim objUserDetails As New User

        BindDdl()
        aryNewOut.Clear()

        dsOut = objStaffClaim.GetOutstationDetails(ViewState("index"))

        If dsOut.Tables(0).Rows.Count > 0 Then
            lblDocDate.Text = Format(dsOut.Tables("MSTR").Rows(0)("SCM_CREATED_DATE"), "dd/MM/yyyy")
            ViewState("scno") = Common.parseNull(dsOut.Tables("MSTR").Rows(0)("SCM_CLAIM_DOC_NO"))
            lblScNo.Text = ViewState("scno")
            lblStatus.Text = Common.parseNull(dsOut.Tables("MSTR").Rows(0)("STATUS_DESC"))

            objUserDetails = objUser.GetUserDetails(dsOut.Tables("MSTR").Rows(0)("SCM_STAFF_ID"), Session("CompanyId"))
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
                If dsOut.Tables("MSTR").Rows(0)("SCM_STATUS") <> SCStatus.DraftSC And dsOut.Tables("MSTR").Rows(0)("SCM_STATUS") <> SCStatus.Rejected Then
                    cmdAddClaim.Visible = False
                    cmdDupLine.Visible = False
                    cmdSave.Visible = False
                    cmdSaveSummary.Visible = False
                    cmdClear.Visible = False
                    ViewState("SCmode") = "v"
                    trInfo.Style("display") = "none"
                End If
            End If
            If dsOut.Tables("DETAILS").Rows.Count > ViewState("Row") Then
                intCount = dsOut.Tables("DETAILS").Rows.Count
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
            'Checkbox, Depart Date, Depart Time, Return Date, Return Time, No Of Day, Destination, Project Code, Currency, Exchange Rate, Meal Rate, Meal Entitled,
            'Free Meal, Actual Meal Claimed, Total Subsistence Allw. Claimed, Total Accommodation Claimed, Gst Amt, Total Acc Allw, Total Amt
            aryNewOut.Add(New String() {"N", "", "00:00", "", "00:00", "", "", "", "MYR", "", "", "", "", "", "", "", "", "", ""})
        Next

        'Put all data in array if record found
        If dsOut.Tables("DETAILS").Rows.Count > 0 Then
            For i = 0 To dsOut.Tables("DETAILS").Rows.Count - 1
                aryNewOut(i)(1) = Common.parseNull(dsOut.Tables("DETAILS").Rows(i)("SCOOD_DEPART_DATE")) 'Depart Date
                aryNewOut(i)(2) = Common.parseNull(dsOut.Tables("DETAILS").Rows(i)("SCOOD_DEPART_TIME")) 'Depart Time
                aryNewOut(i)(3) = Common.parseNull(dsOut.Tables("DETAILS").Rows(i)("SCOOD_RETURN_DATE")) 'Return Date
                aryNewOut(i)(4) = Common.parseNull(dsOut.Tables("DETAILS").Rows(i)("SCOOD_RETURN_TIME")) 'Return Time
                aryNewOut(i)(5) = Common.parseNull(dsOut.Tables("DETAILS").Rows(i)("SCOOD_NO_OF_DAY")) 'No Of Day
                aryNewOut(i)(6) = Common.parseNull(dsOut.Tables("DETAILS").Rows(i)("SCOOD_DESTINATION")) 'Destination
                aryNewOut(i)(7) = Common.parseNull(dsOut.Tables("DETAILS").Rows(i)("SCOOD_PROJ_CODE")) 'Project Code
                aryNewOut(i)(8) = Common.parseNull(dsOut.Tables("DETAILS").Rows(i)("SCOOD_CURRENCY_CODE")) 'Currency
                aryNewOut(i)(9) = IIf(aryNewOut(i)(8) = "MYR", "", Common.parseNull(dsOut.Tables("DETAILS").Rows(i)("SCOOD_EXCHANGE_RATE"))) 'Exchange Rate
                aryNewOut(i)(10) = Common.parseNull(dsOut.Tables("DETAILS").Rows(i)("SCOOD_MEAL_RATE")) 'Meal Rate
                aryNewOut(i)(11) = Common.parseNull(dsOut.Tables("DETAILS").Rows(i)("SCOOD_MEAL_ENTITLED")) 'Meal Entitled
                aryNewOut(i)(12) = Common.parseNull(dsOut.Tables("DETAILS").Rows(i)("SCOOD_FREE_MEAL")) 'Free Meal
                aryNewOut(i)(13) = Common.parseNull(dsOut.Tables("DETAILS").Rows(i)("SCOOD_ACTUAL_MEAL_CLAIM")) 'Actual Meal Claimed
                aryNewOut(i)(14) = Common.parseNull(dsOut.Tables("DETAILS").Rows(i)("SCOOD_TOTAL_SUB_ALLW_CLAIM")) 'Total Subsistence Allw
                aryNewOut(i)(15) = Common.parseNull(dsOut.Tables("DETAILS").Rows(i)("SCOOD_TOTAL_ACC_CLAIM")) 'Total Accomodation Claimed
                aryNewOut(i)(16) = Common.parseNull(dsOut.Tables("DETAILS").Rows(i)("SCOOD_GST_AMT")) 'Gst Amt
                aryNewOut(i)(17) = Common.parseNull(dsOut.Tables("DETAILS").Rows(i)("SCOOD_TOTAL_ACC_ALLW")) 'Total Accomodation Allw
                aryNewOut(i)(18) = Common.parseNull(dsOut.Tables("DETAILS").Rows(i)("SCOOD_TOTAL_CLAIM_AMT")) 'Total Claim Amt
            Next
        End If

    End Sub

    Private Sub ConstructTableOut()
        Dim table As String
        Dim strrow As String = ""
        Dim strrowTotal As String = ""
        Dim i, c, intHour, intMin, count As Integer
        Dim blnChk As Boolean = True
        Dim aryRate As New ArrayList()
        Dim dsCurr As New DataSet

        dsCurr = objStaffClaim.getDdlInfo("1")

        count = aryNewOut.Count
        hidItemLine.Value = count
        For i = 0 To count - 1
            strrow &= "<tr>"

            strrow &= "<td align=""center"" class=""tablecol"">"
            If aryNewOut(i)(0) = "Y" Then
                strrow &= "<input type=""checkbox"" id=""chkSelection" & i & """ checked name=""chkSelection" & i & """ onclick=""checkChild('chkSelection" & i & "')"">"
            Else
                strrow &= "<input type=""checkbox"" id=""chkSelection" & i & """ name=""chkSelection" & i & """ onclick=""checkChild('chkSelection" & i & "')"">"
                blnChk = False
            End If
            strrow &= "</td>"

            strrow &= "<td class=""tablecol"" align=""center"">" & i + 1
            strrow &= "</td>"

            'Depart Date
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:60px;margin-right:0px; "" contentEditable=""false"" class=""txtbox"" type=""text"" id=""txtDepartDt" & i & """ name=""txtDepartDt" & i & """ value=""" & aryNewOut(i)(1) & """>"
            strrow &= "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDepartDt" & i) & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            strrow &= "</td>"

            'Depart Time
            strrow &= "<td class=""tablecol"">"
            strrow &= "<select class=""ddl"" style=""width:48%;margin-right:0px;"" id=""cboDepartHr" & i & """ onchange=""CalMeal(" & i & ")"" name=""cboDepartHr" & i & """>"
            For intHour = 0 To 23
                If Left(aryNewOut(i)(2), 2) = Format(intHour, "00").ToString Then
                    strrow &= "<option value=""" & Format(intHour, "00").ToString & """ selected=""selected"">" & Format(intHour, "00").ToString & "</option>"
                Else
                    strrow &= "<option value=""" & Format(intHour, "00").ToString & """>" & Format(intHour, "00").ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "<select class=""ddl"" style=""width:48%;margin-right:0px;"" id=""cboDepartMin" & i & """ onchange=""CalMeal(" & i & ")"" name=""cboDepartMin" & i & """>"
            For intMin = 0 To 59
                If Right(aryNewOut(i)(2), 2) = Format(intMin, "00").ToString Then
                    strrow &= "<option value=""" & Format(intMin, "00").ToString & """ selected=""selected"">" & Format(intMin, "00").ToString & "</option>"
                Else
                    strrow &= "<option value=""" & Format(intMin, "00").ToString & """>" & Format(intMin, "00").ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "</td>"

            'Return Date
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:60px;margin-right:0px; "" contentEditable=""false"" class=""txtbox"" type=""text"" id=""txtReturnDt" & i & """ name=""txtReturnDt" & i & """ value=""" & aryNewOut(i)(3) & """>"
            strrow &= "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtReturnDt" & i) & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            strrow &= "</td>"

            'Return Time
            strrow &= "<td class=""tablecol"">"
            strrow &= "<select class=""ddl"" style=""width:48%;margin-right:0px;"" id=""cboReturnHr" & i & """ onchange=""CalMeal(" & i & ")"" name=""cboReturnHr" & i & """>"
            For intHour = 0 To 23
                If Left(aryNewOut(i)(4), 2) = Format(intHour, "00").ToString Then
                    strrow &= "<option value=""" & Format(intHour, "00").ToString & """ selected=""selected"">" & Format(intHour, "00").ToString & "</option>"
                Else
                    strrow &= "<option value=""" & Format(intHour, "00").ToString & """>" & Format(intHour, "00").ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "<select class=""ddl"" style=""width:48%;margin-right:0px;"" id=""cboReturnMin" & i & """ onchange=""CalMeal(" & i & ")"" name=""cboReturnMin" & i & """>"
            For intMin = 0 To 59
                If Right(aryNewOut(i)(4), 2) = Format(intMin, "00").ToString Then
                    strrow &= "<option value=""" & Format(intMin, "00").ToString & """ selected=""selected"">" & Format(intMin, "00").ToString & "</option>"
                Else
                    strrow &= "<option value=""" & Format(intMin, "00").ToString & """>" & Format(intMin, "00").ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "</td>"

            'No of Day
            strrow &= "<td class=""tablecol"">"
            strrow &= "<label id=""lblNoOfDay" & i & """ style=""width:100%;margin-right:0px; text-align:right;"">" & aryNewOut(i)(5) & "</label>"
            strrow &= "</td>"

            'Destination
            strrow &= "<td class=""tablecol"">"
            strrow &= "<textarea rows=""2"" style=""font-family:Arial,Verdana;font-size:11px;width:180px;margin-right:0px; "" maxlength=""500"" id=""txtDestination" & i & """ name=""txtDestination" & i & """>" & aryNewOut(i)(6) & "</textarea>"
            strrow &= "</td>"

            'Project Code
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:160px;margin-right:0px; "" maxlength=""50"" class=""txtbox"" type=""text"" id=""txtPC" & i & """ name=""txtPC" & i & """ value=""" & aryNewOut(i)(7) & """>"
            strrow &= "</td>"

            'Currency
            strrow &= "<td class=""tablecol"">"
            strrow &= "<select class=""ddl"" style=""width:100%;margin-right:0px;"" id=""cboCurr" & i & """ onchange=""enableExRate(" & i & ")"" name=""cboCurr" & i & """>"
            strrow &= "<option value="""" selected=""selected"">" & "--Select--" & "</option>"
            For c = 0 To dsCurr.Tables(0).Rows.Count - 1
                If aryNewOut(i)(8) = dsCurr.Tables(0).Rows(c)(0) Then
                    strrow &= "<option title=""" & dsCurr.Tables(0).Rows(c)(0).ToString & """ value=""" & dsCurr.Tables(0).Rows(c)(0).ToString & """ selected=""selected"">" & dsCurr.Tables(0).Rows(c)(0).ToString & "</option>"
                Else
                    strrow &= "<option title=""" & dsCurr.Tables(0).Rows(c)(0).ToString & """ value=""" & dsCurr.Tables(0).Rows(c)(0).ToString & """>" & dsCurr.Tables(0).Rows(c)(0).ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "</td>"

            'Exchange Rate
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:75px;margin-right:0px;"" " & IIf(aryNewOut(i)(8) <> "MYR", "", "disabled") & " onkeypress=""return isDecimalKey(event)"" onkeyup=""CalTotalAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtExRate" & i & """ name=""txtExRate" & i & """ value=""" & aryNewOut(i)(9) & """>"
            strrow &= "</td>"

            'Meal Policy
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:75px;margin-right:0px;"" onkeypress=""return isDecimalKey(event)"" onkeyup=""CalMeal(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtMP" & i & """ name=""txtMP" & i & """ value=""" & aryNewOut(i)(10) & """>"
            strrow &= "</td>"

            'Meal Entitled
            strrow &= "<td class=""tablecol"">"
            strrow &= "<label id=""lblME" & i & """ style=""width:100%;margin-right:0px; text-align:right;"">" & aryNewOut(i)(11) & "</label>"
            strrow &= "</td>"

            'Free Meal
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:75px;margin-right:0px; "" onkeypress=""return isNumberKey(event)"" onkeyup=""CalMeal(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtFreeMeal" & i & """ name=""txtFreeMeal" & i & """ value=""" & aryNewOut(i)(12) & """>"
            strrow &= "</td>"

            'Actual Meal Claimed
            strrow &= "<td class=""tablecol"">"
            strrow &= "<label id=""lblAMC" & i & """ style=""width:100%;margin-right:0px; text-align:right;"">" & aryNewOut(i)(13) & "</label>"
            strrow &= "</td>"

            'Total Subsistence Allw. Claimed
            strrow &= "<td class=""tablecol"">"
            strrow &= "<label id=""lblTSAC" & i & """ style=""width:100%;margin-right:0px; text-align:right;"">" & aryNewOut(i)(14) & "</label>"
            strrow &= "</td>"

            'Total Accommodation Claimed
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:75px;margin-right:0px; "" onkeypress=""return isDecimalKey(event)"" onkeyup=""CalTotalAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtTAC" & i & """ name=""txtTAC" & i & """ value=""" & aryNewOut(i)(15) & """>"
            strrow &= "</td>"

            'Gst Amt
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:75px;margin-right:0px; "" onkeypress=""return isDecimalKey(event)"" onkeyup=""CalTotalAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtGstAmt" & i & """ name=""txtGstAmt" & i & """ value=""" & aryNewOut(i)(16) & """>"
            strrow &= "</td>"

            'Total Accommodation Allw.
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:90px;margin-right:0px; "" onkeypress=""return isDecimalKey(event)"" onkeyup=""CalTotalAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtTAA" & i & """ name=""txtTAA" & i & """ value=""" & aryNewOut(i)(17) & """>"
            strrow &= "</td>"

            'Total Amt
            strrow &= "<td class=""tablecol"">"
            strrow &= "<label id=""lblTotalAmt" & i & """ style=""width:100%;margin-right:0px; text-align:right;"">" & aryNewOut(i)(18) & "</label>"
            strrow &= "</td>"

            strrow &= "</tr>"

        Next

        Dim a As String = "<span class=""errormsg"">*</span>"
        table = "<table cellspacing=""0"" cellpadding=""0"" width=""1775px"" border=""0"">" & _
                           "<tr class=""TableHeader"">" & _
                           "<td margin-right:0px;"" width=""30px"" align=""center""><input type=""checkbox"" id=""chkAll"" " & IIf(blnChk = True, "checked", "") & " name=""chkAll"" onclick=""selectAll();""></td>" & _
                           "<td margin-right:0px;"" width=""30px"">Line</td>" & _
                           "<td margin-right:0px;"" width=""120px"">Departure Date" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""100px"">Departure Time" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""120px"">Return Date" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""100px"">Return Time " & a & "</td>" & _
                           "<td margin-right:0px;"" width=""75px"" align=""right"">No Of Days</td>" & _
                           "<td margin-right:0px;"" width=""180px"">Destination" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""160px"">Project Code</td>" & _
                           "<td margin-right:0px;"" width=""80px"">Currency" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""75px"" align=""right"">Exchange Rate</td>" & _
                           "<td margin-right:0px;"" width=""75px"" align=""right"">Meal Rate per meal</td>" & _
                           "<td margin-right:0px;"" width=""75px"" align=""right"">Meal Entitled</td>" & _
                           "<td margin-right:0px;"" width=""75px"" align=""right"">Free Meal</td>" & _
                           "<td margin-right:0px;"" width=""75px"" align=""right"">Actual Meal Claimed</td>" & _
                           "<td margin-right:0px;"" width=""90px"" align=""right"">Total Subsistence Allw. Claimed</td>" & _
                           "<td margin-right:0px;"" width=""75px"" align=""right"">Total Accommodation Claimed</td>" & _
                           "<td margin-right:0px;"" width=""75px"" align=""right"">GST Amt</td>" & _
                           "<td margin-right:0px;"" width=""90px"" align=""right"">Total Accommodation Allw.</td>" & _
                           "<td margin-right:0px;"" width=""75px"" align=""right"">Total Amount (MYR)</td>" & _
                           "</tr>" & _
                           strrow & _
                           strrowTotal & _
                           "</table>"

        Session("ConstructTableOut") = table
        Session("aryNewOut") = aryNewOut
    End Sub

    Private Function countMealEntitle(ByVal strType As String, ByVal strTimeFr As String, ByVal strTimeTo As String) As Integer
        Dim dblTimeFr, dblTimeTo As Decimal
        dblTimeFr = CDec(strTimeFr)
        dblTimeTo = CDec(strTimeTo)

        If strType = "B" Then
            If dblTimeFr <= 6.3 And dblTimeTo >= 10.3 Then Return 1
        ElseIf strType = "L" Then
            If dblTimeFr <= 10.0 And dblTimeTo >= 14.0 Then Return 1
        ElseIf strType = "D" Then
            If dblTimeFr <= 16.0 And dblTimeTo >= 20.0 Then Return 1
        Else
            Return 0
        End If
    End Function

    Private Function CalNoOfDay(ByVal strDepartDt As String, ByVal strDepartTm As String, ByVal strReturnDt As String, ByVal strReturnTm As String) As String
        Dim strNoOfDay As String = ""
        Dim dtDepart, dtReturn As DateTime
        Dim ts As TimeSpan

        If strDepartDt <> "" And strReturnDt <> "" Then
            dtDepart = DateTime.ParseExact(strDepartDt & " " & strDepartTm & ":00", "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)
            dtReturn = DateTime.ParseExact(strReturnDt & " " & strReturnTm & ":00", "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)
            If dtReturn > dtDepart Then
                ts = dtReturn - dtDepart
                strNoOfDay = Format(ts.TotalHours / 24, "###0.00") '1 Day = 24 Hours
            End If
        End If

        Return strNoOfDay
    End Function

    Private Function CalMealEnt(ByVal strDepartDt As String, ByVal strDepartTm As String, ByVal strReturnDt As String, ByVal strReturnTm As String) As String
        Dim strMealEnt As String = ""
        Dim dtDepart, dtReturn As DateTime
        Dim parsedDate, dDepart, dReturn As Date
        Dim ts As TimeSpan
        Dim lgDiffDay As Long
        Dim intCountMeal As Integer = 0

        If strDepartDt <> "" And strReturnDt <> "" Then
            dDepart = DateTime.ParseExact(strDepartDt, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            dtDepart = DateTime.ParseExact(strDepartDt & " " & strDepartTm & ":00", "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)
            dReturn = DateTime.ParseExact(strReturnDt, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            dtReturn = DateTime.ParseExact(strReturnDt & " " & strReturnTm & ":00", "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)
            If dtReturn > dtDepart Then
                'Calculate Meal Entitled
                lgDiffDay = DateDiff(DateInterval.Day, dDepart, dReturn)

                If lgDiffDay = 0 Then
                    intCountMeal += countMealEntitle("B", Replace(strDepartTm, ":", "."), Replace(strReturnTm, ":", "."))
                    intCountMeal += countMealEntitle("L", Replace(strDepartTm, ":", "."), Replace(strReturnTm, ":", "."))
                    intCountMeal += countMealEntitle("D", Replace(strDepartTm, ":", "."), Replace(strReturnTm, ":", "."))
                ElseIf lgDiffDay > 0 Then
                    intCountMeal += (lgDiffDay - 1) * 3
                    intCountMeal += countMealEntitle("B", Replace(strDepartTm, ":", "."), "23.59")
                    intCountMeal += countMealEntitle("L", Replace(strDepartTm, ":", "."), "23.59")
                    intCountMeal += countMealEntitle("D", Replace(strDepartTm, ":", "."), "23.59")
                    intCountMeal += countMealEntitle("B", "0.00", Replace(strReturnTm, ":", "."))
                    intCountMeal += countMealEntitle("L", "0.00", Replace(strReturnTm, ":", "."))
                    intCountMeal += countMealEntitle("D", "0.00", Replace(strReturnTm, ":", "."))
                End If
                strMealEnt = intCountMeal
            End If
        End If

        Return strMealEnt
    End Function

    Private Function CalActualMealClaim(ByVal strMealEnt As String, ByVal strFreeMeal As String) As String
        Dim strAMC As String = ""

        If strMealEnt <> "" Then
            strAMC = strMealEnt
            If strFreeMeal <> "" Then
                If chkInt(strFreeMeal) Then
                    strAMC = CInt(strMealEnt) - CInt(strFreeMeal)
                Else
                    strAMC = ""
                End If
            End If
        End If

        Return strAMC
    End Function

    Private Function CalTotalAmt(ByVal strCurr As String, ByVal strExRate As String, ByVal strAMC As String, ByVal strTotalAC As String, ByVal strGstAmt As String, ByVal strTotalAA As String) As String
        Dim strTotalAmt As String = ""
        Dim decExRate As Decimal = 0, decTotalAmt As Decimal = 0

        If strAMC <> "" Or strTotalAC <> "" Or strTotalAA <> "" Then
            If strAMC <> "" Then
                decTotalAmt = CDec(strAMC)
            End If

            If strTotalAC <> "" Then
                If chkDec(strTotalAC) Then
                    decTotalAmt = decTotalAmt + CDec(strTotalAC)
                End If
            End If

            If strGstAmt <> "" Then
                If chkDec(strGstAmt) Then
                    decTotalAmt = decTotalAmt + CDec(strGstAmt)
                End If
            End If

            If strTotalAA <> "" Then
                If chkDec(strTotalAA) Then
                    decTotalAmt = decTotalAmt + CDec(strTotalAA)
                End If
            End If

            If strCurr = "MYR" Then
                decExRate = 1
            Else
                If strExRate <> "" And chkDec(strExRate) Then
                    decExRate = CDec(strExRate)
                End If
            End If
            decTotalAmt = decTotalAmt * decExRate
            strTotalAmt = Format(decTotalAmt, "###0.00")
        End If

        Return strTotalAmt
    End Function

    Private Function CalTotalSAC(ByVal strMealRate As String, ByVal strAMC As String) As String
        Dim strTotalSAC As String = "0.00"

        If strMealRate <> "" And strAMC <> "" And chkDec(strMealRate) Then
            strTotalSAC = Format(CDec(strMealRate) * CInt(strAMC), "###0.00")
        End If

        Return strTotalSAC
    End Function

    Private Function chkInt(ByVal strValue As String) As Boolean
        If IsNumeric(strValue) And Regex.IsMatch(Trim(strValue), "^-?[0-9]+$") Then
            chkInt = True
        Else
            chkInt = False
        End If
    End Function

    Private Function chkDec(ByVal strValue As String) As Boolean
        If IsNumeric(strValue) And Regex.IsMatch(Trim(strValue), "(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,2})?$") Then
            chkDec = True
        Else
            chkDec = False
        End If
    End Function

    Private Sub cmdAddClaim_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddClaim.Click
        Rebuild(False)
        BuildLine()
        ConstructTableOut()
    End Sub

    Private Sub Rebuild(Optional ByVal blnChk As Boolean = True)
        Dim i As Integer
        Dim parsedDate As Date
        Dim decGrandTotalAmt As Decimal = 0
        Dim strNoOfDay, strME, strAMC, strTCA As String
        blnEmpty = False
        blnEmpty2 = False
        blnEmpty3 = False
        blnEmpty4 = False
        blnEmpty5 = False
        blnEmpty6 = False
        blnEmpty7 = False
        blnEmpty8 = False
        blnCheck = False
        blnCheck2 = False
        blnCheck3 = False
        blnCheck4 = False
        blnCheck5 = False
        blnCheck6 = False
        blnCheck7 = False
        blnCheck8 = False
        blnCheck9 = False
        blnCheck10 = False
        blnCheck11 = False

        aryNewOut = Session("aryNewOut")
        For i = 0 To aryNewOut.Count - 1
            aryNewOut(i)(0) = IIf(Request.Form("chkSelection" & i) = "on", "Y", "N") 'Checkbox
            aryNewOut(i)(1) = Request.Form("txtDepartDt" & i) 'Depart Date
            aryNewOut(i)(2) = Request.Form("cboDepartHr" & i) & ":" & Request.Form("cboDepartMin" & i) 'Depart Time
            aryNewOut(i)(3) = Request.Form("txtReturnDt" & i) 'Return Date
            aryNewOut(i)(4) = Request.Form("cboReturnHr" & i) & ":" & Request.Form("cboReturnMin" & i) 'Return Time 
            aryNewOut(i)(5) = CalNoOfDay(aryNewOut(i)(1), aryNewOut(i)(2), aryNewOut(i)(3), aryNewOut(i)(4)) 'No Of Day
            aryNewOut(i)(6) = Request.Form("txtDestination" & i) 'Destination
            aryNewOut(i)(7) = Request.Form("txtPC" & i) 'Project Code
            aryNewOut(i)(8) = Request.Form("cboCurr" & i) 'Currency
            aryNewOut(i)(9) = Request.Form("txtExRate" & i) 'Exchange Rate
            aryNewOut(i)(10) = Request.Form("txtMP" & i) 'Meal Rate
            aryNewOut(i)(11) = CalMealEnt(aryNewOut(i)(1), aryNewOut(i)(2), aryNewOut(i)(3), aryNewOut(i)(4)) 'Meal Entitled
            aryNewOut(i)(12) = Request.Form("txtFreeMeal" & i) 'Free Meal
            aryNewOut(i)(13) = CalActualMealClaim(aryNewOut(i)(11), aryNewOut(i)(12)) 'Actual Meal Claimed
            aryNewOut(i)(14) = CalTotalSAC(aryNewOut(i)(10), aryNewOut(i)(13)) 'Total Subsistence Allw
            aryNewOut(i)(15) = Request.Form("txtTAC" & i) 'Total Accomodation Claimed
            aryNewOut(i)(16) = Request.Form("txtGstAmt" & i) 'Total Accomodation Claimed
            aryNewOut(i)(17) = Request.Form("txtTAA" & i) 'Total Accomodation Allw
            aryNewOut(i)(18) = CalTotalAmt(aryNewOut(i)(8), aryNewOut(i)(9), aryNewOut(i)(14), aryNewOut(i)(15), aryNewOut(i)(16), aryNewOut(i)(17)) 'Total Amt
        Next

        If blnChk = True Then
            For i = 0 To aryNewOut.Count - 1
                If aryNewOut(i)(1) <> "" Or aryNewOut(i)(3) <> "" Or aryNewOut(i)(5) <> "" Or aryNewOut(i)(6) <> "" Or aryNewOut(i)(7) <> "" Or _
                aryNewOut(i)(9) <> "" Or aryNewOut(i)(10) <> "" Or aryNewOut(i)(12) <> "" Or aryNewOut(i)(15) <> "" Or aryNewOut(i)(16) <> "" Or aryNewOut(i)(17) <> "" Then
                    'Error if depart date is empty
                    If aryNewOut(i)(1) = "" Then
                        blnEmpty = True
                        strErrEmpty &= IIf(strErrEmpty = "", i + 1, "," & i + 1)
                    Else
                        'Error if Depart Date is invalid
                        If DateDiff(DateInterval.Day, CDate(aryNewOut(i)(1)), Today.Now()) > 110 Then
                            blnCheck = True
                            strErrCheck &= IIf(strErrCheck = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if return date is empty
                    If aryNewOut(i)(3) = "" Then
                        blnEmpty2 = True
                        strErrEmpty2 = i + 1
                    Else
                        ''Error if return date greater than today's date
                        'If CDate(aryNewOut(i)(3)) > Today.Now() Then
                        '    blnCheck2 = True
                        '    strErrCheck2 &= IIf(strErrCheck2 = "", i + 1, "," & i + 1)
                        'End If
                    End If

                    'Error if depart date greater than return date
                    If (blnEmpty = False And blnEmpty2 = False) Then
                        If CDate(aryNewOut(i)(1)) > CDate(aryNewOut(i)(3)) Then
                            blnCheck3 = True
                            strErrCheck3 &= IIf(strErrCheck2 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if No of Day is empty
                    If aryNewOut(i)(5) = "" Then
                        blnEmpty3 = True
                        strErrEmpty3 &= IIf(strErrEmpty3 = "", i + 1, "," & i + 1)
                    End If

                    'Error if Destination is empty
                    If aryNewOut(i)(6) = "" Then
                        blnEmpty4 = True
                        strErrEmpty4 &= IIf(strErrEmpty4 = "", i + 1, "," & i + 1)
                    Else
                        If Not Common.checkMaxLength(aryNewOut(i)(6), 100) Then
                            blnCheck10 = True
                            strErrCheck10 &= IIf(strErrCheck10 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if currency is empty
                    If aryNewOut(i)(8) = "" Then
                        blnEmpty5 = True
                        strErrEmpty5 &= IIf(strErrEmpty5 = "", i + 1, "," & i + 1)
                    Else
                        'Error if Exchange Rate is empty and Currency is not 'MYR'
                        If aryNewOut(i)(8) <> "MYR" And aryNewOut(i)(9) = "" Then
                            blnEmpty6 = True
                            strErrEmpty6 &= IIf(strErrEmpty6 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if Exchange Rate is invalid
                    If aryNewOut(i)(9) <> "" Then
                        If chkDec(aryNewOut(i)(9)) Then
                        Else
                            blnCheck4 = True
                            strErrCheck4 &= IIf(strErrCheck4 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    If aryNewOut(i)(10) = "" And aryNewOut(i)(15) = "" And aryNewOut(i)(17) = "" Then
                        blnEmpty7 = True
                        strErrEmpty7 &= IIf(strErrEmpty7 = "", i + 1, "," & i + 1)
                    End If

                    'Error if meal rate is empty
                    If aryNewOut(i)(10) = "" Then
                        'blnEmpty7 = True
                        'strErrEmpty7 &= IIf(strErrEmpty7 = "", i + 1, "," & i + 1)
                    Else
                        If chkDec(aryNewOut(i)(10)) Then
                        Else
                            blnCheck5 = True
                            strErrCheck5 &= IIf(strErrCheck5 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if free meal is empty
                    If aryNewOut(i)(12) <> "" Then
                        If chkInt(aryNewOut(i)(12)) Then
                        Else
                            blnCheck6 = True
                            strErrCheck6 &= IIf(strErrCheck6 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if total acc claimed is empty
                    If aryNewOut(i)(15) <> "" Then
                        If chkDec(aryNewOut(i)(15)) Then
                        Else
                            blnCheck7 = True
                            strErrCheck7 &= IIf(strErrCheck7 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if gst amt is empty
                    If aryNewOut(i)(16) <> "" Then
                        If aryNewOut(i)(15) = "" Then
                            blnEmpty8 = True
                            strErrEmpty8 &= IIf(strErrEmpty8 = "", i + 1, "," & i + 1)
                        End If

                        If chkDec(aryNewOut(i)(16)) Then
                        Else
                            blnCheck11 = True
                            strErrCheck11 &= IIf(strErrCheck11 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if total acc allw is empty
                    If aryNewOut(i)(17) <> "" Then
                        If chkDec(aryNewOut(i)(17)) Then
                        Else
                            blnCheck8 = True
                            strErrCheck8 &= IIf(strErrCheck8 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    If aryNewOut(i)(18) <> "" Then
                        If CDec(aryNewOut(i)(18)) > 0 Then
                        Else
                            blnCheck9 = True
                            strErrCheck9 &= IIf(strErrCheck9 = "", i + 1, "," & i + 1)
                        End If
                    End If
                End If
            Next
        End If

    End Sub

    Private Sub BuildLine()

        'Checkbox, Depart Date, Depart Time, Return Date, Return Time, No Of Day, Destination, Project Code, Currency, Exchange Rate, Meal Rate, Meal Entitled,
        'Free Meal, Actual Meal Claimed, Total Subsistence Allw. Claimed, Total Accommodation Claimed, Gst Amt, Total Acc Allw, Total Amt
        aryNewOut.Add(New String() {"N", "", "00:00", "", "00:00", "", "", "", "MYR", "", "", "", "", "", "", "", "", "", ""})

    End Sub

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        ValidateFields("Save")
    End Sub

    Private Sub cmdSaveSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSaveSummary.Click
        ValidateFields("Summary")
    End Sub

    Private Sub ValidateFields(ByVal strMode As String)
        'Dim blnGL As Boolean = True
        Dim strFullMsg As String = "<ul type='disc'>"
        Rebuild()

        If blnEmpty = True Then
            strMsg &= displayMsg(strErrEmpty, "Departure Date " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty2 = True Then
            strMsg &= displayMsg(strErrEmpty2, "Return Date " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty3 = True Then
            strMsg &= displayMsg(strErrEmpty3, "Departure Time/Return Time " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty4 = True Then
            strMsg &= displayMsg(strErrEmpty4, "Destination " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty5 = True Then
            strMsg &= displayMsg(strErrEmpty5, "Currency " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty6 = True Then
            strMsg &= displayMsg(strErrEmpty6, "Exchange Rate " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty7 = True Then
            'strMsg &= displayMsg(strErrEmpty7, "Meal Rate " & objGlo.GetErrorMessage("00001"))
            strMsg &= displayMsg(strErrEmpty7, "Please enter at least one claim.")
        End If
        If blnEmpty8 = True Then
            strMsg &= displayMsg(strErrEmpty8, "Total Accommodation Claimed " & objGlo.GetErrorMessage("00001"))
        End If
        If blnCheck = True Then
            strMsg &= displayMsg(strErrCheck, "Departure Date has exceeded 110-days policy.")
        End If
        'If blnCheck2 = True Then
        '    strMsg &= displayMsg(strErrCheck2, "Return Date " & objGlo.GetErrorMessage("00015") & " today's date.")
        'End If
        If blnCheck3 = True Then
            strMsg &= displayMsg(strErrCheck3, "Departure Date should be < Return Date.")
        End If
        If blnCheck4 = True Then
            strMsg &= displayMsg(strErrCheck4, "Exchange Rate " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck5 = True Then
            strMsg &= displayMsg(strErrCheck5, "Meal Rate " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck6 = True Then
            strMsg &= displayMsg(strErrCheck6, "Free Meal " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck7 = True Then
            strMsg &= displayMsg(strErrCheck7, "Total Accommodation Claimed " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck8 = True Then
            strMsg &= displayMsg(strErrCheck8, "Total Accommodation Allw " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck11 = True Then
            strMsg &= displayMsg(strErrCheck11, "GST Amt " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck9 = True Then
            strMsg &= displayMsg(strErrCheck9, "Total Amount cannot be zero.")
        End If
        If blnCheck10 = True Then
            strMsg &= displayMsg(strErrCheck10, "Destination is over limit.")
        End If

        strFullMsg &= strMsg & "</ul>"

        If strMsg <> "" Then
            ConstructTableOut()
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

        For i = 0 To aryNewOut.Count - 1
            If aryNewOut(i)(0) = "Y" Then
                'Depart Date, Depart Time, Return Date, Return Time, No Of Day, Actual Meal Claimed
                aryDup.Add(New String() {aryNewOut(i)(1), aryNewOut(i)(2), aryNewOut(i)(3), aryNewOut(i)(4), aryNewOut(i)(5), aryNewOut(i)(11), aryNewOut(i)(13)})
            End If
        Next

        If aryDup.Count > 0 Then
            For i = 0 To aryDup.Count - 1
                'Checkbox, Depart Date, Depart Time, Return Date, Return Time, No Of Day, Destination, Project Code, Currency, Exchange Rate, Meal Rate, Meal Entitled,
                'Free Meal, Actual Meal Claimed, Total Subsistence Allw. Claimed, Total Accommodation Claimed, Total Acc Allw, Total Amt
                aryNewOut.Add(New String() {"N", aryDup(i)(0), aryDup(i)(1), aryDup(i)(2), aryDup(i)(3), aryDup(i)(4), "", "", "MYR", "", "", aryDup(i)(5), "", aryDup(i)(6), "", "", "", "", ""})
            Next
        End If

        ConstructTableOut()
    End Sub

    Private Sub Save(ByVal strMode As String)
        Dim strNewIndex, strNewSCNo As String
        Dim ds As New DataSet
        Dim blnRecord As Boolean
        BindOut(ds, blnRecord)

        If blnRecord = True Then
            If ViewState("index") <> "" Then
                If objStaffClaim.UpdateOutstation(ds, ViewState("scno"), ViewState("index")) Then
                    If strMode = "Summary" Then
                        Common.NetMsgbox(Me, "Staff Claim Number " & ViewState("scno") & " has been updated.", dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                    Else
                        Common.NetMsgbox(Me, "Staff Claim Number " & ViewState("scno") & " has been updated.", dDispatcher.direct("StaffClaim", "OtherClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                    End If
                Else
                    Common.NetMsgbox(Me, objGlo.GetErrorMessage("00007"), MsgBoxStyle.Information)
                End If
            Else
                If objStaffClaim.chkDocExistMonth = True Then
                    Common.NetMsgbox(Me, "You have already created Staff Claim document for this month.", MsgBoxStyle.Information)
                Else
                    If objStaffClaim.SaveOutstation(ds, strNewSCNo, strNewIndex) Then
                        ViewState("index") = strNewIndex
                        ViewState("scno") = strNewSCNo
                        If strMode = "Summary" Then
                            Common.NetMsgbox(Me, "Staff Claim Number " & strNewSCNo & " has been created.", dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                        Else
                            Common.NetMsgbox(Me, "Staff Claim Number " & strNewSCNo & " has been created.", dDispatcher.direct("StaffClaim", "OtherClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                        End If
                    Else
                        Common.NetMsgbox(Me, objGlo.GetErrorMessage("00007"), MsgBoxStyle.Information)
                    End If
                End If
            End If
        Else
            Common.NetMsgbox(Me, objGlo.GetErrorMessage("00131"), MsgBoxStyle.Information)
        End If
        
        ConstructTableOut()

    End Sub

    Private Sub BindOut(ByRef ds As DataSet, ByRef blnRecord As Boolean)
        Dim dtOutMstr As New DataTable
        Dim dtr As DataRow

        blnRecord = False 'By Default
        dtOutMstr.Columns.Add("DEPT_CODE", Type.GetType("System.String"))
        dtr = dtOutMstr.NewRow
        dtr("DEPT_CODE") = lblDeptId.Text

        dtOutMstr.Rows.Add(dtr)
        ds.Tables.Add(dtOutMstr)

        Dim i As Integer, intLine As Integer = 1
        Dim dtOutDtls As New DataTable
        Dim dtrd As DataRow

        dtOutDtls.Columns.Add("LINE_NO", Type.GetType("System.String"))
        dtOutDtls.Columns.Add("DEPART_DATE", Type.GetType("System.String"))
        dtOutDtls.Columns.Add("DEPART_TIME", Type.GetType("System.String"))
        dtOutDtls.Columns.Add("RETURN_DATE", Type.GetType("System.String"))
        dtOutDtls.Columns.Add("RETURN_TIME", Type.GetType("System.String"))
        dtOutDtls.Columns.Add("NO_OF_DAY", Type.GetType("System.String"))
        dtOutDtls.Columns.Add("DESTINATION", Type.GetType("System.String"))
        dtOutDtls.Columns.Add("PROJ_BASED", Type.GetType("System.String"))
        dtOutDtls.Columns.Add("PROJ_CODE", Type.GetType("System.String"))
        dtOutDtls.Columns.Add("CURRENCY_CODE", Type.GetType("System.String"))
        dtOutDtls.Columns.Add("EXCHANGE_RATE", Type.GetType("System.String"))
        dtOutDtls.Columns.Add("MEAL_RATE", Type.GetType("System.String"))
        dtOutDtls.Columns.Add("MEAL_ENTITLED", Type.GetType("System.String"))
        dtOutDtls.Columns.Add("FREE_MEAL", Type.GetType("System.String"))
        dtOutDtls.Columns.Add("ACTUAL_MEAL_CLAIM", Type.GetType("System.String"))
        dtOutDtls.Columns.Add("TOTAL_SUB_ALLW_CLAIM", Type.GetType("System.String"))
        dtOutDtls.Columns.Add("TOTAL_ACC_CLAIM", Type.GetType("System.String"))
        dtOutDtls.Columns.Add("GST_AMT", Type.GetType("System.String"))
        dtOutDtls.Columns.Add("TOTAL_ACC_ALLW", Type.GetType("System.String"))
        dtOutDtls.Columns.Add("TOTAL_CLAIM_AMT", Type.GetType("System.String"))

        For i = 0 To aryNewOut.Count - 1
            If aryNewOut(i)(1) <> "" Then
                blnRecord = True
                dtrd = dtOutDtls.NewRow

                dtrd("LINE_NO") = intLine
                dtrd("DEPART_DATE") = aryNewOut(i)(1) 'Depart Date
                dtrd("DEPART_TIME") = aryNewOut(i)(2) & ":00" 'Depart Time
                dtrd("RETURN_DATE") = aryNewOut(i)(3) 'Return Date
                dtrd("RETURN_TIME") = aryNewOut(i)(4) & ":00" 'Return Time 
                dtrd("NO_OF_DAY") = aryNewOut(i)(5) 'No Of Day
                dtrd("DESTINATION") = aryNewOut(i)(6) 'Destination
                If aryNewOut(i)(7) <> "" Then
                    dtrd("PROJ_CODE") = aryNewOut(i)(7) 'Project Code
                    If aryNewOut(i)(7).ToString.Length > 5 Then
                        dtrd("PROJ_BASED") = "Y" 'Project Based
                    Else
                        dtrd("PROJ_BASED") = "N"
                    End If
                Else
                    dtrd("PROJ_BASED") = "N" 'Project Based
                    dtrd("PROJ_CODE") = "" 'Project Code
                End If
                dtrd("CURRENCY_CODE") = aryNewOut(i)(8) 'Currency
                dtrd("EXCHANGE_RATE") = IIf(aryNewOut(i)(8) = "MYR", "1.00", aryNewOut(i)(9)) 'Exchange Rate
                dtrd("MEAL_RATE") = IIf(aryNewOut(i)(10) = "", "NULL", aryNewOut(i)(10)) 'Meal Rate
                dtrd("MEAL_ENTITLED") = aryNewOut(i)(11) 'Meal Entitled
                dtrd("FREE_MEAL") = IIf(aryNewOut(i)(12) = "", "NULL", aryNewOut(i)(12)) 'Free Meal
                dtrd("ACTUAL_MEAL_CLAIM") = aryNewOut(i)(13) 'Actual Meal Claimed
                dtrd("TOTAL_SUB_ALLW_CLAIM") = aryNewOut(i)(14) 'Total Subsistence Allw
                dtrd("TOTAL_ACC_CLAIM") = IIf(aryNewOut(i)(15) = "", "NULL", aryNewOut(i)(15)) 'Total Accomodation Claimed
                dtrd("GST_AMT") = IIf(aryNewOut(i)(16) = "", "NULL", aryNewOut(i)(16)) 'GST Amt
                dtrd("TOTAL_ACC_ALLW") = IIf(aryNewOut(i)(17) = "", "NULL", aryNewOut(i)(17)) 'Total Accomodation Allw
                dtrd("TOTAL_CLAIM_AMT") = aryNewOut(i)(18) 'Total Amt

                dtOutDtls.Rows.Add(dtrd)
                intLine = intLine + 1
            End If
        Next

        If blnRecord = False And ViewState("index") <> "" Then
            blnRecord = objStaffClaim.chkClaimFormData(ViewState("scno"), "Outstation")
        End If

        ds.Tables.Add(dtOutDtls)
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
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "OverTimeClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Overtime Claim Form</strong></a></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "OutstationClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><font color=""green""><strong>Outstation Claim Form</strong></font></a></span></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "OtherClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Other Claim Form</strong></a></span></td>" & _
        "</tr>" & _
        "</table>" & _
        "</td></tr>"
        Session("w_SC_Links") = strTableLink

    End Sub
End Class
