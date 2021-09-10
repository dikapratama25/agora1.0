Imports AgoraLegacy
Imports System.Drawing

Partial Class HolidayCalendarAdd
    Inherits AgoraLegacy.AppBaseClass
    Dim ds As DataSet
    Dim lngRecNO As Long
    Dim dDispatcher As New AgoraLegacy.dispatcher
    'Dim msg As New Functions.Message
    Dim objGlobal As New AppGlobals
    Dim dsHoliday As New DataSet
    Dim aryHoliday As New ArrayList
    Dim objDb As New DBAccess.EAD.DBCom

    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        'SetGridProperty(dgHoliday)
        lblCountry.Text = objDb.GetVal("SELECT code_desc FROM code_mstr WHERE code_category = 'CT' AND code_abbr = '" & Request.QueryString("country") & "'")
        If Request.QueryString("state") = "" Then
            lblState.Text = "All States"
        Else
            lblState.Text = objDb.GetVal("SELECT code_desc FROM code_mstr WHERE code_category = 'S' AND code_value = '" & Request.QueryString("country") & "' AND code_abbr = '" & Request.QueryString("state") & "'")
        End If

        If Request.QueryString("mode") = "modify" Then
            lblTitle.Text = "Modify Holiday Calendar"
            cmdClear.Visible = False
        End If
        If Not IsPostBack Then
            ConstructTable()

        End If


    End Sub
    Private Function ConstructTable()
        Dim strrow As String
        Dim i, c, count, Sno As Integer
        Dim table As String
        Dim item2nd As Boolean = False
        Dim found As Boolean = False
        Dim dsHoliday_temp As New ArrayList
        Dim objCAD As New IPP
        Dim objipp As New IPPMain
        Dim HolidayIndex As String

        buildaryHoliday()

        If Request.QueryString("mode") = "modify" Then
            'get detail from database
            Dim ds As New DataSet
            ' Dim venidx As String
            'buildaryHoliday2()
            Sno = Request.QueryString("lineno")
            HolidayIndex = Request.QueryString("index")
            ' venidx = objipp.getIPPCompanyIndex(Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))), ViewState("IPPOfficer"))
            ds = objCAD.GetSelectedHoliday(HolidayIndex)
            strrow &= "<tr style=""background-color:#fdfdfd;"">"

            strrow &= "<td align=""left"">"
            strrow &= "<input type=""hidden""  id=""txtSNo" & i & """ name=""txtSNo" & i & """ value=""" & Sno & """>"
            strrow &= "<asp:label style=""width:1%;"" class=""lbl""  id=""lblSNo" & i & """ name=""lblSNo" & i & """ value=""""/>" & Sno & ""
            strrow &= "</td>"

            strrow &= "<td align=""left"">"
            strrow &= "<span class=""date""><input style=""width:85%;margin-right:0px;"" class=""txtbox""  readonly = ""true"" id=""txtDate" & i & """ name=""txtDate" & i & """ value=""" & ds.Tables(0).Rows(0).Item("hm_date") & """></span>&nbsp;"
            strrow &= "<a href=""#"" id=""btnDate" & i & """ onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDate" & i & "") & "','cal','width=180,height=155,left=270,top=180')""><IMG style=""CURSOR: hand"" height=""16"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & " width=""16""></a>"
            strrow &= "</td>"


            strrow &= "<td >"
            strrow &= "<input style=""width:100%;margin-right:0px; "" class=""txtbox"" type=""text""  id=""txtDesc" & i & """ name=""txtDesc" & i & """ value=""" & ds.Tables(0).Rows(0).Item("hm_desc") & """>"
            strrow &= "</td>"

            strrow &= "</td>"
            strrow &= "</tr>"


            table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:100%;"" id=""tblIPPDocItem"">" &
           "<tr class=""TableHeader"">" &
           "<td style=""width:1%;margin-right:0px;"" align=""left"">No</td>" &
           "<td style=""width:20%;margin-left:0px;"">Date<span id=""lblDate"" class=""errormsg"">*</span></td>" &
           "<td style=""width:65%;margin-left:0px;"">Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
           strrow &
           "</table>"


        Else
            'for new

            count = 10
            Sno = CInt(Request.QueryString("rowcount"))
            For i = 0 To count - 1
                Sno = Sno + 1
                strrow &= "<tr style=""background-color:#fdfdfd;"">"

                strrow &= "<td align=""left"">"
                strrow &= "<input type=""hidden""  id=""txtSNo" & i & """ name=""txtSNo" & i & """ value=""" & Sno & """>"
                strrow &= "<asp:label style=""width:1%;"" class=""lbl""  id=""lblSNo" & i & """ name=""lblSNo" & i & """ value=""""/>" & Sno & ""
                strrow &= "</td>"

                strrow &= "<td align=""left"">"
                strrow &= "<span class=""date""><input style=""width:85%;margin-right:0px;"" class=""txtbox""  readonly = ""true"" name=""txtDate" & i & """ value=""" & aryHoliday(i)(1) & """></span>&nbsp;"
                strrow &= "<a href=""#"" id=""btnDate" & i & """ onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDate" & i & "") & "','cal','width=180,height=155,left=270,top=180')""><IMG style=""CURSOR: hand"" height=""16"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & " width=""16""></a>"
                strrow &= "</td>"

                strrow &= "<td >"
                strrow &= "<input style=""width:100%;margin-left:0px; "" class=""txtbox"" type=""text""  id=""txtCC" & i & """ name=""txtDesc" & i & """ value=""" & aryHoliday(i)(2) & """>"
                strrow &= "</td>"

                strrow &= "</td>"
                strrow &= "</tr>"

            Next

            table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:100%;"" id=""tblIPPDocItem"">" &
           "<tr class=""TableHeader"">" &
           "<td style=""width:1%;margin-right:0px;"" align=""left"">No</td>" &
           "<td style=""width:20%;margin-left:0px;"">Date<span id=""lblDate"" class=""errormsg"">*</span></td>" &
           "<td style=""width:65%;margin-left:0px;"">Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
           strrow &
           "</table>"
        End If

        Session("ConstructTable") = table

    End Function
    Sub buildaryHoliday()
        Dim i As Integer
        For i = 0 To 9
            aryHoliday.Add(New String() {Request.Form("txtSNo" & i), Request.Form("txtDate" & i), Request.Form("txtDesc" & i)})
        Next
    End Sub


    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim objipp As New IPP


        If validateField() Then

            If Request.QueryString("mode") = "modify" Then
                objipp.UpdateHoliday(aryHoliday, Request.QueryString("index"), Request.QueryString("year"), Request.QueryString("country"), Request.QueryString("state"))
            Else
                objipp.SaveHoliday(aryHoliday, Request.QueryString("year"), Request.QueryString("country"), Request.QueryString("state"))
            End If

            Response.Write("<script type=""text/javascript"">window.close();opener.refreshgrid();</script>")
        End If

        'opener.refreshgrid();
    End Sub

    Private Function validateField() As Boolean
        Dim count, i As Integer
        Dim ds As New DataSet

        ConstructTable()

        For i = 0 To aryHoliday.Count - 1

            'If aryHoliday.Item(i)(1) <> "" Or aryHoliday.Item(i)(2) <> "" Then
            '    If aryHoliday.Item(i)(1) = "" Then
            '        vldsum.InnerHtml = "<li>Holiday Date " & objGlobal.GetErrorMessage("00001") & "</li>"
            '        Return False

            '    ElseIf aryHoliday.Item(i)(2) = "" Then
            '        vldsum.InnerHtml = "<li>Description " & objGlobal.GetErrorMessage("00001") & "</li>"
            '        Return False
            '    End If
            'Else
            '    Return True
            'End If          

            'If aryHoliday.Item(i)(1) = "" Then
            '    vldsum.InnerHtml = "<li>Holiday Date " & objGlobal.GetErrorMessage("00001") & "</li>"
            '    Return False

            'Else
            '    Return True
            'End If
            If aryHoliday.Item(0)(1) = "" And aryHoliday.Item(0)(2) = "" Then
                vldsum.InnerHtml = "<li>Please enter holiday start from line 1.</li>"
                Return False
            End If
            'If aryHoliday.Item(i)(1) <> "" And aryHoliday.Item(i)(2) <> "" Then
            If aryHoliday.Item(i)(1) = "" And aryHoliday.Item(i)(2) <> "" Then
                vldsum.InnerHtml = "<li>Holiday Date " & objGlobal.GetErrorMessage("00001") & "</li>"
                Return False
            ElseIf aryHoliday.Item(i)(2) = "" And aryHoliday.Item(i)(1) <> "" Then
                vldsum.InnerHtml = "<li>Description " & objGlobal.GetErrorMessage("00001") & "</li>"
                Return False
                'Else
                '    Return True
            End If
        Next
        Return True
    End Function

End Class