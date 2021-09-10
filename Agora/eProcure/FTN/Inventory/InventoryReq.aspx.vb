'Copyright © 2011 STRATEQ GLOBAL SERVICES. All rights reserved.
Imports AgoraLegacy
Imports eProcure.Component


Public Class InventoryReqFTN
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents hidButton As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidButton1 As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents cmd_Add As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Submit As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Search As System.Web.UI.WebControls.Button
    Protected WithEvents lblIRNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblIssueDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblIssueTo As System.Web.UI.WebControls.Label
    Protected WithEvents lblQty As System.Web.UI.WebControls.Label

    Protected WithEvents btnHidden1 As System.Web.UI.WebControls.Button
    Protected WithEvents cmdPrint As System.Web.UI.HtmlControls.HtmlInputButton

    Protected WithEvents txtIssueTo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRefNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRemark As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtdepartment As System.Web.UI.WebControls.TextBox

    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents txtTemp As System.Web.UI.HtmlControls.HtmlInputText

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objINV As New Inventory
    Dim objGLO As New AppGlobals
    Dim aryInvReq As New ArrayList()
    Dim intNumeric As Boolean = True
    Dim strMsg As String = ""
    Dim LocDesc, SubLocDesc As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        ''cmd_save.Enabled = False
        ''Dim alButtonList As ArrayList
        ''alButtonList = New ArrayList
        ''alButtonList.Add(cmd_save)
        ''htPageAccess.Add("add", alButtonList)
        ''htPageAccess.Add("update", alButtonList)
        ''CheckButtonAccess()
        'cmd_Reset.Enabled = blnCanAdd Or blnCanUpdate Or blnCanDelete
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)

        objINV.GetLocationDesc(LocDesc, SubLocDesc)
        LocDesc = IIf(LocDesc = "", "Location", "" + LocDesc + "")
        SubLocDesc = IIf(SubLocDesc = "", "Sub Location", "" + SubLocDesc + "")

        If Not Page.IsPostBack Then

            Session("aryInvReq") = Nothing
            lblIRNo.Text = "To Be Allocated By System"
            lblIssueDate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Today.Date)

            GenerateTab()

            ViewState("Row") = 5
            BuildRow()
            ConstructTable()

            'cmd_Search.Attributes.Add("onclick", "Search(); ")

            'cmdPrint.Disabled = True

            txtRemark.Attributes.Add("onKeyDown", "limitText (this, 400);")

        End If
        Populate()

    End Sub

    Private Function validateInputs() As Boolean

        If Me.txtRemark.Text.Length > Me.txtRemark.MaxLength Then
            strMsg = "<ul type='disc'><li>" & "IR Remark should be less than " & Me.txtRemark.MaxLength & " characters." & "<ul type='disc'></ul></li></ul>"
            Return False
        End If

        Return True
    End Function

    Private Sub cmd_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Add.Click
        Rebuild()
        BuildLine()
        Rebuild()
        ConstructTable()
        Populate()

    End Sub

    Private Sub hidButton_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidButton.ServerClick
        Rebuild()
        ConstructTable()
    End Sub

    Private Sub hidButton1_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidButton1.ServerClick
        Rebuild()
        ConstructTable()
    End Sub

    Private Function ConstructTable()
        Dim strrow As String
        Dim i, h, j, count As Integer
        Dim table As String
        Dim item2nd As Boolean = False
        Dim found As Boolean = False
        Dim aryInvReq_temp As New ArrayList

        Dim c As Integer
        Dim ds, dsSub As DataSet

        aryInvReq = Session("aryInvReq")
        count = aryInvReq.Count

        For i = 0 To count - 1
            If aryInvReq(i)(0) <> "" Then
                ds = objINV.PopLocationWItem(aryInvReq(i)(0))
                If ds.Tables(0).Rows.Count = 0 Then
                    ds = objINV.PopLocation()
                End If
            Else
                ds = objINV.PopLocation()
            End If

            dsSub = objINV.PopLocation("")
            'If aryInvReq(i)(3) <> Request.QueryString("item") Then
            '    Continue For
            'End If
            'If ((i Mod 2) = 0) Then
            strrow &= "<tr style=""background-color:#fdfdfd;"">"
            strrow &= "<td >"
            strrow &= "<input style=""width:150px;margin-right:0px; ""  class=""txtbox2"" type=""text"" id=""txtItem" & i & """ name=""txtItem" & i & """ value=""" & aryInvReq(i)(0) & """>"
            strrow &= "</td>"

            strrow &= "<td >"
            strrow &= "<input style=""width:355px;margin-right:0px; "" class=""txtbox2"" type=""text"" id=""txtName" & i & """ name=""txtName" & i & """ value=""" & aryInvReq(i)(1) & """>"
            strrow &= "</td>"

            strrow &= "<td >"
            strrow &= "<input style=""width:60px;margin-right:0px; "" onkeypress=""return isNumberKey(event)"" class=""numerictxtbox2"" type=""text"" id=""txtQty" & i & """ name=""txtQty" & i & """ value=""" & aryInvReq(i)(2) & """>"
            strrow &= "</td>"

            strrow &= "<td>"
            strrow &= "<select class=""ddl2"" style=""width:100px;margin-right:0px;"" onchange =""onClick()"" id=""cboLocation" & i & """ name=""cboLocation" & i & """>"
            strrow &= "<option value=""---Select---"" selected=""selected"">" & "---Select---" & "</option>"

            For c = 0 To ds.Tables(0).Rows.Count - 1
                If aryInvReq(i)(3) = ds.Tables(0).Rows(c).Item(0).ToString Then
                    strrow &= "<option value=""" & ds.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & ds.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    dsSub = objINV.PopLocationWItem(aryInvReq(i)(0), ds.Tables(0).Rows(c).Item(0).ToString)
                Else
                    strrow &= "<option value=""" & ds.Tables(0).Rows(c).Item(0).ToString & """>" & ds.Tables(0).Rows(c).Item(0).ToString & "</option>"
                End If
            Next

            strrow &= "</select>"
            strrow &= "</td>"


            strrow &= "<td>"
            strrow &= "<select class=""ddl2"" style=""width:100px;margin-right:0px;"" id=""cboSubLocation" & i & """ name=""cboSubLocation" & i & """>"

            For c = 0 To dsSub.Tables(0).Rows.Count - 1
                If aryInvReq(i)(4) = dsSub.Tables(0).Rows(c).Item(0).ToString Then
                    strrow &= "<option value=""" & dsSub.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & dsSub.Tables(0).Rows(c).Item(0).ToString & "</option>"
                Else
                    strrow &= "<option value=""" & dsSub.Tables(0).Rows(c).Item(0).ToString & """>" & dsSub.Tables(0).Rows(c).Item(0).ToString & "</option>"
                End If

            Next
            If dsSub.Tables(0).Rows.Count = 0 Then
                strrow &= "<option value=""---Select---"" selected=""selected"">" & "---Select---" & "</option>"
            End If

            strrow &= "</select>"
            strrow &= "</td>"
            strrow &= "</tr>"

            'Else
            '    strrow &= "<tr style=""background-color:#f5f9fc;"">"
            'End If

        Next
        table = "<table class=""grid"" style=""margin-top:10px; border-collapse:collapse; line-height:20px; "" >" & _
                "<tr class=""TableHeader""><td style=""width:150px;margin-right:0px;"">Item Code</td><td style=""width:355px; margin-right:0px;"">Item Name</td><td style=""width:60px; text-align:right;""><label id=""lblQty"">Qty</label></td><td style=""width:100px;"">" & LocDesc & "</td><td style=""width:100px;"">" & SubLocDesc & "</td></tr>" & _
                strrow & _
                "</table>"

        Session("ConstructTable") = table

        'cmd_Search.Attributes.Add("onclick", "Search(); ")

    End Function

    Private Function Rebuild()
        Dim i As Integer
        Dim found As Integer
        found = 0
        intNumeric = True

        aryInvReq = Session("aryInvReq")
        For i = 0 To aryInvReq.Count - 1
            aryInvReq(i)(0) = Request.Form("txtItem" & i)
            aryInvReq(i)(1) = Request.Form("txtName" & i)
            aryInvReq(i)(2) = Request.Form("txtQty" & i)
            aryInvReq(i)(3) = Request.Form("cboLocation" & i)
            aryInvReq(i)(4) = Request.Form("cboSubLocation" & i)

            Dim iCheck As Integer
            iCheck = IsNumeric(Request.Form("txtQty" & i))
            'If IsNumeric(Request.Form("txtQty" & i)) = False And Request.Form("txtQty" & i) <> "" Then
            If (Regex.IsMatch(Trim(Request.Form("txtQty" & i)), "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$")) = False And Request.Form("txtQty" & i) <> "" Then
                intNumeric = False
            End If
        Next

        Session("aryInvReq") = Nothing
        Session("aryInvReq") = aryInvReq
    End Function

    Private Function Rebuild2()
        Dim i As Integer
        Dim found As Integer
        found = 0
        intNumeric = True

        aryInvReq = Session("aryInvReq")
        For i = 0 To aryInvReq.Count - 1
            aryInvReq(i)(0) = Request.Form("txtItem" & i)
            aryInvReq(i)(1) = Request.Form("txtName" & i)
            aryInvReq(i)(2) = Request.Form("txtQty" & i)
            aryInvReq(i)(3) = Request.Form("cboLocation" & i)
            aryInvReq(i)(4) = Request.Form("cboSubLocation" & i)

            Dim iCheck As Integer
            iCheck = IsNumeric(Request.Form("txtQty" & i))
            'If IsNumeric(Request.Form("txtQty" & i)) = False And Request.Form("txtQty" & i) <> "" Then
            If (Regex.IsMatch(Trim(Request.Form("txtQty" & i)), "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$")) = False And Request.Form("txtQty" & i) <> "" Then
                intNumeric = False
            End If
        Next

        i = 5
        Do While i < aryInvReq.Count
            If aryInvReq(i)(0) = "" Then
                aryInvReq.RemoveAt(i)
                i = 5
            Else
                i = i + 1
            End If
        Loop

        Session("aryInvReq") = Nothing
        Session("aryInvReq") = aryInvReq
    End Function

    Private Function BuildLine()
        Dim i As Integer
        Dim found As Integer
        found = 0

        aryInvReq = Session("aryInvReq")
        aryInvReq.Add(New String() {"", "", "", "---Select---", "---Select---", ""})
        Session("aryInvReq") = Nothing
        Session("aryInvReq") = aryInvReq
    End Function

    Private Function BuildRow()
        Dim i As Integer
        Dim found As Boolean
        found = False

        If ViewState("Saved") = "Saved" Then
            Exit Function
        End If

        If Session("aryInvReq") Is Nothing Then 'Empty
            For i = 0 To ViewState("Row") - 1
                aryInvReq.Add(New String() {Request.Form("txtItem" & i), Request.Form("txtName" & i), Request.Form("txtQty" & i), Request.Form("cboLocation" & i), Request.Form("cboSubLocation" & i), ""})
            Next
            Session("aryInvReq") = aryInvReq
        End If

    End Function

    Public Sub btnHidden1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden1.Click
        ConstructTable()
    End Sub

    Sub Populate()
        Dim typeahead As String
        Dim i As Integer
        Dim content As String
        Dim nametypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=name")
        Dim codetypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=code")

        aryInvReq = Session("aryInvReq")
        For i = 0 To aryInvReq.Count - 1
            content &= "$(""#txtName" & i & """).autocomplete(""" & nametypeahead & """, {" & vbCrLf & _
            "width: 355," & vbCrLf & _
            "scroll: true," & vbCrLf & _
            "selectFirst: false" & vbCrLf & _
            "});" & vbCrLf & _
            "$(""#txtName" & i & """).result(function(event, data, formatted) {" & vbCrLf & _
            "$(""#txtTemp"").focus();" & vbCrLf & _
            "if (data)" & vbCrLf & _
            "$(""#txtItem" & i & """).val(data[1]);" & vbCrLf & _
            "});" & vbCrLf & _
            "$(""#txtItem" & i & """).autocomplete(""" & codetypeahead & """, {" & vbCrLf & _
            "width: 150," & vbCrLf & _
            "scroll: true," & vbCrLf & _
            "selectFirst: false" & vbCrLf & _
            "});" & vbCrLf & _
            "$(""#txtItem" & i & """).result(function(event, data, formatted) {" & vbCrLf & _
            "$(""#txtTemp"").focus();" & vbCrLf & _
            "if (data)" & vbCrLf & _
            "$(""#txtName" & i & """).val(data[1]);" & vbCrLf & _
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

    Private Sub GenerateTab()
        If strPageId = Nothing Then
            strPageId = Session("strPageId")
        Else
            Session("strPageId") = strPageId
        End If

        Session("w_InventoryReq_tabs") = "<div class=""t_entity""><ul>" & _
                                    "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "InventoryReq.aspx", "pageid=" & strPageId) & """><span>Inventory Requisition</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                     "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "InventoryReqList.aspx", "") & """><span>Inventory Requisition Listing</span></a></li>" & _
                                     "<li><div class=""space""></div></li>" & _
                                   "</ul><div></div></div>"


    End Sub

    Private Sub cmd_Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Submit.Click
        Dim strNewIR, intMsg, Code As String
        Dim iCheck As Boolean = False
        Dim iCheckQty As Boolean = False
        Dim iCheckLoc As Boolean = False
        Dim iCheckQtyZero As Boolean = False
        Dim i As Integer
        Dim RLoc, RSLoc As String

        Rebuild()
        aryInvReq = Session("aryInvReq")
        For i = 0 To aryInvReq.Count - 1
            If aryInvReq(i)(0) <> "" Then
                iCheck = True
                If aryInvReq(i)(0) <> "" And (aryInvReq(i)(2) = "") Then
                    iCheckQty = True
                    Exit For
                    'ElseIf aryInvReq(i)(0) <> "" And (CDec(IIf(aryInvReq(i)(2) = "", 0, aryInvReq(i)(2))) = 0) Then
                    '    iCheckQtyZero = True
                    '    Exit For
                Else
                    If aryInvReq(i)(0) <> "" And aryInvReq(i)(2) <> "" And aryInvReq(i)(3) = "---Select---" Then
                        iCheckLoc = True
                        Exit For
                    End If
                End If
            End If
        Next
        If iCheck = False Then
            strMsg = "<ul type='disc'><li>" & objGLO.GetErrorMessage("00020") & "<ul type='disc'></ul></li></ul>"
        Else
            If iCheckQty = True Then
                strMsg = "<ul type='disc'><li>" & "" & objGLO.GetErrorMessage("00342") & "<ul type='disc'></ul></li></ul>"
            ElseIf iCheckQtyZero = True Then
                strMsg = "<ul type='disc'><li>" & "" & objGLO.GetErrorMessage("00342") & "" & "<ul type='disc'></ul></li></ul>"
            Else
                If iCheckLoc = True Then
                    strMsg = "<ul type='disc'><li>" & objGLO.GetErrorMessage("00014") & "<ul type='disc'></ul></li></ul>"
                End If
            End If
        End If

        If intNumeric = False Then
            strMsg = "<ul type='disc'><li>" & "" & " " & objGLO.GetErrorMessage("00342") & "<ul type='disc'></ul></li></ul>"
        End If

        If txtIssueTo.Text = "" Then
            strMsg = "<ul type='disc'><li>" & lblIssueTo.Text & " " & objGLO.GetErrorMessage("00001") & "<ul type='disc'></ul></li></ul>"
        End If

        Call validateInputs()

        If strMsg <> "" Then
            lblMsg.Text = strMsg
            ConstructTable()
            Exit Sub
        Else
            lblMsg.Text = ""
        End If

        aryInvReq = Session("aryInvReq")
        If Not IsNumeric(Mid(lblIRNo.Text, 3)) Then
            Rebuild()
            ConstructTable()
            intMsg = objINV.insertIR(aryInvReq, strNewIR, txtIssueTo.Text, txtRefNo.Text, txtRemark.Text, Code, txtdepartment.Text, RLoc, RSLoc)
            Rebuild2()
            ConstructTable()
        End If
        Select Case intMsg
            Case WheelMsgNum.Save
                lblIRNo.Text = strNewIR
                'cmd_Submit.Enabled = False
                'cmd_Add.Enabled = False
                'cmd_Search.Enabled = False
                cmd_Submit.Visible = False
                cmd_Add.Visible = False
                cmd_Search.Visible = False
                Common.NetMsgbox(Me, "IR Number " & strNewIR & " " & objGLO.GetErrorMessage("00024"), MsgBoxStyle.Information)
                cmdPrint.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewIRSlip.aspx", "IRNo=" & Trim(lblIRNo.Text) & "&CoyID=" & Session("CompanyID") & "") & "')")
                ' cmdPrint.Disabled = False
                cmdPrint.Visible = True

            Case WheelMsgNum.NotSave
                Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)

            Case 10
                If RSLoc <> "---Select---" And RSLoc <> "" Then
                    Common.NetMsgbox(Me, Code & " From " & RLoc & " & " & RSLoc & " " & objGLO.GetErrorMessage("00022"), MsgBoxStyle.Information)
                Else
                    Common.NetMsgbox(Me, Code & " From " & RLoc & " " & objGLO.GetErrorMessage("00022"), MsgBoxStyle.Information)
                End If


        End Select
    End Sub

    Private Sub cmd_Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Search.Click
        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        Dim strName As String

        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("Search", "InventoryItemSearchPopup.aspx", strName)
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','580px');")
        strscript.Append("document.getElementById('btnHidden1').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())
    End Sub
End Class
