Imports AgoraLegacy
Imports eProcure.Component

Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Drawing

Public Class AllowanceClaim
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
    Dim blnEmpty As Boolean = True, blnEmpty2 As Boolean = True, blnEmpty3 As Boolean = True, blnEmpty4 As Boolean = True
    Dim blnCheck As Boolean = True, blnCheck2 As Boolean = True, blnCheck3 As Boolean = True, blnCheck4 As Boolean = True
    Dim aryNewAllowance As New ArrayList()
    Dim strErrEmpty, strErrEmpty2, strErrEmpty3, strErrEmpty4 As String
    Dim strErrCheck, strErrCheck2, strErrCheck3, strErrCheck4 As String

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
            Session("aryNewAllowance") = Nothing
            lblScNo.Text = "To Be Allocated By System"
            ViewState("index") = Request.QueryString("index")
            ViewState("mode") = Request.QueryString("mode")
            ViewState("Row") = 1
            'DisplayUserDetails()
            BuildRow()
            ConstructTableAllowance()
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
        lblUserName.Text = objUserDetails.Name & " (" & Session("UserId") & ")" 'User Name
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
        ddlSelect.SelectedValue = "Allowance"

    End Sub

    Private Sub BuildRow()
        Dim i, intCount As Integer
        Dim dsAllowance As DataSet
        Dim objUser As New Users
        Dim objUserDetails As New User

        BindDdl()
        aryNewAllowance.Clear()

        dsAllowance = objStaffClaim.GetAllowanceDetails(ViewState("index"))

        If dsAllowance.Tables("MSTR").Rows.Count > 0 Then
            lblDocDate.Text = Format(dsAllowance.Tables("MSTR").Rows(0)("SCM_CREATED_DATE"), "dd/MM/yyyy")
            ViewState("scno") = Common.parseNull(dsAllowance.Tables("MSTR").Rows(0)("SCM_CLAIM_DOC_NO"))
            lblScNo.Text = ViewState("scno")
            lblStatus.Text = Common.parseNull(dsAllowance.Tables("MSTR").Rows(0)("STATUS_DESC"))

            objUserDetails = objUser.GetUserDetails(dsAllowance.Tables("MSTR").Rows(0)("SCM_STAFF_ID"), Session("CompanyId"))
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
                If dsAllowance.Tables("MSTR").Rows(0)("SCM_STATUS") <> SCStatus.DraftSC And dsAllowance.Tables("MSTR").Rows(0)("SCM_STATUS") <> SCStatus.Rejected Then
                    cmdAddClaim.Visible = False
                    cmdDupLine.Visible = False
                    cmdSave.Visible = False
                    cmdSaveSummary.Visible = False
                    cmdClear.Visible = False
                    ViewState("SCmode") = "v"
                    trInfo.Style("display") = "none"
                End If
            End If
            If dsAllowance.Tables("DETAILS").Rows.Count > ViewState("Row") Then
                intCount = dsAllowance.Tables("DETAILS").Rows.Count
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
            'Checkbox, Date From, Date To, Project Code, Purpose, Standby Allow Rate, Shift Allow Rate
            aryNewAllowance.Add(New String() {"N", "", "", "", "", "", ""})
        Next

        'Put all data in array if record found
        If dsAllowance.Tables("DETAILS").Rows.Count > 0 Then
            For i = 0 To dsAllowance.Tables("DETAILS").Rows.Count - 1
                aryNewAllowance(i)(1) = Common.parseNull(dsAllowance.Tables("DETAILS").Rows(i)("SCAD_DATE_FROM")) 'Date From
                aryNewAllowance(i)(2) = Common.parseNull(dsAllowance.Tables("DETAILS").Rows(i)("SCAD_DATE_TO")) 'Date To 
                aryNewAllowance(i)(3) = Common.parseNull(dsAllowance.Tables("DETAILS").Rows(i)("SCAD_PROJ_CODE")) 'Project Code 
                aryNewAllowance(i)(4) = Common.parseNull(dsAllowance.Tables("DETAILS").Rows(i)("SCAD_PURPOSE")) 'Purpose
                aryNewAllowance(i)(5) = Common.parseNull(dsAllowance.Tables("DETAILS").Rows(i)("SCAD_STANDBY_ALLOW_RATE")) 'Standby Allow Rate
                aryNewAllowance(i)(6) = Common.parseNull(dsAllowance.Tables("DETAILS").Rows(i)("SCAD_SHIFT_ALLOW_RATE")) 'Shift Allow Rate
            Next
        End If

    End Sub

    Private Sub ConstructTableAllowance()
        Dim table As String
        Dim strrow As String = ""
        Dim i, c, count As Integer
        Dim blnChk As Boolean = True
        Dim aryStandbyRate As New ArrayList()
        Dim aryShiftRate As New ArrayList()
        aryStandbyRate.Add(New String() {"PRIMARY (MYR200/Week)", "200"})
        aryStandbyRate.Add(New String() {"SECONDARY (MYR150/Week)", "150"})
        aryShiftRate.Add(New String() {"RM 5", "5"})
        aryShiftRate.Add(New String() {"RM 10", "10"})
        aryShiftRate.Add(New String() {"RM 20", "20"})
        aryShiftRate.Add(New String() {"RM 50", "50"})

        count = aryNewAllowance.Count
        hidItemLine.Value = count

        For i = 0 To count - 1
            strrow &= "<tr>"

            strrow &= "<td align=""center"" class=""tablecol"">"
            If aryNewAllowance(i)(0) = "Y" Then
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
            strrow &= "<input style=""width:80px;margin-right:0px; "" contentEditable=""false""  class=""txtbox"" type=""text"" id=""txtDateFr" & i & """ name=""txtDateFr" & i & """ value=""" & aryNewAllowance(i)(1) & """>"
            strrow &= "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateFr" & i) & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            strrow &= "</td>"

            'Date To
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:80px;margin-right:0px; "" contentEditable=""false"" class=""txtbox"" type=""text"" id=""txtDateTo" & i & """ name=""txtDateTo" & i & """ value=""" & aryNewAllowance(i)(2) & """>"
            strrow &= "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateTo" & i) & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            strrow &= "</td>"

            'Project Code
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:160px;margin-right:0px; "" maxlength=""50"" class=""txtbox"" type=""text"" id=""txtPC" & i & """ name=""txtPC" & i & """ value=""" & aryNewAllowance(i)(3) & """>"
            strrow &= "</td>"

            'Purpose
            strrow &= "<td class=""tablecol"">"
            strrow &= "<textarea rows=""2"" style=""font-family:Arial,Verdana;font-size:11px;width:200px;margin-right:0px; "" maxlength=""500"" id=""txtPurpose" & i & """ name=""txtPurpose" & i & """>" & aryNewAllowance(i)(4) & "</textarea>"
            strrow &= "</td>"

            'Standby Allowance
            strrow &= "<td class=""tablecol"">"
            strrow &= "<select class=""ddl"" style=""width:100%;margin-right:0px;"" id=""cboStandby" & i & """ name=""cboStandby" & i & """>"
            strrow &= "<option value="""" selected=""selected"">" & "--Select--" & "</option>"
            For c = 0 To aryStandbyRate.Count - 1
                If aryNewAllowance(i)(5) = aryStandbyRate(c)(1) Then
                    strrow &= "<option title=""" & aryStandbyRate(c)(0).ToString & """ value=""" & aryStandbyRate(c)(1).ToString & """ selected=""selected"">" & aryStandbyRate(c)(0).ToString & "</option>"
                Else
                    strrow &= "<option title=""" & aryStandbyRate(c)(0).ToString & """ value=""" & aryStandbyRate(c)(1).ToString & """>" & aryStandbyRate(c)(0).ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "</td>"

            'Shift Allowance
            strrow &= "<td class=""tablecol"">"
            strrow &= "<select class=""ddl"" style=""width:100%;margin-right:0px;"" id=""cboShift" & i & """ name=""cboShift" & i & """>"
            strrow &= "<option value="""" selected=""selected"">" & "--Select--" & "</option>"
            For c = 0 To aryShiftRate.Count - 1
                If aryNewAllowance(i)(6) = aryShiftRate(c)(1) Then
                    strrow &= "<option title=""" & aryShiftRate(c)(0).ToString & """ value=""" & aryShiftRate(c)(1).ToString & """ selected=""selected"">" & aryShiftRate(c)(0).ToString & "</option>"
                Else
                    strrow &= "<option title=""" & aryShiftRate(c)(0).ToString & """ value=""" & aryShiftRate(c)(1).ToString & """>" & aryShiftRate(c)(0).ToString & "</option>"
                End If
            Next
            strrow &= "</select>"
            strrow &= "</td>"

            strrow &= "</tr>"
        Next

        Dim a As String = "<span class=""errormsg"">*</span>"
        table = "<table cellspacing=""0"" cellpadding=""0"" width=""900px"" border=""0"">" & _
                "<tr class=""TableHeader"">" & _
                "<td margin-right:0px;"" width=""30px"" align=""center""><input type=""checkbox"" id=""chkAll"" " & IIf(blnChk = True, "checked", "") & " name=""chkAll"" onclick=""selectAll();""></td>" & _
                "<td margin-right:0px;"" width=""30px"">Line</td>" & _
                "<td margin-right:0px;"" width=""120px"">Date From" & a & "</td>" & _
                "<td margin-right:0px;"" width=""120px"">Date To" & a & "</td>" & _
                "<td margin-right:0px;"" width=""160px"">Project Code</td>" & _
                "<td margin-right:0px;"" width=""200px"">Purpose" & a & "</td>" & _
                "<td margin-right:0px;"" width=""120px"">Standby Allowance</td>" & _
                "<td margin-right:0px;"" width=""120px"">Shift Allowance</td>" & _
                "</tr>" & _
                strrow & _
                "</table>"

        Session("ConstructTableAllowance") = table
        Session("aryNewAllowance") = aryNewAllowance
    End Sub

    Private Sub cmdAddClaim_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddClaim.Click
        Rebuild(False)
        BuildLine()
        ConstructTableAllowance()
    End Sub

    Private Sub Rebuild(Optional ByVal blnChk As Boolean = True)
        Dim i As Integer
        Dim parsedDate As Date
        blnEmpty = False
        blnEmpty2 = False
        blnEmpty3 = False
        blnEmpty4 = False
        blnCheck = False
        blnCheck2 = False
        blnCheck3 = False
        blnCheck4 = False

        aryNewAllowance = Session("aryNewAllowance")
        For i = 0 To aryNewAllowance.Count - 1
            aryNewAllowance(i)(0) = IIf(Request.Form("chkSelection" & i) = "on", "Y", "N") 'Checkbox
            aryNewAllowance(i)(1) = Request.Form("txtDateFr" & i) 'Date From
            aryNewAllowance(i)(2) = Request.Form("txtDateTo" & i) 'Date To 
            aryNewAllowance(i)(3) = Request.Form("txtPC" & i) 'Project Code 
            aryNewAllowance(i)(4) = Request.Form("txtPurpose" & i) 'Purpose
            aryNewAllowance(i)(5) = Request.Form("cboStandby" & i) 'Standby Allow Rate
            aryNewAllowance(i)(6) = Request.Form("cboShift" & i) 'Shift Allow Rate
        Next

        If blnChk = True Then
            For i = 0 To aryNewAllowance.Count - 1
                'Date From, Date To, (Standby / Shift) is required if either one has value
                If aryNewAllowance(i)(1) <> "" Or aryNewAllowance(i)(2) <> "" Or aryNewAllowance(i)(3) <> "" Or aryNewAllowance(i)(4) <> "" Or aryNewAllowance(i)(5) <> "" Or aryNewAllowance(i)(6) <> "" Then
                    'Error if Date From is empty
                    If aryNewAllowance(i)(1) = "" Then
                        blnEmpty = True
                        strErrEmpty &= IIf(strErrEmpty = "", i + 1, "," & i + 1)
                    Else
                        'Error if Date From is invalid
                        If DateDiff(DateInterval.Day, CDate(aryNewAllowance(i)(1)), Today.Now()) > 110 Then
                            blnCheck = True
                            strErrCheck &= IIf(strErrCheck = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if Date To is empty
                    If aryNewAllowance(i)(2) = "" Then
                        blnEmpty2 = True
                        strErrEmpty2 &= IIf(strErrEmpty2 = "", i + 1, "," & i + 1)
                    Else
                        'Error if Date To greater than today's date
                        If CDate(aryNewAllowance(i)(2)) > Today.Now() Then
                            blnCheck3 = True
                            strErrCheck3 &= IIf(strErrCheck3 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if Date From greater than Date To
                    If (blnEmpty = False And blnEmpty2 = False) Then
                        If CDate(aryNewAllowance(i)(1)) > CDate(aryNewAllowance(i)(2)) Then
                            blnCheck2 = True
                            strErrCheck2 &= IIf(strErrCheck2 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if purpose is empty
                    If aryNewAllowance(i)(4) = "" Then
                        blnEmpty3 = True
                        strErrEmpty3 &= IIf(strErrEmpty3 = "", i + 1, "," & i + 1)
                    Else
                        If Not Common.checkMaxLength(aryNewAllowance(i)(4), 500) Then
                            blnCheck4 = True
                            strErrCheck4 &= IIf(strErrCheck4 = "", i + 1, "," & i + 1)
                        End If
                    End If

                    'Error if both (standby/ shift allowance) are empty
                    If (aryNewAllowance(i)(5) = "" And aryNewAllowance(i)(6) = "") Then
                        blnEmpty4 = True
                        strErrEmpty4 &= IIf(strErrEmpty4 = "", i + 1, "," & i + 1)
                    End If
                End If
            Next
        End If

    End Sub

    Private Sub BuildLine()

        'Checkbox, Date From, Date To, Project Code, Purpose, Standby Allow Rate, Shift Allow Rate
        aryNewAllowance.Add(New String() {"N", "", "", "", "", "", ""})

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
            strMsg &= displayMsg(strErrEmpty2, "Date To " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty3 = True Then
            strMsg &= displayMsg(strErrEmpty3, "Purpose " & objGlo.GetErrorMessage("00001"))
        End If
        If blnEmpty4 = True Then
            strMsg &= displayMsg(strErrEmpty4, "Standby/ Shift Allowance " & objGlo.GetErrorMessage("00001"))
        End If
        If blnCheck = True Then
            strMsg &= displayMsg(strErrCheck, "Date From has exceeded 110-days policy.")
        End If
        If blnCheck2 = True Then
            strMsg &= displayMsg(strErrCheck2, "Date From should be < Date To.")
        End If
        If blnCheck3 = True Then
            strMsg &= displayMsg(strErrCheck3, "Date To " & objGlo.GetErrorMessage("00015") & " today's date.")
        End If
        If blnCheck4 = True Then
            strMsg &= displayMsg(strErrCheck4, "Purpose is over limit.")
        End If

        strFullMsg &= strMsg & "</ul>"

        If strMsg <> "" Then
            ConstructTableAllowance()
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

        For i = 0 To aryNewAllowance.Count - 1
            If aryNewAllowance(i)(0) = "Y" Then
                'Checkbox, Date From, Date To, Project Code, Purpose, Standby Allow Rate, Shift Allow Rate
                aryDup.Add(New String() {"N", aryNewAllowance(i)(1), aryNewAllowance(i)(2), aryNewAllowance(i)(3)})
            End If
        Next

        If aryDup.Count > 0 Then
            For i = 0 To aryDup.Count - 1
                'Checkbox, Date From, Date To, Project Code, Purpose, Standby Allow Rate, Shift Allow Rate
                aryNewAllowance.Add(New String() {"N", aryDup(i)(1), aryDup(i)(2), aryDup(i)(3), "", "", ""})
            Next
        End If

        ConstructTableAllowance()
    End Sub

    Private Sub Save(ByVal strMode As String)
        Dim strNewIndex As String = "", strNewSCNo As String = ""
        Dim ds As New DataSet
        Dim blnRecord As Boolean
        BindAL(ds, blnRecord)

        If blnRecord = True Then
            If ViewState("index") <> "" Then
                If objStaffClaim.UpdateAllowance(ds, ViewState("scno"), ViewState("index")) Then
                    If strMode = "Summary" Then
                        Common.NetMsgbox(Me, "Staff Claim Number " & ViewState("scno") & " has been updated.", dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                    Else
                        Common.NetMsgbox(Me, "Staff Claim Number " & ViewState("scno") & " has been updated.", dDispatcher.direct("StaffClaim", "EntertainmentClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                    End If
                Else
                    Common.NetMsgbox(Me, objGlo.GetErrorMessage("00007"), MsgBoxStyle.Information)
                End If
            Else
                If objStaffClaim.chkDocExistMonth = True Then
                    Common.NetMsgbox(Me, "You have already created Staff Claim document for this month.", MsgBoxStyle.Information)
                Else
                    If objStaffClaim.SaveAllowance(ds, strNewSCNo, strNewIndex) Then
                        ViewState("index") = strNewIndex
                        ViewState("scno") = strNewSCNo
                        If strMode = "Summary" Then
                            Common.NetMsgbox(Me, "Staff Claim Number " & strNewSCNo & " has been created.", dDispatcher.direct("StaffClaim", "ClaimSummaryDetail.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                        Else
                            Common.NetMsgbox(Me, "Staff Claim Number " & strNewSCNo & " has been created.", dDispatcher.direct("StaffClaim", "EntertainmentClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId))
                        End If
                    Else
                        Common.NetMsgbox(Me, objGlo.GetErrorMessage("00007"), MsgBoxStyle.Information)
                    End If
                End If
            End If
        Else
            Common.NetMsgbox(Me, objGlo.GetErrorMessage("00131"), MsgBoxStyle.Information)
        End If
        
        ConstructTableAllowance()

    End Sub

    Private Sub BindAL(ByRef ds As DataSet, ByRef blnRecord As Boolean)
        Dim dtALMstr As New DataTable
        Dim dtr As DataRow

        blnRecord = False 'By Default
        dtALMstr.Columns.Add("DEPT_CODE", Type.GetType("System.String"))
        dtr = dtALMstr.NewRow
        dtr("DEPT_CODE") = lblDeptId.Text

        dtALMstr.Rows.Add(dtr)
        ds.Tables.Add(dtALMstr)

        Dim i As Integer, intLine As Integer = 1
        Dim dtALDtls As New DataTable
        Dim dtrd As DataRow

        dtALDtls.Columns.Add("LINE_NO", Type.GetType("System.String"))
        dtALDtls.Columns.Add("DATE_FROM", Type.GetType("System.String"))
        dtALDtls.Columns.Add("DATE_TO", Type.GetType("System.String"))
        dtALDtls.Columns.Add("PROJ_BASED", Type.GetType("System.String"))
        dtALDtls.Columns.Add("PROJ_CODE", Type.GetType("System.String"))
        dtALDtls.Columns.Add("PURPOSE", Type.GetType("System.String"))
        dtALDtls.Columns.Add("STANDBY_ALLOW_RATE", Type.GetType("System.String"))
        dtALDtls.Columns.Add("SHIFT_ALLOW_RATE", Type.GetType("System.String"))

        For i = 0 To aryNewAllowance.Count - 1
            If aryNewAllowance(i)(1) <> "" Then
                blnRecord = True
                dtrd = dtALDtls.NewRow

                dtrd("LINE_NO") = intLine
                dtrd("DATE_FROM") = aryNewAllowance(i)(1) 'Date From
                dtrd("DATE_TO") = aryNewAllowance(i)(2) 'Date To 
                If aryNewAllowance(i)(3) <> "" Then
                    dtrd("PROJ_CODE") = aryNewAllowance(i)(3) 'Project Code
                    If aryNewAllowance(i)(3).ToString.Length > 5 Then
                        dtrd("PROJ_BASED") = "Y" 'Project Based
                    Else
                        dtrd("PROJ_BASED") = "N"
                    End If
                Else
                    dtrd("PROJ_BASED") = "N" 'Project Based
                    dtrd("PROJ_CODE") = "" 'Project Code
                End If
                dtrd("PURPOSE") = IIf(aryNewAllowance(i)(4) = "", "", aryNewAllowance(i)(4)) 'Purpose
                dtrd("STANDBY_ALLOW_RATE") = IIf(aryNewAllowance(i)(5) = "", "", aryNewAllowance(i)(5)) 'Standby Allowance
                dtrd("SHIFT_ALLOW_RATE") = IIf(aryNewAllowance(i)(6) = "", "", aryNewAllowance(i)(6)) 'Shift Allowance
                'dtrd("GST_INPUT_TAX_CODE") = aryNewAllowance(i)(5) 'Tax Code

                dtALDtls.Rows.Add(dtrd)
                intLine = intLine + 1
            End If
        Next

        If blnRecord = False And ViewState("index") <> "" Then
            blnRecord = objStaffClaim.chkClaimFormData(ViewState("scno"), "Allowance")
        End If

        ds.Tables.Add(dtALDtls)
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
        "<td width=""25%"">" & strIcon & "<span style=""vertical-align:middle; position: relative; display:inline;""><a href=""" & dDispatcher.direct("StaffClaim", "AllowanceClaim.aspx", "index=" & ViewState("index") & "&pageid=" & strPageId) & """><font color=""green""><strong>Standby/Shift Allw. Claim Form</strong></font></a></span></td>" & _
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
