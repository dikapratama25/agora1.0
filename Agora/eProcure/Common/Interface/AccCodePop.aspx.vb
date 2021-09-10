Imports AgoraLegacy
Imports eProcure.Component

Partial Public Class AccCodePop
    Inherits AgoraLegacy.AppBaseClass
    Dim aryDoc As New ArrayList
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim objGlobal As New AppGlobals

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        'SetGridProperty(dtgaccountmapping)
        If Not IsPostBack Then
            'dtgaccountmapping.DataSource = Nothing
            'dtgaccountmapping.DataBind()
            'AddRow(5)

            ConstructTable()            
            PopulateTypeAhead()            
            If Request.QueryString("action") = "edit" Then
                lblHeader.Text = "Modify Account Code Mapping"
            Else
                lblHeader.Text = "Add Account Code Mapping"
            End If
            Me.cmd_back.Attributes.Add("onclick", "window.close();")
        Else
            PopulateTypeAhead()
        End If
    End Sub

    Private Function ConstructTable()
        Dim strrow As String
        Dim i, c, count, Sno As Integer
        Dim table As String
        Dim item2nd As Boolean = False
        Dim found As Boolean = False
        Dim dsDoc_temp As New ArrayList

        buildarydoc()
        Dim _0 = aryDoc
        If Request.QueryString("action") = "edit" Then
            ''get detail from database
            Dim ds As DataSet
            Dim objBC As New BudgetControl
            ds = objBC.GetAccountMapping("", "", "", "", "", "", "", "", Request.QueryString("fromacctindex"), "")
            aryDoc(0)(0) = ds.Tables(0).Rows(0).Item("AM_F_BR_CODE").ToString
            aryDoc(0)(1) = ds.Tables(0).Rows(0).Item("AM_F_GL_CODE").ToString
            aryDoc(0)(2) = ds.Tables(0).Rows(0).Item("AM_F_CC").ToString
            aryDoc(0)(3) = ds.Tables(0).Rows(0).Item("FRMAPCODE").ToString

            'lblHeader.Text = "Modify Account Code Mapping"
            strrow &= "<tr style=""background-color:#fdfdfd;"">"

            ''1) Index no.
            strrow &= "<td align=""right"" visible=""false"">"
            strrow &= "<input type=""hidden""  id=""txtSNo" & i & """ name=""txtSNo" & i & """ value=""" & ds.Tables(0).Rows(0).Item("AM_ACCT_MAP_INDEX").ToString & """ />"
            'strrow &= "<span  class=""lbl"" id=""lblSNo" & i & """ name=""lblSNo" & i & """>" & i + 1 & "</span>" 
            'strrow &= "<input style=""width:1px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtFake" & i & """ name=""txtFake" & i & """ value="""" readonly=""true"" onfocus=""this.blur()"" />"
            strrow &= "</td>"

            ''2) From Branch Code - txtbox
            strrow &= "<td>"
            strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtFromBranchCode" & i & """ name=""txtFromBranchCode" & i & """ value=""" & ds.Tables(0).Rows(0).Item("AM_F_BR_CODE").ToString & """ disabled=""true"" />"
            strrow &= "<input type=""hidden"" id=""hidFromBranchCode" & i & """ value="""" runat=""server"" />"
            strrow &= "<input type=""hidden"" id=""hidFromAcctIndex" & i & """ value=""" & ds.Tables(0).Rows(0).Item("AM_F_ACCT_INDEX").ToString & """ runat=""server"" />"
            strrow &= "</td>"

            ''3) From BR GL Code - auto display
            strrow &= "<td>"
            strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtFromBRGLCode" & i & """ name=""txtFromBRGLCode" & i & """ value=""" & ds.Tables(0).Rows(0).Item("AM_F_GL_CODE").ToString & """ disabled=""true"" />"
            'strrow &= "<asp:Label ID=""txtFromBRGLCode"" runat=""server"" CssClass=""lblInfo"" Text=""" & aryDoc(i)(1) & """></asp:Label>"
            strrow &= "<input type=""hidden"" id=""hidFromBranchCodeVal" & i & """ value="""" runat=""server"" />"
            strrow &= "</td>"

            ''4) From Cost Center - txtbox
            strrow &= "<td>"
            strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtFromCostCenter" & i & """ name=""txtFromCostCenter" & i & """ value=""" & ds.Tables(0).Rows(0).Item("AM_F_CC").ToString & """ disabled=""true"" />"
            strrow &= "<input type=""hidden"" id=""hidFromCostCenter" & i & """ value="""" runat=""server"" />"
            strrow &= "</td>"

            ''5) From Interface Code - auto display
            strrow &= "<td>"
            strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtFromInterfaceCode" & i & """ name=""txtFromInterfaceCode" & i & """ value=""" & ds.Tables(0).Rows(0).Item("FRMAPCODE").ToString & """ disabled=""true"" />"
            strrow &= "<input type=""hidden"" id=""hidFromInterfaceCode" & i & """ value="""" runat=""server"" />"
            strrow &= "</td>"

            ''6) To Branch Code - txtbox
            strrow &= "<td>"
            If ViewState("mode") = "edit" Then
                strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtToBranchCode" & i & """ name=""txtToBranchCode" & i & """ value=""" & aryDoc(i)(4) & """ />"
            Else
                strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtToBranchCode" & i & """ name=""txtToBranchCode" & i & """ value=""" & ds.Tables(0).Rows(0).Item("AM_T_BR_CODE").ToString & """ />"
            End If
            strrow &= "<input type=""hidden"" id=""hidToBranchCode" & i & """ value="""" runat=""server"" />"
            strrow &= "</td>"

            ''7) To BR GL Code - auto display
            strrow &= "<td>"
            If ViewState("mode") = "edit" Then
                strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtToBRGLCode" & i & """ name=""txtToBRGLCode" & i & """ value=""" & aryDoc(i)(5) & """ readonly=""true"" />"
            Else
                strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtToBRGLCode" & i & """ name=""txtToBRGLCode" & i & """ value=""" & ds.Tables(0).Rows(0).Item("AM_T_GL_CODE").ToString & """ readonly=""true"" />"
            End If
            strrow &= "<input type=""hidden"" id=""hidToBRGLCode" & i & """ value="""" runat=""server"" />"
            strrow &= "</td>"

            ''8) To Cost Center - txtbox
            strrow &= "<td>"
            If ViewState("mode") = "edit" Then
                strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtToCostCenter" & i & """ name=""txtToCostCenter" & i & """ value=""" & aryDoc(i)(6) & """ />"
            Else
                strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtToCostCenter" & i & """ name=""txtToCostCenter" & i & """ value=""" & ds.Tables(0).Rows(0).Item("AM_T_CC").ToString & """ />"
            End If
            strrow &= "<input type=""hidden"" id=""hidToCostCenter" & i & """ value="""" runat=""server"" />"
            strrow &= "<input type=""hidden"" id=""hidToAcctIndex" & i & """ value=""" & ds.Tables(0).Rows(0).Item("AM_T_ACCT_INDEX").ToString & """ runat=""server"" />"
            strrow &= "</td>"

            ''9) To Interface Code - auto display 
            strrow &= "<td>"
            If ViewState("mode") = "edit" Then
                strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtToInterfaceCode" & i & """ name=""txtToInterfaceCode" & i & """ value=""" & aryDoc(i)(7) & """ readonly=""true"" />"
            Else
                strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtToInterfaceCode" & i & """ name=""txtToInterfaceCode" & i & """ value=""" & ds.Tables(0).Rows(0).Item("TMAPCODE").ToString & """ readonly=""true"" />"
            End If
            strrow &= "<input type=""hidden"" id=""hidToInterfaceCode" & i & """ value="""" runat=""server"" />"
            strrow &= "</td></tr>"

            table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:985px;"" id=""tblAccountCodeMapping"">" & _
                    "<tr class=""TableHeader"">" & _
                    "<td style=""width:1px;margin-right:0px;"" align=""right"" visible=""false""></td>" & _
                    "<td style=""width:120px;margin-right:0px;"">From Branch Code<span id=""lblUOM"" class=""errormsg"">*</span></td>" & _
                    "<td style=""width:120px;margin-right:0px;"">From BR GL Code</td>" & _
                    "<td style=""width:120px;margin-right:0px;"">From Cost Center<span id=""lblUOM"" class=""errormsg"">*</span></td>" & _
                    "<td style=""width:120px;margin-right:0px;"">From Interface Code</td>" & _
                    "<td style=""width:120px;margin-right:0px;"" bgcolor=""#cccccc"">To Branch Code<span id=""lblUOM"" class=""errormsg"">*</span></td>" & _
                    "<td style=""width:120px;margin-right:0px;"" bgcolor=""#cccccc"">To BR GL Code</td>" & _
                    "<td style=""width:120px;margin-right:0px;"" bgcolor=""#cccccc"">To Cost Center<span id=""lblUOM"" class=""errormsg"">*</span></td>" & _
                    "<td style=""width:120px;margin-right:0px;"" bgcolor=""#cccccc"">To Interface Code</td></tr>" & _
                    strrow & _
                "</table>"

            'Dim strscript As New System.Text.StringBuilder            
            'strscript.Append("<script language=""javascript"">")
            'strscript.Append("document.getElementById('txtToBranchCode""" & i & """').focus();")
            'strscript.Append("</script>")
            'RegisterStartupScript("script1", strscript.ToString())
        Else
            'for new
            'lblHeader.Text = "Add Account Code Mapping"
            count = 5
            Sno = CInt(Request.QueryString("rowcount"))
            For i = 0 To count - 1
                Sno = Sno + 1
                strrow &= "<tr style=""background-color:#fdfdfd;"">"

                ''1) Index no.
                strrow &= "<td align=""right"">"
                strrow &= "<input type=""hidden""  id=""txtSNo" & i & """ name=""txtSNo" & i & """ value=""" & Sno & """>"
                strrow &= "<span  class=""lbl"" id=""lblSNo" & i & """ name=""lblSNo" & i & """>" & i + 1 & "</span>"
                strrow &= "</td>"

                ''2) From Branch Code - txtbox
                strrow &= "<td>"
                strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtFromBranchCode" & i & """ name=""txtFromBranchCode" & i & """ value=""" & aryDoc(i)(0) & """>"
                strrow &= "<input type=""hidden"" id=""hidFromBranchCode" & i & """ value="""" runat=""server"" />"
                strrow &= "<input type=""hidden"" id=""hidFromAcctIndex" & i & """ value="""" runat=""server"" />"
                strrow &= "</td>"

                ''3) From BR GL Code - auto display
                strrow &= "<td>"
                strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtFromBRGLCode" & i & """ name=""txtFromBRGLCode" & i & """ readonly=""true"" value=""" & aryDoc(i)(1) & """>"
                'strrow &= "<asp:Label ID=""txtFromBRGLCode"" runat=""server"" CssClass=""lblInfo"" Text=""" & aryDoc(i)(1) & """></asp:Label>"
                strrow &= "<input type=""hidden"" id=""hidFromBranchCodeVal" & i & """ value="""" runat=""server"" />"
                strrow &= "</td>"

                ''4) From Cost Center - txtbox
                strrow &= "<td>"
                strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtFromCostCenter" & i & """ name=""txtFromCostCenter" & i & """ value=""" & aryDoc(i)(2) & """>"
                strrow &= "<input type=""hidden"" id=""hidFromCostCenter" & i & """ value="""" runat=""server"" />"
                strrow &= "</td>"

                ''5) From Interface Code - auto display
                strrow &= "<td>"
                strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtFromInterfaceCode" & i & """ name=""txtFromInterfaceCode" & i & """ readonly=""true"" value=""" & aryDoc(i)(3) & """>"
                strrow &= "<input type=""hidden"" id=""hidFromInterfaceCode" & i & """ value="""" runat=""server"" />"
                strrow &= "</td>"

                ''6) To Brance Code - txtbox
                strrow &= "<td>"
                strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtToBranchCode" & i & """ name=""txtToBranchCode" & i & """ value=""" & aryDoc(i)(4) & """>"
                strrow &= "<input type=""hidden"" id=""hidToBranchCode" & i & """ value="""" runat=""server"" />"
                strrow &= "</td>"

                ''7) To BR GL Code - auto display
                strrow &= "<td>"
                strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtToBRGLCode" & i & """ name=""txtToBRGLCode" & i & """ readonly=""true"" value=""" & aryDoc(i)(5) & """>"
                strrow &= "<input type=""hidden"" id=""hidToBRGLCode" & i & """ value="""" runat=""server"" />"
                strrow &= "</td>"

                ''8) To Cost Center - txtbox
                strrow &= "<td>"
                strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtToCostCenter" & i & """ name=""txtToCostCenter" & i & """ value=""" & aryDoc(i)(6) & """>"
                strrow &= "<input type=""hidden"" id=""hidToCostCenter" & i & """ value="""" runat=""server"" />"
                strrow &= "<input type=""hidden"" id=""hidToAcctIndex" & i & """ value="""" runat=""server"" />"
                strrow &= "</td>"

                ''9) To Interface Code - auto display 
                strrow &= "<td>"
                strrow &= "<input style=""width:120px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtToInterfaceCode" & i & """ name=""txtToInterfaceCode" & i & """ readonly=""true"" value=""" & aryDoc(i)(7) & """>"
                strrow &= "<input type=""hidden"" id=""hidToInterfaceCode" & i & """ value="""" runat=""server"" />"
                strrow &= "</td>"

            Next

            table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:985px;"" id=""tblAccountCodeMapping"">" & _
         "<tr class=""TableHeader"">" & _
         "<td style=""width:1px;margin-right:0px;"" align=""right""></td>" & _
         "<td style=""width:120px;margin-right:0px;"">From Branch Code<span id=""lblUOM"" class=""errormsg"">*</span></td>" & _
         "<td style=""width:120px;margin-right:0px;"">From BR GL Code</td>" & _
         "<td style=""width:120px;margin-right:0px;"">From Cost Center<span id=""lblUOM"" class=""errormsg"">*</span></td>" & _
         "<td style=""width:120px;margin-right:0px;"">From Interface Code</td>" & _
         "<td style=""width:120px;margin-right:0px;"" bgcolor=""#cccccc"">To Branch Code<span id=""lblUOM"" class=""errormsg"">*</span></td>" & _
         "<td style=""width:120px;margin-right:0px;"" bgcolor=""#cccccc"">To BR GL Code</td>" & _
         "<td style=""width:120px;margin-right:0px;"" bgcolor=""#cccccc"">To Cost Center<span id=""lblUOM"" class=""errormsg"">*</span></td>" & _
         "<td style=""width:120px;margin-right:0px;"" bgcolor=""#cccccc"">To Interface Code</td>" & _
         strrow & _
         "</table>"

        End If

        Session("ConstructTable") = table
    End Function

    Private Function GetValuebasedonTypeAhead(ByVal strValue As String, ByVal strField As String) As String
        Dim objBudgetControl As BudgetControl
        Dim ds As DataSet
        If strField = "br" Then
            ds = objBudgetControl.GetBR_GL_CC(strValue, "", "", "")
            If ds.Tables(0).Rows.Count > 0 Then
                Return ds.Tables(0).Rows(0).Item(1).ToString
            End If
        ElseIf strField = "cc" Then
            ds = objBudgetControl.GetBR_GL_CC("", "", strValue, "")
            If ds.Tables(0).Rows.Count > 0 Then
                Return ds.Tables(0).Rows(0).Item(5).ToString
            End If
        End If
        Return ""
    End Function

    Sub PopulateTypeAhead()
        Dim typeahead As String
        Dim i, count As Integer
        Dim content, content2, validate As String
        Dim strGLCode As String
        'Dim vtypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=IPPCOY")
        Dim brtypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=BRGL&type=br")
        Dim cctypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=BRGL&type=cc")
        
        Dim venidx As String
        Dim objipp As New IPPMain
        If Request.QueryString("action") = "edit" Then
            count = 1
        Else
            count = 10
        End If

        If Request.QueryString("action") = "edit" Then
            For i = 0 To count - 1
                content &= "$(""#txtFromBranchCode" & i & """).autocomplete(""" & brtypeahead & """, {" & vbCrLf & _
                     "width: 180," & vbCrLf & _
                     "scroll: true," & vbCrLf & _
                     "selectFirst: false" & vbCrLf & _
                     "}).result(function(event,data,item) {" & vbCrLf & _
                     "if (data)" & vbCrLf & _
                     "$(""#hidFromBranchCode" & i & """).val(data[0]);" & vbCrLf & _
                     "$(""#hidFromBranchCodeVal" & i & """).val(data[1]);" & vbCrLf & _
                     "});" & vbCrLf & _
                      "$(""#txtFromBranchCode" & i & """).blur(function() {" & vbCrLf & _
                     "var hidfrombranchval = document.getElementById(""hidFromBranchCode" & i & """).value;" & vbCrLf & _
                     "var hidfrombranchval2 = document.getElementById(""hidFromBranchCodeVal" & i & """).value;" & vbCrLf & _
                     "document.getElementById(""txtFromBRGLCode" & i & """).value = hidfrombranchval2;" & vbCrLf & _
                     "if(hidfrombranchval == """")" & vbCrLf & _
                     "{" & vbCrLf & _
                     "$(""#txtFromBranchCode" & i & """).val("""");" & vbCrLf & _
                     "}" & vbCrLf & _
                     "});" & vbCrLf & _
                      "$(""#txtFromBranchCode" & i & """).keyup(function() {" & vbCrLf & _
                     "if ($(""#txtFromBranchCode" & i & """).val() == """") {" & vbCrLf & _
                     "$(""#hidFromBranchCode" & i & """).val(""""); }" & vbCrLf & _
                     "else" & vbCrLf & _
                     "{" & vbCrLf & _
                     "}" & vbCrLf & _
                     "});" & vbCrLf & _
                      "$(""#txtFromBRGLCode" & i & """).blur(function() {" & vbCrLf & _
                     "if ($(""#txtFromBRGLCode" & i & """).val() == """") {" & vbCrLf & _
                     "$(""#hidFromBranchCodeVal" & i & """).val(""""); }" & vbCrLf & _
                     "else" & vbCrLf & _
                     "{" & vbCrLf & _
                      "var changeclick = updateparam(""" & cctypeahead & """,encodeURIComponent($(""#txtFromBranchCode" & i & """).val()),encodeURIComponent($(""#txtFromBRGLCode" & i & """).val()));" & vbCrLf & _
                       "$(""#txtFromCostCenter" & i & """).autocomplete(changeclick, {" & vbCrLf & _
                      "width: 180," & vbCrLf & _
                      "scroll: true," & vbCrLf & _
                      "selectFirst: false" & vbCrLf & _
                      "}).result(function(event,data,item) {" & vbCrLf & _
                      "if (data)" & vbCrLf & _
                      "$(""#hidFromCostCenter" & i & """).val(data[0]);" & vbCrLf & _
                      "$(""#hidFromInterfaceCode" & i & """).val(data[1]);" & vbCrLf & _
                      "});" & vbCrLf & _
                       "$(""#txtFromCostCenter" & i & """).blur(function() {" & vbCrLf & _
                      "var hidfromcostcenter = document.getElementById(""hidFromCostCenter" & i & """).value;" & vbCrLf & _
                      "var hidfrominterfacecode = document.getElementById(""hidFromInterfaceCode" & i & """).value;" & vbCrLf & _
                      "document.getElementById(""txtFromInterfaceCode" & i & """).value = hidfrominterfacecode;" & vbCrLf & _
                      "if(hidfromcostcenter == """")" & vbCrLf & _
                      "{" & vbCrLf & _
                      "$(""#txtFromCostCenter" & i & """).val("""");" & vbCrLf & _
                      "}" & vbCrLf & _
                      "});" & vbCrLf & _
                       "$(""#txtFromCostCenter" & i & """).keyup(function() {" & vbCrLf & _
                      "if ($(""#txtFromCostCenter" & i & """).val() == """") {" & vbCrLf & _
                      "$(""#hidFromCostCenter" & i & """).val(""""); }" & vbCrLf & _
                      "else" & vbCrLf & _
                      "{}" & vbCrLf & _
                      "});" & vbCrLf & _
                       "}" & vbCrLf & _
                     "});" & vbCrLf & _
                       "$(""#txtFromCostCenter" & i & """).autocomplete(updateparam(""" & cctypeahead & """,encodeURIComponent($(""#txtFromBranchCode" & i & """).val()),encodeURIComponent($(""#txtFromBRGLCode" & i & """).val())), {" & vbCrLf & _
                      "width: 180," & vbCrLf & _
                      "scroll: true," & vbCrLf & _
                      "selectFirst: false" & vbCrLf & _
                      "}).result(function(event,data,item) {" & vbCrLf & _
                      "if (data)" & vbCrLf & _
                      "$(""#hidFromCostCenter" & i & """).val(data[0]);" & vbCrLf & _
                      "$(""#hidFromInterfaceCode" & i & """).val(data[1]);" & vbCrLf & _
                      "});" & vbCrLf & _
                       "$(""#txtFromCostCenter" & i & """).blur(function() {" & vbCrLf & _
                      "var hidfromcostcenter = document.getElementById(""hidFromCostCenter" & i & """).value;" & vbCrLf & _
                      "var hidfrominterfacecode = document.getElementById(""hidFromInterfaceCode" & i & """).value;" & vbCrLf & _
                      "document.getElementById(""txtFromInterfaceCode" & i & """).value = hidfrominterfacecode;" & vbCrLf & _
                      "if(hidfromcostcenter == """")" & vbCrLf & _
                      "{" & vbCrLf & _
                      "$(""#txtFromCostCenter" & i & """).val("""");" & vbCrLf & _
                      "}" & vbCrLf & _
                      "});" & vbCrLf & _
                       "$(""#txtFromCostCenter" & i & """).keyup(function() {" & vbCrLf & _
                      "if ($(""#txtFromCostCenter" & i & """).val() == """") {" & vbCrLf & _
                      "$(""#hidFromCostCenter" & i & """).val(""""); }" & vbCrLf & _
                      "});" & vbCrLf & _
                     "$(""#txtToBranchCode" & i & """).autocomplete(""" & brtypeahead & """, {" & vbCrLf & _
                     "width: 180," & vbCrLf & _
                     "scroll: true," & vbCrLf & _
                     "selectFirst: false" & vbCrLf & _
                     "}).result(function(event,data,item) {" & vbCrLf & _
                     "if (data)" & vbCrLf & _
                     "$(""#hidToBranchCode" & i & """).val(data[0]);" & vbCrLf & _
                     "$(""#hidToBRGLCode" & i & """).val(data[1]);" & vbCrLf & _
                     "});" & vbCrLf & _
                      "$(""#txtToBranchCode" & i & """).blur(function() {" & vbCrLf & _
                     "var hidtobranchval = document.getElementById(""hidToBranchCode" & i & """).value;" & vbCrLf & _
                     "var hidtobranchval2 = document.getElementById(""hidToBRGLCode" & i & """).value;" & vbCrLf & _
                     "document.getElementById(""txtToBRGLCode" & i & """).value = hidtobranchval2;" & vbCrLf & _
                     "if(hidtobranchval == """")" & vbCrLf & _
                     "{" & vbCrLf & _
                     "$(""#txtToBranchCode" & i & """).val("""");" & vbCrLf & _
                     "}" & vbCrLf & _
                     "});" & vbCrLf & _
                      "$(""#txtToBranchCode" & i & """).keyup(function() {" & vbCrLf & _
                     "if ($(""#txtToBranchCode" & i & """).val() == """") {" & vbCrLf & _
                     "$(""#hidToBranchCode" & i & """).val(""""); }" & vbCrLf & _
                     "else" & vbCrLf & _
                     "{" & vbCrLf & _
                     "}" & vbCrLf & _
                     "});" & vbCrLf & _
                      "$(""#txtToBRGLCode" & i & """).blur(function() {" & vbCrLf & _
                     "if ($(""#txtToBRGLCode" & i & """).val() == """") {" & vbCrLf & _
                     "$(""#hidToBRGLCode" & i & """).val(""""); }" & vbCrLf & _
                     "else" & vbCrLf & _
                     "{" & vbCrLf & _
                      "var changeclick = updateparam(""" & cctypeahead & """,encodeURIComponent($(""#txtToBranchCode" & i & """).val()),encodeURIComponent($(""#txtToBRGLCode" & i & """).val()));" & vbCrLf & _
                       "$(""#txtToCostCenter" & i & """).autocomplete(changeclick, {" & vbCrLf & _
                      "width: 180," & vbCrLf & _
                      "scroll: true," & vbCrLf & _
                      "selectFirst: false" & vbCrLf & _
                      "}).result(function(event,data,item) {" & vbCrLf & _
                      "if (data)" & vbCrLf & _
                      "$(""#hidToCostCenter" & i & """).val(data[0]);" & vbCrLf & _
                      "$(""#hidToInterfaceCode" & i & """).val(data[1]);" & vbCrLf & _
                      "});" & vbCrLf & _
                       "$(""#txtToCostCenter" & i & """).blur(function() {" & vbCrLf & _
                      "var hidtocostcenter = document.getElementById(""hidToCostCenter" & i & """).value;" & vbCrLf & _
                      "var hidtointerfacecode = document.getElementById(""hidToInterfaceCode" & i & """).value;" & vbCrLf & _
                      "document.getElementById(""txtToInterfaceCode" & i & """).value = hidtointerfacecode;" & vbCrLf & _
                      "if(hidtocostcenter == """")" & vbCrLf & _
                      "{" & vbCrLf & _
                      "$(""#txtToCostCenter" & i & """).val("""");" & vbCrLf & _
                      "}" & vbCrLf & _
                      "});" & vbCrLf & _
                       "$(""#txtToCostCenter" & i & """).keyup(function() {" & vbCrLf & _
                      "if ($(""#txtToCostCenter" & i & """).val() == """") {" & vbCrLf & _
                      "$(""#hidToCostCenter" & i & """).val(""""); }" & vbCrLf & _
                      "else" & vbCrLf & _
                      "{}" & vbCrLf & _
                      "});" & vbCrLf & _
                       "}" & vbCrLf & _
                     "});"
            Next
        Else
                For i = 0 To count - 1
                    content &= "$(""#txtFromBranchCode" & i & """).autocomplete(""" & brtypeahead & """, {" & vbCrLf & _
                          "width: 180," & vbCrLf & _
                          "scroll: true," & vbCrLf & _
                          "selectFirst: false" & vbCrLf & _
                          "}).result(function(event,data,item) {" & vbCrLf & _
                          "if (data)" & vbCrLf & _
                          "$(""#hidFromBranchCode" & i & """).val(data[0]);" & vbCrLf & _
                          "$(""#hidFromBranchCodeVal" & i & """).val(data[1]);" & vbCrLf & _
                          "});" & vbCrLf & _
                           "$(""#txtFromBranchCode" & i & """).blur(function() {" & vbCrLf & _
                          "var hidfrombranchval = document.getElementById(""hidFromBranchCode" & i & """).value;" & vbCrLf & _
                          "var hidfrombranchval2 = document.getElementById(""hidFromBranchCodeVal" & i & """).value;" & vbCrLf & _
                          "document.getElementById(""txtFromBRGLCode" & i & """).value = hidfrombranchval2;" & vbCrLf & _
                          "if(hidfrombranchval == """")" & vbCrLf & _
                          "{" & vbCrLf & _
                          "$(""#txtFromBranchCode" & i & """).val("""");" & vbCrLf & _
                          "}" & vbCrLf & _
                          "});" & vbCrLf & _
                           "$(""#txtFromBranchCode" & i & """).keyup(function() {" & vbCrLf & _
                          "if ($(""#txtFromBranchCode" & i & """).val() == """") {" & vbCrLf & _
                          "$(""#hidFromBranchCode" & i & """).val(""""); }" & vbCrLf & _
                          "else" & vbCrLf & _
                          "{" & vbCrLf & _
                          "}" & vbCrLf & _
                          "});" & vbCrLf & _
                           "$(""#txtFromBRGLCode" & i & """).blur(function() {" & vbCrLf & _
                          "if ($(""#txtFromBRGLCode" & i & """).val() == """") {" & vbCrLf & _
                          "$(""#hidFromBranchCodeVal" & i & """).val(""""); }" & vbCrLf & _
                          "else" & vbCrLf & _
                          "{" & vbCrLf & _
                           "var changeclick = updateparam(""" & cctypeahead & """,encodeURIComponent($(""#txtFromBranchCode" & i & """).val()),encodeURIComponent($(""#txtFromBRGLCode" & i & """).val()));" & vbCrLf & _
                            "$(""#txtFromCostCenter" & i & """).autocomplete(changeclick, {" & vbCrLf & _
                           "width: 180," & vbCrLf & _
                           "scroll: true," & vbCrLf & _
                           "selectFirst: false" & vbCrLf & _
                           "}).result(function(event,data,item) {" & vbCrLf & _
                           "if (data)" & vbCrLf & _
                           "$(""#hidFromCostCenter" & i & """).val(data[0]);" & vbCrLf & _
                           "$(""#hidFromInterfaceCode" & i & """).val(data[1]);" & vbCrLf & _
                           "});" & vbCrLf & _
                            "$(""#txtFromCostCenter" & i & """).blur(function() {" & vbCrLf & _
                           "var hidfromcostcenter = document.getElementById(""hidFromCostCenter" & i & """).value;" & vbCrLf & _
                           "var hidfrominterfacecode = document.getElementById(""hidFromInterfaceCode" & i & """).value;" & vbCrLf & _
                           "document.getElementById(""txtFromInterfaceCode" & i & """).value = hidfrominterfacecode;" & vbCrLf & _
                           "if(hidfromcostcenter == """")" & vbCrLf & _
                           "{" & vbCrLf & _
                           "$(""#txtFromCostCenter" & i & """).val("""");" & vbCrLf & _
                           "}" & vbCrLf & _
                           "});" & vbCrLf & _
                            "$(""#txtFromCostCenter" & i & """).keyup(function() {" & vbCrLf & _
                           "if ($(""#txtFromCostCenter" & i & """).val() == """") {" & vbCrLf & _
                           "$(""#hidFromCostCenter" & i & """).val(""""); }" & vbCrLf & _
                           "else" & vbCrLf & _
                           "{}" & vbCrLf & _
                           "});" & vbCrLf & _
                            "}" & vbCrLf & _
                          "});" & vbCrLf & _
                            "$(""#txtFromCostCenter" & i & """).autocomplete(updateparam(""" & cctypeahead & """,encodeURIComponent($(""#txtFromBranchCode" & i & """).val()),encodeURIComponent($(""#txtFromBRGLCode" & i & """).val())), {" & vbCrLf & _
                           "width: 180," & vbCrLf & _
                           "scroll: true," & vbCrLf & _
                           "selectFirst: false" & vbCrLf & _
                           "}).result(function(event,data,item) {" & vbCrLf & _
                           "if (data)" & vbCrLf & _
                           "$(""#hidFromCostCenter" & i & """).val(data[0]);" & vbCrLf & _
                           "$(""#hidFromInterfaceCode" & i & """).val(data[1]);" & vbCrLf & _
                           "});" & vbCrLf & _
                            "$(""#txtFromCostCenter" & i & """).blur(function() {" & vbCrLf & _
                           "var hidfromcostcenter = document.getElementById(""hidFromCostCenter" & i & """).value;" & vbCrLf & _
                           "var hidfrominterfacecode = document.getElementById(""hidFromInterfaceCode" & i & """).value;" & vbCrLf & _
                           "document.getElementById(""txtFromInterfaceCode" & i & """).value = hidfrominterfacecode;" & vbCrLf & _
                           "if(hidfromcostcenter == """")" & vbCrLf & _
                           "{" & vbCrLf & _
                           "$(""#txtFromCostCenter" & i & """).val("""");" & vbCrLf & _
                           "}" & vbCrLf & _
                           "});" & vbCrLf & _
                            "$(""#txtFromCostCenter" & i & """).keyup(function() {" & vbCrLf & _
                           "if ($(""#txtFromCostCenter" & i & """).val() == """") {" & vbCrLf & _
                           "$(""#hidFromCostCenter" & i & """).val(""""); }" & vbCrLf & _
                           "});" & vbCrLf & _
                          "$(""#txtToBranchCode" & i & """).autocomplete(""" & brtypeahead & """, {" & vbCrLf & _
                          "width: 180," & vbCrLf & _
                          "scroll: true," & vbCrLf & _
                          "selectFirst: false" & vbCrLf & _
                          "}).result(function(event,data,item) {" & vbCrLf & _
                          "if (data)" & vbCrLf & _
                          "$(""#hidToBranchCode" & i & """).val(data[0]);" & vbCrLf & _
                          "$(""#hidToBRGLCode" & i & """).val(data[1]);" & vbCrLf & _
                          "});" & vbCrLf & _
                           "$(""#txtToBranchCode" & i & """).blur(function() {" & vbCrLf & _
                          "var hidtobranchval = document.getElementById(""hidToBranchCode" & i & """).value;" & vbCrLf & _
                          "var hidtobranchval2 = document.getElementById(""hidToBRGLCode" & i & """).value;" & vbCrLf & _
                          "document.getElementById(""txtToBRGLCode" & i & """).value = hidtobranchval2;" & vbCrLf & _
                          "if(hidtobranchval == """")" & vbCrLf & _
                          "{" & vbCrLf & _
                          "$(""#txtToBranchCode" & i & """).val("""");" & vbCrLf & _
                          "}" & vbCrLf & _
                          "});" & vbCrLf & _
                           "$(""#txtToBranchCode" & i & """).keyup(function() {" & vbCrLf & _
                          "if ($(""#txtToBranchCode" & i & """).val() == """") {" & vbCrLf & _
                          "$(""#hidToBranchCode" & i & """).val(""""); }" & vbCrLf & _
                          "else" & vbCrLf & _
                          "{" & vbCrLf & _
                          "}" & vbCrLf & _
                          "});" & vbCrLf & _
                           "$(""#txtToBRGLCode" & i & """).blur(function() {" & vbCrLf & _
                          "if ($(""#txtToBRGLCode" & i & """).val() == """") {" & vbCrLf & _
                          "$(""#hidToBRGLCode" & i & """).val(""""); }" & vbCrLf & _
                          "else" & vbCrLf & _
                          "{" & vbCrLf & _
                           "var changeclick = updateparam(""" & cctypeahead & """,encodeURIComponent($(""#txtToBranchCode" & i & """).val()),encodeURIComponent($(""#txtToBRGLCode" & i & """).val()));" & vbCrLf & _
                            "$(""#txtToCostCenter" & i & """).autocomplete(changeclick, {" & vbCrLf & _
                           "width: 180," & vbCrLf & _
                           "scroll: true," & vbCrLf & _
                           "selectFirst: false" & vbCrLf & _
                           "}).result(function(event,data,item) {" & vbCrLf & _
                           "if (data)" & vbCrLf & _
                           "$(""#hidToCostCenter" & i & """).val(data[0]);" & vbCrLf & _
                           "$(""#hidToInterfaceCode" & i & """).val(data[1]);" & vbCrLf & _
                           "});" & vbCrLf & _
                            "$(""#txtToCostCenter" & i & """).blur(function() {" & vbCrLf & _
                           "var hidtocostcenter = document.getElementById(""hidToCostCenter" & i & """).value;" & vbCrLf & _
                           "var hidtointerfacecode = document.getElementById(""hidToInterfaceCode" & i & """).value;" & vbCrLf & _
                           "document.getElementById(""txtToInterfaceCode" & i & """).value = hidtointerfacecode;" & vbCrLf & _
                           "if(hidtocostcenter == """")" & vbCrLf & _
                           "{" & vbCrLf & _
                           "$(""#txtToCostCenter" & i & """).val("""");" & vbCrLf & _
                           "}" & vbCrLf & _
                           "});" & vbCrLf & _
                            "$(""#txtToCostCenter" & i & """).keyup(function() {" & vbCrLf & _
                           "if ($(""#txtToCostCenter" & i & """).val() == """") {" & vbCrLf & _
                           "$(""#hidToCostCenter" & i & """).val(""""); }" & vbCrLf & _
                           "else" & vbCrLf & _
                           "{}" & vbCrLf & _
                           "});" & vbCrLf & _
                            "}" & vbCrLf & _
                          "});" & vbCrLf & _
                            "$(""#txtToCostCenter" & i & """).autocomplete(updateparam(""" & cctypeahead & """,encodeURIComponent($(""#txtToBranchCode" & i & """).val()),encodeURIComponent($(""#txtToBRGLCode" & i & """).val())), {" & vbCrLf & _
                           "width: 180," & vbCrLf & _
                           "scroll: true," & vbCrLf & _
                           "selectFirst: false" & vbCrLf & _
                           "}).result(function(event,data,item) {" & vbCrLf & _
                           "if (data)" & vbCrLf & _
                           "$(""#hidToCostCenter" & i & """).val(data[0]);" & vbCrLf & _
                           "$(""#hidToInterfaceCode" & i & """).val(data[1]);" & vbCrLf & _
                           "});" & vbCrLf & _
                            "$(""#txtToCostCenter" & i & """).blur(function() {" & vbCrLf & _
                           "var hidtocostcenter = document.getElementById(""hidToCostCenter" & i & """).value;" & vbCrLf & _
                           "var hidtointerfacecode = document.getElementById(""hidToInterfaceCode" & i & """).value;" & vbCrLf & _
                           "document.getElementById(""txtToInterfaceCode" & i & """).value = hidtointerfacecode;" & vbCrLf & _
                           "if(hidtocostcenter == """")" & vbCrLf & _
                           "{" & vbCrLf & _
                           "$(""#txtToCostCenter" & i & """).val("""");" & vbCrLf & _
                           "}" & vbCrLf & _
                           "});" & vbCrLf & _
                            "$(""#txtToCostCenter" & i & """).keyup(function() {" & vbCrLf & _
                           "if ($(""#txtToCostCenter" & i & """).val() == """") {" & vbCrLf & _
                           "$(""#hidToCostCenter" & i & """).val(""""); }" & vbCrLf & _
                           "else" & vbCrLf & _
                           "{}" & vbCrLf & _
                           "});"

                Next
        End If
        
        content = content & "$(""#Form1"").submit(function() {" & vbCrLf & _
        validate & vbCrLf & _
        "});" & vbCrLf

        '  "}).result(function(event, item) {" & vbCrLf & _
        '"$(""#txtTemp"").focus();" & vbCrLf & _
        If Request.QueryString("action") = "edit" Then
            typeahead = "<script language=""javascript"">" & vbCrLf & _
                      "<!--" & vbCrLf & _
                        "$(document).ready(function(){" & vbCrLf & _
                        content & vbCrLf & _
                        content2 & vbCrLf & _
                        "});" & vbCrLf & _
                        "-->" & vbCrLf & _
                        "</script>"
        Else
            typeahead = "<script language=""javascript"">" & vbCrLf & _
          "<!--" & vbCrLf & _
            "$(document).ready(function(){" & vbCrLf & _
            content & vbCrLf & _
            "});" & vbCrLf & _
            "-->" & vbCrLf & _
            "</script>"
        End If

        Session("typeahead") = typeahead
        Session("arydoc") = aryDoc        
        '"$(""#btnSelCostAlloc"").trigger('click');" & vbCrLf & _
    End Sub

    Sub buildarydoc()
        Dim i As Integer
        For i = 0 To 4
            'If Request.Form("txtGLCode" & i) = Nothing Or Request.Form("txtGLCode" & i) = "" Then
            aryDoc.Add(New String() {Request.Form("txtFromBranchCode" & i), Request.Form("txtFromBRGLCode" & i), Request.Form("txtFromCostCenter" & i), Request.Form("txtFromInterfaceCode" & i), Request.Form("txtToBranchCode" & i), Request.Form("txtToBRGLCode" & i), Request.Form("txtToCostCenter" & i), Request.Form("txtToInterfaceCode" & i), Request.Form("txtSNo" & i)})
            'Else

            '    aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), Request.Form("txtQty" & i), Request.Form("txtUnitPrice" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i).Substring(0, InStr(Request.Form("txtGLCode" & i), ":") - 1), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), Request.Form("txtCostAlloc" & i), Request.Form("txtBranch" & i), Request.Form("txtCC" & i), "", "", "", ""})
            'End If

        Next
    End Sub

    Private Sub cmd_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd_Save.Click
        Dim strscript As New System.Text.StringBuilder
        Dim objBC As New BudgetControl
        Dim strMsg As String

        ConstructTable()
        PopulateTypeAhead()
        If ValidateInput() Then
            If Request.QueryString("action") = "edit" Then
                ViewState("mode") = "edit"

                If objBC.UpdateAccountMapping(aryDoc) Then
                    strMsg = objGlobal.GetErrorMessage("00003")
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                End If
                'Response.Write("<script language=""javascript"">window.close();</script>")
            Else
                If objBC.SaveAccountMapping(aryDoc) Then
                    strMsg = objGlobal.GetErrorMessage("00003")
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                Else
                    strMsg = objGlobal.GetErrorMessage("00002")
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                End If
                'Response.Write("<script language=""javascript"">window.close();</script>")
            End If
            ConstructTable()
        End If
    End Sub

    Private Function ValidateInput() As Boolean
        Dim objBC As New BudgetControl
        Dim objDB As New EAD.DBCom
        Dim ds As DataSet
        Dim i, iRow As Integer
        Dim strMsg As String

        For i = 0 To aryDoc.Count - 1
            ''Check fields
            If aryDoc.Item(i)(0) <> "" Or aryDoc.Item(i)(2) <> "" Or aryDoc.Item(i)(4) <> "" Or aryDoc.Item(i)(6) <> "" Then
                If aryDoc.Item(i)(0) = "" Then
                    strMsg = objGlobal.GetErrorMessage("00001")
                    strMsg = "From Branch Code " & strMsg
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Return False
                ElseIf aryDoc.Item(i)(1) = "" Then
                    strMsg = objGlobal.GetErrorMessage("00001")
                    strMsg = "From BR GL Code " & strMsg
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Return False
                ElseIf aryDoc.Item(i)(2) = "" Then
                    strMsg = objGlobal.GetErrorMessage("00001")
                    strMsg = "From Cost Center " & strMsg
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Return False
                ElseIf aryDoc.Item(i)(3) = "" Then
                    strMsg = objGlobal.GetErrorMessage("00001")
                    strMsg = "From Interface Code " & strMsg
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Return False
                ElseIf aryDoc.Item(i)(4) = "" Then
                    strMsg = objGlobal.GetErrorMessage("00001")
                    strMsg = "To Branch Code " & strMsg
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Return False
                ElseIf aryDoc.Item(i)(5) = "" Then
                    strMsg = objGlobal.GetErrorMessage("00001")
                    strMsg = "To BR GL Code " & strMsg
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Return False
                ElseIf aryDoc.Item(i)(6) = "" Then
                    strMsg = objGlobal.GetErrorMessage("00001")
                    strMsg = "To Cost Center " & strMsg
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Return False
                ElseIf aryDoc.Item(i)(7) = "" Then
                    strMsg = objGlobal.GetErrorMessage("00001")
                    strMsg = "To Interface Code " & strMsg
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Return False
                End If

                ''From and To accounts cannot be the same
                If aryDoc.Item(i)(0) = aryDoc.Item(i)(4) And aryDoc.Item(i)(1) = aryDoc.Item(i)(5) And aryDoc.Item(i)(2) = aryDoc.Item(i)(6) And aryDoc.Item(i)(3) = aryDoc.Item(i)(7) Then
                    Common.NetMsgbox(Me, "To and From cannot be of the same account.", MsgBoxStyle.Information)
                    Return False
                End If

                ''Jules 2014.05.07 - Cannot mix type of accounts. HO-HO, BR-BR only.
                Dim tempStr1 As String = aryDoc.Item(i)(0).ToString.ToUpper
                Dim subTempStr1 As String = tempStr1.Substring(0, 2)
                Dim tempStr2 As String = aryDoc.Item(i)(4).ToString.ToUpper
                Dim subTempStr2 As String = tempStr2.Substring(0, 2)

                If subTempStr1 <> subTempStr2 Then
                    Common.NetMsgbox(Me, subTempStr1 & " cannot be mapped to " & subTempStr2 & ".", MsgBoxStyle.Information)
                    Return False
                End If

                ''There must be an existing interface code for the 'From' account (Branch, Branch GL, Cost Center, Interface Code)
                ds = objBC.GetBR_GL_CC(aryDoc.Item(i)(0), aryDoc.Item(i)(1), aryDoc.Item(i)(2), aryDoc.Item(i)(3))
                If ds.Tables(0).Rows.Count = 0 Then
                    Common.NetMsgbox(Me, "Record not found in Interface Mapping.", MsgBoxStyle.Information)
                    Return False
                End If

                ''There must be an existing interface code for the ‘To’ account (Branch, Branch GL, Cost Center, Interface Code)
                ds = objBC.GetBR_GL_CC(aryDoc.Item(i)(4), aryDoc.Item(i)(5), aryDoc.Item(i)(6), aryDoc.Item(i)(7))
                If ds.Tables(0).Rows.Count = 0 Then
                    Common.NetMsgbox(Me, "Record not found in Interface Mapping.", MsgBoxStyle.Information)
                    Return False
                End If

                If Request.QueryString("action") <> "edit" Then
                    ''Check for duplicate From account
                    'ds = objBC.GetAccountMapping(aryDoc.Item(i)(0), aryDoc.Item(i)(1), aryDoc.Item(i)(2), aryDoc.Item(i)(3), "", "", "", "", "", "")
                    ds = objBC.GetAccountMapping(aryDoc.Item(i)(0), aryDoc.Item(i)(1), aryDoc.Item(i)(2), "", "", "", "", "", "", "")
                    If ds.Tables(0).Rows.Count > 0 Then
                        'strMsg = objGlobal.GetErrorMessage("00002")
                        Common.NetMsgbox(Me, "This From account is already mapped.", MsgBoxStyle.Information)
                        Return False
                    End If

                    ''Check for duplicate record
                    ds = objBC.GetAccountMapping(aryDoc.Item(i)(0), aryDoc.Item(i)(1), aryDoc.Item(i)(2), aryDoc.Item(i)(3), aryDoc.Item(i)(4), aryDoc.Item(i)(5), aryDoc.Item(i)(6), aryDoc.Item(i)(6), "", "")
                    If ds.Tables(0).Rows.Count > 0 Then
                        strMsg = objGlobal.GetErrorMessage("00002")
                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        Return False
                    End If

                End If
                iRow = iRow + 1

                If Request.QueryString("action") = "edit" Then
                    If i = 0 Then
                        Return True
                    End If
                End If
            Else
                If i = 0 Then
                    strMsg = objGlobal.GetErrorMessage("00001")
                    strMsg = "From Branch Code " & strMsg
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Return False
                End If
            End If
        Next

        If iRow > 0 Then
            Return True
        Else
            Return False
        End If
    End Function


    Private Sub btnhidden_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhidden.Click

    End Sub
End Class