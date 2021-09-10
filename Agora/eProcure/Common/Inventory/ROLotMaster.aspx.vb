Imports AgoraLegacy
Imports eProcure.Component

Public Class ROLotMaster
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strGRNType As String
    Dim objInv As New Inventory
    Dim objGRN_Ext As New GRN_Ext
    Dim objGlO As New AppGlobals
    Protected WithEvents cmd_Save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Add As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Close As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblItemCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblItemName As System.Web.UI.WebControls.Label
    Protected WithEvents lblRemainQty As System.Web.UI.WebControls.Label

    Protected WithEvents hidButton As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidButtonClose As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidButtonClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidItemLine As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents hidLbl As System.Web.UI.WebControls.Label

    Dim strPONo As String
    Dim strMsg As String = ""
    Dim arySetROLot As New ArrayList()
    Dim intNumeric As Boolean = True
    Dim intNumeric2 As Boolean = True
    Dim intNumeric3 As Boolean = True
    Dim intNumeric4 As Boolean = True
    Dim intNumeric5 As Boolean = True
    Dim intEmpty As Boolean = True
    Dim intEmpty2 As Boolean = True
    Dim intEmpty3 As Boolean = True
    Dim intEmpty4 As Boolean = True
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

        lblItemCode.Text = IIf(Common.parseNull(Request.QueryString("itemcode")) = "", "", Request.QueryString("itemcode"))
        lblItemName.Text = IIf(Common.parseNull(Request.QueryString("itemname")) = "", "", Request.QueryString("itemname"))
        hidItemLine.Value = Request.QueryString("itemline")
        lblRemainQty.Text = Request.QueryString("rqty")
        ViewState("grnno") = Request.QueryString("grnno")
        ViewState("pono") = Request.QueryString("pono")
        ViewState("poline") = Request.QueryString("poline")
        ViewState("qc") = Request.QueryString("qc")

        ' Title
        objInv.GetLocationDesc(LocDesc, SubLocDesc)
        LocDesc = IIf(LocDesc = "", "Location", "" + LocDesc + "")
        SubLocDesc = IIf(SubLocDesc = "", "Sub Location", "" + SubLocDesc + "")

        Dim checkfirst As Boolean = False
        ' Build Default Location Item
        'objINV.GetDefaultLocationDesc(LDesc, SubLDesc)

        If Not Page.IsPostBack Then

            ViewState("Row") = 5
            BuildRow()
            'ReCal()
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

        If decTotal > CDec(Request.QueryString("rqty")) Then
            strMsg = "<ul type='disc'><li>Total of return qty cannot be more than remaining qty.<ul type='disc'></ul></li></ul>"
        End If

        If intNumeric = False Then
            strMsg = "<ul type='disc'><li>Qty " & objGlO.GetErrorMessage("00342") & "<ul type='disc'></ul></li></ul>"
        End If

        If intNumeric2 = False Then
            strMsg = "<ul type='disc'><li>Return qty cannot be more than remaining qty.<ul type='disc'></ul></li></ul>"
        End If

        If intNumeric3 = False Then
            strMsg = "<ul type='disc'><li>Return qty cannot be more than stock balance.<ul type='disc'></ul></li></ul>"
        End If

        If intNumeric4 = False Then
            strMsg = "<ul type='disc'><li>Return qty cannot be more than IQC Approved qty or IQC Rejected qty.<ul type='disc'></ul></li></ul>"
        End If

        If intNumeric5 = False Then
            strMsg = "<ul type='disc'><li>Return qty cannot be more than IQC Approved qty or IQC Rejected qty.<ul type='disc'></ul></li></ul>"
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

        If intEmpty4 = False Then
            strMsg = "<ul type='disc'><li>Return qty " & objGlO.GetErrorMessage("00001") & "<ul type='disc'></ul></li></ul>"
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

        ReCal()
        BuildSave()
        ConstructTable()
        Common.NetMsgbox(Me, objGLO.GetErrorMessage("00003"), MsgBoxStyle.Information)
    End Sub

    Private Function ConstructTable()
        Dim strrow As String
        Dim i, h, j, count As Integer
        Dim decReqQty, decStkQty, decReQty, decIQCQty, decRejIQCQty As Decimal
        Dim table As String
        Dim item2nd As Boolean = False
        Dim found As Boolean = False
        Dim arySetROLot_temp As New ArrayList
        Dim strLot, strLoc, strSubLoc As String
        Dim blnLoc, blnSubLoc As Boolean
        Dim c As Integer
        Dim dsLot, dsLoc, dsSub, dsQty As DataSet
        'dsLot = objInv.PopLotNo(Request.QueryString("itemcode"))
        dsLot = objInv.PopLotInfoByRO(ViewState("grnno"), ViewState("pono"), ViewState("poline"), lblItemCode.Text, , , , "GL_LOT_INDEX")

        arySetROLot = Session("arySetROLot")
        count = arySetROLot.Count

        For i = 0 To count - 1
            'If arySetROLot(i)(4) <> Request.QueryString("itemcode") And arySetROLot(i)(5) <> Request.QueryString("itemline") Then
            '    Continue For
            'End If

            If arySetROLot(i)(4) = Request.QueryString("itemcode") And arySetROLot(i)(5) = Request.QueryString("itemline") Then

                strLot = ""
                strLoc = ""
                strSubLoc = ""

                strrow &= "<tr style=""background-color:#fdfdfd;"">"
                strrow &= "<td>"
                strrow &= "<input style=""width:100%;margin-right:0px; "" onkeypress=""return isNumberKey(event)"" class=""numerictxtbox2"" type=""text"" id=""txtQty" & i & """ name=""txtQty" & i & """ value=""" & arySetROLot(i)(1) & """>"
                strrow &= "</td>"

                strrow &= "<td>"
                strrow &= "<select class=""ddl"" style=""width:100%; margin-right:0px;"" onchange =""onClick()"" id=""cboLot" & i & """ name=""cboLot" & i & """>"
                strrow &= "<option value=""---Select---"" selected=""selected"">" & "---Select---" & "</option>"

                For c = 0 To dsLot.Tables(0).Rows.Count - 1
                    If arySetROLot(i)(0) = dsLot.Tables(0).Rows(c).Item("GL_LOT_INDEX").ToString Then
                        strrow &= "<option value=""" & dsLot.Tables(0).Rows(c).Item("GL_LOT_INDEX").ToString & """ selected=""selected"">" & dsLot.Tables(0).Rows(c).Item("DOL_LOT_NO").ToString & "</option>"
                        'dsLoc = objInv.PopLocByLot(dsLot.Tables(0).Rows(c).Item(0).ToString, Request.QueryString("itemcode"))
                        dsLoc = objInv.PopLotInfoByRO(ViewState("grnno"), ViewState("pono"), ViewState("poline"), lblItemCode.Text, dsLot.Tables(0).Rows(c).Item("GL_LOT_INDEX").ToString, , , "LM_LOCATION")
                        strLot = dsLot.Tables(0).Rows(c).Item("GL_LOT_INDEX").ToString
                    Else
                        strrow &= "<option value=""" & dsLot.Tables(0).Rows(c).Item("GL_LOT_INDEX").ToString & """>" & dsLot.Tables(0).Rows(c).Item("DOL_LOT_NO").ToString & "</option>"
                    End If
                Next

                strrow &= "</select>"
                strrow &= "</td>"

                strrow &= "<td>"
                strrow &= "<select class=""ddl"" style=""width:100%; margin-right:0px;"" onchange =""onClick()"" id=""cboLocation" & i & """ name=""cboLocation" & i & """>"
                strrow &= "<option value=""---Select---"" selected=""selected"">" & "---Select---" & "</option>"

                If Not dsLoc Is Nothing Then
                    If dsLoc.Tables(0).Rows.Count > 0 Then
                        For c = 0 To dsLoc.Tables(0).Rows.Count - 1
                            If arySetROLot(i)(2) = dsLoc.Tables(0).Rows(c).Item("LM_LOCATION").ToString Then
                                strrow &= "<option value=""" & dsLoc.Tables(0).Rows(c).Item("LM_LOCATION").ToString & """ selected=""selected"">" & dsLoc.Tables(0).Rows(c).Item("LM_LOCATION").ToString & "</option>"
                                dsSub = objInv.PopLotInfoByRO(ViewState("grnno"), ViewState("pono"), ViewState("poline"), lblItemCode.Text, strLot, dsLoc.Tables(0).Rows(c).Item("LM_LOCATION").ToString, , "LM_SUB_LOCATION")
                                strLoc = dsLoc.Tables(0).Rows(c).Item("LM_LOCATION").ToString
                            Else
                                strrow &= "<option value=""" & dsLoc.Tables(0).Rows(c).Item("LM_LOCATION").ToString & """>" & dsLoc.Tables(0).Rows(c).Item("LM_LOCATION").ToString & "</option>"
                            End If
                        Next
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
                        strrow &= "<option value=""" & dsSub.Tables(0).Rows(0).Item("LM_SUB_LOCATION").ToString & """ selected=""selected"">" & dsSub.Tables(0).Rows(0).Item("LM_SUB_LOCATION").ToString & "</option>"
                        strSubLoc = dsSub.Tables(0).Rows(0).Item("LM_SUB_LOCATION").ToString
                    ElseIf dsSub.Tables(0).Rows.Count > 1 Then
                        blnSubLoc = False
                        For c = 0 To dsSub.Tables(0).Rows.Count - 1
                            If arySetROLot(i)(3) = dsSub.Tables(0).Rows(c).Item("LM_SUB_LOCATION").ToString Then
                                strrow &= "<option value=""" & dsSub.Tables(0).Rows(c).Item("LM_SUB_LOCATION").ToString & """ selected=""selected"">" & dsSub.Tables(0).Rows(c).Item("LM_SUB_LOCATION").ToString & "</option>"
                                strSubLoc = dsSub.Tables(0).Rows(c).Item("LM_SUB_LOCATION").ToString
                                blnSubLoc = True
                            Else
                                strrow &= "<option value=""" & dsSub.Tables(0).Rows(c).Item("LM_SUB_LOCATION").ToString & """>" & dsSub.Tables(0).Rows(c).Item("LM_SUB_LOCATION").ToString & "</option>"
                            End If
                        Next

                        If blnSubLoc = False Then
                            strSubLoc = dsSub.Tables(0).Rows(0).Item("LM_SUB_LOCATION").ToString
                            'strSubLoc = "---Select---"
                        End If
                    End If
                Else
                    strrow &= "<option value=""---Select---"" selected=""selected"">" & "---Select---" & "</option>"
                    strSubLoc = "---Select---"
                End If

                strrow &= "</select>"
                strrow &= "</td>"

                If strLot <> "" And strLoc <> "" And strSubLoc <> "---Select---" Then
                    If ViewState("qc") = "Y" Then
                        decIQCQty = objInv.chkIQCLotQty(ViewState("grnno"), lblItemCode.Text, , strLot, strLoc, strSubLoc, , True)
                        decRejIQCQty = objInv.chkIQCLotQty(ViewState("grnno"), lblItemCode.Text, , strLot, strLoc, strSubLoc, "Rej", True)

                        strrow &= "<td>"
                        strrow &= "<label id=""lblIQCQty" & i & """ style=""width:100%;margin-right:0px; "" class=""numerictxtbox"" >" & decIQCQty & "</label>"
                        strrow &= "</td>"
                        strrow &= "<td>"
                        strrow &= "<label id=""lblRejIQCQty" & i & """ style=""width:100%;margin-right:0px; "" class=""numerictxtbox"" >" & decRejIQCQty & "</label>"
                        strrow &= "</td>"
                    End If


                    decReqQty = ReCalStk(arySetROLot, strLot, strLoc, strSubLoc)

                    dsQty = objInv.PopLotInfoByRO(ViewState("grnno"), ViewState("pono"), ViewState("poline"), lblItemCode.Text, strLot, strLoc, strSubLoc)
                    decReQty = objInv.chkROLotRemainingQty(ViewState("grnno"), ViewState("poline"), strLot, strLoc, strSubLoc)

                    strrow &= "<td>"
                    strrow &= "<label id=""lblRemainQty" & i & """ style=""width:100%;margin-right:0px; "" class=""numerictxtbox"" >" & CDec(dsQty.Tables(0).Rows(0)("GL_LOT_RECEIVED_QTY")) - decReQty - decReqQty & "</label>"
                    strrow &= "</td>"

                    decStkQty = objInv.getLotBalance(lblItemCode.Text, strLot, strLoc, strSubLoc, True)

                    strrow &= "<td>"
                    strrow &= "<label id=""lblStock" & i & """ style=""width:100%;margin-right:0px; "" class=""numerictxtbox"" >" & decStkQty - decReqQty & "</label>"
                    strrow &= "</td>"


                Else
                    If ViewState("qc") = "Y" Then
                        strrow &= "<td>"
                        strrow &= "<label id=""lblIQCQty" & i & """ style=""width:100%;margin-right:0px; "" class=""numerictxtbox"" ></label>"
                        strrow &= "</td>"
                        strrow &= "<td>"
                        strrow &= "<label id=""lblRejIQCQty" & i & """ style=""width:100%;margin-right:0px; "" class=""numerictxtbox"" ></label>"
                        strrow &= "</td>"
                    End If

                    strrow &= "<td>"
                    strrow &= "<label id=""lblRemainQty" & i & """ style=""width:100%;margin-right:0px; "" class=""numerictxtbox"" ></label>"
                    strrow &= "</td>"

                    strrow &= "<td>"
                    strrow &= "<label id=""lblStock" & i & """ style=""width:100%;margin-right:0px; "" class=""numerictxtbox"" ></label>"
                    strrow &= "</td>"
                End If

                strrow &= "</tr>"

                dsLoc = Nothing
                dsSub = Nothing
                dsQty = Nothing

            End If
        Next

        If ViewState("qc") = "Y" Then
            table = "<table class=""grid"" style=""margin-top:10px; border-collapse:collapse; line-height:20px; "">" & _
                    "<tr class=""TableHeader""><td style=""width:130px;"">Return Qty</td><td style=""width:250px;"">Lot No</td><td style=""width:265px;"">" & LocDesc & "</td><td style=""width:265px;"">" & SubLocDesc & "</td><td style=""width:110px;"">IQC Approved Qty</td><td style=""width:110px;"">IQC Rejected Qty</td><td style=""width:130px;"">Remaining Qty</td><td style=""width:130px;"">Stock Balance</td></tr>" & _
                    strrow & _
                    "</table>"
        Else
            table = "<table class=""grid"" style=""margin-top:10px; border-collapse:collapse; line-height:20px; "">" & _
                    "<tr class=""TableHeader""><td style=""width:130px;"">Return Qty</td><td style=""width:250px;"">Lot No</td><td style=""width:265px;"">" & LocDesc & "</td><td style=""width:265px;"">" & SubLocDesc & "</td><td style=""width:130px;"">Remaining Qty</td><td style=""width:130px;"">Stock Balance</td></tr>" & _
                    strrow & _
                    "</table>"
        End If
        

        Session("ConstructTable") = table

    End Function

    Private Function RebuildClose()
        Dim Loc As String
        Dim iCheck As Integer
        Clear()

        'Loc = objInv.GetDafaultLocation("", "")

        'If CDec(lblBalQty.Text) <> 0 Then

        Dim strscript As New System.Text.StringBuilder
        Dim strFileName As String
        strscript.Append("<script language=""javascript"">")
        strscript.Append("Close();")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())

        'Else
        '    Dim strscript As New System.Text.StringBuilder
        '    Dim strFileName As String
        '    strscript.Append("<script language=""javascript"">")
        '    strscript.Append("Close2(""" & Loc & """);")
        '    strscript.Append("</script>")
        '    RegisterStartupScript("script3", strscript.ToString())
        'End If

    End Function

    Private Function Clear()
        Dim i As Integer
        Dim found As Integer
        found = 0

        If Not Session("arySetROLot") Is Nothing Then
            arySetROLot = Session("arySetROLot")

            Do While i < arySetROLot.Count
                If arySetROLot(i)(4) = lblItemCode.Text And arySetROLot(i)(5) = hidItemLine.Value And arySetROLot(i)(6) = "" Then
                    arySetROLot.RemoveAt(i)
                    i = 0
                Else
                    i = i + 1
                End If
            Loop

            For i = 0 To arySetROLot.Count - 1
                If arySetROLot(i)(4) = lblItemCode.Text And arySetROLot(i)(5) = hidItemLine.Value And arySetROLot(i)(6) = "Clear" Then
                    arySetROLot(i)(0) = arySetROLot(i)(7)
                    arySetROLot(i)(1) = arySetROLot(i)(8)
                    arySetROLot(i)(2) = arySetROLot(i)(9)
                    arySetROLot(i)(3) = arySetROLot(i)(10)

                    arySetROLot(i)(6) = "Save"
                End If
            Next

            Session("arySetROLot") = Nothing
            Session("arySetROLot") = arySetROLot
        End If
    End Function

    Private Function BuildSave()
        Dim i As Integer
        Dim found As Integer
        found = 0

        arySetROLot = Session("arySetROLot")
        For i = 0 To arySetROLot.Count - 1
            If arySetROLot(i)(4) = Request.QueryString("itemcode") And arySetROLot(i)(5) = Request.QueryString("itemline") Then
                arySetROLot(i)(6) = "Save"
            End If
        Next

        Session("arySetROLot") = Nothing
        Session("arySetROLot") = arySetROLot
    End Function

    Private Function Rebuild(Optional ByVal blnChk As Boolean = True)
        Dim i, j As Integer
        Dim found As Integer
        Dim dsQty As DataSet
        found = 0
        intNumeric = True
        intNumeric2 = True
        intNumeric3 = True
        intNumeric4 = True
        intNumeric5 = True
        intEmpty = True
        intEmpty2 = True
        intEmpty3 = True
        intEmpty4 = True
        intDup = True
        decTotal = 0
        Dim decStkQty, decIQCQty, decRejIQCQty As Decimal

        arySetROLot = Session("arySetROLot")
        For i = 0 To arySetROLot.Count - 1
            If arySetROLot(i)(4) = Request.QueryString("itemcode") And arySetROLot(i)(5) = Request.QueryString("itemline") Then
                arySetROLot(i)(7) = arySetROLot(i)(0)
                arySetROLot(i)(8) = arySetROLot(i)(1)
                arySetROLot(i)(9) = arySetROLot(i)(2)
                arySetROLot(i)(10) = arySetROLot(i)(3)

                arySetROLot(i)(0) = Request.Form("cboLot" & i)
                arySetROLot(i)(1) = Request.Form("txtQty" & i)
                arySetROLot(i)(2) = Request.Form("cboLocation" & i)
                arySetROLot(i)(3) = Request.Form("cboSubLocation" & i)
                arySetROLot(i)(4) = Request.QueryString("itemcode")
                arySetROLot(i)(5) = Request.QueryString("itemline")

                arySetROLot(i)(6) = "Clear"

                Dim iCheck As Decimal
                iCheck = IsNumeric(Request.Form("txtQty" & i))
                If (Regex.IsMatch(Trim(Request.Form("txtQty" & i)), "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$")) = False And Request.Form("txtQty" & i) <> "" Then
                    intNumeric = False
                Else
                    decTotal = decTotal + CDec(IIf(Request.Form("txtQty" & i) = "", 0, Request.Form("txtQty" & i)))
                End If

                If arySetROLot(i)(0) = "---Select---" And Request.Form("txtQty" & i) <> "" Then
                    intEmpty = False
                End If

                If arySetROLot(i)(2) = "---Select---" And arySetROLot(i)(0) <> "---Select---" Then
                    intEmpty2 = False
                End If

                If arySetROLot(i)(3) = "---Select---" And arySetROLot(i)(2) <> "---Select---" Then
                    intEmpty3 = False
                End If

                If Request.Form("txtQty" & i) = "" And arySetROLot(i)(0) <> "---Select---" And arySetROLot(i)(2) <> "---Select---" And arySetROLot(i)(3) <> "---Select---" Then
                    intEmpty4 = False
                End If

            End If
        Next

        If intEmpty = True And intEmpty2 = True And intEmpty3 = True Then
            For i = 0 To arySetROLot.Count - 1
                If arySetROLot(i)(4) = Request.QueryString("itemcode") And arySetROLot(i)(5) = Request.QueryString("itemline") And arySetROLot(i)(0) <> "---Select---" Then
                    For j = 0 To arySetROLot.Count - 1
                        If arySetROLot(j)(4) = Request.QueryString("itemcode") And arySetROLot(j)(5) = Request.QueryString("itemline") And arySetROLot(j)(0) <> "---Select---" And i <> j Then
                            If arySetROLot(i)(0) = arySetROLot(j)(0) And arySetROLot(i)(2) = arySetROLot(j)(2) And arySetROLot(i)(3) = arySetROLot(j)(3) Then
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
                For i = 0 To arySetROLot.Count - 1
                    If arySetROLot(i)(4) = Request.QueryString("itemcode") And arySetROLot(i)(5) = Request.QueryString("itemline") And arySetROLot(i)(0) <> "---Select---" Then
                        dsQty = objInv.PopLotInfoByRO(ViewState("grnno"), ViewState("pono"), ViewState("poline"), Request.QueryString("itemcode"), arySetROLot(i)(0), arySetROLot(i)(2), arySetROLot(i)(3))
                        If CDec(IIf(arySetROLot(i)(1) = "", 0, arySetROLot(i)(1))) > CDec(dsQty.Tables(0).Rows(0)("GL_LOT_RECEIVED_QTY")) Then
                            intNumeric2 = False
                        End If

                        decStkQty = 0
                        decStkQty = objInv.getLotBalance(lblItemCode.Text, arySetROLot(i)(0), arySetROLot(i)(2), arySetROLot(i)(3), True)
                        If CDec(IIf(arySetROLot(i)(1) = "", 0, arySetROLot(i)(1))) > decStkQty Then
                            intNumeric3 = False
                        End If

                        If ViewState("qc") = "Y" Then
                            decIQCQty = objInv.chkIQCLotQty(ViewState("grnno"), lblItemCode.Text, , arySetROLot(i)(0), arySetROLot(i)(2), arySetROLot(i)(3), , True)
                            decRejIQCQty = objInv.chkIQCLotQty(ViewState("grnno"), lblItemCode.Text, , arySetROLot(i)(0), arySetROLot(i)(2), arySetROLot(i)(3), "Rej", True)

                            If decIQCQty > 0 Then
                                If CDec(IIf(arySetROLot(i)(1) = "", 0, arySetROLot(i)(1))) > decIQCQty Then
                                    intNumeric4 = False
                                End If
                            Else
                                If decRejIQCQty > 0 Then
                                    If CDec(IIf(arySetROLot(i)(1) = "", 0, arySetROLot(i)(1))) > decRejIQCQty Then
                                        intNumeric5 = False
                                    End If
                                Else
                                    If CDec(IIf(arySetROLot(i)(1) = "", 0, arySetROLot(i)(1))) > decIQCQty Then
                                        intNumeric4 = False
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
            End If
        End If

        Session("arySetROLot") = Nothing
        Session("arySetROLot") = arySetROLot
    End Function

    Private Function BuildLine()
        Dim i As Integer
        Dim found As Integer
        found = 0

        arySetROLot = Session("arySetROLot")
        For i = 0 To arySetROLot.Count - 1
            If arySetROLot(i)(4) = Request.QueryString("itemcode") And arySetROLot(i)(5) = Request.QueryString("itemline") Then
                found = found + 1
            End If
        Next

        arySetROLot.Add(New String() {Request.Form("cboLot" & found + 1), Request.Form("txtQty" & found + 1), Request.Form("cboLocation" & found + 1), Request.Form("cboSubLocation" & found + 1), Request.QueryString("itemcode"), Request.QueryString("itemline"), "Save", "", "", "", ""})

        Session("arySetROLot") = Nothing
        Session("arySetROLot") = arySetROLot
    End Function

    Private Function BuildRow()
        Dim i As Integer
        Dim found As Boolean
        found = False

        If Not Session("arySetROLot") Is Nothing Then 'Empty
            arySetROLot = Session("arySetROLot")
        End If

        If Session("arySetROLot") Is Nothing Then 'Empty
            For i = 0 To ViewState("Row") - 1
                arySetROLot.Add(New String() {Request.Form("cboLot" & i), Request.Form("txtQty" & i), Request.Form("cboLocation" & i), Request.Form("cboSubLocation" & i), Request.QueryString("itemcode"), Request.QueryString("itemline"), "Save", "", "", "", ""})
            Next

            Session("arySetROLot") = arySetROLot
        Else ' Session Not Empty 1.Reload   2.After Add Line
            arySetROLot = Session("arySetROLot")
            For i = 0 To arySetROLot.Count - 1
                If arySetROLot(i)(4) = Request.QueryString("itemcode") And arySetROLot(i)(5) = Request.QueryString("itemline") Then
                    found = True
                    Exit For
                End If
            Next

            If found = False Then

                For i = 0 To ViewState("Row") - 1
                    arySetROLot.Add(New String() {Request.Form("cboLot" & i), Request.Form("txtQty" & i), Request.Form("cboLocation" & i), Request.Form("cboSubLocation" & i), Request.QueryString("itemcode"), Request.QueryString("itemline"), "Save", "", "", "", ""})
                Next

                Session("arySetROLot") = Nothing
            Else
                ReCal()
            End If

            Session("arySetROLot") = arySetROLot
        End If
    End Function

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Clear()
    End Sub

    Private Function ReCal()
        Dim i As Integer
        Dim aryROLot As New ArrayList
        Dim decTotalQty As Decimal
        aryROLot = Session("arySetROLot")

        decTotalQty = 0
        For i = 0 To aryROLot.Count - 1
            If aryROLot(i)(4) = Request.QueryString("itemcode") And aryROLot(i)(5) = Request.QueryString("itemline") Then
                If (Regex.IsMatch(Trim(aryROLot(i)(1)), "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$")) = False And Request.Form("txtQty" & i) <> "" Then
                Else
                    decTotalQty = decTotalQty + CDec(IIf(aryROLot(i)(1) = "", 0, aryROLot(i)(1)))
                End If
            End If
        Next

        lblRemainQty.Text = CDec(Request.QueryString("rqty")) - decTotalQty
    End Function

    Private Function ReCalStk(ByVal aryTemp As ArrayList, ByVal strLot As String, ByVal strLoc As String, ByVal strSubLoc As String) As Decimal
        Dim iCount As Integer
        Dim decTemp As Decimal = 0

        For iCount = 0 To aryTemp.Count - 1
            If aryTemp(iCount)(4) = Request.QueryString("itemcode") And aryTemp(iCount)(5) = Request.QueryString("itemline") And aryTemp(iCount)(0) = strLot And aryTemp(iCount)(2) = strLoc And aryTemp(iCount)(3) = strSubLoc Then
                If IsNumeric(aryTemp(iCount)(1)) And aryTemp(iCount)(1) <> "" Then
                    decTemp = CDec(aryTemp(iCount)(1))
                End If
            End If
        Next

        Return decTemp

    End Function
End Class
