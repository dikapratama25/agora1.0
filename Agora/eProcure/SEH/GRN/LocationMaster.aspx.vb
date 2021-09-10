Imports AgoraLegacy
Imports eProcure.Component

Public Class LocationMasterSEH
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strGRNType As String
    Dim objINV As New Inventory
    Dim objGRN_Ext As New GRN_Ext
    Dim objGLO As New AppGlobals
    Protected WithEvents cmd_Save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Add As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Close As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblItemCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblItemName As System.Web.UI.WebControls.Label
    Protected WithEvents lblRecQty As System.Web.UI.WebControls.Label
    Protected WithEvents lblRejQty As System.Web.UI.WebControls.Label
    Protected WithEvents lblBalQty As System.Web.UI.WebControls.Label

    Protected WithEvents hidButton As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidButtonClose As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidButtonClear As System.Web.UI.HtmlControls.HtmlInputButton

    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents hidLbl As System.Web.UI.WebControls.Label

    Dim strPONo As String
    Dim strMsg As String = ""
    Dim arySetLocation As New ArrayList()
    Dim intNumeric As Boolean = True
    Dim intEmpty As Boolean = True
    Dim intEmpty2 As Boolean = True
    Dim intCalLotQty As Boolean = True
    'Dim intBalQty As Boolean = True
    Dim iTotal As Decimal 'Integer
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
        lblItemCode.Text = IIf(Common.parseNull(Request.QueryString("item")) = "", 0, Request.QueryString("item"))
        lblItemName.Text = IIf(Common.parseNull(Request.QueryString("itemname")) = "", 0, Request.QueryString("itemname"))
        lblRecQty.Text = IIf(Common.parseNull(Request.QueryString("rqty")) = "", 0, Request.QueryString("rqty"))
        lblRejQty.Text = IIf(Common.parseNull(Request.QueryString("rjqty")) = "", 0, Request.QueryString("rjqty"))
        ViewState("itemrow") = Request.QueryString("itemrow")
        ' Title
        objINV.GetLocationDesc(LocDesc, SubLocDesc)
        LocDesc = IIf(LocDesc = "", "Location", "" + LocDesc + "")
        SubLocDesc = IIf(SubLocDesc = "", "Sub Location", "" + SubLocDesc + "")

        'lblBalQty.Text = 0
        Dim checkfirst As Boolean = False
        ' Build Default Location Item
        objINV.GetDefaultLocationDesc(LDesc, SubLDesc)

        If Not Page.IsPostBack Then

            lblBalQty.Text = Request.QueryString("rqty") - IIf(Common.parseNull(Request.QueryString("rjqty")) = "", 0, Request.QueryString("rjqty"))

            If LDesc <> "" Then
                ViewState("Row") = 4
            Else
                ViewState("Row") = 5
            End If

            BuildRow()
            ReCal()
            ConstructTable()


        End If

        lblMsg.Text = ""
    End Sub

    Private Sub cmd_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Add.Click
        Rebuild()
        BuildLine()
        ConstructTable()
    End Sub

    Private Sub hidButton_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidButton.ServerClick
        Rebuild()
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

        If iTotal <= (Request.QueryString("rqty") - IIf(Common.parseNull(Request.QueryString("rjqty")) = "", 0, Request.QueryString("rjqty"))) Then
            lblBalQty.Text = Request.QueryString("rqty") - IIf(Common.parseNull(Request.QueryString("rjqty")) = "", 0, Request.QueryString("rjqty")) - iTotal
        Else
            lblBalQty.Text = Request.QueryString("rqty") - IIf(Common.parseNull(Request.QueryString("rjqty")) = "", 0, Request.QueryString("rjqty"))
        End If

        If intNumeric = False Then
            strMsg = "<ul type='disc'><li>" & "Qty " & objGLO.GetErrorMessage("00342") & "<ul type='disc'></ul></li></ul>"
        End If

        If iTotal > (Request.QueryString("rqty") - IIf(Common.parseNull(Request.QueryString("rjqty")) = "", 0, Request.QueryString("rjqty"))) Then
            strMsg = "<ul type='disc'><li>" & objGLO.GetErrorMessage("00012") & "<ul type='disc'></ul></li></ul>"
        End If

        If intEmpty = False Then
            strMsg = "<ul type='disc'><li>" & objGLO.GetErrorMessage("00019") & "<ul type='disc'></ul></li></ul>"
        End If

        If intEmpty2 = False Then
            strMsg = "<ul type='disc'><li>Lot No is required.<ul type='disc'></ul></li></ul>"
        End If

        If intCalLotQty = False Then
            strMsg = "<ul type='disc'><li>Total of qty must be less than or equal to selected Lot No of qty.<ul type='disc'></ul></li></ul>"
        End If

        'If intBalQty = False Then
        '    strMsg = "<ul type='disc'><li>Balance Qty must be equal to zero.<ul type='disc'></ul></li></ul>"
        'End If

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
        Dim strrow As String
        Dim i, h, j, count As Integer
        Dim table As String
        Dim item2nd As Boolean = False
        Dim found As Boolean = False
        Dim arySetLocation_temp As New ArrayList

        Dim c As Integer
        Dim ds, dsLot, dsSub As DataSet
        ds = objINV.PopLocation()

        arySetLocation = Session("arySetLocation")
        count = arySetLocation.Count

        For i = 0 To count - 1
            dsSub = objINV.PopLocation("")
            If Session("blnGRNTrue") = False Then
                dsLot = objGRN_Ext.GetLocLot(Request.QueryString("DONo"), Request.QueryString("item"), Request.QueryString("vendor"), Request.QueryString("poline"))
            End If

            'If arySetLocation(i)(3) <> Request.QueryString("item") And arySetLocation(i)(8) <> Request.QueryString("itemrow") Then
            '    Continue For
            'End If

            If arySetLocation(i)(3) = Request.QueryString("item") And arySetLocation(i)(8) = Request.QueryString("itemrow") Then



                'If ((i Mod 2) = 0) Then
                strrow &= "<tr style=""background-color:#fdfdfd;"">"
                strrow &= "<td>"
                strrow &= "<select class=""ddl"" style=""width:100%; margin-right:0px;"" onchange =""onClick()"" id=""cboLocation" & i & """ name=""cboLocation" & i & """>"
                strrow &= "<option value=""---Select---"" selected=""selected"">" & "---Select---" & "</option>"

                For c = 0 To ds.Tables(0).Rows.Count - 1
                    If arySetLocation(i)(0) = ds.Tables(0).Rows(c).Item(0).ToString Then
                        strrow &= "<option value=""" & ds.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & ds.Tables(0).Rows(c).Item(0).ToString & "</option>"
                        dsSub = objINV.PopLocation(ds.Tables(0).Rows(c).Item(0).ToString)
                    Else
                        strrow &= "<option value=""" & ds.Tables(0).Rows(c).Item(0).ToString & """>" & ds.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    End If
                Next

                strrow &= "</select>"
                strrow &= "</td>"


                strrow &= "<td>"
                strrow &= "<select class=""ddl"" style=""width:100%; margin-right:0px;"" id=""cboSubLocation" & i & """ name=""cboSubLocation" & i & """>"

                For c = 0 To dsSub.Tables(0).Rows.Count - 1
                    If arySetLocation(i)(1) = dsSub.Tables(0).Rows(c).Item(0).ToString Then
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

                strrow &= "<td >"
                strrow &= "<input style=""width:100%; margin-right:0px; "" class=""numerictxtbox"" onkeypress=""return isNumberKey(event)"" type=""text"" id=""txtQty" & i & """ name=""txtQty" & i & """ value=""" & arySetLocation(i)(2) & """>"
                strrow &= "</td>"

                If Session("blnGRNTrue") = False Then
                    strrow &= "<td>"
                    strrow &= "<select class=""ddl"" style=""width:100%; margin-right:0px;"" id=""cboLot" & i & """ name=""cboLot" & i & """>"
                    strrow &= "<option value=""---Select---"" selected=""selected"">" & "---Select---" & "</option>"

                    For c = 0 To dsLot.Tables(0).Rows.Count - 1
                        If arySetLocation(i)(9) = dsLot.Tables(0).Rows(c).Item(2).ToString Then
                            strrow &= "<option value=""" & dsLot.Tables(0).Rows(c).Item(2).ToString & """ selected=""selected"">" & dsLot.Tables(0).Rows(c).Item(0).ToString & " (" & dsLot.Tables(0).Rows(c).Item(1).ToString & ")" & "</option>"
                        Else
                            strrow &= "<option value=""" & dsLot.Tables(0).Rows(c).Item(2).ToString & """>" & dsLot.Tables(0).Rows(c).Item(0).ToString & " (" & dsLot.Tables(0).Rows(c).Item(1).ToString & ")" & "</option>"
                        End If

                    Next

                    strrow &= "</select>"
                    strrow &= "</td>"
                End If

                strrow &= "</tr>"
                'Else
                '    strrow &= "<tr style=""background-color:#f5f9fc;"">"
                'End If
            End If

        Next
        If Session("blnGRNTrue") = False Then
            table = "<table class=""grid"" style=""margin-top:10px; border-collapse:collapse; line-height:20px; "" >" & _
              "<tr class=""TableHeader""><td style=""width:180px;"">" & LocDesc & "</td><td style=""width:180px;"">" & SubLocDesc & "</td><td style=""width:60px;"">Qty</td><td style=""width:180px;"">Lot No</td></tr>" & _
              strrow & _
              "</table>"
        Else
            table = "<table class=""grid"" style=""margin-top:10px; border-collapse:collapse; line-height:20px; "" >" & _
              "<tr class=""TableHeader""><td style=""width:180px;"">" & LocDesc & "</td><td style=""width:180px;"">" & SubLocDesc & "</td><td style=""width:60px;"">Qty</td></tr>" & _
              strrow & _
              "</table>"
        End If
      

        Session("ConstructTable") = table

    End Function

    Private Function ReCal()
        Dim i As Integer
        Dim found As Integer
        found = 0
        intNumeric = True
        intEmpty = True
        intEmpty2 = True
        iTotal = 0

        arySetLocation = Session("arySetLocation")
        For i = 0 To arySetLocation.Count - 1
            If arySetLocation(i)(3) = Request.QueryString("item") And arySetLocation(i)(8) = Request.QueryString("itemrow") Then

                Dim iCheck As Decimal 'Integer
                iCheck = IsNumeric(arySetLocation(i)(2))
                If (IsNumeric(arySetLocation(i)(2)) = False) And arySetLocation(i)(2) <> "" Then
                    intNumeric = False
                Else
                    iTotal = iTotal + CDec(IIf(arySetLocation(i)(2) = "", 0, arySetLocation(i)(2)))
                End If

                If arySetLocation(i)(0) = "---Select---" And Request.Form("txtQty" & i) <> "" Then
                    intEmpty = False
                End If

            End If

        Next

        If iTotal <= (Request.QueryString("rqty") - IIf(Common.parseNull(Request.QueryString("rjqty")) = "", 0, Request.QueryString("rjqty"))) Then
            lblBalQty.Text = CDec(Request.QueryString("rqty")) - IIf(Common.parseNull(Request.QueryString("rjqty")) = "", 0, CDec(Request.QueryString("rjqty"))) - iTotal
        Else
            lblBalQty.Text = CDec(Request.QueryString("rqty")) - IIf(Common.parseNull(Request.QueryString("rjqty")) = "", 0, CDec(Request.QueryString("rjqty")))
        End If

        Session("arySetLocation") = Nothing
        Session("arySetLocation") = arySetLocation
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

        If Not Session("arySetLocation") Is Nothing Then
            arySetLocation = Session("arySetLocation")

            Do While i < arySetLocation.Count
                If arySetLocation(i)(3) = lblItemCode.Text And arySetLocation(i)(8) = ViewState("itemrow") And arySetLocation(i)(4) = "" Then
                    arySetLocation.RemoveAt(i)
                    i = 0
                Else
                    i = i + 1
                End If
            Loop

            For i = 0 To arySetLocation.Count - 1
                If arySetLocation(i)(3) = lblItemCode.Text And arySetLocation(i)(8) = ViewState("itemrow") And arySetLocation(i)(4) = "Clear" Then
                    arySetLocation(i)(0) = arySetLocation(i)(5)
                    arySetLocation(i)(1) = arySetLocation(i)(6)
                    arySetLocation(i)(2) = arySetLocation(i)(7)
                    arySetLocation(i)(9) = arySetLocation(i)(10)

                    arySetLocation(i)(4) = "Save"
                End If
            Next

            Session("arySetLocation") = Nothing
            Session("arySetLocation") = arySetLocation
        End If
    End Function

    Private Function BuildSave()
        Dim i As Integer
        Dim found As Integer
        found = 0

        arySetLocation = Session("arySetLocation")
        For i = 0 To arySetLocation.Count - 1
            If arySetLocation(i)(3) = Request.QueryString("item") And arySetLocation(i)(8) = Request.QueryString("itemrow") Then
                arySetLocation(i)(4) = "Save"
            End If
        Next

        Session("arySetLocation") = Nothing
        Session("arySetLocation") = arySetLocation
    End Function

    Private Function Rebuild()
        Dim i, j As Integer
        Dim found As Integer
        Dim TotalLotQty As Decimal
        Dim BalQty As Decimal = CDec(lblRecQty.Text) - CDec(lblRejQty.Text)
        Dim dsLot2 As DataSet
        found = 0
        intNumeric = True
        intEmpty = True
        intEmpty2 = True
        intCalLotQty = True
        iTotal = 0

        arySetLocation = Session("arySetLocation")
        For i = 0 To arySetLocation.Count - 1
            If arySetLocation(i)(3) = Request.QueryString("item") And arySetLocation(i)(8) = Request.QueryString("itemrow") Then
                arySetLocation(i)(5) = arySetLocation(i)(0)
                arySetLocation(i)(6) = arySetLocation(i)(1)
                arySetLocation(i)(7) = arySetLocation(i)(2)
                arySetLocation(i)(10) = arySetLocation(i)(9)

                arySetLocation(i)(0) = Request.Form("cboLocation" & i)
                arySetLocation(i)(1) = Request.Form("cboSubLocation" & i)
                arySetLocation(i)(2) = Request.Form("txtQty" & i)
                arySetLocation(i)(3) = Request.QueryString("item")
                arySetLocation(i)(9) = Request.Form("cboLot" & i)

                arySetLocation(i)(4) = "Clear"

                Dim iCheck As Decimal
                iCheck = IsNumeric(Request.Form("txtQty" & i))
                'If IsNumeric(Request.Form("txtQty" & i)) = False And Request.Form("txtQty" & i) <> "" Then
                If (Regex.IsMatch(Trim(Request.Form("txtQty" & i)), "(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$")) = False And Request.Form("txtQty" & i) <> "" Then
                    intNumeric = False
                Else
                    iTotal = iTotal + CDec(IIf(Request.Form("txtQty" & i) = "", 0, Request.Form("txtQty" & i)))
                End If

                If arySetLocation(i)(0) = "---Select---" And Request.Form("txtQty" & i) <> "" Then
                    intEmpty = False
                End If

            End If
        Next

        If Session("blnGRNTrue") = False Then
            For i = 0 To arySetLocation.Count - 1
                If arySetLocation(i)(3) = Request.QueryString("item") And arySetLocation(i)(8) = Request.QueryString("itemrow") Then
                    If arySetLocation(i)(0) <> "---Select---" And Request.Form("txtQty" & i) <> "" And arySetLocation(i)(9) = "---Select---" Then
                        intEmpty2 = False
                    End If
                End If
            Next

            If intNumeric = True And intEmpty2 = True Then
                dsLot2 = objGRN_Ext.GetLocLot(Request.QueryString("DONo"), Request.QueryString("item"), Request.QueryString("vendor"), Request.QueryString("poline"))

                If dsLot2.Tables(0).Rows.Count > 0 Then
                    For i = 0 To dsLot2.Tables(0).Rows.Count - 1
                        TotalLotQty = 0

                        For j = 0 To arySetLocation.Count - 1
                            If CStr(dsLot2.Tables(0).Rows(i)(2)) = arySetLocation(j)(9) And arySetLocation(j)(2) <> "" Then
                                TotalLotQty = TotalLotQty + CDec(arySetLocation(j)(2))
                                found = found + 1
                            End If
                        Next

                        If found > 0 Then
                            If CDec(dsLot2.Tables(0).Rows(i)(1)) < TotalLotQty Then
                                intCalLotQty = False
                                Exit For
                            End If

                            BalQty = BalQty - TotalLotQty
                        End If

                    Next

                    'If intCalLotQty = True Then
                    '    If BalQty > 0 Then
                    '        intBalQty = False
                    '    End If
                    'End If

                End If

            End If
        End If

        Session("arySetLocation") = Nothing
        Session("arySetLocation") = arySetLocation
    End Function

    Private Function BuildLine()
        Dim i As Integer
        Dim found As Integer
        found = 0

        arySetLocation = Session("arySetLocation")
        For i = 0 To arySetLocation.Count - 1
            If arySetLocation(i)(3) = Request.QueryString("item") And arySetLocation(i)(8) = Request.QueryString("itemrow") Then
                found = found + 1
            End If
        Next

        If Session("blnGRNTrue") = False Then
            arySetLocation.Add(New String() {Request.Form("cboLocation" & found + 1), Request.Form("cboSubLocation" & found + 1), Request.Form("txtQty" & found + 1), Request.QueryString("item"), "Save", "", "", "", Request.QueryString("itemrow"), Request.Form("cboLot" & found + 1), ""})
        Else
            arySetLocation.Add(New String() {Request.Form("cboLocation" & found + 1), Request.Form("cboSubLocation" & found + 1), Request.Form("txtQty" & found + 1), Request.QueryString("item"), "Save", "", "", "", Request.QueryString("itemrow"), "", ""})
        End If

        Session("arySetLocation") = Nothing
        Session("arySetLocation") = arySetLocation
    End Function

    Private Function BuildRow()
        Dim i As Integer
        Dim found As Boolean
        found = False

        If Not Session("arySetLocation") Is Nothing Then 'Empty
            arySetLocation = Session("arySetLocation")
        End If

        If Session("arySetLocation") Is Nothing Then 'Empty
            If LDesc <> "" Then
                arySetLocation.Add(New String() {LDesc, SubLDesc, (Request.QueryString("rqty") - IIf(Common.parseNull(Request.QueryString("rjqty")) = "", 0, Request.QueryString("rjqty"))), Request.QueryString("item"), "Save", "", "", "", Request.QueryString("itemrow"), Request.Form("cboLot" & i), ""})
            End If
            For i = 0 To ViewState("Row") - 1
                arySetLocation.Add(New String() {Request.Form("cboLocation" & i), Request.Form("cboSubLocation" & i), Request.Form("txtQty" & i), Request.QueryString("item"), "Save", "", "", "", Request.QueryString("itemrow"), Request.Form("cboLot" & i), ""})
            Next
            Session("arySetLocation") = arySetLocation
        Else ' Session Not Empty 1.Reload   2.After Add Line
            arySetLocation = Session("arySetLocation")
            For i = 0 To arySetLocation.Count - 1
                If arySetLocation(i)(3) = Request.QueryString("item") And arySetLocation(i)(8) = Request.QueryString("itemrow") Then
                    found = True
                    Exit For
                End If
            Next

            If found = False Then
                If LDesc <> "" Then
                    arySetLocation.Add(New String() {LDesc, SubLDesc, (Request.QueryString("rqty") - IIf(Common.parseNull(Request.QueryString("rjqty")) = "", 0, Request.QueryString("rjqty"))), Request.QueryString("item"), "Save", "", "", "", Request.QueryString("itemrow"), Request.Form("cboLot" & i), ""})
                End If
                For i = 0 To ViewState("Row") - 1
                    arySetLocation.Add(New String() {Request.Form("cboLocation" & i), Request.Form("cboSubLocation" & i), Request.Form("txtQty" & i), Request.QueryString("item"), "Save", "", "", "", Request.QueryString("itemrow"), Request.Form("cboLot" & i), ""})
                Next
                Session("arySetLocation") = Nothing
            Else ' If found then update the Default QTY
                ReCal()
                For i = 0 To arySetLocation.Count - 1
                    If arySetLocation(i)(3) = Request.QueryString("item") And arySetLocation(i)(8) = Request.QueryString("itemrow") And arySetLocation(i)(0) = LDesc And arySetLocation(i)(1) = SubLDesc Then
                        arySetLocation(i)(2) = Request.QueryString("rqty") - IIf(Common.parseNull(Request.QueryString("rjqty")) = "", 0, Request.QueryString("rjqty")) - (iTotal - arySetLocation(i)(2))
                        Exit For
                    End If
                Next
                ReCal()
            End If
            Session("arySetLocation") = arySetLocation
        End If
    End Function

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Clear()
    End Sub
End Class
