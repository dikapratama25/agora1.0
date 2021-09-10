Imports AgoraLegacy
Imports eProcure.Component

Public Class MRSLotMaster
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strGRNType As String
    Dim objInv As New Inventory
    Dim objGRN_Ext As New GRN_Ext
    Dim objGlO As New AppGlobals
    Protected WithEvents cmd_Save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Add As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Close As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblMRSNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblItemCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblItemName As System.Web.UI.WebControls.Label
    Protected WithEvents lblIssueQty As System.Web.UI.WebControls.Label
    Protected WithEvents lblBalQty As System.Web.UI.WebControls.Label

    Protected WithEvents hidButton As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidButtonClose As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidButtonClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidItemLine As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents hidLbl As System.Web.UI.WebControls.Label

    Dim strPONo As String
    Dim strMsg As String = ""
    Dim arySetMRSLot As New ArrayList()
    Dim intNumeric As Boolean = True
    Dim intNumeric2 As Boolean = True
    Dim intEmpty As Boolean = True
    Dim intEmpty2 As Boolean = True
    Dim intEmpty3 As Boolean = True
    Dim intDup As Boolean = True
    Dim decTotal As Decimal 'Integer
    Dim LocDesc As String = ""
    Dim SubLocDesc As String = ""
    Dim LDesc As String = ""
    Dim SubLDesc As String = ""

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
    Public Enum EnumLocation
        icLocation = 0
        icSubLocation = 1
        icQty = 2
    End Enum

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblMRSNo.Text = IIf(Common.parseNull(Request.QueryString("mrsno")) = "", "", Request.QueryString("mrsno"))
        lblItemCode.Text = IIf(Common.parseNull(Request.QueryString("itemcode")) = "", "", Request.QueryString("itemcode"))
        lblItemName.Text = IIf(Common.parseNull(Request.QueryString("itemname")) = "", "", Request.QueryString("itemname"))
        lblIssueQty.Text = IIf(Common.parseNull(Request.QueryString("iqty")) = "", 0, Request.QueryString("iqty"))
        hidItemLine.Value = Request.QueryString("itemline")
        ' Title
        objINV.GetLocationDesc(LocDesc, SubLocDesc)
        LocDesc = IIf(LocDesc = "", "Location", "" + LocDesc + "")
        SubLocDesc = IIf(SubLocDesc = "", "Sub Location", "" + SubLocDesc + "")

        'lblBalQty.Text = 0
        Dim checkfirst As Boolean = False
        ' Build Default Location Item
        'objINV.GetDefaultLocationDesc(LDesc, SubLDesc)

        If Not Page.IsPostBack Then

            lblBalQty.Text = Request.QueryString("iqty")
            ViewState("Row") = 5
            BuildRow()
            ReCal()
            ConstructTable()

        End If

        lblMsg.Text = ""
    End Sub

    Private Sub cmd_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Add.Click
        Rebuild(False)
        BuildLine()
        ConstructTable()
    End Sub

    Private Sub hidButton_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidButton.ServerClick
        Rebuild(False)
        ConstructTable()
    End Sub

    Private Sub hidButtonClose_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidButtonClose.ServerClick
        RebuildClose()
    End Sub

    Private Sub hidButtonClear_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidButtonClear.ServerClick
        Clear()
    End Sub

    Private Sub cmd_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Save.Click
        Rebuild()

        If decTotal <= Request.QueryString("iqty") Then
            lblBalQty.Text = Request.QueryString("iqty") - decTotal
        Else
            lblBalQty.Text = Request.QueryString("iqty")
        End If

        If intNumeric = False Then
            strMsg = "<ul type='disc'><li>" & "Qty " & objGLO.GetErrorMessage("00342") & "<ul type='disc'></ul></li></ul>"
        End If

        If intNumeric2 = False Then
            strMsg = "<ul type='disc'><li>" & "Qty cannot be more than stock balance.<ul type='disc'></ul></li></ul>"
        End If

        'If decTotal > Request.QueryString("iqty") Then
        '    strMsg = "<ul type='disc'><li>" & objGlO.GetErrorMessage("00012") & "<ul type='disc'></ul></li></ul>"
        'End If

        If decTotal <> Request.QueryString("iqty") Then
            strMsg = "<ul type='disc'><li>Balance Qty must be equal to 0.<ul type='disc'></ul></li></ul>"
        End If

        If intEmpty = False Then
            strMsg = "<ul type='disc'><li>Lot No " & objGlO.GetErrorMessage("00001") & "<ul type='disc'></ul></li></ul>"
        End If

        If intEmpty2 = False Then
            strMsg = "<ul type='disc'><li>" & objGlO.GetErrorMessage("00019") & "<ul type='disc'></ul></li></ul>"
        End If

        If intEmpty3 = False Then
            strMsg = "<ul type='disc'><li>Please select sub location.<ul type='disc'></ul></li></ul>"
        End If

        If intDup = False Then
            strMsg = "<ul type='disc'><li>" & objGlO.GetErrorMessage("00002") & "<ul type='disc'></ul></li></ul>"
        End If

        If strMsg <> "" Then
            lblMsg.Text = strMsg
            ConstructTable()
            Exit Sub
        Else
            lblMsg.Text = ""
        End If

        BuildSave()
        ConstructTable()
        Common.NetMsgbox(Me, objGLO.GetErrorMessage("00003"), MsgBoxStyle.Information)
    End Sub

    Private Function ConstructTable()
        Dim strrow, strExpiry As String
        Dim i, h, j, count As Integer
        Dim decStkQty As Decimal
        Dim table As String
        Dim item2nd As Boolean = False
        Dim found As Boolean = False
        Dim arySetMRSLot_temp As New ArrayList
        Dim strLot, strLoc, strSubLoc As String
        Dim c As Integer
        Dim blnLoc, blnSubLoc As Boolean
        Dim dsLot, dsLoc, dsSub As DataSet
        dsLot = objInv.PopLotNo(Request.QueryString("itemcode"))
        dsLot = objInv.ChkDupLotNo(dsLot)
        arySetMRSLot = Session("arySetMRSLot")
        count = arySetMRSLot.Count

        For i = 0 To count - 1
            If arySetMRSLot(i)(4) <> Request.QueryString("itemcode") And arySetMRSLot(i)(5) <> Request.QueryString("itemline") Then
                Continue For
            End If

            strLot = ""
            strLoc = ""
            strSubLoc = ""

            strrow &= "<tr style=""background-color:#fdfdfd;"">"
            strrow &= "<td>"
            strrow &= "<select class=""ddl"" style=""width:100%; margin-right:0px;"" onchange =""onClick()"" id=""cboLot" & i & """ name=""cboLot" & i & """>"
            strrow &= "<option value=""---Select---"" selected=""selected"">" & "---Select---" & "</option>"

            For c = 0 To dsLot.Tables(0).Rows.Count - 1
                If arySetMRSLot(i)(0) = dsLot.Tables(0).Rows(c).Item(0).ToString Then
                    strrow &= "<option value=""" & dsLot.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & dsLot.Tables(0).Rows(c).Item(1).ToString & "</option>"
                    dsLoc = objInv.PopLocByLot(dsLot.Tables(0).Rows(c).Item(0).ToString, Request.QueryString("itemcode"))
                    strExpiry = objInv.getLotExpiryDt(dsLot.Tables(0).Rows(c).Item(0).ToString)
                    strLot = dsLot.Tables(0).Rows(c).Item(0).ToString
                Else
                    strrow &= "<option value=""" & dsLot.Tables(0).Rows(c).Item(0).ToString & """>" & dsLot.Tables(0).Rows(c).Item(1).ToString & "</option>"
                End If
            Next

            strrow &= "</select>"
            strrow &= "</td>"

            strrow &= "<td>"
            strrow &= "<label id=""lblExpiry" & i & """ style=""width:100%;margin-right:0px; "">" & strExpiry & "</label>"
            strrow &= "</td>"

            strrow &= "<td>"
            strrow &= "<input style=""width:100%;margin-right:0px; "" onkeypress=""return isNumberKey(event)"" class=""numerictxtbox2"" type=""text"" id=""txtQty" & i & """ name=""txtQty" & i & """ value=""" & arySetMRSLot(i)(1) & """>"
            strrow &= "</td>"

            strrow &= "<td>"
            strrow &= "<select class=""ddl"" style=""width:100%; margin-right:0px;"" onchange =""onClick()"" id=""cboLocation" & i & """ name=""cboLocation" & i & """>"
            strrow &= "<option value=""---Select---"" selected=""selected"">" & "---Select---" & "</option>"

            If Not dsLoc Is Nothing Then
                If dsLoc.Tables(0).Rows.Count > 0 Then
                    blnLoc = False
                    For c = 0 To dsLoc.Tables(0).Rows.Count - 1
                        If arySetMRSLot(i)(2) = dsLoc.Tables(0).Rows(c).Item(0).ToString Then
                            strrow &= "<option value=""" & dsLoc.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & dsLoc.Tables(0).Rows(c).Item(0).ToString & "</option>"
                            dsSub = objInv.PopLocByLot(strLot, Request.QueryString("itemcode"), dsLoc.Tables(0).Rows(c).Item(0).ToString)
                            strLoc = dsLoc.Tables(0).Rows(c).Item(0).ToString
                            blnLoc = True
                        Else
                            strrow &= "<option value=""" & dsLoc.Tables(0).Rows(c).Item(0).ToString & """>" & dsLoc.Tables(0).Rows(c).Item(0).ToString & "</option>"
                        End If
                    Next

                    If blnLoc = False Then
                        strLoc = ""
                    End If
                End If
            End If

            strrow &= "</select>"
            strrow &= "</td>"

            strrow &= "<td>"
            strrow &= "<select class=""ddl"" style=""width:100%; margin-right:0px;"" onchange =""onClick()"" id=""cboSubLocation" & i & """ name=""cboSubLocation" & i & """>"

            If Not dsSub Is Nothing Then
                If dsSub.Tables(0).Rows.Count = 0 Then
                    strrow &= "<option value=""---Select---"" selected=""selected"">" & "---Select---" & "</option>"
                    strSubLoc = "---Select---"
                ElseIf dsSub.Tables(0).Rows.Count = 1 Then
                    strrow &= "<option value=""" & dsSub.Tables(0).Rows(0).Item(0).ToString & """ selected=""selected"">" & dsSub.Tables(0).Rows(0).Item(0).ToString & "</option>"
                    strSubLoc = dsSub.Tables(0).Rows(0).Item(0).ToString
                ElseIf dsSub.Tables(0).Rows.Count > 1 Then
                    blnSubLoc = False
                    For c = 0 To dsSub.Tables(0).Rows.Count - 1
                        If arySetMRSLot(i)(3) = dsSub.Tables(0).Rows(c).Item(0).ToString Then
                            strrow &= "<option value=""" & dsSub.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & dsSub.Tables(0).Rows(c).Item(0).ToString & "</option>"
                            strSubLoc = dsSub.Tables(0).Rows(c).Item(0).ToString
                            blnSubLoc = True
                        Else
                            strrow &= "<option value=""" & dsSub.Tables(0).Rows(c).Item(0).ToString & """>" & dsSub.Tables(0).Rows(c).Item(0).ToString & "</option>"
                        End If
                    Next

                    If blnSubLoc = False Then
                        strSubLoc = dsSub.Tables(0).Rows(0).Item(0).ToString
                        'strSubLoc = "---Select---"
                    End If
                End If
            Else
                strrow &= "<option value=""---Select---"" selected=""selected"">" & "---Select---" & "</option>"
                strSubLoc = "---Select---"
            End If

            strrow &= "</select>"
            strrow &= "</td>"

            If (strLot <> "" And strLoc <> "" And strSubLoc <> "---Select---") Then
                decStkQty = objInv.getLotBalance(Request.QueryString("itemcode"), strLot, strLoc, strSubLoc)
                strrow &= "<td>"
                strrow &= "<label id=""lblStock" & i & """ style=""width:100%;margin-right:0px; "" class=""numerictxtbox"" >" & decStkQty & "</label>"
                strrow &= "</td>"
            Else
                strrow &= "<td>"
                strrow &= "<label id=""lblStock" & i & """ style=""width:100%;margin-right:0px; "" class=""numerictxtbox"" ></label>"
                strrow &= "</td>"
            End If



            strrow &= "</tr>"

            strExpiry = ""
            dsLoc = Nothing
            dsSub = Nothing
        Next
    
        table = "<table class=""grid"" style=""margin-top:10px; border-collapse:collapse; line-height:20px; "">" & _
                "<tr class=""TableHeader""><td style=""width:330;"">Lot No</td><td style=""width:180px;"">Expiry Date</td><td style=""width:130px;"">Qty</td><td style=""width:180px;"">" & LocDesc & "</td><td style=""width:180px;"">" & SubLocDesc & "</td><td style=""width:130px;"">Stock Balance</td></tr>" & _
                strrow & _
                "</table>"

        Session("ConstructTable") = table

    End Function

    Private Function ReCal()
        Dim i As Integer
        Dim found As Integer
        found = 0
        intNumeric = True
        intEmpty = True
        decTotal = 0

        arySetMRSLot = Session("arySetMRSLot")
        For i = 0 To arySetMRSLot.Count - 1
            If arySetMRSLot(i)(4) = Request.QueryString("itemcode") And arySetMRSLot(i)(5) = Request.QueryString("itemline") Then
                If (Regex.IsMatch(Trim(arySetMRSLot(i)(1)), "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$")) = False And arySetMRSLot(i)(1) <> "" Then
                    'intNumeric = False
                Else
                    decTotal = decTotal + CDec(IIf(arySetMRSLot(i)(1) = "", 0, arySetMRSLot(i)(1)))
                End If
            End If
        Next
        
        'For i = 0 To arySetMRSLot.Count - 1
        '    If arySetMRSLot(i)(4) = Request.QueryString("item") And arySetMRSLot(i)(5) = Request.QueryString("itemline") Then

        '        Dim decCheck As Decimal 'Integer
        '        decCheck = IsNumeric(arySetMRSLot(i)(2))
        '        If (IsNumeric(arySetMRSLot(i)(1)) = False) And arySetMRSLot(i)(1) <> "" Then
        '            intNumeric = False
        '        Else
        '            decTotal = decTotal + CDec(IIf(arySetMRSLot(i)(1) = "", 0, arySetMRSLot(i)(1)))
        '        End If

        '        If arySetMRSLot(i)(0) = "---Select---" And Request.Form("txtQty" & i) <> "" Then
        '            intEmpty = False
        '        End If

        '        If arySetMRSLot(i)(2) = "---Select---" And arySetMRSLot(i)(0) <> "---Select---" Then
        '            intEmpty2 = False
        '        End If

        '        If arySetMRSLot(i)(3) = "---Select---" And arySetMRSLot(i)(2) <> "---Select---" Then
        '            intEmpty3 = False
        '        End If
        '    End If

        'Next

        If decTotal <= CDec(Request.QueryString("iqty")) Then
            lblBalQty.Text = CDec(Request.QueryString("iqty")) - decTotal
        Else
            lblBalQty.Text = CDec(Request.QueryString("iqty"))
        End If

        'Session("arySetMRSLot") = Nothing
        'Session("arySetMRSLot") = arySetMRSLot
    End Function

    Private Function RebuildClose()
        Dim Loc As String
        Dim iCheck As Integer
        Clear()

        Loc = objINV.GetDafaultLocation("", "")

        If Session("blnGRNTrue") = True Then
            If CDec(lblBalQty.Text) <> 0 Then
                'Dim vbs As String
                'vbs = vbs & "<script language=""vbs"">"
                'vbs = vbs & "result = MsgBox(""" & objGLO.GetErrorMessage("00011") & """, " & MsgBoxStyle.YesNo & ")"
                'vbs = vbs & vbLf & "document.getElementById(""hidLbl"").innerHTML= result"
                'vbs = vbs & "</script>"
                'Dim rndKey As New Random
                'Me.RegisterStartupScript(rndKey.Next.ToString, vbs)

                Dim strscript As New System.Text.StringBuilder
                Dim strFileName As String
                strscript.Append("<script language=""javascript"">")
                strscript.Append("Close(""" & Loc & """);")
                strscript.Append("</script>")
                RegisterStartupScript("script3", strscript.ToString())

                'If Loc = "" And CInt(lblBalQty.Text) <> 0 Then
                '    Common.NetMsgbox(Me, objGLO.GetErrorMessage("00010"), MsgBoxStyle.Information)
                'End If
            Else
                Dim strscript As New System.Text.StringBuilder
                Dim strFileName As String
                strscript.Append("<script language=""javascript"">")
                strscript.Append("Close2(""" & Loc & """);")
                strscript.Append("</script>")
                RegisterStartupScript("script3", strscript.ToString())
            End If
        Else
            Dim strscript As New System.Text.StringBuilder
            Dim strFileName As String
            strscript.Append("<script language=""javascript"">")
            strscript.Append("Close2(""" & Loc & """);")
            strscript.Append("</script>")
            RegisterStartupScript("script3", strscript.ToString())
        End If


    End Function

    Private Function Clear()
        Dim i As Integer
        Dim found As Integer
        found = 0

        If Not Session("arySetMRSLot") Is Nothing Then
            arySetMRSLot = Session("arySetMRSLot")

            Do While i < arySetMRSLot.Count
                If arySetMRSLot(i)(4) = lblItemCode.Text And arySetMRSLot(i)(5) = hidItemLine.Value And arySetMRSLot(i)(6) = "" Then
                    arySetMRSLot.RemoveAt(i)
                    i = 0
                Else
                    i = i + 1
                End If
            Loop

            For i = 0 To arySetMRSLot.Count - 1
                If arySetMRSLot(i)(4) = lblItemCode.Text And arySetMRSLot(i)(5) = hidItemLine.Value And arySetMRSLot(i)(6) = "Clear" Then
                    arySetMRSLot(i)(0) = arySetMRSLot(i)(7)
                    arySetMRSLot(i)(1) = arySetMRSLot(i)(8)
                    arySetMRSLot(i)(2) = arySetMRSLot(i)(9)
                    arySetMRSLot(i)(3) = arySetMRSLot(i)(10)

                    arySetMRSLot(i)(6) = "Save"
                End If
            Next

            Session("arySetMRSLot") = Nothing
            Session("arySetMRSLot") = arySetMRSLot
        End If
    End Function

    Private Function BuildSave()
        Dim i As Integer
        Dim found As Integer
        found = 0

        arySetMRSLot = Session("arySetMRSLot")
        For i = 0 To arySetMRSLot.Count - 1
            If arySetMRSLot(i)(4) = Request.QueryString("itemcode") And arySetMRSLot(i)(5) = Request.QueryString("itemline") Then
                arySetMRSLot(i)(6) = "Save"
            End If
        Next

        Session("arySetMRSLot") = Nothing
        Session("arySetMRSLot") = arySetMRSLot
    End Function

    Private Function Rebuild(Optional ByVal blnChk As Boolean = True)
        Dim i, j As Integer
        Dim found As Integer
        found = 0
        intNumeric = True
        intNumeric2 = True
        intEmpty = True
        intEmpty2 = True
        intEmpty3 = True
        intDup = True
        decTotal = 0
        Dim decStkQty As Decimal = 0

        arySetMRSLot = Session("arySetMRSLot")
        For i = 0 To arySetMRSLot.Count - 1
            If arySetMRSLot(i)(4) = Request.QueryString("itemcode") And arySetMRSLot(i)(5) = Request.QueryString("itemline") Then
                arySetMRSLot(i)(7) = arySetMRSLot(i)(0)
                arySetMRSLot(i)(8) = arySetMRSLot(i)(1)
                arySetMRSLot(i)(9) = arySetMRSLot(i)(2)
                arySetMRSLot(i)(10) = arySetMRSLot(i)(3)

                arySetMRSLot(i)(0) = Request.Form("cboLot" & i)
                arySetMRSLot(i)(1) = Request.Form("txtQty" & i)
                arySetMRSLot(i)(2) = Request.Form("cboLocation" & i)
                arySetMRSLot(i)(3) = Request.Form("cboSubLocation" & i)
                arySetMRSLot(i)(4) = Request.QueryString("itemcode")
                arySetMRSLot(i)(5) = Request.QueryString("itemline")

                arySetMRSLot(i)(6) = "Clear"

                Dim iCheck As Decimal
                iCheck = IsNumeric(Request.Form("txtQty" & i))
                If (Regex.IsMatch(Trim(Request.Form("txtQty" & i)), "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$")) = False And Request.Form("txtQty" & i) <> "" Then
                    intNumeric = False
                Else
                    decTotal = decTotal + CDec(IIf(Request.Form("txtQty" & i) = "", 0, Request.Form("txtQty" & i)))
                End If

                If arySetMRSLot(i)(0) = "---Select---" And Request.Form("txtQty" & i) <> "" Then
                    intEmpty = False
                End If

                If arySetMRSLot(i)(2) = "---Select---" And arySetMRSLot(i)(0) <> "---Select---" Then
                    intEmpty2 = False
                End If

                If arySetMRSLot(i)(3) = "---Select---" And arySetMRSLot(i)(2) <> "---Select---" Then
                    intEmpty3 = False
                End If

            End If
        Next

        If intEmpty = True And intEmpty2 = True And intEmpty3 = True Then
            For i = 0 To arySetMRSLot.Count - 1
                If arySetMRSLot(i)(4) = Request.QueryString("itemcode") And arySetMRSLot(i)(5) = Request.QueryString("itemline") And arySetMRSLot(i)(0) <> "---Select---" Then
                    For j = 0 To arySetMRSLot.Count - 1
                        If arySetMRSLot(j)(4) = Request.QueryString("itemcode") And arySetMRSLot(j)(5) = Request.QueryString("itemline") And arySetMRSLot(j)(0) <> "---Select---" And i <> j Then
                            If arySetMRSLot(i)(0) = arySetMRSLot(j)(0) And arySetMRSLot(i)(2) = arySetMRSLot(j)(2) And arySetMRSLot(i)(3) = arySetMRSLot(j)(3) Then
                                intDup = False
                                Exit For
                            End If
                        End If
                    Next
                End If

                If intDup = False Then
                    Exit For
                End If
            Next

            If blnChk = True Then
                For i = 0 To arySetMRSLot.Count - 1
                    If arySetMRSLot(i)(4) = Request.QueryString("itemcode") And arySetMRSLot(i)(5) = Request.QueryString("itemline") And arySetMRSLot(i)(0) <> "---Select---" Then
                        decStkQty = objInv.getLotBalance(Request.QueryString("itemcode"), arySetMRSLot(i)(0), arySetMRSLot(i)(2), arySetMRSLot(i)(3))
                        If CDec(IIf(arySetMRSLot(i)(1) = "", 0, arySetMRSLot(i)(1))) > decStkQty Then
                            intNumeric2 = False
                        End If
                    End If
                Next
            End If
        End If

        Session("arySetMRSLot") = Nothing
        Session("arySetMRSLot") = arySetMRSLot
    End Function

    Private Function BuildLine()
        Dim i As Integer
        Dim found As Integer
        found = 0

        arySetMRSLot = Session("arySetMRSLot")
        For i = 0 To arySetMRSLot.Count - 1
            If arySetMRSLot(i)(4) = Request.QueryString("itemcode") And arySetMRSLot(i)(5) = Request.QueryString("itemline") Then
                found = found + 1
            End If
        Next

        arySetMRSLot.Add(New String() {Request.Form("cboLot" & found + 1), Request.Form("txtQty" & found + 1), Request.Form("cboLocation" & found + 1), Request.Form("cboSubLocation" & found + 1), Request.QueryString("itemcode"), Request.QueryString("itemline"), "Save", "", "", "", ""})

        Session("arySetMRSLot") = Nothing
        Session("arySetMRSLot") = arySetMRSLot
    End Function

    Private Function BuildRow()
        Dim i As Integer
        Dim found As Boolean
        found = False

        If Not Session("arySetMRSLot") Is Nothing Then 'Empty
            arySetMRSLot = Session("arySetMRSLot")
        End If

        If Session("arySetMRSLot") Is Nothing Then 'Empty
            For i = 0 To ViewState("Row") - 1
                arySetMRSLot.Add(New String() {Request.Form("cboLot" & i), Request.Form("txtQty" & i), Request.Form("cboLocation" & i), Request.Form("cboSubLocation" & i), Request.QueryString("itemcode"), Request.QueryString("itemline"), "Save", "", "", "", ""})
            Next

            Session("arySetMRSLot") = arySetMRSLot
        Else ' Session Not Empty 1.Reload   2.After Add Line
            arySetMRSLot = Session("arySetMRSLot")
            For i = 0 To arySetMRSLot.Count - 1
                If arySetMRSLot(i)(4) = Request.QueryString("itemcode") And arySetMRSLot(i)(5) = Request.QueryString("itemline") Then
                    found = True
                    Exit For
                End If
            Next

            If found = False Then

                For i = 0 To ViewState("Row") - 1
                    arySetMRSLot.Add(New String() {Request.Form("cboLot" & i), Request.Form("txtQty" & i), Request.Form("cboLocation" & i), Request.Form("cboSubLocation" & i), Request.QueryString("itemcode"), Request.QueryString("itemline"), "Save", "", "", "", ""})
                Next

                Session("arySetMRSLot") = Nothing
            Else ' If found then update the Default QTY
                'ReCal()
                'For i = 0 To arySetMRSLot.Count - 1
                '    If arySetMRSLot(i)(4) = Request.QueryString("itemcode") And arySetMRSLot(i)(0) = LDesc And arySetMRSLot(i)(1) = SubLDesc Then
                '        arySetMRSLot(i)(2) = Request.QueryString("iqty") - IIf(Common.parseNull(Request.QueryString("rjqty")) = "", 0, Request.QueryString("rjqty")) - (decTotal - arySetMRSLot(i)(2))
                '        Exit For
                '    End If
                'Next
                'ReCal()
            End If
            Session("arySetMRSLot") = arySetMRSLot
        End If
    End Function

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Clear()
    End Sub
End Class
