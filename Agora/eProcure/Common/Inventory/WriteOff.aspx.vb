Imports AgoraLegacy
Imports eProcure.Component


Public Class WriteOff
    Inherits AgoraLegacy.AppBaseClass
    Protected WithEvents hidButton As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidButton1 As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents cmd_Add As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Submit As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Search As System.Web.UI.WebControls.Button
    Protected WithEvents lblWONo As System.Web.UI.WebControls.Label
    Protected WithEvents lblWODate As System.Web.UI.WebControls.Label
    Protected WithEvents lblQty As System.Web.UI.WebControls.Label
    Protected WithEvents btnHidden1 As System.Web.UI.WebControls.Button
    Protected WithEvents cmdUpload As System.Web.UI.WebControls.Button
    Protected WithEvents cmdPrint As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents txtRemark As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents txtTemp As System.Web.UI.HtmlControls.HtmlInputText
    Protected WithEvents File1 As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents pnlAttach As System.Web.UI.WebControls.Panel

    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objINV As New Inventory
    Dim objGLO As New AppGlobals
    Dim aryWO As New ArrayList()
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
    End Sub

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)

        objINV.GetLocationDesc(LocDesc, SubLocDesc)
        LocDesc = IIf(LocDesc = "", "Location", "" + LocDesc + "")
        SubLocDesc = IIf(SubLocDesc = "", "Sub Location", "" + SubLocDesc + "")

        If Not Page.IsPostBack Then

            Session("aryWO") = Nothing
            lblWONo.Text = "To Be Allocated By System"
            lblWODate.Text = Common.FormatWheelDate(WheelDateFormat.ShortDate, Today.Date)
            objINV.delete_TempAttachment(Session("CompanyId") & "_" & Session("UserId"))
            GenerateTab()

            ViewState("Row") = 5
            BuildRow()
            ConstructTable()

            'cmd_Search.Attributes.Add("onclick", "Search(); ")

            'cmdPrint.Disabled = True
            cmdUpload.Attributes.Add("onclick", "return checkDocFile('doc','" & File1.ClientID & "');")
            txtRemark.Attributes.Add("onKeyDown", "limitText (this, 400);")

        End If

        displayAttachFile()
        Populate()

    End Sub

    Private Function validateInputs() As Boolean
        If Me.txtRemark.Text.Length > Me.txtRemark.MaxLength Then
            strMsg = "<ul type='disc'><li>" & "WO Remark should be less than " & Me.txtRemark.MaxLength & " characters." & "<ul type='disc'></ul></li></ul>"
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
        Dim table, strUom As String
        Dim item2nd As Boolean = False
        Dim found As Boolean = False
        Dim aryWO_temp As New ArrayList

        Dim c As Integer
        Dim dsLot, dsLoc, dsLocSub As DataSet
        aryWO = Session("aryWO")
        count = aryWO.Count

        For i = 0 To count - 1
            If aryWO(i)(0) <> "" Then
                dsLot = objINV.PopLotWItemLotLocation(aryWO(i)(0))
            Else
                dsLot = Nothing
            End If

            If aryWO(i)(0) <> "" And (aryWO(i)(3) <> "" And aryWO(i)(3) <> "---Select---") Then
                dsLoc = objINV.PopLotWItemLotLocation(aryWO(i)(0), aryWO(i)(3))
            Else
                dsLoc = Nothing
            End If

            If aryWO(i)(0) <> "" And (aryWO(i)(3) <> "" And aryWO(i)(3) <> "---Select---") And (aryWO(i)(4) <> "" And aryWO(i)(4) <> "---Select---") Then
                dsLocSub = objINV.PopLotWItemLotLocation(aryWO(i)(0), aryWO(i)(3), aryWO(i)(4))
            Else
                dsLocSub = Nothing
            End If

            If aryWO(i)(0) <> "" Then
                strUom = objINV.getInvItemUom(aryWO(i)(0))
            Else
                strUom = ""
            End If

            strrow &= "<tr style=""background-color:#fdfdfd;"">"
            strrow &= "<td >"
            strrow &= "<input style=""width:150px;margin-right:0px; ""  class=""txtbox2"" type=""text"" id=""txtItem" & i & """ name=""txtItem" & i & """ value=""" & aryWO(i)(0) & """>"
            strrow &= "</td>"

            strrow &= "<td >"
            strrow &= "<input style=""width:250px;margin-right:0px; "" class=""txtbox2"" type=""text"" id=""txtName" & i & """ name=""txtName" & i & """ value=""" & aryWO(i)(1) & """>"
            strrow &= "</td>"

            strrow &= "<td>"
            strrow &= "<label id=""lblUom" & i & """ style=""width:70px;margin-right:0px; "" class=""txtbox2"" >" & strUom & "</label>"
            strrow &= "</td>"

            strrow &= "<td >"
            strrow &= "<input style=""width:80px;margin-right:0px; "" onkeypress=""return isNumberKey(event)"" class=""numerictxtbox2"" type=""text"" id=""txtQty" & i & """ name=""txtQty" & i & """ value=""" & aryWO(i)(2) & """>"
            strrow &= "</td>"

            strrow &= "<td>"
            strrow &= "<select class=""ddl2"" style=""width:125px;margin-right:0px;"" onchange =""onClick()"" id=""cboLotNo" & i & """ name=""cboLotNo" & i & """>"
            strrow &= "<option value=""---Select---"" selected=""selected"">" & "---Select---" & "</option>"

            If Not dsLot Is Nothing Then
                For c = 0 To dsLot.Tables(0).Rows.Count - 1
                    If aryWO(i)(3) = dsLot.Tables(0).Rows(c).Item(0).ToString Then
                        strrow &= "<option value=""" & dsLot.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & dsLot.Tables(0).Rows(c).Item(0).ToString & "</option>"
                        dsLoc = objINV.PopLotWItemLotLocation(aryWO(i)(0), dsLot.Tables(0).Rows(c).Item(0).ToString)
                    Else
                        strrow &= "<option value=""" & dsLot.Tables(0).Rows(c).Item(0).ToString & """>" & dsLot.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    End If
                Next
            End If

            strrow &= "</select>"
            strrow &= "</td>"

            strrow &= "<td>"
            strrow &= "<select class=""ddl2"" style=""width:125px;margin-right:0px;"" onchange =""onClick()"" id=""cboLocation" & i & """ name=""cboLocation" & i & """>"
            strrow &= "<option value=""---Select---"" selected=""selected"">" & "---Select---" & "</option>"

            If Not dsLoc Is Nothing Then
                For c = 0 To dsLoc.Tables(0).Rows.Count - 1
                    If aryWO(i)(4) = dsLoc.Tables(0).Rows(c).Item(0).ToString Then
                        strrow &= "<option value=""" & dsLoc.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & dsLoc.Tables(0).Rows(c).Item(0).ToString & "</option>"
                        dsLocSub = objINV.PopLotWItemLotLocation(aryWO(i)(0), aryWO(i)(3), dsLoc.Tables(0).Rows(c).Item(0).ToString)
                    Else
                        strrow &= "<option value=""" & dsLoc.Tables(0).Rows(c).Item(0).ToString & """>" & dsLoc.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    End If
                Next
            End If

            strrow &= "</select>"
            strrow &= "</td>"

            strrow &= "<td>"
            strrow &= "<select class=""ddl2"" style=""width:125px;margin-right:0px;"" id=""cboSubLocation" & i & """ name=""cboSubLocation" & i & """>"
            'strrow &= "<option value=""---Select---"" selected=""selected"">" & "---Select---" & "</option>"

            If Not dsLocSub Is Nothing Then
                For c = 0 To dsLocSub.Tables(0).Rows.Count - 1
                    If aryWO(i)(5) = dsLocSub.Tables(0).Rows(c).Item(0).ToString Then
                        strrow &= "<option value=""" & dsLocSub.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & dsLocSub.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    Else
                        strrow &= "<option value=""" & dsLocSub.Tables(0).Rows(c).Item(0).ToString & """>" & dsLocSub.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    End If
                Next
            Else
                strrow &= "<option value=""---Select---"" selected=""selected"">" & "---Select---" & "</option>"
            End If

            'If dsLocSub.Tables(0).Rows.Count = 0 Then
            '    strrow &= "<option value=""---Select---"" selected=""selected"">" & "---Select---" & "</option>"
            'End If

            strrow &= "</select>"
            strrow &= "</td>"
            strrow &= "</tr>"
        Next
        table = "<table class=""grid"" style=""margin-top:10px; border-collapse:collapse; line-height:20px; "" >" & _
                "<tr class=""TableHeader""><td style=""width:150px;margin-right:0px;"">Item Code</td><td style=""width:250px; margin-right:0px;"">Item Name</td><td style=""width:70px; margin-right:0px;"">UOM</td><td style=""width:80px; text-align:right;""><label id=""lblQty"">Write Off Qty</label></td><td style=""width:100px;"">" & "Lot No" & "</td><td style=""width:100px;"">" & LocDesc & "</td><td style=""width:100px;"">" & SubLocDesc & "</td></tr>" & _
                strrow & _
                "</table>"

        Session("ConstructTable") = table
    End Function

    Private Function Rebuild()
        Dim i As Integer
        Dim found As Integer
        found = 0
        intNumeric = True

        aryWO = Session("aryWO")
        For i = 0 To aryWO.Count - 1
            aryWO(i)(0) = Request.Form("txtItem" & i)
            aryWO(i)(1) = Request.Form("txtName" & i)
            aryWO(i)(2) = Request.Form("txtQty" & i)
            aryWO(i)(3) = Request.Form("cboLotNo" & i)
            aryWO(i)(4) = Request.Form("cboLocation" & i)
            aryWO(i)(5) = Request.Form("cboSubLocation" & i)

            Dim iCheck As Integer
            iCheck = IsNumeric(Request.Form("txtQty" & i))
            'If IsNumeric(Request.Form("txtQty" & i)) = False And Request.Form("txtQty" & i) <> "" Then
            If (Regex.IsMatch(Trim(Request.Form("txtQty" & i)), "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$")) = False And Request.Form("txtQty" & i) <> "" Then
                intNumeric = False
            End If
        Next

        Session("aryWO") = Nothing
        Session("aryWO") = aryWO
    End Function

    Private Function Rebuild2()
        Dim i As Integer
        Dim found As Integer
        found = 0
        intNumeric = True

        aryWO = Session("aryWO")
        For i = 0 To aryWO.Count - 1
            aryWO(i)(0) = Request.Form("txtItem" & i)
            aryWO(i)(1) = Request.Form("txtName" & i)
            aryWO(i)(2) = Request.Form("txtQty" & i)
            aryWO(i)(3) = Request.Form("cboLotNo" & i)
            aryWO(i)(4) = Request.Form("cboLocation" & i)
            aryWO(i)(5) = Request.Form("cboSubLocation" & i)

            Dim iCheck As Integer
            iCheck = IsNumeric(Request.Form("txtQty" & i))
            'If IsNumeric(Request.Form("txtQty" & i)) = False And Request.Form("txtQty" & i) <> "" Then
            If (Regex.IsMatch(Trim(Request.Form("txtQty" & i)), "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$")) = False And Request.Form("txtQty" & i) <> "" Then
                intNumeric = False
            End If
        Next

        i = 5
        Do While i < aryWO.Count
            If aryWO(i)(0) = "" Then
                aryWO.RemoveAt(i)
                i = 5
            Else
                i = i + 1
            End If
        Loop

        Session("aryWO") = Nothing
        Session("aryWO") = aryWO
    End Function

    Private Function BuildLine()
        Dim i As Integer
        Dim found As Integer
        found = 0

        aryWO = Session("aryWO")
        aryWO.Add(New String() {"", "", "", "---Select---", "---Select---", "---Select---", ""})
        Session("aryWO") = Nothing
        Session("aryWO") = aryWO
    End Function

    Private Function BuildRow()
        Dim i As Integer
        Dim found As Boolean
        found = False

        If ViewState("Saved") = "Saved" Then
            Exit Function
        End If

        If Session("aryWO") Is Nothing Then 'Empty
            For i = 0 To ViewState("Row") - 1
                aryWO.Add(New String() {Request.Form("txtItem" & i), Request.Form("txtName" & i), Request.Form("txtQty" & i), Request.Form("cboLotNo" & i), Request.Form("cboLocation" & i), Request.Form("cboSubLocation" & i), ""})
            Next
            Session("aryWO") = aryWO
        End If

    End Function

    Public Sub btnHidden1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidden1.Click
        ConstructTable()
    End Sub

    Sub Populate()
        Dim typeahead As String
        Dim i As Integer
        Dim content As String
        Dim nametypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=nameWO")
        Dim codetypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=codeWO")

        aryWO = Session("aryWO")
        For i = 0 To aryWO.Count - 1
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

        Session("w_WriteOff_tabs") = "<div class=""t_entity""><ul>" & _
                "<li><div class=""space""></div></li>" & _
                "<li><a class=""t_entity_btn_selected"" href=""" & dDispatcher.direct("Inventory", "WriteOff.aspx", "pageid=" & strPageId) & """><span>Write Off</span></a></li>" & _
                "<li><div class=""space""></div></li>" & _
                "<li><a class=""t_entity_btn"" href=""" & dDispatcher.direct("Inventory", "WriteOffSearch.aspx", "") & """><span>Write Off Listing</span></a></li>" & _
                "<li><div class=""space""></div></li>" & _
                "</ul><div></div></div>"
    End Sub

    Private Sub cmd_Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Submit.Click
        Dim strNewWO, intMsg, Code As String
        Dim iCheck As Boolean = False
        Dim iCheckQty As Boolean = False
        Dim iCheckLot As Boolean = False
        Dim iCheckLoc As Boolean = False
        Dim iCheckSubLoc As Boolean = False
        Dim iCheckQtyZero As Boolean = False
        Dim i As Integer
        Dim RLot, RLoc, RSLoc As String
        Dim checkSubLocation As String
        Dim objDb As New EAD.DBCom

        Rebuild()
        aryWO = Session("aryWO")
        For i = 0 To aryWO.Count - 1
            If aryWO(i)(0) <> "" Then
                iCheck = True
                If aryWO(i)(0) <> "" And (aryWO(i)(2) = "") Then
                    iCheckQty = True
                    Exit For
                Else
                    If aryWO(i)(0) <> "" And aryWO(i)(2) <> "" And aryWO(i)(3) = "---Select---" Then
                        iCheckLot = True
                        Exit For
                    Else
                        If aryWO(i)(0) <> "" And aryWO(i)(2) <> "" And aryWO(i)(3) <> "---Select---" And aryWO(i)(4) = "---Select---" Then
                            iCheckLoc = True
                            Exit For
                        Else
                            'If aryWO(i)(0) <> "" And aryWO(i)(2) <> "" And aryWO(i)(3) <> "---Select---" And aryWO(i)(4) <> "---Select---" And aryWO(i)(5) = "" Then
                            '    iCheckSubLoc = True

                            '    checkSubLocation = objDb.GetVal("SELECT IFNULL(IM_ITEM_CODE,'') AS IM_ITEM_CODE FROM INVENTORY_DETAIL, INVENTORY_MSTR, INVENTORY_LOT, DO_LOT, LOCATION_MSTR WHERE  ID_INVENTORY_INDEX = IM_INVENTORY_INDEX AND  ID_INVENTORY_INDEX = IL_INVENTORY_INDEX AND IL_LOT_INDEX = DOL_LOT_INDEX AND ID_LOCATION_INDEX = IL_LOCATION_INDEX AND ID_LOCATION_INDEX = LM_LOCATION_INDEX AND IM_ITEM_CODE = '" & aryWO(i)(0) & "' AND  IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND LM_LOCATION = '" & aryWO(i)(4) & "' AND IFNULL(LM_SUB_LOCATION, '') = '' AND DOL_LOT_NO = '" & aryWO(i)(3) & "'")

                            '    If checkSubLocation = "" Then
                            '        strMsg = "<ul type='disc'><li>" & aryWO(i)(0) & " item does/do not exist in specified location." & "<ul type='disc'></ul></li></ul>"
                            '    End If

                            '    Exit For
                            'End If
                        End If
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
            ElseIf iCheckLot = True Then
                strMsg = "<ul type='disc'><li>" & "Not all items are specified with lot no." & "<ul type='disc'></ul></li></ul>"
                'ElseIf (iCheckLoc = True Or iCheckSubLoc = True) Then
                '    strMsg = "<ul type='disc'><li>" & objGLO.GetErrorMessage("00014") & "<ul type='disc'></ul></li></ul>"
            ElseIf (iCheckLoc = True) Then
                strMsg = "<ul type='disc'><li>" & objGLO.GetErrorMessage("00014") & "<ul type='disc'></ul></li></ul>"
            ElseIf (iCheckSubLoc = True) Then
                'checkSubLocation = objDb.GetVal("SELECT IFNULL(IM_ITEM_CODE,'') AS IM_ITEM_CODE FROM INVENTORY_DETAIL, INVENTORY_MSTR, INVENTORY_LOT, DO_LOT, LOCATION_MSTR WHERE  ID_INVENTORY_INDEX = IM_INVENTORY_INDEX AND  ID_INVENTORY_INDEX = IL_INVENTORY_INDEX AND IL_LOT_INDEX = DOL_LOT_INDEX AND ID_LOCATION_INDEX = IL_LOCATION_INDEX AND ID_LOCATION_INDEX = LM_LOCATION_INDEX AND IM_ITEM_CODE = 'INV/TAS/03' AND  IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND LM_LOCATION = '" & aryWO(i)(4) & "' AND IFNULL(LM_SUB_LOCATION, '') = '' AND DOL_LOT_NO = '" & aryWO(i)(3) & "'")

                'If checkSubLocation = "" Then
                '    strMsg = "<ul type='disc'><li>" & aryWO(i)(0) & " item does/do not exist in specified location." & "<ul type='disc'></ul></li></ul>"
                'End If
            End If
        End If

        If intNumeric = False Then
            strMsg = "<ul type='disc'><li>" & "" & " " & objGLO.GetErrorMessage("00342") & "<ul type='disc'></ul></li></ul>"
        End If

        'If txtIssueTo.Text = "" Then
        '    strMsg = "<ul type='disc'><li>" & lblIssueTo.Text & " " & objGLO.GetErrorMessage("00001") & "<ul type='disc'></ul></li></ul>"
        'End If

        Call validateInputs()

        If strMsg <> "" Then
            lblMsg.Text = strMsg
            ConstructTable()
            Exit Sub
        Else
            lblMsg.Text = ""
        End If

        aryWO = Session("aryWO")
        If Not IsNumeric(Mid(lblWONo.Text, 3)) Then
            Rebuild()
            ConstructTable()
            intMsg = objINV.insertWO(aryWO, strNewWO, txtRemark.Text, Code, RLot, RLoc, RSLoc)
            Rebuild2()
            ConstructTable()
        End If
        Select Case intMsg
            Case WheelMsgNum.Save
                lblWONo.Text = strNewWO
                cmd_Submit.Visible = False
                cmd_Add.Visible = False
                cmd_Search.Visible = False
                Common.NetMsgbox(Me, "WO Number " & strNewWO & " " & objGLO.GetErrorMessage("00024"), dDispatcher.direct("Inventory", "WriteOff.aspx", "pageid=" & strPageId), MsgBoxStyle.Information)
                'cmdPrint.Attributes.Add("onclick", "PopWindow('" & dDispatcher.direct("Report", "PreviewIRSlip.aspx", "IRNo=" & Trim(lblWONo.Text) & "&CoyID=" & Session("CompanyID") & "") & "')")
                'cmdPrint.Visible = True

            Case WheelMsgNum.NotSave
                Common.NetMsgbox(Me, MsgRecordNotSave, MsgBoxStyle.Information)

            Case 10
                If iCheckSubLoc = True Then
                    Common.NetMsgbox(Me, Code & " From " & RLot & " & " & RLoc & " does not have enough quantity to be written off.", MsgBoxStyle.Information)
                Else
                    Common.NetMsgbox(Me, Code & " From " & RLot & " & " & RLoc & " & " & RSLoc & " " & "does not have enough quantity to be written off.", MsgBoxStyle.Information)
                End If
        End Select
    End Sub

    Private Sub cmd_Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Search.Click
        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        Dim strName As String

        strscript.Append("<script language=""javascript"">")
        strFileName = dDispatcher.direct("Search", "InventoryWOItemSearchPopup.aspx", "type=WO")
        strFileName = Server.UrlEncode(strFileName)
        strscript.Append("ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','580px');")
        strscript.Append("document.getElementById('btnHidden1').click();")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())
    End Sub

    Private Sub cmdUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        Dim objFile As New FileManagement
        If File1.Value = "" Then

        Else
            Dim objDB As New EAD.DBCom
            Dim dblMaxFileSize As Double = Session("FileSize")

            Dim sFileName As String
            sFileName = System.IO.Path.GetFileName(File1.PostedFile.FileName)

            If Len(sFileName) > 50 Then
                Common.NetMsgbox(Me, "File name exceeds 46 character")
            ElseIf File1.PostedFile.ContentLength > 0 And File1.PostedFile.ContentLength / 1024 <= dblMaxFileSize Then
                objFile.FileUpload(File1, EnumUploadType.DocAttachment, "WO", EnumUploadFrom.FrontOff, Session("CompanyId") & "_" & Session("UserId"), , , , , , "I")
            ElseIf File1.PostedFile.ContentLength = 0 Then
                Common.NetMsgbox(Me, "0 byte document or file not found")
            Else
                Common.NetMsgbox(Me, "File exceeds maximum file size")
            End If

            displayAttachFile()
        End If
    End Sub

    Private Sub displayAttachFile()
        Dim dsAttach As New DataSet
        Dim drvAttach As DataView
        Dim i As Integer
        Dim strFile, strFile1, strURL, strTemp As String
        Dim objFile As New FileManagement

        dsAttach = objINV.getWOAttach(Session("CompanyID") & "_" & Session("UserId"), Session("CompanyID"))

        Dim intCount As Integer
        pnlAttach.Controls.Clear()
        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                If Common.parseNull(drvAttach(i)("CDA_TYPE")) = "I" Then
                    strFile = drvAttach(i)("CDA_ATTACH_FILENAME")
                    strFile1 = drvAttach(i)("CDA_HUB_FILENAME")
                    '*************************meilai 25/2/05****************************
                    'strURL = "<A HREF=../FileDownload.aspx?file=" & Server.UrlEncode(strFile1) & "&actualfile=" & Server.UrlEncode(strFile) & "&type=" & EnumDownLoadType.DocAttachment & "&doctype=RFQ>" & strFile & "</A>"
                    strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DocAttachment, "WO", EnumUploadFrom.FrontOff)
                    '*************************meilai************************************
                    Dim lblBr As New Label
                    Dim lblFile As New Label
                    Dim lnk As New ImageButton
                    lblFile.Text = "&nbsp;" & intCount + 1 & ") " & strURL & " (" & drvAttach(i)("CDA_FILESIZE") & "KB) "
                    lblBr.Text = "<BR>"
                    lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete.gif")
                    lnk.ID = drvAttach(i)("CDA_ATTACH_INDEX")
                    lnk.CausesValidation = False
                    AddHandler lnk.Click, AddressOf deleteAttach

                    pnlAttach.Controls.Add(lblFile)
                    pnlAttach.Controls.Add(lnk)
                    pnlAttach.Controls.Add(lblBr)

                    intCount = intCount + 1
                End If
            Next
        Else
            Dim lblFile As New Label
            lblFile.Text = "No Files Attached"
            pnlAttach.Controls.Add(lblFile)
        End If
    End Sub

    Private Sub deleteAttach(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        objINV.deleteWOAttachment(CType(sender, ImageButton).ID)
        displayAttachFile()

    End Sub
End Class
