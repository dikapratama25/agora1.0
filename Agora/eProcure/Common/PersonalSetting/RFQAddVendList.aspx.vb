Imports AgoraLegacy
Imports eProcure.Component
Imports System.Drawing


Public Class RFQAddVendList
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher

    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session("userAction") = "Modify" Then
                '<fali> added to avoid & and # symbol filtering during passing the listname value
                Dim urlName As String = Request.QueryString("name")
                If urlName.IndexOf("SPSPCE") Then
                    urlName = urlName.Replace("SPSPCE", " ")
                End If
                '</fali>
                txtName.Text = urlName
                hidName.Text = urlName
            End If
            Session("ColumnName") = Nothing
            Session("Columnid") = Nothing
            ConstructTable()
            populate()
        End If
        ValidationSummary1.Visible = False
        btnClose.Attributes.Add("onclick", "window.close();")
        'btnSave.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection');")
        btnDelete.Attributes.Add("onclick", "return CheckAtLeastOne('chkSelection'),'delete';")
    End Sub
    Sub populate()
        'Label
        If Session("userAction") = "Add" Then
            lblHeader.Text = "Add Vendor List"
            lbltitle.Text = "Add Vendor List"
        ElseIf Session("userAction") = "Modify" Then
            lblHeader.Text = "Modify Vendor List"
            lbltitle.Text = "Modify Vendor List"
        End If
        'Type Ahead
        Dim typeahead As String
        Dim count As Integer
        count = ViewState("Count")
        Dim i As Integer
        Dim content As String
        Dim nametypeahead As String
        'Dim objUsers As New Users
        'Dim objUserDetails As New User

        'objUserDetails = objUsers.GetUserDetails(Session("UserId"), Session("CompanyId"))
        'If objUsers.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Purchasing_Officer) Or objUsers.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Purchasing_Manager) Then
        '    nametypeahead = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=VENDORLIST")

        'ElseIf objUsers.IsRoleSelected(objUserDetails.UserGroup, FixedRole.Buyer) Then
        '    nametypeahead = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=EntVENDORLIST")

        'End If
        nametypeahead = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=EntVENDORLIST")
        For i = 0 To count - 1
            content &= "$(""#dtgTest_ctl" & i & "_txtVendor"").autocomplete(""" & nametypeahead & """, {" & vbCrLf & _
            "width: 260," & vbCrLf & _
            "scroll: true," & vbCrLf & _
            "selectFirst: false" & vbCrLf & _
            "});" & vbCrLf & _
            "$(""#dtgTest_ctl" & i & "_txtVendor"").result(function(event, data, formatted) {" & vbCrLf & _
            "if (data)" & vbCrLf & _
            "$(""#dtgTest_ctl" & i & "_txtVendorID"").val(data[1]);" & vbCrLf & _
            "});" & vbCrLf
        Next

        typeahead = "<script language=""javascript"">" & vbCrLf & _
          "<!--" & vbCrLf & _
            "$(document).ready(function(){" & vbCrLf & _
            content & vbCrLf & _
            "});" & vbCrLf & _
            "-->" & vbCrLf & _
            "</script>"
        Session("typeahead") = typeahead
    End Sub
    Private Sub ConstructTable()
        Dim strrow As String
        Dim i, count, tablecount As Integer
        Dim table As String
        Dim aryid As New ArrayList
        Dim aryname As New ArrayList
        Dim objps As New PersonalSetting
        Dim ds As DataSet
        Dim row As DataRow
        If ViewState("Count") Is Nothing Then
            ViewState("Count") = 5
            count = ViewState("Count")
        Else
            count = ViewState("Count")
        End If
        'Add record
        If Session("userAction") = "Add" Or ViewState("NewLine") = "Add" Then
            btnDelete.Enabled = False
            For i = 0 To count - 1
                If Session("Columnid") Is Nothing Then
                    aryid.Add("")
                Else
                    aryid = Session("Columnid")
                    If i = count - 1 Then
                        aryid.Add("")
                    End If
                End If
                If Session("ColumnName") Is Nothing Then
                    aryname.Add("")
                Else
                    aryname = Session("ColumnName")
                    If i = count - 1 Then
                        aryname.Add("")
                    End If
                End If
            Next
        End If
        'Modify record

        If Session("userAction") = "Modify" And ViewState("NewLine") <> "Add" Then
            ds = objps.getAryID(hidName.Text)
            tablecount = ds.Tables(0).Rows.Count
            If tablecount > 0 Then
                btnDelete.Enabled = True
            Else
                btnDelete.Enabled = False
            End If
            For Each row In ds.Tables(0).Rows
                aryid.Add(row("RCDLD_V_COY_ID"))
                aryname.Add(row("CM_COY_NAME"))
            Next
            If tablecount < 6 Then
                count = 5
            Else
                count = tablecount
            End If
            For i = tablecount To count
                aryid.Add("")
                aryname.Add("")
            Next
            ViewState("Count") = count
        End If

        For i = 0 To count - 1
            If ((i Mod 2) = 0) Then
                strrow &= "<tr style=""background-color:#fdfdfd;"">"
                strrow &= "<td align=""center"">"
                strrow &= "<input type=""checkbox"" id=""chkSelection" & i & """ name=""chkSelection" & i & """ onclick=""checkChild('chkSelection" & i & "')"">"
                strrow &= "</td>"
                strrow &= "<td>"
                strrow &= "<input type=""text"" id=""dtgTest_ctl" & i & "_txtVendor"" name=""dtgTest_ctl" & i & "_txtVendor"" value=""" & aryname(i) & """ class=""txtbox"">"
                strrow &= "<input type=""text"" id=""dtgTest_ctl" & i & "_txtVendorID"" name=""dtgTest_ctl" & i & "_txtVendorID"" value=""" & aryid(i) & """  style=""display:none;"">"
                strrow &= "</td>"
                strrow &= "</tr>"
            Else
                strrow &= "<tr style=""background-color:#f5f9fc;"">"
                strrow &= "<td align=""center"">"
                strrow &= "<input type=""checkbox"" id=""chkSelection" & i & """ name=""chkSelection" & i & """ onclick=""checkChild('chkSelection" & i & "')"">"
                strrow &= "</td>"
                strrow &= "<td>"
                strrow &= "<input type=""text"" id=""dtgTest_ctl" & i & "_txtVendor"" name=""dtgTest_ctl" & i & "_txtVendor"" value=""" & aryname(i) & """ class=""txtbox"">"
                strrow &= "<input type=""text"" id=""dtgTest_ctl" & i & "_txtVendorID"" name=""dtgTest_ctl" & i & "_txtVendorID"" value=""" & aryid(i) & """  style=""display:none;"">"
                strrow &= "</td>"
                strrow &= "</tr>"

            End If

        Next
        table = "<table border=""1"" class=""grid"" style=""margin-top:10px; width:100%; border-collapse:collapse; line-height:20px; "" >" & _
                                "<tr class=""GridHeader"" style=""font-weight:bold;""><td width=""10%"" align=""center""><input type=""checkbox"" id=""chkAll"" name=""chkAll"" onclick=""selectAll();""></td><td width=""90%"">Vendor</td></tr>" & _
                                strrow & _
                                "</table>"

        Session("ConstructTable") = table
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim val As String = validateInput(val)
        If val = "False" Then Exit Sub

        ValidationSummary1.Visible = True

        If Page.IsValid Then
            Dim i, count, chkListName, chksave As Integer
            Dim chksaveary As New ArrayList
            Dim objps As New PersonalSetting
            count = ViewState("Count")
            If Session("userAction") = "Add" Then

                txtName.Text = txtName.Text.Trim()
                chkListName = objps.chkAddVenList(txtName.Text)
                If chkListName = 5 Then
                    Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Exclamation)
                    Exit Sub
                End If
                For i = 0 To count - 1
                    'If Request.Form("chkSelection" & i & "") = "on" Then
                    If Request.Form("dtgTest_ctl" & i & "_txtVendorID") <> "" Then
                        chksave = objps.addVenList(txtName.Text, Request.Form("dtgTest_ctl" & i & "_txtVendorID"))
                        chksaveary.Add(chksave)
                    Else
                        chksaveary.Add(WheelMsgNum.NotSave)
                    End If
                    'End If
                Next
                Session("userAction") = "Modify"
                hidName.Text = txtName.Text
                If chksaveary.IndexOf(1) <> -1 Then
                    Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Exclamation)
                ElseIf chksaveary.IndexOf(5) <> -1 Then
                    Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Exclamation)
                ElseIf chksaveary.IndexOf(2) <> -1 Then
                    Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Exclamation)
                End If
                ConstructTable()
                populate()
                Exit Sub
            End If
            If Session("userAction") = "Modify" Then
                If txtName.Text <> hidName.Text Then
                    Dim chkModName As Integer
                    chkModName = objps.chkModVenList(txtName.Text, hidName.Text)
                    If chkModName = 5 Then
                        Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Exclamation)
                        Exit Sub
                    End If
                    chksave = objps.modListName(txtName.Text, hidName.Text)
                    chksaveary.Add(chksave)
                    hidName.Text = txtName.Text
                End If
                For i = 0 To count - 1
                    'If Request.Form("chkSelection" & i & "") = "on" Then
                    If Request.Form("dtgTest_ctl" & i & "_txtVendorID") <> "" Then
                        objps.ClearVendorList(hidName.Text)
                        Exit For
                    End If
                    'End If
                Next
                For i = 0 To count - 1
                    'If Request.Form("chkSelection" & i & "") = "on" Then
                    If Request.Form("dtgTest_ctl" & i & "_txtVendorID") <> "" Then
                        chksave = objps.modVendorList2(txtName.Text, hidName.Text, Request.Form("dtgTest_ctl" & i & "_txtVendorID"))
                        chksaveary.Add(chksave)
                    Else
                        chksaveary.Add(WheelMsgNum.NotSave)
                    End If
                    'End If
                Next
                hidName.Text = txtName.Text
                If chksaveary.IndexOf(1) <> -1 Then
                    Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Exclamation)
                ElseIf chksaveary.IndexOf(5) <> -1 Then
                    Common.NetMsgbox(Me, MsgRecordDuplicate, MsgBoxStyle.Exclamation)
                ElseIf chksaveary.IndexOf(2) <> -1 Then
                    Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Exclamation)
                End If
                ViewState("NewLine") = "Modify"
                ConstructTable()
                populate()
                Exit Sub
            End If
        End If
    End Sub
    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        ValidationSummary1.Visible = False
        Dim i, a, msg As Integer
        Dim count As Integer
        Dim rowcount As Integer
        Dim objps As New PersonalSetting
        rowcount = ViewState("Count")
        Dim aryname, aryid, arymsg As New ArrayList

        count = ViewState("Count")
        For i = 0 To count - 1
            If Request.Form("chkSelection" & i & "") = "on" Then
                rowcount = rowcount - 1
                msg = objps.delVendorList(txtName.Text, Request.Form("dtgTest_ctl" & i & "_txtVendorID"))
                arymsg.Add(msg)
            End If
        Next
        If arymsg.IndexOf(10) <> -1 Then
            Common.NetMsgbox(Me, MsgRecordSave, MsgBoxStyle.Exclamation)
            Session("userAction") = "Add"
            hidName.Text = ""
        ElseIf arymsg.IndexOf(3) <> -1 Then
            Common.NetMsgbox(Me, MsgRecordDelete, MsgBoxStyle.Exclamation)
        ElseIf arymsg.IndexOf(4) <> -1 Then
            Common.NetMsgbox(Me, "Record Not Delete", MsgBoxStyle.Exclamation)
        End If

        If rowcount < 5 Then
            ViewState("Count") = 5
        Else
            ViewState("Count") = rowcount
        End If
        ConstructTable()
        populate()
        'Dim i, a As Integer
        'Dim count As Integer
        'Dim rowcount As Integer
        'Dim leftover As Integer = 0
        'rowcount = ViewState("Count")
        'Dim aryname, aryid As New ArrayList

        'count = ViewState("Count")
        'For i = 0 To count - 1
        '    If Request.Form("chkSelection" & i & "") = "on" Then
        '        rowcount = rowcount - 1
        '        leftover = leftover + 1
        '    Else
        '        aryid.Add(Request.Form("dtgTest_ctl" & i & "_txtVendorID"))
        '        aryname.Add(Request.Form("dtgTest_ctl" & i & "_txtVendor"))
        '    End If



        'Next
        'For a = 0 To leftover - 1
        '    aryid.Add("")
        '    aryname.Add("")
        'Next
        'If rowcount < 5 Then
        '    ViewState("Count") = 5
        'Else
        '    ViewState("Count") = rowcount
        'End If
        'Session("ColumnName") = aryname
        'Session("Columnid") = aryid
        'ConstructTable()
        'populate()

    End Sub
    Private Sub btnAddLine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddLine.Click
        ValidationSummary1.Visible = False
        Dim i As Integer
        Dim count As Integer
        Dim aryname, aryid As New ArrayList
        'Modified by Michael'''''''''''''
        If ViewState("Count") Is Nothing Then
            ViewState("Count") = 5
            count = ViewState("Count")
        Else
            ViewState("Count") += 1
            count = ViewState("Count")
        End If
        ''''''''''''''''''''''''''''''''''''
        'If ViewState("Count") = 5 Then
        'count = ViewState("Count")
        ' Else
        'ViewState("Count") += 1
        count = ViewState("Count")
        'End If
        For i = 0 To count - 1
            aryname.Add(Request.Form("dtgTest_ctl" & i & "_txtVendor"))
            aryid.Add(Request.Form("dtgTest_ctl" & i & "_txtVendorID"))

        Next
        Session("ColumnName") = aryname
        Session("Columnid") = aryid
        'Modified by Michael'''''''''''''
        ' ViewState("Count") += 1
        If Session("UserAction") = "Modify" Then
            ViewState("NewLine") = "Add"
        End If
        '''''''''''''''''''''''''''''''''''
        ConstructTable()
        populate()

    End Sub
    Private Function validateInput(ByVal val) As Boolean
        If Not Me.txtName.Text = "" AndAlso Not IsValidAlphaNumericInput(Trim(txtName.Text)) Then
            Common.NetMsgbox(Me, "Please enter valid characters for your List Name.", MsgBoxStyle.Information)
            val = "False"
        Else
            val = "True"
        End If
        Return val
    End Function
End Class