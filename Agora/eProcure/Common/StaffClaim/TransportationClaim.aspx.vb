Imports AgoraLegacy
Imports eProcure.Component

Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Drawing

Public Class TransportationClaim
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
    Dim blnEmpty As Boolean = True, blnEmpty2 As Boolean = True, blnEmpty3 As Boolean = True, blnEmpty4 As Boolean = True, blnEmpty5 As Boolean = True, _
    blnEmpty6 As Boolean = True, blnEmpty7 As Boolean = True, blnEmpty8 As Boolean = True, blnEmpty9 As Boolean = True, blnEmpty10 As Boolean = True
    Dim blnCheck As Boolean = True, blnCheck2 As Boolean = True, blnCheck3 As Boolean = True, blnCheck4 As Boolean = True, blnCheck5 As Boolean = True, _
    blnCheck6 As Boolean = True, blnCheck7 As Boolean = True, blnCheck8 As Boolean = True, blnCheck9 As Boolean = True, blnCheck10 As Boolean = True, blnCheck11 As Boolean = True, blnCheck12 As Boolean = True, blnCheck13 As Boolean = True
    Dim aryNewTrans As New ArrayList()
    Dim strErrEmpty, strErrEmpty2, strErrEmpty3, strErrEmpty4, strErrEmpty5, strErrEmpty6, strErrEmpty7, strErrEmpty8, strErrEmpty9, strErrEmpty10 As String
    Dim strErrCheck, strErrCheck2, strErrCheck3, strErrCheck4, strErrCheck5, strErrCheck6, strErrCheck7, strErrCheck8, strErrCheck9, strErrCheck10, strErrCheck11, strErrCheck12, strErrCheck13 As String

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
            Session("aryNewTrans") = Nothing
            lblScNo.Text = "To Be Allocated By System"
            ViewState("index") = Request.QueryString("index")
            If ViewState("index") = "" Then
                Session("urlreferer") = Nothing
            End If
            ViewState("mode") = Request.QueryString("mode")
            ViewState("Row") = 1
            'DisplayUserDetails()
            BuildRow()
            ConstructTableTrans()
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
        ddlSelect.SelectedValue = "Transportation"
    End Sub

    Private Sub BuildRow()
        Dim i, intCount As Integer
        Dim dsTrans As DataSet
        Dim objUser As New Users
        Dim objUserDetails As New User

        BindDdl()
        aryNewTrans.Clear()

        dsTrans = objStaffClaim.GetTransportDetails(ViewState("index"))

        If dsTrans.Tables(0).Rows.Count > 0 Then
            lblDocDate.Text = Format(dsTrans.Tables("MSTR").Rows(0)("SCM_CREATED_DATE"), "dd/MM/yyyy")
            ViewState("scno") = Common.parseNull(dsTrans.Tables("MSTR").Rows(0)("SCM_CLAIM_DOC_NO"))
            lblScNo.Text = ViewState("scno")
            lblStatus.Text = Common.parseNull(dsTrans.Tables("MSTR").Rows(0)("STATUS_DESC"))

            objUserDetails = objUser.GetUserDetails(dsTrans.Tables("MSTR").Rows(0)("SCM_STAFF_ID"), Session("CompanyId"))
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
                If dsTrans.Tables("MSTR").Rows(0)("SCM_STATUS") <> SCStatus.DraftSC And dsTrans.Tables("MSTR").Rows(0)("SCM_STATUS") <> SCStatus.Rejected Then
                    cmdAddClaim.Visible = False
                    cmdDupLine.Visible = False
                    cmdSave.Visible = False
                    cmdSaveSummary.Visible = False
                    cmdClear.Visible = False
                    ViewState("SCmode") = "v"
                    trInfo.Style("display") = "none"
                End If
            End If
            
            If dsTrans.Tables("DETAILS").Rows.Count > ViewState("Row") Then
                intCount = dsTrans.Tables("DETAILS").Rows.Count
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
            'Checkbox, Date, Loc From, Loc To, Project Code, CSR No, Purpose, Tax Inv Type, Currency, Exchange Rate, 
            'PT, Parking, Toll, Airfair, Car Mileage, Car Amt, Bike Mileage, Bike Amt, SOG-FE, Smart Pay, Fuel Price, Total Amt
            aryNewTrans.Add(New String() {"N", "", "", "", "", "", "", "N", "MYR", "", "", "", "", "", "", "", "", "", "", "", "", ""})
        Next

        'Put all data in array if record found
        If dsTrans.Tables("DETAILS").Rows.Count > 0 Then
            For i = 0 To dsTrans.Tables("DETAILS").Rows.Count - 1
                aryNewTrans(i)(1) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_DATE")) 'Date 
                aryNewTrans(i)(2) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_LOC_FROM")) 'Location From
                aryNewTrans(i)(3) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_LOC_TO")) 'Location To
                aryNewTrans(i)(4) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_PROJ_CODE")) 'Project Code
                aryNewTrans(i)(5) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_CSR_NO")) 'CSR No
                aryNewTrans(i)(6) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_PURPOSE")) 'Purpose
                aryNewTrans(i)(7) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_TAX_INV_TYPE")) 'Tax Inv Type
                aryNewTrans(i)(8) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_CURRENCY_CODE")) 'Currency
                aryNewTrans(i)(9) = IIf(aryNewTrans(i)(8) = "MYR", "", Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_EXCHANGE_RATE"))) 'Exchange Rate
                aryNewTrans(i)(10) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_PT_AMT")) 'PT
                aryNewTrans(i)(11) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_PARKING_AMT")) 'Parking
                aryNewTrans(i)(12) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_TOLL_AMT")) 'Toll
                aryNewTrans(i)(13) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_AIRFARE_AMT")) 'Airfair
                aryNewTrans(i)(14) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_CAR_MILEAGE")) 'Car Mileage
                aryNewTrans(i)(15) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_CAR_AMT")) 'Car Amt
                aryNewTrans(i)(16) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_BIKE_MILEAGE")) 'Bike Mileage
                aryNewTrans(i)(17) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_BIKE_AMT")) 'Bike Amt
                aryNewTrans(i)(18) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_SOG_FE_AMT")) 'SOG-FE
                aryNewTrans(i)(19) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_SMART_PAY_AMT")) 'Smart Pay
                aryNewTrans(i)(20) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_FUEL_AMT")) 'Fuel Price
                aryNewTrans(i)(21) = Common.parseNull(dsTrans.Tables("DETAILS").Rows(i)("SCTD_TOTAL_AMT")) 'Total Amt
            Next
        End If

    End Sub

    Private Sub ConstructTableTrans()
        Dim table As String
        Dim strrow As String = ""
        Dim i, c, count As Integer
        Dim dsCurr As New DataSet
        Dim blnChk As Boolean = True
        Dim aryTaxInvType As New ArrayList()

        aryTaxInvType.Add(New String() {"Full", "F"})
        aryTaxInvType.Add(New String() {"Simplified", "S"})
        aryTaxInvType.Add(New String() {"Non-Tax Invoice", "N"})

        dsCurr = objStaffClaim.getDdlInfo("1")
        count = aryNewTrans.Count
        hidItemLine.Value = count

        For i = 0 To count - 1
            strrow &= "<tr>"

            strrow &= "<td align=""center"" class=""tablecol"">"
            If aryNewTrans(i)(0) = "Y" Then
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
            strrow &= "<input style=""width:80px;margin-right:0px; "" contentEditable=""false"" class=""txtbox"" type=""text"" id=""txtDate" & i & """ name=""txtDate" & i & """ value=""" & aryNewTrans(i)(1) & """>"
            strrow &= "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDate" & i) & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            strrow &= "</td>"

            'Location From
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:120px;margin-right:0px; "" maxlength=""50"" class=""txtbox"" type=""text"" id=""txtLocFr" & i & """ name=""txtLocFr" & i & """ value=""" & aryNewTrans(i)(2) & """>"
            strrow &= "</td>"

            'Location To
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:120px;margin-right:0px; "" maxlength=""50"" class=""txtbox"" type=""text"" id=""txtLocTo" & i & """ name=""txtLocTo" & i & """ value=""" & aryNewTrans(i)(3) & """>"
            strrow &= "</td>"

            'Project Code
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:160px;margin-right:0px; "" maxlength=""50"" class=""txtbox"" type=""text"" id=""txtPC" & i & """ name=""txtPC" & i & """ value=""" & aryNewTrans(i)(4) & """>"
            strrow &= "</td>"

            'CSR No
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:100px;margin-right:0px; "" maxlength=""50"" class=""txtbox"" type=""text"" id=""txtCSR" & i & """ name=""txtCSR" & i & """ value=""" & aryNewTrans(i)(5) & """>"
            strrow &= "</td>"

            'Purpose
            strrow &= "<td class=""tablecol"">"
            strrow &= "<textarea rows=""2"" style=""font-family:Arial,Verdana;font-size:11px;width:200px;margin-right:0px; "" maxlength=""500"" id=""txtPurpose" & i & """ name=""txtPurpose" & i & """>" & aryNewTrans(i)(6) & "</textarea>"
            strrow &= "</td>"

            'Tax Inv Type
            strrow &= "<td class=""tablecol"">"
            strrow &= "<select class=""ddl"" style=""width:100%;margin-right:0px;"" id=""cboTaxInvType" & i & """ name=""cboTaxInvType" & i & """>"
            'strrow &= "<option value="""" selected=""selected"">" & "--Select--" & "</option>"
            For c = 0 To aryTaxInvType.Count - 1
                If aryNewTrans(i)(7) = aryTaxInvType(c)(1) Then
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
                If aryNewTrans(i)(8) = dsCurr.Tables(0).Rows(c)(0) Then
                    strrow &= "<option title=""" & dsCurr.Tables(0).Rows(c)(0).ToString & """ value=""" & dsCurr.Tables(0).Rows(c)(0).ToString & """ selected=""selected"">" & dsCurr.Tables(0).Rows(c)(0).ToString & "</option>"
                Else
                    strrow &= "<option title=""" & dsCurr.Tables(0).Rows(c)(0).ToString & """ value=""" & dsCurr.Tables(0).Rows(c)(0).ToString & """>" & dsCurr.Tables(0).Rows(c)(0).ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "</td>"

            'Exchange Rate
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:100%;margin-right:0px;"" " & IIf(aryNewTrans(i)(8) <> "MYR", "", "disabled") & " onkeypress=""return isDecimalKey(event)"" onkeyup=""calTotalAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtExRate" & i & """ name=""txtExRate" & i & """ value=""" & aryNewTrans(i)(9) & """>"
            strrow &= "</td>"

            'Public Transport Amt
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:100%;margin-right:0px;"" onkeypress=""return isDecimalKey(event)"" onkeyup=""calTotalAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtPT" & i & """ name=""txtPT" & i & """ value=""" & aryNewTrans(i)(10) & """>"
            strrow &= "</td>"

            'Parking Amt
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:100%;margin-right:0px;"" onkeypress=""return isDecimalKey(event)"" onkeyup=""calTotalAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtParking" & i & """ name=""txtParking" & i & """ value=""" & aryNewTrans(i)(11) & """>"
            strrow &= "</td>"

            'Toll Amt
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:100%;margin-right:0px;"" onkeypress=""return isDecimalKey(event)"" onkeyup=""calTotalAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtToll" & i & """ name=""txtToll" & i & """ value=""" & aryNewTrans(i)(12) & """>"
            strrow &= "</td>"

            'AirFare
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:100%;margin-right:0px;"" onkeypress=""return isDecimalKey(event)"" onkeyup=""calTotalAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtAF" & i & """ name=""txtAF" & i & """ value=""" & aryNewTrans(i)(13) & """>"
            strrow &= "</td>"

            'Car Mileage
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:100%;margin-right:0px;"" onkeypress=""return isDecimalKey(event)"" onkeyup=""CalCarAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtCM" & i & """ name=""txtCM" & i & """ value=""" & aryNewTrans(i)(14) & """>"
            strrow &= "</td>"

            'Car Amt
            strrow &= "<td class=""tablecol"">"
            strrow &= "<label id=""lblCA" & i & """ style=""width:100%;margin-right:0px; text-align:right;"">" & aryNewTrans(i)(15) & "</label>"
            strrow &= "</td>"

            'Bike Mileage
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:100%;margin-right:0px;"" onkeypress=""return isDecimalKey(event)"" onkeyup=""CalBikeAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtBM" & i & """ name=""txtBM" & i & """ value=""" & aryNewTrans(i)(16) & """>"
            strrow &= "</td>"

            'Bike Amt
            strrow &= "<td class=""tablecol"">"
            strrow &= "<label id=""lblBA" & i & """ style=""width:100%;margin-right:0px; text-align:right;"">" & aryNewTrans(i)(17) & "</label>"
            strrow &= "</td>"

            'SOG FE
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:80px;margin-right:0px;"" onkeypress=""return isDecimalKey(event)"" onkeyup=""calTotalAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtSOG" & i & """ name=""txtSOG" & i & """ value=""" & aryNewTrans(i)(18) & """>"
            strrow &= "</td>"

            'Smart Pay
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:80px;margin-right:0px;"" onkeypress=""return isDecimalKey(event)"" onkeyup=""calTotalAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtSP" & i & """ name=""txtSP" & i & """ value=""" & aryNewTrans(i)(19) & """>"
            strrow &= "</td>"

            'Fuel Price
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:80px;margin-right:0px;"" onkeypress=""return isDecimalKey(event)"" onkeyup=""calTotalAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtFP" & i & """ name=""txtFP" & i & """ value=""" & aryNewTrans(i)(20) & """>"
            strrow &= "</td>"

            'Total Amt
            strrow &= "<td class=""tablecol"">"
            strrow &= "<label id=""lblTotalAmt" & i & """ style=""width:100%;margin-right:0px; text-align:right;"">" & aryNewTrans(i)(21) & "</label>"
            strrow &= "</td>"

            strrow &= "</tr>"
        Next

        Dim a As String = "<span class=""errormsg"">*</span>"
        table = "<table cellspacing=""0"" cellpadding=""0"" width=""2275"" border=""0"">" & _
                           "<tr class=""TableHeader"">" & _
                           "<td margin-right:0px;"" width=""30px"" align=""center""><input type=""checkbox"" id=""chkAll"" " & IIf(blnChk = True, "checked", "") & " name=""chkAll"" onclick=""selectAll();""></td>" & _
                           "<td margin-right:0px;"" width=""30px"">No</td>" & _
                           "<td margin-right:0px;"" width=""120px"">Date" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""120px"">Location From" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""120px"">Location To" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""160px"">Project Code</td>" & _
                           "<td margin-right:0px;"" width=""100px"">CSR/ CN No</td>" & _
                           "<td margin-right:0px;"" width=""200px"">Purpose of Trip" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""100px"">Tax Invoice Type" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""80px"">Currency" & a & "</td>" & _
                           "<td margin-right:0px;"" width=""75px"" align=""right"">Exchange Rate</td>" & _
                           "<td margin-right:0px;"" width=""100px"" align=""right"">Public Transportation</td>" & _
                           "<td margin-right:0px;"" width=""100px"" align=""right"">Parking</td>" & _
                           "<td margin-right:0px;"" width=""100px"" align=""right"">Toll</td>" & _
                           "<td margin-right:0px;"" width=""100px"" align=""right"">Airfare</td>" & _
                           "<td margin-right:0px;"" width=""100px"" align=""right"">Car Mileage (km)</td>" & _
                           "<td margin-right:0px;"" width=""100px"" align=""right"">Car (MYR0.60/km)</td>" & _
                           "<td margin-right:0px;"" width=""100px"" align=""right"">Bike Mileage (km)</td>" & _
                           "<td margin-right:0px;"" width=""100px"" align=""right"">Bike (MYR0.30/km)</td>" & _
                           "<td margin-right:0px;"" width=""80px"" align=""right"">SOG-FE (km)</td>" & _
                           "<td margin-right:0px;"" width=""80px"" align=""right"">Smart Pay</td>" & _
                           "<td margin-right:0px;"" width=""80px"" align=""right"">Fuel Price</td>" & _
                           "<td margin-right:0px;"" width=""100px"" align=""right"">Total Amt (MYR)</td>" & _
                           "</tr>" & _
                           strrow & _
                           "</table>"

        Session("ConstructTableTrans") = table
        Session("aryNewTrans") = aryNewTrans
    End Sub

    Private Function CalTotalAmt(ByVal strCurrency As String, ByVal strExRate As String, ByVal strPT As String, ByVal strParking As String, ByVal strToll As String, _
    ByVal strAF As String, ByVal strCarAmt As String, ByVal strBikeAmt As String) As String
        Dim decTotalAmt As Decimal = 0, decExRate As Decimal = 0
        CalTotalAmt = ""
        If strCurrency = "MYR" Then
            decExRate = 1
        Else
            If strExRate <> "" And chkDec(strExRate) Then
                decExRate = CDec(strExRate)
            End If
        End If

        If (strPT <> "" Or strParking <> "" Or strToll <> "" Or strAF <> "" Or strCarAmt <> "" Or strBikeAmt <> "") Then
            If chkDec(strPT) And strPT <> "" Then decTotalAmt += (CDec(strPT) * decExRate)
            If chkDec(strParking) And strParking <> "" Then decTotalAmt += (CDec(strParking) * decExRate)
            If chkDec(strToll) And strToll <> "" Then decTotalAmt += (CDec(strToll) * decExRate)
            If chkDec(strAF) And strAF <> "" Then decTotalAmt += (CDec(strAF) * decExRate)
            If chkDec(strCarAmt) And strCarAmt <> "" Then decTotalAmt += CDec(strCarAmt)
            If chkDec(strBikeAmt) And strBikeAmt <> "" Then decTotalAmt += CDec(strBikeAmt)
            CalTotalAmt = Format(decTotalAmt, "###0.00")
        End If

    End Function

    Private Function CalVehicleAmt(ByVal strRate As String, ByVal strMileage As String) As String
        Dim decRate, decVehicleAmt As Decimal
        Dim dblMileage As Double

        CalVehicleAmt = ""
        If strMileage <> "" Then
            decVehicleAmt = 0
            If chkKm(strMileage) Then
                If strRate = "C" Then
                    decRate = 0.6
                Else
                    decRate = 0.3
                End If
                dblMileage = CDbl(strMileage)
                decVehicleAmt = decRate * dblMileage
            End If
            CalVehicleAmt = Format(decVehicleAmt, "###0.00")
        End If
    End Function

    Private Sub cmdAddClaim_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddClaim.Click
        Rebuild(False)
        BuildLine()
        ConstructTableTrans()
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
        blnCheck12 = False
        blnCheck13 = false

        aryNewTrans = Session("aryNewTrans")
        For i = 0 To aryNewTrans.Count - 1
            aryNewTrans(i)(0) = IIf(Request.Form("chkSelection" & i) = "on", "Y", "N") 'Checkbox
            aryNewTrans(i)(1) = Request.Form("txtDate" & i) 'Date 
            aryNewTrans(i)(2) = Request.Form("txtLocFr" & i) 'Location From
            aryNewTrans(i)(3) = Request.Form("txtLocTo" & i) 'Location To 
            aryNewTrans(i)(4) = Request.Form("txtPC" & i) 'Project Code
            aryNewTrans(i)(5) = Request.Form("txtCSR" & i) 'CSR No
            aryNewTrans(i)(6) = Request.Form("txtPurpose" & i) 'Purpose
            aryNewTrans(i)(7) = Request.Form("cboTaxInvType" & i) 'Tax Inv Type
            aryNewTrans(i)(8) = Request.Form("cboCurr" & i) 'Currency
            aryNewTrans(i)(9) = Request.Form("txtExRate" & i) 'Exchange Rate
            aryNewTrans(i)(10) = Request.Form("txtPT" & i) 'PT 
            aryNewTrans(i)(11) = Request.Form("txtParking" & i) 'Parking
            aryNewTrans(i)(12) = Request.Form("txtToll" & i) 'Toll
            aryNewTrans(i)(13) = Request.Form("txtAF" & i) 'Airfair
            aryNewTrans(i)(14) = Request.Form("txtCM" & i) 'Car Mileage
            aryNewTrans(i)(15) = CalVehicleAmt("C", aryNewTrans(i)(14)) 'Car Amt
            aryNewTrans(i)(16) = Request.Form("txtBM" & i) 'Bike Mileage
            aryNewTrans(i)(17) = CalVehicleAmt("B", aryNewTrans(i)(16)) 'Bike Amt
            aryNewTrans(i)(18) = Request.Form("txtSOG" & i) 'SOG-FE
            aryNewTrans(i)(19) = Request.Form("txtSP" & i) 'Smart Pay
            aryNewTrans(i)(20) = Request.Form("txtFP" & i) 'Fuel Price
            aryNewTrans(i)(21) = CalTotalAmt(aryNewTrans(i)(8), aryNewTrans(i)(9), aryNewTrans(i)(10), aryNewTrans(i)(11), _
                                aryNewTrans(i)(12), aryNewTrans(i)(13), aryNewTrans(i)(15), aryNewTrans(i)(17)) 'Total Amt
        Next

        If blnChk = True Then
            For i = 0 To aryNewTrans.Count - 1
                If aryNewTrans(i)(1) <> "" Or aryNewTrans(i)(2) <> "" Or aryNewTrans(i)(3) <> "" Or aryNewTrans(i)(4) <> "" Or _
                aryNewTrans(i)(5) <> "" Or aryNewTrans(i)(6) <> "" Or aryNewTrans(i)(9) <> "" Or _
                aryNewTrans(i)(10) <> "" Or aryNewTrans(i)(11) <> "" Or aryNewTrans(i)(12) <> "" Or aryNewTrans(i)(13) <> "" Or aryNewTrans(i)(14) <> "" Or _
                aryNewTrans(i)(16) <> "" Or aryNewTrans(i)(18) <> "" Or aryNewTrans(i)(19) <> "" Or aryNewTrans(i)(20) <> "" Then
                    'Error if date is empty
                    If aryNewTrans(i)(1) = "" Then
                        blnEmpty = True
                        strErrEmpty &= IIf(strErrEmpty = "", i + 1, "," & i + 1)
                        'strErrEmpty = i + 1
                    Else
                        'Error if Date is invalid
                        If DateDiff(DateInterval.Day, CDate(aryNewTrans(i)(1)), Today.Now()) > 110 Then
                            blnCheck = True
                            strErrCheck &= IIf(strErrCheck = "", i + 1, "," & i + 1)
                            'strErrCheck = i + 1
                        End If

                        'Error if Date greater than today's date
                        If CDate(aryNewTrans(i)(1)) > Today.Now() Then
                            blnCheck2 = True
                            strErrCheck2 &= IIf(strErrCheck2 = "", i + 1, "," & i + 1)
                            'strErrCheck2 = i + 1
                        End If
                    End If

                    'Error if location from is empty
                    If aryNewTrans(i)(2) = "" Then
                        blnEmpty2 = True
                        strErrEmpty2 &= IIf(strErrEmpty2 = "", i + 1, "," & i + 1)
                        'strErrEmpty2 = i + 1
                    End If

                    'Error if location to is empty
                    If aryNewTrans(i)(3) = "" Then
                        blnEmpty3 = True
                        strErrEmpty3 &= IIf(strErrEmpty3 = "", i + 1, "," & i + 1)
                        'strErrEmpty3 = i + 1
                    End If

                    'Error if purpose is empty
                    If aryNewTrans(i)(6) = "" Then
                        blnEmpty4 = True
                        strErrEmpty4 &= IIf(strErrEmpty4 = "", i + 1, "," & i + 1)
                    Else
                        If Not Common.checkMaxLength(aryNewTrans(i)(6), 500) Then
                            blnCheck12 = True
                            strErrCheck12 &= IIf(strErrCheck12 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if tax inv type is empty
                    If aryNewTrans(i)(7) = "" Then
                        blnEmpty5 = True
                        strErrEmpty5 &= IIf(strErrEmpty5 = "", i + 1, "," & i + 1)
                        'strErrEmpty5 = i + 1
                    End If

                    'Error if currency is empty
                    If aryNewTrans(i)(8) = "" Then
                        blnEmpty6 = True
                        strErrEmpty6 &= IIf(strErrEmpty6 = "", i + 1, "," & i + 1)
                        'strErrEmpty6 = i + 1
                    Else
                        'Error if Exchange Rate is empty and Currency is not 'MYR'
                        If aryNewTrans(i)(8) <> "MYR" And aryNewTrans(i)(9) = "" Then
                            blnEmpty7 = True
                            strErrEmpty7 &= IIf(strErrEmpty7 = "", i + 1, "," & i + 1)
                            'strErrEmpty7 = i + 1
                        End If
                    End If

                    'Error if Exchange Rate is invalid
                    If aryNewTrans(i)(9) <> "" Then
                        If chkDec(aryNewTrans(i)(9)) Then
                        Else
                            blnCheck3 = True
                            strErrCheck3 &= IIf(strErrCheck3 = "", i + 1, "," & i + 1)
                            'strErrCheck3 = i + 1
                        End If
                    End If

                    If aryNewTrans(i)(10) = "" And aryNewTrans(i)(11) = "" And aryNewTrans(i)(12) = "" And aryNewTrans(i)(13) = "" And aryNewTrans(i)(14) = "" _
                        And aryNewTrans(i)(16) = "" And aryNewTrans(i)(18) = "" And aryNewTrans(i)(19) = "" Then
                        blnEmpty8 = True
                        strErrEmpty8 &= IIf(strErrEmpty8 = "", i + 1, "," & i + 1)
                        'strErrEmpty8 = i + 1
                    Else
                        'Error if PT is invalid
                        If aryNewTrans(i)(10) <> "" Then
                            If chkDec(aryNewTrans(i)(10)) Then
                            Else
                                blnCheck4 = True
                                strErrCheck4 &= IIf(strErrCheck4 = "", i + 1, "," & i + 1)
                                'strErrCheck4 = i + 1
                            End If
                        End If

                        'Error if Parking is invalid
                        If aryNewTrans(i)(11) <> "" Then
                            If chkDec(aryNewTrans(i)(11)) Then
                            Else
                                blnCheck5 = True
                                strErrCheck5 &= IIf(strErrCheck5 = "", i + 1, "," & i + 1)
                                'strErrCheck5 = i + 1
                            End If
                        End If

                        'Error if Toll is invalid
                        If aryNewTrans(i)(12) <> "" Then
                            If chkDec(aryNewTrans(i)(12)) Then
                            Else
                                blnCheck6 = True
                                strErrCheck6 &= IIf(strErrCheck6 = "", i + 1, "," & i + 1)
                                'strErrCheck6 = i + 1
                            End If
                        End If

                        'Error if AF is invalid
                        If aryNewTrans(i)(13) <> "" Then
                            If chkDec(aryNewTrans(i)(13)) Then
                            Else
                                blnCheck7 = True
                                strErrCheck7 &= IIf(strErrCheck7 = "", i + 1, "," & i + 1)
                                'strErrCheck7 = i + 1
                            End If
                        End If

                        'Error if car mileage is invalid
                        If aryNewTrans(i)(14) <> "" Then
                            If chkKm(aryNewTrans(i)(14)) Then
                            Else
                                blnCheck8 = True
                                strErrCheck8 &= IIf(strErrCheck8 = "", i + 1, "," & i + 1)
                                'strErrCheck8 = i + 1
                            End If
                        End If

                        'Error if bike mileage is invalid
                        If aryNewTrans(i)(16) <> "" Then
                            If chkKm(aryNewTrans(i)(16)) Then
                            Else
                                blnCheck9 = True
                                strErrCheck9 &= IIf(strErrCheck9 = "", i + 1, "," & i + 1)
                                'strErrCheck9 = i + 1
                            End If
                        End If

                        'Error if SOG is invalid
                        If aryNewTrans(i)(18) <> "" Then
                            If chkKm(aryNewTrans(i)(18)) Then
                            Else
                                blnCheck10 = True
                                strErrCheck10 &= IIf(strErrCheck10 = "", i + 1, "," & i + 1)
                            End If
                        End If

                        'Error if Smart Pay is invalid
                        If aryNewTrans(i)(19) <> "" Then
                            If chkDec(aryNewTrans(i)(19)) Then
                            Else
                                blnCheck11 = True
                                strErrCheck11 &= IIf(strErrCheck11 = "", i + 1, "," & i + 1)
                            End If

                            If aryNewTrans(i)(18) = "" Or aryNewTrans(i)(20) = "" Then
                                blnEmpty10 = True
                                strErrEmpty10 &= IIf(strErrEmpty10 = "", i + 1, "," & i + 1)
                            End If
                        End If

                        'Error if Fuel Price is invalid
                        If aryNewTrans(i)(20) <> "" Then
                            If chkDec(aryNewTrans(i)(20)) Then
                            Else
                                blnCheck13 = True
                                strErrCheck13 &= IIf(strErrCheck13 = "", i + 1, "," & i + 1)
                            End If
                        End If

                        'Error if Fuel Price is required
                        If aryNewTrans(i)(18) <> "" And aryNewTrans(i)(20) = "" Then
                            blnEmpty9 = True
                            strErrEmpty9 &= IIf(strErrEmpty9 = "", i + 1, "," & i + 1)
                            'strErrEmpty9 = i + 1
                        End If

                        ''Error if SOG FE is required
                        'If aryNewTrans(i)(18) = "" And aryNewTrans(i)(20) <> "" Then
                        '    blnEmpty10 = True
                        '    strErrEmpty10 &= IIf(strErrEmpty10 = "", i + 1, "," & i + 1)
                        '    'strErrEmpty10 = i + 1
                        'End If
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

    Private Function chkDec(ByVal strValue As String) As Boolean
        If IsNumeric(strValue) And Regex.IsMatch(Trim(strValue), "(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,2})?$") Then
            chkDec = True
        Else
            chkDec = False
        End If
    End Function

    Private Function chkKm(ByVal strValue As String) As Boolean
        If IsNumeric(strValue) And Regex.IsMatch(Trim(strValue), "(?!^0*$)(?!^0*\.0*$)^\d{1,8}(\.\d{1,1})?$") Then
            chkKm = True
        Else
            chkKm = False
        End If
    End Function

    Private Sub BuildLine()

        'Checkbox, Date, Loc From, Loc To, Project Code, CSR No, Purpose, Tax Inv Type, Currency, Exchange Rate, 
        'PT, Parking, Toll, Airfair, Car Mileage, Car Amt, Bike Mileage, Bike Amt, SOG-FE, Smart Pay, Fuel Price, Total Amt
        aryNewTrans.Add(New String() {"N", "", "", "", "", "", "", "N", "MYR", "", "", "", "", "", "", "", "", "", "", "", "", ""})

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
            strMsg &= displayMsg(strErrEmpty2, "Location From " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty3 = True Then
            strMsg &= displayMsg(strErrEmpty3, "Location To " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty4 = True Then
            strMsg &= displayMsg(strErrEmpty4, "Purpose of Trip " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty5 = True Then
            strMsg &= displayMsg(strErrEmpty5, "Tax Invoice Type " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty6 = True Then
            strMsg &= displayMsg(strErrEmpty6, "Currency " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty7 = True Then
            strMsg &= displayMsg(strErrEmpty7, "Exchange Rate " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty8 = True Then
            strMsg &= displayMsg(strErrEmpty8, "Please enter at least one claim.")
        End If
        If blnEmpty9 = True Then
            strMsg &= displayMsg(strErrEmpty9, "Fuel Price " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty10 = True Then
            strMsg &= displayMsg(strErrEmpty10, "SOG FE/Fuel Price " & objGlo.GetErrorMessage("00106"))
        End If
        If blnCheck = True Then
            strMsg &= displayMsg(strErrCheck, "Date has exceeded 110-days policy.")
        End If
        If blnCheck2 = True Then
            strMsg &= displayMsg(strErrCheck2, "Date " & objGlo.GetErrorMessage("00015") & " today's date.")
        End If
        If blnCheck3 = True Then
            strMsg &= displayMsg(strErrCheck3, "Exchange Rate " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck4 = True Then
            strMsg &= displayMsg(strErrCheck4, "Public Transportation " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck5 = True Then
            strMsg &= displayMsg(strErrCheck5, "Parking " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck6 = True Then
            strMsg &= displayMsg(strErrCheck6, "Toll " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck7 = True Then
            strMsg &= displayMsg(strErrCheck7, "Airfare " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck8 = True Then
            strMsg &= displayMsg(strErrCheck8, "Car Mileage " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck9 = True Then
            strMsg &= displayMsg(strErrCheck9, "Bike Mileage " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck10 = True Then
            strMsg &= displayMsg(strErrCheck10, "SOG FE " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck11 = True Then
            strMsg &= displayMsg(strErrCheck11, "Smart Pay " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck12 = True Then
            strMsg &= displayMsg(strErrCheck12, "Purpose of Trip is over limit.")
        End If

        strFullMsg &= strMsg & "</ul>"

        If strMsg <> "" Then
            ConstructTableTrans()
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

        For i = 0 To aryNewTrans.Count - 1
            If aryNewTrans(i)(0) = "Y" Then
                'Checkbox, Date, Loc From, Loc To, Fuel Price
                aryDup.Add(New String() {aryNewTrans(i)(1), aryNewTrans(i)(2), aryNewTrans(i)(3), aryNewTrans(i)(20)})
            End If
        Next

        If aryDup.Count > 0 Then
            For i = 0 To aryDup.Count - 1
                'Checkbox, Date, Loc From, Loc To, Project Code, CSR No, Purpose, Tax Inv Type, Currency, Exchange Rate, 
                'PT, Parking, Toll, Airfair, Car Mileage, Car Amt, Bike Mileage, Bike Amt, SOG-FE, Smart Pay, Fuel Price, Total Amt
                aryNewTrans.Add(New String() {"N", aryDup(i)(0), aryDup(i)(1), aryDup(i)(2), "", "", "", "N", "MYR", "", "", "", "", "", "", "", "", "", "", "", aryDup(i)(3), ""})
            Next
        End If

        ConstructTableTrans()
    End Sub

    Private Sub Save(ByVal strMode As String)
        Dim strNewIndex, strNewSCNo As String
        Dim ds As New DataSet
        Dim blnRecord As Boolean
        BindTrans(ds, blnRecord)

        If blnRecord = True Then
            If ViewState("index") <> "" Then
                If objStaffClaim.UpdateTransportation(ds, ViewState("scno"), ViewState("index")) Then
                    If strMode = "Summary" Then
                        Common.NetMsgbox(Me, "Staff Claim Number " & ViewState("scno") & " has been updated.", dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                    Else
                        Common.NetMsgbox(Me, "Staff Claim Number " & ViewState("scno") & " has been updated.", dDispatcher.direct("StaffClaim", "AllowanceClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                    End If
                Else
                    Common.NetMsgbox(Me, objGlo.GetErrorMessage("00007"), MsgBoxStyle.Information)
                End If
            Else
                If objStaffClaim.chkDocExistMonth = True Then
                    Common.NetMsgbox(Me, "You have already created Staff Claim document for this month.", MsgBoxStyle.Information)
                Else
                    If objStaffClaim.SaveTransportation(ds, strNewSCNo, strNewIndex) Then
                        ViewState("index") = strNewIndex
                        ViewState("scno") = strNewSCNo
                        If strMode = "Summary" Then
                            Common.NetMsgbox(Me, "Staff Claim Number " & strNewSCNo & " has been created.", dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                        Else
                            Common.NetMsgbox(Me, "Staff Claim Number " & strNewSCNo & " has been created.", dDispatcher.direct("StaffClaim", "AllowanceClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                        End If
                    Else
                        Common.NetMsgbox(Me, objGlo.GetErrorMessage("00007"), MsgBoxStyle.Information)
                    End If
                End If
            End If
        Else
            Common.NetMsgbox(Me, objGlo.GetErrorMessage("00131"), MsgBoxStyle.Information)
        End If

            ConstructTableTrans()

    End Sub

    Private Sub BindTrans(ByRef ds As DataSet, ByRef blnRecord As Boolean)
        Dim dtTranMstr As New DataTable
        Dim dtr As DataRow

        blnRecord = False 'By Default
        dtTranMstr.Columns.Add("DEPT_CODE", Type.GetType("System.String"))
        dtr = dtTranMstr.NewRow
        dtr("DEPT_CODE") = lblDeptId.Text

        dtTranMstr.Rows.Add(dtr)
        ds.Tables.Add(dtTranMstr)

        Dim i As Integer, intLine As Integer = 1
        Dim dtTranDtls As New DataTable
        Dim dtrd As DataRow

        dtTranDtls.Columns.Add("LINE_NO", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("DATE", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("LOC_FROM", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("LOC_TO", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("PROJ_CODE", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("PROJ_BASED", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("CSR_NO", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("PURPOSE", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("TAX_INV_TYPE", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("CURRENCY_CODE", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("EXCHANGE_RATE", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("PT_AMT", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("PARKING_AMT", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("TOLL_AMT", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("AIRFARE_AMT", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("CAR_MILEAGE", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("CAR_AMT", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("BIKE_MILEAGE", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("BIKE_AMT", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("SOG_FE_AMT", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("SMART_PAY_AMT", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("FUEL_AMT", Type.GetType("System.String"))
        dtTranDtls.Columns.Add("TOTAL_AMT", Type.GetType("System.String"))

        For i = 0 To aryNewTrans.Count - 1
            If aryNewTrans(i)(1) <> "" Then
                blnRecord = True
                dtrd = dtTranDtls.NewRow

                dtrd("LINE_NO") = intLine
                dtrd("DATE") = aryNewTrans(i)(1) 'Date 
                dtrd("LOC_FROM") = aryNewTrans(i)(2) 'Location From
                dtrd("LOC_TO") = aryNewTrans(i)(3) 'Location To 
                If aryNewTrans(i)(4) <> "" Then
                    dtrd("PROJ_CODE") = aryNewTrans(i)(4) 'Project Code
                    If aryNewTrans(i)(4).ToString.Length > 5 Then
                        dtrd("PROJ_BASED") = "Y" 'Project Based
                    Else
                        dtrd("PROJ_BASED") = "N"
                    End If
                Else
                    dtrd("PROJ_BASED") = "N" 'Project Based
                    dtrd("PROJ_CODE") = "" 'Project Code
                End If
                dtrd("CSR_NO") = IIf(aryNewTrans(i)(5) = "", "", aryNewTrans(i)(5)) 'CSR No
                dtrd("PURPOSE") = IIf(aryNewTrans(i)(6) = "", "", aryNewTrans(i)(6)) 'Purpose
                dtrd("TAX_INV_TYPE") = IIf(aryNewTrans(i)(7) = "", "", aryNewTrans(i)(7)) 'Tax Inv Type
                dtrd("CURRENCY_CODE") = aryNewTrans(i)(8) 'Currency
                dtrd("EXCHANGE_RATE") = IIf(aryNewTrans(i)(8) = "MYR", "1.00", aryNewTrans(i)(9)) 'Exchange Rate
                dtrd("PT_AMT") = IIf(aryNewTrans(i)(10) = "", "NULL", aryNewTrans(i)(10)) 'PT 
                dtrd("PARKING_AMT") = IIf(aryNewTrans(i)(11) = "", "NULL", aryNewTrans(i)(11)) 'Parking
                dtrd("TOLL_AMT") = IIf(aryNewTrans(i)(12) = "", "NULL", aryNewTrans(i)(12)) 'Toll
                dtrd("AIRFARE_AMT") = IIf(aryNewTrans(i)(13) = "", "NULL", aryNewTrans(i)(13)) 'Airfare
                dtrd("CAR_MILEAGE") = IIf(aryNewTrans(i)(14) = "", "NULL", aryNewTrans(i)(14)) 'Car Mileage
                dtrd("CAR_AMT") = IIf(aryNewTrans(i)(15) = "", "NULL", aryNewTrans(i)(15)) 'Car Amt
                dtrd("BIKE_MILEAGE") = IIf(aryNewTrans(i)(16) = "", "NULL", aryNewTrans(i)(16)) 'Bike Mileage
                dtrd("BIKE_AMT") = IIf(aryNewTrans(i)(17) = "", "NULL", aryNewTrans(i)(17)) 'Bike Amt
                dtrd("SOG_FE_AMT") = IIf(aryNewTrans(i)(18) = "", "NULL", aryNewTrans(i)(18)) 'SOG-FE
                dtrd("SMART_PAY_AMT") = IIf(aryNewTrans(i)(19) = "", "NULL", aryNewTrans(i)(19)) 'Smart Pay
                If aryNewTrans(i)(18) = "" Then
                    dtrd("FUEL_AMT") = "NULL" 'Fuel Price
                Else
                    dtrd("FUEL_AMT") = IIf(aryNewTrans(i)(20) = "", "NULL", aryNewTrans(i)(20)) 'Fuel Price
                End If
                dtrd("TOTAL_AMT") = IIf(aryNewTrans(i)(21) = "", "NULL", aryNewTrans(i)(21))

                dtTranDtls.Rows.Add(dtrd)
                intLine = intLine + 1
            End If
        Next

        If blnRecord = False And ViewState("index") <> "" Then
            blnRecord = objStaffClaim.chkClaimFormData(ViewState("scno"), "Transportation")
        End If

        ds.Tables.Add(dtTranDtls)
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

        strTableLink &= "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "TransportationClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><font color=""green""><strong>Transportation Claim Form</strong></font></a></span></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "AllowanceClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Standby/Shift Allw. Claim Form</strong></a></span></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "EntertainmentClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Entertainment Claim Form</strong></a></span></td>" & _
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
