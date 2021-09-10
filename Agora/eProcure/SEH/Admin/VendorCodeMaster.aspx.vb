Imports AgoraLegacy
Imports eProcure.Component


Public Class VendorCodeMaster
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objDb As New EAD.DBCom
    Dim objGlo As New AppGlobals
    Dim objGlobal_ext As New AppGlobals_Ext
    Dim objAdmin_ext As New Admin_Ext

    Dim strMsg As String = ""
    Dim aryVenCode As New ArrayList()
    Dim blnDup As Boolean = True
    Dim blnDup2 As Boolean = True
    Dim blnDup3 As Boolean = True
    Dim blnEmpty As Boolean = True
    Dim blnEmpty2 As Boolean = True
    Dim blnEmpty3 As Boolean = True
    Dim blnCheck As Boolean = True
    Dim blnCheck2 As Boolean = True
    Dim blnCheck3 As Boolean = True
    Dim blnCheck4 As Boolean = True
    Dim blnCheck5 As Boolean = True
    Dim blnAllTrue As Boolean = True

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Protected WithEvents cmd_Save As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Add As System.Web.UI.WebControls.Button
    Protected WithEvents cmd_Close As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblVenID As System.Web.UI.WebControls.Label
    Protected WithEvents hidVenID As System.Web.UI.WebControls.Label
    Protected WithEvents hidType As System.Web.UI.WebControls.Label
    Protected WithEvents lblBuyerName As System.Web.UI.WebControls.Label
    Protected WithEvents lblRejQty As System.Web.UI.WebControls.Label
    Protected WithEvents lblBalQty As System.Web.UI.WebControls.Label
    Protected WithEvents hidButtonClose As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents hidLbl As System.Web.UI.WebControls.Label

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

        lblVenID.Text = Request.QueryString("venid")
        hidVenID.Text = Request.QueryString("venid")
        hidType.Text = Request.QueryString("type")

        If Not Page.IsPostBack Then
            ViewState("Row") = 5
            BuildRow()
        End If

        lblMsg.Text = ""
    End Sub

    Private Sub BuildRow()
        Dim aryTemp As New ArrayList()
        Dim i, j, intFound As Integer
        Dim blnFound As Boolean = False

        If Session("aryVenCode") Is Nothing Then 'Session Empty
            If Request.QueryString("type") = "AV" Then 'Modify Approved Vendor
               GetVendorCodeInfo()
            Else 'Add New Approved Vendor
                For i = 0 To ViewState("Row") - 1
                    aryVenCode.Add(New String() {"", "", "", lblVenID.Text, hidType.Text, "Add"})
                Next
            End If

            Session("arySetVenCode") = aryVenCode
            ConstructTable()
        Else 'Session Not Empty
            aryTemp = Session("aryVenCode")
            j = 0
            intFound = 0
            For i = 0 To aryTemp.Count - 1
                If aryTemp(i)(3) = lblVenID.Text Then
                    blnFound = True
                    intFound += 1
                End If
            Next

            If blnFound = False Then 'No record found in array
                If Request.QueryString("type") = "AV" Then 'Modify Approved Vendor
                    GetVendorCodeInfo()
                Else 'Add New Approved Vendor
                    For i = 0 To ViewState("Row") - 1
                        aryVenCode.Add(New String() {"", "", "", lblVenID.Text, hidType.Text, "Add"})
                    Next
                End If
            Else 'Record found in array
                If intFound > 5 Then
                    ViewState("Row") = intFound
                End If

                For i = 0 To ViewState("Row") - 1
                    aryVenCode.Add(New String() {"", "", "", lblVenID.Text, hidType.Text, "Add"})
                Next

                For i = 0 To aryTemp.Count - 1
                    If aryTemp(i)(3) = lblVenID.Text Then
                        aryVenCode(j)(0) = aryTemp(i)(0)
                        aryVenCode(j)(1) = aryTemp(i)(1)
                        aryVenCode(j)(2) = aryTemp(i)(2)
                        j += 1
                    End If
                Next
            End If

            Session("arySetVenCode") = aryVenCode
            ConstructTable()
        End If

    End Sub

    Private Sub GetVendorCodeInfo()
        Dim dsVenCode As New DataSet
        Dim i As Integer

        dsVenCode = objAdmin_ext.GetVendorCodeInfo(lblVenID.Text)

        If dsVenCode.Tables(0).Rows.Count > 0 Then
            If dsVenCode.Tables(0).Rows.Count > 5 Then
                ViewState("Row") = dsVenCode.Tables(0).Rows.Count
            End If

            For i = 0 To ViewState("Row") - 1
               aryVenCode.Add(New String() {"", "", "", lblVenID.Text, hidType.Text, "Add"})
            Next

            For i = 0 To dsVenCode.Tables(0).Rows.Count - 1
                aryVenCode(i)(0) = Common.parseNull(dsVenCode.Tables(0).Rows(i)("CVS_SUPP_CODE"))
                aryVenCode(i)(1) = Common.parseNull(dsVenCode.Tables(0).Rows(i)("CVS_DELIVERY_TERM"))
                aryVenCode(i)(2) = Common.parseNull(dsVenCode.Tables(0).Rows(i)("CVS_CURR"))
            Next
        Else
            For i = 0 To ViewState("Row") - 1
                aryVenCode.Add(New String() {"", "", "", lblVenID.Text, hidType.Text, "Add"})
            Next
        End If
    End Sub

    Private Sub ConstructTable()
        Dim strRow, strTable As String
        Dim i, j As Integer
        Dim blnFound As Boolean = False
        Dim dsDelTerm As New DataSet
        Dim dsCurr As New DataSet

        dsDelTerm = objGlobal_ext.FillDelTermForVen()
        dsCurr = objGlobal_ext.FillCurrencyForVen()

        aryVenCode = Session("arySetVenCode")

        For i = 0 To aryVenCode.Count - 1
            If aryVenCode(i)(3) = lblVenID.Text Then
                strRow &= "<tr style=""background-color:#fdfdfd;"">"
                strRow &= "<td>"
                strRow &= "<input style=""width:100%; margin-right:0px; "" class=""txtbox"" type=""text"" id=""txtVenCode" & i & """ name=""txtVenCode" & i & """ value=""" & aryVenCode(i)(0) & """>"
                strRow &= "</td>"
                strRow &= "<td>"
                strRow &= "<select class=""ddl"" style=""width:100%; margin-right:0px;"" id=""cboDelTerm" & i & """ name=""cboDelTerm" & i & """>"
                strRow &= "<option value="""" selected=""selected"">" & "---Select---" & "</option>"

                For j = 0 To dsDelTerm.Tables(0).Rows.Count - 1
                    If aryVenCode(i)(1) = dsDelTerm.Tables(0).Rows(j).Item(0).ToString Then
                        strRow &= "<option value=""" & dsDelTerm.Tables(0).Rows(j).Item(0).ToString & """ selected=""selected"">" & dsDelTerm.Tables(0).Rows(j).Item(0).ToString & " (" & dsDelTerm.Tables(0).Rows(j).Item(1).ToString & ")</option>"
                    Else
                        strRow &= "<option value=""" & dsDelTerm.Tables(0).Rows(j).Item(0).ToString & """>" & dsDelTerm.Tables(0).Rows(j).Item(0).ToString & " (" & dsDelTerm.Tables(0).Rows(j).Item(1).ToString & ")</option>"
                    End If
                Next

                strRow &= "</select>"
                strRow &= "</td>"
                strRow &= "<td>"
                strRow &= "<select class=""ddl"" style=""width:100%; margin-right:0px;"" id=""cboCurrency" & i & """ name=""cboCurrency" & i & """>"
                strRow &= "<option value="""" selected=""selected"">" & "---Select---" & "</option>"

                For j = 0 To dsCurr.Tables(0).Rows.Count - 1
                    If aryVenCode(i)(2) = dsCurr.Tables(0).Rows(j).Item(0).ToString Then
                        strRow &= "<option value=""" & dsCurr.Tables(0).Rows(j).Item(0).ToString & """ selected=""selected"">" & dsCurr.Tables(0).Rows(j).Item(1).ToString & "</option>"
                    Else
                        strRow &= "<option value=""" & dsCurr.Tables(0).Rows(j).Item(0).ToString & """>" & dsCurr.Tables(0).Rows(j).Item(1).ToString & "</option>"
                    End If
                Next

                strRow &= "</select>"
                strRow &= "</td>"
            End If
        Next

        strTable = "<table class=""grid"" style=""margin-top:10px; border-collapse:collapse; line-height:20px; "" >" & _
                "<tr class=""TableHeader""><td style=""width:100px;"">Vendor Code</td><td style=""width:100px;"">Delivery Term</td><td style=""width:60px;"">Currency</td></tr>" & _
                strRow & _
                "</table>"

        Session("ConstructTable") = strTable

    End Sub

    Private Sub cmd_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_Add.Click

        BuildLine()

    End Sub

    Private Sub BuildLine()
        Dim i As Integer

        aryVenCode = Session("arySetVenCode")
        For i = 0 To aryVenCode.Count - 1
            If aryVenCode(i)(3) = lblVenID.Text Then
                aryVenCode(i)(0) = Request.Form("txtVenCode" & i)
                aryVenCode(i)(1) = Request.Form("cboDelTerm" & i)
                aryVenCode(i)(2) = Request.Form("cboCurrency" & i)
            End If
        Next

        aryVenCode.Add(New String() {"", "", "", lblVenID.Text, hidType.Text, "Add"})
        Session("arySetVenCode") = aryVenCode

        ConstructTable()

    End Sub

    Private Sub hidButtonClose_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidButtonClose.ServerClick
        RebuildClose()
    End Sub

    Private Sub cmd_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Save.Click
        Rebuild()

        If blnEmpty = False Then
            strMsg = "<ul type='disc'><li>Vendor Code " & objGlo.GetErrorMessage("00001") & "<ul type='disc'></ul></li></ul>"
        End If
        If blnEmpty2 = False Then
            strMsg = "<ul type='disc'><li>Delivery Term " & objGlo.GetErrorMessage("00001") & "<ul type='disc'></ul></li></ul>"
        End If
        If blnEmpty3 = False Then
            strMsg = "<ul type='disc'><li>Currency " & objGlo.GetErrorMessage("00001") & "<ul type='disc'></ul></li></ul>"
        End If
        If blnDup = False Then
            strMsg = "<ul type='disc'><li>Vendor Code - " & objGlo.GetErrorMessage("00002") & "<ul type='disc'></ul></li></ul>"
        End If
        If blnCheck = False Then
            strMsg = "<ul type='disc'><li>Cannot edit Vendor Code " & ViewState("VenCodeErr") & " as it is tie to product item. <ul type='disc'></ul></li></ul>"
            'strMsg = "<ul type='disc'><li>Vendor Code " & objGlo.GetErrorMessage("00283") & "<ul type='disc'></ul></li></ul>"
        End If
        If blnCheck2 = False Then
            strMsg = "<ul type='disc'><li>Vendor Code is not match to Vendor Code for the same Currency existed in system.<ul type='disc'></ul></li></ul>"
        End If
        If blnCheck3 = False Then
            strMsg = "<ul type='disc'><li>" & objGlo.GetErrorMessage("00345") & "<ul type='disc'></ul></li></ul>"
        End If
        If blnCheck4 = False Then
            strMsg = "<ul type='disc'><li>" & objGlo.GetErrorMessage("00364") & "<ul type='disc'></ul></li></ul>"
        End If
        If blnCheck5 = False Then
            strMsg = "<ul type='disc'><li>Vendor Code has already tied to other vendor company.<ul type='disc'></ul></li></ul>"
        End If

        ConstructTable()

        If strMsg <> "" Then
            lblMsg.Text = strMsg
        Else
            BuildSave()
            lblMsg.Text = ""
            Common.NetMsgbox(Me, objGlo.GetErrorMessage("00003"), MsgBoxStyle.Information)
        End If

    End Sub

    Private Sub Rebuild()
        Dim i, j As Integer
        Dim strSql As String
        Dim aryExtVenCode As New ArrayList()
        Dim blnFound As Boolean
        blnEmpty = True
        blnEmpty2 = True
        blnEmpty3 = True
        blnDup = True
        blnDup2 = True
        blnDup3 = True
        blnCheck = True
        blnCheck2 = True
        blnCheck3 = True
        blnCheck4 = True
        blnCheck5 = True
        blnAllTrue = True

        aryVenCode = Session("arySetVenCode")

        For i = 0 To aryVenCode.Count - 1
            If aryVenCode(i)(3) = lblVenID.Text Then
                aryVenCode(i)(0) = Request.Form("txtVenCode" & i)
                aryVenCode(i)(1) = Request.Form("cboDelTerm" & i)
                aryVenCode(i)(2) = Request.Form("cboCurrency" & i)
            End If
        Next

        For i = 0 To aryVenCode.Count - 1
            'Check if Vendor Code is empty
            If aryVenCode(i)(0) = "" And (aryVenCode(i)(1) <> "" Or aryVenCode(i)(2) <> "") Then
                blnEmpty = False
                'blnAllTrue = False
            End If

            'Check if Delivery Term is empty
            If aryVenCode(i)(1) = "" And (aryVenCode(i)(0) <> "" Or aryVenCode(i)(2) <> "") Then
                blnEmpty2 = False
                'blnAllTrue = False
            End If

            'Check if Currency is empty
            If aryVenCode(i)(2) = "" And (aryVenCode(i)(0) <> "" Or aryVenCode(i)(1) <> "") Then
                blnEmpty3 = False
                'blnAllTrue = False
            End If

            'Check if Vendor Code is duplicate
            For j = 0 To aryVenCode.Count - 1
                If j <> i And aryVenCode(j)(0) <> "" And aryVenCode(j)(0) = aryVenCode(i)(0) Then
                    blnDup = False
                    'blnAllTrue = False
                    Exit For
                End If
            Next

            ''Check if Delivery Term is duplicate
            'For j = 0 To aryVenCode.Count - 1
            '    If j <> i And aryVenCode(j)(1) <> "" And aryVenCode(j)(1) = aryVenCode(i)(1) Then
            '        blnDup2 = False
            '        blnAllTrue = False
            '        Exit For
            '    End If
            'Next

            ''Check if Currency is duplicate
            'For j = 0 To aryVenCode.Count - 1
            '    If j <> i And aryVenCode(j)(2) <> "" And aryVenCode(j)(2) = aryVenCode(i)(2) Then
            '        blnDup3 = False
            '        blnAllTrue = False
            '        Exit For
            '    End If
            'Next

            'Check from table whether vendor code / currency is tied to existing item or not
            If aryVenCode(i)(0) <> "" And aryVenCode(i)(2) <> "" Then
                Dim dsChk As New DataSet
                Dim dsChk2 As New DataSet

                strSql = "SELECT PV_SUPP_CODE, PV_CURR FROM PIM_VENDOR PV LEFT JOIN PRODUCT_MSTR PM ON PV.PV_PRODUCT_INDEX = PM.PM_PRODUCT_INDEX " & _
                        "WHERE PM.PM_S_COY_ID = '" & Session("CompanyId") & "' AND PV_SUPP_CODE = '" & Common.Parse(aryVenCode(i)(0)) & "' AND PV_S_COY_ID = '" & lblVenID.Text & "' "
                dsChk = objDb.FillDs(strSql)

                If dsChk.Tables(0).Rows.Count > 0 Then
                    If aryVenCode(i)(2) <> dsChk.Tables(0).Rows(0)("PV_CURR") Then
                        ViewState("VenCodeErr") = aryVenCode(i)(0)
                        blnCheck = False
                        blnAllTrue = False
                    End If
                Else
                    strSql = "SELECT PV_SUPP_CODE, PV_CURR FROM PIM_VENDOR PV LEFT JOIN PRODUCT_MSTR PM ON PV.PV_PRODUCT_INDEX = PM.PM_PRODUCT_INDEX " & _
                            "WHERE PM.PM_S_COY_ID = '" & Session("CompanyId") & "' AND PV_CURR = '" & aryVenCode(i)(2) & "' AND PV_S_COY_ID = '" & lblVenID.Text & "' "
                    dsChk2 = objDb.FillDs(strSql)

                    If dsChk2.Tables(0).Rows.Count > 0 Then
                        blnCheck2 = False
                        blnAllTrue = False
                    End If
                End If
            End If

            'Compare vendor code / currency with other rows
            If aryVenCode(i)(0) <> "" And aryVenCode(i)(2) <> "" Then
                For j = 0 To aryVenCode.Count - 1
                    If i <> j And aryVenCode(j)(0) <> "" And aryVenCode(j)(2) <> "" Then
                        If aryVenCode(i)(0) = aryVenCode(j)(0) And aryVenCode(i)(2) <> aryVenCode(j)(2) Then
                            blnCheck3 = False
                            'blnAllTrue = False
                        ElseIf aryVenCode(i)(0) <> aryVenCode(j)(0) And aryVenCode(i)(2) = aryVenCode(j)(2) Then
                            blnCheck3 = False
                            'blnAllTrue = False
                        End If
                    End If
                Next
            End If

            'If blnFound = False Then
            If aryVenCode(i)(0) <> "" Then
                blnFound = False

                If Not Session("aryVenCode") Is Nothing Then
                    aryExtVenCode = Session("aryVenCode")
                    For j = 0 To aryExtVenCode.Count - 1
                        If (aryExtVenCode(j)(3) <> aryVenCode(i)(3)) And (aryExtVenCode(j)(0) = aryVenCode(i)(0)) Then
                            blnFound = True
                            Exit For
                        End If
                    Next
                End If

                If blnFound = False Then
                    strSql = "SELECT '*' FROM COMPANY_VENDOR_SUPPCODE " & _
                            "WHERE CVS_B_COY_ID = '" & Session("CompanyId") & "' AND CVS_S_COY_ID <> '" & aryVenCode(i)(3) & "' AND CVS_SUPP_CODE = '" & Common.Parse(aryVenCode(i)(0)) & "'"
                    If objDb.FillDs(strSql).Tables(0).Rows.Count > 0 Then
                        blnFound = True
                    End If
                End If

                If blnFound = True Then
                    blnCheck5 = False
                End If
            End If
            'End If

            If (aryVenCode(i)(1) <> "" And aryVenCode(i)(2) <> "") And (aryVenCode(i)(2) <> "MYR" And aryVenCode(i)(1) = "3233") Then
                blnCheck4 = False
            End If
        Next

        Session("arySetVenCode") = aryVenCode

    End Sub

    Private Sub RebuildClose()

        aryVenCode = Nothing
        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        strscript.Append("Close();")
        strscript.Append("</script>")
        RegisterStartupScript("script3", strscript.ToString())

    End Sub

    Private Sub BuildSave()
        Dim i As Integer
        Dim intFound As Integer
        Dim aryExtVenCode As New ArrayList()
        intFound = 0

        If Not Session("aryVenCode") Is Nothing Then
            aryExtVenCode = Session("aryVenCode")
            aryVenCode = Session("arySetVenCode")

            'Remove record in existing array
            Do While i < aryExtVenCode.Count
                If aryExtVenCode(i)(3) = lblVenID.Text Then
                    aryExtVenCode.RemoveAt(i)
                    i = 0
                Else
                    i = i + 1
                End If
            Loop

            For i = 0 To aryVenCode.Count - 1
                If aryVenCode(i)(0) <> "" And aryVenCode(i)(1) <> "" And aryVenCode(i)(2) <> "" Then
                    aryExtVenCode.Add(New String() {aryVenCode(i)(0), aryVenCode(i)(1), aryVenCode(i)(2), lblVenID.Text, hidType.Text, "Add"})
                End If
            Next

            Session("aryVenCode") = aryExtVenCode
        Else
            'arySetVenCode = Session("arySetVenCode")
            For i = 0 To aryVenCode.Count - 1
                If aryVenCode(i)(0) <> "" And aryVenCode(i)(1) <> "" And aryVenCode(i)(2) <> "" Then
                    aryExtVenCode.Add(New String() {aryVenCode(i)(0), aryVenCode(i)(1), aryVenCode(i)(2), lblVenID.Text, hidType.Text, "Add"})
                End If
            Next
            Session("aryVenCode") = aryExtVenCode
        End If

    End Sub
End Class
