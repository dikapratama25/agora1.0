Imports AgoraLegacy
Imports eProcure.Component

Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Drawing

Public Class OtherClaim
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
    Protected WithEvents lblSummary As System.Web.UI.WebControls.Label
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
    blnEmpty9 As Boolean = True
    Dim blnCheck As Boolean = True, blnCheck2 As Boolean = True, blnCheck3 As Boolean = True, blnCheck4 As Boolean = True, blnCheck5 As Boolean = True, blnCheck6 As Boolean = True, blnCheck7 As Boolean = True
    Dim aryNewOther As New ArrayList()
    Dim strErrEmpty, strErrEmpty2, strErrEmpty3, strErrEmpty4, strErrEmpty5, strErrEmpty6, strErrEmpty7, strErrEmpty8, strErrEmpty9 As String
    Dim strErrCheck, strErrCheck2, strErrCheck3, strErrCheck4, strErrCheck5, strErrCheck6, strErrCheck7 As String

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
            Session("aryNewOther") = Nothing
            lblScNo.Text = "To Be Allocated By System"
            ViewState("index") = Request.QueryString("index")
            ViewState("mode") = Request.QueryString("mode")
            ViewState("Row") = 1
            'DisplayUserDetails()
            BuildRow()
            ConstructTableOther()
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
        ddlSelect.SelectedValue = "Other"
    End Sub

    Private Sub BuildRow()
        Dim i, intCount As Integer
        Dim dsOther As DataSet
        Dim objUser As New Users
        Dim objUserDetails As New User

        BindDdl()
        aryNewOther.Clear()

        dsOther = objStaffClaim.GetMiscDetails(ViewState("index"))

        If dsOther.Tables("MSTR").Rows.Count > 0 Then
            lblDocDate.Text = Format(dsOther.Tables("MSTR").Rows(0)("SCM_CREATED_DATE"), "dd/MM/yyyy")
            ViewState("scno") = Common.parseNull(dsOther.Tables("MSTR").Rows(0)("SCM_CLAIM_DOC_NO"))
            lblScNo.Text = ViewState("scno")
            lblStatus.Text = Common.parseNull(dsOther.Tables("MSTR").Rows(0)("STATUS_DESC"))

            objUserDetails = objUser.GetUserDetails(dsOther.Tables("MSTR").Rows(0)("SCM_STAFF_ID"), Session("CompanyId"))
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
                If dsOther.Tables("MSTR").Rows(0)("SCM_STATUS") <> SCStatus.DraftSC And dsOther.Tables("MSTR").Rows(0)("SCM_STATUS") <> SCStatus.Rejected Then
                    cmdAddClaim.Visible = False
                    cmdDupLine.Visible = False
                    cmdSave.Visible = False
                    cmdSaveSummary.Visible = False
                    cmdClear.Visible = False
                    ViewState("SCmode") = "v"
                    trInfo.Style("display") = "none"
                End If
            End If
            If dsOther.Tables("DETAILS").Rows.Count > ViewState("Row") Then
                intCount = dsOther.Tables("DETAILS").Rows.Count
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
            'Checkbox, Date, Project Code, Type, Tax Invoice Type, Currency, Exchange Rate, Amount, GST Amt, Remark, Total Amt
            aryNewOther.Add(New String() {"N", "", "", "", "N", "MYR", "", "", "0.00", "", ""})
            'aryNewOther.Add(New String() {"N", "", "", "", "MYR", "", "", "", "", "", "", "", "", ""})
        Next

        'Put all data in array if record found
        If dsOther.Tables("DETAILS").Rows.Count > 0 Then
            For i = 0 To dsOther.Tables("DETAILS").Rows.Count - 1
                aryNewOther(i)(1) = Common.parseNull(dsOther.Tables("DETAILS").Rows(i)("SCMD_DATE")) 'Date
                aryNewOther(i)(2) = Common.parseNull(dsOther.Tables("DETAILS").Rows(i)("SCMD_PROJ_CODE")) 'Project Code
                aryNewOther(i)(3) = Common.parseNull(dsOther.Tables("DETAILS").Rows(i)("SCMD_TYPE")) 'Type
                aryNewOther(i)(4) = Common.parseNull(dsOther.Tables("DETAILS").Rows(i)("SCMD_TAX_INV_TYPE")) 'Tax Invoice Type
                aryNewOther(i)(5) = Common.parseNull(dsOther.Tables("DETAILS").Rows(i)("SCMD_CURRENCY_CODE")) 'Currency
                If Common.parseNull(dsOther.Tables("DETAILS").Rows(i)("SCMD_CURRENCY_CODE")) <> "MYR" Then
                    aryNewOther(i)(6) = Common.parseNull(dsOther.Tables("DETAILS").Rows(i)("SCMD_EXCHANGE_RATE")) 'Exchange Rate
                End If
                aryNewOther(i)(7) = Common.parseNull(dsOther.Tables("DETAILS").Rows(i)("SCMD_AMT")) 'Amount
                aryNewOther(i)(8) = Common.parseNull(dsOther.Tables("DETAILS").Rows(i)("SCMD_GST_AMT")) 'GST Amt
                aryNewOther(i)(9) = Common.parseNull(dsOther.Tables("DETAILS").Rows(i)("SCMD_REMARK")) 'Remark
                aryNewOther(i)(10) = Common.parseNull(dsOther.Tables("DETAILS").Rows(i)("SCMD_TOTAL_AMT")) 'Total Amt
            Next
        End If

    End Sub

    Private Sub ConstructTableOther()
        Dim table As String
        Dim strrow As String = ""
        Dim i, c, count As Integer
        Dim blnChk As Boolean = True
        Dim aryType, aryTaxInvType As New ArrayList()
        Dim dsCurr As New DataSet
        Dim decGrandTotalAmount As Decimal = 0

        aryType.Add("Medical Claims (MYR40)")
        aryType.Add("Dental Claims (MYR50)")
        aryType.Add("Laundry")
        aryType.Add("Stationery")
        aryType.Add("Postage")
        aryType.Add("Company H/Phone Off. Calls")
        aryType.Add("Personal H/Phone Off. Calls")
        aryType.Add("Handphone Subsidy")
        aryType.Add("Data Plan")
        aryType.Add("Transportation Claim (ONG)")
        aryType.Add("Storage Allowance")
        aryType.Add("Gifts")
        aryType.Add("Attendence Performance Reward")
        aryType.Add("Others")
        aryType.Add("Travelling Advance Taken")
        
        aryTaxInvType.Add(New String() {"Full", "F"})
        aryTaxInvType.Add(New String() {"Simplified", "S"})
        aryTaxInvType.Add(New String() {"Non-Tax Invoice", "N"})

        dsCurr = objStaffClaim.getDdlInfo("1")
        'dsTaxCode = objStaffClaim.getDdlInfo("2")

        count = aryNewOther.Count
        hidItemLine.Value = count

        For i = 0 To count - 1
            strrow &= "<tr>"

            strrow &= "<td align=""center"" class=""tablecol"">"
            If aryNewOther(i)(0) = "Y" Then
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
            strrow &= "<input style=""width:80px;margin-right:0px; "" contentEditable=""false"" class=""txtbox"" type=""text"" id=""txtDate" & i & """ name=""txtDate" & i & """ value=""" & aryNewOther(i)(1) & """>"
            strrow &= "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDate" & i) & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            strrow &= "</td>"

            'Project Code
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:160px;margin-right:0px; "" maxlength=""50"" class=""txtbox"" type=""text"" id=""txtPC" & i & """ name=""txtPC" & i & """ value=""" & aryNewOther(i)(2) & """>"
            strrow &= "</td>"

            'Type
            strrow &= "<td class=""tablecol"">"
            strrow &= "<select class=""ddl"" style=""width:100%;margin-right:0px;"" id=""cboType" & i & """ name=""cboType" & i & """>"
            strrow &= "<option value="""" selected=""selected"">" & "--Select--" & "</option>"
            For c = 0 To aryType.Count - 1
                If aryNewOther(i)(3) = aryType(c) Then
                    strrow &= "<option title=""" & aryType(c).ToString & """ value=""" & aryType(c).ToString & """ selected=""selected"">" & aryType(c).ToString & "</option>"
                Else
                    strrow &= "<option title=""" & aryType(c).ToString & """ value=""" & aryType(c).ToString & """>" & aryType(c).ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "</td>"

            'Tax Inv Type
            strrow &= "<td class=""tablecol"">"
            strrow &= "<select class=""ddl"" style=""width:100%;margin-right:0px;"" id=""cboTaxInvType" & i & """ onchange=""enableGstAmt(" & i & ")"" name=""cboTaxInvType" & i & """>"
            'strrow &= "<option value="""" selected=""selected"">" & "--Select--" & "</option>"
            For c = 0 To aryTaxInvType.Count - 1
                If aryNewOther(i)(4) = aryTaxInvType(c)(1) Then
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
                If aryNewOther(i)(5) = dsCurr.Tables(0).Rows(c)(0) Then
                    strrow &= "<option title=""" & dsCurr.Tables(0).Rows(c)(0).ToString & """ value=""" & dsCurr.Tables(0).Rows(c)(0).ToString & """ selected=""selected"">" & dsCurr.Tables(0).Rows(c)(0).ToString & "</option>"
                Else
                    strrow &= "<option title=""" & dsCurr.Tables(0).Rows(c)(0).ToString & """ value=""" & dsCurr.Tables(0).Rows(c)(0).ToString & """>" & dsCurr.Tables(0).Rows(c)(0).ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "</td>"

            'Exchange Rate
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:90px;margin-right:0px;"" " & IIf(aryNewOther(i)(5) <> "MYR", "", "disabled") & " onkeypress=""return isDecimalKey(event)"" onkeyup=""calTotalAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtExRate" & i & """ name=""txtExRate" & i & """ value=""" & aryNewOther(i)(6) & """>"
            strrow &= "</td>"

            'Amount
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:75px;margin-right:0px;"" onkeypress=""return isDecimalKey(event)"" onkeyup=""calTotalAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtAmt" & i & """ name=""txtAmt" & i & """ value=""" & aryNewOther(i)(7) & """>"
            strrow &= "</td>"

            'GST Amount
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:75px;margin-right:0px;"" " & IIf(aryNewOther(i)(4) <> "N", "", "disabled") & " onkeypress=""return isDecimalKey(event)"" onkeyup=""calTotalAmt(" & i & ")"" maxlength=""10"" class=""numerictxtbox"" type=""text"" id=""txtGSTAmt" & i & """ name=""txtGSTAmt" & i & """ value=""" & aryNewOther(i)(8) & """>"
            strrow &= "</td>"

            'Remarks
            strrow &= "<td class=""tablecol"">"
            strrow &= "<textarea rows=""2"" style=""font-family:Arial,Verdana;font-size:11px;width:150px;margin-right:0px; "" maxlength=""500"" id=""txtRemark" & i & """ name=""txtRemark" & i & """>" & aryNewOther(i)(9) & "</textarea>"
            strrow &= "</td>"

            'Total Amt
            strrow &= "<td class=""tablecol"">"
            strrow &= "<label id=""lblTotalAmt" & i & """ style=""width:100%;margin-right:0px; text-align:right;"">" & aryNewOther(i)(10) & "</label>"
            strrow &= "</td>"

            strrow &= "</tr>"
        Next

        Dim a As String = "<span class=""errormsg"">*</span>"
        table = "<table cellspacing=""0"" cellpadding=""0"" width=""1240px"" border=""0"">" & _
                "<tr class=""TableHeader"">" & _
                "<td margin-right:0px;"" width=""30px"" align=""center""><input type=""checkbox"" id=""chkAll"" " & IIf(blnChk = True, "checked", "") & " name=""chkAll"" onclick=""selectAll();""></td>" & _
                "<td margin-right:0px;"" width=""30px"">Line</td>" & _
                "<td margin-right:0px;"" width=""120px"">Date" & a & "</td>" & _
                "<td margin-right:0px;"" width=""160px"">Project Code</td>" & _
                "<td margin-right:0px;"" width=""200px"">Type" & a & "</td>" & _
                "<td margin-right:0px;"" width=""120px"">Tax Invoice Type" & a & "</td>" & _
                "<td margin-right:0px;"" width=""80px"">Currency" & a & "</td>" & _
                "<td margin-right:0px;"" width=""90px"" align=""right"">Exchange Rate</td>" & _
                "<td margin-right:0px;"" width=""75px"" align=""right"">Amount" & a & "</td>" & _
                "<td margin-right:0px;"" width=""75px"" align=""right"">GST Amt" & a & "</td>" & _
                "<td margin-right:0px;"" width=""150px"">Remarks</td>" & _
                "<td margin-right:0px;"" width=""110px"" align=""right"">Total Amt (MYR)</td>" & _
                "</tr>" & _
                strrow & _
                "</table>"

        Session("ConstructTableOther") = table
        Session("aryNewOther") = aryNewOther

    End Sub

    Private Function CalTotalAmt(ByVal strCurrency As String, ByVal strExRate As String, ByVal strAmt As String, ByVal strGstAmt As String) As String
        Dim decAmt, decGstAmt As Decimal
        Dim decTotalAmt As Decimal = 0
        Dim decExRate As Decimal = 0

        CalTotalAmt = ""
        If strAmt <> "" And strGstAmt <> "" Then
            If strCurrency = "MYR" Then
                decExRate = 1
            Else
                If strExRate <> "" And chkDec(strExRate, False) Then
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
        End If
    End Function

    Private Sub cmdAddClaim_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddClaim.Click
        Rebuild(False)
        BuildLine()
        ConstructTableOther()
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
        blnCheck = False
        blnCheck2 = False
        blnCheck3 = False
        blnCheck4 = False
        blnCheck5 = False
        blnCheck6 = False
        blnCheck7 = False

        aryNewOther = Session("aryNewOther")
        For i = 0 To aryNewOther.Count - 1
            aryNewOther(i)(0) = IIf(Request.Form("chkSelection" & i) = "on", "Y", "N") 'Checkbox
            aryNewOther(i)(1) = Request.Form("txtDate" & i) 'Date
            aryNewOther(i)(2) = Request.Form("txtPC" & i) 'Project Code
            aryNewOther(i)(3) = Request.Form("cboType" & i) 'Type
            aryNewOther(i)(4) = Request.Form("cboTaxInvType" & i) 'Tax Invoice Type
            aryNewOther(i)(5) = Request.Form("cboCurr" & i) 'Currency
            aryNewOther(i)(6) = Request.Form("txtExRate" & i) 'Exchange Rate
            aryNewOther(i)(7) = Request.Form("txtAmt" & i) 'Amount
            If Request.Form("cboTaxInvType" & i) = "N" Then 'GST Amt
                aryNewOther(i)(8) = "0.00"
            Else
                aryNewOther(i)(8) = Request.Form("txtGSTAmt" & i)
            End If
            aryNewOther(i)(9) = Request.Form("txtRemark" & i) 'Remark
            aryNewOther(i)(10) = CalTotalAmt(aryNewOther(i)(5), aryNewOther(i)(6), aryNewOther(i)(7), aryNewOther(i)(8)) 'Total Amt
        Next

        If blnChk = True Then
            For i = 0 To aryNewOther.Count - 1
                If aryNewOther(i)(1) <> "" Or aryNewOther(i)(2) <> "" Or aryNewOther(i)(3) <> "" Or _
                aryNewOther(i)(6) <> "" Or aryNewOther(i)(7) <> "" Or aryNewOther(i)(9) <> "" Then
                    'Error if date is empty
                    If aryNewOther(i)(1) = "" Then
                        blnEmpty = True
                        strErrEmpty &= IIf(strErrEmpty = "", i + 1, "," & i + 1)
                    Else
                        'Error if Date is invalid
                        If DateDiff(DateInterval.Day, CDate(aryNewOther(i)(1)), Today.Now()) > 110 Then
                            blnCheck = True
                            strErrCheck &= IIf(strErrCheck = "", i + 1, "," & i + 1)
                        End If

                        'Error if Date greater than today's date
                        If CDate(aryNewOther(i)(1)) > Today.Now() Then
                            blnCheck5 = True
                            strErrCheck5 &= IIf(strErrCheck5 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if Type is empty
                    If aryNewOther(i)(3) = "" Then
                        blnEmpty2 = True
                        strErrEmpty2 &= IIf(strErrEmpty2 = "", i + 1, "," & i + 1)
                    Else
                        If aryNewOther(i)(2) <> "" Then
                            If aryNewOther(i)(2).ToString.Length > 5 And (aryNewOther(i)(3) <> "Stationery" And aryNewOther(i)(3) <> "Others" And aryNewOther(i)(3) <> "Data Plan" _
                            And aryNewOther(i)(3) <> "Company H/Phone Off. Calls" And aryNewOther(i)(3) <> "Personal H/Phone Off. Calls" And aryNewOther(i)(3) <> "Handphone Subsidy") Then
                                blnCheck7 = True
                                strErrCheck7 &= IIf(strErrCheck7 = "", i + 1, "," & i + 1)
                            End If
                        End If
                    End If

                    'Error if Tax Invoice Type is empty
                    If aryNewOther(i)(4) = "" Then
                        blnEmpty3 = True
                        strErrEmpty3 &= IIf(strErrEmpty3 = "", i + 1, "," & i + 1)
                    End If

                    'Error if Currency is empty
                    If aryNewOther(i)(5) = "" Then
                        blnEmpty4 = True
                        strErrEmpty4 &= IIf(strErrEmpty4 = "", i + 1, "," & i + 1)
                    Else
                        'Error if Exchange Rate is empty and Currency is not 'MYR'
                        If aryNewOther(i)(5) <> "MYR" And aryNewOther(i)(6) = "" Then
                            blnEmpty5 = True
                            strErrEmpty5 &= IIf(strErrEmpty5 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if Amt is empty
                    If aryNewOther(i)(7) = "" Then
                        blnEmpty6 = True
                        strErrEmpty6 &= IIf(strErrEmpty6 = "", i + 1, "," & i + 1)
                    Else
                        'Error if Amt is invalid
                        If chkDec(aryNewOther(i)(7), False) Then
                        Else
                            blnCheck2 = True
                            strErrCheck2 &= IIf(strErrCheck2 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if Gst Amt is empty
                    If aryNewOther(i)(8) = "" Then
                        blnEmpty7 = True
                        strErrEmpty7 &= IIf(strErrEmpty7 = "", i + 1, "," & i + 1)
                    Else
                        'Error if Amt is invalid
                        If chkDec(aryNewOther(i)(8)) Then
                        Else
                            blnCheck3 = True
                            strErrCheck3 &= IIf(strErrCheck3 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if Exchange Rate is invalid
                    If aryNewOther(i)(6) <> "" Then
                        If chkDec(aryNewOther(i)(6), False) Then
                        Else
                            blnCheck4 = True
                            strErrCheck4 &= IIf(strErrCheck4 = "", i + 1, "," & i + 1)
                        End If
                    End If
                    If aryNewOther(i)(9) <> "" Then
                        If Not Common.checkMaxLength(aryNewOther(i)(9), 500) Then
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

        'Checkbox, Date, Project Code, Type, Tax Invoice Type, Currency, Exchange Rate, Amount, GST Amt, Remark, Total Amt
        aryNewOther.Add(New String() {"N", "", "", "", "N", "MYR", "", "", "0.00", "", ""})

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
            strMsg &= displayMsg(strErrEmpty2, "Type " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty3 = True Then
            strMsg &= displayMsg(strErrEmpty3, "Tax Invoice Type " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty4 = True Then
            strMsg &= displayMsg(strErrEmpty4, "Currency " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty5 = True Then
            strMsg &= displayMsg(strErrEmpty5, "Exchange Rate " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty6 = True Then
            strMsg &= displayMsg(strErrEmpty6, "Amount " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty7 = True Then
            strMsg &= displayMsg(strErrEmpty7, "GST Amount " & objGlo.GetErrorMessage("00001"))
        End If
        If blnCheck = True Then
            strMsg &= displayMsg(strErrCheck, "Date has exceeded 110-days policy.")
        End If
        If blnCheck2 = True Then
            strMsg &= displayMsg(strErrCheck2, "Amount " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck3 = True Then
            strMsg &= displayMsg(strErrCheck3, "GST Amount " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck4 = True Then
            strMsg &= displayMsg(strErrCheck4, "Exchange Rate " & objGlo.GetErrorMessage("00283"))
        End If
        If blnCheck5 = True Then
            strMsg &= displayMsg(strErrCheck5, "Date " & objGlo.GetErrorMessage("00015") & " today's date.")
        End If
        If blnCheck6 = True Then
            strMsg &= displayMsg(strErrCheck6, "Remark is over limit.")
        End If
        If blnCheck7 = True Then
            strMsg &= displayMsg(strErrCheck7, "Selected Type is only for non-Project.")
        End If

        strFullMsg &= strMsg & "</ul>"

        If strMsg <> "" Then
            ConstructTableOther()
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

        For i = 0 To aryNewOther.Count - 1
            If aryNewOther(i)(0) = "Y" Then
                'Checkbox, Date, Project Code, Type, Tax Invoice Type, Currency, Exchange Rate, Amount, GST Amt, Remark, Total Amt
                aryDup.Add(New String() {aryNewOther(i)(1), aryNewOther(i)(2)})
            End If
        Next

        If aryDup.Count > 0 Then
            For i = 0 To aryDup.Count - 1
                'Checkbox, Date, Project Code, Type, Tax Invoice Type, Currency, Exchange Rate, Amount, GST Amt, Remark, Total Amt
                aryNewOther.Add(New String() {"N", aryDup(i)(0), aryDup(i)(1), "", "N", "MYR", "", "", "0.00", "", ""})
            Next
        End If

        ConstructTableOther()
    End Sub

    Private Sub Save(ByVal strMode As String)
        Dim strNewIndex, strNewSCNo As String
        Dim ds As New DataSet
        Dim blnRecord As Boolean
        BindMisc(ds, blnRecord)

        If blnRecord = True Then
            If ViewState("index") <> "" Then
                If objStaffClaim.UpdateMisc(ds, ViewState("scno"), ViewState("index")) Then
                    If strMode = "Summary" Then
                        Common.NetMsgbox(Me, "Staff Claim Number " & ViewState("scno") & " has been updated.", dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                    Else
                        Common.NetMsgbox(Me, "Staff Claim Number " & ViewState("scno") & " has been updated.", dDispatcher.direct("StaffClaim", "TransportationClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                    End If
                Else
                    Common.NetMsgbox(Me, objGlo.GetErrorMessage("00007"), MsgBoxStyle.Information)
                End If
            Else
                If objStaffClaim.chkDocExistMonth = True Then
                    Common.NetMsgbox(Me, "You have already created Staff Claim document for this month.", MsgBoxStyle.Information)
                Else
                    If objStaffClaim.SaveMisc(ds, strNewSCNo, strNewIndex) Then
                        ViewState("index") = strNewIndex
                        ViewState("scno") = strNewSCNo
                        If strMode = "Summary" Then
                            Common.NetMsgbox(Me, "Staff Claim Number " & strNewSCNo & " has been created.", dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                        Else
                            Common.NetMsgbox(Me, "Staff Claim Number " & strNewSCNo & " has been created.", dDispatcher.direct("StaffClaim", "TransportationClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                        End If
                    Else
                        Common.NetMsgbox(Me, objGlo.GetErrorMessage("00007"), MsgBoxStyle.Information)
                    End If
                End If
            End If
        Else
            Common.NetMsgbox(Me, objGlo.GetErrorMessage("00131"), MsgBoxStyle.Information)
        End If
        
        ConstructTableOther()

    End Sub

    Private Sub BindMisc(ByRef ds As DataSet, ByRef blnRecord As Boolean)
        Dim dtMiscMstr As New DataTable
        Dim dtr As DataRow

        blnRecord = False 'By Default
        dtMiscMstr.Columns.Add("DEPT_CODE", Type.GetType("System.String"))
        dtr = dtMiscMstr.NewRow
        dtr("DEPT_CODE") = lblDeptId.Text

        dtMiscMstr.Rows.Add(dtr)
        ds.Tables.Add(dtMiscMstr)

        Dim i As Integer, intLine As Integer = 1
        Dim dtMiscDtls As New DataTable
        Dim dtrd As DataRow

        dtMiscDtls.Columns.Add("LINE_NO", Type.GetType("System.String"))
        dtMiscDtls.Columns.Add("DATE", Type.GetType("System.String"))
        dtMiscDtls.Columns.Add("PROJ_CODE", Type.GetType("System.String"))
        dtMiscDtls.Columns.Add("PROJ_BASED", Type.GetType("System.String"))
        dtMiscDtls.Columns.Add("TYPE", Type.GetType("System.String"))
        dtMiscDtls.Columns.Add("TAX_INV_TYPE", Type.GetType("System.String"))
        dtMiscDtls.Columns.Add("CURRENCY_CODE", Type.GetType("System.String"))
        dtMiscDtls.Columns.Add("EXCHANGE_RATE", Type.GetType("System.String"))
        dtMiscDtls.Columns.Add("AMT", Type.GetType("System.String"))
        dtMiscDtls.Columns.Add("GST_AMT", Type.GetType("System.String"))
        dtMiscDtls.Columns.Add("REMARK", Type.GetType("System.String"))
        dtMiscDtls.Columns.Add("TOTAL_AMT", Type.GetType("System.String"))

        For i = 0 To aryNewOther.Count - 1
            If aryNewOther(i)(1) <> "" Then
                blnRecord = True
                dtrd = dtMiscDtls.NewRow

                dtrd("LINE_NO") = intLine
                dtrd("DATE") = aryNewOther(i)(1) 'Date
                If aryNewOther(i)(2) <> "" Then
                    dtrd("PROJ_CODE") = aryNewOther(i)(2) 'Project Code
                    If aryNewOther(i)(2).ToString.Length > 5 Then
                        dtrd("PROJ_BASED") = "Y" 'Project Based
                    Else
                        dtrd("PROJ_BASED") = "N"
                    End If
                Else
                    dtrd("PROJ_BASED") = "N" 'Project Based
                    dtrd("PROJ_CODE") = "" 'Project Code
                End If

                dtrd("TYPE") = aryNewOther(i)(3) 'Type
                dtrd("TAX_INV_TYPE") = aryNewOther(i)(4) 'Tax Invoice Type
                dtrd("CURRENCY_CODE") = aryNewOther(i)(5) 'Currency
                dtrd("EXCHANGE_RATE") = IIf(aryNewOther(i)(5) = "MYR", "1.00", aryNewOther(i)(6)) 'Exchange Rate
                dtrd("AMT") = aryNewOther(i)(7) 'Amount
                dtrd("GST_AMT") = aryNewOther(i)(8) 'GST Amt
                dtrd("REMARK") = IIf(aryNewOther(i)(9) = "", "", aryNewOther(i)(9)) 'Remark
                dtrd("TOTAL_AMT") = aryNewOther(i)(10) 'Total Amt

                dtMiscDtls.Rows.Add(dtrd)
                intLine = intLine + 1
            End If
        Next

        If blnRecord = False And ViewState("index") <> "" Then
            blnRecord = objStaffClaim.chkClaimFormData(ViewState("scno"), "Other")
        End If

        ds.Tables.Add(dtMiscDtls)
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
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "OutstationClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><strong>Outstation Claim Form</strong></a></span></td>" & _
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "OtherClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><font color=""green""><strong>Other Claim Form</strong></font></a></span></td>" & _
        "</tr>" & _
        "</table>" & _
        "</td></tr>"
        Session("w_SC_Links") = strTableLink

    End Sub
End Class
