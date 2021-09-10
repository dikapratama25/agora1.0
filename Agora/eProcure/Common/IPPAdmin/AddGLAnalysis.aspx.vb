Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions
Imports System.Drawing

Public Class AddGLAnalysis
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    'Dim objInv As New Inventory
    Dim objIPPMain As New IPPMain
    Dim objGlo As New AppGlobals
    Dim objDb As New EAD.DBCom
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAddGL As System.Web.UI.WebControls.Button
    Protected WithEvents cmdAddRule As System.Web.UI.WebControls.Button
    Protected WithEvents cmdClose As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidButton As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidButtonClose As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtRuleCode As System.Web.UI.WebControls.TextBox

    Dim strMsg As String = ""
    Dim intEmpty As Boolean = True
    Dim intDup As Boolean = True
    Dim intDup2 As Boolean = True
    Dim intFound As Boolean = True
    Dim intFound2 As Boolean = True
    Dim intCheck As Boolean = True
    Dim intCheck2 As Boolean = True
    Dim intCheck3 As Boolean = True
    Dim intCheck4 As Boolean = True
    Dim intCheck5 As Boolean = True
    Dim intCheck6 As Boolean = True
    Dim aryNewRule As New ArrayList()
    Dim aryNewGL As New ArrayList()
    Dim strErrRC, strErrRetRC, strErrGL, strErrGL2, strErrRetGL As String

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
        'MyBase.Page_Load(sender, e)

        If Not Page.IsPostBack Then
            Session("aryNewRule") = Nothing
            Session("aryNewGL") = Nothing
            ViewState("ruleindex") = Request.QueryString("ruleindex")
            ViewState("mode") = Request.QueryString("mode")
            ViewState("Row") = 5

            BuildRow()
            ConstructTableRule()
            ConstructTableGL()
        End If

        cmdClose.Attributes.Add("onClick", "window.close();")
        lblMsg.Text = ""
    End Sub

    Private Function BuildRow()
        Dim i, Count As Integer
        Dim dsRule As DataSet

        If ViewState("mode") = "new" Then
            lblTitle.Text = "Add New GL Analysis Rule"
            txtRuleCode.Enabled = True

            For i = 0 To ViewState("Row") - 1
                aryNewRule.Add(New String() {"", "", "Y", ""})
                aryNewGL.Add(New String() {"", ""})
            Next
        Else
            aryNewRule.Clear()
            aryNewGL.Clear()

            dsRule = objIPPMain.GetGLRuleCodeInfo(ViewState("ruleindex"))

            'Sub Description
            lblTitle.Text = "Modify GL Analysis Rule"
            txtRuleCode.Text = dsRule.Tables("MSTR").Rows(0)("IG_GLRULE_CODE")
            'txtRuleCode.Enabled = False

            If dsRule.Tables("RULE").Rows.Count > ViewState("Row") Then
                Count = dsRule.Tables("RULE").Rows.Count
            Else
                Count = ViewState("Row")
            End If

            For i = 0 To Count - 1
                aryNewRule.Add(New String() {"", "", "", ""})
            Next

            For i = 0 To dsRule.Tables("RULE").Rows.Count - 1
                aryNewRule(i)(0) = dsRule.Tables("RULE").Rows(i)("IGC_GLRULE_CATEGORY")
                aryNewRule(i)(1) = dsRule.Tables("RULE").Rows(i)("IGC_GLRULE_CATEGORY_REMARK")
                aryNewRule(i)(2) = dsRule.Tables("RULE").Rows(i)("IGC_GLRULE_CATEGORY_ACTIVE")
                aryNewRule(i)(3) = dsRule.Tables("RULE").Rows(i)("IGC_GLRULE_CATEGORY_INDEX")
            Next


            'GL Code
            If dsRule.Tables("GL").Rows.Count > ViewState("Row") Then
                Count = dsRule.Tables("GL").Rows.Count
            Else
                Count = ViewState("Row")
            End If

            For i = 0 To Count - 1
                aryNewGL.Add(New String() {"", ""})
            Next

            For i = 0 To dsRule.Tables("GL").Rows.Count - 1
                aryNewGL(i)(0) = dsRule.Tables("GL").Rows(i)("IGG_GL_CODE")
                aryNewGL(i)(1) = dsRule.Tables("GL").Rows(i)("IGG_GL_DESC")
            Next

        End If

    End Function

    Private Sub ConstructTableRule()
        Dim table, strrow As String
        Dim i, count As Integer
        Dim blnChk As Boolean = True
        count = aryNewRule.Count

        For i = 0 To count - 1
            strrow &= "<tr>"
            strrow &= "<td class=""tablecol"">"
            'strrow &= "<input type=""hidden"" id=""hidIndex" & i & """ name=""hidIndex" & i & """ value=""" & aryNewRule(i)(3) & "> """
            strrow &= "<textarea rows=""2"" style=""font-family:Arial,Verdana;font-size:11px;width:100%;margin-right:0px; "" maxlength=""500"" id=""txtRule" & i & """ name=""txtRule" & i & """>" & aryNewRule(i)(0) & "</textarea>"
            'strrow &= "<input style=""width:100%;margin-right:0px; "" maxlength=""500"" class=""txtbox"" type=""text"" id=""txtRule" & i & """ name=""txtRule" & i & """ value=""" & aryNewRule(i)(0) & """>"
            strrow &= "</td>"

            strrow &= "<td class=""tablecol"">"
            strrow &= "<textarea rows=""2"" style=""font-family:Arial,Verdana;font-size:11px;width:100%;margin-right:0px; "" maxlength=""500"" id=""txtRemark" & i & """ name=""txtRemark" & i & """>" & aryNewRule(i)(1) & "</textarea>"
            'strrow &= "<input style=""width:100%;margin-right:0px; "" maxlength=""500"" class=""txtbox"" type=""text"" id=""txtRemark" & i & """ name=""txtRemark" & i & """ value=""" & aryNewRule(i)(1) & """>"
            strrow &= "</td>"

            strrow &= "<td align=""center"" class=""tablecol"">"
            If aryNewRule(i)(2) = "Y" Then
                strrow &= "<input type=""checkbox"" id=""chkSelection" & i & """ checked name=""chkSelection" & i & """ onclick=""checkChild('chkSelection" & i & "')"">"
            Else
                strrow &= "<input type=""checkbox"" id=""chkSelection" & i & """ name=""chkSelection" & i & """ onclick=""checkChild('chkSelection" & i & "')"">"
                blnChk = False
            End If
            strrow &= "</td>"
            strrow &= "</tr>"

        Next

        If blnChk = True Then
            table = "<table class=""alltable"" cellspacing=""0"" cellpadding=""0"" border=""0"">" & _
                           "<tr><td class=""TableHeader"" margin-right:0px;"" colspan=""3"">Description Setup</td></tr>" & _
                           "<tr><td class=""tablecol"" style=""width:45%;margin-right:0px;""><strong>Sub Description</strong></td><td class=""tablecol"" style=""width:45%; margin-right:0px;""><strong>Remark</strong></td><td class=""tablecol"" width=""10%"" align=""center""><strong>Active</strong><br/><input type=""checkbox"" id=""chkAll"" checked name=""chkAll"" onclick=""selectAll();""></td></tr>" & _
                           strrow & _
                           "</table>"
        Else
            table = "<table class=""alltable"" cellspacing=""0"" cellpadding=""0"" border=""0"">" & _
                           "<tr><td class=""TableHeader"" margin-right:0px;"" colspan=""3"">Description Setup</td></tr>" & _
                           "<tr><td class=""tablecol"" style=""width:45%;margin-right:0px;""><strong>Sub Description</strong></td><td class=""tablecol"" style=""width:45%; margin-right:0px;""><strong>Remark</strong></td><td class=""tablecol"" width=""10%"" align=""center""><strong>Active</strong><br/><input type=""checkbox"" id=""chkAll"" name=""chkAll"" onclick=""selectAll();""></td></tr>" & _
                           strrow & _
                           "</table>"
        End If
        
        Session("ConstructTableRule") = table
        Session("aryNewRule") = aryNewRule
    End Sub

    Private Sub ConstructTableGL()
        Dim table, strrow As String
        Dim i, count As Integer

        count = aryNewGL.Count

        For i = 0 To count - 1
            strrow &= "<tr style=""background-color:#fdfdfd;"">"
            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:100%;margin-right:0px; ""  class=""txtbox"" type=""text"" id=""txtGLCode" & i & """ name=""txtGLCode" & i & """ value=""" & aryNewGL(i)(0) & """>"
            strrow &= "</td>"

            strrow &= "<td class=""tablecol"">"
            strrow &= "<input style=""width:100%;margin-right:0px; "" class=""txtbox"" type=""text"" id=""txtDesc" & i & """ readonly name=""txtDesc" & i & """ value=""" & aryNewGL(i)(1) & """>"
            strrow &= "</td>"
            strrow &= "</tr>"
        Next

        table = "<table class=""grid"" style=""margin-top:10px; border-collapse:collapse; line-height:20px; "" >" & _
               "<tr class=""TableHeader""><td margin-right:0px;"" colspan=""2"">GL Code Setup</td></tr>" & _
               "<tr><td class=""tablecol"" style=""width:40%;margin-right:0px;""><strong>GL Code</strong></td><td class=""tablecol"" style=""width:60%; margin-right:0px;""><strong>Description</strong></td></tr>" & _
               strrow & _
               "</table>"

        Session("ConstructTableGL") = table
        Session("aryNewGL") = aryNewGL
        Populate()
    End Sub

    Sub Populate()
        'Type Ahead
        Dim typeahead As String
        Dim count As Integer
        count = aryNewGL.Count
        Dim i As Integer
        Dim content, content2 As String
        Dim nametypeahead As String

        nametypeahead = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=GLCode2")
        For i = 0 To count - 1
            content &= "$(""#txtGLCode" & i & """).autocomplete(""" & nametypeahead & """, {" & vbCrLf & _
            "width: 180," & vbCrLf & _
            "scroll: true," & vbCrLf & _
            "selectFirst: false" & vbCrLf & _
            "});" & vbCrLf & _
            "$(""#txtGLCode" & i & """).result(function(event,data,item) " & vbCrLf & _
            "{" & vbCrLf & _
            "if (data)" & vbCrLf & _
            "$(""#txtDesc" & i & """).val(data[1]);" & vbCrLf & _
            "});" & vbCrLf & _
            "$(""#txtGLCode" & i & """).keyup(function() {" & vbCrLf & _
            "if ($(""#txtGLCode" & i & """).val() == """") {" & vbCrLf & _
            "$(""#txtDesc" & i & """).val(""""); }" & vbCrLf & _
            "});" & vbCrLf & _
            "$(""#chkSelection" & i & """).click(function() {" & vbCrLf & _
            "if ($(""#txtRule" & i & """).val() != """" && $(""#chkSelection" & i & """).attr(""checked"")==false) {" & vbCrLf & _
            "var a=$(""#txtRule" & i & """).val();" & vbCrLf & _
            "var r=confirm(""Are you sure that you want to deactivate this Sub Description ("" + a + "")?"");" & vbCrLf & _
            "if (r==false) {" & vbCrLf & _
            "$(""#chkSelection" & i & """).attr(""checked"",""checked""); " & vbCrLf & _
            "checkChild(""chkSelection" & i & """); }}" & vbCrLf & _
            "});" & vbCrLf
        Next

        content2 &= "$(""#chkAll"").click(function() {" & vbCrLf & _
            "if ($(""#chkAll"").attr(""checked"")==false) {" & vbCrLf & _
            "var r=confirm(""Are you sure that you want to deactivate all Sub Description ?"");" & vbCrLf & _
            "if (r==false) {" & vbCrLf & _
            "$(""#chkAll"").attr(""checked"",""checked""); " & vbCrLf & _
            "selectAll();; }}" & vbCrLf & _
            "});" & vbCrLf

        typeahead = "<script language=""javascript"">" & vbCrLf & _
          "<!--" & vbCrLf & _
            "$(document).ready(function(){" & vbCrLf & _
            content & vbCrLf & _
            content2 & vbCrLf & _
            "});" & vbCrLf & _
            "-->" & vbCrLf & _
            "</script>"
        Session("typeahead") = typeahead
    End Sub

    Private Sub cmdAddRule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddRule.Click
        Rebuild(False)
        BuildLine("RC")
        ConstructTableRule()
        ConstructTableGL()
    End Sub

    Private Sub cmdAddGL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddGL.Click
        Rebuild(False)
        BuildLine("GL")
        ConstructTableRule()
        ConstructTableGL()
    End Sub

    Private Function Rebuild(Optional ByVal blnChk As Boolean = True)
        Dim i, j As Integer
        Dim strRetVal As String
        intDup = False
        intDup2 = False
        intEmpty = False
        intFound = False
        intFound2 = False
        intCheck = False
        intCheck2 = False
        intCheck3 = False
        intCheck4 = False
        intCheck5 = False
        intCheck6 = False

        strErrRC = ""
        strErrGL = ""
        strErrGL2 = ""

        aryNewRule = Session("aryNewRule")
        For i = 0 To aryNewRule.Count - 1
            aryNewRule(i)(0) = Request.Form("txtRule" & i)
            aryNewRule(i)(1) = Request.Form("txtRemark" & i)
            aryNewRule(i)(2) = IIf(Request.Form("chkSelection" & i) = "on", "Y", "N")
        Next

        aryNewGL = Session("aryNewGL")
        For i = 0 To aryNewGL.Count - 1
            aryNewGL(i)(0) = Request.Form("txtGLCode" & i)
            aryNewGL(i)(1) = Request.Form("txtDesc" & i)
        Next

        If blnChk = True Then
            'Sub Description
            For i = 0 To aryNewRule.Count - 1
                If aryNewRule(i)(0) <> "" Then
                    'Check Duplicate Sub Description
                    If intDup = False Then
                        For j = 0 To aryNewRule.Count - 1
                            If i <> j And aryNewRule(j)(0) <> "" And aryNewRule(i)(0) = aryNewRule(j)(0) Then
                                intDup = True
                                Exit For
                            End If
                        Next
                    End If

                    'Check Sub Description Max Length
                    If aryNewRule(i)(0) <> "" And aryNewRule(i)(0).ToString.Length > 500 Then
                        intCheck = True
                    End If

                    'Check Remark Max Length
                    If aryNewRule(i)(1) <> "" And aryNewRule(i)(1).ToString.Length > 500 Then
                        intCheck2 = True
                    End If

                    'Check checkbox at least 1 category is active
                    If intCheck6 = False Then
                        If aryNewRule(i)(2) = "Y" Then
                            intCheck6 = True
                        End If
                    End If
                    
                    If objIPPMain.ChkRuleInfo(ViewState("ruleindex"), aryNewRule(i)(0), "RC", ViewState("mode"), strRetVal) = False Then
                        If strErrRC = "" Then
                            strErrRC = aryNewRule(i)(0)
                            strErrRetRC = strRetVal
                        End If
                        intCheck4 = True
                    End If

                    intFound = True
                Else
                    If aryNewRule(i)(1) <> "" And aryNewRule(i)(0) = "" Then
                        intEmpty = True
                    End If
                End If
            Next

            'GL Code
            For i = 0 To aryNewGL.Count - 1
                If aryNewGL(i)(0) <> "" Then
                    'Check Duplicate GL Code
                    If intDup2 = False Then
                        For j = 0 To aryNewGL.Count - 1
                            If i <> j And aryNewGL(i)(0) = aryNewGL(j)(0) And aryNewGL(j)(0) <> "" Then
                                intDup2 = True
                                Exit For
                            End If
                        Next
                    End If

                    'Check GL Code from DB whether it is exist or not
                    If aryNewGL(i)(0) <> "" Then
                        If objDb.Exist("SELECT '*' FROM COMPANY_B_GL_CODE WHERE CBG_B_COY_ID = '" & Session("CompanyId") & "' AND CBG_STATUS = 'A' AND CBG_B_GL_CODE = '" & Common.Parse(aryNewGL(i)(0)) & "'") = 0 Then
                            If strErrGL = "" Then
                                strErrGL = aryNewGL(i)(0)
                            End If
                            intCheck3 = True
                        End If
                    End If

                    If objIPPMain.ChkRuleInfo(ViewState("ruleindex"), aryNewGL(i)(0), "GL", ViewState("mode"), strRetVal) = False Then
                        If strErrGL2 = "" Then
                            strErrGL2 = aryNewGL(i)(0)
                            strErrRetGL = strRetVal
                        End If
                        intCheck5 = True
                    End If

                    intFound2 = True
                End If
            Next
        End If

    End Function

    Private Function BuildLine(ByVal strMode As String)
        Dim i As Integer
        Dim intFound As Integer
        intFound = 0

        If strMode = "RC" Then
            aryNewRule.Add(New String() {"", "", "Y", ""})
        ElseIf strMode = "GL" Then
            aryNewGL.Add(New String() {"", ""})
        End If
        
    End Function

    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim blnGL As Boolean = True
        Rebuild()

        If Me.txtRuleCode.Text = "" Then
            strMsg = "<ul type='disc'><li>GL Analysis Description Code " & objGlo.GetErrorMessage("00001") & "<ul type='disc'></ul></li></ul>"
        Else
            If ViewState("mode") = "new" Then
                blnGL = objIPPMain.ChkGLRuleCode("", txtRuleCode.Text, "a", strMsg)
            Else
                blnGL = objIPPMain.ChkGLRuleCode(ViewState("ruleindex"), txtRuleCode.Text, "m", strMsg, aryNewRule)
            End If
        End If

        If blnGL = True Then
            'GL Code Checking
            If intDup2 = True Then
                strMsg = "<ul type='disc'><li>" & objGlo.GetErrorMessage("00002") & " (GL Code)<ul type='disc'></ul></li></ul>"
            End If
            If intCheck3 = True Then
                strMsg = "<ul type='disc'><li>" & strErrGL & " - " & objGlo.GetErrorMessage("00353") & "<ul type='disc'></ul></li></ul>"
            End If
            If intCheck5 = True Then
                strMsg = "<ul type='disc'><li>GL Code (" & strErrGL2 & ") is tied to " & strErrRetGL & "<ul type='disc'></ul></li></ul>"
                'strMsg = "<ul type='disc'><li>GL Code (" & strErrGL2 & ") " & objGlo.GetErrorMessage("00372") & "<ul type='disc'></ul></li></ul>"
            End If
            If intFound2 = False Then
                strMsg = "<ul type='disc'><li>GL Code " & objGlo.GetErrorMessage("00001") & "<ul type='disc'></ul></li></ul>"
            End If

            'Category/ Sub Description Checking
            If intDup = True Then
                strMsg = "<ul type='disc'><li>" & objGlo.GetErrorMessage("00002") & " (Sub Description)<ul type='disc'></ul></li></ul>"
            End If
            If intCheck = True Then
                strMsg = "<ul type='disc'><li>The max length of Sub Description is 500.<ul type='disc'></ul></li></ul>"
            End If
            If intCheck2 = True Then
                strMsg = "<ul type='disc'><li>The max length of Remark is 500.<ul type='disc'></ul></li></ul>"
            End If
            If intCheck4 = True Then
                strMsg = "<ul type='disc'><li>Sub Description (" & strErrRC & ") is tied to " & strErrRetRC & "<ul type='disc'></ul></li></ul>"
                'strMsg = "<ul type='disc'><li>Sub Description (" & strErrRC & ") " & objGlo.GetErrorMessage("00372") & "<ul type='disc'></ul></li></ul>"
            End If
            If intCheck6 = False Then
                strMsg = "<ul type='disc'><li>At least one sub description must be in active status.<ul type='disc'></ul></li></ul>"
            End If
            If intEmpty = True Then
                strMsg = "<ul type='disc'><li>Sub Description " & objGlo.GetErrorMessage("00001") & "<ul type='disc'></ul></li></ul>"
            End If
            If intFound = False Then
                strMsg = "<ul type='disc'><li>Sub Description " & objGlo.GetErrorMessage("00001") & "<ul type='disc'></ul></li></ul>"
            End If

            If strMsg <> "" Then
                ConstructTableRule()
                ConstructTableGL()
                lblMsg.Text = strMsg
                Exit Sub
            Else
                lblMsg.Text = ""
                Save()
            End If
        Else
            lblMsg.Text = ""
            ConstructTableRule()
            ConstructTableGL()
            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        End If
        
    End Sub

    Private Function Save()
        Dim strNewIndex As String

        If objIPPMain.SaveGLRuleCode(ViewState("ruleindex"), txtRuleCode.Text, ViewState("mode"), aryNewRule, aryNewGL, strMsg, strNewIndex) Then
            If ViewState("mode") = "new" Then
                ViewState("mode") = "edit"
                ViewState("ruleindex") = strNewIndex
            End If

            BuildRow()
            ConstructTableRule()
            ConstructTableGL()
        End If

        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
    End Function

End Class
