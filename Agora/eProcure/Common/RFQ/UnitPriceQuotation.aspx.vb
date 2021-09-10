Imports AgoraLegacy
Imports eProcure.Component
Public Class UnitPriceQuotation
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim strGRNType As String
    Dim objDb As New EAD.DBCom
    Dim objRFQ As New RFQ
    Dim objGLO As New AppGlobals
    Protected WithEvents cmd_Save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Add As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Close As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblItemName As System.Web.UI.WebControls.Label
    Protected WithEvents hiditemline As System.Web.UI.WebControls.Label
    Protected WithEvents hidItem As System.Web.UI.WebControls.Label
    Protected WithEvents lblBuyerName As System.Web.UI.WebControls.Label
    Protected WithEvents lblRejQty As System.Web.UI.WebControls.Label
    Protected WithEvents lblBalQty As System.Web.UI.WebControls.Label
    Protected WithEvents hidButtonClose As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents hidLbl As System.Web.UI.WebControls.Label

    Dim strPONo As String
    Dim strMsg As String = ""
    Dim arySetPrice As New ArrayList()
    Dim aryPrice As New ArrayList()
    Dim intNumeric As Boolean = True
    Dim intNumeric2 As Boolean = True
    Dim intEmpty As Boolean = True
    Dim intEmpty2 As Boolean = True
    Dim intCheck As Boolean = True
    Dim intCheck2 As Boolean = True
    Dim intAllTrue As Boolean = True
    Dim iTotal As Integer
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

       
        lblItemName.Text = Request.QueryString("itemname")
        hidItem.Text = Request.QueryString("qty")
        hiditemline.Text = Request.QueryString("itemline")
        
        If Not Page.IsPostBack Then
            ViewState("Row") = 5
            BuildRow()
            'If Session("aryPrice") Is Nothing Then
            '    BuildRow()
            '    ConstructTable()
            'Else
            '    BuildRow()
            '    ConstructTable(Session("aryPrice"))
            'End If

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

        If intNumeric = False Then
            strMsg = "<ul type='disc'><li>" & "Volume " & objGLO.GetErrorMessage("00013") & "<ul type='disc'></ul></li></ul>"
        End If
        If intNumeric2 = False Then
            strMsg = "<ul type='disc'><li>" & "Price " & objGLO.GetErrorMessage("00013") & "<ul type='disc'></ul></li></ul>"
        End If

        If intEmpty = False Then
            strMsg = "<ul type='disc'><li>" & "Volume cannot be blank or zero" & "<ul type='disc'></ul></li></ul>"
        End If
        If intEmpty2 = False Then
            strMsg = "<ul type='disc'><li>" & "Price cannot be blank or zero" & "<ul type='disc'></ul></li></ul>"
        End If

        If intCheck = False Then
            strMsg = "<ul type='disc'><li>" & "The first volume quantity must be start with 1.00." & "<ul type='disc'></ul></li></ul>"
        End If
        If intCheck2 = False Then
            strMsg = "<ul type='disc'><li>" & "The next volume quantity must be greater than the previous quantity." & "<ul type='disc'></ul></li></ul>"
        End If

        ConstructTable(Session("arySetPrice"))

        If strMsg <> "" Then
            lblMsg.Text = strMsg
        Else
            BuildSave()
            lblMsg.Text = ""
            Common.NetMsgbox(Me, objGLO.GetErrorMessage("00003"), MsgBoxStyle.Information)
        End If

    End Sub

    Private Function ConstructTable(Optional ByVal aryVolume As ArrayList = Nothing, Optional ByVal strMode As String = "")
        Dim strrow As String
        Dim i, count As Integer
        Dim table As String
        Dim item2nd As Boolean = False
        Dim found As Boolean = False

        'Dim arySetPrice_temp As New ArrayList()
        'Dim ds As DataSet
        'Dim strItemCode = Request.QueryString("VItemCode")
        'Dim strVComp = Request.QueryString("VCompId")

        'arySetPrice = Session("arySetPrice")

        If Not aryVolume Is Nothing Then
            arySetPrice = aryVolume
        Else
            If Request.QueryString("mode") = "mod" And Not aryVolume Is Nothing Then

            End If
        End If

        'If Request.QueryString("mode") = "mod" Then
        '    ds = objCat_Ext.GetUnitPrice(strItemCode, strVComp)
        '    For c = 0 To ds.Tables(0).Rows.Count - 1
        '        arySetPrice(c)(0) = ds.Tables(0).Rows(c).Item("PVP_VOLUMN_FROM").ToString
        '        arySetPrice(c)(1) = ds.Tables(0).Rows(c).Item("PVP_VOLUMN_TO").ToString
        '        arySetPrice(c)(2) = ds.Tables(0).Rows(c).Item("PVP_VOLUMN_PRICE").ToString
        '    Next
        'End If

        count = arySetPrice.Count

        For i = 0 To count - 1
            If arySetPrice(i)(3) <> hiditemline.Text Then
                Continue For
            End If
            strrow &= "<tr style=""background-color:#fdfdfd;"">"
            strrow &= "<td>"
            strrow &= "<input style=""width:100%; margin-right:0px; "" class=""numerictxtbox"" onkeypress=""return isDecimalKey(event)"" type=""text"" id=""txtVolume" & i & """ name=""txtVolume" & i & """ value=""" & arySetPrice(i)(0) & """>"
            strrow &= "</td>"
            strrow &= "<td>"
            strrow &= "<input style=""width:100%; margin-right:0px; "" class=""numerictxtbox"" onkeypress=""return isDecimalKey(event)"" type=""text"" id=""txtPrice" & i & """ name=""txtPrice" & i & """ value=""" & arySetPrice(i)(1) & """>"
            strrow &= "</td>"
        Next

        table = "<table class=""grid"" style=""margin-top:10px; border-collapse:collapse; line-height:20px; "" >" & _
                "<tr class=""TableHeader""><td style=""width:180px;"">Volume</td><td style=""width:60px;"">Price</td></tr>" & _
                strrow & _
                "</table>"

        Session("arySetPrice") = arySetPrice
        Session("ConstructTable") = table
    End Function

    Private Function RebuildClose()

        arySetPrice = Nothing

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

        If Not Session("aryPrice") Is Nothing Then
            aryPrice = Session("aryPrice")

            Do While i < aryPrice.Count
                If aryPrice(i)(3) = hiditemline.Text Then
                    aryPrice.RemoveAt(i)
                    i = 0
                Else
                    i = i + 1
                End If
            Loop

            For i = 0 To arySetPrice.Count - 1
                aryPrice.Add(New String() {arySetPrice(i)(0), arySetPrice(i)(1), lblItemName.Text, hiditemline.Text, "Save", "", ""})
            Next

            'For i = 0 To arySetPrice.Count - 1
            '    If arySetPrice(i)(3) = Request.QueryString("vline") Then
            '        arySetPrice(i)(0) = arySetPrice(i)(5)
            '        arySetPrice(i)(1) = arySetPrice(i)(6)
            '        arySetPrice(i)(2) = arySetPrice(i)(7)

            '        arySetPrice(i)(4) = "Save"
            '    End If
            'Next

            Session("aryPrice") = Nothing
            Session("aryPrice") = aryPrice
        Else
            Session("aryPrice") = arySetPrice
        End If



    End Function

    Private Function Rebuild()
        Dim i, j, c As Integer
        Dim found As Integer
        Dim aryTemp As New ArrayList()
        Dim aryTemp2 As New ArrayList()
        found = 0
        intNumeric = True
        intNumeric2 = True
        intEmpty = True
        intEmpty2 = True
        intCheck = True
        intCheck2 = True
        intAllTrue = True

        arySetPrice = Session("arySetPrice")

        For i = 0 To arySetPrice.Count - 1
            If arySetPrice(i)(3) = hiditemline.Text Then

                arySetPrice(i)(5) = arySetPrice(i)(0)
                arySetPrice(i)(6) = arySetPrice(i)(1)
                arySetPrice(i)(0) = Request.Form("txtVolume" & i)
                arySetPrice(i)(1) = Request.Form("txtPrice" & i)

            End If
        Next


        For i = 0 To arySetPrice.Count - 1
            If arySetPrice(i)(3) = hiditemline.Text Then
                If Request.Form("txtVolume" & i) <> "" Or Request.Form("txtPrice" & i) <> "" Then
                    aryTemp.Add(New String() {Request.Form("txtVolume" & i), Request.Form("txtPrice" & i), lblItemName.Text, hiditemline.Text, "Save", arySetPrice(i)(0), arySetPrice(i)(1)})
                End If
            End If
        Next

        aryTemp = aryTemp


        For i = 0 To aryTemp.Count - 1

            'Check for 1st row of volumn whether it is 0.00
            If i = 0 Then
                If (CDbl(aryTemp(i)(0)) <> 1) Then
                    intCheck = False
                    intAllTrue = False
                End If
            End If


            If IsNumeric(aryTemp(i)(0)) = False Then 'Check for volume whether it is numeric number
                intNumeric = False
                intAllTrue = False
            Else

                If (aryTemp(i)(0) = "") Then 'Check for volumn whether it is blank or zero
                    intEmpty = False
                    intAllTrue = False
                Else
                    If CDbl(aryTemp(i)(0)) = 0 Then
                        intEmpty = False
                        intAllTrue = False
                    End If
                End If
            End If

            If IsNumeric(aryTemp(i)(1)) = False Then 'Check for price whether it is numeric number
                intNumeric2 = False
                intAllTrue = False
            Else
                If (aryTemp(i)(1) = "") Then 'Check for price whether it is blank or zero
                    intEmpty2 = False
                    intAllTrue = False
                Else
                    If CDbl(aryTemp(i)(1)) = 0 Then
                        intEmpty2 = False
                        intAllTrue = False
                    End If
                End If

            End If

        Next

        'The next line of volume quantity must be greater than the previous quantity.
        If intEmpty = True And intEmpty2 = True And intCheck = True And intNumeric = True And intNumeric2 = True Then
            For i = 0 To aryTemp.Count - 1
                For c = 0 To aryTemp.Count - 1
                    If i > c Then
                        If CDbl(aryTemp(c)(0)) > CDbl(aryTemp(i)(0)) Then
                            intCheck2 = False
                            intAllTrue = False
                        End If

                    End If
                Next
            Next
        End If

        c = 5
        If c > aryTemp.Count Then
            c = 5
        Else
            c = aryTemp.Count
        End If



        If intAllTrue = True Then
            For i = 0 To c - 1
                aryTemp2.Add(New String() {"", "", lblItemName.Text, hiditemline.Text, "Add", "", ""})
            Next

            For i = 0 To aryTemp.Count - 1

                aryTemp2(i)(0) = Format(CDbl(aryTemp(i)(0)), "##0.00")
                aryTemp2(i)(1) = Format(CDbl(aryTemp(i)(1)), "##0.00")

            Next

            Session("arySetPrice") = aryTemp2
        Else
            Session("arySetPrice") = arySetPrice
        End If

    End Function

    Private Function BuildLine()
        Dim i As Integer
        Dim found As Integer
        found = 0

        arySetPrice = Session("arySetPrice")
        For i = 0 To arySetPrice.Count - 1
            If arySetPrice(i)(3) = hiditemline.Text Then
                arySetPrice(i)(0) = Request.Form("txtVolume" & i)
                arySetPrice(i)(1) = Request.Form("txtPrice" & i)
                found = found + 1
            End If
        Next
        'arySetPrice.Add(New String() {Request.Form("txtVolume" & found + 1), Request.Form("txtPrice" & found + 1), lblItemName.Text, hiditemline.Text, "Add", "", ""})
        arySetPrice.Add(New String() {"", "", lblItemName.Text, hiditemline.Text, "Add", "", ""})
        Session("arySetPrice") = Nothing
        Session("arySetPrice") = arySetPrice

        ConstructTable(arySetPrice)

    End Function

    Private Function BuildRow()
        Dim temp_A As ArrayList
        Dim i As Integer
        Dim found As Boolean
        Dim dsPrice As New DataSet
        found = False

        If Session("aryPrice") Is Nothing Then 'Empty
            If Request.QueryString("mode") = "edit" Then
                dsPrice = objRFQ.GetUnitPriceQuotation(Request.QueryString("rfqid"), hiditemline.Text)

                If dsPrice.Tables(0).Rows.Count > 0 Then
                    If dsPrice.Tables(0).Rows.Count > 5 Then
                        ViewState("Row") = dsPrice.Tables(0).Rows.Count
                    End If

                    For i = 0 To ViewState("Row") - 1
                        If i = 0 Then
                            arySetPrice.Add(New String() {"1.00", Request.Form("txtPrice" & i), lblItemName.Text, hiditemline.Text, "Add", "", ""})
                        Else
                            arySetPrice.Add(New String() {Request.Form("txtVolume" & i), Request.Form("txtPrice" & i), lblItemName.Text, hiditemline.Text, "Add", "", ""})
                        End If


                    Next

                    For i = 0 To dsPrice.Tables(0).Rows.Count - 1
                        arySetPrice(i)(0) = Format(CDbl(dsPrice.Tables(0).Rows(i)("RRVP_VOLUME")), "##0.00")
                        arySetPrice(i)(1) = Format(CDbl(dsPrice.Tables(0).Rows(i)("RRVP_VOLUME_PRICE")), "##0.00")

                    Next
                Else
                    For i = 0 To ViewState("Row") - 1
                        If i = 0 Then
                            arySetPrice.Add(New String() {"1.00", Request.Form("txtPrice" & i), lblItemName.Text, hiditemline.Text, "Add", "", ""})
                        Else
                            arySetPrice.Add(New String() {Request.Form("txtVolume" & i), Request.Form("txtPrice" & i), lblItemName.Text, hiditemline.Text, "Add", "", ""})
                        End If

                    Next
                End If
            Else
                For i = 0 To ViewState("Row") - 1
                    If i = 0 Then
                        arySetPrice.Add(New String() {"1.00", Request.Form("txtPrice" & i), lblItemName.Text, hiditemline.Text, "Add", "", ""})
                    Else
                        arySetPrice.Add(New String() {Request.Form("txtVolume" & i), Request.Form("txtPrice" & i), lblItemName.Text, hiditemline.Text, "Add", "", ""})
                    End If

                Next
            End If

            Session("arySetPrice") = arySetPrice
            ConstructTable(arySetPrice)
        Else ' Session Not Empty 1.Reload   2.After Add Line
            temp_A = Session("aryPrice")
            For i = 0 To temp_A.Count - 1
                If temp_A(i)(3) = hiditemline.Text Then
                    found = True
                    Exit For
                End If

            Next


            If found = False Then
                If Request.QueryString("mode") = "edit" Then
                    dsPrice = objRFQ.GetUnitPriceQuotation(Request.QueryString("rfqid"), hiditemline.Text)

                    If dsPrice.Tables(0).Rows.Count > 0 Then
                        If dsPrice.Tables(0).Rows.Count > 5 Then
                            ViewState("Row") = dsPrice.Tables(0).Rows.Count
                        End If

                        For i = 0 To ViewState("Row") - 1
                            If i = 0 Then
                                arySetPrice.Add(New String() {"1.00", Request.Form("txtPrice" & i), lblItemName.Text, hiditemline.Text, "Add", "", ""})
                            Else
                                arySetPrice.Add(New String() {Request.Form("txtVolume" & i), Request.Form("txtPrice" & i), lblItemName.Text, hiditemline.Text, "Add", "", ""})
                            End If

                        Next

                        For i = 0 To dsPrice.Tables(0).Rows.Count - 1
                            arySetPrice(i)(0) = Format(CDbl(dsPrice.Tables(0).Rows(i)("RRVP_VOLUME")), "##0.00")
                            arySetPrice(i)(1) = Format(CDbl(dsPrice.Tables(0).Rows(i)("RRVP_VOLUME_PRICE")), "##0.00")

                        Next
                    Else

                    End If
                Else
                    For i = 0 To ViewState("Row") - 1
                        If i = 0 Then
                            arySetPrice.Add(New String() {"1.00", Request.Form("txtPrice" & i), lblItemName.Text, hiditemline.Text, "Add", "", ""})
                        Else
                            arySetPrice.Add(New String() {Request.Form("txtVolume" & i), Request.Form("txtPrice" & i), lblItemName.Text, hiditemline.Text, "Add", "", ""})
                        End If
                    Next
                End If

                ConstructTable(arySetPrice)
            Else
                ConstructTable(temp_A)
            End If


        End If



    End Function

End Class
