Imports AgoraLegacy
Imports eProcure.Component
Public Class LotDeliveryOrder
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strGRNType As String
    Dim objDb As New EAD.DBCom
    Dim objRFQ As New RFQ
    Dim objDO As New DeliveryOrder
    Dim objGLO As New AppGlobals
    Protected WithEvents cmd_Save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Add As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Close As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblItemCode As System.Web.UI.WebControls.Label
    Protected WithEvents hiditemline As System.Web.UI.WebControls.Label
    Protected WithEvents hidpoline As System.Web.UI.WebControls.Label
    Protected WithEvents hidShipQty As System.Web.UI.WebControls.Label
    Protected WithEvents lblBuyerName As System.Web.UI.WebControls.Label
    Protected WithEvents lblRejQty As System.Web.UI.WebControls.Label
    Protected WithEvents lblBalQty As System.Web.UI.WebControls.Label
    Protected WithEvents hidButtonClose As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidButtonUpload As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents hidIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidAttachIndex As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidFile As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidBtnDisplay As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidBtnDisplay2 As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents FileDoc As System.Web.UI.HtmlControls.HtmlInputFile

    Dim strPONo As String
    Dim strMsg As String = ""
    Dim aryOriSetLot As New ArrayList()
    Dim arySetLot As New ArrayList()
    Dim aryLot As New ArrayList()
    Dim aryCount As New ArrayList()
    Dim intCheck As Boolean = True
    Dim intCheck2 As Boolean = True
    Dim intCheck3 As Boolean = True
    Dim intCheck4 As Boolean = True
    Dim intNumeric As Boolean = True
    Dim intAlphaNum As Boolean = True
    Dim intEmpty As Boolean = True
    Dim intEmpty2 As Boolean = True
    Dim intEmpty3 As Boolean = True
    Dim intEmpty4 As Boolean = True
    Dim intEmpty5 As Boolean = True
    Dim intAllTrue As Boolean = True
    Dim iTotal As Integer
    Dim LocDesc As String = ""
    Dim SubLocDesc As String = ""
    Dim LDesc As String = ""
    Dim SubLDesc As String = ""
    Dim FileDoc1 As System.Web.UI.HtmlControls.HtmlInputFile

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
        Dim objDb As New EAD.DBCom

        lblItemCode.Text = objDb.GetVal("SELECT PM_VENDOR_ITEM_CODE FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE=" & Request.QueryString("itemcode"))
        hidShipQty.Text = Request.QueryString("shipqty")
        hiditemline.Text = Request.QueryString("itemline")
        hidpoline.Text = Request.QueryString("poline")
        ViewState("submit") = Request.QueryString("submit")
        ViewState("DefaultMfg") = objDO.GetSelectedMfgFromPO(Request.QueryString("poline"), Request.QueryString("PONo"), Request.QueryString("BCoyId"))

        If Not Page.IsPostBack Then
            ViewState("Row") = 5
            BuildRow()
            'If Session("aryLot") Is Nothing Then
            '    BuildRow()
            '    ConstructTable()
            'Else
            '    BuildRow()
            '    ConstructTable(Session("aryLot"))
            'End If

        End If

        If ViewState("submit") = True Then
            cmd_Add.Enabled = False
            cmd_Save.Enabled = False
        End If

        lblMsg.Text = ""
    End Sub

    Private Sub cmd_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Add.Click
        BuildLine()
    End Sub

    Private Sub hidButtonClose_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidButtonClose.ServerClick
        RebuildClose()
    End Sub

    Private Sub cmd_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Save.Click
        Rebuild()

        strMsg = ""
        If intEmpty = False Then
            strMsg &= "<ul type='disc'><li>" & "Lot Qty is required." & "<ul type='disc'></ul></li></ul>"
        End If
        If intEmpty2 = False Then
            strMsg &= "<ul type='disc'><li>" & "Lot No is required." & "<ul type='disc'></ul></li></ul>"
        End If
        If intEmpty3 = False Then
            strMsg &= "<ul type='disc'><li>" & "Mfg Date is required." & "<ul type='disc'></ul></li></ul>"
        End If
        If intEmpty4 = False Then
            strMsg &= "<ul type='disc'><li>" & "Expiry Date is required." & "<ul type='disc'></ul></li></ul>"
        End If
        If intEmpty5 = False Then
            strMsg &= "<ul type='disc'><li>" & "Lot Qty, Lot No, Mfg Date and Expiry Date are required for Lot Line with attachment." & "<ul type='disc'></ul></li></ul>"
        End If

        If intNumeric = False Then
            strMsg &= "<ul type='disc'><li>" & "Invalid Lot Qty." & "<ul type='disc'></ul></li></ul>"
        End If
        If intAlphaNum = False Then
            strMsg &= "<ul type='disc'><li>" & "Invalid Lot No." & "<ul type='disc'></ul></li></ul>"
        End If
        If intCheck = False Then
            strMsg &= "<ul type='disc'><li>" & "Sum Lot Qty must be equal to Ship Qty." & "<ul type='disc'></ul></li></ul>"
        End If
        If intCheck2 = False Then
            strMsg &= "<ul type='disc'><li>" & "Duplicate Lot No in different line is not allowed." & "<ul type='disc'></ul></li></ul>"
        End If
        If intCheck3 = False Then
            strMsg &= "<ul type='disc'><li>" & "Mfg Date should be <= today date" & "<ul type='disc'></ul></li></ul>"
        End If
        If intCheck4 = False Then
            strMsg &= "<ul type='disc'><li>" & "Expiry Date should be > Mfg Date" & "<ul type='disc'></ul></li></ul>"
        End If

        ConstructTable(Session("arySetLot"), , True)

        If strMsg <> "" Then
            lblMsg.Text = strMsg
        Else

            BuildSave()
            lblMsg.Text = ""
            Common.NetMsgbox(Me, objGLO.GetErrorMessage("00003"), MsgBoxStyle.Information)
        End If

    End Sub

    Private Function ConstructTable(Optional ByVal aryVolume As ArrayList = Nothing, Optional ByVal strMode As String = "", Optional ByVal DisplayAttach As Boolean = False)
        Dim strrow As String
        Dim i, c, count As Integer
        Dim table As String
        Dim item2nd As Boolean = False
        Dim found As Boolean = False
        Dim ds As New DataSet
        Dim MfgCalendar, EpyCalendar, strFileName, AttachNewRow As String
        'Dim arySetPrice_temp As New ArrayList()
        'Dim ds As DataSet
        'Dim strItemCode = Request.QueryString("VItemCode")
        'Dim strVComp = Request.QueryString("VCompId")

        'arySetLot = Session("arySetLot")

        If Not aryVolume Is Nothing Then
            arySetLot = aryVolume
        Else
            'If Request.QueryString("mode") = "mod" And Not aryVolume Is Nothing Then

            'End If
        End If

        ds = objDO.GetMfgNameForLot(lblItemCode.Text, Request.QueryString("BCoyID"))

        count = arySetLot.Count

        For i = 0 To count - 1
            If arySetLot(i)(8) <> hiditemline.Text Then
                Continue For
            End If
            strrow &= "<tr style=""background-color:#fdfdfd;"">"
            strrow &= "<td valign=""top"">"
            strrow &= "<input style=""width:100%; margin-right:0px; "" class=""numerictxtbox"" onkeypress=""return isDecimalKey(event)"" type=""text"" id=""txtLotQty" & i & """ name=""txtLotQty" & i & """ value=""" & arySetLot(i)(0) & """>"
            strrow &= "</td>"
            strrow &= "<td valign=""top"">"
            strrow &= "<input style=""width:100%; margin-right:0px; "" maxlength=""20"" class=""txtbox"" onkeypress=""return isAlphaNumericKey(event)"" type=""text"" id=""txtLotNo" & i & """ name=""txtLotNo" & i & """ value=""" & arySetLot(i)(1) & """>"
            strrow &= "</td>"
            strrow &= "<td valign=""top"">"

            MfgCalendar = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtMfgDate" & i & "") & "','cal','width=190,height=165,left=270,top=180');""><IMG style=""CURSOR: hand"" height=""16"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & " width=""16""></A>"

            strrow &= "<input style=""width:70px; margin-right:0px; "" class=""txtbox"" onkeypress=""return isDecimalKey(event)"" type=""text"" readonly id=""txtMfgDate" & i & """ name=""txtMfgDate" & i & """ value=""" & arySetLot(i)(2) & """>" & MfgCalendar
            strrow &= "</td>"
            strrow &= "<td valign=""top"">"

            EpyCalendar = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtEpyDate" & i & "") & "','cal','width=190,height=165,left=270,top=180');""><IMG style=""CURSOR: hand"" height=""16"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & " width=""16""></A>"

            strrow &= "<input style=""width:70px; margin-right:0px; "" class=""txtbox"" onkeypress=""return isDecimalKey(event)"" type=""text"" readonly id=""txtEpyDate" & i & """ name=""txtEpyDate" & i & """ value=""" & arySetLot(i)(3) & """>" & EpyCalendar
            strrow &= "</td>"
            strrow &= "<td valign=""top"">"
            strrow &= "<select class=""ddl"" style=""width:100%; margin-right:0px;"" id=""cboMfgName" & i & """ name=""cboMfgName" & i & """>"
            strrow &= "<option value="""" selected=""selected"">" & "---Select---" & "</option>"

            For c = 0 To ds.Tables(0).Columns.Count - 1
                If Not IsDBNull(ds.Tables(0).Rows(0).Item(c)) Then
                    If ds.Tables(0).Rows(0).Item(c) <> "" Then
                        If arySetLot(i)(4) = ds.Tables(0).Rows(0).Item(c).ToString Then
                            strrow &= "<option value=""" & ds.Tables(0).Rows(0).Item(c).ToString & """ selected=""selected"">" & ds.Tables(0).Rows(0).Item(c).ToString & "</option>"
                        Else
                            If ViewState("DefaultMfg") = ds.Tables(0).Rows(0).Item(c).ToString Then
                                strrow &= "<option value=""" & ds.Tables(0).Rows(0).Item(c).ToString & """ selected=""selected"">" & ds.Tables(0).Rows(0).Item(c).ToString & "</option>"
                            Else
                                strrow &= "<option value=""" & ds.Tables(0).Rows(0).Item(c).ToString & """>" & ds.Tables(0).Rows(0).Item(c).ToString & "</option>"
                            End If
                        End If

                    End If
                End If
            Next

            strrow &= "</select>"
            strrow &= "</td>"
            strrow &= "<td>"

            strFileName = dDispatcher.direct("DO", "DOLotAttachment.aspx", "DONo=" & Request.QueryString("DONo") & "&lineNo=" & i & "&poline=" & Server.UrlEncode(Request.QueryString("poline")) & "&itemcode=" & Server.UrlEncode(Request.QueryString("itemcode")))
            strFileName = Server.UrlEncode(strFileName)

            If ViewState("submit") = False Then
                strrow &= "<input style=""width:40%;"" class=""button"" type=""button""  id=""btnAttach" & i & """ name=""btnAttach" & i & """ onclick=""ShowDialog('" & dDispatcher.direct("Initial", "Dialog.aspx", "page=" & strFileName) & "','250px');"" value=""Add Attachment"">"
            End If

            If DisplayAttach = True Then
                displayAttachFile(AttachNewRow, i, Request.QueryString("poline"))
            End If
            strrow &= AttachNewRow
            strrow &= "</td>"
        Next

        table = "<table class=""grid"" style=""margin-top:10px; border-collapse:collapse; line-height:20px; "" >" & _
                "<tr class=""TableHeader""><td style=""width:60px;"">Lot Qty</td><td style=""width:60px;"">Lot No</td><td style=""width:80px;"">Mfg Date</td><td style=""width:80px;"">Expiry Date</td><td style=""width:80px;"">Mfg Name</td><td style=""width:310px;"">Attachment (eg: CCC, CCA, Inspection Sheet and etc.)</td></tr>" & _
                strrow & _
                "</table>"

        Session("arySetLot") = arySetLot
        Session("ConstructTable") = table
    End Function

    Private Function RebuildClose()
        Dim i, j As Integer
        'Rebuild("Close")
        'If strMsg <> "" Then
        '    BuildRow()
        'Else
        'End If
        arySetLot = Session("arySetLot")

        'If strClose <> "Close" Then
        For i = 0 To arySetLot.Count - 1

            If Request.Form("txtLotQty" & i) = "" Then
                arySetLot(i)(0) = ""
                arySetLot(i)(1) = ""
                arySetLot(i)(2) = ""
                arySetLot(i)(3) = ""
                arySetLot(i)(4) = ""

                If Request.QueryString("mode") = "Edit" Then
                    objDO.deleteTempDOAttachment2(0, Me.Request.QueryString("DONo"), "D", "", True, lblItemCode.Text, i, hidpoline.Text)
                Else
                    objDO.deleteTempDOAttachment2(0, Session.SessionID, "D", "", True, lblItemCode.Text, i, hidpoline.Text)
                End If
            Else
                arySetLot(i)(0) = Request.Form("txtLotQty" & i)
                arySetLot(i)(1) = Request.Form("txtLotNo" & i)
                arySetLot(i)(2) = Request.Form("txtMfgDate" & i)
                arySetLot(i)(3) = Request.Form("txtEpyDate" & i)
                arySetLot(i)(4) = Request.Form("cboMfgName" & i)
            End If
        Next
        arySetLot = Nothing
        'Session("aryLot") = Session("arySetLot")

        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        strscript.Append("Close();")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())

    End Function

    Private Function BuildSave()
        Dim i As Integer
        Dim found As Integer
        found = 0

        arySetLot = Session("arySetLot")

        If Not Session("aryLot") Is Nothing Then
            aryLot = Session("aryLot")

            For i = 0 To arySetLot.Count - 1
                aryLot.Add(New String() {arySetLot(i)(0), arySetLot(i)(1), arySetLot(i)(2), arySetLot(i)(3), arySetLot(i)(4), arySetLot(i)(5), lblItemCode.Text, arySetLot(i)(7), hiditemline.Text, "AA", hidpoline.Text})
                'aryLot.Add(New String() {arySetLot(i)(0), arySetLot(i)(1), lblItemCode.Text, hiditemline.Text, "Save", "", ""})
            Next

            i = 0

            Do While i < aryLot.Count
                If aryLot(i)(8) = hiditemline.Text And aryLot(i)(6) = lblItemCode.Text And aryLot(i)(9) = "" Then
                    aryLot(i)(6) = ""
                    aryLot.RemoveAt(i)
                    i = 0
                Else
                    i = i + 1
                End If
            Loop

            '' ''For i = 0 To arySetLot.Count - 1
            '' ''    aryLot.Add(New String() {arySetLot(i)(0), arySetLot(i)(1), arySetLot(i)(2), arySetLot(i)(3), arySetLot(i)(4), arySetLot(i)(5), lblItemCode.Text, arySetLot(i)(7), hiditemline.Text})
            '' ''    'aryLot.Add(New String() {arySetLot(i)(0), arySetLot(i)(1), lblItemCode.Text, hiditemline.Text, "Save", "", ""})
            '' ''Next

            i = 0
            Do While i < aryLot.Count
                aryLot(i)(9) = ""
                i = i + 1
            Loop

            Session("aryLot") = Nothing
            Session("aryLot") = aryLot
        Else
            i = 0
            Do While i < arySetLot.Count
                arySetLot(i)(9) = ""
                i = i + 1
            Loop

            Session("aryLot") = arySetLot
        End If

        

        'Session("aryLot") = arySetLot
    End Function

    Private Function Rebuild(Optional ByVal strClose As String = "")
        Dim i, j As Integer
        Dim c As Double
        Dim found As Integer
        Dim aryTemp As New ArrayList()
        Dim aryTemp2 As New ArrayList()
        found = 0

        intNumeric = True
        intAlphaNum = True
        intEmpty = True
        intEmpty2 = True
        intEmpty3 = True
        intEmpty4 = True
        intEmpty5 = True
        intCheck = True
        intCheck3 = True
        intCheck4 = True
        intAllTrue = True

        Dim strDONo As String
        If Request.QueryString("DONo") = "" Then
            strDONo = Session.SessionID
        Else
            strDONo = Request.QueryString("DONo")
        End If

        arySetLot = Session("arySetLot") 

        'If strClose <> "Close" Then
        For i = 0 To arySetLot.Count - 1

            If Request.Form("txtLotQty" & i) = "" Then
                arySetLot(i)(0) = ""
                arySetLot(i)(1) = ""
                arySetLot(i)(2) = ""
                arySetLot(i)(3) = ""
                arySetLot(i)(4) = ""

                If Request.QueryString("mode") = "Edit" Then
                    objDO.deleteTempDOAttachment2(0, Me.Request.QueryString("DONo"), "D", "", True, lblItemCode.Text, i, hidpoline.Text)
                Else
                    objDO.deleteTempDOAttachment2(0, Session.SessionID, "D", "", True, lblItemCode.Text, i, hidpoline.Text)
                End If
            Else
                arySetLot(i)(0) = Request.Form("txtLotQty" & i)
                arySetLot(i)(1) = Request.Form("txtLotNo" & i)
                arySetLot(i)(2) = Request.Form("txtMfgDate" & i)
                arySetLot(i)(3) = Request.Form("txtEpyDate" & i)
                arySetLot(i)(4) = Request.Form("cboMfgName" & i)
            End If
        Next
        'Else
        'For i = 0 To arySetLot.Count - 1

        '    If Request.Form("txtLotQty" & i) = "" Then
        '        If Request.QueryString("mode") = "Edit" Then
        '            objDO.deleteTempDOAttachment2(0, Me.Request.QueryString("DONo"), "D", "", True, lblItemCode.Text, i)
        '        Else
        '            objDO.deleteTempDOAttachment2(0, Session.SessionID, "D", "", True, lblItemCode.Text, i)
        '        End If
        '    Else
        '    End If
        'Next
        'End If

        Session("arySetLot") = arySetLot

        j = 0
        For i = 0 To arySetLot.Count - 1
            If arySetLot(i)(0) = "" And arySetLot(i)(1) = "" And arySetLot(i)(2) = "" And arySetLot(i)(3) = "" Then
            Else
                aryTemp.Add(New String() {arySetLot(i)(0), arySetLot(i)(1), arySetLot(i)(2), arySetLot(i)(3), arySetLot(i)(4), arySetLot(i)(7), arySetLot(i)(6), j, arySetLot(i)(8), "", arySetLot(i)(10)})
                j = j + 1
            End If
        Next

        aryTemp = aryTemp

        If aryTemp.Count > 0 Then
            For i = 0 To aryTemp.Count - 1

                'Lot Qty
                If aryTemp(i)(0) = "" Then
                    intEmpty = False
                Else
                    If IsNumeric(aryTemp(i)(0)) Then
                        If aryTemp(i)(0) <> 0 Then
                            aryTemp(i)(0) = Format(CDbl(aryTemp(i)(0)), "##0.00")
                        Else

                            intNumeric = False
                        End If
                    Else
                        intNumeric = False
                    End If
                End If

                'Lot No
                If aryTemp(i)(1) = "" Then
                    intEmpty2 = False
                Else
                    If IsAlphaNum(aryTemp(i)(1)) Then
                    Else
                        intAlphaNum = False
                    End If
                End If

                'Mfg Date
                If aryTemp(i)(2) = "" Then
                    intEmpty3 = False
                End If

                If aryTemp(i)(2) <> "" Then
                    If CDate(aryTemp(i)(2)) > Today.Date Then
                        intCheck3 = False
                    End If
                End If

                'Expiry Date
                If aryTemp(i)(3) = "" Then
                    intEmpty4 = False
                End If

                If aryTemp(i)(3) <> "" And aryTemp(i)(2) <> "" Then
                    If CDate(aryTemp(i)(2)) >= CDate(aryTemp(i)(3)) Then
                        intCheck4 = False
                    End If
                End If

                'Attachment
                Dim strTemp As String = ""
                Dim dsTemp As DataSet
                dsTemp = objDO.getTempDOAttachment(strDONo, "D", lblItemCode.Text, aryTemp(i)(5))


                If dsTemp.Tables(0).Rows.Count > 0 Then
                    If aryTemp(i)(0) = "" And aryTemp(i)(1) = "" And aryTemp(i)(2) = "" And aryTemp(i)(3) = "" Then
                        intEmpty5 = False
                    End If

                End If

                If intEmpty = False Or intEmpty2 = False Or intEmpty3 = False Or intEmpty4 = False Or intEmpty5 = False Or intNumeric = False Or intAlphaNum = False Then
                    intAllTrue = False
                End If

            Next
        Else
            intAllTrue = False
        End If


        'Sum all Lot Qty compare to Ship Qty
        If intAllTrue = True Then
            c = 0

            For i = 0 To aryTemp.Count - 1
                c = c + CDbl(aryTemp(i)(0))
            Next

            If CDbl(hidShipQty.Text) = c Then
                For i = 0 To aryTemp.Count - 1
                    For j = 0 To aryTemp.Count - 1
                        If i <> j And aryTemp(i)(1) = aryTemp(j)(1) Then
                            intCheck2 = False
                            Exit Function
                        End If
                    Next
                Next
            Else
                intCheck = False
                Exit Function
                'Else
                '    Session("aryCount") = aryTemp.Count

                '    If Not Session("aryCount") Is Nothing Then
                '        aryCount = Session("aryCount")

                '        Do While i < aryCount.Count
                '            If aryCount(i)(0) = hiditemline.Text Then
                '                aryCount.RemoveAt(i)
                '                i = 0
                '            Else
                '                i = i + 1
                '            End If
                '        Loop

                '        aryCount.Add(New String() {hiditemline.Text, aryTemp.Count})

                '        Session("aryCount") = Nothing
                '        Session("aryCount") = aryCount
                '    Else
                '        aryCount.Add(New String() {hiditemline.Text, aryTemp.Count})
                '        Session("aryCount") = aryCount
                '    End If
            End If

            If 5 > aryTemp.Count Then
                For i = 0 To 5 - 1
                    aryTemp2.Add(New String() {"", "", "", "", "", "", lblItemCode.Text, i, hiditemline.Text, "", hidpoline.Text})
                Next
            Else
                For i = 0 To aryTemp.Count - 1
                    aryTemp2.Add(New String() {"", "", "", "", "", "", lblItemCode.Text, i, hiditemline.Text, "", hidpoline.Text})
                Next
            End If

            For i = 0 To aryTemp.Count - 1
                aryTemp2(i)(0) = aryTemp(i)(0)
                aryTemp2(i)(1) = aryTemp(i)(1)
                aryTemp2(i)(2) = aryTemp(i)(2)
                aryTemp2(i)(3) = aryTemp(i)(3)
                aryTemp2(i)(4) = aryTemp(i)(4)
                aryTemp2(i)(5) = aryTemp(i)(5)
                aryTemp2(i)(6) = aryTemp(i)(6)
                aryTemp2(i)(8) = aryTemp(i)(8)
                aryTemp2(i)(10) = aryTemp(i)(10)
            Next



            For i = 0 To aryTemp2.Count - 1
                If Not aryTemp2(i)(5) Is Nothing Then
                    If aryTemp2(i)(5) <> "" And (aryTemp2(i)(5) <> aryTemp2(i)(7)) Then
                        objDO.UpdateAttachmentLine(strDONo, lblItemCode.Text, aryTemp2(i)(5), aryTemp2(i)(7), hidpoline.Text)
                    End If
                End If
            Next
        End If

        'If Request.QueryString("mode") = "Edit" Then
        '    objDO.deleteTempDOAttachment(0, Me.Request.QueryString("DONo"), "D", "", True, lblItemCode.Text, "Clean")
        'Else
        '    objDO.deleteTempDOAttachment(0, Session.SessionID, "D", "", True, lblItemCode.Text, "Clean")
        'End If
    End Function

    Private Function BuildLine()
        Dim i As Integer
        Dim found As Integer
        found = 0

        arySetLot = Session("arySetLot")
        For i = 0 To arySetLot.Count - 1
            arySetLot(i)(0) = Request.Form("txtLotQty" & i)
            arySetLot(i)(1) = Request.Form("txtLotNo" & i)
            arySetLot(i)(2) = Request.Form("txtMfgDate" & i)
            arySetLot(i)(3) = Request.Form("txtEpyDate" & i)
            arySetLot(i)(4) = Request.Form("cboMfgName" & i)

            found = found + 1
        Next
        arySetLot.Add(New String() {Request.Form("txtLotQty" & found), Request.Form("txtLotNo" & found), Request.Form("txtMfgDate" & found), Request.Form("txtEpyDate" & found), Request.Form("cboMfgName" & found), "", lblItemCode.Text, found, hiditemline.Text, "", hidpoline.Text})
        Session("arySetLot") = Nothing
        Session("arySetLot") = arySetLot
        Session("aryCount") = Nothing
        Session("aryCount") = arySetLot.Count

        ConstructTable(arySetLot, , True)

    End Function

    Private Function BuildRow()
        Dim tempArray As ArrayList
        Dim i As Integer
        Dim found As Boolean
        Dim dsLot As New DataSet
        found = False

        'If Request.QueryString("mode") = "Edit" Then
        '    objDO.deleteTempDOAttachment(0, Me.Request.QueryString("DONo"), "D", "", True, lblItemCode.Text, "Clean")
        'Else
        '    objDO.deleteTempDOAttachment(0, Session.SessionID, "D", "", True, lblItemCode.Text, "Clean")
        'End If

        If Session("aryLot") Is Nothing Then 'Empty
            If Request.QueryString("mode") = "Edit" Then
                dsLot = objDO.getDOLot(Request.QueryString("DONo"), lblItemCode.Text, hidpoline.Text)

                If dsLot.Tables(0).Rows.Count > 0 Then
                    If dsLot.Tables(0).Rows.Count > 5 Then
                        ViewState("Row") = dsLot.Tables(0).Rows.Count
                    Else
                        ViewState("Row") = 5
                    End If

                    For i = 0 To ViewState("Row") - 1
                        arySetLot.Add(New String() {"", "", "", "", "", "", lblItemCode.Text, i, hiditemline.Text, "", hidpoline.Text})
                    Next

                    For i = 0 To dsLot.Tables(0).Rows.Count - 1
                        arySetLot(i)(0) = Format(CDbl(dsLot.Tables(0).Rows(i)("DOL_LOT_QTY")), "##0.00")
                        arySetLot(i)(1) = dsLot.Tables(0).Rows(i)("DOL_LOT_NO")
                        arySetLot(i)(2) = dsLot.Tables(0).Rows(i)("DOL_DO_MANU_DT")
                        arySetLot(i)(3) = dsLot.Tables(0).Rows(i)("DOL_DO_EXP_DT")
                        arySetLot(i)(4) = dsLot.Tables(0).Rows(i)("DOL_DO_MANUFACTURER")

                    Next
                Else
                    For i = 0 To ViewState("Row") - 1
                        arySetLot.Add(New String() {Request.Form("txtLotQty" & i), Request.Form("txtLotNo" & i), Request.Form("txtMfgDate" & i), Request.Form("txtEpyDate" & i), Request.Form("cboMfgName" & i), "", lblItemCode.Text, i, hiditemline.Text, "", hidpoline.Text})
                    Next
                End If

                objDO.deleteTempDOAttachment(0, Me.Request.QueryString("DONo"), "D", "", True, lblItemCode.Text, hidpoline.Text)
            Else
                For i = 0 To ViewState("Row") - 1
                    arySetLot.Add(New String() {Request.Form("txtLotQty" & i), Request.Form("txtLotNo" & i), Request.Form("txtMfgDate" & i), Request.Form("txtEpyDate" & i), Request.Form("cboMfgName" & i), "", lblItemCode.Text, i, hiditemline.Text, "", hidpoline.Text})
                Next

                objDO.deleteTempDOAttachment(0, Session.SessionID, "D", "", True, lblItemCode.Text, hidpoline.Text)
            End If

            Session("arySetLot") = arySetLot
            ConstructTable(arySetLot, , True)
        Else ' Session Not Empty 1.Reload   2.After Add Line

            tempArray = Session("aryLot")
            'Dim ifound As Integer = 0
            For i = 0 To tempArray.Count - 1
                If tempArray(i)(8) = hiditemline.Text And tempArray(i)(6) = lblItemCode.Text Then
                    found = True
                    'ifound = ifound + 1
                    Exit For
                End If
            Next

            If found = False Then
                If Request.QueryString("mode") = "Edit" Then
                    dsLot = objDO.getDOLot(Request.QueryString("DONo"), lblItemCode.Text, hidpoline.Text)

                    If dsLot.Tables(0).Rows.Count > 0 Then
                        If dsLot.Tables(0).Rows.Count > 5 Then
                            ViewState("Row") = dsLot.Tables(0).Rows.Count
                        Else
                            ViewState("Row") = 5
                        End If



                        For i = 0 To ViewState("Row") - 1
                            arySetLot.Add(New String() {"", "", "", "", "", "", lblItemCode.Text, i, hiditemline.Text, "", hidpoline.Text})
                        Next

                        For i = 0 To dsLot.Tables(0).Rows.Count - 1
                            arySetLot(i)(0) = Format(CDbl(dsLot.Tables(0).Rows(i)("DOL_LOT_QTY")), "##0.00")
                            arySetLot(i)(1) = dsLot.Tables(0).Rows(i)("DOL_LOT_NO")
                            arySetLot(i)(2) = dsLot.Tables(0).Rows(i)("DOL_DO_MANU_DT")
                            arySetLot(i)(3) = dsLot.Tables(0).Rows(i)("DOL_DO_EXP_DT")
                            arySetLot(i)(4) = dsLot.Tables(0).Rows(i)("DOL_DO_MANUFACTURER")

                        Next
                    Else
                        For i = 0 To ViewState("Row") - 1
                            arySetLot.Add(New String() {Request.Form("txtLotQty" & i), Request.Form("txtLotNo" & i), Request.Form("txtMfgDate" & i), Request.Form("txtEpyDate" & i), Request.Form("cboMfgName" & i), "", lblItemCode.Text, i, hiditemline.Text, "", hidpoline.Text})
                        Next

                    End If

                    objDO.deleteTempDOAttachment(0, Me.Request.QueryString("DONo"), "D", "", True, lblItemCode.Text, hidpoline.Text)
                Else
                    For i = 0 To ViewState("Row") - 1
                        arySetLot.Add(New String() {Request.Form("txtLotQty" & i), Request.Form("txtLotNo" & i), Request.Form("txtMfgDate" & i), Request.Form("txtEpyDate" & i), Request.Form("cboMfgName" & i), "", lblItemCode.Text, i, hiditemline.Text, "", hidpoline.Text})
                    Next

                    objDO.deleteTempDOAttachment(0, Session.SessionID, "D", "", True, lblItemCode.Text, hidpoline.Text)
                End If
            
            Else
            

                For i = 0 To tempArray.Count - 1
                    If tempArray(i)(8) = hiditemline.Text And tempArray(i)(6) = lblItemCode.Text Then
                        arySetLot.Add(New String() {tempArray(i)(0), tempArray(i)(1), tempArray(i)(2), tempArray(i)(3), tempArray(i)(4), "", lblItemCode.Text, tempArray(i)(7), hiditemline.Text, "", hidpoline.Text})
                    End If

                Next


            End If

            Session("arySetLot") = arySetLot
            ConstructTable(arySetLot, , True)

        End If
    End Function


    Private Sub hidBtnDisplay_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidBtnDisplay.ServerClick
        Dim i As Integer

        arySetLot = Session("arySetLot")
        For i = 0 To arySetLot.Count - 1
            If Request.Form("txtLotQty" & i) = "" Then
                arySetLot(i)(0) = ""
                arySetLot(i)(1) = ""
                arySetLot(i)(2) = ""
                arySetLot(i)(3) = ""
                arySetLot(i)(4) = ""
            Else
                arySetLot(i)(0) = Request.Form("txtLotQty" & i)
                arySetLot(i)(1) = Request.Form("txtLotNo" & i)
                arySetLot(i)(2) = Request.Form("txtMfgDate" & i)
                arySetLot(i)(3) = Request.Form("txtEpyDate" & i)
                arySetLot(i)(4) = Request.Form("cboMfgName" & i)
            End If
        Next

        Session("arySetLot") = arySetLot
        ConstructTable(Session("arySetLot"), , True)
    End Sub

    Private Sub displayAttachFile(ByRef AttachRow As String, ByVal strLineNo As String, ByVal strPOLine As String)
        Dim dsAttach As New DataSet
        Dim drvAttach As New DataView
        Dim i As Integer
        Dim objFile As New FileManagement
        Dim strFile, strFile1, strURL, strTemp, RemoveBtn As String
        Dim strDocNo = Request.QueryString("DONo")

        If strDocNo = "" Then
            dsAttach = objDO.getTempDOAttachment(Session.SessionID, "D", lblItemCode.Text, strLineNo, strPOLine)
        Else
            If Session("submit") = True Then
                dsAttach = objDO.getDOAttachment(strDocNo, , "D", lblItemCode.Text, strLineNo, strPOLine)
            Else
                dsAttach = objDO.getTempDOAttachment(strDocNo, "D", lblItemCode.Text, strLineNo, strPOLine)
            End If

        End If

        AttachRow = ""

        drvAttach = dsAttach.Tables(0).DefaultView
        If drvAttach.Count > 0 Then
            For i = 0 To drvAttach.Count - 1
                Dim strFilePath As String
                strFile = drvAttach(i)("CDDA_ATTACH_FILENAME")
                strFile1 = drvAttach(i)("CDDA_HUB_FILENAME")
                strURL = objFile.getAttachPath(Server.UrlEncode(strFile), strFile, Server.UrlEncode(strFile1), EnumDownLoadType.DOAttachment, "DO", EnumUploadFrom.FrontOff)
                'Dim lnk As New ImageButton
                'lnk.ImageUrl = dDispatcher.direct("Plugins/images", "i_delete2.gif")
                'lnk.ID = drvAttach(i)("CDDA_ATTACH_INDEX")
                'lnk.CausesValidation = False
                RemoveBtn = "<A onclick=""fireHid3('" & drvAttach(i)("CDDA_ATTACH_INDEX") & "');""><IMG style=""CURSOR: hand"" height=""16"" src=" & dDispatcher.direct("Plugins/images", "i_delete2.gif") & " width=""12""></A>"

                If ViewState("submit") = True Then
                    If i = 0 Then
                        AttachRow &= ""
                    Else
                        AttachRow &= "<br>"
                    End If
                Else
                    AttachRow &= "<br>"
                End If

                If ViewState("submit") = True Then
                    AttachRow &= "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDDA_FILESIZE") & "KB) "
                Else
                    AttachRow &= "&nbsp;" & i + 1 & ") " & strURL & " (" & drvAttach(i)("CDDA_FILESIZE") & "KB) " & RemoveBtn
                End If



            Next
        End If

    End Sub

    Private Sub hidBtnDisplay2_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidBtnDisplay2.ServerClick
        Dim i As Integer
        Dim strDocNo, strStatus As String
        strDocNo = Request.QueryString("DONo")

        If strDocNo <> "" Then
            strDocNo = strDocNo
            strStatus = "U" 'ie delete the attachment that is already in the database or delete the new attachment of Draft DO
        Else
            strDocNo = Session.SessionID
            strStatus = "D" 'ie. delete those attachment that has not been updated into the database
        End If

        objDO.deleteTempDOAttachment(hidAttachIndex.Value, strDocNo, "D", strStatus)

        arySetLot = Session("arySetLot")
        For i = 0 To arySetLot.Count - 1
            If Request.Form("txtLotQty" & i) = "" Then
                arySetLot(i)(0) = ""
                arySetLot(i)(1) = ""
                arySetLot(i)(2) = ""
                arySetLot(i)(3) = ""
                arySetLot(i)(4) = ""
            Else
                arySetLot(i)(0) = Request.Form("txtLotQty" & i)
                arySetLot(i)(1) = Request.Form("txtLotNo" & i)
                arySetLot(i)(2) = Request.Form("txtMfgDate" & i)
                arySetLot(i)(3) = Request.Form("txtEpyDate" & i)
                arySetLot(i)(4) = Request.Form("cboMfgName" & i)
            End If
        Next

        Session("arySetLot") = arySetLot
        ConstructTable(Session("arySetLot"), , True)
        Session("aryLot") = Session("arySetLot")
    End Sub

End Class
