Imports AgoraLegacy
Imports eProcure.Component

Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Drawing

Public Class EntertainmentClaim
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
    blnEmpty5 As Boolean = True, blnEmpty6 As Boolean = True, blnEmpty7 As Boolean = True, blnEmpty8 As Boolean = True, _
    blnEmpty9 As Boolean = True, blnEmpty10 As Boolean = True, blnEmpty11 As Boolean = True, blnEmpty12 As Boolean = True, blnEmpty13 As Boolean = True
    Dim blnCheck As Boolean = True, blnCheck2 As Boolean = True, blnCheck3 As Boolean = True, blnCheck4 As Boolean = True, _
    blnCheck5 As Boolean = True, blnCheck6 As Boolean = True, blnCheck7 As Boolean = True, blnCheck8 As Boolean = True
    Dim aryNewEntertain As New ArrayList()
    Dim strErrEmpty, strErrEmpty2, strErrEmpty3, strErrEmpty4, strErrEmpty5, strErrEmpty6, strErrEmpty7, strErrEmpty8, _
    strErrEmpty9, strErrEmpty10, strErrEmpty11, strErrEmpty12, strErrEmpty13 As String
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
            Session("aryNewEntertain") = Nothing
            lblScNo.Text = "To Be Allocated By System"
            ViewState("index") = Request.QueryString("index")
            ViewState("mode") = Request.QueryString("mode")
            ViewState("Row") = 1
            'DisplayUserDetails()
            BuildRow()
            ConstructTableEntertain()
            GenerateTab()
            GenerateFormLink()
        End If

        lblMsg.Text = ""
        cmdDupLine.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
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
        ddlSelect.SelectedValue = "Entertain"
    End Sub

    Private Sub BuildRow()
        Dim i, intCount As Integer
        Dim dsEntertain As DataSet
        Dim objUser As New Users
        Dim objUserDetails As New User

        BindDdl()
        aryNewEntertain.Clear()

        dsEntertain = objStaffClaim.GetEntertainDetails(ViewState("index"))

        If dsEntertain.Tables(0).Rows.Count > 0 Then
            lblDocDate.Text = Format(dsEntertain.Tables("MSTR").Rows(0)("SCM_CREATED_DATE"), "dd/MM/yyyy")
            ViewState("scno") = Common.parseNull(dsEntertain.Tables("MSTR").Rows(0)("SCM_CLAIM_DOC_NO"))
            lblScNo.Text = ViewState("scno")
            lblStatus.Text = Common.parseNull(dsEntertain.Tables("MSTR").Rows(0)("STATUS_DESC"))

            objUserDetails = objUser.GetUserDetails(dsEntertain.Tables("MSTR").Rows(0)("SCM_STAFF_ID"), Session("CompanyId"))
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
                If dsEntertain.Tables("MSTR").Rows(0)("SCM_STATUS") <> SCStatus.DraftSC And dsEntertain.Tables("MSTR").Rows(0)("SCM_STATUS") <> SCStatus.Rejected Then
                    cmdAddClaim.Visible = False
                    cmdDupLine.Visible = False
                    cmdSave.Visible = False
                    cmdSaveSummary.Visible = False
                    cmdClear.Visible = False
                    ViewState("SCmode") = "v"
                    trInfo.Style("display") = "none"
                End If
            End If
            If dsEntertain.Tables("DETAILS").Rows.Count > ViewState("Row") Then
                intCount = dsEntertain.Tables("DETAILS").Rows.Count
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
            'Checkbox, Date, Person Ent, No of Pax, Organisation Represented, Client Status, Project Code, Purpose, Meal Type, Meal Place, Tax Invoice Type, Currency, Exchange Rate, Amount, GST Amt, Total Amt, Exceed Amt
            aryNewEntertain.Add(New String() {"N", "", "", "", "", "", "", "", "", "", "N", "MYR", "", "", "0.00", "", ""})
        Next

        'Put all data in array if record found
        If dsEntertain.Tables("DETAILS").Rows.Count > 0 Then
            For i = 0 To dsEntertain.Tables("DETAILS").Rows.Count - 1
                aryNewEntertain(i)(1) = Common.parseNull(dsEntertain.Tables("DETAILS").Rows(i)("SCED_DATE")) 'Date
                aryNewEntertain(i)(2) = Common.parseNull(dsEntertain.Tables("DETAILS").Rows(i)("SCED_PERSON_ENT")) 'Person Entertained 
                aryNewEntertain(i)(3) = Common.parseNull(dsEntertain.Tables("DETAILS").Rows(i)("SCED_NO_PAX")) 'No of Pax
                aryNewEntertain(i)(4) = Common.parseNull(dsEntertain.Tables("DETAILS").Rows(i)("SCED_ORG_REP")) 'Organisation Represented
                aryNewEntertain(i)(5) = Common.parseNull(dsEntertain.Tables("DETAILS").Rows(i)("SCED_CLIENT_STATUS")) 'Client Status
                aryNewEntertain(i)(6) = Common.parseNull(dsEntertain.Tables("DETAILS").Rows(i)("SCED_PROJ_CODE")) 'Project Code
                aryNewEntertain(i)(7) = Common.parseNull(dsEntertain.Tables("DETAILS").Rows(i)("SCED_PURPOSE")) 'Purpose
                aryNewEntertain(i)(8) = Common.parseNull(dsEntertain.Tables("DETAILS").Rows(i)("SCED_TYPE")) 'Meal Type
                aryNewEntertain(i)(9) = Common.parseNull(dsEntertain.Tables("DETAILS").Rows(i)("SCED_MEAL_PLACE")) 'Meal Place
                aryNewEntertain(i)(10) = Common.parseNull(dsEntertain.Tables("DETAILS").Rows(i)("SCED_TAX_INV_TYPE")) 'Tax Invoice Type
                aryNewEntertain(i)(11) = Common.parseNull(dsEntertain.Tables("DETAILS").Rows(i)("SCED_CURRENCY_CODE")) 'Currency
                aryNewEntertain(i)(12) = IIf(aryNewEntertain(i)(11) = "MYR", "", Common.parseNull(dsEntertain.Tables("DETAILS").Rows(i)("SCED_EXCHANGE_RATE"))) 'Exchange Rate
                aryNewEntertain(i)(13) = Common.parseNull(dsEntertain.Tables("DETAILS").Rows(i)("SCED_AMT")) 'Amount
                aryNewEntertain(i)(14) = Common.parseNull(dsEntertain.Tables("DETAILS").Rows(i)("SCED_GST_AMT")) 'GST Amt
                aryNewEntertain(i)(15) = Common.parseNull(dsEntertain.Tables("DETAILS").Rows(i)("SCED_TOTAL_AMT")) 'Total Amt
                aryNewEntertain(i)(16) = Common.parseNull(dsEntertain.Tables("DETAILS").Rows(i)("SCED_EXCEED_AMT")) 'Exceed Amt
            Next
        End If

    End Sub

    Private Sub ConstructTableEntertain()
        Dim table As String
        Dim strrow As String = ""
        Dim strrowTotal As String = ""
        Dim i, c, count As Integer
        Dim decTotalAmount, decGrandTotalAmount As Decimal
        Dim blnChk As Boolean = True
        Dim aryClient, aryEntRate, aryTaxInvType As New ArrayList()
        Dim dsCurr, dsTaxCode As New DataSet

        aryClient.Add(New String() {"Existing Customer (EC)", "EC"})
        aryClient.Add(New String() {"Existing Supplier (ES)", "ES"})
        aryClient.Add(New String() {"Potential Customer (PC)", "PC"})
        aryClient.Add(New String() {"Potential Supplier (PS)", "PS"})
        aryClient.Add(New String() {"Non Customer/Supplier (NOC)", "NOC"})
        aryClient.Add(New String() {"Own Company Staff (CST)", "CST"})
        aryClient.Add(New String() {"Related Company Staff (RST)", "RST"})
        aryClient.Add(New String() {"Flow through Sales : Existing Customer (FEC)", "FEC"})
        aryClient.Add(New String() {"Flow through Sales : Existing Supplier (FES)", "FES"})
        
        aryEntRate.Add(New String() {"Breakfast(RM50)", "B50"})
        aryEntRate.Add(New String() {"Breakfast(RM30)", "B30"})
        aryEntRate.Add(New String() {"Lunch(RM50)", "L50"})
        aryEntRate.Add(New String() {"Lunch(RM30)", "L30"})
        aryEntRate.Add(New String() {"Dinner(RM50)", "D50"})

        aryTaxInvType.Add(New String() {"Full", "F"})
        aryTaxInvType.Add(New String() {"Simplified", "S"})
        aryTaxInvType.Add(New String() {"Non-Tax Invoice", "N"})

        dsCurr = objStaffClaim.getDdlInfo("1")

        count = aryNewEntertain.Count
        hidItemLine.Value = count

        For i = 0 To count - 1

            strrow &= "<tr>"

            strrow &= "<td align=""center"" class=""tablecol"">"
            If aryNewEntertain(i)(0) = "Y" Then
                strrow &= "<input type=""checkbox"" id=""chkSelection" & i & """ checked name=""chkSelection" & i & """ onclick=""checkChild('chkSelection" & i & "')"">"
            Else
                strrow &= "<input type=""checkbox"" id=""chkSelection" & i & """ name=""chkSelection" & i & """ onclick=""checkChild('chkSelection" & i & "')"">"
                blnChk = False
            End If
            strrow &= "</td>"

            strrow &= "<td class=""tablecol"" align=""center"">" & i + 1
            strrow &= "</td>"

            'Date
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:80px;margin-right:0px; "" contentEditable=""false"" class=""txtbox"" type=""text"" id=""txtDate" & i & """ name=""txtDate" & i & """ value=""" & aryNewEntertain(i)(1) & """>"
            strrow &= "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDate" & i) & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            strrow &= "</td>"

            'Person Ent
            strrow &= "<td class=""tablecol"">"
            'strrow &= "<input style=""width:120px;margin-right:0px; "" maxlength=""1000"" class=""txtbox"" type=""text"" id=""txtPE" & i & """ name=""txtPE" & i & """ value=""" & aryNewEntertain(i)(2) & """>"
            strrow &= "<textarea rows=""2"" style=""font-family:Arial,Verdana;font-size:11px;width:120px;margin-right:0px; "" maxlength=""1000"" id=""txtPE" & i & """ name=""txtPE" & i & """>" & aryNewEntertain(i)(2) & "</textarea>"
            strrow &= "</td>"

            'No of Pax
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:100px;margin-right:0px; "" onkeypress=""return isNumberKey(event)"" maxlength=""10"" onkeyup=""calExceedAmt(" & i & ")"" class=""numerictxtbox"" type=""text"" id=""txtNoPax" & i & """ name=""txtNoPax" & i & """ value=""" & aryNewEntertain(i)(3) & """>"
            strrow &= "</td>"

            'Organisation Represented
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:120px;margin-right:0px; "" maxlength=""100"" class=""txtbox"" type=""text"" id=""txtOR" & i & """ name=""txtOR" & i & """ value=""" & aryNewEntertain(i)(4) & """>"
            strrow &= "</td>"

            'Client Status
            strrow &= "<td class=""tablecol"">"
            strrow &= "<select class=""ddl"" style=""width:100%;margin-right:0px;"" id=""cboClient" & i & """ name=""cboClient" & i & """>"
            strrow &= "<option value="""" selected=""selected"">" & "--Select--" & "</option>"
            For c = 0 To aryClient.Count - 1
                If aryNewEntertain(i)(5) = aryClient(c)(1) Then
                    strrow &= "<option title=""" & aryClient(c)(0).ToString & """ value=""" & aryClient(c)(1).ToString & """ selected=""selected"">" & aryClient(c)(0).ToString & "</option>"
                Else
                    strrow &= "<option title=""" & aryClient(c)(0).ToString & """ value=""" & aryClient(c)(1).ToString & """>" & aryClient(c)(0).ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "</td>"

            'Project Code
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:160px;margin-right:0px; "" maxlength=""50"" class=""txtbox"" type=""text"" id=""txtPC" & i & """ name=""txtPC" & i & """ value=""" & aryNewEntertain(i)(6) & """>"
            strrow &= "</td>"

            'Purpose
            strrow &= "<td class=""tablecol"">"
            strrow &= "<textarea rows=""2"" style=""font-family:Arial,Verdana;font-size:11px;width:200px;margin-right:0px; "" maxlength=""500"" id=""txtPurpose" & i & """ name=""txtPurpose" & i & """>" & aryNewEntertain(i)(7) & "</textarea>"
            strrow &= "</td>"

            'Meal Type
            strrow &= "<td class=""tablecol"">"
            strrow &= "<select class=""ddl"" style=""width:100%;margin-right:0px;"" id=""cboEntRate" & i & """ onchange=""calExceedAmt(" & i & ")"" name=""cboEntRate" & i & """>"
            strrow &= "<option value="""" selected=""selected"">" & "--Select--" & "</option>"
            For c = 0 To aryEntRate.Count - 1
                If aryNewEntertain(i)(8) = aryEntRate(c)(1) Then
                    strrow &= "<option title=""" & aryEntRate(c)(0).ToString & """ value=""" & aryEntRate(c)(1).ToString & """ selected=""selected"">" & aryEntRate(c)(0).ToString & "</option>"
                Else
                    strrow &= "<option title=""" & aryEntRate(c)(0).ToString & """ value=""" & aryEntRate(c)(1).ToString & """>" & aryEntRate(c)(0).ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "</td>"

            'Meal Place
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:160px;margin-right:0px; "" maxlength=""100"" class=""txtbox"" type=""text"" id=""txtMP" & i & """ name=""txtMP" & i & """ value=""" & aryNewEntertain(i)(9) & """>"
            strrow &= "</td>"

            'Tax Inv Type
            strrow &= "<td class=""tablecol"">"
            strrow &= "<select class=""ddl"" style=""width:100%;margin-right:0px;"" id=""cboTaxInvType" & i & """ onchange=""enableGstAmt(" & i & ")"" name=""cboTaxInvType" & i & """>"
            'strrow &= "<option value="""" selected=""selected"">" & "--Select--" & "</option>"
            For c = 0 To aryTaxInvType.Count - 1
                If aryNewEntertain(i)(10) = aryTaxInvType(c)(1) Then
                    strrow &= "<option title=""" & aryTaxInvType(c)(0).ToString & """ value=""" & aryTaxInvType(c)(1).ToString & """ selected=""selected"">" & aryTaxInvType(c)(0).ToString & "</option>"
                Else
                    strrow &= "<option title=""" & aryTaxInvType(c)(0).ToString & """ value=""" & aryTaxInvType(c)(1).ToString & """>" & aryTaxInvType(c)(0).ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "</td>"

            'Currency
            strrow &= "<td class=""tablecol"">"
            strrow &= "<select class=""ddl"" style=""width:100%;margin-right:0px;"" id=""cboCurr" & i & """ onchange=""enableExRate(" & i & ")"" name=""cboCurr" & i & """>"
            strrow &= "<option value="""" selected=""selected"">" & "--Select--" & "</option>"
            For c = 0 To dsCurr.Tables(0).Rows.Count - 1
                If aryNewEntertain(i)(11) = dsCurr.Tables(0).Rows(c)(0) Then
                    strrow &= "<option title=""" & dsCurr.Tables(0).Rows(c)(0).ToString & """ value=""" & dsCurr.Tables(0).Rows(c)(0).ToString & """ selected=""selected"">" & dsCurr.Tables(0).Rows(c)(0).ToString & "</option>"
                Else
                    strrow &= "<option title=""" & dsCurr.Tables(0).Rows(c)(0).ToString & """ value=""" & dsCurr.Tables(0).Rows(c)(0).ToString & """>" & dsCurr.Tables(0).Rows(c)(0).ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "</td>"

            'Exchange Rate
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:75px;margin-right:0px;"" " & IIf(aryNewEntertain(i)(11) <> "MYR", "", "disabled") & " onkeypress=""return isDecimalKey(event)"" onkeyup=""calTotalAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtExRate" & i & """ name=""txtExRate" & i & """ value=""" & aryNewEntertain(i)(12) & """>"
            strrow &= "</td>"

            'Amount
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:75px;margin-right:0px;"" onkeypress=""return isDecimalKey(event)"" onkeyup=""calTotalAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtAmt" & i & """ name=""txtAmt" & i & """ value=""" & aryNewEntertain(i)(13) & """>"
            strrow &= "</td>"

            'GST Amount
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:75px;margin-right:0px;"" " & IIf(aryNewEntertain(i)(10) <> "N", "", "disabled") & " onkeypress=""return isDecimalKey(event)"" onkeyup=""calTotalAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtGSTAmt" & i & """ name=""txtGSTAmt" & i & """ value=""" & aryNewEntertain(i)(14) & """>"
            strrow &= "</td>"

            'Total Amt
            strrow &= "<td class=""tablecol"">"
            strrow &= "<label id=""lblTotalAmt" & i & """ style=""width:75px;margin-right:0px; text-align:right;"">" & aryNewEntertain(i)(15) & "</label>"
            strrow &= "</td>"

            'Exceed Amt
            strrow &= "<td class=""tablecol"">"
            strrow &= "<label id=""lblExceedAmt" & i & """ style=""width:75px;margin-right:0px; text-align:right;"">" & aryNewEntertain(i)(16) & "</label>"
            strrow &= "</td>"

            strrow &= "</tr>"
        Next

        Dim a As String = "<span class=""errormsg"">*</span>"
        table = "<table cellspacing=""0"" cellpadding=""0"" width=""1915px"" border=""0"">" & _
                           "<tr class=""TableHeader"">" & _
                           "<td margin-right:0px;"" width=""30px"" align=""center""><input type=""checkbox"" id=""chkAll"" " & IIf(blnChk = True, "checked", "") & " name=""chkAll"" onclick=""selectAll();""></td>" & _
                           "<td margin-right:0px;"" width=""30px"">No</td>" & _
                           "<td margin-right:0px;"" width=""120px"">Date" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""120px"">Person Entertained" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""100px"" align=""right"">No. of Pax (Incl. Yourself)" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""120px"">Organisation Represented" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""200px"">Client Status" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""160px"">Project Code</td>" & _
                           "<td margin-right:0px;"" width=""200px"">Purpose" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""120px"">Meal Type" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""160px"">Meal Place" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""100px"">Tax Invoice Type" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""80px"">Currency" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""75px"" align=""right"">Exchange Rate</td>" & _
                           "<td margin-right:0px;"" width=""75px"" align=""right"">Amount" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""75px"" align=""right"">GST Amt" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""75px"" align=""right"">Total Amt (MYR)</td>" & _
                           "<td margin-right:0px;"" width=""75px"" align=""right"">Exceed Amt (MYR)</td>" & _
                           "</tr>" & _
                           strrow & _
                           "</table>"

        Session("ConstructTableEntertain") = table
        Session("aryNewEntertain") = aryNewEntertain
    End Sub

    Private Function CalTotalAmt(ByVal strCurrency As String, ByVal strExRate As String, ByVal strAmt As String, ByVal strGstAmt As String) As String
        Dim decAmt, decGstAmt As Decimal
        Dim decTotalAmt As Decimal = 0
        Dim decExRate As Decimal = 0

        CalTotalAmt = ""
        If strAmt <> "" Or strGstAmt <> "" Then
            If strCurrency = "MYR" Then
                decExRate = 1
            Else
                If strExRate <> "" And chkDec(strExRate) Then
                    decExRate = CDec(strExRate)
                End If
            End If
            If chkDec(strAmt, False) Then
                decAmt = CDec(strAmt)
                decTotalAmt = decTotalAmt + decAmt
            End If
            If chkDec(strGstAmt) Then
                decGstAmt = CDec(strGstAmt)
                decTotalAmt = decTotalAmt + decGstAmt
            End If
            decTotalAmt = decTotalAmt * decExRate
            CalTotalAmt = Format(decTotalAmt, "###0.00")
        Else

        End If
    End Function

    Private Function CalExceedAmt(ByVal strNoOfPax As String, ByVal strEntRate As String, ByVal strClaimAmt As String) As String
        Dim decRate, decClaimAmt, decExceedAmt As Decimal
        Dim intNoPax As Integer

        CalExceedAmt = ""
        If strNoOfPax <> "" And strEntRate <> "" And strClaimAmt <> "" Then
            If chkInt(strNoOfPax) And chkDec(strClaimAmt) Then
                decRate = CDec(Right(strEntRate, 2))
                decClaimAmt = CDec(strClaimAmt)
                intNoPax = CInt(strNoOfPax)
                decExceedAmt = decClaimAmt - (decRate * intNoPax)
                If decExceedAmt > 0 Then
                    CalExceedAmt = Format(decExceedAmt, "###0.00")
                End If
            End If
        End If
    End Function

    Private Sub cmdAddClaim_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddClaim.Click
        Rebuild(False)
        BuildLine()
        ConstructTableEntertain()
    End Sub

    Private Sub Rebuild(Optional ByVal blnChk As Boolean = True)
        Dim i As Integer
        Dim parsedDate As Date
        blnEmpty = False
        blnEmpty2 = False
        blnEmpty3 = False
        blnEmpty4 = False
        blnEmpty5 = False
        blnEmpty6 = False
        blnEmpty7 = False
        blnEmpty8 = False
        blnEmpty9 = False
        blnEmpty10 = False
        blnEmpty11 = False
        blnEmpty12 = False
        blnEmpty13 = False
        blnCheck = False
        blnCheck2 = False
        blnCheck3 = False
        blnCheck4 = False
        blnCheck5 = False
        blnCheck6 = False
        blnCheck7 = False
        blnCheck8 = False

        aryNewEntertain = Session("aryNewEntertain")
        For i = 0 To aryNewEntertain.Count - 1
            aryNewEntertain(i)(0) = IIf(Request.Form("chkSelection" & i) = "on", "Y", "N") 'Checkbox
            aryNewEntertain(i)(1) = Request.Form("txtDate" & i) 'Date
            aryNewEntertain(i)(2) = Request.Form("txtPE" & i) 'Person Entertained 
            aryNewEntertain(i)(3) = Request.Form("txtNoPax" & i) 'No of Pax
            aryNewEntertain(i)(4) = Request.Form("txtOR" & i) 'Organisation Represented
            aryNewEntertain(i)(5) = Request.Form("cboClient" & i) 'Client Status
            aryNewEntertain(i)(6) = Request.Form("txtPC" & i) 'Project Code
            aryNewEntertain(i)(7) = Request.Form("txtPurpose" & i) 'Purpose
            aryNewEntertain(i)(8) = Request.Form("cboEntRate" & i) 'Meal Type
            aryNewEntertain(i)(9) = Request.Form("txtMP" & i) 'Meal Type
            aryNewEntertain(i)(10) = Request.Form("cboTaxInvType" & i) 'Tax Invoice Type
            aryNewEntertain(i)(11) = Request.Form("cboCurr" & i) 'Currency
            aryNewEntertain(i)(12) = Request.Form("txtExRate" & i) 'Exchange Rate
            aryNewEntertain(i)(13) = Request.Form("txtAmt" & i) 'Amount
            If Request.Form("cboTaxInvType" & i) = "N" Then
                aryNewEntertain(i)(14) = "0.00" 'GST Amt
            Else
                aryNewEntertain(i)(14) = Request.Form("txtGSTAmt" & i) 'GST Amt
            End If

            aryNewEntertain(i)(15) = CalTotalAmt(aryNewEntertain(i)(11), aryNewEntertain(i)(12), aryNewEntertain(i)(13), aryNewEntertain(i)(14)) 'Total Amt
            aryNewEntertain(i)(16) = CalExceedAmt(aryNewEntertain(i)(3), aryNewEntertain(i)(8), aryNewEntertain(i)(15)) 'Exceed Amt
        Next

        If blnChk = True Then
            For i = 0 To aryNewEntertain.Count - 1
                If aryNewEntertain(i)(1) <> "" Or aryNewEntertain(i)(2) <> "" Or aryNewEntertain(i)(3) <> "" Or aryNewEntertain(i)(4) <> "" Or _
                aryNewEntertain(i)(5) <> "" Or aryNewEntertain(i)(6) <> "" Or aryNewEntertain(i)(7) <> "" Or aryNewEntertain(i)(8) <> "" Or _
                aryNewEntertain(i)(9) <> "" Or aryNewEntertain(i)(12) <> "" Or aryNewEntertain(i)(13) <> "" Then
                    'Error if date is empty
                    If aryNewEntertain(i)(1) = "" Then
                        blnEmpty = True
                        strErrEmpty &= IIf(strErrEmpty = "", i + 1, "," & i + 1)
                    Else
                        'Error if Date is invalid
                        If DateDiff(DateInterval.Day, CDate(aryNewEntertain(i)(1)), Today.Now()) > 110 Then
                            blnCheck = True
                            strErrCheck &= IIf(strErrCheck = "", i + 1, "," & i + 1)
                        End If

                        'Error if Date greater than today's date
                        If CDate(aryNewEntertain(i)(1)) > Today.Now() Then
                            blnCheck2 = True
                            strErrCheck2 &= IIf(strErrCheck2 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if person entertained is empty
                    If aryNewEntertain(i)(2) = "" Then
                        blnEmpty2 = True
                        strErrEmpty2 &= IIf(strErrEmpty2 = "", i + 1, "," & i + 1)
                    Else
                        If Not Common.checkMaxLength(aryNewEntertain(i)(2), 1000) Then
                            blnCheck8 = True
                            strErrCheck8 &= IIf(strErrCheck8 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if no of pax is empty
                    If aryNewEntertain(i)(3) = "" Then
                        blnEmpty3 = True
                        strErrEmpty3 &= IIf(strErrEmpty3 = "", i + 1, "," & i + 1)
                    Else
                        If chkInt(aryNewEntertain(i)(3)) Then
                        Else
                            blnCheck3 = True
                            strErrCheck3 &= IIf(strErrCheck3 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if organisation represented is empty
                    If aryNewEntertain(i)(4) = "" Then
                        blnEmpty4 = True
                        strErrEmpty4 &= IIf(strErrEmpty4 = "", i + 1, "," & i + 1)
                    End If

                    'Error if client status is empty
                    If aryNewEntertain(i)(5) = "" Then
                        blnEmpty5 = True
                        strErrEmpty5 &= IIf(strErrEmpty5 = "", i + 1, "," & i + 1)
                    End If

                    'Error if purpose is empty
                    If aryNewEntertain(i)(7) = "" Then
                        blnEmpty6 = True
                        strErrEmpty6 &= IIf(strErrEmpty6 = "", i + 1, "," & i + 1)
                    Else
                        If Not Common.checkMaxLength(aryNewEntertain(i)(7), 500) Then
                            blnCheck7 = True
                            strErrCheck7 &= IIf(strErrCheck7 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if type is empty
                    If aryNewEntertain(i)(8) = "" Then
                        blnEmpty7 = True
                        strErrEmpty7 &= IIf(strErrEmpty7 = "", i + 1, "," & i + 1)
                    End If

                    'Error if meal place is empty
                    If aryNewEntertain(i)(9) = "" Then
                        blnEmpty13 = True
                        strErrEmpty13 &= IIf(strErrEmpty13 = "", i + 1, "," & i + 1)
                    End If

                    'Error if type is empty
                    If aryNewEntertain(i)(10) = "" Then
                        blnEmpty8 = True
                        strErrEmpty8 &= IIf(strErrEmpty8 = "", i + 1, "," & i + 1)
                    End If

                    'Error if currency is empty
                    If aryNewEntertain(i)(11) = "" Then
                        blnEmpty9 = True
                        strErrEmpty9 &= IIf(strErrEmpty9 = "", i + 1, "," & i + 1)
                    Else
                        'Error if Exchange Rate is empty and Currency is not 'MYR'
                        If aryNewEntertain(i)(11) <> "MYR" And aryNewEntertain(i)(12) = "" Then
                            blnEmpty10 = True
                            strErrEmpty10 &= IIf(strErrEmpty10 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if amount is empty
                    If aryNewEntertain(i)(13) = "" Then
                        blnEmpty11 = True
                        strErrEmpty11 &= IIf(strErrEmpty11 = "", i + 1, "," & i + 1)
                    Else
                        'Error if amount is invalid
                        If chkDec(aryNewEntertain(i)(13), False) Then
                        Else
                            blnCheck4 = True
                            strErrCheck4 &= IIf(strErrCheck4 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if gst amount is empty
                    If aryNewEntertain(i)(14) = "" Then
                        blnEmpty12 = True
                        strErrEmpty12 &= IIf(strErrEmpty12 = "", i + 1, "," & i + 1)
                    Else
                        'Error if gst amt is invalid
                        If chkDec(aryNewEntertain(i)(14)) Then
                        Else
                            blnCheck5 = True
                            strErrCheck5 &= IIf(strErrCheck5 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if Exchange Rate is invalid
                    If aryNewEntertain(i)(12) <> "" Then
                        If chkDec(aryNewEntertain(i)(12), False) Then
                        Else
                            blnCheck6 = True
                            strErrCheck6 &= IIf(strErrCheck6 = "", i + 1, "," & i + 1)
                        End If
                    End If
                End If
            Next
        End If

    End Sub

    Private Function chkInt(ByVal strValue As String) As Boolean
        If IsNumeric(strValue) And Regex.IsMatch(Trim(strValue), "^-?[0-9]+$") Then
            chkInt = True
        Else
            chkInt = False
        End If
    End Function

    Private Function chkDec(ByVal strValue As String, Optional ByVal blnZero As Boolean = True) As Boolean
        Dim strRegex As String

        If blnZero = True Then
            strRegex = "^\d{1,10}(\.\d{1,2})?$"
        Else
            strRegex = "(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,2})?$"
        End If
        If IsNumeric(strValue) And Regex.IsMatch(Trim(strValue), strRegex) Then
            chkDec = True
        Else
            chkDec = False
        End If
    End Function

    Private Sub BuildLine()

        'Checkbox, Date, Person Ent, No of Pax, Organisation Represented, Client Status, Project Code, Purpose, Meal Type, Meal Place, Tax Invoice Type, Currency, Exchange Rate, Amount, GST Amt, Total Amt, Exceed Amt
        aryNewEntertain.Add(New String() {"N", "", "", "", "", "", "", "", "", "", "N", "MYR", "", "", "0.00", "", ""})

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
            strMsg &= displayMsg(strErrEmpty, "Date " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty2 = True Then
            strMsg &= displayMsg(strErrEmpty2, "Person Entertained " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty3 = True Then
            strMsg &= displayMsg(strErrEmpty3, "No Of Pax " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty4 = True Then
            strMsg &= displayMsg(strErrEmpty4, "Organisation Represented " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty5 = True Then
            strMsg &= displayMsg(strErrEmpty5, "Client Status " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty6 = True Then
            strMsg &= displayMsg(strErrEmpty6, "Purpose " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty7 = True Then
            strMsg &= displayMsg(strErrEmpty7, "Meal Type " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty13 = True Then
            strMsg &= displayMsg(strErrEmpty13, "Meal Place " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty8 = True Then
            strMsg &= displayMsg(strErrEmpty8, "Tax Invoice Type " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty9 = True Then
            strMsg &= displayMsg(strErrEmpty9, "Currency " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty10 = True Then
            strMsg &= displayMsg(strErrEmpty10, "Exchange Rate " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty11 = True Then
            strMsg &= displayMsg(strErrEmpty11, "Amount " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty12 = True Then
            strMsg &= displayMsg(strErrEmpty12, "GST Amount " & objGlo.GetErrorMessage("00001"))
        End If
        If blnCheck = True Then
            strMsg &= displayMsg(strErrCheck, "Date has exceeded 110-days policy.")
        End If
        If blnCheck2 = True Then
            strMsg &= displayMsg(strErrCheck2, "Date " & objGlo.GetErrorMessage("00015") & " today's date.")
        End If
        If blnCheck3 = True Then
            strMsg &= displayMsg(strErrCheck3, "No Of Pax " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck4 = True Then
            strMsg &= displayMsg(strErrCheck4, "Amount " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck5 = True Then
            strMsg &= displayMsg(strErrCheck5, "GST Amount " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck6 = True Then
            strMsg &= displayMsg(strErrCheck6, "Exchange Rate " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck7 = True Then
            strMsg &= displayMsg(strErrCheck7, "Purpose is over limit.")
        End If
        If blnCheck8 = True Then
            strMsg &= displayMsg(strErrCheck8, "Person Entertained is over limit.")
        End If

        strFullMsg &= strMsg & "</ul>"

        If strMsg <> "" Then
            ConstructTableEntertain()
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

        For i = 0 To aryNewEntertain.Count - 1
            If aryNewEntertain(i)(0) = "Y" Then
                'Date, Person Ent, No of Pax, Organisation Represented
                aryDup.Add(New String() {aryNewEntertain(i)(1), aryNewEntertain(i)(2), aryNewEntertain(i)(3), aryNewEntertain(i)(4)})
            End If
        Next

        If aryDup.Count > 0 Then
            For i = 0 To aryDup.Count - 1
                'Checkbox, Date, Person Ent, No of Pax, Organisation Represented, Client Status, Project Code, Purpose, Meal Type, Meal Place, Tax Invoice Type, Currency, Exchange Rate, Amount, GST Amt, Total Amt, Exceed Amt
                aryNewEntertain.Add(New String() {"N", aryDup(i)(0), aryDup(i)(1), aryDup(i)(2), aryDup(i)(3), "", "", "", "", "", "N", "MYR", "", "", "0.00", "", ""})
            Next
        End If

        ConstructTableEntertain()
    End Sub

    Private Sub Save(ByVal strMode As String)
        Dim strNewIndex, strNewSCNo As String
        Dim ds As New DataSet
        Dim blnRecord As Boolean
        BindEnt(ds, blnRecord)

        If blnRecord = True Then
            If ViewState("index") <> "" Then
                If objStaffClaim.UpdateEntertain(ds, ViewState("scno"), ViewState("index")) Then
                    If strMode = "Summary" Then
                        Common.NetMsgbox(Me, "Staff Claim Number " & ViewState("scno") & " has been updated.", dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                    Else
                        Common.NetMsgbox(Me, "Staff Claim Number " & ViewState("scno") & " has been updated.", dDispatcher.direct("StaffClaim", "HardShipClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                    End If
                Else
                    Common.NetMsgbox(Me, objGlo.GetErrorMessage("00007"), MsgBoxStyle.Information)
                End If
            Else
                If objStaffClaim.chkDocExistMonth = True Then
                    Common.NetMsgbox(Me, "You have already created Staff Claim document for this month.", MsgBoxStyle.Information)
                Else
                    If objStaffClaim.SaveEntertain(ds, strNewSCNo, strNewIndex) Then
                        ViewState("index") = strNewIndex
                        ViewState("scno") = strNewSCNo
                        If strMode = "Summary" Then
                            Common.NetMsgbox(Me, "Staff Claim Number " & strNewSCNo & " has been created.", dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                        Else
                            Common.NetMsgbox(Me, "Staff Claim Number " & strNewSCNo & " has been created.", dDispatcher.direct("StaffClaim", "HardShipClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                        End If
                    Else
                        Common.NetMsgbox(Me, objGlo.GetErrorMessage("00007"), MsgBoxStyle.Information)
                    End If
                End If
            End If
        Else
            Common.NetMsgbox(Me, objGlo.GetErrorMessage("00131"), MsgBoxStyle.Information)
        End If
        
        ConstructTableEntertain()

    End Sub

    Private Sub BindEnt(ByRef ds As DataSet, ByRef blnRecord As Boolean)
        Dim dtEntMstr As New DataTable
        Dim dtr As DataRow

        blnRecord = False 'By Default
        dtEntMstr.Columns.Add("DEPT_CODE", Type.GetType("System.String"))
        dtr = dtEntMstr.NewRow
        dtr("DEPT_CODE") = lblDeptId.Text

        dtEntMstr.Rows.Add(dtr)
        ds.Tables.Add(dtEntMstr)

        Dim i As Integer, intLine As Integer = 1
        Dim dtEntDtls As New DataTable
        Dim dtrd As DataRow

        dtEntDtls.Columns.Add("LINE_NO", Type.GetType("System.String"))
        dtEntDtls.Columns.Add("DATE", Type.GetType("System.String"))
        dtEntDtls.Columns.Add("PERSON_ENT", Type.GetType("System.String"))
        dtEntDtls.Columns.Add("NO_PAX", Type.GetType("System.String"))
        dtEntDtls.Columns.Add("ORG_REP", Type.GetType("System.String"))
        dtEntDtls.Columns.Add("CLIENT_STATUS", Type.GetType("System.String"))
        dtEntDtls.Columns.Add("PROJ_CODE", Type.GetType("System.String"))
        dtEntDtls.Columns.Add("PROJ_BASED", Type.GetType("System.String"))
        dtEntDtls.Columns.Add("PURPOSE", Type.GetType("System.String"))
        dtEntDtls.Columns.Add("TYPE", Type.GetType("System.String"))
        dtEntDtls.Columns.Add("MEAL_PLACE", Type.GetType("System.String"))
        dtEntDtls.Columns.Add("TAX_INV_TYPE", Type.GetType("System.String"))
        dtEntDtls.Columns.Add("CURRENCY_CODE", Type.GetType("System.String"))
        dtEntDtls.Columns.Add("EXCHANGE_RATE", Type.GetType("System.String"))
        dtEntDtls.Columns.Add("AMT", Type.GetType("System.String"))
        dtEntDtls.Columns.Add("GST_AMT", Type.GetType("System.String"))
        dtEntDtls.Columns.Add("TOTAL_AMT", Type.GetType("System.String"))
        dtEntDtls.Columns.Add("EXCEED_AMT", Type.GetType("System.String"))

        For i = 0 To aryNewEntertain.Count - 1
            If aryNewEntertain(i)(1) <> "" Then
                blnRecord = True
                dtrd = dtEntDtls.NewRow

                dtrd("LINE_NO") = intLine
                dtrd("DATE") = aryNewEntertain(i)(1) 'Date
                dtrd("PERSON_ENT") = aryNewEntertain(i)(2) 'Person Entertained 
                dtrd("NO_PAX") = aryNewEntertain(i)(3) 'No of Pax
                dtrd("ORG_REP") = aryNewEntertain(i)(4) 'Organisation Represented
                dtrd("CLIENT_STATUS") = aryNewEntertain(i)(5) 'Client Status
                If aryNewEntertain(i)(6) <> "" Then
                    dtrd("PROJ_CODE") = aryNewEntertain(i)(6) 'Project Code
                    If aryNewEntertain(i)(6).ToString.Length > 5 Then
                        dtrd("PROJ_BASED") = "Y" 'Project Based
                    Else
                        dtrd("PROJ_BASED") = "N"
                    End If
                Else
                    dtrd("PROJ_BASED") = "N" 'Project Based
                    dtrd("PROJ_CODE") = "" 'Project Code
                End If
                dtrd("PURPOSE") = aryNewEntertain(i)(7) 'Purpose
                dtrd("TYPE") = aryNewEntertain(i)(8) 'Meal Type
                dtrd("MEAL_PLACE") = aryNewEntertain(i)(9) 'Meal Type
                dtrd("TAX_INV_TYPE") = aryNewEntertain(i)(10) 'Tax Invoice Type
                dtrd("CURRENCY_CODE") = aryNewEntertain(i)(11) 'Currency
                dtrd("EXCHANGE_RATE") = IIf(aryNewEntertain(i)(11) = "MYR", "1.00", aryNewEntertain(i)(12)) 'Exchange Rate
                dtrd("AMT") = aryNewEntertain(i)(13) 'Amount
                dtrd("GST_AMT") = aryNewEntertain(i)(14) 'GST Amt
                dtrd("TOTAL_AMT") = aryNewEntertain(i)(15) 'Total Amt
                dtrd("EXCEED_AMT") = IIf(aryNewEntertain(i)(16) = "", "", aryNewEntertain(i)(16)) 'Exceed Amt

                dtEntDtls.Rows.Add(dtrd)
                intLine = intLine + 1
            End If
        Next

        If blnRecord = False And ViewState("index") <> "" Then
            blnRecord = objStaffClaim.chkClaimFormData(ViewState("scno"), "Ent")
        End If

        ds.Tables.Add(dtEntDtls)
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
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "EntertainmentClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><font color=""green""><strong>Entertainment Claim Form</strong></font></a></span></td>" & _
        "</tr>" & _
        "<tr>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "HardShipClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Hardship Claim Form</strong></a></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "OverTimeClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Overtime Claim Form</strong></a></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "OutstationClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Outstation Claim Form</strong></a></span></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "OtherClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Other Claim Form</strong></a></span></td>" & _
        "</tr>" & _
        "</table>" & _
        "</td></tr>"
        Session("w_SC_Links") = strTableLink

    End Sub
End Class
